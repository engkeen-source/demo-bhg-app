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
    public class SYSOption : Csla.BusinessBase<SYSOption>
    {
        #region Business Properties and Methods

        //declare members
        internal string _opID = string.Empty;
        internal int? _opUserKey = 0;
        internal int? _opGrp = 0;
        internal int? _opSeq = 1;
        internal string _opName1 = string.Empty;
        internal string _opName2 = string.Empty;
        internal string _opName3 = string.Empty;
        internal string _opName4 = string.Empty;
        internal string _opName5 = string.Empty;
        internal string _opName6 = string.Empty;
        internal string _opName7 = string.Empty;
        internal string _opName8 = string.Empty;
        internal string _opName9 = string.Empty;
        internal string _opName10 = string.Empty;
        internal string _opDataType = string.Empty;
        internal string _opValue = string.Empty;
        internal string _msgListTblNm = string.Empty;
        internal int? _msgListDataGrp = 0;
        internal string _accessCode = string.Empty;
        internal string _validationRule = string.Empty;
        internal string _validationLimit = string.Empty;
        internal string _opValueDefault = string.Empty;
        internal string _opRemark1 = string.Empty;
        internal string _opRemark2 = string.Empty;
        internal string _opRemark3 = string.Empty;
        internal string _opRemark4 = string.Empty;
        internal string _opRemark5 = string.Empty;
        internal string _opRemark6 = string.Empty;
        internal string _opRemark7 = string.Empty;
        internal string _opRemark8 = string.Empty;
        internal string _opRemark9 = string.Empty;
        internal string _opRemark10 = string.Empty;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _OpValueDisplay = string.Empty;
        internal string _OpDisplaySql = string.Empty;
        internal string _Change = "...";
        public string OpID
        {
            get
            {                
                return _opID;
            }
        }

        public int? OpUserKey
        {
            get
            {
                return _opUserKey;
            }
        }

        public int? OpGrp
        {
            get
            {
                return _opGrp;
            }
        }

        public int? OpSeq
        {
            get
            {
                return _opSeq;
            }
        }

        public string OpName1
        {
            get
            {
                return _opName1;
            }
        }

        public string OpName2
        {
            get
            {
                return _opName2;
            }
        }

        public string OpName3
        {
            get
            {
                return _opName3;
            }
        }

        public string OpName4
        {
            get
            {
                return _opName4;
            }
        }

        public string OpName5
        {
            get
            {
                return _opName5;
            }
        }

        public string OpName6
        {
            get
            {
                return _opName6;
            }
        }

        public string OpName7
        {
            get
            {
                return _opName7;
            }
        }

        public string OpName8
        {
            get
            {
                return _opName8;
            }
        }

        public string OpName9
        {
            get
            {
                return _opName9;
            }
        }

        public string OpName10
        {
            get
            {
                return _opName10;
            }
        }

        public string OpDataType
        {
            get
            {
                return _opDataType;
            }
        }

        public string OpValue
        {
            get
            {
                return _opValue;
            }
            set
            {
                _opValue = value;
                PropertyHasChanged("OpValue");
            }
        }

        public string MsgListTblNm
        {
            get
            {
                return _msgListTblNm;
            }
        }

        public int? MsgListDataGrp
        {
            get
            {
                return _msgListDataGrp;
            }
        }

        public string AccessCode
        {
            get
            {
                return _accessCode;
            }
        }

        public string ValidationRule
        {
            get
            {
                return _validationRule;
            }
        }

        public string ValidationLimit
        {
            get
            {
                return _validationLimit;
            }
        }

        public string OpValueDefault
        {
            get
            {
                return _opValueDefault;
            }
        }

        public string OpRemark1
        {
            get
            {
                return _opRemark1;
            }
        }

        public string OpRemark2
        {
            get
            {
                return _opRemark2;
            }
        }

        public string OpRemark3
        {
            get
            {
                return _opRemark3;
            }
        }

        public string OpRemark4
        {
            get
            {
                return _opRemark4;
            }
        }

        public string OpRemark5
        {
            get
            {
                return _opRemark5;
            }
        }

        public string OpRemark6
        {
            get
            {
                return _opRemark6;
            }
        }

        public string OpRemark7
        {
            get
            {
                return _opRemark7;
            }
        }

        public string OpRemark8
        {
            get
            {
                return _opRemark8;
            }
        }

        public string OpRemark9
        {
            get
            {
                return _opRemark9;
            }
        }

        public string OpRemark10
        {
            get
            {
                return _opRemark10;
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

        public string OpValueDisplay
        {
            get
            {
                return _OpValueDisplay;
            }
            set
            {
                _OpValueDisplay = value;
                PropertyHasChanged("OpValueDisplay");
            }
        }

        public string OpDisplaySql
        {
            get
            {
                return _OpDisplaySql;
            }
            set
            {
                _OpDisplaySql = value;
                PropertyHasChanged("OpDisplaySql");
            }
        }
        public string Change
        {
            get
            {
                return _Change;
            }
            set
            {
                _Change = value;                
            }
        }
        protected override object GetIdValue()
        {
            return _opID.ToString() + _opUserKey.ToString();
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
            //// OpID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "OpID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpID", 50));
            ////
            //// OpName1
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "OpName1");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpName1", 255));
            ////
            //// OpName2
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpName2", 255));
            ////
            //// OpName3
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpName3", 255));
            ////
            //// OpName4
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpName4", 255));
            ////
            //// OpName5
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpName5", 255));
            ////
            //// OpName6
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpName6", 255));
            ////
            //// OpName7
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpName7", 255));
            ////
            //// OpName8
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpName8", 255));
            ////
            //// OpName9
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpName9", 255));
            ////
            //// OpName10
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpName10", 255));
            ////
            //// OpDataType
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpDataType", 50));
            ////
            //// OpValue
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpValue", 255));
            ////
            //// MsgListTblNm
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgListTblNm", 50));
            ////
            //// MsgListDataGrp
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgListDataGrp", 50));
            ////
            //// AccessCode
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("AccessCode", 255));
            ////
            //// ValidationRule
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ValidationRule", 50));
            ////
            //// ValidationLimit
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ValidationLimit", 50));
            ////
            //// OpValueDefault
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpValueDefault", 50));
            ////
            //// OpRemark1
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpRemark1", 255));
            ////
            //// OpRemark2
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpRemark2", 255));
            ////
            //// OpRemark3
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpRemark3", 255));
            ////
            //// OpRemark4
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpRemark4", 255));
            ////
            //// OpRemark5
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpRemark5", 255));
            ////
            //// OpRemark6
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpRemark6", 255));
            ////
            //// OpRemark7
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpRemark7", 255));
            ////
            //// OpRemark8
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpRemark8", 255));
            ////
            //// OpRemark9
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpRemark9", 255));
            ////
            //// OpRemark10
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OpRemark10", 255));
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

        public SYSOption()
        { /* require use of factory method */ }

        internal static SYSOption New()
        {
            //
            SYSOption child = new SYSOption();
            //
            return child;
        }

        internal static SYSOption NewChild()
        {
            //
            SYSOption child = new SYSOption();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            //
            return child;
        }

        internal static SYSOption Get(SafeDataReader dr)
        {
            //
            SYSOption child = new SYSOption();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }
        
        internal static SYSOption Get( int? opUserKey)
        {
            //
            SYSOption child = new SYSOption();
            child.Fetch(new Criteria(opUserKey,(int?) 1));
            return child;
        }

        internal static SYSOption Get(SqlConnection cn, int? opUserKey)
        {
            //
            SYSOption child = new SYSOption();
            child.Fetch(cn, new Criteria(opUserKey, (int?)1));
            return child;
        }

        public static SYSOption Get(string opID, int? opUserKey)
        {
            
            SYSOption child = new SYSOption();
            child.Fetch(new Criteria(opID, opUserKey, 1));
            return child;
        }
        public static SYSOption Get(SqlConnection cn, string opID, int? opUserKey)
        {

            SYSOption child = new SYSOption();
            child.Fetch(cn,new Criteria(opID, opUserKey, 1));
            return child;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            internal string _opID = string.Empty;
            internal string _opValue = string.Empty;
            public int? _opUserKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(string OpID, int? Option)
            {
                _opID = OpID;
                _option = Option;
            }

            internal Criteria(int? OpUserKey,int? Option)
            {
                _opUserKey = OpUserKey;
                _option = Option;
            }

            internal Criteria(string OpID, int? OpUserKey, int? Option)
            {
                _opID = OpID;
                _opUserKey = OpUserKey;
                _option = Option;
            }

            internal Criteria(string OpID, string  OpValue)
            {
                _opID = OpID;
                _opValue = OpValue;
                _option = 0;
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
                cm.CommandText = "SYSOption_Get";

                if(GFunc.IsNEZ(criteria._opUserKey))
                    criteria._opUserKey = AppInfor.currentUserKey; 

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@OpID", criteria._opID);
                if (criteria._opUserKey == null)
                    cm.Parameters.AddWithValue("@OpUserKey", 0);
                else
                    cm.Parameters.AddWithValue("@OpUserKey", criteria._opUserKey);


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
            _opID = dr.GetString("OpID");
            _opUserKey = dr.GetInt32("OpUserKey");
            _opGrp = dr.GetInt32("OpGrp");
            _opSeq = dr.GetInt32("OpSeq");
            _opName1 = dr.GetString("OpName1");
            _opName2 = dr.GetString("OpName2");
            _opName3 = dr.GetString("OpName3");
            _opName4 = dr.GetString("OpName4");
            _opName5 = dr.GetString("OpName5");
            _opName6 = dr.GetString("OpName6");
            _opName7 = dr.GetString("OpName7");
            _opName8 = dr.GetString("OpName8");
            _opName9 = dr.GetString("OpName9");
            _opName10 = dr.GetString("OpName10");
            _opDataType = dr.GetString("OpDataType");
            _opValue = dr.GetString("OpValue");
            _msgListTblNm = dr.GetString("MsgListTblNm");
            _msgListDataGrp = dr.GetInt32("MsgListDataGrp");
            _accessCode = dr.GetString("AccessCode");
            _validationRule = dr.GetString("ValidationRule");
            _validationLimit = dr.GetString("ValidationLimit");
            _opValueDefault = dr.GetString("OpValueDefault");
            _opRemark1 = dr.GetString("OpRemark1");
            _opRemark2 = dr.GetString("OpRemark2");
            _opRemark3 = dr.GetString("OpRemark3");
            _opRemark4 = dr.GetString("OpRemark4");
            _opRemark5 = dr.GetString("OpRemark5");
            _opRemark6 = dr.GetString("OpRemark6");
            _opRemark7 = dr.GetString("OpRemark7");
            _opRemark8 = dr.GetString("OpRemark8");
            _opRemark9 = dr.GetString("OpRemark9");
            _opRemark10 = dr.GetString("OpRemark10");

            if (dr.GetValue("CreateDate") != null)
                _createDate = dr.GetDateTime("CreateDate");

            _createUserKey = dr.GetInt32("CreateUserKey");

            if (dr.GetValue("LastModifiedDate") != null)
                _lastModifiedDate = dr.GetDateTime("LastModifiedDate");

            _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            _OpDisplaySql = dr.GetString("OpDisplaySql");
            _Change = "Change...";
            ValidationRules.CheckRules();
            this.MarkClean();
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
                cm.CommandText = "SYSOption_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                if (_opID == null)
                    cm.Parameters.AddWithValue("@OpID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpID", _opID);

                if (_opUserKey == null)
                    cm.Parameters.AddWithValue("@OpUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpUserKey", _opUserKey);

                if (_opGrp == null)
                    cm.Parameters.AddWithValue("@OpGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpGrp", _opGrp);

                if (_opSeq == null)
                    cm.Parameters.AddWithValue("@OpSeq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpSeq", _opSeq);

                if (_opName1 == null)
                    cm.Parameters.AddWithValue("@OpName1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName1", _opName1);

                if (_opName2 == null)
                    cm.Parameters.AddWithValue("@OpName2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName2", _opName2);

                if (_opName3 == null)
                    cm.Parameters.AddWithValue("@OpName3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName3", _opName3);

                if (_opName4 == null)
                    cm.Parameters.AddWithValue("@OpName4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName4", _opName4);

                if (_opName5 == null)
                    cm.Parameters.AddWithValue("@OpName5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName5", _opName5);

                if (_opName6 == null)
                    cm.Parameters.AddWithValue("@OpName6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName6", _opName6);

                if (_opName7 == null)
                    cm.Parameters.AddWithValue("@OpName7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName7", _opName7);

                if (_opName8 == null)
                    cm.Parameters.AddWithValue("@OpName8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName8", _opName8);

                if (_opName9 == null)
                    cm.Parameters.AddWithValue("@OpName9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName9", _opName9);

                if (_opName10 == null)
                    cm.Parameters.AddWithValue("@OpName10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName10", _opName10);

                if (_opDataType == null)
                    cm.Parameters.AddWithValue("@OpDataType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpDataType", _opDataType);

                if (_opValue == null)
                    cm.Parameters.AddWithValue("@OpValue", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpValue", _opValue);

                if (_msgListTblNm == null)
                    cm.Parameters.AddWithValue("@MsgListTblNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MsgListTblNm", _msgListTblNm);

                if (_msgListDataGrp == null)
                    cm.Parameters.AddWithValue("@MsgListDataGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MsgListDataGrp", _msgListDataGrp);

                if (_accessCode == null)
                    cm.Parameters.AddWithValue("@AccessCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessCode", _accessCode);

                if (_validationRule == null)
                    cm.Parameters.AddWithValue("@ValidationRule", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ValidationRule", _validationRule);

                if (_validationLimit == null)
                    cm.Parameters.AddWithValue("@ValidationLimit", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ValidationLimit", _validationLimit);

                if (_opValueDefault == null)
                    cm.Parameters.AddWithValue("@OpValueDefault", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpValueDefault", _opValueDefault);

                if (_opRemark1 == null)
                    cm.Parameters.AddWithValue("@OpRemark1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark1", _opRemark1);

                if (_opRemark2 == null)
                    cm.Parameters.AddWithValue("@OpRemark2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark2", _opRemark2);

                if (_opRemark3 == null)
                    cm.Parameters.AddWithValue("@OpRemark3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark3", _opRemark3);

                if (_opRemark4 == null)
                    cm.Parameters.AddWithValue("@OpRemark4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark4", _opRemark4);

                if (_opRemark5 == null)
                    cm.Parameters.AddWithValue("@OpRemark5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark5", _opRemark5);

                if (_opRemark6 == null)
                    cm.Parameters.AddWithValue("@OpRemark6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark6", _opRemark6);

                if (_opRemark7 == null)
                    cm.Parameters.AddWithValue("@OpRemark7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark7", _opRemark7);

                if (_opRemark8 == null)
                    cm.Parameters.AddWithValue("@OpRemark8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark8", _opRemark8);

                if (_opRemark9 == null)
                    cm.Parameters.AddWithValue("@OpRemark9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark9", _opRemark9);

                if (_opRemark10 == null)
                    cm.Parameters.AddWithValue("@OpRemark10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark10", _opRemark10);

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

                if (_OpDisplaySql == null)
                    cm.Parameters.AddWithValue("@OpDisplaySql", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpDisplaySql", _OpDisplaySql);

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
                cm.CommandText = "SYSOption_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);

                if (_opID == null)
                    cm.Parameters.AddWithValue("@OpID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpID", _opID);

                if (_opUserKey == null)
                    cm.Parameters.AddWithValue("@OpUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpUserKey", _opUserKey);

                if (_opGrp == null)
                    cm.Parameters.AddWithValue("@OpGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpGrp", _opGrp);

                if (_opSeq == null)
                    cm.Parameters.AddWithValue("@OpSeq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpSeq", _opSeq);

                if (_opName1 == null)
                    cm.Parameters.AddWithValue("@OpName1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName1", _opName1);

                if (_opName2 == null)
                    cm.Parameters.AddWithValue("@OpName2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName2", _opName2);

                if (_opName3 == null)
                    cm.Parameters.AddWithValue("@OpName3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName3", _opName3);

                if (_opName4 == null)
                    cm.Parameters.AddWithValue("@OpName4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName4", _opName4);

                if (_opName5 == null)
                    cm.Parameters.AddWithValue("@OpName5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName5", _opName5);

                if (_opName6 == null)
                    cm.Parameters.AddWithValue("@OpName6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName6", _opName6);

                if (_opName7 == null)
                    cm.Parameters.AddWithValue("@OpName7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName7", _opName7);

                if (_opName8 == null)
                    cm.Parameters.AddWithValue("@OpName8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName8", _opName8);

                if (_opName9 == null)
                    cm.Parameters.AddWithValue("@OpName9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName9", _opName9);

                if (_opName10 == null)
                    cm.Parameters.AddWithValue("@OpName10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpName10", _opName10);

                if (_opDataType == null)
                    cm.Parameters.AddWithValue("@OpDataType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpDataType", _opDataType);

                if (_opValue == null)
                    cm.Parameters.AddWithValue("@OpValue", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpValue", _opValue);

                if (_msgListTblNm == null)
                    cm.Parameters.AddWithValue("@MsgListTblNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MsgListTblNm", _msgListTblNm);

                if (_msgListDataGrp == null)
                    cm.Parameters.AddWithValue("@MsgListDataGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MsgListDataGrp", _msgListDataGrp);

                if (_accessCode == null)
                    cm.Parameters.AddWithValue("@AccessCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessCode", _accessCode);

                if (_validationRule == null)
                    cm.Parameters.AddWithValue("@ValidationRule", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ValidationRule", _validationRule);

                if (_validationLimit == null)
                    cm.Parameters.AddWithValue("@ValidationLimit", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ValidationLimit", _validationLimit);

                if (_opValueDefault == null)
                    cm.Parameters.AddWithValue("@OpValueDefault", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpValueDefault", _opValueDefault);

                if (_opRemark1 == null)
                    cm.Parameters.AddWithValue("@OpRemark1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark1", _opRemark1);

                if (_opRemark2 == null)
                    cm.Parameters.AddWithValue("@OpRemark2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark2", _opRemark2);

                if (_opRemark3 == null)
                    cm.Parameters.AddWithValue("@OpRemark3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark3", _opRemark3);

                if (_opRemark4 == null)
                    cm.Parameters.AddWithValue("@OpRemark4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark4", _opRemark4);

                if (_opRemark5 == null)
                    cm.Parameters.AddWithValue("@OpRemark5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark5", _opRemark5);

                if (_opRemark6 == null)
                    cm.Parameters.AddWithValue("@OpRemark6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark6", _opRemark6);

                if (_opRemark7 == null)
                    cm.Parameters.AddWithValue("@OpRemark7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark7", _opRemark7);

                if (_opRemark8 == null)
                    cm.Parameters.AddWithValue("@OpRemark8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark8", _opRemark8);

                if (_opRemark9 == null)
                    cm.Parameters.AddWithValue("@OpRemark9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark9", _opRemark9);

                if (_opRemark10 == null)
                    cm.Parameters.AddWithValue("@OpRemark10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpRemark10", _opRemark10);

                if (_createDate == null || ((DateTime)_createDate).Year == 1)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

                if (AppInfor.currentUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);

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

                if (_OpDisplaySql == null)
                    cm.Parameters.AddWithValue("@OpDisplaySql", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpDisplaySql", _OpDisplaySql);
                
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.
        }

        internal bool UpdateValue(Criteria critera)
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
                    retValue = this.UpdateValue(cn, critera);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope                

            return retValue;
        }

        internal bool UpdateValue(SqlConnection cn,Criteria critera)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSOption_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 3);

                if (critera._opID == null)
                    cm.Parameters.AddWithValue("@OpID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpID", critera._opID);                

                if (critera._opValue == null)
                    cm.Parameters.AddWithValue("@OpValue", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpValue", critera._opValue);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "SYSOption_Delete";

                cm.Parameters.AddWithValue("@OpID", criteria._opID);
                cm.Parameters.AddWithValue("@OpUserKey", criteria._opUserKey);

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
       
        internal void ClearDirty()
        {
            this.MarkClean();
        }
 
        #region New GUID

        public static bool NewGUID(SqlConnection cn, int? opUserKey, out int GUID)
        {            
            GUID = 0;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSOption_GetNewLockingGUID";

                if (opUserKey == null)
                    cm.Parameters.AddWithValue("@LoginUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LoginUserKey", opUserKey);
                cm.Parameters.AddWithValue("@LockingGUID", GUID);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters["@LockingGUID"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                if (cm.Parameters["@LockingGUID"].Value == null)
                    GUID = 0;
                else
                {
                    GUID = Convert.ToInt32(cm.Parameters["@LockingGUID"].Value);                        
                }

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;    
            }                            
        }

        #endregion

        #region New Document CodeKey

        public static int NewDocKey(SqlConnection cn, int DocCodeKey)
        {
            string msgID = MsgID.Option.GetOptionFail;
            int newDocKey = 0;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSOption_GetNewDocKey";

                cm.Parameters.AddWithValue("@DocCodeKey", DocCodeKey);
                cm.Parameters.AddWithValue("@NewDocKey", newDocKey);
                cm.Parameters.AddWithValue("@LoginUserKey", AppInfor.currentUserKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();
                

                if (cm.Parameters["@NewDocKey"].Value == DBNull.Value)
                    newDocKey = 1;
                else
                {
                    newDocKey = Convert.ToInt32(cm.Parameters["@NewDocKey"].Value);
                }

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return newDocKey;
                else
                    return 0;
            }            
        }

        #endregion

    }
}


