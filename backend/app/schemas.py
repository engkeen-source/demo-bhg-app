from __future__ import annotations
from datetime import date
from typing import Any
from pydantic import BaseModel, ConfigDict


# ── Customer ─────────────────────────────────────────────────────────────────

class CustomerSummary(BaseModel):
    model_config = ConfigDict(from_attributes=True)
    id: int
    code: str
    name: str

class CustomerDetail(CustomerSummary):
    uen: str | None = None
    representative: str | None = None
    head_sales: str | None = None
    attention: str | None = None
    ar_account_code: str | None = None
    ar_account_name: str | None = None
    price_type: str | None = None
    terms: str | None = None
    currency: str = "SGD"
    tax_code: str = "GST"
    tax_rate: float = 9
    discount_rate: float = 0


# ── Item ─────────────────────────────────────────────────────────────────────

class ItemResult(BaseModel):
    model_config = ConfigDict(from_attributes=True)
    id: int
    code: str
    description: str
    item_type: str
    uom: str | None = None
    uom_con_rate: float = 1
    list_price: float = 0
    latest_cost: float = 0
    obsolete_cost: float = 0
    estore_price: float = 0
    qty_stock: float = 0
    taxable: bool = True
    tax_code: str = "GST"
    tax_rate: float = 9
    default_location: str = "Main"
    bin_location: str | None = None
    class_: str | None = None
    category: str | None = None
    brand: str | None = None
    country: str | None = None
    hs_code: str | None = None


class LineTypeRow(BaseModel):
    item_code: str
    description: str
    item_type: str


# ── Quotation Line ────────────────────────────────────────────────────────────

class QuotationLineIn(BaseModel):
    line_no: int
    marking: str | None = None
    line_type: str
    item_id: int | None = None
    item_code: str | None = None
    description: str | None = None
    qty: float = 0
    direct_ship_qty: float = 0
    uom: str | None = None
    uom_con_rate: float = 1
    project_cost: float = 0
    price: float = 0
    amount: float = 0
    taxable: bool = True
    tax_code: str | None = None
    tax_rate: float = 0
    tax_amt: float = 0
    tax_amt_local: float = 0
    job_id: str | None = None
    location: str | None = None
    stock: float = 0
    latest_cost: float = 0
    obsolete_cost: float = 0
    estore_price: float = 0
    customer_remark: str | None = None
    sales_remark: str | None = None


class QuotationLineOut(QuotationLineIn):
    model_config = ConfigDict(from_attributes=True)
    id: int


# ── Quotation Header ──────────────────────────────────────────────────────────

class QuotationIn(BaseModel):
    doc_id: str | None = None
    doc_date: date
    doc_state: str = "New"
    doc_type: str = "Quotation"
    doc_group: str | None = None
    customer_id: int | None = None
    customer_code: str | None = None
    customer_name: str | None = None
    representative: str | None = None
    head_sales: str | None = None
    attention: str | None = None
    currency: str = "SGD"
    currency_rate: float = 1
    price_type: str | None = None
    terms: str | None = None
    enquiry_date: date | None = None
    valid_date: date | None = None
    quotation_status: str = "Pending"
    reason_for_loss: str | None = None
    customer_po: str | None = None
    reference: str | None = None
    remarks: str | None = None
    ar_account_code: str | None = None
    ar_account_name: str | None = None
    discount_account: str | None = None
    request_remark: str | None = None
    potential_project: bool = False
    printed: bool = False
    discount_rate: float = 0
    tax_code: str | None = None
    tax_rate: float = 9
    lines: list[QuotationLineIn] = []


class QuotationOut(QuotationIn):
    model_config = ConfigDict(from_attributes=True)
    id: int
    sub_total: float = 0
    discount_amt: float = 0
    total_after_dis: float = 0
    tax_total: float = 0
    grand_total: float = 0
    home_sub_total: float = 0
    home_tax_total: float = 0
    home_total: float = 0
    lines: list[QuotationLineOut] = []


# ── Compute (stateless) ───────────────────────────────────────────────────────

class ComputeRequest(BaseModel):
    lines: list[QuotationLineIn]
    currency_rate: float = 1
    discount_rate: float = 0
    tax_rate: float = 9


class ValidationError(BaseModel):
    line_no: int
    field: str
    message: str


class SaveError(BaseModel):
    errors: list[ValidationError]
