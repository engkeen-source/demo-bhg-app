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
    public class REFJobCostType : Csla.BusinessBase<REFJobCostType>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _jobCostTypeKey = 0;
        internal string _jobCostTypeID = string.Empty;
        internal string _jobCostTypeDes = string.Empty;
        internal int? _jobCostHeaderType = 0;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public int? JobCostTypeKey
        {
            get
            {
                return _jobCostTypeKey;
            }
        }

        public string JobCostTypeID
        {
            get
            {              
                return _jobCostTypeID;
            }
            set
            {               
                _jobCostTypeID = value;
                PropertyHasChanged("JobCostTypeID");               
            }
        }

        public string JobCostTypeDes
        {
            get
            {             
                return _jobCostTypeDes;
            }
            set
            {                
                _jobCostTypeDes = value;
                PropertyHasChanged("JobCostTypeDes");              
            }
        }

        public int? JobCostHeaderType
        {
            get
            {
                return _jobCostHeaderType;
            }
            set
            {
                _jobCostHeaderType= value;
                PropertyHasChanged("JobCostHeaderType");
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
            return _jobCostTypeKey.ToString();
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
            //// JobCostTypeID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "JobCostTypeID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("JobCostTypeID", 50));
            ////
            //// JobCostTypeDes
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "JobCostTypeDes");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("JobCostTypeDes", 255));
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

        internal REFJobCostType()
        { /* require use of factory method */ }

        internal static REFJobCostType New()
        {
           
            REFJobCostType child = new REFJobCostType();            
            return child;
        }

        internal static REFJobCostType NewChild()
        {
           
            REFJobCostType child = new REFJobCostType();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();            
            return child;
        }

        internal static REFJobCostType Get(SafeDataReader dr)
        {          
            REFJobCostType child = new REFJobCostType();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static REFJobCostType Get(int? jobCostTypeKey)
        {
            REFJobCostType child = new REFJobCostType();
            child.Fetch(new Criteria(jobCostTypeKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _jobCostTypeKey = null;
            public int? _option = null;
            public string _jobCostTypeID = string.Empty;

            internal Criteria()
            {
            }

            internal Criteria(int? JobCostTypeKey)
            {
                _jobCostTypeKey = JobCostTypeKey;
            }

            internal Criteria(int? JobCostTypeKey, int? Option)
            {
                _jobCostTypeKey = JobCostTypeKey;
                _option = Option;
            }
            //Added By Thida

            internal Criteria(int? JobCostTypeKey, string JobCostTypeID)
            {
                _jobCostTypeKey = JobCostTypeKey;
                _jobCostTypeID = JobCostTypeID;
            }
            internal Criteria(int? JobCostTypeKey, string JobCodeTypeID, int? Option)
            {
                _jobCostTypeKey = JobCostTypeKey;
                _jobCostTypeID = JobCodeTypeID;
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
                cm.CommandText = "REFJobCostType_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                  
                cm.Parameters.AddWithValue("@JobCostTypeKey", criteria._jobCostTypeKey);
               

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
                    retValue = true;
                else
                    retValue = false;
            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _jobCostTypeKey = dr.GetInt32("JobCostTypeKey");
            _jobCostTypeID = dr.GetString("JobCostTypeID");
            _jobCostTypeDes = dr.GetString("JobCostTypeDes");
            _jobCostHeaderType = dr.GetInt32("JobCostHeaderType");
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

        internal bool Insert( out int? jobCostTypeKey)
        {
            bool retValue = false;
            jobCostTypeKey = null;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out jobCostTypeKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? jobCostTypeKey)
        {
            jobCostTypeKey = 0;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFJobCostType_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);                  

                cm.Parameters.AddWithValue("@NewJobCostTypeKey", jobCostTypeKey);

                if (_jobCostTypeKey == null)
                    cm.Parameters.AddWithValue("@JobCostTypeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobCostTypeKey", _jobCostTypeKey);

                if (_jobCostTypeID == null)
                    cm.Parameters.AddWithValue("@JobCostTypeID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobCostTypeID", _jobCostTypeID);

                if (_jobCostTypeDes == null)
                    cm.Parameters.AddWithValue("@JobCostTypeDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobCostTypeDes", _jobCostTypeDes);

                if (_jobCostHeaderType == null)
                    cm.Parameters.AddWithValue("@JobCostHeaderType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobCostHeaderType", _jobCostHeaderType);

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
             
                cm.Parameters["@NewJobCostTypeKey"].Direction = ParameterDirection.Output;

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

              
                jobCostTypeKey = (int)cm.Parameters["@NewJobCostTypeKey"].Value;
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
                cm.CommandText = "REFJobCostType_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);                  
                cm.Parameters.AddWithValue("@NewJobCostTypeKey", 0);

                if (_jobCostTypeKey == null)
                    cm.Parameters.AddWithValue("@JobCostTypeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobCostTypeKey", _jobCostTypeKey);

                if (_jobCostTypeID == null)
                    cm.Parameters.AddWithValue("@JobCostTypeID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobCostTypeID", _jobCostTypeID);

                if (_jobCostTypeDes == null)
                    cm.Parameters.AddWithValue("@JobCostTypeDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobCostTypeDes", _jobCostTypeDes);

                if (_jobCostHeaderType == null)
                    cm.Parameters.AddWithValue("@JobCostHeaderType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobCostHeaderType", _jobCostHeaderType);

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

              
                cm.Parameters["@NewJobCostTypeKey"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "REFJobCostType_Delete";
                
                cm.Parameters.AddWithValue("@JobCostTypeKey", criteria._jobCostTypeKey);
              

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

        internal bool Validation(Criteria criteria,  bool isNew)
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
                cm.CommandText = "REFJobCostType_Validation";              
                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@JobCostTypeKey", criteria._jobCostTypeKey);
                cm.Parameters.AddWithValue("@JobCostTypeID", criteria._jobCostTypeID);
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

        }

    }
}
