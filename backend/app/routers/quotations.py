from decimal import Decimal
from fastapi import APIRouter, Depends, HTTPException, Response
from sqlalchemy import select
from sqlalchemy.orm import selectinload
from sqlalchemy.ext.asyncio import AsyncSession

from app.db import get_db
from app.models import Quotation, QuotationLine
from app.schemas import (
    ComputeRequest, QuotationIn, QuotationOut, SaveError,
)
from app.calc import compute
from app.validation import validate_lines, validate_header

router = APIRouter(prefix="/quotations", tags=["quotations"])


def _apply_compute(q: Quotation, result: dict) -> None:
    """Write computed totals back onto the ORM object."""
    q.sub_total       = result["sub_total"]
    q.discount_amt    = result["discount_amt"]
    q.total_after_dis = result["total_after_dis"]
    q.tax_total       = result["tax_total"]
    q.grand_total     = result["grand_total"]
    q.home_sub_total  = result["home_sub_total"]
    q.home_tax_total  = result["home_tax_total"]
    q.home_total      = result["home_total"]


def _line_to_dict(line: "QuotationIn.lines[0]") -> dict:  # type: ignore[name-defined]
    return line.model_dump()


@router.post("/compute")
async def compute_quotation(req: ComputeRequest):
    """
    Stateless endpoint – returns per-line computed amounts + doc totals.
    Frontend calls this on every line change for authoritative confirmation
    (but also computes locally via calc.ts for instant feedback).
    """
    lines = [l.model_dump() for l in req.lines]
    return compute(
        lines=lines,
        currency_rate=Decimal(str(req.currency_rate)),
        doc_discount_rate=Decimal(str(req.discount_rate)),
        tax_rate=Decimal(str(req.tax_rate)),
    )


@router.post("", response_model=QuotationOut, status_code=201)
async def create_quotation(body: QuotationIn, db: AsyncSession = Depends(get_db)):
    lines_data = [l.model_dump() for l in body.lines]

    # Validate header + lines
    errors = validate_header(body.model_dump()) + validate_lines(lines_data)
    if errors:
        raise HTTPException(status_code=422, detail={"errors": errors})

    # Authoritative recompute (RunCheck parity with C# CalForm)
    computed = compute(
        lines=lines_data,
        currency_rate=Decimal(str(body.currency_rate)),
        doc_discount_rate=Decimal(str(body.discount_rate)),
        tax_rate=Decimal(str(body.tax_rate)),
    )

    # Persist
    q = Quotation(**{
        k: v for k, v in body.model_dump(exclude={"lines"}).items()
    })
    _apply_compute(q, computed)

    line_computed = {r["line_no"]: r for r in computed["lines"]}
    for ldata in lines_data:
        lc = line_computed.get(ldata["line_no"], {})
        line = QuotationLine(**{
            **ldata,
            "amount":       lc.get("amount",       ldata.get("amount", 0)),
            "tax_amt":      lc.get("tax_amt",       ldata.get("tax_amt", 0)),
            "tax_amt_local": lc.get("tax_amt_local", ldata.get("tax_amt_local", 0)),
            "price":        lc.get("price",         ldata.get("price", 0)),
        })
        q.lines.append(line)

    db.add(q)
    await db.commit()
    await db.refresh(q)
    await db.execute(
        select(Quotation)
        .options(selectinload(Quotation.lines))
        .where(Quotation.id == q.id)
    )
    return await _load_quotation(q.id, db)


@router.put("/{quotation_id}", response_model=QuotationOut)
async def update_quotation(
    quotation_id: int, body: QuotationIn, db: AsyncSession = Depends(get_db)
):
    q = await _load_quotation(quotation_id, db)
    if not q:
        raise HTTPException(status_code=404, detail="Quotation not found")

    lines_data = [l.model_dump() for l in body.lines]
    errors = validate_header(body.model_dump()) + validate_lines(lines_data)
    if errors:
        raise HTTPException(status_code=422, detail={"errors": errors})

    computed = compute(
        lines=lines_data,
        currency_rate=Decimal(str(body.currency_rate)),
        doc_discount_rate=Decimal(str(body.discount_rate)),
        tax_rate=Decimal(str(body.tax_rate)),
    )

    # Update header fields
    for k, v in body.model_dump(exclude={"lines"}).items():
        setattr(q, k, v)
    _apply_compute(q, computed)

    # Replace lines — flush orphan DELETEs before INSERTs to avoid
    # unique (quotation_id, line_no) collision within one flush cycle.
    q.lines.clear()
    await db.flush()
    line_computed = {r["line_no"]: r for r in computed["lines"]}
    for ldata in lines_data:
        lc = line_computed.get(ldata["line_no"], {})
        line = QuotationLine(**{
            **ldata,
            "quotation_id": q.id,
            "amount":       lc.get("amount",       ldata.get("amount", 0)),
            "tax_amt":      lc.get("tax_amt",       ldata.get("tax_amt", 0)),
            "tax_amt_local": lc.get("tax_amt_local", ldata.get("tax_amt_local", 0)),
            "price":        lc.get("price",         ldata.get("price", 0)),
        })
        q.lines.append(line)

    await db.commit()
    return await _load_quotation(q.id, db)


@router.get("/{quotation_id}", response_model=QuotationOut)
async def get_quotation(quotation_id: int, db: AsyncSession = Depends(get_db)):
    q = await _load_quotation(quotation_id, db)
    if not q:
        raise HTTPException(status_code=404, detail="Quotation not found")
    return q


@router.get("", response_model=list[QuotationOut])
async def list_quotations(db: AsyncSession = Depends(get_db)):
    result = await db.execute(
        select(Quotation)
        .options(selectinload(Quotation.lines))
        .order_by(Quotation.id.desc())
        .limit(100)
    )
    return result.scalars().all()


async def _load_quotation(quotation_id: int, db: AsyncSession) -> Quotation | None:
    result = await db.execute(
        select(Quotation)
        .options(selectinload(Quotation.lines))
        .where(Quotation.id == quotation_id)
    )
    return result.scalar_one_or_none()
