

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
    public class MSTPriceListDetValue : Csla.BusinessBase<MSTPriceListDetValue>, System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _priceKey = null;
        internal int? _itmKey = null;
        internal int? _itmType = null;
        internal string _itmDes = string.Empty;
        internal string _itmID = string.Empty;
        internal decimal? _itmQty = null;
        internal decimal? _itmPrice = 0;
        internal decimal? _customPrice = 0;
        internal DateTime? _effStartDate = null;
        internal DateTime? _effEndDate = null;
        internal decimal? _effItmQty = null;
        internal decimal? _effItmPrice = 0;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _error = string.Empty;        

        public int? PriceKey
        {
            get
            {
                return _priceKey;
            }
            set
            {
                _priceKey = value;
                PropertyHasChanged("PriceKey");
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

        public int? ItmType
        {
            get
            {
                return _itmType;
            }
            set
            {
                _itmType = value;
                PropertyHasChanged("ItmType");
            }
        }

        public string ItmDes
        {
            get
            {
                return _itmDes;
            }
            set
            {
                _itmDes = value;
                PropertyHasChanged("ItmDes");
            }
        }
        public string ItmID
        {
            get
            {
                return _itmID;
            }
            set
            {
                _itmID = value;
                PropertyHasChanged("ItmID");
            }
        }
        public decimal? ItmQty
        {
            get
            {
                return _itmQty;
            }
            set
            {
                _itmQty = value;
                PropertyHasChanged("ItmQty");
            }
        }

        public decimal? ItmPrice
        {
            get
            {
                return _itmPrice;
            }
            set
            {
                _itmPrice = value;
                PropertyHasChanged("ItmPrice");
            }
        }

        public decimal? CustomPrice
        {
            get
            {
                return _customPrice;
            }
            set
            {
                _customPrice = value;
                PropertyHasChanged("CustomPrice");
            }
        }

        public DateTime? EffStartDate
        {
            get
            {
                return _effStartDate;
            }
            set
            {
                _effStartDate = value;
                PropertyHasChanged("EffStartDate");
            }
        }

        public DateTime? EffEndDate
        {
            get
            {
                return _effEndDate;
            }
            set
            {
                _effEndDate = value;
                PropertyHasChanged("EffEndDate");
            }
        }

        public decimal? EffItmQty
        {
            get
            {
                return _effItmQty;
            }
            set
            {
                _effItmQty = value;
                PropertyHasChanged("EffItmQty");
            }
        }

        public decimal? EffItmPrice
        {
            get
            {
                return _effItmPrice;
            }
            set
            {
                _effItmPrice = value;
                PropertyHasChanged("EffItmPrice");
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

        protected override object GetIdValue()
        {
            return _priceKey.ToString() + _itmKey.ToString();
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
           // ItmDes
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "ItmDes");
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

        public MSTPriceListDetValue()
        { /* require use of factory method */ }

        internal static MSTPriceListDetValue New()
        {
            
            MSTPriceListDetValue child = new MSTPriceListDetValue();
            
            return child;
        }

        internal static MSTPriceListDetValue NewChild()
        {
            
            MSTPriceListDetValue child = new MSTPriceListDetValue();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static MSTPriceListDetValue Get(SafeDataReader dr)
        {
            
            MSTPriceListDetValue child = new MSTPriceListDetValue();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static MSTPriceListDetValue Get(int? priceKey, int? itmKey)
        {
            
            MSTPriceListDetValue child = new MSTPriceListDetValue();
            child.Fetch(new Criteria(priceKey, itmKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _priceKey = null;
            public int? _itmKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? PriceKey)
            {
                _priceKey = PriceKey;
            }

            internal Criteria(int? PriceKey, int? ItmKey)
            {
                _priceKey = PriceKey;
                _itmKey = ItmKey;
            }

            internal Criteria(int? PriceKey, int? ItmKey, int? Option)
            {
                _priceKey = PriceKey;
                _itmKey = ItmKey;
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
                cm.CommandText = "MSTPriceListDetValue_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                
                cm.Parameters.AddWithValue("@PriceKey", criteria._priceKey);
                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
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

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _priceKey = dr.GetInt32("PriceKey");
            _itmKey = dr.GetInt32("ItmKey");
            _itmType = dr.GetInt32("ItmType");
            _itmDes = dr.GetString("ItmDes");
            _itmID = dr.GetString("ItmID");
            _itmQty = dr.GetDecimal("ItmQty");
            _itmPrice = dr.GetDecimal("ItmPrice");
            _customPrice = dr.GetDecimal("CustomPrice");

            if (GFunc.IsNE(dr.GetValue("EffStartDate"))) 
                _effStartDate = null;
            else
                _effStartDate = dr.GetDateTime("EffStartDate");

            if (GFunc.IsNE(dr.GetValue("EffEndDate")))
                _effEndDate = null;
            else
                _effEndDate = dr.GetDateTime("EffEndDate"); 
                            
            _effItmQty = dr.GetDecimal("EffItmQty");
            _effItmPrice = dr.GetDecimal("EffItmPrice");
            
            if (GFunc.IsNE(dr.GetValue("CreateDate")))
                _createDate = null;
            else
                _createDate = dr.GetDateTime("CreateDate"); 
            
            _createUserKey = dr.GetInt32("CreateUserKey");

            if (GFunc.IsNE(dr.GetValue("LastModifiedDate")))
                _lastModifiedDate = null;
            else
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

        internal bool Insert()
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
                cm.CommandText = "MSTPriceListDetValue_AddUpdate";
                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;                    

                if (_priceKey == null)
                    cm.Parameters.AddWithValue("@PriceKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PriceKey", _priceKey);

                if (_itmKey == null)
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_itmType == null)
                    cm.Parameters.AddWithValue("@ItmType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmType", _itmType);

                if (_itmDes == null)
                    cm.Parameters.AddWithValue("@ItmDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmDes", _itmDes);

                if (_itmQty == null)
                    cm.Parameters.AddWithValue("@ItmQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmQty", _itmQty);

                if (_itmPrice == null)
                    cm.Parameters.AddWithValue("@ItmPrice", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmPrice", _itmPrice);

                if (_customPrice == null)
                    cm.Parameters.AddWithValue("@CustomPrice", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CustomPrice", _customPrice);

                if (_effStartDate == null)
                    cm.Parameters.AddWithValue("@EffStartDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EffStartDate", _effStartDate.Value);

                if (_effEndDate == null)
                    cm.Parameters.AddWithValue("@EffEndDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EffEndDate", _effEndDate.Value);

                if (_effItmQty == null)
                    cm.Parameters.AddWithValue("@EffItmQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EffItmQty", _effItmQty);

                if (_effItmPrice == null)
                    cm.Parameters.AddWithValue("@EffItmPrice", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EffItmPrice", _effItmPrice);

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
                cm.CommandText = "MSTPriceListDetValue_AddUpdate";
                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters.AddWithValue("@NewPriceKey", 0);
                cm.Parameters.AddWithValue("@NewItmKey", 0);

                if (_priceKey == null)
                    cm.Parameters.AddWithValue("@PriceKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PriceKey", _priceKey);

                if (_itmKey == null)
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_itmType == null)
                    cm.Parameters.AddWithValue("@ItmType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmType", _itmType);

                if (_itmDes == null)
                    cm.Parameters.AddWithValue("@ItmDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmDes", _itmDes);

                if (_itmQty == null)
                    cm.Parameters.AddWithValue("@ItmQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmQty", _itmQty);

                if (_itmPrice == null)
                    cm.Parameters.AddWithValue("@ItmPrice", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmPrice", _itmPrice);

                if (_customPrice == null)
                    cm.Parameters.AddWithValue("@CustomPrice", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CustomPrice", _customPrice);

                if (_effStartDate == null)
                    cm.Parameters.AddWithValue("@EffStartDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EffStartDate", _effStartDate.Value);

                if (_effEndDate == null)
                    cm.Parameters.AddWithValue("@EffEndDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EffEndDate", _effEndDate.Value);

                if (_effItmQty == null)
                    cm.Parameters.AddWithValue("@EffItmQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EffItmQty", _effItmQty);

                if (_effItmPrice == null)
                    cm.Parameters.AddWithValue("@EffItmPrice", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EffItmPrice", _effItmPrice);

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

                cm.Parameters["@NewPriceKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewItmKey"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "MSTPriceListDetValue_Delete";
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters.AddWithValue("@PriceKey", criteria._priceKey);

                cm.ExecuteNonQuery();

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
                    retValue = this.Validation(cn, criteria,  isNew);
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
                cm.CommandText = "MSTPriceListDetValue_Validation";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters.AddWithValue("@PriceKey", criteria._priceKey);
                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }
            
        }
        #endregion //Data Access - Validation
    }
}


