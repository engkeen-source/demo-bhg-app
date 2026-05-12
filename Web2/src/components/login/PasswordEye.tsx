'use client'

interface Props {
  mode: 'hold' | 'toggle'
  visible: boolean
  onShow: () => void
  onHide: () => void
}

export default function PasswordEye({ mode, visible, onShow, onHide }: Props) {
  const icon = visible ? (
    <svg width="16" height="16" viewBox="0 0 24 24" fill="none">
      <path d="M2 12s3-7 10-7 10 7 10 7-3 7-10 7-10-7-10-7z" stroke="currentColor" strokeWidth="1.5"/>
      <circle cx="12" cy="12" r="3" stroke="currentColor" strokeWidth="1.5"/>
    </svg>
  ) : (
    <svg width="16" height="16" viewBox="0 0 24 24" fill="none">
      <path d="M17.94 17.94A10.07 10.07 0 0112 20c-7 0-11-8-11-8a18.45 18.45 0 015.06-5.94M9.9 4.24A9.12 9.12 0 0112 4c7 0 11 8 11 8a18.5 18.5 0 01-2.16 3.19M1 1l22 22" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
    </svg>
  )

  if (mode === 'hold') {
    return (
      <button
        type="button"
        tabIndex={-1}
        aria-label="Hold to reveal password"
        className="absolute right-3 top-1/2 -translate-y-1/2 text-txt-tertiary hover:text-txt-secondary cursor-pointer select-none"
        onMouseDown={onShow}
        onMouseUp={onHide}
        onMouseLeave={onHide}
        onTouchStart={onShow}
        onTouchEnd={onHide}
      >
        {icon}
      </button>
    )
  }

  return (
    <button
      type="button"
      tabIndex={-1}
      aria-label={visible ? 'Hide password' : 'Show password'}
      className="absolute right-3 top-1/2 -translate-y-1/2 text-txt-tertiary hover:text-txt-secondary cursor-pointer select-none"
      onClick={() => (visible ? onHide() : onShow())}
    >
      {icon}
    </button>
  )
}
