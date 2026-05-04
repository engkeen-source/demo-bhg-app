


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using TAUtil;
namespace BOLib
{
    /// <summary>
    /// Summary description for TASAlerts.
    /// </summary>
    [Serializable()]
    public class TASAlerts : Csla.BusinessListBase<TASAlerts,TASAlert>
    {
        #region +++  Local variables declaration for the class +++

        protected int _AlertKey;
        protected string _AlertID;
        protected string _AlertDes;
        protected int _AlertApplyGrp;
        protected int _AlertApplyTo;
        protected string _AlertIDFrom;
        protected string _AlertIDTo;
        protected string _AlertCondition;
        protected string _AlertValueAmt;
        protected DateTime? _AlertValueDate;
        protected DateTime? _AlertLastActivateDate;
        protected DateTime? _CreateDate;
        protected int? _CreateUserKey;
        protected DateTime? _LastModifiedDate;
        protected int? _LastModifiedUserKey;
        protected string _Custom1;
        protected string _Custom2;
        protected string _Custom3;
        protected string _Custom4;
        protected string _Custom5;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public TASAlerts()
            : base()
        {
            this._AlertID = string.Empty;
            this._AlertDes = string.Empty;
            this._AlertApplyGrp = 0;
            this._AlertApplyTo = 0;
            this._AlertIDFrom = null;
            this._AlertIDTo = null;
            this._AlertCondition = null;
            this._AlertValueAmt = null;
            this._AlertValueDate = DateTime.Today.Date;
            this._AlertLastActivateDate = DateTime.Today.Date;

        }


        public TASAlerts Clone()
        {
            TASAlerts objCopy = (TASAlerts)this.MemberwiseClone();
            return objCopy;
        }
        public static TASAlerts Get(int? AlertKey)
        {
            TASAlerts child = new TASAlerts();
            child.Fetch(new Criteria(AlertKey, 1));
            return child;
        }

        public static TASAlerts New()
        {
            TASAlerts child = new TASAlerts();
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
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public int AlertKey
        {
            get { return _AlertKey; }
            set { value = _AlertKey; }
        }
        public string AlertID
        {

            get
            {
                return this._AlertID;
            }
            set
            {
                this._AlertID = value;
                NotifyPropertyChanged("AlertID");
            }
        }
        public string AlertDes
        {

            get
            {
                return this._AlertDes;
            }
            set
            {
                this._AlertDes = value;
                NotifyPropertyChanged("AlertDes");
            }
        }
        public int AlertApplyGrp
        {

            get
            {
                return this._AlertApplyGrp;
            }
            set
            {
                this._AlertApplyGrp = value;
                NotifyPropertyChanged("AlertApplyGrp");
            }
        }
        public int AlertApplyTo
        {

            get
            {
                return this._AlertApplyTo;
            }
            set
            {
                this._AlertApplyTo = value;
                NotifyPropertyChanged("AlertApplyTo");
            }
        }
        public string AlertIDFrom
        {

            get
            {
                return this._AlertIDFrom;
            }
            set
            {
                this._AlertIDFrom = value;
                NotifyPropertyChanged("AlertIDFrom");
            }
        }
        public string AlertIDTo
        {

            get
            {
                return this._AlertIDTo;
            }
            set
            {
                this._AlertIDTo = value;
                NotifyPropertyChanged("AlertIDTo");
            }
        }
        public string AlertCondition
        {

            get
            {
                return this._AlertCondition;
            }
            set
            {
                this._AlertCondition = value;
                NotifyPropertyChanged("AlertCondition");
            }
        }
        public string AlertValueAmt
        {

            get
            {
                return this._AlertValueAmt;
            }
            set
            {
                this._AlertValueAmt = value;
                NotifyPropertyChanged("AlertValueAmt");
            }
        }
        public DateTime? AlertValueDate
        {

            get
            {
                return this._AlertValueDate;
            }
            set
            {
                this._AlertValueDate = value;
                NotifyPropertyChanged("AlertValueDate");
            }
        }
        public DateTime? AlertLastActivateDate
        {

            get
            {
                return this._AlertLastActivateDate;
            }
            set
            {
                this._AlertLastActivateDate = value;
                NotifyPropertyChanged("AlertLastActivateDate");
            }
        }


        #endregion

        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _DocCodeKey = null;
            public int? _AlertKey = null;
            public int? _option = null;
            public string _DocID = string.Empty;

            internal Criteria()
            {
            }
            internal Criteria(int? AlertKey)
            {
                _AlertKey = AlertKey;
            }
            internal Criteria(int? AlertKey, int? Option)
            {
                _AlertKey = AlertKey;
                _option = Option;
            }
            internal Criteria(int DocCodeKey, int? AlertKey, int? Option)
            {
                _DocCodeKey = DocCodeKey;
                _AlertKey = AlertKey;
                _option = Option;
            }
            internal Criteria(int? DocCodeKey, int? AlertKey, string DocID, int? Option)
            {
                _DocCodeKey = DocCodeKey;
                _AlertKey = AlertKey;
                _DocID = DocID;
                _option = Option;

            }

        }
        #endregion //Criteria


        #region Data Access - Fetch

        internal bool Fetch(Criteria criteria)
        {
            bool retValue = false;
          
            // Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
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
                cm.CommandText = "TASAlerts_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@AlertKey", criteria._AlertKey);
                cm.Parameters.AddWithValue("@DocID", criteria._DocID);
                cm.Parameters.AddWithValue("@DocCodeKey", criteria._DocCodeKey);
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
                    if (!retValue)
                        return false;
                }// Already close and dispose data reader.


                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;
            }// Already close and dispose sql connection.

            return retValue;
        }
        internal static TASAlerts Get(IDataReader dr)
        {
            TASAlerts child = new TASAlerts();
            child.Fetch(dr);
            return child;
        }
        internal static TASAlerts Get(SqlConnection cn, Criteria criteria)
        {
            TASAlerts child = new TASAlerts();
            child.Fetch(cn, criteria);
            return child;
        }

        internal bool Fetch(IDataReader dataReader)
        {
            _AlertKey = dataReader["AlertKey"] == DBNull.Value ? (int)0 : (int)dataReader["AlertKey"];
            _AlertID = dataReader["AlertID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["AlertID"];
            _AlertDes = dataReader["AlertDes"] == DBNull.Value ? (string)string.Empty : (string)dataReader["AlertDes"];
            _AlertApplyGrp = dataReader["AlertApplyGrp"] == DBNull.Value ? (int)0 : (int)dataReader["AlertApplyGrp"];
            _AlertApplyTo = dataReader["AlertApplyTo"] == DBNull.Value ? (int)0 : (int)dataReader["AlertApplyTo"];
            _AlertIDFrom = dataReader["AlertIDFrom"] == DBNull.Value ? (string)null : (string)dataReader["AlertIDFrom"];
            _AlertIDTo = dataReader["AlertIDTo"] == DBNull.Value ? (string)null : (string)dataReader["AlertIDTo"];
            _AlertCondition = dataReader["AlertCondition"] == DBNull.Value ? (string)null : (string)dataReader["AlertCondition"];
            _AlertValueAmt = dataReader["AlertValueAmt"] == DBNull.Value ? (string)null : (string)dataReader["AlertValueAmt"];
            _AlertValueDate = dataReader["AlertValueDate"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["AlertValueDate"];
            _AlertLastActivateDate = dataReader["AlertLastActivateDate"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["AlertLastActivateDate"];
            _CreateDate = dataReader["CreateDate"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["CreateDate"];
            _CreateUserKey = dataReader["CreateUserKey"] == DBNull.Value ? (int?)null : (int?)dataReader["CreateUserKey"];
            _LastModifiedDate = dataReader["LastModifiedDate"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["LastModifiedDate"];
            _LastModifiedUserKey = dataReader["LastModifiedUserKey"] == DBNull.Value ? (int?)null : (int?)dataReader["LastModifiedUserKey"];
            _Custom1 = dataReader["Custom1"] == DBNull.Value ? (string)null : (string)dataReader["Custom1"];
            _Custom2 = dataReader["Custom2"] == DBNull.Value ? (string)null : (string)dataReader["Custom2"];
            _Custom3 = dataReader["Custom3"] == DBNull.Value ? (string)null : (string)dataReader["Custom3"];
            _Custom4 = dataReader["Custom4"] == DBNull.Value ? (string)null : (string)dataReader["Custom4"];
            _Custom5 = dataReader["Custom5"] == DBNull.Value ? (string)null : (string)dataReader["Custom5"];

            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert()
        {
            bool retValue = false;
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }
        internal bool Insert(SqlConnection cn)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "TASAlerts_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                if (_AlertKey == null)
                {
                    cm.Parameters.AddWithValue("@AlertKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertKey", _AlertKey);
                }
                if (_AlertID == null)
                {
                    cm.Parameters.AddWithValue("@AlertID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertID", _AlertID);
                }
                if (_AlertDes == null)
                {
                    cm.Parameters.AddWithValue("@AlertDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertDes", _AlertDes);
                }
                if (_AlertApplyGrp == null)
                {
                    cm.Parameters.AddWithValue("@AlertApplyGrp", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertApplyGrp", _AlertApplyGrp);
                }
                if (_AlertApplyTo == null)
                {
                    cm.Parameters.AddWithValue("@AlertApplyTo", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertApplyTo", _AlertApplyTo);
                }
                if (_AlertIDFrom == null)
                {
                    cm.Parameters.AddWithValue("@AlertIDFrom", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertIDFrom", _AlertIDFrom);
                }
                if (_AlertIDTo == null)
                {
                    cm.Parameters.AddWithValue("@AlertIDTo", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertIDTo", _AlertIDTo);
                }
                if (_AlertCondition == null)
                {
                    cm.Parameters.AddWithValue("@AlertCondition", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertCondition", _AlertCondition);
                }
                if (_AlertValueAmt == null)
                {
                    cm.Parameters.AddWithValue("@AlertValueAmt", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertValueAmt", _AlertValueAmt);
                }
                if (_AlertValueDate == null)
                {
                    cm.Parameters.AddWithValue("@AlertValueDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertValueDate", _AlertValueDate);
                }
                if (_AlertLastActivateDate == null)
                {
                    cm.Parameters.AddWithValue("@AlertLastActivateDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertLastActivateDate", _AlertLastActivateDate);
                }
                if (_CreateDate == null)
                {
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@CreateDate", _CreateDate);
                }
                if (_CreateUserKey == null)
                {
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@CreateUserKey", _CreateUserKey);
                }
                if (_LastModifiedDate == null)
                {
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@LastModifiedDate", _LastModifiedDate);
                }
                if (_LastModifiedUserKey == null)
                {
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", _LastModifiedUserKey);
                }
                if (_Custom1 == null)
                {
                    cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom1", _Custom1);
                }
                if (_Custom2 == null)
                {
                    cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom2", _Custom2);
                }
                if (_Custom3 == null)
                {
                    cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom3", _Custom3);
                }
                if (_Custom4 == null)
                {
                    cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom4", _Custom4);
                }
                if (_Custom5 == null)
                {
                    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom5", _Custom5);
                }

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                // Execute command.
                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql command.                
        }
        #endregion Insert

        #region Data Access - Update

        internal bool Update(out string msgID)
        {
            bool retValue = false;
            msgID = "RecordUpdateFail";
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Update(cn, out msgID);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }
        internal bool Update(SqlConnection cn, out string msgID)
        {
            msgID = "RecordUpdateFail";
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "TASAlerts_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@MsgID", msgID);

                if (_AlertKey == null)
                {
                    cm.Parameters.AddWithValue("@AlertKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertKey", _AlertKey);
                }
                if (_AlertID == null)
                {
                    cm.Parameters.AddWithValue("@AlertID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertID", _AlertID);
                }
                if (_AlertDes == null)
                {
                    cm.Parameters.AddWithValue("@AlertDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertDes", _AlertDes);
                }
                if (_AlertApplyGrp == null)
                {
                    cm.Parameters.AddWithValue("@AlertApplyGrp", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertApplyGrp", _AlertApplyGrp);
                }
                if (_AlertApplyTo == null)
                {
                    cm.Parameters.AddWithValue("@AlertApplyTo", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertApplyTo", _AlertApplyTo);
                }
                if (_AlertIDFrom == null)
                {
                    cm.Parameters.AddWithValue("@AlertIDFrom", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertIDFrom", _AlertIDFrom);
                }
                if (_AlertIDTo == null)
                {
                    cm.Parameters.AddWithValue("@AlertIDTo", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertIDTo", _AlertIDTo);
                }
                if (_AlertCondition == null)
                {
                    cm.Parameters.AddWithValue("@AlertCondition", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertCondition", _AlertCondition);
                }
                if (_AlertValueAmt == null)
                {
                    cm.Parameters.AddWithValue("@AlertValueAmt", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertValueAmt", _AlertValueAmt);
                }
                if (_AlertValueDate == null)
                {
                    cm.Parameters.AddWithValue("@AlertValueDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertValueDate", _AlertValueDate);
                }
                if (_AlertLastActivateDate == null)
                {
                    cm.Parameters.AddWithValue("@AlertLastActivateDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@AlertLastActivateDate", _AlertLastActivateDate);
                }
                if (_CreateDate == null)
                {
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@CreateDate", _CreateDate);
                }
                if (_CreateUserKey == null)
                {
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@CreateUserKey", _CreateUserKey);
                }
                if (_LastModifiedDate == null)
                {
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@LastModifiedDate", _LastModifiedDate);
                }
                if (_LastModifiedUserKey == null)
                {
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", _LastModifiedUserKey);
                }
                if (_Custom1 == null)
                {
                    cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom1", _Custom1);
                }
                if (_Custom2 == null)
                {
                    cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom2", _Custom2);
                }
                if (_Custom3 == null)
                {
                    cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom3", _Custom3);
                }
                if (_Custom4 == null)
                {
                    cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom4", _Custom4);
                }
                if (_Custom5 == null)
                {
                    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Custom5", _Custom5);
                }


                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();
                if (cm.Parameters["@MsgID"].Value == null)
                    msgID = string.Empty;
                else
                    msgID = cm.Parameters["@MsgID"].Value.ToString();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;

            }// Already close and dispose sql command.

        }
        #endregion Update

        #region Data Access - Delete

        internal bool Delete(Criteria criteria, out string msgID)
        {
            bool retValue = false;
            msgID = "RecordDeleteFail";
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Delete(cn, criteria, out msgID);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }
        internal bool Delete(SqlConnection cn, Criteria criteria, out string msgID)
        {
            bool retValue = false;
            msgID = "RecordDeleteFail";
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "TASAlerts_Delete";

                cm.Parameters.AddWithValue("@MsgID", msgID);
                cm.Parameters.AddWithValue("@AlertKey", criteria._AlertKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();
                if (cm.Parameters["@MsgID"].Value == null)
                    msgID = string.Empty;
                else
                    msgID = cm.Parameters["@MsgID"].Value.ToString();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
            }// Already close and dispose sql command.

            return retValue;
        }
        #endregion Delete

        #region Data Access - Validation

        internal bool Validation(Criteria criteria, bool isNew)
        {
            bool retValue = false;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    //Create new sql connection for this method. 
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open sql connection. 
                        cn.Open();
                        retValue = Validation(cn, criteria, isNew);
                    }
                    // No errors - commit transaction
                      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                }// Already close and dispose sql connection.
            }
            catch (TAException taex)
            {
                throw taex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retValue;
        }
        internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
        {

            string msgID = "DocID" + MsgID.Validation.DuplicateRecord;
            try
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "TASAlerts_Validation";

                    cm.Parameters.AddWithValue("@isNew", isNew);
                    cm.Parameters.AddWithValue("@DocCodeKey", criteria._DocCodeKey);
                    cm.Parameters.AddWithValue("@AlertKey", criteria._AlertKey);
                    cm.Parameters.AddWithValue("@DocID", criteria._DocID);
                    cm.Parameters.AddWithValue("@RetValue", 0);

                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        return true;
                    else
                        return false;
                }// Already close and dispose sql command.
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Validation
    }
}





