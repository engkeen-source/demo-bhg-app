using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;using System.ComponentModel;
namespace BOLib
{
	/// <summary>
	/// Summary description for ARQODetItmVendor.
	/// </summary>
	[Serializable]
    public class ARQODetItmVendor : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
	{
        #region +++  Local variables declaration for the class +++
        protected int? _VendorKey;
        protected decimal? _VendorEqItmQty;
        protected decimal? _VendorEqItmPrice;
        protected decimal? _VendorItmQty;
        protected decimal? _VendorItmPrice;
        protected string _VendorLeadTime;
        protected string _VendorWarranty;
        protected string _VendorRemarks;
        protected bool? _Selected;
        protected int? _VendorStatus;
        protected string _VendorID;
        protected string _VendorNm;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public ARQODetItmVendor()
            : base()
        {
            this._VendorKey = 0;
            this._VendorEqItmQty = 0;
            this._VendorEqItmPrice = 0;
            this._VendorItmQty = 0;
            this._VendorItmPrice = 0;
            this._VendorLeadTime = string.Empty;
            this._VendorWarranty = string.Empty;
            this._VendorRemarks = string.Empty;
            this._Selected = false;
            this._VendorStatus = 0;
            this._VendorID = string.Empty;
            this._VendorNm = string.Empty;

        }


        public ARQODetItmVendor Clone()
        {
            ARQODetItmVendor objCopy = (ARQODetItmVendor)this.MemberwiseClone();
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
        public decimal? VendorEqItmQty
        {

            get
            {
                return this._VendorEqItmQty;
            }
            set
            {
                this._VendorEqItmQty = value;
                NotifyPropertyChanged("VendorEqItmQty");
            }
        }
        public decimal? VendorEqItmPrice
        {

            get
            {
                return this._VendorEqItmPrice;
            }
            set
            {
                this._VendorEqItmPrice = value;
                NotifyPropertyChanged("VendorEqItmPrice");
            }
        }
        public decimal? VendorItmQty
        {

            get
            {
                return this._VendorItmQty;
            }
            set
            {
                this._VendorItmQty = value;
                NotifyPropertyChanged("VendorItmQty");
            }
        }
        public decimal? VendorItmPrice
        {

            get
            {
                return this._VendorItmPrice;
            }
            set
            {
                this._VendorItmPrice = value;
                NotifyPropertyChanged("VendorItmPrice");
            }
        }
        public string VendorLeadTime
        {

            get
            {
                return this._VendorLeadTime;
            }
            set
            {
                this._VendorLeadTime = value;
                NotifyPropertyChanged("VendorLeadTime");
            }
        }
        public string VendorWarranty
        {

            get
            {
                return this._VendorWarranty;
            }
            set
            {
                this._VendorWarranty = value;
                NotifyPropertyChanged("VendorWarranty");
            }
        }
        public string VendorRemarks
        {

            get
            {
                return this._VendorRemarks;
            }
            set
            {
                this._VendorRemarks = value;
                NotifyPropertyChanged("VendorRemarks");
            }
        }
        public bool? Selected
        {

            get
            {
                return this._Selected;
            }
            set
            {
                this._Selected = value;
                NotifyPropertyChanged("Selected");
            }
        }
        public int? VendorStatus
        {

            get
            {
                return this._VendorStatus;
            }
            set
            {
                this._VendorStatus = value;
                NotifyPropertyChanged("VendorStatus");
            }
        }
        public string VendorID
        {

            get
            {
                return this._VendorID;
            }
            set
            {
                this._VendorID = value;
                NotifyPropertyChanged("VendorID");
            }
        }
        public string VendorNm
        {

            get
            {
                return this._VendorNm;
            }
            set
            {
                this._VendorNm = value;
                NotifyPropertyChanged("VendorNm");
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
				cm.CommandText = "ARQODetItmVendor_Get";

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
		internal static ARQODetItmVendor Get(IDataReader dr, out string msgID)
		{
			msgID = "RecordGetFail";
			ARQODetItmVendor child = new ARQODetItmVendor();
			child.Fetch(dr, out msgID);
			return child;
		}

		internal bool Fetch(IDataReader dataReader, out string msgID)
		{
            msgID = "RecordGetFail";
			// Fill data to entity object
            _DocKey = (int)dataReader["DocKey"];
            _DocItmKey = (int)dataReader["DocItmKey"];
            _VendorKey = dataReader["VendorKey"] == DBNull.Value ? null : (int?)dataReader["VendorKey"];
            _VendorEqItmQty = dataReader["VendorEqItmQty"] == DBNull.Value ? null : (decimal?)dataReader["VendorEqItmQty"];
            _VendorEqItmPrice = dataReader["VendorEqItmPrice"] == DBNull.Value ? null : (decimal?)dataReader["VendorEqItmPrice"];
            _VendorItmQty = dataReader["VendorItmQty"] == DBNull.Value ? null : (decimal?)dataReader["VendorItmQty"];
            _VendorItmPrice = dataReader["VendorItmPrice"] == DBNull.Value ? null : (decimal?)dataReader["VendorItmPrice"];
            _VendorLeadTime = dataReader["VendorLeadTime"] == DBNull.Value ? string.Empty : dataReader["VendorLeadTime"].ToString();
            _VendorWarranty = dataReader["VendorWarranty"] == DBNull.Value ? string.Empty : dataReader["VendorWarranty"].ToString();
            _VendorRemarks = dataReader["VendorRemarks"] == DBNull.Value ? string.Empty : dataReader["VendorRemarks"].ToString();
            _Selected = dataReader["Selected"] == DBNull.Value ? false : (bool)dataReader["Selected"];
            _VendorStatus = dataReader["VendorStatus"] == DBNull.Value ? null : (int?)dataReader["VendorStatus"];
            _CreateDate =  (DateTime)dataReader["CreateDate"];
            _CreateUserKey = (int)dataReader["CreateUserKey"];
            _LastModifiedDate = (DateTime)dataReader["LastModifiedDate"];
            _LastModifiedUserKey = (int)dataReader["LastModifiedUserKey"];

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
				cm.CommandText = "ARQODetItmVendor_AddUpdate";

				cm.Parameters.AddWithValue("@Option", 0);
				cm.Parameters.AddWithValue("@MsgID", msgID);
				cm.Parameters.AddWithValue("@NewDocKey", 0);
				if (_DocKey == null)
					cm.Parameters.AddWithValue("@DocKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocKey", _DocKey);
				if (_DocItmKey == null)
					cm.Parameters.AddWithValue("@DocItmKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocItmKey", _DocItmKey);
				if (_VendorKey == null)
					cm.Parameters.AddWithValue("@VendorKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorKey", _VendorKey);
				if (_VendorEqItmQty == null)
					cm.Parameters.AddWithValue("@VendorEqItmQty", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorEqItmQty", _VendorEqItmQty);
				if (_VendorEqItmPrice == null)
					cm.Parameters.AddWithValue("@VendorEqItmPrice", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorEqItmPrice", _VendorEqItmPrice);
				if (_VendorItmQty == null)
					cm.Parameters.AddWithValue("@VendorItmQty", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorItmQty", _VendorItmQty);
				if (_VendorItmPrice == null)
					cm.Parameters.AddWithValue("@VendorItmPrice", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorItmPrice", _VendorItmPrice);
				if (_VendorLeadTime == null)
					cm.Parameters.AddWithValue("@VendorLeadTime", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorLeadTime", _VendorLeadTime);
				if (_VendorWarranty == null)
					cm.Parameters.AddWithValue("@VendorWarranty", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorWarranty", _VendorWarranty);
				if (_VendorRemarks == null)
					cm.Parameters.AddWithValue("@VendorRemarks", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorRemarks", _VendorRemarks);
				if (_Selected == null)
					cm.Parameters.AddWithValue("@Selected", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Selected", _Selected);
				if (_VendorStatus == null)
					cm.Parameters.AddWithValue("@VendorStatus", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorStatus", _VendorStatus);
				if (_CreateDate == null)
					cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@CreateDate", _CreateDate);
				if (AppInfor.currentUserKey == null)
					cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
				if (_LastModifiedDate == null)
					cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LastModifiedDate", _LastModifiedDate);
				cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
				
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
				cm.CommandText = "ARQODetItmVendor_AddUpdate";

				cm.Parameters.AddWithValue("@Option", 1);
				cm.Parameters.AddWithValue("@MsgID", msgID);
				cm.Parameters.AddWithValue("@NewDocKey", 0);
				if (_DocKey == null)
					cm.Parameters.AddWithValue("@DocKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocKey", _DocKey);
				if (_DocItmKey == null)
					cm.Parameters.AddWithValue("@DocItmKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocItmKey", _DocItmKey);
				if (_VendorKey == null)
					cm.Parameters.AddWithValue("@VendorKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorKey", _VendorKey);
				if (_VendorEqItmQty == null)
					cm.Parameters.AddWithValue("@VendorEqItmQty", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorEqItmQty", _VendorEqItmQty);
				if (_VendorEqItmPrice == null)
					cm.Parameters.AddWithValue("@VendorEqItmPrice", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorEqItmPrice", _VendorEqItmPrice);
				if (_VendorItmQty == null)
					cm.Parameters.AddWithValue("@VendorItmQty", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorItmQty", _VendorItmQty);
				if (_VendorItmPrice == null)
					cm.Parameters.AddWithValue("@VendorItmPrice", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorItmPrice", _VendorItmPrice);
				if (_VendorLeadTime == null)
					cm.Parameters.AddWithValue("@VendorLeadTime", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorLeadTime", _VendorLeadTime);
				if (_VendorWarranty == null)
					cm.Parameters.AddWithValue("@VendorWarranty", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorWarranty", _VendorWarranty);
				if (_VendorRemarks == null)
					cm.Parameters.AddWithValue("@VendorRemarks", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorRemarks", _VendorRemarks);
				if (_Selected == null)
					cm.Parameters.AddWithValue("@Selected", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Selected", _Selected);
				if (_VendorStatus == null)
					cm.Parameters.AddWithValue("@VendorStatus", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@VendorStatus", _VendorStatus);
				if (_CreateDate == null)
					cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@CreateDate", _CreateDate);
				if (_CreateUserKey == null)
					cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@CreateUserKey", _CreateUserKey);
				if (_LastModifiedDate == null)
					cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LastModifiedDate", _LastModifiedDate);
				if (AppInfor.currentUserKey == null)
					cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
				
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
				cm.CommandText = "ARQODetItmVendor_Delete";

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