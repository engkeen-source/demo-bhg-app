using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Collections;
using System.Transactions;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class TASAlert : Csla.BusinessBase<TASAlert>
    {
        #region Business Properties and Methods

        internal int? _alertKey;
        internal string _alertID;
        internal string _alertDes;
        internal int _alertApplyGrp;
        internal int _alertApplyTo;
        internal string _alertIDFrom;
        internal string _alertIDTo;
        internal string _alertCondition;
        internal string _alertValueAmt;
        internal DateTime? _alertValueDate;
        internal DateTime? _alertLastActivateDate;
        internal int? _recurType;
        internal int _RecurIntHourMins;
        internal int _recurIntDayNum;
        internal int _recurIntWeekNum;
        internal int _recurIntWeekDay;
        internal int _recurIntMthNum;
        internal int _recurIntMthDayNum;
        internal int _recurIntMthWeek;
        internal int _recurIntMthDay;
        internal int _recurIntYearNum;
        internal int _recurIntYearMthNum;
        internal int _recurIntYearDayNum;
        internal int _recurIntYearMthDay;
        internal int _recurIntYearMthWeek;
        internal bool? _inActive;
        internal decimal? _retriggersAfterHours;
        internal DateTime? _NextRunDateTime;
        internal int _TaskState;
        internal DateTime? _createDate;
        internal int? _createUserKey;
        internal DateTime? _lastModifiedDate;
        internal int? _lastModifiedUserKey;
        internal string _custom1;
        internal string _custom2;
        internal string _custom3;
        internal string _custom4;
        internal string _custom5;

        public int? AlertKey
        {
            get { return _alertKey; }
            set 
            {
                _alertKey = value;
                PropertyHasChanged("AlertKey");
            }
        }
        public string AlertID
        {

            get
            {
                return this._alertID;
            }
            set
            {
                this._alertID = value;
                PropertyHasChanged("AlertID");
            }
        }
        public string AlertDes
        {

            get
            {
                return this._alertDes;
            }
            set
            {
                this._alertDes = value;
                PropertyHasChanged("AlertDes");
            }
        }
        public int AlertApplyGrp
        {

            get
            {
                return this._alertApplyGrp;
            }
            set
            {
                this._alertApplyGrp = value;
                PropertyHasChanged("AlertApplyGrp");
            }
        }
        public int AlertApplyTo
        {

            get
            {
                return this._alertApplyTo;
            }
            set
            {
                this._alertApplyTo = value;
                PropertyHasChanged("AlertApplyTo");
            }
        }
        public string AlertIDFrom
        {

            get
            {
                return this._alertIDFrom;
            }
            set
            {
                this._alertIDFrom = value;
                PropertyHasChanged("AlertIDFrom");
            }
        }
        public string AlertIDTo
        {

            get
            {
                return this._alertIDTo;
            }
            set
            {
                this._alertIDTo = value;
                PropertyHasChanged("AlertIDTo");
            }
        }
        public string AlertCondition
        {

            get
            {
                return this._alertCondition;
            }
            set
            {
                this._alertCondition = value;
                PropertyHasChanged("AlertCondition");
            }
        }
        public string AlertValueAmt
        {

            get
            {
                return this._alertValueAmt;
            }
            set
            {
                this._alertValueAmt = value;
                PropertyHasChanged("AlertValueAmt");
            }
        }
        public DateTime? AlertValueDate
        {

            get
            {
                return this._alertValueDate;
            }
            set
            {
                this._alertValueDate = value;
                PropertyHasChanged("AlertValueDate");
            }
        }
        public DateTime? AlertLastActivateDate
        {

            get
            {
                return this._alertLastActivateDate;
            }
            set
            {
                this._alertLastActivateDate = value;
                PropertyHasChanged("AlertLastActivateDate");
            }
        }
        public int? RecurType
        {

            get
            {
                return this._recurType;
            }
            set
            {
                this._recurType = value;
                PropertyHasChanged("RecurType");
            }
        }
        public int RecurIntHourMins
        {

            get
            {
                return this._RecurIntHourMins ;
            }
            set
            {
                this._RecurIntHourMins = value;
                PropertyHasChanged("RecurIntHourMins");
            }
        }
        public int RecurIntDayNum
        {

            get
            {
                return this._recurIntDayNum;
            }
            set
            {
                this._recurIntDayNum = value;
                PropertyHasChanged("RecurIntDayNum");
            }
        }
        public int RecurIntWeekNum
        {

            get
            {
                return this._recurIntWeekNum;
            }
            set
            {
                this._recurIntWeekNum = value;
                PropertyHasChanged("RecurIntWeekNum");
            }
        }
        public int RecurIntWeekDay
        {

            get
            {
                return this._recurIntWeekDay;
            }
            set
            {
                this._recurIntWeekDay = value;
                PropertyHasChanged("RecurIntWeekDay");
            }
        }
        public int RecurIntMthNum
        {

            get
            {
                return this._recurIntMthNum;
            }
            set
            {
                this._recurIntMthNum = value;
                PropertyHasChanged("RecurIntMthNum");
            }
        }
        public int RecurIntMthDayNum
        {

            get
            {
                return this._recurIntMthDayNum;
            }
            set
            {
                this._recurIntMthDayNum = value;
                PropertyHasChanged("RecurIntMthDayNum");
            }
        }
        public int RecurIntMthWeek
        {

            get
            {
                return this._recurIntMthWeek;
            }
            set
            {
                this._recurIntMthWeek = value;
                PropertyHasChanged("RecurIntMthWeek");
            }
        }
        public int RecurIntMthDay
        {

            get
            {
                return this._recurIntMthDay;
            }
            set
            {
                this._recurIntMthDay = value;
                PropertyHasChanged("RecurIntMthDay");
            }
        }
        public int RecurIntYearNum
        {

            get
            {
                return this._recurIntYearNum;
            }
            set
            {
                this._recurIntYearNum = value;
                PropertyHasChanged("RecurIntYearNum");
            }
        }
        public int RecurIntYearMthNum
        {

            get
            {
                return this._recurIntYearMthNum;
            }
            set
            {
                this._recurIntYearMthNum = value;
                PropertyHasChanged("RecurIntYearMthNum");
            }
        }
        public int RecurIntYearDayNum
        {

            get
            {
                return this._recurIntYearDayNum;
            }
            set
            {
                this._recurIntYearDayNum = value;
                PropertyHasChanged("RecurIntYearDayNum");
            }
        }
        public int RecurIntYearMthDay
        {

            get
            {
                return this._recurIntYearMthDay;
            }
            set
            {
                this._recurIntYearMthDay = value;
                PropertyHasChanged("RecurIntYearMthDay");
            }
        }
        public int RecurIntYearMthWeek
        {

            get
            {
                return this._recurIntYearMthWeek;
            }
            set
            {
                this._recurIntYearMthWeek = value;
                PropertyHasChanged("RecurIntYearMthWeek");
            }
        }
        public decimal? ReTriggerAfterHours
        {

            get
            {
                return this._retriggersAfterHours;
            }
            set
            {
                this._retriggersAfterHours = value;
                PropertyHasChanged("ReTriggerAfterHours");
            }
        }
        public DateTime? NextRunDateTime
        {
            get
            {
                return this._NextRunDateTime;
            }
            set
            {
                this._NextRunDateTime = value;
                PropertyHasChanged("NextRunDateTime");
            }
        }
        public int TaskState
        {

            get
            {
                return this._TaskState;
            }
            set
            {
                this._TaskState = value;
                PropertyHasChanged("TaskState");
            }
        }
        public bool? InActive
        {

            get
            {
                return this._inActive;
            }
            set
            {
                this._inActive = value;
                PropertyHasChanged("InActive");
            }
        }

        public DateTime? CreateDate
        {
            get
            {
                return this._createDate;
            }
            set
            {
                this._createDate = value;
                PropertyHasChanged("CreateDate");
            }
        }
        public int? CreateUserKey
        {
            get
            {
                return this._createUserKey;
            }
            set
            {
                this._createUserKey = value;
                PropertyHasChanged("CreateUserKey");
            }
        }
        public DateTime? LastModifiedDate
        {
            get
            {
                return this._lastModifiedDate;
            }
            set
            {
                this._lastModifiedDate = value;
                PropertyHasChanged("LastModifiedDate");
            }
        }
        public int? LastModifiedUserKey
        {
            get
            {
                return this._lastModifiedUserKey;
            }
            set
            {
                this._lastModifiedUserKey = value;
                PropertyHasChanged("LastModifiedUserKey");
            }
        }
        public string Custom1
        {
            get
            {
                return this._custom1;
            }
            set
            {
                this._custom1 = value;
                PropertyHasChanged("Custom1");
            }
        }
        public string Custom2
        {
            get
            {
                return this._custom2;
            }
            set
            {
                this._custom2 = value;
                PropertyHasChanged("Custom2");
            }
        }
        public string Custom3
        {
            get
            {
                return this._custom3;
            }
            set
            {
                this._custom3 = value;
                PropertyHasChanged("Custom3");
            }
        }
        public string Custom4
        {
            get
            {
                return this._custom4;
            }
            set
            {
                this._custom4 = value;
                PropertyHasChanged("Custom4");
            }
        }
        public string Custom5
        {
            get
            {
                return this._custom5;
            }
            set
            {
                this._custom5 = value;
                PropertyHasChanged("Custom5");
            }
        }

        #endregion

        #region Validation Rules
        private void AddCustomRules()
        {
            //add custom/non-generated rules here...
        }

        private void AddCommonRules()
        {
            /*
           //
           // PriceID
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "PriceID");
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PriceID", 50));
           //
           // PriceDes
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "PriceDes");
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PriceDes", 255));
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
            */
        }

        protected override void AddBusinessRules()
        {
            /*
           AddCommonRules();
           AddCustomRules();
            */
        }
        #endregion //Validation Rules

        #region Factory Methods
        internal TASAlert()
           
        {
            this._alertID = string.Empty;
            this._alertDes = string.Empty;
            this._alertApplyGrp = 0;
            this._alertApplyTo = 0;
            this._alertIDFrom = null;
            this._alertIDTo = null;
            this._alertCondition = null;
            this._alertValueAmt = null;
            this._alertValueDate = null;
            this._alertLastActivateDate = null;
            this._recurType = 10;
            this._RecurIntHourMins = 0;
            this._recurIntDayNum = 0;
            this._recurIntWeekNum = 0;
            this._recurIntWeekDay = 0;
            this._recurIntMthNum = 0;
            this._recurIntMthDayNum = 0;
            this._recurIntMthWeek = 0;
            this._recurIntMthDay = 0;
            this._recurIntYearNum = 0;
            this._recurIntYearMthNum = 0;
            this._recurIntYearDayNum = 0;
            this._recurIntYearDayNum = 0;
            this._recurIntYearMthDay = 0;
            this._recurIntYearMthWeek = 0;
            this._NextRunDateTime = null;
            this._TaskState = 1;

        }
        public static TASAlert Get(int? AlertKey)
        {
            TASAlert child = new TASAlert();
            child.Fetch(new Criteria(AlertKey, 1));
            return child;
        }
        public static TASAlert New()
        {
            TASAlert child = new TASAlert();
            return child;
        }        
        /// <summary>
        /// Disposing objects
        /// </summary>
        public void Dispose()
        {

        }

        #endregion        

        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            //public int? _DocCodeKey = null;
            public int? _alertKey = null;
            public int? _option = null;
            public string _alertID = string.Empty;

            internal Criteria()
            {
            }
            internal Criteria(int? AlertKey)
            {
                _alertKey = AlertKey;
            }
            internal Criteria(int? AlertKey, int? Option)
            {
                _alertKey = AlertKey;
                _option = Option;
            }
            internal Criteria(int? AlertKey, string AlertID)
            {
                _alertKey = AlertKey;
                _alertID = AlertID;
            }
            internal Criteria(int? AlertKey, string AlertID, int? Option)
            {                
                _alertKey = AlertKey;
                _alertID = AlertID;
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
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "TASAlert_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@AlertKey", criteria._alertKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Using data reader as record set.
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    //If data reader can read, continue...
                    while (dr.Read())
                    {
                        retValue = this.Fetch(dr);
                    }
                    if (!retValue)
                        return false;
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
            _alertKey = dr.GetInt32("AlertKey");
            _alertID = dr.GetString("AlertID");
            _alertDes = dr.GetString("AlertDes");
            _alertApplyGrp = dr.GetInt32("AlertApplyGrp");
            _alertApplyTo = dr.GetInt32("AlertApplyTo");
            _alertIDFrom = dr.GetString("AlertIDFrom");
            _alertIDTo = dr.GetString("AlertIDTo");
            _alertCondition = dr.GetString("AlertCondition");
            _alertValueAmt = dr.GetString("AlertValueAmt");
            
            if (GFunc.IsNE(dr.GetValue("AlertValueDate")))
                _alertValueDate = null;
            else
                _alertValueDate = dr.GetDateTime("AlertValueDate");

            if (GFunc.IsNE(dr.GetValue("AlertLastActivateDate")))
                _alertLastActivateDate = null;
            else
                _alertLastActivateDate = dr.GetDateTime("AlertLastActivateDate");

            _recurType = dr.GetInt32("RecurType");
            _RecurIntHourMins = dr.GetInt32("RecurIntHourMins");
            _recurIntDayNum = dr.GetInt32("RecurIntDayNum");
            _recurIntWeekNum = dr.GetInt32("RecurIntWeekNum");
            _recurIntWeekDay = dr.GetInt32("RecurIntWeekDay");
            _recurIntMthNum = dr.GetInt32("RecurIntMthNum");
            _recurIntMthDayNum = dr.GetInt32("RecurIntMthDayNum");
            _recurIntMthWeek = dr.GetInt32("RecurIntMthWeek");
            _recurIntMthDay = dr.GetInt32("RecurIntMthDay");
            _recurIntYearNum = dr.GetInt32("RecurIntYearNum");
            _recurIntYearMthNum = dr.GetInt32("RecurIntYearMthNum");
            _recurIntYearDayNum = dr.GetInt32("RecurIntYearDayNum");
            _recurIntYearMthDay = dr.GetInt32("RecurIntYearMthDay");
            _recurIntYearMthWeek = dr.GetInt32("RecurIntYearMthWeek");
            _retriggersAfterHours = dr.GetDecimal("ReTriggerAfterHours");

            if (GFunc.IsNE(dr.GetValue("NextRunDateTime")))
                _NextRunDateTime = null;
            else
                _NextRunDateTime = dr.GetDateTime("NextRunDateTime");
            _TaskState = dr.GetInt32("TaskState");
            _inActive = dr.GetBoolean("InActive");

            if (GFunc.IsNE(dr.GetValue("CreateDate")))
                _createDate = null;
            else
                _createDate = dr.GetDateTime("CreateDate");
            _createUserKey = dr.GetInt32("CreateUserKey");
            if (GFunc.IsNE(dr.GetValue("LastModifiedDate")))
                _lastModifiedDate = null;
            else
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

        internal bool Insert(out int newAlertKey)
        {
            bool retValue = false;            
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn,out newAlertKey);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }
        internal bool Insert(SqlConnection cn, out int newAlertKey)
        {
            newAlertKey = 0;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "TASAlert_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@NewAlertKey", newAlertKey);
                cm.Parameters["@NewAlertKey"].Direction = ParameterDirection.Output;

                if (_alertKey == null)
                {
                    cm.Parameters.AddWithValue("@AlertKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertKey", _alertKey);
                }
                if (_alertID == null)
                {
                    cm.Parameters.AddWithValue("@AlertID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertID", _alertID);
                }
                if (_alertDes == null)
                {
                    cm.Parameters.AddWithValue("@AlertDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertDes", _alertDes);
                }
                if (_alertApplyGrp == null)
                {
                    cm.Parameters.AddWithValue("@AlertApplyGrp", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertApplyGrp", _alertApplyGrp);
                }
                if (_alertApplyTo == null)
                {
                    cm.Parameters.AddWithValue("@AlertApplyTo", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertApplyTo", _alertApplyTo);
                }
                if (_alertIDFrom == null)
                {
                    cm.Parameters.AddWithValue("@AlertIDFrom", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertIDFrom", _alertIDFrom);
                }
                if (_alertIDTo == null)
                {
                    cm.Parameters.AddWithValue("@AlertIDTo", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertIDTo", _alertIDTo);
                }
                if (_alertCondition == null)
                {
                    cm.Parameters.AddWithValue("@AlertCondition", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertCondition", _alertCondition);
                }
                if (_alertValueAmt == null)
                {
                    cm.Parameters.AddWithValue("@AlertValueAmt", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertValueAmt", _alertValueAmt);
                }
                if (_alertValueDate == null)
                {
                    cm.Parameters.AddWithValue("@AlertValueDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertValueDate", _alertValueDate);
                }
                if (_alertLastActivateDate == null)
                {
                    cm.Parameters.AddWithValue("@AlertLastActivateDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertLastActivateDate", _alertLastActivateDate);
                }
                if (_recurType == null)
                {
                    cm.Parameters.AddWithValue("@RecurType", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurType", _recurType);
                }
                if (_RecurIntHourMins == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntHourMins", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntHourMins", _RecurIntHourMins);
                }
                if (_recurIntDayNum == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntDayNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntDayNum", _recurIntDayNum);
                }
                if (_recurIntWeekNum == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntWeekNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntWeekNum", _recurIntWeekNum);
                }
                if (_recurIntWeekDay == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntWeekDay", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntWeekDay", _recurIntWeekDay);
                }
                if (_recurIntMthNum == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntMthNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntMthNum", _recurIntMthNum);
                }
                if (_recurIntMthDayNum == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntMthDayNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntMthDayNum", _recurIntMthDayNum);
                }
                if (_recurIntMthWeek == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntMthWeek", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntMthWeek", _recurIntMthWeek);
                }
                if (_recurIntMthDay == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntMthDay", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntMthDay", _recurIntMthDay);
                }
                if (_recurIntYearNum == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntYearNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntYearNum", _recurIntYearNum);
                }
                if (_recurIntYearMthNum == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntYearMthNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntYearMthNum", _recurIntYearMthNum);
                }
                if (_recurIntYearDayNum == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntYearDayNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntYearDayNum", _recurIntYearDayNum);
                }
                if (_recurIntYearMthDay == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntYearMthDay", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntYearMthDay", _recurIntYearMthDay);
                }
                if (_recurIntYearMthWeek == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntYearMthWeek", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntYearMthWeek", _recurIntYearMthWeek);
                }
                if (_retriggersAfterHours == null)
                {
                    cm.Parameters.AddWithValue("@ReTriggerAfterHours", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ReTriggerAfterHours", _retriggersAfterHours);
                }
                if (_NextRunDateTime == null)
                {
                    cm.Parameters.AddWithValue("@NextRunDateTime", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@NextRunDateTime", _NextRunDateTime);
                }
                if (_TaskState == null)
                {
                    cm.Parameters.AddWithValue("@TaskState", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@TaskState", _TaskState);
                }
                if (_inActive == null)
                {
                    cm.Parameters.AddWithValue("@InActive", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@InActive", _inActive);
                }
                if (_createDate == null)
                {
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@CreateDate", _createDate);
                }
                if (_createUserKey == null)
                {
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@CreateUserKey", _createUserKey);
                }
                if (_lastModifiedDate == null)
                {
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate);
                }
                if (_lastModifiedUserKey == null)
                {
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", _lastModifiedUserKey);
                }
                if (_custom1 == null)
                {
                    cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom1", _custom1);
                }
                if (_custom2 == null)
                {
                    cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom2", _custom2);
                }
                if (_custom3 == null)
                {
                    cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom3", _custom3);
                }
                if (_custom4 == null)
                {
                    cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom4", _custom4);
                }
                if (_custom5 == null)
                {
                    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom5", _custom5);
                }

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                // Execute command.
                cm.ExecuteNonQuery();
                newAlertKey=(int)cm.Parameters["@NewAlertKey"].Value;
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql command.                
        }
        #endregion Insert

        #region Data Access - Update

        internal bool Update()
        {
            bool retValue = false;
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Update(cn);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }
        internal bool Update(SqlConnection cn)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "TASAlert_AddUpdate";
                cm.Parameters.AddWithValue("@NewAlertKey", 0);
                cm.Parameters.AddWithValue("@Option", 1);

                if (_alertKey == null)
                {
                    cm.Parameters.AddWithValue("@AlertKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertKey", _alertKey);
                }
                if (_alertID == null)
                {
                    cm.Parameters.AddWithValue("@AlertID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertID", _alertID);
                }
                if (_alertDes == null)
                {
                    cm.Parameters.AddWithValue("@AlertDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertDes", _alertDes);
                }
                if (_alertApplyGrp == null)
                {
                    cm.Parameters.AddWithValue("@AlertApplyGrp", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertApplyGrp", _alertApplyGrp);
                }
                if (_alertApplyTo == null)
                {
                    cm.Parameters.AddWithValue("@AlertApplyTo", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertApplyTo", _alertApplyTo);
                }
                if (_alertIDFrom == null)
                {
                    cm.Parameters.AddWithValue("@AlertIDFrom", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertIDFrom", _alertIDFrom);
                }
                if (_alertIDTo == null)
                {
                    cm.Parameters.AddWithValue("@AlertIDTo", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertIDTo", _alertIDTo);
                }
                if (_alertCondition == null)
                {
                    cm.Parameters.AddWithValue("@AlertCondition", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertCondition", _alertCondition);
                }
                if (_alertValueAmt == null)
                {
                    cm.Parameters.AddWithValue("@AlertValueAmt", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertValueAmt", _alertValueAmt);
                }
                if (_alertValueDate == null)
                {
                    cm.Parameters.AddWithValue("@AlertValueDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertValueDate", _alertValueDate);
                }
                if (_alertLastActivateDate == null)
                {
                    cm.Parameters.AddWithValue("@AlertLastActivateDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertLastActivateDate", _alertLastActivateDate);
                }
                if (_recurType == null)
                {
                    cm.Parameters.AddWithValue("@RecurType", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurType", _recurType);
                }
                if (_RecurIntHourMins == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntHourMins", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntHourMins", _RecurIntHourMins);
                }
                if (_recurIntDayNum == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntDayNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntDayNum", _recurIntDayNum);
                }
                if (_recurIntWeekNum == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntWeekNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntWeekNum", _recurIntWeekNum);
                }
                if (_recurIntWeekDay == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntWeekDay", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntWeekDay", _recurIntWeekDay);
                }
                if (_recurIntMthNum == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntMthNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntMthNum", _recurIntMthNum);
                }
                if (_recurIntMthDayNum == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntMthDayNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntMthDayNum", _recurIntMthDayNum);
                }
                if (_recurIntMthWeek == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntMthWeek", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntMthWeek", _recurIntMthWeek);
                }
                if (_recurIntMthDay == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntMthDay", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntMthDay", _recurIntMthDay);
                }
                if (_recurIntYearNum == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntYearNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntYearNum", _recurIntYearNum);
                }
                if (_recurIntYearMthNum == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntYearMthNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntYearMthNum", _recurIntYearMthNum);
                }
                if (_recurIntYearDayNum == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntYearDayNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntYearDayNum", _recurIntYearDayNum);
                }
                if (_recurIntYearMthDay == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntYearMthDay", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntYearMthDay", _recurIntYearMthDay);
                }
                if (_recurIntYearMthWeek == null)
                {
                    cm.Parameters.AddWithValue("@RecurIntYearMthWeek", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurIntYearMthWeek", _recurIntYearMthWeek);
                }
                if (_retriggersAfterHours == null)
                {
                    cm.Parameters.AddWithValue("@ReTriggerAfterHours", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ReTriggerAfterHours", _retriggersAfterHours);
                }
                if (_NextRunDateTime == null)
                {
                    cm.Parameters.AddWithValue("@NextRunDateTime", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@NextRunDateTime", _NextRunDateTime);
                }
                if (_TaskState == null)
                {
                    cm.Parameters.AddWithValue("@TaskState", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@TaskState", _TaskState);
                }
                if (_inActive == null)
                {
                    cm.Parameters.AddWithValue("@InActive", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@InActive", _inActive);
                }
                if (_createDate == null)
                {
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@CreateDate", _createDate);
                }
                if (_createUserKey == null)
                {
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@CreateUserKey", _createUserKey);
                }
                if (_lastModifiedDate == null)
                {
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate);
                }
                if (_lastModifiedUserKey == null)
                {
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", _lastModifiedUserKey);
                }
                if (_custom1 == null)
                {
                    cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom1", _custom1);
                }
                if (_custom2 == null)
                {
                    cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom2", _custom2);
                }
                if (_custom3 == null)
                {
                    cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom3", _custom3);
                }
                if (_custom4 == null)
                {
                    cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom4", _custom4);
                }
                if (_custom5 == null)
                {
                    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom5", _custom5);
                }


                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();               

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;

            }// Already close and dispose sql command.

        }
        #endregion Update

        #region Data Access - Delete

        internal bool Delete(Criteria criteria)
        {
            bool retValue = false;            
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Delete(cn, criteria);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }
        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "TASAlert_Delete";
                                
                cm.Parameters.AddWithValue("@AlertKey", criteria._alertKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();
                
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
            }// Already close and dispose sql command.

            return retValue;
        }
        #endregion Delete

        #region Data Access - Validation

        internal bool Validation(Criteria criteria, bool isNew)
        {
            bool retValue = false;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();
                        retValue = Validation(cn, criteria, isNew);
                    }
                    if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  
                        throw new Exception("Transaction has aborted."); scope.Complete();
                }
            }
            catch (TAException taex)
            {
                throw taex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retValue;
        }
        internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
        {   
            try
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "TASAlert_Validation";

                    cm.Parameters.AddWithValue("@isNew", isNew);
                    cm.Parameters.AddWithValue("@AlertKey", criteria._alertKey);                 
                    cm.Parameters.AddWithValue("@AlertID", criteria._alertID);
                    cm.Parameters.AddWithValue("@RetValue", 0);

                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    cm.ExecuteNonQuery();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        return true;
                    else
                        return false;
                }
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Validation
    }
}





