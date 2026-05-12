'use client'

interface Tab {
  id: string
  label: string
}

interface Props {
  tabs: Tab[]
  active: string
  onChange: (id: string) => void
}

export default function TabBar({ tabs, active, onChange }: Props) {
  return (
    <div className="flex border-b border-[#E5DDD3] gap-0">
      {tabs.map(tab => (
        <button
          key={tab.id}
          type="button"
          onClick={() => onChange(tab.id)}
          className={[
            'px-4 py-2 text-[10pt] font-calibri font-medium transition-colors relative',
            active === tab.id
              ? 'text-[#6C4C2C] after:absolute after:bottom-0 after:left-0 after:right-0 after:h-0.5 after:bg-[#6C4C2C]'
              : 'text-[#888] hover:text-[#404040]',
          ].join(' ')}
        >
          {tab.label}
        </button>
      ))}
    </div>
  )
}
