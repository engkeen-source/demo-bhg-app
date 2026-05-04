
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
    public class REFBrand : Csla.BusinessBase<REFBrand>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _brandKey = 0;
        internal string _brandID = string.Empty;
        internal string _brandDes = string.Empty;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public int? BrandKey
        {
            get
            {
                return _brandKey;
            }
        }

        public string BrandID
        {
            get
            {
                return _brandID;
            }
            set
            {
                _brandID = value;
                PropertyHasChanged("BrandID");
            }
        }

        public string BrandDes
        {
            get
            {
                return _brandDes;
            }
            set
            {
                _brandDes = value;
                PropertyHasChanged("BrandDes");
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
            return _brandKey.ToString();
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
            //// BrandID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "BrandID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BrandID", 50));
            ////
            //// BrandDes
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "BrandDes");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BrandDes", 255));
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

        internal REFBrand()
        { /* require use of factory method */
        }

        //internal static REFBrand New()
        //{
        //    
        //    REFBrand child = new REFBrand();
        //    msgID = string.Empty;
        //    return child;
        //}

        internal static REFBrand New()
        {
            
            REFBrand child = new REFBrand();
          
            return child;
        }

        //internal static REFBrand NewChild()
        //{
        //    
        //    REFBrand child = new REFBrand();
        //    child.ValidationRules.CheckRules();
        //    child.MarkAsChild();
        //    msgID = string.Empty;
        //    return child;
        //}

        internal static REFBrand NewChild()
        {
            
            REFBrand child = new REFBrand();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        //internal static REFBrand Get(SafeDataReader dr)
        //{
        //    
        //    REFBrand child = new REFBrand();
        //    child.MarkAsChild();
        //    child.Fetch(dr);
        //    return child;
        //}

        internal static REFBrand Get(SafeDataReader dr)
        {
           
                
                REFBrand child = new REFBrand();
             try
             {
                child.MarkAsChild();
                child.Fetch(dr);
                return child;
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //internal static REFBrand Get(int? brandKey)
        //{
        //    
        //    REFBrand child = new REFBrand();
        //    child.Fetch(new Criteria(brandKey, 1));
        //    return child;
        //}

        //internal static REFBrand Get(int? brandKey)
        //{
        //    REFBrand child = new REFBrand();
        //    try
        //    {
        //        child.Fetch(new Criteria(brandKey, 1));
        //        return child;
        //    }
        //    catch (TAException tex)
        //    {
        //        throw tex;
        //    }
        //    catch (Exception ex)
        //    {                
        //        throw ex;
        //    }
        //}

        public static REFBrand Get(int? brandKey)
        {
            REFBrand child = new REFBrand();
            try
            {
                child.Fetch(new Criteria(brandKey, 1));
                return child;
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _brandKey = null;
            public string _brandID = string.Empty;
            public string _brandIDFrom = "";
            public string _brandIDTo = "";
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? BrandKey)
            {
                _brandKey = BrandKey;
                _brandID = "";
            }

            internal Criteria(int? BrandKey, string BrandID)
            {
                _brandKey = BrandKey;
                _brandID = BrandID;
            }

            internal Criteria(int? BrandKey, int? Option)
            {
                _brandKey = BrandKey;
                _option = Option;
            }
            internal Criteria(int? BrandKey, string BrandID, int? Option)
            {
                _brandKey = BrandKey;
                _brandID = BrandID;
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
        //            cm.CommandText = "REFBrand_Get";

        //            cm.Parameters.AddWithValue("@Option", criteria._option);
        //            
        //            cm.Parameters.AddWithValue("@BrandKey", criteria._brandKey);

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
                cm.CommandText = "REFBrand_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@BrandKey", criteria._brandKey);
                cm.Parameters.AddWithValue("@BrandIDFrom", criteria._brandIDFrom);
                cm.Parameters.AddWithValue("@BrandIDTo", criteria._brandIDTo);
              

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
                    retValue = true;
                else
                    retValue = false;              


            }// Already close and dispose sql connection.
            
            return retValue;
        }

        //internal bool Fetch(SafeDataReader dr)
        //{
        //    bool retValue = false;
        //    
        //    try
        //    {
        //        _brandKey = dr.GetInt32("BrandKey");
        //        _brandID = dr.GetString("BrandID");
        //        _brandDes = dr.GetString("BrandDes");
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
            _brandID = dr.GetString("BrandID");
            _brandDes = dr.GetString("BrandDes");
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
        //                retValue = this.Insert(cn, out brandKey);
        //            }// End of SqlConnection

        //            // No errors - commit transaction
        //              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
        //        }// End of TransactionScope
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return retValue;
        //}

        internal bool Insert(out int? brandKey)
        {
            bool retValue = false;
            string msgID = MsgID.Common.AddFail;
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
                    retValue = this.Insert(cn, out brandKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;            
        }

        //internal bool Insert(SqlConnection cn, out int? brandKey)
        //{
        //    bool retValue = false;
        //    msgID = MsgID.Common.AddFail;
        //    brandKey = 0;
        //    try
        //    {
        //        // Using existing sql connection.
        //        using (SqlCommand cm = cn.CreateCommand())
        //        {
        //            // Get current user key
        //            _createUserKey = AppInfor.currentUserKey;
        //            _lastModifiedUserKey = AppInfor.currentUserKey;

        //            cm.CommandType = CommandType.StoredProcedure;
        //            cm.CommandText = "REFBrand_AddUpdate";

        //            cm.Parameters.AddWithValue("@Option", 0);
        //            

        //            cm.Parameters.AddWithValue("@NewBrandKey", brandKey);

        //            if (_brandKey == null)
        //                cm.Parameters.AddWithValue("@BrandKey", DBNull.Value);
        //            else
        //                cm.Parameters.AddWithValue("@BrandKey", _brandKey);

        //            if (_brandID == null)
        //                cm.Parameters.AddWithValue("@BrandID", DBNull.Value);
        //            else
        //                cm.Parameters.AddWithValue("@BrandID", _brandID);

        //            if (_brandDes == null)
        //                cm.Parameters.AddWithValue("@BrandDes", DBNull.Value);
        //            else
        //                cm.Parameters.AddWithValue("@BrandDes", _brandDes);

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
        //            cm.Parameters["@NewBrandKey"].Direction = ParameterDirection.Output;

        //            // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
        //            cm.Parameters.AddWithValue("@RetValue", 0);
        //            cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

        //            cm.ExecuteNonQuery();

        //            msgID = cm.Parameters["@MsgID"].Value.ToString();
        //            brandKey = (int)cm.Parameters["@NewBrandKey"].Value;

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

        internal bool Insert(SqlConnection cn, out int? brandKey)
        {
            bool retValue = false;
            brandKey = 0;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFBrand_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);
               
                cm.Parameters.AddWithValue("@NewBrandKey", brandKey);

                if (_brandKey == null)
                    cm.Parameters.AddWithValue("@BrandKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BrandKey", _brandKey);

                if (_brandID == null)
                    cm.Parameters.AddWithValue("@BrandID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BrandID", _brandID);

                if (_brandDes == null)
                    cm.Parameters.AddWithValue("@BrandDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BrandDes", _brandDes);

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

               
                cm.Parameters["@NewBrandKey"].Direction = ParameterDirection.Output;

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();
               
                brandKey = (int)cm.Parameters["@NewBrandKey"].Value;

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    retValue = true;
                }
                else
                    retValue=false;

            }// Already close and dispose sql connection.
            
            return retValue;
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

        //internal bool Update(SqlConnection cn)
        //{
        //    bool retValue = false;
        //    
        //    try
        //    {
        //        // Using existing sql connection.
        //        using (SqlCommand cm = cn.CreateCommand())
        //        {
        //            // Get current user key
        //            _lastModifiedUserKey = AppInfor.currentUserKey;

        //            cm.CommandType = CommandType.StoredProcedure;
        //            cm.CommandText = "REFBrand_AddUpdate";

        //            cm.Parameters.AddWithValue("@Option", 1);
        //            
        //            cm.Parameters.AddWithValue("@NewBrandKey", 0);

        //            if (_brandKey == null)
        //                cm.Parameters.AddWithValue("@BrandKey", DBNull.Value);
        //            else
        //                cm.Parameters.AddWithValue("@BrandKey", _brandKey);

        //            if (_brandID == null)
        //                cm.Parameters.AddWithValue("@BrandID", DBNull.Value);
        //            else
        //                cm.Parameters.AddWithValue("@BrandID", _brandID);

        //            if (_brandDes == null)
        //                cm.Parameters.AddWithValue("@BrandDes", DBNull.Value);
        //            else
        //                cm.Parameters.AddWithValue("@BrandDes", _brandDes);

        //            if (_createDate == null)
        //                cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
        //            else
        //                cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

        //            if (_createUserKey == null)
        //                cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
        //            else
        //                cm.Parameters.AddWithValue("@CreateUserKey", _createUserKey);

        //            if (_lastModifiedDate == null)
        //                cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
        //            else
        //                cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

        //            if (AppInfor.currentUserKey == null)
        //                cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
        //            else
        //                cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

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
        //            cm.Parameters["@NewBrandKey"].Direction = ParameterDirection.Output;

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

        internal bool Update(SqlConnection cn)
        {
            bool retValue = false;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFBrand_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
               
                cm.Parameters.AddWithValue("@NewBrandKey", 0);

                if (_brandKey == null)
                    cm.Parameters.AddWithValue("@BrandKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BrandKey", _brandKey);

                if (_brandID == null)
                    cm.Parameters.AddWithValue("@BrandID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BrandID", _brandID);

                if (_brandDes == null)
                    cm.Parameters.AddWithValue("@BrandDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BrandDes", _brandDes);

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
                {
                    retValue = true;
                }
                else
                    retValue = false;

            }// Already close and dispose sql connection.  
          
            return retValue;
        }
        #endregion //Data Access - Update

        #region Data Access - Delete
        
        //internal bool Delete(Criteria criteria)
        //{
        //    bool retValue = false;
        //    msgID = MsgID.Common.DeleteFail;
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

        //                // Call delete method.
        //                retValue = this.Delete(cn, criteria);
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

        //internal bool Delete(SqlConnection cn, Criteria criteria)
        //{
        //    bool retValue = false;
        //    msgID = MsgID.Common.DeleteFail;
        //    try
        //    {
        //        // Using existing sql connection.
        //        using (SqlCommand cm = cn.CreateCommand())
        //        {
        //            cm.CommandType = CommandType.StoredProcedure;
        //            cm.CommandText = "REFBrand_Delete";

        //            
        //            cm.Parameters.AddWithValue("@BrandKey", criteria._brandKey);

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

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFBrand_Delete";
             
                cm.Parameters.AddWithValue("@BrandKey", criteria._brandKey);
               

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();
              

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    retValue = true;
                }
                else
                    retValue = false;

            }// Already close and dispose sql connection.
            
            return retValue;
        }
        #endregion //Data Access - Delete

        #region Data Access - Validation

        //internal bool Validation(Criteria criteria, bool isNew)
        //{
        //    bool retValue = false;
        //    msgID = MsgID.Reference.REFBrand;
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

        //                // Call validation method.
        //                retValue = this.Validation(cn, criteria, isNew);
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
                    retValue = this.Validation(cn, criteria,isNew);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        //internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
        //{
        //    bool retValue = false;
        //    msgID = string.Empty;
        //    try
        //    {
        //        // Using existing sql connection.
        //        using (SqlCommand cm = cn.CreateCommand())
        //        {
        //            cm.CommandType = CommandType.StoredProcedure;
        //            cm.CommandText = "REFBrand_Validation";

        //            
        //            cm.Parameters.AddWithValue("@BrandKey", criteria._brandKey);
        //            cm.Parameters.AddWithValue("@BrandID", criteria._brandID);
        //            cm.Parameters.AddWithValue("@IsNew", isNew);

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
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return retValue;
        //}

        internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFBrand_Validation";
               
                cm.Parameters.AddWithValue("@BrandKey", criteria._brandKey);
                cm.Parameters.AddWithValue("@BrandID", criteria._brandID);
                cm.Parameters.AddWithValue("@IsNew", isNew);
             

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
            _brandKey = 0;
            _brandID = string.Empty;
            _brandDes = string.Empty;
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
