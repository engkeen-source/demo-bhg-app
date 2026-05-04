using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class REFWorkOrderRequestType : Csla.BusinessBase<REFWorkOrderRequestType>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _reqTypeKey = 0;
        internal string _reqTypeID = string.Empty;
        internal string _reqTypeDes = string.Empty;
        internal int? _defItemKey = null;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;

        public int? ReqTypeKey
        {
            get
            {
                return _reqTypeKey;
            }
        }

        public string ReqTypeID
        {
            get
            {
                return _reqTypeID;
            }
            set
            {
                _reqTypeID = value;
                PropertyHasChanged("ReqTypeID");
            }
        }

        public string ReqTypeDes
        {
            get
            {
                return _reqTypeDes;
            }
            set
            {
                _reqTypeDes = value;
                PropertyHasChanged("ReqTypeDes");
            }
        }

        public int? DefItemKey
        {
            get
            {
                return _defItemKey;
            }
            set
            {
                _defItemKey = value;
                PropertyHasChanged("DefItemKey");
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

        protected override object GetIdValue()
        {
            return _reqTypeKey.ToString();
        }

        #endregion //Business Properties and Methods

        #region Factory Methods

        internal REFWorkOrderRequestType()
        { /* require use of factory method */ }

        internal static REFWorkOrderRequestType New()
        {
            REFWorkOrderRequestType child = new REFWorkOrderRequestType();         
            return child;
        }

        internal static REFWorkOrderRequestType NewChild()
        {
            REFWorkOrderRequestType child = new REFWorkOrderRequestType();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static REFWorkOrderRequestType Get(SafeDataReader dr)
        {
            REFWorkOrderRequestType child = new REFWorkOrderRequestType();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static REFWorkOrderRequestType Get(int? reqTypeKey)
        {
            REFWorkOrderRequestType child = new REFWorkOrderRequestType();
            child.Fetch(new Criteria(reqTypeKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _reqTypeKey = null;
            public string _reqTypeID = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? ReqTypeKey)
            {
                _reqTypeKey = ReqTypeKey;
            }

            internal Criteria(int? ReqTypeKey, int? Option)
            {
                _reqTypeKey = ReqTypeKey;
                _option = Option;
            }

            internal Criteria(int? ReqTypeKey, string KeyID)
            {
                _reqTypeKey = ReqTypeKey;
                _reqTypeID = KeyID;
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
                cm.CommandText = "REFWorkOrderRequestType_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@ReqTypeKey", criteria._reqTypeKey);


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

                }// Already close and dispose data reader.



                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.                       

            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _reqTypeKey = dr.GetInt32("ReqTypeKey");
            _reqTypeID = dr.GetString("ReqTypeID");
            _reqTypeDes = dr.GetString("ReqTypeDes");
            _defItemKey = dr.GetInt32("DefItemKey");
            _createDate = dr.GetDateTime("CreateDate");
            _createUserKey = dr.GetInt32("CreateUserKey");
            _lastModifiedDate = dr.GetDateTime("LastModifiedDate");
            _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            ValidationRules.CheckRules();
            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? reqTypeKey)
        {
            bool retValue = false;
            reqTypeKey = null;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out reqTypeKey);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope               

            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? reqTypeKey)
        {
            bool retValue = false;
            reqTypeKey = 0;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFWorkOrderRequestType_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@NewReqTypeKey", reqTypeKey);

                if (_reqTypeKey == null)
                    cm.Parameters.AddWithValue("@ReqTypeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ReqTypeKey", _reqTypeKey);

                if (_reqTypeID == null)
                    cm.Parameters.AddWithValue("@ReqTypeID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ReqTypeID", _reqTypeID);

                if (_reqTypeDes == null)
                    cm.Parameters.AddWithValue("@ReqTypeDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ReqTypeDes", _reqTypeDes);

                if (_defItemKey == null)
                    cm.Parameters.AddWithValue("@DefItemKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DefItemKey", _defItemKey);

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

                cm.Parameters["@NewReqTypeKey"].Direction = ParameterDirection.InputOutput;

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                reqTypeKey = (int)cm.Parameters["@NewReqTypeKey"].Value;

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

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
                cm.CommandText = "REFWorkOrderRequestType_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewReqTypeKey", 0);
                if (_reqTypeKey == null)
                    cm.Parameters.AddWithValue("@ReqTypeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ReqTypeKey", _reqTypeKey);

                if (_reqTypeID == null)
                    cm.Parameters.AddWithValue("@ReqTypeID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ReqTypeID", _reqTypeID);

                if (_reqTypeDes == null)
                    cm.Parameters.AddWithValue("@ReqTypeDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ReqTypeDes", _reqTypeDes);

                if (_defItemKey == null)
                    cm.Parameters.AddWithValue("@DefItemKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DefItemKey", _defItemKey);

                if (_createDate == null)
                {
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                }
                else
                {
                    if (this.IsValidSQLDateTime(_createDate.Value))
                    {
                        cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);
                    }
                    else
                    {
                        DateTime minDateTime = DateTime.MinValue;
                        minDateTime = new DateTime(1753, 1, 1);

                        cm.Parameters.AddWithValue("@CreateDate", minDateTime);
                    }
                }

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

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewReqTypeKey"].Direction = ParameterDirection.InputOutput;

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
                cm.CommandText = "REFWorkOrderRequestType_Delete";

                cm.Parameters.AddWithValue("@ReqTypeKey", criteria._reqTypeKey);

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
                cm.CommandText = "REFWorkOrderRequestType_Validation";


                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@ReqTypeKey", criteria._reqTypeKey);
                cm.Parameters.AddWithValue("@ReqTypeID", criteria._reqTypeID);
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

        private void Clear()
        {
            _reqTypeKey = 0;
            _reqTypeID = string.Empty;
            _reqTypeDes = string.Empty;
            _defItemKey = null;
            _createDate = null;
            _createUserKey = null;
            _lastModifiedDate = null;
            _lastModifiedUserKey = null;

        }

        internal bool IsValidSQLDateTime(DateTime dDate)
        {
            bool valid = false;

            DateTime minDateTime = DateTime.MinValue;
            DateTime maxDateTime = DateTime.MaxValue;

            minDateTime = new DateTime(1753, 1, 1);
            maxDateTime = new DateTime(9999, 12, 31, 23, 59, 59, 997);

            if (dDate >= minDateTime && dDate <= maxDateTime)
            {
                valid = true;
            }

            return valid;
        }
    }
}
