


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for CSCPSDetItm.
    /// </summary>
    [Serializable]
    public class CSCPSDetItm : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected int _LineType;
        protected string _CPDID;
        protected int _CPDDK;
        protected int _CPDDItm;
        protected string _CPDRef;
        protected string _ItmUOMID;
        protected decimal? _ItmDisPrice;
        protected bool _FullPayment;
        protected int _SettlementDocDC;
        protected int _SettlementDocDK;
        protected int _SettlementDocDItm;
        protected string _SettlementDocID;
        protected DateTime? _SettlementDocDate;
        protected string _SettlementDocRef;
        protected string _SettlementDocDes;
        protected decimal? _SettlementItmQty;
        protected string _ItmID;
        protected string _SKU1;
        protected string _SKU2;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public CSCPSDetItm()
            : base()
        {
            this._LineType = 0;
            this._CPDID = null;
            this._CPDDK = 0;
            this._CPDDItm = 0;
            this._CPDRef = null;
            this._ItmUOMID = null;
            this._ItmDisPrice = null;
            this._FullPayment = false;
            this._SettlementDocDC = 0;
            this._SettlementDocDK = 0;
            this._SettlementDocDItm = 0;
            this._SettlementDocID = null;
            this._SettlementDocDate = DateTime.Today.Date;
            this._SettlementDocRef = null;
            this._SettlementDocDes = null;
            this._SettlementItmQty = null;
            this._ItmID = string.Empty;
            this._SKU1 = string.Empty;
            this._SKU2 = string.Empty;

        }


        public CSCPSDetItm Clone()
        {
            CSCPSDetItm objCopy = (CSCPSDetItm)this.MemberwiseClone();
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
        public string CPDID
        {

            get
            {
                return this._CPDID;
            }
            set
            {
                this._CPDID = value;
                NotifyPropertyChanged("CPDID");
            }
        }
        public int CPDDK
        {

            get
            {
                return this._CPDDK;
            }
            set
            {
                this._CPDDK = value;
                NotifyPropertyChanged("CPDDK");
            }
        }
        public int CPDDItm
        {

            get
            {
                return this._CPDDItm;
            }
            set
            {
                this._CPDDItm = value;
                NotifyPropertyChanged("CPDDItm");
            }
        }
        public string CPDRef
        {

            get
            {
                return this._CPDRef;
            }
            set
            {
                this._CPDRef = value;
                NotifyPropertyChanged("CPDRef");
            }
        }
        public string ItmUOMID
        {

            get
            {
                return this._ItmUOMID;
            }
            set
            {
                this._ItmUOMID = value;
                NotifyPropertyChanged("ItmUOMID");
            }
        }
        public decimal? ItmDisPrice
        {

            get
            {
                return this._ItmDisPrice;
            }
            set
            {
                this._ItmDisPrice = value;
                NotifyPropertyChanged("ItmDisPrice");
            }
        }
        public bool FullPayment
        {

            get
            {
                return this._FullPayment;
            }
            set
            {
                this._FullPayment = value;
                NotifyPropertyChanged("FullPayment");
            }
        }
        public int SettlementDocDC
        {

            get
            {
                return this._SettlementDocDC;
            }
            set
            {
                this._SettlementDocDC = value;
                NotifyPropertyChanged("SettlementDocDC");
            }
        }
        public int SettlementDocDK
        {

            get
            {
                return this._SettlementDocDK;
            }
            set
            {
                this._SettlementDocDK = value;
                NotifyPropertyChanged("SettlementDocDK");
            }
        }
        public int SettlementDocDItm
        {

            get
            {
                return this._SettlementDocDItm;
            }
            set
            {
                this._SettlementDocDItm = value;
                NotifyPropertyChanged("SettlementDocDItm");
            }
        }
        public string SettlementDocID
        {

            get
            {
                return this._SettlementDocID;
            }
            set
            {
                this._SettlementDocID = value;
                NotifyPropertyChanged("SettlementDocID");
            }
        }
        public DateTime? SettlementDocDate
        {

            get
            {
                return this._SettlementDocDate;
            }
            set
            {
                this._SettlementDocDate = value;
                NotifyPropertyChanged("SettlementDocDate");
            }
        }
        public string SettlementDocRef
        {

            get
            {
                return this._SettlementDocRef;
            }
            set
            {
                this._SettlementDocRef = value;
                NotifyPropertyChanged("SettlementDocRef");
            }
        }
        public string SettlementDocDes
        {

            get
            {
                return this._SettlementDocDes;
            }
            set
            {
                this._SettlementDocDes = value;
                NotifyPropertyChanged("SettlementDocDes");
            }
        }
        public decimal? SettlementItmQty
        {

            get
            {
                return this._SettlementItmQty;
            }
            set
            {
                this._SettlementItmQty = value;
                NotifyPropertyChanged("SettlementItmQty");
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


        #endregion
    }
}





