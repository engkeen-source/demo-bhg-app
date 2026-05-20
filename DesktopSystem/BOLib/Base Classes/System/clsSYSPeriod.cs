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
    public class SYSPeriod : Csla.BusinessBase<SYSPeriod>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _period = null;
        internal DateTime? _periodStDate = null;
        internal DateTime? _periodEdDate = null;
        internal int? _periodSeq = 1;
        internal int? _periodYear = null;
        internal int? _periodMth = 1;
        internal int? _periodStatus = 0;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public int? Period
        {
            get
            {
                return _period;
            }
        }

        public DateTime? PeriodStDate
        {
            get
            {
                return _periodStDate;
            }
        }

        public DateTime? PeriodEdDate
        {
            get
            {
                return _periodEdDate;
            }
        }

        public int? PeriodSeq
        {
            get
            {
                return _periodSeq;
            }
        }

        public int? PeriodYear
        {
            get
            {
                return _periodYear;
            }
        }

        public int? PeriodMth
        {
            get
            {
                return _periodMth;
            }
        }

        public int? PeriodStatus
        {
            get
            {
                return _periodStatus;
            }
            set
            {
                _periodStatus = value;
                PropertyHasChanged("PeriodStatus");
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
            return _period.ToString();
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
            //// PeriodStDate
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "PeriodStDateString");
            ////
            //// PeriodEdDate
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "PeriodEdDateString");
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

        public SYSPeriod()
        { /* require use of factory method */ }

        internal static SYSPeriod New()
        {
            SYSPeriod child = new SYSPeriod();
            return child;
        }

        internal static SYSPeriod NewChild()
        {
            SYSPeriod child = new SYSPeriod();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static SYSPeriod Get(SafeDataReader dr)
        {
            SYSPeriod child = new SYSPeriod();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static SYSPeriod Get(int? period)
        {
            SYSPeriod child = new SYSPeriod();
            child.Fetch(new Criteria(period, 1));
            return child;
        }

        public static SYSPeriod Get(int? period,int Option)
        {
            SYSPeriod child = new SYSPeriod();
            child.Fetch(new Criteria(period,0, Option));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            internal int? _periodOrYear = null;
            internal int? _option = null;
            internal int? _periodSeq = 0;
            internal int? _period = 0;

            internal Criteria()
            {
                _option = 0;
            }

            internal Criteria(int? PeriodOrYear)
            {
                _periodOrYear = PeriodOrYear;
            }

            internal Criteria(int? PeriodOrYear, int? Option)
            {
                _periodOrYear = PeriodOrYear;
                _option = Option;
            }

            internal Criteria(int? Period, int? PeriodSeq, int? Option)
            {
                _period = Period;
                _periodSeq = PeriodSeq;
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
                cm.CommandText = "SYSPeriod_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                if (criteria._periodOrYear == null || criteria._periodOrYear == 0)
                    cm.Parameters.AddWithValue("@PeriodYear", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PeriodYear", criteria._periodOrYear);

                cm.Parameters.AddWithValue("@PeriodSeq", criteria._periodSeq);
                cm.Parameters.AddWithValue("@Period", criteria._period);
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
            _period = dr.GetInt32("Period");
            _periodStDate = dr.GetDateTime("PeriodStDate");
            _periodEdDate = dr.GetDateTime("PeriodEdDate");
            _periodSeq = dr.GetInt32("PeriodSeq");
            _periodYear = dr.GetInt32("PeriodYear");
            _periodMth = dr.GetInt32("PeriodMth");
            _periodStatus = dr.GetInt32("PeriodStatus");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            this.MarkOld();
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
                cm.CommandText = "SYSPeriod_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

               if (_period == null)
                    cm.Parameters.AddWithValue("@Period", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Period", _period);

                if (_periodStDate == null)
                    cm.Parameters.AddWithValue("@PeriodStDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PeriodStDate", _periodStDate.Value);

                if (_periodEdDate == null)
                    cm.Parameters.AddWithValue("@PeriodEdDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PeriodEdDate", _periodEdDate.Value);

                if (_periodSeq == null)
                    cm.Parameters.AddWithValue("@PeriodSeq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PeriodSeq", _periodSeq);

                if (_periodYear == null)
                    cm.Parameters.AddWithValue("@PeriodYear", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PeriodYear", _periodYear);

                if (_periodMth == null)
                    cm.Parameters.AddWithValue("@PeriodMth", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PeriodMth", _periodMth);

                if (_periodStatus == null)
                    cm.Parameters.AddWithValue("@PeriodStatus", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PeriodStatus", _periodStatus);

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

                    // Call insert method.
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
                cm.CommandText = "SYSPeriod_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);

                if (_period == null)
                    cm.Parameters.AddWithValue("@Period", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Period", _period);

                if (_periodStDate == null)
                    cm.Parameters.AddWithValue("@PeriodStDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PeriodStDate", _periodStDate.Value);

                if (_periodEdDate == null)
                    cm.Parameters.AddWithValue("@PeriodEdDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PeriodEdDate", _periodEdDate.Value);

                if (_periodSeq == null)
                    cm.Parameters.AddWithValue("@PeriodSeq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PeriodSeq", _periodSeq);

                if (_periodYear == null)
                    cm.Parameters.AddWithValue("@PeriodYear", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PeriodYear", _periodYear);

                if (_periodMth == null)
                    cm.Parameters.AddWithValue("@PeriodMth", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PeriodMth", _periodMth);

                if (_periodStatus == null)
                    cm.Parameters.AddWithValue("@PeriodStatus", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PeriodStatus", _periodStatus);

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
                cm.CommandText = "SYSPeriod_Delete";

                cm.Parameters.AddWithValue("@PeriodYear", criteria._periodOrYear);

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
    }
}
