
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
    public class MSTAccBranch : Csla.BusinessBase<MSTAccBranch>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _branchKey = 0;
        internal string _branchID = string.Empty;
        internal string _branchNm = string.Empty;
        internal bool? _inactive = false;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _custom4 = string.Empty;
        internal string _custom5 = string.Empty;

        public int? BranchKey
        {
            get
            {
                CanReadProperty("BranchKey", true);
                return _branchKey;
            }
        }

        public string BranchID
        {
            get
            {
                CanReadProperty("BranchID", true);
                return _branchID;
            }
            set
            {
                CanWriteProperty("BranchID", true);
                if (value == null) value = string.Empty;
                
                    _branchID = value;
                    PropertyHasChanged("BranchID");
                
            }
        }

        public string BranchNm
        {
            get
            {
                CanReadProperty("BranchNm", true);
                return _branchNm;
            }
            set
            {
                CanWriteProperty("BranchNm", true);
                if (value == null) value = string.Empty;
                
                    _branchNm = value;
                    PropertyHasChanged("BranchNm");
                
            }
        }

        public bool? Inactive
        {
            get
            {
                CanReadProperty("Inactive", true);
                return _inactive;
            }
            set
            {
                CanWriteProperty("Inactive", true);
                
                    _inactive = value;
                    PropertyHasChanged("Inactive");
               
            }
        }

        public DateTime? CreateDate
        {
            get
            {
                CanReadProperty("CreateDate", true);
                return _createDate;
            }
            set
            {
                _createDate = value;
            }
        }

        public int? CreateUserKey
        {
            get
            {
                CanReadProperty("CreateUserKey", true);
                return _createUserKey;
            }
            set
            {
                _createUserKey = value;
            }
        }

        public DateTime? LastModifiedDate
        {
            get
            {
                CanReadProperty("LastModifiedDate", true);
                return _lastModifiedDate;
            }
            set
            {
                _lastModifiedDate = value;
            }
        }

        public int? LastModifiedUserKey
        {
            get
            {
                CanReadProperty("LastModifiedUserKey", true);
                return _lastModifiedUserKey;
            }
            set
            {
                _lastModifiedUserKey = value;
            }
        }

        public string Custom1
        {
            get
            {
                CanReadProperty("Custom1", true);
                return _custom1;
            }
            set
            {
                CanWriteProperty("Custom1", true);
                if (value == null) value = string.Empty;
                
                    _custom1 = value;
                    PropertyHasChanged("Custom1");
               
            }
        }

        public string Custom2
        {
            get
            {
                CanReadProperty("Custom2", true);
                return _custom2;
            }
            set
            {
                CanWriteProperty("Custom2", true);
                if (value == null) value = string.Empty;
               
                    _custom2 = value;
                    PropertyHasChanged("Custom2");
                
            }
        }

        public string Custom3
        {
            get
            {
                CanReadProperty("Custom3", true);
                return _custom3;
            }
            set
            {
                CanWriteProperty("Custom3", true);
                if (value == null) value = string.Empty;
               
                    _custom3 = value;
                    PropertyHasChanged("Custom3");
                
            }
        }

        public string Custom4
        {
            get
            {
                CanReadProperty("Custom4", true);
                return _custom4;
            }
            set
            {
                CanWriteProperty("Custom4", true);
                if (value == null) value = string.Empty;
                
                    _custom4 = value;
                    PropertyHasChanged("Custom4");
                
            }
        }

        public string Custom5
        {
            get
            {
                CanReadProperty("Custom5", true);
                return _custom5;
            }
            set
            {
                CanWriteProperty("Custom5", true);
                if (value == null) value = string.Empty;
               
                    _custom5 = value;
                    PropertyHasChanged("Custom5");
                
            }
        }

        protected override object GetIdValue()
        {
            return _branchKey.ToString();
        }

        #endregion //Business Properties and Methods

        #region Validation Rules
        private void AddCustomRules()
        {
            //add custom/non-generated rules here...
        }

        private void AddCommonRules()
        {
            //
            // BranchID
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "BranchID");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BranchID", 50));
            //
            // BranchNm
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "BranchNm");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BranchNm", 255));
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
            //
            // Custom4
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom4", 255));
            //
            // Custom5
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom5", 255));
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal MSTAccBranch()
        { /* require use of factory method */ }

        internal static MSTAccBranch New()
        {            
            MSTAccBranch child = new MSTAccBranch();           
            return child;
        }

        internal static MSTAccBranch NewChild()
        {           
            MSTAccBranch child = new MSTAccBranch();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();          
            return child;
        }

        internal static MSTAccBranch Get(SafeDataReader dr)
        {
            MSTAccBranch child = new MSTAccBranch();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static MSTAccBranch Get(int? branchKey)
        {           
            MSTAccBranch child = new MSTAccBranch();
            child.Fetch(new Criteria(branchKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _branchKey = null;
            public int? _option = null;
            public string _branchID = string.Empty;

            internal Criteria()
            {
            }

            internal Criteria(int? BranchKey)
            {
                _branchKey = BranchKey;
            }

            internal Criteria(int? BranchKey, string BranchID)
            {
                _branchKey = BranchKey;
                _branchID = BranchID;
            }

            internal Criteria(int? BranchKey, int? Option)
            {
                _branchKey = BranchKey;
                _option = Option;
            }
            //Add Thida
            internal Criteria(int? BranchKey, string BranchID, int? Option)
            {
                _branchKey = BranchKey;
                _branchID = BranchID;
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
                cm.CommandText = "MSTAccBranch_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                  
                cm.Parameters.AddWithValue("@BranchKey", criteria._branchKey);                   
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
                    retValue = false;
                }            
            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _branchKey = dr.GetInt32("BranchKey");
            _branchID = dr.GetString("BranchID");
            _branchNm = dr.GetString("BranchNm");
            _inactive = dr.GetBoolean("Inactive");
            
            if (dr.GetValue("CreateDate") == null)
                _createDate = null;
            else
                _createDate = dr.GetDateTime("CreateDate");

            _createUserKey = dr.GetInt32("CreateUserKey");
            if (dr.GetValue("LastModifiedDate") == null)
                _lastModifiedDate = null;
            else
                _lastModifiedDate = dr.GetDateTime("LastModifiedDate");
            _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            _custom4 = dr.GetString("Custom4");
            _custom5 = dr.GetString("Custom5");
            ValidationRules.CheckRules();
            return true;
            
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? branchKey)
        {
            bool retValue = false;           
            branchKey = null;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn,  out branchKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? branchKey)
        {
            branchKey = 0;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTAccBranch_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);                  

                cm.Parameters.AddWithValue("@NewBranchKey", branchKey);

                if (_branchKey == null)
                    cm.Parameters.AddWithValue("@BranchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BranchKey", _branchKey);

                if (_branchID == null)
                    cm.Parameters.AddWithValue("@BranchID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BranchID", _branchID);

                if (_branchNm == null)
                    cm.Parameters.AddWithValue("@BranchNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BranchNm", _branchNm);

                if (_inactive == null)
                    cm.Parameters.AddWithValue("@Inactive", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Inactive", _inactive);

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

                if (_custom4 == null)
                    cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom4", _custom4);

                if (_custom5 == null)
                    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom5", _custom5);
             
                cm.Parameters["@NewBranchKey"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                branchKey = (int)cm.Parameters["@NewBranchKey"].Value;

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
                cm.CommandText = "MSTAccBranch_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);                    
                cm.Parameters.AddWithValue("@NewBranchKey", 0);

                if (_branchKey == null)
                    cm.Parameters.AddWithValue("@BranchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BranchKey", _branchKey);

                if (_branchID == null)
                    cm.Parameters.AddWithValue("@BranchID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BranchID", _branchID);

                if (_branchNm == null)
                    cm.Parameters.AddWithValue("@BranchNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BranchNm", _branchNm);

                if (_inactive == null)
                    cm.Parameters.AddWithValue("@Inactive", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Inactive", _inactive);

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

                if (_custom4 == null)
                    cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom4", _custom4);

                if (_custom5 == null)
                    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom5", _custom5);

              
                cm.Parameters["@NewBranchKey"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                {
                    return false;
                }            
               
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
                cm.CommandText = "MSTAccBranch_Delete";

               
                cm.Parameters.AddWithValue("@BranchKey", criteria._branchKey);                 

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
                cm.CommandText = "MSTAccBranch_Validation";

                cm.Parameters.AddWithValue("@isNew",isNew);                 
                cm.Parameters.AddWithValue("@BranchKey", criteria._branchKey);
                cm.Parameters.AddWithValue("@BranchID", criteria._branchID);                   

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                {
                    return false;
                }  
            }            
        }
        #endregion //Data Access - Validation

        private void Clear()
        {
             _branchKey = null;
             _branchID = string.Empty;
             _branchNm = string.Empty;
             _inactive = false;
             _createDate = null;
             _createUserKey = null;
             _lastModifiedDate=null;
             _lastModifiedUserKey =null;
             _custom1 = string.Empty;
             _custom2 = string.Empty;
             _custom3 = string.Empty;
             _custom4 = string.Empty;
             _custom5 = string.Empty;


        }
    
    }
}

