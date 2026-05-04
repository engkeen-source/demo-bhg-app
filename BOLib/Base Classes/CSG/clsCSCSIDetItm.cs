


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for CSCSIDetItm.
    /// </summary>
    [Serializable]
    public class CSCSIDetItm : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected int _LineType;
        protected int _LineLinkKey;
        protected int _ItmDeptKey;
        protected int? _ItmTranGrpKey;
        protected int _ItmFromLocKey;
        protected int _ItmToLocKey;
        protected int _ItmFromAccKey;
        protected int _ItmToAccKey;
        protected decimal _ItmQtyLink;
        protected decimal? _ItmQtyBalance;
        protected decimal? _ItmPriceBefore;
        protected decimal? _ItmPriceAfter;
        protected decimal _ItmControlPrice;
        protected decimal _ItmControlPriceBase;
        protected string _ARSOID;
        protected int _ARSODK;
        protected int _ARSODItm;
        protected string _ARSOPOID;
        protected string _CSCSIID;
        protected int _CSCSIDK;
        protected int _CSCSIDItm;
        protected string _ItmID;
        protected string _SKU1;
        protected string _SKU2;
        protected string _ItmFromAccID;
        protected string _ItmFromAccDes;
        protected string _ItmToAccID;
        protected string _ItmToAccDes;
        protected decimal? _ItmLatestCostShw;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public CSCSIDetItm()
            : base()
        {
            this._LineType = 0;
            this._LineLinkKey = 0;
            this._ItmDeptKey = 0;
            this._ItmTranGrpKey = 0;
            this._ItmFromLocKey = 0;
            this._ItmToLocKey = 0;
            this._ItmFromAccKey = 0;
            this._ItmToAccKey = 0;
            this._ItmQtyLink = 0;
            this._ItmQtyBalance = 0;
            this._ItmPriceBefore = null;
            this._ItmPriceAfter = null;
            this._ItmControlPrice = 0;
            this._ItmControlPriceBase = 0;
            this._ARSOID = null;
            this._ARSODK = 0;
            this._ARSODItm = 0;
            this._ARSOPOID = null;
            this._CSCSIID = null;
            this._CSCSIDK = 0;
            this._CSCSIDItm = 0;
            this._ItmID = string.Empty;
            this._SKU1 = string.Empty;
            this._SKU2 = string.Empty;
            this._ItmFromAccID = string.Empty;
            this._ItmFromAccDes = string.Empty;
            this._ItmToAccID = string.Empty;
            this._ItmToAccDes = string.Empty;
            this._ItmLatestCostShw = 0;

        }


        public CSCSIDetItm Clone()
        {
            CSCSIDetItm objCopy = (CSCSIDetItm)this.MemberwiseClone();
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
        public int ItmFromLocKey
        {

            get
            {
                return this._ItmFromLocKey;
            }
            set
            {
                this._ItmFromLocKey = value;
                NotifyPropertyChanged("ItmFromLocKey");
            }
        }
        public int ItmToLocKey
        {

            get
            {
                return this._ItmToLocKey;
            }
            set
            {
                this._ItmToLocKey = value;
                NotifyPropertyChanged("ItmToLocKey");
            }
        }
        public int ItmFromAccKey
        {

            get
            {
                return this._ItmFromAccKey;
            }
            set
            {
                this._ItmFromAccKey = value;
                NotifyPropertyChanged("ItmFromAccKey");
            }
        }
        public int ItmToAccKey
        {

            get
            {
                return this._ItmToAccKey;
            }
            set
            {
                this._ItmToAccKey = value;
                NotifyPropertyChanged("ItmToAccKey");
            }
        }
        public decimal ItmQtyLink
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
        public decimal ItmControlPrice
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
        public decimal ItmControlPriceBase
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
        public string CSCSIID
        {

            get
            {
                return this._CSCSIID;
            }
            set
            {
                this._CSCSIID = value;
                NotifyPropertyChanged("CSCSIID");
            }
        }
        public int CSCSIDK
        {

            get
            {
                return this._CSCSIDK;
            }
            set
            {
                this._CSCSIDK = value;
                NotifyPropertyChanged("CSCSIDK");
            }
        }
        public int CSCSIDItm
        {

            get
            {
                return this._CSCSIDItm;
            }
            set
            {
                this._CSCSIDItm = value;
                NotifyPropertyChanged("CSCSIDItm");
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
        public string ItmFromAccID
        {

            get
            {
                return this._ItmFromAccID;
            }
            set
            {
                this._ItmFromAccID = value;
                NotifyPropertyChanged("ItmFromAccID");
            }
        }
        public string ItmFromAccDes
        {

            get
            {
                return this._ItmFromAccDes;
            }
            set
            {
                this._ItmFromAccDes = value;
                NotifyPropertyChanged("ItmFromAccDes");
            }
        }
        public string ItmToAccID
        {

            get
            {
                return this._ItmToAccID;
            }
            set
            {
                this._ItmToAccID = value;
                NotifyPropertyChanged("ItmToAccID");
            }
        }
        public string ItmToAccDes
        {

            get
            {
                return this._ItmToAccDes;
            }
            set
            {
                this._ItmToAccDes = value;
                NotifyPropertyChanged("ItmToAccDes");
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


        #endregion
    }
}





