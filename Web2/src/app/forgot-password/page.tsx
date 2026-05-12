'use client'

import { useState, useEffect } from 'react'
import { useRouter } from 'next/navigation'
import { Company, getCompanies, forgotPassword } from '@/lib/mockApi'

export default function ForgotPasswordPage() {
  const router = useRouter()

  const [companies,     setCompanies]    = useState<Company[]>([])
  const [loadingCo,     setLoadingCo]    = useState(true)
  const [userId,        setUserId]       = useState('')
  const [databaseId,    setDatabaseId]   = useState('')
  const [submitting,    setSubmitting]   = useState(false)
  const [result,        setResult]       = useState<{ success: boolean; message: string } | null>(null)
  const [validationMsg, setValidationMsg] = useState('')

  useEffect(() => {
    getCompanies().then(list => { setCompanies(list); setLoadingCo(false) })
  }, [])

  async function handleSubmit() {
    setValidationMsg('')
    setResult(null)
    if (!userId.trim() || !databaseId) {
      setValidationMsg('User ID and company are required.')
      return
    }
    setSubmitting(true)
    const res = await forgotPassword({ userId: userId.trim(), databaseId })
    setSubmitting(false)
    setResult(res)
  }

  return (
    <div className="min-h-screen flex items-center justify-center p-8 bg-bg-base">
      <div className="w-full max-w-md">
        {/* Back */}
        <button
          type="button"
          onClick={() => router.back()}
          className="flex items-center gap-1.5 text-sm text-txt-secondary hover:text-txt-primary mb-8 transition-colors"
        >
          <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
            <path d="M10 4L6 8l4 4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/>
          </svg>
          Back to login
        </button>

        <div className="bg-bg-surface border border-border rounded-2xl p-8 shadow-card">
          <div className="mb-6">
            <h1 className="text-xl font-semibold text-txt-primary">Reset your password</h1>
            <p className="text-sm text-txt-tertiary mt-1">Enter your User ID and we'll send you a one-time password.</p>
          </div>

          {validationMsg && (
            <div className="mb-4 text-sm border border-amber-200 bg-amber-50 text-amber-800 rounded-lg px-4 py-3">
              {validationMsg}
            </div>
          )}

          {result?.success ? (
            <div className="space-y-5">
              <div className="p-4 bg-emerald-50 border border-emerald-200 rounded-xl">
                <p className="text-sm text-emerald-800 whitespace-pre-line leading-relaxed">{result.message}</p>
              </div>
              <button
                type="button"
                onClick={() => router.push('/login')}
                className="w-full h-10 bg-brand-600 text-white text-sm font-semibold rounded-lg hover:bg-brand-700 transition-colors shadow-sm"
              >
                Back to login
              </button>
            </div>
          ) : (
            <div className="space-y-4">
              <div className="flex flex-col gap-1.5">
                <label className="text-xs font-semibold text-txt-secondary uppercase tracking-wide">User ID</label>
                <input
                  type="text"
                  value={userId}
                  onChange={e => setUserId(e.target.value)}
                  placeholder="Enter your user ID"
                  disabled={submitting}
                  className="w2-input"
                />
              </div>

              <div className="flex flex-col gap-1.5">
                <label className="text-xs font-semibold text-txt-secondary uppercase tracking-wide">Company</label>
                <select
                  value={databaseId}
                  onChange={e => setDatabaseId(e.target.value)}
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

              <button
                type="button"
                onClick={handleSubmit}
                disabled={submitting}
                className="w-full h-10 bg-brand-600 text-white text-sm font-semibold rounded-lg hover:bg-brand-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors shadow-sm mt-2"
              >
                {submitting ? 'Sending OTP…' : 'Send OTP'}
              </button>
            </div>
          )}
        </div>
      </div>
    </div>
  )
}
