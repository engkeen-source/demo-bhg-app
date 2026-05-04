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
    public class SECUser : Csla.BusinessBase<SECUser>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _userKey = 0;
        internal string _userID = string.Empty;
        internal string _userName = string.Empty;
        internal string _password = string.Empty;
        internal bool? _changePWNL = false;
        internal bool? _changePW = true;
        internal bool? _accDisabled = false;
        internal bool? _accLockOut = false;
        internal DateTime? _accLockOutTimeStamp = null;
        internal int? _accessLevel = 0;
        internal int? _branchKey = 0;
        internal int? _deptKey = 0;
        internal int? _tranGrpKey = null;
        internal int? _emKey = null;
        internal int? _conAccessLevel = 0;
        internal int? _conAccessGrp = 0;
        internal int? _itmAccessLevel = 0;
        internal int? _itmAccessGrp = 0;
        internal int? _jobAccessLevel = 0;
        internal int? _jobAccessGrp = 0;
        internal bool? _builtIn = false;
        internal short? _loginRetries = 0;
        internal DateTime? _loginTimeStampCurrent = null;
        internal DateTime? _loginTimeStampLast = null;
        internal string _loginIdentifierCurrent = string.Empty;
        internal string _loginIdentifierLast = string.Empty;
        internal Guid? _securityKey = null;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _userEmail = string.Empty;

        public string UserEmail {
            get
            {
                return _userEmail;
            }
            set
            {
                _userEmail = value;
                PropertyHasChanged("UserEmail");
            }
        }


        public int? UserKey
        {
            get
            {
                CanReadProperty("UserKey", true);
                return _userKey;
            }
            set
            {
                _userKey = value;
                PropertyHasChanged("UserKey");
            }
        }

        public string UserID
        {
            get
            {
                return _userID;
            }
            set
            {
                _userID = value;
                PropertyHasChanged("UserID");
            }
        }

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                PropertyHasChanged("UserName");
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                PropertyHasChanged("Password");
            }
        }

        public bool? ChangePWNL
        {
            get
            {
                CanReadProperty("ChangePWNL", true);
                return _changePWNL;
            }
            set
            {

                _changePWNL = value;
                PropertyHasChanged("ChangePWNL");

            }
        }

        public bool? ChangePW
        {
            get
            {
                return _changePW;
            }
            set
            {

                _changePW = value;
                PropertyHasChanged("ChangePW");

            }
        }

        public bool? AccDisabled
        {
            get
            {
                CanReadProperty("AccDisabled", true);
                return _accDisabled;
            }
            set
            {

                _accDisabled = value;
                PropertyHasChanged("AccDisabled");

            }
        }

        public bool? AccLockOut
        {
            get
            {
                CanReadProperty("AccLockOut", true);
                return _accLockOut;
            }
            set
            {
                _accLockOut = value;
                PropertyHasChanged("AccLockOut");
            }
        }

        public DateTime? AccLockOutTimeStamp
        {
            get
            {
                CanReadProperty("AccLockOutTimeStamp", true);
                return _accLockOutTimeStamp;
            }
        }

        public int? AccessLevel
        {
            get
            {
                return _accessLevel;
            }
            set
            {

                _accessLevel = value;
                PropertyHasChanged("AccessLevel");

            }
        }

        public int? BranchKey
        {
            get
            {
                return _branchKey;
            }
            set
            {

                _branchKey = value;
                PropertyHasChanged("BranchKey");

            }
        }

        public int? DeptKey
        {
            get
            {
                return _deptKey;
            }
            set
            {

                _deptKey = value;
                PropertyHasChanged("DeptKey");

            }
        }

        public int? TranGrpKey
        {
            get
            {
                return _tranGrpKey;
            }
            set
            {

                _tranGrpKey = value;
                PropertyHasChanged("TranGrpKey");

            }
        }

        public int? EMKey
        {
            get
            {
                return _emKey;
            }
            set
            {

                _emKey = value;
                PropertyHasChanged("EMKey");

            }
        }

        public int? ConAccessLevel
        {
            get
            {
                return _conAccessLevel;
            }
            set
            {

                _conAccessLevel = value;
                PropertyHasChanged("ConAccessLevel");

            }
        }

        public int? ConAccessGrp
        {
            get
            {
                return _conAccessGrp;
            }
            set
            {

                _conAccessGrp = value;
                PropertyHasChanged("ConAccessGrp");

            }
        }

        public int? ItmAccessLevel
        {
            get
            {
                return _itmAccessLevel;
            }
            set
            {

                _itmAccessLevel = value;
                PropertyHasChanged("ItmAccessLevel");

            }
        }

        public int? ItmAccessGrp
        {
            get
            {
                return _itmAccessGrp;
            }
            set
            {

                _itmAccessGrp = value;
                PropertyHasChanged("ItmAccessGrp");

            }
        }

        public int? JobAccessLevel
        {
            get
            {
                return _jobAccessLevel;
            }
            set
            {

                _jobAccessLevel = value;
                PropertyHasChanged("JobAccessLevel");

            }
        }

        public int? JobAccessGrp
        {
            get
            {
                return _jobAccessGrp;
            }
            set
            {

                _jobAccessGrp = value;
                PropertyHasChanged("JobAccessGrp");

            }
        }

        public bool? BuiltIn
        {
            get
            {
                CanReadProperty("BuiltIn", true);
                return _builtIn;
            }
        }

        public short? LoginRetries
        {
            get
            {
                CanReadProperty("LoginRetries", true);
                return _loginRetries;
            }
        }

        public DateTime? LoginTimeStampCurrent
        {
            get
            {
                CanReadProperty("LoginTimeStampCurrent", true);
                return _loginTimeStampCurrent;
            }
        }

        public DateTime? LoginTimeStampLast
        {
            get
            {
                CanReadProperty("LoginTimeStampLast", true);
                return _loginTimeStampLast;
            }
        }

        public string LoginIdentifierCurrent
        {
            get
            {
                CanReadProperty("LoginIdentifierCurrent", true);
                return _loginIdentifierCurrent;
            }
        }

        public string LoginIdentifierLast
        {
            get
            {
                CanReadProperty("LoginIdentifierLast", true);
                return _loginIdentifierLast;
            }
        }

        public System.Guid? SecurityKey
        {
            get
            {
                CanReadProperty("SecurityKey", true);
                return _securityKey;
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
            return _userKey.ToString();
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
            // UserID
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "UserID");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("UserID", 50));
            //
            // UserName
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "UserName");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("UserName", 50));
            //
            // Password
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Password", 50));
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

        public SECUser()
        { /* require use of factory method */ }

        internal static SECUser New()
        {

            SECUser child = new SECUser();

            return child;
        }

        internal static SECUser NewChild()
        {
            SECUser child = new SECUser();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static SECUser Get(SafeDataReader dr)
        {
            SECUser child = new SECUser();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static SECUser Get(int? userKey)
        {
            SECUser child = new SECUser();
            child.Fetch(new Criteria(userKey, 1));
            return child;
        }

        public static SECUser Get(string userID)
        {
            SECUser child = new SECUser();
            child.Fetch(new Criteria(userID, 2));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _userKey = null;
            public int? _option = null;
            public string _userID = null;

            internal Criteria()
            {
            }

            internal Criteria(int? UserKey)
            {
                _userKey = UserKey;
            }

            internal Criteria(int? UserKey, int? Option)
            {
                _userKey = UserKey;
                _option = Option;
            }

            internal Criteria(string UserID, int? Option)
            {
                _userID = UserID;
                _option = Option;
            }
            internal Criteria(int? UserKey, string UserID)
            {
                _userKey = UserKey;
                _userID = UserID;
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
                cm.CommandText = "SECUser_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                if (criteria._userKey != null)
                    cm.Parameters.AddWithValue("@UserKey", criteria._userKey);
                else
                    cm.Parameters.AddWithValue("@UserKey", DBNull.Value);

                if (criteria._userID != null)
                    cm.Parameters.AddWithValue("@UserID", criteria._userID);
                else
                    cm.Parameters.AddWithValue("@UserID", DBNull.Value);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                // Using data reader as record set.
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    // If data reader can read, continue...
                    while (dr.Read())
                    {
                        this.Fetch(dr,  criteria);
                    }
                }	// Already close and dispose data reader.


                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;

            }// Already close and dispose sql connection.            
        }

        internal bool Fetch(SafeDataReader dr, Criteria criteria = null)
        {
            _userKey = dr.GetInt32("UserKey");
            _userID = dr.GetString("UserID");
            _userName = dr.GetString("UserName");
            _password = dr.GetString("Password");
            _userEmail = dr.GetString("UserEmail") == "" ? dr.GetString("OpEmail") : dr.GetString("UserEmail");
            _changePWNL = dr.GetBoolean("ChangePWNL");
            _changePW = dr.GetBoolean("ChangePW");
            _accDisabled = dr.GetBoolean("AccDisabled");
            _accLockOut = dr.GetBoolean("AccLockOut");

            if (dr.GetValue("AccLockOutTimeStamp") != null)
                _accLockOutTimeStamp = dr.GetDateTime("AccLockOutTimeStamp");

            _accessLevel = dr.GetInt32("AccessLevel");
            _branchKey = dr.GetInt32("BranchKey");
            _deptKey = dr.GetInt32("DeptKey");
            _tranGrpKey = dr.GetInt32("TranGrpKey");
            if (_tranGrpKey == 0)
                _tranGrpKey = null;
            _emKey = GFunc.NEInt(dr.GetInt32("EMKey"), 0);
            if (_emKey == 0)
                _emKey = null;
            _conAccessLevel = dr.GetInt32("ConAccessLevel");
            _conAccessGrp = dr.GetInt32("ConAccessGrp");
            _itmAccessLevel = dr.GetInt32("ItmAccessLevel");
            _itmAccessGrp = dr.GetInt32("ItmAccessGrp");
            _jobAccessLevel = dr.GetInt32("JobAccessLevel");
            _jobAccessGrp = dr.GetInt32("JobAccessGrp");
            _builtIn = dr.GetBoolean("BuiltIn");
            _loginRetries = dr.GetInt16("LoginRetries");

            if (dr.GetValue("LoginTimeStampCurrent") != null)
                _loginTimeStampCurrent = dr.GetDateTime("LoginTimeStampCurrent");

            if (dr.GetValue("LoginTimeStampLast") != null)
                _loginTimeStampLast = dr.GetDateTime("LoginTimeStampLast");

            _loginIdentifierCurrent = dr.GetString("LoginIdentifierCurrent");
            _loginIdentifierLast = dr.GetString("LoginIdentifierLast");
            _securityKey = dr.GetGuid("SecurityKey");

            if (dr.GetValue("CreateDate") != null)
                _createDate = dr.GetDateTime("CreateDate");

            _createUserKey = dr.GetInt32("CreateUserKey");

            if (dr.GetValue("LastModifiedDate") != null)
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

        internal bool Insert(out int newUserKey)
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
                    retValue = this.Insert(cn, out newUserKey);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope                

            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int newUserKey)
        {
            newUserKey = 0;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "[SECUser_AddUpdate]";

                cm.Parameters.AddWithValue("@Option", 0);


                cm.Parameters.AddWithValue("@NewUserKey", newUserKey);

                if (_userKey == null)
                    cm.Parameters.AddWithValue("@UserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserKey", _userKey);

                if (_userID == null)
                    cm.Parameters.AddWithValue("@UserID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserID", _userID);

                if (_userName == null)
                    cm.Parameters.AddWithValue("@UserName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserName", _userName);

                if (_password == null)
                    cm.Parameters.AddWithValue("@Password", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Password", _password);

                if (_userEmail == null)
                    cm.Parameters.AddWithValue("@UserEmail", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserEmail", _userEmail);

                if (_changePWNL == null)
                    cm.Parameters.AddWithValue("@ChangePWNL", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ChangePWNL", _changePWNL);

                if (_changePW == null)
                    cm.Parameters.AddWithValue("@ChangePW", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ChangePW", _changePW);

                if (_accDisabled == null)
                    cm.Parameters.AddWithValue("@AccDisabled", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccDisabled", _accDisabled);

                if (_accLockOut == null)
                    cm.Parameters.AddWithValue("@AccLockOut", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccLockOut", _accLockOut);

                if (_accLockOutTimeStamp == null)
                    cm.Parameters.AddWithValue("@AccLockOutTimeStamp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccLockOutTimeStamp", _accLockOutTimeStamp.Value);

                if (_accessLevel == null)
                    cm.Parameters.AddWithValue("@AccessLevel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessLevel", _accessLevel);

                if (_branchKey == null)
                    cm.Parameters.AddWithValue("@BranchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BranchKey", _branchKey);

                if (_deptKey == null)
                    cm.Parameters.AddWithValue("@DeptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DeptKey", _deptKey);

                if (_tranGrpKey == null)
                    cm.Parameters.AddWithValue("@TranGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TranGrpKey", _tranGrpKey);

                if (_emKey == null)
                    cm.Parameters.AddWithValue("@EMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EMKey", _emKey);

                if (_conAccessLevel == null)
                    cm.Parameters.AddWithValue("@ConAccessLevel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConAccessLevel", _conAccessLevel);

                if (_conAccessGrp == null)
                    cm.Parameters.AddWithValue("@ConAccessGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConAccessGrp", _conAccessGrp);

                if (_itmAccessLevel == null)
                    cm.Parameters.AddWithValue("@ItmAccessLevel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmAccessLevel", _itmAccessLevel);

                if (_itmAccessGrp == null)
                    cm.Parameters.AddWithValue("@ItmAccessGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmAccessGrp", _itmAccessGrp);

                if (_jobAccessLevel == null)
                    cm.Parameters.AddWithValue("@JobAccessLevel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobAccessLevel", _jobAccessLevel);

                if (_jobAccessGrp == null)
                    cm.Parameters.AddWithValue("@JobAccessGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobAccessGrp", _jobAccessGrp);

                if (_builtIn == null)
                    cm.Parameters.AddWithValue("@BuiltIn", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BuiltIn", _builtIn);

                if (_loginRetries == null)
                    cm.Parameters.AddWithValue("@LoginRetries", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LoginRetries", _loginRetries);

                if (_loginTimeStampCurrent == null)
                    cm.Parameters.AddWithValue("@LoginTimeStampCurrent", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LoginTimeStampCurrent", _loginTimeStampCurrent.Value);

                if (_loginTimeStampLast == null)
                    cm.Parameters.AddWithValue("@LoginTimeStampLast", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LoginTimeStampLast", _loginTimeStampLast.Value);

                if (_loginIdentifierCurrent == null)
                    cm.Parameters.AddWithValue("@LoginIdentifierCurrent", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LoginIdentifierCurrent", _loginIdentifierCurrent);

                if (_loginIdentifierLast == null)
                    cm.Parameters.AddWithValue("@LoginIdentifierLast", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LoginIdentifierLast", _loginIdentifierLast);

                if (_securityKey == null || _securityKey == System.Guid.Empty)
                    cm.Parameters.AddWithValue("@SecurityKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SecurityKey", _securityKey);

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
                cm.Parameters["@NewUserKey"].Direction = ParameterDirection.Output;


                cm.ExecuteNonQuery();


                newUserKey = (int)cm.Parameters["@NewUserKey"].Value;

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
                cm.CommandText = "SECUser_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);

                cm.Parameters.AddWithValue("@NewUserKey", 0);

                if (_userKey == null)
                    cm.Parameters.AddWithValue("@UserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserKey", _userKey);

                if (_userID == null)
                    cm.Parameters.AddWithValue("@UserID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserID", _userID);

                if (_userName == null)
                    cm.Parameters.AddWithValue("@UserName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserName", _userName);

                if (_password == null)
                    cm.Parameters.AddWithValue("@Password", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Password", _password);

                if (_userEmail == null)
                    cm.Parameters.AddWithValue("@UserEmail", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserEmail", _userEmail);


                if (_changePWNL == null)
                    cm.Parameters.AddWithValue("@ChangePWNL", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ChangePWNL", _changePWNL);

                if (_changePW == null)
                    cm.Parameters.AddWithValue("@ChangePW", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ChangePW", _changePW);

                if (_accDisabled == null)
                    cm.Parameters.AddWithValue("@AccDisabled", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccDisabled", _accDisabled);

                if (_accLockOut == null)
                    cm.Parameters.AddWithValue("@AccLockOut", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccLockOut", _accLockOut);

                if (_accLockOutTimeStamp == null)
                    cm.Parameters.AddWithValue("@AccLockOutTimeStamp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccLockOutTimeStamp", _accLockOutTimeStamp.Value);

                if (_accessLevel == null)
                    cm.Parameters.AddWithValue("@AccessLevel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessLevel", _accessLevel);

                if (_branchKey == null)
                    cm.Parameters.AddWithValue("@BranchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BranchKey", _branchKey);

                if (_deptKey == null)
                    cm.Parameters.AddWithValue("@DeptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DeptKey", _deptKey);

                if (_tranGrpKey == null)
                    cm.Parameters.AddWithValue("@TranGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TranGrpKey", _tranGrpKey);

                if (_emKey == null)
                    cm.Parameters.AddWithValue("@EMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EMKey", _emKey);

                if (_conAccessLevel == null)
                    cm.Parameters.AddWithValue("@ConAccessLevel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConAccessLevel", _conAccessLevel);

                if (_conAccessGrp == null)
                    cm.Parameters.AddWithValue("@ConAccessGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConAccessGrp", _conAccessGrp);

                if (_itmAccessLevel == null)
                    cm.Parameters.AddWithValue("@ItmAccessLevel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmAccessLevel", _itmAccessLevel);

                if (_itmAccessGrp == null)
                    cm.Parameters.AddWithValue("@ItmAccessGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmAccessGrp", _itmAccessGrp);

                if (_jobAccessLevel == null)
                    cm.Parameters.AddWithValue("@JobAccessLevel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobAccessLevel", _jobAccessLevel);

                if (_jobAccessGrp == null)
                    cm.Parameters.AddWithValue("@JobAccessGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobAccessGrp", _jobAccessGrp);

                if (_builtIn == null)
                    cm.Parameters.AddWithValue("@BuiltIn", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BuiltIn", _builtIn);

                if (_loginRetries == null)
                    cm.Parameters.AddWithValue("@LoginRetries", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LoginRetries", _loginRetries);

                if (_loginTimeStampCurrent == null)
                    cm.Parameters.AddWithValue("@LoginTimeStampCurrent", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LoginTimeStampCurrent", _loginTimeStampCurrent.Value);

                if (_loginTimeStampLast == null)
                    cm.Parameters.AddWithValue("@LoginTimeStampLast", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LoginTimeStampLast", _loginTimeStampLast.Value);

                if (_loginIdentifierCurrent == null)
                    cm.Parameters.AddWithValue("@LoginIdentifierCurrent", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LoginIdentifierCurrent", _loginIdentifierCurrent);

                if (_loginIdentifierLast == null)
                    cm.Parameters.AddWithValue("@LoginIdentifierLast", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LoginIdentifierLast", _loginIdentifierLast);

                if (_securityKey == null || _securityKey == Guid.Empty)
                    cm.Parameters.AddWithValue("@SecurityKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SecurityKey", _securityKey);

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
            string msgID = MsgID.Common.DeleteFail;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECUser_Delete";
                cm.Parameters.AddWithValue("@UserKey", criteria._userKey);

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
                cm.CommandText = "SECUser_Validation";

                cm.Parameters.AddWithValue("@isNew", isNew);
                cm.Parameters.AddWithValue("@UserKey", criteria._userKey);
                cm.Parameters.AddWithValue("@UserID", criteria._userID);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }

        }

        internal bool CheckUserID(string UserID)
        {
            bool retValue = false;
            string msgID = MsgID.Common.GetFail;
           
                // Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();
                retValue = this.CheckUserID(cn, UserID);
            }
              

            return retValue;

        }
        internal bool CheckUserID(SqlConnection cn, string UserID)
        {
            string msgID = MsgID.Common.GetFail;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECUser_IDCheck";

                if (UserID != null)
                    cm.Parameters.AddWithValue("@UserID", UserID);
                else
                    cm.Parameters.AddWithValue("@UserID", DBNull.Value);

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
        #endregion //Data Access - Validation

        #region Data Access - Custom Update

        public bool CustomUpdate(int? option)
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
                    retValue = this.CustomUpdate(cn, option);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool CustomUpdate(SqlConnection cn, int? option)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECUser_CustomUpdate";

                cm.Parameters.AddWithValue("@Option", option);

                if (!BOLib.GFunc.IsNE(AppInfor.currentUserKey))
                    _lastModifiedUserKey = AppInfor.currentUserKey;

                if (_userKey == null)
                    cm.Parameters.AddWithValue("@UserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserKey", _userKey);

                if (_userID == null)
                    cm.Parameters.AddWithValue("@UserID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserID", _userID);

                if (_userName == null)
                    cm.Parameters.AddWithValue("@UserName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserName", _userName);

                if (_password == null)
                    cm.Parameters.AddWithValue("@Password", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Password", _password);

                if (_userEmail == null)
                    cm.Parameters.AddWithValue("@UserEmail", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserEmail", _userEmail);

                if (_changePWNL == null)
                    cm.Parameters.AddWithValue("@ChangePWNL", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ChangePWNL", _changePWNL);

                if (_changePW == null)
                    cm.Parameters.AddWithValue("@ChangePW", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ChangePW", _changePW);

                if (_accDisabled == null)
                    cm.Parameters.AddWithValue("@AccDisabled", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccDisabled", _accDisabled);

                if (_accLockOut == null)
                    cm.Parameters.AddWithValue("@AccLockOut", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccLockOut", _accLockOut);

                if (_accLockOutTimeStamp == null)
                    cm.Parameters.AddWithValue("@AccLockOutTimeStamp", DBNull.Value);
                else if (_accLockOutTimeStamp.Value.Year == 1)
                    cm.Parameters.AddWithValue("@AccLockOutTimeStamp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccLockOutTimeStamp", _accLockOutTimeStamp.Value);

                if (_accessLevel == null)
                    cm.Parameters.AddWithValue("@AccessLevel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessLevel", _accessLevel);

                if (_branchKey == null)
                    cm.Parameters.AddWithValue("@BranchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BranchKey", _branchKey);

                if (_deptKey == null)
                    cm.Parameters.AddWithValue("@DeptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DeptKey", _deptKey);

                if (_builtIn == null)
                    cm.Parameters.AddWithValue("@BuiltIn", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BuiltIn", _builtIn);

                if (_loginRetries == null)
                    cm.Parameters.AddWithValue("@LoginRetries", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LoginRetries", _loginRetries);

                if (_loginTimeStampCurrent == null)
                    cm.Parameters.AddWithValue("@LoginTimeStampCurrent", DBNull.Value);
                else if (_loginTimeStampCurrent.Value.Year == 1)
                    cm.Parameters.AddWithValue("@LoginTimeStampCurrent", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LoginTimeStampCurrent", _loginTimeStampCurrent.Value);

                if (_loginTimeStampLast == null)
                    cm.Parameters.AddWithValue("@LoginTimeStampLast", DBNull.Value);
                else if (_loginTimeStampLast.Value.Year == 1)
                    cm.Parameters.AddWithValue("@LoginTimeStampLast", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LoginTimeStampLast", _loginTimeStampLast.Value);

                if (_loginIdentifierCurrent == null)
                    cm.Parameters.AddWithValue("@LoginIdentifierCurrent", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LoginIdentifierCurrent", _loginIdentifierCurrent);

                if (_loginIdentifierLast == null)
                    cm.Parameters.AddWithValue("@LoginIdentifierLast", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LoginIdentifierLast", _loginIdentifierLast);

                if (_securityKey == null)
                    cm.Parameters.AddWithValue("@SecurityKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SecurityKey", _securityKey);

                if (_createDate == null)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else if (_createDate.Value.Year == 1)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

                if (AppInfor.currentUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);

                if (_lastModifiedDate == null)
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                else if (_lastModifiedDate.Value.Year == 1)
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
                    cm.Parameters.AddWithValue("@Custom1", AppInfor.ProgramVersion);

                if (_custom2 == null)
                    cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom2", AppInfor.CultureInfo);

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
        #endregion //Data Access - Custom Update

        // added by KKAung on 05 Aug 2022
        internal static DataSet GetPasswordHistory(Criteria criteria) 
        {            

            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "SECPasswordHistory_Get";                                

                    cm.Parameters.AddWithValue("@Option", criteria._option);
                    cm.Parameters.AddWithValue("@UserID", criteria._userID);
                    cm.Parameters.AddWithValue("@UserKey", criteria._userKey);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    SqlDataAdapter adap = new SqlDataAdapter(cm);
                    DataSet dtPwdList = new DataSet();

                    adap.Fill(dtPwdList);

                    return dtPwdList;
                }

            }           

        } // End


        // added by KKAung on 07 Jun 2023 (start)
        public bool AddPasswordHistory(int Option, string UserID, int UserKey)
        {
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                cn.Open();
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "SECPasswordHistory_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", Option);
                    cm.Parameters.AddWithValue("@UserID", UserID);
                    cm.Parameters.AddWithValue("@UserKey", UserKey);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    cm.ExecuteNonQuery();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        return true;
                    else
                        return false;
                }
            }

        } // End

        // Update Password
        public bool UpdatePassword(string _conStr)
        {
            bool retValue = false;
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(_conStr))
                {
                    // Open Connection  
                    cn.Open();

                    using (SqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "SECUser_Forgot_Password";

                        cm.Parameters.AddWithValue("@UserID", UserID);
                        cm.Parameters.AddWithValue("@PlainPassword", Password);
                        cm.Parameters.AddWithValue("@Password", TAUtil.Encoder.Encode(UserKey.ToString() + Password));
                        cm.Parameters.AddWithValue("@RetValue", 0);
                        cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                        cm.ExecuteNonQuery();

                        retValue = (int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass;

                    }


                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }
        


    }
}
