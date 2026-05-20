

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
    public class MSTPriceListDetRatio : Csla.BusinessBase<MSTPriceListDetRatio>, System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _priceKey = 0;
        internal int? _cat1 = 0;
        internal int? _cat2 = 0;
        internal int? _cat3 = 0;
        internal int? _cat4 = 0;
        internal int? _cat5 = 0;
        internal int? _ratioType = 10;
        internal decimal? _percentage = 0;
        internal decimal? _ratio = 1;
        internal DateTime? _effStartDate = null;
        internal DateTime? _effEndDate = null;
        internal decimal? _effPercentage = 0;
        internal decimal? _effRatio = 1;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _error = string.Empty;        

        public int? PriceKey
        {
            get
            {
                return _priceKey;
            }
            set
            {
                _priceKey = value;
                PropertyHasChanged("PriceKey");
            }
        }

        public int? Cat1
        {
            get
            {
                return _cat1;
            }
            set
            {
                _cat1 = value;
                PropertyHasChanged("Cat1");
            }
        }

        public int? Cat2
        {
            get
            {
                return _cat2;
            }
            set
            {
                _cat2 = value;
                PropertyHasChanged("Cat2");
            }
        }

        public int? Cat3
        {
            get
            {
                return _cat3;
            }
            set
            {
                _cat3 = value;
                PropertyHasChanged("Cat3");
            }
        }

        public int? Cat4
        {
            get
            {
                return _cat4;
            }
            set
            {
                _cat4 = value;
                PropertyHasChanged("Cat4");
            }
        }

        public int? Cat5
        {
            get
            {
                return _cat5;
            }
            set
            {
                _cat5 = value;
                PropertyHasChanged("Cat5");
            }
        }

        public int? RatioType
        {
            get
            {
                return _ratioType;
            }
            set
            {
                _ratioType = value;
                PropertyHasChanged("RatioType");
            }
        }

        public decimal? Percentage
        {
            get
            {
                return _percentage;
            }
            set
            {
                _percentage = value;
                PropertyHasChanged("Percentage");
            }
        }

        public decimal? Ratio
        {
            get
            {
                return _ratio;
            }
            set
            {
                _ratio = value;
                PropertyHasChanged("Ratio");
            }
        }

        public DateTime? EffStartDate
        {
            get
            {
                return _effStartDate;
            }
            set
            {
                _effStartDate = value;
                PropertyHasChanged("EffStartDate");
            }
        }

        public DateTime? EffEndDate
        {
            get
            {
                return _effEndDate;
            }
            set
            {
                _effEndDate = value;
                PropertyHasChanged("EffEndDate");
            }
        }

        public decimal? EffPercentage
        {
            get
            {
                return _effPercentage;
            }
            set
            {
                _effPercentage = value;
                PropertyHasChanged("EffPercentage");
            }
        }

        public decimal? EffRatio
        {
            get
            {
                return _effRatio;
            }
            set
            {
                _effRatio = value;
                PropertyHasChanged("EffRatio");
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

        protected override object GetIdValue()
        {
            return _priceKey.ToString() + _cat1.ToString() + _cat2.ToString() + _cat3.ToString() + _cat4.ToString() + _cat5.ToString();
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

        public MSTPriceListDetRatio()
        { /* require use of factory method */ }

        internal static MSTPriceListDetRatio New()
        {
            
            MSTPriceListDetRatio child = new MSTPriceListDetRatio();
            
            return child;
        }

        internal static MSTPriceListDetRatio NewChild()
        {
            
            MSTPriceListDetRatio child = new MSTPriceListDetRatio();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static MSTPriceListDetRatio Get(SafeDataReader dr)
        {
           
            MSTPriceListDetRatio child = new MSTPriceListDetRatio();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTPriceListDetRatio Get(int? priceKey, int? cat1, int? cat2, int? cat3, int? cat4, int? cat5)
        {
           
            MSTPriceListDetRatio child = new MSTPriceListDetRatio();
            child.Fetch(new Criteria(priceKey, cat1, cat2, cat3, cat4, cat5, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _priceKey = null;
            public int? _cat1 = null;
            public int? _cat2 = null;
            public int? _cat3 = null;
            public int? _cat4 = null;
            public int? _cat5 = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? PriceKey)
            {
                _priceKey = PriceKey;
            }

            internal Criteria(int? PriceKey, int? Cat1, int? Cat2, int? Cat3, int? Cat4, int? Cat5)
            {
                _priceKey = PriceKey;
                _cat1 = Cat1;
                _cat2 = Cat2;
                _cat3 = Cat3;
                _cat4 = Cat4;
                _cat5 = Cat5;
            }

            internal Criteria(int? PriceKey, int? Cat1, int? Cat2, int? Cat3, int? Cat4, int? Cat5, int? Option)
            {
                _priceKey = PriceKey;
                _cat1 = Cat1;
                _cat2 = Cat2;
                _cat3 = Cat3;
                _cat4 = Cat4;
                _cat5 = Cat5;
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
                cm.CommandText = "MSTPriceListDetRatio_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@PriceKey", criteria._priceKey);
                cm.Parameters.AddWithValue("@Cat1", criteria._cat1);
                cm.Parameters.AddWithValue("@Cat2", criteria._cat2);
                cm.Parameters.AddWithValue("@Cat3", criteria._cat3);
                cm.Parameters.AddWithValue("@Cat4", criteria._cat4);
                cm.Parameters.AddWithValue("@Cat5", criteria._cat5);

                

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

              

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
           
            
            _priceKey = dr.GetInt32("PriceKey");
            _cat1 = dr.GetInt32("Cat1");
            _cat2 = dr.GetInt32("Cat2");
            _cat3 = dr.GetInt32("Cat3");
            _cat4 = dr.GetInt32("Cat4");
            _cat5 = dr.GetInt32("Cat5");
            _ratioType = dr.GetInt32("RatioType");
            _percentage = dr.GetDecimal("Percentage");
            _ratio = dr.GetDecimal("Ratio");

            if (GFunc.IsNE(dr.GetValue("EffStartDate")))
                _effStartDate = null;
            else
                _effStartDate = dr.GetDateTime("EffStartDate");

            if (GFunc.IsNE(dr.GetValue("EffStartDate")))
                _effStartDate = null;
            else
                _effStartDate = dr.GetDateTime("EffStartDate");

            _effPercentage = dr.GetDecimal("EffPercentage");
            _effRatio = dr.GetDecimal("EffRatio");

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
                cm.CommandText = "MSTPriceListDetRatio_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                
                

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                if (_priceKey == null)
                    cm.Parameters.AddWithValue("@PriceKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PriceKey", _priceKey);

                if (_cat1 == null)
                    cm.Parameters.AddWithValue("@Cat1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Cat1", _cat1);

                if (_cat2 == null)
                    cm.Parameters.AddWithValue("@Cat2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Cat2", _cat2);

                if (_cat3 == null)
                    cm.Parameters.AddWithValue("@Cat3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Cat3", _cat3);

                if (_cat4 == null)
                    cm.Parameters.AddWithValue("@Cat4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Cat4", _cat4);

                if (_cat5 == null)
                    cm.Parameters.AddWithValue("@Cat5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Cat5", _cat5);

                if (_ratioType == null)
                    cm.Parameters.AddWithValue("@RatioType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RatioType", _ratioType);

                if (_percentage == null)
                    cm.Parameters.AddWithValue("@Percentage", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Percentage", _percentage);

                if (_ratio == null)
                    cm.Parameters.AddWithValue("@Ratio", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio", _ratio);

                if (_effStartDate == null)
                    cm.Parameters.AddWithValue("@EffStartDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EffStartDate", _effStartDate.Value);

                if (_effEndDate == null)
                    cm.Parameters.AddWithValue("@EffEndDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EffEndDate", _effEndDate.Value);

                if (_effPercentage == null)
                    cm.Parameters.AddWithValue("@EffPercentage", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EffPercentage", _effPercentage);

                if (_effRatio == null)
                    cm.Parameters.AddWithValue("@EffRatio", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EffRatio", _effRatio);

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
            bool retValue = false;
            
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTPriceListDetRatio_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);

                
                

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewPriceKey", 0);
                cm.Parameters.AddWithValue("@NewCat1", 0);
                cm.Parameters.AddWithValue("@NewCat2", 0);
                cm.Parameters.AddWithValue("@NewCat3", 0);
                cm.Parameters.AddWithValue("@NewCat4", 0);
                cm.Parameters.AddWithValue("@NewCat5", 0);

                if (_priceKey == null)
                    cm.Parameters.AddWithValue("@PriceKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PriceKey", _priceKey);

                if (_cat1 == null)
                    cm.Parameters.AddWithValue("@Cat1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Cat1", _cat1);

                if (_cat2 == null)
                    cm.Parameters.AddWithValue("@Cat2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Cat2", _cat2);

                if (_cat3 == null)
                    cm.Parameters.AddWithValue("@Cat3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Cat3", _cat3);

                if (_cat4 == null)
                    cm.Parameters.AddWithValue("@Cat4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Cat4", _cat4);

                if (_cat5 == null)
                    cm.Parameters.AddWithValue("@Cat5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Cat5", _cat5);

                if (_ratioType == null)
                    cm.Parameters.AddWithValue("@RatioType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RatioType", _ratioType);

                if (_percentage == null)
                    cm.Parameters.AddWithValue("@Percentage", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Percentage", _percentage);

                if (_ratio == null)
                    cm.Parameters.AddWithValue("@Ratio", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio", _ratio);

                if (_effStartDate == null)
                    cm.Parameters.AddWithValue("@EffStartDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EffStartDate", _effStartDate.Value);

                if (_effEndDate == null)
                    cm.Parameters.AddWithValue("@EffEndDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EffEndDate", _effEndDate.Value);

                if (_effPercentage == null)
                    cm.Parameters.AddWithValue("@EffPercentage", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EffPercentage", _effPercentage);

                if (_effRatio == null)
                    cm.Parameters.AddWithValue("@EffRatio", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EffRatio", _effRatio);

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

                cm.Parameters["@NewPriceKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewCat1"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewCat2"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewCat3"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewCat4"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewCat5"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();


                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.
            
            return retValue;
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
                cm.CommandText = "MSTPriceListDetRatio_Delete";

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@PriceKey", criteria._priceKey);

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


