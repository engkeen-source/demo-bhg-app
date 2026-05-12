'use client'

import { usePathname } from 'next/navigation'
import { useMemo } from 'react'
import { getBreadcrumb } from '@/lib/navigation'

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
    <header
      className="flex items-center gap-3 px-4 border-b border-[#E5DDD3] bg-white shrink-0"
      style={{ height: '48px' }}
    >
      {/* Hamburger */}
      <button
        type="button"
        onClick={onToggleSidebar}
        title={collapsed ? 'Expand sidebar' : 'Collapse sidebar'}
        className="p-1.5 rounded hover:bg-[#F3EAE2] transition-colors text-[#404040]"
        aria-label="Toggle sidebar"
      >
        <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
          <path d="M2 4h12M2 8h12M2 12h12" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
        </svg>
      </button>

      {/* Logo */}
      <span className="text-[11pt] font-bold font-calibri text-[#6C4C2C] mr-2 shrink-0">BossSO</span>

      {/* Breadcrumb */}
      <nav className="flex items-center gap-1 flex-1 min-w-0 text-[9pt] font-calibri text-[#888] overflow-hidden">
        {breadcrumb.map((crumb, i) => (
          <span key={i} className="flex items-center gap-1 min-w-0">
            {i > 0 && (
              <svg className="shrink-0" width="10" height="10" viewBox="0 0 16 16" fill="none">
                <path d="M6 4l4 4-4 4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/>
              </svg>
            )}
            <span className={i === breadcrumb.length - 1 ? 'text-[#404040] font-medium truncate' : 'truncate'}>
              {crumb.title}
            </span>
          </span>
        ))}
      </nav>

      {/* Right side */}
      <div className="flex items-center gap-2 shrink-0">
        <span className="text-[8pt] font-calibri text-[#888] hidden sm:block">
          {new Date().toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' })}
        </span>
      </div>
    </header>
  )
}
