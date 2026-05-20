using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for APPODetItm.
    /// </summary>
    [Serializable]
    public class APPODetItm : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    { 
        #region +++  Local variables declaration for the class +++
        protected int _LineType;
        protected int _LineLinkKey;
        protected int _ItmDeptKey;
        protected int? _ItmTranGrpKey;
        protected int? _ItmAccKey;
        protected decimal? _ItmQtyLink;
        protected decimal? _ItmQtyAdj;
        protected decimal? _ItmQtyDelivered;
        protected decimal? _ItmQtyBalance;
        protected int? _ItmOrderStatus;
        protected DateTime? _ItmReqDate;
        protected DateTime? _ItmPrmDate;
        protected decimal? _ItmPriceBefore;
        protected decimal? _ItmPriceAfter;
        protected bool _ItmTaxable;
        protected int? _ItmTaxGrpKey;
        protected decimal? _ItmTaxGrpRate;
        protected decimal? _ItmTaxGrpAmtF;
        protected decimal? _ItmTaxGrpAmtL;
        protected bool _ItmHide;
        protected string _ConfirmID;
        protected string _ConfirmSN;
        protected int? _ItmBatchKey;
        protected decimal? _ItmBatchQty;
        protected string _NSLink;
        protected string _ARQOID;
        protected int _ARQODK;
        protected int _ARQODItm;
        protected int _ARSODK;
        protected int _ARSODItm;
        protected string _ARSOPOID;
        protected string _ARDOID;
        protected int _ARDODK;
        protected int _ARDODItm;
        protected string _ARIVID;
        protected int _ARIVDK;
        protected int _ARIVDItm;
        protected string _ARSOID;
        protected string _ItmID;
        protected string _ItmAccID;
        protected string _ItmAccDes;
        protected string _SKU1;
        protected string _SKU2;
        protected decimal? _ItmLatestCostShw;
        protected string _HSCode;
        protected string _CountryID;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public APPODetItm()
            : base()
        {
            this._LineType = 0;
            this._LineLinkKey = 0;
            this._ItmDeptKey = 0;
            this._ItmTranGrpKey = 0;
            this._ItmAccKey = null;
            this._ItmQtyLink = null;
            this._ItmQtyAdj = null;
            this._ItmQtyDelivered = 0;
            this._ItmQtyBalance = 0;
            this._ItmOrderStatus = 0;
            this._ItmReqDate = DateTime.Today.Date;
            this._ItmPrmDate = DateTime.Today.Date;
            this._ItmPriceBefore = null;
            this._ItmPriceAfter = null;
            this._ItmTaxable = false;
            this._ItmTaxGrpKey = null;
            this._ItmTaxGrpRate = 0;
            this._ItmTaxGrpAmtF = 0;
            this._ItmTaxGrpAmtL = 0;
            this._ItmHide = false;
            this._ConfirmID = null;
            this._ConfirmSN = null;
            this._ItmBatchKey = 0;
            this._ItmBatchQty = 0;
            this._NSLink = string.Empty;
            this._ARQOID = null;
            this._ARQODK = 0;
            this._ARQODItm = 0;
            this._ARSODK = 0;
            this._ARSODItm = 0;
            this._ARSOPOID = null;
            this._ARDOID = null;
            this._ARDODK = 0;
            this._ARDODItm = 0;
            this._ARIVID = null;
            this._ARIVDK = 0;
            this._ARIVDItm = 0;
            this._ARSOID = null;
            this._ItmID = string.Empty;
            this._ItmAccID = string.Empty;
            this._ItmAccDes = string.Empty;
            this._SKU1 = string.Empty;
            this._SKU2 = string.Empty;
            this._ItmLatestCostShw = 0;
            this._HSCode = string.Empty;
            this._CountryID = string.Empty;

        }


        public APPODetItm Clone()
        {
            APPODetItm objCopy = (APPODetItm)this.MemberwiseClone();
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
        public decimal? ItmQtyLink
        {

            get
            {
                return this._ItmQtyLink;
            }
            set
            {
                this._ItmQtyLink = value;
                NotifyPropertyChanged("ItmQtyLink");
            }
        }
        public decimal? ItmQtyAdj
        {

            get
            {
                return this._ItmQtyAdj;
            }
            set
            {
                this._ItmQtyAdj = value;
                NotifyPropertyChanged("ItmQtyAdj");
            }
        }
        public decimal? ItmQtyDelivered
        {

            get
            {
                return this._ItmQtyDelivered;
            }
            set
            {
                this._ItmQtyDelivered = value;
                NotifyPropertyChanged("ItmQtyDelivered");
            }
        }
        public decimal? ItmQtyBalance
        {

            get
            {
                return this._ItmQtyBalance;
            }
            set
            {
                this._ItmQtyBalance = value;
                NotifyPropertyChanged("ItmQtyBalance");
            }
        }
        public int? ItmOrderStatus
        {

            get
            {
                return this._ItmOrderStatus;
            }
            set
            {
                this._ItmOrderStatus = value;
                NotifyPropertyChanged("ItmOrderStatus");
            }
        }
        public DateTime? ItmReqDate
        {

            get
            {
                return this._ItmReqDate;
            }
            set
            {
                this._ItmReqDate = value;
                NotifyPropertyChanged("ItmReqDate");
            }
        }
        public DateTime? ItmPrmDate
        {

            get
            {
                return this._ItmPrmDate;
            }
            set
            {
                this._ItmPrmDate = value;
                NotifyPropertyChanged("ItmPrmDate");
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
        public string ConfirmID
        {

            get
            {
                return this._ConfirmID;
            }
            set
            {
                this._ConfirmID = value;
                NotifyPropertyChanged("ConfirmID");
            }
        }
        public string ConfirmSN
        {

            get
            {
                return this._ConfirmSN;
            }
            set
            {
                this._ConfirmSN = value;
                NotifyPropertyChanged("ConfirmSN");
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
        public string ARDOID
        {

            get
            {
                return this._ARDOID;
            }
            set
            {
                this._ARDOID = value;
                NotifyPropertyChanged("ARDOID");
            }
        }
        public int ARDODK
        {

            get
            {
                return this._ARDODK;
            }
            set
            {
                this._ARDODK = value;
                NotifyPropertyChanged("ARDODK");
            }
        }
        public int ARDODItm
        {

            get
            {
                return this._ARDODItm;
            }
            set
            {
                this._ARDODItm = value;
                NotifyPropertyChanged("ARDODItm");
            }
        }
        public string ARIVID
        {

            get
            {
                return this._ARIVID;
            }
            set
            {
                this._ARIVID = value;
                NotifyPropertyChanged("ARIVID");
            }
        }
        public int ARIVDK
        {

            get
            {
                return this._ARIVDK;
            }
            set
            {
                this._ARIVDK = value;
                NotifyPropertyChanged("ARIVDK");
            }
        }
        public int ARIVDItm
        {

            get
            {
                return this._ARIVDItm;
            }
            set
            {
                this._ARIVDItm = value;
                NotifyPropertyChanged("ARIVDItm");
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
        #endregion
    }
}
