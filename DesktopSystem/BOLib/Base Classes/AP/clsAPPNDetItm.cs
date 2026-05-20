


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for APPNDetItm.
    /// </summary>
    [Serializable]
    public class APPNDetItm : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected int _ItmDeptKey;
        protected int? _ItmTranGrpKey;
        protected decimal _ItmQtyM1;
        protected decimal _ItmQtyM2;
        protected decimal _ItmQtyM3;
        protected decimal _ItmQtyM4;
        protected decimal _ItmQtyM5;
        protected decimal _ItmQtyM6;
        protected decimal _ItmQtyM7;
        protected decimal _ItmQtyM8;
        protected decimal _ItmQtyM9;
        protected decimal _ItmQtyM10;
        protected decimal _ItmQtyM11;
        protected decimal _ItmQtyM12;
        protected decimal _ItmQtyMTotal;
        protected string _ItmID;
        protected string _SKU1;
        protected string _SKU2;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public APPNDetItm()
            : base()
        {
            this._ItmDeptKey = 0;
            this._ItmTranGrpKey = 0;
            this._ItmQtyM1 = 0;
            this._ItmQtyM2 = 0;
            this._ItmQtyM3 = 0;
            this._ItmQtyM4 = 0;
            this._ItmQtyM5 = 0;
            this._ItmQtyM6 = 0;
            this._ItmQtyM7 = 0;
            this._ItmQtyM8 = 0;
            this._ItmQtyM9 = 0;
            this._ItmQtyM10 = 0;
            this._ItmQtyM11 = 0;
            this._ItmQtyM12 = 0;
            this._ItmQtyMTotal = 0;
            this._ItmID = string.Empty;
            this._SKU1 = string.Empty;
            this._SKU2 = string.Empty;

        }


        public APPNDetItm Clone()
        {
            APPNDetItm objCopy = (APPNDetItm)this.MemberwiseClone();
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
        public decimal ItmQtyM1
        {

            get
            {
                return this._ItmQtyM1;
            }
            set
            {
                this._ItmQtyM1 = value;
                NotifyPropertyChanged("ItmQtyM1");
            }
        }
        public decimal ItmQtyM2
        {

            get
            {
                return this._ItmQtyM2;
            }
            set
            {
                this._ItmQtyM2 = value;
                NotifyPropertyChanged("ItmQtyM2");
            }
        }
        public decimal ItmQtyM3
        {

            get
            {
                return this._ItmQtyM3;
            }
            set
            {
                this._ItmQtyM3 = value;
                NotifyPropertyChanged("ItmQtyM3");
            }
        }
        public decimal ItmQtyM4
        {

            get
            {
                return this._ItmQtyM4;
            }
            set
            {
                this._ItmQtyM4 = value;
                NotifyPropertyChanged("ItmQtyM4");
            }
        }
        public decimal ItmQtyM5
        {

            get
            {
                return this._ItmQtyM5;
            }
            set
            {
                this._ItmQtyM5 = value;
                NotifyPropertyChanged("ItmQtyM5");
            }
        }
        public decimal ItmQtyM6
        {

            get
            {
                return this._ItmQtyM6;
            }
            set
            {
                this._ItmQtyM6 = value;
                NotifyPropertyChanged("ItmQtyM6");
            }
        }
        public decimal ItmQtyM7
        {

            get
            {
                return this._ItmQtyM7;
            }
            set
            {
                this._ItmQtyM7 = value;
                NotifyPropertyChanged("ItmQtyM7");
            }
        }
        public decimal ItmQtyM8
        {

            get
            {
                return this._ItmQtyM8;
            }
            set
            {
                this._ItmQtyM8 = value;
                NotifyPropertyChanged("ItmQtyM8");
            }
        }
        public decimal ItmQtyM9
        {

            get
            {
                return this._ItmQtyM9;
            }
            set
            {
                this._ItmQtyM9 = value;
                NotifyPropertyChanged("ItmQtyM9");
            }
        }
        public decimal ItmQtyM10
        {

            get
            {
                return this._ItmQtyM10;
            }
            set
            {
                this._ItmQtyM10 = value;
                NotifyPropertyChanged("ItmQtyM10");
            }
        }
        public decimal ItmQtyM11
        {

            get
            {
                return this._ItmQtyM11;
            }
            set
            {
                this._ItmQtyM11 = value;
                NotifyPropertyChanged("ItmQtyM11");
            }
        }
        public decimal ItmQtyM12
        {

            get
            {
                return this._ItmQtyM12;
            }
            set
            {
                this._ItmQtyM12 = value;
                NotifyPropertyChanged("ItmQtyM12");
            }
        }
        public decimal ItmQtyMTotal
        {

            get
            {
                return this._ItmQtyMTotal;
            }
            set
            {
                this._ItmQtyMTotal = value;
                NotifyPropertyChanged("ItmQtyMTotal");
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


        #endregion
    }
}





