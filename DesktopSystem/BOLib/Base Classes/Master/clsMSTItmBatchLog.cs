

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
    public class MSTItmBatchLog : Csla.BusinessBase<MSTItmBatchLog>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _batchKey = null;
        internal int? _logDC = null;
        internal int? _logDK = null;
        internal int? _logDItm = null;
        internal short? _logType = null;
        internal short? _logSign = null;
        internal DateTime? _logDocDate = null;
        internal decimal? _batchQty = null;
        internal int? _purgeKeep = null;
        internal bool? _purgeData = null;

        public int? BatchKey
        {
            get
            {
                return _batchKey;
            }
            set
            {
                _batchKey = value;
                PropertyHasChanged("BatchKey");
            }
        }

        public int? LogDC
        {
            get
            {
                return _logDC;
            }
            set
            {
                _logDC = value;
                PropertyHasChanged("LogDC");
            }
        }

        public int? LogDK
        {
            get
            {
                return _logDK;
            }
            set
            {
                _logDK = value;
                PropertyHasChanged("LogDK");
            }
        }

        public int? LogDItm
        {
            get
            {
                return _logDItm;
            }
            set
            {
                _logDItm = value;
                PropertyHasChanged("LogDItm");
            }
        }

        public short? LogType
        {
            get
            {
                return _logType;
            }
            set
            {
                _logType = value;
                PropertyHasChanged("LogType");
            }
        }

        public short? LogSign
        {
            get
            {
                return _logSign;
            }
            set
            {
                _logSign = value;
                PropertyHasChanged("LogSign");
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

        public decimal? BatchQty
        {
            get
            {
                return _batchQty;
            }
            set
            {
                _batchQty = value;
                PropertyHasChanged("BatchQty");
            }
        }

        public int? PurgeKeep
        {
            get
            {
                return _purgeKeep;
            }
            set
            {
                _purgeKeep = value;
                PropertyHasChanged("PurgeKeep");
            }
        }

        public bool? PurgeData
        {
            get
            {
                return _purgeData;
            }
            set
            {
                _purgeData = value;
                PropertyHasChanged("PurgeData");
            }
        }

        protected override object GetIdValue()
        {
            return _batchKey.ToString() + _logDC.ToString() + _logDK.ToString() + _logDItm.ToString();
        }

        #endregion //Business Properties and Methods

        #region Validation Rules
        private void AddCustomRules()
        {
            //add custom/non-generated rules here...
        }

        private void AddCommonRules()
        {
            /*

            */
        }

        protected override void AddBusinessRules()
        {
            /*
           AddCommonRules();
           AddCustomRules();
            */
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal MSTItmBatchLog()
        { /* require use of factory method */ }

        internal static MSTItmBatchLog New()
        {            
            MSTItmBatchLog child = new MSTItmBatchLog();           
            return child;
        }

        internal static MSTItmBatchLog NewChild()
        {            
            MSTItmBatchLog child = new MSTItmBatchLog();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();           
            return child;
        }

        internal static MSTItmBatchLog Get(SafeDataReader dr)
        {            
            MSTItmBatchLog child = new MSTItmBatchLog();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTItmBatchLog Get(int? batchKey, int? logDC, int? logDK, int? logDItm)
        {           
            MSTItmBatchLog child = new MSTItmBatchLog();
            child.Fetch(new Criteria(batchKey, logDC, logDK, logDItm, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _batchKey = null;
            public int? _logDC = null;
            public int? _logDK = null;
            public int? _logDItm = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? BatchKey, int? LogDC, int? LogDK, int? LogDItm)
            {
                _batchKey = BatchKey;
                _logDC = LogDC;
                _logDK = LogDK;
                _logDItm = LogDItm;
            }

            internal Criteria(int? BatchKey, int? LogDC, int? LogDK, int? LogDItm, int? Option)
            {
                _batchKey = BatchKey;
                _logDC = LogDC;
                _logDK = LogDK;
                _logDItm = LogDItm;
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
            bool retValue = false;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmBatchLog_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                    
                cm.Parameters.AddWithValue("@BatchKey", criteria._batchKey);
                cm.Parameters.AddWithValue("@LogDC", criteria._logDC);
                cm.Parameters.AddWithValue("@LogDK", criteria._logDK);
                cm.Parameters.AddWithValue("@LogDItm", criteria._logDItm);                 

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                // Using data reader as record set.
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    // If data reader can read, continue...
                    while (dr.Read())
                    {
                        retValue = this.Fetch(dr);
                    }
                }	// Already close and dispose data reader.

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _batchKey = dr.GetInt32("BatchKey");
            _logDC = dr.GetInt32("LogDC");
            _logDK = dr.GetInt32("LogDK");
            _logDItm = dr.GetInt32("LogDItm");
            _logType = dr.GetInt16("LogType");
            _logSign = dr.GetInt16("LogSign");
            _logDocDate = dr.GetDateTime("LogDocDate");
            _batchQty = dr.GetDecimal("BatchQty");
            _purgeKeep = dr.GetInt32("PurgeKeep");
            _purgeData = dr.GetBoolean("PurgeData");
            ValidationRules.CheckRules();
            return true;
            
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? batchKey, out int? logDC, out int? logDK, out int? logDItm)
        {
            bool retValue = false;            
            batchKey = null;
            logDC = null;
            logDK = null;
            logDItm = null;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out batchKey, out logDC, out logDK, out logDItm);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? batchKey, out int? logDC, out int? logDK, out int? logDItm)
        {
            batchKey = 0;
            logDC = 0;
            logDK = 0;
            logDItm = 0;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmBatchLog_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0); 
                if (_batchKey == null)
                    cm.Parameters.AddWithValue("@BatchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchKey", _batchKey);

                if (_logDC == null)
                    cm.Parameters.AddWithValue("@LogDC", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDC", _logDC);

                if (_logDK == null)
                    cm.Parameters.AddWithValue("@LogDK", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDK", _logDK);

                if (_logDItm == null)
                    cm.Parameters.AddWithValue("@LogDItm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDItm", _logDItm);

                if (_logType == null)
                    cm.Parameters.AddWithValue("@LogType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogType", _logType);

                if (_logSign == null)
                    cm.Parameters.AddWithValue("@LogSign", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogSign", _logSign);

                if (_logDocDate == null)
                    cm.Parameters.AddWithValue("@LogDocDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDocDate", _logDocDate.Value);

                if (_batchQty == null)
                    cm.Parameters.AddWithValue("@BatchQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchQty", _batchQty);

                if (_purgeKeep == null)
                    cm.Parameters.AddWithValue("@PurgeKeep", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurgeKeep", _purgeKeep);

                if (_purgeData == null)
                    cm.Parameters.AddWithValue("@PurgeData", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurgeData", _purgeData);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

              
                cm.ExecuteNonQuery();    
                // Check Return Value -- Changed By Richard
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
                cm.CommandText = "MSTItmBatchLog_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewBatchKey", 0);
                cm.Parameters.AddWithValue("@NewLogDC", 0);
                cm.Parameters.AddWithValue("@NewLogDK", 0);
                cm.Parameters.AddWithValue("@NewLogDItm", 0);

                if (_batchKey == null)
                    cm.Parameters.AddWithValue("@BatchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchKey", _batchKey);

                if (_logDC == null)
                    cm.Parameters.AddWithValue("@LogDC", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDC", _logDC);

                if (_logDK == null)
                    cm.Parameters.AddWithValue("@LogDK", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDK", _logDK);

                if (_logDItm == null)
                    cm.Parameters.AddWithValue("@LogDItm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDItm", _logDItm);

                if (_logType == null)
                    cm.Parameters.AddWithValue("@LogType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogType", _logType);

                if (_logSign == null)
                    cm.Parameters.AddWithValue("@LogSign", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogSign", _logSign);

                if (_logDocDate == null)
                    cm.Parameters.AddWithValue("@LogDocDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDocDate", _logDocDate.Value);

                if (_batchQty == null)
                    cm.Parameters.AddWithValue("@BatchQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchQty", _batchQty);

                if (_purgeKeep == null)
                    cm.Parameters.AddWithValue("@PurgeKeep", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurgeKeep", _purgeKeep);

                if (_purgeData == null)
                    cm.Parameters.AddWithValue("@PurgeData", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurgeData", _purgeData);

                cm.Parameters["@NewBatchKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewLogDC"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewLogDK"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewLogDItm"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                // Check Return Value -- Changed By Richard
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
                cm.CommandText = "MSTItmBatchLog_Delete";

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@BatchKey", criteria._batchKey);
                cm.Parameters.AddWithValue("@LogDC", criteria._logDC);
                cm.Parameters.AddWithValue("@LogDK", criteria._logDK);
                cm.Parameters.AddWithValue("@LogDItm", criteria._logDItm);

                cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
              

            }// Already close and dispose sql connection.
            
        }

        #endregion //Data Access - Delete


    }
}


