


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
    /// Summary description for INMFN.
    /// </summary>
    [Serializable]
    public class INMFN : Document, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        internal DateTime _DocAllocateDate;
        internal int _DocDeptKey;
        internal int? _DocTranGrpKey;
        internal int? _DocAccChargesKey;
        internal int? _DocAccOHKey;
        internal int _DocAccRndKey;
        internal int? _DocAccLabourKey;
        internal int _DocGrpKey;
        internal decimal _DocLabourAmtH;
        internal string _DocChargesDes;
        internal decimal _DocChargesAmtH;
        internal decimal _DocFGTotalQty;
        internal decimal _DocFGTotalGram;
        internal int _DocFGVarCostMode;
        internal string _DocLabourDes;
        internal string _DocAccLabourID;
        internal string _DocAccLabourDes;
        internal string _DocAccChargesID;
        internal string _DocAccChargesDes;
        internal string _DocAccOHID;
        internal string _DocAccOHDes;
        internal string _DocAccRndID;
        internal string _DocAccRndDes;
        internal int _DocProDetails;
        internal SYSAttachments attachments;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public INMFN()
            : base()
        {
            this._DocAllocateDate = DateTime.Today.Date;
            this._DocDeptKey = 0;
            this._DocTranGrpKey = 0;
            this._DocAccChargesKey = null;
            this._DocAccOHKey = null;
            this._DocAccRndKey = 0;
            this._DocAccLabourKey = null;
            this._DocGrpKey = 0;
            this._DocLabourAmtH = 0;
            this._DocChargesDes = null;
            this._DocChargesAmtH = 0;
            this._DocFGTotalQty = 0;
            this._DocFGTotalGram = 0;
            this._DocFGVarCostMode = 0;
            this._DocLabourDes = null;
            this._DocAccLabourID = string.Empty;
            this._DocAccLabourDes = string.Empty;
            this._DocAccChargesID = string.Empty;
            this._DocAccChargesDes = string.Empty;
            this._DocAccOHID = string.Empty;
            this._DocAccOHDes = string.Empty;
            this._DocAccRndID = string.Empty;
            this._DocAccRndDes = string.Empty;
            this._DocProDetails= 10;
            this.attachments = new SYSAttachments();
            base.PropertyChanged += new PropertyChangedEventHandler(INMFN_PropertyChanged);
        }
        void INMFN_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }

        public INMFN Clone()
        {
            INMFN objCopy = (INMFN)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
        }
        public static INMFN Get(int? docKey)
        {
            INMFN child = new INMFN();
            child.Fetch(new Criteria(docKey, 1));
            return child;
        }

        public static INMFN New()
        {
            INMFN child = new INMFN();
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


        public DateTime DocAllocateDate
        {

            get
            {
                return this._DocAllocateDate;
            }
            set
            {
                this._DocAllocateDate = value;
                NotifyPropertyChanged("DocAllocateDate");
            }
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
        public int? DocAccChargesKey
        {

            get
            {
                return this._DocAccChargesKey;
            }
            set
            {
                this._DocAccChargesKey = value;
                NotifyPropertyChanged("DocAccChargesKey");
            }
        }
        public int? DocAccOHKey
        {

            get
            {
                return this._DocAccOHKey;
            }
            set
            {
                this._DocAccOHKey = value;
                NotifyPropertyChanged("DocAccOHKey");
            }
        }
        public int DocAccRndKey
        {

            get
            {
                return this._DocAccRndKey;
            }
            set
            {
                this._DocAccRndKey = value;
                NotifyPropertyChanged("DocAccRndKey");
            }
        }
        public int? DocAccLabourKey
        {

            get
            {
                return this._DocAccLabourKey;
            }
            set
            {
                this._DocAccLabourKey = value;
                NotifyPropertyChanged("DocAccLabourKey");
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
        public decimal DocLabourAmtH
        {

            get
            {
                return this._DocLabourAmtH;
            }
            set
            {
                this._DocLabourAmtH = value;
                NotifyPropertyChanged("DocLabourAmtH");
            }
        }
        public string DocChargesDes
        {

            get
            {
                return this._DocChargesDes;
            }
            set
            {
                this._DocChargesDes = value;
                NotifyPropertyChanged("DocChargesDes");
            }
        }
        public decimal DocChargesAmtH
        {

            get
            {
                return this._DocChargesAmtH;
            }
            set
            {
                this._DocChargesAmtH = value;
                NotifyPropertyChanged("DocChargesAmtH");
            }
        }
        public decimal DocFGTotalQty
        {

            get
            {
                return this._DocFGTotalQty;
            }
            set
            {
                this._DocFGTotalQty = value;
                NotifyPropertyChanged("DocFGTotalQty");
            }
        }
        public decimal DocFGTotalGram
        {

            get
            {
                return this._DocFGTotalGram;
            }
            set
            {
                this._DocFGTotalGram = value;
                NotifyPropertyChanged("DocFGTotalGram");
            }
        }
        public int DocFGVarCostMode
        {

            get
            {
                return this._DocFGVarCostMode;
            }
            set
            {
                this._DocFGVarCostMode = value;
                NotifyPropertyChanged("DocFGVarCostMode");
            }
        }
        public string DocLabourDes
        {

            get
            {
                return this._DocLabourDes;
            }
            set
            {
                this._DocLabourDes = value;
                NotifyPropertyChanged("DocLabourDes");
            }
        }
        public string DocAccLabourID
        {

            get
            {
                return this._DocAccLabourID;
            }
            set
            {
                this._DocAccLabourID = value;
                NotifyPropertyChanged("DocAccLabourID");
            }
        }
        public string DocAccLabourDes
        {

            get
            {
                return this._DocAccLabourDes;
            }
            set
            {
                this._DocAccLabourDes = value;
                NotifyPropertyChanged("DocAccLabourDes");
            }
        }
        public string DocAccChargesID
        {

            get
            {
                return this._DocAccChargesID;
            }
            set
            {
                this._DocAccChargesID = value;
                NotifyPropertyChanged("DocAccChargesID");
            }
        }
        public string DocAccChargesDes
        {

            get
            {
                return this._DocAccChargesDes;
            }
            set
            {
                this._DocAccChargesDes = value;
                NotifyPropertyChanged("DocAccChargesDes");
            }
        }
        public string DocAccOHID
        {

            get
            {
                return this._DocAccOHID;
            }
            set
            {
                this._DocAccOHID = value;
                NotifyPropertyChanged("DocAccOHID");
            }
        }
        public string DocAccOHDes
        {

            get
            {
                return this._DocAccOHDes;
            }
            set
            {
                this._DocAccOHDes = value;
                NotifyPropertyChanged("DocAccOHDes");
            }
        }
        public string DocAccRndID
        {

            get
            {
                return this._DocAccRndID;
            }
            set
            {
                this._DocAccRndID = value;
                NotifyPropertyChanged("DocAccRndID");
            }
        }
        public string DocAccRndDes
        {

            get
            {
                return this._DocAccRndDes;
            }
            set
            {
                this._DocAccRndDes = value;
                NotifyPropertyChanged("DocAccRndDes");
            }
        }
        public int DocProDetails
        {

            get
            {
                return this._DocProDetails;
            }
            set
            {
                this._DocProDetails = value;
            }
        }
        public SYSAttachments Attachments
        {
            get { return attachments; }
            set { attachments = value; }
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
                cm.CommandText = "INMFN_Get";

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
        internal static INMFN Get(IDataReader dr)
        {
            INMFN child = new INMFN();
            child.Fetch(dr);
            return child;
        }
        internal static INMFN Get(SqlConnection cn, Criteria criteria)
        {
            INMFN child = new INMFN();
            child.Fetch(cn, criteria);
            return child;
        }

        internal bool Fetch(IDataReader dataReader)
        {
            _DocKey = dataReader["DocKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocKey"];
            _DocCodeKey = dataReader["DocCodeKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocCodeKey"];
            _DocID = dataReader["DocID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocID"];
            _DocDate = dataReader["DocDate"] == DBNull.Value ? (DateTime)DateTime.Today.Date : (DateTime)dataReader["DocDate"];
            _DocAllocateDate = dataReader["DocAllocateDate"] == DBNull.Value ? (DateTime)DateTime.Today.Date : (DateTime)dataReader["DocAllocateDate"];
            _DocType = dataReader["DocType"] == DBNull.Value ? (int)0 : (int)dataReader["DocType"];
            _DocTypeNm = dataReader["DocTypeNm"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocTypeNm"];
            _DocSign = dataReader["DocSign"] == DBNull.Value ? (Int16)0 : (Int16)dataReader["DocSign"];
            _DocDeptKey = dataReader["DocDeptKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocDeptKey"];
            _DocTranGrpKey = dataReader["DocTranGrpKey"] == DBNull.Value ? (int?)0 : (int?)dataReader["DocTranGrpKey"];
            _DocAccChargesKey = dataReader["DocAccChargesKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocAccChargesKey"];
            _DocAccOHKey = dataReader["DocAccOHKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocAccOHKey"];
            _DocAccRndKey = dataReader["DocAccRndKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocAccRndKey"];
            _DocAccLabourKey = dataReader["DocAccLabourKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocAccLabourKey"];
            _DocGrpKey = dataReader["DocGrpKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocGrpKey"];
            _DocEmKey = dataReader["DocEmKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocEmKey"];
            _DocRef = dataReader["DocRef"] == DBNull.Value ? (string)null : (string)dataReader["DocRef"];
            _DocDes = dataReader["DocDes"] == DBNull.Value ? (string)null : (string)dataReader["DocDes"];
            _DocRem = dataReader["DocRem"] == DBNull.Value ? (string)null : (string)dataReader["DocRem"];
            _DocLabourAmtH = dataReader["DocLabourAmtH"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocLabourAmtH"];
            _DocChargesDes = dataReader["DocChargesDes"] == DBNull.Value ? (string)null : (string)dataReader["DocChargesDes"];
            _DocChargesAmtH = dataReader["DocChargesAmtH"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocChargesAmtH"];
            _DocFGTotalQty = dataReader["DocFGTotalQty"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocFGTotalQty"];
            _DocFGTotalGram = dataReader["DocFGTotalGram"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocFGTotalGram"];
            _DocFGVarCostMode = dataReader["DocFGVarCostMode"] == DBNull.Value ? (int)0 : (int)dataReader["DocFGVarCostMode"];
            _DocLabourDes = dataReader["DocLabourDes"] == DBNull.Value ? (string)null : (string)dataReader["DocLabourDes"];
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
            _DocAccLabourID = dataReader["DocAccLabourID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocAccLabourID"];
            _DocAccLabourDes = dataReader["DocAccLabourDes"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocAccLabourDes"];
            _DocAccChargesID = dataReader["DocAccChargesID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocAccChargesID"];
            _DocAccChargesDes = dataReader["DocAccChargesDes"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocAccChargesDes"];
            _DocAccOHID = dataReader["DocAccOHID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocAccOHID"];
            _DocAccOHDes = dataReader["DocAccOHDes"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocAccOHDes"];
            _DocAccRndID = dataReader["DocAccRndID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocAccRndID"];
            _DocAccRndDes = dataReader["DocAccRndDes"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocAccRndDes"];
            _DefLocKey = dataReader["DefLocKey"] == DBNull.Value ? (int?)0 : (int?)dataReader["DefLocKey"];

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
                cm.CommandText = "INMFN_AddUpdate";

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
                if (_DocAllocateDate == null)
                {
                    cm.Parameters.AddWithValue("@DocAllocateDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAllocateDate", _DocAllocateDate);
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
                if (_DocAccChargesKey == null)
                {
                    cm.Parameters.AddWithValue("@DocAccChargesKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccChargesKey", _DocAccChargesKey);
                }
                if (_DocAccOHKey == null)
                {
                    cm.Parameters.AddWithValue("@DocAccOHKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccOHKey", _DocAccOHKey);
                }
                if (_DocAccRndKey == null)
                {
                    cm.Parameters.AddWithValue("@DocAccRndKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccRndKey", _DocAccRndKey);
                }
                if (_DocAccLabourKey == null)
                {
                    cm.Parameters.AddWithValue("@DocAccLabourKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccLabourKey", _DocAccLabourKey);
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
                if (_DocLabourAmtH == null)
                {
                    cm.Parameters.AddWithValue("@DocLabourAmtH", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocLabourAmtH", _DocLabourAmtH);
                }
                if (_DocChargesDes == null)
                {
                    cm.Parameters.AddWithValue("@DocChargesDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocChargesDes", _DocChargesDes);
                }
                if (_DocChargesAmtH == null)
                {
                    cm.Parameters.AddWithValue("@DocChargesAmtH", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocChargesAmtH", _DocChargesAmtH);
                }
                if (_DocFGTotalQty == null)
                {
                    cm.Parameters.AddWithValue("@DocFGTotalQty", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFGTotalQty", _DocFGTotalQty);
                }
                if (_DocFGTotalGram == null)
                {
                    cm.Parameters.AddWithValue("@DocFGTotalGram", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFGTotalGram", _DocFGTotalGram);
                }
                if (_DocFGVarCostMode == null)
                {
                    cm.Parameters.AddWithValue("@DocFGVarCostMode", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFGVarCostMode", _DocFGVarCostMode);
                }
                if (_DocLabourDes == null)
                {
                    cm.Parameters.AddWithValue("@DocLabourDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocLabourDes", _DocLabourDes);
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
                if (_DocAccLabourID == null)
                {
                    cm.Parameters.AddWithValue("@DocAccLabourID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccLabourID", _DocAccLabourID);
                }
                if (_DocAccLabourDes == null)
                {
                    cm.Parameters.AddWithValue("@DocAccLabourDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccLabourDes", _DocAccLabourDes);
                }
                if (_DocAccChargesID == null)
                {
                    cm.Parameters.AddWithValue("@DocAccChargesID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccChargesID", _DocAccChargesID);
                }
                if (_DocAccChargesDes == null)
                {
                    cm.Parameters.AddWithValue("@DocAccChargesDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccChargesDes", _DocAccChargesDes);
                }
                if (_DocAccOHID == null)
                {
                    cm.Parameters.AddWithValue("@DocAccOHID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccOHID", _DocAccOHID);
                }
                if (_DocAccOHDes == null)
                {
                    cm.Parameters.AddWithValue("@DocAccOHDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccOHDes", _DocAccOHDes);
                }
                if (_DocAccRndID == null)
                {
                    cm.Parameters.AddWithValue("@DocAccRndID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccRndID", _DocAccRndID);
                }
                if (_DocAccRndDes == null)
                {
                    cm.Parameters.AddWithValue("@DocAccRndDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccRndDes", _DocAccRndDes);
                }
                if (_DefLocKey == null)
                {
                    cm.Parameters.AddWithValue("@DefLocKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DefLocKey", _DefLocKey);
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
                cm.CommandText = "INMFN_AddUpdate";

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
                if (_DocAllocateDate == null)
                {
                    cm.Parameters.AddWithValue("@DocAllocateDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAllocateDate", _DocAllocateDate);
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
                if (_DocAccChargesKey == null)
                {
                    cm.Parameters.AddWithValue("@DocAccChargesKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccChargesKey", _DocAccChargesKey);
                }
                if (_DocAccOHKey == null)
                {
                    cm.Parameters.AddWithValue("@DocAccOHKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccOHKey", _DocAccOHKey);
                }
                if (_DocAccRndKey == null)
                {
                    cm.Parameters.AddWithValue("@DocAccRndKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccRndKey", _DocAccRndKey);
                }
                if (_DocAccLabourKey == null)
                {
                    cm.Parameters.AddWithValue("@DocAccLabourKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccLabourKey", _DocAccLabourKey);
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
                if (_DocLabourAmtH == null)
                {
                    cm.Parameters.AddWithValue("@DocLabourAmtH", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocLabourAmtH", _DocLabourAmtH);
                }
                if (_DocChargesDes == null)
                {
                    cm.Parameters.AddWithValue("@DocChargesDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocChargesDes", _DocChargesDes);
                }
                if (_DocChargesAmtH == null)
                {
                    cm.Parameters.AddWithValue("@DocChargesAmtH", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocChargesAmtH", _DocChargesAmtH);
                }
                if (_DocFGTotalQty == null)
                {
                    cm.Parameters.AddWithValue("@DocFGTotalQty", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFGTotalQty", _DocFGTotalQty);
                }
                if (_DocFGTotalGram == null)
                {
                    cm.Parameters.AddWithValue("@DocFGTotalGram", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFGTotalGram", _DocFGTotalGram);
                }
                if (_DocFGVarCostMode == null)
                {
                    cm.Parameters.AddWithValue("@DocFGVarCostMode", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFGVarCostMode", _DocFGVarCostMode);
                }
                if (_DocLabourDes == null)
                {
                    cm.Parameters.AddWithValue("@DocLabourDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocLabourDes", _DocLabourDes);
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
                if (_DocAccLabourID == null)
                {
                    cm.Parameters.AddWithValue("@DocAccLabourID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccLabourID", _DocAccLabourID);
                }
                if (_DocAccLabourDes == null)
                {
                    cm.Parameters.AddWithValue("@DocAccLabourDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccLabourDes", _DocAccLabourDes);
                }
                if (_DocAccChargesID == null)
                {
                    cm.Parameters.AddWithValue("@DocAccChargesID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccChargesID", _DocAccChargesID);
                }
                if (_DocAccChargesDes == null)
                {
                    cm.Parameters.AddWithValue("@DocAccChargesDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccChargesDes", _DocAccChargesDes);
                }
                if (_DocAccOHID == null)
                {
                    cm.Parameters.AddWithValue("@DocAccOHID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccOHID", _DocAccOHID);
                }
                if (_DocAccOHDes == null)
                {
                    cm.Parameters.AddWithValue("@DocAccOHDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccOHDes", _DocAccOHDes);
                }
                if (_DocAccRndID == null)
                {
                    cm.Parameters.AddWithValue("@DocAccRndID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccRndID", _DocAccRndID);
                }
                if (_DocAccRndDes == null)
                {
                    cm.Parameters.AddWithValue("@DocAccRndDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccRndDes", _DocAccRndDes);
                }
                if (_DefLocKey == null)
                {
                    cm.Parameters.AddWithValue("@DefLocKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DefLocKey", _DefLocKey);
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
                cm.CommandText = "INMFN_Delete";

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
                    cm.CommandText = "INMFN_Validation";

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
            this._DocAllocateDate = DateTime.Today.Date;
            this._DocDeptKey = 0;
            this._DocTranGrpKey = 0;
            this._DocAccChargesKey = null;
            this._DocAccOHKey = null;
            this._DocAccRndKey = 0;
            this._DocAccLabourKey = null;
            this._DocGrpKey = 0;
            this._DocLabourAmtH = 0;
            this._DocChargesDes = null;
            this._DocChargesAmtH = 0;
            this._DocFGTotalQty = 0;
            this._DocFGTotalGram = 0;
            this._DocFGVarCostMode = 0;
            this._DocLabourDes = null;
            this._DocAccLabourID = string.Empty;
            this._DocAccLabourDes = string.Empty;
            this._DocAccChargesID = string.Empty;
            this._DocAccChargesDes = string.Empty;
            this._DocAccOHID = string.Empty;
            this._DocAccOHDes = string.Empty;
            this._DocAccRndID = string.Empty;
            this._DocAccRndDes = string.Empty;
            this._DocProDetails = 10;
            this.attachments = new SYSAttachments();
        }
    
    }
}





