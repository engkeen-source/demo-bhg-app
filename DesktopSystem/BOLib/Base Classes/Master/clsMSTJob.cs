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
    public class MSTJob : Csla.BusinessBase<MSTJob>, System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _jobKey = 0;
        internal string _jobID = string.Empty;
        internal string _jobDes = string.Empty;
        internal string _jobRem = string.Empty;
        internal int? _jobGrpKey = 0;
        internal string _jobGrpID = string.Empty;
        internal int? _jobConKey = 0;
        internal string _jobConID = string.Empty;
        internal string _jobConNm = string.Empty;
        internal string _jobClass = string.Empty;
        internal string _jobPOID = string.Empty;
        internal DateTime? _jobPODate = null;
        internal string _jobSupervisor = string.Empty;
        internal string _jobContact = string.Empty;
        internal int? _jobEMKey = null;
        internal string _jobShipName = string.Empty;
        internal string _jobShipMark = string.Empty;
        internal DateTime? _jobStartDate = null;
        internal DateTime? _jobTgtDate = null;
        internal DateTime? _jobEndDate = null;
        internal string _jobMemo = string.Empty;
        internal int? _jobStatus = 10;
        internal bool? _jobAttachment = false;
        internal decimal? _contractAmt = 0;
        internal decimal? _retaintionAmt = 0;
        internal DateTime? _retaintionDate = null;
        internal int? _accessLevel = 0;
        internal int? _accessGroup = 0;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal int? _purgeKeep = 0;
        internal bool? _purgeData = false;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _custom4 = string.Empty;
        internal string _custom5 = string.Empty;
        internal string _error = string.Empty;
        internal decimal? _minMarkupSalePercent = 0;
        internal decimal? _maxMarkupSalePercent = 0;
        internal decimal? _projectCostPercent = 0;
        private SYSAttachments attachments= new SYSAttachments();

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

        public int? JobKey
        {
            get
            {
                return _jobKey;
            }
            set
            {
                _jobKey = value;
                PropertyHasChanged("JobKey");
            }
        }

        public string JobID
        {
            get
            {
                return _jobID;
            }
            set
            {
                _jobID = value;
                PropertyHasChanged("JobID");
            }
        }

        public string JobDes
        {
            get
            {
                return _jobDes;
            }
            set
            {
                _jobDes = value;
                PropertyHasChanged("JobDes");
            }
        }

        public string JobRem
        {
            get
            {
                return _jobRem;
            }
            set
            {
                _jobRem = value;
                PropertyHasChanged("JobRem");
            }
        }

        public int? JobGrpKey
        {
            get
            {
                return _jobGrpKey;
            }
            set
            {
                _jobGrpKey = value;
                PropertyHasChanged("JobGrpKey");
            }
        }

        public string JobGrpID
        {
            get
            {
                return _jobGrpID;
            }
            set
            {
                _jobGrpID = value;
                PropertyHasChanged("JobGrpID");
            }
        }

        public int? JobConKey
        {
            get
            {
                return _jobConKey;
            }
            set
            {
                _jobConKey = value;
                PropertyHasChanged("JobConKey");
            }
        }

        public string JobConID
        {
            get
            {
                return _jobConID;
            }
            set
            {
                _jobConID = value;
                PropertyHasChanged("JobConID");
            }
        }

        public string JobConNm
        {

            get
            {
                return this._jobConNm;
            }
            set
            {
                this._jobConNm = value;
                PropertyHasChanged("JobConNm");
            }
        }

        public string JobClass
        {
            get
            {
                return _jobClass;
            }
            set
            {
                _jobClass = value;
                PropertyHasChanged("JobClass");
            }
        }

        public string JobPOID
        {
            get
            {
                return _jobPOID;
            }
            set
            {
                _jobPOID = value;
                PropertyHasChanged("JobPOID");
            }
        }

        public DateTime? JobPODate
        {
            get
            {
                return _jobPODate;
            }
            set
            {
                if (value != null && _jobPODate != value)
                {
                    _jobPODate = value;
                    PropertyHasChanged("JobPODate");
                }
            }
        }

        public string JobSupervisor
        {
            get
            {
                return _jobSupervisor;
            }
            set
            {
                _jobSupervisor = value;
                PropertyHasChanged("JobSupervisor");
            }
        }

        public string JobContact
        {
            get
            {
                return _jobContact;
            }
            set
            {
                _jobContact = value;
                PropertyHasChanged("JobContact");
            }
        }

        public int? JobEMKey
        {
            get
            {
                return _jobEMKey;
            }
            set
            {
                _jobEMKey = value;
                PropertyHasChanged("JobEMKey");
            }
        }

        public string JobShipName
        {
            get
            {
                return _jobShipName;
            }
            set
            {
                _jobShipName = value;
                PropertyHasChanged("JobShipName");
            }
        }

        public string JobShipMark
        {
            get
            {
                return _jobShipMark;
            }
            set
            {
                _jobShipMark = value;
                PropertyHasChanged("JobShipMark");
            }
        }

        public DateTime? JobStartDate
        {
            get
            {
                return _jobStartDate;
            }
            set
            {
                if (value != null && _jobStartDate != value)
                {
                    _jobStartDate = value;
                    PropertyHasChanged("JobStartDate");
                }
            }
        }

        public DateTime? JobTgtDate
        {
            get
            {
                return _jobTgtDate;
            }
            set
            {
                if (value != null && _jobTgtDate != value)
                {
                    _jobTgtDate = value;
                    PropertyHasChanged("JobTgtDate");
                }
            }
        }

        public DateTime? JobEndDate
        {
            get
            {
                return _jobEndDate;
            }
            set
            {
                if (value != null && _jobEndDate != value)
                {
                    _jobEndDate = value;
                    PropertyHasChanged("JobEndDate");
                }
            }
        }

        public string JobMemo
        {
            get
            {
                return _jobMemo;
            }
            set
            {
                _jobMemo = value;
                PropertyHasChanged("JobMemo");
            }
        }

        public int? JobStatus
        {
            get
            {
                return _jobStatus;
            }
            set
            {
                _jobStatus = value;
                PropertyHasChanged("JobStatus");
            }
        }

        public bool? JobAttachment
        {
            get
            {
                return _jobAttachment;
            }
            set
            {
                _jobAttachment = value;
                PropertyHasChanged("JobAttachment");
            }
        }

        public decimal? ContractAmt
        {
            get
            {
                return _contractAmt;
            }
            set
            {
                _contractAmt = value;
                PropertyHasChanged("ContractAmt");
            }
        }

        public decimal? RetaintionAmt
        {
            get
            {
                return _retaintionAmt;
            }
            set
            {
                _retaintionAmt = value;
                PropertyHasChanged("RetaintionAmt");
            }
        }

        public DateTime? RetaintionDate
        {
            get
            {
                return _retaintionDate;
            }
            set
            {
                if (value != null && _retaintionDate != value)
                {
                    _retaintionDate = value;
                    PropertyHasChanged("RetaintionDate");
                }
            }
        }

        public decimal? MinMarkupSalePercent
        {
            get
            {
                return _minMarkupSalePercent;
            }
            set
            {
                _minMarkupSalePercent = value;
                PropertyHasChanged("MinMarkupSalePercent");
            }
        }

        public decimal? MaxMarkupSalePercent
        {
            get
            {
                return _maxMarkupSalePercent;
            }
            set
            {
                _maxMarkupSalePercent = value;
                PropertyHasChanged("MaxMarkupSalePercent");
            }
        }
        public decimal? ProjectCostPercent
        {
            get
            {
                return _projectCostPercent;
            }
            set
            {
                _projectCostPercent = value;
                PropertyHasChanged("ProjectCostPercent");
            }
        }
        public int? AccessLevel
        {
            get
            {
                return _accessLevel;
            }
            set
            {
                _accessLevel = value;
                PropertyHasChanged("AccessLevel");
            }
        }

        public int? AccessGroup
        {
            get
            {
                return _accessGroup;
            }
            set
            {
                _accessGroup = value;
                PropertyHasChanged("AccessGroup");
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

        public int? PurgeKeep
        {
            get
            {
                return _purgeKeep;
            }
            set
            {
                _purgeKeep = value;
                PropertyHasChanged("PurgeKeep");
            }
        }

        public bool? PurgeData
        {
            get
            {
                return _purgeData;
            }
            set
            {
                _purgeData = value;
                PropertyHasChanged("PurgeData");
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

        public SYSAttachments Attachments
        {
            get { return attachments; }
            set { attachments = value; }
        }


        protected override object GetIdValue()
        {
            return _jobKey.ToString();
        }

        #endregion //Business Properties and Methods

        #region Validation Rules
        private void AddCustomRules()
        {
            //add custom/non-generated rules here...
        }

        private void AddCommonRules()
        {
            /*
           //
           // JobID
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "JobID");
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("JobID", 50));
           //
           // JobDes
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "JobDes");
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("JobDes", 255));
           //
           // JobGrpID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("JobGrpID", 50));
           //
           // JobConID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("JobConID", 50));
           //
           // JobClass
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("JobClass", 50));
           //
           // JobPOID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("JobPOID", 50));
           //
           // JobSupervisor
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("JobSupervisor", 50));
           //
           // JobContact
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("JobContact", 50));
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
           //
           // Custom4
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom4", 255));
           //
           // Custom5
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom5", 255));
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

        internal MSTJob()
        { /* require use of factory method */ }

        internal static MSTJob New()
        {
            
            MSTJob child = new MSTJob();
            
            return child;
        }

        internal static MSTJob NewChild()
        {
            
            MSTJob child = new MSTJob();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static MSTJob Get(SafeDataReader dr)
        {
            MSTJob child = new MSTJob();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static MSTJob Get(int? jobKey)
        {
            MSTJob child = new MSTJob();
            child.Fetch(new Criteria(jobKey, 1));
            return child;
        }

        public static MSTJob Get(string jobID)
        {
            MSTJob child = new MSTJob();
            child.Fetch(new Criteria(jobID, 2));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _jobKey = null;
            public int? _option = null;
            public string _jobID = string.Empty;

            internal Criteria()
            {
            }

            internal Criteria(int? JobKey)
            {
                _jobKey = JobKey;
            }

            internal Criteria(string JobID)
            {
                _jobID = JobID;
            }

            internal Criteria(int? JobKey, string JobID)
            {
                _jobKey = JobKey;
                _jobID = JobID;
            }

            internal Criteria(int? JobKey, int? Option)
            {
                _jobKey = JobKey;
                _option = Option;
            }

            internal Criteria(string JobID,int? Option)
            {
                _jobID = JobID;
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
                cm.CommandText = "MSTJob_Get";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                if (!GFunc.IsNEZ(criteria._jobKey))
                    cm.Parameters.AddWithValue("@JobKey", criteria._jobKey);
                else
                    cm.Parameters.AddWithValue("@JobKey", DBNull.Value);

                if (GFunc.NEStr(criteria._jobID,string.Empty) != string.Empty)
                    cm.Parameters.AddWithValue("@JobID", criteria._jobID);
                else
                    cm.Parameters.AddWithValue("@JobID", DBNull.Value);

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
                
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    retValue = true;
                }
                else
                {
                    retValue = false;
                }

            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _jobKey = dr.GetInt32("JobKey");
            _jobID = dr.GetString("JobID");
            _jobDes = dr.GetString("JobDes");
            _jobRem = dr.GetString("JobRem");
            _jobGrpKey = dr.GetInt32("JobGrpKey");
            _jobGrpID = dr.GetString("JobGrpID");
            _jobConKey = dr.GetInt32("JobConKey");
            _jobConID = dr.GetString("JobConID");
            _jobConNm = dr.GetString("JobConNm");
            _jobClass = dr.GetString("JobClass");
            _jobPOID = dr.GetString("JobPOID");

            if (GFunc.IsNE(dr.GetValue("JobPODate")))
                _jobPODate = null;
            else
                _jobPODate = dr.GetDateTime("JobPODate");

            _jobSupervisor = dr.GetString("JobSupervisor");
            _jobContact = dr.GetString("JobContact");
            _jobEMKey = GFunc.NEInt(dr.GetValue("JobEMKey"), 0);
            _jobShipName = dr.GetString("JobShipName");
            _jobShipMark = dr.GetString("JobShipMark");

            if (GFunc.IsNE(dr.GetValue("JobStartDate")))
                _jobStartDate = null;
            else
                _jobStartDate = dr.GetDateTime("JobStartDate");

            if (GFunc.IsNE(dr.GetValue("JobTgtDate")))
                _jobTgtDate = null;
            else
                _jobTgtDate = dr.GetDateTime("JobTgtDate");

            if (GFunc.IsNE(dr.GetValue("JobEndDate")))
                _jobEndDate = null;
            else
                _jobEndDate = dr.GetDateTime("JobEndDate");

            _jobMemo = dr.GetString("JobMemo");
            _jobStatus = dr.GetInt32("JobStatus");
            _jobAttachment = dr.GetBoolean("JobAttachment");
            _contractAmt = dr.GetDecimal("ContractAmt");
            _retaintionAmt = dr.GetDecimal("RetaintionAmt");
            _minMarkupSalePercent = dr.GetDecimal("MinMarkupSalePercent");
            _maxMarkupSalePercent = dr.GetDecimal("MaxMarkupSalePercent");
            _projectCostPercent = dr.GetDecimal("ProjectCostPercent");
            
            if (GFunc.IsNE(dr.GetValue("RetaintionDate")))
                _retaintionDate = null;
            else
                _retaintionDate = dr.GetDateTime("RetaintionDate");
            _accessLevel = dr.GetInt32("AccessLevel");
            _accessGroup = dr.GetInt32("AccessGroup");

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
            _purgeKeep = dr.GetInt32("PurgeKeep");
            _purgeData = dr.GetBoolean("PurgeData");
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

        internal bool Insert(out int? jobKey)
        {
            bool retValue = false;
            jobKey = null;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out jobKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? jobKey)
        {
            jobKey = 0;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTJob_AddUpdate";
                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewJobKey", jobKey);

                if (_jobKey == null)
                    cm.Parameters.AddWithValue("@JobKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobKey", _jobKey);

                if (_jobID == null)
                    cm.Parameters.AddWithValue("@JobID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobID", _jobID);

                if (_jobDes == null)
                    cm.Parameters.AddWithValue("@JobDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobDes", _jobDes);

                if (_jobRem == null)
                    cm.Parameters.AddWithValue("@JobRem", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobRem", _jobRem);

                if (_jobGrpKey == null)
                    cm.Parameters.AddWithValue("@JobGrpKey", 0);
                else
                    cm.Parameters.AddWithValue("@JobGrpKey", _jobGrpKey);

                if (_jobGrpID == null)
                    cm.Parameters.AddWithValue("@JobGrpID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobGrpID", _jobGrpID);

                if (_jobConKey == null)
                    cm.Parameters.AddWithValue("@JobConKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobConKey", _jobConKey);

                if (_jobConID == null)
                    cm.Parameters.AddWithValue("@JobConID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobConID", _jobConID);

                if (_jobClass == null)
                    cm.Parameters.AddWithValue("@JobClass", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobClass", _jobClass);

                if (_jobPOID == null)
                    cm.Parameters.AddWithValue("@JobPOID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobPOID", _jobPOID);

                if (_jobPODate == null)
                    cm.Parameters.AddWithValue("@JobPODate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobPODate", _jobPODate.Value);

                if (_jobSupervisor == null)
                    cm.Parameters.AddWithValue("@JobSupervisor", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobSupervisor", _jobSupervisor);

                if (_jobContact == null)
                    cm.Parameters.AddWithValue("@JobContact", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobContact", _jobContact);

                if (_jobEMKey == null)
                    cm.Parameters.AddWithValue("@JobEMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobEMKey", _jobEMKey);

                if (_jobShipName == null)
                    cm.Parameters.AddWithValue("@JobShipName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobShipName", _jobShipName);

                if (_jobShipMark == null)
                    cm.Parameters.AddWithValue("@JobShipMark", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobShipMark", _jobShipMark);

                if (_jobStartDate == null)
                    cm.Parameters.AddWithValue("@JobStartDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobStartDate", _jobStartDate.Value);

                if (_jobTgtDate == null)
                    cm.Parameters.AddWithValue("@JobTgtDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobTgtDate", _jobTgtDate.Value);

                if (_jobEndDate == null)
                    cm.Parameters.AddWithValue("@JobEndDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobEndDate", _jobEndDate.Value);

                if (_jobMemo == null)
                    cm.Parameters.AddWithValue("@JobMemo", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobMemo", _jobMemo);

                if (_jobStatus == null)
                    cm.Parameters.AddWithValue("@JobStatus", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobStatus", _jobStatus);

                if (_jobAttachment == null)
                    cm.Parameters.AddWithValue("@JobAttachment", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobAttachment", _jobAttachment);

                if (_contractAmt == null)
                    cm.Parameters.AddWithValue("@ContractAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ContractAmt", _contractAmt);

                if (_retaintionAmt == null)
                    cm.Parameters.AddWithValue("@RetaintionAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RetaintionAmt", _retaintionAmt);

                if (_retaintionDate == null)
                    cm.Parameters.AddWithValue("@RetaintionDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RetaintionDate", _retaintionDate.Value);

                if (_accessLevel == null)
                    cm.Parameters.AddWithValue("@AccessLevel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessLevel", _accessLevel);

                if (_accessGroup == null)
                    cm.Parameters.AddWithValue("@AccessGroup", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessGroup", _accessGroup);

                if (_createDate == null)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

                if (!GFunc.IsNEZ(AppInfor.currentUserKey))
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

                if (_purgeKeep == null)
                    cm.Parameters.AddWithValue("@PurgeKeep", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurgeKeep", _purgeKeep);

                if (_purgeData == null)
                    cm.Parameters.AddWithValue("@PurgeData", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurgeData", _purgeData);

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

                //if (_m == null)
                //    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                //else
                //    cm.Parameters.AddWithValue("@Custom5", _custom5);

                //if (_custom5 == null)
                //    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                //else
                //    cm.Parameters.AddWithValue("@Custom5", _custom5);

                //if (_custom5 == null)
                //    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                //else
                //    cm.Parameters.AddWithValue("@Custom5", _custom5);

                cm.Parameters["@NewJobKey"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();
                jobKey = (int)cm.Parameters["@NewJobKey"].Value;

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
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTJob_AddUpdate";
                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewJobKey", 0);

                if (_jobKey == null)
                    cm.Parameters.AddWithValue("@JobKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobKey", _jobKey);

                if (_jobID == null)
                    cm.Parameters.AddWithValue("@JobID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobID", _jobID);

                if (_jobDes == null)
                    cm.Parameters.AddWithValue("@JobDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobDes", _jobDes);

                if (_jobRem == null)
                    cm.Parameters.AddWithValue("@JobRem", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobRem", _jobRem);

                if (_jobGrpKey == null)
                    cm.Parameters.AddWithValue("@JobGrpKey", 0);
                else
                    cm.Parameters.AddWithValue("@JobGrpKey", _jobGrpKey);

                if (_jobGrpID == null)
                    cm.Parameters.AddWithValue("@JobGrpID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobGrpID", _jobGrpID);

                if (_jobConKey == null)
                    cm.Parameters.AddWithValue("@JobConKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobConKey", _jobConKey);

                if (_jobConID == null)
                    cm.Parameters.AddWithValue("@JobConID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobConID", _jobConID);

                if (_jobClass == null)
                    cm.Parameters.AddWithValue("@JobClass", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobClass", _jobClass);

                if (_jobPOID == null)
                    cm.Parameters.AddWithValue("@JobPOID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobPOID", _jobPOID);

                if (_jobPODate == null)
                    cm.Parameters.AddWithValue("@JobPODate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobPODate", _jobPODate.Value);

                if (_jobSupervisor == null)
                    cm.Parameters.AddWithValue("@JobSupervisor", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobSupervisor", _jobSupervisor);

                if (_jobContact == null)
                    cm.Parameters.AddWithValue("@JobContact", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobContact", _jobContact);

                if (_jobEMKey == null)
                    cm.Parameters.AddWithValue("@JobEMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobEMKey", _jobEMKey);

                if (_jobShipName == null)
                    cm.Parameters.AddWithValue("@JobShipName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobShipName", _jobShipName);

                if (_jobShipMark == null)
                    cm.Parameters.AddWithValue("@JobShipMark", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobShipMark", _jobShipMark);

                if (_jobStartDate == null)
                    cm.Parameters.AddWithValue("@JobStartDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobStartDate", _jobStartDate.Value);

                if (_jobTgtDate == null)
                    cm.Parameters.AddWithValue("@JobTgtDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobTgtDate", _jobTgtDate.Value);

                if (_jobEndDate == null)
                    cm.Parameters.AddWithValue("@JobEndDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobEndDate", _jobEndDate.Value);

                if (_jobMemo == null)
                    cm.Parameters.AddWithValue("@JobMemo", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobMemo", _jobMemo);

                if (_jobStatus == null)
                    cm.Parameters.AddWithValue("@JobStatus", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobStatus", _jobStatus);

                if (_jobAttachment == null)
                    cm.Parameters.AddWithValue("@JobAttachment", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobAttachment", _jobAttachment);

                if (_contractAmt == null)
                    cm.Parameters.AddWithValue("@ContractAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ContractAmt", _contractAmt);

                if (_retaintionAmt == null)
                    cm.Parameters.AddWithValue("@RetaintionAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RetaintionAmt", _retaintionAmt);

                if (_retaintionDate == null)
                    cm.Parameters.AddWithValue("@RetaintionDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RetaintionDate", _retaintionDate.Value);

                if (_accessLevel == null)
                    cm.Parameters.AddWithValue("@AccessLevel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessLevel", _accessLevel);

                if (_accessGroup == null)
                    cm.Parameters.AddWithValue("@AccessGroup", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessGroup", _accessGroup);

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

                if (!GFunc.IsNEZ(AppInfor.currentUserKey))
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

                if (_purgeKeep == null)
                    cm.Parameters.AddWithValue("@PurgeKeep", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurgeKeep", _purgeKeep);

                if (_purgeData == null)
                    cm.Parameters.AddWithValue("@PurgeData", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurgeData", _purgeData);

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

                if (_minMarkupSalePercent == null)
                    cm.Parameters.AddWithValue("@MinMarkupSalePercent", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MinMarkupSalePercent", _minMarkupSalePercent);

                if (_maxMarkupSalePercent == null)
                    cm.Parameters.AddWithValue("@MaxMarkupSalePercent", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MaxMarkupSalePercent", _maxMarkupSalePercent);

                if (_projectCostPercent == null)
                    cm.Parameters.AddWithValue("@ProjectCostPercent", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ProjectCostPercent", _projectCostPercent);

                cm.Parameters["@NewJobKey"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "MSTJob_Delete";
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@JobKey", criteria._jobKey);

                cm.ExecuteNonQuery();
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.
            
        }

        #endregion //Data Access - Delete

        #region Data Access - Validation

        internal bool Validation(Criteria criteria, bool? isNew)
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

        internal bool Validation(SqlConnection cn, Criteria criteria, bool? isNew)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTJob_Validation";

                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@JobKey", criteria._jobKey);
                cm.Parameters.AddWithValue("@JobID", criteria._jobID);

                cm.ExecuteNonQuery();
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }
            
        }
        #endregion //Data Access - Validation

        #region Record Access Level

        internal bool CanAccessRecord(int? jobKey)
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
                    retValue = this.CanAccessRecord(cn, jobKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }
        internal bool CanAccessRecord(SqlConnection cn, int? jobKey)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECRecAccess_Check";


                cm.Parameters.AddWithValue("@RetValue", 2);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@Key", jobKey);

                cm.Parameters.AddWithValue("@UserAccessLevel", AppInfor.conAccessLevel);
                cm.Parameters.AddWithValue("@UserKey", AppInfor.CurrentUserKey);

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }
        }
        internal bool AccessLevelUpdate(SqlConnection cn)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTJob_AccessLevelUpdate";
                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                if (_jobKey == null)
                    cm.Parameters.AddWithValue("@JobKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobKey", _jobKey);

                if (_accessLevel == null)
                    cm.Parameters.AddWithValue("@AccessLevel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessLevel", _accessLevel);

                if (_accessGroup == null)
                    cm.Parameters.AddWithValue("@AccessGroup", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessGroup", _accessGroup);

                if (AppInfor.currentUserKey == null)
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.

        }

        #endregion

        private void Clear()
        {
            _jobKey = 0;
            _jobID = string.Empty;
            _jobDes = string.Empty;
            _jobRem = string.Empty;
            _jobGrpKey = 0;
            _jobGrpID = string.Empty;
            _jobConKey = 0;
            _jobConID = string.Empty;
            _jobConNm = string.Empty;
            _jobClass = string.Empty;
            _jobPOID = string.Empty;
            _jobPODate = null;
            _jobSupervisor = string.Empty;
            _jobContact = string.Empty;
            _jobEMKey = null;
            _jobShipName = string.Empty;
            _jobShipMark = string.Empty;
            _jobStartDate = null;
            _jobTgtDate = null;
            _jobEndDate = null;
            _jobMemo = string.Empty;
            _jobStatus = 10;
            _jobAttachment = false;
            _contractAmt = 0;
            _retaintionAmt = 0;
            _retaintionDate = null;
            _accessLevel = 0;
            _accessGroup = 0;
            _createDate = null;
            _createUserKey = null;
            _lastModifiedDate = null;
            _lastModifiedUserKey = null;
            _purgeKeep = 0;
            _purgeData = false;
            _custom1 = string.Empty;
            _custom2 = string.Empty;
            _custom3 = string.Empty;
            _custom4 = string.Empty;
            _custom5 = string.Empty;
           


        }

    
    }
}