'use client'

import { useState, useRef, useEffect } from 'react'
import { useRouter } from 'next/navigation'
import { logout } from '@/lib/mockApi'

// Top-level menu structure matching frmMain Designer (UltraToolbarsManager utolmgrMain)
const MENUS = [
  {
    label: 'File',
    items: ['Change Password', 'Audit Log', 'Switch User', 'System Lock List', 'Period', 'Company Setup', '---', 'Exit'],
  },
  {
    label: 'Security',
    items: ['Group', 'User', 'User List', 'Account Group'],
  },
  {
    label: 'Settings',
    items: ['System Option', 'User Option', 'Document Code', 'General List', 'Wildcard Search List'],
  },
  {
    label: 'Masters',
    items: ['Customer', 'Job', 'Chart of Account', 'Sales Representative', 'Inventory Item', 'Currency', 'Brand'],
  },
  {
    label: 'References',
    items: ['UOM', 'Bank', 'Color', 'Document Group', 'Equipment Type', 'Industry', 'Location', 'Payment Mode', 'Shipping Mode', 'Tax Authority', 'Tax Group', 'Payment Term', 'Territory'],
  },
  {
    label: 'Transactions',
    items: ['Sales ▶', 'Purchase ▶'],
    submenus: {
      'Sales ▶':    ['Quotation', 'Sales Order', 'Delivery Order', 'Sales Invoice', 'Sales Credit Note', 'Payment Received'],
      'Purchase ▶': ['Purchase Order', 'Purchase Delivery', 'Purchase Invoice', 'Payment Issue'],
    },
  },
  { label: 'Reports',    items: ['Quotation Reports', 'Sales Order Reports', 'Invoice Reports', 'AR Reports'] },
  { label: 'Definition', items: ['Financial Report Designer'] },
  { label: 'Windows',    items: ['Cascade', 'Tile Horizontal', 'Tile Vertical', 'Close All'] },
  { label: 'Help',       items: ['About BossSO'] },
]

export default function MainMenuBar() {
  const router = useRouter()
  const [open, setOpen] = useState<number | null>(null)
  const menuRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    function handleClickOutside(e: MouseEvent) {
      if (menuRef.current && !menuRef.current.contains(e.target as Node)) {
        setOpen(null)
      }
    }
    document.addEventListener('mousedown', handleClickOutside)
    return () => document.removeEventListener('mousedown', handleClickOutside)
  }, [])

  function handleItemClick(menuLabel: string, item: string) {
    setOpen(null)
    if (menuLabel === 'File' && item === 'Exit') {
      logout()
      router.push('/login')
    }
    if (menuLabel === 'File' && item === 'Change Password') {
      router.push('/change-password')
    }
    if (menuLabel === 'File' && item === 'Switch User') {
      logout()
      router.push('/login')
    }
  }

  return (
    <div
      ref={menuRef}
      className="flex items-stretch text-[10pt] font-calibri text-[#404040] select-none"
      style={{ background: '#E7D6C5', borderBottom: '1px solid #C8B4A0', height: '22px' }}
    >
      {MENUS.map((menu, idx) => (
        <div key={menu.label} className="relative">
          {/* Menu title */}
          <button
            type="button"
            className={[
              'h-full px-2.5 text-[10pt] font-calibri bg-transparent border-none cursor-pointer',
              open === idx ? 'bg-blue-600 text-white' : 'hover:bg-black/[0.08]',
            ].join(' ')}
            onMouseDown={() => setOpen(open === idx ? null : idx)}
            onMouseEnter={() => open !== null && setOpen(idx)}
          >
            {menu.label}
          </button>

          {/* Dropdown */}
          {open === idx && (
            <div
              className="absolute left-0 top-full z-50 bg-white border border-[#C8B4A0] shadow-lg min-w-[180px] py-0.5"
              style={{ fontSize: '10pt' }}
            >
              {menu.items.map(item => (
                item === '---'
                  ? <div key={item} className="border-t border-gray-300 my-0.5" />
                  : (
                    <button
                      key={item}
                      type="button"
                      className="w-full text-left px-4 py-[3px] text-[10pt] font-calibri text-[#404040] hover:bg-blue-600 hover:text-white cursor-pointer bg-transparent border-none"
                      onClick={() => handleItemClick(menu.label, item)}
                    >
                      {item}
                    </button>
                  )
              ))}
            </div>
          )}
        </div>
      ))}
    </div>
  )
}
