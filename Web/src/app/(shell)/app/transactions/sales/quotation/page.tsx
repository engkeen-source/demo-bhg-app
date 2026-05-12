'use client'

import { useState } from 'react'
import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'
import DocToolbar from '@/components/common/DocToolbar'
import FormField from '@/components/common/FormField'
import DataGrid, { Column } from '@/components/common/DataGrid'
import TabBar from '@/components/common/TabBar'

const LINE_COLS: Column[] = [
  { key: 'no',        header: '#',          width: '36px', align: 'center' },
  { key: 'itemCode',  header: 'Item Code',  width: '110px' },
  { key: 'desc',      header: 'Description' },
  { key: 'qty',       header: 'Qty',        width: '70px', align: 'right' },
  { key: 'uom',       header: 'UOM',        width: '55px' },
  { key: 'unitPrice', header: 'Unit Price', width: '100px', align: 'right' },
  { key: 'disc',      header: 'Disc %',     width: '65px', align: 'right' },
  { key: 'amount',    header: 'Amount',     width: '110px', align: 'right' },
]

const MOCK_LINES = [
  { id: '1', no: '1', itemCode: 'ITM-002', desc: 'Office Chair — Ergonomic Series B', qty: '10', uom: 'PCS', unitPrice: '480.00', disc: '10.00', amount: '4,320.00' },
  { id: '2', no: '2', itemCode: 'ITM-005', desc: 'Standing Desk 140cm x 70cm', qty: '5', uom: 'PCS', unitPrice: '1,200.00', disc: '5.00', amount: '5,700.00' },
]

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

// Search icon SVG (inline, no dependency)
function SearchIcon() {
  return (
    <svg width="14" height="14" viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.8">
      <circle cx="6.5" cy="6.5" r="4.5" />
      <line x1="10.5" y1="10.5" x2="14" y2="14" strokeLinecap="round" />
    </svg>
  )
}

export default function QuotationPage() {
  const [tab, setTab] = useState('main')

  return (
    <div className="space-y-4">
      <PageHeader title="Quotation" description="frmARQO — Quotation entry and management" />

      <Card noPad>
        <div className="px-4 py-2 border-b border-[#E5DDD3]">
          <DocToolbar />
        </div>

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
                {/* Document State */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Document State :</span>
                  <div className={roClass}>New</div>
                </div>
                {/* Document Date */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Document Date :</span>
                  <FormField label="" type="date" defaultValue="2026-05-12" />
                </div>
                {/* Document ID */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Document ID :</span>
                  <FormField label="" placeholder="" />
                </div>
                {/* Document Type */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Document Type : <span className="text-red-500">*</span></span>
                  <FormField label="" as="select">
                    <option>Quotation</option>
                    <option>Sales Order</option>
                    <option>Invoice</option>
                  </FormField>
                </div>
                {/* Document Group */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Document Group :</span>
                  <FormField label="" as="select">
                    <option value="">— Select —</option>
                    <option>Group A</option>
                    <option>Group B</option>
                  </FormField>
                </div>
              </div>

              {/* Section 2 — Dates & Order Refs */}
              <div className={sectionClass}>
                {/* Enquiry Date */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Enquiry Date :</span>
                  <FormField label="" type="date" placeholder="DD-MM-YYYY" />
                </div>
                {/* Valid Date */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Valid Date :</span>
                  <FormField label="" type="date" defaultValue="2026-05-19" />
                </div>
                {/* Quotation Status */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Quotation Status :</span>
                  <FormField label="" as="select">
                    <option>Pending</option>
                    <option>Approved</option>
                    <option>Lost</option>
                    <option>Won</option>
                  </FormField>
                </div>
                {/* Reason for Loss */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Reason for Loss :</span>
                  <FormField label="" placeholder="" />
                </div>
                {/* Customer PO # */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Customer PO # :</span>
                  <FormField label="" placeholder="" />
                </div>
                {/* Sale Order # */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Sale Order # :</span>
                  <FormField label="" placeholder="" />
                </div>
                {/* Delivery Order # */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Delivery Order # :</span>
                  <FormField label="" placeholder="" />
                </div>
              </div>

              {/* Section 3 — AR / Attachments */}
              <div className={sectionClass}>
                {/* AR Account */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>AR Account : <span className="text-red-500">*</span></span>
                  <FormField label="" as="select">
                    <option value="">— Select —</option>
                    <option>AR-001</option>
                    <option>AR-002</option>
                  </FormField>
                </div>
                {/* AR Account Name */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>AR Account Name :</span>
                  <FormField label="" readOnly placeholder="" />
                </div>
                {/* Discount Account */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Discount Account :</span>
                  <FormField label="" as="select">
                    <option value="">— Select —</option>
                    <option>DA-001</option>
                  </FormField>
                </div>
                {/* Attachments */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Attachments :</span>
                  <button className="h-9 px-3 rounded border border-[#D8CFC4] bg-white text-[10pt] font-calibri text-[#404040] text-left hover:bg-[#F3EAE2] transition-colors w-fit">
                    Attached (0)
                  </button>
                </div>
                {/* Request Remark */}
                <div className={`${rowClass} grid-cols-[140px_1fr] items-start`}>
                  <span className="text-[9pt] font-calibri font-semibold text-[#404040] pt-2 whitespace-nowrap">Request Remark :</span>
                  <textarea
                    className="w-full h-20 rounded border border-[#D8CFC4] bg-white px-2.5 py-2 text-[10pt] font-calibri text-[#404040] resize-none focus:outline-none focus:ring-2 focus:ring-[#6C4C2C]/30 focus:border-[#6C4C2C]"
                    placeholder=""
                  />
                </div>
                {/* Potential Project */}
                <div className={`${rowClass} grid-cols-[140px_1fr] items-center`}>
                  <span className={labelClass}>Potential Project :</span>
                  <input type="checkbox" className="h-3.5 w-3.5 accent-[#6C4C2C]" />
                </div>
                {/* Printed */}
                <div className={`${rowClass} grid-cols-[140px_1fr] items-center`}>
                  <span className={labelClass}>Printed :</span>
                  <input type="checkbox" className="h-3.5 w-3.5 accent-[#6C4C2C]" />
                </div>
              </div>
            </div>

            {/* ════ RIGHT COLUMN ════ */}
            <div className="space-y-4">

              {/* Section 1 — Customer */}
              <div className={sectionClass}>
                {/* Customer ID */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Customer ID : <span className="text-red-500">*</span></span>
                  <FormField label="" placeholder="Search customer id" adornment={<SearchIcon />} />
                </div>
                {/* Customer Name */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Customer Name : <span className="text-red-500">*</span></span>
                  <FormField label="" placeholder="Search customer name" adornment={<SearchIcon />} />
                </div>
                {/* Representative */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Representative : <span className="text-red-500">*</span></span>
                  <FormField label="" as="select">
                    <option value="">— Select —</option>
                    <option>REP-001</option>
                  </FormField>
                </div>
                {/* Head Sales */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Head Sales : <span className="text-red-500">*</span></span>
                  <FormField label="" as="select">
                    <option value="">— Select —</option>
                    <option>HS-001</option>
                  </FormField>
                </div>
                {/* Attention */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Attention :</span>
                  <FormField label="" placeholder="" />
                </div>
              </div>

              {/* Section 2 — Reference / Vessel / Currency */}
              <div className={sectionClass}>
                {/* Reference */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Reference :</span>
                  <FormField label="" placeholder="" />
                </div>
                {/* Vessel Name */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Vessel Name :</span>
                  <FormField label="" as="select">
                    <option value="">— Select —</option>
                    <option>Vessel A</option>
                  </FormField>
                </div>
                {/* Document Mark */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Document Mark :</span>
                  <FormField label="" placeholder="" />
                </div>
                {/* Remarks */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Remarks :</span>
                  <FormField label="" placeholder="" />
                </div>
                {/* Price Type + Terms (paired) */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Price Type :</span>
                  <div className="grid grid-cols-2 gap-x-2">
                    <FormField label="" as="select">
                      <option value="">— Select —</option>
                      <option>Retail</option>
                      <option>Wholesale</option>
                    </FormField>
                    <FormField label="Terms :" as="select">
                      <option value="">— Select —</option>
                      <option>Net 30</option>
                      <option>COD</option>
                    </FormField>
                  </div>
                </div>
                {/* Currency */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Currency :</span>
                  <FormField label="" as="select" defaultValue="SGD">
                    <option>SGD</option>
                    <option>MYR</option>
                    <option>USD</option>
                  </FormField>
                </div>
                {/* Currency Rate + Local Rate (paired) */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Currency Rate :</span>
                  <div className="grid grid-cols-2 gap-x-2">
                    <FormField label="" defaultValue="1.0000" inputMode="decimal" className="[&_input]:text-right [&_input]:tabular-nums" />
                    <FormField label="Local Rate :" defaultValue="1.0000" inputMode="decimal" className="[&_input]:text-right [&_input]:tabular-nums" />
                  </div>
                </div>
              </div>

              {/* Section 3 — Totals */}
              <div className={sectionClass}>
                {/* Sub Total */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Sub Total :</span>
                  <FormField label="" readOnly defaultValue="0.00" inputMode="decimal" className="[&_input]:text-right [&_input]:tabular-nums" />
                </div>
                {/* Discount Rate (paired: % + amount) */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Discount Rate :</span>
                  <div className="grid grid-cols-2 gap-x-2">
                    <FormField label="" readOnly defaultValue="0.00" inputMode="decimal" className="[&_input]:text-right [&_input]:tabular-nums" />
                    <FormField label="" readOnly defaultValue="0.00" inputMode="decimal" className="[&_input]:text-right [&_input]:tabular-nums" />
                  </div>
                </div>
                {/* Total */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Total :</span>
                  <FormField label="" readOnly defaultValue="0.00" inputMode="decimal" className="[&_input]:text-right [&_input]:tabular-nums" />
                </div>
                {/* Tax @ (three-cell: %input + select + amount) */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Tax @ :</span>
                  <div className="grid grid-cols-[56px_1fr_80px] gap-x-2">
                    <FormField label="" readOnly defaultValue="0.0%" className="[&_input]:text-right [&_input]:tabular-nums" />
                    <FormField label="" as="select">
                      <option value="">— Tax Code —</option>
                      <option>GST 9%</option>
                      <option>No Tax</option>
                    </FormField>
                    <FormField label="" readOnly defaultValue="0.00" inputMode="decimal" className="[&_input]:text-right [&_input]:tabular-nums" />
                  </div>
                </div>
                {/* Grand Total */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Grand Total :</span>
                  <FormField label="" readOnly defaultValue="0.00" inputMode="decimal" className="[&_input]:text-right [&_input]:tabular-nums font-semibold" />
                </div>
                {/* Home Sub Total */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Home Sub Total :</span>
                  <FormField label="" readOnly defaultValue="0.00" inputMode="decimal" className="[&_input]:text-right [&_input]:tabular-nums" />
                </div>
                {/* Home Tax Total */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Home Tax Total :</span>
                  <FormField label="" readOnly defaultValue="0.00" inputMode="decimal" className="[&_input]:text-right [&_input]:tabular-nums" />
                </div>
                {/* Home Total */}
                <div className={`${rowClass} grid-cols-[140px_1fr]`}>
                  <span className={labelClass}>Home Total :</span>
                  <FormField label="" readOnly defaultValue="0.00" inputMode="decimal" className="[&_input]:text-right [&_input]:tabular-nums" />
                </div>
              </div>

            </div>
          </div>
        )}

        {/* ── ITEM TAB ── */}
        {tab === 'item' && (
          <>
            <div className="border-t border-[#E5DDD3] px-4 py-3">
              <div className="flex items-center justify-between mb-2">
                <span className="text-[9pt] font-semibold font-calibri text-[#404040]">Line Items</span>
                <button className="text-[9pt] font-calibri text-[#6C4C2C] hover:underline">+ Add Line</button>
              </div>
              <DataGrid columns={LINE_COLS} rows={MOCK_LINES} />
            </div>
            <div className="border-t border-[#E5DDD3] flex justify-end px-4 py-3">
              <div className="w-64 space-y-1.5 text-[10pt] font-calibri">
                <div className="flex justify-between text-[#888]"><span>Sub Total</span><span className="tabular-nums">10,020.00</span></div>
                <div className="flex justify-between text-[#888]"><span>Discount</span><span className="tabular-nums">720.00</span></div>
                <div className="border-t border-[#E5DDD3] pt-1.5 flex justify-between font-semibold text-[#6C4C2C] text-[11pt]">
                  <span>Total (SGD)</span><span className="tabular-nums">9,300.00</span>
                </div>
              </div>
            </div>
          </>
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
