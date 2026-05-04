


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for ARCTDetItm.
    /// </summary>
    [Serializable]
    public class ARCTDetItm : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected int _LinkDocDC;
        protected int _LinkDocDK;
        protected string _LinkDocID;
        protected DateTime _LinkDocDate;
        protected int _LinkDocType;
        protected string _LinkDocTypeNm;
        protected int _LinkDocDeptKey;
        protected int? _LinkDocTranGrpKey;
        protected int _LinkDocAccKey;
        protected int? _LinkDocTermKey;
        protected DateTime? _LinkDocDisDate;
        protected DateTime? _LinkDocDueDate;
        protected decimal _LinkDocGrand;
        protected decimal _LinkDocHome;
        protected int _LinkDocCurrKey;
        protected decimal _LinkDocCurrRate;
        protected string _LinkDocRef;
        protected decimal _ItmApplyDueAmtF;
        protected decimal _ItmApplyDueAmtH;
        protected decimal _ItmApplyRate;
        protected decimal _ItmApplyDisAmtF;
        protected decimal _ItmApplyDisAmtH;
        protected int? _ItmApplyDisAccKey;
        protected decimal _ItmApplyDocAmtF;
        protected decimal _ItmApplyDocAmtH;
        protected decimal _ItmApplyPayAmtF;
        protected decimal _ItmApplyPayAmtH;
        protected decimal _ItmApplyGainAmt;
        protected int? _ItmApplyGainAccKey;
        protected bool _ItmApplyFull;
        protected string _ItmApplyDisAccID;
        protected string _ItmApplyDisAccDes;
        protected string _ItmApplyGainAccID;
        protected string _ItmApplyGainAccDes;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public ARCTDetItm()
            : base()
        {
            this._LinkDocDC = 0;
            this._LinkDocDK = 0;
            this._LinkDocID = string.Empty;
            this._LinkDocDate = DateTime.Today.Date;
            this._LinkDocType = 0;
            this._LinkDocTypeNm = string.Empty;
            this._LinkDocDeptKey = 0;
            this._LinkDocTranGrpKey = 0;
            this._LinkDocAccKey = 0;
            this._LinkDocTermKey = null;
            this._LinkDocDisDate = DateTime.Today.Date;
            this._LinkDocDueDate = DateTime.Today.Date;
            this._LinkDocGrand = 0;
            this._LinkDocHome = 0;
            this._LinkDocCurrKey = 0;
            this._LinkDocCurrRate = 0;
            this._LinkDocRef = null;
            this._ItmApplyDueAmtF = 0;
            this._ItmApplyDueAmtH = 0;
            this._ItmApplyRate = 0;
            this._ItmApplyDisAmtF = 0;
            this._ItmApplyDisAmtH = 0;
            this._ItmApplyDisAccKey = null;
            this._ItmApplyDocAmtF = 0;
            this._ItmApplyDocAmtH = 0;
            this._ItmApplyPayAmtF = 0;
            this._ItmApplyPayAmtH = 0;
            this._ItmApplyGainAmt = 0;
            this._ItmApplyGainAccKey = null;
            this._ItmApplyFull = false;
            this._ItmApplyDisAccID = string.Empty;
            this._ItmApplyDisAccDes = string.Empty;
            this._ItmApplyGainAccID = string.Empty;
            this._ItmApplyGainAccDes = string.Empty;

        }


        public ARCTDetItm Clone()
        {
            ARCTDetItm objCopy = (ARCTDetItm)this.MemberwiseClone();
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


        public int LinkDocDC
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
        public int LinkDocDK
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
        public DateTime LinkDocDate
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
        public int LinkDocType
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
        public int LinkDocDeptKey
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
        public int LinkDocAccKey
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
        public decimal LinkDocGrand
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
        public decimal LinkDocHome
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
        public int LinkDocCurrKey
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
        public decimal LinkDocCurrRate
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
        public decimal ItmApplyDueAmtF
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
        public decimal ItmApplyDueAmtH
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
        public decimal ItmApplyRate
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
        public decimal ItmApplyDisAmtF
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
        public decimal ItmApplyDisAmtH
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
        public decimal ItmApplyDocAmtF
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
        public decimal ItmApplyDocAmtH
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
        public decimal ItmApplyPayAmtF
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
        public decimal ItmApplyPayAmtH
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
        public decimal ItmApplyGainAmt
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
        public bool ItmApplyFull
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





