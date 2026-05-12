'use client'

import { useState } from 'react'
import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'
import DocToolbar from '@/components/common/DocToolbar'
import FormField from '@/components/common/FormField'
import DataGrid, { Column } from '@/components/common/DataGrid'
import TabBar from '@/components/common/TabBar'
import Badge from '@/components/common/Badge'

const LINE_COLS: Column[] = [
  { key: 'no',       header: '#',         width: '36px', align: 'center' },
  { key: 'soRef',    header: 'SO Ref',    width: '110px' },
  { key: 'itemCode', header: 'Item Code', width: '110px' },
  { key: 'desc',     header: 'Description' },
  { key: 'soQty',    header: 'SO Qty',    width: '70px', align: 'right' },
  { key: 'doQty',    header: 'DO Qty',    width: '70px', align: 'right' },
  { key: 'uom',      header: 'UOM',       width: '55px' },
]

const MOCK_LINES = [
  { id: '1', no: '1', soRef: 'SO-2026-0010', itemCode: 'ITM-001', desc: 'Aluminium Frame — Type A (Silver)', soQty: '20', doQty: '20', uom: 'PCS' },
  { id: '2', no: '2', soRef: 'SO-2026-0010', itemCode: 'ITM-004', desc: 'Stainless Steel Rod 600mm', soQty: '15', doQty: '15', uom: 'PCS' },
]

const TABS = [
  { id: 'main', label: 'Main' },
  { id: 'del',  label: 'Delivery' },
]

export default function DeliveryOrderPage() {
  const [tab, setTab] = useState('main')

  return (
    <div className="space-y-5">
      <PageHeader
        title="Delivery Order"
        description="frmARSO — Delivery Order mode"
        actions={<Badge variant="open">Open</Badge>}
      />

      <Card noPad>
        <div className="px-5 py-3 border-b border-border">
          <DocToolbar />
        </div>

        <div className="px-5 pt-3">
          <TabBar tabs={TABS} active={tab} onChange={setTab} />
        </div>

        {tab === 'main' && (
          <div className="px-5 pt-4 pb-5 grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
            <FormField label="DO No." value="DO-2026-0006" readOnly />
            <FormField label="DO Date" type="date" defaultValue="2026-05-12" />
            <FormField label="Status" as="select"><option>Open</option><option>Invoiced</option></FormField>
            <FormField label="Customer *" as="select"><option>BossSO Trading Sdn Bhd</option></FormField>
            <FormField label="Salesperson" defaultValue="ADMIN" />
            <FormField label="Customer PO No." placeholder="Optional" />
            <FormField label="Reference" placeholder="Optional" />
          </div>
        )}

        {tab === 'del' && (
          <div className="px-5 py-4 grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-3">
            <FormField label="Delivery Address" defaultValue="123 Jalan Utama, KL" />
            <FormField label="Ship Via" as="select"><option>Road Haulage</option><option>Air Freight</option></FormField>
            <FormField label="Delivery Date" type="date" defaultValue="2026-05-14" />
          </div>
        )}

        <div className="border-t border-border px-5 py-4">
          <div className="flex items-center justify-between mb-3">
            <span className="text-sm font-semibold text-txt-primary">Items to Deliver</span>
            <button className="text-xs text-brand-600 hover:text-brand-700 font-medium">+ Add from SO</button>
          </div>
          <div className="rounded-xl border border-border overflow-hidden">
            <DataGrid columns={LINE_COLS} rows={MOCK_LINES} />
          </div>
        </div>
      </Card>
    </div>
  )
}
