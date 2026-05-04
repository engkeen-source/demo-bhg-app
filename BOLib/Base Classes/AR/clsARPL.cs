


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
    /// Summary description for ARPL.
    /// </summary>
    [Serializable]
    public class ARPL : Document, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        internal int _DocConKey;
        internal string _DocConNm;
        internal string _DocConUEN;
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
        internal string _DocContractNum;
        internal string _DocParcelNum;
        internal string _DocShippingMark;
        internal string _DocFaceMark;
        internal string _DocEndMarking;
        internal int _DocPackingTypeKey;
        internal int _DocWeightUOMKey;
        internal decimal _DocWeightUOMRate;
        internal int _DocTLWeightUOMKey;
        internal decimal _DocTLWeightUOMRate;
        internal int _DocMeasUOMKey;
        internal int _DocTLMeasUOMKey;
        internal decimal _DocTLMeasUOMRate;
        internal string _DocHC1;
        internal string _DocHC2;
        internal string _DocHC3;
        internal string _DocHC4;
        internal string _DocHC5;
        internal string _DocHT1;
        internal string _DocHT2;
        internal string _DocHT3;
        internal string _DocHT4;
        internal string _DocHT5;
        internal string _DocFC1;
        internal string _DocFC2;
        internal string _DocFC3;
        internal string _DocFC4;
        internal string _DocFC5;
        internal string _DocFC6;
        internal string _DocFC7;
        internal decimal _DocFT1;
        internal decimal _DocFT2;
        internal decimal _DocFT3;
        internal decimal _DocFT4;
        internal decimal _DocFT5;
        internal decimal _DocFT6;
        internal decimal _DocFT7;
        internal string _DocConID;
        internal SYSAttachments attachments;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public ARPL()
            : base()
        {
            this._DocConKey = 0;
            this._DocConNm = string.Empty;
            this._DocConUEN = null;
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
            this._DocContractNum = null;
            this._DocParcelNum = null;
            this._DocShippingMark = null;
            this._DocFaceMark = null;
            this._DocEndMarking = null;
            this._DocPackingTypeKey = 0;
            this._DocWeightUOMKey = 0;
            this._DocWeightUOMRate = 0;
            this._DocTLWeightUOMKey = 0;
            this._DocTLWeightUOMRate = 0;
            this._DocMeasUOMKey = 0;
            this._DocTLMeasUOMKey = 0;
            this._DocTLMeasUOMRate = 0;
            this._DocHC1 = null;
            this._DocHC2 = null;
            this._DocHC3 = null;
            this._DocHC4 = null;
            this._DocHC5 = null;
            this._DocHT1 = null;
            this._DocHT2 = null;
            this._DocHT3 = null;
            this._DocHT4 = null;
            this._DocHT5 = null;
            this._DocFC1 = null;
            this._DocFC2 = null;
            this._DocFC3 = null;
            this._DocFC4 = null;
            this._DocFC5 = null;
            this._DocFC6 = null;
            this._DocFC7 = null;
            this._DocFT1 = 0;
            this._DocFT2 = 0;
            this._DocFT3 = 0;
            this._DocFT4 = 0;
            this._DocFT5 = 0;
            this._DocFT6 = 0;
            this._DocFT7 = 0;
            this._DocConID = string.Empty;
            this.attachments = new SYSAttachments();
            base.PropertyChanged += new PropertyChangedEventHandler(ARPL_PropertyChanged);
        }
        void ARPL_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }

        public ARPL Clone()
        {
            ARPL objCopy = (ARPL)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
        }
        public static ARPL Get(int? docKey)
        {
            ARPL child = new ARPL();
            child.Fetch(new Criteria(docKey, 1));
            return child;
        }

        public static ARPL New()
        {
            ARPL child = new ARPL();
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
        public string DocContractNum
        {

            get
            {
                return this._DocContractNum;
            }
            set
            {
                this._DocContractNum = value;
                NotifyPropertyChanged("DocContractNum");
            }
        }
        public string DocParcelNum
        {

            get
            {
                return this._DocParcelNum;
            }
            set
            {
                this._DocParcelNum = value;
                NotifyPropertyChanged("DocParcelNum");
            }
        }
        public string DocShippingMark
        {

            get
            {
                return this._DocShippingMark;
            }
            set
            {
                this._DocShippingMark = value;
                NotifyPropertyChanged("DocShippingMark");
            }
        }
        public string DocFaceMark
        {

            get
            {
                return this._DocFaceMark;
            }
            set
            {
                this._DocFaceMark = value;
                NotifyPropertyChanged("DocFaceMark");
            }
        }
        public string DocEndMarking
        {

            get
            {
                return this._DocEndMarking;
            }
            set
            {
                this._DocEndMarking = value;
                NotifyPropertyChanged("DocEndMarking");
            }
        }
        public int DocPackingTypeKey
        {

            get
            {
                return this._DocPackingTypeKey;
            }
            set
            {
                this._DocPackingTypeKey = value;
                NotifyPropertyChanged("DocPackingTypeKey");
            }
        }
        public int DocWeightUOMKey
        {

            get
            {
                return this._DocWeightUOMKey;
            }
            set
            {
                this._DocWeightUOMKey = value;
                NotifyPropertyChanged("DocWeightUOMKey");
            }
        }
        public decimal DocWeightUOMRate
        {

            get
            {
                return this._DocWeightUOMRate;
            }
            set
            {
                this._DocWeightUOMRate = value;
                NotifyPropertyChanged("DocWeightUOMRate");
            }
        }
        public int DocTLWeightUOMKey
        {

            get
            {
                return this._DocTLWeightUOMKey;
            }
            set
            {
                this._DocTLWeightUOMKey = value;
                NotifyPropertyChanged("DocTLWeightUOMKey");
            }
        }
        public decimal DocTLWeightUOMRate
        {

            get
            {
                return this._DocTLWeightUOMRate;
            }
            set
            {
                this._DocTLWeightUOMRate = value;
                NotifyPropertyChanged("DocTLWeightUOMRate");
            }
        }
        public int DocMeasUOMKey
        {

            get
            {
                return this._DocMeasUOMKey;
            }
            set
            {
                this._DocMeasUOMKey = value;
                NotifyPropertyChanged("DocMeasUOMKey");
            }
        }
        public int DocTLMeasUOMKey
        {

            get
            {
                return this._DocTLMeasUOMKey;
            }
            set
            {
                this._DocTLMeasUOMKey = value;
                NotifyPropertyChanged("DocTLMeasUOMKey");
            }
        }
        public decimal DocTLMeasUOMRate
        {

            get
            {
                return this._DocTLMeasUOMRate;
            }
            set
            {
                this._DocTLMeasUOMRate = value;
                NotifyPropertyChanged("DocTLMeasUOMRate");
            }
        }
        public string DocHC1
        {

            get
            {
                return this._DocHC1;
            }
            set
            {
                this._DocHC1 = value;
                NotifyPropertyChanged("DocHC1");
            }
        }
        public string DocHC2
        {

            get
            {
                return this._DocHC2;
            }
            set
            {
                this._DocHC2 = value;
                NotifyPropertyChanged("DocHC2");
            }
        }
        public string DocHC3
        {

            get
            {
                return this._DocHC3;
            }
            set
            {
                this._DocHC3 = value;
                NotifyPropertyChanged("DocHC3");
            }
        }
        public string DocHC4
        {

            get
            {
                return this._DocHC4;
            }
            set
            {
                this._DocHC4 = value;
                NotifyPropertyChanged("DocHC4");
            }
        }
        public string DocHC5
        {

            get
            {
                return this._DocHC5;
            }
            set
            {
                this._DocHC5 = value;
                NotifyPropertyChanged("DocHC5");
            }
        }
        public string DocHT1
        {

            get
            {
                return this._DocHT1;
            }
            set
            {
                this._DocHT1 = value;
                NotifyPropertyChanged("DocHT1");
            }
        }
        public string DocHT2
        {

            get
            {
                return this._DocHT2;
            }
            set
            {
                this._DocHT2 = value;
                NotifyPropertyChanged("DocHT2");
            }
        }
        public string DocHT3
        {

            get
            {
                return this._DocHT3;
            }
            set
            {
                this._DocHT3 = value;
                NotifyPropertyChanged("DocHT3");
            }
        }
        public string DocHT4
        {

            get
            {
                return this._DocHT4;
            }
            set
            {
                this._DocHT4 = value;
                NotifyPropertyChanged("DocHT4");
            }
        }
        public string DocHT5
        {

            get
            {
                return this._DocHT5;
            }
            set
            {
                this._DocHT5 = value;
                NotifyPropertyChanged("DocHT5");
            }
        }
        public string DocFC1
        {

            get
            {
                return this._DocFC1;
            }
            set
            {
                this._DocFC1 = value;
                NotifyPropertyChanged("DocFC1");
            }
        }
        public string DocFC2
        {

            get
            {
                return this._DocFC2;
            }
            set
            {
                this._DocFC2 = value;
                NotifyPropertyChanged("DocFC2");
            }
        }
        public string DocFC3
        {

            get
            {
                return this._DocFC3;
            }
            set
            {
                this._DocFC3 = value;
                NotifyPropertyChanged("DocFC3");
            }
        }
        public string DocFC4
        {

            get
            {
                return this._DocFC4;
            }
            set
            {
                this._DocFC4 = value;
                NotifyPropertyChanged("DocFC4");
            }
        }
        public string DocFC5
        {

            get
            {
                return this._DocFC5;
            }
            set
            {
                this._DocFC5 = value;
                NotifyPropertyChanged("DocFC5");
            }
        }
        public string DocFC6
        {

            get
            {
                return this._DocFC6;
            }
            set
            {
                this._DocFC6 = value;
                NotifyPropertyChanged("DocFC6");
            }
        }
        public string DocFC7
        {

            get
            {
                return this._DocFC7;
            }
            set
            {
                this._DocFC7 = value;
                NotifyPropertyChanged("DocFC7");
            }
        }
        public decimal DocFT1
        {

            get
            {
                return this._DocFT1;
            }
            set
            {
                this._DocFT1 = value;
                NotifyPropertyChanged("DocFT1");
            }
        }
        public decimal DocFT2
        {

            get
            {
                return this._DocFT2;
            }
            set
            {
                this._DocFT2 = value;
                NotifyPropertyChanged("DocFT2");
            }
        }
        public decimal DocFT3
        {

            get
            {
                return this._DocFT3;
            }
            set
            {
                this._DocFT3 = value;
                NotifyPropertyChanged("DocFT3");
            }
        }
        public decimal DocFT4
        {

            get
            {
                return this._DocFT4;
            }
            set
            {
                this._DocFT4 = value;
                NotifyPropertyChanged("DocFT4");
            }
        }
        public decimal DocFT5
        {

            get
            {
                return this._DocFT5;
            }
            set
            {
                this._DocFT5 = value;
                NotifyPropertyChanged("DocFT5");
            }
        }
        public decimal DocFT6
        {

            get
            {
                return this._DocFT6;
            }
            set
            {
                this._DocFT6 = value;
                NotifyPropertyChanged("DocFT6");
            }
        }
        public decimal DocFT7
        {

            get
            {
                return this._DocFT7;
            }
            set
            {
                this._DocFT7 = value;
                NotifyPropertyChanged("DocFT7");
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
            internal Criteria(int? DocCodeKey, string DocID, int? Option)
            {
                _DocCodeKey = DocCodeKey;
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
                cm.CommandText = "ARPL_Get";

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
        internal static ARPL Get(IDataReader dr)
        {
            ARPL child = new ARPL();
            child.Fetch(dr);
            return child;
        }
        internal static ARPL Get(SqlConnection cn, Criteria criteria)
        {
            ARPL child = new ARPL();
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
            _DocConKey = dataReader["DocConKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocConKey"];
            _DocConNm = dataReader["DocConNm"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocConNm"];
            _DocConUEN = dataReader["DocConUEN"] == DBNull.Value ? (string)null : (string)dataReader["DocConUEN"];
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
            _DocContractNum = dataReader["DocContractNum"] == DBNull.Value ? (string)null : (string)dataReader["DocContractNum"];
            _DocParcelNum = dataReader["DocParcelNum"] == DBNull.Value ? (string)null : (string)dataReader["DocParcelNum"];
            _DocShippingMark = dataReader["DocShippingMark"] == DBNull.Value ? (string)null : (string)dataReader["DocShippingMark"];
            _DocFaceMark = dataReader["DocFaceMark"] == DBNull.Value ? (string)null : (string)dataReader["DocFaceMark"];
            _DocEndMarking = dataReader["DocEndMarking"] == DBNull.Value ? (string)null : (string)dataReader["DocEndMarking"];
            _DocPackingTypeKey = dataReader["DocPackingTypeKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocPackingTypeKey"];
            _DocWeightUOMKey = dataReader["DocWeightUOMKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocWeightUOMKey"];
            _DocWeightUOMRate = dataReader["DocWeightUOMRate"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocWeightUOMRate"];
            _DocTLWeightUOMKey = dataReader["DocTLWeightUOMKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocTLWeightUOMKey"];
            _DocTLWeightUOMRate = dataReader["DocTLWeightUOMRate"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocTLWeightUOMRate"];
            _DocMeasUOMKey = dataReader["DocMeasUOMKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocMeasUOMKey"];
            _DocTLMeasUOMKey = dataReader["DocTLMeasUOMKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocTLMeasUOMKey"];
            _DocTLMeasUOMRate = dataReader["DocTLMeasUOMRate"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocTLMeasUOMRate"];
            _DocHC1 = dataReader["DocHC1"] == DBNull.Value ? (string)null : (string)dataReader["DocHC1"];
            _DocHC2 = dataReader["DocHC2"] == DBNull.Value ? (string)null : (string)dataReader["DocHC2"];
            _DocHC3 = dataReader["DocHC3"] == DBNull.Value ? (string)null : (string)dataReader["DocHC3"];
            _DocHC4 = dataReader["DocHC4"] == DBNull.Value ? (string)null : (string)dataReader["DocHC4"];
            _DocHC5 = dataReader["DocHC5"] == DBNull.Value ? (string)null : (string)dataReader["DocHC5"];
            _DocHT1 = dataReader["DocHT1"] == DBNull.Value ? (string)null : (string)dataReader["DocHT1"];
            _DocHT2 = dataReader["DocHT2"] == DBNull.Value ? (string)null : (string)dataReader["DocHT2"];
            _DocHT3 = dataReader["DocHT3"] == DBNull.Value ? (string)null : (string)dataReader["DocHT3"];
            _DocHT4 = dataReader["DocHT4"] == DBNull.Value ? (string)null : (string)dataReader["DocHT4"];
            _DocHT5 = dataReader["DocHT5"] == DBNull.Value ? (string)null : (string)dataReader["DocHT5"];
            _DocFC1 = dataReader["DocFC1"] == DBNull.Value ? (string)null : (string)dataReader["DocFC1"];
            _DocFC2 = dataReader["DocFC2"] == DBNull.Value ? (string)null : (string)dataReader["DocFC2"];
            _DocFC3 = dataReader["DocFC3"] == DBNull.Value ? (string)null : (string)dataReader["DocFC3"];
            _DocFC4 = dataReader["DocFC4"] == DBNull.Value ? (string)null : (string)dataReader["DocFC4"];
            _DocFC5 = dataReader["DocFC5"] == DBNull.Value ? (string)null : (string)dataReader["DocFC5"];
            _DocFC6 = dataReader["DocFC6"] == DBNull.Value ? (string)null : (string)dataReader["DocFC6"];
            _DocFC7 = dataReader["DocFC7"] == DBNull.Value ? (string)null : (string)dataReader["DocFC7"];
            _DocFT1 = dataReader["DocFT1"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocFT1"];
            _DocFT2 = dataReader["DocFT2"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocFT2"];
            _DocFT3 = dataReader["DocFT3"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocFT3"];
            _DocFT4 = dataReader["DocFT4"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocFT4"];
            _DocFT5 = dataReader["DocFT5"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocFT5"];
            _DocFT6 = dataReader["DocFT6"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocFT6"];
            _DocFT7 = dataReader["DocFT7"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocFT7"];
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
            _DocConID = dataReader["DocConID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocConID"];
            _DefBAddrKey = dataReader["DefBAddrKey"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DefBAddrKey"];
            _DefSAddrKey = dataReader["DefSAddrKey"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DefSAddrKey"];

          

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
                cm.CommandText = "ARPL_AddUpdate";

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
                if (_DocContractNum == null)
                {
                    cm.Parameters.AddWithValue("@DocContractNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocContractNum", _DocContractNum);
                }
                if (_DocParcelNum == null)
                {
                    cm.Parameters.AddWithValue("@DocParcelNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocParcelNum", _DocParcelNum);
                }
                if (_DocShippingMark == null)
                {
                    cm.Parameters.AddWithValue("@DocShippingMark", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocShippingMark", _DocShippingMark);
                }
                if (_DocFaceMark == null)
                {
                    cm.Parameters.AddWithValue("@DocFaceMark", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFaceMark", _DocFaceMark);
                }
                if (_DocEndMarking == null)
                {
                    cm.Parameters.AddWithValue("@DocEndMarking", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocEndMarking", _DocEndMarking);
                }
                if (_DocPackingTypeKey == null)
                {
                    cm.Parameters.AddWithValue("@DocPackingTypeKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPackingTypeKey", _DocPackingTypeKey);
                }
                if (_DocWeightUOMKey == null)
                {
                    cm.Parameters.AddWithValue("@DocWeightUOMKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocWeightUOMKey", _DocWeightUOMKey);
                }
                if (_DocWeightUOMRate == null)
                {
                    cm.Parameters.AddWithValue("@DocWeightUOMRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocWeightUOMRate", _DocWeightUOMRate);
                }
                if (_DocTLWeightUOMKey == null)
                {
                    cm.Parameters.AddWithValue("@DocTLWeightUOMKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTLWeightUOMKey", _DocTLWeightUOMKey);
                }
                if (_DocTLWeightUOMRate == null)
                {
                    cm.Parameters.AddWithValue("@DocTLWeightUOMRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTLWeightUOMRate", _DocTLWeightUOMRate);
                }
                if (_DocMeasUOMKey == null)
                {
                    cm.Parameters.AddWithValue("@DocMeasUOMKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMeasUOMKey", _DocMeasUOMKey);
                }
                if (_DocTLMeasUOMKey == null)
                {
                    cm.Parameters.AddWithValue("@DocTLMeasUOMKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTLMeasUOMKey", _DocTLMeasUOMKey);
                }
                if (_DocTLMeasUOMRate == null)
                {
                    cm.Parameters.AddWithValue("@DocTLMeasUOMRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTLMeasUOMRate", _DocTLMeasUOMRate);
                }
                if (_DocHC1 == null)
                {
                    cm.Parameters.AddWithValue("@DocHC1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHC1", _DocHC1);
                }
                if (_DocHC2 == null)
                {
                    cm.Parameters.AddWithValue("@DocHC2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHC2", _DocHC2);
                }
                if (_DocHC3 == null)
                {
                    cm.Parameters.AddWithValue("@DocHC3", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHC3", _DocHC3);
                }
                if (_DocHC4 == null)
                {
                    cm.Parameters.AddWithValue("@DocHC4", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHC4", _DocHC4);
                }
                if (_DocHC5 == null)
                {
                    cm.Parameters.AddWithValue("@DocHC5", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHC5", _DocHC5);
                }
                if (_DocHT1 == null)
                {
                    cm.Parameters.AddWithValue("@DocHT1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHT1", _DocHT1);
                }
                if (_DocHT2 == null)
                {
                    cm.Parameters.AddWithValue("@DocHT2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHT2", _DocHT2);
                }
                if (_DocHT3 == null)
                {
                    cm.Parameters.AddWithValue("@DocHT3", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHT3", _DocHT3);
                }
                if (_DocHT4 == null)
                {
                    cm.Parameters.AddWithValue("@DocHT4", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHT4", _DocHT4);
                }
                if (_DocHT5 == null)
                {
                    cm.Parameters.AddWithValue("@DocHT5", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHT5", _DocHT5);
                }
                if (_DocFC1 == null)
                {
                    cm.Parameters.AddWithValue("@DocFC1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFC1", _DocFC1);
                }
                if (_DocFC2 == null)
                {
                    cm.Parameters.AddWithValue("@DocFC2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFC2", _DocFC2);
                }
                if (_DocFC3 == null)
                {
                    cm.Parameters.AddWithValue("@DocFC3", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFC3", _DocFC3);
                }
                if (_DocFC4 == null)
                {
                    cm.Parameters.AddWithValue("@DocFC4", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFC4", _DocFC4);
                }
                if (_DocFC5 == null)
                {
                    cm.Parameters.AddWithValue("@DocFC5", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFC5", _DocFC5);
                }
                if (_DocFC6 == null)
                {
                    cm.Parameters.AddWithValue("@DocFC6", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFC6", _DocFC6);
                }
                if (_DocFC7 == null)
                {
                    cm.Parameters.AddWithValue("@DocFC7", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFC7", _DocFC7);
                }
                if (_DocFT1 == null)
                {
                    cm.Parameters.AddWithValue("@DocFT1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFT1", _DocFT1);
                }
                if (_DocFT2 == null)
                {
                    cm.Parameters.AddWithValue("@DocFT2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFT2", _DocFT2);
                }
                if (_DocFT3 == null)
                {
                    cm.Parameters.AddWithValue("@DocFT3", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFT3", _DocFT3);
                }
                if (_DocFT4 == null)
                {
                    cm.Parameters.AddWithValue("@DocFT4", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFT4", _DocFT4);
                }
                if (_DocFT5 == null)
                {
                    cm.Parameters.AddWithValue("@DocFT5", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFT5", _DocFT5);
                }
                if (_DocFT6 == null)
                {
                    cm.Parameters.AddWithValue("@DocFT6", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFT6", _DocFT6);
                }
                if (_DocFT7 == null)
                {
                    cm.Parameters.AddWithValue("@DocFT7", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFT7", _DocFT7);
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
                cm.CommandText = "ARPL_AddUpdate";

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
                if (_DocContractNum == null)
                {
                    cm.Parameters.AddWithValue("@DocContractNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocContractNum", _DocContractNum);
                }
                if (_DocParcelNum == null)
                {
                    cm.Parameters.AddWithValue("@DocParcelNum", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocParcelNum", _DocParcelNum);
                }
                if (_DocShippingMark == null)
                {
                    cm.Parameters.AddWithValue("@DocShippingMark", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocShippingMark", _DocShippingMark);
                }
                if (_DocFaceMark == null)
                {
                    cm.Parameters.AddWithValue("@DocFaceMark", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFaceMark", _DocFaceMark);
                }
                if (_DocEndMarking == null)
                {
                    cm.Parameters.AddWithValue("@DocEndMarking", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocEndMarking", _DocEndMarking);
                }
                if (_DocPackingTypeKey == null)
                {
                    cm.Parameters.AddWithValue("@DocPackingTypeKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPackingTypeKey", _DocPackingTypeKey);
                }
                if (_DocWeightUOMKey == null)
                {
                    cm.Parameters.AddWithValue("@DocWeightUOMKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocWeightUOMKey", _DocWeightUOMKey);
                }
                if (_DocWeightUOMRate == null)
                {
                    cm.Parameters.AddWithValue("@DocWeightUOMRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocWeightUOMRate", _DocWeightUOMRate);
                }
                if (_DocTLWeightUOMKey == null)
                {
                    cm.Parameters.AddWithValue("@DocTLWeightUOMKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTLWeightUOMKey", _DocTLWeightUOMKey);
                }
                if (_DocTLWeightUOMRate == null)
                {
                    cm.Parameters.AddWithValue("@DocTLWeightUOMRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTLWeightUOMRate", _DocTLWeightUOMRate);
                }
                if (_DocMeasUOMKey == null)
                {
                    cm.Parameters.AddWithValue("@DocMeasUOMKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMeasUOMKey", _DocMeasUOMKey);
                }
                if (_DocTLMeasUOMKey == null)
                {
                    cm.Parameters.AddWithValue("@DocTLMeasUOMKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTLMeasUOMKey", _DocTLMeasUOMKey);
                }
                if (_DocTLMeasUOMRate == null)
                {
                    cm.Parameters.AddWithValue("@DocTLMeasUOMRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTLMeasUOMRate", _DocTLMeasUOMRate);
                }
                if (_DocHC1 == null)
                {
                    cm.Parameters.AddWithValue("@DocHC1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHC1", _DocHC1);
                }
                if (_DocHC2 == null)
                {
                    cm.Parameters.AddWithValue("@DocHC2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHC2", _DocHC2);
                }
                if (_DocHC3 == null)
                {
                    cm.Parameters.AddWithValue("@DocHC3", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHC3", _DocHC3);
                }
                if (_DocHC4 == null)
                {
                    cm.Parameters.AddWithValue("@DocHC4", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHC4", _DocHC4);
                }
                if (_DocHC5 == null)
                {
                    cm.Parameters.AddWithValue("@DocHC5", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHC5", _DocHC5);
                }
                if (_DocHT1 == null)
                {
                    cm.Parameters.AddWithValue("@DocHT1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHT1", _DocHT1);
                }
                if (_DocHT2 == null)
                {
                    cm.Parameters.AddWithValue("@DocHT2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHT2", _DocHT2);
                }
                if (_DocHT3 == null)
                {
                    cm.Parameters.AddWithValue("@DocHT3", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHT3", _DocHT3);
                }
                if (_DocHT4 == null)
                {
                    cm.Parameters.AddWithValue("@DocHT4", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHT4", _DocHT4);
                }
                if (_DocHT5 == null)
                {
                    cm.Parameters.AddWithValue("@DocHT5", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocHT5", _DocHT5);
                }
                if (_DocFC1 == null)
                {
                    cm.Parameters.AddWithValue("@DocFC1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFC1", _DocFC1);
                }
                if (_DocFC2 == null)
                {
                    cm.Parameters.AddWithValue("@DocFC2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFC2", _DocFC2);
                }
                if (_DocFC3 == null)
                {
                    cm.Parameters.AddWithValue("@DocFC3", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFC3", _DocFC3);
                }
                if (_DocFC4 == null)
                {
                    cm.Parameters.AddWithValue("@DocFC4", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFC4", _DocFC4);
                }
                if (_DocFC5 == null)
                {
                    cm.Parameters.AddWithValue("@DocFC5", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFC5", _DocFC5);
                }
                if (_DocFC6 == null)
                {
                    cm.Parameters.AddWithValue("@DocFC6", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFC6", _DocFC6);
                }
                if (_DocFC7 == null)
                {
                    cm.Parameters.AddWithValue("@DocFC7", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFC7", _DocFC7);
                }
                if (_DocFT1 == null)
                {
                    cm.Parameters.AddWithValue("@DocFT1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFT1", _DocFT1);
                }
                if (_DocFT2 == null)
                {
                    cm.Parameters.AddWithValue("@DocFT2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFT2", _DocFT2);
                }
                if (_DocFT3 == null)
                {
                    cm.Parameters.AddWithValue("@DocFT3", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFT3", _DocFT3);
                }
                if (_DocFT4 == null)
                {
                    cm.Parameters.AddWithValue("@DocFT4", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFT4", _DocFT4);
                }
                if (_DocFT5 == null)
                {
                    cm.Parameters.AddWithValue("@DocFT5", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFT5", _DocFT5);
                }
                if (_DocFT6 == null)
                {
                    cm.Parameters.AddWithValue("@DocFT6", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFT6", _DocFT6);
                }
                if (_DocFT7 == null)
                {
                    cm.Parameters.AddWithValue("@DocFT7", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocFT7", _DocFT7);
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
                cm.CommandText = "ARPL_Delete";

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
                    cm.CommandText = "ARPL_Validation";

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
            this._DocContractNum = null;
            this._DocParcelNum = null;
            this._DocShippingMark = null;
            this._DocFaceMark = null;
            this._DocEndMarking = null;
            this._DocPackingTypeKey = 0;
            this._DocWeightUOMKey = 0;
            this._DocWeightUOMRate = 0;
            this._DocTLWeightUOMKey = 0;
            this._DocTLWeightUOMRate = 0;
            this._DocMeasUOMKey = 0;
            this._DocTLMeasUOMKey = 0;
            this._DocTLMeasUOMRate = 0;
            this._DocHC1 = null;
            this._DocHC2 = null;
            this._DocHC3 = null;
            this._DocHC4 = null;
            this._DocHC5 = null;
            this._DocHT1 = null;
            this._DocHT2 = null;
            this._DocHT3 = null;
            this._DocHT4 = null;
            this._DocHT5 = null;
            this._DocFC1 = null;
            this._DocFC2 = null;
            this._DocFC3 = null;
            this._DocFC4 = null;
            this._DocFC5 = null;
            this._DocFC6 = null;
            this._DocFC7 = null;
            this._DocFT1 = 0;
            this._DocFT2 = 0;
            this._DocFT3 = 0;
            this._DocFT4 = 0;
            this._DocFT5 = 0;
            this._DocFT6 = 0;
            this._DocFT7 = 0;
            this._DocConID = string.Empty;
            this.attachments = new SYSAttachments();
        }
    }
}





