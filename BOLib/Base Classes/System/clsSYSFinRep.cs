using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using TAUtil;
namespace BOLib
{
    /// <summary>
    /// Summary description for SYSFinRep.
    /// </summary>
    [Serializable]
    public class SYSFinRep : INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected int? _FinRepKey;
        protected int? _RepKey;
        protected string _RepName;
        protected string _RepType;
        protected string _RptNm;
        protected string _RptDes;
        protected string _Remarks;
        protected bool? _BuildIn;
        protected string _PaperSize;
        protected float? _MarginTop;
        protected float? _MarginBottom;
        protected float? _MarginLeft;
        protected float? _MarginRight;
        protected int? _Orientantion;
        protected DateTime? _CreateDate;
        protected int? _CreateUserKey;
        protected DateTime? _LastModifiedDate;
        protected int? _LastModifiedUserKey;
        protected string _Custom1;
        protected string _Custom2;
        protected string _Custom3;
        protected bool _isDirty;
        protected bool _isNew;
        protected bool _isReadOnly;
        protected int? _GUID;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public SYSFinRep()
            : base()
        {
            this._FinRepKey = 0;
            this._RepKey = 0;
            this._RepName = string.Empty;
            this._RepType = string.Empty;
            this.RptNm = string.Empty;
            this.RptDes = string.Empty;
            this._Remarks = string.Empty;
            this._BuildIn = false;
            this._PaperSize = string.Empty;
            this._MarginTop = 0;
            this._MarginBottom = 0;
            this._MarginLeft = 0;
            this._MarginRight = 0;
            this._Orientantion = 0;
            this.CreateDate = Convert.ToDateTime("01-jan-1900 12:00:00 am");
            this.CreateUserKey = 0;
            this.LastModifiedDate = Convert.ToDateTime("01-jan-1900 12:00:00 am");
            this.LastModifiedUserKey = 0;
            this.Custom1 = string.Empty;
            this.Custom2 = string.Empty;
            this.Custom3 = string.Empty;

        }

        public SYSFinRep Clone()
        {
            SYSFinRep objCopy = (SYSFinRep)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
        }
        public static SYSFinRep Get(int? FinRepKey)
        {
            SYSFinRep child = new SYSFinRep();
            child.Fetch(new Criteria(FinRepKey, 1));
            return child;
        }

        public static SYSFinRep New()
        {
            SYSFinRep child = new SYSFinRep();
            return child;
        }

        //Need for implementing IDataErrorInfo interface
        public string this[string name]
        {
            get
            {
                string result = string.Empty;
                return result;
            }
        }
        /// <summary>
        /// Disposing objects
        /// </summary>
        public void Dispose()
        {

        }

        #endregion

        #region +++  Properties  +++

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public int? FinRepKey
        {

            get
            {
                return this._FinRepKey;
            }
            set
            {
                this._FinRepKey = value;
                NotifyPropertyChanged("FinRepKey");
            }
        }
        public int? RepKey
        {

            get
            {
                return this._RepKey;
            }
            set
            {
                this._RepKey = value;
                NotifyPropertyChanged("RepKey");
            }
        }
        public string RepName
        {

            get
            {
                return this._RepName;
            }
            set
            {
                this._RepName = value;
                NotifyPropertyChanged("RepName");
            }
        }
        public string RepType
        {

            get
            {
                return this._RepType;
            }
            set
            {
                this._RepType = value;
                NotifyPropertyChanged("RepType");
            }
        }
        public string RptNm
        {

            get
            {
                return this._RptNm;
            }
            set
            {
                this._RptNm = value;
                NotifyPropertyChanged("RptNm");
            }
        }
        public string RptDes
        {

            get
            {
                return this._RptDes;
            }
            set
            {
                this._RptDes = value;
                NotifyPropertyChanged("RptDes");
            }
        }
        public string Remarks
        {

            get
            {
                return this._Remarks;
            }
            set
            {
                this._Remarks = value;
                NotifyPropertyChanged("Remarks");
            }
        }
        public bool? BuildIn
        {

            get
            {
                return this._BuildIn;
            }
            set
            {
                this._BuildIn = value;
                NotifyPropertyChanged("BuildIn");
            }
        }
        public string PaperSize
        {

            get
            {
                return this._PaperSize;
            }
            set
            {
                this._PaperSize = value;
                NotifyPropertyChanged("PaperSize");
            }
        }
        public float? MarginTop
        {

            get
            {
                return this._MarginTop;
            }
            set
            {
                this._MarginTop = value;
                NotifyPropertyChanged("MarginTop");
            }
        }
        public float? MarginBottom
        {

            get
            {
                return this._MarginBottom;
            }
            set
            {
                this._MarginBottom = value;
                NotifyPropertyChanged("MarginBottom");
            }
        }
        public float? MarginLeft
        {

            get
            {
                return this._MarginLeft;
            }
            set
            {
                this._MarginLeft = value;
                NotifyPropertyChanged("MarginLeft");
            }
        }
        public float? MarginRight
        {

            get
            {
                return this._MarginRight;
            }
            set
            {
                this._MarginRight = value;
                NotifyPropertyChanged("MarginRight");
            }
        }
        public int? Orientantion
        {

            get
            {
                return this._Orientantion;
            }
            set
            {
                this._Orientantion = value;
                NotifyPropertyChanged("Orientantion");
            }
        }

        public DateTime? CreateDate
        {
            get
            {
                return this._CreateDate;
            }
            set
            {
                this._CreateDate = value;
                NotifyPropertyChanged("CreateDate");
            }
        }

        public int? CreateUserKey
        {
            get
            {
                return this._CreateUserKey;
            }
            set
            {
                this._CreateUserKey = value;
                NotifyPropertyChanged("CreateUserKey");
            }
        }

        public DateTime? LastModifiedDate
        {
            get
            {
                return this._LastModifiedDate;
            }
            set
            {
                this._LastModifiedDate = value;
                NotifyPropertyChanged("LastModifiedDate");
            }
        }

        public int? LastModifiedUserKey
        {
            get
            {
                return this._LastModifiedUserKey;
            }
            set
            {
                this._LastModifiedUserKey = value;
                NotifyPropertyChanged("LastModifiedUserKey");
            }
        }

        public string Custom1
        {
            get
            {
                return this._Custom1;
            }
            set
            {
                this._Custom1 = value;
                NotifyPropertyChanged("Custom1");
            }
        }

        public string Custom2
        {
            get
            {
                return this._Custom2;
            }
            set
            {
                this._Custom2 = value;
                NotifyPropertyChanged("Custom2");
            }
        }

        public string Custom3
        {
            get
            {
                return this._Custom3;
            }
            set
            {
                this._Custom3 = value;
                NotifyPropertyChanged("Custom3");
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
                NotifyPropertyChanged("GUID");
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
        #endregion

        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _RepKey = null;
            public int? _FinRepKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }
            internal Criteria(int? FinRepKey)
            {
                _FinRepKey = FinRepKey;
            }
            internal Criteria(int? FinRepKey, int? Option)
            {
                _FinRepKey = FinRepKey;
                _RepKey = 0;
                _option = Option;
            }
            internal Criteria(int RepKey, int? FinRepKey, int? Option)
            {
                _RepKey = RepKey;
                _FinRepKey = FinRepKey;
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
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSFinRep_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@FinRepKey", criteria._FinRepKey);
                cm.Parameters.AddWithValue("@RepKey", criteria._RepKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Using data reader as record set.
                using (IDataReader dr = cm.ExecuteReader())
                {
                    //If data reader can read, continue...
                    while (dr.Read())
                    {
                        retValue = this.Fetch(dr);
                    }
                    if (!retValue)
                        return false;
                }// Already close and dispose data reader.


                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal DataTable FetchAll()
        {

            DataTable reval = new DataTable("dtSYSFinRep");
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    using (SqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "SYSFinRep_Get";

                        cm.Parameters.AddWithValue("@Option", 0);
                        cm.Parameters.AddWithValue("@FinRepKey", 0);
                        cm.Parameters.AddWithValue("@RepKey", 0);
                        cm.Parameters.AddWithValue("@RetValue", 0);
                        cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                        // Using data reader as record set.

                        SqlDataAdapter adt = new SqlDataAdapter(cm);
                        adt.Fill(reval);

                    }// Already close and dispose sql connection.

                }
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }
            return reval;
        }
        internal static SYSFinRep Get(IDataReader dr)
        {
            SYSFinRep child = new SYSFinRep();
            child.Fetch(dr);
            return child;
        }
        internal static SYSFinRep Get(SqlConnection cn, Criteria criteria)
        {
            SYSFinRep child = new SYSFinRep();
            child.Fetch(cn, criteria);
            return child;
        }

        internal bool Fetch(IDataReader dataReader)
			{               
				_FinRepKey = dataReader["FinRepKey"] == DBNull.Value ? (int? )0 : (int? )dataReader["FinRepKey"];				
				_RepKey = dataReader["RepKey"] == DBNull.Value ? (int? )0 : (int? )dataReader["RepKey"];				
				_RepName = dataReader["RepName"] == DBNull.Value ? (string )string.Empty : (string )dataReader["RepName"];				
				_RepType = dataReader["RepType"] == DBNull.Value ? (string )string.Empty : (string )dataReader["RepType"];
                _RptNm = dataReader["RptNm"] == DBNull.Value ? (string)string.Empty : (string)dataReader["RptNm"];
                _RptDes = dataReader["RptDes"] == DBNull.Value ? (string)string.Empty : (string)dataReader["RptDes"];				
				_Remarks = dataReader["Remarks"] == DBNull.Value ? (string )string.Empty : (string )dataReader["Remarks"];				
				_BuildIn = dataReader["BuildIn"] == DBNull.Value ? (bool? )false : (bool? )dataReader["BuildIn"];				
				_PaperSize = dataReader["PaperSize"] == DBNull.Value ? (string )new object() : (string)dataReader["PaperSize"];				
				_MarginTop = dataReader["MarginTop"] == DBNull.Value ? 0.00f : float.Parse(dataReader["MarginTop"].ToString());
                _MarginBottom = dataReader["MarginBottom"] == DBNull.Value ? 0.00f : float.Parse(dataReader["MarginBottom"].ToString());
                _MarginLeft = dataReader["MarginLeft"] == DBNull.Value ? 0.00f : float.Parse(dataReader["MarginLeft"].ToString());
                _MarginRight = dataReader["MarginRight"] == DBNull.Value ? 0.00f : float.Parse(dataReader["MarginRight"].ToString());				
				_Orientantion = dataReader["Orientantion"] == DBNull.Value ? (int? )0 : (int? )dataReader["Orientantion"];				
				_CreateDate = dataReader["CreateDate"] == DBNull.Value ? Convert.ToDateTime("01-jan-1900 12:00:00 am") : (DateTime?)dataReader["CreateDate"];				
				_CreateUserKey = dataReader["CreateUserKey"] == DBNull.Value ? (int? )0 : (int? )dataReader["CreateUserKey"];
                _LastModifiedDate = dataReader["LastModifiedDate"] == DBNull.Value ? Convert.ToDateTime("01-jan-1900 12:00:00 am") : (DateTime?)dataReader["LastModifiedDate"];				
				_LastModifiedUserKey = dataReader["LastModifiedUserKey"] == DBNull.Value ? (int? )0 : (int? )dataReader["LastModifiedUserKey"];				
				_Custom1 = dataReader["Custom1"] == DBNull.Value ? (string )string.Empty : (string )dataReader["Custom1"];				
				_Custom2 = dataReader["Custom2"] == DBNull.Value ? (string )string.Empty : (string )dataReader["Custom2"];				
				_Custom3 = dataReader["Custom3"] == DBNull.Value ? (string )string.Empty : (string )dataReader["Custom3"];				
				 
                return true;
			}
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert()
        {
            bool retValue = false;
            FinRepKey = null;
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn,false);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }
        internal bool Insert(SqlConnection cn,bool IsUpdate)
        {
            //FinRepKey = 0;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSFinRep_AddUpdate";

                cm.Parameters.AddWithValue("@Option", Convert.ToInt32(IsUpdate));
                cm.Parameters.AddWithValue("@NewFinRepKey", 0);

                if (_FinRepKey == null)
                {
                    cm.Parameters.AddWithValue("@FinRepKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@FinRepKey", _FinRepKey);
                }
                if (_RepKey == null)
                {
                    cm.Parameters.AddWithValue("@RepKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RepKey", _RepKey);
                }
                if (_RepName == null)
                {
                    cm.Parameters.AddWithValue("@RepName", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RepName", _RepName);
                }
                if (_RepType == null)
                {
                    cm.Parameters.AddWithValue("@RepType", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RepType", _RepType);
                }
                if (_RptNm == null)
                {
                    cm.Parameters.AddWithValue("@RptNm", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RptNm", _RptNm);
                }
                if (_RptDes == null)
                {
                    cm.Parameters.AddWithValue("@RptDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RptDes", _RptDes);
                }
                if (_Remarks == null)
                {
                    cm.Parameters.AddWithValue("@Remarks", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Remarks", _Remarks);
                }
                if (_BuildIn == null)
                {
                    cm.Parameters.AddWithValue("@BuildIn", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@BuildIn", _BuildIn);
                }
                if (_PaperSize == null)
                {
                    cm.Parameters.AddWithValue("@PaperSize", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@PaperSize", _PaperSize);
                }
                if (_MarginTop == null)
                {
                    cm.Parameters.AddWithValue("@MarginTop", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@MarginTop", _MarginTop);
                }
                if (_MarginBottom == null)
                {
                    cm.Parameters.AddWithValue("@MarginBottom", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@MarginBottom", _MarginBottom);
                }
                if (_MarginLeft == null)
                {
                    cm.Parameters.AddWithValue("@MarginLeft", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@MarginLeft", _MarginLeft);
                }
                if (_MarginRight == null)
                {
                    cm.Parameters.AddWithValue("@MarginRight", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@MarginRight", _MarginRight);
                }
                if (_Orientantion == null)
                {
                    cm.Parameters.AddWithValue("@Orientantion", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Orientantion", _Orientantion);
                }
                if (_CreateDate == null)
                {
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@CreateDate", _CreateDate);
                }
                if (_CreateUserKey == null)
                {
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@CreateUserKey", _CreateUserKey);
                }
                if (_LastModifiedDate == null)
                {
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@LastModifiedDate", _LastModifiedDate);
                }
                if (_LastModifiedUserKey == null)
                {
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", _LastModifiedUserKey);
                }
                if (_Custom1 == null)
                {
                    cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom1", _Custom1);
                }
                if (_Custom2 == null)
                {
                    cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom2", _Custom2);
                }
                if (_Custom3 == null)
                {
                    cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom3", _Custom3);
                }


                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@ReturnFinRepKey", 0);
                cm.Parameters["@ReturnFinRepKey"].Direction = ParameterDirection.Output;

                // Execute command.
                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    if (IsUpdate == false)
                    {this.FinRepKey = GFunc.NEInt(cm.Parameters["@ReturnFinRepKey"].Value, 0);}
                    return true;
                }
                else
                    return false;
            }// Already close and dispose sql command.                
        }
        #endregion Insert

        #region Data Access - Update

        internal bool Update()
        {
            bool retValue = false;
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Update(cn);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }
        internal bool Update(SqlConnection cn)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSFinRep_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                if (_FinRepKey == null)
                {
                    cm.Parameters.AddWithValue("@FinRepKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@FinRepKey", _FinRepKey);
                }
                if (_RepKey == null)
                {
                    cm.Parameters.AddWithValue("@RepKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RepKey", _RepKey);
                }
                if (_RepName == null)
                {
                    cm.Parameters.AddWithValue("@RepName", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RepName", _RepName);
                }
                if (_RepType == null)
                {
                    cm.Parameters.AddWithValue("@RepType", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RepType", _RepType);
                }
                if (_RptNm == null)
                {
                    cm.Parameters.AddWithValue("@RptNm", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RptNm", _RptNm);
                }
                if (_RptDes == null)
                {
                    cm.Parameters.AddWithValue("@RptDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@RptDes", _RepType);
                }

                if (_Remarks == null)
                {
                    cm.Parameters.AddWithValue("@Remarks", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Remarks", _Remarks);
                }
                if (_BuildIn == null)
                {
                    cm.Parameters.AddWithValue("@BuildIn", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@BuildIn", _BuildIn);
                }
                if (_PaperSize == null)
                {
                    cm.Parameters.AddWithValue("@PaperSize", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@PaperSize", _PaperSize);
                }
                if (_MarginTop == null)
                {
                    cm.Parameters.AddWithValue("@MarginTop", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@MarginTop", _MarginTop);
                }
                if (_MarginBottom == null)
                {
                    cm.Parameters.AddWithValue("@MarginBottom", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@MarginBottom", _MarginBottom);
                }
                if (_MarginLeft == null)
                {
                    cm.Parameters.AddWithValue("@MarginLeft", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@MarginLeft", _MarginLeft);
                }
                if (_MarginRight == null)
                {
                    cm.Parameters.AddWithValue("@MarginRight", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@MarginRight", _MarginRight);
                }
                if (_Orientantion == null)
                {
                    cm.Parameters.AddWithValue("@Orientantion", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Orientantion", _Orientantion);
                }
                if (_CreateDate == null)
                {
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@CreateDate", _CreateDate);
                }
                if (_CreateUserKey == null)
                {
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@CreateUserKey", _CreateUserKey);
                }
                if (_LastModifiedDate == null)
                {
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@LastModifiedDate", _LastModifiedDate);
                }
                if (_LastModifiedUserKey == null)
                {
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", _LastModifiedUserKey);
                }
                if (_Custom1 == null)
                {
                    cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom1", _Custom1);
                }
                if (_Custom2 == null)
                {
                    cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom2", _Custom2);
                }
                if (_Custom3 == null)
                {
                    cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom3", _Custom3);
                }

                cm.Parameters.AddWithValue("@NewFinRepKey", 0);
                cm.Parameters["@NewFinRepKey"].Direction = ParameterDirection.Output;
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    this.FinRepKey = (int)cm.Parameters["@NewFinRepKey"].Value;
                    return true;
                }
                else
                    return false;

            }// Already close and dispose sql command.

        }
        #endregion Update

        #region Data Access - Delete

        internal bool Delete(Criteria criteria)
        {
            bool retValue = false;
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Delete(cn, criteria);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }
        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSFinRep_Delete";

                cm.Parameters.AddWithValue("@FinRepKey", criteria._FinRepKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
            }// Already close and dispose sql command.

            return retValue;
        }
        #endregion Delete

        #region Data Access - Validation

        internal bool Validation(Criteria criteria, bool isNew)
        {
            bool retValue = false;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    //Create new sql connection for this method. 
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open sql connection. 
                        cn.Open();
                        retValue = Validation(cn, criteria, isNew);
                    }
                    // No errors - commit transaction
                      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                }// Already close and dispose sql connection.
            }
            catch (TAException taex)
            {
                throw taex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retValue;
        }
        internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
        {

            try
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "SYSFinRep_Validation";


                    cm.Parameters.AddWithValue("@isNew", isNew);
                    cm.Parameters.AddWithValue("@RepKey", criteria._RepKey);
                    cm.Parameters.AddWithValue("@FinRepKey", criteria._FinRepKey);
                    cm.Parameters.AddWithValue("@RetValue", 0);

                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        return true;
                    else
                        return false;
                }// Already close and dispose sql command.
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
        #endregion Validation
    }
}





