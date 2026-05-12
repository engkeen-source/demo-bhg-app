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
  { key: 'accNo',  header: 'Account No.',  width: '130px' },
  { key: 'name',   header: 'Account Name' },
  { key: 'type',   header: 'Type',         width: '120px' },
  { key: 'grp',    header: 'Group',        width: '120px' },
  { key: 'balance', header: 'Balance',     width: '110px', align: 'right' },
  { key: 'status', header: 'Status',       width: '80px',
    render: (row) => {
      const s = row.status as string
      const v = s === 'Active' ? 'active' : s === 'Header' ? 'draft' : 'inactive'
      return <Badge variant={v}>{s}</Badge>
    }
  },
]

const LIST_ROWS = [
  { id: '1', accNo: '100-000', name: 'Current Assets',        type: 'Asset',     grp: 'Balance Sheet', balance: '',           status: 'Header' },
  { id: '2', accNo: '101-000', name: 'Cash on Hand',          type: 'Asset',     grp: 'Balance Sheet', balance: '5,200.00',   status: 'Active' },
  { id: '3', accNo: '103-000', name: 'Trade Receivables',     type: 'Asset',     grp: 'Balance Sheet', balance: '48,200.00',  status: 'Active' },
  { id: '4', accNo: '113-000', name: 'Inventory',             type: 'Asset',     grp: 'Balance Sheet', balance: '32,100.00',  status: 'Active' },
  { id: '5', accNo: '200-000', name: 'Current Liabilities',   type: 'Liability', grp: 'Balance Sheet', balance: '',           status: 'Header' },
  { id: '6', accNo: '201-000', name: 'Trade Payables',        type: 'Liability', grp: 'Balance Sheet', balance: '−18,400.00', status: 'Active' },
  { id: '7', accNo: '401-000', name: 'Sales Revenue',         type: 'Revenue',   grp: 'P&L',           balance: '128,500.00', status: 'Active' },
  { id: '8', accNo: '501-000', name: 'Cost of Goods Sold',    type: 'Expense',   grp: 'P&L',           balance: '89,200.00',  status: 'Active' },
]

const FORM_TABS = [
  { id: 'general', label: 'General' },
  { id: 'budget',  label: 'Budget' },
  { id: 'other',   label: 'Other' },
]

export default function AccountPage() {
  const [selected, setSelected] = useState<string | undefined>()
  const [tab, setTab] = useState('general')
  const [mode, setMode] = useState<'list' | 'edit'>('list')

  if (mode === 'edit') {
    const row = LIST_ROWS.find(r => r.id === selected) ?? LIST_ROWS[1]
    return (
      <div className="space-y-5">
        <PageHeader
          title="Chart of Account"
          description="frmMSTAcc"
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
                <FormField label="Account No. *" defaultValue={row.accNo} />
                <div className="lg:col-span-2">
                  <FormField label="Account Name *" defaultValue={row.name} />
                </div>
                <FormField label="Account Type" as="select">
                  <option>{row.type}</option>
                  <option>Asset</option><option>Liability</option><option>Revenue</option><option>Expense</option><option>Equity</option>
                </FormField>
              </div>
              <div className="grid grid-cols-2 gap-x-6 gap-y-4 lg:grid-cols-4">
                <FormField label="Account Group" as="select"><option>{row.grp}</option><option>Balance Sheet</option><option>P&L</option></FormField>
                <FormField label="Branch" as="select"><option>HQ</option><option>Branch A</option></FormField>
                <FormField label="Department" as="select"><option>—</option><option>Sales</option><option>Admin</option></FormField>
                <FormField label="Status" as="select"><option>Active</option><option>Header</option><option>Inactive</option></FormField>
              </div>
              <div className="grid grid-cols-2 gap-x-6 gap-y-4">
                <FormField label="Opening Balance (MYR)" defaultValue={row.balance || '0.00'} />
                <FormField label="Tax Code" as="select"><option>—</option><option>SR (Standard Rated)</option><option>ZRL (Zero Rated)</option></FormField>
              </div>
            </div>
          )}

          {tab === 'budget' && (
            <div className="px-5 pt-4 pb-5 overflow-auto">
              <table className="text-sm text-txt-primary border-collapse w-full">
                <thead>
                  <tr className="bg-bg-muted">
                    <th className="px-4 py-2.5 text-left text-xs font-semibold text-txt-secondary uppercase tracking-wide border border-border">Period</th>
                    {['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'].map(m => (
                      <th key={m} className="px-2 py-2.5 text-center text-xs font-semibold text-txt-secondary uppercase tracking-wide border border-border w-20">{m}</th>
                    ))}
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <td className="px-4 py-2 border border-border font-medium text-txt-primary">2026</td>
                    {Array(12).fill(null).map((_, i) => (
                      <td key={i} className="px-1 py-1 border border-border">
                        <input
                          defaultValue="10,000"
                          className="w-20 h-8 text-right px-2 rounded-lg border border-border text-xs text-txt-primary tabular-nums focus:outline-none focus:ring-2 focus:ring-brand-500 bg-bg-surface"
                        />
                      </td>
                    ))}
                  </tr>
                </tbody>
              </table>
            </div>
          )}

          {tab === 'other' && (
            <div className="px-5 pt-4 pb-5 grid grid-cols-2 gap-x-6 gap-y-4">
              <FormField label="Transaction Group" as="select"><option>General</option><option>Sales</option><option>Purchase</option></FormField>
              <FormField label="Revaluation" as="select"><option>No</option><option>Yes — Bank</option></FormField>
            </div>
          )}
        </Card>
      </div>
    )
  }

  return (
    <div className="space-y-5">
      <PageHeader
        title="Chart of Account"
        description="frmMSTAcc"
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
            <input placeholder="Search account no. or name…" className="h-8 pl-8 pr-3 w-full rounded-lg border border-border text-sm text-txt-primary focus:outline-none focus:ring-2 focus:ring-brand-500 bg-bg-muted placeholder:text-txt-tertiary" />
          </div>
          <select className="h-8 px-3 rounded-lg border border-border text-sm text-txt-primary focus:outline-none focus:ring-2 focus:ring-brand-500 bg-bg-muted">
            <option>All Types</option><option>Asset</option><option>Liability</option><option>Revenue</option><option>Expense</option><option>Equity</option>
          </select>
        </div>
        <DataGrid
          columns={LIST_COLS}
          rows={LIST_ROWS}
          rowKey="id"
          selectedKey={selected}
          onRowClick={row => { setSelected(row.id as string); setMode('edit') }}
        />
        <div className="px-5 py-2.5 border-t border-border text-xs text-txt-tertiary">{LIST_ROWS.length} accounts</div>
      </Card>
    </div>
  )
}
