'use client'

import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'
import DocToolbar from '@/components/common/DocToolbar'
import FormField from '@/components/common/FormField'
import DataGrid, { Column } from '@/components/common/DataGrid'
import Badge from '@/components/common/Badge'

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
    <div className="space-y-5">
      <PageHeader
        title={title}
        actions={<Badge variant="draft">Draft</Badge>}
      />

      <Card noPad>
        {/* Toolbar */}
        <div className="px-5 py-3 border-b border-border">
          <DocToolbar />
        </div>

        {/* Header fields */}
        <div className="p-5">
          <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
            <FormField label="Doc No." value="—" readOnly />
            <FormField label="Doc Date" type="date" defaultValue="2026-05-12" />
            <FormField label="Customer" as="select">
              <option>BossSO Trading Sdn Bhd</option>
              <option>BossSO Retail Sdn Bhd</option>
            </FormField>
            <FormField label="Reference" placeholder="Optional" />
            <FormField label="Terms" as="select">
              <option>Net 30</option>
              <option>Net 60</option>
              <option>COD</option>
            </FormField>
            <FormField label="Salesperson" defaultValue="ADMIN" />
            <FormField label="Currency" defaultValue="MYR" />
            <FormField label="Delivery Date" type="date" defaultValue="2026-05-19" />
          </div>
          <div className="mt-4">
            <FormField label="Remarks" placeholder="Optional remarks…" />
          </div>
        </div>

        {/* Line items */}
        <div className="border-t border-border px-5 pb-5 space-y-3">
          <div className="flex items-center justify-between pt-4">
            <span className="text-sm font-semibold text-txt-primary">Line Items</span>
            <button className="text-xs text-brand-600 hover:text-brand-700 font-medium">+ Add Line</button>
          </div>
          <div className="rounded-xl border border-border overflow-hidden">
            <DataGrid columns={LINE_COLUMNS} rows={MOCK_LINES} />
          </div>
        </div>

        {/* Totals */}
        <div className="border-t border-border px-5 py-4 flex justify-end">
          <div className="w-64 space-y-2">
            <TotalRow label="Sub Total" value="3,220.00" />
            <TotalRow label="Tax (0%)" value="0.00" />
            <TotalRow label="Discount" value="80.00" />
            <div className="border-t border-border pt-2">
              <TotalRow label="Total (MYR)" value="3,140.00" bold />
            </div>
          </div>
        </div>
      </Card>

      {desktop && (
        <p className="text-xs text-txt-tertiary">
          Phase 2: connects to <code className="bg-bg-muted px-1.5 py-0.5 rounded font-mono text-txt-secondary">{desktop}</code>
        </p>
      )}
    </div>
  )
}

function TotalRow({ label, value, bold }: { label: string; value: string; bold?: boolean }) {
  return (
    <div className={`flex justify-between text-sm ${bold ? 'font-semibold text-txt-primary text-base' : 'text-txt-secondary'}`}>
      <span>{label}</span>
      <span className="tabular-nums">{value}</span>
    </div>
  )
}
