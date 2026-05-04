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
    public class MSTBudget : Csla.BusinessBase<MSTBudget>,System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _budgetType = 0;
        internal int? _budgetBranchKey = 0;
        internal int? _budgetDeptKey = 0;
        internal int? _budgetRecKey = 0;
        internal int? _budgetRecSubKey = 0;
        internal int? _budgetPeriod = 0;
        internal int? _budgetMode = 0;
        internal decimal _budgetAmountH = 0;
        internal int? _budgetItmMode = 0;
        internal decimal? _budgetQty = 0;
        internal decimal? _budgetWeight = 0;
        internal DateTime? _createDate = DateTime.Today.Date ;
        internal int? _createUserKey = 0;
        internal DateTime? _lastModifiedDate = DateTime.Today.Date;
        internal int? _lastModifiedUserKey = 0;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _periodText = string.Empty;
        //internal DateTime? _periodStDate = null;
        //internal DateTime? _periodEdDate = null;
        //internal int? _periodSeq = 0;
        internal string _error = string.Empty;

        public int? BudgetType
        {
            get
            {
                return _budgetType;
            }
            set
            {
                _budgetType = value;
                PropertyHasChanged("BudgetType");
            }
        }

        public int? BudgetBranchKey
        {
            get
            {
                return _budgetBranchKey;
            }
            set
            {
                _budgetBranchKey = value;
                PropertyHasChanged("BudgetBranchKey");
            }
        }

        public int? BudgetDeptKey
        {
            get
            {
                return _budgetDeptKey;
            }
            set
            {
                _budgetDeptKey = value;
                PropertyHasChanged("BudgetDeptKey");
            }
        }

        public int? BudgetRecKey
        {
            get
            {
                return _budgetRecKey;
            }
            set
            {
                _budgetRecKey = value;
                PropertyHasChanged("BudgetRecKey");
            }
        }

        public int? BudgetRecSubKey
        {
            get
            {
                return _budgetRecSubKey;
            }
            set
            {
                _budgetRecSubKey = value;
                PropertyHasChanged("BudgetRecSubKey");
            }
        }

        public int? BudgetPeriod
        {
            get
            {
                return _budgetPeriod;
            }
            set
            {
                _budgetPeriod = value;
                PropertyHasChanged("BudgetPeriod");
            }
        }

        public int? BudgetMode
        {
            get
            {
                return _budgetMode;
            }
            set
            {
                _budgetMode = value;
                PropertyHasChanged("BudgetMode");
            }
        }

        public decimal BudgetAmountH
        {
            get
            {
                return _budgetAmountH;
            }
            set
            {
                _budgetAmountH = value;
                PropertyHasChanged("BudgetAmountH");
            }
        }

        public int? BudgetItmMode
        {
            get
            {
                return _budgetItmMode;
            }
            set
            {
                _budgetItmMode = value;
                PropertyHasChanged("BudgetItmMode");
            }
        }

        public decimal? BudgetQty
        {
            get
            {
                return _budgetQty;
            }
            set
            {
                _budgetQty = value;
                PropertyHasChanged("BudgetQty");
            }
        }

        public decimal? BudgetWeight
        {
            get
            {
                return _budgetWeight;
            }
            set
            {
                _budgetWeight = value;
                PropertyHasChanged("BudgetWeight");
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
        public string PeriodText
        {
            get
            {
                return _periodText;
            }           
        }
        //public DateTime? PeriodStDate
        //{
        //    get
        //    {
        //        return _periodStDate;
        //    }            
        //}

        //public DateTime? PeriodEdDate
        //{
        //    get
        //    {
        //        return _periodEdDate;
        //    }           
        //}

        //public int? PeriodSeq
        //{
        //    get
        //    {
        //        return _periodSeq;
        //    }
        //}

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

        protected override object GetIdValue()
        {
            return _budgetType.ToString() + _budgetBranchKey.ToString() + _budgetDeptKey.ToString() + _budgetRecKey.ToString() + _budgetRecSubKey.ToString() + _budgetPeriod.ToString();
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

        internal MSTBudget()
        { /* require use of factory method */ }

        internal static MSTBudget New()
        {
            
            MSTBudget child = new MSTBudget();
            
            return child;
        }

        internal static MSTBudget NewChild()
        {
            
            MSTBudget child = new MSTBudget();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static MSTBudget Get(SafeDataReader dr)
        {
            
            MSTBudget child = new MSTBudget();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTBudget Get(int? budgetType, int? budgetBranchKey, int? budgetDeptKey, int? budgetRecKey, int? budgetRecSubKey)
        {
            
            MSTBudget child = new MSTBudget();
            child.Fetch(new Criteria(budgetType, budgetBranchKey, budgetDeptKey, budgetRecKey, budgetRecSubKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _budgetType = null;
            public int? _budgetBranchKey = null;
            public int? _budgetDeptKey = null;
            public int? _budgetRecKey = null;
            public int? _budgetRecSubKey = null;
            public int? _budgetPeriod = null;
            public int? _periodFrom = null;
            public int? _periodTo = null;
            public int? _option = null;

            public string _budgetBranchFrom ;
            public string _budgetBranchTo;
            
            public string _budgetDeptFrom;
            public string _budgetDeptTo;

            internal Criteria()
            {
            }

            internal Criteria(int? BudgetType, int? BudgetBranchKey, int? BudgetDeptKey, int? BudgetRecKey, int? BudgetRecSubKey)
            {
                _budgetType = BudgetType;
                _budgetBranchKey = BudgetBranchKey;
                _budgetDeptKey = BudgetDeptKey;
                _budgetRecKey = BudgetRecKey;
                _budgetRecSubKey = BudgetRecSubKey;
            }

            internal Criteria(int? BudgetType, int? BudgetBranchKey, int? BudgetDeptKey, int? BudgetRecKey, int? BudgetRecSubKey, int? Option)
            {
                _budgetType = BudgetType;
                _budgetBranchKey = BudgetBranchKey;
                _budgetDeptKey = BudgetDeptKey;
                _budgetRecKey = BudgetRecKey;
                _budgetRecSubKey = BudgetRecSubKey;
                _option = Option;
            }

            internal Criteria(int? BudgetType, int? BudgetBranchKey, int? BudgetDeptKey, int? BudgetRecKey, int? BudgetRecSubKey, int BudgetPeriod, int? Option)
            {
                _budgetType = BudgetType;
                _budgetBranchKey = BudgetBranchKey;
                _budgetDeptKey = BudgetDeptKey;
                _budgetRecKey = BudgetRecKey;
                _budgetRecSubKey = BudgetRecSubKey;
                _budgetPeriod = BudgetPeriod;
                _option = Option;
            }

            internal Criteria(int? BudgetType, string BudgetBranchFrom, string BudgetBranchTo, string BudgetDeptFrom, string BudgetDeptTo, int? BudgetRecKey, int? BudgetRecSubKey, int PeriodFrom, int PeriodTo, int? Option)
            {
                _budgetType = BudgetType;
                _budgetBranchFrom = BudgetBranchFrom;
                _budgetBranchTo = BudgetBranchTo;
                _budgetDeptFrom = BudgetDeptFrom;
                _budgetDeptTo = BudgetDeptTo;
                _budgetRecKey = BudgetRecKey;
                _budgetRecSubKey = BudgetRecSubKey;
                _periodFrom = PeriodFrom;
                _periodTo = PeriodTo;
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
                cm.CommandText = "MSTBudget_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                   
                cm.Parameters.AddWithValue("@BudgetType", criteria._budgetType);
                cm.Parameters.AddWithValue("@BudgetBranchKey", criteria._budgetBranchKey);
                cm.Parameters.AddWithValue("@BudgetBranchKeyTo", 0);
                cm.Parameters.AddWithValue("@BudgetDeptKey", criteria._budgetDeptKey);
                cm.Parameters.AddWithValue("@BudgetDeptKeyTo", 0);                
                cm.Parameters.AddWithValue("@BudgetRecKey", criteria._budgetRecKey);
                cm.Parameters.AddWithValue("@BudgetRecSubKey", criteria._budgetRecSubKey);
                cm.Parameters.AddWithValue("@BudgetPeriod", criteria._budgetPeriod);
                
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
                    retValue = false;
               

            }// Already close and dispose sql connection.
            
            return retValue;
        }
        internal bool Fetch(IDataReader dr)
        {
            bool retValue = false;
            
            try
            {
                _budgetType = dr["BudgetType"] == DBNull.Value ? null : (int?) dr["BudgetType"];
                _budgetBranchKey = dr["BudgetBranchKey"] == DBNull.Value ? null : (int?)dr["BudgetBranchKey"];
                _budgetDeptKey = dr["BudgetDeptKey"] == DBNull.Value ? null : (int?)dr["BudgetDeptKey"];
                _budgetRecKey = dr["BudgetRecKey"] == DBNull.Value ? null : (int?)dr["BudgetRecKey"];
                _budgetRecSubKey = dr["BudgetRecSubKey"] == DBNull.Value ? null : (int?)dr["BudgetRecSubKey"]; 
                _budgetPeriod = dr["BudgetPeriod"] == DBNull.Value ? null : (int?)dr["BudgetPeriod"]; 
                _budgetMode = dr["BudgetMode"] == DBNull.Value ? null : (int?)dr["BudgetMode"];
                _budgetAmountH = dr["BudgetAmountH"] == DBNull.Value ? 0 : (decimal)dr["BudgetAmountH"];
                _budgetItmMode = dr["BudgetItmMode"] == DBNull.Value ? null : (int?)dr["BudgetItmMode"];
                _budgetQty = dr["BudgetQty"] == DBNull.Value ? null : (decimal?)dr["BudgetQty"];
                _budgetWeight = dr["BudgetWeight"] == DBNull.Value ? null : (decimal?)dr["BudgetWeight"];
                _createDate = dr["CreateDate"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dr["CreateDate"];              
                _createUserKey = dr["CreateUserKey"] == DBNull.Value ? null : (int?)dr["CreateUserKey"];
                _lastModifiedDate = dr["LastModifiedDate"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dr["LastModifiedDate"];
                _lastModifiedUserKey = dr["LastModifiedUserKey"] == DBNull.Value ? null : (int?)dr["LastModifiedUserKey"]; 
                _custom1 = dr["Custom1"] == DBNull.Value ? null : (string)dr["Custom1"];
                _custom2 = dr["Custom2"] == DBNull.Value ? null : (string)dr["Custom2"];
                _custom3 = dr["Custom3"] == DBNull.Value ? null : (string)dr["Custom3"]; 

                if(_budgetPeriod!=null)
                    _periodText = new DateTime(_budgetPeriod.Value / 100, _budgetPeriod.Value % 100, 1).ToString("yyyy MMM");
               
                ValidationRules.CheckRules();
                retValue = true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return retValue;
        }

        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert( out int? budgetType, out int? budgetBranchKey, out int? budgetDeptKey, out int? budgetRecKey, out int? budgetRecSubKey, out int? budgetPeriod)
        {
            bool retValue=false;
            budgetType = null;
            budgetBranchKey = null;
            budgetDeptKey = null;
            budgetRecKey = null;
            budgetRecSubKey = null;
            budgetPeriod = null;
            
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
                cm.CommandText = "MSTBudget_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);
             

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;                   

                if (_budgetType == null)
                    cm.Parameters.AddWithValue("@BudgetType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetType", _budgetType);

                if (_budgetBranchKey == null)
                    cm.Parameters.AddWithValue("@BudgetBranchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetBranchKey", _budgetBranchKey);

                if (_budgetDeptKey == null)
                    cm.Parameters.AddWithValue("@BudgetDeptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetDeptKey", _budgetDeptKey);

                if (_budgetRecKey == null)
                    cm.Parameters.AddWithValue("@BudgetRecKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetRecKey", _budgetRecKey);

                if (_budgetRecSubKey == null)
                    cm.Parameters.AddWithValue("@BudgetRecSubKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetRecSubKey", _budgetRecSubKey);

                if (_budgetPeriod == null)
                    cm.Parameters.AddWithValue("@BudgetPeriod", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetPeriod", _budgetPeriod);

                if (_budgetMode == null)
                    cm.Parameters.AddWithValue("@BudgetMode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetMode", _budgetMode);

                if (_budgetAmountH == null)
                    cm.Parameters.AddWithValue("@BudgetAmountH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetAmountH", _budgetAmountH);

                if (_budgetItmMode == null)
                    cm.Parameters.AddWithValue("@BudgetItmMode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetItmMode", _budgetItmMode);

                if (_budgetQty == null)
                    cm.Parameters.AddWithValue("@BudgetQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetQty", _budgetQty);

                if (_budgetWeight == null)
                    cm.Parameters.AddWithValue("@BudgetWeight", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetWeight", _budgetWeight);

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
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTBudget_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
               
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                if (_budgetType == null)
                    cm.Parameters.AddWithValue("@BudgetType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetType", _budgetType);

                if (_budgetBranchKey == null)
                    cm.Parameters.AddWithValue("@BudgetBranchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetBranchKey", _budgetBranchKey);

                if (_budgetDeptKey == null)
                    cm.Parameters.AddWithValue("@BudgetDeptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetDeptKey", _budgetDeptKey);

                if (_budgetRecKey == null)
                    cm.Parameters.AddWithValue("@BudgetRecKey", 0);
                else
                    cm.Parameters.AddWithValue("@BudgetRecKey", _budgetRecKey);

                if (_budgetRecSubKey == null)
                    cm.Parameters.AddWithValue("@BudgetRecSubKey", 0);
                else
                    cm.Parameters.AddWithValue("@BudgetRecSubKey", _budgetRecSubKey);

                if (_budgetPeriod == null)
                    cm.Parameters.AddWithValue("@BudgetPeriod", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetPeriod", _budgetPeriod);

                if (_budgetMode == null)
                    cm.Parameters.AddWithValue("@BudgetMode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetMode", _budgetMode);

                if (_budgetAmountH == null)
                    cm.Parameters.AddWithValue("@BudgetAmountH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetAmountH", _budgetAmountH);

                if (_budgetItmMode == null)
                    cm.Parameters.AddWithValue("@BudgetItmMode", 0);
                else
                    cm.Parameters.AddWithValue("@BudgetItmMode", _budgetItmMode);

                if (_budgetQty == null)
                    cm.Parameters.AddWithValue("@BudgetQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetQty", _budgetQty);

                if (_budgetWeight == null)
                    cm.Parameters.AddWithValue("@BudgetWeight", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetWeight", _budgetWeight);

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
                cm.CommandText = "MSTBudget_Delete";

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@BudgetType", criteria._budgetType);
                cm.Parameters.AddWithValue("@BudgetBranchKey", criteria._budgetBranchKey);
                cm.Parameters.AddWithValue("@BudgetDeptKey", criteria._budgetDeptKey);
                cm.Parameters.AddWithValue("@BudgetRecKey", criteria._budgetRecKey);
                cm.Parameters.AddWithValue("@BudgetRecSubKey", criteria._budgetRecSubKey);

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
               

            }// Already close and dispose sql connection.
            
        }

        #endregion //Data Access - Delete

        #region Data Access - Validation

        internal bool Validation(Criteria criteria,  bool? isNew)
        {
            bool retValue = false;

            using (TransactionScope scope = new TransactionScope())
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();

                    retValue = this.Validation(cn, criteria, isNew);
                }

                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }
            
            return retValue;
        }

        internal bool Validation(SqlConnection cn, Criteria criteria,  bool? isNew)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTBudget_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);
               

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@BudgetType", criteria._budgetType);
                cm.Parameters.AddWithValue("@BudgetBranchKey", criteria._budgetBranchKey);
                cm.Parameters.AddWithValue("@BudgetDeptKey", criteria._budgetDeptKey);
                cm.Parameters.AddWithValue("@BudgetRecKey", criteria._budgetRecKey);
                cm.Parameters.AddWithValue("@BudgetRecSubKey", criteria._budgetRecSubKey);
                cm.Parameters.AddWithValue("@BudgetPeriod", criteria._budgetPeriod);

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }
        }
        #endregion 

        #region Data Access - Check 

        internal bool CheckData(Criteria criteria)
        {
            bool retValue = false;

            using (TransactionScope scope = new TransactionScope())
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();

                    retValue = this.CheckData(cn, criteria);
                }

                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }

            return retValue;
        }

        internal bool CheckData(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTBudget_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@BudgetType", criteria._budgetType);
                cm.Parameters.AddWithValue("@BudgetBranchIDFrom", criteria._budgetBranchFrom);
                cm.Parameters.AddWithValue("@BudgetBranchIDTo", criteria._budgetBranchTo);
                cm.Parameters.AddWithValue("@BudgetDeptIDFrom", criteria._budgetDeptFrom);
                cm.Parameters.AddWithValue("@BudgetDeptIDTo", criteria._budgetDeptTo);
                cm.Parameters.AddWithValue("@BudgetRecKey", criteria._budgetRecKey);
                cm.Parameters.AddWithValue("@BudgetRecSubKey", criteria._budgetRecSubKey);
                cm.Parameters.AddWithValue("@BudgetPeriod", criteria._periodFrom);
                cm.Parameters.AddWithValue("@BudgetPeriodTo", criteria._periodTo);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }
        }
        #endregion 

        private void Clear()
        {
            _budgetType = 0;
            _budgetBranchKey = 0;
            _budgetDeptKey = 0;
            _budgetRecKey = 0;
            _budgetRecSubKey = 0;
            _budgetPeriod = 0;
            _budgetMode = 0;
            _budgetAmountH = 0;
            _budgetItmMode = 0;
            _budgetQty = 0;
            _budgetWeight = 0;
            _createDate = DateTime.Today.Date;
            _createUserKey = 0;
            _lastModifiedDate = DateTime.Today.Date;
            _lastModifiedUserKey = 0;
            _custom1 = string.Empty;
            _custom2 = string.Empty;
            _custom3 = string.Empty;
            _periodText = string.Empty;
            
        }
    }
}


