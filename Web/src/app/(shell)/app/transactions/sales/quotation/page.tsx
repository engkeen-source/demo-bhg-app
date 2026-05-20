'use client'

import { useState, useEffect, useRef, useCallback } from 'react'
import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'
import DocToolbar from '@/components/common/DocToolbar'
import FormField from '@/components/common/FormField'
import TabBar from '@/components/common/TabBar'
import ItemGrid, { newLine } from '@/components/quotation/ItemGrid'
import { computeLines } from '@/components/quotation/calc'
import {
  searchCustomers, getCustomer, createQuotation, updateQuotation,
  type CustomerSummary, type QuotationHeader, type QuotationLine, type SaveError,
} from '@/lib/api'

// ── helpers ───────────────────────────────────────────────────────────────────

function today(): string {
  return new Date().toISOString().slice(0, 10)
}

function addDays(d: string, n: number): string {
  const dt = new Date(d)
  dt.setDate(dt.getDate() + n)
  return dt.toISOString().slice(0, 10)
}

function fmt(n?: number) {
  return (n ?? 0).toFixed(2)
}

function blankHeader(): QuotationHeader {
  const d = today()
  return {
    doc_date: d, doc_state: 'New', doc_type: 'Quotation',
    currency: 'SGD', currency_rate: 1, quotation_status: 'Pending',
    potential_project: false, printed: false,
    discount_rate: 0, tax_rate: 9, tax_code: 'GST',
    valid_date: addDays(d, 7),
  }
}

// ── Tabs ──────────────────────────────────────────────────────────────────────

const TABS = [
  { id: 'main',    label: 'Main' },
  { id: 'item',    label: 'Item' },
  { id: 'terms',   label: 'Term & Condition' },
  { id: 'address', label: 'Address' },
  { id: 'other',   label: 'Other Information' },
  { id: 'aging',   label: 'AR Aging Status' },
]

const sectionClass = 'rounded border border-[#E5DDD3] p-3 space-y-2'
const rowClass = 'grid items-start gap-x-3 gap-y-2'
const labelClass = 'text-[9pt] font-calibri font-semibold text-[#404040] leading-9 whitespace-nowrap'
const roClass = 'h-9 w-full rounded border border-[#D8CFC4] bg-[#F3EAE2] px-2.5 text-[10pt] font-calibri text-[#888] flex items-center'

function SearchIcon() {
  return (
    <svg width="14" height="14" viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.8">
      <circle cx="6.5" cy="6.5" r="4.5" />
      <line x1="10.5" y1="10.5" x2="14" y2="14" strokeLinecap="round" />
    </svg>
  )
}

// ── Main component ────────────────────────────────────────────────────────────

export default function QuotationPage() {
  const [tab, setTab] = useState('main')
  const [header, setHeader] = useState<QuotationHeader>(blankHeader())
  const [lines, setLines] = useState<QuotationLine[]>(() => [newLine(1)])
  const [defaultLocation, setDefaultLocation] = useState('Main')
  const [saving, setSaving] = useState(false)
  const [saveErrors, setSaveErrors] = useState<SaveError[]>([])
  const [saveSuccess, setSaveSuccess] = useState(false)

  // Customer autocomplete
  const [custQuery, setCustQuery] = useState('')
  const [custSuggestions, setCustSuggestions] = useState<CustomerSummary[]>([])
  const [showCustDropdown, setShowCustDropdown] = useState(false)
  const custSearchTimer = useRef<ReturnType<typeof setTimeout> | null>(null)

  // Live computed totals (from calc.ts)
  const [computed, setComputed] = useState(() => computeLines([], 1, 0, 9))

  // Recompute whenever lines or header rates change
  useEffect(() => {
    const result = computeLines(lines, header.currency_rate ?? 1, header.discount_rate ?? 0, header.tax_rate ?? 9)
    setComputed(result)
  }, [lines, header.currency_rate, header.discount_rate, header.tax_rate])

  // ── Customer autocomplete ─────────────────────────────────────────────────

  function handleCustInput(val: string) {
    setCustQuery(val)
    // Clear previously-selected customer when user types again
    setHeader(h => ({ ...h, customer_id: undefined, customer_code: undefined, customer_name: undefined }))
    setShowCustDropdown(true)
    if (custSearchTimer.current) clearTimeout(custSearchTimer.current)
    // Empty query still loads all customers (server already returns up to 20 when q='')
    custSearchTimer.current = setTimeout(async () => {
      const results = await searchCustomers(val).catch(() => [])
      setCustSuggestions(results)
    }, val.trim() ? 300 : 100)
  }

  async function handleCustSelect(cust: CustomerSummary) {
    setShowCustDropdown(false)
    // Fetch full record and auto-populate header
    const detail = await getCustomer(cust.id).catch(() => null)
    if (!detail) return
    setCustQuery(detail.code)   // show just the code in the Customer ID field
    setHeader(h => ({
      ...h,
      customer_id:     detail.id,
      customer_code:   detail.code,
      customer_name:   detail.name,
      representative:  detail.representative ?? h.representative,
      head_sales:      detail.head_sales ?? h.head_sales,
      attention:       detail.attention ?? h.attention,
      ar_account_code: detail.ar_account_code ?? h.ar_account_code,
      ar_account_name: detail.ar_account_name ?? h.ar_account_name,
      price_type:      detail.price_type ?? h.price_type,
      terms:           detail.terms ?? h.terms,
      currency:        detail.currency,
      tax_code:        detail.tax_code,
      tax_rate:        detail.tax_rate,
      discount_rate:   detail.discount_rate,
    }))
  }

  // ── Save ─────────────────────────────────────────────────────────────────

  const handleSave = useCallback(async () => {
    setSaving(true)
    setSaveErrors([])
    setSaveSuccess(false)
    try {
      const payload = { ...header, ...computed }
      let result
      if (header.id) {
        result = await updateQuotation(header.id, payload, lines)
      } else {
        result = await createQuotation(payload, lines)
      }
      setHeader(h => ({ ...h, id: result.id, doc_id: result.doc_id ?? h.doc_id, doc_state: result.doc_state }))
      setLines(result.lines ?? lines)
      setSaveSuccess(true)
      setTimeout(() => setSaveSuccess(false), 3000)
    } catch (err: unknown) {
      const apiErr = err as { status?: number; detail?: { errors?: SaveError[] } }
      if (apiErr?.detail?.errors) {
        setSaveErrors(apiErr.detail.errors)
      } else {
        setSaveErrors([{ line_no: 0, field: '', message: 'Failed to connect to the backend. Make sure the API server is running.' }])
      }
    } finally {
      setSaving(false)
    }
  }, [header, lines, computed])

  function handleNew() {
    setHeader(blankHeader())
    setLines([newLine(1)])
    setSaveErrors([])
    setSaveSuccess(false)
    setCustQuery('')
  }

  // Group errors by line_no for display
  const headerErrors = saveErrors.filter(e => e.line_no === 0)
  const lineErrors   = saveErrors.filter(e => e.line_no !== 0)

  // ── Render ────────────────────────────────────────────────────────────────

  return (
    <div className="space-y-4">
      <PageHeader title="Quotation" description="frmARQO — Quotation entry and management" />

      <Card noPad>
        <div className="px-4 py-2 border-b border-[#E5DDD3]">
          <DocToolbar
            onNew={handleNew}
            onSave={handleSave}
            disableSave={saving}
          />
        </div>

        {/* Save feedback banner */}
        {saveSuccess && (
          <div className="mx-4 mt-3 px-4 py-2 rounded bg-green-50 border border-green-200 text-[9pt] font-calibri text-green-700">
            Quotation saved{header.doc_id ? ` — Document ID: ${header.doc_id}` : ''}.
          </div>
        )}
        {(headerErrors.length > 0 || lineErrors.length > 0) && (
          <div className="mx-4 mt-3 px-4 py-3 rounded bg-red-50 border border-red-200">
            <p className="text-[9pt] font-semibold font-calibri text-red-700 mb-1">Save Failed</p>
            <ul className="list-disc list-inside space-y-0.5">
              {[...headerErrors, ...lineErrors].map((e, i) => (
                <li key={i} className="text-[9pt] font-calibri text-red-700">{e.message}</li>
              ))}
            </ul>
          </div>
        )}

        <div className="px-4 pt-2">
          <TabBar tabs={TABS} active={tab} onChange={setTab} />
        </div>

        {/* ── MAIN TAB ── */}
        {tab === 'main' && (
          <div className="px-4 pt-3 pb-4 grid grid-cols-1 lg:grid-cols-2 gap-4">

            {/* ════ LEFT COLUMN ════ */}
            <div className="space-y-4">

              {/* Section 1 — Document */}
              <div className={sectionClass}>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Document State :</span>
                  <div className={roClass}>{header.doc_state ?? 'New'}</div>
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Document Date :</span>
                  <FormField label="" type="date" value={header.doc_date ?? ''} onChange={e => setHeader(h => ({ ...h, doc_date: e.target.value }))} />
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Document ID :</span>
                  <div className={roClass}>{header.doc_id ?? ''}</div>
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Document Type : <span className="text-red-500">*</span></span>
                  <FormField label="" as="select" value={header.doc_type} onChange={e => setHeader(h => ({ ...h, doc_type: e.target.value }))}>
                    <option>Quotation</option>
                    <option>Sales Order</option>
                    <option>Invoice</option>
                  </FormField>
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Document Group :</span>
                  <FormField label="" as="select" value={header.doc_group ?? ''} onChange={e => setHeader(h => ({ ...h, doc_group: e.target.value }))}>
                    <option value="">— Select —</option>
                    <option>Singapore</option>
                    <option>Malaysia</option>
                    <option>International</option>
                  </FormField>
                </div>
              </div>

              {/* Section 2 — Dates & Status */}
              <div className={sectionClass}>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Enquiry Date :</span>
                  <FormField label="" type="date" value={header.enquiry_date ?? ''} onChange={e => setHeader(h => ({ ...h, enquiry_date: e.target.value || undefined }))} />
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Valid Date :</span>
                  <FormField label="" type="date" value={header.valid_date ?? ''} onChange={e => setHeader(h => ({ ...h, valid_date: e.target.value || undefined }))} />
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Quotation Status :</span>
                  <FormField label="" as="select" value={header.quotation_status} onChange={e => setHeader(h => ({ ...h, quotation_status: e.target.value }))}>
                    <option>Pending</option>
                    <option>Approved</option>
                    <option>Lost</option>
                    <option>Won</option>
                  </FormField>
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Reason for Loss :</span>
                  <FormField label="" value={header.reason_for_loss ?? ''} onChange={e => setHeader(h => ({ ...h, reason_for_loss: e.target.value }))} />
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Customer PO # :</span>
                  <FormField label="" value={header.customer_po ?? ''} onChange={e => setHeader(h => ({ ...h, customer_po: e.target.value }))} />
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Sale Order # :</span>
                  <FormField label="" readOnly value="" />
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Delivery Order # :</span>
                  <FormField label="" readOnly value="" />
                </div>
              </div>

              {/* Section 3 — AR / Attachments */}
              <div className={sectionClass}>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>AR Account : <span className="text-red-500">*</span></span>
                  <FormField label="" value={header.ar_account_code ?? ''} onChange={e => setHeader(h => ({ ...h, ar_account_code: e.target.value }))} />
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>AR Account Name :</span>
                  <div className={roClass}>{header.ar_account_name ?? ''}</div>
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Discount Account :</span>
                  <FormField label="" value={header.discount_account ?? ''} onChange={e => setHeader(h => ({ ...h, discount_account: e.target.value }))} />
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr] items-start`}>
                  <span className="text-[9pt] font-calibri font-semibold text-[#404040] pt-2 whitespace-nowrap">Request Remark :</span>
                  <textarea
                    className="w-full h-20 rounded border border-[#D8CFC4] bg-white px-2.5 py-2 text-[10pt] font-calibri text-[#404040] resize-none focus:outline-none focus:ring-2 focus:ring-[#6C4C2C]/30 focus:border-[#6C4C2C]"
                    value={header.request_remark ?? ''}
                    onChange={e => setHeader(h => ({ ...h, request_remark: e.target.value }))}
                  />
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr] items-center`}>
                  <span className={labelClass}>Potential Project :</span>
                  <input type="checkbox" className="h-3.5 w-3.5 accent-[#6C4C2C]"
                    checked={header.potential_project}
                    onChange={e => setHeader(h => ({ ...h, potential_project: e.target.checked }))} />
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr] items-center`}>
                  <span className={labelClass}>Printed :</span>
                  <input type="checkbox" className="h-3.5 w-3.5 accent-[#6C4C2C]"
                    checked={header.printed}
                    onChange={e => setHeader(h => ({ ...h, printed: e.target.checked }))} />
                </div>
              </div>
            </div>

            {/* ════ RIGHT COLUMN ════ */}
            <div className="space-y-4">

              {/* Section 1 — Customer */}
              <div className={sectionClass}>
                {/* Customer ID — searchable dropdown; shows all on focus, filters on type */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Customer ID : <span className="text-red-500">*</span></span>
                  <div className="relative">
                    <div className="relative">
                      <input
                        className="h-9 w-full rounded border border-[#D8CFC4] bg-white px-2.5 pr-8 text-[10pt] font-calibri text-[#404040] focus:outline-none focus:ring-2 focus:ring-[#6C4C2C]/30 focus:border-[#6C4C2C]"
                        value={custQuery}
                        onChange={e => handleCustInput(e.target.value)}
                        onFocus={() => {
                          setShowCustDropdown(true)
                          // Load all customers immediately on focus (empty query)
                          if (custSuggestions.length === 0) {
                            searchCustomers('').then(setCustSuggestions).catch(() => {})
                          }
                        }}
                        onBlur={() => setTimeout(() => setShowCustDropdown(false), 200)}
                        placeholder="Search customer id…"
                      />
                      <div className="absolute inset-y-0 right-0 flex items-center pr-2 pointer-events-none text-[#888]">
                        <SearchIcon />
                      </div>
                    </div>
                    {showCustDropdown && custSuggestions.length > 0 && (
                      <ul className="absolute z-30 top-full left-0 right-0 bg-white border border-[#D8CFC4] rounded shadow-lg max-h-48 overflow-y-auto">
                        {custSuggestions.map(c => (
                          <li
                            key={c.id}
                            className="px-3 py-1.5 font-calibri text-[#404040] hover:bg-[#F3EAE2] cursor-pointer"
                            onMouseDown={() => handleCustSelect(c)}
                          >
                            <div className="text-[9pt] font-semibold text-[#6C4C2C]">{c.code} {c.name}</div>
                            <div className="text-[8pt] text-[#888]">{c.name}</div>
                          </li>
                        ))}
                      </ul>
                    )}
                  </div>
                </div>

                {/* Customer Name — auto-populated from selection */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Customer Name : <span className="text-red-500">*</span></span>
                  <div className={roClass}>{header.customer_name ?? ''}</div>
                </div>

                {/* Auto-populated fields */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Representative : <span className="text-red-500">*</span></span>
                  <FormField label="" value={header.representative ?? ''} onChange={e => setHeader(h => ({ ...h, representative: e.target.value }))} />
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Head Sales : <span className="text-red-500">*</span></span>
                  <div className={roClass}>{header.head_sales ?? ''}</div>
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Attention :</span>
                  <FormField label="" value={header.attention ?? ''} onChange={e => setHeader(h => ({ ...h, attention: e.target.value }))} />
                </div>
              </div>

              {/* Section 2 — Reference / Currency */}
              <div className={sectionClass}>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Reference :</span>
                  <FormField label="" value={header.reference ?? ''} onChange={e => setHeader(h => ({ ...h, reference: e.target.value }))} />
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Document Mark :</span>
                  <FormField label="" value={''} onChange={() => {}} />
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Remarks :</span>
                  <FormField label="" value={header.remarks ?? ''} onChange={e => setHeader(h => ({ ...h, remarks: e.target.value }))} />
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Price Type :</span>
                  <div className="grid grid-cols-2 gap-x-2">
                    <FormField label="" as="select" value={header.price_type ?? ''} onChange={e => setHeader(h => ({ ...h, price_type: e.target.value }))}>
                      <option value="">— Select —</option>
                      <option>Standard Price</option>
                      <option>Wholesale Price</option>
                    </FormField>
                    <FormField label="Terms :" as="select" value={header.terms ?? ''} onChange={e => setHeader(h => ({ ...h, terms: e.target.value }))}>
                      <option value="">— Select —</option>
                      <option>30+60</option>
                      <option>Net 30</option>
                      <option>Net 60</option>
                      <option>COD</option>
                    </FormField>
                  </div>
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Currency :</span>
                  <FormField label="" as="select" value={header.currency} onChange={e => setHeader(h => ({ ...h, currency: e.target.value }))}>
                    <option>SGD</option>
                    <option>MYR</option>
                    <option>USD</option>
                  </FormField>
                </div>
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Currency Rate :</span>
                  <div className="grid grid-cols-2 gap-x-2">
                    <FormField label="" type="number" step="any"
                      value={header.currency_rate ?? 1}
                      onChange={e => setHeader(h => ({ ...h, currency_rate: parseFloat(e.target.value) || 1 }))}
                      className="[&_input]:text-right [&_input]:tabular-nums" />
                    <FormField label="Local Rate :" readOnly value={header.currency_rate?.toFixed(4) ?? '1.0000'}
                      className="[&_input]:text-right [&_input]:tabular-nums" />
                  </div>
                </div>
              </div>

              {/* Section 3 — Totals (live from computeLines) */}
              <div className={sectionClass}>
                {[
                  ['Sub Total :', fmt(computed.sub_total)],
                ].map(([label, val]) => (
                  <div key={label} className={`${rowClass} grid-cols-[140px_1fr]`}>
                    <span className={labelClass}>{label}</span>
                    <div className={`${roClass} justify-end tabular-nums`}>{val}</div>
                  </div>
                ))}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Discount Rate :</span>
                  <div className="grid grid-cols-2 gap-x-2">
                    <FormField label="" type="number" step="any" min="0" max="100"
                      value={header.discount_rate ?? 0}
                      onChange={e => setHeader(h => ({ ...h, discount_rate: parseFloat(e.target.value) || 0 }))}
                      className="[&_input]:text-right [&_input]:tabular-nums" />
                    <div className={`${roClass} justify-end tabular-nums`}>{fmt(computed.discount_amt)}</div>
                  </div>
                </div>
                {[
                  ['Total :', fmt(computed.total_after_dis)],
                ].map(([label, val]) => (
                  <div key={label} className={`${rowClass} grid-cols-[140px_1fr]`}>
                    <span className={labelClass}>{label}</span>
                    <div className={`${roClass} justify-end tabular-nums`}>{val}</div>
                  </div>
                ))}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Tax @ :</span>
                  <div className="grid grid-cols-[56px_1fr_80px] gap-x-2">
                    <div className={`${roClass} justify-end tabular-nums`}>{(header.tax_rate ?? 9).toFixed(1)}%</div>
                    <FormField label="" as="select" value={header.tax_code ?? 'GST'} onChange={e => setHeader(h => ({ ...h, tax_code: e.target.value }))}>
                      <option value="GST">GST</option>
                      <option value="">No Tax</option>
                    </FormField>
                    <div className={`${roClass} justify-end tabular-nums`}>{fmt(computed.tax_total)}</div>
                  </div>
                </div>
                {[
                  ['Grand Total :', fmt(computed.grand_total)],
                  ['Home Sub Total :', fmt(computed.home_sub_total)],
                  ['Home Tax Total :', fmt(computed.home_tax_total)],
                  ['Home Total :', fmt(computed.home_total)],
                ].map(([label, val]) => (
                  <div key={label} className={`${rowClass} grid-cols-[140px_1fr]`}>
                    <span className={labelClass}>{label}</span>
                    <div className={`${roClass} justify-end tabular-nums font-semibold`}>{val}</div>
                  </div>
                ))}
              </div>
            </div>
          </div>
        )}

        {/* ── ITEM TAB ── */}
        {tab === 'item' && (
          <div className="px-4 pt-3 pb-4">
            {/* Line-level save errors */}
            {lineErrors.length > 0 && (
              <div className="mb-3 px-4 py-3 rounded bg-red-50 border border-red-200">
                <p className="text-[9pt] font-semibold font-calibri text-red-700 mb-1">Line item errors:</p>
                <ul className="list-disc list-inside space-y-0.5">
                  {lineErrors.map((e, i) => (
                    <li key={i} className="text-[9pt] font-calibri text-red-700">{e.message}</li>
                  ))}
                </ul>
              </div>
            )}
            <ItemGrid
              lines={lines}
              subTotal={computed.sub_total}
              currency={header.currency}
              defaultLocation={defaultLocation}
              onDefaultLocationChange={setDefaultLocation}
              onChange={setLines}
              computedLines={computed.lines}
            />
          </div>
        )}

        {/* ── PLACEHOLDER TABS ── */}
        {['terms', 'address', 'other', 'aging'].includes(tab) && (
          <div className="px-4 py-10 text-center text-[10pt] font-calibri text-[#888]">
            {TABS.find(t => t.id === tab)?.label} — coming soon
          </div>
        )}

      </Card>
    </div>
  )
}
