import { InputHTMLAttributes, SelectHTMLAttributes, TextareaHTMLAttributes, ReactNode } from 'react'

interface BaseProps {
  label: string
  required?: boolean
  error?: string
  className?: string
  hint?: string
}

interface InputProps extends BaseProps, InputHTMLAttributes<HTMLInputElement> {
  as?: 'input'
}

interface SelectProps extends BaseProps, SelectHTMLAttributes<HTMLSelectElement> {
  as: 'select'
  children: ReactNode
}

interface TextareaProps extends BaseProps, TextareaHTMLAttributes<HTMLTextAreaElement> {
  as: 'textarea'
}

type Props = InputProps | SelectProps | TextareaProps

const fieldClass = 'w2-input'

export default function FormField(props: Props) {
  const { label, required, error, hint, className = '', as = 'input', ...rest } = props

  return (
    <div className={['flex flex-col gap-1.5', className].join(' ')}>
      <label className="text-xs font-semibold text-txt-secondary tracking-wide uppercase">
        {label}
        {required && <span className="text-red-500 ml-0.5">*</span>}
      </label>
      {as === 'select' ? (
        <select className={fieldClass} {...(rest as SelectHTMLAttributes<HTMLSelectElement>)}>
          {(props as SelectProps).children}
        </select>
      ) : as === 'textarea' ? (
        <textarea
          className="w-full rounded-lg border border-border-strong bg-bg-surface px-3 py-2 text-sm text-txt-primary placeholder:text-txt-tertiary focus:outline-none focus:ring-2 focus:ring-brand-500 focus:border-brand-500 disabled:bg-bg-muted disabled:cursor-not-allowed resize-none transition-shadow duration-150"
          {...(rest as TextareaHTMLAttributes<HTMLTextAreaElement>)}
        />
      ) : (
        <input className={fieldClass} {...(rest as InputHTMLAttributes<HTMLInputElement>)} />
      )}
      {hint && !error && <p className="text-xs text-txt-tertiary">{hint}</p>}
      {error && <p className="text-xs text-red-600">{error}</p>}
    </div>
  )
}
