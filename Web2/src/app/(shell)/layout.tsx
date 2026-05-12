'use client'

import { useEffect, useState } from 'react'
import { useRouter } from 'next/navigation'
import { getCurrentUser } from '@/lib/mockApi'
import type { UserInfo } from '@/lib/mockApi'
import AppShell from '@/components/shell/AppShell'

export default function ShellLayout({ children }: { children: React.ReactNode }) {
  const router = useRouter()
  const [user, setUser] = useState<UserInfo | null>(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const u = getCurrentUser()
    if (!u) {
      router.replace('/login')
    } else {
      setUser(u)
    }
    setLoading(false)
  }, [router])

  if (loading || !user) {
    return (
      <div className="flex items-center justify-center h-screen bg-bg-base">
        <div className="flex items-center gap-2 text-sm text-txt-tertiary">
          <svg className="animate-spin" width="16" height="16" viewBox="0 0 24 24" fill="none">
            <circle cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="2" strokeDasharray="32" strokeDashoffset="32" className="opacity-25"/>
            <path d="M12 2a10 10 0 0110 10" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/>
          </svg>
          Loading…
        </div>
      </div>
    )
  }

  return <AppShell user={user}>{children}</AppShell>
}
