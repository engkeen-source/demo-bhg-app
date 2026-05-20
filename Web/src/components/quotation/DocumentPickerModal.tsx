'use client'

import { useState, useEffect } from 'react'
import { listQuotations, getQuotation, type QuotationLine, type QuotationHeader } from '@/lib/api'

interface Props {
  onSelect: (lines: QuotationLine[]) => void
  onClose: () => void
}

type QuotationSummary = QuotationHeader & { id: number; lines: QuotationLine[] }

export default function DocumentPickerModal({ onSelect, onClose }: Props) {
  const [loading, setLoading] = useState(true)
  const [quotations, setQuotations] = useState<QuotationSummary[]>([])
  const [selecting, setSelecting] = useState<number | null>(null)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    listQuotations()
      .then(data => setQuotations(data as QuotationSummary[]))
      .catch(() => setError('Failed to load quotations.'))
      .finally(() => setLoading(false))
  }, [])

  async function handleSelect(id: number) {
    setSelecting(id)
    try {
      const doc = await getQuotation(id)
      onSelect(doc.lines)
    } catch {
      setError('Failed to load quotation lines.')
      setSelecting(null)
    }
  }

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/30">
      <div className="bg-white rounded-lg shadow-xl border border-[#E5DDD3] w-[720px] max-h-[70vh] flex flex-col">
        <div className="flex items-center justify-between px-4 py-3 border-b border-[#E5DDD3]">
          <h2 className="text-[10pt] font-semibold font-calibri text-[#6C4C2C]">
            Select Quotation — Insert Lines Above
          </h2>
          <button
            onClick={onClose}
            className="text-[#888] hover:text-[#6C4C2C] text-xl leading-none px-1"
          >×</button>
        </div>

        <div className="overflow-y-auto flex-1 p-3">
          {loading && (
            <p className="text-[9pt] font-calibri text-[#888] text-center py-8">Loading…</p>
          )}
          {error && (
            <p className="text-[9pt] font-calibri text-red-600 text-center py-8">{error}</p>
          )}
          {!loading && !error && quotations.length === 0 && (
            <p className="text-[9pt] font-calibri text-[#888] text-center py-8">
              No saved quotations found.
            </p>
          )}
          {!loading && !error && quotations.length > 0 && (
            <table className="w-full border-collapse text-[9pt] font-calibri">
              <thead>
                <tr className="bg-[#F8F3EE]">
                  <th className="px-2 py-2 text-left font-semibold text-[#6C4C2C] border-b border-[#E5DDD3]">Doc ID</th>
                  <th className="px-2 py-2 text-left font-semibold text-[#6C4C2C] border-b border-[#E5DDD3]">Date</th>
                  <th className="px-2 py-2 text-left font-semibold text-[#6C4C2C] border-b border-[#E5DDD3]">Customer</th>
                  <th className="px-2 py-2 text-left font-semibold text-[#6C4C2C] border-b border-[#E5DDD3]">Status</th>
                  <th className="px-2 py-2 text-right font-semibold text-[#6C4C2C] border-b border-[#E5DDD3]">Grand Total</th>
                  <th className="px-2 py-2 border-b border-[#E5DDD3]"></th>
                </tr>
              </thead>
              <tbody>
                {quotations.map(q => (
                  <tr key={q.id} className="border-b border-[#EDE6DD] hover:bg-[#FFFBF7]">
                    <td className="px-2 py-1.5 text-[#404040]">{q.doc_id ?? `#${q.id}`}</td>
                    <td className="px-2 py-1.5 text-[#404040]">{q.doc_date}</td>
                    <td className="px-2 py-1.5 text-[#404040]">{q.customer_name ?? '—'}</td>
                    <td className="px-2 py-1.5 text-[#888]">{q.quotation_status}</td>
                    <td className="px-2 py-1.5 text-right text-[#404040] tabular-nums">
                      {(q.grand_total ?? 0).toFixed(2)}
                    </td>
                    <td className="px-2 py-1.5">
                      <button
                        disabled={selecting !== null}
                        onClick={() => handleSelect(q.id)}
                        className="h-6 px-3 rounded border border-[#6C4C2C] bg-[#6C4C2C] text-white text-[8pt] font-calibri hover:bg-[#7D5A38] disabled:opacity-50 whitespace-nowrap"
                      >
                        {selecting === q.id ? '…' : 'Select'}
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>

        <div className="px-4 py-2 border-t border-[#E5DDD3] flex justify-end">
          <button
            onClick={onClose}
            className="h-7 px-4 rounded border border-[#D8CFC4] text-[9pt] font-calibri text-[#6C4C2C] hover:bg-[#F3EAE2]"
          >Cancel</button>
        </div>
      </div>
    </div>
  )
}
