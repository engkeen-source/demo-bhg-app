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
    public class REFTaxADetItm : Csla.BusinessBase<REFTaxADetItm>, System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _taxKey = 0;
        internal DateTime? _effDate = null;
        internal decimal? _taxRate = 0;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _error = string.Empty;

        public int? TaxKey
        {
            get
            {
                CanReadProperty("TaxKey", true);
                return _taxKey;
            }
        }

        public DateTime? EffDate
        {
            get
            {
                return _effDate;
            }
            set
            {


                _effDate = value;
                PropertyHasChanged("EffDate");


            }
        }

        public decimal? TaxRate
        {
            get
            {
                return _taxRate;
            }
            set
            {

                _taxRate = value;
                PropertyHasChanged("TaxRate");

            }
        }

        public DateTime? CreateDate
        {
            get
            {
                CanReadProperty("CreateDate", true);
                return _createDate;
            }
        }

        public int? CreateUserKey
        {
            get
            {
                CanReadProperty("CreateUserKey", true);
                return _createUserKey;
            }
        }

        public DateTime? LastModifiedDate
        {
            get
            {
                CanReadProperty("LastModifiedDate", true);
                return _lastModifiedDate;
            }
        }

        public int? LastModifiedUserKey
        {
            get
            {
                CanReadProperty("LastModifiedUserKey", true);
                return _lastModifiedUserKey;
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

                _error = value;

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
            return _taxKey.ToString() + _effDate.ToString();
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
            //// EffDate
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "EffDateString");
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

        public REFTaxADetItm()
        { /* require use of factory method */ }

        public static REFTaxADetItm New()
        {
            REFTaxADetItm child = new REFTaxADetItm();
            return child;
        }

        internal static REFTaxADetItm NewChild()
        {
            REFTaxADetItm child = new REFTaxADetItm();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static REFTaxADetItm Get(SafeDataReader dr)
        {
            REFTaxADetItm child = new REFTaxADetItm();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static REFTaxADetItm Get(int? taxKey, DateTime? effDate)
        {
            REFTaxADetItm child = new REFTaxADetItm();
            child.Fetch(new Criteria(taxKey, effDate, 2));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _taxKey = null;
            public DateTime? _effDate = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? TaxKey)
            {
                _taxKey = TaxKey;
            }

            internal Criteria(int? TaxKey, DateTime? EffDate)
            {
                _taxKey = TaxKey;
                _effDate = EffDate;
            }

            internal Criteria(int? TaxKey, DateTime? EffDate, int? Option)
            {
                _taxKey = TaxKey;
                _effDate = EffDate;
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
                cm.CommandText = "REFTaxADetItm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@TaxKey", criteria._taxKey);
                cm.Parameters.AddWithValue("@EffDate", criteria._effDate);

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
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _taxKey = dr.GetInt32("TaxKey");
            _effDate = dr.GetDateTime("EffDate");
            _taxRate = dr.GetDecimal("TaxRate");
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

        internal bool Insert(out int? taxKey, out DateTime? effDate)
        {
            bool retValue = false;
            taxKey = null;
            effDate = null;

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
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFTaxADetItm_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                if (_taxKey == null)
                    cm.Parameters.AddWithValue("@TaxKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaxKey", _taxKey);

                if (_effDate == null)
                    cm.Parameters.AddWithValue("@EffDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EffDate", _effDate.Value);

                if (_taxRate == null)
                    cm.Parameters.AddWithValue("@TaxRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaxRate", _taxRate);

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
                cm.CommandText = "REFTaxADetItm_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewTaxKey", 0);
                cm.Parameters.AddWithValue("@NewEffDate", DateTime.Today);

                if (_taxKey == null)
                    cm.Parameters.AddWithValue("@TaxKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaxKey", _taxKey);

                if (_effDate == null)
                    cm.Parameters.AddWithValue("@EffDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EffDate", _effDate.Value);

                if (_taxRate == null)
                    cm.Parameters.AddWithValue("@TaxRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaxRate", _taxRate);

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


                cm.Parameters["@NewTaxKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewEffDate"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "REFTaxADetItm_Delete";

                cm.Parameters.AddWithValue("@TaxKey", criteria._taxKey);

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
                cm.CommandText = "REFTaxADetItm_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@TaxKey", criteria._taxKey);
                cm.Parameters.AddWithValue("@EffDate", criteria._effDate);

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
