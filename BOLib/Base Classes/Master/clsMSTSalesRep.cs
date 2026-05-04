
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
    public class MSTSalesRep : Csla.BusinessBase<MSTSalesRep>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _emKey = 0;
        internal string _emID = string.Empty;
        internal string _emNm = string.Empty;
        internal string _emClass = string.Empty;
        internal DateTime? _emDOB = null;
        internal string _emRef = string.Empty;
        internal string _emEmail = string.Empty;
        internal int? _userKey = 0;
        internal int? _jobCostGrpKey = 0;
        internal int? _jobLabourItmKey = 0;
        internal bool? _inactive = false;
        internal DateTime? _dateHired = null;
        internal DateTime? _dateTerminated = null;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _custom4 = string.Empty;
        internal string _custom5 = string.Empty;

        //ttm
        internal decimal? _saleLimit = 0;
        internal decimal? _purchaseLimit = 0;
        internal int? _finalSaleAppKey = 0;
        internal string _finalSaleApprover = string.Empty;
        internal int? _marginLimitForFinalApprover = null;
        public decimal? SaleLimit
        {
            get
            {
                return _saleLimit;
            }
            set
            {
                _saleLimit = value;
                PropertyHasChanged("SaleLimit");
            }
        }
        public decimal? PurchaseLimit
        {
            get
            {
                return _purchaseLimit;
            }
            set
            {
                _purchaseLimit = value;
                PropertyHasChanged("PurchaseLimit");
            }
        }
        public int? FinalSaleAppKey
        {
            get
            {
                return _finalSaleAppKey;
            }
            set
            {
                _finalSaleAppKey = value;
                PropertyHasChanged("FinalSaleAppKey");
            }
        }
        public string FinalSaleApprover
        {
            get
            {
                return _finalSaleApprover;
            }
            set
            {
                _finalSaleApprover = value;
                PropertyHasChanged("FinalSaleApprover");
            }
        }
        public int? MarginLimitForFinalApprover
        {
            get
            {
                return _marginLimitForFinalApprover;
            }
            set
            {
                _marginLimitForFinalApprover = value;
                PropertyHasChanged("MarginLimitForFinalApprover");
            }
        }
        //ttm

        public int? EmKey
        {
            get
            {
                return _emKey;
            }
        }

        public string EmID
        {
            get
            {
                return _emID;
            }
            set
            {
                _emID = value;
                PropertyHasChanged("EmID");
            }
        }

        public string EmNm
        {
            get
            {
                return _emNm;
            }
            set
            {
                _emNm = value;
                PropertyHasChanged("EmNm");
            }
        }

        public string EmClass
        {
            get
            {
                return _emClass;
            }
            set
            {
                _emClass = value;
                PropertyHasChanged("EmClass");
            }
        }

        public DateTime? EmDOB
        {
            get
            {
                return _emDOB;
            }
            set
            {
                _emDOB = value;
                PropertyHasChanged("EmDOB");
            }
        }

        public string EmRef
        {
            get
            {
                return _emRef;
            }
            set
            {
                _emRef = value;
                PropertyHasChanged("EmRef");
            }
        }
        public string EmEmail
        {
            get
            {
                return _emEmail;
            }
            set
            {
                _emEmail = value;
                PropertyHasChanged("EmEmail");
            }
        }

        public int? UserKey
        {
            get
            {
                return _userKey;
            }
            set
            {
                _userKey = value;
                PropertyHasChanged("UserKey");
            }
        }

        public int? JobCostGrpKey
        {
            get
            {
                return _jobCostGrpKey;
            }
            set
            {
                _jobCostGrpKey = value;
                PropertyHasChanged("JobCostGrpKey");
            }
        }

        public int? JobLabourItmKey
        {
            get
            {
                return _jobLabourItmKey;
            }
            set
            {
                _jobLabourItmKey = value;
                PropertyHasChanged("JobLabourItmKey");
            }
        }

        public bool? Inactive
        {
            get
            {
                return _inactive;
            }
            set
            {
                _inactive = value;
                PropertyHasChanged("Inactive");
            }
        }

        public DateTime? DateHired
        {
            get
            {
                return _dateHired;
            }
            set
            {
                _dateHired = value;
                PropertyHasChanged("DateHired");
            }
        }

        public DateTime? DateTerminated
        {
            get
            {
                return _dateTerminated;
            }
            set
            {
                _dateTerminated = value;
                PropertyHasChanged("DateTerminated");
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

        protected override object GetIdValue()
        {
            return _emKey.ToString();
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
            //// EmID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "EmID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EmID", 50));
            ////
            //// EmNm
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "EmNm");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EmNm", 255));
            ////
            //// EmClass
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EmClass", 50));
            ////
            //// EmRef
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EmRef", 50));
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
            ////
            //// Custom4
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom4", 255));
            ////
            //// Custom5
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom5", 255));
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal MSTSalesRep()
        { /* require use of factory method */ }

        internal static MSTSalesRep New()
        {
            
            MSTSalesRep child = new MSTSalesRep();
            
            return child;
        }

        internal static MSTSalesRep NewChild()
        {
            
            MSTSalesRep child = new MSTSalesRep();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static MSTSalesRep Get(SafeDataReader dr)
        {
           
            MSTSalesRep child = new MSTSalesRep();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static MSTSalesRep Get(int? emKey)
        {
            
            MSTSalesRep child = new MSTSalesRep();
            child.Fetch(new Criteria(emKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _emKey = null;
            public int? _option = null;
            public string _emID = string.Empty;

            internal Criteria()
            {
            }

            internal Criteria(int? EmKey)
            {
                _emKey = EmKey;
            }

            internal Criteria(int? EmKey, int? Option)
            {
                _emKey = EmKey;
                _option = Option;
            }
            //Add Thida
            internal Criteria(int? EmKey, string EmID, int? Option)
            {
                _emKey = EmKey;
                _emID = EmID;
                _option = Option;
            }

            internal Criteria(int? EmKey, string EmID)
            {
                _emKey = EmKey;
                _emID = EmID;
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
                cm.CommandText = "MSTSalesRep_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@EmKey", GFunc.IsNE(criteria._emKey) ? 0 : criteria._emKey);
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
            _emKey = dr.GetInt32("EmKey");
            _emID = dr.GetString("EmID");
            _emNm = dr.GetString("EmNm");
            _emClass = dr.GetString("EmClass");

            //EmDOB
            if (GFunc.IsNE(dr.GetValue("EmDOB")))
                _emDOB = null;
            else
                _emDOB = dr.GetDateTime("EmDOB");

            _emRef = dr.GetString("EmRef");
            _emEmail = dr.GetString("EmEmail");
            _userKey = dr.GetInt32("UserKey");
            _jobCostGrpKey = dr.GetInt32("JobCostGrp");
            _jobLabourItmKey = dr.GetInt32("JobLabourItmKey");
            _inactive = dr.GetBoolean("Inactive");
            //DateHired
            if (GFunc.IsNE(dr.GetValue("DateHired")))
                _dateHired = null;
            else
                _dateHired = dr.GetDateTime("DateHired");

            //DateTerminated                
            if (GFunc.IsNE(dr.GetValue("DateTerminated")))
                _dateTerminated = null;
            else
                _dateTerminated = dr.GetDateTime("DateTerminated");

            //CreateDate
            if (GFunc.IsNE(dr.GetValue("CreateDate")))
                _createDate = null;
            else
                _createDate = dr.GetDateTime("CreateDate");

            //LastModifiedDate
            if (GFunc.IsNE(dr.GetValue("LastModifiedDate")))
                _lastModifiedDate = null;
            else
                _lastModifiedDate = dr.GetDateTime("LastModifiedDate");

            _createUserKey = dr.GetInt32("CreateUserKey");
            _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            _custom4 = dr.GetString("Custom4");
            _custom5 = dr.GetString("Custom5");
            _saleLimit = dr.GetDecimal("SaleLimit");
            _purchaseLimit = dr.GetDecimal("PurchaseLimit");
            _finalSaleApprover = dr.GetString("FinalSaleApprover");
            _finalSaleAppKey = dr.GetInt32("FinalSaleAppKey");
            _marginLimitForFinalApprover = dr.GetInt32("MarginLimitForFinalApprover");
            ValidationRules.CheckRules();

            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? emKey)
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
                    retValue = this.Insert(cn,out emKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? emKey)
        {
            emKey = 0;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTSalesRep_AddUpdate";
                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@NewEmKey", emKey);

                if (_emKey == null)
                    cm.Parameters.AddWithValue("@EmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EmKey", _emKey);

                if (_emID == null)
                    cm.Parameters.AddWithValue("@EmID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EmID", _emID);

                if (_emNm == null)
                    cm.Parameters.AddWithValue("@EmNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EmNm", _emNm);

                if (_emClass == null)
                    cm.Parameters.AddWithValue("@EmClass", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EmClass", _emClass);

                if (_emDOB == null)
                    cm.Parameters.AddWithValue("@EmDOB", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EmDOB", _emDOB.Value);

                if (_emRef == null)
                    cm.Parameters.AddWithValue("@EmRef", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EmRef", _emRef);

                if (_emEmail == null)
                    cm.Parameters.AddWithValue("@EmEmail", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EmEmail", _emEmail);

                if (_userKey == null)
                    cm.Parameters.AddWithValue("@UserKey", 0);
                else
                    cm.Parameters.AddWithValue("@UserKey", _userKey);

                if (_jobCostGrpKey == null)
                    cm.Parameters.AddWithValue("@JobCostGrp", 0);
                else
                    cm.Parameters.AddWithValue("@JobCostGrp", _jobCostGrpKey);

                if (_jobLabourItmKey == null)
                    cm.Parameters.AddWithValue("@JobLabourItmKey", 0);
                else
                    cm.Parameters.AddWithValue("@JobLabourItmKey", _jobLabourItmKey);

                if (_inactive == null)
                    cm.Parameters.AddWithValue("@Inactive", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Inactive", _inactive);

                if (_dateHired == null)
                    cm.Parameters.AddWithValue("@DateHired", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DateHired", _dateHired.Value);

                if (_dateTerminated == null)
                    cm.Parameters.AddWithValue("@DateTerminated", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DateTerminated", _dateTerminated.Value);

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

                if (_custom4 == null)
                    cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom4", _custom4);

                if (_custom5 == null)
                    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom5", _custom5);

                if (_saleLimit == null)
                    cm.Parameters.AddWithValue("@SaleLimit", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SaleLimit", _saleLimit);

                if (_purchaseLimit == null)
                    cm.Parameters.AddWithValue("@PurchaseLimit", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurchaseLimit", _purchaseLimit);


                if (_finalSaleApprover == null)
                    cm.Parameters.AddWithValue("@FinalSaleAppKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@FinalSaleAppKey", _finalSaleAppKey);

                if(_marginLimitForFinalApprover==null)
                    cm.Parameters.AddWithValue("@MarginLimitForFinalApprover", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MarginLimitForFinalApprover", _marginLimitForFinalApprover);


                cm.Parameters["@NewEmKey"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();


                emKey = (int)cm.Parameters["@NewEmKey"].Value;

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
                cm.CommandText = "MSTSalesRep_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);

                cm.Parameters.AddWithValue("@NewEmKey", 0);

                if (_emKey == null)
                    cm.Parameters.AddWithValue("@EmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EmKey", _emKey);

                if (_emID == null)
                    cm.Parameters.AddWithValue("@EmID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EmID", _emID);

                if (_emNm == null)
                    cm.Parameters.AddWithValue("@EmNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EmNm", _emNm);

                if (_emClass == null)
                    cm.Parameters.AddWithValue("@EmClass", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EmClass", _emClass);

                if (_emDOB == null)
                    cm.Parameters.AddWithValue("@EmDOB", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EmDOB", _emDOB.Value);

                if (_emRef == null)
                    cm.Parameters.AddWithValue("@EmRef", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EmRef", _emRef);

                if (_emEmail == null)
                    cm.Parameters.AddWithValue("@EmEmail", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EmEmail", _emEmail);

                if (_userKey == null)
                    cm.Parameters.AddWithValue("@UserKey", 0);
                else
                    cm.Parameters.AddWithValue("@UserKey", _userKey);

                if (_jobCostGrpKey == null)
                    cm.Parameters.AddWithValue("@JobCostGrp", 0);
                else
                    cm.Parameters.AddWithValue("@JobCostGrp", _jobCostGrpKey);

                if (_jobLabourItmKey == null)
                    cm.Parameters.AddWithValue("@JobLabourItmKey", 0);
                else
                    cm.Parameters.AddWithValue("@JobLabourItmKey", _jobLabourItmKey);

                if (_inactive == null)
                    cm.Parameters.AddWithValue("@Inactive", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Inactive", _inactive);

                if (_dateHired == null)
                    cm.Parameters.AddWithValue("@DateHired", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DateHired", _dateHired.Value);

                if (_dateTerminated == null)
                    cm.Parameters.AddWithValue("@DateTerminated", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DateTerminated", _dateTerminated.Value);

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

                if (_custom4 == null)
                    cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom4", _custom4);

                if (_custom5 == null)
                    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom5", _custom5);

                if (_saleLimit == null)
                    cm.Parameters.AddWithValue("@SaleLimit", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SaleLimit", _saleLimit);

                if (_purchaseLimit == null)
                    cm.Parameters.AddWithValue("@PurchaseLimit", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurchaseLimit", _purchaseLimit);

                if (_finalSaleApprover == null)
                    cm.Parameters.AddWithValue("@FinalSaleAppKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@FinalSaleAppKey", _finalSaleAppKey);

                if (_marginLimitForFinalApprover == null)
                    cm.Parameters.AddWithValue("@MarginLimitForFinalApprover", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MarginLimitForFinalApprover", _marginLimitForFinalApprover);               


                cm.Parameters["@NewEmKey"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
                cm.CommandText = "MSTSalesRep_Delete";


                cm.Parameters.AddWithValue("@EmKey", criteria._emKey);

                cm.Parameters.AddWithValue("@RetValue", 0);


                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                {
                    return false;
                }

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
                cm.CommandText = "MSTSalesRep_Validation";
                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@EmKey", criteria._emKey);
                cm.Parameters.AddWithValue("@EmID", criteria._emID);
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


        private void Clear()
        {
            _emKey = 0;
            _emID = string.Empty;
            _emNm = string.Empty;
            _emClass = string.Empty;
            _emDOB = null;
            _emRef = string.Empty;
            _emEmail = string.Empty;
            _userKey = 0;
            _jobCostGrpKey = 0;
            _jobLabourItmKey = 0;
            _inactive = false;
            _dateHired = null;
            _dateTerminated = null;
            _createDate = null;
            _createUserKey = null;
            _lastModifiedDate = null;
            _lastModifiedUserKey = null;
            _custom1 = string.Empty;
            _custom2 = string.Empty;
            _custom3 = string.Empty;
            _custom4 = string.Empty;
            _custom5 = string.Empty;
            _saleLimit = 0;
            _purchaseLimit = 0;
            _finalSaleApprover = string.Empty;
            _marginLimitForFinalApprover = 0;

        }
    
    }
}