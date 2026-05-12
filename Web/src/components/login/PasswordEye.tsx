'use client'

import Image from 'next/image'

interface Props {
  // 'hold' = press-and-hold to reveal (frmLogin), 'toggle' = click to toggle (frmSECChangePassword)
  mode:     'hold' | 'toggle'
  visible:  boolean
  onShow:   () => void
  onHide:   () => void
}

export default function PasswordEye({ mode, visible, onShow, onHide }: Props) {
  const src = visible ? '/icons/eye-open.svg' : '/icons/eye-off.svg'

  if (mode === 'hold') {
    return (
      <button
        type="button"
        tabIndex={-1}
        aria-label="Hold to reveal password"
        className="absolute right-2 top-1/2 -translate-y-1/2 cursor-pointer select-none"
        onMouseDown={onShow}
        onMouseUp={onHide}
        onMouseLeave={onHide}
        onTouchStart={onShow}
        onTouchEnd={onHide}
      >
        <Image src={src} alt="" width={16} height={16} />
      </button>
    )
  }

  // toggle mode
  return (
    <button
      type="button"
      tabIndex={-1}
      aria-label={visible ? 'Hide password' : 'Show password'}
      className="absolute right-2 top-1/2 -translate-y-1/2 cursor-pointer select-none"
      onClick={() => (visible ? onHide() : onShow())}
    >
      <Image src={src} alt="" width={16} height={16} />
    </button>
  )
}
