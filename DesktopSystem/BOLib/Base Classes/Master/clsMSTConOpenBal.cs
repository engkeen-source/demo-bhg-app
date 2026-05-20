

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
    public class MSTConOpenBal : Csla.BusinessBase<MSTConOpenBal>
    { 
        #region Business Properties and Methods

        //declare members
        internal int? _docKey = null;
        internal int? _docCodeKey = null;
        internal int? _docBranchKey = null;
        internal string _docID = string.Empty;
        internal DateTime? _docDate = null;
        internal DateTime? _docDateOrg = null;
        internal int? _docConKey = null;
        internal int? _docDeptKey = null;
        internal int? _docAccKey = null;
        internal int? _docGrpKey = null;
        internal decimal? _docGrand = null;
        internal int? _docCurrKey = null;
        internal decimal? _docCurrRate = null;
        internal decimal? _docHome = null;
        internal decimal? _docApplyAmtF = null;
        internal decimal? _docApplyAmtH = null;
        internal bool? _docApplyFull = null;
        internal decimal? _docRevalueAmtH = null;
        internal decimal? _docRevalueRate = null;
        internal string _docPOID = string.Empty;
        internal string _docDOID = string.Empty;
        internal string _docRef = string.Empty;
        internal string _docDes = string.Empty;
        internal string _docRem = string.Empty;
        internal string _docStatus = string.Empty;
        internal int? _docState = null;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal int? _purgeKeep = null;
        internal bool? _purgeData = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public int? DocKey
        {
            get
            {
                return _docKey;
            }
            set
            {
                _docKey = value;
                PropertyHasChanged("DocKey");
            }
        }

        public int? DocCodeKey
        {
            get
            {
                return _docCodeKey;
            }
            set
            {
                _docCodeKey = value;
                PropertyHasChanged("DocCodeKey");
            }
        }

        public int? DocBranchKey
        {
            get
            {
                return _docBranchKey;
            }
            set
            {
                _docBranchKey = value;
                PropertyHasChanged("DocBranchKey");
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

        public DateTime? DocDateOrg
        {
            get
            {
                return _docDateOrg;
            }
            set
            {
                _docDateOrg = value;
                PropertyHasChanged("DocDateOrg");
            }
        }

        public int? DocConKey
        {
            get
            {
                return _docConKey;
            }
            set
            {
                _docConKey = value;
                PropertyHasChanged("DocConKey");
            }
        }

        public int? DocDeptKey
        {
            get
            {
                return _docDeptKey;
            }
            set
            {
                _docDeptKey = value;
                PropertyHasChanged("DocDeptKey");
            }
        }

        public int? DocAccKey
        {
            get
            {
                return _docAccKey;
            }
            set
            {
                _docAccKey = value;
                PropertyHasChanged("DocAccKey");
            }
        }

        public int? DocGrpKey
        {
            get
            {
                return _docGrpKey;
            }
            set
            {
                _docGrpKey = value;
                PropertyHasChanged("DocGrpKey");
            }
        }

        public decimal? DocGrand
        {
            get
            {
                return _docGrand;
            }
            set
            {
                _docGrand = value;
                PropertyHasChanged("DocGrand");
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

        public decimal? DocHome
        {
            get
            {
                return _docHome;
            }
            set
            {
                _docHome = value;
                PropertyHasChanged("DocHome");
            }
        }

        public decimal? DocApplyAmtF
        {
            get
            {
                return _docApplyAmtF;
            }
            set
            {
                _docApplyAmtF = value;
                PropertyHasChanged("DocApplyAmtF");
            }
        }

        public decimal? DocApplyAmtH
        {
            get
            {
                return _docApplyAmtH;
            }
            set
            {
                _docApplyAmtH = value;
                PropertyHasChanged("DocApplyAmtH");
            }
        }

        public bool? DocApplyFull
        {
            get
            {
                return _docApplyFull;
            }
            set
            {
                _docApplyFull = value;
                PropertyHasChanged("DocApplyFull");
            }
        }

        public decimal? DocRevalueAmtH
        {
            get
            {
                return _docRevalueAmtH;
            }
            set
            {
                _docRevalueAmtH = value;
                PropertyHasChanged("DocRevalueAmtH");
            }
        }

        public decimal? DocRevalueRate
        {
            get
            {
                return _docRevalueRate;
            }
            set
            {
                _docRevalueRate = value;
                PropertyHasChanged("DocRevalueRate");
            }
        }

        public string DocPOID
        {
            get
            {
                return _docPOID;
            }
            set
            {
                _docPOID = value;
                PropertyHasChanged("DocPOID");
            }
        }

        public string DocDOID
        {
            get
            {
                return _docDOID;
            }
            set
            {
                _docDOID = value;
                PropertyHasChanged("DocDOID");
            }
        }

        public string DocRef
        {
            get
            {
                return _docRef;
            }
            set
            {
                _docRef = value;
                PropertyHasChanged("DocRef");
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

        public string DocRem
        {
            get
            {
                return _docRem;
            }
            set
            {
                _docRem = value;
                PropertyHasChanged("DocRem");
            }
        }

        public string DocStatus
        {
            get
            {
                return _docStatus;
            }
            set
            {
                _docStatus = value;
                PropertyHasChanged("DocStatus");
            }
        }

        public int? DocState
        {
            get
            {
                return _docState;
            }
            set
            {
                _docState = value;
                PropertyHasChanged("DocState");
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

        protected override object GetIdValue()
        {
            return _docKey.ToString();
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
           // DocPOID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocPOID", 50));
           //
           // DocDOID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocDOID", 50));
           //
           // DocRef
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocRef", 255));
           //
           // DocDes
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocDes", 255));
           //
           // DocRem
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocRem", 255));
           //
           // DocStatus
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocStatus", 50));
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

        internal MSTConOpenBal()
        { /* require use of factory method */ }

        internal static MSTConOpenBal New()
        {
            
            MSTConOpenBal child = new MSTConOpenBal();
            
            return child;
        }

        internal static MSTConOpenBal NewChild()
        {
            
            MSTConOpenBal child = new MSTConOpenBal();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static MSTConOpenBal Get(SafeDataReader dr)
        {
            
            MSTConOpenBal child = new MSTConOpenBal();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTConOpenBal Get(int? docKey)
        {
            
            MSTConOpenBal child = new MSTConOpenBal();
            child.Fetch(new Criteria(docKey, string.Empty, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _docKey = null;
            public int? _option = null;
            public string _DocID;
            internal Criteria()
            {
            }

            internal Criteria(int? DocKey)
            {
                _docKey = DocKey;
            }

            internal Criteria(int? DocKey,string DocID, int? Option)
            {
                _docKey = DocKey;
                _option = Option;
                _DocID = DocID;
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
                cm.CommandText = "MSTConOpenBal_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                    
                cm.Parameters.AddWithValue("@DocKey", criteria._docKey);
                

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
            _docKey = dr.GetInt32("DocKey");
            _docCodeKey = dr.GetInt32("DocCodeKey");
            _docBranchKey = dr.GetInt32("DocBranchKey");
            _docID = dr.GetString("DocID");

            if (GFunc.IsNE(dr.GetValue("DocDate")))
                _docDate = null;
            else
                _docDate = dr.GetDateTime("DocDate");

            if (GFunc.IsNE(dr.GetValue("DocDateOrg")))
                _docDateOrg = null;
            else
                _docDateOrg = dr.GetDateTime("DocDateOrg");

            _docConKey = dr.GetInt32("DocConKey");
            _docDeptKey = dr.GetInt32("DocDeptKey");
            _docAccKey = dr.GetInt32("DocAccKey");
            _docGrpKey = dr.GetInt32("DocGrpKey");
            _docGrand = dr.GetDecimal("DocGrand");
            _docCurrKey = dr.GetInt32("DocCurrKey");
            _docCurrRate = dr.GetDecimal("DocCurrRate");
            _docHome = dr.GetDecimal("DocHome");
            _docApplyAmtF = dr.GetDecimal("DocApplyAmtF");
            _docApplyAmtH = dr.GetDecimal("DocApplyAmtH");
            _docApplyFull = dr.GetBoolean("DocApplyFull");
            _docRevalueAmtH = dr.GetDecimal("DocRevalueAmtH");
            _docRevalueRate = dr.GetDecimal("DocRevalueRate");
            _docPOID = dr.GetString("DocPOID");
            _docDOID = dr.GetString("DocDOID");
            _docRef = dr.GetString("DocRef");
            _docDes = dr.GetString("DocDes");
            _docRem = dr.GetString("DocRem");
            _docStatus = dr.GetString("DocStatus");
            _docState = dr.GetInt32("DocState");
           
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
            ValidationRules.CheckRules();
            
            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? docKey)
        {
            bool retValue = false;
            docKey = null;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn,out docKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Insert(SqlConnection cn,out int? docKey)
        {
            docKey = 0;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTConOpenBal_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);
              
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewDocKey", docKey);

                if (_docKey == null)
                    cm.Parameters.AddWithValue("@DocKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocKey", _docKey);

                if (_docCodeKey == null)
                    cm.Parameters.AddWithValue("@DocCodeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocCodeKey", _docCodeKey);

                if (_docBranchKey == null)
                    cm.Parameters.AddWithValue("@DocBranchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocBranchKey", _docBranchKey);

                if (_docID == null)
                    cm.Parameters.AddWithValue("@DocID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocID", _docID);

                if (_docDate == null)
                    cm.Parameters.AddWithValue("@DocDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDate", _docDate.Value);

                if (_docDateOrg == null)
                    cm.Parameters.AddWithValue("@DocDateOrg", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDateOrg", _docDateOrg.Value);

                if (_docConKey == null)
                    cm.Parameters.AddWithValue("@DocConKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocConKey", _docConKey);

                if (_docDeptKey == null)
                    cm.Parameters.AddWithValue("@DocDeptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDeptKey", _docDeptKey);

                if (_docAccKey == null)
                    cm.Parameters.AddWithValue("@DocAccKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocAccKey", _docAccKey);

                if (_docGrpKey == null)
                    cm.Parameters.AddWithValue("@DocGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocGrpKey", _docGrpKey);

                if (_docGrand == null)
                    cm.Parameters.AddWithValue("@DocGrand", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocGrand", _docGrand);

                if (_docCurrKey == null)
                    cm.Parameters.AddWithValue("@DocCurrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocCurrKey", _docCurrKey);

                if (_docCurrRate == null)
                    cm.Parameters.AddWithValue("@DocCurrRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocCurrRate", _docCurrRate);

                if (_docHome == null)
                    cm.Parameters.AddWithValue("@DocHome", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocHome", _docHome);

                if (_docApplyAmtF == null)
                    cm.Parameters.AddWithValue("@DocApplyAmtF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocApplyAmtF", _docApplyAmtF);

                if (_docApplyAmtH == null)
                    cm.Parameters.AddWithValue("@DocApplyAmtH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocApplyAmtH", _docApplyAmtH);

                if (_docApplyFull == null)
                    cm.Parameters.AddWithValue("@DocApplyFull", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocApplyFull", _docApplyFull);

                if (_docRevalueAmtH == null)
                    cm.Parameters.AddWithValue("@DocRevalueAmtH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocRevalueAmtH", _docRevalueAmtH);

                if (_docRevalueRate == null)
                    cm.Parameters.AddWithValue("@DocRevalueRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocRevalueRate", _docRevalueRate);

                if (_docPOID == null)
                    cm.Parameters.AddWithValue("@DocPOID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocPOID", _docPOID);

                if (_docDOID == null)
                    cm.Parameters.AddWithValue("@DocDOID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDOID", _docDOID);

                if (_docRef == null)
                    cm.Parameters.AddWithValue("@DocRef", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocRef", _docRef);

                if (_docDes == null)
                    cm.Parameters.AddWithValue("@DocDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDes", _docDes);

                if (_docRem == null)
                    cm.Parameters.AddWithValue("@DocRem", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocRem", _docRem);

                if (_docStatus == null)
                    cm.Parameters.AddWithValue("@DocStatus", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocStatus", _docStatus);

                if (_docState == null)
                    cm.Parameters.AddWithValue("@DocState", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocState", _docState);

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

                cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();
               

                docKey = (int)cm.Parameters["@NewDocKey"].Value;

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
                cm.CommandText = "MSTConOpenBal_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);                   

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewDocKey", 0);

                if (_docKey == null)
                    cm.Parameters.AddWithValue("@DocKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocKey", _docKey);

                if (_docCodeKey == null)
                    cm.Parameters.AddWithValue("@DocCodeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocCodeKey", _docCodeKey);

                if (_docBranchKey == null)
                    cm.Parameters.AddWithValue("@DocBranchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocBranchKey", _docBranchKey);

                if (_docID == null)
                    cm.Parameters.AddWithValue("@DocID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocID", _docID);

                if (_docDate == null)
                    cm.Parameters.AddWithValue("@DocDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDate", _docDate.Value);

                if (_docDateOrg == null)
                    cm.Parameters.AddWithValue("@DocDateOrg", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDateOrg", _docDateOrg.Value);

                if (_docConKey == null)
                    cm.Parameters.AddWithValue("@DocConKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocConKey", _docConKey);

                if (_docDeptKey == null)
                    cm.Parameters.AddWithValue("@DocDeptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDeptKey", _docDeptKey);

                if (_docAccKey == null)
                    cm.Parameters.AddWithValue("@DocAccKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocAccKey", _docAccKey);

                if (_docGrpKey == null)
                    cm.Parameters.AddWithValue("@DocGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocGrpKey", _docGrpKey);

                if (_docGrand == null)
                    cm.Parameters.AddWithValue("@DocGrand", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocGrand", _docGrand);

                if (_docCurrKey == null)
                    cm.Parameters.AddWithValue("@DocCurrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocCurrKey", _docCurrKey);

                if (_docCurrRate == null)
                    cm.Parameters.AddWithValue("@DocCurrRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocCurrRate", _docCurrRate);

                if (_docHome == null)
                    cm.Parameters.AddWithValue("@DocHome", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocHome", _docHome);

                if (_docApplyAmtF == null)
                    cm.Parameters.AddWithValue("@DocApplyAmtF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocApplyAmtF", _docApplyAmtF);

                if (_docApplyAmtH == null)
                    cm.Parameters.AddWithValue("@DocApplyAmtH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocApplyAmtH", _docApplyAmtH);

                if (_docApplyFull == null)
                    cm.Parameters.AddWithValue("@DocApplyFull", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocApplyFull", _docApplyFull);

                if (_docRevalueAmtH == null)
                    cm.Parameters.AddWithValue("@DocRevalueAmtH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocRevalueAmtH", _docRevalueAmtH);

                if (_docRevalueRate == null)
                    cm.Parameters.AddWithValue("@DocRevalueRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocRevalueRate", _docRevalueRate);

                if (_docPOID == null)
                    cm.Parameters.AddWithValue("@DocPOID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocPOID", _docPOID);

                if (_docDOID == null)
                    cm.Parameters.AddWithValue("@DocDOID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDOID", _docDOID);

                if (_docRef == null)
                    cm.Parameters.AddWithValue("@DocRef", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocRef", _docRef);

                if (_docDes == null)
                    cm.Parameters.AddWithValue("@DocDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDes", _docDes);

                if (_docRem == null)
                    cm.Parameters.AddWithValue("@DocRem", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocRem", _docRem);

                if (_docStatus == null)
                    cm.Parameters.AddWithValue("@DocStatus", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocStatus", _docStatus);

                if (_docState == null)
                    cm.Parameters.AddWithValue("@DocState", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocState", _docState);

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

                cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "MSTConOpenBal_Delete";
              

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@DocKey", criteria._docKey);

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                    return false;
            }// Already close and dispose sql connection.            
        }

        #endregion //Data Access - Delete

    }
}


