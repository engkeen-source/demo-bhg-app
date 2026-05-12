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
      className="flex items-center border-t border-border bg-bg-surface shrink-0 px-4 gap-4"
      style={{ height: '32px' }}
      title={lastLoginText}
    >
      <StatusItem label={user.userId.toUpperCase()} title={lastLoginText} />
      <Sep />
      <StatusItem label={user.companyNm} />
      <Sep />
      <StatusItem label="Period: May 2026" />
      <Sep />
      <StatusItem label={new Date().toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' })} />
      <Sep />
      <StatusItem label={user.databaseId} />
    </div>
  )
}

function StatusItem({ label, title }: { label: string; title?: string }) {
  return (
    <span
      className="text-xs text-txt-tertiary truncate"
      title={title ?? label}
    >
      {label}
    </span>
  )
}

function Sep() {
  return <span className="text-border shrink-0 text-xs">•</span>
}
