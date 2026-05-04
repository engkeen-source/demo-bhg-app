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
    /// Summary description for GLRV.
    /// </summary>
    [Serializable]
    public class GLRV : Document, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        internal int _DocDeptKey;
        internal int? _DocTranGrpKey;
        internal int _DocGrpKey;
        internal int _DocAccBKKey;
        internal int _DocAccGainKey;
        internal int _DocAccLossKey;
        internal int _DocCurrKey;
        internal decimal _DocRevalueRate;
        internal decimal _DocAccAmtF;
        internal decimal _DocAccAmtHOld;
        internal decimal _DocAccAmtHNew;
        internal decimal _DocAccAmtGainLoss;
        internal decimal _DocCountryRate;
        internal string _DocAccBKID;
        internal string _DocAccBKDes;
        internal string _DocAccGainID;
        internal string _DocAccGainDes;
        internal string _DocAccLossID;
        internal string _DocAccLossDes;
        internal SYSAttachments attachments= new SYSAttachments();

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public GLRV()
            : base()
        {
            this._DocDeptKey = 0;
            this._DocTranGrpKey = 0;
            this._DocGrpKey = 0;
            this._DocAccBKKey = 0;
            this._DocAccGainKey = 0;
            this._DocAccLossKey = 0;
            this._DocCurrKey = 0;
            this._DocRevalueRate = 0;
            this._DocAccAmtF = 0;
            this._DocAccAmtHOld = 0;
            this._DocAccAmtHNew = 0;
            this._DocAccAmtGainLoss = 0;
            this._DocCountryRate = 0;
            this._DocAccBKID = string.Empty;
            this._DocAccBKDes = string.Empty;
            this._DocAccGainID = string.Empty;
            this._DocAccGainDes = string.Empty;
            this._DocAccLossID = string.Empty;
            this._DocAccLossDes = string.Empty;
            base.PropertyChanged += new PropertyChangedEventHandler(GLRV_PropertyChanged);
        }
        void GLRV_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }

        public GLRV Clone()
        {
            GLRV objCopy = (GLRV)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
        }
        public static GLRV Get(int? docKey)
        {
            GLRV child = new GLRV();
            child.Fetch(new Criteria(docKey, 1));
            return child;
        }

        public static GLRV New()
        {
            GLRV child = new GLRV();
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

        internal void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public SYSAttachments Attachments
        {
            get { return attachments; }
            set { attachments = value; }
        }
        public int DocDeptKey
        {

            get
            {
                return this._DocDeptKey;
            }
            set
            {
                this._DocDeptKey = value;
                NotifyPropertyChanged("DocDeptKey");
            }
        }
        public int? DocTranGrpKey
        {

            get
            {
                return this._DocTranGrpKey;
            }
            set
            {
                this._DocTranGrpKey = value;
                NotifyPropertyChanged("DocTranGrpKey");
            }
        }
        public int DocGrpKey
        {

            get
            {
                return this._DocGrpKey;
            }
            set
            {
                this._DocGrpKey = value;
                NotifyPropertyChanged("DocGrpKey");
            }
        }
        public int DocAccBKKey
        {

            get
            {
                return this._DocAccBKKey;
            }
            set
            {
                this._DocAccBKKey = value;
                NotifyPropertyChanged("DocAccBKKey");
            }
        }
        public int DocAccGainKey
        {

            get
            {
                return this._DocAccGainKey;
            }
            set
            {
                this._DocAccGainKey = value;
                NotifyPropertyChanged("DocAccGainKey");
            }
        }
        public int DocAccLossKey
        {

            get
            {
                return this._DocAccLossKey;
            }
            set
            {
                this._DocAccLossKey = value;
                NotifyPropertyChanged("DocAccLossKey");
            }
        }
        public int DocCurrKey
        {

            get
            {
                return this._DocCurrKey;
            }
            set
            {
                this._DocCurrKey = value;
                NotifyPropertyChanged("DocCurrKey");
            }
        }
        public decimal DocRevalueRate
        {

            get
            {
                return this._DocRevalueRate;
            }
            set
            {
                this._DocRevalueRate = value;
                NotifyPropertyChanged("DocRevalueRate");
            }
        }
        public decimal DocAccAmtF
        {

            get
            {
                return this._DocAccAmtF;
            }
            set
            {
                this._DocAccAmtF = value;
                NotifyPropertyChanged("DocAccAmtF");
            }
        }
        public decimal DocAccAmtHOld
        {

            get
            {
                return this._DocAccAmtHOld;
            }
            set
            {
                this._DocAccAmtHOld = value;
                NotifyPropertyChanged("DocAccAmtHOld");
            }
        }
        public decimal DocAccAmtHNew
        {

            get
            {
                return this._DocAccAmtHNew;
            }
            set
            {
                this._DocAccAmtHNew = value;
                NotifyPropertyChanged("DocAccAmtHNew");
            }
        }
        public decimal DocAccAmtGainLoss
        {

            get
            {
                return this._DocAccAmtGainLoss;
            }
            set
            {
                this._DocAccAmtGainLoss = value;
                NotifyPropertyChanged("DocAccAmtGainLoss");
            }
        }
        public decimal DocCountryRate
        {

            get
            {
                return this._DocCountryRate;
            }
            set
            {
                this._DocCountryRate = value;
                NotifyPropertyChanged("DocCountryRate");
            }
        }
        public string DocAccBKID
        {

            get
            {
                return this._DocAccBKID;
            }
            set
            {
                this._DocAccBKID = value;
                NotifyPropertyChanged("DocAccBKID");
            }
        }
        public string DocAccBKDes
        {

            get
            {
                return this._DocAccBKDes;
            }
            set
            {
                this._DocAccBKDes = value;
                NotifyPropertyChanged("DocAccBKDes");
            }
        }
        public string DocAccGainID
        {

            get
            {
                return this._DocAccGainID;
            }
            set
            {
                this._DocAccGainID = value;
                NotifyPropertyChanged("DocAccGainID");
            }
        }
        public string DocAccGainDes
        {

            get
            {
                return this._DocAccGainDes;
            }
            set
            {
                this._DocAccGainDes = value;
                NotifyPropertyChanged("DocAccGainDes");
            }
        }
        public string DocAccLossID
        {

            get
            {
                return this._DocAccLossID;
            }
            set
            {
                this._DocAccLossID = value;
                NotifyPropertyChanged("DocAccLossID");
            }
        }
        public string DocAccLossDes
        {

            get
            {
                return this._DocAccLossDes;
            }
            set
            {
                this._DocAccLossDes = value;
                NotifyPropertyChanged("DocAccLossDes");
            }
        }


        #endregion

        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _DocCodeKey = null;
            public int? _DocKey = null;
            public int? _option = null;
            public string _DocID = string.Empty;

            internal Criteria()
            {
            }
            internal Criteria(int? DocKey)
            {
                _DocKey = DocKey;
            }
            internal Criteria(int? DocKey, int? Option)
            {
                _DocKey = DocKey;
                _option = Option;
            }
            internal Criteria(int DocCodeKey, int? DocKey, int? Option)
            {
                _DocCodeKey = DocCodeKey;
                _DocKey = DocKey;
                _option = Option;
            }
            internal Criteria(int? DocCodeKey, int? DocKey, string DocID, int? Option)
            {
                _DocCodeKey = DocCodeKey;
                _DocKey = DocKey;
                _DocID = DocID;
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
                cm.CommandText = "GLRV_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
                cm.Parameters.AddWithValue("@DocID", criteria._DocID);
                cm.Parameters.AddWithValue("@DocCodeKey", criteria._DocCodeKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Using data reader as record set.
                using (IDataReader dr = cm.ExecuteReader())
                {
                    //If data reader can read, continue...
                    if (dr.Read())
                    {
                        retValue = this.Fetch(dr);
                    }
                    else
                        this.Clear();
                }// Already close and dispose data reader.


                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;
            }// Already close and dispose sql connection.

            return retValue;
        }
        internal static GLRV Get(IDataReader dr)
        {
            GLRV child = new GLRV();
            child.Fetch(dr);
            return child;
        }
        internal static GLRV Get(SqlConnection cn, Criteria criteria)
        {
            GLRV child = new GLRV();
            child.Fetch(cn, criteria);
            return child;
        }

        internal bool Fetch(IDataReader dataReader)
        {
            _DocKey = dataReader["DocKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocKey"];
            _DocCodeKey = dataReader["DocCodeKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocCodeKey"];
            _DocID = dataReader["DocID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocID"];
            _DocDate = dataReader["DocDate"] == DBNull.Value ? (DateTime)DateTime.Today.Date : (DateTime)dataReader["DocDate"];
            _DocType = dataReader["DocType"] == DBNull.Value ? (int)0 : (int)dataReader["DocType"];
            _DocTypeNm = dataReader["DocTypeNm"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocTypeNm"];
            _DocSign = dataReader["DocSign"] == DBNull.Value ? (Int16)0 : (Int16)dataReader["DocSign"];
            _DocDeptKey = dataReader["DocDeptKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocDeptKey"];
            _DocTranGrpKey = dataReader["DocTranGrpKey"] == DBNull.Value ? (int?)0 : (int?)dataReader["DocTranGrpKey"];
            _DocGrpKey = dataReader["DocGrpKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocGrpKey"];
            _DocEmKey = dataReader["DocEmKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocEmKey"];
            _DocAccBKKey = dataReader["DocAccBKKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocAccBKKey"];
            _DocAccGainKey = dataReader["DocAccGainKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocAccGainKey"];
            _DocAccLossKey = dataReader["DocAccLossKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocAccLossKey"];
            _DocRef = dataReader["DocRef"] == DBNull.Value ? (string)null : (string)dataReader["DocRef"];
            _DocDes = dataReader["DocDes"] == DBNull.Value ? (string)null : (string)dataReader["DocDes"];
            _DocRem = dataReader["DocRem"] == DBNull.Value ? (string)null : (string)dataReader["DocRem"];
            _DocCurrKey = dataReader["DocCurrKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocCurrKey"];
            _DocRevalueRate = dataReader["DocRevalueRate"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocRevalueRate"];
            _DocAccAmtF = dataReader["DocAccAmtF"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocAccAmtF"];
            _DocAccAmtHOld = dataReader["DocAccAmtHOld"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocAccAmtHOld"];
            _DocAccAmtHNew = dataReader["DocAccAmtHNew"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocAccAmtHNew"];
            _DocAccAmtGainLoss = dataReader["DocAccAmtGainLoss"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocAccAmtGainLoss"];
            _DocCountryRate = dataReader["DocCountryRate"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocCountryRate"];
            _DocStatus = dataReader["DocStatus"] == DBNull.Value ? (string)null : (string)dataReader["DocStatus"];
            _DocState = dataReader["DocState"] == DBNull.Value ? (int)0 : (int)dataReader["DocState"];
            _DocPrinted = dataReader["DocPrinted"] == DBNull.Value ? (bool)false : (bool)dataReader["DocPrinted"];
            _ApproveUserKey = dataReader["ApproveUserKey"] == DBNull.Value ? (int)0 : (int)dataReader["ApproveUserKey"];
            _ApproveDate = dataReader["ApproveDate"] == DBNull.Value ? null : (DateTime?)dataReader["ApproveDate"];
            _DisapproveUserKey = dataReader["DisapproveUserKey"] == DBNull.Value ? (int)0 : (int)dataReader["DisapproveUserKey"];
            _DisapproveDate = dataReader["DisapproveDate"] == DBNull.Value ? null : (DateTime?)dataReader["DisapproveDate"];
            _DisapproveCount = dataReader["DisapproveCount"] == DBNull.Value ? (Int16)0 : (Int16)dataReader["DisapproveCount"];
            _DisapproveMsg = dataReader["DisapproveMsg"] == DBNull.Value ? (string)null : (string)dataReader["DisapproveMsg"];
            _Attachment = dataReader["Attachment"] == DBNull.Value ? (bool)false : (bool)dataReader["Attachment"];
            _BranchKey = dataReader["BranchKey"] == DBNull.Value ? (int)0 : (int)dataReader["BranchKey"];
            _CreateDate = dataReader["CreateDate"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["CreateDate"];
            _CreateUserKey = dataReader["CreateUserKey"] == DBNull.Value ? (int?)null : (int?)dataReader["CreateUserKey"];
            _LastModifiedDate = dataReader["LastModifiedDate"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["LastModifiedDate"];
            _LastModifiedUserKey = dataReader["LastModifiedUserKey"] == DBNull.Value ? (int?)null : (int?)dataReader["LastModifiedUserKey"];
            
            _PurgeKeep = dataReader["PurgeKeep"] == DBNull.Value ? (int)0 : (int)dataReader["PurgeKeep"];
            _PurgeData = dataReader["PurgeData"] == DBNull.Value ? (bool)false : (bool)dataReader["PurgeData"];
            _Custom1 = dataReader["Custom1"] == DBNull.Value ? (string)null : (string)dataReader["Custom1"];
            _Custom2 = dataReader["Custom2"] == DBNull.Value ? (string)null : (string)dataReader["Custom2"];
            _Custom3 = dataReader["Custom3"] == DBNull.Value ? (string)null : (string)dataReader["Custom3"];
            _Custom4 = dataReader["Custom4"] == DBNull.Value ? (string)null : (string)dataReader["Custom4"];
            _Custom5 = dataReader["Custom5"] == DBNull.Value ? (string)null : (string)dataReader["Custom5"];
            _DocAccBKID = dataReader["DocAccBKID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocAccBKID"];
            _DocAccBKDes = dataReader["DocAccBKDes"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocAccBKDes"];
            _DocAccGainID = dataReader["DocAccGainID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocAccGainID"];
            _DocAccGainDes = dataReader["DocAccGainDes"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocAccGainDes"];
            _DocAccLossID = dataReader["DocAccLossID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocAccLossID"];
            _DocAccLossDes = dataReader["DocAccLossDes"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocAccLossDes"];

           
            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert()
        {
            bool retValue = false;
            DocKey = null;
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
            string msgID = "RecordAddFail";
            DocKey = 0;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "GLRV_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                if (_DocKey == null)
                {
                    cm.Parameters.AddWithValue("@DocKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocKey", _DocKey);
                }
                if (_DocCodeKey == null)
                {
                    cm.Parameters.AddWithValue("@DocCodeKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCodeKey", _DocCodeKey);
                }
                if (_DocID == null)
                {
                    cm.Parameters.AddWithValue("@DocID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocID", _DocID);
                }
                if (_DocDate == null)
                {
                    cm.Parameters.AddWithValue("@DocDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDate", _DocDate);
                }
                if (_DocType == null)
                {
                    cm.Parameters.AddWithValue("@DocType", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocType", _DocType);
                }
                if (_DocTypeNm == null)
                {
                    cm.Parameters.AddWithValue("@DocTypeNm", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTypeNm", _DocTypeNm);
                }
                if (_DocSign == null)
                {
                    cm.Parameters.AddWithValue("@DocSign", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSign", _DocSign);
                }
                if (_DocDeptKey == null)
                {
                    cm.Parameters.AddWithValue("@DocDeptKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDeptKey", _DocDeptKey);
                }
                if (_DocTranGrpKey == null)
                {
                    cm.Parameters.AddWithValue("@DocTranGrpKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTranGrpKey", _DocTranGrpKey);
                }
                if (_DocGrpKey == null)
                {
                    cm.Parameters.AddWithValue("@DocGrpKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocGrpKey", _DocGrpKey);
                }
                if (_DocEmKey == null)
                {
                    cm.Parameters.AddWithValue("@DocEmKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocEmKey", _DocEmKey);
                }
                if (_DocAccBKKey == null)
                {
                    cm.Parameters.AddWithValue("@DocAccBKKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccBKKey", _DocAccBKKey);
                }
                if (_DocAccGainKey == null)
                {
                    cm.Parameters.AddWithValue("@DocAccGainKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccGainKey", _DocAccGainKey);
                }
                if (_DocAccLossKey == null)
                {
                    cm.Parameters.AddWithValue("@DocAccLossKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccLossKey", _DocAccLossKey);
                }
                if (_DocRef == null)
                {
                    cm.Parameters.AddWithValue("@DocRef", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRef", _DocRef);
                }
                if (_DocDes == null)
                {
                    cm.Parameters.AddWithValue("@DocDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDes", _DocDes);
                }
                if (_DocRem == null)
                {
                    cm.Parameters.AddWithValue("@DocRem", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRem", _DocRem);
                }
                if (_DocCurrKey == null)
                {
                    cm.Parameters.AddWithValue("@DocCurrKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCurrKey", _DocCurrKey);
                }
                if (_DocRevalueRate == null)
                {
                    cm.Parameters.AddWithValue("@DocRevalueRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRevalueRate", _DocRevalueRate);
                }
                if (_DocAccAmtF == null)
                {
                    cm.Parameters.AddWithValue("@DocAccAmtF", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccAmtF", _DocAccAmtF);
                }
                if (_DocAccAmtHOld == null)
                {
                    cm.Parameters.AddWithValue("@DocAccAmtHOld", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccAmtHOld", _DocAccAmtHOld);
                }
                if (_DocAccAmtHNew == null)
                {
                    cm.Parameters.AddWithValue("@DocAccAmtHNew", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccAmtHNew", _DocAccAmtHNew);
                }
                if (_DocAccAmtGainLoss == null)
                {
                    cm.Parameters.AddWithValue("@DocAccAmtGainLoss", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccAmtGainLoss", _DocAccAmtGainLoss);
                }
                if (_DocCountryRate == null)
                {
                    cm.Parameters.AddWithValue("@DocCountryRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCountryRate", _DocCountryRate);
                }
                if (_DocStatus == null)
                {
                    cm.Parameters.AddWithValue("@DocStatus", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocStatus", _DocStatus);
                }
                if (_DocState == null)
                {
                    cm.Parameters.AddWithValue("@DocState", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocState", _DocState);
                }
                if (_DocPrinted == null)
                {
                    cm.Parameters.AddWithValue("@DocPrinted", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPrinted", _DocPrinted);
                }
                if (_ApproveUserKey == null)
                {
                    cm.Parameters.AddWithValue("@ApproveUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ApproveUserKey", _ApproveUserKey);
                }
                if (_ApproveDate == null)
                {
                    cm.Parameters.AddWithValue("@ApproveDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ApproveDate", _ApproveDate);
                }
                if (_DisapproveUserKey == null)
                {
                    cm.Parameters.AddWithValue("@DisapproveUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DisapproveUserKey", _DisapproveUserKey);
                }
                if (_DisapproveDate == null)
                {
                    cm.Parameters.AddWithValue("@DisapproveDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DisapproveDate", _DisapproveDate);
                }
                if (_DisapproveCount == null)
                {
                    cm.Parameters.AddWithValue("@DisapproveCount", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DisapproveCount", _DisapproveCount);
                }
                if (_DisapproveMsg == null)
                {
                    cm.Parameters.AddWithValue("@DisapproveMsg", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DisapproveMsg", _DisapproveMsg);
                }
                if (_Attachment == null)
                {
                    cm.Parameters.AddWithValue("@Attachment", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Attachment", _Attachment);
                }
                if (_BranchKey == null)
                {
                    cm.Parameters.AddWithValue("@BranchKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@BranchKey", _BranchKey);
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
                if (_PurgeKeep == null)
                {
                    cm.Parameters.AddWithValue("@PurgeKeep", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@PurgeKeep", _PurgeKeep);
                }
                if (_PurgeData == null)
                {
                    cm.Parameters.AddWithValue("@PurgeData", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@PurgeData", _PurgeData);
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
                if (_Custom4 == null)
                {
                    cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom4", _Custom4);
                }
                if (_Custom5 == null)
                {
                    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom5", _Custom5);
                }
                if (_DocAccBKID == null)
                {
                    cm.Parameters.AddWithValue("@DocAccBKID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccBKID", _DocAccBKID);
                }
                if (_DocAccBKDes == null)
                {
                    cm.Parameters.AddWithValue("@DocAccBKDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccBKDes", _DocAccBKDes);
                }
                if (_DocAccGainID == null)
                {
                    cm.Parameters.AddWithValue("@DocAccGainID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccGainID", _DocAccGainID);
                }
                if (_DocAccGainDes == null)
                {
                    cm.Parameters.AddWithValue("@DocAccGainDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccGainDes", _DocAccGainDes);
                }
                if (_DocAccLossID == null)
                {
                    cm.Parameters.AddWithValue("@DocAccLossID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccLossID", _DocAccLossID);
                }
                if (_DocAccLossDes == null)
                {
                    cm.Parameters.AddWithValue("@DocAccLossDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccLossDes", _DocAccLossDes);
                }


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
        #endregion Insert

        #region Data Access - Update

        internal bool Update(out string msgID)
        {
            bool retValue = false;
            msgID = "RecordUpdateFail";
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Update(cn, out msgID);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }
        internal bool Update(SqlConnection cn, out string msgID)
        {
            msgID = "RecordUpdateFail";
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "GLRV_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@MsgID", msgID);

                if (_DocKey == null)
                {
                    cm.Parameters.AddWithValue("@DocKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocKey", _DocKey);
                }
                if (_DocCodeKey == null)
                {
                    cm.Parameters.AddWithValue("@DocCodeKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCodeKey", _DocCodeKey);
                }
                if (_DocID == null)
                {
                    cm.Parameters.AddWithValue("@DocID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocID", _DocID);
                }
                if (_DocDate == null)
                {
                    cm.Parameters.AddWithValue("@DocDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDate", _DocDate);
                }
                if (_DocType == null)
                {
                    cm.Parameters.AddWithValue("@DocType", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocType", _DocType);
                }
                if (_DocTypeNm == null)
                {
                    cm.Parameters.AddWithValue("@DocTypeNm", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTypeNm", _DocTypeNm);
                }
                if (_DocSign == null)
                {
                    cm.Parameters.AddWithValue("@DocSign", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSign", _DocSign);
                }
                if (_DocDeptKey == null)
                {
                    cm.Parameters.AddWithValue("@DocDeptKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDeptKey", _DocDeptKey);
                }
                if (_DocTranGrpKey == null)
                {
                    cm.Parameters.AddWithValue("@DocTranGrpKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTranGrpKey", _DocTranGrpKey);
                }
                if (_DocGrpKey == null)
                {
                    cm.Parameters.AddWithValue("@DocGrpKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocGrpKey", _DocGrpKey);
                }
                if (_DocEmKey == null)
                {
                    cm.Parameters.AddWithValue("@DocEmKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocEmKey", _DocEmKey);
                }
                if (_DocAccBKKey == null)
                {
                    cm.Parameters.AddWithValue("@DocAccBKKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccBKKey", _DocAccBKKey);
                }
                if (_DocAccGainKey == null)
                {
                    cm.Parameters.AddWithValue("@DocAccGainKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccGainKey", _DocAccGainKey);
                }
                if (_DocAccLossKey == null)
                {
                    cm.Parameters.AddWithValue("@DocAccLossKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccLossKey", _DocAccLossKey);
                }
                if (_DocRef == null)
                {
                    cm.Parameters.AddWithValue("@DocRef", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRef", _DocRef);
                }
                if (_DocDes == null)
                {
                    cm.Parameters.AddWithValue("@DocDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDes", _DocDes);
                }
                if (_DocRem == null)
                {
                    cm.Parameters.AddWithValue("@DocRem", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRem", _DocRem);
                }
                if (_DocCurrKey == null)
                {
                    cm.Parameters.AddWithValue("@DocCurrKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCurrKey", _DocCurrKey);
                }
                if (_DocRevalueRate == null)
                {
                    cm.Parameters.AddWithValue("@DocRevalueRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRevalueRate", _DocRevalueRate);
                }
                if (_DocAccAmtF == null)
                {
                    cm.Parameters.AddWithValue("@DocAccAmtF", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccAmtF", _DocAccAmtF);
                }
                if (_DocAccAmtHOld == null)
                {
                    cm.Parameters.AddWithValue("@DocAccAmtHOld", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccAmtHOld", _DocAccAmtHOld);
                }
                if (_DocAccAmtHNew == null)
                {
                    cm.Parameters.AddWithValue("@DocAccAmtHNew", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccAmtHNew", _DocAccAmtHNew);
                }
                if (_DocAccAmtGainLoss == null)
                {
                    cm.Parameters.AddWithValue("@DocAccAmtGainLoss", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccAmtGainLoss", _DocAccAmtGainLoss);
                }
                if (_DocCountryRate == null)
                {
                    cm.Parameters.AddWithValue("@DocCountryRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCountryRate", _DocCountryRate);
                }
                if (_DocStatus == null)
                {
                    cm.Parameters.AddWithValue("@DocStatus", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocStatus", _DocStatus);
                }
                if (_DocState == null)
                {
                    cm.Parameters.AddWithValue("@DocState", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocState", _DocState);
                }
                if (_DocPrinted == null)
                {
                    cm.Parameters.AddWithValue("@DocPrinted", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPrinted", _DocPrinted);
                }
                if (_ApproveUserKey == null)
                {
                    cm.Parameters.AddWithValue("@ApproveUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ApproveUserKey", _ApproveUserKey);
                }
                if (_ApproveDate == null)
                {
                    cm.Parameters.AddWithValue("@ApproveDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ApproveDate", _ApproveDate);
                }
                if (_DisapproveUserKey == null)
                {
                    cm.Parameters.AddWithValue("@DisapproveUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DisapproveUserKey", _DisapproveUserKey);
                }
                if (_DisapproveDate == null)
                {
                    cm.Parameters.AddWithValue("@DisapproveDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DisapproveDate", _DisapproveDate);
                }
                if (_DisapproveCount == null)
                {
                    cm.Parameters.AddWithValue("@DisapproveCount", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DisapproveCount", _DisapproveCount);
                }
                if (_DisapproveMsg == null)
                {
                    cm.Parameters.AddWithValue("@DisapproveMsg", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DisapproveMsg", _DisapproveMsg);
                }
                if (_Attachment == null)
                {
                    cm.Parameters.AddWithValue("@Attachment", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Attachment", _Attachment);
                }
                if (_BranchKey == null)
                {
                    cm.Parameters.AddWithValue("@BranchKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@BranchKey", _BranchKey);
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
                if (_PurgeKeep == null)
                {
                    cm.Parameters.AddWithValue("@PurgeKeep", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@PurgeKeep", _PurgeKeep);
                }
                if (_PurgeData == null)
                {
                    cm.Parameters.AddWithValue("@PurgeData", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@PurgeData", _PurgeData);
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
                if (_Custom4 == null)
                {
                    cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom4", _Custom4);
                }
                if (_Custom5 == null)
                {
                    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom5", _Custom5);
                }
                if (_DocAccBKID == null)
                {
                    cm.Parameters.AddWithValue("@DocAccBKID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccBKID", _DocAccBKID);
                }
                if (_DocAccBKDes == null)
                {
                    cm.Parameters.AddWithValue("@DocAccBKDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccBKDes", _DocAccBKDes);
                }
                if (_DocAccGainID == null)
                {
                    cm.Parameters.AddWithValue("@DocAccGainID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccGainID", _DocAccGainID);
                }
                if (_DocAccGainDes == null)
                {
                    cm.Parameters.AddWithValue("@DocAccGainDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccGainDes", _DocAccGainDes);
                }
                if (_DocAccLossID == null)
                {
                    cm.Parameters.AddWithValue("@DocAccLossID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccLossID", _DocAccLossID);
                }
                if (_DocAccLossDes == null)
                {
                    cm.Parameters.AddWithValue("@DocAccLossDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccLossDes", _DocAccLossDes);
                }


                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();
                if (cm.Parameters["@MsgID"].Value == null)
                    msgID = string.Empty;
                else
                    msgID = cm.Parameters["@MsgID"].Value.ToString();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;

            }// Already close and dispose sql command.

        }
        #endregion Update

        #region Data Access - Delete

        internal bool Delete(Criteria criteria, out string msgID)
        {
            bool retValue = false;
            msgID = "RecordDeleteFail";
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Delete(cn, criteria, out msgID);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }
        internal bool Delete(SqlConnection cn, Criteria criteria, out string msgID)
        {
            bool retValue = false;
            msgID = "RecordDeleteFail";
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "GLRV_Delete";

                cm.Parameters.AddWithValue("@MsgID", msgID);
                cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();
                if (cm.Parameters["@MsgID"].Value == null)
                    msgID = string.Empty;
                else
                    msgID = cm.Parameters["@MsgID"].Value.ToString();

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

            string msgID = "DocID" + MsgID.Validation.DuplicateRecord;
            try
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "GLRV_Validation";

                    cm.Parameters.AddWithValue("@isNew", isNew);
                    cm.Parameters.AddWithValue("@DocCodeKey", criteria._DocCodeKey);
                    cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
                    cm.Parameters.AddWithValue("@DocID", criteria._DocID);
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

        internal void Clear()
        {
            this._DocKey = 0;
            this._DocDeptKey = 0;
            this._DocTranGrpKey = 0;
            this._DocGrpKey = 0;
            this._DocAccBKKey = 0;
            this._DocAccGainKey = 0;
            this._DocAccLossKey = 0;
            this._DocCurrKey = 0;
            this._DocRevalueRate = 0;
            this._DocAccAmtF = 0;
            this._DocAccAmtHOld = 0;
            this._DocAccAmtHNew = 0;
            this._DocAccAmtGainLoss = 0;
            this._DocCountryRate = 0;
            this._DocAccBKID = string.Empty;
            this._DocAccBKDes = string.Empty;
            this._DocAccGainID = string.Empty;
            this._DocAccGainDes = string.Empty;
            this._DocAccLossID = string.Empty;
            this._DocAccLossDes = string.Empty;

        }
    
    }
}





