import { InputHTMLAttributes, SelectHTMLAttributes, TextareaHTMLAttributes, ReactNode } from 'react'

interface BaseProps {
  label: string
  required?: boolean
  error?: string
  className?: string
}

interface InputProps extends BaseProps, InputHTMLAttributes<HTMLInputElement> {
  as?: 'input'
  adornment?: ReactNode
}

interface SelectProps extends BaseProps, SelectHTMLAttributes<HTMLSelectElement> {
  as: 'select'
  children: ReactNode
}

interface TextareaProps extends BaseProps, TextareaHTMLAttributes<HTMLTextAreaElement> {
  as: 'textarea'
}

interface CheckboxProps extends BaseProps {
  as: 'checkbox'
  checked?: boolean
  defaultChecked?: boolean
  onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void
}

type Props = InputProps | SelectProps | TextareaProps | CheckboxProps

const inputClass = 'h-9 w-full rounded border border-[#D8CFC4] bg-white px-2.5 text-[10pt] font-calibri text-[#404040] focus:outline-none focus:ring-2 focus:ring-[#6C4C2C]/30 focus:border-[#6C4C2C] disabled:bg-[#F3EAE2] disabled:text-[#888]'
const textareaClass = 'w-full rounded border border-[#D8CFC4] bg-white px-2.5 py-2 text-[10pt] font-calibri text-[#404040] resize-none focus:outline-none focus:ring-2 focus:ring-[#6C4C2C]/30 focus:border-[#6C4C2C]'

export default function FormField(props: Props) {
  const { label, required, error, className = '', as = 'input', ...rest } = props

  if (as === 'checkbox') {
    const { checked, defaultChecked, onChange } = props as CheckboxProps
    return (
      <div className={['flex items-center gap-2', className].join(' ')}>
        <input
          type="checkbox"
          id={label}
          checked={checked}
          defaultChecked={defaultChecked}
          onChange={onChange}
          className="h-3.5 w-3.5 accent-[#6C4C2C] cursor-pointer"
        />
        <label htmlFor={label} className="text-[10pt] font-calibri text-[#404040] cursor-pointer select-none">
          {label}
          {required && <span className="text-red-500 ml-0.5">*</span>}
        </label>
        {error && <p className="text-[9pt] text-red-600">{error}</p>}
      </div>
    )
  }

  if (as === 'textarea') {
    const { label: _l, required: _r, error: _e, className: _c, as: _a, ...textareaRest } = props as TextareaProps
    return (
      <div className={['flex flex-col gap-1', className].join(' ')}>
        <label className="text-[9pt] font-calibri font-semibold text-[#404040]">
          {label}
          {required && <span className="text-red-500 ml-0.5">*</span>}
        </label>
        <textarea className={textareaClass} {...textareaRest} />
        {error && <p className="text-[9pt] text-red-600">{error}</p>}
      </div>
    )
  }

  if (as === 'select') {
    return (
      <div className={['flex flex-col gap-1', className].join(' ')}>
        <label className="text-[9pt] font-calibri font-semibold text-[#404040]">
          {label}
          {required && <span className="text-red-500 ml-0.5">*</span>}
        </label>
        <select className={inputClass} {...(rest as SelectHTMLAttributes<HTMLSelectElement>)}>
          {(props as SelectProps).children}
        </select>
        {error && <p className="text-[9pt] text-red-600">{error}</p>}
      </div>
    )
  }

  // input (default)
  const { adornment, label: _l, required: _r, error: _e, className: _c, as: _a, ...inputRest } = props as InputProps
  return (
    <div className={['flex flex-col gap-1', className].join(' ')}>
      <label className="text-[9pt] font-calibri font-semibold text-[#404040]">
        {label}
        {required && <span className="text-red-500 ml-0.5">*</span>}
      </label>
      <div className="relative">
        <input className={[inputClass, adornment ? 'pr-8' : ''].join(' ')} {...inputRest} />
        {adornment && (
          <div className="absolute inset-y-0 right-0 flex items-center pr-2 pointer-events-none text-[#888]">
            {adornment}
          </div>
        )}
      </div>
      {error && <p className="text-[9pt] text-red-600">{error}</p>}
    </div>
  )
}
