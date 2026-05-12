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
  { id: 'remarks', label: 'Remarks' },
  { id: 'other',   label: 'Other Info' },
]

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

        {tab === 'main' && (
          <div className="px-4 pt-3 pb-4 space-y-4">
            <div className="grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-4">
              <FormField label="Doc No." value="QO-2026-0009" readOnly />
              <FormField label="Doc Date" type="date" defaultValue="2026-05-11" />
              <FormField label="Valid Until" type="date" defaultValue="2026-06-11" />
              <FormField label="Status" as="select"><option>Open</option><option>Converted</option><option>Closed</option></FormField>
            </div>
            <div className="grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-4">
              <div className="lg:col-span-2">
                <FormField label="Customer *" as="select">
                  <option>BossSO Holdings Berhad</option>
                  <option>BossSO Trading Sdn Bhd</option>
                </FormField>
              </div>
              <FormField label="Salesperson" defaultValue="ADMIN" />
              <FormField label="Currency" as="select"><option>MYR</option><option>USD</option></FormField>
            </div>
            <div className="grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-4">
              <FormField label="Payment Terms" as="select"><option>Net 30</option><option>COD</option></FormField>
              <FormField label="Customer PO No." placeholder="Optional" />
              <FormField label="Reference" placeholder="Optional" />
            </div>
          </div>
        )}

        {tab === 'remarks' && (
          <div className="px-4 py-4">
            <label className="text-[9pt] font-semibold font-calibri text-[#404040] block mb-1">Remarks</label>
            <textarea className="w-full h-28 rounded border border-[#D8CFC4] px-2.5 py-2 text-[10pt] font-calibri resize-none focus:outline-none focus:ring-2 focus:ring-[#6C4C2C]/30" placeholder="Quotation remarks..." />
          </div>
        )}

        {tab === 'other' && (
          <div className="px-4 py-4 grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-3">
            <FormField label="Territory" as="select"><option>Central</option><option>Northern</option></FormField>
            <FormField label="Price List" as="select"><option>Standard</option><option>VIP</option></FormField>
            <FormField label="Created By" value="ADMIN" readOnly />
          </div>
        )}

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
              <span>Total (MYR)</span><span className="tabular-nums">9,300.00</span>
            </div>
          </div>
        </div>
      </Card>
    </div>
  )
}
