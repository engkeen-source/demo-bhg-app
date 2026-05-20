using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for SYSProcess.
    /// </summary>
    [Serializable]
    public class SYSProcess : IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++

        private int? _CodeKey;
        private string _Action;
        private int? _OldState;
        private int? _NewState;
        private string _ApprovalReq;
        private string _AuthorisedReq;
        private decimal? _Seq;
        private bool _UpApproveInfor;
        private string _UpType;
        private bool _UpAcc;
        private bool _UpItmHis;
        private bool _UpStock;
        private bool _UpCust;
        private bool _UpVend;
        private string _Remarks;
        private string _Custom1;
        private string _Custom2;
        private string _Custom3;
        private bool _isDirty;
        private string _error = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public SYSProcess()
        {
            this._CodeKey = null;
            this._Action = string.Empty;
            this._OldState = null;
            this._NewState = null;
            this._ApprovalReq = string.Empty;
            this._AuthorisedReq = string.Empty;
            this._Seq = null;
            this._UpApproveInfor = false;
            this._UpType = string.Empty;
            this._UpAcc = false;
            this._UpItmHis = false;
            this._UpStock = false;
            this._UpCust = false;
            this._UpVend = false;
            this._Remarks = string.Empty;
            this._Custom1 = string.Empty;
            this._Custom2 = string.Empty;
            this._Custom3 = string.Empty;
            this._isDirty = false;
        }


        public SYSProcess Clone()
        {

            SYSProcess objCopy = (SYSProcess)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
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
            _isDirty = true;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


        public string Error
        {
            get
            {
                return this._error;
            }
            set
            {
                this._error = value;
            }
        }

        public int? CodeKey
        {
            get
            {
                return this._CodeKey;
            }
            set
            {
                this._CodeKey = value;
                NotifyPropertyChanged("CodeKey");
            }
        }

        public string Action
        {
            get
            {
                return this._Action;
            }
            set
            {
                this._Action = value;
                NotifyPropertyChanged("Action");
            }
        }

        public int? OldState
        {
            get
            {
                return this._OldState;
            }
            set
            {
                this._OldState = value;
                NotifyPropertyChanged("OldState");
            }
        }

        public int? NewState
        {
            get
            {
                return this._NewState;
            }
            set
            {
                this._NewState = value;
                NotifyPropertyChanged("NewState");
            }
        }

        public string ApprovalReq
        {
            get
            {
                return this._ApprovalReq;
            }
            set
            {
                this._ApprovalReq = value;
                NotifyPropertyChanged("ApprovalReq");
            }
        }

        public string AuthorisedReq
        {
            get
            {
                return this._AuthorisedReq;
            }
            set
            {
                this._AuthorisedReq = value;
                NotifyPropertyChanged("AuthorisedReq");
            }
        }

        public decimal? Seq
        {
            get
            {
                return this._Seq;
            }
            set
            {
                this._Seq = value;
                NotifyPropertyChanged("Seq");
            }
        }

        public bool UpApproveInfor
        {
            get
            {
                return this._UpApproveInfor;
            }
            set
            {
                this._UpApproveInfor = value;
                NotifyPropertyChanged("UpApproveInfor");
            }
        }

        public string UpType
        {
            get
            {
                return this._UpType;
            }
            set
            {
                this._UpType = value;
                NotifyPropertyChanged("UpType");
            }
        }

        public bool UpAcc
        {
            get
            {
                return this._UpAcc;
            }
            set
            {
                this._UpAcc = value;
                NotifyPropertyChanged("UpAcc");
            }
        }

        public bool UpItmHis
        {
            get
            {
                return this._UpItmHis;
            }
            set
            {
                this._UpItmHis = value;
                NotifyPropertyChanged("UpItmHis");
            }
        }

        public bool UpStock
        {
            get
            {
                return this._UpStock;
            }
            set
            {
                this._UpStock = value;
                NotifyPropertyChanged("UpStock");
            }
        }

        public bool UpCust
        {
            get
            {
                return this._UpCust;
            }
            set
            {
                this._UpCust = value;
                NotifyPropertyChanged("UpCust");
            }
        }

        public bool UpVend
        {
            get
            {
                return this._UpVend;
            }
            set
            {
                this._UpVend = value;
                NotifyPropertyChanged("UpVend");
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

        public bool isDirty
        {
            get
            {
                return this._isDirty;
            }
        }


        #endregion

        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _CodeKey = null;
            public int? _option = null;
            public string _Action = string.Empty;
            public int? _OldState = null;
            public string _ApprovalReq = string.Empty;
            public string _AuthorisedReq = string.Empty;

            internal Criteria()
            {
            }
            internal Criteria(int? CodeKey)
            {
                _CodeKey = CodeKey;
            }
            internal Criteria(int? CodeKey, int? Option)
            {
                _CodeKey = CodeKey;
                _option = Option;
            }
            internal Criteria(int? CodeKey, string Action, int OldState, string ApprovalReq, string AuthorisedReq, int? Option)
            {
                _CodeKey = CodeKey;
                _Action = Action;
                _OldState = OldState;
                _option = Option;
                _ApprovalReq = ApprovalReq;
                _AuthorisedReq = AuthorisedReq;                
                
            }
        }
        #endregion //Criteria


        #region Data Access - Fetch

        internal bool Fetch(Criteria criteria,ref int count)
        {
            bool retValue = false;

            // Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();
                retValue = this.Fetch(cn, criteria,ref count);
            }
       
            return retValue;
        }
        internal bool Fetch(SqlConnection cn, Criteria criteria,ref int count)
        {
            bool retValue = false;
            
            
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSProcess_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@CodeKey", criteria._CodeKey);

                cm.Parameters.AddWithValue("@Action", criteria._Action);
                cm.Parameters.AddWithValue("@OldState", criteria._OldState);
                cm.Parameters.AddWithValue("@ApprovalReq", criteria._ApprovalReq);
                cm.Parameters.AddWithValue("@AuthorisedReq", criteria._AuthorisedReq);
                cm.Parameters.AddWithValue("@Count", count);
                cm.Parameters["@Count"].Direction = ParameterDirection.Output;

                
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
                }// Already close and dispose data reader.
                

                count = Convert.ToInt16(cm.Parameters["@Count"].Value);

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;
            }// Already close and dispose sql connection.
            
            return retValue;
        }

               
        internal static SYSProcess Get(IDataReader dr)
        {
            
            SYSProcess child = new SYSProcess();
            child.Fetch(dr);            
            return child;
        }
        

        internal static SYSProcess Get(int CodeKey, String Action, int OldState, string ApprovalReq, string AuthorisedReq, ref int count)
        {
            
            SYSProcess child = new SYSProcess();
            child.Fetch(new Criteria(CodeKey, Action, OldState, ApprovalReq, AuthorisedReq,1), ref count);
            return child;
        }

        internal bool Fetch(IDataReader dataReader)
        {
            
            
            // Fill data to entity object
            _CodeKey = dataReader["CodeKey"] == DBNull.Value ? null : (int?)dataReader["CodeKey"];
            _Action = dataReader["Action"] == DBNull.Value ? string.Empty : dataReader["Action"].ToString();
            _OldState = dataReader["OldState"] == DBNull.Value ? null : (int?)dataReader["OldState"];
            _NewState = dataReader["NewState"] == DBNull.Value ? null : (int?)dataReader["NewState"];
            _ApprovalReq = dataReader["ApprovalReq"] == DBNull.Value ? string.Empty : dataReader["ApprovalReq"].ToString();
            _AuthorisedReq = dataReader["AuthorisedReq"] == DBNull.Value ? string.Empty : dataReader["AuthorisedReq"].ToString();
            _Seq = dataReader["Seq"] == DBNull.Value ? null : (decimal?)dataReader["Seq"];
            _UpApproveInfor = dataReader["UpApproveInfor"] == DBNull.Value ? false : (bool)dataReader["UpApproveInfor"];
            _UpType = dataReader["UpType"] == DBNull.Value ? string.Empty : dataReader["UpType"].ToString();
            _UpAcc = dataReader["UpAcc"] == DBNull.Value ? false : (bool)dataReader["UpAcc"];
            _UpItmHis = dataReader["UpItmHis"] == DBNull.Value ? false : (bool)dataReader["UpItmHis"];
            _UpStock = dataReader["UpStock"] == DBNull.Value ? false : (bool)dataReader["UpStock"];
            _UpCust = dataReader["UpCust"] == DBNull.Value ? false : (bool)dataReader["UpCust"];
            _UpVend = dataReader["UpVend"] == DBNull.Value ? false : (bool)dataReader["UpVend"];
            _Remarks = dataReader["Remarks"] == DBNull.Value ? string.Empty : dataReader["Remarks"].ToString();
            _Custom1 = dataReader["Custom1"] == DBNull.Value ? string.Empty : dataReader["Custom1"].ToString();
            _Custom2 = dataReader["Custom2"] == DBNull.Value ? string.Empty : dataReader["Custom2"].ToString();
            _Custom3 = dataReader["Custom3"] == DBNull.Value ? string.Empty : dataReader["Custom3"].ToString();


            return true;
           
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert()
        {
            bool retValue = false;
            
            
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            
            return retValue;
        }
        internal bool Insert(SqlConnection cn)
        {
            bool retValue = false;
          
            
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSProcess_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);
                
                cm.Parameters.AddWithValue("@NewCodeKey", 0);
                if (_CodeKey == null)
                    cm.Parameters.AddWithValue("@CodeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeKey", _CodeKey);
                if (_Action == null)
                    cm.Parameters.AddWithValue("@Action", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Action", _Action);
                if (_OldState == null)
                    cm.Parameters.AddWithValue("@OldState", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OldState", _OldState);
                if (_NewState == null)
                    cm.Parameters.AddWithValue("@NewState", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@NewState", _NewState);
                if (_ApprovalReq == null)
                    cm.Parameters.AddWithValue("@ApprovalReq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ApprovalReq", _ApprovalReq);
                if (_AuthorisedReq == null)
                    cm.Parameters.AddWithValue("@AuthorisedReq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AuthorisedReq", _AuthorisedReq);
                if (_Seq == null)
                    cm.Parameters.AddWithValue("@Seq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Seq", _Seq);
                if (_UpApproveInfor == null)
                    cm.Parameters.AddWithValue("@UpApproveInfor", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UpApproveInfor", _UpApproveInfor);
                if (_UpType == null)
                    cm.Parameters.AddWithValue("@UpType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UpType", _UpType);
                if (_UpAcc == null)
                    cm.Parameters.AddWithValue("@UpAcc", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UpAcc", _UpAcc);
                if (_UpItmHis == null)
                    cm.Parameters.AddWithValue("@UpItmHis", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UpItmHis", _UpItmHis);
                if (_UpStock == null)
                    cm.Parameters.AddWithValue("@UpStock", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UpStock", _UpStock);
                if (_UpCust == null)
                    cm.Parameters.AddWithValue("@UpCust", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UpCust", _UpCust);
                if (_UpVend == null)
                    cm.Parameters.AddWithValue("@UpVend", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UpVend", _UpVend);
                if (_Remarks == null)
                    cm.Parameters.AddWithValue("@Remarks", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Remarks", _Remarks);
                if (_Custom1 == null)
                    cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom1", _Custom1);
                if (_Custom2 == null)
                    cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom2", _Custom2);
                if (_Custom3 == null)
                    cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom3", _Custom3);
                
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewCodeKey"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();
                

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue=false;
            }// Already close and dispose sql command.
            
            return retValue;
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
                cm.CommandText = "SYSProcess_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                
                cm.Parameters.AddWithValue("@NewCodeKey", 0);
                if (_CodeKey == null)
                    cm.Parameters.AddWithValue("@CodeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeKey", _CodeKey);
                if (_Action == null)
                    cm.Parameters.AddWithValue("@Action", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Action", _Action);
                if (_OldState == null)
                    cm.Parameters.AddWithValue("@OldState", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OldState", _OldState);
                if (_NewState == null)
                    cm.Parameters.AddWithValue("@NewState", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@NewState", _NewState);
                if (_ApprovalReq == null)
                    cm.Parameters.AddWithValue("@ApprovalReq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ApprovalReq", _ApprovalReq);
                if (_AuthorisedReq == null)
                    cm.Parameters.AddWithValue("@AuthorisedReq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AuthorisedReq", _AuthorisedReq);
                if (_Seq == null)
                    cm.Parameters.AddWithValue("@Seq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Seq", _Seq);
                if (_UpApproveInfor == null)
                    cm.Parameters.AddWithValue("@UpApproveInfor", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UpApproveInfor", _UpApproveInfor);
                if (_UpType == null)
                    cm.Parameters.AddWithValue("@UpType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UpType", _UpType);
                if (_UpAcc == null)
                    cm.Parameters.AddWithValue("@UpAcc", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UpAcc", _UpAcc);
                if (_UpItmHis == null)
                    cm.Parameters.AddWithValue("@UpItmHis", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UpItmHis", _UpItmHis);
                if (_UpStock == null)
                    cm.Parameters.AddWithValue("@UpStock", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UpStock", _UpStock);
                if (_UpCust == null)
                    cm.Parameters.AddWithValue("@UpCust", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UpCust", _UpCust);
                if (_UpVend == null)
                    cm.Parameters.AddWithValue("@UpVend", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UpVend", _UpVend);
                if (_Remarks == null)
                    cm.Parameters.AddWithValue("@Remarks", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Remarks", _Remarks);
                if (_Custom1 == null)
                    cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom1", _Custom1);
                if (_Custom2 == null)
                    cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom2", _Custom2);
                if (_Custom3 == null)
                    cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom3", _Custom3);
                
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewCodeKey"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();
                

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
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
                cm.CommandText = "SYSProcess_Delete";

                
                cm.Parameters.AddWithValue("@CodeKey", criteria._CodeKey);
                
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
        #endregion Delete
    }
}