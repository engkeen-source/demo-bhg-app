'use client'

import { useState, useEffect, useRef } from 'react'
import { useRouter } from 'next/navigation'
import Image from 'next/image'
import PasswordEye from '@/components/login/PasswordEye'
import PasswordRulesPanel from '@/components/changePassword/PasswordRulesPanel'
import { checkPasswordRules, allRulesPassed } from '@/lib/validators'
import { changePassword, getCurrentUser } from '@/lib/mockApi'

interface Field {
  label:    string
  value:    string
  onChange: (v: string) => void
  readOnly?: boolean
  type?:    'text' | 'password' | 'email'
  eye?:     boolean
  tooltip?: string
  tabIndex: number
}

export default function ChangePasswordPage() {
  const router = useRouter()

  const [user,           setUser]          = useState<{ userId: string; userName: string } | null>(null)
  const [oldPw,          setOldPw]         = useState('')
  const [newPw,          setNewPw]         = useState('')
  const [confirmPw,      setConfirmPw]     = useState('')
  const [email,          setEmail]         = useState('')
  const [oldPwVisible,   setOldPwVisible]  = useState(false)
  const [newPwVisible,   setNewPwVisible]  = useState(false)
  const [confPwVisible,  setConfPwVisible] = useState(false)
  const [submitting,     setSubmitting]    = useState(false)
  const [msg,            setMsg]           = useState<{ text: string; ok: boolean } | null>(null)

  const rules    = newPw.length > 0 ? checkPasswordRules(newPw) : null
  const hasInput = newPw.length > 0

  useEffect(() => {
    const u = getCurrentUser()
    if (!u) { router.push('/login'); return }
    setUser({ userId: u.userId, userName: u.userName })
  }, [router])

  async function handleSave() {
    setMsg(null)

    if (!oldPw) { setMsg({ text: 'Please enter your Old Password / OTP.', ok: false }); return }
    if (!newPw) { setMsg({ text: 'Please enter a New Password.', ok: false }); return }
    if (!confirmPw) { setMsg({ text: 'Please confirm your New Password.', ok: false }); return }

    if (rules && !allRulesPassed(rules)) {
      setMsg({ text: 'New password does not meet all requirements.', ok: false })
      return
    }

    if (newPw !== confirmPw) {
      setMsg({ text: 'New Password and Confirm Password do not match.', ok: false })
      return
    }

    setSubmitting(true)
    const result = await changePassword({
      userId: user?.userId ?? '',
      oldPassword: oldPw,
      newPassword: newPw,
      confirmPassword: confirmPw,
      email,
    })
    setSubmitting(false)

    if (result.success) {
      setMsg({ text: result.message, ok: true })
    } else {
      setMsg({ text: result.message, ok: false })
    }
  }

  return (
    <div className="flex items-center justify-start min-h-screen flex-col bg-[#f0f0f0]">

      {/*
        frmSECChangePassword replica:
          ClientSize 728×498, BackColor AliceBlue, shown Maximized from login
          → we render it full-width like a full-screen form
      */}
      <div className="w-full min-h-screen flex flex-col bg-[#F0F8FF]">

        {/* Top toolbar — tspBar, height ~60px, #E7D6C5 */}
        <div
          className="flex items-center gap-0 px-1 border-b border-[#C8B4A0] shrink-0"
          style={{ background: '#E7D6C5', height: '60px' }}
        >
          <ToolbarBtn icon="/icons/door-exit.svg" label="Close" accessKey="c" onClick={() => router.push('/app')} disabled={submitting} />
          <div className="w-px h-9 bg-[#C8B4A0] mx-0.5" />
          <ToolbarBtn icon="/icons/change-password.svg" label="Confirm" accessKey="o" onClick={handleSave} disabled={submitting} />
        </div>

        {/* Body */}
        <div className="flex-1 p-6">
          <div className="max-w-[720px] mx-auto">

            {/* Title row — pictureBox1 + ultraLabel20 "CHANGE PASSWORD" */}
            <div className="flex items-center gap-3 mb-2">
              <Image src="/icons/change-password.svg" alt="" width={50} height={50} />
              <span
                className="font-bold italic font-calibri"
                style={{ fontSize: '15.75pt', color: '#6C4C2C' }}
              >
                CHANGE PASSWORD
              </span>
            </div>

            {/* Brown divider — panel2, height 5, #6C4C2C */}
            <div className="h-[5px] mb-5 rounded-sm" style={{ background: '#6C4C2C' }} />

            {/* Message banner */}
            {msg && (
              <div className={`mb-4 text-[10pt] font-calibri border rounded px-3 py-2 leading-snug ${msg.ok ? 'bg-green-50 border-green-400 text-green-800' : 'bg-red-50 border-red-400 text-red-700'}`}>
                {msg.text}
              </div>
            )}

            {/* Inner panel — #F3EAE2 */}
            <div className="rounded-sm border border-[#C8B4A0] p-5 flex gap-6" style={{ background: '#F3EAE2' }}>

              {/* Left: fields */}
              <div className="flex flex-col gap-4 flex-1">
                {/* User ID — read-only, tabIndex 0 */}
                <LabeledField label="User ID" tabIndex={0}>
                  <input type="text" readOnly value={user?.userId ?? ''} className="boss-input bg-gray-100 cursor-not-allowed" />
                </LabeledField>

                {/* User Name — read-only, tabIndex 1 */}
                <LabeledField label="User Name" tabIndex={1}>
                  <input type="text" readOnly value={user?.userName ?? ''} className="boss-input bg-gray-100 cursor-not-allowed" />
                </LabeledField>

                {/* Old Password / OTP — tabIndex 2 */}
                <LabeledField label="Old Password / OTP" tabIndex={2}>
                  <div className="relative">
                    <input
                      type={oldPwVisible ? 'text' : 'password'}
                      value={oldPw}
                      onChange={e => setOldPw(e.target.value)}
                      tabIndex={2}
                      disabled={submitting}
                      className="boss-input pr-7"
                      placeholder="Enter old password or OTP"
                    />
                    <PasswordEye mode="toggle" visible={oldPwVisible} onShow={() => setOldPwVisible(true)} onHide={() => setOldPwVisible(false)} />
                  </div>
                </LabeledField>

                {/* New Password — tabIndex 3 */}
                <LabeledField label="New Password" tabIndex={3}>
                  <div className="relative">
                    <input
                      type={newPwVisible ? 'text' : 'password'}
                      value={newPw}
                      onChange={e => setNewPw(e.target.value)}
                      onBlur={() => setNewPwVisible(false)}
                      tabIndex={3}
                      disabled={submitting}
                      className="boss-input pr-7"
                      placeholder="Enter new password"
                    />
                    <PasswordEye mode="toggle" visible={newPwVisible} onShow={() => setNewPwVisible(true)} onHide={() => setNewPwVisible(false)} />
                  </div>
                </LabeledField>

                {/* Confirm Password — tabIndex 4 */}
                <LabeledField label="Confirm Password" tabIndex={4}>
                  <div className="relative">
                    <input
                      type={confPwVisible ? 'text' : 'password'}
                      value={confirmPw}
                      onChange={e => setConfirmPw(e.target.value)}
                      onBlur={() => setConfPwVisible(false)}
                      tabIndex={4}
                      disabled={submitting}
                      className="boss-input pr-7"
                      placeholder="Confirm new password"
                    />
                    <PasswordEye mode="toggle" visible={confPwVisible} onShow={() => setConfPwVisible(true)} onHide={() => setConfPwVisible(false)} />
                  </div>
                </LabeledField>

                {/* Email — no tabIndex specified in original, appears after confirm */}
                <LabeledField
                  label="Email"
                  tooltip="This email will receive OTP if you forget password and request OTP."
                >
                  <input
                    type="email"
                    value={email}
                    onChange={e => setEmail(e.target.value)}
                    disabled={submitting}
                    className="boss-input"
                    placeholder="user@example.com"
                  />
                </LabeledField>
              </div>

              {/* Right: password rules panel — ultraGroupBox1 */}
              <div className="w-[260px] shrink-0">
                <PasswordRulesPanel rules={rules} hasInput={hasInput} />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

function LabeledField({
  label, children, tooltip, tabIndex,
}: {
  label: string; children: React.ReactNode; tooltip?: string; tabIndex?: number
}) {
  return (
    <div className="flex flex-col gap-[3px]">
      <label className="text-[10pt] font-calibri italic text-[#404040] flex items-center gap-1">
        {label}
        {tooltip && (
          <span
            className="text-[8pt] text-[#6C4C2C] cursor-help"
            title={tooltip}
          >ⓘ</span>
        )}
      </label>
      {children}
    </div>
  )
}

function ToolbarBtn({
  icon, label, accessKey, onClick, disabled,
}: {
  icon: string; label: string; accessKey: string; onClick: () => void; disabled?: boolean
}) {
  return (
    <button
      type="button"
      onClick={onClick}
      disabled={disabled}
      className="flex flex-col items-center justify-center gap-[3px] w-[70px] h-[50px] text-[10pt] italic font-calibri text-[#404040] bg-transparent border-none cursor-pointer hover:bg-black/[0.06] active:bg-black/[0.12] transition-colors rounded-sm disabled:opacity-50 disabled:cursor-not-allowed"
    >
      <Image src={icon} alt={label} width={24} height={24} />
      <span>{label.split('').map((ch, i) => ch.toLowerCase() === accessKey ? <u key={i}>{ch}</u> : ch)}</span>
    </button>
  )
}
