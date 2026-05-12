'use client'

import { useState } from 'react'
import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'
import DocToolbar from '@/components/common/DocToolbar'
import FormField from '@/components/common/FormField'
import DataGrid, { Column } from '@/components/common/DataGrid'
import TabBar from '@/components/common/TabBar'

const LINE_COLUMNS: Column[] = [
  { key: 'no',        header: '#',          width: '36px',  align: 'center' },
  { key: 'itemCode',  header: 'Item Code',  width: '110px' },
  { key: 'desc',      header: 'Description' },
  { key: 'qty',       header: 'Qty',        width: '70px',  align: 'right' },
  { key: 'uom',       header: 'UOM',        width: '55px' },
  { key: 'unitPrice', header: 'Unit Price', width: '100px', align: 'right' },
  { key: 'disc',      header: 'Disc %',     width: '65px',  align: 'right' },
  { key: 'tax',       header: 'Tax',        width: '55px' },
  { key: 'amount',    header: 'Amount',     width: '110px', align: 'right' },
]

const MOCK_LINES = [
  { id: '1', no: '1', itemCode: 'ITM-001', desc: 'Aluminium Frame — Type A (Silver)', qty: '20', uom: 'PCS', unitPrice: '85.00', disc: '0.00', tax: '0%', amount: '1,700.00' },
  { id: '2', no: '2', itemCode: 'ITM-004', desc: 'Stainless Steel Rod 600mm', qty: '15', uom: 'PCS', unitPrice: '42.00', disc: '5.00', tax: '0%', amount: '598.50' },
  { id: '3', no: '3', itemCode: 'SVC-001', desc: 'Delivery & Installation Service', qty: '1', uom: 'JOB', unitPrice: '350.00', disc: '0.00', tax: '0%', amount: '350.00' },
]

const HEADER_TABS = [
  { id: 'main',     label: 'Main' },
  { id: 'delivery', label: 'Delivery' },
  { id: 'remarks',  label: 'Remarks' },
  { id: 'other',    label: 'Other Info' },
]

export default function SalesOrderPage() {
  const [tab, setTab] = useState('main')
  const [docStatus] = useState<'new' | 'open'>('open')

  return (
    <div className="space-y-4">
      <PageHeader
        title="Sales Order"
        description="frmARSO — Sales Order entry and management"
        actions={
          <span className={`px-2 py-0.5 rounded text-[8pt] font-medium ${docStatus === 'open' ? 'bg-blue-50 text-blue-700' : 'bg-gray-100 text-gray-500'}`}>
            {docStatus === 'open' ? 'Open' : 'New'}
          </span>
        }
      />

      <Card noPad>
        {/* Toolbar */}
        <div className="px-4 py-2 border-b border-[#E5DDD3]">
          <DocToolbar />
        </div>

        {/* Header tabs */}
        <div className="px-4 pt-2">
          <TabBar tabs={HEADER_TABS} active={tab} onChange={setTab} />
        </div>

        {tab === 'main' && (
          <div className="px-4 pt-3 pb-4 space-y-4">
            {/* Row 1 */}
            <div className="grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-4">
              <FormField label="Doc No." value="SO-2026-0012" readOnly />
              <FormField label="Doc Date" type="date" defaultValue="2026-05-12" />
              <FormField label="Due Date" type="date" defaultValue="2026-06-11" />
              <FormField label="Status" as="select"><option>Open</option><option>Closed</option><option>Cancelled</option></FormField>
            </div>
            {/* Row 2 */}
            <div className="grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-4">
              <div className="lg:col-span-2">
                <FormField label="Customer *" as="select">
                  <option>BossSO Trading Sdn Bhd</option>
                  <option>BossSO Retail Sdn Bhd</option>
                  <option>BossSO Holdings Berhad</option>
                </FormField>
              </div>
              <FormField label="Currency" as="select"><option>MYR</option><option>USD</option><option>SGD</option></FormField>
              <FormField label="Exchange Rate" defaultValue="1.0000" type="number" />
            </div>
            {/* Row 3 */}
            <div className="grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-4">
              <FormField label="Salesperson" defaultValue="ADMIN" />
              <FormField label="Payment Terms" as="select"><option>Net 30</option><option>Net 60</option><option>COD</option></FormField>
              <FormField label="Customer PO No." placeholder="Optional" />
              <FormField label="Reference" placeholder="Optional" />
            </div>
          </div>
        )}

        {tab === 'delivery' && (
          <div className="px-4 py-4 grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-3">
            <FormField label="Delivery Address" defaultValue="123 Jalan Utama, Kuala Lumpur" />
            <FormField label="Ship Via" as="select"><option>Air Freight</option><option>Sea Freight</option><option>Road Haulage</option></FormField>
            <FormField label="Ship Name" placeholder="Optional" />
            <FormField label="Delivery Date" type="date" defaultValue="2026-05-19" />
            <FormField label="Port of Discharge" placeholder="Optional" />
            <FormField label="Port of Loading" placeholder="Optional" />
          </div>
        )}

        {tab === 'remarks' && (
          <div className="px-4 py-4 space-y-3">
            <div>
              <label className="text-[9pt] font-semibold font-calibri text-[#404040] block mb-1">Remarks</label>
              <textarea className="w-full h-24 rounded border border-[#D8CFC4] px-2.5 py-2 text-[10pt] font-calibri text-[#404040] resize-none focus:outline-none focus:ring-2 focus:ring-[#6C4C2C]/30" placeholder="Enter remarks..." />
            </div>
            <div>
              <label className="text-[9pt] font-semibold font-calibri text-[#404040] block mb-1">Internal Notes</label>
              <textarea className="w-full h-20 rounded border border-[#D8CFC4] px-2.5 py-2 text-[10pt] font-calibri text-[#404040] resize-none focus:outline-none focus:ring-2 focus:ring-[#6C4C2C]/30" placeholder="Internal only..." />
            </div>
          </div>
        )}

        {tab === 'other' && (
          <div className="px-4 py-4 grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-3">
            <FormField label="Job No." placeholder="Optional" />
            <FormField label="Territory" as="select"><option>Central</option><option>Northern</option><option>Southern</option><option>East Malaysia</option></FormField>
            <FormField label="Industry" as="select"><option>Manufacturing</option><option>Trading</option><option>Retail</option></FormField>
            <FormField label="Price List" as="select"><option>Standard</option><option>VIP</option></FormField>
            <FormField label="Created By" value="ADMIN" readOnly />
            <FormField label="Created Date" value="12/05/2026" readOnly />
          </div>
        )}

        {/* Line items */}
        <div className="border-t border-[#E5DDD3] px-4 py-3">
          <div className="flex items-center justify-between mb-2">
            <span className="text-[9pt] font-semibold font-calibri text-[#404040]">Line Items</span>
            <div className="flex gap-2">
              <button className="text-[9pt] font-calibri text-[#6C4C2C] hover:underline">+ Add Line</button>
              <button className="text-[9pt] font-calibri text-[#888] hover:underline">Import</button>
            </div>
          </div>
          <DataGrid columns={LINE_COLUMNS} rows={MOCK_LINES} />
        </div>

        {/* Footer totals */}
        <div className="border-t border-[#E5DDD3] flex flex-col items-end px-4 py-3 gap-1">
          <div className="w-72 space-y-1.5 text-[10pt] font-calibri">
            <TotalRow label="Sub Total (MYR)" value="2,648.50" />
            <TotalRow label="Discount" value="29.75" />
            <TotalRow label="Tax" value="0.00" />
            <TotalRow label="Rounding" value="0.25" />
            <div className="border-t border-[#E5DDD3] pt-1.5">
              <TotalRow label="Total (MYR)" value="2,619.00" bold />
            </div>
          </div>
        </div>
      </Card>
    </div>
  )
}

function TotalRow({ label, value, bold }: { label: string; value: string; bold?: boolean }) {
  return (
    <div className={`flex justify-between ${bold ? 'font-semibold text-[#6C4C2C] text-[11pt]' : 'text-[#888]'}`}>
      <span>{label}</span>
      <span className="tabular-nums">{value}</span>
    </div>
  )
}
