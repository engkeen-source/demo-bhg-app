

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
    public class MSTItmBatch : Csla.BusinessBase<MSTItmBatch>,System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _batchKey = null;
        internal int? _batchItmKey = null;
        internal string _batchItmID = null;
        internal string _batchID = string.Empty;
        internal DateTime? _batchExpDate = null;
        internal DateTime? _batchMfgDate = null;
        internal decimal? _batchQty = null;
        internal decimal? _batchQtyBal = null;
        internal decimal? _batchCost = null;
        internal bool? _batchStatus = null;
        internal int? _logDC = null;
        internal int? _logDK = null;
        internal int? _logDItm = null;
        internal DateTime? _logDocDate = null;
        internal int? _purgeKeep = null;
        internal bool? _purgeData = null;
        internal string _error = string.Empty;

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

        public int? BatchItmKey
        {
            get
            {
                return _batchItmKey;
            }
            set
            {
                _batchItmKey = value;
                PropertyHasChanged("BatchItmKey");
            }
        }
        public string BatchItmID
        {
            get
            {
                return _batchItmID;
            }
            set
            {
                _batchItmID = value;
                PropertyHasChanged("BatchItmID");
            }
        }

        public string Error
        {
            get
            {
                return _error;
            }
            set
            {
                if (_error != value)
                    _error = value;
            }
        }

        public string BatchID
        {
            get
            {
                return _batchID;
            }
            set
            {
                _batchID = value;
                PropertyHasChanged("BatchID");
            }
        }

        public DateTime? BatchExpDate
        {
            get
            {
                return _batchExpDate;
            }
            set
            {
                _batchExpDate = value;
                PropertyHasChanged("BatchExpDate");
            }
        }

        public DateTime? BatchMfgDate
        {
            get
            {
                return _batchMfgDate;
            }
            set
            {
                _batchMfgDate = value;
                PropertyHasChanged("BatchMfgDate");
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

        public decimal? BatchQtyBal
        {
            get
            {
                return _batchQtyBal;
            }
            set
            {
                _batchQtyBal = value;
                PropertyHasChanged("BatchQtyBal");
            }
        }
        public decimal? BatchCost 
        {
            get
            {
                return _batchCost;
            }
            set
            {
                _batchCost = value;
                PropertyHasChanged("BatchCost");
            }
        }
        public bool? BatchStatus
        {
            get
            {
                return _batchStatus;
            }
            set
            {
                _batchStatus = value;
                PropertyHasChanged("BatchStatus");
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
            return _batchKey.ToString();
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
           //
           // BatchID
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "BatchID");
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BatchID", 50));
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

        public MSTItmBatch()
        { /* require use of factory method */ }

        public static MSTItmBatch New()
        {           
            MSTItmBatch child = new MSTItmBatch();           
            return child;
        }

        public static MSTItmBatch NewChild()
        {           
            MSTItmBatch child = new MSTItmBatch();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();           
            return child;
        }

        public static MSTItmBatch Get(SafeDataReader dr)
        {           
            MSTItmBatch child = new MSTItmBatch();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static MSTItmBatch Get(int? batchKey)
        {            
            MSTItmBatch child = new MSTItmBatch();
            child.Fetch(new Criteria(batchKey, 1));
            return child;
        }
        public static MSTItmBatch Get(int? batchKey,int? itmKey)
        {
            MSTItmBatch child = new MSTItmBatch();
            child.Fetch(new Criteria(batchKey,itmKey , 2));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _batchKey = null;
            public int? _itmKey = null;
            public string _batchID = string.Empty;
            public int? _option = null;
            internal Criteria()
            {
            }

            internal Criteria(int? BatchKey)
            {
                _batchKey = BatchKey;
            }

            internal Criteria(int? BatchKey, int? Option)
            {
                _batchKey = BatchKey;
                _option = Option;
            }
            internal Criteria(int? BatchKey,int? itmKey, int? Option)
            {
                _batchKey = BatchKey;
                _itmKey = itmKey;
                _option = Option;
            }
            internal Criteria(int? BatchKey, int? itmKey,string batchID, int? Option)
            {
                _batchKey = BatchKey;
                _itmKey = itmKey;
                _batchID = batchID;
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
                cm.CommandText = "MSTItmBatch_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                   
                cm.Parameters.AddWithValue("@BatchKey", criteria._batchKey);
                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                // Using data reader as record set.
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    // If data reader can read, continue...
                    if (dr.Read())
                    {
                        retValue = this.Fetch(dr);
                    }
                    else
                        this.Clear();
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
            _batchItmKey = dr.GetInt32("BatchItmKey");
            _batchID = dr.GetString("BatchID");
            _batchExpDate = dr.GetDateTime("BatchExpDate");
            _batchMfgDate = dr.GetDateTime("BatchMfgDate");
            _batchQty = dr.GetDecimal("BatchQty");
            _batchQtyBal = dr.GetDecimal("BatchQtyBal");
            _batchCost = dr.GetDecimal("BatchCost");
            _batchStatus = dr.GetBoolean("BatchStatus");
            _logDC = dr.GetInt32("LogDC");
            _logDK = dr.GetInt32("LogDK");
            _logDItm = dr.GetInt32("LogDItm");
            _logDocDate = dr.GetDateTime("LogDocDate");
            _purgeKeep = dr.GetInt32("PurgeKeep");
            _purgeData = dr.GetBoolean("PurgeData");
            ValidationRules.CheckRules();
            return true;            
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? batchKey)
        {
            bool retValue = false;            
            batchKey = null;            
            // Create Transaction Scope
            //using (TransactionScope scope = new TransactionScope())
            //{
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn,out batchKey);
                }// End of SqlConnection

            //    // No errors - commit transaction
            //      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            //}// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? batchKey)
        {
            batchKey = 0;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmBatch_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewBatchKey", batchKey);

                if (_batchKey == null)
                    cm.Parameters.AddWithValue("@BatchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchKey", _batchKey);

                if (_batchItmKey == null)
                    cm.Parameters.AddWithValue("@BatchItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchItmKey", _batchItmKey);

                if (_batchID == null)
                    cm.Parameters.AddWithValue("@BatchID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchID", _batchID);

                if (_batchExpDate == null)
                    cm.Parameters.AddWithValue("@BatchExpDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchExpDate", _batchExpDate.Value);

                if (_batchMfgDate == null)
                    cm.Parameters.AddWithValue("@BatchMfgDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchMfgDate", _batchMfgDate.Value);

                if (_batchQty == null)
                    cm.Parameters.AddWithValue("@BatchQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchQty", _batchQty);

                if (_batchQtyBal == null)
                    cm.Parameters.AddWithValue("@BatchQtyBal", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchQtyBal", _batchQtyBal);

                if (_batchCost == null)
                    cm.Parameters.AddWithValue("@BatchCost", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchCost", _batchCost);

                if (_batchStatus == null)
                    cm.Parameters.AddWithValue("@BatchStatus", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchStatus", _batchStatus);

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

                if (_logDocDate == null)
                    cm.Parameters.AddWithValue("@LogDocDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDocDate", _logDocDate.Value);

                if (_purgeKeep == null)
                    cm.Parameters.AddWithValue("@PurgeKeep", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurgeKeep", _purgeKeep);

                if (_purgeData == null)
                    cm.Parameters.AddWithValue("@PurgeData", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurgeData", _purgeData);

                cm.Parameters["@NewBatchKey"].Direction = ParameterDirection.Output;

                int result= cm.ExecuteNonQuery();
              

                batchKey = int.Parse(cm.Parameters["@NewBatchKey"].Value.ToString());
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
                cm.CommandText = "MSTItmBatch_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewBatchKey", 0);

                if (_batchKey == null)
                    cm.Parameters.AddWithValue("@BatchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchKey", _batchKey);

                if (_batchItmKey == null)
                    cm.Parameters.AddWithValue("@BatchItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchItmKey", _batchItmKey);

                if (_batchID == null)
                    cm.Parameters.AddWithValue("@BatchID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchID", _batchID);

                if (_batchExpDate == null)
                    cm.Parameters.AddWithValue("@BatchExpDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchExpDate", _batchExpDate.Value);

                if (_batchMfgDate == null)
                    cm.Parameters.AddWithValue("@BatchMfgDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchMfgDate", _batchMfgDate.Value);

                if (_batchQty == null)
                    cm.Parameters.AddWithValue("@BatchQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchQty", _batchQty);

                if (_batchQtyBal == null)
                    cm.Parameters.AddWithValue("@BatchQtyBal", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchQtyBal", _batchQtyBal);

                if (_batchCost == null)
                    cm.Parameters.AddWithValue("@BatchCost", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchCost", _batchCost);

                if (_batchStatus == null)
                    cm.Parameters.AddWithValue("@BatchStatus", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchStatus", _batchStatus);

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

                if (_logDocDate == null)
                    cm.Parameters.AddWithValue("@LogDocDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogDocDate", _logDocDate.Value);

                if (_purgeKeep == null)
                    cm.Parameters.AddWithValue("@PurgeKeep", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurgeKeep", _purgeKeep);

                if (_purgeData == null)
                    cm.Parameters.AddWithValue("@PurgeData", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurgeData", _purgeData);

                cm.Parameters["@NewBatchKey"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "MSTItmBatch_Delete";

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@BatchKey", criteria._batchKey);

                cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.
            
        }

        #endregion //Data Access - Delete

        #region Data Access - Validation

        internal bool Validation(Criteria criteria, bool? isNew)
        {
            bool retValue = false;
            
            //using (TransactionScope scope = new TransactionScope())
            //{
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();

                    retValue = this.Validation(cn, criteria, isNew);
                }

             //     if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            //}
            
            return retValue;
        }

        internal bool Validation(SqlConnection cn, Criteria criteria, bool? isNew)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmBatch_Validation";

                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@BatchKey", criteria._batchKey);
                cm.Parameters.AddWithValue("@BatchID", criteria._batchID);
                cm.Parameters.AddWithValue("@BatchItmKey", criteria._itmKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }
            
        }
        #endregion

        private void Clear()
        {
            _batchKey = null;
            _batchItmKey = null;
            _batchItmID = null;
            _batchID = string.Empty;
            _batchExpDate = null;
            _batchMfgDate = null;
            _batchQty = null;
            _batchQtyBal = null;
            _batchCost = null;
            _batchStatus = null;
            _logDC = null;
            _logDK = null;
            _logDItm = null;
            _logDocDate = null;
            _purgeKeep = null;
            _purgeData = null;

        }
    
    }
}


