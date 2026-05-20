from sqlalchemy import BigInteger, Boolean, Date, ForeignKey, Integer, Numeric, Text
from sqlalchemy.orm import Mapped, mapped_column, relationship
from sqlalchemy.sql import func
from sqlalchemy.types import TIMESTAMP

from app.db import Base


class Customer(Base):
    __tablename__ = "customer"

    id: Mapped[int] = mapped_column(BigInteger, primary_key=True)
    code: Mapped[str] = mapped_column(Text, unique=True, nullable=False)
    name: Mapped[str] = mapped_column(Text, nullable=False)
    uen: Mapped[str | None] = mapped_column(Text)
    representative: Mapped[str | None] = mapped_column(Text)
    head_sales: Mapped[str | None] = mapped_column(Text)
    attention: Mapped[str | None] = mapped_column(Text)
    ar_account_code: Mapped[str | None] = mapped_column(Text)
    ar_account_name: Mapped[str | None] = mapped_column(Text)
    price_type: Mapped[str | None] = mapped_column(Text)
    terms: Mapped[str | None] = mapped_column(Text)
    currency: Mapped[str] = mapped_column(Text, nullable=False, default="SGD")
    tax_code: Mapped[str] = mapped_column(Text, nullable=False, default="GST")
    tax_rate: Mapped[float] = mapped_column(Numeric(9, 4), nullable=False, default=9)
    discount_rate: Mapped[float] = mapped_column(Numeric(9, 4), nullable=False, default=0)
    inactive: Mapped[bool] = mapped_column(Boolean, nullable=False, default=False)
    created_at: Mapped[object] = mapped_column(TIMESTAMP(timezone=True), server_default=func.now())


class Item(Base):
    __tablename__ = "item"

    id: Mapped[int] = mapped_column(BigInteger, primary_key=True)
    code: Mapped[str] = mapped_column(Text, unique=True, nullable=False)
    description: Mapped[str] = mapped_column(Text, nullable=False)
    item_type: Mapped[str] = mapped_column(Text, nullable=False)
    uom: Mapped[str | None] = mapped_column(Text)
    uom_con_rate: Mapped[float] = mapped_column(Numeric(18, 8), nullable=False, default=1)
    list_price: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    latest_cost: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    obsolete_cost: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    estore_price: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    qty_stock: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    taxable: Mapped[bool] = mapped_column(Boolean, nullable=False, default=True)
    tax_code: Mapped[str] = mapped_column(Text, nullable=False, default="GST")
    tax_rate: Mapped[float] = mapped_column(Numeric(9, 4), nullable=False, default=9)
    default_location: Mapped[str] = mapped_column(Text, nullable=False, default="Main")
    bin_location: Mapped[str | None] = mapped_column(Text)
    class_: Mapped[str | None] = mapped_column("class", Text)
    category: Mapped[str | None] = mapped_column(Text)
    brand: Mapped[str | None] = mapped_column(Text)
    country: Mapped[str | None] = mapped_column(Text)
    hs_code: Mapped[str | None] = mapped_column(Text)
    inactive: Mapped[bool] = mapped_column(Boolean, nullable=False, default=False)


class Quotation(Base):
    __tablename__ = "quotation"

    id: Mapped[int] = mapped_column(BigInteger, primary_key=True)
    doc_id: Mapped[str | None] = mapped_column(Text, unique=True)
    doc_date: Mapped[object] = mapped_column(Date, nullable=False)
    doc_state: Mapped[str] = mapped_column(Text, nullable=False, default="New")
    doc_type: Mapped[str] = mapped_column(Text, nullable=False, default="Quotation")
    doc_group: Mapped[str | None] = mapped_column(Text)
    customer_id: Mapped[int | None] = mapped_column(BigInteger, ForeignKey("customer.id"))
    customer_code: Mapped[str | None] = mapped_column(Text)
    customer_name: Mapped[str | None] = mapped_column(Text)
    representative: Mapped[str | None] = mapped_column(Text)
    head_sales: Mapped[str | None] = mapped_column(Text)
    attention: Mapped[str | None] = mapped_column(Text)
    currency: Mapped[str] = mapped_column(Text, nullable=False, default="SGD")
    currency_rate: Mapped[float] = mapped_column(Numeric(18, 8), nullable=False, default=1)
    price_type: Mapped[str | None] = mapped_column(Text)
    terms: Mapped[str | None] = mapped_column(Text)
    enquiry_date: Mapped[object | None] = mapped_column(Date)
    valid_date: Mapped[object | None] = mapped_column(Date)
    quotation_status: Mapped[str] = mapped_column(Text, nullable=False, default="Pending")
    reason_for_loss: Mapped[str | None] = mapped_column(Text)
    customer_po: Mapped[str | None] = mapped_column(Text)
    reference: Mapped[str | None] = mapped_column(Text)
    remarks: Mapped[str | None] = mapped_column(Text)
    ar_account_code: Mapped[str | None] = mapped_column(Text)
    ar_account_name: Mapped[str | None] = mapped_column(Text)
    discount_account: Mapped[str | None] = mapped_column(Text)
    request_remark: Mapped[str | None] = mapped_column(Text)
    potential_project: Mapped[bool] = mapped_column(Boolean, nullable=False, default=False)
    printed: Mapped[bool] = mapped_column(Boolean, nullable=False, default=False)
    sub_total: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    discount_rate: Mapped[float] = mapped_column(Numeric(9, 4), nullable=False, default=0)
    discount_amt: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    total_after_dis: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    tax_code: Mapped[str | None] = mapped_column(Text)
    tax_rate: Mapped[float] = mapped_column(Numeric(9, 4), nullable=False, default=9)
    tax_total: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    grand_total: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    home_sub_total: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    home_tax_total: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    home_total: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    created_at: Mapped[object] = mapped_column(TIMESTAMP(timezone=True), server_default=func.now())
    updated_at: Mapped[object] = mapped_column(TIMESTAMP(timezone=True), server_default=func.now(), onupdate=func.now())

    lines: Mapped[list["QuotationLine"]] = relationship(
        "QuotationLine", back_populates="quotation",
        cascade="all, delete-orphan", order_by="QuotationLine.line_no"
    )
    customer: Mapped[Customer | None] = relationship("Customer")


class QuotationLine(Base):
    __tablename__ = "quotation_line"

    id: Mapped[int] = mapped_column(BigInteger, primary_key=True)
    quotation_id: Mapped[int] = mapped_column(BigInteger, ForeignKey("quotation.id", ondelete="CASCADE"), nullable=False)
    line_no: Mapped[int] = mapped_column(Integer, nullable=False)
    marking: Mapped[str | None] = mapped_column(Text)
    line_type: Mapped[str] = mapped_column(Text, nullable=False)
    item_id: Mapped[int | None] = mapped_column(BigInteger, ForeignKey("item.id"))
    item_code: Mapped[str | None] = mapped_column(Text)
    description: Mapped[str | None] = mapped_column(Text)
    qty: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    direct_ship_qty: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    uom: Mapped[str | None] = mapped_column(Text)
    uom_con_rate: Mapped[float] = mapped_column(Numeric(18, 8), nullable=False, default=1)
    project_cost: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    price: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    amount: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    taxable: Mapped[bool] = mapped_column(Boolean, nullable=False, default=True)
    tax_code: Mapped[str | None] = mapped_column(Text)
    tax_rate: Mapped[float] = mapped_column(Numeric(9, 4), nullable=False, default=0)
    tax_amt: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    tax_amt_local: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    job_id: Mapped[str | None] = mapped_column(Text)
    location: Mapped[str | None] = mapped_column(Text)
    stock: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    latest_cost: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    obsolete_cost: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    estore_price: Mapped[float] = mapped_column(Numeric(19, 4), nullable=False, default=0)
    customer_remark: Mapped[str | None] = mapped_column(Text)
    sales_remark: Mapped[str | None] = mapped_column(Text)

    quotation: Mapped["Quotation"] = relationship("Quotation", back_populates="lines")
    item: Mapped[Item | None] = relationship("Item")
