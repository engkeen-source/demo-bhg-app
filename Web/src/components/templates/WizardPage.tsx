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
    <div className="space-y-4 max-w-2xl">
      <PageHeader title={title} />

      {/* Step indicators */}
      <div className="flex items-center gap-0">
        {STEPS.map((s, i) => (
          <div key={s} className="flex items-center">
            <div className={[
              'w-7 h-7 rounded-full flex items-center justify-center text-[9pt] font-semibold font-calibri transition-colors',
              i < step ? 'bg-[#6C4C2C] text-white' : i === step ? 'bg-[#6C4C2C] text-white ring-2 ring-[#6C4C2C]/30' : 'bg-[#E5DDD3] text-[#888]',
            ].join(' ')}>
              {i < step ? '✓' : i + 1}
            </div>
            <span className={`mx-1 text-[9pt] font-calibri ${i === step ? 'text-[#404040] font-semibold' : 'text-[#888]'}`}>{s}</span>
            {i < STEPS.length - 1 && <div className="w-8 h-px bg-[#E5DDD3] mx-1" />}
          </div>
        ))}
      </div>

      <Card accent>
        {step === 0 && (
          <div className="space-y-4">
            <p className="text-[10pt] font-semibold font-calibri text-[#404040]">Step 1: Select Source</p>
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
            <p className="text-[10pt] font-semibold font-calibri text-[#404040]">Step 2: Options</p>
            <div className="flex gap-6">
              <label className="flex items-center gap-2 text-[10pt] font-calibri cursor-pointer">
                <input type="checkbox" defaultChecked className="accent-[#6C4C2C]" /> Copy line items
              </label>
              <label className="flex items-center gap-2 text-[10pt] font-calibri cursor-pointer">
                <input type="checkbox" defaultChecked className="accent-[#6C4C2C]" /> Copy remarks
              </label>
              <label className="flex items-center gap-2 text-[10pt] font-calibri cursor-pointer">
                <input type="checkbox" className="accent-[#6C4C2C]" /> Recalculate prices
              </label>
            </div>
          </div>
        )}
        {step === 2 && (
          <div className="space-y-2">
            <p className="text-[10pt] font-semibold font-calibri text-[#404040]">Step 3: Preview</p>
            <div className="rounded border border-[#E5DDD3] overflow-hidden">
              <table className="w-full text-[10pt] font-calibri">
                <thead className="bg-[#F3EAE2]">
                  <tr>
                    <th className="px-3 py-2 text-left text-[9pt] font-semibold">Item</th>
                    <th className="px-3 py-2 text-right text-[9pt] font-semibold">Qty</th>
                    <th className="px-3 py-2 text-right text-[9pt] font-semibold">Unit Price</th>
                  </tr>
                </thead>
                <tbody>
                  <tr className="border-t border-[#E5DDD3]">
                    <td className="px-3 py-2">Product A</td>
                    <td className="px-3 py-2 text-right">10</td>
                    <td className="px-3 py-2 text-right">150.00</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        )}
        {step === 3 && (
          <div className="space-y-2">
            <p className="text-[10pt] font-semibold font-calibri text-[#6C4C2C]">Ready to create document.</p>
            <p className="text-[9pt] font-calibri text-[#888]">Click "Finish" to create the new document from the source.</p>
          </div>
        )}

        <div className="mt-6 flex gap-2">
          {step > 0 && <Button variant="secondary" onClick={() => setStep(s => s - 1)}>Back</Button>}
          {step < STEPS.length - 1
            ? <Button variant="primary" onClick={() => setStep(s => s + 1)}>Next</Button>
            : <Button variant="primary">Finish</Button>
          }
        </div>
      </Card>

      {desktop && (
        <p className="text-[8pt] font-calibri text-[#AAA]">
          Phase 2: connects to <code className="bg-[#F3EAE2] px-1 rounded">{desktop}</code>
        </p>
      )}
    </div>
  )
}
