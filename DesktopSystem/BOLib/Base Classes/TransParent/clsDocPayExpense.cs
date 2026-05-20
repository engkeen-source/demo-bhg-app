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
    public class DocPayExpense : INotifyPropertyChanged
    {
      	#region +++  Local variables declaration for the class +++

        internal int _DocKey;
        internal int _DocItmKey;
        internal decimal _ExpSN;
        internal int _ExpDeptKey;
        internal int? _ExpTranGrpKey;
        internal int _ExpAccKey;
        internal DateTime? _ExpDate;
        internal string _ExpRef;
        internal string _ExpDes;
        internal decimal _ExpAmtF;
        internal decimal _ExpAmtH;
        internal decimal _ExpAmtGST;
        internal int _ExpFreightCostKey;
        internal bool _ExpTaxable;
        internal int? _ExpTaxGrpKey;
        internal decimal? _ExpTaxGrpRate;
        internal decimal? _ExpTaxGrpAmtF;
        internal decimal? _ExpTaxGrpAmtL;
        internal int _ExpJobKey;
        internal int _ExpJobPhaseKey;
        internal int _ExpJobTaskKey;
        internal int _ExpJobCostTypeKey;
        internal bool _ExpAttachment;
        internal DateTime? _CreateDate;
        internal int? _CreateUserKey;
        internal DateTime? _LastModifiedDate;
        internal int? _LastModifiedUserKey;
        internal string _LastModifiedUserID;
        internal string _Custom1;
        internal string _Custom2;
        internal string _Custom3;
        internal string _ExpAccID;
        internal string _ExpAccDes;
		internal bool _isDirty;
		internal string _error = string.Empty;

		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region +++  Constructor and destructor codes  +++

		/// <summary>
		/// Default constructor that will initialize all properties with default values.
		/// </summary>

        public DocPayExpense()
		{
            this._DocKey = 0;
            this._DocItmKey = 0;
            this._ExpSN = 0;
            this._ExpDeptKey = 0;
            this._ExpTranGrpKey = 0;
            this._ExpAccKey = 0;
            this._ExpDate = DateTime.Today.Date;
            this._ExpRef = null;
            this._ExpDes = null;
            this._ExpAmtF = 0;
            this._ExpAmtH = 0;
            this._ExpAmtGST = 0;
            this._ExpFreightCostKey = 0;
            this._ExpTaxable = false;
            this._ExpTaxGrpKey = null;
            this._ExpTaxGrpRate = 0;
            this._ExpTaxGrpAmtF = 0;
            this._ExpTaxGrpAmtL = 0;
            this._ExpJobKey = 0;
            this._ExpJobPhaseKey = 0;
            this._ExpJobTaskKey = 0;
            this._ExpJobCostTypeKey = 0;
            this._ExpAttachment = false;
            this._CreateDate = DateTime.Today.Date;
            this._CreateUserKey = null;
            this._LastModifiedDate = DateTime.Today.Date;
            this._LastModifiedUserKey = null;
            this._Custom1 = null;
            this._Custom2 = null;
            this._Custom3 = null;
            this._ExpAccID = string.Empty;
            this._ExpAccDes = string.Empty;
			this._isDirty = false;
		}


        public DocPayExpense Clone()
		{

            DocPayExpense objCopy = (DocPayExpense)this.MemberwiseClone();
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

        public decimal ExpSN
        {

            get
            {
                return this._ExpSN;
            }
            set
            {
                this._ExpSN = value;
                NotifyPropertyChanged("ExpSN");
            }
        }

        public int ExpDeptKey
        {

            get
            {
                return this._ExpDeptKey;
            }
            set
            {
                this._ExpDeptKey = value;
                NotifyPropertyChanged("ExpDeptKey");
            }
        }

        public int? ExpTranGrpKey
        {

            get
            {
                return this._ExpTranGrpKey;
            }
            set
            {
                this._ExpTranGrpKey = value;
                NotifyPropertyChanged("ExpTranGrpKey");
            }
        }

        public int ExpAccKey
        {

            get
            {
                return this._ExpAccKey;
            }
            set
            {
                this._ExpAccKey = value;
                NotifyPropertyChanged("ExpAccKey");
            }
        }

        public DateTime? ExpDate
        {

            get
            {
                return this._ExpDate;
            }
            set
            {
                this._ExpDate = value;
                NotifyPropertyChanged("ExpDate");
            }
        }

        public string ExpRef
        {

            get
            {
                return this._ExpRef;
            }
            set
            {
                this._ExpRef = value;
                NotifyPropertyChanged("ExpRef");
            }
        }

        public string ExpDes
        {

            get
            {
                return this._ExpDes;
            }
            set
            {
                this._ExpDes = value;
                NotifyPropertyChanged("ExpDes");
            }
        }

        public decimal ExpAmtF
        {

            get
            {
                return this._ExpAmtF;
            }
            set
            {
                this._ExpAmtF = value;
                NotifyPropertyChanged("ExpAmtF");
            }
        }

        public decimal ExpAmtH
        {

            get
            {
                return this._ExpAmtH;
            }
            set
            {
                this._ExpAmtH = value;
                NotifyPropertyChanged("ExpAmtH");
            }
        }

        public decimal ExpAmtGST
        {

            get
            {
                return this._ExpAmtGST;
            }
            set
            {
                this._ExpAmtGST = value;
                NotifyPropertyChanged("ExpAmtGST");
            }
        }

        public int ExpFreightCostKey
        {

            get
            {
                return this._ExpFreightCostKey;
            }
            set
            {
                this._ExpAmtGST = value;
                NotifyPropertyChanged("ExpFreightCostKey");
            }
        }

        public bool ExpTaxable
        {

            get
            {
                return this._ExpTaxable;
            }
            set
            {
                this._ExpTaxable = value;
                NotifyPropertyChanged("ExpTaxable");
            }
        }

        public int? ExpTaxGrpKey
        {

            get
            {
                return this._ExpTaxGrpKey;
            }
            set
            {
                this._ExpTaxGrpKey = value;
                NotifyPropertyChanged("ExpTaxGrpKey");
            }
        }

        public decimal? ExpTaxGrpRate
        {

            get
            {
                return this._ExpTaxGrpRate;
            }
            set
            {
                this._ExpTaxGrpRate = value;
                NotifyPropertyChanged("ExpTaxGrpRate");
            }
        }

        public decimal? ExpTaxGrpAmtF
        {

            get
            {
                return this._ExpTaxGrpAmtF;
            }
            set
            {
                this._ExpTaxGrpAmtF = value;
                NotifyPropertyChanged("ExpTaxGrpAmtF");
            }
        }

        public decimal? ExpTaxGrpAmtL
        {

            get
            {
                return this._ExpTaxGrpAmtL;
            }
            set
            {
                this._ExpTaxGrpAmtL = value;
                NotifyPropertyChanged("ExpTaxGrpAmtL");
            }
        }

        public int ExpJobKey
        {

            get
            {
                return this._ExpJobKey;
            }
            set
            {
                this._ExpJobKey = value;
                NotifyPropertyChanged("ExpJobKey");
            }
        }

        public int ExpJobPhaseKey
        {

            get
            {
                return this._ExpJobPhaseKey;
            }
            set
            {
                this._ExpJobPhaseKey = value;
                NotifyPropertyChanged("ExpJobPhaseKey");
            }
        }

        public int ExpJobTaskKey
        {

            get
            {
                return this._ExpJobTaskKey;
            }
            set
            {
                this._ExpJobTaskKey = value;
                NotifyPropertyChanged("ExpJobTaskKey");
            }
        }

        public int ExpJobCostTypeKey
        {

            get
            {
                return this._ExpJobCostTypeKey;
            }
            set
            {
                this._ExpJobCostTypeKey = value;
                NotifyPropertyChanged("ExpJobCostTypeKey");
            }
        }

        public bool ExpAttachment
        {

            get
            {
                return this._ExpAttachment;
            }
            set
            {
                this._ExpAttachment = value;
                NotifyPropertyChanged("ExpAttachment");
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

        public string ExpAccID
        {

            get
            {
                return this._ExpAccID;
            }
            set
            {
                this._ExpAccID = value;
                NotifyPropertyChanged("ExpAccID");
            }
        }

        public string ExpAccDes
        {

            get
            {
                return this._ExpAccDes;
            }
            set
            {
                this._ExpAccDes = value;
                NotifyPropertyChanged("ExpAccDes");
            }
        }


        #endregion
    }
}
