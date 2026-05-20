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
    public class SECGrpDetPerm : Csla.BusinessBase<SECGrpDetPerm>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _grpKey = 0;
        internal string _permID = string.Empty;
        internal string _permDesc = string.Empty;
        internal byte? _permCode = 0;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal int? _permGrpKey = null;
        internal int? _permType = null;
        internal int? _permSeq = null;
        internal bool _perform = false;
        internal bool _read = false;
        internal bool _edit = false;
        internal bool _add = false;
        internal bool _delete = false;
        internal bool _all = false;
        internal string _error = string.Empty;

        public int? GrpKey
        {
            get
            {
                CanReadProperty("GrpKey", true);
                return _grpKey;
            }
        }

        public string PermDesc
        {
            get
            {
                return this._permDesc;
            }
            set
            {
                this._permDesc = value;
                PropertyHasChanged("PermDesc");
            }
        }

        public string PermID
        {
            get
            {
                CanReadProperty("PermID", true);
                return _permID;
            }
        }

        public byte? PermCode
        {
            get
            {
                return _permCode;
            }
            set
            {

                _permCode = value;
                PropertyHasChanged("PermCode");

            }
        }

        public int? PermGrpKey
        {
            get
            {
                CanReadProperty("PermGrpKey", true);
                return _permGrpKey;
            }
        }

        public int? PermType
        {
            get
            {
                CanReadProperty("PermType", true);
                return _permType;
            }
        }
        public int? PermSeq
        {
            get
            {
                CanReadProperty("PermSeq", true);
                return _permSeq;
            }
        }

        public bool APPerform
        {
            get
            {
                return this._perform;
            }
            set
            {
                this._perform = value;
                PropertyHasChanged("APPerform");
            }
        }

        public bool APRead
        {
            get
            {
                return this._read;
            }
            set
            {
                this._read = value;
                PropertyHasChanged("APRead");
            }
        }

        public bool APEdit
        {
            get
            {
                return this._edit;
            }
            set
            {
                this._edit = value;
                PropertyHasChanged("APEdit");
            }
        }

        public bool APAdd
        {
            get
            {
                return this._add;
            }
            set
            {
                this._add = value;
                PropertyHasChanged("APAdd");
            }
        }

        public bool APDelete
        {
            get
            {
                return this._delete;
            }
            set
            {
                this._delete = value;
                PropertyHasChanged("APDelete");
            }
        }

        public bool APAll
        {
            get
            {
                return this._all;
            }
            set
            {
                this._all = value;
                PropertyHasChanged("APAll");
            }
        }

        public string Custom1
        {
            get
            {
                CanReadProperty("Custom1", true);
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
                CanReadProperty("Custom2", true);
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
                CanReadProperty("Custom3", true);
                return _custom3;
            }
            set
            {
                if (value == null) value = string.Empty;

                _custom3 = value;
                PropertyHasChanged("Custom3");

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
            return _grpKey.ToString() + _permID.ToString();
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
            //// PermID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "PermID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PermID", 50));
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

        public SECGrpDetPerm()
        { /* require use of factory method */ }

        internal static SECGrpDetPerm New()
        {
            SECGrpDetPerm child = new SECGrpDetPerm();
            return child;
        }

        internal static SECGrpDetPerm NewChild()
        {
            SECGrpDetPerm child = new SECGrpDetPerm();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static SECGrpDetPerm Get(SafeDataReader dr)
        {
            SECGrpDetPerm child = new SECGrpDetPerm();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SECGrpDetPerm Get(int? grpKey, string permID)
        {
            SECGrpDetPerm child = new SECGrpDetPerm();
            child.Fetch(new Criteria(grpKey, permID, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _grpKey = null;
            internal string _permID = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? GrpKey, string PermID)
            {
                _grpKey = GrpKey;
                _permID = PermID;
            }

            internal Criteria(int? GrpKey, string PermID, int? Option)
            {
                _grpKey = GrpKey;
                _permID = PermID;
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
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECGrpDetPerm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@GrpKey", criteria._grpKey);
                cm.Parameters.AddWithValue("@PermID", criteria._permID);


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
                    return true;
                else
                    return false;

            }// Already close and dispose sql connection.

        }

        internal bool Fetch(SafeDataReader dr)
        {
            _grpKey = dr.GetInt32("GrpKey");
            _permID = dr.GetString("PermID");
            _permCode = dr.GetByte("PermCode");
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
                cm.CommandText = "SECGrpDetPerm_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                if (_grpKey == null)
                    cm.Parameters.AddWithValue("@GrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@GrpKey", _grpKey);

                if (_permID == null)
                    cm.Parameters.AddWithValue("@PermID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PermID", _permID);

                if (_permCode == null)
                    cm.Parameters.AddWithValue("@PermCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PermCode", _permCode);

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
                cm.CommandText = "SECGrpDetPerm_Delete";

                cm.Parameters.AddWithValue("@GrpKey", criteria._grpKey);
                cm.Parameters.AddWithValue("@PermID", criteria._permID);

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
