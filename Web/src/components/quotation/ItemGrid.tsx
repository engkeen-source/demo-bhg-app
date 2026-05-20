'use client'

import { useState, useCallback, useEffect, useRef } from 'react'
import type { QuotationLine, ItemResult, LineTypeRow, ComputeResult } from '@/lib/api'
import { searchItems } from '@/lib/api'
import { PRICE_FORBIDDEN, QTY_FORBIDDEN, ITEM_OPTIONAL } from './calc'
import ItemLookupModal from './ItemLookupModal'
import DocumentPickerModal from './DocumentPickerModal'

interface Props {
  lines: QuotationLine[]
  subTotal: number
  currency: string
  defaultLocation: string
  onDefaultLocationChange: (v: string) => void
  onChange: (lines: QuotationLine[]) => void
  computedLines: ComputeResult['lines']
}

type ModalState =
  | { open: false }
  | { open: true; mode: 'item'; lineNo: number; field: 'item_id' | 'description'; query: string }
  | { open: true; mode: 'linetype'; lineNo: number }

type DocPickerState = { open: false } | { open: true; lineNo: number }

function fmt(n: number) {
  return n.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',')
}

export function newLine(lineNo: number, location = ''): QuotationLine {
  return {
    line_no: lineNo, line_type: '', item_code: '', description: '',
    qty: 0, direct_ship_qty: 0, uom: '', uom_con_rate: 1, project_cost: 0,
    price: 0, amount: 0, taxable: true, tax_code: 'GST', tax_rate: 9,
    tax_amt: 0, tax_amt_local: 0, stock: 0, latest_cost: 0,
    obsolete_cost: 0, estore_price: 0, location,
  }
}

function resequence(lines: QuotationLine[]): QuotationLine[] {
  return lines.map((l, i) => ({ ...l, line_no: i + 1 }))
}

const tdBase = 'border-r border-[#EDE6DD] px-1.5 py-1 text-[9pt] font-calibri text-[#404040] whitespace-nowrap'
const inputBase = 'w-full bg-transparent border-0 outline-none text-[9pt] font-calibri text-[#404040] focus:bg-[#FFFBF7] focus:ring-1 focus:ring-[#6C4C2C]/30 rounded px-0.5'
const roInputBase = 'w-full bg-[#F3EAE2] border-0 outline-none text-[9pt] font-calibri text-[#888] rounded px-0.5 cursor-not-allowed text-right'
const roText = 'text-[9pt] font-calibri text-[#404040] px-0.5'
const roNum  = 'block w-full text-right text-[9pt] font-calibri text-[#404040] tabular-nums px-0.5'

export default function ItemGrid({ lines, subTotal, currency, defaultLocation, onDefaultLocationChange, onChange, computedLines }: Props) {
  const [modal, setModal]       = useState<ModalState>({ open: false })
  const [openMenu, setOpenMenu] = useState<number | null>(null)
  const [menuPos, setMenuPos]   = useState<{ top: number; left: number } | null>(null)
  const [docPicker, setDocPicker] = useState<DocPickerState>({ open: false })
  const [editingLine, setEditingLine]     = useState<number | null>(null)
  const [editSnapshot, setEditSnapshot]   = useState<QuotationLine | null>(null)
  const [dragOver, setDragOver]           = useState<number | null>(null)
  const dragLineRef = useRef<number | null>(null)

  // Keep refs current for stable keydown handler (avoids re-registering on every render)
  const editingLineRef  = useRef(editingLine)
  const editSnapshotRef = useRef(editSnapshot)
  const linesRef        = useRef(lines)
  const onChangeRef     = useRef(onChange)
  useEffect(() => { editingLineRef.current  = editingLine  }, [editingLine])
  useEffect(() => { editSnapshotRef.current = editSnapshot }, [editSnapshot])
  useEffect(() => { linesRef.current        = lines        }, [lines])
  useEffect(() => { onChangeRef.current     = onChange     }, [onChange])

  // Ctrl+S commits; Escape cancels — while any row is being edited
  useEffect(() => {
    function handler(e: KeyboardEvent) {
      if (editingLineRef.current === null) return
      if (e.ctrlKey && e.key === 's') {
        e.preventDefault()
        setEditingLine(null)
        setEditSnapshot(null)
      } else if (e.key === 'Escape') {
        const snap = editSnapshotRef.current
        if (snap) onChangeRef.current(linesRef.current.map(l => l.line_no === snap.line_no ? { ...snap } : l))
        setEditingLine(null)
        setEditSnapshot(null)
      }
    }
    window.addEventListener('keydown', handler)
    return () => window.removeEventListener('keydown', handler)
  }, []) // intentionally empty — uses refs above

  const update = useCallback((lineNo: number, patch: Partial<QuotationLine>) => {
    onChange(lines.map(l => l.line_no === lineNo ? { ...l, ...patch } : l))
  }, [lines, onChange])

  // ── Edit lock ───────────────────────────────────────────────────────────────

  function startEdit(lineNo: number) {
    const line = lines.find(l => l.line_no === lineNo)
    if (!line) return
    setEditingLine(lineNo)
    setEditSnapshot({ ...line })
    setOpenMenu(null)
  }

  function commitEdit() {
    setEditingLine(null)
    setEditSnapshot(null)
  }

  function cancelEdit() {
    const snap = editSnapshotRef.current
    if (snap) onChange(lines.map(l => l.line_no === snap.line_no ? { ...snap } : l))
    setEditingLine(null)
    setEditSnapshot(null)
  }

  // ── Row insertion / deletion ────────────────────────────────────────────────

  function addBelow(lineNo: number) {
    const idx = lines.findIndex(l => l.line_no === lineNo)
    const next = [...lines]
    next.splice(idx + 1, 0, newLine(0, defaultLocation))
    onChange(resequence(next))
    setEditingLine(idx + 2)
    setEditSnapshot(newLine(idx + 2, defaultLocation))
    setOpenMenu(null)
  }

  function addAbove(lineNo: number) {
    const idx = lines.findIndex(l => l.line_no === lineNo)
    const next = [...lines]
    next.splice(idx, 0, newLine(0, defaultLocation))
    onChange(resequence(next))
    setEditingLine(idx + 1)
    setEditSnapshot(newLine(idx + 1, defaultLocation))
    setOpenMenu(null)
  }

  function deleteLine(lineNo: number) {
    if (editingLine === lineNo) { setEditingLine(null); setEditSnapshot(null) }
    onChange(resequence(lines.filter(l => l.line_no !== lineNo)))
    setOpenMenu(null)
  }

  // ── Drag-to-reorder ─────────────────────────────────────────────────────────

  function handleDragStart(e: React.DragEvent, lineNo: number) {
    dragLineRef.current = lineNo
    e.dataTransfer.effectAllowed = 'move'
  }

  function handleDragOver(e: React.DragEvent, lineNo: number) {
    e.preventDefault()
    setDragOver(lineNo)
  }

  function handleDrop(e: React.DragEvent, targetLineNo: number) {
    e.preventDefault()
    const from = dragLineRef.current
    if (from === null || from === targetLineNo) { setDragOver(null); return }
    const fromIdx = lines.findIndex(l => l.line_no === from)
    const toIdx   = lines.findIndex(l => l.line_no === targetLineNo)
    if (fromIdx < 0 || toIdx < 0) { setDragOver(null); return }
    const next = [...lines]
    const [removed] = next.splice(fromIdx, 1)
    next.splice(toIdx, 0, removed)
    onChange(resequence(next))
    dragLineRef.current = null
    setDragOver(null)
  }

  function handleDragEnd() {
    dragLineRef.current = null
    setDragOver(null)
  }

  // ── Footer actions ──────────────────────────────────────────────────────────

  function resequenceMark() {
    onChange(lines.map((l, i) => ({ ...l, marking: String(i + 1) })))
  }

  function orderByMarking() {
    const sorted = [...lines].sort((a, b) => {
      const na = parseFloat(a.marking ?? '') || Infinity
      const nb = parseFloat(b.marking ?? '') || Infinity
      if (na !== nb) return na - nb
      return (a.marking ?? '').localeCompare(b.marking ?? '')
    })
    onChange(resequence(sorted))
  }

  // ── Item ID Enter-to-lookup ─────────────────────────────────────────────────

  async function handleItemIdEnter(lineNo: number, value: string) {
    try {
      const results = await searchItems(value.trim(), 'item_id')
      if (results.length === 1) {
        handleLookupSelect(lineNo, results[0])
      } else {
        setModal({ open: true, mode: 'item', lineNo, field: 'item_id', query: value })
      }
    } catch {
      setModal({ open: true, mode: 'item', lineNo, field: 'item_id', query: value })
    }
  }

  // ── Doc picker ──────────────────────────────────────────────────────────────

  function handleDocPickerSelect(selectedLines: QuotationLine[]) {
    if (!docPicker.open) return
    const idx = lines.findIndex(l => l.line_no === docPicker.lineNo)
    const cleaned = selectedLines.map(l => ({ ...l, id: undefined }))
    const next = [...lines]
    next.splice(idx < 0 ? 0 : idx, 0, ...cleaned)
    onChange(resequence(next))
    setDocPicker({ open: false })
  }

  // ── Item lookup selection ───────────────────────────────────────────────────

  function handleLookupSelect(lineNo: number, row: ItemResult | (LineTypeRow & { id?: undefined })) {
    const patch: Partial<QuotationLine> = {}
    if ('id' in row && row.id !== undefined) {
      const item = row as ItemResult
      patch.item_id       = item.id
      patch.item_code     = item.code
      patch.description   = item.description
      patch.line_type     = item.item_type
      patch.uom           = item.uom ?? ''
      patch.uom_con_rate  = item.uom_con_rate
      patch.price         = item.list_price
      patch.latest_cost   = item.latest_cost
      patch.obsolete_cost = item.obsolete_cost
      patch.estore_price  = item.estore_price
      patch.stock         = item.qty_stock
      patch.taxable       = item.taxable
      patch.tax_code      = item.tax_code
      patch.tax_rate      = item.tax_rate
      patch.location      = item.default_location
      if (PRICE_FORBIDDEN.has(item.item_type)) { patch.price = 0; patch.qty = 0 }
    } else {
      const lt = row as LineTypeRow
      patch.line_type   = lt.item_type
      patch.item_code   = lt.item_code
      patch.description = lt.description
      patch.price = 0; patch.qty = 0; patch.uom = ''
    }
    update(lineNo, patch)
    setModal({ open: false })
  }

  // ── Computed-line lookup (for Amount / Tax Amt display) ─────────────────────

  const byLine = new Map(computedLines.map(c => [c.line_no, c]))

  // ── Row renderer ────────────────────────────────────────────────────────────

  function renderCell(line: QuotationLine) {
    const lt = line.line_type
    const priceForbidden = PRICE_FORBIDDEN.has(lt)
    const qtyForbidden   = QTY_FORBIDDEN.has(lt)
    const isEditing      = editingLine === line.line_no

    return (
      <tr
        key={line.line_no}
        draggable={!isEditing}
        onDragStart={e => !isEditing && handleDragStart(e, line.line_no)}
        onDragOver={e => handleDragOver(e, line.line_no)}
        onDrop={e => handleDrop(e, line.line_no)}
        onDragEnd={handleDragEnd}
        className={`border-b border-[#EDE6DD] ${
          dragOver === line.line_no
            ? 'border-t-2 border-t-[#6C4C2C] bg-[#FBF6F1]'
            : isEditing
              ? 'bg-[#FFFBF7]'
              : 'hover:bg-[#FFFBF7]'
        }`}
      >
        {/* ── Row actions: drag / ▾ / edit-or-save / delete-or-cancel ── */}
        <td className={`${tdBase} w-20 relative`}>
          <div className="relative flex items-center gap-0.5">
            {/* Drag handle */}
            <span
              className="cursor-grab text-[#CCC] hover:text-[#888] text-[16px] leading-none select-none px-0.5"
              title="Drag to reorder"
            >⠿</span>

            {/* ▾ dropdown toggle */}
            <button
              onClick={(e) => {
                e.stopPropagation()
                if (openMenu === line.line_no) {
                  setOpenMenu(null)
                  setMenuPos(null)
                } else {
                  const rect = e.currentTarget.getBoundingClientRect()
                  setMenuPos({ top: rect.bottom + 2, left: rect.left })
                  setOpenMenu(line.line_no)
                }
              }}
              className="w-5 h-5 text-[#888] hover:text-[#6C4C2C] flex items-center justify-center rounded"
              title="Row actions"
            >▾</button>

            {/* Edit → Save (red) */}
            {isEditing ? (
              <button
                onClick={commitEdit}
                title="Change (Ctrl+S)"
                className="w-5 h-5 flex items-center justify-center rounded text-red-500 hover:bg-red-50 border border-red-300"
              >
                <svg width="11" height="11" viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round">
                  <path d="M13 2H5L2 5v9h12V2z"/>
                  <rect x="5" y="9" width="6" height="5" rx="0.5"/>
                  <rect x="5.5" y="2.5" width="5" height="3" rx="0.3"/>
                </svg>
              </button>
            ) : (
              <button
                onClick={() => startEdit(line.line_no)}
                title="Edit row"
                className="w-5 h-5 flex items-center justify-center rounded text-[#888] hover:text-[#6C4C2C]"
              >
                <svg width="11" height="11" viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round">
                  <path d="M11.5 2.5l2 2L5 13H3v-2L11.5 2.5z"/>
                </svg>
              </button>
            )}

            {/* Delete → Cancel × */}
            {isEditing ? (
              <button
                onClick={cancelEdit}
                title="Cancel"
                className="w-5 h-5 flex items-center justify-center rounded text-[#888] hover:text-red-600 font-bold text-[13px] leading-none"
              >×</button>
            ) : (
              <button
                onClick={() => deleteLine(line.line_no)}
                title="Delete row"
                className="w-5 h-5 flex items-center justify-center rounded text-[#888] hover:text-red-500"
              >
                <svg width="11" height="11" viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round">
                  <path d="M3 5h10M7 5V3h2v2M6 5v7a1 1 0 001 1h2a1 1 0 001-1V5H6z"/>
                </svg>
              </button>
            )}

          </div>
        </td>

        {/* SN */}
        <td className={`${tdBase} w-8 text-center text-[#888]`}>{line.line_no}</td>

        {/* Marking */}
        <td className={`${tdBase} w-14`}>
          {isEditing
            ? <input className={inputBase} value={line.marking ?? ''} onChange={e => update(line.line_no, { marking: e.target.value })} />
            : <span className={roText}>{line.marking ?? ''}</span>}
        </td>

        {/* Type */}
        <td className={`${tdBase} w-36`}>
          {isEditing
            ? <div className="relative">
                <select
                  className="w-full appearance-none bg-transparent border-0 outline-none text-[9pt] font-calibri text-[#404040] focus:ring-1 focus:ring-[#6C4C2C]/30 rounded cursor-pointer pr-5"
                  value={lt}
                  onChange={e => update(line.line_no, { line_type: e.target.value, price: 0, qty: 0 })}
                >
                  <option value="">—</option>
                  {['Header','Charges','Stock','Assembly','Non_Stock','Service','Discount','Remark','Sub Total','Total','CF Total'].map(t => (
                    <option key={t} value={t}>{t}</option>
                  ))}
                </select>
                <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center pr-1 text-[#888]">
                  <svg width="10" height="10" viewBox="0 0 10 10" fill="none" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round">
                    <path d="M2 3.5l3 3 3-3"/>
                  </svg>
                </div>
              </div>
            : <span className={roText}>{lt}</span>}
        </td>

        {/* Item ID */}
        <td className={`${tdBase} w-32`}>
          {isEditing ? (
            ITEM_OPTIONAL.has(lt) ? (
              <input className={inputBase} value={line.item_code ?? ''} readOnly
                onDoubleClick={() => setModal({ open: true, mode: 'linetype', lineNo: line.line_no })} />
            ) : (
              <input
                className={inputBase}
                value={line.item_code ?? ''}
                onChange={e => update(line.line_no, { item_code: e.target.value })}
                onDoubleClick={() => setModal({ open: true, mode: 'item', lineNo: line.line_no, field: 'item_id', query: line.item_code ?? '' })}
                onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); handleItemIdEnter(line.line_no, line.item_code ?? '') } }}
                placeholder="Enter or dbl-click"
              />
            )
          ) : <span
              className={`${roText} block w-full min-h-[1.1em] cursor-pointer`}
              onDoubleClick={() => {
                startEdit(line.line_no)
                setModal({ open: true, mode: 'item', lineNo: line.line_no, field: 'item_id', query: line.item_code ?? '' })
              }}
            >{line.item_code ?? ''}</span>}
        </td>

        {/* Description */}
        <td className={`${tdBase} min-w-[200px]`}>
          {isEditing
            ? <input
                className={inputBase}
                value={line.description ?? ''}
                onChange={e => update(line.line_no, { description: e.target.value })}
                onDoubleClick={() => ITEM_OPTIONAL.has(lt)
                  ? setModal({ open: true, mode: 'linetype', lineNo: line.line_no })
                  : setModal({ open: true, mode: 'item', lineNo: line.line_no, field: 'description', query: line.description ?? '' })
                }
              />
            : <span className={roText}>{line.description ?? ''}</span>}
        </td>

        {/* Qty/Ratio */}
        <td className={`${tdBase} w-20`}>
          {isEditing
            ? qtyForbidden
              ? <input className={roInputBase} value="0.00" readOnly />
              : <input className={`${inputBase} text-right`} type="number" step="any" min="0"
                  value={line.qty || ''} onChange={e => update(line.line_no, { qty: parseFloat(e.target.value) || 0 })} />
            : <span className={roNum}>{line.qty ? fmt(line.qty) : ''}</span>}
        </td>

        {/* Direct Ship Qty */}
        <td className={`${tdBase} w-20`}>
          {isEditing
            ? <input className={`${inputBase} text-right`} type="number" step="any" min="0"
                value={line.direct_ship_qty || ''}
                onChange={e => update(line.line_no, { direct_ship_qty: parseFloat(e.target.value) || 0 })} />
            : <span className={roNum}>{line.direct_ship_qty ? fmt(line.direct_ship_qty) : ''}</span>}
        </td>

        {/* UOM */}
        <td className={`${tdBase} w-14`}>
          {isEditing
            ? <input className={inputBase} value={line.uom ?? ''} onChange={e => update(line.line_no, { uom: e.target.value })} />
            : <span className={roText}>{line.uom ?? ''}</span>}
        </td>

        {/* UOM Con-Rate */}
        <td className={`${tdBase} w-20`}>
          {isEditing
            ? <input className={`${inputBase} text-right`} type="number" step="any"
                value={line.uom_con_rate || ''}
                onChange={e => update(line.line_no, { uom_con_rate: parseFloat(e.target.value) || 1 })} />
            : <span className={roNum}>{line.uom_con_rate}</span>}
        </td>

        {/* Project Cost */}
        <td className={`${tdBase} w-20`}>
          {isEditing
            ? <input className={`${inputBase} text-right`} type="number" step="any"
                value={line.project_cost || ''}
                onChange={e => update(line.line_no, { project_cost: parseFloat(e.target.value) || 0 })} />
            : <span className={roNum}>{line.project_cost ? fmt(line.project_cost) : ''}</span>}
        </td>

        {/* Price (BD) */}
        <td className={`${tdBase} w-24`}>
          {isEditing
            ? priceForbidden
              ? <input className={roInputBase} value="0.00" readOnly />
              : <input className={`${inputBase} text-right`} type="number" step="any" min="0"
                  value={line.price || ''}
                  onChange={e => update(line.line_no, { price: parseFloat(e.target.value) || 0 })} />
            : <span className={roNum}>{priceForbidden ? '' : (line.price ? fmt(line.price) : '')}</span>}
        </td>

        {/* Amount */}
        <td className={`${tdBase} w-28`}>
          {priceForbidden ? (
            // Rollup (Sub Total/Total/CF Total) and structural (Header/Remark): computed, read-only
            <span className={roNum}>{fmt(byLine.get(line.line_no)?.amount ?? 0)}</span>
          ) : isEditing ? (
            <input
              className={`${inputBase} text-right`}
              type="number" step="any"
              value={line.amount || ''}
              onChange={e => update(line.line_no, { amount: parseFloat(e.target.value) || 0 })}
              onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); commitEdit() } }}
            />
          ) : (
            <span className={`${roNum} cursor-pointer`} onDoubleClick={() => startEdit(line.line_no)}>{fmt(line.amount)}</span>
          )}
          {lt === 'Header' && (line.amount > 0) && (
            <span className="block text-red-600 text-[7.5pt]">Header cannot have amount</span>
          )}
        </td>

        {/* Tax */}
        <td className={`${tdBase} w-24`}>
          {isEditing && !priceForbidden
            ? <div className="relative">
                <select
                  className="w-full appearance-none bg-transparent border-0 outline-none text-[9pt] font-calibri text-[#404040] cursor-pointer pr-5"
                  value={line.tax_code ?? 'GST'}
                  onChange={e => update(line.line_no, { tax_code: e.target.value })}
                >
                  <option value="GST">GST</option>
                  <option value="">None</option>
                </select>
                <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center pr-1 text-[#888]">
                  <svg width="10" height="10" viewBox="0 0 10 10" fill="none" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round">
                    <path d="M2 3.5l3 3 3-3"/>
                  </svg>
                </div>
              </div>
            : <span className={roText}>{!priceForbidden ? (line.tax_code ?? 'GST') : ''}</span>}
        </td>

        {/* Tax Rate */}
        <td className={`${tdBase} w-16`}>
          {isEditing && !priceForbidden
            ? <input className={`${inputBase} text-right`} type="number" step="any"
                value={line.tax_rate || ''}
                onChange={e => update(line.line_no, { tax_rate: parseFloat(e.target.value) || 0 })} />
            : <span className={roNum}>{!priceForbidden ? line.tax_rate : ''}</span>}
        </td>

        {/* Tax Amt */}
        <td className={`${tdBase} w-24 text-right`}>
          <span className="tabular-nums text-[9pt] font-calibri text-[#888]">{fmt(byLine.get(line.line_no)?.tax_amt ?? 0)}</span>
        </td>

        {/* Tax Amt(L) */}
        <td className={`${tdBase} w-24 text-right`}>
          <span className="tabular-nums text-[9pt] font-calibri text-[#888]">{fmt(byLine.get(line.line_no)?.tax_amt_local ?? 0)}</span>
        </td>

        {/* Job ID */}
        <td className={`${tdBase} w-20`}>
          {isEditing
            ? <input className={inputBase} value={line.job_id ?? ''} onChange={e => update(line.line_no, { job_id: e.target.value })} />
            : <span className={roText}>{line.job_id ?? ''}</span>}
        </td>

        {/* Location */}
        <td className={`${tdBase} w-20`}>
          {isEditing
            ? <input className={inputBase} value={line.location ?? ''} onChange={e => update(line.line_no, { location: e.target.value })} />
            : <span className={roText}>{line.location ?? ''}</span>}
        </td>

        {/* Stock */}
        <td className={`${tdBase} w-16 text-right text-[#888]`}>
          <span className="tabular-nums text-[9pt] font-calibri">{line.stock || 0}</span>
        </td>

        {/* Latest Cost SGD */}
        <td className={`${tdBase} w-24 text-right text-[#888]`}>
          <span className="tabular-nums text-[9pt] font-calibri">{fmt(line.latest_cost)}</span>
        </td>

        {/* Obsolete Cost */}
        <td className={`${tdBase} w-24 text-right text-[#888]`}>
          <span className="tabular-nums text-[9pt] font-calibri">{fmt(line.obsolete_cost)}</span>
        </td>

        {/* EStore Price */}
        <td className={`${tdBase} w-24 text-right text-[#888]`}>
          <span className="tabular-nums text-[9pt] font-calibri">{fmt(line.estore_price)}</span>
        </td>

        {/* Customer Remark */}
        <td className={`${tdBase} min-w-[120px]`}>
          {isEditing
            ? <input className={inputBase} value={line.customer_remark ?? ''} onChange={e => update(line.line_no, { customer_remark: e.target.value })} />
            : <span className={roText}>{line.customer_remark ?? ''}</span>}
        </td>

        {/* Sales Remark */}
        <td className={`${tdBase} min-w-[120px]`}>
          {isEditing
            ? <input className={inputBase} value={line.sales_remark ?? ''} onChange={e => update(line.line_no, { sales_remark: e.target.value })} />
            : <span className={roText}>{line.sales_remark ?? ''}</span>}
        </td>
      </tr>
    )
  }

  const thBase = 'px-1.5 py-2 text-left text-[9pt] font-semibold font-calibri text-[#6C4C2C] whitespace-nowrap border-r border-[#E5DDD3]'

  return (
    <div className="flex flex-col">
      {/* Toolbar — Default Location + Settings gear */}
      <div className="flex items-center justify-between mb-2">
        <div className="flex items-center gap-2">
          <span className="text-[9pt] font-calibri font-semibold text-[#404040] whitespace-nowrap">
            Default Location :
          </span>
          <input
            className="h-7 w-32 rounded border border-[#D8CFC4] bg-white px-2 text-[9pt] font-calibri text-[#404040] focus:outline-none focus:ring-1 focus:ring-[#6C4C2C]/30"
            value={defaultLocation}
            onChange={e => onDefaultLocationChange(e.target.value)}
          />
        </div>
        <button
          className="w-7 h-7 rounded border border-[#D8CFC4] bg-white flex items-center justify-center text-[#6C4C2C] hover:bg-[#F3EAE2]"
          title="Settings"
        >
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round">
            <circle cx="12" cy="12" r="3"/>
            <path d="M12 1v3M12 20v3M4.22 4.22l2.12 2.12M17.66 17.66l2.12 2.12M1 12h3M20 12h3M4.22 19.78l2.12-2.12M17.66 6.34l2.12-2.12"/>
          </svg>
        </button>
      </div>

      {/* Scrollable grid */}
      <div className="overflow-x-auto border border-[#E5DDD3] rounded">
        <table className="border-collapse w-full" style={{ minWidth: '1900px' }}>
          <thead className="bg-[#F8F3EE] sticky top-0 z-10">
            <tr>
              <th className={`${thBase} w-20`}></th>
              <th className={`${thBase} w-8`}>SN</th>
              <th className={`${thBase} w-14`}>Marking</th>
              <th className={`${thBase} w-36`}>Type</th>
              <th className={`${thBase} w-32`}>Item ID</th>
              <th className={`${thBase} min-w-[200px]`}>Description</th>
              <th className={`${thBase} w-20`}>Qty/Ratio</th>
              <th className={`${thBase} w-20`}>Direct Ship</th>
              <th className={`${thBase} w-14`}>UOM</th>
              <th className={`${thBase} w-20`}>UOM Con-Rate</th>
              <th className={`${thBase} w-20`}>Project Cost</th>
              <th className={`${thBase} w-24`}>Price (BD)</th>
              <th className={`${thBase} w-28`}>Amount</th>
              <th className={`${thBase} w-24`}>Tax</th>
              <th className={`${thBase} w-16`}>Tax Rate</th>
              <th className={`${thBase} w-24`}>Tax Amt</th>
              <th className={`${thBase} w-24`}>Tax Amt(L)</th>
              <th className={`${thBase} w-20`}>Job ID</th>
              <th className={`${thBase} w-20`}>Location</th>
              <th className={`${thBase} w-16`}>Stock</th>
              <th className={`${thBase} w-24`}>Latest Cost SGD</th>
              <th className={`${thBase} w-24`}>Obsolete Cost</th>
              <th className={`${thBase} w-24`}>EStore Price</th>
              <th className={`${thBase} min-w-[120px]`}>Customer Remark</th>
              <th className={`${thBase} min-w-[120px]`}>Sales Remark</th>
            </tr>
          </thead>
          <tbody>
            {lines.map(renderCell)}
            {lines.length === 0 && (
              <tr>
                <td colSpan={25} className="text-center py-8 text-[9pt] font-calibri text-[#888]">
                  No line items. Use the buttons below to add rows.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>

      {/* Footer — Add Row, Resequence Mark, Order By Marking, Sub Total */}
      <div className="flex items-center justify-between mt-2 px-1">
        <div className="flex gap-2 flex-wrap">
          <button
            onClick={() => {
              const newNo = lines.length + 1
              onChange(resequence([...lines, newLine(0, defaultLocation)]))
              setEditingLine(newNo)
              setEditSnapshot(newLine(newNo, defaultLocation))
            }}
            className="h-7 px-3 rounded border border-[#D8CFC4] bg-white text-[9pt] font-calibri text-[#6C4C2C] hover:bg-[#F3EAE2] transition-colors"
          >+ Add Row</button>

          <button
            onClick={resequenceMark}
            className="h-7 px-3 rounded border border-[#D8CFC4] bg-white text-[9pt] font-calibri text-[#6C4C2C] hover:bg-[#F3EAE2] transition-colors flex items-center gap-1.5"
            title="Renumber Marking column 1, 2, 3…"
          >
            <svg width="11" height="11" viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round">
              <path d="M2 4h12M2 8h9M2 12h6"/>
            </svg>
            Resequence Mark
          </button>

          <button
            onClick={orderByMarking}
            className="h-7 px-3 rounded border border-[#D8CFC4] bg-white text-[9pt] font-calibri text-[#6C4C2C] hover:bg-[#F3EAE2] transition-colors flex items-center gap-1.5"
            title="Sort rows by Marking value"
          >
            <svg width="11" height="11" viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round">
              <path d="M2 4h12M2 8h12M2 12h12M11 1l2 2-2 2"/>
            </svg>
            Order By Marking
          </button>
        </div>

        <div className="flex items-center gap-3 text-[10pt] font-calibri">
          <span className="text-[#888]">Sub Total:</span>
          <span className="font-semibold text-[#6C4C2C] tabular-nums">{fmt(subTotal)}</span>
        </div>
      </div>

      {/* ▾ Row action dropdown — rendered at component root to escape table overflow clipping */}
      {openMenu !== null && menuPos && (
        <div
          className="fixed z-50 bg-white border border-[#E5DDD3] rounded shadow-md min-w-[210px] text-[9pt] font-calibri flex flex-col"
          style={{ top: menuPos.top, left: menuPos.left }}
        >
          <button className="text-left px-3 py-2 hover:bg-[#F3EAE2]"
            onClick={() => addAbove(openMenu)}>+ Insert Row Above</button>
          <button className="text-left px-3 py-2 hover:bg-[#F3EAE2]"
            onClick={() => addBelow(openMenu)}>+ Insert Row Below</button>
          <button className="text-left px-3 py-2 hover:bg-[#F3EAE2]"
            onClick={() => { setOpenMenu(null); setMenuPos(null); setDocPicker({ open: true, lineNo: openMenu }) }}
          >Insert Data Above From Other</button>
        </div>
      )}

      {/* Backdrop — closes ▾ menu when clicking outside */}
      {openMenu !== null && (
        <div className="fixed inset-0 z-40" onClick={() => { setOpenMenu(null); setMenuPos(null) }} />
      )}

      {/* Item lookup modal */}
      {modal.open && modal.mode === 'item' && (
        <ItemLookupModal
          mode="item"
          initialQuery={modal.query}
          searchField={modal.field}
          onSelect={row => handleLookupSelect(modal.lineNo, row)}
          onClose={() => setModal({ open: false })}
        />
      )}
      {modal.open && modal.mode === 'linetype' && (
        <ItemLookupModal
          mode="linetype"
          onSelect={row => handleLookupSelect(modal.lineNo, row)}
          onClose={() => setModal({ open: false })}
        />
      )}

      {/* Document picker modal */}
      {docPicker.open && (
        <DocumentPickerModal
          onSelect={handleDocPickerSelect}
          onClose={() => setDocPicker({ open: false })}
        />
      )}
    </div>
  )
}
