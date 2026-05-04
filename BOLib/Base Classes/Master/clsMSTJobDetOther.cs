

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Collections;
using System.Transactions;
using BOLib;

namespace BOLib
{
    [Serializable()] 
    public class MSTJobDetOther : Csla.BusinessBase<MSTJobDetOther>,System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _jobKey = 0;
        internal int? _jobOtherKey = 0;
        internal int? _jobPhaseKey = 0;
        internal int? _jobTaskKey = 0;
        internal int? _jobCostTypeKey = 0;
        internal int? _othLineType = 10;
        internal int? _supervisor = 0;
        internal int? _eMKey = 0;
        internal int? _costGrp = 0;
        internal int? _othItmKey = 0;
        internal int? _othItmKeySelect = 0;
        internal string _othItmDes = string.Empty;
        internal string _othItmRem = string.Empty;
        internal decimal? _othQty = 0;
        internal int? _othUOMKey = 0;
        internal decimal? _othConRate = 1;
        internal decimal? _othPriceF = 0;
        internal decimal? _othPriceH = 0;
        internal decimal? _othExpAmtF = 0;
        internal decimal? _othExpAmtH = 0;
        internal decimal? _othRevAmtF = 0;
        internal decimal? _othRevAmtH = 0;
        internal decimal? _othPaidAmtF = 0;
        internal decimal? _othPaidAmtH = 0;
        internal string _docID = string.Empty;
        internal DateTime? _docDate = null;
        internal string _docDes = string.Empty;
        internal int? _docCurrKey = 1;
        internal decimal? _docCurrRate = 1;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _error = string.Empty;

      
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

        public int? JobOtherKey
        {
            get
            {
                return _jobOtherKey;
            }
            set
            {
                _jobOtherKey = value;
                PropertyHasChanged("JobOtherKey");
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

        public int? OthLineType
        {
            get
            {
                return _othLineType;
            }
            set
            {
                _othLineType = value;
                PropertyHasChanged("OthLineType");
            }
        }

        public int? Supervisor
        {
            get
            {
                return _supervisor;
            }
            set
            {
                _supervisor = value;
                PropertyHasChanged("Supervisor");
            }
        }

        public int? EMKey
        {
            get
            {
                return _eMKey;
            }
            set
            {
                _eMKey = value;
                PropertyHasChanged("EMKey");
            }
        }

        public int? CostGrp
        {
            get
            {
                return _costGrp;
            }
            set
            {
                _costGrp = value;
                PropertyHasChanged("CostGrp");
            }
        }

        public int? OthItmKey
        {
            get
            {
                return _othItmKey;
            }
            set
            {
                _othItmKey = value;
                PropertyHasChanged("OthItmKey");
            }
        }

        public int? OthItmKeySelect
        {
            get
            {
                return _othItmKeySelect;
            }
            set
            {
                _othItmKeySelect = value;
                PropertyHasChanged("OthItmKeySelect");
            }
        }

        public string OthItmDes
        {
            get
            {
                return _othItmDes;
            }
            set
            {
                _othItmDes = value;
                PropertyHasChanged("OthItmDes");
            }
        }

        public string OthItmRem
        {
            get
            {
                return _othItmRem;
            }
            set
            {
                _othItmRem = value;
                PropertyHasChanged("OthItmRem");
            }
        }

        public decimal? OthQty
        {
            get
            {
                return _othQty;
            }
            set
            {
                _othQty = value;
                PropertyHasChanged("OthQty");
            }
        }

        public int? OthUOMKey
        {
            get
            {
                return _othUOMKey;
            }
            set
            {
                _othUOMKey = value;
                PropertyHasChanged("OthUOMKey");
            }
        }

        public decimal? OthConRate
        {
            get
            {
                return _othConRate;
            }
            set
            {
                _othConRate = value;
                PropertyHasChanged("OthConRate");
            }
        }

        public decimal? OthPriceF
        {
            get
            {
                return _othPriceF;
            }
            set
            {
                _othPriceF = value;
                PropertyHasChanged("OthPriceF");
            }
        }

        public decimal? OthPriceH
        {
            get
            {
                return _othPriceH;
            }
            set
            {
                _othPriceH = value;
                PropertyHasChanged("OthPriceH");
            }
        }

        public decimal? OthExpAmtF
        {
            get
            {
                return _othExpAmtF;
            }
            set
            {
                _othExpAmtF = value;
                PropertyHasChanged("OthExpAmtF");
            }
        }

        public decimal? OthExpAmtH
        {
            get
            {
                return _othExpAmtH;
            }
            set
            {
                _othExpAmtH = value;
                PropertyHasChanged("OthExpAmtH");
            }
        }

        public decimal? OthRevAmtF
        {
            get
            {
                return _othRevAmtF;
            }
            set
            {
                _othRevAmtF = value;
                PropertyHasChanged("OthRevAmtF");
            }
        }

        public decimal? OthRevAmtH
        {
            get
            {
                return _othRevAmtH;
            }
            set
            {
                _othRevAmtH = value;
                PropertyHasChanged("OthRevAmtH");
            }
        }

        public decimal? OthPaidAmtF
        {
            get
            {
                return _othPaidAmtF;
            }
            set
            {
                _othPaidAmtF = value;
                PropertyHasChanged("OthPaidAmtF");
            }
        }

        public decimal? OthPaidAmtH
        {
            get
            {
                return _othPaidAmtH;
            }
            set
            {
                _othPaidAmtH = value;
                PropertyHasChanged("OthPaidAmtH");
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

        public DateTime? DocDate
        {
            get
            {
                return _docDate;
            }
            set
            {
                _docDate = value;
                PropertyHasChanged("DocDate");
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
            return _jobKey.ToString() + _jobOtherKey.ToString();
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
           // DocID
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "DocID");
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocID", 50));
           //
           // DocDate
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "DocDateString");
           //
           // DocDes
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocDes", 255));
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

        public MSTJobDetOther()
        { /* require use of factory method */ }

        internal static MSTJobDetOther New()
        {
            
            MSTJobDetOther child = new MSTJobDetOther();
            
            return child;
        }

        internal static MSTJobDetOther NewChild()
        {
            
            MSTJobDetOther child = new MSTJobDetOther();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static MSTJobDetOther Get(SafeDataReader dr)
        {
           
            MSTJobDetOther child = new MSTJobDetOther();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTJobDetOther Get(int? jobKey, int? jobOtherKey)
        {
           
            MSTJobDetOther child = new MSTJobDetOther();
            child.Fetch(new Criteria(jobKey, jobOtherKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _jobKey = null;
            public int? _jobOtherKey = null;
            public int? _option = null;

            public int? _emKey = null;
            public DateTime? _docDate = null;

            internal Criteria()
            {
            }

            internal Criteria(int? JobKey, int? JobOtherKey)
            {
                _jobKey = JobKey;
                _jobOtherKey = JobOtherKey;
            }

            internal Criteria(int? JobKey, int? JobOtherKey, int? Option)
            {
                _jobKey = JobKey;
                _jobOtherKey = JobOtherKey;
                _option = Option;
            }

            internal Criteria(int? EmKey, DateTime? DocDate, int? Option)
            {
                _emKey = EmKey;
                _docDate = DocDate;
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
                cm.CommandText = "MSTJobDetOther_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@JobKey", criteria._jobKey);
                cm.Parameters.AddWithValue("@JobOtherKey", criteria._jobOtherKey);

                

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
            _jobOtherKey = dr.GetInt32("JobOtherKey");
            _jobPhaseKey = dr.GetInt32("JobPhaseKey");
            _jobTaskKey = dr.GetInt32("JobTaskKey");
            _jobCostTypeKey = dr.GetInt32("JobCostTypeKey");
            _othLineType = dr.GetInt32("OthLineType");
            _supervisor = dr.GetInt32("Supervisor");
            _eMKey = dr.GetInt32("EMKey");
            _costGrp = dr.GetInt32("CostGrp");
            _othItmKey = dr.GetInt32("OthItmKey");
            _othItmKeySelect = dr.GetInt32("OthItmKeySelect");
            _othItmDes = dr.GetString("OthItmDes");
            _othItmRem = dr.GetString("OthItmRem");
            _othQty = dr.GetDecimal("OthQty");
            _othUOMKey = dr.GetInt32("OthUOMKey");
            _othConRate = dr.GetDecimal("OthConRate");
            _othPriceF = dr.GetDecimal("OthPriceF");
            _othPriceH = dr.GetDecimal("OthPriceH");
            _othExpAmtF = dr.GetDecimal("OthExpAmtF");
            _othExpAmtH = dr.GetDecimal("OthExpAmtH");
            _othRevAmtF = dr.GetDecimal("OthRevAmtF");
            _othRevAmtH = dr.GetDecimal("OthRevAmtH");
            _othPaidAmtF = dr.GetDecimal("OthPaidAmtF");
            _othPaidAmtH = dr.GetDecimal("OthPaidAmtH");
            _docID = dr.GetString("DocID");
            if (GFunc.IsNE(dr.GetValue("DocDate")))
                _docDate = null;
            else
                _docDate = dr.GetDateTime("DocDate");
            _docDes = dr.GetString("DocDes");
            _docCurrKey = dr.GetInt32("DocCurrKey");
            _docCurrRate = dr.GetDecimal("DocCurrRate");
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

        internal bool Insert( out int? jobKey, out int? jobOtherKey)
        {
            bool retValue = false;
            
            jobKey = null;
            jobOtherKey = null;
            
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
                cm.CommandText = "MSTJobDetOther_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                
                

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                //cm.Parameters.AddWithValue("@NewJobKey", 0);
                //cm.Parameters.AddWithValue("@NewJobOtherKey", 0);

                if (_jobKey == null)
                    cm.Parameters.AddWithValue("@JobKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobKey", _jobKey);

                if (_jobOtherKey == null)
                    cm.Parameters.AddWithValue("@JobOtherKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobOtherKey", _jobOtherKey);

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

                if (_othLineType == null)
                    cm.Parameters.AddWithValue("@OthLineType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthLineType", _othLineType);

                if (_supervisor == null)
                    cm.Parameters.AddWithValue("@Supervisor", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Supervisor", _supervisor);

                if (_eMKey == null)
                    cm.Parameters.AddWithValue("@EMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EMKey", _eMKey);

                if (_costGrp == null)
                    cm.Parameters.AddWithValue("@CostGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CostGrp", _costGrp);

                if (_othItmKey == null)
                    cm.Parameters.AddWithValue("@OthItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthItmKey", _othItmKey);

                if (_othItmKeySelect == null)
                    cm.Parameters.AddWithValue("@OthItmKeySelect", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthItmKeySelect", _othItmKeySelect);

                if (_othItmDes == null)
                    cm.Parameters.AddWithValue("@OthItmDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthItmDes", _othItmDes);

                if (_othItmRem == null)
                    cm.Parameters.AddWithValue("@OthItmRem", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthItmRem", _othItmRem);

                if (_othQty == null)
                    cm.Parameters.AddWithValue("@OthQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthQty", _othQty);

                if (_othUOMKey == null)
                    cm.Parameters.AddWithValue("@OthUOMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthUOMKey", _othUOMKey);

                if (_othConRate == null)
                    cm.Parameters.AddWithValue("@OthConRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthConRate", _othConRate);

                if (_othPriceF == null)
                    cm.Parameters.AddWithValue("@OthPriceF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthPriceF", _othPriceF);

                if (_othPriceH == null)
                    cm.Parameters.AddWithValue("@OthPriceH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthPriceH", _othPriceH);

                if (_othExpAmtF == null)
                    cm.Parameters.AddWithValue("@OthExpAmtF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthExpAmtF", _othExpAmtF);

                if (_othExpAmtH == null)
                    cm.Parameters.AddWithValue("@OthExpAmtH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthExpAmtH", _othExpAmtH);

                if (_othRevAmtF == null)
                    cm.Parameters.AddWithValue("@OthRevAmtF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthRevAmtF", _othRevAmtF);

                if (_othRevAmtH == null)
                    cm.Parameters.AddWithValue("@OthRevAmtH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthRevAmtH", _othRevAmtH);

                if (_othPaidAmtF == null)
                    cm.Parameters.AddWithValue("@OthPaidAmtF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthPaidAmtF", _othPaidAmtF);

                if (_othPaidAmtH == null)
                    cm.Parameters.AddWithValue("@OthPaidAmtH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthPaidAmtH", _othPaidAmtH);

                if (_docID == null)
                    cm.Parameters.AddWithValue("@DocID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocID", _docID);

                if (_docDate == null)
                    cm.Parameters.AddWithValue("@DocDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDate", _docDate.Value);

                if (_docDes == null)
                    cm.Parameters.AddWithValue("@DocDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDes", _docDes);

                if (_docCurrKey == null)
                    cm.Parameters.AddWithValue("@DocCurrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocCurrKey", _docCurrKey);

                if (_docCurrRate == null)
                    cm.Parameters.AddWithValue("@DocCurrRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocCurrRate", _docCurrRate);

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
                //cm.Parameters["@NewJobOtherKey"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

              

                //_jobKey = (int)cm.Parameters["@NewJobKey"].Value;
                //_jobOtherKey = (int)cm.Parameters["@NewJobOtherKey"].Value;

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
                cm.CommandText = "MSTJobDetOther_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);

                
                

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                if (_jobKey == null)
                    cm.Parameters.AddWithValue("@JobKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobKey", _jobKey);

                if (_jobOtherKey == null)
                    cm.Parameters.AddWithValue("@JobOtherKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobOtherKey", _jobOtherKey);

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

                if (_othLineType == null)
                    cm.Parameters.AddWithValue("@OthLineType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthLineType", _othLineType);

                if (_supervisor == null)
                    cm.Parameters.AddWithValue("@Supervisor", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Supervisor", _supervisor);

                if (_eMKey == null)
                    cm.Parameters.AddWithValue("@EMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EMKey", _eMKey);

                if (_costGrp == null)
                    cm.Parameters.AddWithValue("@CostGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CostGrp", _costGrp);

                if (_othItmKey == null)
                    cm.Parameters.AddWithValue("@OthItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthItmKey", _othItmKey);

                if (_othItmKeySelect == null)
                    cm.Parameters.AddWithValue("@OthItmKeySelect", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthItmKeySelect", _othItmKeySelect);

                if (_othItmDes == null)
                    cm.Parameters.AddWithValue("@OthItmDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthItmDes", _othItmDes);

                if (_othItmRem == null)
                    cm.Parameters.AddWithValue("@OthItmRem", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthItmRem", _othItmRem);

                if (_othQty == null)
                    cm.Parameters.AddWithValue("@OthQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthQty", _othQty);

                if (_othUOMKey == null)
                    cm.Parameters.AddWithValue("@OthUOMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthUOMKey", _othUOMKey);

                if (_othConRate == null)
                    cm.Parameters.AddWithValue("@OthConRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthConRate", _othConRate);

                if (_othPriceF == null)
                    cm.Parameters.AddWithValue("@OthPriceF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthPriceF", _othPriceF);

                if (_othPriceH == null)
                    cm.Parameters.AddWithValue("@OthPriceH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthPriceH", _othPriceH);

                if (_othExpAmtF == null)
                    cm.Parameters.AddWithValue("@OthExpAmtF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthExpAmtF", _othExpAmtF);

                if (_othExpAmtH == null)
                    cm.Parameters.AddWithValue("@OthExpAmtH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthExpAmtH", _othExpAmtH);

                if (_othRevAmtF == null)
                    cm.Parameters.AddWithValue("@OthRevAmtF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthRevAmtF", _othRevAmtF);

                if (_othRevAmtH == null)
                    cm.Parameters.AddWithValue("@OthRevAmtH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthRevAmtH", _othRevAmtH);

                if (_othPaidAmtF == null)
                    cm.Parameters.AddWithValue("@OthPaidAmtF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthPaidAmtF", _othPaidAmtF);

                if (_othPaidAmtH == null)
                    cm.Parameters.AddWithValue("@OthPaidAmtH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OthPaidAmtH", _othPaidAmtH);

                if (_docID == null)
                    cm.Parameters.AddWithValue("@DocID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocID", _docID);

                if (_docDate == null)
                    cm.Parameters.AddWithValue("@DocDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDate", _docDate.Value);

                if (_docDes == null)
                    cm.Parameters.AddWithValue("@DocDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDes", _docDes);

                if (_docCurrKey == null)
                    cm.Parameters.AddWithValue("@DocCurrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocCurrKey", _docCurrKey);

                if (_docCurrRate == null)
                    cm.Parameters.AddWithValue("@DocCurrRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocCurrRate", _docCurrRate);

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

                cm.Parameters["@NewJobKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewJobOtherKey"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "MSTJobDetOther_Delete";

                
                

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