using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Reflection;
namespace BOLib
{
    /// <summary>
    /// Summary description for zDocINHisTmp.
    /// </summary>
    [Serializable]
    public class DocINHisTmp 
    {
        #region +++  Local variables declaration for the class +++

        private int? _UID;
        private int? _UserKey;
        private int? _ItmKey;
        private int? _DocPeriod;
        private decimal? _PPQty;
        private decimal? _PPAmt;
        private decimal? _PSQty;
        private decimal? _PSAmt;
        private decimal? _PNQty;
        private decimal? _PNAmt;
        private decimal? _APQty;
        private decimal? _APAmt;
        private decimal? _ASQty;
        private decimal? _ASAmt;
        private decimal? _APDQty;
        private decimal? _ADOQty;
        private decimal? _APRQty;
        private decimal? _APRAmt;
        private decimal? _ASRQty;
        private decimal? _ASRAmt;
        private decimal? _IAQty;
        private decimal? _IAAmt;
        private decimal? _IASQty;
        private decimal? _IASAmt;
        private decimal? _PMQty;
        private decimal? _PMAmt;
        private decimal? _AMQty;
        private decimal? _AMAmt;
        private bool _Done;
        private DateTime? _Pdate;
        private bool _isDirty;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public DocINHisTmp()
        {
            this._UID = null;
            this._UserKey = null;
            this._ItmKey = null;
            this._DocPeriod = null;
            this._PPQty = null;
            this._PPAmt = null;
            this._PSQty = null;
            this._PSAmt = null;
            this._PNQty = null;
            this._PNAmt = null;
            this._APQty = null;
            this._APAmt = null;
            this._ASQty = null;
            this._ASAmt = null;
            this._APDQty = null;
            this._ADOQty = null;
            this._APRQty = null;
            this._APRAmt = null;
            this._ASRQty = null;
            this._ASRAmt = null;
            this._IAQty = null;
            this._IAAmt = null;
            this._IASQty = null;
            this._IASAmt = null;
            this._PMQty = null;
            this._PMAmt = null;
            this._AMQty = null;
            this._AMAmt = null;
            this._Done = false;
            this._Pdate = null;
            this._isDirty = false;
        }


        public DocINHisTmp Clone()
        {

            DocINHisTmp objCopy = (DocINHisTmp)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
        }


        /// <summary>
        /// Disposing objects
        /// </summary>
        public void Dispose()
        {
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

        public int? UID
        {
            get
            {
                return this._UID;
            }
            set
            {
                this._UID = value;
                NotifyPropertyChanged("UID");
            }
        }

        public int? UserKey
        {
            get
            {
                return this._UserKey;
            }
            set
            {
                this._UserKey = value;
                NotifyPropertyChanged("UserKey");
            }
        }

        public int? ItmKey
        {
            get
            {
                return this._ItmKey;
            }
            set
            {
                this._ItmKey = value;
                NotifyPropertyChanged("ItmKey");
            }
        }

        public int? DocPeriod
        {
            get
            {
                return this._DocPeriod;
            }
            set
            {
                this._DocPeriod = value;
                NotifyPropertyChanged("DocPeriod");
            }
        }

        public decimal? PPQty
        {
            get
            {
                return this._PPQty;
            }
            set
            {
                this._PPQty = value;
                NotifyPropertyChanged("PPQty");
            }
        }

        public decimal? PPAmt
        {
            get
            {
                return this._PPAmt;
            }
            set
            {
                this._PPAmt = value;
                NotifyPropertyChanged("PPAmt");
            }
        }

        public decimal? PSQty
        {
            get
            {
                return this._PSQty;
            }
            set
            {
                this._PSQty = value;
                NotifyPropertyChanged("PSQty");
            }
        }

        public decimal? PSAmt
        {
            get
            {
                return this._PSAmt;
            }
            set
            {
                this._PSAmt = value;
                NotifyPropertyChanged("PSAmt");
            }
        }

        public decimal? PNQty
        {
            get
            {
                return this._PNQty;
            }
            set
            {
                this._PNQty = value;
                NotifyPropertyChanged("PNQty");
            }
        }

        public decimal? PNAmt
        {
            get
            {
                return this._PNAmt;
            }
            set
            {
                this._PNAmt = value;
                NotifyPropertyChanged("PNAmt");
            }
        }

        public decimal? APQty
        {
            get
            {
                return this._APQty;
            }
            set
            {
                this._APQty = value;
                NotifyPropertyChanged("APQty");
            }
        }

        public decimal? APAmt
        {
            get
            {
                return this._APAmt;
            }
            set
            {
                this._APAmt = value;
                NotifyPropertyChanged("APAmt");
            }
        }

        public decimal? ASQty
        {
            get
            {
                return this._ASQty;
            }
            set
            {
                this._ASQty = value;
                NotifyPropertyChanged("ASQty");
            }
        }

        public decimal? ASAmt
        {
            get
            {
                return this._ASAmt;
            }
            set
            {
                this._ASAmt = value;
                NotifyPropertyChanged("ASAmt");
            }
        }

        public decimal? APDQty
        {
            get
            {
                return this._APDQty;
            }
            set
            {
                this._APDQty = value;
                NotifyPropertyChanged("APDQty");
            }
        }

        public decimal? ADOQty
        {
            get
            {
                return this._ADOQty;
            }
            set
            {
                this._ADOQty = value;
                NotifyPropertyChanged("ADOQty");
            }
        }

        public decimal? APRQty
        {
            get
            {
                return this._APRQty;
            }
            set
            {
                this._APRQty = value;
                NotifyPropertyChanged("APRQty");
            }
        }

        public decimal? APRAmt
        {
            get
            {
                return this._APRAmt;
            }
            set
            {
                this._APRAmt = value;
                NotifyPropertyChanged("APRAmt");
            }
        }

        public decimal? ASRQty
        {
            get
            {
                return this._ASRQty;
            }
            set
            {
                this._ASRQty = value;
                NotifyPropertyChanged("ASRQty");
            }
        }

        public decimal? ASRAmt
        {
            get
            {
                return this._ASRAmt;
            }
            set
            {
                this._ASRAmt = value;
                NotifyPropertyChanged("ASRAmt");
            }
        }

        public decimal? IAQty
        {
            get
            {
                return this._IAQty;
            }
            set
            {
                this._IAQty = value;
                NotifyPropertyChanged("IAQty");
            }
        }

        public decimal? IAAmt
        {
            get
            {
                return this._IAAmt;
            }
            set
            {
                this._IAAmt = value;
                NotifyPropertyChanged("IAAmt");
            }
        }

        public decimal? IASQty
        {
            get
            {
                return this._IASQty;
            }
            set
            {
                this._IASQty = value;
                NotifyPropertyChanged("IASQty");
            }
        }

        public decimal? IASAmt
        {
            get
            {
                return this._IASAmt;
            }
            set
            {
                this._IASAmt = value;
                NotifyPropertyChanged("IASAmt");
            }
        }

        public decimal? PMQty
        {
            get
            {
                return this._PMQty;
            }
            set
            {
                this._PMQty = value;
                NotifyPropertyChanged("PMQty");
            }
        }

        public decimal? PMAmt
        {
            get
            {
                return this._PMAmt;
            }
            set
            {
                this._PMAmt = value;
                NotifyPropertyChanged("PMAmt");
            }
        }

        public decimal? AMQty
        {
            get
            {
                return this._AMQty;
            }
            set
            {
                this._AMQty = value;
                NotifyPropertyChanged("AMQty");
            }
        }

        public decimal? AMAmt
        {
            get
            {
                return this._AMAmt;
            }
            set
            {
                this._AMAmt = value;
                NotifyPropertyChanged("AMAmt");
            }
        }

        public bool Done
        {
            get
            {
                return this._Done;
            }
            set
            {
                this._Done = value;
                NotifyPropertyChanged("Done");
            }
        }

        public DateTime? Pdate
        {
            get
            {
                return this._Pdate;
            }
            set
            {
                this._Pdate = value;
                NotifyPropertyChanged("Pdate");
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

        public static DataTable ToTable()
        {
            DataTable dt = new DataTable();
            Type tType = typeof(DocINHisTmp);
            PropertyInfo[] props = tType.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.Name.Equals("item")) continue;
                DataColumn col = new DataColumn(prop.Name, prop.PropertyType);
                dt.Columns.Add(col);
            }
            return dt;
        }
    }
}