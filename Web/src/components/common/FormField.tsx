import { InputHTMLAttributes, SelectHTMLAttributes, ReactNode } from 'react'

interface BaseProps {
  label: string
  required?: boolean
  error?: string
  className?: string
}

interface InputProps extends BaseProps, InputHTMLAttributes<HTMLInputElement> {
  as?: 'input'
}

interface SelectProps extends BaseProps, SelectHTMLAttributes<HTMLSelectElement> {
  as: 'select'
  children: ReactNode
}

type Props = InputProps | SelectProps

const inputClass = 'h-9 w-full rounded border border-[#D8CFC4] bg-white px-2.5 text-[10pt] font-calibri text-[#404040] focus:outline-none focus:ring-2 focus:ring-[#6C4C2C]/30 focus:border-[#6C4C2C] disabled:bg-[#F3EAE2] disabled:text-[#888]'

export default function FormField(props: Props) {
  const { label, required, error, className = '', as = 'input', ...rest } = props

  return (
    <div className={['flex flex-col gap-1', className].join(' ')}>
      <label className="text-[9pt] font-calibri font-semibold text-[#404040]">
        {label}
        {required && <span className="text-red-500 ml-0.5">*</span>}
      </label>
      {as === 'select' ? (
        <select className={inputClass} {...(rest as SelectHTMLAttributes<HTMLSelectElement>)}>
          {(props as SelectProps).children}
        </select>
      ) : (
        <input className={inputClass} {...(rest as InputHTMLAttributes<HTMLInputElement>)} />
      )}
      {error && <p className="text-[9pt] text-red-600">{error}</p>}
    </div>
  )
}
