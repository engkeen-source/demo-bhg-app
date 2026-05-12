import PageHeader from '@/components/common/PageHeader'
import Card from '@/components/common/Card'

interface Props {
  title: string
  desktop?: string
  description?: string
}

export default function ToolPage({ title, desktop, description }: Props) {
  return (
    <div className="space-y-4 max-w-2xl">
      <PageHeader title={title} />

      <Card accent>
        <p className="text-[10pt] font-calibri text-[#888]">
          {description ?? `This utility page provides access to the ${title} function.`}
        </p>
        <div className="mt-4 p-3 rounded bg-[#F3EAE2] text-[9pt] font-calibri text-[#6C4C2C]">
          Tool functionality available in Phase 2.
        </div>
      </Card>

      {desktop && (
        <p className="text-[8pt] font-calibri text-[#AAA]">
          Phase 2: connects to <code className="bg-[#F3EAE2] px-1 rounded">{desktop}</code>
        </p>
      )}
    </div>
  )
}
