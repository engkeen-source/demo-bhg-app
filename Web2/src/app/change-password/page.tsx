'use client'

import { useState, useEffect } from 'react'
import { useRouter } from 'next/navigation'
import PasswordEye from '@/components/login/PasswordEye'
import PasswordRulesPanel from '@/components/changePassword/PasswordRulesPanel'
import { checkPasswordRules, allRulesPassed } from '@/lib/validators'
import { changePassword, getCurrentUser } from '@/lib/mockApi'

export default function ChangePasswordPage() {
  const router = useRouter()

  const [user,          setUser]         = useState<{ userId: string; userName: string } | null>(null)
  const [oldPw,         setOldPw]        = useState('')
  const [newPw,         setNewPw]        = useState('')
  const [confirmPw,     setConfirmPw]    = useState('')
  const [email,         setEmail]        = useState('')
  const [oldPwVisible,  setOldPwVisible] = useState(false)
  const [newPwVisible,  setNewPwVisible] = useState(false)
  const [confPwVisible, setConfPwVisible] = useState(false)
  const [submitting,    setSubmitting]   = useState(false)
  const [msg,           setMsg]          = useState<{ text: string; ok: boolean } | null>(null)

  const rules    = newPw.length > 0 ? checkPasswordRules(newPw) : null
  const hasInput = newPw.length > 0

  useEffect(() => {
    const u = getCurrentUser()
    if (!u) { router.push('/login'); return }
    setUser({ userId: u.userId, userName: u.userName })
  }, [router])

  async function handleSave() {
    setMsg(null)
    if (!oldPw)     { setMsg({ text: 'Please enter your Old Password / OTP.', ok: false }); return }
    if (!newPw)     { setMsg({ text: 'Please enter a New Password.', ok: false }); return }
    if (!confirmPw) { setMsg({ text: 'Please confirm your New Password.', ok: false }); return }
    if (rules && !allRulesPassed(rules)) { setMsg({ text: 'New password does not meet all requirements.', ok: false }); return }
    if (newPw !== confirmPw) { setMsg({ text: 'New Password and Confirm Password do not match.', ok: false }); return }

    setSubmitting(true)
    const result = await changePassword({ userId: user?.userId ?? '', oldPassword: oldPw, newPassword: newPw, confirmPassword: confirmPw, email })
    setSubmitting(false)
    setMsg({ text: result.message, ok: result.success })
  }

  return (
    <div className="min-h-screen bg-bg-base flex items-start justify-center p-8">
      <div className="w-full max-w-2xl">
        {/* Back */}
        <button
          type="button"
          onClick={() => router.push('/app')}
          className="flex items-center gap-1.5 text-sm text-txt-secondary hover:text-txt-primary mb-6 transition-colors"
        >
          <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
            <path d="M10 4L6 8l4 4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/>
          </svg>
          Back to app
        </button>

        <div className="mb-6">
          <h1 className="text-xl font-semibold text-txt-primary">Change Password</h1>
          <p className="text-sm text-txt-tertiary mt-0.5">Update your account password</p>
        </div>

        {msg && (
          <div className={`mb-5 text-sm border rounded-lg px-4 py-3 ${msg.ok ? 'bg-emerald-50 border-emerald-200 text-emerald-800' : 'bg-red-50 border-red-200 text-red-700'}`}>
            {msg.text}
          </div>
        )}

        <div className="bg-bg-surface border border-border rounded-xl shadow-card p-6 flex gap-8">
          {/* Left: fields */}
          <div className="flex-1 space-y-4">
            <div className="flex flex-col gap-1.5">
              <label className="text-xs font-semibold text-txt-secondary uppercase tracking-wide">User ID</label>
              <input type="text" readOnly value={user?.userId ?? ''} className="w2-input" />
            </div>
            <div className="flex flex-col gap-1.5">
              <label className="text-xs font-semibold text-txt-secondary uppercase tracking-wide">User Name</label>
              <input type="text" readOnly value={user?.userName ?? ''} className="w2-input" />
            </div>

            <div className="flex flex-col gap-1.5">
              <label className="text-xs font-semibold text-txt-secondary uppercase tracking-wide">Old Password / OTP</label>
              <div className="relative">
                <input
                  type={oldPwVisible ? 'text' : 'password'}
                  value={oldPw}
                  onChange={e => setOldPw(e.target.value)}
                  disabled={submitting}
                  placeholder="Enter old password or OTP"
                  className="w2-input pr-10"
                />
                <PasswordEye mode="toggle" visible={oldPwVisible} onShow={() => setOldPwVisible(true)} onHide={() => setOldPwVisible(false)} />
              </div>
            </div>

            <div className="flex flex-col gap-1.5">
              <label className="text-xs font-semibold text-txt-secondary uppercase tracking-wide">New Password</label>
              <div className="relative">
                <input
                  type={newPwVisible ? 'text' : 'password'}
                  value={newPw}
                  onChange={e => setNewPw(e.target.value)}
                  onBlur={() => setNewPwVisible(false)}
                  disabled={submitting}
                  placeholder="Enter new password"
                  className="w2-input pr-10"
                />
                <PasswordEye mode="toggle" visible={newPwVisible} onShow={() => setNewPwVisible(true)} onHide={() => setNewPwVisible(false)} />
              </div>
            </div>

            <div className="flex flex-col gap-1.5">
              <label className="text-xs font-semibold text-txt-secondary uppercase tracking-wide">Confirm Password</label>
              <div className="relative">
                <input
                  type={confPwVisible ? 'text' : 'password'}
                  value={confirmPw}
                  onChange={e => setConfirmPw(e.target.value)}
                  onBlur={() => setConfPwVisible(false)}
                  disabled={submitting}
                  placeholder="Confirm new password"
                  className="w2-input pr-10"
                />
                <PasswordEye mode="toggle" visible={confPwVisible} onShow={() => setConfPwVisible(true)} onHide={() => setConfPwVisible(false)} />
              </div>
            </div>

            <div className="flex flex-col gap-1.5">
              <label className="text-xs font-semibold text-txt-secondary uppercase tracking-wide">Email</label>
              <input
                type="email"
                value={email}
                onChange={e => setEmail(e.target.value)}
                disabled={submitting}
                placeholder="user@example.com"
                className="w2-input"
              />
              <p className="text-xs text-txt-tertiary">Used to receive OTP if you forget your password.</p>
            </div>

            <div className="flex gap-2 pt-2">
              <button
                type="button"
                onClick={handleSave}
                disabled={submitting}
                className="h-10 px-6 bg-brand-600 text-white text-sm font-semibold rounded-lg hover:bg-brand-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors shadow-sm"
              >
                {submitting ? 'Saving…' : 'Save Changes'}
              </button>
              <button
                type="button"
                onClick={() => router.push('/app')}
                disabled={submitting}
                className="h-10 px-6 bg-bg-surface text-txt-secondary text-sm font-medium rounded-lg border border-border hover:bg-bg-muted transition-colors"
              >
                Cancel
              </button>
            </div>
          </div>

          {/* Right: rules */}
          <div className="w-56 shrink-0">
            <PasswordRulesPanel rules={rules} hasInput={hasInput} />
          </div>
        </div>
      </div>
    </div>
  )
}
