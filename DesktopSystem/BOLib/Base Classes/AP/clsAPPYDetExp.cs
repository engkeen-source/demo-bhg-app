using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
	/// <summary>
	/// Summary description for APPYDetExp.
	/// </summary>
	[Serializable]
	public class APPYDetExp : DocPayExpense, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public APPYDetExp()
            : base()
        {

        }


        public APPYDetExp Clone()
        {
            APPYDetExp objCopy = (APPYDetExp)this.MemberwiseClone();
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




        #endregion
        //public APPYDetExp Clone()
        //{
        //    APPYDetExp objCopy = (APPYDetExp)this.MemberwiseClone();
        //    objCopy._isDirty = false;
        //    return objCopy;
        //}

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
				cm.CommandText = "APPYDetExp_Get";
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
		internal static APPYDetExp Get(IDataReader dr, out string msgID)
		{
			msgID = "RecordGetFail";
			APPYDetExp child = new APPYDetExp();
			child.Fetch(dr, out msgID);
			return child;
		}

		internal bool Fetch(IDataReader dataReader, out string msgID)
		{
            msgID = "";
			// Fill data to entity object
			_DocKey =(int)dataReader["DocKey"];
            _DocItmKey = (int)dataReader["DocItmKey"];
            _ExpSN = (decimal)dataReader["ExpSN"];
            _ExpDeptKey =(int)dataReader["ExpDeptKey"];
            _ExpTranGrpKey =dataReader["ExpTranGrpKey"] == DBNull.Value ? null : (int?)dataReader["ExpTranGrpKey"];
            _ExpAccKey =(int)dataReader["ExpAccKey"];
            _ExpDate =dataReader["ExpDate"] == DBNull.Value ? null : (DateTime?)dataReader["ExpDate"];
            _ExpRef =dataReader["ExpRef"].ToString();
            _ExpDes =dataReader["ExpDes"] == DBNull.Value ? string.Empty:dataReader["ExpDes"].ToString();
            _ExpAmtF = (decimal)dataReader["ExpAmtF"];
            _ExpAmtH =(decimal)dataReader["ExpAmtH"];
            _ExpAmtGST =(decimal)dataReader["ExpAmtGST"];
            _ExpFreightCostKey = (int)dataReader["ExpFreightCostKey"];
            _ExpTaxable =dataReader["ExpTaxable"] == DBNull.Value ? false : (bool)dataReader["ExpTaxable"];
            _ExpTaxGrpKey =dataReader["ExpTaxGrpKey"] == DBNull.Value ? null : (int?)dataReader["ExpTaxGrpKey"];
            _ExpTaxGrpRate =dataReader["ExpTaxGrpRate"] == DBNull.Value ? null : (decimal?)dataReader["ExpTaxGrpRate"];
            _ExpTaxGrpAmtF =dataReader["ExpTaxGrpAmtF"] == DBNull.Value ? null : (decimal?)dataReader["ExpTaxGrpAmtF"];
            _ExpTaxGrpAmtL =dataReader["ExpTaxGrpAmtL"] == DBNull.Value ? null : (decimal?)dataReader["ExpTaxGrpAmtL"];
            _ExpJobKey = (int)dataReader["ExpJobKey"];
            _ExpJobPhaseKey = (int)dataReader["ExpJobPhaseKey"];
            _ExpJobTaskKey =(int)dataReader["ExpJobTaskKey"];
            _ExpJobCostTypeKey = (int)dataReader["ExpJobCostTypeKey"];
            _ExpAttachment = dataReader["ExpAttachment"] == DBNull.Value ? false : (bool)dataReader["ExpAttachment"];
            _CreateDate =dataReader["CreateDate"] == DBNull.Value ? null : (DateTime?)dataReader["CreateDate"];
            _CreateUserKey =dataReader["CreateUserKey"] == DBNull.Value ? null : (int?)dataReader["CreateUserKey"];
            _LastModifiedDate =dataReader["LastModifiedDate"] == DBNull.Value ? null : (DateTime?)dataReader["LastModifiedDate"];
            _LastModifiedUserKey =dataReader["LastModifiedUserKey"] == DBNull.Value ? null : (int?)dataReader["LastModifiedUserKey"];
            _Custom1 =dataReader["Custom1"] == DBNull.Value ? string.Empty:dataReader["Custom1"].ToString();
            _Custom2 =dataReader["Custom2"] == DBNull.Value ? string.Empty:dataReader["Custom2"].ToString();
            _Custom3 =dataReader["Custom3"] == DBNull.Value ? string.Empty:dataReader["Custom3"].ToString();



            return true;
		}
			#endregion //Data Access - Fetch

		#region Data Access - Insert

		internal bool Insert(out string msgID, out int? DocKey)
		{
			bool retValue = false;
			msgID = "RecordAddFail";
			DocKey = null;
			try
			{
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
			}
			 catch (Exception ex)
			{
                throw (ex);
			}
			return retValue;
		}
		internal bool Insert(SqlConnection cn, out string msgID, out int? DocKey)
		{
			 msgID = "RecordAddFail";
			DocKey=0;
			    using(SqlCommand cm = cn.CreateCommand())
			{
				cm.CommandType = CommandType.StoredProcedure;
				cm.CommandText = "APPYDetExp_AddUpdate";

				cm.Parameters.AddWithValue("@Option", 0);
				cm.Parameters.AddWithValue("@MsgID", msgID);
				cm.Parameters.AddWithValue("@NewDocKey", DocKey);
				if (_DocKey == null)
					cm.Parameters.AddWithValue("@DocKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocKey", _DocKey);
				if (_DocItmKey == null)
					cm.Parameters.AddWithValue("@DocItmKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocItmKey", _DocItmKey);
				if (_ExpSN == null)
					cm.Parameters.AddWithValue("@ExpSN", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpSN", _ExpSN);
				if (_ExpDeptKey == null)
					cm.Parameters.AddWithValue("@ExpDeptKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpDeptKey", _ExpDeptKey);
				if (_ExpTranGrpKey == null)
					cm.Parameters.AddWithValue("@ExpTranGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTranGrpKey", _ExpTranGrpKey);
				if (_ExpAccKey == null)
					cm.Parameters.AddWithValue("@ExpAccKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAccKey", _ExpAccKey);
				if (_ExpDate == null)
					cm.Parameters.AddWithValue("@ExpDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpDate", _ExpDate);
				if (_ExpRef == null)
					cm.Parameters.AddWithValue("@ExpRef", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpRef", _ExpRef);
				if (_ExpDes == null)
					cm.Parameters.AddWithValue("@ExpDes", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpDes", _ExpDes);
				if (_ExpAmtF == null)
					cm.Parameters.AddWithValue("@ExpAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAmtF", _ExpAmtF);
				if (_ExpAmtH == null)
					cm.Parameters.AddWithValue("@ExpAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAmtH", _ExpAmtH);
				if (_ExpAmtGST == null)
					cm.Parameters.AddWithValue("@ExpAmtGST", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAmtGST", _ExpAmtGST);
                if (_ExpFreightCostKey == null)
                    cm.Parameters.AddWithValue("@ExpFreightCostKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ExpFreightCostKey", _ExpFreightCostKey);
				if (_ExpTaxable == null)
					cm.Parameters.AddWithValue("@ExpTaxable", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxable", _ExpTaxable);
				if (_ExpTaxGrpKey == null)
					cm.Parameters.AddWithValue("@ExpTaxGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxGrpKey", _ExpTaxGrpKey);
				if (_ExpTaxGrpRate == null)
					cm.Parameters.AddWithValue("@ExpTaxGrpRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxGrpRate", _ExpTaxGrpRate);
				if (_ExpTaxGrpAmtF == null)
					cm.Parameters.AddWithValue("@ExpTaxGrpAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxGrpAmtF", _ExpTaxGrpAmtF);
				if (_ExpTaxGrpAmtL == null)
					cm.Parameters.AddWithValue("@ExpTaxGrpAmtL", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxGrpAmtL", _ExpTaxGrpAmtL);
				if (_ExpJobKey == null)
					cm.Parameters.AddWithValue("@ExpJobKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpJobKey", _ExpJobKey);
				if (_ExpJobPhaseKey == null)
					cm.Parameters.AddWithValue("@ExpJobPhaseKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpJobPhaseKey", _ExpJobPhaseKey);
				if (_ExpJobTaskKey == null)
					cm.Parameters.AddWithValue("@ExpJobTaskKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpJobTaskKey", _ExpJobTaskKey);
				if (_ExpJobCostTypeKey == null)
					cm.Parameters.AddWithValue("@ExpJobCostTypeKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpJobCostTypeKey", _ExpJobCostTypeKey);
				if (_ExpAttachment == null)
					cm.Parameters.AddWithValue("@ExpAttachment", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAttachment", _ExpAttachment);
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
				if (_Custom1 == null)
					cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Custom1", _Custom1);
				if (_Custom2 == null)
					cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Custom2", _Custom2);
				if (_Custom3 == null)
					cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Custom3", _Custom3);
				
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
				cm.CommandText = "APPYDetExp_AddUpdate";

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
				if (_ExpSN == null)
					cm.Parameters.AddWithValue("@ExpSN", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpSN", _ExpSN);
				if (_ExpDeptKey == null)
					cm.Parameters.AddWithValue("@ExpDeptKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpDeptKey", _ExpDeptKey);
				if (_ExpTranGrpKey == null)
					cm.Parameters.AddWithValue("@ExpTranGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTranGrpKey", _ExpTranGrpKey);
				if (_ExpAccKey == null)
					cm.Parameters.AddWithValue("@ExpAccKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAccKey", _ExpAccKey);
				if (_ExpDate == null)
					cm.Parameters.AddWithValue("@ExpDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpDate", _ExpDate);
				if (_ExpRef == null)
					cm.Parameters.AddWithValue("@ExpRef", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpRef", _ExpRef);
				if (_ExpDes == null)
					cm.Parameters.AddWithValue("@ExpDes", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpDes", _ExpDes);
				if (_ExpAmtF == null)
					cm.Parameters.AddWithValue("@ExpAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAmtF", _ExpAmtF);
				if (_ExpAmtH == null)
					cm.Parameters.AddWithValue("@ExpAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAmtH", _ExpAmtH);
				if (_ExpAmtGST == null)
					cm.Parameters.AddWithValue("@ExpAmtGST", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAmtGST", _ExpAmtGST);
				if (_ExpTaxable == null)
					cm.Parameters.AddWithValue("@ExpTaxable", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxable", _ExpTaxable);
				if (_ExpTaxGrpKey == null)
					cm.Parameters.AddWithValue("@ExpTaxGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxGrpKey", _ExpTaxGrpKey);
				if (_ExpTaxGrpRate == null)
					cm.Parameters.AddWithValue("@ExpTaxGrpRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxGrpRate", _ExpTaxGrpRate);
				if (_ExpTaxGrpAmtF == null)
					cm.Parameters.AddWithValue("@ExpTaxGrpAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxGrpAmtF", _ExpTaxGrpAmtF);
				if (_ExpTaxGrpAmtL == null)
					cm.Parameters.AddWithValue("@ExpTaxGrpAmtL", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpTaxGrpAmtL", _ExpTaxGrpAmtL);
				if (_ExpJobKey == null)
					cm.Parameters.AddWithValue("@ExpJobKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpJobKey", _ExpJobKey);
				if (_ExpJobPhaseKey == null)
					cm.Parameters.AddWithValue("@ExpJobPhaseKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpJobPhaseKey", _ExpJobPhaseKey);
				if (_ExpJobTaskKey == null)
					cm.Parameters.AddWithValue("@ExpJobTaskKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpJobTaskKey", _ExpJobTaskKey);
				if (_ExpJobCostTypeKey == null)
					cm.Parameters.AddWithValue("@ExpJobCostTypeKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpJobCostTypeKey", _ExpJobCostTypeKey);
				if (_ExpAttachment == null)
					cm.Parameters.AddWithValue("@ExpAttachment", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ExpAttachment", _ExpAttachment);
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
				if (_Custom1 == null)
					cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Custom1", _Custom1);
				if (_Custom2 == null)
					cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Custom2", _Custom2);
				if (_Custom3 == null)
					cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Custom3", _Custom3);
				
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
			try
			{
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
			}
			 catch (Exception ex)
			{
                throw (ex);
			}
			return retValue;
		}
		internal bool Delete(SqlConnection cn, Criteria criteria, out string msgID)
		{
			bool retValue = false;
			 msgID = "RecordDeleteFail";
			try
			{
				using(SqlCommand cm = cn.CreateCommand())
				{
					cm.CommandType = CommandType.StoredProcedure;
					cm.CommandText = "APPYDetExp_Delete";

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
						retValue = true;
				}// Already close and dispose sql command.
			}
			 catch (Exception ex)
			{
                throw (ex);
			}
			return retValue;
		}
		#endregion Delete

    }
}