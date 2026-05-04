using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

namespace BOLib
{       /// <summary>
        /// Summary description for SYSRep.
        /// </summary>
    [Serializable]
    public class MstConRemark : INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++

        private int? _ConRemarkID;
        private int? _ConKey;
        private string _Remark;
        private string _RemarkDesc;
        private string _RemarkType;
        private bool _ActionClose;
        private DateTime? _CreateDate;
        private int? _CreateUserKey;
        private DateTime? _LastModifiedDate;
        private int? _LastModifiedUserKey;
        private bool _isDirty;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public MstConRemark()
        {
            this._ConKey = null;
            this._Remark = string.Empty;
            this._RemarkDesc = string.Empty;
            this._RemarkType = string.Empty;
            this._ActionClose = false;
            this._CreateDate = null;
            this._CreateUserKey = null;
            this._LastModifiedDate = null;
            this._LastModifiedUserKey = null;
            this._isDirty = false;
        }


        public MstConRemark Clone()
        {

            MstConRemark objCopy = (MstConRemark)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
        }

        public static MstConRemark Get(int? ConKey)
        {
            MstConRemark child = new MstConRemark();
            child.Fetch(new Criteria(ConKey, 1));
            return child;
        }
        public static MstConRemark Get(SqlConnection cn, int? ConKey)
        {
            MstConRemark child = new MstConRemark();
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

        public int? ConRemarkID
        {
            get
            {
                return this._ConRemarkID;
            }
            set
            {
                this._ConRemarkID = value;
                NotifyPropertyChanged("ConRemarkID");
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



        public string Remark
        {
            get
            {
                return this._Remark;
            }
            set
            {
                this._Remark = value;
                NotifyPropertyChanged("Remark");
            }
        }
        public string RemarkDesc
        {
            get
            {
                return this._RemarkDesc;
            }
            set
            {
                this._RemarkDesc = value;
                NotifyPropertyChanged("RemarkDesc");
            }
        }

        public string RemarkType
        {
            get
            {
                return this._RemarkType;
            }
            set
            {
                this._RemarkType = value;
                NotifyPropertyChanged("RemarkType");
            }
        }

        public bool ActionClose
        {
            get
            {
                return this._ActionClose;
            }
            set
            {
                this._ActionClose = value;
                NotifyPropertyChanged("ActionClose");
            }
        }

        public DateTime? CreateDate
        {
            get
            {
                return this._CreateDate;
            }
            set
            {
                this._CreateDate = value;
                NotifyPropertyChanged("CreateDate");
            }
        }

        public int? CreateUserKey
        {
            get
            {
                return this._CreateUserKey;
            }
            set
            {
                this._CreateUserKey = value;
                NotifyPropertyChanged("CreateUserKey");
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
            public int? _ConRemarkID = null;
            public int? _ConKey = null;
            public int? _option = null;
            public string _Remark = string.Empty;
            public string _RemarkDesc = string.Empty;
            public bool _ActionClose = false;


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
            internal Criteria(int? ConKey, int? Option, string Remark)
            {
                _ConKey = ConKey;
                _option = Option;
                _Remark = Remark;
            }
            internal Criteria(int? ConKey, int? Option, string Remark, bool ActionClose)
            {
                _ConKey = ConKey;
                _option = Option;
                _Remark = Remark;
                _ActionClose = ActionClose;
            }
            internal Criteria(int? ConKey, int? Option, string Remark, bool ActionClose, int? ConRemarkID)
            {
                _ConRemarkID = ConRemarkID;
                _ConKey = ConKey;
                _option = Option;
                _Remark = Remark;
                _ActionClose = ActionClose;
                _ConRemarkID = ConRemarkID;
            }
            internal Criteria(int? ConKey, int? Option, string Remark, bool ActionClose, int? ConRemarkID, string RemarkDesc)
            {
                _ConRemarkID = ConRemarkID;
                _ConKey = ConKey;
                _option = Option;
                _Remark = Remark;
                _ActionClose = ActionClose;
                _ConRemarkID = ConRemarkID;
                _RemarkDesc = RemarkDesc;
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
                cm.CommandText = "MSTConRemark_Get";
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
        internal static MstConRemark Get(IDataReader dr)
        {

            MstConRemark child = new MstConRemark();
            child.Fetch(dr);
            return child;
        }

        internal bool Fetch(IDataReader dataReader)
        {
            // Fill data to entity object
            _ConKey = dataReader["ConKey"] == DBNull.Value ? null : (int?)dataReader["ConKey"];
            _Remark = dataReader["Remark"] == DBNull.Value ? string.Empty : dataReader["Remark"].ToString();
            _RemarkDesc = dataReader["RemarkDesc"] == DBNull.Value ? string.Empty : dataReader["RemarkDesc"].ToString();
            _RemarkType = dataReader["RemarkType"] == DBNull.Value ? string.Empty : dataReader["RemarkType"].ToString();
            _ActionClose = dataReader["ActionClose"] == DBNull.Value ? false : (bool)dataReader["ActionClose"];
            if (dataReader["CreateDate"] != DBNull.Value)
            {
                _CreateDate = Convert.ToDateTime(dataReader["CreateDate"]);
            }
            _CreateUserKey = Convert.ToInt32(dataReader["CreateUserKey"]);
            if (dataReader["CreateDate"] != DBNull.Value)
            {
                _LastModifiedDate = Convert.ToDateTime(dataReader["LastModifiedDate"]);
            }
            _LastModifiedUserKey = Convert.ToInt32(dataReader["LastModifiedUserKey"]);

            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Custom Update      

        internal bool CustomAddUpdate(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTConRemark_AddUpdate";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@ConKey", criteria._ConKey);
                if (criteria._option == 0)
                {
                    cm.Parameters.AddWithValue("@ConRemarkID", 0);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ConRemarkID", criteria._ConRemarkID);
                }

                cm.Parameters.AddWithValue("@Remark", this.Remark);
                cm.Parameters.AddWithValue("@RemarkDesc", this.RemarkDesc);
                cm.Parameters.AddWithValue("@RemarkType", this.RemarkType );
                cm.Parameters.AddWithValue("@ActionClose", this.ActionClose);
                cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters.AddWithValue("@NewRemarkID", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewRemarkID"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();
                if (criteria._option == 0)
                {
                    if (!GFunc.IsNEZ(cm.Parameters["@NewRemarkID"].Value))
                        this._ConRemarkID = (int)cm.Parameters["@NewRemarkID"].Value;
                }
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
            }// Already close and dispose sql command.      
            return retValue;
        }
        #endregion //Data Access - Custom Update

        #region send emails
        internal bool SendEmailsRemark(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTConRemark_SendEmails";               
                cm.Parameters.AddWithValue("@ConKey", criteria._ConKey); 
                cm.Parameters.AddWithValue("@Remark", this.Remark);
                cm.Parameters.AddWithValue("@RemarkDesc", this.RemarkDesc);
                cm.Parameters.AddWithValue("@RemarkType", this.RemarkType);                
                cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);          
                
                cm.Parameters.AddWithValue("@RetValue", 0);               
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;                
                // Execute command.
                cm.ExecuteNonQuery();                
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
            }// Already close and dispose sql command.      
            return retValue;
        }
        #endregion send emails

        #region update customer record
        internal bool UpdateCustomerRecord(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTConRemark_UpdateCustomerRecord";
                cm.Parameters.AddWithValue("@ConKey", criteria._ConKey);                
                cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
            }// Already close and dispose sql command.      
            return retValue;
        }
        #endregion update customer record
    }

}
