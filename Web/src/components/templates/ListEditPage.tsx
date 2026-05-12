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
    <div className="space-y-4">
      <PageHeader
        title={title}
        actions={
          <>
            <Button variant="primary" size="sm">
              <svg width="12" height="12" viewBox="0 0 16 16" fill="none"><path d="M8 2v12M2 8h12" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/></svg>
              New
            </Button>
            <Button variant="secondary" size="sm" disabled={!selected}>Edit</Button>
            <Button variant="danger" size="sm" disabled={!selected}>Delete</Button>
          </>
        }
      />

      <Card noPad>
        <div className="p-3 border-b border-[#E5DDD3] flex items-center gap-2">
          <input
            placeholder="Search..."
            className="h-8 px-2.5 rounded border border-[#D8CFC4] text-[9pt] font-calibri text-[#404040] focus:outline-none focus:ring-1 focus:ring-[#6C4C2C]/40 w-64"
          />
          <Button variant="ghost" size="sm">Export</Button>
        </div>
        <DataGrid
          columns={columns}
          rows={rows}
          rowKey="id"
          selectedKey={selected}
          onRowClick={row => setSelected(row.id as string)}
        />
        <div className="px-3 py-2 border-t border-[#E5DDD3] text-[9pt] font-calibri text-[#888]">
          {rows.length} record{rows.length !== 1 ? 's' : ''}
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
