


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
    /// Summary description for ARIV.
    /// </summary>
    [Serializable]
    public class ARIV : Document, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        internal int _DocConKey;
        internal string _DocConNm;
        internal string _DocConUEN;
        internal int _DocDeptKey;
        internal int? _DocTranGrpKey;
        internal int _DocAccKey;
        internal int _DocGrpKey;
        internal int? _DocPriceType;
        internal int? _DocTermKey;
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
        internal string _DocSAddrStreet;
        internal string _DocSAddrPOBox;
        internal string _DocSAddrCity;
        internal string _DocSAddrState;
        internal string _DocSAddrZipCode;
        internal string _DocSAddrCountry;
        internal string _DocSAddrRegion;
        internal string _DocSAddrAttn;
        internal string _DocSAddrTel1;
        internal string _DocSAddrTel2;
        internal string _DocSAddrFax;
        internal string _DocSAddrEmail;
        internal string _DocShipName;
        internal string _DocShipMark;
        internal int? _DocShipKey;
        internal DateTime? _DocShipDate;
        internal string _DocCustPONum;
        internal string _DocQONum;
        internal string _DocSONum;
        internal string _DocDONum;
        internal string _DocIVNum;
        internal string _DocPONum;
        internal string _DocPDNum;
        internal string _DocBLNum;
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
        internal int? _DocOverallDisAcc;
        internal decimal _DocOverallDisRate;
        internal decimal _DocOverallDisAmt;
        internal decimal _DocTotalAfterDis;
        internal int? _DocTaxGrpKey;
        internal decimal _DocTaxGrpRate;
        internal decimal _DocTaxTotal;
        internal decimal _DocTotal;
        internal DateTime? _DocPaidDate;
        internal int? _DocPaidAccKey;
        internal int? _DocPaidModeKey;
        internal string _DocPaidChqNum;
        internal string _DocPaidRef;
        internal string _DocPaidDes;
        internal decimal _DocPaidAmtF;
        internal int? _DocPaidBankKey;
        internal decimal _DocGrand;
        internal int _DocCurrKey;
        internal decimal _DocCurrRate;
        internal decimal _DocHomeTaxTotal;
        internal decimal _DocHomeSubTotal;
        internal decimal _DocHome;
        internal decimal _DocCountryRate;
        internal decimal _DocTaxTotalLocal;
        internal DateTime? _DocDisDate;
        internal DateTime? _DocDueDate;
        internal int _DocApplyIVDC;
        internal int _DocApplyIVDK;
        internal string _DocApplyIVID;
        internal decimal _DocApplyGainAmt;
        internal int? _DocApplyGainAccKey;
        internal decimal _DocApplyAmtF;
        internal decimal _DocApplyAmtH;
        internal bool _DocApplyFull;
        internal decimal _DocRevalueAmtH;
        internal decimal _DocRevalueRate;
        internal bool _DocAllowIntIV;
        internal bool _DocIntYN;
        internal string _DocConID;
        internal string _DocAccID;
        internal string _DocAccDes;
        internal string _DocOverallDisAccID;
        internal string _DocOverallDisAccDes;
        internal string _DocPaidAccID;
        internal string _DocPaidAccDes;
        internal string _DocApplyGainAccID;
        internal string _DocApplyGainAccDes;
        private SYSAttachments attachments= new SYSAttachments();

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public ARIV()
            : base()
        {
            this._DocConKey = 0;
            this._DocConNm = string.Empty;
            this._DocConUEN = null;
            this._DocDeptKey = 0;
            this._DocTranGrpKey = 0;
            this._DocAccKey = 0;
            this._DocGrpKey = 0;
            this._DocPriceType = null;
            this._DocTermKey = 0;
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
            this._DocSAddrStreet = null;
            this._DocSAddrPOBox = null;
            this._DocSAddrCity = null;
            this._DocSAddrState = null;
            this._DocSAddrZipCode = null;
            this._DocSAddrCountry = null;
            this._DocSAddrRegion = null;
            this._DocSAddrAttn = null;
            this._DocSAddrTel1 = null;
            this._DocSAddrTel2 = null;
            this._DocSAddrFax = null;
            this._DocSAddrEmail = null;
            this._DocShipName = null;
            this._DocShipMark = null;
            this._DocShipKey = null;
            this._DocShipDate = DateTime.Today.Date;
            this._DocCustPONum = null;
            this._DocQONum = null;
            this._DocSONum = null;
            this._DocDONum = null;
            this._DocIVNum = null;
            this._DocPONum = null;
            this._DocPDNum = null;
            this._DocBLNum = null;
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
            this._DocOverallDisAcc = null;
            this._DocOverallDisRate = 0;
            this._DocOverallDisAmt = 0;
            this._DocTotalAfterDis = 0;
            this._DocTaxGrpKey = null;
            this._DocTaxGrpRate = 0;
            this._DocTaxTotal = 0;
            this._DocTotal = 0;
            this._DocPaidDate = DateTime.Today.Date;
            this._DocPaidAccKey = null;
            this._DocPaidModeKey = null;
            this._DocPaidChqNum = null;
            this._DocPaidRef = null;
            this._DocPaidDes = null;
            this._DocPaidAmtF = 0;
            this._DocPaidBankKey = null;
            this._DocGrand = 0;
            this._DocCurrKey = 0;
            this._DocCurrRate = 0;
            this._DocHomeSubTotal = 0;
            this._DocHomeTaxTotal = 0;
            this._DocHome = 0;
            this._DocCountryRate = 0;
            this._DocTaxTotalLocal = 0;
            this._DocDisDate = DateTime.Today.Date;
            this._DocDueDate = DateTime.Today.Date;
            this._DocApplyIVDC = 0;
            this._DocApplyIVDK = 0;
            this._DocApplyIVID = null;
            this._DocApplyGainAmt = 0;
            this._DocApplyGainAccKey = null;
            this._DocApplyAmtF = 0;
            this._DocApplyAmtH = 0;
            this._DocApplyFull = false;
            this._DocRevalueAmtH = 0;
            this._DocRevalueRate = 0;
            this._DocAllowIntIV = false;
            this._DocIntYN = false;
            this._DocConID = string.Empty;
            this._DocAccID = string.Empty;
            this._DocAccDes = string.Empty;
            this._DocOverallDisAccID = string.Empty;
            this._DocOverallDisAccDes = string.Empty;
            this._DocPaidAccID = string.Empty;
            this._DocPaidAccDes = string.Empty;
            this._DocApplyGainAccID = string.Empty;
            this._DocApplyGainAccDes = string.Empty;          
            attachments = new SYSAttachments();
            base.PropertyChanged += new PropertyChangedEventHandler(ARIV_PropertyChanged);
        }
        void ARIV_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }

        public ARIV Clone()
        {
            ARIV objCopy = (ARIV)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
        }
        public static ARIV Get(int? docKey)
        {
            ARIV child = new ARIV();
            child.Fetch(new Criteria(docKey, 1));
            return child;
        }

        public static ARIV New()
        {
            ARIV child = new ARIV();
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
            try
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(info));
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
        public int DocAccKey
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
        public int? DocPriceType
        {

            get
            {
                return this._DocPriceType;
            }
            set
            {
                this._DocPriceType = value;
                NotifyPropertyChanged("DocPriceType");
            }
        }
        public int? DocTermKey
        {

            get
            {
                return this._DocTermKey;
            }
            set
            {
                this._DocTermKey = value;
                NotifyPropertyChanged("DocTermKey");
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
        public string DocSAddrStreet
        {

            get
            {
                return this._DocSAddrStreet;
            }
            set
            {
                this._DocSAddrStreet = value;
                NotifyPropertyChanged("DocSAddrStreet");
            }
        }
        public string DocSAddrPOBox
        {

            get
            {
                return this._DocSAddrPOBox;
            }
            set
            {
                this._DocSAddrPOBox = value;
                NotifyPropertyChanged("DocSAddrPOBox");
            }
        }
        public string DocSAddrCity
        {

            get
            {
                return this._DocSAddrCity;
            }
            set
            {
                this._DocSAddrCity = value;
                NotifyPropertyChanged("DocSAddrCity");
            }
        }
        public string DocSAddrState
        {

            get
            {
                return this._DocSAddrState;
            }
            set
            {
                this._DocSAddrState = value;
                NotifyPropertyChanged("DocSAddrState");
            }
        }
        public string DocSAddrZipCode
        {

            get
            {
                return this._DocSAddrZipCode;
            }
            set
            {
                this._DocSAddrZipCode = value;
                NotifyPropertyChanged("DocSAddrZipCode");
            }
        }
        public string DocSAddrCountry
        {

            get
            {
                return this._DocSAddrCountry;
            }
            set
            {
                this._DocSAddrCountry = value;
                NotifyPropertyChanged("DocSAddrCountry");
            }
        }
        public string DocSAddrRegion
        {

            get
            {
                return this._DocSAddrRegion;
            }
            set
            {
                this._DocSAddrRegion = value;
                NotifyPropertyChanged("DocSAddrRegion");
            }
        }
        public string DocSAddrAttn
        {

            get
            {
                return this._DocSAddrAttn;
            }
            set
            {
                this._DocSAddrAttn = value;
                NotifyPropertyChanged("DocSAddrAttn");
            }
        }
        public string DocSAddrTel1
        {

            get
            {
                return this._DocSAddrTel1;
            }
            set
            {
                this._DocSAddrTel1 = value;
                NotifyPropertyChanged("DocSAddrTel1");
            }
        }
        public string DocSAddrTel2
        {

            get
            {
                return this._DocSAddrTel2;
            }
            set
            {
                this._DocSAddrTel2 = value;
                NotifyPropertyChanged("DocSAddrTel2");
            }
        }
        public string DocSAddrFax
        {

            get
            {
                return this._DocSAddrFax;
            }
            set
            {
                this._DocSAddrFax = value;
                NotifyPropertyChanged("DocSAddrFax");
            }
        }
        public string DocSAddrEmail
        {

            get
            {
                return this._DocSAddrEmail;
            }
            set
            {
                this._DocSAddrEmail = value;
                NotifyPropertyChanged("DocSAddrEmail");
            }
        }
        public string DocShipName
        {

            get
            {
                return this._DocShipName;
            }
            set
            {
                this._DocShipName = value;
                NotifyPropertyChanged("DocShipName");
            }
        }
        public string DocShipMark
        {

            get
            {
                return this._DocShipMark;
            }
            set
            {
                this._DocShipMark = value;
                NotifyPropertyChanged("DocShipMark");
            }
        }
        public int? DocShipKey
        {

            get
            {
                return this._DocShipKey;
            }
            set
            {
                this._DocShipKey = value;
                NotifyPropertyChanged("DocShipKey");
            }
        }
        public DateTime? DocShipDate
        {

            get
            {
                return this._DocShipDate;
            }
            set
            {
                this._DocShipDate = value;
                NotifyPropertyChanged("DocShipDate");
            }
        }
        public string DocCustPONum
        {

            get
            {
                return this._DocCustPONum;
            }
            set
            {
                this._DocCustPONum = value;
                NotifyPropertyChanged("DocCustPONum");
            }
        }
        public string DocQONum
        {

            get
            {
                return this._DocQONum;
            }
            set
            {
                this._DocQONum = value;
                NotifyPropertyChanged("DocQONum");
            }
        }
        public string DocSONum
        {

            get
            {
                return this._DocSONum;
            }
            set
            {
                this._DocSONum = value;
                NotifyPropertyChanged("DocSONum");
            }
        }
        public string DocDONum
        {

            get
            {
                return this._DocDONum;
            }
            set
            {
                this._DocDONum = value;
                NotifyPropertyChanged("DocDONum");
            }
        }
        public string DocIVNum
        {

            get
            {
                return this._DocIVNum;
            }
            set
            {
                this._DocIVNum = value;
                NotifyPropertyChanged("DocIVNum");
            }
        }
        public string DocPONum
        {

            get
            {
                return this._DocPONum;
            }
            set
            {
                this._DocPONum = value;
                NotifyPropertyChanged("DocPONum");
            }
        }
        public string DocPDNum
        {

            get
            {
                return this._DocPDNum;
            }
            set
            {
                this._DocPDNum = value;
                NotifyPropertyChanged("DocPDNum");
            }
        }
        public string DocBLNum
        {

            get
            {
                return this._DocBLNum;
            }
            set
            {
                this._DocBLNum = value;
                NotifyPropertyChanged("DocBLNum");
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
        public int? DocOverallDisAcc
        {

            get
            {
                return this._DocOverallDisAcc;
            }
            set
            {
                this._DocOverallDisAcc = value;
                NotifyPropertyChanged("DocOverallDisAcc");
            }
        }
        public decimal DocOverallDisRate
        {

            get
            {
                return this._DocOverallDisRate;
            }
            set
            {
                this._DocOverallDisRate = value;
                NotifyPropertyChanged("DocOverallDisRate");
            }
        }
        public decimal DocOverallDisAmt
        {

            get
            {
                return this._DocOverallDisAmt;
            }
            set
            {
                this._DocOverallDisAmt = value;
                NotifyPropertyChanged("DocOverallDisAmt");
            }
        }
        public decimal DocTotalAfterDis
        {

            get
            {
                return this._DocTotalAfterDis;
            }
            set
            {
                this._DocTotalAfterDis = value;
                NotifyPropertyChanged("DocTotalAfterDis");
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
        public decimal DocTotal
        {

            get
            {
                return this._DocTotal;
            }
            set
            {
                this._DocTotal = value;
                NotifyPropertyChanged("DocTotal");
            }
        }
        public DateTime? DocPaidDate
        {

            get
            {
                return this._DocPaidDate;
            }
            set
            {
                this._DocPaidDate = value;
                NotifyPropertyChanged("DocPaidDate");
            }
        }
        public int? DocPaidAccKey
        {

            get
            {
                return this._DocPaidAccKey;
            }
            set
            {
                this._DocPaidAccKey = value;
                NotifyPropertyChanged("DocPaidAccKey");
            }
        }
        public int? DocPaidModeKey
        {

            get
            {
                return this._DocPaidModeKey;
            }
            set
            {
                this._DocPaidModeKey = value;
                NotifyPropertyChanged("DocPaidModeKey");
            }
        }
        public string DocPaidChqNum
        {

            get
            {
                return this._DocPaidChqNum;
            }
            set
            {
                this._DocPaidChqNum = value;
                NotifyPropertyChanged("DocPaidChqNum");
            }
        }
        public string DocPaidRef
        {

            get
            {
                return this._DocPaidRef;
            }
            set
            {
                this._DocPaidRef = value;
                NotifyPropertyChanged("DocPaidRef");
            }
        }
        public string DocPaidDes
        {

            get
            {
                return this._DocPaidDes;
            }
            set
            {
                this._DocPaidDes = value;
                NotifyPropertyChanged("DocPaidDes");
            }
        }
        public decimal DocPaidAmtF
        {

            get
            {
                return this._DocPaidAmtF;
            }
            set
            {
                this._DocPaidAmtF = value;
                NotifyPropertyChanged("DocPaidAmtF");
            }
        }
        public int? DocPaidBankKey
        {

            get
            {
                return this._DocPaidBankKey;
            }
            set
            {
                this._DocPaidBankKey = value;
                NotifyPropertyChanged("DocPaidBankKey");
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
        public decimal DocHomeSubTotal
        {

            get
            {
                return this._DocHomeSubTotal;
            }
            set
            {
                this._DocHomeSubTotal = value;
                NotifyPropertyChanged("DocHomeSubTotal");
            }
        }
        public decimal DocHomeTaxTotal
        {

            get
            {
                return this._DocHomeTaxTotal;
            }
            set
            {
                this._DocHomeTaxTotal = value;
                NotifyPropertyChanged("DocHomeTaxTotal");
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
        public DateTime? DocDisDate
        {

            get
            {
                return this._DocDisDate;
            }
            set
            {
                this._DocDisDate = value;
                NotifyPropertyChanged("DocDisDate");
            }
        }
        public DateTime? DocDueDate
        {

            get
            {
                return this._DocDueDate;
            }
            set
            {
                this._DocDueDate = value;
                NotifyPropertyChanged("DocDueDate");
            }
        }
        public int DocApplyIVDC
        {

            get
            {
                return this._DocApplyIVDC;
            }
            set
            {
                this._DocApplyIVDC = value;
                NotifyPropertyChanged("DocApplyIVDC");
            }
        }
        public int DocApplyIVDK
        {

            get
            {
                return this._DocApplyIVDK;
            }
            set
            {
                this._DocApplyIVDK = value;
                NotifyPropertyChanged("DocApplyIVDK");
            }
        }
        public string DocApplyIVID
        {

            get
            {
                return this._DocApplyIVID;
            }
            set
            {
                this._DocApplyIVID = value;
                NotifyPropertyChanged("DocApplyIVID");
            }
        }
        public decimal DocApplyGainAmt
        {

            get
            {
                return this._DocApplyGainAmt;
            }
            set
            {
                this._DocApplyGainAmt = value;
                NotifyPropertyChanged("DocApplyGainAmt");
            }
        }
        public int? DocApplyGainAccKey
        {

            get
            {
                return this._DocApplyGainAccKey;
            }
            set
            {
                this._DocApplyGainAccKey = value;
                NotifyPropertyChanged("DocApplyGainAccKey");
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
        public decimal DocRevalueAmtH
        {

            get
            {
                return this._DocRevalueAmtH;
            }
            set
            {
                this._DocRevalueAmtH = value;
                NotifyPropertyChanged("DocRevalueAmtH");
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
        public bool DocAllowIntIV
        {

            get
            {
                return this._DocAllowIntIV;
            }
            set
            {
                this._DocAllowIntIV = value;
                NotifyPropertyChanged("DocAllowIntIV");
            }
        }
        public bool DocIntYN
        {

            get
            {
                return this._DocIntYN;
            }
            set
            {
                this._DocIntYN = value;
                NotifyPropertyChanged("DocIntYN");
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
        public string DocOverallDisAccID
        {

            get
            {
                return this._DocOverallDisAccID;
            }
            set
            {
                this._DocOverallDisAccID = value;
                NotifyPropertyChanged("DocOverallDisAccID");
            }
        }
        public string DocOverallDisAccDes
        {

            get
            {
                return this._DocOverallDisAccDes;
            }
            set
            {
                this._DocOverallDisAccDes = value;
                NotifyPropertyChanged("DocOverallDisAccDes");
            }
        }
        public string DocPaidAccID
        {

            get
            {
                return this._DocPaidAccID;
            }
            set
            {
                this._DocPaidAccID = value;
                NotifyPropertyChanged("DocPaidAccID");
            }
        }
        public string DocPaidAccDes
        {

            get
            {
                return this._DocPaidAccDes;
            }
            set
            {
                this._DocPaidAccDes = value;
                NotifyPropertyChanged("DocPaidAccDes");
            }
        }
        public string DocApplyGainAccID
        {

            get
            {
                return this._DocApplyGainAccID;
            }
            set
            {
                this._DocApplyGainAccID = value;
                NotifyPropertyChanged("DocApplyGainAccID");
            }
        }
        public string DocApplyGainAccDes
        {

            get
            {
                return this._DocApplyGainAccDes;
            }
            set
            {
                this._DocApplyGainAccDes = value;
                NotifyPropertyChanged("DocApplyGainAccDes");
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
            public int? _NewDocKey = null;

            internal Criteria()
            {
            }
            internal Criteria(int? DocKey)
            {
                _DocKey = DocKey;
                _option = 1;
                _DocCodeKey = (int)GEnum.SystemCode.Sales_Invoice;
                _DocID = "";
            }
            internal Criteria(int? DocKey, int? Option)
            {
                _DocKey = DocKey;
                _option = Option;
                _DocCodeKey = (int)GEnum.SystemCode.Sales_Invoice;
                _DocID = "";
            }
            internal Criteria(int DocCodeKey, int? DocKey, int? Option)
            {
                _DocCodeKey = DocCodeKey;
                _DocKey = DocKey;
                _option = Option;
                _DocID = "";
            }
            internal Criteria(int? DocCodeKey, int? DocKey, int? NewDocKey,int?Option)
            {
                _DocCodeKey = DocCodeKey;
                _DocKey = DocKey;
                _NewDocKey = NewDocKey;
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
                cm.CommandText = "ARIV_Get";

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
        internal static ARIV Get(IDataReader dr)
        {
            ARIV child = new ARIV();
            child.Fetch(dr);
            return child;
        }
        internal static ARIV Get(SqlConnection cn, Criteria criteria)
        {
            ARIV child = new ARIV();
            child.Fetch(cn, criteria);
            return child;
        }

        internal bool Fetch(IDataReader dataReader)
        {
            return Fetch(dataReader,false);
        }
        #endregion //Data Access - Fetch

        #region Datat Access-Fetch Others applied  Docs
        internal bool Fetch_ARQO(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "ARQO_CreateDoc";

                cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
                cm.Parameters.AddWithValue("@DocCodeKey", criteria._DocCodeKey);
                cm.Parameters.AddWithValue("@NewDocKey", criteria._NewDocKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Using data reader as record set.
                using (IDataReader dr = cm.ExecuteReader())
                {
                    //If data reader can read, continue...
                    if (dr.Read())
                    {
                        retValue = this.Fetch(dr,true);
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
        internal bool Fetch_ARSO(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "ARSO_CreateDoc";

                cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
                cm.Parameters.AddWithValue("@DocCodeKey", criteria._DocCodeKey);
                cm.Parameters.AddWithValue("@NewDocKey", criteria._NewDocKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Using data reader as record set.
                using (IDataReader dr = cm.ExecuteReader())
                {
                    //If data reader can read, continue...
                    while (dr.Read())
                    {
                        retValue = this.Fetch(dr, true);
                    }
                }// Already close and dispose data reader.

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Fetch_ARDO(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "ARDO_CreateDoc";

                cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
                cm.Parameters.AddWithValue("@DocCodeKey", criteria._DocCodeKey);
                cm.Parameters.AddWithValue("@NewDocKey", criteria._NewDocKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Using data reader as record set.
                using (IDataReader dr = cm.ExecuteReader())
                {
                    //If data reader can read, continue...
                    while (dr.Read())
                    {
                        retValue = this.Fetch(dr, true);
                    }
                }// Already close and dispose data reader.

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Fetch(IDataReader dataReader,bool CreateDoc)
        {
            if (CreateDoc == false)
            {
                _DocKey = dataReader["DocKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocKey"];
                _DocCodeKey = dataReader["DocCodeKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocCodeKey"];
                _DocID = dataReader["DocID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocID"];
                _DocType = dataReader["DocType"] == DBNull.Value ? (int)0 : (int)dataReader["DocType"];
               
                _DocSign = dataReader["DocSign"] == DBNull.Value ? (Int16)0 : (Int16)dataReader["DocSign"];
                _DocPaidDate = dataReader["DocPaidDate"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["DocPaidDate"];
                _DocPaidAccKey = dataReader["DocPaidAccKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocPaidAccKey"];
                _DocPaidModeKey = dataReader["DocPaidModeKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocPaidModeKey"];
                _DocPaidChqNum = dataReader["DocPaidChqNum"] == DBNull.Value ? (string)null : (string)dataReader["DocPaidChqNum"];
                _DocPaidRef = dataReader["DocPaidRef"] == DBNull.Value ? (string)null : (string)dataReader["DocPaidRef"];
                _DocPaidDes = dataReader["DocPaidDes"] == DBNull.Value ? (string)null : (string)dataReader["DocPaidDes"];
                _DocPaidAmtF = dataReader["DocPaidAmtF"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocPaidAmtF"];
                _DocPaidBankKey = dataReader["DocPaidBankKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocPaidBankKey"];
                _DocApplyIVDC = dataReader["DocApplyIVDC"] == DBNull.Value ? (int)0 : (int)dataReader["DocApplyIVDC"];
                _DocApplyIVDK = dataReader["DocApplyIVDK"] == DBNull.Value ? (int)0 : (int)dataReader["DocApplyIVDK"];
                _DocApplyIVID = dataReader["DocApplyIVID"] == DBNull.Value ? (string)null : (string)dataReader["DocApplyIVID"];
                _DocApplyGainAmt = dataReader["DocApplyGainAmt"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocApplyGainAmt"];
                _DocApplyGainAccKey = dataReader["DocApplyGainAccKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocApplyGainAccKey"];
                _DocApplyAmtF = dataReader["DocApplyAmtF"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocApplyAmtF"];
                _DocApplyAmtH = dataReader["DocApplyAmtH"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocApplyAmtH"];
                _DocApplyFull = dataReader["DocApplyFull"] == DBNull.Value ? (bool)false : (bool)dataReader["DocApplyFull"];
                _DocRevalueAmtH = dataReader["DocRevalueAmtH"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocRevalueAmtH"];
                _DocRevalueRate = dataReader["DocRevalueRate"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocRevalueRate"];
                _DocAllowIntIV = dataReader["DocAllowIntIV"] == DBNull.Value ? (bool)false : (bool)dataReader["DocAllowIntIV"];
                _DocIntYN = dataReader["DocIntYN"] == DBNull.Value ? (bool)false : (bool)dataReader["DocIntYN"];
                _DocStatus = dataReader["DocStatus"] == DBNull.Value ? (string)null : (string)dataReader["DocStatus"];
                _DocState = dataReader["DocState"] == DBNull.Value ? (int)0 : (int)dataReader["DocState"];
                _DocPrinted = dataReader["DocPrinted"] == DBNull.Value ? (bool)false : (bool)dataReader["DocPrinted"];
                _ApproveUserKey = dataReader["ApproveUserKey"] == DBNull.Value ? (int)0 : (int)dataReader["ApproveUserKey"];
                _ApproveDate = dataReader["ApproveDate"] == DBNull.Value ? null : (DateTime?)dataReader["ApproveDate"];
                _DisapproveUserKey = dataReader["DisapproveUserKey"] == DBNull.Value ? (int)0 : (int)dataReader["DisapproveUserKey"];
                _DisapproveDate = dataReader["DisapproveDate"] == DBNull.Value ? null : (DateTime?)dataReader["DisapproveDate"];
                _DisapproveCount = dataReader["DisapproveCount"] == DBNull.Value ? (Int16)0 : (Int16)dataReader["DisapproveCount"];
                _DisapproveMsg = dataReader["DisapproveMsg"] == DBNull.Value ? (string)null : (string)dataReader["DisapproveMsg"];
                _CreateDate = dataReader["CreateDate"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["CreateDate"];
                _CreateUserKey = dataReader["CreateUserKey"] == DBNull.Value ? (int?)null : (int?)dataReader["CreateUserKey"];
                _LastModifiedDate = dataReader["LastModifiedDate"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["LastModifiedDate"];
                _LastModifiedUserKey = dataReader["LastModifiedUserKey"] == DBNull.Value ? (int?)null : (int?)dataReader["LastModifiedUserKey"];
                _PurgeKeep = dataReader["PurgeKeep"] == DBNull.Value ? (int)0 : (int)dataReader["PurgeKeep"];
                _PurgeData = dataReader["PurgeData"] == DBNull.Value ? (bool)false : (bool)dataReader["PurgeData"];
            }
            _DocTypeNm = dataReader["DocTypeNm"] == DBNull.Value ? "Credit Sale" : (string)dataReader["DocTypeNm"];
            _DocDate = dataReader["DocDate"] == DBNull.Value ? (DateTime)DateTime.Today.Date : (DateTime)dataReader["DocDate"];
            _DocConKey = dataReader["DocConKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocConKey"];
            _DocConNm = dataReader["DocConNm"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocConNm"];
            _DocConUEN = dataReader["DocConUEN"] == DBNull.Value ? (string)null : (string)dataReader["DocConUEN"];
            _DocDeptKey = dataReader["DocDeptKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocDeptKey"];
            _DocTranGrpKey = dataReader["DocTranGrpKey"] == DBNull.Value ? (int?)0 : (int?)dataReader["DocTranGrpKey"];
            _DocAccKey = dataReader["DocAccKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocAccKey"];
            _DocGrpKey = dataReader["DocGrpKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocGrpKey"];
            _DocPriceType = dataReader["DocPriceType"] == DBNull.Value ? (int?)null : (int?)dataReader["DocPriceType"];
            _DocTermKey = dataReader["DocTermKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocTermKey"];
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
            _DocSAddrStreet = dataReader["DocSAddrStreet"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrStreet"];
            _DocSAddrPOBox = dataReader["DocSAddrPOBox"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrPOBox"];
            _DocSAddrCity = dataReader["DocSAddrCity"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrCity"];
            _DocSAddrState = dataReader["DocSAddrState"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrState"];
            _DocSAddrZipCode = dataReader["DocSAddrZipCode"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrZipCode"];
            _DocSAddrCountry = dataReader["DocSAddrCountry"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrCountry"];
            _DocSAddrRegion = dataReader["DocSAddrRegion"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrRegion"];
            _DocSAddrAttn = dataReader["DocSAddrAttn"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrAttn"];
            _DocSAddrTel1 = dataReader["DocSAddrTel1"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrTel1"];
            _DocSAddrTel2 = dataReader["DocSAddrTel2"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrTel2"];
            _DocSAddrFax = dataReader["DocSAddrFax"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrFax"];
            _DocSAddrEmail = dataReader["DocSAddrEmail"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrEmail"];
            _DocShipName = dataReader["DocShipName"] == DBNull.Value ? (string)null : (string)dataReader["DocShipName"];
            _DocShipMark = dataReader["DocShipMark"] == DBNull.Value ? (string)null : (string)dataReader["DocShipMark"];
            _DocShipKey = dataReader["DocShipKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocShipKey"];
            _DocShipDate = dataReader["DocShipDate"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["DocShipDate"];
            _DocCustPONum = dataReader["DocCustPONum"] == DBNull.Value ? (string)null : (string)dataReader["DocCustPONum"];
            _DocQONum = dataReader["DocQONum"] == DBNull.Value ? (string)null : (string)dataReader["DocQONum"];
            _DocSONum = dataReader["DocSONum"] == DBNull.Value ? (string)null : (string)dataReader["DocSONum"];
            _DocDONum = dataReader["DocDONum"] == DBNull.Value ? (string)null : (string)dataReader["DocDONum"];
            _DocIVNum = dataReader["DocIVNum"] == DBNull.Value ? (string)null : (string)dataReader["DocIVNum"];
            _DocPONum = dataReader["DocPONum"] == DBNull.Value ? (string)null : (string)dataReader["DocPONum"];
            _DocPDNum = dataReader["DocPDNum"] == DBNull.Value ? (string)null : (string)dataReader["DocPDNum"];
            _DocBLNum = dataReader["DocBLNum"] == DBNull.Value ? (string)null : (string)dataReader["DocBLNum"];
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
            _DocOverallDisAcc = dataReader["DocOverallDisAcc"] == DBNull.Value ? (int?)null : (int?)dataReader["DocOverallDisAcc"];
            _DocOverallDisRate = dataReader["DocOverallDisRate"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocOverallDisRate"];
            _DocOverallDisAmt = dataReader["DocOverallDisAmt"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocOverallDisAmt"];
            _DocTotalAfterDis = dataReader["DocTotalAfterDis"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocTotalAfterDis"];
            _DocTaxGrpKey = dataReader["DocTaxGrpKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocTaxGrpKey"];
            _DocTaxGrpRate = dataReader["DocTaxGrpRate"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocTaxGrpRate"];
            _DocTaxTotal = dataReader["DocTaxTotal"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocTaxTotal"];
            _DocTotal = dataReader["DocTotal"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocTotal"];
            _DocGrand = dataReader["DocGrand"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocGrand"];
            _DocCurrKey = dataReader["DocCurrKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocCurrKey"];
            _DocCurrRate = dataReader["DocCurrRate"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocCurrRate"];
            _DocHomeSubTotal = dataReader["DocHomeSubTotal"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocHomeSubTotal"];
            _DocHomeTaxTotal = dataReader["DocHomeTaxTotal"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocHomeTaxTotal"];
            _DocHome = dataReader["DocHome"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocHome"];
            _DocCountryRate = dataReader["DocCountryRate"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocCountryRate"];
            _DocTaxTotalLocal = dataReader["DocTaxTotalLocal"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocTaxTotalLocal"];
            _DocDisDate = dataReader["DocDisDate"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["DocDisDate"];
            _DocDueDate = dataReader["DocDueDate"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["DocDueDate"];
            _Attachment = dataReader["Attachment"] == DBNull.Value ? (bool)false : (bool)dataReader["Attachment"];
            _BranchKey = dataReader["BranchKey"] == DBNull.Value ? (int)0 : (int)dataReader["BranchKey"];
            _Custom1 = dataReader["Custom1"] == DBNull.Value ? (string)null : (string)dataReader["Custom1"];
            _Custom2 = dataReader["Custom2"] == DBNull.Value ? (string)null : (string)dataReader["Custom2"];
            _Custom3 = dataReader["Custom3"] == DBNull.Value ? (string)null : (string)dataReader["Custom3"];
            _Custom4 = dataReader["Custom4"] == DBNull.Value ? (string)null : (string)dataReader["Custom4"];
            _Custom5 = dataReader["Custom5"] == DBNull.Value ? (string)null : (string)dataReader["Custom5"];
            _DocConID = dataReader["DocConID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocConID"];
            _DocAccID = dataReader["DocAccID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocAccID"];
            _DocAccDes = dataReader["DocAccDes"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocAccDes"];
            _DocOverallDisAccID = dataReader["DocOverallDisAccID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocOverallDisAccID"];
            _DocOverallDisAccDes = dataReader["DocOverallDisAccDes"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocOverallDisAccDes"];
            _DocPaidAccID = dataReader["DocPaidAccID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocPaidAccID"];
            _DocPaidAccDes = dataReader["DocPaidAccDes"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocPaidAccDes"];
            _DocApplyGainAccID = dataReader["DocApplyGainAccID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocApplyGainAccID"];
            _DocApplyGainAccDes = dataReader["DocApplyGainAccDes"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocApplyGainAccDes"];
            _DefBAddrKey = dataReader["DefBAddrKey"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DefBAddrKey"];
            _DefSAddrKey = dataReader["DefSAddrKey"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DefSAddrKey"];
            _DefLocKey = dataReader["DefLocKey"] == DBNull.Value ? (int?)0 : (int?)dataReader["DefLocKey"];

            return true;
        }
        #endregion

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
                cm.CommandText = "ARIV_AddUpdate";

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
                if (_DocGrpKey == null)
                {
                    cm.Parameters.AddWithValue("@DocGrpKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocGrpKey", _DocGrpKey);
                }
                if (_DocPriceType == null)
                {
                    cm.Parameters.AddWithValue("@DocPriceType", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPriceType", _DocPriceType);
                }
                if (_DocTermKey == null)
                {
                    cm.Parameters.AddWithValue("@DocTermKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTermKey", _DocTermKey);
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
                if (_DocSAddrStreet == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrStreet", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrStreet", _DocSAddrStreet);
                }
                if (_DocSAddrPOBox == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrPOBox", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrPOBox", _DocSAddrPOBox);
                }
                if (_DocSAddrCity == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrCity", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrCity", _DocSAddrCity);
                }
                if (_DocSAddrState == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrState", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrState", _DocSAddrState);
                }
                if (_DocSAddrZipCode == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrZipCode", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrZipCode", _DocSAddrZipCode);
                }
                if (_DocSAddrCountry == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrCountry", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrCountry", _DocSAddrCountry);
                }
                if (_DocSAddrRegion == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrRegion", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrRegion", _DocSAddrRegion);
                }
                if (_DocSAddrAttn == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrAttn", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrAttn", _DocSAddrAttn);
                }
                if (_DocSAddrTel1 == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrTel1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrTel1", _DocSAddrTel1);
                }
                if (_DocSAddrTel2 == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrTel2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrTel2", _DocSAddrTel2);
                }
                if (_DocSAddrFax == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrFax", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrFax", _DocSAddrFax);
                }
                if (_DocSAddrEmail == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrEmail", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrEmail", _DocSAddrEmail);
                }
                if (_DocShipName == null)
                {
                    cm.Parameters.AddWithValue("@DocShipName", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocShipName", _DocShipName);
                }
                if (_DocShipMark == null)
                {
                    cm.Parameters.AddWithValue("@DocShipMark", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocShipMark", _DocShipMark);
                }
                if (_DocShipKey == null)
                {
                    cm.Parameters.AddWithValue("@DocShipKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocShipKey", _DocShipKey);
                }
                if (_DocShipDate == null)
                {
                    cm.Parameters.AddWithValue("@DocShipDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocShipDate", _DocShipDate);
                }
                if (_DocCustPONum == null)
                {
                    cm.Parameters.AddWithValue("@DocCustPONum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCustPONum", _DocCustPONum);
                }
                if (_DocQONum == null)
                {
                    cm.Parameters.AddWithValue("@DocQONum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocQONum", _DocQONum);
                }
                if (_DocSONum == null)
                {
                    cm.Parameters.AddWithValue("@DocSONum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSONum", _DocSONum);
                }
                if (_DocDONum == null)
                {
                    cm.Parameters.AddWithValue("@DocDONum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDONum", _DocDONum);
                }
                if (_DocIVNum == null)
                {
                    cm.Parameters.AddWithValue("@DocIVNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocIVNum", _DocIVNum);
                }
                if (_DocPONum == null)
                {
                    cm.Parameters.AddWithValue("@DocPONum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPONum", _DocPONum);
                }
                if (_DocPDNum == null)
                {
                    cm.Parameters.AddWithValue("@DocPDNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPDNum", _DocPDNum);
                }
                if (_DocBLNum == null)
                {
                    cm.Parameters.AddWithValue("@DocBLNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBLNum", _DocBLNum);
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
                if (_DocOverallDisAcc == null)
                {
                    cm.Parameters.AddWithValue("@DocOverallDisAcc", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocOverallDisAcc", _DocOverallDisAcc);
                }
                if (_DocOverallDisRate == null)
                {
                    cm.Parameters.AddWithValue("@DocOverallDisRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocOverallDisRate", _DocOverallDisRate);
                }
                if (_DocOverallDisAmt == null)
                {
                    cm.Parameters.AddWithValue("@DocOverallDisAmt", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocOverallDisAmt", _DocOverallDisAmt);
                }
                if (_DocTotalAfterDis == null)
                {
                    cm.Parameters.AddWithValue("@DocTotalAfterDis", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTotalAfterDis", _DocTotalAfterDis);
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
                if (_DocTotal == null)
                {
                    cm.Parameters.AddWithValue("@DocTotal", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTotal", _DocTotal);
                }
                if (_DocPaidDate == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidDate", _DocPaidDate);
                }
                if (_DocPaidAccKey == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidAccKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidAccKey", _DocPaidAccKey);
                }
                if (_DocPaidModeKey == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidModeKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidModeKey", _DocPaidModeKey);
                }
                if (_DocPaidChqNum == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidChqNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidChqNum", _DocPaidChqNum);
                }
                if (_DocPaidRef == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidRef", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidRef", _DocPaidRef);
                }
                if (_DocPaidDes == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidDes", _DocPaidDes);
                }
                if (_DocPaidAmtF == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidAmtF", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidAmtF", _DocPaidAmtF);
                }
                if (_DocPaidBankKey == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidBankKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidBankKey", _DocPaidBankKey);
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
                if (_DocHomeSubTotal == null)
                {
                    cm.Parameters.AddWithValue("@DocHomeSubTotal", DBNull.Value);
                }                    
                else
                {
                    cm.Parameters.AddWithValue("@DocHomeSubTotal", _DocHomeSubTotal);
                }                    
                if (_DocHomeTaxTotal == null)
                {
                    cm.Parameters.AddWithValue("@DocHomeTaxTotal", DBNull.Value);
                }                    
                else
                {
                    cm.Parameters.AddWithValue("@DocHomeTaxTotal", _DocHomeTaxTotal);
                }                
                if (_DocHome == null)
                {
                    cm.Parameters.AddWithValue("@DocHome", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHome", _DocHome);
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
                if (_DocDisDate == null)
                {
                    cm.Parameters.AddWithValue("@DocDisDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDisDate", _DocDisDate);
                }
                if (_DocDueDate == null)
                {
                    cm.Parameters.AddWithValue("@DocDueDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDueDate", _DocDueDate);
                }
                if (_DocApplyIVDC == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyIVDC", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyIVDC", _DocApplyIVDC);
                }
                if (_DocApplyIVDK == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyIVDK", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyIVDK", _DocApplyIVDK);
                }
                if (_DocApplyIVID == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyIVID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyIVID", _DocApplyIVID);
                }
                if (_DocApplyGainAmt == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyGainAmt", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyGainAmt", _DocApplyGainAmt);
                }
                if (_DocApplyGainAccKey == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyGainAccKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyGainAccKey", _DocApplyGainAccKey);
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
                if (_DocApplyFull == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyFull", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyFull", _DocApplyFull);
                }
                if (_DocRevalueAmtH == null)
                {
                    cm.Parameters.AddWithValue("@DocRevalueAmtH", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRevalueAmtH", _DocRevalueAmtH);
                }
                if (_DocRevalueRate == null)
                {
                    cm.Parameters.AddWithValue("@DocRevalueRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRevalueRate", _DocRevalueRate);
                }
                if (_DocAllowIntIV == null)
                {
                    cm.Parameters.AddWithValue("@DocAllowIntIV", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAllowIntIV", _DocAllowIntIV);
                }
                if (_DocIntYN == null)
                {
                    cm.Parameters.AddWithValue("@DocIntYN", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocIntYN", _DocIntYN);
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
                if (_DocConID == null)
                {
                    cm.Parameters.AddWithValue("@DocConID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocConID", _DocConID);
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
                if (_DocOverallDisAccID == null)
                {
                    cm.Parameters.AddWithValue("@DocOverallDisAccID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocOverallDisAccID", _DocOverallDisAccID);
                }
                if (_DocOverallDisAccDes == null)
                {
                    cm.Parameters.AddWithValue("@DocOverallDisAccDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocOverallDisAccDes", _DocOverallDisAccDes);
                }
                if (_DocPaidAccID == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidAccID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidAccID", _DocPaidAccID);
                }
                if (_DocPaidAccDes == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidAccDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidAccDes", _DocPaidAccDes);
                }
                if (_DocApplyGainAccID == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyGainAccID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyGainAccID", _DocApplyGainAccID);
                }
                if (_DocApplyGainAccDes == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyGainAccDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyGainAccDes", _DocApplyGainAccDes);
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
                cm.CommandText = "ARIV_AddUpdate";

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
                if (_DocGrpKey == null)
                {
                    cm.Parameters.AddWithValue("@DocGrpKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocGrpKey", _DocGrpKey);
                }
                if (_DocPriceType == null)
                {
                    cm.Parameters.AddWithValue("@DocPriceType", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPriceType", _DocPriceType);
                }
                if (_DocTermKey == null)
                {
                    cm.Parameters.AddWithValue("@DocTermKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTermKey", _DocTermKey);
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
                if (_DocSAddrStreet == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrStreet", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrStreet", _DocSAddrStreet);
                }
                if (_DocSAddrPOBox == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrPOBox", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrPOBox", _DocSAddrPOBox);
                }
                if (_DocSAddrCity == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrCity", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrCity", _DocSAddrCity);
                }
                if (_DocSAddrState == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrState", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrState", _DocSAddrState);
                }
                if (_DocSAddrZipCode == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrZipCode", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrZipCode", _DocSAddrZipCode);
                }
                if (_DocSAddrCountry == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrCountry", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrCountry", _DocSAddrCountry);
                }
                if (_DocSAddrRegion == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrRegion", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrRegion", _DocSAddrRegion);
                }
                if (_DocSAddrAttn == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrAttn", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrAttn", _DocSAddrAttn);
                }
                if (_DocSAddrTel1 == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrTel1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrTel1", _DocSAddrTel1);
                }
                if (_DocSAddrTel2 == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrTel2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrTel2", _DocSAddrTel2);
                }
                if (_DocSAddrFax == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrFax", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrFax", _DocSAddrFax);
                }
                if (_DocSAddrEmail == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrEmail", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrEmail", _DocSAddrEmail);
                }
                if (_DocShipName == null)
                {
                    cm.Parameters.AddWithValue("@DocShipName", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocShipName", _DocShipName);
                }
                if (_DocShipMark == null)
                {
                    cm.Parameters.AddWithValue("@DocShipMark", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocShipMark", _DocShipMark);
                }
                if (_DocShipKey == null)
                {
                    cm.Parameters.AddWithValue("@DocShipKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocShipKey", _DocShipKey);
                }
                if (_DocShipDate == null)
                {
                    cm.Parameters.AddWithValue("@DocShipDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocShipDate", _DocShipDate);
                }
                if (_DocCustPONum == null)
                {
                    cm.Parameters.AddWithValue("@DocCustPONum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCustPONum", _DocCustPONum);
                }
                if (_DocQONum == null)
                {
                    cm.Parameters.AddWithValue("@DocQONum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocQONum", _DocQONum);
                }
                if (_DocSONum == null)
                {
                    cm.Parameters.AddWithValue("@DocSONum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSONum", _DocSONum);
                }
                if (_DocDONum == null)
                {
                    cm.Parameters.AddWithValue("@DocDONum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDONum", _DocDONum);
                }
                if (_DocIVNum == null)
                {
                    cm.Parameters.AddWithValue("@DocIVNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocIVNum", _DocIVNum);
                }
                if (_DocPONum == null)
                {
                    cm.Parameters.AddWithValue("@DocPONum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPONum", _DocPONum);
                }
                if (_DocPDNum == null)
                {
                    cm.Parameters.AddWithValue("@DocPDNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPDNum", _DocPDNum);
                }
                if (_DocBLNum == null)
                {
                    cm.Parameters.AddWithValue("@DocBLNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBLNum", _DocBLNum);
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
                if (_DocOverallDisAcc == null)
                {
                    cm.Parameters.AddWithValue("@DocOverallDisAcc", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocOverallDisAcc", _DocOverallDisAcc);
                }
                if (_DocOverallDisRate == null)
                {
                    cm.Parameters.AddWithValue("@DocOverallDisRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocOverallDisRate", _DocOverallDisRate);
                }
                if (_DocOverallDisAmt == null)
                {
                    cm.Parameters.AddWithValue("@DocOverallDisAmt", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocOverallDisAmt", _DocOverallDisAmt);
                }
                if (_DocTotalAfterDis == null)
                {
                    cm.Parameters.AddWithValue("@DocTotalAfterDis", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTotalAfterDis", _DocTotalAfterDis);
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
                if (_DocTotal == null)
                {
                    cm.Parameters.AddWithValue("@DocTotal", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTotal", _DocTotal);
                }
                if (_DocPaidDate == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidDate", _DocPaidDate);
                }
                if (_DocPaidAccKey == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidAccKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidAccKey", _DocPaidAccKey);
                }
                if (_DocPaidModeKey == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidModeKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidModeKey", _DocPaidModeKey);
                }
                if (_DocPaidChqNum == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidChqNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidChqNum", _DocPaidChqNum);
                }
                if (_DocPaidRef == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidRef", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidRef", _DocPaidRef);
                }
                if (_DocPaidDes == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidDes", _DocPaidDes);
                }
                if (_DocPaidAmtF == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidAmtF", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidAmtF", _DocPaidAmtF);
                }
                if (_DocPaidBankKey == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidBankKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidBankKey", _DocPaidBankKey);
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
                if (_DocDisDate == null)
                {
                    cm.Parameters.AddWithValue("@DocDisDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDisDate", _DocDisDate);
                }
                if (_DocDueDate == null)
                {
                    cm.Parameters.AddWithValue("@DocDueDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDueDate", _DocDueDate);
                }
                if (_DocApplyIVDC == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyIVDC", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyIVDC", _DocApplyIVDC);
                }
                if (_DocApplyIVDK == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyIVDK", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyIVDK", _DocApplyIVDK);
                }
                if (_DocApplyIVID == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyIVID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyIVID", _DocApplyIVID);
                }
                if (_DocApplyGainAmt == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyGainAmt", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyGainAmt", _DocApplyGainAmt);
                }
                if (_DocApplyGainAccKey == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyGainAccKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyGainAccKey", _DocApplyGainAccKey);
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
                if (_DocApplyFull == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyFull", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyFull", _DocApplyFull);
                }
                if (_DocRevalueAmtH == null)
                {
                    cm.Parameters.AddWithValue("@DocRevalueAmtH", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRevalueAmtH", _DocRevalueAmtH);
                }
                if (_DocRevalueRate == null)
                {
                    cm.Parameters.AddWithValue("@DocRevalueRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRevalueRate", _DocRevalueRate);
                }
                if (_DocAllowIntIV == null)
                {
                    cm.Parameters.AddWithValue("@DocAllowIntIV", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAllowIntIV", _DocAllowIntIV);
                }
                if (_DocIntYN == null)
                {
                    cm.Parameters.AddWithValue("@DocIntYN", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocIntYN", _DocIntYN);
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
                if (_DocConID == null)
                {
                    cm.Parameters.AddWithValue("@DocConID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocConID", _DocConID);
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
                if (_DocOverallDisAccID == null)
                {
                    cm.Parameters.AddWithValue("@DocOverallDisAccID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocOverallDisAccID", _DocOverallDisAccID);
                }
                if (_DocOverallDisAccDes == null)
                {
                    cm.Parameters.AddWithValue("@DocOverallDisAccDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocOverallDisAccDes", _DocOverallDisAccDes);
                }
                if (_DocPaidAccID == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidAccID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidAccID", _DocPaidAccID);
                }
                if (_DocPaidAccDes == null)
                {
                    cm.Parameters.AddWithValue("@DocPaidAccDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPaidAccDes", _DocPaidAccDes);
                }
                if (_DocApplyGainAccID == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyGainAccID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyGainAccID", _DocApplyGainAccID);
                }
                if (_DocApplyGainAccDes == null)
                {
                    cm.Parameters.AddWithValue("@DocApplyGainAccDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocApplyGainAccDes", _DocApplyGainAccDes);
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

        internal bool UpdateShipment(string xml)
        {
            bool retValue = false;

            //Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "ARIV_UpdateShipment";

                    cm.Parameters.AddWithValue("@xmlDoc", xml);
                    cm.ExecuteNonQuery();

                }
            }

            return retValue;
        }

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
                cm.CommandText = "ARIV_Delete";

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
                    cm.CommandText = "ARIV_Validation";

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
            this._DocConKey = 0;
            this._DocConNm = string.Empty;
            this._DocConUEN = null;
            this._DocDeptKey = 0;
            this._DocTranGrpKey = 0;
            this._DocAccKey = 0;
            this._DocGrpKey = 0;
            this._DocPriceType = null;
            this._DocTermKey = 0;
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
            this._DocSAddrStreet = null;
            this._DocSAddrPOBox = null;
            this._DocSAddrCity = null;
            this._DocSAddrState = null;
            this._DocSAddrZipCode = null;
            this._DocSAddrCountry = null;
            this._DocSAddrRegion = null;
            this._DocSAddrAttn = null;
            this._DocSAddrTel1 = null;
            this._DocSAddrTel2 = null;
            this._DocSAddrFax = null;
            this._DocSAddrEmail = null;
            this._DocShipName = null;
            this._DocShipMark = null;
            this._DocShipKey = null;
            this._DocShipDate = DateTime.Today.Date;
            this._DocCustPONum = null;
            this._DocQONum = null;
            this._DocSONum = null;
            this._DocDONum = null;
            this._DocIVNum = null;
            this._DocPONum = null;
            this._DocPDNum = null;
            this._DocBLNum = null;
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
            this._DocOverallDisAcc = null;
            this._DocOverallDisRate = 0;
            this._DocOverallDisAmt = 0;
            this._DocTotalAfterDis = 0;
            this._DocTaxGrpKey = null;
            this._DocTaxGrpRate = 0;
            this._DocTaxTotal = 0;
            this._DocTotal = 0;
            this._DocPaidDate = DateTime.Today.Date;
            this._DocPaidAccKey = null;
            this._DocPaidModeKey = null;
            this._DocPaidChqNum = null;
            this._DocPaidRef = null;
            this._DocPaidDes = null;
            this._DocPaidAmtF = 0;
            this._DocPaidBankKey = null;
            this._DocGrand = 0;
            this._DocCurrKey = 0;
            this._DocCurrRate = 0;
            this._DocHomeSubTotal = 0;
            this._DocHomeTaxTotal = 0;
            this._DocHome = 0;
            this._DocCountryRate = 0;
            this._DocTaxTotalLocal = 0;
            this._DocDisDate = DateTime.Today.Date;
            this._DocDueDate = DateTime.Today.Date;
            this._DocApplyIVDC = 0;
            this._DocApplyIVDK = 0;
            this._DocApplyIVID = null;
            this._DocApplyGainAmt = 0;
            this._DocApplyGainAccKey = null;
            this._DocApplyAmtF = 0;
            this._DocApplyAmtH = 0;
            this._DocApplyFull = false;
            this._DocRevalueAmtH = 0;
            this._DocRevalueRate = 0;
            this._DocAllowIntIV = false;
            this._DocIntYN = false;
            this._DocConID = string.Empty;
            this._DocAccID = string.Empty;
            this._DocAccDes = string.Empty;
            this._DocOverallDisAccID = string.Empty;
            this._DocOverallDisAccDes = string.Empty;
            this._DocPaidAccID = string.Empty;
            this._DocPaidAccDes = string.Empty;
            this._DocApplyGainAccID = string.Empty;
            this._DocApplyGainAccDes = string.Empty;            
            attachments = new SYSAttachments();
        }
    
    }
}





