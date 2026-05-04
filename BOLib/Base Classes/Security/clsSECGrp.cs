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
    public class SECGrp : Csla.BusinessBase<SECGrp>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _grpKey = 0;
        internal string _grpID = string.Empty;
        internal string _grpDes = string.Empty;
        internal string _langText1 = string.Empty;
        internal string _langText2 = string.Empty;
        internal string _langText3 = string.Empty;
        internal string _langText4 = string.Empty;
        internal string _langText5 = string.Empty;
        internal string _langText6 = string.Empty;
        internal string _langText7 = string.Empty;
        internal string _langText8 = string.Empty;
        internal string _langText9 = string.Empty;
        internal string _langText10 = string.Empty;
        internal bool? _builtIn = false;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public int? GrpKey
        {
            get
            {
                CanReadProperty("GrpKey", true);
                return _grpKey;
            }
        }

        public string GrpID
        {
            get
            {
                return _grpID;
            }
            set
            {

                if (value == null) value = string.Empty;

                _grpID = value;
                PropertyHasChanged("GrpID");


            }
        }

        public string GrpDes
        {
            get
            {
                return _grpDes;
            }
            set
            {
                if (value == null) value = string.Empty;

                _grpDes = value;
                PropertyHasChanged("GrpDes");


            }
        }

        public string LangText1
        {
            get
            {
                CanReadProperty("LangText1", true);
                return _langText1;
            }
        }

        public string LangText2
        {
            get
            {
                CanReadProperty("LangText2", true);
                return _langText2;
            }
        }

        public string LangText3
        {
            get
            {
                CanReadProperty("LangText3", true);
                return _langText3;
            }
        }

        public string LangText4
        {
            get
            {
                CanReadProperty("LangText4", true);
                return _langText4;
            }
        }

        public string LangText5
        {
            get
            {
                CanReadProperty("LangText5", true);
                return _langText5;
            }
        }

        public string LangText6
        {
            get
            {
                CanReadProperty("LangText6", true);
                return _langText6;
            }
        }

        public string LangText7
        {
            get
            {
                CanReadProperty("LangText7", true);
                return _langText7;
            }
        }

        public string LangText8
        {
            get
            {
                CanReadProperty("LangText8", true);
                return _langText8;
            }
        }

        public string LangText9
        {
            get
            {
                CanReadProperty("LangText9", true);
                return _langText9;
            }
        }

        public string LangText10
        {
            get
            {
                CanReadProperty("LangText10", true);
                return _langText10;
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
            return _grpKey.ToString();
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
            //// GrpID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "GrpID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("GrpID", 20));
            ////
            //// GrpDes
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "GrpDes");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("GrpDes", 255));
            ////
            //// LangText1
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "LangText1");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText1", 255));
            ////
            //// LangText2
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText2", 255));
            ////
            //// LangText3
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText3", 255));
            ////
            //// LangText4
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText4", 255));
            ////
            //// LangText5
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText5", 255));
            ////
            //// LangText6
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText6", 255));
            ////
            //// LangText7
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText7", 255));
            ////
            //// LangText8
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText8", 255));
            ////
            //// LangText9
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText9", 255));
            ////
            //// LangText10
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText10", 255));
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

        internal SECGrp()
        { /* require use of factory method */ }

        internal static SECGrp New()
        {
            SECGrp child = new SECGrp();
            return child;
        }

        internal static SECGrp NewChild()
        {
            SECGrp child = new SECGrp();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static SECGrp Get(SafeDataReader dr)
        {
            SECGrp child = new SECGrp();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SECGrp Get(int? grpKey)
        {
            SECGrp child = new SECGrp();
            child.Fetch(new Criteria(grpKey, string.Empty, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _grpKey = null;
            public int? _option = null;
            public string _GrpID;
            internal Criteria()
            {
            }

            internal Criteria(int? GrpKey)
            {
                _grpKey = GrpKey;
            }

            internal Criteria(int? GrpKey, string GrpID)
            {
                _grpKey = GrpKey;
                _GrpID = GrpID;
            }

            internal Criteria(int? GrpKey, string GrpID, int? Option)
            {
                _grpKey = GrpKey;
                _option = Option;
                _GrpID = GrpID;
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
                cm.CommandText = "SECGrp_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
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
            _grpKey = dr.GetInt32("GrpKey");
            _grpID = dr.GetString("GrpID");
            _grpDes = dr.GetString("GrpDes");
            _langText1 = dr.GetString("LangText1");
            _langText2 = dr.GetString("LangText2");
            _langText3 = dr.GetString("LangText3");
            _langText4 = dr.GetString("LangText4");
            _langText5 = dr.GetString("LangText5");
            _langText6 = dr.GetString("LangText6");
            _langText7 = dr.GetString("LangText7");
            _langText8 = dr.GetString("LangText8");
            _langText9 = dr.GetString("LangText9");
            _langText10 = dr.GetString("LangText10");
            _builtIn = dr.GetBoolean("BuiltIn");
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

        internal bool Insert(out int? grpKey)
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
                    retValue = this.Insert(cn, out grpKey);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? grpKey)
        {
            grpKey = 0;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECGrp_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@NewGrpKey", grpKey);

                if (_grpKey == null)
                    cm.Parameters.AddWithValue("@GrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@GrpKey", _grpKey);

                if (_grpID == null)
                    cm.Parameters.AddWithValue("@GrpID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@GrpID", _grpID);

                if (_grpDes == null)
                    cm.Parameters.AddWithValue("@GrpDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@GrpDes", _grpDes);

                if (_langText1 == null)
                    cm.Parameters.AddWithValue("@LangText1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText1", _langText1);

                if (_langText2 == null)
                    cm.Parameters.AddWithValue("@LangText2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText2", _langText2);

                if (_langText3 == null)
                    cm.Parameters.AddWithValue("@LangText3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText3", _langText3);

                if (_langText4 == null)
                    cm.Parameters.AddWithValue("@LangText4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText4", _langText4);

                if (_langText5 == null)
                    cm.Parameters.AddWithValue("@LangText5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText5", _langText5);

                if (_langText6 == null)
                    cm.Parameters.AddWithValue("@LangText6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText6", _langText6);

                if (_langText7 == null)
                    cm.Parameters.AddWithValue("@LangText7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText7", _langText7);

                if (_langText8 == null)
                    cm.Parameters.AddWithValue("@LangText8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText8", _langText8);

                if (_langText9 == null)
                    cm.Parameters.AddWithValue("@LangText9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText9", _langText9);

                if (_langText10 == null)
                    cm.Parameters.AddWithValue("@LangText10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText10", _langText10);

                if (_builtIn == null)
                    cm.Parameters.AddWithValue("@BuiltIn", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BuiltIn", _builtIn);

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
                cm.Parameters["@NewGrpKey"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                grpKey = (int)cm.Parameters["@NewGrpKey"].Value;

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
                cm.CommandText = "SECGrp_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);

                cm.Parameters.AddWithValue("@NewGrpKey", 0);

                if (_grpKey == null)
                    cm.Parameters.AddWithValue("@GrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@GrpKey", _grpKey);

                if (_grpID == null)
                    cm.Parameters.AddWithValue("@GrpID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@GrpID", _grpID);

                if (_grpDes == null)
                    cm.Parameters.AddWithValue("@GrpDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@GrpDes", _grpDes);

                if (_langText1 == null)
                    cm.Parameters.AddWithValue("@LangText1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText1", _langText1);

                if (_langText2 == null)
                    cm.Parameters.AddWithValue("@LangText2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText2", _langText2);

                if (_langText3 == null)
                    cm.Parameters.AddWithValue("@LangText3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText3", _langText3);

                if (_langText4 == null)
                    cm.Parameters.AddWithValue("@LangText4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText4", _langText4);

                if (_langText5 == null)
                    cm.Parameters.AddWithValue("@LangText5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText5", _langText5);

                if (_langText6 == null)
                    cm.Parameters.AddWithValue("@LangText6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText6", _langText6);

                if (_langText7 == null)
                    cm.Parameters.AddWithValue("@LangText7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText7", _langText7);

                if (_langText8 == null)
                    cm.Parameters.AddWithValue("@LangText8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText8", _langText8);

                if (_langText9 == null)
                    cm.Parameters.AddWithValue("@LangText9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText9", _langText9);

                if (_langText10 == null)
                    cm.Parameters.AddWithValue("@LangText10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LangText10", _langText10);

                if (_builtIn == null)
                    cm.Parameters.AddWithValue("@BuiltIn", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BuiltIn", _builtIn);

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

                cm.Parameters["@NewGrpKey"].Direction = ParameterDirection.Output;

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
            bool retValue = false;
            string msgID = MsgID.Common.DeleteFail;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECGrp_Delete";

                cm.Parameters.AddWithValue("@GrpKey", criteria._grpKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();


                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;
            }// Already close and dispose sql connection.

            return retValue;
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
                cm.CommandText = "SECGrp_Validation";

                cm.Parameters.AddWithValue("@IsNew", isNew);

                cm.Parameters.AddWithValue("@GrpKey", criteria._grpKey);
                cm.Parameters.AddWithValue("@GrpID", this._grpID);

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
