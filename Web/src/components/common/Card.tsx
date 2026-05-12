import { HTMLAttributes } from 'react'

interface Props extends HTMLAttributes<HTMLDivElement> {
  accent?: boolean
  noPad?: boolean
}

export default function Card({ accent = false, noPad = false, className = '', children, ...rest }: Props) {
  return (
    <div
      className={[
        'bg-white border border-[#E5DDD3] rounded-lg overflow-hidden',
        accent ? 'border-t-2 border-t-[#6C4C2C]' : '',
        className,
      ].join(' ')}
      {...rest}
    >
      {noPad ? children : <div className="p-4">{children}</div>}
    </div>
  )
}
