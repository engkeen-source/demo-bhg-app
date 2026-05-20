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
    /// Summary description for zDocItmTmp.
    /// </summary>
    [Serializable]
    public class DocItmGrpTmp 
    {
        #region +++  Local variables declaration for the class +++

        private int? _UID;
        private int? _UserKey;
        private int? _ItmKey;
        private string _ItmType;
        private int? _ItmLocKey;
        private decimal? _ItmQty;
        private decimal? _ItmStkBefore;
        private decimal? _ItmStkAfter;
        private bool _ItmUpdated;
        private string _ItmID;
        private string _ItmDes;
        private int? _ItmBUOMKey;
        private string _ItmBUOMID;
        private bool _isDirty;
        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public DocItmGrpTmp()
        {
            this._UID = null;
            this._UserKey = null;
            this._ItmKey = null;
            this._ItmType = string.Empty;
            this._ItmLocKey = null;
            this._ItmQty = null;
            this._ItmStkBefore = null;
            this._ItmStkAfter = null;
            this._ItmUpdated = false;
            this._ItmID = string.Empty;
            this._ItmDes = string.Empty;
            this._ItmBUOMKey = null;
            this._ItmBUOMID = string.Empty;
            this._isDirty = false;
        }


        public DocItmGrpTmp Clone()
        {
            DocItmGrpTmp objCopy = (DocItmGrpTmp)this.MemberwiseClone();
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

        public string ItmType
        {
            get
            {
                return this._ItmType;
            }
            set
            {
                this._ItmType = value;
                NotifyPropertyChanged("ItmType");
            }
        }

        public int? ItmLocKey
        {
            get
            {
                return this._ItmLocKey;
            }
            set
            {
                this._ItmLocKey = value;
                NotifyPropertyChanged("ItmLocKey");
            }
        }

        public decimal? ItmQty
        {
            get
            {
                return this._ItmQty;
            }
            set
            {
                this._ItmQty = value;
                NotifyPropertyChanged("ItmQty");
            }
        }

        public decimal? ItmStkBefore
        {
            get
            {
                return this._ItmStkBefore;
            }
            set
            {
                this._ItmStkBefore = value;
                NotifyPropertyChanged("ItmStkBefore");
            }
        }

        public decimal? ItmStkAfter
        {
            get
            {
                return this._ItmStkAfter;
            }
            set
            {
                this._ItmStkAfter = value;
                NotifyPropertyChanged("ItmStkAfter");
            }
        }

        public bool ItmUpdated
        {
            get
            {
                return this._ItmUpdated;
            }
            set
            {
                this._ItmUpdated = value;
                NotifyPropertyChanged("ItmUpdated");
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

        public string ItmDes
        {
            get
            {
                return this._ItmDes;
            }
            set
            {
                this._ItmDes = value;
                NotifyPropertyChanged("ItmDes");
            }
        }

        public int? ItmBUOMKey
        {
            get
            {
                return this._ItmBUOMKey;
            }
            set
            {
                this._ItmBUOMKey = value;
                NotifyPropertyChanged("ItmBUOMKey");
            }
        }

        public string ItmBUOMID
        {
            get
            {
                return this._ItmBUOMID;
            }
            set
            {
                this._ItmBUOMID = value;
                NotifyPropertyChanged("ItmBUOMID");
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
            Type tType = typeof(DocItmGrpTmp);
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