


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
    /// Summary description for APPN.
    /// </summary>
    [Serializable]
    public class APPN : Document, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        internal int? _DocConKey;
        internal string _DocConNm;
        internal int? _DocDeptKey;
        internal int? _DocTranGrpKey;
        internal int? _DocGrpKey;
        internal string _DocBAddrStreet;
        internal string _DocBAddrPOBox;
        internal string _DocBAddrCity;
        internal string _DocBAddrState;
        internal string _DocBAddrZipCode;
        internal string _DocBAddrCountry;
        internal string _DocBAddrRegion;
        internal string _DocBAddrAttn;
        internal string _DocBAddrTel1;
        internal string _DocBAddrTel2;
        internal string _DocBAddrFax;
        internal string _DocBAddrEmail;
        internal string _DocSAddrStreet;
        internal string _DocSAddrPOBox;
        internal string _DocSAddrCity;
        internal string _DocSAddrState;
        internal string _DocSAddrZipCode;
        internal string _DocSAddrCountry;
        internal string _DocSAddrRegion;
        internal string _DocSAddrAttn;
        internal string _DocSAddrTel1;
        internal string _DocSAddrTel2;
        internal string _DocSAddrFax;
        internal string _DocSAddrEmail;
        internal int? _DocShipKey;
        internal bool _VendorItmOnly;
        internal int? _QtyMultiplier;
        internal int? _PlanMthRange;
        internal int? _PlanDistributeInterval;
        internal DateTime? _PlanDate;
        internal DateTime? _DocMth1;
        internal DateTime? _DocMth2;
        internal DateTime? _DocMth3;
        internal DateTime? _DocMth4;
        internal DateTime? _DocMth5;
        internal DateTime? _DocMth6;
        internal DateTime? _DocMth7;
        internal DateTime? _DocMth8;
        internal DateTime? _DocMth9;
        internal DateTime? _DocMth10;
        internal DateTime? _DocMth11;
        internal DateTime? _DocMth12;
        internal string _DocConID;
        internal SYSAttachments attachments = new SYSAttachments();
        internal bool autoDistribute;


        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public APPN()
            : base()
        {
            this._DocDate = DateTime.Today.Date;
            this._DocConKey = 0;
            this._DocConNm = string.Empty;
            this._DocDeptKey = 0;
            this._DocTranGrpKey = 0;
            this._DocGrpKey = 0;
            this._DocBAddrStreet = null;
            this._DocBAddrPOBox = null;
            this._DocBAddrCity = null;
            this._DocBAddrState = null;
            this._DocBAddrZipCode = null;
            this._DocBAddrCountry = null;
            this._DocBAddrRegion = null;
            this._DocBAddrAttn = null;
            this._DocBAddrTel1 = null;
            this._DocBAddrTel2 = null;
            this._DocBAddrFax = null;
            this._DocBAddrEmail = null;
            this._DocSAddrStreet = null;
            this._DocSAddrPOBox = null;
            this._DocSAddrCity = null;
            this._DocSAddrState = null;
            this._DocSAddrZipCode = null;
            this._DocSAddrCountry = null;
            this._DocSAddrRegion = null;
            this._DocSAddrAttn = null;
            this._DocSAddrTel1 = null;
            this._DocSAddrTel2 = null;
            this._DocSAddrFax = null;
            this._DocSAddrEmail = null;
            this._DocShipKey = null;
            this._VendorItmOnly = false;
            this._QtyMultiplier = 0;
            this._PlanMthRange = 0;
            this._PlanDistributeInterval = 0;
            this._PlanDate = DateTime.Today.Date;
            this._DocMth1 = DateTime.Today.Date;
            this._DocMth2 = DateTime.Today.Date;
            this._DocMth3 = DateTime.Today.Date;
            this._DocMth4 = DateTime.Today.Date;
            this._DocMth5 = DateTime.Today.Date;
            this._DocMth6 = DateTime.Today.Date;
            this._DocMth7 = DateTime.Today.Date;
            this._DocMth8 = DateTime.Today.Date;
            this._DocMth9 = DateTime.Today.Date;
            this._DocMth10 = DateTime.Today.Date;
            this._DocMth11 = DateTime.Today.Date;
            this._DocMth12 = DateTime.Today.Date;
            this._DocConID = string.Empty;
            base.PropertyChanged += new PropertyChangedEventHandler(APPN_PropertyChanged);
        }
        void APPN_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }

        public APPN Clone()
        {
            APPN objCopy = (APPN)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
        }
        public static APPN Get(int? docKey)
        {
            APPN child = new APPN();
            child.Fetch(new Criteria(docKey, 1));
            return child;
        }

        public static APPN New()
        {
            APPN child = new APPN();
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
        public string DocBAddrStreet
        {

            get
            {
                return this._DocBAddrStreet;
            }
            set
            {
                this._DocBAddrStreet = value;
                NotifyPropertyChanged("DocBAddrStreet");
            }
        }
        public string DocBAddrPOBox
        {

            get
            {
                return this._DocBAddrPOBox;
            }
            set
            {
                this._DocBAddrPOBox = value;
                NotifyPropertyChanged("DocBAddrPOBox");
            }
        }
        public string DocBAddrCity
        {

            get
            {
                return this._DocBAddrCity;
            }
            set
            {
                this._DocBAddrCity = value;
                NotifyPropertyChanged("DocBAddrCity");
            }
        }
        public string DocBAddrState
        {

            get
            {
                return this._DocBAddrState;
            }
            set
            {
                this._DocBAddrState = value;
                NotifyPropertyChanged("DocBAddrState");
            }
        }
        public string DocBAddrZipCode
        {

            get
            {
                return this._DocBAddrZipCode;
            }
            set
            {
                this._DocBAddrZipCode = value;
                NotifyPropertyChanged("DocBAddrZipCode");
            }
        }
        public string DocBAddrCountry
        {

            get
            {
                return this._DocBAddrCountry;
            }
            set
            {
                this._DocBAddrCountry = value;
                NotifyPropertyChanged("DocBAddrCountry");
            }
        }
        public string DocBAddrRegion
        {

            get
            {
                return this._DocBAddrRegion;
            }
            set
            {
                this._DocBAddrRegion = value;
                NotifyPropertyChanged("DocBAddrRegion");
            }
        }
        public string DocBAddrAttn
        {

            get
            {
                return this._DocBAddrAttn;
            }
            set
            {
                this._DocBAddrAttn = value;
                NotifyPropertyChanged("DocBAddrAttn");
            }
        }
        public string DocBAddrTel1
        {

            get
            {
                return this._DocBAddrTel1;
            }
            set
            {
                this._DocBAddrTel1 = value;
                NotifyPropertyChanged("DocBAddrTel1");
            }
        }
        public string DocBAddrTel2
        {

            get
            {
                return this._DocBAddrTel2;
            }
            set
            {
                this._DocBAddrTel2 = value;
                NotifyPropertyChanged("DocBAddrTel2");
            }
        }
        public string DocBAddrFax
        {

            get
            {
                return this._DocBAddrFax;
            }
            set
            {
                this._DocBAddrFax = value;
                NotifyPropertyChanged("DocBAddrFax");
            }
        }
        public string DocBAddrEmail
        {

            get
            {
                return this._DocBAddrEmail;
            }
            set
            {
                this._DocBAddrEmail = value;
                NotifyPropertyChanged("DocBAddrEmail");
            }
        }
        public string DocSAddrStreet
        {

            get
            {
                return this._DocSAddrStreet;
            }
            set
            {
                this._DocSAddrStreet = value;
                NotifyPropertyChanged("DocSAddrStreet");
            }
        }
        public string DocSAddrPOBox
        {

            get
            {
                return this._DocSAddrPOBox;
            }
            set
            {
                this._DocSAddrPOBox = value;
                NotifyPropertyChanged("DocSAddrPOBox");
            }
        }
        public string DocSAddrCity
        {

            get
            {
                return this._DocSAddrCity;
            }
            set
            {
                this._DocSAddrCity = value;
                NotifyPropertyChanged("DocSAddrCity");
            }
        }
        public string DocSAddrState
        {

            get
            {
                return this._DocSAddrState;
            }
            set
            {
                this._DocSAddrState = value;
                NotifyPropertyChanged("DocSAddrState");
            }
        }
        public string DocSAddrZipCode
        {

            get
            {
                return this._DocSAddrZipCode;
            }
            set
            {
                this._DocSAddrZipCode = value;
                NotifyPropertyChanged("DocSAddrZipCode");
            }
        }
        public string DocSAddrCountry
        {

            get
            {
                return this._DocSAddrCountry;
            }
            set
            {
                this._DocSAddrCountry = value;
                NotifyPropertyChanged("DocSAddrCountry");
            }
        }
        public string DocSAddrRegion
        {

            get
            {
                return this._DocSAddrRegion;
            }
            set
            {
                this._DocSAddrRegion = value;
                NotifyPropertyChanged("DocSAddrRegion");
            }
        }
        public string DocSAddrAttn
        {

            get
            {
                return this._DocSAddrAttn;
            }
            set
            {
                this._DocSAddrAttn = value;
                NotifyPropertyChanged("DocSAddrAttn");
            }
        }
        public string DocSAddrTel1
        {

            get
            {
                return this._DocSAddrTel1;
            }
            set
            {
                this._DocSAddrTel1 = value;
                NotifyPropertyChanged("DocSAddrTel1");
            }
        }
        public string DocSAddrTel2
        {

            get
            {
                return this._DocSAddrTel2;
            }
            set
            {
                this._DocSAddrTel2 = value;
                NotifyPropertyChanged("DocSAddrTel2");
            }
        }
        public string DocSAddrFax
        {

            get
            {
                return this._DocSAddrFax;
            }
            set
            {
                this._DocSAddrFax = value;
                NotifyPropertyChanged("DocSAddrFax");
            }
        }
        public string DocSAddrEmail
        {

            get
            {
                return this._DocSAddrEmail;
            }
            set
            {
                this._DocSAddrEmail = value;
                NotifyPropertyChanged("DocSAddrEmail");
            }
        }
        public int? DocShipKey
        {

            get
            {
                return this._DocShipKey;
            }
            set
            {
                this._DocShipKey = value;
                NotifyPropertyChanged("DocShipKey");
            }
        }
        public bool VendorItmOnly
        {

            get
            {
                return this._VendorItmOnly;
            }
            set
            {
                this._VendorItmOnly = value;
                NotifyPropertyChanged("VendorItmOnly");
            }
        }
        public int? QtyMultiplier
        {

            get
            {
                return this._QtyMultiplier;
            }
            set
            {
                this._QtyMultiplier = value;
                NotifyPropertyChanged("QtyMultiplier");
            }
        }
        public int? PlanMthRange
        {

            get
            {
                return this._PlanMthRange;
            }
            set
            {
                this._PlanMthRange = value;
                NotifyPropertyChanged("PlanMthRange");
            }
        }
        public int? PlanDistributeInterval
        {

            get
            {
                return this._PlanDistributeInterval;
            }
            set
            {
                this._PlanDistributeInterval = value;
                NotifyPropertyChanged("PlanDistributeInterval");
            }
        }
        public DateTime? PlanDate
        {

            get
            {
                return this._PlanDate;
            }
            set
            {
                this._PlanDate = value;
                NotifyPropertyChanged("PlanDate");
            }
        }
        public DateTime? DocMth1
        {

            get
            {
                return this._DocMth1;
            }
            set
            {
                this._DocMth1 = value;
                NotifyPropertyChanged("DocMth1");
            }
        }
        public DateTime? DocMth2
        {

            get
            {
                return this._DocMth2;
            }
            set
            {
                this._DocMth2 = value;
                NotifyPropertyChanged("DocMth2");
            }
        }
        public DateTime? DocMth3
        {

            get
            {
                return this._DocMth3;
            }
            set
            {
                this._DocMth3 = value;
                NotifyPropertyChanged("DocMth3");
            }
        }
        public DateTime? DocMth4
        {

            get
            {
                return this._DocMth4;
            }
            set
            {
                this._DocMth4 = value;
                NotifyPropertyChanged("DocMth4");
            }
        }
        public DateTime? DocMth5
        {

            get
            {
                return this._DocMth5;
            }
            set
            {
                this._DocMth5 = value;
                NotifyPropertyChanged("DocMth5");
            }
        }
        public DateTime? DocMth6
        {

            get
            {
                return this._DocMth6;
            }
            set
            {
                this._DocMth6 = value;
                NotifyPropertyChanged("DocMth6");
            }
        }
        public DateTime? DocMth7
        {

            get
            {
                return this._DocMth7;
            }
            set
            {
                this._DocMth7 = value;
                NotifyPropertyChanged("DocMth7");
            }
        }
        public DateTime? DocMth8
        {

            get
            {
                return this._DocMth8;
            }
            set
            {
                this._DocMth8 = value;
                NotifyPropertyChanged("DocMth8");
            }
        }
        public DateTime? DocMth9
        {

            get
            {
                return this._DocMth9;
            }
            set
            {
                this._DocMth9 = value;
                NotifyPropertyChanged("DocMth9");
            }
        }
        public DateTime? DocMth10
        {

            get
            {
                return this._DocMth10;
            }
            set
            {
                this._DocMth10 = value;
                NotifyPropertyChanged("DocMth10");
            }
        }
        public DateTime? DocMth11
        {

            get
            {
                return this._DocMth11;
            }
            set
            {
                this._DocMth11 = value;
                NotifyPropertyChanged("DocMth11");
            }
        }
        public DateTime? DocMth12
        {

            get
            {
                return this._DocMth12;
            }
            set
            {
                this._DocMth12 = value;
                NotifyPropertyChanged("DocMth12");
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
        public bool AutoDistribute
        {

            get
            {
                return this.autoDistribute;
            }
            set
            {
                this.autoDistribute= value;
            }
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
                cm.CommandText = "APPN_Get";

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
        internal static APPN Get(IDataReader dr)
        {
            APPN child = new APPN();
            child.Fetch(dr);
            return child;
        }
        internal static APPN Get(SqlConnection cn, Criteria criteria)
        {
            APPN child = new APPN();
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
            _DocDeptKey = dataReader["DocDeptKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocDeptKey"];
            _DocTranGrpKey = dataReader["DocTranGrpKey"] == DBNull.Value ? (int?)0 : (int?)dataReader["DocTranGrpKey"];
            _DocGrpKey = dataReader["DocGrpKey"] == DBNull.Value ? (int)0 : (int)dataReader["DocGrpKey"];
            _DocEmKey = dataReader["DocEmKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocEmKey"];
            _DocBAddrStreet = dataReader["DocBAddrStreet"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrStreet"];
            _DocBAddrPOBox = dataReader["DocBAddrPOBox"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrPOBox"];
            _DocBAddrCity = dataReader["DocBAddrCity"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrCity"];
            _DocBAddrState = dataReader["DocBAddrState"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrState"];
            _DocBAddrZipCode = dataReader["DocBAddrZipCode"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrZipCode"];
            _DocBAddrCountry = dataReader["DocBAddrCountry"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrCountry"];
            _DocBAddrRegion = dataReader["DocBAddrRegion"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrRegion"];
            _DocBAddrAttn = dataReader["DocBAddrAttn"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrAttn"];
            _DocBAddrTel1 = dataReader["DocBAddrTel1"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrTel1"];
            _DocBAddrTel2 = dataReader["DocBAddrTel2"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrTel2"];
            _DocBAddrFax = dataReader["DocBAddrFax"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrFax"];
            _DocBAddrEmail = dataReader["DocBAddrEmail"] == DBNull.Value ? (string)null : (string)dataReader["DocBAddrEmail"];
            _DocSAddrStreet = dataReader["DocSAddrStreet"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrStreet"];
            _DocSAddrPOBox = dataReader["DocSAddrPOBox"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrPOBox"];
            _DocSAddrCity = dataReader["DocSAddrCity"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrCity"];
            _DocSAddrState = dataReader["DocSAddrState"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrState"];
            _DocSAddrZipCode = dataReader["DocSAddrZipCode"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrZipCode"];
            _DocSAddrCountry = dataReader["DocSAddrCountry"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrCountry"];
            _DocSAddrRegion = dataReader["DocSAddrRegion"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrRegion"];
            _DocSAddrAttn = dataReader["DocSAddrAttn"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrAttn"];
            _DocSAddrTel1 = dataReader["DocSAddrTel1"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrTel1"];
            _DocSAddrTel2 = dataReader["DocSAddrTel2"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrTel2"];
            _DocSAddrFax = dataReader["DocSAddrFax"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrFax"];
            _DocSAddrEmail = dataReader["DocSAddrEmail"] == DBNull.Value ? (string)null : (string)dataReader["DocSAddrEmail"];
            _DocShipKey = dataReader["DocShipKey"] == DBNull.Value ? (int?)null : (int?)dataReader["DocShipKey"];
            _DocRef = dataReader["DocRef"] == DBNull.Value ? (string)null : (string)dataReader["DocRef"];
            _DocDes = dataReader["DocDes"] == DBNull.Value ? (string)null : (string)dataReader["DocDes"];
            _DocRem = dataReader["DocRem"] == DBNull.Value ? (string)null : (string)dataReader["DocRem"];
            _VendorItmOnly = dataReader["VendorItmOnly"] == DBNull.Value ? (bool)false : (bool)dataReader["VendorItmOnly"];
            _QtyMultiplier = dataReader["QtyMultiplier"] == DBNull.Value ? (int)0 : (int)dataReader["QtyMultiplier"];
            _PlanMthRange = dataReader["PlanMthRange"] == DBNull.Value ? (int)0 : (int)dataReader["PlanMthRange"];
            _PlanDistributeInterval = dataReader["PlanDistributeInterval"] == DBNull.Value ? (int)0 : (int)dataReader["PlanDistributeInterval"];
            _PlanDate= dataReader["PlanDate"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["PlanDate"];
            _DocMth1 = dataReader["DocMth1"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["DocMth1"];
            _DocMth2 = dataReader["DocMth2"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["DocMth2"];
            _DocMth3 = dataReader["DocMth3"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["DocMth3"];
            _DocMth4 = dataReader["DocMth4"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["DocMth4"];
            _DocMth5 = dataReader["DocMth5"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["DocMth5"];
            _DocMth6 = dataReader["DocMth6"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["DocMth6"];
            _DocMth7 = dataReader["DocMth7"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["DocMth7"];
            _DocMth8 = dataReader["DocMth8"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["DocMth8"];
            _DocMth9 = dataReader["DocMth9"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["DocMth9"];
            _DocMth10 = dataReader["DocMth10"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["DocMth10"];
            _DocMth11 = dataReader["DocMth11"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["DocMth11"];
            _DocMth12 = dataReader["DocMth12"] == DBNull.Value ? (DateTime?)DateTime.Today.Date : (DateTime?)dataReader["DocMth12"];
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
           
            // Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();
                retValue = this.Insert(cn);
            }
             
            return retValue;
        }
        internal bool Insert(SqlConnection cn)
        {
            string msgID = "RecordAddFail";
            DocKey = 0;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "APPN_AddUpdate";

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
                if (_DocDeptKey == null)
                {
                    cm.Parameters.AddWithValue("@DocDeptKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDeptKey", _DocDeptKey);
                }
                if (_DocTranGrpKey == null)
                {
                    cm.Parameters.AddWithValue("@DocTranGrpKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTranGrpKey", _DocTranGrpKey);
                }
                if (_DocGrpKey == null)
                {
                    cm.Parameters.AddWithValue("@DocGrpKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocGrpKey", _DocGrpKey);
                }
                if (_DocEmKey == null)
                {
                    cm.Parameters.AddWithValue("@DocEmKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocEmKey", _DocEmKey);
                }
                if (_DocBAddrStreet == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrStreet", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrStreet", _DocBAddrStreet);
                }
                if (_DocBAddrPOBox == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrPOBox", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrPOBox", _DocBAddrPOBox);
                }
                if (_DocBAddrCity == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrCity", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrCity", _DocBAddrCity);
                }
                if (_DocBAddrState == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrState", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrState", _DocBAddrState);
                }
                if (_DocBAddrZipCode == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrZipCode", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrZipCode", _DocBAddrZipCode);
                }
                if (_DocBAddrCountry == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrCountry", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrCountry", _DocBAddrCountry);
                }
                if (_DocBAddrRegion == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrRegion", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrRegion", _DocBAddrRegion);
                }
                if (_DocBAddrAttn == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrAttn", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrAttn", _DocBAddrAttn);
                }
                if (_DocBAddrTel1 == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrTel1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrTel1", _DocBAddrTel1);
                }
                if (_DocBAddrTel2 == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrTel2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrTel2", _DocBAddrTel2);
                }
                if (_DocBAddrFax == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrFax", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrFax", _DocBAddrFax);
                }
                if (_DocBAddrEmail == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrEmail", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrEmail", _DocBAddrEmail);
                }
                if (_DocSAddrStreet == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrStreet", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrStreet", _DocSAddrStreet);
                }
                if (_DocSAddrPOBox == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrPOBox", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrPOBox", _DocSAddrPOBox);
                }
                if (_DocSAddrCity == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrCity", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrCity", _DocSAddrCity);
                }
                if (_DocSAddrState == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrState", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrState", _DocSAddrState);
                }
                if (_DocSAddrZipCode == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrZipCode", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrZipCode", _DocSAddrZipCode);
                }
                if (_DocSAddrCountry == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrCountry", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrCountry", _DocSAddrCountry);
                }
                if (_DocSAddrRegion == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrRegion", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrRegion", _DocSAddrRegion);
                }
                if (_DocSAddrAttn == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrAttn", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrAttn", _DocSAddrAttn);
                }
                if (_DocSAddrTel1 == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrTel1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrTel1", _DocSAddrTel1);
                }
                if (_DocSAddrTel2 == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrTel2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrTel2", _DocSAddrTel2);
                }
                if (_DocSAddrFax == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrFax", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrFax", _DocSAddrFax);
                }
                if (_DocSAddrEmail == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrEmail", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrEmail", _DocSAddrEmail);
                }
                if (_DocShipKey == null)
                {
                    cm.Parameters.AddWithValue("@DocShipKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocShipKey", _DocShipKey);
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
                if (_VendorItmOnly == null)
                {
                    cm.Parameters.AddWithValue("@VendorItmOnly", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@VendorItmOnly", _VendorItmOnly);
                }
                if (_QtyMultiplier == null)
                {
                    cm.Parameters.AddWithValue("@QtyMultiplier", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@QtyMultiplier", _QtyMultiplier);
                }
                if (_PlanMthRange == null)
                {
                    cm.Parameters.AddWithValue("@PlanMthRange", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@PlanMthRange", _PlanMthRange);
                }
                if (_PlanDistributeInterval == null)
                {
                    cm.Parameters.AddWithValue("@PlanDistributeInterval", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@PlanDistributeInterval", _PlanDistributeInterval);
                }
                if (_DocMth1 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth1", _DocMth1);
                }
                if (_DocMth2 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth2", _DocMth2);
                }
                if (_DocMth3 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth3", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth3", _DocMth3);
                }
                if (_DocMth4 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth4", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth4", _DocMth4);
                }
                if (_DocMth5 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth5", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth5", _DocMth5);
                }
                if (_DocMth6 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth6", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth6", _DocMth6);
                }
                if (_DocMth7 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth7", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth7", _DocMth7);
                }
                if (_DocMth8 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth8", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth8", _DocMth8);
                }
                if (_DocMth9 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth9", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth9", _DocMth9);
                }
                if (_DocMth10 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth10", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth10", _DocMth10);
                }
                if (_DocMth11 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth11", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth11", _DocMth11);
                }
                if (_DocMth12 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth12", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth12", _DocMth12);
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
                cm.CommandText = "APPN_AddUpdate";

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
                if (_DocDeptKey == null)
                {
                    cm.Parameters.AddWithValue("@DocDeptKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocDeptKey", _DocDeptKey);
                }
                if (_DocTranGrpKey == null)
                {
                    cm.Parameters.AddWithValue("@DocTranGrpKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocTranGrpKey", _DocTranGrpKey);
                }
                if (_DocGrpKey == null)
                {
                    cm.Parameters.AddWithValue("@DocGrpKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocGrpKey", _DocGrpKey);
                }
                if (_DocEmKey == null)
                {
                    cm.Parameters.AddWithValue("@DocEmKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocEmKey", _DocEmKey);
                }
                if (_DocBAddrStreet == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrStreet", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrStreet", _DocBAddrStreet);
                }
                if (_DocBAddrPOBox == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrPOBox", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrPOBox", _DocBAddrPOBox);
                }
                if (_DocBAddrCity == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrCity", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrCity", _DocBAddrCity);
                }
                if (_DocBAddrState == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrState", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrState", _DocBAddrState);
                }
                if (_DocBAddrZipCode == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrZipCode", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrZipCode", _DocBAddrZipCode);
                }
                if (_DocBAddrCountry == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrCountry", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrCountry", _DocBAddrCountry);
                }
                if (_DocBAddrRegion == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrRegion", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrRegion", _DocBAddrRegion);
                }
                if (_DocBAddrAttn == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrAttn", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrAttn", _DocBAddrAttn);
                }
                if (_DocBAddrTel1 == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrTel1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrTel1", _DocBAddrTel1);
                }
                if (_DocBAddrTel2 == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrTel2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrTel2", _DocBAddrTel2);
                }
                if (_DocBAddrFax == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrFax", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrFax", _DocBAddrFax);
                }
                if (_DocBAddrEmail == null)
                {
                    cm.Parameters.AddWithValue("@DocBAddrEmail", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocBAddrEmail", _DocBAddrEmail);
                }
                if (_DocSAddrStreet == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrStreet", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrStreet", _DocSAddrStreet);
                }
                if (_DocSAddrPOBox == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrPOBox", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrPOBox", _DocSAddrPOBox);
                }
                if (_DocSAddrCity == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrCity", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrCity", _DocSAddrCity);
                }
                if (_DocSAddrState == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrState", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrState", _DocSAddrState);
                }
                if (_DocSAddrZipCode == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrZipCode", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrZipCode", _DocSAddrZipCode);
                }
                if (_DocSAddrCountry == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrCountry", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrCountry", _DocSAddrCountry);
                }
                if (_DocSAddrRegion == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrRegion", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrRegion", _DocSAddrRegion);
                }
                if (_DocSAddrAttn == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrAttn", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrAttn", _DocSAddrAttn);
                }
                if (_DocSAddrTel1 == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrTel1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrTel1", _DocSAddrTel1);
                }
                if (_DocSAddrTel2 == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrTel2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrTel2", _DocSAddrTel2);
                }
                if (_DocSAddrFax == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrFax", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrFax", _DocSAddrFax);
                }
                if (_DocSAddrEmail == null)
                {
                    cm.Parameters.AddWithValue("@DocSAddrEmail", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocSAddrEmail", _DocSAddrEmail);
                }
                if (_DocShipKey == null)
                {
                    cm.Parameters.AddWithValue("@DocShipKey", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocShipKey", _DocShipKey);
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
                if (_VendorItmOnly == null)
                {
                    cm.Parameters.AddWithValue("@VendorItmOnly", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@VendorItmOnly", _VendorItmOnly);
                }
                if (_QtyMultiplier == null)
                {
                    cm.Parameters.AddWithValue("@QtyMultiplier", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@QtyMultiplier", _QtyMultiplier);
                }
                if (_PlanMthRange == null)
                {
                    cm.Parameters.AddWithValue("@PlanMthRange", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@PlanMthRange", _PlanMthRange);
                }
                if (_PlanDistributeInterval == null)
                {
                    cm.Parameters.AddWithValue("@PlanDistributeInterval", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@PlanDistributeInterval", _PlanDistributeInterval);
                }
                if (_DocMth1 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth1", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth1", _DocMth1);
                }
                if (_DocMth2 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth2", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth2", _DocMth2);
                }
                if (_DocMth3 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth3", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth3", _DocMth3);
                }
                if (_DocMth4 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth4", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth4", _DocMth4);
                }
                if (_DocMth5 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth5", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth5", _DocMth5);
                }
                if (_DocMth6 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth6", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth6", _DocMth6);
                }
                if (_DocMth7 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth7", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth7", _DocMth7);
                }
                if (_DocMth8 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth8", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth8", _DocMth8);
                }
                if (_DocMth9 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth9", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth9", _DocMth9);
                }
                if (_DocMth10 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth10", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth10", _DocMth10);
                }
                if (_DocMth11 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth11", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth11", _DocMth11);
                }
                if (_DocMth12 == null)
                {
                    cm.Parameters.AddWithValue("@DocMth12", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@DocMth12", _DocMth12);
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
                cm.CommandText = "APPN_Delete";

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
                //using (TransactionScope scope = new TransactionScope())
                //{
                    //Create new sql connection for this method. 
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open sql connection. 
                        cn.Open();
                        retValue = Validation(cn, criteria, isNew);
                    }
                    // No errors - commit transaction
                //      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                //}// Already close and dispose sql connection.
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
                    cm.CommandText = "APPN_Validation";

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


        private void Clear()
        {
            this._DocKey = 0;
            this._DocDate = DateTime.Today.Date;
            this._DocConKey = 0;
            this._DocConNm = string.Empty;
            this._DocDeptKey = 0;
            this._DocTranGrpKey = 0;
            this._DocGrpKey = 0;
            this._DocBAddrStreet = null;
            this._DocBAddrPOBox = null;
            this._DocBAddrCity = null;
            this._DocBAddrState = null;
            this._DocBAddrZipCode = null;
            this._DocBAddrCountry = null;
            this._DocBAddrRegion = null;
            this._DocBAddrAttn = null;
            this._DocBAddrTel1 = null;
            this._DocBAddrTel2 = null;
            this._DocBAddrFax = null;
            this._DocBAddrEmail = null;
            this._DocSAddrStreet = null;
            this._DocSAddrPOBox = null;
            this._DocSAddrCity = null;
            this._DocSAddrState = null;
            this._DocSAddrZipCode = null;
            this._DocSAddrCountry = null;
            this._DocSAddrRegion = null;
            this._DocSAddrAttn = null;
            this._DocSAddrTel1 = null;
            this._DocSAddrTel2 = null;
            this._DocSAddrFax = null;
            this._DocSAddrEmail = null;
            this._DocShipKey = null;
            this._VendorItmOnly = false;
            this._QtyMultiplier = 0;
            this._PlanMthRange = 0;
            this._PlanDistributeInterval = 0;
            this._PlanDate = DateTime.Today.Date;
            this._DocMth1 = DateTime.Today.Date;
            this._DocMth2 = DateTime.Today.Date;
            this._DocMth3 = DateTime.Today.Date;
            this._DocMth4 = DateTime.Today.Date;
            this._DocMth5 = DateTime.Today.Date;
            this._DocMth6 = DateTime.Today.Date;
            this._DocMth7 = DateTime.Today.Date;
            this._DocMth8 = DateTime.Today.Date;
            this._DocMth9 = DateTime.Today.Date;
            this._DocMth10 = DateTime.Today.Date;
            this._DocMth11 = DateTime.Today.Date;
            this._DocMth12 = DateTime.Today.Date;
            this._DocConID = string.Empty;
        }
    
    }
}





