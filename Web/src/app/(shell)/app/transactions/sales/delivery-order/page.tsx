'use client'

import { useState } from 'react'
import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'
import DocToolbar from '@/components/common/DocToolbar'
import FormField from '@/components/common/FormField'
import DataGrid, { Column } from '@/components/common/DataGrid'

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

export default function DeliveryOrderPage() {
  const [tab, setTab] = useState<'main' | 'del'>('main')

  return (
    <div className="space-y-4">
      <PageHeader title="Delivery Order" description="frmARSO — Delivery Order mode" />

      <Card noPad>
        <div className="px-4 py-2 border-b border-[#E5DDD3]">
          <DocToolbar />
        </div>

        <div className="flex border-b border-[#E5DDD3] px-4 pt-2 gap-0">
          {(['main', 'del'] as const).map(t => (
            <button key={t} onClick={() => setTab(t)}
              className={`px-4 py-2 text-[10pt] font-calibri font-medium relative capitalize ${tab === t ? 'text-[#6C4C2C] after:absolute after:bottom-0 after:left-0 after:right-0 after:h-0.5 after:bg-[#6C4C2C]' : 'text-[#888]'}`}>
              {t === 'main' ? 'Main' : 'Delivery'}
            </button>
          ))}
        </div>

        {tab === 'main' && (
          <div className="px-4 pt-3 pb-4 grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-4">
            <FormField label="DO No." value="DO-2026-0006" readOnly />
            <FormField label="DO Date" type="date" defaultValue="2026-05-12" />
            <FormField label="Status" as="select"><option>Open</option><option>Invoiced</option></FormField>
            <FormField label="Customer *" as="select">
              <option>BossSO Trading Sdn Bhd</option>
            </FormField>
            <FormField label="Salesperson" defaultValue="ADMIN" />
            <FormField label="Customer PO No." placeholder="Optional" />
            <FormField label="Reference" placeholder="Optional" />
          </div>
        )}

        {tab === 'del' && (
          <div className="px-4 py-4 grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-3">
            <FormField label="Delivery Address" defaultValue="123 Jalan Utama, KL" />
            <FormField label="Ship Via" as="select"><option>Road Haulage</option><option>Air Freight</option></FormField>
            <FormField label="Delivery Date" type="date" defaultValue="2026-05-14" />
          </div>
        )}

        <div className="border-t border-[#E5DDD3] px-4 py-3">
          <div className="flex items-center justify-between mb-2">
            <span className="text-[9pt] font-semibold font-calibri text-[#404040]">Items to Deliver</span>
            <button className="text-[9pt] font-calibri text-[#6C4C2C] hover:underline">+ Add from SO</button>
          </div>
          <DataGrid columns={LINE_COLS} rows={MOCK_LINES} />
        </div>
      </Card>
    </div>
  )
}
