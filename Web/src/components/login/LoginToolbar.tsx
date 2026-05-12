'use client'

import Image from 'next/image'

interface Props {
  version:    string
  onLogin:    () => void
  onExit:     () => void
  isLoading?: boolean
}

export default function LoginToolbar({ version, onLogin, onExit, isLoading }: Props) {
  return (
    <div
      className="flex items-center px-1 border-b border-[#C8B4A0]"
      style={{ background: '#E7D6C5', height: '74px' }}
    >
      {/* Left: Exit + Login buttons */}
      <div className="flex items-center gap-0">
        <ToolbarButton
          icon="/icons/door-exit.svg"
          label="Exit"
          accessKey="x"
          onClick={onExit}
          disabled={isLoading}
        />
        <div className="w-px h-10 bg-[#C8B4A0] mx-0.5" />
        <ToolbarButton
          icon="/icons/user-login.svg"
          label="Login"
          accessKey="l"
          onClick={onLogin}
          disabled={isLoading}
          primary
        />
      </div>

      {/* Spacer */}
      <div className="flex-1" />

      {/* Right: Version */}
      <div className="flex items-center gap-1 pr-2 text-boss-dark text-[10pt] italic font-calibri">
        <span>Version :</span>
        <span className="font-semibold">{version}</span>
      </div>
    </div>
  )
}

function ToolbarButton({
  icon, label, accessKey, onClick, disabled, primary,
}: {
  icon: string; label: string; accessKey: string
  onClick: () => void; disabled?: boolean; primary?: boolean
}) {
  return (
    <button
      type="button"
      onClick={onClick}
      disabled={disabled}
      className={[
        'flex flex-col items-center justify-center gap-[3px]',
        'w-[70px] h-[55px] text-[10.5pt] italic font-calibri text-boss-dark',
        'bg-transparent border-none cursor-pointer select-none rounded-sm',
        'hover:bg-black/[0.06] active:bg-black/[0.12] transition-colors',
        disabled ? 'opacity-50 cursor-not-allowed' : '',
        primary && !disabled ? 'hover:bg-blue-900/10' : '',
      ].join(' ')}
    >
      <Image src={icon} alt={label} width={28} height={28} />
      <span>
        {label.split('').map((ch, i) =>
          ch.toLowerCase() === accessKey
            ? <u key={i}>{ch}</u>
            : ch
        )}
      </span>
    </button>
  )
}
