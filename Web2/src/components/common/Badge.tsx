type BadgeVariant = 'default' | 'draft' | 'open' | 'posted' | 'paid' | 'cancelled' | 'active' | 'inactive'

interface Props {
  children: React.ReactNode
  variant?: BadgeVariant
}

const styles: Record<BadgeVariant, string> = {
  default:   'bg-bg-muted text-txt-secondary',
  draft:     'bg-zinc-100 text-zinc-600',
  open:      'bg-blue-50 text-blue-700',
  posted:    'bg-emerald-50 text-emerald-700',
  paid:      'bg-brand-50 text-brand-600',
  cancelled: 'bg-red-50 text-red-600',
  active:    'bg-emerald-50 text-emerald-700',
  inactive:  'bg-zinc-100 text-zinc-500',
}

export default function Badge({ children, variant = 'default' }: Props) {
  return (
    <span className={`inline-flex items-center px-2 py-0.5 rounded-md text-xs font-medium ${styles[variant]}`}>
      {children}
    </span>
  )
}
