'use client'

import { useState } from 'react'
import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'
import Button from '@/components/common/Button'
import DataGrid, { Column } from '@/components/common/DataGrid'

interface Props {
  title: string
  desktop?: string
  columns?: Column[]
  rows?: Record<string, unknown>[]
}

const DEFAULT_COLUMNS: Column[] = [
  { key: 'code', header: 'Code', width: '120px' },
  { key: 'name', header: 'Name' },
  { key: 'status', header: 'Status', width: '100px' },
]

const DEFAULT_ROWS = [
  { id: '1', code: 'C0001', name: 'Sample Record A', status: 'Active' },
  { id: '2', code: 'C0002', name: 'Sample Record B', status: 'Active' },
  { id: '3', code: 'C0003', name: 'Sample Record C (Inactive)', status: 'Inactive' },
]

export default function ListEditPage({ title, desktop, columns = DEFAULT_COLUMNS, rows = DEFAULT_ROWS }: Props) {
  const [selected, setSelected] = useState<string | undefined>()

  return (
    <div className="space-y-5">
      <PageHeader
        title={title}
        actions={
          <>
            <Button variant="primary" size="sm">
              <svg width="12" height="12" viewBox="0 0 16 16" fill="none"><path d="M8 2v12M2 8h12" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/></svg>
              New
            </Button>
            <Button variant="secondary" size="sm" disabled={!selected}>Edit</Button>
            <Button variant="ghost" size="sm" disabled={!selected} className="text-red-600 hover:bg-red-50">Delete</Button>
          </>
        }
      />

      <Card noPad>
        <div className="px-5 py-3 border-b border-border flex items-center gap-2">
          <div className="relative flex-1 max-w-xs">
            <svg className="absolute left-2.5 top-1/2 -translate-y-1/2 text-txt-tertiary" width="13" height="13" viewBox="0 0 16 16" fill="none">
              <circle cx="7" cy="7" r="5" stroke="currentColor" strokeWidth="1.5"/>
              <path d="M11 11l3 3" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
            </svg>
            <input
              placeholder="Search…"
              className="h-8 pl-8 pr-3 w-full rounded-lg border border-border text-sm text-txt-primary focus:outline-none focus:ring-2 focus:ring-brand-500 focus:border-brand-500 bg-bg-muted placeholder:text-txt-tertiary"
            />
          </div>
          <Button variant="ghost" size="sm">Export</Button>
        </div>
        <DataGrid
          columns={columns}
          rows={rows}
          rowKey="id"
          selectedKey={selected}
          onRowClick={row => setSelected(row.id as string)}
        />
        <div className="px-5 py-2.5 border-t border-border text-xs text-txt-tertiary">
          {rows.length} record{rows.length !== 1 ? 's' : ''}
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
