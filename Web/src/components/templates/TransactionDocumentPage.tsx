'use client'

import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'
import DocToolbar from '@/components/common/DocToolbar'
import FormField from '@/components/common/FormField'
import DataGrid, { Column } from '@/components/common/DataGrid'

interface Props {
  title: string
  desktop?: string
}

const LINE_COLUMNS: Column[] = [
  { key: 'no', header: '#', width: '40px', align: 'center' },
  { key: 'itemCode', header: 'Item Code', width: '110px' },
  { key: 'description', header: 'Description' },
  { key: 'qty', header: 'Qty', width: '70px', align: 'right' },
  { key: 'uom', header: 'UOM', width: '60px' },
  { key: 'unitPrice', header: 'Unit Price', width: '100px', align: 'right' },
  { key: 'discount', header: 'Disc %', width: '70px', align: 'right' },
  { key: 'amount', header: 'Amount', width: '110px', align: 'right' },
]

const MOCK_LINES = [
  { id: '1', no: '1', itemCode: 'ITM-001', description: 'Product A — Standard Unit', qty: '10', uom: 'PCS', unitPrice: '150.00', discount: '0', amount: '1,500.00' },
  { id: '2', no: '2', itemCode: 'ITM-002', description: 'Product B — Deluxe Set', qty: '5', uom: 'SET', unitPrice: '320.00', discount: '5', amount: '1,520.00' },
  { id: '3', no: '3', itemCode: 'ITM-003', description: 'Service Charge — Installation', qty: '1', uom: 'JOB', unitPrice: '200.00', discount: '0', amount: '200.00' },
]

export default function TransactionDocumentPage({ title, desktop }: Props) {
  return (
    <div className="space-y-4">
      <PageHeader title={title} />

      {/* Toolbar */}
      <Card noPad>
        <div className="px-4 py-2.5 border-b border-[#E5DDD3]">
          <DocToolbar />
        </div>

        {/* Header fields */}
        <div className="p-4">
          <div className="grid grid-cols-2 gap-x-8 gap-y-3 lg:grid-cols-4">
            <FormField label="Doc No." value="—" readOnly />
            <FormField label="Doc Date" type="date" defaultValue="2026-05-12" />
            <FormField
              label="Customer"
              as="select"
            >
              <option>BossSO Trading Sdn Bhd</option>
              <option>BossSO Retail Sdn Bhd</option>
            </FormField>
            <FormField label="Reference" placeholder="Optional" />
            <FormField
              label="Terms"
              as="select"
            >
              <option>Net 30</option>
              <option>Net 60</option>
              <option>COD</option>
            </FormField>
            <FormField label="Salesperson" defaultValue="ADMIN" />
            <FormField label="Currency" defaultValue="MYR" />
            <FormField label="Delivery Date" type="date" defaultValue="2026-05-19" />
          </div>
          <div className="mt-3">
            <FormField label="Remarks" as="input" placeholder="Optional remarks..." />
          </div>
        </div>

        {/* Line items */}
        <div className="px-4 pb-4 space-y-2">
          <div className="flex items-center justify-between">
            <span className="text-[9pt] font-semibold font-calibri text-[#404040]">Line Items</span>
            <button className="text-[9pt] font-calibri text-[#6C4C2C] hover:underline">+ Add Line</button>
          </div>
          <DataGrid columns={LINE_COLUMNS} rows={MOCK_LINES} />
        </div>

        {/* Totals */}
        <div className="border-t border-[#E5DDD3] px-4 py-3 flex justify-end">
          <div className="w-64 space-y-1.5 text-[10pt] font-calibri">
            <TotalRow label="Sub Total" value="3,220.00" />
            <TotalRow label="Tax (0%)" value="0.00" />
            <TotalRow label="Discount" value="80.00" />
            <div className="border-t border-[#E5DDD3] pt-1.5">
              <TotalRow label="Total (MYR)" value="3,140.00" bold />
            </div>
          </div>
        </div>
      </Card>

      {desktop && (
        <p className="text-[8pt] font-calibri text-[#AAA]">
          Phase 2: connects to <code className="bg-[#F3EAE2] px-1 rounded">{desktop}</code>
        </p>
      )}
    </div>
  )
}

function TotalRow({ label, value, bold }: { label: string; value: string; bold?: boolean }) {
  return (
    <div className={`flex justify-between ${bold ? 'font-semibold text-[#6C4C2C]' : 'text-[#888]'}`}>
      <span>{label}</span>
      <span className="tabular-nums">{value}</span>
    </div>
  )
}
