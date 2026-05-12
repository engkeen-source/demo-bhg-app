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
      <div className="flex items-center justify-center h-screen bg-[#FAF8F5]">
        <div className="text-[10pt] font-calibri text-[#888]">Loading…</div>
      </div>
    )
  }

  return <AppShell user={user}>{children}</AppShell>
}
