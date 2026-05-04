using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;


namespace BOLib
{
    [Serializable()]
    public class REFWorkOrderType : Csla.BusinessBase<REFWorkOrderType>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _typeKey = 0;
        internal string _typeID = string.Empty;
        internal string _typeDes = string.Empty;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;

        public int? TypeKey
        {
            get
            {
                return _typeKey;
            }
        }

        public string TypeID
        {
            get
            {
                return _typeID;
            }
            set
            {
                _typeID = value;
                PropertyHasChanged("TypeID");
            }
        }

        public string TypeDes
        {
            get
            {
                return _typeDes;
            }
            set
            {
                _typeDes = value;
                PropertyHasChanged("TypeDes");
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
            return _typeKey.ToString();
        }

        #endregion //Business Properties and Methods

        #region Factory Methods

        internal REFWorkOrderType()
        { /* require use of factory method */ }

        internal static REFWorkOrderType New()
        {
            REFWorkOrderType child = new REFWorkOrderType();         
            return child;
        }

        internal static REFWorkOrderType NewChild()
        {
            REFWorkOrderType child = new REFWorkOrderType();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static REFWorkOrderType Get(SafeDataReader dr)
        {
            REFWorkOrderType child = new REFWorkOrderType();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static REFWorkOrderType Get(int? typeKey)
        {
            REFWorkOrderType child = new REFWorkOrderType();
            child.Fetch(new Criteria(typeKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _typeKey = null;
            public string _typeID = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? TypeKey)
            {
                _typeKey = TypeKey;
            }

            internal Criteria(int? TypeKey, int? Option)
            {
                _typeKey = TypeKey;
                _option = Option;
            }

            internal Criteria(int? TypeKey, string KeyID)
            {
                _typeKey = TypeKey;
                _typeID = KeyID;
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
                cm.CommandText = "REFWorkOrderType_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@TypeKey", criteria._typeKey);


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
            _typeKey = dr.GetInt32("TypeKey");
            _typeID = dr.GetString("TypeID");
            _typeDes = dr.GetString("TypeDes");
            _createDate = dr.GetDateTime("CreateDate");
            _createUserKey = dr.GetInt32("CreateUserKey");
            _lastModifiedDate = dr.GetDateTime("LastModifiedDate");
            _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            ValidationRules.CheckRules();
            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? typeKey)
        {
            bool retValue = false;
            typeKey = null;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out typeKey);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope               

            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? typeKey)
        {
            bool retValue = false;
            typeKey = 0;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFWorkOrderType_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@NewTypeKey", typeKey);

                if (_typeKey == null)
                    cm.Parameters.AddWithValue("@TypeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TypeKey", _typeKey);

                if (_typeID == null)
                    cm.Parameters.AddWithValue("@TypeID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TypeID", _typeID);

                if (_typeDes == null)
                    cm.Parameters.AddWithValue("@TypeDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TypeDes", _typeDes);

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

                cm.Parameters["@NewTypeKey"].Direction = ParameterDirection.InputOutput;

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                typeKey = (int)cm.Parameters["@NewTypeKey"].Value;

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.            

            return retValue;
        }

        //internal bool Insert(SqlConnection cn, out int? typeKey)
        //{
        //    bool retValue = false;
        //    typeKey = 0;
        //    // Using existing sql connection.
        //    using (SqlCommand cm = cn.CreateCommand())
        //    {

        //        cm.CommandType = CommandType.StoredProcedure;
        //        cm.CommandText = "REFWorkOrderType_AddUpdate";

        //        cm.Parameters.AddWithValue("@Option", 0);

        //        cm.Parameters.AddWithValue("@NewTypeKey", typeKey);

        //        if (_typeKey == null)
        //            cm.Parameters.AddWithValue("@TypeKey", DBNull.Value);
        //        else
        //            cm.Parameters.AddWithValue("@TypeKey", _typeKey);

        //        if (_typeID == null)
        //            cm.Parameters.AddWithValue("@TypeID", DBNull.Value);
        //        else
        //            cm.Parameters.AddWithValue("@TypeID", _typeID);

        //        if (_typeDes == null)
        //            cm.Parameters.AddWithValue("@TypeDes", DBNull.Value);
        //        else
        //            cm.Parameters.AddWithValue("@TypeDes", _typeDes);

        //        if (_createDate == null)
        //            cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
        //        else
        //            cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

        //        if (AppInfor.currentUserKey == null)
        //            cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
        //        else
        //            cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);

        //        if (_lastModifiedDate == null)
        //            cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
        //        else
        //            cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

        //        if (_lastModifiedUserKey == null)
        //            cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
        //        else
        //            cm.Parameters.AddWithValue("@LastModifiedUserKey", _lastModifiedUserKey);

        //        cm.Parameters["@NewTypeKey"].Direction = ParameterDirection.InputOutput;

        //        cm.Parameters.AddWithValue("@RetValue", 0);
        //        cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

        //        cm.ExecuteNonQuery();

        //        typeKey = (int)cm.Parameters["@NewTypeKey"].Value;

        //        if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
        //            retValue = true;
        //        else
        //            retValue = false;

        //    }// Already close and dispose sql connection.            

        //    return retValue;
        //}

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
                cm.CommandText = "REFWorkOrderType_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewTypeKey", 0);
                if (_typeKey == null)
                    cm.Parameters.AddWithValue("@TypeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TypeKey", _typeKey);

                if (_typeID == null)
                    cm.Parameters.AddWithValue("@TypeID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TypeID", _typeID);

                if (_typeDes == null)
                    cm.Parameters.AddWithValue("@TypeDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TypeDes", _typeDes);

                if (_createDate == null)
                {
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                }
                else
                {
                    
                    if(this.IsValidSQLDateTime(_createDate.Value))
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
                cm.Parameters["@NewTypeKey"].Direction = ParameterDirection.InputOutput;

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
                cm.CommandText = "REFWorkOrderType_Delete";

                cm.Parameters.AddWithValue("@TypeKey", criteria._typeKey);

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
                cm.CommandText = "REFWorkOrderType_Validation";


                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@TypeKey", criteria._typeKey);
                cm.Parameters.AddWithValue("@TypeID", criteria._typeID);
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
            _typeKey = 0;
            _typeID = string.Empty;
            _typeDes = string.Empty;
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
