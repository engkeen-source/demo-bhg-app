'use client'

import { PasswordRuleResult } from '@/lib/validators'

interface Props {
  rules:      PasswordRuleResult | null
  hasInput:   boolean
}

type RuleKey = keyof PasswordRuleResult

const RULES: Array<{ key: RuleKey; label: string }> = [
  { key: 'notRecent',  label: 'Must not be one of the last three passwords, excluding OTP.' },
  { key: 'length',     label: 'Length must be between 8 and 30 characters.' },
  { key: 'hasUpper',   label: 'Must contain both upper and lower case letters (a-zA-Z).' },
  { key: 'hasDigit',   label: 'Must include at least one digit (0-9).' },
  { key: 'hasSpecial', label: 'Must include at least one special character (!@#$%^&*()_+|~-=`{}[]:";\'<>?,./\\).' },
]

export default function PasswordRulesPanel({ rules, hasInput }: Props) {
  return (
    <div
      className="border border-[#C8B4A0] rounded-sm p-3 flex flex-col gap-1"
      style={{ background: '#FAFAF8' }}
    >
      <p className="text-[9pt] font-bold font-calibri text-[#6C4C2C] mb-1 border-b border-[#C8B4A0] pb-1">
        New Password Requirements :
      </p>

      {RULES.map(({ key, label }) => {
        let icon = '■'
        let colorClass = 'text-[#404040]'

        if (hasInput && rules) {
          if (rules[key]) {
            icon = '✅'
            colorClass = 'text-green-700'
          } else {
            icon = '❎'
            colorClass = 'text-red-600'
          }
        }

        return (
          <div key={key} className={`flex items-start gap-1.5 text-[8.5pt] font-calibri leading-snug ${colorClass}`}>
            <span className="shrink-0 mt-[1px] text-[10px]">{icon}</span>
            <span>{label}</span>
          </div>
        )
      })}
    </div>
  )
}
