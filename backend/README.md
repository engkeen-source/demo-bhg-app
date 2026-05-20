# BossSO Quotation API

Python FastAPI backend for the BossSO Quotation feature.

## Setup

```bash
cd backend
python -m venv .venv
source .venv/bin/activate   # Windows: .venv\Scripts\activate
pip install -r requirements.txt
```

## Configuration

Copy `.env.example` to `.env` and fill in your Supabase DB password:

```bash
cp .env.example .env
# Edit .env: replace [YOUR-PASSWORD] with the Supabase DB password
# Supabase dashboard → Project Settings → Database → Connection string (Transaction pooler)
```

## Run

```bash
uvicorn app.main:app --reload --port 8000
```

API docs: http://localhost:8000/docs

## Key endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | /customers?q= | Customer autocomplete (name/code search) |
| GET | /customers/{id} | Full customer record for auto-populate |
| GET | /items/search?q=&field= | Item lookup (item_id / description / both) |
| GET | /line-types | Structural line type picker (Header/Total/Sub Total/CF Total) |
| POST | /quotations/compute | Stateless total recompute (no DB write) |
| POST | /quotations | Create & save quotation (validates + recomputes) |
| PUT | /quotations/{id} | Update quotation |
| GET | /quotations/{id} | Load quotation |

## Validation errors

`POST /PUT /quotations` returns HTTP 422 with structured errors on save failure:

```json
{
  "errors": [
    { "line_no": 1, "field": "price", "message": "Line 1: a Header line cannot have a price." },
    { "line_no": 3, "field": "qty",   "message": "Line 3: a Charges line does not take a quantity." }
  ]
}
```
