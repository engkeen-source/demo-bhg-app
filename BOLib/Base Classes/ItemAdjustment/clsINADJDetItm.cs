using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

namespace BOLib
{
    /// <summary>
    /// Summary description for INADJDetItm.
    /// </summary>
    [Serializable]
    public class INADJDetItm : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected int _LineType;
        protected int _LineLinkKey;
        protected string _ItmRef;
        protected int _ItmDeptKey;
        protected int? _ItmTranGrpKey;
        protected int _ItmAccKey;
        protected decimal _ItmCost;
        protected decimal _ItmNewCost;
        protected int? _ItmBatchKey;
        protected decimal? _ItmBatchQty;
        protected string _CSCPSID;
        protected int _CSCPSDK;
        protected int _CSCPSDItm;
        protected string _ItmID;
        protected string _ItmAccID;
        protected string _ItmAccDes;
        protected string _SKU1;
        protected string _SKU2;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public INADJDetItm()
            : base()
        {
            this._LineType = 0;
            this._LineLinkKey = 0;
            this._ItmRef = null;
            this._ItmDeptKey = 0;
            this._ItmTranGrpKey = 0;
            this._ItmAccKey = 0;
            this._ItmCost = 0;
            this._ItmNewCost = 0;
            this._ItmBatchKey = 0;
            this._ItmBatchQty = 0;
            this._CSCPSID = null;
            this._CSCPSDK = 0;
            this._CSCPSDItm = 0;
            this._ItmID = string.Empty;
            this._ItmAccID = string.Empty;
            this._ItmAccDes = string.Empty;
            this._SKU1 = string.Empty;
            this._SKU2 = string.Empty;

        }


        public INADJDetItm Clone()
        {
            INADJDetItm objCopy = (INADJDetItm)this.MemberwiseClone();
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
        public string ItmRef
        {

            get
            {
                return this._ItmRef;
            }
            set
            {
                this._ItmRef = value;
                NotifyPropertyChanged("ItmRef");
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
        public int ItmAccKey
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
        public decimal ItmCost
        {

            get
            {
                return this._ItmCost;
            }
            set
            {
                this._ItmCost = value;
                NotifyPropertyChanged("ItmCost");
            }
        }
        public decimal ItmNewCost
        {

            get
            {
                return this._ItmNewCost;
            }
            set
            {
                this._ItmNewCost = value;
                NotifyPropertyChanged("ItmNewCost");
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


        #endregion
    }
}





