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
    public class REFContactInfor : Csla.BusinessBase<REFContactInfor>, System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _uid = 0;
        internal int? _contactLinkType = null;
        internal int? _contactLinkKey = null;
        internal string _contactPerson = string.Empty;
        internal int? _contactType = 10;
        internal string _contactNum = string.Empty;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _error = string.Empty;

        public int? UID
        {
            get
            {
                return _uid;
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
        public int? ContactLinkType
        {
            get
            {
                return _contactLinkType;
            }
        }

        public int? ContactLinkKey
        {
            get
            {
                return _contactLinkKey;
            }
        }

        public string ContactPerson
        {
            get
            {
                return _contactPerson;
            }
            set
            {
                _contactPerson = value;
                PropertyHasChanged("ContactPerson");
            }
        }

        public int? ContactType
        {
            get
            {
                return _contactType;
            }
            set
            {
                _contactType = value;
                PropertyHasChanged("ContactType");
            }
        }

        public string ContactNum
        {
            get
            {
                return _contactNum;
            }
            set
            {
                _contactNum = value;
                PropertyHasChanged("ContactNum");
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

        protected override object GetIdValue()
        {
            return _uid.ToString();
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
            //// ContactPerson
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ContactPerson", 255));
            ////
            //// ContactNum
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "ContactNum");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ContactNum", 255));
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

        public REFContactInfor()
        { /* require use of factory method */ }

        internal static REFContactInfor New()
        {

            REFContactInfor child = new REFContactInfor();
            return child;
        }

        internal static REFContactInfor NewChild()
        {

            REFContactInfor child = new REFContactInfor();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static REFContactInfor Get(SafeDataReader dr)
        {
            REFContactInfor child = new REFContactInfor();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static REFContactInfor Get(int? uid)
        {
            REFContactInfor child = new REFContactInfor();
            child.Fetch(new Criteria(uid, 1));
            return child;
        }
        public static REFContactInfor Get(int? conLinkType, int? conLinkKey, string contactNum)
        {
            REFContactInfor child = new REFContactInfor();
            child.Fetch(new Criteria(conLinkType, conLinkKey, contactNum, 3));
            return child;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _uid = null;
            public int? _contactLinkType = null;
            public int? _contactLinkKey = null;
            public string _contactNum = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? Uid)
            {
                _uid = Uid;
            }

            internal Criteria(int? Uid, int? Option)
            {
                _uid = Uid;
                _option = Option;
            }

            internal Criteria(int? Uid, int? ContactLinkType, int? ContactLinkKey, int? Option)
            {
                _uid = Uid;
                _contactLinkType = ContactLinkType;
                _contactLinkKey = ContactLinkKey;
                _option = Option;
            }
            internal Criteria(int? ContactLinkType, int? ContactLinkKey, string contactNum, int? Option)
            {
                _contactLinkType = ContactLinkType;
                _contactLinkKey = ContactLinkKey;
                _contactNum = contactNum;
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
                cm.CommandText = "REFContactInfor_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);


                if (GFunc.IsNE(criteria._uid))
                    cm.Parameters.AddWithValue("@Uid", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Uid", criteria._uid);
                cm.Parameters.AddWithValue("@ContactLinkKey", criteria._contactLinkKey);
                cm.Parameters.AddWithValue("@ContactLinkType", criteria._contactLinkType);
                cm.Parameters.AddWithValue("@ContactNum", criteria._contactNum);


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
            _uid = dr.GetInt32("UID");
            _contactLinkType = dr.GetInt32("ContactLinkType");
            _contactLinkKey = dr.GetInt32("ContactLinkKey");
            _contactPerson = dr.GetString("ContactPerson");
            _contactType = dr.GetInt32("ContactType");
            _contactNum = dr.GetString("ContactNum");
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

        internal bool Insert(out int? uid)
        {
            bool retValue = false;
            uid = null;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out uid);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? uid)
        {
            uid = 0;

            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFContactInfor_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@NewUid", uid);

                if (_uid == null)
                    cm.Parameters.AddWithValue("@Uid", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Uid", _uid);

                if (_contactLinkType == null)
                    cm.Parameters.AddWithValue("@ContactLinkType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ContactLinkType", _contactLinkType);

                if (_contactLinkKey == null)
                    cm.Parameters.AddWithValue("@ContactLinkKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ContactLinkKey", _contactLinkKey);

                if (_contactPerson == null)
                    cm.Parameters.AddWithValue("@ContactPerson", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ContactPerson", _contactPerson);

                if (_contactType == null)
                    cm.Parameters.AddWithValue("@ContactType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ContactType", _contactType);

                if (_contactNum == null)
                    cm.Parameters.AddWithValue("@ContactNum", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ContactNum", _contactNum);

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

                cm.Parameters["@NewUid"].Direction = ParameterDirection.Output;

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();


                uid = (int)cm.Parameters["@NewUid"].Value;

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
                cm.CommandText = "REFContactInfor_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewUid", 0);

                if (_uid == null)
                    cm.Parameters.AddWithValue("@Uid", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Uid", _uid);

                if (_contactLinkType == null)
                    cm.Parameters.AddWithValue("@ContactLinkType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ContactLinkType", _contactLinkType);

                if (_contactLinkKey == null)
                    cm.Parameters.AddWithValue("@ContactLinkKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ContactLinkKey", _contactLinkKey);

                if (_contactPerson == null)
                    cm.Parameters.AddWithValue("@ContactPerson", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ContactPerson", _contactPerson);

                if (_contactType == null)
                    cm.Parameters.AddWithValue("@ContactType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ContactType", _contactType);

                if (_contactNum == null)
                    cm.Parameters.AddWithValue("@ContactNum", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ContactNum", _contactNum);

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

                cm.Parameters["@NewUid"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "REFContactInfor_Delete";

                cm.Parameters.AddWithValue("@ContactLinkType", criteria._contactLinkType);
                cm.Parameters.AddWithValue("@ContactLinkKey", criteria._contactLinkKey);


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
                cm.CommandText = "REFContactInfor_Validation";

                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@Uid", criteria._uid);



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
