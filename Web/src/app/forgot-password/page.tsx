'use client'

import { useState, useEffect } from 'react'
import { useRouter } from 'next/navigation'
import Image from 'next/image'
import CompanySelect from '@/components/login/CompanySelect'
import { Company, getCompanies, forgotPassword } from '@/lib/mockApi'

export default function ForgotPasswordPage() {
  const router = useRouter()

  const [companies,   setCompanies]  = useState<Company[]>([])
  const [loadingCo,   setLoadingCo]  = useState(true)
  const [userId,      setUserId]     = useState('')
  const [databaseId,  setDatabaseId] = useState('')
  const [submitting,  setSubmitting] = useState(false)
  const [result,      setResult]     = useState<{ success: boolean; message: string } | null>(null)
  const [validationMsg, setValidationMsg] = useState('')

  useEffect(() => {
    getCompanies().then(list => { setCompanies(list); setLoadingCo(false) })
  }, [])

  async function handleSubmit() {
    setValidationMsg('')
    setResult(null)

    // Matches frmLogin lbl_forgot_pw_Click validation: both fields required
    if (!userId.trim() || !databaseId) {
      setValidationMsg('User ID and Company are required to reset your password.')
      return
    }

    setSubmitting(true)
    const res = await forgotPassword({ userId: userId.trim(), databaseId })
    setSubmitting(false)
    setResult(res)
  }

  return (
    <div className="flex items-center justify-center min-h-screen bg-[#f0f0f0] p-4">

      {/* Matches the desktop's modal dialog for Forgot Password */}
      <div
        className="bg-white shadow-[0_6px_24px_rgba(0,0,0,0.22)] border border-[#A0A0A0] flex flex-col"
        style={{ width: '420px' }}
      >
        {/* Toolbar */}
        <div
          className="flex items-center gap-1 px-2 border-b border-[#C8B4A0]"
          style={{ background: '#E7D6C5', height: '74px' }}
        >
          <button
            type="button"
            onClick={() => router.back()}
            className="flex flex-col items-center justify-center gap-[3px] w-[70px] h-[55px] text-[10.5pt] italic font-calibri text-[#404040] bg-transparent border-none cursor-pointer hover:bg-black/[0.06] rounded-sm"
          >
            <Image src="/icons/door-exit.svg" alt="Back" width={28} height={28} />
            <span><u>B</u>ack</span>
          </button>

          <div className="flex-1" />
          <span className="pr-2 text-[10pt] italic font-calibri text-[#404040]">Forgot Password</span>
        </div>

        {/* Body */}
        <div className="px-6 py-5 flex flex-col gap-4" style={{ background: '#F3EAE2' }}>
          {/* Header */}
          <div
            className="px-3 py-[6px] text-[11pt] font-bold italic text-[#404040] border border-[#C8B4A0] rounded-sm"
            style={{ background: 'linear-gradient(180deg,#E7D6C5 0%,#F3EAE2 100%)' }}
          >
            Reset Password
          </div>

          {/* Validation error */}
          {validationMsg && (
            <div className="text-[9pt] font-calibri border border-yellow-400 bg-yellow-50 text-yellow-900 rounded px-2 py-1.5">
              {validationMsg}
            </div>
          )}

          {/* Success message — exact desktop string */}
          {result?.success && (
            <div className="text-[9pt] font-calibri border border-green-400 bg-green-50 text-green-800 rounded px-2 py-2 whitespace-pre-line leading-relaxed">
              {result.message}
            </div>
          )}

          {!result?.success && (
            <>
              {/* User ID field */}
              <div className="flex flex-col gap-1">
                <label className="text-[9pt] font-calibri italic text-[#404040]">User ID</label>
                <input
                  type="text"
                  value={userId}
                  onChange={e => setUserId(e.target.value)}
                  placeholder="Enter your User ID"
                  disabled={submitting}
                  className="h-[26px] text-[11pt] font-calibri border border-gray-400 bg-white px-2 focus:outline-none focus:ring-1 focus:ring-blue-400 placeholder-gray-400"
                />
              </div>

              {/* Company dropdown */}
              <div className="flex flex-col gap-1">
                <label className="text-[9pt] font-calibri italic text-[#404040]">Company</label>
                <CompanySelect
                  companies={companies}
                  loading={loadingCo}
                  value={databaseId}
                  onChange={e => setDatabaseId(e.target.value)}
                  disabled={submitting}
                />
              </div>

              {/* Submit button */}
              <button
                type="button"
                onClick={handleSubmit}
                disabled={submitting}
                className="mt-1 h-[30px] px-6 bg-[#6C4C2C] text-white text-[10pt] italic font-calibri hover:bg-[#5a3d22] active:bg-[#4a3018] transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
              >
                {submitting ? 'Sending…' : 'Send OTP'}
              </button>
            </>
          )}

          {/* Back to login link */}
          {result?.success && (
            <button
              type="button"
              onClick={() => router.push('/login')}
              className="text-[9pt] italic font-calibri text-blue-600 hover:underline text-left bg-transparent border-none p-0 cursor-pointer"
            >
              ← Back to Login
            </button>
          )}
        </div>
      </div>
    </div>
  )
}
