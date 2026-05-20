


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for GLJNLDetItm.
    /// </summary>
    [Serializable]
    public class GLJNLDetItm : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected string _ItmRef;
        protected decimal _ItmCurrRate;
        protected decimal _ItmCreditF;
        protected decimal _ItmDebitF;
        protected decimal _ItmCreditH;
        protected decimal _ItmDebitH;
        protected int _ItmCurrKey;
        protected int _ItmDeptKey;
        protected int? _ItmTranGrpKey;
        protected int _ItmFreightCostKey;
        protected int _ItmAccKey;
        protected decimal _ItmCountryRate;
        protected string _ItmAccID;
        protected string _ItmAccDes;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public GLJNLDetItm()
            : base()
        {
            this._ItmRef = null;
            this._ItmCurrRate = 0;
            this._ItmCreditF = 0;
            this._ItmDebitF = 0;
            this._ItmCreditH = 0;
            this._ItmDebitH = 0;
            this._ItmCurrKey = 0;
            this._ItmDeptKey = 0;
            this._ItmTranGrpKey = 0;
            this._ItmFreightCostKey = 0;
            this._ItmAccKey = 0;
            this._ItmCountryRate = 0;
            this._ItmAccID = string.Empty;
            this._ItmAccDes = string.Empty;

        }


        public GLJNLDetItm Clone()
        {
            GLJNLDetItm objCopy = (GLJNLDetItm)this.MemberwiseClone();
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
        public decimal ItmCurrRate
        {

            get
            {
                return this._ItmCurrRate;
            }
            set
            {
                this._ItmCurrRate = value;
                NotifyPropertyChanged("ItmCurrRate");
            }
        }
        public decimal ItmCreditF
        {

            get
            {
                return this._ItmCreditF;
            }
            set
            {
                this._ItmCreditF = value;
                NotifyPropertyChanged("ItmCreditF");
            }
        }
        public decimal ItmDebitF
        {

            get
            {
                return this._ItmDebitF;
            }
            set
            {
                this._ItmDebitF = value;
                NotifyPropertyChanged("ItmDebitF");
            }
        }
        public decimal ItmCreditH
        {

            get
            {
                return this._ItmCreditH;
            }
            set
            {
                this._ItmCreditH = value;
                NotifyPropertyChanged("ItmCreditH");
            }
        }
        public decimal ItmDebitH
        {

            get
            {
                return this._ItmDebitH;
            }
            set
            {
                this._ItmDebitH = value;
                NotifyPropertyChanged("ItmDebitH");
            }
        }
        public int ItmCurrKey
        {

            get
            {
                return this._ItmCurrKey;
            }
            set
            {
                this._ItmCurrKey = value;
                NotifyPropertyChanged("ItmCurrKey");
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

        public int ItmFreightCostKey
        {

            get
            {
                return this._ItmFreightCostKey;
            }
            set
            {
                this._ItmTranGrpKey = value;
                NotifyPropertyChanged("ItmFreightCostKey");
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
        public decimal ItmCountryRate
        {

            get
            {
                return this._ItmCountryRate;
            }
            set
            {
                this._ItmCountryRate = value;
                NotifyPropertyChanged("ItmCountryRate");
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


        #endregion
    }
}





