

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
    public class MSTItmDetAss : Csla.BusinessBase<MSTItmDetAss>, System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _itmKey = null;
        internal int? _assItmKey = null;
        internal int? _assItmType = null;
        internal decimal? _assSN = null;
        internal decimal? _assQty = 0;
        internal int? _assUOMKey = 0;
        internal bool? _defaultSelection = false;
        internal bool? _lockQty = false;
        internal bool? _toPrint = false;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _error = string.Empty;

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

        public int? AssItmKey
        {
            get
            {
                return _assItmKey;
            }
            set
            {
                _assItmKey = value;
                PropertyHasChanged("AssItmKey");
            }
        }

        public int? AssItmType
        {
            get
            {
                return _assItmType;
            }
            set
            {
                _assItmType = value;
                PropertyHasChanged("AssItmType");
            }
        }

        public decimal? AssSN
        {
            get
            {
                return _assSN;
            }
            set
            {
                _assSN = value;
                PropertyHasChanged("AssSN");
            }
        }

        public decimal? AssQty
        {
            get
            {
                return _assQty;
            }
            set
            {
                _assQty = value;
                PropertyHasChanged("AssQty");
            }
        }

        public int? AssUOMKey
        {
            get
            {
                return _assUOMKey;
            }
            set
            {
                _assUOMKey = value;
                PropertyHasChanged("AssUOMKey");
            }
        }

        public bool? DefaultSelection
        {
            get
            {
                return _defaultSelection;
            }
            set
            {
                _defaultSelection = value;
                PropertyHasChanged("DefaultSelection");
            }
        }

        public bool? LockQty
        {
            get
            {
                return _lockQty;
            }
            set
            {
                _lockQty = value;
                PropertyHasChanged("LockQty");
            }
        }

        public bool? ToPrint
        {
            get
            {
                return _toPrint;
            }
            set
            {
                _toPrint = value;
                PropertyHasChanged("ToPrint");
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
            return _itmKey.ToString() + _assItmKey.ToString();
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

        public MSTItmDetAss()
        { /* require use of factory method */ }

        public static MSTItmDetAss New()
        {          
            MSTItmDetAss child = new MSTItmDetAss();          
            return child;
        }

        public static MSTItmDetAss NewChild()
        {            
            MSTItmDetAss child = new MSTItmDetAss();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();         
            return child;
        }

        public static MSTItmDetAss Get(SafeDataReader dr)
        {          
            MSTItmDetAss child = new MSTItmDetAss();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static MSTItmDetAss Get(int? itmKey, int? assItmKey)
        {            
            MSTItmDetAss child = new MSTItmDetAss();
            child.Fetch(new Criteria(itmKey, assItmKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _itmKey = null;
            public int? _assItmKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? ItmKey, int? AssItmKey)
            {
                _itmKey = ItmKey;
                _assItmKey = AssItmKey;
            }

            internal Criteria(int? ItmKey, int? AssItmKey, int? Option)
            {
                _itmKey = ItmKey;
                _assItmKey = AssItmKey;
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
                cm.CommandText = "MSTItmDetAss_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                   
                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
             //   cm.Parameters.AddWithValue("@AssItmKey", criteria._assItmKey);                  

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
                    retValue=true;
                else
                    retValue=false;

            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _itmKey = dr.GetInt32("ItmKey");
            _assItmKey = dr.GetInt32("AssItmKey");
            _assItmType = dr.GetInt32("AssItmType");
            _assSN = dr.GetDecimal("AssSN");
            _assQty = dr.GetDecimal("AssQty");
            _assUOMKey = dr.GetInt32("AssUOMKey");
            _defaultSelection = dr.GetBoolean("DefaultSelection");
            _lockQty = dr.GetBoolean("LockQty");
            _toPrint = dr.GetBoolean("ToPrint");
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
                cm.CommandText = "MSTItmDetAss_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                //cm.Parameters.AddWithValue("@NewItmKey", itmKey);
                //cm.Parameters.AddWithValue("@NewAssItmKey", assItmKey);

                if (_itmKey == null)
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_assItmKey == null)
                    cm.Parameters.AddWithValue("@AssItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AssItmKey", _assItmKey);

                if (_assItmType == null)
                    cm.Parameters.AddWithValue("@AssItmType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AssItmType", _assItmType);

                if (_assSN == null)
                    cm.Parameters.AddWithValue("@AssSN", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AssSN", _assSN);

                if (_assQty == null)
                    cm.Parameters.AddWithValue("@AssQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AssQty", _assQty);

                if (_assUOMKey == null)
                    cm.Parameters.AddWithValue("@AssUOMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AssUOMKey", _assUOMKey);

                if (_defaultSelection == null)
                    cm.Parameters.AddWithValue("@DefaultSelection", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DefaultSelection", _defaultSelection);

                if (_lockQty == null)
                    cm.Parameters.AddWithValue("@LockQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LockQty", _lockQty);

                if (_toPrint == null)
                    cm.Parameters.AddWithValue("@ToPrint", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ToPrint", _toPrint);

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

                //cm.Parameters["@NewItmKey"].Direction = ParameterDirection.Output;
                //cm.Parameters["@NewAssItmKey"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "MSTItmDetAss_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
            
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewItmKey", 0);
                cm.Parameters.AddWithValue("@NewAssItmKey", 0);

                if (_itmKey == null)
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_assItmKey == null)
                    cm.Parameters.AddWithValue("@AssItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AssItmKey", _assItmKey);

                if (_assItmType == null)
                    cm.Parameters.AddWithValue("@AssItmType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AssItmType", _assItmType);

                if (_assSN == null)
                    cm.Parameters.AddWithValue("@AssSN", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AssSN", _assSN);

                if (_assQty == null)
                    cm.Parameters.AddWithValue("@AssQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AssQty", _assQty);

                if (_assUOMKey == null)
                    cm.Parameters.AddWithValue("@AssUOMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AssUOMKey", _assUOMKey);

                if (_defaultSelection == null)
                    cm.Parameters.AddWithValue("@DefaultSelection", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DefaultSelection", _defaultSelection);

                if (_lockQty == null)
                    cm.Parameters.AddWithValue("@LockQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LockQty", _lockQty);

                if (_toPrint == null)
                    cm.Parameters.AddWithValue("@ToPrint", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ToPrint", _toPrint);

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

                cm.Parameters["@NewItmKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewAssItmKey"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "MSTItmDetAss_Delete";

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
               // cm.Parameters.AddWithValue("@AssItmKey", criteria._assItmKey);

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
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmDetAss_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
                cm.Parameters.AddWithValue("@AssItmKey", criteria._assItmKey);

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


