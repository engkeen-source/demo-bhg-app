'use client'

import { useState } from 'react'

export interface Column<T = Record<string, unknown>> {
  key: string
  header: string
  width?: string
  align?: 'left' | 'right' | 'center'
  render?: (row: T, idx: number) => React.ReactNode
}

interface Props<T extends Record<string, unknown>> {
  columns: Column<T>[]
  rows: T[]
  onRowClick?: (row: T) => void
  selectedKey?: string
  rowKey?: keyof T
  emptyMessage?: string
}

export default function DataGrid<T extends Record<string, unknown>>({
  columns, rows, onRowClick, selectedKey, rowKey = 'id' as keyof T, emptyMessage = 'No records found.',
}: Props<T>) {
  const [sortCol, setSortCol] = useState<string | null>(null)
  const [sortDir, setSortDir] = useState<'asc' | 'desc'>('asc')

  function handleSort(key: string) {
    if (sortCol === key) setSortDir(d => d === 'asc' ? 'desc' : 'asc')
    else { setSortCol(key); setSortDir('asc') }
  }

  const sorted = sortCol
    ? [...rows].sort((a, b) => {
        const av = String(a[sortCol] ?? '')
        const bv = String(b[sortCol] ?? '')
        return sortDir === 'asc' ? av.localeCompare(bv) : bv.localeCompare(av)
      })
    : rows

  return (
    <div className="overflow-auto border border-[#E5DDD3] rounded-lg">
      <table className="w-full text-[10pt] font-calibri text-[#404040] border-collapse">
        <thead>
          <tr className="bg-[#F3EAE2] border-b border-[#E5DDD3]">
            {columns.map(col => (
              <th
                key={col.key}
                onClick={() => handleSort(col.key)}
                className={[
                  'px-3 py-2 font-semibold text-[9pt] text-left whitespace-nowrap cursor-pointer select-none',
                  col.align === 'right' ? 'text-right' : col.align === 'center' ? 'text-center' : 'text-left',
                  'hover:bg-[#E7D6C5] transition-colors',
                ].join(' ')}
                style={col.width ? { width: col.width } : undefined}
              >
                {col.header}
                {sortCol === col.key && (
                  <span className="ml-1 text-[#6C4C2C]">{sortDir === 'asc' ? '↑' : '↓'}</span>
                )}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {sorted.length === 0 ? (
            <tr>
              <td colSpan={columns.length} className="text-center py-8 text-[#888]">{emptyMessage}</td>
            </tr>
          ) : (
            sorted.map((row, idx) => {
              const key = String(row[rowKey] ?? idx)
              const isSelected = selectedKey === key
              return (
                <tr
                  key={key}
                  onClick={() => onRowClick?.(row)}
                  className={[
                    idx % 2 === 0 ? 'bg-white' : 'bg-[#FAF8F5]',
                    'border-b border-[#E5DDD3] last:border-0 transition-colors',
                    onRowClick ? 'cursor-pointer hover:bg-[#F3EAE2]' : '',
                    isSelected ? '!bg-[#E7D6C5]' : '',
                  ].join(' ')}
                >
                  {columns.map(col => (
                    <td
                      key={col.key}
                      className={[
                        'px-3 py-2',
                        col.align === 'right' ? 'text-right' : col.align === 'center' ? 'text-center' : 'text-left',
                      ].join(' ')}
                    >
                      {col.render ? col.render(row, idx) : String(row[col.key] ?? '')}
                    </td>
                  ))}
                </tr>
              )
            })
          )}
        </tbody>
      </table>
    </div>
  )
}
