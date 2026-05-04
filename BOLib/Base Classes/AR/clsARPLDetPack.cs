


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for ARPLDetPack.
    /// </summary>
    [Serializable]
    public class ARPLDetPack : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        
        protected decimal _ItmPackWeightNet;
        protected decimal _ItmPackWeightTare;
        protected decimal _ItmPackWeightGross;
        protected decimal _ItmHeight;
        protected decimal _ItmWidth;
        protected decimal _ItmLength;
        protected decimal _ItmVolume;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public ARPLDetPack()
            : base()
        {
            this._ItmPackWeightNet = 0;
            this._ItmPackWeightTare = 0;
            this._ItmPackWeightGross = 0;
            this._ItmHeight = 0;
            this._ItmWidth = 0;
            this._ItmLength = 0;
            this._ItmVolume = 0;

        }


        public ARPLDetPack Clone()
        {
            ARPLDetPack objCopy = (ARPLDetPack)this.MemberwiseClone();
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


        public decimal ItmPackWeightNet
        {

            get
            {
                return this._ItmPackWeightNet;
            }
            set
            {
                this._ItmPackWeightNet = value;
                NotifyPropertyChanged("ItmPackWeightNet");
            }
        }
        public decimal ItmPackWeightTare
        {

            get
            {
                return this._ItmPackWeightTare;
            }
            set
            {
                this._ItmPackWeightTare = value;
                NotifyPropertyChanged("ItmPackWeightTare");
            }
        }
        public decimal ItmPackWeightGross
        {

            get
            {
                return this._ItmPackWeightGross;
            }
            set
            {
                this._ItmPackWeightGross = value;
                NotifyPropertyChanged("ItmPackWeightGross");
            }
        }
        public decimal ItmHeight
        {

            get
            {
                return this._ItmHeight;
            }
            set
            {
                this._ItmHeight = value;
                NotifyPropertyChanged("ItmHeight");
            }
        }
        public decimal ItmWidth
        {

            get
            {
                return this._ItmWidth;
            }
            set
            {
                this._ItmWidth = value;
                NotifyPropertyChanged("ItmWidth");
            }
        }
        public decimal ItmLength
        {

            get
            {
                return this._ItmLength;
            }
            set
            {
                this._ItmLength = value;
                NotifyPropertyChanged("ItmLength");
            }
        }
        public decimal ItmVolume
        {

            get
            {
                return this._ItmVolume;
            }
            set
            {
                this._ItmVolume = value;
                NotifyPropertyChanged("ItmVolume");
            }
        }
        
        #endregion
    }
}





