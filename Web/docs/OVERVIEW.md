# BossSO Desktop App — Architecture Overview

## Tech Stack

| Layer | Technology |
|---|---|
| Target framework | .NET Framework 4.8 |
| UI host | Windows Forms (MDI) |
| UI widgets | Infragistics NetAdvantage v11.1 (UltraGrid, UltraToolbarsManager, UltraTabbedMdi, UltraWinEditors…) |
| Business object framework | CSLA.NET 3.5.1 — all domain classes extend `BusinessBase` / `BusinessListBase` |
| Database access | Raw ADO.NET — `System.Data.SqlClient`, no ORM |
| Database server | Microsoft SQL Server (Windows or SQL Server auth) |
| Reporting | ActiveReports 6 (primary) + Crystal Reports 12 (legacy) |
| Encryption helper | `TAUtil.dll` — vendor binary (source not in repo). Used for connection-string encryption and password encoding. |
| Deployment | ClickOnce |
| Other deps | Microsoft.Web.WebView2 (HTML email preview), GdPicture.NET 10 (document imaging), Office Interop (Excel, Outlook export), Newtonsoft.Json, MySql.Data 8.0 (secondary) |

## Project Layout

```
bossSO/
  BOLib/               Business Object Library (.NET Class Library)
  WinUI/               Windows Forms UI (entry point: WinUI/Program.cs)
  DBScripts/           SQL DDL fragments (partial schema)
  packages/            NuGet packages (TAUtil.dll lives here)
```

### BOLib sub-folders

| Folder | Purpose |
|---|---|
| `Base Classes/` | ~250 CSLA domain objects grouped by module: AR (Sales Order/Invoice/DO/Quote/Return/Payment), AP (Purchase PO/Delivery/Invoice/Payment), GL (Journal/Deposit), Inventory (Adjustment/Transfer/Manufacturing), Master (Customer, Item, Job, Account, SalesRep…), Reference (lookup tables), Security (User, Group, Permission) |
| `Factory Classes/` | Factory wrappers: `clsLoginFactory`, `clsMSTConFactory`, `clsARSOFactory`, etc. |
| `Global Classes/` | App-wide singletons: `clsDatabase` (connection strings), `clsGVar`, `clsGFunc` (helpers + password verification), `clsGEnum`, `clsGEmail`, `clsAppInforUtility` |
| `List Classes/` | DataTable providers for grid lookups: `clsARList`, `clsMASList`, `clsREFList`, `clsSECList`, `clsSYSList` |
| `Utility Classes/` | `clsSECPermUtility`, `clsSysOptionUtility`, `clsSysIDCounterUtility`, `clsSysLockUtility`, `clsSysMessageUtility`, `clsSysAuditLogUtility` |
| `DocUtility/` | XML / IAF (Malaysian IRB Audit File) export, document serialisation |
| `DocTmp/` | Temporary DTOs for document copying and history |

### WinUI/Forms sub-folders

| Folder | Forms |
|---|---|
| `Main/` | `frmMain` (MDI shell) + `AboutBox` |
| `Security/` | `frmLogin`, `frmSECChangePassword` |
| `AR/` | `frmARSO` (Sales Order), `frmARQO` (Quotation), `frmARRO` (Sales Return) |
| `Masters/` | ~27 masters: Account, Customer/Vendor, Item, Job, SalesRep, ShipName, Budget, PriceInfo, Timesheet, Vehicle, KeyCustomer |
| `References/` | ~28 lookup forms: UOM, Bank, Brand, Category, Color, Currency, TaxA, TaxGrp, Term, Territory, Location, PayMode, ShipVia… |
| `Reports/` | Report viewer, email, print, page-setup, export infrastructure |
| `PoupBrowser/` | ~150 modal helper dialogs (doc search, batch/serial pick, attachments, Excel import…) |
| `PopupTreeView/` | Tree-based picker |

## Database Schema (top level)

Tables are inferred from CSLA classes — only a partial DDL script is in the repo.

| Domain | Key Tables |
|---|---|
| Security | `SEC_User`, `SEC_UserDetGrp`, `SEC_Grp`, `SEC_Perm`, `SEC_UserPermissionVw`, `SEC_PasswordHistory` |
| System | `SYS_Option`, `SYS_Code`, `SYS_IDCounter`, `SYS_Lock`, `SYS_Log`, `SYS_Attachment`, `SYS_Rep` |
| References | `REF_CmpList` (master DB), `REF_AccGrp`, `REF_Bank`, `REF_Brand`, `REF_Cat`, `REF_Curr`, `REF_TaxA`, `REF_TaxGrp`, `REF_Term`, `REF_UOM`, `REF_Loc`… |
| Masters | `MST_Acc`, `MST_Con` (Customer/Vendor), `MST_Itm` (Item), `MST_Job`, `MST_SalesRep`, `MST_Budget`, `MST_PriceList`… |
| Transactions | `AR_SO`, `AR_QO`, `AR_DO`, `AR_IV`, `AR_PY`; `AP_PO`, `AP_BL`, `AP_PY`; `GL_JNL`, `GL_DP`; `IN_ADJ`, `IN_TRN` |

## MDI Shell (frmMain)

- **MDI parent** with Infragistics `UltraTabbedMdiManager` — child forms open as tabs.
- **Menu bar** (`UltraToolbarsManager`): File, Security, Settings, Masters, References, Transactions, Reports, Definition, Windows, Help.
- **Status bar** (`UltraStatusBar`): panels showing current user, company, period, date.
- **Lifecycle**: `frmMain_Load` opens `frmLogin` (modal). After successful login the main form becomes active and populates the status bar from `AppInfor` / `clsAppInforUtility`.

## Multi-Company / Multi-Database

The app supports multiple company databases from a single installation:

1. A **master DB** (configured in `app.config` → `BOSSSystemMasterConnection`) holds `REF_CmpList` — one row per company database with its connection string.
2. At login the user picks a **company name** from a dropdown; the app looks up the matching `REF_CmpList` row and builds the per-company SQL Server connection string.
3. Per-company DBs are entirely separate SQL Server databases; the master DB only stores the directory.
