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
    public class REFTaxGrp : Csla.BusinessBase<REFTaxGrp>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _taxGrpKey = 0;
        internal string _taxGrpID = string.Empty;
        internal string _taxGrpDes = string.Empty;
        internal bool? _gst = false;
        internal bool? _gSTCustom = false;
        internal int? _currKey = 1;
        internal decimal? _openBal = 0;
        internal short? _reportCode = 10;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public int? TaxGrpKey
        {
            get
            {
                CanReadProperty("TaxGrpKey", true);
                return _taxGrpKey;
            }
        }

        public string TaxGrpID
        {
            get
            {
                return _taxGrpID;
            }
            set
            {

                if (value == null) value = string.Empty;

                _taxGrpID = value;
                PropertyHasChanged("TaxGrpID");


            }
        }

        public string TaxGrpDes
        {
            get
            {

                return _taxGrpDes;
            }
            set
            {

                if (value == null) value = string.Empty;

                _taxGrpDes = value;
                PropertyHasChanged("TaxGrpDes");


            }
        }

        public bool? GST
        {
            get
            {
                return _gst;
            }
            set
            {


                _gst = value;
                PropertyHasChanged("GST");


            }
        }

        public bool? GSTCustom
        {
            get
            {
                return _gSTCustom;
            }
            set
            {


                _gSTCustom = value;
                PropertyHasChanged("GSTCustom");


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

        public decimal? OpenBal
        {
            get
            {
                return _openBal;
            }
            set
            {

                _openBal = value;
                PropertyHasChanged("OpenBal");


            }
        }

        public short? ReportCode
        {
            get
            {
                return _reportCode;
            }
            set
            {

                _reportCode = value;
                PropertyHasChanged("ReportCode");

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
                if (value == null) value = string.Empty;

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

                if (value == null) value = string.Empty;

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

                if (value == null) value = string.Empty;

                _custom3 = value;
                PropertyHasChanged("Custom3");


            }
        }

        protected override object GetIdValue()
        {
            return _taxGrpKey.ToString();
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
            //// TaxGrpID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "TaxGrpID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TaxGrpID", 50));
            ////
            //// TaxGrpDes
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "TaxGrpDes");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TaxGrpDes", 255));
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

        public REFTaxGrp()
        { /* require use of factory method */ }

        public static REFTaxGrp New()
        {
            REFTaxGrp child = new REFTaxGrp();
            return child;
        }

        public static REFTaxGrp NewChild()
        {
            REFTaxGrp child = new REFTaxGrp();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        public static REFTaxGrp Get(SafeDataReader dr)
        {
            REFTaxGrp child = new REFTaxGrp();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static REFTaxGrp Get(int? taxGrpKey)
        {
            REFTaxGrp child = new REFTaxGrp();
            child.Fetch(new Criteria(taxGrpKey, 1));
            return child;
        }

        public static REFTaxGrp Get(SqlConnection cn, int? taxGrpKey)
        {
            REFTaxGrp child = new REFTaxGrp();
            child.Fetch(cn, new Criteria(taxGrpKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _taxGrpKey = null;
            public int? _option = null;
            public string _taxGrpID = string.Empty;

            internal Criteria()
            {
            }

            internal Criteria(int? TaxGrpKey)
            {
                _taxGrpKey = TaxGrpKey;
            }

            internal Criteria(int? TaxGrpKey, int? Option)
            {
                _taxGrpKey = TaxGrpKey;
                _option = Option;
            }
            //Add Thida
            internal Criteria(int? TaxGrpKey, string TaxGrpID)
            {
                _taxGrpKey = TaxGrpKey;
                _taxGrpID = TaxGrpID;
            }
            //Add Thida
            internal Criteria(int? TaxGrpKey, string TaxGrpID, int? Option)
            {
                _taxGrpKey = TaxGrpKey;
                _taxGrpID = TaxGrpID;
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
                cm.CommandText = "REFTaxGrp_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@TaxGrpKey", criteria._taxGrpKey);

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
            _taxGrpKey = dr.GetInt32("TaxGrpKey");
            _taxGrpID = dr.GetString("TaxGrpID");
            _taxGrpDes = dr.GetString("TaxGrpDes");
            _gst = dr.GetBoolean("GST");
            _gSTCustom = dr.GetBoolean("GSTCustom");
            _currKey = dr.GetInt32("CurrKey");
            _openBal = dr.GetDecimal("OpenBal");
            _reportCode = dr.GetInt16("ReportCode");
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

        internal bool Insert(out int? taxGrpKey)
        {
            bool retValue = false;
            taxGrpKey = null;
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out taxGrpKey);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? taxGrpKey)
        {
            taxGrpKey = 0;

            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFTaxGrp_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@NewTaxGrpKey", taxGrpKey);

                if (_taxGrpKey == null)
                    cm.Parameters.AddWithValue("@TaxGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaxGrpKey", _taxGrpKey);

                if (_taxGrpID == null)
                    cm.Parameters.AddWithValue("@TaxGrpID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaxGrpID", _taxGrpID);

                if (_taxGrpDes == null)
                    cm.Parameters.AddWithValue("@TaxGrpDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaxGrpDes", _taxGrpDes);

                if (_gst == null)
                    cm.Parameters.AddWithValue("@Gst", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Gst", _gst);

                if (_gSTCustom == null)
                    cm.Parameters.AddWithValue("@GSTCustom", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@GSTCustom", _gSTCustom);

                if (_currKey == null)
                    cm.Parameters.AddWithValue("@CurrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CurrKey", _currKey);

                if (_openBal == null)
                    cm.Parameters.AddWithValue("@OpenBal", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpenBal", _openBal);

                if (_reportCode == null)
                    cm.Parameters.AddWithValue("@ReportCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ReportCode", _reportCode);

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

                cm.Parameters["@NewTaxGrpKey"].Direction = ParameterDirection.Output;

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                taxGrpKey = (int)cm.Parameters["@NewTaxGrpKey"].Value;

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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
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
                cm.CommandText = "REFTaxGrp_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewTaxGrpKey", 0);

                if (_taxGrpKey == null)
                    cm.Parameters.AddWithValue("@TaxGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaxGrpKey", _taxGrpKey);

                if (_taxGrpID == null)
                    cm.Parameters.AddWithValue("@TaxGrpID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaxGrpID", _taxGrpID);

                if (_taxGrpDes == null)
                    cm.Parameters.AddWithValue("@TaxGrpDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaxGrpDes", _taxGrpDes);

                if (_gst == null)
                    cm.Parameters.AddWithValue("@Gst", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Gst", _gst);

                if (_gSTCustom == null)
                    cm.Parameters.AddWithValue("@GSTCustom", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@GSTCustom", _gSTCustom);

                if (_currKey == null)
                    cm.Parameters.AddWithValue("@CurrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CurrKey", _currKey);

                if (_openBal == null)
                    cm.Parameters.AddWithValue("@OpenBal", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpenBal", _openBal);

                if (_reportCode == null)
                    cm.Parameters.AddWithValue("@ReportCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ReportCode", _reportCode);

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

                cm.Parameters["@NewTaxGrpKey"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "REFTaxGrp_Delete";

                cm.Parameters.AddWithValue("@TaxGrpKey", criteria._taxGrpKey);

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
                cm.CommandText = "REFTaxGrp_Validation";

                cm.Parameters.AddWithValue("@isNew", isNew);
                cm.Parameters.AddWithValue("@TaxGrpKey", criteria._taxGrpKey);
                cm.Parameters.AddWithValue("@TaxGrpID", criteria._taxGrpID);

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
            _taxGrpKey = 0;
            _taxGrpID = string.Empty;
            _taxGrpDes = string.Empty;
            _gst = false;
            _gSTCustom = false;
            _currKey = 1;
            _openBal = 0;
            _reportCode = 10;
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
