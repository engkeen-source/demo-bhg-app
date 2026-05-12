'use client'

import { PasswordRuleResult } from '@/lib/validators'

interface Props {
  rules: PasswordRuleResult | null
  hasInput: boolean
}

type RuleKey = keyof PasswordRuleResult

const RULES: Array<{ key: RuleKey; label: string }> = [
  { key: 'notRecent',  label: 'Not one of the last 3 passwords' },
  { key: 'length',     label: 'Between 8 and 30 characters' },
  { key: 'hasUpper',   label: 'Upper and lower case letters' },
  { key: 'hasDigit',   label: 'At least one digit (0–9)' },
  { key: 'hasSpecial', label: 'At least one special character' },
]

export default function PasswordRulesPanel({ rules, hasInput }: Props) {
  return (
    <div className="rounded-xl border border-border bg-bg-muted p-4 space-y-2.5">
      <p className="text-xs font-semibold text-txt-primary uppercase tracking-wide">
        Password Requirements
      </p>
      <div className="space-y-2">
        {RULES.map(({ key, label }) => {
          const passed = hasInput && rules ? rules[key] : null

          return (
            <div key={key} className="flex items-center gap-2.5">
              <div className={[
                'w-4 h-4 rounded-full flex items-center justify-center shrink-0 transition-colors',
                passed === true  ? 'bg-emerald-100' :
                passed === false ? 'bg-red-100' :
                'bg-border',
              ].join(' ')}>
                {passed === true && (
                  <svg width="8" height="8" viewBox="0 0 12 12" fill="none" className="text-emerald-600">
                    <path d="M2 6l3 3 5-5" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/>
                  </svg>
                )}
                {passed === false && (
                  <svg width="8" height="8" viewBox="0 0 12 12" fill="none" className="text-red-500">
                    <path d="M3 3l6 6M9 3l-6 6" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
                  </svg>
                )}
              </div>
              <span className={`text-xs ${
                passed === true ? 'text-emerald-700' :
                passed === false ? 'text-red-600' :
                'text-txt-secondary'
              }`}>
                {label}
              </span>
            </div>
          )
        })}
      </div>
    </div>
  )
}
