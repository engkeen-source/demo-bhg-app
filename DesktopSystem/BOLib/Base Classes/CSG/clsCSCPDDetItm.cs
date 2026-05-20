


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for CSCPDDetItm.
    /// </summary>
    [Serializable]
    public class CSCPDDetItm : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        
        protected decimal? _ItmQtyLink;
        protected decimal? _ItmQtyBalance;
        protected decimal? _ItmPriceBefore;
        protected decimal? _ItmPriceAfter;
        protected string _CSCPOID;
        protected int _CSCPODK;
        protected int _CSCPODItm;
        protected string _CSCPSID;
        protected int _CSCPSDK;
        protected int _CSCPSDItm;
        protected string _ItmID;
        protected string _SKU1;
        protected string _SKU2;
        protected decimal? _ItmLatestCostShw;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public CSCPDDetItm()
            : base()
        {
            this._ItmQtyLink = null;
            this._ItmQtyBalance = 0;
            this._ItmPriceBefore = null;
            this._ItmPriceAfter = null;
            this._CSCPOID = null;
            this._CSCPODK = 0;
            this._CSCPODItm = 0;
            this._CSCPSID = null;
            this._CSCPSDK = 0;
            this._CSCPSDItm = 0;
            this._ItmID = string.Empty;
            this._SKU1 = string.Empty;
            this._SKU2 = string.Empty;
            this._ItmLatestCostShw = 0;

        }


        public CSCPDDetItm Clone()
        {
            CSCPDDetItm objCopy = (CSCPDDetItm)this.MemberwiseClone();
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
        public string CSCPOID
        {

            get
            {
                return this._CSCPOID;
            }
            set
            {
                this._CSCPOID = value;
                NotifyPropertyChanged("CSCPOID");
            }
        }
        public int CSCPODK
        {

            get
            {
                return this._CSCPODK;
            }
            set
            {
                this._CSCPODK = value;
                NotifyPropertyChanged("CSCPODK");
            }
        }
        public int CSCPODItm
        {

            get
            {
                return this._CSCPODItm;
            }
            set
            {
                this._CSCPODItm = value;
                NotifyPropertyChanged("CSCPODItm");
            }
        }
        public string CSCPSID
        {

            get
            {
                return this._CSCPSID;
            }
            set
            {
                this._CSCPSID = value;
                NotifyPropertyChanged("CSCPSID");
            }
        }
        public int CSCPSDK
        {

            get
            {
                return this._CSCPSDK;
            }
            set
            {
                this._CSCPSDK = value;
                NotifyPropertyChanged("CSCPSDK");
            }
        }
        public int CSCPSDItm
        {

            get
            {
                return this._CSCPSDItm;
            }
            set
            {
                this._CSCPSDItm = value;
                NotifyPropertyChanged("CSCPSDItm");
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





