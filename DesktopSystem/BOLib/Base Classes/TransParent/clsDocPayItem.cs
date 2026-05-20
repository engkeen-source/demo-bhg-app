using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace BOLib
{
    /// <summary>
    /// Summary description for APPYDetExp.
    /// </summary>
    [Serializable]
    public class DocPayItem:System.ComponentModel.INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++

        internal int? _DocKey;
        internal int? _DocItmKey;
        internal int? _LinkDocDC;
        internal int? _LinkDocDK;
        internal string _LinkDocID;
        internal DateTime? _LinkDocDate;
        internal int? _LinkDocType;
        internal string _LinkDocTypeNm;
        internal int? _LinkDocDeptKey;
        internal int? _LinkDocTranGrpKey;
        internal int? _LinkDocAccKey;
        internal int? _LinkDocTermKey;
        internal DateTime? _LinkDocDisDate;
        internal DateTime? _LinkDocDueDate;
        internal decimal? _LinkDocGrand;
        internal decimal? _LinkDocHome;
        internal decimal? _LinkDocApplyAmtF;
        internal decimal? _LinkDocApplyAmtH;
        internal int? _LinkDocCurrKey;
        internal decimal? _LinkDocCurrRate;
        internal string _LinkDocRef;
        internal decimal? _ItmApplyDueAmtF;
        internal decimal? _ItmApplyDueAmtH;
        internal decimal? _ItmApplyRate;
        internal decimal? _ItmApplyDisAmtF;
        internal decimal? _ItmApplyDisAmtH;
        internal int? _ItmApplyDisAccKey;
        internal decimal? _ItmApplyDocAmtF;
        internal decimal? _ItmApplyDocAmtH;
        internal decimal? _ItmApplyPayAmtF;
        internal decimal? _ItmApplyPayAmtH;
        internal decimal? _ItmApplyGainAmt;
        internal int? _ItmApplyGainAccKey;
        internal bool? _ItmApplyFull;
        internal bool? _ItmAttachment;
        internal DateTime? _CreateDate;
        internal int? _CreateUserKey;
        internal DateTime? _LastModifiedDate;
        internal int? _LastModifiedUserKey;
        internal string _LastModifiedUserID;
        internal string _Custom1;
        internal string _Custom2;
        internal string _Custom3;
        internal bool? _locked;
        internal bool? _Used;
        internal string _ItmApplyDisAccID;
        internal string _ItmApplyDisAccDes;
        internal string _ItmApplyGainAccID;
        internal string _ItmApplyGainAccDes;

        internal bool _isDirty;
        internal string _error = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

		#region +++  Constructor and destructor codes  +++

		/// <summary>
		/// Default constructor that will initialize all properties with default values.
		/// </summary>

        public DocPayItem()
		{
			this._DocKey = null;
			this._DocItmKey = null;
			this._LinkDocDC = null;
			this._LinkDocDK = null;
			this._LinkDocID = string.Empty;
			this._LinkDocDate = null;
			this._LinkDocType = null;
			this._LinkDocTypeNm = string.Empty;
			this._LinkDocDeptKey = null;
			this._LinkDocTranGrpKey = null;
			this._LinkDocAccKey = null;
			this._LinkDocTermKey = null;
			this._LinkDocDisDate = null;
			this._LinkDocDueDate = null;
			this._LinkDocGrand = null;
			this._LinkDocHome = null;
			this._LinkDocCurrKey = null;
			this._LinkDocCurrRate = null;
			this._LinkDocRef = string.Empty;
			this._ItmApplyDueAmtF = null;
			this._ItmApplyDueAmtH = null;
			this._ItmApplyRate = null;
			this._ItmApplyDisAmtF = null;
			this._ItmApplyDisAmtH = null;
			this._ItmApplyDisAccKey = null;
			this._ItmApplyDocAmtF = null;
			this._ItmApplyDocAmtH = null;
			this._ItmApplyPayAmtF = null;
			this._ItmApplyPayAmtH = null;
			this._ItmApplyGainAmt = null;
			this._ItmApplyGainAccKey = null;
			this._ItmApplyFull = false;
			this._ItmAttachment = false;
			this._CreateDate = null;
			this._CreateUserKey = null;
			this._LastModifiedDate = null;
			this._LastModifiedUserKey = null;
			this._Custom1 = string.Empty;
			this._Custom2 = string.Empty;
			this._Custom3 = string.Empty;
			this._isDirty = false;
		}


        public DocPayItem Clone()
		{

            DocPayItem objCopy = (DocPayItem)this.MemberwiseClone();
			objCopy._isDirty=false;
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
        internal void NotifyPropertyChanged(String info)
        {
            _isDirty = true;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

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

        public bool IsDirty
        {
            get
            {
                return this._isDirty;
            }
        }



        public int? DocKey
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

        public int? DocItmKey
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

        public int? LinkDocDC
        {

            get
            {
                return this._LinkDocDC;
            }
            set
            {
                this._LinkDocDC = value;
                NotifyPropertyChanged("LinkDocDC");
            }
        }

        public int? LinkDocDK
        {

            get
            {
                return this._LinkDocDK;
            }
            set
            {
                this._LinkDocDK = value;
                NotifyPropertyChanged("LinkDocDK");
            }
        }

        public string LinkDocID
        {

            get
            {
                return this._LinkDocID;
            }
            set
            {
                this._LinkDocID = value;
                NotifyPropertyChanged("LinkDocID");
            }
        }

        public DateTime? LinkDocDate
        {

            get
            {
                return this._LinkDocDate;
            }
            set
            {
                this._LinkDocDate = value;
                NotifyPropertyChanged("LinkDocDate");
            }
        }

        public int? LinkDocType
        {

            get
            {
                return this._LinkDocType;
            }
            set
            {
                this._LinkDocType = value;
                NotifyPropertyChanged("LinkDocType");
            }
        }

        public string LinkDocTypeNm
        {

            get
            {
                return this._LinkDocTypeNm;
            }
            set
            {
                this._LinkDocTypeNm = value;
                NotifyPropertyChanged("LinkDocTypeNm");
            }
        }

        public int? LinkDocDeptKey
        {

            get
            {
                return this._LinkDocDeptKey;
            }
            set
            {
                this._LinkDocDeptKey = value;
                NotifyPropertyChanged("LinkDocDeptKey");
            }
        }

        public int? LinkDocTranGrpKey
        {

            get
            {
                return this._LinkDocTranGrpKey;
            }
            set
            {
                this._LinkDocTranGrpKey = value;
                NotifyPropertyChanged("LinkDocTranGrpKey");
            }
        }

        public int? LinkDocAccKey
        {

            get
            {
                return this._LinkDocAccKey;
            }
            set
            {
                this._LinkDocAccKey = value;
                NotifyPropertyChanged("LinkDocAccKey");
            }
        }

        public int? LinkDocTermKey
        {

            get
            {
                return this._LinkDocTermKey;
            }
            set
            {
                this._LinkDocTermKey = value;
                NotifyPropertyChanged("LinkDocTermKey");
            }
        }

        public DateTime? LinkDocDisDate
        {

            get
            {
                return this._LinkDocDisDate;
            }
            set
            {
                this._LinkDocDisDate = value;
                NotifyPropertyChanged("LinkDocDisDate");
            }
        }

        public DateTime? LinkDocDueDate
        {

            get
            {
                return this._LinkDocDueDate;
            }
            set
            {
                this._LinkDocDueDate = value;
                NotifyPropertyChanged("LinkDocDueDate");
            }
        }

        public decimal? LinkDocGrand
        {

            get
            {
                return this._LinkDocGrand;
            }
            set
            {
                this._LinkDocGrand = value;
                NotifyPropertyChanged("LinkDocGrand");
            }
        }

        public decimal? LinkDocHome
        {

            get
            {
                return this._LinkDocHome;
            }
            set
            {
                this._LinkDocHome = value;
                NotifyPropertyChanged("LinkDocHome");
            }
        }

        public decimal? LinkDocApplyAmtF
        {

            get
            {
                return this._LinkDocApplyAmtF;
            }
            set
            {
                this._LinkDocApplyAmtF = value;
                NotifyPropertyChanged("LinkDocApplyAmtF");
            }
        }

        public decimal? LinkDocApplyAmtH
        {

            get
            {
                return this._LinkDocApplyAmtH;
            }
            set
            {
                this._LinkDocApplyAmtH = value;
                NotifyPropertyChanged("LinkDocApplyAmtH");
            }
        }

        public int? LinkDocCurrKey
        {

            get
            {
                return this._LinkDocCurrKey;
            }
            set
            {
                this._LinkDocCurrKey = value;
                NotifyPropertyChanged("LinkDocCurrKey");
            }
        }

        public decimal? LinkDocCurrRate
        {

            get
            {
                return this._LinkDocCurrRate;
            }
            set
            {
                this._LinkDocCurrRate = value;
                NotifyPropertyChanged("LinkDocCurrRate");
            }
        }

        public string LinkDocRef
        {

            get
            {
                return this._LinkDocRef;
            }
            set
            {
                this._LinkDocRef = value;
                NotifyPropertyChanged("LinkDocRef");
            }
        }

        public decimal? ItmApplyDueAmtF
        {

            get
            {
                return this._ItmApplyDueAmtF;
            }
            set
            {
                this._ItmApplyDueAmtF = value;
                NotifyPropertyChanged("ItmApplyDueAmtF");
            }
        }

        public decimal? ItmApplyDueAmtH
        {

            get
            {
                return this._ItmApplyDueAmtH;
            }
            set
            {
                this._ItmApplyDueAmtH = value;
                NotifyPropertyChanged("ItmApplyDueAmtH");
            }
        }

        public decimal? ItmApplyRate
        {

            get
            {
                return this._ItmApplyRate;
            }
            set
            {
                this._ItmApplyRate = value;
                NotifyPropertyChanged("ItmApplyRate");
            }
        }

        public decimal? ItmApplyDisAmtF
        {

            get
            {
                return this._ItmApplyDisAmtF;
            }
            set
            {
                this._ItmApplyDisAmtF = value;
                NotifyPropertyChanged("ItmApplyDisAmtF");
            }
        }

        public decimal? ItmApplyDisAmtH
        {

            get
            {
                return this._ItmApplyDisAmtH;
            }
            set
            {
                this._ItmApplyDisAmtH = value;
                NotifyPropertyChanged("ItmApplyDisAmtH");
            }
        }

        public int? ItmApplyDisAccKey
        {

            get
            {
                return this._ItmApplyDisAccKey;
            }
            set
            {
                this._ItmApplyDisAccKey = value;
                NotifyPropertyChanged("ItmApplyDisAccKey");
            }
        }

        public decimal? ItmApplyDocAmtF
        {

            get
            {
                return this._ItmApplyDocAmtF;
            }
            set
            {
                this._ItmApplyDocAmtF = value;
                NotifyPropertyChanged("ItmApplyDocAmtF");
            }
        }

        public decimal? ItmApplyDocAmtH
        {

            get
            {
                return this._ItmApplyDocAmtH;
            }
            set
            {
                this._ItmApplyDocAmtH = value;
                NotifyPropertyChanged("ItmApplyDocAmtH");
            }
        }

        public decimal? ItmApplyPayAmtF
        {

            get
            {
                return this._ItmApplyPayAmtF;
            }
            set
            {
                this._ItmApplyPayAmtF = value;
                NotifyPropertyChanged("ItmApplyPayAmtF");
            }
        }

        public decimal? ItmApplyPayAmtH
        {

            get
            {
                return this._ItmApplyPayAmtH;
            }
            set
            {
                this._ItmApplyPayAmtH = value;
                NotifyPropertyChanged("ItmApplyPayAmtH");
            }
        }

        public decimal? ItmApplyGainAmt
        {

            get
            {
                return this._ItmApplyGainAmt;
            }
            set
            {
                this._ItmApplyGainAmt = value;
                NotifyPropertyChanged("ItmApplyGainAmt");
            }
        }

        public int? ItmApplyGainAccKey
        {

            get
            {
                return this._ItmApplyGainAccKey;
            }
            set
            {
                this._ItmApplyGainAccKey = value;
                NotifyPropertyChanged("ItmApplyGainAccKey");
            }
        }

        public bool? ItmApplyFull
        {

            get
            {
                return this._ItmApplyFull;
            }
            set
            {
                this._ItmApplyFull = value;
                NotifyPropertyChanged("ItmApplyFull");
            }
        }

        public bool? ItmAttachment
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

        public bool? locked
        {

            get
            {
                return this._locked;
            }
            set
            {
                this._locked = value;
                NotifyPropertyChanged("locked");
            }
        }

        public bool? Used
        {

            get
            {
                return this._Used;
            }
            set
            {
                this._Used = value;
                NotifyPropertyChanged("Used");
            }
        }

        public string ItmApplyDisAccID
        {

            get
            {
                return this._ItmApplyDisAccID;
            }
            set
            {
                this._ItmApplyDisAccID = value;
                NotifyPropertyChanged("ItmApplyDisAccID");
            }
        }

        public string ItmApplyDisAccDes
        {

            get
            {
                return this._ItmApplyDisAccDes;
            }
            set
            {
                this._ItmApplyDisAccDes = value;
                NotifyPropertyChanged("ItmApplyDisAccDes");
            }
        }

        public string ItmApplyGainAccID
        {

            get
            {
                return this._ItmApplyGainAccID;
            }
            set
            {
                this._ItmApplyGainAccID = value;
                NotifyPropertyChanged("ItmApplyGainAccID");
            }
        }

        public string ItmApplyGainAccDes
        {

            get
            {
                return this._ItmApplyGainAccDes;
            }
            set
            {
                this._ItmApplyGainAccDes = value;
                NotifyPropertyChanged("ItmApplyGainAccDes");
            }
        }


        #endregion
    }
}
