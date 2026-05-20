using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for SYSFinRepDet.
    /// </summary>
    [Serializable]
    public class SYSFinRepDet : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected int? _RepDetKey;
        protected int? _FinRepKey;
        protected int? _DetLineType;
        protected string _DetLineDesc;
        protected int? _LineSeq;
        protected string _Remark;
        protected string _FormatExp;
        protected string _FormulaExp;
        protected string _SummaryExp;
        protected string _TotalExp;
        protected decimal? _Height;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public SYSFinRepDet()
            : base()
        {
            this._RepDetKey = 0;
            this._FinRepKey = 0;
            this._DetLineType = 0;
            this._DetLineDesc = string.Empty;
            this._LineSeq = 0;
            this._Remark = string.Empty;
            this._FormatExp = string.Empty;
            this._FormulaExp = string.Empty;
            this._SummaryExp = string.Empty;
            this._TotalExp = string.Empty;

        }


        public SYSFinRepDet Clone()
        {
            SYSFinRepDet objCopy = (SYSFinRepDet)this.MemberwiseClone();
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


        public int? RepDetKey
        {

            get
            {
                return this._RepDetKey;
            }
            set
            {
                this._RepDetKey = value;
                NotifyPropertyChanged("RepDetKey");
            }
        }
        public int? FinRepKey
        {

            get
            {
                return this._FinRepKey;
            }
            set
            {
                this._FinRepKey = value;
                NotifyPropertyChanged("FinRepKey");
            }
        }
        public int? DetLineType
        {

            get
            {
                return this._DetLineType;
            }
            set
            {
                this._DetLineType = value;
                NotifyPropertyChanged("DetLineType");
            }
        }
        public string DetLineDesc
        {

            get
            {
                return this._DetLineDesc;
            }
            set
            {
                this._DetLineDesc = value;
                NotifyPropertyChanged("DetLineDesc");
            }
        }
        public int? LineSeq
        {

            get
            {
                return this._LineSeq;
            }
            set
            {
                this._LineSeq = value;
                NotifyPropertyChanged("LineSeq");
            }
        }
        public string Remark
        {

            get
            {
                return this._Remark;
            }
            set
            {
                this._Remark = value;
                NotifyPropertyChanged("Remark");
            }
        }
        public string FormatExp
        {

            get
            {
                return this._FormatExp;
            }
            set
            {
                this._FormatExp = value;
                NotifyPropertyChanged("FormatExp");
            }
        }
        public string FormulaExp
        {

            get
            {
                return this._FormulaExp;
            }
            set
            {
                this._FormulaExp = value;
                NotifyPropertyChanged("FormulaExp");
            }
        }
        public string SummaryExp
        {

            get
            {
                return this._SummaryExp;
            }
            set
            {
                this._SummaryExp = value;
                NotifyPropertyChanged("SummaryExp");
            }
        }
        public string TotalExp
        {

            get
            {
                return this._TotalExp;
            }
            set
            {
                this._TotalExp = value;
                NotifyPropertyChanged("TotalExp");
            }
        }

        public decimal? Height
        {

            get
            {
                return this._Height;
            }
            set
            {
                this._Height = value;
                NotifyPropertyChanged("Height");
            }
        }
        #endregion
    }
}





