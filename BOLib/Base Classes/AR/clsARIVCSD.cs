using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Collections;
using System.Transactions;
using System.ComponentModel;

namespace BOLib
{
    [Serializable()]
    public class ARIVCSD : Document, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++

        //declare members
        
                
        internal int? _DocConKey;
        internal string _ConID;
        internal string _ConNm;        
        internal decimal? _GrandTotal;
        internal decimal? _HomeSubTotal;
        internal decimal? _HomeTaxTotal;
        internal decimal? _HomeTotal;
        internal string _Remark;
        internal DateTime? _FromDate;
        internal DateTime? _ToDate;
        internal string _Attn;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>
        /// 

        public ARIVCSD()
            : base()
        {
                
        this._DocConKey = 0;
        this._ConID = string.Empty;
        this._ConNm = string.Empty;       
        this._DocTypeNm = string.Empty;
        this._GrandTotal = 0;
        this._HomeSubTotal = 0;
        this._HomeTaxTotal = 0;
        this._HomeTotal = 0;
        this._Remark = string.Empty;
        this._FromDate = DateTime.Today;
        this._ToDate = DateTime.Today;
        this._Attn = string.Empty;
       
        base.PropertyChanged += new PropertyChangedEventHandler(ARIVCSD_PropertyChanged);
        }

        void ARIVCSD_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }

        public ARIVCSD Clone()
        {
            ARIVCSD objCopy = (ARIVCSD)this.MemberwiseClone();
            return objCopy;
        }

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
        

        public string ConID
        {
            get
            {
                return _ConID;
            }
            set
            {
                _ConID = value;
                NotifyPropertyChanged("ConID");
            }
        }

        public string DocConNm
        {
            get
            {
                return _ConNm;
            }
            set
            {
                _ConNm = value;
                NotifyPropertyChanged("DocConNm");
            }
        }

        public string Attn
        {
            get
            {
                return _Attn;
            }
            set
            {
                _Attn = value;
                NotifyPropertyChanged("Attn");
            }
        }

        public DateTime? ToDate
        {
            get
            {
                return _ToDate;
            }
            set
            {
                _ToDate = value;
                NotifyPropertyChanged("ToDate");
            }
        }

        public DateTime? FromDate
        {
            get
            {
                return _FromDate;
            }
            set
            {
                _FromDate = value;
                NotifyPropertyChanged("FromDate");
            }
        }
        public int? DocConKey
        {
            get
            {
                return _DocConKey;
            }
            set
            {
                _DocConKey = value;
                NotifyPropertyChanged("DocConKey");
            }
        }
        public int? DocType
        {
            get
            {
                return _DocType;
            }
            set
            {
                _DocType = value;
                NotifyPropertyChanged("DocType");
            }
        }

       
        public string Remark
        {
            get
            {
                return _Remark;
            }
            set
            {
                _Remark = value;
                NotifyPropertyChanged("Remark");
            }
        }

        public decimal? GrandTotal
        {
            get
            {
                return _GrandTotal;
            }
            set
            {
                _GrandTotal = value;
                NotifyPropertyChanged("GrandTotal");
            }
        }

        public decimal? HomeTaxTotal
        {
            get
            {
                return _HomeTaxTotal;
            }
            set
            {
                _HomeTaxTotal = value;
                NotifyPropertyChanged("HomeTaxTotal");
            }
        }

        public decimal? HomeSubTotal
        {
            get
            {
                return _HomeSubTotal;
            }
            set
            {
                _HomeSubTotal = value;
                NotifyPropertyChanged("HomeSubTotal");
            }
        }

        public decimal? HomeTotal
        {
            get
            {
                return _HomeTotal;
            }
            set
            {
                _HomeTotal = value;
                NotifyPropertyChanged("HomeTotal");
            }
        }
       

        #endregion

       

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _DocKey = null;
            public int? _option = null;
            public string _DocID;
            public int? _DocCodeKey = null;
            internal Criteria()
            {
            }

            internal Criteria(int? DocKey)
            {
                _DocKey = DocKey;
                _DocCodeKey = (int)GEnum.SystemCode.Consolidate_Invoice;
            }

            internal Criteria(int? DocKey, string DocID)
            {
                _DocKey = DocKey;                
                _DocID = DocID;
            }

            internal Criteria(int? DocKey, string DocID, int? Option)
            {
                _DocKey = DocKey;
                _option = Option;
                _DocID = DocID;
            }
        }

        #endregion //Criteria

        public static ARIVCSD Get(int? docKey)
        {
            ARIVCSD child = new ARIVCSD();
            child.Fetch(new Criteria(docKey));
            return child;
        }

        internal static ARIVCSD New(out string msgID)
        {
            msgID = MsgID.Common.NewFail;
            ARIVCSD child = new ARIVCSD();
            msgID = string.Empty;
            return child;
        }

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
                cm.CommandText = "ARIVCSD_Get";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
                cm.Parameters.AddWithValue("@DocID", criteria._DocID);
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
            _DocKey = dr.GetInt32("DocKey");
            _DocID = dr.GetString("DocID");
            _DocConKey = dr.GetInt32("DocConKey");
            _ConNm = dr.GetString("ConNm");

            _DocType = dr.GetInt32("DocType");
            _DocTypeNm = dr.GetString("DocTypeNm");
            _DocCodeKey = dr.GetInt32("DocCodeKey");

            if (GFunc.IsNE(dr.GetValue("DocDate")))
                _DocDate = null;
            else
                _DocDate = dr.GetDateTime("DocDate");

            if (GFunc.IsNE(dr.GetValue("ToDate")))
                _ToDate = null;
            else
                _ToDate = dr.GetDateTime("ToDate");

            if (GFunc.IsNE(dr.GetValue("FromDate")))
                _FromDate = null;
            else
                _FromDate = dr.GetDateTime("FromDate");

            _Remark = dr.GetString("Remark");

            _Attn = dr.GetString("Attn");

            _GrandTotal = Math.Round( dr.GetDecimal("GrandTotal"),2);
            _HomeSubTotal = Math.Round( dr.GetDecimal("HomeSubTotal"),2);
            _HomeTaxTotal = Math.Round( dr.GetDecimal("HomeTaxTotal"),2);
            _HomeTotal = Math.Round( dr.GetDecimal("HomeTotal"),2);

            _CreateUserKey = dr.GetInt32("CreateUserKey");

            if (GFunc.IsNE(dr.GetValue("CreateDate")))
                _CreateDate = null;
            else
                _CreateDate = dr.GetDateTime("CreateDate");

            _LastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");

            if (GFunc.IsNE(dr.GetValue("LastModifiedDate")))
                _LastModifiedDate = null;
            else
                _LastModifiedDate = dr.GetDateTime("LastModifiedDate");

            _LastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            return true;
        }
        #endregion //Data Access - Fetch

        private void Clear()
        {
            _DocKey = 0;
            _DocID = string.Empty;
            _DocType = 100;
            _DocDate = null;
            _DocCodeKey = 0;
            _DocConKey = 0;
            _DocTypeNm = string.Empty;
            _Remark = string.Empty;
            _HomeSubTotal = 0;
            _HomeTaxTotal = 0;
            _HomeTotal = 0;
            _GrandTotal = 0;
            _CreateDate = null;
            _CreateUserKey = null;
            _LastModifiedDate = null;
            _LastModifiedUserKey = null;

        }

        internal bool Update(SqlConnection cn)
        {
            bool retValue = false;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {                
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "ARIVCSD_Update";
                cm.Parameters.AddWithValue("@DocID", _DocID);
                cm.Parameters.AddWithValue("@DocDate", _DocDate);
                cm.Parameters.AddWithValue("@FromDate",_FromDate);
                cm.Parameters.AddWithValue("@ToDate", _ToDate);
                cm.Parameters.AddWithValue("@ConNm", _ConNm);
                cm.Parameters.AddWithValue("@Attn", _Attn);
                if (_Remark == null) _Remark = string.Empty;
                cm.Parameters.AddWithValue("@Remark", _Remark);                  
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                return retValue;
                    // No errors - commit transaction
                    //if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                }// Already close and dispose sql command.

            }// Already close and dispose sql connection.            
        }
    }

