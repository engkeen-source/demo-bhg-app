'use client'

import { useRef, useState, useEffect, KeyboardEvent } from 'react'
import { useRouter } from 'next/navigation'
import Image from 'next/image'
import LoginToolbar from '@/components/login/LoginToolbar'
import TextFieldWithIcon from '@/components/login/TextFieldWithIcon'
import CompanySelect from '@/components/login/CompanySelect'
import PasswordEye from '@/components/login/PasswordEye'
import { Company, login, getCompanies, getRememberedCredentials } from '@/lib/mockApi'

const APP_VERSION = '1.3.40'

interface MsgState {
  text:     string
  severity: 'info' | 'warning' | 'serious'
}

const severityStyle: Record<string, string> = {
  info:    'bg-blue-50   border-blue-300   text-blue-800',
  warning: 'bg-yellow-50 border-yellow-400 text-yellow-900',
  serious: 'bg-red-50    border-red-400    text-red-800',
}

export default function LoginPage() {
  const router = useRouter()

  const [companies,  setCompanies]  = useState<Company[]>([])
  const [loadingCo,  setLoadingCo]  = useState(true)
  const [userId,     setUserId]     = useState('')
  const [password,   setPassword]   = useState('')
  const [databaseId, setDatabaseId] = useState('')
  const [rememberMe, setRememberMe] = useState(false)
  const [pwVisible,  setPwVisible]  = useState(false)
  const [submitting, setSubmitting] = useState(false)
  const [msg,        setMsg]        = useState<MsgState | null>(null)

  const userIdRef   = useRef<HTMLInputElement>(null)
  const passwordRef = useRef<HTMLInputElement>(null)

  // Load companies and restore Remember Me on mount
  useEffect(() => {
    getCompanies().then(list => {
      setCompanies(list)
      setLoadingCo(false)

      const remembered = getRememberedCredentials()
      if (remembered) {
        setRememberMe(true)
        setUserId(remembered.userId)
        setDatabaseId(remembered.databaseId)
        // Password field gets focus when userId is pre-filled (matches frmLogin_Shown)
        setTimeout(() => passwordRef.current?.focus(), 0)
      } else if (list.length === 1) {
        setDatabaseId(list[0].databaseId)
      }
    })
  }, [])

  async function handleLogin() {
    setMsg(null)

    if (!userId.trim()) {
      setMsg({ text: 'Please enter your User ID.', severity: 'info' })
      userIdRef.current?.focus()
      return
    }
    if (!password) {
      setMsg({ text: 'Please enter your Password.', severity: 'info' })
      passwordRef.current?.focus()
      return
    }
    if (!databaseId) {
      setMsg({ text: 'Please select a Company.', severity: 'info' })
      return
    }

    setSubmitting(true)
    const result = await login({ userId, password, databaseId, rememberMe })
    setSubmitting(false)

    if (result.success) {
      router.push('/app')
    } else {
      setMsg({ text: result.message, severity: result.severity })
    }
  }

  function handlePasswordKeyDown(e: KeyboardEvent<HTMLInputElement>) {
    // Enter in Password field submits if both fields have content (matches frmLogin_KeyDown)
    if (e.key === 'Enter' && userId.trim() && password) {
      handleLogin()
    }
  }

  return (
    <div className="flex items-center justify-center min-h-screen bg-[#f0f0f0] p-4">

      {/*
        frmLogin replica:
          ClientSize 490×370, BackColor White, FormBorderStyle FixedDialog,
          StartPosition CenterScreen, ControlBox false
      */}
      <div
        className="bg-white shadow-[0_6px_24px_rgba(0,0,0,0.22)] border border-[#A0A0A0] flex flex-col select-none"
        style={{ width: '490px' }}
      >
        {/* tspBar — top toolbar, height 74, background #E7D6C5 */}
        <LoginToolbar
          version={APP_VERSION}
          onLogin={handleLogin}
          onExit={() => window.history.back()}
          isLoading={submitting}
        />

        {/* Body — below toolbar */}
        <div className="flex bg-white">

          {/* Left column (white) — key icon + 3 italic instruction labels */}
          <div
            className="flex flex-col justify-start items-start pt-6 px-[17px] bg-white shrink-0"
            style={{ width: '201px' }}
          >
            {/* pictureBox1 — Lkeys, 61×58 at position (21, 89) relative to form */}
            <div className="w-full flex justify-center mb-5">
              <Image
                src="/icons/key-large.svg"
                alt="Login"
                width={61}
                height={58}
                priority
              />
            </div>

            {/* label5 — "Welcome to System!" Calibri 10 italic #404040 */}
            <p className="text-[10pt] italic font-calibri text-[#404040] leading-snug mb-2">
              Welcome to System!
            </p>

            {/* label4 — Calibri 10 italic #404040, size 166×62 */}
            <p className="text-[10pt] italic font-calibri text-[#404040] leading-snug mb-3">
              Use a valid User ID and Password to gain access to the system.
            </p>

            {/* label1 — Calibri 10 italic #404040, size 166×62 */}
            <p className="text-[10pt] italic font-calibri text-[#404040] leading-snug">
              Choose associated company in order to connect the system database.
            </p>
          </div>

          {/* Right panel — panel1, background #F3EAE2, at x=201, 273×264 */}
          <div
            className="flex flex-col border-l border-[#C8B4A0]"
            style={{ width: '289px', background: '#F3EAE2' }}
          >
            {/* ultraLabel1 — "Log In" header with gradient, docked Top, 30px */}
            <div
              className="px-3 py-[6px] text-[11pt] font-bold italic text-[#404040] border-b border-[#C8B4A0] shrink-0"
              style={{ background: 'linear-gradient(180deg,#E7D6C5 0%,#F3EAE2 100%)' }}
            >
              Log In
            </div>

            {/* Form fields area */}
            <div className="px-[15px] pt-4 pb-5 flex flex-col gap-3">

              {/* Message banner */}
              {msg && (
                <div className={`text-[9pt] font-calibri border rounded px-2 py-1.5 leading-snug whitespace-pre-line ${severityStyle[msg.severity]}`}>
                  {msg.text}
                </div>
              )}

              {/* UserID — tabIndex 0 */}
              <TextFieldWithIcon
                ref={userIdRef}
                icon="user.svg"
                iconAlt="user"
                placeholder="Enter user ID"
                value={userId}
                onChange={e => setUserId(e.target.value)}
                tabIndex={0}
                autoComplete="username"
                disabled={submitting}
              />

              {/* Password — tabIndex 1, with press-and-hold eye */}
              <div className="relative">
                <TextFieldWithIcon
                  ref={passwordRef}
                  icon="key-small.svg"
                  iconAlt="password"
                  type={pwVisible ? 'text' : 'password'}
                  placeholder="Enter password"
                  value={password}
                  onChange={e => setPassword(e.target.value)}
                  onKeyDown={handlePasswordKeyDown}
                  tabIndex={1}
                  autoComplete="current-password"
                  disabled={submitting}
                  style={{ paddingRight: '28px' }}
                />
                <PasswordEye
                  mode="hold"
                  visible={pwVisible}
                  onShow={() => setPwVisible(true)}
                  onHide={() => setPwVisible(false)}
                />
              </div>

              {/* Company combobox — tabIndex 2 */}
              <CompanySelect
                companies={companies}
                loading={loadingCo}
                value={databaseId}
                onChange={e => setDatabaseId(e.target.value)}
                tabIndex={2}
                disabled={submitting}
              />

              {/* Remember Me row — ultraLabel2 + uchkRememberMe, tabIndex 3+4 */}
              <label className="flex items-center justify-between text-[9pt] font-bold italic font-calibri text-[#404040] cursor-pointer">
                <span tabIndex={3}>Remember Me</span>
                <input
                  type="checkbox"
                  tabIndex={4}
                  checked={rememberMe}
                  onChange={e => setRememberMe(e.target.checked)}
                  className="w-4 h-4 cursor-pointer"
                />
              </label>

              {/* Forgot Password link — tabIndex 7 (matches designer) */}
              <button
                type="button"
                tabIndex={7}
                onClick={() => router.push('/forgot-password')}
                className="text-left text-[9pt] italic font-calibri text-blue-600 hover:underline cursor-pointer bg-transparent border-none p-0 w-fit"
              >
                Forgot Password?
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
