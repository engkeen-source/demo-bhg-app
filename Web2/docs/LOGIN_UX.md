# BossSO Login UI — Pixel-Level Specification

Sourced from:
- `WinUI/Forms/Security/frmLogin.designer.cs`
- `WinUI/Forms/Security/frmLogin.cs`
- `WinUI/Forms/Security/frmSECChangePassword.Designer.cs`
- `WinUI/Forms/Security/frmSECChangePassword.cs`

---

## frmLogin

### Form properties

| Property | Value |
|---|---|
| `ClientSize` | 490 × 370 px |
| `BackColor` | White |
| `FormBorderStyle` | `FixedDialog` (no resize handles) |
| `ControlBox` | `false` (no title-bar ✕ button) |
| `ShowInTaskbar` | `false` |
| `StartPosition` | `CenterScreen` |
| `Font` (default) | Tahoma 10pt |
| `KeyPreview` | `true` |
| Window title | `"Login"` |

### Color palette

| Token | Hex | Usage |
|---|---|---|
| Beige-1 | `#E7D6C5` | Toolbar background, panel-header gradient start |
| Beige-2 | `#F3EAE2` | Right panel background, panel-header gradient end |
| Dark | `#404040` | All label/button text |
| Brown | `#6C4C2C` | Change-password title, divider |
| Link | `#0000FF` / blue | "Forgot Password?" label |
| Placeholder | Silver / `#C0C0C0` | TextBox NullText |

### Toolbar — `tspBar`

| Property | Value |
|---|---|
| Height | 74 px |
| Background | `#E7D6C5` |

| Button | Label | Icon resource | Font | Size | Action |
|---|---|---|---|---|---|
| `tsbExit1` | `"E&xit"` | `door_back_32` | Calibri 11 italic | 70×55 | `DialogResult.Cancel` → closes form |
| `tsbLogin1` | `"&Login"` | `user_next_32` | Calibri 11 italic | 70×55 | `btnOK_Click` → authenticate |
| `toolStripLabel1` | `"Version :"` | — | italic | right-aligned | static |
| `lblVersion` | e.g. `"1.3.40"` | — | — | right-aligned | read-only text box |

### Left column (white background, x=0–200)

| Control | Type | Position | Size | Text / Notes |
|---|---|---|---|---|
| `pictureBox1` | PictureBox | `(21, 89)` | 61×58 | Resource: `Lkeys` (key graphic) |
| `label5` | Label | `(17, 165)` | auto | `"Welcome to System!"` — Calibri 10 italic `#404040` |
| `label4` | Label | `(17, 197)` | 166×62 | `"Use a valid User ID and Password to gain access to the system."` — Calibri 10 italic `#404040` |
| `label1` | Label | `(17, 259)` | 166×62 | `"Choose associated company in order to connect the system database."` — Calibri 10 italic `#404040` |

### Right panel — `panel1`

| Property | Value |
|---|---|
| Position | `(201, 89)` |
| Size | 273×264 px |
| BackColor | `#F3EAE2` |

#### Panel header — `ultraLabel1` (docked Top)

| Property | Value |
|---|---|
| Height | ~30 px |
| Text | `"Log In"` |
| Font | Calibri 11pt Bold+Italic |
| ForeColor | `#404040` |
| Background | Gradient vertical `#E7D6C5 → #F3EAE2` |

#### Form fields inside panel1 (top-to-bottom, x=15)

| TabIndex | Control | Type | Position (in panel) | Size | Config |
|---|---|---|---|---|---|
| 0 | `UserID` | `TAUtil.TATextBoxEditor` | `(15, 77)` | 230×26 | Calibri 11; left icon: `User`; NullText (placeholder): `"Enter user ID"` (silver) |
| 1 | `Password` | `TAUtil.TATextBoxEditor` | `(15, 114)` | 230×26 | Calibri 11; PasswordChar `'*'`; left icon: `PrimaryKeyHS`; NullText: `"Enter password"` |
| — | `pw_eye` | PictureBox | `(223, 118)` | 16×18 | Resource: `eye_off`; **press-and-hold** to reveal (MouseDown → `PasswordChar='\0'`, MouseUp → `'*'`) |
| 2 | `CompanyNm` | `TAUtil.TAComboBox` | `(15, 149)` | 230×26 | Calibri 11; LimitToList; AutoComplete=Append; left icon: `HomeHS`; placeholder: `"Select a company"`; ValueMember=`DataBaseID`, DisplayMember=`CompanyNm` |
| 3 | `ultraLabel2` | Label | `(15, 196)` | 110×27 | Text: `"Remember Me"` — Calibri 9pt Bold+Italic `#404040` |
| 4 | `uchkRememberMe` | CheckBox | `(131, 191)` | 19×25 | Cursor: Hand; transparent |
| 7 | `lbl_forgot_pw` | UltraLabel | `(15, 224)` | 99×18 | Text: `"Forgot Password?"` — Calibri 9pt italic; ForeColor: Blue; Cursor: Hand |

### Tab order

`UserID (0) → Password (1) → CompanyNm (2) → Remember Me label (3) → Remember Me checkbox (4) → Forgot Password (7)`

### Key behaviors

| Trigger | Action |
|---|---|
| `frmLogin_Shown` | Focus set to `Password` if UserID was pre-filled via Remember Me; otherwise set to `UserID` |
| `frmLogin_KeyDown`: Enter in Password field + both fields non-empty | Calls `btnOK_Click` |
| `frmLogin_Load` | Loads company list; if exactly 1 company auto-selects it; if Remember Me was checked pre-fills UserID and DatabaseID |
| `pw_eye MouseDown` | Sets `Password.PasswordChar = '\0'` (reveal) |
| `pw_eye MouseUp / MouseLeave` | Sets `Password.PasswordChar = '*'` (mask) |
| `lbl_forgot_pw Click` | Validates both User ID and Company are filled; calls `objLoginFactory.ForgotPassword` |

### Remember Me persistence

Stored in `resUser.resx` file on disk at `Application.StartupPath`:
- Key `RememberMe` → bool
- Key `LastUserID` → string
- Key `LastDatabaseID` → string

**Web equivalent:** `localStorage` keys `boss_remember_me`, `boss_last_user_id`, `boss_last_database_id`.

---

## frmSECChangePassword

### Form properties

| Property | Value |
|---|---|
| `ClientSize` | 728 × 498 px |
| `BackColor` | AliceBlue (`#F0F8FF`) |
| Shown as | Maximized (`WindowState = Maximized`) |
| Start position | `CenterScreen` |

### Toolbar — `tspBar`

| Property | Value |
|---|---|
| Height | ~60 px |
| Background | `#E7D6C5` |

| Button | Label | Icon | Action |
|---|---|---|---|
| `tsbClose` | `"&Close"` | `close` | Closes form |
| `tsbSave` | `"C&onfirm"` | `save` | Saves new password |

### Title area

| Control | Text | Font | Color |
|---|---|---|---|
| `pictureBox1` | — | — | Resource: `changepassword`, 50×50 |
| `ultraLabel20` | `"CHANGE PASSWORD"` | Calibri **15.75pt** Bold+Italic | `#6C4C2C` |
| `panel2` | — | — | Brown divider, 5px height, `#6C4C2C` |

### Form fields (inner `panel1`, background `#F3EAE2`)

| TabIndex | Control | Label | Notes |
|---|---|---|---|
| 0 | `UserID` | `"User ID"` | Read-only; bound to current user |
| 1 | `UserName` | `"User Name"` | Read-only; bound to current user |
| 2 | `Password` | `"Old Password / OTP"` | PasswordChar `'*'`; eye-toggle `pw_eye`; auto-re-masks on Leave |
| 3 | `NewPassword` | `"New Password"` | Eye-toggle `new_pw_eye`; auto-re-masks on Leave |
| 4 | `ConfirmPassword` | `"Confirm Password"` | Eye-toggle `confirm_pw_eye`; auto-re-masks on Leave |
| — | `UserEmail` | `"Email"` | Tooltip: `"This email will receive OTP if you forget password and request OTP."` |

### Eye toggle mode (change-password vs login)

In `frmSECChangePassword` the eye behaves as a **click-toggle** (not press-and-hold). Clicking shows password; clicking again or leaving the field hides it. This is the **opposite** of the login page's press-and-hold pattern.

### Password Requirements Panel — `ultraGroupBox1`

Live feedback as user types into `NewPassword`:

| Rule | Label text |
|---|---|
| `notRecent` | `"■ Must not be one of the last three passwords, excluding OTP."` |
| `length` | `"■ Length must be between 8 and 30 characters."` |
| `case` | `"■ Must contain both upper and lower case letters (a-zA-Z)."` |
| `digit` | `"■ Must include at least one digit (0-9)."` |
| `special` | `"■ Must include at least one special character (!@#$%^&*()_+|~-=\`{}[]:\";<>?,./\\)."` |

`■` flips to `✅` (green label) when rule passes, `❎` (red label) when it fails.
The header reads: `"New Password Requirements :"` — Calibri 9pt Bold, brown `#6C4C2C`.

### Post-success behavior

- If shown because password was **expired/reset** → message: `"Password changed successfully. Please re-login with new password."` → form closes → user must re-login.
- If shown voluntarily → message: `"Password changed successfully."` → form closes → user stays logged in.
