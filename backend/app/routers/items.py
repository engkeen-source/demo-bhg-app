from fastapi import APIRouter, Depends, Query
from sqlalchemy import select, or_, func
from sqlalchemy.ext.asyncio import AsyncSession

from app.db import get_db
from app.models import Item
from app.schemas import ItemResult, LineTypeRow
from app.enums import LINE_TYPE_PICKER_ROWS

router = APIRouter(tags=["items"])


@router.get("/items/search", response_model=list[ItemResult])
async def search_items(
    q: str = Query(default="", description="Search text"),
    field: str = Query(default="both", description="item_id | description | both"),
    db: AsyncSession = Depends(get_db),
):
    """
    Item lookup used by the double-click / search in the Item grid.
    field=item_id   → match against item.code
    field=description → match against item.description
    field=both (default) → match either
    """
    stmt = (
        select(Item)
        .where(Item.inactive.is_(False))
        .order_by(Item.code)
        .limit(50)
    )
    if q.strip():
        pattern = f"%{q.strip()}%"
        if field == "item_id":
            stmt = stmt.where(func.lower(Item.code).like(func.lower(pattern)))
        elif field == "description":
            stmt = stmt.where(func.lower(Item.description).like(func.lower(pattern)))
        else:
            stmt = stmt.where(
                or_(
                    func.lower(Item.code).like(func.lower(pattern)),
                    func.lower(Item.description).like(func.lower(pattern)),
                )
            )
    result = await db.execute(stmt)
    return result.scalars().all()


@router.get("/line-types", response_model=list[LineTypeRow])
async def get_line_types():
    """
    Returns the structural line-type picker rows (Sub Total / Total / CF Total /
    Header / Remark) — mirrors the 'Stock Details' popup for totals.
    These are NOT real items; they are returned from a static list to keep the
    item table clean.
    """
    return LINE_TYPE_PICKER_ROWS
