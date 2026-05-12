# BossSO Backend — How the Desktop Does It

> **Phase 2 reference.** This document describes the exact backend logic of the
> desktop app so the web app can replicate or replace it in Phase 2.

---

## 1. Startup / Connection String Bootstrap

**File:** `WinUI/Program.cs:23–122`

1. Reads `BOSSSystemMasterConnection` from `app.config` `<appSettings>`.
2. If `appSettings["Encrypted"] == "true"`, decodes it with `TAUtil.Decoder.Decode`.
3. Stores the result in `AppInfor.BossSystemMasterConnectionStr` — this is the
   **master DB** connection used to list companies during login.
4. Launches `Application.Run(new frmMain())`. `frmMain` then calls `frmLogin.ShowDialog()`.

**For web Phase 2:** Store the master DB connection string in a server-side
environment variable (`BOSS_MASTER_CONNECTION`). Decode if legacy-encrypted using a
compatible implementation of `TAUtil.Decoder` (see §8 — password encoding problem).

---

## 2. Company List (REF_CmpList)

**Files:**
- `BOLib/Factory Classes/Security/clsLoginFactory.cs:296–320`
- `BOLib/Base Classes/Reference/clsRefCmpList.cs:231–268`

**Stored procedure:** `REFCmpList_Get @Option=0` (all companies) or `@Option=1` (by ID)
**Table:** `REF_CmpList` in the master DB

| Column | Type | Notes |
|---|---|---|
| `DataBaseID` | nvarchar(50) | Primary key |
| `CompanyNm` | nvarchar(255) | Display name shown in dropdown |
| `DataBaseNm` | nvarchar(255) | SQL Server `Initial Catalog` (database name) |
| `ConnectionDSN` | nvarchar(50) | SQL Server `Data Source` (server/host) |
| `AuthenticateMode` | nvarchar | `"Window"` or `"SQLServer"` |
| `ConnectionUserID` | nvarchar | Used only when `AuthenticateMode == "SQLServer"` |
| `ConnectionPassword` | nvarchar | Stored in clear text in master DB |
| `LastUserID` | nvarchar(255) | Last user who updated this row |

**Per-company connection string built by `clsLoginFactory.GetCurrentConnectionStr`
(`clsLoginFactory.cs:1223–1253`):**
```
Window auth:    "Data Source=<DSN>;Initial Catalog=<DB>;Integrated Security=True"
SQL auth:       "Data Source=<DSN>;Initial Catalog=<DB>;user id=<u>;password=<p>"
```

**Web Phase 2 endpoint:** `GET /api/companies` → returns `[{ databaseId, companyNm }]`.
Backend reads `REF_CmpList` from master DB. Never expose the connection string to the client.

---

## 3. Full Login Code Path

**Entry:** `frmLogin.cs:btnOK_Click (L108–318)` calls `objLoginFactory.Login()`
**Auth logic:** `BOLib/Factory Classes/Security/clsLoginFactory.cs:322–619`

### Step-by-step

1. **Validate DatabaseID** is selected → `GetCurrentConnectionStr()` → test SQL
   connection via `SqlConnection.Open`. Failure: shows the raw exception message and
   aborts.

2. **Database version check** (`L394–412`): reads `SysOption.DatabaseVersion` (4-part
   string from `SYS_Option` table) and compares to the hard-coded
   `AppInfor.DatabaseVersion` in the EXE. Mismatch → calls `Application.Exit()`.
   *(Web: replace with a server-side API-version header check.)*

3. **Database registration check** (`L387–393`, `clsGFunc.cs:1057–1114`):
   `TAUtil.Encoder.Encode(SysOption.CompanyName) == SysOption.DatabaseRegCode`.
   Mismatch → throws `"DatabaseRegCodeInvalid"`.
   *(Web: perform this check server-side; don't expose SysOption values to the client.)*

4. **User ID lookup** — SP `SECUser_IDCheck` → does userId exist?
   Then SP `SECUser_Get` (Option 2, by UserID) → fills `ObjSECUser` with all fields.

5. **Already-logged-in detection** (`L429–468`): checks `Sec_User.LoginIdentifierCurrent`
   + `LoginTimeStampCurrent`. If another session is active, prompts user. There is a
   hard-coded bypass for username `"ALVIN"` (auto-clears) and a system unlock password
   `"techace"` (do NOT port either of these).
   *(Web: use session tokens instead. A new login invalidates all other sessions for
   the same user.)*

6. **Account disabled check** (`L474–478`): `AccDisabled == true` → error message.

7. **Account lockout check** (`L488–501`): `AccLockOut == true` → check if
   `AccLockOutTimeStamp + LoginRetryTimeOut` minutes has elapsed; if so, auto-reset
   retries and allow login. Otherwise show lockout message.

8. **Password verification** (`L507`) — see §4.

9. **On failure** (`L515–550`): increment `LoginRetries` via SP `SECUser_CustomUpdate`.
   - At `retries == LoginRetry - 1` (penultimate): show warning with remaining count.
   - At `retries == LoginRetry` (final): set `AccLockOut = true`, `AccLockOutTimeStamp = now`.

10. **On success** (`L508–570`):
    - `AppInfor.securityKey = Guid.NewGuid()`
    - SP `SECUser_CustomUpdate` → saves `SecurityKey`, `LoginIdentifierCurrent`,
      `LoginTimeStampCurrent`, clears `LoginRetries`.
    - Populates `AppInfor`: `currentUserKey`, `currentUserID`, `currentUserName`,
      access levels/groups (Contact, Item, Job), `branchKey`, `deptKey`, `tranGrpKey`.
    - `SECPermUtility.LoadAllUserPermission()` — loads the full permission matrix.
    - `LoadGlobalOptions()` — reads ~50 `SYS_Option` rows.
    - Writes `resUser.resx` (Remember Me persistence).
    - Writes audit log row.

### Stored procedures invoked at login

| SP | Purpose |
|---|---|
| `REFCmpList_Get` | Company list from master DB |
| `SECUser_IDCheck` | Does user ID exist? |
| `SECUser_Get` | Full user record incl. hashed password |
| `SECUser_CustomUpdate` | Save login timestamp, increment retries, set lockout, logoff |
| `SECPasswordHistory_Get` | Password expiry / reset state |
| `SECPasswordHistory_AddUpdate` | Write new password hash after change |
| `SysOption_*` reads | `LoginRetry`, `LoginRetryTimeOut`, `PasswordExpiredDays`, `DatabaseVersion`, `DatabaseRegCode` |

---

## 4. Password Verification (the Critical Problem for Phase 2)

**File:** `BOLib/Global Classes/clsGFunc.cs:1035–1056`

```csharp
public static bool IsPasswordValid(object Password, object PasswordToCheck) {
    return Password.ToString().Trim()
        == TAUtil.Decoder.Decode(PasswordToCheck.ToString());
}
```

Called as:
```csharp
IsPasswordValid(
    objSecUserFactory.ObjSECUser.UserKey.ToString() + _password,
    objSecUserFactory.ObjSECUser.Password)
```

So the stored `Sec_User.Password` column holds `TAUtil.Encoder.Encode(UserKey + plaintext)`.
The encoding is **reversible** (symmetric), **not** a one-way hash.

The salt is only the integer `UserKey` prefixed to the plaintext. `TAUtil.dll` is a
vendor binary; its source is not in the repo.

### Three options for Phase 2

**Option A — Thin .NET verification microservice (recommended short-term)**

Deploy a minimal ASP.NET Core endpoint that loads `TAUtil.dll` and exposes:

```
POST /verify-password
Body: { storedHash: string, userKey: int, plaintext: string }
Response: { valid: boolean }
```

Your Next.js API calls this service. The service never leaves your internal network.
This avoids reverse-engineering TAUtil and is immediately compatible.

**Option B — Reverse-engineer TAUtil.Encoder**

Decompile `packages/TAUtil.dll` (e.g. with ILSpy). Implement the same algorithm in
TypeScript/Node.js. Risky if TAUtil's encoding changes in a future desktop release.

**Option C — One-time rehash migration (recommended long-term)**

1. Run a one-time migration script using the .NET microservice from Option A:
   - For each user row, call the microservice to decode the stored password.
   - Re-hash with bcrypt/argon2id and store in a new column `PasswordHash`.
2. In the web app, verify against `PasswordHash` (bcrypt).
3. After all users have logged in via web at least once, drop `Password` column.

This eliminates the TAUtil dependency for future logins.

---

## 5. Sec_User Table Schema

Inferred from `BOLib/Base Classes/Security/clsSECUser.cs:703–755` (DataReader column names).

| Column | Type | Notes |
|---|---|---|
| `UserKey` | int | Primary key; also used as password salt prefix |
| `UserID` | nvarchar | Login username |
| `UserName` | nvarchar | Display name |
| `Password` | nvarchar | `TAUtil.Encoder.Encode(UserKey + plaintext)` — reversible |
| `UserEmail` | nvarchar | Used for OTP forgot-password flow |
| `AccDisabled` | bit | Account disabled flag |
| `AccLockOut` | bit | Temporary lockout flag |
| `AccLockOutTimeStamp` | datetime null | When lockout started |
| `LoginRetries` | smallint | Wrong-password counter |
| `LoginIdentifierCurrent` | nvarchar | Workstation IP of active session |
| `LoginIdentifierLast` | nvarchar | Workstation IP of previous session |
| `LoginTimeStampCurrent` | datetime null | When current session started |
| `LoginTimeStampLast` | datetime null | When previous session started |
| `SecurityKey` | uniqueidentifier | Guid set on each successful login |
| `Custom1` | nvarchar | `"X"` = password never expires for this user |
| `EMKey` | int null | FK to employee record |
| `ConAccessLevel/Grp` | int | Contact data access level / group |
| `ItmAccessLevel/Grp` | int | Item data access level / group |
| `JobAccessLevel/Grp` | int | Job data access level / group |
| `BranchKey` | int null | User's default branch |
| `DeptKey` | int null | User's default department |
| `TranGrpKey` | int null | User's default transaction group |

---

## 6. Password Expiry Rules

**File:** `clsLoginFactory.cs:GetPasswordExpiry` → SP `SECPasswordHistory_Get @Option=2`

The SP returns a DataSet. The web app should evaluate the response as follows:

| SP response | Meaning | Action |
|---|---|---|
| `Tables.Count == 1`, `t[0].Rows[0]["Custom1"] == "RESET"` | Password was reset (e.g. forgot-password OTP) | Force change-password before login |
| `Tables.Count == 2`, row in t[0] only | Password expiring soon | Warn: `"Your password will expire in N day(s). Do you want to change it now?"` |
| `Tables.Count == 2`, rows in both | Password already expired | Force change-password |
| `SysOption.PasswordExpiredDays == -1` | Expiry disabled globally | Skip check |
| `Sec_User.Custom1 == "X"` | Expiry disabled per-user | Skip check |

---

## 7. Forgot Password Flow

**File:** `clsLoginFactory.cs:1358–1419`, SP `SECUser_Forgot_Password`

1. Validate User ID and Company are provided.
2. Build per-company connection string.
3. Run `SELECT UserKey FROM Sec_User WHERE userID='<userId>'` (currently SQL-injection
   vulnerable in the desktop — **use a parameterised query in the web app**).
4. Generate 6-character random alphanumeric code via `GFunc.GenerateRandomCode(6)`.
5. Call SP `SECUser_Forgot_Password` — the SP presumably: sets the `Password` column to
   `TAUtil.Encoder.Encode(UserKey + OTP)`, sets a `RESET` flag in `SECPasswordHistory`,
   and sends an email to `Sec_User.UserEmail`.
6. Show: `"A one-time Password has been sent to your email. Please check your inbox.
   If you haven't received the email, kindly inform the E-services team."`

**Web Phase 2:** Reproduce steps 3–6 server-side. Send OTP email via SMTP/SendGrid.
Store a **bcrypt hash** of the OTP (not the reversible TAUtil encoding) in a separate
`PasswordOTP` column with a TTL timestamp.

---

## 8. Hard-Coded Backdoors — DO NOT PORT

These exist in the desktop app and must **never** be replicated in the web app:

| Code | Location | Description |
|---|---|---|
| `"techace"` | `clsLoginFactory.cs:439–447` | System unlock password that bypasses existing-session lockout |
| `username "ALVIN"` auto-clear | `clsLoginFactory.cs:431` | Specific username that auto-clears its own session lock |
| `user id=DevAdmin;password=BH609189` | `WinUI/Program.cs:70` | Hard-coded SQL credential for agent/batch mode |
| Raw SQL interpolation in forgot-pw | `clsLoginFactory.cs:1377` | SQL injection vector |

---

## 9. System Options Consumed at Login

Loaded from `SYS_Option` via `LoadGlobalOptions` (`clsLoginFactory.cs:951–1175`):

`LoginRetry`, `LoginRetryTimeOut`, `PasswordExpiredDays`, `DatabaseVersion`,
`DatabaseRegCode`, `CompanyName`, `UseWorkOrder`, `UseBranch`, `UseDept`,
`UseTranGrp`, `UseWMS`, `UseProject`, `FiscalYearStart`, `CurrencyCode`,
`SearchByWildcard`, `OpEmail` … (~50 options total).

**Web Phase 2:** Load these from the DB on the server side and include the relevant
subset in the login response (e.g., `loginRetry`, `passwordExpiredDays`). Do not
expose all options to the browser.
