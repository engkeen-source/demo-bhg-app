using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;using System.ComponentModel;
namespace BOLib
{
	/// <summary>
	/// Summary description for Document.
	/// </summary>
	[Serializable]
	public class Document : INotifyPropertyChanged
	{
		#region +++  Local variables declaration for the class +++

		internal int? _DocKey;
		internal int? _DocCodeKey;
		internal string _DocID="";
		internal DateTime? _DocDate;
		internal int? _DocType;
		internal string _DocTypeNm;
		internal short? _DocSign;
		internal bool _DocPrinted;
		internal int? _DocState;
		internal string _DocStatus;
		internal int? _BranchKey;
		internal int? _DocEmKey;
		internal string _DocRef;
		internal string _DocDes;
		internal string _DocRem;
		internal bool _Attachment;
		internal int? _ApproveUserKey;
		internal DateTime? _ApproveDate;
		internal int? _DisapproveUserKey;
		internal DateTime? _DisapproveDate;
        internal Int16? _DisapproveCount;//Update DataType int? to Int16 ,because server dataType is smallint
		internal string _DisapproveMsg;
		internal DateTime? _CreateDate;
		internal int? _CreateUserKey;
		internal DateTime? _LastModifiedDate;
		internal int? _LastModifiedUserKey;
        internal string _LastModifiedUserID;
		internal string _Custom1;
		internal string _Custom2;
		internal string _Custom3;
		internal string _Custom4;
		internal string _Custom5;
        internal string _ApprovalStatus;
        internal int? _PurgeKeep;
		internal bool _PurgeData;
        internal int? _DefLocKey; //Not Save in DB. Work in Form for Detail Item Location
        internal int? _DefFromLocKey; //Not Save in DB. Work in Form for Detail Item Location
        internal int? _DefToLocKey; //Not Save in DB. Work in Form for Detail Item Location
        internal string _DefBAddrKey;
        internal string _DefSAddrKey;
        internal int? _DefJobKey; //Not Save in DB. Work in Form for Detail Item Job
        internal int? _DefAccKey; //Not Save in DB. Work in Form for Detail Item Acc

        //internal int? _svrDocPeriod;
        //internal DateTime? _svrDocDate;
        //internal int? _svrDocType;
        //internal short? _svrDocSign;
        //internal int? _svrDocGrpKey;
        //internal int? _svrDocConKey;
        //internal decimal? _svrDocGrand;
        //internal int? _svrDocCurrKey;
        //internal decimal? _svrDocCurrRate;
        //internal decimal? _svrDocHome;
        //internal int? _svrDocApplyIVDC;
        //internal int? _svrDocApplyIVDK;
        //internal decimal? _svrDocApplyGainAmt;
        //internal int? _svrDocPaidAccKey;
        internal bool _isDirty = false;
        internal bool _isReadOnly = true;
        internal bool _isValid = false;
        internal bool _isNew = true;
        internal int? _GUID = 0;

        internal int? _listDefaultDateRange;
        internal bool? _ShowEnquiryForm;
        public event PropertyChangedEventHandler PropertyChanged;
        public event GVar.ListEvent_RefreshRecord FormRefresh;
		#endregion

		#region +++  Constructor and destructor codes  +++

		/// <summary>
		/// Default constructor that will initialize all properties with default values.
		/// </summary>

		public Document()
		{
			this._DocKey = null;
			this._DocCodeKey = null;
			this._DocID = string.Empty;
			this._DocDate = DateTime.Today;
			this._DocType = null;
			this._DocTypeNm = string.Empty;
			this._DocSign = null;
			this._DocPrinted = false;
			this._DocState = null;
			this._DocStatus = string.Empty;
			this._BranchKey = null;
			this._DocEmKey = null;
			this._DocRef = string.Empty;
			this._DocDes = string.Empty;
			this._DocRem = string.Empty;
			this._Attachment = false;
			this._ApproveUserKey = null;
			this._ApproveDate = null;
			this._DisapproveUserKey = null;
			this._DisapproveDate = null;
			this._DisapproveCount = null;
			this._DisapproveMsg = string.Empty;
			this._CreateDate = null;
			this._CreateUserKey = null;
			this._LastModifiedDate = null;
			this._LastModifiedUserKey = null;
			this._Custom1 = string.Empty;
			this._Custom2 = string.Empty;
			this._Custom3 = string.Empty;
			this._Custom4 = string.Empty;
			this._Custom5 = string.Empty;
            this._ApprovalStatus = string.Empty;
            this._PurgeKeep = null;
			this._PurgeData = false;         
            //this._svrDocPeriod = null;
            //this._svrDocDate = null;
            //this._svrDocType = null;
            //this._svrDocSign = null;
            //this._svrDocGrpKey = null;
            //this._svrDocConKey = null;
            //this._svrDocGrand = null;
            //this._svrDocCurrKey = null;
            //this._svrDocCurrRate = null;
            //this._svrDocHome = null;
            //this._svrDocApplyIVDC = null;
            //this._svrDocApplyIVDK = null;
            //this._svrDocApplyGainAmt = null;
            //this._svrDocPaidAccKey = null;
            this._isDirty = false;
            this._isReadOnly = false;
            this._GUID = 0;
		}       

		public Document Clone()
		{

			Document objCopy= (Document) this.MemberwiseClone();
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

        private void NotifyPropertyChanged(String info)
		{
			_isDirty = true;
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}

		#region +++  Properties  +++

		public int? DocKey
		{
			get
			{
				return this._DocKey;
			}
			set
			{
				this._DocKey = value;
			}
		}

		public int? DocCodeKey
		{
			get
			{
				return this._DocCodeKey;
			}
			set
			{
				this._DocCodeKey = value;
			}
		}

		public string DocID
		{
			get
			{
				return this._DocID;
			}
			set
			{
				this._DocID = value;
				NotifyPropertyChanged("DocID");
			}
		}

		public DateTime? DocDate
		{
			get
			{
				return this._DocDate;
			}
			set
			{
				this._DocDate = value;
				NotifyPropertyChanged("DocDate");
			}
		}

		public int? DocType
		{
			get
			{
				return this._DocType;
			}
			set
			{
				this._DocType = value;
				NotifyPropertyChanged("DocType");
			}
		}

		public string DocTypeNm
		{
			get
			{
				return this._DocTypeNm;
			}
			set
			{
				this._DocTypeNm = value;
				NotifyPropertyChanged("DocTypeNm");
			}
		}

		public short? DocSign
		{
			get
			{
				return this._DocSign;
			}
			set
			{
				this._DocSign = value;
				NotifyPropertyChanged("DocSign");
			}
		}

		public bool DocPrinted
		{
			get
			{
				return this._DocPrinted;
			}
			set
			{
				this._DocPrinted = value;
			}
		}

		public int? DocState
		{
			get
			{
				return this._DocState;
			}
			set
			{
				this._DocState = value;
			}
		}

		public string DocStatus
		{
			get
			{
				return this._DocStatus;
			}
			set
			{
				this._DocStatus = value;
				NotifyPropertyChanged("DocStatus");
			}
		}

		public int? BranchKey
		{
			get
			{
				return this._BranchKey;
			}
			set
			{
				this._BranchKey = value;
				NotifyPropertyChanged("BranchKey");
			}
		}

		public int? DocEmKey
		{
			get
			{
				return this._DocEmKey;
			}
			set
			{
				this._DocEmKey = value;
				NotifyPropertyChanged("DocEmKey");
			}
		}

		public string DocRef
		{
			get
			{
				return this._DocRef;
			}
			set
			{
				this._DocRef = value;
				NotifyPropertyChanged("DocRef");
			}
		}

		public string DocDes
		{
			get
			{
				return this._DocDes;
			}
			set
			{
				this._DocDes = value;
				NotifyPropertyChanged("DocDes");
			}
		}

		public string DocRem
		{
			get
			{
				return this._DocRem;
			}
			set
			{
				this._DocRem = value;
				NotifyPropertyChanged("DocRem");
			}
		}

		public bool Attachment
		{
			get
			{
				return this._Attachment;
			}
			set
			{
				this._Attachment = value;
				NotifyPropertyChanged("Attachment");
			}
		}

		public int? ApproveUserKey
		{
			get
			{
				return this._ApproveUserKey;
			}
			set
			{
				this._ApproveUserKey = value;
				NotifyPropertyChanged("ApproveUserKey");
			}
		}

		public DateTime? ApproveDate
		{
			get
			{
				return this._ApproveDate;
			}
			set
			{
				this._ApproveDate = value;
				NotifyPropertyChanged("ApproveDate");
			}
		}

		public int? DisapproveUserKey
		{
			get
			{
				return this._DisapproveUserKey;
			}
			set
			{
				this._DisapproveUserKey = value;
				NotifyPropertyChanged("DisapproveUserKey");
			}
		}

		public DateTime? DisapproveDate
		{
			get
			{
				return this._DisapproveDate;
			}
			set
			{
				this._DisapproveDate = value;
				NotifyPropertyChanged("DisapproveDate");
			}
		}

		public Int16? DisapproveCount //Update DataType int? to Int16 ,because server dataType is smallint
		{
			get
			{
				return this._DisapproveCount;
			}
			set
			{
				this._DisapproveCount = value;
				NotifyPropertyChanged("DisapproveCount");
			}
		}

		public string DisapproveMsg
		{
			get
			{
				return this._DisapproveMsg;
			}
			set
			{
				this._DisapproveMsg = value;
				NotifyPropertyChanged("DisapproveMsg");
			}
		}

		public DateTime? CreateDate
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

		public int? CreateUserKey
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

		public DateTime? LastModifiedDate
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

		public int? LastModifiedUserKey
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
        public string  LastModifiedUserID
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

		public string Custom4
		{
			get
			{
				return this._Custom4;
			}
			set
			{
				this._Custom4 = value;
				NotifyPropertyChanged("Custom4");
			}
		}

		public string Custom5
		{
			get
			{
				return this._Custom5;
			}
			set
			{
				this._Custom5 = value;
				NotifyPropertyChanged("Custom5");
			}
		}

        public string ApprovalStatus
        {
            get
            {
                return this._ApprovalStatus;
            }
            set
            {
                this._ApprovalStatus = value;
                NotifyPropertyChanged("ApprovalStatus");
            }
        }

        public int? PurgeKeep
		{
			get
			{
				return this._PurgeKeep;
			}
			set
			{
				this._PurgeKeep = value;
				NotifyPropertyChanged("PurgeKeep");
			}
		}

		public bool PurgeData
		{
			get
			{
				return this._PurgeData;
			}
			set
			{
				this._PurgeData = value;
				NotifyPropertyChanged("PurgeData");
			}
		}

        //Not Save in DB. Work in Form for Detail Item Location
        public int? DefLocKey
        {
            get
            {
                return this._DefLocKey;
            }
            set
            {
                this._DefLocKey = value;
            }
        }
        //Not Save in DB. Work in Form for Detail Item Location
        public int? DefFromLocKey
        {
            get
            {
                return this._DefFromLocKey;
            }
            set
            {
                this._DefFromLocKey = value;
            }
        }
        //Not Save in DB. Work in Form for Detail Item Location
        public int? DefToLocKey 
        {
            get
            {
                return this._DefToLocKey;
            }
            set
            {
                this._DefToLocKey = value;
            }
        }
        //Not Save in DB. Work in Form for Detail Item Location
        public int? DefJobKey
        {
            get
            {
                return this._DefJobKey;
            }
            set
            {
                this._DefJobKey = value;
            }
        }
        public int? DefAccKey
        {
            get
            {
                return this._DefAccKey;
            }
            set
            {
                this._DefAccKey = value;
            }
        }
        public string DefBAddrKey
        {
            get
            {
                return this._DefBAddrKey;
            }
            set
            {
                this._DefBAddrKey = value;
            }
        }
        public string DefSAddrKey
        {
            get
            {
                return this._DefSAddrKey;
            }
            set
            {
                this._DefSAddrKey = value;
            }
        }

        
        public int? GUID
        {
            get
            {
                return this._GUID;
            }
            set
            {
                this._GUID = value;
            }
        }
		public bool IsDirty
		{
			get
			{
				return this._isDirty;
			}
            set { this._isDirty = value; }
		}
        public bool IsReadOnly
        {
            get
            {
                return this._isReadOnly;
            }
            set { this._isReadOnly= value; }
        }
        public bool IsValid
        {
            get
            {
                return this._isValid;
            } 
            set { this._isValid = value; }

        }
        public bool IsNew
        {
            get
            {
                return this._isNew;
            }
            set { this._isNew = value; }
        }
        public int? ListDefaultDateRange
        {
            get
            {
                return this._listDefaultDateRange;
            }
            set
            {
                this._listDefaultDateRange = value;
            }
        }

        public bool? ShowEnquiryForm
        {
            get
            {
                return this._ShowEnquiryForm;
            }
            set
            {
                this._ShowEnquiryForm = value;
            }
        }
		#endregion		
	}
}