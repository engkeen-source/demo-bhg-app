using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for APPJDetItm.
    /// </summary>
    [Serializable]
    public class APPJDetItm : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected int _ItmLinkDocDK;
        protected int _ItmLinkDocDItm;
        protected string _ItmLinkDocID;
        protected decimal _ItmLinkItmSN;
        protected string _ItmLinkPOID;
        protected decimal _ItmQtyLink;
        protected decimal _ItmQtyAdj;
        protected DateTime _ItmReqDate;
        protected DateTime _ItmPrmDate;
        protected DateTime? _ItmReqDateNew;
        protected DateTime? _ItmPrmDateNew;
        protected int _ItmStatus;
        protected string _ConfirmID;
        protected string _ConfirmSN;
        protected int _PostErr;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public APPJDetItm()
            : base()
        {
            this._ItmLinkDocDK = 0;
            this._ItmLinkDocDItm = 0;
            this._ItmLinkDocID = string.Empty;
            this._ItmLinkItmSN = 0;
            this._ItmLinkPOID = null;
            this._ItmQtyLink = 0;
            this._ItmQtyAdj = 0;
            this._ItmReqDate = DateTime.Today.Date;
            this._ItmPrmDate = DateTime.Today.Date;
            this._ItmReqDateNew = DateTime.Today.Date;
            this._ItmPrmDateNew = DateTime.Today.Date;
            this._ItmStatus = 0;
            this._ConfirmID = null;
            this._ConfirmSN = null;
            this._PostErr = 0;

        }


        public APPJDetItm Clone()
        {
            APPJDetItm objCopy = (APPJDetItm)this.MemberwiseClone();
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


        public int ItmLinkDocDK
        {

            get
            {
                return this._ItmLinkDocDK;
            }
            set
            {
                this._ItmLinkDocDK = value;
                NotifyPropertyChanged("ItmLinkDocDK");
            }
        }
        public int ItmLinkDocDItm
        {

            get
            {
                return this._ItmLinkDocDItm;
            }
            set
            {
                this._ItmLinkDocDItm = value;
                NotifyPropertyChanged("ItmLinkDocDItm");
            }
        }
        public string ItmLinkDocID
        {

            get
            {
                return this._ItmLinkDocID;
            }
            set
            {
                this._ItmLinkDocID = value;
                NotifyPropertyChanged("ItmLinkDocID");
            }
        }
        public decimal ItmLinkItmSN
        {

            get
            {
                return this._ItmLinkItmSN;
            }
            set
            {
                this._ItmLinkItmSN = value;
                NotifyPropertyChanged("ItmLinkItmSN");
            }
        }
        public string ItmLinkPOID
        {

            get
            {
                return this._ItmLinkPOID;
            }
            set
            {
                this._ItmLinkPOID = value;
                NotifyPropertyChanged("ItmLinkPOID");
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
        public decimal ItmQtyAdj
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
        public DateTime ItmReqDate
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
        public DateTime ItmPrmDate
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
        public DateTime? ItmReqDateNew
        {

            get
            {
                return this._ItmReqDateNew;
            }
            set
            {
                this._ItmReqDateNew = value;
                NotifyPropertyChanged("ItmReqDateNew");
            }
        }
        public DateTime? ItmPrmDateNew
        {

            get
            {
                return this._ItmPrmDateNew;
            }
            set
            {
                this._ItmPrmDateNew = value;
                NotifyPropertyChanged("ItmPrmDateNew");
            }
        }
        public int ItmStatus
        {

            get
            {
                return this._ItmStatus;
            }
            set
            {
                this._ItmStatus = value;
                NotifyPropertyChanged("ItmStatus");
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
        public int PostErr
        {

            get
            {
                return this._PostErr;
            }
            set
            {
                this._PostErr = value;
                NotifyPropertyChanged("PostErr");
            }
        }


        #endregion

      
    }
}





