using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
	/// <summary>
	/// Summary description for ARADJ.
	/// </summary>
	[Serializable]
    public class ARADJ : Document, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++

        internal int? _DocDeptKey;
        internal int? _DocTranGrpKey;
        internal int? _DocConKey;
        internal string _DocConNm;
        internal string _DocConUEN;
        internal int? _DocGrpKey;
        internal int? _DocJobKey;
        internal int? _DocJobPhaseKey;
        internal int? _DocJobTaskKey;
        internal int? _DocJobCostTypeKey;
        internal int? _DocAccGLKey;
        internal int? _DocAccARKey;
        internal int? _DocPayModeKey;
        internal DateTime? _DocChqDate;
        internal string _DocChqNum;
        internal int? _DocBankKey;
        internal decimal? _DocGrand;
        internal int? _DocCurrKey;
        internal decimal? _DocCurrRate;
        internal decimal? _DocHome;
        internal decimal? _DocApplyAmtF;
        internal decimal? _DocApplyAmtH;
        internal decimal? _DocCountryRate;
        internal bool _DocApplyFull;
        internal bool _DocDeposit;
        internal string _DocAccARID;
        internal string   _DocAccARDes;
        internal  string  _DocAccGLID;
        internal string   _DocAccGLDes;
        internal string _DocConID;
        internal SYSAttachments attachments= new SYSAttachments();

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public ARADJ()
            :base()
        {
            this._DocDeptKey = null;
            this._DocTranGrpKey = null;
            this._DocConKey = 0;
            this._DocConNm = string.Empty;
            this._DocConUEN = string.Empty;
            this._DocGrpKey = null;
            this._DocJobKey = null;
            this._DocJobPhaseKey = null;
            this._DocJobTaskKey = null;
            this._DocJobCostTypeKey = null;
            this._DocAccGLKey = null;
            this._DocAccARKey = null;
            this._DocPayModeKey = null;
            this._DocChqDate = null;
            this._DocChqNum = string.Empty;
            this._DocBankKey = null;
            this._DocGrand = null;
            this._DocCurrKey = null;
            this._DocCurrRate = null;
            this._DocHome = null;
            this._DocApplyAmtF = null;
            this._DocApplyAmtH = null;
            this._DocCountryRate = null;
            this._DocApplyFull = false;
            this._DocDeposit = false;
            this._DocAccARID = string.Empty;
            this._DocAccARDes = string.Empty;
            this._DocAccGLID = string.Empty;
            this._DocAccGLDes = string.Empty;
            this._DocConID = string.Empty;
            this._isDirty = false;
            base.PropertyChanged += new PropertyChangedEventHandler(ARADJ_PropertyChanged);
        }
        void ARADJ_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }

        public ARADJ Clone()
        {

            ARADJ objCopy = (ARADJ)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
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

        public int? DocKey
        {
            get
            {
                return this._DocKey;
            }
            set
            {
                this._DocKey = value;
                NotifyPropertyChanged("DocKey");
            }
        }

        public int? DocDeptKey
        {
            get
            {
                return this._DocDeptKey;
            }
            set
            {
                this._DocDeptKey = value;
                NotifyPropertyChanged("DocDeptKey");
            }
        }

        public int? DocTranGrpKey
        {
            get
            {
                return this._DocTranGrpKey;
            }
            set
            {
                this._DocTranGrpKey = value;
                NotifyPropertyChanged("DocTranGrpKey");
            }
        }

        public int? DocConKey
        {
            get
            {
                return this._DocConKey;
            }
            set
            {
                this._DocConKey = value;
                NotifyPropertyChanged("DocConKey");
            }
        }

        public string DocConNm
        {
            get
            {
                return this._DocConNm;
            }
            set
            {
                this._DocConNm = value;
                NotifyPropertyChanged("DocConNm");
            }
        }

        public string DocConUEN
        {
            get
            {
                return this._DocConUEN;
            }
            set
            {
                this._DocConUEN = value;
                NotifyPropertyChanged("DocConUEN");
            }
        }

        public int? DocGrpKey
        {
            get
            {
                return this._DocGrpKey;
            }
            set
            {
                this._DocGrpKey = value;
                NotifyPropertyChanged("DocGrpKey");
            }
        }

        public int? DocJobKey
        {
            get
            {
                return this._DocJobKey;
            }
            set
            {
                this._DocJobKey = value;
                NotifyPropertyChanged("DocJobKey");
            }
        }

        public int? DocJobPhaseKey
        {
            get
            {
                return this._DocJobPhaseKey;
            }
            set
            {
                this._DocJobPhaseKey = value;
                NotifyPropertyChanged("DocJobPhaseKey");
            }
        }

        public int? DocJobTaskKey
        {
            get
            {
                return this._DocJobTaskKey;
            }
            set
            {
                this._DocJobTaskKey = value;
                NotifyPropertyChanged("DocJobTaskKey");
            }
        }

        public int? DocJobCostTypeKey
        {
            get
            {
                return this._DocJobCostTypeKey;
            }
            set
            {
                this._DocJobCostTypeKey = value;
                NotifyPropertyChanged("DocJobCostTypeKey");
            }
        }

        public int? DocAccGLKey
        {
            get
            {
                return this._DocAccGLKey;
            }
            set
            {
                this._DocAccGLKey = value;
                NotifyPropertyChanged("DocAccGLKey");
            }
        }

        public int? DocAccARKey
        {
            get
            {
                return this._DocAccARKey;
            }
            set
            {
                this._DocAccARKey = value;
                NotifyPropertyChanged("DocAccARKey");
            }
        }

        public int? DocPayModeKey
        {
            get
            {
                return this._DocPayModeKey;
            }
            set
            {
                this._DocPayModeKey = value;
                NotifyPropertyChanged("DocPayModeKey");
            }
        }

        public DateTime? DocChqDate
        {
            get
            {
                return this._DocChqDate;
            }
            set
            {
                this._DocChqDate = value;
                NotifyPropertyChanged("DocChqDate");
            }
        }

        public string DocChqNum
        {
            get
            {
                return this._DocChqNum;
            }
            set
            {
                this._DocChqNum = value;
                NotifyPropertyChanged("DocChqNum");
            }
        }

        public int? DocBankKey
        {
            get
            {
                return this._DocBankKey;
            }
            set
            {
                this._DocBankKey = value;
                NotifyPropertyChanged("DocBankKey");
            }
        }

        public decimal? DocGrand
        {
            get
            {
                return this._DocGrand;
            }
            set
            {
                this._DocGrand = value;
                NotifyPropertyChanged("DocGrand");
            }
        }

        public int? DocCurrKey
        {
            get
            {
                return this._DocCurrKey;
            }
            set
            {
                this._DocCurrKey = value;
                NotifyPropertyChanged("DocCurrKey");
            }
        }

        public decimal? DocCurrRate
        {
            get
            {
                return this._DocCurrRate;
            }
            set
            {
                this._DocCurrRate = value;
                NotifyPropertyChanged("DocCurrRate");
            }
        }

        public decimal? DocHome
        {
            get
            {
                return this._DocHome;
            }
            set
            {
                this._DocHome = value;
                NotifyPropertyChanged("DocHome");
            }
        }

        public decimal? DocApplyAmtF
        {
            get
            {
                return this._DocApplyAmtF;
            }
            set
            {
                this._DocApplyAmtF = value;
                NotifyPropertyChanged("DocApplyAmtF");
            }
        }

        public decimal? DocApplyAmtH
        {
            get
            {
                return this._DocApplyAmtH;
            }
            set
            {
                this._DocApplyAmtH = value;
                NotifyPropertyChanged("DocApplyAmtH");
            }
        }

        public decimal? DocCountryRate
        {
            get
            {
                return this._DocCountryRate;
            }
            set
            {
                this._DocCountryRate = value;
                NotifyPropertyChanged("DocCountryRate");
            }
        }

        public bool DocApplyFull
        {
            get
            {
                return this._DocApplyFull;
            }
            set
            {
                this._DocApplyFull = value;
                NotifyPropertyChanged("DocApplyFull");
            }
        }

        public bool DocDeposit
        {
            get
            {
                return this._DocDeposit;
            }
            set
            {
                this._DocDeposit = value;
                NotifyPropertyChanged("DocDeposit");
            }
        }
        public string DocAccARID
        {
            get
            {
                return this._DocAccARID;
            }
            set
            {
                this._DocAccARID = value;
                NotifyPropertyChanged("DocAccARID");
            }
        }
        public string DocAccARDes
        {
            get
            {
                return this._DocAccARDes;
            }
            set
            {
                this._DocAccARDes = value;
                NotifyPropertyChanged("DocAccARDes");
            }
        }
        public string DocAccGLID
        {
            get
            {
                return this._DocAccGLID;
            }
            set
            {
                this._DocAccGLID = value;
                NotifyPropertyChanged("DocAccGLID");
            }
        }
        public string DocAccGLDes
        {
            get
            {
                return this._DocAccGLDes;
            }
            set
            {
                this._DocAccGLDes = value;
                NotifyPropertyChanged("DocAccGLDes");
            }
        }
        public string DocConID
        {
            get
            {
                return this._DocConID;
            }
            set
            {
                this._DocConID = value;
                NotifyPropertyChanged("DocConID");
            }
        }
        public SYSAttachments Attachments
        {
            get { return attachments; }
            set { attachments = value; }
        }

        #endregion

		#region Criteria
		[Serializable()]
		internal class Criteria
		{
			public int? _DocKey = null;
			public int? _option = null;
            public string _DocID = string.Empty;           
            public int? _CodeKey = null;

            internal Criteria(int? CodeKey, int? Option)
			{
                _CodeKey = CodeKey;
                _option = Option;
            }
            internal Criteria(int? CodeKey, int? DocKey, int? Option)
			{
                _CodeKey = CodeKey;
                _DocKey = DocKey;
                _option = Option;
			}
			internal Criteria(int? CodeKey, string DocID, int? Option)
			{
                _CodeKey = CodeKey;
                _DocID = DocID;                
				_option = Option;
			}
            internal Criteria(int? DocCodeKey, int? DocKey, string DocID, int? Option)
            {
                _CodeKey = DocCodeKey;
                _DocKey = DocKey;
                _DocID = DocID;
                _option = Option;

            }

		}
		#endregion //Criteria

        internal static ARADJ New(out string msgID)
        {
            msgID = MsgID.Common.NewFail;
            ARADJ child = new ARADJ();
            msgID = string.Empty;
            return child;
        }
		#region Data Access - Fetch

		internal bool Fetch(Criteria criteria)
		{
			bool retValue = false;
		    string msgID = "RecordGetFail";
			
			
			// Create new sql connection for this method. 
			using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
			{
				 // Open sql connection. 
				cn.Open();
				retValue = this.Fetch(cn, criteria);
			}
			
			
			return retValue;
		}
		internal bool Fetch(SqlConnection cn,Criteria criteria)
		{
			bool retValue = false;			
			using(SqlCommand cm = cn.CreateCommand())
			{
				cm.CommandType = CommandType.StoredProcedure;
				cm.CommandText = "ARADJ_Get";

				cm.Parameters.AddWithValue("@Option", criteria._option);
				cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
                cm.Parameters.AddWithValue("@DocID",criteria._DocID);
                cm.Parameters.AddWithValue("@DocCodeKey", criteria._CodeKey);
				
				cm.Parameters.AddWithValue("@RetValue", 0);
				cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
				// Using data reader as record set.
				using (IDataReader dr = cm.ExecuteReader())
				{
					//If data reader can read, continue...
                    if (dr.Read())
                    {
                        retValue = this.Fetch(dr);
                    }
                    else
                        this.Clear();
                   

				}// Already close and dispose data reader.
				
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;
			}// Already close and dispose sql connection.
			
			return retValue;
		}
		internal static ARADJ Get(IDataReader dr, out string msgID)
		{
			msgID = "RecordGetFail";
			ARADJ child = new ARADJ();
			child.Fetch(dr);
			return child;
		}

		internal bool Fetch(IDataReader dataReader)
		{
            _DocKey = dataReader["DocKey"] == DBNull.Value ? null : (int?)dataReader["DocKey"];
            _DocCodeKey = dataReader["DocCodeKey"] == DBNull.Value ? null : (int?)dataReader["DocCodeKey"];
            _DocID = dataReader["DocID"] == DBNull.Value ? string.Empty : dataReader["DocID"].ToString();
            _DocDate = dataReader["DocDate"] == DBNull.Value ? null : (DateTime?)dataReader["DocDate"];
            _DocType = dataReader["DocType"] == DBNull.Value ? null : (int?)dataReader["DocType"];
            _DocTypeNm = dataReader["DocTypeNm"] == DBNull.Value ? string.Empty : dataReader["DocTypeNm"].ToString();
            _DocSign = dataReader["DocSign"] == DBNull.Value ? (short)0 : (short?)dataReader["DocSign"];
            _DocDeptKey = dataReader["DocDeptKey"] == DBNull.Value ? null : (int?)dataReader["DocDeptKey"];
            _DocTranGrpKey = dataReader["DocTranGrpKey"] == DBNull.Value ? null : (int?)dataReader["DocTranGrpKey"];
            _DocConKey = dataReader["DocConKey"] == DBNull.Value ? null : (int?)dataReader["DocConKey"];
            _DocConNm = dataReader["DocConNm"] == DBNull.Value ? string.Empty : dataReader["DocConNm"].ToString();
            _DocConUEN = dataReader["DocConUEN"] == DBNull.Value ? string.Empty : dataReader["DocConUEN"].ToString();
			_DocGrpKey =dataReader["DocGrpKey"]==DBNull.Value ?null: (int?)dataReader["DocGrpKey"];
            _DocEmKey = dataReader["DocEmKey"] == DBNull.Value ? null : (int?)dataReader["DocEmKey"];
            _DocJobKey = dataReader["DocJobKey"] == DBNull.Value ? null : (int?)dataReader["DocJobKey"];
            _DocJobPhaseKey = dataReader["DocJobPhaseKey"] == DBNull.Value ? null : (int?)dataReader["DocJobPhaseKey"];
            _DocJobTaskKey = dataReader["DocJobTaskKey"] == DBNull.Value ? null : (int?)dataReader["DocJobTaskKey"]; ;
            _DocJobCostTypeKey = dataReader["DocJobCostTypeKey"] == DBNull.Value ? null : (int?)dataReader["DocJobCostTypeKey"];
            _DocAccGLKey = dataReader["DocAccGLKey"] == DBNull.Value ? null : (int?)dataReader["DocAccGLKey"];
            _DocAccARKey = dataReader["DocAccARKey"] == DBNull.Value ? null : (int?)dataReader["DocAccARKey"];
            _DocPayModeKey = dataReader["DocPayModeKey"] == DBNull.Value ? null : (int?)dataReader["DocPayModeKey"];
            _DocChqDate = dataReader["DocChqDate"] == DBNull.Value ? null : (DateTime?)dataReader["DocChqDate"];
            _DocChqNum = dataReader["DocChqNum"] == DBNull.Value ? string.Empty : dataReader["DocChqNum"].ToString();
            _DocBankKey = dataReader["DocBankKey"] == DBNull.Value ? null : (int?)dataReader["DocBankKey"];
            _DocRef = dataReader["DocRef"] == DBNull.Value ? string.Empty : dataReader["DocRef"].ToString();
            _DocDes = dataReader["DocDes"] == DBNull.Value ? string.Empty : dataReader["DocDes"].ToString();
            _DocRem = dataReader["DocRem"] == DBNull.Value ? string.Empty : dataReader["DocRem"].ToString();
            _DocGrand = dataReader["DocGrand"] == DBNull.Value ? null : (decimal?)dataReader["DocGrand"];
            _DocCurrKey = dataReader["DocCurrKey"] == DBNull.Value ? null : (int?)dataReader["DocCurrKey"];
            _DocCurrRate = dataReader["DocCurrRate"] == DBNull.Value ? null : (decimal?)dataReader["DocCurrRate"];
            _DocHome = dataReader["DocHome"] == DBNull.Value ? null : (decimal?)dataReader["DocHome"];
            _DocApplyAmtF = dataReader["DocApplyAmtF"] == DBNull.Value ? null : (decimal?)dataReader["DocApplyAmtF"];
            _DocApplyAmtH = dataReader["DocApplyAmtH"] == DBNull.Value ? null : (decimal?)dataReader["DocApplyAmtH"];
            _DocCountryRate = dataReader["DocCountryRate"] == DBNull.Value ? null : (decimal?)dataReader["DocCountryRate"];
			_DocApplyFull = Convert.ToBoolean(dataReader["DocApplyFull"]);
			_DocDeposit = Convert.ToBoolean(dataReader["DocDeposit"]);
            _DocStatus = dataReader["DocStatus"] == DBNull.Value ? string.Empty : dataReader["DocStatus"].ToString();
            _DocState = dataReader["DocState"] == DBNull.Value ? null : (int?)dataReader["DocState"];
            _DocPrinted = dataReader["DocPrinted"] == DBNull.Value ? false : (bool)dataReader["DocPrinted"];
            _ApproveUserKey = dataReader["ApproveUserKey"] == DBNull.Value ? null : (int?)dataReader["ApproveUserKey"];
            _ApproveDate = dataReader["ApproveDate"] == DBNull.Value ? null : (DateTime?)dataReader["ApproveDate"];
            _DisapproveUserKey = dataReader["DocAccGLKey"] == DBNull.Value ? null : (int?)dataReader["DocAccGLKey"];
			_DisapproveDate =dataReader["DisapproveDate"]==DBNull.Value ? null:(DateTime?)dataReader["DisapproveDate"];
            _DisapproveCount = dataReader["DisapproveCount"] == DBNull.Value ? (short)0 : (short?)dataReader["DisapproveCount"];
            _DisapproveMsg = dataReader["DisapproveMsg"] == DBNull.Value ? string.Empty : dataReader["DisapproveMsg"].ToString();
            _Attachment = dataReader["Attachment"] == DBNull.Value ? false : (bool)dataReader["Attachment"];
            _BranchKey = dataReader["BranchKey"] == DBNull.Value ? null : (int?)dataReader["BranchKey"];
            _CreateDate = dataReader["CreateDate"] == DBNull.Value ? null : (DateTime?)dataReader["CreateDate"];
            _CreateUserKey = dataReader["CreateUserKey"] == DBNull.Value ? null : (int?)dataReader["CreateUserKey"];
            _LastModifiedDate = dataReader["LastModifiedDate"] == DBNull.Value ? null : (DateTime?)dataReader["LastModifiedDate"];
            _LastModifiedUserKey = dataReader["LastModifiedUserKey"] == DBNull.Value ? null : (int?)dataReader["LastModifiedUserKey"];
            _PurgeKeep = dataReader["PurgeKeep"] == DBNull.Value ? null : (int?)dataReader["PurgeKeep"];
            _PurgeData = dataReader["PurgeData"] == DBNull.Value ? false : (bool)dataReader["PurgeData"];
            _Custom1 = dataReader["Custom1"] == DBNull.Value ? string.Empty : dataReader["Custom1"].ToString();
            _Custom2 = dataReader["Custom2"] == DBNull.Value ? string.Empty : dataReader["Custom2"].ToString();
            _Custom3 = dataReader["Custom3"] == DBNull.Value ? string.Empty : dataReader["Custom3"].ToString();
            _Custom4 = dataReader["Custom4"] == DBNull.Value ? string.Empty : dataReader["Custom4"].ToString();
            _Custom5 = dataReader["Custom5"] == DBNull.Value ? string.Empty : dataReader["Custom5"].ToString();
            _DocAccARID = dataReader["DocAccARID"] == DBNull.Value ? string.Empty : dataReader["DocAccARID"].ToString();
            _DocAccARDes = dataReader["DocAccARDes"] == DBNull.Value ? string.Empty : dataReader["DocAccARDes"].ToString();
            _DocAccGLID = dataReader["DocAccGLID"] == DBNull.Value ? string.Empty : dataReader["DocAccGLID"].ToString();
            _DocAccGLDes = dataReader["DocAccGLDes"] == DBNull.Value ? string.Empty : dataReader["DocAccGLDes"].ToString();
            
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
				cm.CommandText = "ARADJ_AddUpdate";

				cm.Parameters.AddWithValue("@Option", 0);
				cm.Parameters.AddWithValue("@MsgID", msgID);
				cm.Parameters.AddWithValue("@NewDocKey", DocKey);
				if (_DocKey == null)
					cm.Parameters.AddWithValue("@DocKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocKey", _DocKey);
				if (_DocCodeKey == null)
					cm.Parameters.AddWithValue("@DocCodeKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocCodeKey", _DocCodeKey);
				if (_DocID == null)
					cm.Parameters.AddWithValue("@DocID", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocID", _DocID);
				if (_DocDate == null)
					cm.Parameters.AddWithValue("@DocDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocDate", _DocDate);
				if (_DocType == null)
					cm.Parameters.AddWithValue("@DocType", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocType", _DocType);
				if (_DocTypeNm == null)
					cm.Parameters.AddWithValue("@DocTypeNm", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocTypeNm", _DocTypeNm);
				if (_DocSign == null)
					cm.Parameters.AddWithValue("@DocSign", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocSign", _DocSign);
				if (_DocDeptKey == null)
					cm.Parameters.AddWithValue("@DocDeptKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocDeptKey", _DocDeptKey);
				if (_DocTranGrpKey == null)
					cm.Parameters.AddWithValue("@DocTranGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocTranGrpKey", _DocTranGrpKey);
				if (_DocConKey == null)
					cm.Parameters.AddWithValue("@DocConKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocConKey", _DocConKey);
				if (_DocConNm == null)
					cm.Parameters.AddWithValue("@DocConNm", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocConNm", _DocConNm);
				if (_DocConUEN == null)
					cm.Parameters.AddWithValue("@DocConUEN", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocConUEN", _DocConUEN);
				if (_DocGrpKey == null)
					cm.Parameters.AddWithValue("@DocGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocGrpKey", _DocGrpKey);
				if (_DocEmKey == null)
					cm.Parameters.AddWithValue("@DocEmKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocEmKey", _DocEmKey);
				if (_DocJobKey == null)
					cm.Parameters.AddWithValue("@DocJobKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocJobKey", _DocJobKey);
				if (_DocJobPhaseKey == null)
					cm.Parameters.AddWithValue("@DocJobPhaseKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocJobPhaseKey", _DocJobPhaseKey);
				if (_DocJobTaskKey == null)
					cm.Parameters.AddWithValue("@DocJobTaskKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocJobTaskKey", _DocJobTaskKey);
				if (_DocJobCostTypeKey == null)
					cm.Parameters.AddWithValue("@DocJobCostTypeKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocJobCostTypeKey", _DocJobCostTypeKey);
				if (_DocAccGLKey == null)
					cm.Parameters.AddWithValue("@DocAccGLKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocAccGLKey", _DocAccGLKey);
				if (_DocAccARKey == null)
					cm.Parameters.AddWithValue("@DocAccARKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocAccARKey", _DocAccARKey);
				if (_DocPayModeKey == null)
					cm.Parameters.AddWithValue("@DocPayModeKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocPayModeKey", _DocPayModeKey);
				if (_DocChqDate == null)
					cm.Parameters.AddWithValue("@DocChqDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocChqDate", _DocChqDate);
				if (_DocChqNum == null)
					cm.Parameters.AddWithValue("@DocChqNum", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocChqNum", _DocChqNum);
				if (_DocBankKey == null)
					cm.Parameters.AddWithValue("@DocBankKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocBankKey", _DocBankKey);
				if (_DocRef == null)
					cm.Parameters.AddWithValue("@DocRef", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocRef", _DocRef);
				if (_DocDes == null)
					cm.Parameters.AddWithValue("@DocDes", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocDes", _DocDes);
				if (_DocRem == null)
					cm.Parameters.AddWithValue("@DocRem", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocRem", _DocRem);
				if (_DocGrand == null)
					cm.Parameters.AddWithValue("@DocGrand", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocGrand", _DocGrand);
				if (_DocCurrKey == null)
					cm.Parameters.AddWithValue("@DocCurrKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocCurrKey", _DocCurrKey);
				if (_DocCurrRate == null)
					cm.Parameters.AddWithValue("@DocCurrRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocCurrRate", _DocCurrRate);
				if (_DocHome == null)
					cm.Parameters.AddWithValue("@DocHome", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocHome", _DocHome);
				if (_DocApplyAmtF == null)
					cm.Parameters.AddWithValue("@DocApplyAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocApplyAmtF", _DocApplyAmtF);
				if (_DocApplyAmtH == null)
					cm.Parameters.AddWithValue("@DocApplyAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocApplyAmtH", _DocApplyAmtH);
				if (_DocCountryRate == null)
					cm.Parameters.AddWithValue("@DocCountryRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocCountryRate", _DocCountryRate);
				if (_DocApplyFull == null)
					cm.Parameters.AddWithValue("@DocApplyFull", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocApplyFull", _DocApplyFull);
				if (_DocDeposit == null)
					cm.Parameters.AddWithValue("@DocDeposit", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocDeposit", _DocDeposit);
				if (_DocStatus == null)
					cm.Parameters.AddWithValue("@DocStatus", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocStatus", _DocStatus);
				if (_DocState == null)
					cm.Parameters.AddWithValue("@DocState", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocState", _DocState);
				if (_DocPrinted == null)
					cm.Parameters.AddWithValue("@DocPrinted", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocPrinted", _DocPrinted);
				if (_ApproveUserKey == null)
					cm.Parameters.AddWithValue("@ApproveUserKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ApproveUserKey", _ApproveUserKey);
				if (_ApproveDate == null)
					cm.Parameters.AddWithValue("@ApproveDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ApproveDate", _ApproveDate);
				if (_DisapproveUserKey == null)
					cm.Parameters.AddWithValue("@DisapproveUserKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DisapproveUserKey", _DisapproveUserKey);
				if (_DisapproveDate == null)
					cm.Parameters.AddWithValue("@DisapproveDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DisapproveDate", _DisapproveDate);
				if (_DisapproveCount == null)
					cm.Parameters.AddWithValue("@DisapproveCount", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DisapproveCount", _DisapproveCount);
				if (_DisapproveMsg == null)
					cm.Parameters.AddWithValue("@DisapproveMsg", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DisapproveMsg", _DisapproveMsg);
				if (_Attachment == null)
					cm.Parameters.AddWithValue("@Attachment", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Attachment", _Attachment);
				if (_BranchKey == null)
					cm.Parameters.AddWithValue("@BranchKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@BranchKey", _BranchKey);
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
				if (_PurgeKeep == null)
					cm.Parameters.AddWithValue("@PurgeKeep", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@PurgeKeep", _PurgeKeep);
				if (_PurgeData == null)
					cm.Parameters.AddWithValue("@PurgeData", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@PurgeData", _PurgeData);
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
				if (_Custom4 == null)
					cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Custom4", _Custom4);
				if (_Custom5 == null)
					cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Custom5", _Custom5);
				
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
				cm.CommandText = "ARADJ_AddUpdate";

				cm.Parameters.AddWithValue("@Option", 1);
				cm.Parameters.AddWithValue("@MsgID", msgID);
				cm.Parameters.AddWithValue("@NewDocKey", 0);
				if (_DocKey == null)
					cm.Parameters.AddWithValue("@DocKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocKey", _DocKey);
				if (_DocCodeKey == null)
					cm.Parameters.AddWithValue("@DocCodeKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocCodeKey", _DocCodeKey);
				if (_DocID == null)
					cm.Parameters.AddWithValue("@DocID", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocID", _DocID);
				if (_DocDate == null)
					cm.Parameters.AddWithValue("@DocDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocDate", _DocDate);
				if (_DocType == null)
					cm.Parameters.AddWithValue("@DocType", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocType", _DocType);
				if (_DocTypeNm == null)
					cm.Parameters.AddWithValue("@DocTypeNm", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocTypeNm", _DocTypeNm);
				if (_DocSign == null)
					cm.Parameters.AddWithValue("@DocSign", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocSign", _DocSign);
				if (_DocDeptKey == null)
					cm.Parameters.AddWithValue("@DocDeptKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocDeptKey", _DocDeptKey);
				if (_DocTranGrpKey == null)
					cm.Parameters.AddWithValue("@DocTranGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocTranGrpKey", _DocTranGrpKey);
				if (_DocConKey == null)
					cm.Parameters.AddWithValue("@DocConKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocConKey", _DocConKey);
				if (_DocConNm == null)
					cm.Parameters.AddWithValue("@DocConNm", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocConNm", _DocConNm);
				if (_DocConUEN == null)
					cm.Parameters.AddWithValue("@DocConUEN", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocConUEN", _DocConUEN);
				if (_DocGrpKey == null)
					cm.Parameters.AddWithValue("@DocGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocGrpKey", _DocGrpKey);
				if (_DocEmKey == null)
					cm.Parameters.AddWithValue("@DocEmKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocEmKey", _DocEmKey);
				if (_DocJobKey == null)
					cm.Parameters.AddWithValue("@DocJobKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocJobKey", _DocJobKey);
				if (_DocJobPhaseKey == null)
					cm.Parameters.AddWithValue("@DocJobPhaseKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocJobPhaseKey", _DocJobPhaseKey);
				if (_DocJobTaskKey == null)
					cm.Parameters.AddWithValue("@DocJobTaskKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocJobTaskKey", _DocJobTaskKey);
				if (_DocJobCostTypeKey == null)
					cm.Parameters.AddWithValue("@DocJobCostTypeKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocJobCostTypeKey", _DocJobCostTypeKey);
				if (_DocAccGLKey == null)
					cm.Parameters.AddWithValue("@DocAccGLKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocAccGLKey", _DocAccGLKey);
				if (_DocAccARKey == null)
					cm.Parameters.AddWithValue("@DocAccARKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocAccARKey", _DocAccARKey);
				if (_DocPayModeKey == null)
					cm.Parameters.AddWithValue("@DocPayModeKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocPayModeKey", _DocPayModeKey);
				if (_DocChqDate == null)
					cm.Parameters.AddWithValue("@DocChqDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocChqDate", _DocChqDate);
				if (_DocChqNum == null)
					cm.Parameters.AddWithValue("@DocChqNum", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocChqNum", _DocChqNum);
				if (_DocBankKey == null)
					cm.Parameters.AddWithValue("@DocBankKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocBankKey", _DocBankKey);
				if (_DocRef == null)
					cm.Parameters.AddWithValue("@DocRef", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocRef", _DocRef);
				if (_DocDes == null)
					cm.Parameters.AddWithValue("@DocDes", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocDes", _DocDes);
				if (_DocRem == null)
					cm.Parameters.AddWithValue("@DocRem", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocRem", _DocRem);
				if (_DocGrand == null)
					cm.Parameters.AddWithValue("@DocGrand", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocGrand", _DocGrand);
				if (_DocCurrKey == null)
					cm.Parameters.AddWithValue("@DocCurrKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocCurrKey", _DocCurrKey);
				if (_DocCurrRate == null)
					cm.Parameters.AddWithValue("@DocCurrRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocCurrRate", _DocCurrRate);
				if (_DocHome == null)
					cm.Parameters.AddWithValue("@DocHome", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocHome", _DocHome);
				if (_DocApplyAmtF == null)
					cm.Parameters.AddWithValue("@DocApplyAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocApplyAmtF", _DocApplyAmtF);
				if (_DocApplyAmtH == null)
					cm.Parameters.AddWithValue("@DocApplyAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocApplyAmtH", _DocApplyAmtH);
				if (_DocCountryRate == null)
					cm.Parameters.AddWithValue("@DocCountryRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocCountryRate", _DocCountryRate);
				if (_DocApplyFull == null)
					cm.Parameters.AddWithValue("@DocApplyFull", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocApplyFull", _DocApplyFull);
				if (_DocDeposit == null)
					cm.Parameters.AddWithValue("@DocDeposit", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocDeposit", _DocDeposit);
				if (_DocStatus == null)
					cm.Parameters.AddWithValue("@DocStatus", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocStatus", _DocStatus);
				if (_DocState == null)
					cm.Parameters.AddWithValue("@DocState", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocState", _DocState);
				if (_DocPrinted == null)
					cm.Parameters.AddWithValue("@DocPrinted", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DocPrinted", _DocPrinted);
				if (_ApproveUserKey == null)
					cm.Parameters.AddWithValue("@ApproveUserKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ApproveUserKey", _ApproveUserKey);
				if (_ApproveDate == null)
					cm.Parameters.AddWithValue("@ApproveDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ApproveDate", _ApproveDate);
				if (_DisapproveUserKey == null)
					cm.Parameters.AddWithValue("@DisapproveUserKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DisapproveUserKey", _DisapproveUserKey);
				if (_DisapproveDate == null)
					cm.Parameters.AddWithValue("@DisapproveDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DisapproveDate", _DisapproveDate);
				if (_DisapproveCount == null)
					cm.Parameters.AddWithValue("@DisapproveCount", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DisapproveCount", _DisapproveCount);
				if (_DisapproveMsg == null)
					cm.Parameters.AddWithValue("@DisapproveMsg", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@DisapproveMsg", _DisapproveMsg);
				if (_Attachment == null)
					cm.Parameters.AddWithValue("@Attachment", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Attachment", _Attachment);
				if (_BranchKey == null)
					cm.Parameters.AddWithValue("@BranchKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@BranchKey", _BranchKey);
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
				if (_PurgeKeep == null)
					cm.Parameters.AddWithValue("@PurgeKeep", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@PurgeKeep", _PurgeKeep);
				if (_PurgeData == null)
					cm.Parameters.AddWithValue("@PurgeData", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@PurgeData", _PurgeData);
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
				if (_Custom4 == null)
					cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Custom4", _Custom4);
				if (_Custom5 == null)
					cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@Custom5", _Custom5);
				
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
				cm.CommandText = "ARADJ_Delete";

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
		#region Data Access - Validation

		internal bool Validation(Criteria criteria, bool isNew)
		{
			bool retValue = false;		
			
            //using (TransactionScope scope = new TransactionScope())
            //{
				using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
				{
					cn.Open();
					retValue = Validation(cn, criteria,isNew);
				}
            //      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            //}
			
			return retValue;
		}
		internal bool Validation(SqlConnection cn, Criteria criteria,bool isNew)
		{
			using(SqlCommand cm = cn.CreateCommand())
			{
				cm.CommandType = CommandType.StoredProcedure;
				cm.CommandText = "ARADJ_Validation";

                cm.Parameters.AddWithValue("@isNew", isNew);
                cm.Parameters.AddWithValue("@DocCodeKey", criteria._CodeKey);
                cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
                cm.Parameters.AddWithValue("@DocID", criteria._DocID);                    
                cm.Parameters.AddWithValue("@RetValue", 0);
                
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
				cm.ExecuteNonQuery();
			
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
			}
		}
		#endregion Validation

        private void Clear()
        {
            this._DocKey = 0;
            this._DocDeptKey = null;
            this._DocTranGrpKey = null;
            this._DocConKey = 0;
            this._DocConNm = string.Empty;
            this._DocConUEN = string.Empty;
            this._DocGrpKey = null;
            this._DocJobKey = null;
            this._DocJobPhaseKey = null;
            this._DocJobTaskKey = null;
            this._DocJobCostTypeKey = null;
            this._DocAccGLKey = null;
            this._DocAccARKey = null;
            this._DocPayModeKey = null;
            this._DocChqDate = null;
            this._DocChqNum = string.Empty;
            this._DocBankKey = null;
            this._DocGrand = null;
            this._DocCurrKey = null;
            this._DocCurrRate = null;
            this._DocHome = null;
            this._DocApplyAmtF = null;
            this._DocApplyAmtH = null;
            this._DocCountryRate = null;
            this._DocApplyFull = false;
            this._DocDeposit = false;
            this._DocAccARID = string.Empty;
            this._DocAccARDes = string.Empty;
            this._DocAccGLID = string.Empty;
            this._DocAccGLDes = string.Empty;
            this._DocConID = string.Empty;


            this._isDirty = false;
        }
    
    }
}