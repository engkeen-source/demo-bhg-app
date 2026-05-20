using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
	/// <summary>
	/// Summary description for ARSODetItm.
	/// </summary>
	[Serializable]
    public class ARSODetItm : DocSalePur, IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++
        protected int _LineType;
        protected int _LineLinkKey;
        protected int _ItmDeptKey;
        protected int? _ItmTranGrpKey;
        protected int? _ItmAccKey;
        protected decimal? _ItmQtyLink;
        protected decimal? _ItmQtyAdj;
        protected decimal? _ItmQtyDelivered;
        protected decimal? _ItmQtyBalance;
        protected int? _ItmOrderStatus;
        protected DateTime? _ItmReqDate;
        protected DateTime? _ItmPrmDate;
        protected decimal? _ItmPriceBefore;
        protected decimal? _ItmPriceAfter;
        protected decimal? _ItmControlPrice;
        protected decimal? _ItmControlPriceBase;
        protected bool _ItmTaxable;
        protected int? _ItmTaxGrpKey;
        protected decimal? _ItmTaxGrpRate;
        protected decimal? _ItmTaxGrpAmtF;
        protected decimal? _ItmTaxGrpAmtL;
        protected bool _ItmHide;
        //added by thettm on 24-oct-2017(start)
        protected bool _released_trigger;
        //added by thettm on 24-oct-2017(end)
        //added by thettm on 11-jan-2018(start)
        protected string _grpStock;
        //added by thettm on 11-jan-2018(end)
        protected int? _ItmVendorKey;
        protected int _ItmVendorCurrKey;
        protected float _ItmVendorCurrRate;
        protected decimal _ItmVendorPrice;
        protected decimal _ItmVendorPriceRatio;
        protected bool _ItmVendorPriceLock;
        protected int _ItmIGrpDItm;
        protected bool _ItmIGrpQtyLock;
        protected bool _ItmIGrpToPrint;
        protected decimal _ItmIGrpQtySet;
        protected decimal _ItmIGrpAmtSet;
        protected int? _ItmBatchKey;
        protected decimal? _ItmBatchQty;
        protected string _NSLink;
        protected string _ARQOID;
        protected int _ARQODK;
        protected int _ARQODItm;
        protected string _ItmID;
        protected string _ItmAccID;
        protected string _ItmAccDes;
        protected string _ItmVendorID;
        protected string _ItmVendorNm;
        protected string _SKU1;
        protected string _SKU2;
        protected decimal? _ItmLatestCostShw;
        protected string _APPOID;
        protected string _HSCode;
        protected string _CountryID;

        private string _error = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public ARSODetItm()
            : base()
        {
            this._LineType = 0;
            this._LineLinkKey = 0;
            this._ItmDeptKey = 0;
            this._ItmTranGrpKey = 0;
            this._ItmAccKey = null;
            this._ItmQtyLink = null;
            this._ItmQtyAdj = null;
            this._ItmQtyDelivered = 0;
            this._ItmQtyBalance = 0;
            this._ItmOrderStatus = 0;
            this._ItmReqDate = DateTime.Today.Date;
            this._ItmPrmDate = DateTime.Today.Date;
            this._ItmPriceBefore = null;
            this._ItmPriceAfter = null;
            this._ItmControlPrice = null;
            this._ItmControlPriceBase = null;
            this._ItmTaxable = false;
            this._ItmTaxGrpKey = null;
            this._ItmTaxGrpRate = 0;
            this._ItmTaxGrpAmtF = 0;
            this._ItmTaxGrpAmtL = 0;
            this._ItmHide = false;
            //added by thettm on 24-oct-2017(start)
            this._released_trigger = false;
            //added by thettm on 24-oct-2017(end)
            //added by thettm on 11-jan-2018(start)
            this._grpStock = string.Empty;
            //added by thettm on 11-jan-2018(end)
            this._ItmVendorKey = null;
            this._ItmVendorCurrKey = 0;
            this._ItmVendorCurrRate = 0;
            this._ItmVendorPrice = 0;
            this._ItmVendorPriceRatio = 0;
            this._ItmVendorPriceLock = false;
            this._ItmIGrpDItm = 0;
            this._ItmIGrpQtyLock = false;
            this._ItmIGrpToPrint = false;
            this._ItmIGrpQtySet = 0;
            this._ItmIGrpAmtSet = 0;
            this._ItmBatchKey = 0;
            this._ItmBatchQty = 0;
            this._NSLink = string.Empty;
            this._ARQOID = null;
            this._ARQODK = 0;
            this._ARQODItm = 0;
            this._ItmID = string.Empty;
            this._HSCode = string.Empty;
            this._ItmAccID = string.Empty;
            this._ItmAccDes = string.Empty;
            this._ItmVendorID = string.Empty;
            this._ItmVendorNm = string.Empty;
            this._SKU1 = string.Empty;
            this._SKU2 = string.Empty;
            this._ItmLatestCostShw = 0;
            this._APPOID = string.Empty;
            this._CountryID = string.Empty;


        }


        public ARSODetItm Clone()
        {
            ARSODetItm objCopy = (ARSODetItm)this.MemberwiseClone();
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


        public int LineType
        {

            get
            {
                return this._LineType;
            }
            set
            {
                this._LineType = value;
                NotifyPropertyChanged("LineType");
            }
        }
        public int LineLinkKey
        {

            get
            {
                return this._LineLinkKey;
            }
            set
            {
                this._LineLinkKey = value;
                NotifyPropertyChanged("LineLinkKey");
            }
        }
        public int ItmDeptKey
        {

            get
            {
                return this._ItmDeptKey;
            }
            set
            {
                this._ItmDeptKey = value;
                NotifyPropertyChanged("ItmDeptKey");
            }
        }
        public int? ItmTranGrpKey
        {

            get
            {
                return this._ItmTranGrpKey;
            }
            set
            {
                this._ItmTranGrpKey = value;
                NotifyPropertyChanged("ItmTranGrpKey");
            }
        }
        public int? ItmAccKey
        {

            get
            {
                return this._ItmAccKey;
            }
            set
            {
                this._ItmAccKey = value;
                NotifyPropertyChanged("ItmAccKey");
            }
        }
        public decimal? ItmQtyLink
        {

            get
            {
                return this._ItmQtyLink;
            }
            set
            {
                this._ItmQtyLink = value;
                NotifyPropertyChanged("ItmQtyLink");
            }
        }
        public decimal? ItmQtyAdj
        {

            get
            {
                return this._ItmQtyAdj;
            }
            set
            {
                this._ItmQtyAdj = value;
                NotifyPropertyChanged("ItmQtyAdj");
            }
        }
        public decimal? ItmQtyDelivered
        {

            get
            {
                return this._ItmQtyDelivered;
            }
            set
            {
                this._ItmQtyDelivered = value;
                NotifyPropertyChanged("ItmQtyDelivered");
            }
        }
        public decimal? ItmQtyBalance
        {

            get
            {
                return this._ItmQtyBalance;
            }
            set
            {
                this._ItmQtyBalance = value;
                NotifyPropertyChanged("ItmQtyBalance");
            }
        }
        public int? ItmOrderStatus
        {

            get
            {
                return this._ItmOrderStatus;
            }
            set
            {
                this._ItmOrderStatus = value;
                NotifyPropertyChanged("ItmOrderStatus");
            }
        }
        public DateTime? ItmReqDate
        {

            get
            {
                return this._ItmReqDate;
            }
            set
            {
                this._ItmReqDate = value;
                NotifyPropertyChanged("ItmReqDate");
            }
        }
        public DateTime? ItmPrmDate
        {

            get
            {
                return this._ItmPrmDate;
            }
            set
            {
                this._ItmPrmDate = value;
                NotifyPropertyChanged("ItmPrmDate");
            }
        }
        public decimal? ItmPriceBefore
        {

            get
            {
                return this._ItmPriceBefore;
            }
            set
            {
                this._ItmPriceBefore = value;
                NotifyPropertyChanged("ItmPriceBefore");
            }
        }
        public decimal? ItmPriceAfter
        {

            get
            {
                return this._ItmPriceAfter;
            }
            set
            {
                this._ItmPriceAfter = value;
                NotifyPropertyChanged("ItmPriceAfter");
            }
        }
        public decimal? ItmControlPrice
        {

            get
            {
                return this._ItmControlPrice;
            }
            set
            {
                this._ItmControlPrice = value;
                NotifyPropertyChanged("ItmControlPrice");
            }
        }
        public decimal? ItmControlPriceBase
        {

            get
            {
                return this._ItmControlPriceBase;
            }
            set
            {
                this._ItmControlPriceBase = value;
                NotifyPropertyChanged("ItmControlPriceBase");
            }
        }
        public bool ItmTaxable
        {

            get
            {
                return this._ItmTaxable;
            }
            set
            {
                this._ItmTaxable = value;
                NotifyPropertyChanged("ItmTaxable");
            }
        }
        public int? ItmTaxGrpKey
        {

            get
            {
                return this._ItmTaxGrpKey;
            }
            set
            {
                this._ItmTaxGrpKey = value;
                NotifyPropertyChanged("ItmTaxGrpKey");
            }
        }
        public decimal? ItmTaxGrpRate
        {

            get
            {
                return this._ItmTaxGrpRate;
            }
            set
            {
                this._ItmTaxGrpRate = value;
                NotifyPropertyChanged("ItmTaxGrpRate");
            }
        }
        public decimal? ItmTaxGrpAmtF
        {

            get
            {
                return this._ItmTaxGrpAmtF;
            }
            set
            {
                this._ItmTaxGrpAmtF = value;
                NotifyPropertyChanged("ItmTaxGrpAmtF");
            }
        }
        public decimal? ItmTaxGrpAmtL
        {

            get
            {
                return this._ItmTaxGrpAmtL;
            }
            set
            {
                this._ItmTaxGrpAmtL = value;
                NotifyPropertyChanged("ItmTaxGrpAmtL");
            }
        }
        //added by thettm on 24-oct-2017(start)
        public bool Released_trigger
        {

            get
            {
                return this._released_trigger;
            }
            set
            {
                this._released_trigger = value;
                NotifyPropertyChanged("Released_trigger");
            }
        }
        //added by thettm on 24-oct-2017(end)

        //added by thettm on 11-jan-2018(start)
        public string GrpStock
        {

            get
            {
                return this._grpStock;
            }
            set
            {
                this._grpStock = value;
                NotifyPropertyChanged("GrpStock");
            }
        }
        //added by thettm on 11-jan-2018(end)
        public bool ItmHide
        {

            get
            {
                return this._ItmHide;
            }
            set
            {
                this._ItmHide = value;
                NotifyPropertyChanged("ItmHide");
            }
        }
        public int? ItmVendorKey
        {

            get
            {
                return this._ItmVendorKey;
            }
            set
            {
                this._ItmVendorKey = value;
                NotifyPropertyChanged("ItmVendorKey");
            }
        }
        public int ItmVendorCurrKey
        {

            get
            {
                return this._ItmVendorCurrKey;
            }
            set
            {
                this._ItmVendorCurrKey = value;
                NotifyPropertyChanged("ItmVendorCurrKey");
            }
        }
        public float ItmVendorCurrRate
        {

            get
            {
                return this._ItmVendorCurrRate;
            }
            set
            {
                this._ItmVendorCurrRate = value;
                NotifyPropertyChanged("ItmVendorCurrRate");
            }
        }
        public decimal ItmVendorPrice
        {

            get
            {
                return this._ItmVendorPrice;
            }
            set
            {
                this._ItmVendorPrice = value;
                NotifyPropertyChanged("ItmVendorPrice");
            }
        }
        public decimal ItmVendorPriceRatio
        {

            get
            {
                return this._ItmVendorPriceRatio;
            }
            set
            {
                this._ItmVendorPriceRatio = value;
                NotifyPropertyChanged("ItmVendorPriceRatio");
            }
        }
        public bool ItmVendorPriceLock
        {

            get
            {
                return this._ItmVendorPriceLock;
            }
            set
            {
                this._ItmVendorPriceLock = value;
                NotifyPropertyChanged("ItmVendorPriceLock");
            }
        }
        public int ItmIGrpDItm
        {

            get
            {
                return this._ItmIGrpDItm;
            }
            set
            {
                this._ItmIGrpDItm = value;
                NotifyPropertyChanged("ItmIGrpDItm");
            }
        }
        public bool ItmIGrpQtyLock
        {

            get
            {
                return this._ItmIGrpQtyLock;
            }
            set
            {
                this._ItmIGrpQtyLock = value;
                NotifyPropertyChanged("ItmIGrpQtyLock");
            }
        }
        public bool ItmIGrpToPrint
        {

            get
            {
                return this._ItmIGrpToPrint;
            }
            set
            {
                this._ItmIGrpToPrint = value;
                NotifyPropertyChanged("ItmIGrpToPrint");
            }
        }
        public decimal ItmIGrpQtySet
        {

            get
            {
                return this._ItmIGrpQtySet;
            }
            set
            {
                this._ItmIGrpQtySet = value;
                NotifyPropertyChanged("ItmIGrpQtySet");
            }
        }
        public decimal ItmIGrpAmtSet
        {

            get
            {
                return this._ItmIGrpAmtSet;
            }
            set
            {
                this._ItmIGrpAmtSet = value;
                NotifyPropertyChanged("ItmIGrpAmtSet");
            }
        }
        public int? ItmBatchKey
        {

            get
            {
                return this._ItmBatchKey;
            }
            set
            {
                this._ItmBatchKey = value;
                NotifyPropertyChanged("ItmBatchKey");
            }
        }
        public decimal? ItmBatchQty
        {

            get
            {
                return this._ItmBatchQty;
            }
            set
            {
                this._ItmBatchQty = value;
                NotifyPropertyChanged("ItmBatchQty");
            }
        }
        public string NSLink
        {

            get
            {
                return this._NSLink;
            }
            set
            {
                this._NSLink = value;
                NotifyPropertyChanged("NSLink");
            }
        }
        public string ARQOID
        {

            get
            {
                return this._ARQOID;
            }
            set
            {
                this._ARQOID = value;
                NotifyPropertyChanged("ARQOID");
            }
        }
        public int ARQODK
        {

            get
            {
                return this._ARQODK;
            }
            set
            {
                this._ARQODK = value;
                NotifyPropertyChanged("ARQODK");
            }
        }
        public int ARQODItm
        {

            get
            {
                return this._ARQODItm;
            }
            set
            {
                this._ARQODItm = value;
                NotifyPropertyChanged("ARQODItm");
            }
        }
        public string ItmID
        {

            get
            {
                return this._ItmID;
            }
            set
            {
                this._ItmID = value;
                NotifyPropertyChanged("ItmID");
            }
        }
        public string HSCode
        {

            get
            {
                return this._HSCode;
            }
            set
            {
                this._HSCode = value;
                NotifyPropertyChanged("HSCode");
            }
        }
        public string CountryID
        {

            get
            {
                return this._CountryID;
            }
            set
            {
                this._CountryID = value;
                NotifyPropertyChanged("CountryID");
            }
        }
        public string ItmAccID
        {

            get
            {
                return this._ItmAccID;
            }
            set
            {
                this._ItmAccID = value;
                NotifyPropertyChanged("ItmAccID");
            }
        }
        public string ItmAccDes
        {

            get
            {
                return this._ItmAccDes;
            }
            set
            {
                this._ItmAccDes = value;
                NotifyPropertyChanged("ItmAccDes");
            }
        }
        public string ItmVendorID
        {

            get
            {
                return this._ItmVendorID;
            }
            set
            {
                this._ItmVendorID = value;
                NotifyPropertyChanged("ItmVendorID");
            }
        }
        public string ItmVendorNm
        {

            get
            {
                return this._ItmVendorNm;
            }
            set
            {
                this._ItmVendorNm = value;
                NotifyPropertyChanged("ItmVendorNm");
            }
        }
        public string SKU1
        {

            get
            {
                return this._SKU1;
            }
            set
            {
                this._SKU1 = value;
                NotifyPropertyChanged("SKU1");
            }
        }
        public string SKU2
        {

            get
            {
                return this._SKU2;
            }
            set
            {
                this._SKU2 = value;
                NotifyPropertyChanged("SKU2");
            }
        }
        public decimal? ItmLatestCostShw
        {

            get
            {
                return this._ItmLatestCostShw;
            }
            set
            {
                this._ItmLatestCostShw = value;
                NotifyPropertyChanged("ItmLatestCostShw");
            }
        }

        public string APPOID
        {

            get
            {
                return this._APPOID;
            }
            set
            {
                this._APPOID = value;
                NotifyPropertyChanged("APPOID");
            }
        }
        #endregion

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
				cm.CommandText = "ARSODetItm_Get";

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
		internal static ARSODetItm Get(IDataReader dr, out string msgID)
		{
			msgID = "RecordGetFail";
			ARSODetItm child = new ARSODetItm();
			child.Fetch(dr, out msgID);
			return child;
		}

		internal bool Fetch(IDataReader dataReader, out string msgID)
		{
			msgID = "RecordGetFail";
			// Fill data to entity object
            ItmVendorCurrRate = (float)dataReader["ItmVendorCurrRate"];
            _DocKey = (int)dataReader["DocKey"];
            _DocItmKey = (int)dataReader["DocItmKey"];
            _LineType = (int)dataReader["LineType"];
            _LineLinkKey = (int)dataReader["LineLinkKey"];
            _ItmSN = (decimal)dataReader["ItmSN"];
            _ItmKey = (int)dataReader["ItmKey"];
            _ItmKeySelect =(int)dataReader["ItmKeySelect"];
            _ItmType = (int)dataReader["ItmType"];
            _ItmDes =dataReader["ItmDes"] == DBNull.Value ? string.Empty:dataReader["ItmDes"].ToString();
            _ItmDeptKey = (int)dataReader["ItmDeptKey"];
            _ItmTranGrpKey =dataReader["ItmTranGrpKey"] == DBNull.Value ? null : (int?)dataReader["ItmTranGrpKey"];
            _ItmAccKey =dataReader["ItmAccKey"] == DBNull.Value ? null : (int?)dataReader["ItmAccKey"];
            _ItmLocKey = (int)dataReader["ItmLocKey"];
            _ItmStock = (decimal)dataReader["ItmStock"];
            _ItmQty = (decimal)dataReader["ItmQty"];
            _ItmQtyLink =dataReader["ItmQtyLink"] == DBNull.Value ? null : (decimal?)dataReader["ItmQtyLink"];
            _ItmQtyAdj =dataReader["ItmQtyAdj"] == DBNull.Value ? null : (decimal?)dataReader["ItmQtyAdj"];
            _ItmOrderStatus =dataReader["ItmOrderStatus"] == DBNull.Value ? null : (int?)dataReader["ItmOrderStatus"];
            _ItmReqDate =dataReader["ItmReqDate"] == DBNull.Value ? null : (DateTime?)dataReader["ItmReqDate"];
            _ItmPrmDate =dataReader["ItmPrmDate"] == DBNull.Value ? null : (DateTime?)dataReader["ItmPrmDate"];
            _ItmUOMKey =(int)dataReader["ItmUOMKey"];
            _ItmConRate = (decimal)dataReader["ItmConRate"];
            _ItmLatestCostF = (decimal)dataReader["ItmLatestCostF"];
            _ItmLatestCostH = (decimal)dataReader["ItmLatestCostH"];
            _ItmListPrice = (decimal)dataReader["ItmListPrice"];
            _ItmPriceBefore =dataReader["ItmPriceBefore"] == DBNull.Value ? null : (decimal?)dataReader["ItmPriceBefore"];
            _ItmPriceAfter =dataReader["ItmPriceAfter"] == DBNull.Value ? null : (decimal?)dataReader["ItmPriceAfter"];
            _ItmDisPercent = (decimal)dataReader["ItmDisPercent"];
            _ItmDisValue = (decimal)dataReader["ItmDisValue"];
            _ItmPrice = (decimal)dataReader["ItmPrice"];
            _ItmPriceUser = (decimal)dataReader["ItmPriceUser"];
            _ItmControlPrice =dataReader["ItmControlPrice"] == DBNull.Value ? null : (decimal?)dataReader["ItmControlPrice"];
            _ItmControlPriceBase =dataReader["ItmControlPriceBase"] == DBNull.Value ? null : (decimal?)dataReader["ItmControlPriceBase"];
            _ItmAmtShw = (decimal)dataReader["ItmAmtShw"];
            _ItmAmtF =(decimal)dataReader["ItmAmtF"];
            _ItmAmtH = (decimal)dataReader["ItmAmtH"];
            _ItmTaxable =dataReader["ItmTaxable"] == DBNull.Value ? false : (bool)dataReader["ItmTaxable"];
            _ItmTaxGrpKey =dataReader["ItmTaxGrpKey"] == DBNull.Value ? null : (int?)dataReader["ItmTaxGrpKey"];
            _ItmTaxGrpRate =dataReader["ItmTaxGrpRate"] == DBNull.Value ? null : (decimal?)dataReader["ItmTaxGrpRate"];
            _ItmTaxGrpAmtF =dataReader["ItmTaxGrpAmtF"] == DBNull.Value ? null : (decimal?)dataReader["ItmTaxGrpAmtF"];
            _ItmTaxGrpAmtL =dataReader["ItmTaxGrpAmtL"] == DBNull.Value ? null : (decimal?)dataReader["ItmTaxGrpAmtL"];
            _ItmHide =dataReader["ItmHide"] == DBNull.Value ? false : (bool)dataReader["ItmHide"];
            //added by thettm on 24-oct-2017(start)
            _released_trigger = dataReader["Released_trigger"] == DBNull.Value ? false : (bool)dataReader["Released_trigger"];
            //added by thettm on 24-oct-2017(end)

            //added by thettm on 11-jan-2018(start)
            _grpStock = dataReader["GrpStock"] == DBNull.Value ? string.Empty : dataReader["GrpStock"].ToString();
            //added by thettm on 11-jan-2018(end)

            _ItmColorKey = (int)dataReader["ItmColorKey"];
            _ItmScaleSize =dataReader["ItmScaleSize"] == DBNull.Value ? string.Empty:dataReader["ItmScaleSize"].ToString();
            _ItmPacking =dataReader["ItmPacking"] == DBNull.Value ? string.Empty:dataReader["ItmPacking"].ToString();
            _ItmRem =dataReader["ItmRem"] == DBNull.Value ? string.Empty:dataReader["ItmRem"].ToString();
            _ItmMark =dataReader["ItmMark"] == DBNull.Value ? string.Empty:dataReader["ItmMark"].ToString();
            _ItmVendorKey =dataReader["ItmVendorKey"] == DBNull.Value ? null : (int?)dataReader["ItmVendorKey"];
            _ItmVendorCurrKey =(int)dataReader["ItmVendorCurrKey"];
            _ItmVendorPrice =(decimal)dataReader["ItmVendorPrice"];
            _ItmVendorPriceRatio = (decimal)dataReader["ItmVendorPriceRatio"];
            _ItmVendorPriceLock =dataReader["ItmVendorPriceLock"] == DBNull.Value ? false : (bool)dataReader["ItmVendorPriceLock"];
            _ItmJobKey =(int)dataReader["ItmJobKey"];
            _ItmJobPhaseKey = (int)dataReader["ItmJobPhaseKey"];
            _ItmJobTaskKey = (int)dataReader["ItmJobTaskKey"];
            _ItmJobCostTypeKey = (int)dataReader["ItmJobCostTypeKey"];
            _ItmIGrpDItm =(int)dataReader["ItmIGrpDItm"];
            _ItmIGrpQtyLock =dataReader["ItmIGrpQtyLock"] == DBNull.Value ? false : (bool)dataReader["ItmIGrpQtyLock"];
            _ItmIGrpToPrint =dataReader["ItmIGrpToPrint"] == DBNull.Value ? false : (bool)dataReader["ItmIGrpToPrint"];
            _ItmIGrpQtySet = (decimal)dataReader["ItmIGrpQtySet"];
            _ItmIGrpAmtSet = (decimal)dataReader["ItmIGrpAmtSet"];
            _ItmAttachment =dataReader["ItmAttachment"] == DBNull.Value ? false : (bool)dataReader["ItmAttachment"];
            _NSLink =dataReader["NSLink"] == DBNull.Value ? string.Empty:dataReader["NSLink"].ToString();
            _ARQOID =dataReader["ARQOID"] == DBNull.Value ? string.Empty:dataReader["ARQOID"].ToString();
            _ARQODK = (int)dataReader["ARQODK"];
            _ARQODItm = (int)dataReader["ARQODItm"];
            _CreateDate = (DateTime)dataReader["CreateDate"];
            _CreateUserKey = (int)dataReader["CreateUserKey"];
            _LastModifiedDate = (DateTime)dataReader["LastModifiedDate"];
            _LastModifiedUserKey =(int)dataReader["LastModifiedUserKey"];
            _Custom1 =dataReader["Custom1"] == DBNull.Value ? string.Empty:dataReader["Custom1"].ToString();
            _Custom2 =dataReader["Custom2"] == DBNull.Value ? string.Empty:dataReader["Custom2"].ToString();
            _Custom3 =dataReader["Custom3"] == DBNull.Value ? string.Empty:dataReader["Custom3"].ToString();
            _APPOID = dataReader["APPOID"] == DBNull.Value ? string.Empty : dataReader["APPOID"].ToString();

            msgID = string.Empty;
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
				cm.CommandText = "ARSODetItm_AddUpdate";

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
				if (_LineType == null)
					cm.Parameters.AddWithValue("@LineType", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LineType", _LineType);
				if (_LineLinkKey == null)
					cm.Parameters.AddWithValue("@LineLinkKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LineLinkKey", _LineLinkKey);
				if (_ItmSN == null)
					cm.Parameters.AddWithValue("@ItmSN", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmSN", _ItmSN);
				if (_ItmKey == null)
					cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmKey", _ItmKey);
				if (_ItmKeySelect == null)
					cm.Parameters.AddWithValue("@ItmKeySelect", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmKeySelect", _ItmKeySelect);
				if (_ItmType == null)
					cm.Parameters.AddWithValue("@ItmType", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmType", _ItmType);
				if (_ItmDes == null)
					cm.Parameters.AddWithValue("@ItmDes", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmDes", _ItmDes);
				if (_ItmDeptKey == null)
					cm.Parameters.AddWithValue("@ItmDeptKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmDeptKey", _ItmDeptKey);
				if (_ItmTranGrpKey == null)
					cm.Parameters.AddWithValue("@ItmTranGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmTranGrpKey", _ItmTranGrpKey);
				if (_ItmAccKey == null)
					cm.Parameters.AddWithValue("@ItmAccKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmAccKey", _ItmAccKey);
				if (_ItmLocKey == null)
					cm.Parameters.AddWithValue("@ItmLocKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmLocKey", _ItmLocKey);
				if (_ItmStock == null)
					cm.Parameters.AddWithValue("@ItmStock", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmStock", _ItmStock);
				if (_ItmQty == null)
					cm.Parameters.AddWithValue("@ItmQty", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmQty", _ItmQty);
				if (_ItmQtyLink == null)
					cm.Parameters.AddWithValue("@ItmQtyLink", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmQtyLink", _ItmQtyLink);
				if (_ItmQtyAdj == null)
					cm.Parameters.AddWithValue("@ItmQtyAdj", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmQtyAdj", _ItmQtyAdj);
				if (_ItmOrderStatus == null)
					cm.Parameters.AddWithValue("@ItmOrderStatus", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmOrderStatus", _ItmOrderStatus);
				if (_ItmReqDate == null)
					cm.Parameters.AddWithValue("@ItmReqDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmReqDate", _ItmReqDate);
				if (_ItmPrmDate == null)
					cm.Parameters.AddWithValue("@ItmPrmDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmPrmDate", _ItmPrmDate);
				if (_ItmUOMKey == null)
					cm.Parameters.AddWithValue("@ItmUOMKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmUOMKey", _ItmUOMKey);
				if (_ItmConRate == null)
					cm.Parameters.AddWithValue("@ItmConRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmConRate", _ItmConRate);
				if (_ItmLatestCostF == null)
					cm.Parameters.AddWithValue("@ItmLatestCostF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmLatestCostF", _ItmLatestCostF);
				if (_ItmLatestCostH == null)
					cm.Parameters.AddWithValue("@ItmLatestCostH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmLatestCostH", _ItmLatestCostH);
				if (_ItmListPrice == null)
					cm.Parameters.AddWithValue("@ItmListPrice", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmListPrice", _ItmListPrice);
				if (_ItmPriceBefore == null)
					cm.Parameters.AddWithValue("@ItmPriceBefore", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmPriceBefore", _ItmPriceBefore);
                if (_ItmPriceAfter == null)
                    cm.Parameters.AddWithValue("@ItmPriceAfter", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmPriceAfter", _ItmPriceAfter);
				if (_ItmDisPercent == null)
					cm.Parameters.AddWithValue("@ItmDisPercent", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmDisPercent", _ItmDisPercent);
				if (_ItmDisValue == null)
					cm.Parameters.AddWithValue("@ItmDisValue", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmDisValue", _ItmDisValue);
				if (_ItmPrice == null)
					cm.Parameters.AddWithValue("@ItmPrice", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmPrice", _ItmPrice);
				if (_ItmPriceUser == null)
					cm.Parameters.AddWithValue("@ItmPriceUser", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmPriceUser", _ItmPriceUser);
				if (_ItmControlPrice == null)
					cm.Parameters.AddWithValue("@ItmControlPrice", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmControlPrice", _ItmControlPrice);
				if (_ItmControlPriceBase == null)
					cm.Parameters.AddWithValue("@ItmControlPriceBase", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmControlPriceBase", _ItmControlPriceBase);
				if (_ItmAmtShw == null)
					cm.Parameters.AddWithValue("@ItmAmtShw", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmAmtShw", _ItmAmtShw);
				if (_ItmAmtF == null)
					cm.Parameters.AddWithValue("@ItmAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmAmtF", _ItmAmtF);
				if (_ItmAmtH == null)
					cm.Parameters.AddWithValue("@ItmAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmAmtH", _ItmAmtH);
				if (_ItmTaxable == null)
					cm.Parameters.AddWithValue("@ItmTaxable", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmTaxable", _ItmTaxable);
				if (_ItmTaxGrpKey == null)
					cm.Parameters.AddWithValue("@ItmTaxGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmTaxGrpKey", _ItmTaxGrpKey);
				if (_ItmTaxGrpRate == null)
					cm.Parameters.AddWithValue("@ItmTaxGrpRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmTaxGrpRate", _ItmTaxGrpRate);
				if (_ItmTaxGrpAmtF == null)
					cm.Parameters.AddWithValue("@ItmTaxGrpAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmTaxGrpAmtF", _ItmTaxGrpAmtF);
				if (_ItmTaxGrpAmtL == null)
					cm.Parameters.AddWithValue("@ItmTaxGrpAmtL", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmTaxGrpAmtL", _ItmTaxGrpAmtL);
				if (_ItmHide == null)
					cm.Parameters.AddWithValue("@ItmHide", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmHide", _ItmHide);
				if (_ItmColorKey == null)
					cm.Parameters.AddWithValue("@ItmColorKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmColorKey", _ItmColorKey);
				if (_ItmScaleSize == null)
					cm.Parameters.AddWithValue("@ItmScaleSize", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmScaleSize", _ItmScaleSize);
				if (_ItmPacking == null)
					cm.Parameters.AddWithValue("@ItmPacking", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmPacking", _ItmPacking);
				if (_ItmRem == null)
					cm.Parameters.AddWithValue("@ItmRem", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmRem", _ItmRem);
				if (_ItmMark == null)
					cm.Parameters.AddWithValue("@ItmMark", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmMark", _ItmMark);
				if (_ItmVendorKey == null)
					cm.Parameters.AddWithValue("@ItmVendorKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmVendorKey", _ItmVendorKey);
				if (_ItmVendorCurrKey == null)
					cm.Parameters.AddWithValue("@ItmVendorCurrKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmVendorCurrKey", _ItmVendorCurrKey);
				if (_ItmVendorCurrRate == null)
					cm.Parameters.AddWithValue("@ItmVendorCurrRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmVendorCurrRate", _ItmVendorCurrRate);
				if (_ItmVendorPrice == null)
					cm.Parameters.AddWithValue("@ItmVendorPrice", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmVendorPrice", _ItmVendorPrice);
				if (_ItmVendorPriceRatio == null)
					cm.Parameters.AddWithValue("@ItmVendorPriceRatio", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmVendorPriceRatio", _ItmVendorPriceRatio);
				if (_ItmVendorPriceLock == null)
					cm.Parameters.AddWithValue("@ItmVendorPriceLock", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmVendorPriceLock", _ItmVendorPriceLock);
				if (_ItmJobKey == null)
					cm.Parameters.AddWithValue("@ItmJobKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmJobKey", _ItmJobKey);
				if (_ItmJobPhaseKey == null)
					cm.Parameters.AddWithValue("@ItmJobPhaseKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmJobPhaseKey", _ItmJobPhaseKey);
				if (_ItmJobTaskKey == null)
					cm.Parameters.AddWithValue("@ItmJobTaskKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmJobTaskKey", _ItmJobTaskKey);
				if (_ItmJobCostTypeKey == null)
					cm.Parameters.AddWithValue("@ItmJobCostTypeKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmJobCostTypeKey", _ItmJobCostTypeKey);
				if (_ItmIGrpDItm == null)
					cm.Parameters.AddWithValue("@ItmIGrpDItm", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmIGrpDItm", _ItmIGrpDItm);
				if (_ItmIGrpQtyLock == null)
					cm.Parameters.AddWithValue("@ItmIGrpQtyLock", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmIGrpQtyLock", _ItmIGrpQtyLock);
				if (_ItmIGrpToPrint == null)
					cm.Parameters.AddWithValue("@ItmIGrpToPrint", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmIGrpToPrint", _ItmIGrpToPrint);
				if (_ItmIGrpQtySet == null)
					cm.Parameters.AddWithValue("@ItmIGrpQtySet", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmIGrpQtySet", _ItmIGrpQtySet);
				if (_ItmIGrpAmtSet == null)
					cm.Parameters.AddWithValue("@ItmIGrpAmtSet", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmIGrpAmtSet", _ItmIGrpAmtSet);
				if (_ItmAttachment == null)
					cm.Parameters.AddWithValue("@ItmAttachment", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmAttachment", _ItmAttachment);
				if (_NSLink == null)
					cm.Parameters.AddWithValue("@NSLink", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@NSLink", _NSLink);
				if (_ARQOID == null)
					cm.Parameters.AddWithValue("@ARQOID", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ARQOID", _ARQOID);
				if (_ARQODK == null)
					cm.Parameters.AddWithValue("@ARQODK", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ARQODK", _ARQODK);
				if (_ARQODItm == null)
					cm.Parameters.AddWithValue("@ARQODItm", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ARQODItm", _ARQODItm);
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
				cm.CommandText = "ARSODetItm_AddUpdate";

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
				if (_LineType == null)
					cm.Parameters.AddWithValue("@LineType", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LineType", _LineType);
				if (_LineLinkKey == null)
					cm.Parameters.AddWithValue("@LineLinkKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@LineLinkKey", _LineLinkKey);
				if (_ItmSN == null)
					cm.Parameters.AddWithValue("@ItmSN", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmSN", _ItmSN);
				if (_ItmKey == null)
					cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmKey", _ItmKey);
				if (_ItmKeySelect == null)
					cm.Parameters.AddWithValue("@ItmKeySelect", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmKeySelect", _ItmKeySelect);
				if (_ItmType == null)
					cm.Parameters.AddWithValue("@ItmType", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmType", _ItmType);
				if (_ItmDes == null)
					cm.Parameters.AddWithValue("@ItmDes", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmDes", _ItmDes);
				if (_ItmDeptKey == null)
					cm.Parameters.AddWithValue("@ItmDeptKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmDeptKey", _ItmDeptKey);
				if (_ItmTranGrpKey == null)
					cm.Parameters.AddWithValue("@ItmTranGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmTranGrpKey", _ItmTranGrpKey);
				if (_ItmAccKey == null)
					cm.Parameters.AddWithValue("@ItmAccKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmAccKey", _ItmAccKey);
				if (_ItmLocKey == null)
					cm.Parameters.AddWithValue("@ItmLocKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmLocKey", _ItmLocKey);
				if (_ItmStock == null)
					cm.Parameters.AddWithValue("@ItmStock", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmStock", _ItmStock);
				if (_ItmQty == null)
					cm.Parameters.AddWithValue("@ItmQty", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmQty", _ItmQty);
				if (_ItmQtyLink == null)
					cm.Parameters.AddWithValue("@ItmQtyLink", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmQtyLink", _ItmQtyLink);
				if (_ItmQtyAdj == null)
					cm.Parameters.AddWithValue("@ItmQtyAdj", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmQtyAdj", _ItmQtyAdj);
				if (_ItmOrderStatus == null)
					cm.Parameters.AddWithValue("@ItmOrderStatus", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmOrderStatus", _ItmOrderStatus);
				if (_ItmReqDate == null)
					cm.Parameters.AddWithValue("@ItmReqDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmReqDate", _ItmReqDate);
				if (_ItmPrmDate == null)
					cm.Parameters.AddWithValue("@ItmPrmDate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmPrmDate", _ItmPrmDate);
				if (_ItmUOMKey == null)
					cm.Parameters.AddWithValue("@ItmUOMKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmUOMKey", _ItmUOMKey);
				if (_ItmConRate == null)
					cm.Parameters.AddWithValue("@ItmConRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmConRate", _ItmConRate);
				if (_ItmLatestCostF == null)
					cm.Parameters.AddWithValue("@ItmLatestCostF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmLatestCostF", _ItmLatestCostF);
				if (_ItmLatestCostH == null)
					cm.Parameters.AddWithValue("@ItmLatestCostH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmLatestCostH", _ItmLatestCostH);
				if (_ItmListPrice == null)
					cm.Parameters.AddWithValue("@ItmListPrice", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmListPrice", _ItmListPrice);
				if (_ItmPriceBefore == null)
					cm.Parameters.AddWithValue("@ItmPriceBefore", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmPriceBefore", _ItmPriceBefore);
                if (_ItmPriceAfter == null)
                    cm.Parameters.AddWithValue("@ItmPriceAfter", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmPriceAfter", _ItmPriceAfter);
				if (_ItmDisPercent == null)
					cm.Parameters.AddWithValue("@ItmDisPercent", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmDisPercent", _ItmDisPercent);
				if (_ItmDisValue == null)
					cm.Parameters.AddWithValue("@ItmDisValue", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmDisValue", _ItmDisValue);
				if (_ItmPrice == null)
					cm.Parameters.AddWithValue("@ItmPrice", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmPrice", _ItmPrice);
				if (_ItmPriceUser == null)
					cm.Parameters.AddWithValue("@ItmPriceUser", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmPriceUser", _ItmPriceUser);
				if (_ItmControlPrice == null)
					cm.Parameters.AddWithValue("@ItmControlPrice", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmControlPrice", _ItmControlPrice);
				if (_ItmControlPriceBase == null)
					cm.Parameters.AddWithValue("@ItmControlPriceBase", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmControlPriceBase", _ItmControlPriceBase);
				if (_ItmAmtShw == null)
					cm.Parameters.AddWithValue("@ItmAmtShw", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmAmtShw", _ItmAmtShw);
				if (_ItmAmtF == null)
					cm.Parameters.AddWithValue("@ItmAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmAmtF", _ItmAmtF);
				if (_ItmAmtH == null)
					cm.Parameters.AddWithValue("@ItmAmtH", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmAmtH", _ItmAmtH);
				if (_ItmTaxable == null)
					cm.Parameters.AddWithValue("@ItmTaxable", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmTaxable", _ItmTaxable);
				if (_ItmTaxGrpKey == null)
					cm.Parameters.AddWithValue("@ItmTaxGrpKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmTaxGrpKey", _ItmTaxGrpKey);
				if (_ItmTaxGrpRate == null)
					cm.Parameters.AddWithValue("@ItmTaxGrpRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmTaxGrpRate", _ItmTaxGrpRate);
				if (_ItmTaxGrpAmtF == null)
					cm.Parameters.AddWithValue("@ItmTaxGrpAmtF", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmTaxGrpAmtF", _ItmTaxGrpAmtF);
				if (_ItmTaxGrpAmtL == null)
					cm.Parameters.AddWithValue("@ItmTaxGrpAmtL", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmTaxGrpAmtL", _ItmTaxGrpAmtL);
				if (_ItmHide == null)
					cm.Parameters.AddWithValue("@ItmHide", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmHide", _ItmHide);
				if (_ItmColorKey == null)
					cm.Parameters.AddWithValue("@ItmColorKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmColorKey", _ItmColorKey);
				if (_ItmScaleSize == null)
					cm.Parameters.AddWithValue("@ItmScaleSize", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmScaleSize", _ItmScaleSize);
				if (_ItmPacking == null)
					cm.Parameters.AddWithValue("@ItmPacking", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmPacking", _ItmPacking);
				if (_ItmRem == null)
					cm.Parameters.AddWithValue("@ItmRem", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmRem", _ItmRem);
				if (_ItmMark == null)
					cm.Parameters.AddWithValue("@ItmMark", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmMark", _ItmMark);
				if (_ItmVendorKey == null)
					cm.Parameters.AddWithValue("@ItmVendorKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmVendorKey", _ItmVendorKey);
				if (_ItmVendorCurrKey == null)
					cm.Parameters.AddWithValue("@ItmVendorCurrKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmVendorCurrKey", _ItmVendorCurrKey);
				if (_ItmVendorCurrRate == null)
					cm.Parameters.AddWithValue("@ItmVendorCurrRate", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmVendorCurrRate", _ItmVendorCurrRate);
				if (_ItmVendorPrice == null)
					cm.Parameters.AddWithValue("@ItmVendorPrice", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmVendorPrice", _ItmVendorPrice);
				if (_ItmVendorPriceRatio == null)
					cm.Parameters.AddWithValue("@ItmVendorPriceRatio", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmVendorPriceRatio", _ItmVendorPriceRatio);
				if (_ItmVendorPriceLock == null)
					cm.Parameters.AddWithValue("@ItmVendorPriceLock", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmVendorPriceLock", _ItmVendorPriceLock);
				if (_ItmJobKey == null)
					cm.Parameters.AddWithValue("@ItmJobKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmJobKey", _ItmJobKey);
				if (_ItmJobPhaseKey == null)
					cm.Parameters.AddWithValue("@ItmJobPhaseKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmJobPhaseKey", _ItmJobPhaseKey);
				if (_ItmJobTaskKey == null)
					cm.Parameters.AddWithValue("@ItmJobTaskKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmJobTaskKey", _ItmJobTaskKey);
				if (_ItmJobCostTypeKey == null)
					cm.Parameters.AddWithValue("@ItmJobCostTypeKey", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmJobCostTypeKey", _ItmJobCostTypeKey);
				if (_ItmIGrpDItm == null)
					cm.Parameters.AddWithValue("@ItmIGrpDItm", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmIGrpDItm", _ItmIGrpDItm);
				if (_ItmIGrpQtyLock == null)
					cm.Parameters.AddWithValue("@ItmIGrpQtyLock", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmIGrpQtyLock", _ItmIGrpQtyLock);
				if (_ItmIGrpToPrint == null)
					cm.Parameters.AddWithValue("@ItmIGrpToPrint", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmIGrpToPrint", _ItmIGrpToPrint);
				if (_ItmIGrpQtySet == null)
					cm.Parameters.AddWithValue("@ItmIGrpQtySet", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmIGrpQtySet", _ItmIGrpQtySet);
				if (_ItmIGrpAmtSet == null)
					cm.Parameters.AddWithValue("@ItmIGrpAmtSet", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmIGrpAmtSet", _ItmIGrpAmtSet);
				if (_ItmAttachment == null)
					cm.Parameters.AddWithValue("@ItmAttachment", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ItmAttachment", _ItmAttachment);
				if (_NSLink == null)
					cm.Parameters.AddWithValue("@NSLink", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@NSLink", _NSLink);
				if (_ARQOID == null)
					cm.Parameters.AddWithValue("@ARQOID", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ARQOID", _ARQOID);
				if (_ARQODK == null)
					cm.Parameters.AddWithValue("@ARQODK", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ARQODK", _ARQODK);
				if (_ARQODItm == null)
					cm.Parameters.AddWithValue("@ARQODItm", DBNull.Value);
				else
					cm.Parameters.AddWithValue("@ARQODItm", _ARQODItm);
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
			bool retValue = false;
			msgID = "RecordDeleteFail";
			using(SqlCommand cm = cn.CreateCommand())
			{
				cm.CommandType = CommandType.StoredProcedure;
				cm.CommandText = "ARSODetItm_Delete";

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
	}
}