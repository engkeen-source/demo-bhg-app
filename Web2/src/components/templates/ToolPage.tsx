import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'

interface Props {
  title: string
  desktop?: string
  description?: string
}

export default function ToolPage({ title, desktop, description }: Props) {
  return (
    <div className="space-y-5 max-w-2xl">
      <PageHeader title={title} />

      <Card accent>
        <p className="text-sm text-txt-secondary">
          {description ?? `This utility page provides access to the ${title} function.`}
        </p>
        <div className="mt-5 flex items-center gap-3 p-4 bg-brand-50 rounded-xl border border-brand-100">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" className="text-brand-500 shrink-0">
            <circle cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="1.5"/>
            <path d="M12 8v4M12 16h.01" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/>
          </svg>
          <p className="text-xs text-brand-700">Tool functionality available in Phase 2.</p>
        </div>
      </Card>

      {desktop && (
        <p className="text-xs text-txt-tertiary">
          Phase 2: connects to <code className="bg-bg-muted px-1.5 py-0.5 rounded font-mono text-txt-secondary">{desktop}</code>
        </p>
      )}
    </div>
  )
}
