'use client'

import { useState } from 'react'
import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'
import Button from '@/components/common/Button'
import FormField from '@/components/common/FormField'
import DataGrid, { Column } from '@/components/common/DataGrid'
import TabBar from '@/components/common/TabBar'
import Badge from '@/components/common/Badge'

const LIST_COLS: Column[] = [
  { key: 'code',      header: 'Item Code',  width: '110px' },
  { key: 'name',      header: 'Description' },
  { key: 'category',  header: 'Category',   width: '120px' },
  { key: 'uom',       header: 'UOM',        width: '60px' },
  { key: 'stkQty',    header: 'Stock Qty',  width: '90px', align: 'right' },
  { key: 'costPrice', header: 'Cost Price', width: '100px', align: 'right' },
  { key: 'sellPrice', header: 'Sell Price', width: '100px', align: 'right' },
  { key: 'status',    header: 'Status',     width: '80px',
    render: (row) => (
      <Badge variant={(row.status as string).toLowerCase() === 'active' ? 'active' : 'inactive'}>
        {row.status as string}
      </Badge>
    )
  },
]

const LIST_ROWS = [
  { id: '1', code: 'ITM-001', name: 'Aluminium Frame — Type A (Silver)', category: 'Hardware', uom: 'PCS', stkQty: '150', costPrice: '65.00', sellPrice: '85.00', status: 'Active' },
  { id: '2', code: 'ITM-002', name: 'Office Chair — Ergonomic Series B', category: 'Furniture', uom: 'PCS', stkQty: '25', costPrice: '320.00', sellPrice: '480.00', status: 'Active' },
  { id: '3', code: 'ITM-003', name: 'Stainless Steel Bracket 200mm', category: 'Hardware', uom: 'PCS', stkQty: '8', costPrice: '12.00', sellPrice: '18.50', status: 'Active' },
  { id: '4', code: 'ITM-004', name: 'Stainless Steel Rod 600mm', category: 'Hardware', uom: 'PCS', stkQty: '3', costPrice: '28.00', sellPrice: '42.00', status: 'Active' },
  { id: '5', code: 'ITM-005', name: 'Standing Desk 140×70cm', category: 'Furniture', uom: 'PCS', stkQty: '12', costPrice: '780.00', sellPrice: '1,200.00', status: 'Active' },
  { id: '6', code: 'SVC-001', name: 'Delivery & Installation Service', category: 'Service', uom: 'JOB', stkQty: '—', costPrice: '200.00', sellPrice: '350.00', status: 'Active' },
]

const FORM_TABS = [
  { id: 'general', label: 'General' },
  { id: 'pricing', label: 'Pricing' },
  { id: 'stock',   label: 'Stock Info' },
  { id: 'other',   label: 'Other' },
]

export default function InventoryItemPage() {
  const [selected, setSelected] = useState<string | undefined>()
  const [tab, setTab] = useState('general')
  const [mode, setMode] = useState<'list' | 'edit'>('list')

  if (mode === 'edit') {
    const row = LIST_ROWS.find(r => r.id === selected) ?? LIST_ROWS[0]
    return (
      <div className="space-y-5">
        <PageHeader
          title="Inventory Item"
          description="frmMSTItm"
          actions={
            <>
              <Button variant="secondary" size="sm" onClick={() => setMode('list')}>← Back to List</Button>
              <Button variant="primary" size="sm">Save</Button>
              <Button variant="ghost" size="sm" className="text-red-600 hover:bg-red-50">Delete</Button>
            </>
          }
        />
        <Card noPad>
          <div className="px-5 pt-3">
            <TabBar tabs={FORM_TABS} active={tab} onChange={setTab} />
          </div>

          {tab === 'general' && (
            <div className="px-5 pt-4 pb-5 space-y-4">
              <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
                <FormField label="Item Code *" defaultValue={row.code} />
                <div className="lg:col-span-3">
                  <FormField label="Description *" defaultValue={row.name} />
                </div>
              </div>
              <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
                <FormField label="Category" as="select"><option>{row.category}</option><option>Hardware</option><option>Furniture</option><option>Service</option></FormField>
                <FormField label="Brand" as="select"><option>—</option><option>BrandA</option></FormField>
                <FormField label="UOM" defaultValue={row.uom} />
                <FormField label="Alt UOM" placeholder="Optional" />
              </div>
              <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
                <FormField label="Barcode" placeholder="Optional" />
                <FormField label="Item Type" as="select"><option>Stock</option><option>Non-Stock</option><option>Service</option></FormField>
                <FormField label="Status" as="select"><option>Active</option><option>Inactive</option></FormField>
              </div>
            </div>
          )}

          {tab === 'pricing' && (
            <div className="px-5 pt-4 pb-5">
              <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
                <FormField label="Cost Price" defaultValue={row.costPrice} type="number" />
                <FormField label="Selling Price" defaultValue={row.sellPrice} type="number" />
                <FormField label="Min Price" placeholder="Optional" />
                <FormField label="Tax Group" as="select"><option>Standard (0%)</option><option>Exempt</option></FormField>
              </div>
            </div>
          )}

          {tab === 'stock' && (
            <div className="px-5 pt-4 pb-5">
              <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
                <FormField label="Reorder Level" defaultValue="10" type="number" />
                <FormField label="Reorder Qty" defaultValue="50" type="number" />
                <FormField label="Location" as="select"><option>Main Warehouse</option><option>Store B</option></FormField>
                <FormField label="Current Stock" value={row.stkQty} readOnly />
              </div>
            </div>
          )}

          {tab === 'other' && (
            <div className="px-5 pt-4 pb-5 grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-3">
              <FormField label="GL Sales Account" defaultValue="401-000 Sales Revenue" />
              <FormField label="GL Cost Account" defaultValue="501-000 Cost of Goods Sold" />
              <FormField label="GL Stock Account" defaultValue="113-000 Inventory" />
            </div>
          )}
        </Card>
      </div>
    )
  }

  return (
    <div className="space-y-5">
      <PageHeader
        title="Inventory Item"
        description="frmMSTItm"
        actions={
          <>
            <Button variant="primary" size="sm">
              <svg width="12" height="12" viewBox="0 0 16 16" fill="none"><path d="M8 2v12M2 8h12" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/></svg>
              New
            </Button>
            <Button variant="secondary" size="sm" disabled={!selected} onClick={() => setMode('edit')}>Edit</Button>
            <Button variant="ghost" size="sm">Import</Button>
            <Button variant="ghost" size="sm" disabled={!selected} className="text-red-600 hover:bg-red-50">Delete</Button>
          </>
        }
      />
      <Card noPad>
        <div className="px-5 py-3 border-b border-border flex items-center gap-2">
          <div className="relative flex-1 max-w-sm">
            <svg className="absolute left-2.5 top-1/2 -translate-y-1/2 text-txt-tertiary" width="13" height="13" viewBox="0 0 16 16" fill="none">
              <circle cx="7" cy="7" r="5" stroke="currentColor" strokeWidth="1.5"/>
              <path d="M11 11l3 3" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
            </svg>
            <input placeholder="Search by code, description…" className="h-8 pl-8 pr-3 w-full rounded-lg border border-border text-sm text-txt-primary focus:outline-none focus:ring-2 focus:ring-brand-500 bg-bg-muted placeholder:text-txt-tertiary" />
          </div>
          <select className="h-8 px-3 rounded-lg border border-border text-sm text-txt-primary focus:outline-none focus:ring-2 focus:ring-brand-500 bg-bg-muted">
            <option>All Categories</option><option>Hardware</option><option>Furniture</option><option>Service</option>
          </select>
        </div>
        <DataGrid
          columns={LIST_COLS}
          rows={LIST_ROWS}
          rowKey="id"
          selectedKey={selected}
          onRowClick={row => { setSelected(row.id as string); setMode('edit') }}
        />
        <div className="px-5 py-2.5 border-t border-border text-xs text-txt-tertiary">{LIST_ROWS.length} items</div>
      </Card>
    </div>
  )
}
