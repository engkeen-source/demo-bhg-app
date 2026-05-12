'use client'

import { useState } from 'react'
import EmptyState from './EmptyState'

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
  columns, rows, onRowClick, selectedKey, rowKey = 'id' as keyof T, emptyMessage,
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
    <div className="overflow-auto">
      <table className="w-full text-sm text-txt-primary border-collapse">
        <thead className="sticky top-0 z-10">
          <tr className="bg-bg-muted border-b border-border">
            {columns.map(col => (
              <th
                key={col.key}
                onClick={() => handleSort(col.key)}
                className={[
                  'px-4 py-2.5 text-xs font-semibold text-txt-secondary uppercase tracking-wide whitespace-nowrap cursor-pointer select-none',
                  col.align === 'right' ? 'text-right' : col.align === 'center' ? 'text-center' : 'text-left',
                  'hover:bg-zinc-100 transition-colors',
                ].join(' ')}
                style={col.width ? { width: col.width } : undefined}
              >
                {col.header}
                {sortCol === col.key && (
                  <span className="ml-1 text-brand-500">{sortDir === 'asc' ? '↑' : '↓'}</span>
                )}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {sorted.length === 0 ? (
            <tr>
              <td colSpan={columns.length}>
                <EmptyState message={emptyMessage} />
              </td>
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
                    'border-b border-border last:border-0 transition-colors',
                    onRowClick ? 'cursor-pointer' : '',
                    isSelected ? 'bg-brand-50' : 'hover:bg-bg-muted',
                  ].join(' ')}
                >
                  {columns.map(col => (
                    <td
                      key={col.key}
                      className={[
                        'px-4 py-3',
                        col.align === 'right' ? 'text-right tabular-nums' : col.align === 'center' ? 'text-center' : 'text-left',
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
