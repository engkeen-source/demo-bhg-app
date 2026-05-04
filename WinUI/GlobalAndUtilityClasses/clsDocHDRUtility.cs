using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.Misc;
using System.IO;
using BOLib;
using Infragistics.Win.UltraWinMaskedEdit;
using System.Windows.Forms;
using TAUtil;
using System.Xml;
using System.IO.Compression;
using System.Configuration;
using System.Drawing;

namespace WinUI
{
    public class DocHDRUtil
    {
        //FORM event
        public static void FormControlLock_Set(System.Windows.Forms.Form frm, Document objDoc, string permID, bool FormLoad,bool iscash=false,DataTable dt=null)
        {
            //Note : this function sets the FORM controls Enable, Visible, Readonly property when FormLoad
            //If ObjDoc is readonly then the grid is also set to readonly
            string formCaption = string.Empty;
            int docCurrKey = 0;
            int docTaxGrpKey = 0;
            bool IsCustomGST = false;
            bool IsPendingCancel = false;
            bool draftEnable = false;
            bool ntApprovalRequired = false;

            try
            {
                if (FormLoad)
                {
                    //Set Form Caption
                    //frm.Text = DocComUtility.FormCaption_Set((int)objDoc.DocCodeKey);
                    try
                    {
                        frm.Text = ((GEnum.SystemCode)objDoc.DocCodeKey).ToString().Replace("_"," ");
                    }
                    catch { }

                    if ((int)objDoc.DocCodeKey==(int)GEnum.SystemCode.Purchase_Order && SysOptionUtility.DatabaseBranchCode == "OMS")
                    {
                         GlobalUI.Ctrl_Update(frm, "DocDONUm", GEnum.CtlPropertyUpdate.Enabled, false);
                    }

                    #region Set DocID - Enable/Disable
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Quotation:
                        case (int)GEnum.SystemCode.Sales_Order:
                        case (int)GEnum.SystemCode.Reserve_Order:
                        case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                        case (int)GEnum.SystemCode.Delivery_Order:
                        case (int)GEnum.SystemCode.Packing_List:
                        case (int)GEnum.SystemCode.Sales_Invoice:
                        case (int)GEnum.SystemCode.Sales_Debit_Note:
                        case (int)GEnum.SystemCode.Sales_Credit_Note:
                        case (int)GEnum.SystemCode.Sales_Adjustment:
                        case (int)GEnum.SystemCode.Payment_Received:
                        case (int)GEnum.SystemCode.Contra:
                        case (int)GEnum.SystemCode.Cash_Sale:
                        case (int)GEnum.SystemCode.Cash_Debit_Note:
                        case (int)GEnum.SystemCode.Cash_Credit_Note:
                        case (int)GEnum.SystemCode.Cash_Adjustment:
                        case (int)GEnum.SystemCode.Cash_Payment_Received:
                        case (int)GEnum.SystemCode.Cash_Contra:
                        case (int)GEnum.SystemCode.Purchase_Plan:
                        case (int)GEnum.SystemCode.Purchase_Request:
                        case (int)GEnum.SystemCode.Purchase_Order:
                        case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                        case (int)GEnum.SystemCode.Purchase_Delivery:
                        //case (int)GEnum.SystemCode.Purchase_Invoice:
                        //case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        //case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        case (int)GEnum.SystemCode.Purchase_Adjustment:
                        case (int)GEnum.SystemCode.Payment_Issue:
                        case (int)GEnum.SystemCode.Inventory_Adjustment:
                        case (int)GEnum.SystemCode.Inventory_Production:
                        case (int)GEnum.SystemCode.Inventory_Transfer:
                        case (int)GEnum.SystemCode.Issue_Consignment:
                        case (int)GEnum.SystemCode.Return_Consignment:
                        case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                        case (int)GEnum.SystemCode.Journal:
                        case (int)GEnum.SystemCode.Deposit:
                        case (int)GEnum.SystemCode.Bank_Revaluation:
                            if (SECPermUtility.Perform("SysDocID", false) || objDoc.IsReadOnly)
                                GlobalUI.Ctrl_Update(frm, "DocID", GEnum.CtlPropertyUpdate.Enabled, true);
                            else
                                GlobalUI.Ctrl_Update(frm, "DocID", GEnum.CtlPropertyUpdate.Enabled, false);
                            break;

                        case (int)GEnum.SystemCode.Order_Consignment:
                        case (int)GEnum.SystemCode.Received_Consignment:
                        case (int)GEnum.SystemCode.Consignment_Settlement:
                            if (SECPermUtility.Perform("SysDocID", false) || objDoc.IsReadOnly)
                                GlobalUI.Ctrl_Update(frm, "DocID", GEnum.CtlPropertyUpdate.Enabled, true);
                            else
                                GlobalUI.Ctrl_Update(frm, "DocID", GEnum.CtlPropertyUpdate.Enabled, false);

                            if (objDoc.DocState != (int)GEnum.DocState.New || objDoc.IsReadOnly)
                            {
                                GlobalUI.Ctrl_Update(frm, "DocConKey", GEnum.CtlPropertyUpdate.Enabled, false);
                            }
                            break;
                    }
                    #endregion

                    #region Set btnApplyIV - Enable/Disable
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Sales_Credit_Note:
                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            GlobalUI.Ctrl_Update(frm, "btnApplyIV", GEnum.CtlPropertyUpdate.Enabled, !objDoc.IsReadOnly);
                            break;
                    }
                    #endregion

                    #region Set DocDeptKey - Visibility
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Quotation:
                        case (int)GEnum.SystemCode.Sales_Order:
                        case (int)GEnum.SystemCode.Reserve_Order:
                        case (int)GEnum.SystemCode.Delivery_Order:
                        case (int)GEnum.SystemCode.Sales_Invoice:
                        case (int)GEnum.SystemCode.Sales_Debit_Note:
                        case (int)GEnum.SystemCode.Sales_Credit_Note:
                        case (int)GEnum.SystemCode.Sales_Adjustment:
                        case (int)GEnum.SystemCode.Payment_Received:
                        case (int)GEnum.SystemCode.Contra:
                        case (int)GEnum.SystemCode.Cash_Sale:
                        case (int)GEnum.SystemCode.Cash_Debit_Note:
                        case (int)GEnum.SystemCode.Cash_Credit_Note:
                        case (int)GEnum.SystemCode.Cash_Adjustment:
                        case (int)GEnum.SystemCode.Cash_Payment_Received:
                        case (int)GEnum.SystemCode.Cash_Contra:
                        case (int)GEnum.SystemCode.Purchase_Plan:
                        case (int)GEnum.SystemCode.Purchase_Order:
                        case (int)GEnum.SystemCode.Purchase_Delivery:
                        case (int)GEnum.SystemCode.Purchase_Invoice:
                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        case (int)GEnum.SystemCode.Purchase_Adjustment:
                        case (int)GEnum.SystemCode.Payment_Issue:
                        case (int)GEnum.SystemCode.Inventory_Adjustment:
                        case (int)GEnum.SystemCode.Inventory_Production:
                        case (int)GEnum.SystemCode.Inventory_Transfer:
                        case (int)GEnum.SystemCode.Issue_Consignment:
                        case (int)GEnum.SystemCode.Return_Consignment:
                        case (int)GEnum.SystemCode.Deposit:
                        case (int)GEnum.SystemCode.Bank_Revaluation:
                            if (SysOptionUtility.UseDept)
                            {
                                GlobalUI.Ctrl_Update(frm, "DocDeptKey", GEnum.CtlPropertyUpdate.Visible, true);
                                GlobalUI.Ctrl_Update(frm, "DocDeptKeyLabel", GEnum.CtlPropertyUpdate.Visible, true);
                            }
                            else
                            {
                                GlobalUI.Ctrl_Update(frm, "DocDeptKey", GEnum.CtlPropertyUpdate.Visible, false);
                                GlobalUI.Ctrl_Update(frm, "DocDeptKeyLabel", GEnum.CtlPropertyUpdate.Visible, false);
                            }
                            break;
                    }
                    #endregion

                    #region Set BranchKey - Visibility
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Quotation:
                        case (int)GEnum.SystemCode.Sales_Order:
                        case (int)GEnum.SystemCode.Reserve_Order:
                        case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                        case (int)GEnum.SystemCode.Delivery_Order:
                        case (int)GEnum.SystemCode.Packing_List:
                        case (int)GEnum.SystemCode.Sales_Invoice:
                        case (int)GEnum.SystemCode.Sales_Debit_Note:
                        case (int)GEnum.SystemCode.Sales_Credit_Note:
                        case (int)GEnum.SystemCode.Sales_Adjustment:
                        case (int)GEnum.SystemCode.Payment_Received:
                        case (int)GEnum.SystemCode.Contra:
                        case (int)GEnum.SystemCode.Cash_Sale:
                        case (int)GEnum.SystemCode.Cash_Debit_Note:
                        case (int)GEnum.SystemCode.Cash_Credit_Note:
                        case (int)GEnum.SystemCode.Cash_Adjustment:
                        case (int)GEnum.SystemCode.Cash_Payment_Received:
                        case (int)GEnum.SystemCode.Cash_Contra:
                        case (int)GEnum.SystemCode.Purchase_Plan:
                        case (int)GEnum.SystemCode.Purchase_Request:
                        case (int)GEnum.SystemCode.Purchase_Order:
                        case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                        case (int)GEnum.SystemCode.Purchase_Delivery:
                        case (int)GEnum.SystemCode.Purchase_Invoice:
                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        case (int)GEnum.SystemCode.Purchase_Adjustment:
                        case (int)GEnum.SystemCode.Payment_Issue:
                        case (int)GEnum.SystemCode.Inventory_Adjustment:
                        case (int)GEnum.SystemCode.Inventory_Production:
                        case (int)GEnum.SystemCode.Inventory_Transfer:
                        case (int)GEnum.SystemCode.Issue_Consignment:
                        case (int)GEnum.SystemCode.Return_Consignment:
                        case (int)GEnum.SystemCode.Order_Consignment:
                        case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                        case (int)GEnum.SystemCode.Received_Consignment:
                        case (int)GEnum.SystemCode.Consignment_Settlement:
                        case (int)GEnum.SystemCode.Journal:
                        case (int)GEnum.SystemCode.Deposit:
                        case (int)GEnum.SystemCode.Bank_Revaluation:
                            if (SysOptionUtility.UseBranch)
                            {
                                GlobalUI.Ctrl_Update(frm, "BranchKey", GEnum.CtlPropertyUpdate.Visible, true);
                                GlobalUI.Ctrl_Update(frm, "BranchKeyLabel", GEnum.CtlPropertyUpdate.Visible, true);
                            }
                            else
                            {
                                GlobalUI.Ctrl_Update(frm, "BranchKey", GEnum.CtlPropertyUpdate.Visible, false);
                                GlobalUI.Ctrl_Update(frm, "BranchKeyLabel", GEnum.CtlPropertyUpdate.Visible, false);
                            }
                            break;
                    }
                    #endregion

                    #region Set DocJobKey - Visibility
                    if (SysOptionUtility.UseProject == false)
                    {
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Sales_Adjustment:
                            case (int)GEnum.SystemCode.Cash_Adjustment:
                            case (int)GEnum.SystemCode.Purchase_Adjustment:
                                GlobalUI.Ctrl_Update(frm, "DocJobKey", GEnum.CtlPropertyUpdate.Visible, false);
                                GlobalUI.Ctrl_Update(frm, "DocJobKeyLabel", GEnum.CtlPropertyUpdate.Visible, false);
                                GlobalUI.Ctrl_Update(frm, "DocJobTaskKey", GEnum.CtlPropertyUpdate.Visible, false);
                                GlobalUI.Ctrl_Update(frm, "DocJobTaskKeyLabel", GEnum.CtlPropertyUpdate.Visible, false);
                                GlobalUI.Ctrl_Update(frm, "DocJobCostTypeKey", GEnum.CtlPropertyUpdate.Visible, false);
                                GlobalUI.Ctrl_Update(frm, "DocJobCostTypeKeyLabel", GEnum.CtlPropertyUpdate.Visible, false);
                                GlobalUI.Ctrl_Update(frm, "DocJobPhaseKey", GEnum.CtlPropertyUpdate.Visible, false);
                                GlobalUI.Ctrl_Update(frm, "DocJobPhaseKeyLabel", GEnum.CtlPropertyUpdate.Visible, false);
                                break;

                            case (int)GEnum.SystemCode.Quotation:
                            case (int)GEnum.SystemCode.Sales_Order:
                            case (int)GEnum.SystemCode.Reserve_Order:
                            case (int)GEnum.SystemCode.Delivery_Order:
                            case (int)GEnum.SystemCode.Sales_Invoice:
                            case (int)GEnum.SystemCode.Sales_Debit_Note:
                            case (int)GEnum.SystemCode.Sales_Credit_Note:
                            case (int)GEnum.SystemCode.Cash_Sale:
                            case (int)GEnum.SystemCode.Cash_Debit_Note:
                            case (int)GEnum.SystemCode.Cash_Credit_Note:
                            case (int)GEnum.SystemCode.Purchase_Order:
                            case (int)GEnum.SystemCode.Purchase_Delivery:
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            case (int)GEnum.SystemCode.Inventory_Adjustment:
                            case (int)GEnum.SystemCode.Issue_Consignment:
                            case (int)GEnum.SystemCode.Return_Consignment:
                            case (int)GEnum.SystemCode.Order_Consignment:
                            case (int)GEnum.SystemCode.Received_Consignment:
                            case (int)GEnum.SystemCode.Journal:
                                GlobalUI.Ctrl_Update(frm, "DefJobKey", GEnum.CtlPropertyUpdate.Visible, false);
                                GlobalUI.Ctrl_Update(frm, "DefJobKeyLabel", GEnum.CtlPropertyUpdate.Visible, false);
                                GlobalUI.Ctrl_Update(frm, "btnSetAllDetJob", GEnum.CtlPropertyUpdate.Visible, false);
                                break;
                        }
                    }
                    #endregion

                    #region Set (Payment button, TabApplyCN, TabAdditionalCost - Visibility
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Sales_Invoice:
                        case (int)GEnum.SystemCode.Sales_Debit_Note:
                        case (int)GEnum.SystemCode.Cash_Sale:
                        case (int)GEnum.SystemCode.Cash_Debit_Note:
                            GlobalUI.Ctrl_Update(frm, "btnPayment", GEnum.CtlPropertyUpdate.Visible, true);
                            GlobalUI.Ctrl_Update(frm, "ultraGroupBoxApplyCN", GEnum.CtlPropertyUpdate.Visible, false);
                            break;

                        case (int)GEnum.SystemCode.Sales_Credit_Note:
                        case (int)GEnum.SystemCode.Cash_Credit_Note:
                            GlobalUI.Ctrl_Update(frm, "btnPayment", GEnum.CtlPropertyUpdate.Visible, false);
                            GlobalUI.Ctrl_Update(frm, "ultraGroupBoxApplyCN", GEnum.CtlPropertyUpdate.Visible, true);
                            break;

                        case (int)GEnum.SystemCode.Purchase_Invoice:
                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            GlobalUI.Ctrl_Update(frm, "btnPayment", GEnum.CtlPropertyUpdate.Visible, true);
                            GlobalUI.Ctrl_Update(frm, "ultraGroupBoxApplyCN", GEnum.CtlPropertyUpdate.Visible, false);
                            GlobalUI.Ctrl_Update(frm, "ultraGroupBoxAddCost", GEnum.CtlPropertyUpdate.Visible, true);
                            break;

                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            GlobalUI.Ctrl_Update(frm, "btnPayment", GEnum.CtlPropertyUpdate.Visible, false);
                            GlobalUI.Ctrl_Update(frm, "ultraGroupBoxApplyCN", GEnum.CtlPropertyUpdate.Visible, true);
                            GlobalUI.Ctrl_Update(frm, "ultraGroupBoxAddCost", GEnum.CtlPropertyUpdate.Visible, false);
                            break;

                    }
                    #endregion

                    #region Set Payment Auto Apply
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Payment_Received:
                        case (int)GEnum.SystemCode.Cash_Payment_Received:
                        case (int)GEnum.SystemCode.Payment_Issue:
                            if (SysOptionUtility.GetBool(GVar.SystemOption.OpID.AutoApply))
                                GlobalUI.Ctrl_Update(frm, "chkAutoApply", GEnum.CtlPropertyUpdate.Value, true);
                            else
                                GlobalUI.Ctrl_Update(frm, "chkAutoApply", GEnum.CtlPropertyUpdate.Value, false);
                            break;
                    }
                    #endregion

                    #region Set Control Visible for Issue Consignment
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Issue_Consignment:
                            GlobalUI.Ctrl_Update(frm, "btnApplyReturn", GEnum.CtlPropertyUpdate.Visible, false);
                            break;
                    }
                    #endregion

                    #region Set Control Visible for Issue Consignment
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Issue_Consignment:
                            GlobalUI.Ctrl_Update(frm, "btnApplyReturn", GEnum.CtlPropertyUpdate.Visible, false);
                            break;
                    }
                    #endregion

                    #region Set Total for AP
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Purchase_Order:
                        case (int)GEnum.SystemCode.Purchase_Delivery:
                        case (int)GEnum.SystemCode.Purchase_Invoice:
                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
                       // case (int)GEnum.SystemCode.Payment_Issue:
                            if (SECPermUtility.Perform("ItemViewCost", false) == false)
                            {
                                string[] Cons = new string[] { "DocSubTotal", "DetTabSubTotal", "DocGrand", "DocTotal", "DocHome", "DocOverallDisAmt", "DocPaidAmtF", "DocTotalAfterDis", "DocTaxTotal", "DocTaxTotalLocal" };

                                foreach (string coNm in Cons)
                                {
                                    Control[] cArr = frm.Controls.Find(coNm, true);
                                    if (cArr.Length == 0)
                                        continue;
                                    System.Windows.Forms.Control co = cArr[0];
                                    if (co != null && co.GetType() == typeof(TAUtil.TANumericEditor))
                                    {
                                        TAUtil.TANumericEditor numCon = (TAUtil.TANumericEditor)co;
                                        numCon.PasswordChar = '*';
                                    }
                                }
                            }
                            break;
                    }
                    #endregion

                    #region Disable SubTotal,GrandTotal,Home
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Cash_Adjustment:
                        case (int)GEnum.SystemCode.Sales_Adjustment:
                        case (int)GEnum.SystemCode.Purchase_Adjustment:
                            GlobalUI.Ctrl_Update(frm, "DocSubTotal", GEnum.CtlPropertyUpdate.Enabled, false);
                            GlobalUI.Ctrl_Update(frm, "DocHome", GEnum.CtlPropertyUpdate.Enabled, false);                            
                            break;

                        default:
                            GlobalUI.Ctrl_Update(frm, "DocGrand", GEnum.CtlPropertyUpdate.Enabled, false);
                            GlobalUI.Ctrl_Update(frm, "DocSubTotal", GEnum.CtlPropertyUpdate.Enabled, false);
                            GlobalUI.Ctrl_Update(frm, "DocHomeSub", GEnum.CtlPropertyUpdate.Enabled, false);
                            GlobalUI.Ctrl_Update(frm, "DocHomeTax", GEnum.CtlPropertyUpdate.Enabled, false);
                            GlobalUI.Ctrl_Update(frm, "DocHome", GEnum.CtlPropertyUpdate.Enabled, false);
                            GlobalUI.Ctrl_Update(frm, "DocTotal", GEnum.CtlPropertyUpdate.Enabled, false);
                            GlobalUI.Ctrl_Update(frm, "DocTotalAfterDis", GEnum.CtlPropertyUpdate.Enabled, false);                  
                            break;
                    }
                    #endregion

                    #region Disable DocTranGrpKey
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Quotation:
                        case (int)GEnum.SystemCode.Sales_Order:
                        case (int)GEnum.SystemCode.Reserve_Order:
                        case (int)GEnum.SystemCode.Delivery_Order:
                        case (int)GEnum.SystemCode.Sales_Invoice:
                        case (int)GEnum.SystemCode.Sales_Credit_Note:
                        case (int)GEnum.SystemCode.Sales_Debit_Note:
                        case (int)GEnum.SystemCode.Cash_Sale:
                        case (int)GEnum.SystemCode.Cash_Credit_Note:
                        case (int)GEnum.SystemCode.Cash_Debit_Note: 
                            GlobalUI.Ctrl_Update(frm, "DocTranGrpKey", GEnum.CtlPropertyUpdate.Enabled, false);
                            break;
                    }
                    #endregion

                }

                if (objDoc.IsReadOnly)
                {
                    #region Set all controls(Header/Detail) to readonly and disable most of the button
                    
                    GlobalUI.FormReadOnly_Set(frm);
                    
                    GlobalUI.Ctrl_Update(frm, "tslReadOnly", GEnum.CtlPropertyUpdate.Value, "Read Only");
                    GlobalUI.Ctrl_Update(frm, "tsbClear", GEnum.CtlPropertyUpdate.Enabled, false);
                    GlobalUI.Ctrl_Update(frm, "tsbDraft", GEnum.CtlPropertyUpdate.Enabled, false);
                    GlobalUI.Ctrl_Update(frm, "tsbSave", GEnum.CtlPropertyUpdate.Enabled, false);
                    GlobalUI.Ctrl_Update(frm, "tsbDelete", GEnum.CtlPropertyUpdate.Enabled, false);
                    GlobalUI.Ctrl_Update(frm, "tsbMarkUp", GEnum.CtlPropertyUpdate.Enabled, false);
                    GlobalUI.Ctrl_Update(frm, "tsbCreateSO", GEnum.CtlPropertyUpdate.Enabled, false);
                    GlobalUI.Ctrl_Update(frm, "tsbCreateRO", GEnum.CtlPropertyUpdate.Enabled, false);
                    if(objDoc.DocCodeKey!=(int)GEnum.SystemCode.Purchase_Order)
                        GlobalUI.Ctrl_Update(frm, "tsbCreateIV", GEnum.CtlPropertyUpdate.Enabled, false);
                    if (SysOptionUtility.DatabaseBranchCode == DBCode.ADL && objDoc.DocStatus=="Approved")    // added by KKAung on 04 Jun 2023 // Approved document allow to create PO for ADL
                        GlobalUI.Ctrl_Update(frm, "tsbCreatePO", GEnum.CtlPropertyUpdate.Enabled, true);
                    else
                        GlobalUI.Ctrl_Update(frm, "tsbCreatePO", GEnum.CtlPropertyUpdate.Enabled, false);                    
                   // GlobalUI.Ctrl_Update(frm, "btnAttachmentEdit", GEnum.CtlPropertyUpdate.Enabled, false);
                    GlobalUI.Ctrl_Update(frm, "btnSetAllDetJob", GEnum.CtlPropertyUpdate.Enabled, false);
                    GlobalUI.Ctrl_Update(frm, "btnDocDetItmVendorSelectAll", GEnum.CtlPropertyUpdate.Enabled, false);
                    GlobalUI.Ctrl_Update(frm, "btnDocDetItmVendorUnSelectAll", GEnum.CtlPropertyUpdate.Enabled, false);
                    GlobalUI.Ctrl_Update(frm, "btnGenerateItmVendor", GEnum.CtlPropertyUpdate.Enabled, false);
                    GlobalUI.Ctrl_Update(frm, "btnItmMarkReSequence", GEnum.CtlPropertyUpdate.Enabled, false);
                    GlobalUI.Ctrl_Update(frm, "btnZeroOffBalance", GEnum.CtlPropertyUpdate.Enabled, false);
                    GlobalUI.Ctrl_Update(frm, "btnSubmit", GEnum.CtlPropertyUpdate.Enabled, false);
                    GlobalUI.Ctrl_Update(frm, "btnApprove", GEnum.CtlPropertyUpdate.Enabled, false);
                    GlobalUI.Ctrl_Update(frm, "btnReject", GEnum.CtlPropertyUpdate.Enabled, false);
                    GlobalUI.Ctrl_Update(frm, "btnSend", GEnum.CtlPropertyUpdate.Enabled, false);
                    /* added by YST */
                    GlobalUI.Ctrl_Update(frm, "tslCancelling", GEnum.CtlPropertyUpdate.Value, string.Empty);
                    GlobalUI.Ctrl_Update(frm, "tsbApprove", GEnum.CtlPropertyUpdate.Enabled, true);
                    GlobalUI.Ctrl_Update(frm, "tsbRequest", GEnum.CtlPropertyUpdate.Enabled, true);
                    /* edn by YST */

                    if (objDoc.DocCodeKey==(int)GEnum.SystemCode.Quotation)
                        GlobalUI.Ctrl_Update(frm, "tsbCreateDO", GEnum.CtlPropertyUpdate.Visible, false);
                    else
                        GlobalUI.Ctrl_Update(frm, "tsbCreateDO", GEnum.CtlPropertyUpdate.Visible, true);
                   
                    //added by thettm on 29 jun 2018 (start)
                    if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Order)
                       
                        if (SysOptionUtility.DatabaseBranchCode == "BHM")
                        {
                            /* modified by YST on 2020/10/30 */
                            if (SysOptionUtility.DOManualCreation == false && dt != null)
                            {
                                DataRow[] inventorydr = dt.Select("Itmtype in (100,600,250) and ItmQty > 0 and LineType = 1000");
                                if (inventorydr.Length > 0)
                                {
                                    if (objDoc.DocTypeNm == "Direct Shipment")
                                        GlobalUI.Ctrl_Update(frm, "tsbCreateDO", GEnum.CtlPropertyUpdate.Enabled, true);
                                    else
                                        GlobalUI.Ctrl_Update(frm, "tsbCreateDO", GEnum.CtlPropertyUpdate.Enabled, false);
                                }
                                else
                                {
                                    GlobalUI.Ctrl_Update(frm, "tsbCreateDO", GEnum.CtlPropertyUpdate.Enabled, true);
                                }
                            }
                            else
                                GlobalUI.Ctrl_Update(frm, "tsbCreateDO", GEnum.CtlPropertyUpdate.Enabled, true);
                        }
                        else if (SysOptionUtility.DatabaseBranchCode.Equals("ADL") && objDoc.IsReadOnly == true)    // added by KKAung on 25 Apr 2023
                            GlobalUI.Ctrl_Update(frm, "tsbCreateDO", GEnum.CtlPropertyUpdate.Enabled, false);
                        else
                            GlobalUI.Ctrl_Update(frm, "tsbCreateDO", GEnum.CtlPropertyUpdate.Enabled, true);

                    if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Order)                    
                        if (SysOptionUtility.DatabaseBranchCode == "BHM" && ((BOLib.APPO)objDoc).DocHome > 0)
                        {
                            if (dt == null) dt = new DataTable();
                            DataRow[] inventorydt = dt.Select("Itmtype in (100,600,250)");

                            if (dt.Rows.Count == 0 || inventorydt.Length > 0)
                            {
                                if (SysOptionUtility.PDManualCreation == false)
                                {
                                    /* commented by YST on 2020-02-13 to allow PO to IV directly for Direct Shipment */
                                    //if (objDoc.DocTypeNm != "Direct Shipment")
                                        GlobalUI.Ctrl_Update(frm, "tsbCreatePD", GEnum.CtlPropertyUpdate.Enabled, false);
                                    //else
                                        //GlobalUI.Ctrl_Update(frm, "tsbCreatePD", GEnum.CtlPropertyUpdate.Enabled, true);
                                }
                                else
                                    GlobalUI.Ctrl_Update(frm, "tsbCreatePD", GEnum.CtlPropertyUpdate.Enabled, true);
                            }                                
                        }
                        else
                            GlobalUI.Ctrl_Update(frm, "tsbCreatePD", GEnum.CtlPropertyUpdate.Enabled, true);
                    //added by thettm on 29 jun 2018 (end)                    

                    if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Delivery_Order && SysOptionUtility.DatabaseBranchCode== "BHM")
                    {
                        if (((ARDO)objDoc).PrintDept == "S")
                            GlobalUI.Ctrl_Update(frm, "tsbPrint", GEnum.CtlPropertyUpdate.Enabled, SECPermUtility.Perform("ARDOSalesAllowToPrint", false));
                        else if (((ARDO)objDoc).PrintDept == "L")
                            GlobalUI.Ctrl_Update(frm, "tsbPrint", GEnum.CtlPropertyUpdate.Enabled, SECPermUtility.Perform("ARDOLogisticsAllowToPrint", false));
                        else
                            GlobalUI.Ctrl_Update(frm, "tsbPrint", GEnum.CtlPropertyUpdate.Enabled, false);

                    }

                    #region Set btnApplyIV - Enable/Disable
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Sales_Credit_Note:
                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            GlobalUI.Ctrl_Update(frm, "btnApplyIV", GEnum.CtlPropertyUpdate.Enabled, false);
                            break;
                        case (int)GEnum.SystemCode.Payment_Received:
                        case (int)GEnum.SystemCode.Cash_Payment_Received:
                        case (int)GEnum.SystemCode.Payment_Issue:
                            if ((int)GFunc.GetPropertyValue("DocCurrKey", objDoc) == 1 && !objDoc.IsReadOnly)
                                GlobalUI.Ctrl_Update(frm, "btnMonth", GEnum.CtlPropertyUpdate.Enabled, true);
                            else
                                GlobalUI.Ctrl_Update(frm, "btnMonth", GEnum.CtlPropertyUpdate.Enabled, false);

                            GlobalUI.Ctrl_Update(frm, "btnDueSummary", GEnum.CtlPropertyUpdate.Enabled, true);
                            break;
                    }
                    #endregion

                    #region Setting for Approved/Request/Cancel button

                    //added by May on 30-Oct-2024
                    if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Payment_Issue || objDoc.DocCodeKey == (int)GEnum.SystemCode.GL_Payment
                         || objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Adjustment )
                    {
                        GlobalUI.Ctrl_Update(frm, "DocStatus", GEnum.CtlPropertyUpdate.Readonly, true);                        
                        
                        GlobalUI.Ctrl_Update(frm, "tsbModify", GEnum.CtlPropertyUpdate.Visible, GFunc.NEStr(objDoc.DocStatus,"").Equals("Approved"));


                        if (!(GFunc.NEStr(objDoc.DocStatus,"").Equals("Posted") && SECPermUtility.Perform("PaymentApprovalRequest", false)))
                            GlobalUI.Ctrl_Update(frm, "tsbRequest", GEnum.CtlPropertyUpdate.Visible, false);
                        else
                            GlobalUI.Ctrl_Update(frm, "tsbRequest", GEnum.CtlPropertyUpdate.Visible, true);

                        //if (!(objDoc.DocStatus.Contains("Verif") && SECPermUtility.Perform("PaymentVerify", false)))
                        //    GlobalUI.Ctrl_Update(frm, "tsbVerify", GEnum.CtlPropertyUpdate.Visible, false);
                        //else
                        //    GlobalUI.Ctrl_Update(frm, "tsbVerify", GEnum.CtlPropertyUpdate.Visible, true);

                        if (!GFunc.NEStr(objDoc.DocStatus,"").Contains("Pending for Approv") || !SECPermUtility.Perform("PaymentApproval", false))
                            GlobalUI.Ctrl_Update(frm, "tsbApprove", GEnum.CtlPropertyUpdate.Visible, false);
                        else
                            GlobalUI.Ctrl_Update(frm, "tsbApprove", GEnum.CtlPropertyUpdate.Visible, true);

                        if (GFunc.NEStr(objDoc.DocStatus, "").Contains("Pending for Approv") && SysOptionUtility.DatabaseBranchCode=="DMR")
                            GlobalUI.Ctrl_Update(frm, "tspReject", GEnum.CtlPropertyUpdate.Visible, true);
                        else
                            GlobalUI.Ctrl_Update(frm, "tspReject", GEnum.CtlPropertyUpdate.Visible, false);


                        //if (SysOptionUtility.DatabaseBranchCode.StartsWith("BHM"))
                        //{

                        //}
                        //else
                        //{
                        //    GlobalUI.Ctrl_Update(frm, "tsbApprove", GEnum.CtlPropertyUpdate.Visible, false);
                        //    GlobalUI.Ctrl_Update(frm, "tsbRequest", GEnum.CtlPropertyUpdate.Visible, false);
                        //    GlobalUI.Ctrl_Update(frm, "tsbVerify", GEnum.CtlPropertyUpdate.Visible, false);
                        //}
                    }
                    else
                    {
                        /* added by YST on 2023/06/13 */
                        if (objDoc.DisapproveMsg != null)
                        {
                            GlobalUI.Ctrl_Update(frm, "tslCancelling", GEnum.CtlPropertyUpdate.Value, objDoc.DisapproveMsg);
                            IsPendingCancel = true;
                        }

                        if (SysOptionUtility.DatabaseBranchCode != DBCode.BHM)
                        {
                            Boolean IsLock = false;
                            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                            {
                                cn.Open();
                                IsLock = SysLockUtility.IsLock(cn, false, GEnum.SysLockOption.ByCodeKeyandDataKey, (GEnum.SystemCode)objDoc.DocCodeKey, objDoc.DocKey, 0, objDoc.GUID);
                                cn.Close();
                            }
                            if (IsLock)
                            {
                                GlobalUI.Ctrl_Update(frm, "tsbDelete", GEnum.CtlPropertyUpdate.Enabled, false);
                                GlobalUI.Ctrl_Update(frm, "tsbApprove", GEnum.CtlPropertyUpdate.Enabled, false);
                                GlobalUI.Ctrl_Update(frm, "tsbRequest", GEnum.CtlPropertyUpdate.Enabled, false);

                            }
                            else
                            {
                                GlobalUI.Ctrl_Update(frm, "tsbDelete", GEnum.CtlPropertyUpdate.Enabled, (objDoc.DisapproveUserKey > 0 && SECPermUtility.Delete(permID, false)));
                                GlobalUI.Ctrl_Update(frm, "tsbApprove", GEnum.CtlPropertyUpdate.Enabled, objDoc.ApprovalStatus.Length == 0);
                                GlobalUI.Ctrl_Update(frm, "tsbRequest", GEnum.CtlPropertyUpdate.Enabled, objDoc.ApprovalStatus.Length == 0);

                            }
                            GlobalUI.Ctrl_Update(frm, "tsbApprove", GEnum.CtlPropertyUpdate.Visible, (IsPendingCancel == false && SECPermUtility.Perform("SalesDocumentApproval", false)));
                            GlobalUI.Ctrl_Update(frm, "tsbRequest", GEnum.CtlPropertyUpdate.Visible, (IsPendingCancel == false && SECPermUtility.Perform("SalesDocumentRequestCancel", false)));

                        }

                        else
                        {
                            GlobalUI.Ctrl_Update(frm, "tsbDelete", GEnum.CtlPropertyUpdate.Enabled, (objDoc.DisapproveUserKey > 0 && SECPermUtility.Delete(permID, false)));
                            GlobalUI.Ctrl_Update(frm, "tsbApprove", GEnum.CtlPropertyUpdate.Enabled, objDoc.ApprovalStatus.Length == 0);
                            GlobalUI.Ctrl_Update(frm, "tsbRequest", GEnum.CtlPropertyUpdate.Enabled, objDoc.ApprovalStatus.Length == 0);
                            GlobalUI.Ctrl_Update(frm, "tsbApprove", GEnum.CtlPropertyUpdate.Visible, (IsPendingCancel == false && SECPermUtility.Perform("SalesDocumentApproval", false)));
                            GlobalUI.Ctrl_Update(frm, "tsbRequest", GEnum.CtlPropertyUpdate.Visible, (IsPendingCancel == false && SECPermUtility.Perform("SalesDocumentRequestCancel", false)));
                        }
                        //GlobalUI.Ctrl_Update(frm, "tsbDelete", GEnum.CtlPropertyUpdate.Enabled, (objDoc.DisapproveUserKey > 0 && SECPermUtility.Delete(permID, false)));
                        //GlobalUI.Ctrl_Update(frm, "tsbApprove", GEnum.CtlPropertyUpdate.Enabled, objDoc.ApprovalStatus.Length == 0);
                        //GlobalUI.Ctrl_Update(frm, "tsbRequest", GEnum.CtlPropertyUpdate.Enabled, objDoc.ApprovalStatus.Length == 0);
                        //GlobalUI.Ctrl_Update(frm, "tsbApprove", GEnum.CtlPropertyUpdate.Visible, (IsPendingCancel == false && SECPermUtility.Perform("SalesDocumentApproval", false)));
                        //GlobalUI.Ctrl_Update(frm, "tsbRequest", GEnum.CtlPropertyUpdate.Visible, (IsPendingCancel == false && SECPermUtility.Perform("SalesDocumentRequestCancel", false)));
                    }
                    #endregion

                    #endregion
                    return;
                }
                else
                {
                    GlobalUI.FormReadOnlyClean_Set(frm);

                    #region Set DocCountryRate, DocTaxTotal, DoctaxTotalLocal - Visibility/Enable

                    if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Delivery_Order)
                    {
                        if(((ARDO)objDoc).PrintDept=="S")
                            GlobalUI.Ctrl_Update(frm, "tsbPrint", GEnum.CtlPropertyUpdate.Enabled, SECPermUtility.Perform("ARDOSalesAllowToPrint", false));
                        else if (((ARDO)objDoc).PrintDept == "L")
                            GlobalUI.Ctrl_Update(frm, "tsbPrint", GEnum.CtlPropertyUpdate.Enabled, SECPermUtility.Perform("ARDOLogisticsAllowToPrint", false));
                        else
                            GlobalUI.Ctrl_Update(frm, "tsbPrint", GEnum.CtlPropertyUpdate.Enabled, false);

                    }

                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Sales_Adjustment:
                        case (int)GEnum.SystemCode.Cash_Adjustment:
                        case (int)GEnum.SystemCode.Contra:
                        case (int)GEnum.SystemCode.Cash_Contra:
                        case (int)GEnum.SystemCode.Purchase_Adjustment:
                        case (int)GEnum.SystemCode.Issue_Consignment:
                        case (int)GEnum.SystemCode.Return_Consignment:
                        case (int)GEnum.SystemCode.Order_Consignment:
                        case (int)GEnum.SystemCode.Received_Consignment:
                        case (int)GEnum.SystemCode.Consignment_Settlement:
                        case (int)GEnum.SystemCode.Deposit:
                        case (int)GEnum.SystemCode.Bank_Revaluation:
                        case (int)GEnum.SystemCode.Purchase_Delivery:
                            GlobalUI.Ctrl_Update(frm, "DocCountryRate", GEnum.CtlPropertyUpdate.Visible, false);
                            GlobalUI.Ctrl_Update(frm, "DocCountryRateLabel", GEnum.CtlPropertyUpdate.Visible, false);
                            break;

                        case (int)GEnum.SystemCode.Quotation:
                        case (int)GEnum.SystemCode.Sales_Order:
                        case (int)GEnum.SystemCode.Reserve_Order:
                        case (int)GEnum.SystemCode.Delivery_Order:
                        case (int)GEnum.SystemCode.Sales_Invoice:
                        case (int)GEnum.SystemCode.Sales_Debit_Note:
                        case (int)GEnum.SystemCode.Sales_Credit_Note:
                        case (int)GEnum.SystemCode.Payment_Received:
                        case (int)GEnum.SystemCode.Cash_Sale:
                        case (int)GEnum.SystemCode.Cash_Debit_Note:
                        case (int)GEnum.SystemCode.Cash_Credit_Note:
                        case (int)GEnum.SystemCode.Cash_Payment_Received:
                            docCurrKey = (int)GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
                            if (SysOptionUtility.CountryCurrency == 1)
                            {
                                GlobalUI.Ctrl_Update(frm, "DocCountryRate", GEnum.CtlPropertyUpdate.Enabled, false);
                            }
                            else
                            {
                                if (docCurrKey == SysOptionUtility.CountryCurrency)
                                {
                                    GlobalUI.Ctrl_Update(frm, "DocCountryRate", GEnum.CtlPropertyUpdate.Enabled, false);
                                }
                                else
                                {
                                    GlobalUI.Ctrl_Update(frm, "DocCountryRate", GEnum.CtlPropertyUpdate.Enabled, true);
                                }

                            }
                            break;

                        case (int)GEnum.SystemCode.Purchase_Order:
                        case (int)GEnum.SystemCode.Purchase_Invoice:
                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        case (int)GEnum.SystemCode.Payment_Issue:
                            docCurrKey = (int)GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
                            docTaxGrpKey = GFunc.NEInt(GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc), 0);
                            REFTaxGrp TaxgrpObj = REFTaxGrp.Get(docTaxGrpKey);
                            if (TaxgrpObj.TaxGrpKey != null)
                                IsCustomGST = (bool)TaxgrpObj.GSTCustom;

                            if (SysOptionUtility.CountryCurrency == 1)
                            {
                                GlobalUI.Ctrl_Update(frm, "DocCountryRate", GEnum.CtlPropertyUpdate.Enabled, false);
                            }
                            else
                            {
                                if (IsCustomGST || docCurrKey == SysOptionUtility.CountryCurrency)
                                {
                                    GlobalUI.Ctrl_Update(frm, "DocCountryRate", GEnum.CtlPropertyUpdate.Enabled, false);
                                }
                                else
                                {
                                    GlobalUI.Ctrl_Update(frm, "DocCountryRate", GEnum.CtlPropertyUpdate.Enabled, true);
                                }
                            }

                            //if (IsCustomGST)
                            //{
                            //    GlobalUI.Ctrl_Update(frm, "DocTaxTotal", GEnum.CtlPropertyUpdate.Enabled, false);
                            //    GlobalUI.Ctrl_Update(frm, "DocTaxTotalLocal", GEnum.CtlPropertyUpdate.Enabled, true);
                            //}
                            //else
                            //{
                                GlobalUI.Ctrl_Update(frm, "DocTaxTotal", GEnum.CtlPropertyUpdate.Enabled, true);
                                GlobalUI.Ctrl_Update(frm, "DocTaxTotalLocal", GEnum.CtlPropertyUpdate.Enabled, false);
                            //}
                            break;
                    }
                    #endregion

                    #region Set Button - Enable/Disable base on IsNew, permID(delete) condition

                    GlobalUI.Ctrl_Update(frm, "tslReadOnly", GEnum.CtlPropertyUpdate.Value, string.Empty);
                    GlobalUI.Ctrl_Update(frm, "tsbDraft", GEnum.CtlPropertyUpdate.Enabled, true);
                    GlobalUI.Ctrl_Update(frm, "tsbSave", GEnum.CtlPropertyUpdate.Enabled, true);
                    GlobalUI.Ctrl_Update(frm, "tsbCreateSO", GEnum.CtlPropertyUpdate.Enabled, true);
                    GlobalUI.Ctrl_Update(frm, "tsbCreateRO", GEnum.CtlPropertyUpdate.Enabled, true);
                    GlobalUI.Ctrl_Update(frm, "tsbCreateIV", GEnum.CtlPropertyUpdate.Enabled, true);
                    GlobalUI.Ctrl_Update(frm, "tsbCreatePO", GEnum.CtlPropertyUpdate.Enabled, true);                    
                    GlobalUI.Ctrl_Update(frm, "btnAttachmentEdit", GEnum.CtlPropertyUpdate.Enabled, true);
                    GlobalUI.Ctrl_Update(frm, "btnSetAllDetJob", GEnum.CtlPropertyUpdate.Enabled, true);
                    GlobalUI.Ctrl_Update(frm, "btnDocDetItmVendorSelectAll", GEnum.CtlPropertyUpdate.Enabled, true);
                    GlobalUI.Ctrl_Update(frm, "btnDocDetItmVendorUnSelectAll", GEnum.CtlPropertyUpdate.Enabled, true);
                    GlobalUI.Ctrl_Update(frm, "btnGenerateItmVendor", GEnum.CtlPropertyUpdate.Enabled, true);
                    GlobalUI.Ctrl_Update(frm, "btnItmMarkReSequence", GEnum.CtlPropertyUpdate.Enabled, true);
                    GlobalUI.Ctrl_Update(frm, "btnZeroOffBalance", GEnum.CtlPropertyUpdate.Enabled, true);
                    GlobalUI.Ctrl_Update(frm, "btnSubmit", GEnum.CtlPropertyUpdate.Enabled, true);
                    GlobalUI.Ctrl_Update(frm, "btnApprove", GEnum.CtlPropertyUpdate.Enabled, true);
                    GlobalUI.Ctrl_Update(frm, "btnReject", GEnum.CtlPropertyUpdate.Enabled, true);
                    GlobalUI.Ctrl_Update(frm, "btnSend", GEnum.CtlPropertyUpdate.Enabled, true);
                    /* added by YST */
                    GlobalUI.Ctrl_Update(frm, "tslCancelling", GEnum.CtlPropertyUpdate.Value, string.Empty);
                    GlobalUI.Ctrl_Update(frm, "tsbApprove", GEnum.CtlPropertyUpdate.Enabled, true);
                    GlobalUI.Ctrl_Update(frm, "tsbRequest", GEnum.CtlPropertyUpdate.Enabled, true);
                    /* edn by YST */

                    //added by thettm on 29 jun 2018(start)
                    if (dt != null && objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Order)                                            
                        if (SysOptionUtility.DatabaseBranchCode == "BHM" && ((BOLib.ARSO)objDoc).DocHome > 0)
                        {                            
                            DataRow[] inventorydt = dt.Select("Itmtype in (100,600,250)");

                            if (dt.Rows.Count == 0 || inventorydt.Length > 0)
                            {
                                if (SysOptionUtility.DOManualCreation == false)
                                {
                                    if (objDoc.DocTypeNm != "Direct Shipment")
                                        GlobalUI.Ctrl_Update(frm, "tsbCreateDO", GEnum.CtlPropertyUpdate.Enabled, false);
                                    else
                                        GlobalUI.Ctrl_Update(frm, "tsbCreateDO", GEnum.CtlPropertyUpdate.Enabled, true);
                                }
                                else
                                    GlobalUI.Ctrl_Update(frm, "tsbCreateDO", GEnum.CtlPropertyUpdate.Enabled, true);
                            }

                        }
                        else
                            GlobalUI.Ctrl_Update(frm, "tsbCreateDO", GEnum.CtlPropertyUpdate.Enabled, true);
                   
                    if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Order && dt != null)
                   
                        if (SysOptionUtility.DatabaseBranchCode == "BHM" && ((BOLib.APPO)objDoc).DocHome > 0)
                        {
                            
                            DataRow[] inventorydt = dt.Select("Itmtype in (100,600,250)");

                            if (dt.Rows.Count == 0 || inventorydt.Length > 0)
                            {
                                if (SysOptionUtility.PDManualCreation == false)
                                {
                                    /* commented by YST on 2020-02-13 to allow PO to IV directly for Direct Shipment */
                                    //if (objDoc.DocTypeNm != "Direct Shipment")
                                    GlobalUI.Ctrl_Update(frm, "tsbCreatePD", GEnum.CtlPropertyUpdate.Enabled, false);
                                    //else
                                        //GlobalUI.Ctrl_Update(frm, "tsbCreatePD", GEnum.CtlPropertyUpdate.Enabled, true);
                                }
                                else
                                    GlobalUI.Ctrl_Update(frm, "tsbCreatePD", GEnum.CtlPropertyUpdate.Enabled, true);
                            }
                        }
                        else
                            GlobalUI.Ctrl_Update(frm, "tsbCreatePD", GEnum.CtlPropertyUpdate.Enabled, true);
                    
                    //added by thettm on 29 jun 2018(end)                    
                     
                    if (objDoc.IsNew)
                    {
                        GlobalUI.Ctrl_Update(frm, "tsbClear", GEnum.CtlPropertyUpdate.Enabled, true);
                        GlobalUI.Ctrl_Update(frm, "tsbDelete", GEnum.CtlPropertyUpdate.Enabled, false);
                    }
                    else
                    {
                        GlobalUI.Ctrl_Update(frm, "tsbClear", GEnum.CtlPropertyUpdate.Enabled, false);
                        if (SECPermUtility.Delete(permID, false))
                            GlobalUI.Ctrl_Update(frm, "tsbDelete", GEnum.CtlPropertyUpdate.Enabled, true);
                        else
                            GlobalUI.Ctrl_Update(frm, "tsbDelete", GEnum.CtlPropertyUpdate.Enabled, false);
                    }
                    #endregion

                    #region Set Controls enable/disable base on other conditions
                                      
                    #region  /* commented by YST to use DocApprovalRequired_Get() funtion instead of SysOptionUtility.GetInt() function
                    switch (objDoc.DocCodeKey)
                    {
                        //ttm                       
                        //case (int)GEnum.SystemCode.Quotation:
                        //    if (SysOptionUtility.GetInt("DocApproveForARQO") <= (int)GEnum.ApprovalOpiton.None)
                        //        draftEnable = false;
                        //    break;
                        //ttm

                        /* commented by YST to use DocApprovalRequired_Get() funtion instead of SysOptionUtility.GetInt() function
                         case (int)GEnum.SystemCode.Sales_Order:
                             if (SysOptionUtility.GetInt("DocApproveForARSO") <= (int)GEnum.ApprovalOpiton.None)
                                 draftEnable = false;
                             if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM) // for WMS
                                 draftEnable = true;
                             break;
                         case (int)GEnum.SystemCode.Purchase_Order:
                             if (SysOptionUtility.GetInt("DocApproveForAPPO") <= (int)GEnum.ApprovalOpiton.None)
                                 draftEnable = false;
                             if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM) // for WMS
                                 draftEnable = true;
                             break;
                         //QO & RO are added by YST to follow SO workflow that allows to save as Draft or disable Draft button according to DocApproveForARQO/ARRO option for all subsidiaries
                         case (int)GEnum.SystemCode.Quotation:
                             if (SysOptionUtility.GetInt("DocApproveForARQO") <= (int)GEnum.ApprovalOpiton.None)
                                 draftEnable = false;
                             break;
                         case (int)GEnum.SystemCode.Reserve_Order:
                             if (SysOptionUtility.GetInt("DocApproveForARRO") <= (int)GEnum.ApprovalOpiton.None)
                                 draftEnable = false;
                             break;  
                          */
                    }
                    #endregion

                    /* added by YST */
                    draftEnable = DocUtility.DocApprovalRequired_Get((int)objDoc.DocCodeKey, ref ntApprovalRequired);
                    if (objDoc.DocTypeNm.ToLower().Contains("non-trade")) draftEnable = ntApprovalRequired;
                    if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM) // for WMS
                    {
                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Order || 
                            objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Order)
                            draftEnable = true;                          
                    }
                    GlobalUI.Ctrl_Update(frm, "tsbDraft", GEnum.CtlPropertyUpdate.Enabled, draftEnable);
                    GlobalUI.Ctrl_Update(frm, "btnApprove", GEnum.CtlPropertyUpdate.Enabled, draftEnable);
                    GlobalUI.Ctrl_Update(frm, "btnReject", GEnum.CtlPropertyUpdate.Enabled, draftEnable);
                    GlobalUI.Ctrl_Update(frm, "btnSubmit", GEnum.CtlPropertyUpdate.Enabled, draftEnable);
                /* end */

                /* commented because Approval tab is not using anymore
                //ttm
                if ((int)GEnum.SystemCode.Quotation == objDoc.DocCodeKey)
                    if (objDoc.DocID.ToString() != "")
                    {
                        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                        {
                            cn.Open();

                            //if (objDoc.DocEmKey != 0) 
                            if(!GFunc.IsNEZ(objDoc.DocEmKey)) //Modified by May
                            {
                                if (GFunc.NEInt(GFunc.ExecuteScalar(cn, "select Top 1 UserKey from MST_SalesRep(nolock) where EmKey=" + objDoc.DocEmKey).ToString(), 0) == AppInfor.CurrentUserKey)
                                    GlobalUI.Ctrl_Update(frm, "btnSubmit", GEnum.CtlPropertyUpdate.Enabled, true);
                                else
                                    GlobalUI.Ctrl_Update(frm, "btnSubmit", GEnum.CtlPropertyUpdate.Enabled, false);
                            }
                            else
                                GlobalUI.Ctrl_Update(frm, "btnSubmit", GEnum.CtlPropertyUpdate.Enabled, false);

                            int result = int.Parse(GFunc.ExecuteScalar(cn, "if exists(select 1 from ApprovalInformation(nolock) where UserKey = '" + AppInfor.CurrentUserKey.ToString() +
                                                    "' and QuoDocID = '" + objDoc.DocID.ToString() + "') select 1 else select 0").ToString());

                            if (result == 1)
                            {
                                GlobalUI.Ctrl_Update(frm, "btnApprove", GEnum.CtlPropertyUpdate.Enabled, true);
                                GlobalUI.Ctrl_Update(frm, "btnReject", GEnum.CtlPropertyUpdate.Enabled, true);
                                GlobalUI.Ctrl_Update(frm, "DisapproveMsg", GEnum.CtlPropertyUpdate.Enabled, true);

                                if (objDoc.DocEmKey != SysOptionUtility.GetInt("DefaultFinalAproverForQuotation"))
                                    GlobalUI.Ctrl_Update(frm, "btnSubmit", GEnum.CtlPropertyUpdate.Enabled, true);
                            }
                            else
                            {
                                GlobalUI.Ctrl_Update(frm, "btnApprove", GEnum.CtlPropertyUpdate.Enabled, false);
                                GlobalUI.Ctrl_Update(frm, "btnReject", GEnum.CtlPropertyUpdate.Enabled, false);
                                GlobalUI.Ctrl_Update(frm, "DisapproveMsg", GEnum.CtlPropertyUpdate.Enabled, false);
                            }
                        }
                    }
                    else
                    {
                        GlobalUI.Ctrl_Update(frm, "btnApprove", GEnum.CtlPropertyUpdate.Enabled, false);
                        GlobalUI.Ctrl_Update(frm, "btnReject", GEnum.CtlPropertyUpdate.Enabled, false);
                        GlobalUI.Ctrl_Update(frm, "DisapproveMsg", GEnum.CtlPropertyUpdate.Enabled, false);
                        GlobalUI.Ctrl_Update(frm, "btnSubmit", GEnum.CtlPropertyUpdate.Enabled, false);
                    }

                //ttm
                */

                    #region Setting for Draft/Approved/Request button
                /* added by YST on 2022/07/08 */
                if (objDoc.DocState == (int)GEnum.DocState.New || objDoc.DocState == (int)GEnum.DocState.Draft)
                    {                       
                        //GlobalUI.Ctrl_Update(frm, "tsbDraft", GEnum.CtlPropertyUpdate.Enabled, true);
                        GlobalUI.Ctrl_Update(frm, "tsbApprove", GEnum.CtlPropertyUpdate.Enabled, false);
                        GlobalUI.Ctrl_Update(frm, "tsbRequest", GEnum.CtlPropertyUpdate.Enabled, false);
                    }
                    else
                    {
                        //added by May on 30-Oct-2024
                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Payment_Issue || objDoc.DocCodeKey == (int)GEnum.SystemCode.GL_Payment || objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Adjustment)
                        {
                            GlobalUI.Ctrl_Update(frm, "DocStatus", GEnum.CtlPropertyUpdate.Readonly, true);
                            GlobalUI.Ctrl_Update(frm, "tsbModify", GEnum.CtlPropertyUpdate.Visible, objDoc.DocStatus.Equals("Approved"));

                            if (!(objDoc.DocStatus.Equals("Posted") && SECPermUtility.Perform("PaymentApprovalRequest", false)))
                                GlobalUI.Ctrl_Update(frm, "tsbRequest", GEnum.CtlPropertyUpdate.Visible, false);
                            else
                                GlobalUI.Ctrl_Update(frm, "tsbRequest", GEnum.CtlPropertyUpdate.Visible, true);

                            //if (!(objDoc.DocStatus.Contains("Verif") && SECPermUtility.Perform("PaymentVerify", false)))
                            //    GlobalUI.Ctrl_Update(frm, "tsbVerify", GEnum.CtlPropertyUpdate.Visible, false);
                            //else
                            //    GlobalUI.Ctrl_Update(frm, "tsbVerify", GEnum.CtlPropertyUpdate.Visible, true);

                            if (!objDoc.DocStatus.Contains("Pending for Approv") || !SECPermUtility.Perform("PaymentApproval", false))
                                GlobalUI.Ctrl_Update(frm, "tsbApprove", GEnum.CtlPropertyUpdate.Visible, false);
                            else
                                GlobalUI.Ctrl_Update(frm, "tsbApprove", GEnum.CtlPropertyUpdate.Visible, true);

                            //if (SysOptionUtility.DatabaseBranchCode.StartsWith("BHM"))
                            //{
                                
                            //}
                            //else
                            //{
                            //    GlobalUI.Ctrl_Update(frm, "tsbApprove", GEnum.CtlPropertyUpdate.Visible, false);
                            //    GlobalUI.Ctrl_Update(frm, "tsbRequest", GEnum.CtlPropertyUpdate.Visible, false);
                            //    GlobalUI.Ctrl_Update(frm, "tsbVerify", GEnum.CtlPropertyUpdate.Visible, false);
                            //}
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(objDoc.DisapproveMsg))
                            {
                                GlobalUI.Ctrl_Update(frm, "tslCancelling", GEnum.CtlPropertyUpdate.Value, objDoc.DisapproveMsg);
                                IsPendingCancel = true;
                            }

                            GlobalUI.Ctrl_Update(frm, "tsbDraft", GEnum.CtlPropertyUpdate.Enabled, false);
                            GlobalUI.Ctrl_Update(frm, "tsbApprove", GEnum.CtlPropertyUpdate.Visible, (IsPendingCancel == false && SECPermUtility.Perform("SalesDocumentApproval", false)));
                            GlobalUI.Ctrl_Update(frm, "tsbRequest", GEnum.CtlPropertyUpdate.Visible, (IsPendingCancel == false && SECPermUtility.Perform("SalesDocumentRequestCancel", false)));
                        }
                    }

                    #endregion

                    #region Setting for DocCurrRate
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                        case (int)GEnum.SystemCode.Packing_List:
                        case (int)GEnum.SystemCode.Purchase_Plan:
                        case (int)GEnum.SystemCode.Purchase_Request:
                        case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                        case (int)GEnum.SystemCode.Inventory_Adjustment:
                        case (int)GEnum.SystemCode.Inventory_Production:
                        case (int)GEnum.SystemCode.Inventory_Transfer:
                        case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                        case (int)GEnum.SystemCode.Purchase_Requisition:
                        case (int)GEnum.SystemCode.Journal:
                            break;

                        case (int)GEnum.SystemCode.Contra:
                        case (int)GEnum.SystemCode.Cash_Contra:
                            if (GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 0) == 1)
                            {
                                GlobalUI.Ctrl_Update(frm, "DocCurrRate", GEnum.CtlPropertyUpdate.Enabled, false);
                            }
                            else
                            {
                                GlobalUI.Ctrl_Update(frm, "DocCurrRate", GEnum.CtlPropertyUpdate.Enabled, true);
                            }
                            break;

                        case (int)GEnum.SystemCode.Payment_Received:
                        case (int)GEnum.SystemCode.Cash_Payment_Received:
                        case (int)GEnum.SystemCode.Payment_Issue:
                            if (GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 0) == 1)
                                GlobalUI.Ctrl_Update(frm, "DocCurrRate", GEnum.CtlPropertyUpdate.Enabled, false);
                            else
                                GlobalUI.Ctrl_Update(frm, "DocCurrRate", GEnum.CtlPropertyUpdate.Enabled, true);

                            if (!GFunc.IsNEZ(GFunc.GetPropertyValue("DocAccKey", objDoc)))
                            {
                                MSTAcc obj = MSTAcc.Get(GFunc.GetIntPropertyValue("DocAccKey", objDoc));
                                if (obj.AccCurrKey == 1)
                                    GlobalUI.Ctrl_Update(frm, "DocCurrKey", GEnum.CtlPropertyUpdate.Enabled, true);
                                else
                                    GlobalUI.Ctrl_Update(frm, "DocCurrKey", GEnum.CtlPropertyUpdate.Enabled, false);
                            }
                            else
                                GlobalUI.Ctrl_Update(frm, "DocCurrKey", GEnum.CtlPropertyUpdate.Enabled, true);
                            break;

                        case (int)GEnum.SystemCode.Cash_Adjustment:
                        case (int)GEnum.SystemCode.Sales_Adjustment:
                        case (int)GEnum.SystemCode.Purchase_Adjustment:
                            if (GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 0) == 1)
                                GlobalUI.Ctrl_Update(frm, "DocCurrRate", GEnum.CtlPropertyUpdate.Enabled, false);
                            else
                                GlobalUI.Ctrl_Update(frm, "DocCurrRate", GEnum.CtlPropertyUpdate.Enabled, true);

                            if (!GFunc.IsNEZ(GFunc.GetPropertyValue("DocAccGLKey", objDoc)))
                            {
                                MSTAcc obj = MSTAcc.Get(GFunc.GetIntPropertyValue("DocAccGLKey", objDoc));
                                if (obj.AccCurrKey == 1)
                                    GlobalUI.Ctrl_Update(frm, "DocCurrKey", GEnum.CtlPropertyUpdate.Enabled, true);
                                else
                                    GlobalUI.Ctrl_Update(frm, "DocCurrKey", GEnum.CtlPropertyUpdate.Enabled, false);
                            }
                            else
                                GlobalUI.Ctrl_Update(frm, "DocCurrKey", GEnum.CtlPropertyUpdate.Enabled, true);
                            break;

                        default:
                            if (GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 0) == 1)
                                GlobalUI.Ctrl_Update(frm, "DocCurrRate", GEnum.CtlPropertyUpdate.Enabled, false);
                            else
                                GlobalUI.Ctrl_Update(frm, "DocCurrRate", GEnum.CtlPropertyUpdate.Enabled, true);
                            break;
                    }
                    #endregion

                    #region Setting for DocTaxTotal
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                        case (int)GEnum.SystemCode.Packing_List:
                        case (int)GEnum.SystemCode.Sales_Adjustment:
                        case (int)GEnum.SystemCode.Contra:
                        case (int)GEnum.SystemCode.Cash_Adjustment:
                        case (int)GEnum.SystemCode.Cash_Contra:
                        case (int)GEnum.SystemCode.Purchase_Plan:
                        case (int)GEnum.SystemCode.Purchase_Request:
                        case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                        case (int)GEnum.SystemCode.Purchase_Delivery:
                        case (int)GEnum.SystemCode.Purchase_Adjustment:
                        case (int)GEnum.SystemCode.Inventory_Adjustment:
                        case (int)GEnum.SystemCode.Inventory_Production:
                        case (int)GEnum.SystemCode.Inventory_Transfer:
                        case (int)GEnum.SystemCode.Issue_Consignment:
                        case (int)GEnum.SystemCode.Return_Consignment:
                        case (int)GEnum.SystemCode.Order_Consignment:
                        case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                        case (int)GEnum.SystemCode.Received_Consignment:
                        case (int)GEnum.SystemCode.Consignment_Settlement:
                        case (int)GEnum.SystemCode.Journal:
                        case (int)GEnum.SystemCode.Deposit:
                        case (int)GEnum.SystemCode.Bank_Revaluation:

                        case (int)GEnum.SystemCode.Purchase_Order:
                        case (int)GEnum.SystemCode.Purchase_Invoice:
                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        case (int)GEnum.SystemCode.Payment_Issue:

                        case (int)GEnum.SystemCode.Sales_Invoice:
                        case (int)GEnum.SystemCode.Sales_Credit_Note:
                        case (int)GEnum.SystemCode.Sales_Debit_Note:
                            break;

                        default:
                            GlobalUI.Ctrl_Update(frm, "DocTaxTotal", GEnum.CtlPropertyUpdate.Enabled, false);
                            break;
                    }
                    #endregion

                    #region Setting for DocTypeNm
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Purchase_Adjustment:
                            if (objDoc.DocState == (int)GEnum.DocState.Posted)
                                GlobalUI.Ctrl_Update(frm, "DocTypeNm", GEnum.CtlPropertyUpdate.Enabled, false);
                            break;
                    }
                    #endregion
                    #endregion

                    #region Set ARPY/APPY (AutoApply, Month)
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Payment_Received:
                        case (int)GEnum.SystemCode.Cash_Payment_Received:
                        case (int)GEnum.SystemCode.Payment_Issue:
                            if ((int)GFunc.GetPropertyValue("DocCurrKey", objDoc) == 1 && !objDoc.IsReadOnly)
                                GlobalUI.Ctrl_Update(frm, "btnMonth", GEnum.CtlPropertyUpdate.Enabled, true);
                            else
                                GlobalUI.Ctrl_Update(frm, "btnMonth", GEnum.CtlPropertyUpdate.Enabled, false);

                            GlobalUI.Ctrl_Update(frm, "btnDueSummary", GEnum.CtlPropertyUpdate.Enabled, true);

                            if (objDoc.DocType == 300 || objDoc.DocType == 310)//GST Claim or GST Payment
                            {
                                GlobalUI.Ctrl_Update(frm, "DocSubTotal", GEnum.CtlPropertyUpdate.Enabled, false);
                                GlobalUI.Ctrl_Update(frm, "DocTaxGrpKey", GEnum.CtlPropertyUpdate.Enabled, false);

                            }
                            else if (objDoc.DocType == 320) //Custom Import Tax
                            {
                                GlobalUI.Ctrl_Update(frm, "DocSubTotal", GEnum.CtlPropertyUpdate.Enabled, true);
                                GlobalUI.Ctrl_Update(frm, "DocTaxGrpKey", GEnum.CtlPropertyUpdate.Enabled, true);
                            }
                            else
                            {
                                GlobalUI.Ctrl_Update(frm, "DocSubTotal", GEnum.CtlPropertyUpdate.Enabled, false);
                                GlobalUI.Ctrl_Update(frm, "DocTaxGrpKey", GEnum.CtlPropertyUpdate.Enabled, true);
                            }
                            break;
                    }
                    #endregion

                    #region Set SO/PO/CO disable DocType by conditions
                    //added by thetm on 08 jun 2018(start)
                    if(iscash) GlobalUI.Ctrl_Update(frm, "DocTypeNm", GEnum.CtlPropertyUpdate.Enabled, false);
                    else
                    //added by thetm on 08 jun 2018(end)
                        switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Sales_Order:
                        case (int)GEnum.SystemCode.Reserve_Order:
                        case (int)GEnum.SystemCode.Purchase_Order:
                        case (int)GEnum.SystemCode.Order_Consignment:
                            if (objDoc.DocState == (int)GEnum.DocState.Posted)
                                GlobalUI.Ctrl_Update(frm, "DocTypeNm", GEnum.CtlPropertyUpdate.Enabled, false);
                            else
                                GlobalUI.Ctrl_Update(frm, "DocTypeNm", GEnum.CtlPropertyUpdate.Enabled, true);
                            break;

                        case (int)GEnum.SystemCode.Inventory_Adjustment:
                            //Check for Add New Batch DocType
                            if (objDoc.DocState == (int)GEnum.DocState.Posted && objDoc.DocType == 400)//Add New Batch
                                GlobalUI.Ctrl_Update(frm, "DocTypeNm", GEnum.CtlPropertyUpdate.Enabled, false);
                            else
                                GlobalUI.Ctrl_Update(frm, "DocTypeNm", GEnum.CtlPropertyUpdate.Enabled, true);
                            break;
                    }
                    #endregion
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }

        }
        public static void FormGridLock_Set(Document objDoc, UltraGrid grd, GEnum.Details DocDetailType, bool FormLoad)
        {
            //Note : this function call the respective grid column locking function
            string colNm = string.Empty;
            string editLinkedPermID = "";//mic check

            try
            {
                if (objDoc.IsReadOnly == false)
                {
                    #region Edit Permission Checking for Linked Doc //Mic check
                    if (!FormLoad)
                    {
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Sales_Order:
                           //  case (int)GEnum.SystemCode.Reserve_Order:
                                editLinkedPermID = GVar.PermissionID.Able_to_edit_SO_that_has_been_link_to_Document;
                                break;
                            case (int)GEnum.SystemCode.Purchase_Order:
                                editLinkedPermID = GVar.PermissionID.Able_to_edit_PO_that_has_been_link_to_Document;
                                break;
                            case (int)GEnum.SystemCode.Order_Consignment:
                                editLinkedPermID = GVar.PermissionID.Able_to_edit_Order_Consignment_that_has_been_linked_to_Document;
                                break;
                            case (int)GEnum.SystemCode.Issue_Consignment:
                                editLinkedPermID = GVar.PermissionID.Able_to_edit_Issue_Consignment_that_has_been_linked_to_Document;
                                break;
                        }
                        if (editLinkedPermID != "")
                        {
                            if (SECPermUtility.Perform(editLinkedPermID, false) == false)
                            {
                                foreach (UltraGridRow row in grd.Rows)
                                {
                                    bool rowLock = false;
                                    if (GFunc.NEDec(row.Cells["ItmQtyLink"].Value, 0) != 0)
                                        rowLock = true;
                                    else if (objDoc.DocCodeKey != (int)GEnum.SystemCode.Issue_Consignment)
                                        if (GFunc.NEInt(row.Cells["ItmOrderStatus"].Value, 0) == (int)GEnum.OrderStatus.Delivered)
                                            rowLock = true;

                                    if (rowLock)
                                    {
                                        row.Activation = Activation.ActivateOnly;
                                        ((DataRowView)(row.ListObject)).Row.RowError = "Read Only. You do not have permission to edit a linked Document.";//Mic Check
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Quotation:
                        case (int)GEnum.SystemCode.Issue_Consignment:
                        case (int)GEnum.SystemCode.Return_Consignment:
                            switch (DocDetailType)
                            {
                                case GEnum.Details.Doc_Itm:
                                    FormGridLockItmType_Set(objDoc, grd, FormLoad);
                                    break;

                                default:
                                    FormGridLockNonItmType_Set(objDoc, grd, DocDetailType, FormLoad);
                                    break;
                            }
                            break;

                        case (int)GEnum.SystemCode.Sales_Order:
                        case (int)GEnum.SystemCode.Reserve_Order:
                        case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                        case (int)GEnum.SystemCode.Delivery_Order:
                        case (int)GEnum.SystemCode.Sales_Invoice:
                        case (int)GEnum.SystemCode.Sales_Debit_Note:
                        case (int)GEnum.SystemCode.Sales_Credit_Note:
                        case (int)GEnum.SystemCode.Cash_Sale:
                        case (int)GEnum.SystemCode.Cash_Debit_Note:
                        case (int)GEnum.SystemCode.Cash_Credit_Note:
                        case (int)GEnum.SystemCode.Purchase_Plan:
                        case (int)GEnum.SystemCode.Purchase_Request:
                        case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                        case (int)GEnum.SystemCode.Purchase_Shipment:
                        case (int)GEnum.SystemCode.Purchase_Delivery:
                        case (int)GEnum.SystemCode.Purchase_Invoice:
                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        case (int)GEnum.SystemCode.Inventory_Adjustment:
                        case (int)GEnum.SystemCode.Inventory_Production:
                        case (int)GEnum.SystemCode.Inventory_Transfer:
                        case (int)GEnum.SystemCode.Order_Consignment:
                        case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                        case (int)GEnum.SystemCode.Received_Consignment:
                        case (int)GEnum.SystemCode.Consignment_Settlement:
                            FormGridLockItmType_Set(objDoc, grd, FormLoad);
                            
                            //added by KKAung on 24 Apr 2023   /* updated by YST on 2024/12/18 to check later why ItmQty column specifically AllowEdit for ADL in FormGridLock function */                                                
                            if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Order && SysOptionUtility.DatabaseBranchCode.Equals("ADL")) 
                                grd.DisplayLayout.Bands[0].Columns["ItmQty"].CellActivation = Activation.AllowEdit;

                            if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM && objDoc.DocState <= (int?)GEnum.DocState.Draft)
                            {
                                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Quotation ||
                                    objDoc.DocCodeKey == (int)GEnum.SystemCode.Reserve_Order ||
                                    objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Order)
                                {
                                    int ItmType = 0; MSTItm objItm;
                                    foreach (UltraGridRow row in grd.Rows)
                                    {
                                        ItmType = GFunc.NEInt(row.Cells["ItmType"].Value, 0);
                                        if (ItmType == (int)GEnum.ItemType.Assembly || ItmType == (int)GEnum.ItemType.Stock)
                                        {
                                            objItm = MSTItm.Get(GFunc.NEInt(row.Cells["ItmKey"].Value, 0));
                                            row.Cells["ItmControlPrice"].Appearance.BackColor = GetEstoreCellColor(objItm);
                                            row.Cells["ItmControlPrice"].ActiveAppearance.BackColor = row.Cells["ItmControlPrice"].Appearance.BackColor;
                                        }
                                        else
                                        {
                                            row.Cells["ItmControlPrice"].Appearance.BackColor = Color.White;
                                            row.Cells["ItmControlPrice"].ActiveAppearance.BackColor = row.Cells["ItmControlPrice"].Appearance.BackColor;
                                        }
                                    }
                                }                                                              
                            }
                            if (SysOptionUtility.UseWMS && objDoc.DocCodeKey == (int)GEnum.SystemCode.Delivery_Order)
                            {
                                int ItmType = 0;
                                foreach (UltraGridRow row in grd.Rows)
                                {
                                    ItmType = GFunc.NEInt(row.Cells["ItmType"].Value, 0);
                                    if (GFunc.NEDec(row.Cells["DSQty"].Value, 0) == 0 &&  
                                        (  ItmType == (int)GEnum.ItemType.Assembly
                                        || ItmType == (int)GEnum.ItemType.Stock 
                                        || ItmType == (int)GEnum.ItemType.Non_Stock))
                                    {
                                        row.Cells["ItmQty"].Activation = Activation.ActivateOnly;
                                    }
                                    else
                                    {
                                        row.Cells["ItmQty"].Activation = Activation.AllowEdit;
                                    }
                                }
                            }
                            break;

                        //added by May on 06-Mar-2023
                        case (int)GEnum.SystemCode.Purchase_Order:
                            FormGridLockItmType_Set(objDoc, grd, FormLoad);
                            if (SysOptionUtility.DatabaseBranchCode.Equals("BHM"))
                                foreach (UltraGridRow row in grd.Rows)
                                {
                                    if (!GFunc.IsNEZ(row.Cells["ObCost"].Value))
                                    {
                                        row.Cells["ItmID"].Appearance.BackColor = System.Drawing.Color.LightGreen;
                                    }
                                    else
                                        row.Cells["ItmID"].Appearance.BackColor = System.Drawing.Color.White;
                                }                            
                            break;

                        case (int)GEnum.SystemCode.Packing_List:
                        case (int)GEnum.SystemCode.Payment_Received:
                        case (int)GEnum.SystemCode.Cash_Payment_Received:
                        case (int)GEnum.SystemCode.Contra:
                        case (int)GEnum.SystemCode.Cash_Contra:
                        case (int)GEnum.SystemCode.Payment_Issue:
                        case (int)GEnum.SystemCode.Journal:
                        case (int)GEnum.SystemCode.Deposit:
                            FormGridLockNonItmType_Set(objDoc, grd, DocDetailType, FormLoad);
                            break;
                    }
                }
                if (FormLoad)
                {
                    DocHDRUtil.FormGridSort_Disabled(objDoc, grd, GEnum.Details.Doc_Itm, true);
                }
                return;
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private static void FormGridLockItmType_Set(Document objDoc, UltraGrid grd, bool FormLoad)
        {
            //Cell lock for detail with ItmType (but exclude Packing List)
            //Note : this function implement permanet cell locking when FormLoad
            //This function also implement cell locking base on conditions of itmType and DocCode on the current grid row
            //Packing List not use here even though it has itmtype 
            string colNm = string.Empty;
            GEnum.INTypeGrp itmTypeGrp = 0;
            GEnum.ItemType itmType = 0;
            Activation celllock = Activation.AllowEdit;

            try
            {

                if (FormLoad)
                {
                    #region Set the required Hide/Lock for all column for FormLoad condition and also disallow sorting
                    for (int i = 0; i < grd.DisplayLayout.Bands[0].Columns.Count; i++)
                    {
                        colNm = grd.DisplayLayout.Bands[0].Columns[i].Key;
                        switch (colNm.ToLower())
                        {
                            case "appoid":
                                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Delivery_Order || objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Order
                                    || objDoc.DocCodeKey == (int)GEnum.SystemCode.Quotation || objDoc.DocCodeKey == (int)GEnum.SystemCode.Reserve_Order
                                    || objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Invoice) /* added by YST to be able to edit PO/Bill Num in Sale Invoice form */
                                {
                                    int ItmType = 0;
                                    if (grd.ActiveRow != null)
                                        ItmType = GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, 0);//==(int)GEnum.ItemType.Non_Stock)
                                    else if (grd.ActiveCell != null)
                                        ItmType = GFunc.NEInt(grd.ActiveCell.Row.Cells["ItmType"].Value, 0);

                                    if (ItmType == (int)GEnum.ItemType.Non_Stock || ItmType == (int)GEnum.ItemType.Stock)
                                    {
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.AllowEdit;
                                    }
                                    else
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                                }
                                else
                                    grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                                break;
                            #region Common Lock for all documents
                            case "appdid":                           
                            case "ardoid":
                            case "arivid":
                            case "arqoid":
                            case "arsoid":
                                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Delivery_Order ) /* added by YST to be able to edit PO/Bill Num in Sale Invoice form */
                                {
                                    int ItmType = 0;
                                    if (grd.ActiveRow != null)
                                        ItmType = GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, 0);//==(int)GEnum.ItemType.Non_Stock)
                                    else if (grd.ActiveCell != null)
                                        ItmType = GFunc.NEInt(grd.ActiveCell.Row.Cells["ItmType"].Value, 0);

                                    if (ItmType == (int)GEnum.ItemType.Non_Stock || ItmType == (int)GEnum.ItemType.Stock)
                                    {
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.AllowEdit;
                                    }
                                    else
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                                }
                                else
                                    grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                                break;
                            case "arsopoid":
                            case "bombuomkey":
                            case "bomlabouramt":
                            case "bommultiplier":
                            case "bomused":
                            case "bomusedgram":
                            case "bomusedweight":
                            case "bomweight":
                            case "bomweightuomkey":
                            case "cpdid":
                            case "cpdref":
                            case "cscpoid":
                            case "cscpsid":
                            case "cscsiid":
                            case "fgbuomkey":
                            case "fgoverheadamth":
                            case "fgproducegram":
                            case "fgproduceqty":
                            case "fgproduceweight":
                            case "fgweight":
                            case "fgweightuomkey":
                            case "fullpayment":
                            case "itmcontrolprice":
                            case "itmfgid":
                            case "itmlinkdocid":
                            case "itmlinkitmsn":
                            case "itmlinkpoid":
                            case "itmqtybalance":
                            case "itmsn":
                            case "itmstock":
                            case "itmuomid":
                            case "itmtaxgrprate":
                            //case "itmtaxgrpamtf": /* commented by YST on 11/10/2019 to allow to key in Tax Amt */
                            case "itmtaxgrpamtl":
                            case "itmvendorcurrkey":
                            case "lastmodifieddate":
                            case "posterr":
                            case "settlementdocdate":
                            case "settlementdocdes":
                            case "settlementdocid":
                            case "settlementdocref":
                            case "settlementitmqty":                            
                            case "itmconrate": //added by may on 17 nov 2017
                            //case "hscode": //added by Jane on 16 Jan 2024
                            //case "countryid": //added by Jane on 17 Jan 2024
                            case "psqty":
                            case "itmwrtyenddate": //added by YST on 02 Apr 2025
                                grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                                break;
                            case "itmtype":
                                grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                                break;

                            case "dsqty": //to disable DSQty column in PO -- added by yst on 29 dec 2018
                                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Order)
                                    grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                                break;
                            #endregion

                            #region Hide Column base on system option Use Project
                            case "itmjobkey":
                            case "itmjobphasekey":
                            case "itmjobtaskkey":
                            case "itmjobcosttypekey":
                                if (SysOptionUtility.UseProject == false)
                                    grd.DisplayLayout.Bands[0].Columns[colNm].Hidden = true;
                                break;
                            #endregion

                            #region Hide Column base on system option Use Department
                            case "itmdeptkey":
                                if (SysOptionUtility.UseDept == false)
                                    grd.DisplayLayout.Bands[0].Columns[colNm].Hidden = true;
                                break;
                            #endregion
                        }
                    }

                    //Lock for specific DocCode
                    switch (objDoc.DocCodeKey)
                    {
                        #region Lock for Order adjustment
                        case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                        case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                        case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                            grd.DisplayLayout.Bands[0].Columns["ItmID"].CellActivation = Activation.ActivateOnly;
                            grd.DisplayLayout.Bands[0].Columns["ItmQty"].CellActivation = Activation.ActivateOnly;
                            grd.DisplayLayout.Bands[0].Columns["ItmReqDate"].CellActivation = Activation.ActivateOnly;
                            grd.DisplayLayout.Bands[0].Columns["ItmPrmDate"].CellActivation = Activation.ActivateOnly;
                            grd.DisplayLayout.Bands[0].Columns["ItmMark"].CellActivation = Activation.ActivateOnly;
                            grd.DisplayLayout.Bands[0].Columns["ConfirmID"].CellActivation = Activation.ActivateOnly;
                            grd.DisplayLayout.Bands[0].Columns["ConfirmSN"].CellActivation = Activation.ActivateOnly;
                            grd.DisplayLayout.Bands[0].Columns["ItmAttachment"].CellActivation = Activation.ActivateOnly;
                            break;
                        #endregion

                        #region Lock for Return Consignment
                        case (int)GEnum.SystemCode.Return_Consignment:
                            grd.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                            foreach (UltraGridColumn col in grd.DisplayLayout.Bands[0].Columns)
                            {
                                col.CellActivation = Activation.ActivateOnly;
                            }

                            grd.DisplayLayout.Bands[0].Columns["ItmQty"].CellActivation = Activation.AllowEdit;
                            grd.DisplayLayout.Bands[0].Columns["ItmUOMKey"].CellActivation = Activation.ActivateOnly;
                            grd.DisplayLayout.Bands[0].Columns["ItmListPrice"].CellActivation = Activation.ActivateOnly;
                            grd.DisplayLayout.Bands[0].Columns["ItmPriceAfter"].CellActivation = Activation.ActivateOnly;
                            grd.DisplayLayout.Bands[0].Columns["ItmDisPercent"].CellActivation = Activation.ActivateOnly;
                            grd.DisplayLayout.Bands[0].Columns["ItmDisValue"].CellActivation = Activation.ActivateOnly;
                            grd.DisplayLayout.Bands[0].Columns["ItmPriceUser"].CellActivation = Activation.ActivateOnly;
                            grd.DisplayLayout.Bands[0].Columns["ItmAmtShw"].CellActivation = Activation.ActivateOnly;
                            break;
                        #endregion

                    }
                    #endregion
                }
                else
                {
                    #region set the required Lock for all column for ItmType condition

                    #region set the variable itmTypeGrp and itmType
                    //APPNDetItm do not have ItmType, we treat its row as stock item
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Purchase_Plan:
                            itmTypeGrp = GEnum.INTypeGrp.Stock;
                            itmType = GEnum.ItemType.Stock;
                            break;
                        case (int)GEnum.SystemCode.Consignment_Settlement:
                            itmTypeGrp = GEnum.INTypeGrp.Stock;
                            itmType = GEnum.ItemType.Consignment;
                            break;
                        default:
                            if (grd.ActiveRow != null)
                            {
                                itmTypeGrp = (GEnum.INTypeGrp)GFunc.GetINTypeGroup(GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, (int)GEnum.ItemType.Remark));
                                itmType = (GEnum.ItemType)GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, (int)GEnum.ItemType.Remark);
                            }
                            break;
                    }

                    #endregion

                    //Loop all column and set the required lock property of each column
                    for (int i = 0; i < grd.DisplayLayout.Bands[0].Columns.Count; i++)
                    {
                        colNm = grd.DisplayLayout.Bands[0].Columns[i].Key;

                        switch (colNm.ToLower())
                        {
                            #region Lock column for various itmType conditions

                            case "appoid":
                                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Delivery_Order || objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Order
                                    || objDoc.DocCodeKey == (int)GEnum.SystemCode.Quotation || objDoc.DocCodeKey == (int)GEnum.SystemCode.Reserve_Order
                                    || objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Invoice) /* added by YST to be able to edit PO/Bill Num in Sale Invoice form */
                                {
                                    int ItmType = 0;
                                    if (grd.ActiveRow != null)
                                        ItmType = GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, 0);//==(int)GEnum.ItemType.Non_Stock)
                                    else if (grd.ActiveCell != null)
                                        ItmType = GFunc.NEInt(grd.ActiveCell.Row.Cells["ItmType"].Value, 0);

                                    if (ItmType == (int)GEnum.ItemType.Non_Stock || ItmType == (int)GEnum.ItemType.Stock)
                                    {
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.AllowEdit;
                                    }
                                    else
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                                }
                                else
                                    grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                                break;
                            case "psqty":
                                grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                                break;
                            #region do nothing for these column as they are alway locked
                            case "appdid":
                           // case "appoid":
                            case "ardoid":
                            case "arivid":
                            case "arqoid":
                            case "arsoid":
                                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Delivery_Order ) /* added by YST to be able to edit PO/Bill Num in Sale Invoice form */
                                {
                                    int ItmType = 0;
                                    if (grd.ActiveRow != null)
                                        ItmType = GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, 0);//==(int)GEnum.ItemType.Non_Stock)
                                    else if (grd.ActiveCell != null)
                                        ItmType = GFunc.NEInt(grd.ActiveCell.Row.Cells["ItmType"].Value, 0);

                                    if (ItmType == (int)GEnum.ItemType.Non_Stock || ItmType == (int)GEnum.ItemType.Stock)
                                    {
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.AllowEdit;
                                    }
                                    else
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                                }
                                else
                                    grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                                break;
                            case "arsopoid":
                            case "bombuomkey":
                            case "bomlabouramt":
                            case "bommultiplier":
                            case "bomused":
                            case "bomusedgram":
                            case "bomusedweight":
                            case "bomweight":
                            case "bomweightuomkey":
                            case "cpdid":
                            case "cpdref":
                            case "cscpoid":
                            case "cscpsid":
                            case "cscsiid":
                            case "fgbuomkey":
                            case "fgoverheadamth":
                            case "fgproducegram":
                            case "fgproduceqty":
                            case "fgproduceweight":
                            case "fgweight":
                            case "fgweightuomkey":
                            case "fullpayment":
                            case "itmcontrolprice":
                            case "itmfgid":
                            case "itmlinkdocid":
                            case "itmlinkitmsn":
                            case "itmlinkpoid":
                            case "itmqtybalance":
                            case "itmqtymtotal":
                            case "itmsn":
                            case "itmstock":
                            case "itmtype":
                            case "itmuomid":
                            case "itmvendorcurrkey":
                            case "lastmodifieddate":
                            case "posterr":
                            case "settlementdocdate":
                            case "settlementdocdes":
                            case "settlementdocid":
                            case "settlementdocref":
                            case "settlementitmqty":
                            case "itmconrate": //added by may on 17 nov 2017
                            case "obcost":
                            //case "hscode"://added by jane on 16 jane 2024
                            //case "countryid"://added by jane on 16 jane 2024
                                break;
                            #endregion

                            #region do nothing for these column as they are always editable
                            case "custom1":
                            case "custom2":
                            case "custom3":
                                /* modified by YST on 2023/07/16 to use custom columns for TotalCost, GP & Margin(%) in Athena */
                                if (objDoc.IsReadOnly == false)
                                {
                                    if (SysOptionUtility.DatabaseBranchCode == DBCode.ADL && objDoc.DocCodeKey == (int)GEnum.SystemCode.Quotation)
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                                    else
                                    {
                                        celllock = Activation.AllowEdit;
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                    }
                                }
                                break;
                            case "itmattachment":
                            case "itmcost":
                            case "itmdes":
                            case "itmhide":
                            case "itmpacking":
                            case "itmprmdatenew":
                            case "itmref":
                            case "itmrem":
                            case "itmreply":
                            case "itmreqdatenew":
                            case "itmstatus":
                            case "itmqtym1":
                            case "itmqtym10":
                            case "itmqtym11":
                            case "itmqtym12":
                            case "itmqtym2":
                            case "itmqtym3":
                            case "itmqtym4":
                            case "itmqtym5":
                            case "itmqtym6":
                            case "itmqtym7":
                            case "itmqtym8":
                            case "itmqtym9":
                                if (objDoc.IsReadOnly == false)
                                {
                                    celllock = Activation.AllowEdit;
                                    grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                }
                                break;

                            #endregion

                            #region unlock - Batch Stock (ItmNewCost)
                            case "itmnewcost":
                                switch (objDoc.DocCodeKey)
                                {
                                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                                        switch (itmType)
                                        {
                                            case GEnum.ItemType.StockB:
                                            case GEnum.ItemType.Finished_GDB:
                                            case GEnum.ItemType.Serial_StockB:
                                            case GEnum.ItemType.Serial_Finished_GDB:
                                            case GEnum.ItemType.Consignment:
                                                celllock = Activation.ActivateOnly;
                                                break;
                                            default:
                                                celllock = Activation.AllowEdit;
                                                break;
                                        }
                                        break;
                                    default:
                                        celllock = Activation.AllowEdit;
                                        break;
                                }
                                grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                break;
                            #endregion

                            #region unlock - Stock
                            case "itmfromlockey":
                            case "itmtolockey":
                            case "itmfromacckey":
                            case "itmfromaccid":
                            case "itmfromaccdes":
                            case "itmtoacckey":
                            case "itmtoaccid":
                            case "itmtoaccdes":
                            case "itmlockey":
                                switch (itmTypeGrp)
                                {
                                    case GEnum.INTypeGrp.Stock:
                                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Invoice)
                                        {
                                            if (GFunc.IsNEZ(grd.ActiveRow.Cells["APPDDK"].Value) == false && colNm.ToLower() == "itmlockey")
                                                celllock = Activation.ActivateOnly;
                                            else
                                                celllock = Activation.AllowEdit;
                                        }
                                        else
                                            celllock = Activation.AllowEdit;
                                        break;
                                    default:
                                        celllock = Activation.ActivateOnly;
                                        break;
                                }
                                grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                break;
                            #endregion
                            case "itmaddamth": grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;  break;
                            #region unlock - Stock, Non Stock
                            case "fgreq":
                            case "fgoverheadkey":
                            case "fgoverheadcost":
                            case "bomreq":
                            case "bomissue":
                            case "bomreturn":
                            case "bomlabourcost":
                            case "itmqtydelivered":

                                switch (itmTypeGrp)
                                {
                                    case GEnum.INTypeGrp.Stock:
                                    case GEnum.INTypeGrp.Non_Stock:
                                        // celllock = Activation.AllowEdit;
                                        // added by KKAung on 10-Oct-2022 (start)
                                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Reserve_Order)
                                            celllock = Activation.ActivateOnly;
                                        else
                                            celllock = Activation.AllowEdit;
                                        // (end)                               
                                        break;
                                    default:
                                        celllock = Activation.ActivateOnly;
                                        break;
                                }
                                grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                break;
                            #endregion

                            #region unlock - Stock, Non Stock, Remarks
                            case "itmcolorkey":
                            case "itmscalesize":

                                switch (itmTypeGrp)
                                {
                                    case GEnum.INTypeGrp.Stock:
                                    case GEnum.INTypeGrp.Non_Stock:
                                    case GEnum.INTypeGrp.Remark:
                                        celllock = Activation.AllowEdit;
                                        break;
                                    default:
                                        celllock = Activation.ActivateOnly;
                                        break;
                                }
                                grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                break;
                            #endregion

                            #region unlock - Stock, Non Stock, Remarks (ItmUOMKey)
                            case "itmuomkey":
                                switch (objDoc.DocCodeKey)
                                {
                                    #region Sales Order, Purchase Order, Consignment Order
                                    case (int)GEnum.SystemCode.Sales_Order:
                                    case (int)GEnum.SystemCode.Reserve_Order:
                                    case (int)GEnum.SystemCode.Purchase_Order:
                                    case (int)GEnum.SystemCode.Order_Consignment:
                                        switch (itmType)
                                        {
                                            case GEnum.ItemType.Finished_GD:
                                            case GEnum.ItemType.Stock:
                                            case GEnum.ItemType.Assembly:
                                            case GEnum.ItemType.Consignment:
                                            case GEnum.ItemType.Non_Stock:
                                            case GEnum.ItemType.Service:
                                            case GEnum.ItemType.Header:
                                            case GEnum.ItemType.Remark:
                                                if (!GFunc.IsNEZ(grd.ActiveRow.Cells["ItmQtyLink"].Value))
                                                    celllock = Activation.ActivateOnly;
                                                else
                                                    celllock = Activation.AllowEdit;
                                                break;

                                            default:
                                                celllock = Activation.ActivateOnly;
                                                break;
                                        }
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                    #endregion

                                    #region Delivery Order
                                    case (int)GEnum.SystemCode.Delivery_Order:
                                        switch (itmType)
                                        {
                                            case GEnum.ItemType.Finished_GD:
                                            case GEnum.ItemType.Stock:
                                            case GEnum.ItemType.Assembly:
                                            case GEnum.ItemType.Consignment:
                                            case GEnum.ItemType.Non_Stock:
                                            case GEnum.ItemType.Service:
                                            case GEnum.ItemType.Header:
                                            case GEnum.ItemType.Remark:
                                                if (GFunc.NEInt(grd.ActiveRow.Cells["ARSODK"].Value, 0) != 0)
                                                    celllock = Activation.ActivateOnly;
                                                else
                                                    celllock = Activation.AllowEdit;
                                                break;

                                            default:
                                                celllock = Activation.ActivateOnly;
                                                break;
                                        }
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                    #endregion

                                    #region Issue Consignment
                                    case (int)GEnum.SystemCode.Issue_Consignment:
                                        switch (itmType)
                                        {
                                            case GEnum.ItemType.Finished_GD:
                                            case GEnum.ItemType.Stock:
                                            case GEnum.ItemType.Assembly:
                                            case GEnum.ItemType.Consignment:
                                            case GEnum.ItemType.Non_Stock:
                                            case GEnum.ItemType.Service:
                                            case GEnum.ItemType.Header:
                                            case GEnum.ItemType.Remark:
                                                if (!GFunc.IsNEZ(grd.ActiveRow.Cells["ItmQtyLink"].Value))
                                                    celllock = Activation.ActivateOnly;
                                                else
                                                {
                                                    if (GFunc.NEInt(grd.ActiveRow.Cells["ARSODK"].Value, 0) != 0 || GFunc.NEInt(grd.ActiveRow.Cells["CSCSIDK"].Value, 0) != 0)
                                                        celllock = Activation.ActivateOnly;
                                                    else
                                                        celllock = Activation.AllowEdit;
                                                }
                                                break;

                                            default:
                                                celllock = Activation.ActivateOnly;
                                                break;
                                        }
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                    #endregion

                                    #region Sales Invoice
                                    case (int)GEnum.SystemCode.Sales_Invoice:
                                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                                    case (int)GEnum.SystemCode.Cash_Sale:
                                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                                        switch (itmType)
                                        {
                                            case GEnum.ItemType.Finished_GD:
                                            case GEnum.ItemType.Stock:
                                            case GEnum.ItemType.Assembly:
                                            case GEnum.ItemType.Consignment:
                                            case GEnum.ItemType.Non_Stock:
                                            case GEnum.ItemType.Service:
                                            case GEnum.ItemType.Header:
                                            case GEnum.ItemType.Remark:
                                                if (GFunc.NEInt(grd.ActiveRow.Cells["ARSODK"].Value, 0) != 0 || GFunc.NEInt(grd.ActiveRow.Cells["ARDODK"].Value, 0) != 0)
                                                    celllock = Activation.ActivateOnly;
                                                else
                                                {
                                                    if (GFunc.NEInt(grd.ActiveRow.Cells["CSCPSDK"].Value, 0) != 0 || GFunc.NEInt(grd.ActiveRow.Cells["CSCSIDK"].Value, 0) != 0)
                                                        celllock = Activation.ActivateOnly;
                                                    else
                                                        celllock = Activation.AllowEdit;
                                                }
                                                break;

                                            default:
                                                celllock = Activation.ActivateOnly;
                                                break;
                                        }
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                    #endregion

                                    #region Purchase Delivery
                                    case (int)GEnum.SystemCode.Purchase_Delivery:
                                        switch (itmType)
                                        {
                                            case GEnum.ItemType.Finished_GD:
                                            case GEnum.ItemType.Stock:
                                            case GEnum.ItemType.Assembly:
                                            case GEnum.ItemType.Consignment:
                                            case GEnum.ItemType.Non_Stock:
                                            case GEnum.ItemType.Service:
                                            case GEnum.ItemType.Header:
                                            case GEnum.ItemType.Remark:
                                                if (GFunc.NEInt(grd.ActiveRow.Cells["APPODK"].Value, 0) != 0)
                                                    celllock = Activation.ActivateOnly;
                                                else
                                                    celllock = Activation.AllowEdit;
                                                break;

                                            default:
                                                celllock = Activation.ActivateOnly;
                                                break;
                                        }
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                    #endregion

                                    #region Purchase Invoice
                                    case (int)GEnum.SystemCode.Purchase_Invoice:
                                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                                        switch (itmType)
                                        {
                                            case GEnum.ItemType.Finished_GD:
                                            case GEnum.ItemType.Stock:
                                            case GEnum.ItemType.Assembly:
                                            case GEnum.ItemType.Consignment:
                                            case GEnum.ItemType.Non_Stock:
                                            case GEnum.ItemType.Service:
                                            case GEnum.ItemType.Header:
                                            case GEnum.ItemType.Remark:
                                                if ((int)grd.ActiveRow.Cells["APPODK"].Value != 0 || (int)grd.ActiveRow.Cells["APPDDK"].Value != 0)
                                                    celllock = Activation.ActivateOnly;
                                                else
                                                    celllock = Activation.AllowEdit;
                                                break;

                                            default:
                                                celllock = Activation.ActivateOnly;
                                                break;
                                        }
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                    #endregion

                                    #region Received Consignment
                                    case (int)GEnum.SystemCode.Received_Consignment:
                                        switch (itmType)
                                        {
                                            case GEnum.ItemType.Finished_GD:
                                            case GEnum.ItemType.Stock:
                                            case GEnum.ItemType.Assembly:
                                            case GEnum.ItemType.Consignment:
                                            case GEnum.ItemType.Non_Stock:
                                            case GEnum.ItemType.Service:
                                            case GEnum.ItemType.Header:
                                            case GEnum.ItemType.Remark:
                                                if ((int)grd.ActiveRow.Cells["CSCPODK"].Value != 0 || (int)grd.ActiveRow.Cells["CSCPSDK"].Value != 0)
                                                    celllock = Activation.ActivateOnly;
                                                else
                                                    celllock = Activation.AllowEdit;
                                                break;

                                            default:
                                                celllock = Activation.ActivateOnly;
                                                break;
                                        }
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                    #endregion

                                    #region Inventory Adjustment
                                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                                        switch (itmType)
                                        {
                                            case GEnum.ItemType.Finished_GD:
                                            case GEnum.ItemType.Stock:
                                            case GEnum.ItemType.Assembly:
                                            case GEnum.ItemType.Consignment:
                                            case GEnum.ItemType.Non_Stock:
                                            case GEnum.ItemType.Service:
                                            case GEnum.ItemType.Header:
                                            case GEnum.ItemType.Remark:
                                                if ((int)grd.ActiveRow.Cells["CSCPSDK"].Value != 0)
                                                    celllock = Activation.ActivateOnly;
                                                else
                                                    celllock = Activation.AllowEdit;
                                                break;

                                            default:
                                                celllock = Activation.ActivateOnly;
                                                break;
                                        }
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                    #endregion

                                    #region Return Consignment, (SO,PO,CO) Order Adjustment
                                    case (int)GEnum.SystemCode.Return_Consignment:
                                        //do nothing as it is always lock
                                        break;
                                    #endregion

                                    #region default
                                    default:
                                        switch (itmType)
                                        {
                                            case GEnum.ItemType.Finished_GD:
                                            case GEnum.ItemType.Stock:
                                            case GEnum.ItemType.Assembly:
                                            case GEnum.ItemType.Consignment:
                                            case GEnum.ItemType.Non_Stock:
                                            case GEnum.ItemType.Service:
                                            case GEnum.ItemType.Header:
                                            case GEnum.ItemType.Remark:
                                                celllock = Activation.AllowEdit;
                                                break;

                                            default:
                                                celllock = Activation.ActivateOnly;
                                                break;
                                        }
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                    #endregion
                                }
                                break;
                            #endregion

                            #region unLock - Stock, Non Stock, Charges (ItmID)
                            case "itmid":
                            case "sku1":
                            case "sku2":
                                //May added for Hazardous check on 23-Apr-2018
                                if (grd.ActiveRow != null)
                                    if (!GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                                    {
                                        switch (objDoc.DocCodeKey)
                                        {
                                            case (int)GEnum.SystemCode.Quotation:
                                            case (int)GEnum.SystemCode.Reserve_Order:
                                            case (int)GEnum.SystemCode.Sales_Order:
                                                List<SqlParameter> par = new List<SqlParameter>();
                                                par.Add(new SqlParameter("@ItmKey", GFunc.NEInt(grd.ActiveRow.Cells["ItmKey"].Value, 0)));
                                                DataTable dt = GFunc.ExecuteProc("ItemCheckHazardous", par);
                                                if (dt.Rows.Count > 0)
                                                {
                                                    grd.ActiveRow.Cells["ItmID"].Appearance.ForeColor = System.Drawing.Color.Red;
                                                }
                                                else
                                                    grd.ActiveRow.Cells["ItmID"].Appearance.ForeColor = System.Drawing.Color.Black;
;
                                                break;
                                        }
                                    }
                                switch (objDoc.DocCodeKey)
                                {
                                    #region Sales Order, Purchase Order, Consignment Order
                                    case (int)GEnum.SystemCode.Sales_Order:
                                    case (int)GEnum.SystemCode.Reserve_Order:
                                    case (int)GEnum.SystemCode.Purchase_Order:
                                    case (int)GEnum.SystemCode.Order_Consignment:
                                        switch (itmTypeGrp)
                                        {
                                            case GEnum.INTypeGrp.Stock:
                                            case GEnum.INTypeGrp.Non_Stock:
                                                if (!GFunc.IsNEZ(grd.ActiveRow.Cells["ItmQtyLink"].Value))
                                                    celllock = Activation.ActivateOnly;
                                                else
                                                    celllock = Activation.AllowEdit;
                                                break;

                                            case GEnum.INTypeGrp.Charges:
                                                if (GFunc.NEDec(grd.ActiveRow.Cells["ItmOrderStatus"].Value, 0) == 20)//Delivered
                                                    celllock = Activation.ActivateOnly;
                                                else
                                                    celllock = Activation.AllowEdit;
                                                break;

                                            default:
                                                celllock = Activation.AllowEdit;
                                                break;
                                        }
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                    #endregion

                                    #region Delivery Order
                                    case (int)GEnum.SystemCode.Delivery_Order:
                                        switch (itmTypeGrp)
                                        {
                                            case GEnum.INTypeGrp.Stock:
                                            case GEnum.INTypeGrp.Non_Stock:
                                            case GEnum.INTypeGrp.Charges:
                                                if ((int)grd.ActiveRow.Cells["ARSODK"].Value != 0)
                                                    celllock = Activation.ActivateOnly;
                                                else
                                                    celllock = Activation.AllowEdit;
                                                break;

                                            default:
                                                celllock = Activation.AllowEdit;
                                                break;
                                        }
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                    #endregion

                                    #region Issue Consignment
                                    case (int)GEnum.SystemCode.Issue_Consignment:
                                        switch (itmTypeGrp)
                                        {
                                            case GEnum.INTypeGrp.Stock:
                                            case GEnum.INTypeGrp.Non_Stock:
                                                if (!GFunc.IsNEZ(grd.ActiveRow.Cells["ItmQtyLink"].Value))
                                                    celllock = Activation.ActivateOnly;
                                                else
                                                {
                                                    if (!GFunc.IsNEZ(grd.ActiveRow.Cells["ARSODK"].Value) || !GFunc.IsNEZ(grd.ActiveRow.Cells["CSCSIDK"].Value))
                                                        celllock = Activation.ActivateOnly;
                                                    else
                                                        celllock = Activation.AllowEdit;
                                                }
                                                break;

                                            case GEnum.INTypeGrp.Charges:
                                                if (GFunc.NEDec(grd.ActiveRow.Cells["ItmOrderStatus"].Value, 0) == 20)//Delivered
                                                    celllock = Activation.ActivateOnly;
                                                else
                                                    celllock = Activation.AllowEdit;
                                                break;

                                            default:
                                                celllock = Activation.AllowEdit;
                                                break;
                                        }
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                    #endregion

                                    #region Sales Invoice
                                    case (int)GEnum.SystemCode.Sales_Invoice:
                                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                                    case (int)GEnum.SystemCode.Cash_Sale:
                                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                                        switch (itmTypeGrp)
                                        {
                                            case GEnum.INTypeGrp.Stock:
                                            case GEnum.INTypeGrp.Non_Stock:
                                            case GEnum.INTypeGrp.Charges:
                                                if ((int)grd.ActiveRow.Cells["ARSODK"].Value != 0 || (int)grd.ActiveRow.Cells["ARDODK"].Value != 0)
                                                    celllock = Activation.ActivateOnly;
                                                else
                                                {
                                                    if ((int)grd.ActiveRow.Cells["CSCPSDK"].Value != 0 || (int)grd.ActiveRow.Cells["CSCSIDK"].Value != 0)
                                                        celllock = Activation.ActivateOnly;
                                                    else
                                                        celllock = Activation.AllowEdit;
                                                }
                                                break;

                                            default:
                                                celllock = Activation.AllowEdit;
                                                break;
                                        }
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                    #endregion

                                    #region Purchase Delivery
                                    case (int)GEnum.SystemCode.Purchase_Delivery:
                                        switch (itmTypeGrp)
                                        {
                                            case GEnum.INTypeGrp.Stock:
                                            case GEnum.INTypeGrp.Non_Stock:
                                            case GEnum.INTypeGrp.Charges:
                                                if ((int)grd.ActiveRow.Cells["APPODK"].Value != 0)
                                                    celllock = Activation.ActivateOnly;
                                                else
                                                    celllock = Activation.AllowEdit;
                                                break;

                                            default:
                                                celllock = Activation.AllowEdit;
                                                break;
                                        }
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                    #endregion

                                    #region Purchase Invoice
                                    case (int)GEnum.SystemCode.Purchase_Invoice:
                                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                                        switch (itmTypeGrp)
                                        {
                                            case GEnum.INTypeGrp.Stock:
                                            case GEnum.INTypeGrp.Non_Stock:
                                            case GEnum.INTypeGrp.Charges:
                                                if ((int)grd.ActiveRow.Cells["APPODK"].Value != 0 || (int)grd.ActiveRow.Cells["APPDDK"].Value != 0)
                                                    celllock = Activation.ActivateOnly;
                                                else
                                                    celllock = Activation.AllowEdit;
                                                break;

                                            default:
                                                celllock = Activation.AllowEdit;
                                                break;
                                        }
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                    #endregion

                                    #region Received Consignment
                                    case (int)GEnum.SystemCode.Received_Consignment:
                                        switch (itmTypeGrp)
                                        {
                                            case GEnum.INTypeGrp.Stock:
                                                if ((int)grd.ActiveRow.Cells["CSCPODK"].Value != 0 || (int)grd.ActiveRow.Cells["CSCPSDK"].Value != 0)
                                                    celllock = Activation.ActivateOnly;
                                                else
                                                    celllock = Activation.AllowEdit;
                                                break;

                                            default:
                                                celllock = Activation.AllowEdit;
                                                break;
                                        }
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                    #endregion

                                    #region Inventory Adjustment
                                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                                        switch (itmTypeGrp)
                                        {
                                            case GEnum.INTypeGrp.Stock:
                                                if ((int)grd.ActiveRow.Cells["CSCPSDK"].Value != 0)
                                                    celllock = Activation.ActivateOnly;
                                                else
                                                    celllock = Activation.AllowEdit;
                                                break;

                                            default:
                                                celllock = Activation.AllowEdit;
                                                break;
                                        }
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                    #endregion

                                    #region Return Consignment, (SO,PO,CO) Order Adjustment
                                    case (int)GEnum.SystemCode.Return_Consignment:
                                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                                        //do nothing as it is always lock
                                        break;
                                    #endregion

                                    #region default
                                    default:
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.AllowEdit;
                                        break;
                                    #endregion
                                }
                                break;
                            #endregion

                            #region unlock - Stock, Non Stock, Charges (ItmReqDate, ItmPrmDate)
                            case "itmreqdate":
                            case "itmprmdate":

                                switch (objDoc.DocCodeKey)
                                {
                                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                                        //do nothing as it is always lock on FormLoad
                                        break;

                                    default:
                                        switch (itmTypeGrp)
                                        {
                                            case GEnum.INTypeGrp.Stock:
                                            case GEnum.INTypeGrp.Non_Stock:
                                            case GEnum.INTypeGrp.Charges:
                                                celllock = Activation.AllowEdit;
                                                break;
                                            case GEnum.INTypeGrp.Remark: /* use PromiseDatae as DueDate for Sample Item added by yst on 2020/09/25 */
                                                celllock = Activation.AllowEdit;
                                                if (grd.ActiveCell != null && (grd.ActiveCell.Row.Cells["ItmID"].Value).ToString() != SpecialRemark.Sample)
                                                    celllock = Activation.ActivateOnly;
                                                break;
                                            default:
                                                celllock = Activation.ActivateOnly;
                                                break;
                                        }
                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                }
                                break;
                            #endregion

                            #region unlock - Stock, Non Stock, Charges, Remarks

                            case "itmvendorprice":
                            case "itmvendorpriceratio":
                                switch (itmTypeGrp)
                                {
                                    case GEnum.INTypeGrp.Stock:
                                    case GEnum.INTypeGrp.Non_Stock:
                                    case GEnum.INTypeGrp.Charges:
                                    case GEnum.INTypeGrp.Remark:
                                        celllock = Activation.AllowEdit;
                                        break;
                                    default:
                                        celllock = Activation.ActivateOnly;
                                        break;
                                }
                                grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;

                                break;
                            case "itmvendorkey":
                            case "itmvendorid":
                            case "itmvendornm":
                            case "itmvendorpricelock":
                            case "itmdisprice": //use in Consignment Settlement only

                                switch (itmTypeGrp)
                                {
                                    case GEnum.INTypeGrp.Stock:
                                    case GEnum.INTypeGrp.Non_Stock:
                                    case GEnum.INTypeGrp.Charges:
                                    case GEnum.INTypeGrp.Remark:
                                        celllock = Activation.AllowEdit;
                                        break;
                                    default:
                                        celllock = Activation.ActivateOnly;
                                        break;
                                }
                                grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;

                                break;
                                break;
                            #endregion

                            #region unlock - Stock, Non Stock, Charges, Remarks (ItmListPrice, ItmPriceAfter, ItmDisPercent, ItmDisValue)
                            case "itmlistprice":
                            case "itmpriceafter":
                            case "itmdispercent":
                            case "itmdisvalue":


                                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Return_Consignment)
                                {
                                    //do nothing as it is always lock
                                }
                                else
                                {
                                    switch (itmTypeGrp)
                                    {
                                        case GEnum.INTypeGrp.Stock:
                                        case GEnum.INTypeGrp.Non_Stock:
                                        case GEnum.INTypeGrp.Charges:
                                        case GEnum.INTypeGrp.Remark:
                                            celllock = Activation.AllowEdit;
                                            break;
                                        default:
                                            celllock = Activation.ActivateOnly;
                                            break;
                                    }
                                    grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                }
                                break;
                            #endregion

                            #region unlock -  Remarks (ItmPriceUser)
                            case "itmpriceuser":
                                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Return_Consignment)
                                {
                                    //do nothing as it is always lock
                                }
                                else
                                {
                                    switch (itmTypeGrp)
                                    {
                                        case GEnum.INTypeGrp.Remark:
                                            celllock = Activation.AllowEdit;
                                            break;
                                        default:
                                            celllock = Activation.ActivateOnly;
                                            break;
                                    }
                                    grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                }
                                break;
                            #endregion

                            #region unlock - Stock, Non Stock, Charges, Remarks (ItmVendorCurrRate)
                            case "itmvendorcurrrate":

                                switch (itmTypeGrp)
                                {
                                    case GEnum.INTypeGrp.Stock:
                                    case GEnum.INTypeGrp.Non_Stock:
                                    case GEnum.INTypeGrp.Charges:
                                    case GEnum.INTypeGrp.Remark:
                                        //if ((int)grd.ActiveRow.Cells["ItmVendorCurrKey"].Value == 1)
                                        if (GFunc.NEInt(grd.ActiveRow.Cells["ItmVendorCurrKey"].Value,0) == 1)    // updated by KKAung on 09 Jun 2023
                                            celllock = Activation.ActivateOnly;
                                        else
                                            celllock = Activation.AllowEdit;
                                        break;
                                    default:
                                        celllock = Activation.ActivateOnly;
                                        break;
                                }
                                grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                break;
                            #endregion

                            #region unlock - Stock, Non Stock, Charges, Remarks (ConfirmID, ConfirmSN)
                            case "confirmid":
                            case "confirmsn":

                                switch (itmTypeGrp)
                                {
                                    case GEnum.INTypeGrp.Stock:
                                    case GEnum.INTypeGrp.Non_Stock:
                                    case GEnum.INTypeGrp.Charges:
                                    case GEnum.INTypeGrp.Remark:
                                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Order)
                                            celllock = Activation.AllowEdit;
                                        else
                                            celllock = Activation.ActivateOnly;
                                        break;
                                    default:
                                        celllock = Activation.ActivateOnly;
                                        break;
                                }
                                grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                break;
                            #endregion

                            #region unlock - Stock, Non Stock, Charges, Discount, Remarks
                            case "itmdeptkey":
                            case "itmtrangrpkey":
                            case "itmtaxgrpkey":
                            case "itmacckey":
                            case "itmaccid":
                            case "itmaccdes":
                            case "itmaccinid":
                            case "itmaccindes":
                                switch (itmTypeGrp)
                                {
                                    case GEnum.INTypeGrp.Stock:
                                    case GEnum.INTypeGrp.Non_Stock:
                                    case GEnum.INTypeGrp.Charges:
                                    case GEnum.INTypeGrp.Discount:
                                    case GEnum.INTypeGrp.Remark:
                                        celllock = Activation.AllowEdit;
                                        break;
                                    default:
                                        celllock = Activation.ActivateOnly;
                                        break;
                                }
                                grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                break;
                            #endregion

                            #region unlock - Stock, Non Stock, Charges, Discount, Remarks (ItmJobKey, ItmJobPhaseKey, ItmJobTaskKey, ItmJobCostTypeKey)
                            case "itmjobkey":
                            case "itmjobphasekey":
                            case "itmjobtaskkey":
                            case "itmjobcosttypekey":
                                switch (itmTypeGrp)
                                {
                                    case GEnum.INTypeGrp.Stock:
                                        switch (objDoc.DocCodeKey)
                                        {
                                            //case (int)GEnum.SystemCode.Purchase_Order:
                                            //case (int)GEnum.SystemCode.Purchase_Delivery:
                                            //case (int)GEnum.SystemCode.Purchase_Invoice:
                                            //case (int)GEnum.SystemCode.Purchase_Debit_Note:
                                            //case (int)GEnum.SystemCode.Purchase_Credit_Note:
                                            //    celllock = Activation.ActivateOnly;
                                            //    break;
                                            default:
                                                celllock = Activation.AllowEdit;
                                                break;
                                        }
                                        break;
                                    case GEnum.INTypeGrp.Non_Stock:
                                    case GEnum.INTypeGrp.Charges:
                                    case GEnum.INTypeGrp.Discount:
                                    case GEnum.INTypeGrp.Remark:
                                        celllock = Activation.AllowEdit;
                                        break;
                                    default:
                                        celllock = Activation.ActivateOnly;
                                        break;
                                }
                                grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                break;
                            #endregion

                            #region unlock - Stock, Non Stock, Charges, Discount, Remarks (ItmMark)
                            case "itmmark":

                                switch (objDoc.DocCodeKey)
                                {
                                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                                        celllock = Activation.ActivateOnly;
                                        break;

                                    default:
                                        switch (itmTypeGrp)
                                        {
                                            case GEnum.INTypeGrp.Stock:
                                            case GEnum.INTypeGrp.Non_Stock:
                                            case GEnum.INTypeGrp.Charges:
                                            case GEnum.INTypeGrp.Discount:
                                            case GEnum.INTypeGrp.Remark:
                                                celllock = Activation.AllowEdit;
                                                break;
                                            default:
                                                celllock = Activation.ActivateOnly;
                                                break;
                                        }
                                        break;
                                }
                                grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                break;
                            #endregion

                            #region unlock - Stock, Non Stock, Charges, Discount, Remarks (ItmQty)
                            case "itmqty":

                                switch (objDoc.DocCodeKey)
                                {
                                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                                    case (int)GEnum.SystemCode.Consignment_Settlement:
                                        celllock = Activation.ActivateOnly;
                                        break;
										
									//added by thettm on 28 Jun 2018(start)
                                    case (int)GEnum.SystemCode.Sales_Order:                                    
                                        if (grd.ActiveRow != null && !GFunc.IsNEZ(grd.ActiveRow.Cells["ItmQtyLink"].Value))
                                        {
                                            if ((decimal)grd.ActiveRow.Cells["ItmQtyLink"].Value > 0)
                                                celllock = Activation.ActivateOnly;
                                            else
                                                celllock = Activation.AllowEdit;
                                        }
                                        else
                                            celllock = Activation.AllowEdit;
                                        break;                                      
                                    //added by thettm on 28 Jun 2018(end)
									

                                    case (int)GEnum.SystemCode.Purchase_Invoice:
                                    //case (int)GEnum.SystemCode.Purchase_Debit_Note:
                                    //case (int)GEnum.SystemCode.Purchase_Credit_Note:

                                        #region Lock ItmQty for Purchase Delivery link to Invoice
                                        if (grd.ActiveRow != null && (int)grd.ActiveRow.Cells["APPDDK"].Value != 0)
                                        {
                                            switch (itmType)
                                            {
                                                case GEnum.ItemType.Finished_GDB:
                                                case GEnum.ItemType.Serial_Finished_GDB:
                                                case GEnum.ItemType.Serial_StockB:
                                                case GEnum.ItemType.StockB:
                                                case GEnum.ItemType.Finished_GD:
                                                case GEnum.ItemType.Stock:
                                                case GEnum.ItemType.Non_Stock:
                                                case GEnum.ItemType.Service:
                                                case GEnum.ItemType.Charges:
                                                    celllock = Activation.ActivateOnly;
                                                    break;

                                                default:
                                                    celllock = Activation.AllowEdit;
                                                    break;
                                            }
                                        }
                                        else
                                            celllock = Activation.AllowEdit;

                                        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                        break;
                                        #endregion

                                    default:
                                        switch (itmType)
                                        {
                                            case GEnum.ItemType.Finished_GDB:
                                            case GEnum.ItemType.Serial_StockB:
                                            case GEnum.ItemType.Serial_Finished_GDB:
                                            case GEnum.ItemType.StockB:
                                                // temprorary comment to test Costing
                                                //celllock = Activation.ActivateOnly; 
                                                celllock = Activation.AllowEdit;
                                                break;

                                            case GEnum.ItemType.Assembly:
                                            case GEnum.ItemType.Charges:
                                            case GEnum.ItemType.Consignment:
                                            case GEnum.ItemType.Discount:
                                            case GEnum.ItemType.Finished_GD:
                                            case GEnum.ItemType.Header:
                                            case GEnum.ItemType.Non_Stock:
                                            case GEnum.ItemType.Remark:
                                            case GEnum.ItemType.Service:
                                            case GEnum.ItemType.Stock:
                                                celllock = Activation.AllowEdit;
                                                break;

                                            default:
                                                celllock = Activation.ActivateOnly;
                                                break;
                                        }
                                        break;
                                }
                                grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                break;
                            #endregion

                            #region unlock - Charges
                            case "itmorderstatus":

                                switch (itmTypeGrp)
                                {
                                    case GEnum.INTypeGrp.Charges:
                                        celllock = Activation.AllowEdit;
                                        break;
                                    default:
                                        celllock = Activation.ActivateOnly;
                                        break;
                                }
                                grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                break;
                            #endregion

                            #region unlock - Charges, Discount, Remarks
                            case "itmamtshw":

                                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Return_Consignment)
                                {
                                    //do nothing as it is always lock
                                }
                                else
                                {
                                    switch (itmTypeGrp)
                                    {
                                        case GEnum.INTypeGrp.Discount:
                                        case GEnum.INTypeGrp.Remark:
                                            celllock = Activation.AllowEdit;
                                            break;
                                        default:
                                            celllock = Activation.ActivateOnly;
                                            break;
                                    }
                                    grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = celllock;
                                }
                                break;
                            #endregion

                            #endregion
                        }
                    }
                    #endregion
                }

                #region set Column to show password character base on permission on ItemViewCost
                for (int i = 0; i < grd.DisplayLayout.Bands[0].Columns.Count; i++)
                {
                    colNm = grd.DisplayLayout.Bands[0].Columns[i].Key;
                    switch (colNm.ToLower())
                    {
                        case "itmlatestcostf":
                        case "itmlatestcosth":
                        case "itmlatestcostshw":
                        case "itmvendorprice":
                        case "itmvendorpriceratio":
                            switch (objDoc.DocCodeKey)
                            {

                                case (int)GEnum.SystemCode.Purchase_Order:
                                case (int)GEnum.SystemCode.Purchase_Delivery:
                                case (int)GEnum.SystemCode.Purchase_Invoice:
                                case (int)GEnum.SystemCode.Purchase_Debit_Note:
                                case (int)GEnum.SystemCode.Purchase_Credit_Note:
                                case (int)GEnum.SystemCode.Issue_Consignment:
                                case (int)GEnum.SystemCode.Return_Consignment:
                                case (int)GEnum.SystemCode.Order_Consignment:
                                case (int)GEnum.SystemCode.Received_Consignment:

                                case (int)GEnum.SystemCode.Quotation:
                                case (int)GEnum.SystemCode.Sales_Order:
                                case (int)GEnum.SystemCode.Reserve_Order:
                                case (int)GEnum.SystemCode.Delivery_Order:
                                case (int)GEnum.SystemCode.Sales_Invoice:
                                case (int)GEnum.SystemCode.Sales_Debit_Note:
                                case (int)GEnum.SystemCode.Sales_Credit_Note:
                                case (int)GEnum.SystemCode.Cash_Sale:
                                case (int)GEnum.SystemCode.Cash_Debit_Note:
                                case (int)GEnum.SystemCode.Cash_Credit_Note:
                                    if (SECPermUtility.Perform("ItemViewCost", false) == false) // if there is no permission for ItemViewCost 
                                    {
                                        if (grd.DisplayLayout.Bands[0].Columns[colNm].EditorComponent != null)
                                        {
                                            if (grd.DisplayLayout.Bands[0].Columns[colNm].EditorComponent.GetType() == typeof(TAUtil.TANumericEditor))
                                            {
                                                ((TAUtil.TANumericEditor)grd.DisplayLayout.Bands[0].Columns[colNm].EditorComponent).PasswordChar = '*';
                                                grd.DisplayLayout.Bands[0].Columns[colNm].CellDisplayStyle = CellDisplayStyle.FullEditorDisplay;
                                                grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                                            }
                                        }
                                    }
                                    break;
                                #region commented
                                //else // if there is permission for ItemViewCost 
                                //{
                                //if (grd.DisplayLayout.Bands[0].Columns[colNm].EditorComponent != null)
                                //{
                                //    if (grd.DisplayLayout.Bands[0].Columns[colNm].EditorComponent.GetType() == typeof(TAUtil.TANumericEditor))
                                //    {
                                //        ((TAUtil.TANumericEditor)grd.DisplayLayout.Bands[0].Columns[colNm].EditorComponent).PasswordChar = '\0';

                                //        grd.DisplayLayout.Bands[0].Columns[colNm].CellDisplayStyle = CellDisplayStyle.FullEditorDisplay;

                                //    }
                                //}

                                //switch (itmTypeGrp)
                                //{
                                //    case GEnum.INTypeGrp.Stock:
                                //    case GEnum.INTypeGrp.Non_Stock:
                                //    case GEnum.INTypeGrp.Charges:
                                //        if (colNm.ToLower() == "itmlatestcosth") //itmlatestcosth is always readonly
                                //        {
                                //            grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                                //        }
                                //        else
                                //        {
                                //            grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.AllowEdit;

                                //        }
                                //        break;
                                //    default:
                                //        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                                //        break;
                                //}
                                //}     
                                #endregion
                            }
                            break;
                        case "itmlistprice":
                        case "itmpricebefore":
                        case "itmpriceafter":
                        case "itmprice":
                        case "itmpriceuser":
                        case "itmamtshw":
                        case "itmamtf":
                        case "itmamth":
                        case "itmtaxgrpamtf":
                        case "itmtaxgrpamtl":
                            switch (objDoc.DocCodeKey)
                            {
                                case (int)GEnum.SystemCode.Purchase_Order:
                                case (int)GEnum.SystemCode.Purchase_Delivery:
                                case (int)GEnum.SystemCode.Purchase_Invoice:
                                case (int)GEnum.SystemCode.Purchase_Debit_Note:
                                case (int)GEnum.SystemCode.Purchase_Credit_Note:
                                    if (SECPermUtility.Perform("ItemViewCost", false) == false) // if there is no permission for ItemViewCost 
                                    {
                                        if (grd.DisplayLayout.Bands[0].Columns[colNm].EditorComponent != null)
                                        {
                                            if (grd.DisplayLayout.Bands[0].Columns[colNm].EditorComponent.GetType() == typeof(TAUtil.TANumericEditor))
                                            {
                                                ((TAUtil.TANumericEditor)grd.DisplayLayout.Bands[0].Columns[colNm].EditorComponent).PasswordChar = '*';
                                                grd.DisplayLayout.Bands[0].Columns[colNm].CellDisplayStyle = CellDisplayStyle.FullEditorDisplay;
                                                
                                            }
                                        }
                                    }
                                    break;
                                                                                                                                                        #region commented
                            //else // if there is permission for ItemViewCost 
                            //{
                            //if (grd.DisplayLayout.Bands[0].Columns[colNm].EditorComponent != null)
                            //{
                            //    if (grd.DisplayLayout.Bands[0].Columns[colNm].EditorComponent.GetType() == typeof(TAUtil.TANumericEditor))
                            //    {
                            //        ((TAUtil.TANumericEditor)grd.DisplayLayout.Bands[0].Columns[colNm].EditorComponent).PasswordChar = '\0';

                            //        grd.DisplayLayout.Bands[0].Columns[colNm].CellDisplayStyle = CellDisplayStyle.FullEditorDisplay;

                            //    }
                            //}

                            //switch (itmTypeGrp)
                            //{
                            //    case GEnum.INTypeGrp.Stock:
                            //    case GEnum.INTypeGrp.Non_Stock:
                            //    case GEnum.INTypeGrp.Charges:
                            //        if (colNm.ToLower() == "itmlatestcosth") //itmlatestcosth is always readonly
                            //        {
                            //            grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                            //        }
                            //        else
                            //        {
                            //            grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.AllowEdit;

                            //        }
                            //        break;
                            //    default:
                            //        grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                            //        break;
                            //}
                            //}     
                            #endregion
                            }                            
                            break;
                        case "obcost":
                            switch (objDoc.DocCodeKey)
                            {
                                case (int)GEnum.SystemCode.Quotation:
                                case (int)GEnum.SystemCode.Sales_Order:
                                case (int)GEnum.SystemCode.Reserve_Order:
                                case (int)GEnum.SystemCode.Delivery_Order:
                                case (int)GEnum.SystemCode.Sales_Invoice:
                                case (int)GEnum.SystemCode.Sales_Debit_Note:
                                case (int)GEnum.SystemCode.Sales_Credit_Note:
                                case (int)GEnum.SystemCode.Cash_Sale:
                                case (int)GEnum.SystemCode.Cash_Debit_Note:
                                case (int)GEnum.SystemCode.Cash_Credit_Note:
                                    if (SECPermUtility.Perform("ItemViewCost", false) == false) // if there is no permission for ItemViewCost 
                                    {
                                        if (grd.DisplayLayout.Bands[0].Columns[colNm].EditorComponent != null)
                                        {
                                            if (grd.DisplayLayout.Bands[0].Columns[colNm].EditorComponent.GetType() == typeof(TAUtil.TANumericEditor))
                                            {
                                                ((TAUtil.TANumericEditor)grd.DisplayLayout.Bands[0].Columns[colNm].EditorComponent).PasswordChar = '*';
                                                grd.DisplayLayout.Bands[0].Columns[colNm].CellDisplayStyle = CellDisplayStyle.FullEditorDisplay;
                                                
                                            }
                                        }
                                    }
                                    grd.DisplayLayout.Bands[0].Columns[colNm].CellActivation = Activation.ActivateOnly;
                                    break;
                            }
                            break;
                    }
                }
                #endregion
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private static void FormGridLockNonItmType_Set(Document objDoc, UltraGrid grd, GEnum.Details docDetailType, bool FormLoad)
        {
            //Cell lock for detail without ItmType (but include Packing List)
            //Note : this function implement permanent cell locking when FormLoad
            //This function also implement cell locking base on conditions of DocCode on the current grid row
            //Packing List is use here even though it has ItmType
            string colNm = string.Empty;
            Activation celllock = Activation.AllowEdit;

            try
            {

                if (FormLoad)
                {
                    #region disable Sorting Indicator of columns and hide
                    for (int i = 0; i < grd.DisplayLayout.Bands[0].Columns.Count; i++)
                    {
                        colNm = grd.DisplayLayout.Bands[0].Columns[i].Key;
                        switch (colNm.ToLower())
                        {
                            #region Hide Column base on system option Use Project
                            case "expjobkey":
                            case "expjobphasekey":
                            case "expjobtaskkey":
                            case "expjobcosttypekey":
                            case "itmjobkey":
                            case "itmjobphasekey":
                            case "itmjobtaskkey":
                            case "itmjobcosttypekey":
                                if (SysOptionUtility.UseProject == false)
                                    grd.DisplayLayout.Bands[0].Columns[colNm].Hidden = true;
                                break;
                            #endregion

                            #region Hide Column base on system option Use Department
                            case "expdeptkey":
                            case "detitmdeptkey":
                            case "itmdocdeptkey":
                            case "itmdeptkey":
                                if (SysOptionUtility.UseDept == false)
                                    grd.DisplayLayout.Bands[0].Columns[colNm].Hidden = true;
                                break;
                            #endregion
                        }
                    }
                    #endregion

                    #region Hide column base on DocCodekey
                    switch (objDoc.DocCodeKey)
                    {
                        #region Quotation
                        case (int)GEnum.SystemCode.Quotation:
                            if (docDetailType == GEnum.Details.Doc_ItmVendor)
                            {
                                //do nothing as there is nothing to lock
                            }
                            if (docDetailType == GEnum.Details.Doc_Vendor)
                            {
                                grd.DisplayLayout.Bands[0].Columns["VendorKey"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["VendorCurrKey"].CellActivation = Activation.ActivateOnly;
                            }
                            break;
                        #endregion

                        #region Payment, Contra
                        case (int)GEnum.SystemCode.Payment_Received:
                        case (int)GEnum.SystemCode.Cash_Payment_Received:
                        case (int)GEnum.SystemCode.Payment_Issue:
                        case (int)GEnum.SystemCode.Contra:
                        case (int)GEnum.SystemCode.Cash_Contra:
                            if (docDetailType == GEnum.Details.Doc_Itm)
                            {
                                grd.DisplayLayout.Bands[0].Columns["LinkDocID"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["LinkDocDate"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["LinkDocType"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["LinkDocTypeNm"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["LinkDocTermKey"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["LinkDocDisDate"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["LinkDocDueDate"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["LinkDocGrand"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["LinkDocHome"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["LinkDocCurrKey"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["LinkDocCurrRate"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["LinkDocRef"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["ItmApplyRate"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["ItmApplyGainAmt"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["ItmApplyPayAmtF"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["ItmApplyPayAmtH"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["ItmApplyDueAmtF"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["ItmApplyDueAmtH"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["CustPONum"].CellActivation = Activation.ActivateOnly;

                            }
                            else
                            {
                                grd.DisplayLayout.Bands[0].Columns["ExpSN"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["ExpTaxGrpRate"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["ExpAmtF"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["ExpAmtH"].CellActivation = Activation.ActivateOnly;
                                //grd.DisplayLayout.Bands[0].Columns["ExpTaxGrpAmtF"].CellActivation = Activation.ActivateOnly; /* commented by YST to allow to change point amount of Tax Amount  */
                                grd.DisplayLayout.Bands[0].Columns["ExpTaxGrpAmtL"].CellActivation = Activation.ActivateOnly;
                            }
                            break;   
                        #endregion

                        #region Issue/return Consignment
                        case (int)GEnum.SystemCode.Issue_Consignment:
                        case (int)GEnum.SystemCode.Return_Consignment:
                            if (docDetailType == GEnum.Details.Doc_Exp)
                                grd.DisplayLayout.Bands[0].Columns["ExpSN"].CellActivation = Activation.ActivateOnly;
                            break;
                        #endregion

                        #region Packing List
                        case (int)GEnum.SystemCode.Packing_List:
                            if (docDetailType == GEnum.Details.Doc_Pack)
                            {
                                grd.DisplayLayout.Bands[0].Columns["ItmSN"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["ItmVolume"].CellActivation = Activation.ActivateOnly;
                            }
                            if (docDetailType == GEnum.Details.Doc_Itm)
                            {
                                grd.DisplayLayout.Bands[0].Columns["DetItmSN"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["DetItmType"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["DetItmBatchID"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["DetItmQtyTotal"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["DetItmDocID"].CellActivation = Activation.ActivateOnly;
                            }
                            break;
                        #endregion

                        #region Deposit
                        case (int)GEnum.SystemCode.Deposit:
                            if (docDetailType == GEnum.Details.Doc_Itm)
                            {
                                grd.DisplayLayout.Bands[0].Columns["ItmSN"].CellActivation = Activation.ActivateOnly;
                            }
                            break;
                        #endregion

                        #region Journal
                        case (int)GEnum.SystemCode.Journal:
                            if (docDetailType == GEnum.Details.Doc_Itm)
                            {
                                grd.DisplayLayout.Bands[0].Columns["ItmSN"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["ItmTaxGrpRate"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["ItmTaxGrpAmtL"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["ItmCreditFTotal"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["ItmCreditHTotal"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["ItmDebitFTotal"].CellActivation = Activation.ActivateOnly;
                                grd.DisplayLayout.Bands[0].Columns["ItmDebitHTotal"].CellActivation = Activation.ActivateOnly;

                                //Check Mic                                
                                if (SysOptionUtility.CountryCurrency == 1)
                                {
                                    grd.DisplayLayout.Bands[0].Columns["ItmCountryRate"].CellActivation = Activation.ActivateOnly;
                                }

                            }
                            break;
                        #endregion
                    }
                    #endregion

                }
                else
                {
                    //Loop all column and set the reqired lock property of each column
                    switch (objDoc.DocCodeKey)
                    {
                        #region Deposit
                        case (int)GEnum.SystemCode.Deposit:
                            for (int i = 0; i < grd.Rows.Count; i++)
                            {
                                //Data From Payment 
                                if (GFunc.NEInt(grd.Rows[i].Cells["ItmDocDc"].Value, 0) > 0)
                                    celllock = Activation.ActivateOnly;
                                else
                                    celllock = Activation.AllowEdit;

                                grd.Rows[i].Cells["ItmReFrom"].Activation = celllock;
                                grd.Rows[i].Cells["ItmDocDC"].Activation = celllock;
                                grd.Rows[i].Cells["ItmDocDK"].Activation = celllock;
                                grd.Rows[i].Cells["ItmDocDeptKey"].Activation = celllock;
                                grd.Rows[i].Cells["ItmTranGrpKey"].Activation = celllock;
                                grd.Rows[i].Cells["ItmDocAccKey"].Activation = celllock;
                                grd.Rows[i].Cells["ItmDocRef"].Activation = celllock;
                                grd.Rows[i].Cells["ItmDocChqDate"].Activation = celllock;
                                grd.Rows[i].Cells["ItmDocChqNum"].Activation = celllock;
                                if (celllock == Activation.ActivateOnly)
                                {
                                    grd.Rows[i].Cells["ItmDocCurrKey"].Activation = Activation.ActivateOnly;
                                    grd.Rows[i].Cells["ItmDocCurrRate"].Activation = Activation.ActivateOnly;
                                }
                                else
                                {
                                    if (GFunc.NEInt(grd.Rows[i].Cells["ItmDocCurrKey"].Value, 0) == 1)
                                    {
                                        grd.Rows[i].Cells["ItmDocCurrRate"].Activation = Activation.ActivateOnly;
                                    }
                                    else
                                    {

                                        grd.Rows[i].Cells["ItmDocCurrRate"].Activation = Activation.AllowEdit;
                                    }
                                }

                                grd.Rows[i].Cells["ItmDocAmtF"].Activation = celllock;
                                grd.Rows[i].Cells["ItmDocAmtH"].Activation = celllock;
                                grd.Rows[i].Cells["ItmDocAccID"].Activation = celllock;
                                grd.Rows[i].Cells["ItmDocAccDes"].Activation = celllock;

                                if (celllock == Activation.ActivateOnly)
                                    grd.Rows[i].Cells["ItmDocAccDes"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedTextEditor;

                                if ((int)grd.Rows[i].Cells["ItmDocCurrKey"].Value == (int)GFunc.GetIntPropertyValue("DocCurrKey", objDoc))
                                    grd.Rows[i].Cells["ItmBankAmtF"].Activation = Activation.ActivateOnly;
                                else
                                    grd.Rows[i].Cells["ItmBankAmtF"].Activation = Activation.AllowEdit;
                            }
                            break;
                        #endregion

                        #region Journal
                        case (int)GEnum.SystemCode.Journal:
                            Int32 ItmCurrKey = 1;
                            if (grd.ActiveRow != null)
                            {
                                if (GFunc.NEInt(grd.ActiveRow.Cells["ItmCurrKey"].Value, 0) == 1)
                                    celllock = Activation.ActivateOnly;
                                else
                                    celllock = Activation.AllowEdit;

                                //Check Mic
                                ItmCurrKey = GFunc.NEInt(grd.ActiveRow.Cells["ItmCurrKey"].Value, 0);
                            }

                            grd.DisplayLayout.Bands[0].Columns["ItmCurrRate"].CellActivation = celllock;

                            if (SysOptionUtility.CountryCurrency == 1)
                            {
                                grd.DisplayLayout.Bands[0].Columns["ItmCountryRate"].CellActivation = Activation.ActivateOnly;
                            }
                            else
                            {
                                if (ItmCurrKey == SysOptionUtility.CountryCurrency)
                                {
                                    grd.DisplayLayout.Bands[0].Columns["ItmCountryRate"].CellActivation = Activation.ActivateOnly;
                                }
                                else
                                {
                                    grd.DisplayLayout.Bands[0].Columns["ItmCountryRate"].CellActivation = Activation.AllowEdit;
                                }

                            }
                            break;
                        #endregion
                    }
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static void FormGridSort_Disabled(Document objDoc, UltraGrid grd, GEnum.Details docDetailType, bool FormLoad)
        {
            foreach (UltraGridColumn col in grd.DisplayLayout.Bands[0].Columns)
            {
                col.SortIndicator = SortIndicator.Disabled;
            }
        }//Completed

        //Control Event
        public static bool DocAccID_btnClick(Form frm, Document objDoc, Control ctrl, GEnum.PopupType popUpType, string ContextMenuSetting, string PermID)
        {
            try
            {
                bool result = false;
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ctrl.Name);

                if (EditorButton_Popup((int)objDoc.DocCodeKey, ctrl.Text, listSettingID, (int)popUpType, ref key, ref id, ref des))
                {
                    result = DocAccID_Update(objDoc, ctrl, key, id, des);
                    DocHDRUtil.FormControlLock_Set(frm, objDoc, PermID, false);
                }

                return result;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocAccID_btnClick(Form frm, Document objDoc, Hashtable docDet, Control ctrl, GEnum.PopupType popUpType, string ContextMenuSetting, string PermID)
        {
            try
            {
                bool result = false;
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ctrl.Name);

                if (EditorButton_Popup((int)objDoc.DocCodeKey, ctrl.Text, listSettingID, (int)popUpType, ref key, ref id, ref des))
                {
                    result = DocAccID_Update(objDoc, docDet, ctrl, key, id, des);
                    DocHDRUtil.FormControlLock_Set(frm, objDoc, PermID, false);
                }

                return result;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocAccID_CustomUpdate(Form frm, Document objDoc, Control ctrl, GEnum.RecAccessType recAccessType, string ContextMenuSetting, string PermID)
        {
            try
            {
                bool result = false;
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string ctrlValue = (ctrl.GetType() == typeof(TAUtil.TAComboBox)) ? ((TAUtil.TAComboBox)ctrl).Text.ToString() : ((TAUtil.TATextBoxEditor)ctrl).Text.ToString();//((TAUtil.TAComboBox)ctrl).Text.ToString(); //Mic_Ask_XXX
                //string ctrlValue = ((TAUtil.TAComboBox)ctrl).Text.ToString();
                int popUpType = 0;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ctrl.Name);

                key = GFunc.AccRecord_GetKey(recAccessType, listSettingID, ctrlValue, ref id, ref des, true);
                if (key == 0)
                {
                    if (recAccessType == GEnum.RecAccessType.AccID)
                        popUpType = (int)GEnum.PopupType.AccID;
                    else if (recAccessType == GEnum.RecAccessType.AccDes)
                        popUpType = (int)GEnum.PopupType.AccDes;
                    else
                        return false;

                    EditorButton_Popup((int)objDoc.DocCodeKey, ctrl.Text, listSettingID, popUpType, ref key, ref id, ref des);
                }

                result = DocAccID_Update(objDoc, ctrl, key, id, des);
                DocHDRUtil.FormControlLock_Set(frm, objDoc, PermID, false);
                return result;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocAccID_CustomUpdate(Form frm, Document objDoc, Hashtable docDet, Control ctrl, GEnum.RecAccessType recAccessType, string ContextMenuSetting, string PermID)
        {
            try
            {
                bool result = false;
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string ctrlValue = (ctrl.GetType() == typeof(TAUtil.TAComboBox)) ? ((TAUtil.TAComboBox)ctrl).Text.ToString() : ((TAUtil.TATextBoxEditor)ctrl).Text.ToString();//((TAUtil.TAComboBox)ctrl).Text.ToString(); //Mic_Ask_XXX
                int popUpType = 0;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ctrl.Name);

                key = GFunc.AccRecord_GetKey(recAccessType, listSettingID, ctrlValue, ref id, ref des, true);
                if (key == 0)
                {
                    if (recAccessType == GEnum.RecAccessType.AccID)
                        popUpType = (int)GEnum.PopupType.AccID;
                    else if (recAccessType == GEnum.RecAccessType.AccDes)
                        popUpType = (int)GEnum.PopupType.AccDes;
                    else
                        return false;

                    EditorButton_Popup((int)objDoc.DocCodeKey, ctrl.Text, listSettingID, popUpType, ref key, ref id, ref des);
                }

                result = DocAccID_Update(objDoc, docDet, ctrl, key, id, des);
                DocHDRUtil.FormControlLock_Set(frm, objDoc, PermID, false);
                return result;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private static bool DocAccID_Update(Document objDoc, Control ctrl, int key, string id, string des)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocAccID_Update(cn, objDoc, ctrl, key, id, des);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private static bool DocAccID_Update(Document objDoc, Hashtable docDet, Control ctrl, int key, string id, string des)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocAccID_Update(cn, objDoc, docDet, ctrl, key, id, des);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private static bool DocAccID_Update(SqlConnection cn, Document objDoc, Control ctrl, int key, string id, string des)
        {
            try
            {
                int docCurrKey = 0;
                int accCurrKey = 0;

                if (DocAccID_Validation(cn, objDoc, ctrl, key) == false)
                    return false;

                DocAccID_DependentSet(objDoc, ctrl, key, id, des);

                switch (ctrl.Name.ToLower())
                {
                    case "docaccglkey":
                    case "docaccglid":
                    case "docaccgldes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Sales_Adjustment:
                            case (int)GEnum.SystemCode.Cash_Adjustment:
                            case (int)GEnum.SystemCode.Purchase_Adjustment:
                                accCurrKey = (int)MSTAcc.Get(cn, key).AccCurrKey;
                                docCurrKey = (int)GFunc.GetPropertyValue("DocCurrKey", objDoc);
                                if (accCurrKey != docCurrKey)
                                {
                                    GFunc.SetPropertyValue("DocCurrKey", objDoc, accCurrKey);
                                    if (DocCurrKey_CustomUpdate(cn, objDoc) == false)
                                        return false;
                                }
                                break;
                        }
                        break;
                    case "docaccbkkey":
                    case "docaccbkid":
                    case "docaccbkdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Bank_Revaluation:
                                accCurrKey = (int)MSTAcc.Get(cn, key).AccCurrKey;
                                docCurrKey = (int)GFunc.GetPropertyValue("DocCurrKey", objDoc);
                                if (accCurrKey != docCurrKey)
                                {
                                    GFunc.SetPropertyValue("DocCurrKey", objDoc, accCurrKey);
                                    if (DocCurrKey_CustomUpdate(cn, objDoc) == false)
                                        return false;
                                }
                                break;
                        }
                        break;
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private static bool DocAccID_Update(SqlConnection cn, Document objDoc, Hashtable docDet, Control ctrl, int key, string id, string des)
        {
            try
            {
                if (DocAccID_Validation(cn, objDoc, ctrl, key) == false)
                    return false;

                if (DocAccID_Validation(cn, objDoc, docDet, ctrl, key) == false)
                    return false;

                DocAccID_DependentSet(objDoc, ctrl, key, id, des);

                switch (ctrl.Name.ToLower())
                {
                    case "docacckey":
                    case "docaccid":
                    case "docaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Payment_Issue:
                            case (int)GEnum.SystemCode.Payment_Received:
                            case (int)GEnum.SystemCode.Cash_Payment_Received:
                            case (int)GEnum.SystemCode.Bank_Revaluation:
                                GFunc.SetPropertyValue("DocCurrKey", objDoc, MSTAcc.Get(cn, key).AccCurrKey);
                                if (DocCurrKey_CustomUpdate(cn, objDoc, docDet) == false)
                                    return false;
                                break;
                        }
                        break;

                    case "docaccbkkey":
                    case "docaccbkid":
                    case "docaccbkdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Deposit:
                                GFunc.SetPropertyValue("DocCurrKey", objDoc, MSTAcc.Get(cn, key).AccCurrKey);
                                if (DocCurrKey_CustomUpdate(cn, objDoc, docDet) == false)
                                    return false;
                                break;
                        }
                        break;

                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private static bool DocAccID_Validation(SqlConnection cn, Document objDoc, Control ctrl, int? key)
        {
            try
            {
                string msg = string.Empty;
                bool runCheckNullEmptyZero = false;
                string ctrlName = ctrl.Name;

                switch (ctrlName.ToLower())
                {
                    #region DocAccAPKey - APADJ AP account
                    case "docaccapkey":
                    case "docaccapid":
                    case "docaccapdes":
                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Adjustment)
                        {
                            runCheckNullEmptyZero = true;
                            msg = "AP account cannot be empty";
                        }
                        break;
                    #endregion

                    #region DocAccARKey - ARADJ AR account
                    case "docaccarkey":
                    case "docaccarid":
                    case "docaccardes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Sales_Adjustment:
                            case (int)GEnum.SystemCode.Cash_Adjustment:
                                runCheckNullEmptyZero = true;
                                msg = "AR account cannot be empty";
                                break;
                        }
                        break;
                    #endregion

                    #region DocAccBKKey - Deposit Bank Account
                    case "docaccbkkey":
                    case "docaccbkid":
                    case "docaccbkdes":
                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Deposit)
                        {
                            runCheckNullEmptyZero = true;
                            msg = "Bank account cannot be empty";
                        }
                        break;
                    #endregion

                    #region DocAccChargesKey - INMFN Charges Account
                    case "docaccchargeskey":
                    case "docaccchargesid":
                    case "docaccchargesdes":
                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Inventory_Production)
                        {
                            if (GFunc.GetDecimalPropertyValue("DocChargesAmtH", objDoc) != 0)
                            {
                                runCheckNullEmptyZero = true;
                                msg = "Charges account cannot be empty";
                            }
                        }
                        break;
                    #endregion

                    #region DocAccGainKey - Deposit/Bank Revaluation Gain Account
                    case "docaccgainkey":
                    case "docaccgainid":
                    case "docaccgaindes":
                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Bank_Revaluation)
                        {
                            runCheckNullEmptyZero = true;
                            msg = "Gain/Loss account cannot be empty";
                        }
                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Deposit)
                        {
                            if (GFunc.GetDecimalPropertyValue("DocGainAmtH", objDoc) != 0)
                            {
                                runCheckNullEmptyZero = true;
                                msg = "Gain/Loss account cannot be empty";
                            }
                        }
                        break;
                    #endregion

                    #region DocAccGLKey - ARADJ/APADJ GL account
                    case "docaccglkey":
                    case "docaccglid":
                    case "docaccgldes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Sales_Adjustment:
                            case (int)GEnum.SystemCode.Cash_Adjustment:
                            case (int)GEnum.SystemCode.Purchase_Adjustment:
                                runCheckNullEmptyZero = true;
                                msg = "GL account cannot be empty";
                                break;
                        }
                        break;
                    #endregion

                    #region DocAccKey - Doc AR/AP/GL/INADJ Account
                    case "docacckey":
                    case "docaccid":
                    case "docaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Quotation:
                            case (int)GEnum.SystemCode.Sales_Order:
                            case (int)GEnum.SystemCode.Reserve_Order:
                            case (int)GEnum.SystemCode.Delivery_Order:
                            case (int)GEnum.SystemCode.Sales_Invoice:
                            case (int)GEnum.SystemCode.Sales_Debit_Note:
                            case (int)GEnum.SystemCode.Sales_Credit_Note:
                            case (int)GEnum.SystemCode.Cash_Sale:
                            case (int)GEnum.SystemCode.Cash_Debit_Note:
                            case (int)GEnum.SystemCode.Cash_Credit_Note:
                                runCheckNullEmptyZero = true;
                                msg = "AR account cannot be empty";
                                break;

                            case (int)GEnum.SystemCode.Purchase_Order:
                            case (int)GEnum.SystemCode.Purchase_Delivery:
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
                                runCheckNullEmptyZero = true;
                                msg = "AP account cannot be empty";
                                break;

                            case (int)GEnum.SystemCode.Payment_Received:
                            case (int)GEnum.SystemCode.Cash_Payment_Received:
                            case (int)GEnum.SystemCode.Payment_Issue:
                                runCheckNullEmptyZero = true;
                                msg = "GL account cannot be empty";
                                break;

                            case (int)GEnum.SystemCode.Inventory_Adjustment:
                                runCheckNullEmptyZero = true;
                                msg = "Adjustment account cannot be empty";
                                break;
                        }
                        break;
                    #endregion

                    #region DocAccLabourKey - INMFN Labour Account
                    case "docacclabourkey":
                    case "docacclabourid":
                    case "docacclabourdes":
                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Inventory_Production)
                        {
                            if (GFunc.GetDecimalPropertyValue("DocLabourAmtH", objDoc) != 0)
                            {
                                runCheckNullEmptyZero = true;
                                msg = "Labour account cannot be empty";
                            }
                        }
                        break;
                    #endregion

                    #region DocAccLossKey - Bank Revaluation Loss Account
                    case "docacclosskey":
                    case "docacclossid":
                    case "docacclossdes":
                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Bank_Revaluation)
                        {
                            runCheckNullEmptyZero = true;
                            msg = "Labour account cannot be empty";
                        }
                        break;
                    #endregion

                    #region DocAccOHKey - INMFN Overheads Account
                    case "docaccohkey":
                    case "docaccohid":
                    case "docaccohdes":
                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Inventory_Production)
                        {
                            runCheckNullEmptyZero = true;
                            msg = "OverHeads account cannot be empty";
                        }
                        break;
                    #endregion

                    #region DocAccRndKey - INMFN Rounding Account
                    case "docaccrndkey":
                    case "docaccrndid":
                    case "docaccrnddes":
                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Inventory_Production)
                        {
                            runCheckNullEmptyZero = true;
                            msg = "Rounding account cannot be empty";
                        }
                        break;
                    #endregion

                    #region DocAddCostAccKey - APBL Additional Cost Account
                    case "docaddcostacckey":
                    case "docaddcostaccid":
                    case "docaddcostaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                                if (SysOptionUtility.LandedCostOption == 30)//Post with landed cost
                                {
                                    runCheckNullEmptyZero = true;
                                    msg = "Additional cost account cannot be empty";
                                }
                                break;
                        }
                        break;
                    #endregion

                    #region DocApplyGainAccKey - Doc Credit Note Gain/Loss Account
                    case "docapplygainacckey":
                    case "docapplygainaccid":
                    case "docapplygainaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Sales_Credit_Note:
                            case (int)GEnum.SystemCode.Cash_Credit_Note:
                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
                                if (GFunc.GetDecimalPropertyValue("DocApplyGainAmt", objDoc) != 0)
                                {
                                    runCheckNullEmptyZero = true;
                                    msg = "Gain/Loss account cannot be empty";
                                }
                                break;
                        }
                        break;
                    #endregion

                    #region DocOverallDisAccKey - Doc Overall Discount Account
                    case "docoveralldisacc":
                    case "docoveralldisaccid":
                    case "docoveralldisaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Quotation:
                            case (int)GEnum.SystemCode.Sales_Order:
                            case (int)GEnum.SystemCode.Reserve_Order:
                            case (int)GEnum.SystemCode.Delivery_Order:
                            case (int)GEnum.SystemCode.Sales_Invoice:
                            case (int)GEnum.SystemCode.Sales_Debit_Note:
                            case (int)GEnum.SystemCode.Sales_Credit_Note:
                            case (int)GEnum.SystemCode.Cash_Sale:
                            case (int)GEnum.SystemCode.Cash_Debit_Note:
                            case (int)GEnum.SystemCode.Cash_Credit_Note:
                            case (int)GEnum.SystemCode.Purchase_Order:
                            case (int)GEnum.SystemCode.Purchase_Delivery:
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
                                if (GFunc.GetDecimalPropertyValue("DocOverallDisAmt", objDoc) != 0)
                                {
                                    runCheckNullEmptyZero = true;
                                    msg = "Overall discount account cannot be empty";
                                }
                                break;
                        }
                        break;
                    #endregion

                    #region DocPaidAccKey - Doc Invoice payment Account
                    case "docpaidacckey":
                    case "docpaidaccid":
                    case "docpaidaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Sales_Invoice:
                            case (int)GEnum.SystemCode.Sales_Debit_Note:
                            case (int)GEnum.SystemCode.Cash_Sale:
                            case (int)GEnum.SystemCode.Cash_Debit_Note:
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                                if (GFunc.GetDecimalPropertyValue("DocPaidAmtF", objDoc) != 0)
                                {
                                    runCheckNullEmptyZero = true;
                                    msg = "Payment account cannot be empty";
                                }
                                break;
                        }
                        break;
                    #endregion

                    #region ExpAccKey - ARPY/APPY detail expense account
                    case "expacckey":
                    case "expaccid":
                    case "expaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Payment_Received:
                            case (int)GEnum.SystemCode.Cash_Payment_Received:
                            case (int)GEnum.SystemCode.Payment_Issue:
                                runCheckNullEmptyZero = true;
                                msg = "Detail GL account cannot be empty";
                                break;
                        }
                        break;
                    #endregion

                    #region ItmAccINKey - INMFN detail inventory account
                    case "itmaccinkey":
                    case "itmaccinid":
                    case "itmaccindes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Inventory_Production:
                                runCheckNullEmptyZero = true;
                                msg = "Detail inventory account cannot be empty";
                                break;
                        }
                        break;
                    #endregion

                    #region ItmAccKey - general document detail sales/purchase/inventory account
                    case "itmacckey":
                    case "itmaccid":
                    case "itmaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Quotation:
                            case (int)GEnum.SystemCode.Sales_Order:
                            case (int)GEnum.SystemCode.Reserve_Order:
                            case (int)GEnum.SystemCode.Delivery_Order:
                            case (int)GEnum.SystemCode.Sales_Invoice:
                            case (int)GEnum.SystemCode.Sales_Debit_Note:
                            case (int)GEnum.SystemCode.Sales_Credit_Note:
                            case (int)GEnum.SystemCode.Cash_Sale:
                            case (int)GEnum.SystemCode.Cash_Debit_Note:
                            case (int)GEnum.SystemCode.Cash_Credit_Note:
                            case (int)GEnum.SystemCode.Purchase_Order:
                            case (int)GEnum.SystemCode.Purchase_Delivery:
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            case (int)GEnum.SystemCode.Inventory_Adjustment:
                                runCheckNullEmptyZero = true;
                                msg = "Detail account cannot be empty";
                                break;
                        }
                        break;
                    #endregion

                    #region ItmApplyDisAccKey - Payment/Contra document detail discount account
                    case "itmapplydisacckey":
                    case "itmapplydisaccid":
                    case "itmapplydisaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Payment_Received:
                            case (int)GEnum.SystemCode.Contra:
                            case (int)GEnum.SystemCode.Cash_Payment_Received:
                            case (int)GEnum.SystemCode.Cash_Contra:
                            case (int)GEnum.SystemCode.Payment_Issue:
                                runCheckNullEmptyZero = true;
                                msg = "Detail discount account cannot be empty";
                                break;
                        }
                        break;
                    #endregion

                    #region ItmApplyGainAccKey - Payment/Contra document detail gain/loss account
                    case "itmapplygainacckey":
                    case "itmapplygainaccid":
                    case "itmapplygainaccdes":

                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Payment_Received:
                            case (int)GEnum.SystemCode.Contra:
                            case (int)GEnum.SystemCode.Cash_Payment_Received:
                            case (int)GEnum.SystemCode.Cash_Contra:
                            case (int)GEnum.SystemCode.Payment_Issue:
                                runCheckNullEmptyZero = true;
                                msg = "Detail gain/loss account cannot be empty";
                                break;
                        }
                        break;
                    #endregion

                    #region ItmDocAccKey - deposit document detail account
                    case "itmdocacckey":
                    case "itmdocaccid":
                    case "itmdocaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Deposit:
                                runCheckNullEmptyZero = true;
                                msg = "Detail account cannot be empty";
                                break;
                        }
                        break;
                    #endregion

                    #region ItmFromAccKey/ItmToAccKey - INTRN/CSI/CSR document detail inventory account
                    case "itmfromacckey":
                    case "itmfromaccid":
                    case "itmfromaccdes":
                    case "itmtoacckey":
                    case "itmtoaccid":
                    case "itmtoaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Inventory_Transfer:
                            case (int)GEnum.SystemCode.Issue_Consignment:
                            case (int)GEnum.SystemCode.Return_Consignment:
                                runCheckNullEmptyZero = true;
                                msg = "Detail inventory account cannot be empty";
                                break;
                        }
                        break;
                    #endregion

                    #region LinkDocAccKey - Payment/Contra document detail Link account
                    case "linkdocacckey":
                    case "linkdocaccid":
                    case "linkdocaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Payment_Received:
                            case (int)GEnum.SystemCode.Contra:
                            case (int)GEnum.SystemCode.Cash_Payment_Received:
                            case (int)GEnum.SystemCode.Cash_Contra:
                            case (int)GEnum.SystemCode.Payment_Issue:
                                runCheckNullEmptyZero = true;
                                msg = "Detail link account cannot be empty";
                                break;
                        }
                        break;
                    #endregion

                }

                //Check for Null/Empty/Zero value
                if (runCheckNullEmptyZero)
                {
                    if (GFunc.IsNEZ(key))
                    {
                        MsgBox.Show(cn, msg);
                        return false;
                    }
                }

                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private static bool DocAccID_Validation(SqlConnection cn, Document objDoc, Hashtable docDet, Control ctrl, int? key)
        {
            try
            {
                DataTable dtItm = null;
                DataTable dtExp = null;
                string msg = string.Empty;
                string ctrlName = ctrl.Name;

                if (DocComUtility.DocDetail_Get((int)objDoc.DocCodeKey, docDet, ref dtItm, ref dtExp) == false)
                    return false;

                if (DocAccID_Validation(cn, objDoc, ctrl, key) == false)
                    return false;

                switch (ctrlName.ToLower())
                {
                    #region APPY,ARPY Bank Account
                    case "docacckey":
                    case "docaccid":
                    case "docaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Payment_Received:
                            case (int)GEnum.SystemCode.Cash_Payment_Received:
                            case (int)GEnum.SystemCode.Payment_Issue:
                                //Check for any DocDetail link that prevent user from changing the AccKey
                                foreach (DataRow row in dtItm.Rows)
                                {
                                    if (GFunc.NEDec(row["ItmApplyPayAmtF"], 0) != 0 || GFunc.NEDec(row["ItmApplyDisAmtF"], 0) != 0)//Ask Mic
                                    {
                                        MsgBox.Show(cn, "Can't change GL Account when document detail has been applied.");
                                        return false;
                                    }
                                }
                                break;
                        }
                        break;
                    #endregion

                    #region Deposit Bank Account
                    case "docaccbkkey":
                    case "docaccbkid":
                    case "docaccbkdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Deposit:
                                //Check for any DocDetail link that prevent user from changing the AccKey
                                foreach (DataRow row in dtItm.Rows)
                                {
                                    if (GFunc.NEDec(row["ItmDocAmtF"], 0) != 0 || GFunc.NEDec(row["ItmBankAmtF"], 0) != 0)//Ask Mic
                                    {
                                        MsgBox.Show(cn, "Changes not allow, you have to clear the detail or detail amount has to be zero and try again");
                                        return false;
                                    }
                                }
                                break;
                        }
                        break;


                    #endregion
                }

                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private static bool DocAccID_DependentSet(Document objDoc, Control ctrl, int? key, string id, string des)
        {
            try
            {
                switch (ctrl.Name.ToLower())
                {
                    case "docaccapkey":
                    case "docaccapid":
                    case "docaccapdes":
                        GFunc.SetPropertyValue("DocAccAPKey", objDoc, key);
                        GFunc.SetPropertyValue("DocAccAPID", objDoc, id);
                        GFunc.SetPropertyValue("DocAccAPDes", objDoc, des);
                        break;

                    case "docaccarkey":
                    case "docaccarid":
                    case "docaccardes":
                        GFunc.SetPropertyValue("DocAccARKey", objDoc, key);
                        GFunc.SetPropertyValue("DocAccARID", objDoc, id);
                        GFunc.SetPropertyValue("DocAccARDes", objDoc, des);
                        break;

                    case "docaccbkkey":
                    case "docaccbkid":
                    case "docaccbkdes":
                        GFunc.SetPropertyValue("DocAccBKKey", objDoc, key);
                        GFunc.SetPropertyValue("DocAccBKID", objDoc, id);
                        GFunc.SetPropertyValue("DocAccBKDes", objDoc, des);
                        break;

                    case "docaccchargeskey":
                    case "docaccchargesid":
                    case "docaccchargesdes":
                        GFunc.SetPropertyValue("DocAccChargesKey", objDoc, key);
                        GFunc.SetPropertyValue("DocAccChargesID", objDoc, id);
                        GFunc.SetPropertyValue("DocAccChargesDes", objDoc, des);
                        break;
                    case "docaccgainkey":
                    case "docaccgainid":
                    case "docaccgaindes":
                        GFunc.SetPropertyValue("DocAccGainKey", objDoc, key);
                        GFunc.SetPropertyValue("DocAccGainID", objDoc, id);
                        GFunc.SetPropertyValue("DocAccGainDes", objDoc, des);
                        break;
                    case "docaccglkey":
                    case "docaccglid":
                    case "docaccgldes":
                        GFunc.SetPropertyValue("DocAccGLKey", objDoc, key);
                        GFunc.SetPropertyValue("DocAccGLID", objDoc, id);
                        GFunc.SetPropertyValue("DocAccGLDes", objDoc, des);
                        break;

                    case "docacckey":
                    case "docaccid":
                    case "docaccdes":
                        GFunc.SetPropertyValue("DocAccKey", objDoc, key);
                        GFunc.SetPropertyValue("DocAccID", objDoc, id);
                        GFunc.SetPropertyValue("DocAccDes", objDoc, des);
                        break;

                    case "docacclabourkey":
                    case "docacclabourid":
                    case "docacclabourdes":
                        GFunc.SetPropertyValue("DocAccLabourKey", objDoc, key);
                        GFunc.SetPropertyValue("DocAccLabourID", objDoc, id);
                        GFunc.SetPropertyValue("DocAccLabourDes", objDoc, des);
                        break;

                    case "docacclosskey":
                    case "docacclossid":
                    case "docacclossdes":
                        GFunc.SetPropertyValue("DocAccLossKey", objDoc, key);
                        GFunc.SetPropertyValue("DocAccLossID", objDoc, id);
                        GFunc.SetPropertyValue("DocAccLossDes", objDoc, des);
                        break;

                    case "docaccohkey":
                    case "docaccohid":
                    case "docaccohdes":
                        GFunc.SetPropertyValue("DocAccOHKey", objDoc, key);
                        GFunc.SetPropertyValue("DocAccOHID", objDoc, id);
                        GFunc.SetPropertyValue("DocAccOHDes", objDoc, des);
                        break;

                    case "docaccrndkey":
                    case "docaccrndid":
                    case "docaccrnddes":
                        GFunc.SetPropertyValue("DocAccRndKey", objDoc, key);
                        GFunc.SetPropertyValue("DocAccRndID", objDoc, id);
                        GFunc.SetPropertyValue("DocAccRndDes", objDoc, des);
                        break;

                    case "docaddcostacckey":
                    case "docaddcostaccid":
                    case "docaddcostaccdes":
                        GFunc.SetPropertyValue("DocAddCostAccKey", objDoc, key);
                        GFunc.SetPropertyValue("DocAddCostAccID", objDoc, id);
                        GFunc.SetPropertyValue("DocAddCostAccDes", objDoc, des);
                        break;

                    case "docapplygainacckey":
                    case "docapplygainaccid":
                    case "docapplygainaccdes":
                        GFunc.SetPropertyValue("DocApplyGainAccKey", objDoc, key);
                        GFunc.SetPropertyValue("DocApplyGainAccID", objDoc, id);
                        GFunc.SetPropertyValue("DocApplyGainAccDes", objDoc, des);
                        break;

                    case "docoveralldisacc":
                    case "docoveralldisaccid":
                    case "docoveralldisaccdes":
                        GFunc.SetPropertyValue("DocOverallDisAcc", objDoc, key);
                        GFunc.SetPropertyValue("DocOverallDisAccID", objDoc, id);
                        //GFunc.SetPropertyValue("DocOverallDisDes", objDoc, des); note: this control is removed
                        break;

                    case "docpaidacckey":
                    case "docpaidaccid":
                    case "docpaidaccdes":
                        GFunc.SetPropertyValue("DocPaidAccKey", objDoc, key);
                        GFunc.SetPropertyValue("DocPaidAccID", objDoc, id);
                        GFunc.SetPropertyValue("DocPaidAccDes", objDoc, des);
                        break;
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed

        public static bool CurrencyInfor_Set(Document objDoc)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return CurrencyInfor_Set(cn, objDoc);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool CurrencyInfor_Set(SqlConnection cn, Document objDoc)
        {
            int DocCurrKey = 1;
            decimal CurrRate = 1;
            int DocConKey = 0;
            decimal CountryRate = 1;

            try
            {
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Sales_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Deposit:
                    case (int)GEnum.SystemCode.Bank_Revaluation:
                        DocCurrKey = (int)GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
                        if (!GFunc.IsNE(DocCurrKey))
                        {
                            CurrRate = (decimal)DocComUtility.CurrRate_Get(cn, DocCurrKey, objDoc.DocDate.Value, true);

                            //For Bank revaluation only
                            if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Bank_Revaluation)
                                GFunc.SetPropertyValue("DocRevalueRate", objDoc, CurrRate);
                            else
                                GFunc.SetPropertyValue("DocCurrRate", objDoc, CurrRate);

                            //Set Country Rate
                            if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Deposit || objDoc.DocCodeKey == (int)GEnum.SystemCode.Bank_Revaluation)
                                DocConKey = 0;
                            else
                                DocConKey = GFunc.NEInt(GFunc.GetIntPropertyValue("DocConKey", objDoc), 0);

                            //CountryRate = DocComUtility.CountryRate_Get(cn, DocConKey, DocCurrKey, CurrRate, (DateTime)objDoc.DocDate, true);
                            CountryRate = DocComUtility.CountryRate_Get(cn, DocConKey, DocCurrKey, CurrRate, (DateTime)objDoc.DocDate, true);
                            GFunc.SetPropertyValue("DocCountryRate", objDoc, CountryRate);
                        }
                        break;
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocCurrKey_CustomUpdate(Document objDoc)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocCurrKey_CustomUpdate(cn, objDoc);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocCurrKey_CustomUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocCurrKey_CustomUpdate(cn, objDoc, docDet);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocCurrKey_CustomUpdate(SqlConnection cn, Document objDoc)
        {
            try
            {
                if (CurrencyInfor_Set(cn, objDoc) == false)
                    return false;
                else
                    return DocComUtility.CalForm(cn, objDoc, false);

            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocCurrKey_CustomUpdate(SqlConnection cn, Document objDoc, Hashtable docDet)
        {
            try
            {
                if (DocCurrKey_Validation(cn, objDoc, docDet) == false)
                    return false;

                if (CurrencyInfor_Set(cn, objDoc) == false)
                    return false;

                return DocComUtility.CalForm(cn, objDoc, docDet, true, false);

            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private static bool DocCurrKey_Validation(SqlConnection cn, Document objDoc, Hashtable docDet)
        {
            try
            {
                UltraGrid grdItm = null;
                UltraGrid grdExp = null;
                if (GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 0) < 1)
                {
                    MsgBox.Show(cn, "Currency cann't be empty.");
                    return false;
                }
                if (DocComUtility.DocDetail_Get((int)objDoc.DocCodeKey, docDet, ref grdItm, ref grdExp) == false)
                    return false;

                //Check document header
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        //if (GFunc.IsNEZ(GFunc.GetDecimalPropertyValue("DocPaidAccKey", objDoc)) == false) added by thettm on 05 jun 2018
                        if (GFunc.IsNEZ(GFunc.GetIntPropertyValue("DocPaidAccKey", objDoc)) == false)
                        {
                            MsgBox.Show(cn, "The payment account has been set, please clear the payment details and try again");
                            return false;
                        }
                        break;

                    case (int)GEnum.SystemCode.Consignment_Settlement:
                        if (objDoc.DocState == (int)GEnum.DocState.Posted)
                        {
                            MsgBox.Show(cn, "You cannot change the currency when the document has been posted");
                            return false;
                        }
                        break;
                }

                if (Doc_CheckDetItm(objDoc, grdItm, GEnum.ValidateField.DocCurrKey) == false)
                    return false;

                //Check for any DocDetail link that prevent user from changing the customer
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                        if (Doc_CheckDetItm(objDoc, grdExp, GEnum.ValidateField.DocCurrKey) == false)
                            return true;
                        break;
                    default:
                        if (Doc_CheckDetItm(objDoc, grdItm, GEnum.ValidateField.DocCurrKey) == false)
                            return true;
                        break;
                }


                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        foreach (UltraGridRow row in grdItm.Rows)
                        {
                            if (GFunc.NEDec(row.Cells["ItmApplyDisAmtF"].Value, 0) != 0 ||
                              GFunc.NEDec(row.Cells["ItmApplyPayAmtF"].Value, 0) != 0 ||
                              GFunc.NEDec(row.Cells["ItmApplyGainAmt"].Value, 0) != 0)
                            {
                                MsgBox.Show(cn, "Can't change currency when document detail has been applied.");
                                return false;
                            }
                        }

                        MSTAcc objAcc = new MSTAcc();
                        objAcc.Fetch(cn, new MSTAcc.Criteria(GFunc.GetIntPropertyValue("DocAccKey", objDoc), 1));
                        if (objAcc.AccCurrKey != 1 && objAcc.AccCurrKey != GFunc.GetIntPropertyValue("DocCurrKey", objDoc))
                        {
                            MsgBox.Show(cn, "Cannot change currency as  this is locked by the GL Account currency.");
                            return false;
                        }
                        break;

                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                        foreach (UltraGridRow row in grdItm.Rows)
                        {
                            if (GFunc.NEDec(row.Cells["ItmApplyDisAmtF"].Value, 0) != 0 ||
                              GFunc.NEDec(row.Cells["ItmApplyPayAmtF"].Value, 0) != 0 ||
                              GFunc.NEDec(row.Cells["ItmApplyGainAmt"].Value, 0) != 0)
                            {
                                MsgBox.Show(cn, "Can't change currency when document detail has been applied.");
                                return false;
                            }
                        }
                        break;
                    case (int)GEnum.SystemCode.Deposit:
                        if (grdItm.Rows.Count > 0)
                        {
                            MsgBox.Show(cn, "Can't change currency as long as document details exist. \n\r , clear the details and try again.");
                            return false;
                        }
                        break;
                }

                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocCurrRate_CustomUpdate(Document objDoc)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocCurrRate_CustomUpdate(cn, objDoc);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocCurrRate_CustomUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocCurrRate_CustomUpdate(cn, objDoc, docDet);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocCurrRate_CustomUpdate(SqlConnection cn, Document objDoc)
        {
            try
            {
                decimal? docCurrRate = 1;

                if ((int)objDoc.DocCodeKey == (int)GEnum.SystemCode.Bank_Revaluation)
                {
                    docCurrRate = GFunc.RndC(GFunc.NEDec(GFunc.GetPropertyValue("DocRevalueRate", objDoc), 1), GVar.RndDecs.Curpt);
                    GFunc.SetPropertyValue("DocRevalueRate", objDoc, docCurrRate);
                }
                else
                {
                    docCurrRate = GFunc.RndC(GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1), GVar.RndDecs.Curpt);
                    GFunc.SetPropertyValue("DocCurrRate", objDoc, docCurrRate);
                }

                if (SysOptionUtility.CountryCurrency == 1)
                    GFunc.SetPropertyValue("DocCountryRate", objDoc, docCurrRate);

                return DocComUtility.CalForm(cn, objDoc, false);
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocCurrRate_CustomUpdate(SqlConnection cn, Document objDoc, Hashtable docDet)
        {
            try
            {
                bool SetCountryRate = true;
                DataTable dtItm = null;
                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref dtItm);

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        foreach (DataRow row in dtItm.Rows)
                        {
                            if (GFunc.NEDec(row["ItmApplyDisAmtF"], 0) != 0 || GFunc.NEDec(row["ItmApplyDocAmtF"], 0) != 0 || GFunc.NEDec(row["ItmApplyPayAmtF"], 0) != 0)//Ask Mic
                            {
                                MsgBox.Show(cn, "Transactions has been applied, clear the details and try again");
                                return false;
                            }
                        }
                        break;

                    case (int)GEnum.SystemCode.Consignment_Settlement:
                        if (objDoc.DocState == (int)GEnum.DocState.Posted)
                        {
                            MsgBox.Show(cn, "You cannot change the currency rate when the document has been posted");
                            return false;
                        }
                        break;
                    case (int)GEnum.SystemCode.Deposit:
                        if (dtItm.Rows.Count > 0)
                        {
                            MsgBox.Show(cn, "You cannot change the currency rate when document details exist. \n\r , clear the details and try again");
                            return false;
                        }
                        break;

                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                        SetCountryRate = false;
                        break;
                }

                decimal docCurrRate = GFunc.RndC(GFunc.GetPropertyValue("DocCurrRate", objDoc), GVar.RndDecs.Curpt);
                GFunc.SetPropertyValue("DocCurrRate", objDoc, docCurrRate);

                if (SetCountryRate)
                {
                    if (SysOptionUtility.CountryCurrency == 1)
                        GFunc.SetPropertyValue("DocCountryRate", objDoc, docCurrRate);
                }

                return DocComUtility.CalForm(cn, objDoc, docDet, true, false);
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocCountryRate_CustomUpdate(Document objDoc)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocCountryRate_CustomUpdate(cn, objDoc);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocCountryRate_CustomUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocCountryRate_CustomUpdate(cn, objDoc, docDet);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocCountryRate_CustomUpdate(SqlConnection cn, Document objDoc)
        {
            try
            {
                decimal? docCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);
                GFunc.SetPropertyValue("DocCountryRate", objDoc, docCountryRate);
                return DocComUtility.CalForm(cn, objDoc, false);
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocCountryRate_CustomUpdate(SqlConnection cn, Document objDoc, Hashtable docDet)
        {
            try
            {
                decimal? docCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);
                GFunc.SetPropertyValue("DocCountryRate", objDoc, docCountryRate);
                return DocComUtility.CalForm(cn, objDoc, docDet, true, false);
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed

        public static bool DocConID_btnClick(Form frm, Document objDoc, Control ctrl, GEnum.PopupType popUpType, string ContextMenuSetting, string PermID)
        {
            try
            {
                bool result = false;
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ctrl.Name);

                if (EditorButton_Popup((int)objDoc.DocCodeKey, ctrl.Text, listSettingID, (int)popUpType, ref key, ref id, ref des))
                {
                    result = DocConID_Update(objDoc, ctrl, key, id, des);
                    DocHDRUtil.FormControlLock_Set(frm, objDoc, PermID, false);
                }
                return result;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocConID_btnClick(Form frm, Document objDoc, Hashtable docDet, Control ctrl, GEnum.PopupType popUpType, string ContextMenuSetting, string PermID)
        {
            try
            {
                bool result = false;
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ctrl.Name);

                if (EditorButton_Popup((int)objDoc.DocCodeKey, ctrl.Text, listSettingID, (int)popUpType, ref key, ref id, ref des))
                {
                    result = DocConID_Update(objDoc, docDet, ctrl, key, id, des);
                    DocHDRUtil.FormControlLock_Set(frm, objDoc, PermID, false);
                }
                return result;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocConID_CustomUpdate(Form frm, Document objDoc, Control ctrl, GEnum.RecAccessType recAccessType, string ContextMenuSetting, string PermID)
        {

            try
            {
                bool result = false;
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string ctrlValue = ctrl.Text.ToString();
                int popUpType = 0;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ctrl.Name);

                #region get popUpType and check if user is editing the contact name which we will just do nothing and return true
                switch (recAccessType)
                {
                    case GEnum.RecAccessType.CustID:
                        popUpType = (int)GEnum.PopupType.CusID;
                        break;

                    case GEnum.RecAccessType.CustNm:
                        //Check if user edit contact name, thus we will not do anything and just return 
                        if (GFunc.IsNEZ(GFunc.GetPropertyValue("DocConKey", objDoc)) == false)
                            return true;

                        popUpType = (int)GEnum.PopupType.CusNm;
                        break;

                    case GEnum.RecAccessType.VendID:
                        popUpType = (int)GEnum.PopupType.VendID;
                        break;

                    case GEnum.RecAccessType.VendNm:
                        //Check if user edit contact name, thus we will not do anything and just return 
                        if (GFunc.IsNEZ(GFunc.GetPropertyValue("DocConKey", objDoc)) == false)
                            return true;

                        popUpType = (int)GEnum.PopupType.VendNm;
                        break;

                    default:
                        return false;
                }
                #endregion

                key = GFunc.ConRecord_GetKey(recAccessType, listSettingID, ctrlValue, ref id, ref des, true);
                if (key == 0)
                {
                    EditorButton_Popup((int)objDoc.DocCodeKey, ctrl.Text, listSettingID, (int)popUpType, ref key, ref id, ref des);
                }

                result = DocConID_Update(objDoc, ctrl, key, id, des);
                DocHDRUtil.FormControlLock_Set(frm, objDoc, PermID, false);
                return result;

            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocConID_CustomUpdate(Form frm, Document objDoc, Hashtable docDet, Control ctrl, GEnum.RecAccessType recAccessType, string ContextMenuSetting, string PermID)
        {
            try
            {
                bool result = false;
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string ctrlValue = ctrl.Text.ToString();
                int popUpType = 0;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ctrl.Name);
                UltraGrid grdItm = null;

                if (ctrlValue == string.Empty)
                {
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.WorkOrder:
                            return true;
                        case (int)GEnum.SystemCode.Quotation:
                            return true;
                        case (int)GEnum.SystemCode.Payment_Received:
                        case (int)GEnum.SystemCode.Cash_Payment_Received:
                        case (int)GEnum.SystemCode.Payment_Issue:
                        case (int)GEnum.SystemCode.Contra:
                        case (int)GEnum.SystemCode.Cash_Contra:
                            DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);

                            foreach (UltraGridRow row in grdItm.Rows)
                            {
                                if (GFunc.NEDec(row.Cells["ItmApplyDisAmtF"].Value, 0) != 0 ||
                                  GFunc.NEDec(row.Cells["ItmApplyPayAmtF"].Value, 0) != 0 ||
                                  GFunc.NEDec(row.Cells["ItmApplyGainAmt"].Value, 0) != 0)
                                {
                                    MsgBox.Show("Can't change currency when document detail has been applied.");
                                    return false;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }

                #region get popUpType and check if user is editing the contact name which we will just do nothing and return true
                switch (recAccessType)
                {
                    case GEnum.RecAccessType.CustID:
                        popUpType = (int)GEnum.PopupType.CusID;
                        break;

                    case GEnum.RecAccessType.CustNm:
                        //Check if user edit contact name, thus we will not do anything and just return 
                        if(objDoc.DocCodeKey==(int)GEnum.SystemCode.Payment_Received || objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Payment_Received)
                        {                            
                            Control[] ctrls = frm.Controls.Find("DocConKey", true);
                            if(ctrls.Length>0)
                                if (ctrls[0].Text != "" && !ctrls[0].Text.ToLower().Equals("one-time customer"))
                                    return true;
                                else
                                {
                                    key =GFunc.NEInt(GFunc.GetPropertyValue("DocConKey", objDoc), 0);
                                    des = ctrlValue;
                                }
                        }
                        else if (GFunc.IsNEZ(GFunc.GetPropertyValue("DocConKey", objDoc)) == false)
                            return true;

                        popUpType = (int)GEnum.PopupType.CusNm;
                        break;

                    case GEnum.RecAccessType.VendID:
                        popUpType = (int)GEnum.PopupType.VendID;
                        break;

                    case GEnum.RecAccessType.VendNm:
                        //Check if user edit contact name, thus we will not do anything and just return 
                        if (GFunc.IsNEZ(GFunc.GetPropertyValue("DocConKey", objDoc)) == false)
                            return true;

                        popUpType = (int)GEnum.PopupType.VendNm;
                        break;

                    default:
                        return false;
                }
                #endregion
                if(key==0)
                    ////ttm
                    key = GFunc.ConRecord_GetKey(recAccessType, listSettingID, ctrlValue, ref id, ref des, true, int.Parse(objDoc.DocCodeKey.ToString()));
                    // key = GFunc.ConRecord_GetKey(recAccessType, listSettingID, ctrlValue, ref id, ref des, true);

                if (key == 0)
                {
                    if (EditorButton_Popup((int)objDoc.DocCodeKey, ctrl.Text, listSettingID, (int)popUpType, ref key, ref id, ref des) == false)
                        return false;
                }

                result = DocConID_Update(objDoc, docDet, ctrl, key, id, des);
                DocHDRUtil.FormControlLock_Set(frm, objDoc, PermID, false);
                return result;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private static bool DocConID_Update(Document objDoc, Control ctrl, int key, string id, string des)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocConID_Update(cn, objDoc, ctrl, key, id, des);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocConID_Update(Document objDoc, Hashtable docDet, Control ctrl, int key, string id, string des)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocConID_Update(cn, objDoc, docDet, ctrl, key, id, des);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private static bool DocConID_Update(SqlConnection cn, Document objDoc, Control ctrl, int conKey, string id, string des)
        {
            try
            {

                if (DocConID_Validation(cn, objDoc, ctrl, conKey) == false)
                    return false;

                if (DocConID_DependentSet(cn, objDoc, ctrl, conKey, id, des) == false)
                    return false;

                if (DocComUtility.CalForm(cn, objDoc, false) == false)
                    return false;

                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocConID_Update(SqlConnection cn, Document objDoc, Hashtable docDet, Control ctrl, int key, string id, string des)
        {
            try
            {
                UltraGrid grdItm = null;
                UltraGrid grdExp = null;
                DataTable dtItm = null;
                DataTable dtExp = null;

                if (ctrl.Name.ToLower() == "docconkey")//to do only when ID update. To skip when Name/Des update
                {
                    if (DocConID_Validation(cn, objDoc, docDet, ctrl, key) == false)
                        return false;

                    if (DocConID_DependentSet(cn, objDoc, ctrl, key, id, des) == false)
                        return false;
                }
                #region retrive document details from HashTables
                if (DocComUtility.DocDetail_Get((int)objDoc.DocCodeKey, docDet, ref grdItm, ref grdExp) == false)
                    return false;

                if (DocComUtility.DocDetail_Get((int)objDoc.DocCodeKey, docDet, ref dtItm, ref dtExp) == false)
                    return false;
                #endregion

                #region Clear document detail for Payment, Contra, Order Adjustment and Settlement
                switch (objDoc.DocCodeKey)
                {

                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:

                        //Clear all detail payment/contra when cv is change and add new detail list
                        //insert new applylist here

                        //how to use the transaction
                        //cn.BeginTransaction();
                        //Turn on restore flag to restore objects if any error occurs           

                        #region Fetch Data in dtTables
                        ARPYDetItms ARPYDetItmsObj = new ARPYDetItms(cn);
                        //int varARConKey = (int)GFunc.GetPropertyValue("DocConKey", objDoc);                         
                        if(ctrl.Text.ToLower().Equals("one-time customer") 
                            && (des == "(KEY IN CUSTOMER'S NAME HERE)" || des == ""))//added by May. To get apply list when Customer Name is entered
                        {
                            //skip
                        }
                        else if (!ARPYDetItmsObj.GetApplyList(cn, new ARPYDetItms.Criteria(objDoc.GUID, objDoc.DocCodeKey, objDoc.DocKey, key, AppInfor.CurrentUserKey, 1,des, 1)))
                        {
                            MsgBox.Show(cn, MsgID.Common.GetPaymentListFail);
                            return false;
                        }

                        #endregion
                        grdItm.DataSource = (DataTable)ARPYDetItmsObj;

                        break;
                    case (int)GEnum.SystemCode.Payment_Issue:
                        #region Fetch Data in dtTables
                        APPYDetItms APPYDetItmsObj = new APPYDetItms(cn);
                        int varAPConKey = (int)GFunc.GetPropertyValue("DocConKey", objDoc);
                        if (!APPYDetItmsObj.GetApplyList(cn, new APPYDetItms.Criteria(objDoc.GUID, objDoc.DocCodeKey, objDoc.DocKey, varAPConKey, AppInfor.CurrentUserKey, 1, 1)))
                        {
                            MsgBox.Show(cn, MsgID.Common.GetPaymentListFail);
                            return false;
                        }

                        #endregion
                        grdItm.DataSource = (DataTable)APPYDetItmsObj;
                        break;

                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                        #region Fetch Data in dtTables
                        ARCTDetItms ARCTDetItmsObj = new ARCTDetItms(cn);
                        int varConKey = (int)GFunc.GetPropertyValue("DocConKey", objDoc);
                        if (!ARCTDetItmsObj.GetApplyList(cn, new ARCTDetItms.Criteria(objDoc.GUID, objDoc.DocCodeKey, objDoc.DocKey, varConKey, AppInfor.CurrentUserKey, 1, 1)))
                        {
                            MsgBox.Show(cn, MsgID.Common.GetPaymentListFail);
                            return false;
                        }

                        #endregion
                        grdItm.DataSource = (DataTable)ARCTDetItmsObj;

                        break;

                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                        dtItm.Rows.Clear();
                        break;
                }
                #endregion

                #region Set department default and value in document details
                if (DocDeptKey_CustomUpdate(objDoc, docDet) == false)
                    return false;
                #endregion

                #region reset detail Job Infor for sales document
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                        //foreach (UltraGridRow row in grdItm.Rows)
                        //{
                        //    row.Cells["ItmJobKey"].Value = 0;
                        //    row.Cells["ItmJobPhaseKey"].Value = 0;
                        //    row.Cells["ItmJobCostTypeKey"].Value = 0;
                        //    row.Cells["ItmJobTaskKey"].Value = 0;
                        //}

                        //GFunc.SetPropertyValue("DefJobKey", objDoc, 0);
                        //if (DefJob_CustomUpdate(objDoc, docDet, 0, true) == false)
                        //    return true;
                        break;

                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:

                        foreach (UltraGridRow row in grdExp.Rows)
                        {
                            row.Cells["ExpJobKey"].Value = 0;
                            row.Cells["ExpJobPhaseKey"].Value = 0;
                            row.Cells["ExpJobCostTypeKey"].Value = 0;
                            row.Cells["ExpJobTaskKey"].Value = 0;
                        }

                        GFunc.SetPropertyValue("DefJobKey", objDoc, 0);
                        if (DefJob_CustomUpdate(objDoc, docDet, 0, true) == false)
                            return true;

                        break;
                }
                #endregion

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note :
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                        if (DocTaxGrpKey_CustomUpdate(cn, objDoc, docDet)==false)
                            return false;
                        break;
                    default:
                        if (DocComUtility.CalForm(cn, objDoc, docDet, true, false) == false)
                            return false;
                        break;
                }

                //This is already called inside DocTaxGrpKey_CustomUpdate function
                //if (DocComUtility.CalForm(cn, objDoc, docDet, true, false) == false)
                //    return false;

                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private static bool DocConID_Validation(SqlConnection cn, Document objDoc, Control ctrl, int? key)
        {
            try
            {
                #region check valid conkey
                if (GFunc.IsNEZ(key))
                {
                    MsgBox.Show(cn, MsgID.Document.ConKeyIsRequired);
                    return false;
                }
                #endregion

                #region check record access
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Packing_List:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Sales_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        if (GFunc.CanAccessRecord(cn, key, (int)GEnum.SystemCode.Customer) == false)
                        {
                            MsgBox.Show(cn, "You have no access right to this record");
                            return false;
                        }
                        break;

                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                        if (GFunc.CanAccessRecord(cn, key, (int)GEnum.SystemCode.Vendor) == false)
                        {
                            MsgBox.Show(cn, "You have no access right to this record");
                            return false;
                        }
                        break;
                }
                #endregion

                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private static bool DocConID_Validation(SqlConnection cn, Document objDoc, Hashtable docDet, Control ctrl, int? key)
        {
            try
            {
                UltraGrid grdItm = null;
                UltraGrid grdExp = null;
                DataTable dtItm = null;
                DataTable dtExp = null;

                //retrive datatable and grid from Hashtable
                if (DocComUtility.DocDetail_Get((int)objDoc.DocCodeKey, docDet, ref dtItm, ref dtExp) == false)
                    return false;

                if (DocComUtility.DocDetail_Get((int)objDoc.DocCodeKey, docDet, ref grdItm, ref grdExp) == false)
                    return false;

                //Document Header validation
                if (DocConID_Validation(cn, objDoc, ctrl, key) == false)
                    return false;

                //Check for any DocDetail link that prevent user from changing the customer
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                        if (Doc_CheckDetItm(objDoc, grdExp, GEnum.ValidateField.DocConKey) == false)
                            return false;
                        break;
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                        if (grdItm.Rows.Count > 0)
                        {
                            MsgBox.Show(cn, "Cannot change vendor when detail has been created.");
                            return false;
                        }

                        break;
                    default:
                        if (Doc_CheckDetItm(objDoc, grdItm, GEnum.ValidateField.DocConKey) == false)
                            return false;
                        break;
                }

                //prompt user confirmation before changing Conkey
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                        if (MsgBox.Show(cn, "The system will clear all detail documents, confirm?", GEnum.MsgBoxButton.OK, GEnum.MsgBoxButton.Cancel) != GEnum.MsgBoxButton.OK)
                            return false;
                        else
                            break;

                    case (int)GEnum.SystemCode.Consignment_Settlement:
                        if (MsgBox.Show(cn, "The system will clear all settled documents, confirm?", GEnum.MsgBoxButton.OK, GEnum.MsgBoxButton.Cancel) != GEnum.MsgBoxButton.OK)
                            return false;
                        else
                            break;
                }

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        foreach (UltraGridRow row in grdItm.Rows)
                        {
                            if (GFunc.NEDec(row.Cells["ItmApplyDisAmtF"].Value, 0) != 0 ||
                              GFunc.NEDec(row.Cells["ItmApplyPayAmtF"].Value, 0) != 0 ||
                              GFunc.NEDec(row.Cells["ItmApplyGainAmt"].Value, 0) != 0)
                            {
                                MsgBox.Show(cn, "Can't change Customer when document detail has been applied, please clear the apply list and try again.");
                                return false;
                            }
                        }
                        break;
                }

                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private static bool DocConID_DependentSet(SqlConnection cn, Document objDoc, Control ctrl, int? conKey, string id, string des)
        {
            try
            {
                MSTCon objCon = null;           //Customer or Vendor Object
                DateTime docDate;

                string SetCV = string.Empty;    //Customer or Vendor
                bool SetUEN = false;            //ConUEN
                bool SetCostCentre = false;     //Dept,TranGrp
                bool SetAcc = false;            //DocAccKey
                bool SetDocGrp = false;         //DocGrp
                bool SetPriceType = false;      //PriceTye
                bool SetTermKey = false;        //Term
                bool SetBAddress = false;       //BillingAddr    
                bool SetSAddress = false;       //ShippingAddr
                bool SetShipInfor = false;      //ShipName, ShipMark
                bool SetDocRem = false;         //DocRemDelivery, Payment, Validity, Price
                bool SetTaxInfor = false;       //TaxGrp,TaxRate
                bool SetCurr = false;           //CurrKey,CurrRate,CountryRate
                bool SetJob = false;            //JobKey, Phase, Task, CostType
                bool SetAccAR = false;          //AccAR
                bool SetAccAP = false;          //AccARP
                bool SetApplyList = false;
                bool SetOverallDefaultDis = true;
                bool setTransGrp = false;

                #region Get MSTCon record
                objCon = MSTCon.Get(cn, conKey);
                if (objCon.ConKey == null)
                {
                    MsgBox.Show(cn, "Unable to retrieve record");
                    return false;
                }
                #endregion

                #region Set values to related Contact controls
                docDate = (DateTime)objDoc.DocDate;
                GFunc.SetPropertyValue("DocConKey", objDoc, conKey);
                GFunc.SetPropertyValue("DocConID", objDoc, id);
                GFunc.SetPropertyValue("DocConNm", objDoc, des);
                if (objDoc.DocCodeKey == (int?)GEnum.SystemCode.Payment_Issue ||
                    objDoc.DocCodeKey == (int?)GEnum.SystemCode.GL_Payment ||
                    objDoc.DocCodeKey == (int?)GEnum.SystemCode.Purchase_Adjustment)  /* added by YST on 2020/10/29 */
                {
                    GFunc.SetPropertyValue("Custom1", objDoc, objCon.Country);
                    GFunc.SetPropertyValue("Custom2", objDoc, objCon.BankName);
                    GFunc.SetPropertyValue("Custom3", objDoc, "SHA"); // SHA => bank charges paid by customer
                    GFunc.SetPropertyValue("Custom4", objDoc, objCon.SWIFTCode);
                    GFunc.SetPropertyValue("Custom5", objDoc, objCon.BankAccountNo.Trim() == "" ? objCon.IBAN_NO : objCon.BankAccountNo) ;
                }

                #endregion

                #region Set Default Document Type from Mst_Con ; // Mic Check; Jack Added 9 Nov 2012
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Payment_Issue:
                        if (objCon.ConType == 40) //Non Trade
                        {
                            List<SqlParameter> parmList = new List<SqlParameter>();
                            SqlParameter retVal=new SqlParameter("@RetValue", 0);
                            retVal.Direction=ParameterDirection.Output;
                            parmList.Add(new SqlParameter("@docTypeNm", objCon.VDefaultAPPYDocType));
                            parmList.Add(retVal);
                            GFunc.ExecuteProc(cn, "APPY_CheckDocTypeNmExist", parmList);
                            if (Convert.ToInt16(retVal.Value) > 0) //>0 means exist
                            {
                                GFunc.SetPropertyValue("DocTypeNm", objDoc, objCon.VDefaultAPPYDocType);
                            }
                        }
                        break;                    
                }
                #endregion

                #region Set process to run by Document code
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        SetCV = "Customer";
                        SetUEN = true;
                        SetCostCentre = true;
                        SetAcc = true;
                        SetDocGrp = true;
                        SetPriceType = true;
                        SetTermKey = true;
                        SetBAddress = true;
                        SetSAddress = true;
                        SetShipInfor = true;
                        SetDocRem = true;
                        SetTaxInfor = true;
                        SetCurr = true;

                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Quotation:
                            case (int)GEnum.SystemCode.Sales_Order:
                            case (int)GEnum.SystemCode.Reserve_Order:
                            case (int)GEnum.SystemCode.Delivery_Order:
                            case (int)GEnum.SystemCode.Sales_Invoice:
                            case (int)GEnum.SystemCode.Sales_Debit_Note:
                            case (int)GEnum.SystemCode.Sales_Credit_Note:
                            case (int)GEnum.SystemCode.Cash_Sale:
                            case (int)GEnum.SystemCode.Cash_Debit_Note:
                            case (int)GEnum.SystemCode.Cash_Credit_Note:
                                setTransGrp = true;
                                break;
                            case (int)GEnum.SystemCode.Purchase_Shipment:
                            case (int)GEnum.SystemCode.Purchase_Delivery:
                                SetAcc = false; /* added by YST on 2019/07/22 to block AccCode Changes by Vendor */
                                SetCV = "Vendor";
                                SetTermKey = false;
                                SetBAddress = false;
                                SetSAddress = false;
                                SetTaxInfor = false;
                                break;                            

                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            case (int)GEnum.SystemCode.Purchase_Order:                            
                                SetCV = "Vendor";
                                SetSAddress = false;
                                break;

                            case (int)GEnum.SystemCode.Order_Consignment:
                            case (int)GEnum.SystemCode.Received_Consignment:
                                SetCV = "Vendor";
                                SetCostCentre = false;
                                SetAcc = false;
                                SetShipInfor = false;
                                SetTaxInfor = false;

                                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Received_Consignment)
                                {
                                    SetBAddress = false;
                                    SetSAddress = false;
                                }
                                break;

                            case (int)GEnum.SystemCode.Issue_Consignment:
                            case (int)GEnum.SystemCode.Return_Consignment:
                                SetCV = "Customer";
                                SetAcc = false;
                                SetShipInfor = false;
                                SetTaxInfor = false;
                                break;
                        }
                        break;

                    case (int)GEnum.SystemCode.Sales_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Adjustment:
                        SetCV = "Customer";
                        SetJob = true;
                        SetUEN = true;
                        SetCostCentre = true;
                        SetDocGrp = true;

                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Adjustment)
                        {
                            SetCV = "Vendor";
                            SetAccAP = true;
                        }
                        else
                            SetAccAR = true;

                        break;

                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        SetCV = "Customer";
                        SetUEN = true;
                        SetCostCentre = true;
                        SetAcc = false;
                        SetDocGrp = true;
                        SetBAddress = true;
                        SetTaxInfor = true;

                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Payment_Issue)
                        {
                            SetCV = "Vendor";
                            SetDocRem = true;
                        }
                        break;

                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                        SetCV = "Customer";
                        SetUEN = true;
                        SetCurr = true;
                        SetCostCentre = true;
                        SetDocGrp = true;
                        break;

                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                        SetCV = "Customer";
                        SetDocGrp = true;
                        break;

                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                        SetCV = "Vendor";
                        SetDocGrp = true;
                        break;

                    case (int)GEnum.SystemCode.Packing_List:
                        SetCV = "Customer";
                        SetUEN = true;
                        SetBAddress = true;
                        SetSAddress = true;
                        SetShipInfor = true;
                        SetDocRem = true;
                        break;

                    case (int)GEnum.SystemCode.Purchase_Plan:
                        SetCV = "Vendor";
                        SetCostCentre = true;
                        SetDocGrp = true;
                        SetBAddress = true;
                        SetSAddress = true;
                        break;

                    case (int)GEnum.SystemCode.Consignment_Settlement:
                        SetCV = "Vendor";
                        SetCurr = true;
                        break;

                    default:
                        return false;
                }
                #endregion

                #region Set EM, Branch
                if (GFunc.CompareString(SetCV, "Customer"))
                {                    
                    GFunc.SetPropertyValue("DocEmKey", objDoc, objCon.CEMKey);
                    GFunc.SetPropertyValue("BranchKey", objDoc, GFunc.NEInt(objCon.CBranchKey, GFunc.GetPropertyValue("BranchKey", objDoc)));
                }
                else
                {
                    if (GFunc.IsNEZ(objDoc.DocEmKey))//added by May on 10-Oct-2013
                        GFunc.SetPropertyValue("DocEmKey", objDoc, objCon.VEMKey);
                    GFunc.SetPropertyValue("BranchKey", objDoc, GFunc.NEInt(objCon.VBranchKey, GFunc.GetPropertyValue("BranchKey", objDoc)));
                }
                #endregion

                #region Set UEN,Dept,Acc,DocGrp,PriceType,Term, DocRem(Price,Validity..)
                if (SetUEN)
                    GFunc.SetPropertyValue("DocConUEN", objDoc, objCon.ConUEN);

                if (SetCostCentre)
                {
                    if (GFunc.CompareString(SetCV, "Customer"))
                        GFunc.SetPropertyValue("DocDeptKey", objDoc, GFunc.NEInt(objCon.CDeptKey, GFunc.GetPropertyValue("DocDeptKey", objDoc)));
                    else if (GFunc.CompareString(SetCV, "Vendor"))
                        GFunc.SetPropertyValue("DocDeptKey", objDoc, GFunc.NEInt(objCon.VDeptKey, GFunc.GetPropertyValue("DocDeptKey", objDoc)));
                }

                if (SetAcc)
                    if (GFunc.CompareString(SetCV, "Customer"))
                        GFunc.SetPropertyValue("DocAccKey", objDoc, objCon.CAccKey);
                    else if (GFunc.CompareString(SetCV, "Vendor"))
                        GFunc.SetPropertyValue("DocAccKey", objDoc, objCon.VAccKey);

                if (SetDocGrp)
                    if (GFunc.CompareString(SetCV, "Customer"))
                        GFunc.SetPropertyValue("DocGrpKey", objDoc, objCon.CGrpKey);
                    else if (GFunc.CompareString(SetCV, "Vendor"))
                        GFunc.SetPropertyValue("DocGrpKey", objDoc, objCon.VGrpKey);

                if (SetPriceType)
                    if (GFunc.CompareString(SetCV, "Customer"))
                        GFunc.SetPropertyValue("DocPriceType", objDoc, objCon.CPriceType);
                    else if (GFunc.CompareString(SetCV, "Vendor"))
                        GFunc.SetPropertyValue("DocPriceType", objDoc, objCon.VPriceType);

                if (SetTermKey)
                    if (GFunc.CompareString(SetCV, "Customer"))
                        GFunc.SetPropertyValue("DocTermKey", objDoc, objCon.CTermKey);
                    else if (GFunc.CompareString(SetCV, "Vendor"))
                        GFunc.SetPropertyValue("DocTermKey", objDoc, objCon.VTermKey);

                if (SetDocRem)
                {
                    if (GFunc.CompareString(SetCV, "Customer"))
                    {
                        if(!GFunc.IsNE(objCon.CRemDelivery))
                            GFunc.SetPropertyValue("DocRemDelivery", objDoc, objCon.CRemDelivery);
                        else
                            GFunc.SetPropertyValue("DocRemDelivery", objDoc, "");

                        if (!GFunc.IsNE(objCon.CRemPrice))
                            GFunc.SetPropertyValue("DocRemPrice", objDoc, objCon.CRemPrice);
                        else
                            GFunc.SetPropertyValue("DocRemPrice", objDoc, "");

                        if (!GFunc.IsNE(objCon.CRemValidity))
                            GFunc.SetPropertyValue("DocRemValidity", objDoc, objCon.CRemValidity);
                        else
                            GFunc.SetPropertyValue("DocRemValidity", objDoc, "");

                        if (!GFunc.IsNE(objCon.CRemPayment))
                            GFunc.SetPropertyValue("DocRemPayment", objDoc, objCon.CRemPayment);
                        else
                            GFunc.SetPropertyValue("DocRemPayment", objDoc, "");
                    }
                    else if (GFunc.CompareString(SetCV, "Vendor"))
                    {
                        if (!GFunc.IsNE(objCon.VRemDelivery))
                            GFunc.SetPropertyValue("DocRemDelivery", objDoc, objCon.VRemDelivery);
                        else
                            GFunc.SetPropertyValue("DocRemDelivery", objDoc, "");

                        if (!GFunc.IsNE(objCon.VRemPrice))
                            GFunc.SetPropertyValue("DocRemPrice", objDoc, objCon.VRemPrice);
                        else
                            GFunc.SetPropertyValue("DocRemPrice", objDoc, "");

                        if (!GFunc.IsNE(objCon.VRemValidity))
                            GFunc.SetPropertyValue("DocRemValidity", objDoc, objCon.VRemValidity);
                        else
                            GFunc.SetPropertyValue("DocRemValidity", objDoc, "");

                        if (!GFunc.IsNE(objCon.VRemPayment))
                            GFunc.SetPropertyValue("DocRemPayment", objDoc, objCon.VRemPayment);
                        else
                            GFunc.SetPropertyValue("DocRemPayment", objDoc, "");
                    }
                }

                #endregion

                #region Set Address
                string defaultBAddrID = string.Empty;
                string defaultSAddrID = string.Empty;

                if (GFunc.CompareString(SetCV, "Customer"))
                {
                    if (SetBAddress)
                    {
                        defaultBAddrID = (string)objCon.CDefaultBillAddr; //40=Customer or Vendor
                        DocComUtility.Address_Set(cn, objDoc, (int)GEnum.AddrLinkType.CustomerOrVendor, (int)conKey, defaultBAddrID, SetBAddress, SetSAddress);
                    }
                    if (SetSAddress)
                    {
                        defaultSAddrID = (string)objCon.CDefaultShipAddr; //40=Customer or Vendor
                        DocComUtility.Address_Set(cn, objDoc, (int)GEnum.AddrLinkType.CustomerOrVendor, (int)conKey, defaultSAddrID, false, SetSAddress);
                    }
                }
                else if (GFunc.CompareString(SetCV, "Vendor"))
                {
                    if (SetBAddress)
                    {
                        defaultBAddrID = (string)objCon.VDefaultBillAddr; //40=Customer or Vendor
                        DocComUtility.Address_Set(cn, objDoc, (int)GEnum.AddrLinkType.CustomerOrVendor, (int)conKey, defaultBAddrID, SetBAddress, SetSAddress);
                    }
                    if (SetSAddress)
                    {
                        defaultSAddrID = (string)objCon.VDefaultShipAddr; //40=Customer or Vendor
                        DocComUtility.Address_Set(cn, objDoc, (int)GEnum.AddrLinkType.CustomerOrVendor, (int)conKey, defaultSAddrID, false, SetSAddress);
                    }
                }
                #endregion

                #region Reset ShipName
                if (SetShipInfor)
                {
                    if (GFunc.CompareString(SetCV, "Vendor")==false) //do not need to do for Vendor
                    {
                        GFunc.SetPropertyValue("DocShipName", objDoc, null);
                        GFunc.SetPropertyValue("DocShipMark", objDoc, null);
                    }                   
                }
                #endregion

                #region Set Tax Infor
                if (SetTaxInfor)
                {
                    if (GFunc.CompareString(SetCV, "Customer"))
                    {
                        GFunc.SetPropertyValue("DocTaxGrpKey", objDoc, objCon.CTaxGrpKey);
                        GFunc.SetPropertyValue("DocTaxGrpRate", objDoc, DocComUtility.TaxGrpRate_Get(cn, objCon.CTaxGrpKey, docDate));                        
                    }
                    else if (GFunc.CompareString(SetCV, "Vendor"))
                    {
                        GFunc.SetPropertyValue("DocTaxGrpKey", objDoc, objCon.VTaxGrpKey);
                        GFunc.SetPropertyValue("DocTaxGrpRate", objDoc, DocComUtility.TaxGrpRate_Get(cn, objCon.VTaxGrpKey, docDate));
                    }
                }
                #endregion

                #region Set Currency Infor
                if (SetCurr)
                {
                    if (GFunc.CompareString(SetCV, "Customer"))
                    {
                        GFunc.SetPropertyValue("DocCurrKey", objDoc, objCon.CCurrkey);
                        if (CurrencyInfor_Set(cn, objDoc) == false)
                            return false;
                    }
                    else if (GFunc.CompareString(SetCV, "Vendor"))
                    {
                        GFunc.SetPropertyValue("DocCurrKey", objDoc, objCon.VCurrkey);
                        if (CurrencyInfor_Set(cn, objDoc) == false)
                            return false;
                    }
                }
                #endregion

                #region Reset Job Infor
                //if (SetJob)
                //{
                //    GFunc.SetPropertyValue("DocJobKey", objDoc, 0);
                //    GFunc.SetPropertyValue("DocJobPhaseKey", objDoc, 0);
                //    GFunc.SetPropertyValue("DocJobTaskKey", objDoc, 0);
                //    GFunc.SetPropertyValue("DocJobCostTypeKey", objDoc, 0);
                //}

                #endregion

                #region Set Acc AR/AP Infor
                if (SetAccAR)
                    GFunc.SetPropertyValue("DocAccARKey", objDoc, objCon.CAccKey);
                if (SetAccAP)
                    GFunc.SetPropertyValue("DocAccAPKey", objDoc, objCon.VAccKey);
                #endregion

                if (SetApplyList)
                {
                    //if AccKey (CurrKey) == ConKey(CurrKey) set DocAccKey = null
                    if (!GFunc.IsNEZ(GFunc.GetPropertyValue("DocAccKey", objDoc)))
                    {
                        MSTAcc obj = MSTAcc.Get(GFunc.GetIntPropertyValue("DocAccKey", objDoc));
                        if (obj.AccCurrKey != GFunc.GetIntPropertyValue("DocCurrKey", objDoc))
                        {
                            GFunc.SetPropertyValue("DocAccKey", objDoc, null);
                        }
                    }
                }
                if (setTransGrp)
                {
                    //mts updated on 13 Jan 2022
                    List<SqlParameter> parmList = new List<SqlParameter>();
                    parmList.Add(new SqlParameter("@Option", 2));
                    parmList.Add(new SqlParameter("@EmKey", objCon.CEMKey.Value));
                    DataTable dt = GFunc.ExecuteProc("[MSTUNIT_GET]", parmList);
                    if (dt.Rows.Count > 0)
                    {
                        GFunc.SetPropertyValue("DocTranGrpKey", objDoc, dt.Rows[0]["HeadSalesKey"]);
                        GFunc.SetPropertyValue("Custom4", objDoc, dt.Rows[0]["Team"]);
                        GFunc.SetPropertyValue("Custom5", objDoc, dt.Rows[0]["Team"]);
                    }
                    else
                    {
                        MSTAccTranGrp t = MSTAccTranGrp.Get(objCon.ConChildren.Value);
                        if (t != null)
                            GFunc.SetPropertyValue("DocTranGrpKey", objDoc, t.TranGrpKey);
                    }
                }

                #region SetOverallDefaultDis  //Mic Check ; Jack Added 29 Oct 2012
                
                if (SetOverallDefaultDis)
                {
                    if (true) //GFunc.GetDecimalPropertyValue("DocOverallDisRate", objDoc)<=0)
                    {
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Delivery:
                            case (int)GEnum.SystemCode.Purchase_Order:
                                if(objCon.VOverallDefaultDis>0)
                                    GFunc.SetPropertyValue("DocOverallDisRate", objDoc, objCon.VOverallDefaultDis);
                                break;
                            case (int)GEnum.SystemCode.Delivery_Order:
                            case (int)GEnum.SystemCode.Sales_Invoice:
                            case (int)GEnum.SystemCode.Quotation:
                            case (int)GEnum.SystemCode.Sales_Order:
                            case (int)GEnum.SystemCode.Reserve_Order:
                                if(objCon.COverallDefaultDis>0)
                                    GFunc.SetPropertyValue("DocOverallDisRate", objDoc, objCon.COverallDefaultDis);
                                break;
                            default:
                                break;
                        }
                    }

                }
                
                #endregion

                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Delivery_Order)
                    if (objCon.Custom1.Trim().ToUpper().Equals("NR") && GFunc.IsNE(GFunc.GetPropertyValue("DocCustPONum", objDoc)))
                        GFunc.SetPropertyValue("DocCustPONum", objDoc, "NR");

                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed

        public static bool DocDate_CustomUpdate(Document objDoc)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocDate_CustomUpdate(cn, objDoc);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocDate_CustomUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocDate_CustomUpdate(cn, objDoc, docDet);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocDate_CustomUpdate(SqlConnection cn, Document objDoc)
        {
            try
            {
                if (DocDate_CustomUpdate_DependentSet(cn, objDoc))
                    return DocComUtility.CalForm(cn, objDoc, false);
                else
                    return false;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocDate_CustomUpdate(SqlConnection cn, Document objDoc, Hashtable docDet)
        {
            try
            {
                if (DocDate_CustomUpdate_DependentSet(cn, objDoc))
                    return DocComUtility.CalForm(cn, objDoc, docDet, true, false);
                else
                    return false;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private static bool DocDate_CustomUpdate_DependentSet(SqlConnection cn, Document objDoc)
        {
            int? DocTaxGrpKey = 0;
            decimal? TaxGrpRate = 0;

            try
            {
                objDoc.DocDate = GFunc.NEDateTime(objDoc.DocDate, DateTime.Today);

                #region set DocDateValid. DocDateOrg
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                        if (GFunc.IsNE(GFunc.GetPropertyValue("DocDateValid", objDoc)))
                        {
                            DateTime dt = DateTime.Today.AddDays(GFunc.NEInt(SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultDateValid, cn), 0));
                            GFunc.SetPropertyValue("DocDateValid", objDoc, dt);
                        }
                        break;

                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        if (GFunc.IsNE(GFunc.GetPropertyValue("DocDateOrg", objDoc)))
                            GFunc.SetPropertyValue("DocDateOrg", objDoc, objDoc.DocDate);
                        break;
                }
                #endregion

                #region Set Chqeue Date
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Sales_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Purchase_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        if (GFunc.IsNE(GFunc.GetPropertyValue("DocChqDate", objDoc)))
                            GFunc.SetPropertyValue("DocChqDate", objDoc, GFunc.GetPropertyValue("DocDate", objDoc));
                        break;
                }
                #endregion

                #region Set DocTaxRate
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        DocTaxGrpKey = GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc);
                        if (!GFunc.IsNE(DocTaxGrpKey))
                        {
                            TaxGrpRate = DocComUtility.TaxGrpRate_Get(cn, DocTaxGrpKey, objDoc.DocDate);
                            GFunc.SetPropertyValue("DocTaxGrpRate", objDoc, TaxGrpRate);
                        }                        
                        break;
                }
                #endregion

                //set Currency and Country rate
                return CurrencyInfor_Set(cn, objDoc);
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocDateValid_CustomUpdate(Document objDoc, DateTime? ctrlValue)
        {
            try
            {
                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Quotation)
                {
                    if (GFunc.IsNE(ctrlValue))
                    {
                        MsgBox.Show("Valid date cannot be empty");
                        return false;
                    }
                    if (ctrlValue < objDoc.DocDate)
                    {
                        MsgBox.Show(MsgID.Document.DocDateValidIsLessThanDocDate);
                        return false;
                    }
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocDateOrg_CustomUpdate(Document objDoc, DateTime? ctrlValue)
        {
            try
            {
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        if (GFunc.IsNE(ctrlValue))
                        {
                            MsgBox.Show("Please use a valid date");
                            return false;
                        }
                        break;
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed

        public static bool DocDeptKey_CustomUpdate(Document objDoc)
        {
            try
            {
                int? docDeptKey = GFunc.NEInt(GFunc.GetPropertyValue("DocDeptKey", objDoc), 0);
                GFunc.SetPropertyValue("DocDeptKey", objDoc, docDeptKey);
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocDeptKey_CustomUpdate(Document objDoc, Hashtable docDet)
        {

            try
            {
                UltraGrid grdItm = null;
                UltraGrid grdExp = null;
                DataTable dtItm = null;
                DataTable dtExp = null;
                int? docDeptKey = 0;

                if (DocComUtility.DocDetail_Get((int)objDoc.DocCodeKey, docDet, ref grdItm, ref grdExp) == false)
                    return false;

                if (DocComUtility.DocDetail_Get((int)objDoc.DocCodeKey, docDet, ref dtItm, ref dtExp) == false)
                    return false;
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Deposit:
                        docDeptKey = GFunc.NEInt(GFunc.GetPropertyValue("DocDeptKey", objDoc), 0);
                        GFunc.SetPropertyValue("DocDeptKey", objDoc, docDeptKey);
                        break;
                    default:
                        return true;
                }

                #region Set department cell default value and set all detail department value
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Journal:
                        if (grdItm != null)
                        {
                            grdItm.DisplayLayout.Bands[0].Columns["ItmDeptKey"].DefaultCellValue = GFunc.NEInt(docDeptKey, 0);

                            if (dtItm.Rows.Count > 0)
                            {
                                GEnum.MsgBoxButton userAction = GEnum.MsgBoxButton.Yes;
                                if (GVar.DocUpdateOption[GVar.DeptUpdateOption] != null)
                                    userAction = MsgBox.Show(MsgID.Document.SetDetDepartmentWhenHDRChange, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No);

                                if (userAction == GEnum.MsgBoxButton.Yes)
                                    foreach (DataRow row in dtItm.Rows)
                                    {
                                        if (GFunc.IsPostingItmType(row["ItmType"]))
                                            row["ItmDeptKey"] = GFunc.NEInt(docDeptKey, 0);
                                    }
                            }
                        }
                        break;

                    case (int)GEnum.SystemCode.Purchase_Plan:
                        if (grdItm != null)
                        {
                            grdItm.DisplayLayout.Bands[0].Columns["ItmDeptKey"].DefaultCellValue = GFunc.NEInt(docDeptKey, 0);

                            if (dtItm.Rows.Count > 0)
                            {
                                GEnum.MsgBoxButton userAction = GEnum.MsgBoxButton.Yes;
                                if (GVar.DocUpdateOption[GVar.DeptUpdateOption] != null)
                                    userAction = MsgBox.Show(MsgID.Document.SetDetDepartmentWhenHDRChange, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No);

                                if (userAction == GEnum.MsgBoxButton.Yes)
                                    foreach (DataRow row in dtItm.Rows)
                                    {
                                        row["ItmDeptKey"] = GFunc.NEInt(docDeptKey, 0);
                                    }
                            }
                        }
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                        if (grdItm != null)
                            grdItm.DisplayLayout.Bands[0].Columns["ItmDeptKey"].DefaultCellValue = GFunc.NEInt(docDeptKey, 0);

                        if (grdExp != null)
                            grdExp.DisplayLayout.Bands[0].Columns["ExpDeptKey"].DefaultCellValue = GFunc.NEInt(docDeptKey, 0);

                        if ((grdItm != null && dtItm.Rows.Count > 0) || (grdExp != null && dtExp.Rows.Count > 0))
                        {
                            GEnum.MsgBoxButton userAction = GEnum.MsgBoxButton.Yes;
                            if (GVar.DocUpdateOption[GVar.DeptUpdateOption] != null)
                                userAction = MsgBox.Show(MsgID.Document.SetDetDepartmentWhenHDRChange, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No);

                            if (userAction == GEnum.MsgBoxButton.Yes)
                            {
                                if (grdItm != null)
                                {
                                    foreach (DataRow row in dtItm.Rows)
                                    {
                                        if (GFunc.IsPostingItmType(row["ItmType"]))
                                            row["ItmDeptKey"] = GFunc.NEInt(docDeptKey, 0);
                                    }
                                }
                                if (grdExp != null)
                                {
                                    foreach (DataRow row in dtExp.Rows)
                                    {
                                        row["ExpDeptKey"] = GFunc.NEInt(docDeptKey, 0);
                                    }
                                }
                            }

                        }
                        break;

                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        if (grdExp != null)
                        {
                            grdExp.DisplayLayout.Bands[0].Columns["ExpDeptKey"].DefaultCellValue = GFunc.NEInt(docDeptKey, 0);

                            if (dtExp != null)
                            {
                                if (dtExp.Rows.Count > 0)
                                {
                                    GEnum.MsgBoxButton userAction = GEnum.MsgBoxButton.Yes;
                                    if (GVar.DocUpdateOption[GVar.DeptUpdateOption] == null)
                                        userAction = MsgBox.Show(MsgID.Document.SetDetDepartmentWhenHDRChange, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No);

                                    if (userAction == GEnum.MsgBoxButton.Yes)
                                        foreach (DataRow row in dtExp.Rows)
                                        {
                                            row["ExpDeptKey"] = GFunc.NEInt(docDeptKey, 0);
                                        }
                                }
                            }
                        }
                        break;

                    case (int)GEnum.SystemCode.Deposit:
                        if (grdItm != null)
                        {
                            grdItm.DisplayLayout.Bands[0].Columns["ItmDocDeptKey"].DefaultCellValue = GFunc.NEInt(docDeptKey, 0);

                            if (dtItm != null && dtItm.Rows.Count > 0)
                            {
                                GEnum.MsgBoxButton userAction = GEnum.MsgBoxButton.Yes;
                                if (GVar.DocUpdateOption[GVar.DeptUpdateOption] == null)
                                    userAction = MsgBox.Show(MsgID.Document.SetDetDepartmentWhenHDRChange, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No);

                                if (userAction == GEnum.MsgBoxButton.Yes)
                                    foreach (DataRow row in dtItm.Rows)
                                    {
                                        if ((int)row["ItmDocDC"] > 0)
                                        {
                                            row["ItmDocDeptKey"] = GFunc.NEInt(docDeptKey, 0);
                                        }
                                    }
                            }
                        }
                        break;
                }
                #endregion

                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocDiscountRate_CustomUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocDiscountRate_CustomUpdate(cn, objDoc, docDet);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocDiscountRate_CustomUpdate(SqlConnection cn, Document objDoc, Hashtable docDet)
        {
            try
            {
                int defODAcc = 0;
                string id = string.Empty;
                string des = string.Empty;

                decimal docOverallDisRate = GFunc.NEDec(GFunc.GetDecimalPropertyValue("DocOverallDisRate", objDoc), 0);
                GFunc.SetPropertyValue("DocOverallDisRate", objDoc, docOverallDisRate);

                if (docOverallDisRate != 0)
                {
                    if (GFunc.IsNEZ(GFunc.GetPropertyValue("DocOverallDisAcc", objDoc)))
                    {
                        switch ((int)objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Quotation:
                            case (int)GEnum.SystemCode.Sales_Order:
                            case (int)GEnum.SystemCode.Reserve_Order:
                            case (int)GEnum.SystemCode.Delivery_Order:
                            case (int)GEnum.SystemCode.Sales_Invoice:
                            case (int)GEnum.SystemCode.Sales_Debit_Note:
                            case (int)GEnum.SystemCode.Sales_Credit_Note:
                            case (int)GEnum.SystemCode.Cash_Sale:
                            case (int)GEnum.SystemCode.Cash_Debit_Note:
                            case (int)GEnum.SystemCode.Cash_Credit_Note:
                                defODAcc = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultOverallDiscountAccountAR);
                                break;

                            default:
                                //case (int)GEnum.SystemCode.Purchase_Order:
                                //case (int)GEnum.SystemCode.Purchase_Delivery:
                                //case (int)GEnum.SystemCode.Purchase_Invoice:
                                //case (int)GEnum.SystemCode.Purchase_Debit_Note:
                                //case (int)GEnum.SystemCode.Purchase_Credit_Note:
                                defODAcc = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultOverallDiscountAccountAP);
                                break;
                        }

                        if (GFunc.IsNEZ(defODAcc))
                        {
                            MsgBox.Show(cn, MsgID.Document.DefaultDiscAccNotSet);
                            return false;
                        }
                        else
                        {
                            if (GFunc.Record_GetKey(GEnum.RecAccessTypeAcc.AccKey, defODAcc.ToString(), ref id, ref des, true) > 0)
                            {
                                GFunc.SetPropertyValue("DocOverallDisAcc", objDoc, defODAcc);
                                GFunc.SetPropertyValue("DocOverallDisAccID", objDoc, id);
                            }
                            else
                                return false;
                        }
                    }
                }
                return DocComUtility.CalForm(cn, objDoc, docDet, true, false);
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocDiscountAmt_CustomUpdate(Document objDoc, Hashtable docDet)
        {
            #region vars for cal Tax
            decimal? TotalTaxAmtF = 0;  //Total of ItmTaxGrpAmtF
            decimal? TotalTaxAmtL = 0;  //Total of ItmTaxGrpAmtL
            #endregion

            #region vars for cal Discount
            int? DocTaxKey = 0;
            decimal? DocTaxRate = 0;
            decimal? DocCountryRate = 1;
            decimal? DocCurrRate = 1;
            decimal? DocDisRate = 0;
            decimal? DocTotalAfterDis = 0;
            decimal? DocOverallDisAmt = 0;
            decimal? DocTaxAmt = 0;
            decimal? DocTaxAmtL = 0;
            decimal? DocSubTotal = 0;
            #endregion

            #region vars for cal Additional Cost
            decimal? DocAddFreightF = 0;            //Additional Cost - Freight in Original currency
            decimal? DocAddInsuranceF = 0;          //Additional Cost - Insurrance in Original currency
            decimal? DocAddOthersF = 0;             //Additional Cost - Other Cost in Original currency
            decimal? DocAddCostLumpSumF = 0;        //Additional Cost - Total of Freight + Insurrance + Other Cost in Original currency
            decimal? DocAddCostLumpSumRate = 1;     //Additional Cost - LumpSum exchange rate to Home currency
            decimal? DocAddCostDocHomePercent = 0;  //Additional Cost - Percentage of Document Home amount to be added to additional cost
            decimal? DocAddCostOtherH = 0;          //(LumpSum x LumpSumRate) + (HomePercent x DocHome)
            decimal? DocAddCostChargesH = 0;        //Total of document Charge Type items AmountF x CurrRate
            decimal? DocAddCostTotalH = 0;          //CostOthersH + CostChargesH
            decimal? DocAddCostItmAmtF = 0;         //Total of document ItmAmtF for all Stock, Non Stock and Service INType
            decimal? DocAddCostFactor = 0;          //If CostItmAmtF = 0 Then 0 Else CostTotalH / CostItmAmtF
            decimal? ItmAddCostF = 0;               //Additional Cost to be added to Item Price in original currency (ItmAddAmtH / ItmQty) x CurrRate
            decimal? ItmAddCostH = 0;               //Additional Cost to be added to Item Price in Home currency ItmAddAmtH / ItmQty
            decimal? ItmAddAmtF = 0;                //Additional Amount to be added to Item Amount in original currency ItmAddAmtH x CurrRate
            decimal? ItmAddAmtH = 0;                //Additional Amount to be added to Item Amount in Home currency ItmAmt x AddCostFactor (Only use for Stock,NonStock,Services)

            #endregion

            try
            {
                int defODAcc;
                string id = string.Empty;
                string des = string.Empty;
                DataTable dtItm = null;

                DocCountryRate = GFunc.NEDec(GFunc.GetDecimalPropertyValue("DocCountryRate", objDoc), 1);
                DocOverallDisAmt = GFunc.NEDec(GFunc.GetDecimalPropertyValue("DocOverallDisAmt", objDoc), 0);
                DocOverallDisAmt = GFunc.RndC(DocOverallDisAmt, GVar.RndDecs.Amtpt);
                GFunc.SetPropertyValue("DocOverallDisAmt", objDoc, DocOverallDisAmt);

                if (DocOverallDisAmt != 0)
                {
                    if (GFunc.IsNEZ(GFunc.GetPropertyValue("DocOverallDisAcc", objDoc)))
                    {
                        switch ((int)objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Quotation:
                            case (int)GEnum.SystemCode.Sales_Order:
                            case (int)GEnum.SystemCode.Reserve_Order:
                            case (int)GEnum.SystemCode.Delivery_Order:
                            case (int)GEnum.SystemCode.Sales_Invoice:
                            case (int)GEnum.SystemCode.Sales_Debit_Note:
                            case (int)GEnum.SystemCode.Sales_Credit_Note:
                            case (int)GEnum.SystemCode.Cash_Sale:
                            case (int)GEnum.SystemCode.Cash_Debit_Note:
                            case (int)GEnum.SystemCode.Cash_Credit_Note:
                                defODAcc = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultOverallDiscountAccountAR);
                                break;

                            default:
                                //case (int)GEnum.SystemCode.Purchase_Order:
                                //case (int)GEnum.SystemCode.Purchase_Delivery:
                                //case (int)GEnum.SystemCode.Purchase_Invoice:
                                //case (int)GEnum.SystemCode.Purchase_Debit_Note:
                                //case (int)GEnum.SystemCode.Purchase_Credit_Note:
                                defODAcc = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultOverallDiscountAccountAP);
                                break;
                        }

                        if (GFunc.IsNEZ(defODAcc))
                        {
                            MsgBox.Show(MsgID.Document.DefaultDiscAccNotSet);
                            return false;
                        }
                        else
                        {
                            if (GFunc.Record_GetKey(GEnum.RecAccessTypeAcc.AccKey, defODAcc.ToString(), ref id, ref des, true) > 0)
                            {
                                GFunc.SetPropertyValue("DocOverallDisAcc", objDoc, defODAcc);
                                GFunc.SetPropertyValue("DocOverallDisAccID", objDoc, id);
                            }
                            else
                                return false;
                        }
                    }
                }

                #region looping all detail items to calculate TotalTaxAmtF, DocAddCostItmAmtF, DocAddCostChargesH
                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref dtItm);

                //Sort Ascending and Apply Filter for Parent Items only                 
                DataRow[] drs = dtItm.Select("LineLinkKey=0", "ItmSN ASC");

                foreach (DataRow row in drs)
                {
                    switch (GFunc.GetINTypeGroup(GFunc.NEInt(row["ItmType"], 0)))
                    {
                        case (int)GEnum.INTypeGrp.Stock:
                        case (int)GEnum.INTypeGrp.Non_Stock:
                            DocAddCostItmAmtF = DocAddCostItmAmtF + GFunc.NEDec(row["ItmAmtF"], 0);
                            TotalTaxAmtF = TotalTaxAmtF + GFunc.NEDec(row["ItmTaxGrpAmtF"], 0);
                            TotalTaxAmtL = TotalTaxAmtL + GFunc.NEDec(row["ItmTaxGrpAmtL"], 0);
                            break;

                        case (int)GEnum.INTypeGrp.Charges:
                            DocAddCostChargesH = DocAddCostChargesH + GFunc.NEDec(row["ItmAmtH"], 0);
                            TotalTaxAmtF = TotalTaxAmtF + GFunc.NEDec(row["ItmTaxGrpAmtF"], 0);
                            TotalTaxAmtL = TotalTaxAmtL + GFunc.NEDec(row["ItmTaxGrpAmtL"], 0);
                            break;

                        case (int)GEnum.INTypeGrp.Discount:
                            TotalTaxAmtF = TotalTaxAmtF + GFunc.NEDec(row["ItmTaxGrpAmtF"], 0);
                            TotalTaxAmtL = TotalTaxAmtL + GFunc.NEDec(row["ItmTaxGrpAmtL"], 0);
                            break;
                    }

                }
                #endregion

                #region Calculate Document Total
                TotalTaxAmtF = GFunc.RndC(TotalTaxAmtF, GVar.RndDecs.Amtpt);
                TotalTaxAmtL = GFunc.RndC(TotalTaxAmtL, GVar.RndDecs.Amtpt);
                DocTaxKey = GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc);
                DocTaxRate = GFunc.NEDec(GFunc.GetDecimalPropertyValue("DocTaxGrpRate", objDoc), 0);
                DocCurrRate = GFunc.NEDec(GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc), 1);
                DocTaxAmt = GFunc.NEDec(GFunc.GetDecimalPropertyValue("DocTaxTotal", objDoc), 0);
                DocSubTotal = GFunc.NEDec(GFunc.GetDecimalPropertyValue("DocSubTotal", objDoc), 0);
                DocOverallDisAmt = GFunc.NEDec(GFunc.GetDecimalPropertyValue("DocOverallDisAmt", objDoc), 0);

                DocDisRate = GFunc.RndDC(DocOverallDisAmt, DocSubTotal, GVar.RndDecs.Prcpt) * 100M;
                DocTaxAmt = TotalTaxAmtF - GFunc.RndDC(TotalTaxAmtF * DocDisRate, 100M, GVar.RndDecs.Amtpt);
                DocTaxAmtL = TotalTaxAmtL - GFunc.RndDC(TotalTaxAmtL * DocDisRate, 100M, GVar.RndDecs.Amtpt);
                DocTotalAfterDis = DocSubTotal - DocOverallDisAmt;

                GFunc.SetPropertyValue("DocTotalAfterDis", objDoc, DocTotalAfterDis);
                GFunc.SetPropertyValue("DocOverallDisRate", objDoc, DocDisRate);
                GFunc.SetPropertyValue("DocTaxTotal", objDoc, DocTaxAmt);

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        REFTaxGrp objTaxGrp = REFTaxGrp.Get(GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc));
                        if (objTaxGrp.GSTCustom == true)
                            GFunc.SetPropertyValue("DocTotal", objDoc, DocTotalAfterDis);
                        else
                            GFunc.SetPropertyValue("DocTotal", objDoc, DocTotalAfterDis + DocTaxAmt);
                        break;

                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                        GFunc.SetPropertyValue("DocTotal", objDoc, DocTotalAfterDis + DocTaxAmt);
                        break;
                }

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                        GFunc.SetPropertyValue("DocGrand", objDoc, DocTotalAfterDis + DocTaxAmt);
                        GFunc.SetPropertyValue("DocHome", objDoc, GFunc.RndC(GFunc.GetDecimalPropertyValue("DocGrand", objDoc) * DocCurrRate, GVar.RndDecs.Amtpt));
                        break;

                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                        GFunc.SetPropertyValue("DocGrand", objDoc, DocTotalAfterDis + DocTaxAmt - GFunc.GetDecimalPropertyValue("DocPaidAmtF", objDoc));
                        GFunc.SetPropertyValue("DocHome", objDoc, GFunc.RndC(GFunc.GetDecimalPropertyValue("DocGrand", objDoc) * DocCurrRate, GVar.RndDecs.Amtpt));
                        break;

                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Delivery:   //not applicable
                        GFunc.SetPropertyValue("DocGrand", objDoc, DocTotalAfterDis + DocTaxAmt);
                        GFunc.SetPropertyValue("DocHome", objDoc, GFunc.RndC(GFunc.GetDecimalPropertyValue("DocGrand", objDoc) * DocCurrRate, GVar.RndDecs.Amtpt));
                        break;

                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        GFunc.SetPropertyValue("DocGrand", objDoc, GFunc.GetDecimalPropertyValue("DocTotal", objDoc) - GFunc.GetDecimalPropertyValue("DocPaidAmtF", objDoc));
                        GFunc.SetPropertyValue("DocHome", objDoc, GFunc.RndC(GFunc.GetDecimalPropertyValue("DocGrand", objDoc) * DocCurrRate, GVar.RndDecs.Amtpt));
                        break;
                }

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        GFunc.SetPropertyValue("DocTaxTotalLocal", objDoc, DocTaxAmtL);
                        break;
                }

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:

                        #region Calculate Additional Cost
                        DocAddFreightF = GFunc.NEDec(GFunc.GetDecimalPropertyValue("DocAddFreight", objDoc), 0);
                        DocAddInsuranceF = GFunc.NEDec(GFunc.GetDecimalPropertyValue("DocAddInsurance", objDoc), 0);
                        DocAddOthersF = GFunc.NEDec(GFunc.GetDecimalPropertyValue("DocAddOthers", objDoc), 0);
                        DocAddCostLumpSumRate = GFunc.NEDec(GFunc.GetDecimalPropertyValue("DocAddCostLumpSumRate", objDoc), 0);
                        DocAddCostDocHomePercent = GFunc.NEDec(GFunc.GetDecimalPropertyValue("DocAddCostDocHomePercent", objDoc), 0);

                        DocAddCostLumpSumF = DocAddFreightF + DocAddInsuranceF + DocAddOthersF;
                        DocAddCostOtherH = GFunc.RndC(DocAddCostLumpSumF * DocAddCostLumpSumRate, GVar.RndDecs.Amtpt);
                        DocAddCostOtherH = DocAddCostOtherH + GFunc.RndC(DocAddCostDocHomePercent * GFunc.GetDecimalPropertyValue("DocTotalAfterDis", objDoc) * DocCurrRate, GVar.RndDecs.Amtpt);
                        DocAddCostTotalH = DocAddCostOtherH + DocAddCostChargesH;
                        if (DocAddCostItmAmtF == 0)
                            DocAddCostFactor = 0;
                        else
                            DocAddCostFactor = GFunc.RndDC(DocAddCostTotalH, DocAddCostItmAmtF, GVar.RndDecs.Prcpt);
                        #endregion

                        #region looping to all detail items to set additional cost
                        DataRow[] drItms = dtItm.Select("LineLinkKey=0", "ItmSN ASC");

                        foreach (DataRow row in drItms)
                        {
                            switch (GFunc.GetINTypeGroup(GFunc.NEInt(row["ItmType"], 0)))
                            {
                                case (int)GEnum.INTypeGrp.Stock:
                                case (int)GEnum.INTypeGrp.Non_Stock:
                                    //Calculation
                                    ItmAddAmtH = GFunc.RndDC(GFunc.NEDec(row["ItmAmtF"], 0), DocAddCostItmAmtF, GVar.RndDecs.Prcpt);//Ask Mic
                                    ItmAddAmtH = GFunc.RndC(ItmAddAmtH * DocAddCostOtherH, GVar.RndDecs.Amtpt);
                                    ItmAddAmtF = GFunc.RndDC(ItmAddAmtH, DocCurrRate, GVar.RndDecs.Amtpt);
                                    ItmAddCostH = GFunc.RndDC(ItmAddAmtH, GFunc.NEDec(row["ItmQty"], 0), GVar.RndDecs.Prcpt);//Ask Mic
                                    ItmAddCostF = GFunc.RndDC(ItmAddAmtF, GFunc.NEDec(row["ItmQty"], 0), GVar.RndDecs.Prcpt);//Ask Mic

                                    //Set values to grid
                                    row["ItmAddAmtF"] = GFunc.NEDec(ItmAddAmtF, 0);
                                    row["ItmAddAmtH"] = GFunc.NEDec(ItmAddAmtH, 0);
                                    row["ItmAddCostF"] = GFunc.NEDec(ItmAddCostF, 0);
                                    row["ItmAddCostH"] = GFunc.NEDec(ItmAddCostF, 0);
                                    break;
                            }
                        }
                        #endregion

                        #region set values to document
                        GFunc.SetPropertyValue("DocAddCostLumpSum", objDoc, DocAddCostLumpSumF);
                        GFunc.SetPropertyValue("DocAddCostOthersH", objDoc, DocAddCostOtherH);
                        GFunc.SetPropertyValue("DocAddCostChargesH", objDoc, DocAddCostChargesH);
                        GFunc.SetPropertyValue("DocAddCostTotalH", objDoc, DocAddCostTotalH);
                        GFunc.SetPropertyValue("DocAddCostItmAmtF", objDoc, DocAddCostItmAmtF);
                        GFunc.SetPropertyValue("DocAddCostFactor", objDoc, DocAddCostFactor);
                        #endregion
                        break;
                }
                #endregion

                return DocComUtility.CalForm(objDoc, docDet, true, false);

            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed        
        public static bool DocID_CustomUpdate(Document objDoc, string CtrlValue)
        {
            try
            {
                if (objDoc.IsNew == false)
                {
                    if (GFunc.IsNE(CtrlValue))
                    {
                        MsgBox.Show("Document number cannot be empty");
                        return false;
                    }
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocJobID_btnClick(Document objDoc, TAUtil.TAComboBox ctrl, string ListSettingID, ref int key, ref string id, ref int phaseKey, ref int costTypeKey, ref int taskKey)
        {
            try
            {
                //Note: Currently the id, Des, Phase,task,CostType is not used, in furture we might need it
                string des = string.Empty;
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Sales_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Adjustment:

                        if (DocHDRUtil.EditorButton_Popup(objDoc, ctrl, ListSettingID, (int)GEnum.PopupType.JobID, ref key, ref id, ref des, ref phaseKey, ref costTypeKey, ref taskKey))
                        {
                            GFunc.SetPropertyValue("DocJobKey", objDoc, key);
                            return DocJobKey_CustomUpdate(objDoc);
                        }
                        return true;
                }
                return false;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocJobKey_CustomUpdate(Document objDoc)
        {
            try
            {
                int jobkey = GFunc.NEInt(GFunc.GetIntPropertyValue("DocJobKey", objDoc), 0);

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Sales_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Adjustment:
                        if (GFunc.IsNEZ(jobkey))
                        {
                            GFunc.SetPropertyValue("DocJobKey", objDoc, 0);
                            GFunc.SetPropertyValue("DocJobPhaseKey", objDoc, 0);
                            GFunc.SetPropertyValue("DocJobTaskKey", objDoc, 0);
                            GFunc.SetPropertyValue("DocJobCostTypeKey", objDoc, 0);
                        }
                        break;
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocShipName_CustomUpdate(Document objDoc)
        {
            try
            {
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                        int docConKey = GFunc.NEInt(GFunc.GetPropertyValue("DocConKey", objDoc), 0);
                        string docShipName = GFunc.NEStr(GFunc.GetPropertyValue("DocShipName", objDoc), string.Empty);
                        GFunc.SetPropertyValue("DocShipMark", objDoc, ShipMark_GetLast(docConKey, docShipName).ToString());
                        
                        //Set BillName from MST_Ship --- Mic Check; Jack Added 8 Nov 2012                        
                        MSTShipName shipObj = MSTShipName.Get(docShipName,docConKey); //Should we check for customer or vendor not to be empty?
                        GFunc.SetPropertyValue("DocRem", objDoc, shipObj.BillName);
                        break;
                }
                
               

                return true;

            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocShipMark_btnClick(Document objDoc)
        {
            try
            {
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                        int docConKey = (int)GFunc.GetPropertyValue("DocConKey", objDoc);
                        string docShipName = GFunc.NEStr(GFunc.GetPropertyValue("DocShipName", objDoc), "");
                        GFunc.SetPropertyValue("DocShipMark", objDoc, ShipMark_GetNew(docConKey, docShipName).ToString());
                        break;
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed

        public static bool DocTaxGrpKey_CustomUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocTaxGrpKey_CustomUpdate(cn, objDoc, docDet);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocTaxGrpKey_CustomUpdate(SqlConnection cn, Document objDoc, Hashtable docDet)
        {
            try
            {
                REFTaxGrp objTaxGrp = null;
                int? CountryCurrencyKey = 1;
                int? DocTaxGrpKey = 0;
                decimal? TaxGrpRate = 0;
                UltraGrid grdItm = null;
                UltraGrid grdExp = null;
                DataTable dtItm = null;
                DataTable dtExp = null;

                /* added by YST on 2023/01/18 to allow to change different ItemGST */
                bool changeItemTax = true;
                int ItmTaxGrpKeyCount = 0;

                /* added by YST to auto fill Empty Tax if DirectShipment SO */
                if ( objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Order && (objDoc.DocTypeNm == "Direct Shipment" || objDoc.DocTypeNm == "Sales Order - VN"))
                {
                    GFunc.SetPropertyValue("DocTaxGrpKey", objDoc, DocTaxGrpKey);
                    GFunc.SetPropertyValue("DocTaxGrpRate", objDoc, TaxGrpRate);
                }

                if (DocComUtility.DocDetail_Get((int)objDoc.DocCodeKey, docDet, ref grdItm, ref grdExp) == false)
                    return false;

                if (DocComUtility.DocDetail_Get((int)objDoc.DocCodeKey, docDet, ref dtItm, ref dtExp) == false)
                    return false;                   

                DocTaxGrpKey = GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc);
                TaxGrpRate = DocComUtility.TaxGrpRate_Get(cn, DocTaxGrpKey, GFunc.GetDatePropertyValue("DocDate", objDoc));
                GFunc.SetPropertyValue("DocTaxGrpRate", objDoc, TaxGrpRate);

                //Set all Detail Items TaxKey,Rate

                if (!GFunc.IsNE(grdItm))
                {
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Quotation:
                        case (int)GEnum.SystemCode.Sales_Order:
                        case (int)GEnum.SystemCode.Reserve_Order:
                        case (int)GEnum.SystemCode.Delivery_Order:
                        case (int)GEnum.SystemCode.Sales_Invoice:
                        case (int)GEnum.SystemCode.Sales_Debit_Note:
                        case (int)GEnum.SystemCode.Sales_Credit_Note:
                        case (int)GEnum.SystemCode.Cash_Sale :
                        case (int)GEnum.SystemCode.Cash_Debit_Note:
                        case (int)GEnum.SystemCode.Cash_Credit_Note:

                            /* checking if there have more than one ItemGST are in grid */
                            ItmTaxGrpKeyCount = dtItm.AsEnumerable()
                                                .Where(x => Convert.ToDecimal(x["ItmAmtF"]) > 0)
                                                .Select(r => r.Field<int>("ItmTaxGrpKey"))
                                                .Distinct()
                                                .Count();

                            if (ItmTaxGrpKeyCount > 1)
                            {
                                if (MsgBox.Show(cn, "Do you want to change GST of all items equally ?", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) != GEnum.MsgBoxButton.Yes)
                                    changeItemTax = false;
                            }
                            if (changeItemTax == true)
                            {
                                grdItm.DisplayLayout.Bands[0].Columns["ItmTaxGrpKey"].DefaultCellValue = DocTaxGrpKey;
                                foreach (DataRow dr in dtItm.Rows)
                                {
                                    /* added by YST on 2021/11/05 -- not to show Tax for BankCharges in AR suggested by Josie from Finance */
                                    if (GFunc.NEInt(dr["ItmType"], 0) == (int)GEnum.ItemType.Charges && dr["ItmID"].ToString().ToLower().Contains("bank"))
                                    {
                                        dr["ItmTaxGrpKey"] = 0;
                                        dr["ItmTaxGrpRate"] = 0;
                                    }
                                    else
                                    {
                                        dr["ItmTaxGrpKey"] = DocTaxGrpKey.ToDBValue();
                                        dr["ItmTaxGrpRate"] = TaxGrpRate.ToDBValue();
                                    }
                                }
                            }                            
                            break;

                        case (int)GEnum.SystemCode.Purchase_Order:
                        case (int)GEnum.SystemCode.Purchase_Invoice:
                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        case (int)GEnum.SystemCode.Purchase_Credit_Note:

                            /* checking if there have more than one ItemGST are in grid */
                            ItmTaxGrpKeyCount = dtItm.AsEnumerable()
                                                .Where(x => Convert.ToDecimal(x["ItmAmtF"]) > 0)
                                                .Select(r => r.Field<int>("ItmTaxGrpKey"))
                                                .Distinct()
                                                .Count();

                            if (ItmTaxGrpKeyCount > 1)
                            {
                                if (MsgBox.Show(cn, "Do you want to change GST of all items equally ?", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) != GEnum.MsgBoxButton.Yes)
                                    changeItemTax = false;
                            }
                            if (changeItemTax == true)
                            {
                                grdItm.DisplayLayout.Bands[0].Columns["ItmTaxGrpKey"].DefaultCellValue = DocTaxGrpKey;
                                foreach (DataRow dr in dtItm.Rows)
                                {
                                    dr["ItmTaxGrpKey"] = DocTaxGrpKey.ToDBValue();
                                    dr["ItmTaxGrpRate"] = TaxGrpRate.ToDBValue();
                                }
                            }                                                    
                            break;

                        case (int)GEnum.SystemCode.Payment_Issue:
                            if (objDoc.DocType == 320)//Custom Import GST
                            {
                                decimal SubTotal = (decimal)GFunc.GetDecimalPropertyValue("DocSubTotal", objDoc);
                                GFunc.SetPropertyValue("DocTaxTotal", objDoc, GFunc.RndC(SubTotal * TaxGrpRate, GVar.RndDecs.Amtpt));
                            }
                            break;
                    }
                }
                if (!GFunc.IsNE(grdExp))
                {
                    /* added by YST on 2023/01/18 to allow to change different ItemGST */
                    bool changeExpTax = true;

                    /* checking if there have more than one ItemGST are in grid */
                    int ExpTaxGrpKeyCount = dtExp.AsEnumerable()
                        .Where(x => Convert.ToDecimal(x["ExpAmtF"]) > 0)
                        .Select(r => r.Field<int>("ExpTaxGrpKey"))
                        .Distinct()
                        .Count();

                    if (ExpTaxGrpKeyCount > 1)
                    {
                        if (MsgBox.Show(cn, "Do you want to change GST of all Accounts equally ?", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) != GEnum.MsgBoxButton.Yes)
                            changeExpTax = false;
                    }
                    if (changeExpTax == true)
                    {
                        grdExp.DisplayLayout.Bands[0].Columns["ExpTaxGrpKey"].DefaultCellValue = DocTaxGrpKey;
                        foreach (DataRow dr in dtExp.Rows)
                        {
                            dr["ExpTaxGrpKey"] = DocTaxGrpKey.ToDBValue();
                            dr["ExpTaxGrpRate"] = TaxGrpRate.ToDBValue();
                        }
                    }                               
                }
                return DocComUtility.CalForm(cn, objDoc, docDet, true, false);
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocTaxTotal_CustomUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocTaxTotal_CustomUpdate(cn, objDoc, docDet);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocTaxTotal_CustomUpdate(SqlConnection cn, Document objDoc, Hashtable docDet)
        {
            try
            {
                decimal CountryRate = 1M;
                decimal TaxGrpRate = 0;
                decimal TaxTotalF = 0;
                decimal TaxTotalL = 0;

                UltraGrid grdItm = null;
                UltraGrid grdExp = null;

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Payment_Issue:

                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:

                        TaxGrpRate = GFunc.GetDecimalPropertyValue("DocTaxGrpRate", objDoc).Value;
                        if (TaxGrpRate == 0M)
                        {
                            MsgBox.Show(cn, "Cannot change Tax Amount when Tax Rate is ZERO");
                            return false;
                        }

                        CountryRate = GFunc.GetDecimalPropertyValue("DocCountryRate", objDoc).Value;
                        TaxGrpRate = GFunc.GetDecimalPropertyValue("DocTaxGrpRate", objDoc).Value;
                        TaxTotalF = GFunc.GetDecimalPropertyValue("DocTaxTotal", objDoc).Value;
                        TaxTotalF = GFunc.RndC(TaxTotalF, GVar.RndDecs.Amtpt);
                        TaxTotalL = GFunc.RndC(TaxTotalF * CountryRate, GVar.RndDecs.Amtpt);

                        GFunc.SetPropertyValue("DocTaxTotalLocal", objDoc, TaxTotalL);

                        if (DocComUtility.DocDetail_Get((int)objDoc.DocCodeKey, docDet, ref grdItm, ref grdExp) == false)
                            return false;

                        return DocComUtility.CalForm(cn, objDoc, docDet, false, false);


                    default:
                        return false;
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocTaxTotalLocal_CustomUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocTaxTotalLocal_CustomUpdate(cn, objDoc, docDet);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocTaxTotalLocal_CustomUpdate(SqlConnection cn, Document objDoc, Hashtable docDet)
        {
            try
            {
                decimal TaxTotalF = 0M;
                decimal TaxTotalL = 0M;
                decimal CountryRate = 0M;

                UltraGrid grdItm = null;
                UltraGrid grdExp = null;

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Payment_Issue:

                        TaxTotalF = GFunc.GetDecimalPropertyValue("DocTaxTotal", objDoc).Value;
                        TaxTotalL = GFunc.GetDecimalPropertyValue("DocTaxTotalLocal", objDoc).Value;
                        TaxTotalL = GFunc.RndC(TaxTotalL, GVar.RndDecs.Amtpt);
                        CountryRate = GFunc.RndDC(TaxTotalL, TaxTotalF, GVar.RndDecs.Curpt);
                        GFunc.SetPropertyValue("DocCountryRate", objDoc, CountryRate);

                        if (DocComUtility.DocDetail_Get((int)objDoc.DocCodeKey, docDet, ref grdItm, ref grdExp) == false)
                            return false;

                        //Run Calcualtion to update Detail ItmTaxGrpAmtF/L or ExpTaxGrpAmtF/L
                        if (DocComUtility.CalForm(cn, objDoc, docDet, true, false))
                        {
                            GFunc.SetPropertyValue("DocTaxTotalLocal", objDoc, TaxTotalL);
                            return DocComUtility.CalForm(cn, objDoc, docDet, false, false);
                        }
                        else
                            return false;

                    default:
                        return false;
                }


            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocTypeNm_CustomUpdate(Document objDoc, string ctrlValue)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocTypeNm_CustomUpdate(cn, objDoc, ctrlValue);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocTypeNm_CustomUpdate(Document objDoc, Hashtable docDet, string ctrlValue)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocTypeNm_CustomUpdate(cn, objDoc, docDet, ctrlValue);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocTypeNm_CustomUpdate(SqlConnection cn, Document objDoc, string ctrlValue)
        {
            try
            {
                int docType = 0;
                short docSign = 1;

                if (GFunc.IsNE(ctrlValue))
                {
                    MsgBox.Show(cn, "Document type cannot be empty");
                    return false;
                }

                if (DocComUtility.DocType_Get(cn, (int)objDoc.DocCodeKey, ctrlValue, ref docType, ref docSign))
                {
                    objDoc.DocType = docType;
                    objDoc.DocSign = docSign;
                    return DocComUtility.CalForm(cn, objDoc, false);
                }
                else
                    return false;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocTypeNm_CustomUpdate(SqlConnection cn, Document objDoc, Hashtable docDet, string ctrlValue)
        {
            try
            {
                int docType = 0;
                short docSign = 1;

                if (GFunc.IsNE(ctrlValue))
                {
                    MsgBox.Show(cn, "Document type cannot be empty");
                    return false;
                }

                if (DocComUtility.DocType_Get(cn, (int)objDoc.DocCodeKey, ctrlValue, ref docType, ref docSign))
                {
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Sales_Order:
                        case (int)GEnum.SystemCode.Reserve_Order:
                        case (int)GEnum.SystemCode.Purchase_Order:
                            if (objDoc.DocState == (int)GEnum.DocState.Posted)
                            {
                                if (objDoc.DocType >= 200 && docType <= 110)
                                {
                                    MsgBox.Show(cn, "Changing from Non Tracking to Tracking is not allow");
                                    return false;
                                }
                                if (objDoc.DocType >= 110 && docType <= 200)
                                {
                                    MsgBox.Show(cn, "Changing from Tracking to Non Tracking is not allow");
                                    return false;
                                }
                            }
                            break;

                        case (int)GEnum.SystemCode.Payment_Received:
                            if (docType == 300)  //Claim
                            {
                                DataTable dtExp = null;
                                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, docDet, ref dtExp);
                                if (dtExp.Rows.Count > 0)
                                {
                                    if (MsgBox.Show(cn, "The system will remove all data in the document revenue, continue?", GEnum.MsgBoxButton.OK, GEnum.MsgBoxButton.Cancel) != GEnum.MsgBoxButton.OK)
                                        return false;
                                    else
                                    {
                                        dtExp.Clear();
                                        dtExp.AcceptChanges();
                                    }
                                }
                            }
                            break;

                        case (int)GEnum.SystemCode.Payment_Issue:
                            if (docType == 310 || docType == 320)  //GST Payment or Custom Import Tax
                            {
                                DataTable dtExp = null;
                                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, docDet, ref dtExp);
                                if (dtExp.Rows.Count > 0)
                                {
                                    if (MsgBox.Show(cn, "The system will remove all data in the document expenses, continue?", GEnum.MsgBoxButton.OK, GEnum.MsgBoxButton.Cancel) != GEnum.MsgBoxButton.OK)
                                        return false;
                                    else
                                    {
                                        dtExp.Clear();
                                        dtExp.AcceptChanges();
                                    }
                                }
                            }
                            break;

                        case (int)GEnum.SystemCode.Inventory_Adjustment:
                            if (objDoc.DocState == (int)GEnum.DocState.Posted)
                            {
                                MsgBox.Show(cn, "You cannot Change the document type when your document has been posted");
                                return false;
                            }
                            else
                            {
                                DataTable dtItem = null;
                                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref dtItem);
                                if (dtItem.Rows.Count > 0)
                                {
                                    if (MsgBox.Show(cn, "The system will remove all data in the document detail, continue?", GEnum.MsgBoxButton.OK, GEnum.MsgBoxButton.Cancel) != GEnum.MsgBoxButton.OK)
                                        return false;
                                    else
                                    {
                                        dtItem.Clear();
                                        dtItem.AcceptChanges();


                                    }
                                }
                                if (objDoc.DocTypeNm == "R&D Adjustment")
                                {
                                    GFunc.SetPropertyValue("DocAccKey", objDoc, 1291);
                                    GFunc.SetPropertyValue("DocAccDes", objDoc, "Intangibles Assets - Development");
                                }
                               
                              
                               
                            }
                            break;
                    }

                    if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Order && (objDoc.DocTypeNm == "Direct Shipment" || objDoc.DocTypeNm == "Sales Order - VN"))
                    {
                        int? DocTaxGrpKey = 0;
                        decimal? TaxGrpRate = 0;
                        GFunc.SetPropertyValue("DocTaxGrpKey", objDoc, DocTaxGrpKey);
                        GFunc.SetPropertyValue("DocTaxGrpRate", objDoc, TaxGrpRate);
                    }

                    objDoc.DocType = docType;
                    objDoc.DocSign = docSign;
                    return DocComUtility.CalForm(cn, objDoc, docDet, true, false);
                }
                else
                    return false;

            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocTranGrpID_btnClick(Document objDoc)
        {
            try
            {
                frmPopupTreeView _frmPopupTreeView = new frmPopupTreeView();
                _frmPopupTreeView.ShowDialog();
                if (_frmPopupTreeView.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    GFunc.SetPropertyValue("DocTranGrpKey", objDoc, _frmPopupTreeView.TranGrpKey);
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocTranGrpID_btnClick(Document objDoc, Hashtable docDet)
        {
            try
            {
                frmPopupTreeView _frmPopupTreeView = new frmPopupTreeView();
                _frmPopupTreeView.ShowDialog();
                if (_frmPopupTreeView.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    GFunc.SetPropertyValue("DocTranGrpKey", objDoc, _frmPopupTreeView.TranGrpKey);
                    return DocTranGrpKey_CustomUpdate(objDoc, docDet);
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocTranGrpKey_CustomUpdate(Document objDoc)
        {
            try
            {
                int itmTranGrpKey = GFunc.NEInt(GFunc.GetPropertyValue("DocTranGrpKey", objDoc), 0);
                GFunc.SetPropertyValue("DocTranGrpKey", objDoc, itmTranGrpKey);
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocTranGrpKey_CustomUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                UltraGrid grdItm = null;
                UltraGrid grdExp = null;
                DataTable dtItm = null;
                DataTable dtExp = null;

                if (DocComUtility.DocDetail_Get((int)objDoc.DocCodeKey, docDet, ref grdItm, ref grdExp) == false)
                    return false;

                if (DocComUtility.DocDetail_Get((int)objDoc.DocCodeKey, docDet, ref dtItm, ref dtExp) == false)
                    return false;

                int docTranGrpKey = GFunc.NEInt(GFunc.GetPropertyValue("DocTranGrpKey", objDoc), 0);
                GFunc.SetPropertyValue("DocTranGrpKey", objDoc, docTranGrpKey);

                #region Set TranGrpKey cell default value and set all detail TranGrpKey value
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                        if (grdItm != null)
                        {
                            grdItm.DisplayLayout.Bands[0].Columns["ItmTranGrpKey"].DefaultCellValue = docTranGrpKey;

                            if (dtItm.Rows.Count > 0)
                            {
                                if (MsgBox.Show(MsgID.Document.SetDetTranGrpWhenHDRChange, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                                {
                                    foreach (DataRow row in dtItm.Rows)
                                    {
                                        if (GFunc.IsPostingItmType(row["ItmType"]))
                                            row["ItmTranGrpKey"] = docTranGrpKey;
                                    }
                                }
                            }
                        }
                        break;
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                        if (grdItm != null)
                        {
                            grdItm.DisplayLayout.Bands[0].Columns["ItmTranGrpKey"].DefaultCellValue = docTranGrpKey;
                            if (dtItm.Rows.Count > 0)
                            {
                                if (MsgBox.Show(MsgID.Document.SetDetTranGrpWhenHDRChange, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                                {
                                    foreach (DataRow row in dtItm.Rows)
                                    {
                                        row["ItmTranGrpKey"] = docTranGrpKey;
                                    }
                                }
                            }
                        }
                        break;
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        if (grdExp != null)
                        {
                            grdExp.DisplayLayout.Bands[0].Columns["ExpTranGrpKey"].DefaultCellValue = docTranGrpKey;
                            if (dtItm.Rows.Count > 0)
                            {
                                if (MsgBox.Show(MsgID.Document.SetDetTranGrpWhenHDRChange, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                                {
                                    foreach (DataRow row in dtExp.Rows)
                                    {
                                        row["ExpTranGrpKey"] = docTranGrpKey;
                                    }
                                }
                            }
                        }
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        if (grdItm != null)
                            grdItm.DisplayLayout.Bands[0].Columns["ItmTranGrpKey"].DefaultCellValue = docTranGrpKey;

                        if (grdExp != null)
                            grdExp.DisplayLayout.Bands[0].Columns["ExpTranGrpKey"].DefaultCellValue = docTranGrpKey;
                        if (dtItm.Rows.Count > 0)
                        {
                            if (MsgBox.Show(MsgID.Document.SetDetTranGrpWhenHDRChange, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                            {
                                if (grdItm != null)
                                {
                                    foreach (DataRow row in dtItm.Rows)
                                    {
                                        if (GFunc.IsPostingItmType(row["ItmType"]))
                                            row["ItmTranGrpKey"] = docTranGrpKey;
                                    }
                                }
                                if (grdExp != null)
                                {
                                    foreach (DataRow row in dtExp.Rows)
                                    {
                                        row["ExpTranGrpKey"] = docTranGrpKey;
                                    }
                                }
                            }
                        }
                        break;
                }
                #endregion

                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed

        //Button Events
        public static bool DocApplyIV_Click(Document objDoc)
        {
            try
            {
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        if (GFunc.GetDecimalPropertyValue("DocApplyAmtF", objDoc) != 0 &&
                            GFunc.GetIntPropertyValue("DocApplyIVDC", objDoc) == 0 && GFunc.GetIntPropertyValue("DocApplyIVDK", objDoc) == 0)
                        {
                            MsgBox.Show("This Document has been applied through Payment, Can't Continue");
                            return false;
                        }

                        if (GFunc.IsNEZ(GFunc.GetIntPropertyValue("DocConKey", objDoc)))
                        {
                            MsgBox.Show("Customer/Vendor ID cannot be empty");
                            return false;
                        }
                        //Open Apply Invoice form
                        frmApplyIV applyIV = new frmApplyIV(objDoc);
                        applyIV.ShowDialog();

                        break;
                }
                return true;

            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DocPaid_Click(Document objDoc, Hashtable docDet)
        {
            try
            {
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                        frmPayment payment = new frmPayment(objDoc);
                        if (payment.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            return DocComUtility.CalForm(objDoc, docDet, false, false);
                        break;
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed

        public static bool EditorButton_Popup(int CodeKey, string searchValue, string listSettingID, int popupType, ref int key, ref string id, ref string name)
        {
            frmRecordSearch fpopup = null;
            string[] listSettingIDs = listSettingID.Split('%');

            if (listSettingIDs.Length > 1)
                listSettingID = listSettingIDs[0];

            switch (popupType)
            {
                case (int)GEnum.PopupType.CusID:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, GEnum.PopupType.CusID, (int)GEnum.SystemCode.Customer,CodeKey);
                    break;
                case (int)GEnum.PopupType.CusNm:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, GEnum.PopupType.CusNm, (int)GEnum.SystemCode.Customer, CodeKey);
                    break;
                case (int)GEnum.PopupType.Vehicle:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, GEnum.PopupType.Vehicle, (int)GEnum.SystemCode.Vehicle, CodeKey);
                    break;
                case (int)GEnum.PopupType.VendID:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, GEnum.PopupType.VendID, (int)GEnum.SystemCode.Vendor, CodeKey);
                    break;
                case (int)GEnum.PopupType.VendNm:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, GEnum.PopupType.VendNm, (int)GEnum.SystemCode.Vendor, CodeKey);
                    break;
                case (int)GEnum.PopupType.VendItmID:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, GEnum.PopupType.VendItmID, (int)GEnum.SystemCode.Vendor, CodeKey);
                    break;
                case (int)GEnum.PopupType.AccID:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, GEnum.PopupType.AccID, (int)GEnum.SystemCode.Account, CodeKey);
                    break;
                case (int)GEnum.PopupType.AccDes:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, GEnum.PopupType.AccDes, (int)GEnum.SystemCode.Account, CodeKey);
                    break;
                case (int)GEnum.PopupType.AccLiabilityDes:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, GEnum.PopupType.AccLiabilityDes, (int)GEnum.SystemCode.Account, CodeKey);
                    break;
                case (int)GEnum.PopupType.AccDisID:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, GEnum.PopupType.AccDisID, (int)GEnum.SystemCode.Account, CodeKey);
                    break;
                case (int)GEnum.PopupType.ItmID:

                    if (GFunc.CompareString(listSettingID, "MSTItmC_id"))
                    {
                        int vendorKey = listSettingIDs.Length > 0 ? GFunc.NEInt(listSettingIDs[1], 0) : 0;
                        fpopup = new frmRecordSearch(listSettingID, searchValue, (int)vendorKey, GEnum.PopupType.ItmID, (int)GEnum.SystemCode.Inventory, CodeKey);
                    }
                    else
                        fpopup = new frmRecordSearch(listSettingID, searchValue, GEnum.PopupType.ItmID, (int)GEnum.SystemCode.Inventory, CodeKey);
                    break;
                case (int)GEnum.PopupType.ItmDes:
                    if (GFunc.CompareString(listSettingID, "MSTItmC_des"))
                    {
                        int vendorKey = listSettingIDs.Length > 0 ? GFunc.NEInt(listSettingIDs[1], 0) : 0;
                        fpopup = new frmRecordSearch(listSettingID, searchValue, vendorKey, GEnum.PopupType.ItmDes, (int)GEnum.SystemCode.Inventory, CodeKey);
                    }
                    else
                        fpopup = new frmRecordSearch(listSettingID, searchValue, GEnum.PopupType.ItmDes, (int)GEnum.SystemCode.Inventory, CodeKey);
                    break;
                case (int)GEnum.PopupType.JobDes:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, GEnum.PopupType.JobDes, (int)GEnum.SystemCode.Job, CodeKey);
                    break;

                //-----------For OpenID ---------//
                case (int)GEnum.PopupType.ShipNmOpenID:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, GEnum.PopupType.ShipNmOpenID, (int)GEnum.SystemCode.Ship_Name, CodeKey);
                    break;
                case (int)GEnum.PopupType.PriceInfoOpenID:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, GEnum.PopupType.PriceInfoOpenID, (int)GEnum.SystemCode.Price_List, CodeKey);
                    break;
            }

            if (fpopup.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                key = fpopup.Key;
                id = fpopup.ID;
                name = fpopup.Des;
                fpopup.Dispose();
                return true;
            }
            else
            {
                key = 0;
                id = string.Empty;
                name = string.Empty;
                fpopup.Dispose();
                return false;
            }

        }
        public static bool EditorButton_Popup(Document objDoc, int CodeKey, string searchValue, string listSettingID, int popupType, ref int key, ref string id, ref string name)
        {
            frmRecordSearch fpopup = null;
            switch (popupType)
            {
                case (int)GEnum.PopupType.CusID:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, (GEnum.PopupType)popupType, (int)GEnum.SystemCode.Customer, CodeKey);
                    break;
                case (int)GEnum.PopupType.CusNm:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, (GEnum.PopupType)popupType, (int)GEnum.SystemCode.Customer, CodeKey);
                    break;
                case (int)GEnum.PopupType.VendID:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, (GEnum.PopupType)popupType, (int)GEnum.SystemCode.Vendor, CodeKey);
                    break;
                case (int)GEnum.PopupType.VendNm:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, (GEnum.PopupType)popupType, (int)GEnum.SystemCode.Vendor, CodeKey);
                    break;
                case (int)GEnum.PopupType.VendItmID:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, (GEnum.PopupType)popupType, (int)GEnum.SystemCode.Vendor, CodeKey);
                    break;
                case (int)GEnum.PopupType.AccID:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, (GEnum.PopupType)popupType, (int)GEnum.SystemCode.Account, CodeKey);
                    break;
                case (int)GEnum.PopupType.AccDes:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, (GEnum.PopupType)popupType, (int)GEnum.SystemCode.Account, CodeKey);
                    break;
                case (int)GEnum.PopupType.AccDisID:
                    fpopup = new frmRecordSearch(listSettingID, searchValue, (GEnum.PopupType)popupType, (int)GEnum.SystemCode.Account, CodeKey);
                    break;
                case (int)GEnum.PopupType.ItmID:
                case (int)GEnum.PopupType.ItmStkID:
                case (int)GEnum.PopupType.ItmFinishID:
                    if (GFunc.CompareString(listSettingID, "MSTItmC_id"))
                    {
                        int vendorKey = GFunc.NEInt(GFunc.GetIntPropertyValue("DocConKey", objDoc), 0);
                        fpopup = new frmRecordSearch(listSettingID, searchValue, vendorKey, GEnum.PopupType.ItmID, (int)GEnum.SystemCode.Inventory, CodeKey);
                    }
                    else
                        fpopup = new frmRecordSearch(listSettingID, searchValue, GEnum.PopupType.ItmID, (int)GEnum.SystemCode.Inventory, CodeKey);
                    break;
                case (int)GEnum.PopupType.ItmDes:
                    if (GFunc.CompareString(listSettingID, "MSTItmC_des"))
                    {
                        int vendorKey = GFunc.NEInt(GFunc.GetIntPropertyValue("DocConKey", objDoc), 0);
                        fpopup = new frmRecordSearch(listSettingID, searchValue, vendorKey, GEnum.PopupType.ItmDes, (int)GEnum.SystemCode.Inventory, CodeKey);
                    }
                    else
                        fpopup = new frmRecordSearch(listSettingID, searchValue, GEnum.PopupType.ItmDes, (int)GEnum.SystemCode.Inventory, CodeKey);
                    break;
                //case (int)GEnum.PopupType.ShipID:
                //    fpopup = new frmRecordSearch(listSettingID, new string[] { }, 0, "Ship ID", 0, "Ship Name", 0, (int)GEnum.SystemCode.Ship_Name);
                //    break;
            }

            if (fpopup.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                key = fpopup.Key;
                id = fpopup.ID;
                name = fpopup.Des;
                fpopup.Dispose();
                return true;
            }
            else
            {
                key = 0;
                id = string.Empty;
                name = string.Empty;
                fpopup.Dispose();
                return false;
            }
        }
        public static bool EditorButton_Popup(Document objDoc, TAUtil.TAComboBox cbo, string listSettingID, int popupTypeup, ref int key, ref string id, ref string name, ref int jobPhaseKey, ref int jobCostTypeKey, ref int jobTaskKey)
        {

            frmRecordSearch fpopup = null;
            int? DocConKey = 0;


            if (GFunc.CompareString(listSettingID, "MSTJobSales_id") || GFunc.CompareString(listSettingID, "MSTJobSalesByConKey"))
            {
                DocConKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
                if (popupTypeup == (int)GEnum.PopupType.JobID)
                    fpopup = new frmRecordSearch(listSettingID, cbo.Text, (GEnum.PopupType)popupTypeup, (int)objDoc.DocCodeKey);
                else
                    fpopup = new frmRecordSearch(listSettingID, cbo.Text, (GEnum.PopupType)popupTypeup, (int)objDoc.DocCodeKey);
            }
            else
            {
                if (popupTypeup == (int)GEnum.PopupType.JobID)
                    fpopup = new frmRecordSearch(listSettingID, cbo.Text, (GEnum.PopupType)popupTypeup, (int)objDoc.DocCodeKey);
                else
                    fpopup = new frmRecordSearch(listSettingID, cbo.Text, (GEnum.PopupType)popupTypeup, (int)objDoc.DocCodeKey);
            }

            try
            {
                if (cbo.GetType() == typeof(TAUtil.TAComboBox))
                {

                    if (fpopup.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        key = fpopup.Key;
                        id = fpopup.ID;
                        name = fpopup.Des;
                        jobCostTypeKey = fpopup.JobCostTypeKey;
                        jobPhaseKey = fpopup.JobPhaseKey;
                        jobTaskKey = fpopup.JobTaskKey;
                        fpopup.Dispose();
                        return true;
                    }
                    else
                    {
                        key = 0;
                        id = string.Empty;
                        name = string.Empty;
                        jobCostTypeKey = 0;
                        jobPhaseKey = 0;
                        jobTaskKey = 0;
                        fpopup.Dispose();
                        return false;
                    }
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }
        public static bool EditorButton_Popup(Document objDoc, string searchValue, string listSettingID, int popupTypeup, ref int key, ref string id, ref string name, ref int jobPhaseKey, ref int jobCostTypeKey, ref int jobTaskKey)
        {

            frmRecordSearch fpopup = null;
            int? DocConKey = 0;

            if (GFunc.CompareString(listSettingID, "MSTJobSales_id"))
            {
                DocConKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
                if (popupTypeup == (int)GEnum.PopupType.JobID)
                    fpopup = new frmRecordSearch(listSettingID, searchValue, (int)DocConKey, (GEnum.PopupType)popupTypeup, (int)GEnum.SystemCode.Job);
                else
                    fpopup = new frmRecordSearch(listSettingID, searchValue, (int)DocConKey, (GEnum.PopupType)popupTypeup, (int)GEnum.SystemCode.Job);
            }
            else
            {
                if (popupTypeup == (int)GEnum.PopupType.JobID)
                    fpopup = new frmRecordSearch(listSettingID, searchValue, (GEnum.PopupType)popupTypeup, (int)GEnum.SystemCode.Job);              
            }

            try
            {
                if (fpopup.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    key = fpopup.Key;
                    id = fpopup.ID;
                    name = fpopup.Des;
                    jobCostTypeKey = fpopup.JobCostTypeKey;
                    jobPhaseKey = fpopup.JobPhaseKey;
                    jobTaskKey = fpopup.JobTaskKey;
                    fpopup.Dispose();
                    return true;
                }
                else
                {
                    key = 0;
                    id = string.Empty;
                    name = string.Empty;
                    jobCostTypeKey = 0;
                    jobPhaseKey = 0;
                    jobTaskKey = 0;
                    fpopup.Dispose();
                    return false;
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }

        //Common functions
        public static bool DefLocKey_CustomUpdate(Document objDoc, Hashtable docDet, int? defaultLocKey)
        {
            try
            {
                UltraGrid grdItm = null;
                DataTable dtItm = null;

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref dtItm);
                        if (GFunc.IsNEZ(defaultLocKey) == false)
                        {
                            grdItm.DisplayLayout.Bands[0].Columns["ItmLocKey"].DefaultCellValue = defaultLocKey;
                            if (dtItm.Rows.Count > 0)
                            {
                                if (MsgBox.Show("Apply default location to all details", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                                {
                                    if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Invoice)
                                    {
                                        foreach (DataRow row in dtItm.Rows)
                                        {
                                            if (GFunc.IsStockGroupItmType(row["ItmType"]) && GFunc.IsNEZ(row["APPDDK"]))
                                            {
                                                row["ItmLocKey"] = defaultLocKey;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (DataRow row in dtItm.Rows)
                                        {
                                            if (GFunc.IsStockGroupItmType(row["ItmType"]))
                                            {
                                                row["ItmLocKey"] = defaultLocKey;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DefFromLocKey_CustomUpdate(Document objDoc, Hashtable docDet, int? defaultLocKey)
        {
            try
            {
                UltraGrid grdItm = null;
                DataTable dtItm = null;

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref dtItm);
                        if (GFunc.IsNEZ(defaultLocKey) == false)
                        {
                            grdItm.DisplayLayout.Bands[0].Columns["ItmFromLocKey"].DefaultCellValue = defaultLocKey;

                            if (dtItm.Rows.Count > 0)
                            {
                                if (MsgBox.Show("Apply default From location to all details", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                                {
                                    foreach (DataRow row in dtItm.Rows)
                                    {
                                        if (GFunc.IsPostingItmType(row["ItmType"]))
                                            row["ItmFromLocKey"] = defaultLocKey;
                                    }
                                }
                            }

                        }
                        break;
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DefToLocKey_CustomUpdate(Document objDoc, Hashtable docDet, int? defaultLocKey)
        {
            try
            {
                UltraGrid grdItm = null;
                DataTable dtItm = null;

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref dtItm);
                        if (GFunc.IsNEZ(defaultLocKey) == false)
                        {
                            grdItm.DisplayLayout.Bands[0].Columns["ItmToLocKey"].DefaultCellValue = defaultLocKey;

                            if (dtItm.Rows.Count > 0)
                            {
                                if (MsgBox.Show("Apply default To location to all details", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                                {
                                    foreach (DataRow row in dtItm.Rows)
                                    {
                                        if (GFunc.IsPostingItmType(row["ItmType"]))
                                            row["ItmToLocKey"] = defaultLocKey;
                                    }
                                }
                            }
                        }
                        break;
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DefDateRequired_CustomUpdate(Document objDoc, UltraGrid grd, DateTime? defaultReqDate)
        {
            try
            {
                int itmTypeGrp;               
                DataTable dtItm = null;                               

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Order_Consignment:
                        if (GFunc.IsNEZ(defaultReqDate) == false)
                        {
                            grd.DisplayLayout.Bands[0].Columns["ItmReqDate"].DefaultCellValue = defaultReqDate;

                            if (MsgBox.Show("Apply default required date to all details", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                            {
                                dtItm = (DataTable)grd.DataSource;
                                foreach (DataRow row in dtItm.Rows)
                                {
                                    itmTypeGrp = GFunc.GetINTypeGroup(row["ItmType"]);
                                    switch (itmTypeGrp)
                                    {
                                        case (int)GEnum.INTypeGrp.Stock:
                                        case (int)GEnum.INTypeGrp.Non_Stock:
                                        case (int)GEnum.INTypeGrp.Charges:
                                            row["ItmReqDate"] = defaultReqDate;                                           
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DefDatePromised_CustomUpdate(Document objDoc, UltraGrid grd, DateTime? defaultPrmDate)
        {
            try
            {
                int itmTypeGrp;
                DataTable dtItm = null; 

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Order_Consignment:
                        if (GFunc.IsNEZ(defaultPrmDate) == false)
                        {
                            grd.DisplayLayout.Bands[0].Columns["ItmPrmDate"].DefaultCellValue = defaultPrmDate;

                            if (MsgBox.Show("Apply default required date to all details", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                            {
                                dtItm = (DataTable)grd.DataSource;
                                foreach (DataRow row in dtItm.Rows)
                                {
                                    itmTypeGrp = GFunc.GetINTypeGroup(row["ItmType"]);
                                    switch (itmTypeGrp)
                                    {
                                        case (int)GEnum.INTypeGrp.Stock:
                                        case (int)GEnum.INTypeGrp.Non_Stock:
                                        case (int)GEnum.INTypeGrp.Charges:
                                            row["ItmPrmDate"] = defaultPrmDate;                                            
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DefJob_CustomUpdate(Document objDoc, Hashtable docDet, int defaultJob, bool ApplyToDetail)
        {
            try
            {
                UltraGrid grdItm = null;
                DataTable dtItm = null;

                switch (objDoc.DocCodeKey)
                {
                    #region Set default ItmJobKey for sales document
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref dtItm);  
                        
                        grdItm.DisplayLayout.Bands[0].Columns["ItmJobKey"].DefaultCellValue = defaultJob;

                        if (ApplyToDetail)
                        {
                            if (dtItm.Rows.Count > 0)
                            {
                                foreach (DataRow row in dtItm.Rows)
                                {
                                    if (GFunc.IsPostingItmType(row["ItmType"]))
                                    {
                                        row["ItmJobKey"] = defaultJob;
                                        if (defaultJob == 0)
                                        {
                                            row["ItmJobCostTypeKey"] = 0;
                                            row["ItmJobPhaseKey"] = 0;
                                            row["ItmJobTaskKey"] = 0;
                                        }
                                    }
                                }
                            }
                        }                        
                        break;
                    #endregion

                    #region Set default ItmJobKey for purchase document
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref dtItm);
                        grdItm.DisplayLayout.Bands[0].Columns["ItmJobKey"].DefaultCellValue = defaultJob;

                        if (ApplyToDetail)
                        {
                            if (dtItm.Rows.Count > 0)
                            {
                                int itmTypeGrp;
                                foreach (DataRow row in dtItm.Rows)
                                {
                                    itmTypeGrp = GFunc.GetINTypeGroup(row["ItmType"]);
                                    switch (itmTypeGrp)
                                    {
                                        case (int)GEnum.INTypeGrp.Non_Stock:
                                        case (int)GEnum.INTypeGrp.Charges:
                                        case (int)GEnum.INTypeGrp.Discount:
                                            row["ItmJobKey"] = defaultJob;
                                            if (defaultJob == 0)
                                            {
                                                row["ItmJobCostTypeKey"] = 0;
                                                row["ItmJobPhaseKey"] = 0;
                                                row["ItmJobTaskKey"] = 0;
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Set default ItmJobKey for Inventory document
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref dtItm);
                        grdItm.DisplayLayout.Bands[0].Columns["ItmJobKey"].DefaultCellValue = defaultJob;

                        if (ApplyToDetail)
                        {
                            if (dtItm.Rows.Count > 0)
                            {
                                foreach (DataRow row in dtItm.Rows)
                                {
                                    row["ItmJobKey"] = defaultJob;
                                    if (defaultJob == 0)
                                    {
                                        row["ItmJobCostTypeKey"] = 0;
                                        row["ItmJobPhaseKey"] = 0;
                                        row["ItmJobTaskKey"] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Set default ExpJobKey for ARPY/APPY document
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, docDet, ref grdItm);
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, docDet, ref dtItm);
                        grdItm.DisplayLayout.Bands[0].Columns["ExpJobKey"].DefaultCellValue = defaultJob;

                        if (ApplyToDetail)
                        {
                            if (dtItm.Rows.Count > 0)
                            {
                                foreach (DataRow row in dtItm.Rows)
                                {
                                    row["ExpJobKey"] = defaultJob;
                                    if (defaultJob == 0)
                                    {
                                        row["ExpJobCostTypeKey"] = 0;
                                        row["ExpJobPhaseKey"] = 0;
                                        row["ExpJobTaskKey"] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static bool DefAcc_CustomUpdate(Document objDoc, Hashtable docDet, int defaultAccKey, string defaultAccDes, bool ApplyToDetail)
        {
            try
            {
                UltraGrid grdItm = null;
                DataTable dtItm = null;

                switch (objDoc.DocCodeKey)
                {
                    #region Set default ItmAccKey for sales document
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref dtItm);

                        grdItm.DisplayLayout.Bands[0].Columns["ItmAccKey"].DefaultCellValue = defaultAccKey;
                        //grdItm.DisplayLayout.Bands[0].Columns["ItmAccDes"].DefaultCellValue = defaultAccDes;

                        if (ApplyToDetail)
                        {
                            if (dtItm.Rows.Count > 0)
                            {
                                foreach (DataRow row in dtItm.Rows)
                                {
                                    if (GFunc.IsPostingItmType(row["ItmType"]))
                                    {
                                        row["ItmAccKey"] = defaultAccKey;
                                        row["ItmAccDes"] = defaultAccDes;
                                        if (defaultAccKey == 0)
                                        {
                                            row["ItmAccKey"] = 0;
                                            row["ItmAccDes"] = null;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Set default ItmAccKey for purchase document
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref dtItm);
                        grdItm.DisplayLayout.Bands[0].Columns["ItmAccKey"].DefaultCellValue = defaultAccKey;

                        if (ApplyToDetail)
                        {
                            if (dtItm.Rows.Count > 0)
                            {
                                int itmTypeGrp;
                                foreach (DataRow row in dtItm.Rows)
                                {
                                    itmTypeGrp = GFunc.GetINTypeGroup(row["ItmType"]);
                                    switch (itmTypeGrp)
                                    {
                                        case (int)GEnum.INTypeGrp.Non_Stock:
                                        case (int)GEnum.INTypeGrp.Charges:
                                        case (int)GEnum.INTypeGrp.Discount:
                                            row["ItmAccKey"] = defaultAccKey;
                                            if (defaultAccKey == 0)
                                            {
                                                row["ItmAccKey"] = 0;
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Set default ItmJobKey for Inventory document
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref dtItm);
                        grdItm.DisplayLayout.Bands[0].Columns["ItmAccKey"].DefaultCellValue = defaultAccKey;

                        if (ApplyToDetail)
                        {
                            if (dtItm.Rows.Count > 0)
                            {
                                foreach (DataRow row in dtItm.Rows)
                                {
                                    row["ItmAccKey"] = defaultAccKey;
                                    if (defaultAccKey == 0)
                                    {
                                        row["ItmAccKey"] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    #endregion                    
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed

        public static bool Doc_CheckDetItm(Document objDoc, UltraGrid grd, GEnum.ValidateField validateField)
        {
            //Note 0: Skip, 10: run allow to continue with warning msg, 20: run cannot continue if fail
            const int Warning = 10;
            const int cannotFail = 20;
            bool runAllrows = false;
            int runItmQtyLink = 0;
            int runItmQtyBalance = 0;
            int runItmOrderStatus = 0;
            int runARSODK = 0;
            int runARDODK = 0;
            int runARIVDK = 0;
            int runAPPODK = 0;
            int runAPPDDK = 0;
            int runCSCPODK = 0;
            int runCSCPSDK = 0;
            int runCSCSIDK = 0;
            int runJob = 0;
            int runJobPayment = 0;
            bool jobUsedinDetail = false;
            bool runConsignmentSettlement = false;
            decimal ItmSN = 0;
            int hasColumn = 0;

            try
            {
                #region set process to run base on DocCodeKey
                switch ((int)objDoc.DocCodeKey)
                {
                    #region Sales Order
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                        switch (validateField)
                        {
                            case GEnum.ValidateField.DocConKey:
                                runAllrows = true;
                                runItmQtyLink = cannotFail;
                                runItmOrderStatus = cannotFail;
                                runJob = Warning;
                                break;

                            case GEnum.ValidateField.DocCurrKey:
                                runAllrows = true;
                                runItmQtyLink = cannotFail;
                                runItmOrderStatus = cannotFail;
                                break;

                            case GEnum.ValidateField.DelDocItmKey:
                            case GEnum.ValidateField.ItmKey:
                                runItmQtyLink = cannotFail;
                                runItmOrderStatus = cannotFail;
                                break;

                            case GEnum.ValidateField.ItmJobKey:
                                runItmQtyLink = Warning;
                                runItmOrderStatus = Warning;
                                break;

                            case GEnum.ValidateField.ItmQty:
                            case GEnum.ValidateField.ItmQtyAdj:
                            case GEnum.ValidateField.ItmUOMKey:
                                runItmQtyBalance = cannotFail;
                                break;

                            case GEnum.ValidateField.ItmOrderStatus:
                                runItmOrderStatus = cannotFail;
                                break;
                        }
                        break;
                    #endregion

                    #region Delivery Order
                    case (int)GEnum.SystemCode.Delivery_Order:
                        switch (validateField)
                        {
                            case GEnum.ValidateField.DocConKey:
                                runAllrows = true;
                                runARSODK = cannotFail;
                                runJob = Warning;
                                break;

                            case GEnum.ValidateField.DocCurrKey:
                                runAllrows = true;
                                runARSODK = cannotFail;
                                break;

                            case GEnum.ValidateField.ItmKey:
                                runARSODK = cannotFail;
                                break;

                            case GEnum.ValidateField.ItmJobKey:
                                runARSODK = cannotFail;
                                break;
                        }
                        break;
                    #endregion

                    #region Sales/Cash  Invoice/CD/DN
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                        switch (validateField)
                        {
                            case GEnum.ValidateField.DocConKey:
                                runAllrows = true;
                                runARSODK = cannotFail;
                                runCSCPSDK = cannotFail;
                                runCSCSIDK = cannotFail;
                                runJob = Warning;
                                break;

                            case GEnum.ValidateField.DocCurrKey:
                                runAllrows = true;
                                runARSODK = cannotFail;
                                runCSCPSDK = cannotFail;
                                runCSCSIDK = cannotFail;
                                break;

                            case GEnum.ValidateField.ItmKey:
                                runARSODK = cannotFail;
                                runCSCPSDK = cannotFail;
                                runCSCSIDK = cannotFail;
                                break;

                            case GEnum.ValidateField.ItmJobKey:
                                runARSODK = cannotFail;
                                break;

                            case GEnum.ValidateField.DelDocItmKey:
                                runCSCPSDK = cannotFail;
                                break;

                            //Check CSCPS_DK
                        }
                        break;
                    #endregion

                    #region Purchase Order
                    case (int)GEnum.SystemCode.Purchase_Order:
                        switch (validateField)
                        {
                            case GEnum.ValidateField.DocConKey:
                            case GEnum.ValidateField.DocCurrKey:
                                runAllrows = true;
                                runItmQtyLink = cannotFail;
                                runItmOrderStatus = cannotFail;
                                break;

                            case GEnum.ValidateField.DelDocItmKey:
                            case GEnum.ValidateField.ItmKey:
                                runItmQtyLink = cannotFail;
                                runItmOrderStatus = cannotFail;
                                runARSODK = Warning;
                                runARDODK = Warning;
                                runARIVDK = Warning;
                                break;

                            case GEnum.ValidateField.ItmQty:
                            case GEnum.ValidateField.ItmQtyAdj:
                            case GEnum.ValidateField.ItmUOMKey:
                                runItmQtyBalance = cannotFail;
                                break;

                            case GEnum.ValidateField.ItmOrderStatus:
                                runItmOrderStatus = cannotFail;
                                break;
                        }
                        break;
                    #endregion

                    #region Purchase Shimpment/ Purchase Delivery
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                        switch (validateField)
                        {
                            case GEnum.ValidateField.DocConKey:
                            case GEnum.ValidateField.DocCurrKey:
                                runAllrows = true;
                                runAPPODK = cannotFail;
                                break;

                            case GEnum.ValidateField.DelDocItmKey:
                                runARSODK = Warning;
                                runARDODK = Warning;
                                runARIVDK = Warning;
                                break;

                            case GEnum.ValidateField.ItmKey:
                                runAPPODK = cannotFail;
                                runARSODK = Warning;
                                runARDODK = Warning;
                                runARIVDK = Warning;
                                break;

                            case GEnum.ValidateField.ItmJobKey:
                                runAPPODK = Warning;
                                break;
                        }
                        break;
                    #endregion

                    #region Purchase Invoice/CN/DN
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        switch (validateField)
                        {
                            case GEnum.ValidateField.DocConKey:
                            case GEnum.ValidateField.DocCurrKey:
                                runAllrows = true;
                                runAPPODK = cannotFail;
                                runAPPDDK = cannotFail;
                                break;

                            case GEnum.ValidateField.DelDocItmKey:
                                runARSODK = Warning;
                                runARDODK = Warning;
                                runARIVDK = Warning;
                                break;

                            case GEnum.ValidateField.ItmKey:
                                runAPPODK = cannotFail;
                                runAPPDDK = cannotFail;
                                runARSODK = Warning;
                                runARDODK = Warning;
                                runARIVDK = Warning;
                                break;

                            case GEnum.ValidateField.ItmJobKey:
                                runAPPODK = Warning;
                                runAPPDDK = Warning;
                                break;
                        }
                        break;
                    #endregion

                    #region Order Consignment
                    case (int)GEnum.SystemCode.Order_Consignment:
                        switch (validateField)
                        {
                            case GEnum.ValidateField.DocConKey:
                            case GEnum.ValidateField.DocCurrKey:
                                runAllrows = true;
                                runItmQtyLink = cannotFail;
                                break;

                            case GEnum.ValidateField.DelDocItmKey:
                            case GEnum.ValidateField.ItmKey:
                                runItmQtyLink = cannotFail;
                                break;

                            case GEnum.ValidateField.ItmQty:
                            case GEnum.ValidateField.ItmQtyAdj:
                            case GEnum.ValidateField.ItmUOMKey:
                                runItmQtyBalance = cannotFail;
                                break;
                        }
                        break;
                    #endregion

                    #region Issue Consignment
                    case (int)GEnum.SystemCode.Issue_Consignment:
                        switch (validateField)
                        {
                            case GEnum.ValidateField.DocConKey:
                            case GEnum.ValidateField.DocCurrKey:
                                runAllrows = true;
                                runItmQtyLink = cannotFail;
                                runARSODK = cannotFail;
                                break;

                            case GEnum.ValidateField.DelDocItmKey:
                                runItmQtyLink = cannotFail;
                                break;

                            case GEnum.ValidateField.ItmKey:
                                runItmQtyLink = cannotFail;
                                runARSODK = cannotFail;
                                break;

                            case GEnum.ValidateField.ItmQty:
                            case GEnum.ValidateField.ItmUOMKey:
                                runItmQtyBalance = cannotFail;
                                break;
                        }
                        break;
                    #endregion

                    #region Received Consignment
                    case (int)GEnum.SystemCode.Received_Consignment:
                        switch (validateField)
                        {
                            case GEnum.ValidateField.DocConKey:
                            case GEnum.ValidateField.DocCurrKey:
                                runAllrows = true;
                                runItmQtyLink = cannotFail;
                                runCSCPODK = cannotFail;
                                runCSCPSDK = cannotFail;
                                break;

                            case GEnum.ValidateField.ItmKey:
                                runCSCPODK = cannotFail;
                                runCSCPSDK = cannotFail;
                                break;

                            case GEnum.ValidateField.ItmQty:
                                runItmQtyBalance = cannotFail;
                                break;
                            case GEnum.ValidateField.DelDocItmKey:
                                runItmQtyLink = cannotFail;
                                break;
                        }
                        break;
                    #endregion

                    #region Consignment Settlement
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                        runConsignmentSettlement = true;
                        break;
                    #endregion

                    #region Inventory Adjustment
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                        switch (validateField)
                        {
                            case GEnum.ValidateField.DelDocItmKey:
                            case GEnum.ValidateField.ItmKey:
                                runCSCPSDK = cannotFail;
                                break;
                        }
                        break;
                    #endregion

                    #region Quotation
                    case (int)GEnum.SystemCode.Quotation:
                        return true;
                        //switch (validateField)
                        //{
                        //    case GEnum.ValidateField.DocConKey:
                        //        runJob = Warning;
                        //        break;
                        //}
                        //break;
                    #endregion

                    #region Payment
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                        switch (validateField)
                        {
                            case GEnum.ValidateField.DocConKey:
                                runJobPayment = Warning;
                                break;
                        }
                        break;
                    #endregion

                    default:
                        return true;
                }
                #endregion

                UltraGridRow row = null;

                //to check New Row or not
                row = grd.ActiveRow;
                if (row == null)
                {
                    return true;
                }
                else
                {
                    hasColumn = row.Cells.IndexOf("ItmKey");
                    if (hasColumn != -1)
                    {
                        int? itmKey = GFunc.NEInt(row.Cells["ItmKey"].Value, 0);
                        if (itmKey == 0)//if New row no need to check,skip
                            return true;
                    }
                }

                for (int i = 0; i < grd.Rows.Count; i++)
                {
                    row = grd.Rows[i];

                    //move to the active row
                    if (runAllrows == false)
                        row = grd.ActiveRow;

                    //get Current ItmSN
                    hasColumn = row.Cells.IndexOf("ItmSN");
                    if (hasColumn != -1)
                        ItmSN = GFunc.NEDec(row.Cells["ItmSN"].Value, 0);


                    #region Check ARSODK
                    if (runARSODK > 0)
                    {

                        if (GFunc.NEInt(row.Cells["ARSODK"].Value, 0) > 0)
                        {
                            if (runARSODK == cannotFail)
                            {
                                MsgBox.Show("This transaction is link to Sales Order at Item SN " + ItmSN + " , your action is cancelled");
                                return false;
                            }
                            else
                            {
                                if (MsgBox.Show("This transaction is link to Sales Order at Item SN " + ItmSN + ", continue?", GEnum.MsgBoxDefaultButton.DefaultButton1, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                                    return false;
                            }
                        }
                    }
                    #endregion

                    #region Check ARDODK
                    if (runARDODK > 0)
                    {

                        if (GFunc.NEInt(row.Cells["ARDODK"].Value, 0) > 0)
                        {
                            if (runARDODK == cannotFail)
                            {
                                MsgBox.Show("This transaction is link to Delivery Order at Item SN " + ItmSN + ", your action is cancelled");
                                return false;
                            }
                            else
                            {
                                if (MsgBox.Show("This transaction is link to Delivery Order at Item SN " + ItmSN + ", continue?", GEnum.MsgBoxDefaultButton.DefaultButton1, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                                    return false;
                            }
                        }
                    }
                    #endregion

                    #region Check ARIVDK
                    if (runARIVDK > 0)
                    {

                        if (GFunc.NEInt(row.Cells["ARIVDK"].Value, 0) > 0)
                        {
                            if (runARIVDK == cannotFail)
                            {
                                MsgBox.Show("This transaction is link to Invoice at Item SN " + ItmSN + ", your action is cancelled");
                                return false;
                            }
                            else
                            {
                                if (MsgBox.Show("This transaction is link to Invoice at Item SN " + ItmSN + ", continue?", GEnum.MsgBoxDefaultButton.DefaultButton1, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                                    return false;
                            }
                        }
                    }
                    #endregion

                    #region Check APPODK
                    if (runAPPODK > 0)
                    {
                        if (GFunc.NEInt(row.Cells["APPODK"].Value, 0) > 0)
                        {
                            if (runAPPODK == cannotFail)
                            {
                                MsgBox.Show("This transaction is link to Purchase Order at Item SN " + ItmSN + ", your action is cancelled");
                                return false;
                            }
                            else
                            {
                                if (MsgBox.Show("This transaction is link to Purchase Order at Item SN " + ItmSN + ", continue?", GEnum.MsgBoxDefaultButton.DefaultButton1, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                                    return false;
                            }
                        }
                    }
                    #endregion

                    #region Check APPDDK
                    if (runAPPDDK > 0)
                    {

                        if (GFunc.NEInt(row.Cells["APPDDK"].Value, 0) > 0)
                        {
                            if (runAPPDDK == cannotFail)
                            {
                                MsgBox.Show("This transaction is link to Purchase Delivery at Item SN " + ItmSN + ", your action is cancelled");
                                return false;
                            }
                            else
                            {
                                if (MsgBox.Show("This transaction is link to Purchase Delivery at Item SN " + ItmSN + ", continue?", GEnum.MsgBoxDefaultButton.DefaultButton1, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                                    return false;
                            }
                        }
                    }
                    #endregion

                    #region Check CPODK
                    if (runCSCPODK > 0)
                    {

                        if (GFunc.NEInt(row.Cells["CSCPODK"].Value, 0) > 0)
                        {
                            if (runCSCPODK == cannotFail)
                            {
                                MsgBox.Show("This transaction is link to Consignment Orderat Item SN " + ItmSN + ", your action is cancelled");
                                return false;
                            }
                            else
                            {
                                if (MsgBox.Show("This transaction is link to Consignment Order at Item SN " + ItmSN + ", continue?", GEnum.MsgBoxDefaultButton.DefaultButton1, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                                    return false;
                            }
                        }
                    }
                    #endregion

                    #region Check CPSDK
                    if (runCSCPSDK > 0)
                    {

                        if (GFunc.NEInt(row.Cells["CSCPSDK"].Value, 0) > 0)
                        {
                            if (runCSCPSDK == cannotFail)
                            {
                                MsgBox.Show("This transaction is link to Consignment Settlement at Item SN " + ItmSN + ", your action is cancelled");
                                return false;
                            }
                            else
                            {
                                if (MsgBox.Show("This transaction is link to Consignment Settlement at Item SN " + ItmSN + ", continue?", GEnum.MsgBoxDefaultButton.DefaultButton1, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                                    return false;
                            }
                        }
                    }
                    #endregion

                    #region Check CSIDK
                    if (runCSCSIDK > 0)
                    {

                        if (GFunc.NEInt(row.Cells["CSCSIDK"].Value, 0) > 0)
                        {
                            if (runCSCSIDK == cannotFail)
                            {
                                MsgBox.Show("This transaction is link to Issue Consignment at Item SN " + ItmSN + ", your action is cancelled");
                                return false;
                            }
                            else
                            {
                                if (MsgBox.Show("This transaction is link to Issue Consignment at Item SN " + ItmSN + ", continue?", GEnum.MsgBoxDefaultButton.DefaultButton1, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                                    return false;
                            }
                        }
                    }
                    #endregion

                    #region Check Job Used in detail
                    //if (runJob > 0)
                    //{
                    //    if (GFunc.NEInt(row.Cells["ItmJobKey"].Value, 0) > 0)
                    //        jobUsedinDetail = true;
                    //}

                    if (runJobPayment > 0)
                    {
                        if ((int)row.Cells["ExpJobKey"].Value > 0)
                            jobUsedinDetail = true;
                    }
                    #endregion

                    #region Check ItmQtyLink, ItmQty, ItmOrderStatus

                    hasColumn = row.Cells.IndexOf("ItmSN");
                    if (hasColumn != -1)
                    {
                        switch (GFunc.GetINTypeGroup(GFunc.NEInt(row.Cells["ItmType"].Value, 0)))
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                                if (runItmQtyLink > 0)
                                {
                                    if (GFunc.NEDec(row.Cells["ItmQtyLink"].Value, 0) != 0)
                                        if (runItmQtyLink == cannotFail)
                                        {
                                            MsgBox.Show("This transaction is link to other documents at Item SN " + GFunc.NEInt(ItmSN,0) + ".<br/>Your action is cancelled.");
                                            return false;
                                        }
                                        else
                                        {
                                            if (MsgBox.Show("This transaction is link to other documents at Item SN " + GFunc.NEInt(ItmSN, 0) + ", continue?", GEnum.MsgBoxDefaultButton.DefaultButton1, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                                                return false;
                                        }
                                    return true;
                                }

                                if (runItmQtyBalance > 0)
                                {
                                    switch (objDoc.DocCodeKey)
                                    {
                                        case (int)GEnum.SystemCode.Sales_Order:
                                        case (int)GEnum.SystemCode.Reserve_Order:
                                        case (int)GEnum.SystemCode.Purchase_Order:
                                        case (int)GEnum.SystemCode.Order_Consignment:
                                            if (GFunc.NEDec(row.Cells["ItmQty"].Value, 0) - GFunc.NEDec(row.Cells["ItmQtyLink"].Value, 0) - GFunc.NEDec(row.Cells["ItmQtyAdj"].Value, 0) < 0)
                                            {
                                                if (runItmQtyBalance == cannotFail)
                                                {
                                                    MsgBox.Show("The input qty should not be less than delivered qty at Item SN " + GFunc.NEInt(ItmSN, 0) + ".<br/>Your action is cancelled.");
                                                    return false;
                                                }
                                                else
                                                {
                                                    if (MsgBox.Show("The input qty should not be less than delivered qty at Item SN " + GFunc.NEInt(ItmSN, 0) + ", continue?", GEnum.MsgBoxDefaultButton.DefaultButton1, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                                                        return false;
                                                }
                                            }
                                            break;

                                        case (int)GEnum.SystemCode.Received_Consignment:
                                        case (int)GEnum.SystemCode.Issue_Consignment:
                                        case (int)GEnum.SystemCode.Return_Consignment:
                                            if (GFunc.NEDec(row.Cells["ItmQty"].Value, 0) - GFunc.NEDec(row.Cells["ItmQtyLink"].Value, 0) < 0)
                                            {
                                                if (runItmQtyBalance == cannotFail)
                                                {
                                                    MsgBox.Show("The input qty should not be less than delivered qty at Item SN " + GFunc.NEInt(ItmSN, 0) + ".<br/>Your action is cancelled.");
                                                    return false;
                                                }
                                                else
                                                {
                                                    if (MsgBox.Show("The input qty should not be less than delivered qty at Item SN " + GFunc.NEInt(ItmSN, 0) + ", continue?", GEnum.MsgBoxDefaultButton.DefaultButton1, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                                                        return false;
                                                }
                                            }
                                            break;
                                    }
                                }
                                break;

                            case (int)GEnum.INTypeGrp.Charges:
                                if (runItmOrderStatus > 0)
                                {
                                    if ((int)row.Cells["ItmOrderStatus"].Value == (int)GEnum.OrderStatus.Delivered)
                                    {
                                        if (runItmOrderStatus == cannotFail)
                                        {
                                            MsgBox.Show("This item at Item SN " + GFunc.NEInt(ItmSN, 0) + " has been delivered already.<br/>Your action is cancelled.");
                                            return false;
                                        }
                                        else
                                        {
                                            if (MsgBox.Show("This item at Item SN " + GFunc.NEInt(ItmSN, 0) + " has been delivered already, continue?", GEnum.MsgBoxDefaultButton.DefaultButton1, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                                                return false;
                                        }
                                    }
                                }
                                break;

                        }//End Switch
                    }
                    #endregion

                    if (runAllrows == false)
                        break;
                }

                //if (jobUsedinDetail)
                //{
                //    if (MsgBox.Show("The system will clear all detail Job Information, confirm?", GEnum.MsgBoxDefaultButton.DefaultButton1, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                //        return false;
                //}

                #region Check Consignment Settlement LineType
                if (runConsignmentSettlement)
                {
                    int lineType = GFunc.NEInt(row.Cells["LineType"].Value, 0);

                    if (lineType != (int)GEnum.RecDetailType.DItmExpenses && lineType != 0)
                    {
                        MsgBox.Show("You can only delete expenses line.<br/>Your action is cancelled");
                        return false;
                    }
                }
                #endregion

                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static int ShipMark_GetLast(int? ConKey, string ShipName)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return ShipMark_GetLast(cn, ConKey, ShipName);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static int ShipMark_GetLast(SqlConnection cn, int? ConKey, string ShipName)
        {
            try
            {
                //Checking
                if (GFunc.IsNE(ConKey) || GFunc.IsNE(ShipName))
                    return 0;

                //Get last ShipMark
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@ConKey", ConKey));
                parmList.Add(new SqlParameter("@ShipName", ShipName));

                DataTable dt = GFunc.ExecuteProc(cn, "ROMSTShipMarkLast_Get", parmList);
                if (GFunc.IsNE(dt.Rows[0][0]))
                    return 0;
                else
                    return (int)dt.Rows[0][0];
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static int ShipMark_GetNew(int? conKey, string shipName)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return ShipMark_GetNew(cn, conKey, shipName);
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        public static int ShipMark_GetNew(SqlConnection cn, int? conKey, string shipName)
        {
            MSTShipName objMSTShipName = null;
            int vMaxShipMark = 0;
            bool runInitialNum = false;

            try
            {
                if (GFunc.IsNEZ(conKey))
                {
                    MsgBox.Show(cn, MsgID.Document.CustomerIDcannotbeEmpty);
                    return 0;
                }
                if (GFunc.IsNE(shipName))
                {
                    MsgBox.Show(cn, MsgID.Document.ShipNamecannotbeEmpty);
                    return 0;
                }
                if (MsgBox.Show(cn, MsgID.Document.GeneratenewShipmark, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                {
                    //Get Max ShipMark
                    DataTable dt = GFunc.ExecuteProc(cn, "ROMSTShipMarkLastMax_Get", null);
                    if (dt.Rows.Count > 0)
                    {
                        if (GFunc.IsNEZ(dt.Rows[0][0]))
                            runInitialNum = true;
                        else
                            int.TryParse(dt.Rows[0][0].ToString(), out vMaxShipMark);
                    }
                    else
                        runInitialNum = true;

                    if (runInitialNum)
                    {
                        int ShipMarkIntNum = SysOptionUtility.GetInt(GVar.SystemOption.OpID.ShipMarkInitialNumber, cn);

                        if (GFunc.IsNEZ(ShipMarkIntNum))
                            vMaxShipMark = 0;
                        else
                            int.TryParse(ShipMarkIntNum.ToString(), out vMaxShipMark);
                    }

                    //Add Shipmark record
                    objMSTShipName = MSTShipName.Get(cn, shipName,(int)conKey); //Jack Added Conkey parameter; 9 Nov 2012
                    if (GFunc.IsNE(objMSTShipName) == false)
                    {
                        MSTShipNameDetItm objDet = MSTShipNameDetItm.NewChild();
                        objDet.ShipNameKey = objMSTShipName.ShipNameKey;
                        objDet.ShipMark = vMaxShipMark + 1;
                        objDet.Insert();

                        return (vMaxShipMark + 1);
                    }
                }
                return 0;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            finally
            {
                objMSTShipName = null;
            }
        }//Completed
        //Modified By May on 12-Dec-2022
        public static void ExportToDMAS(int docKey, int docCodeKey, string docID, DateTime docDate, string conID,int UID,string ApproveUser="",DateTime? ApproveDate=null)
        {
            ReportLoader _ReportLoader = new ReportLoader();
            //Get Report Data With repKey
            SYSRep rep = SYSRep.Get(docCodeKey);
            SYSRepRpt rpt = SYSRepRpt.Get(docCodeKey, "", 4);//option 4 For DMAS
            if (rpt.RptNm == "")
                return;
            _ReportLoader._reportFileName = rpt.RptNm;


            //Common Process before Print/Fax/Email/Preview
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@DocKey", docKey));
            parmList.Add(new SqlParameter("@DocCodeKey", docCodeKey));//Request for Quotation RepKey           
            _ReportLoader.ReportSqlParameter = parmList;

            FileStream fs = new FileStream(@"C:\Temp\TAAgentServiceLog1.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            m_streamWriter.WriteLine(Application.StartupPath+ "\n"+rpt.RptNm);
            m_streamWriter.Flush();
            m_streamWriter.Close();

            DataTable dtSource = new DataTable();
            try
            {
                 //Get report Datasource with store procedure name and parameter from Sys_repCriter               
                 dtSource = GFunc.ExecuteProc(rep.RPTRecordSource1, _ReportLoader.ReportSqlParameter);
            }
            catch(Exception ex)
            {
                if (!ex.Message.Contains("There are no records for this report"))
                    throw(ex);
            }
           

            if (dtSource.Rows.Count == 0)
                return;

            //Add QRCode column and data into the datasource
            // dtSource = _ReportLoader.GetSourceWithQRCode(dtSource, Path.GetExtension(_ReportLoader._reportFileName));

            #region +++ setting report parameters +++
            _ReportLoader.ReportParameter = GlobalUI.GetReportParameters();
            int CheckSwitch = GFunc.NEInt(rpt.RptLayOut, 0);
            switch (CheckSwitch)
            {
                case 10: //Option 'All'
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideColHead", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmMarking", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmID", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmDes", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmQty", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmPrice", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmAmt", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideFinalTotal", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pShwItmOnly", false));
                    break;

                case 20: //Option 'Description'                 
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideColHead", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmMarking", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmID", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmDes", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmQty", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmPrice", true));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmAmt", true));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideFinalTotal", true));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pShwItmOnly", true));
                    break;

                case 30: //Option 'Price'                
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideColHead", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmMarking", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmID", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmDes", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmQty", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmPrice", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmAmt", true));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideFinalTotal", true));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pShwItmOnly", true));
                    break;

                case 40: //Option 'Amount'                    
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideColHead", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmMarking", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmID", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmDes", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmQty", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmPrice", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmAmt", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideFinalTotal", true));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pShwItmOnly", true));
                    break;
            }

            _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmCount", !rpt.ShwItmCount));
            _ReportLoader.ReportParameter.Add(new ReportParameter("pHideLetterHead", !rpt.ShwLetterHead));
            _ReportLoader.ReportParameter.Add(new ReportParameter("pRepTitle", rpt.RptTitle));

            #endregion +++ report parameters +++

            if (_ReportLoader._reportFileName.ToUpper().EndsWith(".RPX"))
            {
                _ReportLoader.rpxDoc = new DataDynamics.ActiveReports.ActiveReport();
                //Set report layout with report file name
                _ReportLoader.rpxDoc.LoadLayout(Application.StartupPath + "\\Reports\\" + _ReportLoader._reportFileName);//Orignal

                if (dtSource != null && dtSource.Rows.Count > 0)
                {
                    _ReportLoader.rpxDoc.DataSource = dtSource;
                }
                else
                {
                    return;
                }

                _ReportLoader.rpxDoc.AddScriptReference(Application.StartupPath + "\\TAReport.dll");

                if (GFunc.IsNE(_ReportLoader.ReportParameter) == false)
                {
                    foreach (ReportParameter item in _ReportLoader.ReportParameter)
                    {
                        try
                        {
                            _ReportLoader.rpxDoc.Parameters[item.ParameterName].PromptUser = false;
                            _ReportLoader.rpxDoc.Parameters[item.ParameterName].Value = item.ParameterValue.ToString();
                            _ReportLoader.rpxDoc.Parameters[item.ParameterName].DefaultValue = item.ParameterValue.ToString();
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                    }
                }
                _ReportLoader.rpxDoc.Run();
            }
            else if (_ReportLoader._reportFileName.ToUpper().EndsWith(".RPT"))
            {
                _ReportLoader.rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                _ReportLoader.rptDoc.Load(Application.StartupPath + @"\Reports\" + _ReportLoader._reportFileName);
                _ReportLoader.rptDoc.SetDataSource(dtSource);

                foreach (ReportParameter p in _ReportLoader.ReportParameter)
                {
                    if (_ReportLoader.rptDoc.ParameterFields.Find(p.ParameterName, "") != null)
                        _ReportLoader.rptDoc.SetParameterValue(p.ParameterName, p.ParameterValue);
                }
                //May added on 11-Nov-2025, to set missing parameters of reports
                var missingParams = _ReportLoader.rptDoc.ParameterFields
                .Cast<CrystalDecisions.Shared.ParameterField>()
                .Where(p => (p.CurrentValues == null || p.CurrentValues.Count == 0))
               .ToList();               

                foreach (CrystalDecisions.Shared.ParameterField pf in missingParams)
                {                   
                    if (!pf.HasCurrentValue)
                        if (pf.DefaultValues.Count > 0)
                            _ReportLoader.rptDoc.SetParameterValue(pf.Name, pf.DefaultValues[0]);
                }
            }

            string reportGenPath = _ReportLoader.CreateReportFile(ReportFileType.AcrobatPDFFile);
            
            //May change End
            string currentUserID = SECUser.Get(AppInfor.CurrentUserKey).UserID;

            try
            {
                string[] strs = docID.Split('/');
                string str = reportGenPath;
                if (strs.Length >= 3)
                {
                    str = Path.GetDirectoryName(reportGenPath) + @"\" + "DO" + docDate.ToString("yyyyMM") + "_";
                    str += strs[2] + ".pdf";
                }

                if (str != reportGenPath)
                {
                    if (File.Exists(str))
                        File.Delete(str);

                    File.Move(reportGenPath, str);

                    reportGenPath = str;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
         
           
            if (!GroupExists(rpt.Custom1))
            {
                if (rpt.Custom1 == "")
                    throw new Exception("Export to DMAS failed. Security Group ID cannot be blank in DMAS.");
                else
                    throw new Exception("Export to DMAS failed. Group ID '" + rpt.Custom1 + "' cannot match in DMAS.");

                return;
            }

            if (ApproveDate == null)
                ApproveDate = DateTime.Today;

            using (SqlConnection cn = new SqlConnection(Database.GetDMASConnection()))
            {

                cn.Open();

                // Using existing sql connection.
                using (SqlCommand cm = cn.CreateCommand())
                {

                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "Document_AddUpdate";

                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    //Check Insert or update

                    cm.Parameters.AddWithValue("@Option", 0);

                    cm.Parameters.AddWithValue("@DocKey", 0);
                    cm.Parameters.AddWithValue("@NewDocKey", 0);
                    cm.Parameters.AddWithValue("@DocCodeKey", docCodeKey);
                    cm.Parameters.AddWithValue("@DocName", Path.GetFileNameWithoutExtension(reportGenPath));
                    cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;

                    if (reportGenPath == null)
                        cm.Parameters.AddWithValue("@DocDes", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@DocDes", Path.GetFileNameWithoutExtension(reportGenPath));

                    cm.Parameters.AddWithValue("@IsScan", false);

                    cm.Parameters.AddWithValue("@DocFileName", ((GEnum.SystemCode)docCodeKey).ToString() + " DocNum=" + docID);

                    cm.Parameters.AddWithValue("@DocTypeID", ".pdf");
                    cm.Parameters.AddWithValue("@DocStatus", "Filed");

                    FileStream fss = null;
                    BinaryReader br = null;

                    byte[] buffer = null;
                    fss = new FileStream(reportGenPath, FileMode.Open, FileAccess.Read);
                    br = new BinaryReader(fss);
                    long numBytes = new FileInfo(reportGenPath).Length;
                    buffer = br.ReadBytes((int)numBytes);
                    br.Close();
                    fss.Close();

                    long sizeInKB = numBytes > 0 ? numBytes / long.Parse("1024") : 0;

                    cm.Parameters.AddWithValue("@DocSizeKB", sizeInKB);

                    cm.Parameters.AddWithValue("@CreationDate", DateTime.Now);

                    cm.Parameters.AddWithValue("@CreatedByUserID", AppInfor.CurrentUserID);

                    cm.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

                    cm.Parameters.AddWithValue("@ModifiedByUserID", AppInfor.CurrentUserID);

                    cm.Parameters.AddWithValue("@ApprovedDate", ApproveDate);

                    cm.Parameters.AddWithValue("@ApprovedByUserID", ApproveUser);

                    if (rpt.Custom1 == null)
                        cm.Parameters.AddWithValue("@SecGrp", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@SecGrp", rpt.Custom1);

                    cm.Parameters.AddWithValue("@MetaData01", docID);

                    cm.Parameters.AddWithValue("@MetaData02", "");

                    cm.Parameters.AddWithValue("@MetaData03", 100);

                    cm.Parameters.AddWithValue("@MetaData04", docKey);

                    cm.Parameters.AddWithValue("@MetaData05", conID);

                    cm.Parameters.AddWithValue("@QRCodeData", docKey);

                    cm.Parameters.AddWithValue("@OCRText", "");

                    cm.Parameters.AddWithValue("@DocFile", buffer);

                    cm.Parameters.AddWithValue("@PageRotation", (int)0);

                    cm.Parameters.AddWithValue("@DocSetStatus", "None");

                    cm.Parameters.AddWithValue("@DocSetMasterDocKey", (int)0);

                    int affect = cm.ExecuteNonQuery();

                }// Already close and dispose sql connection.

            }

            //using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            //{

            //    cn.Open();

            //    // Using existing sql connection.
            //    using (SqlCommand cm = cn.CreateCommand())
            //    {

            //        cm.CommandType = CommandType.StoredProcedure;
            //        cm.CommandText = "DMAS_UpdateExportStatus";

            //        cm.Parameters.AddWithValue("@UID", UID);                  

            //        int affect = cm.ExecuteNonQuery();

            //    }// Already close and dispose sql connection.

            //}
        }
        public static void ExportToDMAS(Document objDoc, bool DocApprovalRequired = false)
        {
            string saveFilePath = SysOptionUtility.ExportFilePath + SysOptionUtility.DatabaseBranchCode + "\\" + ConfigurationManager.AppSettings[objDoc.DocCodeKey.ToString()] + "\\";

            /* commented by YST 
            if ((objDoc.DocCodeKey != (int)GEnum.SystemCode.Quotation 
                && objDoc.DocCodeKey != (int)GEnum.SystemCode.Purchase_Order 
                && objDoc.DocCodeKey != (int)GEnum.SystemCode.Sales_Invoice 
                && objDoc.DocCodeKey != (int)GEnum.SystemCode.Cash_Sale 
                && objDoc.DocCodeKey != (int)GEnum.SystemCode.Purchase_Invoice)
                || !SysOptionUtility.DatabaseBranchCode.Equals("ADL"))//May 30-Apr-2023
                return;//added by May on 24 Nov 2021. Not to export to DMAS. Should have changed in calling places but do not want changes in many files for the moment
             */

            if (!(objDoc.DocCodeKey == (int)GEnum.SystemCode.Payment_Issue || objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Adjustment))
            {
                if (!DocApprovalRequired || SysOptionUtility.DatabaseBranchCode == DBCode.BHM) /* modified by YST */
                return;
            }

            ReportLoader _ReportLoader = new ReportLoader();
            //Get Report Data With repKey
            SYSRep rep = SYSRep.Get(objDoc.DocCodeKey);
            SYSRepRpt rpt = SYSRepRpt.Get(objDoc.DocCodeKey, "", 4);//option 4 For DMAS
            if (rpt.RptNm == "")
                return;
            _ReportLoader._reportFileName = rpt.RptNm;


            //Common Process before Print/Fax/Email/Preview
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@DocKey", objDoc.DocKey));
            parmList.Add(new SqlParameter("@DocCodeKey", objDoc.DocCodeKey));//Request for Quotation RepKey           
            _ReportLoader.ReportSqlParameter = parmList;

            //Get report Datasource with store procedure name and parameter from Sys_repCriter               
            DataTable dtSource = GFunc.ExecuteProc(rep.RPTRecordSource1, _ReportLoader.ReportSqlParameter);

            //Add QRCode column and data into the datasource
            // dtSource = _ReportLoader.GetSourceWithQRCode(dtSource, Path.GetExtension(_ReportLoader._reportFileName));            

            #region +++ setting report parameters +++
            _ReportLoader.ReportParameter = GlobalUI.GetReportParameters();
            int CheckSwitch = GFunc.NEInt(rpt.RptLayOut, 0);
            switch (CheckSwitch)
            {
                case 10: //Option 'All'
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideColHead", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmMarking", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmID", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmDes", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmQty", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmPrice", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmAmt", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideFinalTotal", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pShwItmOnly", false));
                    break;

                case 20: //Option 'Description'                 
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideColHead", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmMarking", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmID", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmDes", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmQty", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmPrice", true));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmAmt", true));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideFinalTotal", true));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pShwItmOnly", true));
                    break;

                case 30: //Option 'Price'                
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideColHead", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmMarking", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmID", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmDes", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmQty", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmPrice", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmAmt", true));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideFinalTotal", true));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pShwItmOnly", true));
                    break;

                case 40: //Option 'Amount'                    
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideColHead", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmMarking", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmID", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmDes", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmQty", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmPrice", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmAmt", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideFinalTotal", true));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pShwItmOnly", true));
                    break;
            }

            _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmCount", !rpt.ShwItmCount));
            _ReportLoader.ReportParameter.Add(new ReportParameter("pHideLetterHead", !rpt.ShwLetterHead));
            _ReportLoader.ReportParameter.Add(new ReportParameter("pRepTitle", rpt.RptTitle));

            #endregion +++ report parameters +++

            bool generate2nd = false;

            Generate:

            if (_ReportLoader._reportFileName.ToUpper().EndsWith(".RPX"))
            {
                _ReportLoader.rpxDoc = new DataDynamics.ActiveReports.ActiveReport();
                //Set report layout with report file name
                _ReportLoader.rpxDoc.LoadLayout(Application.StartupPath + "\\Reports\\" + _ReportLoader._reportFileName);//Orignal

                if (dtSource != null && dtSource.Rows.Count > 0)
                {
                    _ReportLoader.rpxDoc.DataSource = dtSource;
                }
                else
                {
                    return;
                }

                _ReportLoader.rpxDoc.AddScriptReference(Application.StartupPath + "\\TAReport.dll");

                if (GFunc.IsNE(_ReportLoader.ReportParameter) == false)
                {
                    foreach (ReportParameter item in _ReportLoader.ReportParameter)
                    {
                        try
                        {
                                _ReportLoader.rpxDoc.Parameters[item.ParameterName].PromptUser = false;
                                _ReportLoader.rpxDoc.Parameters[item.ParameterName].Value = item.ParameterValue.ToString();
                                _ReportLoader.rpxDoc.Parameters[item.ParameterName].DefaultValue = item.ParameterValue.ToString();                           
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
                _ReportLoader.rpxDoc.Run();
            }
            else if (_ReportLoader._reportFileName.ToUpper().EndsWith(".RPT"))
            {
                _ReportLoader.rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                _ReportLoader.rptDoc.Load(Application.StartupPath + @"\Reports\" + _ReportLoader._reportFileName);
               
                if (dtSource.Rows.Count > 0)
                {
                    //dtSource.Rows[dtSource.Rows.Count - 1]["DocStatus"] = "Approved";
                    foreach(DataRow row in dtSource.Rows)
                        row["DocStatus"] = generate2nd ? "Approved" : "Requested";
                    dtSource.AcceptChanges();
                }                
                _ReportLoader.rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                _ReportLoader.rptDoc.Load(Application.StartupPath + @"\Reports\" + _ReportLoader._reportFileName);


                _ReportLoader.rptDoc.SetDataSource(dtSource);

                foreach (ReportParameter p in _ReportLoader.ReportParameter)
                {
                    if (_ReportLoader.rptDoc.ParameterFields.Find(p.ParameterName, "") != null)
                        _ReportLoader.rptDoc.SetParameterValue(p.ParameterName, p.ParameterValue);
                }
            }

            string reportGenPath = _ReportLoader.CreateReportFile(ReportFileType.AcrobatPDFFile);

            try
            {
                string[] strs = objDoc.DocID.Split('/');
                string str = reportGenPath;

                /*
                if (!generate2nd && 
                    (objDoc.DocCodeKey == (int)GEnum.SystemCode.Quotation || 
                     objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Order || 
                     objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Invoice || 
                     objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Sale || 
                     objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Invoice))
                    str = saveFilePath;
                  */

                if (DocApprovalRequired && !generate2nd) 
                    str = saveFilePath;

                /* commented by YST */
                //if (strs.Length >= 3)
                //{
                //    str = Path.GetDirectoryName(str) + @"\" + strs[0] + "_" + strs[1] + "_" + strs[2] + ".pdf";
                //}
                //else
                //{
                //    str = Path.GetDirectoryName(str) + @"\" + objDoc.DocID.Replace('/','_') + ".pdf";
                //}

                str = Path.GetDirectoryName(str) + @"\" + GFunc.ValidateFileName(objDoc.DocID, "_") + ".pdf";

                if (str != reportGenPath)
                {
                    if (File.Exists(str))
                        File.Delete(str);

                    File.Move(reportGenPath, str);

                    reportGenPath = str;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No permission to rename file for DMAS.");
            }

            /*
            if (!generate2nd &&
                    (objDoc.DocCodeKey == (int)GEnum.SystemCode.Quotation ||
                     objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Order ||
                     objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Invoice ||
                     objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Sale ||
                     objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Invoice))
            {
                generate2nd = true;
                goto Generate;
            }  
            */

            if (DocApprovalRequired && !generate2nd)
            {
                generate2nd = true;
                goto Generate;
            }

            //Commented by May on 15-Nov-2024
            //if (!GroupExists(rpt.Custom1))
            //{
            //    if (rpt.Custom1 == "")
            //        throw new Exception("Export to DMAS failed. Security Group ID cannot be blank in DMAS.");
            //    else
            //        throw new Exception("Export to DMAS failed. Group ID '" + rpt.Custom1 + "' cannot match in DMAS.");
            //}

            using (SqlConnection cn = new SqlConnection(Database.GetDMASConnection()))
            // using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.DMASDocConnection))
            {
                cn.Open();

                // Using existing sql connection.
                using (SqlCommand cm = cn.CreateCommand())
                {

                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "Document_AddUpdate";

                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    //Check Insert or update

                    cm.Parameters.AddWithValue("@Option", 0);

                    cm.Parameters.AddWithValue("@DocKey", 0);
                    cm.Parameters.AddWithValue("@NewDocKey", 0);
                    cm.Parameters.AddWithValue("@DocName", Path.GetFileName(reportGenPath));
                    cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;

                    if (reportGenPath == null)
                        cm.Parameters.AddWithValue("@DocDes", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@DocDes", Path.GetFileNameWithoutExtension(reportGenPath));

                    cm.Parameters.AddWithValue("@IsScan", false);

                    cm.Parameters.AddWithValue("@DocFileName", ((GEnum.SystemCode)objDoc.DocCodeKey).ToString() + " DocNum=" + objDoc.DocID);
                    cm.Parameters.AddWithValue("@DocCodeKey", objDoc.DocCodeKey);
                    cm.Parameters.AddWithValue("@DocTypeID", "PrintOut");
                    cm.Parameters.AddWithValue("@DocStatus", "Filed");

                    FileStream fs = null;
                    BinaryReader br = null;

                    byte[] buffer = null;
                    fs = new FileStream(reportGenPath, FileMode.Open, FileAccess.Read);
                    br = new BinaryReader(fs);
                    long numBytes = new FileInfo(reportGenPath).Length;
                    buffer = br.ReadBytes((int)numBytes);
                    br.Close();
                    fs.Close();

                    long sizeInKB = numBytes > 0 ? numBytes / long.Parse("1024") : 0;

                    cm.Parameters.AddWithValue("@DocSizeKB", sizeInKB);

                    cm.Parameters.AddWithValue("@CreationDate", DateTime.Today);

                    cm.Parameters.AddWithValue("@CreatedByUserID", AppInfor.CurrentUserID);

                    cm.Parameters.AddWithValue("@ModifiedDate", DateTime.Today);

                    cm.Parameters.AddWithValue("@ModifiedByUserID", AppInfor.CurrentUserID);

                    cm.Parameters.AddWithValue("@ApprovedDate", DateTime.Today);

                    cm.Parameters.AddWithValue("@ApprovedByUserID", AppInfor.CurrentUserID);

                    if (rpt.Custom1 == null)
                        cm.Parameters.AddWithValue("@SecGrp", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@SecGrp", rpt.Custom1);

                    cm.Parameters.AddWithValue("@MetaData01", objDoc.DocID);

                    cm.Parameters.AddWithValue("@MetaData02", objDoc.DocCodeKey);

                    cm.Parameters.AddWithValue("@MetaData03", objDoc.DocType);

                    cm.Parameters.AddWithValue("@MetaData04", objDoc.DocKey);

                    cm.Parameters.AddWithValue("@MetaData05", MSTCon.Get(Convert.ToInt32(GFunc.GetIntPropertyValue("DocConKey", objDoc))).ConID);

                    cm.Parameters.AddWithValue("@QRCodeData", objDoc.DocKey);

                    cm.Parameters.AddWithValue("@OCRText", "");

                    cm.Parameters.AddWithValue("@DocFile", buffer);

                    cm.Parameters.AddWithValue("@PageRotation", (int)0);

                    cm.Parameters.AddWithValue("@DocSetStatus", "None");

                    cm.Parameters.AddWithValue("@DocSetMasterDocKey", (int)0);

                    int affect = cm.ExecuteNonQuery();
                }                              
            }
        }        
        public static string GetFileFromDMAS(string SecGrpID, string DocName, string docID, int Seq, string DownloadPath = null)
        {
            string filePath = "";
            using (SqlConnection cn = new SqlConnection(Database.GetDMASConnection()))
            {
                cn.Open(); // Using existing sql connection.

                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@SecGrp", SecGrpID));
                parmList.Add(new SqlParameter("@DocName", DocName));
                parmList.Add(new SqlParameter("@DocID", docID));
                parmList.Add(new SqlParameter("@AttachSeq", Seq));

                DataTable dt = GFunc.ExecuteProc(cn, "Document_GetAttachment", parmList);                
                byte[] filedata = (byte[])dt.Rows[0]["DocFile"];
                string extension = Path.GetExtension(dt.Rows[0]["DocName"].ToString()); // "pdf", etc

                /* modified by YST on 2022/03/18 */
                if (DownloadPath == null)
                {
                    filePath = System.IO.Path.GetTempFileName() + extension; // Makes something like "C:\Temp\blah.tmp.pdf"  
                }
                else
                {
                    if (String.IsNullOrEmpty(extension))
                    {
                        extension = ".pdf";
                        DocName = DocName + extension;
                    }
                    else
                    {
                        DocName = dt.Rows[0]["DocName"].ToString();
                    }

                    filePath = DownloadPath + DocName;
                    if (System.IO.File.Exists(filePath))
                    {
                        string DownloadTime = "-" + DateTime.Now.ToString("yyMMddhhmmss");
                        filePath = DownloadPath + DocName.Replace(extension, DownloadTime + extension);
                    }
                }

                File.WriteAllBytes(filePath, filedata);

            }// Already close and dispose sql connection.
            return filePath;
        }               
        public static string GetFileFromDMASByDocKey(int? DocCodeKey, int? DocKey, bool HasPO, string DownloadPath,int Seq=0)
        {
            /* added by YST on 2022/03/18 */
            string fileAttachment = "";
            using (SqlConnection cn = new SqlConnection(Database.GetDMASConnection()))
            {
                cn.Open();
                try
                {
                    List<SqlParameter> parmList = new List<SqlParameter>();
                    parmList.Add(new SqlParameter("@DocCodeKey", DocCodeKey));
                    parmList.Add(new SqlParameter("@DocKey", DocKey));
                    parmList.Add(new SqlParameter("@HasPO", HasPO));
                    parmList.Add(new SqlParameter("@IsFileMerge", false));
                    parmList.Add(new SqlParameter("@BranchCode", SysOptionUtility.DatabaseBranchCode));
                    parmList.Add(new SqlParameter("@Seq", Seq));

                    DataTable dt = GFunc.ExecuteProc(cn, "Document_GetAttachment_ByDocKey", parmList);
                    byte[] filedata;
                    string extension = "", docName = "", fileName = "", docTypeID = "", filePath = "";

                    if (dt == null)
                        return "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        filedata = (byte[])dt.Rows[i]["DocFile"];
                        fileName = GFunc.NEStr(dt.Rows[i]["FileNm"], "");
                        docTypeID = GFunc.NEStr(dt.Rows[i]["DocTypeID"], "");
                        docName = GFunc.NEStr(dt.Rows[i]["DocName"], "");
                        extension = Path.GetExtension(docName);
                        if (String.IsNullOrEmpty(extension)) extension = ".pdf";
                        filePath = DownloadPath + fileName + extension;
                        int c = 1;
                        while (File.Exists(filePath))
                        {
                            filePath = DownloadPath + fileName + "_" + c + extension;
                            c++;
                        }
                        fileAttachment += filePath + "#";
                        File.WriteAllBytes(filePath, filedata);
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.Show(ex.ToString());
                }
                return fileAttachment;
            }
        }
        public static string GetPDFMergeFileFromDMASByDocKey(int? DocCodeKey, int? DocKey, string DocID, bool HasPO, string DownloadPath)
        {

            string fileAttachment = "";
            using (SqlConnection cn = new SqlConnection(Database.GetDMASConnection()))
            {
                cn.Open();
                try
                {
                    List<SqlParameter> parmList = new List<SqlParameter>();
                    parmList.Add(new SqlParameter("@DocCodeKey", DocCodeKey));
                    parmList.Add(new SqlParameter("@DocKey", DocKey));
                    parmList.Add(new SqlParameter("@HasPO", HasPO));
                    parmList.Add(new SqlParameter("@IsFileMerge", false));
                    parmList.Add(new SqlParameter("@BranchCode", SysOptionUtility.DatabaseBranchCode));

                    DataTable dt = GFunc.ExecuteProc(cn, "Document_GetAttachment_ByDocKey", parmList);
                    byte[] filedata;
                    string extension = "", docName = "", fileName = "", docTypeID = "", filePath = "";

                    string licenseNo = "211846748928338721114133297187668";
                    GdPicture10.GdPicturePDF finalPDF = new GdPicture10.GdPicturePDF();
                    finalPDF.SetLicenseNumber(licenseNo);

                    List<string> fNameArr = new List<string>();
                    fileAttachment = DownloadPath + DocID.Replace("/", "") + ".pdf";
                    string nonPDFfiles = "";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        filedata = (byte[])dt.Rows[i]["DocFile"];
                        fileName = GFunc.NEStr(dt.Rows[i]["FileNm"], "");
                        docTypeID = GFunc.NEStr(dt.Rows[i]["DocTypeID"], "");
                        docName = GFunc.NEStr(dt.Rows[i]["DocName"], "");
                        extension = Path.GetExtension(docName);
                        if (String.IsNullOrEmpty(extension))
                            extension = ".pdf";
                        filePath = DownloadPath + fileName + extension;
                        int c = 1;
                        while (File.Exists(filePath))
                        {
                            filePath = DownloadPath + fileName + "_" + c + extension;
                            c++;
                        }

                        File.WriteAllBytes(filePath, filedata);
                        if (extension.ToLower().Equals(".pdf"))
                            fNameArr.Add(filePath);
                        else
                        {
                            nonPDFfiles += "#" + filePath;
                        }
                    }
                    dt.Dispose();

                    finalPDF.MergeDocuments(fNameArr.ToArray(), fileAttachment);
                    
                    foreach(string Individualfile in fNameArr)
                        if(!Individualfile.Equals(fileAttachment))
                            File.Delete(Individualfile);                    

                    if (nonPDFfiles != "")
                    {                        
                         fileAttachment += nonPDFfiles;
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.Show(ex.ToString());
                }
                return fileAttachment;
            }
        }

        /* added by YST */
        public static void ExportDocAttachment(Document objDoc, bool IsDeletedCopy = false, bool ApprovalRequired = false)
        {
            string saveFilePath = SysOptionUtility.ExportFilePath + SysOptionUtility.DatabaseBranchCode + "\\" + ConfigurationManager.AppSettings[objDoc.DocCodeKey.ToString()] + "\\";
            if (!Directory.Exists(saveFilePath))
            {
                MsgBox.Show("Failed to generate file because of folder path exception !", GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                return;
            }

            ReportLoader _ReportLoader = new ReportLoader();
            //Get Report Data With repKey
            SYSRep rep = SYSRep.Get(objDoc.DocCodeKey);
            SYSRepRpt rpt = SYSRepRpt.Get(objDoc.DocCodeKey, "", 4);//option 4 For DMAS
            if (rpt.RptNm == "")
                return;
            _ReportLoader._reportFileName = rpt.RptNm;


            //Common Process before Print/Fax/Email/Preview
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@DocKey", objDoc.DocKey));
            parmList.Add(new SqlParameter("@DocCodeKey", objDoc.DocCodeKey));//Request for Quotation RepKey           
            _ReportLoader.ReportSqlParameter = parmList;

            //Get report Datasource with store procedure name and parameter from Sys_repCriter               
            DataTable dtSource = GFunc.ExecuteProc(rep.RPTRecordSource1, _ReportLoader.ReportSqlParameter);
            if (IsDeletedCopy)
            {
                foreach (DataRow row in dtSource.Rows)
                {
                    row.SetField("DocDisapproveCount", -1);
                }
            }

            if (ApprovalRequired)
            {
                foreach (DataRow row in dtSource.Rows)
                {
                    row.SetField("DocStatus", ApprovalStatus.Requested);
                }
            }

            //Add QRCode column and data into the datasource
            // dtSource = _ReportLoader.GetSourceWithQRCode(dtSource, Path.GetExtension(_ReportLoader._reportFileName));

            #region +++ setting report parameters +++
            _ReportLoader.ReportParameter = GlobalUI.GetReportParameters();
            int CheckSwitch = GFunc.NEInt(rpt.RptLayOut, 0);
            switch (CheckSwitch)
            {
                case 10: //Option 'All'
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideColHead", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmMarking", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmID", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmDes", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmQty", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmPrice", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmAmt", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideFinalTotal", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pShwItmOnly", false));
                    break;

                case 20: //Option 'Description'                 
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideColHead", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmMarking", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmID", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmDes", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmQty", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmPrice", true));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmAmt", true));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideFinalTotal", true));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pShwItmOnly", true));
                    break;

                case 30: //Option 'Price'                
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideColHead", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmMarking", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmID", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmDes", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmQty", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmPrice", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmAmt", true));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideFinalTotal", true));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pShwItmOnly", true));
                    break;

                case 40: //Option 'Amount'                    
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideColHead", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmMarking", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmID", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmDes", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmQty", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmPrice", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmAmt", false));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pHideFinalTotal", true));
                    _ReportLoader.ReportParameter.Add(new ReportParameter("pShwItmOnly", true));
                    break;
            }

            _ReportLoader.ReportParameter.Add(new ReportParameter("pHideItmCount", !rpt.ShwItmCount));
            _ReportLoader.ReportParameter.Add(new ReportParameter("pHideLetterHead", !rpt.ShwLetterHead));
            _ReportLoader.ReportParameter.Add(new ReportParameter("pRepTitle", rpt.RptTitle));

            #endregion +++ report parameters +++

            if (_ReportLoader._reportFileName.ToUpper().EndsWith(".RPX"))
            {
                _ReportLoader.rpxDoc = new DataDynamics.ActiveReports.ActiveReport();
                //Set report layout with report file name
                _ReportLoader.rpxDoc.LoadLayout(Application.StartupPath + "\\Reports\\" + _ReportLoader._reportFileName);//Orignal

                if (dtSource != null && dtSource.Rows.Count > 0)
                {
                    _ReportLoader.rpxDoc.DataSource = dtSource;
                }
                else
                {
                    return;
                }

                _ReportLoader.rpxDoc.AddScriptReference(Application.StartupPath + "\\TAReport.dll");

                if (GFunc.IsNE(_ReportLoader.ReportParameter) == false)
                {
                    foreach (ReportParameter item in _ReportLoader.ReportParameter)
                    {
                        try
                        {
                            _ReportLoader.rpxDoc.Parameters[item.ParameterName].PromptUser = false;
                            _ReportLoader.rpxDoc.Parameters[item.ParameterName].Value = item.ParameterValue.ToString();
                            _ReportLoader.rpxDoc.Parameters[item.ParameterName].DefaultValue = item.ParameterValue.ToString();
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                    }
                }
                _ReportLoader.rpxDoc.Run();
            }
            else if (_ReportLoader._reportFileName.ToUpper().EndsWith(".RPT"))
            {
                _ReportLoader.rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                _ReportLoader.rptDoc.Load(Application.StartupPath + "\\Reports\\" + _ReportLoader._reportFileName);
                _ReportLoader.rptDoc.SetDataSource(dtSource);

                foreach (ReportParameter p in _ReportLoader.ReportParameter)
                {
                    if (_ReportLoader.rptDoc.ParameterFields.Find(p.ParameterName, "") != null)
                        _ReportLoader.rptDoc.SetParameterValue(p.ParameterName, p.ParameterValue);
                }

                var missingParams = _ReportLoader.rptDoc.ParameterFields
                   .Cast<CrystalDecisions.Shared.ParameterField>()
                   .Where(p => (p.CurrentValues == null || p.CurrentValues.Count == 0))
                   .ToList();

                foreach (CrystalDecisions.Shared.ParameterField pf in missingParams)
                {
                    if (!pf.HasCurrentValue)
                        if (pf.DefaultValues.Count > 0)
                            _ReportLoader.rptDoc.SetParameterValue(pf.Name, pf.DefaultValues[0]);
                }
            }

            string reportGenPath = _ReportLoader.CreateReportFile(ReportFileType.AcrobatPDFFile);
            string currentUserID = SECUser.Get(AppInfor.CurrentUserKey).UserID;

            try
            {
                string str = saveFilePath;
                if (objDoc.DocID.Length > 0)
                {
                    str = Path.GetDirectoryName(saveFilePath) + @"\" + (IsDeletedCopy == true ? "WrongCopy_" : "") + GFunc.ValidateFileName(objDoc.DocID,"_") + ".pdf";
                }

                if (str != reportGenPath)
                {
                    if (File.Exists(str))
                        File.Delete(str);

                    File.Move(reportGenPath, str);

                    reportGenPath = str;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No permission to rename the file." + saveFilePath);
            }            
        }
        public static Color GetEstoreCellColor(MSTItm objItm) /* added by YST on 2024-12-10 */
        {
            if (objItm.EStorePrice == -999) return Color.Orange;
            else if (objItm.EStorePrice > 0 && objItm.EStorePrice != objItm.ControlPriceH) return Color.Red;
            else if (objItm.EStorePrice == 0 && objItm.ControlPriceH > 0) return Color.Khaki;
            else return Color.White; // Transparent for GridCellColor
        }
        /* end by YST */

        /*
        public static string GetPDFMergeFileFromDMASByDocKey(int? DocKey, string DocID, bool HasPO, string DownloadPath,bool ZipAllFiles)
        {
           
            string fileAttachment = "";
            using (SqlConnection cn = new SqlConnection(Database.GetDMASConnection()))
            {
                cn.Open();
                try
                {
                    List<SqlParameter> parmList = new List<SqlParameter>();
                    parmList.Add(new SqlParameter("@DocKey", DocKey));
                    parmList.Add(new SqlParameter("@HasPO", HasPO));

                    DataTable dt = GFunc.ExecuteProc(cn, "Document_GetAttachment_ByDocKey", parmList);
                    byte[] filedata;
                    string extension = "", docName = "", fileName = "", docTypeID = "", filePath = "";

                    string licenseNo = "211846748928338721114133297187668";
                    GdPicture10.GdPicturePDF finalPDF = new GdPicture10.GdPicturePDF();
                    finalPDF.SetLicenseNumber(licenseNo);

                    List<string> fNameArr = new List<string>();
                    fileAttachment = DownloadPath + DocID.Replace("/", "") + ".pdf";
                    string nonPDFfiles = "";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        filedata = (byte[])dt.Rows[i]["DocFile"];
                        fileName = GFunc.NEStr(dt.Rows[i]["FileNm"], "");
                        docTypeID = GFunc.NEStr(dt.Rows[i]["DocTypeID"], "");
                        docName = GFunc.NEStr(dt.Rows[i]["DocName"], "");
                        extension = Path.GetExtension(docName);
                        if (String.IsNullOrEmpty(extension))
                            extension = ".pdf";
                        filePath = DownloadPath + fileName + extension;

                        File.WriteAllBytes(filePath, filedata);
                        if (extension.ToLower().Equals(".pdf"))
                            fNameArr.Add(filePath);
                        else
                        {
                            nonPDFfiles += "#" + filePath;
                        }
                    }
                    dt.Dispose();

                    finalPDF.MergeDocuments(fNameArr.ToArray(), fileAttachment);
                    if (nonPDFfiles != "")
                    {
                        if(ZipAllFiles)
                        {                            
                            using (FileStream zipFile = File.Open(DownloadPath + DocID.Replace("/", "") + ".zip", FileMode.Create))
                            {
                                using (var archive = new ZipArchive(zipFile,ZipArchiveMode.Create, false))
                                {                                    
                                    archive.CreateEntryFromFile(fileAttachment, DocID.Replace("/", "") + ".pdf");
                                    string[] nPDFs= nonPDFfiles.Split('#');
                                    foreach (string otherFilePath in nPDFs)
                                        if (otherFilePath != "")
                                        {
                                            archive.CreateEntryFromFile(otherFilePath, Path.GetFileName(otherFilePath));
                                            File.Delete(otherFilePath);
                                        }
                                }
                            }
                            if (File.Exists(fileAttachment))
                                File.Delete(fileAttachment);
                            fileAttachment = DownloadPath + DocID.Replace("/", "") + ".zip";
                        }
                        else
                            fileAttachment += nonPDFfiles;
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.Show(ex.ToString());
                }
                return fileAttachment;
            }
        }*/

        private static bool GroupExists(string SecGrpID)
        {
            using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(
                Database.GetDMASConnection()))
            //   using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.DMASDocConnection))
            {
                cn.Open();
                // Using existing sql connection.
                using (System.Data.SqlClient.SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "SecGrp_Check";

                    cm.Parameters.AddWithValue("@GrpID", SecGrpID);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    cm.ExecuteNonQuery();

                    if ((int)cm.Parameters["@RetValue"].Value == 1)
                        return true;
                    else
                        return false;

                }// Already close and dispose sql connection.

            }
        }

        //Document Functions
        public static void CreateDataTransferLogicData()
        {
            List<string[]> logicDataList = new List<string[]>();


            #region Create Data transfer Logic table
            logicDataList.Add(new string[] { "Yes", "10", "DocKey", "Itm_Before", "#objDocKey", "Error", "", "", "", "" });
            logicDataList.Add(new string[] { "", "20", "DocItmKey", "Itm_Before", "AutoID", "Error", "", "", "", "" });
            logicDataList.Add(new string[] { "", "30", "LineType", "Itm_Before", "#LineType", "1000", "FieldName", "1000", "FieldName", "1000" });
            logicDataList.Add(new string[] { "", "40", "LineLinkKey", "Itm_Before", "#LineLink", "0", "0", "0", "ParentAutoID", "0" });
            logicDataList.Add(new string[] { "", "50", "ItmSN", "Itm_Before", "#CounterSN", "Error", "CounterSN", "CounterSN", "ParentSN", "CounterSN" });
            logicDataList.Add(new string[] { "", "60", "CreateDate", "Itm_Before", "#ToDay", "Error", "", "", "", "" });
            logicDataList.Add(new string[] { "", "70", "CreateUserKey", "Itm_Before", "#CurrentUser", "Error", "", "", "", "" });
            logicDataList.Add(new string[] { "", "80", "LastModifiedDate", "Itm_Before", "#ToDay", "Error", "", "", "", "" });
            logicDataList.Add(new string[] { "", "90", "LastModifiedUserKey", "Itm_Before", "#CurrentUser", "Error", "", "", "", "" });
            logicDataList.Add(new string[] { "Yes", "100", "ItmDes", "Itm_After", "#FieldName", "Error", "", "", "", "" });
            logicDataList.Add(new string[] { "", "110", "ItmDeptKey", "Itm_After", "#FieldName", "0", "", "", "", "" });
            logicDataList.Add(new string[] { "", "120", "ItmTranGrpKey", "Itm_After", "#FieldName", "0", "", "", "", "" });
            logicDataList.Add(new string[] { "", "130", "ItmAccKey", "Itm_After", "#Account", "No Change", "FieldName", "No Change", "FieldName", "No Change" });
            logicDataList.Add(new string[] { "", "140", "ItmFromAccKey", "Itm_After", "#Account", "No Change", "FieldName", "No Change", "FieldName", "No Change" });
            logicDataList.Add(new string[] { "", "150", "ItmToAccKey", "Itm_After", "#Account", "No Change", "FieldName", "No Change", "FieldName", "No Change" });
            logicDataList.Add(new string[] { "", "160", "ItmLocKey", "Itm_After", "#FieldName", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "170", "ItmFromLocKey", "Itm_After", "#FieldName", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "180", "ItmToLocKey", "Itm_After", "#FieldName", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "190", "ItmReqDate", "Itm_After", "#FieldName", "ToDay", "", "", "", "" });
            logicDataList.Add(new string[] { "", "200", "ItmPrmDate", "Itm_After", "#FieldName", "ToDay", "", "", "", "" });
            logicDataList.Add(new string[] { "", "210", "ItmHide", "Itm_After", "#FieldName", "0", "", "", "", "" });
            logicDataList.Add(new string[] { "", "220", "ItmColorKey", "Itm_After", "#FieldName", "No Change", "", "", "", "" });
            logicDataList.Add(new string[] { "", "230", "ItmScaleSize", "Itm_After", "#FieldName", "No Change", "", "", "", "" });
            logicDataList.Add(new string[] { "", "240", "ItmPacking", "Itm_After", "#FieldName", "No Change", "", "", "", "" });
            logicDataList.Add(new string[] { "", "250", "ItmRem", "Itm_After", "#FieldName", "No Change", "", "", "", "" });
            logicDataList.Add(new string[] { "", "260", "ItmMark", "Itm_After", "#FieldName", "No Change", "", "", "", "" });
            logicDataList.Add(new string[] { "", "270", "ItmReply", "Itm_After", "#FieldName", "No Change", "", "", "", "" });
            logicDataList.Add(new string[] { "", "280", "ItmJobKey", "Itm_After", "#IsJobValid", "0", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "290", "ItmJobPhaseKey", "Itm_After", "#IsJobValid", "0", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "300", "ItmJobTaskKey", "Itm_After", "#IsJobValid", "0", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "310", "ItmJobCostTypeKey", "Itm_After", "#IsJobValid", "0", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "320", "ItmIGrpDItm", "Itm_After", "#FieldName", "0", "", "", "", "" });
            logicDataList.Add(new string[] { "", "330", "ItmIGrpQtyLock", "Itm_After", "#FieldName", "0", "", "", "", "" });
            logicDataList.Add(new string[] { "", "340", "ItmIGrpToPrint", "Itm_After", "#FieldName", "1", "", "", "", "" });
            logicDataList.Add(new string[] { "", "350", "ItmIGrpQtySet", "Itm_After", "#FieldName", "0", "", "", "", "" });
            logicDataList.Add(new string[] { "", "360", "ItmIGrpAmtSet", "Itm_After", "#FieldName", "0", "", "", "", "" });
            logicDataList.Add(new string[] { "", "370", "ConfirmID", "Itm_After", "#FieldName", "No Change", "", "", "", "" });
            logicDataList.Add(new string[] { "", "380", "ConfirmSN", "Itm_After", "#FieldName", "No Change", "", "", "", "" });
            logicDataList.Add(new string[] { "", "390", "ItmAttachment", "Itm_After", "#FieldName", "0", "", "", "", "" });
            logicDataList.Add(new string[] { "", "400", "Custom1", "Itm_After", "#FieldName", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "410", "Custom2", "Itm_After", "#FieldName", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "420", "Custom3", "Itm_After", "#FieldName", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "430", "ItmDetSN", "Itm_After", "#FieldName", "0", "", "", "", "" });
            logicDataList.Add(new string[] { "Yes", "440", "ItmVendorKey", "Vend_Before", "#FieldName", "No Change", "", "", "", "" });
            logicDataList.Add(new string[] { "Yes", "450", "ItmVendorNm", "Vend_After", "#FieldName", "No Change", "", "", "", "" });
            logicDataList.Add(new string[] { "", "460", "ItmVendorPrice", "Vend_After", "#FieldName", "No Change", "", "", "", "" });
            logicDataList.Add(new string[] { "", "470", "ItmVendorPriceRatio", "Vend_After", "#FieldName", "0", "", "", "", "" });
            logicDataList.Add(new string[] { "", "480", "ItmVendorPriceLock", "Vend_After", "#FieldName", "0", "", "", "", "" });
            logicDataList.Add(new string[] { "Yes", "490", "ItmUOMKey", "UOM_Before", "#FieldName", "No Change", "", "", "", "" });
            logicDataList.Add(new string[] { "Yes", "500", "ItmQty", "Qty_Before", "#FieldName", "Error", "", "", "", "" });
            logicDataList.Add(new string[] { "Yes", "510", "ItmListPrice", "Price_Before", "#UseSysPrice", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "520", "ItmPriceAfter", "Price_Before", "#UseSysPrice", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "530", "ItmDisPercent", "Price_Before", "#FieldName", "0", "", "", "", "" });
            logicDataList.Add(new string[] { "", "540", "ItmDisValue", "Price_Before", "#FieldName", "0", "", "", "", "" });
            logicDataList.Add(new string[] { "Yes", "550", "ItmAmtShw", "Amt_Before", "#IsDiscount", "No Change", "FieldName", "FieldName", "Error", "Error" });
            logicDataList.Add(new string[] { "Yes", "560", "NSLink", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "570", "ARQOID", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "580", "ARQODK", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "590", "ARQODItm", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "600", "ARSOID", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "610", "ARSODK", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "620", "ARSODItm", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "630", "ARSOPOID", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "640", "ARDOID", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "650", "ARDODK", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "660", "ARDODItm", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "670", "ARIVID", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "680", "ARIVDK", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "690", "ARIVDItm", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "700", "APPOID", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "710", "APPODK", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "720", "APPODItm", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "730", "APPDID", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "740", "APPDDK", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "750", "APPDDItm", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "760", "CSCPOID", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "770", "CSCPODK", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "780", "CSCPODItm", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "790", "CSCPSID", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "800", "CSCPSDK", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "810", "CSCPSDItm", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "820", "CSCSIID", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "830", "CSCSIDK", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "", "840", "CSCSIDItm", "Amt_After", "#UseNSLink", "No Change", "FieldName", "FieldName", "FieldName", "FieldName" });
            logicDataList.Add(new string[] { "Yes", "850", "ItmKey", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "860", "ItmKeySelect", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "870", "ItmType", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "880", "ItmStock", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "890", "ItmQtyLink", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "900", "ItmQtyAdj", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "910", "ItmOrderStatus", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "920", "ItmConRate", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "930", "ItmLatestCostF", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "940", "ItmLatestCostH", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "950", "ItmPriceBefore", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "960", "ItmPrice", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "970", "ItmPriceUser", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "980", "ItmControlPrice", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "990", "ItmControlPriceBase", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1000", "ItmAmtF", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1010", "ItmAmtH", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1020", "ItmTaxable", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1030", "ItmTaxGrpKey", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1040", "ItmTaxGrpRate", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1050", "ItmTaxGrpAmtF", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1060", "ItmTaxGrpAmtL", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1070", "ItmAddCostF", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1080", "ItmAddCostH", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1090", "ItmAddAmtF", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1100", "ItmAddAmtH", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1110", "ItmVendorCurrKey", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1120", "ItmVendorCurrRate", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1130", "ItmBatchKey", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1140", "ItmBatchQty", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1150", "BatchID", "None", "", "", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1160", "ItmCost", "None", "#FieldName", "No Change", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1170", "ItmNewCost", "None", "#FieldName", "No Change", "", "", "", "" });

			//added by thettm on 27 jun 2018(start)
            logicDataList.Add(new string[] { "Yes", "1180", "SerialNos", "None", "#FieldName", "Error", "", "", "", "" });
            //added by thettm on 27 jun 2018(end)

            logicDataList.Add(new string[] { "", "1190", "DSQty", "None", "#FieldName", "No Change", "", "", "", "" });

            logicDataList.Add(new string[] { "", "1200", "ItmRef", "Itm_After", "#FieldName", "No Change", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1210", "ItmWrtyEndDate", "Itm_After", "#FieldName", "No Change", "", "", "", "" });

            logicDataList.Add(new string[] { "", "1220", "HSCode", "Itm_After", "#FieldName", "No Change", "", "", "", "" });
            logicDataList.Add(new string[] { "", "1230", "CountryID", "Itm_After", "#FieldName", "No Change", "", "", "", "" });

            GlobalUI.dtTransferLogicData = new DataTable();
            GlobalUI.dtTransferLogicData.Columns.Add("ExecProc", typeof(string));
            GlobalUI.dtTransferLogicData.Columns.Add("SN", typeof(int));
            GlobalUI.dtTransferLogicData.Columns.Add("FldNm", typeof(string));
            GlobalUI.dtTransferLogicData.Columns.Add("Action", typeof(string));
            GlobalUI.dtTransferLogicData.Columns.Add("FuncNm", typeof(string));
            GlobalUI.dtTransferLogicData.Columns.Add("DefValue", typeof(string));
            GlobalUI.dtTransferLogicData.Columns.Add("GrpParentMatch", typeof(string));
            GlobalUI.dtTransferLogicData.Columns.Add("GrpParentUnMatch", typeof(string));
            GlobalUI.dtTransferLogicData.Columns.Add("GrpChildMatch", typeof(string));
            GlobalUI.dtTransferLogicData.Columns.Add("GrpChildUnMatch", typeof(string));

            foreach (string[] strData in logicDataList)
            {
                DataRow drNew = GlobalUI.dtTransferLogicData.NewRow();
                foreach (DataColumn dc in GlobalUI.dtTransferLogicData.Columns)
                {
                    drNew[dc] = strData[dc.Ordinal];
                }
                GlobalUI.dtTransferLogicData.Rows.Add(drNew);
            }
            #endregion
        }//Completed
        public static bool DocTransferData(int sourceDC, int sourceDK, int sourceConKey, DataTable sourceDT, Document tgtObjDoc, UltraGrid tgtgrd, UltraGrid tdtgrdExp, int InsertAction, string sourceSort, bool useSysPrice, bool useNSLink)
        {
            try
            {
                //Reference : C-Boss\Development Tool\Document DataTransfer.xslx

                #region Declaration
                DataTable dtTransferLogicData = GlobalUI.dtTransferLogicData;
                DataTable tgtDT = tgtgrd.DataSource as DataTable;
                int tgtDC = (int)tgtObjDoc.DocCodeKey;
                int tgtDK = (int)tgtObjDoc.DocKey;
                string[] Actions = { "None", "Itm_Before", "Itm_After", "Vend_Before", "Vend_After", "UOM_Before", "Qty_Before", "Price_Before", "Amt_Before", "Amt_After" };
                UltraGridRow grow = null;
                Hashtable htDetailGrd = new Hashtable();

                GlobalUI.bRuningImport = true;
                DocUtility.bRuningImport = true;

                int ParentAutoKey = 0;
                decimal CounterSN = 0;
                decimal ParentSN = 0;
                string FldNm = string.Empty;
                string FuncNm = string.Empty;
                string DefValue = string.Empty;
                int LineType = 0;

                bool canFireItm = false;
                bool canFireVend = false;
                bool canFireUOM = false;
                bool canFireQty = false;
                bool canFirePrice = false;
                bool canFireAmt = false;
                #endregion

                #region Get the initial current SN and set active row as new row in target grid
                if (tgtgrd.ActiveRow == null)
                {
                    CounterSN = tgtgrd.Rows.GetFilteredInNonGroupByRows().Count();
                    if (CounterSN == 0)
                    {

                        tgtgrd.DisplayLayout.Bands[0].AddNew();
                    }
                    tgtgrd.ActiveRow = tgtgrd.Rows[tgtgrd.Rows.Count - 1];
                }
                else
                {
                    if (tgtgrd.ActiveRow.IsUnmodifiedTemplateAddRow)
                        CounterSN = tgtgrd.Rows.GetFilteredInNonGroupByRows().Count();//total filtered rows count
                    else
                    {
                        switch (tgtDC)
                        {
                            case (int)GEnum.SystemCode.Payment_Issue:
                            case (int)GEnum.SystemCode.Payment_Received:
                            case (int)GEnum.SystemCode.Cash_Payment_Received:
                                CounterSN = (decimal)tgtgrd.ActiveRow.Cells["ExpSN"].Value - 1;
                                break;

                            default:
                                CounterSN = (decimal)tgtgrd.ActiveRow.Cells["ItmSN"].Value - 1;
                               // CounterSN = (decimal)tgtgrd.ActiveRow.Cells["ItmSN"].Value + 1; //Not ready
                                break;
                        }
                        tgtgrd.UpdateData();
                        tgtgrd.DisplayLayout.Bands[0].AddNew();
                        tgtgrd.ActiveRow = tgtgrd.Rows[tgtgrd.Rows.Count - 1];
                    }
                }
                grow = tgtgrd.ActiveRow;
                #endregion

                #region Loop source data to insert into target grid
                string sortorder = sourceDT.DefaultView.Sort.ToString();

                if (sourceSort != string.Empty)
                    sortorder = sourceSort;

                DataRow[] drs = sourceDT.Select("", sortorder);
                foreach (DataRow sourceDR in drs)
                {

                    #region reset flags
                    canFireItm = true;//now always set flag to fire, for furture use
                    canFireVend = true;//now always set flag to fire, for furture use
                    canFireUOM = true;//now always set flag to fire, for furture use
                    canFireQty = true;//now always set flag to fire, for furture use
                    canFirePrice = true;//now always set flag to fire, for furture use
                    canFireAmt = true;//now always set flag to fire, for furture use
                    #endregion
                    
                    #region Perform removal of invalid parent or child item from the source
                    LineType = DocTransferData_ValidSourceRow(tgtDC, sourceDR);
                    if (LineType == 0)
                        continue;   //Do not insert the row
                    else
                    {
                        if (tgtgrd.ActiveRow == null)
                        {
                            tgtgrd.DisplayLayout.Bands[0].AddNew();
                            tgtgrd.ActiveRow = tgtgrd.Rows[tgtgrd.Rows.Count - 1];
                            grow = tgtgrd.ActiveRow;
                        }
                        else
                        {
                            if (tgtgrd.ActiveRow.IsAddRow == false)
                            {
                                tgtgrd.DisplayLayout.Bands[0].AddNew();
                                tgtgrd.ActiveRow = tgtgrd.Rows[tgtgrd.Rows.Count - 1];
                            }
                        }
                        grow = tgtgrd.ActiveRow;
                    }
                    #endregion

                    #region Loop Logic action for each Column. NOTE: the sequence in which column is updated is critical in getting the correct field value/data transferred
                    foreach (string action in Actions)
                    {                      
                        #region Loop Logic Action Detail
                        DataRow[] LogicData = dtTransferLogicData.Select("Action = '" + action + "'", "SN ASC");
                        foreach (DataRow dr in LogicData)
                        {
                            //if (GFunc.NEInt(dr["SN"], 0) == 1160)
                            //    continue;
                            FuncNm = dr["FuncNm"].ToString();
                            FldNm = dr["FldNm"].ToString();
                            DefValue = dr["DefValue"].ToString();                                                    

                            //Set Child column values where no calculation is required
                            if (action == "None")
                            {
                                if (LineType != (int)GEnum.RecDetailType.DItems)
                                {
                                    if (tgtDT.Columns.Contains(FldNm))
                                    {
                                        if (sourceDT.Columns.Contains(FldNm))
                                        {
                                            grow.Cells[FldNm].Value = sourceDR[FldNm];
                                            if (FldNm == "ItmKey")
                                                grow.Cells["ItmID"].Value = sourceDR["ItmID"];  //this code is required because the dtTransferLogicData  did not include the ItmID column
                                        }
                                        else
                                            DocTransferData_UnMatchColumn(sourceDC, tgtDC, FldNm, DefValue, grow);
                                    }
                                }
                            }

                            #region Run Update Process
                            if (GFunc.NEStr(dr["execProc"].ToString(), string.Empty) == "Yes")
                            {
                                switch (action)
                                {
                                    case "Itm_After":
                                        if (canFireItm && LineType == 1000)
                                        {
                                            if (tgtDT.Columns.Contains("ItmKeySelect") && sourceDT.Columns.Contains("ItmKeySelect"))
                                            {
                                                htDetailGrd = new Hashtable();
                                                htDetailGrd.Add(GEnum.Details.Doc_Itm, tgtgrd);
                                                htDetailGrd.Add(GEnum.Details.Doc_Exp, tdtgrdExp);

                                                //Handle Blank Row                                                
                                                if (GFunc.NEInt(sourceDR["ItmKeySelect"], 0) == 0) //used GFun, Modified by Pauk 14 Jun 2011
                                                {
                                                    int key = SysOptionUtility.GetInt(GVar.SystemOption.Document_Defaults.DefaultItmRemark);
                                                    if (DocDetUtil.ItmID_Update(tgtObjDoc, htDetailGrd, key) == false)
                                                        throw new TAException("Invalid Operation occur: " + action.ToString());
                                                }
                                                else
                                                {
                                                    if (DocDetUtil.ItmID_Update(tgtObjDoc, htDetailGrd, GFunc.NEInt(sourceDR["ItmKeySelect"], 0)) == false)
                                                        throw new TAException("Invalid Operation occur: " + action.ToString());
                                                }
                                            }
                                        }
                                        break;

                                    case "Vend_After":
                                        if (canFireVend && LineType == 1000)
                                        {
                                            if (tgtDT.Columns.Contains("ItmVendorKey") && sourceDT.Columns.Contains("ItmVendorKey"))
                                            {
                                                switch (GFunc.GetINTypeGroup(grow.Cells["ItmType"].Value))
                                                {
                                                    case (int)GEnum.INTypeGrp.Charges:
                                                    case (int)GEnum.INTypeGrp.Non_Stock:
                                                    case (int)GEnum.INTypeGrp.Stock:
                                                        if (DocDetUtil.ItmVendorID_Update(tgtObjDoc, tgtgrd, GFunc.NEInt(sourceDR["ItmVendorKey"], 0), "", "") == false)
                                                            throw new TAException("Invalid Operation occur: " + action.ToString());
                                                        break;
                                                }
                                            }
                                        }
                                        break;

                                    case "Qty_Before":
                                        if (canFireUOM && LineType == 1000)
                                        {
                                            if (tgtDT.Columns.Contains("ItmUOMKey") && sourceDT.Columns.Contains("ItmUOMKey"))
                                            {
                                                switch (GFunc.GetINTypeGroup(grow.Cells["ItmType"].Value))
                                                {
                                                    case (int)GEnum.INTypeGrp.Non_Stock:
                                                    case (int)GEnum.INTypeGrp.Stock:                                                  
                                                        if (DocDetUtil.ItmUOMKey_CustomUpdate(tgtObjDoc, tgtgrd,(int?)sourceDC) == false)
                                                            throw new TAException("Invalid Operation occur: " + action.ToString());
                                                        break;
                                                }
                                            }
                                        }
                                        break;

                                    case "Price_Before":
                                        if (canFireQty && LineType == 1000)
                                        {
                                            if (tgtDT.Columns.Contains("ItmQty") && sourceDT.Columns.Contains("ItmQty"))
                                            {
                                                switch (GFunc.GetINTypeGroup(grow.Cells["ItmType"].Value))
                                                {
                                                    case (int)GEnum.INTypeGrp.Discount:
                                                    case (int)GEnum.INTypeGrp.Charges:
                                                    case (int)GEnum.INTypeGrp.Non_Stock:
                                                    case (int)GEnum.INTypeGrp.Stock:
                                                        if (DocDetUtil.ItmQty_CustomUpdate(tgtObjDoc, tgtgrd) == false)
                                                            throw new TAException("Invalid Operation occur: " + action.ToString());
                                                        break;
                                                }
                                            }
                                        }
                                        break;

                                    case "Amt_Before":
                                        if (canFirePrice && LineType == 1000)
                                        {
                                            if (tgtDT.Columns.Contains("ItmPriceAfter") && sourceDT.Columns.Contains("ItmPriceAfter"))
                                            {
                                                switch (GFunc.GetINTypeGroup(grow.Cells["ItmType"].Value))
                                                {
                                                    case (int)GEnum.INTypeGrp.Charges:
                                                    case (int)GEnum.INTypeGrp.Non_Stock:
                                                    case (int)GEnum.INTypeGrp.Stock:
                                                        if (DocDetUtil.ItmPriceAfterTransfer_CustomUpdate(tgtObjDoc, tgtgrd) == false)
                                                            throw new TAException("Invalid Operation occur: " + action.ToString());                                                       
                                                        break;
                                                }
                                            }
                                        }
                                        break;

                                    case "Amt_After":
                                        if (canFireAmt && LineType == 1000)
                                        {
                                            if (tgtDT.Columns.Contains("ItmAmtShw") && sourceDT.Columns.Contains("ItmAmtShw"))
                                            {
                                                switch (GFunc.GetINTypeGroup(grow.Cells["ItmType"].Value))
                                                {
                                                    case (int)GEnum.INTypeGrp.Discount:
                                                        if (DocDetUtil.ItmAmtShwTransfer_CustomeUpdate(tgtObjDoc, tgtgrd) == false)
                                                            throw new TAException("Invalid Operation occur: " + action.ToString());                                                      
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                            #endregion

                            switch (FuncNm)
                            {
                                #region objDocKey
                                case "#objDocKey":
                                    if (tgtDT.Columns.Contains(FldNm))
                                    {
                                        if (tgtgrd.ActiveRow != null)
                                        {
                                            grow.Cells[FldNm].Value = tgtDK;
                                        }
                                    }
                                    else
                                        DocTransferData_UnMatchColumn(sourceDC, tgtDC, FldNm, DefValue, grow);
                                    break;
                                #endregion

                                #region CounterSN
                                case "#CounterSN":
                                    if (tgtDT.Columns.Contains(FldNm))
                                    {
                                        if (LineType == (int)GEnum.RecDetailType.DItems)
                                        {
                                            if (IsParentItmType((int)sourceDR["ItmType"]))
                                            {
                                                ParentSN = CounterSN + 1;
                                            }

                                            
                                            if (InsertAction == (int)GEnum.InsertAction.InsertPO ||InsertAction == (int)GEnum.InsertAction.InsertCO 
                                                || InsertAction == (int)GEnum.InsertAction.InsertCS || InsertAction == (int)GEnum.InsertAction.InsertPD
                                                || InsertAction == (int)GEnum.InsertAction.InsertCSR || InsertAction == (int)GEnum.InsertAction.InsertSO
                                                || InsertAction == (int)GEnum.InsertAction.InsertData)
                                            {
                                                grow.Cells[FldNm].Value = CounterSN + 0.00000001M;
                                                CounterSN = CounterSN + 0.00000001M;
                                               
                                            }
                                            else
                                            {
                                                grow.Cells[FldNm].Value = CounterSN + 0.5M;
                                                CounterSN = CounterSN + 1;
                                            }

                                            
                                        }
                                        else
                                        {
                                            grow.Cells[FldNm].Value = ParentSN;
                                        }
                                    }
                                    else
                                        DocTransferData_UnMatchColumn(sourceDC, tgtDC, FldNm, DefValue, grow);
                                    break;
                                #endregion

                                #region FieldName
                                case "#FieldName":
                                    if (tgtDT.Columns.Contains(FldNm) && sourceDT.Columns.Contains(FldNm))
                                    {                                        
                                        if (sourceDT.Columns.Contains(FldNm))
                                            grow.Cells[FldNm].Value = sourceDR[FldNm];
                                        else
                                            DocTransferData_UnMatchColumn(sourceDC, tgtDC, FldNm, DefValue, grow);
                                    }
                                    break;
                                #endregion

                                #region LineType
                                case "#LineType":
                                    if (tgtgrd.ActiveRow != null)
                                    {
                                        if (tgtDT.Columns.Contains(FldNm))
                                        {
                                            grow.Cells[FldNm].Value = LineType;
                                        }
                                    }
                                    break;
                                #endregion

                                #region LineLink
                                case "#LineLink":
                                    if (tgtDT.Columns.Contains(FldNm) && sourceDT.Columns.Contains(FldNm))
                                    {
                                        if (LineType == (int)GEnum.RecDetailType.DItems)
                                        {
                                            if (IsParentItmType((int)sourceDR["ItmType"]))
                                            {
                                                ParentAutoKey = (int)grow.Cells["DocItmKey"].Value;
                                            }
                                            grow.Cells[FldNm].Value = 0;
                                        }
                                        else
                                        {
                                            grow.Cells[FldNm].Value = ParentAutoKey;
                                        }
                                    }
                                    break;
                                #endregion

                                #region Account
                                case "#Account":
                                    if (tgtDT.Columns.Contains(FldNm))
                                    {
                                        if (sourceDT.Columns.Contains(FldNm))
                                        {
                                            if (LineType == (int)GEnum.RecDetailType.DItems ||
                                                ((GEnum.ItemType)sourceDR["ItmType"] == GEnum.ItemType.Stock
                                                || (GEnum.ItemType)sourceDR["ItmType"] == GEnum.ItemType.Finished_GDB
                                                || (GEnum.ItemType)sourceDR["ItmType"] == GEnum.ItemType.StockB
                                                || (GEnum.ItemType)sourceDR["ItmType"] == GEnum.ItemType.Serial_StockB
                                                || (GEnum.ItemType)sourceDR["ItmType"] == GEnum.ItemType.Finished_GDB
                                                || (GEnum.ItemType)sourceDR["ItmType"] == GEnum.ItemType.Serial_Finished_GDB))
                                            {
                                                if (DocTransferData_DCGrpMatch(sourceDC, tgtDC))
                                                {
                                                    grow.Cells[FldNm].Value = sourceDR[FldNm];
                                                }
                                            }
                                            else
                                            {
                                                grow.Cells[FldNm].Value = 0;//Child Item is only available in AR Doc so it could be sales Account and there are no posting for Child in AR DO so we will set it to 0
                                            }
                                        }
                                        else
                                        {
                                            DocTransferData_UnMatchColumn(sourceDC, tgtDC, FldNm, DefValue, grow);
                                        }
                                    }
                                    break;
                                #endregion

                                #region UseSysPrice
                                case "#UseSysPrice":
                                    if (tgtDT.Columns.Contains(FldNm))
                                    {
                                        if (sourceDT.Columns.Contains(FldNm))
                                        {
                                            if (LineType == (int)GEnum.RecDetailType.DItems)
                                            {
                                                if (useSysPrice == false)
                                                    grow.Cells[FldNm].Value = sourceDR[FldNm];
                                            }
                                            else
                                            {
                                                grow.Cells[FldNm].Value = sourceDR[FldNm];
                                            }
                                        }
                                        else
                                            DocTransferData_UnMatchColumn(sourceDC, tgtDC, FldNm, DefValue, grow);
                                    }
                                    break;
                                #endregion

                                #region IsDiscount
                                case "#IsDiscount":
                                    if (tgtDT.Columns.Contains(FldNm))
                                    {
                                        if (sourceDT.Columns.Contains(FldNm))
                                        {
                                            if (LineType == (int)GEnum.RecDetailType.DItems)
                                            {
                                                if ((int)grow.Cells["ItmType"].Value == (int)GEnum.ItemType.Discount)
                                                {
                                                    grow.Cells[FldNm].Value = sourceDR[FldNm];

                                                    //The below code is replace to force ItmAmtShw to take the source value regardless ItmQty value
                                                    if (GFunc.NEDec(grow.Cells["ItmQty"].Value, 0) != 0)
                                                    {
                                                        grow.Cells[FldNm].Value = sourceDR[FldNm];
                                                        canFireAmt = true;
                                                    }
                                                    else
                                                    {
                                                        canFireAmt = false;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                grow.Cells[FldNm].Value = sourceDR[FldNm];
                                            }
                                        }
                                        else
                                            DocTransferData_UnMatchColumn(sourceDC, tgtDC, FldNm, DefValue, grow);
                                    }
                                    break;
                                #endregion

                                #region UseNSLink
                                case "#UseNSLink":
                                    if (tgtDT.Columns.Contains(FldNm))
                                    {
                                        if (sourceDT.Columns.Contains(FldNm))
                                        {
                                            if (InsertAction > 0)
                                            {
                                                grow.Cells[FldNm].Value = sourceDR[FldNm];
                                            }
                                            else
                                            {
                                                if (useNSLink)
                                                {
                                                    if (GFunc.NEStr(sourceDR[FldNm], "").ToLower().EndsWith("id") && GFunc.NEStr(sourceDR[FldNm], "").ToLower() == "nslink")
                                                        grow.Cells[FldNm].Value = sourceDR[FldNm];

                                                    switch ((GEnum.SystemCode)sourceDC)
                                                    {
                                                        case GEnum.SystemCode.Quotation:
                                                        case GEnum.SystemCode.Sales_Order:
                                                        case GEnum.SystemCode.Delivery_Order:
                                                        case GEnum.SystemCode.Sales_Invoice:
                                                        case GEnum.SystemCode.Cash_Sale:
                                                            if (tgtDC == (int)GEnum.SystemCode.Purchase_Order)
                                                            {
                                                                grow.Cells[FldNm].Value = sourceDR[FldNm];
                                                            }
                                                            break;
                                                    }
                                                }
                                            }

                                        }
                                        else
                                            DocTransferData_UnMatchColumn(sourceDC, tgtDC, FldNm, DefValue, grow);
                                    }
                                    break;
                                #endregion

                                #region ToDay
                                case "#ToDay":
                                    if (tgtDT.Columns.Contains(FldNm))
                                    {
                                        grow.Cells[FldNm].Value = DateTime.Today;
                                    }
                                    break;
                                #endregion

                                #region CurrentUser
                                case "#CurrentUser":
                                    if (tgtDT.Columns.Contains(FldNm))
                                    {
                                        grow.Cells[FldNm].Value = AppInfor.CurrentUserKey;
                                    }
                                    break;
                                #endregion

                                #region IsJobValid
                                case "#IsJobValid":
                                    if (tgtDT.Columns.Contains(FldNm) && sourceDT.Columns.Contains(FldNm))
                                    {
                                        switch (sourceDC)
                                        {
                                            case (int)GEnum.SystemCode.Quotation:
                                            case (int)GEnum.SystemCode.Sales_Order:
                                            case (int)GEnum.SystemCode.Reserve_Order:
                                            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                                            case (int)GEnum.SystemCode.Delivery_Order:
                                            case (int)GEnum.SystemCode.Packing_List:
                                            case (int)GEnum.SystemCode.Sales_Invoice:
                                            case (int)GEnum.SystemCode.DO_to_IV_Transfer:
                                            case (int)GEnum.SystemCode.Sales_Debit_Note:
                                            case (int)GEnum.SystemCode.Sales_Credit_Note:
                                            case (int)GEnum.SystemCode.Sales_Adjustment:
                                            case (int)GEnum.SystemCode.Payment_Received:
                                            case (int)GEnum.SystemCode.Cash_Sale:
                                            case (int)GEnum.SystemCode.Cash_Debit_Note:
                                            case (int)GEnum.SystemCode.Cash_Credit_Note:
                                            case (int)GEnum.SystemCode.Cash_Adjustment:
                                            case (int)GEnum.SystemCode.Cash_Payment_Received:
                                                if (LineType == (int)GEnum.RecDetailType.DItems)
                                                {
                                                    if (sourceConKey != 0 && sourceConKey == GFunc.GetIntPropertyValue("DocConKey", tgtObjDoc))
                                                        grow.Cells[FldNm].Value = sourceDR[FldNm];
                                                }
                                                else
                                                {
                                                    if (sourceConKey != 0 && sourceConKey == GFunc.GetIntPropertyValue("DocConKey", tgtObjDoc))
                                                        grow.Cells[FldNm].Value = sourceDR[FldNm];
                                                    else
                                                        grow.Cells[FldNm].Value = 0;
                                                }
                                                break;

                                            case (int)GEnum.SystemCode.Inventory_Adjustment: // added by YST to import JobID from Excel
                                                {
                                                    grow.Cells[FldNm].Value = sourceDR[FldNm];
                                                }
                                                break;

                                            case (int)GEnum.SystemCode.Job:
                                                if (tgtDC == (int)GEnum.SystemCode.Purchase_Order)
                                                {
                                                    if (GFunc.GetINTypeGroup(GFunc.NEInt(sourceDR["ItmType"], 0)) == (int)GEnum.INTypeGrp.Stock)
                                                        grow.Cells[FldNm].Value = 0;
                                                    else
                                                    {
                                                        grow.Cells[FldNm].Value = sourceDR[FldNm];
                                                        if (FldNm == "ItmJobKey")// To create a reference so that update to Mst_Job
                                                        {
                                                            sourceDR["JobPODK"] = tgtDK;
                                                            sourceDR["JobPODItm"] = grow.Cells["DocItmKey"].Value;
                                                        }
                                                    }
                                                }
                                                break;

                                            case (int)GEnum.SystemCode.Purchase_Requisition:
                                                if (tgtDC == (int)GEnum.SystemCode.Purchase_Order)
                                                {
                                                    //do nothing
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                #endregion
                            }
                        }
                        #endregion
                    }
                    #endregion

                    tgtgrd.ActiveRow.Update();
                }
                #endregion
                if (tgtgrd.ActiveRow == null)
                    tgtgrd.ActiveRow = tgtgrd.Rows[tgtgrd.Rows.Count - 1];
                //Set the cursor to the newly inserted row or rows+1
                if (tgtgrd.ActiveRow.Index + 1 == tgtgrd.Rows.Count)
                {
                    tgtgrd.Rows.TemplateAddRow.Activate();
                    //tgtgrd.ActiveRow.Cells["ItmID"].Activate();
                    //tgtgrd.PerformAction(UltraGridAction.EnterEditMode, false, false);
                }
                else
                {
                    tgtgrd.ActiveRow = tgtgrd.Rows[tgtgrd.ActiveRow.Index + 1];
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            finally
            {
                GlobalUI.bRuningImport = false;
                DocUtility.bRuningImport = false;
            }
        }//Completed

        public static bool DocTransferData(SqlConnection cn,int sourceDC, int sourceDK, int sourceConKey, DataTable sourceDT, Document tgtObjDoc, UltraGrid tgtgrd, UltraGrid tdtgrdExp, int InsertAction, string sourceSort, bool useSysPrice, bool useNSLink)
        {
            try
            {
                //Reference : C-Boss\Development Tool\Document DataTransfer.xslx

                #region Declaration
                DataTable dtTransferLogicData = GlobalUI.dtTransferLogicData;
                DataTable tgtDT = tgtgrd.DataSource as DataTable;
                int tgtDC = (int)tgtObjDoc.DocCodeKey;
                int tgtDK = (int)tgtObjDoc.DocKey;
                string[] Actions = { "None", "Itm_Before", "Itm_After", "Vend_Before", "Vend_After", "UOM_Before", "Qty_Before", "Price_Before", "Amt_Before", "Amt_After" };
                UltraGridRow grow = null;
                Hashtable htDetailGrd = new Hashtable();

                GlobalUI.bRuningImport = true;
                DocUtility.bRuningImport = true;

                int ParentAutoKey = 0;
                decimal CounterSN = 0;
                decimal ParentSN = 0;
                string FldNm = string.Empty;
                string FuncNm = string.Empty;
                string DefValue = string.Empty;
                int LineType = 0;

                bool canFireItm = false;
                bool canFireVend = false;
                bool canFireUOM = false;
                bool canFireQty = false;
                bool canFirePrice = false;
                bool canFireAmt = false;
                #endregion

                ((TAGridEditor)tgtgrd).ActiveConnection = cn;
                #region Get the initial current SN and set active row as new row in target grid
                if (tgtgrd.ActiveRow == null)
                {
                    CounterSN = tgtgrd.Rows.GetFilteredInNonGroupByRows().Count();
                    if (CounterSN == 0)
                    {

                        tgtgrd.DisplayLayout.Bands[0].AddNew();
                    }
                    tgtgrd.ActiveRow = tgtgrd.Rows[tgtgrd.Rows.Count - 1];
                }
                else
                {
                    if (tgtgrd.ActiveRow.IsUnmodifiedTemplateAddRow)
                        CounterSN = tgtgrd.Rows.GetFilteredInNonGroupByRows().Count();//total filtered rows count
                    else
                    {
                        switch (tgtDC)
                        {
                            case (int)GEnum.SystemCode.Payment_Issue:
                            case (int)GEnum.SystemCode.Payment_Received:
                            case (int)GEnum.SystemCode.Cash_Payment_Received:
                                CounterSN = (decimal)tgtgrd.ActiveRow.Cells["ExpSN"].Value - 1;
                                break;

                            default:
                                CounterSN = (decimal)tgtgrd.ActiveRow.Cells["ItmSN"].Value - 1;
                                // CounterSN = (decimal)tgtgrd.ActiveRow.Cells["ItmSN"].Value + 1; //Not ready
                                break;
                        }
                        tgtgrd.UpdateData();
                        tgtgrd.DisplayLayout.Bands[0].AddNew();
                        tgtgrd.ActiveRow = tgtgrd.Rows[tgtgrd.Rows.Count - 1];
                    }
                }
                grow = tgtgrd.ActiveRow;
                #endregion

                #region Loop source data to insert into target grid
                string sortorder = sourceDT.DefaultView.Sort.ToString();

                if (sourceSort != string.Empty)
                    sortorder = sourceSort;

                DataRow[] drs = sourceDT.Select("", sortorder);
                foreach (DataRow sourceDR in drs)
                {

                    #region reset flags
                    canFireItm = true;//now always set flag to fire, for furture use
                    canFireVend = true;//now always set flag to fire, for furture use
                    canFireUOM = true;//now always set flag to fire, for furture use
                    canFireQty = true;//now always set flag to fire, for furture use
                    canFirePrice = true;//now always set flag to fire, for furture use
                    canFireAmt = true;//now always set flag to fire, for furture use
                    #endregion

                    #region Perform removal of invalid parent or child item from the source
                    LineType = DocTransferData_ValidSourceRow(tgtDC, sourceDR);
                    if (LineType == 0)
                        continue;   //Do not insert the row
                    else
                    {
                        if (tgtgrd.ActiveRow == null)
                        {
                            tgtgrd.DisplayLayout.Bands[0].AddNew();
                            tgtgrd.ActiveRow = tgtgrd.Rows[tgtgrd.Rows.Count - 1];
                            grow = tgtgrd.ActiveRow;
                        }
                        else
                        {
                            if (tgtgrd.ActiveRow.IsAddRow == false)
                            {
                                tgtgrd.DisplayLayout.Bands[0].AddNew();
                                tgtgrd.ActiveRow = tgtgrd.Rows[tgtgrd.Rows.Count - 1];
                            }
                        }
                        grow = tgtgrd.ActiveRow;
                    }
                    #endregion

                    #region Loop Logic action for each Column. NOTE: the sequence in which column is updated is critical in getting the correct field value/data transferred
                    foreach (string action in Actions)
                    {
                        #region Loop Logic Action Detail
                        DataRow[] LogicData = dtTransferLogicData.Select("Action = '" + action + "'", "SN ASC");
                        foreach (DataRow dr in LogicData)
                        {
                            FuncNm = dr["FuncNm"].ToString();
                            FldNm = dr["FldNm"].ToString();
                            DefValue = dr["DefValue"].ToString();
                            if (FldNm.ToLower() == "itmamtshw")
                            {
                            }
                            //Set Child column values where no calculation is required
                            if (action == "None")
                            {
                                if (LineType != (int)GEnum.RecDetailType.DItems)
                                {
                                    if (tgtDT.Columns.Contains(FldNm))
                                    {
                                        if (sourceDT.Columns.Contains(FldNm))
                                        {
                                            grow.Cells[FldNm].Value = sourceDR[FldNm];
                                            if (FldNm == "ItmKey")
                                                grow.Cells["ItmID"].Value = sourceDR["ItmID"];  //this code is required because the dtTransferLogicData  did not include the ItmID column
                                        }
                                        else
                                            DocTransferData_UnMatchColumn(sourceDC, tgtDC, FldNm, DefValue, grow);
                                    }
                                }
                            }

                            #region Run Update Process
                            if (GFunc.NEStr(dr["execProc"].ToString(), string.Empty) == "Yes")
                            {
                                switch (action)
                                {
                                    case "Itm_After":
                                        if (canFireItm && LineType == 1000)
                                        {
                                            if (tgtDT.Columns.Contains("ItmKeySelect") && sourceDT.Columns.Contains("ItmKeySelect"))
                                            {
                                                htDetailGrd = new Hashtable();
                                                htDetailGrd.Add(GEnum.Details.Doc_Itm, tgtgrd);
                                                htDetailGrd.Add(GEnum.Details.Doc_Exp, tdtgrdExp);

                                                //Handle Blank Row                                                
                                                if (GFunc.NEInt(sourceDR["ItmKeySelect"], 0) == 0) //used GFun, Modified by Pauk 14 Jun 2011
                                                {
                                                    int key = SysOptionUtility.GetInt(GVar.SystemOption.Document_Defaults.DefaultItmRemark,cn);
                                                    if (DocDetUtil.ItmID_Update(cn,tgtObjDoc, htDetailGrd, key) == false)
                                                        throw new TAException("Invalid Operation occur: " + action.ToString());
                                                }
                                                else
                                                {
                                                    if (DocDetUtil.ItmID_Update(cn,tgtObjDoc, htDetailGrd, GFunc.NEInt(sourceDR["ItmKeySelect"], 0)) == false)
                                                        throw new TAException("Invalid Operation occur: " + action.ToString());
                                                }
                                            }
                                        }
                                        break;

                                    case "Vend_After":
                                        if (canFireVend && LineType == 1000)
                                        {
                                            if (tgtDT.Columns.Contains("ItmVendorKey") && sourceDT.Columns.Contains("ItmVendorKey"))
                                            {
                                                switch (GFunc.GetINTypeGroup(grow.Cells["ItmType"].Value))
                                                {
                                                    case (int)GEnum.INTypeGrp.Charges:
                                                    case (int)GEnum.INTypeGrp.Non_Stock:
                                                    case (int)GEnum.INTypeGrp.Stock:
                                                        if (DocDetUtil.ItmVendorID_Update(cn,tgtObjDoc, tgtgrd, GFunc.NEInt(sourceDR["ItmVendorKey"], 0), "", "") == false)
                                                            throw new TAException("Invalid Operation occur: " + action.ToString());
                                                        break;
                                                }
                                            }
                                        }
                                        break;

                                    case "Qty_Before":
                                        if (canFireUOM && LineType == 1000)
                                        {
                                            if (tgtDT.Columns.Contains("ItmUOMKey") && sourceDT.Columns.Contains("ItmUOMKey"))
                                            {
                                                switch (GFunc.GetINTypeGroup(grow.Cells["ItmType"].Value))
                                                {
                                                    case (int)GEnum.INTypeGrp.Non_Stock:
                                                    case (int)GEnum.INTypeGrp.Stock:
                                                        if (DocDetUtil.ItmUOMKey_CustomUpdate(cn,tgtObjDoc, tgtgrd) == false)
                                                            throw new TAException("Invalid Operation occur: " + action.ToString());
                                                        break;
                                                }
                                            }
                                        }
                                        break;

                                    case "Price_Before":
                                        if (canFireQty && LineType == 1000)
                                        {
                                            if (tgtDT.Columns.Contains("ItmQty") && sourceDT.Columns.Contains("ItmQty"))
                                            {
                                                switch (GFunc.GetINTypeGroup(grow.Cells["ItmType"].Value))
                                                {
                                                    case (int)GEnum.INTypeGrp.Discount:
                                                    case (int)GEnum.INTypeGrp.Charges:
                                                    case (int)GEnum.INTypeGrp.Non_Stock:
                                                    case (int)GEnum.INTypeGrp.Stock:
                                                        if (DocDetUtil.ItmQty_CustomUpdate(cn,tgtObjDoc, tgtgrd) == false)
                                                            throw new TAException("Invalid Operation occur: " + action.ToString());
                                                        break;
                                                }
                                            }
                                        }
                                        break;

                                    case "Amt_Before":
                                        if (canFirePrice && LineType == 1000)
                                        {
                                            if (tgtDT.Columns.Contains("ItmPriceAfter") && sourceDT.Columns.Contains("ItmPriceAfter"))
                                            {
                                                switch (GFunc.GetINTypeGroup(grow.Cells["ItmType"].Value))
                                                {
                                                    case (int)GEnum.INTypeGrp.Charges:
                                                    case (int)GEnum.INTypeGrp.Non_Stock:
                                                    case (int)GEnum.INTypeGrp.Stock:
                                                        //if (DocDetUtil.ItmPriceAfter_CustomUpdate(tgtObjDoc, tgtgrd) == false)
                                                        //    throw new TAException("Invalid Operation occur: " + action.ToString());
                                                        //Not ready, To check if the changes from above commented code to this statement have other problems or not
                                                        if (DocDetUtil.ItmPriceAfterTransfer_CustomUpdate(tgtObjDoc, tgtgrd) == false)
                                                            throw new TAException("Invalid Operation occur: " + action.ToString());
                                                        break;
                                                }
                                            }
                                        }
                                        break;

                                    case "Amt_After":
                                        if (canFireAmt && LineType == 1000)
                                        {
                                            if (tgtDT.Columns.Contains("ItmAmtShw") && sourceDT.Columns.Contains("ItmAmtShw"))
                                            {
                                                switch (GFunc.GetINTypeGroup(grow.Cells["ItmType"].Value))
                                                {
                                                    case (int)GEnum.INTypeGrp.Discount:
                                                        if (DocDetUtil.ItmAmtShwTransfer_CustomeUpdate(tgtObjDoc, tgtgrd) == false)
                                                            throw new TAException("Invalid Operation occur: " + action.ToString());
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                            #endregion

                            switch (FuncNm)
                            {
                                #region objDocKey
                                case "#objDocKey":
                                    if (tgtDT.Columns.Contains(FldNm))
                                    {
                                        if (tgtgrd.ActiveRow != null)
                                        {
                                            grow.Cells[FldNm].Value = tgtDK;
                                        }
                                    }
                                    else
                                        DocTransferData_UnMatchColumn(sourceDC, tgtDC, FldNm, DefValue, grow);
                                    break;
                                #endregion

                                #region CounterSN
                                case "#CounterSN":
                                    if (tgtDT.Columns.Contains(FldNm))
                                    {
                                        if (LineType == (int)GEnum.RecDetailType.DItems)
                                        {
                                            if (IsParentItmType((int)sourceDR["ItmType"]))
                                            {
                                                ParentSN = CounterSN + 1;
                                            }
                                            if (InsertAction == (int)GEnum.InsertAction.InsertPO || InsertAction == (int)GEnum.InsertAction.InsertCO
                                                || InsertAction == (int)GEnum.InsertAction.InsertCS || InsertAction == (int)GEnum.InsertAction.InsertPD
                                                || InsertAction == (int)GEnum.InsertAction.InsertCSR || InsertAction == (int)GEnum.InsertAction.InsertSO
                                                || InsertAction == (int)GEnum.InsertAction.InsertData)
                                            {
                                                grow.Cells[FldNm].Value = CounterSN + 0.00000001M;
                                                CounterSN = CounterSN + 0.00000001M;

                                            }
                                            else
                                            {
                                                grow.Cells[FldNm].Value = CounterSN + 0.5M;
                                                CounterSN = CounterSN + 1;
                                            }

                                            
                                        }
                                        else
                                        {
                                            grow.Cells[FldNm].Value = ParentSN;
                                        }
                                    }
                                    else
                                        DocTransferData_UnMatchColumn(sourceDC, tgtDC, FldNm, DefValue, grow);
                                    break;
                                #endregion

                                #region FieldName
                                case "#FieldName":
                                    if (tgtDT.Columns.Contains(FldNm) && sourceDT.Columns.Contains(FldNm))
                                    {
                                        if (sourceDT.Columns.Contains(FldNm))
                                            grow.Cells[FldNm].Value = sourceDR[FldNm];
                                        else
                                            DocTransferData_UnMatchColumn(sourceDC, tgtDC, FldNm, DefValue, grow);
                                    }
                                    break;
                                #endregion

                                #region LineType
                                case "#LineType":
                                    if (tgtgrd.ActiveRow != null)
                                    {
                                        if (tgtDT.Columns.Contains(FldNm))
                                        {
                                            grow.Cells[FldNm].Value = LineType;
                                        }
                                    }
                                    break;
                                #endregion

                                #region LineLink
                                case "#LineLink":
                                    if (tgtDT.Columns.Contains(FldNm) && sourceDT.Columns.Contains(FldNm))
                                    {
                                        if (LineType == (int)GEnum.RecDetailType.DItems)
                                        {
                                            if (IsParentItmType((int)sourceDR["ItmType"]))
                                            {
                                                ParentAutoKey = (int)grow.Cells["DocItmKey"].Value;
                                            }
                                            grow.Cells[FldNm].Value = 0;
                                        }
                                        else
                                        {
                                            grow.Cells[FldNm].Value = ParentAutoKey;
                                        }
                                    }
                                    break;
                                #endregion

                                #region Account
                                case "#Account":
                                    if (tgtDT.Columns.Contains(FldNm))
                                    {
                                        if (sourceDT.Columns.Contains(FldNm))
                                        {
                                            if (LineType == (int)GEnum.RecDetailType.DItems ||
                                                ((GEnum.ItemType)sourceDR["ItmType"] == GEnum.ItemType.Stock
                                                || (GEnum.ItemType)sourceDR["ItmType"] == GEnum.ItemType.Finished_GDB
                                                || (GEnum.ItemType)sourceDR["ItmType"] == GEnum.ItemType.StockB
                                                || (GEnum.ItemType)sourceDR["ItmType"] == GEnum.ItemType.Serial_StockB
                                                || (GEnum.ItemType)sourceDR["ItmType"] == GEnum.ItemType.Finished_GDB
                                                || (GEnum.ItemType)sourceDR["ItmType"] == GEnum.ItemType.Serial_Finished_GDB))
                                            {
                                                if (DocTransferData_DCGrpMatch(sourceDC, tgtDC))
                                                {
                                                    grow.Cells[FldNm].Value = sourceDR[FldNm];
                                                }
                                            }
                                            else
                                            {
                                                grow.Cells[FldNm].Value = 0;//Child Item is only available in AR Doc so it could be sales Account and there are no posting for Child in AR DO so we will set it to 0
                                            }
                                        }
                                        else
                                        {
                                            DocTransferData_UnMatchColumn(sourceDC, tgtDC, FldNm, DefValue, grow);
                                        }
                                    }
                                    break;
                                #endregion

                                #region UseSysPrice
                                case "#UseSysPrice":
                                    if (tgtDT.Columns.Contains(FldNm))
                                    {
                                        if (sourceDT.Columns.Contains(FldNm))
                                        {
                                            if (LineType == (int)GEnum.RecDetailType.DItems)
                                            {
                                                if (useSysPrice == false)
                                                    grow.Cells[FldNm].Value = sourceDR[FldNm];
                                            }
                                            else
                                            {
                                                grow.Cells[FldNm].Value = sourceDR[FldNm];
                                            }
                                        }
                                        else
                                            DocTransferData_UnMatchColumn(sourceDC, tgtDC, FldNm, DefValue, grow);
                                    }
                                    break;
                                #endregion

                                #region IsDiscount
                                case "#IsDiscount":
                                    if (tgtDT.Columns.Contains(FldNm))
                                    {
                                        if (sourceDT.Columns.Contains(FldNm))
                                        {
                                            if (LineType == (int)GEnum.RecDetailType.DItems)
                                            {
                                                if ((int)grow.Cells["ItmType"].Value == (int)GEnum.ItemType.Discount)
                                                {
                                                    grow.Cells[FldNm].Value = sourceDR[FldNm];

                                                    //The below code is replace to force ItmAmtShw to take the source value regardless ItmQty value
                                                    if (GFunc.NEDec(grow.Cells["ItmQty"].Value, 0) != 0)
                                                    {
                                                        grow.Cells[FldNm].Value = sourceDR[FldNm];
                                                        canFireAmt = true;
                                                    }
                                                    else
                                                    {
                                                        canFireAmt = false;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                grow.Cells[FldNm].Value = sourceDR[FldNm];
                                            }
                                        }
                                        else
                                            DocTransferData_UnMatchColumn(sourceDC, tgtDC, FldNm, DefValue, grow);
                                    }
                                    break;
                                #endregion

                                #region UseNSLink
                                case "#UseNSLink":
                                    if (tgtDT.Columns.Contains(FldNm))
                                    {
                                        if (sourceDT.Columns.Contains(FldNm))
                                        {
                                            if (InsertAction > 0)
                                            {
                                                grow.Cells[FldNm].Value = sourceDR[FldNm];
                                            }
                                            else
                                            {
                                                if (useNSLink)
                                                {
                                                    if (GFunc.NEStr(sourceDR[FldNm], "").ToLower().EndsWith("id") && GFunc.NEStr(sourceDR[FldNm], "").ToLower() == "nslink")
                                                        grow.Cells[FldNm].Value = sourceDR[FldNm];

                                                    switch ((GEnum.SystemCode)sourceDC)
                                                    {
                                                        case GEnum.SystemCode.Quotation:
                                                        case GEnum.SystemCode.Sales_Order:
                                                        case GEnum.SystemCode.Delivery_Order:
                                                        case GEnum.SystemCode.Sales_Invoice:
                                                        case GEnum.SystemCode.Cash_Sale:
                                                            if (tgtDC == (int)GEnum.SystemCode.Purchase_Order)
                                                            {
                                                                grow.Cells[FldNm].Value = sourceDR[FldNm];
                                                            }
                                                            break;
                                                    }
                                                }
                                            }

                                        }
                                        else
                                            DocTransferData_UnMatchColumn(sourceDC, tgtDC, FldNm, DefValue, grow);
                                    }
                                    break;
                                #endregion

                                #region ToDay
                                case "#ToDay":
                                    if (tgtDT.Columns.Contains(FldNm))
                                    {
                                        grow.Cells[FldNm].Value = DateTime.Today;
                                    }
                                    break;
                                #endregion

                                #region CurrentUser
                                case "#CurrentUser":
                                    if (tgtDT.Columns.Contains(FldNm))
                                    {
                                        grow.Cells[FldNm].Value = AppInfor.CurrentUserKey;
                                    }
                                    break;
                                #endregion

                                #region IsJobValid
                                case "#IsJobValid":
                                    if (tgtDT.Columns.Contains(FldNm) && sourceDT.Columns.Contains(FldNm))
                                    {
                                        switch (sourceDC)
                                        {
                                            case (int)GEnum.SystemCode.Quotation:
                                            case (int)GEnum.SystemCode.Sales_Order:
                                            case (int)GEnum.SystemCode.Reserve_Order:
                                            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                                            case (int)GEnum.SystemCode.Delivery_Order:
                                            case (int)GEnum.SystemCode.Packing_List:
                                            case (int)GEnum.SystemCode.Sales_Invoice:
                                            case (int)GEnum.SystemCode.Sales_Debit_Note:
                                            case (int)GEnum.SystemCode.Sales_Credit_Note:
                                            case (int)GEnum.SystemCode.Sales_Adjustment:
                                            case (int)GEnum.SystemCode.Payment_Received:
                                            case (int)GEnum.SystemCode.Cash_Sale:
                                            case (int)GEnum.SystemCode.Cash_Debit_Note:
                                            case (int)GEnum.SystemCode.Cash_Credit_Note:
                                            case (int)GEnum.SystemCode.Cash_Adjustment:
                                            case (int)GEnum.SystemCode.Cash_Payment_Received:
                                                if (LineType == (int)GEnum.RecDetailType.DItems)
                                                {
                                                    if (sourceConKey == GFunc.GetIntPropertyValue("DocConKey", tgtObjDoc))
                                                        grow.Cells[FldNm].Value = sourceDR[FldNm];
                                                }
                                                else
                                                {
                                                    if (sourceConKey == GFunc.GetIntPropertyValue("DocConKey", tgtObjDoc))
                                                        grow.Cells[FldNm].Value = sourceDR[FldNm];
                                                    else
                                                        grow.Cells[FldNm].Value = 0;
                                                }
                                                break;

                                            case (int)GEnum.SystemCode.Job:
                                                if (tgtDC == (int)GEnum.SystemCode.Purchase_Order)
                                                {
                                                    if (GFunc.GetINTypeGroup(GFunc.NEInt(sourceDR["ItmType"], 0)) == (int)GEnum.INTypeGrp.Stock)
                                                        grow.Cells[FldNm].Value = 0;
                                                    else
                                                    {
                                                        grow.Cells[FldNm].Value = sourceDR[FldNm];
                                                        if (FldNm == "ItmJobKey")// To create a reference so that update to Mst_Job
                                                        {
                                                            sourceDR["JobPODK"] = tgtDK;
                                                            sourceDR["JobPODItm"] = grow.Cells["DocItmKey"].Value;
                                                        }
                                                    }
                                                }
                                                break;

                                            case (int)GEnum.SystemCode.Purchase_Requisition:
                                                if (tgtDC == (int)GEnum.SystemCode.Purchase_Order)
                                                {
                                                    //do nothing
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                #endregion
                            }
                        }
                        #endregion
                    }
                    #endregion

                    tgtgrd.ActiveRow.Update();
                }
                #endregion
                if (tgtgrd.ActiveRow == null)
                    tgtgrd.ActiveRow = tgtgrd.Rows[tgtgrd.Rows.Count - 1];
                //Set the cursor to the newly inserted row or rows+1
                if (tgtgrd.ActiveRow.Index + 1 == tgtgrd.Rows.Count)
                {
                    tgtgrd.Rows.TemplateAddRow.Activate();
                    //tgtgrd.ActiveRow.Cells["ItmID"].Activate();
                    //tgtgrd.PerformAction(UltraGridAction.EnterEditMode, false, false);
                }
                else
                {
                    tgtgrd.ActiveRow = tgtgrd.Rows[tgtgrd.ActiveRow.Index + 1];
                }
                //tgtgrd.
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            finally
            {
                GlobalUI.bRuningImport = false;
                DocUtility.bRuningImport = false;
            }
        }//Completed
        public static bool DocTransferData(int sourceDC, int sourceDK, int sourceConKey, DataTable sourceDT, Document tgtObjDoc, UltraGrid tgtgrd, int InsertAction, string sourceSort, bool useSysPrice, bool useNSLink)
        {
            try
            {
                TAUtil.TAGridEditor tgtgrdExp = new TAUtil.TAGridEditor();
                if (DocTransferData(sourceDC, sourceDK, sourceConKey, sourceDT, tgtObjDoc, tgtgrd, tgtgrdExp, InsertAction, sourceSort, useSysPrice, useNSLink))
                    return true;
                else
                    return false;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            finally
            {
                GlobalUI.bRuningImport = false;
                DocUtility.bRuningImport = false;
            }
        }//Completed

        public static bool DocTransferData(SqlConnection cn,int sourceDC, int sourceDK, int sourceConKey, DataTable sourceDT, Document tgtObjDoc, UltraGrid tgtgrd, int InsertAction, string sourceSort, bool useSysPrice, bool useNSLink)
        {
            try
            {
                TAUtil.TAGridEditor tgtgrdExp = new TAUtil.TAGridEditor();
                if (DocTransferData(cn,sourceDC, sourceDK, sourceConKey, sourceDT, tgtObjDoc, tgtgrd, tgtgrdExp, InsertAction, sourceSort, useSysPrice, useNSLink))
                    return true;
                else
                    return false;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            finally
            {
                GlobalUI.bRuningImport = false;
                DocUtility.bRuningImport = false;
            }
        }//Completed
        public static bool DocTransferData_Expense(int sourceDC, int sourceDK, int sourceConKey, DataTable sourceDT, Document tgtObjDoc, UltraGrid tgtgrd, bool InsertMode, string sourceSort, bool useSysPrice, bool useNSLink)
        {
            try
            {
                foreach (DataRow dr in sourceDT.Rows)
                {
                    tgtgrd.DisplayLayout.Bands[0].AddNew();
                    tgtgrd.ActiveRow = tgtgrd.Rows[tgtgrd.Rows.Count - 1];

                    tgtgrd.ActiveRow.Cells["DocKey"].Value = tgtObjDoc.DocKey;
                    tgtgrd.ActiveRow.Cells["ExpDeptKey"].Value = dr["ExpDeptKey"];
                    tgtgrd.ActiveRow.Cells["ExpTranGrpKey"].Value = dr["ExpTranGrpKey"];
                    tgtgrd.ActiveRow.Cells["ExpAccKey"].Value = dr["ExpAccKey"];
                    tgtgrd.ActiveRow.Cells["ExpDate"].Value = dr["ExpDate"];
                    tgtgrd.ActiveRow.Cells["ExpRef"].Value = dr["ExpRef"];
                    tgtgrd.ActiveRow.Cells["ExpDes"].Value = dr["ExpDes"];
                    tgtgrd.ActiveRow.Cells["ExpAmtF"].Value = dr["ExpAmtF"];
                    tgtgrd.ActiveRow.Cells["ExpAmtH"].Value = dr["ExpAmtH"];
                    tgtgrd.ActiveRow.Cells["ExpAmtGST"].Value = dr["ExpAmtGST"];
                    tgtgrd.ActiveRow.Cells["ExpTaxable"].Value = dr["ExpTaxable"];
                    tgtgrd.ActiveRow.Cells["ExpTaxGrpKey"].Value = dr["ExpTaxGrpKey"];
                    tgtgrd.ActiveRow.Cells["ExpTaxGrpRate"].Value = dr["ExpTaxGrpRate"];
                    tgtgrd.ActiveRow.Cells["ExpTaxGrpAmtF"].Value = dr["ExpTaxGrpAmtF"];
                    tgtgrd.ActiveRow.Cells["ExpTaxGrpAmtL"].Value = dr["ExpTaxGrpAmtL"];
                    tgtgrd.ActiveRow.Cells["ExpJobKey"].Value = dr["ExpJobKey"];
                    tgtgrd.ActiveRow.Cells["ExpJobPhaseKey"].Value = dr["ExpJobPhaseKey"];
                    tgtgrd.ActiveRow.Cells["ExpJobTaskKey"].Value = dr["ExpJobTaskKey"];
                    tgtgrd.ActiveRow.Cells["ExpJobCostTypeKey"].Value = dr["ExpJobCostTypeKey"];
                    tgtgrd.ActiveRow.Cells["ExpAttachment"].Value = dr["ExpAttachment"];
                    tgtgrd.ActiveRow.Cells["CreateDate"].Value = DateTime.Today.Date;
                    tgtgrd.ActiveRow.Cells["CreateUserKey"].Value = AppInfor.CurrentUserKey;
                    tgtgrd.ActiveRow.Cells["LastModifiedDate"].Value = DateTime.Today.Date;
                    tgtgrd.ActiveRow.Cells["LastModifiedUserKey"].Value = AppInfor.CurrentUserKey;
                    tgtgrd.ActiveRow.Cells["Custom1"].Value = dr["Custom1"];
                    tgtgrd.ActiveRow.Cells["Custom2"].Value = dr["Custom2"];
                    tgtgrd.ActiveRow.Cells["Custom3"].Value = dr["Custom3"];
                    tgtgrd.ActiveRow.Cells["ExpAccDes"].Value = dr["ExpAccDes"];
                    tgtgrd.ActiveRow.Update();
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            finally
            {
                GlobalUI.bRuningImport = false;
                DocUtility.bRuningImport = false;
            }
        }//Completed
        private static bool DocTransferData_DCGrpMatch(int sourceDC, int targetDC)
        {
            string sourceGrp = string.Empty;
            string targetGrp = string.Empty;

            #region Get SourceDC Group
            switch (sourceDC)
            {
                case (int)GEnum.SystemCode.Quotation:
                case (int)GEnum.SystemCode.Sales_Order:
                case (int)GEnum.SystemCode.Reserve_Order:
                case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                case (int)GEnum.SystemCode.Delivery_Order:
                case (int)GEnum.SystemCode.DO_to_IV_Transfer: /* added by YST to copy all DO data including ItmAccKey of the items when converting from DO to IV */
                case (int)GEnum.SystemCode.Packing_List:
                case (int)GEnum.SystemCode.Sales_Invoice:
                case (int)GEnum.SystemCode.Sales_Debit_Note:
                case (int)GEnum.SystemCode.Sales_Credit_Note:
                case (int)GEnum.SystemCode.Sales_Adjustment:
                case (int)GEnum.SystemCode.Payment_Received:
                case (int)GEnum.SystemCode.Cash_Sale:
                case (int)GEnum.SystemCode.Cash_Debit_Note:
                case (int)GEnum.SystemCode.Cash_Credit_Note:
                case (int)GEnum.SystemCode.Cash_Adjustment:
                case (int)GEnum.SystemCode.Cash_Payment_Received:
                    sourceGrp = "AR";
                    break;

                case (int)GEnum.SystemCode.Contra:
                case (int)GEnum.SystemCode.Cash_Contra:
                    sourceGrp = "ARAP";
                    break;

                case (int)GEnum.SystemCode.Purchase_Plan:
                case (int)GEnum.SystemCode.Purchase_Request:
                case (int)GEnum.SystemCode.Purchase_Order:
                case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                case (int)GEnum.SystemCode.Purchase_Delivery:
                case (int)GEnum.SystemCode.Purchase_Invoice:
                case (int)GEnum.SystemCode.Purchase_Debit_Note:
                case (int)GEnum.SystemCode.Purchase_Credit_Note:
                case (int)GEnum.SystemCode.Purchase_Adjustment:
                case (int)GEnum.SystemCode.Payment_Issue:
                    sourceGrp = "AP";
                    break;

                case (int)GEnum.SystemCode.Inventory_Adjustment:
                case (int)GEnum.SystemCode.Inventory_Production:
                case (int)GEnum.SystemCode.Inventory_Transfer:
                    sourceGrp = "IN";
                    break;
                case (int)GEnum.SystemCode.Issue_Consignment:
                case (int)GEnum.SystemCode.Return_Consignment:
                case (int)GEnum.SystemCode.Order_Consignment:
                case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                case (int)GEnum.SystemCode.Received_Consignment:
                case (int)GEnum.SystemCode.Consignment_Settlement:
                    sourceGrp = "CG";
                    break;

                case (int)GEnum.SystemCode.Journal:
                case (int)GEnum.SystemCode.Deposit:
                case (int)GEnum.SystemCode.Bank_Revaluation:
                    sourceGrp = "GL";
                    break;
            }
            #endregion

            #region Get TargetDC Group
            switch (targetDC)
            {
                case (int)GEnum.SystemCode.Quotation:
                case (int)GEnum.SystemCode.Sales_Order:
                case (int)GEnum.SystemCode.Reserve_Order:
                case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                case (int)GEnum.SystemCode.Delivery_Order:
                case (int)GEnum.SystemCode.Packing_List:
                case (int)GEnum.SystemCode.Sales_Invoice:
                case (int)GEnum.SystemCode.Sales_Debit_Note:
                case (int)GEnum.SystemCode.Sales_Credit_Note:
                case (int)GEnum.SystemCode.Sales_Adjustment:
                case (int)GEnum.SystemCode.Payment_Received:
                case (int)GEnum.SystemCode.Cash_Sale:
                case (int)GEnum.SystemCode.Cash_Debit_Note:
                case (int)GEnum.SystemCode.Cash_Credit_Note:
                case (int)GEnum.SystemCode.Cash_Adjustment:
                case (int)GEnum.SystemCode.Cash_Payment_Received:
                    targetGrp = "AR";
                    break;

                case (int)GEnum.SystemCode.Contra:
                case (int)GEnum.SystemCode.Cash_Contra:
                    targetGrp = "ARAP";
                    break;

                case (int)GEnum.SystemCode.Purchase_Plan:
                case (int)GEnum.SystemCode.Purchase_Request:
                case (int)GEnum.SystemCode.Purchase_Order:
                case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                case (int)GEnum.SystemCode.Purchase_Delivery:
                case (int)GEnum.SystemCode.Purchase_Invoice:
                case (int)GEnum.SystemCode.Purchase_Debit_Note:
                case (int)GEnum.SystemCode.Purchase_Credit_Note:
                case (int)GEnum.SystemCode.Purchase_Adjustment:
                case (int)GEnum.SystemCode.Payment_Issue:
                    targetGrp = "AP";
                    break;

                case (int)GEnum.SystemCode.Inventory_Adjustment:
                case (int)GEnum.SystemCode.Inventory_Production:
                case (int)GEnum.SystemCode.Inventory_Transfer:
                    targetGrp = "IN";
                    break;
                case (int)GEnum.SystemCode.Issue_Consignment:
                case (int)GEnum.SystemCode.Return_Consignment:
                case (int)GEnum.SystemCode.Order_Consignment:
                case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                case (int)GEnum.SystemCode.Received_Consignment:
                case (int)GEnum.SystemCode.Consignment_Settlement:
                    targetGrp = "CG";
                    break;

                case (int)GEnum.SystemCode.Journal:
                case (int)GEnum.SystemCode.Deposit:
                case (int)GEnum.SystemCode.Bank_Revaluation:
                    targetGrp = "GL";
                    break;
            }
            #endregion

            if (sourceGrp == targetGrp)
                return true;
            else
                return false;
        }//Completed
        private static int DocTransferData_ValidSourceRow(int targetDC, DataRow dr)
        {           
            //When we transfer detail data from one doc to another, we need to 
            //1.    remove row that cannot be transfer (e.g no assembly in PO)
            //2.    convert a child row to a parent row (e.g assembly child becomes a parent row)
            //thus this function returns 
            //0 to indicate that this row have to be remove
            //or return the correct linetype to use

            //dr = source detail datarow
            //Return > 0 New LineType 
            //return 0 means invalid row and must not be append to the target grid
            
            string sourceGrp = string.Empty;
            string targetGrp = string.Empty;

            try
            {
                int Itmtype = (int)dr["ItmType"];

                switch (targetDC)
                {
                    #region QO, SO
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                        switch ((int)dr["LineType"])
                        {
                            case (int)GEnum.RecDetailType.DItmBatch:
                            case (int)GEnum.RecDetailType.DItmBatch_Serial:
                            case (int)GEnum.RecDetailType.DItmFinishedGoods_Batch:
                            case (int)GEnum.RecDetailType.DItmFinishedGoods_Batch_Serial:
                            case (int)GEnum.RecDetailType.DItmFinishedGoods_Serial:
                                return 0;

                            default:
                                return (int)dr["LineType"];
                        }
                    #endregion

                    #region DO, IV, CN, DN
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                        return (int)dr["LineType"];
                    #endregion

                    #region PR, PO
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Purchase_Order:
                        switch ((int)dr["LineType"])
                        {
                            case (int)GEnum.RecDetailType.DItems:
                                if (Itmtype == (int)GEnum.ItemType.Assembly)
                                    return 0;
                                else
                                    return (int)dr["LineType"];

                            case (int)GEnum.RecDetailType.DItmAssembly:
                                return (int)GEnum.RecDetailType.DItems; //convert child to parent

                            case (int)GEnum.RecDetailType.DItmBatch:
                            case (int)GEnum.RecDetailType.DItmBatch_Serial:
                            case (int)GEnum.RecDetailType.DItmFinishedGoods_Batch:
                            case (int)GEnum.RecDetailType.DItmFinishedGoods_Batch_Serial:
                            case (int)GEnum.RecDetailType.DItmFinishedGoods_Serial:
                                return 0;

                            default:
                                return (int)dr["LineType"];
                        }
                    #endregion

                    #region PD, BL, CN, DN
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        switch ((int)dr["LineType"])
                        {
                            case (int)GEnum.RecDetailType.DItems:
                                if (Itmtype == (int)GEnum.ItemType.Assembly)
                                    return 0;
                                else
                                    return (int)dr["LineType"];

                            case (int)GEnum.RecDetailType.DItmAssembly:
                                return (int)GEnum.RecDetailType.DItems; //convert child to parent

                            default:
                                return (int)dr["LineType"];
                        }
                    #endregion

                    #region INADJ, INTRN
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                        switch (Itmtype)
                        {
                            case (int)GEnum.ItemType.Finished_GD:
                            case (int)GEnum.ItemType.Finished_GDB:
                            case (int)GEnum.ItemType.Serial_Finished_GDB:
                            case (int)GEnum.ItemType.Serial_StockB:
                            case (int)GEnum.ItemType.Stock:
                            case (int)GEnum.ItemType.StockB:
                            case (int)GEnum.ItemType.Consignment:
                                switch ((int)dr["LineType"])
                                {
                                    case (int)GEnum.RecDetailType.DItems:
                                    case (int)GEnum.RecDetailType.DItmBatch:
                                    case (int)GEnum.RecDetailType.DItmBatch_Serial:
                                    case (int)GEnum.RecDetailType.DItmFinishedGoods_Batch:
                                    case (int)GEnum.RecDetailType.DItmFinishedGoods_Batch_Serial:
                                    case (int)GEnum.RecDetailType.DItmFinishedGoods_Serial:
                                        return (int)dr["LineType"];

                                    case (int)GEnum.RecDetailType.DItmAssembly:
                                        return (int)GEnum.RecDetailType.DItems; //convert child to parent
                                }
                                break;
                        }
                        return 0;
                    #endregion

                    #region Sales Consignment
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        switch (Itmtype)
                        {
                            case (int)GEnum.ItemType.Finished_GD:
                            case (int)GEnum.ItemType.Finished_GDB:
                            case (int)GEnum.ItemType.Serial_Finished_GDB:
                            case (int)GEnum.ItemType.Serial_StockB:
                            case (int)GEnum.ItemType.Stock:
                            case (int)GEnum.ItemType.StockB:
                                switch ((int)dr["LineType"])
                                {
                                    case (int)GEnum.RecDetailType.DItems:
                                    case (int)GEnum.RecDetailType.DItmBatch:
                                    case (int)GEnum.RecDetailType.DItmBatch_Serial:
                                    case (int)GEnum.RecDetailType.DItmFinishedGoods_Batch:
                                    case (int)GEnum.RecDetailType.DItmFinishedGoods_Batch_Serial:
                                    case (int)GEnum.RecDetailType.DItmFinishedGoods_Serial:
                                        return (int)dr["LineType"];
                                }
                                break;
                        }
                        return 0;
                    #endregion

                    #region Purchase Consignment
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        switch (Itmtype)
                        {
                            case (int)GEnum.ItemType.Consignment:
                                return (int)GEnum.RecDetailType.DItems;
                        }
                        return 0;
                    #endregion
                }
                return 0;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private static bool DocTransferData_UnMatchColumn(int sourceDC, int targetDC, string FldNm, string DefValue, UltraGridRow grdrow)
        {

            switch (DefValue)
            {
                case "Error":
                    throw new TAException("Invalid Operation occur:" + FldNm);

                case "No Change":
                    break;

                case "ToDay":
                    grdrow.Cells[FldNm].Value = DateTime.Today;
                    break;

                case "CurrentUser":
                    grdrow.Cells[FldNm].Value = AppInfor.CurrentUserKey;
                    break;

                default:
                    grdrow.Cells[FldNm].Value = DefValue;
                    break;
            }
            return true;
        }//Completed
        private static bool IsParentItmType(int ItmType)
        {
            switch (ItmType)
            {
                case (int)GEnum.ItemType.Assembly:
                case (int)GEnum.ItemType.Finished_GDB:
                case (int)GEnum.ItemType.Serial_Finished_GDB:
                case (int)GEnum.ItemType.Serial_StockB:
                case (int)GEnum.ItemType.StockB:
                    return true;

                default:
                    return false;
            }
        }//Completed

        //Error Methods
        private static Exception Error(Exception ex, bool ShowMessage)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, ShowMessage);

                if (ShowMessage)
                {
                    SysAuditLogUtility.AddErrorLog(ex);
                }
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;

        }
        private static TAException Error(TAException ex, bool ShowMessage)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage);

                if (ShowMessage)
                {
                    SysAuditLogUtility.AddErrorLog(ex);
                }
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        //Function has NOT been match with TBS  --------------------------PLEASE CREATE NEW FUNCTION BELOW THIS LINE-----------------------------------------------
    }
}
