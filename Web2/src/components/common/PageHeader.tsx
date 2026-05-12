import { ReactNode } from 'react'

interface Props {
  title: string
  description?: string
  actions?: ReactNode
}

export default function PageHeader({ title, description, actions }: Props) {
  return (
    <div className="flex items-start justify-between gap-4 mb-6">
      <div>
        <h1 className="text-xl font-semibold text-txt-primary tracking-tight leading-tight">{title}</h1>
        {description && <p className="text-sm text-txt-tertiary mt-0.5">{description}</p>}
      </div>
      {actions && <div className="flex items-center gap-2 shrink-0 mt-0.5">{actions}</div>}
    </div>
  )
}
