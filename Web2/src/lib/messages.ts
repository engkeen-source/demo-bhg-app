// Exact message strings extracted from the desktop app (clsSysMessageUtility + frmLogin.cs + clsLoginFactory.cs)

export const MSG = {
  login: {
    invalidUserId:       'Invalid User ID',
    invalidPassword:     'Invalid Password',
    userDisabled:        'Your account has been disabled. Please contact the system administrator.',
    userLockedOut:       'Sorry, your login has been locked temporarily. Please try again later or contact the system administrator.',
    userLockedOutFinal:  (n: number) =>
      `Sorry, your ${n}${ordinal(n)} attempt was also unsuccessful, your login has been locked temporarily.`,
    passwordWarning:     (current: number, limit: number) =>
      `You have attempted to log in with an invalid password ${current} time${current > 1 ? 's' : ''} already. ` +
      `If the ${limit}${ordinal(limit)} attempt is unsuccessful, your next login will be locked.`,
    dbConnectionInvalid: (company: string) =>
      `Unable to obtain database connection string for company '${company}'.`,
    dbRegCodeInvalid:    'Database Registration Code is Invalid.',
    userIdRequired:      'Please enter your User ID.',
    passwordRequired:    'Please enter your Password.',
    companyRequired:     'Please select a Company.',
    userIdAndCompanyRequired: 'User ID and Company are required to reset your password.',
    forgotPwSuccess:
      'A one-time Password has been sent to your email.\nPlease check your inbox.\nIf you haven\'t received the email, kindly inform the E-services team.',
    forgotPwFailed:      'Failed to reset your password. Please try again.',
    lastLoginInfo:       (user: string, dt: string, identifier: string) =>
      `${user} last logged in on ${dt} from ${identifier}`,
    firstLogin:          (user: string) => `${user} — Welcome! This is your first login.`,
  },
  changePassword: {
    oldPasswordRequired:     'Please enter your Old Password / OTP.',
    newPasswordRequired:     'Please enter a New Password.',
    confirmPasswordRequired: 'Please confirm your New Password.',
    passwordMismatch:        'New Password and Confirm Password do not match.',
    sameAsOld:               'New password must be different from your current password.',
    success:                 'Password changed successfully.',
    successRelogin:          'Password changed successfully. Please re-login with new password.',
    emailTooltip:            'This email will receive OTP if you forget password and request OTP.',
  },
  passwordExpired: {
    reset:    'Your password is reset. Please enter your new password.',
    expired:  'Your password is expired. Please change the password.',
    warning:  (days: number) =>
      `Your password will expire in ${days} day${days !== 1 ? 's' : ''}. Do you want to change it now?`,
  },
}

function ordinal(n: number): string {
  const s = ['th', 'st', 'nd', 'rd']
  const v = n % 100
  return s[(v - 20) % 10] ?? s[v] ?? s[0]
}

// Format a Date to the desktop's exact pattern: "11 May 2026 - 3:45 PM"
export function formatLastLoginDate(d: Date): string {
  const day   = d.getDate()
  const month = d.toLocaleString('en-GB', { month: 'long' })
  const year  = d.getFullYear()
  const time  = d.toLocaleString('en-US', { hour: 'numeric', minute: '2-digit', hour12: true })
  return `${day} ${month} ${year} - ${time}`
}
