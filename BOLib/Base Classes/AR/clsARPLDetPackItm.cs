


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for ARPLDetPackItm.
    /// </summary>
    [Serializable]
    public class ARPLDetPackItm : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected decimal? _DetItmSN;
        protected int? _DetItmKey;
        protected int? _DetItmKeySelect;
        protected int? _DetItmType;
        protected string _DetItmDes;
        protected int? _DetItmDeptKey;
        protected string _DetItmBatchID;
        protected string _DetItmPacking;
        protected decimal? _DetItmQtyPerPack;
        protected decimal? _DetItmQtyTotal;
        protected int? _DetItmUOMKey;
        protected decimal? _DetItmConRate;
        protected decimal? _DetItmWeightNet;
        protected decimal? _DetItmWeightGross;
        protected int? _DetItmWeightUOMKey;
        protected decimal? _DetItmWeightUOMRate;
        protected decimal? _DetItmWeightBaseNet;
        protected decimal? _DetItmWeightBaseGross;
        protected bool? _DetItmHide;
        protected string _DetItmDocID;
        protected string _DetItmMarking;
        protected int? _DetItmColorKey;
        protected string _DetItmScaleSize;
        protected string _DetItmRem;
        protected int? _DocItmDetKey;
        protected string _DetItmID;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public ARPLDetPackItm()
            : base()
        {
            this._DetItmSN = 0;
            this._DetItmKey = 0;
            this._DetItmKeySelect = 0;
            this._DetItmType = 0;
            this._DetItmDes = string.Empty;
            this._DetItmDeptKey = 0;
            this._DetItmBatchID = string.Empty;
            this._DetItmPacking = string.Empty;
            this._DetItmQtyPerPack = 0;
            this._DetItmQtyTotal = 0;
            this._DetItmUOMKey = 0;
            this._DetItmConRate = 0;
            this._DetItmWeightNet = 0;
            this._DetItmWeightGross = 0;
            this._DetItmWeightUOMKey = 0;
            this._DetItmWeightUOMRate = 0;
            this._DetItmWeightBaseNet = 0;
            this._DetItmWeightBaseGross = 0;
            this._DetItmHide = false;
            this._DetItmDocID = string.Empty;
            this._DetItmMarking = string.Empty;
            this._DetItmColorKey = 0;
            this._DetItmScaleSize = string.Empty;
            this._DetItmRem = string.Empty;
            this._DocItmDetKey = 0;
            this._DetItmID = string.Empty;

        }


        public ARPLDetPackItm Clone()
        {
            ARPLDetPackItm objCopy = (ARPLDetPackItm)this.MemberwiseClone();
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


        public decimal? DetItmSN
        {

            get
            {
                return this._DetItmSN;
            }
            set
            {
                this._DetItmSN = value;
                NotifyPropertyChanged("DetItmSN");
            }
        }
        public int? DetItmKey
        {

            get
            {
                return this._DetItmKey;
            }
            set
            {
                this._DetItmKey = value;
                NotifyPropertyChanged("DetItmKey");
            }
        }
        public int? DetItmKeySelect
        {

            get
            {
                return this._DetItmKeySelect;
            }
            set
            {
                this._DetItmKeySelect = value;
                NotifyPropertyChanged("DetItmKeySelect");
            }
        }
        public int? DetItmType
        {

            get
            {
                return this._DetItmType;
            }
            set
            {
                this._DetItmType = value;
                NotifyPropertyChanged("DetItmType");
            }
        }
        public string DetItmDes
        {

            get
            {
                return this._DetItmDes;
            }
            set
            {
                this._DetItmDes = value;
                NotifyPropertyChanged("DetItmDes");
            }
        }
        public int? DetItmDeptKey
        {

            get
            {
                return this._DetItmDeptKey;
            }
            set
            {
                this._DetItmDeptKey = value;
                NotifyPropertyChanged("DetItmDeptKey");
            }
        }
        public string DetItmBatchID
        {

            get
            {
                return this._DetItmBatchID;
            }
            set
            {
                this._DetItmBatchID = value;
                NotifyPropertyChanged("DetItmBatchID");
            }
        }
        public string DetItmPacking
        {

            get
            {
                return this._DetItmPacking;
            }
            set
            {
                this._DetItmPacking = value;
                NotifyPropertyChanged("DetItmPacking");
            }
        }
        public decimal? DetItmQtyPerPack
        {

            get
            {
                return this._DetItmQtyPerPack;
            }
            set
            {
                this._DetItmQtyPerPack = value;
                NotifyPropertyChanged("DetItmQtyPerPack");
            }
        }
        public decimal? DetItmQtyTotal
        {

            get
            {
                return this._DetItmQtyTotal;
            }
            set
            {
                this._DetItmQtyTotal = value;
                NotifyPropertyChanged("DetItmQtyTotal");
            }
        }
        public int? DetItmUOMKey
        {

            get
            {
                return this._DetItmUOMKey;
            }
            set
            {
                this._DetItmUOMKey = value;
                NotifyPropertyChanged("DetItmUOMKey");
            }
        }
        public decimal? DetItmConRate
        {

            get
            {
                return this._DetItmConRate;
            }
            set
            {
                this._DetItmConRate = value;
                NotifyPropertyChanged("DetItmConRate");
            }
        }
        public decimal? DetItmWeightNet
        {

            get
            {
                return this._DetItmWeightNet;
            }
            set
            {
                this._DetItmWeightNet = value;
                NotifyPropertyChanged("DetItmWeightNet");
            }
        }
        public decimal? DetItmWeightGross
        {

            get
            {
                return this._DetItmWeightGross;
            }
            set
            {
                this._DetItmWeightGross = value;
                NotifyPropertyChanged("DetItmWeightGross");
            }
        }
        public int? DetItmWeightUOMKey
        {

            get
            {
                return this._DetItmWeightUOMKey;
            }
            set
            {
                this._DetItmWeightUOMKey = value;
                NotifyPropertyChanged("DetItmWeightUOMKey");
            }
        }
        public decimal? DetItmWeightUOMRate
        {

            get
            {
                return this._DetItmWeightUOMRate;
            }
            set
            {
                this._DetItmWeightUOMRate = value;
                NotifyPropertyChanged("DetItmWeightUOMRate");
            }
        }
        public decimal? DetItmWeightBaseNet
        {

            get
            {
                return this._DetItmWeightBaseNet;
            }
            set
            {
                this._DetItmWeightBaseNet = value;
                NotifyPropertyChanged("DetItmWeightBaseNet");
            }
        }
        public decimal? DetItmWeightBaseGross
        {

            get
            {
                return this._DetItmWeightBaseGross;
            }
            set
            {
                this._DetItmWeightBaseGross = value;
                NotifyPropertyChanged("DetItmWeightBaseGross");
            }
        }
        public bool? DetItmHide
        {

            get
            {
                return this._DetItmHide;
            }
            set
            {
                this._DetItmHide = value;
                NotifyPropertyChanged("DetItmHide");
            }
        }
        public string DetItmDocID
        {

            get
            {
                return this._DetItmDocID;
            }
            set
            {
                this._DetItmDocID = value;
                NotifyPropertyChanged("DetItmDocID");
            }
        }
        public string DetItmMarking
        {

            get
            {
                return this._DetItmMarking;
            }
            set
            {
                this._DetItmMarking = value;
                NotifyPropertyChanged("DetItmMarking");
            }
        }
        public int? DetItmColorKey
        {

            get
            {
                return this._DetItmColorKey;
            }
            set
            {
                this._DetItmColorKey = value;
                NotifyPropertyChanged("DetItmColorKey");
            }
        }
        public string DetItmScaleSize
        {

            get
            {
                return this._DetItmScaleSize;
            }
            set
            {
                this._DetItmScaleSize = value;
                NotifyPropertyChanged("DetItmScaleSize");
            }
        }
        public string DetItmRem
        {

            get
            {
                return this._DetItmRem;
            }
            set
            {
                this._DetItmRem = value;
                NotifyPropertyChanged("DetItmRem");
            }
        }
        public int? DocItmDetKey
        {

            get
            {
                return this._DocItmDetKey;
            }
            set
            {
                this._DocItmDetKey = value;
                NotifyPropertyChanged("DocItmDetKey");
            }
        }
        public string DetItmID
        {

            get
            {
                return this._DetItmID;
            }
            set
            {
                this._DetItmID = value;
                NotifyPropertyChanged("DetItmID");
            }
        }


        #endregion
    }
}





