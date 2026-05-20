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
    public class REFInterest : Csla.BusinessBase<REFInterest>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _intKey = 0;
        internal string _intID = string.Empty;
        internal string _intDes = string.Empty;
        internal decimal? _annualIntRate = 0;
        internal decimal? _minCharge = 0;
        internal bool? _intOnInt = false;
        internal int? _itmKey = null;
        internal string _itmDesDoc = string.Empty;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public int? IntKey
        {
            get
            {
                return _intKey;
            }
        }

        public string IntID
        {
            get
            {
                return _intID;
            }
            set
            {
                _intID = value;
                PropertyHasChanged("IntID");
            }
        }

        public string IntDes
        {
            get
            {
                return _intDes;
            }
            set
            {
                _intDes = value;
                PropertyHasChanged("IntDes");
            }
        }

        public decimal? AnnualIntRate
        {
            get
            {
                return _annualIntRate;
            }
            set
            {
                _annualIntRate = value;
                PropertyHasChanged("AnnualIntRate");
            }
        }

        public decimal? MinCharge
        {
            get
            {
                return _minCharge;
            }
            set
            {
                _minCharge = value;
                PropertyHasChanged("MinCharge");
            }
        }

        public bool? IntOnInt
        {
            get
            {
                return _intOnInt;
            }
            set
            {
                _intOnInt = value;
                PropertyHasChanged("IntOnInt");
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
                if (_itmKey != value && !(_itmKey==null && value==0))
                {
                    _itmKey = value;
                    PropertyHasChanged("ItmKey");
                }
            }
        }

        public string ItmDesDoc
        {
            get
            {
                return _itmDesDoc;
            }
            set
            {
                _itmDesDoc = value;
                PropertyHasChanged("ItmDesDoc");
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
            return _intKey.ToString();
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
            //// IntID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "IntID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("IntID", 50));
            ////
            //// IntDes
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "IntDes");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("IntDes", 255));
            ////
            //// ItmDesDoc
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "ItmDesDoc");
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

        internal REFInterest()
        { /* require use of factory method */ }

        internal static REFInterest New()
        {
           
            REFInterest child = new REFInterest();
            
            return child;
        }

        internal static REFInterest NewChild()
        {
            
            REFInterest child = new REFInterest();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static REFInterest Get(SafeDataReader dr)
        {
            
            REFInterest child = new REFInterest();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static REFInterest Get(int? intKey)
        {
            
            REFInterest child = new REFInterest();
            child.Fetch(new Criteria(intKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _intKey = null;
            public int? _option = null;
            public string _intID = string.Empty;

            internal Criteria()
            {
            }

            internal Criteria(int? IntKey)
            {
                _intKey = IntKey;
            }

            internal Criteria(int? IntKey, string IntID)
            {
                _intKey = IntKey;
                _intID = IntID;
            }

            internal Criteria(int? IntKey, int? Option)
            {
                _intKey = IntKey;
                _option = Option;
            }

            //internal Criteria(int? IntKey, string IntID)
            //{
            //    _intKey = IntKey;
            //    _intID = IntID;
            //}

            //Added Thida
            internal Criteria(int? IntKey, string IntID, int? Option)
            {
                _intKey = IntKey;
                _intID = IntID;
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

        internal bool Fetch(SqlConnection cn, Criteria criteria )
        {
            bool retValue = false;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFInterest_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
             
                cm.Parameters.AddWithValue("@IntKey", criteria._intKey);

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
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
            _intKey = dr.GetInt32("IntKey");
            _intID = dr.GetString("IntID");
            _intDes = dr.GetString("IntDes");
            _annualIntRate = dr.GetDecimal("AnnualIntRate");
            _minCharge = dr.GetDecimal("MinCharge");
            _intOnInt = dr.GetBoolean("IntOnInt");
            _itmKey = dr.GetInt32("ItmKey");
            _itmDesDoc = dr.GetString("ItmDesDoc");
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

        internal bool Insert( out int? intKey)
        {
            bool retValue = false;
            
            intKey = null;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out intKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? intKey)
        {
            intKey = 0;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFInterest_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);
               
                cm.Parameters.AddWithValue("@NewIntKey", intKey);

                if (_intKey == null)
                    cm.Parameters.AddWithValue("@IntKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@IntKey", _intKey);

                if (_intID == null)
                    cm.Parameters.AddWithValue("@IntID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@IntID", _intID);

                if (_intDes == null)
                    cm.Parameters.AddWithValue("@IntDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@IntDes", _intDes);

                if (_annualIntRate == null)
                    cm.Parameters.AddWithValue("@AnnualIntRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AnnualIntRate", _annualIntRate);

                if (_minCharge == null)
                    cm.Parameters.AddWithValue("@MinCharge", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MinCharge", _minCharge);

                if (_intOnInt == null)
                    cm.Parameters.AddWithValue("@IntOnInt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@IntOnInt", _intOnInt);

                if (_itmKey == null)
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_itmDesDoc == null)
                    cm.Parameters.AddWithValue("@ItmDesDoc", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmDesDoc", _itmDesDoc);

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

              
                cm.Parameters["@NewIntKey"].Direction = ParameterDirection.Output;

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                intKey = (int)cm.Parameters["@NewIntKey"].Value;
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
                // Get current user key
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFInterest_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);                  
                cm.Parameters.AddWithValue("@NewIntKey", 0);

                if (_intKey == null)
                    cm.Parameters.AddWithValue("@IntKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@IntKey", _intKey);

                if (_intID == null)
                    cm.Parameters.AddWithValue("@IntID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@IntID", _intID);

                if (_intDes == null)
                    cm.Parameters.AddWithValue("@IntDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@IntDes", _intDes);

                if (_annualIntRate == null)
                    cm.Parameters.AddWithValue("@AnnualIntRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AnnualIntRate", _annualIntRate);

                if (_minCharge == null)
                    cm.Parameters.AddWithValue("@MinCharge", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MinCharge", _minCharge);

                if (_intOnInt == null)
                    cm.Parameters.AddWithValue("@IntOnInt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@IntOnInt", _intOnInt);

                if (_itmKey == null)
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_itmDesDoc == null)
                    cm.Parameters.AddWithValue("@ItmDesDoc", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmDesDoc", _itmDesDoc);

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
               
                cm.Parameters["@NewIntKey"].Direction = ParameterDirection.Output;

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
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
                cm.CommandText = "REFInterest_Delete";
               
                cm.Parameters.AddWithValue("@IntKey", criteria._intKey);
                

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
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

        #endregion //Data Access - Delete

        #region Data Access - Validation

        internal bool Validation(Criteria criteria, bool isNew)
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

        internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFInterest_Validation";
               
                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@IntKey", criteria._intKey);
                cm.Parameters.AddWithValue("@IntID", criteria._intID);
              

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }            
        }
        #endregion //Data Access - Validation

        private void Clear()
        {
            _intKey = 0;
            _intID = string.Empty;
            _intDes = string.Empty;
            _annualIntRate = 0;
            _minCharge = 0;
            _intOnInt = false;
            _itmKey = null;
            _itmDesDoc = string.Empty;
            _createDate = null;
            _createUserKey = null;
            _lastModifiedDate = null;
            _lastModifiedUserKey = null;
            _custom1 = string.Empty;
            _custom2 = string.Empty;
            _custom3 = string.Empty;


        }
    
    }
}
