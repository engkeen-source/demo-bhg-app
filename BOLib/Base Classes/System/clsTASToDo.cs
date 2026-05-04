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
	/// <summary>
	/// Summary description for TASToDo.
	/// </summary>
	[Serializable()]
    public class TASToDo : Csla.BusinessBase<TASToDo>
    {
        #region Business Properties and Methods
        internal int _toDoKey;
        internal string _toDoDes;
        internal int _toDoPriority;
        internal int _toDoType;
        internal DateTime? _dateStart;
        internal DateTime? _timeStart;
        internal DateTime? _dateTarget;
        internal DateTime? _dateEnd;
        internal int? _remindType;
        internal DateTime? _remindDate;
        internal Int16? _remindDayBefore;
        internal Int16? _remindHourBefore;
        internal int? _recurType;
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
        internal int? _docDC;
        internal int? _docDK;
        internal string _docID;
        internal bool _inActive;
        internal DateTime? _createDate;
        internal int? _createUserKey;
        internal DateTime? _lastModifiedDate;
        internal int? _lastModifiedUserKey;
        internal string _custom1;
        internal string _custom2;
        internal string _custom3;
        internal int? _repKey;
        internal string _repFileNm;

        public int ToDoKey
        {

            get
            {
                return this._toDoKey;
            }
            set
            {
                this._toDoKey = value;
                PropertyHasChanged("ToDoKey");
            }
        }
        public string ToDoDes
        {

            get
            {
                return this._toDoDes;
            }
            set
            {
                this._toDoDes = value;
                PropertyHasChanged("ToDoDes");
            }
        }
        public int ToDoPriority
        {

            get
            {
                return this._toDoPriority;
            }
            set
            {
                this._toDoPriority = value;
                PropertyHasChanged("ToDoPriority");
            }
        }
        public int ToDoType
        {

            get
            {
                return this._toDoType;
            }
            set
            {
                this._toDoType = value;
                PropertyHasChanged("ToDoType");
            }
        }
        public DateTime? DateStart
        {

            get
            {
                return this._dateStart;
            }
            set
            {
                this._dateStart = value;
                PropertyHasChanged("DateStart");
            }
        }
        public DateTime? TimeStart
        {

            get
            {
                return this._timeStart;
            }
            set
            {
                this._timeStart = value;
                PropertyHasChanged("TimeStart");
            }
        }
        public DateTime? DateTarget
        {

            get
            {
                return this._dateTarget;
            }
            set
            {
                this._dateTarget = value;
                PropertyHasChanged("DateTarget");
            }
        }
        public DateTime? DateEnd
        {

            get
            {
                return this._dateEnd;
            }
            set
            {
                this._dateEnd = value;
                PropertyHasChanged("DateEnd");
            }
        }
        public int? RemindType
        {

            get
            {
                return this._remindType;
            }
            set
            {
                this._remindType = value;
                PropertyHasChanged("RemindType");
            }
        }
        public DateTime? RemindDate
        {

            get
            {
                return this._remindDate;
            }
            set
            {
                this._remindDate = value;
                PropertyHasChanged("RemindDate");
            }
        }
        public Int16? RemindDayBefore
        {

            get
            {
                return this._remindDayBefore;
            }
            set
            {
                this._remindDayBefore = value;
                PropertyHasChanged("RemindDayBefore");
            }
        }
        public Int16? RemindHourBefore
        {

            get
            {
                return this._remindHourBefore;
            }
            set
            {
                this._remindHourBefore = value;
                PropertyHasChanged("RemindHourBefore");
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
        public int? DocDC
        {

            get
            {
                return this._docDC;
            }
            set
            {
                this._docDC = value;
                PropertyHasChanged("DocDC");
            }
        }
        public int? DocDK
        {

            get
            {
                return this._docDK;
            }
            set
            {
                this._docDK = value;
                PropertyHasChanged("DocDK");
            }
        }
        public string DocID
        {
            get
            {
                return this._docID;
            }
            set
            {
                this._docID = value;
                PropertyHasChanged("DocID");
            }
        }
        public bool InActive
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

        public int? RepKey
        {
            get
            {
                return this._repKey;
            }
            set
            {
                this._repKey = value;
                PropertyHasChanged("RepKey");
            }
        }
        public string RepFileNm
        {
            get
            {
                return this._repFileNm;
            }
            set
            {
                this._repFileNm = value;
                PropertyHasChanged("RepFileNm");
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
        public TASToDo()
        {           
            this._toDoKey = 0;
            this._toDoDes = string.Empty;
            this._toDoPriority = 10;
            this._toDoType = 10;
            this._dateStart = DateTime.Today.Date;
            this._timeStart =Convert.ToDateTime("3:00 AM");
            this._dateTarget = DateTime.Today.Date;
            this._dateEnd = DateTime.Today.Date;
            this._remindType = 0;
            this._remindDate = DateTime.Today.Date;
            this._remindDayBefore = null;
            this._remindHourBefore = null;
            this._recurType = 10;
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
            this._docDC = 0;
            this._docDK = 0;
            this._docID = string.Empty;
            this._inActive = false;
                  
        }
		public static TASToDo Get(int? ToDoKey)
        {           
            TASToDo child = new TASToDo();
            child.Fetch(new Criteria(ToDoKey, 1));
            return child;
        }        		
		public static TASToDo New()
        {
             TASToDo child = new TASToDo();   
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
                public int? _DocCodeKey = null;
				public int? _AlertKey = null;
				public int? _option = null;
                public string _DocID = string.Empty;
               
                internal Criteria()
				{
				}
				internal Criteria(int? AlertKey)
				{
					_AlertKey = AlertKey;
				}
                internal Criteria(int? AlertKey, int? Option)
                {
                    _AlertKey = AlertKey;
                    _option = Option;
                }
                internal Criteria(int DocCodeKey, int? AlertKey, int? Option)
				{
                    _DocCodeKey = DocCodeKey;
					_AlertKey = AlertKey;
					_option = Option;
				}
                internal Criteria(int? DocCodeKey, int? AlertKey, string DocID, int? Option)
                {
                    _DocCodeKey = DocCodeKey;
                    _AlertKey = AlertKey;
                    _DocID = DocID;
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
		internal bool Fetch(SqlConnection cn,Criteria criteria)
		{
			bool retValue = false;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "TASToDo_Get";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@ToDoKey", criteria._AlertKey);                
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
                    retValue=false;
            }// Already close and dispose sql connection.
            
			return retValue;
		}
		internal bool Fetch(SafeDataReader dr)
		{
            _toDoKey = dr.GetInt32("ToDoKey");
			_toDoDes = dr.GetString("ToDoDes");
			_toDoPriority = dr.GetInt32("ToDoPriority");
			_toDoType = dr.GetInt32("ToDoType");
            _repKey = dr.GetInt32("Repkey");
            _repFileNm = dr.GetString("RepFileNm");
            if (GFunc.IsNE(dr.GetValue("DateStart")))
                _dateStart = null;
            else
                _dateStart = dr.GetDateTime("DateStart");
            if (GFunc.IsNE(dr.GetValue("TimeStart")))
                _timeStart = null;
            else
                _timeStart = dr.GetDateTime("TimeStart");
            if (GFunc.IsNE(dr.GetValue("DateTarget")))
                _dateTarget = null;
            else
                _dateTarget = dr.GetDateTime("DateTarget");
            if (GFunc.IsNE(dr.GetValue("DateEnd")))
                _dateEnd = null;
            else
                _dateEnd = dr.GetDateTime("DateEnd");			
			_remindType = dr.GetInt32("RemindType");
            if (GFunc.IsNE(dr.GetValue("RemindDate")))
                _remindDate = null;
            else
                _remindDate = dr.GetDateTime("RemindDate");				
			_remindDayBefore = dr.GetInt16("RemindDayBefore");
			_remindHourBefore = dr.GetInt16("RemindHourBefore");
			_recurType = dr.GetInt32("RecurType");
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
			_docDC =dr.GetInt32("DocDC");				
			_docDK =dr.GetInt32("DocDK");
            _docID= dr.GetString("DocID");
            _inActive = dr.GetBoolean("Inactive");
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
			 
            return true;
		}
		#endregion //Data Access - Fetch

		#region Data Access - Insert

        internal bool Insert(out int NewToDoKey)
		{
			bool retValue = false;
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn, out NewToDoKey);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            
            return retValue;
		}
		
        internal bool Insert(SqlConnection cn,out int NewToDoKey)
		{
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "TASToDo_AddUpdate";
                cm.Parameters.AddWithValue("@NewToDoKey", 0);
                cm.Parameters["@NewToDoKey"].Direction = ParameterDirection.Output;
                cm.Parameters.AddWithValue("@Option", 0);
				
				if (_toDoKey == null)
                {
                    cm.Parameters.AddWithValue("@ToDoKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ToDoKey", _toDoKey);
                }
                if (_toDoDes == null)
                {
                    cm.Parameters.AddWithValue("@ToDoDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ToDoDes", _toDoDes);
                }
                if (_toDoPriority == null)
                {
                    cm.Parameters.AddWithValue("@ToDoPriority", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ToDoPriority", _toDoPriority);
                }
                if (_toDoType == null)
                {
                    cm.Parameters.AddWithValue("@ToDoType", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ToDoType", _toDoType);
                }
                if (_repKey == null)
                {
                    cm.Parameters.AddWithValue("@RepKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RepKey", _repKey);
                }
                if (_repFileNm == null)
                {
                    cm.Parameters.AddWithValue("@RepFileNm", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RepFileNm", _repFileNm);
                }
                if (_dateStart == null)
                {
                    cm.Parameters.AddWithValue("@DateStart", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DateStart", _dateStart);
                }
                if (_timeStart == null)
                {
                    cm.Parameters.AddWithValue("@TimeStart", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@TimeStart", _timeStart);
                }
                if (_dateTarget == null)
                {
                    cm.Parameters.AddWithValue("@DateTarget", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DateTarget", _dateTarget);
                }
                if (_dateEnd == null)
                {
                    cm.Parameters.AddWithValue("@DateEnd", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DateEnd", _dateEnd);
                }
                if (_remindType == null)
                {
                    cm.Parameters.AddWithValue("@RemindType", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RemindType", _remindType);
                }
                if (_remindDate == null)
                {
                    cm.Parameters.AddWithValue("@RemindDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RemindDate", _remindDate);
                }
                if (_remindDayBefore == null)
                {
                    cm.Parameters.AddWithValue("@RemindDayBefore", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RemindDayBefore", _remindDayBefore);
                }
                if (_remindHourBefore == null)
                {
                    cm.Parameters.AddWithValue("@RemindHourBefore", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RemindHourBefore", _remindHourBefore);
                }
                if (_recurType == null)
                {
                    cm.Parameters.AddWithValue("@RecurType", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurType", _recurType);
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
                if (_docDC == null)
                {
                    cm.Parameters.AddWithValue("@DocDC", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDC", _docDC);
                }
                if (_docDK == null)
                {
                    cm.Parameters.AddWithValue("@DocDK", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDK", _docDK);
                }
                if (_docID == null)
                {
                    cm.Parameters.AddWithValue("@DocID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocID", _docID);
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
                                    
                
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                // Execute command.
                cm.ExecuteNonQuery();
                NewToDoKey=(int)cm.Parameters["@NewToDoKey"].Value;
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
			using(SqlCommand cm = cn.CreateCommand())
			{
				cm.CommandType = CommandType.StoredProcedure;
				cm.CommandText = "TASToDo_AddUpdate";
                cm.Parameters.AddWithValue("@NewToDoKey", 0);
				cm.Parameters.AddWithValue("@Option", 1);
				
				if (_toDoKey == null)
                {
                    cm.Parameters.AddWithValue("@ToDoKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ToDoKey", _toDoKey);
                }
                if (_toDoDes == null)
                {
                    cm.Parameters.AddWithValue("@ToDoDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ToDoDes", _toDoDes);
                }
                if (_toDoPriority == null)
                {
                    cm.Parameters.AddWithValue("@ToDoPriority", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ToDoPriority", _toDoPriority);
                }
                if (_toDoType == null)
                {
                    cm.Parameters.AddWithValue("@ToDoType", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ToDoType", _toDoType);
                }
                if (_repKey == null)
                {
                    cm.Parameters.AddWithValue("@RepKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RepKey", _repKey);
                }
                if (_repFileNm == null)
                {
                    cm.Parameters.AddWithValue("@RepFileNm", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RepFileNm", _repFileNm);
                }
                if (_dateStart == null)
                {
                    cm.Parameters.AddWithValue("@DateStart", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DateStart", _dateStart);
                }
                if (_timeStart == null)
                {
                    cm.Parameters.AddWithValue("@TimeStart", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@TimeStart", _timeStart);
                }
                if (_dateTarget == null)
                {
                    cm.Parameters.AddWithValue("@DateTarget", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DateTarget", _dateTarget);
                }
                if (_dateEnd == null)
                {
                    cm.Parameters.AddWithValue("@DateEnd", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DateEnd", _dateEnd);
                }
                if (_remindType == null)
                {
                    cm.Parameters.AddWithValue("@RemindType", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RemindType", _remindType);
                }
                if (_remindDate == null)
                {
                    cm.Parameters.AddWithValue("@RemindDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RemindDate", _remindDate);
                }
                if (_remindDayBefore == null)
                {
                    cm.Parameters.AddWithValue("@RemindDayBefore", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RemindDayBefore", _remindDayBefore);
                }
                if (_remindHourBefore == null)
                {
                    cm.Parameters.AddWithValue("@RemindHourBefore", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RemindHourBefore", _remindHourBefore);
                }
                if (_recurType == null)
                {
                    cm.Parameters.AddWithValue("@RecurType", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RecurType", _recurType);
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
                if (_docDC == null)
                {
                    cm.Parameters.AddWithValue("@DocDC", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDC", _docDC);
                }
                if (_docDK == null)
                {
                    cm.Parameters.AddWithValue("@DocDK", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDK", _docDK);
                }
                if (_docID == null)
                {
                    cm.Parameters.AddWithValue("@DocID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocID", _docID);
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
					retValue = this.Delete(cn,criteria);
				}
				// No errors - commit transaction
				  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
			}// Already close and dispose sql connection.
			
			return retValue;
		}
		internal bool Delete(SqlConnection cn, Criteria criteria)
		{
			bool retValue = false;
			using(SqlCommand cm = cn.CreateCommand())
			{
				cm.CommandType = CommandType.StoredProcedure;
				cm.CommandText = "TASToDo_Delete";

				cm.Parameters.AddWithValue("@ToDoKey", criteria._AlertKey);
				
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
        //Note there are no validation for ToDo, the below validation is kept for futurefeature, currently it is notused at all
		internal bool Validation(Criteria criteria,bool isNew)
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
		internal bool Validation(SqlConnection cn, Criteria criteria,bool isNew)
		{
            try
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "TASToDo_Validation";

                    cm.Parameters.AddWithValue("@isNew", isNew);
                    cm.Parameters.AddWithValue("@DocCodeKey", criteria._DocCodeKey);
                    cm.Parameters.AddWithValue("@ToDoKey", criteria._AlertKey);
                    cm.Parameters.AddWithValue("@DocID", criteria._DocID);
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
      
         

          
         
           