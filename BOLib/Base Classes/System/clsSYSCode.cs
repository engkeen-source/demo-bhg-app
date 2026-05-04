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
    public class SYSCode : Csla.BusinessBase<SYSCode>
    {
        #region Business Properties and Methods

        //declare members
        internal GEnum.SystemCode? _codeKey = null;
        internal string _codeID = string.Empty;
        internal string _permID = string.Empty;
        internal int? _codeGrp = null;
        internal string _codeDesLang1 = string.Empty;
        internal string _codeDesLang2 = string.Empty;
        internal string _codeDesLang3 = string.Empty;
        internal string _codeDesLang4 = string.Empty;
        internal string _codeDesLang5 = string.Empty;
        internal string _codeDesLang6 = string.Empty;
        internal string _codeDesLang7 = string.Empty;
        internal string _codeDesLang8 = string.Empty;
        internal string _codeDesLang9 = string.Empty;
        internal string _codeDesLang10 = string.Empty;
        internal DateTime? _lockPeriodUpTo = null;
        internal bool? _hidden = false;
        internal string _accessCode = string.Empty;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public GEnum.SystemCode? CodeKey
        {
            get
            {
                return _codeKey;
            }
        }

        public string CodeID
        {
            get
            {
                return _codeID;
            }
            set
            {
                _codeID = value;
                PropertyHasChanged("CodeID");
            }
        }

        public string PermID
        {
            get
            {
                return _permID;
            }
        }

        public int? CodeGrp
        {
            get
            {
                return _codeGrp;
            }
        }

        public string CodeDesLang1
        {
            get
            {
                return _codeDesLang1;
            }
        }

        public string CodeDesLang2
        {
            get
            {
                return _codeDesLang2;
            }
        }

        public string CodeDesLang3
        {
            get
            {
                return _codeDesLang3;
            }
        }

        public string CodeDesLang4
        {
            get
            {
                return _codeDesLang4;
            }
        }

        public string CodeDesLang5
        {
            get
            {
                return _codeDesLang5;
            }
        }

        public string CodeDesLang6
        {
            get
            {
                return _codeDesLang6;
            }
        }

        public string CodeDesLang7
        {
            get
            {
                return _codeDesLang7;
            }
        }

        public string CodeDesLang8
        {
            get
            {
                return _codeDesLang8;
            }
        }

        public string CodeDesLang9
        {
            get
            {
                return _codeDesLang9;
            }
        }

        public string CodeDesLang10
        {
            get
            {
                return _codeDesLang10;
            }
        }

        public DateTime? LockPeriodUpTo
        {
            get
            {
                return _lockPeriodUpTo;
            }
            set
            {
                _lockPeriodUpTo = value;
                PropertyHasChanged("LockPeriodUpTo");
            }
        }

        public bool? Hidden
        {
            get
            {
                return _hidden;
            }
        }

        public string AccessCode
        {
            get
            {
                return _accessCode;
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
            return _codeKey.ToString();
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
            //// CodeID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "CodeID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CodeID", 8));
            ////
            //// PermID
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PermID", 50));
            ////
            //// CodeDesLang1
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CodeDesLang1", 255));
            ////
            //// CodeDesLang2
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CodeDesLang2", 255));
            ////
            //// CodeDesLang3
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CodeDesLang3", 255));
            ////
            //// CodeDesLang4
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CodeDesLang4", 255));
            ////
            //// CodeDesLang5
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CodeDesLang5", 255));
            ////
            //// CodeDesLang6
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CodeDesLang6", 255));
            ////
            //// CodeDesLang7
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CodeDesLang7", 255));
            ////
            //// CodeDesLang8
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CodeDesLang8", 255));
            ////
            //// CodeDesLang9
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CodeDesLang9", 255));
            ////
            //// CodeDesLang10
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CodeDesLang10", 255));
            ////
            //// AccessCode
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("AccessCode", 255));
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

        internal SYSCode()
        { /* require use of factory method */ }

        internal static SYSCode New()
        {
            SYSCode child = new SYSCode();
            return child;
        }

        internal static SYSCode NewChild()
        {
            SYSCode child = new SYSCode();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static SYSCode Get(SafeDataReader dr)
        {
            SYSCode child = new SYSCode();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SYSCode Get(int? codeKey)
        {
            SYSCode child = new SYSCode();
            child.Fetch(new Criteria(codeKey, 1));
            return child;
        }

        internal static SYSCode Get(SqlConnection cn, int? codeKey)
        {
            SYSCode child = new SYSCode();
            child.Fetch(cn, new Criteria(codeKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _codeKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? CodeKey)
            {
                _codeKey = CodeKey;
            }

            internal Criteria(int? CodeKey, int? Option)
            {
                _codeKey = CodeKey;
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
                cm.CommandText = "SYSCode_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);

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
                    retValue=false;

            }// Already close and dispose sql connection.
            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _codeKey = (GEnum.SystemCode)dr.GetInt32("CodeKey");
            _codeID = dr.GetString("CodeID");
            _permID = dr.GetString("PermID");
            _codeGrp = dr.GetInt32("CodeGrp");
            _codeDesLang1 = dr.GetString("CodeDesLang1");
            _codeDesLang2 = dr.GetString("CodeDesLang2");
            _codeDesLang3 = dr.GetString("CodeDesLang3");
            _codeDesLang4 = dr.GetString("CodeDesLang4");
            _codeDesLang5 = dr.GetString("CodeDesLang5");
            _codeDesLang6 = dr.GetString("CodeDesLang6");
            _codeDesLang7 = dr.GetString("CodeDesLang7");
            _codeDesLang8 = dr.GetString("CodeDesLang8");
            _codeDesLang9 = dr.GetString("CodeDesLang9");
            _codeDesLang10 = dr.GetString("CodeDesLang10");
            _lockPeriodUpTo = dr.GetDateTime("LockPeriodUpTo");
            _hidden = dr.GetBoolean("Hidden");
            _accessCode = dr.GetString("AccessCode");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            ValidationRules.CheckRules();
            return true;           
        }
        #endregion //Data Access - Fetch

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
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSCode_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
               // cm.Parameters.AddWithValue("@NewCodeKey", 0);

                if (_codeKey == null)
                    cm.Parameters.AddWithValue("@CodeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeKey", _codeKey);

                if (_codeID == null)
                    cm.Parameters.AddWithValue("@CodeID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeID", _codeID);

                if (_permID == null)
                    cm.Parameters.AddWithValue("@PermID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PermID", _permID);

                if (_codeGrp == null)
                    cm.Parameters.AddWithValue("@CodeGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeGrp", _codeGrp);

                if (_codeDesLang1 == null)
                    cm.Parameters.AddWithValue("@CodeDesLang1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeDesLang1", _codeDesLang1);

                if (_codeDesLang2 == null)
                    cm.Parameters.AddWithValue("@CodeDesLang2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeDesLang2", _codeDesLang2);

                if (_codeDesLang3 == null)
                    cm.Parameters.AddWithValue("@CodeDesLang3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeDesLang3", _codeDesLang3);

                if (_codeDesLang4 == null)
                    cm.Parameters.AddWithValue("@CodeDesLang4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeDesLang4", _codeDesLang4);

                if (_codeDesLang5 == null)
                    cm.Parameters.AddWithValue("@CodeDesLang5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeDesLang5", _codeDesLang5);

                if (_codeDesLang6 == null)
                    cm.Parameters.AddWithValue("@CodeDesLang6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeDesLang6", _codeDesLang6);

                if (_codeDesLang7 == null)
                    cm.Parameters.AddWithValue("@CodeDesLang7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeDesLang7", _codeDesLang7);

                if (_codeDesLang8 == null)
                    cm.Parameters.AddWithValue("@CodeDesLang8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeDesLang8", _codeDesLang8);

                if (_codeDesLang9 == null)
                    cm.Parameters.AddWithValue("@CodeDesLang9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeDesLang9", _codeDesLang9);

                if (_codeDesLang10 == null)
                    cm.Parameters.AddWithValue("@CodeDesLang10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeDesLang10", _codeDesLang10);

                if (_lockPeriodUpTo == null || ((DateTime)_lockPeriodUpTo).Year==1)
                    cm.Parameters.AddWithValue("@LockPeriodUpTo", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LockPeriodUpTo", _lockPeriodUpTo.Value);

                if (_hidden == null)
                    cm.Parameters.AddWithValue("@Hidden", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Hidden", _hidden);

                if (_accessCode == null)
                    cm.Parameters.AddWithValue("@AccessCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessCode", _accessCode);

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
                //cm.Parameters["@NewCodeKey"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();


                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.
            
        }
        #endregion //Data Access - Update
    }
}