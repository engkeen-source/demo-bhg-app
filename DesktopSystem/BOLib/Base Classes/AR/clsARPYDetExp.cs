using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

namespace BOLib
{
	/// <summary>
	/// Summary description for ARPYDetExp.
	/// </summary>
	[Serializable]
    public class ARPYDetExp : DocPayExpense, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
       
        private string _error = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public ARPYDetExp()
            : base()
        {
            
            this._isDirty = false;
        }
        public bool IsDirty
        {
            get
            {
                return this._isDirty || base._isDirty;
            }
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

        #endregion
        public ARPYDetExp Clone()
        {
            ARPYDetExp objCopy = (ARPYDetExp)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
        }

		#region Criteria
		[Serializable()]
		internal class Criteria
		{
			public int? _DocKey = null;
			public int? _option = null;
			
			internal Criteria()
			{
			}
			internal Criteria(int? DocKey)
			{
				_DocKey = DocKey;
			}
			internal Criteria(int? DocKey, int? Option)
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
				cm.CommandText = "ARPYDetExp_Get";

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
		internal static ARPYDetExp Get(IDataReader dr, out string msgID)
		{
			msgID = "RecordGetFail";
			ARPYDetExp child = new ARPYDetExp();
			child.Fetch(dr, out msgID);
			return child;
		}

		internal bool Fetch(IDataReader dataReader, out string msgID)
		{			
			msgID = "RecordGetFail";
		    // Fill data to entity object
            DocKey =  (int)dataReader["DocKey"];
            DocItmKey = (int)dataReader["DocItmKey"];
            ExpSN = (decimal)dataReader["ExpSN"];
            ExpDeptKey = (int)dataReader["ExpDeptKey"];
            ExpTranGrpKey = dataReader["ExpTranGrpKey"] == DBNull.Value ? null : (int?)dataReader["ExpTranGrpKey"];
            ExpAccKey =  (int)dataReader["ExpAccKey"];
            ExpDate = dataReader["ExpDate"] == DBNull.Value ? null : (DateTime?)dataReader["ExpDate"];
            ExpRef = dataReader["ExpRef"] == DBNull.Value ? string.Empty : dataReader["ExpRef"].ToString();
            ExpDes = dataReader["ExpDes"] == DBNull.Value ? string.Empty : dataReader["ExpDes"].ToString();
            ExpAmtF =  (decimal)dataReader["ExpAmtF"];
            ExpAmtH =  (decimal)dataReader["ExpAmtH"];
            ExpAmtGST =  (decimal)dataReader["ExpAmtGST"];
            ExpTaxable = dataReader["ExpTaxable"] == DBNull.Value ? false : (bool)dataReader["ExpTaxable"];
            ExpTaxGrpKey = dataReader["ExpTaxGrpKey"] == DBNull.Value ? null : (int?)dataReader["ExpTaxGrpKey"];
            ExpTaxGrpRate = dataReader["ExpTaxGrpRate"] == DBNull.Value ? null : (decimal?)dataReader["ExpTaxGrpRate"];
            ExpTaxGrpAmtF = dataReader["ExpTaxGrpAmtF"] == DBNull.Value ? null : (decimal?)dataReader["ExpTaxGrpAmtF"];
            ExpTaxGrpAmtL = dataReader["ExpTaxGrpAmtL"] == DBNull.Value ? null : (decimal?)dataReader["ExpTaxGrpAmtL"];
            ExpJobKey =  (int)dataReader["ExpJobKey"];
            ExpJobPhaseKey = (int)dataReader["ExpJobPhaseKey"];
            ExpJobTaskKey = (int)dataReader["ExpJobTaskKey"];
            ExpJobCostTypeKey = (int)dataReader["ExpJobCostTypeKey"];
            ExpAttachment = dataReader["ExpAttachment"] == DBNull.Value ? false : (bool)dataReader["ExpAttachment"];
            CreateDate = dataReader["CreateDate"] == DBNull.Value ? null : (DateTime?)dataReader["CreateDate"];
            CreateUserKey = dataReader["CreateUserKey"] == DBNull.Value ? null : (int?)dataReader["CreateUserKey"];
            LastModifiedDate = dataReader["LastModifiedDate"] == DBNull.Value ? null : (DateTime?)dataReader["LastModifiedDate"];
            LastModifiedUserKey = dataReader["LastModifiedUserKey"] == DBNull.Value ? null : (int?)dataReader["LastModifiedUserKey"];
            Custom1 = dataReader["Custom1"] == DBNull.Value ? string.Empty : dataReader["Custom1"].ToString();
            Custom2 = dataReader["Custom2"] == DBNull.Value ? string.Empty : dataReader["Custom2"].ToString();
            Custom3 = dataReader["Custom3"] == DBNull.Value ? string.Empty : dataReader["Custom3"].ToString();


            return true;			
		}
			#endregion //Data Access - Fetch

		#region Data Access - Insert

		internal bool Insert(out string msgID, out int? DocKey)
		{
			bool retValue = false;
			msgID = "RecordAddFail";
			DocKey = null;
			using (TransactionScope scope = new TransactionScope())
			{
				// Create new sql connection for this method. 
				using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
				{
					 // Open sql connection. 
					cn.Open();
					retValue = this.Insert(cn, out msgID, out DocKey);
				}
				// No errors - commit transaction
				  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
			}// Already close and dispose sql connection.
			
			return retValue;
		}
		internal bool Insert(SqlConnection cn, out string msgID, out int? DocKey)
		{
			msgID = "RecordAddFail";
			DocKey=0;
			using(SqlCommand cm = cn.CreateCommand())
			{
				cm.CommandType = CommandType.StoredProcedure;
				cm.CommandText = "ARPYDetExpAddUpdate";

				cm.Parameters.AddWithValue("@Option", 0);
				cm.Parameters.AddWithValue("@MsgID", msgID);
				cm.Parameters.AddWithValue("@NewDocKey", DocKey);
				if (DocKey == null)
					cm.Parameters.AddWithValue("@DocKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocKey", DocKey);
				if (DocItmKey == null)
					cm.Parameters.AddWithValue("@DocItmKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocItmKey", DocItmKey);
				if (ExpSN == null)
					cm.Parameters.AddWithValue("@ExpSN", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpSN", ExpSN);
				if (ExpDeptKey == null)
					cm.Parameters.AddWithValue("@ExpDeptKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpDeptKey", ExpDeptKey);
				if (ExpTranGrpKey == null)
					cm.Parameters.AddWithValue("@ExpTranGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTranGrpKey", ExpTranGrpKey);
				if (ExpAccKey == null)
					cm.Parameters.AddWithValue("@ExpAccKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAccKey", ExpAccKey);
				if (ExpDate == null)
					cm.Parameters.AddWithValue("@ExpDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpDate", ExpDate);
				if (ExpRef == null)
					cm.Parameters.AddWithValue("@ExpRef", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpRef", ExpRef);
				if (ExpDes == null)
					cm.Parameters.AddWithValue("@ExpDes", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpDes", ExpDes);
				if (ExpAmtF == null)
					cm.Parameters.AddWithValue("@ExpAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAmtF", ExpAmtF);
				if (ExpAmtH == null)
					cm.Parameters.AddWithValue("@ExpAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAmtH", ExpAmtH);
				if (ExpAmtGST == null)
					cm.Parameters.AddWithValue("@ExpAmtGST", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAmtGST", ExpAmtGST);
				if (ExpTaxable == null)
					cm.Parameters.AddWithValue("@ExpTaxable", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxable", ExpTaxable);
				if (ExpTaxGrpKey == null)
					cm.Parameters.AddWithValue("@ExpTaxGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxGrpKey", ExpTaxGrpKey);
				if (ExpTaxGrpRate == null)
					cm.Parameters.AddWithValue("@ExpTaxGrpRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxGrpRate", ExpTaxGrpRate);
				if (ExpTaxGrpAmtF == null)
					cm.Parameters.AddWithValue("@ExpTaxGrpAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxGrpAmtF", ExpTaxGrpAmtF);
				if (ExpTaxGrpAmtL == null)
					cm.Parameters.AddWithValue("@ExpTaxGrpAmtL", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxGrpAmtL", ExpTaxGrpAmtL);
				if (ExpJobKey == null)
					cm.Parameters.AddWithValue("@ExpJobKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpJobKey", ExpJobKey);
				if (ExpJobPhaseKey == null)
					cm.Parameters.AddWithValue("@ExpJobPhaseKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpJobPhaseKey", ExpJobPhaseKey);
				if (ExpJobTaskKey == null)
					cm.Parameters.AddWithValue("@ExpJobTaskKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpJobTaskKey", ExpJobTaskKey);
				if (ExpJobCostTypeKey == null)
					cm.Parameters.AddWithValue("@ExpJobCostTypeKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpJobCostTypeKey", ExpJobCostTypeKey);
				if (ExpAttachment == null)
					cm.Parameters.AddWithValue("@ExpAttachment", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAttachment", ExpAttachment);
				if (CreateDate == null)
					cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@CreateDate", CreateDate);
				if (AppInfor.currentUserKey == null)
					cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
				if (LastModifiedDate == null)
					cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LastModifiedDate", LastModifiedDate);
				cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
				if (Custom1 == null)
					cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Custom1", Custom1);
				if (Custom2 == null)
					cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Custom2", Custom2);
				if (Custom3 == null)
					cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Custom3", Custom3);
				
				cm.Parameters.AddWithValue("@RetValue", 0);
				cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
				cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;
				// Execute command.
				cm.ExecuteNonQuery();
				if (cm.Parameters["@MsgID"].Value == null)
					msgID = string.Empty;
				else
					msgID = cm.Parameters["@MsgID"].Value.ToString();

				DocKey =Convert.ToInt32(cm.Parameters["@NewDocKey"].Value.ToString());

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
				cm.CommandText = "ARPYDetExpAddUpdate";

				cm.Parameters.AddWithValue("@Option", 1);
				cm.Parameters.AddWithValue("@MsgID", msgID);
				cm.Parameters.AddWithValue("@NewDocKey", 0);
				if (DocKey == null)
					cm.Parameters.AddWithValue("@DocKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocKey", DocKey);
				if (DocItmKey == null)
					cm.Parameters.AddWithValue("@DocItmKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocItmKey", DocItmKey);
				if (ExpSN == null)
					cm.Parameters.AddWithValue("@ExpSN", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpSN", ExpSN);
				if (ExpDeptKey == null)
					cm.Parameters.AddWithValue("@ExpDeptKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpDeptKey", ExpDeptKey);
				if (ExpTranGrpKey == null)
					cm.Parameters.AddWithValue("@ExpTranGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTranGrpKey", ExpTranGrpKey);
				if (ExpAccKey == null)
					cm.Parameters.AddWithValue("@ExpAccKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAccKey", ExpAccKey);
				if (ExpDate == null)
					cm.Parameters.AddWithValue("@ExpDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpDate", ExpDate);
				if (ExpRef == null)
					cm.Parameters.AddWithValue("@ExpRef", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpRef", ExpRef);
				if (ExpDes == null)
					cm.Parameters.AddWithValue("@ExpDes", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpDes", ExpDes);
				if (ExpAmtF == null)
					cm.Parameters.AddWithValue("@ExpAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAmtF", ExpAmtF);
				if (ExpAmtH == null)
					cm.Parameters.AddWithValue("@ExpAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAmtH", ExpAmtH);
				if (ExpAmtGST == null)
					cm.Parameters.AddWithValue("@ExpAmtGST", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAmtGST", ExpAmtGST);
				if (ExpTaxable == null)
					cm.Parameters.AddWithValue("@ExpTaxable", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxable", ExpTaxable);
				if (ExpTaxGrpKey == null)
					cm.Parameters.AddWithValue("@ExpTaxGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxGrpKey", ExpTaxGrpKey);
				if (ExpTaxGrpRate == null)
					cm.Parameters.AddWithValue("@ExpTaxGrpRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxGrpRate", ExpTaxGrpRate);
				if (ExpTaxGrpAmtF == null)
					cm.Parameters.AddWithValue("@ExpTaxGrpAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxGrpAmtF", ExpTaxGrpAmtF);
				if (ExpTaxGrpAmtL == null)
					cm.Parameters.AddWithValue("@ExpTaxGrpAmtL", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxGrpAmtL", ExpTaxGrpAmtL);
				if (ExpJobKey == null)
					cm.Parameters.AddWithValue("@ExpJobKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpJobKey", ExpJobKey);
				if (ExpJobPhaseKey == null)
					cm.Parameters.AddWithValue("@ExpJobPhaseKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpJobPhaseKey", ExpJobPhaseKey);
				if (ExpJobTaskKey == null)
					cm.Parameters.AddWithValue("@ExpJobTaskKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpJobTaskKey", ExpJobTaskKey);
				if (ExpJobCostTypeKey == null)
					cm.Parameters.AddWithValue("@ExpJobCostTypeKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpJobCostTypeKey", ExpJobCostTypeKey);
				if (ExpAttachment == null)
					cm.Parameters.AddWithValue("@ExpAttachment", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAttachment", ExpAttachment);
				if (CreateDate == null)
					cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@CreateDate", CreateDate);
				if (CreateUserKey == null)
					cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@CreateUserKey", CreateUserKey);
				if (LastModifiedDate == null)
					cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LastModifiedDate", LastModifiedDate);
				if (AppInfor.currentUserKey == null)
					cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
				if (Custom1 == null)
					cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Custom1", Custom1);
				if (Custom2 == null)
					cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Custom2", Custom2);
				if (Custom3 == null)
					cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Custom3", Custom3);
				
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
				cm.CommandText = "ARPYDetExp_Delete";

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