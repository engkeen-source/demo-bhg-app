'use client'

import { useEffect, useState } from 'react'
import { getCurrentUser } from '@/lib/mockApi'
import type { UserInfo } from '@/lib/mockApi'
import Card from '@/components/common/Card'
import Badge from '@/components/common/Badge'
import Link from 'next/link'

export default function DashboardPage() {
  const [user, setUser] = useState<UserInfo | null>(null)

  useEffect(() => {
    setUser(getCurrentUser())
  }, [])

  const stats = [
    { label: 'Open Sales Orders', value: '12', delta: '+3 this week', deltaUp: true, color: 'text-brand-600' },
    { label: 'Pending Deliveries', value: '5', delta: '2 due today', deltaUp: false, color: 'text-amber-600' },
    { label: 'Outstanding AR', value: 'RM 48,200', delta: '4 invoices', deltaUp: false, color: 'text-red-600' },
    { label: 'Low Stock Items', value: '3', delta: 'Reorder needed', deltaUp: false, color: 'text-orange-600' },
  ]

  const quickLinks = [
    { title: 'Quotation',        href: '/app/transactions/sales/quotation',          icon: <DocIcon /> },
    { title: 'Sales Order',      href: '/app/transactions/sales/sales-order',        icon: <CartIcon /> },
    { title: 'Sales Invoice',    href: '/app/transactions/sales/sales-invoice',      icon: <ReceiptIcon /> },
    { title: 'Payment Received', href: '/app/transactions/sales/payment-received',   icon: <PayIcon /> },
    { title: 'Customer / Vendor', href: '/app/masters/customer-vendor',              icon: <PeopleIcon /> },
    { title: 'Inventory Item',   href: '/app/masters/inventory-item',               icon: <BoxIcon /> },
  ]

  const recentActivity = [
    { doc: 'SO-2026-0012', type: 'Sales Order',    customer: 'BossSO Retail Sdn Bhd',    amount: 'RM 3,140.00', date: '12 May', badge: 'open' as const },
    { doc: 'QO-2026-0009', type: 'Quotation',      customer: 'BossSO Holdings Berhad',   amount: 'RM 8,500.00', date: '11 May', badge: 'draft' as const },
    { doc: 'PI-2026-0003', type: 'Purchase Invoice', customer: 'Vendor ABC Sdn Bhd',     amount: 'RM 2,200.00', date: '10 May', badge: 'posted' as const },
    { doc: 'IV-2026-0007', type: 'Sales Invoice',  customer: 'BossSO Trading Sdn Bhd',  amount: 'RM 5,450.00', date: '09 May', badge: 'paid' as const },
  ]

  return (
    <div className="space-y-6">
      {/* Welcome */}
      <div>
        <h1 className="text-xl font-semibold text-txt-primary tracking-tight">
          Good day, {user?.userName ?? 'User'}
        </h1>
        <p className="text-sm text-txt-tertiary mt-0.5">
          {user?.companyNm} · {new Date().toLocaleDateString('en-GB', { weekday: 'long', day: 'numeric', month: 'long', year: 'numeric' })}
        </p>
      </div>

      {/* KPI cards */}
      <div className="grid grid-cols-2 gap-4 lg:grid-cols-4">
        {stats.map(s => (
          <Card key={s.label} className="hover:shadow-card-h transition-shadow">
            <p className="text-xs font-medium text-txt-tertiary uppercase tracking-wide">{s.label}</p>
            <p className={`text-2xl font-bold mt-2 tabular-nums ${s.color}`}>{s.value}</p>
            <p className="text-xs text-txt-tertiary mt-1">{s.delta}</p>
          </Card>
        ))}
      </div>

      {/* Quick access */}
      <div>
        <h2 className="text-sm font-semibold text-txt-primary mb-3">Quick Access</h2>
        <div className="grid grid-cols-2 gap-3 sm:grid-cols-3 lg:grid-cols-6">
          {quickLinks.map(link => (
            <Link
              key={link.href}
              href={link.href}
              className="flex flex-col items-center gap-2.5 p-4 bg-bg-surface border border-border rounded-xl hover:border-brand-300 hover:shadow-card-h transition-all group text-center"
            >
              <div className="w-10 h-10 rounded-xl bg-brand-50 group-hover:bg-brand-100 flex items-center justify-center transition-colors text-brand-500">
                {link.icon}
              </div>
              <span className="text-xs font-medium text-txt-secondary group-hover:text-brand-600 leading-tight transition-colors">{link.title}</span>
            </Link>
          ))}
        </div>
      </div>

      {/* Recent activity */}
      <Card noPad>
        <div className="px-5 py-4 border-b border-border">
          <h2 className="text-sm font-semibold text-txt-primary">Recent Activity</h2>
        </div>
        <div className="divide-y divide-border">
          {recentActivity.map((row, i) => (
            <div key={i} className="flex items-center gap-4 px-5 py-3.5 hover:bg-bg-muted transition-colors">
              <span className="text-xs font-semibold text-brand-600 w-28 shrink-0 tabular-nums">{row.doc}</span>
              <span className="text-xs text-txt-tertiary w-28 shrink-0">{row.type}</span>
              <span className="text-xs text-txt-primary flex-1 truncate">{row.customer}</span>
              <span className="text-xs font-medium text-txt-primary tabular-nums shrink-0">{row.amount}</span>
              <Badge variant={row.badge}>{row.badge.charAt(0).toUpperCase() + row.badge.slice(1)}</Badge>
              <span className="text-xs text-txt-tertiary shrink-0 w-12 text-right">{row.date}</span>
            </div>
          ))}
        </div>
      </Card>
    </div>
  )
}

function DocIcon() {
  return <svg width="18" height="18" viewBox="0 0 24 24" fill="none"><path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z" stroke="currentColor" strokeWidth="1.5"/><path d="M14 2v6h6M16 13H8M16 17H8M10 9H8" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/></svg>
}
function CartIcon() {
  return <svg width="18" height="18" viewBox="0 0 24 24" fill="none"><path d="M6 2L3 6v14a2 2 0 002 2h14a2 2 0 002-2V6l-3-4z" stroke="currentColor" strokeWidth="1.5" strokeLinejoin="round"/><line x1="3" y1="6" x2="21" y2="6" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/><path d="M16 10a4 4 0 01-8 0" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/></svg>
}
function ReceiptIcon() {
  return <svg width="18" height="18" viewBox="0 0 24 24" fill="none"><path d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/><rect x="9" y="3" width="6" height="4" rx="1" stroke="currentColor" strokeWidth="1.5"/><path d="M9 12h6M9 16h4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/></svg>
}
function PayIcon() {
  return <svg width="18" height="18" viewBox="0 0 24 24" fill="none"><rect x="2" y="5" width="20" height="14" rx="2" stroke="currentColor" strokeWidth="1.5"/><path d="M2 10h20" stroke="currentColor" strokeWidth="1.5"/></svg>
}
function PeopleIcon() {
  return <svg width="18" height="18" viewBox="0 0 24 24" fill="none"><path d="M17 21v-2a4 4 0 00-4-4H5a4 4 0 00-4 4v2" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/><circle cx="9" cy="7" r="4" stroke="currentColor" strokeWidth="1.5"/><path d="M23 21v-2a4 4 0 00-3-3.87M16 3.13a4 4 0 010 7.75" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/></svg>
}
function BoxIcon() {
  return <svg width="18" height="18" viewBox="0 0 24 24" fill="none"><path d="M21 16V8a2 2 0 00-1-1.73l-7-4a2 2 0 00-2 0l-7 4A2 2 0 003 8v8a2 2 0 001 1.73l7 4a2 2 0 002 0l7-4A2 2 0 0021 16z" stroke="currentColor" strokeWidth="1.5" strokeLinejoin="round"/><path d="M3.27 6.96L12 12.01l8.73-5.05M12 22.08V12" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/></svg>
}
