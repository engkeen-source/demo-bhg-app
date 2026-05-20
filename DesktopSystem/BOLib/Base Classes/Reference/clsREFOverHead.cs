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
    public class REFOverHead : Csla.BusinessBase<REFOverHead>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _overHeadKey = 0;
        internal string _overHeadID = string.Empty;
        internal string _overHeadDes = string.Empty;
        internal decimal? _overHeadCost = 0;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public int? OverHeadKey
        {
            get
            {
                return this._overHeadKey;
            }
        }

        public string OverHeadID
        {
            get
            {
                return this._overHeadID;
            }
            set
            {
                this._overHeadID = value;
                PropertyHasChanged("OverHeadID");
            }
        }

        public string OverHeadDes
        {
            get
            {
                return this._overHeadDes;
            }
            set
            {
                this._overHeadDes = value;
                PropertyHasChanged("OverHeadDes");
            }
        }

        public decimal? OverHeadCost
        {
            get
            {
                return this._overHeadCost;
            }
            set
            {
                this._overHeadCost = value;
                PropertyHasChanged("OverHeadCost");
            }
        }

        public DateTime? CreateDate
        {
            get
            {
                return this._createDate;
            }
            set
            {
                this._createDate = value;
                PropertyHasChanged("CreateDate");
            }
        }

        public int? CreateUserKey
        {
            get
            {
                return this._createUserKey;
            }
            set
            {
                this._createUserKey = value;
                PropertyHasChanged("CreateUserKey");
            }
        }

        public DateTime? LastModifiedDate
        {
            get
            {
                return this._lastModifiedDate;
            }
            set
            {
                this._lastModifiedDate = value;
                PropertyHasChanged("LastModifiedDate");
            }
        }

        public int? LastModifiedUserKey
        {
            get
            {
                return this._lastModifiedUserKey;
            }
            set
            {
                this._lastModifiedUserKey = value;
                PropertyHasChanged("LastModifiedUserKey");
            }
        }

        public string Custom1
        {
            get
            {
                return this._custom1;
            }
            set
            {
                this._custom1 = value;
                PropertyHasChanged("Custom1");
            }
        }

        public string Custom2
        {
            get
            {
                return this._custom2;
            }
            set
            {
                this._custom2 = value;
                PropertyHasChanged("Custom2");
            }
        }

        public string Custom3
        {
            get
            {
                return this._custom3;
            }
            set
            {
                this._custom3 = value;
                PropertyHasChanged("Custom3");
            }
        }

        protected override object GetIdValue()
        {
            return _overHeadKey.ToString();
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
            //// OverHeadID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "OverHeadID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OverHeadID", 50));
            ////
            //// OverHeadDes
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "OverHeadDes");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OverHeadDes", 255));
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

        internal REFOverHead()
        { /* require use of factory method */ }

        internal static REFOverHead New()
        {
           
            REFOverHead child = new REFOverHead();            
            return child;
        }

        internal static REFOverHead NewChild()
        {
            
            REFOverHead child = new REFOverHead();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
          
            return child;
        }

        internal static REFOverHead Get(SafeDataReader dr)
        {           
            REFOverHead child = new REFOverHead();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static REFOverHead Get(int? overHeadKey)
        {            
            REFOverHead child = new REFOverHead();
            child.Fetch(new Criteria(overHeadKey, 1));
            return child;
        }

        public static REFOverHead Get(SqlConnection cn, int? overHeadKey)
        {
            REFOverHead child = new REFOverHead();
            child.Fetch(cn, new Criteria(overHeadKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _overHeadKey = null;
            public int? _option = null;
            public string _overHeadID = string.Empty;

            internal Criteria()
            {
            }

            internal Criteria(int? OverHeadKey)
            {
                _overHeadKey = OverHeadKey;
            }

            internal Criteria(int? OverHeadKey, int? Option)
            {
                _overHeadKey = OverHeadKey;
                _option = Option;
            }

            internal Criteria(int? OverHeadKey, string OverHeadID)
            {
                _overHeadKey  = OverHeadKey;
                _overHeadID = OverHeadID;
            }

            internal Criteria(int? OverHeadKey, string OverHeadID, int? Option)
            {
                _overHeadKey = OverHeadKey;
                _overHeadID = OverHeadID;
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
                cm.CommandText = "REFOverHead_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                   
                cm.Parameters.AddWithValue("@OverHeadKey", criteria._overHeadKey);                   

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
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
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;

            }// Already close and dispose sql connection.
            
        }

        internal bool Fetch(SafeDataReader dr)
        {
            this._overHeadKey = dr.GetInt32("OverHeadKey");
            this._overHeadID = dr.GetString("OverHeadID");
            this._overHeadDes = dr.GetString("OverHeadDes");
            this._overHeadCost = dr.GetDecimal("OverHeadCost");
            this._createDate = dr.GetDateTime("CreateDate");
            this._createUserKey = dr.GetInt32("CreateUserKey");
            this._lastModifiedDate = dr.GetDateTime("LastModifiedDate");
            this._lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            this._custom1 = dr.GetString("Custom1");
            this._custom2 = dr.GetString("Custom2");
            this._custom3 = dr.GetString("Custom3");
            ValidationRules.CheckRules();
            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? overHeadKey)
        {
            bool retValue = false;           
            overHeadKey = null;
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out overHeadKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? overHeadKey)
        {
            overHeadKey = 0;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFOverHead_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);                   

                cm.Parameters.AddWithValue("@NewOverHeadKey", overHeadKey);

                if (_overHeadKey == null)
                    cm.Parameters.AddWithValue("@OverHeadKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OverHeadKey", _overHeadKey);

                if (_overHeadID == null)
                    cm.Parameters.AddWithValue("@OverHeadID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OverHeadID", _overHeadID);

                if (_overHeadDes == null)
                    cm.Parameters.AddWithValue("@OverHeadDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OverHeadDes", _overHeadDes);

                if (_overHeadCost == null)
                    cm.Parameters.AddWithValue("@OverHeadCost", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OverHeadCost", _overHeadCost);

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
               
                cm.Parameters["@NewOverHeadKey"].Direction = ParameterDirection.Output;

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();
               
                overHeadKey = (int)cm.Parameters["@NewOverHeadKey"].Value;
                // Check Return Value -- Changed By Richard
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
                // Get current user key
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFOverHead_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);                  
                cm.Parameters.AddWithValue("@NewOverHeadKey", 0);

                if (_overHeadKey == null)
                    cm.Parameters.AddWithValue("@OverHeadKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OverHeadKey", _overHeadKey);

                if (_overHeadID == null)
                    cm.Parameters.AddWithValue("@OverHeadID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OverHeadID", _overHeadID);

                if (_overHeadDes == null)
                    cm.Parameters.AddWithValue("@OverHeadDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OverHeadDes", _overHeadDes);

                if (_overHeadCost == null)
                    cm.Parameters.AddWithValue("@OverHeadCost", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OverHeadCost", _overHeadCost);

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
                
                cm.Parameters["@NewOverHeadKey"].Direction = ParameterDirection.Output;

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
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
                cm.CommandText = "REFOverHead_Delete";
        
                cm.Parameters.AddWithValue("@OverHeadKey", criteria._overHeadKey);                   

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
              
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

        internal bool Validation(SqlConnection cn, Criteria criteria,  bool isNew)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFOverHead_Validation";
             
                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@OverHeadKey", criteria._overHeadKey);
                cm.Parameters.AddWithValue("@OverHeadID", criteria._overHeadID);
              

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }
            
        }
        #endregion //Data Access - Validation

        private void Clear()
        {
            _overHeadKey = 0;
            _overHeadID = string.Empty;
            _overHeadDes = string.Empty;
            _overHeadCost = 0;
            _createDate = null;
            _createUserKey = null;
            _lastModifiedDate = null;
            _lastModifiedUserKey = null;
            _custom1 = string.Empty;
            _custom2 = string.Empty;
            _custom3 = string.Empty;


        }
    
    }
}
