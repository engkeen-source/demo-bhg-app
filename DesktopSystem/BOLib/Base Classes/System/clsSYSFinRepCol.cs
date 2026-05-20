using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for SYSFinRepCol.
    /// </summary>
    [Serializable]
    public class SYSFinRepCol : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected int? _ColKey;
        protected int? _FinRepKey;
        protected int? _RepDetKey;
        protected int? _RepFootKey;
        protected int? _ColType;
        protected string _ColDesc;
        protected string _ColTitle;
        protected string _ColTitleRTF;
        protected int? _ColSeq;
        protected string _ColTypeExp;
        protected string _ColFormulaExp;
        protected bool? _ColDisplay;
        protected string _Format;
        protected decimal _Width;
        

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public SYSFinRepCol()
            : base()
        {
            this._ColKey = 0;
            this._FinRepKey = 0;
            this._RepDetKey = 0;
            this._RepFootKey = 0;
            this._ColType = 0;
            this._ColDesc = string.Empty;
            this._ColTitle = string.Empty;
            this._ColTitleRTF = string.Empty;
            this._ColSeq = 0;
            this._ColTypeExp = string.Empty;
            this._ColFormulaExp = string.Empty;
            this._ColDisplay = false;
            this._Format = string.Empty;
            this._Width = 0;
        }
         

        public SYSFinRepCol Clone()
        {
            SYSFinRepCol objCopy = (SYSFinRepCol)this.MemberwiseClone();
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


        public int? ColKey
        {

            get
            {
                return this._ColKey;
            }
            set
            {
                this._ColKey = value;
                NotifyPropertyChanged("ColKey");
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
        public int? ColType
        {

            get
            {
                return this._ColType;
            }
            set
            {
                this._ColType = value;
                NotifyPropertyChanged("ColType");
            }
        }
        public string ColDesc
        {

            get
            {
                return this._ColDesc;
            }
            set
            {
                this._ColDesc = value;
                NotifyPropertyChanged("ColDesc");
            }
        }
        public string ColTitle
        {

            get
            {
                return this._ColTitle;
            }
            set
            {
                this._ColTitle = value;
                NotifyPropertyChanged("ColTitle");
            }
        }
        public string ColTitleRTF
        {

            get
            {
                return this._ColTitleRTF;
            }
            set
            {
                this._ColTitleRTF = value;
                NotifyPropertyChanged("ColTitleRTF");
            }
        }
        public int? ColSeq
        {

            get
            {
                return this._ColSeq;
            }
            set
            {
                this._ColSeq = value;
                NotifyPropertyChanged("ColSeq");
            }
        }
        public string ColTypeExp
        {

            get
            {
                return this._ColTypeExp;
            }
            set
            {
                this._ColTypeExp = value;
                NotifyPropertyChanged("ColTypeExp");
            }
        }
        public string ColFormulaExp
        {

            get
            {
                return this._ColFormulaExp;
            }
            set
            {
                this._ColFormulaExp = value;
                NotifyPropertyChanged("ColFormulaExp");
            }
        }
        public bool? ColDisplay
        {

            get
            {
                return this._ColDisplay;
            }
            set
            {
                this._ColDisplay = value;
                NotifyPropertyChanged("ColDisplay");
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

        public decimal Width 
        {
            get
            { 
                return _Width; 
            }
            set
            { 
                this.Width = value;
                NotifyPropertyChanged("Width");
            }
        }
        
        #endregion
    }
}





