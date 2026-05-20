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
    public class REFCurr : Csla.BusinessBase<REFCurr>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _currKey = 0;
        internal string _currID = string.Empty;
        internal string _currNm = string.Empty;
        internal string _txHdom = string.Empty;
        internal string _txLdom = string.Empty;
        internal string _symHdom = string.Empty;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public int? CurrKey
        {
            get
            {
                return _currKey;
            }
            set
            {
                _currKey=value;
            }
        }

        public string CurrID
        {
            get
            {
                return _currID;
            }
            set
            {
                _currID = value;
                PropertyHasChanged("CurrID");
            }
        }

        public string CurrNm
        {
            get
            {
                return _currNm;
            }
            set
            {
                _currNm = value;
                PropertyHasChanged("CurrNm");
            }
        }

        public string TxHdom
        {
            get
            {
                return _txHdom;
            }
            set
            {
                _txHdom = value;
                PropertyHasChanged("TxHdom");
            }
        }

        public string TxLdom
        {
            get
            {
                return _txLdom;
            }
            set
            {
                _txLdom = value;
                PropertyHasChanged("TxLdom");
            }
        }

        public string SymHdom
        {
            get
            {
                return _symHdom;
            }
            set
            {
                _symHdom = value;
                PropertyHasChanged("SymHdom");
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
            return _currKey.ToString();
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
            //// CurrID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "CurrID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CurrID", 50));
            ////
            //// CurrNm
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "CurrNm");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CurrNm", 50));
            ////
            //// TxHdom
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "TxHdom");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TxHdom", 50));
            ////
            //// TxLdom
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TxLdom", 50));
            ////
            //// SymHdom
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "SymHdom");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("SymHdom", 50));
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

        internal REFCurr()
        { /* require use of factory method */ }

        internal static REFCurr New()
        {           
            REFCurr child = new REFCurr();       
            return child;
        }

        internal static REFCurr NewChild()
        {            
            REFCurr child = new REFCurr();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();           
            return child;
        }

        internal static REFCurr Get(SafeDataReader dr)
        {
            REFCurr child = new REFCurr();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static REFCurr Get(int? currKey)
        {           
            REFCurr child = new REFCurr();
            child.Fetch(new Criteria(currKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _currKey = null;
            public int? _option = null;
            public string _currID = string.Empty;

            internal Criteria()
            {
            }

            internal Criteria(int? CurrKey)
            {
                _currKey = CurrKey;
            }

            internal Criteria(int? CurrKey, int? Option)
            {
                _currKey = CurrKey;
                _option = Option;
            }

            internal Criteria(int? CurrKey, string CurrID)
            {
                _currKey = CurrKey;
                _currID = CurrID;
            }
            //Add Thida
            internal Criteria(int? CurrKey, string CurrID, int? Option)
            {
                _currKey = CurrKey;
                _currID = CurrID;
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
                cm.CommandText = "REFCurr_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                  
                cm.Parameters.AddWithValue("@CurrKey", criteria._currKey);                  

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
                    return true;
                else
                    return false;                                                   

            }// Already close and dispose sql connection.            
        }

        internal bool Fetch(SafeDataReader dr)
        {            
            _currKey = dr.GetInt32("CurrKey");
            _currID = dr.GetString("CurrID");
            _currNm = dr.GetString("CurrNm");
            _txHdom = dr.GetString("TxHdom");
            _txLdom = dr.GetString("TxLdom");
            _symHdom = dr.GetString("SymHdom");
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

        internal bool Insert(out int? currKey)
        {
            bool retValue = false;
            currKey = null;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn,out currKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? currKey)
        {
            currKey = 0;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFCurr_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);                    

                cm.Parameters.AddWithValue("@NewCurrKey", currKey);

                if (_currKey == null)
                    cm.Parameters.AddWithValue("@CurrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CurrKey", _currKey);

                if (_currID == null)
                    cm.Parameters.AddWithValue("@CurrID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CurrID", _currID);

                if (_currNm == null)
                    cm.Parameters.AddWithValue("@CurrNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CurrNm", _currNm);

                if (_txHdom == null)
                    cm.Parameters.AddWithValue("@TxHdom", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TxHdom", _txHdom);

                if (_txLdom == null)
                    cm.Parameters.AddWithValue("@TxLdom", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TxLdom", _txLdom);

                if (_symHdom == null)
                    cm.Parameters.AddWithValue("@SymHdom", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SymHdom", _symHdom);

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
              
                cm.Parameters["@NewCurrKey"].Direction = ParameterDirection.Output;

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

              
                currKey = (int)cm.Parameters["@NewCurrKey"].Value;

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
                cm.CommandText = "REFCurr_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);                  
                cm.Parameters.AddWithValue("@NewCurrKey", 0);

                if (_currKey == null)
                    cm.Parameters.AddWithValue("@CurrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CurrKey", _currKey);

                if (_currID == null)
                    cm.Parameters.AddWithValue("@CurrID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CurrID", _currID);

                if (_currNm == null)
                    cm.Parameters.AddWithValue("@CurrNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CurrNm", _currNm);

                if (_txHdom == null)
                    cm.Parameters.AddWithValue("@TxHdom", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TxHdom", _txHdom);

                if (_txLdom == null)
                    cm.Parameters.AddWithValue("@TxLdom", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TxLdom", _txLdom);

                if (_symHdom == null)
                    cm.Parameters.AddWithValue("@SymHdom", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SymHdom", _symHdom);

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
               
                cm.Parameters["@NewCurrKey"].Direction = ParameterDirection.Output;

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
            bool retValue = false;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFCurr_Delete";
                
                cm.Parameters.AddWithValue("@CurrKey", criteria._currKey);                   

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
            
            return retValue;
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

        internal bool Validation(SqlConnection cn, Criteria criteria,  bool isNew)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFCurr_Validation";
               
                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@CurrKey", criteria._currKey);
                cm.Parameters.AddWithValue("@CurrID", criteria._currID);

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
            _currKey = 0;
            _currID = string.Empty;
            _currNm = string.Empty;
            _txHdom = string.Empty;
            _txLdom = string.Empty;
            _symHdom = string.Empty;
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
