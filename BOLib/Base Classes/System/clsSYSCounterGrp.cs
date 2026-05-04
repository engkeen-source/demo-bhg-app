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
    public class SYSCounterGrp : Csla.BusinessBase<SYSCounterGrp>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _codeKey = null;
        internal int? _counterGrp = 0;
        internal int? _initialNumber = 1000;
        internal bool? _reset = false;

        public int? CodeKey
        {
            get
            {
                return _codeKey;
            }
        }

        public int? CounterGrp
        {
            get
            {
                return _counterGrp;
            }
        }

        public int? InitialNumber
        {
            get
            {             
                return _initialNumber;
            }
            set
            {
                _initialNumber = value;
                PropertyHasChanged("InitialNumber");               
            }
        }

        public bool? Reset
        {
            get
            {             
                return _reset;
            }
            set
            {               
                _reset = value;
                PropertyHasChanged("Reset");
            }
        }

        protected override object GetIdValue()
        {
            return _codeKey.ToString() + _counterGrp.ToString();
        }

        #endregion //Business Properties and Methods

        #region Validation Rules
        private void AddCustomRules()
        {
            //add custom/non-generated rules here...
        }

        private void AddCommonRules()
        {

        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal SYSCounterGrp()
        { /* require use of factory method */ }

        internal static SYSCounterGrp New()
        {
            
            SYSCounterGrp child = new SYSCounterGrp();
            
            return child;
        }

        internal static SYSCounterGrp NewChild()
        {
            //
            SYSCounterGrp child = new SYSCounterGrp();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            //
            return child;
        }

        internal static SYSCounterGrp Get(SafeDataReader dr)
        {
            //
            SYSCounterGrp child = new SYSCounterGrp();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SYSCounterGrp Get(GEnum.SystemCode codeKey, int? counterGrp)
        {
            //
            SYSCounterGrp child = new SYSCounterGrp();
            child.Fetch(new Criteria(codeKey, counterGrp, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public GEnum.SystemCode? _codeKey = null;
            public int? _counterGrp = null;
            public int? _option = null;

            internal Criteria()
            {
            }        

            internal Criteria(GEnum.SystemCode? CodeKey, int? CounterGrp)
            {
                _codeKey = CodeKey;
                _counterGrp = CounterGrp;
            }

            internal Criteria(GEnum.SystemCode? CodeKey, int? CounterGrp, int? Option)
            {
                _codeKey = CodeKey;
                _counterGrp = CounterGrp;
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
                cm.CommandText = "SYSCounterGrp_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);
                cm.Parameters.AddWithValue("@CounterGrp", criteria._counterGrp);
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
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _codeKey = dr.GetInt32("CodeKey");
            _counterGrp = dr.GetInt32("CounterGrp");
            _initialNumber = dr.GetInt32("InitialNumber");
            _reset = dr.GetBoolean("Reset");
            ValidationRules.CheckRules();
            return true;        
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert()
        {
            bool retValue = false;
            //
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
                cm.CommandText = "SYSCounterGrp_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                if (_codeKey == null)
                    cm.Parameters.AddWithValue("@CodeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeKey", _codeKey);

                if (_counterGrp == null)
                    cm.Parameters.AddWithValue("@CounterGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CounterGrp", _counterGrp);

                if (_initialNumber == null)
                    cm.Parameters.AddWithValue("@InitialNumber", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@InitialNumber", _initialNumber);

                if (_reset == null)
                    cm.Parameters.AddWithValue("@Reset", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Reset", _reset);


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
                cm.CommandText = "SYSCounterGrp_Delete";

                cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);
                //cm.Parameters.AddWithValue("@CounterGrp", criteria._counterGrp);
               // cm.Parameters.AddWithValue("@Option", criteria._option);
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
