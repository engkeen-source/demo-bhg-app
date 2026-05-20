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
	/// Summary description for APPY.
	/// </summary>
	[Serializable]
    public class APPY : Document,INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        internal DateTime? _DocDateOrg;
        internal int _DocConKey;
        internal string _DocConNm;
        internal string _DocConUEN;
        internal int _DocGrpKey;
        internal string _DocBAddrStreet;
        internal string _DocBAddrPOBox;
        internal string _DocBAddrCity;
        internal string _DocBAddrState;
        internal string _DocBAddrZipCode;
        internal string _DocBAddrCountry;
        internal string _DocBAddrRegion;
        internal string _DocBAddrAttn;
        internal string _DocBAddrTel1;
        internal string _DocBAddrTel2;
        internal string _DocBAddrFax;
        internal string _DocBAddrEmail;
        internal int? _DocDeptKey;
        internal int? _DocTranGrpKey;
        internal int? _DocAccKey;
        internal int? _DocPayModeKey;
        internal DateTime? _DocChqDate;
        internal string _DocChqNum;
        internal int? _DocBankKey;
        internal string _DocRemDelivery;
        internal string _DocRemPrice;
        internal string _DocRemValidity;
        internal string _DocRemPayment;
        internal string _DocPermitNum;
        internal string _DocGoodsDestination;
        internal string _DocCountryOrigin;
        internal string _DocRemAdditional1;
        internal string _DocRemAdditional2;
        internal string _DocRemAdditional3;
        internal string _DocRemAdditional4;
        internal decimal _DocSubTotal;
        internal int? _DocTaxGrpKey;
        internal decimal _DocTaxGrpRate;
        internal decimal _DocTaxTotal;
        internal decimal _DocGrand;
        internal int _DocCurrKey;
        internal decimal _DocCurrRate;
        internal decimal _DocHome;
        internal decimal _DocApplyAmtF;
        internal decimal _DocApplyAmtH;
        internal decimal _DocCountryRate;
        internal decimal _DocTaxTotalLocal;
        internal bool _DocApplyFull;
        internal bool _DocDeposit;
        internal string _DocConID;
        internal string _DocAccID;
        internal string _DocAccDes;
        internal string _Doc_PUR_Code; //added by thettm on 04 jun 2018
        internal bool _VerifyAsApprove;
        internal SYSAttachments attachments = new SYSAttachments();

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public APPY()
            : base()
        {
            this._DocDateOrg = DateTime.Today.Date;
            this._DocConKey = 0;
            this._DocConNm = string.Empty;
            this._DocConUEN = null;
            this._DocGrpKey = 0;
            this._DocBAddrStreet = null;
            this._DocBAddrPOBox = null;
            this._DocBAddrCity = null;
            this._DocBAddrState = null;
            this._DocBAddrZipCode = null;
            this._DocBAddrCountry = null;
            this._DocBAddrRegion = null;
            this._DocBAddrAttn = null;
            this._DocBAddrTel1 = null;
            this._DocBAddrTel2 = null;
            this._DocBAddrFax = null;
            this._DocBAddrEmail = null;
            this._DocDeptKey = 0;
            this._DocTranGrpKey = 0;
            this._DocAccKey = 0;
            this._DocPayModeKey = null;
            this._DocChqDate = DateTime.Today.Date;
            this._DocChqNum = null;
            this._DocBankKey = null;
            this._DocRemDelivery = null;
            this._DocRemPrice = null;
            this._DocRemValidity = null;
            this._DocRemPayment = null;
            this._DocPermitNum = null;
            this._DocGoodsDestination = null;
            this._DocCountryOrigin = null;
            this._DocRemAdditional1 = null;
            this._DocRemAdditional2 = null;
            this._DocRemAdditional3 = null;
            this._DocRemAdditional4 = null;
            this._DocSubTotal = 0;
            this._DocTaxGrpKey = null;
            this._DocTaxGrpRate = 0;
            this._DocTaxTotal = 0;
            this._DocGrand = 0;
            this._DocCurrKey = 0;
            this._DocCurrRate = 0;
            this._DocHome = 0;
            this._DocApplyAmtF = 0;
            this._DocCountryRate = 0;
            this._DocTaxTotalLocal = 0;
            this._DocApplyFull = false;
            this._DocDeposit = false;
            this._DocConID = string.Empty;
            this._DocAccID = string.Empty;
            this._DocAccDes = string.Empty;
            this._Doc_PUR_Code = string.Empty; //added by thettm on 04 jun 2018
            this._VerifyAsApprove = false;
            base.PropertyChanged += new PropertyChangedEventHandler(APPY_PropertyChanged);
        }

        void APPY_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }

        public APPY Clone()
        {
            APPY objCopy = (APPY)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
        }
        public static APPY Get(int? docKey)
        {
            APPY child = new APPY();
            child.Fetch(new Criteria(docKey, 1));
            return child;
        }

        public static APPY New()
        {
            APPY child = new APPY();
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


        public DateTime? DocDateOrg
        {

            get
            {
                return this._DocDateOrg;
            }
            set
            {
                this._DocDateOrg = value;
                NotifyPropertyChanged("DocDateOrg");
            }
        }
        public int DocConKey
        {

            get
            {
                return this._DocConKey;
            }
            set
            {
                this._DocConKey = value;
                NotifyPropertyChanged("DocConKey");
            }
        }
        public string DocConNm
        {

            get
            {
                return this._DocConNm;
            }
            set
            {
                this._DocConNm = value;
                NotifyPropertyChanged("DocConNm");
            }
        }
        public string DocConUEN
        {

            get
            {
                return this._DocConUEN;
            }
            set
            {
                this._DocConUEN = value;
                NotifyPropertyChanged("DocConUEN");
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
        public string DocBAddrStreet
        {

            get
            {
                return this._DocBAddrStreet;
            }
            set
            {
                this._DocBAddrStreet = value;
                NotifyPropertyChanged("DocBAddrStreet");
            }
        }
        public string DocBAddrPOBox
        {

            get
            {
                return this._DocBAddrPOBox;
            }
            set
            {
                this._DocBAddrPOBox = value;
                NotifyPropertyChanged("DocBAddrPOBox");
            }
        }
        public string DocBAddrCity
        {

            get
            {
                return this._DocBAddrCity;
            }
            set
            {
                this._DocBAddrCity = value;
                NotifyPropertyChanged("DocBAddrCity");
            }
        }
        public string DocBAddrState
        {

            get
            {
                return this._DocBAddrState;
            }
            set
            {
                this._DocBAddrState = value;
                NotifyPropertyChanged("DocBAddrState");
            }
        }
        public string DocBAddrZipCode
        {

            get
            {
                return this._DocBAddrZipCode;
            }
            set
            {
                this._DocBAddrZipCode = value;
                NotifyPropertyChanged("DocBAddrZipCode");
            }
        }
        public string DocBAddrCountry
        {

            get
            {
                return this._DocBAddrCountry;
            }
            set
            {
                this._DocBAddrCountry = value;
                NotifyPropertyChanged("DocBAddrCountry");
            }
        }
        public string DocBAddrRegion
        {

            get
            {
                return this._DocBAddrRegion;
            }
            set
            {
                this._DocBAddrRegion = value;
                NotifyPropertyChanged("DocBAddrRegion");
            }
        }
        public string DocBAddrAttn
        {

            get
            {
                return this._DocBAddrAttn;
            }
            set
            {
                this._DocBAddrAttn = value;
                NotifyPropertyChanged("DocBAddrAttn");
            }
        }
        public string DocBAddrTel1
        {

            get
            {
                return this._DocBAddrTel1;
            }
            set
            {
                this._DocBAddrTel1 = value;
                NotifyPropertyChanged("DocBAddrTel1");
            }
        }
        public string DocBAddrTel2
        {

            get
            {
                return this._DocBAddrTel2;
            }
            set
            {
                this._DocBAddrTel2 = value;
                NotifyPropertyChanged("DocBAddrTel2");
            }
        }
        public string DocBAddrFax
        {

            get
            {
                return this._DocBAddrFax;
            }
            set
            {
                this._DocBAddrFax = value;
                NotifyPropertyChanged("DocBAddrFax");
            }
        }
        public string DocBAddrEmail
        {

            get
            {
                return this._DocBAddrEmail;
            }
            set
            {
                this._DocBAddrEmail = value;
                NotifyPropertyChanged("DocBAddrEmail");
            }
        }
        public int? DocDeptKey
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
        public int? DocAccKey
        {

            get
            {
                return this._DocAccKey;
            }
            set
            {
                this._DocAccKey = value;
                NotifyPropertyChanged("DocAccKey");
            }
        }
        public int? DocPayModeKey
        {

            get
            {
                return this._DocPayModeKey;
            }
            set
            {
                this._DocPayModeKey = value;
                NotifyPropertyChanged("DocPayModeKey");
            }
        }
        public DateTime? DocChqDate
        {

            get
            {
                return this._DocChqDate;
            }
            set
            {
                this._DocChqDate = value;
                NotifyPropertyChanged("DocChqDate");
            }
        }
        public string DocChqNum
        {

            get
            {
                return this._DocChqNum;
            }
            set
            {
                this._DocChqNum = value;
                NotifyPropertyChanged("DocChqNum");
            }
        }
        public int? DocBankKey
        {

            get
            {
                return this._DocBankKey;
            }
            set
            {
                this._DocBankKey = value;
                NotifyPropertyChanged("DocBankKey");
            }
        }
        public string DocRemDelivery
        {

            get
            {
                return this._DocRemDelivery;
            }
            set
            {
                this._DocRemDelivery = value;
                NotifyPropertyChanged("DocRemDelivery");
            }
        }
        public string DocRemPrice
        {

            get
            {
                return this._DocRemPrice;
            }
            set
            {
                this._DocRemPrice = value;
                NotifyPropertyChanged("DocRemPrice");
            }
        }
        public string DocRemValidity
        {

            get
            {
                return this._DocRemValidity;
            }
            set
            {
                this._DocRemValidity = value;
                NotifyPropertyChanged("DocRemValidity");
            }
        }
        public string DocRemPayment
        {

            get
            {
                return this._DocRemPayment;
            }
            set
            {
                this._DocRemPayment = value;
                NotifyPropertyChanged("DocRemPayment");
            }
        }
        public string DocPermitNum
        {

            get
            {
                return this._DocPermitNum;
            }
            set
            {
                this._DocPermitNum = value;
                NotifyPropertyChanged("DocPermitNum");
            }
        }
        public string DocGoodsDestination
        {

            get
            {
                return this._DocGoodsDestination;
            }
            set
            {
                this._DocGoodsDestination = value;
                NotifyPropertyChanged("DocGoodsDestination");
            }
        }
        public string DocCountryOrigin
        {

            get
            {
                return this._DocCountryOrigin;
            }
            set
            {
                this._DocCountryOrigin = value;
                NotifyPropertyChanged("DocCountryOrigin");
            }
        }
        public string DocRemAdditional1
        {

            get
            {
                return this._DocRemAdditional1;
            }
            set
            {
                this._DocRemAdditional1 = value;
                NotifyPropertyChanged("DocRemAdditional1");
            }
        }
        public string DocRemAdditional2
        {

            get
            {
                return this._DocRemAdditional2;
            }
            set
            {
                this._DocRemAdditional2 = value;
                NotifyPropertyChanged("DocRemAdditional2");
            }
        }
        public string DocRemAdditional3
        {

            get
            {
                return this._DocRemAdditional3;
            }
            set
            {
                this._DocRemAdditional3 = value;
                NotifyPropertyChanged("DocRemAdditional3");
            }
        }
        public string DocRemAdditional4
        {

            get
            {
                return this._DocRemAdditional4;
            }
            set
            {
                this._DocRemAdditional4 = value;
                NotifyPropertyChanged("DocRemAdditional4");
            }
        }
        public decimal DocSubTotal
        {

            get
            {
                return this._DocSubTotal;
            }
            set
            {
                this._DocSubTotal = value;
                NotifyPropertyChanged("DocSubTotal");
            }
        }
        public int? DocTaxGrpKey
        {

            get
            {
                return this._DocTaxGrpKey;
            }
            set
            {
                this._DocTaxGrpKey = value;
                NotifyPropertyChanged("DocTaxGrpKey");
            }
        }
        public decimal DocTaxGrpRate
        {

            get
            {
                return this._DocTaxGrpRate;
            }
            set
            {
                this._DocTaxGrpRate = value;
                NotifyPropertyChanged("DocTaxGrpRate");
            }
        }
        public decimal DocTaxTotal
        {

            get
            {
                return this._DocTaxTotal;
            }
            set
            {
                this._DocTaxTotal = value;
                NotifyPropertyChanged("DocTaxTotal");
            }
        }
        public decimal DocGrand
        {

            get
            {
                return this._DocGrand;
            }
            set
            {
                this._DocGrand = value;
                NotifyPropertyChanged("DocGrand");
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
        public decimal DocCurrRate
        {

            get
            {
                return this._DocCurrRate;
            }
            set
            {
                this._DocCurrRate = value;
                NotifyPropertyChanged("DocCurrRate");
            }
        }
        public decimal DocHome
        {

            get
            {
                return this._DocHome;
            }
            set
            {
                this._DocHome = value;
                NotifyPropertyChanged("DocHome");
            }
        }
        public decimal DocApplyAmtF
        {

            get
            {
                return this._DocApplyAmtF;
            }
            set
            {
                this._DocApplyAmtF = value;
                NotifyPropertyChanged("DocApplyAmtF");
            }
        }
        public decimal DocApplyAmtH
        {

            get
            {
                return this._DocApplyAmtH;
            }
            set
            {
                this._DocApplyAmtH = value;
                NotifyPropertyChanged("DocApplyAmtH");
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
        public decimal DocTaxTotalLocal
        {

            get
            {
                return this._DocTaxTotalLocal;
            }
            set
            {
                this._DocTaxTotalLocal = value;
                NotifyPropertyChanged("DocTaxTotalLocal");
            }
        }
        public bool DocApplyFull
        {

            get
            {
                return this._DocApplyFull;
            }
            set
            {
                this._DocApplyFull = value;
                NotifyPropertyChanged("DocApplyFull");
            }
        }
        public bool DocDeposit
        {

            get
            {
                return this._DocDeposit;
            }
            set
            {
                this._DocDeposit = value;
                NotifyPropertyChanged("DocDeposit");
            }
        }
        public string DocConID
        {

            get
            {
                return this._DocConID;
            }
            set
            {
                this._DocConID = value;
                NotifyPropertyChanged("DocConID");
            }
        }
        public string DocAccID
        {

            get
            {
                return this._DocAccID;
            }
            set
            {
                this._DocAccID = value;
                NotifyPropertyChanged("DocAccID");
            }
        }
        public string DocAccDes
        {

            get
            {
                return this._DocAccDes;
            }
            set
            {
                this._DocAccDes = value;
                NotifyPropertyChanged("DocAccDes");
            }
        }
        //added by thettm on 04 jun 2018 (start)
        public string Doc_PUR_Code
        {

            get
            {
                return this._Doc_PUR_Code;
            }
            set
            {
                this._Doc_PUR_Code = value;
                NotifyPropertyChanged("Doc_PUR_Code");
            }
        }

        public bool VerifyAsApprove
        {

            get
            {
                return this._VerifyAsApprove;
            }
            set
            {
                this._VerifyAsApprove = value;
                NotifyPropertyChanged("VerifyAsApprove");
            }
        }

        //added by thettm on 04 jun 2018 (start)
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
            internal Criteria(int? CodeKey, string DocID, int? Option)
            {
                _DocCodeKey = CodeKey;
                _DocID = DocID;
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
                cm.CommandText = "APPY_Get";

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
        internal static APPY Get(IDataReader dr)
        {
            APPY child = new APPY();
            child.Fetch(dr);
            return child;
        }
        internal static APPY Get(SqlConnection cn, Criteria criteria)
        {
            APPY child = new APPY();
            child.Fetch(cn, criteria);
            return child;
        }

        internal bool Fetch(IDataReader dataReader)
        {
            _DocKey = dataReader["DocKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocKey"];
            _DocCodeKey = dataReader["DocCodeKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocCodeKey"];
            _DocID = dataReader["DocID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocID"];
            _DocDate = dataReader["DocDate"] == DBNull.Value ? (DateTime)DateTime.Today.Date : (DateTime)dataReader["DocDate"];
            _DocDateOrg = dataReader["DocDateOrg"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["DocDateOrg"];
            _DocType = dataReader["DocType"] == DBNull.Value ? (int)0 : (int)dataReader["DocType"];
            _DocTypeNm = dataReader["DocTypeNm"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocTypeNm"];
            _DocSign = dataReader["DocSign"] == DBNull.Value ? (Int16)0 : (Int16)dataReader["DocSign"];
            _DocConKey = dataReader["DocConKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocConKey"];
            _DocConNm = dataReader["DocConNm"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocConNm"];
            _DocConUEN = dataReader["DocConUEN"] == DBNull.Value ? (string)null : (string)dataReader["DocConUEN"];
            _DocGrpKey = dataReader["DocGrpKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocGrpKey"];
            _DocEmKey = dataReader["DocEmKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocEmKey"];
            _DocBAddrStreet = dataReader["DocBAddrStreet"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrStreet"];
            _DocBAddrPOBox = dataReader["DocBAddrPOBox"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrPOBox"];
            _DocBAddrCity = dataReader["DocBAddrCity"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrCity"];
            _DocBAddrState = dataReader["DocBAddrState"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrState"];
            _DocBAddrZipCode = dataReader["DocBAddrZipCode"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrZipCode"];
            _DocBAddrCountry = dataReader["DocBAddrCountry"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrCountry"];
            _DocBAddrRegion = dataReader["DocBAddrRegion"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrRegion"];
            _DocBAddrAttn = dataReader["DocBAddrAttn"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrAttn"];
            _DocBAddrTel1 = dataReader["DocBAddrTel1"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrTel1"];
            _DocBAddrTel2 = dataReader["DocBAddrTel2"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrTel2"];
            _DocBAddrFax = dataReader["DocBAddrFax"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrFax"];
            _DocBAddrEmail = dataReader["DocBAddrEmail"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrEmail"];
            _DocDeptKey = dataReader["DocDeptKey"] == DBNull.Value ? (int?)0 : (int?)dataReader["DocDeptKey"];
            _DocTranGrpKey = dataReader["DocTranGrpKey"] == DBNull.Value ? (int?)0 : (int?)dataReader["DocTranGrpKey"];
            _DocAccKey = dataReader["DocAccKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocAccKey"];
            _DocPayModeKey = dataReader["DocPayModeKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocPayModeKey"];
            _DocChqDate = dataReader["DocChqDate"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["DocChqDate"];
            _DocChqNum = dataReader["DocChqNum"] == DBNull.Value ? (string)null : (string)dataReader["DocChqNum"];
            _DocBankKey = dataReader["DocBankKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocBankKey"];
            _DocRef = dataReader["DocRef"] == DBNull.Value ? (string)null : (string)dataReader["DocRef"];
            _DocDes = dataReader["DocDes"] == DBNull.Value ? (string)null : (string)dataReader["DocDes"];
            _DocRem = dataReader["DocRem"] == DBNull.Value ? (string)null : (string)dataReader["DocRem"];
            _DocRemDelivery = dataReader["DocRemDelivery"] == DBNull.Value ? (string)null : (string)dataReader["DocRemDelivery"];
            _DocRemPrice = dataReader["DocRemPrice"] == DBNull.Value ? (string)null : (string)dataReader["DocRemPrice"];
            _DocRemValidity = dataReader["DocRemValidity"] == DBNull.Value ? (string)null : (string)dataReader["DocRemValidity"];
            _DocRemPayment = dataReader["DocRemPayment"] == DBNull.Value ? (string)null : (string)dataReader["DocRemPayment"];
            _DocPermitNum = dataReader["DocPermitNum"] == DBNull.Value ? (string)null : (string)dataReader["DocPermitNum"];
            _DocGoodsDestination = dataReader["DocGoodsDestination"] == DBNull.Value ? (string)null : (string)dataReader["DocGoodsDestination"];
            _DocCountryOrigin = dataReader["DocCountryOrigin"] == DBNull.Value ? (string)null : (string)dataReader["DocCountryOrigin"];
            _DocRemAdditional1 = dataReader["DocRemAdditional1"] == DBNull.Value ? (string)null : (string)dataReader["DocRemAdditional1"];
            _DocRemAdditional2 = dataReader["DocRemAdditional2"] == DBNull.Value ? (string)null : (string)dataReader["DocRemAdditional2"];
            _DocRemAdditional3 = dataReader["DocRemAdditional3"] == DBNull.Value ? (string)null : (string)dataReader["DocRemAdditional3"];
            _DocRemAdditional4 = dataReader["DocRemAdditional4"] == DBNull.Value ? (string)null : (string)dataReader["DocRemAdditional4"];
            _DocSubTotal = dataReader["DocSubTotal"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocSubTotal"];
            _DocTaxGrpKey = dataReader["DocTaxGrpKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocTaxGrpKey"];
            _DocTaxGrpRate = dataReader["DocTaxGrpRate"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocTaxGrpRate"];
            _DocTaxTotal = dataReader["DocTaxTotal"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocTaxTotal"];
            _DocGrand = dataReader["DocGrand"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocGrand"];
            _DocCurrKey = dataReader["DocCurrKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocCurrKey"];
            _DocCurrRate = dataReader["DocCurrRate"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocCurrRate"];
            _DocHome = dataReader["DocHome"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocHome"];
            _DocApplyAmtF = dataReader["DocApplyAmtF"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocApplyAmtF"];
            _DocApplyAmtH = dataReader["DocApplyAmtH"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocApplyAmtH"];
            _DocCountryRate = dataReader["DocCountryRate"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocCountryRate"];
            _DocTaxTotalLocal = dataReader["DocTaxTotalLocal"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocTaxTotalLocal"];
            _DocApplyFull = dataReader["DocApplyFull"] == DBNull.Value ? (bool)false : (bool)dataReader["DocApplyFull"];
            _DocDeposit = dataReader["DocDeposit"] == DBNull.Value ? (bool)false : (bool)dataReader["DocDeposit"];
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
            _DefBAddrKey = dataReader["DefBAddrKey"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DefBAddrKey"];
            _DefSAddrKey = dataReader["DefSAddrKey"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DefSAddrKey"];
            _DocConID = dataReader["ConID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["ConID"];
            _DocAccID = dataReader["DocAccID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocAccID"];
            _DocAccDes = dataReader["DocAccDes"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocAccDes"];

            //added by thettm on 04 jun 2018
            _Doc_PUR_Code = dataReader["Doc_PUR_Code"] == DBNull.Value ? (string)string.Empty : (string)dataReader["Doc_PUR_Code"];
            if (dataReader.GetSchemaTable().Select("ColumnName = 'VerifyAsApprove'").Length == 1)
                _VerifyAsApprove = dataReader["VerifyAsApprove"] == DBNull.Value ? (bool)false : (bool)dataReader["VerifyAsApprove"];
            else
                _VerifyAsApprove = false;
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
           
            DocKey = 0;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "APPY_AddUpdate";

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
                if (_DocDateOrg == null)
                {
                    cm.Parameters.AddWithValue("@DocDateOrg", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDateOrg", _DocDateOrg);
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
                if (_DocConKey == null)
                {
                    cm.Parameters.AddWithValue("@DocConKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocConKey", _DocConKey);
                }
                if (_DocConNm == null)
                {
                    cm.Parameters.AddWithValue("@DocConNm", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocConNm", _DocConNm);
                }
                if (_DocConUEN == null)
                {
                    cm.Parameters.AddWithValue("@DocConUEN", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocConUEN", _DocConUEN);
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
                if (_DocBAddrStreet == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrStreet", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrStreet", _DocBAddrStreet);
                }
                if (_DocBAddrPOBox == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrPOBox", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrPOBox", _DocBAddrPOBox);
                }
                if (_DocBAddrCity == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrCity", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrCity", _DocBAddrCity);
                }
                if (_DocBAddrState == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrState", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrState", _DocBAddrState);
                }
                if (_DocBAddrZipCode == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrZipCode", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrZipCode", _DocBAddrZipCode);
                }
                if (_DocBAddrCountry == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrCountry", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrCountry", _DocBAddrCountry);
                }
                if (_DocBAddrRegion == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrRegion", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrRegion", _DocBAddrRegion);
                }
                if (_DocBAddrAttn == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrAttn", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrAttn", _DocBAddrAttn);
                }
                if (_DocBAddrTel1 == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrTel1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrTel1", _DocBAddrTel1);
                }
                if (_DocBAddrTel2 == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrTel2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrTel2", _DocBAddrTel2);
                }
                if (_DocBAddrFax == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrFax", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrFax", _DocBAddrFax);
                }
                if (_DocBAddrEmail == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrEmail", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrEmail", _DocBAddrEmail);
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
                if (_DocAccKey == null)
                {
                    cm.Parameters.AddWithValue("@DocAccKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccKey", _DocAccKey);
                }
                if (_DocPayModeKey == null)
                {
                    cm.Parameters.AddWithValue("@DocPayModeKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPayModeKey", _DocPayModeKey);
                }
                if (_DocChqDate == null)
                {
                    cm.Parameters.AddWithValue("@DocChqDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocChqDate", _DocChqDate);
                }
                if (_DocChqNum == null)
                {
                    cm.Parameters.AddWithValue("@DocChqNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocChqNum", _DocChqNum);
                }
                if (_DocBankKey == null)
                {
                    cm.Parameters.AddWithValue("@DocBankKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBankKey", _DocBankKey);
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
                if (_DocRemDelivery == null)
                {
                    cm.Parameters.AddWithValue("@DocRemDelivery", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRemDelivery", _DocRemDelivery);
                }
                if (_DocRemPrice == null)
                {
                    cm.Parameters.AddWithValue("@DocRemPrice", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRemPrice", _DocRemPrice);
                }
                if (_DocRemValidity == null)
                {
                    cm.Parameters.AddWithValue("@DocRemValidity", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRemValidity", _DocRemValidity);
                }
                if (_DocRemPayment == null)
                {
                    cm.Parameters.AddWithValue("@DocRemPayment", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRemPayment", _DocRemPayment);
                }
                if (_DocPermitNum == null)
                {
                    cm.Parameters.AddWithValue("@DocPermitNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPermitNum", _DocPermitNum);
                }
                if (_DocGoodsDestination == null)
                {
                    cm.Parameters.AddWithValue("@DocGoodsDestination", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocGoodsDestination", _DocGoodsDestination);
                }
                if (_DocCountryOrigin == null)
                {
                    cm.Parameters.AddWithValue("@DocCountryOrigin", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCountryOrigin", _DocCountryOrigin);
                }
                if (_DocRemAdditional1 == null)
                {
                    cm.Parameters.AddWithValue("@DocRemAdditional1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRemAdditional1", _DocRemAdditional1);
                }
                if (_DocRemAdditional2 == null)
                {
                    cm.Parameters.AddWithValue("@DocRemAdditional2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRemAdditional2", _DocRemAdditional2);
                }
                if (_DocRemAdditional3 == null)
                {
                    cm.Parameters.AddWithValue("@DocRemAdditional3", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRemAdditional3", _DocRemAdditional3);
                }
                if (_DocRemAdditional4 == null)
                {
                    cm.Parameters.AddWithValue("@DocRemAdditional4", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRemAdditional4", _DocRemAdditional4);
                }
                if (_DocSubTotal == null)
                {
                    cm.Parameters.AddWithValue("@DocSubTotal", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSubTotal", _DocSubTotal);
                }
                if (_DocTaxGrpKey == null)
                {
                    cm.Parameters.AddWithValue("@DocTaxGrpKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTaxGrpKey", _DocTaxGrpKey);
                }
                if (_DocTaxGrpRate == null)
                {
                    cm.Parameters.AddWithValue("@DocTaxGrpRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTaxGrpRate", _DocTaxGrpRate);
                }
                if (_DocTaxTotal == null)
                {
                    cm.Parameters.AddWithValue("@DocTaxTotal", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTaxTotal", _DocTaxTotal);
                }
                if (_DocGrand == null)
                {
                    cm.Parameters.AddWithValue("@DocGrand", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocGrand", _DocGrand);
                }
                if (_DocCurrKey == null)
                {
                    cm.Parameters.AddWithValue("@DocCurrKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCurrKey", _DocCurrKey);
                }
                if (_DocCurrRate == null)
                {
                    cm.Parameters.AddWithValue("@DocCurrRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCurrRate", _DocCurrRate);
                }
                if (_DocHome == null)
                {
                    cm.Parameters.AddWithValue("@DocHome", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHome", _DocHome);
                }
                if (_DocApplyAmtF == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyAmtF", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyAmtF", _DocApplyAmtF);
                }
                if (_DocApplyAmtH == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyAmtH", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyAmtH", _DocApplyAmtH);
                }
                if (_DocCountryRate == null)
                {
                    cm.Parameters.AddWithValue("@DocCountryRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCountryRate", _DocCountryRate);
                }
                if (_DocTaxTotalLocal == null)
                {
                    cm.Parameters.AddWithValue("@DocTaxTotalLocal", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTaxTotalLocal", _DocTaxTotalLocal);
                }
                if (_DocApplyFull == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyFull", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyFull", _DocApplyFull);
                }
                if (_DocDeposit == null)
                {
                    cm.Parameters.AddWithValue("@DocDeposit", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDeposit", _DocDeposit);
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
                if (_DefBAddrKey == null)
                {
                    cm.Parameters.AddWithValue("@DefBAddrKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DefBAddrKey", _DefBAddrKey);
                }
                if (_DefSAddrKey == null)
                {
                    cm.Parameters.AddWithValue("@DefSAddrKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DefSAddrKey", _DefSAddrKey);
                }
                if (_DocConID == null)
                {
                    cm.Parameters.AddWithValue("@ConID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ConID", _DocConID);
                }
                if (_DocAccID == null)
                {
                    cm.Parameters.AddWithValue("@DocAccID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccID", _DocAccID);
                }
                if (_DocAccDes == null)
                {
                    cm.Parameters.AddWithValue("@DocAccDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccDes", _DocAccDes);
                }
               
                cm.Parameters.AddWithValue("@VerifyAsApprove", _VerifyAsApprove);
               

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
                cm.CommandText = "APPY_AddUpdate";

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
                if (_DocDateOrg == null)
                {
                    cm.Parameters.AddWithValue("@DocDateOrg", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDateOrg", _DocDateOrg);
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
                if (_DocConKey == null)
                {
                    cm.Parameters.AddWithValue("@DocConKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocConKey", _DocConKey);
                }
                if (_DocConNm == null)
                {
                    cm.Parameters.AddWithValue("@DocConNm", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocConNm", _DocConNm);
                }
                if (_DocConUEN == null)
                {
                    cm.Parameters.AddWithValue("@DocConUEN", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocConUEN", _DocConUEN);
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
                if (_DocBAddrStreet == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrStreet", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrStreet", _DocBAddrStreet);
                }
                if (_DocBAddrPOBox == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrPOBox", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrPOBox", _DocBAddrPOBox);
                }
                if (_DocBAddrCity == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrCity", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrCity", _DocBAddrCity);
                }
                if (_DocBAddrState == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrState", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrState", _DocBAddrState);
                }
                if (_DocBAddrZipCode == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrZipCode", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrZipCode", _DocBAddrZipCode);
                }
                if (_DocBAddrCountry == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrCountry", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrCountry", _DocBAddrCountry);
                }
                if (_DocBAddrRegion == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrRegion", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrRegion", _DocBAddrRegion);
                }
                if (_DocBAddrAttn == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrAttn", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrAttn", _DocBAddrAttn);
                }
                if (_DocBAddrTel1 == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrTel1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrTel1", _DocBAddrTel1);
                }
                if (_DocBAddrTel2 == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrTel2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrTel2", _DocBAddrTel2);
                }
                if (_DocBAddrFax == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrFax", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrFax", _DocBAddrFax);
                }
                if (_DocBAddrEmail == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrEmail", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrEmail", _DocBAddrEmail);
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
                if (_DocAccKey == null)
                {
                    cm.Parameters.AddWithValue("@DocAccKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccKey", _DocAccKey);
                }
                if (_DocPayModeKey == null)
                {
                    cm.Parameters.AddWithValue("@DocPayModeKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPayModeKey", _DocPayModeKey);
                }
                if (_DocChqDate == null)
                {
                    cm.Parameters.AddWithValue("@DocChqDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocChqDate", _DocChqDate);
                }
                if (_DocChqNum == null)
                {
                    cm.Parameters.AddWithValue("@DocChqNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocChqNum", _DocChqNum);
                }
                if (_DocBankKey == null)
                {
                    cm.Parameters.AddWithValue("@DocBankKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBankKey", _DocBankKey);
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
                if (_DocRemDelivery == null)
                {
                    cm.Parameters.AddWithValue("@DocRemDelivery", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRemDelivery", _DocRemDelivery);
                }
                if (_DocRemPrice == null)
                {
                    cm.Parameters.AddWithValue("@DocRemPrice", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRemPrice", _DocRemPrice);
                }
                if (_DocRemValidity == null)
                {
                    cm.Parameters.AddWithValue("@DocRemValidity", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRemValidity", _DocRemValidity);
                }
                if (_DocRemPayment == null)
                {
                    cm.Parameters.AddWithValue("@DocRemPayment", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRemPayment", _DocRemPayment);
                }
                if (_DocPermitNum == null)
                {
                    cm.Parameters.AddWithValue("@DocPermitNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPermitNum", _DocPermitNum);
                }
                if (_DocGoodsDestination == null)
                {
                    cm.Parameters.AddWithValue("@DocGoodsDestination", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocGoodsDestination", _DocGoodsDestination);
                }
                if (_DocCountryOrigin == null)
                {
                    cm.Parameters.AddWithValue("@DocCountryOrigin", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCountryOrigin", _DocCountryOrigin);
                }
                if (_DocRemAdditional1 == null)
                {
                    cm.Parameters.AddWithValue("@DocRemAdditional1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRemAdditional1", _DocRemAdditional1);
                }
                if (_DocRemAdditional2 == null)
                {
                    cm.Parameters.AddWithValue("@DocRemAdditional2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRemAdditional2", _DocRemAdditional2);
                }
                if (_DocRemAdditional3 == null)
                {
                    cm.Parameters.AddWithValue("@DocRemAdditional3", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRemAdditional3", _DocRemAdditional3);
                }
                if (_DocRemAdditional4 == null)
                {
                    cm.Parameters.AddWithValue("@DocRemAdditional4", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRemAdditional4", _DocRemAdditional4);
                }
                if (_DocSubTotal == null)
                {
                    cm.Parameters.AddWithValue("@DocSubTotal", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSubTotal", _DocSubTotal);
                }
                if (_DocTaxGrpKey == null)
                {
                    cm.Parameters.AddWithValue("@DocTaxGrpKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTaxGrpKey", _DocTaxGrpKey);
                }
                if (_DocTaxGrpRate == null)
                {
                    cm.Parameters.AddWithValue("@DocTaxGrpRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTaxGrpRate", _DocTaxGrpRate);
                }
                if (_DocTaxTotal == null)
                {
                    cm.Parameters.AddWithValue("@DocTaxTotal", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTaxTotal", _DocTaxTotal);
                }
                if (_DocGrand == null)
                {
                    cm.Parameters.AddWithValue("@DocGrand", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocGrand", _DocGrand);
                }
                if (_DocCurrKey == null)
                {
                    cm.Parameters.AddWithValue("@DocCurrKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCurrKey", _DocCurrKey);
                }
                if (_DocCurrRate == null)
                {
                    cm.Parameters.AddWithValue("@DocCurrRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCurrRate", _DocCurrRate);
                }
                if (_DocHome == null)
                {
                    cm.Parameters.AddWithValue("@DocHome", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHome", _DocHome);
                }
                if (_DocApplyAmtF == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyAmtF", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyAmtF", _DocApplyAmtF);
                }
                if (_DocApplyAmtH == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyAmtH", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyAmtH", _DocApplyAmtH);
                }
                if (_DocCountryRate == null)
                {
                    cm.Parameters.AddWithValue("@DocCountryRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCountryRate", _DocCountryRate);
                }
                if (_DocTaxTotalLocal == null)
                {
                    cm.Parameters.AddWithValue("@DocTaxTotalLocal", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTaxTotalLocal", _DocTaxTotalLocal);
                }
                if (_DocApplyFull == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyFull", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyFull", _DocApplyFull);
                }
                if (_DocDeposit == null)
                {
                    cm.Parameters.AddWithValue("@DocDeposit", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDeposit", _DocDeposit);
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
                if (_DefBAddrKey == null)
                {
                    cm.Parameters.AddWithValue("@DefBAddrKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DefBAddrKey", _DefBAddrKey);
                }
                if (_DefSAddrKey == null)
                {
                    cm.Parameters.AddWithValue("@DefSAddrKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DefSAddrKey", _DefSAddrKey);
                }
                if (_DocConID == null)
                {
                    cm.Parameters.AddWithValue("@ConID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ConID", _DocConID);
                }
                if (_DocAccID == null)
                {
                    cm.Parameters.AddWithValue("@DocAccID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccID", _DocAccID);
                }
                if (_DocAccDes == null)
                {
                    cm.Parameters.AddWithValue("@DocAccDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAccDes", _DocAccDes);
                }

                cm.Parameters.AddWithValue("@VerifyAsApprove", _VerifyAsApprove);

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

        #region Update Doc Status
        internal bool UpdateDocStatus()
        {
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                cn.Open();
            
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "Doc_APPY_UpdateDocStatus";

                    cm.Parameters.AddWithValue("@DocKey", _DocKey);
                    cm.Parameters.AddWithValue("@UserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@DocStatus", _DocStatus);
                    // Execute command.
                    cm.ExecuteNonQuery();              
                }// Already close and dispose sql command.
            }
            return true;
        }
        #endregion

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
                cm.CommandText = "APPY_Delete";

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
            try
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "APPY_Validation";

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

        private void Clear()
        {
            this._DocKey = 0;
            this._DocDateOrg = DateTime.Today.Date;
            this._DocConKey = 0;
            this._DocConNm = string.Empty;
            this._DocConUEN = null;
            this._DocGrpKey = 0;
            this._DocBAddrStreet = null;
            this._DocBAddrPOBox = null;
            this._DocBAddrCity = null;
            this._DocBAddrState = null;
            this._DocBAddrZipCode = null;
            this._DocBAddrCountry = null;
            this._DocBAddrRegion = null;
            this._DocBAddrAttn = null;
            this._DocBAddrTel1 = null;
            this._DocBAddrTel2 = null;
            this._DocBAddrFax = null;
            this._DocBAddrEmail = null;
            this._DocDeptKey = 0;
            this._DocTranGrpKey = 0;
            this._DocAccKey = 0;
            this._DocPayModeKey = null;
            this._DocChqDate = DateTime.Today.Date;
            this._DocChqNum = null;
            this._DocBankKey = null;
            this._DocRemDelivery = null;
            this._DocRemPrice = null;
            this._DocRemValidity = null;
            this._DocRemPayment = null;
            this._DocPermitNum = null;
            this._DocGoodsDestination = null;
            this._DocCountryOrigin = null;
            this._DocRemAdditional1 = null;
            this._DocRemAdditional2 = null;
            this._DocRemAdditional3 = null;
            this._DocRemAdditional4 = null;
            this._DocSubTotal = 0;
            this._DocTaxGrpKey = null;
            this._DocTaxGrpRate = 0;
            this._DocTaxTotal = 0;
            this._DocGrand = 0;
            this._DocCurrKey = 0;
            this._DocCurrRate = 0;
            this._DocHome = 0;
            this._DocApplyAmtF = 0;
            this._DocCountryRate = 0;
            this._DocTaxTotalLocal = 0;
            this._DocApplyFull = false;
            this._DocDeposit = false;
            this._DocConID = string.Empty;
            this._DocAccID = string.Empty;
            this._DocAccDes = string.Empty;
            this._Doc_PUR_Code = string.Empty; //added by thettm on 04 jun 2018
            this._VerifyAsApprove = false;
        }
    
    }
}