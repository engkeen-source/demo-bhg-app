import { ButtonHTMLAttributes } from 'react'

type Variant = 'primary' | 'secondary' | 'ghost' | 'danger'
type Size = 'sm' | 'md'

interface Props extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: Variant
  size?: Size
}

export default function Button({ variant = 'primary', size = 'md', className = '', children, ...rest }: Props) {
  const base = 'inline-flex items-center gap-1.5 font-calibri font-medium rounded transition-colors focus:outline-none focus:ring-2 focus:ring-[#6C4C2C]/30 disabled:opacity-50 disabled:cursor-not-allowed'

  const variants: Record<Variant, string> = {
    primary:   'bg-[#6C4C2C] text-white hover:bg-[#553A20] border border-[#6C4C2C]',
    secondary: 'bg-white text-[#6C4C2C] border border-[#6C4C2C] hover:bg-[#F3EAE2]',
    ghost:     'bg-transparent text-[#6C4C2C] hover:bg-[#F3EAE2] border border-transparent',
    danger:    'bg-red-600 text-white hover:bg-red-700 border border-red-600',
  }

  const sizes: Record<Size, string> = {
    sm: 'px-2.5 py-1 text-[9pt]',
    md: 'px-3.5 py-1.5 text-[10pt]',
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
