"""
Quotation computation engine – ported from clsDocComUtility.cs CalForm.

Single source of truth for amount rollup. Also used by the frontend (calc.ts).
"""
from __future__ import annotations
from decimal import Decimal, ROUND_HALF_UP
from typing import Any

from app.enums import (
    GRP_STOCK, GRP_CHARGES, GRP_DISCOUNT,
    GRP_TOTAL, GRP_STRUCTURAL,
    LINE_TYPE_SUB_TOTAL, LINE_TYPE_TOTAL, LINE_TYPE_CF_TOTAL,
)

_ZERO = Decimal("0")
_CENT = Decimal("0.0001")  # 4 dp rounding for money


def _rnd(v: Decimal) -> Decimal:
    return v.quantize(_CENT, rounding=ROUND_HALF_UP)


def compute(
    lines: list[dict[str, Any]],
    currency_rate: Decimal = Decimal("1"),
    doc_discount_rate: Decimal = _ZERO,
    tax_rate: Decimal = Decimal("9"),
) -> dict[str, Any]:
    """
    Iterate lines (sorted by line_no) and compute per-line amounts + document totals.

    Returns a dict:
      {
        "lines": [ {line_no, amount, tax_amt, tax_amt_local, price}, ... ],
        "sub_total", "discount_amt", "total_after_dis",
        "tax_total", "grand_total",
        "home_sub_total", "home_tax_total", "home_total"
      }

    Mirrors CalForm logic (clsDocComUtility.cs ~550-1843):
      - Stock/Non_Stock/Service/Assembly/Charges/Discount: amount = row.amount (user-entered)
      - Sub Total row: amount = total_st (sum since last reset)
      - Total row: amount = total_previous + total_st
      - CF Total row: amount = total_cf (running cumulative)
      - Header/Remark: contribute 0 to det_total_amt
    """
    sorted_lines = sorted(lines, key=lambda r: r.get("line_no", 0))

    total_previous = _ZERO   # last Total row value
    total_st       = _ZERO   # running sub-total since last reset
    total_cf       = _ZERO   # cumulative carry-forward
    det_total_amt  = _ZERO   # feeds doc sub_total (excludes Header/Total rows)
    taxable_total  = _ZERO
    reset_total    = False

    computed_lines: list[dict[str, Any]] = []

    for row in sorted_lines:
        lt = row.get("line_type", "")
        price = Decimal(str(row.get("price", 0)))
        qty   = Decimal(str(row.get("qty", 0)))
        taxable = bool(row.get("taxable", True))
        row_tax_rate = Decimal(str(row.get("tax_rate", 0)))
        line_no = row.get("line_no", 0)

        amt = _ZERO
        tax_amt = _ZERO
        tax_amt_local = _ZERO

        if lt in GRP_STOCK:
            amt = _rnd(Decimal(str(row.get("amount", 0))))
            if taxable and row_tax_rate > 0:
                tax_amt = _rnd(amt * row_tax_rate / 100)
            tax_amt_local = _rnd(tax_amt * currency_rate)
            det_total_amt += amt
            if taxable:
                taxable_total += amt
            total_cf += amt
            if reset_total:
                total_st = amt
                reset_total = False
            else:
                total_st += amt

        elif lt in GRP_CHARGES:
            amt = _rnd(Decimal(str(row.get("amount", 0))))
            if taxable and row_tax_rate > 0:
                tax_amt = _rnd(amt * row_tax_rate / 100)
            tax_amt_local = _rnd(tax_amt * currency_rate)
            det_total_amt += amt
            if taxable:
                taxable_total += amt
            total_cf += amt
            if reset_total:
                total_st = amt
                reset_total = False
            else:
                total_st += amt

        elif lt in GRP_DISCOUNT:
            amt = _rnd(Decimal(str(row.get("amount", 0))))
            det_total_amt += amt
            total_cf += amt
            if reset_total:
                total_st = amt
                reset_total = False
            else:
                total_st += amt

        elif lt in GRP_TOTAL:
            # Total rollup rows (clsDocComUtility.cs:1507-1590)
            if lt == LINE_TYPE_SUB_TOTAL:
                amt = total_st
            elif lt == LINE_TYPE_CF_TOTAL:
                amt = total_cf
            else:  # Total
                amt = total_previous + total_st

            total_previous = amt
            reset_total = True
            # price/tax forced to zero for these rows
            price = _ZERO
            tax_amt = _ZERO
            tax_amt_local = _ZERO

        else:
            # Header / Remark (clsDocComUtility.cs:1595-1629)
            # Does not contribute to det_total_amt
            amt = _ZERO
            price = _ZERO
            tax_amt = _ZERO
            tax_amt_local = _ZERO

        computed_lines.append({
            "line_no": line_no,
            "price": float(price),
            "amount": float(amt),
            "tax_amt": float(tax_amt),
            "tax_amt_local": float(tax_amt_local),
        })

    # Document totals (clsDocComUtility.cs:1784-1843)
    sub_total    = _rnd(det_total_amt)
    discount_amt = _rnd(sub_total * doc_discount_rate / 100)
    after_dis    = sub_total - discount_amt
    taxable_after_dis = _rnd(taxable_total - _rnd(taxable_total * doc_discount_rate / 100))
    tax_total    = _rnd(taxable_after_dis * tax_rate / 100)
    grand_total  = after_dis + tax_total

    home_sub     = _rnd(sub_total  * currency_rate)
    home_tax     = _rnd(tax_total  * currency_rate)
    home_total   = _rnd(after_dis  * currency_rate) + home_tax

    return {
        "lines": computed_lines,
        "sub_total":       float(sub_total),
        "discount_amt":    float(discount_amt),
        "total_after_dis": float(after_dis),
        "tax_total":       float(tax_total),
        "grand_total":     float(grand_total),
        "home_sub_total":  float(home_sub),
        "home_tax_total":  float(home_tax),
        "home_total":      float(home_total),
    }
