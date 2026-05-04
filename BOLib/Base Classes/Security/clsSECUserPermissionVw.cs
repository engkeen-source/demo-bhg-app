

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
    public class SECUserPermissionVw : Csla.BusinessBase<SECUserPermissionVw>
    {
        #region Business Properties and Methods

        //declare members
        internal Guid? _securityKey = null;
        internal string _permID = string.Empty;
        internal byte? _permCode = null;
        internal bool _canList = false;
        internal bool _canPerform = false;
        internal bool _canRead = false;
        internal bool _canEdit = false;
        internal bool _canAdd = false;
        internal bool _canDelete = false;
        internal int _permType = 0;

        public System.Guid? SecurityKey
        {
            get
            {
                CanReadProperty("SecurityKey", true);
                return _securityKey;
            }
            set
            {
                CanWriteProperty("SecurityKey", true);

                _securityKey = value;
                PropertyHasChanged("SecurityKey");

            }
        }

        public string PermID
        {
            get
            {
                CanReadProperty("PermID", true);
                return _permID;
            }
            set
            {
                CanWriteProperty("PermID", true);
                if (value == null) value = string.Empty;

                _permID = value;
                PropertyHasChanged("PermID");

            }
        }

        public byte? PermCode
        {
            get
            {
                CanReadProperty("PermCode", true);
                return _permCode;
            }
            set
            {
                CanWriteProperty("PermCode", true);

                _permCode = value;
                PropertyHasChanged("PermCode");

            }
        }

        public bool CanList
        {
            get
            {
                CanReadProperty("CanList", true);
                return _canList;
            }
            set
            {
                CanWriteProperty("CanList", true);

                _canList = value;
                PropertyHasChanged("CanList");

            }
        }

        public bool CanPerform
        {
            get
            {
                CanReadProperty("CanPerform", true);
                return _canPerform;
            }
            set
            {
                CanWriteProperty("CanPerform", true);

                _canPerform = value;
                PropertyHasChanged("CanPerform");

            }
        }

        public bool CanRead
        {
            get
            {
                CanReadProperty("CanRead", true);
                return _canRead;
            }
            set
            {
                CanWriteProperty("CanRead", true);

                _canRead = value;
                PropertyHasChanged("CanRead");

            }
        }

        public bool CanEdit
        {
            get
            {
                CanReadProperty("CanEdit", true);
                return _canEdit;
            }
            set
            {
                CanWriteProperty("CanEdit", true);

                _canEdit = value;
                PropertyHasChanged("CanEdit");

            }
        }

        public bool CanAdd
        {
            get
            {
                CanReadProperty("CanAdd", true);
                return _canAdd;
            }
            set
            {
                CanWriteProperty("CanAdd", true);

                _canAdd = value;
                PropertyHasChanged("CanAdd");

            }
        }

        public bool CanDelete
        {
            get
            {
                CanReadProperty("CanDelete", true);
                return _canDelete;
            }
            set
            {
                CanWriteProperty("CanDelete", true);

                _canDelete = value;
                PropertyHasChanged("CanDelete");

            }
        }

        //Created new properties to include msgID parameter
        public bool Read(ref string msgID)
        {
            if (!_canRead)
            {
                msgID = MsgID.Permission.PermReadIsFalse;
            }
            return _canRead;
        }

        public bool Add(ref string msgID)
        {
            if (!_canAdd)
            {
                msgID = MsgID.Permission.PermAddIsFalse;
            }
            return _canAdd;
        }

        public bool Edit(ref string msgID)
        {
            if (!_canEdit)
            {
                msgID = MsgID.Permission.PermEditIsFalse;
            }
            return _canEdit;
        }

        public bool Delete(ref string msgID)
        {
            if (!_canDelete)
            {
                msgID = MsgID.Permission.PermDeletesFalse;
            }
            return _canDelete;
        }

        public bool Perform(ref string msgID)
        {
            if (!_canPerform)
            {
                msgID = MsgID.Permission.PermPerformIsFalse;
            }
            return _canPerform;
        }

        protected override object GetIdValue()
        {
            return _securityKey.ToString() + _permID.ToString();
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
            // PermID
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "PermID");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PermID", 50));
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal SECUserPermissionVw()
        { /* require use of factory method */ }

        internal static SECUserPermissionVw New()
        {

            SECUserPermissionVw child = new SECUserPermissionVw();

            return child;
        }

        internal static SECUserPermissionVw NewChild()
        {

            SECUserPermissionVw child = new SECUserPermissionVw();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();

            return child;
        }

        internal static SECUserPermissionVw Get(SafeDataReader dr)
        {

            SECUserPermissionVw child = new SECUserPermissionVw();
            child.MarkAsChild();

            return child;
        }

        internal static SECUserPermissionVw Get(Guid? securityKey, string permID)
        {

            SECUserPermissionVw child = new SECUserPermissionVw();
            child.Fetch(new Criteria(securityKey, permID, 1));
            return child;
        }
        internal static SECUserPermissionVw Get(SqlConnection cn, Guid? securityKey, string permID)
        {

            SECUserPermissionVw child = new SECUserPermissionVw();
            child.Fetch(cn, new Criteria(securityKey, permID, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public Guid? _securityKey = null;
            internal string _permID = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(Guid? SecurityKey, string PermID)
            {
                _securityKey = SecurityKey;
                _permID = PermID;
            }

            internal Criteria(Guid? SecurityKey, string PermID, int? Option)
            {
                _securityKey = SecurityKey;
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
            string msgID = MsgID.Common.GetFail;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECUserPermissionVw_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@SecurityKey", criteria._securityKey);
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

            _securityKey = dr.GetGuid("SecurityKey");
            _permID = dr.GetString("PermID");
            _permCode = dr.GetByte("PermCode");
            _canList = dr.GetBoolean("CanList");
            _canPerform = dr.GetBoolean("CanPerform");
            _canRead = dr.GetBoolean("CanRead");
            _canEdit = dr.GetBoolean("CanEdit");
            _canAdd = dr.GetBoolean("CanAdd");
            _canDelete = dr.GetBoolean("CanDelete");
            _permType = dr.GetInt32("PermType");
           
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
            string msgID = MsgID.Common.AddFail;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECUserPermissionVw_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@MsgID", msgID);

                if (_securityKey == null)
                    cm.Parameters.AddWithValue("@SecurityKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SecurityKey", _securityKey);

                if (_permID == null)
                    cm.Parameters.AddWithValue("@PermID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PermID", _permID);

                if (_permCode == null)
                    cm.Parameters.AddWithValue("@PermCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PermCode", _permCode);

                if (_canList == null)
                    cm.Parameters.AddWithValue("@CanList", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CanList", _canList);

                if (_canPerform == null)
                    cm.Parameters.AddWithValue("@CanPerform", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CanPerform", _canPerform);

                if (_canRead == null)
                    cm.Parameters.AddWithValue("@CanRead", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CanRead", _canRead);

                if (_canEdit == null)
                    cm.Parameters.AddWithValue("@CanEdit", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CanEdit", _canEdit);

                if (_canAdd == null)
                    cm.Parameters.AddWithValue("@CanAdd", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CanAdd", _canAdd);

                if (_canDelete == null)
                    cm.Parameters.AddWithValue("@CanDelete", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CanDelete", _canDelete);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;



                cm.ExecuteNonQuery();

                msgID = cm.Parameters["@MsgID"].Value.ToString();

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
                cm.CommandText = "SECUserPermissionVw_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);



                if (_securityKey == null)
                    cm.Parameters.AddWithValue("@SecurityKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SecurityKey", _securityKey);

                if (_permID == null)
                    cm.Parameters.AddWithValue("@PermID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PermID", _permID);

                if (_permCode == null)
                    cm.Parameters.AddWithValue("@PermCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PermCode", _permCode);

                if (_canList == null)
                    cm.Parameters.AddWithValue("@CanList", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CanList", _canList);

                if (_canPerform == null)
                    cm.Parameters.AddWithValue("@CanPerform", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CanPerform", _canPerform);

                if (_canRead == null)
                    cm.Parameters.AddWithValue("@CanRead", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CanRead", _canRead);

                if (_canEdit == null)
                    cm.Parameters.AddWithValue("@CanEdit", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CanEdit", _canEdit);

                if (_canAdd == null)
                    cm.Parameters.AddWithValue("@CanAdd", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CanAdd", _canAdd);

                if (_canDelete == null)
                    cm.Parameters.AddWithValue("@CanDelete", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CanDelete", _canDelete);

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

            string msgID = MsgID.Common.DeleteFail;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECUserPermissionVw_Delete";

                cm.Parameters.AddWithValue("@MsgID", msgID);
                cm.Parameters.AddWithValue("@SecurityKey", criteria._securityKey);
                cm.Parameters.AddWithValue("@PermID", criteria._permID);


                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                if (cm.Parameters["@MsgID"].Value == null)
                    msgID = string.Empty;
                else
                    msgID = cm.Parameters["@MsgID"].Value.ToString();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.

        }

        #endregion //Data Access - Delete

    }
}


