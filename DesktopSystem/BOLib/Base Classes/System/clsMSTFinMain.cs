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
    public class MSTFinMain : Csla.BusinessBase<MSTFinMain>
    {
        #region Business Properties and Methods

        //declare members
        protected int _repKey = 0;
        protected string _repName = string.Empty;
        protected int _repType = 0;
        protected string _remarks = string.Empty;
        protected string _paperSize = string.Empty;
        protected decimal? _marginTop = 0.0M;
        protected decimal? _marginBottom = 0.0M;
        protected decimal? _marginLeft = 0.0M;
        protected decimal? _marginRight = 0.0M;
        protected bool _hidden = false;
        protected bool _buildIn = false;
        protected DateTime? _createDate = null;
        protected int? _createUserKey = 0;
        protected DateTime? _lastModifiedDate = null;
        protected int? _lastModifiedUserKey = 0;
        protected string _custom1 = string.Empty;
        protected string _custom2 = string.Empty;
        protected string _custom3 = string.Empty;
        protected bool _isNew = false;
        protected bool _isReadOnly = false;
        protected int? _GUID = null;
        protected bool _isDirty = false;

        public int RepKey
        {
            get
            {
                return _repKey;
            }            
            set
            {
                _repKey = value;
                PropertyHasChanged("RepKey");
            }
        }

        public string RepName
        {
            get
            {
                return _repName;
            }            
            set
            {
                _repName = value;
                PropertyHasChanged("RepName");
            }
        }

        public int RepType
        {
            get
            {
                return _repType;
            }            
            set
            {
                _repType = value;
                PropertyHasChanged("RepType");
            }
        }

        public string Remarks
        {
            get
            {
                return _remarks;
            }            
            set
            {
                _remarks = value;
                PropertyHasChanged("Remarks");
            }
        }

        public string PaperSize
        {
            get
            {
                return _paperSize;
            }            
            set
            {
                _paperSize = value;
                PropertyHasChanged("PaperSize");
            }
        }

        public decimal? MarginTop
        {
            get
            {
                return _marginTop;
            }            
            set
            {
                _marginTop = value;
                PropertyHasChanged("MarginTop");
            }
        }

        public decimal? MarginBottom
        {
            get
            {
                return _marginBottom;
            }            
            set
            {
                _marginBottom = value;
                PropertyHasChanged("MarginBottom");
            }
        }

        public decimal? MarginLeft
        {
            get
            {
                return _marginLeft;
            }            
            set
            {
                _marginLeft = value;
                PropertyHasChanged("MarginLeft");
            }
        }

        public decimal? MarginRight
        {
            get
            {
                return _marginRight;
            }            
            set
            {
                _marginRight = value;
                PropertyHasChanged("MarginRight");
            }
        }

        public bool Hidden
        {
            get
            {
                return _hidden;
            }            
            set
            {
                _hidden = value;
                PropertyHasChanged("Hidden");
            }
        }

        public bool BuildIn
        {
            get
            {
                return _buildIn;
            }            
            set
            {
                _buildIn = value;
                PropertyHasChanged("BuildIn");
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


        public bool IsNew
        {
            get
            {
                return this._isNew;
            }
            set { this._isNew = value; }
        }

        public bool IsReadOnly
        {
            get
            {
                return this._isReadOnly;
            }
            set { this._isReadOnly = value; }
        }

        public int? GUID
        {
            get
            {
                return this._GUID;
            }
            set
            {
                this._GUID = value;
                PropertyHasChanged("GUID");
            }
        }

        public bool IsDirty
        {
            get
            {
                return this._isDirty;
            }
            set { this._isDirty = value; }
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
            //// MSTFinMain
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "MSTFinMain");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MSTFinMainID", 50));
            ////
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods
        public static MSTFinMain New()
        {
            MSTFinMain child = new MSTFinMain();         
            return child;
        }

        internal static MSTFinMain NewChild()
        {
            MSTFinMain child = new MSTFinMain();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static MSTFinMain Get(SafeDataReader dr)
        {
            MSTFinMain child = new MSTFinMain();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static MSTFinMain Get(int repKey)
        {
            MSTFinMain child = new MSTFinMain();
            child.Fetch(new Criteria(repKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
        public int? _repKey = 0;
        public string _repName = string.Empty;
        public int _repType = 0;
        public string _remarks = string.Empty;
        public string _paperSize = string.Empty;
        public decimal? _marginTop = 0.0M;
        public decimal? _marginBottom = 0.0M;
        public decimal? _marginLeft = 0.0M;
        public decimal? _marginRight = 0.0M;
        public bool _hidden = false;
        public bool _buildIn = false;
        public string _custom1 = string.Empty;
        public string _custom2 = string.Empty;
        public string _custom3 = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int repKey)
            {
                _repKey = repKey;
                _option = 1;
            }

            internal Criteria(int? repKey, int? Option)
            {
                _repKey = repKey;
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
                cm.CommandText = "MSTFinMain_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@RepKey", criteria._repKey);


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
            _repKey =  dr.GetInt32("RepKey");
            _repName =  dr.GetString("RepName");
            _repType =  dr.GetInt32("RepType");
            _remarks =  dr.GetString("Remarks");
            _paperSize =  dr.GetString("PaperSize");
            _marginTop =  dr.GetDecimal("MarginTop");
            _marginBottom =  dr.GetDecimal("MarginBottom");
            _marginLeft =  dr.GetDecimal("MarginLeft");
            _marginRight =  dr.GetDecimal("MarginRight");
            _hidden =  dr.GetBoolean("Hidden");
            _buildIn =  dr.GetBoolean("BuildIn");
            _createDate = dr.GetDateTime("CreateDate");
            _createUserKey = dr.GetInt32("CreateUserKey");
            _lastModifiedDate = dr.GetDateTime("LastModifiedDate");
            _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            _custom1 =  dr.GetString("Custom1");
            _custom2 =  dr.GetString("Custom2");
            _custom3 =  dr.GetString("Custom3");
            ValidationRules.CheckRules();
            return true;            
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(int repKey)
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
                    retValue= this.Insert(cn,repKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope               
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn,int repKey)
        {
            bool retValue = false;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTFinMain_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                //cm.Parameters.AddWithValue("@NewRepKey", repKey);

                if (_repKey == null)
                    cm.Parameters.AddWithValue("@RepKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepKey", _repKey);

                if (_repName == null)
                    cm.Parameters.AddWithValue("@RepName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepName", _repName);

                if (_repType == null)
                    cm.Parameters.AddWithValue("@RepType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepType", _repType);

                if (_remarks == null)
                    cm.Parameters.AddWithValue("@Remarks", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Remarks", _remarks);

                if (_paperSize == null)
                    cm.Parameters.AddWithValue("@PaperSize", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PaperSize", _paperSize);

                if (_marginTop == null)
                    cm.Parameters.AddWithValue("@MarginTop", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MarginTop", _marginTop);

                if (_marginBottom == null)
                    cm.Parameters.AddWithValue("@MarginBottom", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MarginBottom", _marginBottom);

                if (_marginLeft == null)
                    cm.Parameters.AddWithValue("@MarginLeft", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MarginLeft", _marginLeft);

                if (_marginRight == null)
                    cm.Parameters.AddWithValue("@MarginRight", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MarginRight", _marginRight);

                if (_hidden == null)
                    cm.Parameters.AddWithValue("@Hidden", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Hidden", _hidden);

                if (_buildIn == null)
                    cm.Parameters.AddWithValue("@BuildIn", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BuildIn", _buildIn);

                if (_createDate == null)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate);

                if (_createUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateUserKey", _createUserKey);

                if (_lastModifiedDate == null)
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate);

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

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                int i=cm.ExecuteNonQuery();               
                

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.            

            return retValue;
        }

        #endregion //Data Access - Insert

        #region Data Access - New        

        internal int New(SqlConnection cn)
        {
            int retValue = 0;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTFinDetail_GetNew";

                if (_repName == null)
                    cm.Parameters.AddWithValue("@RepKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepKey", _repKey); 

                if (_repName == null)
                    cm.Parameters.AddWithValue("@RepName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepName", _repName);                

                if (_remarks == null)
                    cm.Parameters.AddWithValue("@Remarks", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Remarks", _remarks);                

                if (_createUserKey == null)
                    cm.Parameters.AddWithValue("@UserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserKey", AppInfor.currentUserKey);            

                cm.Parameters.AddWithValue("@NewRepKey", 0);
                cm.Parameters["@NewRepKey"].Direction = ParameterDirection.Output;

                int i = cm.ExecuteNonQuery();
                if ((int)cm.Parameters["@NewRepKey"].Value > 0)
                    retValue = (int)cm.Parameters["@NewRepKey"].Value;                

            }// Already close and dispose sql connection.            

            return retValue;
        }

        #endregion //Data Access - New

        #region Data Access - Update

        internal bool Update(bool IsNewSave)
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
                    retValue = this.Update(cn, IsNewSave);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();                  
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Update(SqlConnection cn, bool IsNewSave)
        {           
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTFinMain_AddUpdate";

                int vOption = GFunc.NEInt(!IsNewSave, 0);
                cm.Parameters.AddWithValue("@Option", vOption);
                cm.Parameters.AddWithValue("@NewRepKey", 0);

                if (_repKey == null)
                    cm.Parameters.AddWithValue("@RepKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepKey", _repKey);

                if (_repName == null)
                    cm.Parameters.AddWithValue("@RepName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepName", _repName);

                if (_repType == null)
                    cm.Parameters.AddWithValue("@RepType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepType", _repType);

                if (_remarks == null)
                    cm.Parameters.AddWithValue("@Remarks", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Remarks", _remarks);

                if (_paperSize == null)
                    cm.Parameters.AddWithValue("@PaperSize", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PaperSize", _paperSize);

                if (_marginTop == null)
                    cm.Parameters.AddWithValue("@MarginTop", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MarginTop", _marginTop);

                if (_marginBottom == null)
                    cm.Parameters.AddWithValue("@MarginBottom", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MarginBottom", _marginBottom);

                if (_marginLeft == null)
                    cm.Parameters.AddWithValue("@MarginLeft", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MarginLeft", _marginLeft);

                if (_marginRight == null)
                    cm.Parameters.AddWithValue("@MarginRight", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MarginRight", _marginRight);

                if (_hidden == null)
                    cm.Parameters.AddWithValue("@Hidden", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Hidden", _hidden);

                if (_buildIn == null)
                    cm.Parameters.AddWithValue("@BuildIn", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BuildIn", _buildIn);

                if (_createDate == null)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate);

                if (_createUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateUserKey", _createUserKey);

                if (_lastModifiedDate == null || _lastModifiedDate == DateTime.MinValue)
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate);

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

                cm.Parameters["@NewRepKey"].Direction = ParameterDirection.InputOutput;

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    this.RepKey = GFunc.NEInt(cm.Parameters["@NewRepKey"].Value, 0);
                    return true;
                }
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
                cm.CommandText = "MSTFinMain_Delete";

                cm.Parameters.AddWithValue("@RepKey", this.RepKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                int i =cm.ExecuteNonQuery();

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
                cm.CommandText = "MSTFinMain_Validation";

               
                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@RepKey", 0);

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
    }
}
