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

  const w = collapsed ? 'w-14' : 'w-60'

  return (
    <aside
      className={`${w} flex flex-col shrink-0 bg-[#F3EAE2] border-r border-[#E5DDD3] transition-all duration-200 overflow-hidden`}
      style={{ height: '100%' }}
    >
      {/* Search */}
      {!collapsed && (
        <div className="px-3 py-2 border-b border-[#E5DDD3]">
          <div className="relative">
            <svg className="absolute left-2 top-1/2 -translate-y-1/2 text-[#888] pointer-events-none" width="12" height="12" viewBox="0 0 16 16" fill="none">
              <circle cx="7" cy="7" r="5" stroke="currentColor" strokeWidth="1.5"/>
              <path d="M11 11l3 3" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
            </svg>
            <input
              value={search}
              onChange={e => setSearch(e.target.value)}
              placeholder="Search..."
              className="w-full h-7 pl-6 pr-2 rounded border border-[#D8CFC4] bg-white text-[9pt] font-calibri text-[#404040] focus:outline-none focus:ring-1 focus:ring-[#6C4C2C]/40"
            />
            {search && (
              <button onClick={() => setSearch('')} className="absolute right-1.5 top-1/2 -translate-y-1/2 text-[#888] hover:text-[#404040]">
                <svg width="10" height="10" viewBox="0 0 16 16" fill="none"><path d="M2 2l12 12M14 2L2 14" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/></svg>
              </button>
            )}
          </div>
        </div>
      )}

      {/* Nav items */}
      <nav className="flex-1 overflow-y-auto py-1">
        {filteredNav.map(section => {
          const isSectionOpen = openSection === section.id || (query.length > 0)

          return (
            <div key={section.id}>
              {/* Section header */}
              <button
                type="button"
                onClick={() => toggleSection(section.id)}
                title={collapsed ? section.title : undefined}
                className={[
                  'w-full flex items-center gap-2.5 px-3 py-2 text-left transition-colors',
                  'hover:bg-[#E7D6C5] text-[#404040]',
                  isSectionOpen && !query ? 'font-semibold' : 'font-medium',
                ].join(' ')}
              >
                <SidebarIcon name={section.icon} />
                {!collapsed && (
                  <>
                    <span className="flex-1 text-[10pt] font-calibri truncate">{section.title}</span>
                    <svg
                      className={`shrink-0 transition-transform ${isSectionOpen ? 'rotate-180' : ''}`}
                      width="12" height="12" viewBox="0 0 16 16" fill="none"
                    >
                      <path d="M4 6l4 4 4-4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/>
                    </svg>
                  </>
                )}
              </button>

              {/* Section children */}
              {isSectionOpen && !collapsed && (
                <div>
                  {section.children.map(child => {
                    if (isLeaf(child)) {
                      const active = isActive(child.slug)
                      return (
                        <Link
                          key={child.slug}
                          href={`/app/${child.slug}`}
                          className={[
                            'flex items-center gap-2 pl-9 pr-3 py-1.5 text-[10pt] font-calibri transition-colors',
                            active
                              ? 'bg-[#E7D6C5] text-[#6C4C2C] font-semibold border-l-2 border-[#6C4C2C] pl-[34px]'
                              : 'text-[#404040] hover:bg-[#EAE0D6]',
                          ].join(' ')}
                        >
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
                              'w-full flex items-center gap-2 pl-7 pr-3 py-1.5 text-[10pt] font-calibri transition-colors',
                              hasActive ? 'text-[#6C4C2C] font-semibold' : 'text-[#404040]',
                              'hover:bg-[#EAE0D6]',
                            ].join(' ')}
                          >
                            <svg
                              className={`shrink-0 transition-transform ${isGroupOpen ? 'rotate-90' : ''}`}
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
                                  'flex items-center pl-12 pr-3 py-1.5 text-[9.5pt] font-calibri transition-colors',
                                  active
                                    ? 'bg-[#E7D6C5] text-[#6C4C2C] font-semibold border-l-2 border-[#6C4C2C] pl-[46px]'
                                    : 'text-[#555] hover:bg-[#EAE0D6] hover:text-[#404040]',
                                ].join(' ')}
                              >
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
      <div className="border-t border-[#E5DDD3] relative">
        <button
          type="button"
          onClick={() => setUserMenuOpen(v => !v)}
          title={collapsed ? user.userName : undefined}
          className="w-full flex items-center gap-2 px-3 py-2 hover:bg-[#E7D6C5] transition-colors"
        >
          <div className="w-7 h-7 rounded-full bg-[#6C4C2C] text-white flex items-center justify-center text-[9pt] font-semibold shrink-0">
            {user.userName.charAt(0).toUpperCase()}
          </div>
          {!collapsed && (
            <>
              <div className="flex-1 text-left min-w-0">
                <p className="text-[9pt] font-semibold font-calibri text-[#404040] truncate">{user.userName}</p>
                <p className="text-[8pt] font-calibri text-[#888] truncate">{user.userId.toUpperCase()}</p>
              </div>
              <svg width="12" height="12" viewBox="0 0 16 16" fill="none" className="shrink-0 text-[#888]">
                <path d="M4 6l4 4 4-4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
              </svg>
            </>
          )}
        </button>

        {userMenuOpen && (
          <div className={`absolute bottom-full ${collapsed ? 'left-14' : 'left-0 right-0'} bg-white border border-[#E5DDD3] shadow-lg rounded-t-lg overflow-hidden z-50`}>
            <Link
              href="/app/security/change-password"
              onClick={() => setUserMenuOpen(false)}
              className="flex items-center gap-2 px-4 py-2.5 text-[10pt] font-calibri text-[#404040] hover:bg-[#F3EAE2] transition-colors"
            >
              Change Password
            </Link>
            <button
              type="button"
              onClick={handleLogout}
              className="w-full flex items-center gap-2 px-4 py-2.5 text-[10pt] font-calibri text-red-600 hover:bg-red-50 transition-colors"
            >
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
      width={16}
      height={16}
      className="shrink-0"
      style={{ color: 'currentColor' }}
    />
  )
}
