// Password validation rules matching frmSECChangePassword / clsSECChangePasswordFactory

export interface PasswordRuleResult {
  length:     boolean  // 8–30 characters
  hasUpper:   boolean  // at least one uppercase A-Z
  hasLower:   boolean  // at least one lowercase a-z
  hasDigit:   boolean  // at least one digit 0-9
  hasSpecial: boolean  // at least one special character
  notRecent:  boolean  // not one of the last 3 passwords (mocked: always true in Phase 1)
}

const SPECIAL_RE = /[!@#$%^&*()\-_+|~=`{}[\]:";'<>?,./\\]/

export function checkPasswordRules(
  newPassword: string,
  recentPasswords: string[] = []
): PasswordRuleResult {
  return {
    length:     newPassword.length >= 8 && newPassword.length <= 30,
    hasUpper:   /[A-Z]/.test(newPassword),
    hasLower:   /[a-z]/.test(newPassword),
    hasDigit:   /[0-9]/.test(newPassword),
    hasSpecial: SPECIAL_RE.test(newPassword),
    notRecent:  !recentPasswords.includes(newPassword),
  }
}

export function allRulesPassed(rules: PasswordRuleResult): boolean {
  return Object.values(rules).every(Boolean)
}

// Returns a string describing what failed, mirroring the desktop's error substrings
// used in ShowPasswordError() to flip ■ → ✅/❎
export function describeFailures(rules: PasswordRuleResult): string[] {
  const failures: string[] = []
  if (!rules.length)     failures.push('length')
  if (!rules.hasUpper || !rules.hasLower) failures.push('case')
  if (!rules.hasDigit)   failures.push('digit')
  if (!rules.hasSpecial) failures.push('special')
  if (!rules.notRecent)  failures.push('last')
  return failures
}
