# UI Assets Reference

## Desktop Resources (originals)

All original icon resources live in `WinUI/Properties/Resources.resx` and are embedded
in the WinUI assembly. They are `.ico` / `.png` / `.bmp` files accessed via
`Properties.Resources.<Name>` in C#.

To extract them:
1. Open the solution in Visual Studio.
2. Open `WinUI/Properties/Resources.resx` in the Resource Editor.
3. Right-click each image → **Save image as…**

Or use `ILSpy` to extract from the compiled `WinUI.exe` assembly.

---

## Icon Inventory

| Resource name | Used in | Description | Web replacement |
|---|---|---|---|
| `Lkeys` | `frmLogin` (pictureBox1, 61×58) | A yellow/gold key on light background — the main login graphic | `public/icons/key-large.svg` |
| `User` | `frmLogin` UserID left icon, 14×14 | Person silhouette | `public/icons/user.svg` |
| `PrimaryKeyHS` | `frmLogin` Password left icon, 14×14 | Small key icon | `public/icons/key-small.svg` |
| `HomeHS` | `frmLogin` Company left icon, 14×14 | House / home | `public/icons/home.svg` |
| `door_back_32` | Toolbar Exit button, 28×28 | Door with back-arrow | `public/icons/door-exit.svg` |
| `user_next_32` | Toolbar Login button, 28×28 | Person with forward-arrow | `public/icons/user-login.svg` |
| `eye_off` | `frmLogin` pw_eye (default) | Eye with slash (masked) | `public/icons/eye-off.svg` |
| `eye_open` | `frmLogin` pw_eye (while held) | Open eye (revealed) | `public/icons/eye-open.svg` |
| `changepassword` | `frmSECChangePassword` pictureBox1, 50×50 | Lock with refresh arrows | `public/icons/change-password.svg` |
| `close` | Change-pw toolbar Close button | ✕ or door | `public/icons/door-exit.svg` (reused) |
| `save` | Change-pw toolbar Confirm button | Floppy/checkmark | `public/icons/change-password.svg` (reused) |

## App Icons

| File | Used in | Notes |
|---|---|---|
| `WinUI/boss.ico` | Main app `.ico` | Multi-resolution Windows icon |
| `WinUI/Account.ico` | Secondary icon | — |

Web replacement: `public/boss-icon.svg`

---

## Font Stack

The desktop uses **Calibri** (default since Office 2007, installed with Windows).
For web:

```css
font-family: 'Calibri', 'Trebuchet MS', 'Liberation Sans', Arial, sans-serif;
```

On Windows the user will see Calibri (pixel-perfect match). On macOS/Linux they'll see
Trebuchet MS or Arial as fallback — still visually close.

Optional: embed Calibri via `@font-face` using a licensed font file if pixel-perfect
cross-platform match is required.

## Color Tokens

Defined in `Web/src/styles/globals.css` and `tailwind.config.ts`:

| Token | Value | Source |
|---|---|---|
| `--beige-1` / `boss.beige1` | `#E7D6C5` | Toolbar + panel-header gradient start |
| `--beige-2` / `boss.beige2` | `#F3EAE2` | Right panel bg + gradient end |
| `--boss-dark` / `boss.dark` | `#404040` | All text |
| `--boss-brown` / `boss.brown` | `#6C4C2C` | Change-pw title + divider |
| Panel border | `#C8B4A0` | Inferred from designer |
| Dialog shadow | `rgba(0,0,0,0.22)` | Approximation of WinForms `FixedDialog` shadow |
| MDI background | `#E8E8E8` | Approximation of `MdiClient` default gray |
