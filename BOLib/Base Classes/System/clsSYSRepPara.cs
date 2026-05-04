using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

namespace BOLib
{
    /// <summary>
    /// Summary description for SYSRepPara.
    /// </summary>
    [Serializable]
    public class SYSRepPara : IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++

        private int? _RepKey;
        private string _ParName;
        private string _ParDataType;
        private decimal? _Seq;
        private string _ParDefaultValue;
        private string _Custom1;
        private string _Custom2;
        private string _Custom3;
        private bool _isDirty;
        private string _error = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public SYSRepPara()
        {
            this._RepKey = null;
            this._ParName = string.Empty;
            this._ParDataType = string.Empty;
            this._Seq = null;
            this._Custom1 = string.Empty;
            this._Custom2 = string.Empty;
            this._Custom3 = string.Empty;
            this._isDirty = false;
        }

        public SYSRepPara Clone()
        {

            SYSRepPara objCopy = (SYSRepPara)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
        }

        public static SYSRepPara New()
        {
            SYSRepPara child = new SYSRepPara();
            return child;
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

        private void NotifyPropertyChanged(String info)
        {
            _isDirty = true;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


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

        public int? RepKey
        {
            get
            {
                return this._RepKey;
            }
            set
            {
                this._RepKey = value;
                NotifyPropertyChanged("RepKey");
            }
        }

        public string ParName
        {
            get
            {
                return this._ParName;
            }
            set
            {
                this._ParName = value;
                NotifyPropertyChanged("ParName");
            }
        }

        public string ParDataType
        {
            get
            {
                return this._ParDataType;
            }
            set
            {
                this._ParDataType = value;
                NotifyPropertyChanged("ParDataType");
            }
        }

        public decimal? Seq
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
        public string ParDefaultValue
        {
            get
            {
                return this._ParDefaultValue;
            }
            set
            {
                this._ParDefaultValue = value;
                NotifyPropertyChanged("ParDefaultValue");
            }
        }

        public string Custom1
        {
            get
            {
                return this._Custom1;
            }
            set
            {
                this._Custom1 = value;
                NotifyPropertyChanged("Custom1");
            }
        }

        public string Custom2
        {
            get
            {
                return this._Custom2;
            }
            set
            {
                this._Custom2 = value;
                NotifyPropertyChanged("Custom2");
            }
        }

        public string Custom3
        {
            get
            {
                return this._Custom3;
            }
            set
            {
                this._Custom3 = value;
                NotifyPropertyChanged("Custom3");
            }
        }

        public bool IsDirty
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
            public int? _RepKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }
            internal Criteria(int? RepKey)
            {
                _RepKey = RepKey;
            }
            internal Criteria(int? RepKey, int? Option)
            {
                _RepKey = RepKey;
                _option = Option;
            }
        }
        #endregion //Criteria


        #region Data Access - Fetch

        internal bool Fetch(Criteria criteria)
        {
            bool retValue = false;
 
            // Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(BOLib.Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();
                retValue = this.Fetch(cn, criteria);
            }
         
            return retValue;
        }
        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSRepPara_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@RepKey", criteria._RepKey);
                
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Using data reader as record set.
                using (IDataReader dr = cm.ExecuteReader())
                {
                    //If data reader can read, continue...
                    while (dr.Read())
                    {
                        retValue = this.Fetch(dr);
                    }
                }// Already close and dispose data reader.
                
                if ((int)cm.Parameters["@RetValue"].Value == 1)
                    retValue = true;
            }// Already close and dispose sql connection.
            return retValue;
        }
        internal static SYSRepPara Get(IDataReader dr)
        {
            
            SYSRepPara child = new SYSRepPara();
            child.Fetch(dr);
            return child;
        }

        internal bool Fetch(IDataReader dataReader)
        {
            
            
            // Fill data to entity object            
            _RepKey = dataReader["RepKey"] == DBNull.Value ? null : (int?)dataReader["RepKey"];
            _ParName = dataReader["ParName"] == DBNull.Value ? string.Empty : dataReader["ParName"].ToString();
            _ParDataType = dataReader["ParDataType"] == DBNull.Value ? string.Empty : dataReader["ParDataType"].ToString();
            _ParDefaultValue = dataReader["ParDefaultValue"] == DBNull.Value ? string.Empty : dataReader["ParDefaultValue"].ToString();
            _Seq = dataReader["Seq"] == DBNull.Value ? null : (decimal?)dataReader["Seq"];
            _Custom1 = dataReader["Custom1"] == DBNull.Value ? string.Empty : dataReader["Custom1"].ToString();
            _Custom2 = dataReader["Custom2"] == DBNull.Value ? string.Empty : dataReader["Custom2"].ToString();
            _Custom3 = dataReader["Custom3"] == DBNull.Value ? string.Empty : dataReader["Custom3"].ToString();


            return true;
        }
        #endregion //Data Access - Fetch

    }
}