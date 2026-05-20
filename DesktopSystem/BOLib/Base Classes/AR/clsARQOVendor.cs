using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;using System.ComponentModel;
namespace BOLib
{
	/// <summary>
	/// Summary description for ARQOVendor.
	/// </summary>
	[Serializable]
    public class ARQOVendor : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
	{
        #region +++  Local variables declaration for the class +++
        protected int? _VendorKey;
        protected int? _VendorCurrKey;
        protected int? _TransmitMode;
        protected string _Attention;
        protected string _EmailAddr;
        protected string _FaxNumber;
        protected int? _TransmitStatus;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public ARQOVendor()
            : base()
        {
            this._VendorKey = 0;
            this._VendorCurrKey = 0;
            this._TransmitMode = 0;
            this._Attention = string.Empty;
            this._EmailAddr = string.Empty;
            this._FaxNumber = string.Empty;
            this._TransmitStatus = 0;

        }


        public ARQOVendor Clone()
        {
            ARQOVendor objCopy = (ARQOVendor)this.MemberwiseClone();
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


        public int? VendorKey
        {

            get
            {
                return this._VendorKey;
            }
            set
            {
                this._VendorKey = value;
                NotifyPropertyChanged("VendorKey");
            }
        }
        public int? VendorCurrKey
        {

            get
            {
                return this._VendorCurrKey;
            }
            set
            {
                this._VendorCurrKey = value;
                NotifyPropertyChanged("VendorCurrKey");
            }
        }
        public int? TransmitMode
        {

            get
            {
                return this._TransmitMode;
            }
            set
            {
                this._TransmitMode = value;
                NotifyPropertyChanged("TransmitMode");
            }
        }
        public string Attention
        {

            get
            {
                return this._Attention;
            }
            set
            {
                this._Attention = value;
                NotifyPropertyChanged("Attention");
            }
        }
        public string EmailAddr
        {

            get
            {
                return this._EmailAddr;
            }
            set
            {
                this._EmailAddr = value;
                NotifyPropertyChanged("EmailAddr");
            }
        }
        public string FaxNumber
        {

            get
            {
                return this._FaxNumber;
            }
            set
            {
                this._FaxNumber = value;
                NotifyPropertyChanged("FaxNumber");
            }
        }
        public int? TransmitStatus
        {

            get
            {
                return this._TransmitStatus;
            }
            set
            {
                this._TransmitStatus = value;
                NotifyPropertyChanged("TransmitStatus");
            }
        }


        #endregion

		#region Criteria
		[Serializable()]
		internal class Criteria
		{
			public int _DocKey =0;
			public int _option =0;
			
			internal Criteria()
			{
			}
			internal Criteria(int DocKey)
			{
				_DocKey = DocKey;
			}
			internal Criteria(int DocKey, int Option)
			{
				_DocKey = DocKey;
				_option = Option;
			}
		}
		#endregion //Criteria


		#region Data Access - Fetch

		internal bool Fetch(Criteria criteria, out string msgID)
		{
			bool retValue = false;
			msgID = "RecordGetFail";
			
			// Create new sql connection for this method. 
			using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
			{
				 // Open sql connection. 
				cn.Open();
				retValue = this.Fetch(cn, criteria, out msgID);
			}
				
			
			return retValue;
		}
		internal bool Fetch(SqlConnection cn,Criteria criteria, out string msgID)
		{
			bool retValue = false;
			msgID = "RecordGetFail";
			using(SqlCommand cm = cn.CreateCommand())
			{
				cm.CommandType = CommandType.StoredProcedure;
				cm.CommandText = "ARQOVendor_Get";

				cm.Parameters.AddWithValue("@Option", criteria._option);
				cm.Parameters.AddWithValue("@MsgID", msgID);
				cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
				
				cm.Parameters.AddWithValue("@RetValue", 0);
				cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
				// Using data reader as record set.
				using (IDataReader dr = cm.ExecuteReader())
				{
					//If data reader can read, continue...
					while (dr.Read())
					{
						retValue = this.Fetch(dr, out msgID);
					}
				}// Already close and dispose data reader.
				if (cm.Parameters["@MsgID"].Value == null)
					msgID = string.Empty;
				else
					msgID = cm.Parameters["@MsgID"].Value.ToString();
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;
			}// Already close and dispose sql connection.
			
			return retValue;
		}
		internal static ARQOVendor Get(IDataReader dr, out string msgID)
		{
			msgID = "RecordGetFail";
			ARQOVendor child = new ARQOVendor();
			child.Fetch(dr, out msgID);
			return child;
		}

		internal bool Fetch(IDataReader dataReader, out string msgID)
		{
			msgID = "RecordGetFail";
			// Fill data to entity object
            _DocKey =  (int)dataReader["DocKey"];
            _VendorKey = dataReader["VendorKey"] == DBNull.Value ? null : (int?)dataReader["VendorKey"];
            _VendorCurrKey = dataReader["VendorCurrKey"] == DBNull.Value ? null : (int?)dataReader["VendorCurrKey"];
            _TransmitMode = dataReader["TransmitMode"] == DBNull.Value ? null : (int?)dataReader["TransmitMode"];
            _Attention = dataReader["Attention"] == DBNull.Value ? string.Empty : dataReader["Attention"].ToString();
            _EmailAddr = dataReader["EmailAddr"] == DBNull.Value ? string.Empty : dataReader["EmailAddr"].ToString();
            _FaxNumber = dataReader["FaxNumber"] == DBNull.Value ? string.Empty : dataReader["FaxNumber"].ToString();
            _TransmitStatus = dataReader["TransmitStatus"] == DBNull.Value ? null : (int?)dataReader["TransmitStatus"];
            return true;
		}
			#endregion //Data Access - Fetch

		#region Data Access - Insert

		internal bool Insert(out string msgID)
		{
			bool retValue = false;
			msgID = "RecordAddFail";
			using (TransactionScope scope = new TransactionScope())
			{
				// Create new sql connection for this method. 
				using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
				{
					 // Open sql connection. 
					cn.Open();
					retValue = this.Insert(cn, out msgID);
				}
				// No errors - commit transaction
				  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
			}// Already close and dispose sql connection.
			
			return retValue;
		}
		internal bool Insert(SqlConnection cn, out string msgID)
		{			
			msgID = "RecordAddFail";
			using(SqlCommand cm = cn.CreateCommand())
			{
				cm.CommandType = CommandType.StoredProcedure;
				cm.CommandText = "ARQOVendor_AddUpdate";

				cm.Parameters.AddWithValue("@Option", 0);
				cm.Parameters.AddWithValue("@MsgID", msgID);
				cm.Parameters.AddWithValue("@NewDocKey", 0);
				if (_DocKey == null)
					cm.Parameters.AddWithValue("@DocKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocKey", _DocKey);
				if (_VendorKey == null)
					cm.Parameters.AddWithValue("@VendorKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorKey", _VendorKey);
				if (_VendorCurrKey == null)
					cm.Parameters.AddWithValue("@VendorCurrKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorCurrKey", _VendorCurrKey);
				if (_TransmitMode == null)
					cm.Parameters.AddWithValue("@TransmitMode", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@TransmitMode", _TransmitMode);
				if (_Attention == null)
					cm.Parameters.AddWithValue("@Attention", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Attention", _Attention);
				if (_EmailAddr == null)
					cm.Parameters.AddWithValue("@EmailAddr", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@EmailAddr", _EmailAddr);
				if (_FaxNumber == null)
					cm.Parameters.AddWithValue("@FaxNumber", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@FaxNumber", _FaxNumber);
				if (_TransmitStatus == null)
					cm.Parameters.AddWithValue("@TransmitStatus", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@TransmitStatus", _TransmitStatus);
				
				cm.Parameters.AddWithValue("@RetValue", 0);
				cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
				cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;
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
			using(SqlCommand cm = cn.CreateCommand())
			{
				cm.CommandType = CommandType.StoredProcedure;
				cm.CommandText = "ARQOVendor_AddUpdate";

				cm.Parameters.AddWithValue("@Option", 1);
				cm.Parameters.AddWithValue("@MsgID", msgID);
				cm.Parameters.AddWithValue("@NewDocKey", 0);
				if (_DocKey == null)
					cm.Parameters.AddWithValue("@DocKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocKey", _DocKey);
				if (_VendorKey == null)
					cm.Parameters.AddWithValue("@VendorKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorKey", _VendorKey);
				if (_VendorCurrKey == null)
					cm.Parameters.AddWithValue("@VendorCurrKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorCurrKey", _VendorCurrKey);
				if (_TransmitMode == null)
					cm.Parameters.AddWithValue("@TransmitMode", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@TransmitMode", _TransmitMode);
				if (_Attention == null)
					cm.Parameters.AddWithValue("@Attention", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Attention", _Attention);
				if (_EmailAddr == null)
					cm.Parameters.AddWithValue("@EmailAddr", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@EmailAddr", _EmailAddr);
				if (_FaxNumber == null)
					cm.Parameters.AddWithValue("@FaxNumber", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@FaxNumber", _FaxNumber);
				if (_TransmitStatus == null)
					cm.Parameters.AddWithValue("@TransmitStatus", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@TransmitStatus", _TransmitStatus);
				
				cm.Parameters.AddWithValue("@RetValue", 0);
				cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
				cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;
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
					retValue = this.Delete(cn,criteria, out msgID);
				}
				// No errors - commit transaction
				  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
			}// Already close and dispose sql connection.
			
			return retValue;
		}

		internal bool Delete(SqlConnection cn, Criteria criteria, out string msgID)
		{
			msgID = "RecordDeleteFail";
			using(SqlCommand cm = cn.CreateCommand())
			{
				cm.CommandType = CommandType.StoredProcedure;
				cm.CommandText = "ARQOVendor_Delete";

				cm.Parameters.AddWithValue("@MsgID", msgID);
				cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
				
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
		#endregion Delete
	}
}