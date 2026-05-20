"""
Line-item and header validation – ported from clsARQOFactory.cs DocDetItm_Validation.
Returns structured errors so the frontend can display specific messages per field.
"""
from __future__ import annotations
from typing import Any

from app.enums import (
    GRP_STOCK, GRP_CHARGES, GRP_DISCOUNT, GRP_TOTAL, GRP_STRUCTURAL,
    PRICE_FORBIDDEN_TYPES, QTY_FORBIDDEN_TYPES, ITEM_OPTIONAL_TYPES,
    LINE_TYPE_REMARK,
)


def _err(errors: list, line_no: int, field: str, message: str) -> None:
    errors.append({"line_no": line_no, "field": field, "message": message})


def validate_lines(lines: list[dict[str, Any]]) -> list[dict[str, Any]]:
    """
    Validate quotation line items.

    Returns a list of {line_no, field, message} dicts.
    Empty list = valid.
    """
    errors: list[dict[str, Any]] = []
    seen_line_nos: set[int] = set()

    for row in lines:
        lt    = row.get("line_type", "")
        n     = row.get("line_no", 0)
        price = float(row.get("price", 0) or 0)
        qty   = float(row.get("qty", 0) or 0)

        # Duplicate line number check (clsARQOFactory.cs:2311-2315)
        if n in seen_line_nos:
            _err(errors, n, "line_no", f"Duplicate line number {n}.")
        seen_line_nos.add(n)

        # line_type must be set
        if not lt:
            _err(errors, n, "line_type", f"Line {n}: line type is required.")
            continue

        # Price forbidden on structural/total types (clsARQOFactory.cs:2284-2293)
        if lt in PRICE_FORBIDDEN_TYPES and price != 0:
            type_label = lt if lt != "Header" else "Header"
            _err(errors, n, "price",
                 f"Line {n}: a {type_label} line cannot have a price.")

        # Qty forbidden on Charges / Total / Header / Remark (clsARQOFactory.cs:2223-2228)
        if lt in QTY_FORBIDDEN_TYPES and qty != 0:
            _err(errors, n, "qty",
                 f"Line {n}: a {lt} line does not take a quantity.")

        # Item required for non-structural types (clsARQOFactory.cs:2132-2141)
        if lt not in ITEM_OPTIONAL_TYPES and lt != LINE_TYPE_REMARK:
            item_id = row.get("item_id") or row.get("item_code")
            if not item_id:
                _err(errors, n, "item_code",
                     f"Line {n}: an item must be selected.")

        # Stock / Assembly / Non_Stock / Service – uom required; qty optional (amount entered directly)
        if lt in GRP_STOCK:
            if not row.get("uom"):
                _err(errors, n, "uom",
                     f"Line {n}: {lt} line requires a UOM.")

        # Description must not be blank for revenue lines
        if lt not in GRP_STRUCTURAL | GRP_TOTAL:
            if not (row.get("description") or "").strip():
                _err(errors, n, "description",
                     f"Line {n}: description is required.")

    return errors


def validate_header(header: dict[str, Any]) -> list[dict[str, Any]]:
    """Validate quotation header fields."""
    errors: list[dict[str, Any]] = []

    if not header.get("customer_id") and not header.get("customer_code"):
        errors.append({"line_no": 0, "field": "customer_id",
                        "message": "Customer is required."})

    if not header.get("doc_date"):
        errors.append({"line_no": 0, "field": "doc_date",
                        "message": "Document date is required."})

    return errors
