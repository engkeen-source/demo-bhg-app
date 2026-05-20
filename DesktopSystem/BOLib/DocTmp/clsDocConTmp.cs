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
    /// Summary description for zDocConTmp.
    /// </summary>
    [Serializable]
    public class DocConTmp 
    {
        #region +++  Local variables declaration for the class +++

        private int? _UID;
        private int? _UserKey;
        private int? _ConKey;
        private decimal? _CR;
        private decimal? _CH;
        private decimal? _V;
        private bool _isDirty;
        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public DocConTmp()
        {
            this._UID = null;
            this._UserKey = null;
            this._ConKey = null;
            this._CR = null;
            this._CH = null;
            this._V = null;
            this._isDirty = false;
        }


        public DocConTmp Clone()
        {

            DocConTmp objCopy = (DocConTmp)this.MemberwiseClone();
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
            //if (PropertyChanged != null)
            //{
            //    PropertyChanged(this, new PropertyChangedEventArgs(info));
            //}
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

        public decimal? CR
        {
            get
            {
                return this._CR;
            }
            set
            {
                this._CR = value;
                NotifyPropertyChanged("CR");
            }
        }

        public decimal? CH
        {
            get
            {
                return this._CH;
            }
            set
            {
                this._CH = value;
                NotifyPropertyChanged("CH");
            }
        }

        public decimal? V
        {
            get
            {
                return this._V;
            }
            set
            {
                this._V = value;
                NotifyPropertyChanged("V");
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
            Type tType = typeof(DocConTmp);
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