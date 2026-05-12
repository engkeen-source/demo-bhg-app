'use client'

import Image from 'next/image'
import { SelectHTMLAttributes } from 'react'
import { Company } from '@/lib/mockApi'

interface Props extends Omit<SelectHTMLAttributes<HTMLSelectElement>, 'className'> {
  companies: Company[]
  loading?:  boolean
}

export default function CompanySelect({ companies, loading, ...rest }: Props) {
  return (
    <div className="relative">
      {/* Home icon on the left */}
      <span className="absolute left-2 top-1/2 -translate-y-1/2 flex items-center pointer-events-none z-10">
        <Image src="/icons/home.svg" alt="company" width={14} height={14} />
      </span>

      <select
        {...rest}
        className={[
          'w-full h-[26px] text-[11pt] font-calibri',
          'border border-gray-400 bg-white',
          'pl-7 pr-2 appearance-none',
          'focus:outline-none focus:ring-1 focus:ring-blue-400',
          !rest.value ? 'text-gray-400' : 'text-boss-dark',
          rest.disabled ? 'bg-gray-100 cursor-not-allowed' : '',
        ].join(' ')}
      >
        <option value="" className="text-gray-400">
          {loading ? 'Loading companies…' : 'Select a company'}
        </option>
        {companies.map(c => (
          <option key={c.databaseId} value={c.databaseId} className="text-boss-dark">
            {c.companyNm}
          </option>
        ))}
      </select>

      {/* Dropdown arrow */}
      <span className="absolute right-2 top-1/2 -translate-y-1/2 pointer-events-none text-gray-500 text-xs">▼</span>
    </div>
  )
}
