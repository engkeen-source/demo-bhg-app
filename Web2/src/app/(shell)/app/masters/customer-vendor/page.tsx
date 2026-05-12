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
  { key: 'code',    header: 'Code',    width: '100px' },
  { key: 'name',    header: 'Name' },
  { key: 'type',    header: 'Type',    width: '100px' },
  { key: 'phone',   header: 'Phone',   width: '130px' },
  { key: 'balance', header: 'Balance', width: '110px', align: 'right' },
  { key: 'status',  header: 'Status',  width: '80px',
    render: (row) => (
      <Badge variant={(row.status as string).toLowerCase() === 'active' ? 'active' : 'inactive'}>
        {row.status as string}
      </Badge>
    )
  },
]

const LIST_ROWS = [
  { id: '1', code: 'C-0001', name: 'BossSO Trading Sdn Bhd', type: 'Customer', phone: '03-1234 5678', balance: '2,619.00', status: 'Active' },
  { id: '2', code: 'C-0002', name: 'BossSO Retail Sdn Bhd', type: 'Customer', phone: '03-2345 6789', balance: '0.00', status: 'Active' },
  { id: '3', code: 'V-0001', name: 'Vendor ABC Sdn Bhd', type: 'Vendor', phone: '03-3456 7890', balance: '−2,200.00', status: 'Active' },
  { id: '4', code: 'C-0003', name: 'Mega Corp Bhd', type: 'Customer', phone: '03-4567 8901', balance: '8,100.00', status: 'Active' },
  { id: '5', code: 'V-0002', name: 'Supplier XYZ Pte Ltd', type: 'Vendor', phone: '+65 6789 0123', balance: '0.00', status: 'Inactive' },
]

const FORM_TABS = [
  { id: 'general',   label: 'General' },
  { id: 'contacts',  label: 'Contacts' },
  { id: 'addresses', label: 'Addresses' },
  { id: 'financial', label: 'Financial' },
  { id: 'other',     label: 'Other' },
]

export default function CustomerVendorPage() {
  const [selected, setSelected] = useState<string>('1')
  const [tab, setTab] = useState('general')
  const [mode, setMode] = useState<'list' | 'edit'>('list')

  function handleEdit(row: Record<string, unknown>) {
    setSelected(row.id as string)
    setMode('edit')
  }

  if (mode === 'edit') {
    return (
      <div className="space-y-5">
        <PageHeader
          title="Customer / Vendor Record"
          description="frmMSTCon"
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
                <FormField label="Code *" defaultValue="C-0001" />
                <div className="lg:col-span-2">
                  <FormField label="Name *" defaultValue="BossSO Trading Sdn Bhd" />
                </div>
                <FormField label="Type" as="select"><option>Customer</option><option>Vendor</option><option>Both</option></FormField>
              </div>
              <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
                <FormField label="Registration No." defaultValue="1234567-X" />
                <FormField label="Tax Reg. No." placeholder="Optional" />
                <FormField label="Phone" defaultValue="03-1234 5678" />
                <FormField label="Fax" placeholder="Optional" />
              </div>
              <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
                <FormField label="Email" defaultValue="accounts@bosso.com" />
                <FormField label="Website" placeholder="Optional" />
                <FormField label="Industry" as="select"><option>Trading</option><option>Manufacturing</option><option>Retail</option></FormField>
                <FormField label="Territory" as="select"><option>Central</option><option>Northern</option></FormField>
              </div>
              <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
                <FormField label="Status" as="select"><option>Active</option><option>Inactive</option><option>Blacklisted</option></FormField>
                <FormField label="Credit Limit (MYR)" defaultValue="50000" type="number" />
                <FormField label="Credit Days" defaultValue="30" type="number" />
              </div>
            </div>
          )}

          {tab === 'contacts' && (
            <div className="px-5 pt-4 pb-5 space-y-4">
              <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-3">
                <FormField label="Contact Person" defaultValue="Ahmad bin Karim" />
                <FormField label="Designation" defaultValue="Accounts Manager" />
                <FormField label="Mobile" defaultValue="012-345 6789" />
                <FormField label="Direct Email" defaultValue="ahmad@bosso.com" />
              </div>
            </div>
          )}

          {tab === 'addresses' && (
            <div className="px-5 pt-4 pb-5 space-y-4">
              <p className="text-xs font-semibold text-txt-secondary uppercase tracking-wide">Billing Address</p>
              <div className="grid grid-cols-2 gap-x-6 gap-y-4">
                <div className="col-span-2">
                  <FormField label="Address Line 1" defaultValue="Level 3, Tower A, KLCC" />
                </div>
                <FormField label="City" defaultValue="Kuala Lumpur" />
                <FormField label="Postcode" defaultValue="50450" />
                <FormField label="State" as="select"><option>Kuala Lumpur</option><option>Selangor</option><option>Penang</option></FormField>
                <FormField label="Country" as="select"><option>Malaysia</option><option>Singapore</option></FormField>
              </div>
            </div>
          )}

          {tab === 'financial' && (
            <div className="px-5 pt-4 pb-5">
              <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-3">
                <FormField label="Payment Terms" as="select"><option>Net 30</option><option>Net 60</option><option>COD</option></FormField>
                <FormField label="Payment Mode" as="select"><option>Cheque</option><option>Bank Transfer</option><option>Cash</option></FormField>
                <FormField label="Currency" as="select"><option>MYR</option><option>USD</option><option>SGD</option></FormField>
                <FormField label="AR Account" defaultValue="103-000 Trade Receivables" />
                <FormField label="Tax Group" as="select"><option>Standard (0%)</option><option>Exempt</option></FormField>
              </div>
            </div>
          )}

          {tab === 'other' && (
            <div className="px-5 pt-4 pb-5 grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-3">
              <FormField label="Salesperson" defaultValue="ADMIN" />
              <FormField label="Price List" as="select"><option>Standard</option><option>VIP</option></FormField>
              <FormField label="Discount %" defaultValue="0" type="number" />
              <FormField label="Created By" value="ADMIN" readOnly />
              <FormField label="Created Date" value="01/01/2026" readOnly />
            </div>
          )}
        </Card>
      </div>
    )
  }

  return (
    <div className="space-y-5">
      <PageHeader
        title="Customer / Vendor Record"
        description="frmMSTCon"
        actions={
          <>
            <Button variant="primary" size="sm">
              <svg width="12" height="12" viewBox="0 0 16 16" fill="none"><path d="M8 2v12M2 8h12" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/></svg>
              New
            </Button>
            <Button variant="secondary" size="sm" disabled={!selected} onClick={() => setMode('edit')}>Edit</Button>
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
            <input placeholder="Search by code, name, phone…" className="h-8 pl-8 pr-3 w-full rounded-lg border border-border text-sm text-txt-primary focus:outline-none focus:ring-2 focus:ring-brand-500 focus:border-brand-500 bg-bg-muted placeholder:text-txt-tertiary" />
          </div>
          <select className="h-8 px-3 rounded-lg border border-border text-sm text-txt-primary focus:outline-none focus:ring-2 focus:ring-brand-500 bg-bg-muted">
            <option>All Types</option><option>Customer</option><option>Vendor</option>
          </select>
          <select className="h-8 px-3 rounded-lg border border-border text-sm text-txt-primary focus:outline-none focus:ring-2 focus:ring-brand-500 bg-bg-muted">
            <option>Active</option><option>All Status</option><option>Inactive</option>
          </select>
        </div>
        <DataGrid
          columns={LIST_COLS}
          rows={LIST_ROWS}
          rowKey="id"
          selectedKey={selected}
          onRowClick={handleEdit}
        />
        <div className="px-5 py-2.5 border-t border-border text-xs text-txt-tertiary">
          {LIST_ROWS.length} records
        </div>
      </Card>
    </div>
  )
}
