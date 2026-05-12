'use client'

import { UserInfo } from '@/lib/mockApi'
import { formatLastLoginDate } from '@/lib/messages'

interface Props {
  user: UserInfo
}

export default function StatusBar({ user }: Props) {
  const lastLoginText = `${user.userId.toUpperCase()} last logged in on ${formatLastLoginDate(user.loginTime)} from ${user.identifier}`

  return (
    <div
      className="flex items-center border-t border-[#C8B4A0] text-[9pt] font-calibri text-[#404040] shrink-0"
      style={{ background: '#E7D6C5', height: '22px' }}
      title={lastLoginText}
    >
      {/* Panel: Login (username) */}
      <StatusPanel value={user.userId.toUpperCase()} width={120} tooltip={lastLoginText} />
      <PanelDivider />

      {/* Panel: Company */}
      <StatusPanel value={user.companyNm} width={200} />
      <PanelDivider />

      {/* Panel: Period (mocked) */}
      <StatusPanel value="Period: May 2026" width={120} />
      <PanelDivider />

      {/* Panel: Date */}
      <StatusPanel value={new Date().toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' })} width={100} />
      <PanelDivider />

      {/* Panel: DB ID */}
      <StatusPanel value={user.databaseId} width={80} />
    </div>
  )
}

function StatusPanel({ value, width, tooltip }: { value: string; width: number; tooltip?: string }) {
  return (
    <div
      className="px-2 py-0 truncate"
      style={{ width: `${width}px`, minWidth: `${width}px` }}
      title={tooltip ?? value}
    >
      {value}
    </div>
  )
}

function PanelDivider() {
  return <div className="w-px h-4 bg-[#C8B4A0] shrink-0" />
}
