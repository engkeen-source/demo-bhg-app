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
    /// Summary description for APBL.
    /// </summary>
    [Serializable]
    public class APBL : Document, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        internal DateTime _DocDateOrg;
        internal int? _DocConKey;
        internal string _DocConNm;
        internal string _DocConUEN;
        internal int? _DocDeptKey;
        internal int? _DocTranGrpKey;
        internal int? _DocAccKey;
        internal int? _DocGrpKey;
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
        internal DateTime? _DocPaidDate;
        internal int? _DocPaidAccKey;
        internal int? _DocPaidModeKey;
        internal string _DocPaidChqNum;
        internal string _DocPaidRef;
        internal string _DocPaidDes;
        internal decimal _DocPaidAmtF;
        internal int? _DocPaidBankKey;
        internal decimal _DocTotal;
        internal decimal _DocGrand;
        internal int? _DocCurrKey;
        internal decimal _DocCurrRate;
        internal decimal _DocHomeTaxTotal;
        internal decimal _DocHomeSubTotal;
        internal decimal _DocHome;
        internal decimal _DocCountryRate;
        internal decimal _DocTaxTotalLocal;
        internal DateTime? _DocDueDate;
        internal decimal _DocAddFreight;
        internal decimal _DocAddInsurance;
        internal decimal _DocAddOthers;
        internal decimal _DocAddCostLumpSum;
        internal decimal _DocAddCostLumpSumRate;
        internal decimal _DocAddCostDocHomePercent;
        internal decimal _DocAddCostOthersH;
        internal decimal _DocAddCostChargesH;
        internal decimal _DocAddCostTotalH;
        internal decimal _DocAddCostItmAmtF;
        internal decimal _DocAddCostFactor;
        internal int? _DocAddCostAccKey;
        internal int? _DocApplyIVDC;
        internal int? _DocApplyIVDK;
        internal string _DocApplyIVID;
        internal decimal _DocApplyGainAmt;
        internal int? _DocApplyGainAccKey;
        internal decimal _DocApplyAmtF;
        internal decimal _DocApplyAmtH;
        internal bool _DocApplyFull;
        internal decimal _DocRevalueAmtH;
        internal decimal _DocRevalueRate;
        internal DateTime? _DocDisDate;
        internal string _DocConID;
        internal string _DocAccID;
        internal string _DocAccDes;
        internal string _DocOverallDisAccID;
        internal string _DocOverallDisAccDes;
        internal string _DocPaidAccID;
        internal string _DocPaidAccDes;
        internal string _DocAddCostAccID;
        internal string _DocAddCostAccDes;
        internal string _DocApplyGainAccID;
        internal string _DocApplyGainAccDes;
        internal SYSAttachments attachments = new SYSAttachments();

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public APBL()
            : base()
        {
            this._DocDateOrg = DateTime.Today.Date;
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
            this._DocPaidDate = DateTime.Today.Date;
            this._DocPaidAccKey = null;
            this._DocPaidModeKey = null;
            this._DocPaidChqNum = null;
            this._DocPaidRef = null;
            this._DocPaidDes = null;
            this._DocPaidAmtF = 0;
            this._DocPaidBankKey = null;
            this._DocTotal = 0;
            this._DocGrand = 0;
            this._DocCurrKey = 0;
            this._DocCurrRate = 0;
            this._DocHome = 0;
            this._DocCountryRate = 0;
            this._DocTaxTotalLocal = 0;
            this._DocDueDate = DateTime.Today.Date;
            this._DocAddFreight = 0;
            this._DocAddInsurance = 0;
            this._DocAddOthers = 0;
            this._DocAddCostLumpSum = 0;
            this._DocAddCostLumpSumRate = 0;
            this._DocAddCostDocHomePercent = 0;
            this._DocAddCostOthersH = 0;
            this._DocAddCostChargesH = 0;
            this._DocAddCostTotalH = 0;
            this._DocAddCostItmAmtF = 0;
            this._DocAddCostFactor = 0;
            this._DocAddCostAccKey = null;
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
            this._DocDisDate = DateTime.Today.Date;
            this._DocConID = string.Empty;
            this._DocAccID = string.Empty;
            this._DocAccDes = string.Empty;
            this._DocOverallDisAccID = string.Empty;
            this._DocOverallDisAccDes = string.Empty;
            this._DocPaidAccID = string.Empty;
            this._DocPaidAccDes = string.Empty;
            this._DocAddCostAccID = string.Empty;
            this._DocAddCostAccDes = string.Empty;
            this._DocApplyGainAccID = string.Empty;
            this._DocApplyGainAccDes = string.Empty;
            base.PropertyChanged += new PropertyChangedEventHandler(APBL_PropertyChanged);
        }

        void APBL_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }


        public APBL Clone()
        {
            APBL objCopy = (APBL)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
        }
        public static APBL Get(int? docKey)
        {
            APBL child = new APBL();
            child.Fetch(new Criteria(docKey, 1));
            return child;
        }

        public static APBL New()
        {
            APBL child = new APBL();
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
            attachments = null;
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


        public DateTime DocDateOrg
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
        public int? DocConKey
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
        public int? DocGrpKey
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
        public int? DocCurrKey
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
        public decimal DocAddFreight
        {

            get
            {
                return this._DocAddFreight;
            }
            set
            {
                this._DocAddFreight = value;
                NotifyPropertyChanged("DocAddFreight");
            }
        }
        public decimal DocAddInsurance
        {

            get
            {
                return this._DocAddInsurance;
            }
            set
            {
                this._DocAddInsurance = value;
                NotifyPropertyChanged("DocAddInsurance");
            }
        }
        public decimal DocAddOthers
        {

            get
            {
                return this._DocAddOthers;
            }
            set
            {
                this._DocAddOthers = value;
                NotifyPropertyChanged("DocAddOthers");
            }
        }
        public decimal DocAddCostLumpSum
        {

            get
            {
                return this._DocAddCostLumpSum;
            }
            set
            {
                this._DocAddCostLumpSum = value;
                NotifyPropertyChanged("DocAddCostLumpSum");
            }
        }
        public decimal DocAddCostLumpSumRate
        {

            get
            {
                return this._DocAddCostLumpSumRate;
            }
            set
            {
                this._DocAddCostLumpSumRate = value;
                NotifyPropertyChanged("DocAddCostLumpSumRate");
            }
        }
        public decimal DocAddCostDocHomePercent
        {

            get
            {
                return this._DocAddCostDocHomePercent;
            }
            set
            {
                this._DocAddCostDocHomePercent = value;
                NotifyPropertyChanged("DocAddCostDocHomePercent");
            }
        }
        public decimal DocAddCostOthersH
        {

            get
            {
                return this._DocAddCostOthersH;
            }
            set
            {
                this._DocAddCostOthersH = value;
                NotifyPropertyChanged("DocAddCostOthersH");
            }
        }
        public decimal DocAddCostChargesH
        {

            get
            {
                return this._DocAddCostChargesH;
            }
            set
            {
                this._DocAddCostChargesH = value;
                NotifyPropertyChanged("DocAddCostChargesH");
            }
        }
        public decimal DocAddCostTotalH
        {

            get
            {
                return this._DocAddCostTotalH;
            }
            set
            {
                this._DocAddCostTotalH = value;
                NotifyPropertyChanged("DocAddCostTotalH");
            }
        }
        public decimal DocAddCostItmAmtF
        {

            get
            {
                return this._DocAddCostItmAmtF;
            }
            set
            {
                this._DocAddCostItmAmtF = value;
                NotifyPropertyChanged("DocAddCostItmAmtF");
            }
        }
        public decimal DocAddCostFactor
        {

            get
            {
                return this._DocAddCostFactor;
            }
            set
            {
                this._DocAddCostFactor = value;
                NotifyPropertyChanged("DocAddCostFactor");
            }
        }
        public int? DocAddCostAccKey
        {

            get
            {
                return this._DocAddCostAccKey;
            }
            set
            {
                this._DocAddCostAccKey = value;
                NotifyPropertyChanged("DocAddCostAccKey");
            }
        }
        public int? DocApplyIVDC
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
        public int? DocApplyIVDK
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
        public string DocAddCostAccID
        {

            get
            {
                return this._DocAddCostAccID;
            }
            set
            {
                this._DocAddCostAccID = value;
                NotifyPropertyChanged("DocAddCostAccID");
            }
        }
        public string DocAddCostAccDes
        {

            get
            {
                return this._DocAddCostAccDes;
            }
            set
            {
                this._DocAddCostAccDes = value;
                NotifyPropertyChanged("DocAddCostAccDes");
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
            public int? _DocConKey = null;
            public int? _NewDocKey = null;

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
                _NewDocKey = Option;
            }
            internal Criteria(int? DocCodeKey, int? DocKey, string DocID, int? Option)
            {
                _DocCodeKey = DocCodeKey;
                _DocKey = DocKey;
                _DocID = DocID;
                _option = Option;

            }
            internal Criteria(int? DocCodeKey, int? DocKey, string DocID,int?DocConKey, int? Option)
            {
                _DocCodeKey = DocCodeKey;
                _DocKey = DocKey;
                _DocID = DocID;
                _option = Option;
                _DocConKey = DocConKey;

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
                cm.CommandText = "APBL_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
                cm.Parameters.AddWithValue("@DocID", criteria._DocID);
                cm.Parameters.AddWithValue("@DocCodeKey", criteria._DocCodeKey);
                cm.Parameters.AddWithValue("@DocConKey", criteria._DocConKey);
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
        internal static APBL Get(IDataReader dr)
        {
            APBL child = new APBL();
            child.Fetch(dr);
            return child;
        }
        internal static APBL Get(SqlConnection cn, Criteria criteria)
        {
            APBL child = new APBL();
            child.Fetch(cn, criteria);
            return child;
        }
        internal bool Fetch_APPD(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "APPD_CreateDoc";

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
        internal bool Fetch_APPO(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "APPO_CreateDoc";

                cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
                cm.Parameters.AddWithValue("@DocCodeKey", criteria._DocCodeKey);
                cm.Parameters.AddWithValue("@NewDocKey", criteria._NewDocKey);
                cm.Parameters.AddWithValue("@useRemarkAsDes", 0);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Using data reader as record set.
                using (IDataReader dr = cm.ExecuteReader())
                {
                    //If data reader can read, continue...
                    if (dr.Read())
                    {
                        retValue = this.Fetch(dr, true);
                    }
                    else
                        this.Clear();

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

        internal bool Fetch(IDataReader dataReader)
        {
            return Fetch(dataReader, false);
        }
        internal bool Fetch(IDataReader dataReader, bool CreateDoc)
        {
            if (CreateDoc == false)
            {
                _DocKey = dataReader["DocKey"] == DBNull.Value ? 0 : (int)dataReader["DocKey"];
                _DocCodeKey = dataReader["DocCodeKey"] == DBNull.Value ? 0 : (int)dataReader["DocCodeKey"];
                _DocID = dataReader["DocID"] == DBNull.Value ? string.Empty : (string)dataReader["DocID"];
                _DocType = dataReader["DocType"] == DBNull.Value ? 0 : (int)dataReader["DocType"];
                _DocTypeNm = dataReader["DocTypeNm"] == DBNull.Value ? string.Empty : (string)dataReader["DocTypeNm"];
                _DocSign = dataReader["DocSign"] == DBNull.Value ? 0 : (short?)dataReader["DocSign"];
                _DocPaidDate = dataReader["DocPaidDate"] == DBNull.Value ? DateTime.Today.Date : (DateTime?)dataReader["DocPaidDate"];
                _DocPaidAccKey = dataReader["DocPaidAccKey"] == DBNull.Value ? null : (int?)dataReader["DocPaidAccKey"];
                _DocPaidModeKey = dataReader["DocPaidModeKey"] == DBNull.Value ? null : (int?)dataReader["DocPaidModeKey"];
                _DocPaidChqNum = dataReader["DocPaidChqNum"] == DBNull.Value ? null : (string)dataReader["DocPaidChqNum"];
                _DocPaidRef = dataReader["DocPaidRef"] == DBNull.Value ? null : (string)dataReader["DocPaidRef"];
                _DocPaidDes = dataReader["DocPaidDes"] == DBNull.Value ? null : (string)dataReader["DocPaidDes"];
                _DocPaidAmtF = dataReader["DocPaidAmtF"] == DBNull.Value ? 0 : (decimal)dataReader["DocPaidAmtF"];
                _DocPaidBankKey = dataReader["DocPaidBankKey"] == DBNull.Value ? null : (int?)dataReader["DocPaidBankKey"];
                _DocApplyIVDC = dataReader["DocApplyIVDC"] == DBNull.Value ? 0 : (int)dataReader["DocApplyIVDC"];
                _DocApplyIVDK = dataReader["DocApplyIVDK"] == DBNull.Value ? 0 : (int)dataReader["DocApplyIVDK"];
                _DocApplyIVID = dataReader["DocApplyIVID"] == DBNull.Value ? null : (string)dataReader["DocApplyIVID"];
                _DocApplyGainAmt = dataReader["DocApplyGainAmt"] == DBNull.Value ? 0 : (decimal)dataReader["DocApplyGainAmt"];
                _DocApplyGainAccKey = dataReader["DocApplyGainAccKey"] == DBNull.Value ? null : (int?)dataReader["DocApplyGainAccKey"];
                _DocApplyAmtF = dataReader["DocApplyAmtF"] == DBNull.Value ? 0 : (decimal)dataReader["DocApplyAmtF"];
                _DocApplyAmtH = dataReader["DocApplyAmtH"] == DBNull.Value ? 0 : (decimal)dataReader["DocApplyAmtH"];
                _DocApplyFull = dataReader["DocApplyFull"] == DBNull.Value ? false : (bool)dataReader["DocApplyFull"];
                _DocRevalueAmtH = dataReader["DocRevalueAmtH"] == DBNull.Value ? 0 : (decimal)dataReader["DocRevalueAmtH"];
                _DocRevalueRate = dataReader["DocRevalueRate"] == DBNull.Value ? 0 : (decimal)dataReader["DocRevalueRate"];
                _DocStatus = dataReader["DocStatus"] == DBNull.Value ? null : (string)dataReader["DocStatus"];
                _DocState = dataReader["DocState"] == DBNull.Value ? 0 : (int)dataReader["DocState"];
                _DocPrinted = dataReader["DocPrinted"] == DBNull.Value ? false : (bool)dataReader["DocPrinted"];
                _ApproveUserKey = dataReader["ApproveUserKey"] == DBNull.Value ? 0 : (int)dataReader["ApproveUserKey"];
                _ApproveDate = dataReader["ApproveDate"] == DBNull.Value ? null : (DateTime?)dataReader["ApproveDate"];
                _DisapproveUserKey = dataReader["DisapproveUserKey"] == DBNull.Value ? 0 : (int)dataReader["DisapproveUserKey"];
                _DisapproveDate = dataReader["DisapproveDate"] == DBNull.Value ? null : (DateTime?)dataReader["DisapproveDate"];
                _DisapproveCount = dataReader["DisapproveCount"] == DBNull.Value ? 0 : (short?)dataReader["DisapproveCount"];
                _DisapproveMsg = dataReader["DisapproveMsg"] == DBNull.Value ? null : (string)dataReader["DisapproveMsg"];
                _CreateDate = dataReader["CreateDate"] == DBNull.Value ? DateTime.Today.Date : (DateTime?)dataReader["CreateDate"];
                _CreateUserKey = dataReader["CreateUserKey"] == DBNull.Value ? null : (int?)dataReader["CreateUserKey"];
                _LastModifiedDate = dataReader["LastModifiedDate"] == DBNull.Value ? DateTime.Today.Date : (DateTime?)dataReader["LastModifiedDate"];
                _LastModifiedUserKey = dataReader["LastModifiedUserKey"] == DBNull.Value ? null : (int?)dataReader["LastModifiedUserKey"];
                _PurgeKeep = dataReader["PurgeKeep"] == DBNull.Value ? 0 : (int)dataReader["PurgeKeep"];
                _PurgeData = dataReader["PurgeData"] == DBNull.Value ? false : (bool)dataReader["PurgeData"];
            }

            _DocDate = dataReader["DocDate"] == DBNull.Value ? DateTime.Today.Date : (DateTime)dataReader["DocDate"];
            _DocDateOrg = dataReader["DocDateOrg"] == DBNull.Value ? DateTime.Today.Date : (DateTime)dataReader["DocDateOrg"];
            _DocConKey = dataReader["DocConKey"] == DBNull.Value ? 0 : (int)dataReader["DocConKey"];
            _DocConNm = dataReader["DocConNm"] == DBNull.Value ? string.Empty : (string)dataReader["DocConNm"];
            _DocConUEN = dataReader["DocConUEN"] == DBNull.Value ? null : (string)dataReader["DocConUEN"];
            _DocDeptKey = dataReader["DocDeptKey"] == DBNull.Value ? 0 : (int)dataReader["DocDeptKey"];
            _DocTranGrpKey = dataReader["DocTranGrpKey"] == DBNull.Value ? 0 : (int?)dataReader["DocTranGrpKey"];
            _DocAccKey = dataReader["DocAccKey"] == DBNull.Value ? 0 : (int)dataReader["DocAccKey"];
            _DocGrpKey = dataReader["DocGrpKey"] == DBNull.Value ? 0 : (int)dataReader["DocGrpKey"];
            _DocPriceType = dataReader["DocPriceType"] == DBNull.Value ? null : (int?)dataReader["DocPriceType"];
            _DocTermKey = dataReader["DocTermKey"] == DBNull.Value ? null : (int?)dataReader["DocTermKey"];
            _DocEmKey = dataReader["DocEmKey"] == DBNull.Value ? null : (int?)dataReader["DocEmKey"];
            _DocBAddrStreet = dataReader["DocBAddrStreet"] == DBNull.Value ? null : (string)dataReader["DocBAddrStreet"];
            _DocBAddrPOBox = dataReader["DocBAddrPOBox"] == DBNull.Value ? null : (string)dataReader["DocBAddrPOBox"];
            _DocBAddrCity = dataReader["DocBAddrCity"] == DBNull.Value ? null : (string)dataReader["DocBAddrCity"];
            _DocBAddrState = dataReader["DocBAddrState"] == DBNull.Value ? null : (string)dataReader["DocBAddrState"];
            _DocBAddrZipCode = dataReader["DocBAddrZipCode"] == DBNull.Value ? null : (string)dataReader["DocBAddrZipCode"];
            _DocBAddrCountry = dataReader["DocBAddrCountry"] == DBNull.Value ? null : (string)dataReader["DocBAddrCountry"];
            _DocBAddrRegion = dataReader["DocBAddrRegion"] == DBNull.Value ? null : (string)dataReader["DocBAddrRegion"];
            _DocBAddrAttn = dataReader["DocBAddrAttn"] == DBNull.Value ? null : (string)dataReader["DocBAddrAttn"];
            _DocBAddrTel1 = dataReader["DocBAddrTel1"] == DBNull.Value ? null : (string)dataReader["DocBAddrTel1"];
            _DocBAddrTel2 = dataReader["DocBAddrTel2"] == DBNull.Value ? null : (string)dataReader["DocBAddrTel2"];
            _DocBAddrFax = dataReader["DocBAddrFax"] == DBNull.Value ? null : (string)dataReader["DocBAddrFax"];
            _DocBAddrEmail = dataReader["DocBAddrEmail"] == DBNull.Value ? null : (string)dataReader["DocBAddrEmail"];
            _DocShipName = dataReader["DocShipName"] == DBNull.Value ? null : (string)dataReader["DocShipName"];
            _DocShipMark = dataReader["DocShipMark"] == DBNull.Value ? null : (string)dataReader["DocShipMark"];
            _DocShipKey = dataReader["DocShipKey"] == DBNull.Value ? null : (int?)dataReader["DocShipKey"];
            _DocShipDate = dataReader["DocShipDate"] == DBNull.Value ? DateTime.Today.Date : (DateTime?)dataReader["DocShipDate"];
            _DocCustPONum = dataReader["DocCustPONum"] == DBNull.Value ? null : (string)dataReader["DocCustPONum"];
            _DocQONum = dataReader["DocQONum"] == DBNull.Value ? null : (string)dataReader["DocQONum"];
            _DocSONum = dataReader["DocSONum"] == DBNull.Value ? null : (string)dataReader["DocSONum"];
            _DocDONum = dataReader["DocDONum"] == DBNull.Value ? null : (string)dataReader["DocDONum"];
            _DocIVNum = dataReader["DocIVNum"] == DBNull.Value ? null : (string)dataReader["DocIVNum"];
            _DocPONum = dataReader["DocPONum"] == DBNull.Value ? null : (string)dataReader["DocPONum"];
            _DocPDNum = dataReader["DocPDNum"] == DBNull.Value ? null : (string)dataReader["DocPDNum"];
            _DocRef = dataReader["DocRef"] == DBNull.Value ? null : (string)dataReader["DocRef"];
            _DocDes = dataReader["DocDes"] == DBNull.Value ? null : (string)dataReader["DocDes"];
            _DocRem = dataReader["DocRem"] == DBNull.Value ? null : (string)dataReader["DocRem"];
            _DocRemDelivery = dataReader["DocRemDelivery"] == DBNull.Value ? null : (string)dataReader["DocRemDelivery"];
            _DocRemPrice = dataReader["DocRemPrice"] == DBNull.Value ? null : (string)dataReader["DocRemPrice"];
            _DocRemValidity = dataReader["DocRemValidity"] == DBNull.Value ? null : (string)dataReader["DocRemValidity"];
            _DocRemPayment = dataReader["DocRemPayment"] == DBNull.Value ? null : (string)dataReader["DocRemPayment"];
            _DocPermitNum = dataReader["DocPermitNum"] == DBNull.Value ? null : (string)dataReader["DocPermitNum"];
            _DocGoodsDestination = dataReader["DocGoodsDestination"] == DBNull.Value ? null : (string)dataReader["DocGoodsDestination"];
            _DocCountryOrigin = dataReader["DocCountryOrigin"] == DBNull.Value ? null : (string)dataReader["DocCountryOrigin"];
            _DocRemAdditional1 = dataReader["DocRemAdditional1"] == DBNull.Value ? null : (string)dataReader["DocRemAdditional1"];
            _DocRemAdditional2 = dataReader["DocRemAdditional2"] == DBNull.Value ? null : (string)dataReader["DocRemAdditional2"];
            _DocRemAdditional3 = dataReader["DocRemAdditional3"] == DBNull.Value ? null : (string)dataReader["DocRemAdditional3"];
            _DocRemAdditional4 = dataReader["DocRemAdditional4"] == DBNull.Value ? null : (string)dataReader["DocRemAdditional4"];
            _DocSubTotal = dataReader["DocSubTotal"] == DBNull.Value ? 0 : (decimal)dataReader["DocSubTotal"];
            _DocOverallDisAcc = dataReader["DocOverallDisAcc"] == DBNull.Value ? null : (int?)dataReader["DocOverallDisAcc"];
            _DocOverallDisRate = dataReader["DocOverallDisRate"] == DBNull.Value ? 0 : (decimal)dataReader["DocOverallDisRate"];
            _DocOverallDisAmt = dataReader["DocOverallDisAmt"] == DBNull.Value ? 0 : (decimal)dataReader["DocOverallDisAmt"];
            _DocTotalAfterDis = dataReader["DocTotalAfterDis"] == DBNull.Value ? 0 : (decimal)dataReader["DocTotalAfterDis"];
            _DocTaxGrpKey = dataReader["DocTaxGrpKey"] == DBNull.Value ? null : (int?)dataReader["DocTaxGrpKey"];
            _DocTaxGrpRate = dataReader["DocTaxGrpRate"] == DBNull.Value ? 0 : (decimal)dataReader["DocTaxGrpRate"];
            _DocTaxTotal = dataReader["DocTaxTotal"] == DBNull.Value ? 0 : (decimal)dataReader["DocTaxTotal"];
            _DocTotal = dataReader["DocTotal"] == DBNull.Value ? 0 : (decimal)dataReader["DocTotal"];
            _DocGrand = dataReader["DocGrand"] == DBNull.Value ? 0 : (decimal)dataReader["DocGrand"];
            _DocCurrKey = dataReader["DocCurrKey"] == DBNull.Value ? 0 : (int)dataReader["DocCurrKey"];
            _DocCurrRate = dataReader["DocCurrRate"] == DBNull.Value ? 0 : (decimal)dataReader["DocCurrRate"];
            _DocHomeSubTotal = dataReader["DocHomeSubTotal"] == DBNull.Value ? 0 : (decimal)dataReader["DocHomeSubTotal"];
            _DocHomeTaxTotal = dataReader["DocHomeTaxTotal"] == DBNull.Value ? 0 : (decimal)dataReader["DocHomeTaxTotal"];
            _DocHome = dataReader["DocHome"] == DBNull.Value ? 0 : (decimal)dataReader["DocHome"];
            _DocCountryRate = dataReader["DocCountryRate"] == DBNull.Value ? 0 : (decimal)dataReader["DocCountryRate"];
            _DocTaxTotalLocal = dataReader["DocTaxTotalLocal"] == DBNull.Value ? 0 : (decimal)dataReader["DocTaxTotalLocal"];
            _DocDueDate = dataReader["DocDueDate"] == DBNull.Value ? DateTime.Today.Date : (DateTime?)dataReader["DocDueDate"];
            _DocAddFreight = dataReader["DocAddFreight"] == DBNull.Value ? 0 : (decimal)dataReader["DocAddFreight"];
            _DocAddInsurance = dataReader["DocAddInsurance"] == DBNull.Value ? 0 : (decimal)dataReader["DocAddInsurance"];
            _DocAddOthers = dataReader["DocAddOthers"] == DBNull.Value ? 0 : (decimal)dataReader["DocAddOthers"];
            _DocAddCostLumpSum = dataReader["DocAddCostLumpSum"] == DBNull.Value ? 0 : (decimal)dataReader["DocAddCostLumpSum"];
            _DocAddCostLumpSumRate = dataReader["DocAddCostLumpSumRate"] == DBNull.Value ? 0 : (decimal)dataReader["DocAddCostLumpSumRate"];
            _DocAddCostDocHomePercent = dataReader["DocAddCostDocHomePercent"] == DBNull.Value ? 0 : (decimal)dataReader["DocAddCostDocHomePercent"];
            _DocAddCostOthersH = dataReader["DocAddCostOthersH"] == DBNull.Value ? 0 : (decimal)dataReader["DocAddCostOthersH"];
            _DocAddCostChargesH = dataReader["DocAddCostChargesH"] == DBNull.Value ? 0 : (decimal)dataReader["DocAddCostChargesH"];
            _DocAddCostTotalH = dataReader["DocAddCostTotalH"] == DBNull.Value ? 0 : (decimal)dataReader["DocAddCostTotalH"];
            _DocAddCostItmAmtF = dataReader["DocAddCostItmAmtF"] == DBNull.Value ? 0 : (decimal)dataReader["DocAddCostItmAmtF"];
            _DocAddCostFactor = dataReader["DocAddCostFactor"] == DBNull.Value ? 0 : (decimal)dataReader["DocAddCostFactor"];
            _DocAddCostAccKey = dataReader["DocAddCostAccKey"] == DBNull.Value ? null : (int?)dataReader["DocAddCostAccKey"];
            _DocDisDate = dataReader["DocDisDate"] == DBNull.Value ? DateTime.Today.Date : (DateTime?)dataReader["DocDisDate"];
            _Attachment = dataReader["Attachment"] == DBNull.Value ? false : (bool)dataReader["Attachment"];
            _BranchKey = dataReader["BranchKey"] == DBNull.Value ? 0 : (int)dataReader["BranchKey"];
            _Custom1 = dataReader["Custom1"] == DBNull.Value ? null : (string)dataReader["Custom1"];
            _Custom2 = dataReader["Custom2"] == DBNull.Value ? null : (string)dataReader["Custom2"];
            _Custom3 = dataReader["Custom3"] == DBNull.Value ? null : (string)dataReader["Custom3"];
            _Custom4 = dataReader["Custom4"] == DBNull.Value ? null : (string)dataReader["Custom4"];
            _Custom5 = dataReader["Custom5"] == DBNull.Value ? null : (string)dataReader["Custom5"];
            _DocConID = dataReader["DocConID"] == DBNull.Value ? string.Empty : (string)dataReader["DocConID"];
            _DocAccID = dataReader["DocAccID"] == DBNull.Value ? string.Empty : (string)dataReader["DocAccID"];
            _DocAccDes = dataReader["DocAccDes"] == DBNull.Value ? string.Empty : (string)dataReader["DocAccDes"];
            _DocOverallDisAccID = dataReader["DocOverallDisAccID"] == DBNull.Value ? string.Empty : (string)dataReader["DocOverallDisAccID"];
            _DocOverallDisAccDes = dataReader["DocOverallDisAccDes"] == DBNull.Value ? string.Empty : (string)dataReader["DocOverallDisAccDes"];
            _DocPaidAccID = dataReader["DocPaidAccID"] == DBNull.Value ? string.Empty : (string)dataReader["DocPaidAccID"];
            _DocPaidAccDes = dataReader["DocPaidAccDes"] == DBNull.Value ? string.Empty : (string)dataReader["DocPaidAccDes"];
            _DocAddCostAccID = dataReader["DocAddCostAccID"] == DBNull.Value ? string.Empty : (string)dataReader["DocAddCostAccID"];
            _DocAddCostAccDes = dataReader["DocAddCostAccDes"] == DBNull.Value ? string.Empty : (string)dataReader["DocAddCostAccDes"];
            _DocApplyGainAccID = dataReader["DocApplyGainAccID"] == DBNull.Value ? string.Empty : (string)dataReader["DocApplyGainAccID"];
            _DocApplyGainAccDes = dataReader["DocApplyGainAccDes"] == DBNull.Value ? string.Empty : (string)dataReader["DocApplyGainAccDes"];
            _DefBAddrKey = dataReader["DefBAddrKey"] == DBNull.Value ? string.Empty : (string)dataReader["DefBAddrKey"];
            _DefLocKey = dataReader["DefLocKey"] == DBNull.Value ? 0 : (int?)dataReader["DefLocKey"];
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
                cm.CommandText = "APBL_AddUpdate";

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
                if (_DocTotal == null)
                {
                    cm.Parameters.AddWithValue("@DocTotal", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTotal", _DocTotal);
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
                if (_DocDueDate == null)
                {
                    cm.Parameters.AddWithValue("@DocDueDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDueDate", _DocDueDate);
                }
                if (_DocAddFreight == null)
                {
                    cm.Parameters.AddWithValue("@DocAddFreight", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddFreight", _DocAddFreight);
                }
                if (_DocAddInsurance == null)
                {
                    cm.Parameters.AddWithValue("@DocAddInsurance", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddInsurance", _DocAddInsurance);
                }
                if (_DocAddOthers == null)
                {
                    cm.Parameters.AddWithValue("@DocAddOthers", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddOthers", _DocAddOthers);
                }
                if (_DocAddCostLumpSum == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostLumpSum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostLumpSum", _DocAddCostLumpSum);
                }
                if (_DocAddCostLumpSumRate == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostLumpSumRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostLumpSumRate", _DocAddCostLumpSumRate);
                }
                if (_DocAddCostDocHomePercent == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostDocHomePercent", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostDocHomePercent", _DocAddCostDocHomePercent);
                }
                if (_DocAddCostOthersH == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostOthersH", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostOthersH", _DocAddCostOthersH);
                }
                if (_DocAddCostChargesH == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostChargesH", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostChargesH", _DocAddCostChargesH);
                }
                if (_DocAddCostTotalH == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostTotalH", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostTotalH", _DocAddCostTotalH);
                }
                if (_DocAddCostItmAmtF == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostItmAmtF", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostItmAmtF", _DocAddCostItmAmtF);
                }
                if (_DocAddCostFactor == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostFactor", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostFactor", _DocAddCostFactor);
                }
                if (_DocAddCostAccKey == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostAccKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostAccKey", _DocAddCostAccKey);
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
                if (_DocDisDate == null)
                {
                    cm.Parameters.AddWithValue("@DocDisDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDisDate", _DocDisDate);
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
                if (_DocAddCostAccID == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostAccID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostAccID", _DocAddCostAccID);
                }
                if (_DocAddCostAccDes == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostAccDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostAccDes", _DocAddCostAccDes);
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
                cm.CommandText = "APBL_AddUpdate";

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
                if (_DocTotal == null)
                {
                    cm.Parameters.AddWithValue("@DocTotal", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTotal", _DocTotal);
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
                if (_DocDueDate == null)
                {
                    cm.Parameters.AddWithValue("@DocDueDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDueDate", _DocDueDate);
                }
                if (_DocAddFreight == null)
                {
                    cm.Parameters.AddWithValue("@DocAddFreight", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddFreight", _DocAddFreight);
                }
                if (_DocAddInsurance == null)
                {
                    cm.Parameters.AddWithValue("@DocAddInsurance", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddInsurance", _DocAddInsurance);
                }
                if (_DocAddOthers == null)
                {
                    cm.Parameters.AddWithValue("@DocAddOthers", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddOthers", _DocAddOthers);
                }
                if (_DocAddCostLumpSum == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostLumpSum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostLumpSum", _DocAddCostLumpSum);
                }
                if (_DocAddCostLumpSumRate == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostLumpSumRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostLumpSumRate", _DocAddCostLumpSumRate);
                }
                if (_DocAddCostDocHomePercent == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostDocHomePercent", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostDocHomePercent", _DocAddCostDocHomePercent);
                }
                if (_DocAddCostOthersH == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostOthersH", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostOthersH", _DocAddCostOthersH);
                }
                if (_DocAddCostChargesH == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostChargesH", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostChargesH", _DocAddCostChargesH);
                }
                if (_DocAddCostTotalH == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostTotalH", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostTotalH", _DocAddCostTotalH);
                }
                if (_DocAddCostItmAmtF == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostItmAmtF", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostItmAmtF", _DocAddCostItmAmtF);
                }
                if (_DocAddCostFactor == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostFactor", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostFactor", _DocAddCostFactor);
                }
                if (_DocAddCostAccKey == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostAccKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostAccKey", _DocAddCostAccKey);
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
                if (_DocDisDate == null)
                {
                    cm.Parameters.AddWithValue("@DocDisDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDisDate", _DocDisDate);
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
                if (_DocAddCostAccID == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostAccID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostAccID", _DocAddCostAccID);
                }
                if (_DocAddCostAccDes == null)
                {
                    cm.Parameters.AddWithValue("@DocAddCostAccDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocAddCostAccDes", _DocAddCostAccDes);
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
                cm.CommandText = "APBL_Delete";

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
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = Validation(cn, criteria, isNew);
                }
                 
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
                    cm.CommandText = "APBL_Validation";

                    cm.Parameters.AddWithValue("@isNew", isNew);
                    cm.Parameters.AddWithValue("@DocCodeKey", criteria._DocCodeKey);
                    cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
                    cm.Parameters.AddWithValue("@DocID", criteria._DocID);
                    cm.Parameters.AddWithValue("@DocConKey", criteria._option);
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
            this._DocPaidDate = DateTime.Today.Date;
            this._DocPaidAccKey = null;
            this._DocPaidModeKey = null;
            this._DocPaidChqNum = null;
            this._DocPaidRef = null;
            this._DocPaidDes = null;
            this._DocPaidAmtF = 0;
            this._DocPaidBankKey = null;
            this._DocTotal = 0;
            this._DocGrand = 0;
            this._DocCurrKey = 0;
            this._DocCurrRate = 0;
            this._DocHome = 0;
            this._DocCountryRate = 0;
            this._DocTaxTotalLocal = 0;
            this._DocDueDate = DateTime.Today.Date;
            this._DocAddFreight = 0;
            this._DocAddInsurance = 0;
            this._DocAddOthers = 0;
            this._DocAddCostLumpSum = 0;
            this._DocAddCostLumpSumRate = 0;
            this._DocAddCostDocHomePercent = 0;
            this._DocAddCostOthersH = 0;
            this._DocAddCostChargesH = 0;
            this._DocAddCostTotalH = 0;
            this._DocAddCostItmAmtF = 0;
            this._DocAddCostFactor = 0;
            this._DocAddCostAccKey = null;
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
            this._DocDisDate = DateTime.Today.Date;
            this._DocConID = string.Empty;
            this._DocAccID = string.Empty;
            this._DocAccDes = string.Empty;
            this._DocOverallDisAccID = string.Empty;
            this._DocOverallDisAccDes = string.Empty;
            this._DocPaidAccID = string.Empty;
            this._DocPaidAccDes = string.Empty;
            this._DocAddCostAccID = string.Empty;
            this._DocAddCostAccDes = string.Empty;
            this._DocApplyGainAccID = string.Empty;
            this._DocApplyGainAccDes = string.Empty;
           

        }
    }
}





