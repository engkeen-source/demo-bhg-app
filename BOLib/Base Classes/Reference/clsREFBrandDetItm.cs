
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
    public class REFBrandDetItm : Csla.BusinessBase<REFBrandDetItm>,System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _brandKey = 0;
        internal string _model = string.Empty;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _error = string.Empty;        

        public int? BrandKey
        {
            get
            {
                return _brandKey;
            }
        }
        
        public string Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                PropertyHasChanged("Model");                
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
                if (_error!=value)
                    _error = value;
            }  
        }
        public DateTime? CreateDate
        {
            get
            {
                return _createDate;
            }
        }

        public int? CreateUserKey
        {
            get
            {
                return _createUserKey;
            }
        }

        public DateTime? LastModifiedDate
        {
            get
            {
                return _lastModifiedDate;
            }
        }

        public int? LastModifiedUserKey
        {
            get
            {
                return _lastModifiedUserKey;
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
            return _brandKey.ToString() + _model.ToString();
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
            //// Model
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "Model");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Model", 50));
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

        public REFBrandDetItm()
        { /* require use of factory method */ }

        internal static REFBrandDetItm New()
        {
            
            REFBrandDetItm child = new REFBrandDetItm();
            
            return child;
        }

        internal static REFBrandDetItm NewChild()
        {
            
            REFBrandDetItm child = new REFBrandDetItm();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
           
            return child;
        }

        internal static REFBrandDetItm Get(SafeDataReader dr)
        {
            REFBrandDetItm child = new REFBrandDetItm();
           
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
            
        }

        internal static REFBrandDetItm Get(int? brandKey, string model)
        {
            REFBrandDetItm child = new REFBrandDetItm();
            
            child.Fetch(new Criteria(brandKey, model, 2));
            return child;
            
        }

        #endregion //Factory Method

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _brandKey = null;
            internal string _model = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? BrandKey)
            {
                _brandKey = BrandKey;
            }

            internal Criteria(int? BrandKey, string Model)
            {
                _brandKey = BrandKey;
                _model = Model;
            }

            internal Criteria(int? BrandKey, string Model, int? Option)
            {
                _brandKey = BrandKey;
                _model = Model;
                _option = Option;
            }
        }

        #endregion //Criteria

        #region Data Access - Fetch

        //internal bool Fetch(Criteria criteria)
        //{
        //    bool retValue = false;
        //    
        //    try
        //    {
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            // Create new sql connection for this method. 
        //            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
        //            {
        //                // Open sql connection. 
        //                cn.Open();
        //                retValue = this.Fetch(cn, criteria);
        //            }
        //            // No errors - commit transaction
        //              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
        //        }// Already close and dispose sql connection.
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return retValue;
        //}

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

        //internal bool Fetch(SqlConnection cn, Criteria criteria)
        //{
        //    bool retValue = false;
        //    
        //    try
        //    {
        //        // Using existing sql connection.
        //        using (SqlCommand cm = cn.CreateCommand())
        //        {
        //            cm.CommandType = CommandType.StoredProcedure;
        //            cm.CommandText = "REFBrandDetItm_Get";

        //            cm.Parameters.AddWithValue("@Option", criteria._option);
        //            
        //            cm.Parameters.AddWithValue("@BrandKey", criteria._brandKey);
        //            cm.Parameters.AddWithValue("@Model", criteria._model);

        //            

        //            // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
        //            cm.Parameters.AddWithValue("@RetValue", 0);
        //            cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

        //            // Using data reader as record set.
        //            using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
        //            {
        //                // If data reader can read, continue...
        //                while (dr.Read())
        //                {
        //                    retValue = this.Fetch(dr);
        //                }
        //            }	// Already close and dispose data reader.

        //            if (cm.Parameters["@MsgID"].Value == null)
        //                msgID = string.Empty;
        //            else
        //                msgID = cm.Parameters["@MsgID"].Value.ToString();

        //            // Check Return Value -- Changed By Richard
        //            if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
        //                retValue = true;                                        
        //        }// Already close and dispose sql connection.
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return retValue;
        //}

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFBrandDetItm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
               
                cm.Parameters.AddWithValue("@BrandKey", criteria._brandKey);
                cm.Parameters.AddWithValue("@Model", criteria._model);
            

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
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
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.           
        }

        //internal bool Fetch(SafeDataReader dr)
        //{
        //    bool retValue = false;
        //    
        //    try
        //    {
        //        _brandKey = dr.GetInt32("BrandKey");
        //        _model = dr.GetString("Model");
        //        _createDate = dr.GetDateTime("CreateDate");
        //        _createUserKey = dr.GetInt32("CreateUserKey");
        //        _lastModifiedDate = dr.GetDateTime("LastModifiedDate");
        //        _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
        //        _custom1 = dr.GetString("Custom1");
        //        _custom2 = dr.GetString("Custom2");
        //        _custom3 = dr.GetString("Custom3");
        //        ValidationRules.CheckRules();
        //        retValue = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        retValue = false;
        //    }
        //    return retValue;
        //}

        internal bool Fetch(SafeDataReader dr)
        {
            _brandKey = dr.GetInt32("BrandKey");
            _model = dr.GetString("Model");
            _createDate = dr.GetDateTime("CreateDate");
            _createUserKey = dr.GetInt32("CreateUserKey");
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

        //internal bool Insert( out int? brandKey)
        //{
        //    bool retValue = false;
        //    msgID = MsgID.Common.AddFail;
        //    brandKey = null;
        //    try
        //    {
        //        // Create Transaction Scope
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            // Create SqlConnection
        //            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
        //            {
        //                // Open Connection
        //                cn.Open();

        //                // Call insert method.
        //                retValue = this.Insert(cn);
        //                //retValue = this.Insert(cn, out brandKey);
        //            }// End of SqlConnection

        //            // No errors - commit transaction
        //              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
        //        }// End of TransactionScope
        //    }
        //    catch (Exception ex)
        //    {
        //        retValue = false;
        //    }
        //    return retValue;
        //}

        internal bool Insert(out int? brandKey)
        {
            bool retValue = false;
            brandKey = null;
           
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

        //internal bool Insert(SqlConnection cn)
        //{
        //    bool retValue = false;
        //    msgID = MsgID.Common.AddFail;
        //    try
        //    {
        //        // Using existing sql connection.
        //        using (SqlCommand cm = cn.CreateCommand())
        //        {
        //            // Get current user key
        //            _createUserKey = AppInfor.currentUserKey;
        //            _lastModifiedUserKey = AppInfor.currentUserKey;

        //            cm.CommandType = CommandType.StoredProcedure;
        //            cm.CommandText = "REFBrandDetItm_AddUpdate";

        //            cm.Parameters.AddWithValue("@Option", 0);
        //            

        //            if (_brandKey == null)
        //                cm.Parameters.AddWithValue("@BrandKey", DBNull.Value);
        //            else
        //                cm.Parameters.AddWithValue("@BrandKey", _brandKey);

        //            if (_model == null)
        //                cm.Parameters.AddWithValue("@Model", DBNull.Value);
        //            else
        //                cm.Parameters.AddWithValue("@Model", _model);

        //            if (_createDate == null)
        //                cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
        //            else
        //                cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

        //            if (AppInfor.currentUserKey == null)
        //                cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
        //            else
        //                 cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);

        //            if (_lastModifiedDate == null)
        //                cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
        //            else
        //                cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

        //            if (_lastModifiedUserKey == null)
        //                cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
        //            else
        //                cm.Parameters.AddWithValue("@LastModifiedUserKey", _lastModifiedUserKey);

        //            if (_custom1 == null)
        //                cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
        //            else
        //                cm.Parameters.AddWithValue("@Custom1", _custom1);

        //            if (_custom2 == null)
        //                cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
        //            else
        //                cm.Parameters.AddWithValue("@Custom2", _custom2);

        //            if (_custom3 == null)
        //                cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
        //            else
        //                cm.Parameters.AddWithValue("@Custom3", _custom3);

        //            

        //            // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
        //            cm.Parameters.AddWithValue("@RetValue", 0);
        //            cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

        //            cm.ExecuteNonQuery();

        //            if (cm.Parameters["@MsgID"].Value == null)
        //                msgID = string.Empty;
        //            else
        //                msgID = cm.Parameters["@MsgID"].Value.ToString();


        //            // Check Return Value -- Changed By Richard
        //            if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
        //                retValue = true;                                        

        //        }// Already close and dispose sql connection.
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return retValue;
        //}

        internal bool Insert(SqlConnection cn)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFBrandDetItm_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);                   

                if (_brandKey == null)
                    cm.Parameters.AddWithValue("@BrandKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BrandKey", _brandKey);

                if (_model == null)
                    cm.Parameters.AddWithValue("@Model", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Model", _model);

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
                cm.CommandText = "REFBrandDetItm_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);                   
                cm.Parameters.AddWithValue("@NewBrandKey", 0);

                if (_brandKey == null)
                    cm.Parameters.AddWithValue("@BrandKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BrandKey", _brandKey);

                if (_model == null)
                    cm.Parameters.AddWithValue("@Model", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Model", _model);

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
              
                cm.Parameters["@NewBrandKey"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "REFBrandDetItm_Delete";
           
                cm.Parameters.AddWithValue("@BrandKey", criteria._brandKey);           

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

        internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
        {            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFBrandDetItm_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);                 
                cm.Parameters.AddWithValue("@BrandKey", criteria._brandKey);
                cm.Parameters.AddWithValue("@Model", criteria._model);
             

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

            

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return  true;
                else 
                    return false;
            }
            
        }
        #endregion //Data Access - Validation
    }
}
