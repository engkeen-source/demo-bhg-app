from fastapi import APIRouter, Depends, HTTPException, Query
from sqlalchemy import select, or_, func
from sqlalchemy.ext.asyncio import AsyncSession

from app.db import get_db
from app.models import Customer
from app.schemas import CustomerDetail, CustomerSummary

router = APIRouter(prefix="/customers", tags=["customers"])


@router.get("", response_model=list[CustomerSummary])
async def search_customers(
    q: str = Query(default="", description="Search by name or code"),
    db: AsyncSession = Depends(get_db),
):
    """Autocomplete search – returns id/code/name for the dropdown."""
    stmt = (
        select(Customer)
        .where(Customer.inactive.is_(False))
        .order_by(Customer.code)
        .limit(20)
    )
    if q.strip():
        pattern = f"%{q.strip()}%"
        stmt = stmt.where(
            or_(
                func.lower(Customer.name).like(func.lower(pattern)),
                func.lower(Customer.code).like(func.lower(pattern)),
            )
        )
    result = await db.execute(stmt)
    return result.scalars().all()


@router.get("/{customer_id}", response_model=CustomerDetail)
async def get_customer(customer_id: int, db: AsyncSession = Depends(get_db)):
    """Full customer record for auto-populate on selection."""
    row = await db.get(Customer, customer_id)
    if not row:
        raise HTTPException(status_code=404, detail="Customer not found")
    return row
