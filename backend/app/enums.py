"""
Line type constants – ported from clsGEnum.cs (GEnum.ItemType + INTypeGrp).
"""

# Display names used in the grid Type column (and stored in quotation_line.line_type)
LINE_TYPE_HEADER    = "Header"
LINE_TYPE_CHARGES   = "Charges"
LINE_TYPE_ASSEMBLY  = "Assembly"
LINE_TYPE_STOCK     = "Stock"
LINE_TYPE_NON_STOCK = "Non_Stock"
LINE_TYPE_SERVICE   = "Service"
LINE_TYPE_DISCOUNT  = "Discount"
LINE_TYPE_REMARK    = "Remark"
LINE_TYPE_SUB_TOTAL = "Sub Total"
LINE_TYPE_TOTAL     = "Total"
LINE_TYPE_CF_TOTAL  = "CF Total"

# INTypeGrp groupings (clsGEnum.cs:419) used by calc engine
GRP_STOCK      = frozenset({LINE_TYPE_STOCK, LINE_TYPE_NON_STOCK, LINE_TYPE_SERVICE, LINE_TYPE_ASSEMBLY})
GRP_CHARGES    = frozenset({LINE_TYPE_CHARGES})
GRP_DISCOUNT   = frozenset({LINE_TYPE_DISCOUNT})
GRP_TOTAL      = frozenset({LINE_TYPE_SUB_TOTAL, LINE_TYPE_TOTAL, LINE_TYPE_CF_TOTAL})
GRP_STRUCTURAL = frozenset({LINE_TYPE_HEADER, LINE_TYPE_REMARK})  # zero contribution to det_total_amt

# Types that may NOT have a price entered (enforced in validation + UI)
PRICE_FORBIDDEN_TYPES = GRP_TOTAL | GRP_STRUCTURAL

# Types that may NOT have a qty entered
QTY_FORBIDDEN_TYPES = GRP_CHARGES | GRP_TOTAL | GRP_STRUCTURAL

# Types that don't require an item_id (structural / total rows)
ITEM_OPTIONAL_TYPES = GRP_TOTAL | GRP_STRUCTURAL

# Picker rows returned by /line-types (mirrors quotation-item-enterTotal.jpg)
LINE_TYPE_PICKER_ROWS = [
    {"item_code": "Sub Total",  "description": "Sub Total",  "item_type": LINE_TYPE_SUB_TOTAL},
    {"item_code": "Sub Total1", "description": "Sub Total",  "item_type": LINE_TYPE_SUB_TOTAL},
    {"item_code": "To",         "description": "Total",      "item_type": LINE_TYPE_TOTAL},
    {"item_code": "CF Total",   "description": "CF Total",   "item_type": LINE_TYPE_CF_TOTAL},
    {"item_code": "H",          "description": "Enter Heading", "item_type": LINE_TYPE_HEADER},
    {"item_code": "Remark",     "description": "Remark",     "item_type": LINE_TYPE_REMARK},
]
