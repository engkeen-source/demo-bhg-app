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
    public class REFTerm : Csla.BusinessBase<REFTerm>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _termKey = 0;
        internal string _termID = string.Empty;
        internal string _termDes = string.Empty;
        internal bool? _standTerm = true;
        internal short? _standNetDueDay = 0;
        internal short? _standDisDay = 0;
        internal decimal? _standDisPercent = 0;
        internal short? _standAddDays = 0;
        internal short? _dateNetDueDay = 1;
        internal short? _dateDueDayNextMth = 0;
        internal decimal? _dateDisPercent = 0;
        internal short? _dateDisDay = 0;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public int? TermKey
        {
            get
            {
                return _termKey;
            }
        }

        public string TermID
        {
            get
            {
                return _termID;
            }
            set
            {
                _termID = value;
                PropertyHasChanged("TermID");
            }
        }

        public string TermDes
        {
            get
            {
                return _termDes;
            }
            set
            {
                _termDes = value;
                PropertyHasChanged("TermDes");
            }
        }

        public bool? StandTerm
        {
            get
            {
                return _standTerm;
            }
            set
            {
                _standTerm = value;
                PropertyHasChanged("StandTerm");
            }
        }

        public short? StandNetDueDay
        {
            get
            {
                return _standNetDueDay;
            }
            set
            {
                _standNetDueDay = value;
                PropertyHasChanged("StandNetDueDay");
            }
        }

        public short? StandDisDay
        {
            get
            {
                return _standDisDay;
            }
            set
            {
                _standDisDay = value;
                PropertyHasChanged("StandDisDay");
            }
        }

        public decimal? StandDisPercent
        {
            get
            {
                return _standDisPercent;
            }
            set
            {
                _standDisPercent = value;
                PropertyHasChanged("StandDisPercent");
            }
        }

        public short? StandAddDays
        {
            get
            {
                return _standAddDays;
            }
            set
            {
                _standAddDays = value;
                PropertyHasChanged("StandAddDays");
            }
        }

        public short? DateNetDueDay
        {
            get
            {
                return _dateNetDueDay;
            }
            set
            {
                _dateNetDueDay = value;
                PropertyHasChanged("DateNetDueDay");
            }
        }

        public short? DateDueDayNextMth
        {
            get
            {
                return _dateDueDayNextMth;
            }
            set
            {
                _dateDueDayNextMth = value;
                PropertyHasChanged("DateDueDayNextMth");
            }
        }

        public decimal? DateDisPercent
        {
            get
            {
                return _dateDisPercent;
            }
            set
            {
                _dateDisPercent = value;
                PropertyHasChanged("DateDisPercent");
            }
        }

        public short? DateDisDay
        {
            get
            {
                return _dateDisDay;
            }
            set
            {
                _dateDisDay = value;
                PropertyHasChanged("DateDisDay");
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
            return _termKey.ToString();
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
            //// TermID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "TermID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TermID", 50));
            ////
            //// TermDes
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "TermDes");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TermDes", 255));
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

        internal REFTerm()
        { /* require use of factory method */ }

        internal static REFTerm New()
        {
            REFTerm child = new REFTerm();
            return child;
        }

        internal static REFTerm NewChild()
        {
            REFTerm child = new REFTerm();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static REFTerm Get(SafeDataReader dr)
        {
            REFTerm child = new REFTerm();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static REFTerm Get(int? termKey)
        {
            REFTerm child = new REFTerm();
            child.Fetch(new Criteria(termKey, 1));
            return child;
        }
        internal static REFTerm Get(SqlConnection cn, int? termKey)
        {
            REFTerm child = new REFTerm();
            child.Fetch(cn,new Criteria(termKey, 1));
            return child;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _termKey = null;
            public int? _option = null;
            public string _termID = string.Empty;

            internal Criteria()
            {
            }

            internal Criteria(int? TermKey)
            {
                _termKey = TermKey;
            }

            internal Criteria(int? TermKey, int? Option)
            {
                _termKey = TermKey;
                _option = Option;
            }

            internal Criteria(int? TermKey, string TermID)
            {
                _termKey = TermKey;
                _termID = TermID;
            }

            //Add Thida
            internal Criteria(int? TermKey, string TermID, int? Option)
            {
                _termKey = TermKey;
                _termID = TermID;
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
                cm.CommandText = "REFTerm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@TermKey", criteria._termKey);

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
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
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _termKey = dr.GetInt32("TermKey");
            _termID = dr.GetString("TermID");
            _termDes = dr.GetString("TermDes");
            _standTerm = dr.GetBoolean("StandTerm");
            _standNetDueDay = dr.GetInt16("StandNetDueDay");
            _standDisDay = dr.GetInt16("StandDisDay");
            _standDisPercent = dr.GetDecimal("StandDisPercent");
            _standAddDays = dr.GetInt16("StandAddDays");
            _dateNetDueDay = dr.GetInt16("DateNetDueDay");
            _dateDueDayNextMth = dr.GetInt16("DateDueDayNextMth");
            _dateDisPercent = dr.GetDecimal("DateDisPercent");
            _dateDisDay = dr.GetInt16("DateDisDay");
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

        internal bool Insert(out int? termKey)
        {
            bool retValue = false;
            termKey = null;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out termKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? termKey)
        {
            termKey = 0;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFTerm_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@NewTermKey", termKey);

                if (_termKey == null)
                    cm.Parameters.AddWithValue("@TermKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TermKey", _termKey);

                if (_termID == null)
                    cm.Parameters.AddWithValue("@TermID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TermID", _termID);

                if (_termDes == null)
                    cm.Parameters.AddWithValue("@TermDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TermDes", _termDes);

                if (_standTerm == null)
                    cm.Parameters.AddWithValue("@StandTerm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandTerm", _standTerm);

                if (_standNetDueDay == null)
                    cm.Parameters.AddWithValue("@StandNetDueDay", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandNetDueDay", _standNetDueDay);

                if (_standDisDay == null)
                    cm.Parameters.AddWithValue("@StandDisDay", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandDisDay", _standDisDay);

                if (_standDisPercent == null)
                    cm.Parameters.AddWithValue("@StandDisPercent", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandDisPercent", _standDisPercent);

                if (_standAddDays == null)
                    cm.Parameters.AddWithValue("@StandAddDays", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandAddDays", _standAddDays);

                if (_dateNetDueDay == null)
                    cm.Parameters.AddWithValue("@DateNetDueDay", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DateNetDueDay", _dateNetDueDay);

                if (_dateDueDayNextMth == null)
                    cm.Parameters.AddWithValue("@DateDueDayNextMth", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DateDueDayNextMth", _dateDueDayNextMth);

                if (_dateDisPercent == null)
                    cm.Parameters.AddWithValue("@DateDisPercent", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DateDisPercent", _dateDisPercent);

                if (_dateDisDay == null)
                    cm.Parameters.AddWithValue("@DateDisDay", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DateDisDay", _dateDisDay);

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

                cm.Parameters["@NewTermKey"].Direction = ParameterDirection.Output;

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                termKey = (int)cm.Parameters["@NewTermKey"].Value;

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
                cm.CommandText = "REFTerm_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewTermKey", 0);

                if (_termKey == null)
                    cm.Parameters.AddWithValue("@TermKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TermKey", _termKey);

                if (_termID == null)
                    cm.Parameters.AddWithValue("@TermID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TermID", _termID);

                if (_termDes == null)
                    cm.Parameters.AddWithValue("@TermDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TermDes", _termDes);

                if (_standTerm == null)
                    cm.Parameters.AddWithValue("@StandTerm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandTerm", _standTerm);

                if (_standNetDueDay == null)
                    cm.Parameters.AddWithValue("@StandNetDueDay", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandNetDueDay", _standNetDueDay);

                if (_standDisDay == null)
                    cm.Parameters.AddWithValue("@StandDisDay", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandDisDay", _standDisDay);

                if (_standDisPercent == null)
                    cm.Parameters.AddWithValue("@StandDisPercent", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandDisPercent", _standDisPercent);

                if (_standAddDays == null)
                    cm.Parameters.AddWithValue("@StandAddDays", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandAddDays", _standAddDays);

                if (_dateNetDueDay == null)
                    cm.Parameters.AddWithValue("@DateNetDueDay", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DateNetDueDay", _dateNetDueDay);

                if (_dateDueDayNextMth == null)
                    cm.Parameters.AddWithValue("@DateDueDayNextMth", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DateDueDayNextMth", _dateDueDayNextMth);

                if (_dateDisPercent == null)
                    cm.Parameters.AddWithValue("@DateDisPercent", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DateDisPercent", _dateDisPercent);

                if (_dateDisDay == null)
                    cm.Parameters.AddWithValue("@DateDisDay", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DateDisDay", _dateDisDay);

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

                cm.Parameters["@NewTermKey"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "REFTerm_Delete";

                cm.Parameters.AddWithValue("@TermKey", criteria._termKey);

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
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFTerm_Validation";

                cm.Parameters.AddWithValue("@isNew", isNew);
                cm.Parameters.AddWithValue("@TermKey", criteria._termKey);
                cm.Parameters.AddWithValue("@TermID", criteria._termID);

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

        private void Clear()
        {
            _termKey = 0;
            _termID = string.Empty;
            _termDes = string.Empty;
            _standTerm = true;
            _standNetDueDay = 0;
            _standDisDay = 0;
            _standDisPercent = 0;
            _standAddDays = 0;
            _dateNetDueDay = 1;
            _dateDueDayNextMth = 0;
            _dateDisPercent = 0;
            _dateDisDay = 0;
            _createDate = null;
            _createUserKey = null;
            _lastModifiedDate = null;
            _lastModifiedUserKey = null;
            _custom1 = string.Empty;
            _custom2 = string.Empty;
            _custom3 = string.Empty;
            
        }
    
    }
}
