import Button from './Button'

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
    <div className="flex items-center gap-2">
      <Button variant="primary" size="sm" onClick={onNew} disabled={disableNew}>
        <svg width="12" height="12" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg">
          <path d="M8 2v12M2 8h12" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/>
        </svg>
        New
      </Button>
      <Button variant="secondary" size="sm" onClick={onSave} disabled={disableSave}>
        <svg width="12" height="12" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg">
          <rect x="2" y="2" width="12" height="12" rx="1" stroke="currentColor" strokeWidth="1.5"/>
          <path d="M5 2v4h6V2" stroke="currentColor" strokeWidth="1.5"/>
          <rect x="4" y="9" width="8" height="3" rx="0.5" stroke="currentColor" strokeWidth="1.5"/>
        </svg>
        Save
      </Button>
      <Button variant="ghost" size="sm" onClick={onPrint}>
        <svg width="12" height="12" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg">
          <rect x="3" y="1" width="10" height="6" rx="0.5" stroke="currentColor" strokeWidth="1.5"/>
          <rect x="3" y="9" width="10" height="6" rx="0.5" stroke="currentColor" strokeWidth="1.5"/>
          <path d="M1 6h14v5H1z" stroke="currentColor" strokeWidth="1.5"/>
          <circle cx="12" cy="8.5" r="0.75" fill="currentColor"/>
        </svg>
        Print
      </Button>
      <Button variant="ghost" size="sm" onClick={onEmail}>
        <svg width="12" height="12" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg">
          <rect x="1" y="3" width="14" height="10" rx="1" stroke="currentColor" strokeWidth="1.5"/>
          <path d="M1 4l7 5 7-5" stroke="currentColor" strokeWidth="1.5"/>
        </svg>
        Email
      </Button>
      <div className="flex-1" />
      <Button variant="danger" size="sm" onClick={onDelete} disabled={disableDelete}>
        <svg width="12" height="12" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg">
          <path d="M2 4h12M6 4V2h4v2M5 4v9a1 1 0 001 1h4a1 1 0 001-1V4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
        </svg>
        Delete
      </Button>
    </div>
  )
}
