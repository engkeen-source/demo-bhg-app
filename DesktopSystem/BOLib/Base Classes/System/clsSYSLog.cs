using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Collections;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SYSLog : Csla.BusinessBase<SYSLog>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _uid = 0;
        internal DateTime? _logDateTime = null;
        internal DateTime? _logDate = null;
        internal int? _logType = 0;
        internal int? _logMode = 0;
        internal int? _logCodeKey = 0;
        internal string _logCodeID = string.Empty;
        internal int? _logDK = 0;
        internal string _logDocID = string.Empty;
        internal DateTime? _logDocDate = null;
        internal string _logDocTypeNm = string.Empty;
        internal string _logHeader = string.Empty;
        internal string _logDetail1 = string.Empty;
        internal string _logDetail2 = string.Empty;
        internal string _logDetail3 = string.Empty;
        internal string _logDetail4 = string.Empty;
        internal string _logDetail5 = string.Empty;
        internal string _logDetail6 = string.Empty;
        internal string _logDetail7 = string.Empty;
        internal string _logDetail8 = string.Empty;
        internal string _logDetail9 = string.Empty;
        internal string _logKeyWords = string.Empty;
        internal string _sysOption = string.Empty;
        internal string _userID = string.Empty;
        internal string _userNm = string.Empty;
        internal string _actFrm = string.Empty;
        internal string _actCtrl = string.Empty;
        internal string _errNum = string.Empty;
        internal string _errMsg = string.Empty;
        internal string _errDetails = string.Empty;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public int? UID
        {
            get
            {
                return _uid;
            }
        }

        public DateTime? LogDateTime
        {
            get
            {
                return _logDateTime;
            }
        }

        public DateTime? LogDate
        {
            get
            {
                return _logDate;
            }
        }

        public int? LogType
        {
            get
            {
                return _logType;
            }
        }

        public int? LogMode
        {
            get
            {
                return _logMode;
            }
        }

        public int? LogCodeKey
        {
            get
            {
                return _logCodeKey;
            }
        }

        public string LogCodeID
        {
            get
            {
                return _logCodeID;
            }
        }

        public int? LogDK
        {
            get
            {
                return _logDK;
            }
        }

        public string LogDocID
        {
            get
            {
                return _logDocID;
            }
        }

        public DateTime? LogDocDate
        {
            get
            {
                return _logDocDate;
            }
        }

        public string LogDocTypeNm
        {
            get
            {
                return _logDocTypeNm;
            }
        }

        public string LogHeader
        {
            get
            {
                return _logHeader;
            }
        }

        public string LogDetail1
        {
            get
            {
                return _logDetail1;
            }
        }

        public string LogDetail2
        {
            get
            {
                return _logDetail2;
            }
        }

        public string LogDetail3
        {
            get
            {
                return _logDetail3;
            }
        }

        public string LogDetail4
        {
            get
            {
                return _logDetail4;
            }
        }

        public string LogDetail5
        {
            get
            {
                return _logDetail5;
            }
        }

        public string LogDetail6
        {
            get
            {
                return _logDetail6;
            }
        }

        public string LogDetail7
        {
            get
            {
                return _logDetail7;
            }
        }

        public string LogDetail8
        {
            get
            {
                return _logDetail8;
            }
        }

        public string LogDetail9
        {
            get
            {
                return _logDetail9;
            }
        }

        public string LogKeyWords
        {
            get
            {
                return _logKeyWords;
            }
        }

        public string SysOption
        {
            get
            {
                return _sysOption;
            }            
        }

        public string UserID
        {
            get
            {
                return _userID;
            }
        }

        public string UserNm
        {
            get
            {
                return _userNm;
            }
        }

        public string ActFrm
        {
            get
            {
                return _actFrm;
            }
        }

        public string ActCtrl
        {
            get
            {
                return _actCtrl;
            }
        }

        public string ErrNum
        {
            get
            {
                return _errNum;
            }
        }

        public string ErrMsg
        {
            get
            {
                return _errMsg;
            }
        }

        public string ErrDetails
        {
            get
            {
                return _errDetails;
            }
        }

        public string Custom1
        {
            get
            {                
                return _custom1;
            }
            set
            {
                _custom1 = value;
                PropertyHasChanged("Custom1");              
            }
        }

        public string Custom2
        {
            get
            {               
                return _custom2;
            }
            set
            {
                _custom2 = value;
                PropertyHasChanged("Custom2");               
            }
        }

        public string Custom3
        {
            get
            {              
                return _custom3;
            }
            set
            {

                _custom3 = value;
                PropertyHasChanged("Custom3");
            }
        }

        protected override object GetIdValue()
        {
            return _uid.ToString();
        }

        #endregion //Business Properties and Methods

        #region Validation Rules
        private void AddCustomRules()
        {
            //add custom/non-generated rules here...
        }

        private void AddCommonRules()
        {
            ////
            //// LogDateTime
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "LogDateTimeString");
            ////
            //// LogDate
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "LogDateString");
            ////
            //// LogCodeID
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LogCodeID", 8));
            ////
            //// LogDocID
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LogDocID", 50));
            ////
            //// LogDocTypeNm
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LogDocTypeNm", 50));
            ////
            //// LogHeader
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LogHeader", 255));
            ////
            //// LogKeyWords
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LogKeyWords", 255));
            ////
            //// UserID
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("UserID", 50));
            ////
            //// UserNm
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("UserNm", 50));
            ////
            //// ActFrm
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ActFrm", 50));
            ////
            //// ActCtrl
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ActCtrl", 50));
            ////
            //// ErrNum
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ErrNum", 255));
            ////
            //// Custom1
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom1", 255));
            ////
            //// Custom2
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom2", 255));
            ////
            //// Custom3
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom3", 255));
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal SYSLog()
        { /* require use of factory method */ }

        internal static SYSLog New()
        {
           
            SYSLog child = new SYSLog();
            
            return child;
        }

        internal static SYSLog NewChild()
        {
           
            SYSLog child = new SYSLog();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static SYSLog Get(SafeDataReader dr)
        {
            
            SYSLog child = new SYSLog();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SYSLog Get(int? uid)
        {
            
            SYSLog child = new SYSLog();
            child.Fetch(new Criteria(uid, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _uid = null;
            public int? _option = null;
            public int? _logCodeKey = null;
            public int? _logDK = null;

            internal Criteria()
            {
            }

            internal Criteria(int? Uid)
            {
                _uid = Uid;
            }

            internal Criteria(int? Uid, int? Option)
            {
                _uid = Uid;
                _option = Option;
            }

            internal Criteria(int? LogCodeKye, int? LogDK, int? Option)
            {
                _logCodeKey = LogCodeKye;
                _logDK = LogDK;
                _option = Option;
            }

        }

        #endregion //Criteria

        #region Data Access - Fetch

        internal bool Fetch(Criteria criteria)
        {
            bool retValue = false;
  
            // Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();
                retValue = this.Fetch(cn, criteria);
            }
           
            return retValue;
        }

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSLog_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@Uid", criteria._uid);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                

                // Using data reader as record set.
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    // If data reader can read, continue...
                    while (dr.Read())
                    {
                        this.Fetch(dr);
                    }
                   
                }	// Already close and dispose data reader.

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;    

            }// Already close and dispose sql connection.
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _uid = dr.GetInt32("UID");
            _logDateTime = dr.GetDateTime("LogDateTime");
            _logDate = dr.GetDateTime("LogDate");
            _logType = dr.GetInt32("LogType");
            _logMode = dr.GetInt32("LogMode");
            _logCodeKey = dr.GetInt32("LogCodeKey");
            _logCodeID = dr.GetString("LogCodeID");
            _logDK = dr.GetInt32("LogDK");
            _logDocID = dr.GetString("LogDocID");
            _logDocDate = dr.GetDateTime("LogDocDate");
            _logDocTypeNm = dr.GetString("LogDocTypeNm");
            _logHeader = dr.GetString("LogHeader");
            //Start changed by Thida
            //LogDetail1 = dr.GetString("LogDetail1");
            //LogDetail2 = dr.GetString("LogDetail2");
            //LogDetail3 = dr.GetString("LogDetail3");
            //LogDetail4 = dr.GetString("LogDetail4");
            //LogDetail5 = dr.GetString("LogDetail5");
            //LogDetail6 = dr.GetString("LogDetail6");
            //LogDetail7 = dr.GetString("LogDetail7");
            //LogDetail8 = dr.GetString("LogDetail8");
            //LogDetail9 = dr.GetString("LogDetail9");
            //SysOption = dr.GetString("SysOption");
            //End changed by Thida
            _logKeyWords = dr.GetString("LogKeyWords");                
            _userID = dr.GetString("UserID");
            _userNm = dr.GetString("UserNm");
            _actFrm = dr.GetString("ActFrm");
            _actCtrl = dr.GetString("ActCtrl");
            _errNum = dr.GetString("ErrNum");
            _errMsg = dr.GetString("ErrMsg");
            _errDetails = dr.GetString("ErrDetails");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            ValidationRules.CheckRules();
            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert()
        {
            bool retValue = false;
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BOSSSystemMasterConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope                
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn)
        {
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSLog_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);
                

                cm.Parameters.AddWithValue("@NewUid", 0);

                if (_uid == null)
                    cm.Parameters.AddWithValue("@Uid", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Uid", _uid);

                if (_logDateTime == null)
                    cm.Parameters.AddWithValue("@LogDateTime", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDateTime", _logDateTime.Value);

                if (_logDate == null)
                    cm.Parameters.AddWithValue("@LogDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDate", _logDate.Value);

                if (_logType == null)
                    cm.Parameters.AddWithValue("@LogType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogType", _logType);

                if (_logMode == null)
                    cm.Parameters.AddWithValue("@LogMode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogMode", _logMode);

                if (_logCodeKey == null)
                    cm.Parameters.AddWithValue("@LogCodeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogCodeKey", _logCodeKey);

                if (_logCodeID == null)
                    cm.Parameters.AddWithValue("@LogCodeID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogCodeID", _logCodeID);

                if (_logDK == null)
                    cm.Parameters.AddWithValue("@LogDK", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDK", _logDK);

                if (_logDocID == null)
                    cm.Parameters.AddWithValue("@LogDocID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDocID", _logDocID);

                if (_logDocDate == null)
                    cm.Parameters.AddWithValue("@LogDocDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDocDate", _logDocDate.Value);

                if (_logDocTypeNm == null)
                    cm.Parameters.AddWithValue("@LogDocTypeNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDocTypeNm", _logDocTypeNm);

                if (_logHeader == null)
                    cm.Parameters.AddWithValue("@LogHeader", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogHeader", _logHeader);

                if (_logDetail1 == null)
                    cm.Parameters.AddWithValue("@LogDetail1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDetail1", _logDetail1);

                if (_logDetail2 == null)
                    cm.Parameters.AddWithValue("@LogDetail2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDetail2", _logDetail2);

                if (_logDetail3 == null)
                    cm.Parameters.AddWithValue("@LogDetail3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDetail3", _logDetail3);

                if (_logDetail4 == null)
                    cm.Parameters.AddWithValue("@LogDetail4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDetail4", _logDetail4);

                if (_logDetail5 == null)
                    cm.Parameters.AddWithValue("@LogDetail5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDetail5", _logDetail5);

                if (_logDetail6 == null)
                    cm.Parameters.AddWithValue("@LogDetail6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDetail6", _logDetail6);

                if (_logDetail7 == null)
                    cm.Parameters.AddWithValue("@LogDetail7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDetail7", _logDetail7);

                if (_logDetail8 == null)
                    cm.Parameters.AddWithValue("@LogDetail8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDetail8", _logDetail8);

                if (_logDetail9 == null)
                    cm.Parameters.AddWithValue("@LogDetail9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDetail9", _logDetail9);

                if (_logKeyWords == null)
                    cm.Parameters.AddWithValue("@LogKeyWords", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogKeyWords", _logKeyWords);

                if (_sysOption == null)
                    cm.Parameters.AddWithValue("@SysOption", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SysOption", _sysOption);

                if (_userID == null)
                    cm.Parameters.AddWithValue("@UserID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserID", _userID);

                if (_userNm == null)
                    cm.Parameters.AddWithValue("@UserNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserNm", _userNm);

                if (_actFrm == null)
                    cm.Parameters.AddWithValue("@ActFrm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ActFrm", _actFrm);

                if (_actCtrl == null)
                    cm.Parameters.AddWithValue("@ActCtrl", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ActCtrl", _actCtrl);

                if (_errNum == null)
                    cm.Parameters.AddWithValue("@ErrNum", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ErrNum", _errNum);

                if (_errMsg == null)
                    cm.Parameters.AddWithValue("@ErrMsg", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ErrMsg", _errMsg);

                if (_errDetails == null)
                    cm.Parameters.AddWithValue("@ErrDetails", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ErrDetails", _errDetails);

                if (_custom1 == null)
                    cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom1", _custom1);

                if (_custom2 == null)
                    cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom2", _custom2);

                if (_custom3 == null)
                    cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom3", _custom3);

                
                cm.Parameters["@NewUid"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                
                _uid = (int)cm.Parameters["@NewUid"].Value;

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;    
            }// Already close and dispose sql connection.
        }

        #endregion //Data Access - Insert       

        #region Data Access - Delete

        internal bool Delete(Criteria criteria)
        {
            bool retValue = false;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call delete method.
                    retValue = this.Delete(cn, criteria);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSLog_Delete";

                
                cm.Parameters.AddWithValue("@Uid", criteria._uid);

                
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

               

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;    
            }// Already close and dispose sql connection.
        }

        #endregion //Data Access - Delete
    }
}