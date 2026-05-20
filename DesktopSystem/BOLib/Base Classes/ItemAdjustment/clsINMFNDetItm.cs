


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for INMFNDetItm.
    /// </summary>
    [Serializable]
    public class INMFNDetItm : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected int _LineType;
        protected int _LineLinkKey;
        protected decimal _ItmSN;
        protected int _ItmFGKey;
        protected int _ItmFGKeySelect;
        protected int _ItmKey;
        protected int _ItmKeySelect;
        protected int _ItmType;
        protected string _ItmDes;
        protected int _ItmDeptKey;
        protected int? _ItmTranGrpKey;
        protected int? _ItmAccINKey;
        protected int? _ItmLocKey;
        protected string _ItmPacking;
        protected decimal? _FGWeight;
        protected int? _FGWeightUOMKey;
        protected decimal? _FGReq;
        protected decimal? _FGProduceQty;
        protected decimal? _FGProduceWeight;
        protected decimal? _FGProduceGram;
        protected int? _FGOverHeadKey;
        protected decimal? _FGOverHeadCost;
        protected decimal? _FGOverHeadAmtH;
        protected decimal? _FGCostRatio;
        protected int? _BOMMultiplier;
        protected int? _BOMBUOMKey;
        protected decimal? _BOMWeight;
        protected int? _BOMWeightUOMKey;
        protected decimal? _BOMReq;
        protected decimal? _BOMIssue;
        protected decimal? _BOMReturn;
        protected decimal? _BOMUsed;
        protected decimal? _BOMUsedWeight;
        protected decimal? _BOMUsedGram;
        protected decimal? _BOMLabourCost;
        protected decimal? _BOMLabourAmt;
        protected int? _FGBUOMKey;
        protected int? _ItmBatchKey;
        protected decimal? _ItmBatchQty;
        protected string _ItmFGID;
        protected string _ItmID;
        protected string _SKU1;
        protected string _SKU2;
        protected string _ItmAccINID;
        protected string _ItmAccINDes;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public INMFNDetItm()
            : base()
        {
            this._LineType = 0;
            this._LineLinkKey = 0;
            this._ItmSN = 0;
            this._ItmFGKey = 0;
            this._ItmFGKeySelect = 0;
            this._ItmKey = 0;
            this._ItmKeySelect = 0;
            this._ItmType = 0;
            this._ItmDes = string.Empty;
            this._ItmDeptKey = 0;
            this._ItmTranGrpKey = 0;
            this._ItmAccINKey = null;
            this._ItmLocKey = null;
            this._ItmPacking = null;
            this._FGWeight = null;
            this._FGWeightUOMKey = null;
            this._FGReq = null;
            this._FGProduceQty = null;
            this._FGProduceWeight = null;
            this._FGProduceGram = null;
            this._FGOverHeadKey = null;
            this._FGOverHeadCost = null;
            this._FGOverHeadAmtH = null;
            this._FGCostRatio = null;
            this._BOMMultiplier = null;
            this._BOMBUOMKey = null;
            this._BOMWeight = null;
            this._BOMWeightUOMKey = null;
            this._BOMReq = null;
            this._BOMIssue = null;
            this._BOMReturn = null;
            this._BOMUsed = null;
            this._BOMUsedWeight = null;
            this._BOMUsedGram = null;
            this._BOMLabourCost = null;
            this._BOMLabourAmt = null;
            this._FGBUOMKey = null;
            this._ItmBatchKey = 0;
            this._ItmBatchQty = 0;
            this._ItmFGID = string.Empty;
            this._ItmID = string.Empty;
            this._SKU1 = string.Empty;
            this._SKU2 = string.Empty;
            this._ItmAccINID = string.Empty;
            this._ItmAccINDes = string.Empty;

        }


        public INMFNDetItm Clone()
        {
            INMFNDetItm objCopy = (INMFNDetItm)this.MemberwiseClone();
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
        public int ItmFGKey
        {

            get
            {
                return this._ItmFGKey;
            }
            set
            {
                this._ItmFGKey = value;
                NotifyPropertyChanged("ItmFGKey");
            }
        }
        public int ItmFGKeySelect
        {

            get
            {
                return this._ItmFGKeySelect;
            }
            set
            {
                this._ItmFGKeySelect = value;
                NotifyPropertyChanged("ItmFGKeySelect");
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
        public int? ItmAccINKey
        {

            get
            {
                return this._ItmAccINKey;
            }
            set
            {
                this._ItmAccINKey = value;
                NotifyPropertyChanged("ItmAccINKey");
            }
        }
        public int? ItmLocKey
        {

            get
            {
                return this._ItmLocKey;
            }
            set
            {
                this._ItmLocKey = value;
                NotifyPropertyChanged("ItmLocKey");
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
        public decimal? FGWeight
        {

            get
            {
                return this._FGWeight;
            }
            set
            {
                this._FGWeight = value;
                NotifyPropertyChanged("FGWeight");
            }
        }
        public int? FGWeightUOMKey
        {

            get
            {
                return this._FGWeightUOMKey;
            }
            set
            {
                this._FGWeightUOMKey = value;
                NotifyPropertyChanged("FGWeightUOMKey");
            }
        }
        public decimal? FGReq
        {

            get
            {
                return this._FGReq;
            }
            set
            {
                this._FGReq = value;
                NotifyPropertyChanged("FGReq");
            }
        }
        public decimal? FGProduceQty
        {

            get
            {
                return this._FGProduceQty;
            }
            set
            {
                this._FGProduceQty = value;
                NotifyPropertyChanged("FGProduceQty");
            }
        }
        public decimal? FGProduceWeight
        {

            get
            {
                return this._FGProduceWeight;
            }
            set
            {
                this._FGProduceWeight = value;
                NotifyPropertyChanged("FGProduceWeight");
            }
        }
        public decimal? FGProduceGram
        {

            get
            {
                return this._FGProduceGram;
            }
            set
            {
                this._FGProduceGram = value;
                NotifyPropertyChanged("FGProduceGram");
            }
        }
        public int? FGOverHeadKey
        {

            get
            {
                return this._FGOverHeadKey;
            }
            set
            {
                this._FGOverHeadKey = value;
                NotifyPropertyChanged("FGOverHeadKey");
            }
        }
        public decimal? FGOverHeadCost
        {

            get
            {
                return this._FGOverHeadCost;
            }
            set
            {
                this._FGOverHeadCost = value;
                NotifyPropertyChanged("FGOverHeadCost");
            }
        }
        public decimal? FGOverHeadAmtH
        {

            get
            {
                return this._FGOverHeadAmtH;
            }
            set
            {
                this._FGOverHeadAmtH = value;
                NotifyPropertyChanged("FGOverHeadAmtH");
            }
        }
        public decimal? FGCostRatio
        {

            get
            {
                return this._FGCostRatio;
            }
            set
            {
                this._FGCostRatio = value;
                NotifyPropertyChanged("FGCostRatio");
            }
        }
        public int? BOMMultiplier
        {

            get
            {
                return this._BOMMultiplier;
            }
            set
            {
                this._BOMMultiplier = value;
                NotifyPropertyChanged("BOMMultiplier");
            }
        }
        public int? BOMBUOMKey
        {

            get
            {
                return this._BOMBUOMKey;
            }
            set
            {
                this._BOMBUOMKey = value;
                NotifyPropertyChanged("BOMBUOMKey");
            }
        }
        public decimal? BOMWeight
        {

            get
            {
                return this._BOMWeight;
            }
            set
            {
                this._BOMWeight = value;
                NotifyPropertyChanged("BOMWeight");
            }
        }
        public int? BOMWeightUOMKey
        {

            get
            {
                return this._BOMWeightUOMKey;
            }
            set
            {
                this._BOMWeightUOMKey = value;
                NotifyPropertyChanged("BOMWeightUOMKey");
            }
        }
        public decimal? BOMReq
        {

            get
            {
                return this._BOMReq;
            }
            set
            {
                this._BOMReq = value;
                NotifyPropertyChanged("BOMReq");
            }
        }
        public decimal? BOMIssue
        {

            get
            {
                return this._BOMIssue;
            }
            set
            {
                this._BOMIssue = value;
                NotifyPropertyChanged("BOMIssue");
            }
        }
        public decimal? BOMReturn
        {

            get
            {
                return this._BOMReturn;
            }
            set
            {
                this._BOMReturn = value;
                NotifyPropertyChanged("BOMReturn");
            }
        }
        public decimal? BOMUsed
        {

            get
            {
                return this._BOMUsed;
            }
            set
            {
                this._BOMUsed = value;
                NotifyPropertyChanged("BOMUsed");
            }
        }
        public decimal? BOMUsedWeight
        {

            get
            {
                return this._BOMUsedWeight;
            }
            set
            {
                this._BOMUsedWeight = value;
                NotifyPropertyChanged("BOMUsedWeight");
            }
        }
        public decimal? BOMUsedGram
        {

            get
            {
                return this._BOMUsedGram;
            }
            set
            {
                this._BOMUsedGram = value;
                NotifyPropertyChanged("BOMUsedGram");
            }
        }
        public decimal? BOMLabourCost
        {

            get
            {
                return this._BOMLabourCost;
            }
            set
            {
                this._BOMLabourCost = value;
                NotifyPropertyChanged("BOMLabourCost");
            }
        }
        public decimal? BOMLabourAmt
        {

            get
            {
                return this._BOMLabourAmt;
            }
            set
            {
                this._BOMLabourAmt = value;
                NotifyPropertyChanged("BOMLabourAmt");
            }
        }
        public int? FGBUOMKey
        {

            get
            {
                return this._FGBUOMKey;
            }
            set
            {
                this._FGBUOMKey = value;
                NotifyPropertyChanged("FGBUOMKey");
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
        public string ItmFGID
        {

            get
            {
                return this._ItmFGID;
            }
            set
            {
                this._ItmFGID = value;
                NotifyPropertyChanged("ItmFGID");
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
        public string ItmAccINID
        {

            get
            {
                return this._ItmAccINID;
            }
            set
            {
                this._ItmAccINID = value;
                NotifyPropertyChanged("ItmAccINID");
            }
        }
        public string ItmAccINDes
        {

            get
            {
                return this._ItmAccINDes;
            }
            set
            {
                this._ItmAccINDes = value;
                NotifyPropertyChanged("ItmAccINDes");
            }
        }


        #endregion
    }
}





