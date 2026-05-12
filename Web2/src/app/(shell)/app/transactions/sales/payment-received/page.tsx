'use client'

import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'
import DocToolbar from '@/components/common/DocToolbar'
import FormField from '@/components/common/FormField'
import DataGrid, { Column } from '@/components/common/DataGrid'
import Badge from '@/components/common/Badge'

const APPLY_COLS: Column[] = [
  { key: 'check',   header: '',           width: '36px', align: 'center' },
  { key: 'invNo',   header: 'Invoice No.', width: '130px' },
  { key: 'invDate', header: 'Date',        width: '100px' },
  { key: 'total',   header: 'Invoice Amt', width: '110px', align: 'right' },
  { key: 'balance', header: 'Balance',     width: '110px', align: 'right' },
  { key: 'apply',   header: 'Apply Amt',   width: '110px', align: 'right' },
]

const MOCK_INVOICES = [
  { id: '1', check: '☑', invNo: 'IV-2026-0005', invDate: '28/04/2026', total: '1,850.00', balance: '1,850.00', apply: '1,850.00' },
  { id: '2', check: '☑', invNo: 'IV-2026-0007', invDate: '12/05/2026', total: '2,619.00', balance: '2,619.00', apply: '769.00' },
  { id: '3', check: '☐', invNo: 'IV-2026-0008', invDate: '12/05/2026', total: '5,450.00', balance: '5,450.00', apply: '0.00' },
]

export default function PaymentReceivedPage() {
  return (
    <div className="space-y-5">
      <PageHeader
        title="Payment Received"
        description="frmARRO — Payment receipt from customer"
        actions={<Badge variant="draft">Draft</Badge>}
      />

      <Card noPad>
        <div className="px-5 py-3 border-b border-border">
          <DocToolbar />
        </div>

        <div className="px-5 pt-4 pb-5 space-y-4">
          <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
            <FormField label="Receipt No." value="PR-2026-0004" readOnly />
            <FormField label="Receipt Date" type="date" defaultValue="2026-05-12" />
            <FormField label="Currency" as="select"><option>MYR</option><option>USD</option></FormField>
            <FormField label="Exchange Rate" defaultValue="1.0000" type="number" />
          </div>
          <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
            <div className="lg:col-span-2">
              <FormField label="Customer *" as="select"><option>BossSO Trading Sdn Bhd</option></FormField>
            </div>
            <FormField label="Payment Mode" as="select">
              <option>Cheque</option><option>Bank Transfer</option><option>Cash</option><option>Credit Card</option>
            </FormField>
            <FormField label="Bank Account" as="select">
              <option>Maybank — MYR 1234</option><option>CIMB — MYR 5678</option>
            </FormField>
          </div>
          <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
            <FormField label="Cheque / Ref No." placeholder="Optional" />
            <FormField label="Cheque Date" type="date" />
            <FormField label="Salesperson" defaultValue="ADMIN" />
          </div>
        </div>

        {/* Amount + Remarks */}
        <div className="border-t border-border px-5 py-4 bg-bg-muted/40">
          <div className="flex items-end gap-6">
            <div className="flex-1">
              <FormField as="textarea" label="Remarks" placeholder="Optional…" />
            </div>
            <div className="w-56 shrink-0">
              <label className="text-xs font-semibold text-txt-secondary uppercase tracking-wide block mb-1.5">Amount Received (MYR)</label>
              <input
                type="number"
                defaultValue="2619.00"
                className="w-full h-11 rounded-lg border-2 border-brand-500 px-3 text-right text-base font-bold text-brand-600 tabular-nums focus:outline-none focus:ring-2 focus:ring-brand-500/30"
              />
            </div>
          </div>
        </div>

        {/* Apply to invoices */}
        <div className="border-t border-border px-5 py-4">
          <span className="text-sm font-semibold text-txt-primary block mb-3">Apply to Outstanding Invoices</span>
          <div className="rounded-xl border border-border overflow-hidden">
            <DataGrid columns={APPLY_COLS} rows={MOCK_INVOICES} />
          </div>
        </div>

        <div className="border-t border-border px-5 py-4 flex justify-end">
          <div className="w-64 space-y-2">
            <TotalRow label="Amount Received" value="2,619.00" />
            <TotalRow label="Applied" value="2,619.00" />
            <div className="border-t border-border pt-2.5">
              <TotalRow label="Un-applied" value="0.00" bold />
            </div>
          </div>
        </div>
      </Card>
    </div>
  )
}

function TotalRow({ label, value, bold }: { label: string; value: string; bold?: boolean }) {
  return (
    <div className={`flex justify-between text-sm ${bold ? 'font-bold text-txt-primary' : 'text-txt-secondary'}`}>
      <span>{label}</span>
      <span className="tabular-nums">{value}</span>
    </div>
  )
}
