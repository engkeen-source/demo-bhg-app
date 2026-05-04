

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
    public class MSTItmSerial : Csla.BusinessBase<MSTItmSerial>, System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _serialKey = null;
        internal int? _itmKey = null;
        internal int? _batchKey = null;
        internal string _serialID = string.Empty;
        internal DateTime? _mfgDate = null;
        internal DateTime? _expiryDate = null;
        internal int? _itmStatus = null;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _error = string.Empty;

        public int? SerialKey
        {
            get
            {
                return _serialKey;
            }
            set
            {
                _serialKey = value;
                PropertyHasChanged("SerialKey");
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

        public int? ItmKey
        {
            get
            {
                return _itmKey;
            }
            set
            {
                _itmKey = value;
                PropertyHasChanged("ItmKey");
            }
        }

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

        public string SerialID
        {
            get
            {
                return _serialID;
            }
            set
            {
                _serialID = value;
                PropertyHasChanged("SerialID");
            }
        }

        public DateTime? MfgDate
        {
            get
            {
                return _mfgDate;
            }
            set
            {
                _mfgDate = value;
                PropertyHasChanged("MfgDate");
            }
        }

        public DateTime? ExpiryDate
        {
            get
            {
                return _expiryDate;
            }
            set
            {
                _expiryDate = value;
                PropertyHasChanged("ExpiryDate");
            }
        }

        public int? ItmStatus
        {
            get
            {
                return _itmStatus;
            }
            set
            {
                _itmStatus = value;
                PropertyHasChanged("ItmStatus");
            }
        }

        public DateTime? CreateDate
        {
            get
            {
                return _createDate;
            }
            set
            {
                _createDate = value;
                PropertyHasChanged("CreateDate");
            }
        }

        public int? CreateUserKey
        {
            get
            {
                return _createUserKey;
            }
            set
            {
                _createUserKey = value;
                PropertyHasChanged("CreateUserKey");
            }
        }

        public DateTime? LastModifiedDate
        {
            get
            {
                return _lastModifiedDate;
            }
            set
            {
                _lastModifiedDate = value;
                PropertyHasChanged("LastModifiedDate");
            }
        }

        public int? LastModifiedUserKey
        {
            get
            {
                return _lastModifiedUserKey;
            }
            set
            {
                _lastModifiedUserKey = value;
                PropertyHasChanged("LastModifiedUserKey");
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
            return _serialKey.ToString();
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
           // SerialID
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "SerialID");
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("SerialID", 50));
           //
           // Custom1
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom1", 255));
           //
           // Custom2
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom2", 255));
           //
           // Custom3
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom3", 255));
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

        public MSTItmSerial()
        { /* require use of factory method */ }

        public static MSTItmSerial New()
        {           
            MSTItmSerial child = new MSTItmSerial();         
            return child;
        }

        public static MSTItmSerial NewChild()
        {           
            MSTItmSerial child = new MSTItmSerial();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();           
            return child;
        }

        public static MSTItmSerial Get(SafeDataReader dr)
        {           
            MSTItmSerial child = new MSTItmSerial();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static MSTItmSerial Get(int? serialKey)
        {        
            MSTItmSerial child = new MSTItmSerial();
            child.Fetch(new Criteria((int)serialKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int _serialKey = 0;
            public int? _option = null;
           
            internal Criteria()
            {
            }

            internal Criteria(int SerialKey)
            {
                _serialKey = SerialKey;
            }

            internal Criteria(int SerialKey, int? Option)
            {
                _serialKey = SerialKey;
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
                cm.CommandText = "MSTItmSerial_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                   
                cm.Parameters.AddWithValue("@SerialKey", criteria._serialKey);
                cm.Parameters.AddWithValue("@ItmKey", 0);                  

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
                {
                    retValue = true;
                }
                else
                {
                    retValue=false;
                } 
            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _serialKey = dr.GetInt32("SerialKey");
            _itmKey = dr.GetInt32("ItmKey");
            _batchKey = dr.GetInt32("BatchKey");
            _serialID = dr.GetString("SerialID");
            _mfgDate = dr.GetDateTime("MfgDate");
            _expiryDate = dr.GetDateTime("ExpiryDate");
            _itmStatus = dr.GetInt32("ItmStatus");
            _createDate = dr.GetDateTime("CreateDate");
            _createUserKey = dr.GetInt32("CreateUserKey");
            _lastModifiedDate = dr.GetDateTime("LastModifiedDate");
            _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            ValidationRules.CheckRules();
            return true;            
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? serialKey)
        {
            bool retValue = false;           
            serialKey = null;
            
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
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmSerial_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);                   

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;


                if (_serialKey == null)
                    cm.Parameters.AddWithValue("@SerialKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SerialKey", _serialKey);

                if (_itmKey == null)
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_batchKey == null)
                    cm.Parameters.AddWithValue("@BatchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchKey", _batchKey);

                if (_serialID == null)
                    cm.Parameters.AddWithValue("@SerialID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SerialID", _serialID);

                if (_mfgDate == null)
                    cm.Parameters.AddWithValue("@MfgDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MfgDate", _mfgDate.Value);

                if (_expiryDate == null)
                    cm.Parameters.AddWithValue("@ExpiryDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ExpiryDate", _expiryDate.Value);

                if (_itmStatus == null)
                    cm.Parameters.AddWithValue("@ItmStatus", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmStatus", _itmStatus);

                if (_createDate == null)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

                if (AppInfor.currentUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                     cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);

                if (_lastModifiedDate == null)
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

                if (_lastModifiedUserKey == null)
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", _lastModifiedUserKey);

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
                cm.CommandText = "MSTItmSerial_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewSerialKey", 0);

                if (_serialKey == null)
                    cm.Parameters.AddWithValue("@SerialKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SerialKey", _serialKey);

                if (_itmKey == null)
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_batchKey == null)
                    cm.Parameters.AddWithValue("@BatchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchKey", _batchKey);

                if (_serialID == null)
                    cm.Parameters.AddWithValue("@SerialID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SerialID", _serialID);

                if (_mfgDate == null)
                    cm.Parameters.AddWithValue("@MfgDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MfgDate", _mfgDate.Value);

                if (_expiryDate == null)
                    cm.Parameters.AddWithValue("@ExpiryDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ExpiryDate", _expiryDate.Value);

                if (_itmStatus == null)
                    cm.Parameters.AddWithValue("@ItmStatus", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmStatus", _itmStatus);

                if (_createDate == null)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

                if (_createUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateUserKey", _createUserKey);

                if (_lastModifiedDate == null)
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

                if (AppInfor.currentUserKey == null)
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

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

                cm.Parameters["@NewSerialKey"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "MSTItmSerial_Delete";

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@SerialKey", criteria._serialKey);

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

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call validation method.
                    retValue = this.Validation(cn, criteria, isNew);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Validation(SqlConnection cn, Criteria criteria, bool? isNew)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmSerial_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@SerialKey", criteria._serialKey);

                cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }            
        }
        #endregion //Data Access - Validation
    }
}


