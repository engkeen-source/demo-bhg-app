import { HTMLAttributes } from 'react'

interface Props extends HTMLAttributes<HTMLDivElement> {
  accent?: boolean
  noPad?: boolean
}

export default function Card({ accent = false, noPad = false, className = '', children, ...rest }: Props) {
  return (
    <div
      className={[
        'bg-bg-surface border border-border rounded-xl shadow-card overflow-hidden',
        accent ? 'border-t-2 border-t-brand-500' : '',
        className,
      ].join(' ')}
      {...rest}
    >
      {noPad ? children : <div className="p-5">{children}</div>}
    </div>
  )
}
