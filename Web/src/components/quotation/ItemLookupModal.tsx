'use client'

import { useState, useEffect, useRef } from 'react'
import { searchItems, getLineTypes } from '@/lib/api'
import type { ItemResult, LineTypeRow } from '@/lib/api'

interface Props {
  /** 'item' = search real items; 'linetype' = show structural line types */
  mode: 'item' | 'linetype'
  initialQuery?: string
  searchField?: 'item_id' | 'description' | 'both'
  onSelect: (row: ItemResult | (LineTypeRow & { id?: undefined })) => void
  onClose: () => void
}

type Row = ItemResult | (LineTypeRow & { id?: undefined; qty_stock?: number; uom?: string; bin_location?: string; class_?: string; category?: string; brand?: string; country?: string; hs_code?: string })

export default function ItemLookupModal({ mode, initialQuery = '', searchField = 'both', onSelect, onClose }: Props) {
  const [query, setQuery] = useState(initialQuery)
  const [rows, setRows] = useState<Row[]>([])
  const [loading, setLoading] = useState(false)
  const inputRef = useRef<HTMLInputElement>(null)

  useEffect(() => { inputRef.current?.focus() }, [])

  useEffect(() => {
    if (mode === 'linetype') {
      setLoading(true)
      getLineTypes().then(r => { setRows(r as Row[]); setLoading(false) }).catch(() => setLoading(false))
      return
    }
    if (!query.trim()) { setRows([]); return }
    const t = setTimeout(() => {
      setLoading(true)
      searchItems(query, searchField)
        .then(r => { setRows(r); setLoading(false) })
        .catch(() => setLoading(false))
    }, 250)
    return () => clearTimeout(t)
  }, [query, mode, searchField])

  function handleKey(e: React.KeyboardEvent) {
    if (e.key === 'Escape') onClose()
  }

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-black/40"
      onKeyDown={handleKey}
    >
      <div className="bg-white rounded shadow-xl w-[900px] max-w-[96vw] max-h-[80vh] flex flex-col">
        {/* Header */}
        <div className="flex items-center justify-between px-4 py-3 border-b border-[#E5DDD3]">
          <span className="text-[10pt] font-semibold font-calibri text-[#404040]">
            {mode === 'linetype' ? 'Select Line Type' : 'Stock Details'}
          </span>
          <button
            onClick={onClose}
            className="text-[#888] hover:text-[#404040] text-lg leading-none"
          >×</button>
        </div>

        {/* Search bar (item mode only) */}
        {mode === 'item' && (
          <div className="px-4 py-2 border-b border-[#E5DDD3] flex items-center gap-2">
            <span className="text-[9pt] font-calibri text-[#888] whitespace-nowrap">Item Name</span>
            <input
              ref={inputRef}
              value={query}
              onChange={e => setQuery(e.target.value)}
              className="flex-1 h-8 rounded border border-[#D8CFC4] bg-white px-2.5 text-[10pt] font-calibri text-[#404040] focus:outline-none focus:ring-2 focus:ring-[#6C4C2C]/30 focus:border-[#6C4C2C]"
              placeholder="Search by item ID or description…"
            />
            <button
              onClick={() => {
                setLoading(true)
                searchItems(query, searchField)
                  .then(r => { setRows(r); setLoading(false) })
                  .catch(() => setLoading(false))
              }}
              className="h-8 px-3 rounded border border-[#D8CFC4] bg-white text-[10pt] font-calibri text-[#6C4C2C] hover:bg-[#F3EAE2] transition-colors"
            >
              🔍
            </button>
          </div>
        )}

        {/* Table */}
        <div className="flex-1 overflow-auto">
          <table className="w-full text-[9pt] font-calibri border-collapse min-w-[800px]">
            <thead className="sticky top-0 bg-[#F8F3EE] border-b border-[#E5DDD3]">
              <tr>
                <th className="w-16 px-2 py-2 text-left text-[#6C4C2C] font-semibold"></th>
                <th className="px-2 py-2 text-left text-[#6C4C2C] font-semibold">Item ID</th>
                <th className="px-2 py-2 text-left text-[#6C4C2C] font-semibold">Description</th>
                {mode === 'item' && (<>
                  <th className="px-2 py-2 text-left text-[#6C4C2C] font-semibold">Bin Location</th>
                  <th className="px-2 py-2 text-right text-[#6C4C2C] font-semibold">Stock Qty</th>
                  <th className="px-2 py-2 text-left text-[#6C4C2C] font-semibold">UOM</th>
                </>)}
                <th className="px-2 py-2 text-left text-[#6C4C2C] font-semibold">Type</th>
                {mode === 'item' && (<>
                  <th className="px-2 py-2 text-left text-[#6C4C2C] font-semibold">Class</th>
                  <th className="px-2 py-2 text-left text-[#6C4C2C] font-semibold">Category</th>
                  <th className="px-2 py-2 text-left text-[#6C4C2C] font-semibold">Brand</th>
                  <th className="px-2 py-2 text-left text-[#6C4C2C] font-semibold">Country</th>
                  <th className="px-2 py-2 text-left text-[#6C4C2C] font-semibold">HSCode</th>
                </>)}
              </tr>
            </thead>
            <tbody>
              {loading && (
                <tr><td colSpan={12} className="text-center py-6 text-[#888]">Loading…</td></tr>
              )}
              {!loading && rows.length === 0 && (
                <tr><td colSpan={12} className="text-center py-6 text-[#888]">
                  {mode === 'item' ? 'Enter a search term above.' : 'No line types found.'}
                </td></tr>
              )}
              {rows.map((row, i) => (
                <tr
                  key={i}
                  className="border-b border-[#F0E8E0] hover:bg-[#FBF6F1] cursor-pointer"
                  onClick={() => onSelect(row)}
                >
                  <td className="px-2 py-1.5">
                    <button
                      onClick={e => { e.stopPropagation(); onSelect(row) }}
                      className="text-[#2563EB] hover:underline text-[9pt]"
                    >Select</button>
                  </td>
                  <td className="px-2 py-1.5 text-[#404040]">{'code' in row ? row.code : row.item_code}</td>
                  <td className="px-2 py-1.5 text-[#404040] max-w-[220px] truncate">{row.description}</td>
                  {mode === 'item' && (<>
                    <td className="px-2 py-1.5 text-[#888]">{'bin_location' in row ? (row.bin_location ?? '') : ''}</td>
                    <td className="px-2 py-1.5 text-right text-[#404040]">{'qty_stock' in row ? (row.qty_stock ?? 0) : ''}</td>
                    <td className="px-2 py-1.5 text-[#888]">{'uom' in row ? (row.uom ?? '') : ''}</td>
                  </>)}
                  <td className="px-2 py-1.5 text-[#888]">{'item_type' in row ? row.item_type : ''}</td>
                  {mode === 'item' && (<>
                    <td className="px-2 py-1.5 text-[#888]">{'class_' in row ? (row.class_ ?? '') : ''}</td>
                    <td className="px-2 py-1.5 text-[#888]">{'category' in row ? (row.category ?? '') : ''}</td>
                    <td className="px-2 py-1.5 text-[#888]">{'brand' in row ? (row.brand ?? '') : ''}</td>
                    <td className="px-2 py-1.5 text-[#888]">{'country' in row ? (row.country ?? '') : ''}</td>
                    <td className="px-2 py-1.5 text-[#888]">{'hs_code' in row ? (row.hs_code ?? '') : ''}</td>
                  </>)}
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {/* Footer */}
        <div className="px-4 py-2 border-t border-[#E5DDD3] flex justify-between items-center">
          <span className="text-[9pt] font-calibri text-[#888]">
            {rows.length > 0 ? `${rows.length} item${rows.length !== 1 ? 's' : ''}` : ''}
          </span>
          <button
            onClick={onClose}
            className="h-8 px-4 rounded border border-[#D8CFC4] bg-white text-[10pt] font-calibri text-[#404040] hover:bg-[#F3EAE2] transition-colors"
          >Close</button>
        </div>
      </div>
    </div>
  )
}
