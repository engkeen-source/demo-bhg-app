


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for GLDPDetItm.
    /// </summary>
    [Serializable]
    public class GLDPDetItm : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected string _ItmReFrom;
        protected int _ItmDocDC;
        protected int _ItmDocDK;
        protected int _ItmDocDeptKey;
        protected int? _ItmTranGrpKey;
        protected int _ItmDocAccKey;
        protected string _ItmDocRef;
        protected DateTime? _ItmDocChqDate;
        protected string _ItmDocChqNum;
        protected int _ItmDocCurrKey;
        protected decimal _ItmDocCurrRate;
        protected decimal _ItmDocAmtF;
        protected decimal _ItmDocAmtH;
        protected decimal _ItmBankRate;
        protected decimal _ItmBankAmtF;
        protected decimal _ItmBankAmtH;
        protected string _ItmDocAccID;
        protected string _ItmDocAccDes;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public GLDPDetItm()
            : base()
        {
            this._ItmReFrom = string.Empty;
            this._ItmDocDC = 0;
            this._ItmDocDK = 0;
            this._ItmDocDeptKey = 0;
            this._ItmTranGrpKey = 0;
            this._ItmDocAccKey = 0;
            this._ItmDocRef = null;
            this._ItmDocChqDate = DateTime.Today.Date;
            this._ItmDocChqNum = null;
            this._ItmDocCurrKey = 0;
            this._ItmDocCurrRate = 0;
            this._ItmDocAmtF = 0;
            this._ItmDocAmtH = 0;
            this._ItmBankRate = 0;
            this._ItmBankAmtF = 0;
            this._ItmBankAmtH = 0;
            this._ItmDocAccID = string.Empty;
            this._ItmDocAccDes = string.Empty;

        }


        public GLDPDetItm Clone()
        {
            GLDPDetItm objCopy = (GLDPDetItm)this.MemberwiseClone();
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


        public string ItmReFrom
        {

            get
            {
                return this._ItmReFrom;
            }
            set
            {
                this._ItmReFrom = value;
                NotifyPropertyChanged("ItmReFrom");
            }
        }
        public int ItmDocDC
        {

            get
            {
                return this._ItmDocDC;
            }
            set
            {
                this._ItmDocDC = value;
                NotifyPropertyChanged("ItmDocDC");
            }
        }
        public int ItmDocDK
        {

            get
            {
                return this._ItmDocDK;
            }
            set
            {
                this._ItmDocDK = value;
                NotifyPropertyChanged("ItmDocDK");
            }
        }
        public int ItmDocDeptKey
        {

            get
            {
                return this._ItmDocDeptKey;
            }
            set
            {
                this._ItmDocDeptKey = value;
                NotifyPropertyChanged("ItmDocDeptKey");
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
        public int ItmDocAccKey
        {

            get
            {
                return this._ItmDocAccKey;
            }
            set
            {
                this._ItmDocAccKey = value;
                NotifyPropertyChanged("ItmDocAccKey");
            }
        }
        public string ItmDocRef
        {

            get
            {
                return this._ItmDocRef;
            }
            set
            {
                this._ItmDocRef = value;
                NotifyPropertyChanged("ItmDocRef");
            }
        }
        public DateTime? ItmDocChqDate
        {

            get
            {
                return this._ItmDocChqDate;
            }
            set
            {
                this._ItmDocChqDate = value;
                NotifyPropertyChanged("ItmDocChqDate");
            }
        }
        public string ItmDocChqNum
        {

            get
            {
                return this._ItmDocChqNum;
            }
            set
            {
                this._ItmDocChqNum = value;
                NotifyPropertyChanged("ItmDocChqNum");
            }
        }
        public int ItmDocCurrKey
        {

            get
            {
                return this._ItmDocCurrKey;
            }
            set
            {
                this._ItmDocCurrKey = value;
                NotifyPropertyChanged("ItmDocCurrKey");
            }
        }
        public decimal ItmDocCurrRate
        {

            get
            {
                return this._ItmDocCurrRate;
            }
            set
            {
                this._ItmDocCurrRate = value;
                NotifyPropertyChanged("ItmDocCurrRate");
            }
        }
        public decimal ItmDocAmtF
        {

            get
            {
                return this._ItmDocAmtF;
            }
            set
            {
                this._ItmDocAmtF = value;
                NotifyPropertyChanged("ItmDocAmtF");
            }
        }
        public decimal ItmDocAmtH
        {

            get
            {
                return this._ItmDocAmtH;
            }
            set
            {
                this._ItmDocAmtH = value;
                NotifyPropertyChanged("ItmDocAmtH");
            }
        }
        public decimal ItmBankRate
        {

            get
            {
                return this._ItmBankRate;
            }
            set
            {
                this._ItmBankRate = value;
                NotifyPropertyChanged("ItmBankRate");
            }
        }
        public decimal ItmBankAmtF
        {

            get
            {
                return this._ItmBankAmtF;
            }
            set
            {
                this._ItmBankAmtF = value;
                NotifyPropertyChanged("ItmBankAmtF");
            }
        }
        public decimal ItmBankAmtH
        {

            get
            {
                return this._ItmBankAmtH;
            }
            set
            {
                this._ItmBankAmtH = value;
                NotifyPropertyChanged("ItmBankAmtH");
            }
        }
        public string ItmDocAccID
        {

            get
            {
                return this._ItmDocAccID;
            }
            set
            {
                this._ItmDocAccID = value;
                NotifyPropertyChanged("ItmDocAccID");
            }
        }
        public string ItmDocAccDes
        {

            get
            {
                return this._ItmDocAccDes;
            }
            set
            {
                this._ItmDocAccDes = value;
                NotifyPropertyChanged("ItmDocAccDes");
            }
        }


        #endregion
    }
}





