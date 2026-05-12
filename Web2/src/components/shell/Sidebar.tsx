'use client'

import { useState, useMemo } from 'react'
import Link from 'next/link'
import { usePathname, useRouter } from 'next/navigation'
import { NAV, isLeaf, NavSection, NavGroup, NavLeaf } from '@/lib/navigation'
import { logout } from '@/lib/mockApi'
import type { UserInfo } from '@/lib/mockApi'

interface Props {
  collapsed: boolean
  user: UserInfo
}

export default function Sidebar({ collapsed, user }: Props) {
  const pathname = usePathname()
  const router = useRouter()
  const [openSection, setOpenSection] = useState<string | null>('transactions')
  const [openGroup, setOpenGroup] = useState<string | null>(null)
  const [search, setSearch] = useState('')
  const [userMenuOpen, setUserMenuOpen] = useState(false)

  const query = search.toLowerCase().trim()

  const filteredNav = useMemo(() => {
    if (!query) return NAV
    return NAV.map(section => {
      const filteredChildren = section.children
        .map(child => {
          if (isLeaf(child)) {
            return child.title.toLowerCase().includes(query) ? child : null
          } else {
            const leaves = child.children.filter(l => l.title.toLowerCase().includes(query))
            return leaves.length > 0 ? { ...child, children: leaves } : null
          }
        })
        .filter(Boolean) as (NavLeaf | NavGroup)[]
      return filteredChildren.length > 0 ? { ...section, children: filteredChildren } : null
    }).filter(Boolean) as NavSection[]
  }, [query])

  function isActive(slug: string) {
    return pathname === `/app/${slug}` || pathname.startsWith(`/app/${slug}/`)
  }

  function toggleSection(id: string) {
    setOpenSection(prev => prev === id ? null : id)
    setOpenGroup(null)
  }

  function toggleGroup(id: string) {
    setOpenGroup(prev => prev === id ? null : id)
  }

  function handleLogout() {
    logout()
    router.push('/login')
  }

  const w = collapsed ? 'w-16' : 'w-64'

  return (
    <aside
      className={`${w} flex flex-col shrink-0 bg-bg-surface border-r border-border transition-all duration-200 overflow-hidden`}
      style={{ height: '100%' }}
    >
      {/* Search */}
      {!collapsed && (
        <div className="px-3 py-3 border-b border-border">
          <div className="relative">
            <svg className="absolute left-2.5 top-1/2 -translate-y-1/2 text-txt-tertiary pointer-events-none" width="13" height="13" viewBox="0 0 16 16" fill="none">
              <circle cx="7" cy="7" r="5" stroke="currentColor" strokeWidth="1.5"/>
              <path d="M11 11l3 3" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
            </svg>
            <input
              value={search}
              onChange={e => setSearch(e.target.value)}
              placeholder="Search…"
              className="w-full h-8 pl-7 pr-2 rounded-lg border border-border text-xs text-txt-primary bg-bg-muted focus:outline-none focus:ring-2 focus:ring-brand-500 focus:border-brand-500 focus:bg-bg-surface placeholder:text-txt-tertiary transition-all"
            />
            {search && (
              <button onClick={() => setSearch('')} className="absolute right-2 top-1/2 -translate-y-1/2 text-txt-tertiary hover:text-txt-primary">
                <svg width="10" height="10" viewBox="0 0 16 16" fill="none"><path d="M2 2l12 12M14 2L2 14" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/></svg>
              </button>
            )}
          </div>
        </div>
      )}

      {/* Nav items */}
      <nav className="flex-1 overflow-y-auto py-2">
        {filteredNav.map(section => {
          const isSectionOpen = openSection === section.id || (query.length > 0)

          return (
            <div key={section.id} className="mb-0.5">
              {/* Section header */}
              <button
                type="button"
                onClick={() => toggleSection(section.id)}
                title={collapsed ? section.title : undefined}
                className={[
                  'w-full flex items-center gap-2.5 px-3 py-2 text-left transition-colors rounded-lg mx-1',
                  'hover:bg-bg-muted',
                  isSectionOpen && !query ? 'text-txt-primary font-semibold' : 'text-txt-secondary',
                  collapsed ? 'justify-center mx-2 w-auto' : '',
                ].join(' ')}
              >
                <SidebarIcon name={section.icon} />
                {!collapsed && (
                  <>
                    <span className="flex-1 text-xs font-medium truncate">{section.title}</span>
                    <svg
                      className={`shrink-0 text-txt-tertiary transition-transform ${isSectionOpen ? 'rotate-180' : ''}`}
                      width="12" height="12" viewBox="0 0 16 16" fill="none"
                    >
                      <path d="M4 6l4 4 4-4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/>
                    </svg>
                  </>
                )}
              </button>

              {/* Section children */}
              {isSectionOpen && !collapsed && (
                <div className="mt-0.5">
                  {section.children.map(child => {
                    if (isLeaf(child)) {
                      const active = isActive(child.slug)
                      return (
                        <Link
                          key={child.slug}
                          href={`/app/${child.slug}`}
                          className={[
                            'flex items-center gap-2 pl-10 pr-3 py-1.5 mx-1 rounded-lg text-xs transition-colors',
                            active
                              ? 'bg-brand-50 text-brand-600 font-semibold'
                              : 'text-txt-secondary hover:bg-bg-muted hover:text-txt-primary',
                          ].join(' ')}
                        >
                          {active && <span className="w-1 h-1 rounded-full bg-brand-500 shrink-0" />}
                          {child.title}
                        </Link>
                      )
                    } else {
                      const isGroupOpen = openGroup === child.id || query.length > 0
                      const hasActive = child.children.some(l => isActive(l.slug))
                      return (
                        <div key={child.id}>
                          <button
                            type="button"
                            onClick={() => toggleGroup(child.id)}
                            className={[
                              'w-full flex items-center gap-2 pl-7 pr-3 py-1.5 mx-1 rounded-lg text-xs transition-colors',
                              hasActive ? 'text-brand-600 font-semibold' : 'text-txt-secondary hover:text-txt-primary',
                              'hover:bg-bg-muted',
                            ].join(' ')}
                          >
                            <svg
                              className={`shrink-0 transition-transform text-txt-tertiary ${isGroupOpen ? 'rotate-90' : ''}`}
                              width="10" height="10" viewBox="0 0 16 16" fill="none"
                            >
                              <path d="M6 4l4 4-4 4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/>
                            </svg>
                            <span className="flex-1 truncate">{child.title}</span>
                          </button>
                          {isGroupOpen && child.children.map(leaf => {
                            const active = isActive(leaf.slug)
                            return (
                              <Link
                                key={leaf.slug}
                                href={`/app/${leaf.slug}`}
                                className={[
                                  'flex items-center gap-2 pl-12 pr-3 py-1.5 mx-1 rounded-lg text-xs transition-colors',
                                  active
                                    ? 'bg-brand-50 text-brand-600 font-semibold'
                                    : 'text-txt-secondary hover:bg-bg-muted hover:text-txt-primary',
                                ].join(' ')}
                              >
                                {active && <span className="w-1 h-1 rounded-full bg-brand-500 shrink-0" />}
                                {leaf.title}
                              </Link>
                            )
                          })}
                        </div>
                      )
                    }
                  })}
                </div>
              )}
            </div>
          )
        })}
      </nav>

      {/* User profile bottom */}
      <div className="border-t border-border relative">
        <button
          type="button"
          onClick={() => setUserMenuOpen(v => !v)}
          title={collapsed ? user.userName : undefined}
          className="w-full flex items-center gap-2.5 px-3 py-3 hover:bg-bg-muted transition-colors"
        >
          <div className="w-7 h-7 rounded-full bg-brand-600 text-white flex items-center justify-center text-xs font-semibold shrink-0">
            {user.userName.charAt(0).toUpperCase()}
          </div>
          {!collapsed && (
            <>
              <div className="flex-1 text-left min-w-0">
                <p className="text-xs font-semibold text-txt-primary truncate">{user.userName}</p>
                <p className="text-[10px] text-txt-tertiary truncate">{user.userId.toUpperCase()}</p>
              </div>
              <svg width="12" height="12" viewBox="0 0 16 16" fill="none" className="shrink-0 text-txt-tertiary">
                <path d="M4 6l4 4 4-4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
              </svg>
            </>
          )}
        </button>

        {userMenuOpen && (
          <div className={`absolute bottom-full ${collapsed ? 'left-16' : 'left-2 right-2'} bg-bg-surface border border-border shadow-pop rounded-xl overflow-hidden z-50 mb-1`}>
            <Link
              href="/change-password"
              onClick={() => setUserMenuOpen(false)}
              className="flex items-center gap-2.5 px-4 py-2.5 text-xs text-txt-primary hover:bg-bg-muted transition-colors"
            >
              <svg width="14" height="14" viewBox="0 0 16 16" fill="none" className="text-txt-tertiary"><circle cx="8" cy="8" r="6" stroke="currentColor" strokeWidth="1.5"/><path d="M8 5v3l2 2" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/></svg>
              Change Password
            </Link>
            <div className="border-t border-border" />
            <button
              type="button"
              onClick={handleLogout}
              className="w-full flex items-center gap-2.5 px-4 py-2.5 text-xs text-red-600 hover:bg-red-50 transition-colors"
            >
              <svg width="14" height="14" viewBox="0 0 16 16" fill="none"><path d="M10 3h3a1 1 0 011 1v8a1 1 0 01-1 1h-3M7 11l3-3-3-3M10 8H3" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/></svg>
              Logout
            </button>
          </div>
        )}
      </div>
    </aside>
  )
}

function SidebarIcon({ name }: { name: string }) {
  return (
    <img
      src={`/icons/sidebar/${name}.svg`}
      alt=""
      width={15}
      height={15}
      className="shrink-0 opacity-60"
    />
  )
}
