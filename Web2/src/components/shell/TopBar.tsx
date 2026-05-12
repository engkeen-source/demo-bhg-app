'use client'

import { usePathname } from 'next/navigation'
import { useMemo } from 'react'
import { getBreadcrumb } from '@/lib/navigation'
import Link from 'next/link'

interface Props {
  onToggleSidebar: () => void
  collapsed: boolean
}

export default function TopBar({ onToggleSidebar, collapsed }: Props) {
  const pathname = usePathname()

  const breadcrumb = useMemo(() => {
    const slug = pathname.replace(/^\/app\//, '')
    if (!slug || slug === 'app') return [{ title: 'Dashboard', href: '/app' }]
    return getBreadcrumb(slug)
  }, [pathname])

  return (
    <header className="flex items-center gap-3 px-4 border-b border-border bg-bg-surface shrink-0 h-14">
      {/* Hamburger */}
      <button
        type="button"
        onClick={onToggleSidebar}
        title={collapsed ? 'Expand sidebar' : 'Collapse sidebar'}
        className="p-2 rounded-lg hover:bg-bg-muted transition-colors text-txt-tertiary hover:text-txt-primary focus:outline-none"
        aria-label="Toggle sidebar"
      >
        <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
          <path d="M2 4h12M2 8h12M2 12h12" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
        </svg>
      </button>

      {/* Logo */}
      <Link href="/app" className="text-sm font-bold text-brand-600 mr-2 shrink-0 hover:text-brand-700 transition-colors">
        BossSO
      </Link>

      {/* Separator */}
      <div className="w-px h-4 bg-border shrink-0" />

      {/* Breadcrumb */}
      <nav className="flex items-center gap-1 flex-1 min-w-0 text-xs text-txt-tertiary overflow-hidden">
        {breadcrumb.map((crumb, i) => (
          <span key={i} className="flex items-center gap-1 min-w-0">
            {i > 0 && (
              <svg className="shrink-0 text-txt-tertiary" width="10" height="10" viewBox="0 0 16 16" fill="none">
                <path d="M6 4l4 4-4 4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/>
              </svg>
            )}
            <span className={i === breadcrumb.length - 1 ? 'text-txt-primary font-medium truncate' : 'truncate'}>
              {crumb.title}
            </span>
          </span>
        ))}
      </nav>

      {/* Right side */}
      <div className="flex items-center gap-2 shrink-0">
        <span className="text-xs text-txt-tertiary hidden sm:block">
          {new Date().toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' })}
        </span>
      </div>
    </header>
  )
}
