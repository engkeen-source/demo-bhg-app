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
using Infragistics.Win.Misc;
using System.IO;
using System.Text.RegularExpressions;
using TAUtil;

namespace BOLib
{
    public class DocComUtility
    {
        public GVar.RecordSelectedEvent RecordSelectedEvent = null;
        public GVar.PopupSelectedEvent PopupSelectedEvent = null;
        public static string AppRunningState = "";
      
        /// <summary>
        /// Called from frmPopupAttachment Saving
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="attachs"></param>
        /// <param name="attachOlds"></param>
        /// <returns></returns>
        public static bool ItmAttachment_Set(DataTable dt, ref SYSAttachments attachOlds, int? docItmKey)
        {

            try
            {
                if (attachOlds == null)
                {
                    attachOlds = new SYSAttachments();
                }

                //SYSAttachment.Criteria criteria = new SYSAttachment.Criteria();
                //criteria._option = 0;
                //criteria._docDK = (int)dt.Rows[0]["DocDK"];
                //criteria._docDC = (int)dt.Rows[0]["DocDC"];
                //criteria._docDetailType = (int)dt.Rows[0]["DocDetailType"];
                //criteria._docDItm = (int)dt.Rows[0]["DocDItm"];
                //criteria._seq = 0;

                int count = attachOlds.Count;
                //int i = 0;
                //while (i<count)
                //{
                //    if (attachOlds[i].DocDItm == docItmKey)
                //        attachOlds.RemoveAt(i);
                //    i++;
                //}
                //Remove All first                                
                for (int i = 0; i < count; i++)
                {
                    if (attachOlds[0].DocDItm == docItmKey)
                        attachOlds.RemoveAt(0);
                }


                //Add all in the DataTable
                foreach (DataRow dr in dt.Rows)
                {
                    SYSAttachment attach = SYSAttachment.NewChild();

                    attach._docDC = GFunc.NEInt(dr["DocDC"], 0);
                    attach._docDetailType = GFunc.NEInt(dr["DocDetailType"], 0);
                    attach._docDItm = GFunc.NEInt(dr["DocDItm"], 0);
                    attach._docDK = GFunc.NEInt(dr["DocDK"], 0);
                    attach._attachSize = GFunc.NEInt(dr["AttachSize"], 0);     //File Size
                    attach._attachFileType = dr["AttachFileType"].ToString(); //File Extension
                    attach._attachDes = dr["AttachDes"].ToString();         //File Name
                    attach._attachPath = dr["AttachPath"].ToString(); //File Path   
                    attach._custom1 = dr["Custom1"].ToString();
                    attach._seq = GFunc.NEInt(dr["Seq"], 0);

                    attachOlds.Add(attach);
                }
                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {

            }
        }
        public static bool Address_Set(Document objDoc, int AddrLinkType, int AddrLinkKey, string addrID, bool SetBillAddr, bool SetShipAddr)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return Address_Set(cn, objDoc, AddrLinkType, AddrLinkKey, addrID, SetBillAddr, SetShipAddr);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public static bool Address_Set(Document objDoc, int? addrKey, bool SetBillAddr, bool SetShipAddr)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return Address_Set(cn, objDoc, addrKey, SetBillAddr, SetShipAddr);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public static bool Address_Set(SqlConnection cn, Document objDoc, int AddrLinkType, int AddrLinkKey, string addrID, bool SetBillAddr, bool SetShipAddr)
        {
            #region declaration
            REFAddr objAddr = null;
            string DefAddrKey = string.Empty;
            string DocAddrStreet = string.Empty;
            string DocAddrPOBox = string.Empty;
            string DocAddrCity = string.Empty;
            string DocAddrState = string.Empty;
            string DocAddrZipCode = string.Empty;
            string DocAddrCountry = string.Empty;
            string DocAddrRegion = string.Empty;
            string DocAddrAttn = string.Empty;
            string DocAddrTel1 = string.Empty;
            string DocAddrTel2 = string.Empty;
            string DocAddrFax = string.Empty;
            string DocAddrEmail = string.Empty;
            int DocAddrShipViaKey = 0;
            #endregion

            try
            {
                if (addrID != string.Empty)
                {
                    objAddr = REFAddr.Get(cn,AddrLinkType, AddrLinkKey, addrID);
                    if (objAddr.AddrKey != null)
                    {
                        #region get Addr infor
                        DefAddrKey = objAddr._addrKey.ToString();
                        DocAddrStreet = objAddr.AddrStreet;
                        DocAddrPOBox = objAddr.AddrPOBox;
                        DocAddrCity = objAddr.AddrCity;
                        DocAddrState = objAddr.AddrState;
                        DocAddrZipCode = objAddr.AddrZipCode;
                        DocAddrCountry = objAddr.AddrCountry;
                        DocAddrRegion = objAddr.AddrRegion;
                        DocAddrAttn = objAddr.AddrAttn;
                        DocAddrTel1 = objAddr.AddrTel1;
                        DocAddrTel2 = objAddr.AddrTel2;
                        DocAddrFax = objAddr.AddrFax;
                        DocAddrEmail = objAddr.AddrEmail;
                        DocAddrShipViaKey = GFunc.NEInt(objAddr.AddrShipViaKey, 0);
                        #endregion
                    }
                }

                if (SetBillAddr)
                {
                    #region set bill address
                    GFunc.SetPropertyValue("DefBAddrKey", objDoc, DefAddrKey);
                    GFunc.SetPropertyValue("DocBAddrStreet", objDoc, DocAddrStreet);
                    GFunc.SetPropertyValue("DocBAddrPOBox", objDoc, DocAddrPOBox);
                    GFunc.SetPropertyValue("DocBAddrCity", objDoc, DocAddrCity);
                    GFunc.SetPropertyValue("DocBAddrState", objDoc, DocAddrState);
                    GFunc.SetPropertyValue("DocBAddrZipCode", objDoc, DocAddrZipCode);
                    GFunc.SetPropertyValue("DocBAddrCountry", objDoc, DocAddrCountry);
                    GFunc.SetPropertyValue("DocBAddrRegion", objDoc, DocAddrRegion);
                    GFunc.SetPropertyValue("DocBAddrAttn", objDoc, DocAddrAttn);
                    GFunc.SetPropertyValue("DocBAddrTel1", objDoc, DocAddrTel1);
                    GFunc.SetPropertyValue("DocBAddrTel2", objDoc, DocAddrTel2);
                    GFunc.SetPropertyValue("DocBAddrFax", objDoc, DocAddrFax);
                    GFunc.SetPropertyValue("DocBAddrEmail", objDoc, DocAddrEmail);
                    #endregion
                }
                if (SetShipAddr)
                {
                    #region set shipping address
                    GFunc.SetPropertyValue("DefSAddrKey", objDoc, DefAddrKey);
                    GFunc.SetPropertyValue("DocSAddrStreet", objDoc, DocAddrStreet);
                    GFunc.SetPropertyValue("DocSAddrPOBox", objDoc, DocAddrPOBox);
                    GFunc.SetPropertyValue("DocSAddrCity", objDoc, DocAddrCity);
                    GFunc.SetPropertyValue("DocSAddrState", objDoc, DocAddrState);
                    GFunc.SetPropertyValue("DocSAddrZipCode", objDoc, DocAddrZipCode);
                    GFunc.SetPropertyValue("DocSAddrCountry", objDoc, DocAddrCountry);
                    GFunc.SetPropertyValue("DocSAddrRegion", objDoc, DocAddrRegion);
                    GFunc.SetPropertyValue("DocSAddrAttn", objDoc, DocAddrAttn);
                    GFunc.SetPropertyValue("DocSAddrTel1", objDoc, DocAddrTel1);
                    GFunc.SetPropertyValue("DocSAddrTel2", objDoc, DocAddrTel2);
                    GFunc.SetPropertyValue("DocSAddrFax", objDoc, DocAddrFax);
                    GFunc.SetPropertyValue("DocSAddrEmail", objDoc, DocAddrEmail);
                    #endregion
                }

                #region set Shipping mode
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Packing_List:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                        GFunc.SetPropertyValue("DocShipKey", objDoc, DocAddrShipViaKey);
                        break;
                }
                #endregion

                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                objAddr = null;
            }
        }//Completed
        public static bool Address_Set(SqlConnection cn, Document objDoc, int? addrKey, bool SetBillAddr, bool SetShipAddr)
        {
            #region Declaration
            REFAddr objAddr;
            string DefAddrKey = string.Empty;
            string DocAddrStreet = string.Empty;
            string DocAddrPOBox = string.Empty;
            string DocAddrCity = string.Empty;
            string DocAddrState = string.Empty;
            string DocAddrZipCode = string.Empty;
            string DocAddrCountry = string.Empty;
            string DocAddrRegion = string.Empty;
            string DocAddrAttn = string.Empty;
            string DocAddrTel1 = string.Empty;
            string DocAddrTel2 = string.Empty;
            string DocAddrFax = string.Empty;
            string DocAddrEmail = string.Empty;
            int DocAddrShipViaKey = 0;
            #endregion

            try
            {
                if (GFunc.IsNEZ(addrKey) == false)
                {
                    objAddr = REFAddr.Get(cn, addrKey);
                    if (objAddr.AddrKey != null)
                    {
                        #region get Addr infor
                        DefAddrKey = objAddr._addrKey.ToString();
                        DocAddrStreet = objAddr.AddrStreet;
                        DocAddrPOBox = objAddr.AddrPOBox;
                        DocAddrCity = objAddr.AddrCity;
                        DocAddrState = objAddr.AddrState;
                        DocAddrZipCode = objAddr.AddrZipCode;
                        DocAddrCountry = objAddr.AddrCountry;
                        DocAddrRegion = objAddr.AddrRegion;
                        DocAddrAttn = objAddr.AddrAttn;
                        DocAddrTel1 = objAddr.AddrTel1;
                        DocAddrTel2 = objAddr.AddrTel2;
                        DocAddrFax = objAddr.AddrFax;
                        DocAddrEmail = objAddr.AddrEmail;
                        DocAddrShipViaKey = GFunc.NEInt(objAddr.AddrShipViaKey, 0);
                        #endregion
                    }
                }
                if (SetBillAddr)
                {
                    #region set billing address
                    GFunc.SetPropertyValue("DefBAddrKey", objDoc, DefAddrKey);
                    GFunc.SetPropertyValue("DocBAddrStreet", objDoc, DocAddrStreet);
                    GFunc.SetPropertyValue("DocBAddrPOBox", objDoc, DocAddrPOBox);
                    GFunc.SetPropertyValue("DocBAddrCity", objDoc, DocAddrCity);
                    GFunc.SetPropertyValue("DocBAddrState", objDoc, DocAddrState);
                    GFunc.SetPropertyValue("DocBAddrZipCode", objDoc, DocAddrZipCode);
                    GFunc.SetPropertyValue("DocBAddrCountry", objDoc, DocAddrCountry);
                    GFunc.SetPropertyValue("DocBAddrRegion", objDoc, DocAddrRegion);
                    GFunc.SetPropertyValue("DocBAddrAttn", objDoc, DocAddrAttn);
                    GFunc.SetPropertyValue("DocBAddrTel1", objDoc, DocAddrTel1);
                    GFunc.SetPropertyValue("DocBAddrTel2", objDoc, DocAddrTel2);
                    GFunc.SetPropertyValue("DocBAddrFax", objDoc, DocAddrFax);
                    GFunc.SetPropertyValue("DocBAddrEmail", objDoc, DocAddrEmail);
                    #endregion
                }
                if (SetShipAddr)
                {
                    #region set shipping address
                    GFunc.SetPropertyValue("DefSAddrKey", objDoc, DefAddrKey);
                    GFunc.SetPropertyValue("DocSAddrStreet", objDoc, DocAddrStreet);
                    GFunc.SetPropertyValue("DocSAddrPOBox", objDoc, DocAddrPOBox);
                    GFunc.SetPropertyValue("DocSAddrCity", objDoc, DocAddrCity);
                    GFunc.SetPropertyValue("DocSAddrState", objDoc, DocAddrState);
                    GFunc.SetPropertyValue("DocSAddrZipCode", objDoc, DocAddrZipCode);
                    GFunc.SetPropertyValue("DocSAddrCountry", objDoc, DocAddrCountry);
                    GFunc.SetPropertyValue("DocSAddrRegion", objDoc, DocAddrRegion);
                    GFunc.SetPropertyValue("DocSAddrAttn", objDoc, DocAddrAttn);
                    GFunc.SetPropertyValue("DocSAddrTel1", objDoc, DocAddrTel1);
                    GFunc.SetPropertyValue("DocSAddrTel2", objDoc, DocAddrTel2);
                    GFunc.SetPropertyValue("DocSAddrFax", objDoc, DocAddrFax);
                    GFunc.SetPropertyValue("DocSAddrEmail", objDoc, DocAddrEmail);
                    #endregion
                }

                #region set shipping mode
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Packing_List:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                        GFunc.SetPropertyValue("DocShipKey", objDoc, DocAddrShipViaKey);
                        break;
                }
                #endregion

                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        private static bool ApplyIV_Reset(Document objDoc)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return ApplyIV_Reset(cn, objDoc);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        private static bool ApplyIV_Reset(SqlConnection cn, Document objDoc)
        {
            GEnum.SystemCode codeKey = 0;
            int? docKey = 0;
            int? GUID = objDoc.GUID;
            int? dataKey = 0;
            int? InprogressKey = 0;
            int? SvrDocApplyIVDK = 0;


            try
            {
                codeKey = (GEnum.SystemCode)GFunc.GetIntPropertyValue("DocApplyIVDC", objDoc);
                docKey = GFunc.GetIntPropertyValue("DocKey", objDoc);
                dataKey = GFunc.GetIntPropertyValue("DocApplyIVDK", objDoc);//get DocApplyIVDK(Local)


                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:

                        DataTable dtSvrData = SvrData_Get(cn, (int?)codeKey, docKey);
                        if (!GFunc.IsNE(dtSvrData))
                        {
                            if (dtSvrData.Rows.Count > 0)
                            {
                                SvrDocApplyIVDK = GFunc.NEInt(dtSvrData.Rows[0]["DocApplyIVDK"], 0);
                            }
                        }

                        if (dataKey != SvrDocApplyIVDK)
                        {
                            if (!SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByCodeKeyandDataKey, codeKey, GUID, dataKey, InprogressKey))
                                return false;
                        }

                        GFunc.SetPropertyValue("DocApplyIVDC", objDoc, 0);
                        GFunc.SetPropertyValue("DocApplyIVDK", objDoc, 0);
                        GFunc.SetPropertyValue("DocApplyIVID", objDoc, null);
                        GFunc.SetPropertyValue("DocApplyGainAccKey", objDoc, null);
                        GFunc.SetPropertyValue("DocApplyGainAmt", objDoc, 0M);
                        GFunc.SetPropertyValue("DocApplyAmtF", objDoc, 0M);
                        GFunc.SetPropertyValue("DocApplyAmtH", objDoc, 0M);
                        break;
                }
                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public static bool Alert_Set()
        {
            try
            {
                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }       
      
        /// <summary>
        /// To Calculate Detail Grid Rows for Doc Header  
        /// Called from Detail row update Event and Header Update
        /// </summary>
        /// <param name="objDoc">Document object</param>
        /// <param name="details">Detail Grids</param>
        /// <param name="CalTax">flag for Tax Calculation</param>
        /// <param name="RunCheck">flag for Calculation or Value Checking for Grid</param>
        /// <returns></returns>
        public static bool CalForm(Document objDoc, bool RunCheck)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return CalForm(cn, objDoc, RunCheck);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed

        public static bool CalForm(Document objDoc, Hashtable details, bool CalTax, bool RunCheck)
        {
            try
            {
                
                //foreach (Form f in Application.OpenForms)
                for (int i = Application.OpenForms.Count - 1; i >= 0; i--) //Popup forms are always the last index
                {
                    string popupformName = Application.OpenForms[i].Name.ToLower();
                    if (popupformName == "frmdoccopy" || popupformName == "frmspecialcalculation" 
                        || popupformName == "frminsertdata" || popupformName == "frminsertso" || popupformName == "frminsertpo"
                        || popupformName == "frminsertpd" || popupformName == "frminsertcpo" || popupformName == "frminsertcsi"
                        )
                        return true;
                }
              
               
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return CalForm(cn, objDoc, details, CalTax, RunCheck);
                }
                //else
                //    return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public static bool CalForm(SqlConnection cn, Document objDoc, bool RunCheck)
        {
            try
            {
                Hashtable htDetailGrd = new Hashtable();
                return CalForm(cn, objDoc, htDetailGrd, false, RunCheck);
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public static bool CalForm(SqlConnection cn, Document objDoc, Hashtable details, bool CalTax, bool RunCheck)
        {
            //CalTax if true -  recalculate all line tax value It is only set to false when the ItmTaxAmtF/L is change as 
            //                  we do not want do recalculate in that situation
            DataTable dtItem = null;
            DataTable dtExp = null;
            DataTable dtPack = null;
            
            try
            {
                //foreach (Form f in Application.OpenForms)
                for (int i = Application.OpenForms.Count - 1; i >= 0; i--) //Popup forms are always the last index
                {
                    string popupformName = Application.OpenForms[i].Name.ToLower();
                    if (popupformName == "frmdoccopy" || popupformName == "frmspecialcalculation" 
                        || popupformName == "frminsertdata" || popupformName == "frminsertso" || popupformName == "frminsertpo"
                        || popupformName == "frmInsertpd" || popupformName == "frminsertcpo" || popupformName == "frminsertcsi"
                        )
                        return true;
                }

                
                #region check Detail DataTable
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:

                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref dtItem);
                        break;
                    case (int)GEnum.SystemCode.Packing_List:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref dtItem);
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Pack, details, ref dtPack);
                        break;
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref dtItem);
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, details, ref dtExp);
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
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                        return CalDocAR(cn, objDoc, dtItem, CalTax, RunCheck);

                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                        if (CalDocAR(cn, objDoc, dtItem, CalTax, RunCheck))
                            if (RunCheck == false)
                                return ApplyIV_Reset(objDoc);
                            else
                                return true;
                        break;

                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        return CalDocAP(cn, objDoc, dtItem, CalTax, RunCheck);

                    case (int)GEnum.SystemCode.Purchase_Delivery:
                        return CalDocAPPD(cn, objDoc, dtItem, CalTax, RunCheck);

                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        if (CalDocAP(cn, objDoc, dtItem, CalTax, RunCheck))
                            if (RunCheck == false)
                                return ApplyIV_Reset(objDoc);
                            else
                                return true;
                        break;
                    case (int)GEnum.SystemCode.Purchase_Request:
                        return CalDocAPRQ(cn, objDoc, dtItem, RunCheck);                       

                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                        return CalDocCT(cn, objDoc, dtItem, RunCheck);

                    case (int)GEnum.SystemCode.Sales_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Adjustment:
                        return CalDocAJ(cn, objDoc, RunCheck);

                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                        return CalDocCPO(cn, objDoc, dtItem, RunCheck);

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        return CalDocCSI(cn, objDoc, dtExp, dtItem, RunCheck);

                    case (int)GEnum.SystemCode.Consignment_Settlement:
                        return CalDocCPS(cn, objDoc, dtItem, RunCheck);

                    case (int)GEnum.SystemCode.Deposit:
                        return CalDocDP(cn, objDoc, dtItem, RunCheck);

                    case (int)GEnum.SystemCode.Journal:
                        return CalDocJN(cn, objDoc, dtItem, RunCheck);

                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        return CalDocPY(cn, objDoc, dtExp, dtItem, CalTax, RunCheck);

                    case (int)GEnum.SystemCode.Packing_List:
                        return CalDocPL(cn, objDoc, dtPack, dtItem, RunCheck);

                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                        return CalDocIN(objDoc, dtItem);

                    default:
                        //No Calculation required for other doccode
                        return true;
                }
                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                dtItem = null;
                dtExp = null;
                dtPack = null;
            }
        }//Completed
        private static bool CalDocAJ(SqlConnection cn, Document objDoc, bool RunCheck)
        {
            decimal? CurrRate = 1;
            decimal? DocGrand = 0;
            decimal? DocHome = 0;

            try
            {
                CurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 0);
                DocGrand = GFunc.NEDec(GFunc.GetPropertyValue("DocGrand", objDoc), 0);
                DocHome = GFunc.RndC(DocGrand * CurrRate, GVar.RndDecs.Amtpt);

                if (RunCheck)
                {
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocHome", objDoc), 0) != DocHome)
                    {
                        MsgBox.Show(cn, MsgID.Document.SaveFailTotalCalculationWrong);
                        return false;
                    }
                    else
                        GFunc.SetPropertyValue("DocHome", objDoc, DocHome);
                }
                else
                {
                    GFunc.SetPropertyValue("DocHome", objDoc, DocHome);
                }
                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        private static bool CalDocAR(SqlConnection cn, Document objDoc, DataTable dtItems, bool CalTax, bool RunCheck)
        {
            #region variable
            IEnumerable<DataRow> dtItemFilter = null;
            IEnumerable<DataRow> dtParentFilter = null;
            bool FailCheck = false;
            //bool FailCheckPriceUser = false; for testing only to remove later added by jane
            decimal? PreItmAmt = 0;
            decimal? CurItmAmt = 0;
            decimal? TotalPrevious = 0; //Previous Total line value - use for calculation of Total Line
            decimal? TotalST = 0;       //Sub Total
            decimal? TotalCF = 0;       //CF Total
            decimal? TotalTaxAmtF = 0;  //Total of ItmTaxGrpAmtF
            decimal? TotalTaxAmtL = 0;  //Total of ItmTaxGrpAmtL
            decimal? TotalTaxableAmtF = 0;  //Total Taxable AmountF
            decimal? DetTotalAmtF = 0;  //Total of all detail items w/o gst

            bool ResetTotal = false;

            DateTime DocDate;
            int? DocConKey = 0;
            int? DocCurrKey = 1;
            decimal? DocCurrRate = 1;
            decimal? DocCountryRate = 1;
            decimal? DocDisRate = 0;
            decimal? DocTotalAfterDis = 0;
            decimal? DocOverallDisAmt = 0;
            int? DocTaxKey = 0;
            decimal? DocTaxRate = 0;
            decimal? DocTaxAmt = 0;
            decimal? DocTaxAmtL = 0;

            decimal? ItmQty = 0;
            decimal? ItmPriceAfter = 0;
            decimal? ItmDisRate = 0;
            decimal? ItmDisValue = 0;
            decimal? ItmDisPercent = 0;
            decimal? ItmPriceUser = 0;
            decimal? ItmPrice = 0;
            decimal? ItmAmtShw = 0;
            decimal? ItmAmtF = 0;
            decimal? ItmAmtH = 0;
            int? ItmTaxGrpKey = 0;
            decimal? ItmTaxGrpRate = 0;
            decimal? ItmTaxGrpAmtF = 0;
            decimal? ItmTaxGrpAmtL = 0;
            bool ItmTaxable = false;

            int PriceDec = 0;
            int PriceRoundMode = 0;
            

            string OpValue = string.Empty;
            decimal? SN = 0;
            #endregion

            try
            {
                #region Assign Variables                

                dtItemFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == 0).OrderBy(r => r.Field<decimal>("ItmSN"));

                DocDate = (DateTime)objDoc.DocDate;
                DocConKey = GFunc.NEInt((int?)GFunc.GetPropertyValue("DocConKey", objDoc), 0);
                DocCurrKey = GFunc.NEInt((int?)GFunc.GetPropertyValue("DocCurrKey", objDoc), 0);
                DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                DocTaxKey = GFunc.NEInt(GFunc.GetPropertyValue("DocTaxGrpKey", objDoc), 0);
                DocTaxRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);

                DocDisRate = GFunc.NEDec(GFunc.GetPropertyValue("DocOverallDisRate", objDoc), 0);
                if (SysOptionUtility.GetInt("CountryCurrency", cn) == 1) // Country Currency = Home -- Added by Jane. Mic to check
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                else
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);
                #endregion

                if (dtItems.Rows.Count > 0)
                {
                    #region  looping to all detail items to calculate each row
                    foreach (DataRow row in dtItemFilter)
                    {
                        #region reset variables
                        ItmQty = 0;
                        ItmPriceAfter = 0;
                        ItmDisRate = 0;
                        ItmDisValue = 0;
                        ItmPriceUser = 0;
                        ItmPrice = 0;
                        ItmAmtShw = 0;
                        ItmAmtF = 0;
                        ItmAmtH = 0;
                        ItmTaxGrpKey = 0;
                        ItmTaxGrpRate = 0;
                        ItmTaxGrpAmtF = 0;
                        ItmTaxGrpAmtL = 0;
                        ItmTaxable = false;
                        #endregion

                        #region Set variables
                        ItmQty = GFunc.NEDec(row["ItmQty"], 0);
                        ItmPriceAfter = GFunc.NEDec(row["ItmPriceAfter"], 0);
                        ItmPriceUser = GFunc.NEDec(row["ItmPriceUser"], 0);
                        ItmTaxGrpKey = GFunc.NEInt(row["ItmTaxGrpKey"], 0);
                        ItmTaxGrpRate = GFunc.NEDec(row["ItmTaxGrpRate"], 0);

                        //ItmTaxgrpRate must always follows DocTaxRate when the ItmTaxGrpKey = DocTaxGrpKey
                        if (DocTaxKey == ItmTaxGrpKey)
                            ItmTaxGrpRate = DocTaxRate;
                        #endregion

                        switch (GFunc.GetINTypeGroup(GFunc.NEInt(row["ItmType"], 0)))
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:

                                #region Calculate ItmPrice, ItmAmtShw, ItmAmtF, ItmAmtH

                                //--to check Incorrect ItmPriceUser and ItmAmt-- to remove later  Added by Jane --------------------
                                /*
                                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Quotation ||
                                        objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Order ||
                                        objDoc.DocCodeKey == (int)GEnum.SystemCode.Delivery_Order ||
                                        objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Invoice ||
                                        objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Debit_Note ||
                                        objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Credit_Note ||
                                        objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Sale ||
                                        objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Debit_Note ||
                                        objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Credit_Note )
                                {
                                    PriceDec = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceDecPlace, cn);
                                    PriceRoundMode = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceRoundMode, cn);

                                    ItmDisPercent = GFunc.NEDec(row["ItmDisPercent"], 0);
                                    ItmDisValue = GFunc.RndUD(ItmPriceAfter * ItmDisPercent / 100M, PriceRoundMode, PriceDec);
                                    ItmPriceUser = GFunc.RndUD(ItmPriceAfter - ItmDisValue, PriceRoundMode, PriceDec);
                                }
                                //------------------------------------------------------------------
                                 */
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
                                   
                                ItmAmtShw = GFunc.RndC(ItmPriceUser * ItmQty, GVar.RndDecs.Amtpt);
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
                                    ItmTaxable = false;
                                }
                                else
                                {
                                    //when itmtaxrate > 0 the ItmTaxGrpKey must be the same as the DocTaxGrpkey
                                    if ((ItmTaxGrpRate > 0 && DocTaxKey != ItmTaxGrpKey) == true)
                                        ItmTaxGrpKey = DocTaxKey;

                                    if (ItmTaxGrpRate > 0)
                                    {
                                        if (CalTax)
                                        {
                                            ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                                            ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                                        }
                                        else//set current value
                                        {
                                            ItmTaxGrpAmtF = GFunc.NEDec(row["ItmTaxGrpAmtF"], 0);
                                            ItmTaxGrpAmtL = GFunc.NEDec(row["ItmTaxGrpAmtL"], 0);
                                        }
                                        ItmTaxable = true;
                                    }
                                    else
                                    {
                                        ItmTaxGrpAmtF = 0;
                                        ItmTaxGrpAmtL = 0;
                                        ItmTaxable = false;
                                    }
                                }
                                #endregion
                                
                                if (RunCheck)
                                {
                                    #region Checking 
                                    /*
                                    //to check Incorrect ItmPriceUser and ItmAmt-- to remove later  Added by Jane------------------ 
                                    if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Quotation ||
                                        objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Order ||
                                        objDoc.DocCodeKey == (int)GEnum.SystemCode.Delivery_Order ||
                                        objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Invoice ||
                                        objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Debit_Note ||
                                        objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Credit_Note ||
                                        objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Sale ||
                                        objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Debit_Note ||
                                        objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Credit_Note )
                                    {
                                        if (GFunc.NEDec(row["ItmPriceUser"], 0) != ItmPriceUser)
                                        {
                                            DataSet ds = new DataSet();
                                            DataTable dt = null;
                                            dt = GFunc.ConvertObjectToDataTable(objDoc, "dtHeader");
                                            ds.Tables.Add(dt);
                                            ds.Tables.Add(dtItems.Copy());
                                            string XMLformat = GFunc.ConvertDataTableToXML(ds);
                                            System.IO.File.WriteAllText(Application.StartupPath + "\\ErrorXML.txt", XMLformat);
                                            ds = null;
                                            dt = null;
                                            FailCheckPriceUser = true;
                                        }
                                    }
                                    //------------------------------------------------------------
                                     */
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != ItmPrice)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtShw"], 0) != ItmAmtShw)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != ItmAmtF)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != ItmAmtH)
                                        FailCheck = true;
                                    if ((bool)row["ItmTaxable"] != ItmTaxable)
                                        FailCheck = true;
                                    if (GFunc.NEInt(row["ItmTaxGrpKey"], 0) != ItmTaxGrpKey)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpRate"], 0) != ItmTaxGrpRate)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtF"], 0) != ItmTaxGrpAmtF)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtL"], 0) != ItmTaxGrpAmtL)
                                        FailCheck = true;

                                   /* //to check Incorrect ItmPriceUser and ItmAmt-- to remove later  Added by Jane ----------------
                                    if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Quotation ||
                                       objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Order ||
                                       objDoc.DocCodeKey == (int)GEnum.SystemCode.Delivery_Order ||
                                       objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Invoice ||
                                       objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Debit_Note ||
                                       objDoc.DocCodeKey == (int)GEnum.SystemCode.Sales_Credit_Note ||
                                       objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Sale ||
                                       objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Debit_Note ||
                                       objDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Credit_Note)
                                    {
                                        if (FailCheckPriceUser)
                                        {
                                            MessageBox.Show("Wrong Unit Price calculation!.Please call Techace Support immediately to report this issue.");                                         
                                            return false;
                                        }
                                    }
                                    //----------------------------------------------------------
                                    */
                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                    #endregion
                                }

                                else
                                {
                                    #region Set value to grid
                                    row["ItmPrice"] = ItmPrice;
                                    row["ItmAmtShw"] = ItmAmtShw.ToDBValue();
                                    row["ItmAmtF"] = ItmAmtF;
                                    row["ItmAmtH"] = ItmAmtH;
                                    row["ItmTaxable"] = ItmTaxable;
                                    row["ItmTaxGrpKey"] = GFunc.NEInt(ItmTaxGrpKey, 0);
                                    row["ItmTaxGrpRate"] = ItmTaxGrpRate;
                                    row["ItmTaxGrpAmtF"] = ItmTaxGrpAmtF;
                                    row["ItmTaxGrpAmtL"] = ItmTaxGrpAmtL;
                                    #endregion
                                }

                                #region Calculate running total: CurItmAmt,TotalTaxAmtF,TotalTaxAmtL,DetTotalAmtF,TotalST,TotalTT,TotalCF
                                CurItmAmt = ItmAmtShw;
                                TotalTaxAmtF = TotalTaxAmtF + ItmTaxGrpAmtF;
                                TotalTaxAmtL = TotalTaxAmtL + ItmTaxGrpAmtL;
                                DetTotalAmtF = DetTotalAmtF + ItmAmtF;
                                if (ItmTaxGrpAmtF != 0)
                                    TotalTaxableAmtF = TotalTaxableAmtF + ItmAmtF;

                                TotalCF = TotalCF + CurItmAmt;
                                if (ResetTotal)
                                {
                                    TotalST = CurItmAmt;
                                }
                                else
                                {
                                    TotalST += CurItmAmt;
                                }
                                ResetTotal = false;
                                #endregion

                                if (RunCheck == false)
                                {
                                    #region Update LatestCost and Control Price
                                    row["ItmLatestCostF"] = GFunc.RndDC(GFunc.NEDec(row["ItmLatestCostH"], 0) * GFunc.NEDec(row["ItmConRate"], 0), DocCurrRate, GVar.RndDecs.Prcpt);
                                    row["ItmControlPrice"] = GFunc.RndDC(GFunc.NEDec(row["ItmControlPriceBase"], 0) * GFunc.NEDec(row["ItmConRate"], 0), DocCurrRate, GVar.RndDecs.Prcpt);
                                    #endregion
                                }
                                break;

                            case (int)GEnum.INTypeGrp.Charges:

                                #region Calculate ItmDisPercent,ItmDisValue,ItmPriceUser,ItmAmtShw
                                if (ItmQty > 0)
                                {
                                    //Calculate Percentage of Discount/Charges
                                    ItmPriceAfter = GFunc.RndC(Math.Abs(PreItmAmt.Value) * ItmQty, GVar.RndDecs.Prcpt);
                                }
                                else
                                {
                                    //Use ItmAmtshw
                                    ItmPriceAfter = GFunc.NEDec(row["ItmPriceAfter"], 0);
                                }
                                ItmDisRate = GFunc.NEDec(row["ItmDisPercent"], 0);
                                ItmDisValue = ItmPriceAfter * ItmDisRate / 100M;
                                ItmPriceUser = ItmPriceAfter - ItmDisValue;
                                ItmAmtShw = GFunc.RndC(ItmPriceUser, GVar.RndDecs.Amtpt);

                                #endregion

                                #region Calculate ItmPrice, ItmAmtF, ItmAmtH
                                if (objDoc.DocType == 110)   //if Tax Inclusive,
                                {
                                    if (ItmTaxGrpRate > 0)
                                        if (DocTaxRate == 0)
                                            ItmPrice = ItmAmtShw;
                                        else
                                            ItmPrice = GFunc.RndDC(ItmAmtShw, (1 + ItmTaxGrpRate), GVar.RndDecs.Prcpt);
                                    else
                                        ItmPrice = ItmAmtShw;
                                }
                                else
                                {
                                    ItmPrice = ItmAmtShw;
                                }

                                ItmAmtF = GFunc.RndC(ItmPrice, GVar.RndDecs.Amtpt);
                                ItmAmtH = GFunc.RndC(ItmPrice * DocCurrRate, GVar.RndDecs.Amtpt);
                                #endregion

                                #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                                if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                                {
                                    //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                                    ItmTaxGrpKey = DocTaxKey;
                                    ItmTaxGrpRate = 0;
                                    ItmTaxGrpAmtF = 0;
                                    ItmTaxGrpAmtL = 0;
                                    ItmTaxable = false;
                                }
                                else
                                {
                                    //when itmtaxrate > 0 the ItmTaxGrpKey must be the same as the DocTaxGrpkey
                                    if ((ItmTaxGrpRate > 0 && DocTaxKey != ItmTaxGrpKey) == true)
                                        ItmTaxGrpKey = DocTaxKey;

                                    if (ItmTaxGrpRate > 0)
                                    {
                                        if (CalTax)
                                        {
                                            ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                                            ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                                        }
                                        else//set current value
                                        {
                                            ItmTaxGrpAmtF = GFunc.NEDec(row["ItmTaxGrpAmtF"], 0);
                                            ItmTaxGrpAmtL = GFunc.NEDec(row["ItmTaxGrpAmtL"], 0);
                                        }

                                        ItmTaxable = true;
                                    }
                                    else
                                    {
                                        ItmTaxGrpAmtF = 0;
                                        ItmTaxGrpAmtL = 0;
                                        ItmTaxable = false;
                                    }
                                }
                                #endregion

                                if (RunCheck)
                                {
                                    #region Checking
                                    if (GFunc.NEDec(row["ItmDisValue"], 0) != ItmDisValue)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmPriceUser"], 0) != ItmAmtShw)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != ItmPrice)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtShw"], 0) != ItmAmtShw)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != ItmAmtF)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != ItmAmtH)
                                        FailCheck = true;
                                    if ((bool)row["ItmTaxable"] != ItmTaxable)
                                        FailCheck = true;
                                    if (GFunc.NEInt(row["ItmTaxGrpKey"], 0) != ItmTaxGrpKey)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpRate"], 0) != ItmTaxGrpRate)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtF"], 0) != ItmTaxGrpAmtF)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtL"], 0) != ItmTaxGrpAmtL)
                                        FailCheck = true;

                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Set value to grid
                                    row["ItmDisPercent"] = ItmDisRate.ToDBValue();
                                    row["ItmDisValue"] = ItmDisValue.ToDBValue();
                                    row["ItmPriceAfter"] = ItmPriceAfter.ToDBValue();
                                    row["ItmPriceUser"] = ItmAmtShw.ToDBValue();
                                    row["ItmPrice"] = ItmPrice;
                                    row["ItmAmtShw"] = ItmAmtShw.ToDBValue();
                                    row["ItmAmtF"] = ItmAmtF;
                                    row["ItmAmtH"] = ItmAmtH;
                                    row["ItmTaxable"] = ItmTaxable;
                                    row["ItmTaxGrpKey"] = ItmTaxGrpKey.ToDBValue();
                                    row["ItmTaxGrpRate"] = ItmTaxGrpRate;
                                    row["ItmTaxGrpAmtF"] = ItmTaxGrpAmtF;
                                    row["ItmTaxGrpAmtL"] = ItmTaxGrpAmtL;
                                    #endregion
                                }

                                #region Calculate running total: CurItmAmt,TotalTaxAmtF,TotalTaxAmtL,DetTotalAmtF,TotalST,TotalTT,TotalCF
                                CurItmAmt = ItmAmtShw;
                                TotalTaxAmtF = TotalTaxAmtF + ItmTaxGrpAmtF;
                                TotalTaxAmtL = TotalTaxAmtL + ItmTaxGrpAmtL;
                                DetTotalAmtF = DetTotalAmtF + ItmAmtF;
                                if (ItmTaxGrpAmtF != 0)
                                    TotalTaxableAmtF = TotalTaxableAmtF + ItmAmtF;

                                TotalCF = TotalCF + CurItmAmt;
                                if (ResetTotal)
                                {
                                    TotalST = CurItmAmt;
                                }
                                else
                                {
                                    TotalST += CurItmAmt;
                                }
                                ResetTotal = false;
                                #endregion

                                if (RunCheck == false)
                                {
                                    #region Update LatestCost and Control Price
                                    row["ItmLatestCostF"] = GFunc.RndDC(GFunc.NEDec(row["ItmLatestCostH"], 0), DocCurrRate, GVar.RndDecs.Prcpt);
                                    row["ItmControlPrice"] = GFunc.RndDC(GFunc.NEDec(row["ItmControlPriceBase"], 0), DocCurrRate, GVar.RndDecs.Prcpt);
                                    #endregion
                                }

                                break;

                            case (int)GEnum.INTypeGrp.Discount:

                                #region Calculate ItmAmtShw
                                if (ItmQty > 0)
                                {
                                    //Calculate Percentage of Discount/Charges
                                    ItmAmtShw = -GFunc.RndC(Math.Abs(PreItmAmt.Value) * GFunc.NEDec(row["ItmQty"], 0), GVar.RndDecs.Amtpt);
                                }
                                else
                                {
                                    //Use ItmAmtshw
                                    ItmAmtShw = GFunc.RndC(GFunc.NEDec(row["ItmAmtShw"], 0), GVar.RndDecs.Amtpt);
                                }
                                #endregion

                                #region Calculate ItmAmtF, ItmAmtH
                                if (objDoc.DocType == 110)   //if Tax Inclusive,
                                {
                                    if (ItmTaxGrpRate > 0)
                                        if (DocTaxRate == 0)
                                            ItmAmtF = ItmAmtShw;
                                        else
                                            ItmAmtF = GFunc.RndDC(ItmAmtShw, (1 + ItmTaxGrpRate), GVar.RndDecs.Amtpt);
                                    else
                                        ItmAmtF = ItmAmtShw;
                                }
                                else
                                {
                                    ItmAmtF = ItmAmtShw;
                                }
                                ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                                #endregion

                                #region Calculate ItmTaxGrpKey, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                                if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                                {
                                    //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                                    ItmTaxGrpKey = DocTaxKey;
                                    ItmTaxGrpRate = 0;
                                    ItmTaxGrpAmtF = 0;
                                    ItmTaxGrpAmtL = 0;
                                    ItmTaxable = false;
                                }
                                else
                                {
                                    //when itmtaxrate > 0 the ItmTaxGrpKey must be the same as the DocTaxGrpkey
                                    if ((ItmTaxGrpRate > 0 && DocTaxKey != ItmTaxGrpKey) == true)
                                        ItmTaxGrpKey = DocTaxKey;

                                    if (ItmTaxGrpRate > 0)
                                    {
                                        if (CalTax)
                                        {
                                            ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                                            ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                                        }
                                        else//set current value
                                        {
                                            ItmTaxGrpAmtF = GFunc.NEDec(row["ItmTaxGrpAmtF"], 0);
                                            ItmTaxGrpAmtL = GFunc.NEDec(row["ItmTaxGrpAmtL"], 0);
                                        }
                                        ItmTaxable = true;
                                    }
                                    else
                                    {
                                        ItmTaxGrpAmtF = 0;
                                        ItmTaxGrpAmtL = 0;
                                        ItmTaxable = false;
                                    }
                                }
                                #endregion

                                if (RunCheck)
                                {
                                    #region Checking
                                    if (!GFunc.IsNE(row["ItmListPrice"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmPriceBefore"]))
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmVendorPrice"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmVendorPriceRatio"], 0) != 0)
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmPriceAfter"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmDisPercent"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmDisValue"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmPriceUser"]))
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtShw"], 0) != ItmAmtShw)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != ItmAmtF)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != ItmAmtH)
                                        FailCheck = true;
                                    if ((bool)row["ItmTaxable"] != ItmTaxable)
                                        FailCheck = true;
                                    if (GFunc.NEInt(row["ItmTaxGrpKey"], 0) != ItmTaxGrpKey)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpRate"], 0) != ItmTaxGrpRate)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtF"], 0) != ItmTaxGrpAmtF)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtL"], 0) != ItmTaxGrpAmtL)
                                        FailCheck = true;


                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Set Values to grid
                                    row["ItmListPrice"] = DBNull.Value;
                                    row["ItmPriceBefore"] = DBNull.Value;
                                    row["ItmVendorPrice"] = 0;
                                    row["ItmVendorPriceRatio"] = 0;
                                    row["ItmPriceAfter"] = DBNull.Value;
                                    row["ItmDisPercent"] = DBNull.Value;
                                    row["ItmDisValue"] = DBNull.Value;
                                    row["ItmPriceUser"] = DBNull.Value;
                                    row["ItmAmtShw"] = ItmAmtShw;
                                    row["ItmPrice"] = 0;
                                    row["ItmAmtF"] = ItmAmtF;
                                    row["ItmAmtH"] = ItmAmtH;
                                    row["ItmTaxable"] = ItmTaxable;
                                    row["ItmTaxGrpKey"] = ItmTaxGrpKey;
                                    row["ItmTaxGrpRate"] = ItmTaxGrpRate;
                                    row["ItmTaxGrpAmtF"] = ItmTaxGrpAmtF;
                                    row["ItmTaxGrpAmtL"] = ItmTaxGrpAmtL;
                                    #endregion
                                }

                                #region Calculate running total: CurItmAmt,TotalTaxAmtF,TotalTaxAmtL,DetTotalAmtF,TotalST,TotalTT,TotalCF
                                CurItmAmt = ItmAmtShw;
                                TotalTaxAmtF = TotalTaxAmtF + ItmTaxGrpAmtF;
                                TotalTaxAmtL = TotalTaxAmtL + ItmTaxGrpAmtL;
                                DetTotalAmtF = DetTotalAmtF + ItmAmtF;
                                if (ItmTaxGrpAmtF != 0)
                                    TotalTaxableAmtF = TotalTaxableAmtF + ItmAmtF;

                                TotalCF = TotalCF + CurItmAmt;
                                if (ResetTotal)
                                {
                                    TotalST = CurItmAmt;
                                }
                                else
                                {
                                    TotalST += CurItmAmt;
                                }
                                ResetTotal = false;
                                #endregion

                                if (RunCheck == false)
                                {
                                    #region Update LatestCost and Control Price
                                    row["ItmLatestCostF"] = 0;
                                    row["ItmControlPrice"] = 0;
                                    #endregion
                                }
                                break;

                            case (int)GEnum.INTypeGrp.Total:

                                #region Calculate CurItmAmt
                                if (GFunc.NEInt(row["ItmType"], 0) == (int)GEnum.ItemType.Sub_Total)
                                {
                                    CurItmAmt = TotalST;
                                }
                                else if (GFunc.NEInt(row["ItmType"], 0) == (int)GEnum.ItemType.BF_Total)
                                {
                                    CurItmAmt = TotalCF;
                                }
                                else
                                {
                                    CurItmAmt = TotalPrevious + TotalST;
                                }
                                TotalPrevious = CurItmAmt;
                                ResetTotal = true;
                                #endregion

                                if (RunCheck)
                                {
                                    #region Checking
                                    if (!GFunc.IsNE(row["ItmListPrice"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmPriceBefore"]))
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmVendorPrice"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmVendorPriceRatio"], 0) != 0)
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmPriceAfter"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmDisPercent"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmDisValue"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmPriceUser"]))
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtShw"], 0) != CurItmAmt)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != 0)
                                        FailCheck = true;
                                    if ((bool)row["ItmTaxable"] != false)
                                        FailCheck = true;
                                    if (GFunc.NEInt(row["ItmTaxGrpKey"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpRate"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtF"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtL"], 0) != 0)
                                        FailCheck = true;

                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Update ItmListPrice,ItmPriceBefore,ItmVendorPrice,ItmVendorPriceRatio,ItmPriceAfter,ItmDisPercent,ItmDisValue,ItmPriceUser,ItmAmtShw
                                    row["ItmListPrice"] = DBNull.Value;
                                    row["ItmPriceBefore"] = DBNull.Value;
                                    row["ItmVendorPrice"] = 0;
                                    row["ItmVendorPriceRatio"] = 0;
                                    row["ItmPriceAfter"] = DBNull.Value;
                                    row["ItmDisPercent"] = DBNull.Value;
                                    row["ItmDisValue"] = DBNull.Value;
                                    row["ItmPriceUser"] = DBNull.Value;
                                    row["ItmPrice"] = 0;
                                    row["ItmAmtShw"] = CurItmAmt;
                                    row["ItmAmtF"] = 0;
                                    row["ItmAmtH"] = 0;
                                    row["ItmTaxable"] = false;
                                    row["ItmTaxGrpKey"] = 0;
                                    row["ItmTaxGrpRate"] = 0;
                                    row["ItmTaxGrpAmtF"] = 0;
                                    row["ItmTaxGrpAmtL"] = 0;
                                    #endregion
                                }
                                break;

                            default:
                                if (GFunc.IsNEZ(row["ItmType"]))
                                {
                                    MsgBox.Show(cn, MsgID.Common.InvalidParametersWithFields + "%Item Type%Cal Doc AR");
                                    return false;
                                }

                                #region Assume Header,Remark - Update ItmPrice, ItmAmtF, ItmAmtH
                                if (RunCheck)
                                {
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtF"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtL"], 0) != 0)
                                        FailCheck = true;

                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                }
                                else
                                {
                                    row["ItmPrice"] = 0;
                                    row["ItmAmtF"] = 0;
                                    row["ItmAmtH"] = 0;
                                    row["ItmTaxGrpAmtF"] = 0;
                                    row["ItmTaxGrpAmtL"] = 0;
                                }
                                #endregion
                                break;
                        }

                        if (RunCheck == false)
                        {
                            #region Reassign Item SN numbering
                            SN = SN + 1;
                            row["ItmSN"] = SN;
                            #endregion
                        }
                        PreItmAmt = CurItmAmt;
                    }
                    #endregion

                    if (RunCheck == false)
                    {
                        #region Assign SN to all detail assembly and batch

                        dtParentFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("LineType") == 1000 && r.Field<int>("LineLinkKey") == 0)
                            .OrderBy(p => p.Field<decimal>("ItmSN"));

                        //to set Childs' ItmSN and ItmDetSN
                        foreach (DataRow rowParent in dtParentFilter)
                        {
                            dtItemFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("LineType") >= 1000
                                && r.Field<int>("LineLinkKey") == GFunc.NEInt(rowParent["DocItmKey"], -1)).OrderBy(p => p.Field<decimal>("ItmDetSN"));

                            if(dtItemFilter.Count()>0) //contain child items
                            {
                                switch (rowParent.Field<int>("ItmType"))
                                {
                                    case (int)GEnum.ItemType.Assembly:                                                                      
                                    case (int)GEnum.ItemType.StockB:
                                    case (int)GEnum.ItemType.Finished_GDB:
                                    case (int)GEnum.ItemType.Serial_StockB:
                                    case (int)GEnum.ItemType.Serial_Finished_GDB:                                   
                                        break;  
                                    default:
                                        foreach (DataRow rowChild in dtItemFilter)
                                        {
                                            //Not Ready, Mic to check for Batch Items type
                                            if (rowChild.Field<int>("LineType") == 1100 && rowParent.Field<int>("ItmType") != (int)GEnum.ItemType.Assembly)
                                                rowChild.Delete();                                           
                                        }
                                        dtItems.AcceptChanges();
                                        continue; //skip to another parent row
                                }
                            }

                            int childSN = 1;

                            foreach (DataRow rowChild in dtItemFilter)
                            {
                                rowChild["ItmSN"] = rowParent["ItmSN"];

                                rowChild["ItmDetSN"] = childSN;
                                childSN++;

                                if ((GFunc.NEInt(rowChild["LineType"], 0) == 1100/*Assembley*/) && (GFunc.NEBool(rowChild["ItmIGrpQtyLock"], false) == false))
                                {
                                    rowChild["ItmQty"] = GFunc.RndC(GFunc.NEDec(rowParent["ItmQty"], 0)/*Parent Qty*/ * GFunc.NEDec(rowChild["ItmIGrpQtySet"], 0), GVar.RndDecs.Qtypt);
                                    decimal amtShw = GFunc.RndC(GFunc.NEDec(rowChild["ItmQty"], 0) * GFunc.NEDec(rowChild["ItmPriceAfter"], 0), GVar.RndDecs.Amtpt);
                                    rowChild["ItmAmtShw"] = amtShw;
                                    rowChild["ItmAmtF"] = amtShw;
                                    rowChild["ItmAmtH"] = GFunc.RndC(amtShw * DocCurrRate, GVar.RndDecs.Amtpt);
                                }
                            }
                        }

                        #endregion
                    }
                }
                if (RunCheck)
                {
                    #region Check Document Total
                    TotalTaxAmtF = GFunc.RndC(TotalTaxAmtF, GVar.RndDecs.Amtpt);
                    TotalTaxAmtL = GFunc.RndC(TotalTaxAmtL, GVar.RndDecs.Amtpt);
                    DocOverallDisAmt = GFunc.RndDC(DetTotalAmtF * DocDisRate, 100M, GVar.RndDecs.Amtpt);
                    DocTotalAfterDis = DetTotalAmtF - DocOverallDisAmt;
                    if (CalTax)
                    {
                        if (SysOptionUtility.TaxCalculationOnTotal)
                        {
                            TotalTaxableAmtF = TotalTaxableAmtF - GFunc.RndDC(TotalTaxableAmtF * DocDisRate, 100M, GVar.RndDecs.Amtpt);
                            DocTaxAmt = GFunc.RndC(TotalTaxableAmtF * DocTaxRate, GVar.RndDecs.Amtpt);
                            DocTaxAmtL = GFunc.RndC(TotalTaxableAmtF * DocTaxRate * DocCountryRate, GVar.RndDecs.Amtpt);
                        }
                        else
                        {
                            DocTaxAmt = TotalTaxAmtF - GFunc.RndDC(TotalTaxAmtF * DocDisRate, 100M, GVar.RndDecs.Amtpt);
                            DocTaxAmtL = TotalTaxAmtL - GFunc.RndDC(TotalTaxAmtL * DocDisRate, 100M, GVar.RndDecs.Amtpt);
                        }
                    }
                    else
                    {
                        DocTaxAmt = GFunc.GetDecimalPropertyValue("DocTaxTotal", objDoc).Value;
                        DocTaxAmtL = GFunc.GetDecimalPropertyValue("DocTaxTotalLocal", objDoc).Value;
                    }

                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocSubTotal", objDoc), 0) != DetTotalAmtF)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocOverallDisAmt", objDoc), 0) != DocOverallDisAmt)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocTotalAfterDis", objDoc), 0) != DocTotalAfterDis)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocTaxTotal", objDoc), 0) != DocTaxAmt)
                        FailCheck = true;

                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Quotation:
                        case (int)GEnum.SystemCode.Sales_Order:
                        case (int)GEnum.SystemCode.Reserve_Order:
                        case (int)GEnum.SystemCode.Delivery_Order:
                            if (GFunc.NEDec(GFunc.GetPropertyValue("DocGrand", objDoc), 0) != DocTotalAfterDis + DocTaxAmt)
                                FailCheck = true;
                            break;

                        case (int)GEnum.SystemCode.Sales_Invoice:
                        case (int)GEnum.SystemCode.Sales_Debit_Note:
                        case (int)GEnum.SystemCode.Sales_Credit_Note:
                        case (int)GEnum.SystemCode.Cash_Sale:
                        case (int)GEnum.SystemCode.Cash_Debit_Note:
                        case (int)GEnum.SystemCode.Cash_Credit_Note:
                            if (GFunc.NEDec(GFunc.GetPropertyValue("DocTotal", objDoc), 0) != DocTotalAfterDis + DocTaxAmt)
                                FailCheck = true;
                            if (GFunc.NEDec(GFunc.GetPropertyValue("DocGrand", objDoc), 0) != DocTotalAfterDis + DocTaxAmt - GFunc.GetDecimalPropertyValue("DocPaidAmtF", objDoc))
                                FailCheck = true;
                            break;
                    }
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocHome", objDoc), 0) != GFunc.RndC(GFunc.GetDecimalPropertyValue("DocGrand", objDoc) * DocCurrRate, GVar.RndDecs.Amtpt))
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocTaxTotalLocal", objDoc), 0) != DocTaxAmtL)
                        FailCheck = true;

                    if (FailCheck)
                    {
                        MsgBox.Show(cn, MsgID.Document.SaveFailTotalCalculationWrong);
                        return false;
                    }
                    #endregion
                }
                else
                {
                    #region Calculate Document Total
                    TotalTaxAmtF = GFunc.RndC(TotalTaxAmtF, GVar.RndDecs.Amtpt);
                    TotalTaxAmtL = GFunc.RndC(TotalTaxAmtL, GVar.RndDecs.Amtpt);
                    DocOverallDisAmt = GFunc.RndDC(DetTotalAmtF * DocDisRate, 100M, GVar.RndDecs.Amtpt);
                    DocTotalAfterDis = DetTotalAmtF - DocOverallDisAmt;
                    if (CalTax)
                    {
                        if (SysOptionUtility.TaxCalculationOnTotal)
                        {
                            TotalTaxableAmtF = TotalTaxableAmtF - GFunc.RndDC(TotalTaxableAmtF * DocDisRate, 100M, GVar.RndDecs.Amtpt);
                            DocTaxAmt = GFunc.RndC(TotalTaxableAmtF * DocTaxRate, GVar.RndDecs.Amtpt);
                            DocTaxAmtL = GFunc.RndC(TotalTaxableAmtF * DocTaxRate * DocCountryRate, GVar.RndDecs.Amtpt);
                        }
                        else
                        {
                            DocTaxAmt = TotalTaxAmtF - GFunc.RndDC(TotalTaxAmtF * DocDisRate, 100M, GVar.RndDecs.Amtpt);
                            DocTaxAmtL = TotalTaxAmtL - GFunc.RndDC(TotalTaxAmtL * DocDisRate, 100M, GVar.RndDecs.Amtpt);
                        }
                    }
                    else
                    {
                        DocTaxAmt = GFunc.GetDecimalPropertyValue("DocTaxTotal", objDoc).Value;
                        DocTaxAmtL = GFunc.GetDecimalPropertyValue("DocTaxTotalLocal", objDoc).Value;
                    }
                    GFunc.SetPropertyValue("DocSubTotal", objDoc, DetTotalAmtF);
                    GFunc.SetPropertyValue("DocOverallDisAmt", objDoc, DocOverallDisAmt);
                    GFunc.SetPropertyValue("DocTotalAfterDis", objDoc, DocTotalAfterDis);
                    GFunc.SetPropertyValue("DocTaxTotal", objDoc, DocTaxAmt);

                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Quotation:
                        case (int)GEnum.SystemCode.Sales_Order:
                        case (int)GEnum.SystemCode.Delivery_Order:
                        case (int)GEnum.SystemCode.Reserve_Order:
                            GFunc.SetPropertyValue("DocGrand", objDoc, DocTotalAfterDis + DocTaxAmt);
                            break;

                        case (int)GEnum.SystemCode.Sales_Invoice:
                        case (int)GEnum.SystemCode.Sales_Debit_Note:
                        case (int)GEnum.SystemCode.Sales_Credit_Note:
                        case (int)GEnum.SystemCode.Cash_Sale:
                        case (int)GEnum.SystemCode.Cash_Debit_Note:
                        case (int)GEnum.SystemCode.Cash_Credit_Note:
                            GFunc.SetPropertyValue("DocGrand", objDoc, DocTotalAfterDis + DocTaxAmt - GFunc.GetDecimalPropertyValue("DocPaidAmtF", objDoc));
                            GFunc.SetPropertyValue("DocTotal", objDoc, DocTotalAfterDis + DocTaxAmt);
                            break;
                    }
                    GFunc.SetPropertyValue("DocHome", objDoc, GFunc.RndC(GFunc.GetDecimalPropertyValue("DocGrand", objDoc) * DocCurrRate, GVar.RndDecs.Amtpt));
                    GFunc.SetPropertyValue("DocTaxTotalLocal", objDoc, DocTaxAmtL);
                    #endregion
                }
                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        private static bool CalDocAP(SqlConnection cn, Document objDoc, DataTable dtItems, bool CalTax, bool RunCheck)
        {

            #region variable
            IEnumerable<DataRow> dtItemFilter = null;
            IEnumerable<DataRow> dtParentFilter = null;
            bool FailCheck = false;
            decimal? PreItmAmt = 0;
            decimal? CurItmAmt = 0;
            decimal? TotalPrevious = 0; //Previous Total line value - use for calculation of Total Line
            decimal? TotalST = 0;       //Sub Total
            decimal? TotalCF = 0;       //CF Total
            decimal? TotalTaxAmtF = 0;  //Total of ItmTaxGrpAmtF
            decimal? TotalTaxAmtL = 0;  //Total of ItmTaxGrpAmtL
            decimal? TotalTaxableAmtF = 0;  //Total Taxable AmountF
            decimal? DetTotalAmtF = 0;  //Total of all detail items w/o gst
            bool ResetTotal = false;

            DateTime DocDate;
            int? DocConKey = 0;
            int? DocCurrKey = 1;
            decimal? DocCurrRate = 1;
            decimal? DocCountryRate = 1;
            decimal? DocDisRate = 0;
            decimal? DocTotalAfterDis = 0;
            decimal? DocOverallDisAmt = 0;
            int? DocTaxKey = 0;
            decimal? DocTaxRate = 0;
            decimal? DocTaxAmt = 0;
            decimal? DocTaxAmtL = 0;
            decimal DocTotal = 0M;

            decimal? ItmQty = 0;
            decimal? ItmPriceAfter = 0;
            decimal? ItmDisRate = 0;
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
            bool ItmTaxable = false;

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

            string OpValue = string.Empty;
            decimal? SN = 0;

            REFTaxGrp objTaxGrp = REFTaxGrp.Get(cn, GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc));

            #endregion

            try
            {
                #region Assign Variables
                dtItemFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == 0).OrderBy(r => r.Field<decimal>("ItmSN"));

                DocDate = (DateTime)objDoc.DocDate;
                DocConKey = GFunc.NEInt(GFunc.GetPropertyValue("DocConKey", objDoc), 0);
                DocCurrKey = GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 0);
                DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                DocTaxKey = GFunc.NEInt(GFunc.GetPropertyValue("DocTaxGrpKey", objDoc), 0);
                DocTaxRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);
                DocDisRate = GFunc.NEDec(GFunc.GetPropertyValue("DocOverallDisRate", objDoc), 0);
                //if (SysOptionUtility.GetInt("CountryCurrency", cn) == 1) // Country Currency = Home -- Added by Jane. Mic to check
                    //DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                if ((DocCurrKey == SysOptionUtility.GetInt("CountryCurrency", cn))|| objTaxGrp.GSTCustom == true)// Doc Currency= Country Currency OR GSTCustom-- Modified by May. Mic to check
                {
                    DocCountryRate = 1;
                    GFunc.SetPropertyValue("DocCountryRate", objDoc, DocCountryRate);
                }
                else 
                {
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);
                    if (objTaxGrp.GSTCustom == false) //And DocCurrKey <> CountryCurrency
                    {
                        if (DocCountryRate == 1)//This condition occurs when TaxGrp changed from CustomGST to normal GST
                        {
                            DocCountryRate = DocCurrRate;
                            GFunc.SetPropertyValue("DocCountryRate", objDoc, DocCountryRate);
                        }
                    }
                }
               
                #endregion

                if (dtItems.Rows.Count > 0)
                {
                    #region looping to all detail items to calculate each row
                    foreach (DataRow row in dtItemFilter)
                    {
                        #region reset variables
                        ItmQty = 0;
                        ItmPriceAfter = 0;
                        ItmDisRate = 0;
                        ItmDisValue = 0;
                        ItmPriceUser = 0;
                        ItmPrice = 0;
                        ItmAmtShw = 0;
                        ItmAmtF = 0;
                        ItmAmtH = 0;
                        ItmTaxGrpKey = 0;
                        ItmTaxGrpRate = 0;
                        ItmTaxGrpAmtF = 0;
                        ItmTaxGrpAmtL = 0;
                        ItmTaxable = false;
                        #endregion

                        #region Set variables
                        ItmQty = GFunc.NEDec(row["ItmQty"], 0);
                        ItmPriceAfter = GFunc.NEDec(row["ItmPriceAfter"], 0);
                        ItmPriceUser = GFunc.NEDec(row["ItmPriceUser"], 0);
                        ItmTaxGrpKey = GFunc.NEInt(row["ItmTaxGrpKey"], 0);
                        ItmTaxGrpRate = GFunc.NEDec(row["ItmTaxGrpRate"], 0);

                        //ItmTaxgrpRate must always follows DocTaxRate when the ItmTaxGrpKey = DocTaxGrpKey
                        if (DocTaxKey == ItmTaxGrpKey)
                            ItmTaxGrpRate = DocTaxRate;
                        #endregion

                        switch (GFunc.GetINTypeGroup(GFunc.NEInt(row["ItmType"], 0)))
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:

                                #region Calculate ItmPrice, ItmAmtShw, ItmAmtF, ItmAmtH
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

                                ItmAmtShw = GFunc.RndC(ItmPriceUser * ItmQty, GVar.RndDecs.Amtpt);
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
                                    ItmTaxable = false;
                                }
                                else
                                {
                                    //when itmtaxrate > 0 the ItmTaxGrpKey must be the same as the DocTaxGrpkey
                                    if ((ItmTaxGrpRate > 0 && DocTaxKey != ItmTaxGrpKey) == true)
                                        ItmTaxGrpKey = DocTaxKey;

                                    if (ItmTaxGrpRate > 0)
                                    {
                                        if (CalTax)
                                        {
                                            ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                                            ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                                        }
                                        else//set current value
                                        {
                                            ItmTaxGrpAmtF = GFunc.NEDec(row["ItmTaxGrpAmtF"], 0);
                                            ItmTaxGrpAmtL = GFunc.NEDec(row["ItmTaxGrpAmtL"], 0);
                                        }
                                        ItmTaxable = true;
                                    }
                                    else
                                    {
                                        ItmTaxGrpAmtF = 0;
                                        ItmTaxGrpAmtL = 0;
                                        ItmTaxable = false;
                                    }
                                }
                                #endregion

                                if (RunCheck)
                                {
                                    #region Checking
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != ItmPrice)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtShw"], 0) != ItmAmtShw)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != ItmAmtF)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != ItmAmtH)
                                        FailCheck = true;
                                    if ((bool)row["ItmTaxable"] != ItmTaxable)
                                        FailCheck = true;
                                    if (GFunc.NEInt(row["ItmTaxGrpKey"], 0) != ItmTaxGrpKey)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpRate"], 0) != ItmTaxGrpRate)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtF"], 0) != ItmTaxGrpAmtF)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtL"], 0) != ItmTaxGrpAmtL)
                                        FailCheck = true;

                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                    #endregion
                                }

                                else
                                {
                                    #region Set value to grid
                                    row["ItmPrice"] = ItmPrice;
                                    row["ItmAmtShw"] = ItmAmtShw.ToDBValue();
                                    row["ItmAmtF"] = ItmAmtF;
                                    row["ItmAmtH"] = ItmAmtH;
                                    row["ItmTaxable"] = ItmTaxable;
                                    row["ItmTaxGrpKey"] = ItmTaxGrpKey.ToDBValue();
                                    row["ItmTaxGrpRate"] = ItmTaxGrpRate;
                                    row["ItmTaxGrpAmtF"] = ItmTaxGrpAmtF;
                                    row["ItmTaxGrpAmtL"] = ItmTaxGrpAmtL;
                                    #endregion
                                }

                                #region Calculate running total: CurItmAmt,DocAddCostItmAmtF,TotalTaxAmtF,TotalTaxAmtL,DetTotalAmtF,TotalST,TotalTT,TotalCF
                                CurItmAmt = ItmAmtShw;
                                DocAddCostItmAmtF = DocAddCostItmAmtF + ItmAmtF;
                                TotalTaxAmtF = TotalTaxAmtF + ItmTaxGrpAmtF;
                                TotalTaxAmtL = TotalTaxAmtL + ItmTaxGrpAmtL;
                                DetTotalAmtF = DetTotalAmtF + ItmAmtF;
                                if (ItmTaxGrpAmtF != 0)
                                    TotalTaxableAmtF = TotalTaxableAmtF + ItmAmtF;

                                TotalCF = TotalCF + CurItmAmt;
                                if (ResetTotal)
                                {
                                    TotalST = CurItmAmt;
                                }
                                else
                                {
                                    TotalST += CurItmAmt;
                                }
                                ResetTotal = false;
                                #endregion

                                if (RunCheck == false)
                                {
                                    #region Update LatestCost
                                    row["ItmLatestCostF"] = GFunc.RndDC(GFunc.NEDec(row["ItmLatestCostH"], 0) * GFunc.NEDec(row["ItmConRate"], 0), DocCurrRate, GVar.RndDecs.Prcpt);
                                    #endregion
                                }
                                break;

                            case (int)GEnum.INTypeGrp.Charges:

                                #region Calculate ItmDisPercent,ItmDisValue,ItmPriceUser,ItmAmtShw
                                if (ItmQty > 0)
                                {
                                    ItmPriceAfter = GFunc.RndC(Math.Abs(PreItmAmt.Value) * ItmQty, GVar.RndDecs.Prcpt);
                                }
                                else
                                {
                                    ItmPriceAfter = GFunc.NEDec(row["ItmPriceAfter"], 0);
                                }
                                ItmDisRate = GFunc.NEDec(row["ItmDisPercent"], 0);
                                ItmDisValue = ItmPriceAfter * ItmDisRate / 100M;
                                ItmPriceUser = ItmPriceAfter - ItmDisValue;
                                ItmAmtShw = GFunc.RndC(ItmPriceUser, GVar.RndDecs.Amtpt);

                                #endregion

                                #region Calculate ItmPrice, ItmAmtF, ItmAmtH
                                if (objDoc.DocType == 110)   //if Tax Inclusive,
                                {
                                    if (ItmTaxGrpRate > 0)
                                        if (DocTaxRate == 0)
                                            ItmPrice = ItmAmtShw;
                                        else
                                            ItmPrice = GFunc.RndDC(ItmAmtShw, (1 + ItmTaxGrpRate), GVar.RndDecs.Prcpt);
                                    else
                                        ItmPrice = ItmAmtShw;
                                }
                                else
                                {
                                    ItmPrice = ItmAmtShw;
                                }

                                ItmAmtF = GFunc.RndC(ItmPrice, GVar.RndDecs.Amtpt);
                                ItmAmtH = GFunc.RndC(ItmPrice * DocCurrRate, GVar.RndDecs.Amtpt);
                                #endregion

                                #region Calculate ItmTaxGrpKey, ItmTaxGrpRate, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                                if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                                {
                                    //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                                    ItmTaxGrpKey = DocTaxKey;
                                    ItmTaxGrpRate = 0;
                                    ItmTaxGrpAmtF = 0;
                                    ItmTaxGrpAmtL = 0;
                                    ItmTaxable = false;
                                }
                                else
                                {
                                    //when itmtaxrate > 0 the ItmTaxGrpKey must be the same as the DocTaxGrpkey
                                    if ((ItmTaxGrpRate > 0 && DocTaxKey != ItmTaxGrpKey) == true)
                                        ItmTaxGrpKey = DocTaxKey;

                                    if (ItmTaxGrpRate > 0)
                                    {
                                        if (CalTax)
                                        {
                                            ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                                            ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                                        }
                                        else//set current value
                                        {
                                            ItmTaxGrpAmtF = GFunc.NEDec(row["ItmTaxGrpAmtF"], 0);
                                            ItmTaxGrpAmtL = GFunc.NEDec(row["ItmTaxGrpAmtL"], 0);
                                        }
                                        ItmTaxable = true;
                                    }
                                    else
                                    {
                                        ItmTaxGrpAmtF = 0;
                                        ItmTaxGrpAmtL = 0;
                                        ItmTaxable = false;
                                    }
                                }
                                #endregion

                                if (RunCheck)
                                {
                                    #region Checking
                                    if (GFunc.NEDec(row["ItmDisValue"], 0) != ItmDisValue)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmPriceUser"], 0) != ItmAmtShw)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != ItmPrice)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtShw"], 0) != ItmAmtShw)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != ItmAmtF)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != ItmAmtH)
                                        FailCheck = true;
                                    if ((bool)row["ItmTaxable"] != ItmTaxable)
                                        FailCheck = true;
                                    if (GFunc.NEInt(row["ItmTaxGrpKey"], 0) != ItmTaxGrpKey)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpRate"], 0) != ItmTaxGrpRate)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtF"], 0) != ItmTaxGrpAmtF)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtL"], 0) != ItmTaxGrpAmtL)
                                        FailCheck = true;

                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Set value to grid
                                    row["ItmDisPercent"] = ItmDisRate.ToDBValue();
                                    row["ItmDisValue"] = ItmDisValue.ToDBValue();
                                    row["ItmPriceAfter"] = ItmPriceAfter.ToDBValue();
                                    row["ItmPriceUser"] = ItmAmtShw.ToDBValue();
                                    row["ItmPrice"] = ItmPrice;
                                    row["ItmAmtShw"] = ItmAmtShw.ToDBValue();
                                    row["ItmAmtF"] = ItmAmtF;
                                    row["ItmAmtH"] = ItmAmtH;
                                    row["ItmTaxable"] = ItmTaxable;
                                    row["ItmTaxGrpKey"] = ItmTaxGrpKey.ToDBValue();
                                    row["ItmTaxGrpRate"] = ItmTaxGrpRate;
                                    row["ItmTaxGrpAmtF"] = ItmTaxGrpAmtF;
                                    row["ItmTaxGrpAmtL"] = ItmTaxGrpAmtL;
                                    #endregion
                                }

                                #region Calculate running total: CurItmAmt,DocAddCostChargesH,TotalTaxAmtF,TotalTaxAmtL,DetTotalAmtF,TotalST,TotalTT,TotalCF
                                CurItmAmt = ItmAmtShw;
                                DocAddCostChargesH = DocAddCostChargesH + ItmAmtH;
                                TotalTaxAmtF = TotalTaxAmtF + ItmTaxGrpAmtF;
                                TotalTaxAmtL = TotalTaxAmtL + ItmTaxGrpAmtL;
                                DetTotalAmtF = DetTotalAmtF + ItmAmtF;
                                if (ItmTaxGrpAmtF != 0)
                                    TotalTaxableAmtF = TotalTaxableAmtF + ItmAmtF;

                                TotalCF = TotalCF + CurItmAmt;
                                if (ResetTotal)
                                {
                                    TotalST = CurItmAmt;
                                }
                                else
                                {
                                    TotalST += CurItmAmt;
                                }
                                ResetTotal = false;
                                #endregion

                                if (RunCheck == false)
                                {
                                    #region Update LatestCost and Control Price
                                    row["ItmLatestCostF"] = GFunc.RndDC(GFunc.NEDec(row["ItmLatestCostH"], 0), DocCurrRate, GVar.RndDecs.Prcpt);
                                    #endregion
                                }

                                break;

                            case (int)GEnum.INTypeGrp.Discount:

                                #region Calculate ItmAmtShw
                                if (ItmQty > 0)
                                {
                                    //Calculate Percentage of Discount/Charges
                                    ItmAmtShw = -GFunc.RndC(Math.Abs(PreItmAmt.Value) * GFunc.NEDec(row["ItmQty"], 0), GVar.RndDecs.Amtpt);
                                }
                                else
                                {
                                    //Use ItmAmtshw
                                    ItmAmtShw = GFunc.RndC(GFunc.NEDec(row["ItmAmtShw"], 0), GVar.RndDecs.Amtpt);
                                }
                                #endregion

                                #region Calculate ItmAmtF, ItmAmtH
                                if (objDoc.DocType == 110)   //if Tax Inclusive,
                                {
                                    if (ItmTaxGrpRate > 0)
                                        if (DocTaxRate == 0)
                                            ItmAmtF = ItmAmtShw;
                                        else
                                            ItmAmtF = GFunc.RndDC(ItmAmtShw, (1 + ItmTaxGrpRate), GVar.RndDecs.Amtpt);
                                    else
                                        ItmAmtF = ItmAmtShw;
                                }
                                else
                                {
                                    ItmAmtF = ItmAmtShw;
                                }
                                ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                                #endregion

                                #region Calculate ItmTaxGrpKey, ItmTaxGrpAmtF, ItmTaxGrpAmtH, ItmTaxable
                                if (DocTaxRate == 0 && ItmTaxGrpRate > 0)
                                {
                                    //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                                    ItmTaxGrpKey = DocTaxKey;
                                    ItmTaxGrpRate = 0;
                                    ItmTaxGrpAmtF = 0;
                                    ItmTaxGrpAmtL = 0;
                                    ItmTaxable = false;
                                }
                                else
                                {
                                    //when itmtaxrate > 0 the ItmTaxGrpKey must be the same as the DocTaxGrpkey
                                    if ((ItmTaxGrpRate > 0 && DocTaxKey != ItmTaxGrpKey) == true)
                                        ItmTaxGrpKey = DocTaxKey;

                                    if (ItmTaxGrpRate > 0)
                                    {
                                        if (CalTax)
                                        {
                                            ItmTaxGrpAmtF = GFunc.RndC(ItmAmtF * ItmTaxGrpRate, GVar.RndDecs.Amtpt);
                                            ItmTaxGrpAmtL = GFunc.RndC((ItmAmtF * ItmTaxGrpRate) * DocCountryRate, GVar.RndDecs.Amtpt);//Check Mic
                                        }
                                        else//set current value
                                        {
                                            ItmTaxGrpAmtF = GFunc.NEDec(row["ItmTaxGrpAmtF"], 0);
                                            ItmTaxGrpAmtL = GFunc.NEDec(row["ItmTaxGrpAmtL"], 0);
                                        }
                                        ItmTaxable = true;
                                    }
                                    else
                                    {
                                        ItmTaxGrpAmtF = 0;
                                        ItmTaxGrpAmtL = 0;
                                        ItmTaxable = false;
                                    }
                                }
                                #endregion

                                if (RunCheck)
                                {
                                    #region Checking
                                    if (!GFunc.IsNE(row["ItmListPrice"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmPriceBefore"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmPriceAfter"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmDisPercent"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmDisValue"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmPriceUser"]))
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtShw"], 0) != ItmAmtShw)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != ItmAmtF)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != ItmAmtH)
                                        FailCheck = true;
                                    if ((bool)row["ItmTaxable"] != ItmTaxable)
                                        FailCheck = true;
                                    if (GFunc.NEInt(row["ItmTaxGrpKey"], 0) != ItmTaxGrpKey)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpRate"], 0) != ItmTaxGrpRate)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtF"], 0) != ItmTaxGrpAmtF)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtL"], 0) != ItmTaxGrpAmtL)
                                        FailCheck = true;

                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Set Values to grid
                                    row["ItmListPrice"] = DBNull.Value;
                                    row["ItmPriceBefore"] = DBNull.Value;
                                    row["ItmPriceAfter"] = DBNull.Value;
                                    row["ItmDisPercent"] = DBNull.Value;
                                    row["ItmDisValue"] = DBNull.Value;
                                    row["ItmPriceUser"] = DBNull.Value;
                                    row["ItmAmtShw"] = ItmAmtShw;
                                    row["ItmPrice"] = 0;
                                    row["ItmAmtF"] = ItmAmtF;
                                    row["ItmAmtH"] = ItmAmtH;
                                    row["ItmTaxable"] = ItmTaxable;
                                    row["ItmTaxGrpKey"] = ItmTaxGrpKey;
                                    row["ItmTaxGrpRate"] = ItmTaxGrpRate;
                                    row["ItmTaxGrpAmtF"] = ItmTaxGrpAmtF;
                                    row["ItmTaxGrpAmtL"] = ItmTaxGrpAmtL;
                                    #endregion
                                }

                                #region Calculate running total: CurItmAmt,TotalTaxAmtF,TotalTaxAmtL,DetTotalAmtF,TotalST,TotalTT,TotalCF
                                CurItmAmt = ItmAmtShw;
                                TotalTaxAmtF = TotalTaxAmtF + ItmTaxGrpAmtF;
                                TotalTaxAmtL = TotalTaxAmtL + ItmTaxGrpAmtL;
                                DetTotalAmtF = DetTotalAmtF + ItmAmtF;
                                if (ItmTaxGrpAmtF != 0)
                                    TotalTaxableAmtF = TotalTaxableAmtF + ItmAmtF;

                                TotalCF = TotalCF + CurItmAmt;
                                if (ResetTotal)
                                {
                                    TotalST = CurItmAmt;
                                }
                                else
                                {
                                    TotalST += CurItmAmt;
                                }
                                ResetTotal = false;
                                #endregion

                                if (RunCheck == false)
                                {
                                    #region Update LatestCost and Control Price
                                    row["ItmLatestCostF"] = 0;
                                    #endregion
                                }
                                break;

                            case (int)GEnum.INTypeGrp.Total:

                                #region Calculate CurItmAmt
                                if (GFunc.NEInt(row["ItmType"], 0) == (int)GEnum.ItemType.Sub_Total)
                                {
                                    CurItmAmt = TotalST;
                                }
                                else if (GFunc.NEInt(row["ItmType"], 0) == (int)GEnum.ItemType.BF_Total)// if Item type is BF_Total 
                                {
                                    CurItmAmt = TotalCF;
                                }
                                else
                                {
                                    CurItmAmt = TotalPrevious + TotalST;
                                }
                                TotalPrevious = CurItmAmt;
                                ResetTotal = true;
                                #endregion

                                if (RunCheck)
                                {
                                    #region Checking
                                    if (!GFunc.IsNE(row["ItmListPrice"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmPriceBefore"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmPriceAfter"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmDisPercent"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmDisValue"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmPriceUser"]))
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtShw"], 0) != CurItmAmt)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != 0)
                                        FailCheck = true;
                                    if ((bool)row["ItmTaxable"] != false)
                                        FailCheck = true;
                                    if (GFunc.NEInt(row["ItmTaxGrpKey"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpRate"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtF"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtL"], 0) != 0)
                                        FailCheck = true;

                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Update ItmListPrice,ItmPriceBefore,ItmVendorPrice,ItmVendorPriceRatio,ItmPriceAfter,ItmDisPercent,ItmDisValue,ItmPriceUser,ItmAmtShw
                                    row["ItmListPrice"] = DBNull.Value;
                                    row["ItmPriceBefore"] = DBNull.Value;
                                    row["ItmPriceAfter"] = DBNull.Value;
                                    row["ItmDisPercent"] = DBNull.Value;
                                    row["ItmDisValue"] = DBNull.Value;
                                    row["ItmPriceUser"] = DBNull.Value;
                                    row["ItmPrice"] = 0;
                                    row["ItmAmtShw"] = CurItmAmt;
                                    row["ItmAmtF"] = 0;
                                    row["ItmAmtH"] = 0;
                                    row["ItmTaxable"] = false;
                                    row["ItmTaxGrpKey"] = 0;
                                    row["ItmTaxGrpRate"] = 0;
                                    row["ItmTaxGrpAmtF"] = 0;
                                    row["ItmTaxGrpAmtL"] = 0;
                                    #endregion
                                }
                                break;

                            default:

                                #region Assume Header,Remark - Update ItmPrice, ItmAmtF, ItmAmtH
                                if (RunCheck)
                                {
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtF"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmTaxGrpAmtL"], 0) != 0)
                                        FailCheck = true;

                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                }
                                else
                                {
                                    row["ItmPrice"] = 0;
                                    row["ItmAmtF"] = 0;
                                    row["ItmAmtH"] = 0;
                                    row["ItmTaxGrpAmtF"] = 0;
                                    row["ItmTaxGrpAmtL"] = 0;
                                }
                                #endregion
                                break;
                        }

                        if (RunCheck == false)
                        {
                            #region Reassign Item SN numbering
                            SN = SN + 1;
                            row["ItmSN"] = SN;
                            #endregion
                        }
                        PreItmAmt = CurItmAmt;
                    }
                    #endregion

                    if (RunCheck == false)
                    {
                        #region Assign SN to all detail assembly and batch

                        dtParentFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("LineType") == 1000 && r.Field<int>("LineLinkKey") == 0 && r.Field<int>("ItmType") == 250);
                        foreach (DataRow rowParent in dtParentFilter)
                        {
                            dtItemFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("LineType") >= 1000
                                && r.Field<int>("LineLinkKey") == GFunc.NEInt(rowParent["DocItmKey"], -1)).OrderBy(p => p.Field<decimal>("ItmDetSN"));
                            int childSN = 1;
                            foreach (DataRow rowChild in dtItemFilter)
                            {
                                rowChild["ItmSN"] = rowParent["ItmSN"];
                                rowChild["ItmDetSN"] = childSN;
                                childSN++;
                            }
                        }
                        #endregion
                    }
                }

                if (RunCheck)
                {
                    #region Check Document Total

                    TotalTaxAmtF = GFunc.RndC(TotalTaxAmtF, GVar.RndDecs.Amtpt);
                    TotalTaxAmtL = GFunc.RndC(TotalTaxAmtL, GVar.RndDecs.Amtpt);
                    DocOverallDisAmt = GFunc.RndDC(DetTotalAmtF * DocDisRate, 100M, GVar.RndDecs.Amtpt);
                    DocTotalAfterDis = DetTotalAmtF - DocOverallDisAmt;
                    DocTaxAmt = GFunc.GetDecimalPropertyValue("DocTaxTotal", objDoc).Value;
                    DocTaxAmtL = GFunc.GetDecimalPropertyValue("DocTaxTotalLocal", objDoc).Value;

                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocSubTotal", objDoc), 0) != DetTotalAmtF)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocOverallDisAmt", objDoc), 0) != DocOverallDisAmt)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocTotalAfterDis", objDoc), 0) != DocTotalAfterDis)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocTaxTotal", objDoc), 0) != DocTaxAmt)
                        FailCheck = true;

                    if (GFunc.IsNE(GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc)))
                    {
                        DocTotal = (decimal)DocTotalAfterDis + (decimal)DocTaxAmt;
                    }
                    else
                    {
                       // REFTaxGrp objTaxGrp = REFTaxGrp.Get(cn, GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc));
                        //Modified by May, moved to the declaration section 
                        if (objTaxGrp.GSTCustom == true)
                            DocTotal = (decimal)DocTotalAfterDis;
                        else
                            DocTotal = (decimal)DocTotalAfterDis + (decimal)DocTaxAmt;
                    }

                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocTaxTotalLocal", objDoc), 0) != DocTaxAmtL)
                        FailCheck = true;

                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Purchase_Order:
                            if (GFunc.NEDec(GFunc.GetPropertyValue("DocGrand", objDoc), 0) != DocTotal)
                                FailCheck = true;
                            break;

                        case (int)GEnum.SystemCode.Purchase_Invoice:
                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            if (GFunc.NEDec(GFunc.GetPropertyValue("DocTotal", objDoc), 0) != DocTotal)
                                FailCheck = true;

                            if (GFunc.NEDec(GFunc.GetPropertyValue("DocGrand", objDoc), 0) != DocTotal - GFunc.GetDecimalPropertyValue("DocPaidAmtF", objDoc))
                                FailCheck = true;
                            break;
                    }
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocHome", objDoc), 0) != GFunc.RndC(GFunc.GetDecimalPropertyValue("DocGrand", objDoc) * DocCurrRate, GVar.RndDecs.Amtpt))
                        FailCheck = true;

                    if (FailCheck)
                    {
                        MsgBox.Show(cn, MsgID.Document.SaveFailTotalCalculationWrong);
                        return false;
                    }
                    #endregion

                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Purchase_Invoice:
                        case (int)GEnum.SystemCode.Purchase_Debit_Note:

                            #region Calculate Additional Cost
                            DocAddFreightF = GFunc.NEDec(GFunc.GetPropertyValue("DocAddFreight", objDoc), 0);
                            DocAddInsuranceF = GFunc.NEDec(GFunc.GetPropertyValue("DocAddInsurance", objDoc), 0);
                            DocAddOthersF = GFunc.NEDec(GFunc.GetPropertyValue("DocAddOthers", objDoc), 0);
                            DocAddCostLumpSumRate = GFunc.NEDec(GFunc.GetPropertyValue("DocAddCostLumpSumRate", objDoc), 0);
                            DocAddCostDocHomePercent = GFunc.RndDC(GFunc.GetPropertyValue("DocAddCostDocHomePercent", objDoc), 100, GVar.RndDecs.Prcpt);

                            DocAddCostLumpSumF = DocAddFreightF + DocAddInsuranceF + DocAddOthersF;
                            DocAddCostOtherH = GFunc.RndC(DocAddCostLumpSumF * DocAddCostLumpSumRate, GVar.RndDecs.Amtpt);
                            DocAddCostOtherH = DocAddCostOtherH + GFunc.RndC(DocAddCostDocHomePercent * GFunc.NEDec(GFunc.GetPropertyValue("DocTotalAfterDis", objDoc), 0) * DocCurrRate, GVar.RndDecs.Amtpt);
                            DocAddCostTotalH = DocAddCostOtherH + DocAddCostChargesH;
                            if (DocAddCostItmAmtF == 0)
                                DocAddCostFactor = 0;
                            else
                                DocAddCostFactor = GFunc.RndDC(DocAddCostTotalH, DocAddCostItmAmtF, GVar.RndDecs.Prcpt);
                            #endregion

                            #region looping to all detail items to check additional cost
                            dtItemFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == 0).OrderBy(r => r.Field<decimal>("ItmSN"));
                            foreach (DataRow row in dtItemFilter)
                            {
                                switch (GFunc.GetINTypeGroup(GFunc.NEInt(row["ItmType"], 0)))
                                {
                                    case (int)GEnum.INTypeGrp.Stock:
                                    case (int)GEnum.INTypeGrp.Non_Stock:
                                        //Calculation
                                        ItmAddAmtH = GFunc.RndDC(GFunc.NEDec(row["ItmAmtF"], 0), DocAddCostItmAmtF, GVar.RndDecs.Curpt);
                                        ItmAddAmtH = GFunc.RndC(ItmAddAmtH * DocAddCostTotalH, GVar.RndDecs.Amtpt);
                                        ItmAddAmtF = GFunc.RndDC(ItmAddAmtH, DocCurrRate, GVar.RndDecs.Amtpt);
                                        ItmAddCostH = GFunc.RndDC(ItmAddAmtH, GFunc.NEDec(row["ItmQty"], 0), GVar.RndDecs.Prcpt);
                                        ItmAddCostF = GFunc.RndDC(ItmAddAmtF, GFunc.NEDec(row["ItmQty"], 0), GVar.RndDecs.Prcpt);

                                        //Set values to grid
                                        if (GFunc.NEDec(row["ItmAddAmtF"], 0) != ItmAddAmtF)
                                            FailCheck = true;
                                        if (GFunc.NEDec(row["ItmAddAmtH"], 0) != ItmAddAmtH)
                                            FailCheck = true;
                                        if (GFunc.NEDec(row["ItmAddCostF"], 0) != ItmAddCostF)
                                            FailCheck = true;
                                        if (GFunc.NEDec(row["ItmAddCostH"], 0) != ItmAddCostH)
                                            FailCheck = true;

                                        if (FailCheck)
                                        {
                                            MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                            return false;
                                        }
                                        break;
                                }
                            }
                            #endregion

                            #region check values in document 
                            if (SysOptionUtility.GetInt("LatestCostOption", cn) != 10 || SysOptionUtility.GetInt("LandedCostOption", cn) != 10)
                            {
                                if (GFunc.NEDec(GFunc.GetPropertyValue("DocAddCostLumpSum", objDoc), 0) != DocAddCostLumpSumF)
                                    FailCheck = true;
                                if (GFunc.NEDec(GFunc.GetPropertyValue("DocAddCostOthersH", objDoc), 0) != DocAddCostOtherH)
                                    FailCheck = true;
                                if (GFunc.NEDec(GFunc.GetPropertyValue("DocAddCostChargesH", objDoc), 0) != DocAddCostChargesH)
                                    FailCheck = true;
                                if (GFunc.NEDec(GFunc.GetPropertyValue("DocAddCostTotalH", objDoc), 0) != DocAddCostTotalH)
                                    FailCheck = true;
                                if (GFunc.NEDec(GFunc.GetPropertyValue("DocAddCostItmAmtF", objDoc), 0) != DocAddCostItmAmtF)
                                    FailCheck = true;
                                if (GFunc.NEDec(GFunc.GetPropertyValue("DocAddCostFactor", objDoc), 0) != DocAddCostFactor)
                                    FailCheck = true;

                                if (FailCheck)
                                {
                                    MsgBox.Show(cn, MsgID.Document.SaveFailTotalCalculationWrong);
                                    return false;
                                }
                            }
                            
                            #endregion

                            break;
                    }
                }
                else
                {
                    #region Calculate Document Total
                    TotalTaxAmtF = GFunc.RndC(TotalTaxAmtF, GVar.RndDecs.Amtpt);
                    TotalTaxAmtL = GFunc.RndC(TotalTaxAmtL, GVar.RndDecs.Amtpt);
                    DocOverallDisAmt = GFunc.RndDC(DetTotalAmtF * DocDisRate, 100M, GVar.RndDecs.Amtpt);
                    DocTotalAfterDis = DetTotalAmtF - DocOverallDisAmt;

                    if (CalTax)
                    {
                        if (SysOptionUtility.TaxCalculationOnTotal)
                        {
                            TotalTaxableAmtF = TotalTaxableAmtF - GFunc.RndDC(TotalTaxableAmtF * DocDisRate, 100M, GVar.RndDecs.Amtpt);
                            DocTaxAmt = GFunc.RndC(TotalTaxableAmtF * DocTaxRate, GVar.RndDecs.Amtpt);
                            DocTaxAmtL = GFunc.RndC(TotalTaxableAmtF * DocTaxRate * DocCountryRate, GVar.RndDecs.Amtpt);
                        }
                        else
                        {
                            DocTaxAmt = TotalTaxAmtF - GFunc.RndDC(TotalTaxAmtF * DocDisRate, 100M, GVar.RndDecs.Amtpt);
                            DocTaxAmtL = TotalTaxAmtL - GFunc.RndDC(TotalTaxAmtL * DocDisRate, 100M, GVar.RndDecs.Amtpt);
                        }
                    }
                    else
                    {
                        DocTaxAmt = GFunc.GetDecimalPropertyValue("DocTaxTotal", objDoc).Value;
                        DocTaxAmtL = GFunc.GetDecimalPropertyValue("DocTaxTotalLocal", objDoc).Value;
                    }

                    GFunc.SetPropertyValue("DocSubTotal", objDoc, DetTotalAmtF);
                    GFunc.SetPropertyValue("DocOverallDisAmt", objDoc, DocOverallDisAmt);
                    GFunc.SetPropertyValue("DocTotalAfterDis", objDoc, DocTotalAfterDis);
                    GFunc.SetPropertyValue("DocTaxTotal", objDoc, DocTaxAmt);

                    if (GFunc.IsNE(GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc)))
                    {
                        DocTotal = (decimal)DocTotalAfterDis + (decimal)DocTaxAmt;
                    }
                    else
                    {
                        //REFTaxGrp objTaxGrp = REFTaxGrp.Get(cn, GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc));
                        //Modified by May, moved to the declaration section 
                        if (objTaxGrp.GSTCustom == true)
                            DocTotal = (decimal)DocTotalAfterDis;
                        else
                            DocTotal = (decimal)DocTotalAfterDis + (decimal)DocTaxAmt;
                    }

                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Purchase_Order:
                            GFunc.SetPropertyValue("DocTaxTotal", objDoc, DocTaxAmt);
                            GFunc.SetPropertyValue("DocGrand", objDoc, DocTotal);
                            break;

                        case (int)GEnum.SystemCode.Purchase_Invoice:
                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            GFunc.SetPropertyValue("DocTaxTotal", objDoc, DocTaxAmt);
                            GFunc.SetPropertyValue("DocTotal", objDoc, DocTotal);
                            GFunc.SetPropertyValue("DocGrand", objDoc, DocTotal - GFunc.GetDecimalPropertyValue("DocPaidAmtF", objDoc));
                            break;
                    }

                    GFunc.SetPropertyValue("DocHome", objDoc, GFunc.RndC(GFunc.GetDecimalPropertyValue("DocGrand", objDoc) * DocCurrRate, GVar.RndDecs.Amtpt));
                    GFunc.SetPropertyValue("DocTaxTotalLocal", objDoc, DocTaxAmtL);
                    #endregion

                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Purchase_Invoice:
                        case (int)GEnum.SystemCode.Purchase_Debit_Note:

                            #region Calculate Additional Cost
                            DocAddFreightF = GFunc.NEDec(GFunc.GetPropertyValue("DocAddFreight", objDoc), 0);
                            DocAddInsuranceF = GFunc.NEDec(GFunc.GetPropertyValue("DocAddInsurance", objDoc), 0);
                            DocAddOthersF = GFunc.NEDec(GFunc.GetPropertyValue("DocAddOthers", objDoc), 0);
                            DocAddCostLumpSumRate = GFunc.NEDec(GFunc.GetPropertyValue("DocAddCostLumpSumRate", objDoc), 0);
                            DocAddCostDocHomePercent = GFunc.RndDC(GFunc.GetPropertyValue("DocAddCostDocHomePercent", objDoc), 100, GVar.RndDecs.Prcpt);

                            DocAddCostLumpSumF = DocAddFreightF + DocAddInsuranceF + DocAddOthersF;
                            DocAddCostOtherH = GFunc.RndC(DocAddCostLumpSumF * DocAddCostLumpSumRate, GVar.RndDecs.Amtpt);
                            DocAddCostOtherH = DocAddCostOtherH + GFunc.RndC(DocAddCostDocHomePercent * GFunc.NEDec(GFunc.GetPropertyValue("DocTotalAfterDis", objDoc), 0) * DocCurrRate, GVar.RndDecs.Amtpt);
                            DocAddCostTotalH = DocAddCostOtherH + DocAddCostChargesH;
                            if (DocAddCostItmAmtF == 0)
                                DocAddCostFactor = 0;
                            else
                                DocAddCostFactor = GFunc.RndDC(DocAddCostTotalH, DocAddCostItmAmtF, GVar.RndDecs.Prcpt);
                            #endregion

                            #region looping to all detail items to set additional cost
                            dtItemFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == 0).OrderBy(r => r.Field<decimal>("ItmSN"));
                            foreach (DataRow row in dtItemFilter)
                            {
                                switch (GFunc.GetINTypeGroup(GFunc.NEInt(row["ItmType"], 0)))
                                {
                                    case (int)GEnum.INTypeGrp.Stock:
                                    case (int)GEnum.INTypeGrp.Non_Stock:
                                        //Calculation
                                        ItmAddAmtH = GFunc.RndDC(GFunc.NEDec(row["ItmAmtF"], 0), DocAddCostItmAmtF, GVar.RndDecs.Curpt);
                                        ItmAddAmtH = GFunc.RndC(ItmAddAmtH * DocAddCostTotalH, GVar.RndDecs.Amtpt);
                                        ItmAddAmtF = GFunc.RndDC(ItmAddAmtH, DocCurrRate, GVar.RndDecs.Amtpt);
                                        ItmAddCostH = GFunc.RndDC(ItmAddAmtH, GFunc.NEDec(row["ItmQty"], 0), GVar.RndDecs.Prcpt);
                                        ItmAddCostF = GFunc.RndDC(ItmAddAmtF, GFunc.NEDec(row["ItmQty"], 0), GVar.RndDecs.Prcpt);

                                        //Set values to grid
                                        row["ItmAddAmtF"] = ItmAddAmtF;
                                        row["ItmAddAmtH"] = ItmAddAmtH;
                                        row["ItmAddCostF"] = ItmAddCostF;
                                        row["ItmAddCostH"] = ItmAddCostH;
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

                             #region calculating additional discount for all detail if there is.. Added by May on 21 Dec 2018
                            DataRow[] drs = dtItems.Select("ItmType=710","ItmSN ASC");
                            decimal prevSN = 0;
                            foreach (DataRow drDis in drs)
                            {          
                                decimal disSN=GFunc.NEDec(drDis["ItmSN"],0);
                                dtItemFilter= dtItems.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == 0 && r.Field<int>("ItmType") != 800 && r.Field<int>("ItmType") != 810
                                    && r.Field<decimal>("ItmSN") <= disSN && r.Field<decimal>("ItmSN") > prevSN).OrderBy(r => r.Field<decimal>("ItmSN"));

                                decimal? sum =GFunc.NEDec(dtItems.Compute("Sum(ItmAmtH)", "(ItmType=100 or ItmType=600) and ItmSN<" + disSN + " and ItmSN>" + prevSN),0);                        
                             
                                foreach (DataRow row in dtItemFilter)
                                {
                                    switch (GFunc.GetINTypeGroup(GFunc.NEInt(row["ItmType"], 0)))
                                    {
                                        case (int)GEnum.INTypeGrp.Stock:
                                        case (int)GEnum.INTypeGrp.Non_Stock:                                          
                                           
                                            DataRow[] drTot = dtItems.Select("ItmType in(820,825,830) and ItmSN<" + disSN + " and ItmSN>"+prevSN, "ItmSN DESC");
                                            if (drTot.Count() > 0 || sum>0 )
                                            {
                                                if (drTot.Count() > 0)
                                                    sum = GFunc.NEDec(drTot[0]["ItmAmtShw"], 0) * DocCurrRate;
                                                decimal ItmAddDisAmtH = GFunc.RndDC(GFunc.NEDec(row["ItmAmtH"], 0), sum, GVar.RndDecs.Curpt);
                                                ItmAddDisAmtH = GFunc.RndC(ItmAddDisAmtH * GFunc.NEDec(drDis["ItmAmtH"], 0) * -1, GVar.RndDecs.Amtpt);
                                                row["ItmAddDisAmtH"] = ItmAddDisAmtH;
                                                row["ItmAddDisAmtF"] = GFunc.RndDC(ItmAddDisAmtH, DocCurrRate, GVar.RndDecs.Amtpt);
                                                row["ItmAddDisH"] = GFunc.RndDC(ItmAddDisAmtH, GFunc.NEDec(row["ItmQty"], 0), GVar.RndDecs.Prcpt);
                                                row["ItmAddDisF"] = GFunc.RndDC(GFunc.NEDec(row["ItmAddDisAmtF"],0), GFunc.NEDec(row["ItmQty"], 0), GVar.RndDecs.Prcpt);
                                            }                                               
                                            else
                                            {
                                                row["ItmAddDisAmtH"] = 0;
                                                row["ItmAddDisAmtF"] = 0;
                                                row["ItmAddDisH"] = 0;
                                                row["ItmAddDisF"] = 0;
                                            }                                         
                                       
                                            break;
                                    }                                    
                                }
                                prevSN = disSN;
                            }
                            #endregion

                            break;
                    }
                }
                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        private static bool CalDocAPPD(SqlConnection cn, Document objDoc, DataTable dtItems, bool CalTax, bool RunCheck)
        {
            #region variable
            IEnumerable<DataRow> dtItemFilter = null;
            IEnumerable<DataRow> dtParentFilter = null;
            bool FailCheck = false;
            decimal? PreItmAmt = 0;
            decimal? CurItmAmt = 0;
            decimal? TotalPrevious = 0; //Previous Total line value - use for calculation of Total Line
            decimal? TotalST = 0;       //Sub Total
            decimal? TotalCF = 0;       //CF Total           
            decimal? DetTotalAmtF = 0;  //Total of all detail items w/o gst
            bool ResetTotal = false;

            DateTime DocDate;
            int? DocConKey = 0;
            int? DocCurrKey = 1;
            decimal? DocCurrRate = 1;
            decimal? DocCountryRate = 1;
            decimal? DocDisRate = 0;
            decimal? DocTotalAfterDis = 0;
            decimal? DocOverallDisAmt = 0;

            decimal? ItmQty = 0;
            decimal? ItmPriceAfter = 0;
            decimal? ItmDisRate = 0;
            decimal? ItmDisValue = 0;
            decimal? ItmPriceUser = 0;
            decimal? ItmPrice = 0;
            decimal? ItmAmtShw = 0;
            decimal? ItmAmtF = 0;
            decimal? ItmAmtH = 0;

            string OpValue = string.Empty;
            decimal? SN = 0;
            #endregion

            try
            {
                #region Assign Variables
                dtItemFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == 0).OrderBy(r => r.Field<decimal>("ItmSN"));

                DocDate = (DateTime)objDoc.DocDate;
                DocConKey = GFunc.NEInt(GFunc.GetPropertyValue("DocConKey", objDoc), 0);
                DocCurrKey = GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 0);
                DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                DocDisRate = GFunc.NEDec(GFunc.GetPropertyValue("DocOverallDisRate", objDoc), 0);
                if (SysOptionUtility.GetInt("CountryCurrency", cn) == 1) // Country Currency = Home -- Added by Jane. Mic to check
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                else
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);
                #endregion

                if (dtItems.Rows.Count > 0)
                {
                    #region looping to all detail items to calculate each row
                    foreach (DataRow row in dtItemFilter)
                    {
                        #region reset variables
                        ItmQty = 0;
                        ItmPriceAfter = 0;
                        ItmDisRate = 0;
                        ItmDisValue = 0;
                        ItmPriceUser = 0;
                        ItmPrice = 0;
                        ItmAmtShw = 0;
                        ItmAmtF = 0;
                        ItmAmtH = 0;

                        #endregion

                        #region Set variables
                        ItmQty = GFunc.NEDec(row["ItmQty"], 0);
                        ItmPriceAfter = GFunc.NEDec(row["ItmPriceAfter"], 0);
                        ItmPriceUser = GFunc.NEDec(row["ItmPriceUser"], 0);

                        #endregion

                        switch (GFunc.GetINTypeGroup(GFunc.NEInt(row["ItmType"], 0)))
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:

                                #region Calculate ItmPrice, ItmAmtShw, ItmAmtF, ItmAmtH
                                ItmPrice = ItmPriceUser;
                                ItmAmtShw = GFunc.RndC(ItmPriceUser * ItmQty, GVar.RndDecs.Amtpt);
                                ItmAmtF = GFunc.RndC(ItmPrice * ItmQty, GVar.RndDecs.Amtpt);
                                ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                                #endregion

                                if (RunCheck)
                                {
                                    #region Checking
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != ItmPrice)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtShw"], 0) != ItmAmtShw)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != ItmAmtF)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != ItmAmtH)
                                        FailCheck = true;

                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                    #endregion
                                }

                                else
                                {
                                    #region Set value to grid
                                    row["ItmPrice"] = ItmPrice;
                                    row["ItmAmtShw"] = ItmAmtShw;
                                    row["ItmAmtF"] = ItmAmtF;
                                    row["ItmAmtH"] = ItmAmtH;
                                    #endregion
                                }

                                #region Calculate running total: CurItmAmt,DocAddCostItmAmtF,TotalTaxAmtF,TotalTaxAmtL,DetTotalAmtF,TotalST,TotalTT,TotalCF
                                CurItmAmt = ItmAmtShw;
                                DetTotalAmtF = DetTotalAmtF + ItmAmtF;
                                TotalCF = TotalCF + CurItmAmt;
                                if (ResetTotal)
                                {
                                    TotalST = CurItmAmt;
                                }
                                else
                                {
                                    TotalST += CurItmAmt;
                                }
                                ResetTotal = false;
                                #endregion

                                if (RunCheck == false)
                                {
                                    #region Update LatestCost
                                    row["ItmLatestCostF"] = GFunc.RndDC(GFunc.NEDec(row["ItmLatestCostH"], 0) * GFunc.NEDec(row["ItmConRate"], 0), DocCurrRate, GVar.RndDecs.Prcpt);
                                    #endregion
                                }
                                break;

                            case (int)GEnum.INTypeGrp.Charges:

                                #region Calculate ItmDisPercent,ItmDisValue,ItmPriceUser,ItmAmtShw
                                if (ItmQty > 0)
                                {
                                    //Calculate Percentage of Discount/Charges
                                    ItmPriceAfter = GFunc.RndC(Math.Abs(PreItmAmt.Value) * ItmQty, GVar.RndDecs.Prcpt);
                                }
                                else
                                {
                                    //Use ItmAmtshw
                                    ItmPriceAfter = GFunc.NEDec(row["ItmPriceAfter"], 0);
                                }
                                ItmDisRate = GFunc.NEDec(row["ItmDisPercent"], 0);
                                ItmDisValue = ItmPriceAfter * ItmDisRate / 100M;
                                ItmPriceUser = ItmPriceAfter - ItmDisValue;
                                ItmAmtShw = GFunc.RndC(ItmPriceUser, GVar.RndDecs.Amtpt);
                                #endregion

                                #region Calculate ItmPrice, ItmAmtF, ItmAmtH
                                ItmPrice = ItmAmtShw;
                                ItmAmtF = GFunc.RndC(ItmPrice, GVar.RndDecs.Amtpt);
                                ItmAmtH = GFunc.RndC(ItmPrice * DocCurrRate, GVar.RndDecs.Amtpt);
                                #endregion

                                if (RunCheck)
                                {
                                    #region Checking
                                    if (GFunc.NEDec(row["ItmDisValue"], 0) != ItmDisValue)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmPriceUser"], 0) != ItmAmtShw)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != ItmPrice)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtShw"], 0) != ItmAmtShw)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != ItmAmtF)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != ItmAmtH)
                                        FailCheck = true;

                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Set value to grid
                                    row["ItmDisPercent"] = ItmDisRate.ToDBValue();
                                    row["ItmDisValue"] = ItmDisValue.ToDBValue();
                                    row["ItmPriceAfter"] = ItmPriceAfter.ToDBValue();
                                    row["ItmPriceUser"] = ItmAmtShw.ToDBValue();
                                    row["ItmPrice"] = ItmPrice;
                                    row["ItmAmtShw"] = ItmAmtShw.ToDBValue();
                                    row["ItmAmtF"] = ItmAmtF;
                                    row["ItmAmtH"] = ItmAmtH;
                                    #endregion
                                }

                                #region Calculate running total: CurItmAmt,DocAddCostChargesH,TotalTaxAmtF,TotalTaxAmtL,DetTotalAmtF,TotalST,TotalTT,TotalCF
                                CurItmAmt = ItmAmtShw;
                                DetTotalAmtF = DetTotalAmtF + ItmAmtF;
                                TotalCF = TotalCF + CurItmAmt;
                                if (ResetTotal)
                                {
                                    TotalST = CurItmAmt;
                                }
                                else
                                {
                                    TotalST += CurItmAmt;
                                }
                                ResetTotal = false;
                                #endregion

                                if (RunCheck == false)
                                {
                                    #region Update LatestCost and Control Price
                                    row["ItmLatestCostF"] = GFunc.RndDC(GFunc.NEDec(row["ItmLatestCostH"], 0), DocCurrRate, GVar.RndDecs.Prcpt);
                                    #endregion
                                }

                                break;

                            case (int)GEnum.INTypeGrp.Discount:

                                #region Calculate ItmAmtShw
                                if (ItmQty > 0)
                                {
                                    //Calculate Percentage of Discount/Charges
                                    ItmAmtShw = -GFunc.RndC(Math.Abs(PreItmAmt.Value) * GFunc.NEDec(row["ItmQty"], 0), GVar.RndDecs.Amtpt);
                                }
                                else
                                {
                                    //Use ItmAmtshw
                                    ItmAmtShw = GFunc.RndC(GFunc.NEDec(row["ItmAmtShw"], 0), GVar.RndDecs.Amtpt);
                                }
                                #endregion

                                #region Calculate ItmAmtF, ItmAmtH
                                ItmAmtF = ItmAmtShw;
                                ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                                #endregion

                                if (RunCheck)
                                {
                                    #region Checking
                                    if (!GFunc.IsNE(row["ItmListPrice"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmPriceBefore"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmPriceAfter"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmDisPercent"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmDisValue"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmPriceUser"]))
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtShw"], 0) != ItmAmtShw)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != ItmAmtF)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != ItmAmtH)
                                        FailCheck = true;

                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Set Values to grid
                                    row["ItmListPrice"] = DBNull.Value;
                                    row["ItmPriceBefore"] = DBNull.Value;
                                    row["ItmPriceAfter"] = DBNull.Value;
                                    row["ItmDisPercent"] = DBNull.Value;
                                    row["ItmDisValue"] = DBNull.Value;
                                    row["ItmPriceUser"] = DBNull.Value;
                                    row["ItmAmtShw"] = ItmAmtShw;
                                    row["ItmPrice"] = 0;
                                    row["ItmAmtF"] = ItmAmtF;
                                    row["ItmAmtH"] = ItmAmtH;
                                    #endregion
                                }

                                #region Calculate running total: CurItmAmt,TotalTaxAmtF,TotalTaxAmtL,DetTotalAmtF,TotalST,TotalTT,TotalCF
                                CurItmAmt = ItmAmtShw;
                                DetTotalAmtF = DetTotalAmtF + ItmAmtF;
                                TotalCF = TotalCF + CurItmAmt;
                                if (ResetTotal)
                                {
                                    TotalST = CurItmAmt;
                                }
                                else
                                {
                                    TotalST += CurItmAmt;
                                }
                                ResetTotal = false;
                                #endregion

                                if (RunCheck == false)
                                {
                                    #region Update LatestCost and Control Price
                                    row["ItmLatestCostF"] = 0;
                                    #endregion
                                }
                                break;

                            case (int)GEnum.INTypeGrp.Total:

                                #region Calculate CurItmAmt
                                if (GFunc.NEInt(row["ItmType"], 0) == (int)GEnum.ItemType.Sub_Total)
                                {
                                    CurItmAmt = TotalST;
                                }
                                else if (GFunc.NEInt(row["ItmType"], 0) == (int)GEnum.ItemType.BF_Total)
                                {
                                    CurItmAmt = TotalCF;
                                }
                                else
                                {
                                    CurItmAmt = TotalPrevious + TotalST;
                                }
                                TotalPrevious = CurItmAmt;
                                ResetTotal = true;
                                #endregion

                                if (RunCheck)
                                {
                                    #region Checking
                                    if (!GFunc.IsNE(row["ItmListPrice"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmPriceBefore"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmPriceAfter"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmDisPercent"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmDisValue"]))
                                        FailCheck = true;
                                    if (!GFunc.IsNE(row["ItmPriceUser"]))
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtShw"], 0) != CurItmAmt)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != 0)
                                        FailCheck = true;


                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Update ItmListPrice,ItmPriceBefore,ItmVendorPrice,ItmVendorPriceRatio,ItmPriceAfter,ItmDisPercent,ItmDisValue,ItmPriceUser,ItmAmtShw
                                    row["ItmListPrice"] = DBNull.Value;
                                    row["ItmPriceBefore"] = DBNull.Value;
                                    row["ItmPriceAfter"] = DBNull.Value;
                                    row["ItmDisPercent"] = DBNull.Value;
                                    row["ItmDisValue"] = DBNull.Value;
                                    row["ItmPriceUser"] = DBNull.Value;
                                    row["ItmPrice"] = 0;
                                    row["ItmAmtShw"] = CurItmAmt;
                                    row["ItmAmtF"] = 0;
                                    row["ItmAmtH"] = 0;
                                    #endregion
                                }
                                break;

                            default:

                                #region Assume Header,Remark - Update ItmPrice, ItmAmtF, ItmAmtH
                                if (RunCheck)
                                {
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != 0)
                                        FailCheck = true;

                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                }
                                else
                                {
                                    row["ItmPrice"] = 0;
                                    row["ItmAmtF"] = 0;
                                    row["ItmAmtH"] = 0;
                                }
                                #endregion
                                break;
                        }

                        if (RunCheck == false)
                        {
                            #region Reassign Item SN numbering
                            SN = SN + 1;
                            row["ItmSN"] = SN;
                            #endregion
                        }
                        PreItmAmt = CurItmAmt;
                    }
                    #endregion

                    if (RunCheck == false)
                    {
                        #region Assign SN to all detail assembly and batch

                        dtParentFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == 0).OrderBy(p => p.Field<decimal>("ItmSN"));

                        //to set Childs' ItmSN and ItmDetSN
                        foreach (DataRow rowParent in dtParentFilter)
                        {
                            int childSN = 1;
                            dtItemFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == GFunc.NEInt(rowParent["DocItmKey"], -1)).OrderBy(p => p.Field<decimal>("ItmDetSN"));
                            foreach (DataRow rowChild in dtItemFilter)
                            {
                                rowChild["ItmSN"] = rowParent["ItmSN"];
                                rowChild["ItmDetSN"] = childSN;
                                childSN++;
                            }
                        }
                        #endregion
                    }

                    if (RunCheck)
                    {
                        #region Check Document Total
                        DocOverallDisAmt = GFunc.RndDC(DetTotalAmtF * DocDisRate, 100M, GVar.RndDecs.Amtpt);
                        DocTotalAfterDis = DetTotalAmtF - DocOverallDisAmt;

                        if (GFunc.NEDec(GFunc.GetPropertyValue("DocSubTotal", objDoc), 0) != DetTotalAmtF)
                            FailCheck = true;
                        if (GFunc.NEDec(GFunc.GetPropertyValue("DocOverallDisAmt", objDoc), 0) != DocOverallDisAmt)
                            FailCheck = true;
                        if (GFunc.NEDec(GFunc.GetPropertyValue("DocTotalAfterDis", objDoc), 0) != DocTotalAfterDis)
                            FailCheck = true;


                        if (GFunc.NEDec(GFunc.GetPropertyValue("DocHome", objDoc), 0) != GFunc.RndC(GFunc.GetDecimalPropertyValue("DocGrand", objDoc) * DocCurrRate, GVar.RndDecs.Amtpt))
                            FailCheck = true;

                        if (FailCheck)
                        {
                            MsgBox.Show(cn, MsgID.Document.SaveFailTotalCalculationWrong);
                            return false;
                        }
                        #endregion

                    }
                    else
                    {
                        #region Calculate Document Total
                        DocOverallDisAmt = GFunc.RndDC(DetTotalAmtF * DocDisRate, 100M, GVar.RndDecs.Amtpt);
                        DocTotalAfterDis = DetTotalAmtF - DocOverallDisAmt;

                        GFunc.SetPropertyValue("DocSubTotal", objDoc, DetTotalAmtF);
                        GFunc.SetPropertyValue("DocOverallDisAmt", objDoc, DocOverallDisAmt);
                        GFunc.SetPropertyValue("DocTotalAfterDis", objDoc, DocTotalAfterDis);
                        GFunc.SetPropertyValue("DocGrand", objDoc, DocTotalAfterDis);
                        GFunc.SetPropertyValue("DocHome", objDoc, GFunc.RndC(GFunc.GetDecimalPropertyValue("DocGrand", objDoc) * DocCurrRate, GVar.RndDecs.Amtpt));

                        #endregion
                    }
                }

                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        private static bool CalDocAPRQ(SqlConnection cn, Document objDoc, DataTable dtItems, bool RunCheck)
        {
            #region variable
            decimal? SN = 0;
            #endregion

            try
            {
                if (dtItems.Rows.Count > 0)
                {
                    #region looping to all detail items to calculate each row
                    IEnumerable<DataRow> dtItemFilter = dtItems.AsEnumerable().OrderBy(r => r.Field<decimal>("ItmSN"));

                    foreach (DataRow row in dtItemFilter)
                    {

                        SN = SN + 1;
                        row["ItmSN"] = SN;

                    }
                    #endregion
                }
                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        private static bool CalDocCT(SqlConnection cn, Document objDoc, DataTable dtItems, bool RunCheck)
        {

            decimal? TotalApplyARAmtH = 0;
            decimal? TotalApplyAPAmtH = 0;
            decimal? DocGrand = 0;
            bool FailCheck = false;

            try
            {
                if (dtItems.Rows.Count > 0)
                {
                    foreach (DataRow row in dtItems.Rows)
                    {
                        switch (GFunc.NEInt(row["LinkDocDC"], 0))
                        {

                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.AP_Opening_Balance:
                            case (int)GEnum.SystemCode.Purchase_Adjustment:
                                TotalApplyAPAmtH += GFunc.NEDec(row["ItmApplyPayAmtH"], 0);
                                break;

                            case (int)GEnum.SystemCode.Sales_Invoice:
                            case (int)GEnum.SystemCode.Sales_Debit_Note:
                            case (int)GEnum.SystemCode.Sales_Credit_Note:
                            case (int)GEnum.SystemCode.Cash_Sale:
                            case (int)GEnum.SystemCode.Cash_Debit_Note:
                            case (int)GEnum.SystemCode.Cash_Credit_Note:
                            case (int)GEnum.SystemCode.AR_Opening_Balance:
                            case (int)GEnum.SystemCode.AR_Cash_Opening_Balance:
                            case (int)GEnum.SystemCode.Sales_Adjustment:
                            case (int)GEnum.SystemCode.Cash_Adjustment:
                                TotalApplyARAmtH += GFunc.NEDec(row["ItmApplyPayAmtH"], 0);
                                DocGrand += GFunc.NEDec(row["ItmApplyDocAmtF"], 0);
                                break;
                        }
                    }
                }
                if (RunCheck)
                {
                    #region Check Document Total

                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocApplyARAmtH", objDoc), 0) != TotalApplyARAmtH)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocApplyAPAmtH", objDoc), 0) != TotalApplyAPAmtH)
                        FailCheck = true;

                    if (TotalApplyAPAmtH != TotalApplyARAmtH)
                    {
                        MsgBox.Show(cn, MsgID.Document.MustEqualARnAP, GEnum.MsgBoxIcon.Alert, GEnum.MsgBoxButton.OK);
                        return false;
                    }

                    if (FailCheck)
                    {
                        MsgBox.Show(cn, MsgID.Document.SaveFailTotalCalculationWrong);
                        return false;
                    }
                    #endregion
                }
                else
                {
                    #region Calculate Document Total
                    GFunc.SetPropertyValue("DocApplyARAmtH", objDoc, TotalApplyARAmtH);
                    GFunc.SetPropertyValue("DocApplyAPAmtH", objDoc, TotalApplyAPAmtH);
                    GFunc.SetPropertyValue("DocHome", objDoc, TotalApplyARAmtH);
                    GFunc.SetPropertyValue("DocGrand", objDoc, DocGrand);
                    #endregion
                }
                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        private static bool CalDocPY(SqlConnection cn, Document objDoc, DataTable dtExp, DataTable dtItems, bool CalTax, bool RunCheck)
        {
            #region variable
            IEnumerable<DataRow> dtExpFilter = null;
            IEnumerable<DataRow> dtParentFilter = null;
            bool FailCheck = false;
            decimal? TotalTaxAmtF = 0;      //Total of ItmTaxGrpAmtF
            decimal? TotalTaxAmtL = 0;      //Total of ItmTaxGrpAmtL
            decimal? SubTotal = 0;         //Total of all detail items w/o gst
            decimal? TotalApplyAmtF = 0;    //Total of ItmApplyPayAmtF
            decimal? TotalApplyAmtH = 0;    //Total of ItmApplyPayAmtH

            DateTime DocDate;
            int? DocConKey = 0;
            int? DocCurrKey = 1;
            decimal? DocCurrRate = 1;
            decimal? DocCountryRate = 1;
            int? DocTaxKey = 0;
            decimal? DocTaxRate = 0;

            decimal? ExpAmtGST = 0;
            decimal? ExpAmtF = 0;
            decimal? ExpAmtH = 0;
            int? ExpTaxGrpKey = 0;
            decimal? ExpTaxGrpRate = 0;
            decimal? ExpTaxGrpAmtF = 0;
            decimal? ExpTaxGrpAmtL = 0;
            bool ExpTaxable = false;
            decimal? TotalTaxableAmtF = 0;  //Total Taxable AmountF
            decimal? SN = 0;
            decimal? DocTaxAmt = 0;
            decimal? DocTaxAmtL = 0;
            #endregion

            try
            {
                #region Assign Variables

                dtExpFilter = dtExp.AsEnumerable().OrderBy(r => r.Field<decimal>("ExpSN"));

                DocDate = (DateTime)objDoc.DocDate;
                DocConKey = GFunc.NEInt((int?)GFunc.GetPropertyValue("DocConKey", objDoc), 0);
                DocCurrKey = GFunc.NEInt((int?)GFunc.GetPropertyValue("DocCurrKey", objDoc), 0);
                DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                DocTaxKey = (int?)GFunc.GetPropertyValue("DocTaxGrpKey", objDoc);
                DocTaxRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTaxGrpRate", objDoc), 0);
                if (SysOptionUtility.GetInt("CountryCurrency", cn) == 1) // Country Currency = Home -- Added by Jane. Mic to check
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                else
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);
                #endregion

                #region Perform process for GST Claim, GST Payment and Custom Import Tax
                switch ((int)objDoc.DocCodeKey)
                {
                    #region ARPY
                    case (int)GEnum.SystemCode.Payment_Received:
                        if (objDoc.DocType == 300)  //GST Claim
                        {
                            //Clear Apply amount
                            foreach (DataRow dr in dtItems.Rows)
                            {
                                dr["ItmApplyDisAmtF"] = 0;
                                dr["ItmApplyDisAmtH"] = 0;
                                dr["ItmApplyDocAmtF"] = 0;
                                dr["ItmApplyDocAmtH"] = 0;
                                dr["ItmApplyPayAmtF"] = 0;
                                dr["ItmApplyPayAmtH"] = 0;
                                dr["ItmApplyGainAmt"] = 0;
                            }
                            dtItems.AcceptChanges();

                            //Clear Tax
                            GFunc.SetPropertyValue("DocTaxGrpKey", objDoc, null);
                            GFunc.SetPropertyValue("DocTaxGrpRate", objDoc, 0M);
                        }
                        break;
                    #endregion

                    #region APPY
                    case (int)GEnum.SystemCode.Payment_Issue:
                        if (objDoc.DocType == 310 || objDoc.DocType == 320) //GST Payment or Custom Import Tax
                        {
                            //Clear Apply amount
                            foreach (DataRow dr in dtItems.Rows)
                            {
                                dr["ItmApplyDisAmtF"] = 0;
                                dr["ItmApplyDisAmtH"] = 0;
                                dr["ItmApplyDocAmtF"] = 0;
                                dr["ItmApplyDocAmtH"] = 0;
                                dr["ItmApplyPayAmtF"] = 0;
                                dr["ItmApplyPayAmtH"] = 0;
                                dr["ItmApplyGainAmt"] = 0;
                            }
                            dtItems.AcceptChanges();
                        }

                        if (objDoc.DocType == 310)  //GST Payment
                        {
                            //Clear Tax
                            GFunc.SetPropertyValue("DocTaxGrpKey", objDoc, null);
                            GFunc.SetPropertyValue("DocTaxGrpRate", objDoc, 0M);
                        }

                        if (objDoc.DocType == 320)  //Custom Import Tax
                        {
                            if (dtExp.Rows.Count > 0)
                            {
                                #region looping to all detail exp to calculate each row
                                foreach (DataRow row in dtExpFilter)
                                {
                                    #region reset variables
                                    ExpAmtGST = 0;
                                    ExpAmtF = 0;
                                    ExpAmtH = 0;
                                    ExpTaxGrpKey = 0;
                                    ExpTaxGrpRate = 0;
                                    ExpTaxGrpAmtF = 0;
                                    ExpTaxGrpAmtL = 0;
                                    ExpTaxable = false;
                                    #endregion

                                    #region Calculate ExpAmtGST, ExpAmtF, ExpAmtH
                                    ExpAmtGST = GFunc.NEDec(row["ExpAmtGST"], 0);
                                    ExpAmtF = ExpAmtGST;
                                    ExpAmtH = GFunc.RndC(ExpAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                                    #endregion

                                    if (RunCheck)
                                    {
                                        #region Checking
                                        if (GFunc.NEDec(row["ExpAmtGST"], 0) != ExpAmtGST)
                                            FailCheck = true;
                                        if (GFunc.NEDec(row["ExpAmtF"], 0) != ExpAmtF)
                                            FailCheck = true;
                                        if (GFunc.NEDec(row["ExpAmtH"], 0) != ExpAmtH)
                                            FailCheck = true;
                                        if ((bool)row["ExpTaxable"] != false)
                                            FailCheck = true;
                                        if (GFunc.IsNEZ(row["ExpTaxGrpKey"]) == false)
                                            FailCheck = true;
                                        if (GFunc.NEDec(row["ExpTaxGrpRate"], 0) != 0M)
                                            FailCheck = true;
                                        if (GFunc.NEDec(row["ExpTaxGrpAmtF"], 0) != 0M)
                                            FailCheck = true;
                                        if (GFunc.NEDec(row["ExpTaxGrpAmtL"], 0) != 0M)
                                            FailCheck = true;

                                        if (FailCheck)
                                        {
                                            MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ExpSN"].ToString() + "%" + row["ExpSN"].ToString());
                                            return false;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Set value to grid
                                        row["ExpAmtGST"] = ExpAmtGST;
                                        row["ExpAmtF"] = ExpAmtF;
                                        row["ExpAmtH"] = ExpAmtH;
                                        row["ExpTaxable"] = ExpTaxable;
                                        row["ExpTaxGrpKey"] = ExpTaxGrpKey.ToDBValue();
                                        row["ExpTaxGrpRate"] = ExpTaxGrpRate;
                                        row["ExpTaxGrpAmtF"] = ExpTaxGrpAmtF;
                                        row["ExpTaxGrpAmtL"] = ExpTaxGrpAmtL;
                                        #endregion
                                    }

                                    #region Calculate running total: CurItmAmt,TotalTaxAmtF,TotalTaxAmtL,DetTotalAmtF,TotalST,TotalTT,TotalCF
                                    TotalTaxAmtF = TotalTaxAmtF + ExpAmtF;
                                    #endregion

                                    if (RunCheck == false)
                                    {
                                        #region Reassign Item SN numbering
                                        SN = SN + 1;
                                        row["ExpSN"] = SN;
                                        #endregion
                                    }
                                }
                                #endregion
                            }

                            if (RunCheck)
                            {
                                #region Check Document Total
                                TotalTaxAmtF = GFunc.RndC(TotalTaxAmtF, GVar.RndDecs.Amtpt);
                                SubTotal = GFunc.RndDC(TotalTaxAmtF, DocTaxRate, GVar.RndDecs.Amtpt);

                                if (GFunc.NEDec(GFunc.GetPropertyValue("DocApplyAmtF", objDoc), 0) != 0M)
                                    FailCheck = true;
                                if (GFunc.NEDec(GFunc.GetPropertyValue("DocApplyAmtH", objDoc), 0) != 0M)
                                    FailCheck = true;
                                if (GFunc.NEDec(GFunc.GetPropertyValue("DocSubTotal", objDoc), 0) != SubTotal)
                                    FailCheck = true;
                                if (GFunc.NEDec(GFunc.GetPropertyValue("DocTaxTotal", objDoc), 0) != TotalTaxAmtF)
                                    FailCheck = true;
                                if (GFunc.NEDec(GFunc.GetPropertyValue("DocGrand", objDoc), 0) != TotalTaxAmtF)
                                    FailCheck = true;
                                if (GFunc.NEDec(GFunc.GetPropertyValue("DocHome", objDoc), 0) != GFunc.RndC(TotalTaxAmtF * DocCurrRate, GVar.RndDecs.Amtpt))
                                    FailCheck = true;
                                if (GFunc.NEDec(GFunc.GetPropertyValue("DocTaxTotalLocal", objDoc), 0) != GFunc.RndC(TotalTaxAmtF * DocCountryRate, GVar.RndDecs.Amtpt))
                                    FailCheck = true;

                                if (FailCheck)
                                {
                                    MsgBox.Show(cn, MsgID.Document.SaveFailTotalCalculationWrongInPY);
                                    return false;
                                }
                                #endregion
                            }
                            else
                            {
                                #region Calculate Document Total

                                TotalTaxAmtF = GFunc.RndC(TotalTaxAmtF, GVar.RndDecs.Amtpt);
                                GFunc.SetPropertyValue("DocApplyAmtF", objDoc, 0M);
                                GFunc.SetPropertyValue("DocApplyAmtH", objDoc, 0M);
                                GFunc.SetPropertyValue("DocSubTotal", objDoc, GFunc.RndDC(TotalTaxAmtF, DocTaxRate, GVar.RndDecs.Amtpt));
                                GFunc.SetPropertyValue("DocTaxTotal", objDoc, TotalTaxAmtF);
                                GFunc.SetPropertyValue("DocGrand", objDoc, TotalTaxAmtF);
                                GFunc.SetPropertyValue("DocHome", objDoc, GFunc.RndC(TotalTaxAmtF * DocCurrRate, GVar.RndDecs.Amtpt));
                                GFunc.SetPropertyValue("DocTaxTotalLocal", objDoc, GFunc.RndC(TotalTaxAmtF * DocCountryRate, GVar.RndDecs.Amtpt));

                                #endregion
                            }
                            return true;
                        }
                        break;
                    #endregion
                }
                #endregion

                if (dtItems.Rows.Count > 0)
                {
                    #region looping to all detail items to calculate total ItmApplyPayAmtF
                    foreach (DataRow row in dtItems.Rows)
                    {
                        TotalApplyAmtF = TotalApplyAmtF + GFunc.NEDec(row["ItmApplyPayAmtF"], 0);
                        TotalApplyAmtH = TotalApplyAmtH + GFunc.NEDec(row["ItmApplyPayAmtH"], 0);
                    }

                    #endregion
                }

                if (dtExp.Rows.Count > 0)
                {
                    #region looping to all detail exp to calculate each row
                    foreach (DataRow row in dtExpFilter)
                    {
                        #region reset variables
                        ExpAmtGST = 0;
                        ExpAmtF = 0;
                        ExpAmtH = 0;
                        ExpTaxGrpKey = 0;
                        ExpTaxGrpRate = 0;
                        ExpTaxGrpAmtF = 0;
                        ExpTaxGrpAmtL = 0;
                        ExpTaxable = false;
                        #endregion

                        #region Set variables
                        ExpTaxGrpKey = GFunc.NEInt(row["ExpTaxGrpKey"], 0);
                        ExpTaxGrpRate = GFunc.NEDec(row["ExpTaxGrpRate"], 0);

                        //ExpTaxGrpRate must always follows DocTaxRate when the ExpTaxGrpKey = DocTaxGrpKey
                        if (DocTaxKey == ExpTaxGrpKey)
                            ExpTaxGrpRate = DocTaxRate;
                        #endregion

                        #region Calculate ExpAmtGST, ExpAmtF, ExpAmtH
                        ExpAmtGST = GFunc.NEDec(row["ExpAmtGST"], 0);
                        if (objDoc.DocType == 110)
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

                        #region Calculate ExpTaxGrpKey, ExpTaxGrpRate, ExpTaxGrpAmtF, ExpTaxGrpAmtH, ExpTaxable
                        if (DocTaxRate == 0 && ExpTaxGrpRate > 0)
                        {
                            //need to set ItmTax to DocTax when doc DocTax is 0% and ItmTax is <> 0%
                            ExpTaxGrpKey = DocTaxKey;
                            ExpTaxGrpRate = 0;
                            ExpTaxGrpAmtF = 0;
                            ExpTaxGrpAmtL = 0;
                            ExpTaxable = false;
                        }
                        else
                        {
                            //when itmtaxrate > 0 the ItmTaxGrpKey must be the same as the DocTaxGrpkey
                            if ((ExpTaxGrpRate > 0 && DocTaxKey != ExpTaxGrpKey) == true)
                                ExpTaxGrpKey = DocTaxKey;

                            if (ExpTaxGrpRate > 0)
                            {
                                if (CalTax)
                                {
                                    ExpTaxGrpAmtF = GFunc.RndC(ExpAmtF * ExpTaxGrpRate, GVar.RndDecs.Amtpt);
                                    ExpTaxGrpAmtL = GFunc.RndC(ExpTaxGrpAmtF * DocCountryRate, GVar.RndDecs.Amtpt);
                                }
                                else
                                {
                                    ExpTaxGrpAmtF = GFunc.NEDec(row["ExpTaxGrpAmtF"], 0);
                                    ExpTaxGrpAmtL = GFunc.NEDec(row["ExpTaxGrpAmtL"], 0);
                                }
                                ExpTaxable = true;
                            }
                            else
                            {
                                ExpTaxGrpAmtF = 0;
                                ExpTaxGrpAmtL = 0;
                                ExpTaxable = false;
                            }
                        }
                        #endregion

                        if (RunCheck)
                        {
                            #region Checking
                            if (GFunc.NEDec(row["ExpAmtGST"], 0) != ExpAmtGST)
                                FailCheck = true;
                            if (GFunc.NEDec(row["ExpAmtF"], 0) != ExpAmtF)
                                FailCheck = true;
                            if (GFunc.NEDec(row["ExpAmtH"], 0) != ExpAmtH)
                                FailCheck = true;
                            if ((bool)row["ExpTaxable"] != ExpTaxable)
                                FailCheck = true;
                            if (GFunc.NEInt(row["ExpTaxGrpKey"], 0) != ExpTaxGrpKey)
                                FailCheck = true;
                            if (GFunc.NEDec(row["ExpTaxGrpRate"], 0) != ExpTaxGrpRate)
                                FailCheck = true;
                            if (GFunc.NEDec(row["ExpTaxGrpAmtF"], 0) != ExpTaxGrpAmtF)
                                FailCheck = true;
                            if (GFunc.NEDec(row["ExpTaxGrpAmtL"], 0) != ExpTaxGrpAmtL)
                                FailCheck = true;

                            if (FailCheck)
                            {
                                MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ExpSN"].ToString() + "%" + row["ExpSN"].ToString());
                                return false;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Set value to grid
                            row["ExpAmtGST"] = ExpAmtGST;
                            row["ExpAmtF"] = ExpAmtF;
                            row["ExpAmtH"] = ExpAmtH;
                            row["ExpTaxable"] = ExpTaxable;
                            row["ExpTaxGrpKey"] = ExpTaxGrpKey.ToDBValue();
                            row["ExpTaxGrpRate"] = ExpTaxGrpRate;
                            row["ExpTaxGrpAmtF"] = ExpTaxGrpAmtF;
                            row["ExpTaxGrpAmtL"] = ExpTaxGrpAmtL;
                            #endregion
                        }

                        #region Calculate running total: CurItmAmt,TotalTaxAmtF,TotalTaxAmtL,DetTotalAmtF,TotalST,TotalTT,TotalCF
                        TotalTaxAmtF = TotalTaxAmtF + ExpTaxGrpAmtF;
                        TotalTaxAmtL = TotalTaxAmtL + ExpTaxGrpAmtL;
                        if (ExpTaxGrpAmtF != 0)
                            TotalTaxableAmtF = TotalTaxableAmtF + ExpAmtF;

                        SubTotal = SubTotal + ExpAmtF;
                        #endregion

                        if (RunCheck == false)
                        {
                            #region Reassign Item SN numbering
                            SN = SN + 1;
                            row["ExpSN"] = SN;
                            #endregion
                        }
                    }
                    #endregion
                }

                if (RunCheck)
                {
                    #region Check Document Total
                    TotalTaxAmtF = GFunc.RndC(TotalTaxAmtF, GVar.RndDecs.Amtpt);
                    TotalTaxAmtL = GFunc.RndC(TotalTaxAmtL, GVar.RndDecs.Amtpt);
                    SubTotal = GFunc.NEDec(GFunc.GetPropertyValue("DocSubTotal", objDoc), 0);
                    DocTaxAmt = GFunc.GetDecimalPropertyValue("DocTaxTotal", objDoc).Value;
                    DocTaxAmtL = GFunc.GetDecimalPropertyValue("DocTaxTotalLocal", objDoc).Value;

                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocApplyAmtF", objDoc), 0) != TotalApplyAmtF)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocApplyAmtH", objDoc), 0) != TotalApplyAmtH)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocSubTotal", objDoc), 0) != SubTotal)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocTaxTotal", objDoc), 0) != DocTaxAmt)
                        FailCheck = true;

                    REFTaxGrp objTaxGrp = REFTaxGrp.Get(GFunc.NEInt(GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc), 0));
                    if (objTaxGrp.GSTCustom == true)
                    {
                        if (GFunc.NEDec(GFunc.GetPropertyValue("DocGrand", objDoc), 0) != SubTotal + TotalApplyAmtF)
                            FailCheck = true;
                    }
                    else
                    {
                        if (GFunc.NEDec(GFunc.GetPropertyValue("DocGrand", objDoc), 0) != SubTotal + TotalApplyAmtF + DocTaxAmt)
                            FailCheck = true;

                    }

                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocHome", objDoc), 0) != GFunc.RndC((GFunc.GetDecimalPropertyValue("DocGrand", objDoc) - TotalApplyAmtF) * DocCurrRate, GVar.RndDecs.Amtpt) + TotalApplyAmtH)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocTaxTotalLocal", objDoc), 0) != DocTaxAmtL)
                        FailCheck = true;

                    if (FailCheck)
                    {
                        MsgBox.Show(cn, MsgID.Document.SaveFailTotalCalculationWrongInPY);
                        return false;
                    }
                    #endregion
                }
                else
                {
                    #region Calculate Document Total
                    if (CalTax == false)
                    {
                        DocTaxAmt = GFunc.GetDecimalPropertyValue("DocTaxTotal", objDoc).Value;
                        DocTaxAmtL = GFunc.GetDecimalPropertyValue("DocTaxTotalLocal", objDoc).Value;
                    }
                    else
                    {
                        if (SysOptionUtility.TaxCalculationOnTotal)
                        {
                            DocTaxAmt = GFunc.RndC(TotalTaxableAmtF * DocTaxRate, GVar.RndDecs.Amtpt);
                            DocTaxAmtL = GFunc.RndC(TotalTaxableAmtF * DocTaxRate * DocCountryRate, GVar.RndDecs.Amtpt);
                        }
                        else
                        {
                            DocTaxAmt = TotalTaxAmtF;
                            DocTaxAmtL = TotalTaxAmtL;
                        }
                    }

                    TotalTaxAmtF = GFunc.RndC(TotalTaxAmtF, GVar.RndDecs.Amtpt);
                    TotalTaxAmtL = GFunc.RndC(TotalTaxAmtL, GVar.RndDecs.Amtpt);
                    GFunc.SetPropertyValue("DocApplyAmtF", objDoc, TotalApplyAmtF);
                    GFunc.SetPropertyValue("DocApplyAmtH", objDoc, TotalApplyAmtH);
                    GFunc.SetPropertyValue("DocSubTotal", objDoc, SubTotal);
                    GFunc.SetPropertyValue("DocTaxTotal", objDoc, DocTaxAmt);

                    REFTaxGrp objTaxGrp = REFTaxGrp.Get(GFunc.NEInt(GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc), 0));
                    if (objTaxGrp.GSTCustom == true)
                        GFunc.SetPropertyValue("DocGrand", objDoc, SubTotal + TotalApplyAmtF);
                    else
                        GFunc.SetPropertyValue("DocGrand", objDoc, SubTotal + TotalApplyAmtF + DocTaxAmt);

                    GFunc.SetPropertyValue("DocHome", objDoc, GFunc.RndC((GFunc.GetDecimalPropertyValue("DocGrand", objDoc) - TotalApplyAmtF) * DocCurrRate, GVar.RndDecs.Amtpt) + TotalApplyAmtH);
                    GFunc.SetPropertyValue("DocTaxTotalLocal", objDoc, DocTaxAmtL);

                    #endregion
                }
                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        private static bool CalDocPL(SqlConnection cn, Document objDoc, DataTable dtPack, DataTable dtItems, bool RunCheck)
        {
            #region Variables
            IEnumerable<DataRow> dtItemFilter = null;
            //Header  
            decimal? docWeightUOMRate = 0;
            decimal? docFooterWeightUOMRate = 0;
            decimal? docFooterMeasUOMRate = 0;
            decimal? docTotalWeightNet = 0;
            decimal? docTotalWeightGross = 0;
            decimal? docTotalVolume = 0;
            decimal? TotalDocItm = 0;
            decimal? TotalDocQty = 0;
            decimal? TotalDocPack = 0;

            //Pack         
            decimal? PackQty = 0;
            decimal? PackWeightNet = 0;
            decimal? PackWeightGross = 0;
            decimal? PackHeight = 0;
            decimal? PackWidth = 0;
            decimal? PackLength = 0;
            decimal? PackVolume = 0;

            //Detail Item
            decimal? ItmConRate = 0;
            decimal? ItmWeightNet = 0;
            decimal? ItmWeightGross = 0;
            decimal? ItmWeightUOMRate = 0;
            decimal? calRate = 0;
            decimal? ItmWeightBaseNet = 0;
            decimal? ItmWeightBaseGross = 0;
            decimal? TotalItmWeightBaseNet = 0;
            decimal? TotalItmWeightBaseGross = 0;

            decimal counterPack = 0M;
            decimal counterDet = 0M;
            bool FailCheck = false;
            #endregion

            try
            {
                #region Assign Variables
                docWeightUOMRate = GFunc.NEDec(GFunc.GetPropertyValue("DocWeightUOMRate", objDoc), 0);
                docFooterWeightUOMRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTLWeightUOMRate", objDoc), 0);
                docFooterMeasUOMRate = GFunc.NEDec(GFunc.GetPropertyValue("DocTLMeasUOMRate", objDoc), 0);
                #endregion

                IEnumerable<DataRow> dtPackFilter = dtPack.AsEnumerable().OrderBy(r => r.Field<decimal>("ItmSN"));
                counterDet = 0M;
                foreach (DataRow rowPack in dtPackFilter)
                {
                    #region Cal Detail Weight

                    dtItemFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("DocItmKey") == GFunc.NEInt(rowPack["DocItmKey"], 0)).OrderBy(r => r.Field<decimal>("DetItmSN"));
                    TotalItmWeightBaseNet = 0;      //Reset value
                    TotalItmWeightBaseGross = 0;    //Reset value

                    foreach (DataRow rowItm in dtItemFilter)
                    {
                        ItmConRate = GFunc.NEDec(rowItm["DetItmConRate"], 0);
                        ItmWeightNet = GFunc.NEDec(rowItm["DetItmWeightNet"], 0);
                        ItmWeightGross = GFunc.NEDec(rowItm["DetItmWeightGross"], 0);
                        ItmWeightUOMRate = GFunc.NEDec(rowItm["DetItmWeightUOMRate"], 0);

                        calRate = GFunc.RndDC(ItmWeightUOMRate, docWeightUOMRate, GVar.RndDecs.Prcpt);

                        ItmWeightBaseNet = GFunc.RndC(ItmWeightNet * calRate * ItmConRate, GVar.RndDecs.Prcpt);
                        ItmWeightBaseGross = GFunc.RndC(ItmWeightGross * calRate * ItmConRate, GVar.RndDecs.Prcpt);

                        if (RunCheck)
                        {
                            if (GFunc.NEDec(rowItm["DetItmWeightBaseNet"], 0) != ItmWeightBaseNet)
                                FailCheck = true;
                            if (GFunc.NEDec(rowItm["DetItmWeightBaseGross"], 0) != ItmWeightBaseGross)
                                FailCheck = true;

                            if (FailCheck)
                            {
                                MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + rowItm["DetItmSN"].ToString() + "%" + rowItm["DetItmSN"].ToString());
                                return false;
                            }
                        }
                        else
                        {
                            counterDet++;
                            rowItm["DetItmSN"] = counterDet;
                            rowItm["DetItmWeightBaseNet"] = ItmWeightBaseNet;
                            rowItm["DetItmWeightBaseGross"] = ItmWeightBaseGross;
                        }

                        TotalItmWeightBaseNet += GFunc.RndC(ItmWeightBaseNet * GFunc.NEDec(rowItm["DetItmQtyTotal"], 0), GVar.RndDecs.Prcpt);
                        TotalItmWeightBaseGross += GFunc.RndC(ItmWeightBaseGross * GFunc.NEDec(rowItm["DetItmQtyTotal"], 0), GVar.RndDecs.Prcpt);
                        TotalDocQty += GFunc.NEDec(rowItm["DetItmQtyTotal"], 0);
                        TotalDocItm += 1;

                    }
                    #endregion

                    #region Cal pack Weight
                    PackQty = GFunc.NEDec(rowPack["ItmQty"], 0);
                    PackWeightNet = GFunc.RndDC(TotalItmWeightBaseGross, PackQty, GVar.RndDecs.Prcpt);
                    PackWeightGross = PackWeightNet + GFunc.NEDec(rowPack["ItmPackWeightTare"], 0);

                    if (RunCheck)
                    {
                        if (GFunc.NEDec(rowPack["ItmPackWeightNet"], 0) != PackWeightNet)
                            FailCheck = true;
                        if (GFunc.NEDec(rowPack["ItmPackWeightGross"], 0) != PackWeightGross)
                            FailCheck = true;

                        if (FailCheck)
                        {
                            MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + rowPack["ItmSN"].ToString() + "%" + rowPack["ItmSN"].ToString());
                            return false;
                        }
                    }
                    else
                    {
                        rowPack["ItmPackWeightNet"] = PackWeightNet;
                        rowPack["ItmPackWeightGross"] = PackWeightGross;

                        counterPack++;
                        rowPack["ItmSN"] = counterPack;
                    }

                    docTotalWeightNet += TotalItmWeightBaseGross;
                    docTotalWeightGross += TotalItmWeightBaseGross + GFunc.RndC(PackQty * GFunc.NEDec(rowPack["ItmPackWeightTare"], 0), GVar.RndDecs.Prcpt);
                    #endregion

                    #region Cal Pack Volume
                    PackHeight = GFunc.NEDec(rowPack["ItmHeight"], 0);
                    PackLength = GFunc.NEDec(rowPack["ItmLength"], 0);
                    PackWidth = GFunc.NEDec(rowPack["ItmWidth"], 0);

                    PackVolume = GFunc.RndC(PackHeight * PackLength * PackWidth, GVar.RndDecs.Prcpt);
                    rowPack["ItmVolume"] = PackVolume;
                    TotalDocPack += PackQty;
                    docTotalVolume += GFunc.RndC(PackVolume * PackQty, GVar.RndDecs.Prcpt);
                    #endregion
                }

                #region Cal Footer Value
                calRate = GFunc.RndDC(docWeightUOMRate, docFooterWeightUOMRate, GVar.RndDecs.Prcpt);
                docTotalWeightNet = GFunc.RndC(docTotalWeightNet * calRate, GVar.RndDecs.Prcpt);
                docTotalWeightGross = GFunc.RndC(docTotalWeightGross * calRate, GVar.RndDecs.Prcpt);
                docTotalVolume = GFunc.RndC(docTotalVolume * docFooterMeasUOMRate, GVar.RndDecs.Prcpt);

                if (RunCheck)
                {
                    #region Checking
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocFT1", objDoc), 0) != TotalDocItm)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocFT2", objDoc), 0) != TotalDocQty)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocFT3", objDoc), 0) != TotalDocPack)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocFT4", objDoc), 0) != docTotalWeightNet)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocFT5", objDoc), 0) != docTotalWeightGross)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocFT6", objDoc), 0) != docTotalVolume)
                        FailCheck = true;

                    if (FailCheck)
                    {
                        MsgBox.Show(cn, MsgID.Document.SaveFailTotalCalculationWrong);
                        return false;
                    }
                    #endregion
                }
                else
                {
                    #region Set Document Total
                    GFunc.SetPropertyValue("DocFT1", objDoc, TotalDocItm);
                    GFunc.SetPropertyValue("DocFT2", objDoc, TotalDocQty);
                    GFunc.SetPropertyValue("DocFT3", objDoc, TotalDocPack);
                    GFunc.SetPropertyValue("DocFT4", objDoc, docTotalWeightNet);
                    GFunc.SetPropertyValue("DocFT5", objDoc, docTotalWeightGross);
                    GFunc.SetPropertyValue("DocFT6", objDoc, docTotalVolume);
                    #endregion
                }

                return true;
                #endregion
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        private static bool CalDocCSI(SqlConnection cn, Document objDoc, DataTable dtExp, DataTable dtItems, bool RunCheck)
        {
            #region variable
            IEnumerable<DataRow> dtItemFilter = null;
            IEnumerable<DataRow> dtParentFilter = null;
            bool FailCheck = false;
            decimal? DetTotalAmtF = 0;  //Total of all detail items w/o gst

            DateTime DocDate;
            int? DocConKey = 0;
            int? DocCurrKey = 1;
            decimal? DocCurrRate = 1;
            decimal? DocCountryRate = 1;

            decimal? ItmQty = 0;
            decimal? ItmPriceAfter = 0;
            decimal? ItmPriceUser = 0;
            decimal? ItmDisRate = 0;
            decimal? ItmDisValue = 0;
            decimal? ItmPrice = 0;
            decimal? ItmAmtShw = 0;
            decimal? ItmAmtF = 0;
            decimal? ItmAmtH = 0;

            long Counter = 0;
            decimal? ItmSN = 0;
            decimal? ExpSN = 0;
            #endregion

            try
            {
                #region Assign Variables
                dtItemFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == 0).OrderBy(r => r.Field<decimal>("ItmSN"));

                DocDate = (DateTime)objDoc.DocDate;
                DocConKey = GFunc.NEInt((int?)GFunc.GetPropertyValue("DocConKey", objDoc), 0);
                DocCurrKey = GFunc.NEInt((int?)GFunc.GetPropertyValue("DocCurrKey", objDoc), 0);
                DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                if (SysOptionUtility.GetInt("CountryCurrency", cn) == 1) // Country Currency = Home -- Added by Jane. Mic to check
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                else
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);
                #endregion

                if (dtExp != null)
                {
                    if (dtExp.Rows.Count > 0)
                    {
                        #region looping to all detail Exp to calculate each row
                        IEnumerable<DataRow> dtExpFilter = dtExp.AsEnumerable().OrderBy(r => r.Field<decimal>("ExpSN"));

                        foreach (DataRow row in dtExpFilter)
                        {
                            #region Reassign Exp SN numbering
                            ExpSN = ExpSN + 1;
                            row["ExpSN"] = ExpSN;
                            #endregion
                        }
                        #endregion
                    }
                }
                if (dtItems.Rows.Count > 0)
                {
                    #region looping to all detail items to calculate each row
                    foreach (DataRow row in dtItemFilter)
                    {
                        #region reset variables
                        ItmQty = 0;
                        ItmPriceAfter = 0;
                        ItmDisRate = 0;
                        ItmDisValue = 0;
                        ItmPriceUser = 0;
                        ItmPrice = 0;
                        ItmAmtShw = 0;
                        ItmAmtF = 0;
                        ItmAmtH = 0;
                        #endregion

                        #region Set variables
                        ItmQty = GFunc.NEDec(row["ItmQty"], 0);
                        ItmPriceAfter = GFunc.NEDec(row["ItmPriceAfter"], 0);
                        ItmPriceUser = GFunc.NEDec(row["ItmPriceUser"], 0);
                        #endregion

                        switch (GFunc.GetINTypeGroup(GFunc.NEInt(row["ItmType"], 0)))
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:

                                #region Calculate ItmPrice, ItmAmtShw, ItmAmtF, ItmAmtH
                                ItmPrice = ItmPriceUser;
                                ItmAmtShw = GFunc.RndC(ItmPriceUser * ItmQty, GVar.RndDecs.Amtpt);
                                ItmAmtF = GFunc.RndC(ItmPrice * ItmQty, GVar.RndDecs.Amtpt);
                                ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                                #endregion

                                if (RunCheck)
                                {
                                    #region Checking
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != ItmPrice)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtShw"], 0) != ItmAmtShw)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != ItmAmtF)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != ItmAmtH)
                                        FailCheck = true;

                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Set value to grid
                                    row["ItmPrice"] = ItmPrice;
                                    row["ItmAmtShw"] = ItmAmtShw;
                                    row["ItmAmtF"] = ItmAmtF;
                                    row["ItmAmtH"] = ItmAmtH;
                                    #endregion
                                }

                                #region Calculate running total: DetTotalAmtF
                                DetTotalAmtF = DetTotalAmtF + ItmAmtF;
                                #endregion

                                if (RunCheck == false)
                                {
                                    #region Update LatestCost and Control Price
                                    row["ItmLatestCostF"] = GFunc.RndDC(GFunc.NEDec(row["ItmLatestCostH"], 0) * GFunc.NEDec(row["ItmConRate"], 0), DocCurrRate, GVar.RndDecs.Prcpt);
                                    row["ItmControlPrice"] = GFunc.RndDC(GFunc.NEDec(row["ItmControlPriceBase"], 0) * GFunc.NEDec(row["ItmConRate"], 0), DocCurrRate, GVar.RndDecs.Prcpt);
                                    #endregion
                                }
                                break;

                            default:

                                #region Assume Header,Remark - Update ItmPrice, ItmAmtF, ItmAmtH
                                if (RunCheck)
                                {
                                    #region Checking
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != 0)
                                        FailCheck = true;

                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Set value to grid
                                    row["ItmPrice"] = 0;
                                    row["ItmAmtF"] = 0;
                                    row["ItmAmtH"] = 0;
                                    #endregion
                                }
                                #endregion
                                break;
                        }

                        if (RunCheck == false)
                        {
                            #region Reassign Item SN numbering
                            ItmSN = ItmSN + 1;
                            row["ItmSN"] = ItmSN;
                            #endregion
                        }
                    }
                    #endregion

                    if (RunCheck == false)
                    {
                        #region Assign SN to all detail items,Parents and Childs

                        dtParentFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == 0 && r.Field<int>("LineType") == 1000);

                        foreach (DataRow rowParent in dtParentFilter)
                        {
                            dtItemFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == GFunc.NEInt(rowParent["DocItmKey"], -1) && r.Field<int>("LineType") >= 1000);
                            int childSN = 1;
                            foreach (DataRow rowChild in dtItemFilter)
                            {
                                rowChild["ItmSN"] = rowParent["ItmSN"];
                                rowChild["ItmDetSN"] = childSN;
                                childSN++;
                            }
                        }

                        #endregion
                    }
                }

                if (RunCheck)
                {
                    #region Check Document Total
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocGrand", objDoc), 0) != DetTotalAmtF)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocHome", objDoc), 0) != GFunc.RndC(DetTotalAmtF * DocCurrRate, GVar.RndDecs.Amtpt))
                        FailCheck = true;

                    if (FailCheck)
                    {
                        MsgBox.Show(cn, MsgID.Document.SaveFailTotalCalculationWrong);
                        return false;
                    }
                    #endregion
                }
                else
                {
                    #region Calculate Document Total
                    GFunc.SetPropertyValue("DocGrand", objDoc, DetTotalAmtF);
                    GFunc.SetPropertyValue("DocHome", objDoc, GFunc.RndC(DetTotalAmtF * DocCurrRate, GVar.RndDecs.Amtpt));
                    #endregion
                }
                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        private static bool CalDocCPO(SqlConnection cn, Document objDoc, DataTable dtItems, bool RunCheck)
        {
            #region variable
            IEnumerable<DataRow> dtItemFilter = null;
            IEnumerable<DataRow> dtParentFilter = null;
            bool FailCheck = false;
            decimal? DetTotalAmtF = 0;  //Total of all detail items w/o gst

            DateTime DocDate;
            int? DocConKey = 0;
            int? DocCurrKey = 1;
            decimal? DocCurrRate = 1;
            decimal? DocCountryRate = 1;

            decimal? ItmQty = 0;
            decimal? ItmPriceAfter = 0;
            decimal? ItmDisRate = 0;
            decimal? ItmDisValue = 0;
            decimal? ItmPriceUser = 0;
            decimal? ItmPrice = 0;
            decimal? ItmAmtShw = 0;
            decimal? ItmAmtF = 0;
            decimal? ItmAmtH = 0;

            decimal? SN = 0;
            #endregion

            try
            {
                #region Assign Variables
                dtItemFilter = dtItems.AsEnumerable().OrderBy(r => r.Field<decimal>("ItmSN"));

                DocDate = (DateTime)objDoc.DocDate;
                DocConKey = GFunc.NEInt((int?)GFunc.GetPropertyValue("DocConKey", objDoc), 0);
                DocCurrKey = GFunc.NEInt((int?)GFunc.GetPropertyValue("DocCurrKey", objDoc), 0);
                DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                if (SysOptionUtility.GetInt("CountryCurrency", cn) == 1) // Country Currency = Home -- Added by Jane. Mic to check
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                else
                    DocCountryRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCountryRate", objDoc), 1);
                #endregion

                if (dtItems.Rows.Count > 0)
                {
                    #region looping to all detail items to calculate each row
                    foreach (DataRow row in dtItemFilter)
                    {

                        #region reset variables
                        ItmQty = 0;
                        ItmPriceAfter = 0;
                        ItmDisRate = 0;
                        ItmDisValue = 0;
                        ItmPriceUser = 0;
                        ItmPrice = 0;
                        ItmAmtShw = 0;
                        ItmAmtF = 0;
                        ItmAmtH = 0;
                        #endregion

                        #region Set variables
                        ItmQty = GFunc.NEDec(row["ItmQty"], 0);
                        ItmPriceAfter = GFunc.NEDec(row["ItmPriceAfter"], 0);
                        ItmPriceUser = GFunc.NEDec(row["ItmPriceUser"], 0);
                        #endregion

                        switch (GFunc.GetINTypeGroup(GFunc.NEInt(row["ItmType"], 0)))
                        {
                            case (int)GEnum.INTypeGrp.Stock:
                            case (int)GEnum.INTypeGrp.Non_Stock:

                                #region Calculate ItmPrice, ItmAmtShw, ItmAmtF, ItmAmtH
                                ItmPrice = ItmPriceUser;
                                ItmAmtShw = GFunc.RndC(ItmPriceUser * ItmQty, GVar.RndDecs.Amtpt);
                                ItmAmtF = GFunc.RndC(ItmPrice * ItmQty, GVar.RndDecs.Amtpt);
                                ItmAmtH = GFunc.RndC(ItmAmtF * DocCurrRate, GVar.RndDecs.Amtpt);
                                #endregion

                                if (RunCheck)
                                {
                                    #region Checking
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != ItmPrice)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtShw"], 0) != ItmAmtShw)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != ItmAmtF)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != ItmAmtH)
                                        FailCheck = true;

                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Set value to grid
                                    row["ItmPrice"] = ItmPrice;
                                    row["ItmAmtShw"] = ItmAmtShw;
                                    row["ItmAmtF"] = ItmAmtF;
                                    row["ItmAmtH"] = ItmAmtH;
                                    #endregion
                                }

                                #region Calculate running total: DetTotalAmtF
                                DetTotalAmtF = DetTotalAmtF + ItmAmtF;
                                #endregion

                                if (RunCheck == false)
                                {
                                    #region Update LatestCost
                                    row["ItmLatestCostF"] = GFunc.RndDC(GFunc.NEDec(row["ItmLatestCostH"], 0) * GFunc.NEDec(row["ItmConRate"], 0), DocCurrRate, GVar.RndDecs.Prcpt);
                                    #endregion
                                }
                                break;

                            default:

                                #region Assume Header,Remark - Update ItmPrice, ItmAmtF, ItmAmtH
                                if (RunCheck)
                                {
                                    if (GFunc.NEDec(row["ItmPrice"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtF"], 0) != 0)
                                        FailCheck = true;
                                    if (GFunc.NEDec(row["ItmAmtH"], 0) != 0)
                                        FailCheck = true;

                                    if (FailCheck)
                                    {
                                        MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["ItmSN"].ToString() + "%" + row["ItmSN"].ToString());
                                        return false;
                                    }
                                }
                                else
                                {
                                    row["ItmPrice"] = 0;
                                    row["ItmAmtF"] = 0;
                                    row["ItmAmtH"] = 0;
                                }
                                #endregion
                                break;
                        }

                        if (RunCheck == false)
                        {
                            #region Reassign Item SN numbering
                            SN = SN + 1;
                            row["ItmSN"] = SN;
                            #endregion
                        }
                    }
                    #endregion
                }

                if (RunCheck)
                {
                    #region Check Document Total

                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocGrand", objDoc), 0) != DetTotalAmtF)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocHome", objDoc), 0) != GFunc.RndC(DetTotalAmtF * DocCurrRate, GVar.RndDecs.Amtpt))
                        FailCheck = true;

                    if (FailCheck)
                    {
                        MsgBox.Show(cn, MsgID.Document.SaveFailTotalCalculationWrong);
                        return false;
                    }
                    #endregion
                }
                else
                {
                    #region Calculate Document Total
                    GFunc.SetPropertyValue("DocGrand", objDoc, DetTotalAmtF);
                    GFunc.SetPropertyValue("DocHome", objDoc, GFunc.RndC(DetTotalAmtF * DocCurrRate, GVar.RndDecs.Amtpt));
                    #endregion
                }
                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        private static bool CalDocCPS(SqlConnection cn, Document objDoc, DataTable dtItems, bool RunCheck)
        {
            #region variable
            bool FailCheck = false;
            decimal? TotalSale = 0;             //Total of all Sales Line
            decimal? TotalSaleAfterDis = 0;     //Total of all Sales Line after discount
            decimal? TotalExpense = 0;          //Total of all Expense Line

            decimal? DocCurrRate = 1;

            decimal? ItmQty = 0;
            decimal? ItmPrice = 0;
            decimal? ItmDisPrice = 0;
            decimal? ItmAmtF = 0;
            decimal? ItmAmtH = 0;
            #endregion

            try
            {
                DocCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 0);

                if (dtItems.Rows.Count > 0)
                {
                    #region looping to all detail items to calculate each row

                    foreach (DataRow row in dtItems.Rows)
                    {
                        #region Calculate Line values
                        if (GFunc.NEInt(row["LineType"], 0) == 4030) //Expense Line Type
                        {
                            ItmAmtF = GFunc.NEDec(row["ItmAmtF"], 0);
                            ItmAmtH = GFunc.RndC(DocCurrRate * ItmAmtF, GVar.RndDecs.Amtpt);
                            TotalExpense += ItmAmtF;
                        }
                        else  //Sales Line
                        {
                            ItmQty = GFunc.NEDec(row["ItmQty"], 0);
                            ItmPrice = GFunc.NEDec(row["ItmPrice"], 0);
                            ItmDisPrice = GFunc.NEDec(row["ItmDisPrice"], 0);
                            ItmAmtF = GFunc.RndC(ItmQty * ItmDisPrice, GVar.RndDecs.Amtpt);
                            ItmAmtH = GFunc.RndC(DocCurrRate * ItmAmtF, GVar.RndDecs.Amtpt);

                            TotalSale += GFunc.RndC(ItmQty * ItmPrice, GVar.RndDecs.Amtpt);
                            TotalSaleAfterDis += ItmAmtF;
                        }
                        #endregion

                        if (RunCheck)
                        {
                            #region Checking
                            if (GFunc.NEDec(row["ItmAmtF"], 0) != ItmAmtF)
                                FailCheck = true;
                            if (GFunc.NEDec(row["ItmAmtH"], 0) != ItmAmtH)
                                FailCheck = true;

                            if (FailCheck)
                            {
                                MsgBox.Show(cn, MsgID.Document.SaveFailDetailCalculationWrong + "%" + row["CPDID"].ToString() + "%" + row["SettlementDocID"].ToString());
                                return false;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Set value to grid
                            row["ItmAmtF"] = ItmAmtF;
                            row["ItmAmtH"] = ItmAmtH;
                            row["ItmDetSN"] = 0;
                            #endregion
                        }
                    }
                    #endregion
                }
                if (RunCheck)
                {
                    #region Check Document Total
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocSales", objDoc), 0) != TotalSale)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocSalesAfterDis", objDoc), 0) != TotalSaleAfterDis)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocExpense", objDoc), 0) != TotalExpense)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocGrand", objDoc), 0) != TotalSaleAfterDis - TotalExpense)
                        FailCheck = true;

                    if (FailCheck)
                    {
                        MsgBox.Show(cn, MsgID.Document.SaveFailTotalCalculationWrong);
                        return false;
                    }
                    #endregion
                }
                else
                {
                    #region Calculate Document Total
                    GFunc.SetPropertyValue("DocSales", objDoc, TotalSale);
                    GFunc.SetPropertyValue("DocSalesAfterDis", objDoc, TotalSaleAfterDis);
                    GFunc.SetPropertyValue("DocExpense", objDoc, TotalExpense);
                    GFunc.SetPropertyValue("DocGrand", objDoc, TotalSaleAfterDis - TotalExpense);
                    #endregion
                }
                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        private static bool CalDocDP(SqlConnection cn, Document objDoc, DataTable dtItems, bool RunCheck)
        {
            #region variable
            bool FailCheck = false;
            decimal ItmDocCurRate = 1M;
            decimal ItmDocAmtF = 0;
            decimal ItmBankRate = 0;
            decimal TotalItmDocAmtH = 0;
            decimal TotalDocHome = 0;
            decimal TotalDocGrand = 0;
            decimal TotalDocGainAmtH = 0;
            decimal SN = 0;
            bool havediffCurrency = false;
            #endregion

            try
            {
                int DocCurrKey = (int)GFunc.GetIntPropertyValue("DocCurrKey", objDoc);
                decimal DocCurRate = (decimal)GFunc.GetDecimalPropertyValue("DocCurrRate", objDoc);

                if (dtItems.Rows.Count > 0)
                {
                    #region looping all detail items to calculate each row
                    IEnumerable<DataRow> dtItemFilter = dtItems.AsEnumerable().OrderBy(r => r.Field<decimal>("ItmSN"));

                    foreach (DataRow row in dtItemFilter)
                    {
                        SN++;
                        row["ItmSN"] = SN;

                        if (DocCurrKey == (int)row["ItmDocCurrKey"])
                        {
                            ItmBankRate = 1;
                        }
                        else
                        {
                            ItmBankRate = GFunc.RndDC((decimal)row["ItmDocCurrRate"], DocCurRate, GVar.RndDecs.Curpt);
                            havediffCurrency = true;
                        }

                        ItmDocAmtF = (decimal)row["ItmDocAmtF"];
                        ItmDocCurRate = (decimal)row["ItmDocCurrRate"];

                        row["ItmBankRate"] = ItmBankRate;
                        row["ItmDocAmtH"] = GFunc.RndC(ItmDocAmtF * ItmDocCurRate, GVar.RndDecs.Amtpt);
                        row["ItmBankAmtF"] = GFunc.RndC(ItmDocAmtF * ItmBankRate, GVar.RndDecs.Amtpt);
                        row["ItmBankAmtH"] = GFunc.RndC(ItmDocAmtF * ItmBankRate * DocCurRate, GVar.RndDecs.Amtpt);

                        TotalDocGrand = TotalDocGrand + (decimal)row["ItmBankAmtF"];
                        TotalItmDocAmtH = TotalItmDocAmtH + (decimal)row["ItmDocAmtH"];
                    }
                    #endregion
                }

                TotalDocHome = GFunc.RndC(TotalDocGrand * DocCurRate, GVar.RndDecs.Amtpt);
                TotalDocGainAmtH = TotalDocHome - TotalItmDocAmtH;

                if (RunCheck)
                {
                    #region Check Document Total
                    if (havediffCurrency && objDoc.IsReadOnly == false)
                    {
                        if (GFunc.NEDec(GFunc.GetPropertyValue("DocGrand", objDoc), 0) != TotalDocGrand)
                            FailCheck = true;
                    }
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocHome", objDoc), 0) != TotalDocHome)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocGainAmtH", objDoc), 0) != TotalDocGainAmtH)
                        FailCheck = true;

                    if (FailCheck)
                    {
                        MsgBox.Show(cn, MsgID.Document.SaveFailTotalCalculationWrong);
                        return false;
                    }
                    #endregion
                }
                else
                {
                    #region Calculate Document Total
                    GFunc.SetPropertyValue("DocGrand", objDoc, TotalDocGrand);
                    GFunc.SetPropertyValue("DocHome", objDoc, TotalDocHome);
                    GFunc.SetPropertyValue("DocGainAmtH", objDoc, TotalDocGainAmtH);
                    #endregion
                }
                return true;

            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        private static bool CalDocJN(SqlConnection cn, Document objDoc, DataTable dtItems, bool RunCheck)
        {
            #region variable
            bool FailCheck = false;
            decimal? TotalCRH = 0;      //Total of all Credit Home
            decimal? TotalDBH = 0;      //Total of all Debit Home

            decimal? ItmDBH = 0;
            decimal? ItmCRH = 0;

            decimal? SN = 0;
            #endregion

            try
            {
                if (dtItems.Rows.Count > 0)
                {
                    #region looping to all detail items to calculate each row
                    IEnumerable<DataRow> dtItemFilter = dtItems.AsEnumerable().OrderBy(r => r.Field<decimal>("ItmSN"));
                    foreach (DataRow row in dtItemFilter)
                    {
                        ItmDBH = GFunc.NEDec(row["ItmDebitHTotal"], 0);
                        ItmCRH = GFunc.NEDec(row["ItmCreditHTotal"], 0);

                        SN = SN + 1;
                        row["ItmSN"] = SN;

                        TotalDBH += ItmDBH;
                        TotalCRH += ItmCRH;

                    }
                    #endregion
                }
                if (RunCheck)
                {
                    #region Check Document Total
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocTotalDebitH", objDoc), 0) != TotalDBH)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocTotalCreditH", objDoc), 0) != TotalCRH)
                        FailCheck = true;
                    if (GFunc.NEDec(GFunc.GetPropertyValue("DocTotalCreditH", objDoc), 0) != GFunc.NEDec(GFunc.GetPropertyValue("DocTotalDebitH", objDoc), 0))
                        FailCheck = true;
                    if (FailCheck)
                    {
                        MsgBox.Show(cn, "Unbalance Entries, please verify that your journal entries Total Debit and Credit in Home currency are match");
                        return false;
                    }
                    #endregion
                }
                else
                {
                    #region Calculate Document Total
                    GFunc.SetPropertyValue("DocTotalDebitH", objDoc, TotalDBH);
                    GFunc.SetPropertyValue("DocTotalCreditH", objDoc, TotalCRH);
                    #endregion
                }
                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        private static bool CalDocIN(Document objDoc, DataTable dtItems)
        {
            try
            {
                IEnumerable<DataRow> dtItemFilter = null;
                IEnumerable<DataRow> dtParentFilter = null;

                int previousLineType = 0;
                decimal SN = 0;
                int childSN = 1;

                if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Inventory_Production)
                {
                    if (dtItems.Rows.Count > 0)
                    {
                        #region looping to all detail items to calculate each row
                        dtParentFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == 0).OrderBy(r => r.Field<int>("LineType")).ThenBy(r => r.Field<decimal>("ItmSN"));
                        foreach (DataRow row in dtParentFilter)
                        {
                            if (previousLineType != (int)row["LineType"])
                            {
                                SN = 1;
                                previousLineType = (int)row["LineType"];
                            }
                            else
                            {
                                SN = SN + 1;
                            }

                            row["ItmSN"] = SN;
                            childSN = 1;

                            dtItemFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == GFunc.NEInt(row["DocItmKey"], 0)).OrderBy(r => r.Field<decimal>("ItmDetSN"));
                            foreach (DataRow rowChild in dtItemFilter)
                            {
                                rowChild["ItmDetSN"] = childSN;
                                childSN++;
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    if (dtItems.Rows.Count > 0)
                    {
                        #region looping to all detail items to calculate each row
                        dtParentFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == 0).OrderBy(r => r.Field<decimal>("ItmSN"));
                        foreach (DataRow row in dtParentFilter)
                        {
                            SN = SN + 1;
                            row["ItmSN"] = SN;
                            childSN = 1;

                            dtItemFilter = dtItems.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == GFunc.NEInt(row["DocItmKey"], 0)).OrderBy(r => r.Field<decimal>("ItmDetSN"));
                            foreach (DataRow rowChild in dtItemFilter)
                            {
                                rowChild["ItmDetSN"] = childSN;
                                childSN++;
                            }
                        }
                        #endregion
                    }
                }
                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed

        public static decimal? CurrRate_Get(int? currKey, DateTime? docDate, bool MsgYN)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return CurrRate_Get(cn, currKey, docDate, MsgYN);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public static decimal? CurrRate_Get(SqlConnection cn, int? currKey, DateTime? docDate, bool MsgYN)
        {
            //MsgYN     (True: Display Rate not current message, False: no message)
            int? opValue = 0;

            try
            {
                //Home Currency
                if (currKey == 1)
                    return 1;
                else
                {
                    opValue = SysOptionUtility.GetInt("WarnCurrencyRateNotValid", cn);

                    List<SqlParameter> parmList = new List<SqlParameter>();
                    parmList.Add(new SqlParameter("@Curr", currKey));
                    parmList.Add(new SqlParameter("@DocDate", docDate));
                    DataTable dt = GFunc.ExecuteProc(cn, "ROREFCurrRate_Get", parmList);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (MsgYN)
                        {
                            if (docDate != Convert.ToDateTime(dt.Rows[0]["CurrDate"]))
                            {
                                //Current Date
                                if (opValue == 10)
                                    MsgBox.Show(cn, MsgID.Document.CurrencyRateNotCurrent);

                                //Current Week
                                else if (opValue == 20)
                                {
                                    DateTime vCurrencyDate = Convert.ToDateTime(dt.Rows[0]["CurrDate"]);
                                    int vCurrencyWeek = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(vCurrencyDate, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                                    int vDocWeek = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear((DateTime)docDate, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                                    if (vCurrencyWeek != vDocWeek)
                                        MsgBox.Show(cn, MsgID.Document.CurrencyRateNotCurrent);
                                }
                                //Current Month
                                else if (opValue == 30)
                                {
                                    if (Convert.ToDateTime(dt.Rows[0]["CurrDate"]).Month != ((DateTime)docDate).Month)
                                        MsgBox.Show(cn, MsgID.Document.CurrencyRateNotCurrent);
                                }
                            }
                        }
                        return GFunc.NEDec(dt.Rows[0]["CurrRate"], 0);
                    }
                    else
                    {
                        //always return rate = 1 when no rate can be found
                        if (MsgYN && opValue != 5)
                            MsgBox.Show(cn, MsgID.Document.CurrencyRateNotCurrent);

                        return 1;
                    }
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static decimal CountryRate_Get(int conKey, int currKey, decimal currRate, DateTime docDate, bool MsgYN)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return CountryRate_Get(cn, conKey, currKey, currRate, docDate, MsgYN);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public static decimal CountryRate_Get(SqlConnection cn, int conKey, int currKey, decimal currRate, DateTime docDate, bool MsgYN)
        {
            //MsgYN     (True: Display Rate not current message, False: no message)
            DataTable dt = null;
            int? opValue = 0;
            string fldNmDate = string.Empty;
            string fldNmRate = string.Empty;
            bool runCurrencyGet = false;

            try
            {
                //Currency CountryCurrency
                if (SysOptionUtility.CountryCurrency == 1)
                    return currRate;
                else
                {
                    opValue = SysOptionUtility.GetInt("WarnCountryRateNotValid", cn);

                    List<SqlParameter> parmList = new List<SqlParameter>();
                    if (conKey > 0)
                    {
                        parmList.Add(new SqlParameter("@Curr", currKey));
                        parmList.Add(new SqlParameter("@DocDate", docDate));
                        parmList.Add(new SqlParameter("@ConKey", conKey));
                        dt = GFunc.ExecuteProc(cn, "ROREFCurrRateCon_Get", parmList);
                        fldNmDate = "ConCurrDate";
                        fldNmRate = "ConCurrRate";
                        if (dt == null || dt.Rows.Count == 0)
                            runCurrencyGet = true;
                    }
                    else
                    {
                        runCurrencyGet = true;
                    }

                    if (runCurrencyGet)
                    {
                        parmList.Clear();
                        parmList.Add(new SqlParameter("@Curr", currKey));
                        parmList.Add(new SqlParameter("@DocDate", docDate));
                        dt = GFunc.ExecuteProc(cn, "ROREFCurrRate_Get", parmList);
                        fldNmDate = "CurrDate";                   
                        fldNmRate = "CountryRate";
                    }

                    if (currKey == SysOptionUtility.CountryCurrency)
                        return 1;

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //Check is currency current base on option
                        if (MsgYN)
                        {
                            if (docDate != Convert.ToDateTime(dt.Rows[0][fldNmDate]))
                            {
                                switch (opValue)
                                {
                                    case 10:    //Current Date
                                        MsgBox.Show(cn, MsgID.Document.CountryCurrencyRateNotCurrent);
                                        break;

                                    case 20:    //Current Week
                                        DateTime vCurrencyDate = Convert.ToDateTime(dt.Rows[0][fldNmDate]);
                                        int vCurrencyWeek = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(vCurrencyDate, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                                        int vDocWeek = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(docDate, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                                        if (vCurrencyWeek != vDocWeek)
                                            MsgBox.Show(cn, MsgID.Document.CountryCurrencyRateNotCurrent);
                                        break;

                                    case 30:    //Current Month
                                        if (Convert.ToDateTime(dt.Rows[0][fldNmDate]).Month != docDate.Month)
                                            MsgBox.Show(cn, MsgID.Document.CountryCurrencyRateNotCurrent);
                                        break;
                                }
                            }
                        }
                        return GFunc.NEDec(dt.Rows[0][fldNmRate], 0);
                    }
                    else
                    {
                        //always return rate = 1 when no rate can be found
                        if (MsgYN && opValue != 5)
                            MsgBox.Show(cn, MsgID.Document.CountryCurrencyRateNotCurrent);

                        return 1;
                    }
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                dt = null;
            }
        }//Completed

        public static bool DocType_Get(SqlConnection cn, int docCodeKey, string docTypeNm, ref int docType, ref short docSign)
        {

            try
            {
                DataTable dt = null;

                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@Option", 5));
                parmList.Add(new SqlParameter("@CodeKey", docCodeKey));
                parmList.Add(new SqlParameter("@DocTypeNm", docTypeNm));
                parmList.Add(new SqlParameter("@RetValue", 0));
                parmList[3].Direction = ParameterDirection.Output;
                dt = GFunc.ExecuteProc(cn, "SYSDocTypeDetNm_Get", parmList);

                if (dt != null && dt.Rows.Count > 0)
                {
                    docType = (int)dt.Rows[0]["DocType"];
                    docSign = (short)dt.Rows[0]["DocSign"];
                }
                else
                {
                    MsgBox.Show(cn, "Unable to retrive Document Type Information");
                    return false;
                }
                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public static string DocCreditCashType_Get(int docCodeKey)
        {

            try
            {
                //check 10-Cash/20-Credit type for Customer 
                switch (docCodeKey)
                {

                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Packing_List:
                        return "10,20,30";    //Credit and Cash document

                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Sales_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        return "10";    //Credit document

                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Contra:
                        return "20";    //Cash document

                    default:
                        return "10,20,30";    //Credit and Cash document
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public static void DocCreditCashType_Get(int docCodeKey, ref string ccbCredit, ref string ccbCash, ref string ccbBoth)
        {

            try
            {
                //check 10-Cash/20-Credit type for Customer 
                switch (docCodeKey)
                {

                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Packing_List:
                        //Credit and Cash document
                        ccbCredit = "10";
                        ccbCash = "20";
                        ccbBoth = "30";
                        break;

                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Sales_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        //Credit document
                        ccbCredit = "10";
                        ccbCash = "0";
                        ccbBoth = "0";
                        break;
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Contra:
                        //Cash document
                        ccbCredit = "0";
                        ccbCash = "20";
                        ccbBoth = "0";
                        break;
                    default:
                        //Credit and Cash document
                        ccbCredit = "10";
                        ccbCash = "20";
                        ccbBoth = "30";
                        break;
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public static string DocConType_Get(int docCodeKey)
        {
            const int Cust = 10;
            const int Vend = 20;
            const int Both = 30;
            const int Pros = 40;

            try
            {
                switch (docCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                        return "10,30,40";
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
                    case (int)GEnum.SystemCode.Job:
                        return "20,30";

                    case (int)GEnum.SystemCode.Sales_Order:
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
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.DO_to_IV_Transfer:
                        return "10,30";
                    case (int)GEnum.SystemCode.Contra:
                        return "30";
                    default:
                        return "30";
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public static void DocConType_Get(int docCodeKey, ref string conCust, ref string conVen, ref string conBoth, ref string conPros)
        {
            const int Cust = 10;
            const int Vend = 20;
            const int Both = 30;
            const int Pros = 40;

            try
            {
                switch (docCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:

                        conCust = "10";
                        conVen = "20";
                        conBoth = "30";
                        conPros = "40";
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
                    case (int)GEnum.SystemCode.Job:
                        conCust = "0";
                        conVen = "20";
                        conBoth = "30";
                        conPros = "0";
                        break;

                    case (int)GEnum.SystemCode.Sales_Order:
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
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.DO_to_IV_Transfer:

                        conCust = "10";
                        conVen = "0";
                        conBoth = "30";
                        conPros = "0";
                        break;

                    case (int)GEnum.SystemCode.Contra:

                        conCust = "0";
                        conVen = "0";
                        conBoth = "30";
                        conPros = "0";
                        break;
                    default:
                        conCust = "10";
                        conVen = "20";
                        conBoth = "30";
                        conPros = "40";
                        break;
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed

        public static int GridAutoID_Get(UltraGrid tagrdDetItms, string headerColumnName, string detailColumnName)
        {
            //This function return the next auto id (DocItmKey) in the grid
            try
            {
                TAUtil.TAGridEditor grd = (TAUtil.TAGridEditor)tagrdDetItms;
                string HeaderKey = string.Empty;
                int maxDetKey = 0;
                int rowCount = 0;

                rowCount = ((DataTable)grd.DataSource).Rows.Count;
                if (rowCount == 0)
                {
                    grd.HeaderObjectKey = string.Empty;
                    grd.DetailObjectKey = 0;
                    return 1;
                }
                else
                {
                    maxDetKey = (grd.DataSource as DataTable).AsEnumerable().Max(k => k.Field<int>(detailColumnName));
                    if (maxDetKey < rowCount)
                        return rowCount + 1;
                    else
                    {
                        HeaderKey = GFunc.NEStr(((DataTable)grd.DataSource).Rows[0][headerColumnName], string.Empty);
                        if (GFunc.IsNEZ(HeaderKey))
                        {
                            grd.HeaderObjectKey = string.Empty;
                            return rowCount + 1;
                        }
                        else
                        {
                            if (HeaderKey != grd.HeaderObjectKey)
                            {
                                //reset
                                grd.HeaderObjectKey = HeaderKey;
                                grd.DetailObjectKey = GFunc.NEInt((grd.DataSource as DataTable).AsEnumerable().Max(k => k.Field<int>(detailColumnName)), rowCount);
                                return grd.DetailObjectKey + 1;
                            }
                            else
                            {
                                //Update
                                maxDetKey = GFunc.NEInt((grd.DataSource as DataTable).AsEnumerable().Max(k => k.Field<int>(detailColumnName)), rowCount);
                                if (grd.DetailObjectKey < maxDetKey)
                                {
                                    grd.DetailObjectKey = maxDetKey;
                                    return maxDetKey + 1;
                                }
                                else
                                {
                                    return grd.DetailObjectKey + 1;
                                }
                            }
                        }
                    }
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public static void DocDetail_Get(GEnum.Details name, Hashtable details, ref DataTable dtdetail)
        {
            //Check the Type of Detail and 
            //Get Detail as DataTable or Grid from HashTable
            try
            {
                if (details.Contains(name))  //check DataTable is contains in HashTable
                {
                    if (GFunc.CompareString(details[name].GetType().Name, "DataTable") || details[name].GetType().BaseType == typeof(DataTable))
                    {
                        dtdetail = (DataTable)details[name];
                        dtdetail.AcceptChanges();
                        if (dtdetail.DefaultView.Count > 0)
                        {
                            if (dtdetail.DefaultView[dtdetail.DefaultView.Count - 1].Row.RowState == DataRowState.Detached)
                            {
                                if (dtdetail.Rows.Count > 0)
                                {
                                    dtdetail.DefaultView[dtdetail.DefaultView.Count - 1].Delete();
                                }
                            }
                        }
                    }
                    else
                        dtdetail = ((UltraGrid)details[name]).DataSource as DataTable;
                }
                else
                    MsgBox.Show("Unable to retrive DataTable from Hashtable");

            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public static void DocDetail_Get(GEnum.Details name, Hashtable details, ref UltraGrid grddetail)
        {
            try
            {
                if (details.Contains(name))                 //check DataTable is contains in HashTable
                    grddetail = (UltraGrid)details[name];    //return DataTable
                else
                    MsgBox.Show("Unable to retrive grid from Hashtable");

            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public static void DocDetail_Get(GEnum.Details name, Hashtable details, ref SYSAttachments objSYSAttachments)
        {
            //Check the Type of Detail and 
            //Get Detail as objSYSAttachments from HashTable
            try
            {
                if (details.Contains(name))  //check DataTable is contains in HashTable
                {
                    if (details[name].GetType() == typeof(SYSAttachments))
                    {
                        objSYSAttachments = (SYSAttachments)details[name];
                    }
                    else
                        objSYSAttachments = null;
                }
                else
                    MsgBox.Show("Unable to retrive Attachment object from Hashtable");

            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public static void DocDetail_Get(GEnum.Details name, Hashtable details, ref TAUtil.TAGridEditor grddetail)
        {
            try
            {
                if (details.Contains(name))                 //check DataTable is contains in HashTable
                {
                    grddetail = (TAUtil.TAGridEditor)details[name];    //return DataTable
                    grddetail.Tag = null;
                }
                else
                    MsgBox.Show("Unable to retrive grid from Hashtable");

            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public static bool DocDetail_Get(int docCodeKey, Hashtable grdDetails, ref UltraGrid grdItm, ref UltraGrid grdExp)
        {
            try
            {
                switch (docCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, grdDetails, ref grdItm);
                        return true;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, grdDetails, ref grdItm);
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, grdDetails, ref grdExp);
                        return true;

                    case (int)GEnum.SystemCode.Packing_List:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, grdDetails, ref grdItm);
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Pack, grdDetails, ref grdExp);
                        return true;
                }

                MsgBox.Show("Unable to retrive detail grid from document code");
                return false;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public static bool DocDetail_Get(int docCodeKey, Hashtable grdDetails, ref DataTable dtItm, ref DataTable dtExp)
        {
            try
            {
                switch (docCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, grdDetails, ref dtItm);
                        return true;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, grdDetails, ref dtItm);
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, grdDetails, ref dtExp);
                        return true;

                    case (int)GEnum.SystemCode.Packing_List:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Pack, grdDetails, ref dtExp);
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, grdDetails, ref dtItm);
                        return true;
                }

                MsgBox.Show("Unable to retrive detail grid from document code");
                return false;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed

        public static string FormCaption_Set(int DocCodeKey)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return FormCaption_Set(cn, DocCodeKey);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static string FormCaption_Set(SqlConnection cn, int DocCodeKey)
        {
            try
            {
                string formCaption = string.Empty;

                SYSCode _sysCode = SYSCode.Get(cn, DocCodeKey);
                if (GFunc.IsNE(_sysCode) == false)
                    formCaption = _sysCode._codeDesLang1;
                else
                    formCaption = ((GEnum.SystemCode)DocCodeKey).ToString();

                return formCaption;
            }

            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static bool INactiveSelection(bool? ActiveStatus, string FieldName)
        {
            try
            {
                if (!GFunc.IsNE(ActiveStatus))
                    if ((bool)ActiveStatus)
                    {
                        MsgBox.Show(MsgID.Document.InactiveSelection + "%" + FieldName);
                        return true;
                    }

                return false;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static bool IsItmCostingContinuous()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return IsItmCostingContinuous(cn);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static bool IsItmCostingContinuous(SqlConnection cn)
        {
            int opValue = 0;
            try
            {
                //Continuous or Batch COS Posting
                opValue = SysOptionUtility.InventoryValuationMethod;
                if (opValue == 10 || opValue == 30)
                    return true;

                return false;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static decimal? MarkUpRateDivisionLimit_Set(decimal? MarkupRate, int MarkUpType, decimal? ListPrice, decimal? VendorPrice, decimal? ItmPriceAfter)
        {
            try
            {
                //Note for Division Marup the value must be within [0-0.999999]
                if (MarkUpType == 30 && (GFunc.IsNEZ(ListPrice) || GFunc.IsNEZ(ItmPriceAfter)))
                    return 0M;

                if (MarkUpType == 40 && (GFunc.IsNEZ(VendorPrice) || GFunc.IsNEZ(ItmPriceAfter)))
                    return 0M;

                if (MarkupRate >= 1)
                    return 0.999999M;
                else if (GFunc.NEDec(MarkupRate, 0) < 0)
                    return 0M;
                else
                    return MarkupRate;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted

        public static bool PriceCurrencyPosition_Get(ref int? CurrPos, int? DocCurrKey)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return PriceCurrencyPosition_Get(cn, ref CurrPos, DocCurrKey);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static bool PriceCurrencyPosition_Get(SqlConnection cn, ref int? CurrPos, int? DocCurrKey)
        {
            CurrPos = 1;
            try
            {
                //Retrieve the Item Currency postion from SYS_Option
                for (; CurrPos <= 15; CurrPos++)
                {
                    if (DocCurrKey == SysOptionUtility.GetInt("ItemPriceCurr" + CurrPos, cn))
                        break;
                }

                if (CurrPos > 15)
                {
                    if (SysOptionUtility.GetBool("WarnCurrencyPositionNotValid", cn))
                    {
                        MsgBox.Show(cn, MsgID.Document.CannotMatchCurrencyPosition);
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static bool PriceInfor_Get(int? PriceKey, ref int? PriceCode, ref int? PriceType, ref int? PriceCurrKey)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return PriceInfor_Get(cn, PriceKey, ref PriceCode, ref PriceType, ref PriceCurrKey);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static bool PriceInfor_Get(SqlConnection cn, int? PriceKey, ref int? PriceCode, ref int? PriceType, ref int? PriceCurrKey)
        {
            MSTPriceList objMSTPriceList = null;
            PriceCode = 0;
            PriceType = 0;
            PriceCurrKey = 0;

            try
            {
                if (GFunc.IsNEZ(PriceKey))
                {
                    return false;
                }

                //Get Built In Code from MST_PriceList
                objMSTPriceList = MSTPriceList.Get(cn, PriceKey);
                if (objMSTPriceList.PriceKey == null)
                {
                    return false;
                }
                else
                {
                    PriceCode = objMSTPriceList.BuildInCode.Value;
                    PriceType = objMSTPriceList.PriceType.Value;
                    PriceCurrKey = objMSTPriceList.CurrKey.Value;
                    return true;
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                objMSTPriceList = null;
            }
        }//CodeCompleted
        public static decimal PriceByMSTItmDet_Get(int? PriceListCode, int? ItmKey, int? CurrKey)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return PriceByMSTItmDet_Get(cn, PriceListCode, ItmKey, CurrKey);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static decimal PriceByMSTItmDet_Get(SqlConnection cn, int? PriceListCode, int? ItmKey, int? CurrKey)
        {
            string PriceProperty = string.Empty;
            MSTItmDetPrice objDetPrice = null;
            int? CurrPos = 0;

            try
            {

                //Return 0 when the paramter value is invalid
                if (GFunc.IsNEZ(PriceListCode) || GFunc.IsNEZ(ItmKey) || GFunc.IsNEZ(CurrKey))
                {
                    return 0;
                }

                //Get Price Currency Position from System Option
                if (PriceCurrencyPosition_Get(cn, ref CurrPos, CurrKey))
                {
                    //return 0 when no record from MSTItmDetPrice
                    objDetPrice = MSTItmDetPrice.Get(cn, ItmKey);
                    if (objDetPrice.ItmKey == null)
                        return 0;
                }
                else
                    return 0;

                //Get property name based on Price Type
                switch ((int)PriceListCode)
                {
                    //StandardPrice1 or ...or StandardPrice15
                    case (int)GEnum.PriceListCode.UseStandardPrice:
                        PriceProperty = "StandardPrice" + CurrPos;
                        break;

                    //StandardCost1 or ...or StandardCost15
                    case (int)GEnum.PriceListCode.UseStandardCost:
                        PriceProperty = "StandardCost" + CurrPos;
                        break;

                    //if iCurrencyPos=1 -> Price0101, if iCurrencyPos=15 -> Price1501
                    case (int)GEnum.PriceListCode.UsePrice1:
                        PriceProperty = "Price01" + CurrPos.Value.ToString("00");
                        break;
                    case (int)GEnum.PriceListCode.UsePrice2:
                        PriceProperty = "Price02" + CurrPos.Value.ToString("00");
                        break;
                    case (int)GEnum.PriceListCode.UsePrice3:
                        PriceProperty = "Price03" + CurrPos.Value.ToString("00");
                        break;
                    case (int)GEnum.PriceListCode.UsePrice4:
                        PriceProperty = "Price04" + CurrPos.Value.ToString("00");
                        break;
                    case (int)GEnum.PriceListCode.UsePrice5:
                        PriceProperty = "Price05" + CurrPos.Value.ToString("00");
                        break;
                    case (int)GEnum.PriceListCode.UsePrice6:
                        PriceProperty = "Price06" + CurrPos.Value.ToString("00");
                        break;
                    case (int)GEnum.PriceListCode.UsePrice7:
                        PriceProperty = "Price07" + CurrPos.Value.ToString("00");
                        break;
                    case (int)GEnum.PriceListCode.UsePrice8:
                        PriceProperty = "Price08" + CurrPos.Value.ToString("00");
                        break;
                    case (int)GEnum.PriceListCode.UsePrice9:
                        PriceProperty = "Price09" + CurrPos.Value.ToString("00");
                        break;
                    case (int)GEnum.PriceListCode.UsePrice10:
                        PriceProperty = "Price10" + CurrPos.Value.ToString("00");
                        break;
                    case (int)GEnum.PriceListCode.UsePrice11:
                        PriceProperty = "Price11" + CurrPos.Value.ToString("00");
                        break;
                    case (int)GEnum.PriceListCode.UsePrice12:
                        PriceProperty = "Price12" + CurrPos.Value.ToString("00");
                        break;
                    case (int)GEnum.PriceListCode.UsePrice13:
                        PriceProperty = "Price13" + CurrPos.Value.ToString("00");
                        break;
                    case (int)GEnum.PriceListCode.UsePrice14:
                        PriceProperty = "Price14" + CurrPos.Value.ToString("00");
                        break;
                    case (int)GEnum.PriceListCode.UsePrice15:
                        PriceProperty = "Price15" + CurrPos.Value.ToString("00");
                        break;
                    default:
                        //return when unable to generate property name based on Price Type
                        return 0;
                }

                //Retrieve the value of above property
                PropertyInfo propertyInfo = null;
                object propertyValue = null;

                propertyInfo = objDetPrice.GetType().GetProperty(PriceProperty);

                if (!GFunc.IsNE(propertyInfo))
                    propertyValue = propertyInfo.GetValue(objDetPrice, null);

                if (propertyValue != null)
                {
                    return GFunc.NEDec(propertyValue, 0);
                }
                else
                    return 0;

            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                objDetPrice = null;
            }
        }//CodeCompleted
        public static decimal PriceByTransaction_Get(int? PriceListCode, int? ItmKey, int? ConKey, int? CurrKey)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return PriceByTransaction_Get(cn, PriceListCode, ItmKey, ConKey, CurrKey);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static decimal PriceByTransaction_Get(SqlConnection cn, int? PriceListCode, int? ItmKey, int? ConKey, int? CurrKey)
        {
            DataTable dt = null;
            try
            {
                //Is Paramter valid
                if (GFunc.IsNEZ(PriceListCode) || GFunc.IsNEZ(ItmKey) || GFunc.IsNEZ(CurrKey))
                {
                    return 0;
                }

                //Get Transaction price
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@CurrKey", CurrKey));
                parmList.Add(new SqlParameter("@ItmKey", ItmKey));
                parmList.Add(new SqlParameter("@PriceListCode", PriceListCode));
                parmList.Add(new SqlParameter("@ConKey", GFunc.NEInt(ConKey, 0)));
                parmList.Add(new SqlParameter("@resultValid", 0));

                parmList[4].Direction = ParameterDirection.Output;
                dt = GFunc.ExecuteProc(cn, "ROPriceByTrans", parmList);

                if (Convert.ToInt32(parmList[4].Value) <= 0 || dt.Rows.Count < 0)
                {
                    if (SysOptionUtility.GetBool("WarnPriceByTranInValid"))
                    {
                        MsgBox.Show(cn, "No Previous Price!");
                    }
                    return 0;
                }
                else if (dt.Rows.Count > 0)
                    return GFunc.NEDec(dt.Rows[0]["LastPrice"], 0);

                else
                {
                    if (SysOptionUtility.GetBool("WarnPriceByTranInValid"))
                    {
                        MsgBox.Show(cn, "No record found");
                    }
                    return 0;
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                dt = null;
            }
        }//CodeCompleted

        //For Assembly Child Item
        public static decimal PriceByTransaction_Get(SqlConnection cn, int? PriceListCode, int? ItmKey, int? ConKey, int? CurrKey, Boolean ShowMsg)
        {
            DataTable dt = null;
            try
            {
                //Is Paramter valid
                if (GFunc.IsNEZ(PriceListCode) || GFunc.IsNEZ(ItmKey) || GFunc.IsNEZ(CurrKey))
                {
                    return 0;
                }

                //Get Transaction price
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@CurrKey", CurrKey));
                parmList.Add(new SqlParameter("@ItmKey", ItmKey));
                parmList.Add(new SqlParameter("@PriceListCode", PriceListCode));
                parmList.Add(new SqlParameter("@ConKey", GFunc.NEInt(ConKey, 0)));
                parmList.Add(new SqlParameter("@resultValid", 0));

                parmList[4].Direction = ParameterDirection.Output;
                dt = GFunc.ExecuteProc(cn, "ROPriceByTrans", parmList);

                if (Convert.ToInt32(parmList[4].Value) <= 0 || dt.Rows.Count < 0)
                {
                    if (ShowMsg)
                    {
                        if (SysOptionUtility.GetBool("WarnPriceByTranInValid"))
                        {
                            MsgBox.Show(cn, "Getting Transaction Last Price Fail!");
                        }
                    }
                    return 0;
                }
                else if (dt.Rows.Count > 0)
                    return GFunc.NEDec(dt.Rows[0]["LastPrice"], 0);

                else
                {
                    if (ShowMsg)
                    {
                        if (SysOptionUtility.GetBool("WarnPriceByTranInValid"))
                        {
                            MsgBox.Show(cn, "No record found");
                        }
                    }
                    return 0;
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                dt = null;
            }
        }//CodeCompleted
        public static bool PriceByValue_Get(int? PriceKey, int? ItmKey, int? CurrKey, ref decimal? ItmPrice, ref decimal? ItmQty, ref string ItmDes)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return PriceByValue_Get(cn, PriceKey, ItmKey, CurrKey, ref ItmPrice, ref ItmQty, ref ItmDes);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static bool PriceByValue_Get(SqlConnection cn, int? PriceKey, int? ItmKey, int? CurrKey, ref decimal? ItmPrice, ref decimal? ItmQty, ref string ItmDes)
        {
            DataTable dt = null;
            ItmPrice = 0;

            try
            {
                //Is parameters valid
                if (GFunc.IsNEZ(PriceKey))
                {
                    return false;
                }

                if (GFunc.IsNEZ(ItmKey) || GFunc.IsNEZ(CurrKey))
                {
                    return false;
                }

                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@CurrKey", CurrKey));
                parmList.Add(new SqlParameter("@ItmKey", ItmKey));
                parmList.Add(new SqlParameter("@PriceKey", PriceKey));

                dt = GFunc.ExecuteProc(cn, "ROMSTPriceListDetValue_Get", parmList);

                if (dt.Rows.Count > 0)
                {
                    if (GFunc.IsNE(dt.Rows[0]["EffStartDate"]) || GFunc.IsNE(dt.Rows[0]["EffEndDate"]))
                    {
                        ItmPrice = GFunc.NEDec(dt.Rows[0]["ItmPrice"], 0);
                        ItmQty = GFunc.NEDec(dt.Rows[0]["ItmQty"], 0);
                    }
                    else
                    {
                        if (Convert.ToDateTime(dt.Rows[0]["EffStartDate"]) <= DateTime.Today && Convert.ToDateTime(dt.Rows[0]["EffEndDate"]) >= DateTime.Today)
                        {
                            ItmPrice = GFunc.NEDec(dt.Rows[0]["EffItmPrice"], 0);
                            ItmQty = GFunc.NEDec(dt.Rows[0]["EffItmQty"], 0);
                        }
                    }
                    ItmDes = dt.Rows[0]["ItmDes"].ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                dt = null;
            }
        }//CodeCompleted

        public static bool PriceByValueVendorInfo_Get(SqlConnection cn, int? PriceKey, int? ItmKey,int? ConKey, int? CurrKey,ref int? VendorKey, ref decimal? VendorPrice, ref string VendorItmDes)
        {
            DataTable dt = null;
            VendorPrice = 0;

            try
            {
                //Is parameters valid
                if (GFunc.IsNEZ(PriceKey))
                {
                    return false;
                }

                if (GFunc.IsNEZ(ItmKey) || GFunc.IsNEZ(CurrKey))
                {
                    return false;
                }

                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@CurrKey", CurrKey));
                parmList.Add(new SqlParameter("@ItmKey", ItmKey));
                parmList.Add(new SqlParameter("@PriceKey", PriceKey));
                parmList.Add(new SqlParameter("@VendorKey", ConKey));

                dt = GFunc.ExecuteProc(cn, "ROMSTPriceListDetValueVendor_Get", parmList);

                if (dt.Rows.Count > 0)
                {

                    VendorPrice = GFunc.NEDec(dt.Rows[0]["VendorPrice"], 0);
                    VendorKey = GFunc.NEInt(dt.Rows[0]["VendorKey"], 0);
 
                    VendorItmDes  = dt.Rows[0]["VendorItmDes"].ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                dt = null;
            }
        }//CodeCompleted
        public static bool PriceByRatio_Get(int? PriceKey, int? ItmKey, int? CurrKey, ref decimal? ItmPrice)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return PriceByRatio_Get(cn, PriceKey, ItmKey, CurrKey, ref ItmPrice);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static bool PriceByRatio_Get(SqlConnection cn, int? PriceKey, int? ItmKey, int? CurrKey, ref decimal? ItmPrice)
        {
            int? CurrPos = 0;
            DataTable dt;
            ItmPrice = 0;

            try
            {
                //Is parameters valid
                if (GFunc.IsNEZ(PriceKey))
                    return false;

                if (GFunc.IsNEZ(ItmKey) || GFunc.IsNEZ(CurrKey))
                {
                    return false;
                }

                //Get Price Currency Position from System Option
                if (PriceCurrencyPosition_Get(cn, ref CurrPos, CurrKey))
                {
                    return false;
                }

                //Get Transaction price
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@ItmKey", ItmKey));
                parmList.Add(new SqlParameter("@PriceKey", PriceKey));
                parmList.Add(new SqlParameter("@DocDate", DateTime.Today.Date));
                parmList.Add(new SqlParameter("@@currencyPosition", CurrPos));

                dt = GFunc.ExecuteProc(cn, "ROPriceByRatio", parmList);

                if (dt.Rows.Count > 0)
                {
                    ItmPrice = GFunc.NEDec(dt.Rows[0]["Price"], 0);
                    return true;
                }
                else
                {
                    if (SysOptionUtility.GetBool("WarnPriceByTranInValid", cn))
                    {
                        MsgBox.Show(cn, MsgID.Common.UnableToGetPriceRatio);
                    }
                    return false;
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                dt = null;
            }
        }//CodeCompleted
        public static bool Price_Get(int? PriceKey, int? ItmKey, int? ConKey, int? CurrKey, string DocMode, ref decimal? ItmPrice, ref decimal? ItmQty, ref string ItmDes)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return Price_Get(cn, PriceKey, ItmKey, ConKey, CurrKey, DocMode, ref ItmPrice, ref ItmQty, ref ItmDes);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static bool Price_Get(SqlConnection cn, int? PriceKey, int? ItmKey, int? ConKey, int? CurrKey, string DocMode, ref decimal? ItmPrice, ref decimal? ItmQty, ref string ItmDes)
        {
            int? PriceListCode = 0;
            int? PriceListCurrKey = 0;
            int? PriceListType = 0;
            ItmPrice = 0;

            try
            {
                //Is parameters valid
                if (GFunc.IsNE(ItmKey) || GFunc.IsNE(CurrKey))
                {
                    return false;
                }

                //Use StandardCost when there are no PriceType available
                if (GFunc.IsNE(PriceKey))
                {
                    //Item Price List
                    PriceListCode = DocMode == "AR" ? (int)GEnum.PriceListCode.UseStandardPrice : (int)GEnum.PriceListCode.UseStandardCost;
                    ItmPrice = PriceByMSTItmDet_Get(cn, PriceListCode, ItmKey, CurrKey);
                    return true;
                }

                //Retrieve BuildInCode, PriceType, PriceCurrKey by Price Key
                if (PriceInfor_Get(cn, PriceKey, ref PriceListCode, ref PriceListType, ref PriceListCurrKey) == false)
                {
                    return false;
                }

                if ((int?)PriceListCode == 10)
                {
                    //Price List
                    if (CurrKey != PriceListCurrKey)
                    {
                        return false;
                    }
                    else
                    {
                        if (PriceListType == 10)
                        {
                            //Price by Value
                            if (PriceByValue_Get(cn, PriceKey, ItmKey, CurrKey, ref ItmPrice, ref ItmQty, ref ItmDes) == false)
                            {
                                PriceListCode = DocMode == "AR" ? (int)GEnum.PriceListCode.UseStandardPrice : (int)GEnum.PriceListCode.UseStandardCost;
                                ItmPrice = PriceByMSTItmDet_Get(cn, PriceListCode, ItmKey, CurrKey);
                            }
                        }
                        else
                        {
                            //Price by Ratio
                            if (PriceByRatio_Get(cn, PriceKey, ItmKey, CurrKey, ref ItmPrice) == false)
                            {
                                PriceListCode = DocMode == "AR" ? (int)GEnum.PriceListCode.UseStandardPrice : (int)GEnum.PriceListCode.UseStandardCost;
                                ItmPrice = PriceByMSTItmDet_Get(cn, PriceListCode, ItmKey, CurrKey);
                            }
                        }
                        return true;
                    }
                }
                else if ((int?)PriceListCode > 99 && (int?)PriceListCode < 999)
                {
                    //Transaction Price
                    ItmPrice = PriceByTransaction_Get(cn, (int?)PriceListCode, ItmKey, ConKey, CurrKey);
                    return true;
                }
                else
                {
                    //Item Price List
                    ItmPrice = PriceByMSTItmDet_Get(cn, PriceListCode, ItmKey, CurrKey);
                    return true;
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static decimal Price_Get(int? PriceKey, int? ItmKey, int? ConKey, int? CurrKey)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return Price_Get(cn, PriceKey, ItmKey, ConKey, CurrKey);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted

        public static bool PriceVendorInfo_Get(int? PriceKey, int? ItmKey, int? ConKey, int? CurrKey, string DocMode, ref int? VendorKey, ref decimal? VendorPrice, ref string VendorItmDes)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return PriceVendorInfo_Get(cn, PriceKey, ItmKey, ConKey, CurrKey, DocMode,ref VendorKey, ref VendorPrice,ref VendorItmDes);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static bool PriceVendorInfo_Get(SqlConnection cn, int? PriceKey, int? ItmKey, int? ConKey, int? CurrKey, string DocMode,ref int? VendorKey, ref decimal? VendorPrice,ref string VendorItmDes)
        {
            int? PriceListCode = 0;
            int? PriceListCurrKey = 0;
            int? PriceListType = 0;
           

            try
            {
                //Is parameters valid
                if (GFunc.IsNE(ItmKey) || GFunc.IsNE(CurrKey))
                {
                    return false;
                }

                //Use StandardCost when there are no PriceType available
                if (GFunc.IsNE(PriceKey))
                {
                    //Item Price List
                    PriceListCode = DocMode == "AR" ? (int)GEnum.PriceListCode.UseStandardPrice : (int)GEnum.PriceListCode.UseStandardCost;
                    
                    return true;
                }

                //Retrieve BuildInCode, PriceType, PriceCurrKey by Price Key
                if (PriceInfor_Get(cn, PriceKey, ref PriceListCode, ref PriceListType, ref PriceListCurrKey) == false)
                {
                    return false;
                }

                if ((int?)PriceListCode == 10)
                {
                    //Price List
                    if (CurrKey != PriceListCurrKey)
                    {
                        return false;
                    }
                    else
                    {
                        if (PriceListType == 10)
                        {
                            //Price by Value
                            return PriceByValueVendorInfo_Get(cn, PriceKey, ItmKey, ConKey, CurrKey, ref VendorKey, ref VendorPrice, ref VendorItmDes);
                        }

                        return true;
                    }
                }
                else { return false; }

                return true;
               
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        //For Assemblly Child Item
        public static decimal Price_Get(int? PriceKey, int? ItmKey, int? ConKey, int? CurrKey, Boolean ShowMsg)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return Price_Get(cn, PriceKey, ItmKey, ConKey, CurrKey, ShowMsg);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static decimal Price_Get(SqlConnection cn, int? PriceKey, int? ItmKey, int? ConKey, int? CurrKey)
        {
            int? PriceListCode = 0;
            int? PriceListCurrKey = 0;
            int? PriceListType = 0;
            decimal? ItmPrice = 0;            //value is never return to caller, sole purpose is for filling the paramenter requirements of calling other func
            decimal? ItmQty = 0;            //value is never return to caller, sole purpose is for filling the paramenter requirements of calling other func
            string ItmDes = string.Empty;   //value is never return to caller, sole purpose is for filling the paramenter requirements of calling other func

            try
            {
                //Is parameters valid
                if (GFunc.IsNEZ(PriceKey) || GFunc.IsNEZ(ItmKey) || GFunc.IsNEZ(CurrKey))
                    return 0;

                //Retrieve BuildInCode, PriceType, PriceCurrKey by Price Key
                if (PriceInfor_Get(cn, PriceKey, ref PriceListCode, ref PriceListType, ref PriceListCurrKey) == false)
                    return 0;

                if ((int?)PriceListCode == 10)
                {
                    //Price List
                    if (CurrKey != PriceListCurrKey)
                        return 0;
                    else
                    {
                        if (PriceListType == 10)
                        {
                            //Price by Value
                            if (PriceByValue_Get(cn, PriceKey, ItmKey, CurrKey, ref ItmPrice, ref ItmQty, ref ItmDes))
                                return GFunc.NEDec(ItmPrice, 0);
                            else
                                return 0;
                        }
                        else
                        {
                            //Price by Ratio
                            if (PriceByRatio_Get(cn, PriceKey, ItmKey, CurrKey, ref ItmPrice))
                                return GFunc.NEDec(ItmPrice, 0);
                            else
                                return 0;
                        }
                    }
                }
                else if ((int?)PriceListCode > 99 && (int?)PriceListCode < 999)
                {
                    //Transaction Price
                    return PriceByTransaction_Get(cn, (int?)PriceListCode, ItmKey, ConKey, CurrKey);
                }
                else
                {
                    //Item Price List
                    return PriceByMSTItmDet_Get(cn, PriceListCode, ItmKey, CurrKey);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static decimal Price_Get(SqlConnection cn, int? PriceKey, int? ItmKey, int? ConKey, int? CurrKey, Boolean ShowMsg)
        {
            int? PriceListCode = 0;
            int? PriceListCurrKey = 0;
            int? PriceListType = 0;
            decimal? ItmPrice = 0;            //value is never return to caller, sole purpose is for filling the paramenter requirements of calling other func
            decimal? ItmQty = 0;            //value is never return to caller, sole purpose is for filling the paramenter requirements of calling other func
            string ItmDes = string.Empty;   //value is never return to caller, sole purpose is for filling the paramenter requirements of calling other func

            try
            {
                //Is parameters valid
                if (GFunc.IsNEZ(PriceKey) || GFunc.IsNEZ(ItmKey) || GFunc.IsNEZ(CurrKey))
                    return 0;

                //Retrieve BuildInCode, PriceType, PriceCurrKey by Price Key
                if (PriceInfor_Get(cn, PriceKey, ref PriceListCode, ref PriceListType, ref PriceListCurrKey) == false)
                    return 0;

                if ((int?)PriceListCode == 10)
                {
                    //Price List
                    if (CurrKey != PriceListCurrKey)
                        return 0;
                    else
                    {
                        if (PriceListType == 10)
                        {
                            //Price by Value
                            if (PriceByValue_Get(cn, PriceKey, ItmKey, CurrKey, ref ItmPrice, ref ItmQty, ref ItmDes))
                                return GFunc.NEDec(ItmPrice, 0);
                            else
                                return 0;
                        }
                        else
                        {
                            //Price by Ratio
                            if (PriceByRatio_Get(cn, PriceKey, ItmKey, CurrKey, ref ItmPrice))
                                return GFunc.NEDec(ItmPrice, 0);
                            else
                                return 0;
                        }
                    }
                }
                else if ((int?)PriceListCode > 99 && (int?)PriceListCode < 999)
                {
                    //Transaction Price
                    return PriceByTransaction_Get(cn, (int?)PriceListCode, ItmKey, ConKey, CurrKey, ShowMsg);
                }
                else
                {
                    //Item Price List
                    return PriceByMSTItmDet_Get(cn, PriceListCode, ItmKey, CurrKey);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static decimal? PTTax(decimal? Value, decimal? Tax, decimal? TaxGrp, string CreditORDebit, string CreditORDebitColumn)
        {
            try
            {
                decimal? Result = 0;
                decimal? a = 0;
                int b = 0;
                decimal? c = 0;

                if (Value == 0 || Tax == 0 || TaxGrp == 0)
                    Result = 0;
                else
                {
                    if (CreditORDebit == CreditORDebitColumn)
                        b = 1;
                    else
                        b = -1;

                    c = Tax / TaxGrp;
                    a = Value / c * b;

                    if (a >= 0)
                        Result = GFunc.RndC(a, GVar.RndDecs.Amtpt);
                    else
                        Result = 0;
                }
                return Result;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static decimal? PT(decimal? Value, string CreditORDebit, string CreditORDebitColumn)
        {
            try
            {
                decimal? Result = 0;
                int b = 0;

                if (CreditORDebit == CreditORDebitColumn)
                    b = 1;
                else
                    b = -1;

                Result = GFunc.RndC(Value * b, GVar.RndDecs.Amtpt);
                if (Result < 0)
                    Result = 0;
                return Result;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted

        public static decimal QtyDiscount_Get(int? PriceKey, int? ItmKey, decimal? ItmBQty)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return QtyDiscount_Get(cn, PriceKey, ItmKey, ItmBQty);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static decimal QtyDiscount_Get(SqlConnection cn, int? PriceKey, int? ItmKey, decimal? ItmBQty)
        {
            MSTItm objItm = null;
            MSTItmDetPrice objItmDetPrice = null;
            int? PriceCode = 0;
            int? PriceType = 0;
            int? PriceCurrKey = 0;

            try
            {
                //Is paramter valid
                if (GFunc.IsNEZ(ItmKey) || GFunc.IsNE(ItmBQty))
                    return 0;

                if (PriceInfor_Get(cn, PriceKey, ref PriceCode, ref PriceType, ref PriceCurrKey) == false)
                    return 0;

                if (PriceCode < 1000)// NOT USING(Standard Price or Price 1 to 15 or Standard Cost)
                    return 0;

                if (SysOptionUtility.GetBool(GVar.SystemOption.OpID.UseQtyDiscountPricing, cn))
                {
                    //Get MSTItm
                    objItm = MSTItm.Get(cn, ItmKey);
                    if (objItm.ItmKey == null)
                        return 0;

                    switch (GFunc.GetINTypeGroup(objItm.ItmType))
                    {
                        case (int)GEnum.INTypeGrp.Stock:
                        case (int)GEnum.INTypeGrp.Non_Stock:
                            objItmDetPrice = MSTItmDetPrice.Get(cn, ItmKey);
                            if (objItmDetPrice.ItmKey == null)
                                return 0;
                            else
                            {
                                //Try to get the discount ratio in which the Qty fall
                                if (objItmDetPrice.QtyDisQty1 <= ItmBQty && objItmDetPrice.QtyDisQty1 > 0)
                                    if (objItmDetPrice.QtyDisQty2 <= ItmBQty && objItmDetPrice.QtyDisQty2 > objItmDetPrice.QtyDisQty1)
                                        if (objItmDetPrice.QtyDisQty3 <= ItmBQty && objItmDetPrice.QtyDisQty3 > objItmDetPrice.QtyDisQty2)
                                            if (objItmDetPrice.QtyDisQty4 <= ItmBQty && objItmDetPrice.QtyDisQty4 > objItmDetPrice.QtyDisQty3)
                                                if (objItmDetPrice.QtyDisQty5 <= ItmBQty && objItmDetPrice.QtyDisQty5 > objItmDetPrice.QtyDisQty4)
                                                    return objItmDetPrice.QtyDisRatio5.Value;
                                                else
                                                    return objItmDetPrice.QtyDisRatio4.Value;
                                            else
                                                return objItmDetPrice.QtyDisRatio3.Value;
                                        else
                                            return objItmDetPrice.QtyDisRatio2.Value;
                                    else
                                        return objItmDetPrice.QtyDisRatio1.Value;
                                else
                                    return 0;
                            }

                        default:
                            return 0;
                    }
                }
                else
                    return 0;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                objItm = null;
                objItmDetPrice = null;
            }

        }//CodeCompleted
        public static decimal OverHeadCost_Get(int? OverHeadKey)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return OverHeadCost_Get(cn, OverHeadKey);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static decimal OverHeadCost_Get(SqlConnection cn, int? OverHeadKey)
        {
            REFOverHead objOverhead = null;

            try
            {
                if (GFunc.IsNE(OverHeadKey))
                {
                    MsgBox.Show(cn, MsgID.Common.InvalidParameters + "%OverHeadCost_Get");
                }

                objOverhead = REFOverHead.Get(cn, (int?)OverHeadKey);
                if (objOverhead == null)
                {
                    return 0;
                }
                else
                {
                    return GFunc.NEDec(objOverhead.OverHeadCost, 0);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                objOverhead = null;
            }
        }//CodeCompleted
        public static DataTable SvrData_Get(int? docCodeKey, int? docKey)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return SvrData_Get(cn, docCodeKey, docKey);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static DataTable SvrData_Get(SqlConnection cn, int? docCodeKey, int? docKey)
        {
            try
            {
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@DocCodeKey", docCodeKey));
                parmList.Add(new SqlParameter("@DocKey", docKey));

                DataTable dtSvr = GFunc.ExecuteProc(cn, "Doc_SvrData_Get", parmList);

                return dtSvr;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static decimal TaxGrpRate_Get(int? taxGrpKey, DateTime? docDate)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return TaxGrpRate_Get(cn, taxGrpKey, docDate);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static decimal TaxGrpRate_Get(SqlConnection cn, int? taxGrpKey, DateTime? docDate)
        {
            try
            {
                if (!GFunc.IsNEZ(taxGrpKey))
                {
                    List<SqlParameter> parmList = new List<SqlParameter>();
                    parmList.Add(new SqlParameter("@TaxGrp", taxGrpKey));
                    parmList.Add(new SqlParameter("@DocDate", docDate));

                    DataTable dt = GFunc.ExecuteProc(cn, "ROREFTaxRate_Get", parmList);

                    if (dt.Rows.Count > 0)
                    {
                        return GFunc.NEDec(dt.Rows[0][0], 0);
                    }
                }
                return 0;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static decimal UOMConRate_Get(int? ItmKey, int? UOMKey,int? CallerDocCodeKey=0)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return UOMConRate_Get(cn, ItmKey, UOMKey, CallerDocCodeKey);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static decimal UOMConRate_Get(SqlConnection cn, int? ItmKey, int? UOMKey,int? CallerDocCodeKey)
        {
            MSTItm objItm = null;
            REFUOMDetItms objREFUOMDetItms = null;
            int opValue = 0;
            try
            {
                //Invalid parameter
                if (GFunc.IsNEZ(ItmKey) || GFunc.IsNEZ(UOMKey))
                    return 1;

                //Get UOMConRate
                objItm = MSTItm.Get(cn, ItmKey);
                if (objItm == null)
                    return 1;
                else
                {
                    if (objItm.BUOMKey == UOMKey)
                        return 1;
                    else
                    {
                        objREFUOMDetItms = REFUOMDetItms.Get(cn, objItm.BUOMKey, UOMKey);
                        if (objREFUOMDetItms.Rows.Count > 0)
                            return GFunc.NEDec(objREFUOMDetItms.Rows[0]["UOMConRate"], 0);
                        else
                        {
                            //Display warning message when no conversion rate available
                            opValue = SysOptionUtility.GetInt("WarnNonConvertableUOM", cn);

                            if (CallerDocCodeKey != (int?)GEnum.SystemCode.DO_to_IV_Transfer )//Added by Jane. when transfer DO to invoice, uom rate is not supposed to check.
                            {
                                switch (GFunc.GetINTypeGroup(objItm.ItmType))
                                {
                                    case (int)GEnum.INTypeGrp.Stock:
                                        //Stock only or Stock or NonStock
                                        if (opValue == 20 || opValue == 30)
                                            MsgBox.Show(cn, MsgID.Document.NoUOMConversionRate);
                                        break;

                                    case (int)GEnum.INTypeGrp.Non_Stock:
                                        //Stock or NonStock
                                        if (opValue == 30)
                                            MsgBox.Show(cn, MsgID.Document.NoUOMConversionRate);
                                        break;
                                }
                            }
                            return 1;
                        }
                    }
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                objItm = null;
                objREFUOMDetItms = null;
            }
        }//CodeCompleted
        public static decimal UOMGramRate_Get(int? UOMKey)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return UOMGramRate_Get(cn, UOMKey);
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted
        public static decimal UOMGramRate_Get(SqlConnection cn, int? UOMKey)
        {
            REFUOMDetItms objREFUOMDetItms = null;

            try
            {
                objREFUOMDetItms = REFUOMDetItms.Get(cn, UOMKey, 3, 0);
                if (objREFUOMDetItms.Rows.Count > 0)
                    return GFunc.NEDec(objREFUOMDetItms.Rows[0]["GramRate"], 1) == 0 ? 1 : GFunc.NEDec(objREFUOMDetItms.Rows[0]["GramRate"], 1);
                else
                    return 1;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                objREFUOMDetItms = null;
            }
        }//CodeCompleted

        //Notifier
        public static void Notifier_CtrlSearch(Control cons, BOLib.UINotifierEventArgs e, ErrorProvider errorProvider1)
        {
            try
            {
                string propertyNm = string.Empty;
                string conNm = string.Empty;
                Control errCon = null;

                foreach (object key in e.PropertyMessage.Keys)
                {
                    //Prevent Index Out of Range 'Tmp'
                    Control[] Tmp = cons.Controls.Find(key.ToString(), true);
                    errCon = (Tmp.Length > 0) ? Tmp[0] : new Control();

                    if (GFunc.IsNE(errCon) == false)
                    {
                        errorProvider1.SetError(errCon, e.PropertyMessage[key].ToString());
                    }
                }
                if (errCon != null)
                {
                    SelectParentTab(errCon);
                    errCon.Focus();
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//CodeCompleted

        //used in Notifier_CtrlSearch function.
        //Purpose : When Validation failed, to open the tab where the error control exists if there is parent Tab.      
        private static void SelectParentTab(Control c)
        {
            Control cParent = c.Parent;
            while (cParent != null)
            {
                if (cParent.GetType() == typeof(Form))
                    break;
                else if (cParent.GetType() == typeof(Infragistics.Win.UltraWinTabControl.UltraTabPageControl))
                {
                    ((Infragistics.Win.UltraWinTabControl.UltraTabPageControl)cParent).Tab.Selected = true;
                    break;
                }
                cParent = cParent.Parent;
            }
        }

        public static void SelectParentTab(TAUtil.TAGridEditor grid)
        {
            SelectParentTab(grid as Control);
            grid.Focus();
        }

        public static void InvokeGridNotifier(string gridNm, UINotifierEventArgs e, GVar.UINotifierEvent errorNotifierHeader_Set)
        {
            if (errorNotifierHeader_Set != null)
            {
                e.PropertyMessage.Clear();
                e.PropertyMessage.Add("grid", gridNm);
                errorNotifierHeader_Set.Invoke(e, e);
            }
        }

        public static void ClearErrNotifier(Control cons, BOLib.UINotifierEventArgs e, ErrorProvider errorProvider1)
        {
            try
            {
                foreach (Control ctrl in cons.Controls)
                {
                    if (ctrl.HasChildren && GFunc.IsNE(ctrl.Controls[0].Name) == false)
                    {
                        ClearErrNotifier(ctrl, e, errorProvider1);
                    }
                    else
                    {

                        if ((GFunc.CompareString(GVar.ControlType.TATextBoxEditor, ctrl.GetType().Name)) ||
                    (GFunc.CompareString(GVar.ControlType.TADateEditor, ctrl.GetType().Name)) ||
                    (GFunc.CompareString(GVar.ControlType.TADateEditor, ctrl.GetType().Name)) ||
                    (GFunc.CompareString(GVar.ControlType.TANumericEditor, ctrl.GetType().Name)) ||
                    (GFunc.CompareString(GVar.ControlType.EmbeddableTextBoxWithUIPermissions, ctrl.GetType().Name)))
                        {

                            errorProvider1.SetError(ctrl, string.Empty);
                        }

                    }
                }
            }
            catch (TAException tex)
            {
                MsgBox.Show(tex.MsgID);
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
        }//CodeCompleted

        //Set Error Methods
        private static Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);

            }
            return ex;
        }//CodeCompleted
        private static TAException Error(TAException ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyTAException(ex, false);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }//CodeCompleted

        //Function has NOT been match with TBS  --------------------------PLEASE CREATE NEW FUNCTION BELOW THIS LINE-----------------------------------------------

        #region remove if not required
        //not used anymore ,replace with GridAutoID_Get(),
        //keep for reference
        //public static int DocItmKey_GetMax(UltraGrid tagrdDetItms, string headerColumnName, string detailColumnName)
        //{
        //    int maxDocItmKey = 0;
        //    int currentMaxDetailKey = 0;
        //    int currentHeaderKey = 0;
        //    int headerObjValue = 0;
        //    int detailObjValue = 0;
        //    TAUtil.TAGridEditor grd = (TAUtil.TAGridEditor)tagrdDetItms;
        //    //get last store values
        //    if (GFunc.IsNE(grd.HeaderObjectKey) == false)
        //    {
        //        int.TryParse(grd.HeaderObjectKey.ToString(), out headerObjValue);

        //    }
        //    if (GFunc.IsNE(grd.DetailObjectKey) == false)
        //    {
        //        int.TryParse(grd.DetailObjectKey.ToString(), out detailObjValue);

        //    }

        //    currentMaxDetailKey = (grd.DataSource as DataTable).Rows.Count == 0 ? 1 : (grd.DataSource as DataTable).AsEnumerable().Max(k => k.Field<int>(detailColumnName));
        //    currentHeaderKey = (grd.DataSource as DataTable).Rows.Count == 0 ? headerObjValue : (grd.DataSource as DataTable).AsEnumerable().Max(k => k.Field<int>(headerColumnName));

        //    maxDocItmKey = detailObjValue;

        //    //if current header key and last header key are not equal, we assume that is new Document object and reset DocKey
        //    if (headerObjValue != currentHeaderKey)
        //    {
        //        maxDocItmKey = currentMaxDetailKey;
        //        grd.HeaderObjectKey = currentHeaderKey.ToString();
        //        grd.DetailObjectKey = currentMaxDetailKey;
        //    }
        //    maxDocItmKey++;
        //    return maxDocItmKey;
        //}
        //public static void DocItmKey_SetMax(UltraGrid tagrdDetItms, string detailColumnName)
        //{
        //    int currRowDocItmKey = 0;
        //    int detailObjValue = 0;
        //    TAUtil.TAGridEditor grd = (TAUtil.TAGridEditor)tagrdDetItms;

        //    if (GFunc.IsNE(grd.DetailObjectKey) == false)
        //    {
        //        int.TryParse(grd.DetailObjectKey.ToString(), out detailObjValue);

        //    }
        //    if (GFunc.IsNE(grd.ActiveRow))
        //    {
        //        currRowDocItmKey = 0;
        //    }
        //    else
        //    {
        //        currRowDocItmKey = GFunc.NEInt(grd.ActiveRow.Cells[detailColumnName].Value, 0);
        //    }

        //    if (currRowDocItmKey > detailObjValue)
        //    {
        //        grd.DetailObjectKey = currRowDocItmKey;
        //    }

        //}
        //public static void DocItmKey_SetLastMax(UltraGrid tagrdDetItms, string detailColumnName)
        //{
        //    int currentMaxDetailKey = 0;
        //    TAUtil.TAGridEditor grd = (TAUtil.TAGridEditor)tagrdDetItms;

        //    currentMaxDetailKey = (grd.DataSource as DataTable).Rows.Count == 0 ? 1 : (grd.DataSource as DataTable).AsEnumerable().Max(k => k.Field<int>(detailColumnName));
        //    grd.DetailObjectKey = currentMaxDetailKey;

        //}
        //public static int DocItmKey_Get1(UltraGrid tagrdDetItms, string headerColumnName, string detailColumnName)
        //{
        //    int maxDocItmKey = 0;
        //    int currRowDocItmKey = 0;
        //    int currentHeaderKey = 0;
        //    int headerObjValue = 0;
        //    int detailObjValue = 0;
        //    TAUtil.TAGridEditor grd = (TAUtil.TAGridEditor)tagrdDetItms;
        //    if (GFunc.IsNE(grd.HeaderObjectKey) == false)
        //    {
        //        int.TryParse(grd.HeaderObjectKey.ToString(), out headerObjValue);

        //    }
        //    if (GFunc.IsNE(grd.DetailObjectKey) == false)
        //    {
        //        int.TryParse(grd.DetailObjectKey.ToString(), out detailObjValue);

        //    }
        //    if (GFunc.IsNE(grd.ActiveRow))
        //    {
        //        currRowDocItmKey = 0;
        //    }
        //    else
        //    {
        //        currRowDocItmKey = GFunc.NEInt(grd.ActiveRow.Cells[detailColumnName].Value, 0);
        //    }

        //    if (currRowDocItmKey <= 1)//only new row will increment)
        //    {
        //        maxDocItmKey = (grd.DataSource as DataTable).Rows.Count == 0 ? 1 : (grd.DataSource as DataTable).AsEnumerable().Max(k => k.Field<int>(detailColumnName));
        //        currentHeaderKey = (grd.DataSource as DataTable).Rows.Count == 0 ? 0 : (grd.DataSource as DataTable).AsEnumerable().Max(k => k.Field<int>(headerColumnName));
        //        if (detailObjValue > maxDocItmKey)
        //        {
        //            maxDocItmKey = detailObjValue;
        //        }
        //        //if current header key and last header key are not equal, we assume that is new Document object and reset DocKey
        //        if (headerObjValue != currentHeaderKey)
        //        {
        //            maxDocItmKey = 0;
        //            grd.HeaderObjectKey = currentHeaderKey.ToString();
        //        }
        //        maxDocItmKey++;
        //        grd.DetailObjectKey = maxDocItmKey;
        //    }
        //    else
        //    {
        //        maxDocItmKey = currRowDocItmKey;
        //        //if current header key and last header key are not equal, we assume that is new Document object and reset DocKey
        //        if (headerObjValue != currentHeaderKey)
        //        {
        //            maxDocItmKey = 1;
        //            grd.HeaderObjectKey = currentHeaderKey.ToString();
        //        }
        //        grd.DetailObjectKey = maxDocItmKey;
        //    }
        //    return maxDocItmKey;
        //}
        #endregion
    }
}