# BossSO Web — Phase 1 Skin

A faithful web replica of the BossSO desktop ERP login experience.

**Stack:** Next.js 14 (App Router) + TypeScript + Tailwind CSS  
**Phase:** 1 — UI skin only. All backend calls are mocked locally. No database.

---

## Quick Start

```bash
cd Web
npm install
npm run dev
```

Then open [http://localhost:3000](http://localhost:3000).

---

## Demo credentials

| Field | Value |
|---|---|
| User ID | Any value (`admin`, `manager`, `user1`, or anything) |
| Password | `demo` |
| Company | Any of the three in the dropdown |

Wrong password 3 times triggers the lockout message. Refresh to reset (or clear `localStorage`).

---

## Pages

| URL | Desktop form | Notes |
|---|---|---|
| `/login` | `frmLogin` | Full replica: toolbar, key graphic, login panel |
| `/forgot-password` | (inline in frmLogin) | Accepts User ID + Company, shows OTP-sent message |
| `/change-password` | `frmSECChangePassword` | Live password-rules checklist; requires active session |
| `/main` | `frmMain` | MDI shell stub: menu bar + status bar; File → Change Password and File → Exit are active |

---

## What is mocked

- **Company list** — 3 static companies with 250 ms fake latency.
- **Login** — succeeds with `password=demo`; simulates lockout after 3 wrong attempts using `localStorage`.
- **Remember Me** — persisted to `localStorage` (`boss_remember_me`, `boss_last_user_id`, `boss_last_database_id`), exactly like the desktop's `resUser.resx`.
- **Forgot Password** — always returns the desktop's OTP-sent success message after 600 ms.
- **Change Password** — validates the 5 password rules client-side; succeeds if old password is `demo`.
- **Session** — user info stored in `sessionStorage` (`boss_current_user`). Navigating to `/change-password` or `/main` without a session redirects to `/login`.

---

## Documentation

| Doc | Contents |
|---|---|
| `docs/OVERVIEW.md` | Desktop app architecture, tech stack, module map, database schema |
| `docs/LOGIN_UX.md` | Pixel-level spec of `frmLogin` and `frmSECChangePassword` (sizes, colors, fonts, behaviors) |
| `docs/BACKEND.md` | **How the desktop authenticates** — full login code path, stored procedures, `Sec_User` schema, password encoding, lockout rules, forgot-password flow, hard-coded backdoors |
| `docs/PHASE2_API.md` | Proposed REST API contract (`/api/companies`, `/api/auth/login`, etc.), environment variables, session strategy, migration checklist |
| `docs/ASSETS.md` | Icon inventory, font stack, color tokens |

---

## Phase 2 — wiring to the real backend

See `docs/PHASE2_API.md` for the full migration guide. High-level steps:

1. Deploy a thin .NET microservice that wraps `TAUtil.dll` for password verification (or run a rehash migration to bcrypt).
2. Add Next.js API routes (`/api/companies`, `/api/auth/login`, etc.) that connect to the SQL Server.
3. Replace the mock functions in `src/lib/mockApi.ts` with real `fetch` calls.
4. Replace `sessionStorage` user storage with an HTTP-only session cookie.
5. Configure environment variables (`BOSS_MASTER_CONNECTION`, `SESSION_SECRET`, etc.).
