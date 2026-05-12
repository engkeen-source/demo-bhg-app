interface Props {
  onNew?: () => void
  onSave?: () => void
  onDelete?: () => void
  onPrint?: () => void
  onEmail?: () => void
  disableNew?: boolean
  disableSave?: boolean
  disableDelete?: boolean
}

export default function DocToolbar({ onNew, onSave, onDelete, onPrint, onEmail, disableNew, disableSave, disableDelete }: Props) {
  return (
    <div className="flex items-center gap-1">
      <ToolBtn icon={<PlusIcon />} label="New" onClick={onNew} disabled={disableNew} primary />
      <Divider />
      <ToolBtn icon={<SaveIcon />} label="Save" onClick={onSave} disabled={disableSave} />
      <ToolBtn icon={<PrintIcon />} label="Print" onClick={onPrint} />
      <ToolBtn icon={<EmailIcon />} label="Email" onClick={onEmail} />
      <div className="flex-1" />
      <Divider />
      <ToolBtn icon={<TrashIcon />} label="Delete" onClick={onDelete} disabled={disableDelete} danger />
    </div>
  )
}

function ToolBtn({ icon, label, onClick, disabled, primary, danger }: {
  icon: React.ReactNode; label: string; onClick?: () => void; disabled?: boolean; primary?: boolean; danger?: boolean
}) {
  return (
    <button
      type="button"
      onClick={onClick}
      disabled={disabled}
      title={label}
      className={[
        'flex items-center gap-1.5 px-3 py-1.5 rounded-lg text-xs font-medium transition-all duration-150',
        'focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-500',
        'disabled:opacity-40 disabled:cursor-not-allowed',
        primary ? 'bg-brand-600 text-white hover:bg-brand-700 shadow-sm' :
        danger  ? 'text-red-600 hover:bg-red-50' :
                  'text-txt-secondary hover:bg-bg-muted hover:text-txt-primary',
      ].join(' ')}
    >
      {icon}
      {label}
    </button>
  )
}

function Divider() {
  return <div className="w-px h-5 bg-border mx-1 shrink-0" />
}

function PlusIcon() {
  return <svg width="13" height="13" viewBox="0 0 16 16" fill="none"><path d="M8 2v12M2 8h12" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/></svg>
}

function SaveIcon() {
  return <svg width="13" height="13" viewBox="0 0 16 16" fill="none"><rect x="2" y="2" width="12" height="12" rx="1.5" stroke="currentColor" strokeWidth="1.5"/><path d="M5 2v4h6V2" stroke="currentColor" strokeWidth="1.5"/><rect x="4" y="9" width="8" height="3" rx="0.5" stroke="currentColor" strokeWidth="1.5"/></svg>
}

function PrintIcon() {
  return <svg width="13" height="13" viewBox="0 0 16 16" fill="none"><rect x="3" y="1" width="10" height="5" rx="0.5" stroke="currentColor" strokeWidth="1.5"/><rect x="3" y="9" width="10" height="6" rx="0.5" stroke="currentColor" strokeWidth="1.5"/><path d="M1 6h14v5H1z" stroke="currentColor" strokeWidth="1.5"/><circle cx="12" cy="8.5" r="0.75" fill="currentColor"/></svg>
}

function EmailIcon() {
  return <svg width="13" height="13" viewBox="0 0 16 16" fill="none"><rect x="1" y="3" width="14" height="10" rx="1.5" stroke="currentColor" strokeWidth="1.5"/><path d="M1 4l7 5 7-5" stroke="currentColor" strokeWidth="1.5"/></svg>
}

function TrashIcon() {
  return <svg width="13" height="13" viewBox="0 0 16 16" fill="none"><path d="M2 4h12M6 4V2h4v2M5 4v9a1 1 0 001 1h4a1 1 0 001-1V4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/></svg>
}
