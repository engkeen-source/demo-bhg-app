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
    public class SYSApp : Csla.BusinessBase<SYSApp>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _appKey = 0;
        internal string _appID = string.Empty;
        internal string _appDes = string.Empty;
        internal string _appObjType = string.Empty;
        internal string _appObj = string.Empty;
        internal bool? _buildIn = false;
        internal int? _userKey = 0;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _custom4 = string.Empty;
        internal string _custom5 = string.Empty;

        public int? AppKey
        {
            get
            {
                return _appKey;
            }
        }

        public string AppID
        {
            get
            {
                return _appID;
            }
            set
            {
                _appID = value;
                PropertyHasChanged("AppID");
            }
        }

        public string AppDes
        {
            get
            {
                return _appDes;
            }
            set
            {
                _appDes = value;
                PropertyHasChanged("AppDes");
            }
        }

        public string AppObjType
        {
            get
            {
                return _appObjType;
            }
        }

        public string AppObj
        {
            get
            {
                return _appObj;
            }
        }

        public bool? BuildIn
        {
            get
            {
                return _buildIn;
            }
        }

        public int? UserKey
        {
            get
            {
                return _userKey;
            }
            set
            {
                _userKey = value;
                PropertyHasChanged("UserKey");
            }
        }

        public DateTime? CreateDate
        {
            get
            {
                return _createDate;
            }
        }

        public int? CreateUserKey
        {
            get
            {
                return _createUserKey;
            }
        }

        public DateTime? LastModifiedDate
        {
            get
            {
                return _lastModifiedDate;
            }
        }

        public int? LastModifiedUserKey
        {
            get
            {
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

        public string Custom4
        {
            get
            {

                return _custom4;
            }
            set
            {
                _custom4 = value;
                PropertyHasChanged("Custom4");
            }
        }

        public string Custom5
        {
            get
            {

                return _custom5;
            }
            set
            {
                _custom5 = value;
                PropertyHasChanged("Custom5");
            }
        }

        protected override object GetIdValue()
        {
            return _appKey.ToString();
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
            //// AppID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "AppID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("AppID", 50));
            ////
            //// AppDes
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "AppDes");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("AppDes", 255));
            ////
            //// AppObjType
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "AppObjType");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("AppObjType", 50));
            ////
            //// AppObj
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "AppObj");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("AppObj", 50));
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
            ////
            //// Custom4
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom4", 255));
            ////
            //// Custom5
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom5", 255));
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal SYSApp()
        { /* require use of factory method */ }

        internal static SYSApp New()
        {
            
            SYSApp child = new SYSApp();
            
            return child;
        }

        internal static SYSApp NewChild()
        {
            
            SYSApp child = new SYSApp();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static SYSApp Get(SafeDataReader dr)
        {

            SYSApp child = new SYSApp();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SYSApp Get(int? appKey)
        {

            SYSApp child = new SYSApp();
            child.Fetch(new Criteria(appKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _appKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? AppKey)
            {
                _appKey = AppKey;
            }

            internal Criteria(int? AppKey, int? Option)
            {
                _appKey = AppKey;
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
                cm.CommandText = "SYSApp_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@AppKey", criteria._appKey);

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

            _appKey = dr.GetInt32("AppKey");
            _appID = dr.GetString("AppID");
            _appDes = dr.GetString("AppDes");
            _appObjType = dr.GetString("AppObjType");
            _appObj = dr.GetString("AppObj");
            _buildIn = dr.GetBoolean("BuildIn");
            _userKey = dr.GetInt32("UserKey");
            _createDate = dr.GetDateTime("CreateDate");
            _createUserKey = dr.GetInt32("CreateUserKey");
            _lastModifiedDate = dr.GetDateTime("LastModifiedDate");
            _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            _custom4 = dr.GetString("Custom4");
            _custom5 = dr.GetString("Custom5");
            ValidationRules.CheckRules();
            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? appKey)
        {
            bool retValue = false;

            appKey = null;
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out appKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? appKey)
        {
            
            appKey = 0;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSApp_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);
                

                cm.Parameters.AddWithValue("@NewAppKey", appKey);

                if (_appKey == null)
                    cm.Parameters.AddWithValue("@AppKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AppKey", _appKey);

                if (_appID == null)
                    cm.Parameters.AddWithValue("@AppID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AppID", _appID);

                if (_appDes == null)
                    cm.Parameters.AddWithValue("@AppDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AppDes", _appDes);

                if (_appObjType == null)
                    cm.Parameters.AddWithValue("@AppObjType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AppObjType", _appObjType);

                if (_appObj == null)
                    cm.Parameters.AddWithValue("@AppObj", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AppObj", _appObj);

                if (_buildIn == null)
                    cm.Parameters.AddWithValue("@BuildIn", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BuildIn", _buildIn);

                if (_userKey == null)
                    cm.Parameters.AddWithValue("@UserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserKey", _userKey);

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

                if (_custom4 == null)
                    cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom4", _custom4);

                if (_custom5 == null)
                    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom5", _custom5);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                
                cm.Parameters["@NewAppKey"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                

                appKey = (int)cm.Parameters["@NewAppKey"].Value;

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
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            return true;
        }

        internal bool Update(SqlConnection cn)
        {
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSApp_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                
                cm.Parameters.AddWithValue("@NewAppKey", 0);

                if (_appKey == null)
                    cm.Parameters.AddWithValue("@AppKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AppKey", _appKey);

                if (_appID == null)
                    cm.Parameters.AddWithValue("@AppID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AppID", _appID);

                if (_appDes == null)
                    cm.Parameters.AddWithValue("@AppDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AppDes", _appDes);

                if (_appObjType == null)
                    cm.Parameters.AddWithValue("@AppObjType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AppObjType", _appObjType);

                if (_appObj == null)
                    cm.Parameters.AddWithValue("@AppObj", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AppObj", _appObj);

                if (_buildIn == null)
                    cm.Parameters.AddWithValue("@BuildIn", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BuildIn", _buildIn);

                if (_userKey == null)
                    cm.Parameters.AddWithValue("@UserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserKey", _userKey);

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

                if (_custom4 == null)
                    cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom4", _custom4);

                if (_custom5 == null)
                    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom5", _custom5);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                
                cm.Parameters["@NewAppKey"].Direction = ParameterDirection.Output;

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
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSApp_Delete";

                
                cm.Parameters.AddWithValue("@AppKey", criteria._appKey);

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
            string msgID = MsgID.Common.ValidationFail;
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
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
        {
            string msgID = MsgID.Common.ValidationFail;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSApp_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@AppKey", criteria._appKey);

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