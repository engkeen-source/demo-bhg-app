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
    public class REFCurrDetCon : Csla.BusinessBase<REFCurrDetCon>,System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _currKey = 0;
        internal int? _conKey = 0;
        internal DateTime? _conCurrDate =null;
        internal decimal? _conCurrRate = 1;        
        internal decimal? _conCustomRate1 = 1;
        internal decimal? _conCustomRate2 = 1;
        internal decimal? _conCustomRate3 = 1;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _error = string.Empty;

        public int? CurrKey
        {
            get
            {
                return _currKey;
            }
        }

        public DateTime? ConCurrDate
        {
            get
            {
                return _conCurrDate;
            }
            set
            {
                if (_conCurrDate != value)
                {
                    _conCurrDate = value;
                    PropertyHasChanged("conCurrDate");
                }
            }
        }

        public decimal? ConCurrRate
        {
            get
            {              
                return _conCurrRate;
            }
            set
            {
                if (_conCurrRate != value)
                {
                    _conCurrRate = value;
                    PropertyHasChanged("conCurrRate");
                }
            }
        }

        public int? ConKey
        {
            get
            {               
                return _conKey;
            }
            set
            {
                if (_conKey != value)
                {
                    _conKey = value;
                    PropertyHasChanged("conKey");
                }
            }
        }

        public decimal? ConCustomRate1//conCustomRate1
        {
            get
            {               
                return _conCustomRate1;
            }
            set
            {
                if (_conCustomRate1 != value)
                {
                    _conCustomRate1 = value;
                    PropertyHasChanged("conCustomRate1");
                }
            }
        }

        public decimal? ConCustomRate2
        {
            get
            {                
                return _conCustomRate2;
            }
            set
            {
                if (_conCustomRate2 != value)
                {
                    _conCustomRate2 = value;
                    PropertyHasChanged("conCustomRate2");
                }
            }
        }

        public decimal? ConCustomRate3
        {
            get
            {
                return _conCustomRate3;
            }
            set
            {
                if (_conCustomRate3 != value)
                {
                    _conCustomRate3 = value;
                    PropertyHasChanged("conCustomRate3");
                }
            }
        }

        public DateTime? CreateDate
        {
            get
            {
                return _createDate;
            }
        }

        public int? CreateUserKey
        {
            get
            {
                return _createUserKey;
            }

        }

        public DateTime? LastModifiedDate
        {
            get
            {
                return _lastModifiedDate;
            }
        }

        public int? LastModifiedUserKey
        {
            get
            {
                return _lastModifiedUserKey;
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

        public string Error
        {
            get
            {
                return _error;
            }
            set
            {
                if(_error!=value)
                    _error = value;
            }
        }

        protected override object GetIdValue()
        {
            return _currKey.ToString() + _conCurrDate.ToString();
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
            //// conCurrDate
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "conCurrDateString");
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

        public REFCurrDetCon()
        { /* require use of factory method */ }

        public static REFCurrDetCon New()
        {           
            REFCurrDetCon child = new REFCurrDetCon();           
            return child;
        }

        internal static REFCurrDetCon NewChild()
        {           
            REFCurrDetCon child = new REFCurrDetCon();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();            
            return child;
        }

        internal static REFCurrDetCon Get(SafeDataReader dr)
        {            
            REFCurrDetCon child = new REFCurrDetCon();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static REFCurrDetCon Get(int? currKey, DateTime? conCurrDate)
        {           
            REFCurrDetCon child = new REFCurrDetCon();
            child.Fetch(new Criteria(currKey, conCurrDate, 2));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _currKey = null;
            public int? _conKey = null;
            public DateTime? _conCurrDate = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? CurrKey)
            {
                _currKey = CurrKey;
            }

            internal Criteria(int? CurrKey, DateTime? conCurrDate ,int conKey )
            {
                _currKey = CurrKey;
                _conKey = conKey;
                _conCurrDate = conCurrDate;
            }

            internal Criteria(int? CurrKey, DateTime? conCurrDate, int conKey, int? Option)
            {
                _currKey = CurrKey;
                _conKey = conKey;
                _conCurrDate = conCurrDate;
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
                cm.CommandText = "REFCurrDetCon_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);             
                cm.Parameters.AddWithValue("@CurrKey", criteria._currKey);
                cm.Parameters.AddWithValue("@conKey", criteria._conKey);
                cm.Parameters.AddWithValue("@conCurrDate", criteria._conCurrDate);                   

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
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
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.
            
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _currKey = dr.GetInt32("CurrKey");
            _conCurrDate = dr.GetDateTime("conCurrDate");
            _conCurrRate = dr.GetDecimal("conCurrRate");
            _conKey = dr.GetInt32("conKey");
            _conCustomRate1 = dr.GetDecimal("conCustomRate1");
            _conCustomRate2 = dr.GetDecimal("conCustomRate2");
            _conCustomRate3 = dr.GetDecimal("conCustomRate3");
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
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFCurrDetCon_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);                    

                if (_currKey == null)
                    cm.Parameters.AddWithValue("@CurrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CurrKey", _currKey);

                if (_conCurrDate == null)
                    cm.Parameters.AddWithValue("@conCurrDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@conCurrDate", _conCurrDate.Value);

                if (_conCurrRate == null)
                    cm.Parameters.AddWithValue("@conCurrRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@conCurrRate", _conCurrRate);

                if (_conKey == null)
                    cm.Parameters.AddWithValue("@conKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@conKey", _conKey);

                if (_conCustomRate1 == null)
                    cm.Parameters.AddWithValue("@conCustomRate1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@conCustomRate1", _conCustomRate1);

                if (_conCustomRate2 == null)
                    cm.Parameters.AddWithValue("@conCustomRate2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@conCustomRate2", _conCustomRate2);

                if (_conCustomRate3 == null)
                    cm.Parameters.AddWithValue("@conCustomRate3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@conCustomRate3", _conCustomRate3);

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
				cm.CommandText = "REFCurrDetCon_AddUpdate";

				cm.Parameters.AddWithValue("@Option", 1);					
			
				if (_currKey == null)
					cm.Parameters.AddWithValue("@CurrKey" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@CurrKey" , _currKey);

				if (_conCurrDate == null)
					cm.Parameters.AddWithValue("@conCurrDate" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@conCurrDate" , _conCurrDate.Value);

				if (_conCurrRate == null)
					cm.Parameters.AddWithValue("@conCurrRate" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@conCurrRate" , _conCurrRate);

				if (_conKey == null)
					cm.Parameters.AddWithValue("@conKey" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@conKey" , _conKey);

				if (_conCustomRate1 == null)
					cm.Parameters.AddWithValue("@conCustomRate1" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@conCustomRate1" , _conCustomRate1);

				if (_conCustomRate2 == null)
					cm.Parameters.AddWithValue("@conCustomRate2" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@conCustomRate2" , _conCustomRate2);

				if (_conCustomRate3 == null)
					cm.Parameters.AddWithValue("@conCustomRate3" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@conCustomRate3" , _conCustomRate3);

				if (_createDate == null)
					cm.Parameters.AddWithValue("@CreateDate" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@CreateDate" , _createDate.Value);

				 if (_createUserKey == null)
					cm.Parameters.AddWithValue("@CreateUserKey" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@CreateUserKey" , _createUserKey);

				if (_lastModifiedDate == null)
					cm.Parameters.AddWithValue("@LastModifiedDate" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@LastModifiedDate" , _lastModifiedDate.Value);

				if (AppInfor.currentUserKey == null)
					cm.Parameters.AddWithValue("@LastModifiedUserKey" , DBNull.Value); 
				else
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

				if (_custom1 == null)
					cm.Parameters.AddWithValue("@Custom1" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@Custom1" , _custom1);

				if (_custom2 == null)
					cm.Parameters.AddWithValue("@Custom2" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@Custom2" , _custom2);

				if (_custom3 == null)
					cm.Parameters.AddWithValue("@Custom3" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@Custom3" , _custom3);
				

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
                cm.CommandText = "REFCurrDetCon_Delete";
              
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

        internal bool Validation(SqlConnection cn, Criteria criteria,bool isNew)
        {            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFCurrDetCon_Validation";
                
                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@CurrKey", criteria._currKey);
                cm.Parameters.AddWithValue("@conCurrDate", criteria._conCurrDate);                  

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
    }
}
