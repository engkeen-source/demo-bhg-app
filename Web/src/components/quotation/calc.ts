/**
 * Frontend port of backend/app/calc.py (clsDocComUtility.cs CalForm).
 * Used for live, instant amount recompute on every keystroke.
 * Must stay in sync with the Python version.
 */

import type { QuotationLine, ComputeResult } from '@/lib/api'

const GRP_STOCK      = new Set(['Stock', 'Non_Stock', 'Service', 'Assembly'])
const GRP_CHARGES    = new Set(['Charges'])
const GRP_DISCOUNT   = new Set(['Discount'])
const GRP_TOTAL      = new Set(['Sub Total', 'Total', 'CF Total'])
const GRP_STRUCTURAL = new Set(['Header', 'Remark'])

function rnd4(n: number): number {
  return Math.round(n * 10000) / 10000
}

export function computeLines(
  lines: QuotationLine[],
  currencyRate = 1,
  docDiscountRate = 0,
  taxRate = 9,
): ComputeResult {
  const sorted = [...lines].sort((a, b) => a.line_no - b.line_no)

  let totalPrevious = 0
  let totalSt = 0
  let totalCf = 0
  let detTotalAmt = 0
  let taxableTotal = 0
  let resetTotal = false

  const computedLines: ComputeResult['lines'] = []

  for (const row of sorted) {
    const lt = row.line_type
    let price = row.price ?? 0
    const qty = row.qty ?? 0
    const taxable = row.taxable ?? true
    const rowTaxRate = row.tax_rate ?? 0

    let amt = 0
    let taxAmt = 0
    let taxAmtLocal = 0

    if (GRP_STOCK.has(lt)) {
      amt = rnd4(row.amount ?? 0)
      if (taxable && rowTaxRate > 0) taxAmt = rnd4(amt * rowTaxRate / 100)
      taxAmtLocal = rnd4(taxAmt * currencyRate)
      detTotalAmt += amt
      if (taxable) taxableTotal += amt
      totalCf += amt
      if (resetTotal) { totalSt = amt; resetTotal = false } else { totalSt += amt }

    } else if (GRP_CHARGES.has(lt)) {
      amt = rnd4(row.amount ?? 0)
      if (taxable && rowTaxRate > 0) taxAmt = rnd4(amt * rowTaxRate / 100)
      taxAmtLocal = rnd4(taxAmt * currencyRate)
      detTotalAmt += amt
      if (taxable) taxableTotal += amt
      totalCf += amt
      if (resetTotal) { totalSt = amt; resetTotal = false } else { totalSt += amt }

    } else if (GRP_DISCOUNT.has(lt)) {
      amt = rnd4(row.amount ?? 0)
      detTotalAmt += amt
      totalCf += amt
      if (resetTotal) { totalSt = amt; resetTotal = false } else { totalSt += amt }

    } else if (GRP_TOTAL.has(lt)) {
      if (lt === 'Sub Total') amt = totalSt
      else if (lt === 'CF Total') amt = totalCf
      else amt = totalPrevious + totalSt   // Total

      totalPrevious = amt
      resetTotal = true
      price = 0
      taxAmt = 0
      taxAmtLocal = 0

    } else {
      // Header / Remark – zero contribution
      amt = 0; price = 0; taxAmt = 0; taxAmtLocal = 0
    }

    computedLines.push({
      line_no: row.line_no,
      price,
      amount: amt,
      tax_amt: taxAmt,
      tax_amt_local: taxAmtLocal,
    })
  }

  const subTotal       = rnd4(detTotalAmt)
  const discountAmt    = rnd4(subTotal * docDiscountRate / 100)
  const afterDis       = rnd4(subTotal - discountAmt)
  const taxableAfterDis = rnd4(taxableTotal - rnd4(taxableTotal * docDiscountRate / 100))
  const taxTotal       = rnd4(taxableAfterDis * taxRate / 100)
  const grandTotal     = rnd4(afterDis + taxTotal)
  const homeSubTotal   = rnd4(subTotal  * currencyRate)
  const homeTaxTotal   = rnd4(taxTotal  * currencyRate)
  const homeTotal      = rnd4(afterDis  * currencyRate) + homeTaxTotal

  return {
    lines: computedLines,
    sub_total:       subTotal,
    discount_amt:    discountAmt,
    total_after_dis: afterDis,
    tax_total:       taxTotal,
    grand_total:     grandTotal,
    home_sub_total:  homeSubTotal,
    home_tax_total:  homeTaxTotal,
    home_total:      homeTotal,
  }
}

/** Types where price entry is blocked in the UI */
export const PRICE_FORBIDDEN = new Set([...GRP_TOTAL, ...GRP_STRUCTURAL])
/** Types where qty entry is blocked in the UI */
export const QTY_FORBIDDEN   = new Set([...GRP_CHARGES, ...GRP_TOTAL, ...GRP_STRUCTURAL])
/** Types that don't require an item selection */
export const ITEM_OPTIONAL   = new Set([...GRP_TOTAL, ...GRP_STRUCTURAL])

export const ALL_LINE_TYPES = [
  'Header', 'Charges', 'Stock', 'Assembly', 'Non_Stock', 'Service', 'Discount', 'Remark',
  'Sub Total', 'Total', 'CF Total',
]
