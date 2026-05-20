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
    public class SYSIDcounter : Csla.BusinessBase<SYSIDcounter>
    {
        #region Business Properties and Methods

        //declare members
        internal GEnum.SystemCode? _codeKey = null;
        internal int? _period = 0;
        internal string _counterGrpStr = "0";
        internal int? _docGrpKey = 0;
        internal int? _conKey = 0;
        internal int? _eMKey = 0;
        internal int? _lastCounter = 0;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public GEnum.SystemCode? CodeKey
        {
            get
            {
                return _codeKey;
            }
        }

        public int? Period
        {
            get
            {
                return _period;
            }
        }

        public string CounterGrpStr
        {
            get
            {
                return _counterGrpStr;
            }
        }

        public int? DocGrpKey
        {
            get
            {
                return _docGrpKey;
            }
        }

        public int? ConKey
        {
            get
            {
                return _conKey;
            }
        }

        public int? EMKey
        {
            get
            {
                return _eMKey;
            }
        }

        public int? LastCounter
        {
            get
            {
                return _lastCounter;
            }
            set
            {
                _lastCounter = value;
                PropertyHasChanged("LastCounter");
            }
        }

        public string Custom1
        {
            get
            {
                CanReadProperty("Custom1", true);
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
            return _codeKey.ToString() + _period.ToString() + _counterGrpStr.ToString() + _docGrpKey.ToString() + _conKey.ToString() + _eMKey.ToString();
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
            //// CounterGrpStr
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "CounterGrpStr");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CounterGrpStr", 50));
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

        internal SYSIDcounter()
        { /* require use of factory method */ }

        internal static SYSIDcounter New()
        {
            
            SYSIDcounter child = new SYSIDcounter();
            
            return child;
        }

        internal static SYSIDcounter NewChild()
        {
            
            SYSIDcounter child = new SYSIDcounter();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static SYSIDcounter Get(SafeDataReader dr)
        {
            
            SYSIDcounter child = new SYSIDcounter();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SYSIDcounter Get(GEnum.SystemCode codeKey, int? period, string counterGrpStr, int? docGrpKey, int? conKey, int? eMKey)
        {
            
            SYSIDcounter child = new SYSIDcounter();
            child.Fetch(new Criteria(codeKey, period, counterGrpStr, docGrpKey, conKey, eMKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public GEnum.SystemCode? _codeKey = null;
            public int? _period = null;
            internal string _counterGrpStr = string.Empty;
            public int? _docGrpKey = null;
            public int? _conKey = null;
            public int? _eMKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(GEnum.SystemCode CodeKey, int? Period, string CounterGrpStr, int? DocGrpKey, int? ConKey, int? EMKey)
            {
                _codeKey = CodeKey;
                _period = Period;
                _counterGrpStr = CounterGrpStr;
                _docGrpKey = DocGrpKey;
                _conKey = ConKey;
                _eMKey = EMKey;
            }

            internal Criteria(GEnum.SystemCode CodeKey, int? Period, string CounterGrpStr, int? DocGrpKey, int? ConKey, int? EMKey, int? Option)
            {
                _codeKey = CodeKey;
                _period = Period;
                _counterGrpStr = CounterGrpStr;
                _docGrpKey = DocGrpKey;
                _conKey = ConKey;
                _eMKey = EMKey;
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
                cm.CommandText = "SYSIDcounter_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);
                if (criteria._period != null)
                    cm.Parameters.AddWithValue("@Period", criteria._period);
                else
                    cm.Parameters.AddWithValue("@Period", DBNull.Value);

                if (criteria._counterGrpStr != null)
                    cm.Parameters.AddWithValue("@CounterGrpStr", criteria._counterGrpStr);
                else
                    cm.Parameters.AddWithValue("@CounterGrpStr", DBNull.Value);

                if (criteria._docGrpKey != null)
                    cm.Parameters.AddWithValue("@DocGrpKey", criteria._docGrpKey);
                else
                    cm.Parameters.AddWithValue("@DocGrpKey", DBNull.Value);

                if (criteria._conKey != null)
                    cm.Parameters.AddWithValue("@ConKey", criteria._conKey);
                else
                    cm.Parameters.AddWithValue("@ConKey", DBNull.Value);

                if (criteria._eMKey != null)
                    cm.Parameters.AddWithValue("@EMKey", criteria._eMKey);
                else
                    cm.Parameters.AddWithValue("@EMKey", DBNull.Value);

                
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
            
            _codeKey = (GEnum.SystemCode?)dr.GetInt32("CodeKey");
            _period = dr.GetInt32("Period");
            _counterGrpStr = dr.GetString("CounterGrpStr");
            _docGrpKey = dr.GetInt32("DocGrpKey");
            _conKey = dr.GetInt32("ConKey");
            _eMKey = dr.GetInt32("EMKey");
            _lastCounter = dr.GetInt32("LastCounter");
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
                cm.CommandText = "SYSIDcounter_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);
                                  

                if (_codeKey == null)
                    cm.Parameters.AddWithValue("@CodeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeKey", _codeKey);

                if (_period == null)
                    cm.Parameters.AddWithValue("@Period", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Period", _period);

                if (_counterGrpStr == null)
                    cm.Parameters.AddWithValue("@CounterGrpStr", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CounterGrpStr", _counterGrpStr);

                if (_docGrpKey == null)
                    cm.Parameters.AddWithValue("@DocGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocGrpKey", _docGrpKey);

                if (_conKey == null)
                    cm.Parameters.AddWithValue("@ConKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey", _conKey);

                if (_eMKey == null)
                    cm.Parameters.AddWithValue("@EMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EMKey", _eMKey);

                if (_lastCounter == null)
                    cm.Parameters.AddWithValue("@LastCounter", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastCounter", _lastCounter);

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
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSIDcounter_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                
              
                if (_codeKey == null)
                    cm.Parameters.AddWithValue("@CodeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeKey", _codeKey);

                if (_period == null)
                    cm.Parameters.AddWithValue("@Period", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Period", _period);

                if (_counterGrpStr == null)
                    cm.Parameters.AddWithValue("@CounterGrpStr", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CounterGrpStr", _counterGrpStr);

                if (_docGrpKey == null)
                    cm.Parameters.AddWithValue("@DocGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocGrpKey", _docGrpKey);

                if (_conKey == null)
                    cm.Parameters.AddWithValue("@ConKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey", _conKey);

                if (_eMKey == null)
                    cm.Parameters.AddWithValue("@EMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EMKey", _eMKey);

                if (_lastCounter == null)
                    cm.Parameters.AddWithValue("@LastCounter", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastCounter", _lastCounter);

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
                cm.CommandText = "SYSIDcounter_Delete";

                
                cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);
                cm.Parameters.AddWithValue("@Period", criteria._period);
                cm.Parameters.AddWithValue("@CounterGrpStr", criteria._counterGrpStr);
                cm.Parameters.AddWithValue("@DocGrpKey", criteria._docGrpKey);
                cm.Parameters.AddWithValue("@ConKey", criteria._conKey);
                cm.Parameters.AddWithValue("@EMKey", criteria._eMKey);

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


