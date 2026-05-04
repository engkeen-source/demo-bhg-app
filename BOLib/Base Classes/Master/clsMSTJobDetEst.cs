

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
    public class MSTJobDetEst : Csla.BusinessBase<MSTJobDetEst>, System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _jobKey = null;
        internal Boolean? _Selected = null;
        internal int? _jobEstKey = 0;
        internal int? _jobPhaseKey = 0;
        internal int? _jobTaskKey = 0;
        internal int? _jobCostTypeKey = 0;
        internal decimal? _estSN = 0;
        internal int? _estItmKey = null;
        internal int? _estItmKeySelect = null;
        internal string _estItmDes = string.Empty;
        internal string _estItmRem = string.Empty;
        internal decimal? _estQty = 0;
        internal int? _estUOMKey = null;
        internal decimal? _estConRate = 0;
        internal decimal? _estCostF = 0;
        internal decimal? _estCostH = 0;
        internal decimal? _estAmtF = 0;
        internal decimal? _estAmtH = 0;
        internal int? _docDK = 0;
        internal int? _docDItm = 0;
        internal string _docID = string.Empty;
        internal string _docDes = string.Empty;
        internal int? _docVendorKey = null;
        internal int? _docCurrKey = 1;
        internal decimal? _docCurrRate = 1;
        internal DateTime? _docETD = null;
        internal int? _transmitMode = 20;
        internal string _attention = string.Empty;
        internal string _emailAddr = string.Empty;
        internal string _faxNumber = string.Empty;
        internal int? _transmitStatus = 30;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _error = string.Empty;
        internal string _DocVendorID = string.Empty;
        internal string _DocVendorNm = string.Empty;
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
        public Boolean? Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                _Selected = value;
                PropertyHasChanged("Selected");
            }
        }
        public int? JobEstKey
        {
            get
            {
                return _jobEstKey;
            }
            set
            {
                _jobEstKey = value;
                PropertyHasChanged("JobEstKey");
            }
        }

        public int? JobPhaseKey
        {
            get
            {
                return _jobPhaseKey;
            }
            set
            {
                _jobPhaseKey = value;
                PropertyHasChanged("JobPhaseKey");
            }
        }

        public int? JobTaskKey
        {
            get
            {
                return _jobTaskKey;
            }
            set
            {
                _jobTaskKey = value;
                PropertyHasChanged("JobTaskKey");
            }
        }

        public int? JobCostTypeKey
        {
            get
            {
                return _jobCostTypeKey;
            }
            set
            {
                _jobCostTypeKey = value;
                PropertyHasChanged("JobCostTypeKey");
            }
        }

        public decimal? EstSN
        {
            get
            {
                return _estSN;
            }
            set
            {
                _estSN = value;
                PropertyHasChanged("EstSN");
            }
        }

        public int? EstItmKey
        {
            get
            {
                return _estItmKey;
            }
            set
            {
                _estItmKey = value;
                PropertyHasChanged("EstItmKey");
            }
        }

        public int? EstItmKeySelect
        {
            get
            {
                return _estItmKeySelect;
            }
            set
            {
                _estItmKeySelect = value;
                PropertyHasChanged("EstItmKeySelect");
            }
        }

        public string EstItmDes
        {
            get
            {
                return _estItmDes;
            }
            set
            {
                _estItmDes = value;
                PropertyHasChanged("EstItmDes");
            }
        }

        public string EstItmRem
        {
            get
            {
                return _estItmRem;
            }
            set
            {
                _estItmRem = value;
                PropertyHasChanged("EstItmRem");
            }
        }

        public decimal? EstQty
        {
            get
            {
                return _estQty;
            }
            set
            {
                _estQty = value;
                PropertyHasChanged("EstQty");
            }
        }

        public int? EstUOMKey
        {
            get
            {
                return _estUOMKey;
            }
            set
            {
                _estUOMKey = value;
                PropertyHasChanged("EstUOMKey");
            }
        }

        public decimal? EstConRate
        {
            get
            {
                return _estConRate;
            }
            set
            {
                _estConRate = value;
                PropertyHasChanged("EstConRate");
            }
        }

        public decimal? EstCostF
        {
            get
            {
                return _estCostF;
            }
            set
            {
                _estCostF = value;
                PropertyHasChanged("EstCostF");
            }
        }

        public decimal? EstCostH
        {
            get
            {
                return _estCostH;
            }
            set
            {
                _estCostH = value;
                PropertyHasChanged("EstCostH");
            }
        }

        public decimal? EstAmtF
        {
            get
            {
                return _estAmtF;
            }
            set
            {
                _estAmtF = value;
                PropertyHasChanged("EstAmtF");
            }
        }

        public decimal? EstAmtH
        {
            get
            {
                return _estAmtH;
            }
            set
            {
                _estAmtH = value;
                PropertyHasChanged("EstAmtH");
            }
        }

        public int? DocDK
        {
            get
            {
                return _docDK;
            }
            set
            {
                _docDK = value;
                PropertyHasChanged("DocDK");
            }
        }

        public int? DocDItm
        {
            get
            {
                return _docDItm;
            }
            set
            {
                _docDItm = value;
                PropertyHasChanged("DocDItm");
            }
        }

        public string DocID
        {
            get
            {
                return _docID;
            }
            set
            {
                _docID = value;
                PropertyHasChanged("DocID");
            }
        }

        public string DocDes
        {
            get
            {
                return _docDes;
            }
            set
            {
                _docDes = value;
                PropertyHasChanged("DocDes");
            }
        }

        public int? DocVendorKey
        {
            get
            {
                return _docVendorKey;
            }
            set
            {
                _docVendorKey = value;
                PropertyHasChanged("DocVendorKey");
            }
        }

        public int? DocCurrKey
        {
            get
            {
                return _docCurrKey;
            }
            set
            {
                _docCurrKey = value;
                PropertyHasChanged("DocCurrKey");
            }
        }

        public decimal? DocCurrRate
        {
            get
            {
                return _docCurrRate;
            }
            set
            {
                _docCurrRate = value;
                PropertyHasChanged("DocCurrRate");
            }
        }

        public DateTime? DocETD
        {
            get
            {
                return _docETD;
            }
            set
            {
                _docETD = value;
                PropertyHasChanged("DocETD");
            }
        }

        public int? TransmitMode
        {
            get
            {
                return _transmitMode;
            }
            set
            {
                _transmitMode = value;
                PropertyHasChanged("TransmitMode");
            }
        }

        public string Attention
        {
            get
            {
                return _attention;
            }
            set
            {
                _attention = value;
                PropertyHasChanged("Attention");
            }
        }

        public string EmailAddr
        {
            get
            {
                return _emailAddr;
            }
            set
            {
                _emailAddr = value;
                PropertyHasChanged("EmailAddr");
            }
        }

        public string FaxNumber
        {
            get
            {
                return _faxNumber;
            }
            set
            {
                _faxNumber = value;
                PropertyHasChanged("FaxNumber");
            }
        }

        public int? TransmitStatus
        {
            get
            {
                return _transmitStatus;
            }
            set
            {
                _transmitStatus = value;
                PropertyHasChanged("TransmitStatus");
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
        public string DocVendorID 
        {
            get
            {
                return _DocVendorID;
            }
            set
            {
                _DocVendorID = value;
                PropertyHasChanged("DocVendorID");
            }
        }
        public string DocVendorNm
        {
            get
            {
                return _DocVendorNm;
            }
            set
            {
                _DocVendorNm = value;
                PropertyHasChanged("DocVendorNm");
            }
        }
        protected override object GetIdValue()
        {
            return _jobKey.ToString() + _jobEstKey.ToString();
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

        public MSTJobDetEst()
        { /* require use of factory method */ }

        internal static MSTJobDetEst New()
        {
            
            MSTJobDetEst child = new MSTJobDetEst();
            
            return child;
        }

        internal static MSTJobDetEst NewChild()
        {
            
            MSTJobDetEst child = new MSTJobDetEst();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static MSTJobDetEst Get(SafeDataReader dr)
        {
           
            MSTJobDetEst child = new MSTJobDetEst();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTJobDetEst Get(int? jobKey, int? jobEstKey)
        {
           
            MSTJobDetEst child = new MSTJobDetEst();
            child.Fetch(new Criteria(jobKey, jobEstKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _jobKey = null;
            public int? _jobEstKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? JobKey, int? JobEstKey)
            {
                _jobKey = JobKey;
                _jobEstKey = JobEstKey;
            }

            internal Criteria(int? JobKey, int? JobEstKey, int? Option)
            {
                _jobKey = JobKey;
                _jobEstKey = JobEstKey;
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
                cm.CommandText = "MSTJobDetEst_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@JobKey", criteria._jobKey);
                cm.Parameters.AddWithValue("@JobEstKey", criteria._jobEstKey);

                

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

             

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            
            _jobKey = dr.GetInt32("JobKey");
            _jobEstKey = dr.GetInt32("JobEstKey");
            _jobPhaseKey = dr.GetInt32("JobPhaseKey");
            _jobTaskKey = dr.GetInt32("JobTaskKey");
            _jobCostTypeKey = dr.GetInt32("JobCostTypeKey");
            _estSN = dr.GetDecimal("EstSN");
            _estItmKey = GFunc.NEInt(dr.GetValue("EstItmKey"),0);
            _estItmKeySelect = GFunc.NEInt(dr.GetValue("EstItmKeySelect"),0);
            _estItmDes = dr.GetString("EstItmDes");
            _estItmRem = dr.GetString("EstItmRem");
            _estQty = dr.GetDecimal("EstQty");
            _estUOMKey = GFunc.NEInt(dr.GetValue("EstUOMKey"),0);
            _estConRate = dr.GetDecimal("EstConRate");
            _estCostF = dr.GetDecimal("EstCostF");
            _estCostH = dr.GetDecimal("EstCostH");
            _estAmtF = dr.GetDecimal("EstAmtF");
            _estAmtH = dr.GetDecimal("EstAmtH");
            _docDK = dr.GetInt32("DocDK");
            _docDItm = dr.GetInt32("DocDItm");
            _docID = dr.GetString("DocID");
            _docDes = dr.GetString("DocDes");
            _docVendorKey = GFunc.NEInt(dr.GetValue("DocVendorKey"), 0);
            _docCurrKey = dr.GetInt32("DocCurrKey");
            _docCurrRate = dr.GetDecimal("DocCurrRate");

            if (GFunc.IsNE(dr.GetValue("DocETD")))
                _docETD = null;
            else
                _docETD = dr.GetDateTime("DocETD");

            _transmitMode = dr.GetInt32("TransmitMode");
            _attention = dr.GetString("Attention");
            _emailAddr = dr.GetString("emailAddr");
            _faxNumber = dr.GetString("FaxNumber");
            _transmitStatus = dr.GetInt32("TransmitStatus");
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
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTJobDetEst_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                
                

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                //cm.Parameters.AddWithValue("@NewJobKey", 0);
                //cm.Parameters.AddWithValue("@NewJobEstKey", 0);

                if (_jobKey == null)
                    cm.Parameters.AddWithValue("@JobKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobKey", _jobKey);

                if (_jobEstKey == null)
                    cm.Parameters.AddWithValue("@JobEstKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobEstKey", _jobEstKey);

                if (_jobPhaseKey == null)
                    cm.Parameters.AddWithValue("@JobPhaseKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobPhaseKey", _jobPhaseKey);

                if (_jobTaskKey == null)
                    cm.Parameters.AddWithValue("@JobTaskKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobTaskKey", _jobTaskKey);

                if (_jobCostTypeKey == null)
                    cm.Parameters.AddWithValue("@JobCostTypeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobCostTypeKey", _jobCostTypeKey);

                if (_estSN == null)
                    cm.Parameters.AddWithValue("@EstSN", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstSN", _estSN);

                if (_estItmKey == null)
                    cm.Parameters.AddWithValue("@EstItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstItmKey", _estItmKey);

                if (_estItmKeySelect == null)
                    cm.Parameters.AddWithValue("@EstItmKeySelect", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstItmKeySelect", _estItmKeySelect);

                if (_estItmDes == null)
                    cm.Parameters.AddWithValue("@EstItmDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstItmDes", _estItmDes);

                if (_estItmRem == null)
                    cm.Parameters.AddWithValue("@EstItmRem", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstItmRem", _estItmRem);

                if (_estQty == null)
                    cm.Parameters.AddWithValue("@EstQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstQty", _estQty);

                if (_estUOMKey == null)
                    cm.Parameters.AddWithValue("@EstUOMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstUOMKey", _estUOMKey);

                if (_estConRate == null)
                    cm.Parameters.AddWithValue("@EstConRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstConRate", _estConRate);

                if (_estCostF == null)
                    cm.Parameters.AddWithValue("@EstCostF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstCostF", _estCostF);

                if (_estCostH == null)
                    cm.Parameters.AddWithValue("@EstCostH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstCostH", _estCostH);

                if (_estAmtF == null)
                    cm.Parameters.AddWithValue("@EstAmtF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstAmtF", _estAmtF);

                if (_estAmtH == null)
                    cm.Parameters.AddWithValue("@EstAmtH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstAmtH", _estAmtH);

                if (_docDK == null)
                    cm.Parameters.AddWithValue("@DocDK", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDK", _docDK);

                if (_docDItm == null)
                    cm.Parameters.AddWithValue("@DocDItm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDItm", _docDItm);

                if (_docID == null)
                    cm.Parameters.AddWithValue("@DocID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocID", _docID);

                if (_docDes == null)
                    cm.Parameters.AddWithValue("@DocDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDes", _docDes);

                if (_docVendorKey == null)
                    cm.Parameters.AddWithValue("@DocVendorKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocVendorKey", _docVendorKey);

                if (_docCurrKey == null)
                    cm.Parameters.AddWithValue("@DocCurrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocCurrKey", _docCurrKey);

                if (_docCurrRate == null)
                    cm.Parameters.AddWithValue("@DocCurrRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocCurrRate", _docCurrRate);

                if (_docETD == null)
                    cm.Parameters.AddWithValue("@DocETD", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocETD", _docETD.Value);

                if (_transmitMode == null)
                    cm.Parameters.AddWithValue("@TransmitMode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TransmitMode", _transmitMode);

                if (_attention == null)
                    cm.Parameters.AddWithValue("@Attention", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Attention", _attention);

                if (_emailAddr == null)
                    cm.Parameters.AddWithValue("@EmailAddr", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EmailAddr", _emailAddr);

                if (_faxNumber == null)
                    cm.Parameters.AddWithValue("@FaxNumber", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@FaxNumber", _faxNumber);

                if (_transmitStatus == null)
                    cm.Parameters.AddWithValue("@TransmitStatus", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TransmitStatus", _transmitStatus);

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

                if (!GFunc.IsNEZ(AppInfor.currentUserKey))
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

                //cm.Parameters["@NewJobKey"].Direction = ParameterDirection.Output;
                //cm.Parameters["@NewJobEstKey"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

               
                //_jobKey = (int)cm.Parameters["@NewJobKey"].Value;
                //_jobEstKey = (int)cm.Parameters["@NewJobEstKey"].Value;

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
                cm.CommandText = "MSTJobDetEst_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);

                
                

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                 if (_jobKey == null)
                    cm.Parameters.AddWithValue("@JobKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobKey", _jobKey);

                if (_jobEstKey == null)
                    cm.Parameters.AddWithValue("@JobEstKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobEstKey", _jobEstKey);

                if (_jobPhaseKey == null)
                    cm.Parameters.AddWithValue("@JobPhaseKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobPhaseKey", _jobPhaseKey);

                if (_jobTaskKey == null)
                    cm.Parameters.AddWithValue("@JobTaskKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobTaskKey", _jobTaskKey);

                if (_jobCostTypeKey == null)
                    cm.Parameters.AddWithValue("@JobCostTypeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobCostTypeKey", _jobCostTypeKey);

                if (_estSN == null)
                    cm.Parameters.AddWithValue("@EstSN", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstSN", _estSN);

                if (_estItmKey == null)
                    cm.Parameters.AddWithValue("@EstItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstItmKey", _estItmKey);

                if (_estItmKeySelect == null)
                    cm.Parameters.AddWithValue("@EstItmKeySelect", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstItmKeySelect", _estItmKeySelect);

                if (_estItmDes == null)
                    cm.Parameters.AddWithValue("@EstItmDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstItmDes", _estItmDes);

                if (_estItmRem == null)
                    cm.Parameters.AddWithValue("@EstItmRem", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstItmRem", _estItmRem);

                if (_estQty == null)
                    cm.Parameters.AddWithValue("@EstQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstQty", _estQty);

                if (_estUOMKey == null)
                    cm.Parameters.AddWithValue("@EstUOMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstUOMKey", _estUOMKey);

                if (_estConRate == null)
                    cm.Parameters.AddWithValue("@EstConRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstConRate", _estConRate);

                if (_estCostF == null)
                    cm.Parameters.AddWithValue("@EstCostF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstCostF", _estCostF);

                if (_estCostH == null)
                    cm.Parameters.AddWithValue("@EstCostH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstCostH", _estCostH);

                if (_estAmtF == null)
                    cm.Parameters.AddWithValue("@EstAmtF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstAmtF", _estAmtF);

                if (_estAmtH == null)
                    cm.Parameters.AddWithValue("@EstAmtH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EstAmtH", _estAmtH);

                if (_docDK == null)
                    cm.Parameters.AddWithValue("@DocDK", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDK", _docDK);

                if (_docDItm == null)
                    cm.Parameters.AddWithValue("@DocDItm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDItm", _docDItm);

                if (_docID == null)
                    cm.Parameters.AddWithValue("@DocID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocID", _docID);

                if (_docDes == null)
                    cm.Parameters.AddWithValue("@DocDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDes", _docDes);

                if (_docVendorKey == null)
                    cm.Parameters.AddWithValue("@DocVendorKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocVendorKey", _docVendorKey);

                if (_docCurrKey == null)
                    cm.Parameters.AddWithValue("@DocCurrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocCurrKey", _docCurrKey);

                if (_docCurrRate == null)
                    cm.Parameters.AddWithValue("@DocCurrRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocCurrRate", _docCurrRate);

                if (_docETD == null)
                    cm.Parameters.AddWithValue("@DocETD", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocETD", _docETD.Value);

                if (_transmitMode == null)
                    cm.Parameters.AddWithValue("@TransmitMode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TransmitMode", _transmitMode);

                if (_attention == null)
                    cm.Parameters.AddWithValue("@Attention", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Attention", _attention);

                if (_emailAddr == null)
                    cm.Parameters.AddWithValue("@EmailAddr", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EmailAddr", _emailAddr);

                if (_faxNumber == null)
                    cm.Parameters.AddWithValue("@FaxNumber", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@FaxNumber", _faxNumber);

                if (_transmitStatus == null)
                    cm.Parameters.AddWithValue("@TransmitStatus", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TransmitStatus", _transmitStatus);

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
                cm.CommandText = "MSTJobDetEst_Delete";

                
                

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

    }

}