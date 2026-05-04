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
    public class REFCurrDetItm : Csla.BusinessBase<REFCurrDetItm>,System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _currKey = 0;
        internal DateTime? _currDate = DateTime.Today.Date;
        internal decimal? _currRate = 1;
        internal decimal? _countryRate = 1;
        internal decimal? _customRate1 = 1;
        internal decimal? _customRate2 = 1;
        internal decimal? _customRate3 = 1;
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

        public DateTime? CurrDate
        {
            get
            {
                return _currDate;
            }
            set
            {
                if (_currDate != value)
                {
                    _currDate = value;
                    PropertyHasChanged("CurrDate");
                }
            }
        }

        public decimal? CurrRate
        {
            get
            {              
                return _currRate;
            }
            set
            {
                if (_currRate != value)
                {
                    _currRate = value;
                    PropertyHasChanged("CurrRate");
                }
            }
        }

        public decimal? CountryRate
        {
            get
            {               
                return _countryRate;
            }
            set
            {
                if (_countryRate != value)
                {
                    _countryRate = value;
                    PropertyHasChanged("CountryRate");
                }
            }
        }

        public decimal? CustomRate1
        {
            get
            {               
                return _customRate1;
            }
            set
            {
                if (_customRate1 != value)
                {
                    _customRate1 = value;
                    PropertyHasChanged("CustomRate1");
                }
            }
        }

        public decimal? CustomRate2
        {
            get
            {                
                return _customRate2;
            }
            set
            {
                if (_customRate2 != value)
                {
                    _customRate2 = value;
                    PropertyHasChanged("CustomRate2");
                }
            }
        }

        public decimal? CustomRate3
        {
            get
            {
                return _customRate3;
            }
            set
            {
                if (_customRate3 != value)
                {
                    _customRate3 = value;
                    PropertyHasChanged("CustomRate3");
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
            return _currKey.ToString() + _currDate.ToString();
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
            //// CurrDate
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "CurrDateString");
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

        public REFCurrDetItm()
        { /* require use of factory method */ }

        public static REFCurrDetItm New(out string msgID)
        {
            msgID = MsgID.Common.NewFail;
            REFCurrDetItm child = new REFCurrDetItm();
            msgID = string.Empty;
            return child;
        }

        internal static REFCurrDetItm NewChild(out string msgID)
        {
            msgID = MsgID.Common.NewFail;
            REFCurrDetItm child = new REFCurrDetItm();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            msgID = string.Empty;
            return child;
        }

        internal static REFCurrDetItm Get(SafeDataReader dr, out string msgID)
        {
            msgID = MsgID.Common.GetFail;
            REFCurrDetItm child = new REFCurrDetItm();
            child.MarkAsChild();
            child.Fetch(dr, out msgID);
            return child;
        }

        internal static REFCurrDetItm Get(int? currKey, DateTime? currDate, out string msgID)
        {
            msgID = MsgID.Common.GetFail;
            REFCurrDetItm child = new REFCurrDetItm();
            child.Fetch(new Criteria(currKey, currDate, 2), out msgID);
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _currKey = null;
            public DateTime? _currDate = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? CurrKey)
            {
                _currKey = CurrKey;
            }

            internal Criteria(int? CurrKey, DateTime? CurrDate)
            {
                _currKey = CurrKey;
                _currDate = CurrDate;
            }

            internal Criteria(int? CurrKey, DateTime? CurrDate, int? Option)
            {
                _currKey = CurrKey;
                _currDate = CurrDate;
                _option = Option;
            }
        }

        #endregion //Criteria

        #region Data Access - Fetch

        internal bool Fetch(Criteria criteria, out string msgID)
        {
            bool retValue = false;
            msgID = MsgID.Common.GetFail;
            try
            {
          
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Fetch(cn, criteria, out msgID);
                }              
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }

        internal bool Fetch(SqlConnection cn, Criteria criteria, out string msgID)
        {
            bool retValue = false;
            msgID = MsgID.Common.GetFail;
            try
            {
                // Using existing sql connection.
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "REFCurrDetItm_Get";

                    cm.Parameters.AddWithValue("@Option", criteria._option);
                    cm.Parameters.AddWithValue("@MsgID", msgID);
                    cm.Parameters.AddWithValue("@CurrKey", criteria._currKey);
                    cm.Parameters.AddWithValue("@CurrDate", criteria._currDate);

                    cm.Parameters["@MsgID"].Direction = ParameterDirection.InputOutput;

                    // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    // Using data reader as record set.
                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        // If data reader can read, continue...
                        while (dr.Read())
                        {
                            retValue = this.Fetch(dr, out msgID);
                        }
                    }	// Already close and dispose data reader.

                    if (cm.Parameters["@MsgID"].Value == null)
                        msgID = string.Empty;
                    else
                        msgID = cm.Parameters["@MsgID"].Value.ToString();

                    // Check Return Value -- Changed By Richard
                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;                                        

                }// Already close and dispose sql connection.
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }

        internal bool Fetch(SafeDataReader dr, out string msgID)
        {
            bool retValue = false;
            msgID = MsgID.Common.GetFail;
            try
            {
                _currKey = dr.GetInt32("CurrKey");
                _currDate = dr.GetDateTime("CurrDate");
                _currRate = dr.GetDecimal("CurrRate");
                _countryRate = dr.GetDecimal("CountryRate");
                _customRate1 = dr.GetDecimal("CustomRate1");
                _customRate2 = dr.GetDecimal("CustomRate2");
                _customRate3 = dr.GetDecimal("CustomRate3");
                _createDate = dr.GetDateTime("CreateDate");
                _createUserKey = dr.GetInt32("CreateUserKey");
                _lastModifiedDate = dr.GetDateTime("LastModifiedDate");
                _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
                _custom1 = dr.GetString("Custom1");
                _custom2 = dr.GetString("Custom2");
                _custom3 = dr.GetString("Custom3");
                ValidationRules.CheckRules();
                retValue = true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return retValue;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out string msgID, out int? currKey)
        {
            bool retValue = false;
            msgID = MsgID.Common.AddFail;
            currKey = null;
            try
            {
                // Create Transaction Scope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        // Call insert method.
                        retValue = this.Insert(cn, out msgID);
                    }// End of SqlConnection

                    // No errors - commit transaction
                      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                }// End of TransactionScope
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }

        internal bool Insert(SqlConnection cn, out string msgID)
        {
            bool retValue = false;
            msgID = MsgID.Common.AddFail;
            try
            {
                // Using existing sql connection.
                using (SqlCommand cm = cn.CreateCommand())
                {
                    // Get current user key
                    _createUserKey = AppInfor.currentUserKey;
                    _lastModifiedUserKey = AppInfor.currentUserKey;

                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "REFCurrDetItm_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@MsgID", msgID);

                    if (_currKey == null)
                        cm.Parameters.AddWithValue("@CurrKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CurrKey", _currKey);

                    if (_currDate == null)
                        cm.Parameters.AddWithValue("@CurrDate", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CurrDate", _currDate.Value);

                    if (_currRate == null)
                        cm.Parameters.AddWithValue("@CurrRate", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CurrRate", _currRate);

                    if (_countryRate == null)
                        cm.Parameters.AddWithValue("@CountryRate", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CountryRate", _countryRate);

                    if (_customRate1 == null)
                        cm.Parameters.AddWithValue("@CustomRate1", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CustomRate1", _customRate1);

                    if (_customRate2 == null)
                        cm.Parameters.AddWithValue("@CustomRate2", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CustomRate2", _customRate2);

                    if (_customRate3 == null)
                        cm.Parameters.AddWithValue("@CustomRate3", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CustomRate3", _customRate3);

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

                    cm.Parameters["@MsgID"].Direction = ParameterDirection.InputOutput;

                    // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    cm.ExecuteNonQuery();

                    if (cm.Parameters["@MsgID"].Value == null)
                        msgID = string.Empty;
                    else
                        msgID = cm.Parameters["@MsgID"].Value.ToString();

                    // Check Return Value -- Changed By Richard
                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;                                        

                }// Already close and dispose sql connection.
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }

        #endregion //Data Access - Insert

        #region Data Access - Update

        internal bool Update(out string msgID)
        {
            bool retValue = false;
            msgID = MsgID.Common.UpdateFail;
            try
            {
                // Create Transaction Scope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        // Call update method.
                        retValue = this.Update(cn, out msgID);
                    }// End of SqlConnection

                    // No errors - commit transaction
                      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                }// End of TransactionScope
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }

        internal bool Update(SqlConnection cn, out string msgID)
		{
			bool retValue = false;
			msgID = MsgID.Common.UpdateFail;
			try
			{
				// Using existing sql connection.
				using (SqlCommand cm = cn.CreateCommand())
				{
                    // Get current user key
                    _lastModifiedUserKey = AppInfor.currentUserKey;

					cm.CommandType = CommandType.StoredProcedure;
					cm.CommandText = "REFCurrDetItm_AddUpdate";

					cm.Parameters.AddWithValue("@Option", 1);
					cm.Parameters.AddWithValue("@MsgID", msgID);
				
					if (_currKey == null)
						cm.Parameters.AddWithValue("@CurrKey" , DBNull.Value); 
					else 
						cm.Parameters.AddWithValue("@CurrKey" , _currKey);

					if (_currDate == null)
						cm.Parameters.AddWithValue("@CurrDate" , DBNull.Value); 
					else 
						cm.Parameters.AddWithValue("@CurrDate" , _currDate.Value);

					if (_currRate == null)
						cm.Parameters.AddWithValue("@CurrRate" , DBNull.Value); 
					else 
						cm.Parameters.AddWithValue("@CurrRate" , _currRate);

					if (_countryRate == null)
						cm.Parameters.AddWithValue("@CountryRate" , DBNull.Value); 
					else 
						cm.Parameters.AddWithValue("@CountryRate" , _countryRate);

					if (_customRate1 == null)
						cm.Parameters.AddWithValue("@CustomRate1" , DBNull.Value); 
					else 
						cm.Parameters.AddWithValue("@CustomRate1" , _customRate1);

					if (_customRate2 == null)
						cm.Parameters.AddWithValue("@CustomRate2" , DBNull.Value); 
					else 
						cm.Parameters.AddWithValue("@CustomRate2" , _customRate2);

					if (_customRate3 == null)
						cm.Parameters.AddWithValue("@CustomRate3" , DBNull.Value); 
					else 
						cm.Parameters.AddWithValue("@CustomRate3" , _customRate3);

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

					cm.Parameters["@MsgID"].Direction = ParameterDirection.InputOutput;

                    // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

					cm.ExecuteNonQuery();

					if(cm.Parameters["@MsgID"].Value == null)
						msgID = string.Empty;
					else
						msgID = cm.Parameters["@MsgID"].Value.ToString();

                    // Check Return Value -- Changed By Richard
                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;                                        

				}// Already close and dispose sql connection.
			}
			catch (Exception ex)
			{
                throw ex;
			}
			return retValue;
		}
        #endregion //Data Access - Update

        #region Data Access - Delete

        internal bool Delete(Criteria criteria, out string msgID)
        {
            bool retValue = false;
            msgID = MsgID.Common.DeleteFail;
            try
            {
                // Create Transaction Scope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        // Call delete method.
                        retValue = this.Delete(cn, criteria, out msgID);
                    }// End of SqlConnection

                    // No errors - commit transaction
                      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                }// End of TransactionScope
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria, out string msgID)
        {
            bool retValue = false;
            msgID = MsgID.Common.DeleteFail;
            try
            {
                // Using existing sql connection.
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "REFCurrDetItm_Delete";

                    cm.Parameters.AddWithValue("@MsgID", msgID);
                    cm.Parameters.AddWithValue("@CurrKey", criteria._currKey);

                    cm.Parameters["@MsgID"].Direction = ParameterDirection.InputOutput;

                    // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    cm.ExecuteNonQuery();

                    if (cm.Parameters["@MsgID"].Value == null)
                        msgID = string.Empty;
                    else
                        msgID = cm.Parameters["@MsgID"].Value.ToString();

                    // Check Return Value -- Changed By Richard
                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;                                        

                }// Already close and dispose sql connection.
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }

        #endregion //Data Access - Delete

        #region Data Access - Validation

        internal bool Validation(Criteria criteria, out string msgID, bool isNew)
        {
            bool retValue = false;
            msgID = "";
            try
            {
                // Create Transaction Scope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        // Call validation method.
                        retValue = this.Validation(cn, criteria, out msgID, isNew);
                    }// End of SqlConnection

                    // No errors - commit transaction
                      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                }// End of TransactionScope
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }

        internal bool Validation(SqlConnection cn, Criteria criteria, out string msgID, bool isNew)
        {
            bool retValue = false;
            msgID = string.Empty;
            try
            {
                // Using existing sql connection.
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "REFCurrDetItm_Validation";

                    cm.Parameters.AddWithValue("@MsgID", msgID);
                    cm.Parameters.AddWithValue("@IsNew", isNew);
                    cm.Parameters.AddWithValue("@CurrKey", criteria._currKey);
                    cm.Parameters.AddWithValue("@CurrDate", criteria._currDate);

                    cm.Parameters["@MsgID"].Direction = ParameterDirection.InputOutput;

                    // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    cm.ExecuteNonQuery();

                    if (cm.Parameters["@MsgID"].Value == null)
                        msgID = string.Empty;
                    else
                        msgID = cm.Parameters["@MsgID"].Value.ToString();

                    // Check Return Value -- Changed By Richard
                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }
        #endregion //Data Access - Validation
    }
}
