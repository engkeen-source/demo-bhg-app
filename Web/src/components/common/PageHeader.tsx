import { ReactNode } from 'react'

interface Props {
  title: string
  description?: string
  actions?: ReactNode
}

export default function PageHeader({ title, description, actions }: Props) {
  return (
    <div className="flex items-center justify-between gap-4 mb-4">
      <div>
        <h1 className="text-[15pt] font-semibold font-calibri text-[#404040] leading-tight">{title}</h1>
        {description && <p className="text-[9pt] font-calibri text-[#888] mt-0.5">{description}</p>}
      </div>
      {actions && <div className="flex items-center gap-2 shrink-0">{actions}</div>}
    </div>
  )
}
