'use client'

import { useState } from 'react'
import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'
import Button from '@/components/common/Button'
import FormField from '@/components/common/FormField'
import DataGrid, { Column } from '@/components/common/DataGrid'
import TabBar from '@/components/common/TabBar'

const LIST_COLS: Column[] = [
  { key: 'accNo',  header: 'Account No.',  width: '130px' },
  { key: 'name',   header: 'Account Name' },
  { key: 'type',   header: 'Type',         width: '130px' },
  { key: 'grp',    header: 'Group',        width: '130px' },
  { key: 'balance', header: 'Balance',     width: '110px', align: 'right' },
  { key: 'status', header: 'Status',       width: '80px' },
]

const LIST_ROWS = [
  { id: '1', accNo: '100-000', name: 'Current Assets',        type: 'Asset',     grp: 'Balance Sheet', balance: '',          status: 'Header' },
  { id: '2', accNo: '101-000', name: 'Cash on Hand',          type: 'Asset',     grp: 'Balance Sheet', balance: '5,200.00',  status: 'Active' },
  { id: '3', accNo: '103-000', name: 'Trade Receivables',     type: 'Asset',     grp: 'Balance Sheet', balance: '48,200.00', status: 'Active' },
  { id: '4', accNo: '113-000', name: 'Inventory',             type: 'Asset',     grp: 'Balance Sheet', balance: '32,100.00', status: 'Active' },
  { id: '5', accNo: '200-000', name: 'Current Liabilities',   type: 'Liability', grp: 'Balance Sheet', balance: '',          status: 'Header' },
  { id: '6', accNo: '201-000', name: 'Trade Payables',        type: 'Liability', grp: 'Balance Sheet', balance: '−18,400.00', status: 'Active' },
  { id: '7', accNo: '401-000', name: 'Sales Revenue',         type: 'Revenue',   grp: 'P&L',           balance: '128,500.00', status: 'Active' },
  { id: '8', accNo: '501-000', name: 'Cost of Goods Sold',    type: 'Expense',   grp: 'P&L',           balance: '89,200.00', status: 'Active' },
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
      <div className="space-y-4">
        <PageHeader
          title="Chart of Account"
          description="frmMSTAcc"
          actions={
            <>
              <Button variant="secondary" size="sm" onClick={() => setMode('list')}>← Back to List</Button>
              <Button variant="primary" size="sm">Save</Button>
              <Button variant="danger" size="sm">Delete</Button>
            </>
          }
        />
        <Card noPad>
          <div className="px-4 pt-2">
            <TabBar tabs={FORM_TABS} active={tab} onChange={setTab} />
          </div>

          {tab === 'general' && (
            <div className="px-4 pt-3 pb-4 space-y-4">
              <div className="grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-4">
                <FormField label="Account No. *" defaultValue={row.accNo} />
                <div className="lg:col-span-2">
                  <FormField label="Account Name *" defaultValue={row.name} />
                </div>
                <FormField label="Account Type" as="select">
                  <option>{row.type}</option>
                  <option>Asset</option>
                  <option>Liability</option>
                  <option>Revenue</option>
                  <option>Expense</option>
                  <option>Equity</option>
                </FormField>
              </div>
              <div className="grid grid-cols-2 gap-x-6 gap-y-3 lg:grid-cols-4">
                <FormField label="Account Group" as="select"><option>{row.grp}</option><option>Balance Sheet</option><option>P&L</option></FormField>
                <FormField label="Branch" as="select"><option>HQ</option><option>Branch A</option></FormField>
                <FormField label="Department" as="select"><option>—</option><option>Sales</option><option>Admin</option></FormField>
                <FormField label="Status" as="select"><option>Active</option><option>Header</option><option>Inactive</option></FormField>
              </div>
              <div className="grid grid-cols-2 gap-x-6 gap-y-3">
                <FormField label="Opening Balance (MYR)" defaultValue={row.balance || '0.00'} />
                <FormField label="Tax Code" as="select"><option>—</option><option>SR (Standard Rated)</option><option>ZRL (Zero Rated)</option></FormField>
              </div>
            </div>
          )}

          {tab === 'budget' && (
            <div className="px-4 pt-3 pb-4">
              <div className="overflow-auto">
                <table className="text-[10pt] font-calibri text-[#404040] border-collapse">
                  <thead>
                    <tr className="bg-[#F3EAE2]">
                      <th className="px-3 py-2 text-left text-[9pt] font-semibold border border-[#E5DDD3]">Period</th>
                      {['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'].map(m => (
                        <th key={m} className="px-3 py-2 text-center text-[9pt] font-semibold border border-[#E5DDD3] w-20">{m}</th>
                      ))}
                    </tr>
                  </thead>
                  <tbody>
                    <tr>
                      <td className="px-3 py-2 border border-[#E5DDD3] font-medium">2026</td>
                      {Array(12).fill(null).map((_, i) => (
                        <td key={i} className="px-1 py-1 border border-[#E5DDD3]">
                          <input defaultValue="10,000" className="w-20 h-7 text-right px-1.5 rounded border border-[#D8CFC4] text-[9pt] font-calibri focus:outline-none focus:ring-1 focus:ring-[#6C4C2C]/40" />
                        </td>
                      ))}
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {tab === 'other' && (
            <div className="px-4 pt-3 pb-4 grid grid-cols-2 gap-x-6 gap-y-3">
              <FormField label="Transaction Group" as="select"><option>General</option><option>Sales</option><option>Purchase</option></FormField>
              <FormField label="Revaluation" as="select"><option>No</option><option>Yes — Bank</option></FormField>
            </div>
          )}
        </Card>
      </div>
    )
  }

  return (
    <div className="space-y-4">
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
            <Button variant="danger" size="sm" disabled={!selected}>Delete</Button>
          </>
        }
      />
      <Card noPad>
        <div className="p-3 border-b border-[#E5DDD3] flex items-center gap-2">
          <input placeholder="Search account no. or name..." className="h-8 px-2.5 rounded border border-[#D8CFC4] text-[9pt] font-calibri w-64 focus:outline-none focus:ring-1 focus:ring-[#6C4C2C]/40" />
          <select className="h-8 px-2 rounded border border-[#D8CFC4] text-[9pt] font-calibri text-[#404040] focus:outline-none">
            <option>All Types</option>
            <option>Asset</option>
            <option>Liability</option>
            <option>Revenue</option>
            <option>Expense</option>
            <option>Equity</option>
          </select>
        </div>
        <DataGrid columns={LIST_COLS} rows={LIST_ROWS} rowKey="id" selectedKey={selected} onRowClick={row => { setSelected(row.id as string); setMode('edit') }} />
        <div className="px-3 py-2 border-t border-[#E5DDD3] text-[9pt] font-calibri text-[#888]">{LIST_ROWS.length} accounts</div>
      </Card>
    </div>
  )
}
