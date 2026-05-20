


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for INTRNDetItm.
    /// </summary>
    [Serializable]
    public class INTRNDetItm : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected int _LineType;
        protected int _LineLinkKey;
        protected decimal _ItmSN;
        protected int _ItmKey;
        protected int _ItmKeySelect;
        protected int _ItmType;
        protected string _ItmDes;
        protected int _ItmDeptKey;
        protected int? _ItmTranGrpKey;
        protected int _ItmToLocKey;
        protected int _ItmFromAccKey;
        protected int _ItmToAccKey;
        protected int _ItmFromLocKey;
        protected decimal _ItmStock;
        protected decimal _ItmQty;
        protected int _ItmUOMKey;
        protected decimal _ItmConRate;
        protected int? _ItmColorKey;
        protected string _ItmScaleSize;
        protected string _ItmPacking;
        protected int? _ItmBatchKey;
        protected decimal? _ItmBatchQty;
        protected string _ItmID;
        protected string _SKU1;
        protected string _SKU2;
        protected string _ItmFromAccID;
        protected string _ItmFromAccDes;
        protected string _ItmToAccID;
        protected string _ItmToAccDes;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public INTRNDetItm()
            : base()
        {
            this._LineType = 0;
            this._LineLinkKey = 0;
            this._ItmSN = 0;
            this._ItmKey = 0;
            this._ItmKeySelect = 0;
            this._ItmType = 0;
            this._ItmDes = string.Empty;
            this._ItmDeptKey = 0;
            this._ItmTranGrpKey = 0;
            this._ItmToLocKey = 0;
            this._ItmFromAccKey = 0;
            this._ItmToAccKey = 0;
            this._ItmFromLocKey = 0;
            this._ItmStock = 0;
            this._ItmQty = 0;
            this._ItmUOMKey = 0;
            this._ItmConRate = 0;
            this._ItmColorKey = null;
            this._ItmScaleSize = null;
            this._ItmPacking = null;
            this._ItmBatchKey = 0;
            this._ItmBatchQty = 0;
            this._ItmID = string.Empty;
            this._SKU1 = string.Empty;
            this._SKU2 = string.Empty;
            this._ItmFromAccID = string.Empty;
            this._ItmFromAccDes = string.Empty;
            this._ItmToAccID = string.Empty;
            this._ItmToAccDes = string.Empty;

        }


        public INTRNDetItm Clone()
        {
            INTRNDetItm objCopy = (INTRNDetItm)this.MemberwiseClone();
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
        public decimal ItmSN
        {

            get
            {
                return this._ItmSN;
            }
            set
            {
                this._ItmSN = value;
                NotifyPropertyChanged("ItmSN");
            }
        }
        public int ItmKey
        {

            get
            {
                return this._ItmKey;
            }
            set
            {
                this._ItmKey = value;
                NotifyPropertyChanged("ItmKey");
            }
        }
        public int ItmKeySelect
        {

            get
            {
                return this._ItmKeySelect;
            }
            set
            {
                this._ItmKeySelect = value;
                NotifyPropertyChanged("ItmKeySelect");
            }
        }
        public int ItmType
        {

            get
            {
                return this._ItmType;
            }
            set
            {
                this._ItmType = value;
                NotifyPropertyChanged("ItmType");
            }
        }
        public string ItmDes
        {

            get
            {
                return this._ItmDes;
            }
            set
            {
                this._ItmDes = value;
                NotifyPropertyChanged("ItmDes");
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
        public decimal ItmStock
        {

            get
            {
                return this._ItmStock;
            }
            set
            {
                this._ItmStock = value;
                NotifyPropertyChanged("ItmStock");
            }
        }
        public decimal ItmQty
        {

            get
            {
                return this._ItmQty;
            }
            set
            {
                this._ItmQty = value;
                NotifyPropertyChanged("ItmQty");
            }
        }
        public int ItmUOMKey
        {

            get
            {
                return this._ItmUOMKey;
            }
            set
            {
                this._ItmUOMKey = value;
                NotifyPropertyChanged("ItmUOMKey");
            }
        }
        public decimal ItmConRate
        {

            get
            {
                return this._ItmConRate;
            }
            set
            {
                this._ItmConRate = value;
                NotifyPropertyChanged("ItmConRate");
            }
        }
        public int? ItmColorKey
        {

            get
            {
                return this._ItmColorKey;
            }
            set
            {
                this._ItmColorKey = value;
                NotifyPropertyChanged("ItmColorKey");
            }
        }
        public string ItmScaleSize
        {

            get
            {
                return this._ItmScaleSize;
            }
            set
            {
                this._ItmScaleSize = value;
                NotifyPropertyChanged("ItmScaleSize");
            }
        }
        public string ItmPacking
        {

            get
            {
                return this._ItmPacking;
            }
            set
            {
                this._ItmPacking = value;
                NotifyPropertyChanged("ItmPacking");
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


        #endregion
    }
}





