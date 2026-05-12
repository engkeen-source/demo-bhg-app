'use client'

import { useState } from 'react'
import { ReactNode } from 'react'
import Sidebar from './Sidebar'
import TopBar from './TopBar'
import StatusBar from './StatusBar'
import type { UserInfo } from '@/lib/mockApi'

interface Props {
  user: UserInfo
  children: ReactNode
}

export default function AppShell({ user, children }: Props) {
  const [collapsed, setCollapsed] = useState(false)

  return (
    <div className="flex flex-col h-screen bg-[#FAF8F5] overflow-hidden">
      {/* Top bar */}
      <TopBar onToggleSidebar={() => setCollapsed(v => !v)} collapsed={collapsed} />

      {/* Middle: sidebar + content */}
      <div className="flex flex-1 min-h-0">
        <Sidebar collapsed={collapsed} user={user} />
        <main className="flex-1 overflow-auto p-5">
          {children}
        </main>
      </div>

      {/* Status bar */}
      <StatusBar user={user} />
    </div>
  )
}
