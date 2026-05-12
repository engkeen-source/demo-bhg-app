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
    <div className="flex border-b border-border gap-0 overflow-x-auto">
      {tabs.map(tab => (
        <button
          key={tab.id}
          type="button"
          onClick={() => onChange(tab.id)}
          className={[
            'px-4 py-2.5 text-sm font-medium transition-colors relative whitespace-nowrap shrink-0',
            'focus:outline-none',
            active === tab.id
              ? 'text-brand-600 after:absolute after:bottom-0 after:left-0 after:right-0 after:h-0.5 after:bg-brand-600 after:rounded-t'
              : 'text-txt-tertiary hover:text-txt-secondary hover:bg-bg-muted rounded-t-lg',
          ].join(' ')}
        >
          {tab.label}
        </button>
      ))}
    </div>
  )
}
