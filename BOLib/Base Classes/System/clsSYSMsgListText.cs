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
    public class SYSMsgListText : Csla.BusinessBase<SYSMsgListText>,System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _dataGrp = null;
        internal string _msgValue = string.Empty;
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
        internal bool? _buildIn = false;
        internal bool? _hidden = false;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _error = string.Empty;

        public int? DataGrp
        {
            get
            {
                return _dataGrp;
            }
            set
            {
                _dataGrp = value;
            }
        }

        public string MsgValue
        {
            get
            {
                return _msgValue;
            }
            set
            {
                _msgValue = value;
                PropertyHasChanged("MsgValue");
            }
        }

        public string LangText1
        {
            get
            {
                return _langText1;
            }            
        }

        public string LangText2
        {
            get
            {
                return _langText2;
            }
        }

        public string LangText3
        {
            get
            {
                return _langText3;
            }
        }

        public string LangText4
        {
            get
            {
                return _langText4;
            }
        }

        public string LangText5
        {
            get
            {
                return _langText5;
            }
        }

        public string LangText6
        {
            get
            {
                return _langText6;
            }
        }

        public string LangText7
        {
            get
            {
                return _langText7;
            }
        }

        public string LangText8
        {
            get
            {
                return _langText8;
            }
        }

        public string LangText9
        {
            get
            {
                return _langText9;
            }
        }

        public string LangText10
        {
            get
            {
                return _langText10;
            }
        }

        public bool? BuildIn
        {
            get
            {
                return _buildIn;
            }
        }

        public bool? Hidden
        {
            get
            {
                return _hidden;
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

        public string Error
        {
            get
            {
                return _error;
            }
            set
            {
                if (_error != value)
                    _error = value;
            }
        }

        protected override object GetIdValue()
        {
            return _dataGrp.ToString() + _msgValue.ToString();
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
            //// DataGrp
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "DataGrp");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DataGrp", 50));
            ////
            //// MsgValue
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "MsgValue");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgValue", 255));
            ////
            //// LangText1
            ////
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
            //AddCommonRules();
            //AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        public SYSMsgListText()
        { /* require use of factory method */           
        }

        internal static SYSMsgListText New()
        {
            
            SYSMsgListText child = new SYSMsgListText();
            
            return child;
        }

        internal static SYSMsgListText NewChild()
        {
            //
            SYSMsgListText child = new SYSMsgListText();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            //
            return child;
        }

        internal static SYSMsgListText Get(SafeDataReader dr)
        {
            SYSMsgListText child = new SYSMsgListText();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SYSMsgListText Get(int? dataGrp, string msgValue)
        {
            SYSMsgListText child = new SYSMsgListText();
            child.Fetch(new Criteria(dataGrp, msgValue, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            internal int? _dataGrp = 0;
            internal string _msgID = string.Empty;
            internal string _msgValue = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? DataGrp)
            {
                _dataGrp = DataGrp;
            }

            internal Criteria(int? DataGrp, string MsgValue)
            {
                _dataGrp = DataGrp;
                _msgValue = MsgValue;
            }

            internal Criteria(int? DataGrp, string MsgID, int? Option)
            {
                _dataGrp = DataGrp;
                _msgID = MsgID;
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
                cm.CommandText = "SYSMsgListText_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@DataGrp", criteria._dataGrp);

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
            _dataGrp = dr.GetInt32("DataGrp");
            _msgValue = dr.GetString("MsgValue");
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
            _buildIn = dr.GetBoolean("BuildIn");
            _hidden = dr.GetBoolean("Hidden");
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
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSMsgListText_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);


                if (_dataGrp == null)
                    cm.Parameters.AddWithValue("@DataGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DataGrp", _dataGrp);

                if (_msgValue == null)
                    cm.Parameters.AddWithValue("@MsgValue", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MsgValue", _msgValue);

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

                if (_buildIn == null)
                    cm.Parameters.AddWithValue("@BuildIn", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BuildIn", _buildIn);

                if (_hidden == null)
                    cm.Parameters.AddWithValue("@Hidden", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Hidden", _hidden);

                //  if (_createDate == null )
                cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                //else
                //cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

                if (AppInfor.currentUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);

                // if (_lastModifiedDate == null)
                cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                //else
                //    cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

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
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
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
                cm.CommandText = "SYSMsgListText_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                //cm.Parameters.AddWithValue("@NewDataGrp", string.Empty);
                //cm.Parameters.AddWithValue("@NewMsgValue", string.Empty);

                if (_dataGrp == null)
                    cm.Parameters.AddWithValue("@DataGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DataGrp", _dataGrp);

                if (_msgValue == null)
                    cm.Parameters.AddWithValue("@MsgValue", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MsgValue", _msgValue);

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

                if (_buildIn == null)
                    cm.Parameters.AddWithValue("@BuildIn", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BuildIn", _buildIn);

                if (_hidden == null)
                    cm.Parameters.AddWithValue("@Hidden", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Hidden", _hidden);

                if (_createDate == null || ((DateTime)_createDate).Year == 1)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

                if (_createUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateUserKey", _createUserKey);

                //if (_lastModifiedDate == null)
                cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                //else
                //    cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

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

                //cm.Parameters["@NewDataGrp"].Direction = ParameterDirection.Output;
                //cm.Parameters["@NewMsgValue"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "SYSMsgListText_Delete";

                cm.Parameters.AddWithValue("@DataGrp", criteria._dataGrp);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // cm.Parameters.AddWithValue("@MsgValue", criteria._msgValue);

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.
        }



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
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }
        internal bool Validation(SqlConnection cn, bool isNew)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSMsgListText_Validation";

                if ((bool)isNew)
                    cm.Parameters.AddWithValue("@isNew", 1);
                else
                    cm.Parameters.AddWithValue("@isNew", 0);

                cm.Parameters.AddWithValue("@DataGrp", _dataGrp);
                cm.Parameters.AddWithValue("@MsgValue", _msgValue);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }
        }
        internal bool Validation(SqlConnection cn, Criteria criteria,  bool isNew)
        {
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSMsgListText_Validation";

                if ((bool)isNew)
                    cm.Parameters.AddWithValue("@isNew", 1);
                else
                    cm.Parameters.AddWithValue("@isNew", 0);

                cm.Parameters.AddWithValue("@DataGrp", criteria._dataGrp);
                cm.Parameters.AddWithValue("@MsgValue", criteria._msgValue);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }
        }

        #endregion //Data Access - Delete
    }
}
