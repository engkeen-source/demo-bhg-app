'use client'

import { useEffect, useState } from 'react'
import { getCurrentUser } from '@/lib/mockApi'
import type { UserInfo } from '@/lib/mockApi'
import Card from '@/components/common/Card'
import PageHeader from '@/components/common/PageHeader'
import Link from 'next/link'

export default function DashboardPage() {
  const [user, setUser] = useState<UserInfo | null>(null)

  useEffect(() => {
    setUser(getCurrentUser())
  }, [])

  const quickLinks = [
    { title: 'Quotation',       href: '/app/transactions/sales/quotation',     icon: '📋' },
    { title: 'Sales Order',     href: '/app/transactions/sales/sales-order',   icon: '🛒' },
    { title: 'Sales Invoice',   href: '/app/transactions/sales/sales-invoice', icon: '🧾' },
    { title: 'Payment Received', href: '/app/transactions/sales/payment-received', icon: '💳' },
    { title: 'Customer / Vendor', href: '/app/masters/customer-vendor',         icon: '👥' },
    { title: 'Inventory Item',  href: '/app/masters/inventory-item',           icon: '📦' },
  ]

  const stats = [
    { label: 'Open Sales Orders',   value: '12', change: '+3 this week',  color: 'text-blue-600' },
    { label: 'Pending Deliveries',  value: '5',  change: '2 due today',   color: 'text-orange-600' },
    { label: 'Outstanding AR',      value: 'RM 48,200', change: '4 invoices', color: 'text-red-600' },
    { label: 'Low Stock Items',     value: '3',  change: 'Reorder needed', color: 'text-yellow-700' },
  ]

  return (
    <div className="space-y-5">
      <PageHeader
        title={`Welcome back, ${user?.userName ?? 'User'}`}
        description={`${user?.companyNm ?? ''} · ${new Date().toLocaleDateString('en-GB', { weekday: 'long', day: 'numeric', month: 'long', year: 'numeric' })}`}
      />

      {/* Stats */}
      <div className="grid grid-cols-2 gap-3 lg:grid-cols-4">
        {stats.map(s => (
          <Card key={s.label} accent>
            <p className="text-[9pt] font-calibri text-[#888]">{s.label}</p>
            <p className={`text-[18pt] font-bold font-calibri mt-1 ${s.color}`}>{s.value}</p>
            <p className="text-[8pt] font-calibri text-[#AAA] mt-0.5">{s.change}</p>
          </Card>
        ))}
      </div>

      {/* Quick access */}
      <div>
        <h2 className="text-[11pt] font-bold font-calibri text-[#404040] mb-3">Quick Access</h2>
        <div className="grid grid-cols-2 gap-3 sm:grid-cols-3 lg:grid-cols-6">
          {quickLinks.map(link => (
            <Link
              key={link.href}
              href={link.href}
              className="flex flex-col items-center gap-2 p-4 bg-white border border-[#E5DDD3] rounded-lg hover:bg-[#F3EAE2] hover:border-[#6C4C2C]/40 transition-colors text-center group"
            >
              <span className="text-2xl">{link.icon}</span>
              <span className="text-[9pt] font-calibri font-medium text-[#404040] group-hover:text-[#6C4C2C] leading-tight">{link.title}</span>
            </Link>
          ))}
        </div>
      </div>

      {/* Recent activity */}
      <Card accent>
        <h2 className="text-[11pt] font-bold font-calibri text-[#404040] mb-3">Recent Activity</h2>
        <div className="space-y-2">
          {[
            { doc: 'SO-2026-0012', type: 'Sales Order',   customer: 'BossSO Retail Sdn Bhd',     amount: 'RM 3,140.00', date: '12 May 2026' },
            { doc: 'QO-2026-0009', type: 'Quotation',     customer: 'BossSO Holdings Berhad',    amount: 'RM 8,500.00', date: '11 May 2026' },
            { doc: 'PI-2026-0003', type: 'Purchase Invoice', customer: 'Vendor ABC Sdn Bhd',     amount: 'RM 2,200.00', date: '10 May 2026' },
            { doc: 'IV-2026-0007', type: 'Sales Invoice',  customer: 'BossSO Trading Sdn Bhd',   amount: 'RM 5,450.00', date: '09 May 2026' },
          ].map((row, i) => (
            <div key={i} className="flex items-center gap-3 py-2 border-b border-[#F3EAE2] last:border-0">
              <span className="text-[9pt] font-medium font-calibri text-[#6C4C2C] w-28 shrink-0">{row.doc}</span>
              <span className="text-[9pt] font-calibri text-[#888] w-28 shrink-0">{row.type}</span>
              <span className="text-[9pt] font-calibri text-[#404040] flex-1 truncate">{row.customer}</span>
              <span className="text-[9pt] font-calibri text-[#404040] tabular-nums shrink-0">{row.amount}</span>
              <span className="text-[8pt] font-calibri text-[#AAA] shrink-0">{row.date}</span>
            </div>
          ))}
        </div>
      </Card>
    </div>
  )
}
