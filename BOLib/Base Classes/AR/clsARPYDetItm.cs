using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
	/// <summary>
	/// Summary description for ARPYDetItm.
	/// </summary>
	[Serializable]
    public class ARPYDetItm : DocPayItem, IDataErrorInfo, INotifyPropertyChanged
	{
         #region +++  Local variables declaration for the class +++
       
        private string _error = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public ARPYDetItm()
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

        public ARPYDetItm Clone()
        {

            ARPYDetItm objCopy = (ARPYDetItm)this.MemberwiseClone();
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
				cm.CommandText = "ARPYDetItm_Get";

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
		internal static ARPYDetItm Get(IDataReader dr, out string msgID)
		{
			msgID = "RecordGetFail";
			ARPYDetItm child = new ARPYDetItm();
			child.Fetch(dr, out msgID);
			return child;
		}

		internal bool Fetch(IDataReader dataReader, out string msgID)
		{
			msgID = "RecordGetFail";
			// Fill data to entity object
            DocKey = dataReader["DocKey"] == DBNull.Value ? null : (int?)dataReader["DocKey"];
            DocItmKey = dataReader["DocItmKey"] == DBNull.Value ? null : (int?)dataReader["DocItmKey"];
            LinkDocDC = dataReader["LinkDocDC"] == DBNull.Value ? null : (int?)dataReader["LinkDocDC"];
            LinkDocDK = dataReader["LinkDocDK"] == DBNull.Value ? null : (int?)dataReader["LinkDocDK"];
            LinkDocID = dataReader["LinkDocID"] == DBNull.Value ? string.Empty : dataReader["LinkDocID"].ToString();
            LinkDocDate = dataReader["LinkDocDate"] == DBNull.Value ? null : (DateTime?)dataReader["LinkDocDate"];
            LinkDocType = dataReader["LinkDocType"] == DBNull.Value ? null : (int?)dataReader["LinkDocType"];
            LinkDocTypeNm = dataReader["LinkDocTypeNm"] == DBNull.Value ? string.Empty : dataReader["LinkDocTypeNm"].ToString();
            LinkDocDeptKey = dataReader["LinkDocDeptKey"] == DBNull.Value ? null : (int?)dataReader["LinkDocDeptKey"];
            LinkDocTranGrpKey = dataReader["LinkDocTranGrpKey"] == DBNull.Value ? null : (int?)dataReader["LinkDocTranGrpKey"];
            LinkDocAccKey = dataReader["LinkDocAccKey"] == DBNull.Value ? null : (int?)dataReader["LinkDocAccKey"];
            LinkDocTermKey = dataReader["LinkDocTermKey"] == DBNull.Value ? null : (int?)dataReader["LinkDocTermKey"];
            LinkDocDisDate = dataReader["LinkDocDisDate"] == DBNull.Value ? null : (DateTime?)dataReader["LinkDocDisDate"];
            LinkDocDueDate = dataReader["LinkDocDueDate"] == DBNull.Value ? null : (DateTime?)dataReader["LinkDocDueDate"];
            LinkDocGrand = dataReader["LinkDocGrand"] == DBNull.Value ? null : (decimal?)dataReader["LinkDocGrand"];
            LinkDocHome = dataReader["LinkDocHome"] == DBNull.Value ? null : (decimal?)dataReader["LinkDocHome"];
            LinkDocCurrKey = dataReader["LinkDocCurrKey"] == DBNull.Value ? null : (int?)dataReader["LinkDocCurrKey"];
            LinkDocCurrRate = dataReader["LinkDocCurrRate"] == DBNull.Value ? null : (decimal?)dataReader["LinkDocCurrRate"];
            LinkDocRef = dataReader["LinkDocRef"] == DBNull.Value ? string.Empty : dataReader["LinkDocRef"].ToString();
            ItmApplyDueAmtF = dataReader["ItmApplyDueAmtF"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyDueAmtF"];
            ItmApplyDueAmtH = dataReader["ItmApplyDueAmtH"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyDueAmtH"];
            ItmApplyRate = dataReader["ItmApplyRate"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyRate"];
            ItmApplyDisAmtF = dataReader["ItmApplyDisAmtF"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyDisAmtF"];
            ItmApplyDisAmtH = dataReader["ItmApplyDisAmtH"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyDisAmtH"];
            ItmApplyDisAccKey = dataReader["ItmApplyDisAccKey"] == DBNull.Value ? null : (int?)dataReader["ItmApplyDisAccKey"];
            ItmApplyDocAmtF = dataReader["ItmApplyDocAmtF"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyDocAmtF"];
            ItmApplyDocAmtH = dataReader["ItmApplyDocAmtH"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyDocAmtH"];
            ItmApplyPayAmtF = dataReader["ItmApplyPayAmtF"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyPayAmtF"];
            ItmApplyPayAmtH = dataReader["ItmApplyPayAmtH"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyPayAmtH"];
            ItmApplyGainAmt = dataReader["ItmApplyGainAmt"] == DBNull.Value ? null : (decimal?)dataReader["ItmApplyGainAmt"];
            ItmApplyGainAccKey = dataReader["ItmApplyGainAccKey"] == DBNull.Value ? null : (int?)dataReader["ItmApplyGainAccKey"];
            ItmApplyFull = dataReader["ItmApplyFull"] == DBNull.Value ? false : (bool)dataReader["ItmApplyFull"];
            ItmAttachment = dataReader["ItmAttachment"] == DBNull.Value ? false : (bool)dataReader["ItmAttachment"];
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
				cm.CommandText = "ARPYDetItmAddUpdate";

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
				if (LinkDocDC == null)
					cm.Parameters.AddWithValue("@LinkDocDC", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDC", LinkDocDC);
				if (LinkDocDK == null)
					cm.Parameters.AddWithValue("@LinkDocDK", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDK", LinkDocDK);
				if (LinkDocID == null)
					cm.Parameters.AddWithValue("@LinkDocID", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocID", LinkDocID);
				if (LinkDocDate == null)
					cm.Parameters.AddWithValue("@LinkDocDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDate", LinkDocDate);
				if (LinkDocType == null)
					cm.Parameters.AddWithValue("@LinkDocType", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocType", LinkDocType);
				if (LinkDocTypeNm == null)
					cm.Parameters.AddWithValue("@LinkDocTypeNm", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocTypeNm", LinkDocTypeNm);
				if (LinkDocDeptKey == null)
					cm.Parameters.AddWithValue("@LinkDocDeptKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDeptKey", LinkDocDeptKey);
				if (LinkDocTranGrpKey == null)
					cm.Parameters.AddWithValue("@LinkDocTranGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocTranGrpKey", LinkDocTranGrpKey);
				if (LinkDocAccKey == null)
					cm.Parameters.AddWithValue("@LinkDocAccKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocAccKey", LinkDocAccKey);
				if (LinkDocTermKey == null)
					cm.Parameters.AddWithValue("@LinkDocTermKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocTermKey", LinkDocTermKey);
				if (LinkDocDisDate == null)
					cm.Parameters.AddWithValue("@LinkDocDisDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDisDate", LinkDocDisDate);
				if (LinkDocDueDate == null)
					cm.Parameters.AddWithValue("@LinkDocDueDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDueDate", LinkDocDueDate);
				if (LinkDocGrand == null)
					cm.Parameters.AddWithValue("@LinkDocGrand", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocGrand", LinkDocGrand);
				if (LinkDocHome == null)
					cm.Parameters.AddWithValue("@LinkDocHome", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocHome", LinkDocHome);
				if (LinkDocCurrKey == null)
					cm.Parameters.AddWithValue("@LinkDocCurrKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocCurrKey", LinkDocCurrKey);
				if (LinkDocCurrRate == null)
					cm.Parameters.AddWithValue("@LinkDocCurrRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocCurrRate", LinkDocCurrRate);
				if (LinkDocRef == null)
					cm.Parameters.AddWithValue("@LinkDocRef", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocRef", LinkDocRef);
				if (ItmApplyDueAmtF == null)
					cm.Parameters.AddWithValue("@ItmApplyDueAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDueAmtF", ItmApplyDueAmtF);
				if (ItmApplyDueAmtH == null)
					cm.Parameters.AddWithValue("@ItmApplyDueAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDueAmtH", ItmApplyDueAmtH);
				if (ItmApplyRate == null)
					cm.Parameters.AddWithValue("@ItmApplyRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyRate", ItmApplyRate);
				if (ItmApplyDisAmtF == null)
					cm.Parameters.AddWithValue("@ItmApplyDisAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDisAmtF", ItmApplyDisAmtF);
				if (ItmApplyDisAmtH == null)
					cm.Parameters.AddWithValue("@ItmApplyDisAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDisAmtH", ItmApplyDisAmtH);
				if (ItmApplyDisAccKey == null)
					cm.Parameters.AddWithValue("@ItmApplyDisAccKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDisAccKey", ItmApplyDisAccKey);
				if (ItmApplyDocAmtF == null)
					cm.Parameters.AddWithValue("@ItmApplyDocAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDocAmtF", ItmApplyDocAmtF);
				if (ItmApplyDocAmtH == null)
					cm.Parameters.AddWithValue("@ItmApplyDocAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDocAmtH", ItmApplyDocAmtH);
				if (ItmApplyPayAmtF == null)
					cm.Parameters.AddWithValue("@ItmApplyPayAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyPayAmtF", ItmApplyPayAmtF);
				if (ItmApplyPayAmtH == null)
					cm.Parameters.AddWithValue("@ItmApplyPayAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyPayAmtH", ItmApplyPayAmtH);
				if (ItmApplyGainAmt == null)
					cm.Parameters.AddWithValue("@ItmApplyGainAmt", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyGainAmt", ItmApplyGainAmt);
				if (ItmApplyGainAccKey == null)
					cm.Parameters.AddWithValue("@ItmApplyGainAccKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyGainAccKey", ItmApplyGainAccKey);
				if (ItmApplyFull == null)
					cm.Parameters.AddWithValue("@ItmApplyFull", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyFull", ItmApplyFull);
				if (ItmAttachment == null)
					cm.Parameters.AddWithValue("@ItmAttachment", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmAttachment", ItmAttachment);
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
				cm.CommandText = "ARPYDetItmAddUpdate";

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
				if (LinkDocDC == null)
					cm.Parameters.AddWithValue("@LinkDocDC", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDC", LinkDocDC);
				if (LinkDocDK == null)
					cm.Parameters.AddWithValue("@LinkDocDK", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDK", LinkDocDK);
				if (LinkDocID == null)
					cm.Parameters.AddWithValue("@LinkDocID", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocID", LinkDocID);
				if (LinkDocDate == null)
					cm.Parameters.AddWithValue("@LinkDocDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDate", LinkDocDate);
				if (LinkDocType == null)
					cm.Parameters.AddWithValue("@LinkDocType", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocType", LinkDocType);
				if (LinkDocTypeNm == null)
					cm.Parameters.AddWithValue("@LinkDocTypeNm", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocTypeNm", LinkDocTypeNm);
				if (LinkDocDeptKey == null)
					cm.Parameters.AddWithValue("@LinkDocDeptKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDeptKey", LinkDocDeptKey);
				if (LinkDocTranGrpKey == null)
					cm.Parameters.AddWithValue("@LinkDocTranGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocTranGrpKey", LinkDocTranGrpKey);
				if (LinkDocAccKey == null)
					cm.Parameters.AddWithValue("@LinkDocAccKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocAccKey", LinkDocAccKey);
				if (LinkDocTermKey == null)
					cm.Parameters.AddWithValue("@LinkDocTermKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocTermKey", LinkDocTermKey);
				if (LinkDocDisDate == null)
					cm.Parameters.AddWithValue("@LinkDocDisDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDisDate", LinkDocDisDate);
				if (LinkDocDueDate == null)
					cm.Parameters.AddWithValue("@LinkDocDueDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocDueDate", LinkDocDueDate);
				if (LinkDocGrand == null)
					cm.Parameters.AddWithValue("@LinkDocGrand", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocGrand", LinkDocGrand);
				if (LinkDocHome == null)
					cm.Parameters.AddWithValue("@LinkDocHome", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocHome", LinkDocHome);
				if (LinkDocCurrKey == null)
					cm.Parameters.AddWithValue("@LinkDocCurrKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocCurrKey", LinkDocCurrKey);
				if (LinkDocCurrRate == null)
					cm.Parameters.AddWithValue("@LinkDocCurrRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocCurrRate", LinkDocCurrRate);
				if (LinkDocRef == null)
					cm.Parameters.AddWithValue("@LinkDocRef", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LinkDocRef", LinkDocRef);
				if (ItmApplyDueAmtF == null)
					cm.Parameters.AddWithValue("@ItmApplyDueAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDueAmtF", ItmApplyDueAmtF);
				if (ItmApplyDueAmtH == null)
					cm.Parameters.AddWithValue("@ItmApplyDueAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDueAmtH", ItmApplyDueAmtH);
				if (ItmApplyRate == null)
					cm.Parameters.AddWithValue("@ItmApplyRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyRate", ItmApplyRate);
				if (ItmApplyDisAmtF == null)
					cm.Parameters.AddWithValue("@ItmApplyDisAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDisAmtF", ItmApplyDisAmtF);
				if (ItmApplyDisAmtH == null)
					cm.Parameters.AddWithValue("@ItmApplyDisAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDisAmtH", ItmApplyDisAmtH);
				if (ItmApplyDisAccKey == null)
					cm.Parameters.AddWithValue("@ItmApplyDisAccKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDisAccKey", ItmApplyDisAccKey);
				if (ItmApplyDocAmtF == null)
					cm.Parameters.AddWithValue("@ItmApplyDocAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDocAmtF", ItmApplyDocAmtF);
				if (ItmApplyDocAmtH == null)
					cm.Parameters.AddWithValue("@ItmApplyDocAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyDocAmtH", ItmApplyDocAmtH);
				if (ItmApplyPayAmtF == null)
					cm.Parameters.AddWithValue("@ItmApplyPayAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyPayAmtF", ItmApplyPayAmtF);
				if (ItmApplyPayAmtH == null)
					cm.Parameters.AddWithValue("@ItmApplyPayAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyPayAmtH", ItmApplyPayAmtH);
				if (ItmApplyGainAmt == null)
					cm.Parameters.AddWithValue("@ItmApplyGainAmt", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyGainAmt", ItmApplyGainAmt);
				if (ItmApplyGainAccKey == null)
					cm.Parameters.AddWithValue("@ItmApplyGainAccKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyGainAccKey", ItmApplyGainAccKey);
				if (ItmApplyFull == null)
					cm.Parameters.AddWithValue("@ItmApplyFull", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmApplyFull", ItmApplyFull);
				if (ItmAttachment == null)
					cm.Parameters.AddWithValue("@ItmAttachment", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmAttachment", ItmAttachment);
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
				cm.CommandText = "ARPYDetItm_Delete";

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