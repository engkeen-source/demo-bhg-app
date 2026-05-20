using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;using System.ComponentModel;
namespace BOLib
{
	/// <summary>
	/// Summary description for docSalePur.
	/// </summary>
	[Serializable]
	public class DocSalePur : INotifyPropertyChanged
	{
		#region +++  Local variables declaration for the class +++

		internal int _DocKey;
		internal int _DocItmKey;
		internal decimal _ItmSN;
		internal int _ItmKey;
		internal int _ItmKeySelect;
		internal int _ItmType;
		internal string _ItmDes;
		internal string _ItmMark;
		internal string _ItmPacking;
		internal string _ItmRem;
		internal bool _ItmAttachment;
		internal int _ItmColorKey;
		internal string _ItmScaleSize;
		internal decimal _ItmQty;
		internal decimal _ItmStock;
		internal int _ItmLocKey;
		internal int _ItmUOMKey;
		internal decimal _ItmConRate;
		internal decimal _ItmLatestCostF;
		internal decimal _ItmLatestCostH;
		internal decimal _ItmListPrice;
		internal decimal _ItmDisPercent;
		internal decimal _ItmDisValue;
		internal decimal _ItmPrice;
		internal decimal _ItmPriceUser;
		internal decimal _ItmAmtShw;
		internal decimal _ItmAmtF;
		internal decimal _ItmAmtH;
		internal int _ItmJobKey;
		internal int _ItmJobPhaseKey;
		internal int _ItmJobTaskKey;
		internal int _ItmJobCostTypeKey;
		internal DateTime _CreateDate;
		internal int _CreateUserKey;
		internal DateTime _LastModifiedDate;
		internal int _LastModifiedUserKey;
        internal string _LastModifiedUserID;
		internal string _Custom1;
		internal string _Custom2;
		internal string _Custom3;
		internal bool _isDirty;
        internal int _ItmRecordAccess;

        internal int _SvrItmBUOMKey;
        internal string _SvrItmBUOMID;
        internal decimal _SvrItmLocQty;

        internal int _SvrLinkDocPeriod;
        internal DateTime _SvrLinkItmPrmDate;
        internal decimal _SvrLinkItmConRate;

		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region +++  Constructor and destructor codes  +++

		/// <summary>
		/// Default constructor that will initialize all properties with default values.
		/// </summary>

		public DocSalePur()
		{
            this._DocKey =0;
            this._DocItmKey =0;
            this._ItmSN = 0;
			this._ItmKey =0;
			this._ItmKeySelect =0;
			this._ItmType =0;
			this._ItmDes = string.Empty;
			this._ItmMark = string.Empty;
			this._ItmPacking = string.Empty;
			this._ItmRem = string.Empty;
			this._ItmAttachment = false;
			this._ItmColorKey =0;
			this._ItmScaleSize = string.Empty;
			this._ItmQty =0;
			this._ItmStock =0;
			this._ItmLocKey =0;
			this._ItmUOMKey =0;
			this._ItmConRate =0;
			this._ItmLatestCostF =0;
			this._ItmLatestCostH =0;
			this._ItmListPrice =0;
			this._ItmDisPercent = 0;
			this._ItmDisValue = 0;
			this._ItmPrice =0;
			this._ItmPriceUser =0;
			this._ItmAmtShw =0;
			this._ItmAmtF =0;
			this._ItmAmtH =0;
			this._ItmJobKey = 0;
			this._ItmJobPhaseKey = 0;
			this._ItmJobTaskKey = 0;
			this._ItmJobCostTypeKey = 0;
            this._CreateDate = new DateTime(1900, 1, 1);
			this._CreateUserKey =0;
            this._LastModifiedDate = new DateTime(1900, 1, 1);
			this._LastModifiedUserKey =0;
			this._Custom1 = string.Empty;
			this._Custom2 = string.Empty;
			this._Custom3 = string.Empty;
			this._isDirty = false;
		}


		public DocSalePur Clone()
		{

			DocSalePur objCopy= (DocSalePur) this.MemberwiseClone();
			objCopy._isDirty=false;
			return objCopy;
		}


		/// <summary>
		/// Disposing objects
		/// </summary>
		public void Dispose()
		{
		}

		#endregion

		internal void NotifyPropertyChanged(String info)
		{
			_isDirty = true;
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}
		#region +++  Properties  +++

		public int DocKey
		{
			get
			{
				return this._DocKey;
			}
			set
			{
				this._DocKey = value;
				NotifyPropertyChanged("DocKey");
			}
		}

		public int DocItmKey
		{
			get
			{
				return this._DocItmKey;
			}
			set
			{
				this._DocItmKey = value;
				NotifyPropertyChanged("DocItmKey");
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

		public string ItmMark
		{
			get
			{
				return this._ItmMark;
			}
			set
			{
				this._ItmMark = value;
				NotifyPropertyChanged("ItmMark");
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

		public string ItmRem
		{
			get
			{
				return this._ItmRem;
			}
			set
			{
				this._ItmRem = value;
				NotifyPropertyChanged("ItmRem");
			}
		}

		public bool ItmAttachment
		{
			get
			{
				return this._ItmAttachment;
			}
			set
			{
				this._ItmAttachment = value;
				NotifyPropertyChanged("ItmAttachment");
			}
		}

		public int ItmColorKey
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

		public int ItmLocKey
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

		public decimal ItmLatestCostF
		{
			get
			{
				return this._ItmLatestCostF;
			}
			set
			{
				this._ItmLatestCostF = value;
				NotifyPropertyChanged("ItmLatestCostF");
			}
		}

		public decimal ItmLatestCostH
		{
			get
			{
				return this._ItmLatestCostH;
			}
			set
			{
				this._ItmLatestCostH = value;
				NotifyPropertyChanged("ItmLatestCostH");
			}
		}

		public decimal ItmListPrice
		{
			get
			{
				return this._ItmListPrice;
			}
			set
			{
				this._ItmListPrice = value;
				NotifyPropertyChanged("ItmListPrice");
			}
		}

		public decimal ItmDisPercent
		{
			get
			{
				return this._ItmDisPercent;
			}
			set
			{
				this._ItmDisPercent = value;
				NotifyPropertyChanged("ItmDisPercent");
			}
		}

		public decimal ItmDisValue
		{
			get
			{
				return this._ItmDisValue;
			}
			set
			{
				this._ItmDisValue = value;
				NotifyPropertyChanged("ItmDisValue");
			}
		}

		public decimal ItmPrice
		{
			get
			{
				return this._ItmPrice;
			}
			set
			{
				this._ItmPrice = value;
				NotifyPropertyChanged("ItmPrice");
			}
		}

		public decimal ItmPriceUser
		{
			get
			{
				return this._ItmPriceUser;
			}
			set
			{
				this._ItmPriceUser = value;
				NotifyPropertyChanged("ItmPriceUser");
			}
		}

		public decimal ItmAmtShw
		{
			get
			{
				return this._ItmAmtShw;
			}
			set
			{
				this._ItmAmtShw = value;
				NotifyPropertyChanged("ItmAmtShw");
			}
		}

		public decimal ItmAmtF
		{
			get
			{
				return this._ItmAmtF;
			}
			set
			{
				this._ItmAmtF = value;
				NotifyPropertyChanged("ItmAmtF");
			}
		}

		public decimal ItmAmtH
		{
			get
			{
				return this._ItmAmtH;
			}
			set
			{
				this._ItmAmtH = value;
				NotifyPropertyChanged("ItmAmtH");
			}
		}

		public int ItmJobKey
		{
			get
			{
				return this._ItmJobKey;
			}
			set
			{
				this._ItmJobKey = value;
				NotifyPropertyChanged("ItmJobKey");
			}
		}

		public int ItmJobPhaseKey
		{
			get
			{
				return this._ItmJobPhaseKey;
			}
			set
			{
				this._ItmJobPhaseKey = value;
				NotifyPropertyChanged("ItmJobPhaseKey");
			}
		}

		public int ItmJobTaskKey
		{
			get
			{
				return this._ItmJobTaskKey;
			}
			set
			{
				this._ItmJobTaskKey = value;
				NotifyPropertyChanged("ItmJobTaskKey");
			}
		}

		public int ItmJobCostTypeKey
		{
			get
			{
				return this._ItmJobCostTypeKey;
			}
			set
			{
				this._ItmJobCostTypeKey = value;
				NotifyPropertyChanged("ItmJobCostTypeKey");
			}
		}
        public int SvrItmBUOMKey
        {
            get
            {
                return this._SvrItmBUOMKey;
            }
            set
            {
                this._SvrItmBUOMKey = value;                
            }
        }
        public string SvrItmBUOMID
        {
            get
            {
                return this._SvrItmBUOMID;
            }
            set
            {
                this._SvrItmBUOMID = value;               
            }
        }
        public decimal SvrItmLocQty
        {
            get
            {
                return this._SvrItmLocQty;
            }
            set
            {
                this._SvrItmLocQty = value;               
            }
        }
        public int SvrLinkDocPeriod
        {
            get
            {
                return this._SvrLinkDocPeriod;
            }
            set
            {
                this._SvrLinkDocPeriod = value;
            }
        }
        public DateTime SvrLinkItmPrmDate
        {
            get
            {
                return this._SvrLinkItmPrmDate;
            }
            set
            {
                this._SvrLinkItmPrmDate = value;               
            }
        }
        public decimal SvrLinkItmConRate
        {
            get
            {
                return this._SvrLinkItmConRate;
            }
            set
            {
                this._SvrLinkItmConRate = value;
            }
        }
		public DateTime CreateDate
		{
			get
			{
				return this._CreateDate;
			}
			set
			{
				this._CreateDate = value;
				NotifyPropertyChanged("CreateDate");
			}
		}

		public int CreateUserKey
		{
			get
			{
				return this._CreateUserKey;
			}
			set
			{
				this._CreateUserKey = value;
				NotifyPropertyChanged("CreateUserKey");
			}
		}

		public DateTime LastModifiedDate
		{
			get
			{
				return this._LastModifiedDate;
			}
			set
			{
				this._LastModifiedDate = value;
				NotifyPropertyChanged("LastModifiedDate");
			}
		}

		public int LastModifiedUserKey
		{
			get
			{
				return this._LastModifiedUserKey;
			}
			set
			{
				this._LastModifiedUserKey = value;
				NotifyPropertyChanged("LastModifiedUserKey");
			}
		}
        public string LastModifiedUserID
        {
            get
            {
                return this._LastModifiedUserID;
            }
            set
            {
                this._LastModifiedUserID = value;
                NotifyPropertyChanged("LastModifiedUserID");
            }
        }
		public string Custom1
		{
			get
			{
				return this._Custom1;
			}
			set
			{
				this._Custom1 = value;
				NotifyPropertyChanged("Custom1");
			}
		}

		public string Custom2
		{
			get
			{
				return this._Custom2;
			}
			set
			{
				this._Custom2 = value;
				NotifyPropertyChanged("Custom2");
			}
		}

		public string Custom3
		{
			get
			{
				return this._Custom3;
			}
			set
			{
				this._Custom3 = value;
				NotifyPropertyChanged("Custom3");
			}
		}

		public bool IsDirty
		{
			get
			{
				return this._isDirty;
			}
		}


		#endregion		
	}
}