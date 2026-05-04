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
    public class SYSAppDetItm : Csla.BusinessBase<SYSAppDetItm>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _appKey = 0;
        internal string _appObjSub = "NA";
        internal string _appObjItm = "NA";
        internal string _appProperty = string.Empty;

        public int? AppKey
        {
            get
            {
                return _appKey;
            }
        }

        public string AppObjSub
        {
            get
            {
                return _appObjSub;
            }
        }

        public string AppObjItm
        {
            get
            {
                return _appObjItm;
            }
        }

        public string AppProperty
        {
            get
            {
                return _appProperty;
            }
        }

        protected override object GetIdValue()
        {
            return _appKey.ToString() + _appObjSub.ToString() + _appObjItm.ToString();
        }

        #endregion //Business Properties and Methods

        #region Validation Rules
        private void AddCustomRules()
        {
            //add custom/non-generated rules here...
        }

        private void AddCommonRules()
        {
            //
            // AppObjSub
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "AppObjSub");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("AppObjSub", 50));
            //
            // AppObjItm
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "AppObjItm");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("AppObjItm", 50));
            //
            // AppProperty
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "AppProperty");
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal SYSAppDetItm()
        { /* require use of factory method */ }

        internal static SYSAppDetItm New()
        {
            
            SYSAppDetItm child = new SYSAppDetItm();
            
            return child;
        }

        internal static SYSAppDetItm NewChild()
        {
            
            SYSAppDetItm child = new SYSAppDetItm();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static SYSAppDetItm Get(SafeDataReader dr)
        {
            
            SYSAppDetItm child = new SYSAppDetItm();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SYSAppDetItm Get(int? appKey, string appObjSub, string appObjItm)
        {
            
            SYSAppDetItm child = new SYSAppDetItm();
            child.Fetch(new Criteria(appKey, appObjSub, appObjItm, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _appKey = null;
            internal string _appObjSub = string.Empty;
            internal string _appObjItm = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            //Added By Thida

            internal Criteria(int? AppKey, int? Option)
            {
                _appKey = AppKey;
                _option = Option;
            }

            internal Criteria(int? AppKey, string AppObjSub, string AppObjItm, int? Option)
            {
                _appKey = AppKey;
                _appObjSub = AppObjSub;
                _appObjItm = AppObjItm;
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
                cm.CommandText = "SYSAppDetItm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@AppKey", criteria._appKey);
                cm.Parameters.AddWithValue("@AppObjSub", criteria._appObjSub);
                cm.Parameters.AddWithValue("@AppObjItm", criteria._appObjItm);

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
            
            _appKey = dr.GetInt32("AppKey");
            _appObjSub = dr.GetString("AppObjSub");
            _appObjItm = dr.GetString("AppObjItm");
            _appProperty = dr.GetString("AppProperty");
            ValidationRules.CheckRules();
            return true;                  
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert( out int? appKey, out string appObjSub, out string appObjItm)
        {
            bool retValue = false;

            
            appKey = null;
            appObjSub = string.Empty;
            appObjItm = string.Empty;
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out appKey, out appObjSub, out appObjItm);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? appKey, out string appObjSub, out string appObjItm)
        {
            
            appKey = 0;
            appObjSub = string.Empty;
            appObjItm = string.Empty;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSAppDetItm_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);
                

                if (_appKey == null)
                    cm.Parameters.AddWithValue("@AppKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AppKey", _appKey);

                if (_appObjSub == null)
                    cm.Parameters.AddWithValue("@AppObjSub", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AppObjSub", _appObjSub);

                if (_appObjItm == null)
                    cm.Parameters.AddWithValue("@AppObjItm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AppObjItm", _appObjItm);

                if (_appProperty == null)
                    cm.Parameters.AddWithValue("@AppProperty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AppProperty", _appProperty);

                
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
                cm.CommandText = "SYSAppDetItm_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                

                if (_appKey == null)
                    cm.Parameters.AddWithValue("@AppKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AppKey", _appKey);

                if (_appObjSub == null)
                    cm.Parameters.AddWithValue("@AppObjSub", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AppObjSub", _appObjSub);

                if (_appObjItm == null)
                    cm.Parameters.AddWithValue("@AppObjItm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AppObjItm", _appObjItm);

                if (_appProperty == null)
                    cm.Parameters.AddWithValue("@AppProperty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AppProperty", _appProperty);

                

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
                cm.CommandText = "SYSAppDetItm_Delete";

                
                cm.Parameters.AddWithValue("@AppKey", criteria._appKey);
                cm.Parameters.AddWithValue("@AppObjSub", criteria._appObjSub);
                cm.Parameters.AddWithValue("@AppObjItm", criteria._appObjItm);

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