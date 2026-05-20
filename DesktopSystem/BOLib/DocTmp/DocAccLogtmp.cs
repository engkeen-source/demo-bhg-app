using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for zDTAccLogtmp.
    /// </summary>
    [Serializable]
    public class DocAccLogTmp : INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++

        private int? _LogKey;
        private string _Trans;
        private int? _DC;
        private int? _DK;
        private int? _DItm;
        private int? _Tax;
        private int? _Acc;
        private int? _INKey;
        private string _ID;
        private DateTime? _Date;
        private int? _P;
        private int? _Dept;
        private int? _TranGrp;
        private int? _Grp;
        private string _DT;
        private string _DTUser;
        private int? _Curr;
        private decimal? _CRate;
        private string _CV;
        private string _Ref;
        private string _Des;
        private float? _Seq;
        private decimal? _FC;
        private decimal? _FD;
        private decimal? _HC;
        private decimal? _HD;
        private decimal? _TaxSub;
        private bool _Deposit;
        private bool _Recon;
        private decimal? _COSqty;
        private decimal? _COScost;
        private bool _isDirty;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public DocAccLogTmp()
        {
            this._LogKey = null;
            this._Trans = string.Empty;
            this._DC = null;
            this._DK = null;
            this._DItm = null;
            this._Tax = null;
            this._Acc = null;
            this._INKey = null;
            this._ID = string.Empty;
            this._Date = null;
            this._P = null;
            this._Dept = null;
            this._TranGrp = null;
            this._Grp = null;
            this._DT = string.Empty;
            this._DTUser = string.Empty;
            this._Curr = null;
            this._CRate = null;
            this._CV = string.Empty;
            this._Ref = string.Empty;
            this._Des = string.Empty;
            this._Seq = null;
            this._FC = null;
            this._FD = null;
            this._HC = null;
            this._HD = null;
            this._TaxSub = null;
            this._Deposit = false;
            this._Recon = false;
            this._COSqty = null;
            this._COScost = null;
            this._isDirty = false;
        }


        public DocAccLogTmp Clone()
        {

            DocAccLogTmp objCopy = (DocAccLogTmp)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
        }


        /// <summary>
        /// Disposing objects
        /// </summary>
        public void Dispose()
        {
            if (this._Seq != null)
                this._Seq = null;
        }

        #endregion

        #region +++  Properties  +++

        private void NotifyPropertyChanged(String info)
        {
            _isDirty = true;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public int? LogKey
        {
            get
            {
                return this._LogKey;
            }
            set
            {
                this._LogKey = value;
                NotifyPropertyChanged("LogKey");
            }
        }

        public string Trans
        {
            get
            {
                return this._Trans;
            }
            set
            {
                this._Trans = value;
                NotifyPropertyChanged("Trans");
            }
        }

        public int? DC
        {
            get
            {
                return this._DC;
            }
            set
            {
                this._DC = value;
                NotifyPropertyChanged("DC");
            }
        }

        public int? DK
        {
            get
            {
                return this._DK;
            }
            set
            {
                this._DK = value;
                NotifyPropertyChanged("DK");
            }
        }

        public int? DItm
        {
            get
            {
                return this._DItm;
            }
            set
            {
                this._DItm = value;
                NotifyPropertyChanged("DItm");
            }
        }

        public int? Tax
        {
            get
            {
                return this._Tax;
            }
            set
            {
                this._Tax = value;
                NotifyPropertyChanged("Tax");
            }
        }

        public int? Acc
        {
            get
            {
                return this._Acc;
            }
            set
            {
                this._Acc = value;
                NotifyPropertyChanged("Acc");
            }
        }

        public int? INKey
        {
            get
            {
                return this._INKey;
            }
            set
            {
                this._INKey = value;
                NotifyPropertyChanged("INKey");
            }
        }

        public string ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this._ID = value;
                NotifyPropertyChanged("ID");
            }
        }

        public DateTime? Date
        {
            get
            {
                return this._Date;
            }
            set
            {
                this._Date = value;
                NotifyPropertyChanged("Date");
            }
        }

        public int? P
        {
            get
            {
                return this._P;
            }
            set
            {
                this._P = value;
                NotifyPropertyChanged("P");
            }
        }

        public int? Dept
        {
            get
            {
                return this._Dept;
            }
            set
            {
                this._Dept = value;
                NotifyPropertyChanged("Dept");
            }
        }

        public int? TranGrp
        {
            get
            {
                return this._TranGrp;
            }
            set
            {
                this._TranGrp = value;
                NotifyPropertyChanged("TranGrp");
            }
        }

        public int? Grp
        {
            get
            {
                return this._Grp;
            }
            set
            {
                this._Grp = value;
                NotifyPropertyChanged("Grp");
            }
        }

        public string DT
        {
            get
            {
                return this._DT;
            }
            set
            {
                this._DT = value;
                NotifyPropertyChanged("DT");
            }
        }

        public string DTUser
        {
            get
            {
                return this._DTUser;
            }
            set
            {
                this._DTUser = value;
                NotifyPropertyChanged("DTUser");
            }
        }

        public int? Curr
        {
            get
            {
                return this._Curr;
            }
            set
            {
                this._Curr = value;
                NotifyPropertyChanged("Curr");
            }
        }

        public decimal? CRate
        {
            get
            {
                return this._CRate;
            }
            set
            {
                this._CRate = value;
                NotifyPropertyChanged("CRate");
            }
        }

        public string CV
        {
            get
            {
                return this._CV;
            }
            set
            {
                this._CV = value;
                NotifyPropertyChanged("CV");
            }
        }

        public string Ref
        {
            get
            {
                return this._Ref;
            }
            set
            {
                this._Ref = value;
                NotifyPropertyChanged("Ref");
            }
        }

        public string Des
        {
            get
            {
                return this._Des;
            }
            set
            {
                this._Des = value;
                NotifyPropertyChanged("Des");
            }
        }

        public float? Seq
        {
            get
            {
                return this._Seq;
            }
            set
            {
                this._Seq = value;
                NotifyPropertyChanged("Seq");
            }
        }

        public decimal? FC
        {
            get
            {
                return this._FC;
            }
            set
            {
                this._FC = value;
                NotifyPropertyChanged("FC");
            }
        }

        public decimal? FD
        {
            get
            {
                return this._FD;
            }
            set
            {
                this._FD = value;
                NotifyPropertyChanged("FD");
            }
        }

        public decimal? HC
        {
            get
            {
                return this._HC;
            }
            set
            {
                this._HC = value;
                NotifyPropertyChanged("HC");
            }
        }

        public decimal? HD
        {
            get
            {
                return this._HD;
            }
            set
            {
                this._HD = value;
                NotifyPropertyChanged("HD");
            }
        }

        public decimal? TaxSub
        {
            get
            {
                return this._TaxSub;
            }
            set
            {
                this._TaxSub = value;
                NotifyPropertyChanged("TaxSub");
            }
        }

        public bool Deposit
        {
            get
            {
                return this._Deposit;
            }
            set
            {
                this._Deposit = value;
                NotifyPropertyChanged("Deposit");
            }
        }

        public bool Recon
        {
            get
            {
                return this._Recon;
            }
            set
            {
                this._Recon = value;
                NotifyPropertyChanged("Recon");
            }
        }

        public decimal? COSqty
        {
            get
            {
                return this._COSqty;
            }
            set
            {
                this._COSqty = value;
                NotifyPropertyChanged("COSqty");
            }
        }

        public decimal? COScost
        {
            get
            {
                return this._COScost;
            }
            set
            {
                this._COScost = value;
                NotifyPropertyChanged("COScost");
            }
        }

        public bool isDirty
        {
            get
            {
                return this._isDirty;
            }
        }


        #endregion

        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _LogKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }
            internal Criteria(int? LogKey)
            {
                _LogKey = LogKey;
            }
            internal Criteria(int? LogKey, int? Option)
            {
                _LogKey = LogKey;
                _option = Option;
            }
        }
        #endregion //Criteria

    }
}