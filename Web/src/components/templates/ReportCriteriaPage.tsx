'use client'

import { useState } from 'react'
import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'
import Button from '@/components/common/Button'
import FormField from '@/components/common/FormField'

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
    <div className="space-y-4">
      <PageHeader title={title} description="Set criteria and generate the report." />

      <Card accent>
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
          <FormField label="Date From" type="date" defaultValue="2026-01-01" />
          <FormField label="Date To" type="date" defaultValue="2026-05-12" />
          <FormField
            label="Customer"
            as="select"
          >
            <option value="">All Customers</option>
            <option>BossSO Trading Sdn Bhd</option>
            <option>BossSO Retail Sdn Bhd</option>
          </FormField>
          <FormField
            label="Document Status"
            as="select"
          >
            <option value="">All</option>
            <option>Open</option>
            <option>Closed</option>
            <option>Cancelled</option>
          </FormField>
          <FormField label="Doc No. From" placeholder="Optional" />
          <FormField label="Doc No. To" placeholder="Optional" />
        </div>

        <div className="mt-5 flex items-center gap-3">
          <Button variant="primary" onClick={handleGenerate} disabled={generating}>
            {generating ? 'Generating…' : 'Generate Report'}
          </Button>
          <Button variant="ghost">Export to Excel</Button>
          <Button variant="ghost">Print</Button>
        </div>
      </Card>

      {generated && (
        <Card noPad>
          <div className="px-4 py-3 border-b border-[#E5DDD3] flex items-center justify-between">
            <span className="text-[10pt] font-semibold font-calibri text-[#404040]">Report Preview</span>
            <span className="text-[9pt] font-calibri text-[#888]">3 records</span>
          </div>
          <table className="w-full text-[10pt] font-calibri text-[#404040] border-collapse">
            <thead>
              <tr className="bg-[#F3EAE2] border-b border-[#E5DDD3]">
                <th className="px-3 py-2 text-left text-[9pt] font-semibold">Doc No.</th>
                <th className="px-3 py-2 text-left text-[9pt] font-semibold">Date</th>
                <th className="px-3 py-2 text-left text-[9pt] font-semibold">Customer</th>
                <th className="px-3 py-2 text-right text-[9pt] font-semibold">Amount</th>
                <th className="px-3 py-2 text-center text-[9pt] font-semibold">Status</th>
              </tr>
            </thead>
            <tbody>
              {[
                { doc: 'DOC-0001', date: '12/01/2026', customer: 'BossSO Trading Sdn Bhd', amount: '3,140.00', status: 'Open' },
                { doc: 'DOC-0002', date: '14/02/2026', customer: 'BossSO Retail Sdn Bhd', amount: '1,850.00', status: 'Closed' },
                { doc: 'DOC-0003', date: '03/03/2026', customer: 'BossSO Holdings Berhad', amount: '5,200.00', status: 'Open' },
              ].map((row, i) => (
                <tr key={i} className={`border-b border-[#E5DDD3] ${i % 2 === 1 ? 'bg-[#FAF8F5]' : 'bg-white'}`}>
                  <td className="px-3 py-2 text-[#6C4C2C] font-medium">{row.doc}</td>
                  <td className="px-3 py-2">{row.date}</td>
                  <td className="px-3 py-2">{row.customer}</td>
                  <td className="px-3 py-2 text-right tabular-nums">{row.amount}</td>
                  <td className="px-3 py-2 text-center">
                    <span className={`px-2 py-0.5 rounded text-[8pt] font-medium ${row.status === 'Open' ? 'bg-blue-50 text-blue-700' : 'bg-green-50 text-green-700'}`}>
                      {row.status}
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </Card>
      )}

      {desktop && (
        <p className="text-[8pt] font-calibri text-[#AAA]">
          Phase 2: connects to <code className="bg-[#F3EAE2] px-1 rounded">{desktop}</code>
        </p>
      )}
    </div>
  )
}
