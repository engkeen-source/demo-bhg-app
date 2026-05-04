

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Collections;
using System.Transactions;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class MSTItmDetBOM : Csla.BusinessBase<MSTItmDetBOM>,System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _itmKey = null;
        internal int? _bOMItmKey = null;
        internal int? _bOMLineType = null;
        internal int? _bOMItmType = null;
        internal int? _bOMUOMKey = null;
        internal decimal? _bOMQty = 0;
        internal decimal? _bOMLabourCost = 0;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _error= string.Empty;

        public int? ItmKey
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
        public int? BOMItmKey
        {
            get
            {
                return _bOMItmKey;
            }
            set
            {
                _bOMItmKey = value;
                PropertyHasChanged("BOMItmKey");
            }
        }

        public int? BOMLineType
        {
            get
            {
                return _bOMLineType;
            }
            set
            {
                _bOMLineType = value;
                PropertyHasChanged("BOMLineType");
            }
        }

        public int? BOMItmType
        {
            get
            {
                return _bOMItmType;
            }
            set
            {
                _bOMItmType = value;
                PropertyHasChanged("BOMItmType");
            }
        }

        public int? BOMUOMKey
        {
            get
            {
                return _bOMUOMKey;
            }
            set
            {
                _bOMUOMKey = value;
                PropertyHasChanged("BOMUOMKey");
            }
        }

        public decimal? BOMQty
        {
            get
            {
                return _bOMQty;
            }
            set
            {
                _bOMQty = value;
                PropertyHasChanged("BOMQty");
            }
        }

        public decimal? BOMLabourCost
        {
            get
            {
                return _bOMLabourCost;
            }
            set
            {
                _bOMLabourCost = value;
                PropertyHasChanged("BOMLabourCost");
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

        protected override object GetIdValue()
        {
            return _itmKey.ToString() + _bOMItmKey.ToString();
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

        public MSTItmDetBOM()
        { /* require use of factory method */ }

        internal static MSTItmDetBOM New()
        {           
            MSTItmDetBOM child = new MSTItmDetBOM();         
            return child;
        }

        public static MSTItmDetBOM NewChild()
        {          
            MSTItmDetBOM child = new MSTItmDetBOM();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();           
            return child;
        }

        public static MSTItmDetBOM Get(SafeDataReader dr)
        {           
            MSTItmDetBOM child = new MSTItmDetBOM();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static MSTItmDetBOM Get(int? itmKey)
        {
          
            MSTItmDetBOM child = new MSTItmDetBOM();
            child.Fetch(new Criteria(itmKey,  1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _itmKey = null;
            public int? _bOMItmKey = null;
            public int? _option = null;
            public int? _bOMLineType = null;


            internal Criteria()
            {
            }

            internal Criteria(int? ItmKey, int? option)
            {
                _itmKey = ItmKey;
                _option = option;
            }

            internal Criteria(int? ItmKey, BOLib.GEnum.BOMLineType BOMLineType, int? Option)
            {
                _itmKey = ItmKey;
                _bOMLineType =(int?) BOMLineType;
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
                cm.CommandText = "MSTItmDetBOM_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
            
                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
                cm.Parameters.AddWithValue("@BOMLineType", criteria._bOMLineType);                  

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
                // Check Return Value -- Changed By Richard
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
            _itmKey = dr.GetInt32("ItmKey");
            _bOMItmKey = dr.GetInt32("BOMItmKey");
            _bOMLineType = dr.GetInt32("BOMLineType");
            _bOMItmType = dr.GetInt32("BOMItmType");
            _bOMUOMKey = dr.GetInt32("BOMUOMKey");
            _bOMQty = dr.GetDecimal("BOMQty");
            _bOMLabourCost = dr.GetDecimal("BOMLabourCost");
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
                cm.CommandText = "MSTItmDetBOM_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);                   

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;                    

                if (_itmKey == null)
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_bOMItmKey == null)
                    cm.Parameters.AddWithValue("@BOMItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BOMItmKey", _bOMItmKey);

                if (_bOMLineType == null)
                    cm.Parameters.AddWithValue("@BOMLineType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BOMLineType", _bOMLineType);

                if (_bOMItmType == null)
                    cm.Parameters.AddWithValue("@BOMItmType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BOMItmType", _bOMItmType);

                if (_bOMUOMKey == null)
                    cm.Parameters.AddWithValue("@BOMUOMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BOMUOMKey", _bOMUOMKey);

                if (_bOMQty == null)
                    cm.Parameters.AddWithValue("@BOMQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BOMQty", _bOMQty);

                if (_bOMLabourCost == null)
                    cm.Parameters.AddWithValue("@BOMLabourCost", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BOMLabourCost", _bOMLabourCost);

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
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmDetBOM_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);                  

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewItmKey", 0);
                cm.Parameters.AddWithValue("@NewBOMItmKey", 0);

                if (_itmKey == null)
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_bOMItmKey == null)
                    cm.Parameters.AddWithValue("@BOMItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BOMItmKey", _bOMItmKey);

                if (_bOMLineType == null)
                    cm.Parameters.AddWithValue("@BOMLineType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BOMLineType", _bOMLineType);

                if (_bOMItmType == null)
                    cm.Parameters.AddWithValue("@BOMItmType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BOMItmType", _bOMItmType);

                if (_bOMUOMKey == null)
                    cm.Parameters.AddWithValue("@BOMUOMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BOMUOMKey", _bOMUOMKey);

                if (_bOMQty == null)
                    cm.Parameters.AddWithValue("@BOMQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BOMQty", _bOMQty);

                if (_bOMLabourCost == null)
                    cm.Parameters.AddWithValue("@BOMLabourCost", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BOMLabourCost", _bOMLabourCost);

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

                cm.Parameters["@NewItmKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewBOMItmKey"].Direction = ParameterDirection.Output;

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
            try
            {
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
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmDetBOM_Delete";

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
                cm.Parameters.AddWithValue("@BOMLineType", criteria._bOMLineType);
                //cm.Parameters.AddWithValue("@BOMItmKey", criteria._bOMItmKey);

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

        internal bool Validation(Criteria criteria, bool? isNew)
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

        internal bool Validation(SqlConnection cn, Criteria criteria,  bool? isNew)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmDetBOM_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);
              
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
                cm.Parameters.AddWithValue("@BOMItmKey", criteria._bOMItmKey);

                cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }            
        }
        #endregion //Data Access - Validation
    }
}


