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
    public class SECUserDetGrp : Csla.BusinessBase<SECUserDetGrp>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _userKey = 0;
        internal int? _grpKey = null;
        internal string _grpID = string.Empty;
        internal string _grpDes = string.Empty;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal bool? _wSelected = false;
        internal string _error = string.Empty;

        public int? UserKey
        {
            get
            {
                CanReadProperty("UserKey", true);
                return _userKey;
            }
        }

        public int? GrpKey
        {
            get
            {
                CanReadProperty("GrpKey", true);
                return _grpKey;
            }
            set
            {
                CanWriteProperty("GrpKey", true);

                _grpKey = value;
                PropertyHasChanged("GrpKey");

            }
        }

        public string GrpID
        {
            get
            {
                return _grpID;
            }
        }

        public string GrpDes
        {
            get
            {
                return this._grpDes;
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

        public bool? WSelected
        {
            get
            {
                return _wSelected;
            }
            set
            {

                _wSelected = value;
                PropertyHasChanged("WSelected");

            }
        }

        public string Error
        {
            get
            {
                return this._error;
            }
            set
            {
                if (this._error != value)
                    this._error = value;
            }
        }

        protected override object GetIdValue()
        {
            return _userKey.ToString() + _grpKey.ToString();
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

        public SECUserDetGrp()
        { /* require use of factory method */ }

        internal static SECUserDetGrp New()
        {
            SECUserDetGrp child = new SECUserDetGrp();
            return child;
        }

        internal static SECUserDetGrp NewChild()
        {
            SECUserDetGrp child = new SECUserDetGrp();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static SECUserDetGrp Get(SafeDataReader dr)
        {
            SECUserDetGrp child = new SECUserDetGrp();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SECUserDetGrp Get(int? userKey, int? grpKey)
        {
            SECUserDetGrp child = new SECUserDetGrp();
            child.Fetch(new Criteria(userKey, grpKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _userKey = null;
            public int? _grpKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? UserKey, int? GrpKey)
            {
                _userKey = UserKey;
                _grpKey = GrpKey;
            }

            internal Criteria(int? UserKey, int? GrpKey, int? Option)
            {
                _userKey = UserKey;
                _grpKey = GrpKey;
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
            string msgID = MsgID.Common.GetFail;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECUserDetGrp_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@UserKey", criteria._userKey);
                cm.Parameters.AddWithValue("@GrpKey", criteria._grpKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                // Using data reader as record set.
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    // If data reader can read, continue...
                    while (dr.Read())
                    {
                        this.Fetch(dr);
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
            _userKey = dr.GetInt32("UserKey");
            _grpKey = dr.GetInt32("GrpKey");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            _wSelected = dr.GetBoolean("wSelected");
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
                cm.CommandText = "SECUserDetGrp_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                if (_userKey == null)
                    cm.Parameters.AddWithValue("@UserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserKey", _userKey);

                if (_grpKey == null)
                    cm.Parameters.AddWithValue("@GrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@GrpKey", _grpKey);

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

                if (_wSelected == null)
                    cm.Parameters.AddWithValue("@WSelected", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@WSelected", _wSelected);


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
                cm.CommandText = "SECUserDetGrp_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewUserKey", 0);
                cm.Parameters.AddWithValue("@NewGrpKey", 0);

                if (_userKey == null)
                    cm.Parameters.AddWithValue("@UserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserKey", _userKey);

                if (_grpKey == null)
                    cm.Parameters.AddWithValue("@GrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@GrpKey", _grpKey);

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

                if (_wSelected == null)
                    cm.Parameters.AddWithValue("@WSelected", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@WSelected", _wSelected);


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
                cm.CommandText = "SECUserDetGrp_Delete";

                cm.Parameters.AddWithValue("@UserKey", criteria._userKey);
                cm.Parameters.AddWithValue("@GrpKey", criteria._grpKey);
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

    }
}