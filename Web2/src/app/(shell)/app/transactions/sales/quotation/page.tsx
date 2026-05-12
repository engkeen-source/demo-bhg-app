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
  { key: 'no',        header: '#',          width: '36px', align: 'center' },
  { key: 'itemCode',  header: 'Item Code',  width: '110px' },
  { key: 'desc',      header: 'Description' },
  { key: 'qty',       header: 'Qty',        width: '70px', align: 'right' },
  { key: 'uom',       header: 'UOM',        width: '55px' },
  { key: 'unitPrice', header: 'Unit Price', width: '100px', align: 'right' },
  { key: 'disc',      header: 'Disc %',     width: '65px', align: 'right' },
  { key: 'amount',    header: 'Amount',     width: '110px', align: 'right' },
]

const MOCK_LINES = [
  { id: '1', no: '1', itemCode: 'ITM-002', desc: 'Office Chair — Ergonomic Series B', qty: '10', uom: 'PCS', unitPrice: '480.00', disc: '10.00', amount: '4,320.00' },
  { id: '2', no: '2', itemCode: 'ITM-005', desc: 'Standing Desk 140cm x 70cm', qty: '5', uom: 'PCS', unitPrice: '1,200.00', disc: '5.00', amount: '5,700.00' },
]

const TABS = [
  { id: 'main',    label: 'Main' },
  { id: 'item',    label: 'Items' },
  { id: 'terms',   label: 'Terms & Conditions' },
  { id: 'address', label: 'Address' },
  { id: 'other',   label: 'Other' },
  { id: 'aging',   label: 'AR Aging' },
]

export default function QuotationPage() {
  const [tab, setTab] = useState('main')

  return (
    <div className="space-y-5">
      <PageHeader
        title="Quotation"
        description="frmARQO — Quotation entry and management"
        actions={<Badge variant="draft">Pending</Badge>}
      />

      <Card noPad>
        <div className="px-5 py-3 border-b border-border">
          <DocToolbar />
        </div>

        <div className="px-5 pt-3">
          <TabBar tabs={TABS} active={tab} onChange={setTab} />
        </div>

        {tab === 'main' && (
          <div className="px-5 pt-4 pb-5 grid grid-cols-1 lg:grid-cols-2 gap-6">
            {/* Left column */}
            <div className="space-y-4">
              <SectionCard title="Document">
                <FormField label="Document State" value="New" readOnly />
                <FormField label="Document Date" type="date" defaultValue="2026-05-12" />
                <FormField label="Document ID" placeholder="" />
                <FormField label="Document Type *" as="select">
                  <option>Quotation</option><option>Sales Order</option>
                </FormField>
                <FormField label="Document Group" as="select">
                  <option value="">— Select —</option><option>Group A</option>
                </FormField>
              </SectionCard>

              <SectionCard title="Dates &amp; References">
                <FormField label="Enquiry Date" type="date" />
                <FormField label="Valid Date" type="date" defaultValue="2026-05-26" />
                <FormField label="Quotation Status" as="select">
                  <option>Pending</option><option>Approved</option><option>Won</option><option>Lost</option>
                </FormField>
                <FormField label="Customer PO #" placeholder="Optional" />
                <FormField label="Sales Order #" placeholder="Optional" />
              </SectionCard>

              <SectionCard title="Accounts">
                <FormField label="AR Account *" as="select">
                  <option value="">— Select —</option><option>AR-001</option>
                </FormField>
                <FormField label="AR Account Name" readOnly placeholder="" />
                <FormField label="Remarks" as="textarea" placeholder="Optional remarks…" />
              </SectionCard>
            </div>

            {/* Right column */}
            <div className="space-y-4">
              <SectionCard title="Customer">
                <FormField label="Customer ID *" placeholder="Search customer…" />
                <FormField label="Customer Name *" placeholder="Search name…" />
                <FormField label="Representative *" as="select">
                  <option value="">— Select —</option><option>REP-001</option>
                </FormField>
                <FormField label="Attention" placeholder="Optional" />
              </SectionCard>

              <SectionCard title="Currency &amp; Pricing">
                <div className="grid grid-cols-2 gap-3">
                  <FormField label="Currency" as="select">
                    <option>SGD</option><option>MYR</option><option>USD</option>
                  </FormField>
                  <FormField label="Currency Rate" defaultValue="1.0000" />
                </div>
                <div className="grid grid-cols-2 gap-3">
                  <FormField label="Price Type" as="select">
                    <option value="">— Select —</option><option>Retail</option>
                  </FormField>
                  <FormField label="Terms" as="select">
                    <option value="">— Select —</option><option>Net 30</option>
                  </FormField>
                </div>
              </SectionCard>

              <SectionCard title="Totals">
                <div className="space-y-2">
                  <TotalRow label="Sub Total" value="0.00" />
                  <TotalRow label="Discount" value="0.00" />
                  <TotalRow label="Tax" value="0.00" />
                  <div className="border-t border-border pt-2">
                    <TotalRow label="Grand Total (SGD)" value="0.00" bold />
                  </div>
                </div>
              </SectionCard>
            </div>
          </div>
        )}

        {tab === 'item' && (
          <div className="px-5 py-4">
            <div className="flex items-center justify-between mb-3">
              <span className="text-sm font-semibold text-txt-primary">Line Items</span>
              <button className="text-xs text-brand-600 hover:text-brand-700 font-medium">+ Add Line</button>
            </div>
            <div className="rounded-xl border border-border overflow-hidden">
              <DataGrid columns={LINE_COLS} rows={MOCK_LINES} />
            </div>
            <div className="flex justify-end mt-4">
              <div className="w-64 space-y-2">
                <TotalRow label="Sub Total" value="10,020.00" />
                <TotalRow label="Discount" value="720.00" />
                <div className="border-t border-border pt-2">
                  <TotalRow label="Total (SGD)" value="9,300.00" bold />
                </div>
              </div>
            </div>
          </div>
        )}

        {['terms', 'address', 'other', 'aging'].includes(tab) && (
          <div className="px-5 py-16 text-center">
            <p className="text-sm text-txt-tertiary">{TABS.find(t => t.id === tab)?.label} — available in Phase 2</p>
          </div>
        )}
      </Card>
    </div>
  )
}

function SectionCard({ title, children }: { title: string; children: React.ReactNode }) {
  return (
    <div className="rounded-xl border border-border bg-bg-muted/40 p-4 space-y-3">
      <p className="text-xs font-semibold text-txt-tertiary uppercase tracking-wide">{title}</p>
      {children}
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
