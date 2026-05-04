using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infragistics.Win.UltraWinGrid;
using System.Data;
using System.Transactions;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using BOLib;
using Infragistics.Win.UltraWinMaskedEdit;
using TAUtil;
using System.Drawing;
using static BOLib.GEnum;

namespace WinUI
{
    public class DocDetUtil
    {
        //Grid Common Event
        
        public static bool AutoIncrement(int docCodeKey, UltraGrid grd)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = grd.DataSource as DataTable;

                switch (docCodeKey)
                {
                    #region DocDetItm DocItmKey, ItmSN

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
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:

                        if (dt.Columns.Contains("DocItmKey"))
                        {
                            dt.Columns["DocItmKey"].DefaultValue = DocComUtility.GridAutoID_Get(grd, "DocKey", "DocItmKey");
                        }
                        if (dt.Columns.Contains("DocItmDetKey"))
                        {
                            dt.Columns["DocItmDetKey"].DefaultValue = GFunc.NEInt(dt.Compute("Max(DocItmDetKey)", "DocItmKey =" + dt.Columns["DocItmKey"].DefaultValue), 0) + 1;
                        }
                        if (dt.Columns.Contains("DetItmSN"))
                        {
                            dt.Columns["DetItmSN"].DefaultValue = GFunc.NEInt(dt.Compute("Max(DetItmSN)", "DocItmDetKey =" + grd.ActiveRow.Cells["DocItmDetKey"].Value), 0) + 1;
                        }
                        else if (dt.Columns.Contains("ItmSN"))
                        {
                            dt.Columns["ItmSN"].DefaultValue = dt.Rows.Count == 0 ? 1 : dt.AsEnumerable().Max(p => p.Field<decimal>("ItmSN")) + 1;
                        }
                        if (dt.Columns.Contains("ExpSN"))
                        {
                            dt.Columns["ExpSN"].DefaultValue = dt.Rows.Count == 0 ? 1 : dt.AsEnumerable().Max(p => p.Field<decimal>("ExpSN")) + 1;
                        }

                        break;

                    #endregion

                    #region Packing List

                    case (int)GEnum.SystemCode.Packing_List:

                        if (GFunc.CompareString(grd.Name, "tagrdDetPack"))
                        {
                            if (dt.Columns.Contains("DocItmKey"))
                            {
                                dt.Columns["DocItmKey"].DefaultValue = DocComUtility.GridAutoID_Get(grd, "DocKey", "DocItmKey");
                            }
                        }
                        else
                        {
                            if (dt.Columns.Contains("DocItmDetKey"))
                            {
                                dt.Columns["DocItmDetKey"].DefaultValue = DocComUtility.GridAutoID_Get(grd, "DocKey", "DocItmDetKey");
                            }
                        }

                        if (dt.Columns.Contains("DetItmSN"))
                        {
                            dt.Columns["DetItmSN"].DefaultValue = GFunc.NEInt(dt.Compute("Max(DetItmSN)", "DocItmKey =" + dt.Columns["DocItmKey"].DefaultValue), 0) + 1;
                        }
                        else if (dt.Columns.Contains("ItmSN"))
                        {
                            dt.Columns["ItmSN"].DefaultValue = dt.Rows.Count == 0 ? 1 : dt.AsEnumerable().Max(p => p.Field<decimal>("ItmSN")) + 1;
                        }
                        break;

                    #endregion

                    #region Production

                    case (int)GEnum.SystemCode.Inventory_Production:

                        dt.Columns["DocItmKey"].DefaultValue = DocComUtility.GridAutoID_Get(grd, "DocKey", "DocItmKey"); ;

                        if (dt.Columns.Contains("ItmSN"))
                        {
                            int linetype = 0;
                            switch (grd.Name.ToLower())
                            {
                                case "tagrddetfinished":
                                    linetype = 3000;
                                    break;

                                case "tagrddetraw":
                                    linetype = 3100;
                                    break;

                                default:   //case "tagrddetpacking":
                                    linetype = 3200;
                                    break;
                            }
                            dt.Columns["ItmSN"].DefaultValue = GFunc.NEInt(dt.Compute("Max(ItmSN)", "LineType =" + linetype), 0) + 1;                            
                        }
                        break;

                    #endregion

                    #region Master and Reference and others
                    //Add require additional code key
                    case (int)GEnum.SystemCode.Job:
                        if (dt.Columns.Contains("JobEstKey"))
                        {
                            dt.Columns["JobEstKey"].DefaultValue = DocComUtility.GridAutoID_Get(grd, "JobKey", "JobEstKey");
                        }
                        if (dt.Columns.Contains("JobOtherKey"))
                        {
                            dt.Columns["JobOtherKey"].DefaultValue = DocComUtility.GridAutoID_Get(grd, "JobKey", "JobOtherKey");
                        }
                        if (dt.Columns.Contains("EstSN"))
                        {

                            dt.Columns["EstSN"].DefaultValue = dt.Rows.Count == 0 ? 1 : dt.AsEnumerable().Max(p => p.Field<decimal>("EstSN")) + 1;
                        }

                        break;
                    case (int)GEnum.SystemCode.Inventory:
                     
                        if (dt.Columns.Contains("AssSN"))
                        {
                            dt.Columns["AssSN"].DefaultValue = dt.Rows.Count == 0 ? 1 : dt.AsEnumerable().Max(p => p.Field<decimal>("AssSN")) + 1;
                        }

                        break;

                    #endregion
                }
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
            finally
            {
                dt = null;
            }
        }//Completed
        public static void ItmRow_AfterRowActivate(Document objDoc, UltraGrid grd)
        {
            try
            {
                GlobalUI.PopupRefresh(grd);
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
        public static bool ItmRow_AddBlankRow(Document objDoc, Hashtable details, GEnum.Details docDetailType)
        {
            try
            {
                //use in inserting new row in document detail thru shortcut menu "insert row"
                UltraGrid grd = null;
                DocComUtility.DocDetail_Get(docDetailType, details, ref grd);
                DataTable dt = grd.DataSource as DataTable;
                int DocItmKey = 0;
                string DocumentDetKeyStr = "DocItmKey";

                
                if (docDetailType == GEnum.Details.Doc_Itm)
                {
                    switch (objDoc.DocCodeKey)
                    {
                        #region DocDetItm ItmSN
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
                        case (int)GEnum.SystemCode.Purchase_Request:
                        case (int)GEnum.SystemCode.Purchase_Order:
                        case (int)GEnum.SystemCode.Purchase_Shipment:
                        case (int)GEnum.SystemCode.Purchase_Delivery:
                        case (int)GEnum.SystemCode.Purchase_Invoice:
                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        case (int)GEnum.SystemCode.Issue_Consignment:
                        case (int)GEnum.SystemCode.Order_Consignment:
                        case (int)GEnum.SystemCode.Journal:
                            if (grd.ActiveRow != null)
                            {
                                decimal LastSN = 0;
                                if (grd.ActiveRow.IsAddRow)
                                {
                                    if (dt.Rows.Count == 0)
                                        LastSN = 1;
                                    else
                                        LastSN = dt.AsEnumerable().Max(p => p.Field<decimal>("ItmSN")) + 1;

                                    if (grd.ActiveRow.IsUnmodifiedTemplateAddRow == false)
                                    {
                                        //save the uncommmitted new row before we start to insert the new row
                                        if (grd.ActiveRow.Update() == false)
                                            return false;
                                    }
                                }
                                else
                                {
                                    LastSN = GFunc.NEDec(grd.ActiveRow.Cells["ItmSN"].Value, 0);
                                }

                                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Packing_List)
                                {
                                    DocItmKey = DocComUtility.GridAutoID_Get(grd, "DocKey", "DocDetItmKey");
                                    DocumentDetKeyStr = "DocDetItmKey";
                                }
                                else
                                    DocItmKey = DocComUtility.GridAutoID_Get(grd, "DocKey", "DocItmKey");

                                if (objDoc.DocCodeKey != (int)GEnum.SystemCode.Journal) //Not Journal
                                {
                                    if (DocDetUtil.ItmRow_AddBlankRowRemark(objDoc, grd, DocItmKey, LastSN))
                                    {
                                        DocComUtility.CalForm(objDoc, details, true, false);
                                        UltraGridRow grdRow = grd.Rows.OfType<UltraGridRow>().ToList().Find(r => r.Cells[DocumentDetKeyStr].Value.ToString().Equals(DocItmKey.ToString()));
                                        grd.ActiveRow = grdRow;
                                    }
                                    else
                                        return false;
                                }
                                else//Journal
                                {
                                    if (DocDetUtil.ItmRow_AddBlankRowJournal(objDoc, grd, DocItmKey, LastSN))
                                    {
                                        DocComUtility.CalForm(objDoc, details, true, false);
                                        UltraGridRow grdRow = grd.Rows.OfType<UltraGridRow>().ToList().Find(r => r.Cells[DocumentDetKeyStr].Value.ToString().Equals(DocItmKey.ToString()));
                                        grd.ActiveRow = grdRow;
                                    }
                                    else
                                        return false;
                                }
                            }
                            break;
                        #endregion
                    }
                }
                else if (docDetailType == GEnum.Details.Doc_Exp)
                {
                    switch (objDoc.DocCodeKey)
                    {
                        #region DocDetItm ItmSN
                        case (int)GEnum.SystemCode.Cash_Payment_Received:
                        case (int)GEnum.SystemCode.Payment_Received:
                        case (int)GEnum.SystemCode.Payment_Issue:                       
                            if (grd.ActiveRow != null)
                            {
                                decimal LastSN = 0;
                                if (grd.ActiveRow.IsAddRow)
                                {
                                    if (dt.Rows.Count == 0)
                                        LastSN = 1;
                                    else
                                        LastSN = dt.AsEnumerable().Max(p => p.Field<decimal>("ExpSN")) + 1;

                                    if (grd.ActiveRow.IsUnmodifiedTemplateAddRow == false)
                                    {
                                        //save the uncommmitted new row before we start to insert the new row
                                        if (grd.ActiveRow.Update() == false)
                                            return false;
                                    }
                                }
                                else
                                {
                                    LastSN = GFunc.NEDec(grd.ActiveRow.Cells["ExpSN"].Value, 0);
                                }

                                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Packing_List)
                                {
                                    DocItmKey = DocComUtility.GridAutoID_Get(grd, "DocKey", "DocDetItmKey");
                                    DocumentDetKeyStr = "DocDetItmKey";
                                }
                                else
                                    DocItmKey = DocComUtility.GridAutoID_Get(grd, "DocKey", "DocItmKey");

                                if (DocDetUtil.ItmRow_AddBlankRowExpGrid(objDoc, grd, DocItmKey, LastSN))
                                {
                                    DocComUtility.CalForm(objDoc, details, true, false);
                                    UltraGridRow grdRow = grd.Rows.OfType<UltraGridRow>().ToList().Find(r => r.Cells[DocumentDetKeyStr].Value.ToString().Equals(DocItmKey.ToString()));
                                    grd.ActiveRow = grdRow;
                                }
                                else
                                    return false;
                            }
                            break;
                        #endregion
                    }
                }
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
        public static bool ItmRow_AddBlankRowRemark(Document objDoc, UltraGrid grd, int DocItmKey, decimal DocItmSN)
        {
            try
            {
                #region Declaration of variables
                DataTable dtItm = null;
                MSTItm objItm = null;

                int key = SysOptionUtility.GetInt(GVar.SystemOption.Document_Defaults.DefaultItmRemark);
                int ItmKey = 0;
                int ItmKeySelected = 0;
                int ItmType = 0;
                string ItmID = string.Empty;
                string ItmDes = string.Empty;
                int ItmTypeGrp = 0;
                #endregion

                dtItm = (DataTable)grd.DataSource;
                DataRow drNew = dtItm.NewRow();

                #region Get MSTItm Object
                objItm = MSTItm.Get(key);
                if (GFunc.NEInt(objItm.SubstituteItmKey, 0) > 0)
                {
                    ItmKey = (int)objItm.SubstituteItmKey;
                    ItmKeySelected = (int)objItm.ItmKey;
                    objItm = MSTItm.Get(key);
                }
                else
                {
                    ItmKey = (int)objItm.ItmKey;
                    ItmKeySelected = key;
                }
                ItmID = objItm.ItmID;
                ItmType = (int)objItm.ItmType;
                ItmTypeGrp = GFunc.GetINTypeGroup(ItmType);
                ItmDes = objItm.ItmDes;
                DocItmSN = DocItmSN - 0.5M;
                #endregion
                
                #region set values in document detail

                drNew["DocKey"] = objDoc.DocKey;

                switch (objDoc.DocCodeKey)
                {
                    #region ARQO
                    case (int)GEnum.SystemCode.Quotation:
                        drNew["DocItmKey"] = DocItmKey;
                        drNew["ItmSN"] = DocItmSN;
                        drNew["ItmKey"] = ItmKey;
                        drNew["ItmKeySelect"] = ItmKeySelected;
                        drNew["ItmID"] = ItmID;
                        drNew["ItmType"] = ItmType;
                        drNew["ItmDes"] = ItmDes;
                        drNew["ItmDeptKey"] = 0;
                        drNew["ItmTranGrpKey"] = 0;
                        drNew["ItmAccKey"] = DBNull.Value;
                        drNew["ItmLocKey"] = DBNull.Value;
                        drNew["ItmStock"] = DBNull.Value;
                        drNew["ItmQty"] = DBNull.Value;
                        drNew["ItmReqDate"] = DBNull.Value;
                        drNew["ItmPrmDate"] = DBNull.Value;
                        drNew["ItmUOMKey"] = DBNull.Value;
                        drNew["ItmConRate"] = DBNull.Value;
                        drNew["ItmLatestCostF"] = DBNull.Value;
                        drNew["ItmLatestCostH"] = DBNull.Value;
                        drNew["ItmListPrice"] = DBNull.Value;
                        drNew["ItmPriceBefore"] = DBNull.Value;
                        drNew["ItmPriceAfter"] = DBNull.Value;
                        drNew["ItmDisPercent"] = DBNull.Value;
                        drNew["ItmDisValue"] = DBNull.Value;
                        drNew["ItmPrice"] = 0M;
                        drNew["ItmPriceUser"] = DBNull.Value;
                        drNew["ItmControlPrice"] = DBNull.Value;
                        drNew["ItmControlPriceBase"] = DBNull.Value;
                        drNew["ItmAmtShw"] = DBNull.Value;
                        drNew["ItmAmtF"] = 0M;
                        drNew["ItmAmtH"] = 0M;
                        drNew["ItmTaxable"] = objItm.Taxable;
                        drNew["ItmTaxGrpKey"] = DBNull.Value;
                        drNew["ItmTaxGrpRate"] = 0M;
                        drNew["ItmTaxGrpAmtF"] = 0M;
                        drNew["ItmTaxGrpAmtL"] = 0M;
                        drNew["ItmColorKey"] = DBNull.Value;
                        drNew["ItmScaleSize"] = DBNull.Value;
                        drNew["ItmPacking"] = DBNull.Value;
                        drNew["ItmMark"] = DBNull.Value;
                        drNew["ItmVendorKey"] = DBNull.Value;
                        drNew["ItmVendorCurrKey"] = 1;
                        drNew["ItmVendorCurrRate"] = 1M;
                        drNew["ItmVendorPrice"] = 0M;
                        drNew["ItmVendorPriceRatio"] = 0M;
                        drNew["ItmJobKey"] = 0;
                        drNew["ItmJobPhaseKey"] = 0;
                        drNew["ItmJobTaskKey"] = 0;
                        drNew["ItmJobCostTypeKey"] = 0;
                        drNew["ItmIGrpDItm"] = 0;
                        drNew["ItmIGrpQtyLock"] = false;
                        drNew["ItmIGrpToPrint"] = true;
                        drNew["ItmIGrpQtySet"] = 0;
                        drNew["ItmIGrpAmtSet"] = 0;
                        break;
                    #endregion

                    #region ARSO
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                        drNew["DocItmKey"] = DocItmKey;
                        drNew["ItmSN"] = DocItmSN;
                        drNew["ItmKey"] = ItmKey;
                        drNew["ItmKeySelect"] = ItmKeySelected;
                        drNew["ItmID"] = ItmID;
                        drNew["ItmType"] = ItmType;
                        drNew["ItmDes"] = ItmDes;
                        drNew["ItmDeptKey"] = 0;
                        drNew["ItmTranGrpKey"] = 0;
                        drNew["ItmAccKey"] = DBNull.Value;
                        drNew["ItmLocKey"] = DBNull.Value;
                        drNew["ItmStock"] = DBNull.Value;
                        drNew["ItmQty"] = DBNull.Value;
                        drNew["ItmQtyLink"] = DBNull.Value;
                        drNew["ItmQtyAdj"] = DBNull.Value;
                        drNew["ItmQtyBalance"] = DBNull.Value;
                        drNew["ItmOrderStatus"] = DBNull.Value;
                        drNew["ItmReqDate"] = DBNull.Value;
                        drNew["ItmPrmDate"] = DBNull.Value;
                        drNew["ItmUOMKey"] = DBNull.Value;
                        drNew["ItmConRate"] = DBNull.Value;
                        drNew["ItmLatestCostF"] = DBNull.Value;
                        drNew["ItmLatestCostH"] = DBNull.Value;
                        drNew["ItmListPrice"] = DBNull.Value;
                        drNew["ItmPriceBefore"] = DBNull.Value;
                        drNew["ItmPriceAfter"] = DBNull.Value;
                        drNew["ItmDisPercent"] = DBNull.Value;
                        drNew["ItmDisValue"] = DBNull.Value;
                        drNew["ItmPrice"] = 0M;
                        drNew["ItmPriceUser"] = DBNull.Value;
                        drNew["ItmControlPrice"] = DBNull.Value;
                        drNew["ItmControlPriceBase"] = DBNull.Value;
                        drNew["ItmAmtShw"] = DBNull.Value;
                        drNew["ItmAmtF"] = 0M;
                        drNew["ItmAmtH"] = 0M;
                        drNew["ItmTaxable"] = objItm.Taxable;
                        drNew["ItmTaxGrpKey"] = DBNull.Value;
                        drNew["ItmTaxGrpRate"] = 0M;
                        drNew["ItmTaxGrpAmtF"] = 0M;
                        drNew["ItmTaxGrpAmtL"] = 0M;
                        drNew["ItmColorKey"] = DBNull.Value;
                        drNew["ItmScaleSize"] = DBNull.Value;
                        drNew["ItmPacking"] = DBNull.Value;
                        drNew["ItmMark"] = DBNull.Value;
                        drNew["ItmVendorKey"] = DBNull.Value;
                        drNew["ItmVendorCurrKey"] = 1;
                        drNew["ItmVendorCurrRate"] = 1M;
                        drNew["ItmVendorPrice"] = 0;
                        drNew["ItmVendorPriceRatio"] = 0;
                        drNew["ItmJobKey"] = 0;
                        drNew["ItmJobPhaseKey"] = 0;
                        drNew["ItmJobTaskKey"] = 0;
                        drNew["ItmJobCostTypeKey"] = 0;
                        drNew["ItmIGrpDItm"] = 0;
                        drNew["ItmIGrpQtyLock"] = false;
                        drNew["ItmIGrpToPrint"] = true;
                        drNew["ItmIGrpQtySet"] = 0M;
                        drNew["ItmIGrpAmtSet"] = 0M;
                        drNew["ARQOID"] = DBNull.Value;
                        drNew["ARQODK"] = 0;
                        drNew["ARQODItm"] = 0;
                        break;
                    #endregion

                    #region ARDO
                    case (int)GEnum.SystemCode.Delivery_Order:
                        drNew["DocItmKey"] = DocItmKey;
                        drNew["ItmSN"] = DocItmSN;
                        drNew["ItmKey"] = ItmKey;
                        drNew["ItmKeySelect"] = ItmKeySelected;
                        drNew["ItmID"] = ItmID;
                        drNew["ItmType"] = objItm.ItmType;
                        drNew["ItmDes"] = ItmDes;
                        drNew["ItmDeptKey"] = 0;
                        drNew["ItmTranGrpKey"] = 0;
                        drNew["ItmAccKey"] = DBNull.Value;
                        drNew["ItmLocKey"] = DBNull.Value;
                        drNew["ItmStock"] = DBNull.Value;
                        drNew["ItmQty"] = DBNull.Value;
                        drNew["ItmUOMKey"] = DBNull.Value;
                        drNew["ItmConRate"] = DBNull.Value;
                        drNew["ItmLatestCostF"] = DBNull.Value;
                        drNew["ItmLatestCostH"] = DBNull.Value;
                        drNew["ItmListPrice"] = DBNull.Value;
                        drNew["ItmPriceBefore"] = DBNull.Value;
                        drNew["ItmPriceAfter"] = DBNull.Value;
                        drNew["ItmDisPercent"] = DBNull.Value;
                        drNew["ItmDisValue"] = DBNull.Value;
                        drNew["ItmPrice"] = 0M;
                        drNew["ItmPriceUser"] = DBNull.Value;
                        drNew["ItmControlPrice"] = DBNull.Value;
                        drNew["ItmControlPriceBase"] = DBNull.Value;
                        drNew["ItmAmtShw"] = DBNull.Value;
                        drNew["ItmAmtF"] = 0M;
                        drNew["ItmAmtH"] = 0M;
                        drNew["ItmTaxable"] = objItm.Taxable;
                        drNew["ItmTaxGrpKey"] = DBNull.Value;
                        drNew["ItmTaxGrpRate"] = 0M;
                        drNew["ItmTaxGrpAmtF"] = 0M;
                        drNew["ItmTaxGrpAmtL"] = 0M;
                        drNew["ItmColorKey"] = DBNull.Value;
                        drNew["ItmScaleSize"] = DBNull.Value;
                        drNew["ItmPacking"] = DBNull.Value;
                        drNew["ItmMark"] = DBNull.Value;
                        drNew["ItmVendorKey"] = DBNull.Value;
                        drNew["ItmVendorCurrKey"] = 1;
                        drNew["ItmVendorCurrRate"] = 1M;
                        drNew["ItmVendorPrice"] = 0;
                        drNew["ItmVendorPriceRatio"] = 0;
                        drNew["ItmJobKey"] = 0;
                        drNew["ItmJobPhaseKey"] = 0;
                        drNew["ItmJobTaskKey"] = 0;
                        drNew["ItmJobCostTypeKey"] = 0;
                        drNew["ItmIGrpDItm"] = 0;
                        drNew["ItmIGrpQtyLock"] = false;
                        drNew["ItmIGrpToPrint"] = true;
                        drNew["ItmIGrpQtySet"] = 0;
                        drNew["ItmIGrpAmtSet"] = 0;
                        drNew["ARQOID"] = DBNull.Value;
                        drNew["ARQODK"] = 0;
                        drNew["ARQODItm"] = 0;
                        drNew["ARSOID"] = DBNull.Value;
                        drNew["ARSODK"] = 0;
                        drNew["ARSODItm"] = 0;
                        drNew["ARSOPOID"] = DBNull.Value;
                        break;
                    #endregion

                    #region ARIV, DN, CN
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                        drNew["DocItmKey"] = DocItmKey;
                        drNew["ItmSN"] = DocItmSN;
                        drNew["ItmKey"] = ItmKey;
                        drNew["ItmKeySelect"] = ItmKeySelected;
                        drNew["ItmID"] = ItmID;
                        drNew["ItmType"] = ItmType;
                        drNew["ItmDes"] = ItmDes;
                        drNew["ItmDeptKey"] = 0;
                        drNew["ItmTranGrpKey"] = 0;
                        drNew["ItmAccKey"] = DBNull.Value;
                        drNew["ItmLocKey"] = DBNull.Value;
                        drNew["ItmStock"] = DBNull.Value;
                        drNew["ItmQty"] = DBNull.Value;
                        drNew["ItmUOMKey"] = DBNull.Value;
                        drNew["ItmConRate"] = DBNull.Value;
                        drNew["ItmLatestCostF"] = DBNull.Value;
                        drNew["ItmLatestCostH"] = DBNull.Value;
                        drNew["ItmListPrice"] = DBNull.Value;
                        drNew["ItmPriceBefore"] = DBNull.Value;
                        drNew["ItmPriceAfter"] = DBNull.Value;
                        drNew["ItmDisPercent"] = DBNull.Value;
                        drNew["ItmDisValue"] = DBNull.Value;
                        drNew["ItmPrice"] = 0M;
                        drNew["ItmPriceUser"] = DBNull.Value;
                        drNew["ItmControlPrice"] = DBNull.Value;
                        drNew["ItmControlPriceBase"] = DBNull.Value;
                        drNew["ItmAmtShw"] = DBNull.Value;
                        drNew["ItmAmtF"] = 0M;
                        drNew["ItmAmtH"] = 0M;
                        drNew["ItmTaxable"] = objItm.Taxable;
                        drNew["ItmTaxGrpKey"] = DBNull.Value;
                        drNew["ItmTaxGrpRate"] = 0M;
                        drNew["ItmTaxGrpAmtF"] = 0M;
                        drNew["ItmTaxGrpAmtL"] = 0M;
                        drNew["ItmColorKey"] = DBNull.Value;
                        drNew["ItmScaleSize"] = DBNull.Value;
                        drNew["ItmPacking"] = DBNull.Value;
                        drNew["ItmMark"] = DBNull.Value;
                        drNew["ItmVendorKey"] = DBNull.Value;
                        drNew["ItmVendorCurrKey"] = 1;
                        drNew["ItmVendorCurrRate"] = 1M;
                        drNew["ItmVendorPrice"] = 0M;
                        drNew["ItmVendorPriceRatio"] = 0M;
                        drNew["ItmJobKey"] = 0;
                        drNew["ItmJobPhaseKey"] = 0;
                        drNew["ItmJobTaskKey"] = 0;
                        drNew["ItmJobCostTypeKey"] = 0;
                        drNew["ItmIGrpDItm"] = 0;
                        drNew["ItmIGrpQtyLock"] = false;
                        drNew["ItmIGrpToPrint"] = true;
                        drNew["ItmIGrpQtySet"] = 0;
                        drNew["ItmIGrpAmtSet"] = 0;
                        drNew["ARQOID"] = DBNull.Value;
                        drNew["ARQODK"] = 0;
                        drNew["ARQODItm"] = 0;
                        drNew["ARSOID"] = DBNull.Value;
                        drNew["ARSODK"] = 0;
                        drNew["ARSODItm"] = 0;
                        drNew["ARSOPOID"] = DBNull.Value;
                        drNew["ARDOID"] = DBNull.Value;
                        drNew["ARDODK"] = 0;
                        drNew["ARDODItm"] = 0;
                        drNew["CSCPSID"] = DBNull.Value;
                        drNew["CSCPSDK"] = 0;
                        drNew["CSCPSDItm"] = 0;
                        drNew["CSCSIID"] = DBNull.Value;
                        drNew["CSCSIDK"] = 0;
                        drNew["CSCSIDItm"] = 0;
                        break;
                    #endregion

                    #region ARPL
                    case (int)GEnum.SystemCode.Packing_List:
                        drNew["DocDetItmKey"] = DocItmKey;
                        drNew["DetItmSN"] = DocItmSN;
                        drNew["DetItmKey"] = ItmKey;
                        drNew["DetItmKeySelect"] = ItmKeySelected;
                        drNew["DetItmID"] = ItmID;
                        drNew["DetItmType"] = ItmType;
                        drNew["DetItmDes"] = ItmDes;
                        drNew["DetItmDeptKey"] = 0;
                        drNew["DetItmBatchID"] = DBNull.Value;
                        drNew["DetItmPacking"] = DBNull.Value;
                        drNew["DetItmQtyPerPack"] = 0M;
                        drNew["DetItmQtyTotal"] = 0M;
                        drNew["DetItmUOMKey"] = DBNull.Value;
                        drNew["DetItmConRate"] = 1M;
                        drNew["DetItmWeightNet"] = 0M;
                        drNew["DetItmWeightGross"] = 0M;
                        drNew["DetItmWeightUOMKey"] = DBNull.Value;
                        drNew["DetItmWeightUOMRate"] = 1M;
                        drNew["DetItmWeightBaseNet"] = 0M;
                        drNew["DetItmWeightBaseGross"] = 0M;
                        drNew["DetItmHide"] = false;
                        drNew["DetItmDocID"] = DBNull.Value;
                        drNew["DetItmMarking"] = DBNull.Value;
                        drNew["DetItmColorKey"] = DBNull.Value;
                        drNew["DetItmScaleSize"] = DBNull.Value;
                        drNew["DetItmRem"] = DBNull.Value;
                        break;
                    #endregion

                    #region APPN
                    case (int)GEnum.SystemCode.Purchase_Plan:
                        drNew["DocItmKey"] = DocItmKey;
                        drNew["ItmSN"] = DocItmSN;
                        drNew["ItmKey"] = ItmKey;
                        drNew["ItmKeySelect"] = ItmKeySelected;
                        drNew["ItmID"] = ItmID;
                        drNew["ItmDes"] = ItmDes;
                        drNew["ItmDeptKey"] = 0;
                        drNew["ItmTranGrpKey"] = 0;
                        drNew["ItmQtyM1"] = 0M;
                        drNew["ItmQtyM2"] = 0M;
                        drNew["ItmQtyM3"] = 0M;
                        drNew["ItmQtyM4"] = 0M;
                        drNew["ItmQtyM5"] = 0M;
                        drNew["ItmQtyM6"] = 0M;
                        drNew["ItmQtyM7"] = 0M;
                        drNew["ItmQtyM8"] = 0M;
                        drNew["ItmQtyM9"] = 0M;
                        drNew["ItmQtyM10"] = 0M;
                        drNew["ItmQtyM11"] = 0M;
                        drNew["ItmQtyM12"] = 0M;
                        drNew["ItmQtyMTotal"] = 0M;
                        break;
                    #endregion

                    #region APRQ
                    case (int)GEnum.SystemCode.Purchase_Request:
                        drNew["DocItmKey"] = DocItmKey;
                        drNew["ItmSN"] = DocItmSN;
                        drNew["ItmKey"] = ItmKey;
                        drNew["ItmKeySelect"] = ItmKeySelected;
                        drNew["ItmID"] = ItmID;
                        drNew["ItmType"] = ItmType;
                        drNew["ItmDes"] = ItmDes;
                        drNew["ItmDeptKey"] = 0;
                        drNew["ItmTranGrpKey"] = 0;
                        drNew["ItmQty"] = DBNull.Value;
                        drNew["ItmUOMKey"] = DBNull.Value;
                        drNew["ItmConRate"] = DBNull.Value;
                        drNew["ItmColorKey"] = DBNull.Value;
                        drNew["ItmScaleSize"] = DBNull.Value;
                        drNew["ItmPacking"] = DBNull.Value;
                        drNew["ItmMark"] = DBNull.Value;
                        break;
                    #endregion

                    #region APPO
                    case (int)GEnum.SystemCode.Purchase_Order:
                        drNew["DocItmKey"] = DocItmKey;
                        drNew["ItmSN"] = DocItmSN;
                        drNew["ItmKey"] = ItmKey;
                        drNew["ItmKeySelect"] = ItmKeySelected;
                        drNew["ItmID"] = ItmID;
                        drNew["ItmType"] = ItmType;
                        drNew["ItmDes"] = ItmDes;
                        drNew["ItmDeptKey"] = 0;
                        drNew["ItmTranGrpKey"] = 0;
                        drNew["ItmAccKey"] = DBNull.Value;
                        drNew["ItmLocKey"] = DBNull.Value;
                        drNew["ItmStock"] = DBNull.Value;
                        drNew["ItmQty"] = DBNull.Value;
                        drNew["ItmQtyLink"] = DBNull.Value;
                        drNew["ItmQtyAdj"] = DBNull.Value;
                        drNew["ItmOrderStatus"] = DBNull.Value;
                        drNew["ItmReqDate"] = DBNull.Value;
                        drNew["ItmPrmDate"] = DBNull.Value;
                        drNew["ItmUOMKey"] = DBNull.Value;
                        drNew["ItmConRate"] = DBNull.Value;
                        drNew["ItmLatestCostF"] = DBNull.Value;
                        drNew["ItmLatestCostH"] = DBNull.Value;
                        drNew["ItmListPrice"] = DBNull.Value;
                        drNew["ItmPriceBefore"] = DBNull.Value;
                        drNew["ItmPriceAfter"] = DBNull.Value;
                        drNew["ItmDisPercent"] = DBNull.Value;
                        drNew["ItmDisValue"] = DBNull.Value;
                        drNew["ItmPrice"] = 0M;
                        drNew["ItmPriceUser"] = DBNull.Value;
                        drNew["ItmAmtShw"] = DBNull.Value;
                        drNew["ItmAmtF"] = 0M;
                        drNew["ItmAmtH"] = 0M;
                        drNew["ItmTaxable"] = objItm.Taxable;
                        drNew["ItmTaxGrpKey"] = DBNull.Value;
                        drNew["ItmTaxGrpRate"] = 0M;
                        drNew["ItmTaxGrpAmtF"] = 0M;
                        drNew["ItmTaxGrpAmtL"] = 0M;
                        drNew["ItmColorKey"] = DBNull.Value;
                        drNew["ItmScaleSize"] = DBNull.Value;
                        drNew["ItmPacking"] = DBNull.Value;
                        drNew["ItmMark"] = DBNull.Value;
                        drNew["ItmJobKey"] = 0;
                        drNew["ItmJobPhaseKey"] = 0;
                        drNew["ItmJobTaskKey"] = 0;
                        drNew["ItmJobCostTypeKey"] = 0;
                        drNew["ARQOID"] = DBNull.Value;
                        drNew["ARQODK"] = 0;
                        drNew["ARQODItm"] = 0;
                        drNew["ARSOID"] = DBNull.Value;
                        drNew["ARSODK"] = 0;
                        drNew["ARSODItm"] = 0;
                        drNew["ARSOPOID"] = DBNull.Value;
                        drNew["ARDOID"] = DBNull.Value;
                        drNew["ARDODK"] = 0;
                        drNew["ARDODItm"] = 0;
                        drNew["ARIVID"] = DBNull.Value;
                        drNew["ARIVDK"] = 0;
                        drNew["ARIVDItm"] = 0;
                        break;
                    #endregion

                    #region APPD
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                        drNew["DocItmKey"] = DocItmKey;
                        drNew["ItmSN"] = DocItmSN;
                        drNew["ItmKey"] = ItmKey;
                        drNew["ItmKeySelect"] = ItmKeySelected;
                        drNew["ItmID"] = ItmID;
                        drNew["ItmType"] = ItmType;
                        drNew["ItmDes"] = ItmDes;
                        drNew["ItmDeptKey"] = 0;
                        drNew["ItmTranGrpKey"] = 0;
                        drNew["ItmAccKey"] = DBNull.Value;
                        drNew["ItmLocKey"] = DBNull.Value;
                        drNew["ItmStock"] = DBNull.Value;
                        drNew["ItmQty"] = DBNull.Value;
                        drNew["ItmUOMKey"] = DBNull.Value;
                        drNew["ItmConRate"] = DBNull.Value;
                        drNew["ItmLatestCostF"] = DBNull.Value;
                        drNew["ItmLatestCostH"] = DBNull.Value;
                        drNew["ItmListPrice"] = DBNull.Value;
                        drNew["ItmPriceBefore"] = DBNull.Value;
                        drNew["ItmPriceAfter"] = DBNull.Value;
                        drNew["ItmDisPercent"] = DBNull.Value;
                        drNew["ItmDisValue"] = DBNull.Value;
                        drNew["ItmPrice"] = 0M;
                        drNew["ItmPriceUser"] = DBNull.Value;
                        drNew["ItmAmtShw"] = DBNull.Value;
                        drNew["ItmAmtF"] = 0M;
                        drNew["ItmAmtH"] = 0M;
                        drNew["ItmTaxable"] = objItm.Taxable;
                        drNew["ItmTaxGrpAmtF"] = 0;
                        drNew["ItmTaxGrpAmtL"] = 0;
                        drNew["ItmColorKey"] = DBNull.Value;
                        drNew["ItmScaleSize"] = DBNull.Value;
                        drNew["ItmPacking"] = DBNull.Value;
                        drNew["ItmMark"] = DBNull.Value;
                        drNew["ItmJobKey"] = 0;
                        drNew["ItmJobPhaseKey"] = 0;
                        drNew["ItmJobTaskKey"] = 0;
                        drNew["ItmJobCostTypeKey"] = 0;
                        drNew["ARQOID"] = DBNull.Value;
                        drNew["ARQODK"] = 0;
                        drNew["ARQODItm"] = 0;
                        drNew["ARSOID"] = DBNull.Value;
                        drNew["ARSODK"] = 0;
                        drNew["ARSODItm"] = 0;
                        drNew["ARSOPOID"] = DBNull.Value;
                        drNew["ARDOID"] = DBNull.Value;
                        drNew["ARDODK"] = 0;
                        drNew["ARDODItm"] = 0;
                        drNew["ARIVID"] = DBNull.Value;
                        drNew["ARIVDK"] = 0;
                        drNew["ARIVDItm"] = 0;
                        drNew["APPOID"] = DBNull.Value;
                        drNew["APPODK"] = 0;
                        drNew["APPODItm"] = 0;
                        break;
                    #endregion

                    #region APBL, DN, CN
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        drNew["DocItmKey"] = DocItmKey;
                        drNew["ItmSN"] = DocItmSN;
                        drNew["ItmKey"] = ItmKey;
                        drNew["ItmKeySelect"] = ItmKeySelected;
                        drNew["ItmID"] = ItmID;
                        drNew["ItmType"] = ItmType;
                        drNew["ItmDes"] = ItmDes;
                        drNew["ItmDeptKey"] = 0;
                        drNew["ItmTranGrpKey"] = 0;
                        drNew["ItmAccKey"] = DBNull.Value;
                        drNew["ItmLocKey"] = DBNull.Value;
                        drNew["ItmStock"] = DBNull.Value;
                        drNew["ItmQty"] = DBNull.Value;
                        drNew["ItmUOMKey"] = DBNull.Value;
                        drNew["ItmConRate"] = DBNull.Value;
                        drNew["ItmLatestCostF"] = DBNull.Value;
                        drNew["ItmLatestCostH"] = DBNull.Value;
                        drNew["ItmListPrice"] = DBNull.Value;
                        drNew["ItmPriceBefore"] = DBNull.Value;
                        drNew["ItmPriceAfter"] = DBNull.Value;
                        drNew["ItmDisPercent"] = DBNull.Value;
                        drNew["ItmDisValue"] = DBNull.Value;
                        drNew["ItmPrice"] = 0M;
                        drNew["ItmPriceUser"] = DBNull.Value;
                        drNew["ItmAmtShw"] = DBNull.Value;
                        drNew["ItmAmtF"] = 0M;
                        drNew["ItmAmtH"] = 0M;
                        drNew["ItmTaxable"] = objItm.Taxable;
                        drNew["ItmTaxGrpKey"] = DBNull.Value;
                        drNew["ItmTaxGrpRate"] = 0M;
                        drNew["ItmTaxGrpAmtF"] = 0M;
                        drNew["ItmTaxGrpAmtL"] = 0M;
                        drNew["ItmAddCostF"] = 0M;
                        drNew["ItmAddCostH"] = 0M;
                        drNew["ItmAddAmtF"] = 0M;
                        drNew["ItmAddAmtH"] = 0M;
                        drNew["ItmColorKey"] = DBNull.Value;
                        drNew["ItmScaleSize"] = DBNull.Value;
                        drNew["ItmPacking"] = DBNull.Value;
                        drNew["ItmMark"] = DBNull.Value;
                        drNew["ItmJobKey"] = 0;
                        drNew["ItmJobPhaseKey"] = 0;
                        drNew["ItmJobTaskKey"] = 0;
                        drNew["ItmJobCostTypeKey"] = 0;
                        drNew["ARQOID"] = DBNull.Value;
                        drNew["ARQODK"] = 0;
                        drNew["ARQODItm"] = 0;
                        drNew["ARSOID"] = DBNull.Value;
                        drNew["ARSODK"] = 0;
                        drNew["ARSODItm"] = 0;
                        drNew["ARSOPOID"] = DBNull.Value;
                        drNew["ARDOID"] = DBNull.Value;
                        drNew["ARDODK"] = 0;
                        drNew["ARDODItm"] = 0;
                        drNew["ARIVID"] = DBNull.Value;
                        drNew["ARIVDK"] = 0;
                        drNew["ARIVDItm"] = 0;
                        drNew["APPOID"] = DBNull.Value;
                        drNew["APPODK"] = 0;
                        drNew["APPODItm"] = 0;
                        drNew["APPDID"] = DBNull.Value;
                        drNew["APPDDK"] = 0;
                        drNew["APPDDItm"] = 0;
                        break;
                    #endregion

                    #region INADJ
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                        drNew["DocItmKey"] = DocItmKey;
                        drNew["ItmSN"] = DocItmSN;
                        drNew["ItmKey"] = ItmKey;
                        drNew["ItmKeySelect"] = ItmKeySelected;
                        drNew["ItmID"] = ItmID;
                        drNew["ItmType"] = ItmType;
                        drNew["ItmDes"] = ItmDes;
                        drNew["ItmDeptKey"] = 0;
                        drNew["ItmTranGrpKey"] = 0;
                        drNew["ItmAccKey"] = DBNull.Value;
                        drNew["ItmLocKey"] = DBNull.Value;
                        drNew["ItmStock"] = DBNull.Value;
                        drNew["ItmQty"] = DBNull.Value;
                        drNew["ItmUOMKey"] = DBNull.Value;
                        drNew["ItmConRate"] = DBNull.Value;
                        drNew["ItmCost"] = DBNull.Value;
                        drNew["ItmNewCost"] = 0;
                        drNew["ItmColorKey"] = DBNull.Value;
                        drNew["ItmScaleSize"] = DBNull.Value;
                        drNew["ItmPacking"] = DBNull.Value;
                        drNew["ItmJobKey"] = 0;
                        drNew["ItmJobPhaseKey"] = 0;
                        drNew["ItmJobTaskKey"] = 0;
                        drNew["ItmJobCostTypeKey"] = 0;
                        drNew["CSCPSID"] = DBNull.Value;
                        drNew["CSCPSDK"] = 0;
                        drNew["CSCPSDItm"] = 0;
                        break;
                    #endregion

                    #region INMFN
                    case (int)GEnum.SystemCode.Inventory_Production:
                        drNew["DocItmKey"] = DocItmKey;
                        drNew["ItmSN"] = DocItmSN;
                        drNew["ItmKey"] = ItmKey;
                        drNew["ItmKeySelect"] = ItmKeySelected;
                        drNew["ItmID"] = ItmID;
                        drNew["ItmFGKey"] = 0;
                        drNew["ItmFGKeySelect"] = 0;
                        drNew["ItmType"] = ItmType;
                        drNew["ItmDes"] = ItmDes;
                        drNew["ItmDeptKey"] = 0;
                        drNew["ItmTranGrpKey"] = 0;
                        drNew["ItmAccINKey"] = DBNull.Value;
                        drNew["ItmLocKey"] = DBNull.Value;
                        drNew["ItmPacking"] = DBNull.Value;

                        switch (GFunc.NEInt(drNew["LineType"], 0))
                        {
                            case 3010:	//Document Detail Finished Goods - Batch
                            case 3020:	//Document Detail Finished Goods - Batch - Serial
                            case 3030:	//Document Detail Finished Goods - Serial
                                drNew["FGBUOMKey"] = DBNull.Value;
                                drNew["FGWeight"] = 0M;
                                drNew["FGWeightUOMKey"] = DBNull.Value;
                                drNew["FGReq"] = 0;
                                drNew["FGProduceQty"] = 0M;
                                drNew["FGProduceWeight"] = 0M;
                                drNew["FGProduceGram"] = 0M;
                                drNew["FGOverHeadKey"] = DBNull.Value;
                                drNew["FGOverHeadCost"] = 0M;
                                drNew["FGOverHeadAmtH"] = 0M;
                                drNew["FGCostRatio"] = 0M;
                                drNew["BOMMultiplier"] = 1M;
                                drNew["BOMBUOMKey"] = DBNull.Value;
                                drNew["BOMWeight"] = 0M;
                                drNew["BOMWeightUOMKey"] = DBNull.Value;
                                drNew["BOMReq"] = 0M;
                                drNew["BOMIssue"] = 0M;
                                drNew["BOMReturn"] = 0M;
                                drNew["BOMUsed"] = 0M;
                                drNew["BOMUsedWeight"] = 0M;
                                drNew["BOMUsedGram"] = 0M;
                                drNew["BOMLabourCost"] = 0M;
                                drNew["BOMLabourAmt"] = 0M;
                                break;

                            case 3100:	//Document Detail Raw Material
                            case 3110:	//Document Detail Raw Material - Batch
                            case 3120:	//Document Detail Raw Material - Batch Serial
                            case 3130:	//Document Detail Raw Material - Serial
                            case 3200:	//Document Detail Packing Material
                            case 3210:	//Document Detail Packing Material - Batch
                            case 3220:	//Document Detail Packing Material - Batch - Serial
                            case 3230:	//Document Detail Packing Material - Serial
                            case 3300:	//Document Detail Other Manuafacturing Cost
                                drNew["FGBUOMKey"] = DBNull.Value;
                                drNew["FGWeight"] = 0M;
                                drNew["FGWeightUOMKey"] = DBNull.Value;
                                drNew["FGReq"] = 0M;
                                drNew["FGProduceQty"] = 0M;
                                drNew["FGProduceWeight"] = 0M;
                                drNew["FGProduceGram"] = 0M;
                                drNew["FGOverHeadKey"] = DBNull.Value;
                                drNew["FGOverHeadCost"] = 0M;
                                drNew["FGOverHeadAmtH"] = 0M;
                                drNew["FGCostRatio"] = 0M;
                                drNew["BOMMultiplier"] = 1M;
                                drNew["BOMBUOMKey"] = DBNull.Value;
                                drNew["BOMWeight"] = 0M;
                                drNew["BOMWeightUOMKey"] = DBNull.Value;
                                drNew["BOMReq"] = 0M;
                                drNew["BOMIssue"] = 0M;
                                drNew["BOMReturn"] = 0M;
                                drNew["BOMUsed"] = 0M;
                                drNew["BOMUsedWeight"] = 0M;
                                drNew["BOMUsedGram"] = 0M;
                                drNew["BOMLabourCost"] = 0M;
                                drNew["BOMLabourAmt"] = 0M;
                                break;
                        }
                        break;
                    #endregion

                    #region INTRN
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                        drNew["DocItmKey"] = DocItmKey;
                        drNew["ItmSN"] = DocItmSN;
                        drNew["ItmKey"] = ItmKey;
                        drNew["ItmKeySelect"] = ItmKeySelected;
                        drNew["ItmID"] = ItmID;
                        drNew["ItmType"] = ItmType;
                        drNew["ItmDes"] = ItmDes;
                        drNew["ItmDeptKey"] = 0;
                        drNew["ItmTranGrpKey"] = 0;
                        drNew["ItmFromLocKey"] = 0;
                        drNew["ItmToLocKey"] = 0;
                        drNew["ItmFromAccKey"] = DBNull.Value;
                        drNew["ItmToAccKey"] = DBNull.Value;
                        drNew["ItmStock"] = DBNull.Value;
                        drNew["ItmQty"] = DBNull.Value;
                        drNew["ItmUOMKey"] = DBNull.Value;
                        drNew["ItmConRate"] = DBNull.Value;
                        drNew["ItmColorKey"] = DBNull.Value;
                        drNew["ItmScaleSize"] = DBNull.Value;
                        drNew["ItmPacking"] = DBNull.Value;
                        break;
                    #endregion

                    #region CSCSI, CSR
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        drNew["DocItmKey"] = DocItmKey;
                        drNew["ItmSN"] = DocItmSN;
                        drNew["ItmKey"] = ItmKey;
                        drNew["ItmKeySelect"] = ItmKeySelected;
                        drNew["ItmID"] = ItmID;
                        drNew["ItmType"] = ItmType;
                        drNew["ItmDes"] = ItmDes;
                        drNew["ItmDeptKey"] = 0;
                        drNew["ItmTranGrpKey"] = 0;
                        drNew["ItmFromLocKey"] = 0;
                        drNew["ItmToLocKey"] = 0;
                        drNew["ItmFromAccKey"] = DBNull.Value;
                        drNew["ItmToAccKey"] = DBNull.Value;
                        drNew["ItmStock"] = DBNull.Value;
                        drNew["ItmQty"] = DBNull.Value;
                        drNew["ItmQtyLink"] = DBNull.Value;
                        drNew["ItmUOMKey"] = DBNull.Value;
                        drNew["ItmConRate"] = DBNull.Value;
                        drNew["ItmLatestCostF"] = DBNull.Value;
                        drNew["ItmLatestCostH"] = DBNull.Value;
                        drNew["ItmListPrice"] = DBNull.Value;
                        drNew["ItmPriceBefore"] = DBNull.Value;
                        drNew["ItmPriceAfter"] = DBNull.Value;
                        drNew["ItmDisPercent"] = DBNull.Value;
                        drNew["ItmDisValue"] = DBNull.Value;
                        drNew["ItmPrice"] = 0M;
                        drNew["ItmPriceUser"] = DBNull.Value;
                        drNew["ItmControlPrice"] = DBNull.Value;
                        drNew["ItmControlPriceBase"] = DBNull.Value;
                        drNew["ItmAmtShw"] = DBNull.Value;
                        drNew["ItmAmtF"] = 0M;
                        drNew["ItmAmtH"] = 0M;
                        drNew["ItmColorKey"] = DBNull.Value;
                        drNew["ItmScaleSize"] = DBNull.Value;
                        drNew["ItmPacking"] = DBNull.Value;
                        drNew["ItmMark"] = DBNull.Value;
                        drNew["ItmJobKey"] = 0;
                        drNew["ItmJobPhaseKey"] = 0;
                        drNew["ItmJobTaskKey"] = 0;
                        drNew["ItmJobCostTypeKey"] = 0;
                        drNew["ARSOID"] = DBNull.Value;
                        drNew["ARSODK"] = 0;
                        drNew["ARSODItm"] = 0;
                        drNew["ARSOPOID"] = DBNull.Value;
                        drNew["CSCSIID"] = DBNull.Value;
                        drNew["CSCSIDK"] = 0;
                        drNew["CSCSIDItm"] = 0;
                        drNew["ItmFromAccDes"] = DBNull.Value;
                        drNew["ItmToAccDes"] = DBNull.Value;
                        break;
                    #endregion

                    #region CSCPO
                    case (int)GEnum.SystemCode.Order_Consignment:
                        drNew["DocItmKey"] = DocItmKey;
                        drNew["ItmSN"] = DocItmSN;
                        drNew["ItmKey"] = ItmKey;
                        drNew["ItmKeySelect"] = ItmKeySelected;
                        drNew["ItmID"] = ItmID;
                        drNew["ItmType"] = ItmType;
                        drNew["ItmDes"] = ItmDes;
                        drNew["ItmLocKey"] = DBNull.Value;
                        drNew["ItmStock"] = DBNull.Value;
                        drNew["ItmQty"] = DBNull.Value;
                        drNew["ItmQtyLink"] = DBNull.Value;
                        drNew["ItmQtyAdj"] = DBNull.Value;
                        drNew["ItmQtyBalance"] = DBNull.Value;
                        drNew["ItmReqDate"] = DBNull.Value;
                        drNew["ItmPrmDate"] = DBNull.Value;
                        drNew["ItmUOMKey"] = DBNull.Value;
                        drNew["ItmConRate"] = DBNull.Value;
                        drNew["ItmLatestCostF"] = DBNull.Value;
                        drNew["ItmLatestCostH"] = DBNull.Value;
                        drNew["ItmListPrice"] = DBNull.Value;
                        drNew["ItmPriceBefore"] = DBNull.Value;
                        drNew["ItmPriceAfter"] = DBNull.Value;
                        drNew["ItmDisPercent"] = DBNull.Value;
                        drNew["ItmDisValue"] = DBNull.Value;
                        drNew["ItmPrice"] = 0M;
                        drNew["ItmPriceUser"] = DBNull.Value;
                        drNew["ItmAmtShw"] = DBNull.Value;
                        drNew["ItmAmtF"] = 0M;
                        drNew["ItmAmtH"] = 0M;
                        drNew["ItmColorKey"] = DBNull.Value;
                        drNew["ItmScaleSize"] = DBNull.Value;
                        drNew["ItmPacking"] = DBNull.Value;
                        drNew["ItmMark"] = DBNull.Value;
                        drNew["ItmJobKey"] = 0;
                        drNew["ItmJobPhaseKey"] = 0;
                        drNew["ItmJobTaskKey"] = 0;
                        drNew["ItmJobCostTypeKey"] = 0;
                        break;
                    #endregion

                    #region CSCPD
                    case (int)GEnum.SystemCode.Received_Consignment:
                        drNew["DocItmKey"] = DocItmKey;
                        drNew["ItmSN"] = DocItmSN;
                        drNew["ItmKey"] = ItmKey;
                        drNew["ItmKeySelect"] = ItmKeySelected;
                        drNew["ItmID"] = ItmID;
                        drNew["ItmType"] = ItmType;
                        drNew["ItmDes"] = ItmDes;
                        drNew["ItmLocKey"] = DBNull.Value;
                        drNew["ItmStock"] = DBNull.Value;
                        drNew["ItmQty"] = DBNull.Value;
                        drNew["ItmUOMKey"] = DBNull.Value;
                        drNew["ItmConRate"] = DBNull.Value;
                        drNew["ItmLatestCostF"] = DBNull.Value;
                        drNew["ItmLatestCostH"] = DBNull.Value;
                        drNew["ItmListPrice"] = DBNull.Value;
                        drNew["ItmPriceBefore"] = DBNull.Value;
                        drNew["ItmPriceAfter"] = DBNull.Value;
                        drNew["ItmDisPercent"] = DBNull.Value;
                        drNew["ItmDisValue"] = DBNull.Value;
                        drNew["ItmPrice"] = 0M;
                        drNew["ItmPriceUser"] = DBNull.Value;
                        drNew["ItmAmtShw"] = DBNull.Value;
                        drNew["ItmAmtF"] = 0M;
                        drNew["ItmAmtH"] = 0M;
                        drNew["ItmColorKey"] = DBNull.Value;
                        drNew["ItmScaleSize"] = DBNull.Value;
                        drNew["ItmPacking"] = DBNull.Value;
                        drNew["ItmMark"] = DBNull.Value;
                        drNew["ItmJobKey"] = 0;
                        drNew["ItmJobPhaseKey"] = 0;
                        drNew["ItmJobTaskKey"] = 0;
                        drNew["ItmJobCostTypeKey"] = 0;
                        drNew["CSCPOID"] = DBNull.Value;
                        drNew["CSCPODK"] = 0;
                        drNew["CSCPODItm"] = 0;
                        drNew["CSCPSID"] = DBNull.Value;
                        drNew["CSCPSDK"] = 0;
                        drNew["CSCPSDItm"] = 0;
                        break;
                    #endregion
                }

                dtItm.Rows.Add(drNew);
                dtItm.AcceptChanges();
                grd.Rows.Refresh(RefreshRow.ReloadData);
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
        public static bool ItmRow_AddBlankRowExpGrid(Document objDoc, UltraGrid grd, int DocItmKey, decimal ExpSN)
        {
            try
            {
                #region Declaration of variables
                DataTable dtItm = null;               

                dtItm = (DataTable)grd.DataSource;
                DataRow drNew = dtItm.NewRow();

                #endregion

                #region set values in document detail

                drNew["DocKey"] = objDoc.DocKey;
                ExpSN = ExpSN - 0.5M;

                switch (objDoc.DocCodeKey)
                {                   
                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                        drNew["DocItmKey"] = DocItmKey;
                        drNew["ExpSN"] = ExpSN;
                        //drNew["ExpTaxable"] = objItm.Taxable;
                        drNew["ExpTaxGrpKey"] = GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc).ToDBValue();
                        drNew["ExpTaxGrpRate"] = GFunc.GetDecimalPropertyValue("DocTaxGrpRate", objDoc);
                        drNew["ExpTaxGrpAmtF"] =0M;
                        drNew["ExpTaxGrpAmtL"] = 0M;   
                        break;                                   
                }

                dtItm.Rows.Add(drNew);
                dtItm.AcceptChanges();
                grd.Rows.Refresh(RefreshRow.ReloadData);
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

        public static bool ItmRow_AddBlankRowJournal(Document objDoc, UltraGrid grd, int DocItmKey, decimal ItmSN)
        {
            try
            {
                #region Declaration of variables
                DataTable dtItm = null;

                dtItm = (DataTable)grd.DataSource;
                DataRow drNew = dtItm.NewRow();

                #endregion

                #region set values in document detail

                drNew["DocKey"] = objDoc.DocKey;
                ItmSN = ItmSN - 0.5M;

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Journal:                  
                        drNew["DocItmKey"] = DocItmKey;
                        drNew["ItmSN"] = ItmSN;                      
                        break;
                }

                dtItm.Rows.Add(drNew);
                dtItm.AcceptChanges();
                grd.Rows.Refresh(RefreshRow.ReloadData);
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
        public static bool ItmRow_CellDblClick(Document objDoc, UltraGrid grd, string colKey)
        {
            try
            {
                switch (colKey.ToLower())
                {
                    case "itmdes":
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
                            case (int)GEnum.SystemCode.Purchase_Shipment:
                            case (int)GEnum.SystemCode.Purchase_Delivery:
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            case (int)GEnum.SystemCode.Inventory_Adjustment:
                            case (int)GEnum.SystemCode.Inventory_Production:
                            case (int)GEnum.SystemCode.Inventory_Transfer:
                                DocDetUtil.ItmDes_DblClick(objDoc, grd);
                                break;
                        }
                        break;

                    case "itmqty":
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
                            case (int)GEnum.SystemCode.Inventory_Adjustment:
                            case (int)GEnum.SystemCode.Inventory_Transfer:
                            case (int)GEnum.SystemCode.Issue_Consignment:
                            case (int)GEnum.SystemCode.Return_Consignment:
                                DocDetUtil.ItmQty_DblClick(objDoc, grd);
                                break;
                        }
                        break;

                    case "itmqtydelivered":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Sales_Order:
                            case (int)GEnum.SystemCode.Reserve_Order:
                            case (int)GEnum.SystemCode.Purchase_Order:
                            case (int)GEnum.SystemCode.Issue_Consignment:
                            case (int)GEnum.SystemCode.Order_Consignment:
                                DocDetUtil.ItmQtyDelivere_DblClick(objDoc, grd);
                                break;
                        }
                        break;

                    case "itmvendorprice":
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
                                DocDetUtil.ItmVendorPrice_DblClick(objDoc, grd);
                                DocDetUtil.ItmLatestCostF_DblClick(objDoc, grd);
                                DocDetUtil.ItmLatestCostH_DblClick(objDoc, grd);
                                DocDetUtil.ItmVendorPriceRatio_DblClick(objDoc, grd);
                                grd.Refresh();
                                break;
                        }
                        break;

                    case "itmlatestcostf":
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
                                DocDetUtil.ItmVendorPrice_DblClick(objDoc, grd);
                                DocDetUtil.ItmLatestCostF_DblClick(objDoc, grd);
                                DocDetUtil.ItmLatestCostH_DblClick(objDoc, grd);
                                DocDetUtil.ItmVendorPriceRatio_DblClick(objDoc, grd);
                                grd.Refresh();
                                break;
                            case (int)GEnum.SystemCode.Purchase_Order:
                            case (int)GEnum.SystemCode.Purchase_Shipment:
                            case (int)GEnum.SystemCode.Purchase_Delivery:
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            case (int)GEnum.SystemCode.Issue_Consignment:
                            case (int)GEnum.SystemCode.Return_Consignment:
                            case (int)GEnum.SystemCode.Order_Consignment:
                            case (int)GEnum.SystemCode.Received_Consignment:
                                DocDetUtil.ItmLatestCostF_DblClick(objDoc, grd);
                                DocDetUtil.ItmLatestCostH_DblClick(objDoc, grd);
                                grd.Refresh();
                                break;
                        }
                        break;
                    case "itmlatestcosth":
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
                                DocDetUtil.ItmVendorPrice_DblClick(objDoc, grd);
                                DocDetUtil.ItmLatestCostF_DblClick(objDoc, grd);
                                DocDetUtil.ItmLatestCostH_DblClick(objDoc, grd);
                                DocDetUtil.ItmVendorPriceRatio_DblClick(objDoc, grd);
                                grd.Refresh();
                                break;
                            case (int)GEnum.SystemCode.Purchase_Order:
                            case (int)GEnum.SystemCode.Purchase_Shipment:
                            case (int)GEnum.SystemCode.Purchase_Delivery:
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            case (int)GEnum.SystemCode.Issue_Consignment:
                            case (int)GEnum.SystemCode.Return_Consignment:
                            case (int)GEnum.SystemCode.Order_Consignment:
                            case (int)GEnum.SystemCode.Received_Consignment:
                                DocDetUtil.ItmLatestCostF_DblClick(objDoc, grd);
                                DocDetUtil.ItmLatestCostH_DblClick(objDoc, grd);
                                grd.Refresh();
                                break;
                        }
                        break;

                    case "itmvendorpriceratio":
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
                                DocDetUtil.ItmVendorPrice_DblClick(objDoc, grd);
                                DocDetUtil.ItmLatestCostF_DblClick(objDoc, grd);
                                DocDetUtil.ItmLatestCostH_DblClick(objDoc, grd);
                                DocDetUtil.ItmVendorPriceRatio_DblClick(objDoc, grd);
                                grd.Refresh();
                                break;
                        }
                        break;
                    case "itmcontrolprice":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Quotation:
                            case (int)GEnum.SystemCode.Reserve_Order:                            
                            case (int)GEnum.SystemCode.Sales_Order:
                            case (int)GEnum.SystemCode.Delivery_Order:
                            case (int)GEnum.SystemCode.Sales_Invoice:
                            case (int)GEnum.SystemCode.Cash_Sale:
                                DocDetUtil.ItmControlPrice_DblClick(objDoc, grd);
                                break;
                        }
                        break;
                }
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
        public static bool ItmRow_CancelDelete(Document objDoc, Hashtable docDet, GEnum.Details docDetType)
        {
            if (objDoc.IsReadOnly) return true;

            int PreRowIndex = 0;
            UltraGrid grd = null;
            bool deleteCancel = true;
            try
            {
                bool runProcessItm = false;
                bool runProcessExp = false;
                bool runProcessPOCsg = false;
                bool runProcessPack = false;
                bool runProcessPackItm = false;
                bool runProcessItmVendor = false;

                DocComUtility.DocDetail_Get(docDetType, docDet, ref grd);
                DataTable dt = grd.DataSource as DataTable;
                if (grd.ActiveRow.Index > 0)
                    PreRowIndex = grd.ActiveRow.Index - 1;

                #region get process to run
                switch (objDoc.DocCodeKey)
                {
                    #region Quotation
                    case (int)GEnum.SystemCode.Quotation:
                        switch (docDetType)
                        {
                            case GEnum.Details.Doc_Itm:
                                runProcessItm = true;
                                break;

                            case GEnum.Details.Doc_ItmVendor:
                                runProcessItmVendor = true;
                                break;

                            case GEnum.Details.Doc_Vendor:
                                MsgBox.Show("you cannot delete the vendor list");
                                return true;

                            default:
                                MsgBox.Show("Unable to match details type, cannot delete");
                                return true;
                        }
                        break;
                    #endregion

                    #region Payment
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:

                        switch (docDetType)
                        {
                            case GEnum.Details.Doc_Itm:
                                MsgBox.Show("You cannot delete details from the document apply list");
                                return true;

                            case GEnum.Details.Doc_Exp:
                                runProcessExp = true;
                                break;

                            default:
                                MsgBox.Show("Unable to match details type, cannot delete");
                                return true;
                        }
                        break;
                    #endregion

                    #region Document with Itm detail
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
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                        if (docDetType == GEnum.Details.Doc_Itm)
                            runProcessItm = true;
                        else if (docDetType == GEnum.Details.Doc_CsgItm)
                            runProcessPOCsg = true;
                        else
                        {
                            MsgBox.Show("Unable to match details type, cannot delete");
                            return true;
                        }
                        break;
                    #endregion

                    #region Issue/return consignment
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        switch (docDetType)
                        {
                            case GEnum.Details.Doc_Itm:
                                runProcessItm = true;
                                break;

                            case GEnum.Details.Doc_Exp:
                                runProcessExp = true;
                                break;

                            default:
                                MsgBox.Show("Unable to match details type, cannot delete");
                                return true;
                        }
                        break;
                    #endregion

                    #region Packing List
                    case (int)GEnum.SystemCode.Packing_List:
                        switch (docDetType)
                        {
                            case GEnum.Details.Doc_Pack:
                                runProcessPack = true;
                                break;

                            case GEnum.Details.Doc_Itm:
                                runProcessPackItm = true;
                                break;

                            default:
                                MsgBox.Show("Unable to match details type, cannot delete");
                                return true;
                        }
                        break;
                    #endregion

                    #region Order Adjustment and Contra
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                        MsgBox.Show("You cannot delete any detail for this type of documents");
                        return true;
                    #endregion

                    #region Production
                    case (int)GEnum.SystemCode.Inventory_Production:
                        if (grd.ActiveRow != null)
                        {
                            if (GFunc.NEInt(grd.ActiveRow.Cells["LineType"].Value, 0) == 3000)
                            {
                                int finishedGoodsListItmKey = GFunc.NEInt(grd.ActiveRow.Cells["ItmKey"].Value, 0);

                                DataRow[] dr = dt.Select("LineType = 3200 AND ItmFGKey = " + finishedGoodsListItmKey);
                                if(dr.Length != 0)
                                {
                                    MsgBox.Show("The Item ID is used in packing material list, cannot delete");
                                    return false;
                                }
                            }
                        }

                        if (docDetType == GEnum.Details.Doc_Itm)
                            runProcessItm = true;
                        else
                        {
                            MsgBox.Show("Unable to match details type, cannot delete");
                            return true;
                        }

                        break;
                    #endregion

                    default:
                        MsgBox.Show("Unable to match document code. cannot delete");
                        return true;
                }
                #endregion

                #region run delete Detail Item
                if (runProcessItm)
                {
                    if (DocHDRUtil.Doc_CheckDetItm(objDoc, grd, GEnum.ValidateField.DelDocItmKey) == false)
                        return true;

                    if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnDeleteDocumentDetail))
                    {
                        GEnum.MsgBoxButton btnSelect = GEnum.MsgBoxButton.Cancel;
                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Invoice)
                        {
                            if (GFunc.NEInt(grd.ActiveRow.Cells["APPDDK"].Value, 0) > 0)
                            {
                                btnSelect = MsgBox.Show(MsgID.Record.ConfirmDeleteRecord + "%" + grd.ActiveRow.Cells["ItmSN"].Value + " and related rows",
                                                      GEnum.MsgBoxIcon.Question,GEnum.MsgBoxDefaultButton.DefaultButton2,
                                                      GEnum.MsgBoxButton.Delete,
                                                      GEnum.MsgBoxButton.Dont_Delete,
                                                      GEnum.MsgBoxButton.I_Dont_Know);
                            }
                            else
                            {
                                btnSelect = MsgBox.Show(MsgID.Record.ConfirmDeleteRecord + "%" + grd.ActiveRow.Cells["ItmSN"].Value,
                                                                                  GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2,
                                                                                  GEnum.MsgBoxButton.Delete,
                                                                                  GEnum.MsgBoxButton.Dont_Delete,
                                                                                  GEnum.MsgBoxButton.I_Dont_Know);
                            }
                        }
                        else
                        {
                            btnSelect = MsgBox.Show(MsgID.Record.ConfirmDeleteRecord + "%" + grd.ActiveRow.Cells["ItmSN"].Value,
                                                  GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2,
                                                  GEnum.MsgBoxButton.Delete,
                                                  GEnum.MsgBoxButton.Dont_Delete,
                                                  GEnum.MsgBoxButton.I_Dont_Know);
                        }

                        // Check Delete
                        if (btnSelect != GEnum.MsgBoxButton.Delete)
                            return true;
                    }


                    int _docItmKey = GFunc.NEInt(grd.ActiveRow.Cells["DocItmKey"].Value, 0);
                    DataRow[] drParent;
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Purchase_Invoice:
                            if (GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, 0) != 810)
                            {
                                int _docKey = GFunc.NEInt(grd.ActiveRow.Cells["APPDDK"].Value, 0);

                                if (_docKey > 0)
                                {
                                    drParent = dt.Select("APPDDK=" + _docKey);
                                    for (int i = 0; i < drParent.Length; i++)
                                    {
                                        dt.Rows.Remove(drParent[i]);
                                    }
                                }
                                else
                                {
                                    drParent = dt.Select("DocItmKey=" + _docItmKey);
                                    if (drParent.Length > 0)
                                    {
                                        dt.Rows.Remove(drParent[0]);
                                    }
                                }
                            }
                            else
                            {
                                drParent = dt.Select("DocItmKey=" + _docItmKey);
                                if (drParent.Length > 0)
                                {
                                    dt.Rows.Remove(drParent[0]);
                                }
                            }
                            break;

                        default:
                            drParent = dt.Select("DocItmKey=" + _docItmKey);
                            if (drParent.Length > 0)
                            {
                                dt.Rows.Remove(drParent[0]);
                            }
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
                        case (int)GEnum.SystemCode.Cash_Sale:
                        case (int)GEnum.SystemCode.Cash_Debit_Note:
                        case (int)GEnum.SystemCode.Cash_Credit_Note:
                        case (int)GEnum.SystemCode.Purchase_Request:
                        case (int)GEnum.SystemCode.Purchase_Order:
                        case (int)GEnum.SystemCode.Purchase_Shipment:
                        case (int)GEnum.SystemCode.Purchase_Delivery:
                        case (int)GEnum.SystemCode.Purchase_Invoice:
                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        case (int)GEnum.SystemCode.Inventory_Adjustment:
                        case (int)GEnum.SystemCode.Inventory_Production:
                        case (int)GEnum.SystemCode.Inventory_Transfer:
                        case (int)GEnum.SystemCode.Issue_Consignment:
                        case (int)GEnum.SystemCode.Return_Consignment:
                            //to delete Childs
                            DataRow[] drChilds = dt.Select("LineLinkKey=" + _docItmKey);
                            for (int i = 0; i < drChilds.Length; i++)
                            {
                                dt.Rows.Remove(drChilds[i]);
                            }

                            //For Quotation we need to remove the related ItmVendor
                            if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Quotation)
                            {
                                DataTable dtItmVendor = null;
                                DocComUtility.DocDetail_Get(GEnum.Details.Doc_ItmVendor, docDet, ref dtItmVendor);

                                DataRow[] drchilds = dtItmVendor.Select("DocItmKey=" + _docItmKey);
                                for (int i = 0; i < drchilds.Length; i++)
                                {
                                    dtItmVendor.Rows.Remove(drchilds[i]);
                                }
                            }

                            break;

                    }

                    DocComUtility.CalForm(objDoc, docDet, true, false);
                }
                #endregion

                #region run delete Detail Expenses
                if (runProcessExp)
                {
                    if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnDeleteDocumentDetail))
                    {
                        // Ask Confirmation for Delete
                        GEnum.MsgBoxButton btnSelect;
                        btnSelect = MsgBox.Show(MsgID.Record.ConfirmDeleteRecord + "% " + grd.ActiveRow.Cells["ExpSN"].Value,
                                              GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2,
                                              GEnum.MsgBoxButton.Delete,
                                              GEnum.MsgBoxButton.Dont_Delete,
                                              GEnum.MsgBoxButton.I_Dont_Know);

                        // Check Delete
                        if (btnSelect != GEnum.MsgBoxButton.Delete)
                            return true;
                    }

                    int _docItmKey = GFunc.NEInt(grd.ActiveRow.Cells["DocItmKey"].Value, 0);
                    DataRow[] drParent = dt.Select("DocItmKey=" + _docItmKey);

                    if (drParent.Length > 0)
                    {
                        dt.Rows.Remove(drParent[0]);
                    }
                    DocComUtility.CalForm(objDoc, docDet, true, false);
                }
                #endregion

                #region run delete Detail PO Csg Items
                if (runProcessPOCsg)
                {
                    if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnDeleteDocumentDetail))
                    {
                        // Ask Confirmation for Delete
                        GEnum.MsgBoxButton btnSelect;
                        btnSelect = MsgBox.Show(MsgID.Record.ConfirmDeleteRecord + "% " + grd.ActiveRow.Cells["ItmSN"].Value,
                                              GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2,
                                              GEnum.MsgBoxButton.Delete,
                                              GEnum.MsgBoxButton.Dont_Delete,
                                              GEnum.MsgBoxButton.I_Dont_Know);

                        // Check Delete
                        if (btnSelect != GEnum.MsgBoxButton.Delete)
                            return true;
                    }

                    int _docItmKey = GFunc.NEInt(grd.ActiveRow.Cells["DocItmKey"].Value, 0);
                    DataRow[] drParent = dt.Select("DocItmKey=" + _docItmKey);

                    if (drParent.Length > 0)
                    {
                        dt.Rows.Remove(drParent[0]);
                    }
                    DocComUtility.CalForm(objDoc, docDet, true, false,true);
                }
                #endregion

                #region run delete Detail Item Vendor
                if (runProcessItmVendor)
                {
                    if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnDeleteDocumentDetail))
                    {
                        // Ask Confirmation for Delete
                        GEnum.MsgBoxButton btnSelect;
                        btnSelect = MsgBox.Show("Cofirm deletion",
                                              GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2,
                                              GEnum.MsgBoxButton.Delete,
                                              GEnum.MsgBoxButton.Dont_Delete,
                                              GEnum.MsgBoxButton.I_Dont_Know);

                        // Check Delete
                        if (btnSelect != GEnum.MsgBoxButton.Delete)
                            return true;
                    }

                    int _docItmKey = GFunc.NEInt(grd.ActiveRow.Cells["DocItmKey"].Value, 0);
                    DataRow[] drParent = dt.Select("DocItmKey=" + _docItmKey);

                    if (drParent.Length > 0)
                    {
                        dt.Rows.Remove(drParent[0]);
                    }
                }
                #endregion

                #region run delete Detail pack
                if (runProcessPack)
                {
                    if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnDeleteDocumentDetail))
                    {
                        // Ask Confirmation for Delete
                        GEnum.MsgBoxButton btnSelect;
                        btnSelect = MsgBox.Show(MsgID.Record.ConfirmDeleteRecord + "% " + grd.ActiveRow.Cells["ItmSN"].Value,
                                              GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2,
                                              GEnum.MsgBoxButton.Delete,
                                              GEnum.MsgBoxButton.Dont_Delete,
                                              GEnum.MsgBoxButton.I_Dont_Know);

                        // Check Delete
                        if (btnSelect != GEnum.MsgBoxButton.Delete)
                            return true;
                    }

                    int _docItmKey = GFunc.NEInt(grd.ActiveRow.Cells["DocItmKey"].Value, 0);
                    DataRow[] drParent = dt.Select("DocItmKey=" + _docItmKey);

                    if (drParent.Length > 0)
                    {
                        dt.Rows.Remove(drParent[0]);
                    }

                    //to delete Childs
                    DataRow[] drChilds = dt.Select("DocItmKey=" + _docItmKey);
                    for (int i = 0; i < drChilds.Length; i++)
                    {
                        dt.Rows.Remove(drChilds[i]);
                    }

                    DocComUtility.CalForm(objDoc, docDet, true, false);

                    grd = null;
                    DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grd);
                    dt = grd.DataSource as DataTable;
                    if (dt != null)
                    {

                        DataRow[] drDetChild = dt.Select("DocItmKey=" + _docItmKey);

                        if (drDetChild.Length > 0)
                        {
                            dt.Rows.Remove(drDetChild[0]);
                        }
                    }
                    DocComUtility.CalForm(objDoc, docDet, true, false);
                }
                #endregion

                #region run delete Detail pack items
                if (runProcessPackItm)
                {
                    if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnDeleteDocumentDetail))
                    {
                        // Ask Confirmation for Delete
                        GEnum.MsgBoxButton btnSelect;
                        btnSelect = MsgBox.Show(MsgID.Record.ConfirmDeleteRecord + "% " + grd.ActiveRow.Cells["DetItmSN"].Value,
                                              GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2,
                                              GEnum.MsgBoxButton.Delete,
                                              GEnum.MsgBoxButton.Dont_Delete,
                                              GEnum.MsgBoxButton.I_Dont_Know);

                        // Check Delete
                        if (btnSelect != GEnum.MsgBoxButton.Delete)
                            return true;
                    }

                    grd = null;
                    DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grd);
                    dt = grd.DataSource as DataTable;



                    if (dt != null)
                    {
                        int _docItmDetKey = GFunc.NEInt(grd.ActiveRow.Cells["DocItmDetKey"].Value, 0); //Pauk 15Jun 2011 change DocItmKey to DocItmDetKey
                        DataRow[] drParent = dt.Select("DocItmDetKey=" + _docItmDetKey);

                        if (drParent.Length > 0)
                        {
                            dt.Rows.Remove(drParent[0]);
                        }
                    }
                    DocComUtility.CalForm(objDoc, docDet, true, false);
                }
                #endregion

                objDoc.IsDirty = true;
                deleteCancel = false;
                return false;
            }
            catch (TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            finally
            {
                if (grd.Rows.Count > 0 && deleteCancel == false)
                {
                    grd.Rows[PreRowIndex].Selected = true;
                    grd.Rows[PreRowIndex].Activate();
                }
            }
        }//Completed
        public static bool ItmRow_Update(Document objDoc, Hashtable details, bool POCsgItems = false)
        {
            try
            {
                return ItmRow_Update(objDoc, details, false,false,POCsgItems);
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
        public static bool ItmRow_Update(Document objDoc, Hashtable details, bool DetTaxAmtChangeByUser, bool ColTaxAmt = false,bool POCsgItems=false)
        {
            try
            {
                bool caltax = true;
                if (DetTaxAmtChangeByUser)
                    caltax = false;

                if (DocComUtility.CalForm(objDoc, details, caltax, false, ColTaxAmt,POCsgItems))
                {
                    objDoc.IsDirty = true;
                    return true;
                }

                return false;
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

        public static bool ItmRow_Update(SqlConnection cn,Document objDoc, Hashtable details, bool DetTaxAmtChangeByUser, bool POCsgItems = false)
        {
            try
            {
                bool caltax = true;
                if (DetTaxAmtChangeByUser)
                    caltax = false;

                if (DocComUtility.CalForm(cn,objDoc, details, caltax, false,POCsgItems))
                {
                    objDoc.IsDirty = true;
                    return true;
                }
                return false;
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
        public static bool ItmRow_CustomCellUpdate(Document objDoc, Hashtable docDet, GEnum.Details docDetType, string listSettingID,int jobItem=0)
        {
            try
            {
                UltraGrid grd = null;
                string currentColumnKey = string.Empty;
                DocComUtility.DocDetail_Get(docDetType, docDet, ref grd);

                UltraGridCell gridCell = grd.ActiveCell;                
                currentColumnKey = grd.ActiveCell.Column.Key;
                string currentColumnValue = grd.ActiveCell.Text.ToString();

                switch (currentColumnKey.ToLower())
                {
                    #region Perform process for the active column (this column are without TextEditor Control)
                    case "itmid":
                    case "detitmid":
                        if (DocDetUtil.ItmID_CustomUpdate(objDoc, docDet, currentColumnValue, GEnum.RecAccessType.ItemID, listSettingID, jobItem))
                        {                           
                            DocHDRUtil.FormGridLock_Set(objDoc, grd, GEnum.Details.Doc_Itm, false);
                            GlobalUI.PopupRefresh(grd);
                            return true;
                        }
                        else 
                            return false;

                    case "itmdes":
                        if (DocDetUtil.ItmID_CustomUpdate(objDoc, docDet, currentColumnValue, GEnum.RecAccessType.ItemDes, listSettingID, jobItem))
                        {
                            DocHDRUtil.FormGridLock_Set(objDoc, grd, GEnum.Details.Doc_Itm, false);
                            GlobalUI.PopupRefresh(grd);
                            return true;
                        }
                        else
                            return false;
                        break;

                    case "itmacckey":
                    case "itmapplydisacckey":
                    case "itmapplygainacckey":
                    case "expacckey":
                    case "itmfromacckey":
                    case "itmtoacckey":
                    case "itmdocacckey":
                        if (DocDetUtil.DetAccID_CustomUpdate(objDoc, docDet, gridCell, GEnum.RecAccessType.AccID, listSettingID) == false)
                            return true;
                        break;
                    case "itmaccdes":
                    case "itmdocaccdes":
                        if (DocDetUtil.DetAccID_CustomUpdate(objDoc, docDet, gridCell, GEnum.RecAccessType.AccDes, listSettingID) == false)
                            return true;
                        break;
                    case "itmvendorkey":
                        if (DocDetUtil.ItmVendorID_CustomUpdate(objDoc, grd, GEnum.RecAccessType.VendID, listSettingID)==false)
                            return false;
                        break;
                    case "itmvendornm":
                        if (DocDetUtil.ItmVendorID_CustomUpdate(objDoc, grd, GEnum.RecAccessType.VendNm, listSettingID)==false)
                            return false;
                        break;
                    case "itmdeptkey":
                        if (DocDetUtil.ItmDeptKey_CustomUpdate(objDoc, grd) == false)
                            return true;
                        break;

                    case "itmjobid":
                    case "itmjobkey":
                        if (DocDetUtil.DetJobKey_CustomUpdate(objDoc, docDet) == false)
                            return true;
                        break;

                    case "itmtransgrpid":
                    case "itmtrangrpkey":
                        if (DocDetUtil.ItmTranGrpKey_CustomUpdate(objDoc, grd) == false)
                            return true;
                        break;

                    case "itmlockey":
                        if (DocDetUtil.ItmLocKey_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmfromlockey":
                        if (DocDetUtil.ItmFromLocKey_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmtolockey":
                        if (DocDetUtil.ItmFromLocKey_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmqty":
                        if (DocDetUtil.ItmQty_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmuomkey":
                        if (DocDetUtil.ItmUOMKey_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmlistprice":
                        if (DocDetUtil.ItmListPrice_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmpriceafter":
                        if (DocDetUtil.ItmPriceAfter_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmdispercent":
                        if (DocDetUtil.ItmDisPercent_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmdisvalue":
                        if (DocDetUtil.ItmDisValue_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmpriceuser":
                        if (DocDetUtil.ItmPriceUser_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmamtshw":
                        if (DocDetUtil.ItmAmtShw_CustomeUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmtaxgrpkey":
                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Journal)
                        {
                            if (DocDetUtil.GNLItmTaxGrpKey_CustomUpdate(objDoc, grd) == false)
                                return false;
                        }
                        else if (DocDetUtil.ItmTaxGrpKey_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmvendorcurrrate":
                        if (DocDetUtil.ItmVendorCurrRate_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmvendorprice":
                        if (DocDetUtil.ItmVendorPrice_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmvendorpriceratio":
                        if (DocDetUtil.ItmMarkupRate_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmjobphasekey":
                        if (DocDetUtil.DetJobPhaseKey_CustomUpdate(objDoc, docDet) == false)
                            return false;
                        break;
                    case "itmjobtaskkey":
                        if (DocDetUtil.DetJobTaskKey_CustomUpdate(objDoc, docDet) == false)
                            return false;
                        break;
                    case "itmjobcosttypekey":
                        if (DocDetUtil.DetJobCostTypeKey_CustomUpdate(objDoc, docDet) == false)
                            return false;
                        break;
                    case "sku1":
                        if (DocDetUtil.SKU_CustomUpdate(objDoc, docDet, 1, grd.ActiveCell.Text) == false)
                            return false;
                        else
                            DocHDRUtil.FormGridLock_Set(objDoc, grd, GEnum.Details.Doc_Itm, false);
                        break;
                    case "sku2":
                        if (DocDetUtil.SKU_CustomUpdate(objDoc, docDet, 2, grd.ActiveCell.Text) == false)
                            return false;
                        else
                            DocHDRUtil.FormGridLock_Set(objDoc, grd, GEnum.Details.Doc_Itm, false);
                        break;
                    case "itmqtydelivered":
                        if (DocDetUtil.ItmQtyDelivered_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmorderstatus":
                        if (DocDetUtil.ItmOrderStatus_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "expdeptkey":
                        if (DocDetUtil.ExpDeptKey_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "expamt":
                        if (DocDetUtil.ExpAmt_CustomeUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "expamtgst":
                        if (DocDetUtil.ExpAmtGST_CustomeUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "exptaxgrpkey":
                        if (DocDetUtil.ExpTaxGrpKey_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "expjobphasekey":
                        if (DocDetUtil.DetJobPhaseKey_CustomUpdate(objDoc, docDet) == false)
                            return false;
                        break;
                    case "expjobtaskkey":
                        if (DocDetUtil.DetJobTaskKey_CustomUpdate(objDoc, docDet) == false)
                            return false;
                        break;
                    case "expjobcosttypekey":
                        if (DocDetUtil.DetJobCostTypeKey_CustomUpdate(objDoc, docDet) == false)
                            return false;
                        break;
                    case "itmapplydisamtf":
                        if (DocDetUtil.ItmApplyDisAmtF_CustomeUpdate(objDoc, docDet,false) == false)
                            return false;
                        break;
                    case "itmapplydocamtf":
                        if (DocDetUtil.ItmApplyDocAmtF_CustomeUpdate(objDoc, docDet) == false)
                            return false;
                        break;
                    case "itmapplyfull":
                        if (DocDetUtil.ItmApplyFull_CustomeUpdate(objDoc, docDet) == false)
                            return false;
                        break;
                    case "itmprmdatenew":
                        if (DocDetUtil.ItmPrmDateNew_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmprice":
                        if (DocDetUtil.ItmPrice_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmdisprice":
                        if (DocDetUtil.ItmDisPrice_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmdocdeptkey":
                        if (DocDetUtil.ItmDocDeptKey_CustomeUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmdocamtf":
                        MsgBox.Show("Invalid call to ItmDocAmtF_CustomUpdate from RowCustomCellUpdate");
                        return false;
                    case "itmamtf":
                        if (DocDetUtil.ItmAmtF_CustomeUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmdoccurrkey":
                        MsgBox.Show("Invalid call to ItmDocCurrKey_CustomUpdate from RowCustomCellUpdate");
                        return false;
                    case "itmdoccurrrate":
                        MsgBox.Show("Invalid call to ItmDocCurrRate_CustomUpdate from RowCustomCellUpdate");
                        return false;
                    case "itmbankamtf":
                        MsgBox.Show("Invalid call to ItmBankAmtF_CustomUpdate from RowCustomCellUpdate");
                        return false;
                    case "itmbankrate":
                        MsgBox.Show("Invalid call to ItmBankRate_CustomUpdate from RowCustomCellUpdate");
                        return false;
                    case "itmcost":
                        if (DocDetUtil.ItmCost_CustomeUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmcountryrate":
                        if (DocDetUtil.ItmCountryRate_CustomeUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmcreditf":
                        if (DocDetUtil.ItmCreditF_CustomeUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmcredith":
                        if (DocDetUtil.ItmCreditH_CustomeUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmcurrkey":
                        if (DocDetUtil.ItmCurrKey_CustomeUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmcurrrate":
                        if (DocDetUtil.ItmCurrRate_CustomeUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmdebitf":
                        if (DocDetUtil.ItmDebitF_CustomeUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmdebith":
                        if (DocDetUtil.ItmDebitH_CustomeUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmqtym1":
                    case "itmqtym2":
                    case "itmqtym3":
                    case "itmqtym4":
                    case "itmqtym5":
                    case "itmqtym6":
                    case "itmqtym7":
                    case "itmqtym8":
                    case "itmqtym9":
                    case "itmqtym10":
                    case "itmqtym11":
                    case "itmqtym12":
                        if (DocDetUtil.ItmQtyM_CustomeUpdate(objDoc, grd, currentColumnKey) == false)
                            return false;
                        break;
                    case "itmqtymtotal":
                        MsgBox.Show("Invalid call to ItmQtyMTotal_CustomUpdate from RowCustomCellUpdate");
                        return false;
                    case "itmnewcost":
                        if (DocDetUtil.ItmNewCost_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "fgreq":
                        if (DocDetUtil.FGReq_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "fgoverheadkey":
                        if (DocDetUtil.FGOverHeadKey_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "fgoverheadcost":
                        if (DocDetUtil.FGOverHeadCost_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "bomreq":
                        if (DocDetUtil.BOMReq_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "bomissue":
                        if (DocDetUtil.BOMIssue_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "bomreturn":
                        if (DocDetUtil.BOMReturn_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "bomused":
                        if (DocDetUtil.BOMUsed_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "bomlabourcost":
                        if (DocDetUtil.BOMLabourCost_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "itmpackweightnet":
                        if (DocDetUtil.ItmPackWeightNet_CustomeUpdate(objDoc, docDet) == false)
                            return false;
                        break;
                    case "itmpackweighttare":
                        if (DocDetUtil.ItmPackWeightTare_CustomeUpdate(objDoc, docDet) == false)
                            return false;
                        break;
                    case "itmpackweightgross":
                        if (DocDetUtil.ItmPackWeightGross_CustomeUpdate(objDoc, docDet) == false)
                            return false;
                        break;
                    case "detitmdeptkey":
                        if (DocDetUtil.DetItmDeptKey_CustomeUpdate(objDoc, docDet) == false)
                            return false;
                        break;
                    case "detitmqtyperpack":
                        if (DocDetUtil.DetItmQtyPerPack_CustomeUpdate(objDoc, docDet) == false)
                            return false;
                        break;
                    case "detitmqtytotal":
                        if (DocDetUtil.DetItmQtyTotal_CustomeUpdate(objDoc, docDet) == false)
                            return false;
                        break;
                    case "detitmuomkey":
                        if (DocDetUtil.DetItmUOMKey_CustomeUpdate(objDoc, docDet) == false)
                            return false;
                        break;
                    case "detitmweightgross":
                        if (DocDetUtil.DetItmWeightGross_CustomeUpdate(objDoc, docDet) == false)
                            return false;
                        break;
                    case "detitmweightnet":
                        if (DocDetUtil.DetItmWeightNet_CustomeUpdate(objDoc, docDet) == false)
                            return false;
                        break;
                    case "detitmweightuomkey":
                        if (DocDetUtil.DetItmWeightUOMKey_CustomeUpdate(objDoc, docDet) == false)
                            return false;
                        break;
                    case "itmfgkey":
                        if (DocDetUtil.ItmFGKey_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break;
                    case "dsqty":
                        if (DocDetUtil.DSQty_CustomUpdate(objDoc, grd) == false)
                            return false;
                        break; /* added by yst on 30 dec 2018 to auto fill 0 when cell is empty */
                        #endregion
                }
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

        // added by yst on 30 dec 2018 to auto fill 0 when cell is empty
        public static bool DSQty_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            string msgID = string.Empty;
            int ItmType;
            try
            {
                ItmType = GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, 0);
                switch (ItmType)
                {
                    case (int)GEnum.ItemType.Stock:
                    case (int)GEnum.ItemType.Non_Stock:                   
                        grd.ActiveRow.Cells["DSQty"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["DSQty"].Value, 0), GVar.RndDecs.Qtypt);
                        break;
                }
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

        //Grid Cell Button Click
        public static void DetItmGrid_CellButtonClick(Document objDoc, Hashtable docDet, UltraGridCell gridCell, string listSettingID)
        {
            try
            {

                TAUtil.TAGridEditor vgrid = (TAUtil.TAGridEditor)docDet[GEnum.Details.Doc_Itm];
                switch (gridCell.Column.Key.ToLower())
                {

                    case "itmid":
                    case "itmdes":
                    case "detitmid":
                    case "detitmdes":
                        DocDetUtil.ItmID_btnClick(objDoc, docDet, gridCell, gridCell.Column.Key.ToLower().ToString().EndsWith("id") ? GEnum.PopupType.ItmID : GEnum.PopupType.ItmDes, listSettingID);
                        DocHDRUtil.FormGridLock_Set(objDoc, vgrid, GEnum.Details.Doc_Itm, false);
                        GlobalUI.PopupRefresh(vgrid);
                        break;
                    case "itmacckey":
                    case "itmapplydisacckey":
                    case "itmapplygainacckey":
                    case "itmfromacckey":
                    case "itmtoacckey":
                    case "expacckey":
                    case "itmdocacckey":
                        DocDetUtil.DetAccID_btnClick(objDoc, docDet, gridCell, GEnum.PopupType.AccID, listSettingID);
                        break;
                    case "itmaccdes":
                    case "itmapplydisaccdes":
                    case "itmapplygainaccdes":
                    case "itmfromaccdes":
                    case "itmtoaccdes":
                    case "expaccdes":
                    case "itmdocaccdes":
                        DocDetUtil.DetAccID_btnClick(objDoc, docDet, gridCell, GEnum.PopupType.AccDes, listSettingID);
                        break;

                    case "itmvendorkey":
                    case "vendorid":
                        DocDetUtil.ItmVendorID_btnClick(objDoc, vgrid, gridCell, GEnum.PopupType.VendID, listSettingID);
                        break;
                    case "itmvendornm":
                    case "vendornm":
                        DocDetUtil.ItmVendorID_btnClick(objDoc, vgrid, gridCell, GEnum.PopupType.VendNm, listSettingID);
                        break;
                    case "itmtrangrpkey":
                        DocDetUtil.ItmTranGrpID_btnClick(objDoc, vgrid);
                        break;
                    case "itmjobkey":
                    case "expjobkey":
                        DocDetUtil.DetJobID_btnClick(objDoc, docDet, gridCell.Text.ToString(), listSettingID);
                        break;
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
        public static bool ItmAttachment_btnClick(Form frm, SYSAttachments DocAttachment, Document objDoc, UltraGrid grd)
        {
            try
            {
                frmAttachment f = null;
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
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                        f = new frmAttachment(DocAttachment, (int?)objDoc.DocCodeKey, objDoc.DocKey, (int?)grd.ActiveRow.Cells["DocItmKey"].Value, (int)grd.ActiveRow.Cells["LineType"].Value);

                        break;
                    default:
                        f = new frmAttachment(DocAttachment, (int?)objDoc.DocCodeKey, objDoc.DocKey, (int?)grd.ActiveRow.Cells["DocItmKey"].Value, 0);

                        break;
                }

                f.ShowDialog(frm);
                if (f.DialogResult == DialogResult.Yes)
                    grd.ActiveRow.Cells["ItmAttachment"].Value = true;
                else
                    grd.ActiveRow.Cells["ItmAttachment"].Value = false;
                return false;
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

        //Grid Cell Event
        private static bool SKU_CustomUpdate(Document objDoc, Hashtable docDet, int? SKUType, string Ctrl_SKUText)
        {
            try
            {
                int? key = 0;

                if (Ctrl_SKUText == string.Empty)
                    return false;
                else
                {
                    MSTItm objItm = MSTItm.Get(Ctrl_SKUText, SKUType);

                    if (GFunc.NEInt(objItm.SubstituteItmKey, 0) > 0)
                    {
                        key = objItm.SubstituteItmKey;
                    }
                    else
                    {
                        key = objItm.ItmKey;
                    }

                    return ItmID_Update(objDoc, docDet, GFunc.NEInt(key, 0));
                }
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
        public static bool ItmID_btnClick(Document objDoc, Hashtable docDet, UltraGridCell gridCell, GEnum.PopupType popUpType, string listSettingID)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;

                if (DocHDRUtil.EditorButton_Popup(objDoc, (int)objDoc.DocCodeKey, gridCell.Text, listSettingID, (int)popUpType, ref key, ref id, ref des))
                {
                    if (ItmID_Update(objDoc, docDet, key))
                    {
                        return true;
                    }
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
        public static bool ItmID_CustomUpdate(Document objDoc, Hashtable docDet, TAUtil.TATextBoxEditor ctrl, int key)
        {
            //note: this function is only use in InsertDataMatrix
            try
            {
                UltraGrid grdItm = null;

                string id = string.Empty;
                string des = string.Empty;
                string ctrlValue = string.Empty;
                int popUpType = 0;

                #region Get ActiveCell Value
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Packing_List:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                        break;

                    default:
                        MsgBox.Show("Unable to match Document Code");
                        return false;
                }
                #endregion

                if (ItmID_Update(objDoc, docDet, key))
                {
                    ctrl.IsDirty = false;
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
        public static bool ItmID_CustomUpdate(Document objDoc, Hashtable docDet, string ItmID, GEnum.RecAccessType recAccessType, string listSettingID, int jobItem = 0)
        {
            try
            {
                UltraGrid grdItm = null;
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string ctrlValue = string.Empty;
                int popUpType = 0;

                #region Get ActiveCell Value
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Packing_List:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment: 
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                        break;
                    case (int)GEnum.SystemCode.Journal:
                        return true;
                        break;
                    default:
                        MsgBox.Show("Unable to match Document Code");
                        return false;
                }
                #endregion

                #region get popUpType and check if user is editing the item description which we will just do nothing and return true
                switch (recAccessType)
                {
                    case GEnum.RecAccessType.ItemID:
                    case GEnum.RecAccessType.ItemIDSub:
                        if ((grdItm.DataSource as DataTable).Columns.Contains("DetItmID"))//Packing List 
                        {
                            ctrlValue = (grdItm.ActiveRow != null) ? GFunc.NEStr(grdItm.ActiveRow.Cells["DetItmID"].Value.ToString(), string.Empty) : string.Empty;
                        }
                        else
                        {
                            ctrlValue = (grdItm.ActiveRow != null) ? GFunc.NEStr(grdItm.ActiveRow.Cells["ItmID"].Value.ToString(), string.Empty) : string.Empty;
                        }
                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Inventory_Production)
                        {
                            if (GFunc.GetIntPropertyValue("DocProDetails", objDoc) == (int)GEnum.InventoryDetails.FinishedGoods)
                            {
                                popUpType = (int)GEnum.PopupType.ItmFinishID;
                            }
                            else
                            {
                                popUpType = (int)GEnum.PopupType.ItmStkID;
                            }
                        }
                        else
                        {
                            popUpType = (int)GEnum.PopupType.ItmID;
                        }
                        break;

                    case GEnum.RecAccessType.ItemDes:
                    case GEnum.RecAccessType.ItemDesSub:
                        //if ItmKey >=0 means that the user has already selected an item (or insert Blank Row) and the user is trying to amend the description
                        //so no update is required
                        if (GFunc.IsNE(grdItm.ActiveRow.Cells["ItmKey"].Value) == false)
                            return true;

                        ctrlValue = GFunc.NEStr(grdItm.ActiveRow.Cells["ItmDes"].Value.ToString(), string.Empty);
                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Inventory_Production)
                        {
                            if (GFunc.GetIntPropertyValue("DocProDetails", objDoc) == (int)GEnum.InventoryDetails.FinishedGoods)
                            {
                                popUpType = (int)GEnum.PopupType.ItmFinishDes;
                            }
                            else
                            {
                                popUpType = (int)GEnum.PopupType.ItmStkDes;
                            }
                        }
                        else
                        {
                            popUpType = (int)GEnum.PopupType.ItmDes;
                        }
                        break;

                    default:
                        return false;
                }
                #endregion

                key = GFunc.ItmRecord_GetKey(recAccessType, listSettingID, ctrlValue, 0, ref id, ref des, true);
                if (key == 0)
                {
                    if (DocHDRUtil.EditorButton_Popup(objDoc, (int)objDoc.DocCodeKey, ItmID, listSettingID, popUpType, ref key, ref id, ref des) == false)
                        return false;
                }

                if (ItmID_Update(objDoc, docDet, key, jobItem))
                {
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

        //job 0=Not JobItem, 1=Exclusive Job Item, 2=Non Exclusive Job Item
        public static bool ItmID_Update(Document objDoc, Hashtable docDet, int key, int jobItem = 0)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();

                    return ItmID_Update(cn, objDoc, docDet, key, jobItem);
                    
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
        public static bool ItmID_Update(SqlConnection cn, Document objDoc, Hashtable docDet, int key, int jobItem = 0)
        {
            try
            {
                #region Declaration of variables
                DataTable dtItm = null;
                UltraGrid grdItm = null;
                UltraGridRow grdRow = null;    //active row
                MSTItm objItm = null;

                string msgID = string.Empty;
                bool RunGetItmPrice = false;
                GEnum.SpecialCalculationType calType = GEnum.SpecialCalculationType.Sale;

                int? ItmKey = 0;
                int? ItmKeySelected = 0;
                int? ItmType = 0;
                string ItmID = string.Empty;
                string IndustryPN = string.Empty;
                string CountryID = string.Empty;
                string ItmDes = string.Empty;
                int? ItmAccKey = 0;
                decimal? ItmStock = 0;
                decimal? ItmQty = 0;
                decimal? ItmQtyLink = 0;
                decimal? ItmQtyAdj = 0;
                int? ItmOrderStatus = 0;
                int? ItmUOMKey = 0;
                decimal? ItmConRate = 0;
                decimal? ItmLatestCostF = 0;
                decimal? ItmLatestCostH = 0;
                decimal? ItmPriceList = 0;
                decimal? ItmPriceBefore = 0;
                decimal? ItmPriceAfter = 0;
                decimal? ItmDisPercent = 0;
                decimal? ItmDisValue = 0;
                decimal? ItmPriceUser = 0;
                decimal? ItmControlPrice = 0;
                decimal? ItmControlPriceBase = 0;
                decimal? ItmAmtShw = 0;
                int? ItmColorKey = 0;
                string ItmScaleSize = string.Empty;
                string ItmPacking = string.Empty;
                decimal? ItmWeightUOMRate = 1;
                string ItmMark = string.Empty;
                string ItmRem = string.Empty;
                string ItmRef = string.Empty;

                int MarkUpType = 0;
                decimal? ItmMarkupRate = 0;
                decimal? ItmMarkupRatio = 0;
                decimal minMarkupRate = 0;

                int? ItmVendorKey = null;
                int? ItmVendorCurrKey = 1;
                decimal? ItmVendorCurrRate = 1;
                decimal? ItmVendorPrice = 0;

                int? DocConKey = 0;
                int? DocCurrKey = 1;
                decimal? DocCurrRate = 1;
                int? DocPriceTypeKey = null;
                string DocMode = string.Empty;              //AR,AP,IN
                int? ItmTypeGrp = 0;
                bool runSpecialCalculation = false;
                #endregion

                #region Check detail for condition that disallow changes of itemid
                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref dtItm);

                if (DocHDRUtil.Doc_CheckDetItm(objDoc, grdItm, GEnum.ValidateField.ItmKey))
                    grdRow = grdItm.ActiveRow;
                else
                    return false;

                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Inventory_Production)
                {
                    if (Check_DuplicateItm(objDoc, grdItm, key, GFunc.NEInt(grdItm.ActiveRow.Cells["ItmFGKey"].Value, 0)) == false)
                        return false;
                }
                #endregion

                #region Get MSTItm Object
                objItm = MSTItm.Get(cn, key);
                if (GFunc.NEInt(objItm.SubstituteItmKey, 0) > 0)
                {
                    ItmKey = (int)objItm.SubstituteItmKey;
                    ItmKeySelected = (int)objItm.ItmKey;
                    objItm = MSTItm.Get(cn, key);
                }
                else
                {
                    ItmKey = (int)objItm.ItmKey;
                    ItmKeySelected = key;
                }

                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Quotation ||
                    objDoc.DocCodeKey == (int)GEnum.SystemCode.Reserve_Order ||
                    objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Order ||
                     objDoc.DocCodeKey == (int)GEnum.SystemCode.Delivery_Order)
                {
                    if (jobItem == 1 && objItm.ItmType < 800)
                    {
                        MsgBox.Show("This Quotation includes Job items to sell exclusively. Only remarks or header items are allowed to add.");
                        return false;
                    }
                }
                if (objDoc.DocCodeKey==(int)GEnum.SystemCode.Purchase_Order ||
                    objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Delivery ||
                    objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Invoice)
                {
                    if(objItm.BlockPurchase)
                    {
                        MsgBox.Show("Item is blocked to purchase.\nPlease check with the management to purchase this item.");
                        return false;
                    }
                }
                /* added by yst to avoid inventory discrepancies between wms and boss , DocState is checked for Copy function */
                if (SysOptionUtility.UseWMS && objDoc.DocCodeKey == (int)GEnum.SystemCode.Delivery_Order && objDoc.DocState > (int)GEnum.DocState.New)
                {
                    int itmType = (int)objItm.ItmType;
                    if (itmType > 0)
                    {
                        if (itmType == (int)GEnum.ItemType.Stock ||
                            itmType == (int)GEnum.ItemType.Non_Stock ||
                            itmType == (int)GEnum.ItemType.Assembly)
                        {
                            MsgBox.Show("This type of item is not allowed to be added in DO only. SO and DO must tally.", GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                            return false;
                        }
                    }
                }
                /* end */
                #endregion

                #region Set Defaultvalues to variables base on ItmType
                switch ((int)objItm.ItmType)
                {
                    case (int)GEnum.ItemType.Stock:
                    case (int)GEnum.ItemType.StockB:
                    case (int)GEnum.ItemType.Consignment:
                    case (int)GEnum.ItemType.Finished_GD:
                    case (int)GEnum.ItemType.Finished_GDB:
                    case (int)GEnum.ItemType.Serial_Finished_GDB:
                    case (int)GEnum.ItemType.Serial_StockB:
                        ItmStock = 0;
                        ItmQty = 0;
                        ItmConRate = 0;
                        ItmLatestCostF = 0;
                        ItmLatestCostH = 0;
                        ItmPriceBefore = 0;
                        ItmPriceAfter = 0;
                        ItmDisPercent = 0;
                        ItmDisValue = 0;
                        ItmPriceUser = 0;
                        ItmControlPrice = 0;
                        ItmControlPriceBase = 0;
                        break;

                    case (int)GEnum.ItemType.Assembly:
                    case (int)GEnum.ItemType.Non_Stock:
                    case (int)GEnum.ItemType.Service:
                    case (int)GEnum.ItemType.Charges:
                        ItmStock = 0;
                        ItmQty = 0;
                        ItmConRate = 0;
                        ItmLatestCostF = 0;
                        ItmLatestCostH = 0;
                        ItmPriceBefore = 0;
                        ItmPriceAfter = 0;
                        ItmDisPercent = 0;
                        ItmDisValue = 0;
                        ItmPriceUser = 0;
                        ItmControlPrice = 0;
                        ItmControlPriceBase = 0;

                        break;

                    case (int)GEnum.ItemType.Discount:
                        ItmStock = 0;
                        ItmQty = null;
                        ItmConRate = 0;
                        ItmLatestCostF = 0;
                        ItmLatestCostH = 0;
                        ItmPriceBefore = 0;
                        ItmPriceAfter = 0;
                        ItmDisPercent = 0;
                        ItmDisValue = 0;
                        ItmPriceUser = 0;
                        ItmControlPrice = 0;
                        ItmControlPriceBase = 0;
                        break;

                    case (int)GEnum.ItemType.Master:
                    case (int)GEnum.ItemType.Header:
                    case (int)GEnum.ItemType.Remark:
                    case (int)GEnum.ItemType.Sub_Total:
                    case (int)GEnum.ItemType.BF_Total:
                    case (int)GEnum.ItemType.Total:
                        ItmStock = null;
                        ItmQty = null;
                        ItmConRate = null;
                        ItmLatestCostF = null;
                        ItmLatestCostH = null;
                        ItmPriceBefore = null;
                        ItmPriceAfter = null;
                        ItmDisPercent = null;
                        ItmDisValue = null;
                        ItmPriceUser = null;
                        ItmControlPrice = null;
                        ItmControlPriceBase = null;
                        break;

                }
                #endregion

                #region set process to run base on DocCode
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
                        DocMode = "AR";
                        DocConKey = (int?)GFunc.GetPropertyValue("DocConKey", objDoc); //Ask Mic
                        DocCurrKey = (int?)GFunc.GetPropertyValue("DocCurrKey", objDoc);//Ask Mic
                        DocCurrRate = (decimal?)GFunc.GetPropertyValue("DocCurrRate", objDoc);//Ask Mic
                        DocPriceTypeKey = (int?)GFunc.GetPropertyValue("DocPriceType", objDoc);//Ask Mic
                        RunGetItmPrice = true;
                        calType = GEnum.SpecialCalculationType.Sale;
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        DocMode = "AR";
                        DocConKey = (int?)GFunc.GetPropertyValue("DocConKey", objDoc);
                        DocCurrKey = (int?)GFunc.GetPropertyValue("DocCurrKey", objDoc);
                        DocCurrRate = (decimal?)GFunc.GetPropertyValue("DocCurrRate", objDoc);
                        DocPriceTypeKey = (int?)GFunc.GetPropertyValue("DocPriceType", objDoc);
                        RunGetItmPrice = true;
                        break;

                    case (int)GEnum.SystemCode.Purchase_Plan:
                        DocMode = "AP";
                        DocConKey = (int?)GFunc.GetPropertyValue("DocConKey", objDoc);
                        calType = GEnum.SpecialCalculationType.Purchase;
                        break;

                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        DocMode = "AP";
                        DocConKey = (int?)GFunc.GetPropertyValue("DocConKey", objDoc);
                        DocCurrKey = (int?)GFunc.GetPropertyValue("DocCurrKey", objDoc);
                        DocCurrRate = (decimal?)GFunc.GetPropertyValue("DocCurrRate", objDoc);
                        DocPriceTypeKey = (int?)GFunc.GetPropertyValue("DocPriceType", objDoc);
                        RunGetItmPrice = true;
                        calType = GEnum.SpecialCalculationType.Purchase;
                        break;

                    case (int)GEnum.SystemCode.Purchase_Request:
                        DocMode = "AP";
                        DocConKey = 0;
                        DocCurrKey = 1;
                        DocCurrRate = 1;
                        DocPriceTypeKey = 1000;
                        RunGetItmPrice = false;
                        calType = GEnum.SpecialCalculationType.Purchase;
                        break;

                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        DocMode = "AP";
                        DocConKey = (int?)GFunc.GetPropertyValue("DocConKey", objDoc);
                        DocCurrKey = (int?)GFunc.GetPropertyValue("DocCurrKey", objDoc);
                        DocCurrRate = (decimal?)GFunc.GetPropertyValue("DocCurrRate", objDoc);
                        DocPriceTypeKey = (int?)GFunc.GetPropertyValue("DocPriceType", objDoc);
                        RunGetItmPrice = true;
                        break;

                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                        DocMode = "IN";
                        DocConKey = 0;
                        DocCurrKey = 1;
                        DocCurrRate = 1;
                        DocPriceTypeKey = 1000; //Standard Price
                        RunGetItmPrice = false;
                        break;

                    case (int)GEnum.SystemCode.Packing_List:
                        //do nothing
                        break;

                    default:
                        MsgBox.Show(cn,MsgID.Document.DocumentCodeNotMatch, GEnum.MsgBoxIcon.Error, GEnum.MsgBoxButton.OK);
                        return false;
                }
                #endregion

                #region set DocDetail variables (General information account,uom,cost, etc)

                #region set ItmID, ItmType and ItmTypeGrp
                ItmID = objItm.ItmID;
                IndustryPN = objItm.IndustryPN;
                CountryID = objItm.CountryID;
                ItmType = objItm.ItmType;
                ItmTypeGrp = GFunc.GetINTypeGroup(ItmType);
                #endregion

                #region set ItmDes
                //comment by Jane. changed to show the itmdes for Total ItemType also. for BHM. 
                //if (ItmTypeGrp == (int)GEnum.INTypeGrp.Total)
                //    ItmDes = string.Empty;
                //else
                    ItmDes = objItm.ItmDes;
                #endregion

               
                #region set ItmAccKey
                switch (ItmTypeGrp)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                        switch (DocMode.ToLower())
                        {
                            case "ar":
                                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Issue_Consignment || objDoc.DocCodeKey == (int)GEnum.SystemCode.Return_Consignment)
                                {                                 
                                    ItmAccKey = (objDoc.DefAccKey > 0) ? objDoc.DefAccKey : objItm.AccINKey;
                                }
                                else
                                {
                                    ItmAccKey = (objDoc.DefAccKey > 0) ? objDoc.DefAccKey : objItm.AccICKey;
                                }
                                break;

                            case "ap":
                                if (ItmType == (int)GEnum.ItemType.Consignment)
                                {
                                    ItmAccKey = objItm.AccPHKey;
                                }
                                else
                                {
                                    if (DocComUtility.IsItmCostingContinuous(cn))
                                        ItmAccKey = objItm.AccINKey;
                                    else
                                        ItmAccKey = objItm.AccPHKey;
                                }

                                break;

                            default:
                                //IN Document
                                ItmAccKey = objItm.AccINKey;
                                break;
                        }
                        break;

                    case (int)GEnum.INTypeGrp.Non_Stock:
                    case (int)GEnum.INTypeGrp.Charges:
                    case (int)GEnum.INTypeGrp.Discount:
                        if (GFunc.CompareString(DocMode, "AR"))
                            ItmAccKey = (objDoc.DefAccKey > 0) ? objDoc.DefAccKey : objItm.AccICKey;
                        else
                            ItmAccKey = (objDoc.DefAccKey > 0) ? objDoc.DefAccKey : objItm.AccPHKey;
                        break;

                    default:
                        ItmAccKey = null;
                        break;
                }


                #endregion

                #region set ItmStock
                if (ItmTypeGrp == (int)GEnum.INTypeGrp.Stock)
                    ItmStock = objItm.QtyStock;
                else
                    ItmStock = null;
                #endregion

                #region set ItmQty, ItmQtyLink, ItmQtyAdj, ItmOrderStatus
                switch (ItmTypeGrp)
                {
                    case (int)GEnum.INTypeGrp.Charges:
                        ItmQty = 0;
                        ItmQtyLink = 0;
                        ItmQtyAdj = 0;
                        ItmOrderStatus = 10;    //Pending
                        break;
                    case (int)GEnum.INTypeGrp.Total:
                    case (int)GEnum.INTypeGrp.Remark:
                        ItmQtyLink = 0;
                        ItmQtyAdj = 0;
                        ItmOrderStatus = 0;    //NA
                        break;
                    default:
                        ItmQty = 0;
                        ItmQtyLink = 0;
                        ItmQtyAdj = 0;
                        ItmOrderStatus = 0;     //NA
                        break;
                }
                #endregion

                #region set ItmUOMKey, ItmConRate
                switch (ItmTypeGrp)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                        ItmUOMKey = objItm.BUOMKey;
                        ItmConRate = 1;
                        break;
                    default:
                        ItmUOMKey = null;
                        ItmConRate = null;
                        break;
                }
                #endregion

                #region Set ItmLatestCostF, ItmLatestCostH
                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Inventory_Adjustment)
                {
                    if (SysOptionUtility.LandedCostOption == 30)  //Post with landed cost
                    {
                        //Use the landed cost as the system maintain the stock value by landed value
                        ItmLatestCostH = GFunc.NEDec(objItm.CostLanded.Value, 0);
                        ItmLatestCostF = GFunc.RndC(ItmLatestCostH / DocCurrRate, GVar.RndDecs.Prcpt);
                    }
                    else
                    {
                        //Use the latest cost as the system maintain the stock value by latest value
                        ItmLatestCostH = GFunc.NEDec(objItm.CostLatest.Value, 0);
                        ItmLatestCostF = GFunc.RndC(ItmLatestCostH / DocCurrRate, GVar.RndDecs.Prcpt);
                    }
                }
                else
                {
                    switch (ItmTypeGrp)
                    {
                        case (int)GEnum.INTypeGrp.Stock:
                        case (int)GEnum.INTypeGrp.Non_Stock:
                        case (int)GEnum.INTypeGrp.Charges:
                            if (GFunc.CompareString(DocMode, "AR"))
                            {
                                ItmLatestCostH = GFunc.NEDec(objItm.CostLanded, 0);
                                ItmLatestCostF = GFunc.RndC(ItmLatestCostH / DocCurrRate, GVar.RndDecs.Prcpt);
                            }
                            else
                            {
                                ItmLatestCostH = GFunc.NEDec(objItm.CostLatest, 0);
                                ItmLatestCostF = GFunc.RndC(ItmLatestCostH / DocCurrRate, GVar.RndDecs.Prcpt);
                            }
                            break;

                        default:
                            ItmLatestCostH = null;
                            ItmLatestCostF = null;
                            break;
                    }
                }
                #endregion

                #region set ItmControlPriceBase, ItmControlPrice
                switch (ItmTypeGrp)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                    case (int)GEnum.INTypeGrp.Charges:
                        switch (SysOptionUtility.GetInt("SalesPriceControl",cn))
                        {
                            case 10:
                            case 15:
                                if (objItm.EStorePrice != -999 && objItm.EStorePrice != 0)
                                    ItmControlPriceBase = objItm.EStorePrice;
                                else
                                    ItmControlPriceBase = objItm.ControlPriceH;
                                break;
                            case 20:
                            case 25:
                                ItmControlPriceBase = objItm.CostLatest;
                                break;
                            case 30:
                            case 35:
                                ItmControlPriceBase = objItm.CostAvg;
                                break;
                            case 40:
                            case 45:
                                ItmControlPriceBase = objItm.CostLanded;
                                break;
                            default:
                                ItmControlPriceBase = 0;
                                break;
                        }
                        ItmControlPrice = GFunc.RndDC(ItmControlPriceBase, DocCurrRate, GVar.RndDecs.Prcpt);
                        break;

                    default:
                        ItmControlPriceBase = 0;
                        ItmControlPrice = 0;
                        break;
                }
                #endregion

                #region set ItmColorKey, ItmScaleSize, ItmPacking
                switch (ItmTypeGrp)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                        ItmColorKey = objItm.ColorKey;
                        ItmScaleSize = objItm.ScaleSize;
                        ItmPacking = objItm.INPacking;
                        break;

                    default:
                        ItmColorKey = null;
                        ItmScaleSize = string.Empty;
                        ItmPacking = string.Empty;
                        break;
                }
                #endregion

                #region set ItmWeightUOMRate
                switch (ItmTypeGrp)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                        ItmWeightUOMRate = DocComUtility.UOMGramRate_Get(cn,GFunc.NEInt(objItm.BUOMKey, 0));
                        break;

                    default:
                        ItmWeightUOMRate = 1;
                        break;
                }
                #endregion


                #endregion

                #region set DocDetail variables (ItmPriceBefore, ItmPriceAfter, Dis, PriceUser, AmtShw)
                if (RunGetItmPrice)
                {
                    DocComUtility.Price_Get(cn, DocPriceTypeKey, ItmKey, DocConKey, DocCurrKey,DocMode, ref ItmPriceBefore, ref ItmQty, ref ItmDes);

                    //to calculate UOM calculation

                    if (calType == GEnum.SpecialCalculationType.Sale)
                    {
                        if (objItm.SaleUOM != string.Empty)
                            runSpecialCalculation = true;
                    }
                    else if (calType == GEnum.SpecialCalculationType.Purchase)
                    {
                        if (objItm.PurchaseUOM != string.Empty)
                            runSpecialCalculation = true;
                    }
                    
                    if (DocMode == "AR")
                    {
                        ItmPriceList = DocComUtility.PriceByMSTItmDet_Get(cn,(int)GEnum.PriceListCode.UseStandardPrice, ItmKey, DocCurrKey);
                        //Set Vendorprice from PriceList
                        if (SysOptionUtility.GetBool("SetVendorPrice",cn) == true)

                            //--If item's Vendorprice is existing in PriceList, get from PriceList. If NOT, get vendorprice from item's Default Vendor.

                            //Get VendorPrice from PriceList
                            if (DocComUtility.PriceVendorInfo_Get(cn, DocPriceTypeKey, ItmKey, DocConKey, DocCurrKey, DocMode,ref ItmVendorKey, ref ItmVendorPrice , ref ItmRem )== false )
                            {
                                // Get VendorPrice from Item's default vendor's vendorPrice
                                // [ Vendor,Curr,CurRate,VendPrice] /*Mic Check; Jack Added 25-Oct-12*/
                                 switch ((int)GEnum.INTypeGrp.Stock)
                                    {
                                        case (int)GEnum.INTypeGrp.Stock:
                                        case (int)GEnum.INTypeGrp.Non_Stock:
                                        case (int)GEnum.INTypeGrp.Charges:
                                        case (int)GEnum.INTypeGrp.Discount:

                                            if (objItm.CSGVendorKey > 0)
                                            {
                                                MSTCon objVendor = MSTCon.Get(cn,objItm.CSGVendorKey);
                                                REFCurr objCurr = REFCurr.Get(objVendor.VCurrkey);
                                                ItmVendorKey = objItm.CSGVendorKey;
                                                ItmVendorCurrKey = objCurr.CurrKey;
                                                ItmVendorCurrRate = (decimal)DocComUtility.CurrRate_Get(objCurr.CurrKey, GFunc.GetDatePropertyValue("DocDate", objDoc), true);
                                                //Set default item's price from Vendor Price List
                                                if (SysOptionUtility.GetBool("SetVendorPrice") == true)
                                                    ItmVendorPrice = DocComUtility.Price_Get(objVendor.VPriceType, ItmKey, ItmVendorKey, ItmVendorCurrKey);
                                            }

                                            break;
                                        default:
                                    }
                                

                            }
                    }
                    else if (DocMode == "AP")
                        ItmPriceList = DocComUtility.PriceByMSTItmDet_Get(cn,(int)GEnum.PriceListCode.UseStandardCost, ItmKey, DocCurrKey);

                    ItmPriceAfter = ItmPriceBefore;
                    ItmDisPercent = DocComUtility.QtyDiscount_Get(cn, DocPriceTypeKey, ItmKey, ItmQty);
                    ItmDisValue = GFunc.RndC(ItmPriceAfter * ItmDisPercent, GVar.RndDecs.Prcpt);
                    ItmPriceUser = ItmPriceAfter - ItmDisValue;

                    switch (ItmTypeGrp)
                    {
                        case (int)GEnum.INTypeGrp.Stock:
                        case (int)GEnum.INTypeGrp.Non_Stock:
                            ItmAmtShw = GFunc.RndC(ItmQty * ItmPriceUser, GVar.RndDecs.Amtpt);
                            break;
                        case (int)GEnum.INTypeGrp.Charges:
                            ItmAmtShw = ItmPriceUser;
                            break;
                        case (int)GEnum.INTypeGrp.Discount:
                            ItmAmtShw = 0;
                            break;
                        default:
                            ItmLatestCostF = null;
                            ItmLatestCostH = null;
                            ItmPriceList = null;
                            ItmPriceBefore = null;
                            ItmPriceAfter = null;
                            ItmDisPercent = null;
                            ItmDisValue = null;
                            ItmPriceUser = null;
                            ItmAmtShw = null;
                            break;
                    }
                }
                #endregion

                #region Calculate ItmMarkUpRate from MarkupType and run Row Calculation if required
                MarkUpType = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultDocMarkupType,cn);
                minMarkupRate = GFunc.RndDC(SysOptionUtility.GetDec(GVar.SystemOption.Posting_Option.DocumentMinMarkup,cn), 100M, GVar.RndDecs.Prcpt);

                if (ItmPriceAfter == 0)
                    ItmMarkupRatio = minMarkupRate;
                else
                {
                    switch (MarkUpType)
                    {
                        //Multiply by Retail Price
                        case 10:
                            ItmMarkupRatio = GFunc.RndDC(ItmPriceAfter, ItmPriceList, GVar.RndDecs.Prcpt) - 1;
                            break;

                        //Multiply by Vendor Cost
                        case 20:
                            if (DocCurrKey == 1)
                                ItmMarkupRatio = GFunc.RndDC(ItmPriceAfter, 0M, GVar.RndDecs.Prcpt) - 1;
                            else
                            {
                                ItmMarkupRatio = GFunc.RndC(ItmPriceAfter * DocCurrRate, GVar.RndDecs.Prcpt);
                                ItmMarkupRatio = GFunc.RndDC(ItmMarkupRatio, 1, GVar.RndDecs.Prcpt) - 1;
                            }
                            break;

                        //Divided by Retail Price
                        case 30:
                            ItmMarkupRatio = 1 - GFunc.RndDC(ItmPriceList, ItmPriceAfter, GVar.RndDecs.Prcpt);
                            break;

                        //Divided by Vendor Cost
                        case 40:
                            if (DocCurrKey == 1)
                                ItmMarkupRatio = 1 - GFunc.RndDC(0M, ItmPriceAfter, GVar.RndDecs.Prcpt);
                            else
                            {
                                ItmMarkupRatio = 1 - GFunc.RndDC(ItmMarkupRatio, ItmPriceAfter * DocCurrRate, GVar.RndDecs.Prcpt);
                            }
                            break;
                    }
                }
                //Divided by Retail Price Or  Divided by Vendor Cost
                if (MarkUpType == 30 || MarkUpType == 40)
                    ItmMarkupRatio = DocComUtility.MarkUpRateDivisionLimit_Set(ItmMarkupRatio, MarkUpType, ItmPriceList, 0M, ItmPriceAfter);

                ItmMarkupRate = GFunc.RndC(ItmMarkupRatio * 100, GVar.RndDecs.Qtypt);
                #endregion

                #region set ItmMark
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
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        ItmMark = ItmMark_GetNew(cn,grdItm, (int)ItmType);
                        break;
                }
                #endregion

                #region set values in document detail
                switch (objDoc.DocCodeKey)
                {
                    #region ARQO
                    case (int)GEnum.SystemCode.Quotation:
                        grdRow.Cells["ItmKey"].Value = ItmKey;
                        grdRow.Cells["ItmKeySelect"].Value = ItmKeySelected;
                        grdRow.Cells["ItmID"].Value = ItmID;
                        grdRow.Cells["HSCode"].Value = IndustryPN;
                        grdRow.Cells["CountryID"].Value = CountryID;
                        grdRow.Cells["ItmType"].Value = ItmType;
                        grdRow.Cells["ItmDes"].Value = ItmDes;
                        grdRow.Cells["ItmDeptKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmDeptKey"].Value, 0)); //Ask Mic
                        grdRow.Cells["ItmTranGrpKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmTranGrpKey"].Value, 0)); //Ask Mic
                        switch (ItmTypeGrp)
                        {
                            case (int)GEnum.INTypeGrp.Total:
                                break;

                            default:
                                grdRow.Cells["ItmAccKey"].Value = ItmAccKey.ToDBValue(); //Ask Mic //Ask Mic

                                MSTAcc objMSTAcc = MSTAcc.Get(cn,ItmAccKey);
                                if (!GFunc.IsNEZ(objMSTAcc.AccKey))
                                {
                                    grdRow.Cells["ItmAccID"].Value = objMSTAcc.AccID;
                                    grdRow.Cells["ItmAccDes"].Value = objMSTAcc.AccDes;
                                }
                                break;
                        }

                        grdRow.Cells["ItmLocKey"].Value = GetItmInfor_Loc(DocMode, objItm, GFunc.NEInt(grdRow.Cells["ItmLocKey"].Value, 0)).ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmStock"].Value = ItmStock.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQty"].Value = ItmQty.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmReqDate"].Value = GFunc.NEStr(grdRow.Cells["ItmReqDate"].Value, objDoc.DocDate);
                        grdRow.Cells["ItmPrmDate"].Value = GFunc.NEStr(grdRow.Cells["ItmPrmDate"].Value, objDoc.DocDate);
                        grdRow.Cells["ItmUOMKey"].Value = ItmUOMKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmConRate"].Value = ItmConRate.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLatestCostF"].Value = ItmLatestCostF.ToDBValue(); //Ask Mic
                        grdRow.Cells["ObCost"].Value = GFunc.NEDec(objItm.ObCost,0);
                        grdRow.Cells["ItmLatestCostH"].Value = ItmLatestCostH.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmListPrice"].Value = ItmPriceList.ToDBValue();
                        grdRow.Cells["ItmPriceBefore"].Value = ItmPriceBefore.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPriceAfter"].Value = ItmPriceAfter.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisPercent"].Value = ItmDisPercent.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPrice"].Value = 0;
                        grdRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmControlPrice"].Value = ItmControlPrice.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmControlPriceBase"].Value = ItmControlPriceBase.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtF"].Value = 0;
                        grdRow.Cells["ItmAmtH"].Value = 0;
                        grdRow.Cells["ItmTaxable"].Value = objItm.Taxable;
                        grdRow.Cells["ItmTaxGrpKey"].Value = GetItmInfor_TaxGrpKey(objItm, (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc)).ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmTaxGrpRate"].Value = GetItmInfor_TaxGrpRate(objItm, (decimal?)GFunc.GetPropertyValue("DocTaxGrpRate", objDoc));
                        grdRow.Cells["ItmTaxGrpAmtF"].Value = 0;
                        grdRow.Cells["ItmTaxGrpAmtL"].Value = 0;
                        grdRow.Cells["ItmColorKey"].Value = ItmColorKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmScaleSize"].Value = ItmScaleSize;
                        grdRow.Cells["ItmPacking"].Value = ItmPacking;
                        grdRow.Cells["ItmMark"].Value = ItmMark;
                        grdRow.Cells["ItmRem"].Value = ItmRem ; // Check Mic                       
                        grdRow.Cells["ItmVendorKey"].Value = ItmVendorKey.ToDBValue(); //previous value DBNull.Value; Check, Mic 25 Oct 2012; JackChanged
                        grdRow.Cells["ItmVendorCurrKey"].Value = ItmVendorCurrKey; //previous value 1; Check, Mic 25 Oct 2012; JackChanged
                        grdRow.Cells["ItmVendorCurrRate"].Value = ItmVendorCurrRate;//previous value 1; Check, Mic 25 Oct 2012; JackChanged
                        grdRow.Cells["ItmVendorPrice"].Value = ItmVendorPrice;//previous value 0; Check, Mic 25 Oct 2012; JackChanged 
                        grdRow.Cells["ItmVendorPriceRatio"].Value = ItmMarkupRate;
                        grdRow.Cells["ItmJobKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));//Ask Mic
                        grdRow.Cells["ItmJobPhaseKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobPhaseKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));//Ask Mic
                        grdRow.Cells["ItmJobTaskKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobTaskKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));//Ask Mic
                        grdRow.Cells["ItmJobCostTypeKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobCostTypeKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));//Ask Mic
                        grdRow.Cells["ItmIGrpDItm"].Value = 0;
                        grdRow.Cells["ItmIGrpQtyLock"].Value = false;
                        grdRow.Cells["ItmIGrpToPrint"].Value = true;
                        grdRow.Cells["ItmIGrpQtySet"].Value = 0;
                        grdRow.Cells["ItmIGrpAmtSet"].Value = 0;
                        break;
                    #endregion

                    #region ARSO
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                        grdRow.Cells["ItmKey"].Value = ItmKey;
                        grdRow.Cells["ItmKeySelect"].Value = ItmKeySelected;
                        grdRow.Cells["ItmID"].Value = ItmID;
                        grdRow.Cells["HSCode"].Value = IndustryPN;
                        grdRow.Cells["CountryID"].Value = CountryID;
                        grdRow.Cells["ItmType"].Value = ItmType;
                        grdRow.Cells["ItmDes"].Value = ItmDes;
                        grdRow.Cells["ItmDeptKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmDeptKey"].Value, 0));
                        grdRow.Cells["ItmTranGrpKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmTranGrpKey"].Value, 0));
                        switch (ItmTypeGrp)
                        {
                            case (int)GEnum.INTypeGrp.Total:
                                break;
                            default:
                                grdRow.Cells["ItmAccKey"].Value = ItmAccKey.ToDBValue(); //Ask Mic

                                MSTAcc objMSTAcc = MSTAcc.Get(ItmAccKey);
                                if (!GFunc.IsNEZ(objMSTAcc.AccKey))
                                {
                                    grdRow.Cells["ItmAccID"].Value = objMSTAcc.AccID;
                                    grdRow.Cells["ItmAccDes"].Value = objMSTAcc.AccDes;
                                }
                                break;
                        }
                        grdRow.Cells["ItmLocKey"].Value = GetItmInfor_Loc(DocMode, objItm, GFunc.NEInt(grdRow.Cells["ItmLocKey"].Value, 0)).ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmStock"].Value = ItmStock.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQty"].Value = ItmQty.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQtyLink"].Value = ItmQtyLink.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQtyAdj"].Value = ItmQtyAdj.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQtyBalance"].Value = ItmQty.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmOrderStatus"].Value = ItmOrderStatus;
                        DateTime? reqDate = GFunc.GetDatePropertyValue("DocReqDate", objDoc);
                        if (reqDate != null && reqDate.HasValue)
                        {
                            grdRow.Cells["ItmReqDate"].Value = reqDate.Value;
                            grdRow.Cells["ItmPrmDate"].Value = reqDate.Value; /* added by YST on 2023/05/30 */                            
                        }

                        /* ItmPrmDate also should follow DocReqDate like ItmReqDate because all reports & lists show ItmPrmDate only. 
                         * requested by Feliani, commented by YST , discussed and comfirmed by May on 2023/05/30 */
                        //DateTime? prmDate = GFunc.GetDatePropertyValue("DocPrmDate", objDoc);
                        //if (prmDate != null && prmDate.HasValue)
                        //{
                        //    grdRow.Cells["ItmPrmDate"].Value = prmDate.Value;
                        //}
                        grdRow.Cells["ItmUOMKey"].Value = ItmUOMKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmConRate"].Value = ItmConRate.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLatestCostF"].Value = ItmLatestCostF.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLatestCostH"].Value = ItmLatestCostH.ToDBValue(); //Ask Mic
                        grdRow.Cells["ObCost"].Value = GFunc.NEDec(objItm.ObCost, 0);
                        grdRow.Cells["ItmListPrice"].Value = ItmPriceList.ToDBValue();
                        grdRow.Cells["ItmPriceBefore"].Value = ItmPriceBefore.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPriceAfter"].Value = ItmPriceAfter.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisPercent"].Value = ItmDisPercent.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPrice"].Value = 0;
                        grdRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmControlPrice"].Value = ItmControlPrice.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmControlPriceBase"].Value = ItmControlPriceBase.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtF"].Value = 0;
                        grdRow.Cells["ItmAmtH"].Value = 0;
                        grdRow.Cells["ItmTaxable"].Value = objItm.Taxable;
                        grdRow.Cells["ItmTaxGrpKey"].Value = GetItmInfor_TaxGrpKey(objItm, (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc)).ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmTaxGrpRate"].Value = GetItmInfor_TaxGrpRate(objItm, (decimal?)GFunc.GetPropertyValue("DocTaxGrpRate", objDoc));
                        grdRow.Cells["ItmTaxGrpAmtF"].Value = 0;
                        grdRow.Cells["ItmTaxGrpAmtL"].Value = 0;
                        grdRow.Cells["ItmColorKey"].Value = ItmColorKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmScaleSize"].Value = ItmScaleSize;
                        grdRow.Cells["ItmPacking"].Value = ItmPacking;
                        grdRow.Cells["ItmMark"].Value = ItmMark;
                        grdRow.Cells["ItmRem"].Value = ItmRem; // Check Mic                   
                        grdRow.Cells["ItmVendorKey"].Value = ItmVendorKey; //previous value DBNull.Value; Check, Mic 25 Oct 2012; JackChanged
                        grdRow.Cells["ItmVendorCurrKey"].Value = ItmVendorCurrKey; //previous value 1; Check, Mic 25 Oct 2012; JackChanged
                        grdRow.Cells["ItmVendorCurrRate"].Value = ItmVendorCurrRate;//previous value 1; Check, Mic 25 Oct 2012; JackChanged
                        grdRow.Cells["ItmVendorPrice"].Value = ItmVendorPrice;//previous value 0; Check, Mic 25 Oct 2012; JackChanged
                        grdRow.Cells["ItmVendorPriceRatio"].Value = 0;
                        grdRow.Cells["ItmJobKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));//Ask Mic
                        grdRow.Cells["ItmJobPhaseKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobPhaseKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));//Ask Mic
                        grdRow.Cells["ItmJobTaskKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobTaskKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));//Ask Mic
                        grdRow.Cells["ItmJobCostTypeKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobCostTypeKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));//Ask Mic
                        grdRow.Cells["ItmIGrpDItm"].Value = 0;
                        grdRow.Cells["ItmIGrpQtyLock"].Value = false;
                        grdRow.Cells["ItmIGrpToPrint"].Value = true;
                        grdRow.Cells["ItmIGrpQtySet"].Value = 0;
                        grdRow.Cells["ItmIGrpAmtSet"].Value = 0;
                        grdRow.Cells["ARQOID"].Value = null;
                        grdRow.Cells["ARQODK"].Value = 0;
                        grdRow.Cells["ARQODItm"].Value = 0;

                        /* added by YST to capture QARemark or Defect Report ID in SO , RO doesn't have ItmRef */
                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Order)
                        {
                            grdRow.Cells["ItmRef"].Value = ItmRef;
                            if (objItm.SalesWrtyYr > 0 && reqDate != null)
                            {
                                double salesWrtyDays = 0.0;
                                salesWrtyDays = Convert.ToDouble(objItm.SalesWrtyYr * 365);
                                grdRow.Cells["ItmWrtyEndDate"].Value = ((DateTime)reqDate).AddDays(salesWrtyDays);
                            }

                        }

                        /* added by YST to put auto-check Hide column for the remark regaring reverse GST and Customer PO print by Logistic */
                        if (ItmID == SpecialRemark.GSTReverse ||
                            ItmID == SpecialRemark.GSTReverseRemark ||
                            ItmID == SpecialRemark.CustomerPO)
                        {
                            grdRow.Cells["ItmHide"].Value = true;
                        }
                        else
                        {
                            grdRow.Cells["ItmHide"].Value = false;
                        }
                        
                        break;
                    #endregion

                    #region ARDO
                    case (int)GEnum.SystemCode.Delivery_Order:
                        grdRow.Cells["ItmKey"].Value = ItmKey;
                        grdRow.Cells["ItmKeySelect"].Value = ItmKeySelected;
                        grdRow.Cells["ItmID"].Value = ItmID;
                        grdRow.Cells["HSCode"].Value = IndustryPN;
                        grdRow.Cells["CountryID"].Value = CountryID;
                        grdRow.Cells["ItmType"].Value = objItm.ItmType;
                        grdRow.Cells["ItmDes"].Value = ItmDes;
                        grdRow.Cells["ItmDeptKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmDeptKey"].Value, 0));
                        grdRow.Cells["ItmTranGrpKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmTranGrpKey"].Value, 0));
                        switch (ItmTypeGrp)
                        {
                            case (int)GEnum.INTypeGrp.Total:
                                break;
                            default:

                                grdRow.Cells["ItmAccKey"].Value = ItmAccKey.ToDBValue(); //Ask Mic

                                MSTAcc objMSTAcc = MSTAcc.Get(ItmAccKey);
                                if (!GFunc.IsNEZ(objMSTAcc.AccKey))
                                {
                                    grdRow.Cells["ItmAccID"].Value = objMSTAcc.AccID;
                                    grdRow.Cells["ItmAccDes"].Value = objMSTAcc.AccDes;
                                }
                                break;
                        }
                        grdRow.Cells["ItmLocKey"].Value = GetItmInfor_Loc(DocMode, objItm, GFunc.NEInt(grdRow.Cells["ItmLocKey"].Value, 0)).ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmStock"].Value = ItmStock.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQty"].Value = ItmQty.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmUOMKey"].Value = ItmUOMKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmConRate"].Value = ItmConRate.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLatestCostF"].Value = ItmLatestCostF.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLatestCostH"].Value = ItmLatestCostH.ToDBValue(); //Ask Mic
                        grdRow.Cells["ObCost"].Value = GFunc.NEDec(objItm.ObCost, 0);
                        grdRow.Cells["ItmListPrice"].Value = ItmPriceList.ToDBValue();
                        grdRow.Cells["ItmPriceBefore"].Value = ItmPriceBefore.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPriceAfter"].Value = ItmPriceAfter.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisPercent"].Value = ItmDisPercent.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPrice"].Value = 0;
                        grdRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmControlPrice"].Value = ItmControlPrice.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmControlPriceBase"].Value = ItmControlPriceBase.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtF"].Value = 0;
                        grdRow.Cells["ItmAmtH"].Value = 0;
                        grdRow.Cells["ItmTaxable"].Value = objItm.Taxable;
                        grdRow.Cells["ItmTaxGrpKey"].Value = GetItmInfor_TaxGrpKey(objItm, (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc)).ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmTaxGrpRate"].Value = GetItmInfor_TaxGrpRate(objItm, (decimal?)GFunc.GetPropertyValue("DocTaxGrpRate", objDoc));
                        grdRow.Cells["ItmTaxGrpAmtF"].Value = 0;
                        grdRow.Cells["ItmTaxGrpAmtL"].Value = 0;
                        grdRow.Cells["ItmColorKey"].Value = ItmColorKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmScaleSize"].Value = ItmScaleSize;
                        grdRow.Cells["ItmPacking"].Value = ItmPacking;
                        grdRow.Cells["ItmMark"].Value = ItmMark;
                        grdRow.Cells["ItmRem"].Value = ItmRem; // Check Mic                       
                        grdRow.Cells["ItmVendorKey"].Value = ItmVendorKey; //previous value DBNull.Value; Check, Mic 25 Oct 2012; JackChanged
                        grdRow.Cells["ItmVendorCurrKey"].Value = ItmVendorCurrKey; //previous value 1; Check, Mic 25 Oct 2012; JackChanged
                        grdRow.Cells["ItmVendorCurrRate"].Value = ItmVendorCurrRate;//previous value 1; Check, Mic 25 Oct 2012; JackChanged
                        grdRow.Cells["ItmVendorPrice"].Value = ItmVendorPrice;//previous value 0; Check, Mic 25 Oct 2012; JackChanged
                        grdRow.Cells["ItmVendorPriceRatio"].Value = 0;
                        grdRow.Cells["ItmJobKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));//Ask Mic
                        grdRow.Cells["ItmJobPhaseKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobPhaseKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));//Ask Mic
                        grdRow.Cells["ItmJobTaskKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobTaskKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));//Ask Mic
                        grdRow.Cells["ItmJobCostTypeKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobCostTypeKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));//Ask Mic
                        grdRow.Cells["ItmIGrpDItm"].Value = 0;
                        grdRow.Cells["ItmIGrpQtyLock"].Value = false;
                        grdRow.Cells["ItmIGrpToPrint"].Value = true;
                        grdRow.Cells["ItmIGrpQtySet"].Value = 0;
                        grdRow.Cells["ItmIGrpAmtSet"].Value = 0;
                        grdRow.Cells["ARQOID"].Value = null;
                        grdRow.Cells["ARQODK"].Value = 0;
                        grdRow.Cells["ARQODItm"].Value = 0;
                        grdRow.Cells["ARSOID"].Value = null;
                        grdRow.Cells["ARSODK"].Value = 0;
                        grdRow.Cells["ARSODItm"].Value = 0;
                        grdRow.Cells["ARSOPOID"].Value = null;
                        /* added by YST */
                        //grdRow.Cells["ItmRef"].Value = ItmRef; 
                        //DateTime? DocDate = GFunc.GetDatePropertyValue("DocDate", objDoc);
                        //if (objItm.SalesWrtyYr > 0 && DocDate != null)
                        //{
                        //    double salesWrtyDays = 0.0;
                        //    salesWrtyDays = Convert.ToDouble(objItm.SalesWrtyYr * 365);
                        //    grdRow.Cells["ItmWrtyEndDate"].Value = ((DateTime)DocDate).AddDays(salesWrtyDays);
                        //}
                        /* end by YST */
                        break;
                    #endregion

                    #region ARIV, DN, CN
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                        grdRow.Cells["ItmKey"].Value = ItmKey;
                        grdRow.Cells["ItmKeySelect"].Value = ItmKeySelected;
                        grdRow.Cells["ItmID"].Value = ItmID;
                        grdRow.Cells["ItmType"].Value = ItmType;
                        grdRow.Cells["ItmDes"].Value = ItmDes;
                        grdRow.Cells["ItmDeptKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmDeptKey"].Value, 0));
                        grdRow.Cells["ItmTranGrpKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmTranGrpKey"].Value, 0));
                        switch (ItmTypeGrp)
                        {
                            case (int)GEnum.INTypeGrp.Total:
                                break;
                            default:
                                grdRow.Cells["ItmAccKey"].Value = ItmAccKey.ToDBValue(); //Ask Mic
                                MSTAcc objMSTAcc = MSTAcc.Get(ItmAccKey);
                                if (!GFunc.IsNEZ(objMSTAcc.AccKey))
                                {
                                    grdRow.Cells["ItmAccID"].Value = objMSTAcc.AccID;
                                    grdRow.Cells["ItmAccDes"].Value = objMSTAcc.AccDes;
                                }
                                break;
                        }
                        grdRow.Cells["ItmLocKey"].Value = GetItmInfor_Loc(DocMode, objItm, GFunc.NEInt(grdRow.Cells["ItmLocKey"].Value, 0)).ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmStock"].Value = ItmStock.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQty"].Value = ItmQty.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmUOMKey"].Value = ItmUOMKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmConRate"].Value = ItmConRate.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLatestCostF"].Value = ItmLatestCostF.ToDBValue(); //Ask Mic
                        grdRow.Cells["ObCost"].Value = GFunc.NEDec(objItm.ObCost, 0);
                        grdRow.Cells["ItmLatestCostH"].Value = ItmLatestCostH.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmListPrice"].Value = ItmPriceList.ToDBValue();
                        grdRow.Cells["ItmPriceBefore"].Value = ItmPriceBefore.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPriceAfter"].Value = ItmPriceAfter.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisPercent"].Value = ItmDisPercent.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPrice"].Value = 0;
                        grdRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmControlPrice"].Value = ItmControlPrice.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmControlPriceBase"].Value = ItmControlPriceBase.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtF"].Value = 0;
                        grdRow.Cells["ItmAmtH"].Value = 0;
                        grdRow.Cells["ItmTaxable"].Value = objItm.Taxable;
                        grdRow.Cells["ItmTaxGrpKey"].Value = GetItmInfor_TaxGrpKey(objItm, (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc)).ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmTaxGrpRate"].Value = GetItmInfor_TaxGrpRate(objItm, (decimal?)GFunc.GetPropertyValue("DocTaxGrpRate", objDoc));
                        grdRow.Cells["ItmTaxGrpAmtF"].Value = 0;
                        grdRow.Cells["ItmTaxGrpAmtL"].Value = 0;
                        grdRow.Cells["ItmColorKey"].Value = ItmColorKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmScaleSize"].Value = ItmScaleSize;
                        grdRow.Cells["ItmPacking"].Value = ItmPacking;
                        grdRow.Cells["ItmMark"].Value = ItmMark;
                        grdRow.Cells["ItmRem"].Value = ItmRem; // Check Mic                        
                        grdRow.Cells["ItmVendorKey"].Value = ItmVendorKey; //previous value DBNull.Value; Check, Mic 25 Oct 2012; JackChanged
                        grdRow.Cells["ItmVendorCurrKey"].Value = ItmVendorCurrKey; //previous value 1; Check, Mic 25 Oct 2012; JackChanged
                        grdRow.Cells["ItmVendorCurrRate"].Value = ItmVendorCurrRate;//previous value 1; Check, Mic 25 Oct 2012; JackChanged
                        grdRow.Cells["ItmVendorPrice"].Value = ItmVendorPrice;//previous value 0; Check, Mic 25 Oct 2012; JackChanged
                        grdRow.Cells["ItmVendorPriceRatio"].Value = 0;
                        grdRow.Cells["ItmJobKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));//Ask Mic
                        grdRow.Cells["ItmJobPhaseKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobPhaseKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));//Ask Mic
                        grdRow.Cells["ItmJobTaskKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobTaskKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));//Ask Mic
                        grdRow.Cells["ItmJobCostTypeKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobCostTypeKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));//Ask Mic
                        grdRow.Cells["ItmIGrpDItm"].Value = 0;
                        grdRow.Cells["ItmIGrpQtyLock"].Value = false;
                        grdRow.Cells["ItmIGrpToPrint"].Value = true;
                        grdRow.Cells["ItmIGrpQtySet"].Value = 0;
                        grdRow.Cells["ItmIGrpAmtSet"].Value = 0;
                        grdRow.Cells["ARQOID"].Value = null;
                        grdRow.Cells["ARQODK"].Value = 0;
                        grdRow.Cells["ARQODItm"].Value = 0;
                        grdRow.Cells["ARSOID"].Value = null;
                        grdRow.Cells["ARSODK"].Value = 0;
                        grdRow.Cells["ARSODItm"].Value = 0;
                        grdRow.Cells["ARSOPOID"].Value = null;
                        grdRow.Cells["ARDOID"].Value = null;
                        grdRow.Cells["ARDODK"].Value = 0;
                        grdRow.Cells["ARDODItm"].Value = 0;
                        grdRow.Cells["CSCPSID"].Value = null;
                        grdRow.Cells["CSCPSDK"].Value = 0;
                        grdRow.Cells["CSCPSDItm"].Value = 0;
                        grdRow.Cells["CSCSIID"].Value = null;
                        grdRow.Cells["CSCSIDK"].Value = 0;
                        grdRow.Cells["CSCSIDItm"].Value = 0;
                        grdRow.Cells["ItmRef"].Value = ItmRef; // added by YST
                        grdRow.Cells["ItmWrtyEndDate"].Value = null; // added by YST
                        break;
                    #endregion

                    #region ARPL
                    case (int)GEnum.SystemCode.Packing_List:
                        grdRow.Cells["DetItmKey"].Value = ItmKey;
                        grdRow.Cells["DetItmKeySelect"].Value = ItmKeySelected;
                        grdRow.Cells["DetItmID"].Value = ItmID;
                        grdRow.Cells["DetItmType"].Value = ItmType;
                        grdRow.Cells["DetItmDes"].Value = ItmDes;
                        grdRow.Cells["DetItmDeptKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["DetItmDeptKey"].Value, 0));
                        grdRow.Cells["DetItmBatchID"].Value = null;
                        grdRow.Cells["DetItmPacking"].Value = ItmPacking;
                        grdRow.Cells["DetItmQtyPerPack"].Value = 0;
                        grdRow.Cells["DetItmQtyTotal"].Value = 0;
                        grdRow.Cells["DetItmUOMKey"].Value = ItmUOMKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["DetItmConRate"].Value = GFunc.NEDec(ItmConRate, 1);
                        grdRow.Cells["DetItmWeightNet"].Value = GFunc.NEDec(objItm.WeightNet, 0);
                        grdRow.Cells["DetItmWeightGross"].Value = GFunc.NEDec(objItm.WeightGross, 0);
                        grdRow.Cells["DetItmWeightUOMKey"].Value = objItm.WeightUOMKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["DetItmWeightUOMRate"].Value = GFunc.NEDec(ItmWeightUOMRate, 1);
                        grdRow.Cells["DetItmWeightBaseNet"].Value = GFunc.NEDec(objItm.WeightNet * GFunc.RndDC(ItmWeightUOMRate, (decimal?)GFunc.GetPropertyValue("DocWeightUOMRate", objDoc), GVar.RndDecs.Prcpt), 0);
                        grdRow.Cells["DetItmWeightBaseGross"].Value = GFunc.NEDec(objItm.WeightGross * GFunc.RndDC(ItmWeightUOMRate, (decimal?)GFunc.GetPropertyValue("DocWeightUOMRate", objDoc), GVar.RndDecs.Prcpt), 0);
                        grdRow.Cells["DetItmHide"].Value = false;
                        grdRow.Cells["DetItmDocID"].Value = null;
                        grdRow.Cells["DetItmMarking"].Value = null;
                        grdRow.Cells["DetItmColorKey"].Value = ItmColorKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["DetItmScaleSize"].Value = ItmScaleSize;
                        grdRow.Cells["DetItmRem"].Value = null;
                        break;
                    #endregion

                    #region APPN
                    case (int)GEnum.SystemCode.Purchase_Plan:
                        grdRow.Cells["ItmKey"].Value = ItmKey;
                        grdRow.Cells["ItmKeySelect"].Value = ItmKeySelected;
                        grdRow.Cells["ItmID"].Value = ItmID;
                        grdRow.Cells["ItmDes"].Value = ItmDes;
                        grdRow.Cells["ItmDeptKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmDeptKey"].Value, 0));//Ask Mic
                        grdRow.Cells["ItmTranGrpKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmTranGrpKey"].Value, 0));
                        grdRow.Cells["ItmQtyM1"].Value = 0;
                        grdRow.Cells["ItmQtyM2"].Value = 0;
                        grdRow.Cells["ItmQtyM3"].Value = 0;
                        grdRow.Cells["ItmQtyM4"].Value = 0;
                        grdRow.Cells["ItmQtyM5"].Value = 0;
                        grdRow.Cells["ItmQtyM6"].Value = 0;
                        grdRow.Cells["ItmQtyM7"].Value = 0;
                        grdRow.Cells["ItmQtyM8"].Value = 0;
                        grdRow.Cells["ItmQtyM9"].Value = 0;
                        grdRow.Cells["ItmQtyM10"].Value = 0;
                        grdRow.Cells["ItmQtyM11"].Value = 0;
                        grdRow.Cells["ItmQtyM12"].Value = 0;
                        grdRow.Cells["ItmQtyMTotal"].Value = 0;
                        break;
                    #endregion

                    #region APRQ
                    case (int)GEnum.SystemCode.Purchase_Request:
                        grdRow.Cells["ItmKey"].Value = ItmKey;
                        grdRow.Cells["ItmKeySelect"].Value = ItmKeySelected;
                        grdRow.Cells["ItmID"].Value = ItmID;
                        grdRow.Cells["ItmType"].Value = ItmType;
                        grdRow.Cells["ItmDes"].Value = ItmDes;
                        grdRow.Cells["ItmDeptKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmDeptKey"].Value, 0)); //Ask Mic
                        grdRow.Cells["ItmTranGrpKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmTranGrpKey"].Value, 0)); //Ask Mic
                        grdRow.Cells["ItmQty"].Value = ItmQty.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmUOMKey"].Value = ItmUOMKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmConRate"].Value = ItmConRate.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmColorKey"].Value = ItmColorKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmScaleSize"].Value = ItmScaleSize;
                        grdRow.Cells["ItmPacking"].Value = ItmPacking;
                        grdRow.Cells["ItmMark"].Value = ItmMark;
                        break;
                    #endregion

                    #region APPO
                    case (int)GEnum.SystemCode.Purchase_Order:
                        grdRow.Cells["ItmKey"].Value = ItmKey;
                        grdRow.Cells["ItmKeySelect"].Value = ItmKeySelected;
                        grdRow.Cells["ItmID"].Value = ItmID;
                        grdRow.Cells["HSCode"].Value = IndustryPN;
                        grdRow.Cells["CountryID"].Value = CountryID;
                        grdRow.Cells["ItmType"].Value = ItmType;
                        grdRow.Cells["ItmDes"].Value = ItmDes;
                        grdRow.Cells["ItmDeptKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmDeptKey"].Value, 0));
                        grdRow.Cells["ItmTranGrpKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmTranGrpKey"].Value, 0));
                        switch (ItmTypeGrp)
                        {
                            case (int)GEnum.INTypeGrp.Total:
                                break;
                            default:
                                grdRow.Cells["ItmAccKey"].Value = ItmAccKey.ToDBValue(); //Ask Mic

                                MSTAcc objMSTAcc = MSTAcc.Get(cn,ItmAccKey);
                                if (!GFunc.IsNEZ(objMSTAcc.AccKey))
                                {
                                    grdRow.Cells["ItmAccID"].Value = objMSTAcc.AccID;
                                    grdRow.Cells["ItmAccDes"].Value = objMSTAcc.AccDes;
                                }
                                break;
                        }
                        grdRow.Cells["ItmLocKey"].Value = GetItmInfor_Loc(DocMode, objItm, GFunc.NEInt(grdRow.Cells["ItmLocKey"].Value, 0)).ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmStock"].Value = ItmStock.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQty"].Value = ItmQty.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQtyLink"].Value = ItmQtyLink.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQtyAdj"].Value = ItmQtyAdj.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmOrderStatus"].Value = ItmOrderStatus;
                        DateTime? reqDateTmp = GFunc.GetDatePropertyValue("DocReqDate", objDoc);
                        if (reqDateTmp != null && reqDateTmp.HasValue)
                        {
                            grdRow.Cells["ItmReqDate"].Value = reqDateTmp.Value;
                        }

                        DateTime? prmDateTmp = GFunc.GetDatePropertyValue("DocPrmDate", objDoc);
                        if (prmDateTmp != null && prmDateTmp.HasValue)
                        {
                            grdRow.Cells["ItmPrmDate"].Value = prmDateTmp.Value;
                        }
                        grdRow.Cells["ItmUOMKey"].Value = ItmUOMKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmConRate"].Value = ItmConRate.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLatestCostF"].Value = ItmLatestCostF.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLatestCostH"].Value = ItmLatestCostH.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmListPrice"].Value = ItmPriceList.ToDBValue();
                        grdRow.Cells["ItmPriceBefore"].Value = ItmPriceBefore.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPriceAfter"].Value = ItmPriceAfter.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisPercent"].Value = ItmDisPercent.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPrice"].Value = 0;
                        grdRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtF"].Value = 0;
                        grdRow.Cells["ItmAmtH"].Value = 0;
                        grdRow.Cells["ItmTaxable"].Value = objItm.Taxable;
                        grdRow.Cells["ItmTaxGrpKey"].Value = GetItmInfor_TaxGrpKey(objItm, (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc)).ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmTaxGrpRate"].Value = GetItmInfor_TaxGrpRate(objItm, (decimal?)GFunc.GetPropertyValue("DocTaxGrpRate", objDoc));
                        grdRow.Cells["ItmTaxGrpAmtF"].Value = 0;
                        grdRow.Cells["ItmTaxGrpAmtL"].Value = 0;
                        grdRow.Cells["ItmColorKey"].Value = ItmColorKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmScaleSize"].Value = ItmScaleSize;
                        grdRow.Cells["ItmPacking"].Value = ItmPacking;
                        grdRow.Cells["ItmMark"].Value = ItmMark;
                        grdRow.Cells["ItmJobKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobPhaseKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobPhaseKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobTaskKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobTaskKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobCostTypeKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobCostTypeKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ARQOID"].Value = null;
                        grdRow.Cells["ARQODK"].Value = 0;
                        grdRow.Cells["ARQODItm"].Value = 0;
                        grdRow.Cells["ARSOID"].Value = null;
                        grdRow.Cells["ARSODK"].Value = 0;
                        grdRow.Cells["ARSODItm"].Value = 0;
                        grdRow.Cells["ARSOPOID"].Value = null;
                        grdRow.Cells["ARDOID"].Value = null;
                        grdRow.Cells["ARDODK"].Value = 0;
                        grdRow.Cells["ARDODItm"].Value = 0;
                        grdRow.Cells["ARIVID"].Value = null;
                        grdRow.Cells["ARIVDK"].Value = 0;
                        grdRow.Cells["ARIVDItm"].Value = 0;
                        
                        //Added by May on 08-Mar-2023
                        if (SysOptionUtility.DatabaseBranchCode.Equals("BHM"))
                            grdRow.Cells["ObCost"].Value = objItm.ObCost;
                        break;

                    #endregion

                    #region APPS, APPD
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                        grdRow.Cells["ItmKey"].Value = ItmKey;
                        grdRow.Cells["ItmKeySelect"].Value = ItmKeySelected;
                        grdRow.Cells["ItmID"].Value = ItmID;
                        grdRow.Cells["HSCode"].Value = IndustryPN;
                        grdRow.Cells["CountryID"].Value = CountryID;
                        grdRow.Cells["ItmType"].Value = ItmType;
                        grdRow.Cells["ItmDes"].Value = ItmDes;
                        grdRow.Cells["ItmDeptKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmDeptKey"].Value, 0));
                        grdRow.Cells["ItmTranGrpKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmTranGrpKey"].Value, 0));
                        switch (ItmTypeGrp)
                        {
                            case (int)GEnum.INTypeGrp.Total:
                                break;
                            default:
                                grdRow.Cells["ItmAccKey"].Value = ItmAccKey.ToDBValue(); //Ask Mic

                                MSTAcc objMSTAcc = MSTAcc.Get(ItmAccKey);
                                if (!GFunc.IsNEZ(objMSTAcc.AccKey))
                                {
                                    grdRow.Cells["ItmAccID"].Value = objMSTAcc.AccID;
                                    grdRow.Cells["ItmAccDes"].Value = objMSTAcc.AccDes;
                                }
                                break;
                        }
                        grdRow.Cells["ItmLocKey"].Value = GetItmInfor_Loc(DocMode, objItm, GFunc.NEInt(grdRow.Cells["ItmLocKey"].Value, 0)).ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmStock"].Value = ItmStock.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQty"].Value = ItmQty.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmUOMKey"].Value = ItmUOMKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmConRate"].Value = ItmConRate.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLatestCostF"].Value = ItmLatestCostF.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLatestCostH"].Value = ItmLatestCostH.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmListPrice"].Value = ItmPriceList.ToDBValue();
                        grdRow.Cells["ItmPriceBefore"].Value = ItmPriceBefore.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPriceAfter"].Value = ItmPriceAfter.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisPercent"].Value = ItmDisPercent.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPrice"].Value = 0;
                        grdRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtF"].Value = 0;
                        grdRow.Cells["ItmAmtH"].Value = 0;
                        grdRow.Cells["ItmTaxable"].Value = objItm.Taxable;
                        //row.Cells["ItmTaxGrpKey"].Value = GetItmInfor_TaxGrpKey(objItm, (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc)).ToDBValue(); //Ask Mic //not used
                        //row.Cells["ItmTaxGrpRate"].Value = GetItmInfor_TaxGrpRate(objItm, (decimal?)GFunc.GetPropertyValue("DocTaxGrpRate", objDoc));//not used
                        grdRow.Cells["ItmTaxGrpAmtF"].Value = 0;
                        grdRow.Cells["ItmTaxGrpAmtL"].Value = 0;
                        grdRow.Cells["ItmColorKey"].Value = ItmColorKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmScaleSize"].Value = ItmScaleSize;
                        grdRow.Cells["ItmPacking"].Value = ItmPacking;
                        grdRow.Cells["ItmMark"].Value = ItmMark;
                        grdRow.Cells["ItmJobKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobPhaseKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobPhaseKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobTaskKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobTaskKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobCostTypeKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobCostTypeKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ARQOID"].Value = null;
                        grdRow.Cells["ARQODK"].Value = 0;
                        grdRow.Cells["ARQODItm"].Value = 0;
                        grdRow.Cells["ARSOID"].Value = null;
                        grdRow.Cells["ARSODK"].Value = 0;
                        grdRow.Cells["ARSODItm"].Value = 0;
                        grdRow.Cells["ARSOPOID"].Value = null;
                        grdRow.Cells["ARDOID"].Value = null;
                        grdRow.Cells["ARDODK"].Value = 0;
                        grdRow.Cells["ARDODItm"].Value = 0;
                        grdRow.Cells["ARIVID"].Value = null;
                        grdRow.Cells["ARIVDK"].Value = 0;
                        grdRow.Cells["ARIVDItm"].Value = 0;
                        grdRow.Cells["APPOID"].Value = null;
                        grdRow.Cells["APPODK"].Value = 0;
                        grdRow.Cells["APPODItm"].Value = 0;
                        break;
                    #endregion

                    #region APBL, DN, CN
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        grdRow.Cells["ItmKey"].Value = ItmKey;
                        grdRow.Cells["ItmKeySelect"].Value = ItmKeySelected;
                        grdRow.Cells["ItmID"].Value = ItmID;
                        grdRow.Cells["HSCode"].Value = IndustryPN;
                        grdRow.Cells["CountryID"].Value = CountryID;
                        grdRow.Cells["ItmType"].Value = ItmType;
                        grdRow.Cells["ItmDes"].Value = ItmDes;
                        grdRow.Cells["ItmDeptKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmDeptKey"].Value, 0));
                        grdRow.Cells["ItmTranGrpKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmTranGrpKey"].Value, 0));
                        switch (ItmTypeGrp)
                        {
                            case (int)GEnum.INTypeGrp.Total:
                                break;
                            default:
                                grdRow.Cells["ItmAccKey"].Value = ItmAccKey.ToDBValue(); //Ask Mic

                                MSTAcc objMSTAcc = MSTAcc.Get(ItmAccKey);
                                if (!GFunc.IsNEZ(objMSTAcc.AccKey))
                                {
                                    grdRow.Cells["ItmAccID"].Value = objMSTAcc.AccID;
                                    grdRow.Cells["ItmAccDes"].Value = objMSTAcc.AccDes;
                                }
                                break;
                        }
                        grdRow.Cells["ItmLocKey"].Value = GetItmInfor_Loc(DocMode, objItm, GFunc.NEInt(grdRow.Cells["ItmLocKey"].Value, 0)).ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmStock"].Value = ItmStock.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQty"].Value = ItmQty.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmUOMKey"].Value = ItmUOMKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmConRate"].Value = ItmConRate.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLatestCostF"].Value = ItmLatestCostF.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLatestCostH"].Value = ItmLatestCostH.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmListPrice"].Value = ItmPriceList.ToDBValue();
                        grdRow.Cells["ItmPriceBefore"].Value = ItmPriceBefore.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPriceAfter"].Value = ItmPriceAfter.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisPercent"].Value = ItmDisPercent.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPrice"].Value = 0;
                        grdRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtF"].Value = 0;
                        grdRow.Cells["ItmAmtH"].Value = 0;
                        grdRow.Cells["ItmTaxable"].Value = objItm.Taxable;
                        grdRow.Cells["ItmTaxGrpKey"].Value = GetItmInfor_TaxGrpKey(objItm, (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc)).ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmTaxGrpRate"].Value = GetItmInfor_TaxGrpRate(objItm, (decimal?)GFunc.GetPropertyValue("DocTaxGrpRate", objDoc));
                        grdRow.Cells["ItmTaxGrpAmtF"].Value = 0;
                        grdRow.Cells["ItmTaxGrpAmtL"].Value = 0;
                        grdRow.Cells["ItmAddCostF"].Value = 0;
                        grdRow.Cells["ItmAddCostH"].Value = 0;
                        grdRow.Cells["ItmAddAmtF"].Value = 0;
                        grdRow.Cells["ItmAddAmtH"].Value = 0;
                        grdRow.Cells["ItmColorKey"].Value = ItmColorKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmScaleSize"].Value = ItmScaleSize;
                        grdRow.Cells["ItmPacking"].Value = ItmPacking;
                        grdRow.Cells["ItmMark"].Value = ItmMark;
                        grdRow.Cells["ItmJobKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobPhaseKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobPhaseKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobTaskKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobTaskKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobCostTypeKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobCostTypeKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ARQOID"].Value = null;
                        grdRow.Cells["ARQODK"].Value = 0;
                        grdRow.Cells["ARQODItm"].Value = 0;
                        grdRow.Cells["ARSOID"].Value = null;
                        grdRow.Cells["ARSODK"].Value = 0;
                        grdRow.Cells["ARSODItm"].Value = 0;
                        grdRow.Cells["ARSOPOID"].Value = null;
                        grdRow.Cells["ARDOID"].Value = null;
                        grdRow.Cells["ARDODK"].Value = 0;
                        grdRow.Cells["ARDODItm"].Value = 0;
                        grdRow.Cells["ARIVID"].Value = null;
                        grdRow.Cells["ARIVDK"].Value = 0;
                        grdRow.Cells["ARIVDItm"].Value = 0;
                        grdRow.Cells["APPOID"].Value = null;
                        grdRow.Cells["APPODK"].Value = 0;
                        grdRow.Cells["APPODItm"].Value = 0;
                        grdRow.Cells["APPDID"].Value = null;
                        grdRow.Cells["APPDDK"].Value = 0;
                        grdRow.Cells["APPDDItm"].Value = 0;
                        if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Invoice && ItmType == (int)GEnum.ItemType.Charges) /* added by YST on 2021/08/15 */
                            grdRow.Cells["Custom1"].Value = objItm.Custom1;
                        break;
                    #endregion

                    #region INADJ
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                        grdRow.Cells["ItmKey"].Value = ItmKey;
                        grdRow.Cells["ItmKeySelect"].Value = ItmKeySelected;
                        grdRow.Cells["ItmID"].Value = ItmID;
                        grdRow.Cells["ItmType"].Value = ItmType;
                        grdRow.Cells["ItmDes"].Value = ItmDes;
                        grdRow.Cells["ItmDeptKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmDeptKey"].Value, 0));
                        grdRow.Cells["ItmTranGrpKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmTranGrpKey"].Value, 0));
                        switch (ItmTypeGrp)
                        {
                            case (int)GEnum.INTypeGrp.Total:
                                break;
                            default:
                                grdRow.Cells["ItmAccKey"].Value = ItmAccKey.ToDBValue(); //Ask Mic
                                MSTAcc objMSTAcc = MSTAcc.Get(cn,ItmAccKey);
                                if (!GFunc.IsNEZ(objMSTAcc.AccKey))
                                {
                                    grdRow.Cells["ItmAccID"].Value = objMSTAcc.AccID;
                                    grdRow.Cells["ItmAccDes"].Value = objMSTAcc.AccDes;
                                }
                                break;
                        }
                        grdRow.Cells["ItmLocKey"].Value = GetItmInfor_Loc(DocMode, objItm, GFunc.NEInt(grdRow.Cells["ItmLocKey"].Value, 0)).ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmStock"].Value = ItmStock.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQty"].Value = ItmQty.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmUOMKey"].Value = ItmUOMKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmConRate"].Value = ItmConRate.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmCost"].Value =grdRow.Cells["ItmCost"].Value == DBNull.Value ? ItmLatestCostH.ToDBValue() : grdRow.Cells["ItmCost"].Value; // ItmLatestCostH.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmNewCost"].Value = grdRow.Cells["ItmNewCost"].Value == DBNull.Value ? 0 : grdRow.Cells["ItmNewCost"].Value;
                        grdRow.Cells["ItmColorKey"].Value = ItmColorKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmScaleSize"].Value = ItmScaleSize;
                        grdRow.Cells["ItmPacking"].Value = ItmPacking;
                        grdRow.Cells["ItmJobKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobPhaseKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobPhaseKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobTaskKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobTaskKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobCostTypeKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobCostTypeKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["CSCPSID"].Value = null;
                        grdRow.Cells["CSCPSDK"].Value = 0;
                        grdRow.Cells["CSCPSDItm"].Value = 0;
                        break;
                    #endregion

                    #region INMFN
                    case (int)GEnum.SystemCode.Inventory_Production:
                        grdRow.Cells["ItmKey"].Value = ItmKey;
                        grdRow.Cells["ItmKeySelect"].Value = ItmKeySelected;
                        grdRow.Cells["ItmID"].Value = ItmID;
                        grdRow.Cells["ItmFGKey"].Value = 0;
                        grdRow.Cells["ItmFGKeySelect"].Value = 0;
                        grdRow.Cells["ItmType"].Value = ItmType;
                        grdRow.Cells["ItmDes"].Value = ItmDes;
                        grdRow.Cells["ItmDeptKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmDeptKey"].Value, 0));
                        grdRow.Cells["ItmTranGrpKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmTranGrpKey"].Value, 0));
                        grdRow.Cells["ItmAccINKey"].Value = ItmAccKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLocKey"].Value = GetItmInfor_Loc(DocMode, objItm, GFunc.NEInt(grdRow.Cells["ItmLocKey"].Value, 0)).ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPacking"].Value = ItmPacking;

                        switch (GFunc.NEInt(grdRow.Cells["LineType"].Value, 0))
                        {
                            case 3000:  //Document Detail Finished Goods
                            case 3010:	//Document Detail Finished Goods - Batch
                            case 3020:	//Document Detail Finished Goods - Batch - Serial
                            case 3030:	//Document Detail Finished Goods - Serial
                                grdRow.Cells["FGBUOMKey"].Value = ItmUOMKey.ToDBValue(); //Ask Mic
                                grdRow.Cells["FGWeight"].Value = GFunc.NEDec(objItm.WeightNet, 0);
                                grdRow.Cells["FGWeightUOMKey"].Value = objItm.WeightUOMKey.ToDBValue(); //Ask Mic
                                grdRow.Cells["FGReq"].Value = 0;
                                grdRow.Cells["FGProduceQty"].Value = 0;
                                grdRow.Cells["FGProduceWeight"].Value = 0;
                                grdRow.Cells["FGProduceGram"].Value = 0;
                                grdRow.Cells["FGOverHeadKey"].Value = objItm.BOMOverHeadKey.ToDBValue(); //Ask Mic
                                grdRow.Cells["FGOverHeadCost"].Value = DocComUtility.OverHeadCost_Get((int?)objItm.BOMOverHeadKey);
                                grdRow.Cells["FGOverHeadAmtH"].Value = 0;
                                grdRow.Cells["FGCostRatio"].Value = 0;
                                grdRow.Cells["BOMMultiplier"].Value = objItm.BOMMultiplier;
                                grdRow.Cells["BOMBUOMKey"].Value = DBNull.Value;
                                grdRow.Cells["BOMWeight"].Value = 0;
                                grdRow.Cells["BOMWeightUOMKey"].Value = DBNull.Value;
                                grdRow.Cells["BOMReq"].Value = 0;
                                grdRow.Cells["BOMIssue"].Value = 0;
                                grdRow.Cells["BOMReturn"].Value = 0;
                                grdRow.Cells["BOMUsed"].Value = 0;
                                grdRow.Cells["BOMUsedWeight"].Value = 0;
                                grdRow.Cells["BOMUsedGram"].Value = 0;
                                grdRow.Cells["BOMLabourCost"].Value = 0;
                                grdRow.Cells["BOMLabourAmt"].Value = 0;
                                break;

                            case 3100:	//Document Detail Raw Material
                            case 3110:	//Document Detail Raw Material - Batch
                            case 3120:	//Document Detail Raw Material - Batch Serial
                            case 3130:	//Document Detail Raw Material - Serial
                            case 3200:	//Document Detail Packing Material
                            case 3210:	//Document Detail Packing Material - Batch
                            case 3220:	//Document Detail Packing Material - Batch - Serial
                            case 3230:	//Document Detail Packing Material - Serial
                            case 3300:	//Document Detail Other Manuafacturing Cost
                                grdRow.Cells["FGBUOMKey"].Value = DBNull.Value;
                                grdRow.Cells["FGWeight"].Value = 0;
                                grdRow.Cells["FGWeightUOMKey"].Value = DBNull.Value;
                                grdRow.Cells["FGReq"].Value = 0;
                                grdRow.Cells["FGProduceQty"].Value = 0;
                                grdRow.Cells["FGProduceWeight"].Value = 0;
                                grdRow.Cells["FGProduceGram"].Value = 0;
                                grdRow.Cells["FGOverHeadKey"].Value = DBNull.Value;
                                grdRow.Cells["FGOverHeadCost"].Value = 0;
                                grdRow.Cells["FGOverHeadAmtH"].Value = 0;
                                grdRow.Cells["FGCostRatio"].Value = 0;
                                grdRow.Cells["BOMMultiplier"].Value = GFunc.NEInt(objItm.BOMMultiplier, 1);
                                grdRow.Cells["BOMBUOMKey"].Value = objItm.BUOMKey.ToDBValue(); //Ask Mic
                                grdRow.Cells["BOMWeight"].Value = GFunc.NEDec(objItm.WeightNet, 0);
                                grdRow.Cells["BOMWeightUOMKey"].Value = objItm.WeightUOMKey.ToDBValue(); //Ask Mic
                                grdRow.Cells["BOMReq"].Value = 0;
                                grdRow.Cells["BOMIssue"].Value = 0;
                                grdRow.Cells["BOMReturn"].Value = 0;
                                grdRow.Cells["BOMUsed"].Value = 0;
                                grdRow.Cells["BOMUsedWeight"].Value = 0;
                                grdRow.Cells["BOMUsedGram"].Value = 0;
                                grdRow.Cells["BOMLabourCost"].Value = 0;
                                grdRow.Cells["BOMLabourAmt"].Value = 0;
                                break;
                        }
                        break;
                    #endregion

                    #region INTRN
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                        grdRow.Cells["ItmKey"].Value = ItmKey;
                        grdRow.Cells["ItmKeySelect"].Value = ItmKeySelected;
                        grdRow.Cells["ItmID"].Value = ItmID;
                        grdRow.Cells["ItmType"].Value = ItmType;
                        grdRow.Cells["ItmDes"].Value = ItmDes;
                        grdRow.Cells["ItmDeptKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmDeptKey"].Value, 0));
                        grdRow.Cells["ItmTranGrpKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmTranGrpKey"].Value, 0));
                        grdRow.Cells["ItmFromLocKey"].Value = GetItmInfor_Loc(DocMode, objItm, GFunc.NEInt(grdRow.Cells["ItmFromLocKey"].Value, 0));
                        grdRow.Cells["ItmToLocKey"].Value = GetItmInfor_Loc(DocMode, objItm, GFunc.NEInt(grdRow.Cells["ItmToLocKey"].Value, 0));
                        grdRow.Cells["ItmStock"].Value = ItmStock;
                        grdRow.Cells["ItmQty"].Value = ItmQty;
                        grdRow.Cells["ItmUOMKey"].Value = ItmUOMKey;
                        grdRow.Cells["ItmConRate"].Value = ItmConRate;
                        grdRow.Cells["ItmColorKey"].Value = ItmColorKey.ToDBValue();
                        grdRow.Cells["ItmScaleSize"].Value = ItmScaleSize;
                        grdRow.Cells["ItmPacking"].Value = ItmPacking;

                        if (ItmType == (int)GEnum.ItemType.Consignment)
                        {
                            grdRow.Cells["ItmFromAccKey"].Value = 0;
                            grdRow.Cells["ItmToAccKey"].Value = 0;
                            grdRow.Cells["ItmFromAccID"].Value = string.Empty;
                            grdRow.Cells["ItmFromAccDes"].Value = string.Empty;
                            grdRow.Cells["ItmToAccID"].Value = string.Empty;
                            grdRow.Cells["ItmToAccDes"].Value = string.Empty;
                        }
                        else
                        {
                            grdRow.Cells["ItmFromAccKey"].Value = ItmAccKey;
                            grdRow.Cells["ItmToAccKey"].Value = ItmAccKey;
                            MSTAcc objMSTAcc = MSTAcc.Get(ItmAccKey);
                            if (!GFunc.IsNEZ(objMSTAcc.AccKey))
                            {
                                grdRow.Cells["ItmFromAccID"].Value = objMSTAcc.AccID;
                                grdRow.Cells["ItmFromAccDes"].Value = objMSTAcc.AccDes;
                                grdRow.Cells["ItmToAccID"].Value = objMSTAcc.AccID;
                                grdRow.Cells["ItmToAccDes"].Value = objMSTAcc.AccDes;
                            }
                        }
                        break;
                    #endregion

                    #region CSCSI, CSR
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        grdRow.Cells["ItmKey"].Value = ItmKey;
                        grdRow.Cells["ItmKeySelect"].Value = ItmKeySelected;
                        grdRow.Cells["ItmID"].Value = ItmID;
                        grdRow.Cells["ItmType"].Value = ItmType;
                        grdRow.Cells["ItmDes"].Value = ItmDes;
                        grdRow.Cells["ItmDeptKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmDeptKey"].Value, 0));
                        grdRow.Cells["ItmTranGrpKey"].Value = GetItmInfor_DeptTranGrp(objItm, GFunc.NEInt(grdRow.Cells["ItmTranGrpKey"].Value, 0));
                        grdRow.Cells["ItmFromLocKey"].Value = GFunc.NEInt(GetItmInfor_Loc(DocMode, objItm, GFunc.NEInt(grdRow.Cells["ItmFromLocKey"].Value, 0)), 0);
                        grdRow.Cells["ItmToLocKey"].Value = GFunc.NEInt(GetItmInfor_Loc(DocMode, objItm, GFunc.NEInt(grdRow.Cells["ItmToLocKey"].Value, 0)), 0);
                        grdRow.Cells["ItmFromAccKey"].Value = ItmAccKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmToAccKey"].Value = ItmAccKey.ToDBValue(); //Ask Mic

                        grdRow.Cells["ItmStock"].Value = ItmStock.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQty"].Value = ItmQty.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQtyLink"].Value = ItmQtyLink.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmUOMKey"].Value = ItmUOMKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmConRate"].Value = ItmConRate.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLatestCostF"].Value = ItmLatestCostF.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLatestCostH"].Value = ItmLatestCostH.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmListPrice"].Value = ItmPriceList.ToDBValue();
                        grdRow.Cells["ItmPriceBefore"].Value = ItmPriceBefore.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPriceAfter"].Value = ItmPriceAfter.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisPercent"].Value = ItmDisPercent.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPrice"].Value = 0;
                        grdRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmControlPrice"].Value = ItmControlPrice.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmControlPriceBase"].Value = ItmControlPriceBase.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtF"].Value = 0;
                        grdRow.Cells["ItmAmtH"].Value = 0;
                        grdRow.Cells["ItmColorKey"].Value = ItmColorKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmScaleSize"].Value = ItmScaleSize;
                        grdRow.Cells["ItmPacking"].Value = ItmPacking;
                        grdRow.Cells["ItmMark"].Value = ItmMark;
                        grdRow.Cells["ItmJobKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobPhaseKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobPhaseKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobTaskKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobTaskKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobCostTypeKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobCostTypeKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ARSOID"].Value = null;
                        grdRow.Cells["ARSODK"].Value = 0;
                        grdRow.Cells["ARSODItm"].Value = 0;
                        grdRow.Cells["ARSOPOID"].Value = null;
                        grdRow.Cells["CSCSIID"].Value = null;
                        grdRow.Cells["CSCSIDK"].Value = 0;
                        grdRow.Cells["CSCSIDItm"].Value = 0;

                        if (ItmAccKey > 0)
                        {
                            DataTable dt = ((TAUtil.TAComboBox)grdItm.DisplayLayout.Bands[0].Columns["ItmFromAccKey"].EditorComponent).DataSource as DataTable;
                            DataRow[] rows = dt.Select("Key=" + ItmAccKey);
                            grdRow.Cells["ItmFromAccDes"].Value = rows[0]["Des"];
                            grdRow.Cells["ItmToAccDes"].Value = rows[0]["Des"];
                        }
                        break;
                    #endregion

                    #region CSCPO
                    case (int)GEnum.SystemCode.Order_Consignment:
                        grdRow.Cells["ItmKey"].Value = ItmKey;
                        grdRow.Cells["ItmKeySelect"].Value = ItmKeySelected;
                        grdRow.Cells["ItmID"].Value = ItmID;
                        grdRow.Cells["ItmType"].Value = ItmType;
                        grdRow.Cells["ItmDes"].Value = ItmDes;
                        grdRow.Cells["ItmLocKey"].Value = GetItmInfor_Loc(DocMode, objItm, GFunc.NEInt(grdRow.Cells["ItmLocKey"].Value, 0)).ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmStock"].Value = ItmStock.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQty"].Value = ItmQty.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQtyLink"].Value = ItmQtyLink.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQtyAdj"].Value = ItmQtyAdj.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQtyBalance"].Value = ItmQty.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmReqDate"].Value = GFunc.NEStr(grdRow.Cells["ItmReqDate"].Value, objDoc.DocDate);
                        grdRow.Cells["ItmPrmDate"].Value = GFunc.NEStr(grdRow.Cells["ItmPrmDate"].Value, objDoc.DocDate);
                        grdRow.Cells["ItmUOMKey"].Value = ItmUOMKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmConRate"].Value = ItmConRate.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLatestCostF"].Value = ItmLatestCostF.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLatestCostH"].Value = ItmLatestCostH.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmListPrice"].Value = ItmPriceList.ToDBValue();
                        grdRow.Cells["ItmPriceBefore"].Value = ItmPriceBefore.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPriceAfter"].Value = ItmPriceAfter.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisPercent"].Value = ItmDisPercent.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPrice"].Value = 0;
                        grdRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtF"].Value = 0;
                        grdRow.Cells["ItmAmtH"].Value = 0;
                        grdRow.Cells["ItmColorKey"].Value = ItmColorKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmScaleSize"].Value = ItmScaleSize;
                        grdRow.Cells["ItmPacking"].Value = ItmPacking;
                        grdRow.Cells["ItmMark"].Value = ItmMark;
                        grdRow.Cells["ItmJobKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobPhaseKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobPhaseKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobTaskKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobTaskKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobCostTypeKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobCostTypeKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        break;
                    #endregion

                    #region CSCPD
                    case (int)GEnum.SystemCode.Received_Consignment:
                        grdRow.Cells["ItmKey"].Value = ItmKey;
                        grdRow.Cells["ItmKeySelect"].Value = ItmKeySelected;
                        grdRow.Cells["ItmID"].Value = ItmID;
                        grdRow.Cells["ItmType"].Value = ItmType;
                        grdRow.Cells["ItmDes"].Value = ItmDes;
                        grdRow.Cells["ItmLocKey"].Value = GetItmInfor_Loc(DocMode, objItm, GFunc.NEInt(grdRow.Cells["ItmLocKey"].Value, 0)).ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmStock"].Value = ItmStock.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQty"].Value = ItmQty.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmQtyLink"].Value = ItmQtyLink.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmUOMKey"].Value = ItmUOMKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmConRate"].Value = ItmConRate.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLatestCostF"].Value = ItmLatestCostF.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmLatestCostH"].Value = ItmLatestCostH.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmListPrice"].Value = ItmPriceList.ToDBValue();
                        grdRow.Cells["ItmPriceBefore"].Value = ItmPriceBefore.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPriceAfter"].Value = ItmPriceAfter.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisPercent"].Value = ItmDisPercent.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmPrice"].Value = 0;
                        grdRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmAmtF"].Value = 0;
                        grdRow.Cells["ItmAmtH"].Value = 0;
                        grdRow.Cells["ItmColorKey"].Value = ItmColorKey.ToDBValue(); //Ask Mic
                        grdRow.Cells["ItmScaleSize"].Value = ItmScaleSize;
                        grdRow.Cells["ItmPacking"].Value = ItmPacking;
                        grdRow.Cells["ItmMark"].Value = ItmMark;
                        grdRow.Cells["ItmJobKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobPhaseKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobPhaseKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobTaskKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobTaskKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["ItmJobCostTypeKey"].Value = GetItmInfor_Job((int)objDoc.DocCodeKey, objItm, GFunc.NEInt(grdRow.Cells["ItmJobCostTypeKey"].Value, 0), GFunc.NEInt(grdRow.Cells["ItmJobKey"].Value, 0));
                        grdRow.Cells["CSCPOID"].Value = null;
                        grdRow.Cells["CSCPODK"].Value = 0;
                        grdRow.Cells["CSCPODItm"].Value = 0;
                        grdRow.Cells["CSCPSID"].Value = null;
                        grdRow.Cells["CSCPSDK"].Value = 0;
                        grdRow.Cells["CSCPSDItm"].Value = 0;
                        break;
                    #endregion
                }

                #endregion

                #region Run Special calculation for SPECIAL UOM
                if (runSpecialCalculation)
                {
                    if (GlobalUI.bRuningImport == false)
                    {
                        frmSpecialCalculation specCal = new frmSpecialCalculation(objDoc, calType, GEnum.SpecialCalculationProcessType.UOM, grdItm);
                        if (specCal.ShowDialog() == DialogResult.OK)
                        {
                            grdRow.Cells["ItmDes"].Value = specCal.ItmDes;
                            grdRow.Cells["ItmQty"].Value = specCal.ItmQty;
                            if (ItmQty_CustomUpdate(objDoc, grdItm) == false)
                                return false;
                        }
                    }
                }
                #endregion

                #region delete child details
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Packing_List:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:

                        //Skip for those code keys
                        break;
                    default:
                        switch (ItmType)
                        {
                            case (int)GEnum.ItemType.Assembly:
                            case (int)GEnum.ItemType.StockB:
                            case (int)GEnum.ItemType.Finished_GDB:
                            case (int)GEnum.ItemType.Serial_Finished_GDB:
                            case (int)GEnum.ItemType.Serial_StockB:
                                //Remove Child Itms by DocItmKey=LineLinkKey
                                int itmKey = GFunc.NEInt(grdRow.Cells["DocItmKey"].Value, 0);

                                for (int i = 0; i < dtItm.Rows.Count; i++)
                                {
                                    DataRow dr = dtItm.Rows[i];
                                    if (GFunc.NEInt(dr["LineLinkKey"], 0) == itmKey)
                                        dtItm.Rows.Remove(dr);
                                }
                                break;
                        }
                        break;
                }
                #endregion

                #region open popupform Batch/Assembly
                if (GlobalUI.bRuningImport == false)
                {
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Quotation:
                        case (int)GEnum.SystemCode.Sales_Order:
                        case (int)GEnum.SystemCode.Reserve_Order:
                            switch (ItmType)
                            {
                                case (int)GEnum.ItemType.Assembly:
                                    frmAssemblyEntry assemblyPopup = new frmAssemblyEntry(objDoc, grdItm, GFunc.NEInt(grdRow.Cells["DocItmKey"].Value, 0), true);
                                    if(jobItem!=2)
                                        assemblyPopup.ShowDialog();
                                    else
                                    {
                                        assemblyPopup.LoadJobChildItems();
                                    }
                                    break;
                            }
                            break;

                        case (int)GEnum.SystemCode.Delivery_Order:
                        case (int)GEnum.SystemCode.Sales_Invoice:
                        case (int)GEnum.SystemCode.Sales_Debit_Note:
                        case (int)GEnum.SystemCode.Sales_Credit_Note:
                        case (int)GEnum.SystemCode.Cash_Sale:
                        case (int)GEnum.SystemCode.Cash_Debit_Note:
                        case (int)GEnum.SystemCode.Cash_Credit_Note:
                            switch (ItmType)
                            {
                                case (int)GEnum.ItemType.StockB:
                                case (int)GEnum.ItemType.Finished_GDB:
                                case (int)GEnum.ItemType.Serial_Finished_GDB:
                                case (int)GEnum.ItemType.Serial_StockB:
                                    if (objDoc.DocCodeKey != (int)GEnum.SystemCode.Inventory_Production && objDoc.DocCodeKey != (int)GEnum.SystemCode.Inventory_Adjustment)
                                    {
                                        frmBatchEntry batchPopup = new frmBatchEntry(objDoc, grdItm, false);
                                        batchPopup.ShowDialog();
                                    }
                                    break;

                                case (int)GEnum.ItemType.Assembly:
                                    frmAssemblyEntry assemblyPopup = new frmAssemblyEntry(objDoc, grdItm, GFunc.NEInt(grdRow.Cells["DocItmKey"].Value, 0), true);
                                    assemblyPopup.ShowDialog();
                                    break;
                            }
                            break;
                        case (int)GEnum.SystemCode.Purchase_Shipment:
                        case (int)GEnum.SystemCode.Purchase_Delivery:
                        case (int)GEnum.SystemCode.Purchase_Invoice:
                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        case (int)GEnum.SystemCode.Inventory_Adjustment:
                        case (int)GEnum.SystemCode.Inventory_Transfer:
                        case (int)GEnum.SystemCode.Issue_Consignment:
                        case (int)GEnum.SystemCode.Return_Consignment:
                        case (int)GEnum.SystemCode.Inventory_Production:
                            switch (ItmType)
                            {
                                case (int)GEnum.ItemType.StockB:
                                case (int)GEnum.ItemType.Finished_GDB:
                                case (int)GEnum.ItemType.Serial_Finished_GDB:
                                case (int)GEnum.ItemType.Serial_StockB:
                                    if (objDoc.DocCodeKey != (int)GEnum.SystemCode.Inventory_Production)
                                    {
                                        frmBatchEntry batchPopup = new frmBatchEntry(objDoc, grdItm, false);
                                        batchPopup.ShowDialog();
                                    }
                                    break;
                            }
                            break;
                    }
                }
                #endregion
                
                GlobalUI.PopupRefresh(grdItm);

                //Jack Added;
                //To Set default ItmRef from DocDes
                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Inventory_Adjustment)
                {
                    object docTmp = GFunc.GetPropertyValue("DocDes", objDoc);
                    string docDes = string.Empty;
                    if (GFunc.IsNE(docTmp) == false)
                    {
                        docDes = docTmp.ToString();
                        if(GFunc.IsNE(grdRow.Cells["ItmRef"].Value))
                            grdRow.Cells["ItmRef"].Value = docDes;
                    }

                }                
                //End Jack Added;

                //Remove Later;
                //Mic Check ; Jack Added; 30 Nov 2012
                //To Check vendor is valid if selected Itm has default vendor
                //if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Delivery_Order || objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Order ||
                //    objDoc.DocCodeKey == (int)GEnum.SystemCode.Quotation || objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Invoice (all that has vendor key))
                //{
                //    if (GFunc.IsNEZ(grdItm.ActiveRow.Cells["ItmVendorKey"].Value) == false)
                //    {
                //        MSTCon conObjTmp = MSTCon.Get(cn, (int)grdItm.ActiveRow.Cells["ItmVendorKey"].Value);
                //        string id = conObjTmp.ConID;
                //        string des = conObjTmp.ConNm;
                //        return ItmVendorID_Update(objDoc, grdItm, (int)conObjTmp.ConKey, id, des);
                //    }
                //}


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

        public static bool DetAccID_btnClick(Document objDoc, Hashtable docDet, UltraGridCell gridCell, GEnum.PopupType popUpType, string listSettingID)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;

                if (DocHDRUtil.EditorButton_Popup((int)objDoc.DocCodeKey, gridCell.Text, listSettingID, (int)popUpType, ref key, ref id, ref des))
                    return DetAccID_Update(objDoc, docDet, gridCell, key, id, des);
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
        public static bool DetAccID_CustomUpdate(Document objDoc, Hashtable docDet, UltraGridCell gridCell, GEnum.RecAccessType recAccessType, string listSettingID)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string ctrlValue = GFunc.NEStr(gridCell.Text, "");
                int popUpType = 0;

                key = GFunc.AccRecord_GetKey(recAccessType, listSettingID, ctrlValue, ref id, ref des, true);
                if (key == 0)
                {
                    if (recAccessType == GEnum.RecAccessType.AccID)
                        popUpType = (int)GEnum.PopupType.AccID;
                    else if (recAccessType == GEnum.RecAccessType.AccDes)
                        popUpType = (int)GEnum.PopupType.AccDes;
                    else
                        return false;

                    if (DocHDRUtil.EditorButton_Popup((int)objDoc.DocCodeKey, gridCell.Text, listSettingID, popUpType, ref key, ref id, ref des) == false)
                        return false;
                }

                return DetAccID_Update(objDoc, docDet, gridCell, key, id, des);
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
        //With Text Editor
        private static bool DetAccID_Update(Document objDoc, Hashtable docDet, TAUtil.TATextBoxEditor ctrl, int key, string id, string des)
        {
            try
            {
                if (DetAccID_Validation(objDoc, docDet, ctrl, key) == false)
                    return false;

                if (DetAccID_DependentSet(objDoc, docDet, ctrl, key, id, des) == false)
                {
                    return false;
                }

                //switch (ctrl.Name)
                //{
                //    case "DocAccKey":
                //    case "DocAccID":
                //    case "DocAccDes":                      
                //        switch (objDoc.DocCodeKey)
                //        {
                //            case (int)GEnum.SystemCode.Deposit:
                //                GFunc.SetPropertyValue("DocCurrKey", objDoc, MSTAcc.Get(key).AccCurrKey);
                //                if (DocHDRUtil.DocCurrKey_CustomUpdate( objDoc, docDet) == false)
                //                    return false;
                //                break;
                //        }
                //        break;

                //}

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
        private static bool DetAccID_Validation(Document objDoc, Hashtable docDet, TAUtil.TATextBoxEditor ctrl, int? key)
        {
            try
            {
                UltraGrid grdItm = null;
                UltraGrid grdExp = null;

                if (DocComUtility.DocDetail_Get((int)objDoc.DocCodeKey, docDet, ref grdItm, ref grdExp) == false)
                    return false;

                switch (ctrl.Name.ToLower())
                {
                    case "expacckey":
                    case "expaccid":
                    case "expaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Payment_Received:
                            case (int)GEnum.SystemCode.Cash_Payment_Received:
                            case (int)GEnum.SystemCode.Payment_Issue:
                                if (GFunc.IsNEZ(key))
                                {
                                    MsgBox.Show("Detail account cannot be empty");
                                    return false;
                                }
                                break;
                        }
                        break;

                    case "itmaccinkey":
                    case "itmaccinid":
                    case "itmaccindes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Inventory_Production:
                                if (GFunc.IsNEZ(grdItm.ActiveRow.Cells["ItmKey"].Value))
                                {
                                    MsgBox.Show("Item ID cannot be empty");
                                    return false;
                                }
                                if (GFunc.IsNEZ(key))
                                {
                                    MsgBox.Show("Detail inventory account cannot be empty");
                                    return false;
                                }
                                break;
                        }
                        break;

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
                                if (GFunc.IsNEZ(grdItm.ActiveRow.Cells["ItmKey"].Value))
                                {
                                    MsgBox.Show("Item ID cannot be empty");
                                    return false;
                                }
                                if (GFunc.IsPostingItmType(grdItm.ActiveRow.Cells["ItmType"].Value))
                                    if (GFunc.IsNEZ(key))
                                    {
                                        MsgBox.Show("Detail sales account cannot be empty");
                                        return false;
                                    }
                                break;

                            case (int)GEnum.SystemCode.Purchase_Order:
                            case (int)GEnum.SystemCode.Purchase_Shipment:
                            case (int)GEnum.SystemCode.Purchase_Delivery:
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
                                if (GFunc.IsNEZ(grdItm.ActiveRow.Cells["ItmKey"].Value))
                                {
                                    MsgBox.Show("Item ID cannot be empty");
                                    return false;
                                }
                                if (GFunc.IsPostingItmType(grdItm.ActiveRow.Cells["ItmType"].Value))
                                    if (GFunc.IsNEZ(key))
                                    {
                                        MsgBox.Show("Detail purchase/inventory account cannot be empty");
                                        return false;
                                    }
                                break;


                            case (int)GEnum.SystemCode.Inventory_Adjustment:
                                if (GFunc.IsNEZ(grdItm.ActiveRow.Cells["ItmKey"].Value))
                                {
                                    MsgBox.Show("Item ID cannot be empty");
                                    return false;
                                }
                                if (GFunc.IsNEZ(key))
                                {
                                    MsgBox.Show("Detail inventory account cannot be empty");
                                    return false;
                                }
                                break;

                            case (int)GEnum.SystemCode.Journal:
                                if (GFunc.IsNEZ(key))
                                {
                                    MsgBox.Show("Detail account cannot be empty");
                                    return false;
                                }
                                break;
                        }
                        break;

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
                                if (GFunc.NEDec(grdItm.ActiveRow.Cells["ItmApplyDueAmtF"].Value, 0) != 0)
                                {
                                    if (GFunc.IsNEZ(key))
                                    {
                                        MsgBox.Show("Detail discount account cannot be empty");
                                        return false;
                                    }
                                }
                                break;
                        }
                        break;

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
                                if (GFunc.NEDec(grdItm.ActiveRow.Cells["ItmApplyGainAmt"].Value, 0) != 0) 
                                {
                                    if (GFunc.IsNEZ(key))
                                    {
                                        MsgBox.Show("Detail gain/loss account cannot be empty");
                                        return false;
                                    }
                                }
                                break;
                        }
                        break;

                    case "itmdocacckey":
                    case "itmdocaccid":
                    case "itmdocaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Deposit:
                                if (GFunc.IsNEZ(key))
                                {
                                    MsgBox.Show("Detail account cannot be empty");
                                    return false;
                                }
                                break;
                        }
                        break;

                    case "itmfromacckey":
                    case "itmfromaccid":
                    case "itmfromaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Inventory_Transfer:
                            case (int)GEnum.SystemCode.Issue_Consignment:
                            case (int)GEnum.SystemCode.Return_Consignment:
                                if (GFunc.IsNEZ(grdItm.ActiveRow.Cells["ItmKey"].Value))
                                {
                                    MsgBox.Show("Item ID cannot be empty");
                                    return false;
                                }
                                if (GFunc.IsNEZ(key))
                                {
                                    MsgBox.Show("Detail inventory account FROM cannot be empty");
                                    return false;
                                }
                                break;
                        }
                        break;

                    case "itmtoacckey":
                    case "itmtoaccid":
                    case "itmtoaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Inventory_Transfer:
                            case (int)GEnum.SystemCode.Issue_Consignment:
                            case (int)GEnum.SystemCode.Return_Consignment:
                                if (GFunc.IsNEZ(grdItm.ActiveRow.Cells["ItmKey"].Value))
                                {
                                    MsgBox.Show("Item ID cannot be empty");
                                    return false;
                                }
                                if (GFunc.IsNEZ(key))
                                {
                                    MsgBox.Show("Detail inventory account TO cannot be empty");
                                    return false;
                                }
                                break;
                        }
                        break;

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
                                if (GFunc.IsNEZ(key))
                                {
                                    MsgBox.Show("Detail link account cannot be empty");
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
        private static bool DetAccID_DependentSet(Document objDoc, Hashtable docDet, TAUtil.TATextBoxEditor ctrl, int? key, string id, string des)
        {

            UltraGrid grdItm = null;
            UltraGrid grdExp = null;

            if (DocComUtility.DocDetail_Get((int)objDoc.DocCodeKey, docDet, ref grdItm, ref grdExp) == false)
                return false;

            try
            {
                switch (ctrl.Name.ToLower())
                {
                    case "expacckey":
                    case "expaccid":
                    case "expaccdes":
                        grdExp.ActiveRow.Cells["ExpAccKey"].Value = key.ToDBValue();
                        grdExp.ActiveRow.Cells["ExpAccID"].Value = id;
                        grdExp.ActiveRow.Cells["ExpAccDes"].Value = des;
                        break;

                    case "itmaccinkey":
                    case "itmaccinid":
                    case "itmaccindes":
                        grdItm.ActiveRow.Cells["ItmAccINKey"].Value = key.ToDBValue();
                        grdItm.ActiveRow.Cells["ItmAccINID"].Value = id;
                        grdItm.ActiveRow.Cells["ItmAccINDes"].Value = des;

                        break;

                    case "itmacckey":
                    case "itmaccid":
                    case "itmaccdes":
                        grdItm.ActiveRow.Cells["ItmAccKey"].Value = key.ToDBValue();
                        grdItm.ActiveRow.Cells["ItmAccID"].Value = id;
                        grdItm.ActiveRow.Cells["ItmAccDes"].Value = des;
                        break;

                    case "itmapplydisacckey":
                    case "itmapplydisaccid":
                    case "itmapplydisaccdes":
                        grdItm.ActiveRow.Cells["ItmApplyDisAccKey"].Value = key.ToDBValue();
                        grdItm.ActiveRow.Cells["ItmApplyDisAccID"].Value = id;
                        grdItm.ActiveRow.Cells["ItmApplyDisAccDes"].Value = des;
                        break;

                    case "itmapplygainacckey":
                    case "itmapplygainaccid":
                    case "itmapplygainaccdes":
                        grdItm.ActiveRow.Cells["ItmApplyGainAccKey"].Value = key.ToDBValue();
                        grdItm.ActiveRow.Cells["ItmApplyGainAccID"].Value = id;
                        grdItm.ActiveRow.Cells["ItmApplyGainAccDes"].Value = des;
                        break;

                    case "itmdocacckey":
                    case "itmdocaccid":
                    case "itmdocaccdes":
                        grdItm.ActiveRow.Cells["ItmDocAccKey"].Value = key.ToDBValue();
                        grdItm.ActiveRow.Cells["ItmDocAccID"].Value = id;
                        grdItm.ActiveRow.Cells["ItmDocAccDes"].Value = des;
                        break;

                    case "itmfromacckey":
                    case "itmfromaccid":
                    case "itmfromaccdes":
                        grdItm.ActiveRow.Cells["ItmFromAccKey"].Value = key.ToDBValue();
                        grdItm.ActiveRow.Cells["ItmFromAccID"].Value = id;
                        grdItm.ActiveRow.Cells["ItmFromAccDes"].Value = des;
                        break;

                    case "itmtoacckey":
                    case "itmtoaccid":
                    case "itmtoaccdes":
                        grdItm.ActiveRow.Cells["ItmToAccKey"].Value = key.ToDBValue();
                        grdItm.ActiveRow.Cells["ItmToAccID"].Value = id;
                        grdItm.ActiveRow.Cells["ItmToAccDes"].Value = des;
                        break;

                    case "linkdocacckey":
                    case "linkdocaccid":
                    case "linkdocaccdes":
                        GFunc.SetPropertyValue("LinkDocAccKey", objDoc, key.ToDBValue());
                        GFunc.SetPropertyValue("LinkDocAccID", objDoc, id);
                        GFunc.SetPropertyValue("LinkDocAccDes", objDoc, des);
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
        //With Grid Cell
        private static bool DetAccID_Update(Document objDoc, Hashtable docDet, UltraGridCell gridCell, int key, string id, string des)
        {
            UltraGrid grdItm=null ;
            try
            {
                if (DetAccID_Validation(objDoc, docDet, gridCell, key) == false)
                    return false;

                if (DetAccID_DependentSet(objDoc, docDet, gridCell, key, id, des) == false)
                {
                    return false;
                }
                switch (gridCell.Column.Key.ToLower())
                {
                    case "docacckey":
                    case "docaccid":
                    case "docaccdes":

                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Deposit:
                                GFunc.SetPropertyValue("DocCurrKey", objDoc, MSTAcc.Get(key).AccCurrKey);
                                if (DocHDRUtil.DocCurrKey_CustomUpdate(objDoc, docDet) == false)
                                    return false;
                                break;
                        }
                        break;
                    case "itmaccdes":
                    case "itmacckey":
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Journal:
                                gridCell.Row.Cells["ItmCurrKey"].Value = MSTAcc.Get(key).AccCurrKey;
                                if (GFunc.NEInt(gridCell.Row.Cells["ItmCurrKey"].Value, 0) != 1)//Currency is not home currency //NotReady
                                {
                                    gridCell.Row.Cells["ItmCurrKey"].Activation = Activation.ActivateOnly;
                                }
                                else
                                {
                                    gridCell.Row.Cells["ItmCurrKey"].Activation = Activation.AllowEdit;
                                }
                                if (ItmCurrKey_CustomeUpdate(objDoc, grdItm) == false)
                                    return false;
                                break;
                        }
                        break;
                    case "itmdocacckey":
                    case "itmdocaccid":
                    case "itmdocaccdes":
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                        switch (objDoc.DocCodeKey)
                        {                           
                            case (int)GEnum.SystemCode.Deposit:

                                int? currKey= MSTAcc.Get(key).AccCurrKey;
                                gridCell.Row.Cells["ItmDocCurrKey"].Value = currKey;

                                if (GFunc.IsNEZ(currKey))
                                    throw new TAException("Invalid Call, Detail Currency cannot be empty");

                                decimal currRate = (decimal)DocComUtility.CurrRate_Get(currKey, GFunc.GetDatePropertyValue("DocDate", objDoc), true);

                                gridCell.Row.Cells["ItmDocCurrRate"].Value = currRate;

                                gridCell.Row.Cells["ItmDocAmtH"].Value = GFunc.RndC((decimal)gridCell.Row.Cells["ItmDocAmtF"].Value * currRate, GVar.RndDecs.Amtpt);
                                if (GFunc.NEInt(gridCell.Row.Cells["ItmDocCurrKey"].Value, 0) == 1)
                                {
                                    gridCell.Row.Cells["ItmDocCurrKey"].Activation = Activation.AllowEdit;
                                    gridCell.Row.Cells["ItmDocCurrRate"].Activation = Activation.ActivateOnly;
                                }
                                else
                                {
                                    gridCell.Row.Cells["ItmDocCurrKey"].Activation = Activation.ActivateOnly;
                                    gridCell.Row.Cells["ItmDocCurrRate"].Activation = Activation.AllowEdit;
                                }
                                DocHDRUtil.FormGridLock_Set(objDoc, grdItm , GEnum.Details.Doc_Itm, false);
                                break;
                        }
                        break;

                }
                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Journal)
                {
                    object docTmp = GFunc.GetPropertyValue("DocDes", objDoc);
                    string docDes = string.Empty;
                    if (GFunc.IsNE(docTmp) == false)
                    {
                        docDes = docTmp.ToString();
                        if(GFunc.IsNE(gridCell.Row.Cells["ItmDes"].Value))
                            gridCell.Row.Cells["ItmDes"].Value = docDes;
                    }

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
        private static bool DetAccID_Validation(Document objDoc, Hashtable docDet, UltraGridCell gridCell, int? key)
        {
            try
            {
                UltraGrid grdItm = null;
                UltraGrid grdExp = null;

                if (DocComUtility.DocDetail_Get((int)objDoc.DocCodeKey, docDet, ref grdItm, ref grdExp) == false)
                    return false;

                switch (gridCell.Column.Key.ToLower())
                {
                    case "expacckey":
                    case "expaccid":
                    case "expaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Payment_Received:
                            case (int)GEnum.SystemCode.Cash_Payment_Received:
                            case (int)GEnum.SystemCode.Payment_Issue:
                                if (GFunc.IsNEZ(key))
                                {
                                    MsgBox.Show("Detail account cannot be empty");
                                    return false;
                                }
                                break;
                        }
                        break;

                    case "itmaccinkey":
                    case "itmaccinid":
                    case "itmaccindes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Inventory_Production:
                                if (GFunc.IsNEZ(grdItm.ActiveRow.Cells["ItmKey"].Value))
                                {
                                    MsgBox.Show("Item ID cannot be empty");
                                    return false;
                                }
                                if (GFunc.IsNEZ(key))
                                {
                                    MsgBox.Show("Detail inventory account cannot be empty");
                                    return false;
                                }
                                break;
                        }
                        break;

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
                                if (GFunc.IsNEZ(grdItm.ActiveRow.Cells["ItmKey"].Value))
                                {
                                    MsgBox.Show("Item ID cannot be empty");
                                    return false;
                                }
                                if (GFunc.IsPostingItmType(grdItm.ActiveRow.Cells["ItmType"].Value))
                                    if (GFunc.IsNEZ(key))
                                    {
                                        MsgBox.Show("Detail sales account cannot be empty");
                                        return false;
                                    }
                                break;

                            case (int)GEnum.SystemCode.Purchase_Order:
                            case (int)GEnum.SystemCode.Purchase_Shipment:
                            case (int)GEnum.SystemCode.Purchase_Delivery:
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
                                if (GFunc.IsNEZ(grdItm.ActiveRow.Cells["ItmKey"].Value))
                                {
                                    MsgBox.Show("Item ID cannot be empty");
                                    return false;
                                }
                                if (GFunc.IsPostingItmType(grdItm.ActiveRow.Cells["ItmType"].Value))
                                    if (GFunc.IsNEZ(key))
                                    {
                                        MsgBox.Show("Detail purchase/inventory account cannot be empty");
                                        return false;
                                    }
                                break;


                            case (int)GEnum.SystemCode.Inventory_Adjustment:
                                if (GFunc.IsNEZ(grdItm.ActiveRow.Cells["ItmKey"].Value))
                                {
                                    MsgBox.Show("Item ID cannot be empty");
                                    return false;
                                }
                                if (GFunc.IsNEZ(key))
                                {
                                    MsgBox.Show("Detail inventory account cannot be empty");
                                    return false;
                                }
                                break;

                            case (int)GEnum.SystemCode.Journal:
                                if (GFunc.IsNEZ(key))
                                {
                                    MsgBox.Show("Detail account cannot be empty");
                                    return false;
                                }
                                break;
                        }
                        break;

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
                                if (GFunc.NEDec(grdItm.ActiveRow.Cells["ItmApplyDueAmtF"].Value, 0) != 0)
                                {
                                    if (GFunc.IsNEZ(key))
                                    {
                                        MsgBox.Show("Detail discount account cannot be empty");
                                        return false;
                                    }
                                }
                                break;
                        }
                        break;

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
                                if (GFunc.NEDec(grdItm.ActiveRow.Cells["ItmApplyGainAmt"].Value, 0) != 0)
                                {
                                    if (GFunc.IsNEZ(key))
                                    {
                                        MsgBox.Show("Detail gain/loss account cannot be empty");
                                        return false;
                                    }
                                }
                                break;
                        }
                        break;

                    case "itmdocacckey":
                    case "itmdocaccid":
                    case "itmdocaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Deposit:
                                if (GFunc.IsNEZ(key))
                                {
                                    MsgBox.Show("Detail account cannot be empty");
                                    return false;
                                }
                                break;
                        }
                        break;

                    case "itmfromacckey":
                    case "itmfromaccid":
                    case "itmfromaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Inventory_Transfer:
                            case (int)GEnum.SystemCode.Issue_Consignment:
                            case (int)GEnum.SystemCode.Return_Consignment:
                                if (GFunc.IsNEZ(grdItm.ActiveRow.Cells["ItmKey"].Value))
                                {
                                    MsgBox.Show("Item ID cannot be empty");
                                    return false;
                                }
                                if (GFunc.IsNEZ(key))
                                {
                                    MsgBox.Show("Detail inventory account FROM cannot be empty");
                                    return false;
                                }
                                break;
                        }
                        break;

                    case "itmtoacckey":
                    case "itmtoaccid":
                    case "itmtoaccdes":
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Inventory_Transfer:
                            case (int)GEnum.SystemCode.Issue_Consignment:
                            case (int)GEnum.SystemCode.Return_Consignment:
                                if (GFunc.IsNEZ(grdItm.ActiveRow.Cells["ItmKey"].Value))
                                {
                                    MsgBox.Show("Item ID cannot be empty");
                                    return false;
                                }
                                if (GFunc.IsNEZ(key))
                                {
                                    MsgBox.Show("Detail inventory account TO cannot be empty");
                                    return false;
                                }
                                break;
                        }
                        break;

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
                                if (GFunc.IsNEZ(key))
                                {
                                    MsgBox.Show("Detail link account cannot be empty");
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
        private static bool DetAccID_DependentSet(Document objDoc, Hashtable docDet, UltraGridCell gridCell, int? key, string id, string des)
        {

            UltraGrid grdItm = null;
            UltraGrid grdExp = null;

            if (DocComUtility.DocDetail_Get((int)objDoc.DocCodeKey, docDet, ref grdItm, ref grdExp) == false)
                return false;


            try
            {
                switch (gridCell.Column.Key.ToLower())
                {
                    case "expacckey":
                    case "expaccid":
                    case "expaccdes":
                        grdExp.ActiveRow.Cells["ExpAccKey"].Value = key.ToDBValue();//Ask Mic
                        grdExp.ActiveRow.Cells["ExpAccID"].Value = id;
                        grdExp.ActiveRow.Cells["ExpAccDes"].Value = des;
                        break;

                    case "itmaccinkey":
                    case "itmaccinid":
                    case "itmaccindes":
                        grdItm.ActiveRow.Cells["ItmAccINKey"].Value = key.ToDBValue();//Ask Mic
                        grdItm.ActiveRow.Cells["ItmAccINID"].Value = id;
                        grdItm.ActiveRow.Cells["ItmAccINDes"].Value = des;

                        break;

                    case "itmacckey":
                    case "itmaccid":
                    case "itmaccdes":
                        grdItm.ActiveRow.Cells["ItmAccKey"].Value = key.ToDBValue();//Ask Mic
                        grdItm.ActiveRow.Cells["ItmAccID"].Value = id;
                        grdItm.ActiveRow.Cells["ItmAccDes"].Value = des;
                        break;

                    case "itmapplydisacckey":
                    case "itmapplydisaccid":
                    case "itmapplydisaccdes":
                        grdItm.ActiveRow.Cells["ItmApplyDisAccKey"].Value = key.ToDBValue();//Ask Mic
                        grdItm.ActiveRow.Cells["ItmApplyDisAccID"].Value = id;
                        grdItm.ActiveRow.Cells["ItmApplyDisAccDes"].Value = des;
                        break;

                    case "itmapplygainacckey":
                    case "itmapplygainaccid":
                    case "itmapplygainaccdes":
                        grdItm.ActiveRow.Cells["ItmApplyGainAccKey"].Value = key.ToDBValue();//Ask Mic
                        grdItm.ActiveRow.Cells["ItmApplyGainAccID"].Value = id;
                        grdItm.ActiveRow.Cells["ItmApplyGainAccDes"].Value = des;
                        break;

                    case "itmdocacckey":
                    case "itmdocaccid":
                    case "itmdocaccdes":
                        grdItm.ActiveRow.Cells["ItmDocAccKey"].Value = key.ToDBValue();//Ask Mic
                        grdItm.ActiveRow.Cells["ItmDocAccID"].Value = id;
                        grdItm.ActiveRow.Cells["ItmDocAccDes"].Value = des;
                        break;

                    case "itmfromacckey":
                    case "itmfromaccid":
                    case "itmfromaccdes":
                        grdItm.ActiveRow.Cells["ItmFromAccKey"].Value = key.ToDBValue();//Ask Mic
                        grdItm.ActiveRow.Cells["ItmFromAccID"].Value = id;
                        grdItm.ActiveRow.Cells["ItmFromAccDes"].Value = des;
                        break;

                    case "itmtoacckey":
                    case "itmtoaccid":
                    case "itmtoaccdes":
                        grdItm.ActiveRow.Cells["ItmToAccKey"].Value = key.ToDBValue(); //Ask Mic
                        grdItm.ActiveRow.Cells["ItmToAccID"].Value = id;
                        grdItm.ActiveRow.Cells["ItmToAccDes"].Value = des;
                        break;

                    case "linkdocacckey":
                    case "linkdocaccid":
                    case "linkdocaccdes":
                        GFunc.SetPropertyValue("LinkDocAccKey", objDoc, key.ToDBValue()); //Ask Mic
                        GFunc.SetPropertyValue("LinkDocAccID", objDoc, id);
                        GFunc.SetPropertyValue("LinkDocAccDes", objDoc, des);
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

        public static bool ItmApplyDisAmtF_CustomeUpdate(Document objDoc, Hashtable docDet,bool showPopup)
        {
            try
            {
                UltraGrid grd = null;
                UltraGridRow grdRow = null;
                decimal docLinkCurrRate = 1;
                decimal itmApplyDisAmtF = 0;
                decimal itmApplyDocAmtF = 0;
                decimal itmApplyDueAmtF = 0;

                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grd);
                grdRow = grd.ActiveRow;

                //validation check
                if (GFunc.IsNE(grdRow.Cells["ItmApplyDisAmtF"].Value))
                {
                    MsgBox.Show("Discount amount cannot be empty");
                    return false;
                }

                if (showPopup)
                {
                    frmPopupApplyDis frm = new frmPopupApplyDis(objDoc, ref grd);
                    frm.ShowDialog();
                }
                else if (GFunc.NEDec(grdRow.Cells["ItmApplyDisAmtF"].OriginalValue, 0) == 0 ||
                    GFunc.NEDec(grdRow.Cells["ItmApplyDisAccKey"].Value, 0) == 0)
                {
                   //Changed by May on 10 Jun 2014 per Mic instruction                  
                   
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Payment_Received:
                        case (int)GEnum.SystemCode.Cash_Payment_Received:
                            grdRow.Cells["ItmApplyDisAccKey"].Value = SysOptionUtility.GetInt("AccDiscountAllowed");
                            break;
                        case (int)GEnum.SystemCode.Payment_Issue:
                            grdRow.Cells["ItmApplyDisAccKey"].Value = SysOptionUtility.GetInt("AccDiscountReceived");
                            break;
                    }                   
                }

                //Validation
                itmApplyDueAmtF = GFunc.RndC(grd.ActiveRow.Cells["ItmApplyDueAmtF"].Value, GVar.RndDecs.Amtpt);
                itmApplyDisAmtF = GFunc.RndC(grd.ActiveRow.Cells["ItmApplyDisAmtF"].Value, GVar.RndDecs.Amtpt);
                itmApplyDocAmtF = GFunc.RndC(grd.ActiveRow.Cells["ItmApplyDocAmtF"].Value, GVar.RndDecs.Amtpt);

                
                if (GFunc.IsBetweenDec(itmApplyDisAmtF + itmApplyDocAmtF, 0, itmApplyDueAmtF) == false)
                {
                    MsgBox.Show("You have exceeded the amount due, please amend your discount amount");
                    return false;
                }

                if (itmApplyDueAmtF == (itmApplyDocAmtF + itmApplyDisAmtF))
                    grd.ActiveRow.Cells["ItmApplyFull"].Value = 1;
                else
                    grd.ActiveRow.Cells["ItmApplyFull"].Value = 0;

                docLinkCurrRate = GFunc.NEDec(grdRow.Cells["LinkDocCurrRate"].Value, 0);
                grdRow.Cells["ItmApplyDisAmtF"].Value = itmApplyDisAmtF;

                if (itmApplyDueAmtF == itmApplyDisAmtF + itmApplyDocAmtF)
                    grdRow.Cells["ItmApplyDisAmtH"].Value = GFunc.NEDec(grdRow.Cells["ItmApplyDueAmtH"].Value, 0) - GFunc.NEDec(grdRow.Cells["ItmApplyDocAmtH"].Value, 0);
                else
                    grdRow.Cells["ItmApplyDisAmtH"].Value = GFunc.RndC(itmApplyDisAmtF * docLinkCurrRate, GVar.RndDecs.Amtpt);

                return grdRow.Update();
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
        private static bool ItmApplyDocAmtF_CustomeUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                UltraGrid grd = null;
                UltraGridRow grdRow = null;
                int docCurrKey = 1;
                decimal itmApplyDisAmtF = 0;
                decimal itmApplyDocAmtF = 0;
                decimal itmApplyDueAmtF = 0;

                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grd);
                grdRow = grd.ActiveRow;
                DataRow NewRow =null;
                //validation check
                if (GFunc.IsNE(grdRow.Cells["ItmApplyDocAmtF"].Value))
                {
                    MsgBox.Show("Apply amount cannot be empty.");
                    return false;
                }
              

                docCurrKey = GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 0); // Ask Mic
                if (docCurrKey == 1 && GFunc.NEInt(grdRow.Cells["LinkDocCurrKey"].Value, 0) == 1)//Home currency // Mic Check - Should be use systemOption
                {
                    //Validation
                    itmApplyDueAmtF = GFunc.NEDec(grd.ActiveRow.Cells["ItmApplyDueAmtF"].Value, 0);
                    itmApplyDisAmtF = GFunc.NEDec(grd.ActiveRow.Cells["ItmApplyDisAmtF"].Value, 0);
                    itmApplyDocAmtF = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmApplyDocAmtF"].Value, 0), GVar.RndDecs.Amtpt); //Ask Mic


                    if (GFunc.IsBetweenDec(itmApplyDisAmtF + itmApplyDocAmtF, 0, itmApplyDueAmtF) == false)
                    {
                        MsgBox.Show("You have exceeded the amount due, please amend your apply amount");
                        return false;
                    }

                    if (itmApplyDueAmtF == (itmApplyDocAmtF + itmApplyDisAmtF))
                        grd.ActiveRow.Cells["ItmApplyFull"].Value = 1;
                    else
                        grd.ActiveRow.Cells["ItmApplyFull"].Value = 0;

                    //Set values to grid
                    grd.ActiveRow.Cells["ItmApplyRate"].Value = 1;
                    grd.ActiveRow.Cells["ItmApplyDocAmtF"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmApplyDocAmtF"].Value, 0), GVar.RndDecs.Amtpt);
                    grd.ActiveRow.Cells["ItmApplyDocAmtH"].Value = GFunc.NEDec(grd.ActiveRow.Cells["ItmApplyDocAmtF"].Value, 0);
                    grd.ActiveRow.Cells["ItmApplyPayAmtF"].Value = GFunc.NEDec(grd.ActiveRow.Cells["ItmApplyDocAmtF"].Value, 0);
                    grd.ActiveRow.Cells["ItmApplyPayAmtH"].Value = GFunc.NEDec(grd.ActiveRow.Cells["ItmApplyDocAmtF"].Value, 0);
                    grd.ActiveRow.Cells["ItmApplyGainAmt"].Value = 0;
                    return grdRow.Update();
                }
                else
                {
                    if (GFunc.NEDec(grd.ActiveRow.Cells["ItmApplyDocAmtF"].Value, 0) != 0)
                    {
                        frmPopupApplyGain frm = new frmPopupApplyGain(objDoc, ref grd);
                        frm.ShowDialog();
                        if (frm.DialogResult == DialogResult.OK)
                            return grdRow.Update();
                    }
                    else
                    {
                        grd.ActiveRow.Cells["ItmApplyRate"].Value = 1;
                        grd.ActiveRow.Cells["ItmApplyDocAmtF"].Value = 0;
                        grd.ActiveRow.Cells["ItmApplyDocAmtH"].Value = 0;
                        grd.ActiveRow.Cells["ItmApplyPayAmtF"].Value = 0;
                        grd.ActiveRow.Cells["ItmApplyPayAmtH"].Value = 0;
                        grd.ActiveRow.Cells["ItmApplyGainAmt"].Value = 0;
                        grd.ActiveRow.Cells["ItmApplyFull"].Value = 0;
                        return grdRow.Update();;
                    }
                }
                return false;
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
        private static bool ItmApplyFull_CustomeUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                UltraGrid grd = null;
                UltraGridRow grdRow = null;
                int docCurrKey = 1;
                decimal itmApplyDisAmtF = 0;
                decimal itmApplyDocAmtF = 0;
                decimal itmApplyDueAmtF = 0;
                bool itmApplyFull = false;
                decimal maxAmountF = 0;

                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grd);
                grdRow = grd.ActiveRow;
                docCurrKey = GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 0);

                #region set variables
                itmApplyFull = GFunc.IsNE(grd.ActiveRow.Cells["ItmApplyFull"].Value) == true ? false : (bool)grd.ActiveRow.Cells["ItmApplyFull"].Value;
                itmApplyDueAmtF = GFunc.NEDec(grd.ActiveRow.Cells["ItmApplyDueAmtF"].Value, 0); //Ask Mic
                itmApplyDisAmtF = GFunc.NEDec(grd.ActiveRow.Cells["ItmApplyDisAmtF"].Value, 0);// Ask Mic
                itmApplyDocAmtF = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmApplyDocAmtF"].Value, 0), GVar.RndDecs.Amtpt); //Ask Mic
                #endregion

                if (docCurrKey == 1 && GFunc.NEInt(grdRow.Cells["LinkDocCurrKey"].Value, 0) == 1)//Home currency
                {
                    #region set max apply amount to grid
                    if (itmApplyFull)
                    {
                        if (itmApplyDueAmtF >= 0)
                            maxAmountF = GFunc.NEDec(GFunc.RndC(itmApplyDueAmtF - itmApplyDisAmtF, GVar.RndDecs.Amtpt), 0);
                        else
                            maxAmountF = GFunc.NEDec(GFunc.RndC(itmApplyDueAmtF + itmApplyDisAmtF, GVar.RndDecs.Amtpt), 0);

                        if (maxAmountF == 0)
                            return true;
                        else
                        {
                            //Set values to grid
                            grd.ActiveRow.Cells["ItmApplyRate"].Value = 1;
                            grd.ActiveRow.Cells["ItmApplyDocAmtF"].Value = maxAmountF;
                            grd.ActiveRow.Cells["ItmApplyDocAmtH"].Value = maxAmountF;
                            grd.ActiveRow.Cells["ItmApplyPayAmtF"].Value = maxAmountF;
                            grd.ActiveRow.Cells["ItmApplyPayAmtH"].Value = maxAmountF;
                            grd.ActiveRow.Cells["ItmApplyGainAmt"].Value = 0;
                            return grdRow.Update();
                        }
                    }
                    else
                    {
                        //Set values to grid
                        grd.ActiveRow.Cells["ItmApplyRate"].Value = 1;
                        grd.ActiveRow.Cells["ItmApplyDocAmtF"].Value = 0;
                        grd.ActiveRow.Cells["ItmApplyDocAmtH"].Value = 0;
                        grd.ActiveRow.Cells["ItmApplyPayAmtF"].Value = 0;
                        grd.ActiveRow.Cells["ItmApplyPayAmtH"].Value = 0;
                        grd.ActiveRow.Cells["ItmApplyGainAmt"].Value = 0;
                        return grdRow.Update();
                    }
                    #endregion
                }
                else
                {
                    if (itmApplyFull)
                    {
                        frmPopupApplyGain frm = new frmPopupApplyGain(objDoc, ref grd);
                        frm.ShowDialog();
                        if (frm.DialogResult == DialogResult.OK)
                            return grdRow.Update();
                    }
                    else
                    {
                        grdRow.Cells["ItmApplyRate"].Value = 1;
                        grdRow.Cells["ItmApplyDocAmtF"].Value = 0;
                        grdRow.Cells["ItmApplyDocAmtH"].Value = 0;
                        grdRow.Cells["ItmApplyPayAmtF"].Value = 0;
                        grdRow.Cells["ItmApplyPayAmtH"].Value = 0;
                        grdRow.Cells["ItmApplyGainAmt"].Value = 0;
                        return grdRow.Update();
                    }
                }
                return false;
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
        public static bool ItmAmtShw_CustomeUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                #region Declare variables
                bool RunCalculateAR = false;
                bool RunCalculateAP = false;
                bool RunNullChargeItemQty = false;
                bool RunCalculateTax = false;
                int ItmType = 0;

                bool HaveTaxFields = false;
                int? DocTaxKey = 0;
                decimal? DocTaxRate = 0;
                decimal? DocCurrRate = 1;
                decimal? DocCountryRate = 1;

                decimal? ItmPriceAfter = 0;
                decimal? ItmDisPercent = 0;
                decimal? ItmDisValue = 0;
                decimal? ItmPriceUser = 0;
                decimal? ItmPrice = 0;
                decimal? ItmAmtShw = 0;
                decimal? ItmAmtF = 0;
                decimal? ItmAmtH = 0;
                int? ItmTaxGrpKey = 0;
                decimal? ItmTaxGrpRate = 0;
                decimal? ItmTaxGrpAmtF = 0;
                decimal? ItmTaxGrpAmtL = 0;
                #endregion

                #region Check DocCodeKey if any calculation is to be run
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
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        HaveTaxFields = true;
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                switch (ItmType)
                {
                    case (int)GEnum.INTypeGrp.Charges:
                    case (int)GEnum.INTypeGrp.Discount:
                        grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;
                        grd.ActiveRow.Cells["ItmAmtShw"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmAmtShw"].Value, 0), GVar.RndDecs.Amtpt);
                        break;

                    case (int)GEnum.INTypeGrp.Remark:
                        return true;

                    default:
                        MsgBox.Show(MsgID.Document.NotAllowedInput + "%Amount");
                        grd.ActiveRow.Cells["ItmAmtShw"].Value = DBNull.Value;
                        return false;
                }
                #endregion

                if (Check_RunProcess((int)objDoc.DocCodeKey, grd, ref RunCalculateAR, ref RunCalculateAP, ref RunNullChargeItemQty, ref RunCalculateTax) == false)
                    return false;

                #region Calculation
                if (RunCalculateAR || RunCalculateAP)
                {
                    #region set variables
                    if (HaveTaxFields)
                    {
                        ItmTaxGrpRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmTaxGrpRate"].Value, 0);
                        DocTaxKey = (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc);
                        DocTaxRate = GFunc.NEDec((decimal?)GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);
                    }
                    else
                    {
                        ItmTaxGrpKey = 0;
                        ItmTaxGrpRate = 0;
                        DocTaxKey = 0;
                        DocTaxRate = 0;
                    }

                    DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                    if (SysOptionUtility.GetInt("CountryCurrency") == 1) // Country Currency = Home -- Added by Jane. Mic to check
                        DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                    else
                        DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);

                    ItmPriceAfter = GFunc.NEDec(grd.ActiveRow.Cells["ItmPriceAfter"].Value, 0);
                    ItmAmtShw = GFunc.NEDec(grd.ActiveRow.Cells["ItmAmtShw"].Value, 0);
                    ItmPriceUser = ItmAmtShw;
                    #endregion

                    #region Standard Calculation
                    ItmDisValue = ItmPriceAfter - ItmPriceUser;
                    ItmDisPercent = GFunc.RndDC(ItmDisValue, ItmPriceUser, GVar.RndDecs.Prcpt);
                    if (objDoc.DocType == 110)   //if Tax Inclusive,
                    {
                        if (ItmTaxGrpRate > 0)
                            if (DocTaxRate == 0)
                                ItmPrice = ItmPriceUser;
                            else
                                ItmPrice = GFunc.RndDC(ItmPriceUser, (1 + ItmTaxGrpRate), GVar.RndDecs.Prcpt);
                        else
                            ItmPrice = ItmPriceUser;
                    }
                    else
                    {
                        ItmPrice = ItmPriceUser;
                    }

                    ItmAmtF = ItmPrice;
                    ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                    #endregion

                    #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                    if (RunCalculateTax)
                    {
                        if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                        {
                            //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                            ItmTaxGrpKey = DocTaxKey;
                            ItmTaxGrpRate = 0;
                            ItmTaxGrpAmtF = 0;
                            ItmTaxGrpAmtL = 0;
                        }
                        else
                        {
                            if (ItmTaxGrpRate > 0)
                            {
                                ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt );
                                ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                            }
                            else
                            {
                                ItmTaxGrpAmtF = 0;
                                ItmTaxGrpAmtL = 0;
                            }
                        }
                    }
                    #endregion

                    #region Set grid with calculated values
                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                    {
                        grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;
                        grd.ActiveRow.Cells["ItmDisPercent"].Value = ItmDisPercent.ToDBValue();
                        grd.ActiveRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue();
                        grd.ActiveRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue();
                        grd.ActiveRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue();
                        grd.ActiveRow.Cells["ItmPrice"].Value = ItmPrice;
                        grd.ActiveRow.Cells["ItmAmtF"].Value = ItmAmtF;
                        grd.ActiveRow.Cells["ItmAmtH"].Value = ItmAmtH;
                        if (HaveTaxFields)
                        {
                            grd.ActiveRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                            grd.ActiveRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;
                        }
                    }
                    else //assume Discount
                    {
                        grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;
                        grd.ActiveRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue();
                        grd.ActiveRow.Cells["ItmPrice"].Value = ItmPrice;
                        grd.ActiveRow.Cells["ItmAmtF"].Value = ItmAmtF;
                        grd.ActiveRow.Cells["ItmAmtH"].Value = ItmAmtH;
                        if (HaveTaxFields)
                        {
                            grd.ActiveRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                            grd.ActiveRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;
                        }
                    }

                    #endregion
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
        public static bool ItmAmtShwTransfer_CustomeUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                #region Declare variables
                bool RunCalculateAR = false;
                bool RunCalculateAP = false;
                bool RunNullChargeItemQty = false;
                bool RunCalculateTax = false;
                int ItmType = 0;

                bool HaveTaxFields = false;
                int? DocTaxKey = 0;
                decimal? DocTaxRate = 0;
                decimal? DocCurrRate = 1;
                decimal? DocCountryRate = 1;

                decimal? ItmPriceAfter = 0;
                decimal? ItmDisPercent = 0;
                decimal? ItmDisValue = 0;
                decimal? ItmPriceUser = 0;
                decimal? ItmPrice = 0;
                decimal? ItmAmtShw = 0;
                decimal? ItmAmtF = 0;
                decimal? ItmAmtH = 0;
                int? ItmTaxGrpKey = 0;
                decimal? ItmTaxGrpRate = 0;
                decimal? ItmTaxGrpAmtF = 0;
                decimal? ItmTaxGrpAmtL = 0;
                #endregion

                #region Check DocCodeKey if any calculation is to be run
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
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        HaveTaxFields = true;
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                switch (ItmType)
                {
                    case (int)GEnum.INTypeGrp.Charges:
                    case (int)GEnum.INTypeGrp.Discount:
                        //[grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;]
                        grd.ActiveRow.Cells["ItmAmtShw"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmAmtShw"].Value, 0), GVar.RndDecs.Amtpt);
                        break;

                    case (int)GEnum.INTypeGrp.Remark:
                        return true;

                    default:
                        MsgBox.Show(MsgID.Document.NotAllowedInput + "%Amount");
                        grd.ActiveRow.Cells["ItmAmtShw"].Value = DBNull.Value;
                        return false;
                }
                #endregion

                if (Check_RunProcess((int)objDoc.DocCodeKey, grd, ref RunCalculateAR, ref RunCalculateAP, ref RunNullChargeItemQty, ref RunCalculateTax) == false)
                    return false;

                #region Calculation
                if (RunCalculateAR || RunCalculateAP)
                {
                    #region set variables
                    if (HaveTaxFields)
                    {
                        ItmTaxGrpRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmTaxGrpRate"].Value, 0);
                        DocTaxKey = (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc);
                        DocTaxRate = GFunc.NEDec((decimal?)GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);
                    }
                    else
                    {
                        ItmTaxGrpKey = 0;
                        ItmTaxGrpRate = 0;
                        DocTaxKey = 0;
                        DocTaxRate = 0;
                    }

                    DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                    if (SysOptionUtility.GetInt("CountryCurrency") == 1) // Country Currency = Home -- Added by Jane. Mic to check
                        DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                    else
                        DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);

                    ItmPriceAfter = GFunc.NEDec(grd.ActiveRow.Cells["ItmPriceAfter"].Value, 0);
                    ItmAmtShw = GFunc.NEDec(grd.ActiveRow.Cells["ItmAmtShw"].Value, 0);
                    ItmPriceUser = ItmAmtShw;
                    #endregion

                    #region Standard Calculation
                    ItmDisValue = ItmPriceAfter - ItmPriceUser;
                    ItmDisPercent = GFunc.RndDC(ItmDisValue, ItmPriceUser, GVar.RndDecs.Prcpt);
                    if (objDoc.DocType == 110)   //if Tax Inclusive,
                    {
                        if (ItmTaxGrpRate > 0)
                            if (DocTaxRate == 0)
                                ItmPrice = ItmPriceUser;
                            else
                                ItmPrice = GFunc.RndDC(ItmPriceUser, (1 + ItmTaxGrpRate), GVar.RndDecs.Prcpt);
                        else
                            ItmPrice = ItmPriceUser;
                    }
                    else
                    {
                        ItmPrice = ItmPriceUser;
                    }

                    ItmAmtF = ItmPrice;
                    ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                    #endregion

                    #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                    if (RunCalculateTax)
                    {
                        if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                        {
                            //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                            ItmTaxGrpKey = DocTaxKey;
                            ItmTaxGrpRate = 0;
                            ItmTaxGrpAmtF = 0;
                            ItmTaxGrpAmtL = 0;
                        }
                        else
                        {
                            if (ItmTaxGrpRate > 0)
                            {
                                ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                                ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                            }
                            else
                            {
                                ItmTaxGrpAmtF = 0;
                                ItmTaxGrpAmtL = 0;
                            }
                        }
                    }
                    #endregion

                    #region Set grid with calculated values
                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                    {
                        //[grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;]
                        grd.ActiveRow.Cells["ItmDisPercent"].Value = ItmDisPercent.ToDBValue();
                        grd.ActiveRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue();
                        grd.ActiveRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue();
                        grd.ActiveRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue();
                        grd.ActiveRow.Cells["ItmPrice"].Value = ItmPrice;
                        grd.ActiveRow.Cells["ItmAmtF"].Value = ItmAmtF;
                        grd.ActiveRow.Cells["ItmAmtH"].Value = ItmAmtH;
                        if (HaveTaxFields)
                        {
                            grd.ActiveRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                            grd.ActiveRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;
                        }
                    }
                    else //assume Discount
                    {
                        //[grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;]
                        grd.ActiveRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue();
                        grd.ActiveRow.Cells["ItmPrice"].Value = ItmPrice;
                        grd.ActiveRow.Cells["ItmAmtF"].Value = ItmAmtF;
                        grd.ActiveRow.Cells["ItmAmtH"].Value = ItmAmtH;
                        if (HaveTaxFields)
                        {
                            grd.ActiveRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                            grd.ActiveRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;
                        }
                    }

                    #endregion
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
        private static bool ItmCost_CustomeUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                decimal itmCost = 0M;

                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Inventory_Adjustment)
                {
                    itmCost = GFunc.RndC(grd.ActiveRow.Cells["ItmCost"].Value, GVar.RndDecs.Prcpt);
                    if (itmCost < 0) itmCost = 0;

                    grd.ActiveRow.Cells["ItmCost"].Value = itmCost;
                }
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

        private static bool ItmCreditF_CustomeUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                UltraGridRow row = null;

                if (grd.ActiveRow.Cells["ItmAccKey"] != null)
                {
                    row = grd.ActiveRow;
                    decimal creditF = GFunc.NEDec(row.Cells["ItmCreditF"].Value, 0);
                    decimal currRate = GFunc.NEDec(row.Cells["ItmCurrRate"].Value, 1);
                    row.Cells["ItmCreditH"].Value = GFunc.RndC(creditF * currRate, GVar.RndDecs.Amtpt);
                    row.Cells["ItmDebitF"].Value = 0;
                    row.Cells["ItmDebitH"].Value = 0;
                    return grd.ActiveRow.Update();
                }
                return false;
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
        private static bool ItmCreditH_CustomeUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                UltraGridRow row = null;
                int HomeCurrKey = SysOptionUtility.BaseCurrency;
                if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmAccKey"].Value) == false)
                {
                    row = grd.ActiveRow;
                    decimal creditH = GFunc.NEDec(row.Cells["ItmCreditH"].Value, 0);
                    decimal currRate = GFunc.NEDec(row.Cells["ItmCurrRate"].Value, 1);
                    if (GFunc.NEInt(row.Cells["ItmCurrKey"].Value, 1) == HomeCurrKey) //home currency 
                    {
                        row.Cells["ItmCurrRate"].Value = 1;// //not ready - check Mic -- modified by Jane  
                        row.Cells["ItmCreditF"].Value = creditH;//not ready - check Mic -- modified by Jane  
                    }
                    else
                    {                        
                        if (GFunc.NEDec(row.Cells["ItmCreditF"].Value, 0) == 0)
                        {
                            row.Cells["ItmCreditF"].Value = row.Cells["ItmDebitF"].Value;
                            row.Cells["ItmDebitF"].Value = 0;
                        }
                        row.Cells["ItmCurrRate"].Value = GFunc.RndC(creditH / GFunc.NEDec(row.Cells["ItmCreditF"].Value, 0), GVar.RndDecs.Curpt);// check Mic -- modified by Jane 
                        //row.Cells["ItmCreditF"].Value = GFunc.RndDC(creditH, currRate, GVar.RndDecs.Amtpt); /not ready - check mic
                       
                    }
                    row.Cells["ItmDebitF"].Value = 0;
                    row.Cells["ItmDebitH"].Value = 0;
                    return grd.ActiveRow.Update();
                }
                return false;

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
        private static bool ItmCurrKey_CustomeUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                UltraGridRow row = null;

                if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmAccKey"].Value) == false)
                {
                    row = grd.ActiveRow;
                    int itmCurrKey = GFunc.NEInt(row.Cells["ItmCurrKey"].Value, 0);
                    grd.ActiveRow.Cells["ItmCurrRate"].Value = DocComUtility.CurrRate_Get(itmCurrKey, objDoc.DocDate, true);

                    DocDetUtil.ItmCurrRate_CustomeUpdate(objDoc, grd);
                    DocHDRUtil.FormGridLock_Set(objDoc, grd, GEnum.Details.Doc_Itm, false);
                }
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
        private static bool ItmCurrRate_CustomeUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                UltraGridRow row = null;

                if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmAccKey"].Value) == false)
                {
                    row = grd.ActiveRow;
                    decimal creditF = GFunc.NEDec(row.Cells["ItmCreditF"].Value, 0);
                    decimal debitF = GFunc.NEDec(row.Cells["ItmDebitF"].Value, 0);
                    decimal currRate = GFunc.NEDec(row.Cells["ItmCurrRate"].Value, 1);
                    row.Cells["ItmCreditH"].Value = GFunc.RndC(creditF * currRate, GVar.RndDecs.Amtpt);
                    row.Cells["ItmDebitH"].Value = GFunc.RndC(debitF * currRate, GVar.RndDecs.Amtpt);
                    return grd.ActiveRow.Update();
                }
                return false;
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
        private static bool ItmDebitF_CustomeUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                UltraGridRow row = null;

                if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmAccKey"].Value) == false)
                {
                    row = grd.ActiveRow;
                    decimal debitF = GFunc.NEDec(row.Cells["ItmDebitF"].Value, 0);
                    decimal currRate = GFunc.NEDec(row.Cells["ItmCurrRate"].Value, 1);
                    row.Cells["ItmDebitH"].Value = GFunc.RndC(debitF * currRate, GVar.RndDecs.Amtpt);
                    row.Cells["ItmCreditF"].Value = 0;
                    row.Cells["ItmCreditH"].Value = 0;
                    return grd.ActiveRow.Update();
                }
                return false;
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
        private static bool ItmDebitH_CustomeUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                UltraGridRow row = null;
                int HomeCurrKey = SysOptionUtility.BaseCurrency;

                if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmAccKey"].Value) == false)
                {
                    row = grd.ActiveRow;
                    decimal debitH = GFunc.NEDec(row.Cells["ItmDebitH"].Value, 0);
                    decimal currRate = GFunc.NEDec(row.Cells["ItmCurrRate"].Value, 1);

                    if (GFunc.NEInt(row.Cells["ItmCurrKey"].Value, 1) == HomeCurrKey) //home currency 
                    {
                        row.Cells["ItmCurrRate"].Value = 1;// not ready - check Mic -- modified by Jane on 06-Sep-2013
                        row.Cells["ItmDebitF"].Value = debitH; //not ready - check Mic -- modified by Jane on 06-Sep-2013
                    }
                    else
                    {
                        if (GFunc.NEDec(row.Cells["ItmDebitF"].Value, 0) == 0)
                        {
                            row.Cells["ItmDebitF"].Value = row.Cells["ItmCreditF"].Value;
                            row.Cells["ItmCreditF"].Value = 0;
                        }
                        row.Cells["ItmCurrRate"].Value = GFunc.RndC(debitH / GFunc.NEDec(row.Cells["ItmDebitF"].Value, 0), GVar.RndDecs.Curpt);//not ready - check Mic -- modified by Jane on 06-Sep-2013
                        //row.Cells["ItmDebitF"].Value = GFunc.RndDC(debitH, currRate, GVar.RndDecs.Amtpt); //not ready - check Mic -- modified by Jane on 06-Sep-2013
                        
                    }
                    
                    row.Cells["ItmCreditF"].Value = 0;
                    row.Cells["ItmCreditH"].Value = 0;
                    return grd.ActiveRow.Update();
                }
                return false;

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

        private static bool ItmDes_DblClick(Document objDoc, UltraGrid grd)
        {
            try
            {
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Purchase_Order:
                        if (GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, 0) == (int)GEnum.ItemType.Assembly)
                            GlobalUI.PopupDisplayBA(GlobalUI.Form_Name.FRM_ASSEMBLYENTRY, (TAUtil.TAGridEditor)grd);
                        break;

                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        switch (GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, 0))
                        {
                            case (int)GEnum.ItemType.StockB:
                            case (int)GEnum.ItemType.Finished_GDB:
                            case (int)GEnum.ItemType.Serial_Finished_GDB:
                                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Inventory_Production)
                                {
                                    GlobalUI.PopupDisplayBA(GlobalUI.Form_Name.FRM_BATCHSELECTION, (TAUtil.TAGridEditor)grd);
                                }
                                else
                                {
                                    GlobalUI.PopupDisplayBA(GlobalUI.Form_Name.FRM_BATCHENTRY, (TAUtil.TAGridEditor)grd);
                                }
                                break;

                            case (int)GEnum.ItemType.Assembly:
                                GlobalUI.PopupDisplayBA(GlobalUI.Form_Name.FRM_ASSEMBLYENTRY, (TAUtil.TAGridEditor)grd);
                                break;
                        }
                        break;
                }
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
        private static bool ItmDeptKey_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                        //do nothing as there are nothing to check
                        break;
                    default:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
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
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        switch (GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value))
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                            case (int)GEnum.INTypeGrp.Charges:
                            case (int)GEnum.INTypeGrp.Discount:
                            case (int)GEnum.INTypeGrp.Remark:
                            case (int)GEnum.INTypeGrp.Empty:
                                grd.ActiveRow.Cells["ItmDeptKey"].Value = GFunc.NEInt(grd.ActiveRow.Cells["ItmDeptKey"].Value, 0);
                                return true;

                            default:
                                MsgBox.Show(MsgID.Document.NotAllowedInput + "%Department");
                                grd.ActiveRow.Cells["ItmDeptKey"].Value = 0;
                                return false;
                        }

                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Journal:
                        grd.ActiveRow.Cells["ItmDeptKey"].Value = GFunc.NEInt(grd.ActiveRow.Cells["ItmDeptKey"].Value, 0);
                        break;
                }
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
        private static bool ItmDisPercent_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                #region Declare variables
                bool RunCalculateAR = false;
                bool RunCalculateAP = false;
                bool RunCalculateTax = false;
                bool RunNullChargeItemQty = false;

                bool HaveTaxFields = false;
                int? DocTaxKey = 0;
                decimal? DocTaxRate = 0;
                decimal? DocCurrRate = 1;
                decimal? DocCountryRate = 1;

                int? ItmType = 0;
                decimal? ItmQty = 0;
                decimal? ItmPriceAfter = 0;
                decimal? ItmDisPercent = 0;
                decimal? ItmDisValue = 0;
                decimal? ItmPriceUser = 0;
                decimal? ItmAmtShw = 0;
                decimal? ItmPrice = 0;
                decimal? ItmAmtF = 0;
                decimal? ItmAmtH = 0;
                int? ItmTaxGrpKey = 0;
                decimal? ItmTaxGrpRate = 0;
                decimal? ItmTaxGrpAmtF = 0;
                decimal? ItmTaxGrpAmtL = 0;
                decimal? ItmVendorPrice = 0;
                decimal? ItmTotalCost = 0;
                decimal? ItmGP = 0;

                int PriceDec = 0; //added by jane on 9-Jun-2014
                int PriceRoundMode = 0;//added by jane on 9-Jun-2014
                #endregion

                #region Check DocCodeKey if any calculation is to be run
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
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        HaveTaxFields = false;
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        break;

                    case (int)GEnum.SystemCode.Consignment_Settlement:
                        ItmDisPercent = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmDisPercent"].Value, 0), GVar.RndDecs.Prcpt);
                        if (ItmDisPercent < 0)
                        {
                            MsgBox.Show("discount percentage cannot be < 0");
                            return false;
                        }
                        return ItmPrice_CustomUpdate(objDoc, grd);

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                switch (ItmType)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                    case (int)GEnum.INTypeGrp.Charges:
                        grd.ActiveRow.Cells["ItmDisPercent"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmDisPercent"].Value, 0), GVar.RndDecs.Prcpt);
                        ItmVendorPrice = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorPrice"].Value, 0);
                        break;

                    case (int)GEnum.INTypeGrp.Remark:
                        return true;

                    default:
                        //reset user input value
                        MsgBox.Show(MsgID.Document.NotAllowedInput + "%Discount Percent");
                        grd.ActiveRow.Cells["ItmDisPercent"].Value = DBNull.Value;
                        return false;
                }
                #endregion

                if (Check_RunProcess((int)objDoc.DocCodeKey, grd, ref RunCalculateAR, ref RunCalculateAP, ref RunNullChargeItemQty, ref RunCalculateTax) == false)
                    return false;

                #region run NullChargeItemQty
                if (RunNullChargeItemQty)
                    grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;
                #endregion

                #region run RunCalculateARAP
                if (RunCalculateAR || RunCalculateAP)
                {
                    #region set variables
                    if (HaveTaxFields)
                    {
                        ItmTaxGrpRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmTaxGrpRate"].Value, 0);
                        DocTaxKey = GFunc.NEInt(GFunc.GetPropertyValue("DocTaxGrpKey", objDoc), 0);
                        DocTaxRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);
                    }
                    else
                    {
                        ItmTaxGrpKey = 0;
                        ItmTaxGrpRate = 0;
                        DocTaxKey = 0;
                        DocTaxRate = 0;
                    }

                    DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                    if (SysOptionUtility.GetInt("CountryCurrency") == 1) // Country Currency = Home -- Added by Jane. Mic to check
                        DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                    else
                        DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);

                    ItmQty = GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0);
                    ItmPriceAfter = GFunc.NEDec(grd.ActiveRow.Cells["ItmPriceAfter"].Value, 0);
                    ItmDisPercent = GFunc.NEDec(grd.ActiveRow.Cells["ItmDisPercent"].Value, 0);

                    PriceDec = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceDecPlace);
                    PriceRoundMode = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceRoundMode);
                    #endregion

                    #region Standard Calculation
                    //ItmDisValue = GFunc.RndC(ItmPriceAfter * ItmDisPercent / 100M, GVar.RndDecs.Prcpt);// commented by Jane on 9-Jun-2014
                    //ItmPriceUser = ItmPriceAfter - GFunc.NEDec(ItmDisValue, DBNull.Value);// commented by Jane on 9-Jun-2014

                    ItmDisValue = GFunc.RndUD(ItmPriceAfter * ItmDisPercent / 100M, PriceRoundMode, PriceDec); // added by Jane on  on 9-Jun-2014
                    ItmPriceUser = GFunc.RndUD(ItmPriceAfter - ItmDisValue, PriceRoundMode, PriceDec);// added by Jane on  on 9-Jun-2014

                    //---added by jane on 9-Jun-2014. Mic need to check!
                    //for eg. if itmpriceafter is 75.99. and the pricedec option is WHOLE NUMBER and priceroundmode is roundup, then calculated priceuser become 76
                    //so the difference of priceafter and priceuser will be the discount. 75.99 - 76 = -0.01
                    //Remark: if priceroundmode is roundup , discount will be (-)value and if rounddown, will be (+)value.
                    if (ItmPriceAfter - ItmDisValue != ItmPriceUser)
                    {
                        ItmDisValue = ItmPriceAfter - ItmPriceUser;
                    }
                    //----------------------

                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                        ItmAmtShw = ItmPriceUser;
                    else
                        ItmAmtShw = GFunc.RndC(ItmPriceUser * ItmQty, GVar.RndDecs.Amtpt);

                    if (objDoc.DocType == 110)   //if Tax Inclusive,
                    {
                        if (ItmTaxGrpRate > 0)
                            if (DocTaxRate == 0)
                                ItmPrice = ItmPriceUser;
                            else
                                ItmPrice = GFunc.RndDC(ItmPriceUser, (1 + ItmTaxGrpRate), GVar.RndDecs.Prcpt);
                        else
                            ItmPrice = ItmPriceUser;
                    }
                    else
                    {
                        ItmPrice = ItmPriceUser;
                    }

                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                        ItmAmtF = ItmPrice;
                    else
                        ItmAmtF = GFunc.RndC(ItmPrice * ItmQty, GVar.RndDecs.Amtpt);

                    ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                    #endregion

                    #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                    if (RunCalculateTax)
                    {
                        if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                        {
                            //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                            ItmTaxGrpKey = DocTaxKey;
                            ItmTaxGrpRate = 0;
                            ItmTaxGrpAmtF = 0;
                            ItmTaxGrpAmtL = 0;
                        }
                        else
                        {
                            if (ItmTaxGrpRate > 0)
                            {
                                ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt );
                                ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                            }
                            else
                            {
                                ItmTaxGrpAmtF = 0;
                                ItmTaxGrpAmtL = 0;
                            }
                        }
                    }
                    #endregion

                    #region Set grid with calculated values
                    grd.ActiveRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue();
                    grd.ActiveRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue();
                    grd.ActiveRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue();
                    grd.ActiveRow.Cells["ItmPrice"].Value = ItmPrice;
                    grd.ActiveRow.Cells["ItmAmtF"].Value = ItmAmtF;
                    grd.ActiveRow.Cells["ItmAmtH"].Value = ItmAmtH;
                    if (HaveTaxFields)
                    {
                        grd.ActiveRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                        grd.ActiveRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;
                    }
                    if (SysOptionUtility.DatabaseBranchCode == DBCode.ADL && objDoc.DocCodeKey == (int)GEnum.SystemCode.Quotation) /* added by YST on 2023/07/15 -- requested by Zaw from Athena */
                    {
                        ItmTotalCost = GFunc.RndC(ItmQty * ItmVendorPrice, GVar.RndDecs.Amtpt);
                        ItmGP = ItmAmtShw - ItmTotalCost;
                        grd.ActiveRow.Cells["Custom1"].Value = String.Format("{0: #,##0.00; (#,##0.00)} ", ItmTotalCost);
                        grd.ActiveRow.Cells["Custom2"].Value = String.Format("{0: #,##0.00; (#,##0.00)} ", ItmGP);
                        if (ItmAmtShw > 0)
                        {
                            grd.ActiveRow.Cells["Custom3"].Value = GFunc.RndC((ItmGP / ItmAmtShw) * 100, GVar.RndDecs.Amtpt).ToString() + "%"; /* Margin (%) for ADPL according to Zaw's formula */
                        }
                    }
                    #endregion
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
        private static bool ItmDisPrice_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                decimal docCurrRate = 1;
                decimal itmQty = 0;
                decimal itmPrice = 0;
                decimal? itmDisPercent = 0;
                decimal itmDisPrice = 0;
                decimal itmAmtF = 0;
                decimal itmAmtH = 0;
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        docCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                        itmQty = GFunc.NEDec(grd.ActiveRow.Cells["itmQty"].Value, 0); 
                        itmPrice = GFunc.NEDec(grd.ActiveRow.Cells["ItmPrice"].Value, 0); 
                        itmDisPrice = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmDisPrice"].Value, 0), GVar.RndDecs.Prcpt);
                        if (itmDisPrice < 0)
                        {
                            MsgBox.Show("discount price cannot be < 0");
                            return false;
                        }
                        if (itmDisPrice > itmPrice)
                        {
                            MsgBox.Show("discount price cannot be > price");
                            return false;
                        }
                        itmDisPercent = 1M - GFunc.NEDec(GFunc.RndDC(itmDisPrice, itmPrice, GVar.RndDecs.Prcpt), 0); //Ask Mic
                        itmAmtF = GFunc.NEDec(GFunc.RndC(itmDisPrice * itmQty, GVar.RndDecs.Amtpt), 0);
                        itmAmtH = GFunc.NEDec(GFunc.RndC(itmAmtF * docCurrRate, GVar.RndDecs.Amtpt), 0);

                        grd.ActiveRow.Cells["ItmDisPercent"].Value = itmDisPercent.ToDBValue();
                        grd.ActiveRow.Cells["ItmDisPrice"].Value = itmDisPrice;
                        grd.ActiveRow.Cells["ItmAmtF"].Value = itmAmtF;
                        grd.ActiveRow.Cells["ItmAmtH"].Value = itmAmtH;
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
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

        private static bool ItmDisValue_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                #region Declare variables
                bool RunCalculateAR = false;
                bool RunCalculateAP = false;
                bool RunCalculateTax = false;
                bool RunNullChargeItemQty = false;

                bool HaveTaxFields = false;
                int? DocTaxKey = 0;
                decimal? DocTaxRate = 0;
                decimal? DocCurrRate = 1;
                decimal? DocCountryRate = 1;

                int? ItmType = 0;
                decimal? ItmQty = 0;
                decimal? ItmPriceAfter = 0;
                decimal? ItmDisPercent = 0;
                decimal? ItmDisValue = 0;
                decimal? ItmPriceUser = 0;
                decimal? ItmAmtShw = 0;
                decimal? ItmPrice = 0;
                decimal? ItmAmtF = 0;
                decimal? ItmAmtH = 0;
                int? ItmTaxGrpKey = 0;
                decimal? ItmTaxGrpRate = 0;
                decimal? ItmTaxGrpAmtF = 0;
                decimal? ItmTaxGrpAmtL = 0;
                #endregion

                #region Check DocCodeKey if any calculation is to be run
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
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return true;
                        }
                        HaveTaxFields = true;
                        break;


                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                switch (ItmType)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                    case (int)GEnum.INTypeGrp.Charges:
                        grd.ActiveRow.Cells["ItmDisValue"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmDisValue"].Value, 0), GVar.RndDecs.Prcpt);
                        break;

                    case (int)GEnum.INTypeGrp.Remark:
                        return true;

                    default:
                        //reset user input value
                        MsgBox.Show(MsgID.Document.NotAllowedInput + "%Discount Value");
                        grd.ActiveRow.Cells["ItmDisValue"].Value = DBNull.Value;
                        return true;
                }
                #endregion

                if (Check_RunProcess((int)objDoc.DocCodeKey, grd, ref RunCalculateAR, ref RunCalculateAP, ref RunNullChargeItemQty, ref RunCalculateTax) == false)
                    return false;

                #region run NullChargeItemQty
                if (RunNullChargeItemQty)
                    grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;
                #endregion

                #region run RunCalculateARAP
                if (RunCalculateAR || RunCalculateAP)
                {
                    #region set variables
                    if (HaveTaxFields)
                    {
                        ItmTaxGrpRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmTaxGrpRate"].Value, 0);
                        DocTaxKey = (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc);
                        DocTaxRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);
                    }
                    else
                    {
                        ItmTaxGrpKey = 0;
                        ItmTaxGrpRate = 0;
                        DocTaxKey = 0;
                        DocTaxRate = 0;
                    }

                    DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                    if (SysOptionUtility.GetInt("CountryCurrency") == 1) // Country Currency = Home -- Added by Jane. Mic to check
                        DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                    else
                        DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);


                    ItmQty = GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0);
                    ItmPriceAfter = GFunc.NEDec(grd.ActiveRow.Cells["ItmPriceAfter"].Value, 0);
                    ItmDisValue = GFunc.NEDec(grd.ActiveRow.Cells["ItmDisValue"].Value, 0);
                    #endregion

                    #region Standard Calculation
                    ItmDisPercent = GFunc.RndDC(ItmDisValue * 100M, ItmPriceAfter, GVar.RndDecs.Prcpt);
                    ItmPriceUser = ItmPriceAfter - ItmDisValue;
                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                        ItmAmtShw = ItmPriceUser;
                    else
                        ItmAmtShw = GFunc.RndC(ItmPriceUser * ItmQty, GVar.RndDecs.Amtpt);

                    if (objDoc.DocType == 110)   //if Tax Inclusive,
                    {
                        if (ItmTaxGrpRate > 0)
                            if (DocTaxRate == 0)
                                ItmPrice = ItmPriceUser;
                            else
                                ItmPrice = GFunc.RndDC(ItmPriceUser, (1 + ItmTaxGrpRate), GVar.RndDecs.Prcpt);
                        else
                            ItmPrice = ItmPriceUser;
                    }
                    else
                    {
                        ItmPrice = ItmPriceUser;
                    }

                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                        ItmAmtF = ItmPrice;
                    else
                        ItmAmtF = GFunc.RndC(ItmPrice * ItmQty, GVar.RndDecs.Amtpt);

                    ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                    #endregion

                    #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                    if (RunCalculateTax)
                    {
                        if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                        {
                            //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                            ItmTaxGrpKey = DocTaxKey;
                            ItmTaxGrpRate = 0;
                            ItmTaxGrpAmtF = 0;
                            ItmTaxGrpAmtL = 0;
                        }
                        else
                        {
                            if (ItmTaxGrpRate > 0)
                            {
                                ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt );
                                ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                            }
                            else
                            {
                                ItmTaxGrpAmtF = 0;
                                ItmTaxGrpAmtL = 0;
                            }
                        }
                    }
                    #endregion

                    #region Set grid with calculated values
                    grd.ActiveRow.Cells["ItmDisPercent"].Value = GFunc.NEDec(ItmDisPercent, DBNull.Value);
                    grd.ActiveRow.Cells["ItmPriceUser"].Value = GFunc.NEDec(ItmPriceUser, DBNull.Value);
                    grd.ActiveRow.Cells["ItmAmtShw"].Value = GFunc.NEDec(ItmAmtShw, DBNull.Value);
                    grd.ActiveRow.Cells["ItmPrice"].Value = ItmPrice;
                    grd.ActiveRow.Cells["ItmAmtF"].Value = ItmAmtF;
                    grd.ActiveRow.Cells["ItmAmtH"].Value = ItmAmtH;
                    if (HaveTaxFields)
                    {
                        grd.ActiveRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                        grd.ActiveRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;
                    }
                    #endregion
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
        }//CodeCompleted
        private static bool ItmLatestCostF_DblClick(Document objDoc, UltraGrid grd)
        {
            try
            {
                if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                {
                    MsgBox.Show("Item ID cannot be empty");
                    return false;
                }

                GEnum.INTypeGrp itmTypeGrp = 0;
                GEnum.ItemType itmType = 0;

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
                            itmTypeGrp = (GEnum.INTypeGrp)GFunc.GetINTypeGroup(GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, 810));//set to remarks if nullempty
                            itmType = (GEnum.ItemType)GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, 810);//set to remarks if nullempty
                        }
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
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        switch (GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value))
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                            case (int)GEnum.INTypeGrp.Charges:
                                if (((TAUtil.TANumericEditor)grd.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].EditorComponent).PasswordChar != '*')
                                {

                                    if (grd.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].EditorComponent != null)
                                        if (grd.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].EditorComponent.GetType() == typeof(TAUtil.TANumericEditor))
                                        {
                                            ((TAUtil.TANumericEditor)grd.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].EditorComponent).PasswordChar = '*';
                                            grd.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].CellDisplayStyle = CellDisplayStyle.FullEditorDisplay;
                                            grd.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].CellActivation = Activation.ActivateOnly;

                                        }

                                }
                                else
                                {
                                    if (SECPermUtility.Perform("ItemViewCost", false) == true)
                                    {
                                        if (grd.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].EditorComponent != null)
                                            if (grd.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].EditorComponent.GetType() == typeof(TAUtil.TANumericEditor))
                                            {
                                                ((TAUtil.TANumericEditor)grd.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].EditorComponent).PasswordChar = char.Parse("\0");

                                                grd.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].CellDisplayStyle = CellDisplayStyle.FullEditorDisplay;

                                            }

                                        switch (itmTypeGrp)
                                        {
                                            case GEnum.INTypeGrp.Stock:
                                            case GEnum.INTypeGrp.Non_Stock:
                                            case GEnum.INTypeGrp.Charges:

                                                grd.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].CellActivation = Activation.AllowEdit;


                                                break;
                                            default:
                                                grd.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].CellActivation = Activation.ActivateOnly;
                                                break;
                                        }


                                    }
                                }
                                grd.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].ResetCellAppearance();
                                grd.ActiveRow.Cells[grd.ActiveCell.Column.Index + 1].Activate();
                                grd.ActiveRow.Cells[grd.ActiveCell.Column.Index - 1].Activate();
                                break;
                        }
                        break;
                }
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
        }
        private static bool ItmLatestCostH_DblClick(Document objDoc, UltraGrid grd)
        {
            try
            {
                if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                {
                    MsgBox.Show("Item ID cannot be empty");
                    return false;
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
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        switch (GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value))
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                            case (int)GEnum.INTypeGrp.Charges:
                                if (((TAUtil.TANumericEditor)grd.DisplayLayout.Bands[0].Columns["ItmLatestCostH"].EditorComponent).PasswordChar != '*')
                                {

                                    if (grd.DisplayLayout.Bands[0].Columns["ItmLatestCostH"].EditorComponent != null)
                                        if (grd.DisplayLayout.Bands[0].Columns["ItmLatestCostH"].EditorComponent.GetType() == typeof(TAUtil.TANumericEditor))
                                        {
                                            ((TAUtil.TANumericEditor)grd.DisplayLayout.Bands[0].Columns["ItmLatestCostH"].EditorComponent).PasswordChar = '*';
                                            grd.DisplayLayout.Bands[0].Columns["ItmLatestCostH"].CellDisplayStyle = CellDisplayStyle.FullEditorDisplay;
                                            grd.DisplayLayout.Bands[0].Columns["ItmLatestCostH"].CellActivation = Activation.ActivateOnly;

                                        }
                                }
                                else
                                {
                                    if (SECPermUtility.Perform("ItemViewCost", false) == true)
                                    {
                                        if (grd.DisplayLayout.Bands[0].Columns["ItmLatestCostH"].EditorComponent != null)
                                            if (grd.DisplayLayout.Bands[0].Columns["ItmLatestCostH"].EditorComponent.GetType() == typeof(TAUtil.TANumericEditor))
                                            {
                                                ((TAUtil.TANumericEditor)grd.DisplayLayout.Bands[0].Columns["ItmLatestCostH"].EditorComponent).PasswordChar = char.Parse("\0");

                                                grd.DisplayLayout.Bands[0].Columns["ItmLatestCostH"].CellDisplayStyle = CellDisplayStyle.FullEditorDisplay;
                                                grd.DisplayLayout.Bands[0].Columns["ItmLatestCostH"].CellActivation = Activation.ActivateOnly;
                                            }


                                    }
                                }
                                grd.DisplayLayout.Bands[0].Columns["ItmLatestCostH"].ResetCellAppearance();
                                grd.ActiveRow.Cells[grd.ActiveCell.Column.Index + 1].Activate();
                                grd.ActiveRow.Cells[grd.ActiveCell.Column.Index - 1].Activate();
                                break;
                        }
                        break;
                }
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
        }
        private static bool ItmVendorPrice_DblClick(Document objDoc, UltraGrid grd)
        {
            try
            {
                if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                {
                    MsgBox.Show("Item ID cannot be empty");
                    return false;
                }
                GEnum.INTypeGrp itmTypeGrp = 0;
                GEnum.ItemType itmType = 0;

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
                            itmTypeGrp = (GEnum.INTypeGrp)GFunc.GetINTypeGroup(GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, 810));//set to remarks if nullempty
                            itmType = (GEnum.ItemType)GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, 810);//set to remarks if nullempty
                        }
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
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        switch (GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value))
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                            case (int)GEnum.INTypeGrp.Charges:
                                if (((TAUtil.TANumericEditor)grd.DisplayLayout.Bands[0].Columns["ItmVendorPrice"].EditorComponent).PasswordChar != '*')
                                {

                                    if (grd.DisplayLayout.Bands[0].Columns["ItmVendorPrice"].EditorComponent != null)
                                        if (grd.DisplayLayout.Bands[0].Columns["ItmVendorPrice"].EditorComponent.GetType() == typeof(TAUtil.TANumericEditor))
                                        {
                                            ((TAUtil.TANumericEditor)grd.DisplayLayout.Bands[0].Columns["ItmVendorPrice"].EditorComponent).PasswordChar = '*';
                                            grd.DisplayLayout.Bands[0].Columns["ItmVendorPrice"].CellDisplayStyle = CellDisplayStyle.FullEditorDisplay;
                                            grd.DisplayLayout.Bands[0].Columns["ItmVendorPrice"].CellActivation = Activation.ActivateOnly;

                                        }
                                }
                                else
                                {
                                    if (SECPermUtility.Perform("ItemViewCost", false) == true)
                                    {
                                        if (grd.DisplayLayout.Bands[0].Columns["ItmVendorPrice"].EditorComponent != null)
                                            if (grd.DisplayLayout.Bands[0].Columns["ItmVendorPrice"].EditorComponent.GetType() == typeof(TAUtil.TANumericEditor))
                                            {
                                                ((TAUtil.TANumericEditor)grd.DisplayLayout.Bands[0].Columns["ItmVendorPrice"].EditorComponent).PasswordChar = char.Parse("\0");

                                                grd.DisplayLayout.Bands[0].Columns["ItmVendorPrice"].CellDisplayStyle = CellDisplayStyle.FullEditorDisplay;

                                            }

                                        switch (itmTypeGrp)
                                        {
                                            case GEnum.INTypeGrp.Stock:
                                            case GEnum.INTypeGrp.Non_Stock:
                                            case GEnum.INTypeGrp.Charges:

                                                grd.DisplayLayout.Bands[0].Columns["ItmVendorPrice"].CellActivation = Activation.AllowEdit;


                                                break;
                                            default:
                                                grd.DisplayLayout.Bands[0].Columns["ItmVendorPrice"].CellActivation = Activation.ActivateOnly;
                                                break;
                                        }


                                    }
                                }
                                grd.DisplayLayout.Bands[0].Columns["ItmVendorPrice"].ResetCellAppearance();
                                grd.ActiveRow.Cells[grd.ActiveCell.Column.Index + 1].Activate();
                                grd.ActiveRow.Cells[grd.ActiveCell.Column.Index - 1].Activate();
                                break;
                        }
                        break;
                }
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
        }
        private static bool ItmVendorPriceRatio_DblClick(Document objDoc, UltraGrid grd)
        {
            try
            {
                if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                {
                    MsgBox.Show("Item ID cannot be empty");
                    return false;
                }
                GEnum.INTypeGrp itmTypeGrp = 0;
                GEnum.ItemType itmType = 0;

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
                            itmTypeGrp = (GEnum.INTypeGrp)GFunc.GetINTypeGroup(GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, 810));//set to remarks if nullempty
                            itmType = (GEnum.ItemType)GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, 810);//set to remarks if nullempty
                        }
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
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        switch (GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value))
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                            case (int)GEnum.INTypeGrp.Charges:
                                if (((TAUtil.TANumericEditor)grd.DisplayLayout.Bands[0].Columns["ItmVendorPriceRatio"].EditorComponent).PasswordChar != '*')
                                {

                                    if (grd.DisplayLayout.Bands[0].Columns["ItmVendorPriceRatio"].EditorComponent != null)
                                        if (grd.DisplayLayout.Bands[0].Columns["ItmVendorPriceRatio"].EditorComponent.GetType() == typeof(TAUtil.TANumericEditor))
                                        {
                                            ((TAUtil.TANumericEditor)grd.DisplayLayout.Bands[0].Columns["ItmVendorPriceRatio"].EditorComponent).PasswordChar = '*';
                                            grd.DisplayLayout.Bands[0].Columns["ItmVendorPriceRatio"].CellDisplayStyle = CellDisplayStyle.FullEditorDisplay;
                                            grd.DisplayLayout.Bands[0].Columns["ItmVendorPriceRatio"].CellActivation = Activation.ActivateOnly;
                                        }
                                }
                                else
                                {
                                    if (SECPermUtility.Perform("ItemViewCost", false) == true)
                                    {
                                        if (grd.DisplayLayout.Bands[0].Columns["ItmVendorPriceRatio"].EditorComponent != null)
                                            if (grd.DisplayLayout.Bands[0].Columns["ItmVendorPriceRatio"].EditorComponent.GetType() == typeof(TAUtil.TANumericEditor))
                                            {
                                                ((TAUtil.TANumericEditor)grd.DisplayLayout.Bands[0].Columns["ItmVendorPriceRatio"].EditorComponent).PasswordChar = char.Parse("\0");

                                                grd.DisplayLayout.Bands[0].Columns["ItmVendorPriceRatio"].CellDisplayStyle = CellDisplayStyle.FullEditorDisplay;

                                            }
                                        switch (itmTypeGrp)
                                        {
                                            case GEnum.INTypeGrp.Stock:
                                            case GEnum.INTypeGrp.Non_Stock:
                                            case GEnum.INTypeGrp.Charges:

                                                grd.DisplayLayout.Bands[0].Columns["ItmVendorPriceRatio"].CellActivation = Activation.AllowEdit;


                                                break;
                                            default:
                                                grd.DisplayLayout.Bands[0].Columns["ItmVendorPriceRatio"].CellActivation = Activation.ActivateOnly;
                                                break;
                                        }

                                    }
                                }
                                grd.DisplayLayout.Bands[0].Columns["ItmVendorPriceRatio"].ResetCellAppearance();
                                grd.ActiveRow.Cells[grd.ActiveCell.Column.Index + 1].Activate();
                                grd.ActiveRow.Cells[grd.ActiveCell.Column.Index - 1].Activate();
                                break;
                        }
                        break;
                }
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
        }

        public static bool DetJobID_btnClick(Document objDoc, Hashtable docDet, string searchValue, string listSettingID)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                int phaseKey = 0, costTypeKey = 0, taskKey = 0;

                UltraGrid grd = null;

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, docDet, ref grd);
                        break;

                    default:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grd);
                        break;
                }

                if (DocHDRUtil.EditorButton_Popup(objDoc, searchValue, listSettingID, (int)GEnum.PopupType.JobID, ref key, ref id, ref des, ref phaseKey, ref costTypeKey, ref taskKey))
                {
                    return DetJobKey_Update(objDoc, grd, key, phaseKey, taskKey, costTypeKey);
                }

                return false;
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
        public static bool DetJobKey_CustomUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                int? key = 0;
                int phaseKey = 0, costTypeKey = 0, taskKey = 0;

                UltraGrid grd = null;

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
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grd);
                        key = GFunc.NEInt(grd.ActiveRow.Cells["ItmJobKey"].Value, 0);
                        phaseKey = GFunc.NEInt(grd.ActiveRow.Cells["ItmJobPhaseKey"].Value, 0);
                        taskKey = GFunc.NEInt(grd.ActiveRow.Cells["ItmJobTaskKey"].Value, 0);
                        costTypeKey = GFunc.NEInt(grd.ActiveRow.Cells["ItmJobCostTypeKey"].Value, 0);
                        return DetJobKey_Update(objDoc, grd, key, phaseKey, taskKey, costTypeKey);

                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, docDet, ref grd);
                        key = GFunc.NEInt(grd.ActiveRow.Cells["ExpJobKey"].Value, 0);
                        phaseKey = GFunc.NEInt(grd.ActiveRow.Cells["ExpJobPhaseKey"].Value, 0);
                        taskKey = GFunc.NEInt(grd.ActiveRow.Cells["ExpJobTaskKey"].Value, 0);
                        costTypeKey = GFunc.NEInt(grd.ActiveRow.Cells["ExpJobCostTypeKey"].Value, 0);
                        return DetJobKey_Update(objDoc, grd, key, phaseKey, taskKey, costTypeKey);

                    default:
                        MsgBox.Show("Unable to match Document Code");
                        return false;
                }
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
        private static bool DetJobKey_Update(Document objDoc, UltraGrid grd, int? key, int? pKey, int? tKey, int? cKey)
        {

            try
            {
                int ItmType = 0;

                #region Check DocCodeKey if any calculation is to be run
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
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }

                        if (DocHDRUtil.Doc_CheckDetItm(objDoc, grd, GEnum.ValidateField.ItmJobKey) == false)
                            return false;

                        #region set values base on ItmType
                        ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                        switch (ItmType)
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                                switch (objDoc.DocCodeKey)
                                {
                                    case (int)GEnum.SystemCode.Purchase_Order:
                                    case (int)GEnum.SystemCode.Purchase_Shipment:
                                    case (int)GEnum.SystemCode.Purchase_Delivery:
                                    case (int)GEnum.SystemCode.Purchase_Invoice:
                                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                                        grd.ActiveRow.Cells["ItmJobKey"].Value = 0;
                                        grd.ActiveRow.Cells["ItmJobPhaseKey"].Value = 0;
                                        grd.ActiveRow.Cells["ItmJobTaskKey"].Value = 0;
                                        grd.ActiveRow.Cells["ItmJobCostTypeKey"].Value = 0;
                                        MsgBox.Show("Stock Items in purchase document cannot be assigned with Job, use inventory adjustment instead");
                                        return true;

                                    default:
                                        if (GFunc.IsNEZ(key))
                                        {
                                            grd.ActiveRow.Cells["ItmJobKey"].Value = 0;
                                            grd.ActiveRow.Cells["ItmJobPhaseKey"].Value = 0;
                                            grd.ActiveRow.Cells["ItmJobTaskKey"].Value = 0;
                                            grd.ActiveRow.Cells["ItmJobCostTypeKey"].Value = 0;
                                        }
                                        else
                                        {
                                            grd.ActiveRow.Cells["ItmJobKey"].Value = key;
                                            grd.ActiveRow.Cells["ItmJobPhaseKey"].Value = GFunc.NEInt(pKey, 0);
                                            grd.ActiveRow.Cells["ItmJobTaskKey"].Value = GFunc.NEInt(tKey, 0);
                                            grd.ActiveRow.Cells["ItmJobCostTypeKey"].Value = GFunc.NEInt(cKey, 0);
                                        }
                                        break;
                                }
                                break;

                            case (int)GEnum.INTypeGrp.Remark:
                            case (int)GEnum.INTypeGrp.Total:
                            case (int)GEnum.INTypeGrp.Empty:
                                grd.ActiveRow.Cells["ItmJobKey"].Value = 0;
                                grd.ActiveRow.Cells["ItmJobPhaseKey"].Value = 0;
                                grd.ActiveRow.Cells["ItmJobTaskKey"].Value = 0;
                                grd.ActiveRow.Cells["ItmJobCostTypeKey"].Value = 0;
                                MsgBox.Show("Non posting item type (remarks, header, total) cannot be assigned with Job");
                                return true;

                            default:
                                if (GFunc.IsNEZ(key))
                                {
                                    grd.ActiveRow.Cells["ItmJobKey"].Value = 0;
                                    grd.ActiveRow.Cells["ItmJobPhaseKey"].Value = 0;
                                    grd.ActiveRow.Cells["ItmJobTaskKey"].Value = 0;
                                    grd.ActiveRow.Cells["ItmJobCostTypeKey"].Value = 0;
                                }
                                else
                                {
                                    grd.ActiveRow.Cells["ItmJobKey"].Value = key;
                                    grd.ActiveRow.Cells["ItmJobPhaseKey"].Value = GFunc.NEInt(pKey, 0);
                                    grd.ActiveRow.Cells["ItmJobTaskKey"].Value = GFunc.NEInt(tKey, 0);
                                    grd.ActiveRow.Cells["ItmJobCostTypeKey"].Value = GFunc.NEInt(cKey, 0);
                                }
                                break;
                        }
                        #endregion

                        break;

                    case (int)GEnum.SystemCode.Journal:
                        if (GFunc.IsNEZ(key))
                        {
                            grd.ActiveRow.Cells["ItmJobKey"].Value = 0;
                            grd.ActiveRow.Cells["ItmJobPhaseKey"].Value = 0;
                            grd.ActiveRow.Cells["ItmJobTaskKey"].Value = 0;
                            grd.ActiveRow.Cells["ItmJobCostTypeKey"].Value = 0;
                        }
                        else
                        {
                            grd.ActiveRow.Cells["ItmJobKey"].Value = key;
                            grd.ActiveRow.Cells["ItmJobPhaseKey"].Value = GFunc.NEInt(pKey, 0);
                            grd.ActiveRow.Cells["ItmJobTaskKey"].Value = GFunc.NEInt(tKey, 0);
                            grd.ActiveRow.Cells["ItmJobCostTypeKey"].Value = GFunc.NEInt(cKey, 0);
                        }
                        break;

                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        if (GFunc.IsNEZ(key))
                        {
                            grd.ActiveRow.Cells["ExpJobKey"].Value = 0;
                            grd.ActiveRow.Cells["ExpJobPhaseKey"].Value = 0;
                            grd.ActiveRow.Cells["ExpJobTaskKey"].Value = 0;
                            grd.ActiveRow.Cells["ExpJobCostTypeKey"].Value = 0;
                        }
                        else
                        {
                            grd.ActiveRow.Cells["ExpJobKey"].Value = key;
                            grd.ActiveRow.Cells["ExpJobPhaseKey"].Value = GFunc.NEInt(pKey, 0);
                            grd.ActiveRow.Cells["ExpJobTaskKey"].Value = GFunc.NEInt(tKey, 0);
                            grd.ActiveRow.Cells["ExpJobCostTypeKey"].Value = GFunc.NEInt(cKey, 0);
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
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
        private static bool DetJobPhaseKey_CustomUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                return DetJobKey_CustomUpdate(objDoc, docDet);
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
        private static bool DetJobTaskKey_CustomUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                return DetJobKey_CustomUpdate(objDoc, docDet);
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
        private static bool DetJobCostTypeKey_CustomUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                return DetJobKey_CustomUpdate(objDoc, docDet);
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

        private static bool FGReq_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                decimal FGReqQty = 0;
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Inventory_Production:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        FGReqQty = (decimal)GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["FGReq"].Value, 0), GVar.RndDecs.Qtypt);
                        if (FGReqQty < 0)
                        {
                            MsgBox.Show("Finished goods qty cannot be < 0");
                            return false;
                        }
                        grd.ActiveRow.Cells["FGReq"].Value = FGReqQty;
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
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
        private static bool FGOverHeadCost_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                decimal overHeadCost = 0;
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Inventory_Production:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }

                        overHeadCost = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["FGOverHeadCost"].Value, 0), GVar.RndDecs.Prcpt);
                        if (overHeadCost < 0)
                        {
                            MsgBox.Show("Overhead Cost cannot be < 0");
                            return false;
                        }
                        grd.ActiveRow.Cells["FGOverHeadCost"].Value = overHeadCost;
                        grd.ActiveRow.Cells["FGOverHeadAmtH"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["FGProduceQty"].Value, 0) * overHeadCost, GVar.RndDecs.Amtpt);

                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
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
        private static bool FGOverHeadKey_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                int key = 0;
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Inventory_Production:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }

                        key = GFunc.NEInt(grd.ActiveRow.Cells["FGOverHeadKey"].Value, 0);
                        if (key > 0)
                        {
                            REFOverHead objOverHead = REFOverHead.Get(key);
                            grd.ActiveRow.Cells["FGOverHeadCost"].Value = objOverHead.OverHeadCost;
                            return FGOverHeadCost_CustomUpdate(objDoc, grd);
                        }
                        else
                        {
                            grd.ActiveRow.Cells["FGOverHeadCost"].Value = 0;
                            grd.ActiveRow.Cells["FGOverHeadAmtH"].Value = 0;
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
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
        private static bool BOMReq_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                decimal BOMReqQty = 0;
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Inventory_Production:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        BOMReqQty = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["BOMReq"].Value, 0), GVar.RndDecs.Qtypt);
                        if (BOMReqQty < 0)
                        {
                            MsgBox.Show("material goods qty cannot be < 0");
                            return false;
                        }
                        grd.ActiveRow.Cells["BOMReq"].Value = BOMReqQty;
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
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
        private static bool BOMIssue_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                decimal BOMIssueQty = 0, BOMReturnQty = 0, BOMUsedQty = 0;

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Inventory_Production:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        BOMIssueQty = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["BOMIssue"].Value, 0), GVar.RndDecs.Qtypt);
                        BOMReturnQty = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["BOMReturn"].Value, 0), GVar.RndDecs.Qtypt);
                        BOMUsedQty = BOMIssueQty - BOMReturnQty;

                        grd.ActiveRow.Cells["BOMIssue"].Value = BOMIssueQty;
                        grd.ActiveRow.Cells["BOMReturn"].Value = BOMReturnQty;
                        grd.ActiveRow.Cells["BOMUsed"].Value = BOMUsedQty;
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
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
        private static bool BOMReturn_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                decimal BOMIssueQty = 0, BOMReturnQty = 0, BOMUsedQty = 0;

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Inventory_Production:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        BOMIssueQty = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["BOMIssue"].Value, 0), GVar.RndDecs.Qtypt);
                        BOMReturnQty = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["BOMReturn"].Value, 0), GVar.RndDecs.Qtypt);
                        BOMUsedQty = BOMIssueQty - BOMReturnQty;

                        grd.ActiveRow.Cells["BOMIssue"].Value = BOMIssueQty;
                        grd.ActiveRow.Cells["BOMReturn"].Value = BOMReturnQty;
                        grd.ActiveRow.Cells["BOMUsed"].Value = BOMUsedQty;
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
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
        private static bool BOMUsed_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                decimal BOMIssueQty = 0, BOMReturnQty = 0, BOMUsedQty = 0;
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Inventory_Production:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        BOMIssueQty = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["BOMIssue"].Value, 0), GVar.RndDecs.Qtypt);
                        BOMUsedQty = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["BOMUsed"].Value, 0), GVar.RndDecs.Qtypt);
                        BOMReturnQty = BOMIssueQty - BOMUsedQty;

                        grd.ActiveRow.Cells["BOMIssue"].Value = BOMIssueQty;
                        grd.ActiveRow.Cells["BOMReturn"].Value = BOMReturnQty;
                        grd.ActiveRow.Cells["BOMUsed"].Value = BOMUsedQty;
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
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
        private static bool BOMLabourCost_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                decimal bomLabourCost = 0;
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Inventory_Production:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        bomLabourCost = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["BOMLabourCost"].Value, 0), GVar.RndDecs.Prcpt);
                        if (bomLabourCost < 0)
                        {
                            MsgBox.Show("Labour Cost cannot be < 0");
                            return false;
                        }

                        grd.ActiveRow.Cells["BOMLabourCost"].Value = bomLabourCost;
                        grd.ActiveRow.Cells["BOMLabourAmt"].Value = GFunc.RndC(bomLabourCost * GFunc.NEDec(grd.ActiveRow.Cells["FGProduceQty"].Value, 0), GVar.RndDecs.Amtpt);
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
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

        private static bool ItmLocKey_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                int ItmType = 0;

                #region Check DocCodeKey if any calculation is to be run
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
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                switch (ItmType)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmLocKey"].Value))
                        {
                            MsgBox.Show(MsgID.Common.CannotBeEmpty + "%Location");
                            return false;
                        }
                        break;

                    default:
                        MsgBox.Show(MsgID.Document.NotAllowedInput + "%Location");
                        grd.ActiveRow.Cells["ItmLocKey"].Value = DBNull.Value;
                        return false;
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
        private static bool ItmFromLocKey_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                int ItmType = 0;

                #region Check DocCodeKey if any calculation is to be run
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                switch (ItmType)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmFromLocKey"].Value))
                        {
                            MsgBox.Show(MsgID.Common.CannotBeEmpty + "%Location");
                            return false;
                        }
                        break;

                    default:
                        MsgBox.Show(MsgID.Document.NotAllowedInput + "%Markup Price");
                        grd.ActiveRow.Cells["ItmFromLocKey"].Value = DBNull.Value;
                        return false;
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
        private static bool ItmToLocKey_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                int ItmType = 0;

                #region Check DocCodeKey if any calculation is to be run
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                switch (ItmType)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmToLocKey"].Value))
                        {
                            MsgBox.Show(MsgID.Common.CannotBeEmpty + "%Location");
                            return false;
                        }
                        break;

                    default:
                        MsgBox.Show(MsgID.Document.NotAllowedInput + "%Markup Price");
                        grd.ActiveRow.Cells["ItmToLocKey"].Value = DBNull.Value;
                        return false;
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
        private static bool ItmListPrice_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            #region Declare variables
            bool RunCalculateAR = false;
            bool RunCalculateAP = false;
            bool RunCalculateTax = false;
            bool RunNullChargeItemQty = false;

            int MarkUpType = 0;
            int PriceDec = 0;
            int PriceRoundMode = 0;

            bool HaveTaxFields = false;
            int? DocCurrKey = 0;
            int? DocTaxKey = 0;
            decimal? DocTaxRate = 0;
            decimal? DocCurrRate = 1;
            decimal? DocCountryRate = 1;

            int? ItmType = 0;
            decimal? ItmQty = 0;
            decimal? ItmListPrice = 0;
            int? ItmVendorCurrKey = 0;
            decimal? ItmVendorCurrRate = 0;
            decimal? ItmMarkupRate = 0;
            decimal? ItmMarkupRatio = 0;
            decimal? ItmVendorPrice = 0;
            decimal? ItmPriceAfter = 0;
            decimal? ItmDisPercent = 0;
            decimal? ItmDisValue = 0;
            decimal? ItmPriceUser = 0;
            decimal? ItmAmtShw = 0;
            decimal? ItmPrice = 0;
            decimal? ItmAmtF = 0;
            decimal? ItmAmtH = 0;
            int? ItmTaxGrpKey = 0;
            decimal? ItmTaxGrpRate = 0;
            decimal? ItmTaxGrpAmtF = 0;
            decimal? ItmTaxGrpAmtL = 0;
            #endregion

            try
            {
                #region Check DocCodeKey if any calculation is to be run
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
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        HaveTaxFields = true;
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                switch (ItmType)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                    case (int)GEnum.INTypeGrp.Charges:
                        //grd.ActiveRow.Cells["ItmListPrice"].Value = GFunc.RndC(grd.ActiveRow.Cells["ItmListPrice"].Value, GVar.RndDecs.Prcpt);
                        grd.ActiveRow.Cells["ItmListPrice"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmListPrice"].Value, 0), GVar.RndDecs.Prcpt);
                        break;

                    case (int)GEnum.INTypeGrp.Remark:
                        return true;

                    default:
                        MsgBox.Show(MsgID.Document.NotAllowedInput + "%List Price");
                        grd.ActiveRow.Cells["ItmListPrice"].Value = DBNull.Value;
                        return false;
                }
                #endregion

                if (Check_RunProcess((int)objDoc.DocCodeKey, grd, ref RunCalculateAR, ref RunCalculateAP, ref RunNullChargeItemQty, ref RunCalculateTax) == false)
                    return false;

                #region run NullChargeItemQty
                if (RunNullChargeItemQty)
                    grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;
                #endregion

                #region run RunCalculateAR
                if (RunCalculateAR)
                {
                    #region Set Variables
                    if (HaveTaxFields)
                    {
                        ItmTaxGrpRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmTaxGrpRate"].Value, 0);
                        DocTaxKey = (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc);
                        DocTaxRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);
                    }
                    else
                    {
                        ItmTaxGrpKey = 0;
                        ItmTaxGrpRate = 0;
                        DocTaxKey = 0;
                        DocTaxRate = 0;
                    }

                    MarkUpType = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultDocMarkupType);
                    PriceDec = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceDecPlace);
                    PriceRoundMode = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceRoundMode);

                    DocCurrKey = GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 1);
                    DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                    if (SysOptionUtility.GetInt("CountryCurrency") == 1) // Country Currency = Home -- Added by Jane. Mic to check
                        DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                    else
                        DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);

                    ItmQty = GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0);
                    ItmListPrice = GFunc.NEDec(grd.ActiveRow.Cells["ItmListPrice"].Value, 0);
                    ItmVendorCurrKey = GFunc.NEInt(grd.ActiveRow.Cells["ItmVendorCurrKey"].Value, 1);
                    ItmVendorCurrRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorCurrRate"].Value, 1);
                    ItmVendorPrice = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorPrice"].Value, 0);
                    ItmMarkupRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorPriceRatio"].Value, 0);
                    ItmMarkupRatio = GFunc.RndDC(ItmMarkupRate, 100, GVar.RndDecs.Prcpt);
                    ItmPriceAfter = GFunc.NEDec(grd.ActiveRow.Cells["ItmPriceAfter"].Value, 0);
                    ItmDisPercent = GFunc.NEDec(grd.ActiveRow.Cells["ItmDisPercent"].Value, 0);

                    #endregion

                    if ((bool)grd.ActiveRow.Cells["ItmVendorPriceLock"].Value || ItmMarkupRatio <= 0M)
                    {
                        #region Calculate ItmMarkUpRate from MarkupType and run Row calculation if required
                        switch (MarkUpType)
                        {
                            //Multiply by Retail Price
                            case 10:
                                ItmMarkupRatio = GFunc.RndDC(ItmPriceAfter, ItmListPrice, GVar.RndDecs.Prcpt) - 1;
                                break;

                            //Divided by Retail Price
                            case 30:
                                ItmMarkupRatio = 1 - GFunc.RndDC(ItmListPrice, ItmPriceAfter, GVar.RndDecs.Prcpt);
                                break;

                            default:
                                return true;
                        }
                        #endregion

                        #region Row Calculation and set values to grid
                        //Divided by Retail Price Or  Divided by Vendor Cost
                        if (MarkUpType == 30 || MarkUpType == 40)
                        {
                            ItmMarkupRatio = DocComUtility.MarkUpRateDivisionLimit_Set(ItmMarkupRatio, MarkUpType, ItmListPrice, ItmVendorPrice, ItmPriceAfter);
                        }
                        ItmMarkupRate = GFunc.RndC(ItmMarkupRatio * 100, GVar.RndDecs.Qtypt);
                        grd.ActiveRow.Cells["ItmVendorPriceRatio"].Value = ItmMarkupRate;
                        #endregion

                        return true;
                    }
                    else
                    {
                        #region Calculate ItmPriceAfter from MarkupType and run Row Calculation if required
                        switch (MarkUpType)
                        {
                            //Multiply by Retail Price
                            case 10:
                                ItmPriceAfter = GFunc.RndUD(ItmListPrice * (1 + ItmMarkupRatio), PriceRoundMode, PriceDec);
                                break;

                            //Divided by Retail Price
                            case 30:
                                ItmPriceAfter = GFunc.RndDC(ItmListPrice, (1 - ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                                break;

                            default:
                                return true;
                        }
                        #endregion

                        #region Row Calculation
                        ItmDisValue = GFunc.RndUD(ItmPriceAfter * ItmDisPercent / 100M, PriceRoundMode, PriceDec);
                        ItmPriceUser = GFunc.RndUD(ItmPriceAfter - ItmDisValue, PriceRoundMode, PriceDec);
                        if (ItmType == (int)GEnum.INTypeGrp.Charges)
                            ItmAmtShw = ItmPriceUser;
                        else
                            ItmAmtShw = GFunc.RndC(ItmPriceUser * ItmQty, GVar.RndDecs.Amtpt);

                        if (objDoc.DocType == 110)   //if Tax Inclusive,
                        {
                            if (ItmTaxGrpRate > 0)
                                if (DocTaxRate == 0)
                                    ItmPrice = ItmPriceUser;
                                else
                                    ItmPrice = GFunc.RndDC(ItmPriceUser, (1 + ItmTaxGrpRate), GVar.RndDecs.Prcpt);
                            else
                                ItmPrice = ItmPriceUser;
                        }
                        else
                        {
                            ItmPrice = ItmPriceUser;
                        }

                        if (ItmType == (int)GEnum.INTypeGrp.Charges)
                            ItmAmtF = ItmPrice;
                        else
                            ItmAmtF = GFunc.RndC(ItmPrice * ItmQty, GVar.RndDecs.Amtpt);

                        ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                        #endregion

                        #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable

                        if (RunCalculateTax)
                        {
                            if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                            {
                                //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                                ItmTaxGrpKey = DocTaxKey;
                                ItmTaxGrpRate = 0;
                                ItmTaxGrpAmtF = 0;
                                ItmTaxGrpAmtL = 0;
                            }
                            else
                            {
                                if (ItmTaxGrpRate > 0)
                                {
                                    ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt );
                                    ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                                }
                                else
                                {
                                    ItmTaxGrpAmtF = 0;
                                    ItmTaxGrpAmtL = 0;
                                }
                            }
                        }
                        #endregion

                        #region set values to grid
                        grd.ActiveRow.Cells["ItmPriceAfter"].Value = ItmPriceAfter.ToDBValue();
                        grd.ActiveRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue();
                        grd.ActiveRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue();
                        grd.ActiveRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue();
                        grd.ActiveRow.Cells["ItmPrice"].Value = ItmPrice;
                        grd.ActiveRow.Cells["ItmAmtF"].Value = ItmAmtF;
                        grd.ActiveRow.Cells["ItmAmtH"].Value = ItmAmtH;
                        if (HaveTaxFields)
                        {
                            grd.ActiveRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                            grd.ActiveRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;
                        }
                        #endregion

                        return true;
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

        public static bool DocItmKey_Sequence(UltraGrid grd)
        {
            DataTable dt = grd.DataSource as DataTable;

            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (GFunc.NEInt(row["DocItmKey"], 0) == 0)
                    {
                        row["DocItmKey"] = dt.Rows.Count == 0 ? 1 : dt.AsEnumerable().Max(k => k.Field<int>("DocItmKey")) + 1;
                    }
                }

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
        public static bool ItmMark_ReSequence(Document objDoc, UltraGrid grd)
        {
            long lCounter = 0;
            
            try
            {
                if (MsgBox.Show(MsgID.Document.ConfirmRegenerateItemMarkingSequence, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                {

                    foreach (UltraGridRow row in grd.Rows.GetFilteredInNonGroupByRows())
                    {
                        switch (GFunc.GetINTypeGroup(row.Cells["ItmType"].Value))
                        {
                            case (int)GEnum.INTypeGrp.Remark:
                            case (int)GEnum.INTypeGrp.Total:
                            case (int)GEnum.INTypeGrp.Empty:
                                row.Cells["ItmMark"].Value = string.Empty;
                                break;
                            default:
                                lCounter += 1;
                                row.Cells["ItmMark"].Value = lCounter.ToString();
                                break;
                        }
                    }
                    objDoc.IsDirty = true;
                    grd.UpdateData();
                }
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

        public static bool DetItm_ZeroOffBalance(Document objDoc, UltraGrid grd)
        {
            long lCounter = 0;
            int ItmType = 0;
            Decimal ItmQtyLink = 0;
            Decimal ItmQtyAdj = 0;
            Decimal ItmQty = 0;

            Boolean IsValid = false;
            try
            {
                foreach (UltraGridRow row in grd.Rows.GetFilteredInNonGroupByRows())
                {
                    if (GFunc.NEDec(row.Cells["ItmQtyBalance"].Value, 0) > 0)
                    {
                        IsValid = true;
                        break;
                    }
                }
                if (!IsValid) return false;

                if (MsgBox.Show(MsgID.Document.ConfirmZeroOffBalance, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                {

                    foreach (UltraGridRow row in grd.Rows.GetFilteredInNonGroupByRows())
                    {
                        ItmType = GFunc.GetINTypeGroup(row.Cells["ItmType"].Value);
                        #region Set ItmQty, ItmQtyBalance
                        switch (objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Sales_Order:
                            case (int)GEnum.SystemCode.Reserve_Order:
                            case (int)GEnum.SystemCode.Purchase_Order:
                            case (int)GEnum.SystemCode.Order_Consignment:
                                switch (ItmType)
                                {
                                    case (int)GEnum.INTypeGrp.Stock:
                                    case (int)GEnum.INTypeGrp.Non_Stock:
                                        if (GFunc.NEDec(row.Cells["ItmQtyBalance"].Value, 0) > 0)
                                        {
                                            ItmQtyLink = GFunc.RndC(GFunc.NEDec(row.Cells["ItmQtyLink"].Value, 0), GVar.RndDecs.Qtypt);
                                            ItmQtyAdj = GFunc.RndC(GFunc.NEDec(row.Cells["ItmQtyAdj"].Value, 0), GVar.RndDecs.Qtypt);
                                            ItmQty = ItmQtyLink + ItmQtyAdj;
                                            row.Cells["ItmQtyBalance"].Value = ItmQty - ItmQtyLink - ItmQtyAdj;
                                            row.Cells["ItmQty"].Value = ItmQty;
                                        }
                                        break;
                                }
                                break;


                        }
                        #endregion


                    }
                    objDoc.IsDirty = true;
                    grd.UpdateData();
                    return true;
                }
                else 
                    return false;
            }
            catch (TAException taex)
            {
                throw Error(taex, false);
                return false;
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
                return false;
            }
        }//Completed
        public static string ItmMark_GetNew(UltraGrid grd, int ItmType)
        {
            decimal LastMark = 0;
            decimal MaxMark = 0;
            decimal NewMark = 0;

            try
            {
                if (GFunc.IsNE(grd.ActiveRow.Cells["ItmMark"].Value))
                {
                    switch (ItmType)
                    {
                        case (int)GEnum.ItemType.Finished_GDB:
                        case (int)GEnum.ItemType.Serial_Finished_GDB:
                        case (int)GEnum.ItemType.Serial_StockB:
                        case (int)GEnum.ItemType.StockB:
                        case (int)GEnum.ItemType.Finished_GD:
                        case (int)GEnum.ItemType.Stock:
                        case (int)GEnum.ItemType.Consignment:
                        case (int)GEnum.ItemType.Assembly:
                        case (int)GEnum.ItemType.Non_Stock:
                        case (int)GEnum.ItemType.Service:
                        case (int)GEnum.ItemType.Charges:
                        case (int)GEnum.ItemType.Discount:

                            if (SysOptionUtility.GetBool(GVar.SystemOption.OpID.DocumentDetailAutoMarkingNumbering) == true)
                            {
                                foreach (UltraGridRow row in grd.Rows)
                                {
                                    if (Decimal.TryParse(row.Cells["ItmMark"].Value.ToString(), out LastMark))
                                    {
                                        if (LastMark > MaxMark)
                                            MaxMark = LastMark;
                                    }
                                }
                                NewMark = MaxMark + 1;
                                return NewMark.ToString();
                            }
                            return string.Empty;

                        default:
                            return string.Empty;
                    }

                }
                else
                    return grd.ActiveRow.Cells["ItmMark"].Value.ToString();

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

        public static string ItmMark_GetNew(System.Data.SqlClient.SqlConnection cn,UltraGrid grd, int ItmType)
        {
            decimal LastMark = 0;
            decimal MaxMark = 0;
            decimal NewMark = 0;

            try
            {
                if (grd.ActiveRow == null)
                    return "";
                if (GFunc.IsNE(grd.ActiveRow.Cells["ItmMark"].Value))
                {
                    switch (ItmType)
                    {
                        case (int)GEnum.ItemType.Finished_GDB:
                        case (int)GEnum.ItemType.Serial_Finished_GDB:
                        case (int)GEnum.ItemType.Serial_StockB:
                        case (int)GEnum.ItemType.StockB:
                        case (int)GEnum.ItemType.Finished_GD:
                        case (int)GEnum.ItemType.Stock:
                        case (int)GEnum.ItemType.Consignment:
                        case (int)GEnum.ItemType.Assembly:
                        case (int)GEnum.ItemType.Non_Stock:
                        case (int)GEnum.ItemType.Service:
                        case (int)GEnum.ItemType.Charges:
                        case (int)GEnum.ItemType.Discount:

                            if (SysOptionUtility.GetBool(GVar.SystemOption.OpID.DocumentDetailAutoMarkingNumbering,cn) == true)
                            {
                                foreach (UltraGridRow row in grd.Rows)
                                {
                                    if (Decimal.TryParse(row.Cells["ItmMark"].Value.ToString(), out LastMark))
                                    {
                                        if (LastMark > MaxMark)
                                            MaxMark = LastMark;
                                    }
                                }
                                NewMark = MaxMark + 1;
                                return NewMark.ToString();
                            }
                            return string.Empty;

                        default:
                            return string.Empty;
                    }

                }
                else
                    return grd.ActiveRow.Cells["ItmMark"].Value.ToString();

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

        private static bool ItmMarkupRate_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                UltraGridRow grdRow = grd.ActiveRow;

                int MarkUpType = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultDocMarkupType);
                int PriceDec = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceDecPlace);
                int PriceRoundMode = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceRoundMode);

                return ItmMarkupRate_Update(objDoc, grdRow, MarkUpType, PriceDec, PriceRoundMode);
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
        public static bool ItmMarkupRate_Update(Document objDoc, UltraGridRow grdRow, int MarkUpType, int PriceDec, int PriceRoundMode)
        {
            #region Declare variables
            bool RunCalculateAR = false;
            bool RunNullChargeItemQty = false;

            int? DocCurrKey = 0;
            int? DocTaxKey = 0;
            decimal? DocTaxRate = 0;
            decimal? DocCurrRate = 1;
            decimal? DocCountryRate = 1;

            int? ItmType = 0;
            decimal? ItmQty = 0;
            decimal? ItmListPrice = 0;
            int? ItmVendorCurrKey = 0;
            decimal? ItmVendorCurrRate = 0;
            decimal? ItmMarkupRate = 0;
            decimal? ItmMarkupRatio = 0;
            decimal? ItmVendorPrice = 0;
            decimal? ItmPriceAfter = 0;
            decimal? ItmDisPercent = 0;
            decimal? ItmDisValue = 0;
            decimal? ItmPriceUser = 0;
            decimal? ItmAmtShw = 0;
            decimal? ItmPrice = 0;
            decimal? ItmAmtF = 0;
            decimal? ItmAmtH = 0;
            int? ItmTaxGrpKey = 0;
            decimal? ItmTaxGrpRate = 0;
            decimal? ItmTaxGrpAmtF = 0;
            decimal? ItmTaxGrpAmtL = 0;
            #endregion

            try
            {
                #region Set process to run by DocCode and ItmType
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
                        if (GFunc.IsNEZ(grdRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        ItmType = GFunc.GetINTypeGroup(grdRow.Cells["ItmType"].Value);
                        switch (ItmType)
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                                grdRow.Cells["ItmVendorPriceRatio"].Value = GFunc.RndC(GFunc.NEDec(grdRow.Cells["ItmVendorPriceRatio"].Value, 0), GVar.RndDecs.Prcpt);
                                RunCalculateAR = true;
                                break;

                            case (int)GEnum.INTypeGrp.Charges:
                                grdRow.Cells["ItmVendorPriceRatio"].Value = GFunc.RndC(GFunc.NEDec(grdRow.Cells["ItmVendorPriceRatio"].Value, 0), GVar.RndDecs.Prcpt);
                                RunNullChargeItemQty = true;
                                RunCalculateAR = true;
                                break;

                            case (int)GEnum.INTypeGrp.Remark:
                                return true;

                            default:
                                //reset user input value
                                MsgBox.Show(MsgID.Document.NotAllowedInput + "%Markup Value");
                                grdRow.Cells["ItmVendorPriceRatio"].Value = DBNull.Value;
                                return false;
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region run NullChargeItemQty
                if (RunNullChargeItemQty)
                    grdRow.Cells["ItmQty"].Value = DBNull.Value;
                #endregion

                #region run RunCalculateAR
                if (RunCalculateAR)
                {
                    #region Setup variables
                    ItmTaxGrpRate = GFunc.NEDec(grdRow.Cells["ItmTaxGrpRate"].Value, 0);
                    DocTaxKey = (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc);
                    DocTaxRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);

                    MarkUpType = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultDocMarkupType);
                    PriceDec = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceDecPlace);
                    PriceRoundMode = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceRoundMode);

                    DocCurrKey = (int)GFunc.GetPropertyValue("DocCurrKey", objDoc);
                    DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                    if (SysOptionUtility.GetInt("CountryCurrency") == 1) // Country Currency = Home -- Added by Jane. Mic to check
                        DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                    else
                        DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);

                    ItmQty = GFunc.NEDec(grdRow.Cells["ItmQty"].Value, 0);
                    ItmListPrice = GFunc.NEDec(grdRow.Cells["ItmListPrice"].Value, 0);
                    ItmVendorCurrKey = GFunc.NEInt(grdRow.Cells["ItmVendorCurrKey"].Value, 1);
                    ItmVendorCurrRate = GFunc.NEDec(grdRow.Cells["ItmVendorCurrRate"].Value, 1);
                    ItmVendorPrice = GFunc.NEDec(grdRow.Cells["ItmVendorPrice"].Value, 0);
                    ItmMarkupRate = GFunc.NEDec(grdRow.Cells["ItmVendorPriceRatio"].Value, 0);
                    ItmMarkupRatio = GFunc.RndDC(ItmMarkupRate, 100, GVar.RndDecs.Prcpt);
                    ItmPriceAfter = GFunc.NEDec(grdRow.Cells["ItmPriceAfter"].Value, 0);
                    ItmDisPercent = GFunc.NEDec(grdRow.Cells["ItmDisPercent"].Value, 0);
                    #endregion

                    if ((bool)grdRow.Cells["ItmVendorPriceLock"].Value == false)
                    {
                        #region Calculate ItmPriceAfter from MarkupType and run Row Calculation if required
                        switch (MarkUpType)
                        {
                            //Multiply by Retail Price
                            case 10:
                                ItmPriceAfter = GFunc.RndUD(ItmListPrice * (1 + ItmMarkupRatio), PriceRoundMode, PriceDec);
                                break;

                            //Multiply by Vendor Cost
                            case 20:
                                if (DocCurrKey == ItmVendorCurrKey)
                                {
                                    ItmPriceAfter = GFunc.RndUD(ItmVendorPrice * (1 + ItmMarkupRatio), PriceRoundMode, PriceDec);
                                }
                                else
                                {
                                    ItmPriceAfter = GFunc.RndC(ItmVendorPrice * ItmVendorCurrRate * (1 + ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndDC(ItmPriceAfter, DocCurrRate, GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                                }
                                break;

                            //Divided by Retail Price
                            case 30:
                                ItmPriceAfter = GFunc.RndDC(ItmListPrice, (1 - ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                                break;

                            //Divided by Vendor Cost
                            case 40:
                                if (DocCurrKey == ItmVendorCurrKey)
                                {
                                    ItmPriceAfter = GFunc.RndDC(ItmVendorPrice, (1 - ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                                }
                                else
                                {
                                    ItmPriceAfter = GFunc.RndDC(ItmVendorPrice * ItmVendorCurrRate, (1 - ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndDC(ItmPriceAfter, DocCurrRate, GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                                }
                                break;
                        }
                        #endregion

                        #region standard calculation
                        ItmDisValue = GFunc.RndUD(ItmPriceAfter * ItmDisPercent / 100M, PriceRoundMode, PriceDec);
                        ItmPriceUser = GFunc.RndUD(ItmPriceAfter - ItmDisValue, PriceRoundMode, PriceDec);
                        if (ItmType == (int)GEnum.INTypeGrp.Charges)
                            ItmAmtShw = ItmPriceUser;
                        else
                            ItmAmtShw = GFunc.RndC(ItmPriceUser * ItmQty, GVar.RndDecs.Amtpt);

                        if (objDoc.DocType == 110)   //if Tax Inclusive,
                        {
                            if (ItmTaxGrpRate > 0)
                                if (DocTaxRate == 0)
                                    ItmPrice = ItmPriceUser;
                                else
                                    ItmPrice = GFunc.RndDC(ItmPriceUser, (1 + ItmTaxGrpRate), GVar.RndDecs.Prcpt);
                            else
                                ItmPrice = ItmPriceUser;
                        }
                        else
                        {
                            ItmPrice = ItmPriceUser;
                        }

                        if (ItmType == (int)GEnum.INTypeGrp.Charges)
                            ItmAmtF = ItmPrice;
                        else
                            ItmAmtF = GFunc.RndC(ItmPrice * ItmQty, GVar.RndDecs.Amtpt);

                        ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                        #endregion

                        #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                        if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                        {
                            //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                            ItmTaxGrpKey = DocTaxKey;
                            ItmTaxGrpRate = 0;
                            ItmTaxGrpAmtF = 0;
                            ItmTaxGrpAmtL = 0;
                        }
                        else
                        {
                            if (ItmTaxGrpRate > 0)
                            {
                                ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                                ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                            }
                            else
                            {
                                ItmTaxGrpAmtF = 0;
                                ItmTaxGrpAmtL = 0;
                            }
                        }
                        #endregion

                        #region set grid with calculated values
                        grdRow.Cells["ItmPriceAfter"].Value = ItmPriceAfter.ToDBValue();
                        grdRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue();
                        grdRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue();
                        grdRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue();
                        grdRow.Cells["ItmPrice"].Value = ItmPrice;
                        grdRow.Cells["ItmAmtF"].Value = ItmAmtF;
                        grdRow.Cells["ItmAmtH"].Value = ItmAmtH;
                        grdRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                        grdRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;
                        #endregion
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
        public static bool ItmMarkupRate_Update(Document objDoc, DataRow dr, int MarkUpType, int PriceDec, int PriceRoundMode)
        {
            #region Declare variables
            bool RunCalculateAR = false;
            bool RunNullChargeItemQty = false;

            int? DocCurrKey = 0;
            int? DocTaxKey = 0;
            decimal? DocTaxRate = 0;
            decimal? DocCurrRate = 1;
            decimal? DocCountryRate = 1;

            int? ItmType = 0;
            decimal? ItmQty = 0;
            decimal? ItmListPrice = 0;
            int? ItmVendorCurrKey = 0;
            decimal? ItmVendorCurrRate = 0;
            decimal? ItmMarkupRate = 0;
            decimal? ItmMarkupRatio = 0;
            decimal? ItmVendorPrice = 0;
            decimal? ItmPriceAfter = 0;
            decimal? ItmDisPercent = 0;
            decimal? ItmDisValue = 0;
            decimal? ItmPriceUser = 0;
            decimal? ItmAmtShw = 0;
            decimal? ItmPrice = 0;
            decimal? ItmAmtF = 0;
            decimal? ItmAmtH = 0;
            int? ItmTaxGrpKey = 0;
            decimal? ItmTaxGrpRate = 0;
            decimal? ItmTaxGrpAmtF = 0;
            decimal? ItmTaxGrpAmtL = 0;
            #endregion

            try
            {
                #region Set process to run by DocCode and ItmType
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
                        if (GFunc.IsNEZ(dr["ItmKey"]))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        ItmType = GFunc.GetINTypeGroup(dr["ItmType"]);
                        switch (ItmType)
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                                dr["ItmVendorPriceRatio"] = GFunc.RndC(GFunc.NEDec(dr["ItmVendorPriceRatio"], 0), GVar.RndDecs.Prcpt);
                                RunCalculateAR = true;
                                break;

                            case (int)GEnum.INTypeGrp.Charges:
                                dr["ItmVendorPriceRatio"] = GFunc.RndC(GFunc.NEDec(dr["ItmVendorPriceRatio"], 0), GVar.RndDecs.Prcpt);
                                RunNullChargeItemQty = true;
                                RunCalculateAR = true;
                                break;

                            case (int)GEnum.INTypeGrp.Remark:
                                return true;

                            default:
                                //reset user input value
                                MsgBox.Show(MsgID.Document.NotAllowedInput + "%Markup Value");
                                dr["ItmVendorPriceRatio"] = DBNull.Value;
                                return false;
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region run NullChargeItemQty
                if (RunNullChargeItemQty)
                    dr["ItmQty"] = DBNull.Value;
                #endregion

                #region run RunCalculateAR
                if (RunCalculateAR)
                {
                    #region Setup variables
                    ItmTaxGrpRate = GFunc.NEDec(dr["ItmTaxGrpRate"], 0);
                    DocTaxKey = (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc);
                    DocTaxRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);

                    MarkUpType = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultDocMarkupType);
                    PriceDec = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceDecPlace);
                    PriceRoundMode = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceRoundMode);

                    DocCurrKey = (int)GFunc.GetPropertyValue("DocCurrKey", objDoc);
                    DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                    if (SysOptionUtility.GetInt("CountryCurrency") == 1) // Country Currency = Home -- Added by Jane. Mic to check
                        DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                    else
                        DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);

                    ItmQty = GFunc.NEDec(dr["ItmQty"], 0);
                    ItmListPrice = GFunc.NEDec(dr["ItmListPrice"], 0);
                    ItmVendorCurrKey = GFunc.NEInt(dr["ItmVendorCurrKey"], 1);
                    ItmVendorCurrRate = GFunc.NEDec(dr["ItmVendorCurrRate"], 1);
                    ItmVendorPrice = GFunc.NEDec(dr["ItmVendorPrice"], 0);
                    ItmMarkupRate = GFunc.NEDec(dr["ItmVendorPriceRatio"], 0);
                    ItmMarkupRatio = GFunc.RndDC(ItmMarkupRate, 100, GVar.RndDecs.Prcpt);
                    ItmPriceAfter = GFunc.NEDec(dr["ItmPriceAfter"], 0);
                    ItmDisPercent = GFunc.NEDec(dr["ItmDisPercent"], 0);
                    #endregion

                    if ((bool)dr["ItmVendorPriceLock"] == false)
                    {
                        #region Calculate ItmPriceAfter from MarkupType and run Row Calculation if required
                        switch (MarkUpType)
                        {
                            //Multiply by Retail Price
                            case 10:
                                ItmPriceAfter = GFunc.RndUD(ItmListPrice * (1 + ItmMarkupRatio), PriceRoundMode, PriceDec);
                                break;

                            //Multiply by Vendor Cost
                            case 20:
                                if (DocCurrKey == ItmVendorCurrKey)
                                {
                                    ItmPriceAfter = GFunc.RndUD(ItmVendorPrice * (1 + ItmMarkupRatio), PriceRoundMode, PriceDec);
                                }
                                else
                                {
                                    ItmPriceAfter = GFunc.RndC(ItmVendorPrice * ItmVendorCurrRate * (1 + ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndDC(ItmPriceAfter, DocCurrRate, GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                                }
                                break;

                            //Divided by Retail Price
                            case 30:
                                ItmPriceAfter = GFunc.RndDC(ItmListPrice, (1 - ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                                break;

                            //Divided by Vendor Cost
                            case 40:
                                if (DocCurrKey == ItmVendorCurrKey)
                                {
                                    ItmPriceAfter = GFunc.RndDC(ItmVendorPrice, (1 - ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                                }
                                else
                                {
                                    ItmPriceAfter = GFunc.RndDC(ItmVendorPrice * ItmVendorCurrRate, (1 - ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndDC(ItmPriceAfter, DocCurrRate, GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                                }
                                break;
                        }
                        #endregion

                        #region standard calculation
                        ItmDisValue = GFunc.RndUD(ItmPriceAfter * ItmDisPercent / 100M, PriceRoundMode, PriceDec);
                        ItmPriceUser = GFunc.RndUD(ItmPriceAfter - ItmDisValue, PriceRoundMode, PriceDec);
                        if (ItmType == (int)GEnum.INTypeGrp.Charges)
                            ItmAmtShw = ItmPriceUser;
                        else
                            ItmAmtShw = GFunc.RndC(ItmPriceUser * ItmQty, GVar.RndDecs.Amtpt);

                        if (objDoc.DocType == 110)   //if Tax Inclusive,
                        {
                            if (ItmTaxGrpRate > 0)
                                if (DocTaxRate == 0)
                                    ItmPrice = ItmPriceUser;
                                else
                                    ItmPrice = GFunc.RndDC(ItmPriceUser, (1 + ItmTaxGrpRate), GVar.RndDecs.Prcpt);
                            else
                                ItmPrice = ItmPriceUser;
                        }
                        else
                        {
                            ItmPrice = ItmPriceUser;
                        }

                        if (ItmType == (int)GEnum.INTypeGrp.Charges)
                            ItmAmtF = ItmPrice;
                        else
                            ItmAmtF = GFunc.RndC(ItmPrice * ItmQty, GVar.RndDecs.Amtpt);

                        ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                        #endregion

                        #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                        if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                        {
                            //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                            ItmTaxGrpKey = DocTaxKey;
                            ItmTaxGrpRate = 0;
                            ItmTaxGrpAmtF = 0;
                            ItmTaxGrpAmtL = 0;
                        }
                        else
                        {
                            if (ItmTaxGrpRate > 0)
                            {
                                ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                                ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                            }
                            else
                            {
                                ItmTaxGrpAmtF = 0;
                                ItmTaxGrpAmtL = 0;
                            }
                        }
                        #endregion

                        #region set grid with calculated values
                        dr["ItmPriceAfter"] = ItmPriceAfter.ToDBValue();
                        dr["ItmDisValue"] = ItmDisValue.ToDBValue();
                        dr["ItmPriceUser"] = ItmPriceUser.ToDBValue();
                        dr["ItmAmtShw"] = ItmAmtShw.ToDBValue();
                        dr["ItmPrice"] = ItmPrice;
                        dr["ItmAmtF"] = ItmAmtF;
                        dr["ItmAmtH"] = ItmAmtH;
                        dr["ItmTaxGrpAmtF"] = ItmTaxGrpAmtF;
                        dr["ItmTaxGrpAmtL"] = ItmTaxGrpAmtL;
                        #endregion
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
        private static bool ItmNewCost_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            //Note: ItmNewCost will only have value of 0: for Qty Adjustment and -1 for Cost Adjustment
            //For Batch Item no cost adjustment is allowed
            try
            {
                int itmType = 0;
                decimal itmNewCost = 0;
                decimal itmQty = 0;
                decimal itmCost = 0;

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }

                        itmType = (int)grd.ActiveRow.Cells["ItmType"].Value;
                        itmQty = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0), GVar.RndDecs.Qtypt);
                        itmCost = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmCost"].Value, 0), GVar.RndDecs.Prcpt);
                        itmNewCost = GFunc.NEDec(grd.ActiveRow.Cells["ItmNewCost"].Value, 0);

                        switch (itmType)
                        {
                            case (int)GEnum.ItemType.Finished_GDB:
                            case (int)GEnum.ItemType.Serial_Finished_GDB:
                            case (int)GEnum.ItemType.Serial_StockB:
                            case (int)GEnum.ItemType.StockB:
                                //For Batch, no cost adjustment is allowed
                                grd.ActiveRow.Cells["ItmNewCost"].Value = 0;
                                break;

                            default:
                                //when revalue item to new cost, the qty and cost cannot be -ve
                                grd.ActiveRow.Cells["ItmNewCost"].Value = itmNewCost;

                                if (itmNewCost < 0)
                                {
                                    if (itmCost < 0)
                                        grd.ActiveRow.Cells["ItmCost"].Value = 0;

                                    if (itmQty < 0)
                                        grd.ActiveRow.Cells["ItmQty"].Value = 0;
                                }
                                break;
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
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
        private static bool ItmOrderStatus_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Purchase_Order:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }

                        switch (GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value))
                        {
                            case (int)GEnum.INTypeGrp.Charges:
                                int ItmOrderStatusOldValue = GFunc.NEInt(grd.ActiveRow.Cells["ItmOrderStatus"].OriginalValue, 0);
                                int ItmOrderStatusNewValue = GFunc.NEInt(grd.ActiveRow.Cells["ItmOrderStatus"].Value, 0);

                                if (ItmOrderStatusOldValue == 20)   //Old Value is Delivered
                                {
                                    MsgBox.Show("Cannot change Status once it is delivered");
                                    return false;
                                }
                                else
                                {
                                    if (ItmOrderStatusNewValue == 20)   //New Value is Delivered
                                    {
                                        MsgBox.Show("Cannot change Status to delivered, status is updated to delivered once it is link to other document");
                                        return false;
                                    }
                                    else if (ItmOrderStatusNewValue == 0)//NA
                                    {
                                        MsgBox.Show("Cannot change Status to NA, status is must be Pending or Cancelled");
                                        return false;
                                    }
                                }
                                break;

                            case (int)GEnum.INTypeGrp.Remark:
                                return false;

                            default:
                                MsgBox.Show(MsgID.Document.NotAllowedInput + "%Order Status");
                                grd.ActiveRow.Cells["ItmOrderStatus"].Value = 0;
                                return true;
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
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

        private static bool ItmPrmDateNew_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                DateTime? itmPrmDateNew;
                DateTime itmPrmDate;
                int itmStatus;

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty.");
                            return false;
                        }
                        //If ItmStatus is not equal to "No Adjustment" and "Cancel"
                        if (GFunc.NEInt(grd.ActiveRow.Cells["ItmStatus"].Value, 0) != 10 && GFunc.NEInt(grd.ActiveRow.Cells["ItmStatus"].Value, 0) != 40)
                        {
                            if (GFunc.IsNE(grd.ActiveRow.Cells["ItmPrmDateNew"].Value))
                            {
                                ((DataRowView)grd.ActiveRow.ListObject).Row.RowError = "New Promised Date  cannot be empty.";
                                return false;
                            }
                        }
                        else
                        {                            
                            return true;
                        }

                        itmPrmDate = (DateTime)grd.ActiveRow.Cells["ItmPrmDate"].Value;
                        itmPrmDateNew = (DateTime)grd.ActiveRow.Cells["ItmPrmDateNew"].Value;

                        if (itmPrmDateNew > itmPrmDate)
                            itmStatus = (int)GEnum.ItmStatus.Postphone;

                        else if (itmPrmDateNew < itmPrmDate)
                            itmStatus = (int)GEnum.ItmStatus.Advance;

                        else
                            itmStatus = (int)GEnum.ItmStatus.No_Adjustment;

                        grd.ActiveRow.Cells["ItmStatus"].Value = itmStatus;
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
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
        private static bool ItmPrice_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                decimal docCurrRate = 1;
                decimal itmQty = 0;
                decimal itmPrice = 0;
                decimal itmDisPercent = 0;
                decimal itmDisPrice = 0;
                decimal itmAmtF = 0;
                decimal itmAmtH = 0;
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        docCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1); //Ask Mic
                        itmQty = GFunc.NEDec(grd.ActiveRow.Cells["itmQty"].Value, 0); //Ask Mic
                        itmPrice = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmPrice"].Value, 0), GVar.RndDecs.Prcpt); //Ask Mic
                        itmDisPercent = GFunc.NEDec(grd.ActiveRow.Cells["ItmDisPercent"].Value, 0);
                        if (itmPrice < 0)
                        {
                            MsgBox.Show("price cannot be < 0");
                            return false;
                        }
                        itmDisPrice = GFunc.NEDec(GFunc.RndC(itmPrice - itmPrice * itmDisPercent, GVar.RndDecs.Prcpt), 0); //Ask Mic
                        itmAmtF = GFunc.NEDec(GFunc.RndC(itmDisPrice * itmQty, GVar.RndDecs.Amtpt), 0);//Ask Mic
                        itmAmtH = GFunc.NEDec(GFunc.RndC(itmAmtF * docCurrRate, GVar.RndDecs.Amtpt), 0);//Ask Mic

                        grd.ActiveRow.Cells["ItmPrice"].Value = itmPrice;
                        grd.ActiveRow.Cells["ItmDisPrice"].Value = itmDisPrice;
                        grd.ActiveRow.Cells["ItmAmtF"].Value = itmAmtF;
                        grd.ActiveRow.Cells["ItmAmtH"].Value = itmAmtH;
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
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
        public static bool ItmPriceAfter_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            #region Declare variables
            bool RunCalculateAR = false;
            bool RunCalculateAP = false;
            bool RunCalculateTax = false;
            bool RunNullChargeItemQty = false;

            int MarkUpType = 0;
            int PriceDec = 0;
            int PriceRoundMode = 0;

            bool HaveTaxFields = false;
            int? DocTaxKey = 0;
            decimal? DocTaxRate = 0;
            int? DocCurrKey = 0;
            decimal? DocCurrRate = 1;
            decimal? DocCountryRate = 1;

            int? ItmType = 0;
            decimal? ItmQty = 0;
            decimal? ItmListPrice = 0;
            int? ItmVendorCurrKey = 0;
            decimal? ItmVendorCurrRate = 0;
            decimal? ItmMarkupRate = 0;
            decimal? ItmMarkupRatio = 0;
            decimal? ItmVendorPrice = 0;
            decimal? ItmPriceAfter = 0;
            decimal? ItmDisPercent = 0;
            decimal? ItmDisValue = 0;
            decimal? ItmPriceUser = 0;
            decimal? ItmPrice = 0;
            decimal? ItmAmtF = 0;
            decimal? ItmAmtH = 0;
            decimal? ItmAmtShw = 0;
            int? ItmTaxGrpKey = 0;
            decimal? ItmTaxGrpRate = 0;
            decimal? ItmTaxGrpAmtF = 0;
            decimal? ItmTaxGrpAmtL = 0;
            decimal? ItmTotalCost = 0;
            decimal? ItmGP = 0;
            #endregion

            try
            {
                #region Check DocCodeKey if any calculation is to be run
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
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }                        
                        HaveTaxFields = true;
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        break;

                    case (int)GEnum.SystemCode.Return_Consignment:
                        return true;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                switch (ItmType)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                    case (int)GEnum.INTypeGrp.Charges:
                        grd.ActiveRow.Cells["ItmPriceAfter"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmPriceAfter"].Value, 0), GVar.RndDecs.Prcpt);
                        break;

                    case (int)GEnum.INTypeGrp.Remark:
                        return true;

                    default:
                        MsgBox.Show(MsgID.Document.NotAllowedInput + "%Price");
                        grd.ActiveRow.Cells["ItmPriceAfter"].Value = DBNull.Value;
                        return false;
                }
                #endregion                
                //Get Process to Run

                if (Check_RunProcess((int)objDoc.DocCodeKey, grd, ref RunCalculateAR, ref RunCalculateAP, ref RunNullChargeItemQty, ref RunCalculateTax) == false)
                    return false;

                #region run NullChargeItemQty
                if (RunNullChargeItemQty)
                    grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;
                #endregion

                #region Setup Currency and tax variables
                if (HaveTaxFields)
                {
                    ItmTaxGrpRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmTaxGrpRate"].Value, 0);
                    DocTaxKey = (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc);
                    DocTaxRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);
                }
                else
                {
                    ItmTaxGrpKey = 0;
                    ItmTaxGrpRate = 0;
                    DocTaxKey = 0;
                    DocTaxRate = 0;
                }

                DocCurrKey = GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 1); //Ask Mic
                DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1); //Ask Mic

                int countryCurr = 1;
                if (((TAUtil.TAGridEditor)grd).ActiveConnection != null)
                    countryCurr = SysOptionUtility.GetInt("CountryCurrency",((TAUtil.TAGridEditor)grd).ActiveConnection);
                else
                    countryCurr = SysOptionUtility.GetInt("CountryCurrency");
                if (countryCurr== 1) // Country Currency = Home -- Added by Jane. Mic to check
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                else
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);
                #endregion/

                #region run RunCalculateAR
                if (RunCalculateAR)
                {
                    #region set variables
                    MarkUpType = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultDocMarkupType);
                    PriceDec = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceDecPlace);
                    PriceRoundMode = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceRoundMode);

                    ItmQty = GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0);
                    ItmListPrice = GFunc.NEDec(grd.ActiveRow.Cells["ItmListPrice"].Value, 0);
                    ItmVendorCurrKey = GFunc.NEInt(grd.ActiveRow.Cells["ItmVendorCurrKey"].Value, 1);
                    ItmVendorCurrRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorCurrRate"].Value, 1);
                    ItmVendorPrice = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorPrice"].Value, 0);
                    ItmMarkupRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorPriceRatio"].Value, 0);
                    ItmMarkupRatio = GFunc.RndDC(ItmMarkupRate, 100, GVar.RndDecs.Prcpt);
                    ItmPriceAfter = GFunc.NEDec(grd.ActiveRow.Cells["ItmPriceAfter"].Value, 0);
                    ItmDisPercent = GFunc.NEDec(grd.ActiveRow.Cells["ItmDisPercent"].Value, 0);
                    #endregion

                    #region Calculate ItmMarkUpRate from MarkupType and run Row Calculation if required
                    switch (MarkUpType)
                    {
                        //Multiply by Retail Price
                        case 10:
                            ItmMarkupRatio = GFunc.RndDC(ItmPriceAfter, ItmListPrice, GVar.RndDecs.Prcpt) - 1;
                            break;

                        //Multiply by Vendor Cost
                        case 20:
                            if (DocCurrKey == ItmVendorCurrKey)
                                ItmMarkupRatio = GFunc.RndDC(ItmPriceAfter, ItmVendorPrice, GVar.RndDecs.Prcpt) - 1;
                            else
                            {
                                ItmMarkupRatio = GFunc.RndC(ItmPriceAfter * DocCurrRate, GVar.RndDecs.Prcpt);
                                ItmMarkupRatio = GFunc.RndDC(ItmMarkupRatio, ItmVendorPrice * ItmVendorCurrRate, GVar.RndDecs.Prcpt) - 1;
                            }
                            break;

                        //Divided by Retail Price
                        case 30:
                            ItmMarkupRatio = 1 - GFunc.RndDC(ItmListPrice, ItmPriceAfter, GVar.RndDecs.Prcpt);
                            break;

                        //Divided by Vendor Cost
                        case 40:
                            if (DocCurrKey == ItmVendorCurrKey)
                                ItmMarkupRatio = 1 - GFunc.RndDC(ItmVendorPrice, ItmPriceAfter, GVar.RndDecs.Prcpt);
                            else
                            {
                                ItmMarkupRatio = GFunc.RndC(ItmVendorPrice * ItmVendorCurrRate, GVar.RndDecs.Prcpt);
                                ItmMarkupRatio = 1 - GFunc.RndDC(ItmMarkupRatio, ItmPriceAfter * DocCurrRate, GVar.RndDecs.Prcpt);
                            }
                            break;
                    }

                    //Divided by Retail Price Or  Divided by Vendor Cost
                    if (MarkUpType == 30 || MarkUpType == 40)
                        ItmMarkupRatio = DocComUtility.MarkUpRateDivisionLimit_Set(ItmMarkupRatio, MarkUpType, ItmListPrice, ItmVendorPrice, ItmPriceAfter);

                    ItmMarkupRate = GFunc.RndC(ItmMarkupRatio * 100, GVar.RndDecs.Qtypt);
                    grd.ActiveRow.Cells["ItmVendorPriceRatio"].Value = ItmMarkupRate;
                    #endregion

                    #region standard calculation
                    ItmDisValue = GFunc.RndUD(ItmPriceAfter * ItmDisPercent / 100M, PriceRoundMode, PriceDec);
                    ItmPriceUser = GFunc.RndUD(ItmPriceAfter - ItmDisValue, PriceRoundMode, PriceDec);

                    //---added by jane on 9-Jun-2014. Mic need to check!
                    //for eg. if itmpriceafter is 75.99. and the pricedec option is WHOLE NUMBER and priceroundmode is roundup, then calculated priceuser become 76
                    //so the difference of priceafter and priceuser will be the discount. 75.99 - 76 = -0.01
                    //Remark: if priceroundmode is roundup , discount will be (-)value and if rounddown, will be (+)value.
                    if (ItmPriceAfter - ItmDisValue != ItmPriceUser)
                    {
                        ItmDisValue = ItmPriceAfter - ItmPriceUser;
                    }
                    //----------------------

                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                        ItmAmtShw = ItmPriceUser;
                    else
                        ItmAmtShw = GFunc.RndC(ItmPriceUser * ItmQty, GVar.RndDecs.Amtpt);

                    if (objDoc.DocType == 110)   //if Tax Inclusive,
                    {
                        if (ItmTaxGrpRate > 0)
                            if (DocTaxRate == 0)
                                ItmPrice = ItmPriceUser;
                            else
                                ItmPrice = GFunc.RndDC(ItmPriceUser, (1 + ItmTaxGrpRate), GVar.RndDecs.Prcpt);
                        else
                            ItmPrice = ItmPriceUser;
                    }
                    else
                    {
                        ItmPrice = ItmPriceUser;
                    }

                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                        ItmAmtF = ItmPrice;
                    else
                        ItmAmtF = GFunc.RndC(ItmPrice * ItmQty, GVar.RndDecs.Amtpt);

                    ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                    #endregion

                    #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                    if (RunCalculateTax)
                    {
                        if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                        {
                            //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                            ItmTaxGrpKey = DocTaxKey;
                            ItmTaxGrpRate = 0;
                            ItmTaxGrpAmtF = 0;
                            ItmTaxGrpAmtL = 0;
                        }
                        else
                        {
                            if (ItmTaxGrpRate > 0)
                            {
                                ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                                //ItmTaxGrpAmtL = GFunc.RndC(ItmTaxGrpAmtF * DocCountryRate, GVar.RndDecs.Amtpt);
                                ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                            }
                            else
                            {
                                ItmTaxGrpAmtF = 0;
                                ItmTaxGrpAmtL = 0;
                            }
                        }
                    }
                    #endregion

                    #region Set grid with calculated values
                    grd.ActiveRow.Cells["ItmVendorPriceRatio"].Value = ItmMarkupRate;
                    grd.ActiveRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue();//Ask Mic
                    grd.ActiveRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue();//Ask Mic
                    grd.ActiveRow.Cells["ItmPrice"].Value = ItmPrice;
                    grd.ActiveRow.Cells["ItmAmtShw"].Value = ItmAmtShw;
                    grd.ActiveRow.Cells["ItmAmtF"].Value = ItmAmtF;
                    grd.ActiveRow.Cells["ItmAmtH"].Value = ItmAmtH;
                    if (HaveTaxFields)
                    {
                        grd.ActiveRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                        grd.ActiveRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;
                    }
                    if (SysOptionUtility.DatabaseBranchCode == DBCode.ADL && objDoc.DocCodeKey == (int)GEnum.SystemCode.Quotation) /* added by YST on 2023/07/15 -- requested by Zaw from Athena */
                    {
                        ItmTotalCost = GFunc.RndC(ItmQty * ItmVendorPrice, GVar.RndDecs.Amtpt);
                        ItmGP = ItmAmtShw - ItmTotalCost;
                        grd.ActiveRow.Cells["Custom1"].Value = String.Format("{0: #,##0.00; (#,##0.00)} ", ItmTotalCost);
                        grd.ActiveRow.Cells["Custom2"].Value = String.Format("{0: #,##0.00; (#,##0.00)} ", ItmGP); 
                        if (ItmAmtShw > 0)
                        {
                            grd.ActiveRow.Cells["Custom3"].Value = GFunc.RndC((ItmGP / ItmAmtShw) * 100, GVar.RndDecs.Amtpt).ToString() + "%"; /* Margin (%) for ADPL according to Zaw's formula */
                        }
                    }

                    #endregion
                }
                #endregion

                #region run RunCalculateAP
                if (RunCalculateAP)
                {
                    #region set variables
                    ItmQty = GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0);
                    ItmPriceAfter = GFunc.NEDec(grd.ActiveRow.Cells["ItmPriceAfter"].Value, 0);
                    ItmDisPercent = GFunc.NEDec(grd.ActiveRow.Cells["ItmDisPercent"].Value, 0);
                    ItmDisValue = GFunc.RndC(ItmPriceAfter * ItmDisPercent / 100M, GVar.RndDecs.Prcpt);
                    #endregion

                    #region standard calculation
                    ItmPriceUser = ItmPriceAfter - ItmDisValue;
                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                        ItmAmtShw = ItmPriceUser;
                    else
                        ItmAmtShw = GFunc.RndC(ItmPriceUser * ItmQty, GVar.RndDecs.Amtpt);

                    if (objDoc.DocType == 110)   //if Tax Inclusive,
                    {
                        if (ItmTaxGrpRate > 0)
                            if (DocTaxRate == 0)
                                ItmPrice = ItmPriceUser;
                            else
                                ItmPrice = GFunc.RndDC(ItmPriceUser, (1 + ItmTaxGrpRate), GVar.RndDecs.Prcpt);
                        else
                            ItmPrice = ItmPriceUser;
                    }
                    else
                    {
                        ItmPrice = ItmPriceUser;
                    }

                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                        ItmAmtF = ItmPrice;
                    else
                        ItmAmtF = GFunc.RndC(ItmPrice * ItmQty, GVar.RndDecs.Amtpt);

                    ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                    #endregion

                    #region Calcmulate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                    if (RunCalculateTax)
                    {
                        if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                        {
                            //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                            ItmTaxGrpKey = DocTaxKey;
                            ItmTaxGrpRate = 0;
                            ItmTaxGrpAmtF = 0;
                            ItmTaxGrpAmtL = 0;
                        }
                        else
                        {
                            if (ItmTaxGrpRate > 0)
                            {
                                ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                                ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                            }
                            else
                            {
                                ItmTaxGrpAmtF = 0;
                                ItmTaxGrpAmtL = 0;
                            }
                        }
                    }
                    #endregion

                    #region Set grid with calculated values
                    grd.ActiveRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue(); //Ask Mic
                    grd.ActiveRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue();//Ask Mic
                    grd.ActiveRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue();//Ask Mic
                    grd.ActiveRow.Cells["ItmPrice"].Value = ItmPrice;
                    grd.ActiveRow.Cells["ItmAmtF"].Value = ItmAmtF;
                    grd.ActiveRow.Cells["ItmAmtH"].Value = ItmAmtH;
                    if (HaveTaxFields)
                    {
                        grd.ActiveRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                        grd.ActiveRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;
                    }
                    #endregion
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
        public static bool ItmPriceAfterTransfer_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            #region Declare variables
            bool RunCalculateAR = false;
            bool RunCalculateAP = false;
            bool RunCalculateTax = false;
            bool RunNullChargeItemQty = false;

            int MarkUpType = 0;
            int PriceDec = 0;
            int PriceRoundMode = 0;

            bool HaveTaxFields = false;
            int? DocTaxKey = 0;
            decimal? DocTaxRate = 0;
            int? DocCurrKey = 0;
            decimal? DocCurrRate = 1;
            decimal? DocCountryRate = 1;

            int? ItmType = 0;
            decimal? ItmQty = 0;
            decimal? ItmListPrice = 0;
            int? ItmVendorCurrKey = 0;
            decimal? ItmVendorCurrRate = 0;
            decimal? ItmMarkupRate = 0;
            decimal? ItmMarkupRatio = 0;
            decimal? ItmVendorPrice = 0;
            decimal? ItmPriceAfter = 0;
            decimal? ItmDisPercent = 0;
            decimal? ItmDisValue = 0;
            decimal? ItmPriceUser = 0;
            decimal? ItmPrice = 0;
            decimal? ItmAmtF = 0;
            decimal? ItmAmtH = 0;
            decimal? ItmAmtShw = 0;
            int? ItmTaxGrpKey = 0;
            decimal? ItmTaxGrpRate = 0;
            decimal? ItmTaxGrpAmtF = 0;
            decimal? ItmTaxGrpAmtL = 0;
            decimal? ItmTotalCost = 0;
            decimal? ItmGP = 0;
            #endregion

            try
            {
                #region Check DocCodeKey if any calculation is to be run
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
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        HaveTaxFields = true;
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        break;

                    case (int)GEnum.SystemCode.Return_Consignment:
                        return true;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                switch (ItmType)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                    case (int)GEnum.INTypeGrp.Charges:
                        grd.ActiveRow.Cells["ItmPriceAfter"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmPriceAfter"].Value, 0), GVar.RndDecs.Prcpt);
                        break;

                    case (int)GEnum.INTypeGrp.Remark:
                        return true;

                    default:
                        MsgBox.Show(MsgID.Document.NotAllowedInput + "%Price");
                        grd.ActiveRow.Cells["ItmPriceAfter"].Value = DBNull.Value;
                        return false;
                }
                #endregion

                //Get Process to Run
                if (Check_RunProcess((int)objDoc.DocCodeKey, grd, ref RunCalculateAR, ref RunCalculateAP, ref RunNullChargeItemQty, ref RunCalculateTax) == false)
                    return false;

                #region run NullChargeItemQty
                //[if (RunNullChargeItemQty)
                //    grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;]
                #endregion

                #region Setup Currency and tax variables
                if (HaveTaxFields)
                {
                    ItmTaxGrpRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmTaxGrpRate"].Value, 0);
                    DocTaxKey = (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc);
                    DocTaxRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);
                }
                else
                {
                    ItmTaxGrpKey = 0;
                    ItmTaxGrpRate = 0;
                    DocTaxKey = 0;
                    DocTaxRate = 0;
                }

                DocCurrKey = GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 1); //Ask Mic
                DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1); //Ask Mic

                int countryCurr = 1;
                if (((TAUtil.TAGridEditor)grd).ActiveConnection != null)
                    countryCurr = SysOptionUtility.GetInt("CountryCurrency", ((TAUtil.TAGridEditor)grd).ActiveConnection);
                else
                    countryCurr = SysOptionUtility.GetInt("CountryCurrency");
                if (countryCurr == 1) // Country Currency = Home -- Added by Jane. Mic to check
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                else
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);
                #endregion/

                #region run RunCalculateAR
                if (RunCalculateAR)
                {
                    #region set variables
                    MarkUpType = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultDocMarkupType);
                    PriceDec = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceDecPlace);
                    PriceRoundMode = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceRoundMode);

                    ItmQty = GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0);
                    ItmListPrice = GFunc.NEDec(grd.ActiveRow.Cells["ItmListPrice"].Value, 0);
                    ItmVendorCurrKey = GFunc.NEInt(grd.ActiveRow.Cells["ItmVendorCurrKey"].Value, 1);
                    ItmVendorCurrRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorCurrRate"].Value, 1);
                    ItmVendorPrice = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorPrice"].Value, 0);
                    ItmMarkupRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorPriceRatio"].Value, 0);
                    ItmMarkupRatio = GFunc.RndDC(ItmMarkupRate, 100, GVar.RndDecs.Prcpt);
                    ItmPriceAfter = GFunc.NEDec(grd.ActiveRow.Cells["ItmPriceAfter"].Value, 0);
                    ItmDisPercent = GFunc.NEDec(grd.ActiveRow.Cells["ItmDisPercent"].Value, 0);
                    #endregion

                    #region Calculate ItmMarkUpRate from MarkupType and run Row Calculation if required
                    switch (MarkUpType)
                    {
                        //Multiply by Retail Price
                        case 10:
                            ItmMarkupRatio = GFunc.RndDC(ItmPriceAfter, ItmListPrice, GVar.RndDecs.Prcpt) - 1;
                            break;

                        //Multiply by Vendor Cost
                        case 20:
                            if (DocCurrKey == ItmVendorCurrKey)
                                ItmMarkupRatio = GFunc.RndDC(ItmPriceAfter, ItmVendorPrice, GVar.RndDecs.Prcpt) - 1;
                            else
                            {
                                ItmMarkupRatio = GFunc.RndC(ItmPriceAfter * DocCurrRate, GVar.RndDecs.Prcpt);
                                ItmMarkupRatio = GFunc.RndDC(ItmMarkupRatio, ItmVendorPrice * ItmVendorCurrRate, GVar.RndDecs.Prcpt) - 1;
                            }
                            break;

                        //Divided by Retail Price
                        case 30:
                            ItmMarkupRatio = 1 - GFunc.RndDC(ItmListPrice, ItmPriceAfter, GVar.RndDecs.Prcpt);
                            break;

                        //Divided by Vendor Cost
                        case 40:
                            if (DocCurrKey == ItmVendorCurrKey)
                                ItmMarkupRatio = 1 - GFunc.RndDC(ItmVendorPrice, ItmPriceAfter, GVar.RndDecs.Prcpt);
                            else
                            {
                                ItmMarkupRatio = GFunc.RndC(ItmVendorPrice * ItmVendorCurrRate, GVar.RndDecs.Prcpt);
                                ItmMarkupRatio = 1 - GFunc.RndDC(ItmMarkupRatio, ItmPriceAfter * DocCurrRate, GVar.RndDecs.Prcpt);
                            }
                            break;
                    }

                    //Divided by Retail Price Or  Divided by Vendor Cost
                    if (MarkUpType == 30 || MarkUpType == 40)
                        ItmMarkupRatio = DocComUtility.MarkUpRateDivisionLimit_Set(ItmMarkupRatio, MarkUpType, ItmListPrice, ItmVendorPrice, ItmPriceAfter);

                    ItmMarkupRate = GFunc.RndC(ItmMarkupRatio * 100, GVar.RndDecs.Qtypt);
                    grd.ActiveRow.Cells["ItmVendorPriceRatio"].Value = ItmMarkupRate;
                    #endregion

                    #region standard calculation
                    ItmDisValue = GFunc.RndUD(ItmPriceAfter * ItmDisPercent / 100M, PriceRoundMode, PriceDec);
                    ItmPriceUser = GFunc.RndUD(ItmPriceAfter - ItmDisValue, PriceRoundMode, PriceDec);
                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                        ItmAmtShw = ItmPriceUser;
                    else
                        ItmAmtShw = GFunc.RndC(ItmPriceUser * ItmQty, GVar.RndDecs.Amtpt);

                    if (objDoc.DocType == 110)   //if Tax Inclusive,
                    {
                        if (ItmTaxGrpRate > 0)
                            if (DocTaxRate == 0)
                                ItmPrice = ItmPriceUser;
                            else
                                ItmPrice = GFunc.RndDC(ItmPriceUser, (1 + ItmTaxGrpRate), GVar.RndDecs.Prcpt);
                        else
                            ItmPrice = ItmPriceUser;
                    }
                    else
                    {
                        ItmPrice = ItmPriceUser;
                    }

                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                        ItmAmtF = ItmPrice;
                    else
                        ItmAmtF = GFunc.RndC(ItmPrice * ItmQty, GVar.RndDecs.Amtpt);

                    ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                    #endregion

                    #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                    if (RunCalculateTax)
                    {
                        if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                        {
                            //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                            ItmTaxGrpKey = DocTaxKey;
                            ItmTaxGrpRate = 0;
                            ItmTaxGrpAmtF = 0;
                            ItmTaxGrpAmtL = 0;
                        }
                        else
                        {
                            if (ItmTaxGrpRate > 0)
                            {
                                ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                                //ItmTaxGrpAmtL = GFunc.RndC(ItmTaxGrpAmtF * DocCountryRate, GVar.RndDecs.Amtpt);
                                ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                            }
                            else
                            {
                                ItmTaxGrpAmtF = 0;
                                ItmTaxGrpAmtL = 0;
                            }
                        }
                    }
                    #endregion

                    #region Set grid with calculated values
                    grd.ActiveRow.Cells["ItmVendorPriceRatio"].Value = ItmMarkupRate;
                    grd.ActiveRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue();//Ask Mic
                    grd.ActiveRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue();//Ask Mic
                    grd.ActiveRow.Cells["ItmPrice"].Value = ItmPrice;
                    grd.ActiveRow.Cells["ItmAmtShw"].Value = ItmAmtShw;
                    grd.ActiveRow.Cells["ItmAmtF"].Value = ItmAmtF;
                    grd.ActiveRow.Cells["ItmAmtH"].Value = ItmAmtH;
                    if (HaveTaxFields)
                    {
                        grd.ActiveRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                        grd.ActiveRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;
                    }
                    if (SysOptionUtility.DatabaseBranchCode == DBCode.ADL && objDoc.DocCodeKey == (int)GEnum.SystemCode.Quotation) /* added by YST on 2023/07/15 -- requested by Zaw from Athena */
                    {
                        ItmTotalCost = GFunc.RndC(ItmQty * ItmVendorPrice, GVar.RndDecs.Amtpt);
                        ItmGP = ItmAmtShw - ItmTotalCost;
                        grd.ActiveRow.Cells["Custom1"].Value = String.Format("{0: #,##0.00; (#,##0.00)} ", ItmTotalCost);
                        grd.ActiveRow.Cells["Custom2"].Value = String.Format("{0: #,##0.00; (#,##0.00)} ", ItmGP);
                        if (ItmAmtShw > 0)
                        {
                            grd.ActiveRow.Cells["Custom3"].Value = GFunc.RndC((ItmGP / ItmAmtShw), GVar.RndDecs.Amtpt).ToString() + "%"; /* Margin (%) for ADPL according to Zaw's formula */
                        }                           
                    }
                    #endregion
                }
                #endregion

                #region run RunCalculateAP
                if (RunCalculateAP)
                {
                    #region set variables
                    ItmQty = GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0);
                    ItmPriceAfter = GFunc.NEDec(grd.ActiveRow.Cells["ItmPriceAfter"].Value, 0);
                    ItmDisPercent = GFunc.NEDec(grd.ActiveRow.Cells["ItmDisPercent"].Value, 0);
                    ItmDisValue = GFunc.RndC(ItmPriceAfter * ItmDisPercent / 100M, GVar.RndDecs.Prcpt);
                    #endregion

                    #region standard calculation
                    ItmPriceUser = ItmPriceAfter - ItmDisValue;
                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                        ItmAmtShw = ItmPriceUser;
                    else
                        ItmAmtShw = GFunc.RndC(ItmPriceUser * ItmQty, GVar.RndDecs.Amtpt);

                    if (objDoc.DocType == 110)   //if Tax Inclusive,
                    {
                        if (ItmTaxGrpRate > 0)
                            if (DocTaxRate == 0)
                                ItmPrice = ItmPriceUser;
                            else
                                ItmPrice = GFunc.RndDC(ItmPriceUser, (1 + ItmTaxGrpRate), GVar.RndDecs.Prcpt);
                        else
                            ItmPrice = ItmPriceUser;
                    }
                    else
                    {
                        ItmPrice = ItmPriceUser;
                    }

                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                        ItmAmtF = ItmPrice;
                    else
                        ItmAmtF = GFunc.RndC(ItmPrice * ItmQty, GVar.RndDecs.Amtpt);

                    ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                    #endregion

                    #region Calcmulate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                    if (RunCalculateTax)
                    {
                        if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                        {
                            //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                            ItmTaxGrpKey = DocTaxKey;
                            ItmTaxGrpRate = 0;
                            ItmTaxGrpAmtF = 0;
                            ItmTaxGrpAmtL = 0;
                        }
                        else
                        {
                            if (ItmTaxGrpRate > 0)
                            {
                                ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                                ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                            }
                            else
                            {
                                ItmTaxGrpAmtF = 0;
                                ItmTaxGrpAmtL = 0;
                            }
                        }
                    }
                    #endregion

                    #region Set grid with calculated values
                    grd.ActiveRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue(); //Ask Mic
                    grd.ActiveRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue();//Ask Mic
                    grd.ActiveRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue();//Ask Mic
                    grd.ActiveRow.Cells["ItmPrice"].Value = ItmPrice;
                    grd.ActiveRow.Cells["ItmAmtF"].Value = ItmAmtF;
                    grd.ActiveRow.Cells["ItmAmtH"].Value = ItmAmtH;
                    if (HaveTaxFields)
                    {
                        grd.ActiveRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                        grd.ActiveRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;
                    }
                    #endregion
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
        private static bool ItmPriceUser_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            #region Declare variables
            bool RunCalculateAP = false;
            bool RunNullChargeItemQty = false;
            bool RunCalculateTax = false;
            bool RunCalculateAR = false;

            bool HavetaxFields = false;
            int? DocTaxKey = 0;
            decimal? DocTaxRate = 0;
            decimal? DocCurrRate = 1;
            decimal? DocCountryRate = 1;

            int? ItmType = 0;
            decimal? ItmQty = 0;
            decimal? ItmPriceAfter = 0;
            decimal? ItmDisPercent = 0;
            decimal? ItmDisValue = 0;
            decimal? ItmPriceUser = 0;
            decimal? ItmAmtShw = 0;
            decimal? ItmPrice = 0;
            decimal? ItmAmtF = 0;
            decimal? ItmAmtH = 0;
            int? ItmTaxGrpKey = 0;
            decimal? ItmTaxGrpRate = 0;
            decimal? ItmTaxGrpAmtF = 0;
            decimal? ItmTaxGrpAmtL = 0;
            #endregion

            try
            {
                #region Check DocCodeKey if any calculation is to be run
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
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        HavetaxFields = true;
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                switch (ItmType)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                    case (int)GEnum.INTypeGrp.Charges:
                        grd.ActiveRow.Cells["ItmPriceUser"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmPriceUser"].Value, 0), GVar.RndDecs.Prcpt);
                        break;

                    case (int)GEnum.INTypeGrp.Remark:
                        return true;

                    default:
                        //reset user input value
                        MsgBox.Show(MsgID.Document.NotAllowedInput + "%Price");
                        grd.ActiveRow.Cells["ItmPriceUser"].Value = DBNull.Value;
                        return false;
                }
                #endregion

                if (Check_RunProcess((int)objDoc.DocCodeKey, grd, ref RunCalculateAR, ref RunCalculateAP, ref RunNullChargeItemQty, ref RunCalculateTax) == false)
                    return false;

                #region run NullChargeItemQty
                if (RunNullChargeItemQty)
                    grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;
                #endregion

                #region run RunCalculateARAP
                if (RunCalculateAR || RunCalculateAP)
                {
                    #region set variables
                    if (HavetaxFields)
                    {
                        ItmTaxGrpRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmTaxGrpRate"].Value, 0);
                        DocTaxKey = (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc);
                        DocTaxRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);
                    }
                    else
                    {
                        ItmTaxGrpKey = 0;
                        ItmTaxGrpRate = 0;
                        DocTaxKey = 0;
                        DocTaxRate = 0;
                    }

                    DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                    if (SysOptionUtility.GetInt("CountryCurrency") == 1) // Country Currency = Home -- Added by Jane. Mic to check
                        DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                    else
                        DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);

                    ItmQty = GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0);
                    ItmPriceAfter = GFunc.NEDec(grd.ActiveRow.Cells["ItmPriceAfter"].Value, 0);
                    ItmPriceUser = GFunc.NEDec(grd.ActiveRow.Cells["ItmPriceUser"].Value, 0);
                    #endregion

                    #region standard calculation
                    ItmDisValue = ItmPriceAfter - ItmPriceUser;
                    ItmDisPercent = GFunc.RndDC(ItmDisValue, ItmPriceUser, GVar.RndDecs.Prcpt);
                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                        ItmAmtShw = ItmPriceUser;
                    else
                        ItmAmtShw = GFunc.RndC(ItmQty * ItmPriceUser, GVar.RndDecs.Amtpt);

                    if (objDoc.DocType == 110)   //if Tax Inclusive,
                    {
                        if (ItmTaxGrpRate > 0)
                            if (DocTaxRate == 0)
                                ItmPrice = ItmPriceUser;
                            else
                                ItmPrice = GFunc.RndDC(ItmPriceUser, (1 + ItmTaxGrpRate), GVar.RndDecs.Prcpt);
                        else
                            ItmPrice = ItmPriceUser;
                    }
                    else
                    {
                        ItmPrice = ItmPriceUser;
                    }

                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                        ItmAmtF = ItmPrice;
                    else
                        ItmAmtF = GFunc.RndC(ItmPrice * ItmQty, GVar.RndDecs.Amtpt);

                    ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                    #endregion

                    #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable

                    if (RunCalculateTax)
                    {
                        if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                        {
                            //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                            ItmTaxGrpKey = DocTaxKey;
                            ItmTaxGrpRate = 0;
                            ItmTaxGrpAmtF = 0;
                            ItmTaxGrpAmtL = 0;
                        }
                        else
                        {
                            if (ItmTaxGrpRate > 0)
                            {
                                ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                                ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                            }
                            else
                            {
                                ItmTaxGrpAmtF = 0;
                                ItmTaxGrpAmtL = 0;
                            }
                        }
                    }
                    #endregion

                    #region Set grid with calculated values
                    grd.ActiveRow.Cells["ItmDisPercent"].Value = ItmDisPercent.ToDBValue();//Ask Mic
                    grd.ActiveRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue();//Ask Mic
                    grd.ActiveRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue();//Ask Mic
                    grd.ActiveRow.Cells["ItmPrice"].Value = ItmPrice;
                    grd.ActiveRow.Cells["ItmAmtF"].Value = ItmAmtF;
                    grd.ActiveRow.Cells["ItmAmtH"].Value = ItmAmtH;
                    if (HavetaxFields)
                    {
                        grd.ActiveRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                        grd.ActiveRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;
                    }
                    #endregion
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
        public static bool ItmQty_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            #region declaration
            string msgID = string.Empty;

            bool RunItmQtyAR = false;
            bool RunItmQtyAP = false;

            decimal? ItmQty = 0;
            decimal? ItmBQty = 0;       //Base Qty
            decimal? UOMConRate = 1;
            decimal? ItmDisPercent = 0;
            decimal? ItmQtyLink = 0;
            decimal? ItmQtyAdj = 0;
            int ItmType;
            #endregion

            try
            {
                #region Check DocCodeKey if any calculation is to be run
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
                    case (int)GEnum.SystemCode.Issue_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }                        
                        RunItmQtyAR = true;
                        break;

                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        RunItmQtyAP = true;
                        break;

                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }                        
                        RunItmQtyAP = true;
                        break;

                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }

                        decimal itmQty = GFunc.RndC(grd.ActiveRow.Cells["ItmQty"].Value, GVar.RndDecs.Qtypt);
                        int itmtype = (int)grd.ActiveRow.Cells["ItmType"].Value;
                        switch (itmtype)
                        {
                            case (int)GEnum.ItemType.Finished_GDB:
                            case (int)GEnum.ItemType.Serial_Finished_GDB:
                            case (int)GEnum.ItemType.Serial_StockB:
                            case (int)GEnum.ItemType.StockB:
                                if (GFunc.NEDec(grd.ActiveRow.Cells["ItmNewCost"].Value, 0) < 0)
                                    grd.ActiveRow.Cells["ItmNewCost"].Value = 0;//Disallow Cost Adjustment for Batch
                                else
                                {
                                    if (objDoc.DocType == 400 && itmQty < 0)//For Add New Batch Qty must be >= 0
                                    {
                                        MsgBox.Show("Qty must be >= 0");
                                        return false;
                                    }
                                }
                                break;

                            default:
                                if (GFunc.NEDec(grd.ActiveRow.Cells["ItmNewCost"].Value, 0) < 0)//Cost Adjustment
                                {
                                    if (itmQty < 0)
                                    {
                                        MsgBox.Show("Qty must be >= 0");
                                        return false;
                                    }
                                }
                                break;
                        }

                        grd.ActiveRow.Cells["ItmQty"].Value = itmQty;
                        if (itmQty < 0)
                            grd.ActiveRow.Cells["ItmCost"].Value = 0; 

                        return true;

                    case (int)GEnum.SystemCode.Packing_List:
                        grd.ActiveRow.Cells["ItmQty"].Value = GFunc.RndC(grd.ActiveRow.Cells["ItmQty"].Value, GVar.RndDecs.Qtypt);
                        return true;

                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        grd.ActiveRow.Cells["ItmQty"].Value = GFunc.RndC(grd.ActiveRow.Cells["ItmQty"].Value, GVar.RndDecs.Qtypt);
                        return true;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                                
                #region RunItmQtyAR
                if (RunItmQtyAR)
                {
                    switch (ItmType)
                    {
                        case (int)GEnum.INTypeGrp.Stock:
                        case (int)GEnum.INTypeGrp.Non_Stock:
                            //get Qty Discount percentage
                            ItmQty = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0), GVar.RndDecs.Qtypt);
                            UOMConRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmConRate"].Value, 1);
                            ItmBQty = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0) * UOMConRate, GVar.RndDecs.Qtypt); //Ask Mic
                            decimal dis = DocComUtility.QtyDiscount_Get(GFunc.GetIntPropertyValue("DocPriceType", objDoc), GFunc.NEInt(grd.ActiveRow.Cells["ItmKey"].Value, 0), ItmBQty);
                            if (dis > 0M)
                                ItmDisPercent = dis;
                            else
                                ItmDisPercent = GFunc.NEDec(grd.ActiveRow.Cells["ItmDisPercent"].Value, 0);

                            if (DocHDRUtil.Doc_CheckDetItm(objDoc, grd, GEnum.ValidateField.ItmQty) == false)
                                return false;
                            
                            //Set Values to grid and calculate row
                            grd.ActiveRow.Cells["ItmQty"].Value = ItmQty;
                            grd.ActiveRow.Cells["ItmDisPercent"].Value = ItmDisPercent;
                            if (ItmDisPercent_CustomUpdate(objDoc, grd) == false)
                                return false;
                            break;

                        case (int)GEnum.INTypeGrp.Charges:
                            //Calculate row when ItmQty is null
                            if (GFunc.IsNE(grd.ActiveRow.Cells["ItmQty"].Value))
                            {
                                if (ItmPriceAfter_CustomUpdate(objDoc, grd) == false)
                                    return false;
                            }
                            else
                            {
                                grd.ActiveRow.Cells["ItmQty"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0), GVar.RndDecs.Qtypt);
                                return true;
                            }
                            break;

                        case (int)GEnum.INTypeGrp.Discount:
                            if (!GFunc.IsNE(grd.ActiveRow.Cells["ItmQty"].Value))
                                grd.ActiveRow.Cells["ItmQty"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0), GVar.RndDecs.Qtypt);
                            return true;

                        case (int)GEnum.INTypeGrp.Remark:
                            if (GFunc.IsNE(grd.ActiveRow.Cells["ItmQty"].Text))
                                grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;
                            return true;

                        default:
                            MsgBox.Show(MsgID.Document.NotAllowedInput + "%Quantity");
                            grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;
                            return false;
                    }
                }
                #endregion

                #region RunItmQtyAP
                if (RunItmQtyAP)
                {
                    switch (ItmType)
                    {
                        case (int)GEnum.INTypeGrp.Stock:
                        case (int)GEnum.INTypeGrp.Non_Stock:

                            if (DocHDRUtil.Doc_CheckDetItm(objDoc, grd, GEnum.ValidateField.ItmQty) == false)
                                return false;

                            //Set Values to grid and calculate row
                            grd.ActiveRow.Cells["ItmQty"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0), GVar.RndDecs.Qtypt);
                            if (ItmPriceAfter_CustomUpdate(objDoc, grd) == false)
                                return false;
                            break;

                        case (int)GEnum.INTypeGrp.Charges:
                            //Calculate row when ItmQty is null
                            if (GFunc.IsNE(grd.ActiveRow.Cells["ItmQty"].Value))
                            {
                                if (ItmPriceAfter_CustomUpdate(objDoc, grd) == false)
                                    return false;
                            }
                            else
                            {
                                grd.ActiveRow.Cells["ItmQty"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0), GVar.RndDecs.Qtypt);
                            }
                            break;

                        case (int)GEnum.INTypeGrp.Discount:
                            if (!GFunc.IsNE(grd.ActiveRow.Cells["ItmQty"].Value))
                                grd.ActiveRow.Cells["ItmQty"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0), GVar.RndDecs.Qtypt);
                            break;

                        case (int)GEnum.INTypeGrp.Remark:
                            return true;

                        default:
                            MsgBox.Show(MsgID.Document.NotAllowedInput + "%Quantity");
                            grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;
                            return false;
                    }

                }
                #endregion

                //Change Qty Balance based on Qty 
                #region Set ItmQtyDelivered, ItmQtyBalance
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Order_Consignment:
                        switch (ItmType)
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                                ItmQty = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0), GVar.RndDecs.Qtypt);
                                ItmQtyLink = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQtyLink"].Value, 0), GVar.RndDecs.Qtypt);
                                ItmQtyAdj = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQtyAdj"].Value, 0), GVar.RndDecs.Qtypt);
                                grd.ActiveRow.Cells["ItmQtyBalance"].Value = ItmQty - ItmQtyLink - ItmQtyAdj;
                                break;
                        }
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                        switch (ItmType)
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                                ItmQty = GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0);
                                ItmQtyLink = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQtyLink"].Value, 0), GVar.RndDecs.Qtypt);
                                grd.ActiveRow.Cells["ItmQtyBalance"].Value = ItmQty - ItmQtyLink;
                                break;
                        }
                        break;
                    case (int)GEnum.SystemCode.Received_Consignment:
                        switch (ItmType)
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                                ItmQty = GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0);
                                ItmQtyLink = GFunc.RndC(GFunc.NEInt(grd.ActiveRow.Cells["ItmQtyLink"].Value, 0), GVar.RndDecs.Qtypt);
                                grd.ActiveRow.Cells["ItmQtyBalance"].Value = ItmQty - ItmQtyLink;
                                break;
                        }
                        break;

                }
                #endregion

                if (objDoc.DocTypeNm.Equals("Direct Shipment"))
                {
                    int itmtype = (int)grd.ActiveRow.Cells["ItmType"].Value;
                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Sales_Order:
                        case (int)GEnum.SystemCode.Delivery_Order:
                        case (int)GEnum.SystemCode.Sales_Invoice:
                        case (int)GEnum.SystemCode.Purchase_Order:
                        case (int)GEnum.SystemCode.Purchase_Shipment:
                        case (int)GEnum.SystemCode.Purchase_Delivery:
                        case (int)GEnum.SystemCode.Purchase_Invoice:
                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            if (itmtype == (int)GEnum.ItemType.Stock || itmtype == (int)GEnum.ItemType.Non_Stock)
                            {
                                ItmQty = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0), GVar.RndDecs.Qtypt);
                                grd.ActiveRow.Cells["DSQty"].Value = ItmQty;
                            }
                            break;
                    }
                }
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
        public static bool ItmQty_CustomUpdate(SqlConnection cn,Document objDoc, UltraGrid grd)
        {
            #region declaration
            string msgID = string.Empty;

            bool RunItmQtyAR = false;
            bool RunItmQtyAP = false;

            decimal? ItmQty = 0;
            decimal? ItmBQty = 0;       //Base Qty
            decimal? UOMConRate = 1;
            decimal? ItmDisPercent = 0;
            decimal? ItmQtyLink = 0;
            decimal? ItmQtyAdj = 0;
            int ItmType;
            #endregion

            try
            {
                #region Check DocCodeKey if any calculation is to be run
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
                    case (int)GEnum.SystemCode.Issue_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        #region To remove later  - to check Item Price User calculation ISSUE
                        grd.ActiveRow.Cells["Custom1"].Value = GFunc.NEStr(grd.ActiveRow.Cells["Custom1"].Value,"")+"/Q" + GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, -99999);
                        #endregion
                        RunItmQtyAR = true;
                        break;

                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        RunItmQtyAP = true;
                        break;

                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        RunItmQtyAP = true;
                        break;

                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }

                        decimal itmQty = GFunc.RndC(grd.ActiveRow.Cells["ItmQty"].Value, GVar.RndDecs.Qtypt);
                        int itmtype = (int)grd.ActiveRow.Cells["ItmType"].Value;
                        switch (itmtype)
                        {
                            case (int)GEnum.ItemType.Finished_GDB:
                            case (int)GEnum.ItemType.Serial_Finished_GDB:
                            case (int)GEnum.ItemType.Serial_StockB:
                            case (int)GEnum.ItemType.StockB:
                                if (GFunc.NEDec(grd.ActiveRow.Cells["ItmNewCost"].Value, 0) < 0)
                                    grd.ActiveRow.Cells["ItmNewCost"].Value = 0;//Disallow Cost Adjustment for Batch
                                else
                                {
                                    if (objDoc.DocType == 400 && itmQty < 0)//For Add New Batch Qty must be >= 0
                                    {
                                        MsgBox.Show("Qty must be >= 0");
                                        return false;
                                    }
                                }
                                break;

                            default:
                                if (GFunc.NEDec(grd.ActiveRow.Cells["ItmNewCost"].Value, 0) < 0)//Cost Adjustment
                                {
                                    if (itmQty < 0)
                                    {
                                        MsgBox.Show("Qty must be >= 0");
                                        return false;
                                    }
                                }
                                break;
                        }

                        grd.ActiveRow.Cells["ItmQty"].Value = itmQty;
                        if (itmQty < 0)
                            grd.ActiveRow.Cells["ItmCost"].Value = 0;

                        return true;

                    case (int)GEnum.SystemCode.Packing_List:
                        grd.ActiveRow.Cells["ItmQty"].Value = GFunc.RndC(grd.ActiveRow.Cells["ItmQty"].Value, GVar.RndDecs.Qtypt);
                        return true;

                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        grd.ActiveRow.Cells["ItmQty"].Value = GFunc.RndC(grd.ActiveRow.Cells["ItmQty"].Value, GVar.RndDecs.Qtypt);
                        return true;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);

                #region RunItmQtyAR
                if (RunItmQtyAR)
                {
                    switch (ItmType)
                    {
                        case (int)GEnum.INTypeGrp.Stock:
                        case (int)GEnum.INTypeGrp.Non_Stock:
                            //get Qty Discount percentage
                            ItmQty = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0), GVar.RndDecs.Qtypt);
                            UOMConRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmConRate"].Value, 1);
                            ItmBQty = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0) * UOMConRate, GVar.RndDecs.Qtypt); //Ask Mic
                            decimal dis = DocComUtility.QtyDiscount_Get(cn,GFunc.GetIntPropertyValue("DocPriceType", objDoc), GFunc.NEInt(grd.ActiveRow.Cells["ItmKey"].Value, 0), ItmBQty);
                            if (dis > 0M)
                                ItmDisPercent = dis;
                            else
                                ItmDisPercent = GFunc.NEDec(grd.ActiveRow.Cells["ItmDisPercent"].Value, 0);

                            if (DocHDRUtil.Doc_CheckDetItm(objDoc, grd, GEnum.ValidateField.ItmQty) == false)
                                return false;

                            //Set Values to grid and calculate row
                            grd.ActiveRow.Cells["ItmQty"].Value = ItmQty;
                            grd.ActiveRow.Cells["ItmDisPercent"].Value = ItmDisPercent;
                            if (ItmDisPercent_CustomUpdate(objDoc, grd) == false)
                                return false;
                            break;

                        case (int)GEnum.INTypeGrp.Charges:
                            //Calculate row when ItmQty is null
                            if (GFunc.IsNE(grd.ActiveRow.Cells["ItmQty"].Value))
                            {
                                if (ItmPriceAfter_CustomUpdate(objDoc, grd) == false)
                                    return false;
                            }
                            else
                            {
                                grd.ActiveRow.Cells["ItmQty"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0), GVar.RndDecs.Qtypt);
                                return true;
                            }
                            break;

                        case (int)GEnum.INTypeGrp.Discount:
                            if (!GFunc.IsNE(grd.ActiveRow.Cells["ItmQty"].Value))
                                grd.ActiveRow.Cells["ItmQty"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0), GVar.RndDecs.Qtypt);
                            return true;

                        case (int)GEnum.INTypeGrp.Remark:
                            if (GFunc.IsNE(grd.ActiveRow.Cells["ItmQty"].Text))
                                grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;
                            return true;

                        default:
                            MsgBox.Show(MsgID.Document.NotAllowedInput + "%Quantity");
                            grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;
                            return false;
                    }
                }
                #endregion

                #region RunItmQtyAP
                if (RunItmQtyAP)
                {
                    switch (ItmType)
                    {
                        case (int)GEnum.INTypeGrp.Stock:
                        case (int)GEnum.INTypeGrp.Non_Stock:

                            if (DocHDRUtil.Doc_CheckDetItm(objDoc, grd, GEnum.ValidateField.ItmQty) == false)
                                return false;

                            //Set Values to grid and calculate row
                            grd.ActiveRow.Cells["ItmQty"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0), GVar.RndDecs.Qtypt);
                            if (ItmPriceAfter_CustomUpdate(objDoc, grd) == false)
                                return false;
                            break;

                        case (int)GEnum.INTypeGrp.Charges:
                            //Calculate row when ItmQty is null
                            if (GFunc.IsNE(grd.ActiveRow.Cells["ItmQty"].Value))
                            {
                                if (ItmPriceAfter_CustomUpdate(objDoc, grd) == false)
                                    return false;
                            }
                            else
                            {
                                grd.ActiveRow.Cells["ItmQty"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0), GVar.RndDecs.Qtypt);
                            }
                            break;

                        case (int)GEnum.INTypeGrp.Discount:
                            if (!GFunc.IsNE(grd.ActiveRow.Cells["ItmQty"].Value))
                                grd.ActiveRow.Cells["ItmQty"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0), GVar.RndDecs.Qtypt);
                            break;

                        case (int)GEnum.INTypeGrp.Remark:
                            return true;

                        default:
                            MsgBox.Show(MsgID.Document.NotAllowedInput + "%Quantity");
                            grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;
                            return false;
                    }

                }
                #endregion

                //Change Qty Balance based on Qty 
                #region Set ItmQtyDelivered, ItmQtyBalance
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Order_Consignment:
                        switch (ItmType)
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                                ItmQty = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0), GVar.RndDecs.Qtypt);
                                ItmQtyLink = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQtyLink"].Value, 0), GVar.RndDecs.Qtypt);
                                ItmQtyAdj = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQtyAdj"].Value, 0), GVar.RndDecs.Qtypt);
                                grd.ActiveRow.Cells["ItmQtyBalance"].Value = ItmQty - ItmQtyLink - ItmQtyAdj;
                                break;
                        }
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                        switch (ItmType)
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                                ItmQty = GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0);
                                ItmQtyLink = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQtyLink"].Value, 0), GVar.RndDecs.Qtypt);
                                grd.ActiveRow.Cells["ItmQtyBalance"].Value = ItmQty - ItmQtyLink;
                                break;
                        }
                        break;
                    case (int)GEnum.SystemCode.Received_Consignment:
                        switch (ItmType)
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                                ItmQty = GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0);
                                ItmQtyLink = GFunc.RndC(GFunc.NEInt(grd.ActiveRow.Cells["ItmQtyLink"].Value, 0), GVar.RndDecs.Qtypt);
                                grd.ActiveRow.Cells["ItmQtyBalance"].Value = ItmQty - ItmQtyLink;
                                break;
                        }
                        break;

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

        private static bool ItmQty_DblClick(Document objDoc, UltraGrid grd)
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
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:

                        switch (GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, 0))
                        {                            
                            case (int)GEnum.ItemType.StockB:
                            case (int)GEnum.ItemType.Finished_GDB:
                            case (int)GEnum.ItemType.Serial_Finished_GDB:
                            case (int)GEnum.ItemType.Serial_StockB:

                                switch ((GEnum.SystemCode)objDoc.DocCodeKey)
                                {
                                    case GEnum.SystemCode.Delivery_Order:
                                    case GEnum.SystemCode.Sales_Invoice:
                                    case GEnum.SystemCode.Sales_Debit_Note:
                                    case GEnum.SystemCode.Sales_Credit_Note:
                                    case GEnum.SystemCode.Cash_Sale:
                                    case GEnum.SystemCode.Cash_Debit_Note:
                                    case GEnum.SystemCode.Cash_Credit_Note:
                                    case GEnum.SystemCode.Purchase_Debit_Note:
                                    case GEnum.SystemCode.Purchase_Credit_Note:
                                    case GEnum.SystemCode.Issue_Consignment:
                                    case GEnum.SystemCode.Return_Consignment:
                                    case GEnum.SystemCode.Inventory_Transfer:
                                    case GEnum.SystemCode.Purchase_Delivery:
                                    case GEnum.SystemCode.Inventory_Adjustment:
                                    case GEnum.SystemCode.Purchase_Invoice:
                                        frmBatchEntry batchPopup = new frmBatchEntry(objDoc, grd, false);
                                        batchPopup.ShowDialog();
                                        break;                                   
                                }
                                break;

                            case (int)GEnum.ItemType.Assembly:
                                bool vDataEntryMode = !(objDoc.DocCodeKey == (int)GEnum.SystemCode.Delivery_Order && SysOptionUtility.UseWMS);
                                frmAssemblyEntry AssemblyPopup = new frmAssemblyEntry(objDoc, grd, GFunc.NEInt(grd.ActiveRow.Cells["DocItmKey"].Value, 0), vDataEntryMode);
                                AssemblyPopup.ShowDialog();
                                break;

                            default:
                                break;
                        }
                        break;
                }
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
            finally
            {
                grd = null;
            }
        }//Completed
        private static bool ItmQtyDelivered_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            decimal? ItmQty = 0;
            decimal? ItmQtyDelivered = 0;
            decimal? ItmQtyLink = 0;
            decimal? ItmQtyAdj = 0;

            try
            {
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Order_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }

                        switch (GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value))
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                                ItmQty = GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0);
                                ItmQtyDelivered = GFunc.NEDec(grd.ActiveRow.Cells["ItmQtyDelivered"].Value, 0);
                                ItmQtyLink = GFunc.NEDec(grd.ActiveRow.Cells["ItmQtyLink"].Value, 0);
                                ItmQtyAdj = ItmQtyDelivered - ItmQtyLink;

                                //Set Values to grid and calculate row
                                grd.ActiveRow.Cells["ItmQtyDelivered"].Value = ItmQtyDelivered;
                                grd.ActiveRow.Cells["ItmQtyBalance"].Value = ItmQty - ItmQtyDelivered;
                                grd.ActiveRow.Cells["ItmQtyAdj"].Value = ItmQtyAdj;
                                break;

                            case (int)GEnum.INTypeGrp.Remark:
                                return false;

                            default:
                                MsgBox.Show(MsgID.Document.NotAllowedInput + "%NotReady QuantityDelivered");
                                grd.ActiveRow.Cells["ItmQtyDelivered"].Value = DBNull.Value;
                                return false;
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }

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
        private static void ItmQtyDelivere_DblClick(Document objDoc, UltraGrid grd)
        {
            try
            {
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        switch (GFunc.GetINTypeGroup(GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, 0)))
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                            case (int)GEnum.INTypeGrp.Charges:
                                frmItmQtyDeliveredHis orderPopup = new frmItmQtyDeliveredHis(objDoc.DocCodeKey, objDoc.DocKey, GFunc.NEInt(grd.ActiveRow.Cells["DocItmKey"].Value, 0));
                                orderPopup.ShowDialog();
                                break;
                        }
                        break;
                }
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
        private static void ItmControlPrice_DblClick(Document objDoc, UltraGrid grd)
        {
            int ItmKey = 0, ItmType = 0; string ItmID = "";
            ItmType = GFunc.NEInt(grd.ActiveRow.Cells["ItmType"].Value, 0);
            ItmKey = GFunc.NEInt(grd.ActiveRow.Cells["ItmKey"].Value, 0);
            ItmID = GFunc.NEStr(grd.ActiveRow.Cells["ItmID"].Value, "");

            if (ItmType == (int)GEnum.ItemType.Stock || ItmType == (int)GEnum.ItemType.Assembly)
            {
                frmPopupEstoreInfo f;
                //If it is already loaded, take that one
                foreach (Form form in Application.OpenForms[0].OwnedForms)
                {
                    if (form.Name == "frmPopupEstoreInfo")
                    {
                        f = (frmPopupEstoreInfo)form;
                        f.Reload(ItmKey, ItmID.Trim());
                        return;
                    }
                }

                //If it's not loaded yet, create new
                f = new frmPopupEstoreInfo(ItmKey, ItmID.Trim());
                f.Show(frmMain.gfrmMain);
            }
        }
        private static bool ItmStatus_CustomUpdate(Document objDoc, UltraGrid grd, int adjInterval, DateTime adjDefaultDate)
        {
            try
            {
                DateTime? itmPrmDateNew = null;
                DateTime itmPrmDate;
                GEnum.ItmStatus itmStatus;

                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }

                        itmStatus = (GEnum.ItmStatus)grd.ActiveRow.Cells["ItmStatus"].Value;
                        itmPrmDate = (DateTime)grd.ActiveRow.Cells["ItmPrmDate"].Value;

                        switch (itmStatus)
                        {
                            case GEnum.ItmStatus.Postphone:
                                if (adjInterval == 0)
                                    itmPrmDateNew = adjDefaultDate;
                                else
                                    itmPrmDateNew = itmPrmDate.AddMonths(adjInterval);

                                break;

                            case GEnum.ItmStatus.Advance:
                                if (adjInterval == 0)
                                    itmPrmDateNew = adjDefaultDate;
                                else
                                    itmPrmDateNew = itmPrmDate.AddMonths(-adjInterval);
                                break;

                            case GEnum.ItmStatus.Cancel:
                            case GEnum.ItmStatus.No_Adjustment:
                                itmPrmDateNew = null;
                                break;
                        }

                        grd.ActiveRow.Cells["ItmPrmDateNew"].Value = itmPrmDateNew;
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
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

        private static bool ItmTaxGrpKey_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            #region declaration
            int? DocTaxGrpKey = 0;
            decimal? DocTaxGrpRate = 0;

            string msgID = string.Empty;
            DateTime? DocDate = null;
            int? DocTaxKey = 0;
            decimal? DocTaxRate = 0;
            decimal? DocCurrRate = 1;
            decimal? DocCountryRate = 1;

            decimal? ItmAmtF = 0;
            decimal? ItmAmtH = 0;
            int? ItmTaxGrpKey = 0;
            decimal? ItmTaxGrpRate = 0;
            decimal? ItmTaxGrpAmtF = 0;
            decimal? ItmTaxGrpAmtL = 0;
            int ItmType = 0;
            #endregion

            try
            {
                #region Check DocCodeKey if any calculation is to be run
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
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region set variables
                DocTaxGrpKey = (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc);
                DocTaxGrpRate = (decimal?)GFunc.GetPropertyValue("DocTaxGrpRate", objDoc);
                ItmTaxGrpKey = GFunc.NEInt(grd.ActiveRow.Cells["ItmTaxGrpKey"].Value, 0);
                DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                if (SysOptionUtility.GetInt("CountryCurrency") == 1) // Country Currency = Home -- Added by Jane. Mic to check
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                else
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);
                DocDate = (DateTime?)GFunc.GetDatePropertyValue("DocDate", objDoc);
                DocTaxKey = (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc);
                DocTaxRate = (decimal?)GFunc.GetPropertyValue("DocTaxGrpRate", objDoc);
                ItmTaxGrpKey = GFunc.NEInt(grd.ActiveRow.Cells["ItmTaxGrpKey"].Value, 0);
                ItmAmtF = GFunc.NEDec(grd.ActiveRow.Cells["ItmAmtF"].Value, 0);
                ItmAmtH = GFunc.NEDec(grd.ActiveRow.Cells["ItmAmtH"].Value, 0);
                #endregion

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                switch (ItmType)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                    case (int)GEnum.INTypeGrp.Charges:
                    case (int)GEnum.INTypeGrp.Discount:
                        ItmTaxGrpRate = DocComUtility.TaxGrpRate_Get(ItmTaxGrpKey, GFunc.GetDatePropertyValue("DocDate", objDoc));

                        if (DocTaxGrpRate == 0 && ItmTaxGrpRate > 0)
                        {
                            MsgBox.Show(MsgID.Document.InvalidItmTaxGrpWhenDocTaxGrpIsZero);
                            return false;
                        }

                        #region commented by YST on 2023/01/18 to allow to key different ColumnTaxAmount requested by Josie/Susan from Finance Deapartment
                        /* 
                        //when itmtaxrate > 0 the taxgrpkey must be same
                        if ((ItmTaxGrpRate > 0 && DocTaxGrpKey != ItmTaxGrpKey) == true)
                        {
                            MsgBox.Show(MsgID.Common.MustBeSame + "%Item Tax Group%Doc Tax Group");
                            return false;
                        }
                        */
                        #endregion

                        break;

                    case (int)GEnum.INTypeGrp.Remark:
                        return true;

                    default:
                        MsgBox.Show(MsgID.Document.NotAllowedInput + "%Line Tax");
                        grd.ActiveRow.Cells["ItmTaxGrpKey"].Value = DBNull.Value;
                        return false;
                }
                #endregion

                #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                {
                    //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                    ItmTaxGrpKey = DocTaxKey;
                    ItmTaxGrpRate = 0;
                    ItmTaxGrpAmtF = 0;
                    ItmTaxGrpAmtL = 0;
                }
                else
                {
                    if (ItmTaxGrpRate > 0)
                    {
                        ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                        ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                    }
                    else
                    {
                        ItmTaxGrpAmtF = 0;
                        ItmTaxGrpAmtL = 0;
                    }
                }
                #endregion

                #region Set values to grid
                grd.ActiveRow.Cells["ItmTaxGrpKey"].Value = ItmTaxGrpKey.ToDBValue();
                grd.ActiveRow.Cells["ItmTaxGrpRate"].Value = ItmTaxGrpRate;
                grd.ActiveRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                grd.ActiveRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;
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
        
        public static bool ItmTranGrpID_btnClick(Document objDoc, UltraGrid grd)
        {
            try
            {
                frmPopupTreeView _frmPopupTreeView = new frmPopupTreeView();
                _frmPopupTreeView.ShowDialog();
                if (_frmPopupTreeView.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    grd.ActiveRow.Cells["ItmTranGrpKey"].Value = _frmPopupTreeView.TranGrpKey;
                    return ItmTranGrpKey_CustomUpdate(objDoc, grd);
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
        public static bool ItmTranGrpKey_CustomUpdate(Document objDoc, UltraGrid grd)
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
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }

                        switch (GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value))
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                            case (int)GEnum.INTypeGrp.Charges:
                            case (int)GEnum.INTypeGrp.Discount:
                            case (int)GEnum.INTypeGrp.Remark:
                            case (int)GEnum.INTypeGrp.Empty:
                                grd.ActiveRow.Cells["ItmTranGrpKey"].Value = GFunc.NEInt(grd.ActiveRow.Cells["ItmTranGrpKey"].Value, 0);
                                break;

                            default:
                                MsgBox.Show(MsgID.Document.NotAllowedInput + "%Transaction Group");
                                grd.ActiveRow.Cells["ItmTranGrpKey"].Value = 0;
                                return false;
                        }
                        break;

                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                        grd.ActiveRow.Cells["ItmTranGrpKey"].Value = GFunc.NEInt(grd.ActiveRow.Cells["ItmTranGrpKey"].Value, 0);
                        break;
                }
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
        public static bool ItmUOMKey_CustomUpdate(Document objDoc, UltraGrid grd,int? CallerDocCodeKey = 0)
        {
            try
            {
                #region declaration
                bool RunItmControlPrice = false;
                bool RunItmConRate = false;
                bool RunItmQty_Update = false;

                int ItmType = 0;
                decimal? CurrRate = 1;
                decimal? UOMConRate = 1;
                #endregion

                #region Check DocCodeKey if any calculation is to be run
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
                    case (int)GEnum.SystemCode.Issue_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        RunItmConRate = true;
                        RunItmControlPrice = true;
                        RunItmQty_Update = true;
                        break;

                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        RunItmConRate = true;
                        RunItmQty_Update = true;
                        break;

                    case (int)GEnum.SystemCode.Return_Consignment:
                        //this code is only use by DocHeader Utillity - TransferData Function
                        //we return true w/o any calculation as return consignment is not allowed to modify UOMKey
                        return true;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                switch (ItmType)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                        if (GFunc.IsNE(grd.ActiveRow.Cells["ItmUOMKey"].Value))
                        {
                            MsgBox.Show(MsgID.Common.CannotBeEmpty + "%UOM");
                            return false;
                        }
                        break;

                    case (int)GEnum.INTypeGrp.Remark:
                        return true;

                    default:
                        MsgBox.Show(MsgID.Document.NotAllowedInput + "%UOM");
                        grd.ActiveRow.Cells["ItmUOMKey"].Value = DBNull.Value;
                        return true;
                }
                #endregion

                #region Calculation
                if (RunItmConRate)
                {
                    switch (ItmType)
                    {
                        case (int)GEnum.INTypeGrp.Stock:
                        case (int)GEnum.INTypeGrp.Non_Stock:
                            //Set UOM Converstion Rate
                            UOMConRate = DocComUtility.UOMConRate_Get(GFunc.NEInt(grd.ActiveRow.Cells["ItmKey"].Value, 0), GFunc.NEInt(grd.ActiveRow.Cells["ItmUOMKey"].Value, 0),CallerDocCodeKey); //Ask Mic
                            grd.ActiveRow.Cells["ItmConRate"].Value = UOMConRate;
                            break;
                    }
                }

                if (RunItmControlPrice)
                {
                    switch (ItmType)
                    {
                        case (int)GEnum.INTypeGrp.Stock:
                        case (int)GEnum.INTypeGrp.Non_Stock:
                            CurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
                            grd.ActiveRow.Cells["ItmControlPrice"].Value = GFunc.RndDC(CurrRate * GFunc.NEDec(grd.ActiveRow.Cells["ItmControlPriceBase"].Value, 0), UOMConRate, GVar.RndDecs.Prcpt);
                            break;
                    }
                }

                if (RunItmQty_Update)
                {
                    if (!ItmQty_CustomUpdate(objDoc, grd))
                        return false;
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

        public static bool ItmUOMKey_CustomUpdate(SqlConnection cn, Document objDoc, UltraGrid grd,int? DocCodeKey = 0)
        {
            try
            {
                #region declaration
                bool RunItmControlPrice = false;
                bool RunItmConRate = false;
                bool RunItmQty_Update = false;

                int ItmType = 0;
                decimal? CurrRate = 1;
                decimal? UOMConRate = 1;
                #endregion

                #region Check DocCodeKey if any calculation is to be run
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
                    case (int)GEnum.SystemCode.Issue_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        RunItmConRate = true;
                        RunItmControlPrice = true;
                        RunItmQty_Update = true;
                        break;

                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        RunItmConRate = true;
                        RunItmQty_Update = true;
                        break;

                    case (int)GEnum.SystemCode.Return_Consignment:
                        //this code is only use by DocHeader Utillity - TransferData Function
                        //we return true w/o any calculation as return consignment is not allowed to modify UOMKey
                        return true;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                switch (ItmType)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                        if (GFunc.IsNE(grd.ActiveRow.Cells["ItmUOMKey"].Value))
                        {
                            MsgBox.Show(MsgID.Common.CannotBeEmpty + "%UOM");
                            return false;
                        }
                        break;

                    case (int)GEnum.INTypeGrp.Remark:
                        return true;

                    default:
                        MsgBox.Show(MsgID.Document.NotAllowedInput + "%UOM");
                        grd.ActiveRow.Cells["ItmUOMKey"].Value = DBNull.Value;
                        return true;
                }
                #endregion

                #region Calculation
                if (RunItmConRate)
                {
                    switch (ItmType)
                    {
                        case (int)GEnum.INTypeGrp.Stock:
                        case (int)GEnum.INTypeGrp.Non_Stock:
                            //Set UOM Converstion Rate
                            UOMConRate = DocComUtility.UOMConRate_Get(cn,GFunc.NEInt(grd.ActiveRow.Cells["ItmKey"].Value, 0), GFunc.NEInt(grd.ActiveRow.Cells["ItmUOMKey"].Value, 0),DocCodeKey); //Ask Mic
                            grd.ActiveRow.Cells["ItmConRate"].Value = UOMConRate;
                            break;
                    }
                }

                if (RunItmControlPrice)
                {
                    switch (ItmType)
                    {
                        case (int)GEnum.INTypeGrp.Stock:
                        case (int)GEnum.INTypeGrp.Non_Stock:
                            CurrRate = GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);
                            grd.ActiveRow.Cells["ItmControlPrice"].Value = GFunc.RndDC(CurrRate * GFunc.NEDec(grd.ActiveRow.Cells["ItmControlPriceBase"].Value, 0), UOMConRate, GVar.RndDecs.Prcpt);
                            break;
                    }
                }

                if (RunItmQty_Update)
                {
                    if (!ItmQty_CustomUpdate(cn,objDoc, grd))
                        return false;
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

        public static bool ItmVendorID_btnClick(Document objDoc, UltraGrid grd, UltraGridCell gridCell, GEnum.PopupType popUpType, string listSettingID)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;

                if (DocHDRUtil.EditorButton_Popup((int)objDoc.DocCodeKey, gridCell.Text, listSettingID, (int)popUpType, ref key, ref id, ref des))
                    return ItmVendorID_Update(objDoc, grd, key, id, des);
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
        public static bool ItmVendorID_CustomUpdate(Document objDoc, UltraGrid grd, GEnum.RecAccessType recAccessType, string listSettingID)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string ctrlValue = grd.ActiveCell.Text.ToString();
                int popUpType = 0;

                //get popUpType (user cannot change the vendor name it will always search for the vendor record)
                switch (recAccessType)
                {
                    case GEnum.RecAccessType.VendID:
                        popUpType = (int)GEnum.PopupType.VendID;
                        break;

                    case GEnum.RecAccessType.VendNm:
                        //When ItmVendorKey is NOT Null or empty or 0 means that the user has selected an CV but wish to amend the name w/o selecting another CV
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmVendorKey"].Value) == false)
                            return true;

                        popUpType = (int)GEnum.PopupType.VendNm;
                        break;

                    default:
                        return false;
                }

                key = GFunc.ConRecord_GetKey(recAccessType, listSettingID, ctrlValue, ref id, ref des, true);
                if (key == 0)
                {
                    if(grd.ActiveRow.Cells["ItmVendorKey"].Text!="")
                        if (DocHDRUtil.EditorButton_Popup((int)objDoc.DocCodeKey, ctrlValue, listSettingID, popUpType, ref key, ref id, ref des) == false)
                            return false;
                }

                return ItmVendorID_Update(objDoc, grd, key, id, des);
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
        //With Text Editor
        private static bool ItmVendorID_Update(Document objDoc, UltraGrid grd, TAUtil.TATextBoxEditor ctrl, int key, string id, string des)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return ItmVendorID_Update(cn, objDoc, grd, ctrl, key, id, des);
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
        private static bool ItmVendorID_Update(SqlConnection cn, Document objDoc, UltraGrid grd, TAUtil.TATextBoxEditor ctrl, int? key, string id, string des)
        {
            try
            {
                #region declare variables
                MSTCon objCon = null;
                string msgID = string.Empty;

                bool RunItmPriceAfterAR = true;
                int MarkUpType = 0;
                int PriceDec = 0;
                int PriceRoundMode = 0;

                DateTime? DocDate;
                int? DocCurrKey = 0;
                int? DocTaxKey = 0;
                decimal? DocTaxRate = 0;
                decimal? DocCurrRate = 1;
                decimal? DocCountryRate = 1;

                int? VendorPriceKey = 0;
                int? VendorCurrKey = 0;
                decimal? VendorCurrRate = 0;
                decimal? VendorPrice = 0;

                int? ItmKey = 0;
                int? ItmType = 0;
                decimal? ItmQty = 0;
                decimal? ItmListPrice = 0;
                int? ItmVendorCurrKey = 0;
                decimal? ItmVendorCurrRate = 0;
                decimal? ItmMarkupRate = 0;
                decimal? ItmMarkupRatio = 0;
                decimal? ItmVendorPrice = 0;
                decimal? ItmPriceAfter = 0;
                decimal? ItmDisPercent = 0;
                decimal? ItmDisValue = 0;
                decimal? ItmPriceUser = 0;
                decimal? ItmAmtShw = 0;
                decimal? ItmPrice = 0;
                decimal? ItmAmtF = 0;
                decimal? ItmAmtH = 0;
                int? ItmTaxGrpKey = 0;
                decimal? ItmTaxGrpRate = 0;
                decimal? ItmTaxGrpAmtF = 0;
                decimal? ItmTaxGrpAmtL = 0;
                #endregion

                #region Check DocCodeKey if any calculation is to be run
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
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show(cn,"Item ID cannot be empty");
                            return false;
                        }
                        break;

                    default:
                        MsgBox.Show(cn,"Unable to match document code");
                        return false;
                }
                #endregion

                if (ItmVendorID_Validation(cn, objDoc, grd, ctrl, key) == false)
                    return false;

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                switch (ItmType)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                        grd.ActiveRow.Cells["ItmVendorKey"].Value = key.ToDBValue(); //Ask Mic
                        grd.ActiveRow.Cells["ItmVendorID"].Value = id;
                        grd.ActiveRow.Cells["ItmVendorNm"].Value = des;
                        break;

                    case (int)GEnum.INTypeGrp.Charges:
                        grd.ActiveRow.Cells["ItmVendorKey"].Value = key.ToDBValue(); //Ask Mic
                        grd.ActiveRow.Cells["ItmVendorID"].Value = id;
                        grd.ActiveRow.Cells["ItmVendorNm"].Value = des;
                        grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;
                        break;

                    case (int)GEnum.INTypeGrp.Remark:
                        grd.ActiveRow.Cells["ItmVendorKey"].Value = key.ToDBValue(); //Ask Mic
                        grd.ActiveRow.Cells["ItmVendorID"].Value = id;
                        grd.ActiveRow.Cells["ItmVendorNm"].Value = des;
                        return true;

                    default:
                        MsgBox.Show(cn,MsgID.Document.NotAllowedInput + "%Vendor");
                        return false;
                }
                #endregion

                #region Set Variables
                DocDate = objDoc.DocDate;
                ItmKey = GFunc.NEInt(grd.ActiveRow.Cells["ItmKey"].Value, 0);//Ask Mic

                //Get Vendor Infor
                objCon = MSTCon.Get(cn, key);
                if (objCon.ConKey == null)
                {
                    VendorPriceKey = 0;
                    VendorCurrKey = 1;
                    VendorCurrRate = 1;
                    ItmVendorPrice = 0;
                }
                else
                {
                    VendorPriceKey = GFunc.NEInt(objCon.VPriceType, 0);
                    VendorCurrKey = GFunc.NEInt(objCon.VCurrkey, 1);
                    VendorCurrRate = DocComUtility.CurrRate_Get(VendorCurrKey, DocDate, true);
                    VendorPrice = DocComUtility.Price_Get(VendorPriceKey, ItmKey, key, VendorCurrKey);
                }

                ItmTaxGrpRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmTaxGrpRate"].Value, 0);
                DocTaxKey = (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc);
                DocTaxRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);

                MarkUpType = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultDocMarkupType);
                PriceDec = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceDecPlace);
                PriceRoundMode = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceRoundMode);

                DocCurrKey = GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 1); //Ask Mic
                DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                if (SysOptionUtility.GetInt("CountryCurrency", cn) == 1) // Country Currency = Home -- Added by Jane. Mic to check
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                else
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);

                ItmQty = GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0);
                ItmListPrice = GFunc.NEDec(grd.ActiveRow.Cells["ItmListPrice"].Value, 0);
                ItmVendorCurrKey = GFunc.NEInt(grd.ActiveRow.Cells["ItmVendorCurrKey"].Value, 1);
                ItmVendorCurrRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorCurrRate"].Value, 1);
                ItmVendorPrice = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorPrice"].Value, 0);
                ItmMarkupRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorPriceRatio"].Value, 0);
                ItmMarkupRatio = GFunc.RndDC(ItmMarkupRate, 100, GVar.RndDecs.Prcpt);
                ItmPriceAfter = GFunc.NEDec(grd.ActiveRow.Cells["ItmPriceAfter"].Value, 0);
                ItmDisPercent = GFunc.NEDec(grd.ActiveRow.Cells["ItmDisPercent"].Value, 0);
                #endregion

                #region check option if require to change vendor price and run ItmPriceAfterAR
                if (VendorCurrKey == ItmVendorCurrKey)
                {
                    if (SysOptionUtility.GetBool(GVar.SystemOption.OpID.DocumentDetailChangeVendorChangePrice))
                    {
                        ItmVendorCurrRate = VendorCurrRate;
                        ItmVendorPrice = VendorPrice;
                        RunItmPriceAfterAR = true;
                    }
                    else
                        return true;
                }
                else
                {
                    //When Current ItmVendorCurrKey <> New VendorCurrKey Option(DocumentDetailChangeVendorChangePrice) is ignore
                    ItmVendorCurrKey = VendorCurrKey;
                    ItmVendorCurrRate = VendorCurrRate;
                    ItmVendorPrice = VendorPrice;
                    RunItmPriceAfterAR = true;
                }
                #endregion

                if (RunItmPriceAfterAR)
                {
                    if ((bool)grd.ActiveRow.Cells["ItmVendorPriceLock"].Value)
                    {
                        #region if require calculate ItmMarkUpRate from MarkupType and run Row calculation
                        switch (MarkUpType)
                        {
                            //Multiply by Vendor Cost
                            case 20:
                                if (DocCurrKey == ItmVendorCurrKey)
                                    ItmMarkupRatio = GFunc.RndDC(ItmPriceAfter, ItmVendorPrice, GVar.RndDecs.Prcpt) - 1;
                                else
                                {
                                    ItmMarkupRatio = GFunc.RndC(ItmPriceAfter * DocCurrRate, GVar.RndDecs.Prcpt);
                                    ItmMarkupRatio = GFunc.RndDC(ItmMarkupRatio, ItmVendorPrice * ItmVendorCurrRate, GVar.RndDecs.Prcpt) - 1;
                                }
                                break;

                            //Divided by Vendor Cost
                            case 40:
                                if (DocCurrKey == ItmVendorCurrKey)
                                    ItmMarkupRatio = 1 - GFunc.RndDC(ItmVendorPrice, ItmPriceAfter, GVar.RndDecs.Prcpt);
                                else
                                {
                                    ItmMarkupRatio = GFunc.RndC(ItmVendorPrice * ItmVendorCurrRate, GVar.RndDecs.Prcpt);
                                    ItmMarkupRatio = 1 - GFunc.RndDC(ItmMarkupRatio, ItmPriceAfter * DocCurrRate, GVar.RndDecs.Prcpt);
                                }
                                break;

                            default:
                                //For 10 and 30 we will not calculate as it does not use VendorPrice
                                grd.ActiveRow.Cells["ItmVendorCurrKey"].Value = ItmVendorCurrKey;
                                grd.ActiveRow.Cells["ItmVendorCurrRate"].Value = ItmVendorCurrRate;
                                grd.ActiveRow.Cells["ItmVendorPrice"].Value = ItmVendorPrice;
                                return true;
                        }
                        #endregion

                        #region Row Calculation - ItmVendor CurrKey,CurrRate,Price, Ratio and set grid values
                        //Divided by Retail Price Or  Divided by Vendor Cost
                        if (MarkUpType == 30 || MarkUpType == 40)
                        {
                            ItmMarkupRatio = DocComUtility.MarkUpRateDivisionLimit_Set(ItmMarkupRatio, MarkUpType, ItmListPrice, ItmVendorPrice, ItmPriceAfter);
                        }
                        ItmMarkupRate = GFunc.RndC(ItmMarkupRatio * 100, GVar.RndDecs.Qtypt);

                        grd.ActiveRow.Cells["ItmVendorCurrKey"].Value = ItmVendorCurrKey;
                        grd.ActiveRow.Cells["ItmVendorCurrRate"].Value = ItmVendorCurrRate;
                        grd.ActiveRow.Cells["ItmVendorPrice"].Value = ItmVendorPrice;
                        grd.ActiveRow.Cells["ItmVendorPriceRatio"].Value = ItmMarkupRate;
                        #endregion

                    }
                    else
                    {
                        #region If require calculate ItmPriceAfter from MarkupType and run Row Calculation if required
                        switch (MarkUpType)
                        {
                            //Multiply by Vendor Cost
                            case 20:
                                if (DocCurrKey == ItmVendorCurrKey)
                                {
                                    ItmPriceAfter = GFunc.RndUD(ItmVendorPrice * (1 + ItmMarkupRatio), PriceRoundMode, PriceDec);
                                }
                                else
                                {
                                    ItmPriceAfter = GFunc.RndC(ItmVendorPrice * ItmVendorCurrRate * (1 + ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndDC(ItmPriceAfter, DocCurrRate, GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                                }
                                break;

                            //Divided by Vendor Cost
                            case 40:
                                if (DocCurrKey == ItmVendorCurrKey)
                                {
                                    ItmPriceAfter = GFunc.RndDC(ItmVendorPrice, (1 - ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                                }
                                else
                                {
                                    ItmPriceAfter = GFunc.RndDC(ItmVendorPrice * ItmVendorCurrRate, (1 - ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndDC(ItmPriceAfter, DocCurrRate, GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                                }
                                break;

                            default:
                                //For 10 and 30 we will not do anything to ItmMarkupRate as it does not use VendorPrice
                                break;
                        }
                        #endregion

                        #region Row Calculation - ItmVendor CurrKey,CurrRate,Price, PriceAfter,DisValue,PriceUser,Amt
                        ItmDisValue = GFunc.RndUD(ItmPriceAfter * ItmDisPercent / 100M, PriceRoundMode, PriceDec);
                        ItmPriceUser = GFunc.RndUD(ItmPriceAfter - ItmDisValue, PriceRoundMode, PriceDec);
                        if (ItmType == (int)GEnum.INTypeGrp.Charges)
                            ItmAmtShw = ItmPriceUser;
                        else
                            ItmAmtShw = GFunc.RndC(ItmPriceUser * ItmQty, GVar.RndDecs.Prcpt);

                        if (objDoc.DocType == 110)   //if Tax Inclusive,
                        {
                            if (ItmTaxGrpRate > 0)
                                if (DocTaxRate == 0)
                                    ItmPrice = ItmPriceUser;
                                else
                                    ItmPrice = GFunc.RndDC(ItmPriceUser, (1 + ItmTaxGrpRate), GVar.RndDecs.Prcpt);
                            else
                                ItmPrice = ItmPriceUser;
                        }
                        else
                        {
                            ItmPrice = ItmPriceUser;
                        }

                        if (ItmType == (int)GEnum.INTypeGrp.Charges)
                            ItmAmtF = ItmPrice;
                        else
                            ItmAmtF = GFunc.RndC(ItmPrice * ItmQty, GVar.RndDecs.Amtpt);

                        ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                        #endregion

                        #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                        if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                        {
                            //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                            ItmTaxGrpKey = DocTaxKey;
                            ItmTaxGrpRate = 0;
                            ItmTaxGrpAmtF = 0;
                            ItmTaxGrpAmtL = 0;
                        }
                        else
                        {
                            if (ItmTaxGrpRate > 0)
                            {
                                ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                                ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                            }
                            else
                            {
                                ItmTaxGrpAmtF = 0;
                                ItmTaxGrpAmtL = 0;
                            }
                        }
                        #endregion

                        #region set values to grid
                        grd.ActiveRow.Cells["ItmVendorCurrKey"].Value = ItmVendorCurrKey;
                        grd.ActiveRow.Cells["ItmVendorCurrRate"].Value = ItmVendorCurrRate;
                        grd.ActiveRow.Cells["ItmVendorPrice"].Value = ItmVendorPrice;
                        grd.ActiveRow.Cells["ItmPriceAfter"].Value = ItmPriceAfter.ToDBValue();
                        grd.ActiveRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue();
                        grd.ActiveRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue();
                        grd.ActiveRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue();
                        grd.ActiveRow.Cells["ItmPrice"].Value = ItmPrice;
                        grd.ActiveRow.Cells["ItmAmtF"].Value = ItmAmtF;
                        grd.ActiveRow.Cells["ItmAmtH"].Value = ItmAmtH;
                        grd.ActiveRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                        grd.ActiveRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;
                        #endregion
                    }
                }

                ctrl.IsDirty = false;
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
        private static bool ItmVendorID_Validation(SqlConnection cn, Document objDoc, UltraGrid grd, TAUtil.TATextBoxEditor ctrl, int? key)
        {
            try
            {
                #region check row is not empty
                if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                {
                    MsgBox.Show(cn,"Item ID cannot be empty");
                    return false;
                }
                #endregion

                #region check valid conkey
                if (GFunc.IsNEZ(key))
                {
                    switch (GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value))
                    {
                        case (int)GEnum.INTypeGrp.Discount:
                        case (int)GEnum.INTypeGrp.Total:
                        case (int)GEnum.INTypeGrp.Empty:
                            MsgBox.Show(cn,MsgID.Document.NotAllowedInput + "%Vendor");
                            return false;
                    }
                }
                #endregion

                #region check vendor is active
                //Mic Check; Jack Added; if vendor is deactivated, it will not be allowed to select
                MSTCon conObjTemp = MSTCon.Get(cn, key);
                if (conObjTemp.Inactive == true)
                {
                    MsgBox.Show("Vendor is inactive and cannot be selected!");
                    return false;
                }
                //End Jack Added
                #endregion

                #region check record access
                if (GFunc.IsNEZ(key))
                {
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
                        case (int)GEnum.SystemCode.Purchase_Shipment:
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
                }
                #endregion

                if (DocHDRUtil.Doc_CheckDetItm(objDoc, grd, GEnum.ValidateField.ItmConKey) == false)
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
        //With Grid Cell Editor
        public static bool ItmVendorID_Update(Document objDoc, UltraGrid grd, int key, string id, string des)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return ItmVendorID_Update(cn, objDoc, grd, key, id, des);
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
        internal static bool ItmVendorID_Update(SqlConnection cn, Document objDoc, UltraGrid grd, int key, string id, string des)
        {
            try
            {
                #region declare variables
                MSTCon objCon = null;
                string msgID = string.Empty;

                bool RunItmPriceAfterAR = true;
                int MarkUpType = 0;
                int PriceDec = 0;
                int PriceRoundMode = 0;

                DateTime? DocDate;
                int? DocCurrKey = 0;
                int? DocTaxKey = 0;
                decimal? DocTaxRate = 0;
                decimal? DocCurrRate = 1;
                decimal? DocCountryRate = 1;

                int? VendorPriceKey = 0;
                int? VendorCurrKey = 0;
                decimal? VendorCurrRate = 0;
                decimal? VendorPrice = 0;

                int? ItmKey = 0;
                int? ItmType = 0;
                decimal? ItmQty = 0;
                decimal? ItmListPrice = 0;
                int? ItmVendorCurrKey = 0;
                decimal? ItmVendorCurrRate = 0;
                decimal? ItmMarkupRate = 0;
                decimal? ItmMarkupRatio = 0;
                decimal? ItmVendorPrice = 0;
                decimal? ItmPriceAfter = 0;
                decimal? ItmDisPercent = 0;
                decimal? ItmDisValue = 0;
                decimal? ItmPriceUser = 0;
                decimal? ItmAmtShw = 0;
                decimal? ItmPrice = 0;
                decimal? ItmAmtF = 0;
                decimal? ItmAmtH = 0;
                int? ItmTaxGrpKey = 0;
                decimal? ItmTaxGrpRate = 0;
                decimal? ItmTaxGrpAmtF = 0;
                decimal? ItmTaxGrpAmtL = 0;
                #endregion

                #region Check DocCodeKey if any calculation is to be run
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
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show(cn,"Item ID cannot be empty");
                            return false;
                        }
                        break;

                    default:
                        MsgBox.Show(cn,"Unable to match document code");
                        return false;
                }
                #endregion

                if (ItmVendorID_Validation(cn, objDoc, grd, key) == false)
                    return false;

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                switch (ItmType)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                        grd.ActiveRow.Cells["ItmVendorKey"].Value = key;
                        grd.ActiveRow.Cells["ItmVendorNm"].Value = des;
                        break;

                    case (int)GEnum.INTypeGrp.Charges:
                        grd.ActiveRow.Cells["ItmVendorKey"].Value = key;
                        grd.ActiveRow.Cells["ItmVendorNm"].Value = des;
                        grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;
                        break;

                    case (int)GEnum.INTypeGrp.Remark:
                        grd.ActiveRow.Cells["ItmVendorKey"].Value = key;
                        grd.ActiveRow.Cells["ItmVendorNm"].Value = des;
                        return true;

                    default:
                        MsgBox.Show(cn,MsgID.Document.NotAllowedInput + "%Vendor");
                        return false;
                }
                #endregion

                #region Set Variables
                DocDate = objDoc.DocDate;
                ItmKey = GFunc.NEInt(grd.ActiveRow.Cells["ItmKey"].Value, 0);

                //Get Vendor Infor
                objCon = MSTCon.Get(cn, key);
                if (objCon.ConKey == null)
                {
                    VendorPriceKey = 0;
                    VendorCurrKey = 1;
                    VendorCurrRate = 1;
                    ItmVendorPrice = 0;
                }
                else
                {
                    VendorPriceKey = GFunc.NEInt(objCon.VPriceType, 0);
                    VendorCurrKey = GFunc.NEInt(objCon.VCurrkey, 1);
                    VendorCurrRate = DocComUtility.CurrRate_Get(VendorCurrKey, DocDate, true);
                    VendorPrice = DocComUtility.Price_Get(VendorPriceKey, ItmKey, key, VendorCurrKey);
                }

                ItmTaxGrpRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmTaxGrpRate"].Value, 0);
                DocTaxKey = GFunc.NEInt(GFunc.GetPropertyValue("DocTaxGrpKey", objDoc), 0);//Ask Mic
                DocTaxRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);

                MarkUpType = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultDocMarkupType);
                PriceDec = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceDecPlace);
                PriceRoundMode = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceRoundMode);

                DocCurrKey = GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 1); //Ask Mic
                DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);//Ask Mic
                if (SysOptionUtility.GetInt("CountryCurrency", cn) == 1) // Country Currency = Home -- Added by Jane. Mic to check
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                else
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);

                ItmQty = GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0);
                ItmListPrice = GFunc.NEDec(grd.ActiveRow.Cells["ItmListPrice"].Value, 0);
                ItmVendorCurrKey = VendorCurrKey;
                ItmVendorCurrRate = VendorCurrRate;
                ItmVendorPrice = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorPrice"].Value, 0);
                ItmMarkupRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorPriceRatio"].Value, 0);
                ItmMarkupRatio = GFunc.RndDC(ItmMarkupRate, 100, GVar.RndDecs.Prcpt);
                ItmPriceAfter = GFunc.NEDec(grd.ActiveRow.Cells["ItmPriceAfter"].Value, 0);
                ItmDisPercent = GFunc.NEDec(grd.ActiveRow.Cells["ItmDisPercent"].Value, 0);
                #endregion

                #region check option if require to change vendor price and run ItmPriceAfterAR
                if (VendorCurrKey == GFunc.NEInt(grd.ActiveRow.Cells["ItmVendorCurrKey"].Value, 1))
                {
                    if (SysOptionUtility.GetBool(GVar.SystemOption.OpID.DocumentDetailChangeVendorChangePrice))
                    {
                        ItmVendorCurrRate = VendorCurrRate;
                        ItmVendorPrice = VendorPrice;
                        RunItmPriceAfterAR = true;
                    }
                    else
                        return true;
                }
                else
                {
                    //When Current ItmVendorCurrKey <> New VendorCurrKey Option(DocumentDetailChangeVendorChangePrice) is ignore
                    ItmVendorCurrKey = VendorCurrKey;
                    ItmVendorCurrRate = VendorCurrRate;
                    ItmVendorPrice = VendorPrice;
                    RunItmPriceAfterAR = true;
                }
                #endregion

                if (RunItmPriceAfterAR)
                {
                    if ((bool)grd.ActiveRow.Cells["ItmVendorPriceLock"].Value || ItmMarkupRatio == 0)
                    {
                        #region if require calculate ItmMarkUpRate from MarkupType and run Row calculation
                        switch (MarkUpType)
                        {
                            //Multiply by Vendor Cost
                            case 20:
                                if (DocCurrKey == ItmVendorCurrKey)
                                    ItmMarkupRatio = GFunc.RndDC(ItmPriceAfter, ItmVendorPrice, GVar.RndDecs.Prcpt) - 1;
                                else
                                {
                                    ItmMarkupRatio = GFunc.RndC(ItmPriceAfter * DocCurrRate, GVar.RndDecs.Prcpt);
                                    ItmMarkupRatio = GFunc.RndDC(ItmMarkupRatio, ItmVendorPrice * ItmVendorCurrRate, GVar.RndDecs.Prcpt) - 1;
                                }
                                break;

                            //Divided by Vendor Cost
                            case 40:
                                if (DocCurrKey == ItmVendorCurrKey)
                                    ItmMarkupRatio = 1 - GFunc.RndDC(ItmVendorPrice, ItmPriceAfter, GVar.RndDecs.Prcpt);
                                else
                                {
                                    ItmMarkupRatio = GFunc.RndC(ItmVendorPrice * ItmVendorCurrRate, GVar.RndDecs.Prcpt);
                                    ItmMarkupRatio = 1 - GFunc.RndDC(ItmMarkupRatio, ItmPriceAfter * DocCurrRate, GVar.RndDecs.Prcpt);
                                }
                                break;

                            default:
                                //For 10 and 30 we will not calculate as it does not use VendorPrice
                                grd.ActiveRow.Cells["ItmVendorCurrKey"].Value = ItmVendorCurrKey;
                                grd.ActiveRow.Cells["ItmVendorCurrRate"].Value = ItmVendorCurrRate;
                                grd.ActiveRow.Cells["ItmVendorPrice"].Value = ItmVendorPrice;
                                return true;
                        }
                        #endregion

                        #region Row Calculation - ItmVendor CurrKey,CurrRate,Price, Ratio and set grid values
                        //Divided by Retail Price Or  Divided by Vendor Cost
                        if (MarkUpType == 30 || MarkUpType == 40)
                        {
                            ItmMarkupRatio = DocComUtility.MarkUpRateDivisionLimit_Set(ItmMarkupRatio, MarkUpType, ItmListPrice, ItmVendorPrice, ItmPriceAfter);
                        }
                        ItmMarkupRate = GFunc.RndC(ItmMarkupRatio * 100, GVar.RndDecs.Qtypt);

                        grd.ActiveRow.Cells["ItmVendorCurrKey"].Value = ItmVendorCurrKey;
                        grd.ActiveRow.Cells["ItmVendorCurrRate"].Value = ItmVendorCurrRate;
                        grd.ActiveRow.Cells["ItmVendorPrice"].Value = ItmVendorPrice;
                        grd.ActiveRow.Cells["ItmVendorPriceRatio"].Value = ItmMarkupRate;
                        #endregion

                    }
                    else
                    {
                        #region If require calculate ItmPriceAfter from MarkupType and run Row Calculation if required
                        switch (MarkUpType)
                        {
                            //Multiply by Vendor Cost
                            case 20:
                                if (DocCurrKey == ItmVendorCurrKey)
                                {
                                    ItmPriceAfter = GFunc.RndUD(ItmVendorPrice * (1 + ItmMarkupRatio), PriceRoundMode, PriceDec);
                                }
                                else
                                {
                                    ItmPriceAfter = GFunc.RndC(ItmVendorPrice * ItmVendorCurrRate * (1 + ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndDC(ItmPriceAfter, DocCurrRate, GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                                }
                                break;

                            //Divided by Vendor Cost
                            case 40:
                                if (DocCurrKey == ItmVendorCurrKey)
                                {
                                    ItmPriceAfter = GFunc.RndDC(ItmVendorPrice, (1 - ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                                }
                                else
                                {
                                    ItmPriceAfter = GFunc.RndDC(ItmVendorPrice * ItmVendorCurrRate, (1 - ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndDC(ItmPriceAfter, DocCurrRate, GVar.RndDecs.Prcpt);
                                    ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                                }
                                break;

                            default:
                                //For 10 and 30 we will not do anything to ItmMarkupRate as it does not use VendorPrice
                                break;
                        }
                        #endregion

                        #region Row Calculation - ItmVendor CurrKey,CurrRate,Price, PriceAfter,DisValue,PriceUser,Amt
                        ItmDisValue = GFunc.RndUD(ItmPriceAfter * ItmDisPercent / 100M, PriceRoundMode, PriceDec);
                        ItmPriceUser = GFunc.RndUD(ItmPriceAfter - ItmDisValue, PriceRoundMode, PriceDec);
                        if (ItmType == (int)GEnum.INTypeGrp.Charges)
                            ItmAmtShw = ItmPriceUser;
                        else
                            ItmAmtShw = GFunc.RndC(ItmPriceUser * ItmQty, GVar.RndDecs.Prcpt);

                        if (objDoc.DocType == 110)   //if Tax Inclusive,
                        {
                            if (ItmTaxGrpRate > 0)
                                if (DocTaxRate == 0)
                                    ItmPrice = ItmPriceUser;
                                else
                                    ItmPrice = GFunc.RndDC(ItmPriceUser, (1 + ItmTaxGrpRate), GVar.RndDecs.Prcpt);
                            else
                                ItmPrice = ItmPriceUser;
                        }
                        else
                        {
                            ItmPrice = ItmPriceUser;
                        }

                        if (ItmType == (int)GEnum.INTypeGrp.Charges)
                            ItmAmtF = ItmPrice;
                        else
                            ItmAmtF = GFunc.RndC(ItmPrice * ItmQty, GVar.RndDecs.Amtpt);

                        ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                        #endregion

                        #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                        if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                        {
                            //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                            ItmTaxGrpKey = DocTaxKey;
                            ItmTaxGrpRate = 0;
                            ItmTaxGrpAmtF = 0;
                            ItmTaxGrpAmtL = 0;
                        }
                        else
                        {
                            if (ItmTaxGrpRate > 0)
                            {
                                ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                                ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Mic
                            }
                            else
                            {
                                ItmTaxGrpAmtF = 0;
                                ItmTaxGrpAmtL = 0;
                            }
                        }
                        #endregion

                        #region set values to grid
                        grd.ActiveRow.Cells["ItmVendorCurrKey"].Value = ItmVendorCurrKey;
                        grd.ActiveRow.Cells["ItmVendorCurrRate"].Value = ItmVendorCurrRate;
                        grd.ActiveRow.Cells["ItmVendorPrice"].Value = ItmVendorPrice;
                        grd.ActiveRow.Cells["ItmPriceAfter"].Value = ItmPriceAfter.ToDBValue();
                        grd.ActiveRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue();
                        grd.ActiveRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue();
                        grd.ActiveRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue();
                        grd.ActiveRow.Cells["ItmPrice"].Value = ItmPrice;
                        grd.ActiveRow.Cells["ItmAmtF"].Value = ItmAmtF;
                        grd.ActiveRow.Cells["ItmAmtH"].Value = ItmAmtH;
                        grd.ActiveRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                        grd.ActiveRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;
                        #endregion
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
        public static bool ItmVendorID_Validation(SqlConnection cn, Document objDoc, UltraGrid grd, int? key)
        {
            try
            {
                #region check row is not empty
                if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                {
                    MsgBox.Show(cn,"Item ID cannot be empty");
                    return false;
                }
                #endregion

                #region check valid conkey
                if (GFunc.IsNEZ(key))
                {
                    switch (GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value))
                    {
                        case (int)GEnum.INTypeGrp.Discount:
                        case (int)GEnum.INTypeGrp.Total:
                        case (int)GEnum.INTypeGrp.Empty:
                            MsgBox.Show(cn,MsgID.Document.NotAllowedInput + "%Vendor");
                            return false;
                    }
                }
                #endregion

                #region check vendor is active
                // commented by Jane 02-Oct-2025, coz user can not create invoice from DO when itmvendor in DO is inactive.
                ////Mic Check; Jack Added; if vendor is deactivated, it will not be allowed to select
                //MSTCon conObjTemp = MSTCon.Get(cn, key);
                //if (conObjTemp.Inactive == true)
                //{
                //    MsgBox.Show("Vendor is inactive and cannot be selected!");
                //    grd.ActiveCell = grd.ActiveRow.Cells["ItmVendorKey"];
                //    return false;
                //}
                ////End Jack Added
                #endregion

                #region check record access
                if (GFunc.IsNEZ(key))
                {
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
                        case (int)GEnum.SystemCode.Purchase_Shipment:
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
                }
                #endregion

                if (DocHDRUtil.Doc_CheckDetItm(objDoc, grd, GEnum.ValidateField.ItmConKey) == false)
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

        private static bool ItmVendorCurrRate_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            #region Declare variables
            int MarkUpType = 0;
            int PriceDec = 0;
            int PriceRoundMode = 0;

            int? DocCurrKey = 0;
            int? DocTaxKey = 0;
            decimal? DocTaxRate = 0;
            decimal? DocCurrRate = 1;
            decimal? DocCountryRate = 1;

            int? ItmType = 0;
            decimal? ItmQty = 0;
            decimal? ItmListPrice = 0;
            int? ItmVendorCurrKey = 0;
            decimal? ItmVendorCurrRate = 0;
            decimal? ItmMarkupRate = 0;
            decimal? ItmMarkupRatio = 0;
            decimal? ItmVendorPrice = 0;
            decimal? ItmPriceAfter = 0;
            decimal? ItmDisPercent = 0;
            decimal? ItmDisValue = 0;
            decimal? ItmPriceUser = 0;
            decimal? ItmAmtShw = 0;
            decimal? ItmPrice = 0;
            decimal? ItmAmtF = 0;
            decimal? ItmAmtH = 0;
            int? ItmTaxGrpKey = 0;
            decimal? ItmTaxGrpRate = 0;
            decimal? ItmTaxGrpAmtF = 0;
            decimal? ItmTaxGrpAmtL = 0;
            #endregion

            try
            {
                #region Check DocCodeKey if any calculation is to be run
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
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                switch (ItmType)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                        grd.ActiveRow.Cells["ItmVendorCurrRate"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorCurrRate"].Value, 1), GVar.RndDecs.Curpt); //ASk Mic
                        break;

                    case (int)GEnum.INTypeGrp.Charges:
                        grd.ActiveRow.Cells["ItmVendorCurrRate"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorCurrRate"].Value, 1), GVar.RndDecs.Curpt); //Ask Mic
                        grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;
                        break;

                    case (int)GEnum.INTypeGrp.Remark:
                        return true;

                    default:
                        MsgBox.Show(MsgID.Document.NotAllowedInput + "%Vendor Currency Rate");
                        grd.ActiveRow.Cells["ItmVendorCurrRate"].Value = 1;
                        return false;
                }
                #endregion

                #region Set Variables
                ItmTaxGrpRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmTaxGrpRate"].Value, 0);
                DocTaxKey = (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc);
                DocTaxRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);

                MarkUpType = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultDocMarkupType);
                PriceDec = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceDecPlace);
                PriceRoundMode = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceRoundMode);

                DocCurrKey = GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 1); //Ask Mic
                DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1); //Ask Mic
                if (SysOptionUtility.GetInt("CountryCurrency") == 1) // Country Currency = Home -- Added by Jane. Mic to check
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                else
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);

                ItmQty = GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0);
                ItmListPrice = GFunc.NEDec(grd.ActiveRow.Cells["ItmListPrice"].Value, 0);
                ItmVendorCurrKey = GFunc.NEInt(grd.ActiveRow.Cells["ItmVendorCurrKey"].Value, 1);
                ItmVendorCurrRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorCurrRate"].Value, 1);
                ItmVendorPrice = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorPrice"].Value, 0);
                ItmMarkupRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorPriceRatio"].Value, 0);
                ItmMarkupRatio = GFunc.RndDC(ItmMarkupRate, 100, GVar.RndDecs.Prcpt);
                ItmPriceAfter = GFunc.NEDec(grd.ActiveRow.Cells["ItmPriceAfter"].Value, 0);
                ItmDisPercent = GFunc.NEDec(grd.ActiveRow.Cells["ItmDisPercent"].Value, 0);
                #endregion

                if ((bool)grd.ActiveRow.Cells["ItmVendorPriceLock"].Value || ItmMarkupRatio <= 0)
                {
                    #region Calculate ItmMarkUpRate from MarkupType and run Row calculation
                    switch (MarkUpType)
                    {
                        //Multiply by Vendor Cost
                        case 20:
                            if (DocCurrKey == ItmVendorCurrKey)
                                ItmMarkupRatio = GFunc.RndDC(ItmPriceAfter, ItmVendorPrice, GVar.RndDecs.Prcpt) - 1;
                            else
                            {
                                ItmMarkupRatio = GFunc.RndC(ItmPriceAfter * DocCurrRate, GVar.RndDecs.Prcpt);
                                ItmMarkupRatio = GFunc.RndDC(ItmMarkupRatio, ItmVendorPrice * ItmVendorCurrRate, GVar.RndDecs.Prcpt) - 1;
                            }
                            break;

                        //Divided by Vendor Cost
                        case 40:
                            if (DocCurrKey == ItmVendorCurrKey)
                                ItmMarkupRatio = 1 - GFunc.RndDC(ItmVendorPrice, ItmPriceAfter, GVar.RndDecs.Prcpt);
                            else
                            {
                                ItmMarkupRatio = GFunc.RndC(ItmVendorPrice * ItmVendorCurrRate, GVar.RndDecs.Prcpt);
                                ItmMarkupRatio = 1 - GFunc.RndDC(ItmMarkupRatio, ItmPriceAfter * DocCurrRate, GVar.RndDecs.Prcpt);
                            }
                            break;

                        default:
                            return true;
                    }
                    #endregion

                    #region Row Calculation and set grid values
                    if (MarkUpType == 30 || MarkUpType == 40)
                    {
                        ItmMarkupRatio = DocComUtility.MarkUpRateDivisionLimit_Set(ItmMarkupRatio, MarkUpType, ItmListPrice, ItmVendorPrice, ItmPriceAfter);
                    }
                    ItmMarkupRate = GFunc.RndC(ItmMarkupRatio * 100, GVar.RndDecs.Qtypt);
                    grd.ActiveRow.Cells["ItmVendorPriceRatio"].Value = ItmMarkupRate;
                    #endregion
                }
                else
                {
                    #region Calculate ItmPriceAfter from MarkupType and run Row Calculation if required
                    switch (MarkUpType)
                    {
                        //Multiply by Vendor Cost
                        case 20:
                            if (DocCurrKey == ItmVendorCurrKey)
                            {
                                ItmPriceAfter = GFunc.RndUD(ItmVendorPrice * (1 + ItmMarkupRatio), PriceRoundMode, PriceDec);
                            }
                            else
                            {
                                ItmPriceAfter = GFunc.RndC(ItmVendorPrice * ItmVendorCurrRate * (1 + ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                ItmPriceAfter = GFunc.RndDC(ItmPriceAfter, DocCurrRate, GVar.RndDecs.Prcpt);
                                ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                            }
                            break;

                        //Divided by Vendor Cost
                        case 40:
                            if (DocCurrKey == ItmVendorCurrKey)
                            {
                                ItmPriceAfter = GFunc.RndDC(ItmVendorPrice, (1 - ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                            }
                            else
                            {
                                ItmPriceAfter = GFunc.RndDC(ItmVendorPrice * ItmVendorCurrRate, (1 - ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                ItmPriceAfter = GFunc.RndDC(ItmPriceAfter, DocCurrRate, GVar.RndDecs.Prcpt);
                                ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                            }
                            break;

                        default:
                            return true;
                    }
                    #endregion

                    #region row calculation
                    ItmDisValue = GFunc.RndUD(ItmPriceAfter * ItmDisPercent / 100M, PriceRoundMode, PriceDec);
                    ItmPriceUser = GFunc.RndUD(ItmPriceAfter - ItmDisValue, PriceRoundMode, PriceDec);
                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                        ItmAmtShw = ItmPriceUser;
                    else
                        ItmAmtShw = GFunc.RndC(ItmPriceUser * ItmQty, GVar.RndDecs.Amtpt);

                    if (objDoc.DocType == 110)   //if Tax Inclusive,
                    {
                        if (ItmTaxGrpRate > 0)
                            if (DocTaxRate == 0)
                                ItmPrice = ItmPriceUser;
                            else
                                ItmPrice = GFunc.RndDC(ItmPriceUser, (1 + ItmTaxGrpRate), GVar.RndDecs.Prcpt);
                        else
                            ItmPrice = ItmPriceUser;
                    }
                    else
                    {
                        ItmPrice = ItmPriceUser;
                    }

                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                        ItmAmtF = ItmPrice;
                    else
                        ItmAmtF = GFunc.RndC(ItmPrice * ItmQty, GVar.RndDecs.Amtpt);

                    ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                    #endregion

                    #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                    if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                    {
                        //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                        ItmTaxGrpKey = DocTaxKey;
                        ItmTaxGrpRate = 0;
                        ItmTaxGrpAmtF = 0;
                        ItmTaxGrpAmtL = 0;
                    }
                    else
                    {
                        if (ItmTaxGrpRate > 0)
                        {
                            ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                            ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                        }
                        else
                        {
                            ItmTaxGrpAmtF = 0;
                            ItmTaxGrpAmtL = 0;
                        }
                    }
                    #endregion

                    #region set values to grid
                    grd.ActiveRow.Cells["ItmPriceAfter"].Value = ItmPriceAfter.ToDBValue();
                    grd.ActiveRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue();
                    grd.ActiveRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue();
                    grd.ActiveRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue();
                    grd.ActiveRow.Cells["ItmPrice"].Value = ItmPrice;
                    grd.ActiveRow.Cells["ItmAmtF"].Value = ItmAmtF;
                    grd.ActiveRow.Cells["ItmAmtH"].Value = ItmAmtH;
                    grd.ActiveRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                    grd.ActiveRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;
                    #endregion
                }

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
        private static bool ItmVendorPrice_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            #region Declare variables
            int MarkUpType = 0;
            int PriceDec = 0;
            int PriceRoundMode = 0;

            int? DocCurrKey = 0;
            int? DocTaxKey = 0;
            decimal? DocTaxRate = 0;
            decimal? DocCurrRate = 1;
            decimal? DocCountryRate = 1;

            int? ItmType = 0;
            decimal? ItmQty = 0;
            decimal? ItmListPrice = 0;
            int? ItmVendorCurrKey = 0;
            decimal? ItmVendorCurrRate = 0;
            decimal? ItmMarkupRate = 0;
            decimal? ItmMarkupRatio = 0;
            decimal? ItmVendorPrice = 0;
            decimal? ItmPriceAfter = 0;
            decimal? ItmDisPercent = 0;
            decimal? ItmDisValue = 0;
            decimal? ItmPriceUser = 0;
            decimal? ItmAmtShw = 0;
            decimal? ItmPrice = 0;
            decimal? ItmAmtF = 0;
            decimal? ItmAmtH = 0;
            int? ItmTaxGrpKey = 0;
            decimal? ItmTaxGrpRate = 0;
            decimal? ItmTaxGrpAmtF = 0;
            decimal? ItmTaxGrpAmtL = 0;
            decimal? ItmTotalCost = 0;
            decimal? ItmGP = 0;
            #endregion

            try
            {
                #region Check DocCodeKey if any calculation is to be run
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
                        if (GFunc.IsNEZ(grd.ActiveRow.Cells["ItmKey"].Value))
                        {
                            MsgBox.Show("Item ID cannot be empty");
                            return false;
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);
                switch (ItmType)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                        grd.ActiveRow.Cells["ItmVendorPrice"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorPrice"].Value, 0), GVar.RndDecs.Prcpt);
                        break;

                    case (int)GEnum.INTypeGrp.Charges:
                        grd.ActiveRow.Cells["ItmVendorPrice"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorPrice"].Value, 0), GVar.RndDecs.Prcpt);
                        grd.ActiveRow.Cells["ItmQty"].Value = DBNull.Value;
                        break;
                    case (int)GEnum.INTypeGrp.Remark:
                        return true;

                    default:
                        MsgBox.Show(MsgID.Document.NotAllowedInput + "%Vendor Price");
                        grd.ActiveRow.Cells["ItmVendorPrice"].Value = DBNull.Value;
                        return false;
                }
                #endregion

                #region Set Variables
                ItmTaxGrpRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmTaxGrpRate"].Value, 0);
                DocTaxKey = (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc);
                DocTaxRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);

                MarkUpType = SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultDocMarkupType);
                PriceDec = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceDecPlace);
                PriceRoundMode = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceRoundMode);

                DocCurrKey = GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 1); //Ask Mic
                DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                if (SysOptionUtility.GetInt("CountryCurrency") == 1) // Country Currency = Home -- Added by Jane. Mic to check
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                else
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);

                ItmQty = GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0);
                ItmListPrice = GFunc.NEDec(grd.ActiveRow.Cells["ItmListPrice"].Value, 0);
                ItmVendorCurrKey = GFunc.NEInt(grd.ActiveRow.Cells["ItmVendorCurrKey"].Value, 1);
                ItmVendorCurrRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorCurrRate"].Value, 1);
                ItmVendorPrice = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorPrice"].Value, 0);
                ItmMarkupRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmVendorPriceRatio"].Value, 0);
                ItmMarkupRatio = GFunc.RndDC(ItmMarkupRate, 100, GVar.RndDecs.Prcpt);
                ItmPriceAfter = GFunc.NEDec(grd.ActiveRow.Cells["ItmPriceAfter"].Value, 0);
                ItmDisPercent = GFunc.NEDec(grd.ActiveRow.Cells["ItmDisPercent"].Value, 0);
                #endregion

                if (MarkUpType > 0 && ((bool)grd.ActiveRow.Cells["ItmVendorPriceLock"].Value || ItmMarkupRatio <= 0M))
                {
                    if (!SysOptionUtility.GetBool("RecalculateMarkUpWhenVendPriceChange"))
                    {
                        return true;
                    }
                    #region Calculate ItmMarkUpRate from MarkupType and run Row calculation
                    switch (MarkUpType)
                    {
                        //Multiply by Vendor Cost
                        case 20:
                            if (DocCurrKey == ItmVendorCurrKey)
                                ItmMarkupRatio = GFunc.RndDC(ItmPriceAfter, ItmVendorPrice, GVar.RndDecs.Prcpt) - 1;
                            else
                            {
                                ItmMarkupRatio = GFunc.RndC(ItmPriceAfter * DocCurrRate, GVar.RndDecs.Prcpt);
                                ItmMarkupRatio = GFunc.RndDC(ItmMarkupRatio, ItmVendorPrice * ItmVendorCurrRate, GVar.RndDecs.Prcpt) - 1;
                            }
                            break;

                        //Divided by Vendor Cost
                        case 40:
                            if (DocCurrKey == ItmVendorCurrKey)
                                ItmMarkupRatio = 1 - GFunc.RndDC(ItmVendorPrice, ItmPriceAfter, GVar.RndDecs.Prcpt);
                            else
                            {
                                ItmMarkupRatio = GFunc.RndC(ItmVendorPrice * ItmVendorCurrRate, GVar.RndDecs.Prcpt);
                                ItmMarkupRatio = 1 - GFunc.RndDC(ItmMarkupRatio, ItmPriceAfter * DocCurrRate, GVar.RndDecs.Prcpt);
                            }
                            break;

                        default:
                            return true;
                    }
                    #endregion

                    #region Row Calculation and set grid values
                    //Divided by Retail Price Or  Divided by Vendor Cost
                    if (MarkUpType == 30 || MarkUpType == 40)
                    {
                        ItmMarkupRatio = DocComUtility.MarkUpRateDivisionLimit_Set(ItmMarkupRatio, MarkUpType, ItmListPrice, ItmVendorPrice, ItmPriceAfter);
                    }
                    ItmMarkupRate = GFunc.RndC(ItmMarkupRatio * 100, GVar.RndDecs.Qtypt);
                    grd.ActiveRow.Cells["ItmVendorPriceRatio"].Value = ItmMarkupRate;                    
                    #endregion
                }
                else
                {
                    #region Calculate ItmPriceAfter from MarkupType and run Row Calculation if required
                    switch (MarkUpType)//to test with TBS 
                    {
                        //Multiply by Vendor Cost
                        case 20:
                            if (DocCurrKey == ItmVendorCurrKey)
                            {
                                ItmPriceAfter = GFunc.RndUD(ItmVendorPrice * (1 + ItmMarkupRatio), PriceRoundMode, PriceDec);
                            }
                            else
                            {
                                ItmPriceAfter = GFunc.RndC(ItmVendorPrice * ItmVendorCurrRate * (1 + ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                ItmPriceAfter = GFunc.RndDC(ItmPriceAfter, DocCurrRate, GVar.RndDecs.Prcpt);
                                ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                            }
                            break;

                        //Divided by Vendor Cost
                        case 40:
                            if (DocCurrKey == ItmVendorCurrKey)
                            {
                                ItmPriceAfter = GFunc.RndDC(ItmVendorPrice, (1 - ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                            }
                            else
                            {
                                ItmPriceAfter = GFunc.RndDC(ItmVendorPrice * ItmVendorCurrRate, (1 - ItmMarkupRatio), GVar.RndDecs.Prcpt);
                                ItmPriceAfter = GFunc.RndDC(ItmPriceAfter, DocCurrRate, GVar.RndDecs.Prcpt);
                                ItmPriceAfter = GFunc.RndUD(ItmPriceAfter, PriceRoundMode, PriceDec);
                            }
                            break;
                        case 0:break;

                        default:
                            return true;
                    }
                    #endregion

                    #region row calculation
                    ItmDisValue = GFunc.RndUD(ItmPriceAfter * ItmDisPercent / 100M, PriceRoundMode, PriceDec);
                    ItmPriceUser = GFunc.RndUD(ItmPriceAfter - ItmDisValue, PriceRoundMode, PriceDec);
                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                        ItmAmtShw = ItmPriceUser;
                    else
                        ItmAmtShw = GFunc.RndC(ItmPriceUser * ItmQty, GVar.RndDecs.Amtpt);

                    if (objDoc.DocType == 110)   //if Tax Inclusive,
                    {
                        if (ItmTaxGrpRate > 0)
                            if (DocTaxRate == 0)
                                ItmPrice = ItmPriceUser;
                            else
                                ItmPrice = GFunc.RndDC(ItmPriceUser, (1 + ItmTaxGrpRate), GVar.RndDecs.Prcpt);
                        else
                            ItmPrice = ItmPriceUser;
                    }
                    else
                    {
                        ItmPrice = ItmPriceUser;
                    }

                    if (ItmType == (int)GEnum.INTypeGrp.Charges)
                        ItmAmtF = ItmPrice;
                    else
                        ItmAmtF = GFunc.RndC(ItmPrice * ItmQty, GVar.RndDecs.Amtpt);

                    ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                    #endregion

                    #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                    if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                    {
                        //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                        ItmTaxGrpKey = DocTaxKey;
                        ItmTaxGrpRate = 0;
                        ItmTaxGrpAmtF = 0;
                        ItmTaxGrpAmtL = 0;
                    }
                    else
                    {
                        if (ItmTaxGrpRate > 0)
                        {
                            ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                            ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                        }
                        else
                        {
                            ItmTaxGrpAmtF = 0;
                            ItmTaxGrpAmtL = 0;
                        }
                    }
                    #endregion

                    #region set values to grid
                    grd.ActiveRow.Cells["ItmPriceAfter"].Value = ItmPriceAfter.ToDBValue(); 
                    grd.ActiveRow.Cells["ItmDisValue"].Value = ItmDisValue.ToDBValue();
                    grd.ActiveRow.Cells["ItmPriceUser"].Value = ItmPriceUser.ToDBValue();
                    grd.ActiveRow.Cells["ItmAmtShw"].Value = ItmAmtShw.ToDBValue();
                    grd.ActiveRow.Cells["ItmPrice"].Value = ItmPrice;
                    grd.ActiveRow.Cells["ItmAmtF"].Value = ItmAmtF;
                    grd.ActiveRow.Cells["ItmAmtH"].Value = ItmAmtH;
                    grd.ActiveRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                    grd.ActiveRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;                    
                    #endregion
                }

                if (SysOptionUtility.DatabaseBranchCode == DBCode.ADL && objDoc.DocCodeKey == (int)GEnum.SystemCode.Quotation) /* added by YST on 2023/07/15 -- requested by Zaw from Athena */
                {
                    ItmTotalCost = GFunc.RndC(ItmQty * ItmVendorPrice, GVar.RndDecs.Amtpt);
                    ItmGP = ItmAmtShw - ItmTotalCost;
                    grd.ActiveRow.Cells["Custom1"].Value = String.Format("{0: #,##0.00; (#,##0.00)} ", ItmTotalCost);
                    grd.ActiveRow.Cells["Custom2"].Value = String.Format("{0: #,##0.00; (#,##0.00)} ", ItmGP);
                    if (ItmAmtShw > 0)
                    {
                        grd.ActiveRow.Cells["Custom3"].Value = GFunc.RndC((ItmGP / ItmAmtShw)*100, GVar.RndDecs.Amtpt).ToString() + "%"; /* Margin (%) for ADPL according to Zaw's formula */
                    }
                }

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
        public static bool ItmVendorVendID_btnClick(Document objDoc, UltraGrid grd, UltraGridCell gridCell, GEnum.PopupType popUpType, string listSettingID, int detkey, int itmkey, decimal qty)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;

                if (DocHDRUtil.EditorButton_Popup((int)objDoc.DocCodeKey, gridCell.Text, listSettingID, (int)popUpType, ref key, ref id, ref des))
                    return ItmVendorVendID_Update(objDoc, grd, detkey, itmkey, qty, key, id, des);
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
        public static bool ItmVendorVendID_CustomUpdate(Document objDoc, UltraGrid grd, string ListSettingID, GEnum.RecAccessType recAccessType, int detkey, int itmkey, decimal qty)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string ctrlValue = grd.ActiveCell.Text.ToString();
                int popUpType = 0;

                //get popUpType (user cannot change the vendor name it will always search for the vendor record)
                switch (recAccessType)
                {
                    case GEnum.RecAccessType.VendID:
                        popUpType = (int)GEnum.PopupType.VendID;
                        break;

                    case GEnum.RecAccessType.VendNm:
                        popUpType = (int)GEnum.PopupType.VendNm;
                        break;

                    default:
                        return false;
                }

                key = GFunc.ConRecord_GetKey(recAccessType, ListSettingID, ctrlValue, ref id, ref des, true);
                if (key == 0)
                {
                    if (DocHDRUtil.EditorButton_Popup(objDoc, (int)objDoc.DocCodeKey, ctrlValue, ListSettingID, popUpType, ref key, ref id, ref des) == false)
                        return false;
                }

                return ItmVendorVendID_Update(objDoc, grd, detkey, itmkey, qty, key, id, des);
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
        private static bool ItmVendorVendID_Update(Document objDoc, UltraGrid grd, int detkey, int itmkey, decimal qty, int key, string id, string des)
        {
            try
            {
                DataTable dt = grd.DataSource as DataTable;
                int countlimit = 0; //to handle vendorkey change on a new row
                if (grd.ActiveRow.IsAddRow == false)
                    countlimit = 1; //to handle vendorkey change on a existing row


                var varVendors = from row in dt.AsEnumerable()
                                 where ((row.Field<int>("DocItmKey") == detkey &&
                                 row.Field<int?>("VendorKey") == key))
                                 select new
                                 {
                                     VendorKey = row.Field<int?>("VendorKey")
                                 };
                if (varVendors.Count() > countlimit)
                {
                    MsgBox.Show("duplicate vendor, please select another vendor");
                    return false;
                }

                MSTCon objCon = MSTCon.Get(key);
                grd.ActiveRow.Cells["VendorEqItmQty"].Value = GFunc.NEDec(qty, 0);
                grd.ActiveRow.Cells["VendorEqItmPrice"].Value = DocComUtility.Price_Get(objCon.VPriceType, itmkey, key, objCon.VCurrkey);

                grd.ActiveRow.Cells["VendorKey"].Value = key;
                grd.ActiveRow.Cells["VendorID"].Value = id;
                grd.ActiveRow.Cells["VendorNm"].Value = des;

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

        private static bool ItmDocDeptKey_CustomeUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                int deptkey = GFunc.NEInt(grd.ActiveRow.Cells["ItmDocDeptKey"].Value, 0);
                grd.ActiveRow.Cells["ItmDocDeptKey"].Value = deptkey;
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
        private static bool ItmAmtF_CustomeUpdate(Document objDoc, UltraGrid grd)
        {
            //LineType = 4000 -IV ,4010- Adj, 4020-CSCPD return (this code is obsolete)  ,4030- Exp

            try
            {
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                        int LineType = GFunc.NEInt(grd.ActiveRow.Cells["LineType"].Value, 0);

                        if (LineType != 4000 || LineType != 4010)
                            grd.ActiveRow.Cells["LineType"].Value = 4030;

                        break;
                }
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
        private static bool ItmCountryRate_CustomeUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                decimal ItmCountryRate = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmCountryRate"].Value, 1), GVar.RndDecs.Curpt);
                grd.ActiveRow.Cells["ItmCountryRate"].Value = ItmCountryRate;
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
        private static bool ItmQtyM_CustomeUpdate(Document objDoc, UltraGrid grd, string ColNm)
        {
            try
            {
                decimal itmQtyTotal = 0;

                if (GFunc.NEInt(grd.ActiveRow.Cells["ItmKey"].Value, 0) > 0)
                {
                    grd.ActiveRow.Cells[ColNm].Value = GFunc.NEDec(grd.ActiveRow.Cells[ColNm].Value, 0);

                    for (int i = 0; i < 12; i++)
                    {
                        if (GFunc.NEDec(grd.ActiveRow.Cells["ItmQtyM" + (i + 1)].Value, 0) > 0)
                        {
                            itmQtyTotal = GFunc.RndC(itmQtyTotal + GFunc.NEDec(grd.ActiveRow.Cells["ItmQtyM" + (i + 1)].Value, 0), GVar.RndDecs.Qtypt);
                        }
                    }
                    grd.ActiveRow.Cells["ItmQtyMTotal"].Value = itmQtyTotal;
                }
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
        private static bool ItmPackWeightNet_CustomeUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                UltraGrid grdPack = null;
                UltraGridCell gridCell = null;
                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Pack, docDet, ref grdPack);
                if (grdPack.ActiveRow == null)
                    return false;

                gridCell = grdPack.ActiveRow.Cells["ItmPackWeightNet"];
                gridCell.Value = GFunc.NEDec(gridCell.Value, 0);
                return grdPack.ActiveRow.Update();
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
        private static bool ItmPackWeightTare_CustomeUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                UltraGrid grdPack = null;
                UltraGridCell gridCell = null;
                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Pack, docDet, ref grdPack);
                if (grdPack.ActiveRow == null)
                    return false;

                gridCell = grdPack.ActiveRow.Cells["ItmPackWeightTare"];
                gridCell.Value = GFunc.NEDec(gridCell.Value, 0);
                return grdPack.ActiveRow.Update();
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
        private static bool ItmPackWeightGross_CustomeUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                UltraGrid grdPack = null;
                UltraGridCell gridCell = null;
                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Pack, docDet, ref grdPack);
                if (grdPack.ActiveRow == null)
                    return false;

                gridCell = grdPack.ActiveRow.Cells["ItmPackWeightGross"];
                gridCell.Value = GFunc.NEDec(gridCell.Value, 0);
                return grdPack.ActiveRow.Update();
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
        private static bool ItmHeight_CustomeUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                UltraGrid grdPack = null;
                UltraGridCell gridCell = null;
                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Pack, docDet, ref grdPack);
                if (grdPack.ActiveRow == null)
                    return false;

                gridCell = grdPack.ActiveRow.Cells["ItmHeight"];
                gridCell.Value = GFunc.NEDec(gridCell.Value, 0);
                return grdPack.ActiveRow.Update();
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
        private static bool ItmWidth_CustomeUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                UltraGrid grdPack = null;
                UltraGridCell gridCell = null;
                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Pack, docDet, ref grdPack);
                if (grdPack.ActiveRow == null)
                    return false;

                gridCell = grdPack.ActiveRow.Cells["ItmWidth"];
                gridCell.Value = GFunc.NEDec(gridCell.Value, 0);
                return grdPack.ActiveRow.Update();
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
        private static bool ItmLength_CustomeUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                UltraGrid grdPack = null;
                UltraGridCell gridCell = null;
                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdPack);
                if (grdPack.ActiveRow == null)
                    return false;

                gridCell = grdPack.ActiveRow.Cells["ItmLength"];
                gridCell.Value = GFunc.NEDec(gridCell.Value, 0);
                return grdPack.ActiveRow.Update();
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
        private static bool DetItmDeptKey_CustomeUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                UltraGrid grdItm = null;
                UltraGridCell gridCell = null;
                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                if (grdItm.ActiveRow == null)
                    return false;

                gridCell = grdItm.ActiveRow.Cells["DetItmDeptKey"];
                gridCell.Value = GFunc.NEInt(gridCell.Value, 0);

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
        private static bool DetItmQtyPerPack_CustomeUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {

                UltraGrid grdItm = null;
                UltraGrid grdPack = null;
                UltraGridCell gridCell = null;
                decimal QtyPerPack = 0;
                decimal NoOfPack = 0;
                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Pack, docDet, ref grdPack);

                if (grdPack.ActiveRow == null)
                    return false;

                if (grdItm.ActiveRow == null)
                    return false;

                gridCell = grdItm.ActiveRow.Cells["DetItmQtyPerPack"];
                QtyPerPack = GFunc.NEDec(gridCell.Value, 0);
                NoOfPack = GFunc.NEDec(grdPack.ActiveRow.Cells["ItmQty"].Value, 0);//No of packing
                if (QtyPerPack > 0)
                {
                    gridCell.Row.Cells["DetItmQtyTotal"].Value = GFunc.RndC(QtyPerPack * NoOfPack, GVar.RndDecs.Qtypt);
                }
                gridCell.Value = QtyPerPack;
                return grdItm.ActiveRow.Update();
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
        private static bool DetItmQtyTotal_CustomeUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                UltraGrid grdItm = null;
                UltraGridCell gridCell = null;
                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                if (grdItm.ActiveRow == null)
                    return false;

                gridCell = grdItm.ActiveRow.Cells["DetItmQtyTotal"];
                gridCell.Value = GFunc.NEDec(gridCell.Value, 0);
                return grdItm.ActiveRow.Update();
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
        private static bool DetItmUOMKey_CustomeUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                UltraGrid grdItm = null;
                UltraGridCell gridCell = null;
                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                if (grdItm.ActiveRow == null)
                    return false;

                gridCell = grdItm.ActiveRow.Cells["DetItmUOMKey"];
                gridCell.Value = GFunc.NEInt(gridCell.Value, 0);
                grdItm.ActiveRow.Cells["DetItmConRate"].Value = DocComUtility.UOMConRate_Get((int)grdItm.ActiveRow.Cells["DetItmKey"].Value, (int)gridCell.Value);
                return grdItm.ActiveRow.Update();
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
        private static bool DetItmWeightGross_CustomeUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                UltraGrid grdItm = null;
                UltraGridCell gridCell = null;
                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                if (grdItm.ActiveRow == null)
                    return false;

                gridCell = grdItm.ActiveRow.Cells["DetItmWeightGross"];
                gridCell.Value = GFunc.NEDec(gridCell.Value, 0);
                return grdItm.ActiveRow.Update();
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
        private static bool DetItmWeightNet_CustomeUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {
                UltraGrid grdItm = null;
                UltraGridCell gridCell = null;
                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                if (grdItm.ActiveRow == null)
                    return false;

                gridCell = grdItm.ActiveRow.Cells["DetItmWeightNet"];
                gridCell.Value = GFunc.NEDec(gridCell.Value, 0);
                return grdItm.ActiveRow.Update();
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
        private static bool DetItmWeightUOMKey_CustomeUpdate(Document objDoc, Hashtable docDet)
        {
            try
            {

                UltraGrid grdItm = null;
                UltraGridCell gridCell = null;
                DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, docDet, ref grdItm);
                if (grdItm.ActiveRow == null)
                    return false;

                gridCell = grdItm.ActiveRow.Cells["DetItmWeightUOMKey"];
                gridCell.Value = GFunc.NEInt(gridCell.Value, 0);
                grdItm.ActiveRow.Cells["DetItmWeightUOMRate"].Value = DocComUtility.UOMGramRate_Get((int)gridCell.Value);
                return grdItm.ActiveRow.Update();
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
        private static bool ItmFGKey_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                if (Check_DuplicateItm(objDoc, grd, GFunc.NEInt(grd.ActiveRow.Cells["ItmKey"].Value, 0), GFunc.NEInt(grd.ActiveCell.Value, 0)))
                {
                    grd.ActiveRow.Cells["ItmFGKey"].Value = GFunc.NEInt(grd.ActiveCell.Value,0);
                    grd.ActiveRow.Cells["ItmFGKeySelect"].Value = GFunc.NEInt(grd.ActiveCell.Value, 0);
                    return true;
                }
                else
                    return false;
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

        private static bool ExpAmt_CustomeUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                #region Check DocCodeKey if any calculation is to be run
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        grd.ActiveRow.Cells["ExpAmt"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ExpAmt"].Value, 0), GVar.RndDecs.Amtpt); //Ask Mic
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
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
        public static bool ExpAmtGST_CustomeUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                #region Declare variables
                int? DocTaxKey = 0;
                decimal? DocTaxRate = 0;
                decimal? DocCurrRate = 1;
                decimal? DocCountryRate = 1;

                decimal? ExpAmtGST = 0;
                decimal? ExpAmtF = 0;
                decimal? ExpAmtH = 0;
                int? ExpTaxGrpKey = 0;
                decimal? ExpTaxGrpRate = 0;
                decimal? ExpTaxGrpAmtF = 0;
                decimal? ExpTaxGrpAmtL = 0;
                #endregion

               
                #region Check DocCodeKey if any calculation is to be run
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        grd.ActiveRow.Cells["ExpAmtGST"].Value = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ExpAmtGST"].Value, 0), GVar.RndDecs.Amtpt);//Ask Mic
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                //Validation
                if (GFunc.NEDec(grd.ActiveRow.Cells["ExpAmtGST"].Value, 0) != 0)
                {
                    if (GFunc.GetIntPropertyValue("DocAccKey", objDoc) <= 0)
                    {
                        MsgBox.Show("GL Account can not be empty.");
                        return false;
                    }

                    if (GFunc.GetIntPropertyValue("DocCurrKey", objDoc) <= 0)
                    {
                        MsgBox.Show("Currency can not be empty.");
                        return false;
                    }
                }

                #region set variables
                ExpTaxGrpRate = GFunc.NEDec(grd.ActiveRow.Cells["ExpTaxGrpRate"].Value, 0); //Ask Mic
                DocTaxKey = (int?)(GFunc.GetPropertyValue("DocTaxGrpKey", objDoc)); // Ask Mic
                DocTaxRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);

                DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                if (SysOptionUtility.GetInt("CountryCurrency") == 1) // Country Currency = Home -- Added by Jane. Mic to check
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                else
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);
                ExpAmtGST = GFunc.NEDec(grd.ActiveRow.Cells["ExpAmtGST"].Value, 0);
                #endregion

                #region Standard Calculation
                if (objDoc.DocType == 110)   //if Tax Inclusive,
                {
                    if (ExpTaxGrpRate > 0)
                        if (DocTaxRate == 0)
                            ExpAmtF = ExpAmtGST;
                        else
                            ExpAmtF = GFunc.RndDC(ExpAmtGST, (1 + ExpTaxGrpRate), GVar.RndDecs.Amtpt);
                    else
                        ExpAmtF = ExpAmtGST;
                }
                else
                {
                    ExpAmtF = ExpAmtGST;
                }
                ExpAmtH = GFunc.RndC(ExpAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                #endregion

                #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                if (DocTaxRate == 0 && ExpTaxGrpRate > 0)
                {
                    //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                    ExpTaxGrpKey = DocTaxKey;
                    ExpTaxGrpRate = 0;
                    ExpTaxGrpAmtF = 0;
                    ExpTaxGrpAmtL = 0;
                }
                else
                {
                    if (ExpTaxGrpRate > 0)
                    {
                        ExpTaxGrpAmtF = GFunc.RndC(ExpAmtF * ExpTaxGrpRate, GVar.RndDecs.Prcpt);
                        ExpTaxGrpAmtL = GFunc.RndC(ExpTaxGrpAmtF * DocCountryRate, GVar.RndDecs.Prcpt);
                    }
                    else
                    {
                        ExpTaxGrpAmtF = 0;
                        ExpTaxGrpAmtL = 0;
                    }
                }
                #endregion

                #region Set grid with calculated values
                grd.ActiveRow.Cells["ExpAmtGST"].Value = ExpAmtGST;
                grd.ActiveRow.Cells["ExpAmtF"].Value = ExpAmtF;
                grd.ActiveRow.Cells["ExpAmtH"].Value = ExpAmtH;
                grd.ActiveRow.Cells["ExpTaxGrpAmtF"].Value = ExpTaxGrpAmtF;
                grd.ActiveRow.Cells["ExpTaxGrpAmtL"].Value = ExpTaxGrpAmtL;
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
        private static bool ExpDeptKey_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        grd.ActiveRow.Cells["ExpDeptKey"].Value = GFunc.NEInt(grd.ActiveRow.Cells["ExpDeptKey"].Value, 0);
                        break;

                    default:
                        MsgBox.Show(MsgID.Document.NotAllowedInput + "%Department");
                        grd.ActiveRow.Cells["ExpDeptKey"].Value = 0;
                        return false;
                }
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

        private static bool GNLItmTaxGrpKey_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            string msgID = string.Empty;

            DateTime? DocDate = null;
          
            decimal? ItmCurrRate = 1;
            decimal? CountryRate = 1;

            decimal? ItmCreditF = 0;
            decimal? ItmCreditH = 0;
            decimal? ItmDebitF = 0;
            decimal? ItmDebitH = 0;
            int? ItmTaxGrpKey = 0;
            decimal? ItmTaxGrpRate = 0;
            decimal? ItmTaxGrpAmtF = 0;
            decimal? ItmTaxGrpAmtL = 0;
            
            try
            {
                #region set variables
               
                if (SysOptionUtility.GetInt("CountryCurrency") == 1)
                    CountryRate = GFunc.NEDec(grd.ActiveRow.Cells["ItmCurrRate"].Value, 1);
                else
                    CountryRate = 1;
                DocDate = (DateTime?)GFunc.GetDatePropertyValue("DocDate", objDoc);
                ItmTaxGrpKey = GFunc.NEInt(grd.ActiveRow.Cells["ItmTaxGrpKey"].Value, 0);
                ItmCreditF = GFunc.NEDec(grd.ActiveRow.Cells["ItmCreditF"].Value, 0);
                ItmCreditH = GFunc.NEDec(grd.ActiveRow.Cells["ItmCreditH"].Value, 0);
                ItmDebitF = GFunc.NEDec(grd.ActiveRow.Cells["ItmDebitF"].Value, 0);
                ItmDebitH = GFunc.NEDec(grd.ActiveRow.Cells["ItmDebitH"].Value, 0);
                #endregion

                #region set grid value in preparation to run calculation base on ItmType conditions
                ItmTaxGrpRate = DocComUtility.TaxGrpRate_Get(ItmTaxGrpKey, GFunc.GetDatePropertyValue("DocDate", objDoc));

                if (ItmTaxGrpRate > 0)
                {
                    if (ItmCreditF != 0 && ItmCreditH != 0)
                    {
                        ItmTaxGrpAmtF = GFunc.RndC(ItmCreditF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                        ItmTaxGrpAmtL = GFunc.RndC(ItmTaxGrpAmtF * CountryRate, GVar.RndDecs.Amtpt);
                    }
                    else if (ItmDebitF != 0 && ItmDebitH != 0)
                    {
                        ItmTaxGrpAmtF = GFunc.RndC(ItmDebitF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                        ItmTaxGrpAmtL = GFunc.RndC(ItmTaxGrpAmtF * CountryRate, GVar.RndDecs.Amtpt);
                    }
                }
                else
                {
                    ItmTaxGrpAmtF = 0;
                    ItmTaxGrpAmtL = 0;
                }
              
                #endregion

                #region Set values to grid
                grd.ActiveRow.Cells["ItmTaxGrpKey"].Value = ItmTaxGrpKey.ToDBValue();
                grd.ActiveRow.Cells["ItmTaxGrpRate"].Value = ItmTaxGrpRate;
                grd.ActiveRow.Cells["ItmTaxGrpAmtF"].Value = ItmTaxGrpAmtF;
                grd.ActiveRow.Cells["ItmTaxGrpAmtL"].Value = ItmTaxGrpAmtL;
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

        private static bool ExpTaxGrpKey_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            string msgID = string.Empty;
            int? DocTaxGrpKey = 0;
            decimal? DocTaxGrpRate = 0;

            DateTime? DocDate = null;
            decimal? DocTaxRate = 0;
            decimal? DocCurrRate = 1;
            decimal? DocCountryRate = 1;

            decimal? ExpAmtF = 0;
            decimal? ExpAmtH = 0;
            int? ExpTaxGrpKey = 0;
            decimal? ExpTaxGrpRate = 0;
            decimal? ExpTaxGrpAmtF = 0;
            decimal? ExpTaxGrpAmtL = 0;
            try
            {
                #region Check DocCodeKey if any calculation is to be run
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                #endregion

                #region set variables
                DocTaxGrpKey = (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc);
                DocTaxGrpRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);
                DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                if (SysOptionUtility.GetInt("CountryCurrency") == 1) // Country Currency = Home -- Added by Jane. Mic to check
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                else
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);
                DocDate = (DateTime?)GFunc.GetDatePropertyValue("DocDate", objDoc);
                ExpTaxGrpKey = GFunc.NEInt(grd.ActiveRow.Cells["ExpTaxGrpKey"].Value, 0);
                ExpAmtF = GFunc.NEDec(grd.ActiveRow.Cells["ExpAmtF"].Value, 0); 
                ExpAmtH = GFunc.NEDec(grd.ActiveRow.Cells["ExpAmtH"].Value, 0); 
                #endregion

                #region set grid value in preparation to run calculation base on ItmType conditions
                ExpTaxGrpRate = DocComUtility.TaxGrpRate_Get(ExpTaxGrpKey, GFunc.GetDatePropertyValue("DocDate", objDoc));

                if (DocTaxGrpRate == 0 && ExpTaxGrpRate > 0)
                {
                    MsgBox.Show(MsgID.Document.InvalidItmTaxGrpWhenDocTaxGrpIsZero);
                    return false;
                }

                #region commented by YST on 2023/01/18 to allow to key different ColumnTaxAmount requested by Josie/Susan from Finance Deapartment
                /*
                //when itmtaxrate > 0 the taxgrpkey must be same
                if ((ExpTaxGrpRate > 0 && DocTaxGrpKey != ExpTaxGrpKey) == true)
                {
                    MsgBox.Show(MsgID.Common.MustBeSame + "%Item Tax Group%Doc Tax Group");
                    return false;
                }
                */
                #endregion

                #endregion

                #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                if (DocTaxGrpRate == 0 && ExpTaxGrpRate > 0)
                {
                    //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                    ExpTaxGrpKey = DocTaxGrpKey;
                    ExpTaxGrpRate = 0;
                    ExpTaxGrpAmtF = 0;
                    ExpTaxGrpAmtL = 0;
                }
                else
                {
                    if (ExpTaxGrpRate > 0)
                    {
                        ExpTaxGrpAmtF = GFunc.RndC(ExpAmtF * ExpTaxGrpRate, GVar.RndDecs.Prcpt);
                        ExpTaxGrpAmtL = GFunc.RndC(ExpTaxGrpAmtF * DocCountryRate, GVar.RndDecs.Prcpt);
                    }
                    else
                    {
                        ExpTaxGrpAmtF = 0;
                        ExpTaxGrpAmtL = 0;
                    }
                }
                #endregion

                #region Set values to grid
                grd.ActiveRow.Cells["ExpTaxGrpKey"].Value = ExpTaxGrpKey.ToDBValue();
                grd.ActiveRow.Cells["ExpTaxGrpRate"].Value = ExpTaxGrpRate;
                grd.ActiveRow.Cells["ExpTaxGrpAmtF"].Value = ExpTaxGrpAmtF;
                grd.ActiveRow.Cells["ExpTaxGrpAmtL"].Value = ExpTaxGrpAmtL;
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
        
        public static bool ExpTranGrpID_btnClick(Document objDoc, UltraGrid grd)
        {
            try
            {
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        frmPopupTreeView _frmPopupTreeView = new frmPopupTreeView();
                        _frmPopupTreeView.ShowDialog();

                        if (_frmPopupTreeView.DialogResult == System.Windows.Forms.DialogResult.OK)
                            grd.ActiveRow.Cells["ExpTranGrpKey"].Value = _frmPopupTreeView.TranGrpKey;
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
                return ExpTranGrpKey_CustomUpdate(objDoc, grd);
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
        public static bool ExpTranGrpKey_CustomUpdate(Document objDoc, UltraGrid grd)
        {
            try
            {
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        grd.ActiveRow.Cells["ExpTranGrpKey"].Value = GFunc.NEInt(grd.ActiveRow.Cells["ExpTranGrpKey"].Value, 0);
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
                }
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

        //Functions
        private static int? GetItmInfor_DeptTranGrp(MSTItm objItm, int? CurrentValue)
        {
            try
            {
                if (GFunc.GetINTypeGroup(objItm.ItmType) == (int)GEnum.INTypeGrp.Total)
                    return 0;
                else
                    return GFunc.NEInt(CurrentValue, 0);
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
        private static int? GetItmInfor_Loc(string DocMode, MSTItm objItm, int? CurrentValue)
        {
            try
            {
                if (GFunc.GetINTypeGroup(objItm.ItmType) == (int)GEnum.INTypeGrp.Stock)
                {
                    if (GFunc.CompareString(DocMode, "AP") || GFunc.CompareString(DocMode, "IN"))
                    {
                        if (GFunc.NEInt(objItm.DefLocPurchase, 0) > 0)
                            return objItm.DefLocPurchase;
                    }
                    else
                    {
                        if (GFunc.NEInt(objItm.DefLocSale, 0) > 0)
                            return objItm.DefLocSale;
                    }
                    return CurrentValue;
                }
                return null;
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
        private static int? GetItmInfor_TaxGrpKey(MSTItm objItm, int? DocTaxGrpKey)
        {
            try
            {
                switch (GFunc.GetINTypeGroup(objItm.ItmType))
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                    case (int)GEnum.INTypeGrp.Charges:
                    case (int)GEnum.INTypeGrp.Discount:
                        if ((bool)objItm.Taxable)
                        {
                            return DocTaxGrpKey;
                        }
                        else
                        {
                            return null;
                        }

                    default:
                        return null;
                }
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
        private static decimal? GetItmInfor_TaxGrpRate(MSTItm objItm, decimal? DocTaxRate)
        {
            try
            {
                switch (GFunc.GetINTypeGroup(objItm.ItmType))
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                    case (int)GEnum.INTypeGrp.Charges:
                    case (int)GEnum.INTypeGrp.Discount:
                        if ((bool)objItm.Taxable)
                        {
                            return (decimal?)DocTaxRate;
                        }
                        else
                        {
                            return 0;
                        }

                    default:
                        return 0;
                }
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
        private static int? GetItmInfor_Job(int docCodeKey, MSTItm objItm, int? CurrentValue, int? CurrentJobKey)
        {

            try
            {
                if (objItm.ItmType == (int)GEnum.ItemType.Remark)
                    return GFunc.NEInt(CurrentValue, 0);               

                    switch (GFunc.GetINTypeGroup(objItm.ItmType))
                {
                    case (int)GEnum.INTypeGrp.Stock:
                        switch (docCodeKey)
                        {
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            case (int)GEnum.SystemCode.Purchase_Shipment:
                            case (int)GEnum.SystemCode.Purchase_Delivery:
                            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                            case (int)GEnum.SystemCode.Purchase_Plan:
                            case (int)GEnum.SystemCode.Purchase_Request:
                            case (int)GEnum.SystemCode.Purchase_Order:
                            case (int)GEnum.SystemCode.Purchase_Adjustment:
                            case (int)GEnum.SystemCode.Payment_Issue:
                                return 0;

                            default:
                                if (GFunc.IsNEZ(CurrentJobKey))
                                    return 0;
                                else
                                    return GFunc.NEInt(CurrentValue, 0);

                        }
                    case (int)GEnum.INTypeGrp.Non_Stock:
                    case (int)GEnum.INTypeGrp.Charges:
                    case (int)GEnum.INTypeGrp.Discount:
                        if (GFunc.IsNEZ(CurrentJobKey))
                            return 0;
                        else
                            return GFunc.NEInt(CurrentValue, 0);

                    default:                        
                            return 0;
                }
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
        private static bool Check_RunProcess(int DocCode, UltraGrid grd, ref bool RunCalculateAR, ref bool RunCalculateAP, ref bool RunNullChargeItemQty, ref bool RunCalculateTax)
        {
            int? ItmType = 0;
            try
            {
                ItmType = GFunc.GetINTypeGroup(grd.ActiveRow.Cells["ItmType"].Value);

                #region Set process to run by DocCode and ItmType
                switch (DocCode)
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
                        switch (ItmType)
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                                RunCalculateAR = true;
                                RunCalculateTax = true;
                                break;
                            case (int)GEnum.INTypeGrp.Discount:
                                RunCalculateTax = true;
                                break;
                            case (int)GEnum.INTypeGrp.Charges:
                                RunNullChargeItemQty = true;
                                RunCalculateAR = true;
                                RunCalculateTax = true;
                                break;

                            default:
                                return true;
                        }
                        break;
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        switch (ItmType)
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                                RunCalculateAP = true;
                                RunCalculateTax = true;
                                break;
                            case (int)GEnum.INTypeGrp.Discount:
                                RunCalculateTax = true;
                                break;
                            case (int)GEnum.INTypeGrp.Charges:
                                RunNullChargeItemQty = true;
                                RunCalculateAP = true;
                                RunCalculateTax = true;
                                break;
                            default:
                                return true;
                        }
                        break;
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        switch (ItmType)
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:
                                RunCalculateAP = true;
                                break;

                            case (int)GEnum.INTypeGrp.Charges:
                                RunNullChargeItemQty = true;
                                RunCalculateAP = true;
                                break;

                            default:
                                return true;
                        }
                        break;

                    default:
                        MsgBox.Show("Unable to match document code");
                        return false;
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
        private static bool Check_DuplicateItm(Document objDoc, UltraGrid grd, int key, int FGkey)
        {
            //This function is only use for INMFN detail validation
            try
            {
                int DocItmKey = GFunc.NEInt(grd.ActiveRow.Cells["DocItmKey"].Value, 0);
                switch (GFunc.GetIntPropertyValue("DocProDetails", objDoc))
                {
                    case (int)GEnum.InventoryDetails.FinishedGoods:
                        foreach (UltraGridRow dr in grd.Rows)
                        {
                            if (GFunc.NEInt(dr.Cells["DocItmKey"].Value, 0) != DocItmKey && GFunc.NEInt(dr.Cells["LineType"].Value, 0) == 3000 && GFunc.NEInt(dr.Cells["ItmKey"].Value, 0) == key)
                            {
                                MsgBox.Show("You cannot have Duplicate Item ID in the finished goods list");
                                return false;
                            }
                        }
                        break;

                    case (int)GEnum.InventoryDetails.PackingMaterial:
                        foreach (UltraGridRow dr in grd.Rows)
                        {
                            if (GFunc.NEInt(dr.Cells["DocItmKey"].Value, 0) != DocItmKey && GFunc.NEInt(dr.Cells["LineType"].Value, 0) == 3000 && GFunc.NEInt(dr.Cells["ItmKey"].Value, 0) == key)
                            {
                                MsgBox.Show("You cannot have Item ID in the packing material list which also in the finished good list");
                                return false;
                            }

                            if (GFunc.NEInt(dr.Cells["DocItmKey"].Value, 0) != DocItmKey && GFunc.NEInt(dr.Cells["LineType"].Value, 0) == 3200 && GFunc.NEInt(dr.Cells["ItmKey"].Value, 0) == key
                                && GFunc.NEInt(dr.Cells["ItmFGKey"].Value, 0) == FGkey)
                            {
                                MsgBox.Show("You cannot have Duplicate (Item ID + Finished Goods Item ID) in the packing material list");
                                return false;
                            }
                        }
                        break;

                    case (int)GEnum.InventoryDetails.RawMaterial:
                        foreach (UltraGridRow dr in grd.Rows)
                        {
                            if (GFunc.NEInt(dr.Cells["DocItmKey"].Value, 0) != DocItmKey && GFunc.NEInt(dr.Cells["LineType"].Value, 0) == 3000 && GFunc.NEInt(dr.Cells["ItmKey"].Value, 0) == key)
                            {
                                MsgBox.Show("You cannot have Item ID in the raw material list which also in the finished good list");
                                return false;
                            }
                            
                            if (GFunc.NEInt(dr.Cells["LineType"].Value, 0) == 3100 && GFunc.NEInt(dr.Cells["ItmKey"].Value, 0) == key)
                            {
                                MsgBox.Show("You cannot have Duplicate Item ID in the raw material list");
                                return false;
                            }
                        }
                        break;
                }

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
        
        //Set Error Methods
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

        //Mic Check ; Jack Added 3 Dec 2012
        public static void DetItm_OrderByMarking(DataTable dtItems, UltraGrid grd)
        {
            try
            {
                //if all the Itm mark is empty, skip marking sorting
                

                //Temporarily Remove Filter from DataTable
                string rowFilter = dtItems.DefaultView.RowFilter;
                dtItems.DefaultView.RowFilter = "";
                
                //if there are rows that ItmMark is blank, replace with previous one plus 0.1 or add zzzz                
                DataTable dtMarking=dtItems.DefaultView.ToTable(); //Because; Only the default view is ordered by Serial No; So we have had to assign Itm Mark based on Default View
                dtItems.PrimaryKey = new DataColumn[] { dtItems.Columns["DocItmKey"] };
                DataRow previousRowNotHeader=null;// We must retain previous row; when the following row is blank(except header), we'll replace with it
                for (int i = 0; i < dtMarking.Rows.Count; i++)
                {                    
                    DataRow dr = dtItems.Rows.Find(dtMarking.Rows[i]["DocItmKey"]);
                    //Header Item Type is skipped
                    if (GFunc.NEInt(dr["ItmType"], 0) != 800)
                    {
                        if (GFunc.IsNE(dr["ItmMark"]) == true)
                        {
                            if (i > 0)
                                dr["ItmMark"] = previousRowNotHeader["ItmMark"] + "zzzzzN";
                            else
                                dr["ItmMark"] = "00000";
                        }
                        previousRowNotHeader = dr;
                    }
                }
                
                //------------------For Header Item Type [ Inser Custom Marking (Logic is following row's last char ascii value -1] ---
                
                List<string> customMarkingList=new List<string>();
                DataRow RowFollowedByHeader = null;// We must retain previous row; when the following row is blank(except header), we'll replace with it
                for (int i = dtMarking.Rows.Count-1; i >-1 ; i--)
                {
                    DataRow dr = dtItems.Rows.Find(dtMarking.Rows[i]["DocItmKey"]);
                    //Header Item Type is skipped
                    if (GFunc.NEInt(dr["ItmType"], 0) == 800)
                    {
                        if (GFunc.IsNE(dr["ItmMark"]) == true)
                        {
                            if (i == dtMarking.Rows.Count - 1)
                                dr["ItmMark"] = "zzzzz";
                            else
                            {
                                string FollowingRowItmMark = RowFollowedByHeader["ItmMark"].ToString();
                                char lastChar = FollowingRowItmMark[FollowingRowItmMark.Length - 1];
                                char charLessthanLastChar = (char)(Encoding.ASCII.GetBytes(new char[]{lastChar})[0]-1);
                                dr["ItmMark"] = FollowingRowItmMark.Remove(FollowingRowItmMark.Length-1)+charLessthanLastChar;
                                customMarkingList.Add(dr["ItmMark"].ToString());
                            }
                                
                        }                        
                    }
                    RowFollowedByHeader = dr;
                }
                dtItems.PrimaryKey = null;



                string[] markingList = new string[dtItems.Rows.Count];
                for (int i = 0; i < dtItems.Rows.Count; i++)
                {
                    markingList[i] = dtItems.Rows[i]["ItmMark"].ToString();
                }
                StringLogicalComparer nc = new StringLogicalComparer();
                Array.Sort(markingList, nc);

                //Set All Itm SN to Zero
                for (int i = 0; i < dtItems.Rows.Count; i++)
                {
                    dtItems.Rows[i]["ItmSN"] = 0;
                }

                int j = 1;
                for (int i = 0; i < markingList.Length; i++)
                {
                    foreach (DataRow row in dtItems.Rows)
                    {
                        if (row["ItmMark"].ToString() == markingList[i].ToString() && row["ItmSN"].ToString()=="0")
                        {
                            //For header record, update SN
                            if (row["LineLinkKey"].ToString() == "0")
                                row["ItmSN"] = j;

                            //For Above header record, check for any child item and update SN
                            for (int k = 0; k < dtItems.Rows.Count; k++)
                            {
                                if (dtItems.Rows[k]["LineLinkKey"] == row["DocItmKey"])
                                    row["ItmSN"] = j;
                            }
                            j++;
                            break;
                        }
                    }
                }

                //Remove zzzzz and 00000 from ItmMark
                for (int i = 0; i < dtItems.Rows.Count; i++)
                {
                    if (dtItems.Rows[i]["ItmMark"].ToString().Contains("zzzzz") || dtItems.Rows[i]["ItmMark"].ToString().Contains("00000"))
                    {
                        dtItems.Rows[i]["ItmMark"] = "";
                    }
                }

                //Remove Custom ItmMark for Header
                for (int i = 0; i < customMarkingList.Count; i++)
                {
                    for (int k = 0; k < dtItems.Rows.Count; k++)
                    {
                        if (dtItems.Rows[k]["ItmMark"].ToString().Contains(customMarkingList[i]))
                        {
                            dtItems.Rows[k]["ItmMark"] = "";
                        }
                    }
                    
                }

                dtItems.DefaultView.Sort = "ItmSN";
                

                //Restore row filter
                dtItems.DefaultView.RowFilter = rowFilter;
                grd.DataSource = dtItems;
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

        public static void UpdateItmAccFromMaster(Document objDoc, UltraGrid grd)
        {
            MSTItm objItm = null;
            string DocMode = string.Empty;              //AR,AP,IN
            int? ItmTypeGrp = 0;
            int? ItmKey = 0;
            int? ItmKeySelected = 0;
            int? ItmType = 0;
            string ItmID = string.Empty;
            int? ItmAccKey = 0;

            foreach (UltraGridRow grdRow in grd.Rows)
            {
                #region Get MSTItm Object
                ItmKey = GFunc.NEInt(grdRow.Cells["ItmKey"].Value, 0);
                objItm = MSTItm.Get(ItmKey);
                if (GFunc.NEInt(objItm.SubstituteItmKey, 0) > 0)
                {
                    ItmKey = (int)objItm.SubstituteItmKey;
                    ItmKeySelected = (int)objItm.ItmKey;
                    objItm = MSTItm.Get(ItmKey);
                }
                else
                {
                    ItmKey = (int)objItm.ItmKey;
                    ItmKeySelected = ItmKey;
                }

                #endregion

                #region set process to run base on DocCode
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
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        DocMode = "AR";
                        break;

                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        DocMode = "AP";
                        break;


                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                        DocMode = "IN";

                        break;

                    default:
                        DocMode = "";
                        break;
                }
                #endregion
                ItmType = objItm.ItmType;
                ItmTypeGrp = GFunc.GetINTypeGroup(ItmType);
                #region set ItmAccKey
                switch (ItmTypeGrp)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                        switch (DocMode.ToLower())
                        {
                            case "ar":
                                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Issue_Consignment || objDoc.DocCodeKey == (int)GEnum.SystemCode.Return_Consignment)
                                {
                                    ItmAccKey = (objDoc.DefAccKey > 0) ? objDoc.DefAccKey : objItm.AccINKey;
                                }
                                else
                                {
                                    ItmAccKey = (objDoc.DefAccKey > 0) ? objDoc.DefAccKey : objItm.AccICKey;
                                }
                                break;

                            case "ap":
                                if (ItmType == (int)GEnum.ItemType.Consignment)
                                {
                                    ItmAccKey = objItm.AccPHKey;
                                }
                                else
                                {
                                    if (DocComUtility.IsItmCostingContinuous())
                                        ItmAccKey = objItm.AccINKey;
                                    else
                                        ItmAccKey = objItm.AccPHKey;
                                }

                                break;

                            default:
                                //IN Document
                                ItmAccKey = objItm.AccINKey;
                                break;
                        }
                        break;

                    case (int)GEnum.INTypeGrp.Non_Stock:
                        if (SysOptionUtility.DatabaseBranchCode == DBCode.ADL)
                        {
                            ItmAccKey = GFunc.NEInt(grdRow.Cells["ItmAccKey"].Value, 0);
                        }
                        else
                        {
                            if (GFunc.CompareString(DocMode, "AR"))
                                ItmAccKey = (objDoc.DefAccKey > 0) ? objDoc.DefAccKey : objItm.AccICKey;
                            else
                                ItmAccKey = (objDoc.DefAccKey > 0) ? objDoc.DefAccKey : objItm.AccPHKey;
                        }
                        break;
                    case (int)GEnum.INTypeGrp.Charges:
                    case (int)GEnum.INTypeGrp.Discount:
                        if (GFunc.CompareString(DocMode, "AR"))
                            ItmAccKey = (objDoc.DefAccKey > 0) ? objDoc.DefAccKey : objItm.AccICKey;
                        else
                            ItmAccKey = (objDoc.DefAccKey > 0) ? objDoc.DefAccKey : objItm.AccPHKey;
                        break;

                    default:
                        ItmAccKey = null;
                        break;
                }
                #endregion

                #region set values in document detail
                grdRow.Cells["ItmAccKey"].Value = ItmAccKey.ToDBValue(); //Ask Mic //Ask Mic

                MSTAcc objMSTAcc = MSTAcc.Get(ItmAccKey);
                if (!GFunc.IsNEZ(objMSTAcc.AccKey))
                {
                    grdRow.Cells["ItmAccID"].Value = objMSTAcc.AccID;
                    grdRow.Cells["ItmAccDes"].Value = objMSTAcc.AccDes;
                }

                #endregion

            }

            grd.UpdateData();
        }
    }

}
