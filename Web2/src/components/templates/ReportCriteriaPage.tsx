'use client'

import { useState } from 'react'
import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'
import Button from '@/components/common/Button'
import FormField from '@/components/common/FormField'
import Badge from '@/components/common/Badge'

interface Props {
  title: string
  desktop?: string
}

export default function ReportCriteriaPage({ title, desktop }: Props) {
  const [generating, setGenerating] = useState(false)
  const [generated, setGenerated] = useState(false)

  function handleGenerate() {
    setGenerating(true)
    setGenerated(false)
    setTimeout(() => { setGenerating(false); setGenerated(true) }, 1200)
  }

  return (
    <div className="space-y-5">
      <PageHeader title={title} description="Set criteria and generate the report." />

      <Card accent>
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
          <FormField label="Date From" type="date" defaultValue="2026-01-01" />
          <FormField label="Date To" type="date" defaultValue="2026-05-12" />
          <FormField label="Customer" as="select">
            <option value="">All Customers</option>
            <option>BossSO Trading Sdn Bhd</option>
            <option>BossSO Retail Sdn Bhd</option>
          </FormField>
          <FormField label="Document Status" as="select">
            <option value="">All</option>
            <option>Open</option>
            <option>Closed</option>
            <option>Cancelled</option>
          </FormField>
          <FormField label="Doc No. From" placeholder="Optional" />
          <FormField label="Doc No. To" placeholder="Optional" />
        </div>

        <div className="mt-6 flex items-center gap-2">
          <Button variant="primary" onClick={handleGenerate} disabled={generating}>
            {generating ? (
              <>
                <svg className="animate-spin" width="13" height="13" viewBox="0 0 24 24" fill="none">
                  <circle cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="2" strokeDasharray="32" strokeDashoffset="32" className="opacity-25"/>
                  <path d="M12 2a10 10 0 0110 10" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/>
                </svg>
                Generating…
              </>
            ) : 'Generate Report'}
          </Button>
          <Button variant="ghost">Export to Excel</Button>
          <Button variant="ghost">Print</Button>
        </div>
      </Card>

      {generated && (
        <Card noPad>
          <div className="px-5 py-3 border-b border-border flex items-center justify-between">
            <span className="text-sm font-semibold text-txt-primary">Report Preview</span>
            <span className="text-xs text-txt-tertiary">3 records</span>
          </div>
          <table className="w-full text-sm text-txt-primary border-collapse">
            <thead>
              <tr className="bg-bg-muted border-b border-border">
                <th className="px-4 py-2.5 text-left text-xs font-semibold text-txt-secondary uppercase tracking-wide">Doc No.</th>
                <th className="px-4 py-2.5 text-left text-xs font-semibold text-txt-secondary uppercase tracking-wide">Date</th>
                <th className="px-4 py-2.5 text-left text-xs font-semibold text-txt-secondary uppercase tracking-wide">Customer</th>
                <th className="px-4 py-2.5 text-right text-xs font-semibold text-txt-secondary uppercase tracking-wide">Amount</th>
                <th className="px-4 py-2.5 text-center text-xs font-semibold text-txt-secondary uppercase tracking-wide">Status</th>
              </tr>
            </thead>
            <tbody>
              {[
                { doc: 'DOC-0001', date: '12/01/2026', customer: 'BossSO Trading Sdn Bhd', amount: '3,140.00', status: 'Open' },
                { doc: 'DOC-0002', date: '14/02/2026', customer: 'BossSO Retail Sdn Bhd', amount: '1,850.00', status: 'Closed' },
                { doc: 'DOC-0003', date: '03/03/2026', customer: 'BossSO Holdings Berhad', amount: '5,200.00', status: 'Open' },
              ].map((row, i) => (
                <tr key={i} className="border-b border-border last:border-0 hover:bg-bg-muted transition-colors">
                  <td className="px-4 py-3 text-brand-600 font-medium">{row.doc}</td>
                  <td className="px-4 py-3 text-txt-secondary">{row.date}</td>
                  <td className="px-4 py-3">{row.customer}</td>
                  <td className="px-4 py-3 text-right tabular-nums">{row.amount}</td>
                  <td className="px-4 py-3 text-center">
                    <Badge variant={row.status === 'Open' ? 'open' : 'posted'}>{row.status}</Badge>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </Card>
      )}

      {desktop && (
        <p className="text-xs text-txt-tertiary">
          Phase 2: connects to <code className="bg-bg-muted px-1.5 py-0.5 rounded font-mono text-txt-secondary">{desktop}</code>
        </p>
      )}
    </div>
  )
}
