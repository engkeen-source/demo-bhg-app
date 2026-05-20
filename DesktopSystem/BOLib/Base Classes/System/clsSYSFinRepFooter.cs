using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for SYSFinRepFooter.
    /// </summary>
    [Serializable]
    public class SYSFinRepFooter : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected int? _RepFootKey;
        protected int? _FinRepKey;
        protected int? _FootLineType;
        protected string _FootLineDesc;
        protected int? _LineSeq;
        protected string _LineText;
        protected string _LineTextRTF;
        protected string _FormulaExp;
        protected string _SummaryExp;
        protected string _Format;
        protected decimal? _Height;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public SYSFinRepFooter()
            : base()
        {
            this._RepFootKey = 0;
            this._FinRepKey = 0;
            this._FootLineType = 0;
            this._FootLineDesc = string.Empty;
            this._LineSeq = 0;
            this._LineText = string.Empty;
            this._LineTextRTF = string.Empty;
            this._FormulaExp = string.Empty;
            this._SummaryExp = string.Empty;

        }


        public SYSFinRepFooter Clone()
        {
            SYSFinRepFooter objCopy = (SYSFinRepFooter)this.MemberwiseClone();
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


        public int? RepFootKey
        {

            get
            {
                return this._RepFootKey;
            }
            set
            {
                this._RepFootKey = value;
                NotifyPropertyChanged("RepFootKey");
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
        public int? FootLineType
        {

            get
            {
                return this._FootLineType;
            }
            set
            {
                this._FootLineType = value;
                NotifyPropertyChanged("FootLineType");
            }
        }
        public string FootLineDesc
        {

            get
            {
                return this._FootLineDesc;
            }
            set
            {
                this._FootLineDesc = value;
                NotifyPropertyChanged("FootLineDesc");
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
        public string LineText
        {

            get
            {
                return this._LineText;
            }
            set
            {
                this._LineText = value;
                NotifyPropertyChanged("LineText");
            }
        }
        public string LineTextRTF
        {

            get
            {
                return this._LineTextRTF;
            }
            set
            {
                this._LineTextRTF = value;
                NotifyPropertyChanged("LineTextRTF");
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

        public string Format
        {

            get
            {
                return this._Format;
            }
            set
            {
                this._Format = value;
                NotifyPropertyChanged("Format");
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





