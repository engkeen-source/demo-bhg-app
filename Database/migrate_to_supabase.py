"""
BossSSM → Supabase migration script.

Reads from SQL Server (Docker) and bulk-inserts into Supabase PostgreSQL.

Tables:  MST_Con → customer
         MST_Itm → item
         AR_QO   → quotation
         AR_QODetItm → quotation_line
"""

import sys
import pymssql
import psycopg2
import psycopg2.extras

# ── connection strings ────────────────────────────────────────────────────────
MSSQL_HOST   = "127.0.0.1"
MSSQL_PORT   = 1433
MSSQL_USER   = "SA"
MSSQL_PASS   = "Boss@Admin2026"
MSSQL_DB     = "BossSSM"

PG_DSN = (
    "postgresql://postgres.wvmjvigbysxwqahzbkps:o9RbpkXZF1dYWZTf"
    "@aws-1-ap-southeast-2.pooler.supabase.com:6543/postgres"
    "?sslmode=require"
)

BATCH = 500  # rows per INSERT


# ── item type mapping (clsGEnum.cs ItemType) ─────────────────────────────────
def itm_type(t):
    return {
        100: "Stock", 110: "Stock", 200: "Stock", 210: "Stock",
        250: "Assembly", 310: "Stock", 410: "Stock", 510: "Stock",
        600: "Non_Stock", 610: "Service",
        700: "Charges", 710: "Discount",
        800: "Header", 810: "Remark",
        820: "Sub Total", 825: "Total", 830: "CF Total",
    }.get(t, "Stock")


def quotation_status(s):
    return {10: "Pending", 20: "Won", 30: "Lost"}.get(s, "Pending")


def doc_state(s):
    return {100: "New"}.get(s, "New")


def safe(v):
    """Convert None to None (psycopg2 handles None → NULL)."""
    return v


def batched(seq, n):
    for i in range(0, len(seq), n):
        yield seq[i:i + n]


# ─────────────────────────────────────────────────────────────────────────────
def main():
    print("Connecting to SQL Server …")
    ms = pymssql.connect(
        server=MSSQL_HOST, port=MSSQL_PORT,
        user=MSSQL_USER, password=MSSQL_PASS,
        database=MSSQL_DB, charset="UTF-8",
        tds_version="7.4",
    )
    mc = ms.cursor(as_dict=True)

    print("Connecting to Supabase …")
    pg = psycopg2.connect(PG_DSN)
    pg.autocommit = False
    pc = pg.cursor()

    # ── 0. clear existing data ────────────────────────────────────────────────
    print("Clearing existing data …")
    pc.execute("TRUNCATE quotation_line, quotation, item, customer RESTART IDENTITY CASCADE")
    pg.commit()

    # ── 1. customers ──────────────────────────────────────────────────────────
    print("Fetching customers from MST_Con …")
    mc.execute("""
        SELECT
            c.ConID                 AS code,
            c.ConNm                 AS name,
            c.ConUEN                AS uen,
            em.EmNm                 AS representative,
            c.CDefaultContact       AS attention,
            acc.AccID               AS ar_account_code,
            acc.AccDes              AS ar_account_name,
            CAST(c.CPriceType AS VARCHAR) AS price_type,
            t.TermID                AS terms,
            c.CCurrID               AS currency,
            tg.TaxGrpID             AS tax_code,
            CASE tg.TaxGrpID
                WHEN 'GST3%' THEN 3
                WHEN 'GST4%' THEN 4
                WHEN 'GST5%' THEN 5
                WHEN 'GST7%' THEN 7
                WHEN 'GST8%' THEN 8
                WHEN 'GST'   THEN 9
                WHEN 'ZGST'  THEN 0
                WHEN 'EGST'  THEN 0
                ELSE 9
            END                     AS tax_rate,
            c.COverallDefaultDis    AS discount_rate,
            c.Inactive              AS inactive
        FROM MST_Con c
        LEFT JOIN MST_SalesRep  em  ON em.EmKey  = c.CEMKey
        LEFT JOIN REF_Term       t  ON t.TermKey  = c.CTermKey
        LEFT JOIN REF_TaxGrp     tg ON tg.TaxGrpKey = c.CTaxGrpKey
        LEFT JOIN MST_Acc        acc ON acc.AccKey = c.CAccKey
        WHERE c.ConType IN (1, 3)   -- customer or both (not vendor-only)
           OR c.CAccKey IS NOT NULL  -- has AR account
        ORDER BY c.ConID
    """)
    customers = mc.fetchall()
    print(f"  {len(customers)} customers")

    insert_customer = """
        INSERT INTO customer
            (code, name, uen, representative, attention,
             ar_account_code, ar_account_name, price_type, terms,
             currency, tax_code, tax_rate, discount_rate, inactive)
        VALUES %s
        ON CONFLICT (code) DO NOTHING
    """
    for batch in batched(customers, BATCH):
        rows = [(
            r["code"], r["name"], r["uen"],
            r["representative"], r["attention"],
            r["ar_account_code"], r["ar_account_name"],
            r["price_type"] if r["price_type"] not in ("0", "None", None) else None,
            r["terms"],
            r["currency"] or "SGD",
            r["tax_code"] or "GST",
            r["tax_rate"] or 9,
            float(r["discount_rate"] or 0),
            bool(r["inactive"]),
        ) for r in batch]
        psycopg2.extras.execute_values(pc, insert_customer, rows)
    pg.commit()
    print(f"  ✓ customers inserted")

    # ── 2. items ──────────────────────────────────────────────────────────────
    print("Fetching items from MST_Itm …")
    mc.execute("""
        SELECT
            i.ItmID                 AS code,
            i.ItmDes                AS description,
            i.ItmType               AS item_type_int,
            i.BUOMID                AS uom,
            i.SaleUOMRate           AS uom_con_rate,
            i.ControlPriceH         AS list_price,
            i.CostLatest            AS latest_cost,
            i.QtyStock              AS qty_stock,
            i.Taxable               AS taxable,
            i.INClass               AS class,
            i.CatID1                AS category,
            i.BrandID               AS brand,
            i.CountryID             AS country,
            i.Inactive              AS inactive
        FROM MST_Itm i
        WHERE i.ItmType <> 900   -- exclude Master parent items
        ORDER BY i.ItmID
    """)
    items = mc.fetchall()
    print(f"  {len(items)} items")

    insert_item = """
        INSERT INTO item
            (code, description, item_type, uom, uom_con_rate,
             list_price, latest_cost, qty_stock,
             taxable, tax_code, tax_rate,
             class, category, brand, country, inactive)
        VALUES %s
        ON CONFLICT (code) DO NOTHING
    """
    for batch in batched(items, BATCH):
        rows = [(
            r["code"],
            r["description"],
            itm_type(r["item_type_int"]),
            r["uom"],
            float(r["uom_con_rate"] or 1),
            float(r["list_price"] or 0),
            float(r["latest_cost"] or 0),
            float(r["qty_stock"] or 0),
            bool(r["taxable"]),
            "GST",
            9,
            r["class"],
            r["category"],
            r["brand"],
            r["country"],
            bool(r["inactive"]),
        ) for r in batch]
        psycopg2.extras.execute_values(pc, insert_item, rows)
    pg.commit()
    print(f"  ✓ items inserted")

    # ── 3. quotations ─────────────────────────────────────────────────────────
    print("Fetching quotations from AR_QO …")
    mc.execute("""
        SELECT
            q.DocKey                AS src_doc_key,
            q.DocID                 AS doc_id,
            CONVERT(date, q.DocDate) AS doc_date,
            q.DocState              AS doc_state_int,
            q.DocTypeNm             AS doc_type,
            c.ConID                 AS customer_code,
            q.DocConNm              AS customer_name,
            em.EmNm                 AS representative,
            q.DocBAddrAttn          AS attention,
            curr.CurrID             AS currency,
            CAST(q.DocCurrRate AS FLOAT) AS currency_rate,
            CAST(q.DocPriceType AS VARCHAR) AS price_type,
            t.TermID                AS terms,
            CONVERT(date, q.DocEnquiryDate) AS enquiry_date,
            CONVERT(date, q.DocDateValid)   AS valid_date,
            q.DocQuoteStatus        AS quote_status_int,
            q.DocQuoteReason        AS reason_for_loss,
            q.DocCustPONum          AS customer_po,
            q.DocRef                AS reference,
            q.DocRem                AS remarks,
            acc.AccID               AS ar_account_code,
            acc.AccDes              AS ar_account_name,
            q.DocRemAdditional1     AS request_remark,
            q.IsKPIPotentialPrj     AS potential_project,
            q.DocPrinted            AS printed,
            CAST(q.DocSubTotal AS FLOAT)        AS sub_total,
            CAST(q.DocOverallDisRate AS FLOAT)  AS discount_rate,
            CAST(q.DocOverallDisAmt AS FLOAT)   AS discount_amt,
            CAST(q.DocTotalAfterDis AS FLOAT)   AS total_after_dis,
            tg.TaxGrpID             AS tax_code,
            CAST(q.DocTaxGrpRate AS FLOAT)      AS tax_rate,
            CAST(q.DocTaxTotal AS FLOAT)        AS tax_total,
            CAST(q.DocGrand AS FLOAT)           AS grand_total,
            CAST(q.DocSubTotal AS FLOAT)        AS home_sub_total,
            CAST(q.DocTaxTotalLocal AS FLOAT)   AS home_tax_total,
            CAST(q.DocHome AS FLOAT)            AS home_total
        FROM AR_QO q
        LEFT JOIN MST_Con       c   ON c.ConKey   = q.DocConKey
        LEFT JOIN MST_SalesRep  em  ON em.EmKey   = q.DocEmKey
        LEFT JOIN REF_Curr      curr ON curr.CurrKey = q.DocCurrKey
        LEFT JOIN REF_Term      t   ON t.TermKey   = q.DocTermKey
        LEFT JOIN REF_TaxGrp    tg  ON tg.TaxGrpKey = q.DocTaxGrpKey
        LEFT JOIN MST_Acc       acc ON acc.AccKey  = q.DocAccKey
        ORDER BY q.DocKey
    """)
    quotations = mc.fetchall()
    print(f"  {len(quotations)} quotations")

    insert_quotation = """
        INSERT INTO quotation
            (doc_id, doc_date, doc_state, doc_type,
             customer_id, customer_code, customer_name,
             representative, attention,
             currency, currency_rate, price_type, terms,
             enquiry_date, valid_date,
             quotation_status, reason_for_loss,
             customer_po, reference, remarks,
             ar_account_code, ar_account_name, request_remark,
             potential_project, printed,
             sub_total, discount_rate, discount_amt, total_after_dis,
             tax_code, tax_rate, tax_total, grand_total,
             home_sub_total, home_tax_total, home_total)
        VALUES %s
        ON CONFLICT (doc_id) DO NOTHING
    """

    # We'll store (src_doc_key → pg_id) after insert for quotation_line FK
    doc_key_map = {}

    for batch in batched(quotations, BATCH):
        rows = []
        for r in batch:
            rows.append((
                r["doc_id"],
                r["doc_date"],
                doc_state(r["doc_state_int"]),
                r["doc_type"] or "Quotation",
                None,  # customer_id resolved via UPDATE below
                r["customer_code"],
                r["customer_name"],
                r["representative"],
                r["attention"],
                r["currency"] or "SGD",
                r["currency_rate"] or 1,
                r["price_type"] if r["price_type"] not in ("0", "None", None) else None,
                r["terms"],
                r["enquiry_date"],
                r["valid_date"],
                quotation_status(r["quote_status_int"]),
                r["reason_for_loss"],
                r["customer_po"],
                r["reference"],
                r["remarks"],
                r["ar_account_code"],
                r["ar_account_name"],
                r["request_remark"],
                bool(r["potential_project"]),
                bool(r["printed"]),
                r["sub_total"] or 0,
                r["discount_rate"] or 0,
                r["discount_amt"] or 0,
                r["total_after_dis"] or 0,
                r["tax_code"] or "GST",
                r["tax_rate"] or 9,
                r["tax_total"] or 0,
                r["grand_total"] or 0,
                r["home_sub_total"] or 0,
                r["home_tax_total"] or 0,
                r["home_total"] or 0,
            ))
        psycopg2.extras.execute_values(pc, insert_quotation, rows)

    pg.commit()

    # Fix customer_id FK via customer_code
    print("  Resolving customer_id FK …")
    pc.execute("""
        UPDATE quotation q
        SET customer_id = c.id
        FROM customer c
        WHERE q.customer_code = c.code
          AND q.customer_id IS NULL
    """)
    pg.commit()

    # Build src_doc_key → pg quotation.id map
    mc.execute("SELECT DocKey, DocID FROM AR_QO")
    docid_to_src = {r["DocID"]: r["DocKey"] for r in mc.fetchall()}
    pc.execute("SELECT id, doc_id FROM quotation")
    for pg_id, doc_id in pc.fetchall():
        src_key = docid_to_src.get(doc_id)
        if src_key:
            doc_key_map[src_key] = pg_id

    print(f"  ✓ quotations inserted, {len(doc_key_map)} doc_key mappings built")

    # Build item code → pg item.id map
    print("  Building item_id map …")
    mc.execute("SELECT ItmKey, ItmID FROM MST_Itm")
    itm_src = {r["ItmKey"]: r["ItmID"] for r in mc.fetchall()}
    pc.execute("SELECT id, code FROM item")
    item_code_map = {}
    for pg_id, code in pc.fetchall():
        item_code_map[code] = pg_id
    itm_key_map = {k: item_code_map.get(v) for k, v in itm_src.items()}

    # ── 4. quotation lines ────────────────────────────────────────────────────
    print("Fetching quotation lines from AR_QODetItm …")
    mc.execute("""
        SELECT
            d.DocKey                AS src_doc_key,
            ROW_NUMBER() OVER (PARTITION BY d.DocKey ORDER BY d.ItmSN, d.DocItmKey) AS line_no,
            d.ItmMark               AS marking,
            d.ItmKey                AS src_itm_key,
            i.ItmType               AS item_type_int,
            i.ItmID                 AS item_code,
            d.ItmDes                AS description,
            CAST(d.ItmQty AS FLOAT) AS qty,
            CAST(d.DSQty AS FLOAT)  AS direct_ship_qty,
            u.UOMID                 AS uom,
            CAST(d.ItmConRate AS FLOAT)         AS uom_con_rate,
            CAST(d.ItmPrice AS FLOAT)           AS price,
            CAST(d.ItmAmtF AS FLOAT)            AS amount,
            d.ItmTaxable                        AS taxable,
            tg.TaxGrpID                         AS tax_code,
            CAST(d.ItmTaxGrpRate AS FLOAT)      AS tax_rate,
            CAST(d.ItmTaxGrpAmtF AS FLOAT)      AS tax_amt,
            CAST(d.ItmTaxGrpAmtL AS FLOAT)      AS tax_amt_local,
            l.LocID                             AS location,
            CAST(d.ItmStock AS FLOAT)           AS stock,
            CAST(d.ItmLatestCostH AS FLOAT)     AS latest_cost,
            CAST(d.ObCost AS FLOAT)             AS obsolete_cost,
            d.ItmRem                            AS sales_remark
        FROM AR_QODetItm d
        LEFT JOIN MST_Itm   i  ON i.ItmKey   = d.ItmKey
        LEFT JOIN REF_UOM   u  ON u.UOMKey   = d.ItmUOMKey
        LEFT JOIN REF_TaxGrp tg ON tg.TaxGrpKey = d.ItmTaxGrpKey
        LEFT JOIN REF_Loc   l  ON l.LocKey   = d.ItmLocKey
        ORDER BY d.DocKey, d.ItmSN
    """)
    lines = mc.fetchall()
    print(f"  {len(lines)} quotation lines")

    insert_line = """
        INSERT INTO quotation_line
            (quotation_id, line_no, marking, line_type,
             item_id, item_code, description,
             qty, direct_ship_qty, uom, uom_con_rate,
             price, amount,
             taxable, tax_code, tax_rate, tax_amt, tax_amt_local,
             location, stock, latest_cost, obsolete_cost, sales_remark)
        VALUES %s
    """
    skipped = 0
    for batch in batched(lines, BATCH):
        rows = []
        for r in batch:
            qid = doc_key_map.get(r["src_doc_key"])
            if not qid:
                skipped += 1
                continue
            iid = itm_key_map.get(r["src_itm_key"])
            rows.append((
                qid,
                int(r["line_no"] or 0),
                r["marking"],
                itm_type(r["item_type_int"]) if r["item_type_int"] else "Stock",
                iid,
                r["item_code"],
                r["description"],
                r["qty"] or 0,
                r["direct_ship_qty"] or 0,
                r["uom"],
                r["uom_con_rate"] or 1,
                r["price"] or 0,
                r["amount"] or 0,
                bool(r["taxable"]),
                r["tax_code"] or "GST",
                r["tax_rate"] or 0,
                r["tax_amt"] or 0,
                r["tax_amt_local"] or 0,
                r["location"],
                r["stock"] or 0,
                r["latest_cost"] or 0,
                r["obsolete_cost"] or 0,
                r["sales_remark"],
            ))
        if rows:
            psycopg2.extras.execute_values(pc, insert_line, rows)
    pg.commit()
    print(f"  ✓ quotation_lines inserted (skipped {skipped} orphan lines)")

    # ── done ──────────────────────────────────────────────────────────────────
    pc.execute("""
        SELECT
            (SELECT COUNT(*) FROM customer) AS customers,
            (SELECT COUNT(*) FROM item) AS items,
            (SELECT COUNT(*) FROM quotation) AS quotations,
            (SELECT COUNT(*) FROM quotation_line) AS lines
    """)
    row = pc.fetchone()
    print(f"\n✓ Migration complete:")
    print(f"  customers:      {row[0]}")
    print(f"  items:          {row[1]}")
    print(f"  quotations:     {row[2]}")
    print(f"  quotation_lines:{row[3]}")

    pc.close(); pg.close()
    mc.close(); ms.close()


if __name__ == "__main__":
    main()
