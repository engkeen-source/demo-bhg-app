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
    /// Summary description for CSCPS.
    /// </summary>
    [Serializable]
    public class CSCPS : Document, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        internal int _DocConKey;
        internal string _DocConNm;
        internal int _DocPriceAssign;
        internal decimal _DocDisPercent;
        internal decimal _DocGrand;
        internal int _DocCurrKey;
        internal decimal _DocCurrRate;
        internal decimal _DocSales;
        internal decimal _DocSalesAfterDis;
        internal decimal _DocExpense;
        internal decimal _DocCountryRate;
        internal string _DocConID;
        internal SYSAttachments attachments= new SYSAttachments();


        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public CSCPS()
            : base()
        {
            this._DocConKey = 0;
            this._DocConNm = string.Empty;
            this._DocPriceAssign = 0;
            this._DocDisPercent = 0;
            this._DocGrand = 0;
            this._DocCurrKey = 0;
            this._DocCurrRate = 0;
            this._DocSales = 0;
            this._DocSalesAfterDis = 0;
            this._DocExpense = 0;
            this._DocCountryRate = 0;
            this._DocConID = string.Empty;
            base.PropertyChanged += new PropertyChangedEventHandler(CSCPS_PropertyChanged);
        }
        void CSCPS_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }

        public CSCPS Clone()
        {
            CSCPS objCopy = (CSCPS)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
        }
        public static CSCPS Get(int? docKey)
        {
            CSCPS child = new CSCPS();
            child.Fetch(new Criteria(docKey, 1));
            return child;
        }

        public static CSCPS New()
        {
            CSCPS child = new CSCPS();
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

        internal void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


        public int DocConKey
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
        public int DocPriceAssign
        {

            get
            {
                return this._DocPriceAssign;
            }
            set
            {
                this._DocPriceAssign = value;
                NotifyPropertyChanged("DocPriceAssign");
            }
        }
        public decimal DocDisPercent
        {

            get
            {
                return this._DocDisPercent;
            }
            set
            {
                this._DocDisPercent = value;
                NotifyPropertyChanged("DocDisPercent");
            }
        }
        public decimal DocGrand
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
        public int DocCurrKey
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
        public decimal DocCurrRate
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
        public decimal DocSales
        {

            get
            {
                return this._DocSales;
            }
            set
            {
                this._DocSales = value;
                NotifyPropertyChanged("DocSales");
            }
        }
        public decimal DocSalesAfterDis
        {

            get
            {
                return this._DocSalesAfterDis;
            }
            set
            {
                this._DocSalesAfterDis = value;
                NotifyPropertyChanged("DocSalesAfterDis");
            }
        }
        public decimal DocExpense
        {

            get
            {
                return this._DocExpense;
            }
            set
            {
                this._DocExpense = value;
                NotifyPropertyChanged("DocExpense");
            }
        }
        public decimal DocCountryRate
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
            public int? _DocCodeKey = null;
            public int? _DocKey = null;
            public int? _option = null;
            public string _DocID = string.Empty;

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
            internal Criteria(int DocCodeKey, int? DocKey, int? Option)
            {
                _DocCodeKey = DocCodeKey;
                _DocKey = DocKey;
                _option = Option;
            }
            internal Criteria(int? DocCodeKey, int? DocKey, string DocID, int? Option)
            {
                _DocCodeKey = DocCodeKey;
                _DocKey = DocKey;
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
                cm.CommandText = "CSCPS_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
                cm.Parameters.AddWithValue("@DocID", criteria._DocID);
                cm.Parameters.AddWithValue("@DocCodeKey", criteria._DocCodeKey);
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
        internal static CSCPS Get(IDataReader dr)
        {
            CSCPS child = new CSCPS();
            child.Fetch(dr);
            return child;
        }
        internal static CSCPS Get(SqlConnection cn, Criteria criteria)
        {
            CSCPS child = new CSCPS();
            child.Fetch(cn, criteria);
            return child;
        }

        internal bool Fetch(IDataReader dataReader)
        {
            _DocKey = dataReader["DocKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocKey"];
            _DocCodeKey = dataReader["DocCodeKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocCodeKey"];
            _DocID = dataReader["DocID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocID"];
            _DocDate = dataReader["DocDate"] == DBNull.Value ? (DateTime)DateTime.Today.Date : (DateTime)dataReader["DocDate"];
            _DocType = dataReader["DocType"] == DBNull.Value ? (int)0 : (int)dataReader["DocType"];
            _DocTypeNm = dataReader["DocTypeNm"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocTypeNm"];
            _DocSign = dataReader["DocSign"] == DBNull.Value ? (Int16)0 : (Int16)dataReader["DocSign"];
            _DocConKey = dataReader["DocConKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocConKey"];
            _DocConNm = dataReader["DocConNm"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocConNm"];
            _DocEmKey = dataReader["DocEmKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocEmKey"];
            _DocPriceAssign = dataReader["DocPriceAssign"] == DBNull.Value ? (int)0 : (int)dataReader["DocPriceAssign"];
            _DocRef = dataReader["DocRef"] == DBNull.Value ? (string)null : (string)dataReader["DocRef"];
            _DocDes = dataReader["DocDes"] == DBNull.Value ? (string)null : (string)dataReader["DocDes"];
            _DocRem = dataReader["DocRem"] == DBNull.Value ? (string)null : (string)dataReader["DocRem"];
            _DocDisPercent = dataReader["DocDisPercent"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocDisPercent"];
            _DocGrand = dataReader["DocGrand"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocGrand"];
            _DocCurrKey = dataReader["DocCurrKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocCurrKey"];
            _DocCurrRate = dataReader["DocCurrRate"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocCurrRate"];
            _DocSales = dataReader["DocSales"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocSales"];
            _DocSalesAfterDis = dataReader["DocSalesAfterDis"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocSalesAfterDis"];
            _DocExpense = dataReader["DocExpense"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocExpense"];
            _DocCountryRate = dataReader["DocCountryRate"] == DBNull.Value ? (decimal)0 : (decimal)dataReader["DocCountryRate"];
            _DocStatus = dataReader["DocStatus"] == DBNull.Value ? (string)null : (string)dataReader["DocStatus"];
            _DocState = dataReader["DocState"] == DBNull.Value ? (int)0 : (int)dataReader["DocState"];
            _DocPrinted = dataReader["DocPrinted"] == DBNull.Value ? (bool)false : (bool)dataReader["DocPrinted"];
            _ApproveUserKey = dataReader["ApproveUserKey"] == DBNull.Value ? (int)0 : (int)dataReader["ApproveUserKey"];
            _ApproveDate = dataReader["ApproveDate"] == DBNull.Value ? null : (DateTime?)dataReader["ApproveDate"];
            _DisapproveUserKey = dataReader["DisapproveUserKey"] == DBNull.Value ? (int)0 : (int)dataReader["DisapproveUserKey"];
            _DisapproveDate = dataReader["DisapproveDate"] == DBNull.Value ? null : (DateTime?)dataReader["DisapproveDate"];
            _DisapproveCount = dataReader["DisapproveCount"] == DBNull.Value ? (Int16)0 : (Int16)dataReader["DisapproveCount"];
            _DisapproveMsg = dataReader["DisapproveMsg"] == DBNull.Value ? (string)null : (string)dataReader["DisapproveMsg"];
            _Attachment = dataReader["Attachment"] == DBNull.Value ? (bool)false : (bool)dataReader["Attachment"];
            _BranchKey = dataReader["BranchKey"] == DBNull.Value ? (int)0 : (int)dataReader["BranchKey"];
            _CreateDate = dataReader["CreateDate"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["CreateDate"];
            _CreateUserKey = dataReader["CreateUserKey"] == DBNull.Value ? (int?)null : (int?)dataReader["CreateUserKey"];
            _LastModifiedDate = dataReader["LastModifiedDate"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["LastModifiedDate"];
            _LastModifiedUserKey = dataReader["LastModifiedUserKey"] == DBNull.Value ? (int?)null : (int?)dataReader["LastModifiedUserKey"];
            _PurgeKeep = dataReader["PurgeKeep"] == DBNull.Value ? (int)0 : (int)dataReader["PurgeKeep"];
            _PurgeData = dataReader["PurgeData"] == DBNull.Value ? (bool)false : (bool)dataReader["PurgeData"];
            _Custom1 = dataReader["Custom1"] == DBNull.Value ? (string)null : (string)dataReader["Custom1"];
            _Custom2 = dataReader["Custom2"] == DBNull.Value ? (string)null : (string)dataReader["Custom2"];
            _Custom3 = dataReader["Custom3"] == DBNull.Value ? (string)null : (string)dataReader["Custom3"];
            _Custom4 = dataReader["Custom4"] == DBNull.Value ? (string)null : (string)dataReader["Custom4"];
            _Custom5 = dataReader["Custom5"] == DBNull.Value ? (string)null : (string)dataReader["Custom5"];
            _DocConID = dataReader["DocConID"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DocConID"];
            _DefBAddrKey = dataReader["DefBAddrKey"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DefBAddrKey"];
            _DefSAddrKey = dataReader["DefSAddrKey"] == DBNull.Value ? (string)string.Empty : (string)dataReader["DefSAddrKey"];
            _DefLocKey = dataReader["DefLocKey"] == DBNull.Value ? (int?)0 : (int?)dataReader["DefLocKey"];

            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert()
        {
            bool retValue = false;
            DocKey = null;
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
            string msgID = "RecordAddFail";
            DocKey = 0;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "CSCPS_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                if (_DocKey == null)
                {
                    cm.Parameters.AddWithValue("@DocKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocKey", _DocKey);
                }
                if (_DocCodeKey == null)
                {
                    cm.Parameters.AddWithValue("@DocCodeKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCodeKey", _DocCodeKey);
                }
                if (_DocID == null)
                {
                    cm.Parameters.AddWithValue("@DocID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocID", _DocID);
                }
                if (_DocDate == null)
                {
                    cm.Parameters.AddWithValue("@DocDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDate", _DocDate);
                }
                if (_DocType == null)
                {
                    cm.Parameters.AddWithValue("@DocType", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocType", _DocType);
                }
                if (_DocTypeNm == null)
                {
                    cm.Parameters.AddWithValue("@DocTypeNm", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTypeNm", _DocTypeNm);
                }
                if (_DocSign == null)
                {
                    cm.Parameters.AddWithValue("@DocSign", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSign", _DocSign);
                }
                if (_DocConKey == null)
                {
                    cm.Parameters.AddWithValue("@DocConKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocConKey", _DocConKey);
                }
                if (_DocConNm == null)
                {
                    cm.Parameters.AddWithValue("@DocConNm", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocConNm", _DocConNm);
                }
                if (_DocEmKey == null)
                {
                    cm.Parameters.AddWithValue("@DocEmKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocEmKey", _DocEmKey);
                }
                if (_DocPriceAssign == null)
                {
                    cm.Parameters.AddWithValue("@DocPriceAssign", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPriceAssign", _DocPriceAssign);
                }
                if (_DocRef == null)
                {
                    cm.Parameters.AddWithValue("@DocRef", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRef", _DocRef);
                }
                if (_DocDes == null)
                {
                    cm.Parameters.AddWithValue("@DocDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDes", _DocDes);
                }
                if (_DocRem == null)
                {
                    cm.Parameters.AddWithValue("@DocRem", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRem", _DocRem);
                }
                if (_DocDisPercent == null)
                {
                    cm.Parameters.AddWithValue("@DocDisPercent", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDisPercent", _DocDisPercent);
                }
                if (_DocGrand == null)
                {
                    cm.Parameters.AddWithValue("@DocGrand", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocGrand", _DocGrand);
                }
                if (_DocCurrKey == null)
                {
                    cm.Parameters.AddWithValue("@DocCurrKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCurrKey", _DocCurrKey);
                }
                if (_DocCurrRate == null)
                {
                    cm.Parameters.AddWithValue("@DocCurrRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCurrRate", _DocCurrRate);
                }
                if (_DocSales == null)
                {
                    cm.Parameters.AddWithValue("@DocSales", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSales", _DocSales);
                }
                if (_DocSalesAfterDis == null)
                {
                    cm.Parameters.AddWithValue("@DocSalesAfterDis", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSalesAfterDis", _DocSalesAfterDis);
                }
                if (_DocExpense == null)
                {
                    cm.Parameters.AddWithValue("@DocExpense", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocExpense", _DocExpense);
                }
                if (_DocCountryRate == null)
                {
                    cm.Parameters.AddWithValue("@DocCountryRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCountryRate", _DocCountryRate);
                }
                if (_DocStatus == null)
                {
                    cm.Parameters.AddWithValue("@DocStatus", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocStatus", _DocStatus);
                }
                if (_DocState == null)
                {
                    cm.Parameters.AddWithValue("@DocState", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocState", _DocState);
                }
                if (_DocPrinted == null)
                {
                    cm.Parameters.AddWithValue("@DocPrinted", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPrinted", _DocPrinted);
                }
                if (_ApproveUserKey == null)
                {
                    cm.Parameters.AddWithValue("@ApproveUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ApproveUserKey", _ApproveUserKey);
                }
                if (_ApproveDate == null)
                {
                    cm.Parameters.AddWithValue("@ApproveDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ApproveDate", _ApproveDate);
                }
                if (_DisapproveUserKey == null)
                {
                    cm.Parameters.AddWithValue("@DisapproveUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DisapproveUserKey", _DisapproveUserKey);
                }
                if (_DisapproveDate == null)
                {
                    cm.Parameters.AddWithValue("@DisapproveDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DisapproveDate", _DisapproveDate);
                }
                if (_DisapproveCount == null)
                {
                    cm.Parameters.AddWithValue("@DisapproveCount", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DisapproveCount", _DisapproveCount);
                }
                if (_DisapproveMsg == null)
                {
                    cm.Parameters.AddWithValue("@DisapproveMsg", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DisapproveMsg", _DisapproveMsg);
                }
                if (_Attachment == null)
                {
                    cm.Parameters.AddWithValue("@Attachment", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Attachment", _Attachment);
                }
                if (_BranchKey == null)
                {
                    cm.Parameters.AddWithValue("@BranchKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@BranchKey", _BranchKey);
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
                if (_PurgeKeep == null)
                {
                    cm.Parameters.AddWithValue("@PurgeKeep", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@PurgeKeep", _PurgeKeep);
                }
                if (_PurgeData == null)
                {
                    cm.Parameters.AddWithValue("@PurgeData", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@PurgeData", _PurgeData);
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
                if (_DocConID == null)
                {
                    cm.Parameters.AddWithValue("@DocConID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocConID", _DocConID);
                }
                if (_DefBAddrKey == null)
                {
                    cm.Parameters.AddWithValue("@DefBAddrKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DefBAddrKey", _DefBAddrKey);
                }
                if (_DefSAddrKey == null)
                {
                    cm.Parameters.AddWithValue("@DefSAddrKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DefSAddrKey", _DefSAddrKey);
                }
                if (_DefLocKey == null)
                {
                    cm.Parameters.AddWithValue("@DefLocKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DefLocKey", _DefLocKey);
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
                cm.CommandText = "CSCPS_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@MsgID", msgID);

                if (_DocKey == null)
                {
                    cm.Parameters.AddWithValue("@DocKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocKey", _DocKey);
                }
                if (_DocCodeKey == null)
                {
                    cm.Parameters.AddWithValue("@DocCodeKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCodeKey", _DocCodeKey);
                }
                if (_DocID == null)
                {
                    cm.Parameters.AddWithValue("@DocID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocID", _DocID);
                }
                if (_DocDate == null)
                {
                    cm.Parameters.AddWithValue("@DocDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDate", _DocDate);
                }
                if (_DocType == null)
                {
                    cm.Parameters.AddWithValue("@DocType", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocType", _DocType);
                }
                if (_DocTypeNm == null)
                {
                    cm.Parameters.AddWithValue("@DocTypeNm", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTypeNm", _DocTypeNm);
                }
                if (_DocSign == null)
                {
                    cm.Parameters.AddWithValue("@DocSign", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSign", _DocSign);
                }
                if (_DocConKey == null)
                {
                    cm.Parameters.AddWithValue("@DocConKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocConKey", _DocConKey);
                }
                if (_DocConNm == null)
                {
                    cm.Parameters.AddWithValue("@DocConNm", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocConNm", _DocConNm);
                }
                if (_DocEmKey == null)
                {
                    cm.Parameters.AddWithValue("@DocEmKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocEmKey", _DocEmKey);
                }
                if (_DocPriceAssign == null)
                {
                    cm.Parameters.AddWithValue("@DocPriceAssign", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPriceAssign", _DocPriceAssign);
                }
                if (_DocRef == null)
                {
                    cm.Parameters.AddWithValue("@DocRef", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRef", _DocRef);
                }
                if (_DocDes == null)
                {
                    cm.Parameters.AddWithValue("@DocDes", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDes", _DocDes);
                }
                if (_DocRem == null)
                {
                    cm.Parameters.AddWithValue("@DocRem", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocRem", _DocRem);
                }
                if (_DocDisPercent == null)
                {
                    cm.Parameters.AddWithValue("@DocDisPercent", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDisPercent", _DocDisPercent);
                }
                if (_DocGrand == null)
                {
                    cm.Parameters.AddWithValue("@DocGrand", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocGrand", _DocGrand);
                }
                if (_DocCurrKey == null)
                {
                    cm.Parameters.AddWithValue("@DocCurrKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCurrKey", _DocCurrKey);
                }
                if (_DocCurrRate == null)
                {
                    cm.Parameters.AddWithValue("@DocCurrRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCurrRate", _DocCurrRate);
                }
                if (_DocSales == null)
                {
                    cm.Parameters.AddWithValue("@DocSales", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSales", _DocSales);
                }
                if (_DocSalesAfterDis == null)
                {
                    cm.Parameters.AddWithValue("@DocSalesAfterDis", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSalesAfterDis", _DocSalesAfterDis);
                }
                if (_DocExpense == null)
                {
                    cm.Parameters.AddWithValue("@DocExpense", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocExpense", _DocExpense);
                }
                if (_DocCountryRate == null)
                {
                    cm.Parameters.AddWithValue("@DocCountryRate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocCountryRate", _DocCountryRate);
                }
                if (_DocStatus == null)
                {
                    cm.Parameters.AddWithValue("@DocStatus", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocStatus", _DocStatus);
                }
                if (_DocState == null)
                {
                    cm.Parameters.AddWithValue("@DocState", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocState", _DocState);
                }
                if (_DocPrinted == null)
                {
                    cm.Parameters.AddWithValue("@DocPrinted", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocPrinted", _DocPrinted);
                }
                if (_ApproveUserKey == null)
                {
                    cm.Parameters.AddWithValue("@ApproveUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ApproveUserKey", _ApproveUserKey);
                }
                if (_ApproveDate == null)
                {
                    cm.Parameters.AddWithValue("@ApproveDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ApproveDate", _ApproveDate);
                }
                if (_DisapproveUserKey == null)
                {
                    cm.Parameters.AddWithValue("@DisapproveUserKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DisapproveUserKey", _DisapproveUserKey);
                }
                if (_DisapproveDate == null)
                {
                    cm.Parameters.AddWithValue("@DisapproveDate", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DisapproveDate", _DisapproveDate);
                }
                if (_DisapproveCount == null)
                {
                    cm.Parameters.AddWithValue("@DisapproveCount", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DisapproveCount", _DisapproveCount);
                }
                if (_DisapproveMsg == null)
                {
                    cm.Parameters.AddWithValue("@DisapproveMsg", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DisapproveMsg", _DisapproveMsg);
                }
                if (_Attachment == null)
                {
                    cm.Parameters.AddWithValue("@Attachment", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@Attachment", _Attachment);
                }
                if (_BranchKey == null)
                {
                    cm.Parameters.AddWithValue("@BranchKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@BranchKey", _BranchKey);
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
                if (_PurgeKeep == null)
                {
                    cm.Parameters.AddWithValue("@PurgeKeep", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@PurgeKeep", _PurgeKeep);
                }
                if (_PurgeData == null)
                {
                    cm.Parameters.AddWithValue("@PurgeData", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@PurgeData", _PurgeData);
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
                if (_DocConID == null)
                {
                    cm.Parameters.AddWithValue("@DocConID", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocConID", _DocConID);
                }
                if (_DefBAddrKey == null)
                {
                    cm.Parameters.AddWithValue("@DefBAddrKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DefBAddrKey", _DefBAddrKey);
                }
                if (_DefSAddrKey == null)
                {
                    cm.Parameters.AddWithValue("@DefSAddrKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DefSAddrKey", _DefSAddrKey);
                }
                if (_DefLocKey == null)
                {
                    cm.Parameters.AddWithValue("@DefLocKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DefLocKey", _DefLocKey);
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
                cm.CommandText = "CSCPS_Delete";

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
                    cm.CommandText = "CSCPS_Validation";

                    cm.Parameters.AddWithValue("@isNew", isNew);
                    cm.Parameters.AddWithValue("@DocCodeKey", criteria._DocCodeKey);
                    cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
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

        internal void Clear()
        {
            this._DocKey = 0;
            this._DocConKey = 0;
            this._DocConNm = string.Empty;
            this._DocPriceAssign = 0;
            this._DocDisPercent = 0;
            this._DocGrand = 0;
            this._DocCurrKey = 0;
            this._DocCurrRate = 0;
            this._DocSales = 0;
            this._DocSalesAfterDis = 0;
            this._DocExpense = 0;
            this._DocCountryRate = 0;
            this._DocConID = string.Empty;
        }
    }
}





