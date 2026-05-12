import { ButtonHTMLAttributes } from 'react'

type Variant = 'primary' | 'secondary' | 'ghost' | 'danger'
type Size = 'sm' | 'md' | 'lg'

interface Props extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: Variant
  size?: Size
}

export default function Button({ variant = 'primary', size = 'md', className = '', children, ...rest }: Props) {
  const base = 'inline-flex items-center gap-1.5 font-medium rounded-lg transition-all duration-150 focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-500 focus-visible:ring-offset-1 disabled:opacity-50 disabled:cursor-not-allowed select-none'

  const variants: Record<Variant, string> = {
    primary:   'bg-brand-600 text-white hover:bg-brand-700 shadow-sm hover:shadow',
    secondary: 'bg-bg-surface text-txt-primary border border-border hover:bg-bg-muted shadow-sm',
    ghost:     'bg-transparent text-txt-secondary hover:bg-bg-muted hover:text-txt-primary',
    danger:    'bg-red-600 text-white hover:bg-red-700 shadow-sm hover:shadow',
  }

  const sizes: Record<Size, string> = {
    sm: 'px-3 py-1.5 text-xs',
    md: 'px-4 py-2 text-sm',
    lg: 'px-5 py-2.5 text-sm',
  }

  return (
    <button
      className={[base, variants[variant], sizes[size], className].join(' ')}
      {...rest}
    >
      {children}
    </button>
  )
}
