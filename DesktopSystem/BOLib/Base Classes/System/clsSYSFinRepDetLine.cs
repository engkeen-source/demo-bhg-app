using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for SYSFinRepDetLine.
    /// </summary>
    [Serializable]
    public class SYSFinRepDetLine : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected int? _RepDetLineKey;
        protected int? _FinRepKey;
        protected int? _RepDetKey;
        protected int? _ColKey;
        protected int? _AccType;
        protected string _FromAccID;
        protected string _ToAccID;
        protected string _FromDept;
        protected string _ToDept;
        protected string _FromBranch;
        protected string _ToBranch;
        protected string _TransGroup;
        protected string _SummaryExpression;
        protected string _TotalExpression;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public SYSFinRepDetLine()
            : base()
        {
            this._RepDetLineKey = 0;
            this._FinRepKey = 0;
            this._RepDetKey = 0;
            this._ColKey = 0;
            this._AccType = 0;
            this._FromAccID = string.Empty;
            this._ToAccID = string.Empty;
            this._FromDept = string.Empty;
            this._ToDept = string.Empty;
            this._FromBranch = string.Empty;
            this._ToBranch = string.Empty;
            this._TransGroup = string.Empty;
            this._SummaryExpression = string.Empty;

        }


        public SYSFinRepDetLine Clone()
        {
            SYSFinRepDetLine objCopy = (SYSFinRepDetLine)this.MemberwiseClone();
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

        public int? RepDetLineKey
        {

            get
            {
                return this._RepDetLineKey;
            }
            set
            {
                this._RepDetLineKey = value;
                NotifyPropertyChanged("RepDetLineKey");
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
        public int? AccType
        {

            get
            {
                return this._AccType;
            }
            set
            {
                this._AccType = value;
                NotifyPropertyChanged("AccType");
            }
        }
        public string FromAccID
        {

            get
            {
                return this._FromAccID;
            }
            set
            {
                this._FromAccID = value;
                NotifyPropertyChanged("FromAccID");
            }
        }
        public string ToAccID
        {

            get
            {
                return this._ToAccID;
            }
            set
            {
                this._ToAccID = value;
                NotifyPropertyChanged("ToAccID");
            }
        }
        public string FromDept
        {

            get
            {
                return this._FromDept;
            }
            set
            {
                this._FromDept = value;
                NotifyPropertyChanged("FromDept");
            }
        }
        public string ToDept
        {

            get
            {
                return this._ToDept;
            }
            set
            {
                this._ToDept = value;
                NotifyPropertyChanged("ToDept");
            }
        }
        public string FromBranch
        {

            get
            {
                return this._FromBranch;
            }
            set
            {
                this._FromBranch = value;
                NotifyPropertyChanged("FromBranch");
            }
        }
        public string ToBranch
        {

            get
            {
                return this._ToBranch;
            }
            set
            {
                this._ToBranch = value;
                NotifyPropertyChanged("ToBranch");
            }
        }
        public string TransGroup
        {

            get
            {
                return this._TransGroup;
            }
            set
            {
                this._TransGroup = value;
                NotifyPropertyChanged("TransGroup");
            }
        }
        public string SummaryExpression
        {

            get
            {
                return this._SummaryExpression;
            }
            set
            {
                this._SummaryExpression = value;
                NotifyPropertyChanged("SummaryExpression");
            }
        }
        public string TotalExpression
        {

            get
            {
                return this._TotalExpression;
            }
            set
            {
                this._TotalExpression = value;
                NotifyPropertyChanged("TotalExpression");
            }
        }

        #endregion
    }
}





