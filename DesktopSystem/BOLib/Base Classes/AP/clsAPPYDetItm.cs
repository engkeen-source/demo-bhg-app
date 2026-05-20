using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
	/// <summary>
	/// Summary description for APPYDetItm.
	/// </summary>
	[Serializable]
    public class APPYDetItm : DocPayItem, IDataErrorInfo, INotifyPropertyChanged

    {
      
        #region +++  Local variables declaration for the class +++
       
        private string _error = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public APPYDetItm()
            : base()
        {
            
            this._isDirty = false;
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

        public bool IsDirty
        {
            get
            {
                return this._isDirty || base._isDirty;
            }
        }


        #endregion
        //public APPYDetItm Clone()
        //{

        //    APPYDetItm objCopy = (APPYDetItm)this.MemberwiseClone();
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
				cm.CommandText = "APPYDetItm_Get";
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
		internal static APPYDetItm Get(IDataReader dr, out string msgID)
		{
			msgID = "RecordGetFail";
			APPYDetItm child = new APPYDetItm();
			child.Fetch(dr, out msgID);
			return child;
		}

		internal bool Fetch(IDataReader dataReader, out string msgID)
		{
			msgID = "RecordGetFail";
			
			// Fill data to entity object            
            _DocKey = dataReader["DocKey"] == DBNull.Value ? null : (int?)dataReader["DocKey"];
            _DocItmKey = dataReader["DocItmKey"] == DBNull.Value ? null : (int?)dataReader["DocItmKey"];
            _LinkDocDC = dataReader["LinkDocDC"] == DBNull.Value ? null : (int?)dataReader["LinkDocDC"];
            _LinkDocDK = dataReader["LinkDocDK"] == DBNull.Value ? null : (int?)dataReader["LinkDocDK"];
            _LinkDocID = dataReader["LinkDocID"] == DBNull.Value ? string.Empty : dataReader["LinkDocID"].ToString();
            _LinkDocDate = dataReader["LinkDocDate"] == DBNull.Value ? null : (DateTime?)dataReader["LinkDocDate"];
            _LinkDocType = dataReader["LinkDocType"] == DBNull.Value ? null : (int?)dataReader["LinkDocType"];
            _LinkDocTypeNm = dataReader["LinkDocTypeNm"] == DBNull.Value ? string.Empty : dataReader["LinkDocTypeNm"].ToString();
            _LinkDocDeptKey = dataReader["LinkDocDeptKey"] == DBNull.Value ? null : (int?)dataReader["LinkDocDeptKey"];
            _LinkDocTranGrpKey = dataReader["LinkDocTranGrpKey"] == DBNull.Value ? null : (int?)dataReader["LinkDocTranGrpKey"];
            _LinkDocAccKey = dataReader["LinkDocAccKey"] == DBNull.Value ? null : (int?)dataReader["LinkDocAccKey"];
            _LinkDocTermKey = dataReader["LinkDocTermKey"] == DBNull.Value ? null : (int?)dataReader["LinkDocTermKey"];
            _LinkDocDisDate = dataReader["LinkDocDisDate"] == DBNull.Value ? null : (DateTime?)dataReader["LinkDocDisDate"];
            _LinkDocDueDate = dataReader["LinkDocDueDate"] == DBNull.Value ? null : (DateTime?)dataReader["LinkDocDueDate"];
            _LinkDocGrand = dataReader["LinkDocGrand"] == DBNull.Value ? null : (decimal?)dataReader["LinkDocGrand"];
            _LinkDocHome = dataReader["LinkDocHome"] == DBNull.Value ? null : (decimal?)dataReader["LinkDocHome"];
            _LinkDocCurrKey = dataReader["LinkDocCurrKey"] == DBNull.Value ? null : (int?)dataReader["LinkDocCurrKey"];
            _LinkDocCurrRate = dataReader["LinkDocCurrRate"] == DBNull.Value ? null : (decimal?)dataReader["LinkDocCurrRate"];
            _LinkDocRef = dataReader["LinkDocRef"] == DBNull.Value ? string.Empty : dataReader["LinkDocRef"].ToString();
            _ItmApplyDueAmtF = dataReader["ItmApplyDueAmtF"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyDueAmtF"];
            _ItmApplyDueAmtH = dataReader["ItmApplyDueAmtH"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyDueAmtH"];
            _ItmApplyRate = dataReader["ItmApplyRate"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyRate"];
            _ItmApplyDisAmtF = dataReader["ItmApplyDisAmtF"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyDisAmtF"];
            _ItmApplyDisAmtH = dataReader["ItmApplyDisAmtH"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyDisAmtH"];
            _ItmApplyDisAccKey = dataReader["ItmApplyDisAccKey"] == DBNull.Value ? null : (int?)dataReader["ItmApplyDisAccKey"];
            _ItmApplyDocAmtF = dataReader["ItmApplyDocAmtF"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyDocAmtF"];
            _ItmApplyDocAmtH = dataReader["ItmApplyDocAmtH"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyDocAmtH"];
            _ItmApplyPayAmtF = dataReader["ItmApplyPayAmtF"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyPayAmtF"];
            _ItmApplyPayAmtH = dataReader["ItmApplyPayAmtH"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyPayAmtH"];
            _ItmApplyGainAmt = dataReader["ItmApplyGainAmt"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyGainAmt"];
            _ItmApplyGainAccKey = dataReader["ItmApplyGainAccKey"] == DBNull.Value ? null : (int?)dataReader["ItmApplyGainAccKey"];
            _ItmApplyFull = dataReader["ItmApplyFull"] == DBNull.Value ? false : (bool)dataReader["ItmApplyFull"];
            _ItmAttachment = dataReader["ItmAttachment"] == DBNull.Value ? false : (bool)dataReader["ItmAttachment"];
            _CreateDate = dataReader["CreateDate"] == DBNull.Value ? null : (DateTime?)dataReader["CreateDate"];
            _CreateUserKey = dataReader["CreateUserKey"] == DBNull.Value ? null : (int?)dataReader["CreateUserKey"];
            _LastModifiedDate = dataReader["LastModifiedDate"] == DBNull.Value ? null : (DateTime?)dataReader["LastModifiedDate"];
            _LastModifiedUserKey = dataReader["LastModifiedUserKey"] == DBNull.Value ? null : (int?)dataReader["LastModifiedUserKey"];
            _Custom1 = dataReader["Custom1"] == DBNull.Value ? string.Empty : dataReader["Custom1"].ToString();
            _Custom2 = dataReader["Custom2"] == DBNull.Value ? string.Empty : dataReader["Custom2"].ToString();
            _Custom3 = dataReader["Custom3"] == DBNull.Value ? string.Empty : dataReader["Custom3"].ToString();


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
				cm.CommandText = "APPYDetItm_AddUpdate";

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
				if (_LinkDocDC == null)
					cm.Parameters.AddWithValue("@LinkDocDC", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDC", _LinkDocDC);
				if (_LinkDocDK == null)
					cm.Parameters.AddWithValue("@LinkDocDK", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDK", _LinkDocDK);
				if (_LinkDocID == null)
					cm.Parameters.AddWithValue("@LinkDocID", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocID", _LinkDocID);
				if (_LinkDocDate == null)
					cm.Parameters.AddWithValue("@LinkDocDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDate", _LinkDocDate);
				if (_LinkDocType == null)
					cm.Parameters.AddWithValue("@LinkDocType", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocType", _LinkDocType);
				if (_LinkDocTypeNm == null)
					cm.Parameters.AddWithValue("@LinkDocTypeNm", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocTypeNm", _LinkDocTypeNm);
				if (_LinkDocDeptKey == null)
					cm.Parameters.AddWithValue("@LinkDocDeptKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDeptKey", _LinkDocDeptKey);
				if (_LinkDocTranGrpKey == null)
					cm.Parameters.AddWithValue("@LinkDocTranGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocTranGrpKey", _LinkDocTranGrpKey);
				if (_LinkDocAccKey == null)
					cm.Parameters.AddWithValue("@LinkDocAccKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocAccKey", _LinkDocAccKey);
				if (_LinkDocTermKey == null)
					cm.Parameters.AddWithValue("@LinkDocTermKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocTermKey", _LinkDocTermKey);
				if (_LinkDocDisDate == null)
					cm.Parameters.AddWithValue("@LinkDocDisDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDisDate", _LinkDocDisDate);
				if (_LinkDocDueDate == null)
					cm.Parameters.AddWithValue("@LinkDocDueDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDueDate", _LinkDocDueDate);
				if (_LinkDocGrand == null)
					cm.Parameters.AddWithValue("@LinkDocGrand", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocGrand", _LinkDocGrand);
				if (_LinkDocHome == null)
					cm.Parameters.AddWithValue("@LinkDocHome", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocHome", _LinkDocHome);
				if (_LinkDocCurrKey == null)
					cm.Parameters.AddWithValue("@LinkDocCurrKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocCurrKey", _LinkDocCurrKey);
				if (_LinkDocCurrRate == null)
					cm.Parameters.AddWithValue("@LinkDocCurrRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocCurrRate", _LinkDocCurrRate);
				if (_LinkDocRef == null)
					cm.Parameters.AddWithValue("@LinkDocRef", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocRef", _LinkDocRef);
				if (_ItmApplyDueAmtF == null)
					cm.Parameters.AddWithValue("@ItmApplyDueAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDueAmtF", _ItmApplyDueAmtF);
				if (_ItmApplyDueAmtH == null)
					cm.Parameters.AddWithValue("@ItmApplyDueAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDueAmtH", _ItmApplyDueAmtH);
				if (_ItmApplyRate == null)
					cm.Parameters.AddWithValue("@ItmApplyRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyRate", _ItmApplyRate);
				if (_ItmApplyDisAmtF == null)
					cm.Parameters.AddWithValue("@ItmApplyDisAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDisAmtF", _ItmApplyDisAmtF);
				if (_ItmApplyDisAmtH == null)
					cm.Parameters.AddWithValue("@ItmApplyDisAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDisAmtH", _ItmApplyDisAmtH);
				if (_ItmApplyDisAccKey == null)
					cm.Parameters.AddWithValue("@ItmApplyDisAccKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDisAccKey", _ItmApplyDisAccKey);
				if (_ItmApplyDocAmtF == null)
					cm.Parameters.AddWithValue("@ItmApplyDocAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDocAmtF", _ItmApplyDocAmtF);
				if (_ItmApplyDocAmtH == null)
					cm.Parameters.AddWithValue("@ItmApplyDocAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDocAmtH", _ItmApplyDocAmtH);
				if (_ItmApplyPayAmtF == null)
					cm.Parameters.AddWithValue("@ItmApplyPayAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyPayAmtF", _ItmApplyPayAmtF);
				if (_ItmApplyPayAmtH == null)
					cm.Parameters.AddWithValue("@ItmApplyPayAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyPayAmtH", _ItmApplyPayAmtH);
				if (_ItmApplyGainAmt == null)
					cm.Parameters.AddWithValue("@ItmApplyGainAmt", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyGainAmt", _ItmApplyGainAmt);
				if (_ItmApplyGainAccKey == null)
					cm.Parameters.AddWithValue("@ItmApplyGainAccKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyGainAccKey", _ItmApplyGainAccKey);
				if (_ItmApplyFull == null)
					cm.Parameters.AddWithValue("@ItmApplyFull", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyFull", _ItmApplyFull);
				if (_ItmAttachment == null)
					cm.Parameters.AddWithValue("@ItmAttachment", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmAttachment", _ItmAttachment);
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
				cm.CommandText = "APPYDetItm_AddUpdate";

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
				if (_LinkDocDC == null)
					cm.Parameters.AddWithValue("@LinkDocDC", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDC", _LinkDocDC);
				if (_LinkDocDK == null)
					cm.Parameters.AddWithValue("@LinkDocDK", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDK", _LinkDocDK);
				if (_LinkDocID == null)
					cm.Parameters.AddWithValue("@LinkDocID", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocID", _LinkDocID);
				if (_LinkDocDate == null)
					cm.Parameters.AddWithValue("@LinkDocDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDate", _LinkDocDate);
				if (_LinkDocType == null)
					cm.Parameters.AddWithValue("@LinkDocType", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocType", _LinkDocType);
				if (_LinkDocTypeNm == null)
					cm.Parameters.AddWithValue("@LinkDocTypeNm", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocTypeNm", _LinkDocTypeNm);
				if (_LinkDocDeptKey == null)
					cm.Parameters.AddWithValue("@LinkDocDeptKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDeptKey", _LinkDocDeptKey);
				if (_LinkDocTranGrpKey == null)
					cm.Parameters.AddWithValue("@LinkDocTranGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocTranGrpKey", _LinkDocTranGrpKey);
				if (_LinkDocAccKey == null)
					cm.Parameters.AddWithValue("@LinkDocAccKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocAccKey", _LinkDocAccKey);
				if (_LinkDocTermKey == null)
					cm.Parameters.AddWithValue("@LinkDocTermKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocTermKey", _LinkDocTermKey);
				if (_LinkDocDisDate == null)
					cm.Parameters.AddWithValue("@LinkDocDisDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDisDate", _LinkDocDisDate);
				if (_LinkDocDueDate == null)
					cm.Parameters.AddWithValue("@LinkDocDueDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDueDate", _LinkDocDueDate);
				if (_LinkDocGrand == null)
					cm.Parameters.AddWithValue("@LinkDocGrand", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocGrand", _LinkDocGrand);
				if (_LinkDocHome == null)
					cm.Parameters.AddWithValue("@LinkDocHome", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocHome", _LinkDocHome);
				if (_LinkDocCurrKey == null)
					cm.Parameters.AddWithValue("@LinkDocCurrKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocCurrKey", _LinkDocCurrKey);
				if (_LinkDocCurrRate == null)
					cm.Parameters.AddWithValue("@LinkDocCurrRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocCurrRate", _LinkDocCurrRate);
				if (_LinkDocRef == null)
					cm.Parameters.AddWithValue("@LinkDocRef", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocRef", _LinkDocRef);
				if (_ItmApplyDueAmtF == null)
					cm.Parameters.AddWithValue("@ItmApplyDueAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDueAmtF", _ItmApplyDueAmtF);
				if (_ItmApplyDueAmtH == null)
					cm.Parameters.AddWithValue("@ItmApplyDueAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDueAmtH", _ItmApplyDueAmtH);
				if (_ItmApplyRate == null)
					cm.Parameters.AddWithValue("@ItmApplyRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyRate", _ItmApplyRate);
				if (_ItmApplyDisAmtF == null)
					cm.Parameters.AddWithValue("@ItmApplyDisAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDisAmtF", _ItmApplyDisAmtF);
				if (_ItmApplyDisAmtH == null)
					cm.Parameters.AddWithValue("@ItmApplyDisAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDisAmtH", _ItmApplyDisAmtH);
				if (_ItmApplyDisAccKey == null)
					cm.Parameters.AddWithValue("@ItmApplyDisAccKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDisAccKey", _ItmApplyDisAccKey);
				if (_ItmApplyDocAmtF == null)
					cm.Parameters.AddWithValue("@ItmApplyDocAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDocAmtF", _ItmApplyDocAmtF);
				if (_ItmApplyDocAmtH == null)
					cm.Parameters.AddWithValue("@ItmApplyDocAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDocAmtH", _ItmApplyDocAmtH);
				if (_ItmApplyPayAmtF == null)
					cm.Parameters.AddWithValue("@ItmApplyPayAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyPayAmtF", _ItmApplyPayAmtF);
				if (_ItmApplyPayAmtH == null)
					cm.Parameters.AddWithValue("@ItmApplyPayAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyPayAmtH", _ItmApplyPayAmtH);
				if (_ItmApplyGainAmt == null)
					cm.Parameters.AddWithValue("@ItmApplyGainAmt", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyGainAmt", _ItmApplyGainAmt);
				if (_ItmApplyGainAccKey == null)
					cm.Parameters.AddWithValue("@ItmApplyGainAccKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyGainAccKey", _ItmApplyGainAccKey);
				if (_ItmApplyFull == null)
					cm.Parameters.AddWithValue("@ItmApplyFull", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyFull", _ItmApplyFull);
				if (_ItmAttachment == null)
					cm.Parameters.AddWithValue("@ItmAttachment", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmAttachment", _ItmAttachment);
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
				cm.CommandText = "APPYDetItm_Delete";

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