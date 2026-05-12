# Phase 2 API Contract

> Proposed REST API for the Next.js backend-for-frontend (Next.js App Router API routes).
> All endpoints live under `/api/`. Authentication uses **HTTP-only cookies** (no JWTs in localStorage).

---

## Stack additions for Phase 2

- `mssql` npm package — SQL Server client for Node.js
- `bcryptjs` — password hashing (for rehashed passwords; see Option C in `BACKEND.md`)
- `node-fetch` or built-in `fetch` — to call the .NET verification microservice (Option A)
- `nodemailer` or `@sendgrid/mail` — OTP emails
- `iron-session` or `next-auth` — HTTP-only session cookie management

---

## Endpoints

### `GET /api/companies`

Returns the company list from `REF_CmpList` in the master DB.

**Response `200`:**
```json
[
  { "databaseId": "DB001", "companyNm": "BossSO Trading Sdn Bhd" },
  { "databaseId": "DB002", "companyNm": "BossSO Retail Sdn Bhd" }
]
```

**Implementation:**
```typescript
// src/app/api/companies/route.ts
import { getMasterDb } from '@/lib/db'

export async function GET() {
  const db = await getMasterDb()
  const rows = await db.query('EXEC REFCmpList_Get @Option=0')
  return Response.json(rows.map(r => ({ databaseId: r.DataBaseID, companyNm: r.CompanyNm })))
}
```

---

### `POST /api/auth/login`

**Request body:**
```json
{
  "userId": "admin",
  "password": "P@ssw0rd!",
  "databaseId": "DB001",
  "rememberMe": true
}
```

**Response `200` (success):**
```json
{
  "success": true,
  "user": {
    "userId": "ADMIN",
    "userName": "Administrator",
    "companyNm": "BossSO Trading Sdn Bhd",
    "databaseId": "DB001",
    "loginTime": "2026-05-11T09:30:00.000Z",
    "sessionId": "uuid-v4"
  }
}
```

**Response `401` (failure):**
```json
{
  "success": false,
  "message": "Invalid Password",
  "severity": "info" | "warning" | "serious",
  "locked": false
}
```

**Server-side implementation steps** (mirrors `clsLoginFactory.Login`):

1. Validate inputs.
2. Build per-company connection string from `REF_CmpList` (never send to client).
3. Run `EXEC SECUser_IDCheck @UserID=@uid`.
4. Run `EXEC SECUser_Get @Option=2, @UserID=@uid` to get full user row.
5. Check `AccDisabled`, `AccLockOut` / `AccLockOutTimeStamp`.
6. Verify password (see §Password Verification below).
7. Handle retries and lockout.
8. On success: `EXEC SECUser_CustomUpdate` to save `SecurityKey`, `LoginIdentifier`, `LoginTimeStamp`; clear retries.
9. Set HTTP-only session cookie with `{ userId, userKey, databaseId, sessionId, companyNm }`.
10. Return user info.

**Password Verification (server-side options):**

*Option A (short-term — TAUtil microservice):*
```typescript
const res = await fetch('http://internal-tautil-service/verify-password', {
  method: 'POST',
  body: JSON.stringify({ storedHash: user.Password, userKey: user.UserKey, plaintext: params.password }),
})
const { valid } = await res.json()
```

*Option C (long-term — bcrypt after migration):*
```typescript
import bcrypt from 'bcryptjs'
const valid = await bcrypt.compare(params.password, user.PasswordHash)
```

---

### `POST /api/auth/logout`

Clears the session cookie.

**Response `200`:** `{ "success": true }`

---

### `POST /api/auth/forgot-password`

**Request body:**
```json
{ "userId": "admin", "databaseId": "DB001" }
```

**Response `200`:**
```json
{
  "success": true,
  "message": "A one-time Password has been sent to your email.\nPlease check your inbox.\nIf you haven't received the email, kindly inform the E-services team."
}
```

**Server-side:**
1. Lookup user by `userId` in the company DB (parameterised query — not the raw string interpolation in the desktop).
2. Generate 6-char OTP with `crypto.randomBytes`.
3. Store `bcrypt.hash(OTP)` + expiry timestamp in a `PasswordOTP` column (or `SECPasswordHistory`).
4. Send OTP email via SMTP/SendGrid.
5. Call `EXEC SECUser_Forgot_Password` if you want to keep legacy compatibility.

---

### `POST /api/auth/change-password`

**Request body:**
```json
{
  "oldPassword": "demo",
  "newPassword": "NewP@ss1!",
  "confirmPassword": "NewP@ss1!",
  "email": "user@example.com"
}
```

**Response `200`:**
```json
{ "success": true, "message": "Password changed successfully." }
```

**Server-side:**
1. Read session cookie to get `userId` and `databaseId`.
2. Verify `oldPassword` against current stored password.
3. Validate new password against the 5 rules.
4. Check against last 3 passwords via `SECPasswordHistory`.
5. Store new hash via `EXEC SECPasswordHistory_AddUpdate`.
6. Update `Sec_User.Password` / `Sec_User.PasswordHash`.
7. If the session was flagged as a forced change (password expired/OTP reset), return `successRelogin` message and clear the session.

---

## Environment Variables (Phase 2)

```env
BOSS_MASTER_CONNECTION=Server=...;Database=BossSOmaster;...
BOSS_MASTER_ENCRYPTED=false        # set true if using TAUtil-encoded connection string
TAUTIL_SERVICE_URL=http://localhost:5050  # if using Option A
SESSION_SECRET=<32-char random>
SMTP_HOST=smtp.example.com
SMTP_PORT=587
SMTP_USER=noreply@example.com
SMTP_PASS=...
```

---

## Session Strategy

Use **HTTP-only cookies** (not localStorage JWTs) to prevent XSS token theft:

```typescript
// iron-session config
const sessionOptions = {
  password: process.env.SESSION_SECRET,
  cookieName: 'boss_session',
  cookieOptions: {
    secure: process.env.NODE_ENV === 'production',
    httpOnly: true,
    sameSite: 'lax',
    maxAge: 60 * 60 * 8,  // 8 hours
  },
}
```

---

## Migration Checklist (Phase 1 → Phase 2)

- [ ] Replace `getCompanies()` in `mockApi.ts` with a real `fetch('/api/companies')` call.
- [ ] Replace `login()` in `mockApi.ts` with `fetch('/api/auth/login', { method: 'POST', ... })`.
- [ ] Replace `forgotPassword()` and `changePassword()` similarly.
- [ ] Remove `sessionStorage` user storage; read current user from the session cookie via `GET /api/auth/me`.
- [ ] Deploy `TAUtil.dll` microservice (Option A) or run the rehash migration (Option C).
- [ ] Ensure `REF_CmpList` master DB is reachable from the server environment.
- [ ] Set all environment variables.
- [ ] Test lockout, expiry, OTP flows end-to-end.
