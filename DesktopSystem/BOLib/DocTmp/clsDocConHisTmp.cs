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
    /// Summary description for zDocConHisTmp.
    /// </summary>
    [Serializable]
    public class DocConHisTmp 
    {
        #region +++  Local variables declaration for the class +++

        private int? _UID;
        private int? _UserKey;
        private int? _ConKey;
        private int? _P;
        private int? _DC;
        private int? _Grp;
        private int? _Curr;
        private decimal? _FCr;
        private decimal? _HCr;
        private decimal? _FCh;
        private decimal? _HCh;
        private decimal? _FV;
        private decimal? _HV;
        private bool _wrkDone;
        private bool _isDirty;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public DocConHisTmp()
        {
            this._UID = null;
            this._UserKey = null;
            this._ConKey = null;
            this._P = null;
            this._DC = null;
            this._Grp = null;
            this._Curr = null;
            this._FCr = null;
            this._HCr = null;
            this._FCh = null;
            this._HCh = null;
            this._FV = null;
            this._HV = null;
            this._wrkDone = false;
            this._isDirty = false;
        }


        public DocConHisTmp Clone()
        {

            DocConHisTmp objCopy = (DocConHisTmp)this.MemberwiseClone();
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

        public int? ConKey
        {
            get
            {
                return this._ConKey;
            }
            set
            {
                this._ConKey = value;
                NotifyPropertyChanged("ConKey");
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

        public decimal? FCr
        {
            get
            {
                return this._FCr;
            }
            set
            {
                this._FCr = value;
                NotifyPropertyChanged("FCr");
            }
        }

        public decimal? HCr
        {
            get
            {
                return this._HCr;
            }
            set
            {
                this._HCr = value;
                NotifyPropertyChanged("HCr");
            }
        }

        public decimal? FCh
        {
            get
            {
                return this._FCh;
            }
            set
            {
                this._FCh = value;
                NotifyPropertyChanged("FCh");
            }
        }

        public decimal? HCh
        {
            get
            {
                return this._HCh;
            }
            set
            {
                this._HCh = value;
                NotifyPropertyChanged("HCh");
            }
        }

        public decimal? FV
        {
            get
            {
                return this._FV;
            }
            set
            {
                this._FV = value;
                NotifyPropertyChanged("FV");
            }
        }

        public decimal? HV
        {
            get
            {
                return this._HV;
            }
            set
            {
                this._HV = value;
                NotifyPropertyChanged("HV");
            }
        }

        public bool wrkDone
        {
            get
            {
                return this._wrkDone;
            }
            set
            {
                this._wrkDone = value;
                NotifyPropertyChanged("wrkDone");
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
            Type tType = typeof(DocConHisTmp);
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