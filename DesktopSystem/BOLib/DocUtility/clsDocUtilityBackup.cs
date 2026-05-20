using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Infragistics.Win.UltraWinGrid;
using System.IO;
using System.Collections;
using System.Transactions;
using BOLib;
using System.Reflection;
using System.Windows.Forms;

namespace BOLib
{
    public class DocUtilityBackup
    {
        //#region Declaration
        //public static bool bRuningImport = false;
        //private const int logLineType = 10; //no Detail;
        //private const int logSeq = 10;      //Document Header;       
        //private static string docAutoID = string.Empty;
        //private enum ItmTableIndex { ItemHis = 0, OutofStock = 1, OutofStockLocation = 2 };
        //#endregion

        //public static bool Document_New(SqlConnection cn, Document objDoc, Hashtable dtDetails)
        //{
        //    string OpValue = string.Empty;
        //    int newDK = GFunc.NEInt(objDoc.DocKey, 0);
        //    TAUtil.TAGridEditor grdItem = null;
        //    TAUtil.TAGridEditor grdExp = null;
        //    try
        //    {
        //        #region check Detail DataTable
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Purchase_Request:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Received_Consignment:
        //            case (int)GEnum.SystemCode.Packing_List:
        //            case (int)GEnum.SystemCode.Consignment_Settlement:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Journal:
        //            case (int)GEnum.SystemCode.Deposit:
        //                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, dtDetails, ref grdItem);
        //                grdItem.HeaderObjectKey = newDK.ToString();
        //                grdItem.DetailObjectKey = 0;
        //                break;

        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, dtDetails, ref grdItem);
        //                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, dtDetails, ref grdExp);
        //                grdItem.HeaderObjectKey = newDK.ToString();
        //                grdItem.DetailObjectKey = 0;
        //                break;
        //        }
        //        #endregion

        //        //Set DocKey
        //        objDoc._DocKey = newDK;

        //        #region Set default DocType infor
        //        DataTable dt = GFunc.GetDefaultDocType(cn, objDoc.DocCodeKey);

        //        if (dt != null)
        //        {
        //            if (dt.Rows.Count > 0)
        //            {
        //                objDoc._DocType = (int)dt.Rows[0]["DocType"];
        //                objDoc._DocTypeNm = dt.Rows[0]["DocTypeNm"].ToString();
        //                objDoc._DocSign = (short)dt.Rows[0]["DocSign"];
        //                objDoc._DocKey = newDK;
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //        #endregion Set DocType

        //        #region Set default Department
        //        int defaultDeptKey = 0;
        //        if (SysOptionUtility.UseDept)
        //        {
        //            defaultDeptKey = AppInfor.deptKey;
        //        }

        //        #region set default department for Document Header
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Deposit:
        //            case (int)GEnum.SystemCode.Bank_Revaluation:
        //                GFunc.SetPropertyValue("DocDeptKey", objDoc, defaultDeptKey);
        //                break;
        //        }
        //        #endregion

        //        #region Set Document Detail grid default Department value
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Purchase_Request:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Journal:
        //                grdItem.DisplayLayout.Bands[0].Columns["ItmDeptKey"].DefaultCellValue = defaultDeptKey;
        //                break;

        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //                grdItem.DisplayLayout.Bands[0].Columns["LinkDocDeptKey"].DefaultCellValue = defaultDeptKey;
        //                grdExp.DisplayLayout.Bands[0].Columns["ExpDeptKey"].DefaultCellValue = defaultDeptKey;
        //                break;

        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //                grdItem.DisplayLayout.Bands[0].Columns["ItmDeptKey"].DefaultCellValue = defaultDeptKey;
        //                grdExp.DisplayLayout.Bands[0].Columns["ExpDeptKey"].DefaultCellValue = defaultDeptKey;
        //                break;

        //            case (int)GEnum.SystemCode.Deposit: //Document Deposit
        //                grdItem.DisplayLayout.Bands[0].Columns["ItmDocDeptKey"].DefaultCellValue = defaultDeptKey;
        //                break;
        //        }
        //        #endregion

        //        #endregion

        //        #region Set default Branch

        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:                       //Document Enquiry
        //            case (int)GEnum.SystemCode.Purchase_Plan:                   //Document Request
        //            case (int)GEnum.SystemCode.Purchase_Request:
        //            case (int)GEnum.SystemCode.Sales_Order:                     //Document Order
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //            case (int)GEnum.SystemCode.Sales_Order_Adjustment:          //Document Order Adjustment
        //            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Works_Order:                     //Document Work
        //            case (int)GEnum.SystemCode.Delivery_Order:                  //Document Delivery
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Received_Consignment:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Packing_List:                    //Document Packing List
        //            case (int)GEnum.SystemCode.Consignment_Settlement:          //Document Settlement
        //            case (int)GEnum.SystemCode.Sales_Invoice:                   //Document Invoice
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Sales_Adjustment:                //Document Adjustment
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Received:                //Document Payment
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:            //Document Inventory
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Journal:                         //Document Account
        //            case (int)GEnum.SystemCode.Deposit:
        //            case (int)GEnum.SystemCode.Bank_Revaluation:
        //                if (SysOptionUtility.UseBranch)
        //                    GFunc.SetPropertyValue("BranchKey", objDoc, AppInfor.branchKey);
        //                else
        //                    GFunc.SetPropertyValue("BranchKey", objDoc, 0);
        //                break;
        //        }

        //        #endregion Set Branch

        //        #region Set default TranGroup

        //        #region set default TranGroup for Document Header
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Journal:
        //            case (int)GEnum.SystemCode.Deposit:
        //            case (int)GEnum.SystemCode.Bank_Revaluation:
        //                GFunc.SetPropertyValue("DocTranGrpKey", objDoc, AppInfor.tranGrpKey);
        //                break;

        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //                GFunc.SetPropertyValue("DocTranGrpKey", objDoc, AppInfor.tranGrpKey);
        //                break;
        //        }
        //        #endregion

        //        #region Set Document Detail Grid Default ItmTransGrpKey

        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Journal:
        //            case (int)GEnum.SystemCode.Deposit:
        //            case (int)GEnum.SystemCode.Purchase_Request:
        //                grdItem.DisplayLayout.Bands[0].Columns["ItmTranGrpKey"].DefaultCellValue = AppInfor.tranGrpKey;
        //                break;
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //                grdExp.DisplayLayout.Bands[0].Columns["ExpTranGrpKey"].DefaultCellValue = AppInfor.tranGrpKey;
        //                break;
        //        }
        //        #endregion

        //        #endregion Set TranGroup

        //        #region Set default Location
        //        int defaultLocation = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultInventoryLocation, cn);
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Received_Consignment:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //                objDoc.DefLocKey = defaultLocation;
        //                grdItem.DisplayLayout.Bands[0].Columns["ItmLocKey"].DefaultCellValue = defaultLocation;
        //                break;
        //            case (int)GEnum.SystemCode.Purchase_Request:
        //                objDoc.DefLocKey = defaultLocation;
        //                break;
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //                objDoc.DefFromLocKey = defaultLocation;
        //                objDoc.DefToLocKey = defaultLocation;
        //                grdItem.DisplayLayout.Bands[0].Columns["ItmFromLocKey"].DefaultCellValue = defaultLocation;
        //                grdItem.DisplayLayout.Bands[0].Columns["ItmToLocKey"].DefaultCellValue = defaultLocation;
        //                break;
        //        }
        //        #endregion Set Location

        //        #region Set default Country Rate
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Received_Consignment:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Consignment_Settlement:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Deposit:
        //            case (int)GEnum.SystemCode.Bank_Revaluation:
        //                GFunc.SetPropertyValue("DocCountryRate", objDoc, DocComUtility.CountryRate_Get(cn, 0, (int)GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 0), (decimal)GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 0), objDoc.DocDate ?? DateTime.Today, true));
        //                break;
        //        }
        //        #endregion Set Country Rate

        //        #region Set default EmKey
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Purchase_Request:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Received_Consignment:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Packing_List:
        //            case (int)GEnum.SystemCode.Consignment_Settlement:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Journal:
        //            case (int)GEnum.SystemCode.Deposit:
        //            case (int)GEnum.SystemCode.Bank_Revaluation:
        //                objDoc.DocEmKey = AppInfor.emKey;
        //                break;
        //        }
        //        #endregion Set EmKey

        //        #region Set default Shipping Address
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //                int? DefaultShipaddrKey = SysOptionUtility.GetInt("DefaultShippingAddr", cn);
        //                DocComUtility.Address_Set(cn, objDoc, DefaultShipaddrKey, false, true);
        //                break;
        //        }
        //        #endregion

        //        #region Set default for Special Fields
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //                GFunc.SetPropertyValue("QtyMultiplier", objDoc, SysOptionUtility.GetInt("APPNDefaultQtyMultiplier", cn));
        //                GFunc.SetPropertyValue("PlanMthRange", objDoc, SysOptionUtility.GetInt("APPNDefaultPlanMth", cn));
        //                GFunc.SetPropertyValue("PlanDistributeInterval", objDoc, SysOptionUtility.GetInt("APPNDefaultPlanDistributeInterval", cn));
        //                break;

        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //                GFunc.SetPropertyValue("DocPayModeKey", objDoc, SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultPaymentMode, cn));
        //                GFunc.SetPropertyValue("DocTaxGrpKey", objDoc, SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultPaymentTaxGrp, cn));

        //                int? DocTaxGrpKey = GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc);
        //                if (!GFunc.IsNEZ(DocTaxGrpKey))
        //                {
        //                    Decimal varTaxGrpRate = DocComUtility.TaxGrpRate_Get(cn, GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc).Value, objDoc.DocDate);
        //                    GFunc.SetPropertyValue("DocTaxGrpRate", objDoc, varTaxGrpRate);

        //                    grdExp.DisplayLayout.Bands[0].Columns["ExpTaxGrpKey"].DefaultCellValue = DocTaxGrpKey;
        //                    grdExp.DisplayLayout.Bands[0].Columns["ExpTaxGrpRate"].DefaultCellValue = varTaxGrpRate;
        //                }
        //                break;

        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //                GFunc.SetPropertyValue("DocPayModeKey", objDoc, SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultPaymentMode, cn));
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //                GFunc.SetPropertyValue("DocAddCostAccKey", objDoc, SysOptionUtility.GetInt("AccLandedCost", cn));
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //                GFunc.SetPropertyValue("DocAccKey", objDoc, SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultAPPDAcc, cn));
        //                break;

        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //                GFunc.SetPropertyValue("DocAccKey", objDoc, SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultINAdjAcc, cn));
        //                break;

        //            case (int)GEnum.SystemCode.Inventory_Production:
        //                GFunc.SetPropertyValue("DocAccOHKey", objDoc, SysOptionUtility.GetInt(GVar.SystemOption.OpID.MFNOverHeadAcc, cn));
        //                GFunc.SetPropertyValue("DocAccRndKey", objDoc, SysOptionUtility.GetInt(GVar.SystemOption.OpID.MFNRoundingAcc, cn));
        //                GFunc.SetPropertyValue("DocFGVarCostMode", objDoc, SysOptionUtility.GetInt(GVar.SystemOption.OpID.MFNCostMode, cn));
        //                break;
        //        }
        //        #endregion Set Special Fields

        //        objDoc.IsDirty = false;
        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //public static bool ButtonAction_Get(Document objDoc, ref GEnum.DocAction ButtonAction)
        //{
        //    //This function is use only for (New, Save, Close button_Click and form close) on the document form
        //    //to determine the ButtonAction
        //    string DocApproveOptID = string.Empty;
        //    bool runApproval = false;
        //    bool runCanPostOnlyOnce = false;
        //    bool runDeliveryOrder = false;
        //    bool runNormal = false;

        //    try
        //    {
        //        switch (objDoc.DocCodeKey)
        //        {
        //            #region DC with approval
        //            case (int)GEnum.SystemCode.Quotation:
        //                DocApproveOptID = "DocApproveForARQO";
        //                runApproval = true;
        //                break;

        //            case (int)GEnum.SystemCode.Sales_Order:
        //                DocApproveOptID = "DocApproveForARSO";
        //                runApproval = true;
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Order:
        //                DocApproveOptID = "DocApproveForAPPO";
        //                runApproval = true;
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Request:
        //                DocApproveOptID = "DocApproveForAPRQ";
        //                break;

        //            case (int)GEnum.SystemCode.Order_Consignment:
        //                DocApproveOptID = "DocApproveForINCPO";
        //                runApproval = true;
        //                break;

        //            case (int)GEnum.SystemCode.Inventory_Production:
        //                DocApproveOptID = "DocApproveForINPDT";
        //                runApproval = true;
        //                break;
        //            #endregion

        //            #region DC that can post only once
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //                runCanPostOnlyOnce = true;
        //                break;
        //            #endregion

        //            case (int)GEnum.SystemCode.Delivery_Order:
        //                runDeliveryOrder = true;
        //                break;

        //            default:
        //                runNormal = true;
        //                break;
        //        }

        //        if (runApproval)
        //        {
        //            if (objDoc.DocState == (int)GEnum.DocState.Posted)
        //                ButtonAction = GEnum.DocAction.Post;
        //            else
        //            {
        //                if (SysOptionUtility.GetInt(DocApproveOptID) != (int)GEnum.ApprovalOpiton.None)
        //                    ButtonAction = GEnum.DocAction.Save;
        //                else
        //                    ButtonAction = GEnum.DocAction.Post;
        //            }
        //        }

        //        if (runCanPostOnlyOnce)
        //        {
        //            if (objDoc.DocState == (int)GEnum.DocState.Posted)
        //            {
        //                ButtonAction = GEnum.DocAction.Undetermine;
        //                return false;
        //            }
        //            else
        //                ButtonAction = GEnum.DocAction.Save;
        //        }

        //        if (runDeliveryOrder)
        //        {
        //            if (objDoc.DocState == (int)GEnum.DocState.Invoiced)
        //            {
        //                ButtonAction = GEnum.DocAction.Undetermine;
        //                return false;
        //            }
        //            else
        //                ButtonAction = GEnum.DocAction.Post;
        //        }

        //        if (runNormal)
        //            ButtonAction = GEnum.DocAction.Post;

        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //public static bool CreateDocumentDocType_Set(SqlConnection cn, Document objDoc, int SourceDC, int SourceDK)
        //{
        //    //note: we might need to change this in future to be able to set the objDoc doctype base on the source information
        //    try
        //    {
        //        DataTable dt = GFunc.GetDefaultDocType(cn, objDoc.DocCodeKey);

        //        if (dt != null)
        //        {
        //            if (dt.Rows.Count > 0)
        //            {
        //                objDoc._DocType = (int)dt.Rows[0]["DocType"];
        //                objDoc._DocTypeNm = dt.Rows[0]["DocTypeNm"].ToString();
        //                objDoc._DocSign = (short)dt.Rows[0]["DocSign"];
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }

        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed

        ////Document Open Record
        //public static bool Doc_OpenDisAllowEdit(SqlConnection cn, Document objDoc)
        //{
        //    #region declaration
        //    bool showWarnMsg = false;
        //    bool runRevalueCheck = false;
        //    bool runPostedCheck = false;
        //    bool runDepositedCheck = false;
        //    bool runInvoicedCheck = false;
        //    bool runAppliedCheck = false;
        //    bool runReconciledCheck = false;
        //    #endregion

        //    try
        //    {
        //        showWarnMsg = SysOptionUtility.GetBool("WarnOpenDocumentAsReadOnly", cn);

        //        #region set process to run base on DocCodeKey
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //                runInvoicedCheck = true;
        //                break;

        //            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //                runPostedCheck = true;
        //                break;

        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                runRevalueCheck = true;
        //                runAppliedCheck = true;
        //                runReconciledCheck = true;
        //                break;

        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //                runRevalueCheck = true;
        //                runDepositedCheck = true;
        //                runAppliedCheck = true;
        //                runReconciledCheck = true;
        //                break;

        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //                runRevalueCheck = true;
        //                runDepositedCheck = true;
        //                runReconciledCheck = true;
        //                break;

        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //                runRevalueCheck = true;
        //                runReconciledCheck = true;
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //                runInvoicedCheck = true;
        //                runReconciledCheck = true;
        //                break;

        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Journal:
        //            case (int)GEnum.SystemCode.Deposit:
        //            case (int)GEnum.SystemCode.Bank_Revaluation:
        //                runReconciledCheck = true;
        //                break;

        //            default:
        //                return false;
        //        }
        //        #endregion

        //        #region Check document has been revalue
        //        if (runRevalueCheck)
        //        {
        //            if (SysOptionUtility.GetInt("LastARAPRevaluationPeriod", cn) > Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM")))
        //            {
        //                if (showWarnMsg)
        //                    MsgBox.Show("The document has been revalue, opening document as read only");

        //                return true;
        //            }
        //        }
        //        #endregion

        //        #region Check document has been posted
        //        if (runPostedCheck)
        //        {
        //            if (objDoc.DocState == (int)GEnum.DocState.Posted)
        //            {
        //                if (showWarnMsg)
        //                    MsgBox.Show("The document has been posted, opening document as read only");

        //                return true;
        //            }
        //        }
        //        #endregion

        //        #region Check document has been deposited
        //        if (runDepositedCheck)
        //        {
        //            if ((bool)GFunc.GetPropertyValue("DocDeposit", objDoc) == true)
        //            {
        //                if (showWarnMsg)
        //                    MsgBox.Show("The document has been deposited, opening document as read only");

        //                return true;
        //            }
        //        }
        //        #endregion

        //        #region Check document has been invoiced
        //        if (runInvoicedCheck)
        //        {
        //            if (objDoc.DocState == (int)GEnum.DocState.Invoiced)
        //            {
        //                if (showWarnMsg)
        //                    MsgBox.Show("The document has been invoiced, opening document as read only");

        //                return true;
        //            }
        //        }
        //        #endregion

        //        #region Check document has been applied
        //        if (runAppliedCheck)
        //        {
        //            if ((decimal)GFunc.GetPropertyValue("DocApplyAmtF", objDoc) != 0)
        //            {
        //                if (showWarnMsg)
        //                    MsgBox.Show("The document has been applied, opening document as read only");

        //                return true;
        //            }
        //        }
        //        #endregion

        //        #region Check document has been reconciled
        //        if (runReconciledCheck)
        //        {
        //            if (GFunc.IsDocReconciled(cn, (int)objDoc.DocCodeKey, (int)objDoc.DocKey, 0))
        //            {
        //                if (showWarnMsg)
        //                    MsgBox.Show("The document has been reconciled, opening document as read only");

        //                return true;
        //            }
        //        }
        //        #endregion

        //        return false;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //public static bool Doc_OpenDisAllowEdit(Document objDoc)
        //{
        //    #region declaration
        //    bool showWarnMsg = false;
        //    bool runRevalueCheck = false;
        //    bool runPostedCheck = false;
        //    bool runDepositedCheck = false;
        //    bool runInvoicedCheck = false;
        //    bool runAppliedCheck = false;
        //    bool runReconciledCheck = false;
        //    #endregion

        //    try
        //    {
        //        showWarnMsg = SysOptionUtility.GetBool("WarnOpenDocumentAsReadOnly");

        //        #region set process to run base on DocCodeKey
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //                runInvoicedCheck = true;
        //                break;

        //            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //                runPostedCheck = true;
        //                break;

        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                runRevalueCheck = true;
        //                runPostedCheck = true;
        //                runAppliedCheck = true;
        //                runReconciledCheck = true;
        //                break;

        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //                runRevalueCheck = true;
        //                runDepositedCheck = true;
        //                runAppliedCheck = true;
        //                runReconciledCheck = true;
        //                break;

        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //                runRevalueCheck = true;
        //                runDepositedCheck = true;
        //                runReconciledCheck = true;
        //                break;

        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //                runRevalueCheck = true;
        //                runReconciledCheck = true;
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //                runInvoicedCheck = true;
        //                runReconciledCheck = true;
        //                break;

        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Journal:
        //            case (int)GEnum.SystemCode.Deposit:
        //            case (int)GEnum.SystemCode.Bank_Revaluation:
        //                runReconciledCheck = true;
        //                break;

        //            default:
        //                return false;
        //        }
        //        #endregion

        //        #region Check document has been revalue
        //        if (runRevalueCheck)
        //        {
        //            if (SysOptionUtility.GetInt("LastARAPRevaluationPeriod") > Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM")))
        //            {
        //                if (showWarnMsg)
        //                    MsgBox.Show("The document has been revalue, opening document as read only");

        //                return true;
        //            }
        //        }
        //        #endregion

        //        #region Check document has been posted
        //        if (runPostedCheck)
        //        {
        //            if (objDoc.DocState == (int)GEnum.DocState.Posted)
        //            {
        //                if (showWarnMsg)
        //                    MsgBox.Show("The document has been posted, opening document as read only");

        //                return true;
        //            }
        //        }
        //        #endregion

        //        #region Check document has been deposited
        //        if (runDepositedCheck)
        //        {
        //            if ((bool)GFunc.GetPropertyValue("DocDeposit", objDoc) == true)
        //            {
        //                if (showWarnMsg)
        //                    MsgBox.Show("The document has been deposited, opening document as read only");

        //                return true;
        //            }
        //        }
        //        #endregion

        //        #region Check document has been invoiced
        //        if (runInvoicedCheck)
        //        {
        //            if (objDoc.DocState == (int)GEnum.DocState.Invoiced)
        //            {
        //                if (showWarnMsg)
        //                    MsgBox.Show("The document has been invoiced, opening document as read only");

        //                return true;
        //            }
        //        }
        //        #endregion

        //        #region Check document has been applied
        //        if (runAppliedCheck)
        //        {
        //            if (GFunc.NEDec(GFunc.GetPropertyValue("DocApplyAmtF", objDoc), 0) != 0)
        //            {
        //                if (showWarnMsg)
        //                    MsgBox.Show("The document has been applied, opening document as read only");

        //                return true;
        //            }
        //        }
        //        #endregion

        //        #region Check document has been reconciled
        //        if (runReconciledCheck)
        //        {
        //            if (GFunc.IsDocReconciled((int)objDoc.DocCodeKey, (int)objDoc.DocKey, 0))
        //            {
        //                if (showWarnMsg)
        //                    MsgBox.Show("The document has been reconciled, opening document as read only");

        //                return true;
        //            }
        //        }
        //        #endregion

        //        return false;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed

        //public static TransactionOptions GetTransOption()
        //{            
        //    TransactionOptions transOpt = new TransactionOptions();
        //    transOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;
        //    transOpt.Timeout = new TimeSpan(0, 5, 0);

        //    return transOpt;
        //}
      
        //public static bool Doc_SaveProcess(Document objDoc, Hashtable dtDetails, string permID, int ButtonAction, bool canclePopup)
        //{
        //    int OldDocState = objDoc.DocState.Value;
        //    try
        //    {
        //        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, GetTransOption()))
        //        {
        //            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
        //            {                        
        //                cn.Open();
        //                if (Doc_SaveProcess(cn, objDoc, dtDetails, permID, ButtonAction, canclePopup) == true)
        //                {
        //                      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }
        //        }
        //        //Add to AuditLog
        //        if (OldDocState == (int)GEnum.DocState.New)
        //            SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, (GEnum.SystemCode)objDoc.DocCodeKey.Value, objDoc.DocKey, objDoc.DocID, objDoc.DocDate, objDoc.DocTypeNm, objDoc, dtDetails);
        //        else
        //            SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, (GEnum.SystemCode)objDoc.DocCodeKey.Value, objDoc.DocKey, objDoc.DocID, objDoc.DocDate, objDoc.DocTypeNm, objDoc, dtDetails);
        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //public static bool Doc_SaveProcess(SqlConnection cn, Document objDoc, Hashtable dtDetails, string permID, int ButtonAction, bool canclePopup)
        //{
        //    bool ApprovalReq = false;
        //    bool Authorised = false;

        //    int NewDocState = 0;
        //    int OldDocState = 0;
        //    string DocAutoID = string.Empty;

        //    DataTable dtItem = null;
        //    DataTable dtVendorItem = null;
        //    DataTable dtVendor = null;
        //    DataTable dtExp = null;
        //    DataTable dtPack = null;
        //    DataTable dtSvrData = null;
        //    SYSAttachments objSYSAttachments = null;

        //    try
        //    {
        //        #region Prompt to user if re-post document
        //        if (SysOptionUtility.GetBool("WarnResaveDocument", cn))
        //            if (objDoc.DocState == (int)GEnum.DocState.Posted)
        //                if (MsgBox.Show(cn, "This document has been posted, do you wish to re-post", GEnum.MsgBoxDefaultButton.DefaultButton1, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
        //                    return false;
        //        #endregion

        //        OldDocState = objDoc.DocState.Value;

        //        #region retrive Detail DataTable from hashtable
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, dtDetails, ref dtItem);
        //                DocComUtility.DocDetail_Get(GEnum.Details.Doc_ItmVendor, dtDetails, ref dtVendorItem);
        //                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Vendor, dtDetails, ref dtVendor);
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Purchase_Request:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Received_Consignment:

        //            case (int)GEnum.SystemCode.Consignment_Settlement:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Journal:
        //            case (int)GEnum.SystemCode.Deposit:
        //                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, dtDetails, ref dtItem);
        //                break;

        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, dtDetails, ref dtItem);
        //                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, dtDetails, ref dtExp);
        //                break;

        //            case (int)GEnum.SystemCode.Packing_List:
        //                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Pack, dtDetails, ref dtPack);
        //                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, dtDetails, ref dtItem);
        //                break;
        //        }

        //        //All Documents have attachements
        //        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Attachment, dtDetails, ref objSYSAttachments);
        //        #endregion

        //        #region Set Server DateTime If Create and Modified Date is null
        //        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);

        //        //Set Header Obj
        //        objDoc.CreateDate = GFunc.NEDateTime(objDoc.CreateDate, svrDateTime);
        //        objDoc.CreateUserKey = GFunc.NEInt(objDoc.CreateUserKey, AppInfor.currentUserKey);
        //        objDoc.LastModifiedDate = svrDateTime;
        //        objDoc.LastModifiedUserKey = AppInfor.currentUserKey;

        //        //Set Detail DataTable
        //        //Items 
        //        if (dtItem != null)
        //            foreach (DataRow dr in dtItem.Rows)
        //            {
        //                dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
        //                dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
        //                dr["LastModifiedDate"] = svrDateTime;
        //                dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
        //            }
        //        //Expenses
        //        if (dtExp != null)
        //            foreach (DataRow dr in dtExp.Rows)
        //            {
        //                dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
        //                dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
        //                dr["LastModifiedDate"] = svrDateTime;
        //                dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
        //            }
        //        //Item Vendors
        //        if (dtVendorItem != null)
        //            foreach (DataRow dr in dtVendorItem.Rows)
        //            {
        //                dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
        //                dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
        //                dr["LastModifiedDate"] = svrDateTime;
        //                dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
        //            }
        //        //Venodors
        //        if (dtVendor != null)
        //            foreach (DataRow dr in dtVendor.Rows)
        //            {
        //                dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
        //                dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
        //                dr["LastModifiedDate"] = svrDateTime;
        //                dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
        //            }

        //        if (dtPack != null)
        //            foreach (DataRow dr in dtPack.Rows)
        //            {
        //                dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
        //                dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
        //                dr["LastModifiedDate"] = svrDateTime;
        //                dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
        //            }
        //        #endregion

        //        #region Check Permission
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //        {
        //            //Check for permission to Add New
        //            if (SECPermUtility.Add(cn, permID, true) == false)
        //                return false;
        //        }
        //        else
        //        {
        //            //Check for permission to Edit Existing Document
        //            if (SECPermUtility.Edit(cn, permID, true) == false)
        //                return false;
        //        }
        //        #endregion

        //        //Check for condition where saving is disallowed
        //        if (Doc_Disallow_Check(cn, objDoc, ButtonAction) == false)
        //            return false;

        //        //get Svr Data
        //        dtSvrData = DocComUtility.SvrData_Get(cn, objDoc.DocCodeKey, objDoc.DocKey);
        //        if (GFunc.IsNE(dtSvrData))
        //            return false;

        //        //Check for condition where period is closed or revaluation has been perform on the period
        //        if (Doc_PeriodClose_Check(cn, objDoc, ButtonAction, dtSvrData) == false)
        //            return false;

        //        //Check if any of document posting has been reconciled
        //        if (GFunc.IsDocReconciled(cn, (int)objDoc.DocCodeKey, (int)objDoc.DocKey, 0))
        //            return false;

        //        //Check detail ItmQty match with child detail ItmBatchQty and saving new batch into Mst_ItmBatch
        //        if (BatchQty_Check(cn, objDoc, dtItem, canclePopup, ButtonAction) == false)
        //        {
        //            MsgBox.Show(cn, MsgID.Common.ValidationFail + "%Batch Qty");
        //            return false;
        //        }

        //        //Set fields to valid values before document is save
        //        if (HiddenValue_Set(cn, objDoc, dtItem, dtExp, ButtonAction) == false)
        //        {
        //            MsgBox.Show(cn, MsgID.Common.SaveFail + "%Hidden Value");
        //            return false;
        //        }

        //        //check Details calculation
        //        if (!DocComUtility.CalForm(cn, objDoc, dtDetails, false, true))
        //        {
        //            MsgBox.Show(cn, MsgID.Common.SaveFail + "%Calculate Form");
        //            return false;
        //        }

        //        //Check SalesPriceControl is exceed for DO,IV,CN,DN,QO only                    
        //        if (Doc_SaleControlPrice_Check(cn, objDoc, dtItem, ButtonAction, canclePopup) == false)
        //            return false;

        //        //Check and Get Document authorisation
        //        if (DocAuthorisation_Get(cn, objDoc, ButtonAction, ref ApprovalReq, ref Authorised) == false)
        //            return false;

        //        //Perform posting Process               
        //        if (Doc_Posting(cn, objDoc, dtDetails, ButtonAction, dtSvrData, ApprovalReq, Authorised, out DocAutoID, out NewDocState) == false)
        //            return false;

        //        AttachmentSave(cn, objSYSAttachments, objDoc);

        //        //Update objDoc(DocID,DocState) with newDocID and newDocState
        //        if (OldDocState == (int)GEnum.DocState.New)
        //        {
        //            if (objDoc.DocID == string.Empty && DocAutoID != string.Empty)
        //                objDoc.DocID = DocAutoID;
        //            objDoc.DocState = NewDocState;
        //            SysLockUtility.AddLock(cn, true, objDoc.GUID, (GEnum.SystemCode)objDoc.DocCodeKey, objDoc.DocKey);
        //        }

        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //public static bool Doc_DeleteProcess(Document objDoc, Hashtable dtDetails, string permID)
        //{
        //    try
        //    {
        //        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
        //        {
        //            cn.Open();
        //            return Doc_DeleteProcess(cn, objDoc, dtDetails, permID);
        //        }
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //public static bool Doc_DeleteProcess(SqlConnection cn, Document objDoc, Hashtable dtDetails, string permID)
        //{

        //    #region declaration

        //    bool ApprovalReq = true;                            //for delete this is always true
        //    bool Authorised = false;                            //for delete this is always true
        //    int ButtonAction = (int)GEnum.DocAction.Delete;     //for delete this is always delete
        //    int NewDocState = 0;
        //    int? OldDocState = objDoc.DocState;
        //    string DocAutoID = string.Empty;
        //    DataTable dtSvrData = null;
        //    DataTable dtItem = null;

        //    #endregion

        //    try
        //    {
        //        //Check Permission
        //        if (SECPermUtility.Delete(cn, permID, true) == false)
        //            return false;

        //        //Check for condition where deletion is disallowed
        //        if (Doc_Disallow_Check(cn, objDoc, ButtonAction) == false)
        //            return false;

        //        //get Svr Data
        //        dtSvrData = DocComUtility.SvrData_Get(cn, objDoc.DocCodeKey, objDoc.DocKey);
        //        if (GFunc.IsNE(dtSvrData) == true)
        //            return false;

        //        //Check for condition where period is closed or revaluation has been perform on the period
        //        if (Doc_PeriodClose_Check(cn, objDoc, ButtonAction, dtSvrData) == false)
        //            return false;

        //        //Check if any of document posting has been reconciled
        //        if (GFunc.IsDocReconciled(cn, (int)objDoc.DocCodeKey, (int)objDoc.DocKey, 0))
        //            return false;

        //        //Check if the Batch is used in other dependency tables
        //        if (ButtonAction == (int)GEnum.DocAction.Delete)
        //        {
        //            switch (objDoc.DocCodeKey)
        //            {
        //                case (int)GEnum.SystemCode.Purchase_Invoice:
        //                case (int)GEnum.SystemCode.Purchase_Delivery:
        //                case (int)GEnum.SystemCode.Inventory_Adjustment:
        //                case (int)GEnum.SystemCode.Inventory_Production:
        //                    if (GFunc.CheckBatchDependantsExists(cn, 0, (int)objDoc.DocCodeKey, (int)objDoc.DocKey))
        //                        return false;

        //                    break;
        //            }
        //        }

        //        //Delete Confirmation.
        //        if (ButtonAction == (int)GEnum.DocAction.Delete)
        //        {
        //            if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnDeleteDocument, cn)) //Mic_Ask_XXX
        //            {
        //                if (MsgBox.Show(cn, MsgID.Common.ConfirmDelete, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
        //                    return false;
        //            }
        //        }

        //        //Perform posting Process               
        //        if (Doc_Posting(cn, objDoc, dtDetails, ButtonAction, dtSvrData, ApprovalReq, Authorised, out DocAutoID, out NewDocState) == false)
        //            return false;

        //        //Add to AuditLog
        //        SysAuditLogUtility.AddAuditLog(cn, GEnum.AuditLogMode.Delete, (GEnum.SystemCode)objDoc.DocCodeKey.Value, objDoc.DocKey, objDoc.DocID, objDoc.DocDate, objDoc.DocTypeNm, objDoc, dtDetails);

        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //private static bool BatchQty_Check(Document objDoc, DataTable dtItems, bool canclePopup)
        //{
        //    try
        //    {
        //        #region "insert into Mst_BatchItm and Get BatchKey"
        //        int? BatchKey = 0, logDC = 0, logDK = 0, logDItm = 0, itmKey = 0;
        //        string BatchID = string.Empty;
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Opening_Balance:

        //                foreach (DataRow dr in dtItems.Rows)
        //                {
        //                    if (int.Parse(dr["ItmBatchKey"].ToString()) == 0 && int.Parse(dr["LineType"].ToString()) > 1000 &&
        //                        (int.Parse(dr["ItmType"].ToString()) == (int)GEnum.ItemType.StockB
        //                        || int.Parse(dr["ItmType"].ToString()) == (int)GEnum.ItemType.Finished_GDB
        //                        || int.Parse(dr["ItmType"].ToString()) == (int)GEnum.ItemType.Serial_StockB
        //                        || int.Parse(dr["ItmType"].ToString()) == (int)GEnum.ItemType.Serial_Finished_GDB))
        //                    {
        //                        MSTItmBatch Batch = new MSTItmBatch();
        //                        itmKey = GFunc.IsNE(dr["ItmKey"]) ? 0 : int.Parse(dr["ItmKey"].ToString());
        //                        BatchID = GFunc.IsNE(dr["BatchID"]) ? string.Empty : dr["BatchID"].ToString();
        //                        if (Batch.Validation(new MSTItmBatch.Criteria(BatchKey, itmKey, BatchID, 1), objDoc.IsNew) == false)
        //                        {
        //                            return false;
        //                        }
        //                        Batch.LogDC = objDoc.DocCodeKey;
        //                        Batch.LogDK = objDoc.DocKey;
        //                        Batch.LogDocDate = objDoc.DocDate;
        //                        Batch.LogDItm = GFunc.IsNE(dr["DocItmKey"]) ? 0 : int.Parse(dr["DocItmKey"].ToString());
        //                        Batch.BatchID = BatchID;
        //                        Batch.BatchItmKey = itmKey;
        //                        Batch.BatchQty = GFunc.IsNE(dr["ItmBatchQty"]) ? 0 : decimal.Parse(dr["ItmBatchQty"].ToString());
        //                        Batch.BatchQtyBal = Batch._batchQty;
        //                        Batch.BatchStatus = false;
        //                        Batch.PurgeData = false;
        //                        Batch.PurgeKeep = 0;
        //                        Batch.Insert(out BatchKey);

        //                        dr["ItmBatchKey"] = BatchKey;

        //                        MSTItmBatchLog BatchLog = new MSTItmBatchLog();
        //                        BatchLog.LogDC = objDoc.DocCodeKey;
        //                        BatchLog.LogDK = objDoc.DocKey;
        //                        BatchLog.LogDocDate = objDoc.DocDate;
        //                        BatchLog.LogDItm = GFunc.IsNE(dr["DocItmKey"]) ? 0 : int.Parse(dr["DocItmKey"].ToString());
        //                        BatchLog.BatchKey = BatchKey;
        //                        BatchLog.BatchQty = GFunc.IsNE(dr["ItmBatchQty"]) ? 0 : decimal.Parse(dr["ItmBatchQty"].ToString());
        //                        BatchLog.LogType = 0;
        //                        BatchLog.PurgeData = false;
        //                        BatchLog.PurgeKeep = 0;
        //                        BatchLog.Insert(out BatchKey, out logDC, out logDK, out logDItm);
        //                    }

        //                }
        //                break;




        //        }
        //        #endregion
        //        if (!canclePopup)
        //        {
        //            switch (objDoc.DocCodeKey)
        //            {
        //                case (int)GEnum.SystemCode.Delivery_Order:
        //                case (int)GEnum.SystemCode.Sales_Invoice:
        //                case (int)GEnum.SystemCode.Sales_Debit_Note:
        //                case (int)GEnum.SystemCode.Sales_Credit_Note:
        //                case (int)GEnum.SystemCode.Cash_Sale:
        //                case (int)GEnum.SystemCode.Cash_Debit_Note:
        //                case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                case (int)GEnum.SystemCode.Purchase_Delivery:
        //                case (int)GEnum.SystemCode.Purchase_Invoice:
        //                case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //                case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                case (int)GEnum.SystemCode.Inventory_Adjustment:
        //                case (int)GEnum.SystemCode.Inventory_Production:
        //                case (int)GEnum.SystemCode.Inventory_Transfer:

        //                    //get Parent Items ,linetype = 1000 and ItmType is (Batch [...B])
        //                    var getParent = from row in dtItems.AsEnumerable()
        //                                    where row.Field<int>("LineType") == 1000 &&
        //                                    (row.Field<int>("ItmType") == 110 || row.Field<int>("ItmType") == 210 || row.Field<int>("ItmType") == 310 || row.Field<int>("ItmType") == 410)
        //                                    select new
        //                                    {
        //                                        DocItmKey = row.Field<int>("DocItmKey"),
        //                                        ItmQty = row.Field<decimal>("ItmQty")
        //                                    };

        //                    //get child Items
        //                    var getChilds = from row in dtItems.AsEnumerable()
        //                                    where row.Field<int>("LineType") > 1000
        //                                    group row by new
        //                                    {
        //                                        lineLinkKey = row.Field<int>("LineLinkKey")

        //                                    } into grp
        //                                    select new
        //                                    {
        //                                        LineLinkKey = grp.Key.lineLinkKey,
        //                                        ItmBatchQty = grp.Sum(r => r.Field<decimal>("ItmBatchQty"))
        //                                    };
        //                    //join Parent and Childs 
        //                    var result = from parent in getParent
        //                                 join child in getChilds on parent.DocItmKey equals child.LineLinkKey
        //                                 select new
        //                                 {
        //                                     ItmQty = parent.ItmQty,
        //                                     ItmBatchQty = child.ItmBatchQty,
        //                                 };

        //                    //check ItmQty vs the total sum of childdetail ItmBatchQty
        //                    foreach (var row in result)
        //                    {
        //                        if (GFunc.NEDec(row.ItmQty, 0) != GFunc.NEDec(row.ItmBatchQty, 0))
        //                        {
        //                            MsgBox.Show("Batch Qty validation failed, process cancelled");
        //                            return false;
        //                        }
        //                    }
        //                    break;

        //            }
        //        }

        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}
        //public static bool BatchQty_Check(SqlConnection cn, Document objDoc, DataTable dtItems, bool canclePopup, int ButtonAction)
        //{
        //    //Note: canclePopup is no longer in use as the batch Entry popup form is call thru this function : GlobalUI.UpdateBatchChildItem()
        //    //now we just keep in for future if we need to implement it next time
        //    try
        //    {
        //        int? BatchKey = 0, logDC = 0, logDK = 0, logDItm = 0, itmKey = 0;
        //        string BatchID = string.Empty;

        //        #region Insert into Mst_BatchItm and Get BatchKey
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //                foreach (DataRow dr in dtItems.Rows)
        //                {
        //                    //New Batch
        //                    if (int.Parse(dr["ItmBatchKey"].ToString()) == 0 &&
        //                        (int.Parse(dr["LineType"].ToString()) != 3000 && int.Parse(dr["LineType"].ToString()) != 3100 && int.Parse(dr["LineType"].ToString()) != 3200) &&
        //                        (int.Parse(dr["ItmType"].ToString()) == (int)GEnum.ItemType.StockB
        //                        || int.Parse(dr["ItmType"].ToString()) == (int)GEnum.ItemType.Finished_GDB
        //                        || int.Parse(dr["ItmType"].ToString()) == (int)GEnum.ItemType.Serial_StockB
        //                        || int.Parse(dr["ItmType"].ToString()) == (int)GEnum.ItemType.Serial_Finished_GDB))
        //                    {
        //                        MSTItmBatch Batch = new MSTItmBatch();
        //                        itmKey = GFunc.NEInt(dr["ItmKey"], 0);
        //                        BatchID = GFunc.NEStr(dr["BatchID"], string.Empty);
        //                        if (Batch.Validation(cn, new MSTItmBatch.Criteria(BatchKey, itmKey, BatchID, 1), objDoc.IsNew) == false)
        //                            return false;

        //                        Batch.LogDC = objDoc.DocCodeKey;
        //                        Batch.LogDK = objDoc.DocKey;
        //                        Batch.LogDocDate = objDoc.DocDate;
        //                        Batch.LogDItm = (int)dr["DocItmKey"];
        //                        Batch.BatchID = BatchID;
        //                        Batch.BatchItmKey = itmKey;
        //                        Batch.BatchQty = (decimal)dr["ItmBatchQty"];
        //                        Batch.BatchQtyBal = 0;
        //                        Batch.BatchCost = 0;
        //                        Batch.BatchStatus = false;
        //                        Batch.PurgeData = false;
        //                        Batch.PurgeKeep = 0;
        //                        Batch.Insert(cn, out BatchKey);

        //                        dr["ItmBatchKey"] = BatchKey;

        //                    }
        //                }
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Opening_Balance:
        //                foreach (DataRow dr in dtItems.Rows)
        //                {
        //                    //New Batch
        //                    if (int.Parse(dr["ItmBatchKey"].ToString()) == 0 && int.Parse(dr["LineType"].ToString()) > 1000 &&
        //                        (int.Parse(dr["ItmType"].ToString()) == (int)GEnum.ItemType.StockB
        //                        || int.Parse(dr["ItmType"].ToString()) == (int)GEnum.ItemType.Finished_GDB
        //                        || int.Parse(dr["ItmType"].ToString()) == (int)GEnum.ItemType.Serial_StockB
        //                        || int.Parse(dr["ItmType"].ToString()) == (int)GEnum.ItemType.Serial_Finished_GDB))
        //                    {
        //                        MSTItmBatch Batch = new MSTItmBatch();
        //                        itmKey = GFunc.NEInt(dr["ItmKey"], 0);
        //                        BatchID = GFunc.NEStr(dr["BatchID"], string.Empty);
        //                        if (Batch.Validation(cn, new MSTItmBatch.Criteria(BatchKey, itmKey, BatchID, 1), objDoc.IsNew) == false)
        //                        {
        //                            return false;
        //                        }
        //                        Batch.LogDC = objDoc.DocCodeKey;
        //                        Batch.LogDK = objDoc.DocKey;
        //                        Batch.LogDocDate = objDoc.DocDate;
        //                        Batch.LogDItm = (int)dr["DocItmKey"];
        //                        Batch.BatchID = BatchID;
        //                        Batch.BatchItmKey = itmKey;
        //                        Batch.BatchQty = (decimal)dr["ItmBatchQty"];
        //                        Batch.BatchQtyBal = 0;
        //                        Batch.BatchCost = 0;
        //                        Batch.BatchStatus = false;
        //                        Batch.PurgeData = false;
        //                        Batch.PurgeKeep = 10;
        //                        Batch.Insert(cn, out BatchKey);

        //                        dr["ItmBatchKey"] = BatchKey;
        //                    }
        //                }
        //                break;
        //        }
        //        #endregion

        //        #region Check Batch Qty
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:

        //                dtItems.DefaultView.RowFilter = "ItmType IN (" + (int)GEnum.ItemType.StockB + "," + (int)GEnum.ItemType.Finished_GDB + "," + (int)GEnum.ItemType.Serial_StockB + "," + (int)GEnum.ItemType.Serial_Finished_GDB + ") ";
        //                DataTable dt = dtItems.DefaultView.ToTable(false, "LineType", "LineLinkKey", "DocItmKey", "ItmType", "ItmQty", "ItmBatchQty");
        //                dt.TableName = "dtDetail";
        //                string xmlData = GFunc.ConvertDataTableToXML(dt);
        //                dtItems.DefaultView.RowFilter = "";

        //                List<SqlParameter> paraList = new List<SqlParameter>();
        //                paraList.Add(new SqlParameter("@xmlData", xmlData));
        //                SqlParameter RetValue = new SqlParameter();
        //                RetValue.ParameterName = "@RetValue";
        //                RetValue.Value = 0;
        //                RetValue.Direction = ParameterDirection.InputOutput;
        //                paraList.Add(RetValue);
        //                GFunc.ExecuteNonQueryProc(cn, "BatchQty_Check", paraList);
        //                if (GFunc.NEInt(RetValue.Value, 0) == (int)GEnum.SpState.Fail)
        //                {
        //                    MsgBox.Show("Batch Qty validation failed");
        //                    return false;
        //                }

        //                break;

        //            case (int)GEnum.SystemCode.Inventory_Production:
        //                dtItems.DefaultView.RowFilter = "ItmType IN (" + (int)GEnum.ItemType.StockB + "," + (int)GEnum.ItemType.Finished_GDB + "," + (int)GEnum.ItemType.Serial_StockB + "," + (int)GEnum.ItemType.Serial_Finished_GDB + ") ";


        //                DataTable dtPDT = dtItems.DefaultView.ToTable(false, "LineType", "LineLinkKey", "DocItmKey", "ItmType", "FGProduceQty", "BOMUsed", "ItmBatchQty");
        //                dtPDT.TableName = "dtDetail";
        //                string xmlPDTData = GFunc.ConvertDataTableToXML(dtPDT);
        //                dtItems.DefaultView.RowFilter = "";

        //                List<SqlParameter> paraPDTList = new List<SqlParameter>();
        //                paraPDTList.Add(new SqlParameter("@xmlData", xmlPDTData));
        //                SqlParameter RetPDTValue = new SqlParameter();
        //                RetPDTValue.ParameterName = "@RetValue";
        //                RetPDTValue.Value = 0;
        //                RetPDTValue.Direction = ParameterDirection.InputOutput;
        //                paraPDTList.Add(RetPDTValue);
        //                GFunc.ExecuteNonQueryProc(cn, "BatchQty_Check", paraPDTList);
        //                if (GFunc.NEInt(RetPDTValue.Value, 0) == (int)GEnum.SpState.Fail)
        //                {
        //                    MsgBox.Show("Batch Qty validation failed");
        //                    return false;
        //                }

        //                break;
        //        }
        //        #endregion

        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //public static bool HiddenValue_Set(SqlConnection cn, Document objDoc, DataTable dtItem, DataTable dtExp, int ButtonAction)
        //{
        //    string msgID = string.Empty;
        //    DataTable dtParent = null;

        //    try
        //    {
        //        #region set DocDate
        //        //Note: during copyForm function, this value may be set to DateTime
        //        //we need to ensure that the DocDate is alway storing a Date value only w/o the time
        //        GFunc.SetPropertyValue("DocDate", objDoc, objDoc.DocDate.Value.Date);
        //        #endregion

        //        #region set disdate and duedate
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                if (DocDisDueDate_Set(cn, objDoc) == false)
        //                    return false;
        //                break;
        //        }

        //        #endregion

        //        #region set departmentkey
        //        int deptOpt = 0;
        //        int deptKey = 0;
        //        bool runDetAssBatch = false;

        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            case (int)GEnum.SystemCode.Bank_Revaluation:
        //                if (GFunc.IsNE(GFunc.GetPropertyValue("DocDeptKey", objDoc)))
        //                    GFunc.SetPropertyValue("DocDeptKey", objDoc, 0);
        //                break;

        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                runDetAssBatch = true;
        //                deptKey = GFunc.NEInt(GFunc.GetPropertyValue("DocDeptKey", objDoc), 0);
        //                GFunc.SetPropertyValue("DocDeptKey", objDoc, deptKey);
        //                deptOpt = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DepartmentOptionForAR, cn);
        //                if (HiddenValueDept_Set(cn, dtItem, deptOpt, deptKey, true, (int)objDoc.DocCodeKey) == false)
        //                    return true;
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //                deptKey = GFunc.NEInt(GFunc.GetPropertyValue("DocDeptKey", objDoc), 0);
        //                GFunc.SetPropertyValue("DocDeptKey", objDoc, deptKey);
        //                deptOpt = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DepartmentOptionForAP, cn);
        //                if (HiddenValueDept_Set(cn, dtItem, deptOpt, deptKey, false, (int)objDoc.DocCodeKey) == false)
        //                    return true;
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                runDetAssBatch = true;
        //                deptKey = GFunc.NEInt(GFunc.GetPropertyValue("DocDeptKey", objDoc), 0);
        //                GFunc.SetPropertyValue("DocDeptKey", objDoc, deptKey);
        //                deptOpt = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DepartmentOptionForAP, cn);
        //                if (HiddenValueDept_Set(cn, dtItem, deptOpt, deptKey, true, (int)objDoc.DocCodeKey) == false)
        //                    return true;
        //                break;

        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //                deptKey = GFunc.NEInt(GFunc.GetPropertyValue("DocDeptKey", objDoc), 0);
        //                GFunc.SetPropertyValue("DocDeptKey", objDoc, deptKey);
        //                deptOpt = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DepartmentOptionForIN, cn);
        //                if (HiddenValueDept_Set(cn, dtItem, deptOpt, deptKey, true, (int)objDoc.DocCodeKey) == false)
        //                    return true;
        //                break;

        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //                deptKey = GFunc.NEInt(GFunc.GetPropertyValue("DocDeptKey", objDoc), 0);
        //                GFunc.SetPropertyValue("DocDeptKey", objDoc, deptKey);
        //                deptOpt = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DepartmentOptionForAR, cn);
        //                if (HiddenValueDept_Set(cn, dtExp, deptOpt, deptKey, false, (int)objDoc.DocCodeKey) == false)
        //                    return true;
        //                break;

        //            case (int)GEnum.SystemCode.Payment_Issue:
        //                deptKey = GFunc.NEInt(GFunc.GetPropertyValue("DocDeptKey", objDoc), 0);
        //                GFunc.SetPropertyValue("DocDeptKey", objDoc, deptKey);
        //                deptOpt = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DepartmentOptionForAP, cn);
        //                if (HiddenValueDept_Set(cn, dtExp, deptOpt, deptKey, false, GFunc.NEInt(objDoc.DocCodeKey, 0)) == false)
        //                    return true;
        //                break;

        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //                deptKey = (int)GFunc.NEInt(GFunc.GetPropertyValue("DocDeptKey", objDoc), 0);
        //                GFunc.SetPropertyValue("DocDeptKey", objDoc, deptKey);
        //                deptOpt = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DepartmentOptionForCSG, cn);
        //                if (HiddenValueDept_Set(cn, dtItem, deptOpt, deptKey, true, (int)objDoc.DocCodeKey) == false)
        //                    return false;
        //                else
        //                {
        //                    if (HiddenValueDept_Set(cn, dtExp, deptOpt, deptKey, false, (int)objDoc.DocCodeKey) == false)
        //                        return false;
        //                }
        //                break;

        //            case (int)GEnum.SystemCode.Deposit:
        //                deptKey = (int)GFunc.NEInt(GFunc.GetPropertyValue("DocDeptKey", objDoc), 0);
        //                GFunc.SetPropertyValue("DocDeptKey", objDoc, deptKey);
        //                deptOpt = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DepartmentOptionForGL, cn);
        //                if (HiddenValueDept_Set(cn, dtItem, deptOpt, deptKey, false, (int)objDoc.DocCodeKey) == false)
        //                    return true;
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Request:
        //                runDetAssBatch = true;
        //                deptKey = 0;
        //                deptOpt = 0;
        //                if (HiddenValueDept_Set(cn, dtItem, deptOpt, deptKey, true, (int)objDoc.DocCodeKey) == false)
        //                    return true;
        //                break;

        //            case (int)GEnum.SystemCode.Journal:
        //                deptKey = 0;
        //                deptOpt = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DepartmentOptionForGL, cn);
        //                if (HiddenValueDept_Set(cn, dtItem, deptOpt, deptKey, false, (int)objDoc.DocCodeKey) == false)
        //                    return true;
        //                break;

        //            case (int)GEnum.SystemCode.Packing_List:
        //                deptKey = 0;
        //                deptOpt = 0;
        //                if (HiddenValueDept_Set(cn, dtItem, deptOpt, deptKey, false, (int)objDoc.DocCodeKey) == false)
        //                    return true;
        //                break;
        //        }
        //        #endregion

        //        #region set detail Job infor (LineType 1000) & Set Detail Assembly/Batch Dept and TranGrp to same as the parent
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:

        //                #region set detail Expense Job Infor
        //                foreach (DataRowView row in dtExp.DefaultView)
        //                {
        //                    row["ExpJobKey"] = GFunc.NEInt(row["ExpJobKey"], 0);
        //                    row["ExpJobPhaseKey"] = GFunc.NEInt(row["ExpJobPhaseKey"], 0);
        //                    row["ExpJobTaskKey"] = GFunc.NEInt(row["ExpJobTaskKey"], 0);
        //                    row["ExpJobCostTypeKey"] = GFunc.NEInt(row["ExpJobCostTypeKey"], 0);
        //                }
        //                break;
        //                #endregion

        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //            case (int)GEnum.SystemCode.Received_Consignment:
        //            case (int)GEnum.SystemCode.Journal:

        //                #region set detail item Job infor (LineType 1000)

        //                dtItem.DefaultView.RowFilter = "";
        //                //Check if LineType column exist in this detail DataTable ,because some document doesn't have it.
        //                if (dtItem.Columns.Contains("LineType"))
        //                    dtItem.DefaultView.RowFilter = "LineType = " + (int)GEnum.RecDetailType.DItems + "";

        //                foreach (DataRowView row in dtItem.DefaultView)
        //                {
        //                    if (dtItem.Columns.Contains("LineType"))
        //                        //to apply filter on defaultView new row.
        //                        if (GFunc.NEInt(row["LineType"], 0) != (int)GEnum.RecDetailType.DItems)
        //                            continue;

        //                    switch (objDoc.DocCodeKey)
        //                    {
        //                        case (int)GEnum.SystemCode.Purchase_Order:
        //                        case (int)GEnum.SystemCode.Purchase_Delivery:
        //                        case (int)GEnum.SystemCode.Purchase_Invoice:
        //                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                        case (int)GEnum.SystemCode.Issue_Consignment:
        //                        case (int)GEnum.SystemCode.Return_Consignment:
        //                        case (int)GEnum.SystemCode.Order_Consignment:
        //                            if (GFunc.GetINTypeGroup(GFunc.NEInt(row["ItmType"], 0)) == (int)GEnum.INTypeGrp.Stock)
        //                            {
        //                                row["ItmJobKey"] = 0;
        //                                row["ItmJobPhaseKey"] = 0;
        //                                row["ItmJobTaskKey"] = 0;
        //                                row["ItmJobCostTypeKey"] = 0;
        //                            }
        //                            else
        //                            {
        //                                row["ItmJobKey"] = GFunc.NEInt(row["ItmJobKey"], 0);
        //                                row["ItmJobPhaseKey"] = GFunc.NEInt(row["ItmJobPhaseKey"], 0);
        //                                row["ItmJobTaskKey"] = GFunc.NEInt(row["ItmJobTaskKey"], 0);
        //                                row["ItmJobCostTypeKey"] = GFunc.NEInt(row["ItmJobCostTypeKey"], 0);

        //                            }
        //                            break;
        //                        case (int)GEnum.SystemCode.Quotation:
        //                        case (int)GEnum.SystemCode.Sales_Order:
        //                        case (int)GEnum.SystemCode.Sales_Invoice:
        //                        case (int)GEnum.SystemCode.Delivery_Order:
        //                        case (int)GEnum.SystemCode.Inventory_Adjustment:
        //                        case (int)GEnum.SystemCode.Journal:
        //                            row["ItmJobKey"] = GFunc.NEInt(row["ItmJobKey"], 0);
        //                            row["ItmJobPhaseKey"] = GFunc.NEInt(row["ItmJobPhaseKey"], 0);
        //                            row["ItmJobTaskKey"] = GFunc.NEInt(row["ItmJobTaskKey"], 0);
        //                            row["ItmJobCostTypeKey"] = GFunc.NEInt(row["ItmJobCostTypeKey"], 0);
        //                            break;
        //                    }
        //                }
        //                #endregion

        //                #region Get copy of Parent dataTable (note: this dataTable is use for updating child data, no data is updated into this parent table
        //                if (dtItem.Columns.Contains("LineType"))
        //                    dtItem.DefaultView.RowFilter = "ItmType IN(110,210,250,310,410) And LineLinkKey=0";//Assembly and Batch  
        //                dtParent = dtItem.DefaultView.ToTable();
        //                dtItem.DefaultView.RowFilter = "";
        //                #endregion

        //                #region Set Detail child Assembly/Batch Dept and TranGrp to same as the parent
        //                if (runDetAssBatch)
        //                {
        //                    int itmDepKey = 0;
        //                    int itmTranGrpKey = 0;
        //                    int itmJobKey = 0;
        //                    int itmJobPhaseKey = 0;
        //                    int itmJobTaskKey = 0;
        //                    int itmJobCostTypeKey = 0;

        //                    dtParent.DefaultView.RowFilter = "ItmType IN(110,210,250,310,410) And LineLinkKey=0";//Assembly and Batch                                                        

        //                    foreach (DataRowView rowParent in dtParent.DefaultView)
        //                    {
        //                        #region get parent Dept
        //                        switch (objDoc.DocCodeKey)
        //                        {
        //                            case (int)GEnum.SystemCode.Quotation:
        //                            case (int)GEnum.SystemCode.Sales_Order:
        //                            case (int)GEnum.SystemCode.Delivery_Order:
        //                            case (int)GEnum.SystemCode.Sales_Invoice:
        //                            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //                            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //                            case (int)GEnum.SystemCode.Cash_Sale:
        //                            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //                            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                            case (int)GEnum.SystemCode.Purchase_Plan:
        //                            case (int)GEnum.SystemCode.Purchase_Request:
        //                            case (int)GEnum.SystemCode.Purchase_Order:
        //                            case (int)GEnum.SystemCode.Purchase_Delivery:
        //                            case (int)GEnum.SystemCode.Purchase_Invoice:
        //                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //                            case (int)GEnum.SystemCode.Inventory_Production:
        //                            case (int)GEnum.SystemCode.Inventory_Transfer:
        //                            case (int)GEnum.SystemCode.Issue_Consignment:
        //                            case (int)GEnum.SystemCode.Return_Consignment:
        //                            case (int)GEnum.SystemCode.Journal:
        //                                itmDepKey = GFunc.NEInt(rowParent["ItmDeptKey"], 0);
        //                                break;
        //                        }
        //                        #endregion

        //                        #region get parent TranGrp
        //                        switch (objDoc.DocCodeKey)
        //                        {
        //                            case (int)GEnum.SystemCode.Quotation:
        //                            case (int)GEnum.SystemCode.Sales_Order:
        //                            case (int)GEnum.SystemCode.Delivery_Order:
        //                            case (int)GEnum.SystemCode.Sales_Invoice:
        //                            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //                            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //                            case (int)GEnum.SystemCode.Cash_Sale:
        //                            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //                            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                            case (int)GEnum.SystemCode.Purchase_Plan:
        //                            case (int)GEnum.SystemCode.Purchase_Request:
        //                            case (int)GEnum.SystemCode.Purchase_Order:
        //                            case (int)GEnum.SystemCode.Purchase_Delivery:
        //                            case (int)GEnum.SystemCode.Purchase_Invoice:
        //                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //                            case (int)GEnum.SystemCode.Inventory_Production:
        //                            case (int)GEnum.SystemCode.Inventory_Transfer:
        //                            case (int)GEnum.SystemCode.Issue_Consignment:
        //                            case (int)GEnum.SystemCode.Return_Consignment:
        //                            case (int)GEnum.SystemCode.Journal:
        //                            case (int)GEnum.SystemCode.Deposit:
        //                                itmTranGrpKey = GFunc.NEInt(rowParent["ItmTranGrpKey"], 0);
        //                                break;
        //                        }
        //                        #endregion

        //                        #region get parent Job
        //                        switch (objDoc.DocCodeKey)
        //                        {
        //                            case (int)GEnum.SystemCode.Quotation:
        //                            case (int)GEnum.SystemCode.Sales_Order:
        //                            case (int)GEnum.SystemCode.Delivery_Order:
        //                            case (int)GEnum.SystemCode.Sales_Invoice:
        //                            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //                            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //                            case (int)GEnum.SystemCode.Cash_Sale:
        //                            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //                            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                            case (int)GEnum.SystemCode.Purchase_Order:
        //                            case (int)GEnum.SystemCode.Purchase_Delivery:
        //                            case (int)GEnum.SystemCode.Purchase_Invoice:
        //                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //                            case (int)GEnum.SystemCode.Issue_Consignment:
        //                            case (int)GEnum.SystemCode.Return_Consignment:
        //                            case (int)GEnum.SystemCode.Order_Consignment:
        //                            case (int)GEnum.SystemCode.Received_Consignment:
        //                            case (int)GEnum.SystemCode.Journal:
        //                                itmJobKey = GFunc.NEInt(rowParent["ItmJobKey"], 0);
        //                                itmJobPhaseKey = GFunc.NEInt(rowParent["ItmJobPhaseKey"], 0);
        //                                itmJobTaskKey = GFunc.NEInt(rowParent["ItmJobTaskKey"], 0);
        //                                itmJobCostTypeKey = GFunc.NEInt(rowParent["ItmJobCostTypeKey"], 0);
        //                                itmTranGrpKey = GFunc.NEInt(rowParent["ItmTranGrpKey"], 0);
        //                                break;
        //                        }
        //                        #endregion

        //                        #region update Child infor base on Parent Dept/TransGrp/Job values
        //                        dtItem.DefaultView.RowFilter = "LineLinkKey=" + rowParent["DocItmKey"];
        //                        foreach (DataRowView rowChild in dtItem.DefaultView)
        //                        {
        //                            //to apply filter on defaultView new row.
        //                            if (GFunc.NEInt(rowChild["LineLinkKey"], 0) != GFunc.NEInt(rowParent["DocItmKey"], 0))
        //                                continue;

        //                            switch (objDoc.DocCodeKey)
        //                            {
        //                                case (int)GEnum.SystemCode.Quotation:
        //                                case (int)GEnum.SystemCode.Sales_Order:
        //                                case (int)GEnum.SystemCode.Delivery_Order:
        //                                case (int)GEnum.SystemCode.Sales_Invoice:
        //                                case (int)GEnum.SystemCode.Sales_Debit_Note:
        //                                case (int)GEnum.SystemCode.Sales_Credit_Note:
        //                                case (int)GEnum.SystemCode.Cash_Sale:
        //                                case (int)GEnum.SystemCode.Cash_Debit_Note:
        //                                case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                                    rowChild["ItmDeptKey"] = itmDepKey;
        //                                    rowChild["ItmTranGrpKey"] = itmTranGrpKey;
        //                                    rowChild["ItmJobKey"] = itmJobKey;
        //                                    rowChild["ItmJobPhaseKey"] = itmJobPhaseKey;
        //                                    rowChild["ItmJobTaskKey"] = itmJobTaskKey;
        //                                    rowChild["ItmJobCostTypeKey"] = itmJobCostTypeKey;
        //                                    rowChild["ItmAccKey"] = 0;//Child for AR AccKey is always 0 as there are no posting, note: COS posting uses MSTItm.AccIN
        //                                    break;

        //                                case (int)GEnum.SystemCode.Purchase_Order:
        //                                case (int)GEnum.SystemCode.Purchase_Delivery:
        //                                case (int)GEnum.SystemCode.Purchase_Invoice:
        //                                case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //                                case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                                case (int)GEnum.SystemCode.Inventory_Adjustment:
        //                                    rowChild["ItmDeptKey"] = itmDepKey;
        //                                    rowChild["ItmTranGrpKey"] = itmTranGrpKey;
        //                                    rowChild["ItmJobKey"] = itmJobKey;
        //                                    rowChild["ItmJobPhaseKey"] = itmJobPhaseKey;
        //                                    rowChild["ItmJobTaskKey"] = itmJobTaskKey;
        //                                    rowChild["ItmJobCostTypeKey"] = itmJobCostTypeKey;
        //                                    rowChild["ItmAccKey"] = rowParent["ItmAccKey"];//for AP/IN, we need to ensure that the AccKey for Parent and Child Batch is the same (note: AP do not have Assembly child, only batch child)
        //                                    break;


        //                                case (int)GEnum.SystemCode.Issue_Consignment:
        //                                case (int)GEnum.SystemCode.Return_Consignment:
        //                                    rowChild["ItmDeptKey"] = itmDepKey;
        //                                    rowChild["ItmTranGrpKey"] = itmTranGrpKey;
        //                                    rowChild["ItmJobKey"] = itmJobKey;
        //                                    rowChild["ItmJobPhaseKey"] = itmJobPhaseKey;
        //                                    rowChild["ItmJobTaskKey"] = itmJobTaskKey;
        //                                    rowChild["ItmJobCostTypeKey"] = itmJobCostTypeKey;
        //                                    rowChild["ItmFromAccKey"] = rowParent["ItmFromAccKey"];//we need to ensure that the AccKey for Parent and Child Batch is the same
        //                                    rowChild["ItmToAccKey"] = rowParent["ItmToAccKey"];//we need to ensure that the AccKey for Parent and Child Batch is the same
        //                                    break;

        //                                case (int)GEnum.SystemCode.Journal:
        //                                    rowChild["ItmDeptKey"] = itmDepKey;
        //                                    rowChild["ItmTranGrpKey"] = itmTranGrpKey;
        //                                    rowChild["ItmJobKey"] = itmJobKey;
        //                                    rowChild["ItmJobPhaseKey"] = itmJobPhaseKey;
        //                                    rowChild["ItmJobTaskKey"] = itmJobTaskKey;
        //                                    rowChild["ItmJobCostTypeKey"] = itmJobCostTypeKey;
        //                                    break;

        //                                case (int)GEnum.SystemCode.Deposit:
        //                                    rowChild["ItmTranGrpKey"] = itmTranGrpKey;
        //                                    break;

        //                                case (int)GEnum.SystemCode.Purchase_Request:
        //                                case (int)GEnum.SystemCode.Inventory_Production:
        //                                case (int)GEnum.SystemCode.Purchase_Plan:
        //                                    rowChild["ItmDeptKey"] = itmDepKey;
        //                                    rowChild["ItmTranGrpKey"] = itmTranGrpKey;
        //                                    break;

        //                                case (int)GEnum.SystemCode.Inventory_Transfer:
        //                                    rowChild["ItmDeptKey"] = itmDepKey;
        //                                    rowChild["ItmTranGrpKey"] = itmTranGrpKey;
        //                                    rowChild["ItmFromAccKey"] = rowParent["ItmFromAccKey"];//we need to ensure that the AccKey for Parent and Child Batch is the same
        //                                    rowChild["ItmToAccKey"] = rowParent["ItmToAccKey"];//we need to ensure that the AccKey for Parent and Child Batch is the same
        //                                    break;

        //                            }
        //                        }
        //                        #endregion
        //                    }

        //                    dtItem.DefaultView.RowFilter = "";
        //                }
        //                #endregion

        //                break;
        //        }
        //        #endregion

        //        #region Set Detail Batch ItmCost to same as the parent for INADJ
        //        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Inventory_Adjustment)
        //        {

        //            decimal? itmCost = 0;

        //            dtParent.DefaultView.RowFilter = "ItmType IN(110,210,310,410) And LineLinkKey=0";//Batch                    

        //            foreach (DataRowView rowParent in dtParent.DefaultView)
        //            {
        //                itmCost = GFunc.NEDec(rowParent["ItmCost"], 0);

        //                //Child Itms
        //                DataRow[] dtChild = dtItem.Select("LineLinkKey= " + GFunc.NEInt(rowParent["DocItmKey"], -1));//we need to use -1 to prevent updating the parent row as parent row value is always 0

        //                //Set the Value
        //                foreach (DataRow rowChild in dtChild)
        //                {
        //                    rowChild["ItmCost"] = itmCost;
        //                    rowChild["ItmNewCost"] = 0M;    //For Batch, ItmNewCost is always 0
        //                }
        //            }
        //            dtItem.AcceptChanges();
        //        }
        //        #endregion

        //        #region Set Detail Batch ItmPrice to same as the parent for APBL/DN/CN
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:

        //                decimal? itmPrice = 0;
        //                decimal? itmAmtH = 0;

        //                foreach (DataRowView rowParent in dtParent.DefaultView)
        //                {
        //                    itmPrice = GFunc.NEDec(rowParent["ItmPrice"], 0);
        //                    itmAmtH = GFunc.NEDec(rowParent["ItmAmtH"], 0);

        //                    //Child Itms
        //                    DataRow[] dtChild = dtItem.Select("LineLinkKey= " + GFunc.NEInt(rowParent["DocItmKey"], 0));

        //                    //Set the Value
        //                    foreach (DataRow rowChild in dtChild)
        //                    {
        //                        rowChild["ItmPrice"] = itmPrice;
        //                        rowChild["ItmAmtH"] = itmAmtH;
        //                        rowChild["ItmQty"] = GFunc.NEDec(rowChild["ItmBatchQty"], 0);
        //                    }
        //                }
        //                dtItem.AcceptChanges();
        //                break;
        //        }
        //        #endregion

        //        #region Set Detail Batch ItmPrice,ItmAccKey  to same as the parent for ARIV/DN/CN
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                decimal? itmPrice = 0;
        //                decimal? itmAmtH = 0;
        //                decimal? itmVendorPriceRatio = 0;
        //                int ItmAccKey = 0;

        //                foreach (DataRowView rowParent in dtParent.DefaultView)
        //                {
        //                    itmPrice = GFunc.NEDec(rowParent["ItmPrice"], 0);
        //                    itmAmtH = GFunc.NEDec(rowParent["ItmAmtH"], 0);
        //                    itmVendorPriceRatio = GFunc.NEDec(rowParent["ItmVendorPriceRatio"], 0);

        //                    //Child Itms
        //                    DataRow[] dtChild = dtItem.Select("LineLinkKey= " + GFunc.NEInt(rowParent["DocItmKey"], 0));

        //                    //Set the Value
        //                    foreach (DataRow rowChild in dtChild)
        //                    {
        //                        switch ((int)rowChild["LineType"])
        //                        {
        //                            case (int)GEnum.RecDetailType.DItmAssembly:
        //                            case (int)GEnum.RecDetailType.DItmAssembly_Batch:
        //                            case (int)GEnum.RecDetailType.DItmAssembly_Batch_Serial:
        //                            case (int)GEnum.RecDetailType.DItmAssembly_Serial:
        //                                rowChild["ItmAccKey"] = 0;//Child for AR cannot have a acckey as it is never posted, for COS posting, the INAccKey is done in the COS Posting
        //                                break;

        //                            default:
        //                                rowChild["ItmPrice"] = itmPrice;
        //                                rowChild["ItmAmtH"] = itmAmtH;
        //                                rowChild["ItmVendorPriceRatio"] = itmVendorPriceRatio;
        //                                rowChild["ItmQty"] = GFunc.NEDec(rowChild["ItmBatchQty"], 0);
        //                                rowChild["ItmAccKey"] = 0;//Child for AR cannot have a acckey as it is never posted, for COS posting, the INAccKey is done in the COS Posting
        //                                break;
        //                        }
        //                    }
        //                }
        //                dtItem.AcceptChanges();
        //                break;
        //        }

        //        #endregion

        //        #region Set Detail Batch ItmPrice to same as the parent for APPD
        //        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Delivery)
        //        {
        //            decimal? itmPrice = 0;
        //            decimal? itmAmtH = 0;

        //            foreach (DataRowView rowParent in dtParent.DefaultView)
        //            {
        //                itmPrice = GFunc.NEDec(rowParent["ItmPrice"], 0);
        //                itmAmtH = GFunc.NEDec(rowParent["ItmAmtH"], 0);

        //                //Child Itms
        //                DataRow[] dtChild = dtItem.Select("LineLinkKey= " + GFunc.NEDec(rowParent["DocItmKey"], 0));

        //                //Set the Value
        //                foreach (DataRow rowChild in dtChild)
        //                {
        //                    rowChild["ItmPrice"] = itmPrice;
        //                    rowChild["ItmAmtH"] = itmAmtH;
        //                    rowChild["ItmQty"] = GFunc.NEDec(rowChild["ItmBatchQty"], 0);
        //                }
        //            }
        //            dtItem.AcceptChanges();
        //        }
        //        #endregion

        //        #region Set Detail Batch ItmVendorKey,ItmPrice,ItmAmtH to same as the parent for ARDO
        //        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Delivery_Order)
        //        {
        //            decimal? itmPrice = 0;
        //            decimal? itmVendorKey = 0;
        //            decimal? itmAmtH = 0;

        //            foreach (DataRowView rowParent in dtParent.DefaultView)
        //            {
        //                itmPrice = GFunc.NEDec(rowParent["ItmPrice"], 0);
        //                itmAmtH = GFunc.NEDec(rowParent["ItmAmtH"], 0);

        //                //Child Itms
        //                DataRow[] dtChild = dtItem.Select("LineLinkKey= " + GFunc.NEInt(rowParent["DocItmKey"], 0));

        //                //Set the Value
        //                foreach (DataRow rowChild in dtChild)
        //                {
        //                    switch ((int)rowChild["LineType"])
        //                    {
        //                        case (int)GEnum.RecDetailType.DItmAssembly:
        //                        case (int)GEnum.RecDetailType.DItmAssembly_Batch:
        //                        case (int)GEnum.RecDetailType.DItmAssembly_Batch_Serial:
        //                        case (int)GEnum.RecDetailType.DItmAssembly_Serial:
        //                            rowChild["ItmAccKey"] = 0;//Child for AR cannot have a acckey as it is never posted, for COS posting, the INAccKey is done in the COS Posting
        //                            break;

        //                        default:
        //                            rowChild["ItmPrice"] = itmPrice;
        //                            rowChild["ItmAmtH"] = itmAmtH;
        //                            rowChild["ItmQty"] = GFunc.NEDec(rowChild["ItmBatchQty"], 0);
        //                            rowChild["ItmAccKey"] = 0;//Child for AR cannot have a acckey as it is never posted, for COS posting, the INAccKey is done in the COS Posting
        //                            break;
        //                    }
        //                }
        //            }
        //            dtItem.AcceptChanges();
        //            dtItem.DefaultView.RowFilter = "";

        //        }
        //        #endregion

        //        #region set docgrpkey

        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Purchase_Request:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Received_Consignment:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Journal:
        //            case (int)GEnum.SystemCode.Deposit:
        //            case (int)GEnum.SystemCode.Bank_Revaluation:
        //            case (int)GEnum.SystemCode.Consignment_Settlement:
        //            case (int)GEnum.SystemCode.Packing_List:
        //                break;
        //            default:
        //                if (GFunc.IsNE(GFunc.GetPropertyValue("DocGrpKey", objDoc)))
        //                    GFunc.SetPropertyValue("DocGrpKey", objDoc, 0);
        //                break;
        //        }

        //        #endregion

        //        #region set allocatedate
        //        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Inventory_Production)
        //        {
        //            if (ButtonAction == (int)GEnum.DocAction.Post && objDoc.DocState != (int)GEnum.DocState.Posted)
        //            {
        //                if (GFunc.IsNE(GFunc.GetPropertyValue("DocAllocateDate", objDoc)))
        //                    GFunc.SetPropertyValue("DocAllocateDate", objDoc, objDoc.DocDate);

        //                if (MsgBox.Show(cn, MsgID.Document.DocDateChangeSystemDate, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
        //                    objDoc.DocDate = DateTime.Today;
        //            }
        //        }
        //        #endregion

        //        #region set countryrate

        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Received_Consignment:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Consignment_Settlement:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //                if (GFunc.IsNE(GFunc.GetDecimalPropertyValue("DocCountryRate", objDoc)))
        //                {
        //                    if (SysOptionUtility.CountryCurrency == 1)
        //                        GFunc.SetPropertyValue("DocCountryRate", objDoc, 1);
        //                    else
        //                    {
        //                        decimal? rate = DocComUtility.CountryRate_Get(cn, GFunc.GetIntPropertyValue("DocConKey", objDoc).Value, GFunc.GetIntPropertyValue("DocCurrKey", objDoc).Value, objDoc.DocDate.Value, true);
        //                        GFunc.SetPropertyValue("DocCountryRate", objDoc, rate);

        //                    }
        //                }
        //                break;

        //            case (int)GEnum.SystemCode.Deposit:
        //            case (int)GEnum.SystemCode.Bank_Revaluation:
        //                if (GFunc.IsNE(GFunc.GetDecimalPropertyValue("DocCountryRate", objDoc)))
        //                {
        //                    if (SysOptionUtility.CountryCurrency == 1)
        //                        GFunc.SetPropertyValue("DocCountryRate", objDoc, 1);
        //                    else
        //                    {
        //                        decimal? rate = DocComUtility.CurrRate_Get(cn, GFunc.GetIntPropertyValue("DocCurrKey", objDoc).Value, objDoc.DocDate.Value, true);
        //                        GFunc.SetPropertyValue("DocCountryRate", objDoc, rate);
        //                    }
        //                }
        //                break;
        //        }

        //        #endregion

        //        #region set landedCost Account
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                if (GFunc.IsNEZ(GFunc.GetIntPropertyValue("DocAddCostAccKey", objDoc)))
        //                {
        //                    int landedCostAccKey = SysOptionUtility.GetInt("AccLandedCost");
        //                    GFunc.SetPropertyValue("DocAddCostAccKey", objDoc, landedCostAccKey);
        //                }
        //                break;
        //        }
        //        #endregion

        //        #region set item lockey,ItmAddAmt,INADJ-ItmCost,MFN(gram produce/used, units produce/used, cost ratio)

        //        int Lockey = 0;
        //        int LocKeyFrom = 0;
        //        int LocKeyTo = 0;

        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //            case (int)GEnum.SystemCode.Received_Consignment:
        //                Lockey = GFunc.NEInt(objDoc.DefLocKey, 0);
        //                foreach (DataRowView row in dtItem.DefaultView)
        //                {
        //                    if (GFunc.GetINTypeGroup(row["ItmType"]) == (int)GEnum.INTypeGrp.Stock)
        //                    {
        //                        if (GFunc.IsNEZ(row["ItmLocKey"]))
        //                            row["ItmLocKey"] = Lockey;
        //                        break;
        //                    }
        //                }
        //                break;

        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //                Lockey = GFunc.NEInt(objDoc.DefLocKey, 0);
        //                foreach (DataRowView row in dtItem.DefaultView)
        //                {
        //                    if (GFunc.IsNEZ(row["ItmLocKey"]))
        //                        row["ItmLocKey"] = Lockey;

        //                    switch (GFunc.NEInt(row["ItmType"], 0))
        //                    {
        //                        case (int)GEnum.ItemType.StockB:
        //                        case (int)GEnum.ItemType.Finished_GDB:
        //                        case (int)GEnum.ItemType.Serial_StockB:
        //                        case (int)GEnum.ItemType.Serial_Finished_GDB:
        //                            if ((decimal)row["ItmQty"] <= 0)
        //                                row["ItmCost"] = 0;
        //                            break;
        //                    }
        //                }
        //                break;

        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //                LocKeyFrom = GFunc.NEInt(objDoc.DefFromLocKey, 0);
        //                LocKeyTo = GFunc.NEInt(objDoc.DefToLocKey, 0);
        //                foreach (DataRowView row in dtItem.DefaultView)
        //                {
        //                    if (GFunc.GetINTypeGroup(row["ItmType"]) == (int)GEnum.INTypeGrp.Stock)
        //                    {
        //                        if (GFunc.IsNEZ(row["ItmFromLocKey"]))
        //                            row["ItmFromLocKey"] = LocKeyFrom;
        //                        if (GFunc.IsNEZ(row["ItmToLocKey"]))
        //                            row["ItmToLocKey"] = LocKeyTo;
        //                    }
        //                }
        //                break;

        //            case (int)GEnum.SystemCode.Inventory_Production:
        //                decimal? TotalFGProduceQty = 0;
        //                decimal? TotalFGProduceGram = 0;
        //                decimal? GramRate = 0;
        //                int MFNCostModeUseGram = 0;
        //                int MFNCostModeUseQty = 0;

        //                #region Set Variable values use in Calculation
        //                //Get all UOM : use in calculating FGproduce gram produce and material gram used
        //                DataTable dtUOM = REFList.GetUOMs(cn, out msgID);
        //                if (dtUOM == null)
        //                    return false;
        //                else
        //                    //Set PrimaryKey to use Find function (In Runtime, Find funtion would raise error if you don't set primary Key)
        //                    dtUOM.PrimaryKey = new DataColumn[] { dtUOM.Columns["UOMKey"] };

        //                //Update finished goods Cost Ratio
        //                if (GFunc.GetIntPropertyValue("DocFGVarCostMode", objDoc) == 20)//By Weight
        //                {
        //                    //by weight
        //                    MFNCostModeUseGram = 1;
        //                    MFNCostModeUseQty = 0;
        //                }
        //                else
        //                {
        //                    //by units
        //                    MFNCostModeUseGram = 0;
        //                    MFNCostModeUseQty = 1;
        //                }
        //                #endregion

        //                #region Set Lockey, Finished goods gram produce, material gram used
        //                foreach (DataRowView row in dtItem.DefaultView)
        //                {
        //                    //Set Location                                       
        //                    if (GFunc.IsNEZ(row["ItmLocKey"]))
        //                        row["ItmLocKey"] = objDoc.DefToLocKey;
        //                    if (dtItem.Columns.Contains("LineType"))
        //                    {
        //                        //Set gram product for detail finished goods
        //                        if (GFunc.NEInt(row["LineType"], 0) == 3000)//Finished Goods
        //                        {
        //                            DataRow dr = dtUOM.Rows.Find(row["FGWeightUOMKey"]);
        //                            if (GFunc.IsNE(dr) || GFunc.IsNE(dr["GramRate"]))
        //                                GramRate = 1;
        //                            else
        //                                GramRate = GFunc.NEDec(dr["GramRate"], 0);

        //                            row["FGProduceWeight"] = GFunc.RndC(GFunc.NEDec(row["FGProduceQty"], 0) * GFunc.NEDec(row["FGWeight"], 0), GVar.RndDecs.Amtpt);
        //                            row["FGProduceGram"] = GFunc.RndC(GFunc.NEDec(row["FGProduceWeight"], 0) * GramRate, GVar.RndDecs.Amtpt);
        //                            row["FGOverHeadAmtH"] = GFunc.RndC(GFunc.NEDec(row["FGOverHeadCost"], 0) * GFunc.NEDec(row["FGProduceQty"], 0), GVar.RndDecs.Amtpt);

        //                            TotalFGProduceQty += GFunc.NEDec(row["FGProduceQty"], 0);
        //                            TotalFGProduceGram += GFunc.NEDec(row["FGProduceGram"], 0);
        //                        }

        //                        //Set gram used for detail material and packing
        //                        else if (GFunc.NEInt(row["LineType"], 0) >= 3100 && GFunc.NEInt(row["LineType"], 0) <= 3230)//Raw and Packing material
        //                        {
        //                            DataRow dr = dtUOM.Rows.Find(row["BOMWeightUOMKey"]);
        //                            if (GFunc.IsNE(dr) || GFunc.IsNE(dr["GramRate"]))
        //                                GramRate = 1;
        //                            else
        //                                GramRate = GFunc.NEDec(dr["GramRate"], 0);

        //                            row["BOMUsedGram"] = GFunc.RndC(GFunc.NEDec(row["BOMUsedWeight"], 0) * GramRate, GVar.RndDecs.Amtpt);
        //                        }
        //                    }
        //                }
        //                #endregion

        //                #region Calculate for Finished Goods Parent
        //                //set the finished goods cost ratio 
        //                //If Use gram to calculate cost ratio for each (finished goods gram produce / Total gram produce)
        //                //If Use units to calculate cost ratio for each (finished goods units produce / Total units produce)
        //                //([FGProduceGram]*[pUseGram]+[FGProduceQty]*[pUseQty])/([pTotalGram]*[pUseGram]+[pTotalQty]*[pUseQty])
        //                //Set for Parent Finished Goods
        //                dtItem.DefaultView.RowFilter = "LineType= 3000";//finished goods
        //                dtItem.DefaultView.Sort = "ItmSN";
        //                foreach (DataRowView dr in dtItem.DefaultView)
        //                {
        //                    if ((int)dr["LineType"] != 3000)
        //                        continue;

        //                    dr["FGCostRatio"] = GFunc.RndDC(GFunc.NEDec(dr["FGProduceGram"], 0) * MFNCostModeUseGram +
        //                        GFunc.NEDec(dr["FGProduceQty"], 0) * MFNCostModeUseQty,
        //                        TotalFGProduceGram * MFNCostModeUseGram + TotalFGProduceQty * MFNCostModeUseQty, GVar.RndDecs.COSpt);
        //                }
        //                dtItem.AcceptChanges();
        //                #endregion

        //                #region Calculate for Finished Goods Child
        //                //Setting Childs Finished Goods
        //                dtItem.DefaultView.RowFilter = "";
        //                dtParent = dtItem.DefaultView.ToTable();    //get a copy of the Caller datatable

        //                dtItem.DefaultView.RowFilter = "LineType> 3000 and LineType<=3030";//Child
        //                dtItem.DefaultView.Sort = "ItmSN";
        //                foreach (DataRowView dr in dtItem.DefaultView)
        //                {
        //                    if ((int)dr["LineType"] <= 3000 && (int)dr["LineType"] > 3030)
        //                        continue;

        //                    DataRow row = dtUOM.Rows.Find(dr["FGWeightUOMKey"]);
        //                    if (GFunc.IsNE(row) || GFunc.IsNE(row["GramRate"]))
        //                        GramRate = 1;
        //                    else
        //                        GramRate = GFunc.NEDec(row["GramRate"], 0);

        //                    dr["FGProduceQty"] = dr["ItmBatchQty"];
        //                    dr["FGProduceWeight"] = GFunc.RndC(GFunc.NEDec(dr["ItmBatchQty"], 0) * GFunc.NEDec(dr["FGWeight"], 0), GVar.RndDecs.Amtpt);
        //                    dr["FGProduceGram"] = GFunc.RndC(GFunc.NEDec(dr["FGProduceWeight"], 0) * GramRate, GVar.RndDecs.Amtpt);


        //                    dtParent.DefaultView.RowFilter = "DocItmKey=" + GFunc.NEInt(dr["LineLinkKey"], 0);
        //                    if (dtParent.DefaultView.Count > 0)
        //                    {
        //                        dr["FGOverHeadCost"] = dtParent.DefaultView[0]["FGOverHeadCost"];

        //                    }
        //                    dr["FGOverHeadAmtH"] = GFunc.RndC(GFunc.NEDec(dr["ItmBatchQty"], 0) * GFunc.NEDec(dr["FGOverHeadCost"], 0), GVar.RndDecs.Amtpt);
        //                    dr["FGCostRatio"] = GFunc.RndDC(GFunc.NEDec(dr["FGProduceGram"], 0) * MFNCostModeUseGram +
        //                        GFunc.NEDec(dr["FGProduceQty"], 0) * MFNCostModeUseQty,
        //                        TotalFGProduceGram * MFNCostModeUseGram + TotalFGProduceQty * MFNCostModeUseQty, GVar.RndDecs.COSpt);
        //                }
        //                dtItem.AcceptChanges();
        //                dtItem.DefaultView.RowFilter = "";
        //                #endregion

        //                break;
        //        }

        //        #endregion

        //        #region set applystatus

        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //                if ((GFunc.GetDecimalPropertyValue("DocGrand", objDoc) * objDoc.DocSign) - GFunc.GetDecimalPropertyValue("DocApplyAmtF", objDoc) == 0)
        //                    GFunc.SetPropertyValue("DocApplyFull", objDoc, true);
        //                else
        //                    GFunc.SetPropertyValue("DocApplyFull", objDoc, false);
        //                break;
        //        }

        //        #endregion

        //        #region Set NSLink

        //        string NSLinkStr = objDoc.DocCodeKey.ToString() + "-" + objDoc.DocKey.ToString();
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                foreach (DataRowView dr in dtItem.DefaultView)
        //                {
        //                    if (GFunc.IsNE(dr["NSLink"]) || dr["NSLink"].ToString() == "0")
        //                        dr["NSLink"] = NSLinkStr + "-" + dr["DocItmKey"].ToString();
        //                }
        //                dtItem.AcceptChanges();
        //                break;
        //        }
        //        #endregion

        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //private static bool HiddenValueDept_Set(SqlConnection cn, DataTable dt, int deptOpt, int deptKey, bool useItmType, int docCodeKey)
        //{
        //    int itmTypeGrp;
        //    bool exitLoop = false;
        //    try
        //    {
        //        foreach (DataRowView row in dt.DefaultView)
        //        {
        //            //set itmType
        //            if (useItmType)
        //                itmTypeGrp = GFunc.GetINTypeGroup(row["ItmType"]);
        //            else
        //                itmTypeGrp = docCodeKey;

        //            //set detailDept
        //            switch (itmTypeGrp)
        //            {
        //                #region set by itmtype
        //                case (int)GEnum.INTypeGrp.Empty:
        //                case (int)GEnum.INTypeGrp.Total:
        //                    row["ItmDeptKey"] = 0;
        //                    break;

        //                case (int)GEnum.INTypeGrp.Remark:
        //                    row["ItmDeptKey"] = GFunc.NEInt(row["ItmDeptKey"], 0);
        //                    break;

        //                case (int)GEnum.INTypeGrp.Stock:
        //                case (int)GEnum.INTypeGrp.Non_Stock:
        //                case (int)GEnum.INTypeGrp.Charges:
        //                case (int)GEnum.INTypeGrp.Discount:
        //                    if (GFunc.IsNEZ(row["ItmDeptKey"]))
        //                    {
        //                        switch (deptOpt)
        //                        {
        //                            case 10:    //dept >0
        //                                if (bRuningImport == false)
        //                                {
        //                                    MsgBox.Show(cn, "DetailLineNodepartment%" + row["ItmSN"].ToString());
        //                                    exitLoop = true;
        //                                }
        //                                break;

        //                            case 20:    //dept >0 but use DocDeptKey if possible
        //                                if (deptKey > 0)
        //                                    row["ItmDeptKey"] = deptKey;
        //                                else
        //                                {
        //                                    if (bRuningImport == false)
        //                                    {
        //                                        MsgBox.Show(cn, "DetailLineNodepartment%" + row["ItmSN"].ToString());
        //                                        exitLoop = true;
        //                                    }
        //                                }
        //                                break;

        //                            default:
        //                                row["ItmDeptKey"] = 0;
        //                                break;
        //                        }
        //                    }
        //                    break;
        //                #endregion

        //                #region set by doccode payment
        //                case (int)GEnum.SystemCode.Payment_Received:
        //                case (int)GEnum.SystemCode.Cash_Payment_Received:
        //                case (int)GEnum.SystemCode.Payment_Issue:
        //                    if (GFunc.IsNEZ(row["ExpDeptKey"]))
        //                    {
        //                        switch (deptOpt)
        //                        {
        //                            case 10:    //dept >0
        //                                MsgBox.Show(cn, "DetailLineNodepartment%" + row["ExpSN"].ToString());
        //                                exitLoop = true;
        //                                break;

        //                            case 20:    //dept >0 but use DocDeptKey if possible
        //                                if (deptKey > 0)
        //                                    row["ExpDeptKey"] = deptKey;
        //                                else
        //                                {
        //                                    MsgBox.Show(cn, "DetailLineNodepartment%" + row["ExpSN"].ToString());
        //                                    exitLoop = true;
        //                                }
        //                                break;
        //                            default:
        //                                row["ExpDeptKey"] = 0;
        //                                break;
        //                        }
        //                    }
        //                    break;
        //                #endregion

        //                #region set by doccode issue/return cosignment
        //                case (int)GEnum.SystemCode.Issue_Consignment:
        //                case (int)GEnum.SystemCode.Return_Consignment:
        //                    if (GFunc.IsNEZ(row["ExpDeptKey"]))
        //                    {
        //                        switch (deptOpt)
        //                        {
        //                            case 10:    //dept >0
        //                                MsgBox.Show(cn, "DetailLineNodepartment%" + row["ExpSN"].ToString());
        //                                exitLoop = true;
        //                                break;

        //                            case 20:    //dept >0 but use DocDeptKey if possible
        //                                if (deptKey > 0)
        //                                    row["ExpDeptKey"] = deptKey;
        //                                else
        //                                {
        //                                    MsgBox.Show(cn, "DetailLineNodepartment%" + row["ExpSN"].ToString());
        //                                    exitLoop = true;
        //                                }
        //                                break;
        //                            default:
        //                                row["ExpDeptKey"] = 0;
        //                                break;
        //                        }
        //                    }
        //                    break;

        //                #endregion

        //                #region set by doccode deposit
        //                case (int)GEnum.SystemCode.Deposit:
        //                    if (GFunc.IsNEZ(row["ItmDocDeptKey"]) && (GFunc.NEInt(row["ItmDocDC"], 0) == 0))
        //                    {
        //                        switch (deptOpt)
        //                        {
        //                            case 10:    //dept >0
        //                                MsgBox.Show(cn, "DetailLineNodepartment%" + row["ItmSN"].ToString());
        //                                exitLoop = true;
        //                                break;

        //                            case 20:    //dept >0 but use DocDeptKey if possible
        //                                if (deptKey > 0)
        //                                    row["ItmDocDeptKey"] = deptKey;
        //                                else
        //                                {
        //                                    MsgBox.Show(cn, "DetailLineNodepartment%" + row["ItmSN"].ToString());
        //                                    exitLoop = true;
        //                                }
        //                                break;
        //                            default:
        //                                row["ItmDocDeptKey"] = 0;
        //                                break;
        //                        }
        //                    }
        //                    break;
        //                #endregion

        //                #region set by doccode Journal
        //                case (int)GEnum.SystemCode.Journal:
        //                    if (GFunc.IsNEZ(row["ItmDeptKey"]))
        //                    {
        //                        switch (deptOpt)
        //                        {
        //                            case 10:    //dept >0
        //                                MsgBox.Show(cn, "DetailLineNodepartment%" + row["ItmSN"].ToString());
        //                                exitLoop = true;
        //                                break;

        //                            case 20:    //dept >0 but use DocDeptKey if possible
        //                                if (deptKey > 0)
        //                                    row["ItmDeptKey"] = deptKey;
        //                                else
        //                                {
        //                                    MsgBox.Show(cn, "DetailLineNodepartment%" + row["ItmSN"].ToString());
        //                                    exitLoop = true;
        //                                }
        //                                break;
        //                            default:
        //                                row["ItmDeptKey"] = 0;
        //                                break;
        //                        }
        //                    }
        //                    break;
        //                #endregion

        //                #region set by doccode packing List
        //                case (int)GEnum.SystemCode.Packing_List:
        //                    if (GFunc.IsNEZ(row["DetItmDeptKey"]))
        //                    {
        //                        switch (deptOpt)
        //                        {
        //                            case 10:    //dept >0
        //                                MsgBox.Show(cn, "DetailLineNodepartment%" + row["DetItmSN"].ToString());
        //                                exitLoop = true;
        //                                break;

        //                            case 20:    //dept >0 but use DocDeptKey if possible
        //                                if (deptKey > 0)
        //                                    row["DetItmDeptKey"] = deptKey;
        //                                else
        //                                {
        //                                    MsgBox.Show(cn, "DetailLineNodepartment%" + row["DetItmSN"].ToString());
        //                                    exitLoop = true;
        //                                }
        //                                break;
        //                            default:
        //                                row["DetItmDeptKey"] = 0;
        //                                break;
        //                        }
        //                    }
        //                    break;
        //                #endregion
        //            }

        //            if (exitLoop)
        //            {
        //                return false;
        //            }
        //        }
        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //public static bool HiddenValueCurrentRow_Set(SqlConnection cn, Document objDoc, int DetailType, DataRow dr)
        //{
        //    try
        //    {
        //        #region set departmentkey

        //        if (HiddenValueDeptCurrentRow_Set(cn, objDoc, DetailType, dr) == false)
        //            return true;

        //        #endregion

        //        #region set detail Job infor
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:

        //                #region set detail item Job infor
        //                if (DetailType == (int)GEnum.Details.Doc_Exp)
        //                {
        //                    dr["ExpJobKey"] = GFunc.NEInt(dr["ExpJobKey"], 0);
        //                    dr["ExpJobPhaseKey"] = GFunc.NEInt(dr["ExpJobPhaseKey"], 0);
        //                    dr["ExpJobTaskKey"] = GFunc.NEInt(dr["ExpJobTaskKey"], 0);
        //                    dr["ExpJobCostTypeKey"] = GFunc.NEInt(dr["ExpJobCostTypeKey"], 0);
        //                }
        //                break;
        //                #endregion

        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //            case (int)GEnum.SystemCode.Received_Consignment:
        //            case (int)GEnum.SystemCode.Journal:

        //                #region set detail item Job infor
        //                switch (objDoc.DocCodeKey)
        //                {
        //                    case (int)GEnum.SystemCode.Purchase_Order:
        //                    case (int)GEnum.SystemCode.Purchase_Delivery:
        //                    case (int)GEnum.SystemCode.Purchase_Invoice:
        //                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                    case (int)GEnum.SystemCode.Issue_Consignment:
        //                    case (int)GEnum.SystemCode.Return_Consignment:
        //                    case (int)GEnum.SystemCode.Order_Consignment:
        //                        if (GFunc.GetINTypeGroup(GFunc.NEInt(dr["ItmType"], 0)) == (int)GEnum.INTypeGrp.Stock)
        //                        {
        //                            dr["ItmJobKey"] = 0;
        //                            dr["ItmJobPhaseKey"] = 0;
        //                            dr["ItmJobTaskKey"] = 0;
        //                            dr["ItmJobCostTypeKey"] = 0;
        //                        }
        //                        else
        //                        {
        //                            dr["ItmJobKey"] = GFunc.NEInt(dr["ItmJobKey"], 0);
        //                            dr["ItmJobPhaseKey"] = GFunc.NEInt(dr["ItmJobPhaseKey"], 0);
        //                            dr["ItmJobTaskKey"] = GFunc.NEInt(dr["ItmJobTaskKey"], 0);
        //                            dr["ItmJobCostTypeKey"] = GFunc.NEInt(dr["ItmJobCostTypeKey"], 0);
        //                        }
        //                        break;

        //                    case (int)GEnum.SystemCode.Quotation:
        //                    case (int)GEnum.SystemCode.Sales_Order:
        //                    case (int)GEnum.SystemCode.Sales_Invoice:
        //                    case (int)GEnum.SystemCode.Delivery_Order:
        //                    case (int)GEnum.SystemCode.Inventory_Adjustment:
        //                    case (int)GEnum.SystemCode.Journal:
        //                        dr["ItmJobKey"] = GFunc.NEInt(dr["ItmJobKey"], 0);
        //                        dr["ItmJobPhaseKey"] = GFunc.NEInt(dr["ItmJobPhaseKey"], 0);
        //                        dr["ItmJobTaskKey"] = GFunc.NEInt(dr["ItmJobTaskKey"], 0);
        //                        dr["ItmJobCostTypeKey"] = GFunc.NEInt(dr["ItmJobCostTypeKey"], 0);
        //                        break;
        //                }
        //                break;
        //                #endregion
        //        }
        //        #endregion

        //        #region set item lockey,INADJ-ItmCost

        //        int Lockey = 0;
        //        int LocKeyFrom = 0;
        //        int LocKeyTo = 0;

        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //            case (int)GEnum.SystemCode.Received_Consignment:
        //                Lockey = GFunc.NEInt(objDoc.DefLocKey, 0);
        //                if (GFunc.GetINTypeGroup(dr["ItmType"]) == (int)GEnum.INTypeGrp.Stock)
        //                {
        //                    if (GFunc.IsNEZ(dr["ItmLocKey"]))
        //                        dr["ItmLocKey"] = Lockey;
        //                    break;
        //                }
        //                break;

        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //                Lockey = GFunc.NEInt(objDoc.DefLocKey, 0);
        //                if (GFunc.IsNEZ(dr["ItmLocKey"]))
        //                    dr["ItmLocKey"] = Lockey;

        //                switch (GFunc.NEInt(dr["ItmType"], 0))
        //                {
        //                    case (int)GEnum.ItemType.StockB:
        //                    case (int)GEnum.ItemType.Finished_GDB:
        //                    case (int)GEnum.ItemType.Serial_StockB:
        //                    case (int)GEnum.ItemType.Serial_Finished_GDB:
        //                        if ((decimal)dr["ItmQty"] <= 0)
        //                            dr["ItmCost"] = 0;
        //                        break;
        //                }
        //                break;

        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //                LocKeyFrom = GFunc.NEInt(objDoc.DefFromLocKey, 0);
        //                LocKeyTo = GFunc.NEInt(objDoc.DefToLocKey, 0);
        //                if (GFunc.GetINTypeGroup(dr["ItmType"]) == (int)GEnum.INTypeGrp.Stock)
        //                {
        //                    if (GFunc.IsNEZ(dr["ItmFromLocKey"]))
        //                        dr["ItmFromLocKey"] = LocKeyFrom;
        //                    if (GFunc.IsNEZ(dr["ItmToLocKey"]))
        //                        dr["ItmToLocKey"] = LocKeyTo;
        //                }
        //                break;

        //            case (int)GEnum.SystemCode.Inventory_Production:
        //                #region Set Lockey, Finished goods gram produce, material gram used
        //                //Set Location                                       
        //                if (GFunc.IsNEZ(dr["ItmLocKey"]))
        //                    dr["ItmLocKey"] = objDoc.DefToLocKey;

        //                break;
        //                #endregion
        //        }

        //        #endregion

        //        #region Set NSLink

        //        string NSLinkStr = objDoc.DocCodeKey.ToString() + "-" + objDoc.DocKey.ToString();
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                if (GFunc.IsNE(dr["NSLink"]) || dr["NSLink"].ToString() == "0")
        //                    dr["NSLink"] = NSLinkStr + "-" + dr["DocItmKey"].ToString();
        //                break;
        //        }
        //        #endregion

        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //private static bool HiddenValueDeptCurrentRow_Set(SqlConnection cn, Document objDoc, int detailType, DataRow dr)
        //{
        //    int docCodeKey = 0;
        //    bool useItmType = false;
        //    int itmTypeGrp;
        //    int deptKey = 0;
        //    int deptOpt = 0;
        //    bool skipProcess = false;
        //    try
        //    {
        //        #region Get process to run
        //        docCodeKey = (int)objDoc.DocCodeKey;

        //        switch (docCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //                if (detailType == (int)GEnum.Details.Doc_Itm)
        //                    useItmType = true;
        //                else
        //                    skipProcess = true;
        //                break;

        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                deptKey = GFunc.NEInt(GFunc.GetPropertyValue("DocDeptKey", objDoc), 0);
        //                deptOpt = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DepartmentOptionForAR, cn);
        //                useItmType = true;
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //                deptKey = GFunc.NEInt(GFunc.GetPropertyValue("DocDeptKey", objDoc), 0);
        //                deptOpt = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DepartmentOptionForAP, cn);
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                deptKey = GFunc.NEInt(GFunc.GetPropertyValue("DocDeptKey", objDoc), 0);
        //                deptOpt = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DepartmentOptionForAP, cn);
        //                useItmType = true;
        //                break;

        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //                deptKey = GFunc.NEInt(GFunc.GetPropertyValue("DocDeptKey", objDoc), 0);
        //                deptOpt = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DepartmentOptionForIN, cn);
        //                useItmType = true;
        //                break;

        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //                deptKey = GFunc.NEInt(GFunc.GetPropertyValue("DocDeptKey", objDoc), 0);
        //                deptOpt = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DepartmentOptionForAR, cn);
        //                if (detailType != (int)GEnum.Details.Doc_Exp)
        //                    skipProcess = true;
        //                break;

        //            case (int)GEnum.SystemCode.Payment_Issue:
        //                deptKey = GFunc.NEInt(GFunc.GetPropertyValue("DocDeptKey", objDoc), 0);
        //                deptOpt = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DepartmentOptionForAP, cn);
        //                if (detailType != (int)GEnum.Details.Doc_Exp)
        //                    skipProcess = true;
        //                break;

        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //                deptKey = (int)GFunc.NEInt(GFunc.GetPropertyValue("DocDeptKey", objDoc), 0);
        //                deptOpt = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DepartmentOptionForCSG, cn);
        //                if (detailType == (int)GEnum.Details.Doc_Itm)
        //                     useItmType = true;
        //                break;

        //            case (int)GEnum.SystemCode.Deposit:
        //                deptKey = (int)GFunc.NEInt(GFunc.GetPropertyValue("DocDeptKey", objDoc), 0);
        //                deptOpt = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DepartmentOptionForGL, cn);
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Request:
        //                deptKey = 0;
        //                deptOpt = 0;
        //                useItmType = true;
        //                break;

        //            case (int)GEnum.SystemCode.Journal:
        //                deptKey = 0;
        //                deptOpt = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DepartmentOptionForGL, cn);
        //                break;

        //            case (int)GEnum.SystemCode.Packing_List:
        //                deptKey = 0;
        //                deptOpt = 0;
        //                if (detailType != (int)GEnum.Details.Doc_Itm)
        //                    skipProcess = true;
        //                break;

        //            default:
        //                skipProcess = true;
        //                break;
        //        }

        //        if (useItmType)
        //            itmTypeGrp = GFunc.GetINTypeGroup(dr["ItmType"]);
        //        else
        //            itmTypeGrp = docCodeKey;

        //        #endregion

        //        //set detailDept
        //        if (skipProcess)
        //            return true;

        //        switch (itmTypeGrp)
        //        {
        //            #region set by itmtype
        //            case (int)GEnum.INTypeGrp.Empty:
        //            case (int)GEnum.INTypeGrp.Total:
        //                dr["ItmDeptKey"] = 0;
        //                break;

        //            case (int)GEnum.INTypeGrp.Remark:
        //                dr["ItmDeptKey"] = GFunc.NEInt(dr["ItmDeptKey"], 0);
        //                break;

        //            case (int)GEnum.INTypeGrp.Stock:
        //            case (int)GEnum.INTypeGrp.Non_Stock:
        //            case (int)GEnum.INTypeGrp.Charges:
        //            case (int)GEnum.INTypeGrp.Discount:
        //                if (GFunc.IsNEZ(dr["ItmDeptKey"]))
        //                {
        //                    switch (deptOpt)
        //                    {
        //                        case 10:    //dept >0
        //                            if (bRuningImport == false)
        //                            {
        //                                MsgBox.Show(cn, "DetailLineNodepartment%" + dr["ItmSN"].ToString());
        //                            }
        //                            break;

        //                        case 20:    //dept >0 but use DocDeptKey if possible
        //                            if (deptKey > 0)
        //                                dr["ItmDeptKey"] = deptKey;
        //                            else
        //                            {
        //                                if (bRuningImport == false)
        //                                {
        //                                    MsgBox.Show(cn, "DetailLineNodepartment%" + dr["ItmSN"].ToString());
        //                                }
        //                            }
        //                            break;

        //                        default:
        //                            dr["ItmDeptKey"] = 0;
        //                            break;
        //                    }
        //                }
        //                break;
        //            #endregion

        //            #region set by doccode payment
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //                if (GFunc.IsNEZ(dr["ExpDeptKey"]))
        //                {
        //                    switch (deptOpt)
        //                    {
        //                        case 10:    //dept >0
        //                            MsgBox.Show(cn, "DetailLineNodepartment%" + dr["ExpSN"].ToString());
        //                            break;

        //                        case 20:    //dept >0 but use DocDeptKey if possible
        //                            if (deptKey > 0)
        //                                dr["ExpDeptKey"] = deptKey;
        //                            else
        //                            {
        //                                MsgBox.Show(cn, "DetailLineNodepartment%" + dr["ExpSN"].ToString());
        //                            }
        //                            break;
        //                        default:
        //                            dr["ExpDeptKey"] = 0;
        //                            break;
        //                    }
        //                }
        //                break;
        //            #endregion

        //            #region set by doccode issue/return cosignment
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //                if (GFunc.IsNEZ(dr["ExpDeptKey"]))
        //                {
        //                    switch (deptOpt)
        //                    {
        //                        case 10:    //dept >0
        //                            MsgBox.Show(cn, "DetailLineNodepartment%" + dr["ExpSN"].ToString());
        //                            break;

        //                        case 20:    //dept >0 but use DocDeptKey if possible
        //                            if (deptKey > 0)
        //                                dr["ExpDeptKey"] = deptKey;
        //                            else
        //                            {
        //                                MsgBox.Show(cn, "DetailLineNodepartment%" + dr["ExpSN"].ToString());
        //                            }
        //                            break;
        //                        default:
        //                            dr["ExpDeptKey"] = 0;
        //                            break;
        //                    }
        //                }
        //                break;

        //            #endregion

        //            #region set by doccode deposit
        //            case (int)GEnum.SystemCode.Deposit:
        //                if (GFunc.IsNEZ(dr["ItmDocDeptKey"]) && (GFunc.NEInt(dr["ItmDocDC"], 0) == 0))
        //                {
        //                    switch (deptOpt)
        //                    {
        //                        case 10:    //dept >0
        //                            MsgBox.Show(cn, "DetailLineNodepartment%" + dr["ItmSN"].ToString());
        //                            break;

        //                        case 20:    //dept >0 but use DocDeptKey if possible
        //                            if (deptKey > 0)
        //                                dr["ItmDocDeptKey"] = deptKey;
        //                            else
        //                            {
        //                                MsgBox.Show(cn, "DetailLineNodepartment%" + dr["ItmSN"].ToString());
        //                            }
        //                            break;
        //                        default:
        //                            dr["ItmDocDeptKey"] = 0;
        //                            break;
        //                    }
        //                }
        //                break;
        //            #endregion

        //            #region set by doccode Journal
        //            case (int)GEnum.SystemCode.Journal:
        //                if (GFunc.IsNEZ(dr["ItmDeptKey"]))
        //                {
        //                    switch (deptOpt)
        //                    {
        //                        case 10:    //dept >0
        //                            MsgBox.Show(cn, "DetailLineNodepartment%" + dr["ItmSN"].ToString());
        //                            break;

        //                        case 20:    //dept >0 but use DocDeptKey if possible
        //                            if (deptKey > 0)
        //                                dr["ItmDeptKey"] = deptKey;
        //                            else
        //                            {
        //                                MsgBox.Show(cn, "DetailLineNodepartment%" + dr["ItmSN"].ToString());
        //                            }
        //                            break;
        //                        default:
        //                            dr["ItmDeptKey"] = 0;
        //                            break;
        //                    }
        //                }
        //                break;
        //            #endregion

        //            #region set by doccode packing List
        //            case (int)GEnum.SystemCode.Packing_List:
        //                if (GFunc.IsNEZ(dr["DetItmDeptKey"]))
        //                {
        //                    switch (deptOpt)
        //                    {
        //                        case 10:    //dept >0
        //                            MsgBox.Show(cn, "DetailLineNodepartment%" + dr["DetItmSN"].ToString());
        //                            break;

        //                        case 20:    //dept >0 but use DocDeptKey if possible
        //                            if (deptKey > 0)
        //                                dr["DetItmDeptKey"] = deptKey;
        //                            else
        //                            {
        //                                MsgBox.Show(cn, "DetailLineNodepartment%" + dr["DetItmSN"].ToString());
        //                            }
        //                            break;
        //                        default:
        //                            dr["DetItmDeptKey"] = 0;
        //                            break;
        //                    }
        //                }
        //                break;
        //            #endregion
        //        }

        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //private static bool DocDisDueDate_Set(Document objDoc)
        //{
        //    string OpValue = string.Empty;
        //    short? DueDay = 0;
        //    short? NextDue = 0;

        //    try
        //    {
        //        int? TermKey = GFunc.GetIntPropertyValue("DocTermKey", objDoc);

        //        //When no Term is define, use the document date as the Discount and Due Date
        //        if (GFunc.IsNEZ(TermKey))
        //        {
        //            GFunc.SetPropertyValue("DocDisDate", objDoc, objDoc.DocDate);
        //            GFunc.SetPropertyValue("DocDueDate", objDoc, objDoc.DocDate);
        //        }
        //        else
        //        {
        //            //Get the Discount and Due Date base on the TermKey and Document Date
        //            REFTerm objTerm = REFTerm.Get(TermKey);
        //            if (objTerm.StandTerm == true)
        //            {
        //                //Standard term
        //                GFunc.SetPropertyValue("DocDisDate", objDoc, objDoc.DocDate.Value.AddDays((double)objTerm.StandDisDay));
        //                GFunc.SetPropertyValue("DocDueDate", objDoc, objDoc.DocDate.Value.AddDays((double)objTerm.StandNetDueDay));
        //            }
        //            else
        //            {
        //                //Date Driven Term
        //                DueDay = objTerm.DateNetDueDay;
        //                NextDue = objTerm.DateDueDayNextMth;

        //                DateTime dDueDate = DocDueDate_Get(objDoc.DocDate.Value, DueDay.Value, NextDue.Value);

        //                GFunc.SetPropertyValue("DocDueDate", objDoc, dDueDate);
        //                GFunc.SetPropertyValue("DocDisDate", objDoc, dDueDate.AddDays(-1 * (double)objTerm.DateDisDay));
        //            }
        //        }
        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //private static bool DocDisDueDate_Set(SqlConnection cn, Document objDoc)
        //{
        //    string OpValue = string.Empty;
        //    short? DueDay = 0;
        //    short? NextDue = 0;

        //    try
        //    {
        //        int? TermKey = GFunc.GetIntPropertyValue("DocTermKey", objDoc);

        //        //When no Term is define, use the document date as the Discount and Due Date
        //        if (GFunc.IsNEZ(TermKey))
        //        {
        //            GFunc.SetPropertyValue("DocDisDate", objDoc, objDoc.DocDate);
        //            GFunc.SetPropertyValue("DocDueDate", objDoc, objDoc.DocDate);
        //        }
        //        else
        //        {
        //            //Get the Discount and Due Date base on the TermKey and Document Date
        //            REFTerm objTerm = REFTerm.Get(cn, TermKey);
        //            if (objTerm.StandTerm == true)
        //            {
        //                //Standard term
        //                GFunc.SetPropertyValue("DocDisDate", objDoc, objDoc.DocDate.Value.AddDays((double)objTerm.StandDisDay));
        //                GFunc.SetPropertyValue("DocDueDate", objDoc, objDoc.DocDate.Value.AddDays((double)objTerm.StandNetDueDay));
        //            }
        //            else
        //            {
        //                //Date Driven Term
        //                DueDay = objTerm.DateNetDueDay;
        //                NextDue = objTerm.DateDueDayNextMth;

        //                DateTime dDueDate = DocDueDate_Get(objDoc.DocDate.Value, DueDay.Value, NextDue.Value);

        //                GFunc.SetPropertyValue("DocDueDate", objDoc, dDueDate);
        //                GFunc.SetPropertyValue("DocDisDate", objDoc, dDueDate.AddDays(-1 * (double)objTerm.DateDisDay));
        //            }
        //        }
        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //private static DateTime DocDueDate_Get(DateTime invoiceDate, short DueDayOfMth, short DaysBefPostponeNextMth)
        //{
        //    DateTime postPoneDate = new DateTime();
        //    DateTime DueDateOfCurrMth = new DateTime();
        //    DateTime DueDateOfNextMth = new DateTime();

        //    DateTime LastDateOfCurrMth = new DateTime(invoiceDate.Year, invoiceDate.Month,/*Last Day*/ DateTime.DaysInMonth(invoiceDate.Year, invoiceDate.Month));
        //    DateTime LastDateOfNextMth = new DateTime(invoiceDate.AddMonths(1).Year, invoiceDate.AddMonths(1).Month,/*Last Day of Next Month*/ DateTime.DaysInMonth(invoiceDate.AddMonths(1).Year, invoiceDate.AddMonths(1).Month));

        //    try
        //    {
        //        if (DueDayOfMth <= LastDateOfCurrMth.Day)
        //        {
        //            DueDateOfCurrMth = new DateTime(invoiceDate.Year, invoiceDate.Month, DueDayOfMth);
        //            //DueDateOfCurrMth- DaysBefPostponeNextMth
        //            postPoneDate = DueDateOfCurrMth.AddDays(-1 * DaysBefPostponeNextMth);
        //        }
        //        else
        //        {
        //            DueDateOfCurrMth = new DateTime(invoiceDate.Year, invoiceDate.Month, LastDateOfCurrMth.Day);
        //            //LastDateOfCurrMth- DaysBefPostponeNextMth
        //            postPoneDate = LastDateOfCurrMth.AddDays(-1 * DaysBefPostponeNextMth);
        //        }

        //        if (DueDayOfMth <= LastDateOfNextMth.Day)
        //            DueDateOfNextMth = new DateTime(invoiceDate.AddMonths(1).Year, invoiceDate.AddMonths(1).Month, DueDayOfMth);
        //        else
        //            DueDateOfNextMth = new DateTime(invoiceDate.AddMonths(1).Year, invoiceDate.AddMonths(1).Month, LastDateOfNextMth.Day);

        //        if (invoiceDate < postPoneDate)
        //            return DueDateOfCurrMth;
        //        else
        //            return DueDateOfNextMth;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //private static bool Doc_Disallow_Check(SqlConnection cn, Document objDoc, int ButtonAction)
        //{
        //    //Check for condition where saving or delete is disallowed
        //    try
        //    {
        //        DataTable dtResult = null;
        //        SqlParameter para = null;
        //        int linkFound = 0;

        //        #region Check for disallow condition by DocCodeKey
        //        switch (objDoc.DocCodeKey)
        //        {
        //            //case (int)GEnum.SystemCode.Delivery_Order:
        //            //    if (objDoc.DocState == (int)GEnum.DocState.Invoiced)
        //            //    {
        //            //        if (ButtonAction == (int)GEnum.DocAction.Delete)
        //            //        {
        //            //            if (MsgBox.Show(cn, MsgID.Document.InvoiceWarnDeleteDO + "%" + objDoc.DocID, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
        //            //                return false;
        //            //        }
        //            //        else
        //            //        {
        //            //            MsgBox.Show("Delivery Order has been invoiced, cannot re-post");
        //            //            return false;
        //            //        }
        //            //    }
        //            //    break;

        //            //case (int)GEnum.SystemCode.Sales_Invoice:
        //            //case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            //case (int)GEnum.SystemCode.Cash_Sale:
        //            //case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            //case (int)GEnum.SystemCode.Purchase_Invoice:
        //            //case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            //case (int)GEnum.SystemCode.Sales_Adjustment:
        //            //case (int)GEnum.SystemCode.Cash_Adjustment:
        //            //case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            //    if (GFunc.GetDecimalPropertyValue("DocApplyAmtF", objDoc) != 0)
        //            //    {
        //            //        if (ButtonAction == (int)GEnum.DocAction.Delete)
        //            //        {
        //            //            MsgBox.Show(cn, MsgID.Document.AppliedDocCannotBeDeleted);
        //            //            return false;
        //            //        }
        //            //        else
        //            //        {
        //            //            MsgBox.Show(cn, "Document has been applied, cannot re-post");
        //            //            return false;
        //            //        }
        //            //    }
        //            //    break;

        //            //case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            //case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            //case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            //    if (GFunc.GetIntPropertyValue("DocApplyIVDK", objDoc) == 0 && GFunc.GetDecimalPropertyValue("DocApplyAmtF", objDoc) != 0)
        //            //    {
        //            //        if (ButtonAction == (int)GEnum.DocAction.Delete)
        //            //        {
        //            //            MsgBox.Show(cn, MsgID.Document.AppliedDocCannotBeDeleted);
        //            //            return false;
        //            //        }
        //            //        else
        //            //        {
        //            //            MsgBox.Show(cn, "Document has been applied, cannot re-post");
        //            //            return false;
        //            //        }
        //            //    }
        //            //    break;

        //            //case (int)GEnum.SystemCode.Purchase_Delivery:
        //            //    if (objDoc.DocState == (int)GEnum.DocState.Invoiced)
        //            //    {
        //            //        if (ButtonAction == (int)GEnum.DocAction.Delete)
        //            //        {
        //            //            MsgBox.Show(cn, MsgID.Document.InvoicedDocCannotBeDeleted);
        //            //            return false;
        //            //        }
        //            //        else
        //            //        {
        //            //            MsgBox.Show(cn, "Document has been applied, cannot re-post");
        //            //            return false;
        //            //        }
        //            //    }
        //            //    break;

        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //                if (GFunc.NEBool(GFunc.GetPropertyValue("DocDeposit", objDoc), false))
        //                {
        //                    if (ButtonAction == (int)GEnum.DocAction.Delete)
        //                    {
        //                        MsgBox.Show(cn, "Document has been deposited, cannot delete");
        //                        return false;
        //                    }
        //                    else
        //                    {
        //                        MsgBox.Show(cn, "Document has been deposited, cannot re-post");
        //                        return false;
        //                    }
        //                }
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //                if (objDoc.DocState == (int)GEnum.DocState.Posted)
        //                {
        //                    if (ButtonAction == (int)GEnum.DocAction.Delete)
        //                    {
        //                        if (MsgBox.Show(cn, MsgID.Document.WarnPostedDocDelete, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
        //                            return false;
        //                    }
        //                    else
        //                    {
        //                        MsgBox.Show(cn, "Document has been posted, cannot re-post");
        //                        return false;
        //                    }
        //                }
        //                break;
        //        }
        //        #endregion

        //        //Check for disallow delete condition (Use in template, To Do and Document link)
        //        if (ButtonAction == (int)GEnum.DocAction.Delete)
        //        {
        //            #region Check for document that is use in document template

        //            List<SqlParameter> paraListA = new List<SqlParameter>();
        //            paraListA.Add(new SqlParameter("@DocCodeKey", objDoc.DocCodeKey));
        //            paraListA.Add(new SqlParameter("@DocKey", objDoc.DocKey));
        //            para = new SqlParameter("@LinkFound", 0);
        //            para.Direction = ParameterDirection.Output;
        //            paraListA.Add(para);

        //            dtResult = GFunc.ExecuteProc(cn, "Doc_CheckUseInTemplate", paraListA);
        //            linkFound = GFunc.NEInt(para.Value, 0);

        //            switch (linkFound)
        //            {
        //                case 1:
        //                    MsgBoxGrid.Show(cn, MsgID.Document.DocUseInTemplate, dtResult,GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
        //                    return false;

        //                case 2:
        //                    MsgBoxGrid.Show(cn, "Use in To Do, cannot delete", dtResult, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
        //                    return false;
        //            }
        //            #endregion
        //        }

        //        #region Check for document link

        //        #region Get DocApplyAmtF and DocApplyIVDK Property
        //        decimal pDocApplyAmtF = 0M;
        //        int pDocApplyIVDK = 0;

        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                pDocApplyAmtF = GFunc.NEDec(GFunc.GetDecimalPropertyValue("DocApplyAmtF", objDoc), 0M);
        //                pDocApplyIVDK = GFunc.NEInt(GFunc.GetIntPropertyValue("DocApplyIVDK", objDoc), 0);
        //                break;

        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //                pDocApplyAmtF = GFunc.NEDec(GFunc.GetDecimalPropertyValue("DocApplyAmtF", objDoc), 0M);
        //                break;
        //        }

        //        #endregion

        //        List<SqlParameter> paraListB = new List<SqlParameter>();
        //        paraListB.Add(new SqlParameter("@DocCodeKey", objDoc.DocCodeKey));
        //        paraListB.Add(new SqlParameter("@DocKey", objDoc.DocKey));
        //        paraListB.Add(new SqlParameter("@ButtonAction", ButtonAction));
        //        paraListB.Add(new SqlParameter("@DocState", objDoc.DocState));
        //        paraListB.Add(new SqlParameter("@DocApplyAmtF", pDocApplyAmtF));
        //        paraListB.Add(new SqlParameter("@DocApplyIVDK", pDocApplyIVDK));
        //        para = new SqlParameter("@LinkFound", 0);
        //        para.Direction = ParameterDirection.Output;
        //        paraListB.Add(para);

        //        dtResult = GFunc.ExecuteProc(cn, "Doc_CheckLink", paraListB);
        //        linkFound = GFunc.NEInt(para.Value, 0);
        //        string msgID = string.Empty;

        //        if (linkFound > 0 && linkFound < 100) //Critical Error
        //        {
        //            #region Critical Error
        //            switch (linkFound)
        //            {
        //                case 1:
        //                    msgID = MsgID.Document.CannotEditDocApplied;
        //                    break;

        //                case 2:
        //                    msgID = MsgID.Document.CannotDeleteDocApplied;
        //                    break;

        //                case 3:
        //                    msgID = MsgID.Document.InvoicedDocCannotBeSave;
        //                    break;

        //                case 4:
        //                    msgID = MsgID.Document.InvoicedDocCannotBeDeleted;
        //                    break;

        //                case 5:
        //                    msgID = MsgID.Document.CannotSaveDocHasLinked;
        //                    break;

        //                case 6:
        //                    msgID = MsgID.Document.CannotDeleteDocHasLinked;
        //                    break;

        //                default:
        //                    msgID = "Process cannot continue, document is linked";
        //                    break;
        //            }
        //            MsgBoxGrid.Show(cn, msgID, dtResult, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
        //            return false;
        //            #endregion
        //        }
        //        else if (linkFound > 100)// --User is allowed to continue
        //        {
        //            #region non critical Error where user can choose to continue
        //            switch (linkFound)
        //            {
        //                case 100:
        //                    msgID = MsgID.Document.WarnDocIsInvoiced;
        //                    break;

        //                case 110:
        //                    msgID = MsgID.Document.WarnDocIsLink;
        //                    break;

        //                default:
        //                    msgID = "Continue with process?";
        //                    break;
        //            }
        //            if (MsgBoxGrid.Show(cn, msgID,  dtResult, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
        //            {
        //                return false;
        //            }
        //            #endregion
        //        }
        //        #endregion

        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //private static bool Doc_PeriodClose_Check(SqlConnection cn, Document objDoc, int ButtonAction, DataTable dtSvrData)
        //{
        //    bool runCheckType = true;
        //    bool runCheckPeriod = true;
        //    int? SvrDocPeriod = 0;
        //    DateTime SvrDocDate = Convert.ToDateTime("1/1/1900");

        //    string CheckType = string.Empty;
        //    DateTime CheckDate = new DateTime();
        //    int? CheckPeriod = 0;

        //    try
        //    {

        //        #region set process to run
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Sales_Order:
        //                if ((bool)SysOptionUtility.GetBool("IgnorePeriodLockForARSO", cn))
        //                {
        //                    return true;
        //                }
        //                break;

        //            case (int)GEnum.SystemCode.Quotation:
        //                if ((bool)SysOptionUtility.GetBool("IgnorePeriodLockForARQO", cn))
        //                {
        //                    return true;
        //                }
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Order:
        //                if ((bool)SysOptionUtility.GetBool("IgnorePeriodLockForAPPO", cn))
        //                {
        //                    return true;
        //                }
        //                break;
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //                if ((bool)SysOptionUtility.GetBool("IgnorePeriodLockForCSPO", cn))
        //                {
        //                    return true;
        //                }
        //                break;

        //            case (int)GEnum.SystemCode.Packing_List:
        //                if ((bool)SysOptionUtility.GetBool("IgnorePeriodLockForARPL", cn))
        //                {
        //                    return true;
        //                }
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //                return true;

        //            case (int)GEnum.SystemCode.Inventory_Production:
        //                if (objDoc.DocState == (int)GEnum.DocState.Posted)
        //                {
        //                    if (ButtonAction == (int)GEnum.DocAction.Delete)
        //                        CheckType = GVar.UpdateType.Svr;
        //                    else
        //                        CheckType = GVar.UpdateType.All;

        //                    runCheckType = false;
        //                    runCheckPeriod = true;
        //                }
        //                else
        //                {
        //                    runCheckType = false;
        //                    runCheckPeriod = false;
        //                }
        //                break;
        //        }
        //        #endregion

        //        if (runCheckType)
        //        {
        //            #region get CheckType
        //            if (objDoc.DocState == (int)GEnum.DocState.New)
        //                CheckType = GVar.UpdateType.Obj;
        //            else
        //            {
        //                if (ButtonAction == (int)GEnum.DocAction.Delete)
        //                    CheckType = GVar.UpdateType.Svr;
        //                else
        //                    CheckType = GVar.UpdateType.All;
        //            }
        //            #endregion
        //        }

        //        if (runCheckPeriod)
        //        {
        //            #region get Svr DocDate and DocPeriod
        //            if (GFunc.CompareString(CheckType, GVar.UpdateType.All) || GFunc.CompareString(CheckType, GVar.UpdateType.Svr))
        //            {
        //                if (dtSvrData.Rows.Count > 0)
        //                {
        //                    SvrDocPeriod = Convert.ToInt32(GFunc.NEDateTime(dtSvrData.Rows[0]["DocDate"], DateTime.Today).ToString("yyyyMM"));
        //                    SvrDocDate = Convert.ToDateTime(dtSvrData.Rows[0]["DocDate"]);
        //                }
        //            }
        //            #endregion

        //            #region Set CheckDate/CheckPeriod
        //            switch (CheckType)
        //            {
        //                case GVar.UpdateType.Svr:
        //                    CheckDate = (DateTime)SvrDocDate;
        //                    CheckPeriod = SvrDocPeriod;
        //                    break;

        //                case GVar.UpdateType.Obj:
        //                    CheckDate = objDoc.DocDate.Value;
        //                    CheckPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //                    break;

        //                case GVar.UpdateType.All:
        //                    if (GFunc.IsNE(SvrDocDate))
        //                    {
        //                        CheckDate = objDoc.DocDate.Value;
        //                        CheckPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //                    }
        //                    else
        //                    {
        //                        if (SvrDocDate < objDoc.DocDate)
        //                        {
        //                            CheckDate = SvrDocDate;
        //                            CheckPeriod = SvrDocPeriod;
        //                        }
        //                        else
        //                        {
        //                            CheckDate = objDoc.DocDate.Value;
        //                            CheckPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //                        }
        //                    }
        //                    break;
        //            }
        //            #endregion

        //            #region Check for posting date within accounting period that is open
        //            if (GFunc.DocPeriod_Get(cn, CheckDate, ref CheckPeriod) == false)
        //            {
        //                MsgBox.Show(cn, MsgID.Document.PeriodIsClosed + "%" + CheckDate.ToString("dd MMM yyyy"));
        //                return false;
        //            }
        //            #endregion

        //            #region Check for ARAPRevaluePeriod
        //            switch (objDoc.DocCodeKey)
        //            {
        //                case (int)GEnum.SystemCode.Sales_Invoice:
        //                case (int)GEnum.SystemCode.Sales_Credit_Note:
        //                case (int)GEnum.SystemCode.Sales_Debit_Note:
        //                case (int)GEnum.SystemCode.Cash_Sale:
        //                case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                case (int)GEnum.SystemCode.Cash_Debit_Note:
        //                case (int)GEnum.SystemCode.Payment_Received:
        //                case (int)GEnum.SystemCode.Cash_Payment_Received:
        //                case (int)GEnum.SystemCode.Sales_Adjustment:
        //                case (int)GEnum.SystemCode.Cash_Adjustment:
        //                case (int)GEnum.SystemCode.Purchase_Invoice:
        //                case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //                case (int)GEnum.SystemCode.Payment_Issue:
        //                case (int)GEnum.SystemCode.Contra:
        //                case (int)GEnum.SystemCode.Cash_Contra:
        //                    int? lastRevARAP = SysOptionUtility.GetInt("LastARAPRevaluationPeriod", cn);
        //                    if (GFunc.IsNEZ(lastRevARAP) == false)
        //                    {
        //                        if (lastRevARAP >= CheckPeriod)
        //                        {
        //                            MsgBox.Show(cn, MsgID.Document.PostPeriodFallInRevaluationPeriod + "%" + lastRevARAP);
        //                            return false;
        //                        }
        //                    }
        //                    break;
        //            }
        //            #endregion
        //        }
        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //private static bool Doc_SaleControlPrice_Check(SqlConnection cn, Document objDoc, DataTable dtItems, int ButtonAction, bool cancelPopup)
        //{
        //    bool runProcess = false;
        //    int SalePriceCtrlOption = 0;

        //    DataTable dtSaleControlPrice = new DataTable();
        //    dtSaleControlPrice.Columns.Add("ItmSN", typeof(int));
        //    dtSaleControlPrice.Columns.Add("ItmType", typeof(int));
        //    dtSaleControlPrice.Columns.Add("ItmDes", typeof(string));
        //    dtSaleControlPrice.Columns.Add("ItmQty", typeof(int));
        //    dtSaleControlPrice.Columns.Add("ItmUOMID", typeof(string));
        //    dtSaleControlPrice.Columns.Add("ItmPrice", typeof(decimal));
        //    dtSaleControlPrice.Columns.Add("ItmControlPrice", typeof(decimal));

        //    try
        //    {
        //        if (!cancelPopup)
        //        {
        //            string ItmTypeFilter = "ItmType in (" + (int)GEnum.ItemType.Stock + "," +
        //                                                    (int)GEnum.ItemType.StockB + "," +
        //                                                    (int)GEnum.ItemType.Finished_GD + "," +
        //                                                    (int)GEnum.ItemType.Finished_GDB + "," +
        //                                                    (int)GEnum.ItemType.Serial_StockB + "," +
        //                                                    (int)GEnum.ItemType.Serial_Finished_GDB + "," +
        //                                                    (int)GEnum.ItemType.Consignment + "," +
        //                                                    (int)GEnum.ItemType.Assembly + "," +
        //                                                    (int)GEnum.ItemType.Non_Stock + "," +
        //                                                    (int)GEnum.ItemType.Service + "," +
        //                                                    (int)GEnum.ItemType.Charges + ")";
        //            #region set process to run and filter
        //            DataTable dt = null;
        //            string vFilter=string.Empty;
        //            switch (objDoc.DocCodeKey)
        //            {
        //                case (int)GEnum.SystemCode.Quotation:
        //                case (int)GEnum.SystemCode.Sales_Order:
        //                case (int)GEnum.SystemCode.Delivery_Order:
        //                case (int)GEnum.SystemCode.Sales_Invoice:
        //                case (int)GEnum.SystemCode.Sales_Debit_Note:
        //                case (int)GEnum.SystemCode.Sales_Credit_Note:
        //                case (int)GEnum.SystemCode.Cash_Sale:
        //                case (int)GEnum.SystemCode.Cash_Debit_Note:
        //                case (int)GEnum.SystemCode.Cash_Credit_Note: 
        //                    vFilter = "ItmControlPrice>ItmPrice And LineType=1000 And " + ItmTypeFilter;
        //                    dt = dtItems.Select(vFilter).CopyToDataTable();
        //                    runProcess = true;
        //                    break;

        //                case (int)GEnum.SystemCode.Issue_Consignment:
        //                    vFilter ="ItmControlPrice>ItmPrice And " + ItmTypeFilter;
        //                    dt = dtItems.Select(vFilter).CopyToDataTable();
        //                    runProcess = true;
        //                    break;
        //            }
        //            #endregion

        //            if (runProcess)
        //            {
        //                //Check for exceed sale control
        //                //15=Control Sale Price with Minimum Sale Price ,25=Control Sale Price with Latest Cost,35=Control Sale Price with Average Cost
        //                SalePriceCtrlOption = SysOptionUtility.GetInt("SalesPriceControl", cn);
        //                if (SalePriceCtrlOption == 15 || SalePriceCtrlOption == 25 || SalePriceCtrlOption == 35)
        //                {
        //                    #region get list of detail with price < controlprice
        //                    DataRow drSaleControl;
        //                    foreach (DataRow dr in dt.Rows)
        //                    {
        //                        drSaleControl = dtSaleControlPrice.NewRow();
        //                        drSaleControl["ItmSN"] = GFunc.NEDec(dr["ItmSN"], 0);
        //                        drSaleControl["ItmType"] = GFunc.NEInt(dr["ItmType"], 0);
        //                        drSaleControl["ItmDes"] = GFunc.NEStr(dr["ItmDes"], string.Empty);
        //                        drSaleControl["ItmQty"] = GFunc.NEDec(dr["ItmQty"], 0);

        //                        int? uomKey = GFunc.NEInt(dr["ItmUOMKey"], 0);
        //                        REFUOM uom = REFUOM.Get(cn, uomKey);
        //                        drSaleControl["ItmUOMID"] = uom.UOMID;

        //                        drSaleControl["ItmPrice"] = dr["ItmPrice"];
        //                        drSaleControl["ItmControlPrice"] = dr["ItmControlPrice"];
        //                        dtSaleControlPrice.Rows.Add(drSaleControl);
        //                    }
        //                    #endregion

        //                    #region open controlprice popup form in dialog
        //                    if (dt.Rows.Count > 0)
        //                    {
        //                        //Allow user to override the control price                    

        //                        Assembly oAName = Assembly.LoadFile(Application.ExecutablePath.ToString());
        //                        Type typ = oAName.GetType("WinUI.frmSaleControlPriceCheck");

        //                        int docCodeKey = GFunc.NEInt(objDoc.DocCodeKey, 0);
        //                        object calcInstance = Activator.CreateInstance(typ, new object[] { cn, docCodeKey, dtSaleControlPrice });

        //                        if (((Form)calcInstance).ShowDialog() != System.Windows.Forms.DialogResult.OK)
        //                        {
        //                            MsgBox.Show(cn, MsgID.Document.ItemsBelowSaleLimit);
        //                            return false;
        //                        }

        //                    }
        //                    #endregion
        //                }
        //            }

        //        }
        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
          
        //}//Completed
        //private static bool DocAuthorisation_Get(SqlConnection cn, Document objDoc, int ButtonAction, ref bool ApproveReq, ref bool Authorised)
        //{
        //    ApproveReq = false;
        //    Authorised = false;
        //    int DocApprove = 0;
        //    decimal? Limit1 = 0;
        //    decimal? Limit2 = 0;
        //    decimal? Limit3 = 0;
        //    decimal? Limit4 = 0;
        //    decimal? Limit5 = 0;
        //    string LimitPerm1 = string.Empty;
        //    string LimitPerm2 = string.Empty;
        //    string LimitPerm3 = string.Empty;
        //    string LimitPerm4 = string.Empty;
        //    string LimitPerm5 = string.Empty;
        //    string LimitPermUnlimited = string.Empty;
        //    decimal? CheckAmtH = 0;

        //    try
        //    {
        //        #region Get Approval Option and approval limit
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //                DocApprove = SysOptionUtility.GetInt("DocApproveForARQO", cn);
        //                if (DocApprove > 0) ApproveReq = true;
        //                if (ApproveReq)
        //                {
        //                    Limit1 = SysOptionUtility.GetDec("DocApproveForARQOLimit1", cn);
        //                    Limit2 = SysOptionUtility.GetDec("DocApproveForARQOLimit2", cn);
        //                    Limit3 = SysOptionUtility.GetDec("DocApproveForARQOLimit3", cn);
        //                    Limit4 = SysOptionUtility.GetDec("DocApproveForARQOLimit4", cn);
        //                    Limit5 = SysOptionUtility.GetDec("DocApproveForARQOLimit5", cn);

        //                    LimitPerm1 = GVar.PermissionID.ARQOApproveLimit1;
        //                    LimitPerm2 = GVar.PermissionID.ARQOApproveLimit2;
        //                    LimitPerm3 = GVar.PermissionID.ARQOApproveLimit3;
        //                    LimitPerm4 = GVar.PermissionID.ARQOApproveLimit4;
        //                    LimitPerm5 = GVar.PermissionID.ARQOApproveLimit5;
        //                    LimitPermUnlimited = GVar.PermissionID.ARQOApproveNoLimit;
        //                }
        //                break;

        //            case (int)GEnum.SystemCode.Sales_Order:

        //                DocApprove = SysOptionUtility.GetInt("DocApproveForARSO", cn);
        //                if (DocApprove > 0) ApproveReq = true;
        //                if (ApproveReq)
        //                {
        //                    Limit1 = SysOptionUtility.GetDec("DocApproveForARSOLimit1", cn);
        //                    Limit2 = SysOptionUtility.GetDec("DocApproveForARSOLimit2", cn);
        //                    Limit3 = SysOptionUtility.GetDec("DocApproveForARSOLimit3", cn);
        //                    Limit4 = SysOptionUtility.GetDec("DocApproveForARSOLimit4", cn);
        //                    Limit5 = SysOptionUtility.GetDec("DocApproveForARSOLimit5", cn);

        //                    LimitPerm1 = GVar.PermissionID.ARSOApproveLimit1;
        //                    LimitPerm2 = GVar.PermissionID.ARSOApproveLimit2;
        //                    LimitPerm3 = GVar.PermissionID.ARSOApproveLimit3;
        //                    LimitPerm4 = GVar.PermissionID.ARSOApproveLimit4;
        //                    LimitPerm5 = GVar.PermissionID.ARSOApproveLimit5;
        //                    LimitPermUnlimited = GVar.PermissionID.ARSOApproveNoLimit;
        //                }
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Order:
        //                DocApprove = SysOptionUtility.GetInt("DocApproveForAPPO", cn);
        //                if (DocApprove > 0) ApproveReq = true;
        //                if (ApproveReq)
        //                {
        //                    Limit1 = SysOptionUtility.GetDec("DocApproveForAPPOLimit1", cn);
        //                    Limit2 = SysOptionUtility.GetDec("DocApproveForAPPOLimit2", cn);
        //                    Limit3 = SysOptionUtility.GetDec("DocApproveForAPPOLimit3", cn);
        //                    Limit4 = SysOptionUtility.GetDec("DocApproveForAPPOLimit4", cn);
        //                    Limit5 = SysOptionUtility.GetDec("DocApproveForAPPOLimit5", cn);

        //                    LimitPerm1 = GVar.PermissionID.APPOApproveLimit1;
        //                    LimitPerm2 = GVar.PermissionID.APPOApproveLimit2;
        //                    LimitPerm3 = GVar.PermissionID.APPOApproveLimit3;
        //                    LimitPerm4 = GVar.PermissionID.APPOApproveLimit4;
        //                    LimitPerm5 = GVar.PermissionID.APPOApproveLimit5;
        //                    LimitPermUnlimited = GVar.PermissionID.APPOApproveNoLimit;
        //                }
        //                break;
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //                DocApprove = SysOptionUtility.GetInt("DocApproveForINCPO", cn);
        //                if (DocApprove > 0) ApproveReq = true;
        //                if (ApproveReq)
        //                {
        //                    Limit1 = SysOptionUtility.GetDec("DocApproveForINCPOLimit1", cn);
        //                    Limit2 = SysOptionUtility.GetDec("DocApproveForINCPOLimit2", cn);
        //                    Limit3 = SysOptionUtility.GetDec("DocApproveForINCPOLimit3", cn);
        //                    Limit4 = SysOptionUtility.GetDec("DocApproveForINCPOLimit4", cn);
        //                    Limit5 = SysOptionUtility.GetDec("DocApproveForINCPOLimit5", cn);

        //                    LimitPerm1 = GVar.PermissionID.CSCPOApproveLimit1;
        //                    LimitPerm2 = GVar.PermissionID.CSCPOApproveLimit2;
        //                    LimitPerm3 = GVar.PermissionID.CSCPOApproveLimit3;
        //                    LimitPerm4 = GVar.PermissionID.CSCPOApproveLimit4;
        //                    LimitPerm5 = GVar.PermissionID.CSCPOApproveLimit5;
        //                    LimitPermUnlimited = GVar.PermissionID.CSCPOApproveLimit5;
        //                }
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Request:
        //                ApproveReq = SysOptionUtility.GetBool("DocApproveForAPRQ", cn);

        //                break;
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //                ApproveReq = SysOptionUtility.GetBool("DocApproveForAPPN", cn);

        //                break;

        //            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //                ApproveReq = SysOptionUtility.GetBool("DocApproveForARSJ", cn);
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //                ApproveReq = SysOptionUtility.GetBool("DocApproveForAPPJ", cn);
        //                break;

        //            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //                ApproveReq = SysOptionUtility.GetBool("DocApproveForINCPJ", cn);
        //                break;

        //            case (int)GEnum.SystemCode.Inventory_Production:
        //                ApproveReq = true;
        //                break;

        //            default:
        //                ApproveReq = false;
        //                Authorised = true;
        //                break;
        //        }
        //        #endregion

        //        #region Check if Authorised
        //        if (ApproveReq)
        //        {
        //            switch (objDoc.DocCodeKey)
        //            {
        //                case (int)GEnum.SystemCode.Quotation:
        //                case (int)GEnum.SystemCode.Sales_Order:
        //                case (int)GEnum.SystemCode.Purchase_Order:
        //                case (int)GEnum.SystemCode.Order_Consignment:
        //                    if (SECPermUtility.Perform(cn, LimitPermUnlimited, false))
        //                    {
        //                        Authorised = true;
        //                    }
        //                    else
        //                    {
        //                        CheckAmtH = GFunc.GetDecimalPropertyValue("DocHome", objDoc);

        //                        if (CheckAmtH < Limit1)
        //                        {
        //                            if (SECPermUtility.Perform(cn, LimitPerm1, true))
        //                                Authorised = true;
        //                        }
        //                        else if (CheckAmtH < Limit2)
        //                        {
        //                            if (SECPermUtility.Perform(cn, LimitPerm2, true))
        //                                Authorised = true;
        //                        }
        //                        else if (CheckAmtH < Limit3)
        //                        {
        //                            if (SECPermUtility.Perform(cn, LimitPerm3, true))
        //                                Authorised = true;
        //                        }
        //                        else if (CheckAmtH < Limit4)
        //                        {
        //                            if (SECPermUtility.Perform(cn, LimitPerm4, true))
        //                                Authorised = true;
        //                        }
        //                        else if (CheckAmtH < Limit5)
        //                        {
        //                            if (SECPermUtility.Perform(cn, LimitPerm5, true))
        //                                Authorised = true;
        //                        }
        //                    }
        //                    break;

        //                case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //                    if (SECPermUtility.Perform(cn, GVar.PermissionID.ARSJApprove, true))
        //                    {
        //                        Authorised = true;
        //                    }
        //                    break;

        //                case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //                    if (SECPermUtility.Perform(cn, GVar.PermissionID.APPJApprove, true))
        //                    {
        //                        Authorised = true;
        //                    }
        //                    break;

        //                case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //                    if (SECPermUtility.Perform(cn, GVar.PermissionID.CSCPJApprove, true))
        //                    {
        //                        Authorised = true;
        //                    }
        //                    break;

        //                case (int)GEnum.SystemCode.Inventory_Production:
        //                    if (SECPermUtility.Perform(cn, GVar.PermissionID.INPDTApprove, true))
        //                    {
        //                        Authorised = true;
        //                    }
        //                    break;

        //                case (int)GEnum.SystemCode.Purchase_Plan:
        //                    if (SECPermUtility.Perform(cn, GVar.PermissionID.APPNApprove, true))
        //                    {
        //                        Authorised = true;
        //                    }
        //                    break;

        //                case (int)GEnum.SystemCode.Purchase_Request:
        //                    if (SECPermUtility.Perform(cn, GVar.PermissionID.APRQApprove, true))
        //                    {
        //                        Authorised = true;
        //                    }
        //                    break;
        //            }
        //        }
        //        else
        //        {
        //            Authorised = true;  //Added by mic to set authorised true when approval is not required
        //        }

        //        #endregion

        //        #region Display action not allow and not authorised message
        //        if (ApproveReq)
        //        {
        //            //Check for Action that is not allowed on document State
        //            switch (objDoc.DocState)
        //            {
        //                #region Posted
        //                case (int)GEnum.DocState.Posted:
        //                    switch (ButtonAction)
        //                    {
        //                        case (int)GEnum.DocAction.Save:
        //                            MsgBox.Show(cn, MsgID.Document.DocCannotBeSavedAsDraft);
        //                            return false;
        //                        case (int)GEnum.DocAction.Submit:
        //                            MsgBox.Show(cn, MsgID.Document.PostedDocCannotBeSubmitted);
        //                            return false;
        //                        case (int)GEnum.DocAction.Reject:
        //                            MsgBox.Show(cn, MsgID.Document.PostedDocCannotBeRejected);
        //                            return false;
        //                        case (int)GEnum.DocAction.Approve:
        //                            MsgBox.Show(cn, MsgID.Document.PostedDocCannotBeApproved);
        //                            return false;
        //                    }
        //                    break;
        //                #endregion

        //                #region New, Draft, Rejected, Approved
        //                case (int)GEnum.DocState.New:
        //                case (int)GEnum.DocState.Draft:
        //                case (int)GEnum.DocState.Rejected:
        //                case (int)GEnum.DocState.Approved:
        //                    switch (ButtonAction)
        //                    {
        //                        case (int)GEnum.DocAction.Reject:
        //                            MsgBox.Show(cn, "You cannot reject a document that is NOT pending");
        //                            return false;
        //                        case (int)GEnum.DocAction.Approve:
        //                            MsgBox.Show(cn, "You cannot approve a document that is NOT pending");
        //                            return false;
        //                    }
        //                    break;
        //                #endregion

        //                #region Pending
        //                default:
        //                    //Check for Action that is not allowed when user have no authority to REJECT or APPROVE
        //                    if (Authorised == false)
        //                    {
        //                        switch (ButtonAction)
        //                        {
        //                            case (int)GEnum.DocAction.Reject:
        //                                MsgBox.Show(cn, MsgID.Document.NoAuthorityToRejectDoc);
        //                                return false;

        //                            case (int)GEnum.DocAction.Approve:
        //                                MsgBox.Show(cn, MsgID.Document.NoAuthorityToApproveDoc);
        //                                return false;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (ButtonAction == (int)GEnum.DocAction.Reject)
        //                        {
        //                            //popup zoom form for user to enter disapprove text
        //                            string disapproveText = GFunc.GetStringPropertyValue("DisapproveMsg", objDoc);
        //                            TAUtil.frmZoom frm = new TAUtil.frmZoom(disapproveText, "REASON FOR REJECTION", true);
        //                            frm.ShowDialog();
        //                            if (frm.DialogResult == DialogResult.OK)
        //                            {
        //                                disapproveText = frm.ZoomText;
        //                                objDoc._DisapproveUserKey = AppInfor.CurrentUserKey;
        //                                objDoc._DisapproveMsg = disapproveText;
        //                            }
        //                            else
        //                                return false;
        //                        }
        //                        else if (ButtonAction == (int)GEnum.DocAction.Post)
        //                        {
        //                            objDoc._ApproveUserKey = AppInfor.CurrentUserKey;
        //                        }
        //                    }
        //                    break;
        //                #endregion
        //            }
        //        }
        //        #endregion

        //        if (Authorised)
        //        {
        //            if (ButtonAction == (int)GEnum.DocAction.Post)
        //                objDoc._ApproveUserKey = AppInfor.CurrentUserKey;
        //        }
        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed

        ////Document Posting Process Post
        //private static bool Doc_Posting(SqlConnection cn, Document objDoc, Hashtable dtDetails, int ButtonAction, DataTable dtSvrData, bool ApprovalReq, bool Authorised, out string DocAutoID, out int NewDocState)
        //{

        //    #region Declaration
        //    bool postingLockAdded = false;
        //    int? inProgressKey = -1000;
        //    int? dataKey = 0;
        //    int docConKey = 0;
        //    int docGrpKey = 0;
        //    GEnum.SysLockOption option = GEnum.SysLockOption.ByCodeKeyAndDataKeyAndInprogressKeyAndGUID;
        //    NewDocState = 0;
        //    DocAutoID = string.Empty;
        //    bool cancelProcess = false;

        //    string UpType = string.Empty;
        //    bool UpItmHis = false;
        //    bool UpStock = false;
        //    bool UpCust = false;
        //    bool UpVend = false;
        //    bool UpAcc = false;

        //    DataTable dtDocDetItm = null;   //Caller Document Detail Items
        //    DataTable dtDocDetExp = null;   //Caller Document Detail Expenses

        //    //for Saving
        //    DataTable dsItems = null;       //working table for MSTItm, MSTItmLoc and MSTItmHis update
        //    DataTable dsItemsBatch = null;  //working table for MSTItmBatch, MSTItmBatchLog update
        //    DataTable dtCV = null;          //working table for MSTCon, MSTConHis update
        //    DataTable dtPost = null;        //working table for Posting GLLog update
        //    #endregion

        //    try
        //    {
        //        #region get Detail DataTable from hashtable
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Purchase_Request:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Received_Consignment:
        //            case (int)GEnum.SystemCode.Packing_List:
        //            case (int)GEnum.SystemCode.Consignment_Settlement:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Journal:
        //            case (int)GEnum.SystemCode.Deposit:
        //                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, dtDetails, ref dtDocDetItm);
        //                break;
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, dtDetails, ref dtDocDetItm);
        //                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, dtDetails, ref dtDocDetExp);
        //                break;
        //        }
        //        #endregion

        //        #region Get process to run in posting
        //        if (Doc_SetSaveProcess(cn, objDoc, ButtonAction, ApprovalReq, Authorised, ref NewDocState, ref UpType, ref  UpItmHis, ref  UpStock, ref  UpCust, ref  UpVend, ref  UpAcc) == false)
        //            return false;
        //        #endregion

        //        #region Add Posting Lock
        //        if (ButtonAction != (int)GEnum.DocAction.Delete && objDoc.DocState == (int)GEnum.DocState.New)
        //        {
        //            if (SysLockUtility.AddPostingLock(cn, false, objDoc.GUID, (GEnum.SystemCode)objDoc.DocCodeKey))
        //            {
        //                postingLockAdded = true;
        //            }
        //            else
        //            {
        //                MsgBox.Show(cn, "Another session is in progress, Please try again a few seconds later");
        //                return false;
        //            }
        //        }
        //        #endregion

        //        #region Get NewDocID OR Look for Duplicate DocID for DocState(NEW) / check duplicate DocID for DocState(EDIT)
        //        if (ButtonAction != (int)GEnum.DocAction.Delete)
        //        {
        //            if (objDoc.DocState == (int)GEnum.DocState.New)
        //            {
        //                if (GFunc.IsNE(objDoc.DocID.Trim()))
        //                {
        //                    #region Generate AutoID

        //                    #region Get docConKey
        //                    switch (objDoc.DocCodeKey)
        //                    {
        //                        case (int)GEnum.SystemCode.Quotation:
        //                        case (int)GEnum.SystemCode.Sales_Order:
        //                        case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //                        case (int)GEnum.SystemCode.Delivery_Order:
        //                        case (int)GEnum.SystemCode.Packing_List:
        //                        case (int)GEnum.SystemCode.Sales_Invoice:
        //                        case (int)GEnum.SystemCode.Sales_Debit_Note:
        //                        case (int)GEnum.SystemCode.Sales_Credit_Note:
        //                        case (int)GEnum.SystemCode.Sales_Adjustment:
        //                        case (int)GEnum.SystemCode.Payment_Received:
        //                        case (int)GEnum.SystemCode.Contra:
        //                        case (int)GEnum.SystemCode.Cash_Sale:
        //                        case (int)GEnum.SystemCode.Cash_Debit_Note:
        //                        case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                        case (int)GEnum.SystemCode.Cash_Adjustment:
        //                        case (int)GEnum.SystemCode.Cash_Payment_Received:
        //                        case (int)GEnum.SystemCode.Cash_Contra:
        //                        case (int)GEnum.SystemCode.Purchase_Plan:
        //                        case (int)GEnum.SystemCode.Purchase_Order:
        //                        case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //                        case (int)GEnum.SystemCode.Purchase_Delivery:
        //                        case (int)GEnum.SystemCode.Purchase_Invoice:
        //                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                        case (int)GEnum.SystemCode.Purchase_Adjustment:
        //                        case (int)GEnum.SystemCode.Payment_Issue:
        //                        case (int)GEnum.SystemCode.Issue_Consignment:
        //                        case (int)GEnum.SystemCode.Return_Consignment:
        //                        case (int)GEnum.SystemCode.Order_Consignment:
        //                        case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //                        case (int)GEnum.SystemCode.Received_Consignment:
        //                        case (int)GEnum.SystemCode.Consignment_Settlement:
        //                            docConKey = GFunc.NEInt((int)GFunc.GetIntPropertyValue("DocConKey", objDoc), 0);
        //                            break;
        //                    }
        //                    #endregion

        //                    #region Get docGrpKey
        //                    switch (objDoc.DocCodeKey)
        //                    {
        //                        case (int)GEnum.SystemCode.Quotation:
        //                        case (int)GEnum.SystemCode.Sales_Order:
        //                        case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //                        case (int)GEnum.SystemCode.Delivery_Order:
        //                        case (int)GEnum.SystemCode.Sales_Invoice:
        //                        case (int)GEnum.SystemCode.Sales_Debit_Note:
        //                        case (int)GEnum.SystemCode.Sales_Credit_Note:
        //                        case (int)GEnum.SystemCode.Sales_Adjustment:
        //                        case (int)GEnum.SystemCode.Payment_Received:
        //                        case (int)GEnum.SystemCode.Contra:
        //                        case (int)GEnum.SystemCode.Cash_Sale:
        //                        case (int)GEnum.SystemCode.Cash_Debit_Note:
        //                        case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                        case (int)GEnum.SystemCode.Cash_Adjustment:
        //                        case (int)GEnum.SystemCode.Cash_Payment_Received:
        //                        case (int)GEnum.SystemCode.Cash_Contra:
        //                        case (int)GEnum.SystemCode.Purchase_Plan:
        //                        case (int)GEnum.SystemCode.Purchase_Request:
        //                        case (int)GEnum.SystemCode.Purchase_Order:
        //                        case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //                        case (int)GEnum.SystemCode.Purchase_Delivery:
        //                        case (int)GEnum.SystemCode.Purchase_Invoice:
        //                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                        case (int)GEnum.SystemCode.Purchase_Adjustment:
        //                        case (int)GEnum.SystemCode.Payment_Issue:
        //                        case (int)GEnum.SystemCode.Inventory_Adjustment:
        //                        case (int)GEnum.SystemCode.Inventory_Production:
        //                        case (int)GEnum.SystemCode.Inventory_Transfer:
        //                        case (int)GEnum.SystemCode.Issue_Consignment:
        //                        case (int)GEnum.SystemCode.Return_Consignment:
        //                        case (int)GEnum.SystemCode.Order_Consignment:
        //                        case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //                        case (int)GEnum.SystemCode.Received_Consignment:
        //                        case (int)GEnum.SystemCode.Journal:
        //                        case (int)GEnum.SystemCode.Deposit:
        //                        case (int)GEnum.SystemCode.Bank_Revaluation:
        //                            docGrpKey = GFunc.NEInt((int)GFunc.GetIntPropertyValue("DocGrpKey", objDoc), 0);
        //                            break;
        //                    }

        //                    #endregion

        //                    //this call has been changed, so need to be commented to prevent errors
        //                    //if (SysIDCounterUtility.Get(cn, out DocAutoID, (GEnum.SystemCode)objDoc.DocCodeKey, objDoc.DocTypeNm, docGrpKey, docConKey, objDoc.DocEmKey, objDoc.DocDate.Value))
        //                    //    objDoc.DocID = DocAutoID;
        //                    //else
        //                    //    return false;

        //                    #endregion
        //                }
        //                else
        //                {
        //                    #region Check Duplicate DocID
        //                    switch (objDoc.DocCodeKey)
        //                    {
        //                        case (int)GEnum.SystemCode.Purchase_Invoice:
        //                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //                        case (int)GEnum.SystemCode.Purchase_Delivery:
        //                            // for APBL,APDN,APCN,APPD must check with ConKey + DocID
        //                            docConKey = GFunc.NEInt((int)GFunc.GetIntPropertyValue("DocConKey", objDoc), 0);
        //                            if (SysIDCounterUtility.DuplicateFound(cn, objDoc.DocID, (GEnum.SystemCode)objDoc.DocCodeKey, docConKey, 0, 0))
        //                            {
        //                                MsgBox.Show(cn, "DocID" + MsgID.Validation.DuplicateRecord);
        //                                return false;
        //                            }
        //                            break;

        //                        default:
        //                            if (SysIDCounterUtility.DuplicateFound(cn, objDoc.DocID, (GEnum.SystemCode)objDoc.DocCodeKey, (int)objDoc.DocKey, 0, 0, 0))
        //                            {
        //                                MsgBox.Show(cn, "DocID" + MsgID.Validation.DuplicateRecord);
        //                                return false;
        //                            }
        //                            break;
        //                    }
        //                    #endregion
        //                }
        //            }
        //            else
        //            {
        //                if (GFunc.IsNE(objDoc.DocID.Trim()))
        //                {
        //                    MsgBox.Show(cn, "Document Number" + MsgID.Validation.IsRequire);
        //                    return false;
        //                }
        //                else
        //                {
        //                    #region Check Duplicate DocID
        //                    // for APBL,APDN,APCN,APPD must check with ConKey + DocID
        //                    switch (objDoc.DocCodeKey)
        //                    {
        //                        case (int)GEnum.SystemCode.Purchase_Invoice:
        //                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //                        case (int)GEnum.SystemCode.Purchase_Delivery:
        //                            docConKey = GFunc.NEInt(GFunc.GetIntPropertyValue("DocConKey", objDoc), 0);//Ask Mic
        //                            if (SysIDCounterUtility.DuplicateFound(cn, objDoc.DocID, (GEnum.SystemCode)objDoc.DocCodeKey, GFunc.NEInt(objDoc.DocKey, 0), docConKey, 0, 0))
        //                            {
        //                                MsgBox.Show(cn, "DocID" + MsgID.Validation.DuplicateRecord);
        //                                return false;
        //                            }
        //                            break;

        //                        default:

        //                            if (SysIDCounterUtility.DuplicateFound(cn, objDoc.DocID, (GEnum.SystemCode)objDoc.DocCodeKey, GFunc.NEInt(objDoc.DocKey, 0), 0, 0, 0))
        //                            {
        //                                MsgBox.Show(cn, "DocID" + MsgID.Validation.DuplicateRecord);
        //                                return false;
        //                            }
        //                            break;
        //                    }
        //                    #endregion
        //                }
        //            }
        //        }
        //        #endregion

        //        #region  Prepare Itm History
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Received_Consignment:
        //            case (int)GEnum.SystemCode.Consignment_Settlement:
        //                dsItems = Doc_Item_Prepare(cn, objDoc, NewDocState, dtDocDetItm, UpType, UpStock, UpItmHis, out cancelProcess);
        //                if (cancelProcess)
        //                    return false;
        //                break;

        //            default:
        //                dsItems = new DataTable();    //need to have an empty dataset as it is required in the Doc_Update() later
        //                break;
        //        }
        //        #endregion

        //        #region  Prepare Itm Batch History
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //                dsItemsBatch = Doc_ItemBatch_Prepare(cn, objDoc, NewDocState, dtDocDetItm, UpType, UpStock, UpItmHis, out cancelProcess);
        //                if (cancelProcess)
        //                    return false;
        //                break;

        //            default:
        //                dsItemsBatch = new DataTable();    //need to have an empty dataset as it is required in the Doc_Update() later
        //                break;
        //        }
        //        #endregion

        //        #region  Prepare Customer History
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //                dtCV = Doc_CV_Prepare(cn, objDoc, dtSvrData, UpType, out cancelProcess).Copy();
        //                if (cancelProcess)
        //                    return false;

        //                break;

        //            default:
        //                dtCV = new DataTable(); //need to have an empty datatable as it is required in the Doc_Update() later
        //                break;
        //        }
        //        #endregion

        //        //Prepare Posting      
        //        dtPost = Doc_Posting_Prepare(cn, objDoc, ButtonAction, dtDetails, DocAutoID);

        //        //Perform actual posting to server data
        //        if (Doc_Update(cn, objDoc, dtDetails, dsItems, dsItemsBatch, dtCV, dtPost, ButtonAction, NewDocState, UpType, UpItmHis, UpStock, UpCust, UpVend, UpAcc, docAutoID))
        //            return true;
        //        else
        //            return false;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dtDocDetItm = null;
        //        dsItems = null;
        //        dtCV = null;
        //        dtDocDetExp = null;
        //        dtPost = null;
        //        if (postingLockAdded)
        //            SysLockUtility.RemoveLock(cn, true, (int?)option, (GEnum.SystemCode)objDoc.DocCodeKey, objDoc.GUID, dataKey, inProgressKey);
        //    }
        //}//Completed
        //private static bool Doc_SetSaveProcess(SqlConnection cn, Document objDoc, int ButtonAction, bool ApprovalReq, bool Authorised, ref int NewDocState, ref string UpType, ref bool UpItmHis, ref bool UpStock, ref bool UpCust, ref bool UpVend, ref bool UpAcc)
        //{
        //    string approvalReq = "Anything";
        //    string authorized = "No";
        //    try
        //    {
        //        GEnum.DocAction bAction = (GEnum.DocAction)int.Parse(ButtonAction.ToString());

        //        //The below code was modified to set the parameters for No approval required and No authorisation required conditions
        //        if (ApprovalReq)
        //            approvalReq = "Yes";
        //        else
        //            approvalReq = "No";

        //        if (Authorised)
        //            authorized = "Yes";
        //        else
        //            authorized = "No";

        //        List<SqlParameter> parmList = new List<SqlParameter>();
        //        parmList.Add(new SqlParameter("@DocCodeKey", objDoc.DocCodeKey));
        //        parmList.Add(new SqlParameter("@Action", bAction.ToString()));
        //        parmList.Add(new SqlParameter("@OldState", objDoc.DocState));
        //        parmList.Add(new SqlParameter("@ApprovalReq", approvalReq));
        //        parmList.Add(new SqlParameter("@AuthorisedReq", authorized));
        //        DataTable dt = GFunc.ExecuteProc(cn, "Doc_GetSaveProcessSetting", parmList);

        //        if (dt.Rows.Count > 0)
        //        {
        //            NewDocState = GFunc.NEInt(dt.Rows[0]["NewState"], 0);
        //            UpType = GFunc.NEStr(dt.Rows[0]["UpType"], string.Empty);
        //            UpItmHis = (bool)dt.Rows[0]["UpItmHis"];
        //            UpStock = (bool)dt.Rows[0]["UpStock"];
        //            UpCust = (bool)dt.Rows[0]["UpCust"];
        //            UpVend = (bool)dt.Rows[0]["UpVend"];
        //            UpAcc = (bool)dt.Rows[0]["UpAcc"];
        //        }
        //        else
        //        {
        //            if (Authorised)
        //            {
        //                MsgBox.Show(cn, "This process is not allowed");
        //            }
        //            else
        //            {
        //                MsgBox.Show(cn, "You do not have the permission to perform this function");
        //            }
        //            return false;
        //        }
        //        return true;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //private static DataTable Doc_Item_Prepare(SqlConnection cn, Document objDoc, int NewDocState, DataTable dtDocDetItm, string upType, bool upStock, bool upItmHis, out bool CancelProcess)
        //{
        //    //this function returns the wItmHis working table which is use for Updating MSTItm, MSTItmLoc, MSTItmHis

        //    #region Local Variables
        //    int TranSign = 1;               //Sales (-1), Purchase (+1)           
        //    const int SaleSign = -1;        //Data from Server (-1) 
        //    const int PurchaseSign = 1;     //Data in local DataTable (+1)
        //    string LinkKeys = "ARQODK,ARQODItm,ARSODK,ARSODItm,ARDODK,ARDODItm,ARIVDK,ARIVDItm,APPODK,APPODItm,APPDDK,APPDDItm,CSCPODK,CSCPODItm,CSCPSDK,CSCPSDItm,CSCSIDK,CSCSIDItm,CPDDK,CPDDItm";
        //    string xmlPara = string.Empty;
        //    CancelProcess = false;          //Set default value 

        //    List<SqlParameter> paraList = null;
        //    DataTable dtDocDetItmCopy = new DataTable();            //A copy of the Caller Document Detail Item Table
        //    DataTable dtTrans = new DataTable("dtTrans");           //Working table to store transaction from obj and server, this working table is pass to a SP to return the wItmHis table
        //    DataTable dtItmHis = new DataTable("dtItmHis");         //Working table to update MSTItm, MSTItmDetLoc and MSTItmHis

        //    #endregion

        //    try
        //    {
        //        #region return empty table when no stock or Item History update is required
        //        if (upStock == false && upItmHis == false)
        //            return dtItmHis;
        //        #endregion

        //        #region Creating dtTrans table structure
        //        dtTrans.Columns.Add("ItmKey", typeof(int));
        //        dtTrans.Columns.Add("LocKey", typeof(int));
        //        dtTrans.Columns.Add("Period", typeof(int));
        //        dtTrans.Columns.Add("LineType", typeof(int));
        //        dtTrans.Columns.Add("TransQty", typeof(decimal));
        //        dtTrans.Columns.Add("ItmAmt", typeof(decimal));
        //        dtTrans.Columns.Add("ItmQtyAdj", typeof(decimal));
        //        dtTrans.Columns.Add("ARQODK", typeof(int));
        //        dtTrans.Columns.Add("ARQODItm", typeof(int));
        //        dtTrans.Columns.Add("ARSODK", typeof(int));
        //        dtTrans.Columns.Add("ARSODItm", typeof(int));
        //        dtTrans.Columns.Add("ARDODK", typeof(int));
        //        dtTrans.Columns.Add("ARDODItm", typeof(int));
        //        dtTrans.Columns.Add("ARIVDK", typeof(int));
        //        dtTrans.Columns.Add("ARIVDItm", typeof(int));
        //        dtTrans.Columns.Add("APPODK", typeof(int));
        //        dtTrans.Columns.Add("APPODItm", typeof(int));
        //        dtTrans.Columns.Add("APPDDK", typeof(int));
        //        dtTrans.Columns.Add("APPDDItm", typeof(int));
        //        dtTrans.Columns.Add("CSCPODK", typeof(int));
        //        dtTrans.Columns.Add("CSCPODItm", typeof(int));
        //        dtTrans.Columns.Add("CSCPSDK", typeof(int));
        //        dtTrans.Columns.Add("CSCPSDItm", typeof(int));
        //        dtTrans.Columns.Add("CSCSIDK", typeof(int));
        //        dtTrans.Columns.Add("CSCSIDItm", typeof(int));
        //        dtTrans.Columns.Add("CPDDK", typeof(int));
        //        dtTrans.Columns.Add("CPDDItm", typeof(int));

        //        //set default to zero
        //        foreach (DataColumn col in dtTrans.Columns)
        //        {
        //            col.DefaultValue = 0;
        //        }

        //        #endregion

        //        #region Declare Item Type search condition and set variable default values

        //        dtDocDetItmCopy = dtDocDetItm.Copy();
        //        short docSign = (short)objDoc.DocSign;
        //        int lineType = 1000;
        //        int period = 0;
        //        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Inventory_Production)
        //        {
        //            if (upStock)
        //                period = GFunc.NEInt(GFunc.GetDatePropertyValue("DocDate", objDoc).Value.ToString("yyyyMM"), 0);
        //            else
        //                period = GFunc.NEInt(GFunc.GetDatePropertyValue("DocAllocateDate", objDoc).Value.ToString("yyyyMM"), 0);
        //        }
        //        else
        //        {
        //            period = int.Parse(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //        }

        //        StringBuilder ItmType = new StringBuilder();
        //        ItmType.Append((int)GEnum.ItemType.Stock + ",");
        //        ItmType.Append((int)GEnum.ItemType.StockB + ",");
        //        ItmType.Append((int)GEnum.ItemType.Finished_GD + ",");
        //        ItmType.Append((int)GEnum.ItemType.Finished_GDB + ",");
        //        ItmType.Append((int)GEnum.ItemType.Serial_StockB + ",");
        //        ItmType.Append((int)GEnum.ItemType.Serial_Finished_GDB + ",");
        //        ItmType.Append((int)GEnum.ItemType.Consignment);

        //        #endregion

        //        #region Assign TransSign

        //        switch (objDoc.DocCodeKey)
        //        {
        //            //sales
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //                TranSign = SaleSign;
        //                break;

        //            //Purchase
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Received_Consignment:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Consignment_Settlement:
        //                TranSign = PurchaseSign;
        //                break;

        //            default:
        //                return new DataTable();
        //        }

        //        #endregion

        //        #region Prepare working tables(dtTran) for MSTItm, MSTItmDetLoc, MSTItmHis updates
        //        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Order_Adjustment ||
        //            objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Order_Adjustment ||
        //            objDoc.DocCodeKey == (int)GEnum.SystemCode.Consignment_Order_Adjustment ||
        //            objDoc.DocCodeKey == (int)GEnum.SystemCode.Consignment_Settlement)
        //        {

        //            #region for SO/PO/CO order adjustment and CPS (prepare with obj and Server Data)
        //            if (upItmHis)
        //            {
        //                #region Get related ARSOItm, APPOItm, CSCPOItm, (CSCPS - ARIV/INADJ) transaction to adjust from Server as dtAdjustment
        //                DataTable dtXml = new DataTable();
        //                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Consignment_Settlement)
        //                {
        //                    dtXml = (from row in dtDocDetItmCopy.AsEnumerable()
        //                             where row.Field<int>("LineType") != 4030/*Settlement expenses*/
        //                             select new
        //                             {
        //                                 SettlementDocDC = row.Field<int>("SettlementDocDC"),
        //                                 SettlementDocDK = row.Field<int>("SettlementDocDK"),
        //                                 SettlementDocDItm = row.Field<int>("SettlementDocDItm"),
        //                                 LineType = row.Field<int>("LineType"),
        //                                 ItmKey = row.Field<int>("ItmKey"),
        //                                 ItmQty = row.Field<decimal>("ItmQty"),
        //                                 CPDDK = row.Field<int>("CPDDK"),
        //                                 CPDDItm = row.Field<int>("CPDDItm")
        //                             }).AsDataTable();
        //                }
        //                else
        //                {

        //                    dtXml = (from row in dtDocDetItmCopy.AsEnumerable()
        //                             select new
        //                             {
        //                                 ItmLinkDocDK = row.Field<int>("ItmLinkDocDK"),
        //                                 ItmLinkDocDItm = row.Field<int>("ItmLinkDocDItm"),
        //                                 ItmKey = row.Field<int>("ItmKey"),
        //                                 ItmPrmDateNew = row.Field<DateTime?>("ItmPrmDateNew"),
        //                                 ItmStatus = row.Field<int>("ItmStatus")
        //                             }).AsDataTable();
        //                }
        //                dtXml.TableName = "dtDocDetail";
        //                xmlPara = GFunc.ConvertDataTableToXML(dtXml);
        //                paraList = new List<SqlParameter>();
        //                paraList.Add(new SqlParameter("@DocCodeKey", (int)objDoc.DocCodeKey));
        //                paraList.Add(new SqlParameter("@xmlDocDetail", xmlPara));
        //                DataTable dtAdjustment = GFunc.ExecuteProc(cn, "Doc_PrepareItmHisAPPJ_Get", paraList);
        //                #endregion

        //                #region Use dtAdjustment to prepare dtTrans

        //                DataRow dr = null;
        //                foreach (DataRow rowAdj in dtAdjustment.Rows)
        //                {
        //                    if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Consignment_Settlement)
        //                    {
        //                        #region create dtTrans transactions for Settlement

        //                        DataRow drG = dtTrans.NewRow();
        //                        drG["ItmKey"] = (int)rowAdj["ItmKey"];
        //                        drG["Period"] = period;
        //                        drG["LineType"] = (int)rowAdj["LineType"];
        //                        drG["LocKey"] = (int)rowAdj["LocKey"];
        //                        drG["TransQty"] = GFunc.RndC(TranSign * (decimal)rowAdj["ItmQty"] * docSign * (decimal)rowAdj["ItmConRate"], GVar.RndDecs.Qtypt);
        //                        foreach (DataColumn dc in dtDocDetItmCopy.Columns)
        //                        {
        //                            if (LinkKeys.Split(",".ToCharArray()).Contains(dc.ColumnName))
        //                            {
        //                                if (dtAdjustment.Columns.Contains(dc.ColumnName))
        //                                {
        //                                    drG[dc.ColumnName] = rowAdj[dc.ColumnName];
        //                                }
                                       
        //                            }
        //                        }
        //                        dtTrans.Rows.Add(drG);

        //                        #endregion
        //                    }
        //                    else
        //                    {
        //                        #region create reverse dtTrans transactions for Order Adjustment
        //                        int itmStatus = (int)rowAdj["ItmStatus"];
        //                        if (itmStatus != (int)GEnum.ItemAdjustmentStatus.No_Adjustment) //Cancel, Advance and Postphone
        //                        {
        //                            dr = dtTrans.NewRow();
        //                            dr["ItmKey"] = (int)rowAdj["ItmKey"];
        //                            dr["LocKey"] = (int)rowAdj["LocKey"];
        //                            dr["Period"] = GFunc.NEInt((DateTime.Parse(rowAdj["ItmPrmDate"].ToString()).ToString("yyyyMM")), 0);
        //                            dr["TransQty"] = GFunc.RndC(-TranSign * (decimal)rowAdj["ItmQty"] * docSign * (decimal)rowAdj["ItmConRate"], GVar.RndDecs.Qtypt);
        //                            dr["ItmQtyAdj"] = GFunc.RndC(-TranSign * (decimal)rowAdj["ItmQtyAdj"] * docSign * (decimal)rowAdj["ItmConRate"], GVar.RndDecs.Qtypt);
        //                            dr["ItmAmt"] = GFunc.RndC(-TranSign * (decimal)rowAdj["ItmAmtH"] * docSign, GVar.RndDecs.Amtpt);
        //                            dtTrans.Rows.Add(dr);
        //                        }
        //                        #endregion

        //                        #region create dtTrans transactions
        //                        if (itmStatus == (int)GEnum.ItemAdjustmentStatus.Advance || itmStatus == (int)GEnum.ItemAdjustmentStatus.Postpone)
        //                        {
        //                            dr = dtTrans.NewRow();
        //                            dr["ItmKey"] = (int)rowAdj["ItmKey"];
        //                            dr["LocKey"] = (int)rowAdj["LocKey"];
        //                            dr["Period"] = GFunc.NEInt(DateTime.Parse(rowAdj["ItmPrmDateNew"].ToString()).ToString("yyyyMM"), 0);
        //                            dr["TransQty"] = GFunc.RndC(TranSign * (decimal)rowAdj["ItmQty"] * docSign * (decimal)rowAdj["ItmConRate"], GVar.RndDecs.Qtypt);
        //                            dr["ItmQtyAdj"] = GFunc.RndC(TranSign * (decimal)rowAdj["ItmQtyAdj"] * docSign * (decimal)rowAdj["ItmConRate"], GVar.RndDecs.Qtypt);
        //                            dr["ItmAmt"] = GFunc.RndC(TranSign * (decimal)rowAdj["ItmAmtH"] * docSign, GVar.RndDecs.Amtpt);
        //                            dtTrans.Rows.Add(dr);
        //                        }
        //                        #endregion
        //                    }
        //                }
        //                #endregion
        //            }
        //            else
        //                return null;//Return Empty table
        //            #endregion
        //        }
        //        else
        //        {
        //            #region prepare with obj Data only
        //            if (GFunc.CompareString(upType, "obj") || GFunc.CompareString(upType, "all"))
        //            {
        //                dtDocDetItmCopy.DefaultView.RowFilter = "ItmType In(" + ItmType + ")";
        //                foreach (DataRowView rowLocal in dtDocDetItmCopy.DefaultView)
        //                {
        //                    switch (objDoc.DocCodeKey)
        //                    {
        //                        case (int)GEnum.SystemCode.Inventory_Production:

        //                            #region Prepare dtTran
        //                            lineType = (int)rowLocal["lineType"];

        //                            if (lineType == 3000 || lineType == 3100 || lineType == 3200)
        //                            {
        //                                DataRow drA = dtTrans.NewRow();
        //                                drA["ItmKey"] = (int)rowLocal["ItmKey"];
        //                                drA["Period"] = period;
        //                                drA["LineType"] = lineType;
        //                                drA["LocKey"] = (int)rowLocal["ItmLocKey"];

        //                                switch (lineType)
        //                                {
        //                                    case 3000:  //Finished Goods
        //                                        if (upStock)
        //                                            drA["TransQty"] = GFunc.RndC(PurchaseSign * GFunc.NEDec(rowLocal["FGProduceQty"], 0) * docSign, GVar.RndDecs.Qtypt);
        //                                        else
        //                                            drA["TransQty"] = GFunc.RndC(PurchaseSign * GFunc.NEDec(rowLocal["FGReq"], 0) * docSign, GVar.RndDecs.Qtypt);
        //                                        break;

        //                                    case 3100:  //Material
        //                                    case 3200:  //Packing
        //                                        if (upStock)
        //                                            drA["TransQty"] = GFunc.RndC(SaleSign * (GFunc.NEDec(rowLocal["BOMUsed"], 0) * docSign * GFunc.NEDec(rowLocal["BOMMultiplier"], 1)), GVar.RndDecs.Qtypt);
        //                                        else
        //                                            drA["TransQty"] = GFunc.RndC(SaleSign * (GFunc.NEDec(rowLocal["BOMReq"], 0) * docSign * GFunc.NEDec(rowLocal["BOMMultiplier"], 1)), GVar.RndDecs.Qtypt);
        //                                        break;
        //                                }

        //                                foreach (DataColumn dc in dtDocDetItmCopy.Columns)
        //                                {
        //                                    if (LinkKeys.Split(",".ToCharArray()).Contains(dc.ColumnName))
        //                                    {
        //                                        drA[dc.ColumnName] = rowLocal[dc.ColumnName];
        //                                    }
        //                                }
        //                                dtTrans.Rows.Add(drA);
        //                            }
        //                            break;
        //                            #endregion

        //                        case (int)GEnum.SystemCode.Inventory_Adjustment:

        //                            #region Prepare dtTran
        //                            if ((int)rowLocal["lineType"] == 1000)
        //                            {
        //                                if ((decimal)rowLocal["ItmNewCost"] >= 0)
        //                                {
        //                                    DataRow drB = dtTrans.NewRow();
        //                                    drB["ItmKey"] = (int)rowLocal["ItmKey"];
        //                                    drB["Period"] = period;
        //                                    drB["LineType"] = 1000;
        //                                    drB["LocKey"] = (int)rowLocal["ItmLocKey"];
        //                                    drB["TransQty"] = GFunc.RndC(TranSign * (decimal)rowLocal["ItmQty"] * docSign * (decimal)rowLocal["ItmConRate"], GVar.RndDecs.Qtypt);

        //                                    foreach (DataColumn dc in dtDocDetItmCopy.Columns)
        //                                    {
        //                                        if (LinkKeys.Split(",".ToCharArray()).Contains(dc.ColumnName))
        //                                        {
        //                                            drB[dc.ColumnName] = rowLocal[dc.ColumnName];
        //                                        }
        //                                    }
        //                                    dtTrans.Rows.Add(drB);
        //                                }
        //                            }
        //                            break;
        //                            #endregion

        //                        case (int)GEnum.SystemCode.Issue_Consignment:
        //                        case (int)GEnum.SystemCode.Return_Consignment:

        //                            #region prepare dtTran
        //                            if ((int)rowLocal["lineType"] == 1000)
        //                            {
        //                                DataRow drCFrom = dtTrans.NewRow();
        //                                drCFrom["ItmKey"] = (int)rowLocal["ItmKey"];
        //                                drCFrom["Period"] = period;
        //                                drCFrom["LineType"] = 1000;
        //                                drCFrom["LocKey"] = (int)rowLocal["ItmFromLocKey"];
        //                                drCFrom["TransQty"] = GFunc.RndC(SaleSign * (decimal)rowLocal["ItmQty"] * docSign * (decimal)rowLocal["ItmConRate"], GVar.RndDecs.Qtypt);
        //                                drCFrom["ItmAmt"] = GFunc.RndC(SaleSign * (decimal)rowLocal["ItmAmtH"] * docSign, GVar.RndDecs.Amtpt);
        //                                foreach (DataColumn dc in dtDocDetItmCopy.Columns)
        //                                {
        //                                    if (LinkKeys.Split(",".ToCharArray()).Contains(dc.ColumnName))
        //                                    {
        //                                        drCFrom[dc.ColumnName] = rowLocal[dc.ColumnName];
        //                                    }
        //                                }
        //                                dtTrans.Rows.Add(drCFrom);

        //                                DataRow drCTo = dtTrans.NewRow();
        //                                drCTo["ItmKey"] = (int)rowLocal["ItmKey"];
        //                                drCTo["Period"] = period;
        //                                drCTo["LineType"] = lineType;
        //                                drCTo["LocKey"] = (int)rowLocal["ItmToLocKey"];
        //                                drCTo["TransQty"] = GFunc.RndC(PurchaseSign * (decimal)rowLocal["ItmQty"] * docSign * (decimal)rowLocal["ItmConRate"], GVar.RndDecs.Qtypt);
        //                                drCTo["ItmAmt"] = GFunc.RndC(PurchaseSign * (decimal)rowLocal["ItmAmtH"] * docSign, GVar.RndDecs.Amtpt);
        //                                foreach (DataColumn dc in dtDocDetItmCopy.Columns)
        //                                {
        //                                    if (LinkKeys.Split(",".ToCharArray()).Contains(dc.ColumnName))
        //                                    {
        //                                        drCTo[dc.ColumnName] = rowLocal[dc.ColumnName];
        //                                    }
        //                                }
        //                                dtTrans.Rows.Add(drCTo);
        //                            }
        //                            break;
        //                            #endregion

        //                        case (int)GEnum.SystemCode.Inventory_Transfer:

        //                            #region Prepare dtTran
        //                            if ((int)rowLocal["lineType"] == 1000)
        //                            {
        //                                DataRow drDFrom = dtTrans.NewRow();
        //                                drDFrom["ItmKey"] = (int)rowLocal["ItmKey"];
        //                                drDFrom["Period"] = period;
        //                                drDFrom["LineType"] = 1000;
        //                                drDFrom["LocKey"] = (int)rowLocal["ItmFromLocKey"];
        //                                drDFrom["TransQty"] = GFunc.RndC(SaleSign * (decimal)rowLocal["ItmQty"] * docSign * (decimal)rowLocal["ItmConRate"], GVar.RndDecs.Qtypt);
        //                                foreach (DataColumn dc in dtDocDetItmCopy.Columns)
        //                                {
        //                                    if (LinkKeys.Split(",".ToCharArray()).Contains(dc.ColumnName))
        //                                    {
        //                                        drDFrom[dc.ColumnName] = rowLocal[dc.ColumnName];
        //                                    }
        //                                }
        //                                dtTrans.Rows.Add(drDFrom);

        //                                DataRow drDTo = dtTrans.NewRow();
        //                                drDTo["ItmKey"] = (int)rowLocal["ItmKey"];
        //                                drDTo["Period"] = period;
        //                                drDTo["LineType"] = 1000;
        //                                drDTo["LocKey"] = (int)rowLocal["ItmToLocKey"];
        //                                drDTo["TransQty"] = GFunc.RndC(PurchaseSign * (decimal)rowLocal["ItmQty"] * docSign * (decimal)rowLocal["ItmConRate"], GVar.RndDecs.Qtypt);
        //                                foreach (DataColumn dc in dtDocDetItmCopy.Columns)
        //                                {
        //                                    if (LinkKeys.Split(",".ToCharArray()).Contains(dc.ColumnName))
        //                                    {
        //                                        drDTo[dc.ColumnName] = rowLocal[dc.ColumnName];
        //                                    }
        //                                }
        //                                dtTrans.Rows.Add(drDTo);
        //                            }
        //                            break;
        //                            #endregion

        //                        case (int)GEnum.SystemCode.Sales_Order:
        //                        case (int)GEnum.SystemCode.Purchase_Order:                                
                                    
        //                            #region Prepare dtTran
        //                            if ((int)rowLocal["lineType"] == 1000 && upItmHis)
        //                            {
        //                                DataRow drE = dtTrans.NewRow();
        //                                drE["ItmKey"] = (int)rowLocal["ItmKey"];
        //                                drE["Period"] = period;
        //                                drE["LineType"] = 1000;
        //                                drE["LocKey"] = (int)rowLocal["ItmLocKey"];
        //                                drE["TransQty"] = GFunc.RndC(TranSign * (decimal)rowLocal["ItmQty"] * docSign * (decimal)rowLocal["ItmConRate"], GVar.RndDecs.Qtypt);
        //                                drE["ItmQtyAdj"] = GFunc.RndC(TranSign * (decimal)rowLocal["ItmQtyAdj"] * docSign * (decimal)rowLocal["ItmConRate"], GVar.RndDecs.Qtypt);
        //                                drE["ItmAmt"] = GFunc.RndC(TranSign * (decimal)rowLocal["ItmAmtH"] * docSign, GVar.RndDecs.Amtpt);
        //                                foreach (DataColumn dc in dtDocDetItmCopy.Columns)
        //                                {
        //                                    if (LinkKeys.Split(",".ToCharArray()).Contains(dc.ColumnName))
        //                                    {
        //                                        drE[dc.ColumnName] = rowLocal[dc.ColumnName];
        //                                    }
        //                                }
        //                                dtTrans.Rows.Add(drE);
        //                            }
        //                            break;
        //                            #endregion

        //                        case (int)GEnum.SystemCode.Order_Consignment:

        //                            #region Prepare dtTran
        //                            if (upItmHis)
        //                            {
        //                                DataRow drE = dtTrans.NewRow();
        //                                drE["ItmKey"] = (int)rowLocal["ItmKey"];
        //                                drE["Period"] = period;
        //                                drE["LineType"] = 1000;
        //                                drE["LocKey"] = (int)rowLocal["ItmLocKey"];
        //                                drE["TransQty"] = GFunc.RndC(TranSign * (decimal)rowLocal["ItmQty"] * docSign * (decimal)rowLocal["ItmConRate"], GVar.RndDecs.Qtypt);
        //                                drE["ItmQtyAdj"] = GFunc.RndC(TranSign * (decimal)rowLocal["ItmQtyAdj"] * docSign * (decimal)rowLocal["ItmConRate"], GVar.RndDecs.Qtypt);
        //                                drE["ItmAmt"] = GFunc.RndC(TranSign * (decimal)rowLocal["ItmAmtH"] * docSign, GVar.RndDecs.Amtpt);
        //                                foreach (DataColumn dc in dtDocDetItmCopy.Columns)
        //                                {
        //                                    if (LinkKeys.Split(",".ToCharArray()).Contains(dc.ColumnName))
        //                                    {
        //                                        drE[dc.ColumnName] = rowLocal[dc.ColumnName];
        //                                    }
        //                                }
        //                                dtTrans.Rows.Add(drE);
        //                            }
        //                            break;
        //                            #endregion

        //                        case (int)GEnum.SystemCode.Received_Consignment:

        //                            #region Prepare dtTran
        //                            DataRow drF = null;
        //                            drF = dtTrans.NewRow();
        //                            drF["ItmKey"] = (int)rowLocal["ItmKey"];
        //                            drF["Period"] = period;
        //                            drF["LineType"] = 1000;
        //                            drF["LocKey"] = (int)rowLocal["ItmLocKey"];
        //                            drF["TransQty"] = GFunc.RndC(TranSign * (decimal)rowLocal["ItmQty"] * docSign * (decimal)rowLocal["ItmConRate"], GVar.RndDecs.Qtypt);
        //                            drF["ItmAmt"] = GFunc.RndC(TranSign * (decimal)rowLocal["ItmAmtH"] * docSign, GVar.RndDecs.Amtpt);
        //                            foreach (DataColumn dc in dtDocDetItmCopy.Columns)
        //                            {
        //                                if (LinkKeys.Split(",".ToCharArray()).Contains(dc.ColumnName))
        //                                {
        //                                    drF[dc.ColumnName] = rowLocal[dc.ColumnName];
        //                                }
        //                            }
        //                            dtTrans.Rows.Add(drF);
        //                            break;
        //                            #endregion

        //                        case (int)GEnum.SystemCode.Delivery_Order:
        //                        case (int)GEnum.SystemCode.Sales_Invoice:
        //                        case (int)GEnum.SystemCode.Sales_Debit_Note:
        //                        case (int)GEnum.SystemCode.Sales_Credit_Note:
        //                        case (int)GEnum.SystemCode.Cash_Sale:
        //                        case (int)GEnum.SystemCode.Cash_Debit_Note:
        //                        case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                        case (int)GEnum.SystemCode.Purchase_Delivery:
        //                        case (int)GEnum.SystemCode.Purchase_Invoice:
        //                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //                        case (int)GEnum.SystemCode.Purchase_Credit_Note:

        //                            #region Prepare dtTran
        //                            DataRow drH = null;
        //                            if ((int)rowLocal["LineType"] == 1000)
        //                            {
        //                                drH = dtTrans.NewRow();
        //                                drH["ItmKey"] = (int)rowLocal["ItmKey"];
        //                                drH["Period"] = period;
        //                                drH["LineType"] = 1000;
        //                                drH["LocKey"] = (int)rowLocal["ItmLocKey"];
        //                                drH["TransQty"] = GFunc.RndC(TranSign * (decimal)rowLocal["ItmQty"] * docSign * (decimal)rowLocal["ItmConRate"], GVar.RndDecs.Qtypt);
        //                                drH["ItmAmt"] = GFunc.RndC(TranSign * (decimal)rowLocal["ItmAmtH"] * docSign, GVar.RndDecs.Amtpt);
        //                                foreach (DataColumn dc in dtDocDetItmCopy.Columns)
        //                                {
        //                                    if (LinkKeys.Split(",".ToCharArray()).Contains(dc.ColumnName))
        //                                    {
        //                                        drH[dc.ColumnName] = rowLocal[dc.ColumnName];
        //                                    }
        //                                }
        //                                dtTrans.Rows.Add(drH);
        //                            }
        //                            else if ((int)rowLocal["LineType"] == 1100)//Assembly
        //                            {
        //                                drH = dtTrans.NewRow();
        //                                drH["ItmKey"] = (int)rowLocal["ItmKey"];
        //                                drH["Period"] = period;
        //                                drH["LineType"] = 1100;
        //                                drH["LocKey"] = (int)rowLocal["ItmLocKey"];
        //                                drH["TransQty"] = GFunc.RndC(TranSign * (decimal)rowLocal["ItmQty"] * docSign * (decimal)rowLocal["ItmConRate"], GVar.RndDecs.Qtypt);
        //                                drH["ItmAmt"] = GFunc.RndC(TranSign * (decimal)rowLocal["ItmAmtH"] * docSign, GVar.RndDecs.Amtpt);
        //                                dtTrans.Rows.Add(drH);
        //                            }
        //                            break;
        //                            #endregion

        //                        default:
        //                            return null;
        //                    }
        //                }
        //            }
        //            #endregion
        //        }
        //        #endregion

        //        #region validation for insufficient stock level and return datatable for dtItmHis

        //        DataTable dtOutOfStockList = new DataTable("dtLoc");    //Use to display list of outof stock items
        //        bool StockLevelOutOfStockOccurred = false;
        //        xmlPara = GFunc.ConvertDataTableToXML(dtTrans);
        //        paraList = new List<SqlParameter>();
        //        paraList.Add(new SqlParameter("@DocCodeKey", objDoc.DocCodeKey));
        //        paraList.Add(new SqlParameter("@DocKey", objDoc.DocKey));
        //        paraList.Add(new SqlParameter("@NewDocState", NewDocState));
        //        paraList.Add(new SqlParameter("@UpStock", upStock));
        //        paraList.Add(new SqlParameter("@UpType", upType));
        //        paraList.Add(new SqlParameter("@xmlTrans", xmlPara));
        //        SqlParameter para = new SqlParameter();
        //        int bNotEnough = 0;
        //        para.ParameterName = "@IsNoEnoughQty";
        //        para.Value = bNotEnough;
        //        para.Direction = ParameterDirection.Output;
        //        paraList.Add(para);
        //        DataSet dsTrans = GFunc.ExecuteProcDataSet(cn, "[Doc_PrepareItmHis_Get]", paraList);
        //        bNotEnough = (int)para.Value;

        //        switch (bNotEnough)
        //        {
        //            case 1:
        //                #region Check of insufficient Stock Level
        //                GEnum.AllowOutOfStockType warningLevel = (GEnum.AllowOutOfStockType)SysOptionUtility.GetInt(MsgID.SystemOption.Posting.AllowOutOfStock, cn);
        //                if (warningLevel == GEnum.AllowOutOfStockType.Can_not_Save_When_Not_Enough_Stock)
        //                {
        //                    dtOutOfStockList = dsTrans.Tables[(int)ItmTableIndex.OutofStock].Copy();
        //                    GEnum.MsgBoxButton result = MsgBoxGrid.Show(cn, "InsufficientStockNotAllow", dtOutOfStockList, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
        //                    CancelProcess = true;
        //                    return null;
        //                }

        //                if (warningLevel == GEnum.AllowOutOfStockType.Warn_When_Not_Enough_Stock)
        //                {
        //                    dtOutOfStockList = dsTrans.Tables[(int)ItmTableIndex.OutofStock].Copy();
        //                    GEnum.MsgBoxButton result = MsgBoxGrid.Show(cn, MsgID.ItemPrepare.InsufficientStock,  dtOutOfStockList,GEnum.MsgBoxIcon.ErrorInfo,GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No);
        //                    if (result == GEnum.MsgBoxButton.No)
        //                    {
        //                        CancelProcess = true;
        //                        return null;
        //                    }
        //                }
        //                #endregion
        //                break;

        //            case 2:
        //                #region Check for insufficient Stock Location level
        //                if (StockLevelOutOfStockOccurred == false)
        //                {
        //                    warningLevel = (GEnum.AllowOutOfStockType)SysOptionUtility.GetInt(MsgID.SystemOption.Posting.AllowOutOfStockLocation, cn);
        //                    if (warningLevel == GEnum.AllowOutOfStockType.Can_not_Save_When_Not_Enough_Stock)
        //                    {
        //                        CancelProcess = true;
        //                        return null;
        //                    }
        //                    if (warningLevel == GEnum.AllowOutOfStockType.Warn_When_Not_Enough_Stock)
        //                    {
        //                        dtOutOfStockList = dsTrans.Tables[(int)ItmTableIndex.OutofStockLocation].Copy();
        //                        GEnum.MsgBoxButton result = MsgBoxGrid.Show(cn, MsgID.ItemPrepare.InsufficientStock,  dtOutOfStockList,GEnum.MsgBoxIcon.ErrorInfo,GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No);
        //                        if (result == GEnum.MsgBoxButton.No)
        //                        {
        //                            CancelProcess = true;
        //                            return null;
        //                        }
        //                    }
        //                }
        //                #endregion
        //                break;
        //        }

        //        #endregion

        //        dtItmHis = dsTrans.Tables[(int)ItmTableIndex.ItemHis].Copy();
        //        return dtItmHis;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw taex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dtItmHis = null;
        //        dtDocDetItmCopy = null;
        //    }
        //}//Completed
        //private static DataTable Doc_ItemBatch_Prepare(SqlConnection cn, Document objDoc, int NewDocState, DataTable dtDocDetItm, string upType, bool upStock, bool upItmHis, out bool CancelProcess)
        //{
        //    //this function returns the wItmHisBatch working table which is use for Updating MSTItmBatch, MSTItmBatchLog

        //    #region Local Variables
        //    const int SaleSign = -1;
        //    const int PurchaseSign = 1;
        //    int TranSign = 1; //Sales (-1), Purchase (+1)            
        //    int lineType = (int)GEnum.RecDetailType.DItems;             //Use only in INMFN to prepare dtTrans
        //    CancelProcess = false;//Set default value 

        //    string xmlPara = string.Empty;
        //    List<SqlParameter> paraList = null;

        //    DataTable dtDocDetItmCopy = new DataTable();                //A copy of the Caller Document Detail Item Table
        //    DataTable dtTrans = new DataTable("dtBatchTrans");          //Working table to store transaction from obj, this working table is pass to a SP to return the wItmHisBatch table
        //    DataTable dtItmHisBatch = new DataTable("dtItmHisBatch");  //Working table to update MSTItm, MSTItmDetLoc and MSTItmHis
        //    #endregion

        //    try
        //    {
        //        #region return empty table when no stock or Item History update is required
        //        if (upStock == false && upItmHis == false)
        //            return dtItmHisBatch;
        //        #endregion

        //        #region Creating dtTrans table structure
        //        dtTrans.Columns.Add("BatchKey", typeof(int));
        //        dtTrans.Columns.Add("LineType", typeof(int));
        //        dtTrans.Columns.Add("LogDC", typeof(int));
        //        dtTrans.Columns.Add("LogDK", typeof(int));
        //        dtTrans.Columns.Add("LogDItm", typeof(int));
        //        dtTrans.Columns.Add("LogType", typeof(int));
        //        dtTrans.Columns.Add("LogSign", typeof(int));
        //        dtTrans.Columns.Add("LogDocDate", typeof(DateTime));
        //        dtTrans.Columns.Add("BatchQty", typeof(decimal));
        //        dtTrans.Columns.Add("PurgeKeep", typeof(int));
        //        dtTrans.Columns.Add("PurgeData", typeof(Boolean));
        //        dtTrans.Columns.Add("BatchCost", typeof(decimal));
        //        #endregion

        //        #region Declare Item Type search condition and Set variables
        //        dtDocDetItmCopy = dtDocDetItm.Copy();
        //        decimal docCurrRate = 1;

        //        switch ((int)objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //                docCurrRate = 1M;
        //                break;

        //            default:
        //                docCurrRate = (decimal)GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //                break;
        //        }

        //        StringBuilder ItmType = new StringBuilder();
        //        ItmType.Append((int)GEnum.ItemType.StockB + ",");
        //        ItmType.Append((int)GEnum.ItemType.Finished_GDB + ",");
        //        ItmType.Append((int)GEnum.ItemType.Serial_StockB + ",");
        //        ItmType.Append((int)GEnum.ItemType.Serial_Finished_GDB);
        //        #endregion

        //        #region Declare Line Type search condition
        //        StringBuilder LineType = new StringBuilder();
        //        LineType.Append((int)GEnum.RecDetailType.DItmBatch + ",");
        //        LineType.Append((int)GEnum.RecDetailType.DItmBatch_Serial + ",");
        //        LineType.Append((int)GEnum.RecDetailType.DItmFinishedGoods_Batch + ",");
        //        LineType.Append((int)GEnum.RecDetailType.DItmFinishedGoods_Batch_Serial + ",");
        //        LineType.Append((int)GEnum.RecDetailType.DItmRawMaterial_Batch + ",");
        //        LineType.Append((int)GEnum.RecDetailType.DItmRawMaterial_Batch_Serial + ",");
        //        LineType.Append((int)GEnum.RecDetailType.DItmPackingMaterial_Batch + ",");
        //        LineType.Append((int)GEnum.RecDetailType.DItmPackingMaterial_Batch_Serial);
        //        #endregion

        //        #region Assign TransSign
        //        switch (objDoc.DocCodeKey)
        //        {
        //            //sales
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //                TranSign = SaleSign;
        //                break;

        //            //Purchase
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //                TranSign = PurchaseSign;
        //                break;

        //            default:
        //                return new DataTable();
        //        }
        //        #endregion

        //        #region prepare with obj Data only in dtTrans
        //        if (GFunc.CompareString(upType, "obj") || GFunc.CompareString(upType, "all"))
        //        {
        //            dtDocDetItmCopy.DefaultView.RowFilter = "ItmType In(" + ItmType + ") and LineType In(" + LineType + ")";

        //            foreach (DataRow rowLocal in dtDocDetItmCopy.DefaultView.ToTable().Rows)
        //            {
        //                switch (objDoc.DocCodeKey)
        //                {
        //                    case (int)GEnum.SystemCode.Delivery_Order:
        //                    case (int)GEnum.SystemCode.Sales_Invoice:
        //                    case (int)GEnum.SystemCode.Sales_Credit_Note:
        //                    case (int)GEnum.SystemCode.Sales_Debit_Note:
        //                    case (int)GEnum.SystemCode.Cash_Sale:
        //                    case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                    case (int)GEnum.SystemCode.Cash_Debit_Note:
        //                    case (int)GEnum.SystemCode.Purchase_Delivery:
        //                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                    case (int)GEnum.SystemCode.Purchase_Debit_Note:

        //                        #region Prepare dtTran
        //                        DataRow dr = dtTrans.NewRow();
        //                        dr["BatchKey"] = rowLocal["ItmBatchKey"];
        //                        dr["LineType"] = rowLocal["LineType"];
        //                        dr["LogDC"] = objDoc.DocCodeKey;
        //                        dr["LogDK"] = objDoc.DocKey;
        //                        dr["LogDItm"] = (int)rowLocal["DocItmKey"];
        //                        dr["LogType"] = GEnum.BatchLogType.NormalTransaction;
        //                        dr["LogDocDate"] = objDoc.DocDate;
        //                        dr["PurgeKeep"] = 0;
        //                        dr["PurgeData"] = 0;
        //                        dr["BatchQty"] = (decimal)rowLocal["ItmBatchQty"];
        //                        dr["LogSign"] = TranSign * objDoc.DocSign;
        //                        dr["BatchCost"] = GFunc.RndC((decimal)rowLocal["ItmPrice"] * docCurrRate, GVar.RndDecs.Prcpt);
        //                        dtTrans.Rows.Add(dr);
        //                        break;
        //                        #endregion

        //                    case (int)GEnum.SystemCode.Purchase_Invoice:

        //                        #region Prepare dtTran
        //                        DataRow drAPBL = dtTrans.NewRow();
        //                        drAPBL["BatchKey"] = rowLocal["ItmBatchKey"];
        //                        drAPBL["LineType"] = rowLocal["LineType"];
        //                        drAPBL["LogDC"] = objDoc.DocCodeKey;
        //                        drAPBL["LogDK"] = objDoc.DocKey;
        //                        drAPBL["LogDItm"] = (int)rowLocal["DocItmKey"];
        //                        drAPBL["LogDocDate"] = objDoc.DocDate;
        //                        drAPBL["PurgeKeep"] = 0;
        //                        drAPBL["PurgeData"] = 0;
        //                        drAPBL["BatchQty"] = (decimal)rowLocal["ItmBatchQty"];
        //                        drAPBL["LogSign"] = TranSign * objDoc.DocSign;
        //                        drAPBL["BatchCost"] = GFunc.RndC((decimal)rowLocal["ItmPrice"] * docCurrRate, GVar.RndDecs.Prcpt);
        //                        if (GFunc.NEInt(rowLocal["APPDDK"], 0) > 0)
        //                            drAPBL["LogType"] = GEnum.BatchLogType.AbNormalTransaction;
        //                        else
        //                            drAPBL["LogType"] = GEnum.BatchLogType.NormalTransaction;

        //                        dtTrans.Rows.Add(drAPBL);
        //                        break;
        //                        #endregion

        //                    case (int)GEnum.SystemCode.Inventory_Production:

        //                        #region Prepare dtTran
        //                        lineType = GFunc.NEInt(rowLocal["lineType"].ToString(), 0);
        //                        DataRow drPDT = dtTrans.NewRow();
        //                        drPDT["BatchKey"] = rowLocal["ItmBatchKey"];
        //                        drPDT["LineType"] = rowLocal["LineType"];
        //                        drPDT["LogDC"] = objDoc.DocCodeKey;
        //                        drPDT["LogDK"] = objDoc.DocKey;
        //                        drPDT["LogDItm"] = (int)rowLocal["DocItmKey"];
        //                        drPDT["LogDocDate"] = objDoc.DocDate;
        //                        drPDT["PurgeKeep"] = 0;
        //                        drPDT["PurgeData"] = 0;
        //                        drPDT["BatchCost"] = 0;

        //                        switch (lineType)
        //                        {
        //                            case (int)GEnum.RecDetailType.DItmFinishedGoods_Batch:
        //                            case (int)GEnum.RecDetailType.DItmFinishedGoods_Batch_Serial:
        //                                if (upStock)
        //                                {
        //                                    drPDT["LogType"] = GEnum.BatchLogType.NormalTransaction;
        //                                    drPDT["BatchQty"] = (decimal)rowLocal["ItmBatchQty"];
        //                                }
        //                                else
        //                                {
        //                                    drPDT["LogType"] = GEnum.BatchLogType.AbNormalTransaction;
        //                                    drPDT["BatchQty"] = (decimal)rowLocal["ItmBatchQty"];
        //                                }
        //                                drPDT["LogSign"] = TranSign * objDoc.DocSign;
        //                                break;

        //                            case (int)GEnum.RecDetailType.DItmRawMaterial_Batch:
        //                            case (int)GEnum.RecDetailType.DItmRawMaterial_Batch_Serial:
        //                            case (int)GEnum.RecDetailType.DItmPackingMaterial_Batch:
        //                            case (int)GEnum.RecDetailType.DItmPackingMaterial_Batch_Serial:
        //                                if (upStock)
        //                                {
        //                                    drPDT["LogType"] = GEnum.BatchLogType.NormalTransaction;
        //                                    drPDT["BatchQty"] = GFunc.RndC((decimal)rowLocal["ItmBatchQty"] * GFunc.NEDec(rowLocal["BOMMultiplier"], 1), GVar.RndDecs.Qtypt);
        //                                }
        //                                else
        //                                {
        //                                    drPDT["LogType"] = GEnum.BatchLogType.AbNormalTransaction;
        //                                    drPDT["BatchQty"] = GFunc.RndC((decimal)rowLocal["ItmBatchQty"] * GFunc.NEDec(rowLocal["BOMMultiplier"], 1), GVar.RndDecs.Qtypt); ;
        //                                }
        //                                drPDT["LogSign"] = -TranSign * objDoc.DocSign;
        //                                break;
        //                        }
        //                        dtTrans.Rows.Add(drPDT);
        //                        break;
        //                        #endregion

        //                    case (int)GEnum.SystemCode.Inventory_Adjustment:

        //                        #region Prepare dtTran
        //                        DataRow drAdj = dtTrans.NewRow();
        //                        drAdj["BatchKey"] = rowLocal["ItmBatchKey"];
        //                        drAdj["LineType"] = rowLocal["LineType"];
        //                        drAdj["LogDC"] = objDoc.DocCodeKey;
        //                        drAdj["LogDK"] = objDoc.DocKey;
        //                        drAdj["LogDItm"] = (int)rowLocal["DocItmKey"];
        //                        drAdj["LogType"] = GEnum.BatchLogType.NormalTransaction;
        //                        drAdj["LogDocDate"] = objDoc.DocDate;
        //                        drAdj["PurgeKeep"] = 0;
        //                        drAdj["PurgeData"] = 0;
        //                        drAdj["BatchQty"] = (decimal)rowLocal["ItmBatchQty"];
        //                        drAdj["LogSign"] = TranSign * objDoc.DocSign;
        //                        drAdj["BatchCost"] = GFunc.RndC((decimal)rowLocal["ItmCost"] * docCurrRate, GVar.RndDecs.Prcpt);
        //                        dtTrans.Rows.Add(drAdj);
        //                        break;
        //                        #endregion

        //                    case (int)GEnum.SystemCode.Issue_Consignment:
        //                    case (int)GEnum.SystemCode.Return_Consignment:

        //                        #region Prepare dtTran
        //                        DataRow drConsignment = dtTrans.NewRow();
        //                        drConsignment["BatchKey"] = rowLocal["ItmBatchKey"];
        //                        drConsignment["LineType"] = rowLocal["LineType"];
        //                        drConsignment["LogDC"] = objDoc.DocCodeKey;
        //                        drConsignment["LogDK"] = objDoc.DocKey;
        //                        drConsignment["LogDItm"] = (int)rowLocal["DocItmKey"];
        //                        drConsignment["LogType"] = GEnum.BatchLogType.AbNormalTransaction;
        //                        drConsignment["LogDocDate"] = objDoc.DocDate;
        //                        drConsignment["PurgeKeep"] = 0;
        //                        drConsignment["PurgeData"] = 0;
        //                        drConsignment["BatchQty"] = (decimal)rowLocal["ItmBatchQty"];
        //                        drConsignment["LogSign"] = TranSign;
        //                        drConsignment["BatchCost"] = 0M;
        //                        dtTrans.Rows.Add(drConsignment);
        //                        break;
        //                        #endregion

        //                    case (int)GEnum.SystemCode.Inventory_Transfer:

        //                        #region Prepare dtTran
        //                        DataRow drTransfer = dtTrans.NewRow();
        //                        drTransfer["BatchKey"] = rowLocal["ItmBatchKey"];
        //                        drTransfer["LineType"] = rowLocal["LineType"];
        //                        drTransfer["LogDC"] = objDoc.DocCodeKey;
        //                        drTransfer["LogDK"] = objDoc.DocKey;
        //                        drTransfer["LogDItm"] = (int)rowLocal["DocItmKey"];
        //                        drTransfer["LogType"] = GEnum.BatchLogType.AbNormalTransaction;
        //                        drTransfer["LogDocDate"] = objDoc.DocDate;
        //                        drTransfer["PurgeKeep"] = 0;
        //                        drTransfer["PurgeData"] = 0;
        //                        drTransfer["BatchQty"] = (decimal)rowLocal["ItmBatchQty"];
        //                        drTransfer["LogSign"] = TranSign;
        //                        drTransfer["BatchCost"] = 0M;
        //                        dtTrans.Rows.Add(drTransfer);
        //                        break;
        //                        #endregion

        //                    default:
        //                        return null;
        //                }
        //            }
        //        }
        //        #endregion

        //        #region validation for insufficient batch qty level and return datatable for dtItmHis
        //        xmlPara = GFunc.ConvertDataTableToXML(dtTrans);
        //        paraList = new List<SqlParameter>();
        //        paraList.Add(new SqlParameter("@DocCodeKey", objDoc.DocCodeKey));
        //        paraList.Add(new SqlParameter("@DocKey", objDoc.DocKey));
        //        paraList.Add(new SqlParameter("@DocState", NewDocState));
        //        paraList.Add(new SqlParameter("@UpStock", upStock));
        //        paraList.Add(new SqlParameter("@UpType", upType));
        //        paraList.Add(new SqlParameter("@xmlBatchTrans", xmlPara));
        //        SqlParameter para = new SqlParameter();
        //        bool bNotEnough = false;
        //        para.ParameterName = "@IsNotEnoughQty";
        //        para.Value = bNotEnough;
        //        para.Direction = ParameterDirection.Output;
        //        paraList.Add(para);
        //        DataSet dsTrans = GFunc.ExecuteProcDataSet(cn, "[Doc_PrepareItmBatchCheck_Get]", paraList);
        //        bNotEnough = (bool)para.Value;

        //        if (bNotEnough)
        //        {
        //            DataTable dtOutOfStockList = dsTrans.Tables[(int)ItmTableIndex.OutofStock].Copy();
        //            GEnum.MsgBoxButton result = MsgBoxGrid.Show(cn, "One or More Items does not have sufficient Batch Qty",dtOutOfStockList,GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK );
        //            CancelProcess = true;
        //            return null;
        //        }
        //        #endregion

        //        dtItmHisBatch = dsTrans.Tables[(int)ItmTableIndex.ItemHis].Copy();
        //        return dtItmHisBatch;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw taex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dtItmHisBatch = null;
        //        dtDocDetItmCopy = null;
        //        dtTrans = null;
        //    }
        //}//Completed
        //private static DataTable Doc_CV_Prepare(SqlConnection cn, Document objDoc, DataTable dtSvrData, string upType, out bool cancelProcess)
        //{
        //    try
        //    {
        //        #region Local Variables

        //        //Process Variables
        //        int limitControlOpt = 0;
        //        bool runCheck = false;
        //        bool runCreditLimitCheck = false;
        //        bool runCreditLimitControl = false;
        //        bool runExceedTermCheck = false;
        //        bool runExceedTermControl = false;
        //        bool runCheckdocApplyGainAmt = false;
        //        decimal exceedAmt = 0;
        //        SqlCommand cm = null;

        //        //Common Document variables
        //        int docCodeKey = 0;
        //        short docSign = 0;

        //        //Obj Variables
        //        int docConKey = 0;
        //        int docPeriod = 0;
        //        int docGrpKey = 0;
        //        int docCurrKey = 0;
        //        decimal docGrand = 0;
        //        decimal docHome = 0;
        //        decimal docApplyGainAmt = 0;

        //        //Server Variables
        //        int svrTransSign = -1;
        //        int svrdocConKey = 0;
        //        int svrdocPeriod = 0;
        //        int svrdocGrpKey = 0;
        //        int svrdocCurrKey = 0;
        //        decimal svrdocGrand = 0;
        //        decimal svrdocHome = 0;
        //        decimal svrdocApplyGainAmt = 0;

        //        //Variable for Balance Calculation
        //        decimal? CRAmt = 0;
        //        decimal? CHAmt = 0;
        //        decimal? VAmt = 0;
        //        decimal? FCR = 0;
        //        decimal? FCH = 0;
        //        decimal? FV = 0;
        //        decimal? HCR = 0;
        //        decimal? HCH = 0;
        //        decimal? HV = 0;
        //        decimal? FAmt = 0;
        //        decimal? HAmt = 0;
        //        #endregion

        //        #region Creating dtConHis table structure
        //        DataTable dtConHis = new DataTable("ConHistory");
        //        dtConHis.Columns.Add("ConKey", typeof(int));
        //        dtConHis.Columns.Add("Period", typeof(int));
        //        dtConHis.Columns.Add("DocDC", typeof(int));
        //        dtConHis.Columns.Add("DocGrpKey", typeof(int));
        //        dtConHis.Columns.Add("DocCurrKey", typeof(int));
        //        dtConHis.Columns.Add("CRAmt", typeof(decimal));
        //        dtConHis.Columns.Add("ChAmt", typeof(decimal));
        //        dtConHis.Columns.Add("VAmt", typeof(decimal));
        //        dtConHis.Columns.Add("CrAmtF", typeof(decimal));
        //        dtConHis.Columns.Add("CrAmtH", typeof(decimal));
        //        dtConHis.Columns.Add("ChAmtF", typeof(decimal));
        //        dtConHis.Columns.Add("ChAmtH", typeof(decimal));
        //        dtConHis.Columns.Add("VAmtF", typeof(decimal));
        //        dtConHis.Columns.Add("VAmtH", typeof(decimal));

        //        //set default to zero
        //        foreach (DataColumn col in dtConHis.Columns)
        //        {
        //            col.DefaultValue = 0;
        //        }

        //        #endregion

        //        #region set check condition by doccodekey
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //                runCheckdocApplyGainAmt = false;
        //                runCheck = true;
        //                break;

        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //                runCheckdocApplyGainAmt = true;
        //                runCheck = true;
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                runCheckdocApplyGainAmt = true;
        //                break;

        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //                break;

        //            default:
        //                cancelProcess = false;
        //                return dtConHis;
        //        }
        //        #endregion

        //        #region Get obj and server data
        //        docCodeKey = (int)objDoc.DocCodeKey;
        //        docSign = (short)objDoc.DocSign.Value;

        //        docConKey = GFunc.NEInt((int)GFunc.GetIntPropertyValue("DocConKey", objDoc), 0);
        //        docPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //        docGrpKey = (int)GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //        docCurrKey = (int)GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //        docGrand = (decimal)GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //        docHome = (decimal)GFunc.GetDecimalPropertyValue("DocHome", objDoc);

        //        if (runCheckdocApplyGainAmt)
        //            docApplyGainAmt = GFunc.NEDec((decimal)GFunc.GetDecimalPropertyValue("DocApplyGainAmt", objDoc), 0);
        //        else
        //            docApplyGainAmt = 0;

        //        if (dtSvrData.Rows.Count > 0)
        //        {
        //            svrdocConKey = (int)dtSvrData.Rows[0]["DocConKey"];
        //            svrdocPeriod = Convert.ToInt32(GFunc.NEDateTime(dtSvrData.Rows[0]["DocDate"], DateTime.Today).ToString("yyyyMM"));
        //            svrdocGrpKey = (int)dtSvrData.Rows[0]["DocGrpKey"];
        //            svrdocCurrKey = (int)dtSvrData.Rows[0]["DocCurrKey"];
        //            svrdocGrand = (decimal)dtSvrData.Rows[0]["DocGrand"];
        //            svrdocHome = (decimal)dtSvrData.Rows[0]["DocHome"];

        //            if (runCheckdocApplyGainAmt)
        //                svrdocApplyGainAmt = GFunc.NEDec(dtSvrData.Rows[0]["DocApplyGainAmt"], 0);
        //            else
        //                svrdocApplyGainAmt = 0;
        //        }
        //        #endregion

        //        #region Validation for Exceed Credit Limit and Term
        //        if (runCheck)
        //        {
        //            #region set check process to run
        //            limitControlOpt = SysOptionUtility.GetInt("CheckCustomerLimit", cn);
        //            switch (limitControlOpt)
        //            {
        //                case 20:    //warn if exceed credit limit
        //                    runCreditLimitCheck = true;
        //                    break;

        //                case 30:    //can't save if exceed credit limit
        //                    runCreditLimitCheck = true;
        //                    runCreditLimitControl = true;
        //                    break;

        //                case 40:	//warn if exceed term
        //                    runExceedTermCheck = true;
        //                    break;

        //                case 50:	//can't save if exceed term
        //                    runExceedTermCheck = true;
        //                    runExceedTermControl = true;
        //                    break;

        //                case 60:	//warn if exceed credit limit or term
        //                    runCreditLimitCheck = true;
        //                    runExceedTermCheck = true;
        //                    break;

        //                case 70:	//can't save if exceed credit limit or term
        //                    runCreditLimitCheck = true;
        //                    runCreditLimitControl = true;
        //                    runExceedTermCheck = true;
        //                    runExceedTermControl = true;
        //                    break;
        //            }
        //            #endregion

        //            #region credit limit check
        //            if (runCreditLimitCheck)
        //            {
        //                cm = cn.CreateCommand();
        //                cm.CommandType = CommandType.StoredProcedure;
        //                cm.CommandText = "Doc_CheckExceedTerm";
        //                cm.Parameters.AddWithValue("@Option", (Int32)0);
        //                cm.Parameters.AddWithValue("@ConKey", docConKey);
        //                cm.Parameters.AddWithValue("@Exceed", 0);
        //                cm.Parameters["@Exceed"].Direction = ParameterDirection.Output;
        //                cm.Parameters.AddWithValue("@AmountExceeded", 0M);
        //                cm.Parameters["@AmountExceeded"].Direction = ParameterDirection.Output;
        //                cm.ExecuteNonQuery();

        //                if (int.Parse(cm.Parameters["@Exceed"].Value.ToString()) == 1)
        //                {
        //                    exceedAmt = (decimal)cm.Parameters["@AmountExceeded"].Value;
        //                    if (runCreditLimitControl)
        //                    {
        //                        MsgBox.Show(cn, "Credit Limit has been exceeded by : " + exceedAmt);
        //                        cancelProcess = true;
        //                        return dtConHis;
        //                    }
        //                    else
        //                    {
        //                        if (MsgBox.Show(cn, "Credit Limit has been exceeded by : " + exceedAmt + " , continue ?", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
        //                        {
        //                            cancelProcess = true;
        //                            return dtConHis;
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion

        //            #region exceed term check
        //            if (runExceedTermCheck)
        //            {
        //                cm = cn.CreateCommand();
        //                cm.CommandType = CommandType.StoredProcedure;
        //                cm.CommandText = "Doc_CheckExceedTerm";
        //                cm.Parameters.AddWithValue("@Option", (Int32)1);
        //                cm.Parameters.AddWithValue("@ConKey", GFunc.NEInt(GFunc.GetPropertyValue("DocConKey", objDoc), 0));
        //                cm.Parameters.AddWithValue("@Exceed", 0);
        //                cm.Parameters["@Exceed"].Direction = ParameterDirection.Output;
        //                cm.Parameters.AddWithValue("@AmountExceeded", 0);
        //                cm.Parameters["@AmountExceeded"].Direction = ParameterDirection.Output;
        //                cm.ExecuteNonQuery();

        //                if (int.Parse(cm.Parameters["@Exceed"].Value.ToString()) == 1)
        //                {
        //                    if (runExceedTermControl)
        //                    {
        //                        MsgBox.Show(cn, "There are document that has exceeded it's term");
        //                        cancelProcess = true;
        //                        return dtConHis;
        //                    }
        //                    else
        //                    {
        //                        if (MsgBox.Show(cn, "There are document that has exceeded it's term , continue ?", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
        //                        {
        //                            cancelProcess = true;
        //                            return dtConHis;
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion
        //        }
        //        #endregion

        //        #region Calculate dtConHis for obj data
        //        if (upType == GVar.UpdateType.Obj || upType == GVar.UpdateType.All)
        //        {
        //            #region Calculate FAmt and Hamt
        //            switch (objDoc.DocCodeKey)
        //            {
        //                case (int)GEnum.SystemCode.Sales_Credit_Note:
        //                case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                    FAmt = GFunc.RndC(docGrand * docSign, GVar.RndDecs.Amtpt);
        //                    HAmt = GFunc.RndC((docHome + docApplyGainAmt) * docSign, GVar.RndDecs.Amtpt);
        //                    break;

        //                default:
        //                    FAmt = GFunc.RndC(docGrand * docSign, GVar.RndDecs.Amtpt);
        //                    HAmt = GFunc.RndC(docHome * docSign, GVar.RndDecs.Amtpt);
        //                    break;
        //            }
        //            #endregion

        //            #region Assign CR/CH/V Amt base on DocCode and FAmt/HAmt
        //            switch (objDoc.DocCodeKey)
        //            {
        //                case (int)GEnum.SystemCode.Sales_Invoice:
        //                case (int)GEnum.SystemCode.Sales_Credit_Note:
        //                case (int)GEnum.SystemCode.Sales_Debit_Note:
        //                case (int)GEnum.SystemCode.Payment_Received:
        //                case (int)GEnum.SystemCode.Sales_Adjustment:
        //                    CRAmt = HAmt;
        //                    FCR = FAmt;
        //                    HCR = HAmt;
        //                    CHAmt = 0;
        //                    FCH = 0;
        //                    HCH = 0;
        //                    VAmt = 0;
        //                    FV = 0;
        //                    HV = 0;
        //                    break;

        //                case (int)GEnum.SystemCode.Cash_Sale:
        //                case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                case (int)GEnum.SystemCode.Cash_Debit_Note:
        //                case (int)GEnum.SystemCode.Cash_Payment_Received:
        //                case (int)GEnum.SystemCode.Cash_Adjustment:
        //                    CRAmt = 0;
        //                    FCR = 0;
        //                    HCR = 0;
        //                    CHAmt = HAmt;
        //                    FCH = FAmt;
        //                    HCH = HAmt;
        //                    VAmt = 0;
        //                    FV = 0;
        //                    HV = 0;
        //                    break;

        //                case (int)GEnum.SystemCode.Purchase_Invoice:
        //                case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //                case (int)GEnum.SystemCode.Payment_Issue:
        //                case (int)GEnum.SystemCode.Purchase_Adjustment:
        //                    CRAmt = 0;
        //                    FCR = 0;
        //                    HCR = 0;
        //                    CHAmt = 0;
        //                    FCH = 0;
        //                    HCH = 0;
        //                    VAmt = HAmt;
        //                    FV = FAmt;
        //                    HV = HAmt;
        //                    break;

        //                case (int)GEnum.SystemCode.Contra:
        //                    CRAmt = HAmt;
        //                    FCR = FAmt;
        //                    HCR = HAmt;
        //                    CHAmt = 0;
        //                    FCH = 0;
        //                    HCH = 0;
        //                    VAmt = HAmt;
        //                    FV = FAmt;
        //                    HV = HAmt;
        //                    break;

        //                case (int)GEnum.SystemCode.Cash_Contra:
        //                    CRAmt = 0;
        //                    FCR = 0;
        //                    HCR = 0;
        //                    CHAmt = HAmt;
        //                    FCH = FAmt;
        //                    HCH = HAmt;
        //                    VAmt = HAmt;
        //                    FV = FAmt;
        //                    HV = HAmt;
        //                    break;
        //            }
        //            #endregion

        //            #region Append dtConHis from variables
        //            DataRow dr = dtConHis.NewRow();
        //            dr["ConKey"] = docConKey;
        //            dr["Period"] = docPeriod;
        //            dr["DocDC"] = docCodeKey;
        //            dr["DocGrpKey"] = docGrpKey;
        //            dr["DocCurrKey"] = docCurrKey;
        //            dr["CrAmtF"] = FCR;
        //            dr["CrAmtH"] = HCR;
        //            dr["ChAmtF"] = FCH;
        //            dr["ChAmtH"] = HCH;
        //            dr["VAmtF"] = FV;
        //            dr["VAmtH"] = HV;
        //            dtConHis.Rows.Add(dr);
        //            #endregion
        //        }
        //        dtConHis.AcceptChanges();
        //        #endregion

        //        #region Calculate dtConHis for Server Data
        //        if (upType == GVar.UpdateType.Svr || upType == GVar.UpdateType.All)
        //        {
        //            #region Calculate FAmt and Hamt
        //            switch (objDoc.DocCodeKey)
        //            {
        //                case (int)GEnum.SystemCode.Sales_Credit_Note:
        //                case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                    FAmt = GFunc.RndC(svrdocGrand * docSign * svrTransSign, GVar.RndDecs.Amtpt);
        //                    HAmt = GFunc.RndC((svrdocHome + svrdocApplyGainAmt) * docSign * svrTransSign, GVar.RndDecs.Amtpt);
        //                    break;

        //                default:
        //                    FAmt = GFunc.RndC(svrdocGrand * docSign * svrTransSign, GVar.RndDecs.Amtpt);
        //                    HAmt = GFunc.RndC(svrdocHome * docSign * svrTransSign, GVar.RndDecs.Amtpt);
        //                    break;
        //            }
        //            #endregion

        //            #region Assign CR/CH/V Amt base on DocCode and FAmt/HAmt

        //            switch (objDoc.DocCodeKey)
        //            {
        //                case (int)GEnum.SystemCode.Sales_Invoice:
        //                case (int)GEnum.SystemCode.Sales_Credit_Note:
        //                case (int)GEnum.SystemCode.Sales_Debit_Note:
        //                case (int)GEnum.SystemCode.Payment_Received:
        //                case (int)GEnum.SystemCode.Sales_Adjustment:
        //                    CRAmt = HAmt;
        //                    FCR = FAmt;
        //                    HCR = HAmt;
        //                    CHAmt = 0;
        //                    FCH = 0;
        //                    HCH = 0;
        //                    VAmt = 0;
        //                    FV = 0;
        //                    HV = 0;
        //                    break;

        //                case (int)GEnum.SystemCode.Cash_Sale:
        //                case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                case (int)GEnum.SystemCode.Cash_Debit_Note:
        //                case (int)GEnum.SystemCode.Cash_Payment_Received:
        //                case (int)GEnum.SystemCode.Cash_Adjustment:
        //                    CRAmt = 0;
        //                    FCR = 0;
        //                    HCR = 0;
        //                    CHAmt = HAmt;
        //                    FCH = FAmt;
        //                    HCH = HAmt;
        //                    VAmt = 0;
        //                    FV = 0;
        //                    HV = 0;
        //                    break;

        //                case (int)GEnum.SystemCode.Purchase_Invoice:
        //                case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //                case (int)GEnum.SystemCode.Payment_Issue:
        //                case (int)GEnum.SystemCode.Purchase_Adjustment:
        //                    CRAmt = 0;
        //                    FCR = 0;
        //                    HCR = 0;
        //                    CHAmt = 0;
        //                    FCH = 0;
        //                    HCH = 0;
        //                    VAmt = HAmt;
        //                    FV = FAmt;
        //                    HV = HAmt;
        //                    break;

        //                case (int)GEnum.SystemCode.Contra:
        //                    CRAmt = HAmt;
        //                    FCR = FAmt;
        //                    HCR = HAmt;
        //                    CHAmt = 0;
        //                    FCH = 0;
        //                    HCH = 0;
        //                    VAmt = HAmt;
        //                    FV = FAmt;
        //                    HV = HAmt;
        //                    break;

        //                case (int)GEnum.SystemCode.Cash_Contra:
        //                    CRAmt = 0;
        //                    FCR = 0;
        //                    HCR = 0;
        //                    CHAmt = HAmt;
        //                    FCH = FAmt;
        //                    HCH = HAmt;
        //                    VAmt = 0;
        //                    VAmt = HAmt;
        //                    FV = FAmt;
        //                    HV = HAmt;
        //                    break;
        //            }
        //            #endregion

        //            #region Append dtConHis from variables
        //            DataRow dr = dtConHis.NewRow();

        //            dr["ConKey"] = svrdocConKey;
        //            dr["Period"] = svrdocPeriod;
        //            dr["DocDC"] = docCodeKey;
        //            dr["DocGrpKey"] = svrdocGrpKey;
        //            dr["DocCurrKey"] = svrdocCurrKey;
        //            dr["CrAmtF"] = FCR;
        //            dr["CrAmtH"] = HCR;
        //            dr["ChAmtF"] = FCH;
        //            dr["ChAmtH"] = HCH;
        //            dr["VAmtF"] = FV;
        //            dr["VAmtH"] = HV;
        //            dtConHis.Rows.Add(dr);
        //            #endregion
        //        }
        //        dtConHis.AcceptChanges();
        //        #endregion

        //        cancelProcess = false;
        //        return dtConHis;
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.Show(cn, ex.ToString());
        //        throw ex;
        //    }
        //}//Completed
        //private static DataTable Doc_Posting_Prepare(SqlConnection cn, Document objDoc, int ButtonAction, Hashtable dtDetails, string DocAutoID)
        //{
        //    docAutoID = DocAutoID;
        //    DataTable dtItem = null;
        //    DataTable dtExp = null;
        //    DataTable dtPosting = null;

        //    try
        //    {
        //        #region check Detail DataTable
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Purchase_Request:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Received_Consignment:
        //            case (int)GEnum.SystemCode.Packing_List:
        //            case (int)GEnum.SystemCode.Consignment_Settlement:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Journal:
        //            case (int)GEnum.SystemCode.Deposit:
        //                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, dtDetails, ref dtItem);
        //                break;
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, dtDetails, ref dtItem);
        //                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, dtDetails, ref dtExp);
        //                break;
        //        }
        //        #endregion

        //        if (ButtonAction != (int)GEnum.DocAction.Delete)//New or Edit
        //        {
        //            switch (objDoc.DocCodeKey)
        //            {
        //                case (int)GEnum.SystemCode.Sales_Invoice:
        //                case (int)GEnum.SystemCode.Sales_Credit_Note:
        //                case (int)GEnum.SystemCode.Sales_Debit_Note:
        //                case (int)GEnum.SystemCode.Cash_Sale:
        //                case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                case (int)GEnum.SystemCode.Cash_Debit_Note:
        //                    dtPosting = DTPostARIV(cn, objDoc, dtItem);
        //                    break;
        //                case (int)GEnum.SystemCode.Payment_Received:
        //                case (int)GEnum.SystemCode.Cash_Payment_Received:
        //                    dtPosting = DTPostARPY(cn, objDoc, dtItem, dtExp);
        //                    break;
        //                case (int)GEnum.SystemCode.Sales_Adjustment:
        //                case (int)GEnum.SystemCode.Cash_Adjustment:
        //                    dtPosting = DTPostARRF(cn, objDoc);
        //                    break;
        //                case (int)GEnum.SystemCode.Purchase_Invoice:
        //                case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //                    dtPosting = DTPostAPBL(cn, objDoc, dtItem);
        //                    break;
        //                case (int)GEnum.SystemCode.Payment_Issue:
        //                    dtPosting = DTPostAPPY(cn, objDoc, dtItem, dtExp);
        //                    break;
        //                case (int)GEnum.SystemCode.Purchase_Adjustment:
        //                    dtPosting = DTPostAPRF(cn, objDoc);
        //                    break;
        //                case (int)GEnum.SystemCode.Purchase_Delivery:
        //                    dtPosting = DTPostAPPD(cn, objDoc, dtItem);
        //                    break;
        //                case (int)GEnum.SystemCode.Contra:
        //                case (int)GEnum.SystemCode.Cash_Contra:
        //                    dtPosting = DTPostARCT(cn, objDoc, dtItem);
        //                    break;
        //                case (int)GEnum.SystemCode.Inventory_Adjustment:
        //                    dtPosting = DTPostINADJ(cn, objDoc, dtItem);
        //                    break;
        //                case (int)GEnum.SystemCode.Journal:
        //                    dtPosting = DTPostGLJN(cn, objDoc, dtItem);
        //                    break;
        //                case (int)GEnum.SystemCode.Deposit:
        //                    dtPosting = DTPostGLDP(cn, objDoc, dtItem);
        //                    break;
        //                case (int)GEnum.SystemCode.Bank_Revaluation:
        //                    dtPosting = DTPostGLRV(cn, objDoc, dtItem);
        //                    break;
        //            }
        //        }
        //        return dtPosting;
        //    }
        //    catch (TAException tex)
        //    {
        //        MsgBox.Show(cn, tex.MsgID);
        //        throw tex;
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.Show(cn, ex.ToString());
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dtExp = null;
        //        dtItem = null;
        //    }
        //}//Completed
        //private static bool Doc_Update(SqlConnection cn, Document objDoc, Hashtable htDetail, DataTable dsItmHis, DataTable dsItmHisBatch, DataTable dtConHis, DataTable dtPost, int ButtonAction, int NewDocState, string UpType, bool UpItmHis, bool UpStock, bool UpCust, bool UpVend, bool UpAcc, string DocAutoID)
        //{
        //    DataSet dsDocumentUpdate = new DataSet();
        //    DataTable dtObject;
        //    DataTable dtDetItm = new DataTable();
        //    DataTable dtDetExp = new DataTable();
        //    DataTable dtDetPack;

        //    string headerTableName = "";
        //    try
        //    {
        //        if (ButtonAction != (int)GEnum.DocAction.Delete)
        //            objDoc.DocState = NewDocState;

        //        #region assign value to DataTables
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //                headerTableName = "dtARQO";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetItm.TableName = "dtARQODetItm";
        //                DataTable dtDetVendor = ((DataTable)htDetail[GEnum.Details.Doc_Vendor]).Copy();
        //                dtDetVendor.TableName = "dtARQOVendor";
        //                DataTable dtDetItmVendor = ((DataTable)htDetail[GEnum.Details.Doc_ItmVendor]).Copy();

        //                dtDetItmVendor.TableName = "dtARQODetItmVendor";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                dsDocumentUpdate.Tables.Add(dtDetVendor);
        //                dsDocumentUpdate.Tables.Add(dtDetItmVendor);
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //                headerTableName = "dtAPPN";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetItm.TableName = "dtAPPNDetItm";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Request:
        //                headerTableName = "dtAPRQ";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();

        //                dtDetItm.TableName = "dtAPRQDetItm";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                break;

        //            case (int)GEnum.SystemCode.Sales_Order:
        //                headerTableName = "dtARSO";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetItm.TableName = "dtARSODetItm";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Order:
        //                headerTableName = "dtAPPO";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetItm.TableName = "dtAPPODetItm";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                break;

        //            case (int)GEnum.SystemCode.Order_Consignment:
        //                headerTableName = "dtCSCPO";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetItm.TableName = "dtCSCPODetItm";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                break;

        //            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //                headerTableName = "dtAPPJ";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetItm.TableName = "dtAPPJDetItm";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                break;

        //            case (int)GEnum.SystemCode.Delivery_Order:
        //                headerTableName = "dtARDO";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetItm.TableName = "dtARDODetItm";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //                headerTableName = "dtAPPD";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetItm.TableName = "dtAPPDDetItm";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                break;

        //            case (int)GEnum.SystemCode.Received_Consignment:
        //                headerTableName = "dtCSCPD";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetItm.TableName = "dtCSCPDDetItm";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                break;

        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //                headerTableName = "dtCSCSI";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetItm.TableName = "dtCSCSIDetItm";
        //                dtDetExp = ((DataTable)htDetail[GEnum.Details.Doc_Exp]).Copy();
        //                dtDetExp.TableName = "dtCSCSIDetExp";

        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                dsDocumentUpdate.Tables.Add(dtDetExp);
        //                break;

        //            case (int)GEnum.SystemCode.Packing_List:
        //                headerTableName = "dtARPL";
        //                dtDetPack = ((DataTable)htDetail[GEnum.Details.Doc_Pack]).Copy();
        //                dtDetPack.TableName = "dtARPLDetPack";
        //                DataTable dtDetPackItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetPackItm.TableName = "dtARPLDetPackItm";

        //                dsDocumentUpdate.Tables.Add(dtDetPack);
        //                dsDocumentUpdate.Tables.Add(dtDetPackItm);
        //                break;

        //            case (int)GEnum.SystemCode.Consignment_Settlement:
        //                headerTableName = "dtCSCPS";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();

        //                dtDetItm.TableName = "dtCSCPSDetItm";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                break;

        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //                headerTableName = "dtARIV";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();

        //                dtDetItm.TableName = "dtARIVDetItm";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //                headerTableName = "dtAPBL";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();

        //                dtDetItm.TableName = "dtAPBLDetItm";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                break;

        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //                headerTableName = "dtARADJ";
        //                break;

        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //                headerTableName = "dtAPADJ";
        //                break;

        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //                headerTableName = "dtARPY";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetItm.TableName = "dtARPYDetItm";
        //                dtDetExp = ((DataTable)htDetail[GEnum.Details.Doc_Exp]).Copy();
        //                dtDetExp.TableName = "dtARPYDetExp";

        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                dsDocumentUpdate.Tables.Add(dtDetExp);
        //                break;

        //            case (int)GEnum.SystemCode.Payment_Issue:
        //                headerTableName = "dtAPPY";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetItm.TableName = "dtAPPYDetItm";
        //                dtDetExp = ((DataTable)htDetail[GEnum.Details.Doc_Exp]).Copy();
        //                dtDetExp.TableName = "dtAPPYDetExp";

        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                dsDocumentUpdate.Tables.Add(dtDetExp);
        //                break;

        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //                headerTableName = "dtARCT";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetItm.TableName = "dtARCTDetItm";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                break;

        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //                headerTableName = "dtINADJ";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetItm.TableName = "dtINADJDetItm";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                break;

        //            case (int)GEnum.SystemCode.Inventory_Production:
        //                headerTableName = "dtINMFN";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetItm.TableName = "dtINMFNDetItm";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                break;

        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //                headerTableName = "dtINTRN";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetItm.TableName = "dtINTRNDetItm";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                break;

        //            case (int)GEnum.SystemCode.Journal:
        //                headerTableName = "dtGLJNL";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetItm.TableName = "dtGLJNLDetItm";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                break;

        //            case (int)GEnum.SystemCode.Deposit:
        //                headerTableName = "dtGLDP";
        //                dtDetItm = ((DataTable)htDetail[GEnum.Details.Doc_Itm]).Copy();
        //                dtDetItm.TableName = "dtGLDPDetItm";
        //                dsDocumentUpdate.Tables.Add(dtDetItm);
        //                break;

        //            case (int)GEnum.SystemCode.Bank_Revaluation:
        //                headerTableName = "dtGLRV";
        //                break;

        //        }
        //        #endregion

        //        if (headerTableName == "")
        //        {
        //            MsgBox.Show(cn, MsgID.Document.DocumentCodeNotMatch);
        //            return false;
        //        }

        //        dtObject = GFunc.ConvertObjectToDataTable(objDoc, headerTableName);
        //        dsDocumentUpdate.Tables.Add(dtObject);

        //        string XMLformat = "";
        //        List<SqlParameter> paraList = new List<SqlParameter>();
        //        paraList.Add(new SqlParameter("@DocKey", objDoc.DocKey));
        //        paraList.Add(new SqlParameter("@DocCodeKey", objDoc.DocCodeKey));
        //        paraList.Add(new SqlParameter("@DocType", objDoc.DocType));
        //        paraList.Add(new SqlParameter("@GUID", objDoc.GUID));
        //        paraList.Add(new SqlParameter("@UserKey", AppInfor.currentUserKey));
        //        paraList.Add(new SqlParameter("@ButtonAction", ButtonAction));
        //        paraList.Add(new SqlParameter("@UpType", UpType));
        //        paraList.Add(new SqlParameter("@NewDocState", NewDocState));
        //        paraList.Add(new SqlParameter("@UpItmHis", UpItmHis));
        //        paraList.Add(new SqlParameter("@UpStock", UpStock));
        //        paraList.Add(new SqlParameter("@DocAutoID", DocAutoID));
        //        paraList.Add(new SqlParameter("@UpCust", UpCust));
        //        paraList.Add(new SqlParameter("@UpVend", UpVend));
        //        paraList.Add(new SqlParameter("@UpAcc", UpAcc));
        //        paraList.Add(new SqlParameter("@DocState", objDoc.DocState));

        //        #region Add dataTable parameter
        //        dsItmHis.TableName = "dtItmHis";
        //        XMLformat = GFunc.ConvertDataTableToXML(dsItmHis);
        //        paraList.Add(new SqlParameter("@xmlDocItmHis", XMLformat));

        //        dsItmHisBatch.TableName = "dtItmHisBatch";
        //        XMLformat = GFunc.ConvertDataTableToXML(dsItmHisBatch);
        //        paraList.Add(new SqlParameter("@xmlDocItmHisBatch", XMLformat));

        //        dtConHis.TableName = "dtConHis";
        //        XMLformat = GFunc.ConvertDataTableToXML(dtConHis);
        //        paraList.Add(new SqlParameter("@xmlDocConHis", XMLformat));

        //        if (dtPost == null || dtPost.Rows.Count == 0)
        //            dtPost = new DataTable();

        //        dtPost.TableName = "dtPost";
        //        XMLformat = GFunc.ConvertDataTableToXML(dtPost);
        //        paraList.Add(new SqlParameter("@xmlDocPost", XMLformat));

        //        DataTable dtSysLogBatch = new DataTable();
        //        if (SysOptionUtility.GetInt("InventoryValuationMethod", cn) == (int)GEnum.InventoryValuationMethod.COSBatchPosting)
        //            dtSysLogBatch = DTPostSysLogBatch(cn, objDoc, ButtonAction);

        //        dtSysLogBatch.TableName = "dtDocLogBatch";
        //        XMLformat = GFunc.ConvertDataTableToXML(dtSysLogBatch);
        //        paraList.Add(new SqlParameter("@xmlDocLogbatch", XMLformat));

        //        XMLformat = GFunc.ConvertDataTableToXML(dsDocumentUpdate);
        //        paraList.Add(new SqlParameter("@xmlDocDetail", XMLformat));
        //        #endregion

        //        int retval = 0;
        //        SqlParameter para = new SqlParameter("@RetValue", retval);
        //        para.Direction = ParameterDirection.Output;
        //        paraList.Add(para);
        //        GFunc.ExecuteNonQueryProc(cn, "[Doc_Save]", paraList);

        //        //return result
        //        if (GFunc.NEInt(para.Value, 0) == 1)
        //            return true;
        //        else
        //        {
        //            MsgBox.Show("Unable to post document");
        //            return false;
        //        }
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dtObject = null;
        //        dtDetPack = null;
        //        dtDetExp = null;
        //        dsDocumentUpdate = null;
        //    }
        //}//Completed

        ////Document Posting - GL Account Posting
        //private static DataTable DTPostAPBL(Document objDoc, DataTable dtItems)
        //{
        //    #region Declaration
        //    DataTable dtTaxGrpList = null;
        //    DataTable dtIsCustomGST = null;
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();

        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    int? DocCurrkey = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //    decimal? DocCurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //    int? DocCVKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
        //    string DocCVNmDoc = GFunc.GetStringPropertyValue("DocConNm", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    int? DocOverallDisAcc = GFunc.GetIntPropertyValue("DocOverallDisAcc", objDoc);
        //    decimal? DocOverallDisAmt = GFunc.GetDecimalPropertyValue("DocOverallDisAmt", objDoc);
        //    int? DocTaxGrpKey = GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc);
        //    decimal? DocTaxTotal = GFunc.GetDecimalPropertyValue("DocTaxTotal", objDoc);
        //    decimal? DocTaxGrpRate = GFunc.GetDecimalPropertyValue("DocTaxGrpRate", objDoc);
        //    int? DocPaidAccKey = GFunc.GetIntPropertyValue("DocPaidAccKey", objDoc);
        //    decimal? DocPaidAmtF = GFunc.GetDecimalPropertyValue("DocPaidAmtF", objDoc);
        //    decimal? DocGrand = GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //    decimal? DocHome = GFunc.GetDecimalPropertyValue("DocHome", objDoc);
        //    decimal? DocApplyGainAmt = GFunc.GetDecimalPropertyValue("DocApplyGainAmt", objDoc);

        //    int? AccGSTCustom = SysOptionUtility.GetInt("AccGSTCustom");//for Tax
        //    int? DocAccKey = GFunc.GetIntPropertyValue("DocAccKey", objDoc);
        //    int? DocAddCostAccKey = GFunc.GetIntPropertyValue("DocAddCostAccKey", objDoc);
        //    int? DocApplyGainAccKey = GFunc.GetIntPropertyValue("DocApplyGainAccKey", objDoc);
        //    #endregion

        //    try
        //    {
        //        //check DocID
        //        //if New set docAutoID
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;


        //        #region Post Tax
        //        if (!GFunc.IsNE(DocTaxGrpKey))
        //        {
        //            List<SqlParameter> parmList = new List<SqlParameter>();
        //            parmList.Add(new SqlParameter("@TaxGrpKey", DocTaxGrpKey));

        //            dtIsCustomGST = GFunc.ExecuteProc("Doc_IsCustomGST", parmList);


        //            if (dtIsCustomGST.Rows.Count != 0)
        //            {

        //                List<SqlParameter> TaxparmList = new List<SqlParameter>();
        //                TaxparmList.Add(new SqlParameter("@DocDate", objDoc.DocDate));
        //                TaxparmList.Add(new SqlParameter("@TaxGrpKey", DocTaxGrpKey));

        //                dtTaxGrpList = GFunc.ExecuteProc("Doc_GetTaxGrpList", TaxparmList);

        //                if (dtIsCustomGST.Rows[0]["GSTCustom"].ToString() == "1")//GSTCustom is true
        //                {
        //                    #region Post Custom Import Tax - Debit GST Controller and Credit Custom Import Tax Account
        //                    if (DocTaxTotal != 0)
        //                    {
        //                        if (dtTaxGrpList.Rows.Count != 0)
        //                        {
        //                            foreach (DataRow drTax in dtTaxGrpList.Rows)
        //                            {
        //                                dr = dtPost.NewRow();
        //                                dr["LogDC"] = objDoc.DocCodeKey;
        //                                dr["LogDK"] = objDoc.DocKey;
        //                                dr["LogDItm"] = 0;
        //                                dr["LogLineType"] = logLineType; //No Detail
        //                                dr["LogAccKey"] = drTax["AccKey"];
        //                                dr["LogTrans"] = PostType.TX;
        //                                dr["LogTaxKey"] = drTax["TaxKey"];
        //                                dr["LogSeq"] = logLineType;//Document Header
        //                                dr["DocID"] = docID;
        //                                dr["DocDate"] = objDoc.DocDate;
        //                                dr["DocPeriod"] = DocPeriod;
        //                                dr["DocBranchKey"] = DocBranchKey;
        //                                dr["DocDeptKey"] = DocDeptKey;
        //                                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                                dr["DocGrpKey"] = DocGrpKey;
        //                                dr["DocType"] = objDoc.DocType;
        //                                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                                dr["DocCurrkey"] = DocCurrkey;
        //                                dr["DocCurrRate"] = DocCurrRate;
        //                                dr["DocCVKey"] = DocCVKey;
        //                                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                                dr["DocChqNum"] = string.Empty;
        //                                dr["DocRef"] = DocRef;
        //                                dr["DocDes"] = DocDes;
        //                                dr["DetItmKey"] = 0;
        //                                dr["DetItmKeySelect"] = 0;
        //                                dr["DetItmDes"] = string.Empty;
        //                                dr["DetRef"] = drTax["TaxDes"];
        //                                dr["DetJobKey"] = 0;
        //                                dr["DetJobPhaseKey"] = 0;
        //                                dr["DetJobTaskKey"] = 0;
        //                                dr["DetJobCostTypeKey"] = 0;
        //                                dr["LogFC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "C");
        //                                dr["LogFD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "D");
        //                                dr["LogHC"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "C");
        //                                dr["LogHD"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "D");
        //                                dr["LogRecon"] = false;
        //                                dr["LogReconPeriod"] = 0;
        //                                dtPost.Rows.Add(dr);


        //                                dr = dtPost.NewRow();
        //                                dr["LogDC"] = objDoc.DocCodeKey;
        //                                dr["LogDK"] = objDoc.DocKey;
        //                                dr["LogDItm"] = 0;
        //                                dr["LogLineType"] = logLineType; //No Detail
        //                                dr["LogAccKey"] = AccGSTCustom;
        //                                dr["LogTrans"] = PostType.TXC;
        //                                dr["LogTaxKey"] = GFunc.NEInt(drTax["TaxKey"], 0);
        //                                dr["LogSeq"] = logLineType;//Document Header
        //                                dr["DocID"] = docID;
        //                                dr["DocDate"] = objDoc.DocDate;
        //                                dr["DocPeriod"] = DocPeriod;
        //                                dr["DocBranchKey"] = DocBranchKey;
        //                                dr["DocDeptKey"] = DocDeptKey;
        //                                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                                dr["DocGrpKey"] = DocGrpKey;
        //                                dr["DocType"] = objDoc.DocType;
        //                                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                                dr["DocCurrkey"] = DocCurrkey;
        //                                dr["DocCurrRate"] = DocCurrRate;
        //                                dr["DocCVKey"] = DocCVKey;
        //                                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                                dr["DocChqNum"] = string.Empty;
        //                                dr["DocRef"] = DocRef;
        //                                dr["DocDes"] = DocDes;
        //                                dr["DetItmKey"] = 0;
        //                                dr["DetItmKeySelect"] = 0;
        //                                dr["DetItmDes"] = string.Empty;
        //                                dr["DetRef"] = drTax["TaxDes"];
        //                                dr["DetJobKey"] = 0;
        //                                dr["DetJobPhaseKey"] = 0;
        //                                dr["DetJobTaskKey"] = 0;
        //                                dr["DetJobCostTypeKey"] = 0;
        //                                dr["LogFC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "C");
        //                                dr["LogFD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "D");
        //                                dr["LogHC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "C");
        //                                dr["LogHD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "D");
        //                                dr["LogRecon"] = false;
        //                                dr["LogReconPeriod"] = 0;
        //                                dtPost.Rows.Add(dr);

        //                            }
        //                        }
        //                    }
        //                    #endregion
        //                }
        //                else
        //                {
        //                    #region Post Tax
        //                    if (DocTaxTotal != 0)
        //                    {

        //                        if (dtTaxGrpList.Rows.Count != 0)
        //                        {

        //                            foreach (DataRow drTax in dtTaxGrpList.Rows)
        //                            {
        //                                dr = dtPost.NewRow();
        //                                dr["LogDC"] = objDoc.DocCodeKey;
        //                                dr["LogDK"] = objDoc.DocKey;
        //                                dr["LogDItm"] = 0;
        //                                dr["LogLineType"] = logLineType; //No Detail
        //                                dr["LogAccKey"] = drTax["AccKey"];//Ask Mic
        //                                dr["LogTrans"] = PostType.TX;
        //                                dr["LogTaxKey"] = drTax["TaxKey"];
        //                                dr["LogSeq"] = logLineType;//Document Header
        //                                dr["DocID"] = docID;
        //                                dr["DocDate"] = objDoc.DocDate;
        //                                dr["DocPeriod"] = DocPeriod;
        //                                dr["DocBranchKey"] = DocBranchKey;
        //                                dr["DocDeptKey"] = DocDeptKey;
        //                                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                                dr["DocGrpKey"] = DocGrpKey;
        //                                dr["DocType"] = objDoc.DocType;
        //                                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                                dr["DocCurrkey"] = DocCurrkey;
        //                                dr["DocCurrRate"] = DocCurrRate;
        //                                dr["DocCVKey"] = DocCVKey;
        //                                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                                dr["DocChqNum"] = string.Empty;
        //                                dr["DocRef"] = DocRef;
        //                                dr["DocDes"] = DocDes;
        //                                dr["DetItmKey"] = 0;
        //                                dr["DetItmKeySelect"] = 0;
        //                                dr["DetItmDes"] = string.Empty;
        //                                dr["DetRef"] = drTax["TaxDes"];
        //                                dr["DetJobKey"] = 0;
        //                                dr["DetJobPhaseKey"] = 0;
        //                                dr["DetJobTaskKey"] = 0;
        //                                dr["DetJobCostTypeKey"] = 0;
        //                                dr["LogFC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "C");
        //                                dr["LogFD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "D");
        //                                dr["LogHC"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "C");
        //                                dr["LogHD"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "D");
        //                                dr["LogRecon"] = false;
        //                                dr["LogReconPeriod"] = 0;
        //                                dtPost.Rows.Add(dr);
        //                            }
        //                        }
        //                    }
        //                    #endregion
        //                }
        //            }
        //        }
        //        #endregion

        //        #region Post Overall Discount
        //        if (GFunc.NEDec(DocOverallDisAmt, 0) != 0)
        //        {
        //            if (GFunc.NEInt(DocOverallDisAcc, 0) <= 0)
        //            {
        //                throw new TAException("Overall discount account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocOverallDisAcc;
        //            dr["LogTrans"] = PostType.DIS;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = GFunc.GetStringPropertyValue("DocPONum", objDoc);
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocOverallDisAmt * objDoc.DocSign, "C", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocOverallDisAmt * objDoc.DocSign, "C", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocCurrRate * DocOverallDisAmt * objDoc.DocSign, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocCurrRate * DocOverallDisAmt * objDoc.DocSign, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Payment BK
        //        if (GFunc.NEDec(DocPaidAmtF, 0) != 0)
        //        {
        //            if (GFunc.NEInt(DocPaidAccKey, 0) <= 0)
        //            {
        //                throw new TAException("Payment account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocPaidAccKey;
        //            dr["LogTrans"] = PostType.BK;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocPaidChqNum", objDoc);
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = GFunc.GetStringPropertyValue("DocPONum", objDoc);
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocPaidAmtF * objDoc.DocSign, "C", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocPaidAmtF * objDoc.DocSign, "C", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocPaidAmtF * objDoc.DocSign * DocCurrRate, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocPaidAmtF * objDoc.DocSign * DocCurrRate, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Purchase

        //        dtItems.DefaultView.RowFilter = "ItmType<=710 AND LineType=1000";//"Assembly" Or "Charges" Or "Discount" Or "Non Stock" Or "Service" Or "Stock" Or "Finished GD" Or "Finished GDB" Or "StockB" And LineType=Detail Items
        //        foreach (DataRowView drv in dtItems.DefaultView)
        //        {
        //            if (GFunc.NEDec(drv["ItmAmtH"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(drv["ItmAccKey"], 0) <= 0)
        //                {
        //                    throw new TAException("Detail account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = drv["LineType"];
        //                dr["LogAccKey"] = drv["ItmAccKey"];
        //                dr["LogTrans"] = PostType.ITM;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["ItmSN"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["ItmDeptKey"], 0);
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["ItmTranGrpKey"], 0);
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = drv["ItmKey"];
        //                dr["DetItmKeySelect"] = drv["ItmKeySelect"];
        //                dr["DetItmDes"] = drv["ItmDes"];
        //                dr["DetRef"] = drv["APPOID"];
        //                dr["DetJobKey"] = GFunc.NEInt(drv["ItmJobKey"], 0);
        //                dr["DetJobPhaseKey"] = GFunc.NEInt(drv["ItmJobPhaseKey"], 0);
        //                dr["DetJobTaskKey"] = GFunc.NEInt(drv["ItmJobTaskKey"], 0);
        //                dr["DetJobCostTypeKey"] = GFunc.NEInt(drv["ItmJobCostTypeKey"], 0);
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtF"], 0) + GFunc.NEDec(drv["ItmAddAmtF"], 0) * objDoc.DocSign, "D", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtF"], 0) + GFunc.NEDec(drv["ItmAddAmtF"], 0) * objDoc.DocSign, "D", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtH"], 0) + GFunc.NEDec(drv["ItmAddAmtH"], 0) * objDoc.DocSign, "D", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtH"], 0) + GFunc.NEDec(drv["ItmAddAmtH"], 0) * objDoc.DocSign, "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post Landed Cost


        //        var getTotalAddCost = from row in dtItems.AsEnumerable()
        //                              where row.Field<int>("LineType") == 1000   //filter for LineType=1000
        //                              group row by new
        //                              {
        //                                  dKey = row.Field<int>("DocKey"),

        //                              } into grp
        //                              select new
        //                              {
        //                                  DKey = grp.Key.dKey,
        //                                  ItmAddAmtF = grp.Sum(r => r.Field<decimal>("ItmAddAmtF")),
        //                                  ItmAddAmtH = grp.Sum(r => r.Field<decimal>("ItmAddAmtH"))
        //                              };


        //        foreach (var drv in getTotalAddCost)
        //        {
        //            if (GFunc.NEDec(drv.ItmAddAmtH, 0) != 0)
        //            {
        //                if (GFunc.NEInt(DocAddCostAccKey, 0) <= 0)
        //                {
        //                    throw new TAException("Landed cost account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = 0;
        //                dr["LogLineType"] = logLineType; //No Detail
        //                dr["LogAccKey"] = DocAddCostAccKey;
        //                dr["LogTrans"] = PostType.LDC;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = logLineType;//Document Header
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = DocDeptKey;
        //                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = GFunc.GetStringPropertyValue("DocPONum", objDoc);
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv.ItmAddAmtF, 0) * objDoc.DocSign, "C", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv.ItmAddAmtF, 0) * objDoc.DocSign, "C", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv.ItmAddAmtH, 0) * objDoc.DocSign, "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv.ItmAddAmtH, 0) * objDoc.DocSign, "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }

        //        #endregion

        //        #region Post AP
        //        if (GFunc.NEDec(DocHome, 0) != 0)
        //        {
        //            if (GFunc.NEInt(DocAccKey, 0) <= 0)
        //            {
        //                throw new Exception("AP account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;//Ask Mic
        //            dr["LogDK"] = objDoc.DocKey;//Ask Mic
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccKey;
        //            dr["LogTrans"] = PostType.AP;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = GFunc.GetStringPropertyValue("DocPONum", objDoc);
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "C", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "C", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome * objDoc.DocSign, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome * objDoc.DocSign, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Gain AP
        //        if (GFunc.NEDec(DocApplyGainAmt, 0) != 0)
        //        {
        //            if (GFunc.NEInt(DocAccKey, 0) <= 0)
        //            {
        //                throw new TAException("Gain account cannot be empty");
        //            }
        //            if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Credit_Note || objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Credit_Note)
        //            {
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = 0;
        //                dr["LogLineType"] = logLineType; //No Detail
        //                dr["LogAccKey"] = DocAccKey;
        //                dr["LogTrans"] = PostType.GLB;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = logLineType;//Document Header
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = DocDeptKey;
        //                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = GFunc.GetStringPropertyValue("DocApplyIVID", objDoc);
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = 0;
        //                dr["LogFD"] = 0;
        //                dr["LogHC"] = DocComUtility.PT(DocApplyGainAmt, "D", "C");
        //                dr["LogHD"] = DocComUtility.PT(DocApplyGainAmt, "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post Gain IC
        //        if (GFunc.NEDec(DocApplyGainAmt, 0) != 0)
        //        {
        //            if (GFunc.NEInt(DocApplyGainAccKey, 0) <= 0)
        //            {
        //                throw new TAException("Gain account cannot be empty");
        //            }
        //            if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Credit_Note || objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Credit_Note)
        //            {
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = 0;
        //                dr["LogLineType"] = logLineType; //No Detail
        //                dr["LogAccKey"] = DocApplyGainAccKey;
        //                dr["LogTrans"] = PostType.GLP;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = logLineType;//Document Header
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = DocDeptKey;
        //                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = 1;
        //                dr["DocCurrRate"] = 1;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = GFunc.GetStringPropertyValue("DocApplyIVID", objDoc);
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = 0;
        //                dr["LogFD"] = 0;
        //                dr["LogHC"] = DocComUtility.PT(DocApplyGainAmt, "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(DocApplyGainAmt, "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        DTPostRnd(objDoc, dtPost);
        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        if (dtItems != null)
        //        {
        //            dtItems.DefaultView.RowFilter = "";
        //            dtItems.DefaultView.RowFilter = "LineType =1000";
        //        }

        //        dr = null;
        //        dtIsCustomGST = null;
        //        dtItems = null;
        //    }

        //}
        //private static DataTable DTPostAPBL(SqlConnection cn, Document objDoc, DataTable dtItems)
        //{
        //    #region Declaration
        //    DataTable dtTaxGrpList = null;
        //    DataTable dtIsCustomGST = null;
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();

        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    int? DocCurrkey = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //    decimal? DocCurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //    int? DocCVKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
        //    string DocCVNmDoc = GFunc.GetStringPropertyValue("DocConNm", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    int? DocOverallDisAcc = GFunc.GetIntPropertyValue("DocOverallDisAcc", objDoc);
        //    decimal? DocOverallDisAmt = GFunc.GetDecimalPropertyValue("DocOverallDisAmt", objDoc);
        //    int? DocTaxGrpKey = GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc);
        //    decimal? DocTaxTotal = GFunc.GetDecimalPropertyValue("DocTaxTotal", objDoc);
        //    decimal? DocTaxGrpRate = GFunc.GetDecimalPropertyValue("DocTaxGrpRate", objDoc);
        //    int? DocPaidAccKey = GFunc.GetIntPropertyValue("DocPaidAccKey", objDoc);
        //    decimal? DocPaidAmtF = GFunc.GetDecimalPropertyValue("DocPaidAmtF", objDoc);
        //    decimal? DocGrand = GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //    decimal? DocHome = GFunc.GetDecimalPropertyValue("DocHome", objDoc);
        //    decimal? DocApplyGainAmt = GFunc.GetDecimalPropertyValue("DocApplyGainAmt", objDoc);

        //    int? AccGSTCustom = SysOptionUtility.GetInt("AccGSTCustom", cn);//for Tax
        //    int? DocAccKey = GFunc.GetIntPropertyValue("DocAccKey", objDoc);
        //    int? DocAddCostAccKey = GFunc.GetIntPropertyValue("DocAddCostAccKey", objDoc);
        //    int? DocApplyGainAccKey = GFunc.GetIntPropertyValue("DocApplyGainAccKey", objDoc);
        //    #endregion

        //    try
        //    {
        //        //check DocID
        //        //if New set docAutoID
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;


        //        #region Post Tax
        //        if (!GFunc.IsNE(DocTaxGrpKey))
        //        {
        //            List<SqlParameter> parmList = new List<SqlParameter>();
        //            parmList.Add(new SqlParameter("@TaxGrpKey", DocTaxGrpKey));

        //            dtIsCustomGST = GFunc.ExecuteProc(cn, "Doc_IsCustomGST", parmList);


        //            if (dtIsCustomGST.Rows.Count != 0)
        //            {

        //                List<SqlParameter> TaxparmList = new List<SqlParameter>();
        //                TaxparmList.Add(new SqlParameter("@DocDate", objDoc.DocDate));
        //                TaxparmList.Add(new SqlParameter("@TaxGrpKey", DocTaxGrpKey));

        //                dtTaxGrpList = GFunc.ExecuteProc(cn, "Doc_GetTaxGrpList", TaxparmList);

        //                if (dtIsCustomGST.Rows[0]["GSTCustom"].ToString() == "1")//GSTCustom is true
        //                {
        //                    #region Post Custom Import Tax - Debit GST Controller and Credit Custom Import Tax Account
        //                    if (DocTaxTotal != 0)
        //                    {
        //                        if (dtTaxGrpList.Rows.Count != 0)
        //                        {
        //                            foreach (DataRow drTax in dtTaxGrpList.Rows)
        //                            {
        //                                dr = dtPost.NewRow();
        //                                dr["LogDC"] = objDoc.DocCodeKey;
        //                                dr["LogDK"] = GFunc.NEInt(objDoc.DocKey, 0);//Ask Mic
        //                                dr["LogDItm"] = 0;
        //                                dr["LogLineType"] = logLineType; //No Detail
        //                                dr["LogAccKey"] = GFunc.NEInt(drTax["AccKey"], 0);//Ask Mic
        //                                dr["LogTrans"] = PostType.TX;
        //                                dr["LogTaxKey"] = GFunc.NEInt(drTax["TaxKey"], 0);//Ask Mic
        //                                dr["LogSeq"] = logLineType;//Document Header
        //                                dr["DocID"] = docID;
        //                                dr["DocDate"] = objDoc.DocDate;
        //                                dr["DocPeriod"] = DocPeriod;
        //                                dr["DocBranchKey"] = DocBranchKey;
        //                                dr["DocDeptKey"] = DocDeptKey;
        //                                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                                dr["DocGrpKey"] = DocGrpKey;
        //                                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                                dr["DocCurrkey"] = DocCurrkey;
        //                                dr["DocCurrRate"] = DocCurrRate;
        //                                dr["DocCVKey"] = DocCVKey;
        //                                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                                dr["DocChqNum"] = string.Empty;
        //                                dr["DocRef"] = DocRef;
        //                                dr["DocDes"] = DocDes;
        //                                dr["DetItmKey"] = 0;
        //                                dr["DetItmKeySelect"] = 0;
        //                                dr["DetItmDes"] = string.Empty;
        //                                dr["DetRef"] = drTax["TaxDes"];
        //                                dr["DetJobKey"] = 0;
        //                                dr["DetJobPhaseKey"] = 0;
        //                                dr["DetJobTaskKey"] = 0;
        //                                dr["DetJobCostTypeKey"] = 0;
        //                                dr["LogFC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "C");//Ask Mic
        //                                dr["LogFD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "D");//Ask Mic
        //                                dr["LogHC"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "C");//Ask Mic
        //                                dr["LogHD"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "D");//Ask Mic
        //                                dr["LogRecon"] = false;
        //                                dr["LogReconPeriod"] = 0;
        //                                dtPost.Rows.Add(dr);


        //                                dr = dtPost.NewRow();
        //                                dr["LogDC"] = objDoc.DocCodeKey;
        //                                dr["LogDK"] = objDoc.DocKey;
        //                                dr["LogDItm"] = 0;
        //                                dr["LogLineType"] = logLineType; //No Detail
        //                                dr["LogAccKey"] = AccGSTCustom;
        //                                dr["LogTrans"] = PostType.TXC;
        //                                dr["LogTaxKey"] = drTax["TaxKey"];
        //                                dr["LogSeq"] = logLineType;//Document Header
        //                                dr["DocID"] = docID;
        //                                dr["DocDate"] = objDoc.DocDate;
        //                                dr["DocPeriod"] = DocPeriod;
        //                                dr["DocBranchKey"] = DocBranchKey;
        //                                dr["DocDeptKey"] = DocDeptKey;
        //                                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                                dr["DocGrpKey"] = DocGrpKey;
        //                                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                                dr["DocCurrkey"] = DocCurrkey;
        //                                dr["DocCurrRate"] = DocCurrRate;
        //                                dr["DocCVKey"] = DocCVKey;
        //                                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                                dr["DocChqNum"] = string.Empty;
        //                                dr["DocRef"] = DocRef;
        //                                dr["DocDes"] = DocDes;
        //                                dr["DetItmKey"] = 0;
        //                                dr["DetItmKeySelect"] = 0;
        //                                dr["DetItmDes"] = string.Empty;
        //                                dr["DetRef"] = drTax["TaxDes"];
        //                                dr["DetJobKey"] = 0;
        //                                dr["DetJobPhaseKey"] = 0;
        //                                dr["DetJobTaskKey"] = 0;
        //                                dr["DetJobCostTypeKey"] = 0;
        //                                dr["LogFC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "C");//Ask Mic
        //                                dr["LogFD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "D");//Ask Mic
        //                                dr["LogHC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "C");//Ask Mic
        //                                dr["LogHD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "D");//Ask Mic
        //                                dr["LogRecon"] = false;
        //                                dr["LogReconPeriod"] = 0;
        //                                dtPost.Rows.Add(dr);

        //                            }
        //                        }
        //                    }
        //                    #endregion
        //                }
        //                else
        //                {
        //                    #region Post Tax
        //                    if (DocTaxTotal != 0)
        //                    {

        //                        if (dtTaxGrpList.Rows.Count != 0)
        //                        {

        //                            foreach (DataRow drTax in dtTaxGrpList.Rows)
        //                            {
        //                                dr = dtPost.NewRow();
        //                                dr["LogDC"] = objDoc.DocCodeKey;
        //                                dr["LogDK"] = objDoc.DocKey;
        //                                dr["LogDItm"] = 0;
        //                                dr["LogLineType"] = logLineType; //No Detail
        //                                dr["LogAccKey"] = drTax["AccKey"];
        //                                dr["LogTrans"] = PostType.TX;
        //                                dr["LogTaxKey"] = drTax["TaxKey"];
        //                                dr["LogSeq"] = logLineType;//Document Header
        //                                dr["DocID"] = docID;
        //                                dr["DocDate"] = objDoc.DocDate;
        //                                dr["DocPeriod"] = DocPeriod;
        //                                dr["DocBranchKey"] = DocBranchKey;
        //                                dr["DocDeptKey"] = DocDeptKey;
        //                                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                                dr["DocGrpKey"] = DocGrpKey;
        //                                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                                dr["DocCurrkey"] = DocCurrkey;
        //                                dr["DocCurrRate"] = DocCurrRate;
        //                                dr["DocCVKey"] = DocCVKey;
        //                                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                                dr["DocChqNum"] = string.Empty;
        //                                dr["DocRef"] = DocRef;
        //                                dr["DocDes"] = DocDes;
        //                                dr["DetItmKey"] = 0;
        //                                dr["DetItmKeySelect"] = 0;
        //                                dr["DetItmDes"] = string.Empty;
        //                                dr["DetRef"] = drTax["TaxDes"];
        //                                dr["DetJobKey"] = 0;
        //                                dr["DetJobPhaseKey"] = 0;
        //                                dr["DetJobTaskKey"] = 0;
        //                                dr["DetJobCostTypeKey"] = 0;
        //                                dr["LogFC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "C");
        //                                dr["LogFD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "D");
        //                                dr["LogHC"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "C");
        //                                dr["LogHD"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "D");
        //                                dr["LogRecon"] = false;
        //                                dr["LogReconPeriod"] = 0;
        //                                dtPost.Rows.Add(dr);
        //                            }
        //                        }
        //                    }
        //                    #endregion
        //                }
        //            }
        //        }
        //        #endregion

        //        #region Post Overall Discount
        //        if (GFunc.NEDec(DocOverallDisAmt, 0) != 0)
        //        {
        //            if (GFunc.NEInt(DocOverallDisAcc, 0) <= 0)
        //            {
        //                throw new TAException("Overall discount account cannot be empty");

        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocOverallDisAcc;
        //            dr["LogTrans"] = PostType.DIS;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = GFunc.GetStringPropertyValue("DocPONum", objDoc);
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocOverallDisAmt * objDoc.DocSign, "C", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocOverallDisAmt * objDoc.DocSign, "C", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocCurrRate * DocOverallDisAmt * objDoc.DocSign, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocCurrRate * DocOverallDisAmt * objDoc.DocSign, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Payment BK
        //        if (GFunc.NEDec(DocPaidAmtF, 0) != 0)
        //        {
        //            if (GFunc.NEInt(DocPaidAccKey, 0) <= 0)
        //            {
        //                throw new TAException("Payment account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocPaidAccKey;
        //            dr["LogTrans"] = PostType.BK;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocPaidChqNum", objDoc);
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = GFunc.GetStringPropertyValue("DocPONum", objDoc);
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocPaidAmtF * objDoc.DocSign, "C", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocPaidAmtF * objDoc.DocSign, "C", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocPaidAmtF * objDoc.DocSign * DocCurrRate, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocPaidAmtF * objDoc.DocSign * DocCurrRate, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Purchase

        //        dtItems.DefaultView.RowFilter = "ItmType<=710 AND LineType=1000";//"Assembly" Or "Charges" Or "Discount" Or "Non Stock" Or "Service" Or "Stock" Or "Finished GD" Or "Finished GDB" Or "StockB" And LineType=Detail Items
        //        foreach (DataRowView drv in dtItems.DefaultView)
        //        {
        //            if (GFunc.NEDec(drv["ItmAmtH"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(drv["ItmAccKey"], 0) < 0)
        //                {
        //                    throw new TAException("Detail account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = drv["LineType"];
        //                dr["LogAccKey"] = drv["ItmAccKey"];
        //                dr["LogTrans"] = PostType.ITM;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["ItmSN"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["ItmDeptKey"], 0);//Ask Mic
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["ItmTranGrpKey"], 0);//Ask Mic
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = GFunc.NEInt(drv["ItmKey"], 0);//Ask Mic
        //                dr["DetItmKeySelect"] = GFunc.NEInt(drv["ItmKeySelect"], 0);//Ask Mic
        //                dr["DetItmDes"] = drv["ItmDes"];
        //                dr["DetRef"] = drv["APPOID"];
        //                dr["DetJobKey"] = GFunc.NEInt(drv["ItmJobKey"], 0);//Ask Mic
        //                dr["DetJobPhaseKey"] = GFunc.NEInt(drv["ItmJobPhaseKey"], 0);//Ask Mic
        //                dr["DetJobTaskKey"] = GFunc.NEInt(drv["ItmJobTaskKey"], 0);//Ask Mic
        //                dr["DetJobCostTypeKey"] = GFunc.NEInt(drv["ItmJobCostTypeKey"], 0);//Ask Mic
        //                dr["LogFC"] = DocComUtility.PT((GFunc.NEDec(drv["ItmAmtF"], 0) + GFunc.NEDec(drv["ItmAddAmtF"], 0)) * objDoc.DocSign, "D", "C");//Ask Mic
        //                dr["LogFD"] = DocComUtility.PT((GFunc.NEDec(drv["ItmAmtF"], 0) + GFunc.NEDec(drv["ItmAddAmtF"], 0)) * objDoc.DocSign, "D", "D");//Ask Mic
        //                dr["LogHC"] = DocComUtility.PT((GFunc.NEDec(drv["ItmAmtH"], 0) + GFunc.NEDec(drv["ItmAddAmtH"], 0)) * objDoc.DocSign, "D", "C");//Ask Mic
        //                dr["LogHD"] = DocComUtility.PT((GFunc.NEDec(drv["ItmAmtH"], 0) + GFunc.NEDec(drv["ItmAddAmtH"], 0)) * objDoc.DocSign, "D", "D");//Ask Mic
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post Landed Cost


        //        var getTotalAddCost = from row in dtItems.AsEnumerable()
        //                              where row.Field<int>("LineType") == 1000   //filter for LineType=1000
        //                              group row by new
        //                              {
        //                                  dKey = row.Field<int>("DocKey"),

        //                              } into grp
        //                              select new
        //                              {
        //                                  DKey = grp.Key.dKey,
        //                                  ItmAddAmtF = grp.Sum(r => r.Field<decimal>("ItmAddAmtF")),
        //                                  ItmAddAmtH = grp.Sum(r => r.Field<decimal>("ItmAddAmtH"))
        //                              };


        //        foreach (var drv in getTotalAddCost)
        //        {
        //            if (GFunc.NEDec(drv.ItmAddAmtH, 0) != 0)
        //            {
        //                if (GFunc.NEInt(DocAddCostAccKey, 0) <= 0)
        //                {
        //                    throw new TAException("Landed cost account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = 0;
        //                dr["LogLineType"] = logLineType; //No Detail
        //                dr["LogAccKey"] = GFunc.NEInt(DocAddCostAccKey, 0);
        //                dr["LogTrans"] = PostType.LDC;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = logLineType;//Document Header
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = DocDeptKey;
        //                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = GFunc.GetStringPropertyValue("DocPONum", objDoc);
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv.ItmAddAmtF, 0) * objDoc.DocSign, "C", "C");//Ask Mic
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv.ItmAddAmtF, 0) * objDoc.DocSign, "C", "D");//Ask Mic
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv.ItmAddAmtH, 0) * objDoc.DocSign, "C", "C");//Ask Mic
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv.ItmAddAmtH, 0) * objDoc.DocSign, "C", "D");//Ask Mic
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }

        //        #endregion

        //        #region Post AP
        //        if (GFunc.NEDec(DocHome, 0) != 0)
        //        {
        //            if (GFunc.NEInt(DocAccKey, 0) <= 0)
        //            {
        //                throw new TAException("AP account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccKey;
        //            dr["LogTrans"] = PostType.AP;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = GFunc.GetStringPropertyValue("DocPONum", objDoc);
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "C", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "C", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome * objDoc.DocSign, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome * objDoc.DocSign, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Gain AP
        //        if (DocApplyGainAmt != 0)
        //        {
        //            if (GFunc.NEInt(DocAccKey, 0) <= 0)
        //            {
        //                throw new TAException("Gain account cannot be empty");
        //            }
        //            if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Credit_Note || objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Credit_Note)
        //            {
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = 0;
        //                dr["LogLineType"] = logLineType; //No Detail
        //                dr["LogAccKey"] = DocAccKey;
        //                dr["LogTrans"] = PostType.GLB;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = logLineType;//Document Header
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = DocDeptKey;
        //                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = GFunc.GetStringPropertyValue("DocApplyIVID", objDoc);
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = 0;
        //                dr["LogFD"] = 0;
        //                dr["LogHC"] = DocComUtility.PT(DocApplyGainAmt, "D", "C");
        //                dr["LogHD"] = DocComUtility.PT(DocApplyGainAmt, "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post Gain IC
        //        if (GFunc.NEDec(DocApplyGainAmt, 0) != 0)
        //        {
        //            if (GFunc.NEInt(DocApplyGainAccKey, 0) <= 0)
        //            {
        //                throw new TAException("Gain account cannot be empty");
        //            }
        //            if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Credit_Note || objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Credit_Note)
        //            {
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = 0;
        //                dr["LogLineType"] = logLineType; //No Detail
        //                dr["LogAccKey"] = DocApplyGainAccKey;
        //                dr["LogTrans"] = PostType.GLP;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = logLineType;//Document Header
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = DocDeptKey;
        //                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = 1;
        //                dr["DocCurrRate"] = 1;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = GFunc.GetStringPropertyValue("DocApplyIVID", objDoc);
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = 0;
        //                dr["LogFD"] = 0;
        //                dr["LogHC"] = DocComUtility.PT(DocApplyGainAmt, "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(DocApplyGainAmt, "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        DTPostRnd(cn, objDoc, dtPost);
        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        if (dtItems != null)
        //        {
        //            dtItems.DefaultView.RowFilter = "";
        //            dtItems.DefaultView.RowFilter = "LineType =1000";
        //        }
        //        dr = null;
        //        dtIsCustomGST = null;
        //        dtItems = null;
        //    }

        //}
        //private static DataTable DTPostAPPD(Document objDoc, DataTable dtItems)
        //{
        //    #region Declaration
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    int? DocCurrkey = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //    decimal? DocCurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //    int? DocCVKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
        //    string DocCVNmDoc = GFunc.GetStringPropertyValue("DocConNm", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    int? DocOverallDisAcc = GFunc.GetIntPropertyValue("DocOverallDisAcc", objDoc);
        //    decimal? DocOverallDisAmt = GFunc.GetDecimalPropertyValue("DocOverallDisAmt", objDoc);
        //    decimal? DocGrand = GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //    decimal? DocHome = GFunc.GetDecimalPropertyValue("DocHome", objDoc);

        //    int? DocAccKey = GFunc.GetIntPropertyValue("DocAccKey", objDoc);

        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        #region Post Overall Discount
        //        if (GFunc.NEDec(DocOverallDisAmt, 0) != 0)
        //        {
        //            if (GFunc.NEInt(DocOverallDisAcc, 0) <= 0)
        //            {
        //                throw new TAException("Overall discount account cannot be empty");

        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocOverallDisAcc;
        //            dr["LogTrans"] = PostType.DIS;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = GFunc.GetStringPropertyValue("DocPONum", objDoc);
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocOverallDisAmt * objDoc.DocSign, "C", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocOverallDisAmt * objDoc.DocSign, "C", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocCurrRate * DocOverallDisAmt * objDoc.DocSign, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocCurrRate * DocOverallDisAmt * objDoc.DocSign, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Itm Purchase

        //        dtItems.DefaultView.RowFilter = "ItmType<=710 AND LineType=1000";
        //        foreach (DataRowView drv in dtItems.DefaultView)
        //        {
        //            if (GFunc.NEDec(drv["ItmAmtH"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(drv["ItmAccKey"], 0) <= 0)
        //                {
        //                    throw new TAException("Detail account cannot be empty");
        //                    //return false;
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = drv["LineType"];
        //                dr["LogAccKey"] = drv["ItmAccKey"];
        //                dr["LogTrans"] = PostType.ITM;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = GFunc.NEDec(drv["ItmSN"], 0);//Ask Mic
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["ItmDeptKey"], 0);//Ask Mic
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["ItmTranGrpKey"], 0);//Ask Mic
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = drv["ItmKey"];
        //                dr["DetItmKeySelect"] = drv["ItmKeySelect"];
        //                dr["DetItmDes"] = drv["ItmDes"];
        //                dr["DetRef"] = drv["APPOID"];
        //                dr["DetJobKey"] = GFunc.NEInt(drv["ItmJobKey"], 0);//Ask Mic
        //                dr["DetJobPhaseKey"] = GFunc.NEInt(drv["ItmJobPhaseKey"], 0);//Ask Mic
        //                dr["DetJobTaskKey"] = GFunc.NEInt(drv["ItmJobTaskKey"], 0);//Ask Mic
        //                dr["DetJobCostTypeKey"] = GFunc.NEInt(drv["ItmJobCostTypeKey"], 0);//Ask Mic
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtF"], 0) * objDoc.DocSign, "D", "C");//Ask Mic
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtF"], 0) * objDoc.DocSign, "D", "D");//Ask Mic
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtH"], 0) * objDoc.DocSign, "D", "C");//Ask Mic
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtH"], 0) * objDoc.DocSign, "D", "D");//Ask Mic
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post AP Accrual
        //        if (DocHome != 0)
        //        {
        //            if (GFunc.NEInt(DocAccKey, 0) <= 0)
        //            {
        //                throw new TAException("GL account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccKey;
        //            dr["LogTrans"] = PostType.AP;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = GFunc.GetStringPropertyValue("DocPONum", objDoc);
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "D", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "D", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome * objDoc.DocSign, "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome * objDoc.DocSign, "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        DTPostRnd(objDoc, dtPost);

        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        if (dtItems != null)
        //        {
        //            dtItems.DefaultView.RowFilter = "";
        //            dtItems.DefaultView.RowFilter = "LineType =1000";
        //        }
        //        dr = null;
        //    }
        //}
        //private static DataTable DTPostAPPD(SqlConnection cn, Document objDoc, DataTable dtItems)
        //{
        //    #region Declaration
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    int? DocCurrkey = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //    decimal? DocCurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //    int? DocCVKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
        //    string DocCVNmDoc = GFunc.GetStringPropertyValue("DocConNm", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    int? DocOverallDisAcc = GFunc.GetIntPropertyValue("DocOverallDisAcc", objDoc);
        //    decimal? DocOverallDisAmt = GFunc.GetDecimalPropertyValue("DocOverallDisAmt", objDoc);
        //    decimal? DocGrand = GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //    decimal? DocHome = GFunc.GetDecimalPropertyValue("DocHome", objDoc);

        //    int? DocAccKey = GFunc.GetIntPropertyValue("DocAccKey", objDoc);

        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        #region Post Overall Discount
        //        if (GFunc.NEDec(DocOverallDisAmt, 0) != 0)
        //        {
        //            if (GFunc.NEInt(DocOverallDisAcc, 0) <= 0)
        //            {
        //                throw new TAException("Overall discount account cannot be empty");

        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocOverallDisAcc;
        //            dr["LogTrans"] = PostType.DIS;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = GFunc.GetStringPropertyValue("DocPONum", objDoc);
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocOverallDisAmt * objDoc.DocSign, "C", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocOverallDisAmt * objDoc.DocSign, "C", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocCurrRate * DocOverallDisAmt * objDoc.DocSign, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocCurrRate * DocOverallDisAmt * objDoc.DocSign, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Itm Purchase

        //        dtItems.DefaultView.RowFilter = "ItmType<=710 AND LineType=1000";
        //        foreach (DataRowView drv in dtItems.DefaultView)
        //        {
        //            if (GFunc.NEDec(drv["ItmAmtH"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(drv["ItmAccKey"], 0) <= 0)
        //                {
        //                    throw new TAException("Detail account cannot be empty");
        //                    //return false;
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = drv["LineType"];
        //                dr["LogAccKey"] = drv["ItmAccKey"];
        //                dr["LogTrans"] = PostType.ITM;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["ItmSN"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["ItmDeptKey"], 0);
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["ItmTranGrpKey"], 0);
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = drv["ItmKey"];
        //                dr["DetItmKeySelect"] = drv["ItmKeySelect"];
        //                dr["DetItmDes"] = drv["ItmDes"];
        //                dr["DetRef"] = drv["APPOID"];
        //                dr["DetJobKey"] = GFunc.NEInt(drv["ItmJobKey"], 0);
        //                dr["DetJobPhaseKey"] = GFunc.NEInt(drv["ItmJobPhaseKey"], 0);
        //                dr["DetJobTaskKey"] = GFunc.NEInt(drv["ItmJobTaskKey"], 0);
        //                dr["DetJobCostTypeKey"] = GFunc.NEInt(drv["ItmJobCostTypeKey"], 0);
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtF"], 0) * objDoc.DocSign, "D", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtF"], 0) * objDoc.DocSign, "D", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtH"], 0) * objDoc.DocSign, "D", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtH"], 0) * objDoc.DocSign, "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post AP Accrual
        //        if (DocHome != 0)
        //        {
        //            if (GFunc.NEInt(DocAccKey, 0) <= 0)
        //            {
        //                throw new TAException("GL account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccKey;
        //            dr["LogTrans"] = PostType.AP;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = GFunc.GetStringPropertyValue("DocPONum", objDoc);
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "C", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "C", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome * objDoc.DocSign, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome * objDoc.DocSign, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        DTPostRnd(cn, objDoc, dtPost);

        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        if (dtItems != null)
        //        {
        //            dtItems.DefaultView.RowFilter = "";
        //            dtItems.DefaultView.RowFilter = "LineType =1000";
        //        }
        //        dr = null;
        //    }
        //}
        //private static DataTable DTPostAPPY(Document objDoc, DataTable dtItems, DataTable dtExp)
        //{
        //    #region Declaration
        //    DataTable dtTaxGrpList = new DataTable();
        //    DataTable dtIsCustomGST = null;
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    int? DocCurrkey = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //    decimal? DocCurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //    int? DocCVKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
        //    string DocCVNmDoc = GFunc.GetStringPropertyValue("DocConNm", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    int? DocTaxGrpKey = GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc);
        //    decimal? DocTaxTotal = GFunc.GetDecimalPropertyValue("DocTaxTotal", objDoc);
        //    decimal? DocTaxGrpRate = GFunc.GetDecimalPropertyValue("DocTaxGrpRate", objDoc);
        //    decimal? DocGrand = GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //    decimal? DocHome = GFunc.GetDecimalPropertyValue("DocHome", objDoc);
        //    int? DocAccKey = GFunc.GetIntPropertyValue("DocAccKey", objDoc);
        //    int? AccGSTCustom = SysOptionUtility.GetInt("AccGSTCustom");//for Tax

        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        DataTable dtItem = dtItems.Copy();//Copy to Local DataTable and then Sort and Add Seq No.
        //        dtItem.Columns.Add("Seq", typeof(int));
        //        int seq = 1;
        //        dtItem.DefaultView.Sort = "LinkDocDate,LinkDocID";
        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            drv["Seq"] = seq;
        //            seq += 1;
        //        }


        //        #region Post Itm Discount AP Account

        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            if (GFunc.NEDec(drv["ItmApplyDisAmtH"], 0) != 0)//Ask Mic
        //            {
        //                if (GFunc.NEInt(drv["LinkDocAccKey"], 0) <= 0)//Ask Mic
        //                {
        //                    throw new TAException("Detail AP account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["LinkDocAccKey"];
        //                dr["LogTrans"] = PostType.ItmDisB;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["LinkDocDeptKey"], 0);//Ask Mic
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["LinkDocTranGrpKey"], 0);//Ask Mic
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtF"], 0), "D", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtF"], 0), "D", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtH"], 0), "D", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtH"], 0), "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);

        //            }
        //        }
        //        #endregion

        //        #region Post Itm Discount Expense Account

        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            if (GFunc.NEDec(drv["ItmApplyDisAmtH"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(drv["ItmApplyDisAccKey"], 0) <= 0)
        //                {
        //                    throw new TAException("Detail discount account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["ItmApplyDisAccKey"];
        //                dr["LogTrans"] = PostType.ItmDisP;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["LinkDocDeptKey"], 0);
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["LinkDocTranGrpKey"], 0);
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtF"], 0), "C", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtF"], 0), "C", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtH"], 0), "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtH"], 0), "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //                seq += 1;
        //            }
        //        }
        //        #endregion

        //        #region Post Itm Pay AP

        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            if (GFunc.NEDec(drv["ItmApplyDocAmtH"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(drv["LinkDocAccKey"], 0) <= 0)
        //                {
        //                    throw new TAException("Detail AP account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["LinkDocAccKey"];
        //                dr["LogTrans"] = PostType.ItmAP;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["LinkDocDeptKey"], 0);//Ask Mic
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["LinkDocTranGrpKey"], 0);//Ask Mic
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDocAmtF"], 0), "D", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDocAmtF"], 0), "D", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDocAmtH"], 0), "D", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDocAmtH"], 0), "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);

        //            }
        //        }
        //        #endregion

        //        #region Post Itm Loss Exp

        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            if (GFunc.NEDec(drv["ItmApplyGainAmt"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(drv["ItmApplyGainAccKey"], 0) <= 0)
        //                {
        //                    throw new TAException("Detail Loss account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["ItmApplyGainAccKey"];
        //                dr["LogTrans"] = PostType.ItmGLP;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["LinkDocDeptKey"], 0);
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["LinkDocTranGrpKey"], 0);
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = 0;
        //                dr["LogFD"] = 0;
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyGainAmt"], 0), "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyGainAmt"], 0), "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);

        //            }
        //        }
        //        #endregion

        //        #region Post Rec BK
        //        if (GFunc.NEDec(DocHome, 0) != 0)
        //        {
        //            if (GFunc.NEInt(DocAccKey, 0) <= 0)
        //            {
        //                throw new TAException("GL account cannot be empty");

        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccKey;
        //            dr["LogTrans"] = PostType.BK;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocGrand, "C", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand, "C", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Tax
        //        if (!GFunc.IsNE(DocTaxGrpKey))
        //        {
        //            List<SqlParameter> parmList = new List<SqlParameter>();
        //            parmList.Add(new SqlParameter("@TaxGrpKey", DocTaxGrpKey));

        //            dtIsCustomGST = GFunc.ExecuteProc("Doc_IsCustomGST", parmList);

        //            if (dtIsCustomGST.Rows.Count != 0)
        //            {
        //                List<SqlParameter> TaxparmList = new List<SqlParameter>();
        //                TaxparmList.Add(new SqlParameter("@DocDate", objDoc.DocDate));
        //                TaxparmList.Add(new SqlParameter("@TaxGrpKey", DocTaxGrpKey));

        //                dtTaxGrpList = GFunc.ExecuteProc("Doc_GetTaxGrpList", TaxparmList);

        //                if (GFunc.NEInt(dtIsCustomGST.Rows[0]["GSTCustom"], 0) == 1)//GSTCustom is true
        //                {
        //                    #region Post Custom Import Tax - Debit GST Controller and Credit Custom Import Tax Account
        //                    if (DocTaxTotal != 0)
        //                    {
        //                        if (dtTaxGrpList.Rows.Count != 0)
        //                        {
        //                            foreach (DataRow drTax in dtTaxGrpList.Rows)
        //                            {
        //                                dr = dtPost.NewRow();
        //                                dr["LogDC"] = objDoc.DocCodeKey;
        //                                dr["LogDK"] = objDoc.DocKey;
        //                                dr["LogDItm"] = 0;
        //                                dr["LogLineType"] = logLineType; //No Detail
        //                                dr["LogAccKey"] = drTax["AccKey"];
        //                                dr["LogTrans"] = PostType.TX;
        //                                dr["LogTaxKey"] = drTax["TaxKey"];
        //                                dr["LogSeq"] = logLineType;//Document Header
        //                                dr["DocID"] = docID;
        //                                dr["DocDate"] = objDoc.DocDate;
        //                                dr["DocPeriod"] = DocPeriod;
        //                                dr["DocBranchKey"] = DocBranchKey;
        //                                dr["DocDeptKey"] = DocDeptKey;
        //                                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                                dr["DocGrpKey"] = DocGrpKey;
        //                                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                                dr["DocCurrkey"] = DocCurrkey;
        //                                dr["DocCurrRate"] = DocCurrRate;
        //                                dr["DocCVKey"] = DocCVKey;
        //                                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                                dr["DocRef"] = DocRef;
        //                                dr["DocDes"] = DocDes;
        //                                dr["DetItmKey"] = 0;
        //                                dr["DetItmKeySelect"] = 0;
        //                                dr["DetItmDes"] = string.Empty;
        //                                dr["DetRef"] = drTax["TaxDes"];
        //                                dr["DetJobKey"] = 0;
        //                                dr["DetJobPhaseKey"] = 0;
        //                                dr["DetJobTaskKey"] = 0;
        //                                dr["DetJobCostTypeKey"] = 0;
        //                                dr["LogFC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "C");
        //                                dr["LogFD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "D");
        //                                dr["LogHC"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "C");
        //                                dr["LogHD"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "D");
        //                                dr["LogRecon"] = false;
        //                                dr["LogReconPeriod"] = 0;
        //                                dtPost.Rows.Add(dr);


        //                                dr = dtPost.NewRow();
        //                                dr["LogDC"] = objDoc.DocCodeKey;
        //                                dr["LogDK"] = objDoc.DocKey;
        //                                dr["LogDItm"] = 0;
        //                                dr["LogLineType"] = logLineType; //No Detail
        //                                dr["LogAccKey"] = AccGSTCustom;
        //                                dr["LogTrans"] = PostType.TXC;
        //                                dr["LogTaxKey"] = drTax["TaxKey"];
        //                                dr["LogSeq"] = logLineType;//Document Header
        //                                dr["DocID"] = docID;
        //                                dr["DocDate"] = objDoc.DocDate;
        //                                dr["DocPeriod"] = DocPeriod;
        //                                dr["DocBranchKey"] = DocBranchKey;
        //                                dr["DocDeptKey"] = DocDeptKey;
        //                                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                                dr["DocGrpKey"] = DocGrpKey;
        //                                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                                dr["DocCurrkey"] = DocCurrkey;
        //                                dr["DocCurrRate"] = DocCurrRate;
        //                                dr["DocCVKey"] = DocCVKey;
        //                                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                                dr["DocRef"] = DocRef;
        //                                dr["DocDes"] = DocDes;
        //                                dr["DetItmKey"] = 0;
        //                                dr["DetItmKeySelect"] = 0;
        //                                dr["DetItmDes"] = string.Empty;
        //                                dr["DetRef"] = drTax["TaxDes"];
        //                                dr["DetJobKey"] = 0;
        //                                dr["DetJobPhaseKey"] = 0;
        //                                dr["DetJobTaskKey"] = 0;
        //                                dr["DetJobCostTypeKey"] = 0;
        //                                dr["LogFC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "C");
        //                                dr["LogFD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "D");
        //                                dr["LogHC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "C");
        //                                dr["LogHD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "D");
        //                                dr["LogRecon"] = false;
        //                                dr["LogReconPeriod"] = 0;
        //                                dtPost.Rows.Add(dr);

        //                            }
        //                        }
        //                    }
        //                    #endregion
        //                }
        //                else
        //                {
        //                    #region Post Tax
        //                    if (DocTaxTotal != 0)
        //                    {

        //                        if (dtTaxGrpList.Rows.Count != 0)
        //                        {

        //                            foreach (DataRow drTax in dtTaxGrpList.Rows)
        //                            {
        //                                dr = dtPost.NewRow();
        //                                dr["LogDC"] = objDoc.DocCodeKey;
        //                                dr["LogDK"] = objDoc.DocKey;
        //                                dr["LogDItm"] = 0;
        //                                dr["LogLineType"] = logLineType; //No Detail
        //                                dr["LogAccKey"] = drTax["AccKey"];
        //                                dr["LogTrans"] = PostType.TX;
        //                                dr["LogTaxKey"] = drTax["TaxKey"];
        //                                dr["LogSeq"] = logLineType;//Document Header
        //                                dr["DocID"] = docID;
        //                                dr["DocDate"] = objDoc.DocDate;
        //                                dr["DocPeriod"] = DocPeriod;
        //                                dr["DocBranchKey"] = DocBranchKey;
        //                                dr["DocDeptKey"] = DocDeptKey;
        //                                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                                dr["DocGrpKey"] = DocGrpKey;
        //                                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                                dr["DocCurrkey"] = DocCurrkey;
        //                                dr["DocCurrRate"] = DocCurrRate;
        //                                dr["DocCVKey"] = DocCVKey;
        //                                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                                dr["DocRef"] = DocRef;
        //                                dr["DocDes"] = DocDes;
        //                                dr["DetItmKey"] = 0;
        //                                dr["DetItmKeySelect"] = 0;
        //                                dr["DetItmDes"] = string.Empty;
        //                                dr["DetRef"] = drTax["TaxDes"];
        //                                dr["DetJobKey"] = 0;
        //                                dr["DetJobPhaseKey"] = 0;
        //                                dr["DetJobTaskKey"] = 0;
        //                                dr["DetJobCostTypeKey"] = 0;
        //                                dr["LogFC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "C");
        //                                dr["LogFD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "D");
        //                                dr["LogHC"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "C");
        //                                dr["LogHD"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "D", "D");
        //                                dr["LogRecon"] = false;
        //                                dr["LogReconPeriod"] = 0;
        //                                dtPost.Rows.Add(dr);
        //                            }
        //                        }
        //                    }
        //                    #endregion
        //                }
        //            }
        //        }
        //        #endregion

        //        #region Post Itm  Exp

        //        foreach (DataRow drv in dtExp.Rows)
        //        {
        //            if (GFunc.NEDec(drv["ExpAmtH"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(drv["ExpAccKey"], 0) <= 0)
        //                {
        //                    throw new TAException("Detail account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["ExpAccKey"];
        //                dr["LogTrans"] = PostType.ItmExp;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["ExpSN"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["ExpDeptKey"], 0);
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["ExpTranGrpKey"], 0);
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = drv["ExpDes"];
        //                dr["DetRef"] = drv["ExpRef"];
        //                dr["DetJobKey"] = GFunc.NEInt(drv["ExpJobKey"], 0);
        //                dr["DetJobPhaseKey"] = GFunc.NEInt(drv["ExpJobPhaseKey"], 0);
        //                dr["DetJobTaskKey"] = GFunc.NEInt(drv["ExpJobTaskKey"], 0);
        //                dr["DetJobCostTypeKey"] = GFunc.NEInt(drv["ExpJobCostTypeKey"], 0);
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEInt(drv["ExpAmtF"], 0) * objDoc.DocSign, "D", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEInt(drv["ExpAmtF"], 0) * objDoc.DocSign, "D", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEInt(drv["ExpAmtH"], 0) * objDoc.DocSign, "D", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEInt(drv["ExpAmtH"], 0) * objDoc.DocSign, "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        DTPostRnd(objDoc, dtPost);
        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dtIsCustomGST = null;
        //    }
        //}
        //private static DataTable DTPostAPPY(SqlConnection cn, Document objDoc, DataTable dtItems, DataTable dtExp)
        //{
        //    #region Declaration
        //    DataTable dtTaxGrpList = new DataTable();
        //    DataTable dtIsCustomGST = null;
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    int? DocCurrkey = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //    decimal? DocCurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //    int? DocCVKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
        //    string DocCVNmDoc = GFunc.GetStringPropertyValue("DocConNm", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    int? DocTaxGrpKey = GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc);
        //    decimal? DocTaxTotal = GFunc.GetDecimalPropertyValue("DocTaxTotal", objDoc);
        //    decimal? DocTaxGrpRate = GFunc.GetDecimalPropertyValue("DocTaxGrpRate", objDoc);
        //    decimal? DocGrand = GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //    decimal? DocHome = GFunc.GetDecimalPropertyValue("DocHome", objDoc);
        //    int? DocAccKey = GFunc.GetIntPropertyValue("DocAccKey", objDoc);
        //    int? AccGSTCustom = SysOptionUtility.GetInt("AccGSTCustom", cn);//for Tax

        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        DataTable dtItem = new DataTable();
        //        GFunc.CopyDataTableToDetailObject(dtItems, dtItem);
        //        dtItem.Columns.Add("Seq", typeof(int));
        //        int seq = 1;
        //        dtItem.DefaultView.Sort = "LinkDocDate,LinkDocID";
        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            drv["Seq"] = seq;
        //            seq += 1;
        //        }


        //        #region Post Itm Discount AP Account

        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            if (GFunc.IsNE(drv["ItmApplyDisAmtH"])) continue;
        //            if (GFunc.NEDec(drv["ItmApplyDisAmtH"], 0) != 0)
        //            {
        //                if (!GFunc.IsNE(drv["LinkDocAccKey"]))
        //                {
        //                    if (GFunc.NEInt(drv["LinkDocAccKey"], 0) <= 0)
        //                    {
        //                        throw new TAException("Detail AP account cannot be empty");
        //                    }
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["LinkDocAccKey"];
        //                dr["LogTrans"] = PostType.ItmDisB;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["LinkDocDeptKey"], 0);//Ask Mic
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["LinkDocTranGrpKey"], 0);//Ask Mic
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = GFunc.NEStr(drv["LinkDocID"], 0);//Ask Mic
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtF"], 0), "D", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtF"], 0), "D", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtH"], 0), "D", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtH"], 0), "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);

        //            }
        //        }
        //        #endregion

        //        #region Post Itm Discount Expense Account

        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            if (GFunc.IsNE(drv["ItmApplyDisAmtH"])) continue;
        //            if (GFunc.NEDec(drv["ItmApplyDisAmtH"], 0) != 0)
        //            {
        //                if (!GFunc.IsNE(drv["ItmApplyDisAccKey"]))
        //                {
        //                    if (GFunc.NEInt(drv["ItmApplyDisAccKey"], 0) <= 0)
        //                    {
        //                        throw new TAException("Detail discount account cannot be empty");
        //                    }
        //                }

        //                if (GFunc.IsNE(drv["ItmApplyDisAmtH"])) continue;

        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["ItmApplyDisAccKey"];
        //                dr["LogTrans"] = PostType.ItmDisP;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["LinkDocDeptKey"], 0);//Ask Mic
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["LinkDocTranGrpKey"], 0);//Ask Mic
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtF"], 0), "C", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtF"], 0), "C", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtH"], 0), "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtH"], 0), "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //                seq += 1;
        //            }
        //        }
        //        #endregion

        //        #region Post Itm Pay AP

        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {

        //            if (GFunc.IsNE(drv["ItmApplyDocAmtH"])) continue;
        //            if (GFunc.NEDec(drv["ItmApplyDocAmtH"], 0) != 0)
        //            {
        //                if (!GFunc.IsNE(drv["LinkDocAccKey"]))
        //                {
        //                    if (GFunc.NEInt(drv["LinkDocAccKey"], 0) <= 0)
        //                    {
        //                        throw new TAException("Detail AP account cannot be empty");
        //                    }
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["LinkDocAccKey"];
        //                dr["LogTrans"] = PostType.ItmAP;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["LinkDocDeptKey"], 0);//Ask Mic
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["LinkDocTranGrpKey"], 0);//Ask Mic
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDocAmtF"], 0), "D", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDocAmtF"], 0), "D", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDocAmtH"], 0), "D", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDocAmtH"], 0), "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);

        //            }
        //        }
        //        #endregion

        //        #region Post Itm Loss Exp

        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {

        //            if (GFunc.IsNE(drv["ItmApplyGainAmt"])) continue;
        //            if (GFunc.NEDec(drv["ItmApplyGainAmt"], 0) != 0)
        //            {
        //                if (!GFunc.IsNE(drv["ItmApplyGainAccKey"]))
        //                {
        //                    if (GFunc.NEDec(drv["ItmApplyGainAccKey"], 0) <= 0)
        //                    {
        //                        throw new TAException("Detail AP account cannot be empty");
        //                    }

        //                }

        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["ItmApplyGainAccKey"];
        //                dr["LogTrans"] = PostType.ItmGLP;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["LinkDocDeptKey"], 0);
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["LinkDocTranGrpKey"], 0);
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = 0;
        //                dr["LogFD"] = 0;
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyGainAmt"], 0), "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyGainAmt"], 0), "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);

        //            }
        //        }
        //        #endregion

        //        #region Post Rec BK
        //        if (DocHome != 0)
        //        {
        //            if (GFunc.NEInt(DocAccKey, 0) <= 0)
        //            {
        //                throw new TAException("GL account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccKey;
        //            dr["LogTrans"] = PostType.BK;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocGrand, "C", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand, "C", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Tax
        //        if (!GFunc.IsNE(DocTaxGrpKey))
        //        {
        //            List<SqlParameter> parmList = new List<SqlParameter>();
        //            parmList.Add(new SqlParameter("@TaxGrpKey", DocTaxGrpKey));

        //            dtIsCustomGST = GFunc.ExecuteProc(cn, "Doc_IsCustomGST", parmList);

        //            if (dtIsCustomGST.Rows.Count != 0)
        //            {
        //                List<SqlParameter> TaxparmList = new List<SqlParameter>();
        //                TaxparmList.Add(new SqlParameter("@DocDate", objDoc.DocDate));
        //                TaxparmList.Add(new SqlParameter("@TaxGrpKey", DocTaxGrpKey));

        //                dtTaxGrpList = GFunc.ExecuteProc(cn, "Doc_GetTaxGrpList", TaxparmList);

        //                if ((bool)dtIsCustomGST.Rows[0]["GSTCustom"] == true)//GSTCustom is true
        //                {
        //                    #region Post Custom Import Tax - Debit GST Controller and Credit Custom Import Tax Account
        //                    if (DocTaxTotal != 0)
        //                    {
        //                        if (dtTaxGrpList.Rows.Count != 0)
        //                        {
        //                            foreach (DataRow drTax in dtTaxGrpList.Rows)
        //                            {
        //                                dr = dtPost.NewRow();
        //                                dr["LogDC"] = objDoc.DocCodeKey;
        //                                dr["LogDK"] = objDoc.DocKey;
        //                                dr["LogDItm"] = 0;
        //                                dr["LogLineType"] = logLineType; //No Detail
        //                                dr["LogAccKey"] = drTax["AccKey"];
        //                                dr["LogTrans"] = PostType.TX;
        //                                dr["LogTaxKey"] = drTax["TaxKey"];
        //                                dr["LogSeq"] = logLineType;//Document Header
        //                                dr["DocID"] = docID;
        //                                dr["DocDate"] = objDoc.DocDate;
        //                                dr["DocPeriod"] = DocPeriod;
        //                                dr["DocBranchKey"] = DocBranchKey;
        //                                dr["DocDeptKey"] = DocDeptKey;
        //                                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                                dr["DocGrpKey"] = DocGrpKey;
        //                                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                                dr["DocCurrkey"] = DocCurrkey;
        //                                dr["DocCurrRate"] = DocCurrRate;
        //                                dr["DocCVKey"] = DocCVKey;
        //                                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                                dr["DocRef"] = DocRef;
        //                                dr["DocDes"] = DocDes;
        //                                dr["DetItmKey"] = 0;
        //                                dr["DetItmKeySelect"] = 0;
        //                                dr["DetItmDes"] = string.Empty;
        //                                dr["DetRef"] = drTax["TaxDes"];
        //                                dr["DetJobKey"] = 0;
        //                                dr["DetJobPhaseKey"] = 0;
        //                                dr["DetJobTaskKey"] = 0;
        //                                dr["DetJobCostTypeKey"] = 0;
        //                                dr["LogFC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * -objDoc.DocSign, DocTaxGrpRate, "D", "C");
        //                                dr["LogFD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * -objDoc.DocSign, DocTaxGrpRate, "D", "D");
        //                                dr["LogHC"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * -objDoc.DocSign, DocTaxGrpRate, "D", "C");
        //                                dr["LogHD"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * -objDoc.DocSign, DocTaxGrpRate, "D", "D");
        //                                dr["LogRecon"] = false;
        //                                dr["LogReconPeriod"] = 0;
        //                                dtPost.Rows.Add(dr);


        //                                dr = dtPost.NewRow();
        //                                dr["LogDC"] = objDoc.DocCodeKey;
        //                                dr["LogDK"] = objDoc.DocKey;
        //                                dr["LogDItm"] = 0;
        //                                dr["LogLineType"] = logLineType; //No Detail
        //                                dr["LogAccKey"] = AccGSTCustom;
        //                                dr["LogTrans"] = PostType.TXC;
        //                                dr["LogTaxKey"] = drTax["TaxKey"];
        //                                dr["LogSeq"] = logLineType;//Document Header
        //                                dr["DocID"] = docID;
        //                                dr["DocDate"] = objDoc.DocDate;
        //                                dr["DocPeriod"] = DocPeriod;
        //                                dr["DocBranchKey"] = DocBranchKey;
        //                                dr["DocDeptKey"] = DocDeptKey;
        //                                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                                dr["DocGrpKey"] = DocGrpKey;
        //                                dr["DocType"] = objDoc.DocType;
        //                                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                                dr["DocCurrkey"] = DocCurrkey;
        //                                dr["DocCurrRate"] = DocCurrRate;
        //                                dr["DocCVKey"] = DocCVKey;
        //                                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                                dr["DocRef"] = DocRef;
        //                                dr["DocDes"] = DocDes;
        //                                dr["DetItmKey"] = 0;
        //                                dr["DetItmKeySelect"] = 0;
        //                                dr["DetItmDes"] = string.Empty;
        //                                dr["DetRef"] = drTax["TaxDes"];
        //                                dr["DetJobKey"] = 0;
        //                                dr["DetJobPhaseKey"] = 0;
        //                                dr["DetJobTaskKey"] = 0;
        //                                dr["DetJobCostTypeKey"] = 0;
        //                                dr["LogFC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * -objDoc.DocSign, DocTaxGrpRate, "C", "C");
        //                                dr["LogFD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * -objDoc.DocSign, DocTaxGrpRate, "C", "D");
        //                                dr["LogHC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * -objDoc.DocSign, DocTaxGrpRate, "C", "C");
        //                                dr["LogHD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * -objDoc.DocSign, DocTaxGrpRate, "C", "D");
        //                                dr["LogRecon"] = false;
        //                                dr["LogReconPeriod"] = 0;
        //                                dtPost.Rows.Add(dr);

        //                            }
        //                        }
        //                    }
        //                    #endregion
        //                }
        //                else
        //                {
        //                    #region Post Tax
        //                    if (DocTaxTotal != 0)
        //                    {

        //                        if (dtTaxGrpList.Rows.Count != 0)
        //                        {

        //                            foreach (DataRow drTax in dtTaxGrpList.Rows)
        //                            {
        //                                dr = dtPost.NewRow();
        //                                dr["LogDC"] = objDoc.DocCodeKey;
        //                                dr["LogDK"] = objDoc.DocKey;
        //                                dr["LogDItm"] = 0;
        //                                dr["LogLineType"] = logLineType; //No Detail
        //                                dr["LogAccKey"] = drTax["AccKey"];
        //                                dr["LogTrans"] = PostType.TX;
        //                                dr["LogTaxKey"] = drTax["TaxKey"];
        //                                dr["LogSeq"] = logLineType;//Document Header
        //                                dr["DocID"] = docID;
        //                                dr["DocDate"] = objDoc.DocDate;
        //                                dr["DocPeriod"] = DocPeriod;
        //                                dr["DocBranchKey"] = DocBranchKey;
        //                                dr["DocDeptKey"] = DocDeptKey;
        //                                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                                dr["DocGrpKey"] = DocGrpKey;
        //                                dr["DocType"] = objDoc.DocType;
        //                                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                                dr["DocCurrkey"] = DocCurrkey;
        //                                dr["DocCurrRate"] = DocCurrRate;
        //                                dr["DocCVKey"] = DocCVKey;
        //                                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                                dr["DocRef"] = DocRef;
        //                                dr["DocDes"] = DocDes;
        //                                dr["DetItmKey"] = 0;
        //                                dr["DetItmKeySelect"] = 0;
        //                                dr["DetItmDes"] = string.Empty;
        //                                dr["DetRef"] = drTax["TaxDes"];
        //                                dr["DetJobKey"] = 0;
        //                                dr["DetJobPhaseKey"] = 0;
        //                                dr["DetJobTaskKey"] = 0;
        //                                dr["DetJobCostTypeKey"] = 0;
        //                                dr["LogFC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * -objDoc.DocSign, DocTaxGrpRate, "D", "C");//Use Credit because DocSign is -ve
        //                                dr["LogFD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * -objDoc.DocSign, DocTaxGrpRate, "D", "D");
        //                                dr["LogHC"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * -objDoc.DocSign, DocTaxGrpRate, "D", "C");
        //                                dr["LogHD"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * -objDoc.DocSign, DocTaxGrpRate, "D", "D");
        //                                dr["LogRecon"] = false;
        //                                dr["LogReconPeriod"] = 0;
        //                                dtPost.Rows.Add(dr);
        //                            }
        //                        }
        //                    }
        //                    #endregion
        //                }
        //            }
        //        }
        //        #endregion

        //        #region Post Itm revenue

        //        foreach (DataRow drv in dtExp.Rows)
        //        {
        //            if (GFunc.NEInt(drv["ExpAmtH"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(drv["ExpAccKey"], 0) <= 0)
        //                {
        //                    throw new TAException("Detail account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["ExpAccKey"];
        //                dr["LogTrans"] = PostType.ItmExp;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["ExpSN"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["ExpDeptKey"], 0);//Ask Mic
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["ExpTranGrpKey"], 0);//Ask Mic
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = drv["ExpDes"];
        //                dr["DetRef"] = drv["ExpRef"];
        //                dr["DetJobKey"] = drv["ExpJobKey"];
        //                dr["DetJobPhaseKey"] = drv["ExpJobPhaseKey"];
        //                dr["DetJobTaskKey"] = drv["ExpJobTaskKey"];
        //                dr["DetJobCostTypeKey"] = drv["ExpJobCostTypeKey"];
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ExpAmtF"], 0) * -objDoc.DocSign, "D", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ExpAmtF"], 0) * -objDoc.DocSign, "D", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ExpAmtH"], 0) * -objDoc.DocSign, "D", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ExpAmtH"], 0) * -objDoc.DocSign, "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        DTPostRnd(cn, objDoc, dtPost);
        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dtIsCustomGST = null;
        //    }
        //}
        //private static DataTable DTPostAPRF(Document objDoc)
        //{
        //    #region Declaration

        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    int? DocCurrkey = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //    decimal? DocCurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //    int? DocCVKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
        //    string DocCVNmDoc = GFunc.GetStringPropertyValue("DocConNm", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    decimal? DocGrand = GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //    decimal? DocHome = GFunc.GetDecimalPropertyValue("DocHome", objDoc);
        //    int? DocAccAPKey = GFunc.GetIntPropertyValue("DocAccAPKey", objDoc);
        //    int? DocAccGLKey = GFunc.GetIntPropertyValue("DocAccGLKey", objDoc);
        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        #region Post Rec AP
        //        if (DocGrand != 0)
        //        {
        //            if (GFunc.NEInt(DocAccAPKey, 0) <= 0)
        //            {
        //                throw new TAException("AP account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccAPKey;
        //            dr["LogTrans"] = PostType.AP;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = GFunc.GetStringPropertyValue("DocJobKey", objDoc);
        //            dr["DetJobPhaseKey"] = GFunc.GetStringPropertyValue("DocJobPhaseKey", objDoc);
        //            dr["DetJobTaskKey"] = GFunc.GetStringPropertyValue("DocJobTaskKey", objDoc);
        //            dr["DetJobCostTypeKey"] = GFunc.GetStringPropertyValue("DocJobCostTypeKey", objDoc);
        //            dr["LogFC"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "C", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "C", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome * objDoc.DocSign, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome * objDoc.DocSign, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Rec BK
        //        if (DocHome != 0)
        //        {
        //            if (GFunc.NEInt(DocAccGLKey, 0) <= 0)
        //            {
        //                throw new TAException("GL account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccGLKey;
        //            dr["LogTrans"] = PostType.BK;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = GFunc.GetStringPropertyValue("DocJobKey", objDoc);
        //            dr["DetJobPhaseKey"] = GFunc.GetStringPropertyValue("DocJobPhaseKey", objDoc);
        //            dr["DetJobTaskKey"] = GFunc.GetStringPropertyValue("DocJobTaskKey", objDoc);
        //            dr["DetJobCostTypeKey"] = GFunc.GetStringPropertyValue("DocJobCostTypeKey", objDoc);
        //            dr["LogFC"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "D", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "D", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome * objDoc.DocSign, "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome * objDoc.DocSign, "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        DTPostRnd(objDoc, dtPost);
        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dr = null;
        //    }
        //}
        //private static DataTable DTPostAPRF(SqlConnection cn, Document objDoc)
        //{
        //    #region Declaration

        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    int? DocCurrkey = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //    decimal? DocCurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //    int? DocCVKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
        //    string DocCVNmDoc = GFunc.GetStringPropertyValue("DocConNm", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    decimal? DocGrand = GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //    decimal? DocHome = GFunc.GetDecimalPropertyValue("DocHome", objDoc);
        //    int? DocAccAPKey = GFunc.GetIntPropertyValue("DocAccAPKey", objDoc);
        //    int? DocAccGLKey = GFunc.GetIntPropertyValue("DocAccGLKey", objDoc);
        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        #region Post Rec AP
        //        if (DocGrand != 0)
        //        {
        //            if (GFunc.NEInt(DocAccAPKey, 0) <= 0)
        //            {
        //                throw new TAException("AP account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccAPKey;
        //            dr["LogTrans"] = PostType.AP;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = GFunc.GetStringPropertyValue("DocJobKey", objDoc);
        //            dr["DetJobPhaseKey"] = GFunc.GetStringPropertyValue("DocJobPhaseKey", objDoc);
        //            dr["DetJobTaskKey"] = GFunc.GetStringPropertyValue("DocJobTaskKey", objDoc);
        //            dr["DetJobCostTypeKey"] = GFunc.GetStringPropertyValue("DocJobCostTypeKey", objDoc);
        //            dr["LogFC"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "C", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "C", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome * objDoc.DocSign, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome * objDoc.DocSign, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Rec BK
        //        if (DocHome != 0)
        //        {
        //            if (GFunc.NEInt(DocAccGLKey, 0) <= 0)
        //            {
        //                throw new TAException("GL account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccGLKey;
        //            dr["LogTrans"] = PostType.BK;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = GFunc.GetStringPropertyValue("DocJobKey", objDoc);
        //            dr["DetJobPhaseKey"] = GFunc.GetStringPropertyValue("DocJobPhaseKey", objDoc);
        //            dr["DetJobTaskKey"] = GFunc.GetStringPropertyValue("DocJobTaskKey", objDoc);
        //            dr["DetJobCostTypeKey"] = GFunc.GetStringPropertyValue("DocJobCostTypeKey", objDoc);
        //            dr["LogFC"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "D", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "D", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome * objDoc.DocSign, "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome * objDoc.DocSign, "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        DTPostRnd(cn, objDoc, dtPost);
        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dr = null;
        //    }
        //}
        //private static DataTable DTPostARIV(Document objDoc, DataTable dtItems)
        //{
        //    #region Declaration
        //    string msgID = string.Empty;
        //    DataTable dtTaxGrpList = new DataTable();
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    int? DocCurrkey = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //    decimal? DocCurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //    int? DocCVKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
        //    string DocCVNmDoc = GFunc.GetStringPropertyValue("DocConNm", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    int? DocOverallDisAcc = GFunc.GetIntPropertyValue("DocOverallDisAcc", objDoc);
        //    decimal? DocOverallDisAmt = GFunc.GetDecimalPropertyValue("DocOverallDisAmt", objDoc);
        //    int? DocTaxGrpKey = GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc);
        //    decimal? DocTaxTotal = GFunc.GetDecimalPropertyValue("DocTaxTotal", objDoc);
        //    decimal? DocTaxGrpRate = GFunc.GetDecimalPropertyValue("DocTaxGrpRate", objDoc);
        //    int? DocPaidAccKey = GFunc.GetIntPropertyValue("DocPaidAccKey", objDoc);
        //    decimal? DocPaidAmtF = GFunc.GetDecimalPropertyValue("DocPaidAmtF", objDoc);
        //    decimal? DocGrand = GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //    decimal? DocHome = GFunc.GetDecimalPropertyValue("DocHome", objDoc);
        //    decimal? DocApplyGainAmt = GFunc.GetDecimalPropertyValue("DocApplyGainAmt", objDoc);
        //    int? DocAccKey = GFunc.GetIntPropertyValue("DocAccKey", objDoc);
        //    int? DocApplyGainAccKey = GFunc.GetIntPropertyValue("DocApplyGainAccKey", objDoc);
        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        #region Post Tax
        //        if (DocTaxTotal != 0)
        //        {

        //            List<SqlParameter> TaxparmList = new List<SqlParameter>();
        //            TaxparmList.Add(new SqlParameter("@DocDate", objDoc.DocDate));
        //            TaxparmList.Add(new SqlParameter("@TaxGrpKey", DocTaxGrpKey));

        //            dtTaxGrpList = GFunc.ExecuteProc("Doc_GetTaxGrpList", TaxparmList);


        //            if (dtTaxGrpList.Rows.Count != 0)
        //            {

        //                foreach (DataRow drTax in dtTaxGrpList.Rows)
        //                {
        //                    dr = dtPost.NewRow();
        //                    dr["LogDC"] = objDoc.DocCodeKey;
        //                    dr["LogDK"] = objDoc.DocKey;
        //                    dr["LogDItm"] = 0;
        //                    dr["LogLineType"] = logLineType; //No Detail
        //                    dr["LogAccKey"] = drTax["AccKey"];
        //                    dr["LogTrans"] = PostType.TX;
        //                    dr["LogTaxKey"] = drTax["TaxKey"];
        //                    dr["LogSeq"] = logLineType;//Document Header
        //                    dr["DocID"] = docID;
        //                    dr["DocDate"] = objDoc.DocDate;
        //                    dr["DocPeriod"] = DocPeriod;
        //                    dr["DocBranchKey"] = DocBranchKey;
        //                    dr["DocDeptKey"] = DocDeptKey;
        //                    dr["DocTranGrpKey"] = DocTranGrpKey;
        //                    dr["DocGrpKey"] = DocGrpKey;
        //                    dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                    dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                    dr["DocCurrkey"] = DocCurrkey;
        //                    dr["DocCurrRate"] = DocCurrRate;
        //                    dr["DocCVKey"] = DocCVKey;
        //                    dr["DocCVNmDoc"] = DocCVNmDoc;
        //                    dr["DocChqNum"] = string.Empty;
        //                    dr["DocRef"] = DocRef;
        //                    dr["DocDes"] = DocDes;
        //                    dr["DetItmKey"] = 0;
        //                    dr["DetItmKeySelect"] = 0;
        //                    dr["DetItmDes"] = string.Empty;
        //                    dr["DetRef"] = GFunc.NEInt(dtTaxGrpList.Rows[0]["TaxDes"], 0);//Ask Mic
        //                    dr["DetJobKey"] = 0;
        //                    dr["DetJobPhaseKey"] = 0;
        //                    dr["DetJobTaskKey"] = 0;
        //                    dr["DetJobCostTypeKey"] = 0;
        //                    dr["LogFC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "C");
        //                    dr["LogFD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "D");
        //                    dr["LogHC"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "C");
        //                    dr["LogHD"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "D");
        //                    dr["LogRecon"] = false;
        //                    dr["LogReconPeriod"] = 0;
        //                    dtPost.Rows.Add(dr);
        //                }
        //            }
        //        }
        //        #endregion

        //        #region Post Overall Discount
        //        if (DocOverallDisAmt != 0)
        //        {
        //            if (DocOverallDisAcc < 0)
        //            {
        //                throw new TAException("Overall Discount account cannot be empty");
        //                //return false;
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocOverallDisAcc;
        //            dr["LogTrans"] = PostType.DIS;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = "Discount";
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocOverallDisAmt * objDoc.DocSign, "D", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocOverallDisAmt * objDoc.DocSign, "D", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocCurrRate * DocOverallDisAmt * objDoc.DocSign, "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocCurrRate * DocOverallDisAmt * objDoc.DocSign, "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post AddHoc Payment
        //        if (DocPaidAmtF != 0)
        //        {
        //            if (GFunc.NEInt(DocPaidAccKey, 0) <= 0)
        //            {
        //                throw new TAException("Payment account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocPaidAccKey;
        //            dr["LogTrans"] = PostType.BK;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocPaidChqNum", objDoc);
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = GFunc.GetStringPropertyValue("DocSONum", objDoc);
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocPaidAmtF * objDoc.DocSign, "D", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocPaidAmtF * objDoc.DocSign, "D", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocCurrRate * DocPaidAmtF * objDoc.DocSign, "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocCurrRate * DocPaidAmtF * objDoc.DocSign, "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Itm

        //        dtItems.DefaultView.RowFilter = "ItmType<=710 AND LineType=1000";
        //        foreach (DataRowView drv in dtItems.DefaultView)
        //        {
        //            if (GFunc.NEDec(drv["ItmAmtH"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(drv["ItmAccKey"], 0) <= 0)
        //                {
        //                    throw new TAException("Detail account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = drv["LineType"];
        //                dr["LogAccKey"] = drv["ItmAccKey"];
        //                dr["LogTrans"] = PostType.ITM;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = GFunc.NEDec(drv["ItmSN"], 0);
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["ItmDeptKey"], 0);
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["ItmTranGrpKey"], 0);
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = drv["ItmKey"];
        //                dr["DetItmKeySelect"] = drv["ItmKeySelect"];
        //                dr["DetItmDes"] = drv["ItmDes"];
        //                dr["DetRef"] = string.Empty;
        //                dr["DetJobKey"] = GFunc.NEInt(drv["ItmJobKey"], 0);
        //                dr["DetJobPhaseKey"] = GFunc.NEInt(drv["ItmJobPhaseKey"], 0);
        //                dr["DetJobTaskKey"] = GFunc.NEInt(drv["ItmJobTaskKey"], 0);
        //                dr["DetJobCostTypeKey"] = GFunc.NEInt(drv["ItmJobCostTypeKey"], 0);
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtF"], 0) * objDoc.DocSign, "C", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtF"], 0) * objDoc.DocSign, "C", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtH"], 0) * objDoc.DocSign, "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtH"], 0) * objDoc.DocSign, "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post AR
        //        if (DocHome != 0)
        //        {
        //            if (GFunc.NEInt(DocAccKey, 0) <= 0)
        //            {
        //                throw new TAException("AR Account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccKey;
        //            dr["LogTrans"] = PostType.AR;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = GFunc.GetStringPropertyValue("DocSONum", objDoc);
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "D", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "D", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome * objDoc.DocSign, "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome * objDoc.DocSign, "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Gain AR
        //        if (DocApplyGainAmt != 0)
        //        {
        //            if (GFunc.NEInt(DocAccKey, 0) <= 0)
        //            {
        //                throw new TAException("Gain Account cannot be empty");
        //            }
        //            if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Credit_Note || objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Credit_Note)
        //            {
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = 0;
        //                dr["LogLineType"] = logLineType; //No Detail
        //                dr["LogAccKey"] = DocAccKey;
        //                dr["LogTrans"] = PostType.GLB;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = logLineType;//Document Header
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = DocDeptKey;
        //                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = GFunc.GetStringPropertyValue("DocApplyIVID", objDoc);
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = 0;
        //                dr["LogFD"] = 0;
        //                dr["LogHC"] = DocComUtility.PT(DocApplyGainAmt * objDoc.DocSign, "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(DocApplyGainAmt * objDoc.DocSign, "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post Gain IC
        //        if (DocApplyGainAmt != 0)
        //        {
        //            if (GFunc.NEInt(DocApplyGainAccKey, 0) <= 0)
        //            {
        //                throw new TAException("Gain Account cannot be empty");
        //            }
        //            if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Credit_Note || objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Credit_Note)
        //            {
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = 0;
        //                dr["LogLineType"] = logLineType; //No Detail
        //                dr["LogAccKey"] = DocApplyGainAccKey;
        //                dr["LogTrans"] = PostType.GLP;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = logLineType;//Document Header
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = DocDeptKey;
        //                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = GFunc.GetStringPropertyValue("DocApplyIVID", objDoc);
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = 0;
        //                dr["LogFD"] = 0;
        //                dr["LogHC"] = DocComUtility.PT(DocApplyGainAmt * objDoc.DocSign, "D", "C");
        //                dr["LogHD"] = DocComUtility.PT(DocApplyGainAmt * objDoc.DocSign, "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        DTPostRnd(objDoc, dtPost);

        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        if (dtItems != null)
        //        {
        //            dtItems.DefaultView.RowFilter = "";
        //            dtItems.DefaultView.RowFilter = "LineType =1000";
        //        }

        //        dr = null;
        //    }
        //}
        //private static DataTable DTPostARIV(SqlConnection cn, Document objDoc, DataTable dtItems)
        //{
        //    #region Declaration
        //    DataTable dtTaxGrpList = new DataTable();
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion



        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    int? DocCurrkey = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //    decimal? DocCurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //    int? DocCVKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
        //    string DocCVNmDoc = GFunc.GetStringPropertyValue("DocConNm", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    int? DocOverallDisAcc = GFunc.GetIntPropertyValue("DocOverallDisAcc", objDoc);
        //    decimal? DocOverallDisAmt = GFunc.GetDecimalPropertyValue("DocOverallDisAmt", objDoc);
        //    int? DocTaxGrpKey = GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc);
        //    decimal? DocTaxTotal = GFunc.GetDecimalPropertyValue("DocTaxTotal", objDoc);
        //    decimal? DocTaxGrpRate = GFunc.GetDecimalPropertyValue("DocTaxGrpRate", objDoc);
        //    int? DocPaidAccKey = GFunc.GetIntPropertyValue("DocPaidAccKey", objDoc);
        //    decimal? DocPaidAmtF = GFunc.GetDecimalPropertyValue("DocPaidAmtF", objDoc);
        //    decimal? DocGrand = GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //    decimal? DocHome = GFunc.GetDecimalPropertyValue("DocHome", objDoc);
        //    decimal? DocApplyGainAmt = GFunc.GetDecimalPropertyValue("DocApplyGainAmt", objDoc);
        //    int? DocAccKey = GFunc.GetIntPropertyValue("DocAccKey", objDoc);
        //    int? DocApplyGainAccKey = GFunc.GetIntPropertyValue("DocApplyGainAccKey", objDoc);
        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        #region Post Tax
        //        if (DocTaxTotal != 0)
        //        {

        //            List<SqlParameter> TaxparmList = new List<SqlParameter>();
        //            TaxparmList.Add(new SqlParameter("@DocDate", objDoc.DocDate));
        //            TaxparmList.Add(new SqlParameter("@TaxGrpKey", DocTaxGrpKey));

        //            dtTaxGrpList = GFunc.ExecuteProc(cn, "Doc_GetTaxGrpList", TaxparmList);

        //            //To Ammend for any other tax posting
        //            if (dtTaxGrpList.Rows.Count != 0)
        //            {

        //                foreach (DataRow drTax in dtTaxGrpList.Rows)
        //                {
        //                    dr = dtPost.NewRow();
        //                    dr["LogDC"] = objDoc.DocCodeKey;
        //                    dr["LogDK"] = objDoc.DocKey;
        //                    dr["LogDItm"] = 0;
        //                    dr["LogLineType"] = logLineType; //No Detail
        //                    dr["LogAccKey"] = drTax["AccKey"];
        //                    dr["LogTrans"] = PostType.TX;
        //                    dr["LogTaxKey"] = drTax["TaxKey"];
        //                    dr["LogSeq"] = logLineType;//Document Header
        //                    dr["DocID"] = docID;
        //                    dr["DocDate"] = objDoc.DocDate;
        //                    dr["DocPeriod"] = DocPeriod;
        //                    dr["DocBranchKey"] = DocBranchKey;
        //                    dr["DocDeptKey"] = DocDeptKey;
        //                    dr["DocTranGrpKey"] = DocTranGrpKey;
        //                    dr["DocGrpKey"] = DocGrpKey;
        //                    dr["DocType"] = objDoc.DocType;
        //                    dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                    dr["DocCurrkey"] = DocCurrkey;
        //                    dr["DocCurrRate"] = DocCurrRate;
        //                    dr["DocCVKey"] = DocCVKey;
        //                    dr["DocCVNmDoc"] = DocCVNmDoc;
        //                    dr["DocChqNum"] = string.Empty;
        //                    dr["DocRef"] = DocRef;
        //                    dr["DocDes"] = DocDes;
        //                    dr["DetItmKey"] = 0;
        //                    dr["DetItmKeySelect"] = 0;
        //                    dr["DetItmDes"] = string.Empty;
        //                    dr["DetRef"] = dtTaxGrpList.Rows[0]["TaxDes"];
        //                    dr["DetJobKey"] = 0;
        //                    dr["DetJobPhaseKey"] = 0;
        //                    dr["DetJobTaskKey"] = 0;
        //                    dr["DetJobCostTypeKey"] = 0;
        //                    dr["LogFC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "C");//Ask Mic
        //                    dr["LogFD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "D");//Ask Mic
        //                    dr["LogHC"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "C");//Ask Mic
        //                    dr["LogHD"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "D");//Ask Mic
        //                    dr["LogRecon"] = false;
        //                    dr["LogReconPeriod"] = 0;
        //                    dtPost.Rows.Add(dr);
        //                }
        //            }
        //        }
        //        #endregion

        //        #region Post Overall Discount
        //        if (DocOverallDisAmt != 0)
        //        {

        //            if (GFunc.NEInt(DocOverallDisAcc, 0) <= 0)
        //            {
        //                throw new TAException("Overall Discount account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocOverallDisAcc;
        //            dr["LogTrans"] = PostType.DIS;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = "Discount";
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocOverallDisAmt * objDoc.DocSign, "D", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocOverallDisAmt * objDoc.DocSign, "D", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocCurrRate * DocOverallDisAmt * objDoc.DocSign, "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocCurrRate * DocOverallDisAmt * objDoc.DocSign, "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post AddHoc Payment
        //        if (DocPaidAmtF != 0)
        //        {
        //            if (GFunc.NEInt(DocPaidAccKey, 0) <= 0)
        //            {
        //                throw new TAException("Paid Account cannot be empty");

        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocPaidAccKey;
        //            dr["LogTrans"] = PostType.BK;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocPaidChqNum", objDoc);
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = GFunc.GetStringPropertyValue("DocSONum", objDoc);
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocPaidAmtF * objDoc.DocSign, "D", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocPaidAmtF * objDoc.DocSign, "D", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocCurrRate * DocPaidAmtF * objDoc.DocSign, "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocCurrRate * DocPaidAmtF * objDoc.DocSign, "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Itm

        //        dtItems.DefaultView.RowFilter = "ItmType<=710 AND LineType=1000";
        //        foreach (DataRowView drv in dtItems.DefaultView)
        //        {
        //            if (GFunc.NEDec(drv["ItmAmtH"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(drv["ItmAccKey"], 0) <= 0)
        //                {
        //                    throw new TAException("Detail Account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = drv["LineType"];
        //                dr["LogAccKey"] = drv["ItmAccKey"];
        //                dr["LogTrans"] = PostType.ITM;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["ItmSN"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["ItmDeptKey"], 0);
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["ItmTranGrpKey"], 0);
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = drv["ItmKey"];
        //                dr["DetItmKeySelect"] = drv["ItmKeySelect"];
        //                dr["DetItmDes"] = drv["ItmDes"];
        //                dr["DetRef"] = string.Empty;
        //                dr["DetJobKey"] = GFunc.NEInt(drv["ItmJobKey"], 0);
        //                dr["DetJobPhaseKey"] = GFunc.NEInt(drv["ItmJobPhaseKey"], 0);
        //                dr["DetJobTaskKey"] = GFunc.NEInt(drv["ItmJobTaskKey"], 0);
        //                dr["DetJobCostTypeKey"] = GFunc.NEInt(drv["ItmJobCostTypeKey"], 0);
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtF"], 0) * objDoc.DocSign, "C", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtF"], 0) * objDoc.DocSign, "C", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtH"], 0) * objDoc.DocSign, "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmAmtH"], 0) * objDoc.DocSign, "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post AR
        //        if (DocHome != 0)
        //        {
        //            if (GFunc.NEInt(DocAccKey, 0) <= 0)
        //            {
        //                throw new TAException("AR Account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccKey;
        //            dr["LogTrans"] = PostType.AR;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = GFunc.GetStringPropertyValue("DocSONum", objDoc);
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "D", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "D", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome * objDoc.DocSign, "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome * objDoc.DocSign, "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Gain AR
        //        if (DocApplyGainAmt != 0)
        //        {
        //            if (GFunc.NEInt(DocAccKey, 0) <= 0)
        //            {
        //                throw new TAException("Gain account cannot be empty");
        //            }
        //            if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Credit_Note || objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Credit_Note)
        //            {
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = 0;
        //                dr["LogLineType"] = logLineType; //No Detail
        //                dr["LogAccKey"] = DocAccKey;
        //                dr["LogTrans"] = PostType.GLB;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = logLineType;//Document Header
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = DocDeptKey;
        //                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = GFunc.GetStringPropertyValue("DocApplyIVID", objDoc);
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = 0;
        //                dr["LogFD"] = 0;
        //                dr["LogHC"] = DocComUtility.PT(DocApplyGainAmt * objDoc.DocSign, "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(DocApplyGainAmt * objDoc.DocSign, "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post Gain IC
        //        if (DocApplyGainAmt != 0)
        //        {
        //            if (GFunc.NEInt(DocApplyGainAccKey, 0) <= 0)
        //            {
        //                throw new TAException("Gain account cannot be empty");
        //            }
        //            if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Credit_Note || objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Credit_Note)
        //            {
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = 0;
        //                dr["LogLineType"] = logLineType; //No Detail
        //                dr["LogAccKey"] = DocApplyGainAccKey;
        //                dr["LogTrans"] = PostType.GLP;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = logLineType;//Document Header
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = DocDeptKey;
        //                dr["DocTranGrpKey"] = DocTranGrpKey;
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = GFunc.GetStringPropertyValue("DocApplyIVID", objDoc);
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = 0;
        //                dr["LogFD"] = 0;
        //                dr["LogHC"] = DocComUtility.PT(DocApplyGainAmt * objDoc.DocSign, "D", "C");
        //                dr["LogHD"] = DocComUtility.PT(DocApplyGainAmt * objDoc.DocSign, "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        DTPostRnd(cn, objDoc, dtPost);
        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        if (dtItems != null)
        //        {
        //            dtItems.DefaultView.RowFilter = "";
        //            dtItems.DefaultView.RowFilter = "LineType =1000";
        //        }
        //        dr = null;
        //    }
        //}
        //private static DataTable DTPostARPY(Document objDoc, DataTable dtItems, DataTable dtExp)
        //{
        //    #region Declaration

        //    DataRow dr = null;
        //    DataTable dtTaxGrpList = new DataTable();
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));//Ask mic
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    int? DocCurrkey = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //    decimal? DocCurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //    int? DocCVKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
        //    string DocCVNmDoc = GFunc.GetStringPropertyValue("DocConNm", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    int? DocTaxGrpKey = GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc);
        //    decimal? DocTaxTotal = GFunc.GetDecimalPropertyValue("DocTaxTotal", objDoc);
        //    decimal? DocTaxGrpRate = GFunc.GetDecimalPropertyValue("DocTaxGrpRate", objDoc);
        //    decimal? DocGrand = GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //    decimal? DocHome = GFunc.GetDecimalPropertyValue("DocHome", objDoc);
        //    int? DocAccKey = GFunc.GetIntPropertyValue("DocAccKey", objDoc);
        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        DataTable dtItem = dtItems.Copy();//Copy to Local DataTable and then Sort and Add Seq No.
        //        dtItem.Columns.Add("Seq", typeof(int));
        //        int seq = 1;
        //        dtItem.DefaultView.Sort = "LinkDocDate,LinkDocID";
        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            drv["Seq"] = seq;
        //            seq += 1;
        //        }

        //        #region Post Itm Discount AR Account


        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            if (GFunc.IsNE(drv["ItmApplyDisAmtH"])) continue;
        //            if (GFunc.NEDec(drv["ItmApplyDisAmtH"], 0) != 0)
        //            {
        //                if (!GFunc.IsNE(drv["LinkDocAccKey"]))
        //                {
        //                    if (GFunc.NEInt(drv["LinkDocAccKey"], 0) <= 0)
        //                    {
        //                        throw new TAException("Detail AR account cannot be empty");
        //                    }
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["LinkDocAccKey"];
        //                dr["LogTrans"] = PostType.ItmDisB;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["LinkDocDeptKey"], 0);
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["LinkDocTranGrpKey"], 0);
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = objDoc.DocID;
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtF"], 0), "C", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtF"], 0), "C", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtH"], 0), "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtH"], 0), "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post Itm Discount Exp Account

        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            if (GFunc.IsNE(drv["ItmApplyDisAmtH"])) continue;
        //            if (GFunc.NEDec(drv["ItmApplyDisAmtH"], 0) != 0)
        //            {
        //                if (!GFunc.IsNE(drv["ItmApplyDisAccKey"]))
        //                {
        //                    if (GFunc.NEInt(drv["ItmApplyDisAccKey"], 0) <= 0)
        //                    {
        //                        throw new TAException("Detail discount account cannot be empty");
        //                    }
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = GFunc.NEInt(drv["ItmApplyDisAccKey"], 0);
        //                dr["LogTrans"] = PostType.ItmDisP;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["LinkDocDeptKey"], 0);
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["LinkDocTranGrpKey"], 0);
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = objDoc.DocID;
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtF"], 0), "D", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtF"], 0), "D", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtH"], 0), "D", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtH"], 0), "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post Itm Pay AR

        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            if (GFunc.IsNE(drv["ItmApplyDocAmtH"])) continue;
        //            if (GFunc.NEDec(drv["ItmApplyDocAmtH"], 0) != 0)
        //            {
        //                if (!GFunc.IsNE(drv["LinkDocAccKey"]))
        //                {
        //                    if (GFunc.NEInt(drv["LinkDocAccKey"], 0) <= 0)
        //                    {
        //                        throw new TAException("Detail AR account cannot be empty");
        //                    }
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];//Ask Mic
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = GFunc.NEInt(drv["LinkDocAccKey"], 0);//Ask Mic
        //                dr["LogTrans"] = PostType.ItmAR;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["LinkDocDeptKey"], 0);//Ask Mic
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["LinkDocTranGrpKey"], 0);//Ask Mic
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDocAmtF"], 0), "C", "C");//Ask Mic
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDocAmtF"], 0), "C", "D");//Ask Mic
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDocAmtH"], 0), "C", "C");//Ask Mic
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDocAmtH"], 0), "C", "D");//Ask Mic
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post Itm Gain IC

        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            if (GFunc.IsNE(drv["ItmApplyGainAmt"])) continue;
        //            if (GFunc.NEDec(drv["ItmApplyGainAmt"], 0) != 0)//Ask Mic
        //            {
        //                if (!GFunc.IsNE(drv["ItmApplyGainAccKey"]))
        //                {
        //                    if (GFunc.NEInt(drv["ItmApplyGainAccKey"], 0) <= 0)//Ask Mic
        //                    {
        //                        throw new TAException("Detail gain account cannot be empty");
        //                        //return false;
        //                    }
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = GFunc.NEInt(drv["ItmApplyGainAccKey"], 0);
        //                dr["LogTrans"] = PostType.ItmGLP;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["LinkDocDeptKey"], 0);
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["LinkDocTranGrpKey"], 0);
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = objDoc.DocID;
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = 0;
        //                dr["LogFD"] = 0;
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyGainAmt"], 0), "D", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyGainAmt"], 0), "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);

        //            }
        //        }
        //        #endregion

        //        #region Post Rec BK
        //        if (DocHome != 0)
        //        {
        //            if (GFunc.NEInt(DocAccKey, 0) <= 0)
        //            {
        //                throw new TAException("GL account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccKey;
        //            dr["LogTrans"] = PostType.BK;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocGrand, "D", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand, "D", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome, "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome, "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Tax
        //        if (DocTaxTotal != 0)
        //        {
        //            List<SqlParameter> TaxparmList = new List<SqlParameter>();
        //            TaxparmList.Add(new SqlParameter("@DocDate", objDoc.DocDate));
        //            TaxparmList.Add(new SqlParameter("@TaxGrpKey", DocTaxGrpKey));

        //            dtTaxGrpList = GFunc.ExecuteProc("Doc_GetTaxGrpList", TaxparmList);


        //            if (dtTaxGrpList.Rows.Count != 0)
        //            {

        //                foreach (DataRow drTax in dtTaxGrpList.Rows)
        //                {
        //                    dr = dtPost.NewRow();
        //                    dr["LogDC"] = objDoc.DocCodeKey;
        //                    dr["LogDK"] = objDoc.DocKey;
        //                    dr["LogDItm"] = 0;
        //                    dr["LogLineType"] = logLineType; //No Detail
        //                    dr["LogAccKey"] = drTax["AccKey"];
        //                    dr["LogTrans"] = PostType.TX;
        //                    dr["LogTaxKey"] = drTax["TaxKey"];
        //                    dr["LogSeq"] = logLineType;//Document Header
        //                    dr["DocID"] = docID;
        //                    dr["DocDate"] = objDoc.DocDate;
        //                    dr["DocPeriod"] = DocPeriod;
        //                    dr["DocBranchKey"] = DocBranchKey;
        //                    dr["DocDeptKey"] = DocDeptKey;
        //                    dr["DocTranGrpKey"] = DocTranGrpKey;
        //                    dr["DocGrpKey"] = DocGrpKey;
        //                    dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);//Ask Mic
        //                    dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                    dr["DocCurrkey"] = DocCurrkey;
        //                    dr["DocCurrRate"] = DocCurrRate;
        //                    dr["DocCVKey"] = DocCVKey;
        //                    dr["DocCVNmDoc"] = DocCVNmDoc;
        //                    dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                    dr["DocRef"] = DocRef;
        //                    dr["DocDes"] = DocDes;
        //                    dr["DetItmKey"] = 0;
        //                    dr["DetItmKeySelect"] = 0;
        //                    dr["DetItmDes"] = string.Empty;
        //                    dr["DetRef"] = dtTaxGrpList.Rows[0]["TaxDes"];
        //                    dr["DetJobKey"] = 0;
        //                    dr["DetJobPhaseKey"] = 0;
        //                    dr["DetJobTaskKey"] = 0;
        //                    dr["DetJobCostTypeKey"] = 0;
        //                    dr["LogFC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "C");
        //                    dr["LogFD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "D");
        //                    dr["LogHC"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "C");
        //                    dr["LogHD"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * objDoc.DocSign, DocTaxGrpRate, "C", "D");
        //                    dr["LogRecon"] = false;
        //                    dr["LogReconPeriod"] = 0;
        //                    dtPost.Rows.Add(dr);
        //                }
        //            }
        //        }
        //        #endregion

        //        #region Post Itm Discount Exp

        //        foreach (DataRow drv in dtExp.Rows)
        //        {
        //            if (GFunc.IsNE(drv["ExpAmtH"])) continue;
        //            if (GFunc.NEDec(drv["ExpAmtH"], 0) != 0)
        //            {
        //                if (!GFunc.IsNE(drv["ExpAccKey"]))
        //                {
        //                    if (GFunc.NEDec(drv["ExpAccKey"], 0) <= 0)
        //                    {

        //                        throw new TAException("Detail account cannot be empty");
        //                    }
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["ExpAccKey"];
        //                dr["LogTrans"] = PostType.EXP;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["ExpSN"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = GFunc.NEInt(drv["ExpDeptKey"], 0);
        //                dr["DocTranGrpKey"] = GFunc.NEInt(drv["ExpTranGrpKey"], 0);
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = GFunc.NEInt(objDoc.DocType, 0);
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = objDoc.DocID;
        //                dr["DetRef"] = string.Empty;
        //                dr["DetJobKey"] = GFunc.NEInt(drv["ExpJobKey"], 0);
        //                dr["DetJobPhaseKey"] = GFunc.NEInt(drv["ExpJobPhaseKey"], 0);
        //                dr["DetJobTaskKey"] = GFunc.NEInt(drv["ExpJobTaskKey"], 0);
        //                dr["DetJobCostTypeKey"] = GFunc.NEInt(drv["ExpJobCostTypeKey"], 0);
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ExpAmtF"], 0) * objDoc.DocSign, "C", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ExpAmtF"], 0) * objDoc.DocSign, "C", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ExpAmtH"], 0) * objDoc.DocSign, "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ExpAmtH"], 0) * objDoc.DocSign, "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        DTPostRnd(objDoc, dtPost);

        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dr = null;
        //    }
        //}
        //private static DataTable DTPostARPY(SqlConnection cn, Document objDoc, DataTable dtItems, DataTable dtExp)
        //{
        //    #region Declaration

        //    DataRow dr = null;
        //    DataTable dtTaxGrpList = new DataTable();
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    int? DocCurrkey = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //    decimal? DocCurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //    int? DocCVKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
        //    string DocCVNmDoc = GFunc.GetStringPropertyValue("DocConNm", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    int? DocTaxGrpKey = GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc);
        //    decimal? DocTaxTotal = GFunc.GetDecimalPropertyValue("DocTaxTotal", objDoc);
        //    decimal? DocTaxGrpRate = GFunc.GetDecimalPropertyValue("DocTaxGrpRate", objDoc);
        //    decimal? DocGrand = GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //    decimal? DocHome = GFunc.GetDecimalPropertyValue("DocHome", objDoc);
        //    int? DocAccKey = GFunc.GetIntPropertyValue("DocAccKey", objDoc);
        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        // DataTable dtItem = dtItems.Copy();//Copy to Local DataTable and then Sort and Add Seq No.
        //        DataTable dtItem = new DataTable();
        //        GFunc.CopyDataTableToDetailObject(dtItems, dtItem);
        //        dtItem.Columns.Add("Seq", typeof(int));
        //        int seq = 1;
        //        dtItem.DefaultView.Sort = "LinkDocDate,LinkDocID";
        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            drv["Seq"] = seq;
        //            seq += 1;
        //        }

        //        #region Post Itm Discount AR Account


        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            if (GFunc.IsNE(drv["ItmApplyDisAmtH"])) continue;
        //            if (GFunc.NEDec(drv["ItmApplyDisAmtH"], 0) != 0)
        //            {
        //                if (!GFunc.IsNE(drv["LinkDocAccKey"]))
        //                {
        //                    if (int.Parse(drv["LinkDocAccKey"].ToString()) <= 0)
        //                    {
        //                        throw new TAException("Detail AR account cannot be empty");
        //                    }
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["LinkDocAccKey"];
        //                dr["LogTrans"] = PostType.ItmDisB;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = drv["LinkDocDeptKey"];
        //                dr["DocTranGrpKey"] = drv["LinkDocTranGrpKey"];
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = objDoc.DocID;
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtF"], 0), "C", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtF"], 0), "C", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtH"], 0), "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtH"], 0), "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post Itm Discount Exp Account

        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            if (GFunc.IsNEZ(drv["ItmApplyDisAmtH"])) continue;
        //            if (GFunc.NEDec(drv["ItmApplyDisAmtH"], 0) != 0)
        //            {
        //                if (!GFunc.IsNE(drv["ItmApplyDisAccKey"]))
        //                {
        //                    if (GFunc.NEInt(drv["ItmApplyDisAccKey"], 0) <= 0)
        //                    {
        //                        throw new TAException("Detail discount account cannot be empty");
        //                    }
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["ItmApplyDisAccKey"];
        //                dr["LogTrans"] = PostType.ItmDisP;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = drv["LinkDocDeptKey"];
        //                dr["DocTranGrpKey"] = drv["LinkDocTranGrpKey"];
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = objDoc.DocID;
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtF"], 0), "D", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtF"], 0), "D", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtH"], 0), "D", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDisAmtH"], 0), "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post Itm Pay AR

        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            if (GFunc.IsNE(drv["ItmApplyDocAmtH"])) continue;
        //            if (GFunc.NEDec(drv["ItmApplyDocAmtH"], 0) != 0)
        //            {
        //                if (!GFunc.IsNE(drv["LinkDocAccKey"]))
        //                {
        //                    if (GFunc.NEInt(drv["LinkDocAccKey"], 0) <= 0)
        //                    {
        //                        throw new TAException("Detail AR account cannot be empty");
        //                    }
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["LinkDocAccKey"];
        //                dr["LogTrans"] = PostType.ItmAR;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = drv["LinkDocDeptKey"];
        //                dr["DocTranGrpKey"] = drv["LinkDocTranGrpKey"];
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDocAmtF"], 0), "C", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDocAmtF"], 0), "C", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDocAmtH"], 0), "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyDocAmtH"], 0), "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post Itm Gain IC

        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            if (GFunc.IsNE(drv["ItmApplyGainAmt"])) continue;
        //            if (GFunc.NEDec(drv["ItmApplyGainAmt"], 0) != 0)
        //            {
        //                if (!GFunc.IsNE(drv["ItmApplyGainAccKey"]))
        //                {
        //                    if (GFunc.NEDec(drv["ItmApplyGainAccKey"], 0) < 0)
        //                    {
        //                        throw new TAException("Detail gain account cannot be empty");
        //                    }
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["ItmApplyGainAccKey"];
        //                dr["LogTrans"] = PostType.ItmGLP;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = drv["LinkDocDeptKey"];
        //                dr["DocTranGrpKey"] = drv["LinkDocTranGrpKey"];
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = objDoc.DocID;
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = 0;
        //                dr["LogFD"] = 0;
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyGainAmt"], 0), "D", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyGainAmt"], 0), "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);

        //            }
        //        }
        //        #endregion

        //        #region Post Rec BK
        //        if (DocHome != 0)
        //        {
        //            if (GFunc.NEInt(DocAccKey, 0) <= 0)
        //            {
        //                throw new TAException("GL account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccKey;
        //            dr["LogTrans"] = PostType.BK;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocGrand, "D", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand, "D", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome, "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome, "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Tax
        //        if (DocTaxTotal != 0)
        //        {
        //            List<SqlParameter> TaxparmList = new List<SqlParameter>();
        //            TaxparmList.Add(new SqlParameter("@DocDate", objDoc.DocDate));
        //            TaxparmList.Add(new SqlParameter("@TaxGrpKey", DocTaxGrpKey));

        //            dtTaxGrpList = GFunc.ExecuteProc("Doc_GetTaxGrpList", TaxparmList);


        //            if (dtTaxGrpList.Rows.Count != 0)
        //            {

        //                foreach (DataRow drTax in dtTaxGrpList.Rows)
        //                {
        //                    dr = dtPost.NewRow();
        //                    dr["LogDC"] = objDoc.DocCodeKey;
        //                    dr["LogDK"] = objDoc.DocKey;
        //                    dr["LogDItm"] = 0;
        //                    dr["LogLineType"] = logLineType; //No Detail
        //                    dr["LogAccKey"] = drTax["AccKey"];
        //                    dr["LogTrans"] = PostType.TX;
        //                    dr["LogTaxKey"] = drTax["TaxKey"];
        //                    dr["LogSeq"] = logLineType;//Document Header
        //                    dr["DocID"] = docID;
        //                    dr["DocDate"] = objDoc.DocDate;
        //                    dr["DocPeriod"] = DocPeriod;
        //                    dr["DocBranchKey"] = DocBranchKey;
        //                    dr["DocDeptKey"] = DocDeptKey;
        //                    dr["DocTranGrpKey"] = DocTranGrpKey;
        //                    dr["DocGrpKey"] = DocGrpKey;
        //                    dr["DocType"] = objDoc.DocType;
        //                    dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                    dr["DocCurrkey"] = DocCurrkey;
        //                    dr["DocCurrRate"] = DocCurrRate;
        //                    dr["DocCVKey"] = DocCVKey;
        //                    dr["DocCVNmDoc"] = DocCVNmDoc;
        //                    dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                    dr["DocRef"] = DocRef;
        //                    dr["DocDes"] = DocDes;
        //                    dr["DetItmKey"] = 0;
        //                    dr["DetItmKeySelect"] = 0;
        //                    dr["DetItmDes"] = string.Empty;
        //                    dr["DetRef"] = dtTaxGrpList.Rows[0]["TaxDes"];
        //                    dr["DetJobKey"] = 0;
        //                    dr["DetJobPhaseKey"] = 0;
        //                    dr["DetJobTaskKey"] = 0;
        //                    dr["DetJobCostTypeKey"] = 0;
        //                    dr["LogFC"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * -objDoc.DocSign, DocTaxGrpRate, "C", "C");
        //                    dr["LogFD"] = DocComUtility.PTTax(DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * -objDoc.DocSign, DocTaxGrpRate, "C", "D");
        //                    dr["LogHC"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * -objDoc.DocSign, DocTaxGrpRate, "C", "C");
        //                    dr["LogHD"] = DocComUtility.PTTax(DocCurrRate * DocTaxTotal, GFunc.NEDec(dtTaxGrpList.Rows[0]["TaxRate"], 0) * -objDoc.DocSign, DocTaxGrpRate, "C", "D");
        //                    dr["LogRecon"] = false;
        //                    dr["LogReconPeriod"] = 0;
        //                    dtPost.Rows.Add(dr);
        //                }
        //            }
        //        }
        //        #endregion

        //        #region Post Itm Expenses

        //        foreach (DataRow drv in dtExp.Rows)
        //        {
        //            if (GFunc.IsNE(drv["ExpAmtH"])) continue;
        //            if (GFunc.NEDec(drv["ExpAmtH"], 0) != 0)
        //            {
        //                if (!GFunc.IsNE(drv["ExpAccKey"]))
        //                {
        //                    if (GFunc.NEInt(drv["ExpAccKey"], 0) <= 0)
        //                    {
        //                        throw new TAException("Detail account cannot be empty");
        //                    }
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["ExpAccKey"];
        //                dr["LogTrans"] = PostType.EXP;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["ExpSN"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = drv["ExpDeptKey"];
        //                dr["DocTranGrpKey"] = drv["ExpTranGrpKey"];
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = objDoc.DocID;
        //                dr["DetRef"] = string.Empty;
        //                dr["DetJobKey"] = drv["ExpJobKey"];
        //                dr["DetJobPhaseKey"] = drv["ExpJobPhaseKey"];
        //                dr["DetJobTaskKey"] = drv["ExpJobTaskKey"];
        //                dr["DetJobCostTypeKey"] = drv["ExpJobCostTypeKey"];
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ExpAmtF"], 0) * -objDoc.DocSign, "C", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ExpAmtF"], 0) * -objDoc.DocSign, "C", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ExpAmtH"], 0) * -objDoc.DocSign, "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ExpAmtH"], 0) * -objDoc.DocSign, "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        DTPostRnd(cn, objDoc, dtPost);

        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dr = null;
        //    }
        //}
        //private static DataTable DTPostARRF(Document objDoc)
        //{
        //    #region Declaration
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    int? DocCurrkey = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //    decimal? DocCurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //    int? DocCVKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
        //    string DocCVNmDoc = GFunc.GetStringPropertyValue("DocConNm", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    decimal? DocGrand = GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //    decimal? DocHome = GFunc.GetDecimalPropertyValue("DocHome", objDoc);
        //    int? DocAccARKey = GFunc.GetIntPropertyValue("DocAccARKey", objDoc);
        //    int? DocAccGLKey = GFunc.GetIntPropertyValue("DocAccGLKey", objDoc);
        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        #region Post AR
        //        if (DocGrand != 0)
        //        {
        //            if (GFunc.NEInt(DocAccARKey, 0) <= 0)
        //            {
        //                throw new TAException("AR account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccARKey;
        //            dr["LogTrans"] = PostType.AR;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = GFunc.GetStringPropertyValue("DocJobKey", objDoc);
        //            dr["DetJobPhaseKey"] = GFunc.GetStringPropertyValue("DocJobPhaseKey", objDoc);
        //            dr["DetJobTaskKey"] = GFunc.GetStringPropertyValue("DocJobTaskKey", objDoc);
        //            dr["DetJobCostTypeKey"] = GFunc.GetStringPropertyValue("DocJobCostTypeKey", objDoc);
        //            dr["LogFC"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "D", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "D", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome * objDoc.DocSign, "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome * objDoc.DocSign, "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Rec BK
        //        if (DocHome != 0)
        //        {
        //            if (GFunc.NEInt(DocAccGLKey, 0) <= 0)
        //            {
        //                throw new TAException("GL account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccGLKey;
        //            dr["LogTrans"] = PostType.BK;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = GFunc.GetStringPropertyValue("DocJobKey", objDoc);
        //            dr["DetJobPhaseKey"] = GFunc.GetStringPropertyValue("DocJobPhaseKey", objDoc);
        //            dr["DetJobTaskKey"] = GFunc.GetStringPropertyValue("DocJobTaskKey", objDoc);
        //            dr["DetJobCostTypeKey"] = GFunc.GetStringPropertyValue("DocJobCostTypeKey", objDoc);
        //            dr["LogFC"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "C", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "C", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome * objDoc.DocSign, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome * objDoc.DocSign, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        DTPostRnd(objDoc, dtPost);

        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dr = null;
        //    }
        //}
        //private static DataTable DTPostARRF(SqlConnection cn, Document objDoc)
        //{
        //    #region Declaration
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    int? DocCurrkey = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //    decimal? DocCurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //    int? DocCVKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
        //    string DocCVNmDoc = GFunc.GetStringPropertyValue("DocConNm", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    decimal? DocGrand = GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //    decimal? DocHome = GFunc.GetDecimalPropertyValue("DocHome", objDoc);
        //    int? DocAccARKey = GFunc.GetIntPropertyValue("DocAccARKey", objDoc);
        //    int? DocAccGLKey = GFunc.GetIntPropertyValue("DocAccGLKey", objDoc);
        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        #region Post AR
        //        if (DocGrand != 0)
        //        {
        //            if (GFunc.NEInt(DocAccARKey, 0) <= 0)
        //            {
        //                throw new TAException("AR account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccARKey;
        //            dr["LogTrans"] = PostType.AR;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = GFunc.GetIntPropertyValue("DocJobKey", objDoc);
        //            dr["DetJobPhaseKey"] = GFunc.GetIntPropertyValue("DocJobPhaseKey", objDoc);
        //            dr["DetJobTaskKey"] = GFunc.GetIntPropertyValue("DocJobTaskKey", objDoc);
        //            dr["DetJobCostTypeKey"] = GFunc.GetIntPropertyValue("DocJobCostTypeKey", objDoc);
        //            dr["LogFC"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "D", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "D", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome * objDoc.DocSign, "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome * objDoc.DocSign, "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        #region Post Rec BK
        //        if (DocHome != 0)
        //        {
        //            if (GFunc.NEInt(DocAccGLKey, 0) <= 0)
        //            {
        //                throw new TAException("GL account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccGLKey;
        //            dr["LogTrans"] = PostType.BK;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = DocCVKey;
        //            dr["DocCVNmDoc"] = DocCVNmDoc;
        //            dr["DocChqNum"] = GFunc.GetStringPropertyValue("DocChqNum", objDoc);
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = GFunc.GetStringPropertyValue("DocJobKey", objDoc);
        //            dr["DetJobPhaseKey"] = GFunc.GetStringPropertyValue("DocJobPhaseKey", objDoc);
        //            dr["DetJobTaskKey"] = GFunc.GetStringPropertyValue("DocJobTaskKey", objDoc);
        //            dr["DetJobCostTypeKey"] = GFunc.GetStringPropertyValue("DocJobCostTypeKey", objDoc);
        //            dr["LogFC"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "C", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "C", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome * objDoc.DocSign, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome * objDoc.DocSign, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        DTPostRnd(cn, objDoc, dtPost);

        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dr = null;
        //    }
        //}
        //private static DataTable DTPostARCT(Document objDoc, DataTable dtItems)
        //{
        //    #region Declaration
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    int? DocCurrkey = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //    decimal? DocCurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //    int? DocCVKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
        //    string DocCVNmDoc = GFunc.GetStringPropertyValue("DocConNm", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);
        //    decimal? DocGrand = GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //    decimal? DocHome = GFunc.GetDecimalPropertyValue("DocHome", objDoc);

        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        DataTable dtItem = dtItems.Copy();//Copy to Local DataTable and then Sort and Add Seq No.
        //        dtItem.Columns.Add("Seq", typeof(int));
        //        int seq = 1;
        //        dtItem.DefaultView.Sort = "LinkDocDate,LinkDocID";
        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            drv["Seq"] = seq;
        //            seq += 1;
        //        }

        //        #region Post Itm Pay AR

        //        #region dtItem Filter
        //        StringBuilder DocDC = new StringBuilder();
        //        DocDC.Append((int)GEnum.SystemCode.Sales_Invoice + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Sales_Credit_Note + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Sales_Debit_Note + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Sales_Adjustment + ",");
        //        DocDC.Append((int)GEnum.SystemCode.AR_Opening_Balance + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Cash_Sale + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Cash_Debit_Note + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Cash_Credit_Note + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Cash_Adjustment + ",");
        //        DocDC.Append((int)GEnum.SystemCode.AR_Cash_Opening_Balance);
        //        #endregion

        //        dtItem.DefaultView.RowFilter = "LinkDocDC IN(" + DocDC + ")";//LinkDocDC = Sales IV/DN/CN/ADJ/OPenBal + Cash IV/DN/CN/ADJ/OpenBal

        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            if (GFunc.NEInt(drv["ItmApplyPayAmtH"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(drv["LinkDocAccKey"], 0) < 0)
        //                {
        //                    throw new TAException("Detail account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = string.Empty;
        //                dr["LogAccKey"] = drv["LinkDocAccKey"];
        //                dr["LogTrans"] = PostType.ItmAR;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = drv["LinkDocDeptKey"];
        //                dr["DocTranGrpKey"] = drv["LinkDocTranGrpKey"];
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = drv["LinkDocDes"];
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyPayAmtF"], 0), "C", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyPayAmtF"], 0), "C", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyPayAmtH"], 0), "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyPayAmtH"], 0), "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);

        //            }
        //        }
        //        #endregion

        //        #region Post Itm Pay AP

        //        #region dtItem Filter
        //        DocDC = null;
        //        DocDC.Append((int)GEnum.SystemCode.Purchase_Invoice + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Purchase_Credit_Note + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Purchase_Debit_Note + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Purchase_Adjustment);
        //        DocDC.Append((int)GEnum.SystemCode.AP_Opening_Balance + ",");
        //        #endregion

        //        dtItem.DefaultView.RowFilter = "LinkDocDC IN(" + DocDC + ")"; //LinkDocDC = AP Invoice/DN/CN/ADJ/OpenBal 

        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            if (GFunc.NEInt(drv["ItmApplyPayAmtH"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(drv["LinkDocAccKey"], 0) < 0)
        //                {
        //                    throw new TAException("Detail account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = string.Empty;
        //                dr["LogAccKey"] = drv["LinkDocAccKey"];
        //                dr["LogTrans"] = PostType.ItmAP;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = drv["LinkDocDeptKey"];
        //                dr["DocTranGrpKey"] = drv["LinkDocTranGrpKey"];
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = drv["LinkDocDes"];
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyPayAmtF"], 0), "D", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyPayAmtF"], 0), "D", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyPayAmtH"], 0), "D", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyPayAmtH"], 0), "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        DTPostRnd(objDoc, dtPost);
        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dr = null;
        //    }
        //}
        //private static DataTable DTPostARCT(SqlConnection cn, Document objDoc, DataTable dtItems)
        //{
        //    #region Declaration
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    int? DocCurrkey = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //    decimal? DocCurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //    int? DocCVKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
        //    string DocCVNmDoc = GFunc.GetStringPropertyValue("DocConNm", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);
        //    decimal? DocGrand = GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //    decimal? DocHome = GFunc.GetDecimalPropertyValue("DocHome", objDoc);

        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;


        //        //Copy to Local DataTable and then Sort and Add Seq No.
        //        DataTable dtItem = new DataTable();
        //        GFunc.CopyDataTableToDetailObject(dtItems, dtItem);

        //        dtItem.Columns.Add("Seq", typeof(int));
        //        int seq = 1;
        //        dtItem.DefaultView.Sort = "LinkDocDate,LinkDocID";
        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            drv["Seq"] = seq;
        //            seq += 1;
        //        }

        //        #region Post Itm Pay AR

        //        #region dtItem Filter
        //        StringBuilder DocDC = new StringBuilder();
        //        DocDC.Append((int)GEnum.SystemCode.Sales_Invoice + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Sales_Credit_Note + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Sales_Debit_Note + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Sales_Adjustment + ",");
        //        DocDC.Append((int)GEnum.SystemCode.AR_Opening_Balance + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Cash_Sale + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Cash_Debit_Note + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Cash_Credit_Note + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Cash_Adjustment + ",");
        //        DocDC.Append((int)GEnum.SystemCode.AR_Cash_Opening_Balance);
        //        #endregion

        //        dtItem.DefaultView.RowFilter = "LinkDocDC IN(" + DocDC + ")";//LinkDocDC = Sales IV/DN/CN/ADJ/OPenBal + Cash IV/DN/CN/ADJ/OpenBal

        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            if (GFunc.NEDec(drv["ItmApplyPayAmtH"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(drv["LinkDocAccKey"], 0) <= 0)
        //                {
        //                    throw new TAException("Detail account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["LinkDocAccKey"];
        //                dr["LogTrans"] = PostType.ItmAR;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = drv["LinkDocDeptKey"];
        //                dr["DocTranGrpKey"] = drv["LinkDocTranGrpKey"];
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyPayAmtF"], 0), "C", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyPayAmtF"], 0), "C", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyPayAmtH"], 0), "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyPayAmtH"], 0), "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);

        //            }
        //        }
        //        #endregion

        //        #region Post Itm Pay AP

        //        #region dtItem Filter
        //        DocDC = new StringBuilder();
        //        DocDC.Append((int)GEnum.SystemCode.Purchase_Invoice + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Purchase_Credit_Note + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Purchase_Debit_Note + ",");
        //        DocDC.Append((int)GEnum.SystemCode.Purchase_Adjustment);
        //        DocDC.Append((int)GEnum.SystemCode.AP_Opening_Balance + ",");
        //        #endregion

        //        dtItem.DefaultView.RowFilter = "LinkDocDC IN(" + DocDC + ")"; //LinkDocDC = AP Invoice/DN/CN/ADJ/OpenBal 

        //        foreach (DataRowView drv in dtItem.DefaultView)
        //        {
        //            if (GFunc.NEDec(drv["ItmApplyPayAmtH"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(drv["LinkDocAccKey"], 0) <= 0)
        //                {
        //                    throw new TAException("Detail account cannot be empty");
        //                    //return false;
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["LinkDocAccKey"];
        //                dr["LogTrans"] = PostType.ItmAP;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["Seq"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = drv["LinkDocDeptKey"];
        //                dr["DocTranGrpKey"] = drv["LinkDocTranGrpKey"];
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = DocCVKey;
        //                dr["DocCVNmDoc"] = DocCVNmDoc;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = drv["LinkDocID"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyPayAmtF"], 0), "D", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyPayAmtF"], 0), "D", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyPayAmtH"], 0), "D", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmApplyPayAmtH"], 0), "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        DTPostRnd(cn, objDoc, dtPost);
        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dr = null;
        //    }
        //}
        //private static DataTable DTPostGLDP(Document objDoc, DataTable dtItems)
        //{
        //    #region Declaration
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    int? DocCurrkey = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //    decimal? DocCurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    decimal? DocGrand = GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //    decimal? DocHome = GFunc.GetDecimalPropertyValue("DocHome", objDoc);
        //    decimal? DocGainAmtH = GFunc.GetDecimalPropertyValue("DocGainAmtH", objDoc);
        //    int? DocAccGainKey = GFunc.GetIntPropertyValue("DocAccGainKey", objDoc);
        //    int? DocAccKey = GFunc.GetIntPropertyValue("DocAccKey", objDoc);
        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        #region Post Itm Deposit

        //        foreach (DataRow drv in dtItems.Rows)
        //        {
        //            if (GFunc.NEDec(drv["ItmDocAmtH"], 0) != 0)
        //            {
        //                if (GFunc.NEDec(drv["ItmDocAccKey"], 0) <= 0)
        //                {
        //                    throw new TAException("Detail account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["ItmDocAccKey"];
        //                dr["LogTrans"] = PostType.ITM;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["ItmSN"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = drv["ItmDocDeptKey"];
        //                dr["DocTranGrpKey"] = drv["ItmTranGrpKey"];
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = 0;
        //                dr["DocCVNmDoc"] = drv["ItmReForm"];
        //                dr["DocChqNum"] = drv["ItmDocChqNum"];
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = 0;
        //                dr["DetRef"] = drv["ItmDocRef"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmDocAmtF"], 0) * objDoc.DocSign, "C", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmDocAmtF"], 0) * objDoc.DocSign, "C", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmDocAmtH"], 0) * objDoc.DocSign, "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmDocAmtH"], 0) * objDoc.DocSign, "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post Gain
        //        if (DocGainAmtH != 0)
        //        {
        //            if (GFunc.NEInt(DocAccGainKey, 0) <= 0)
        //            {
        //                throw new TAException("Gain account cannot be empty");
        //            }

        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccGainKey;
        //            dr["LogTrans"] = PostType.GLP;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = 0;
        //            dr["DocCVNmDoc"] = string.Empty;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = 0;
        //            dr["LogFD"] = 0;
        //            dr["LogHC"] = DocComUtility.PT(DocGainAmtH * objDoc.DocSign, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocGainAmtH * objDoc.DocSign, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);

        //        }
        //        #endregion

        //        #region Post Rec BK
        //        if (DocHome != 0)
        //        {
        //            if (GFunc.NEInt(DocAccKey, 0) <= 0)
        //            {
        //                throw new TAException("GL account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccKey;
        //            dr["LogTrans"] = PostType.BK;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = 0;
        //            dr["DocCVNmDoc"] = string.Empty;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "D", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "D", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome * objDoc.DocSign, "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome * objDoc.DocSign, "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        DTPostRnd(objDoc, dtPost);

        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dr = null;
        //    }
        //}
        //private static DataTable DTPostGLDP(SqlConnection cn, Document objDoc, DataTable dtItems)
        //{
        //    #region Declaration
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    int? DocCurrkey = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //    decimal? DocCurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    decimal? DocGrand = GFunc.GetDecimalPropertyValue("DocGrand", objDoc);
        //    decimal? DocHome = GFunc.GetDecimalPropertyValue("DocHome", objDoc);
        //    decimal? DocGainAmtH = GFunc.GetDecimalPropertyValue("DocGainAmtH", objDoc);
        //    int? DocAccGainKey = GFunc.GetIntPropertyValue("DocAccGainKey", objDoc);
        //    int? DocAccBKKey = GFunc.GetIntPropertyValue("DocAccBKKey", objDoc);
        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        #region Post Itm Deposit

        //        foreach (DataRow drv in dtItems.Rows)
        //        {
        //            if (GFunc.NEDec(drv["ItmDocAmtH"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(drv["ItmDocAccKey"], 0) <= 0)
        //                {
        //                    throw new TAException("Detail account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["ItmDocAccKey"];
        //                dr["LogTrans"] = PostType.ITM;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["ItmSN"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = drv["ItmDocDeptKey"];
        //                dr["DocTranGrpKey"] = drv["ItmTranGrpKey"];
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = DocCurrkey;
        //                dr["DocCurrRate"] = DocCurrRate;
        //                dr["DocCVKey"] = 0;
        //                dr["DocCVNmDoc"] = drv["ItmReFrom"];
        //                dr["DocChqNum"] = drv["ItmDocChqNum"];
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = 0;
        //                dr["DetRef"] = drv["ItmDocRef"];
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmDocAmtF"], 0) * objDoc.DocSign, "C", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmDocAmtF"], 0) * objDoc.DocSign, "C", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmDocAmtH"], 0) * objDoc.DocSign, "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmDocAmtH"], 0) * objDoc.DocSign, "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post Gain
        //        if (DocGainAmtH != 0)
        //        {
        //            if (GFunc.NEInt(DocAccGainKey, 0) <= 0)
        //            {
        //                throw new TAException("Gain account cannot be empty");
        //            }

        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccGainKey;
        //            dr["LogTrans"] = PostType.GLP;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = 0;
        //            dr["DocCVNmDoc"] = string.Empty;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = 0;
        //            dr["LogFD"] = 0;
        //            dr["LogHC"] = DocComUtility.PT(DocGainAmtH * objDoc.DocSign, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocGainAmtH * objDoc.DocSign, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);

        //        }
        //        #endregion

        //        #region Post Rec BK
        //        if (DocHome != 0)
        //        {
        //            if (GFunc.NEInt(DocAccBKKey, 0) <= 0)
        //            {
        //                throw new TAException("GL account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccBKKey;
        //            dr["LogTrans"] = PostType.BK;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = DocCurrkey;
        //            dr["DocCurrRate"] = DocCurrRate;
        //            dr["DocCVKey"] = 0;
        //            dr["DocCVNmDoc"] = string.Empty;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "D", "C");
        //            dr["LogFD"] = DocComUtility.PT(DocGrand * objDoc.DocSign, "D", "D");
        //            dr["LogHC"] = DocComUtility.PT(DocHome * objDoc.DocSign, "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocHome * objDoc.DocSign, "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        DTPostRnd(cn, objDoc, dtPost);

        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dr = null;
        //    }
        //}
        //private static DataTable DTPostGLJN(Document objDoc, DataTable dtItems)
        //{
        //    #region Declaration
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = 0;
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    decimal? DocGrand = 0;
        //    decimal? DocHome = 0;
        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        #region Post Itm

        //        foreach (DataRow drv in dtItems.Rows)
        //        {
        //            //ItmCreditH <>0 or ItmDebitH<>0
        //            if (GFunc.NEDec(drv["ItmCreditH"], 0) != 0 || GFunc.NEDec(drv["ItmDebitH"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(drv["ItmAccKey"], 0) <= 0)
        //                {
        //                    throw new TAException("Detail account cannot be empty");
        //                    //return false;
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                //dr["LogLineType"] = drv["LineType"];
        //                //dr["LogAccKey"] = drv["ItmAccKey"];
        //                dr["LogTrans"] = PostType.ITM;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["ItmSN"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = drv["ItmDeptKey"];
        //                dr["DocTranGrpKey"] = drv["ItmTranGrpKey"];
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = drv["ItmCurrKey"];
        //                dr["DocCurrRate"] = drv["ItmCurrRate"];
        //                dr["DocCVKey"] = 0;
        //                dr["DocCVNmDoc"] = string.Empty;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = drv["ItmDes"];
        //                dr["DetRef"] = drv["ItmRef"];
        //                dr["DetJobKey"] = drv["ItmJobKey"];
        //                dr["DetJobPhaseKey"] = drv["ItmJobPhaseKey"];
        //                dr["DetJobTaskKey"] = drv["ItmJobTaskKey"];
        //                dr["DetJobCostTypeKey"] = drv["ItmJobCostTypeKey"];
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmCreditF"], 0) - GFunc.NEDec(drv["ItmDebitF"], 0), "C", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmDebitF"], 0) - GFunc.NEDec(drv["ItmCreditF"], 0), "D", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmCreditH"], 0) - GFunc.NEDec(drv["ItmDebitH"], 0), "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmDebitH"], 0) - GFunc.NEDec(drv["ItmCreditH"], 0), "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        DTPostRnd(objDoc, dtPost);
        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dr = null;
        //    }
        //}
        //private static DataTable DTPostGLJN(SqlConnection cn, Document objDoc, DataTable dtItems)
        //{
        //    #region Declaration
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));

        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = 0;
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    decimal? DocGrand = 0;
        //    decimal? DocHome = 0;
        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        #region Post Itm

        //        foreach (DataRow drv in dtItems.Rows)
        //        {
        //            //ItmCreditH <>0 or ItmDebitH<>0
        //            if (GFunc.NEDec(drv["ItmCreditH"], 0) != 0 || GFunc.NEDec(drv["ItmDebitH"], 0) != 0)//ask mic
        //            {
        //                if (GFunc.NEInt(drv["ItmAccKey"], 0) <= 0)
        //                {
        //                    throw new TAException("Detail account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = drv["ItmAccKey"];
        //                dr["LogTrans"] = PostType.ITM;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["ItmSN"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = drv["ItmDeptKey"];
        //                dr["DocTranGrpKey"] = drv["ItmTranGrpKey"];
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = drv["ItmCurrKey"];
        //                dr["DocCurrRate"] = GFunc.NEDec(drv["ItmCurrRate"], 1);
        //                dr["DocCVKey"] = 0;
        //                dr["DocCVNmDoc"] = string.Empty;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = drv["ItmDes"];
        //                dr["DetRef"] = drv["ItmRef"];
        //                dr["DetJobKey"] = drv["ItmJobKey"];
        //                dr["DetJobPhaseKey"] = drv["ItmJobPhaseKey"];
        //                dr["DetJobTaskKey"] = drv["ItmJobTaskKey"];
        //                dr["DetJobCostTypeKey"] = drv["ItmJobCostTypeKey"];
        //                dr["LogFC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmCreditF"], 0) - GFunc.NEDec(drv["ItmDebitF"], 0), "C", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmDebitF"], 0) - GFunc.NEDec(drv["ItmCreditF"], 0), "D", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.NEDec(drv["ItmCreditH"], 0) - GFunc.NEDec(drv["ItmDebitH"], 0), "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.NEDec(drv["ItmDebitH"], 0) - GFunc.NEDec(drv["ItmCreditH"], 0), "D", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        DTPostRnd(cn, objDoc, dtPost);
        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dr = null;
        //    }
        //}
        //private static DataTable DTPostGLRV(Document objDoc, DataTable dtItems)
        //{
        //    #region Declaration

        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    decimal? DocGrand = 0;
        //    decimal? DocHome = 0;
        //    decimal? DocAccAmtGainLoss = GFunc.GetDecimalPropertyValue("DocAccAmtGainLoss", objDoc);
        //    int? DocAccBKKey = GFunc.GetIntPropertyValue("DocAccBKKey", objDoc);
        //    int? DocAccGainKey = GFunc.GetIntPropertyValue("DocAccGainKey", objDoc);
        //    int? DocAccLossKey = GFunc.GetIntPropertyValue("DocAccLossKey", objDoc);
        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        #region set GLRV amount
        //        List<SqlParameter> paraList = new List<SqlParameter>();
        //        paraList.Add(new SqlParameter("@DocCodeKey", objDoc.DocCodeKey));
        //        paraList.Add(new SqlParameter("@DocKey", objDoc.DocKey));
        //        paraList.Add(new SqlParameter("@BKAcc", DocAccBKKey));
        //        paraList.Add(new SqlParameter("@Period", DocPeriod));
        //        SqlParameter para = new SqlParameter("@RetValue", typeof(int));
        //        para.Direction = ParameterDirection.Output;
        //        paraList.Add(para);
        //        DataTable dt = GFunc.ExecuteProc("Doc_GLRV_RevalueAmtGet", paraList);
        //        if ((int)para.Value != 1)
        //            throw new TAException("Unable to calculate revaluation value.");

        //        if (dt == null || dt.Rows.Count == 0)
        //            throw new TAException("Unable to calculate revaluation value.");

        //        decimal totalHC = GFunc.NEDec(dt.Rows[0]["TotalHC"], 0M);
        //        decimal totalHD = GFunc.NEDec(dt.Rows[0]["TotalHD"], 0M);

        //        DocAccAmtGainLoss = totalHC - totalHD;
        //        GFunc.SetPropertyValue("DocAccAmtGainLoss", objDoc, DocAccAmtGainLoss);
        //        #endregion

        //        #region Post  Bank
        //        if (DocAccAmtGainLoss != 0)
        //        {
        //            if (GFunc.NEInt(DocAccBKKey, 0) <= 0)
        //            {
        //                throw new TAException("Gain account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccBKKey;
        //            dr["LogTrans"] = PostType.BK;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //            dr["DocCurrRate"] = GFunc.GetIntPropertyValue("DocRevalueRate", objDoc);
        //            dr["DocCVKey"] = 0;
        //            dr["DocCVNmDoc"] = string.Empty;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = 0;
        //            dr["LogFD"] = 0;
        //            dr["LogHC"] = DocComUtility.PT(DocAccAmtGainLoss * objDoc.DocSign, "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocAccAmtGainLoss * objDoc.DocSign, "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);

        //        }
        //        #endregion

        //        #region Post Gain/Loss
        //        if (DocAccAmtGainLoss != 0)
        //        {

        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail

        //            //if DocAccAmtGainLoss <0 use DocAccGainKey else DocAccLossKey
        //            if (GFunc.NEInt(DocAccAmtGainLoss, 0) <= 0)
        //            {
        //                if (GFunc.NEInt(DocAccGainKey, 0) <= 0)
        //                {
        //                    throw new TAException("Gain account cannot be empty");
        //                }
        //                dr["LogAccKey"] = DocAccGainKey;
        //            }
        //            else
        //            {
        //                if (GFunc.NEInt(DocAccLossKey, 0) <= 0)
        //                {
        //                    throw new TAException("Loss account cannot be empty");
        //                }
        //                dr["LogAccKey"] = DocAccLossKey;
        //            }

        //            dr["LogTrans"] = PostType.GLP;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //            dr["DocCurrRate"] = GFunc.GetIntPropertyValue("DocRevalueRate", objDoc);
        //            dr["DocCVKey"] = 0;
        //            dr["DocCVNmDoc"] = string.Empty;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = 0;
        //            dr["LogFD"] = 0;
        //            dr["LogHC"] = DocComUtility.PT(DocAccAmtGainLoss * objDoc.DocSign, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocAccAmtGainLoss * objDoc.DocSign, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);

        //        }
        //        #endregion

        //        DTPostRnd(objDoc, dtPost);
        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dr = null;
        //    }
        //}//Completed
        //private static DataTable DTPostGLRV(SqlConnection cn, Document objDoc, DataTable dtItems)
        //{
        //    #region Declaration

        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();

        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int DocDeptKey = (int)GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int DocGrpKey = (int)GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    decimal DocAccAmtGainLoss = (decimal)GFunc.GetDecimalPropertyValue("DocAccAmtGainLoss", objDoc);
        //    int DocAccBKKey = (int)GFunc.GetIntPropertyValue("DocAccBKKey", objDoc);
        //    int DocAccGainKey = GFunc.NEInt(GFunc.GetIntPropertyValue("DocAccGainKey", objDoc), 0);
        //    int DocAccLossKey = GFunc.NEInt(GFunc.GetIntPropertyValue("DocAccLossKey", objDoc), 0);
        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        #region set GLRV amount
        //        List<SqlParameter> paraList = new List<SqlParameter>();
        //        paraList.Add(new SqlParameter("@DocCodeKey", objDoc.DocCodeKey));
        //        paraList.Add(new SqlParameter("@DocKey", objDoc.DocKey));
        //        paraList.Add(new SqlParameter("@BKAcc", DocAccBKKey));
        //        paraList.Add(new SqlParameter("@Period", DocPeriod));
        //        SqlParameter para = new SqlParameter("@RetValue", SqlDbType.Int);
        //        para.Direction = ParameterDirection.Output;
        //        //para.Value = 0;
        //        paraList.Add(para);
        //        DataTable dt = GFunc.ExecuteProc(cn, "Doc_GLRV_RevalueAmtGet", paraList);
        //        if ((int)para.Value != 1)
        //            throw new TAException("Unable to calculate revaluation value.");

        //        if (dt == null || dt.Rows.Count == 0)
        //            throw new TAException("Unable to calculate revaluation value.");

        //        decimal totalHC = GFunc.NEDec(dt.Rows[0]["TotalHC"], 0M);
        //        decimal totalHD = GFunc.NEDec(dt.Rows[0]["TotalHD"], 0M);

        //        DocAccAmtGainLoss = totalHC - totalHD;
        //        GFunc.SetPropertyValue("DocAccAmtGainLoss", objDoc, DocAccAmtGainLoss);
        //        #endregion

        //        #region Post  Bank
        //        if (GFunc.NEInt(DocAccAmtGainLoss, 0) != 0)
        //        {
        //            if (GFunc.NEInt(DocAccBKKey, 0) <= 0)
        //            {
        //                throw new TAException("Gain account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail
        //            dr["LogAccKey"] = DocAccBKKey;
        //            dr["LogTrans"] = PostType.BK;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //            dr["DocCurrRate"] = GFunc.GetIntPropertyValue("DocRevalueRate", objDoc);
        //            dr["DocCVKey"] = 0;
        //            dr["DocCVNmDoc"] = string.Empty;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = 0;
        //            dr["LogFD"] = 0;
        //            dr["LogHC"] = DocComUtility.PT(DocAccAmtGainLoss * objDoc.DocSign, "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocAccAmtGainLoss * objDoc.DocSign, "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);

        //        }
        //        #endregion

        //        #region Post Gain/Loss
        //        if (DocAccAmtGainLoss != 0)
        //        {

        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = 0;
        //            dr["LogLineType"] = logLineType; //No Detail

        //            if (DocAccAmtGainLoss < 0)
        //            {
        //                if (DocAccGainKey < 0)
        //                {
        //                    throw new TAException("Gain account cannot be empty");
        //                }
        //                dr["LogAccKey"] = DocAccGainKey;
        //            }
        //            else
        //            {
        //                if (GFunc.NEInt(DocAccLossKey, 0) <= 0)
        //                {
        //                    throw new TAException("Loss account cannot be empty");
        //                }
        //                dr["LogAccKey"] = DocAccLossKey;
        //            }

        //            dr["LogTrans"] = PostType.GLP;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = logLineType;//Document Header
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = DocDeptKey;
        //            dr["DocTranGrpKey"] = DocTranGrpKey;
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
        //            dr["DocCurrRate"] = GFunc.GetIntPropertyValue("DocRevalueRate", objDoc);
        //            dr["DocCVKey"] = 0;
        //            dr["DocCVNmDoc"] = string.Empty;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = 0;
        //            dr["DetItmKeySelect"] = 0;
        //            dr["DetItmDes"] = string.Empty;
        //            dr["DetRef"] = string.Empty;
        //            dr["DetJobKey"] = 0;
        //            dr["DetJobPhaseKey"] = 0;
        //            dr["DetJobTaskKey"] = 0;
        //            dr["DetJobCostTypeKey"] = 0;
        //            dr["LogFC"] = 0;
        //            dr["LogFD"] = 0;
        //            dr["LogHC"] = DocComUtility.PT(DocAccAmtGainLoss * objDoc.DocSign, "C", "C");
        //            dr["LogHD"] = DocComUtility.PT(DocAccAmtGainLoss * objDoc.DocSign, "C", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);

        //        }
        //        #endregion

        //        DTPostRnd(cn, objDoc, dtPost);
        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dr = null;
        //    }
        //}//Completed
        //private static DataTable DTPostINADJ(Document objDoc, DataTable dtItems)
        //{
        //    #region Declaration
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);
        //    int? DocAccKey = GFunc.GetIntPropertyValue("DocAccKey", objDoc);

        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        #region Post  Itm ADJ
        //        dtItems.DefaultView.RowFilter = "ItmType<=710 AND LineType=1000";//"Finished GD" Or "Finished GDB" Or "StockB" Or "Stock" Or "Assembly"
        //        foreach (DataRowView drv in dtItems.DefaultView)
        //        {

        //            if (GFunc.NEDec(drv["ItmQty"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(DocAccKey, 0) <= 0)
        //                {
        //                    throw new TAException("GL account cannot be empty");
        //                    //return false;
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = drv["LineType"];
        //                dr["LogAccKey"] = DocAccKey;
        //                dr["LogTrans"] = PostType.ADJ;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["ItmSN"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = drv["ItmDeptKey"];
        //                dr["DocTranGrpKey"] = drv["ItmTranGrpKey"];
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = 1;
        //                dr["DocCurrRate"] = 1;
        //                dr["DocCVKey"] = 0;
        //                dr["DocCVNmDoc"] = string.Empty;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = drv["ItmKey"];
        //                dr["DetItmKeySelect"] = drv["ItmKeySelect"];
        //                dr["DetItmDes"] = drv["ItmDes"];
        //                dr["DetRef"] = drv["ItmRef"];
        //                dr["DetJobKey"] = drv["ItmJobKey"];
        //                dr["DetJobPhaseKey"] = drv["ItmJobPhaseKey"];
        //                dr["DetJobTaskKey"] = drv["ItmJobTaskKey"];
        //                dr["DetJobCostTypeKey"] = drv["ItmJobCostTypeKey"];
        //                dr["LogFC"] = DocComUtility.PT(GFunc.RndC(GFunc.NEDec(drv["ItmQty"], 0) * GFunc.NEDec(drv["ItmCost"], 0), GVar.RndDecs.Amtpt), "C", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.RndC(GFunc.NEDec(drv["ItmQty"], 0) * GFunc.NEDec(drv["ItmCost"], 0), GVar.RndDecs.Amtpt), "C", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.RndC(GFunc.NEDec(drv["ItmQty"], 0) * GFunc.NEDec(drv["ItmCost"], 0), GVar.RndDecs.Amtpt), "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.RndC(GFunc.NEDec(drv["ItmQty"], 0) * GFunc.NEDec(drv["ItmCost"], 0), GVar.RndDecs.Amtpt), "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post IN

        //        dtItems.DefaultView.RowFilter = "ItmType<=710 AND LineType=1000";//"Finished GD" Or "Finished GDB" Or "StockB" Or "Stock" Or "Assembly"
        //        foreach (DataRowView drv in dtItems.DefaultView)
        //        {
        //            if (GFunc.NEInt(drv["ItmAccKey"], 0) <= 0)
        //            {
        //                throw new TAException("Detail account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = drv["DocItmKey"];
        //            dr["LogLineType"] = drv["LineType"];
        //            dr["LogAccKey"] = drv["ItmAccKey"];
        //            dr["LogTrans"] = PostType.ADJ;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = drv["ItmSN"];
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = drv["ItmDeptKey"];
        //            dr["DocTranGrpKey"] = drv["ItmTranGrpKey"];
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = 1;
        //            dr["DocCurrRate"] = 1;
        //            dr["DocCVKey"] = 0;
        //            dr["DocCVNmDoc"] = string.Empty;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = drv["ItmKey"];
        //            dr["DetItmKeySelect"] = drv["ItmKeySelect"];
        //            dr["DetItmDes"] = drv["ItmDes"];
        //            dr["DetRef"] = drv["ItmRef"];
        //            dr["DetJobKey"] = drv["ItmJobKey"];
        //            dr["DetJobPhaseKey"] = drv["ItmJobPhaseKey"];
        //            dr["DetJobTaskKey"] = drv["ItmJobTaskKey"];
        //            dr["DetJobCostTypeKey"] = drv["ItmJobCostTypeKey"];
        //            dr["LogFC"] = DocComUtility.PT(GFunc.RndC(GFunc.NEDec(drv["ItmQty"], 0) * GFunc.NEDec(drv["ItmCost"], 0), GVar.RndDecs.Amtpt), "D", "C");
        //            dr["LogFD"] = DocComUtility.PT(GFunc.RndC(GFunc.NEDec(drv["ItmQty"], 0) * GFunc.NEDec(drv["ItmCost"], 0), GVar.RndDecs.Amtpt), "D", "D");
        //            dr["LogHC"] = DocComUtility.PT(GFunc.RndC(GFunc.NEDec(drv["ItmQty"], 0) * GFunc.NEDec(drv["ItmCost"], 0), GVar.RndDecs.Amtpt), "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(GFunc.RndC(GFunc.NEDec(drv["ItmQty"], 0) * GFunc.NEDec(drv["ItmCost"], 0), GVar.RndDecs.Amtpt), "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion

        //        DTPostRnd(objDoc, dtPost);
        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dr = null;
        //    }
        //}
        //private static DataTable DTPostINADJ(SqlConnection cn, Document objDoc, DataTable dtItems)
        //{
        //    #region Declaration
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #region dtPost field list
        //    dtPost.Columns.Add("LogKey", typeof(int));
        //    dtPost.Columns.Add("LogDC", typeof(int));
        //    dtPost.Columns.Add("LogDK", typeof(int));
        //    dtPost.Columns.Add("LogDItm", typeof(int));
        //    dtPost.Columns.Add("LogLineType", typeof(int));
        //    dtPost.Columns.Add("LogAccKey", typeof(int));
        //    dtPost.Columns.Add("LogTrans", typeof(string));
        //    dtPost.Columns.Add("LogTaxKey", typeof(int));
        //    dtPost.Columns.Add("LogSeq", typeof(decimal));
        //    dtPost.Columns.Add("DocID", typeof(string));
        //    dtPost.Columns.Add("DocDate", typeof(DateTime));
        //    dtPost.Columns.Add("DocPeriod", typeof(decimal));
        //    dtPost.Columns.Add("DocBranchKey", typeof(int));
        //    dtPost.Columns.Add("DocDeptKey", typeof(int));
        //    dtPost.Columns.Add("DocTranGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocGrpKey", typeof(int));
        //    dtPost.Columns.Add("DocType", typeof(int));
        //    dtPost.Columns.Add("DocTypeNm", typeof(string));
        //    dtPost.Columns.Add("DocCurrkey", typeof(int));
        //    dtPost.Columns.Add("DocCurrRate", typeof(decimal));
        //    dtPost.Columns.Add("DocCVKey", typeof(int));
        //    dtPost.Columns.Add("DocCVNmDoc", typeof(string));
        //    dtPost.Columns.Add("DocChqNum", typeof(string));
        //    dtPost.Columns.Add("DocRef", typeof(string));
        //    dtPost.Columns.Add("DocDes", typeof(string));
        //    dtPost.Columns.Add("DetItmKey", typeof(int));
        //    dtPost.Columns.Add("DetItmKeySelect", typeof(int));
        //    dtPost.Columns.Add("DetItmDes", typeof(string));
        //    dtPost.Columns.Add("DetRef", typeof(string));
        //    dtPost.Columns.Add("DetJobKey", typeof(int));
        //    dtPost.Columns.Add("DetJobPhaseKey", typeof(int));
        //    dtPost.Columns.Add("DetJobTaskKey", typeof(int));
        //    dtPost.Columns.Add("DetJobCostTypeKey", typeof(int));
        //    dtPost.Columns.Add("LogFC", typeof(decimal));
        //    dtPost.Columns.Add("LogFD", typeof(decimal));
        //    dtPost.Columns.Add("LogHC", typeof(decimal));
        //    dtPost.Columns.Add("LogHD", typeof(decimal));
        //    dtPost.Columns.Add("LogRecon", typeof(bool));
        //    dtPost.Columns.Add("LogReconPeriod", typeof(int));
        //    #endregion

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);
        //    int? DocAccKey = GFunc.GetIntPropertyValue("DocAccKey", objDoc);

        //    #endregion

        //    try
        //    {
        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        #region Post  Itm ADJ
        //        dtItems.DefaultView.RowFilter = "ItmType<=710 AND LineType=1000";//"Finished GD" Or "Finished GDB" Or "StockB" Or "Stock" Or "Assembly"
        //        foreach (DataRowView drv in dtItems.DefaultView)
        //        {

        //            if (GFunc.NEDec(drv["ItmQty"], 0) != 0)
        //            {
        //                if (GFunc.NEInt(DocAccKey, 0) <= 0)
        //                {
        //                    throw new TAException("GL account cannot be empty");
        //                }
        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = drv["DocItmKey"];
        //                dr["LogLineType"] = drv["LineType"];
        //                dr["LogAccKey"] = DocAccKey;
        //                dr["LogTrans"] = PostType.ADJ;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = drv["ItmSN"];
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = drv["ItmDeptKey"];
        //                dr["DocTranGrpKey"] = drv["ItmTranGrpKey"];
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = 1;
        //                dr["DocCurrRate"] = 1;
        //                dr["DocCVKey"] = 0;
        //                dr["DocCVNmDoc"] = string.Empty;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = drv["ItmKey"];
        //                dr["DetItmKeySelect"] = drv["ItmKeySelect"];
        //                dr["DetItmDes"] = drv["ItmDes"];
        //                dr["DetRef"] = drv["ItmRef"];
        //                dr["DetJobKey"] = drv["ItmJobKey"];
        //                dr["DetJobPhaseKey"] = drv["ItmJobPhaseKey"];
        //                dr["DetJobTaskKey"] = drv["ItmJobTaskKey"];
        //                dr["DetJobCostTypeKey"] = drv["ItmJobCostTypeKey"];
        //                dr["LogFC"] = DocComUtility.PT(GFunc.RndC(GFunc.NEDec(drv["ItmQty"], 0) * GFunc.NEDec(drv["ItmCost"], 0), GVar.RndDecs.Amtpt), "C", "C");
        //                dr["LogFD"] = DocComUtility.PT(GFunc.RndC(GFunc.NEDec(drv["ItmQty"], 0) * GFunc.NEDec(drv["ItmCost"], 0), GVar.RndDecs.Amtpt), "C", "D");
        //                dr["LogHC"] = DocComUtility.PT(GFunc.RndC(GFunc.NEDec(drv["ItmQty"], 0) * GFunc.NEDec(drv["ItmCost"], 0), GVar.RndDecs.Amtpt), "C", "C");
        //                dr["LogHD"] = DocComUtility.PT(GFunc.RndC(GFunc.NEDec(drv["ItmQty"], 0) * GFunc.NEDec(drv["ItmCost"], 0), GVar.RndDecs.Amtpt), "C", "D");
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);
        //            }
        //        }
        //        #endregion

        //        #region Post IN

        //        dtItems.DefaultView.RowFilter = "ItmType<=710 AND LineType=1000 AND ItmType <> 510";//"Finished GD" Or "Finished GDB" Or "StockB" Or "Stock" Or "Assembly"
        //        foreach (DataRowView drv in dtItems.DefaultView)
        //        {
        //            if (GFunc.NEInt(drv["ItmAccKey"], 0) <= 0)
        //            {
        //                throw new TAException("Detail account cannot be empty");
        //            }
        //            dr = dtPost.NewRow();
        //            dr["LogDC"] = objDoc.DocCodeKey;
        //            dr["LogDK"] = objDoc.DocKey;
        //            dr["LogDItm"] = drv["DocItmKey"];
        //            dr["LogLineType"] = drv["LineType"];
        //            dr["LogAccKey"] = drv["ItmAccKey"];
        //            dr["LogTrans"] = PostType.ADJ;
        //            dr["LogTaxKey"] = 0;
        //            dr["LogSeq"] = drv["ItmSN"];
        //            dr["DocID"] = docID;
        //            dr["DocDate"] = objDoc.DocDate;
        //            dr["DocPeriod"] = DocPeriod;
        //            dr["DocBranchKey"] = DocBranchKey;
        //            dr["DocDeptKey"] = drv["ItmDeptKey"];
        //            dr["DocTranGrpKey"] = drv["ItmTranGrpKey"];
        //            dr["DocGrpKey"] = DocGrpKey;
        //            dr["DocType"] = objDoc.DocType;
        //            dr["DocTypeNm"] = objDoc.DocTypeNm;
        //            dr["DocCurrkey"] = 1;
        //            dr["DocCurrRate"] = 1;
        //            dr["DocCVKey"] = 0;
        //            dr["DocCVNmDoc"] = string.Empty;
        //            dr["DocChqNum"] = string.Empty;
        //            dr["DocRef"] = DocRef;
        //            dr["DocDes"] = DocDes;
        //            dr["DetItmKey"] = drv["ItmKey"];
        //            dr["DetItmKeySelect"] = drv["ItmKeySelect"];
        //            dr["DetItmDes"] = drv["ItmDes"];
        //            dr["DetRef"] = drv["ItmRef"];
        //            dr["DetJobKey"] = drv["ItmJobKey"];
        //            dr["DetJobPhaseKey"] = drv["ItmJobPhaseKey"];
        //            dr["DetJobTaskKey"] = drv["ItmJobTaskKey"];
        //            dr["DetJobCostTypeKey"] = drv["ItmJobCostTypeKey"];
        //            dr["LogFC"] = DocComUtility.PT(GFunc.RndC(GFunc.NEDec(drv["ItmQty"], 0) * GFunc.NEDec(drv["ItmCost"], 0), GVar.RndDecs.Amtpt), "D", "C");
        //            dr["LogFD"] = DocComUtility.PT(GFunc.RndC(GFunc.NEDec(drv["ItmQty"], 0) * GFunc.NEDec(drv["ItmCost"], 0), GVar.RndDecs.Amtpt), "D", "D");
        //            dr["LogHC"] = DocComUtility.PT(GFunc.RndC(GFunc.NEDec(drv["ItmQty"], 0) * GFunc.NEDec(drv["ItmCost"], 0), GVar.RndDecs.Amtpt), "D", "C");
        //            dr["LogHD"] = DocComUtility.PT(GFunc.RndC(GFunc.NEDec(drv["ItmQty"], 0) * GFunc.NEDec(drv["ItmCost"], 0), GVar.RndDecs.Amtpt), "D", "D");
        //            dr["LogRecon"] = false;
        //            dr["LogReconPeriod"] = 0;
        //            dtPost.Rows.Add(dr);
        //        }
        //        #endregion
        //        dtItems.DefaultView.RowFilter = "";
        //        DTPostRnd(cn, objDoc, dtPost);
        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dr = null;
        //    }
        //}
        //private static DataTable DTPostSysLogBatch(SqlConnection cn, Document objDoc, int ButtonAction)
        //{
        //    #region Declaration
        //    DataRow dr = null;
        //    DataTable dtPost = new DataTable();
        //    #endregion

        //    #region Create dtPost structure
        //    dtPost.Columns.Add("LogBatchDateTime", typeof(DateTime));
        //    dtPost.Columns.Add("LogBatchDate", typeof(DateTime));
        //    dtPost.Columns.Add("LogBatchMode", typeof(int));
        //    dtPost.Columns.Add("LogBatchDC", typeof(int));
        //    dtPost.Columns.Add("LogBatchDK", typeof(int));
        //    dtPost.Columns.Add("LogDocID", typeof(string));
        //    dtPost.Columns.Add("LogDocDate", typeof(DateTime));
        //    dtPost.Columns.Add("LogDocTypeNm", typeof(string));
        //    dtPost.Columns.Add("LogBatchPostDone", typeof(Boolean));
        //    dtPost.Columns.Add("LogBatchPostDate", typeof(DateTime));
        //    dtPost.Columns.Add("LogUserKey", typeof(int));
        //    dtPost.Columns.Add("PostByUserKey", typeof(int));
        //    dtPost.Columns.Add("Custom1", typeof(string));
        //    dtPost.Columns.Add("Custom2", typeof(string));
        //    dtPost.Columns.Add("Custom3", typeof(string));
        //    #endregion

        //    try
        //    {
        //        #region Set variables
        //        string docID = string.Empty;
        //        int logBatchMode = 0;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        if (objDoc.IsNew)
        //            logBatchMode = (int)GEnum.CosBatchLogMode.Add;
        //        else if (ButtonAction == (int)GEnum.DocAction.Delete)
        //            logBatchMode = (int)GEnum.CosBatchLogMode.Delete;
        //        else
        //            logBatchMode = (int)GEnum.CosBatchLogMode.Edit;
        //        #endregion

        //        dr = dtPost.NewRow();
        //        dr["LogBatchDateTime"] = objDoc.DocDate;
        //        dr["LogBatchDate"] = objDoc.DocDate;
        //        dr["LogBatchMode"] = logBatchMode;
        //        dr["LogBatchDC"] = objDoc.DocCodeKey;
        //        dr["LogBatchDK"] = objDoc.DocKey;
        //        dr["LogDocID"] = docID;
        //        dr["LogDocDate"] = objDoc.DocDate;
        //        dr["LogDocTypeNm"] = objDoc.DocTypeNm;
        //        dr["LogBatchPostDone"] = false;
        //        dr["LogUserKey"] = AppInfor.CurrentUserKey;
        //        dr["PostByUserKey"] = 0;
        //        dtPost.Rows.Add(dr);

        //        return dtPost;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //private static bool DTPostRnd(Document objDoc, DataTable dtPost)
        //{
        //    #region Vars Declaration
        //    DataRow dr = null;

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = 0;
        //    int? DocTranGrpKey = 0;
        //    int? DocGrpKey = 0;

        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    int? DocAccKey = SysOptionUtility.GetInt("AccRoundingAdj");//Get Option Rounding Adj Account


        //    decimal? sum_HC = 0;
        //    decimal? sum_HD = 0;
        //    decimal? RH = 0;
        //    decimal? FC = 0;
        //    decimal? FD = 0;
        //    decimal? HC = 0;
        //    decimal? HD = 0;
        //    #endregion

        //    try
        //    {
        //        //Dept
        //        switch (objDoc.DocCodeKey)
        //        {

        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Deposit:
        //            case (int)GEnum.SystemCode.Bank_Revaluation:

        //                DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //                break;
        //            default:
        //                DocDeptKey = 0;
        //                break;

        //        }

        //        //TranGrp
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Journal:
        //            case (int)GEnum.SystemCode.Deposit:
        //            case (int)GEnum.SystemCode.Bank_Revaluation:
        //                DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //                break;
        //            default:
        //                DocTranGrpKey = 0;
        //                break;

        //        }

        //        //DocGrp
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Purchase_Request:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Received_Consignment:
        //            case (int)GEnum.SystemCode.Journal:
        //            case (int)GEnum.SystemCode.Deposit:
        //            case (int)GEnum.SystemCode.Bank_Revaluation:
        //                DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //                break;
        //            default:
        //                DocGrpKey = 0;
        //                break;

        //        }


        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        sum_HC = dtPost.AsEnumerable().Sum(row => row.Field<decimal>("LogHC"));
        //        sum_HD = dtPost.AsEnumerable().Sum(row => row.Field<decimal>("LogHD"));

        //        if (sum_HC != sum_HD)
        //        {
        //            RH = sum_HD - sum_HC;

        //            if (RH != 0)
        //            {
        //                #region Post Rnd
        //                if (RH > 0)//More Debit therefore must add to Credit
        //                {
        //                    FC = RH;
        //                    HC = RH;
        //                    FD = 0;
        //                    HD = 0;

        //                }
        //                else //More Credit therefore must add to Debit
        //                {

        //                    FC = 0;
        //                    HC = 0;
        //                    FD = -RH;
        //                    HD = -RH;
        //                }


        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = 0;
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = DocAccKey;
        //                dr["LogTrans"] = PostType.RND;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = 0;
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = 0;
        //                dr["DocTranGrpKey"] = 0;
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = 0;
        //                dr["DocCurrRate"] = 0;
        //                dr["DocCVKey"] = 0;
        //                dr["DocCVNmDoc"] = string.Empty;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = string.Empty;
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = FC;
        //                dr["LogFD"] = FD;
        //                dr["LogHC"] = HC;
        //                dr["LogHD"] = HD;
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);

        //                #endregion

        //            }

        //        }
        //        return true;

        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dr = null;
        //    }

        //}
        //private static bool DTPostRnd(SqlConnection cn, Document objDoc, DataTable dtPost)
        //{
        //    #region Vars Declaration
        //    DataRow dr = null;

        //    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate, DateTime.Today).ToString("yyyyMM"));
        //    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        //    int? DocDeptKey = 0;
        //    int? DocTranGrpKey = 0;
        //    int? DocGrpKey = 0;

        //    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        //    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        //    int? DocAccKey = SysOptionUtility.GetInt("AccRoundingAdj", cn);//Get Option Rounding Adj Account


        //    decimal? sum_HC = 0;
        //    decimal? sum_HD = 0;
        //    decimal? RH = 0;
        //    decimal? FC = 0;
        //    decimal? FD = 0;
        //    decimal? HC = 0;
        //    decimal? HD = 0;
        //    #endregion

        //    try
        //    {
        //        //Dept
        //        switch (objDoc.DocCodeKey)
        //        {

        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Deposit:
        //            case (int)GEnum.SystemCode.Bank_Revaluation:
        //                DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        //                break;

        //            default:
        //                DocDeptKey = 0;
        //                break;

        //        }

        //        //TranGrp
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Journal:
        //            case (int)GEnum.SystemCode.Deposit:
        //            case (int)GEnum.SystemCode.Bank_Revaluation:
        //                DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        //                break;

        //            default:
        //                DocTranGrpKey = 0;
        //                break;

        //        }

        //        //DocGrp
        //        switch (objDoc.DocCodeKey)
        //        {
        //            case (int)GEnum.SystemCode.Quotation:
        //            case (int)GEnum.SystemCode.Sales_Order:
        //            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Delivery_Order:
        //            case (int)GEnum.SystemCode.Sales_Invoice:
        //            case (int)GEnum.SystemCode.Sales_Debit_Note:
        //            case (int)GEnum.SystemCode.Sales_Credit_Note:
        //            case (int)GEnum.SystemCode.Sales_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Received:
        //            case (int)GEnum.SystemCode.Contra:
        //            case (int)GEnum.SystemCode.Cash_Sale:
        //            case (int)GEnum.SystemCode.Cash_Debit_Note:
        //            case (int)GEnum.SystemCode.Cash_Credit_Note:
        //            case (int)GEnum.SystemCode.Cash_Adjustment:
        //            case (int)GEnum.SystemCode.Cash_Payment_Received:
        //            case (int)GEnum.SystemCode.Cash_Contra:
        //            case (int)GEnum.SystemCode.Purchase_Plan:
        //            case (int)GEnum.SystemCode.Purchase_Request:
        //            case (int)GEnum.SystemCode.Purchase_Order:
        //            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Purchase_Delivery:
        //            case (int)GEnum.SystemCode.Purchase_Invoice:
        //            case (int)GEnum.SystemCode.Purchase_Debit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Credit_Note:
        //            case (int)GEnum.SystemCode.Purchase_Adjustment:
        //            case (int)GEnum.SystemCode.Payment_Issue:
        //            case (int)GEnum.SystemCode.Inventory_Adjustment:
        //            case (int)GEnum.SystemCode.Inventory_Production:
        //            case (int)GEnum.SystemCode.Inventory_Transfer:
        //            case (int)GEnum.SystemCode.Issue_Consignment:
        //            case (int)GEnum.SystemCode.Return_Consignment:
        //            case (int)GEnum.SystemCode.Order_Consignment:
        //            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
        //            case (int)GEnum.SystemCode.Received_Consignment:
        //            case (int)GEnum.SystemCode.Journal:
        //            case (int)GEnum.SystemCode.Deposit:
        //            case (int)GEnum.SystemCode.Bank_Revaluation:
        //                DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);
        //                break;

        //            default:
        //                DocGrpKey = 0;
        //                break;

        //        }


        //        string docID = string.Empty;
        //        if (objDoc.DocState == (int)GEnum.DocState.New)
        //            docID = docAutoID;
        //        else
        //            docID = objDoc.DocID;

        //        sum_HC = dtPost.AsEnumerable().Sum(row => row.Field<decimal>("LogHC"));
        //        sum_HD = dtPost.AsEnumerable().Sum(row => row.Field<decimal>("LogHD"));

        //        if (sum_HC != sum_HD)
        //        {
        //            RH = sum_HD - sum_HC;

        //            if (RH != 0)
        //            {
        //                #region Post Rnd
        //                if (RH > 0)//More Debit therefore must add to Credit
        //                {
        //                    FC = RH;
        //                    HC = RH;
        //                    FD = 0;
        //                    HD = 0;

        //                    //to remove after test
        //                    //if (objDoc.DocSign == 1)
        //                    //{
        //                    //    FC = RH;
        //                    //    HC = RH;
        //                    //    FD = 0;
        //                    //    HD = 0;
        //                    //}
        //                    //else
        //                    //{
        //                    //    FC = 0;
        //                    //    HC = 0;
        //                    //    FD = RH;
        //                    //    HD = RH;
        //                    //}
        //                }
        //                else //More Credit therefore must add to Debit
        //                {
        //                    FC = 0;
        //                    HC = 0;
        //                    FD = -RH;   //need to make sure that the value posted is +ve 
        //                    HD = -RH;   //need to make sure that the value posted is +ve 

        //                    //to remove after test
        //                    //if (objDoc.DocSign == 1)
        //                    //{
        //                    //    FC = 0;
        //                    //    HC = 0;
        //                    //    FD = RH;
        //                    //    HD = RH;
        //                    //}
        //                    //else
        //                    //{
        //                    //    FC = RH;
        //                    //    HC = RH;
        //                    //    FD = 0;
        //                    //    HD = 0;
        //                    //}
        //                }


        //                dr = dtPost.NewRow();
        //                dr["LogDC"] = objDoc.DocCodeKey;
        //                dr["LogDK"] = objDoc.DocKey;
        //                dr["LogDItm"] = 0;
        //                dr["LogLineType"] = 0;
        //                dr["LogAccKey"] = DocAccKey;
        //                dr["LogTrans"] = PostType.RND;
        //                dr["LogTaxKey"] = 0;
        //                dr["LogSeq"] = 0;
        //                dr["DocID"] = docID;
        //                dr["DocDate"] = objDoc.DocDate;
        //                dr["DocPeriod"] = DocPeriod;
        //                dr["DocBranchKey"] = DocBranchKey;
        //                dr["DocDeptKey"] = 0;
        //                dr["DocTranGrpKey"] = 0;
        //                dr["DocGrpKey"] = DocGrpKey;
        //                dr["DocType"] = objDoc.DocType;
        //                dr["DocTypeNm"] = objDoc.DocTypeNm;
        //                dr["DocCurrkey"] = 0;
        //                dr["DocCurrRate"] = 1;
        //                dr["DocCVKey"] = 0;
        //                dr["DocCVNmDoc"] = string.Empty;
        //                dr["DocChqNum"] = string.Empty;
        //                dr["DocRef"] = DocRef;
        //                dr["DocDes"] = DocDes;
        //                dr["DetItmKey"] = 0;
        //                dr["DetItmKeySelect"] = 0;
        //                dr["DetItmDes"] = string.Empty;
        //                dr["DetRef"] = string.Empty;
        //                dr["DetJobKey"] = 0;
        //                dr["DetJobPhaseKey"] = 0;
        //                dr["DetJobTaskKey"] = 0;
        //                dr["DetJobCostTypeKey"] = 0;
        //                dr["LogFC"] = FC;
        //                dr["LogFD"] = FD;
        //                dr["LogHC"] = HC;
        //                dr["LogHD"] = HD;
        //                dr["LogRecon"] = false;
        //                dr["LogReconPeriod"] = 0;
        //                dtPost.Rows.Add(dr);

        //                #endregion

        //            }

        //        }
        //        return true;

        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //    finally
        //    {
        //        dr = null;
        //    }

        //}
        ////private static bool DTPostRnd(SqlConnection cn, Document objDoc, DataTable dtPost)
        ////{
        ////    #region Vars Declaration
        ////    DataRow dr = null;

        ////    decimal? DocPeriod = Convert.ToInt32(GFunc.NEDateTime(objDoc.DocDate,DateTime.Today).ToString("yyyyMM"));
        ////    int? DocBranchKey = GFunc.GetIntPropertyValue("BranchKey", objDoc);
        ////    int? DocDeptKey = GFunc.GetIntPropertyValue("DocDeptKey", objDoc);
        ////    int? DocTranGrpKey = GFunc.GetIntPropertyValue("DocTranGrpKey", objDoc);
        ////    int? DocGrpKey = GFunc.GetIntPropertyValue("DocGrpKey", objDoc);

        ////    string DocRef = GFunc.GetStringPropertyValue("DocRef", objDoc);
        ////    string DocDes = GFunc.GetStringPropertyValue("DocDes", objDoc);

        ////    int? DocAccKey = SysOptionUtility.GetInt("AccRoundingAdj", cn);//Get Option Rounding Adj Account


        ////    decimal? sum_HC = 0;
        ////    decimal? sum_HD = 0;
        ////    decimal? RH = 0;
        ////    decimal? FC = 0;
        ////    decimal? FD = 0;
        ////    decimal? HC = 0;
        ////    decimal? HD = 0;
        ////    #endregion

        ////    try
        ////    {
        ////        string docID = string.Empty;
        ////        if (objDoc.DocState == (int)GEnum.DocState.New)
        ////            docID = docAutoID;
        ////        else
        ////            docID = objDoc.DocID;

        ////        sum_HC = dtPost.AsEnumerable().Sum(row => row.Field<decimal?>("LogHC"));
        ////        sum_HD = dtPost.AsEnumerable().Sum(row => row.Field<decimal?>("LogHD"));

        ////        if (sum_HC != sum_HD)
        ////        {
        ////            RH = sum_HD - sum_HC;

        ////            if (RH != 0)
        ////            {
        ////                #region Post Rnd
        ////                if (RH > 0)//More Debit therefore must add to Credit
        ////                {
        ////                    FC = RH;
        ////                    HC = RH;
        ////                    FD = 0;
        ////                    HD = 0;

        ////                }
        ////                else //More Credit therefore must add to Debit
        ////                {

        ////                    FC = 0;
        ////                    HC = 0;
        ////                    FD = RH;
        ////                    HD = RH;
        ////                }


        ////                dr = dtPost.NewRow();
        ////                dr["LogDC"] = objDoc.DocCodeKey;
        ////                dr["LogDK"] = objDoc.DocKey;
        ////                dr["LogDItm"] = 0;
        ////                dr["LogLineType"] = 0;
        ////                dr["LogAccKey"] = DocAccKey;
        ////                dr["LogTrans"] = PostType.RND;
        ////                dr["LogTaxKey"] = 0;
        ////                dr["LogSeq"] = 0;
        ////                dr["DocID"] = docID;
        ////                dr["DocDate"] = objDoc.DocDate;
        ////                dr["DocPeriod"] = DocPeriod;
        ////                dr["DocBranchKey"] = DocBranchKey;
        ////                dr["DocDeptKey"] = 0;
        ////                dr["DocTranGrpKey"] = 0;
        ////                dr["DocGrpKey"] = DocGrpKey;
        ////                dr["DocType"] = objDoc.DocType;
        ////                dr["DocTypeNm"] = objDoc.DocTypeNm;
        ////                dr["DocCurrkey"] = 0;
        ////                dr["DocCurrRate"] = 1;
        ////                dr["DocCVKey"] = 0;
        ////                dr["DocCVNmDoc"] = string.Empty;
        ////                dr["DocChqNum"] = string.Empty;
        ////                dr["DocRef"] = DocRef;
        ////                dr["DocDes"] = DocDes;
        ////                dr["DetItmKey"] = 0;
        ////                dr["DetItmKeySelect"] = 0;
        ////                dr["DetItmDes"] = string.Empty;
        ////                dr["DetRef"] = string.Empty;
        ////                dr["DetJobKey"] = 0;
        ////                dr["DetJobPhaseKey"] = 0;
        ////                dr["DetJobTaskKey"] = 0;
        ////                dr["DetJobCostTypeKey"] = 0;
        ////                dr["LogFC"] = FC;
        ////                dr["LogFD"] = FD;
        ////                dr["LogHC"] = HC;
        ////                dr["LogHD"] = HD;
        ////                dr["LogRecon"] = false;
        ////                dr["LogReconPeriod"] = 0;
        ////                dtPost.Rows.Add(dr);

        ////                #endregion

        ////            }

        ////        }
        ////        return true;

        ////    }
        ////    catch (TAException taex)
        ////    {
        ////        throw Error(taex);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw Error(ex);
        ////    }
        ////    finally
        ////    {
        ////        dr = null;
        ////    }

        ////}

        //public static DataSet Doc_Copy(int sourceDocCodeKey, int copyDocKey, int destinationDocCodeKey, Document docNew, bool NSLink)
        //{
        //    try
        //    {
        //        List<SqlParameter> paraList = new List<SqlParameter>();
        //        paraList.Add(new SqlParameter("@SourceDocCodeKey", sourceDocCodeKey));
        //        paraList.Add(new SqlParameter("@destinationDocCodeKey", destinationDocCodeKey));
        //        paraList.Add(new SqlParameter("@CopyDocKey", copyDocKey));
        //        paraList.Add(new SqlParameter("@DocKey", docNew.DocKey));
        //        paraList.Add(new SqlParameter("@DocCodeKey", docNew.DocCodeKey));
        //        paraList.Add(new SqlParameter("@DocSign", docNew.DocSign));
        //        paraList.Add(new SqlParameter("@DocID", docNew.DocID));
        //        paraList.Add(new SqlParameter("@DocType", docNew.DocType));
        //        paraList.Add(new SqlParameter("@DocTypeNm", docNew.DocTypeNm));
        //        paraList.Add(new SqlParameter("@NSLink", NSLink));
        //        paraList.Add(new SqlParameter("@UserKey", AppInfor.currentUserKey));
                
        //        return GFunc.ExecuteProcDataSet("Document_Copy", paraList); ;
        //    }
        //    catch (TAException taex)
        //    {
        //        throw Error(taex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //public static bool AttachmentSave(SqlConnection cn, SYSAttachments attachments, Document objDoc)
        //{
        //    //Saving or Deleting Attachments in Documents
        //    try
        //    {
        //        #region Declaration
        //        List<SqlParameter> list = new List<SqlParameter>();
        //        DataTable dtAttachment = new DataTable("Table1");
        //        dtAttachment.Columns.Add("AttachDes", typeof(string));
        //        dtAttachment.Columns.Add("AttachFileType", typeof(string));
        //        dtAttachment.Columns.Add("AttachPath", typeof(string));
        //        dtAttachment.Columns.Add("AttachSize", typeof(int));
        //        dtAttachment.Columns.Add("Custom1", typeof(string));
        //        dtAttachment.Columns.Add("Custom2", typeof(string));
        //        dtAttachment.Columns.Add("Custom3", typeof(string));
        //        dtAttachment.Columns.Add("DocDC", typeof(int));
        //        dtAttachment.Columns.Add("DocDetailType", typeof(int));
        //        dtAttachment.Columns.Add("DocDItm", typeof(int));
        //        dtAttachment.Columns.Add("DocDK", typeof(int));
        //        dtAttachment.Columns.Add("Seq", typeof(int));
        //        #endregion

        //        if (GFunc.IsNE(attachments))
        //        {
        //            list.Add(new SqlParameter("@Option", 1));
        //            list.Add(new SqlParameter("@DocDC", objDoc.DocCodeKey));
        //            list.Add(new SqlParameter("@DocDK", objDoc.DocKey));
        //            list.Add(new SqlParameter("@RetValue", 0));
        //            list[3].Direction = ParameterDirection.Output;
        //            GFunc.ExecuteNonQueryProc(cn, "sysAttachment_Delete", list);

        //            if (GFunc.NEInt(list[1].Value, 0) == (int)GEnum.SpState.Pass)
        //                return true;
        //            else
        //                return false;
        //        }

        //        foreach (SYSAttachment attach in attachments)
        //        {
        //            dtAttachment.Rows.Add(new object[] { attach.AttachDes, attach.AttachFileType, attach.AttachPath, attach.AttachSize, attach.Custom1, attach.Custom2, attach.Custom3, attach.DocDC, attach.DocDetailType, attach.DocDItm, attach.DocDK, attach.Seq });
        //        }

        //        string xmlAttachment = GFunc.ConvertDataTableToXML(dtAttachment);
        //        list.Add(new SqlParameter("@Attachment", xmlAttachment));
        //        list.Add(new SqlParameter("@RetValue", 0));
        //        list[1].Direction = ParameterDirection.Output;
        //        GFunc.ExecuteNonQueryProc(cn, "sysAttachment_Save", list);

        //        if (GFunc.NEInt(list[1].Value, 0) == (int)GEnum.SpState.Pass)
        //            return true;
        //        else
        //            return false;
        //    }
        //    catch (TAException ex)
        //    {
        //        throw Error(ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed
        //public static bool AttachmentSave(SqlConnection cn, SYSAttachments attachments, GEnum.SystemCode DocDc, int? DocKey)
        //{
        //    //Saving or Deleting Attachments in Master records
        //    try
        //    {
        //        #region Declaration
        //        List<SqlParameter> list = new List<SqlParameter>();
        //        DataTable dtAttachment = new DataTable("Table1");
        //        dtAttachment.Columns.Add("AttachDes", typeof(string));
        //        dtAttachment.Columns.Add("AttachFileType", typeof(string));
        //        dtAttachment.Columns.Add("AttachPath", typeof(string));
        //        dtAttachment.Columns.Add("AttachSize", typeof(int));
        //        dtAttachment.Columns.Add("Custom1", typeof(string));
        //        dtAttachment.Columns.Add("Custom2", typeof(string));
        //        dtAttachment.Columns.Add("Custom3", typeof(string));
        //        dtAttachment.Columns.Add("DocDC", typeof(int));
        //        dtAttachment.Columns.Add("DocDetailType", typeof(int));
        //        dtAttachment.Columns.Add("DocDItm", typeof(int));
        //        dtAttachment.Columns.Add("DocDK", typeof(int));
        //        dtAttachment.Columns.Add("Seq", typeof(int));
        //        #endregion

        //        if (GFunc.IsNE(attachments))
        //        {
        //            list.Add(new SqlParameter("@Option", 1));
        //            list.Add(new SqlParameter("@DocDC", DocDc));
        //            list.Add(new SqlParameter("@DocDK", DocKey));
        //            list.Add(new SqlParameter("@RetValue", 0));
        //            list[3].Direction = ParameterDirection.Output;
        //            GFunc.ExecuteNonQueryProc(cn, "sysAttachment_Delete", list);

        //            if (GFunc.NEInt(list[1].Value, 0) == (int)GEnum.SpState.Pass)
        //                return true;
        //            else
        //                return false;
        //        }

        //        foreach (SYSAttachment attach in attachments)
        //        {
        //            dtAttachment.Rows.Add(new object[] { attach.AttachDes, attach.AttachFileType, attach.AttachPath, attach.AttachSize, attach.Custom1, attach.Custom2, attach.Custom3, attach.DocDC, attach.DocDetailType, attach.DocDItm, attach.DocDK, attach.Seq });
        //        }

        //        string xmlAttachment = GFunc.ConvertDataTableToXML(dtAttachment);
        //        list.Add(new SqlParameter("@Attachment", xmlAttachment));
        //        list.Add(new SqlParameter("@RetValue", 0));
        //        list[1].Direction = ParameterDirection.Output;
        //        GFunc.ExecuteNonQueryProc(cn, "sysAttachment_Save", list);

        //        if (GFunc.NEInt(list[1].Value, 0) == (int)GEnum.SpState.Pass)
        //            return true;
        //        else
        //            return false;
        //    }
        //    catch (TAException ex)
        //    {
        //        throw Error(ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}//Completed

        ////Set Error Methods
        //private static Exception Error(Exception ex)
        //{
        //    try
        //    {
        //        ex = SysAuditLogUtility.ModifyException(ex, false);
        //    }
        //    catch (Exception nex)
        //    {
        //        MsgBox.Show(nex.Message);
        //    }
        //    return ex;
        //}
        //private static TAException Error(TAException ex)
        //{
        //    try
        //    {
        //        ex = SysAuditLogUtility.ModifyTAException(ex, false);
        //    }
        //    catch (Exception nex)
        //    {
        //        MsgBox.Show(nex.Message);
        //    }
        //    return ex;
        //}
    }
}
