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
    public class SYSDocTypeDetNm : Csla.BusinessBase<SYSDocTypeDetNm>, System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _codeKey = null;
        internal string _docTypeNm = string.Empty;
        internal int? _docType = null;
        internal string _docTypeNmLang1 = string.Empty;
        internal string _docTypeNmLang2 = string.Empty;
        internal string _docTypeNmLang3 = string.Empty;
        internal string _docTypeNmLang4 = string.Empty;
        internal string _docTypeNmLang5 = string.Empty;
        internal string _docTypeNmLang6 = string.Empty;
        internal string _docTypeNmLang7 = string.Empty;
        internal string _docTypeNmLang8 = string.Empty;
        internal string _docTypeNmLang9 = string.Empty;
        internal string _docTypeNmLang10 = string.Empty;
        internal bool? _systemReq = false;
        internal bool? _setAsDefault = false;
        internal bool? _hidden = false;
        internal int? _counterGrp = 0;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _error = string.Empty;      

        public int? CodeKey
        {
            get
            {               
                return _codeKey;
            }
        }

        public string DocTypeNm
        {
            get
            {

                return _docTypeNm;
            }
            set
            {
                _docTypeNm = value;
                PropertyHasChanged("DocTypeNm");
            }
        }

        public int? DocType
        {
            get
            {
                return _docType;
            }

            set
            {
                    _docType = value;
                    PropertyHasChanged("DocType");
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

        public string DocTypeNmLang1
        {
            get
            {
                return _docTypeNmLang1;
            }
        }

        public string DocTypeNmLang2
        {
            get
            {
                return _docTypeNmLang2;
            }
        }

        public string DocTypeNmLang3
        {
            get
            {
                return _docTypeNmLang3;
            }
        }

        public string DocTypeNmLang4
        {
            get
            {
                return _docTypeNmLang4;
            }
        }

        public string DocTypeNmLang5
        {
            get
            {
                return _docTypeNmLang5;
            }
        }

        public string DocTypeNmLang6
        {
            get
            {
                return _docTypeNmLang6;
            }
        }

        public string DocTypeNmLang7
        {
            get
            {
                return _docTypeNmLang7;
            }
        }

        public string DocTypeNmLang8
        {
            get
            {
                return _docTypeNmLang8;
            }
        }

        public string DocTypeNmLang9
        {
            get
            {
                return _docTypeNmLang9;
            }
        }

        public string DocTypeNmLang10
        {
            get
            {
                return _docTypeNmLang10;
            }
        }

        public bool? SystemReq
        {
            get
            {
                return _systemReq;
            }
        }

        public bool? SetAsDefault
        {
            get
            {

                return _setAsDefault;
            }
            set
            {
                _setAsDefault = value;
                PropertyHasChanged("SetAsDefault");
            }
        }

        public bool? Hidden
        {
            get
            {
                return _hidden;
            }
        }

        public int? CounterGrp
        {
            get
            {

                return _counterGrp;
            }
            set
            {
                _counterGrp = value;
                PropertyHasChanged("CounterGrp");
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
            return _codeKey.ToString() + _docTypeNm.ToString();
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
            //// DocTypeNm
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "DocTypeNm");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocTypeNm", 50));
            ////
            //// DocTypeNmLang1
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "DocTypeNmLang1");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocTypeNmLang1", 255));
            ////
            //// DocTypeNmLang2
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocTypeNmLang2", 255));
            ////
            //// DocTypeNmLang3
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocTypeNmLang3", 255));
            ////
            //// DocTypeNmLang4
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocTypeNmLang4", 255));
            ////
            //// DocTypeNmLang5
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocTypeNmLang5", 255));
            ////
            //// DocTypeNmLang6
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocTypeNmLang6", 255));
            ////
            //// DocTypeNmLang7
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocTypeNmLang7", 255));
            ////
            //// DocTypeNmLang8
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocTypeNmLang8", 255));
            ////
            //// DocTypeNmLang9
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocTypeNmLang9", 255));
            ////
            //// DocTypeNmLang10
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocTypeNmLang10", 255));
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

        internal SYSDocTypeDetNm()
        { /* require use of factory method */ }

        internal static SYSDocTypeDetNm New()
        {
            
            SYSDocTypeDetNm child = new SYSDocTypeDetNm();
            return child;
        }

        internal static SYSDocTypeDetNm NewChild()
        {
            SYSDocTypeDetNm child = new SYSDocTypeDetNm();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static SYSDocTypeDetNm Get(SafeDataReader dr)
        {
            SYSDocTypeDetNm child = new SYSDocTypeDetNm();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SYSDocTypeDetNm Get(GEnum.SystemCode codeKey, string docTypeNm)
        {
            SYSDocTypeDetNm child = new SYSDocTypeDetNm();
            child.Fetch(new Criteria(codeKey, docTypeNm, 1));
            return child;
        }

        internal static SYSDocTypeDetNm Get(GEnum.SystemCode codeKey)
        {
            SYSDocTypeDetNm child = new SYSDocTypeDetNm();
            child.Fetch(new Criteria(codeKey, 1));
            return child;
        }

        internal static SYSDocTypeDetNm Get(SqlConnection cn, GEnum.SystemCode codeKey)
        {
            SYSDocTypeDetNm child = new SYSDocTypeDetNm();
            child.Fetch(cn,new Criteria(codeKey, 1));
            return child;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public GEnum.SystemCode? _codeKey = null;
            internal string _docTypeNm = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(GEnum.SystemCode? CodeKey, string DocTypeNm)
            {
                _codeKey = CodeKey;
                _docTypeNm = DocTypeNm;
            }

            internal Criteria(GEnum.SystemCode? CodeKey, int? Option)
            {
                _codeKey = CodeKey;
                _docTypeNm = "%";
                _option = Option;
            }

            internal Criteria(GEnum.SystemCode? CodeKey, string DocTypeNm, int? Option)
            {
                _codeKey = CodeKey;
                _docTypeNm = DocTypeNm;
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
                cm.CommandText = "SYSDocTypeDetNm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@CodeKey", (int)criteria._codeKey);
                cm.Parameters.AddWithValue("@DocTypeNm", criteria._docTypeNm);
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

        internal bool? Fetch(SafeDataReader dr)
        {
            _codeKey = dr.GetInt32("CodeKey");
            _docTypeNm = dr.GetString("DocTypeNm");
            _docType = dr.GetInt32("DocType");
            _docTypeNmLang1 = dr.GetString("DocTypeNmLang1");
            _docTypeNmLang2 = dr.GetString("DocTypeNmLang2");
            _docTypeNmLang3 = dr.GetString("DocTypeNmLang3");
            _docTypeNmLang4 = dr.GetString("DocTypeNmLang4");
            _docTypeNmLang5 = dr.GetString("DocTypeNmLang5");
            _docTypeNmLang6 = dr.GetString("DocTypeNmLang6");
            _docTypeNmLang7 = dr.GetString("DocTypeNmLang7");
            _docTypeNmLang8 = dr.GetString("DocTypeNmLang8");
            _docTypeNmLang9 = dr.GetString("DocTypeNmLang9");
            _docTypeNmLang10 = dr.GetString("DocTypeNmLang10");
            _systemReq = dr.GetBoolean("SystemReq");
            _setAsDefault = dr.GetBoolean("SetAsDefault");
            _hidden = dr.GetBoolean("Hidden");
            _counterGrp = dr.GetInt32("CounterGrp");
            _createDate = dr.GetDateTime("CreateDate");
            _createUserKey = dr.GetInt32("CreateUserKey");
            _lastModifiedDate = dr.GetDateTime("LastModifiedDate");
            _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            ValidationRules.CheckRules();
            MarkClean();
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
                cm.CommandText = "SYSDocTypeDetNm_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                if (_codeKey == null)
                    cm.Parameters.AddWithValue("@CodeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeKey", _codeKey);

                if (_docTypeNm == null)
                    cm.Parameters.AddWithValue("@DocTypeNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNm", _docTypeNm);

                if (_docType == null)
                    cm.Parameters.AddWithValue("@DocType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocType", _docType);

                if (_docTypeNmLang1 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang1", _docTypeNmLang1);

                if (_docTypeNmLang2 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang2", _docTypeNmLang2);

                if (_docTypeNmLang3 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang3", _docTypeNmLang3);

                if (_docTypeNmLang4 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang4", _docTypeNmLang4);

                if (_docTypeNmLang5 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang5", _docTypeNmLang5);

                if (_docTypeNmLang6 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang6", _docTypeNmLang6);

                if (_docTypeNmLang7 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang7", _docTypeNmLang7);

                if (_docTypeNmLang8 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang8", _docTypeNmLang8);

                if (_docTypeNmLang9 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang9", _docTypeNmLang9);

                if (_docTypeNmLang10 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang10", _docTypeNmLang10);

                if (_systemReq == null)
                    cm.Parameters.AddWithValue("@SystemReq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SystemReq", _systemReq);

                if (_setAsDefault == null)
                    cm.Parameters.AddWithValue("@SetAsDefault", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SetAsDefault", _setAsDefault);

                if (_hidden == null)
                    cm.Parameters.AddWithValue("@Hidden", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Hidden", _hidden);

                if (_counterGrp == null)
                    cm.Parameters.AddWithValue("@CounterGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CounterGrp", _counterGrp);

                //if (_createDate == null)
                cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                //else
                //    cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

                if (AppInfor.currentUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                     cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);

                //if (_lastModifiedDate == null)
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
                cm.CommandText = "SYSDocTypeDetNm_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);


                if (_codeKey == null)
                    cm.Parameters.AddWithValue("@CodeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeKey", _codeKey);

                if (_docTypeNm == null)
                    cm.Parameters.AddWithValue("@DocTypeNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNm", _docTypeNm);

                if (_docType == null)
                    cm.Parameters.AddWithValue("@DocType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocType", _docType);

                if (_docTypeNmLang1 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang1", _docTypeNmLang1);

                if (_docTypeNmLang2 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang2", _docTypeNmLang2);

                if (_docTypeNmLang3 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang3", _docTypeNmLang3);

                if (_docTypeNmLang4 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang4", _docTypeNmLang4);

                if (_docTypeNmLang5 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang5", _docTypeNmLang5);

                if (_docTypeNmLang6 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang6", _docTypeNmLang6);

                if (_docTypeNmLang7 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang7", _docTypeNmLang7);

                if (_docTypeNmLang8 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang8", _docTypeNmLang8);

                if (_docTypeNmLang9 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang9", _docTypeNmLang9);

                if (_docTypeNmLang10 == null)
                    cm.Parameters.AddWithValue("@DocTypeNmLang10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNmLang10", _docTypeNmLang10);

                if (_systemReq == null)
                    cm.Parameters.AddWithValue("@SystemReq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SystemReq", _systemReq);

                if (_setAsDefault == null)
                    cm.Parameters.AddWithValue("@SetAsDefault", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SetAsDefault", _setAsDefault);

                if (_hidden == null)
                    cm.Parameters.AddWithValue("@Hidden", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Hidden", _hidden);

                if (_counterGrp == null)
                    cm.Parameters.AddWithValue("@CounterGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CounterGrp", _counterGrp);

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
                cm.CommandText = "SYSDocTypeDetNm_Delete";

                cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);
                //cm.Parameters.AddWithValue("@Option", criteria._option);

                //cm.Parameters.AddWithValue("@DocTypeNm", criteria._docTypeNm);
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
                cm.CommandText = "SYSDocTypeDetNm_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);

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
