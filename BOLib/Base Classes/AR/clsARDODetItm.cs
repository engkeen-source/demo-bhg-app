


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for ARDODetItm.
    /// </summary>
    [Serializable]
    public class ARDODetItm : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected int _LineType;
        protected int _LineLinkKey;
        protected int _ItmDeptKey;
        protected int? _ItmTranGrpKey;
        protected int? _ItmAccKey;
        protected decimal? _ItmPriceBefore;
        protected decimal? _ItmPriceAfter;
        protected decimal? _ItmControlPrice;
        protected decimal? _ItmControlPriceBase;
        protected bool _ItmTaxable;
        protected int? _ItmTaxGrpKey;
        protected decimal? _ItmTaxGrpRate;
        protected decimal? _ItmTaxGrpAmtF;
        protected decimal? _ItmTaxGrpAmtL;
        protected bool _ItmHide;
        protected int? _ItmVendorKey;
        protected int _ItmVendorCurrKey;
        protected float _ItmVendorCurrRate;
        protected decimal _ItmVendorPrice;
        protected decimal _ItmVendorPriceRatio;
        protected bool _ItmVendorPriceLock;
        protected int _ItmIGrpDItm;
        protected bool _ItmIGrpQtyLock;
        protected bool _ItmIGrpToPrint;
        protected decimal _ItmIGrpQtySet;
        protected decimal _ItmIGrpAmtSet;
        protected int? _ItmBatchKey;
        protected decimal? _ItmBatchQty;
        protected string _NSLink;
        protected string _ARQOID;
        protected int _ARQODK;
        protected int _ARQODItm;
        protected string _ARSOID;
        protected int _ARSODK;
        protected int _ARSODItm;
        protected string _ARSOPOID;
        protected string _ItmID;
        protected string _ItmAccID;
        protected string _ItmAccDes;
        protected string _ItmVendorID;
        protected string _ItmVendorNm;
        protected string _SKU1;
        protected string _SKU2;
        protected decimal? _ItmLatestCostShw;
        protected string _APPOID;
        protected decimal? _APPOSN;
        protected string _HSCode;
        protected string _CountryID;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public ARDODetItm()
            : base()
        {
            this._LineType = 0;
            this._LineLinkKey = 0;
            this._ItmDeptKey = 0;
            this._ItmTranGrpKey = 0;
            this._ItmAccKey = null;
            this._ItmPriceBefore = null;
            this._ItmPriceAfter = null;
            this._ItmControlPrice = null;
            this._ItmControlPriceBase = null;
            this._ItmTaxable = false;
            this._ItmTaxGrpKey = null;
            this._ItmTaxGrpRate = 0;
            this._ItmTaxGrpAmtF = 0;
            this._ItmTaxGrpAmtL = 0;
            this._ItmHide = false;
            this._ItmVendorKey = null;
            this._ItmVendorCurrKey = 0;
            this._ItmVendorCurrRate = 0;
            this._ItmVendorPrice = 0;
            this._ItmVendorPriceRatio = 0;
            this._ItmVendorPriceLock = false;
            this._ItmIGrpDItm = 0;
            this._ItmIGrpQtyLock = false;
            this._ItmIGrpToPrint = false;
            this._ItmIGrpQtySet = 0;
            this._ItmIGrpAmtSet = 0;
            this._ItmBatchKey = 0;
            this._ItmBatchQty = 0;
            this._NSLink = string.Empty;
            this._ARQOID = null;
            this._ARQODK = 0;
            this._ARQODItm = 0;
            this._ARSOID = null;
            this._ARSODK = 0;
            this._ARSODItm = 0;
            this._ARSOPOID = null;
            this._ItmID = string.Empty;
            this._HSCode = string.Empty;
            this._ItmAccID = string.Empty;
            this._ItmAccDes = string.Empty;
            this._ItmVendorID = string.Empty;
            this._ItmVendorNm = string.Empty;
            this._SKU1 = string.Empty;
            this._SKU2 = string.Empty;
            this._ItmLatestCostShw = 0;
            this._CountryID = string.Empty;
        }


        public ARDODetItm Clone()
        {
            ARDODetItm objCopy = (ARDODetItm)this.MemberwiseClone();
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

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


        public int LineType
        {

            get
            {
                return this._LineType;
            }
            set
            {
                this._LineType = value;
                NotifyPropertyChanged("LineType");
            }
        }
        public int LineLinkKey
        {

            get
            {
                return this._LineLinkKey;
            }
            set
            {
                this._LineLinkKey = value;
                NotifyPropertyChanged("LineLinkKey");
            }
        }
        public int ItmDeptKey
        {

            get
            {
                return this._ItmDeptKey;
            }
            set
            {
                this._ItmDeptKey = value;
                NotifyPropertyChanged("ItmDeptKey");
            }
        }
        public int? ItmTranGrpKey
        {

            get
            {
                return this._ItmTranGrpKey;
            }
            set
            {
                this._ItmTranGrpKey = value;
                NotifyPropertyChanged("ItmTranGrpKey");
            }
        }
        public int? ItmAccKey
        {

            get
            {
                return this._ItmAccKey;
            }
            set
            {
                this._ItmAccKey = value;
                NotifyPropertyChanged("ItmAccKey");
            }
        }
        public decimal? ItmPriceBefore
        {

            get
            {
                return this._ItmPriceBefore;
            }
            set
            {
                this._ItmPriceBefore = value;
                NotifyPropertyChanged("ItmPriceBefore");
            }
        }
        public decimal? ItmPriceAfter
        {

            get
            {
                return this._ItmPriceAfter;
            }
            set
            {
                this._ItmPriceAfter = value;
                NotifyPropertyChanged("ItmPriceAfter");
            }
        }
        public decimal? ItmControlPrice
        {

            get
            {
                return this._ItmControlPrice;
            }
            set
            {
                this._ItmControlPrice = value;
                NotifyPropertyChanged("ItmControlPrice");
            }
        }
        public decimal? ItmControlPriceBase
        {

            get
            {
                return this._ItmControlPriceBase;
            }
            set
            {
                this._ItmControlPriceBase = value;
                NotifyPropertyChanged("ItmControlPriceBase");
            }
        }
        public bool ItmTaxable
        {

            get
            {
                return this._ItmTaxable;
            }
            set
            {
                this._ItmTaxable = value;
                NotifyPropertyChanged("ItmTaxable");
            }
        }
        public int? ItmTaxGrpKey
        {

            get
            {
                return this._ItmTaxGrpKey;
            }
            set
            {
                this._ItmTaxGrpKey = value;
                NotifyPropertyChanged("ItmTaxGrpKey");
            }
        }
        public decimal? ItmTaxGrpRate
        {

            get
            {
                return this._ItmTaxGrpRate;
            }
            set
            {
                this._ItmTaxGrpRate = value;
                NotifyPropertyChanged("ItmTaxGrpRate");
            }
        }
        public decimal? ItmTaxGrpAmtF
        {

            get
            {
                return this._ItmTaxGrpAmtF;
            }
            set
            {
                this._ItmTaxGrpAmtF = value;
                NotifyPropertyChanged("ItmTaxGrpAmtF");
            }
        }
        public decimal? ItmTaxGrpAmtL
        {

            get
            {
                return this._ItmTaxGrpAmtL;
            }
            set
            {
                this._ItmTaxGrpAmtL = value;
                NotifyPropertyChanged("ItmTaxGrpAmtL");
            }
        }
        public bool ItmHide
        {

            get
            {
                return this._ItmHide;
            }
            set
            {
                this._ItmHide = value;
                NotifyPropertyChanged("ItmHide");
            }
        }
        public int? ItmVendorKey
        {

            get
            {
                return this._ItmVendorKey;
            }
            set
            {
                this._ItmVendorKey = value;
                NotifyPropertyChanged("ItmVendorKey");
            }
        }
        public int ItmVendorCurrKey
        {

            get
            {
                return this._ItmVendorCurrKey;
            }
            set
            {
                this._ItmVendorCurrKey = value;
                NotifyPropertyChanged("ItmVendorCurrKey");
            }
        }
        public float ItmVendorCurrRate
        {

            get
            {
                return this._ItmVendorCurrRate;
            }
            set
            {
                this._ItmVendorCurrRate = value;
                NotifyPropertyChanged("ItmVendorCurrRate");
            }
        }
        public decimal ItmVendorPrice
        {

            get
            {
                return this._ItmVendorPrice;
            }
            set
            {
                this._ItmVendorPrice = value;
                NotifyPropertyChanged("ItmVendorPrice");
            }
        }
        public decimal ItmVendorPriceRatio
        {

            get
            {
                return this._ItmVendorPriceRatio;
            }
            set
            {
                this._ItmVendorPriceRatio = value;
                NotifyPropertyChanged("ItmVendorPriceRatio");
            }
        }
        public bool ItmVendorPriceLock
        {

            get
            {
                return this._ItmVendorPriceLock;
            }
            set
            {
                this._ItmVendorPriceLock = value;
                NotifyPropertyChanged("ItmVendorPriceLock");
            }
        }
        public int ItmIGrpDItm
        {

            get
            {
                return this._ItmIGrpDItm;
            }
            set
            {
                this._ItmIGrpDItm = value;
                NotifyPropertyChanged("ItmIGrpDItm");
            }
        }
        public bool ItmIGrpQtyLock
        {

            get
            {
                return this._ItmIGrpQtyLock;
            }
            set
            {
                this._ItmIGrpQtyLock = value;
                NotifyPropertyChanged("ItmIGrpQtyLock");
            }
        }
        public bool ItmIGrpToPrint
        {

            get
            {
                return this._ItmIGrpToPrint;
            }
            set
            {
                this._ItmIGrpToPrint = value;
                NotifyPropertyChanged("ItmIGrpToPrint");
            }
        }
        public decimal ItmIGrpQtySet
        {

            get
            {
                return this._ItmIGrpQtySet;
            }
            set
            {
                this._ItmIGrpQtySet = value;
                NotifyPropertyChanged("ItmIGrpQtySet");
            }
        }
        public decimal ItmIGrpAmtSet
        {

            get
            {
                return this._ItmIGrpAmtSet;
            }
            set
            {
                this._ItmIGrpAmtSet = value;
                NotifyPropertyChanged("ItmIGrpAmtSet");
            }
        }
        public int? ItmBatchKey
        {

            get
            {
                return this._ItmBatchKey;
            }
            set
            {
                this._ItmBatchKey = value;
                NotifyPropertyChanged("ItmBatchKey");
            }
        }
        public decimal? ItmBatchQty
        {

            get
            {
                return this._ItmBatchQty;
            }
            set
            {
                this._ItmBatchQty = value;
                NotifyPropertyChanged("ItmBatchQty");
            }
        }
        public string NSLink
        {

            get
            {
                return this._NSLink;
            }
            set
            {
                this._NSLink = value;
                NotifyPropertyChanged("NSLink");
            }
        }
        public string ARQOID
        {

            get
            {
                return this._ARQOID;
            }
            set
            {
                this._ARQOID = value;
                NotifyPropertyChanged("ARQOID");
            }
        }
        public int ARQODK
        {

            get
            {
                return this._ARQODK;
            }
            set
            {
                this._ARQODK = value;
                NotifyPropertyChanged("ARQODK");
            }
        }
        public int ARQODItm
        {

            get
            {
                return this._ARQODItm;
            }
            set
            {
                this._ARQODItm = value;
                NotifyPropertyChanged("ARQODItm");
            }
        }
        public string ARSOID
        {

            get
            {
                return this._ARSOID;
            }
            set
            {
                this._ARSOID = value;
                NotifyPropertyChanged("ARSOID");
            }
        }
        public int ARSODK
        {

            get
            {
                return this._ARSODK;
            }
            set
            {
                this._ARSODK = value;
                NotifyPropertyChanged("ARSODK");
            }
        }
        public int ARSODItm
        {

            get
            {
                return this._ARSODItm;
            }
            set
            {
                this._ARSODItm = value;
                NotifyPropertyChanged("ARSODItm");
            }
        }
        public string ARSOPOID
        {

            get
            {
                return this._ARSOPOID;
            }
            set
            {
                this._ARSOPOID = value;
                NotifyPropertyChanged("ARSOPOID");
            }
        }
        public string ItmID
        {

            get
            {
                return this._ItmID;
            }
            set
            {
                this._ItmID = value;
                NotifyPropertyChanged("ItmID");
            }
        }
        public string HSCode
        {

            get
            {
                return this._HSCode;
            }
            set
            {
                this._HSCode = value;
                NotifyPropertyChanged("HSCode");
            }
        }
        public string CountryID
        {

            get
            {
                return this._CountryID;
            }
            set
            {
                this._CountryID = value;
                NotifyPropertyChanged("CountryID");
            }
        }
        public string ItmAccID
        {

            get
            {
                return this._ItmAccID;
            }
            set
            {
                this._ItmAccID = value;
                NotifyPropertyChanged("ItmAccID");
            }
        }
        public string ItmAccDes
        {

            get
            {
                return this._ItmAccDes;
            }
            set
            {
                this._ItmAccDes = value;
                NotifyPropertyChanged("ItmAccDes");
            }
        }
        public string ItmVendorID
        {

            get
            {
                return this._ItmVendorID;
            }
            set
            {
                this._ItmVendorID = value;
                NotifyPropertyChanged("ItmVendorID");
            }
        }
        public string ItmVendorNm
        {

            get
            {
                return this._ItmVendorNm;
            }
            set
            {
                this._ItmVendorNm = value;
                NotifyPropertyChanged("ItmVendorNm");
            }
        }
        public string SKU1
        {

            get
            {
                return this._SKU1;
            }
            set
            {
                this._SKU1 = value;
                NotifyPropertyChanged("SKU1");
            }
        }
        public string SKU2
        {

            get
            {
                return this._SKU2;
            }
            set
            {
                this._SKU2 = value;
                NotifyPropertyChanged("SKU2");
            }
        }
        public decimal? ItmLatestCostShw
        {

            get
            {
                return this._ItmLatestCostShw;
            }
            set
            {
                this._ItmLatestCostShw = value;
                NotifyPropertyChanged("ItmLatestCostShw");
            }
        }

        public string APPOID
        {

            get
            {
                return this._APPOID;
            }
            set
            {
                this._APPOID = value;
                NotifyPropertyChanged("APPOID");
            }
        }
        public decimal? APPOSN
        {

            get
            {
                return this._APPOSN;
            }
            set
            {
                this._ItmLatestCostShw = value;
                NotifyPropertyChanged("APPOSN");
            }
        }

        #endregion

    }
}





