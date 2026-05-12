'use client'

// Phase 1 mock — all calls are local, no network. Replace implementations in Phase 2.

export interface Company {
  databaseId: string
  companyNm:  string
}

export interface UserInfo {
  userId:      string
  userName:    string
  companyNm:   string
  databaseId:  string
  loginTime:   Date
  sessionId:   string
  identifier:  string   // workstation IP equivalent — browser user-agent hash
}

export interface LoginResult {
  success:   true
  user:      UserInfo
}

export interface LoginError {
  success:   false
  message:   string
  severity:  'info' | 'warning' | 'serious'
  locked?:   boolean
}

// ─── Mock data ────────────────────────────────────────────────────────────────

const COMPANIES: Company[] = [
  { databaseId: 'DB001', companyNm: 'BossSO Trading Sdn Bhd' },
  { databaseId: 'DB002', companyNm: 'BossSO Retail Sdn Bhd' },
  { databaseId: 'DB003', companyNm: 'BossSO Holdings Berhad' },
]

const DEMO_USERS: Record<string, { userName: string; password: string }> = {
  admin:   { userName: 'Administrator',  password: 'demo' },
  manager: { userName: 'Sales Manager',  password: 'demo' },
  user1:   { userName: 'Normal User',    password: 'demo' },
}

const LOGIN_RETRY_LIMIT = 3

// localStorage keys
const LS_RETRIES   = (dbId: string) => `boss_retries_${dbId}`
const LS_LOCKED    = (dbId: string) => `boss_locked_${dbId}`
const LS_REMEMBER  = 'boss_remember_me'
const LS_LAST_UID  = 'boss_last_user_id'
const LS_LAST_DBID = 'boss_last_database_id'
const SS_USER      = 'boss_current_user'

// ─── Helpers ─────────────────────────────────────────────────────────────────

function delay(ms: number): Promise<void> {
  return new Promise(resolve => setTimeout(resolve, ms))
}

function browserIdentifier(): string {
  const raw = (navigator.userAgent + screen.width + screen.height).slice(0, 40)
  let hash = 0
  for (let i = 0; i < raw.length; i++) hash = ((hash << 5) - hash) + raw.charCodeAt(i)
  return `WEB-${Math.abs(hash).toString(16).toUpperCase().padStart(8, '0')}`
}

// ─── Public API ──────────────────────────────────────────────────────────────

export async function getCompanies(): Promise<Company[]> {
  await delay(250)
  return COMPANIES
}

export async function login(params: {
  userId:     string
  password:   string
  databaseId: string
  rememberMe: boolean
}): Promise<LoginResult | LoginError> {
  await delay(600)

  const { userId, password, databaseId, rememberMe } = params
  const uid = userId.trim().toLowerCase()
  const company = COMPANIES.find(c => c.databaseId === databaseId)

  if (!company) {
    return { success: false, severity: 'info', message: `Unable to obtain database connection string for company '${databaseId}'.` }
  }

  // Check lockout
  if (localStorage.getItem(LS_LOCKED(databaseId)) === '1') {
    return {
      success: false, severity: 'serious', locked: true,
      message: 'Sorry, your login has been locked temporarily. Please try again later or contact the system administrator.',
    }
  }

  const user = DEMO_USERS[uid]
  const validPassword = password === 'demo' || (user && password === user.password)

  if (!validPassword) {
    const retriesRaw = parseInt(localStorage.getItem(LS_RETRIES(databaseId)) ?? '0', 10)
    const newRetries = retriesRaw + 1
    localStorage.setItem(LS_RETRIES(databaseId), String(newRetries))

    if (newRetries >= LOGIN_RETRY_LIMIT) {
      localStorage.setItem(LS_LOCKED(databaseId), '1')
      return {
        success: false, severity: 'serious', locked: true,
        message: `Sorry, your ${LOGIN_RETRY_LIMIT}${ordinal(LOGIN_RETRY_LIMIT)} attempt was also unsuccessful, your login has been locked temporarily.`,
      }
    }

    if (newRetries === LOGIN_RETRY_LIMIT - 1) {
      return {
        success: false, severity: 'warning',
        message: `You have attempted to log in with an invalid password ${newRetries} time${newRetries > 1 ? 's' : ''} already. ` +
          `If the ${LOGIN_RETRY_LIMIT}${ordinal(LOGIN_RETRY_LIMIT)} attempt is unsuccessful, your next login will be locked.`,
      }
    }

    return { success: false, severity: 'info', message: 'Invalid Password' }
  }

  // Success — clear retries
  localStorage.removeItem(LS_RETRIES(databaseId))
  localStorage.removeItem(LS_LOCKED(databaseId))

  // Handle Remember Me
  if (rememberMe) {
    localStorage.setItem(LS_REMEMBER, '1')
    localStorage.setItem(LS_LAST_UID,  userId.trim())
    localStorage.setItem(LS_LAST_DBID, databaseId)
  } else {
    localStorage.removeItem(LS_REMEMBER)
    localStorage.removeItem(LS_LAST_UID)
    localStorage.removeItem(LS_LAST_DBID)
  }

  const userInfo: UserInfo = {
    userId:     userId.trim(),
    userName:   user?.userName ?? userId.trim(),
    companyNm:  company.companyNm,
    databaseId,
    loginTime:  new Date(),
    sessionId:  crypto.randomUUID(),
    identifier: browserIdentifier(),
  }

  sessionStorage.setItem(SS_USER, JSON.stringify({
    ...userInfo,
    loginTime: userInfo.loginTime.toISOString(),
  }))

  return { success: true, user: userInfo }
}

export function getRememberedCredentials(): { userId: string; databaseId: string } | null {
  if (localStorage.getItem(LS_REMEMBER) !== '1') return null
  const userId = localStorage.getItem(LS_LAST_UID)
  const databaseId = localStorage.getItem(LS_LAST_DBID)
  if (!userId || !databaseId) return null
  return { userId, databaseId }
}

export function getCurrentUser(): UserInfo | null {
  try {
    const raw = sessionStorage.getItem(SS_USER)
    if (!raw) return null
    const parsed = JSON.parse(raw)
    return { ...parsed, loginTime: new Date(parsed.loginTime) }
  } catch {
    return null
  }
}

export function logout(): void {
  sessionStorage.removeItem(SS_USER)
}

export async function forgotPassword(params: {
  userId:     string
  databaseId: string
}): Promise<{ success: boolean; message: string }> {
  await delay(600)
  // Phase 1: always succeeds (any non-empty userId)
  return {
    success: true,
    message: "A one-time Password has been sent to your email.\nPlease check your inbox.\nIf you haven't received the email, kindly inform the E-services team.",
  }
}

export async function changePassword(params: {
  userId:      string
  oldPassword: string
  newPassword: string
  confirmPassword: string
  email:       string
}): Promise<{ success: boolean; message: string }> {
  await delay(700)

  if (params.newPassword !== params.confirmPassword) {
    return { success: false, message: 'New Password and Confirm Password do not match.' }
  }
  if (params.oldPassword !== 'demo') {
    return { success: false, message: 'Old Password / OTP is incorrect.' }
  }
  if (params.newPassword === params.oldPassword) {
    return { success: false, message: 'New password must be different from your current password.' }
  }

  return { success: true, message: 'Password changed successfully.' }
}

function ordinal(n: number): string {
  const s = ['th', 'st', 'nd', 'rd']
  const v = n % 100
  return s[(v - 20) % 10] ?? s[v] ?? s[0]
}
