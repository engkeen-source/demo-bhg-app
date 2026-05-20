using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for SYSFinRepHead.
    /// </summary>
    [Serializable]
    public class SYSFinRepHead : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected int? _RepHeadKey;
        protected int? _FinRepKey;
        protected int? _LineSeq;
        protected string _LineText;
        protected string _LineTextRTF;
        protected string _Format;
        protected decimal? _Height;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public SYSFinRepHead()
            : base()
        {
            this._RepHeadKey = 0;
            this._FinRepKey = 0;
            this._LineSeq = 0;
            this._LineText = string.Empty;
            this._LineTextRTF = string.Empty;

        }


        public SYSFinRepHead Clone()
        {
            SYSFinRepHead objCopy = (SYSFinRepHead)this.MemberwiseClone();
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


        public int? RepHeadKey
        {

            get
            {
                return this._RepHeadKey;
            }
            set
            {
                this._RepHeadKey = value;
                NotifyPropertyChanged("RepHeadKey");
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





