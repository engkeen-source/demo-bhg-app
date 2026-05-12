export type LeafType = 'dashboard' | 'list-edit' | 'transaction' | 'report' | 'wizard' | 'tool'

export interface NavLeaf {
  slug: string
  title: string
  type: LeafType
  desktop?: string
  description?: string
}

export interface NavGroup {
  id: string
  title: string
  children: NavLeaf[]
}

export interface NavSection {
  id: string
  title: string
  icon: string
  children: (NavLeaf | NavGroup)[]
}

function isLeaf(node: NavLeaf | NavGroup): node is NavLeaf {
  return 'slug' in node
}

export { isLeaf }

export const NAV: NavSection[] = [
  {
    id: 'home',
    title: 'Dashboard',
    icon: 'home',
    children: [
      { slug: 'app', title: 'Overview', type: 'dashboard' },
    ],
  },
  {
    id: 'transactions',
    title: 'Transactions',
    icon: 'transactions',
    children: [
      {
        id: 'sales',
        title: 'Sales',
        children: [
          { slug: 'transactions/sales/quotation',           title: 'Quotation',                  type: 'transaction', desktop: 'frmARQO' },
          { slug: 'transactions/sales/sales-order',         title: 'Sales Order',                type: 'transaction', desktop: 'frmARSO' },
          { slug: 'transactions/sales/reserve-order',       title: 'Reserve Order',              type: 'transaction', desktop: 'frmARSO (Reserve Order)' },
          { slug: 'transactions/sales/delivery-order',      title: 'Delivery Order',             type: 'transaction', desktop: 'frmARSO' },
          { slug: 'transactions/sales/sales-invoice',       title: 'Sales Invoice',              type: 'transaction', desktop: 'frmARSO' },
          { slug: 'transactions/sales/sales-credit-note',   title: 'Sales Credit Note',          type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/sales/sales-debit-note',    title: 'Sales Debit Note',           type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/sales/payment-received',    title: 'Payment Received',           type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/sales/cash-sale',           title: 'Cash Sale',                  type: 'transaction', desktop: 'frmARSO (Cash Sale)' },
          { slug: 'transactions/sales/cash-payment-received', title: 'Cash Payment Received',   type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/sales/cash-credit-note',    title: 'Cash Credit Note',           type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/sales/cash-debit-note',     title: 'Cash Debit Note',            type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/sales/contra',              title: 'Contra',                     type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/sales/deposit',             title: 'Deposit',                    type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/sales/consignment-order',   title: 'Consignment Order',          type: 'transaction', desktop: 'frmARSO (Consignment Order)' },
          { slug: 'transactions/sales/consignment-settlement', title: 'Consignment Settlement',  type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/sales/sales-adjustment',    title: 'Sales Adjustment',           type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/sales/packing-list',        title: 'Packing List',               type: 'transaction', desktop: 'frmARSO (Packing List)' },
          { slug: 'transactions/sales/ar-opening-balance',  title: 'AR Opening Balance',         type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/sales/ar-cash-opening-balance', title: 'AR Cash Opening Balance', type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/sales/do-to-iv-transfer',   title: 'DO to IV Transfer',          type: 'tool',        desktop: 'frmARSO (DO→IV)' },
          { slug: 'transactions/sales/works-order',         title: 'Works Order',                type: 'transaction', desktop: 'frmARSO (Works Order)' },
        ],
      },
      {
        id: 'purchase',
        title: 'Purchase',
        children: [
          { slug: 'transactions/purchase/purchase-request',  title: 'Purchase Request',          type: 'transaction', desktop: 'frmARQO (PR)' },
          { slug: 'transactions/purchase/purchase-order',    title: 'Purchase Order',            type: 'transaction', desktop: 'frmARSO (PO)' },
          { slug: 'transactions/purchase/purchase-delivery', title: 'Purchase Delivery',         type: 'transaction', desktop: 'frmARSO (GR)' },
          { slug: 'transactions/purchase/purchase-invoice',  title: 'Purchase Invoice',          type: 'transaction', desktop: 'frmARSO (PI)' },
          { slug: 'transactions/purchase/purchase-credit-note', title: 'Purchase Credit Note',   type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/purchase/purchase-debit-note',  title: 'Purchase Debit Note',    type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/purchase/payment-issue',     title: 'Payment Issue',             type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/purchase/purchase-adjustment', title: 'Purchase Adjustment',     type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/purchase/purchase-plan',     title: 'Purchase Plan',             type: 'transaction', desktop: 'frmARQO (PPL)' },
          { slug: 'transactions/purchase/purchase-shipment', title: 'Purchase Shipment',         type: 'transaction', desktop: 'frmARSO (Shipment)' },
          { slug: 'transactions/purchase/purchase-order-adjustment', title: 'PO Adjustment',     type: 'transaction', desktop: 'frmARSO' },
          { slug: 'transactions/purchase/arap-revaluation',  title: 'AR/AP Revaluation',         type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/purchase/ap-opening-balance', title: 'AP Opening Balance',       type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/purchase/generate-po',       title: 'Generate PO',               type: 'wizard',      desktop: 'frmGeneratePO' },
        ],
      },
      {
        id: 'accounts',
        title: 'Accounts',
        children: [
          { slug: 'transactions/accounts/journal',             title: 'Journal',                 type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/accounts/cash-flow',           title: 'Cash Flow',               type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/accounts/cash-contra',         title: 'Cash Contra',             type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/accounts/financial-charge',    title: 'Financial Charge',        type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/accounts/bank-revaluation',    title: 'Bank Revaluation',        type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/accounts/account-opening-balance', title: 'Account Opening Balance', type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/accounts/cash-adjustment',     title: 'Cash Adjustment',         type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/accounts/budget',              title: 'Budget',                  type: 'list-edit',   desktop: 'frmMSTBudget' },
          { slug: 'transactions/accounts/job-timesheet',       title: 'Job Timesheet',           type: 'transaction', desktop: 'frmMSTTimesheet' },
        ],
      },
      {
        id: 'inventory',
        title: 'Inventory',
        children: [
          { slug: 'transactions/inventory/inventory-adjustment',    title: 'Inventory Adjustment',     type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/inventory/inventory-transfer',      title: 'Inventory Transfer',       type: 'transaction', desktop: 'frmARSO' },
          { slug: 'transactions/inventory/inventory-production',    title: 'Inventory Production',     type: 'transaction', desktop: 'frmARSO' },
          { slug: 'transactions/inventory/inventory-opening-balance', title: 'Inventory Opening Balance', type: 'transaction', desktop: 'frmARRO' },
          { slug: 'transactions/inventory/stock-take',              title: 'Stock Take',               type: 'tool',        desktop: 'frmItemStockTake' },
        ],
      },
    ],
  },
  {
    id: 'masters',
    title: 'Masters',
    icon: 'masters',
    children: [
      { slug: 'masters/customer-vendor',    title: 'Customer / Vendor Record',  type: 'list-edit',  desktop: 'frmMSTCon' },
      { slug: 'masters/inventory-item',     title: 'Inventory Item',            type: 'list-edit',  desktop: 'frmMSTItm' },
      { slug: 'masters/account',            title: 'Chart of Account',          type: 'list-edit',  desktop: 'frmMSTAcc' },
      { slug: 'masters/account-branch',     title: 'Account Branch',            type: 'list-edit',  desktop: 'frmMSTAccBranch' },
      { slug: 'masters/account-department', title: 'Department',                type: 'list-edit',  desktop: 'frmMSTAccDept' },
      { slug: 'masters/account-tran-group', title: 'Transaction Group',         type: 'list-edit',  desktop: 'frmMSTAccTranGrp' },
      { slug: 'masters/job',                title: 'Job Record',                type: 'list-edit',  desktop: 'frmMSTJob' },
      { slug: 'masters/sales-representative', title: 'Sales Representative',   type: 'list-edit',  desktop: 'frmMSTSalesRep' },
      { slug: 'masters/price-list',         title: 'Price List',                type: 'list-edit',  desktop: 'frmMSTPriceInfo' },
      { slug: 'masters/vehicle',            title: 'Vehicle Record',            type: 'list-edit',  desktop: 'frmMSTVehicle' },
      { slug: 'masters/ship-name',          title: 'Ship Name',                 type: 'list-edit',  desktop: 'frmMSTShipName' },
      { slug: 'masters/account-open-bal',   title: 'Account Opening Balance',   type: 'list-edit',  desktop: 'frmMSTAccOpenBal' },
      { slug: 'masters/customer-open-bal',  title: 'Customer/Vendor Opening Balance', type: 'list-edit', desktop: 'frmMSTConOpenBal' },
      { slug: 'masters/item-opening-bal',   title: 'Item Opening Balance',      type: 'list-edit',  desktop: 'frmMSTItmOpeningBal' },
      { slug: 'masters/item-update',        title: 'Item Mass Update',          type: 'tool',       desktop: 'frmItmUpdate' },
      { slug: 'masters/key-customer',       title: 'Key Customer',              type: 'list-edit',  desktop: 'frmKeyCustomer' },
      { slug: 'masters/budget-copy',        title: 'Budget Copy',               type: 'wizard',     desktop: 'frmMSTBudgetCopy' },
    ],
  },
  {
    id: 'references',
    title: 'References',
    icon: 'references',
    children: [
      { slug: 'references/uom',             title: 'Unit of Measure (UOM)',     type: 'list-edit',  desktop: 'frmREFUOM' },
      { slug: 'references/bank',            title: 'Bank',                      type: 'list-edit',  desktop: 'frmREFBank' },
      { slug: 'references/brand',           title: 'Brand',                     type: 'list-edit',  desktop: 'frmREFBrand' },
      { slug: 'references/category',        title: 'Category',                  type: 'list-edit',  desktop: 'frmREFCat' },
      { slug: 'references/color',           title: 'Color',                     type: 'list-edit',  desktop: 'frmREFColor' },
      { slug: 'references/currency',        title: 'Currency',                  type: 'list-edit',  desktop: 'frmREFCurr' },
      { slug: 'references/document-group',  title: 'Document Group',            type: 'list-edit',  desktop: 'frmREFDocGrp' },
      { slug: 'references/equipment-type',  title: 'Equipment Type',            type: 'list-edit',  desktop: 'frmREFEqptType' },
      { slug: 'references/id-format',       title: 'ID Format / Document Code', type: 'list-edit',  desktop: 'frmREFIDFormat' },
      { slug: 'references/industry',        title: 'Industry',                  type: 'list-edit',  desktop: 'frmREFIndustry' },
      { slug: 'references/interest',        title: 'Interest',                  type: 'list-edit',  desktop: 'frmREFInterest' },
      { slug: 'references/job-cost-type',   title: 'Job Cost Type',             type: 'list-edit',  desktop: 'frmREFJobCostType' },
      { slug: 'references/job-group',       title: 'Job Group',                 type: 'list-edit',  desktop: 'frmREFJobGrp' },
      { slug: 'references/job-phase',       title: 'Job Phase',                 type: 'list-edit',  desktop: 'frmREFJobPhase' },
      { slug: 'references/job-task',        title: 'Job Task',                  type: 'list-edit',  desktop: 'frmREFJobTask' },
      { slug: 'references/location',        title: 'Location / Warehouse',      type: 'list-edit',  desktop: 'frmREFLoc' },
      { slug: 'references/overhead',        title: 'Overhead',                  type: 'list-edit',  desktop: 'frmREFOverHead' },
      { slug: 'references/packing-type',    title: 'Packing Type',              type: 'list-edit',  desktop: 'frmREFPackingType' },
      { slug: 'references/payment-mode',    title: 'Payment Mode',              type: 'list-edit',  desktop: 'frmREFPayMode' },
      { slug: 'references/scale',           title: 'Scale',                     type: 'list-edit',  desktop: 'frmREFScale' },
      { slug: 'references/shipping-mode',   title: 'Shipping Mode / Vessel',    type: 'list-edit',  desktop: 'frmREFShipVia' },
      { slug: 'references/tax-authority',   title: 'Tax Authority',             type: 'list-edit',  desktop: 'frmREFTaxA' },
      { slug: 'references/tax-group',       title: 'Tax Group',                 type: 'list-edit',  desktop: 'frmREFTaxGrp' },
      { slug: 'references/payment-term',    title: 'Payment Term',              type: 'list-edit',  desktop: 'frmREFTerm' },
      { slug: 'references/territory',       title: 'Territory',                 type: 'list-edit',  desktop: 'frmREFTerritory' },
      { slug: 'references/account-group',   title: 'Account Group',             type: 'list-edit',  desktop: 'frmREFAccGrp' },
      { slug: 'references/work-order-type', title: 'Work Order Type',           type: 'list-edit',  desktop: 'frmREFWorkOrderType' },
      { slug: 'references/work-order-req-type', title: 'Work Order Request Type', type: 'list-edit', desktop: 'frmREFWorkOrderReqType' },
    ],
  },
  {
    id: 'reports',
    title: 'Reports',
    icon: 'reports',
    children: [
      {
        id: 'reports-sales',
        title: 'Sales Reports',
        children: [
          { slug: 'reports/quotation-listing',         title: 'Quotation Listing',          type: 'report' },
          { slug: 'reports/sales-order-listing',       title: 'Sales Order Listing',        type: 'report' },
          { slug: 'reports/delivery-order-listing',    title: 'Delivery Order Listing',     type: 'report' },
          { slug: 'reports/invoice-listing',           title: 'Invoice Listing',            type: 'report' },
          { slug: 'reports/sales-analysis',            title: 'Sales Analysis',             type: 'report' },
          { slug: 'reports/customer-statement',        title: 'Customer Statement',         type: 'report', desktop: 'frmCustomerStatement' },
          { slug: 'reports/ar-aging',                  title: 'AR Aging',                   type: 'report' },
          { slug: 'reports/ar-collection',             title: 'AR Collection',              type: 'report' },
        ],
      },
      {
        id: 'reports-purchase',
        title: 'Purchase Reports',
        children: [
          { slug: 'reports/purchase-order-listing',    title: 'Purchase Order Listing',     type: 'report' },
          { slug: 'reports/purchase-invoice-listing',  title: 'Purchase Invoice Listing',   type: 'report' },
          { slug: 'reports/ap-aging',                  title: 'AP Aging',                   type: 'report' },
        ],
      },
      {
        id: 'reports-inventory',
        title: 'Inventory Reports',
        children: [
          { slug: 'reports/stock-listing',             title: 'Stock Listing',              type: 'report' },
          { slug: 'reports/stock-movement',            title: 'Stock Movement',             type: 'report' },
          { slug: 'reports/stock-valuation',           title: 'Stock Valuation',            type: 'report' },
        ],
      },
      {
        id: 'reports-financial',
        title: 'Financial Reports',
        children: [
          { slug: 'reports/trial-balance',             title: 'Trial Balance',              type: 'report' },
          { slug: 'reports/profit-loss',               title: 'Profit & Loss',              type: 'report' },
          { slug: 'reports/balance-sheet',             title: 'Balance Sheet',              type: 'report' },
          { slug: 'reports/general-ledger',            title: 'General Ledger',             type: 'report' },
          { slug: 'reports/financial-report-designer', title: 'Financial Report Designer',  type: 'tool' },
        ],
      },
    ],
  },
  {
    id: 'security',
    title: 'Security',
    icon: 'security',
    children: [
      { slug: 'security/group',           title: 'Security Group',            type: 'list-edit',  desktop: 'frmSECGrp' },
      { slug: 'security/user',            title: 'User',                      type: 'list-edit',  desktop: 'frmSECUser' },
      { slug: 'security/user-list',       title: 'User List',                 type: 'list-edit',  desktop: 'frmSECUserList' },
      { slug: 'security/account-group',   title: 'Account Group',             type: 'list-edit',  desktop: 'frmSECAccGrp' },
      { slug: 'security/change-password', title: 'Change Password',           type: 'tool',       desktop: 'frmSECChangePassword' },
    ],
  },
  {
    id: 'settings',
    title: 'Settings',
    icon: 'settings',
    children: [
      { slug: 'settings/system-option',          title: 'System Option',           type: 'tool',      desktop: 'frmSYSOption' },
      { slug: 'settings/user-option',            title: 'User Option',             type: 'tool',      desktop: 'frmSYSUserOption' },
      { slug: 'settings/document-code',          title: 'Document Code',           type: 'list-edit', desktop: 'frmREFIDFormat' },
      { slug: 'settings/general-list',           title: 'General List',            type: 'list-edit', desktop: 'frmSYSGeneralList' },
      { slug: 'settings/wildcard-search',        title: 'Wildcard Search List',    type: 'list-edit', desktop: 'frmSYSWildcard' },
      { slug: 'settings/period',                 title: 'Period',                  type: 'tool',      desktop: 'frmPeriodChooser' },
      { slug: 'settings/company-setup',          title: 'Company Setup',           type: 'tool',      desktop: 'frmSYSCompanySetup' },
      { slug: 'settings/audit-log',              title: 'Audit Log',               type: 'list-edit', desktop: 'frmSYSAuditLog' },
      { slug: 'settings/system-lock',            title: 'System Lock List',        type: 'list-edit', desktop: 'frmSYSLockList' },
      { slug: 'settings/switch-user',            title: 'Switch User',             type: 'tool' },
      { slug: 'settings/excel-import',           title: 'Excel Import',            type: 'wizard',    desktop: 'frmExcelImport' },
    ],
  },
  {
    id: 'help',
    title: 'Help',
    icon: 'help',
    children: [
      { slug: 'help/about', title: 'About BossSO', type: 'tool' },
    ],
  },
]

export function findLeaf(slugPath: string): NavLeaf | undefined {
  for (const section of NAV) {
    for (const child of section.children) {
      if (isLeaf(child)) {
        if (child.slug === slugPath) return child
      } else {
        for (const leaf of child.children) {
          if (leaf.slug === slugPath) return leaf
        }
      }
    }
  }
  return undefined
}

export function getAllLeaves(): NavLeaf[] {
  const leaves: NavLeaf[] = []
  for (const section of NAV) {
    for (const child of section.children) {
      if (isLeaf(child)) {
        leaves.push(child)
      } else {
        leaves.push(...child.children)
      }
    }
  }
  return leaves
}

export function getBreadcrumb(slugPath: string): { title: string; href: string }[] {
  for (const section of NAV) {
    for (const child of section.children) {
      if (isLeaf(child)) {
        if (child.slug === slugPath) {
          return [
            { title: section.title, href: '#' },
            { title: child.title, href: `/${child.slug}` },
          ]
        }
      } else {
        for (const leaf of child.children) {
          if (leaf.slug === slugPath) {
            return [
              { title: section.title, href: '#' },
              { title: child.title, href: '#' },
              { title: leaf.title, href: `/${leaf.slug}` },
            ]
          }
        }
      }
    }
  }
  return []
}
