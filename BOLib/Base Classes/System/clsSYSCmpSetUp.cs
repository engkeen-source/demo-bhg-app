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
    public class SYSCmpSetUp : Csla.BusinessBase<SYSCmpSetUp>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _taskSeq = null;
        internal int? _taskCode = null;
        internal string _taskDesLang1 = string.Empty;
        internal string _taskDesLang2 = string.Empty;
        internal string _taskDesLang3 = string.Empty;
        internal string _taskDesLang4 = string.Empty;
        internal string _taskDesLang5 = string.Empty;
        internal string _taskDesLang6 = string.Empty;
        internal string _taskDesLang7 = string.Empty;
        internal string _taskDesLang8 = string.Empty;
        internal string _taskDesLang9 = string.Empty;
        internal string _taskDesLang10 = string.Empty;
        internal bool? _taskCompleted = false;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public int? TaskSeq
        {
            get
            {
                return _taskSeq;
            }
        }

        public int? TaskCode
        {
            get
            {
                return _taskCode;
            }
        }

        public string TaskDesLang1
        {
            get
            {
                return _taskDesLang1;
            }
        
        }

        public string TaskDesLang2
        {
            get
            {
                return _taskDesLang2;
            }
        }

        public string TaskDesLang3
        {
            get
            {
                return _taskDesLang3;
            }
        }

        public string TaskDesLang4
        {
            get
            {
                return _taskDesLang4;
            }
        }

        public string TaskDesLang5
        {
            get
            {
                return _taskDesLang5;
            }
        }

        public string TaskDesLang6
        {
            get
            {
                return _taskDesLang6;
            }
        }

        public string TaskDesLang7
        {
            get
            {
                return _taskDesLang7;
            }
        }

        public string TaskDesLang8
        {
            get
            {
                return _taskDesLang8;
            }
        }

        public string TaskDesLang9
        {
            get
            {
                return _taskDesLang9;
            }
        }

        public string TaskDesLang10
        {
            get
            {
                return _taskDesLang10;
            }
        }

        public bool? TaskCompleted
        {
            get
            {
                return _taskCompleted;
            }
            set
            {
                _taskCompleted = value;
                PropertyHasChanged("TaskCompleted");
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
            return _taskSeq.ToString();
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
            //// TaskDesLang1
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "TaskDesLang1");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TaskDesLang1", 255));
            ////
            //// TaskDesLang2
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TaskDesLang2", 255));
            ////
            //// TaskDesLang3
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TaskDesLang3", 255));
            ////
            //// TaskDesLang4
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TaskDesLang4", 255));
            ////
            //// TaskDesLang5
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TaskDesLang5", 255));
            ////
            //// TaskDesLang6
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TaskDesLang6", 255));
            ////
            //// TaskDesLang7
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TaskDesLang7", 255));
            ////
            //// TaskDesLang8
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TaskDesLang8", 255));
            ////
            //// TaskDesLang9
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TaskDesLang9", 255));
            ////
            //// TaskDesLang10
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TaskDesLang10", 255));
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

        internal SYSCmpSetUp()
        { /* require use of factory method */ }

        internal static SYSCmpSetUp New()
        {
            
            SYSCmpSetUp child = new SYSCmpSetUp();
            
            return child;
        }

        internal static SYSCmpSetUp NewChild()
        {
            
            SYSCmpSetUp child = new SYSCmpSetUp();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static SYSCmpSetUp Get(SafeDataReader dr)
        {
            
            SYSCmpSetUp child = new SYSCmpSetUp();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SYSCmpSetUp Get(int? taskSeq)
        {
            
            SYSCmpSetUp child = new SYSCmpSetUp();
            child.Fetch(new Criteria(taskSeq, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _taskSeq = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? TaskSeq)
            {
                _taskSeq = TaskSeq;
            }

            internal Criteria(int? TaskSeq, int? Option)
            {
                _taskSeq = TaskSeq;
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
                cm.CommandText = "SYSCmpSetUp_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@TaskSeq", criteria._taskSeq);

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
            
            _taskSeq = dr.GetInt32("TaskSeq");
            _taskCode = dr.GetInt32("TaskCode");
            _taskDesLang1 = dr.GetString("TaskDesLang1");
            _taskDesLang2 = dr.GetString("TaskDesLang2");
            _taskDesLang3 = dr.GetString("TaskDesLang3");
            _taskDesLang4 = dr.GetString("TaskDesLang4");
            _taskDesLang5 = dr.GetString("TaskDesLang5");
            _taskDesLang6 = dr.GetString("TaskDesLang6");
            _taskDesLang7 = dr.GetString("TaskDesLang7");
            _taskDesLang8 = dr.GetString("TaskDesLang8");
            _taskDesLang9 = dr.GetString("TaskDesLang9");
            _taskDesLang10 = dr.GetString("TaskDesLang10");
            _taskCompleted = dr.GetBoolean("TaskCompleted");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            ValidationRules.CheckRules();
            return true;           
        }
        #endregion //Data Access - Fetch

        #region Data Access - Update

        internal bool Update()
        {
            bool retVal = false;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call update method.
                    retVal=this.Update(cn);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            return retVal;
        }

        internal bool Update(SqlConnection cn)
        {
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSCmpSetUp_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                

                if (_taskSeq == null)
                    cm.Parameters.AddWithValue("@TaskSeq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaskSeq", _taskSeq);

                if (_taskCode == null)
                    cm.Parameters.AddWithValue("@TaskCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaskCode", _taskCode);

                if (_taskDesLang1 == null)
                    cm.Parameters.AddWithValue("@TaskDesLang1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaskDesLang1", _taskDesLang1);

                if (_taskDesLang2 == null)
                    cm.Parameters.AddWithValue("@TaskDesLang2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaskDesLang2", _taskDesLang2);

                if (_taskDesLang3 == null)
                    cm.Parameters.AddWithValue("@TaskDesLang3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaskDesLang3", _taskDesLang3);

                if (_taskDesLang4 == null)
                    cm.Parameters.AddWithValue("@TaskDesLang4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaskDesLang4", _taskDesLang4);

                if (_taskDesLang5 == null)
                    cm.Parameters.AddWithValue("@TaskDesLang5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaskDesLang5", _taskDesLang5);

                if (_taskDesLang6 == null)
                    cm.Parameters.AddWithValue("@TaskDesLang6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaskDesLang6", _taskDesLang6);

                if (_taskDesLang7 == null)
                    cm.Parameters.AddWithValue("@TaskDesLang7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaskDesLang7", _taskDesLang7);

                if (_taskDesLang8 == null)
                    cm.Parameters.AddWithValue("@TaskDesLang8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaskDesLang8", _taskDesLang8);

                if (_taskDesLang9 == null)
                    cm.Parameters.AddWithValue("@TaskDesLang9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaskDesLang9", _taskDesLang9);

                if (_taskDesLang10 == null)
                    cm.Parameters.AddWithValue("@TaskDesLang10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaskDesLang10", _taskDesLang10);

                if (_taskCompleted == null)
                    cm.Parameters.AddWithValue("@TaskCompleted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TaskCompleted", _taskCompleted);

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

    }
}
