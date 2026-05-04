



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
    public class MSTEqpt : Csla.BusinessBase<MSTEqpt>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _eqptKey = 0;
        internal string _eqptNm = string.Empty;
        internal string _eqptDes = string.Empty;
        internal bool? _templateYN = false;
        internal int? _eqptConKey = 0;
        internal int? _eqptItmKey = 0;
        internal int? _eqptItmKeySelect = 0;
        internal int? _eqptBrandKey = 0;
        internal string _eqptModel = string.Empty;
        internal int? _eqptTypeKey = 0;
        internal string _eqptSerialNum = string.Empty;
        internal string _eqptStatus = string.Empty;
        internal DateTime? _eqptMfgDate = null;
        internal DateTime? _eqptExpDate = null;
        internal string _eqptLoc = string.Empty;
        internal decimal? _eqptQty = 0;
        internal int? _eqptEmKey = 0;
        internal string _eqptSalesdoc = string.Empty;
        internal DateTime? _eqptSalesdate = null;
        internal decimal? _eqptSalesQty = 0;
        internal string _eqptContractnum = string.Empty;
        internal string _eqptContractdetail = string.Empty;
        internal DateTime? _eqptWarrantystart = null;
        internal DateTime? _eqptWarrantyEnd = null;
        internal string _eqptWarrantydetail = string.Empty;
        internal DateTime? _eqptMaintstart = null;
        internal int? _eqptMainGrpKey = 0;
        internal string _eqptrem1 = string.Empty;
        internal string _eqptrem2 = string.Empty;
        internal string _eqptrem3 = string.Empty;
        internal string _eqptrem4 = string.Empty;
        internal string _eqptrem5 = string.Empty;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

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

        public string EqptNm
        {
            get
            {
                return _eqptNm;
            }
            set
            {

                _eqptNm = value;
                PropertyHasChanged("EqptNm");

            }
        }

        public string EqptDes
        {
            get
            {
                return _eqptDes;
            }
            set
            {

                _eqptDes = value;
                PropertyHasChanged("EqptDes");

            }
        }

        public bool? TemplateYN
        {
            get
            {
                return _templateYN;
            }
            set
            {

                _templateYN = value;
                PropertyHasChanged("TemplateYN");

            }
        }

        public int? EqptConKey
        {
            get
            {
                return _eqptConKey;
            }
            set
            {
                _eqptConKey = value;
                PropertyHasChanged("EqptConKey");

            }
        }

        public int? EqptItmKey
        {
            get
            {
                return _eqptItmKey;
            }
            set
            {

                _eqptItmKey = value;
                PropertyHasChanged("EqptItmKey");

            }
        }

        public int? EqptItmKeySelect
        {
            get
            {
                return _eqptItmKeySelect;
            }
            set
            {

                _eqptItmKeySelect = value;
                PropertyHasChanged("EqptItmKeySelect");

            }
        }

        public int? EqptBrandKey
        {
            get
            {
                return _eqptBrandKey;
            }
            set
            {

                _eqptBrandKey = value;
                PropertyHasChanged("EqptBrandKey");

            }
        }

        public string EqptModel
        {
            get
            {
                return _eqptModel;
            }
            set
            {

                _eqptModel = value;
                PropertyHasChanged("EqptModel");

            }
        }

        public int? EqptTypeKey
        {
            get
            {
                return _eqptTypeKey;
            }
            set
            {

                _eqptTypeKey = value;
                PropertyHasChanged("EqptTypeKey");

            }
        }

        public string EqptSerialNum
        {
            get
            {
                return _eqptSerialNum;
            }
            set
            {
                _eqptSerialNum = value;
                PropertyHasChanged("EqptSerialNum");

            }
        }

        public string EqptStatus
        {
            get
            {
                return _eqptStatus;
            }
            set
            {

                _eqptStatus = value;
                PropertyHasChanged("EqptStatus");

            }
        }

        public DateTime? EqptMfgDate
        {
            get
            {
                return _eqptMfgDate;
            }
            set
            {

                _eqptMfgDate = value;
                PropertyHasChanged("EqptMfgDate");

            }
        }

        public DateTime? EqptExpDate
        {
            get
            {
                return _eqptExpDate;
            }
            set
            {

                _eqptExpDate = value;
                PropertyHasChanged("EqptExpDate");

            }
        }

        public string EqptLoc
        {
            get
            {
                return _eqptLoc;
            }
            set
            {

                _eqptLoc = value;
                PropertyHasChanged("EqptLoc");

            }
        }

        public decimal? EqptQty
        {
            get
            {
                return _eqptQty;
            }
            set
            {

                _eqptQty = value;
                PropertyHasChanged("EqptQty");

            }
        }

        public int? EqptEmKey
        {
            get
            {
                return _eqptEmKey;
            }
            set
            {

                _eqptEmKey = value;
                PropertyHasChanged("EqptEmKey");

            }
        }

        public string EqptSalesdoc
        {
            get
            {
                return _eqptSalesdoc;
            }
            set
            {

                _eqptSalesdoc = value;
                PropertyHasChanged("EqptSalesdoc");

            }
        }

        public DateTime? EqptSalesdate
        {
            get
            {
                return _eqptSalesdate;
            }
            set
            {

                _eqptSalesdate = value;
                PropertyHasChanged("EqptSalesdate");

            }
        }

        public decimal? EqptSalesQty
        {
            get
            {
                return _eqptSalesQty;
            }
            set
            {

                _eqptSalesQty = value;
                PropertyHasChanged("EqptSalesQty");

            }
        }

        public string EqptContractnum
        {
            get
            {
                return _eqptContractnum;
            }
            set
            {

                _eqptContractnum = value;
                PropertyHasChanged("EqptContractnum");

            }
        }

        public string EqptContractdetail
        {
            get
            {
                return _eqptContractdetail;
            }
            set
            {

                _eqptContractdetail = value;
                PropertyHasChanged("EqptContractdetail");

            }
        }

        public DateTime? EqptWarrantystart
        {
            get
            {
                return _eqptWarrantystart;
            }
            set
            {

                _eqptWarrantystart = value;
                PropertyHasChanged("EqptWarrantystart");

            }
        }

        public DateTime? EqptWarrantyEnd
        {
            get
            {
                return _eqptWarrantyEnd;
            }
            set
            {

                _eqptWarrantyEnd = value;
                PropertyHasChanged("EqptWarrantyEnd");

            }
        }

        public string EqptWarrantydetail
        {
            get
            {
                return _eqptWarrantydetail;
            }
            set
            {

                _eqptWarrantydetail = value;
                PropertyHasChanged("EqptWarrantydetail");

            }
        }

        public DateTime? EqptMaintstart
        {
            get
            {
                return _eqptMaintstart;
            }
            set
            {

                _eqptMaintstart = value;
                PropertyHasChanged("EqptMaintstart");

            }
        }

        public int? EqptMainGrpKey
        {
            get
            {
                return _eqptMainGrpKey;
            }
            set
            {

                _eqptMainGrpKey = value;
                PropertyHasChanged("EqptMainGrpKey");

            }
        }

        public string Eqptrem1
        {
            get
            {
                return _eqptrem1;
            }
            set
            {

                _eqptrem1 = value;
                PropertyHasChanged("Eqptrem1");

            }
        }

        public string Eqptrem2
        {
            get
            {
                return _eqptrem2;
            }
            set
            {
                if (value == null) value = string.Empty;

                _eqptrem2 = value;
                PropertyHasChanged("Eqptrem2");

            }
        }

        public string Eqptrem3
        {
            get
            {
                return _eqptrem3;
            }
            set
            {

                _eqptrem3 = value;
                PropertyHasChanged("Eqptrem3");

            }
        }

        public string Eqptrem4
        {
            get
            {
                return _eqptrem4;
            }
            set
            {

                _eqptrem4 = value;
                PropertyHasChanged("Eqptrem4");

            }
        }

        public string Eqptrem5
        {
            get
            {
                return _eqptrem5;
            }
            set
            {

                _eqptrem5 = value;
                PropertyHasChanged("Eqptrem5");

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
            return _eqptKey.ToString();
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
            // EqptNm
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "EqptNm");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptNm", 50));
            //
            // EqptDes
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptDes", 255));
            //
            // EqptModel
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptModel", 50));
            //
            // EqptSerialNum
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSerialNum", 50));
            //
            // EqptStatus
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptStatus", 50));
            //
            // EqptLoc
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptLoc", 50));
            //
            // EqptSalesdoc
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptSalesdoc", 50));
            //
            // EqptContractnum
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptContractnum", 255));
            //
            // EqptContractdetail
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EqptContractdetail", 255));
            //
            // Eqptrem1
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Eqptrem1", 255));
            //
            // Eqptrem2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Eqptrem2", 255));
            //
            // Eqptrem3
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Eqptrem3", 255));
            //
            // Eqptrem4
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Eqptrem4", 255));
            //
            // Eqptrem5
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Eqptrem5", 255));
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

        public MSTEqpt()
        { /* require use of factory method */ }

        internal static MSTEqpt New()
        {

            MSTEqpt child = new MSTEqpt();

            return child;
        }

        internal static MSTEqpt NewChild()
        {

            MSTEqpt child = new MSTEqpt();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();

            return child;
        }

        internal static MSTEqpt Get(SafeDataReader dr)
        {
            string msgID = "RecordGetFail";
            MSTEqpt child = new MSTEqpt();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTEqpt Get(int? eqptKey)
        {
            string msgID = "RecordGetFail";
            MSTEqpt child = new MSTEqpt();
            child.Fetch(new Criteria(eqptKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            internal int? _eqptKey = null;
            internal int? _option = null;
            internal int? _eqptConKey = null;
            internal string _eqptName = string.Empty;

            internal Criteria()
            {
            }

            internal Criteria(int? EqptKey)
            {
                _eqptKey = EqptKey;
            }

            internal Criteria(int? EqptKey, int? Option)
            {
                _eqptKey = EqptKey;
                _option = Option;
            }

            internal Criteria(int? EqptKey, string EqptName)
            {
                _eqptKey = EqptKey;
                _eqptName = EqptName;
            }

            internal Criteria(int? EqptConKey, int? EqptKey, int? Option)
            {
                _eqptKey = EqptKey;
                _eqptConKey = EqptConKey;
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
                cm.CommandText = "MSTEqpt_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@EqptKey", criteria._eqptKey);


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
            _eqptKey = dr.GetInt32("EqptKey");
            _eqptNm = dr.GetString("EqptNm");
            _eqptDes = dr.GetString("EqptDes");
            _templateYN = dr.GetBoolean("TemplateYN");
            _eqptConKey = dr.GetInt32("EqptConKey");
            _eqptItmKey = dr.GetInt32("EqptItmKey");
            _eqptItmKeySelect = dr.GetInt32("EqptItmKeySelect");
            _eqptBrandKey = dr.GetInt32("EqptBrandKey");
            _eqptModel = dr.GetString("EqptModel");
            _eqptTypeKey = dr.GetInt32("EqptTypeKey");
            _eqptSerialNum = dr.GetString("EqptSerialNum");
            _eqptStatus = dr.GetString("EqptStatus");

            if (GFunc.IsNE(dr.GetValue("EqptMfgDate")))
                _eqptMfgDate = null;
            else
                _eqptMfgDate = dr.GetDateTime("EqptMfgDate");

            if (GFunc.IsNE(dr.GetValue("EqptExpDate")))
                _eqptExpDate = null;
            else
                _eqptExpDate = dr.GetDateTime("EqptExpDate");

            _eqptLoc = dr.GetString("EqptLoc");
            _eqptQty = dr.GetDecimal("EqptQty");
            _eqptEmKey = dr.GetInt32("EqptEmKey");
            _eqptSalesdoc = dr.GetString("EqptSalesdoc");

            if (GFunc.IsNE(dr.GetValue("EqptSalesdate")))
                _eqptSalesdate = null;
            else
                _eqptSalesdate = dr.GetDateTime("EqptSalesdate");

            _eqptSalesQty = dr.GetDecimal("EqptSalesQty");
            _eqptContractnum = dr.GetString("EqptContractnum");
            _eqptContractdetail = dr.GetString("EqptContractdetail");

            if (GFunc.IsNE(dr.GetValue("EqptWarrantystart")))
                _eqptWarrantystart = null;
            else
                _eqptWarrantystart = dr.GetDateTime("EqptWarrantystart");

            if (GFunc.IsNE(dr.GetValue("EqptWarrantyEnd")))
                _eqptWarrantyEnd = null;
            else
                _eqptWarrantyEnd = dr.GetDateTime("EqptWarrantyEnd");

            _eqptWarrantydetail = dr.GetString("EqptWarrantydetail");

            if (GFunc.IsNE(dr.GetValue("EqptMaintstart")))
                _eqptMaintstart = null;
            else
                _eqptMaintstart = dr.GetDateTime("EqptMaintstart");

            _eqptMainGrpKey = dr.GetInt32("EqptMainGrpKey");
            _eqptrem1 = dr.GetString("Eqptrem1");
            _eqptrem2 = dr.GetString("Eqptrem2");
            _eqptrem3 = dr.GetString("Eqptrem3");
            _eqptrem4 = dr.GetString("Eqptrem4");
            _eqptrem5 = dr.GetString("Eqptrem5");

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

        internal bool Insert(out int? eqptKey)
        {
            bool retValue = false;
            eqptKey = null;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out eqptKey);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? eqptKey)
        {
            eqptKey = 0;

            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTEqpt_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@NewEqptKey", eqptKey);

                if (_eqptKey == null)
                    cm.Parameters.AddWithValue("@EqptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptKey", _eqptKey);

                if (_eqptNm == null)
                    cm.Parameters.AddWithValue("@EqptNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptNm", _eqptNm);

                if (_eqptDes == null)
                    cm.Parameters.AddWithValue("@EqptDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptDes", _eqptDes);

                if (_templateYN == null)
                    cm.Parameters.AddWithValue("@TemplateYN", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TemplateYN", _templateYN);

                if (_eqptConKey == null)
                    cm.Parameters.AddWithValue("@EqptConKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptConKey", _eqptConKey);

                if (_eqptItmKey == null)
                    cm.Parameters.AddWithValue("@EqptItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptItmKey", _eqptItmKey);

                if (_eqptItmKeySelect == null)
                    cm.Parameters.AddWithValue("@EqptItmKeySelect", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptItmKeySelect", _eqptItmKeySelect);

                if (_eqptBrandKey == null)
                    cm.Parameters.AddWithValue("@EqptBrandKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptBrandKey", _eqptBrandKey);

                if (_eqptModel == null)
                    cm.Parameters.AddWithValue("@EqptModel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptModel", _eqptModel);

                if (_eqptTypeKey == null)
                    cm.Parameters.AddWithValue("@EqptTypeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptTypeKey", _eqptTypeKey);

                if (_eqptSerialNum == null)
                    cm.Parameters.AddWithValue("@EqptSerialNum", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSerialNum", _eqptSerialNum);

                if (_eqptStatus == null)
                    cm.Parameters.AddWithValue("@EqptStatus", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptStatus", _eqptStatus);

                if (_eqptMfgDate == null)
                    cm.Parameters.AddWithValue("@EqptMfgDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptMfgDate", _eqptMfgDate.Value);

                if (_eqptExpDate == null)
                    cm.Parameters.AddWithValue("@EqptExpDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptExpDate", _eqptExpDate.Value);

                if (_eqptLoc == null)
                    cm.Parameters.AddWithValue("@EqptLoc", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptLoc", _eqptLoc);

                if (_eqptQty == null)
                    cm.Parameters.AddWithValue("@EqptQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptQty", _eqptQty);

                if (_eqptEmKey == null)
                    cm.Parameters.AddWithValue("@EqptEmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptEmKey", _eqptEmKey);

                if (_eqptSalesdoc == null)
                    cm.Parameters.AddWithValue("@EqptSalesdoc", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSalesdoc", _eqptSalesdoc);

                if (_eqptSalesdate == null)
                    cm.Parameters.AddWithValue("@EqptSalesdate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSalesdate", _eqptSalesdate.Value);

                if (_eqptSalesQty == null)
                    cm.Parameters.AddWithValue("@EqptSalesQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSalesQty", _eqptSalesQty);

                if (_eqptContractnum == null)
                    cm.Parameters.AddWithValue("@EqptContractnum", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptContractnum", _eqptContractnum);

                if (_eqptContractdetail == null)
                    cm.Parameters.AddWithValue("@EqptContractdetail", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptContractdetail", _eqptContractdetail);

                if (_eqptWarrantystart == null)
                    cm.Parameters.AddWithValue("@EqptWarrantystart", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptWarrantystart", _eqptWarrantystart.Value);

                if (_eqptWarrantyEnd == null)
                    cm.Parameters.AddWithValue("@EqptWarrantyEnd", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptWarrantyEnd", _eqptWarrantyEnd.Value);

                if (_eqptWarrantydetail == null)
                    cm.Parameters.AddWithValue("@EqptWarrantydetail", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptWarrantydetail", _eqptWarrantydetail);

                if (_eqptMaintstart == null)
                    cm.Parameters.AddWithValue("@EqptMaintstart", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptMaintstart", _eqptMaintstart.Value);

                if (_eqptMainGrpKey == null)
                    cm.Parameters.AddWithValue("@EqptMainGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptMainGrpKey", _eqptMainGrpKey);

                if (_eqptrem1 == null)
                    cm.Parameters.AddWithValue("@Eqptrem1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Eqptrem1", _eqptrem1);

                if (_eqptrem2 == null)
                    cm.Parameters.AddWithValue("@Eqptrem2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Eqptrem2", _eqptrem2);

                if (_eqptrem3 == null)
                    cm.Parameters.AddWithValue("@Eqptrem3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Eqptrem3", _eqptrem3);

                if (_eqptrem4 == null)
                    cm.Parameters.AddWithValue("@Eqptrem4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Eqptrem4", _eqptrem4);

                if (_eqptrem5 == null)
                    cm.Parameters.AddWithValue("@Eqptrem5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Eqptrem5", _eqptrem5);

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

                cm.Parameters["@NewEqptKey"].Direction = ParameterDirection.Output;
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                eqptKey = (int)cm.Parameters["@NewEqptKey"].Value;

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
                cm.CommandText = "MSTEqpt_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewEqptKey", 0);

                if (_eqptKey == null)
                    cm.Parameters.AddWithValue("@EqptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptKey", _eqptKey);

                if (_eqptNm == null)
                    cm.Parameters.AddWithValue("@EqptNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptNm", _eqptNm);

                if (_eqptDes == null)
                    cm.Parameters.AddWithValue("@EqptDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptDes", _eqptDes);

                if (_templateYN == null)
                    cm.Parameters.AddWithValue("@TemplateYN", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TemplateYN", _templateYN);

                if (_eqptConKey == null)
                    cm.Parameters.AddWithValue("@EqptConKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptConKey", _eqptConKey);

                if (_eqptItmKey == null)
                    cm.Parameters.AddWithValue("@EqptItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptItmKey", _eqptItmKey);

                if (_eqptItmKeySelect == null)
                    cm.Parameters.AddWithValue("@EqptItmKeySelect", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptItmKeySelect", _eqptItmKeySelect);

                if (_eqptBrandKey == null)
                    cm.Parameters.AddWithValue("@EqptBrandKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptBrandKey", _eqptBrandKey);

                if (_eqptModel == null)
                    cm.Parameters.AddWithValue("@EqptModel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptModel", _eqptModel);

                if (_eqptTypeKey == null)
                    cm.Parameters.AddWithValue("@EqptTypeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptTypeKey", _eqptTypeKey);

                if (_eqptSerialNum == null)
                    cm.Parameters.AddWithValue("@EqptSerialNum", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSerialNum", _eqptSerialNum);

                if (_eqptStatus == null)
                    cm.Parameters.AddWithValue("@EqptStatus", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptStatus", _eqptStatus);

                if (_eqptMfgDate == null)
                    cm.Parameters.AddWithValue("@EqptMfgDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptMfgDate", _eqptMfgDate.Value);

                if (_eqptExpDate == null)
                    cm.Parameters.AddWithValue("@EqptExpDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptExpDate", _eqptExpDate.Value);

                if (_eqptLoc == null)
                    cm.Parameters.AddWithValue("@EqptLoc", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptLoc", _eqptLoc);

                if (_eqptQty == null)
                    cm.Parameters.AddWithValue("@EqptQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptQty", _eqptQty);

                if (_eqptEmKey == null)
                    cm.Parameters.AddWithValue("@EqptEmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptEmKey", _eqptEmKey);

                if (_eqptSalesdoc == null)
                    cm.Parameters.AddWithValue("@EqptSalesdoc", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSalesdoc", _eqptSalesdoc);

                if (_eqptSalesdate == null)
                    cm.Parameters.AddWithValue("@EqptSalesdate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSalesdate", _eqptSalesdate.Value);

                if (_eqptSalesQty == null)
                    cm.Parameters.AddWithValue("@EqptSalesQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptSalesQty", _eqptSalesQty);

                if (_eqptContractnum == null)
                    cm.Parameters.AddWithValue("@EqptContractnum", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptContractnum", _eqptContractnum);

                if (_eqptContractdetail == null)
                    cm.Parameters.AddWithValue("@EqptContractdetail", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptContractdetail", _eqptContractdetail);

                if (_eqptWarrantystart == null)
                    cm.Parameters.AddWithValue("@EqptWarrantystart", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptWarrantystart", _eqptWarrantystart.Value);

                if (_eqptWarrantyEnd == null)
                    cm.Parameters.AddWithValue("@EqptWarrantyEnd", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptWarrantyEnd", _eqptWarrantyEnd.Value);

                if (_eqptWarrantydetail == null)
                    cm.Parameters.AddWithValue("@EqptWarrantydetail", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptWarrantydetail", _eqptWarrantydetail);

                if (_eqptMaintstart == null)
                    cm.Parameters.AddWithValue("@EqptMaintstart", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptMaintstart", _eqptMaintstart.Value);

                if (_eqptMainGrpKey == null)
                    cm.Parameters.AddWithValue("@EqptMainGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptMainGrpKey", _eqptMainGrpKey);

                if (_eqptrem1 == null)
                    cm.Parameters.AddWithValue("@Eqptrem1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Eqptrem1", _eqptrem1);

                if (_eqptrem2 == null)
                    cm.Parameters.AddWithValue("@Eqptrem2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Eqptrem2", _eqptrem2);

                if (_eqptrem3 == null)
                    cm.Parameters.AddWithValue("@Eqptrem3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Eqptrem3", _eqptrem3);

                if (_eqptrem4 == null)
                    cm.Parameters.AddWithValue("@Eqptrem4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Eqptrem4", _eqptrem4);

                if (_eqptrem5 == null)
                    cm.Parameters.AddWithValue("@Eqptrem5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Eqptrem5", _eqptrem5);

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

                cm.Parameters["@NewEqptKey"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "MSTEqpt_Delete";

                cm.Parameters.AddWithValue("@EqptKey", criteria._eqptKey);

                if (criteria._eqptConKey == null)
                    cm.Parameters.AddWithValue("@EqptConKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EqptConKey", criteria._eqptConKey);

                if (criteria._option == null)
                    cm.Parameters.AddWithValue("@Option", DBNull.Value);
                else
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
                cm.CommandText = "MSTEqpt_Validation";

                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@EqptKey", criteria._eqptKey);
                cm.Parameters.AddWithValue("@EqptNm", criteria._eqptName);
                cm.Parameters.AddWithValue("@RetValue", 0);
                // cm.Parameters.AddWithValue("@MsgID",msgID);
                //                    
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
            _eqptKey = 0;
            _eqptNm = string.Empty;
            _eqptDes = string.Empty;
            _templateYN = false;
            _eqptConKey = 0;
            _eqptItmKey = 0;
            _eqptItmKeySelect = 0;
            _eqptBrandKey = 0;
            _eqptModel = string.Empty;
            _eqptTypeKey = 0;
            _eqptSerialNum = string.Empty;
            _eqptStatus = string.Empty;
            _eqptMfgDate = null;
            _eqptExpDate = null;
            _eqptLoc = string.Empty;
            _eqptQty = 0;
            _eqptEmKey = 0;
            _eqptSalesdoc = string.Empty;
            _eqptSalesdate = null;
            _eqptSalesQty = 0;
            _eqptContractnum = string.Empty;
            _eqptContractdetail = string.Empty;
            _eqptWarrantystart = null;
            _eqptWarrantyEnd = null;
            _eqptWarrantydetail = string.Empty;
            _eqptMaintstart = null;
            _eqptMainGrpKey = 0;
            _eqptrem1 = string.Empty;
            _eqptrem2 = string.Empty;
            _eqptrem3 = string.Empty;
            _eqptrem4 = string.Empty;
            _eqptrem5 = string.Empty;
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