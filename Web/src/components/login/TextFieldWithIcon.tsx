'use client'

import Image from 'next/image'
import { InputHTMLAttributes, forwardRef } from 'react'

interface Props extends Omit<InputHTMLAttributes<HTMLInputElement>, 'className'> {
  icon: string   // path under /icons/
  iconAlt: string
}

const TextFieldWithIcon = forwardRef<HTMLInputElement, Props>(
  ({ icon, iconAlt, ...rest }, ref) => {
    return (
      <div className="relative">
        {/* Left icon — matches WinForms Appearance.Image on the editor */}
        <span className="absolute left-2 top-1/2 -translate-y-1/2 flex items-center pointer-events-none">
          <Image src={`/icons/${icon}`} alt={iconAlt} width={14} height={14} />
        </span>
        <input
          ref={ref}
          {...rest}
          className={[
            'w-full h-[26px] text-[11pt] font-calibri',
            'border border-gray-400 bg-white',
            'pl-7 pr-2',
            'focus:outline-none focus:ring-1 focus:ring-blue-400',
            'placeholder-gray-400',
            rest.disabled ? 'bg-gray-100 text-gray-500 cursor-not-allowed' : '',
          ].join(' ')}
        />
      </div>
    )
  }
)

TextFieldWithIcon.displayName = 'TextFieldWithIcon'
export default TextFieldWithIcon
