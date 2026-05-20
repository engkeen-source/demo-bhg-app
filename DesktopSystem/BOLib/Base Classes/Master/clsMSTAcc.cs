

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
    public class MSTAcc : Csla.BusinessBase<MSTAcc>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _accKey = 0;
        internal int? _accTypeKey = null;
        internal string _accID = string.Empty;
        internal string _accDes = string.Empty;
        internal int? _accGrpKey = 0;
        internal string _accGrpID = string.Empty;
        internal int? _accCurrKey = 1;
        internal bool? _inactive = false;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _custom4 = string.Empty;
        internal string _custom5 = string.Empty;
        internal string _approvalStatus = string.Empty;

        public int? AccKey
        {
            get
            {
                CanReadProperty("AccKey", true);
                return _accKey;
            }
        }

        public int? AccTypeKey
        {
            get
            {
                CanReadProperty("AccTypeKey", true);
                return _accTypeKey;
            }
            set
            {
                CanWriteProperty("AccTypeKey", true);

                _accTypeKey = value;
                PropertyHasChanged("AccTypeKey");

            }
        }

        public string AccID
        {
            get
            {
                CanReadProperty("AccID", true);
                return _accID;
            }
            set
            {
                CanWriteProperty("AccID", true);
                if (value == null) value = string.Empty;

                _accID = value;
                PropertyHasChanged("AccID");

            }
        }

        public string AccDes
        {
            get
            {
                CanReadProperty("AccDes", true);
                return _accDes;
            }
            set
            {
                CanWriteProperty("AccDes", true);
                if (value == null) value = string.Empty;

                _accDes = value;
                PropertyHasChanged("AccDes");

            }
        }

        public int? AccGrpKey
        {
            get
            {
                CanReadProperty("AccGrpKey", true);
                return _accGrpKey;
            }
            set
            {

                _accGrpKey = value;
                PropertyHasChanged("AccGrpKey");

            }
        }

        public string AccGrpID
        {
            get
            {
                CanReadProperty("AccGrpID", true);
                return _accGrpID;
            }
            set
            {
                CanWriteProperty("AccGrpID", true);
                if (value == null) value = string.Empty;

                _accGrpID = value;
                PropertyHasChanged("AccGrpID");

            }
        }

        public int? AccCurrKey
        {
            get
            {
                CanReadProperty("AccCurrKey", true);
                return _accCurrKey;
            }
            set
            {
                CanWriteProperty("AccCurrKey", true);

                _accCurrKey = value;
                PropertyHasChanged("AccCurrKey");

            }
        }

        public bool? Inactive
        {
            get
            {
                CanReadProperty("Inactive", true);
                return _inactive;
            }
            set
            {
                CanWriteProperty("Inactive", true);

                _inactive = value;
                PropertyHasChanged("Inactive");

            }
        }

        public DateTime? CreateDate
        {
            get
            {
                CanReadProperty("CreateDate", true);
                return _createDate;
            }
            set
            {
                CanWriteProperty("CreateDate", true);

                _createDate = value;
                PropertyHasChanged("CreateDate");

            }
        }

        public int? CreateUserKey
        {
            get
            {
                CanReadProperty("CreateUserKey", true);
                return _createUserKey;
            }
            set
            {
                CanWriteProperty("CreateUserKey", true);

                _createUserKey = value;
                PropertyHasChanged("CreateUserKey");

            }
        }

        public DateTime? LastModifiedDate
        {
            get
            {
                CanReadProperty("LastModifiedDate", true);
                return _lastModifiedDate;
            }
            set
            {
                CanWriteProperty("LastModifiedDate", true);

                _lastModifiedDate = value;
                PropertyHasChanged("LastModifiedDate");

            }
        }

        public int? LastModifiedUserKey
        {
            get
            {
                CanReadProperty("LastModifiedUserKey", true);
                return _lastModifiedUserKey;
            }
            set
            {
                CanWriteProperty("LastModifiedUserKey", true);

                _lastModifiedUserKey = value;
                PropertyHasChanged("LastModifiedUserKey");

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
                CanWriteProperty("Custom1", true);
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
                CanWriteProperty("Custom2", true);
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
                CanWriteProperty("Custom3", true);
                if (value == null) value = string.Empty;

                _custom3 = value;
                PropertyHasChanged("Custom3");

            }
        }

        public string Custom4
        {
            get
            {
                CanReadProperty("Custom4", true);
                return _custom4;
            }
            set
            {
                CanWriteProperty("Custom4", true);
                if (value == null) value = string.Empty;
                _custom4 = value;
                PropertyHasChanged("Custom4");

            }
        }

        public string Custom5
        {
            get
            {
                CanReadProperty("Custom5", true);
                return _custom5;
            }
            set
            {
                CanWriteProperty("Custom5", true);
                if (value == null) value = string.Empty;

                _custom5 = value;
                PropertyHasChanged("Custom5");

            }
        }

        public string ApprovalStatus
        {
            get
            {
                CanReadProperty("ApprovalStatus", true);
                return _approvalStatus;
            }
            set
            {
                CanWriteProperty("ApprovalStatus", true);
                if (value == null) value = string.Empty;

                _approvalStatus = value;
                PropertyHasChanged("ApprovalStatus");

            }
        }

        protected override object GetIdValue()
        {
            return _accKey.ToString();
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
            // AccID
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "AccID");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("AccID", 50));
            //
            // AccDes
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "AccDes");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("AccDes", 255));
            //
            // AccGrpID
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("AccGrpID", 50));
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
            //
            // Custom4
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom4", 255));
            //
            // Custom5
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom5", 255));
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        public MSTAcc()
        { /* require use of factory method */ }

        internal static MSTAcc New()
        {

            MSTAcc child = new MSTAcc();

            return child;
        }

        internal static MSTAcc NewChild()
        {

            MSTAcc child = new MSTAcc();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static MSTAcc Get(SafeDataReader dr)
        {
            MSTAcc child = new MSTAcc();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static MSTAcc Get(int? accKey)
        {
            MSTAcc child = new MSTAcc();
            child.Fetch(new Criteria(accKey, 1));
            return child;
        }

        public static MSTAcc Get(SqlConnection cn, int? accKey)
        {
            MSTAcc child = new MSTAcc();
            child.Fetch(cn, new Criteria(accKey, 1));
            return child;
        }

        public static MSTAcc Get(string accID)
        {
            MSTAcc child = new MSTAcc();
            child.Fetch(new Criteria(accID, 2));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        public class Criteria
        {
            public int? _accKey = null;
            public int? _option = null;
            public string _accID = string.Empty;
            internal Criteria()
            {
            }
            internal Criteria(int? AccKey)
            {
                _accKey = AccKey;
            }
            internal Criteria(int? AccKey, string AccID)
            {
                _accKey = AccKey;
                _accID = AccID;
            }
            public Criteria(int? AccKey, int? Option)
            {
                _accKey = AccKey;
                _option = Option;
            }
            internal Criteria(string AccID, int? Option)
            {
                _accID = AccID;
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

        public bool Fetch(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;

            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTAcc_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                if (!GFunc.IsNEZ(criteria._accKey))
                    cm.Parameters.AddWithValue("@AccKey", criteria._accKey);
                else
                    cm.Parameters.AddWithValue("@AccKey", DBNull.Value);
                if (!GFunc.IsNE(criteria._accID))
                    cm.Parameters.AddWithValue("@AccID", criteria._accID);
                else
                    cm.Parameters.AddWithValue("@AccID", DBNull.Value);

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
                {
                    retValue = true;
                }
            }// Already close and dispose sql connection.            

            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _accKey = dr.GetInt32("AccKey");
            _accTypeKey = dr.GetInt32("AccTypeKey");
            _accID = dr.GetString("AccID");
            _accDes = dr.GetString("AccDes");
            _accGrpKey = dr.GetInt32("AccGrpKey");
            _accGrpID = dr.GetString("AccGrpID");
            _accCurrKey = dr.GetInt32("AccCurrKey");
            _inactive = dr.GetBoolean("Inactive");

            if (dr.GetValue("CreateDate") == null)
                _createDate = null;
            else
                _createDate = dr.GetDateTime("CreateDate");

            _createUserKey = dr.GetInt32("CreateUserKey");
            if (dr.GetValue("LastModifiedDate") == null)
                _lastModifiedDate = null;
            else
                _lastModifiedDate = dr.GetDateTime("LastModifiedDate");

            _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            _custom4 = dr.GetString("Custom4");
            _custom5 = dr.GetString("Custom5");
            _approvalStatus = dr.GetString("ApprovalStatus");
            ValidationRules.CheckRules();

            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? accKey)
        {
            bool retValue = false;
            accKey = null;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out accKey);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? accKey)
        {
            bool retValue = false;
            accKey = 0;

            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTAcc_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@NewAccKey", accKey);

                if (_accKey == null)
                    cm.Parameters.AddWithValue("@AccKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccKey", _accKey);

                if (_accTypeKey == null)
                    cm.Parameters.AddWithValue("@AccTypeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccTypeKey", _accTypeKey);

                if (_accID == null)
                    cm.Parameters.AddWithValue("@AccID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccID", _accID);

                if (_accDes == null)
                    cm.Parameters.AddWithValue("@AccDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccDes", _accDes);

                if (_accGrpKey == null)
                    cm.Parameters.AddWithValue("@AccGrpKey", 0);
                else
                    cm.Parameters.AddWithValue("@AccGrpKey", _accGrpKey);

                if (_accGrpID == null)
                    cm.Parameters.AddWithValue("@AccGrpID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccGrpID", _accGrpID);

                if (_accCurrKey == null)
                    cm.Parameters.AddWithValue("@AccCurrKey", 1);
                else
                    cm.Parameters.AddWithValue("@AccCurrKey", _accCurrKey);

                if (_inactive == null)
                    cm.Parameters.AddWithValue("@Inactive", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Inactive", _inactive);

                if (_createDate == null)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

                if (AppInfor.currentUserKey == 0)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);

                if (_lastModifiedDate == null)
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

                if (AppInfor.currentUserKey == 0)
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

                if (_custom4 == null)
                    cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom4", _custom4);

                if (_custom5 == null)
                    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom5", _custom5);

                if (_approvalStatus == null)
                    cm.Parameters.AddWithValue("@ApprovalStatus", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ApprovalStatus", _approvalStatus);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters["@NewAccKey"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    retValue = true;
                    accKey = (int)cm.Parameters["@NewAccKey"].Value;
                }
                else
                {
                    retValue = false;
                }

            }// Already close and dispose sql connection.

            return retValue;
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
                cm.CommandText = "MSTAcc_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewAccKey", 0);

                if (_accKey == null)
                    cm.Parameters.AddWithValue("@AccKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccKey", _accKey);

                if (_accTypeKey == null)
                    cm.Parameters.AddWithValue("@AccTypeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccTypeKey", _accTypeKey);

                if (_accID == null)
                    cm.Parameters.AddWithValue("@AccID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccID", _accID);

                if (_accDes == null)
                    cm.Parameters.AddWithValue("@AccDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccDes", _accDes);

                if (_accGrpKey == null)
                    cm.Parameters.AddWithValue("@AccGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccGrpKey", _accGrpKey);

                if (_accGrpID == null)
                    cm.Parameters.AddWithValue("@AccGrpID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccGrpID", _accGrpID);

                if (_accCurrKey == null)
                    cm.Parameters.AddWithValue("@AccCurrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccCurrKey", _accCurrKey);

                if (_inactive == null)
                    cm.Parameters.AddWithValue("@Inactive", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Inactive", _inactive);

                if (_createDate == null)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

                if (_createUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateUserKey", _createUserKey);

                cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);

                if (AppInfor.currentUserKey == 0)
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

                if (_custom4 == null)
                    cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom4", _custom4);

                if (_custom5 == null)
                    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom5", _custom5);
                if (_approvalStatus == null)
                    cm.Parameters.AddWithValue("@ApprovalStatus", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ApprovalStatus", _approvalStatus);

                cm.Parameters["@NewAccKey"].Direction = ParameterDirection.Output;
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.ExecuteNonQuery();

                // Check Return Value -- Changed By Richard
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
                cm.CommandText = "MSTAcc_Delete";

                cm.Parameters.AddWithValue("@AccKey", criteria._accKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                // Check Return Value -- Changed By Richard
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
                cm.CommandText = "MSTAcc_Validation";

                cm.Parameters.AddWithValue("@isNew", isNew);
                cm.Parameters.AddWithValue("@AccKey", criteria._accKey);
                cm.Parameters.AddWithValue("@AccID", criteria._accID);

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
            _accKey = 0;
            _accTypeKey = null;
            _accID = string.Empty;
            _accDes = string.Empty;
            _accGrpKey = 0;
            _accGrpID = string.Empty;
            _accCurrKey = 1;
            _inactive = false;
            _createDate = null;
            _createUserKey = null;
            _lastModifiedDate = null;
            _lastModifiedUserKey = null;
            _custom1 = string.Empty;
            _custom2 = string.Empty;
            _custom3 = string.Empty;
            _custom4 = string.Empty;
            _custom5 = string.Empty;
            _approvalStatus = string.Empty;
        }
    }
}


