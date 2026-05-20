using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

namespace BOLib
{
    /// <summary>
	/// Summary description for SYSRep.
	/// </summary>
	[Serializable]
    public class MSTConManage : INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++

        private int? _ConKey;
        private bool _ActiveWithProb;
        private bool _OrangeCus;
        private DateTime? _FollowUpDate;        
        private int? _UserKey;
        private DateTime? _LastModifiedDate;
        private int? _LastModifiedUserKey;
        private bool _isDirty;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public MSTConManage()
        {
            this._ConKey = null;
            this._ActiveWithProb = false;
            this._OrangeCus = false;
            this._FollowUpDate = null;
            this._UserKey = null;
            this._LastModifiedDate = null;
            this._LastModifiedUserKey = null;
            this._isDirty = false;
        }


        public MSTConManage Clone()
        {
            MSTConManage objCopy = (MSTConManage)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
        }

        public static MSTConManage Get(int? ConKey)
        {
            MSTConManage child = new MSTConManage();
            child.Fetch(new Criteria(ConKey, 1));
            return child;
        }
        public static MSTConManage Get(SqlConnection cn, int? ConKey)
        {
            MSTConManage child = new MSTConManage();
            child.Fetch(cn, new Criteria(ConKey, 1));
            return child;
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

        public bool ActiveWithProb
        {
            get
            {
                return this._ActiveWithProb;
            }
            set
            {
                this._ActiveWithProb = value;
                NotifyPropertyChanged("ActiveWithProb");
            }
        }

        public bool OrangeCus
        {
            get
            {
                return this._OrangeCus;
            }
            set
            {
                this._OrangeCus = value;
                NotifyPropertyChanged("OrangeCus");
            }
        }

        public DateTime? FollowUpDate
        {
            get
            {
                return this._FollowUpDate;
            }
            set
            {
                this._FollowUpDate = value;
                NotifyPropertyChanged("FollowUpDate");
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


        public DateTime? LastModifiedDate
        {
            get
            {
                return this._LastModifiedDate;
            }
            set
            {
                this._LastModifiedDate = value;
                NotifyPropertyChanged("LastModifiedDate");
            }
        }

        public int? LastModifiedUserKey
        {
            get
            {
                return this._LastModifiedUserKey;
            }
            set
            {
                this._LastModifiedUserKey = value;
                NotifyPropertyChanged("LastModifiedUserKey");
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
            public int? _ConKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }
            internal Criteria(int? ConKey)
            {
                _option = 1;
                _ConKey = ConKey;
            }
            internal Criteria(int? ConKey, int? Option)
            {
                _ConKey = ConKey;
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
                cm.CommandText = "MSTCon_Manage";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@ConKey", criteria._ConKey);
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
        internal static MSTConManage Get(IDataReader dr)
        {
            MSTConManage child = new MSTConManage();
            child.Fetch(dr);
            return child;
        }

        internal bool Fetch(IDataReader dataReader)
        {   // Fill data to entity object
            _ConKey = dataReader["ConKey"] == DBNull.Value ? null : (int?)dataReader["ConKey"];
            _ActiveWithProb = dataReader["ActiveWithProb"] == DBNull.Value ? false : (bool)dataReader["ActiveWithProb"];
            _OrangeCus = dataReader["OrangeCus"] == DBNull.Value ? false : (bool)dataReader["OrangeCus"];
            if (dataReader["CreateDate"] != DBNull.Value)
            {
                _LastModifiedDate = Convert.ToDateTime(dataReader["LastModifiedDate"]);
            }
            _LastModifiedUserKey = Convert.ToInt32(dataReader["LastModifiedUserKey"]);
            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Custom Update      

        internal bool CustomUpdate(SqlConnection cn)
        {
            // Fill data to entity object               
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSRep_CustomUpdate";
                cm.Parameters.AddWithValue("@RepKey", this._ConKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == 1)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.                   
        }
        #endregion //Data Access - Custom Update
    }
}
