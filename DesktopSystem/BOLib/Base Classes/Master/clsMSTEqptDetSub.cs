
using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Collections;
using System.Transactions;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class MSTEqptDetSub : Csla.BusinessBase<MSTEqptDetSub>, System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _eqptKey = 0;
        internal int? _eqptSubKey = 0;
        internal int? _eqptSubItmKey = 0;
        internal int? _eqptSubItmKeySelect = 0;
        internal string _eqptSubName = string.Empty;
        internal string _eqptSubDes = string.Empty;
        internal int? _eqptSubBrandKey = 0;
        internal string _eqptSubModel = string.Empty;
        internal int? _eqptSubTypeKey = 0;
        internal string _eqptSubSerialNum = string.Empty;
        internal string _eqptSubStatus = string.Empty;
        internal DateTime? _eqptSubMfgDate = null;
        internal DateTime? _eqptSubExpDate = null;
        internal string _eqptSubLoc = string.Empty;
        internal decimal? _eqptSubQty = 0;
        internal int? _eqptSubEmKey = 0;
        internal DateTime? _eqptSubSalesdate = null;
        internal decimal? _eqptSubSalesQty = 0;
        internal DateTime? _eqptSubWarrantystart = null;
        internal DateTime? _eqptSubWarrantyEnd = null;
        internal string _eqptSubWarrantydetail = string.Empty;
        internal string _eqptSubrem1 = string.Empty;
        internal string _eqptSubrem2 = string.Empty;
        internal string _eqptSubrem3 = string.Empty;
        internal string _eqptSubrem4 = string.Empty;
        internal string _eqptSubrem5 = string.Empty;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
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

        public int? EqptSubItmKey
        {
            get
            {
                return _eqptSubItmKey;
            }
            set
            {

                _eqptSubItmKey = value;
                PropertyHasChanged("EqptSubItmKey");

            }
        }

        public int? EqptSubItmKeySelect
        {
            get
            {
                return _eqptSubItmKeySelect;
            }
            set
            {

                _eqptSubItmKeySelect = value;
                PropertyHasChanged("EqptSubItmKeySelect");

            }
        }

        public string EqptSubName
        {
            get
            {
                return _eqptSubName;
            }
            set
            {

                _eqptSubName = value;
                PropertyHasChanged("EqptSubName");

            }
        }

        public string EqptSubDes
        {
            get
            {
                return _eqptSubDes;
            }
            set
            {

                _eqptSubDes = value;
                PropertyHasChanged("EqptSubDes");

            }
        }

        public int? EqptSubBrandKey
        {
            get
            {
                return _eqptSubBrandKey;
            }
            set
            {

                _eqptSubBrandKey = value;
                PropertyHasChanged("EqptSubBrandKey");

            }
        }

        public string EqptSubModel
        {
            get
            {
                return _eqptSubModel;
            }
            set
            {

                _eqptSubModel = value;
                PropertyHasChanged("EqptSubModel");

            }
        }

        public int? EqptSubTypeKey
        {
            get
            {
                return _eqptSubTypeKey;
            }
            set
            {

                _eqptSubTypeKey = value;
                PropertyHasChanged("EqptSubTypeKey");

            }
        }

        public string EqptSubSerialNum
        {
            get
            {
                return _eqptSubSerialNum;
            }
            set
            {

                _eqptSubSerialNum = value;
                PropertyHasChanged("EqptSubSerialNum");

            }
        }

        public string EqptSubStatus
        {
            get
            {
                return _eqptSubStatus;
            }
            set
            {

                _eqptSubStatus = value;
                PropertyHasChanged("EqptSubStatus");

            }
        }

        public DateTime? EqptSubMfgDate
        {
            get
            {
                return _eqptSubMfgDate;
            }
            set
            {

                _eqptSubMfgDate = value;
                PropertyHasChanged("EqptSubMfgDate");

            }
        }

        public DateTime? EqptSubExpDate
        {
            get
            {
                return _eqptSubExpDate;
            }
            set
            {

                _eqptSubExpDate = value;
                PropertyHasChanged("EqptSubExpDate");
            }
        }

        public string EqptSubLoc
        {
            get
            {
                return _eqptSubLoc;
            }
            set
            {

                _eqptSubLoc = value;
                PropertyHasChanged("EqptSubLoc");

            }
        }

        public decimal? EqptSubQty
        {
            get
            {
                return _eqptSubQty;
            }
            set
            {

                _eqptSubQty = value;
                PropertyHasChanged("EqptSubQty");

            }
        }

        public int? EqptSubEmKey
        {
            get
            {
                return _eqptSubEmKey;
            }
            set
            {
                _eqptSubEmKey = value;
                PropertyHasChanged("EqptSubEmKey");

            }
        }

        public DateTime? EqptSubSalesdate
        {
            get
            {
                return _eqptSubSalesdate;
            }
            set
            {

                _eqptSubSalesdate = value;
                PropertyHasChanged("EqptSubSalesdate");

            }
        }

        public decimal? EqptSubSalesQty
        {
            get
            {
                return _eqptSubSalesQty;
            }
            set
            {

                _eqptSubSalesQty = value;
                PropertyHasChanged("EqptSubSalesQty");
            }
        }

        public DateTime? EqptSubWarrantystart
        {
            get
            {
                return _eqptSubWarrantystart;
            }
            set
            {

                _eqptSubWarrantystart = value;
                PropertyHasChanged("EqptSubWarrantystart");

            }
        }

        public DateTime? EqptSubWarrantyEnd
        {
            get
            {
                return _eqptSubWarrantyEnd;
            }
            set
            {

                _eqptSubWarrantyEnd = value;
                PropertyHasChanged("EqptSubWarrantyEnd");

            }
        }

        public string EqptSubWarrantydetail
        {
            get
            {
                return _eqptSubWarrantydetail;
            }
            set
            {

                _eqptSubWarrantydetail = value;
                PropertyHasChanged("EqptSubWarrantydetail");

            }
        }

        public string EqptSubrem1
        {
            get
            {
                return _eqptSubrem1;
            }
            set
            {

                _eqptSubrem1 = value;
                PropertyHasChanged("EqptSubrem1");

            }
        }

        public string EqptSubrem2
        {
            get
            {
                return _eqptSubrem2;
            }
            set
            {

                _eqptSubrem2 = value;
                PropertyHasChanged("EqptSubrem2");

            }
        }

        public string EqptSubrem3
        {
            get
            {
                return _eqptSubrem3;
            }
            set
            {

                _eqptSubrem3 = value;
                PropertyHasChanged("EqptSubrem3");

            }
        }

        public string EqptSubrem4
        {
            get
            {
                return _eqptSubrem4;
            }
            set
            {

                _eqptSubrem4 = value;
                PropertyHasChanged("EqptSubrem4");

            }
        }

        public string EqptSubrem5
        {
            get
            {
                return _eqptSubrem5;
            }
            set
            {

                _eqptSubrem5 = value;
                PropertyHasChanged("EqptSubrem5");

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
            return _eqptKey.ToString() + _eqptSubKey.ToString();
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
            // EqptSubName
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "EqptSubName");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSubName", 255));
            //
            // EqptSubDes
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSubDes", 255));
            //
            // EqptSubModel
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSubModel", 50));
            //
            // EqptSubSerialNum
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSubSerialNum", 50));
            //
            // EqptSubStatus
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSubStatus", 50));
            //
            // EqptSubLoc
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSubLoc", 50));
            //
            // EqptSubrem1
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSubrem1", 255));
            //
            // EqptSubrem2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSubrem2", 255));
            //
            // EqptSubrem3
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSubrem3", 255));
            //
            // EqptSubrem4
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSubrem4", 255));
            //
            // EqptSubrem5
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSubrem5", 255));
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

        internal MSTEqptDetSub()
        { /* require use of factory method */ }

        internal static MSTEqptDetSub New()
        {

            MSTEqptDetSub child = new MSTEqptDetSub();

            return child;
        }

        internal static MSTEqptDetSub NewChild()
        {

            MSTEqptDetSub child = new MSTEqptDetSub();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();

            return child;
        }

        internal static MSTEqptDetSub Get(SafeDataReader dr)
        {
            string msgID = "RecordGetFail";
            MSTEqptDetSub child = new MSTEqptDetSub();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTEqptDetSub Get(int? eqptKey, int? eqptSubKey)
        {
            string msgID = "RecordGetFail";
            MSTEqptDetSub child = new MSTEqptDetSub();
            child.Fetch(new Criteria(eqptKey, eqptSubKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _eqptKey = null;
            public int? _eqptSubKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? EqptKey, int? EqptSubKey)
            {
                _eqptKey = EqptKey;
                _eqptSubKey = EqptSubKey;
            }

            internal Criteria(int? EqptKey, int? EqptSubKey, int? Option)
            {
                _eqptKey = EqptKey;
                _eqptSubKey = EqptSubKey;
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
                cm.CommandText = "MSTEqptDetSub_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@EqptKey", criteria._eqptKey);
                cm.Parameters.AddWithValue("@EqptSubKey", criteria._eqptSubKey);

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
                {
                    retValue = true;
                }
                else
                    retValue = false;
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _eqptKey = dr.GetInt32("EqptKey");
            _eqptSubKey = dr.GetInt32("EqptSubKey");
            _eqptSubItmKey = dr.GetInt32("EqptSubItmKey");
            _eqptSubItmKeySelect = dr.GetInt32("EqptSubItmKeySelect");
            _eqptSubName = dr.GetString("EqptSubName");
            _eqptSubDes = dr.GetString("EqptSubDes");
            _eqptSubBrandKey = dr.GetInt32("EqptSubBrandKey");
            _eqptSubModel = dr.GetString("EqptSubModel");
            _eqptSubTypeKey = dr.GetInt32("EqptSubTypeKey");
            _eqptSubSerialNum = dr.GetString("EqptSubSerialNum");
            _eqptSubStatus = dr.GetString("EqptSubStatus");
            if (GFunc.IsNE(dr.GetValue("EqptSubMfgDate")))
                _eqptSubMfgDate = null;
            else
                _eqptSubMfgDate = dr.GetDateTime("EqptSubMfgDate");

            if (GFunc.IsNE(dr.GetValue("EqptSubExpDate")))
                _eqptSubExpDate = null;
            else
                _eqptSubExpDate = dr.GetDateTime("EqptSubExpDate");

            _eqptSubLoc = dr.GetString("EqptSubLoc");
            _eqptSubQty = dr.GetDecimal("EqptSubQty");
            _eqptSubEmKey = dr.GetInt32("EqptSubEmKey");
            if (GFunc.IsNE(dr.GetValue("EqptSubSalesdate")))
                _eqptSubSalesdate = null;
            else
                _eqptSubSalesdate = dr.GetDateTime("EqptSubSalesdate");

            _eqptSubSalesQty = dr.GetDecimal("EqptSubSalesQty");
            if (GFunc.IsNE(dr.GetValue("EqptSubWarrantystart")))
                _eqptSubWarrantystart = null;
            else
                _eqptSubWarrantystart = dr.GetDateTime("EqptSubWarrantystart");

            if (GFunc.IsNE(dr.GetValue("EqptSubWarrantyEnd")))
                _eqptSubWarrantyEnd = null;
            else
                _eqptSubWarrantyEnd = dr.GetDateTime("EqptSubWarrantyEnd");

            _eqptSubWarrantydetail = dr.GetString("EqptSubWarrantydetail");
            _eqptSubrem1 = dr.GetString("EqptSubrem1");
            _eqptSubrem2 = dr.GetString("EqptSubrem2");
            _eqptSubrem3 = dr.GetString("EqptSubrem3");
            _eqptSubrem4 = dr.GetString("EqptSubrem4");
            _eqptSubrem5 = dr.GetString("EqptSubrem5");

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
                cm.CommandText = "MSTEqptDetSub_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                if (_eqptKey == null)
                    cm.Parameters.AddWithValue("@EqptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptKey", _eqptKey);

                if (_eqptSubKey == null)
                    cm.Parameters.AddWithValue("@EqptSubKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubKey", _eqptSubKey);

                if (_eqptSubItmKey == null)
                    cm.Parameters.AddWithValue("@EqptSubItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubItmKey", _eqptSubItmKey);

                if (_eqptSubItmKeySelect == null)
                    cm.Parameters.AddWithValue("@EqptSubItmKeySelect", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubItmKeySelect", _eqptSubItmKeySelect);

                if (_eqptSubName == null)
                    cm.Parameters.AddWithValue("@EqptSubName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubName", _eqptSubName);

                if (_eqptSubDes == null)
                    cm.Parameters.AddWithValue("@EqptSubDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDes", _eqptSubDes);

                if (_eqptSubBrandKey == null)
                    cm.Parameters.AddWithValue("@EqptSubBrandKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubBrandKey", _eqptSubBrandKey);

                if (_eqptSubModel == null)
                    cm.Parameters.AddWithValue("@EqptSubModel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubModel", _eqptSubModel);

                if (_eqptSubTypeKey == null)
                    cm.Parameters.AddWithValue("@EqptSubTypeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubTypeKey", _eqptSubTypeKey);

                if (_eqptSubSerialNum == null)
                    cm.Parameters.AddWithValue("@EqptSubSerialNum", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubSerialNum", _eqptSubSerialNum);

                if (_eqptSubStatus == null)
                    cm.Parameters.AddWithValue("@EqptSubStatus", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubStatus", _eqptSubStatus);

                if (_eqptSubMfgDate == null)
                    cm.Parameters.AddWithValue("@EqptSubMfgDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubMfgDate", _eqptSubMfgDate.Value);

                if (_eqptSubExpDate == null)
                    cm.Parameters.AddWithValue("@EqptSubExpDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubExpDate", _eqptSubExpDate.Value);

                if (_eqptSubLoc == null)
                    cm.Parameters.AddWithValue("@EqptSubLoc", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubLoc", _eqptSubLoc);

                if (_eqptSubQty == null)
                    cm.Parameters.AddWithValue("@EqptSubQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubQty", _eqptSubQty);

                if (_eqptSubEmKey == null)
                    cm.Parameters.AddWithValue("@EqptSubEmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubEmKey", _eqptSubEmKey);

                if (_eqptSubSalesdate == null)
                    cm.Parameters.AddWithValue("@EqptSubSalesdate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubSalesdate", _eqptSubSalesdate.Value);

                if (_eqptSubSalesQty == null)
                    cm.Parameters.AddWithValue("@EqptSubSalesQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubSalesQty", _eqptSubSalesQty);

                if (_eqptSubWarrantystart == null)
                    cm.Parameters.AddWithValue("@EqptSubWarrantystart", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubWarrantystart", _eqptSubWarrantystart.Value);

                if (_eqptSubWarrantyEnd == null)
                    cm.Parameters.AddWithValue("@EqptSubWarrantyEnd", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubWarrantyEnd", _eqptSubWarrantyEnd.Value);

                if (_eqptSubWarrantydetail == null)
                    cm.Parameters.AddWithValue("@EqptSubWarrantydetail", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubWarrantydetail", _eqptSubWarrantydetail);

                if (_eqptSubrem1 == null)
                    cm.Parameters.AddWithValue("@EqptSubrem1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubrem1", _eqptSubrem1);

                if (_eqptSubrem2 == null)
                    cm.Parameters.AddWithValue("@EqptSubrem2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubrem2", _eqptSubrem2);

                if (_eqptSubrem3 == null)
                    cm.Parameters.AddWithValue("@EqptSubrem3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubrem3", _eqptSubrem3);

                if (_eqptSubrem4 == null)
                    cm.Parameters.AddWithValue("@EqptSubrem4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubrem4", _eqptSubrem4);

                if (_eqptSubrem5 == null)
                    cm.Parameters.AddWithValue("@EqptSubrem5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubrem5", _eqptSubrem5);

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
                cm.CommandText = "MSTEqptDetSub_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);

                if (_eqptKey == null)
                    cm.Parameters.AddWithValue("@EqptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptKey", _eqptKey);

                if (_eqptSubKey == null)
                    cm.Parameters.AddWithValue("@EqptSubKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubKey", _eqptSubKey);

                if (_eqptSubItmKey == null)
                    cm.Parameters.AddWithValue("@EqptSubItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubItmKey", _eqptSubItmKey);

                if (_eqptSubItmKeySelect == null)
                    cm.Parameters.AddWithValue("@EqptSubItmKeySelect", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubItmKeySelect", _eqptSubItmKeySelect);

                if (_eqptSubName == null)
                    cm.Parameters.AddWithValue("@EqptSubName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubName", _eqptSubName);

                if (_eqptSubDes == null)
                    cm.Parameters.AddWithValue("@EqptSubDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubDes", _eqptSubDes);

                if (_eqptSubBrandKey == null)
                    cm.Parameters.AddWithValue("@EqptSubBrandKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubBrandKey", _eqptSubBrandKey);

                if (_eqptSubModel == null)
                    cm.Parameters.AddWithValue("@EqptSubModel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubModel", _eqptSubModel);

                if (_eqptSubTypeKey == null)
                    cm.Parameters.AddWithValue("@EqptSubTypeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubTypeKey", _eqptSubTypeKey);

                if (_eqptSubSerialNum == null)
                    cm.Parameters.AddWithValue("@EqptSubSerialNum", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubSerialNum", _eqptSubSerialNum);

                if (_eqptSubStatus == null)
                    cm.Parameters.AddWithValue("@EqptSubStatus", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubStatus", _eqptSubStatus);

                if (_eqptSubMfgDate == null)
                    cm.Parameters.AddWithValue("@EqptSubMfgDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubMfgDate", _eqptSubMfgDate.Value);

                if (_eqptSubExpDate == null)
                    cm.Parameters.AddWithValue("@EqptSubExpDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubExpDate", _eqptSubExpDate.Value);

                if (_eqptSubLoc == null)
                    cm.Parameters.AddWithValue("@EqptSubLoc", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubLoc", _eqptSubLoc);

                if (_eqptSubQty == null)
                    cm.Parameters.AddWithValue("@EqptSubQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubQty", _eqptSubQty);

                if (_eqptSubEmKey == null)
                    cm.Parameters.AddWithValue("@EqptSubEmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubEmKey", _eqptSubEmKey);

                if (_eqptSubSalesdate == null)
                    cm.Parameters.AddWithValue("@EqptSubSalesdate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubSalesdate", _eqptSubSalesdate.Value);

                if (_eqptSubSalesQty == null)
                    cm.Parameters.AddWithValue("@EqptSubSalesQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubSalesQty", _eqptSubSalesQty);

                if (_eqptSubWarrantystart == null)
                    cm.Parameters.AddWithValue("@EqptSubWarrantystart", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubWarrantystart", _eqptSubWarrantystart.Value);

                if (_eqptSubWarrantyEnd == null)
                    cm.Parameters.AddWithValue("@EqptSubWarrantyEnd", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubWarrantyEnd", _eqptSubWarrantyEnd.Value);

                if (_eqptSubWarrantydetail == null)
                    cm.Parameters.AddWithValue("@EqptSubWarrantydetail", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubWarrantydetail", _eqptSubWarrantydetail);

                if (_eqptSubrem1 == null)
                    cm.Parameters.AddWithValue("@EqptSubrem1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubrem1", _eqptSubrem1);

                if (_eqptSubrem2 == null)
                    cm.Parameters.AddWithValue("@EqptSubrem2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubrem2", _eqptSubrem2);

                if (_eqptSubrem3 == null)
                    cm.Parameters.AddWithValue("@EqptSubrem3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubrem3", _eqptSubrem3);

                if (_eqptSubrem4 == null)
                    cm.Parameters.AddWithValue("@EqptSubrem4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubrem4", _eqptSubrem4);

                if (_eqptSubrem5 == null)
                    cm.Parameters.AddWithValue("@EqptSubrem5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSubrem5", _eqptSubrem5);

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
                cm.CommandText = "MSTEqptDetSub_Delete";

                cm.Parameters.AddWithValue("@EqptKey", criteria._eqptKey);
                //   cm.Parameters.AddWithValue("@EqptSubKey", criteria._eqptSubKey);
                cm.Parameters.AddWithValue("@Option", criteria._option);


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
                        retValue = this.Validation(cn, criteria, isNew);
                    }// End of SqlConnection

                    // No errors - commit transaction
                    if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                }// End of TransactionScope

            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }

        internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTEqptDetSub_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@EqptKey", criteria._eqptKey);
                cm.Parameters.AddWithValue("@EqptSubKey", criteria._eqptSubKey);

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