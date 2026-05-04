using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using TAUtil;

namespace BOLib
{
    public class SysOptionUtility
    {
        #region Properties

        public static int LanguageUsed
        {
            get;
            set;
        }
        //added by thettm on 18 jul 2018(start)
        public static string DatabaseBranchCode
        {
            get;
            set;
        }        
        public static bool DOManualCreation
        {
            get;
            set;
        }
        public static bool PDManualCreation
        {
            get;
            set;
        }
        //added by thettm on 18 jul 2018(end)
        public static int SearchCustomerNumChar
        {
            get;
            set;
        }
        public static int SearchVendorNumChar
        {
            get;
            set;
        }
        public static int SearchAccountNumChar
        {
            get;
            set;
        }
        public static int SearchJobNumChar
        {
            get;
            set;
        }
        public static int SearchItemNumChar
        {
            get;
            set;
        }
        public static GEnum.SearchMatchOption SearchCustomerMatch
        {
            get;
            set;
        }
        public static GEnum.SearchMatchOption SearchVendorMatch
        {
            get;
            set;
        }
        public static GEnum.SearchMatchOption SearchAccountMatch
        {
            get;
            set;
        }
        public static GEnum.SearchMatchOption SearchJobMatch
        {
            get;
            set;
        }
        public static GEnum.SearchMatchOption SearchItemMatch
        {
            get;
            set;
        }
        public static GEnum.SearchMatchOption SearchItemMatchID
        {
            get;
            set;
        }
        public static bool HasDMASLink
        {
            get;
            set;
        }
        public static bool UseDept
        {
            get;
            set;
        }
        public static bool UseProject
        {
            get;
            set;
        }
        public static bool UseBranch
        {
            get;
            set;
        }
        public static bool UseWMS
        {
            get;
            set;
        }
        public static int? CountryCurrency
        {
            get;
            set;
        }
        public static bool TaxCalculationOnTotal
        {
            get;
            set;
        }
        public static int BaseCurrency
        {
            get;
            set;
        }
        public static int FiscalStartPeriod
        {
            get;
            set;
        }
        //added by thettm on 17 jan 2019 (ProcessingFee)
        public static decimal ProcessingFee
        {
            get;
            set;
        }
        public static int ProcessingItem
        {
            get;
            set;
        }
        public static int InventoryValuationMethod
        {
            get;
            set;
        }
        public static int LastARAPRevaluationPeriod
        {
            get;
            set;
        }
        public static DateTime? LastYearEndedDate
        {
            get;
            set;
        }
        public static int LandedCostOption
        {
            get;
            set;
        }
        public static int StockCountStatus
        {
            get;
            set;
        }
        public static decimal StockCountItemTotal
        {
            get;
            set;
        }
        public static decimal StockCountItemCounted
        {
            get;
            set;
        }
        public static decimal StockCountItemRemaining
        {
            get;
            set;
        }
        public static int ShipMarkInitialNumber
        {
            get;
            set;
        }
        /* added by YST */
        public static string UOBBulkFilePath
        {
            get;
            set;
        }
        public static string UOBBulkFilePath_OMSMaster
        {
            get;
            set;
        }
        public static string ExportFilePath
        {
            get;
            set;
        } 
        /* end by YST */


        #endregion

        public static int GetInt(string opID)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return GetInt(opID, cn);
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static int GetInt(string opID, SqlConnection cn)
        {
            try
            {

                int retValue = 0;
                SYSOption objSYSOption = new SYSOption();
                objSYSOption.Fetch(cn, new SYSOption.Criteria(opID, AppInfor.currentUserKey, 1));

                if (!GFunc.IsNE(objSYSOption) && !GFunc.IsNE(objSYSOption.OpID))
                {
                    if (GFunc.CompareString(objSYSOption.OpDataType , "Integer")==false)
                    {
                        MsgBox.Show(cn, MsgID.Option.InvalidCallOption);
                        return retValue;
                    }

                    if (objSYSOption.OpValue == string.Empty)
                        objSYSOption.OpValue = objSYSOption.OpValueDefault;

                    if (Int32.TryParse(objSYSOption.OpValue, out retValue))
                    {
                        return retValue;
                    }
                    else
                    {
                        if (GFunc.CompareString(objSYSOption.ValidationRule , "NoRule")==false)
                            MsgBox.Show(cn,MsgID.Option.GetOptionFail);

                        return retValue;
                    }
                }
                else
                {
                    MsgBox.Show(cn, MsgID.Option.GetOptionFail);
                    return retValue;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static int? GetNullInt(string opID)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return GetNullInt(opID, cn);
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static int? GetNullInt(string opID, SqlConnection cn)
        {
            int retValue = 0;
            try
            {
                SYSOption objSYSOption = new SYSOption();
                objSYSOption.Fetch(cn, new SYSOption.Criteria(opID, AppInfor.currentUserKey, 1));

                if (!GFunc.IsNE(objSYSOption) && !GFunc.IsNE(objSYSOption.OpID))
                {
                    if (GFunc.CompareString(objSYSOption.OpDataType , "Integer")==false)
                    {
                        MsgBox.Show(cn, MsgID.Option.InvalidCallOption);
                        return retValue;
                    }

                    if (objSYSOption.OpValue == string.Empty)
                        objSYSOption.OpValue = objSYSOption.OpValueDefault;

                    if (Int32.TryParse(objSYSOption.OpValue, out retValue))
                        return retValue;
                    else
                        return null;
                }
                else
                {
                    MsgBox.Show(cn, MsgID.Option.GetOptionFail);
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static bool GetBool(string opID)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return GetBool(opID, cn);
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool GetBool(string opID, SqlConnection cn)
        {
            bool retValue = false;
            try
            {
                SYSOption objSYSOption = new SYSOption();
                objSYSOption.Fetch(cn, new SYSOption.Criteria(opID, AppInfor.currentUserKey, 1));

                if (!GFunc.IsNE(objSYSOption) && !GFunc.IsNE(objSYSOption.OpID))
                {
                    if (GFunc.CompareString(objSYSOption.OpDataType.ToUpper() , "BIT")==false && GFunc.CompareString(objSYSOption.OpDataType.ToUpper() , "BOOLEAN")==false)
                    {
                        MsgBox.Show(cn, MsgID.Option.InvalidCallOption);
                        return retValue;
                    }

                    if (objSYSOption.OpValue == string.Empty)
                        objSYSOption.OpValue = objSYSOption.OpValueDefault;


                    if (GFunc.IsNE(objSYSOption.OpValue))
                    {
                        return retValue;
                    }
                    else
                    {
                        if (objSYSOption.OpValue == "1" || GFunc.CompareString(objSYSOption.OpValue.ToUpper(),"TRUE" ))
                            objSYSOption.OpValue = "true";
                        else if (objSYSOption.OpValue == "0" || GFunc.CompareString(objSYSOption.OpValue.ToUpper(),"FALSE"))
                            objSYSOption.OpValue = "false";

                        Boolean.TryParse(objSYSOption.OpValue, out retValue);
                        return retValue;
                    }
                }
                else
                {
                    MsgBox.Show(cn, MsgID.Option.GetOptionFail);
                    return retValue;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static decimal? GetDec(string opID)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return GetDec(opID, cn);
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static decimal? GetDec(string opID, SqlConnection cn)
        {
            try
            {

                decimal retValue = 0;
                SYSOption objSYSOption = new SYSOption();
                objSYSOption.Fetch(cn, new SYSOption.Criteria(opID, AppInfor.currentUserKey, 1));

                if (!GFunc.IsNE(objSYSOption) && !GFunc.IsNE(objSYSOption.OpID))
                {
                    if (GFunc.CompareString(objSYSOption.OpDataType.ToUpper() , "MONEY")==false)
                    {
                        MsgBox.Show(cn, MsgID.Option.InvalidCallOption);
                        return retValue;
                    }

                    if (objSYSOption.OpValue == string.Empty)
                        objSYSOption.OpValue = objSYSOption.OpValueDefault;

                    if (GFunc.IsNE(objSYSOption.OpValue))
                    {
                        return null;
                    }
                    else
                    {
                        Decimal.TryParse(objSYSOption.OpValue, out retValue);
                        return retValue;
                    }
                }
                else
                {
                    MsgBox.Show(cn, MsgID.Option.GetOptionFail);
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static string GetStr(string opID)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return GetStr(opID, cn);
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static string GetStr(string opID, SqlConnection cn)
        {
            try
            {
                SYSOption objSYSOption = new SYSOption();
                objSYSOption.Fetch(cn, new SYSOption.Criteria(opID, AppInfor.currentUserKey, 1));

                if (!GFunc.IsNE(objSYSOption) && !GFunc.IsNE(objSYSOption.OpID))
                {
                    if (GFunc.CompareString(objSYSOption.OpDataType , "nvarchar")==false)
                    {
                        MsgBox.Show(cn, MsgID.Option.InvalidCallOption);
                        return null;
                    }

                    if (objSYSOption.OpValue == string.Empty)
                        objSYSOption.OpValue = objSYSOption.OpValueDefault;

                    return objSYSOption.OpValue;
                }
                else
                {
                    MsgBox.Show(cn, MsgID.Option.GetOptionFail);
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static string GetDisplayStr(string opID)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return GetDisplayStr(opID, cn);
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static string GetDisplayStr(string opID, SqlConnection cn)
        {
            try
            {
                string OpDisplayValue = "";

                SYSOption objSYSOption = new SYSOption();
                objSYSOption.Fetch(cn, new SYSOption.Criteria(opID, AppInfor.currentUserKey, 1));

                if (objSYSOption.OpValue == string.Empty)
                    objSYSOption.OpValue = objSYSOption.OpValueDefault;

                if (!GFunc.IsNE(objSYSOption) && !GFunc.IsNE(objSYSOption.OpID))
                {
                    if (GFunc.CompareString(objSYSOption.OpDataType, "integer"))
                    {
                        System.Data.DataTable dt = GFunc.ExecuteQuery(string.Format(objSYSOption.OpDisplaySql,objSYSOption.OpValue));
                        if (dt.Rows.Count > 0)
                            OpDisplayValue = GFunc.NEStr(dt.Rows[0][0], "");
                        dt.Dispose();
                    }

                    return OpDisplayValue;
                }
                else
                {
                    MsgBox.Show(cn, MsgID.Option.GetOptionFail);
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static DateTime? GetDate(string opID)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return GetDate(opID, cn);
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static DateTime? GetDate(string opID, SqlConnection cn)
        {
            try
            {

                DateTime retValue = new DateTime();
                SYSOption objSYSOption = new SYSOption();
                objSYSOption.Fetch(cn, new SYSOption.Criteria(opID, AppInfor.currentUserKey, 1));

                if (!GFunc.IsNE(objSYSOption) && !GFunc.IsNE(objSYSOption.OpID))
                {
                    if (GFunc.CompareString(objSYSOption.OpDataType , "Date")==false)
                    {
                        MsgBox.Show(cn, MsgID.Option.InvalidCallOption);
                        return null;
                    }

                    if (objSYSOption.OpValue == string.Empty)
                        objSYSOption.OpValue = objSYSOption.OpValueDefault;

                    if (GFunc.IsNE(objSYSOption.OpValue))
                    {
                        return null;
                    }
                    else
                    {
                        DateTime.TryParse(objSYSOption.OpValue, out retValue);
                        return retValue;
                    }
                }
                else
                {
                    MsgBox.Show(cn, MsgID.Option.GetOptionFail);
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static int GetNewLockingGUID(SqlConnection cn)
        {
            int GUID = 0;
            try
            {
                int currentUserKey = AppInfor.currentUserKey;

                if (!SYSOption.NewGUID(cn, currentUserKey, out GUID))
                    GUID = 0;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            return GUID;
        }

        public static int GetNewLockingGUID(SqlConnection cn, bool DisplayMsg)
        {
            int GUID = 0;
            try
            {
                int currentUserKey = AppInfor.currentUserKey;

                if (!SYSOption.NewGUID(cn, currentUserKey, out GUID))
                {
                    GUID = 0;
                    if (DisplayMsg)
                        MsgBox.Show(MsgID.Option.GetLockingGUIDFail);
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            return GUID;
        }

        public static int NewDocKey_Get(SqlConnection cn, GEnum.SystemCode codeKey)
        {
            try
            {
                return SYSOption.NewDocKey(cn, (int)codeKey);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static bool Update(string opID,string  opValue)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return Update(opID,opValue, cn);
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool Update(string opID, string  opValue, SqlConnection cn)
        {
            try
            {

                
                SYSOption objSYSOption = new SYSOption();
               return  objSYSOption.UpdateValue(cn, new SYSOption.Criteria(opID, opValue));

                
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }


        public static int GetSysOpInt(string opID)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    try
                    {

                        int retValue = 0;
                        SYSOption objSYSOption = new SYSOption();
                        objSYSOption.Fetch(cn, new SYSOption.Criteria(opID,-1, 7));

                        if (!GFunc.IsNE(objSYSOption) && !GFunc.IsNE(objSYSOption.OpID))
                        {
                            if (GFunc.CompareString(objSYSOption.OpDataType, "Integer") == false)
                            {
                                MsgBox.Show(cn, MsgID.Option.InvalidCallOption);
                                return retValue;
                            }

                            if (objSYSOption.OpValue == string.Empty)
                                objSYSOption.OpValue = objSYSOption.OpValueDefault;

                            if (Int32.TryParse(objSYSOption.OpValue, out retValue))
                            {
                                return retValue;
                            }
                            else
                            {
                                if (GFunc.CompareString(objSYSOption.ValidationRule, "NoRule") == false)
                                    MsgBox.Show(cn, MsgID.Option.GetOptionFail);

                                return retValue;
                            }
                        }
                        else
                        {
                            MsgBox.Show(cn, MsgID.Option.GetOptionFail);
                            return retValue;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static bool GetSysOpBool(string opID)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    bool retValue = false;
                    try
                    {
                        SYSOption objSYSOption = new SYSOption();
                        objSYSOption.Fetch(cn, new SYSOption.Criteria(opID, -1, 7));

                        if (!GFunc.IsNE(objSYSOption) && !GFunc.IsNE(objSYSOption.OpID))
                        {
                            if (GFunc.CompareString(objSYSOption.OpDataType.ToUpper(), "BIT") == false && GFunc.CompareString(objSYSOption.OpDataType.ToUpper(), "BOOLEAN") == false)
                            {
                                MsgBox.Show(cn, MsgID.Option.InvalidCallOption);
                                return retValue;
                            }

                            if (objSYSOption.OpValue == string.Empty)
                                objSYSOption.OpValue = objSYSOption.OpValueDefault;


                            if (GFunc.IsNE(objSYSOption.OpValue))
                            {
                                return retValue;
                            }
                            else
                            {
                                if (objSYSOption.OpValue == "1" || GFunc.CompareString(objSYSOption.OpValue.ToUpper(), "TRUE"))
                                    objSYSOption.OpValue = "true";
                                else if (objSYSOption.OpValue == "0" || GFunc.CompareString(objSYSOption.OpValue.ToUpper(), "FALSE"))
                                    objSYSOption.OpValue = "false";

                                Boolean.TryParse(objSYSOption.OpValue, out retValue);
                                return retValue;
                            }
                        }
                        else
                        {
                            MsgBox.Show(cn, MsgID.Option.GetOptionFail);
                            return retValue;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static decimal? GetSysOpDec(string opID)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    try
                    {

                        decimal retValue = 0;
                        SYSOption objSYSOption = new SYSOption();
                        objSYSOption.Fetch(cn, new SYSOption.Criteria(opID, -1, 7));

                        if (!GFunc.IsNE(objSYSOption) && !GFunc.IsNE(objSYSOption.OpID))
                        {
                            if (GFunc.CompareString(objSYSOption.OpDataType.ToUpper(), "MONEY") == false)
                            {
                                MsgBox.Show(cn, MsgID.Option.InvalidCallOption);
                                return retValue;
                            }

                            if (objSYSOption.OpValue == string.Empty)
                                objSYSOption.OpValue = objSYSOption.OpValueDefault;

                            if (GFunc.IsNE(objSYSOption.OpValue))
                            {
                                return null;
                            }
                            else
                            {
                                Decimal.TryParse(objSYSOption.OpValue, out retValue);
                                return retValue;
                            }
                        }
                        else
                        {
                            MsgBox.Show(cn, MsgID.Option.GetOptionFail);
                            return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static string GetSysOpStr(string opID)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    try
                    {
                        SYSOption objSYSOption = new SYSOption();
                        objSYSOption.Fetch(cn, new SYSOption.Criteria(opID, -1, 7));

                        if (!GFunc.IsNE(objSYSOption) && !GFunc.IsNE(objSYSOption.OpID))
                        {
                            if (GFunc.CompareString(objSYSOption.OpDataType, "nvarchar") == false)
                            {
                                MsgBox.Show(cn, MsgID.Option.InvalidCallOption);
                                return null;
                            }

                            if (objSYSOption.OpValue == string.Empty)
                                objSYSOption.OpValue = objSYSOption.OpValueDefault;

                            return objSYSOption.OpValue;
                        }
                        else
                        {
                            MsgBox.Show(cn, MsgID.Option.GetOptionFail);
                            return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static DateTime? GetSysOpDate(string opID)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    try
                    {

                        DateTime retValue = new DateTime();
                        SYSOption objSYSOption = new SYSOption();
                        objSYSOption.Fetch(cn, new SYSOption.Criteria(opID, -1, 7));

                        if (!GFunc.IsNE(objSYSOption) && !GFunc.IsNE(objSYSOption.OpID))
                        {
                            if (GFunc.CompareString(objSYSOption.OpDataType, "Date") == false)
                            {
                                MsgBox.Show(cn, MsgID.Option.InvalidCallOption);
                                return null;
                            }

                            if (objSYSOption.OpValue == string.Empty)
                                objSYSOption.OpValue = objSYSOption.OpValueDefault;

                            if (GFunc.IsNE(objSYSOption.OpValue))
                            {
                                return null;
                            }
                            else
                            {
                                DateTime.TryParse(objSYSOption.OpValue, out retValue);
                                return retValue;
                            }
                        }
                        else
                        {
                            MsgBox.Show(cn, MsgID.Option.GetOptionFail);
                            return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        //Error Exceptions
        private static Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { });
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
        private static TAException Error(TAException ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { });
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

    }
}
