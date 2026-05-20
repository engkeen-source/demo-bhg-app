


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for CSCPODetItm.
    /// </summary>
    [Serializable]
    public class CSCPODetItm : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected decimal? _ItmQtyLink;
        protected decimal? _ItmQtyAdj;
        protected decimal? _ItmQtyDelivered;
        protected decimal? _ItmQtyBalance;
        protected DateTime? _ItmReqDate;
        protected DateTime? _ItmPrmDate;
        protected decimal? _ItmPriceBefore;
        protected decimal? _ItmPriceAfter;
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

        public CSCPODetItm()
            : base()
        {
            this._ItmQtyLink = null;
            this._ItmQtyAdj = null;
            this._ItmQtyDelivered = 0;
            this._ItmQtyBalance = 0;
            this._ItmReqDate = DateTime.Today.Date;
            this._ItmPrmDate = DateTime.Today.Date;
            this._ItmPriceBefore = null;
            this._ItmPriceAfter = null;
            this._ItmID = string.Empty;
            this._SKU1 = string.Empty;
            this._SKU2 = string.Empty;
            this._ItmLatestCostShw = 0;

        }


        public CSCPODetItm Clone()
        {
            CSCPODetItm objCopy = (CSCPODetItm)this.MemberwiseClone();
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





