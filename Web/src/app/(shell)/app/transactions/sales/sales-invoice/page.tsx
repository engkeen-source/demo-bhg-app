'use client'

import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'
import DocToolbar from '@/components/common/DocToolbar'
import FormField from '@/components/common/FormField'
import DataGrid, { Column } from '@/components/common/DataGrid'

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
    <div className="space-y-4">
      <PageHeader
        title="Sales Invoice"
        description="frmARSO — Invoice mode"
        actions={<span className="px-2 py-0.5 rounded text-[8pt] font-medium bg-green-50 text-green-700">Invoiced</span>}
      />

      <Card noPad>
        <div className="px-4 py-2 border-b border-[#E5DDD3]">
          <DocToolbar />
        </div>

        <div className="px-4 pt-4 pb-4 space-y-4">
          <div className="grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-4">
            <FormField label="Invoice No." value="IV-2026-0007" readOnly />
            <FormField label="Invoice Date" type="date" defaultValue="2026-05-12" />
            <FormField label="Due Date" type="date" defaultValue="2026-06-11" />
            <FormField label="Status" as="select"><option>Open</option><option>Paid</option><option>Partial</option></FormField>
          </div>
          <div className="grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-4">
            <div className="lg:col-span-2">
              <FormField label="Bill To *" as="select">
                <option>BossSO Trading Sdn Bhd</option>
              </FormField>
            </div>
            <FormField label="Currency" as="select"><option>MYR</option><option>USD</option></FormField>
            <FormField label="Payment Terms" as="select"><option>Net 30</option><option>COD</option></FormField>
          </div>
          <div className="grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-4">
            <FormField label="Salesperson" defaultValue="ADMIN" />
            <FormField label="DO Ref" defaultValue="DO-2026-0006" readOnly />
            <FormField label="Customer PO No." defaultValue="PO-CUST-0012" />
          </div>
        </div>

        <div className="border-t border-[#E5DDD3] px-4 py-3">
          <span className="text-[9pt] font-semibold font-calibri text-[#404040] block mb-2">Invoice Lines</span>
          <DataGrid columns={LINE_COLS} rows={LINES} />
        </div>

        <div className="border-t border-[#E5DDD3] flex justify-end px-4 py-3">
          <div className="w-72 space-y-1.5 text-[10pt] font-calibri">
            <div className="flex justify-between text-[#888]"><span>Sub Total</span><span className="tabular-nums">2,648.50</span></div>
            <div className="flex justify-between text-[#888]"><span>Less: Discount</span><span className="tabular-nums">29.75</span></div>
            <div className="flex justify-between text-[#888]"><span>Tax (SST 0%)</span><span className="tabular-nums">0.00</span></div>
            <div className="flex justify-between text-[#888]"><span>Rounding Adj.</span><span className="tabular-nums">0.25</span></div>
            <div className="border-t border-[#E5DDD3] pt-1.5 flex justify-between font-semibold text-[#6C4C2C] text-[11pt]">
              <span>Invoice Total (MYR)</span><span className="tabular-nums">2,619.00</span>
            </div>
            <div className="flex justify-between text-green-700 font-medium"><span>Paid</span><span className="tabular-nums">0.00</span></div>
            <div className="flex justify-between text-red-600 font-semibold"><span>Balance Due</span><span className="tabular-nums">2,619.00</span></div>
          </div>
        </div>
      </Card>
    </div>
  )
}
