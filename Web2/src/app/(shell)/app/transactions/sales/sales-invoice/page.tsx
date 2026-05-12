'use client'

import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'
import DocToolbar from '@/components/common/DocToolbar'
import FormField from '@/components/common/FormField'
import DataGrid, { Column } from '@/components/common/DataGrid'
import Badge from '@/components/common/Badge'

const LINE_COLS: Column[] = [
  { key: 'no',       header: '#',          width: '36px', align: 'center' },
  { key: 'doRef',    header: 'DO Ref',     width: '110px' },
  { key: 'itemCode', header: 'Item Code',  width: '110px' },
  { key: 'desc',     header: 'Description' },
  { key: 'qty',      header: 'Qty',        width: '70px', align: 'right' },
  { key: 'uom',      header: 'UOM',        width: '55px' },
  { key: 'price',    header: 'Unit Price', width: '100px', align: 'right' },
  { key: 'disc',     header: 'Disc %',     width: '65px', align: 'right' },
  { key: 'amount',   header: 'Amount',     width: '110px', align: 'right' },
]

const LINES = [
  { id: '1', no: '1', doRef: 'DO-2026-0006', itemCode: 'ITM-001', desc: 'Aluminium Frame — Type A', qty: '20', uom: 'PCS', price: '85.00', disc: '0.00', amount: '1,700.00' },
  { id: '2', no: '2', doRef: 'DO-2026-0006', itemCode: 'ITM-004', desc: 'Stainless Steel Rod 600mm', qty: '15', uom: 'PCS', price: '42.00', disc: '5.00', amount: '598.50' },
  { id: '3', no: '3', doRef: 'DO-2026-0006', itemCode: 'SVC-001', desc: 'Delivery & Installation', qty: '1', uom: 'JOB', price: '350.00', disc: '0.00', amount: '350.00' },
]

export default function SalesInvoicePage() {
  return (
    <div className="space-y-5">
      <PageHeader
        title="Sales Invoice"
        description="frmARSO — Invoice mode"
        actions={<Badge variant="posted">Invoiced</Badge>}
      />

      <Card noPad>
        <div className="px-5 py-3 border-b border-border">
          <DocToolbar />
        </div>

        <div className="px-5 pt-4 pb-5 space-y-4">
          <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
            <FormField label="Invoice No." value="IV-2026-0007" readOnly />
            <FormField label="Invoice Date" type="date" defaultValue="2026-05-12" />
            <FormField label="Due Date" type="date" defaultValue="2026-06-11" />
            <FormField label="Status" as="select"><option>Open</option><option>Paid</option><option>Partial</option></FormField>
          </div>
          <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
            <div className="lg:col-span-2">
              <FormField label="Bill To *" as="select"><option>BossSO Trading Sdn Bhd</option></FormField>
            </div>
            <FormField label="Currency" as="select"><option>MYR</option><option>USD</option></FormField>
            <FormField label="Payment Terms" as="select"><option>Net 30</option><option>COD</option></FormField>
          </div>
          <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
            <FormField label="Salesperson" defaultValue="ADMIN" />
            <FormField label="DO Ref" defaultValue="DO-2026-0006" readOnly />
            <FormField label="Customer PO No." defaultValue="PO-CUST-0012" />
          </div>
        </div>

        <div className="border-t border-border px-5 py-4">
          <span className="text-sm font-semibold text-txt-primary block mb-3">Invoice Lines</span>
          <div className="rounded-xl border border-border overflow-hidden">
            <DataGrid columns={LINE_COLS} rows={LINES} />
          </div>
        </div>

        <div className="border-t border-border px-5 py-4 flex justify-end">
          <div className="w-72 space-y-2">
            <TotalRow label="Sub Total" value="2,648.50" />
            <TotalRow label="Less: Discount" value="29.75" />
            <TotalRow label="Tax (SST 0%)" value="0.00" />
            <TotalRow label="Rounding Adj." value="0.25" />
            <div className="border-t border-border pt-2.5">
              <TotalRow label="Invoice Total (MYR)" value="2,619.00" bold />
            </div>
            <TotalRow label="Paid" value="0.00" color="text-emerald-600" />
            <TotalRow label="Balance Due" value="2,619.00" color="text-red-600" />
          </div>
        </div>
      </Card>
    </div>
  )
}

function TotalRow({ label, value, bold, color }: { label: string; value: string; bold?: boolean; color?: string }) {
  return (
    <div className={`flex justify-between text-sm ${bold ? `font-bold text-txt-primary text-base` : color ?? 'text-txt-secondary'}`}>
      <span>{label}</span>
      <span className="tabular-nums">{value}</span>
    </div>
  )
}
