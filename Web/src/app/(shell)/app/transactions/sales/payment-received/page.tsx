'use client'

import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'
import DocToolbar from '@/components/common/DocToolbar'
import FormField from '@/components/common/FormField'
import DataGrid, { Column } from '@/components/common/DataGrid'

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
    <div className="space-y-4">
      <PageHeader title="Payment Received" description="frmARRO — Payment receipt from customer" />

      <Card noPad>
        <div className="px-4 py-2 border-b border-[#E5DDD3]">
          <DocToolbar />
        </div>

        <div className="px-4 pt-4 pb-4 space-y-4">
          <div className="grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-4">
            <FormField label="Receipt No." value="PR-2026-0004" readOnly />
            <FormField label="Receipt Date" type="date" defaultValue="2026-05-12" />
            <FormField label="Currency" as="select"><option>MYR</option><option>USD</option></FormField>
            <FormField label="Exchange Rate" defaultValue="1.0000" type="number" />
          </div>
          <div className="grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-4">
            <div className="lg:col-span-2">
              <FormField label="Customer *" as="select">
                <option>BossSO Trading Sdn Bhd</option>
              </FormField>
            </div>
            <FormField label="Payment Mode" as="select">
              <option>Cheque</option>
              <option>Bank Transfer</option>
              <option>Cash</option>
              <option>Credit Card</option>
            </FormField>
            <FormField label="Bank Account" as="select">
              <option>Maybank — MYR 1234</option>
              <option>CIMB — MYR 5678</option>
            </FormField>
          </div>
          <div className="grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-4">
            <FormField label="Cheque / Ref No." placeholder="Optional" />
            <FormField label="Cheque Date" type="date" />
            <FormField label="Salesperson" defaultValue="ADMIN" />
          </div>
        </div>

        {/* Amount box */}
        <div className="border-t border-[#E5DDD3] px-4 py-3 bg-[#FAF8F5]">
          <div className="flex items-end gap-6">
            <div className="flex-1">
              <label className="text-[9pt] font-semibold font-calibri text-[#404040] block mb-1">Remarks</label>
              <textarea className="w-full h-12 rounded border border-[#D8CFC4] px-2.5 py-1.5 text-[10pt] font-calibri resize-none focus:outline-none focus:ring-2 focus:ring-[#6C4C2C]/30" placeholder="Optional..." />
            </div>
            <div className="w-56 text-[11pt] font-calibri">
              <label className="text-[9pt] font-semibold text-[#404040] block mb-1">Amount Received (MYR)</label>
              <input
                type="number"
                defaultValue="2619.00"
                className="w-full h-10 rounded border-2 border-[#6C4C2C] px-2.5 text-right font-semibold text-[#6C4C2C] focus:outline-none focus:ring-2 focus:ring-[#6C4C2C]/30 text-[12pt]"
              />
            </div>
          </div>
        </div>

        {/* Apply to invoices */}
        <div className="border-t border-[#E5DDD3] px-4 py-3">
          <span className="text-[9pt] font-semibold font-calibri text-[#404040] block mb-2">Apply to Outstanding Invoices</span>
          <DataGrid columns={APPLY_COLS} rows={MOCK_INVOICES} />
        </div>

        <div className="border-t border-[#E5DDD3] flex justify-end px-4 py-3">
          <div className="w-64 space-y-1.5 text-[10pt] font-calibri">
            <div className="flex justify-between text-[#888]"><span>Amount Received</span><span className="tabular-nums">2,619.00</span></div>
            <div className="flex justify-between text-[#888]"><span>Applied</span><span className="tabular-nums">2,619.00</span></div>
            <div className="border-t border-[#E5DDD3] pt-1.5 flex justify-between font-semibold text-[#6C4C2C] text-[11pt]">
              <span>Un-applied</span><span className="tabular-nums">0.00</span>
            </div>
          </div>
        </div>
      </Card>
    </div>
  )
}
