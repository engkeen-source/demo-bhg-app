'use client'

import { useRef, useState, useEffect, KeyboardEvent } from 'react'
import { useRouter } from 'next/navigation'
import PasswordEye from '@/components/login/PasswordEye'
import { Company, login, getCompanies, getRememberedCredentials } from '@/lib/mockApi'

const APP_VERSION = '1.3.40'

interface MsgState {
  text: string
  severity: 'info' | 'warning' | 'serious'
}

const severityStyle: Record<string, string> = {
  info:    'bg-blue-50 border-blue-200 text-blue-800',
  warning: 'bg-amber-50 border-amber-200 text-amber-900',
  serious: 'bg-red-50 border-red-200 text-red-800',
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

  useEffect(() => {
    getCompanies().then(list => {
      setCompanies(list)
      setLoadingCo(false)
      const remembered = getRememberedCredentials()
      if (remembered) {
        setRememberMe(true)
        setUserId(remembered.userId)
        setDatabaseId(remembered.databaseId)
        setTimeout(() => passwordRef.current?.focus(), 0)
      } else if (list.length === 1) {
        setDatabaseId(list[0].databaseId)
      }
    })
  }, [])

  async function handleLogin() {
    setMsg(null)
    if (!userId.trim()) { setMsg({ text: 'Please enter your User ID.', severity: 'info' }); userIdRef.current?.focus(); return }
    if (!password)      { setMsg({ text: 'Please enter your password.', severity: 'info' }); passwordRef.current?.focus(); return }
    if (!databaseId)    { setMsg({ text: 'Please select a company.', severity: 'info' }); return }

    setSubmitting(true)
    const result = await login({ userId, password, databaseId, rememberMe })
    setSubmitting(false)

    if (result.success) {
      router.push('/app')
    } else {
      setMsg({ text: result.message, severity: result.severity })
    }
  }

  function handleKeyDown(e: KeyboardEvent<HTMLInputElement>) {
    if (e.key === 'Enter' && userId.trim() && password) handleLogin()
  }

  return (
    <div className="min-h-screen flex">
      {/* Left panel — gradient brand panel */}
      <div className="hidden lg:flex lg:w-[55%] bg-gradient-to-br from-brand-600 to-brand-700 flex-col items-center justify-center p-12 relative overflow-hidden">
        {/* Decorative circles */}
        <div className="absolute top-[-80px] left-[-80px] w-64 h-64 rounded-full bg-white/5" />
        <div className="absolute bottom-[-120px] right-[-60px] w-96 h-96 rounded-full bg-white/5" />
        <div className="absolute top-1/3 right-[-40px] w-48 h-48 rounded-full bg-white/5" />

        <div className="relative z-10 text-center max-w-sm">
          <div className="text-white/90 text-4xl font-bold tracking-tight mb-3">BossSO</div>
          <p className="text-white/70 text-base leading-relaxed">
            Modern ERP for modern business.<br />
            Manage sales, inventory, accounts and more.
          </p>
          <div className="mt-10 grid grid-cols-3 gap-4 text-white/60">
            {['Sales', 'Inventory', 'Accounts'].map(item => (
              <div key={item} className="flex flex-col items-center gap-2">
                <div className="w-10 h-10 rounded-xl bg-white/10 flex items-center justify-center">
                  <div className="w-2 h-2 rounded-full bg-white/80" />
                </div>
                <span className="text-xs font-medium">{item}</span>
              </div>
            ))}
          </div>
        </div>

        <div className="absolute bottom-6 text-white/40 text-xs">v{APP_VERSION}</div>
      </div>

      {/* Right panel — login form */}
      <div className="flex-1 flex items-center justify-center p-8 bg-bg-base">
        <div className="w-full max-w-sm">
          {/* Mobile logo */}
          <div className="lg:hidden text-center mb-8">
            <span className="text-2xl font-bold text-brand-600">BossSO</span>
          </div>

          <div className="mb-8">
            <h1 className="text-2xl font-semibold text-txt-primary">Welcome back</h1>
            <p className="text-sm text-txt-tertiary mt-1">Sign in to your account to continue</p>
          </div>

          {/* Message banner */}
          {msg && (
            <div className={`mb-5 text-sm border rounded-lg px-4 py-3 ${severityStyle[msg.severity]}`}>
              {msg.text}
            </div>
          )}

          <div className="space-y-4">
            {/* User ID */}
            <div className="flex flex-col gap-1.5">
              <label className="text-xs font-semibold text-txt-secondary uppercase tracking-wide">User ID</label>
              <input
                ref={userIdRef}
                type="text"
                value={userId}
                onChange={e => setUserId(e.target.value)}
                placeholder="Enter your user ID"
                tabIndex={0}
                autoComplete="username"
                disabled={submitting}
                className="w2-input"
              />
            </div>

            {/* Password */}
            <div className="flex flex-col gap-1.5">
              <label className="text-xs font-semibold text-txt-secondary uppercase tracking-wide">Password</label>
              <div className="relative">
                <input
                  ref={passwordRef}
                  type={pwVisible ? 'text' : 'password'}
                  value={password}
                  onChange={e => setPassword(e.target.value)}
                  onKeyDown={handleKeyDown}
                  placeholder="Enter your password"
                  tabIndex={1}
                  autoComplete="current-password"
                  disabled={submitting}
                  className="w2-input pr-10"
                />
                <PasswordEye mode="hold" visible={pwVisible} onShow={() => setPwVisible(true)} onHide={() => setPwVisible(false)} />
              </div>
            </div>

            {/* Company */}
            <div className="flex flex-col gap-1.5">
              <label className="text-xs font-semibold text-txt-secondary uppercase tracking-wide">Company</label>
              <select
                value={databaseId}
                onChange={e => setDatabaseId(e.target.value)}
                tabIndex={2}
                disabled={submitting || loadingCo}
                className="w2-input"
              >
                {loadingCo ? (
                  <option value="">Loading…</option>
                ) : (
                  <>
                    <option value="">Select a company…</option>
                    {companies.map(c => (
                      <option key={c.databaseId} value={c.databaseId}>{c.companyNm}</option>
                    ))}
                  </>
                )}
              </select>
            </div>

            {/* Remember me row */}
            <div className="flex items-center justify-between">
              <label className="flex items-center gap-2 cursor-pointer">
                <input
                  type="checkbox"
                  checked={rememberMe}
                  onChange={e => setRememberMe(e.target.checked)}
                  tabIndex={3}
                  className="w-4 h-4 accent-brand-600"
                />
                <span className="text-sm text-txt-secondary">Remember me</span>
              </label>
              <button
                type="button"
                onClick={() => router.push('/forgot-password')}
                className="text-sm text-brand-600 hover:text-brand-700 font-medium"
              >
                Forgot password?
              </button>
            </div>

            {/* Submit */}
            <button
              type="button"
              onClick={handleLogin}
              disabled={submitting}
              className="w-full h-10 bg-brand-600 text-white text-sm font-semibold rounded-lg hover:bg-brand-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors shadow-sm hover:shadow mt-2"
            >
              {submitting ? (
                <span className="flex items-center justify-center gap-2">
                  <svg className="animate-spin" width="14" height="14" viewBox="0 0 24 24" fill="none">
                    <circle cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="2" strokeDasharray="32" strokeDashoffset="32" className="opacity-25"/>
                    <path d="M12 2a10 10 0 0110 10" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/>
                  </svg>
                  Signing in…
                </span>
              ) : 'Sign in'}
            </button>
          </div>
        </div>
      </div>
    </div>
  )
}
