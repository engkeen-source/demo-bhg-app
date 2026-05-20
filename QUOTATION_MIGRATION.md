# Quotation (frmARQO) — Migration Status & Roadmap

## Overview

This document tracks the port of the BossSO desktop **Quotation** module (`frmARQO`) to
the modern web stack. The source of truth is the original WinForms `.NET 4.8` codebase.
Key C# source files ported:

| C# source | Purpose |
|-----------|---------|
| `BOLib/DocUtility/clsDocComUtility.cs` | `CalForm` — per-line amount + Total/SubTotal/CF rollup |
| `BOLib/Factory Classes/AR/clsARQOFactory.cs` | `DocDetItm_Validation` — line-level save validation |
| `WinUI/GlobalAndUtilityClasses/clsDocHDRUtility.cs` | `DocConID_DependentSet` — customer field auto-populate |
| `BOLib/Global Classes/clsGEnum.cs` | `ItemType` / `INTypeGrp` enums |
| `DBScripts/CreateTablesScripts.sql` | Schema reference (`AR_QO`, `AR_QODetItm`) |

---

## Architecture

### Backend — `/backend/`

| Component | Details |
|-----------|---------|
| Framework | FastAPI + Uvicorn (Python 3.12) |
| ORM | SQLAlchemy 2.x async + asyncpg |
| Database | Supabase Postgres (project `wvmjvigbysxwqahzbkps`) |
| Connection | Transaction pooler port 6543 (`aws-1-ap-southeast-2`); `statement_cache_size=0` for PgBouncer safety |
| Config | `backend/.env` — `DATABASE_URL` must use the pooler string from your Supabase dashboard |

Key backend modules:

| File | Role |
|------|------|
| `app/calc.py` | Port of `CalForm` — computes per-line `amount`/`tax_amt` + doc totals |
| `app/validation.py` | Port of `DocDetItm_Validation` — returns `{line_no, field, message}` errors |
| `app/enums.py` | `LineType`, `PRICE_FORBIDDEN`, `QTY_FORBIDDEN`, `ITEM_OPTIONAL` sets |
| `app/routers/customers.py` | `GET /customers?q=` (empty → all), `GET /customers/{id}` |
| `app/routers/items.py` | `GET /items/search?q=&field=`, `GET /line-types` |
| `app/routers/quotations.py` | `POST /quotations`, `PUT /quotations/{id}`, `GET /quotations/{id}`, `GET /quotations`, `POST /quotations/compute` |

### Database — Supabase

| Table | Mirrors |
|-------|---------|
| `customer` | `MST_Con` (customer fields) |
| `item` | `MST_Itm` |
| `quotation` | `AR_QO` (header) |
| `quotation_line` | `AR_QODetItm` (detail) |

Seeded with ~6 customers (incl. ARCO ILLUMINATION, ATHENA, BENG HUI) and ~15 items
(CHARGES, several stock codes, Sub Total/Total/CF Total structural types).

### Frontend — `Web/`

| Component | Path | Role |
|-----------|------|------|
| Quotation page | `src/app/(shell)/app/transactions/sales/quotation/page.tsx` | Controlled state, customer autocomplete, totals, save |
| Item grid | `src/components/quotation/ItemGrid.tsx` | Type-driven rows, edit-lock, drag-reorder, Enter-lookup |
| Item lookup modal | `src/components/quotation/ItemLookupModal.tsx` | "Stock Details" picker (item or line-type mode) |
| Document picker | `src/components/quotation/DocumentPickerModal.tsx` | "Insert Data Above From Other" |
| Frontend calc | `src/components/quotation/calc.ts` | Port of `CalForm` for live totals without a server round-trip |
| API client | `src/lib/api.ts` | Thin `fetch` wrapper to `NEXT_PUBLIC_API_BASE_URL` |

---

## What's Done

### Phase 1 — Backend + DB
- [x] FastAPI app with CORS, async SQLAlchemy, Pydantic v2
- [x] 4 Supabase tables created + seeded
- [x] `calc.py`: `CalForm` rollup — Header lines excluded from Sub Total; Total row = previous+running; CF Total = cumulative
- [x] `validation.py`: field-level errors (Header cannot have price; Charges has no qty; duplicate SN; etc.)
- [x] All CRUD + compute endpoints
- [x] PgBouncer transaction-pooler hardening (`statement_cache_size=0`, unique prepared-statement names)

### Phase 2 — Frontend wire-up
- [x] Controlled Quotation page with `useReducer`-style state
- [x] Customer ID field: click → show all customers; type → filter; select → auto-populate 15+ header fields
- [x] Item tab: type-driven line grid (Header/Charges/Stock/Assembly/Sub Total/Total/CF Total)
- [x] Per-row edit-lock: rows read-only until pencil → Save (Ctrl+S) or Cancel (Esc)
- [x] Drag-to-reorder rows (native HTML5 drag)
- [x] Item ID Enter-to-lookup (exact → auto-fill; multiple → picker; 0 → empty picker)
- [x] Double-click Item ID / Description → Stock Details picker
- [x] Structural line type picker (Sub Total / Total / CF Total / Header / Remark)
- [x] Insert Data Above From Other (DocumentPickerModal → picks saved quotation → inserts lines)
- [x] Footer: Resequence Mark, Order By Marking
- [x] Live frontend CalForm rollup (`calc.ts`) — instant Sub Total/Tax/Grand Total feedback
- [x] Save: 422 errors mapped to per-line / header inline messages (not generic)
- [x] Makefile: `make install`, `make dev` for one-command start

---

## How to Run

### Prerequisites
- Node.js 20+, Python 3.12+
- Supabase project with the tables migrated (already applied to `wvmjvigbysxwqahzbkps`)

### Setup
```bash
# From repo root
make install         # creates Python venv, npm install
```

Edit `backend/.env`:
```
# Copy from Supabase → Project Settings → Database → Transaction pooler connection string
DATABASE_URL=postgresql://postgres.wvmjvigbysxwqahzbkps:<YOUR-PASSWORD>@aws-1-ap-southeast-2.pooler.supabase.com:6543/postgres
CORS_ORIGINS=["http://localhost:3000"]
```
> The host segment (`aws-1-ap-southeast-2`) must match your project's region exactly.
> Copy the full string from the dashboard — do not guess.

### Run
```bash
make dev             # backend :8000 + frontend :3000 in parallel
```

- Backend docs: http://localhost:8000/docs
- Frontend: http://localhost:3000/app/transactions/sales/quotation

---

## Roadmap / Next Steps

### High priority
- [ ] Document number generation (`doc_id` — year/sequence auto-assign on first save)
- [ ] Sales Order tab (`/app/transactions/sales/sales-order`) — same backend pattern, `AR_SO` tables
- [ ] Delivery Order / Sales Invoice / Payment Received — wire the same way

### Medium
- [ ] Term & Condition tab — free-text terms entry
- [ ] Address tab — delivery address + billing address
- [ ] Other Information tab — additional doc metadata fields
- [ ] AR Aging Status tab — read-only AR ledger view for the customer
- [ ] Quotation list/search page (grid of all quotations, open/duplicate/delete)

### Lower
- [ ] Print / PDF export (map to the WinForms crystal report layout)
- [ ] Auth wiring (currently the app shell has a session guard stub — connect to real auth)
- [ ] Masters: Customer / Inventory Item / Account record editing (currently skin-only)
- [ ] Multi-currency exchange rate lookup
- [ ] Approval workflow (Pending → Approved → Won/Lost)
