
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
    public class REFBank : Csla.BusinessBase<REFBank>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _bankKey = 0;
        internal string _bankID = string.Empty;
        internal string _bankDes = string.Empty;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public int? BankKey
        {
            get
            {
                return _bankKey;
            }            
        }

        public string BankID
        {
            get
            {
                return _bankID;
            }
            set
            {
                _bankID = value;
                PropertyHasChanged("BankID");
            }
        }

        public string BankDes
        {
            get
            {
                return _bankDes;
            }
            set
            {
                _bankDes = value;
                PropertyHasChanged("BankDes");
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
            return _bankKey.ToString();
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
            //// BankID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "BankID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BankID", 50));
            ////
            //// BankDes
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "BankDes");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BankDes", 255));
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

        internal REFBank()
        { /* require use of factory method */ }

        internal static REFBank New()
        {
            REFBank child = new REFBank();         
            return child;
        }

        internal static REFBank NewChild()
        {
            REFBank child = new REFBank();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static REFBank Get(SafeDataReader dr)
        {
            REFBank child = new REFBank();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static REFBank Get(int? bankKey)
        {
            REFBank child = new REFBank();
            child.Fetch(new Criteria(bankKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _bankKey = null;
            public string _bankID = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? BankKey)
            {
                _bankKey = BankKey;
            }

            internal Criteria(int? BankKey, int? Option)
            {
                _bankKey = BankKey;
                _option = Option;
            }

            internal Criteria(int? BankKey, string BankID)
            {
                _bankKey = BankKey;
                _bankID = BankID;
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
                retValue=this.Fetch(cn, criteria);                           
            }
        
            return retValue;
        }

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            bool retValue=false;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFBank_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@BankKey", criteria._bankKey);


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

                }// Already close and dispose data reader.



                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.                       

            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {            
            _bankKey = dr.GetInt32("BankKey");
            _bankID = dr.GetString("BankID");
            _bankDes = dr.GetString("BankDes");
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

        internal bool Insert(out int? bankKey)
        {
            bool retValue = false;
            bankKey = null;
           
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue= this.Insert(cn,out bankKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope               
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn,out int? bankKey)
        {
            bool retValue = false;
            bankKey = 0;            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFBank_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@NewBankKey", bankKey);

                if (_bankKey == null)
                    cm.Parameters.AddWithValue("@BankKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BankKey", _bankKey);

                if (_bankID == null)
                    cm.Parameters.AddWithValue("@BankID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BankID", _bankID);

                if (_bankDes == null)
                    cm.Parameters.AddWithValue("@BankDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BankDes", _bankDes);

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

                cm.Parameters["@NewBankKey"].Direction = ParameterDirection.InputOutput;

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();
                
                bankKey = (int)cm.Parameters["@NewBankKey"].Value;

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.            

            return retValue;
        }

        #endregion //Data Access - Insert

        #region Data Access - Update

        internal bool Update()
        {
            bool retValue=false;
            
                // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call update method.
                    retValue=this.Update(cn);
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
                cm.CommandText = "REFBank_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewBankKey", 0);
                if (_bankKey == null)
                    cm.Parameters.AddWithValue("@BankKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BankKey", _bankKey);

                if (_bankID == null)
                    cm.Parameters.AddWithValue("@BankID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BankID", _bankID);

                if (_bankDes == null)
                    cm.Parameters.AddWithValue("@BankDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BankDes", _bankDes);

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

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewBankKey"].Direction = ParameterDirection.InputOutput;

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
                   retValue= this.Delete(cn, criteria);
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
                cm.CommandText = "REFBank_Delete";

                cm.Parameters.AddWithValue("@BankKey", criteria._bankKey);

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

        #region Data Access - Validation

        internal bool Validation(Criteria criteria,bool isNew)
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
                    retValue= this.Validation(cn, criteria, isNew);
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
                cm.CommandText = "REFBank_Validation";

               
                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@BankKey", criteria._bankKey);
                cm.Parameters.AddWithValue("@BankID", criteria._bankID);
                cm.Parameters.AddWithValue("@RetValue", 0);

                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();


                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }
                        
        }
        #endregion //Data Access - Validation

        private void Clear()
        {
            _bankKey = 0;
            _bankID = string.Empty;
            _bankDes = string.Empty;
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
