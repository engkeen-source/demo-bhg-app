

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
    public class MSTEqptDetSubItm : Csla.BusinessBase<MSTEqptDetSubItm>, System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _eqptKey = 0;
        internal int? _eqptSubKey = 0;
        internal int? _eqptSubDetKey = 0;
        internal int? _eqptSubDetItmKey = 0;
        internal int? _eqptSubDetItmKeySelect = 0;
        internal decimal? _eqptSubDetSN = 0;
        internal string _eqptSubDetDes = string.Empty;
        internal decimal? _eqptSubDetQty = 0;
        internal decimal? _eqptSubDetSalesQty = 0;
        internal string _eqptSubDetRem1 = string.Empty;
        internal string _eqptSubDetRem2 = string.Empty;
        internal string _eqptSubDetRem3 = string.Empty;
        internal string _eqptSubDetRem4 = string.Empty;
        internal string _eqptSubDetRem5 = string.Empty;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = 0;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _error = string.Empty;

        public int? EqptKey
        {
            get
            {
                return _eqptKey;
            }
            set
            {
                _eqptKey = value;
                PropertyHasChanged("EqptKey");

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

        public int? EqptSubKey
        {
            get
            {
                return _eqptSubKey;
            }
            set
            {

                _eqptSubKey = value;
                PropertyHasChanged("EqptSubKey");

            }
        }

        public int? EqptSubDetKey
        {
            get
            {
                return _eqptSubDetKey;
            }
            set
            {

                _eqptSubDetKey = value;
                PropertyHasChanged("EqptSubDetKey");

            }
        }

        public int? EqptSubDetItmKey
        {
            get
            {
                return _eqptSubDetItmKey;
            }
            set
            {

                _eqptSubDetItmKey = value;
                PropertyHasChanged("EqptSubDetItmKey");

            }
        }

        public int? EqptSubDetItmKeySelect
        {
            get
            {
                return _eqptSubDetItmKeySelect;
            }
            set
            {

                _eqptSubDetItmKeySelect = value;
                PropertyHasChanged("EqptSubDetItmKeySelect");

            }
        }

        public decimal? EqptSubDetSN
        {
            get
            {
                return _eqptSubDetSN;
            }
            set
            {

                _eqptSubDetSN = value;
                PropertyHasChanged("EqptSubDetSN");

            }
        }

        public string EqptSubDetDes
        {
            get
            {
                return _eqptSubDetDes;
            }
            set
            {

                _eqptSubDetDes = value;
                PropertyHasChanged("EqptSubDetDes");

            }
        }

        public decimal? EqptSubDetQty
        {
            get
            {
                return _eqptSubDetQty;
            }
            set
            {

                _eqptSubDetQty = value;
                PropertyHasChanged("EqptSubDetQty");

            }
        }

        public decimal? EqptSubDetSalesQty
        {
            get
            {
                return _eqptSubDetSalesQty;
            }
            set
            {

                _eqptSubDetSalesQty = value;
                PropertyHasChanged("EqptSubDetSalesQty");

            }
        }

        public string EqptSubDetRem1
        {
            get
            {
                return _eqptSubDetRem1;
            }
            set
            {

                _eqptSubDetRem1 = value;
                PropertyHasChanged("EqptSubDetRem1");

            }
        }

        public string EqptSubDetRem2
        {
            get
            {
                return _eqptSubDetRem2;
            }
            set
            {

                _eqptSubDetRem2 = value;
                PropertyHasChanged("EqptSubDetRem2");

            }
        }

        public string EqptSubDetRem3
        {
            get
            {
                return _eqptSubDetRem3;
            }
            set
            {

                _eqptSubDetRem3 = value;
                PropertyHasChanged("EqptSubDetRem3");

            }
        }

        public string EqptSubDetRem4
        {
            get
            {
                return _eqptSubDetRem4;
            }
            set
            {

                _eqptSubDetRem4 = value;
                PropertyHasChanged("EqptSubDetRem4");

            }
        }

        public string EqptSubDetRem5
        {
            get
            {
                return _eqptSubDetRem5;
            }
            set
            {

                _eqptSubDetRem5 = value;
                PropertyHasChanged("EqptSubDetRem5");

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
            return _eqptKey.ToString() + _eqptSubKey.ToString() + _eqptSubDetKey.ToString();
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
            // EqptSubDetDes
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "EqptSubDetDes");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSubDetDes", 255));
            //
            // EqptSubDetRem1
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSubDetRem1", 255));
            //
            // EqptSubDetRem2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSubDetRem2", 255));
            //
            // EqptSubDetRem3
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSubDetRem3", 255));
            //
            // EqptSubDetRem4
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSubDetRem4", 255));
            //
            // EqptSubDetRem5
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSubDetRem5", 255));
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
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal MSTEqptDetSubItm()
        { /* require use of factory method */ }

        internal static MSTEqptDetSubItm New()
        {

            MSTEqptDetSubItm child = new MSTEqptDetSubItm();

            return child;
        }

        internal static MSTEqptDetSubItm NewChild()
        {

            MSTEqptDetSubItm child = new MSTEqptDetSubItm();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();

            return child;
        }

        internal static MSTEqptDetSubItm Get(SafeDataReader dr)
        {
            string msgID = "RecordGetFail";
            MSTEqptDetSubItm child = new MSTEqptDetSubItm();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTEqptDetSubItm Get(int? eqptKey, int? eqptSubKey, int? eqptSubDetKey)
        {
            string msgID = "RecordGetFail";
            MSTEqptDetSubItm child = new MSTEqptDetSubItm();
            child.Fetch(new Criteria(eqptKey, eqptSubKey, eqptSubDetKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _eqptKey = null;
            public int? _eqptSubKey = null;
            public int? _eqptSubDetKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? EqptKey, int? EqptSubKey, int? EqptSubDetKey)
            {
                _eqptKey = EqptKey;
                _eqptSubKey = EqptSubKey;
                _eqptSubDetKey = EqptSubDetKey;
            }

            internal Criteria(int? EqptKey, int? EqptSubKey, int? EqptSubDetKey, int? Option)
            {
                _eqptKey = EqptKey;
                _eqptSubKey = EqptSubKey;
                _eqptSubDetKey = EqptSubDetKey;
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
                cm.CommandText = "MSTEqptDetSubItm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@EqptKey", criteria._eqptKey);
                cm.Parameters.AddWithValue("@EqptSubKey", criteria._eqptSubKey);
                cm.Parameters.AddWithValue("@EqptSubDetKey", criteria._eqptSubDetKey);

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

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;
                    else
                        retValue = false;


                }	// Already close and dispose data reader.

            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _eqptKey = dr.GetInt32("EqptKey");
            _eqptSubKey = dr.GetInt32("EqptSubKey");
            _eqptSubDetKey = dr.GetInt32("EqptSubDetKey");
            _eqptSubDetItmKey = dr.GetInt32("EqptSubDetItmKey");
            _eqptSubDetItmKeySelect = dr.GetInt32("EqptSubDetItmKeySelect");
            _eqptSubDetSN = dr.GetDecimal("EqptSubDetSN");
            _eqptSubDetDes = dr.GetString("EqptSubDetDes");
            _eqptSubDetQty = dr.GetDecimal("EqptSubDetQty");
            _eqptSubDetSalesQty = dr.GetDecimal("EqptSubDetSalesQty");
            _eqptSubDetRem1 = dr.GetString("EqptSubDetRem1");
            _eqptSubDetRem2 = dr.GetString("EqptSubDetRem2");
            _eqptSubDetRem3 = dr.GetString("EqptSubDetRem3");
            _eqptSubDetRem4 = dr.GetString("EqptSubDetRem4");
            _eqptSubDetRem5 = dr.GetString("EqptSubDetRem5");

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

        internal bool Insert(out int? eqptKey, out int? eqptSubKey, out int? eqptSubDetKey)
        {
            bool retValue = false;
            eqptKey = null;
            eqptSubKey = null;
            eqptSubDetKey = null;

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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Insert(SqlConnection cn)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTEqptDetSubItm_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                if (_eqptKey == null)
                    cm.Parameters.AddWithValue("@EqptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptKey", _eqptKey);

                if (_eqptSubKey == null)
                    cm.Parameters.AddWithValue("@EqptSubKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubKey", _eqptSubKey);

                if (_eqptSubDetKey == null)
                    cm.Parameters.AddWithValue("@EqptSubDetKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetKey", _eqptSubDetKey);

                if (_eqptSubDetItmKey == null)
                    cm.Parameters.AddWithValue("@EqptSubDetItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetItmKey", _eqptSubDetItmKey);

                if (_eqptSubDetItmKeySelect == null)
                    cm.Parameters.AddWithValue("@EqptSubDetItmKeySelect", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetItmKeySelect", _eqptSubDetItmKeySelect);

                if (_eqptSubDetSN == null)
                    cm.Parameters.AddWithValue("@EqptSubDetSN", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetSN", _eqptSubDetSN);

                if (_eqptSubDetDes == null)
                    cm.Parameters.AddWithValue("@EqptSubDetDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetDes", _eqptSubDetDes);

                if (_eqptSubDetQty == null)
                    cm.Parameters.AddWithValue("@EqptSubDetQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetQty", _eqptSubDetQty);

                if (_eqptSubDetSalesQty == null)
                    cm.Parameters.AddWithValue("@EqptSubDetSalesQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetSalesQty", _eqptSubDetSalesQty);

                if (_eqptSubDetRem1 == null)
                    cm.Parameters.AddWithValue("@EqptSubDetRem1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetRem1", _eqptSubDetRem1);

                if (_eqptSubDetRem2 == null)
                    cm.Parameters.AddWithValue("@EqptSubDetRem2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetRem2", _eqptSubDetRem2);

                if (_eqptSubDetRem3 == null)
                    cm.Parameters.AddWithValue("@EqptSubDetRem3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetRem3", _eqptSubDetRem3);

                if (_eqptSubDetRem4 == null)
                    cm.Parameters.AddWithValue("@EqptSubDetRem4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetRem4", _eqptSubDetRem4);

                if (_eqptSubDetRem5 == null)
                    cm.Parameters.AddWithValue("@EqptSubDetRem5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetRem5", _eqptSubDetRem5);

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


                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope


            return retValue;
        }

        internal bool Update(SqlConnection cn)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTEqptDetSubItm_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);

                if (_eqptKey == null)
                    cm.Parameters.AddWithValue("@EqptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptKey", _eqptKey);

                if (_eqptSubKey == null)
                    cm.Parameters.AddWithValue("@EqptSubKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubKey", _eqptSubKey);

                if (_eqptSubDetKey == null)
                    cm.Parameters.AddWithValue("@EqptSubDetKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetKey", _eqptSubDetKey);

                if (_eqptSubDetItmKey == null)
                    cm.Parameters.AddWithValue("@EqptSubDetItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetItmKey", _eqptSubDetItmKey);

                if (_eqptSubDetItmKeySelect == null)
                    cm.Parameters.AddWithValue("@EqptSubDetItmKeySelect", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetItmKeySelect", _eqptSubDetItmKeySelect);

                if (_eqptSubDetSN == null)
                    cm.Parameters.AddWithValue("@EqptSubDetSN", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetSN", _eqptSubDetSN);

                if (_eqptSubDetDes == null)
                    cm.Parameters.AddWithValue("@EqptSubDetDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetDes", _eqptSubDetDes);

                if (_eqptSubDetQty == null)
                    cm.Parameters.AddWithValue("@EqptSubDetQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetQty", _eqptSubDetQty);

                if (_eqptSubDetSalesQty == null)
                    cm.Parameters.AddWithValue("@EqptSubDetSalesQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetSalesQty", _eqptSubDetSalesQty);

                if (_eqptSubDetRem1 == null)
                    cm.Parameters.AddWithValue("@EqptSubDetRem1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetRem1", _eqptSubDetRem1);

                if (_eqptSubDetRem2 == null)
                    cm.Parameters.AddWithValue("@EqptSubDetRem2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetRem2", _eqptSubDetRem2);

                if (_eqptSubDetRem3 == null)
                    cm.Parameters.AddWithValue("@EqptSubDetRem3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetRem3", _eqptSubDetRem3);

                if (_eqptSubDetRem4 == null)
                    cm.Parameters.AddWithValue("@EqptSubDetRem4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetRem4", _eqptSubDetRem4);

                if (_eqptSubDetRem5 == null)
                    cm.Parameters.AddWithValue("@EqptSubDetRem5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDetRem5", _eqptSubDetRem5);

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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTEqptDetSubItm_Delete";

                cm.Parameters.AddWithValue("@EqptKey", criteria._eqptKey);
                cm.Parameters.AddWithValue("@EqptSubKey", criteria._eqptSubKey);
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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTEqptDetSubItm_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@EqptKey", criteria._eqptKey);
                cm.Parameters.AddWithValue("@EqptSubKey", criteria._eqptSubKey);
                cm.Parameters.AddWithValue("@EqptSubDetKey", criteria._eqptSubDetKey);

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
    }
}