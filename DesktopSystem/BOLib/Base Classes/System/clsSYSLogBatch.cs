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
    public class SYSLogBatch : Csla.BusinessBase<SYSLogBatch>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _uid = 0;
        internal DateTime? _logBatchDateTime = null;
        internal DateTime? _logBatchDate = null;
        internal int? _logBatchMode = null;
        internal int? _logBatchDC = null;
        internal int? _logBatchDK = null;
        internal string _logDocID = string.Empty;
        internal DateTime? _logDocDate = null;
        internal string _logDocTypeNm = string.Empty;
        internal bool? _logBatchPostDone = false;
        internal DateTime? _logBatchPostDate = null;
        internal int? _logUserKey = null;
        internal int? _postByUserKey = 0;
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

        public DateTime? LogBatchDateTime
        {
            get
            {
                return _logBatchDateTime;
            }
            set
            {
                _logBatchDateTime = value;
                PropertyHasChanged("LogBatchDateTime");
            }
        }

        public DateTime? LogBatchDate
        {
            get
            {
                return _logBatchDate;
            }
            set
            {
                _logBatchDate = value;
                PropertyHasChanged("LogBatchDate");
            }
        }

        public int? LogBatchMode
        {
            get
            {
                return _logBatchMode;
            }
            set
            {
                _logBatchMode = value;
                PropertyHasChanged("LogBatchMode");
            }
        }

        public int? LogBatchDC
        {
            get
            {
                return _logBatchDC;
            }
            set
            {
                _logBatchDC = value;
                PropertyHasChanged("LogBatchDC");
            }
        }

        public int? LogBatchDK
        {
            get
            {
                return _logBatchDK;
            }
            set
            {
                _logBatchDK = value;
                PropertyHasChanged("LogBatchDK");
            }
        }

        public string LogDocID
        {
            get
            {
                return _logDocID;
            }
            set
            {
                _logDocID = value;
                PropertyHasChanged("LogDocID");
            }
        }

        public DateTime? LogDocDate
        {
            get
            {
                return _logDocDate;
            }
            set
            {
                _logDocDate = value;
                PropertyHasChanged("LogDocDate");
            }
        }

        public string LogDocTypeNm
        {
            get
            {
                return _logDocTypeNm;
            }
            set
            {
                _logDocTypeNm = value;
                PropertyHasChanged("LogDocTypeNm");
            }
        }

        public bool? LogBatchPostDone
        {
            get
            {
                return _logBatchPostDone;
            }
            set
            {
                _logBatchPostDone = value;
                PropertyHasChanged("LogBatchPostDone");
            }
        }

        public DateTime? LogBatchPostDate
        {
            get
            {
                return _logBatchPostDate;
            }
            set
            {
                _logBatchPostDate = value;
                PropertyHasChanged("LogBatchPostDate");
            }
        }

        public int? LogUserKey
        {
            get
            {
                return _logUserKey;
            }
            set
            {
                _logUserKey = value;
                PropertyHasChanged("LogUserKey");
            }
        }

        public int? PostByUserKey
        {
            get
            {
                return _postByUserKey;
            }
            set
            {
                _postByUserKey = value;
                PropertyHasChanged("PostByUserKey");
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
            //// LogBatchDateTime
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "LogBatchDateTimeString");
            ////
            //// LogBatchDate
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "LogBatchDateString");
            ////
            //// LogDocID
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LogDocID", 50));
            ////
            //// LogDocTypeNm
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LogDocTypeNm", 50));
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

        internal SYSLogBatch()
        { /* require use of factory method */ }

        internal static SYSLogBatch New()
        {
            
            SYSLogBatch child = new SYSLogBatch();
            
            return child;
        }

        internal static SYSLogBatch NewChild()
        {
            
            SYSLogBatch child = new SYSLogBatch();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static SYSLogBatch Get(SafeDataReader dr)
        {
            
            SYSLogBatch child = new SYSLogBatch();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SYSLogBatch Get(int? uid)
        {
            
            SYSLogBatch child = new SYSLogBatch();
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
            public int? _logBatchDC = null;
            public int? _logBatchDK = null;
            public DateTime? _logBatchDate = null;

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

            internal Criteria(int? LogBatchDC, int? LogBatchDK, DateTime LogBatchDate, int? Option)
            {
                _logBatchDC = LogBatchDC;
                _logBatchDK = LogBatchDK;
                _logBatchDate = LogBatchDate; 
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
                cm.CommandText = "SYSLogBatch_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@Uid", criteria._uid);
                cm.Parameters.AddWithValue("@LogBatchDCid", criteria._logBatchDC);
                cm.Parameters.AddWithValue("@LogBatchDK", criteria._logBatchDK);
                cm.Parameters.AddWithValue("@LogBatchDateTime", criteria._logBatchDate);

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
            _logBatchDateTime = dr.GetDateTime("LogBatchDateTime");
            _logBatchDate = dr.GetDateTime("LogBatchDate");
            _logBatchMode = dr.GetInt32("LogBatchMode");
            _logBatchDC = dr.GetInt32("LogBatchDC");
            _logBatchDK = dr.GetInt32("LogBatchDK");
            _logDocID = dr.GetString("LogDocID");
            _logDocDate = dr.GetDateTime("LogDocDate");
            _logDocTypeNm = dr.GetString("LogDocTypeNm");
            _logBatchPostDone = dr.GetBoolean("LogBatchPostDone");
            _logBatchPostDate = dr.GetDateTime("LogBatchPostDate");
            _logUserKey = dr.GetInt32("LogUserKey");
            _postByUserKey = dr.GetInt32("PostByUserKey");
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
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
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
                // Get current user key
                _logUserKey = AppInfor.currentUserKey;
                _postByUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSLogBatch_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);
                

                cm.Parameters.AddWithValue("@NewUid", _uid);

                if (_uid == null)
                    cm.Parameters.AddWithValue("@Uid", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Uid", _uid);

                if (_logBatchDateTime == null)
                    cm.Parameters.AddWithValue("@LogBatchDateTime", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogBatchDateTime", _logBatchDateTime.Value);

                if (_logBatchDate == null)
                    cm.Parameters.AddWithValue("@LogBatchDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogBatchDate", _logBatchDate.Value);

                if (_logBatchMode == null)
                    cm.Parameters.AddWithValue("@LogBatchMode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogBatchMode", _logBatchMode);

                if (_logBatchDC == null)
                    cm.Parameters.AddWithValue("@LogBatchDC", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogBatchDC", _logBatchDC);

                if (_logBatchDK == null)
                    cm.Parameters.AddWithValue("@LogBatchDK", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogBatchDK", _logBatchDK);

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

                if (_logBatchPostDone == null)
                    cm.Parameters.AddWithValue("@LogBatchPostDone", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogBatchPostDone", _logBatchPostDone);

                if (_logBatchPostDate == null)
                    cm.Parameters.AddWithValue("@LogBatchPostDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogBatchPostDate", _logBatchPostDate.Value);

                if (_logUserKey == null)
                    cm.Parameters.AddWithValue("@LogUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogUserKey", _logUserKey);

                if (_postByUserKey == null)
                    cm.Parameters.AddWithValue("@PostByUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PostByUserKey", _postByUserKey);

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

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                
                cm.Parameters["@NewUid"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                
                _uid = (int)cm.Parameters["@NewUid"].Value;

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;    
            }// Already close and dispose sql connection.
        }

        #endregion //Data Access - Insert

        #region Data Access - Update

        internal bool Update()
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

                    // Call update method.
                    retValue = this.Update(cn);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Update(SqlConnection cn)
        {
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSLogBatch_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                
                cm.Parameters.AddWithValue("@NewUid", 0);

                if (_uid == null)
                    cm.Parameters.AddWithValue("@Uid", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Uid", _uid);

                if (_logBatchDateTime == null)
                    cm.Parameters.AddWithValue("@LogBatchDateTime", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogBatchDateTime", _logBatchDateTime.Value);

                if (_logBatchDate == null)
                    cm.Parameters.AddWithValue("@LogBatchDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogBatchDate", _logBatchDate.Value);

                if (_logBatchMode == null)
                    cm.Parameters.AddWithValue("@LogBatchMode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogBatchMode", _logBatchMode);

                if (_logBatchDC == null)
                    cm.Parameters.AddWithValue("@LogBatchDC", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogBatchDC", _logBatchDC);

                if (_logBatchDK == null)
                    cm.Parameters.AddWithValue("@LogBatchDK", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogBatchDK", _logBatchDK);

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

                if (_logBatchPostDone == null)
                    cm.Parameters.AddWithValue("@LogBatchPostDone", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogBatchPostDone", _logBatchPostDone);

                if (_logBatchPostDate == null)
                    cm.Parameters.AddWithValue("@LogBatchPostDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogBatchPostDate", _logBatchPostDate.Value);

                if (_logUserKey == null)
                    cm.Parameters.AddWithValue("@LogUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogUserKey", _logUserKey);

                if (_postByUserKey == null)
                    cm.Parameters.AddWithValue("@PostByUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PostByUserKey", _postByUserKey);

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

                

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;    
            }// Already close and dispose sql connection.
        }
        #endregion //Data Access - Update

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
                cm.CommandText = "SYSLogBatch_Delete";

                
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
