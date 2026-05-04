
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using TAUtil;

namespace BOLib
{
    public class SysAuditLogUtility 
    {
        public static bool AddAuditLog(GEnum.AuditLogMode LogMode, GEnum.SystemCode CodeKey,int? RecKey,string RecID, object[] AuditLogInfo)
        {   
            SYSLog objSYSLog = SYSLog.New();

            objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;
            objSYSLog._logMode = (int?)LogMode;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._userNm = AppInfor.currentUserName;
            objSYSLog._logCodeKey = (int?)CodeKey;
            objSYSLog._logCodeID = GFunc.NEStr(CodeKey, string.Empty);
            objSYSLog._logDK = RecKey;
            objSYSLog._logDocID= RecID;

            if (AuditLogInfo != null)
            {  
                for(int i = 0; i< AuditLogInfo.Length; i++)
                {
                    if (AuditLogInfo[i] != null)
                    {
                        string _log = GFunc.ConvertObjectToXML(AuditLogInfo[i]);

                        switch (i)
                        {
                            case 0: objSYSLog._logDetail1 = _log; break;
                            case 1: objSYSLog._logDetail2 = _log; break;
                            case 2: objSYSLog._logDetail3 = _log; break;
                            case 3: objSYSLog._logDetail4 = _log; break;
                            case 4: objSYSLog._logDetail5 = _log; break;
                            case 5: objSYSLog._logDetail6 = _log; break;
                            case 6: objSYSLog._logDetail7 = _log; break;
                            case 7: objSYSLog._logDetail8 = _log; break;
                            case 8: objSYSLog._logDetail9 = _log; break;
                        }
                    }
                }
            }  

            try
            {
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BOSSSystemMasterConnection))
                    {
                        cn.Open();
                        if (objSYSLog.Insert(cn))
                        {       
                            //comment by Jane. IF TransactionScope is opened with Suppress Option, can not get TransactionInformation Status 
                            //if (System.Transactions.Transaction.Current.TransactionInformation.Status != System.Transactions.TransactionStatus.Active)  throw new Exception("Transaction has aborted."); 
                            scope.Complete();
                            return true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AddAuditLog", "Audit Log Fail", ex.StackTrace);
              
            }

            return false;
        }

        public static bool AddAuditLog(GEnum.AuditLogMode LogMode, GEnum.SystemCode CodeKey, int? RecKey, string RecID,string logTypeName, object[] AuditLogInfo)
        {
            SYSLog objSYSLog = SYSLog.New();

            objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;
            objSYSLog._logMode = (int?)LogMode;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._userNm = AppInfor.currentUserName;
            objSYSLog._logCodeKey = (int?)CodeKey;
            objSYSLog._logCodeID = GFunc.NEStr(CodeKey, string.Empty);
            objSYSLog._logDK = RecKey;
            objSYSLog._logDocID = RecID;
            objSYSLog._logDocTypeNm = logTypeName;

            if (AuditLogInfo != null)
            {
                for (int i = 0; i < AuditLogInfo.Length; i++)
                {
                    if (AuditLogInfo[i] != null)
                    {
                        string _log = GFunc.ConvertObjectToXML(AuditLogInfo[i]);

                        switch (i)
                        {
                            case 0: objSYSLog._logDetail1 = _log; break;
                            case 1: objSYSLog._logDetail2 = _log; break;
                            case 2: objSYSLog._logDetail3 = _log; break;
                            case 3: objSYSLog._logDetail4 = _log; break;
                            case 4: objSYSLog._logDetail5 = _log; break;
                            case 5: objSYSLog._logDetail6 = _log; break;
                            case 6: objSYSLog._logDetail7 = _log; break;
                            case 7: objSYSLog._logDetail8 = _log; break;
                            case 8: objSYSLog._logDetail9 = _log; break;
                        }
                    }
                }
            }

            try
            {
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BOSSSystemMasterConnection))
                    {
                        cn.Open();
                        if (objSYSLog.Insert(cn))
                        {
                            //comment by Jane. IF TransactionScope is opened with Suppress Option, can not get TransactionInformation Status 
                            //if (System.Transactions.Transaction.Current.TransactionInformation.Status != System.Transactions.TransactionStatus.Active)  throw new Exception("Transaction has aborted."); 
                            scope.Complete();
                            return true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AddAuditLog", "Audit Log Fail", ex.StackTrace);

            }

            return false;
        }

        public static bool AddAuditLog(GEnum.AuditLogMode LogMode, GEnum.SystemCode CodeKey, int? RecKey, string RecID, DateTime logDate, string logTypeName, object[] AuditLogInfo,DateTime logDocDate)
        {
            SYSLog objSYSLog = SYSLog.New();

            objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;
            objSYSLog._logMode = (int?)LogMode;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._userNm = AppInfor.currentUserName;
            objSYSLog._logCodeKey = (int?)CodeKey;
            objSYSLog._logCodeID = GFunc.NEStr(CodeKey, string.Empty);
            objSYSLog._logDate = logDate;
            objSYSLog._logDocDate = logDocDate;
            objSYSLog._logDocTypeNm = logTypeName;
            objSYSLog._logDK = RecKey;
            objSYSLog._logDocID = RecID;

            if (AuditLogInfo != null)
            {
                for (int i = 0; i < AuditLogInfo.Length; i++)
                {
                    if (AuditLogInfo[i] != null)
                    {
                        string _log = "";
                        if (AuditLogInfo[i].GetType() == typeof(String))
                        {
                            switch (i)
                            {
                                case 0: objSYSLog._custom1 = AuditLogInfo[i].ToString(); break;
                                case 1: objSYSLog._custom2 = AuditLogInfo[i].ToString(); break;
                                case 2: objSYSLog._custom3 = AuditLogInfo[i].ToString(); break;
                            }

                        }
                        else
                        {
                            _log = GFunc.ConvertDataTableToXML((DataTable)AuditLogInfo[i]);

                            switch (i)
                            {
                                case 0: objSYSLog._logDetail1 = _log; break;
                                case 1: objSYSLog._logDetail2 = _log; break;
                                case 2: objSYSLog._logDetail3 = _log; break;
                                case 3: objSYSLog._logDetail4 = _log; break;
                                case 4: objSYSLog._logDetail5 = _log; break;
                                case 5: objSYSLog._logDetail6 = _log; break;
                                case 6: objSYSLog._logDetail7 = _log; break;
                                case 7: objSYSLog._logDetail8 = _log; break;
                                case 8: objSYSLog._logDetail9 = _log; break;
                            }
                        }
                    }
                }
            }

            try
            {
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BOSSSystemMasterConnection))
                    {
                        cn.Open();
                        if (objSYSLog.Insert(cn))
                        {
                            //comment by Jane. IF TransactionScope is opened with Suppress Option, can not get TransactionInformation Status 
                            //if (System.Transactions.Transaction.Current.TransactionInformation.Status != System.Transactions.TransactionStatus.Active)  throw new Exception("Transaction has aborted."); 
                            scope.Complete();
                            return true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AddAuditLog", "Audit Log Fail", ex.StackTrace);

            }

            return false;
        }

        public static bool AddAuditLog(GEnum.AuditLogMode LogMode, GEnum.SystemCode CodeKey, int? RecKey, string RecID,DateTime logDate,string logTypeName, object[] AuditLogInfo)
        {
            SYSLog objSYSLog = SYSLog.New();

            objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;
            objSYSLog._logMode = (int?)LogMode;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._userNm = AppInfor.currentUserName;
            objSYSLog._logCodeKey = (int?)CodeKey;
            objSYSLog._logCodeID = GFunc.NEStr(CodeKey, string.Empty);
            objSYSLog._logDate = logDate;
            objSYSLog._logDocTypeNm = logTypeName;
            objSYSLog._logDK = RecKey;
            objSYSLog._logDocID = RecID;            

            if (AuditLogInfo != null)
            {
                for (int i = 0; i < AuditLogInfo.Length; i++)
                {
                    if (AuditLogInfo[i] != null)
                    {
                        string _log = GFunc.ConvertDataTableToXML((DataTable)AuditLogInfo[i]);                       
                  
                        switch (i)
                        {
                            case 0: objSYSLog._logDetail1 = _log; break;
                            case 1: objSYSLog._logDetail2 = _log; break;
                            case 2: objSYSLog._logDetail3 = _log; break;
                            case 3: objSYSLog._logDetail4 = _log; break;
                            case 4: objSYSLog._logDetail5 = _log; break;
                            case 5: objSYSLog._logDetail6 = _log; break;
                            case 6: objSYSLog._logDetail7 = _log; break;
                            case 7: objSYSLog._logDetail8 = _log; break;
                            case 8: objSYSLog._logDetail9 = _log; break;
                        }
                       
                    }
                }
            }

            try
            {
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BOSSSystemMasterConnection))
                    {
                        cn.Open();
                        if (objSYSLog.Insert(cn))
                        {
                            //comment by Jane. IF TransactionScope is opened with Suppress Option, can not get TransactionInformation Status 
                            //if (System.Transactions.Transaction.Current.TransactionInformation.Status != System.Transactions.TransactionStatus.Active)  throw new Exception("Transaction has aborted."); 
                            scope.Complete();
                            return true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AddAuditLog", "Audit Log Fail", ex.StackTrace);

            }

            return false;
        }

        internal static bool AddAuditLog(SqlConnection cn, GEnum.AuditLogMode LogMode, GEnum.SystemCode CodeKey, int RecKey, string RecID, object[] AuditLogInfo)
        {   
            SYSLog objSYSLog = SYSLog.New();

            objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;
            objSYSLog._logMode = (int?)LogMode;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._userNm = AppInfor.currentUserName;
            objSYSLog._logCodeKey = (int?)CodeKey;
            objSYSLog._logCodeID = CodeKey.ToString();
            objSYSLog._logDK = RecKey;
            objSYSLog._logDocID = RecID;

           

            if (AuditLogInfo != null)
            {  
                for(int i = 0; i< AuditLogInfo.Length; i++)
                {
                    if (AuditLogInfo[i] != null)
                    {
                        string _log = GFunc.ConvertObjectToXML(AuditLogInfo[i]);

                        switch (i)
                        {
                            case 0: objSYSLog._logDetail1 = _log; break;
                            case 1: objSYSLog._logDetail2 = _log; break;
                            case 2: objSYSLog._logDetail3 = _log; break;
                            case 3: objSYSLog._logDetail4 = _log; break;
                            case 4: objSYSLog._logDetail5 = _log; break;
                            case 5: objSYSLog._logDetail6 = _log; break;
                            case 6: objSYSLog._logDetail7 = _log; break;
                            case 7: objSYSLog._logDetail8 = _log; break;
                            case 8: objSYSLog._logDetail9 = _log; break;
                        }
                    }
                }
            }  

            try
            {                
                if (objSYSLog.Insert(cn))
                {                         
                    return true;
                }
                  
            }
            catch (Exception ex)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AddAuditLog", "Audit Log Fail", ex.StackTrace);
              
            }

            return false;
        }

        internal static bool AddAuditLog(GEnum.AuditLogMode LogMode, GEnum.SystemCode CodeKey, int? DocKey, string DocID, DateTime? DocDate, string DocTypeNm, object Header, Hashtable Details)
        {
            SYSLog objSYSLog = SYSLog.New();
            int i = 1;

            objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;
            objSYSLog._logMode = (int?)LogMode;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._userNm = AppInfor.currentUserName;
            objSYSLog._logCodeKey = (int?)CodeKey;
            objSYSLog._logCodeID = GFunc.NEStr(CodeKey,string.Empty);
            objSYSLog._logCodeID = CodeKey.ToString();
           

            objSYSLog._logDK = DocKey;
            objSYSLog._logDocID = DocID;
            objSYSLog._logDocDate = DocDate;
            objSYSLog._logDocTypeNm = DocTypeNm;

            if (Header != null)
            {
                objSYSLog._logHeader = Header.GetType().ToString();               
                objSYSLog._logDetail1 = GFunc.ConvertObjectToXML(Header);
            }

            foreach (object val in Details.Values)
            {
                string _log = GFunc.ConvertObjectToXML(val);

                switch (i)
                {
                    case 1: objSYSLog._logDetail2 = _log; break;
                    case 2: objSYSLog._logDetail3 = _log; break;
                    case 3: objSYSLog._logDetail4 = _log; break;
                    case 4: objSYSLog._logDetail5 = _log; break;
                    case 5: objSYSLog._logDetail6 = _log; break;
                    case 6: objSYSLog._logDetail7 = _log; break;
                    case 7: objSYSLog._logDetail8 = _log; break;
                    case 8: objSYSLog._logDetail9 = _log; break;

                }
                i++;
            }

            try
            {
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BOSSSystemMasterConnection))
                    {
                        cn.Open();
                        if (objSYSLog.Insert(cn))
                        {
                            //comment by Jane. IF TransactionScope is opened with Suppress Option, can not get TransactionInformation Status                  
                            //if (System.Transactions.Transaction.Current.TransactionInformation.Status != System.Transactions.TransactionStatus.Active)  throw new Exception("Transaction has aborted."); 
                            scope.Complete();
                            return true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AddAuditLog", "Audit Log Add Fail", ex.StackTrace);             
            }
            return false;
        }

        internal static bool AddAuditLog(SqlConnection cn, GEnum.AuditLogMode LogMode, GEnum.SystemCode CodeKey, int? DocKey, string DocID, DateTime? DocDate, string DocTypeNm, object Header, Hashtable Details)
        {
            SYSLog objSYSLog = SYSLog.New();
            int i = 1;

            objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;
            objSYSLog._logMode = (int?)LogMode;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._userNm = AppInfor.currentUserName;
            objSYSLog._logCodeKey = (int?)CodeKey;
            objSYSLog._logCodeID = GFunc.NEStr(CodeKey, string.Empty);
            objSYSLog._logDK = DocKey;
            objSYSLog._logDocID = DocID;
            objSYSLog._logDocDate = DocDate;
            objSYSLog._logDocTypeNm = DocTypeNm;

            if (Header != null)
            {
                objSYSLog._logHeader = Header.GetType().ToString();
                objSYSLog._logDetail1 = GFunc.ConvertObjectToXML(Header);
            }

            foreach (object val in Details.Values)
            {
                string _log = GFunc.ConvertObjectToXML(val);

                switch (i)
                {
                    case 1: objSYSLog._logDetail2 = _log; break;
                    case 2: objSYSLog._logDetail3 = _log; break;
                    case 3: objSYSLog._logDetail4 = _log; break;
                    case 4: objSYSLog._logDetail5 = _log; break;
                    case 5: objSYSLog._logDetail6 = _log; break;
                    case 6: objSYSLog._logDetail7 = _log; break;
                    case 7: objSYSLog._logDetail8 = _log; break;
                    case 8: objSYSLog._logDetail9 = _log; break;

                }
                i++;
            }

            try
            {
               
                if (objSYSLog.Insert(cn))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AddAuditLog", "Audit Log Add Fail", ex.StackTrace);
            }
            return false;
        }
     

        public static bool AddErrorLog(GEnum.AuditLogMode LogMode, int? CodeKey, Exception ex, bool isAppExecption, bool displayMsg)
        {

            SYSLog objSYSLog = SYSLog.New();

            objSYSLog._logType = (int?)GEnum.AuditLogType.SystemError;
            objSYSLog._logMode = (int?)LogMode; ;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._userNm = AppInfor.currentUserName;
            objSYSLog._logCodeKey = CodeKey;
            objSYSLog._logCodeID = GFunc.NEStr(CodeKey, string.Empty);
            objSYSLog._errMsg = ex.Message;
            objSYSLog._errDetails = ex.StackTrace;

            try
            {
                if (displayMsg)
                {
                    if (isAppExecption)
                        MsgBox.Show(ex.Message);
                    else
                        MsgBox.Show(ex.Message);
                }

                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BOSSSystemMasterConnection))
                    {
                        cn.Open();
                        if (objSYSLog.Insert(cn))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AddErrorLog", "Error Log Add Fail", e.StackTrace);               
            }
            return false;
        }

        public static bool AddErrorLog(GEnum.AuditLogMode LogMode, int? CodeKey, Exception ex, bool isAppExecption, bool displayMsg, object AuditLogInfo)
        {
            SYSLog objSYSLog = SYSLog.New();            

            objSYSLog._logType = (int?)GEnum.AuditLogType.SystemError;
            objSYSLog._logMode = (int?)LogMode; ;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._userNm = AppInfor.currentUserName;
            objSYSLog._logCodeKey = CodeKey;
            objSYSLog._logCodeID = GFunc.NEStr(CodeKey, string.Empty);
            objSYSLog._errMsg = ex.Message;
            objSYSLog._errDetails = ex.StackTrace;

            try
            {
                if (displayMsg)
                {
                    if (isAppExecption)
                        MsgBox.Show(ex.Message);
                    else
                        MsgBox.Show(ex.Message);
                }

                if (AuditLogInfo!= null)
                {
                    objSYSLog._logHeader = AuditLogInfo.GetType().ToString();
                    objSYSLog._logDetail1 = GFunc.ConvertObjectToXML(AuditLogInfo);
                }

                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BOSSSystemMasterConnection))
                    {
                        cn.Open();
                        if (objSYSLog.Insert(cn))
                        {
                           return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AddErrorLog", "Error Log Add Fail", e.StackTrace);                
            }
            return false;
        }

        public static bool AddErrorLog(GEnum.AuditLogMode LogMode, int? CodeKey, Exception ex, bool isAppExecption, bool displayMsg, object[] AuditLogInfoList)
        {
            SYSLog objSYSLog = SYSLog.New();

            objSYSLog._logType = (int?)GEnum.AuditLogType.SystemError;
            objSYSLog._logMode = (int?)LogMode; ;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._userNm = AppInfor.currentUserName;
            objSYSLog._logCodeKey = CodeKey;
            objSYSLog._logCodeID = GFunc.NEStr(CodeKey, string.Empty);
            objSYSLog._errMsg = ex.Message;
            objSYSLog._errDetails = ex.StackTrace;

            try
            {
                if (displayMsg)
                {
                    if (isAppExecption)
                        MsgBox.Show(ex.Message);
                    else
                        MsgBox.Show(ex.Message);
                }

                int i = 1;

                if (AuditLogInfoList != null)
                {
                    foreach (object o in AuditLogInfoList)
                    {
                        if (o == null)
                            continue;
                        objSYSLog._logHeader = o.GetType().ToString() + ",";

                        switch (i)
                        {
                            case 1: objSYSLog._logDetail1 = GFunc.ConvertObjectToXML(AuditLogInfoList[0]); break;
                            case 2: objSYSLog._logDetail2 = GFunc.ConvertObjectToXML(AuditLogInfoList[1]); break;
                            case 3: objSYSLog._logDetail3 = GFunc.ConvertObjectToXML(AuditLogInfoList[2]); break;
                            case 4: objSYSLog._logDetail4 = GFunc.ConvertObjectToXML(AuditLogInfoList[3]); break;
                            case 5: objSYSLog._logDetail5 = GFunc.ConvertObjectToXML(AuditLogInfoList[4]); break;
                            case 6: objSYSLog._logDetail6 = GFunc.ConvertObjectToXML(AuditLogInfoList[5]); break;
                            case 7: objSYSLog._logDetail7 = GFunc.ConvertObjectToXML(AuditLogInfoList[6]); break;
                            case 8: objSYSLog._logDetail8 = GFunc.ConvertObjectToXML(AuditLogInfoList[7]); break;
                            case 9: objSYSLog._logDetail9 = GFunc.ConvertObjectToXML(AuditLogInfoList[8]); break;
                        }
                        i++;
                    }
                }

                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BOSSSystemMasterConnection))
                    {
                        cn.Open();
                        if (objSYSLog.Insert(cn))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AddErrorLog", "Error Log Add Fail", e.StackTrace);
            }
            return false;
        }

        #region Error Stack Record Create
        public static Exception ModifyException(Exception ex)
        {
            try
            {
                ErrorRecorder l_errorRecorder = new ErrorRecorder(false, ex.StackTrace, new object[] { });
                ex.Data.Add(ex.Data.Count, l_errorRecorder);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        public static Exception ModifyException(Exception ex, bool IsShowMessage)
        {
            try
            {
                ErrorRecorder l_errorRecorder = new ErrorRecorder(IsShowMessage, ex.StackTrace, new object[] { });
                ex.Data.Add(ex.Data.Count, l_errorRecorder);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        public static Exception ModifyException(Exception ex, bool IsShowMessage, object[] ObjColection)
        {
            try
            {
                ErrorRecorder l_errorRecorder = new ErrorRecorder(IsShowMessage, ex.StackTrace, ObjColection);
                ex.Data.Add(ex.Data.Count, l_errorRecorder);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        public static Exception ModifyException(Exception ex, bool IsShowMessage, object[] ObjColection, GEnum.AuditLogMode LogMode)
        {
            try
            {
                ErrorRecorder l_errorRecorder = new ErrorRecorder(IsShowMessage, ex.StackTrace, ObjColection, LogMode);
                ex.Data.Add(ex.Data.Count, l_errorRecorder);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        public static Exception ModifyException(Exception ex, bool IsShowMessage, object[] ObjColection, GEnum.AuditLogMode LogMode, GEnum.SystemCode CodeKey)
        {
            try
            {
                ErrorRecorder l_errorRecorder = new ErrorRecorder(IsShowMessage, ex.StackTrace, ObjColection, LogMode, CodeKey);
                ex.Data.Add(ex.Data.Count, l_errorRecorder);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        public static Exception ModifyException(Exception ex, bool IsShowMessage, object[] ObjColection, GEnum.SystemCode CodeKey)
        {
            try
            {
                ErrorRecorder l_errorRecorder = new ErrorRecorder(IsShowMessage, ex.StackTrace, ObjColection, GEnum.AuditLogMode.Error, CodeKey);
                ex.Data.Add(ex.Data.Count, l_errorRecorder);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        //TA Exception
        public static TAException ModifyTAException(TAException ex)
        {
            try
            {
                ErrorRecorder l_errorRecorder = new ErrorRecorder(false, ex.StackTrace, new object[] { });
                ex.Data.Add(ex.Data.Count, l_errorRecorder);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        public static TAException ModifyTAException(TAException ex, bool IsShowMessage)
        {
            try
            {
                ErrorRecorder l_errorRecorder = new ErrorRecorder(IsShowMessage, ex.StackTrace, new object[] { });
                ex.Data.Add(ex.Data.Count, l_errorRecorder);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        public static TAException ModifyTAException(TAException ex, bool IsShowMessage, object[] ObjColection)
        {
            try
            {
                ErrorRecorder l_errorRecorder = new ErrorRecorder(IsShowMessage, ex.StackTrace, ObjColection);
                ex.Data.Add(ex.Data.Count, l_errorRecorder);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        public static TAException ModifyTAException(TAException ex, bool IsShowMessage, object[] ObjColection, GEnum.AuditLogMode LogMode)
        {
            try
            {
                ErrorRecorder l_errorRecorder = new ErrorRecorder(IsShowMessage, ex.StackTrace, ObjColection, LogMode);
                ex.Data.Add(ex.Data.Count, l_errorRecorder);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        public static TAException ModifyTAException(TAException ex, bool IsShowMessage, List<object> ObjColection, GEnum.AuditLogMode LogMode, GEnum.SystemCode CodeKey)
        {
            try
            {
                //ErrorRecorder l_errorRecorder = new ErrorRecorder(IsShowMessage, ex.StackTrace, ObjColection, LogMode, CodeKey);
                //ex.Data.Add(ex.Data.Count, l_errorRecorder);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        public static TAException ModifyTAException(TAException ex, bool IsShowMessage, object[] ObjColection, GEnum.SystemCode CodeKey)
        {
            try
            {
                ErrorRecorder l_errorRecorder = new ErrorRecorder(IsShowMessage, ex.StackTrace, ObjColection, GEnum.AuditLogMode.Error, CodeKey);
                ex.Data.Add(ex.Data.Count, l_errorRecorder);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }


        public static bool AddErrorLog(Exception ex)
        {
            bool displayedMessage = false;
            try
            {
                IEnumerator ie = ex.Data.Keys.GetEnumerator();

                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BOSSSystemMasterConnection))
                    {
                        cn.Open();
                        while (ie.MoveNext())
                        {
                            int tmp = -1;
                            if (ie.Current != null)
                            {
                                if (int.TryParse(ie.Current.ToString(), out tmp))
                                {
                                    ErrorRecorder l_errorRecorder = (ErrorRecorder)ex.Data[Convert.ToInt32(ie.Current)];
                                    if (l_errorRecorder.ObjectCarrier.Length > 0)
                                    {
                                        AddErrorLog(cn, l_errorRecorder.LogMode, (int)l_errorRecorder.CodeKey, ex, false, false, l_errorRecorder.ObjectCarrier);
                                    }
                                    else
                                    {
                                        AddErrorLog(cn, l_errorRecorder.LogMode, (int)l_errorRecorder.CodeKey, ex, false, false);
                                    }

                                    if (l_errorRecorder.ShowMessage)
                                    {
                                        if (displayedMessage == false)
                                        {
                                            MsgBox.Show(ex.Message, GEnum.MsgBoxIcon.Information, GEnum.MsgBoxButton.OK);
                                            displayedMessage = true;
                                        }

                                    }
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return false;
        }

        public static bool AddErrorLog(TAException ex)
        {
            bool displayedMessage = false;
            try
            {
                IEnumerator ie = ex.Data.Keys.GetEnumerator();

                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BOSSSystemMasterConnection))
                    {
                        while (ie.MoveNext())
                        {
                            int tmp = -1;
                            if (ie.Current != null)
                            {
                                if (int.TryParse(ie.Current.ToString(), out tmp))
                                {
                                    ErrorRecorder l_errorRecorder = (ErrorRecorder)ex.Data[Convert.ToInt32(ie.Current)];

                                    if (l_errorRecorder.ObjectCarrier.Length > 0)
                                    {
                                        AddErrorLog(cn, l_errorRecorder.LogMode, (int)l_errorRecorder.CodeKey, ex, false, false, l_errorRecorder.ObjectCarrier);
                                    }
                                    else
                                    {
                                        AddErrorLog(cn, l_errorRecorder.LogMode, (int)l_errorRecorder.CodeKey, ex, false, false);
                                    }

                                    if (l_errorRecorder.ShowMessage)
                                    {
                                        if (displayedMessage == false)
                                        {
                                            //if (ex.StackTrace.IndexOf("Object sender") > 0)//Testing, commented. To show Message for whatever object
                                            //{
                                                BOLib.MsgBox.Show(ex.MsgID);
                                                displayedMessage = true;
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return false;
        }

        private static bool AddErrorLog(SqlConnection cn, GEnum.AuditLogMode LogMode, int? CodeKey, Exception ex, bool isAppExecption, bool displayMsg)
        {
            SYSLog objSYSLog = SYSLog.New();
            objSYSLog._logType = (int?)GEnum.AuditLogType.SystemError;
            objSYSLog._logMode = (int?)LogMode; ;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._userNm = AppInfor.currentUserName;
            objSYSLog._logCodeKey = CodeKey;
            objSYSLog._errMsg = ex.Message;
            objSYSLog._errDetails = ex.StackTrace;

            try
            {
                if (displayMsg)
                {
                    if (isAppExecption)
                        MsgBox.Show(cn,ex.Message);
                    else
                        MsgBox.Show(cn,ex.Message);
                }

                if (objSYSLog.Insert(cn))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AddErrorLog", "Error Log Add Fail", e.StackTrace);
            }
            return false;
        }

        private static bool AddErrorLog(SqlConnection cn, GEnum.AuditLogMode LogMode, int? CodeKey, Exception ex, bool isAppExecption, bool displayMsg, object AuditLogInfo)
        {
            SYSLog objSYSLog = SYSLog.New();

            objSYSLog._logType = (int?)GEnum.AuditLogType.SystemError;
            objSYSLog._logMode = (int?)LogMode; ;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._userNm = AppInfor.currentUserName;
            objSYSLog._logCodeKey = CodeKey;
            objSYSLog._errMsg = ex.Message;
            objSYSLog._errDetails = ex.StackTrace;

            try
            {
                if (displayMsg)
                {
                    if (isAppExecption)
                        MsgBox.Show(cn,ex.Message);
                    else
                        MsgBox.Show(cn,ex.Message);
                }

                if (AuditLogInfo != null)
                {
                    objSYSLog._logHeader = AuditLogInfo.GetType().ToString();
                    objSYSLog._logDetail1 = GFunc.ConvertObjectToXML(AuditLogInfo);
                }

                if (objSYSLog.Insert(cn))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AddErrorLog", "Error Log Add Fail", e.StackTrace);
            }
            return false;
        }
        
        //Currently below functions are used in GlobalUI only....;intend to use in all later --Jack
        //Jack -- Implementation for new add error log method, which just only perform logging not concern with displaying message
        public static bool AddErrorLog_New(Exception ex)
        {
            try
            {
                IEnumerator ie = ex.Data.Keys.GetEnumerator();

                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BOSSSystemMasterConnection))
                    {
                        cn.Open();
                        while (ie.MoveNext())
                        {
                            int tmp = -1;
                            if (ie.Current != null)
                            {
                                if (int.TryParse(ie.Current.ToString(), out tmp))
                                {
                                    ErrorRecorder l_errorRecorder = (ErrorRecorder)ex.Data[Convert.ToInt32(ie.Current)];
                                    if (l_errorRecorder.ObjectCarrier.Length > 0)
                                    {
                                        AddErrorLog(cn, l_errorRecorder.LogMode, (int)l_errorRecorder.CodeKey, ex, false, false, l_errorRecorder.ObjectCarrier);
                                    }
                                    else
                                    {
                                        AddErrorLog(cn, l_errorRecorder.LogMode, (int)l_errorRecorder.CodeKey, ex, false, false);
                                    }
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return false;
        }

        public static bool AddErrorLog_New(TAException ex)
        {
            try
            {
                IEnumerator ie = ex.Data.Keys.GetEnumerator();

                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BOSSSystemMasterConnection))
                    {
                        while (ie.MoveNext())
                        {
                            int tmp = -1;
                            if (ie.Current != null)
                            {
                                if (int.TryParse(ie.Current.ToString(), out tmp))
                                {
                                    ErrorRecorder l_errorRecorder = (ErrorRecorder)ex.Data[Convert.ToInt32(ie.Current)];

                                    if (l_errorRecorder.ObjectCarrier.Length > 0)
                                    {
                                        AddErrorLog(cn, l_errorRecorder.LogMode, (int)l_errorRecorder.CodeKey, ex, false, false, l_errorRecorder.ObjectCarrier);
                                    }
                                    else
                                    {
                                        AddErrorLog(cn, l_errorRecorder.LogMode, (int)l_errorRecorder.CodeKey, ex, false, false);
                                    }
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return false;
        }

        private static bool AddErrorLog_New(SqlConnection cn, GEnum.AuditLogMode LogMode, int? CodeKey, Exception ex, bool isAppExecption)
        {
            SYSLog objSYSLog = SYSLog.New();
            objSYSLog._logType = (int?)GEnum.AuditLogType.SystemError;
            objSYSLog._logMode = (int?)LogMode; ;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._userNm = AppInfor.currentUserName;
            objSYSLog._logCodeKey = CodeKey;
            objSYSLog._errMsg = ex.Message;
            objSYSLog._errDetails = ex.StackTrace;

            try
            {
                if (objSYSLog.Insert(cn))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AddErrorLog", "Error Log Add Fail", e.StackTrace);
            }
            return false;
        }

        private static bool AddErrorLog_New(SqlConnection cn, GEnum.AuditLogMode LogMode, int? CodeKey, Exception ex, bool isAppExecption,object AuditLogInfo)
        {
            SYSLog objSYSLog = SYSLog.New();

            objSYSLog._logType = (int?)GEnum.AuditLogType.SystemError;
            objSYSLog._logMode = (int?)LogMode; ;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._userNm = AppInfor.currentUserName;
            objSYSLog._logCodeKey = CodeKey;
            objSYSLog._errMsg = ex.Message;
            objSYSLog._errDetails = ex.StackTrace;

            try
            {
                if (AuditLogInfo != null)
                {
                    objSYSLog._logHeader = AuditLogInfo.GetType().ToString();
                    objSYSLog._logDetail1 = GFunc.ConvertObjectToXML(AuditLogInfo);
                }

                if (objSYSLog.Insert(cn))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AddErrorLog", "Error Log Add Fail", e.StackTrace);
            }
            return false;
        }
        
        //Jack -- Implementation for new Modify Exception function which not only appends hierarchical exception and display message immediately
        //-- For TAException
        public static TAException AppendTAException(TAException ex, bool ShowMessage)
        {
            try
            {
                ErrorRecorder errorRecorder = new ErrorRecorder(ShowMessage, ex.StackTrace, new object[] { });
                ex.Data.Add(ex.Data.Count, errorRecorder);
                if (ShowMessage) BOLib.MsgBox.Show(ex.MsgID, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        public static TAException AppendTAException(TAException ex, bool ShowMessage, object[] ObjColection)
        {
            try
            {
                ErrorRecorder errorRecorder = new ErrorRecorder(ShowMessage, ex.StackTrace, ObjColection);
                ex.Data.Add(ex.Data.Count, errorRecorder);
                if (ShowMessage) BOLib.MsgBox.Show(ex.MsgID, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        public static TAException AppendTAException(TAException ex, bool ShowMessage, object[] ObjColection, GEnum.AuditLogMode LogMode)
        {
            try
            {
                ErrorRecorder errorRecorder = new ErrorRecorder(ShowMessage, ex.StackTrace, ObjColection, LogMode);
                ex.Data.Add(ex.Data.Count, errorRecorder);
                if (ShowMessage) BOLib.MsgBox.Show(ex.MsgID, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        public static TAException AppendTAException(TAException ex, bool ShowMessage, object[] ObjColection, GEnum.SystemCode CodeKey)
        {
            try
            {
                ErrorRecorder errorRecorder = new ErrorRecorder(ShowMessage, ex.StackTrace, ObjColection, GEnum.AuditLogMode.Error, CodeKey);
                ex.Data.Add(ex.Data.Count, errorRecorder);
                if (ShowMessage) BOLib.MsgBox.Show(ex.MsgID, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
        
        //-- For Normal Exception
        public static Exception AppendException(Exception ex, bool ShowMessage)
        {
            try
            {
                ErrorRecorder errorRecorder = new ErrorRecorder(ShowMessage, ex.StackTrace, new object[] { });
                ex.Data.Add(ex.Data.Count, errorRecorder);
                if (ShowMessage) MsgBox.Show(ex.Message, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        public static Exception AppendException(Exception ex, bool ShowMessage, object[] ObjColection)
        {
            try
            {
                ErrorRecorder errorRecorder = new ErrorRecorder(ShowMessage, ex.StackTrace, ObjColection);
                ex.Data.Add(ex.Data.Count, errorRecorder);
                if (ShowMessage) MsgBox.Show(ex.Message, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        public static Exception AppendException(Exception ex, bool ShowMessage, object[] ObjColection, GEnum.AuditLogMode LogMode)
        {
            try
            {
                ErrorRecorder errorRecorder = new ErrorRecorder(ShowMessage, ex.StackTrace, ObjColection, LogMode);
                ex.Data.Add(ex.Data.Count, errorRecorder);
                if (ShowMessage) MsgBox.Show(ex.Message, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        public static Exception AppendException(Exception ex, bool ShowMessage, object[] ObjColection, GEnum.AuditLogMode LogMode, GEnum.SystemCode CodeKey)
        {
            try
            {
                ErrorRecorder errorRecorder = new ErrorRecorder(ShowMessage, ex.StackTrace, ObjColection, LogMode, CodeKey);
                ex.Data.Add(ex.Data.Count, errorRecorder);
                if (ShowMessage) MsgBox.Show(ex.Message, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        public static Exception AppendException(Exception ex, bool ShowMessage, object[] ObjColection, GEnum.SystemCode CodeKey)
        {
            try
            {
                ErrorRecorder errorRecorder = new ErrorRecorder(ShowMessage, ex.StackTrace, ObjColection, GEnum.AuditLogMode.Error, CodeKey);
                ex.Data.Add(ex.Data.Count, errorRecorder);
                if (ShowMessage) MsgBox.Show(ex.Message, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }


        #endregion

        public static DataTable ConvertXmlToDataTable(GEnum.SystemCode docCodeKey, string sXml)
        {
            DataTable dt = new DataTable();

            if (sXml != "")
            {
                DataSet ds = new DataSet();
                StringReader read = new StringReader(sXml);
                System.Xml.XmlTextReader lxmlReader = new System.Xml.XmlTextReader(read);
                ds.ReadXml(lxmlReader);
                dt = new DataTable();
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Value", typeof(string));
                switch (docCodeKey)
                {
                    case GEnum.SystemCode.System_Option:

                        //dt.Columns.Add("DataType", typeof(string));
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow drNew = dt.NewRow();
                            drNew["Name"] = dr["OpName1"].ToString();
                            drNew["Value"] = dr["OpValue"].ToString();

                            //dr["DataType"] = dc.DataType.ToString();
                            dt.Rows.Add(drNew);
                        }
                        break;
                    default:

                        //dt.Columns.Add("DataType", typeof(string));
                        foreach (DataColumn dc in ds.Tables[0].Columns)
                        {
                            DataRow dr = dt.NewRow();
                            dr["Name"] = dc.ColumnName;
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                dr["Value"] = ds.Tables[0].Rows[0][dc].ToString();
                            }
                            //dr["DataType"] = dc.DataType.ToString();
                            dt.Rows.Add(dr);
                        }
                        break;
                }
                return dt;


            }

            return null;
        }

        public static DataTable GetAuditLogList(DateTime LogDateFrom, DateTime LogDateTo, string userID)
        {
            System.Data.DataSet dsResult = new System.Data.DataSet();

            using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(Database.BOSSSystemMasterConnection))
            {
                System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand();
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.CommandText = "SYSLog_GetFilterByDate";


                cm.Parameters.AddWithValue("@LogDateFrom", LogDateFrom);
                cm.Parameters.AddWithValue("@LogDateTo", LogDateTo);
                cm.Parameters.AddWithValue("@UserID", userID);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.CommandTimeout = 1000;

                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                try
                {
                    sqlCon.Open();
                    sqlAdp.Fill(dsResult);
                    return dsResult.Tables[0];
                }
                catch (TAException tex)
                {
                    throw tex;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }
     
        internal static void WriteErrorLogFile(string className, string funcName, string errorMsg, string systemInnerMsg)
        {
            Stream stream;
            DirectoryInfo di;
            string fileName;

            try
            {
                di = Directory.CreateDirectory("BossSystemErrorLog");
                fileName = di.FullName + "\\AuditLog" + DateTime.Today.ToString("ddmmmyyyy") + ".txt";

                if (File.Exists(fileName))
                    stream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.Write);
                else
                    stream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.Write);

                using (StreamWriter oStreamWriter = new StreamWriter(stream))
                {
                    oStreamWriter.WriteLine("Error occured in class\t\t: " + className);
                    oStreamWriter.WriteLine("In function \t\t\t: " + funcName);
                    oStreamWriter.WriteLine("Program error Code\t\t: " + errorMsg);
                    oStreamWriter.WriteLine("Syster Error stack\t\t: ");
                    oStreamWriter.WriteLine("\t" + systemInnerMsg);
                    oStreamWriter.WriteLine();
                    oStreamWriter.WriteLine("\t****************************\t");
                    oStreamWriter.WriteLine();
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }

        }

        #region **** Not Used to delete later        
       
        //public static bool AddAuditLog(GEnum.AuditLogMode LogMode, GEnum.SystemCode CodeKey, List<object> AuditLogInfo)
        //{
        //    bool ok = false;
        //    string MsgID = BOLib.MsgID.System.AuditLogAddFail;
        //    SYSLog objSYSLog;

        //    objSYSLog = SYSLog.New();

        //    objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;
        //    objSYSLog._logMode = (int?)LogMode;
        //    objSYSLog._userID = AppInfor.currentUserID;
        //    objSYSLog._logCodeKey = (int?)CodeKey;
        //    if (AuditLogInfo != null)
        //    {
        //        bool IsConvertToXML = true;
        //        objSYSLog._logHeader = AuditLogInfo.GetType().ToString();
        //        int i = 0;


        //        if (IsConvertToXML)
        //        {
        //            foreach (object val in AuditLogInfo)
        //            {
        //                string _log = GFunc.ConvertObjectToXML(val);

        //                switch (i)
        //                {
        //                    case 0: objSYSLog._logDetail1 = _log; break;
        //                    case 1: objSYSLog._logDetail2 = _log; break;
        //                    case 2: objSYSLog._logDetail3 = _log; break;
        //                    case 3: objSYSLog._logDetail4 = _log; break;
        //                    case 4: objSYSLog._logDetail5 = _log; break;
        //                    case 5: objSYSLog._logDetail6 = _log; break;
        //                    case 6: objSYSLog._logDetail7 = _log; break;
        //                    case 7: objSYSLog._logDetail8 = _log; break;
        //                    case 8: objSYSLog._logDetail9 = _log; break;

        //                }
        //                i++;
        //            }
        //        }
        //        else
        //        {
        //            objSYSLog._logDetail1 = GFunc.ConvertObjectToXML(AuditLogInfo);
        //        }

        //    }

        //    try
        //    {
        //        using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
        //        {
        //            using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BOSSSystemMasterConnection))
        //            {
        //                cn.Open();
        //                if (objSYSLog.Insert(cn))
        //                {
        //                    MsgID = string.Empty;
        //                      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
        //                    return true;
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteErrorLogFile("SysAuditLogUtility", "AddAuditLog", MsgID, ex.StackTrace);
        //        ok = false;
        //    }
        //    return ok;
        //}
        //internal static bool AddAuditLog(SqlConnection cn, GEnum.AuditLogMode LogMode, GEnum.SystemCode CodeKey, List<object> AuditLogInfo)
        //{
        //    bool ok = false;
        //    string MsgID = BOLib.MsgID.System.AuditLogAddFail;
        //    SYSLog objSYSLog;

        //    objSYSLog = SYSLog.New();

        //    objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;
        //    objSYSLog._logMode = (int?)LogMode;
        //    objSYSLog._userID = AppInfor.currentUserID;
        //    objSYSLog._logCodeKey = (int?)CodeKey;
        //    if (AuditLogInfo != null)
        //    {
        //        bool IsConvertToXML = true;
        //        objSYSLog._logHeader = AuditLogInfo.GetType().ToString();
        //        int i = 0;
        //        switch (CodeKey)
        //        {
        //            case GEnum.SystemCode.System_Option:
        //                IsConvertToXML = false;
        //                break;

        //        }

        //        if (IsConvertToXML)
        //        {
        //            foreach (object val in AuditLogInfo)
        //            {
        //                string _log = GFunc.ConvertObjectToXML(val);

        //                switch (i)
        //                {
        //                    case 0: objSYSLog._logDetail1 = _log; break;
        //                    case 1: objSYSLog._logDetail2 = _log; break;
        //                    case 2: objSYSLog._logDetail3 = _log; break;
        //                    case 3: objSYSLog._logDetail4 = _log; break;
        //                    case 4: objSYSLog._logDetail5 = _log; break;
        //                    case 5: objSYSLog._logDetail6 = _log; break;
        //                    case 6: objSYSLog._logDetail7 = _log; break;
        //                    case 7: objSYSLog._logDetail8 = _log; break;
        //                    case 8: objSYSLog._logDetail9 = _log; break;

        //                }
        //                i++;
        //            }
        //        }
        //        else
        //        {
        //            objSYSLog._logDetail1 = GFunc.ConvertObjectToXML(AuditLogInfo);
        //        }
        //    }

        //    try
        //    {
        //        if (objSYSLog.Insert(cn))
        //        {
        //            MsgID = string.Empty;
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteErrorLogFile("SysAuditLogUtility", "AddAuditLog", MsgID, ex.StackTrace);
        //        ok = false;
        //    }
        //    return ok;
        //}

        internal static bool AddAuditLog(GEnum.AuditLogMode LogMode, GEnum.SystemCode CodeKey, int LogDk, string LogDocID, DateTime LogDocDate, string ErrMsg, string LogDocTypeNm, object AuditLogInfo)
        {
            string MsgID = BOLib.MsgID.System.AuditLogAddFail;

            SYSLog objSYSLog;
            bool ok = false;

            objSYSLog = SYSLog.New();
            objSYSLog._logMode = (int?)LogMode;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._userNm = AppInfor.currentUserName;
            objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;
            objSYSLog._logCodeKey = (int?)CodeKey;
            objSYSLog._logDK = LogDk;
            objSYSLog._logDocID = LogDocID;
            objSYSLog._logDocDate = LogDocDate;
            objSYSLog._logDocTypeNm = LogDocTypeNm;
            objSYSLog._errMsg = ErrMsg;

            if (AuditLogInfo != null)
            {
                objSYSLog._logHeader = AuditLogInfo.GetType().ToString();
                objSYSLog._logDetail1 = GFunc.ConvertObjectToXML(AuditLogInfo);
            }

            try
            {
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BOSSSystemMasterConnection))
                    {
                        cn.Open();
                        if (objSYSLog.Insert(cn))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AddAuditLog", MsgID, ex.StackTrace);
                ok = false;
            }
            return ok;
        }
        internal static bool AddAuditLog(SqlConnection cn, GEnum.AuditLogMode LogMode, GEnum.SystemCode CodeKey, int LogDk, string LogDocID, DateTime LogDocDate, string ErrMsg, string LogDocTypeNm, object AuditLogInfo)
        {
            string MsgID = BOLib.MsgID.System.AuditLogAddFail;

            SYSLog objSYSLog;
            bool ok = false;

            objSYSLog = SYSLog.New();
            objSYSLog._logMode = (int?)LogMode;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;
            objSYSLog._logCodeKey = (int?)CodeKey;
            objSYSLog._logDK = LogDk;
            objSYSLog._logDocID = LogDocID;
            objSYSLog._logDocDate = LogDocDate;
            objSYSLog._logDocTypeNm = LogDocTypeNm;
            objSYSLog._errMsg = ErrMsg;

            if (AuditLogInfo != null)
            {
                objSYSLog._logHeader = AuditLogInfo.GetType().ToString();
                objSYSLog._logDetail1 = GFunc.ConvertObjectToXML(AuditLogInfo);
            }

            try
            {
                if (objSYSLog.Insert(cn))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AddAuditLog", MsgID, ex.StackTrace);
                ok = false;
            }
            return ok;
        }

        internal static bool AddAuditLogList(GEnum.AuditLogMode LogMode, GEnum.SystemCode CodeKey, object[] AuditLogInfoList)
        {
            string MsgID = BOLib.MsgID.System.AuditLogAddFail;
            bool ok = false;
            SYSLog objSYSLog;
            objSYSLog = SYSLog.New();
            objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;
            objSYSLog._logMode = (int?)LogMode;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;

            objSYSLog._logCodeKey = (int?)CodeKey;

            int i = 1;

            if (AuditLogInfoList != null)
            {
                foreach (object o in AuditLogInfoList)
                {
                    objSYSLog._logHeader = AuditLogInfoList[i - 1].GetType().ToString() + ",";

                    switch (i)
                    {
                        case 1: objSYSLog._logDetail1 = GFunc.ConvertObjectToXML(AuditLogInfoList[0]); break;
                        case 2: objSYSLog._logDetail2 = GFunc.ConvertObjectToXML(AuditLogInfoList[1]); break;
                        case 3: objSYSLog._logDetail3 = GFunc.ConvertObjectToXML(AuditLogInfoList[2]); break;
                        case 4: objSYSLog._logDetail4 = GFunc.ConvertObjectToXML(AuditLogInfoList[3]); break;
                        case 5: objSYSLog._logDetail5 = GFunc.ConvertObjectToXML(AuditLogInfoList[4]); break;
                        case 6: objSYSLog._logDetail6 = GFunc.ConvertObjectToXML(AuditLogInfoList[5]); break;
                        case 7: objSYSLog._logDetail7 = GFunc.ConvertObjectToXML(AuditLogInfoList[6]); break;
                        case 8: objSYSLog._logDetail8 = GFunc.ConvertObjectToXML(AuditLogInfoList[7]); break;
                        case 9: objSYSLog._logDetail9 = GFunc.ConvertObjectToXML(AuditLogInfoList[8]); break;
                    }
                    i++;
                }
            }

            try
            {
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BOSSSystemMasterConnection))
                    {
                        cn.Open();
                        if (objSYSLog.Insert(cn))
                        {
                            MsgID = string.Empty;
                            ok = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AuditLogInfoList", MsgID, ex.StackTrace);
                ok = false;
            }
            return ok;
        }
        internal static bool AddAuditLogList(SqlConnection cn, GEnum.AuditLogMode LogMode, GEnum.SystemCode CodeKey, object[] AuditLogInfoList)
        {
            string MsgID = BOLib.MsgID.System.AuditLogAddFail;
            bool ok = false;
            SYSLog objSYSLog;
            objSYSLog = SYSLog.New();
            objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;
            objSYSLog._logMode = (int?)LogMode;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;

            objSYSLog._logCodeKey = (int?)CodeKey;

            int i = 1;

            if (AuditLogInfoList != null)
            {
                foreach (object o in AuditLogInfoList)
                {
                    objSYSLog._logHeader = AuditLogInfoList[i - 1].GetType().ToString() + ",";

                    switch (i)
                    {
                        case 1: objSYSLog._logDetail1 = GFunc.ConvertObjectToXML(AuditLogInfoList[0]); break;
                        case 2: objSYSLog._logDetail2 = GFunc.ConvertObjectToXML(AuditLogInfoList[1]); break;
                        case 3: objSYSLog._logDetail3 = GFunc.ConvertObjectToXML(AuditLogInfoList[2]); break;
                        case 4: objSYSLog._logDetail4 = GFunc.ConvertObjectToXML(AuditLogInfoList[3]); break;
                        case 5: objSYSLog._logDetail5 = GFunc.ConvertObjectToXML(AuditLogInfoList[4]); break;
                        case 6: objSYSLog._logDetail6 = GFunc.ConvertObjectToXML(AuditLogInfoList[5]); break;
                        case 7: objSYSLog._logDetail7 = GFunc.ConvertObjectToXML(AuditLogInfoList[6]); break;
                        case 8: objSYSLog._logDetail8 = GFunc.ConvertObjectToXML(AuditLogInfoList[7]); break;
                        case 9: objSYSLog._logDetail9 = GFunc.ConvertObjectToXML(AuditLogInfoList[8]); break;
                    }
                    i++;
                }
            }

            try
            {
                if (objSYSLog.Insert(cn))
                {
                    MsgID = string.Empty;
                    ok = true;
                }
            }
            catch (Exception ex)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AuditLogInfoList", MsgID, ex.StackTrace);
                ok = false;
            }
            return ok;
        }

        internal static bool AddAuditLogList(GEnum.AuditLogMode LogMode, GEnum.SystemCode CodeKey, int LogDk, string LogDocID, DateTime LogDocDate, string LogDocTypeNm, object[] AuditLogInfoList)
        {
            string MsgID = BOLib.MsgID.System.AuditLogAddFail;
            bool ok = false;
            SYSLog objSYSLog;
            objSYSLog = SYSLog.New();
            objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;
            objSYSLog._logMode = (int?)LogMode;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;

            objSYSLog._logCodeKey = (int?)CodeKey;
            objSYSLog._logDK = LogDk;
            objSYSLog._logDocID = LogDocID;
            objSYSLog._logDocDate = LogDocDate;
            objSYSLog._logDocTypeNm = LogDocTypeNm;

            int i = 1;
            if (AuditLogInfoList != null)
            {
                foreach (object o in AuditLogInfoList)
                {
                    objSYSLog._logHeader = AuditLogInfoList[i - 1].GetType().ToString() + ",";

                    switch (i)
                    {
                        case 1: objSYSLog._logDetail1 = GFunc.ConvertObjectToXML(AuditLogInfoList[0]); break;
                        case 2: objSYSLog._logDetail2 = GFunc.ConvertObjectToXML(AuditLogInfoList[1]); break;
                        case 3: objSYSLog._logDetail3 = GFunc.ConvertObjectToXML(AuditLogInfoList[2]); break;
                        case 4: objSYSLog._logDetail4 = GFunc.ConvertObjectToXML(AuditLogInfoList[3]); break;
                        case 5: objSYSLog._logDetail5 = GFunc.ConvertObjectToXML(AuditLogInfoList[4]); break;
                        case 6: objSYSLog._logDetail6 = GFunc.ConvertObjectToXML(AuditLogInfoList[5]); break;
                        case 7: objSYSLog._logDetail7 = GFunc.ConvertObjectToXML(AuditLogInfoList[6]); break;
                        case 8: objSYSLog._logDetail8 = GFunc.ConvertObjectToXML(AuditLogInfoList[7]); break;
                        case 9: objSYSLog._logDetail9 = GFunc.ConvertObjectToXML(AuditLogInfoList[8]); break;
                    }
                    i++;
                }
            }
            try
            {
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BOSSSystemMasterConnection))
                    {
                        cn.Open();
                        if (objSYSLog.Insert(cn))
                        {
                            MsgID = string.Empty;
                            ok = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AuditLogInfoList", MsgID, ex.StackTrace);
                ok = false;
            }
            return ok;
        }
        internal static bool AddAuditLogList(SqlConnection cn, GEnum.AuditLogMode LogMode, GEnum.SystemCode CodeKey, int LogDk, string LogDocID, DateTime LogDocDate, string LogDocTypeNm, object[] AuditLogInfoList)
        {
            string MsgID = BOLib.MsgID.System.AuditLogAddFail;
            bool ok = false;
            SYSLog objSYSLog;
            objSYSLog = SYSLog.New();
            objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;
            objSYSLog._logMode = (int?)LogMode;
            objSYSLog._userID = AppInfor.currentUserID;
            objSYSLog._logType = (int?)GEnum.AuditLogType.AuditTrail;

            objSYSLog._logCodeKey = (int?)CodeKey;
            objSYSLog._logDK = LogDk;
            objSYSLog._logDocID = LogDocID;
            objSYSLog._logDocDate = LogDocDate;
            objSYSLog._logDocTypeNm = LogDocTypeNm;

            int i = 1;
            if (AuditLogInfoList != null)
            {
                foreach (object o in AuditLogInfoList)
                {
                    objSYSLog._logHeader = AuditLogInfoList[i - 1].GetType().ToString() + ",";

                    switch (i)
                    {
                        case 1: objSYSLog._logDetail1 = GFunc.ConvertObjectToXML(AuditLogInfoList[0]); break;
                        case 2: objSYSLog._logDetail2 = GFunc.ConvertObjectToXML(AuditLogInfoList[1]); break;
                        case 3: objSYSLog._logDetail3 = GFunc.ConvertObjectToXML(AuditLogInfoList[2]); break;
                        case 4: objSYSLog._logDetail4 = GFunc.ConvertObjectToXML(AuditLogInfoList[3]); break;
                        case 5: objSYSLog._logDetail5 = GFunc.ConvertObjectToXML(AuditLogInfoList[4]); break;
                        case 6: objSYSLog._logDetail6 = GFunc.ConvertObjectToXML(AuditLogInfoList[5]); break;
                        case 7: objSYSLog._logDetail7 = GFunc.ConvertObjectToXML(AuditLogInfoList[6]); break;
                        case 8: objSYSLog._logDetail8 = GFunc.ConvertObjectToXML(AuditLogInfoList[7]); break;
                        case 9: objSYSLog._logDetail9 = GFunc.ConvertObjectToXML(AuditLogInfoList[8]); break;
                    }
                    i++;
                }
            }
            try
            {
                if (objSYSLog.Insert(cn))
                {
                    MsgID = string.Empty;
                    ok = true;
                }
            }
            catch (Exception ex)
            {
                WriteErrorLogFile("SysAuditLogUtility", "AuditLogInfoList", MsgID, ex.StackTrace);
                ok = false;
            }
            return ok;
        }        
        #endregion  **** Not Used to comment later
    }
}


