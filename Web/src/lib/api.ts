'use client'

const BASE = process.env.NEXT_PUBLIC_API_BASE_URL ?? 'http://localhost:8000'

async function apiFetch<T>(path: string, init?: RequestInit): Promise<T> {
  const res = await fetch(`${BASE}${path}`, {
    headers: { 'Content-Type': 'application/json' },
    ...init,
  })
  if (!res.ok) {
    const body = await res.json().catch(() => ({}))
    const err: { status: number; detail?: unknown } = { status: res.status, detail: body?.detail }
    throw err
  }
  return res.json()
}

// ── Customer ─────────────────────────────────────────────────────────────────

export interface CustomerSummary { id: number; code: string; name: string }
export interface CustomerDetail extends CustomerSummary {
  uen?: string
  representative?: string
  head_sales?: string
  attention?: string
  ar_account_code?: string
  ar_account_name?: string
  price_type?: string
  terms?: string
  currency: string
  tax_code: string
  tax_rate: number
  discount_rate: number
}

export function searchCustomers(q: string): Promise<CustomerSummary[]> {
  return apiFetch(`/customers?q=${encodeURIComponent(q)}`)
}

export function getCustomer(id: number): Promise<CustomerDetail> {
  return apiFetch(`/customers/${id}`)
}

// ── Items ─────────────────────────────────────────────────────────────────────

export interface ItemResult {
  id: number
  code: string
  description: string
  item_type: string
  uom?: string
  uom_con_rate: number
  list_price: number
  latest_cost: number
  obsolete_cost: number
  estore_price: number
  qty_stock: number
  taxable: boolean
  tax_code: string
  tax_rate: number
  default_location: string
  bin_location?: string
  class_?: string
  category?: string
  brand?: string
  country?: string
  hs_code?: string
}

export interface LineTypeRow { item_code: string; description: string; item_type: string }

export function searchItems(q: string, field: 'item_id' | 'description' | 'both' = 'both'): Promise<ItemResult[]> {
  return apiFetch(`/items/search?q=${encodeURIComponent(q)}&field=${field}`)
}

export function getLineTypes(): Promise<LineTypeRow[]> {
  return apiFetch('/line-types')
}

// ── Quotation ─────────────────────────────────────────────────────────────────

export interface QuotationLine {
  id?: number
  line_no: number
  marking?: string
  line_type: string
  item_id?: number
  item_code?: string
  description?: string
  qty: number
  direct_ship_qty: number
  uom?: string
  uom_con_rate: number
  project_cost: number
  price: number
  amount: number
  taxable: boolean
  tax_code?: string
  tax_rate: number
  tax_amt: number
  tax_amt_local: number
  job_id?: string
  location?: string
  stock: number
  latest_cost: number
  obsolete_cost: number
  estore_price: number
  customer_remark?: string
  sales_remark?: string
}

export interface QuotationHeader {
  id?: number
  doc_id?: string
  doc_date: string
  doc_state: string
  doc_type: string
  doc_group?: string
  customer_id?: number
  customer_code?: string
  customer_name?: string
  representative?: string
  head_sales?: string
  attention?: string
  currency: string
  currency_rate: number
  price_type?: string
  terms?: string
  enquiry_date?: string
  valid_date?: string
  quotation_status: string
  reason_for_loss?: string
  customer_po?: string
  reference?: string
  remarks?: string
  ar_account_code?: string
  ar_account_name?: string
  discount_account?: string
  request_remark?: string
  potential_project: boolean
  printed: boolean
  discount_rate: number
  tax_code?: string
  tax_rate: number
  // computed totals (read-only from server)
  sub_total?: number
  discount_amt?: number
  total_after_dis?: number
  tax_total?: number
  grand_total?: number
  home_sub_total?: number
  home_tax_total?: number
  home_total?: number
}

export interface ComputeResult {
  lines: Array<{ line_no: number; price: number; amount: number; tax_amt: number; tax_amt_local: number }>
  sub_total: number
  discount_amt: number
  total_after_dis: number
  tax_total: number
  grand_total: number
  home_sub_total: number
  home_tax_total: number
  home_total: number
}

export interface SaveError { line_no: number; field: string; message: string }

export function computeQuotation(
  lines: QuotationLine[], currencyRate: number, discountRate: number, taxRate: number
): Promise<ComputeResult> {
  return apiFetch('/quotations/compute', {
    method: 'POST',
    body: JSON.stringify({ lines, currency_rate: currencyRate, discount_rate: discountRate, tax_rate: taxRate }),
  })
}

export function createQuotation(
  header: QuotationHeader, lines: QuotationLine[]
): Promise<QuotationHeader & { id: number; lines: QuotationLine[] }> {
  return apiFetch('/quotations', {
    method: 'POST',
    body: JSON.stringify({ ...header, lines }),
  })
}

export function updateQuotation(
  id: number, header: QuotationHeader, lines: QuotationLine[]
): Promise<QuotationHeader & { id: number; lines: QuotationLine[] }> {
  return apiFetch(`/quotations/${id}`, {
    method: 'PUT',
    body: JSON.stringify({ ...header, lines }),
  })
}

export function getQuotation(id: number): Promise<QuotationHeader & { lines: QuotationLine[] }> {
  return apiFetch(`/quotations/${id}`)
}

export function listQuotations(): Promise<Array<QuotationHeader & { id: number; lines: QuotationLine[] }>> {
  return apiFetch('/quotations')
}
