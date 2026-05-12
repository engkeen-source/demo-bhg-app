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

const STEPS = ['Source', 'Options', 'Preview', 'Confirm']

export default function WizardPage({ title, desktop }: Props) {
  const [step, setStep] = useState(0)

  return (
    <div className="space-y-5 max-w-2xl">
      <PageHeader title={title} />

      {/* Step indicators */}
      <div className="flex items-center">
        {STEPS.map((s, i) => (
          <div key={s} className="flex items-center flex-1 last:flex-none">
            <div className="flex items-center gap-2">
              <div className={[
                'w-7 h-7 rounded-full flex items-center justify-center text-xs font-semibold transition-all',
                i < step ? 'bg-brand-600 text-white' :
                i === step ? 'bg-brand-600 text-white ring-4 ring-brand-100' :
                'bg-bg-muted text-txt-tertiary border border-border',
              ].join(' ')}>
                {i < step ? (
                  <svg width="12" height="12" viewBox="0 0 16 16" fill="none"><path d="M3 8l3 3 7-7" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/></svg>
                ) : i + 1}
              </div>
              <span className={`text-xs font-medium ${i === step ? 'text-txt-primary' : i < step ? 'text-brand-600' : 'text-txt-tertiary'}`}>{s}</span>
            </div>
            {i < STEPS.length - 1 && (
              <div className={`flex-1 mx-3 h-px ${i < step ? 'bg-brand-500' : 'bg-border'} transition-colors`} />
            )}
          </div>
        ))}
      </div>

      <Card accent>
        {step === 0 && (
          <div className="space-y-4">
            <p className="text-sm font-semibold text-txt-primary">Step 1: Select Source</p>
            <FormField as="select" label="Source Document Type">
              <option>Sales Order</option>
              <option>Quotation</option>
              <option>Purchase Order</option>
            </FormField>
            <FormField label="Source Doc No." placeholder="e.g. SO-0001" />
          </div>
        )}
        {step === 1 && (
          <div className="space-y-4">
            <p className="text-sm font-semibold text-txt-primary">Step 2: Options</p>
            <div className="flex flex-col gap-3">
              {['Copy line items', 'Copy remarks', 'Recalculate prices'].map((opt, i) => (
                <label key={opt} className="flex items-center gap-3 cursor-pointer">
                  <input type="checkbox" defaultChecked={i < 2} className="w-4 h-4 accent-brand-600" />
                  <span className="text-sm text-txt-primary">{opt}</span>
                </label>
              ))}
            </div>
          </div>
        )}
        {step === 2 && (
          <div className="space-y-3">
            <p className="text-sm font-semibold text-txt-primary">Step 3: Preview</p>
            <div className="rounded-xl border border-border overflow-hidden">
              <table className="w-full text-sm text-txt-primary">
                <thead className="bg-bg-muted">
                  <tr>
                    <th className="px-4 py-2.5 text-left text-xs font-semibold text-txt-secondary uppercase tracking-wide">Item</th>
                    <th className="px-4 py-2.5 text-right text-xs font-semibold text-txt-secondary uppercase tracking-wide">Qty</th>
                    <th className="px-4 py-2.5 text-right text-xs font-semibold text-txt-secondary uppercase tracking-wide">Unit Price</th>
                  </tr>
                </thead>
                <tbody>
                  <tr className="border-t border-border">
                    <td className="px-4 py-3">Product A</td>
                    <td className="px-4 py-3 text-right tabular-nums">10</td>
                    <td className="px-4 py-3 text-right tabular-nums">150.00</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        )}
        {step === 3 && (
          <div className="space-y-3">
            <div className="flex items-center gap-3 p-4 bg-emerald-50 rounded-xl border border-emerald-200">
              <svg width="20" height="20" viewBox="0 0 24 24" fill="none" className="text-emerald-600 shrink-0">
                <circle cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="2"/>
                <path d="M8 12l3 3 5-5" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
              </svg>
              <div>
                <p className="text-sm font-semibold text-emerald-800">Ready to create document</p>
                <p className="text-xs text-emerald-600 mt-0.5">Click "Finish" to create the new document from the source.</p>
              </div>
            </div>
          </div>
        )}

        <div className="mt-6 flex gap-2">
          {step > 0 && <Button variant="secondary" onClick={() => setStep(s => s - 1)}>Back</Button>}
          {step < STEPS.length - 1
            ? <Button variant="primary" onClick={() => setStep(s => s + 1)}>Next →</Button>
            : <Button variant="primary">Finish</Button>
          }
        </div>
      </Card>

      {desktop && (
        <p className="text-xs text-txt-tertiary">
          Phase 2: connects to <code className="bg-bg-muted px-1.5 py-0.5 rounded font-mono text-txt-secondary">{desktop}</code>
        </p>
      )}
    </div>
  )
}
