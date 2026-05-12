'use client'

import { useState } from 'react'
import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'
import DocToolbar from '@/components/common/DocToolbar'
import FormField from '@/components/common/FormField'
import DataGrid, { Column } from '@/components/common/DataGrid'
import TabBar from '@/components/common/TabBar'
import Badge from '@/components/common/Badge'

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

  return (
    <div className="space-y-5">
      <PageHeader
        title="Sales Order"
        description="frmARSO — Sales Order entry and management"
        actions={<Badge variant="open">Open</Badge>}
      />

      <Card noPad>
        {/* Toolbar */}
        <div className="px-5 py-3 border-b border-border">
          <DocToolbar />
        </div>

        {/* Header tabs */}
        <div className="px-5 pt-3">
          <TabBar tabs={HEADER_TABS} active={tab} onChange={setTab} />
        </div>

        {tab === 'main' && (
          <div className="px-5 pt-4 pb-5 space-y-4">
            <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
              <FormField label="Doc No." value="SO-2026-0012" readOnly />
              <FormField label="Doc Date" type="date" defaultValue="2026-05-12" />
              <FormField label="Due Date" type="date" defaultValue="2026-06-11" />
              <FormField label="Status" as="select"><option>Open</option><option>Closed</option><option>Cancelled</option></FormField>
            </div>
            <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
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
            <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
              <FormField label="Salesperson" defaultValue="ADMIN" />
              <FormField label="Payment Terms" as="select"><option>Net 30</option><option>Net 60</option><option>COD</option></FormField>
              <FormField label="Customer PO No." placeholder="Optional" />
              <FormField label="Reference" placeholder="Optional" />
            </div>
          </div>
        )}

        {tab === 'delivery' && (
          <div className="px-5 py-4 grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-3">
            <FormField label="Delivery Address" defaultValue="123 Jalan Utama, Kuala Lumpur" />
            <FormField label="Ship Via" as="select"><option>Air Freight</option><option>Sea Freight</option><option>Road Haulage</option></FormField>
            <FormField label="Ship Name" placeholder="Optional" />
            <FormField label="Delivery Date" type="date" defaultValue="2026-05-19" />
            <FormField label="Port of Discharge" placeholder="Optional" />
            <FormField label="Port of Loading" placeholder="Optional" />
          </div>
        )}

        {tab === 'remarks' && (
          <div className="px-5 py-4 space-y-4">
            <FormField as="textarea" label="Remarks" placeholder="Enter remarks…" />
            <FormField as="textarea" label="Internal Notes" placeholder="Internal only…" hint="Not printed on document" />
          </div>
        )}

        {tab === 'other' && (
          <div className="px-5 py-4 grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-3">
            <FormField label="Job No." placeholder="Optional" />
            <FormField label="Territory" as="select"><option>Central</option><option>Northern</option><option>Southern</option><option>East Malaysia</option></FormField>
            <FormField label="Industry" as="select"><option>Manufacturing</option><option>Trading</option><option>Retail</option></FormField>
            <FormField label="Price List" as="select"><option>Standard</option><option>VIP</option></FormField>
            <FormField label="Created By" value="ADMIN" readOnly />
            <FormField label="Created Date" value="12/05/2026" readOnly />
          </div>
        )}

        {/* Line items */}
        <div className="border-t border-border px-5 py-4">
          <div className="flex items-center justify-between mb-3">
            <span className="text-sm font-semibold text-txt-primary">Line Items</span>
            <div className="flex gap-3">
              <button className="text-xs text-brand-600 hover:text-brand-700 font-medium">+ Add Line</button>
              <button className="text-xs text-txt-tertiary hover:text-txt-secondary">Import</button>
            </div>
          </div>
          <div className="rounded-xl border border-border overflow-hidden">
            <DataGrid columns={LINE_COLUMNS} rows={MOCK_LINES} />
          </div>
        </div>

        {/* Footer totals */}
        <div className="border-t border-border px-5 py-4 flex justify-end">
          <div className="w-72 space-y-2">
            <TotalRow label="Sub Total (MYR)" value="2,648.50" />
            <TotalRow label="Discount" value="29.75" />
            <TotalRow label="Tax" value="0.00" />
            <TotalRow label="Rounding" value="0.25" />
            <div className="border-t border-border pt-2.5">
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
    <div className={`flex justify-between text-sm ${bold ? 'font-bold text-txt-primary text-base' : 'text-txt-secondary'}`}>
      <span>{label}</span>
      <span className="tabular-nums">{value}</span>
    </div>
  )
}
