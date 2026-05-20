


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for CSCSIDetExp.
    /// </summary>
    [Serializable]
    public class CSCSIDetExp : DocPayExpense,IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected decimal _ExpAmt;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public CSCSIDetExp()
            : base()
        {
            this._ExpAmt = 0;

        }


        public CSCSIDetExp Clone()
        {
            CSCSIDetExp objCopy = (CSCSIDetExp)this.MemberwiseClone();
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


        public decimal ExpAmt
        {

            get
            {
                return this._ExpAmt;
            }
            set
            {
                this._ExpAmt = value;
                NotifyPropertyChanged("ExpAmt");
            }
        }


        #endregion
    }
}





