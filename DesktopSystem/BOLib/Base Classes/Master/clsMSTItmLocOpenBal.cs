
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
    public class MSTItmLocOpenBal : Csla.BusinessBase<MSTItmLocOpenBal>
    {
        #region Business Properties and Methods

        //declare members
        internal int _itmKey = 0;
        internal int _batchKey = 0;
        internal int _locKey = 0;
        internal DateTime _datePurchase;
        internal decimal _qty = 0;        
        internal DateTime _BatchExpDate;
        internal DateTime _BatchMfgDate;
        internal decimal _BatchCost = 0;
        internal string _BatchID = string.Empty;


        public int ItmKey
        {
            get
            {
                return _itmKey;
            }            
            set
            {
                _itmKey = value;
                PropertyHasChanged("ItmKey");
            }
        }

        public int BatchKey
        {
            get
            {
                return _batchKey;
            }            
            set
            {
                _batchKey = value;
                PropertyHasChanged("BatchKey");
            }
        }

        public int LocKey
        {
            get
            {
                return _locKey;
            }            
            set
            {
                _locKey = value;
                PropertyHasChanged("LocKey");
            }
        }

        public DateTime DatePurchase
        {
            get
            {
                return _datePurchase;
            }            
            set
            {
                _datePurchase = value;
                PropertyHasChanged("DatePurchase");
            }
        }

        public decimal Qty
        {
            get
            {
                return _qty;
            }            
            set
            {
                _qty = value;
                PropertyHasChanged("Qty");
            }
        }
        public DateTime BatchExpDate
        {
            get
            {
                return _BatchExpDate;
            }
            set
            {
                _BatchExpDate = value;
                PropertyHasChanged("BatchExpDate");
            }
        }
        public DateTime BatchMfgDate
        {
            get
            {
                return _BatchMfgDate;
            }
            set
            {
                _BatchMfgDate = value;
                PropertyHasChanged("BatchMfgDate");
            }
        }
        public string BatchID 
        {
            get
            {
                return _BatchID;
            }
            set
            {
                _BatchID = value;
                PropertyHasChanged("BatchID");
            }
        }
        public decimal BatchCost
        {
            get
            {
                return _BatchCost;
            }
            set
            {
                _BatchCost = value;
                PropertyHasChanged("BatchCost");
            }
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
            //// MSTItmLocOpenBal
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "MST_ItmLocOpenBal");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MST_ItmLocOpenBalID", 50));
            ////
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal MSTItmLocOpenBal()
        { /* require use of factory method */ }

        internal static MSTItmLocOpenBal New()
        {
            MSTItmLocOpenBal child = new MSTItmLocOpenBal();         
            return child;
        }

        internal static MSTItmLocOpenBal NewChild()
        {
            MSTItmLocOpenBal child = new MSTItmLocOpenBal();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static MSTItmLocOpenBal Get(SafeDataReader dr)
        {
            MSTItmLocOpenBal child = new MSTItmLocOpenBal();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTItmLocOpenBal Get(int itmKey,int batchKey,int locKey,DateTime datePurchase)
        {
            MSTItmLocOpenBal child = new MSTItmLocOpenBal();
            child.Fetch(new Criteria(itmKey,batchKey,locKey,datePurchase, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
        public int _itmKey = 0;
        public int _batchKey = 0;
        public int _locKey = 0;
        public DateTime _datePurchase;
        public decimal _qty = 0;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int itmKey,int batchKey,int locKey,DateTime datePurchase)
            {
                _itmKey = itmKey;
                _batchKey = batchKey;
                _locKey = locKey;
                _datePurchase = datePurchase;
            }

            internal Criteria(int itmKey,int batchKey,int locKey,DateTime datePurchase, int? Option)
            {
                _itmKey = itmKey;
                _batchKey = batchKey;
                _locKey = locKey;
                _datePurchase = datePurchase;
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
                retValue=this.Fetch(cn, criteria);                           
            }
                          
            
            return retValue;
        }

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            bool retValue=false;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MST_ItmLocOpenBal_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
                cm.Parameters.AddWithValue("@BatchKey", criteria._batchKey);
                cm.Parameters.AddWithValue("@LocKey", criteria._locKey);
                cm.Parameters.AddWithValue("@DatePurchase", criteria._datePurchase);


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

                }// Already close and dispose data reader.



                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.                       

            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {            
            _itmKey =  dr.GetInt32("ItmKey");
            _batchKey =  dr.GetInt32("BatchKey");
            _locKey =  dr.GetInt32("LocKey");
            _datePurchase =  dr.GetDateTime("DatePurchase");
            _qty =  dr.GetDecimal("Qty");
            _BatchID = dr.GetString("BatchID");
            _BatchExpDate = dr.GetDateTime("BatchExpDate");
            _BatchMfgDate = dr.GetDateTime("BatchMfgDate");
            _BatchCost = dr.GetDecimal("Qty");
            ValidationRules.CheckRules();
         
            return true;            
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(int itmKey,int batchKey,int locKey,DateTime datePurchase)
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
                    retValue= this.Insert(cn,itmKey,batchKey,locKey,datePurchase);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope               
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn,int itmKey,int batchKey,int locKey,DateTime datePurchase)
        {
            bool retValue = false;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                //_createUserKey = AppInfor.currentUserKey;
                //_lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MST_ItmLocOpenBal_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@NewItmKey", itmKey);
                cm.Parameters.AddWithValue("@NewBatchKey", batchKey);
                cm.Parameters.AddWithValue("@NewLocKey", locKey);
                cm.Parameters.AddWithValue("@NewDatePurchase", datePurchase);

                if (_itmKey == null)
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_batchKey == null)
                    cm.Parameters.AddWithValue("@BatchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchKey", _batchKey);

                if (_locKey == null)
                    cm.Parameters.AddWithValue("@LocKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LocKey", _locKey);

                if (_datePurchase == null)
                    cm.Parameters.AddWithValue("@DatePurchase", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DatePurchase", _datePurchase);

                if (_qty == null)
                    cm.Parameters.AddWithValue("@Qty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Qty", _qty);


                cm.Parameters["@NewItmKey"].Direction = ParameterDirection.InputOutput;
                cm.Parameters["@NewBatchKey"].Direction = ParameterDirection.InputOutput;
                cm.Parameters["@NewLocKey"].Direction = ParameterDirection.InputOutput;
                cm.Parameters["@NewDatePurchase"].Direction = ParameterDirection.InputOutput;

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();
                
                itmKey = (int)cm.Parameters["@NewItmKey"].Value;
                batchKey = (int)cm.Parameters["@NewBatchKey"].Value;
                locKey = (int)cm.Parameters["@NewLocKey"].Value;
                datePurchase = (DateTime)cm.Parameters["@NewDatePurchase"].Value;

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.            

            return retValue;
        }

        #endregion //Data Access - Insert

        #region Data Access - Update

        internal bool Update()
        {
            bool retValue=false;
            
                // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call update method.
                    retValue=this.Update(cn);
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
                // Get current user key
                //_lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MST_ItmLocOpenBal_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewItmKey", 0);
                cm.Parameters.AddWithValue("@NewBatchKey", 0);
                cm.Parameters.AddWithValue("@NewLocKey", 0);
                cm.Parameters.AddWithValue("@NewDatePurchase", 0);

                if (_itmKey == null)
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_batchKey == null)
                    cm.Parameters.AddWithValue("@BatchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchKey", _batchKey);

                if (_locKey == null)
                    cm.Parameters.AddWithValue("@LocKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LocKey", _locKey);

                if (_datePurchase == null)
                    cm.Parameters.AddWithValue("@DatePurchase", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DatePurchase", _datePurchase);

                if (_qty == null)
                    cm.Parameters.AddWithValue("@Qty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Qty", _qty);

                cm.Parameters["@NewItmKey"].Direction = ParameterDirection.InputOutput;
                cm.Parameters["@NewBatchKey"].Direction = ParameterDirection.InputOutput;
                cm.Parameters["@NewLocKey"].Direction = ParameterDirection.InputOutput;
                cm.Parameters["@NewDatePurchase"].Direction = ParameterDirection.InputOutput;

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
                   retValue= this.Delete(cn, criteria);
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
                cm.CommandText = "MST_ItmLocOpenBal_Delete";

                cm.Parameters.AddWithValue("@ItmKey", 0);
                cm.Parameters.AddWithValue("@BatchKey", 0);
                cm.Parameters.AddWithValue("@LocKey", 0);
                cm.Parameters.AddWithValue("@DatePurchase", 0);

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

        #region Data Access - Validation

        internal bool Validation(Criteria criteria,bool isNew)
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
                    retValue= this.Validation(cn, criteria, isNew);
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
                cm.CommandText = "MST_ItmLocOpenBal_Validation";

               
                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@ItmKey", 0);
                cm.Parameters.AddWithValue("@BatchKey", 0);
                cm.Parameters.AddWithValue("@LocKey", 0);
                cm.Parameters.AddWithValue("@DatePurchase", 0);

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
            _itmKey = 0;
            _batchKey = 0;
            _locKey = 0;            
            _qty = 0;            
            _BatchCost = 0;
            _BatchID = string.Empty;

        }
    
    }
}
