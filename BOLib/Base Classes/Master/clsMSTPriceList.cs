

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
    public class MSTPriceList : Csla.BusinessBase<MSTPriceList>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _priceKey = 0;
        internal string _priceID = string.Empty;
        internal string _priceDes = string.Empty;
        internal int? _priceType = 10;
        internal int? _currKey = 1;
        internal int? _buildInCode = 10;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

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
        public string PriceID
        {
            get
            {
                return _priceID;
            }
            set
            {
                _priceID = value;
                PropertyHasChanged("PriceID");
            }
        }
        public string PriceDes
        {
            get
            {
                return _priceDes;
            }
            set
            {
                _priceDes = value;
                PropertyHasChanged("PriceDes");
            }
        }
        public int? PriceType
        {
            get
            {
                return _priceType;
            }
            set
            {
                _priceType = value;
                PropertyHasChanged("PriceType");
            }
        }
        public int? CurrKey
        {
            get
            {
                return _currKey;
            }
            set
            {
                _currKey = value;
                PropertyHasChanged("CurrKey");
            }
        }
        public int? BuildInCode
        {
            get
            {
                return _buildInCode;
            }
            set
            {
                _buildInCode = value;
                PropertyHasChanged("BuildInCode");
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
            return _priceKey.ToString();
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
           // PriceID
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "PriceID");
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PriceID", 50));
           //
           // PriceDes
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "PriceDes");
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PriceDes", 255));
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

        internal MSTPriceList()
        { /* require use of factory method */ }
        internal static MSTPriceList New()
        {           
            MSTPriceList child = new MSTPriceList();            
            return child;
        }
        internal static MSTPriceList NewChild()
        {
           
            MSTPriceList child = new MSTPriceList();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }
        internal static MSTPriceList Get(SafeDataReader dr)
        {
           
            MSTPriceList child = new MSTPriceList();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }
        public static MSTPriceList Get(int? priceKey)
        {
            
            MSTPriceList child = new MSTPriceList();
            child.Fetch(new Criteria(priceKey,string.Empty, 1));
            return child;
        }
        public static MSTPriceList Get(string priceID)
        {

            MSTPriceList child = new MSTPriceList();
            child.Fetch(new Criteria(0, priceID, 2));
            return child;
        }
        public static MSTPriceList Get(SqlConnection cn, int? priceKey)
        {

            MSTPriceList child = new MSTPriceList();
            child.Fetch(cn,new Criteria(priceKey, string.Empty, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _priceKey = null;
            public int? _option = null;
            public string _priceID;
            internal Criteria()
            {
            }

            internal Criteria(int? PriceKey)
            {
                _priceKey = PriceKey;
            }

            internal Criteria(int? PriceKey, string PriceID)
            {
                _priceKey = PriceKey;
                _priceID = PriceID;
            }

            internal Criteria(int? PriceKey,string PriceID, int? Option)
            {
                _priceKey = PriceKey;
                _option = Option;
                _priceID = PriceID;
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
                cm.CommandText = "MSTPriceList_Get";
                cm.Parameters.AddWithValue("@Option", criteria._option);                    
                cm.Parameters.AddWithValue("@PriceKey", criteria._priceKey);
                cm.Parameters.AddWithValue("@PriceID", criteria._priceID);
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
            _priceKey = dr.GetInt32("PriceKey");
            _priceID = dr.GetString("PriceID");
            _priceDes = dr.GetString("PriceDes");
            _priceType = dr.GetInt32("PriceType");
            _currKey = dr.GetInt32("CurrKey");
            _buildInCode = dr.GetInt32("BuildInCode");

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

        internal bool Insert( out int priceKey)
        {
            bool retValue = false;
            
            priceKey = 0;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out priceKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }
        internal bool Insert(SqlConnection cn, out int priceKey)
        {
            priceKey = 0;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTPriceList_AddUpdate";
                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters.AddWithValue("@NewPriceKey", priceKey);
                if (_priceKey == null)
                    cm.Parameters.AddWithValue("@PriceKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PriceKey", _priceKey);

                if (_priceID == null)
                    cm.Parameters.AddWithValue("@PriceID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PriceID", _priceID);

                if (_priceDes == null)
                    cm.Parameters.AddWithValue("@PriceDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PriceDes", _priceDes);

                if (_priceType == null)
                    cm.Parameters.AddWithValue("@PriceType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PriceType", _priceType);

                if (_currKey == null)
                    cm.Parameters.AddWithValue("@CurrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CurrKey", _currKey);

                if (_buildInCode == null)
                    cm.Parameters.AddWithValue("@BuildInCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BuildInCode", _buildInCode);

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

                cm.Parameters["@NewPriceKey"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();
                
                priceKey = (int)cm.Parameters["@NewPriceKey"].Value;

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
                cm.CommandText = "MSTPriceList_AddUpdate";
                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewPriceKey", 0);

                if (_priceKey == null)
                    cm.Parameters.AddWithValue("@PriceKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PriceKey", _priceKey);

                if (_priceID == null)
                    cm.Parameters.AddWithValue("@PriceID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PriceID", _priceID);

                if (_priceDes == null)
                    cm.Parameters.AddWithValue("@PriceDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PriceDes", _priceDes);

                if (_priceType == null)
                    cm.Parameters.AddWithValue("@PriceType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PriceType", _priceType);

                if (_currKey == null)
                    cm.Parameters.AddWithValue("@CurrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CurrKey", _currKey);

                if (_buildInCode == null)
                    cm.Parameters.AddWithValue("@BuildInCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BuildInCode", _buildInCode);

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
                cm.CommandText = "MSTPriceList_Delete";
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters.AddWithValue("@PriceKey", criteria._priceKey);
                cm.ExecuteNonQuery();
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
                cm.CommandText = "MSTPriceList_Validation";
                
                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@PriceKey", criteria._priceKey);
                cm.Parameters.AddWithValue("@PriceID", criteria._priceID);
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
            _priceKey = 0;
            _priceID = string.Empty;
            _priceDes = string.Empty;
            _priceType = 10;
            _currKey = 1;
            _buildInCode = 10;
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


