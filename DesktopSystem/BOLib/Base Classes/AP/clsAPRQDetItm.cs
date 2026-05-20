


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for APRQDetItm.
    /// </summary>
    [Serializable]
    public class APRQDetItm : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected int _LineType;
        protected int _LineLinkKey;
        protected int _ItmDeptKey;
        protected int? _ItmTranGrpKey;
        protected DateTime? _ItmReqDate;
        protected string _ItmReply;
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

        public APRQDetItm()
            : base()
        {
            this._LineType = 0;
            this._LineLinkKey = 0;
            this._ItmDeptKey = 0;
            this._ItmTranGrpKey = 0;
            this._ItmReqDate = DateTime.Today.Date;
            this._ItmReply = null;
            this._ItmID = string.Empty;
            this._SKU1 = string.Empty;
            this._SKU2 = string.Empty;

        }


        public APRQDetItm Clone()
        {
            APRQDetItm objCopy = (APRQDetItm)this.MemberwiseClone();
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


        public int LineType
        {

            get
            {
                return this._LineType;
            }
            set
            {
                this._LineType = value;
                NotifyPropertyChanged("LineType");
            }
        }
        public int LineLinkKey
        {

            get
            {
                return this._LineLinkKey;
            }
            set
            {
                this._LineLinkKey = value;
                NotifyPropertyChanged("LineLinkKey");
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
        public DateTime? ItmReqDate
        {

            get
            {
                return this._ItmReqDate;
            }
            set
            {
                this._ItmReqDate = value;
                NotifyPropertyChanged("ItmReqDate");
            }
        }
        public string ItmReply
        {

            get
            {
                return this._ItmReply;
            }
            set
            {
                this._ItmReply = value;
                NotifyPropertyChanged("ItmReply");
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





