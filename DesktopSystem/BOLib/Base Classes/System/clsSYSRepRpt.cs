using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

namespace BOLib
{
	/// <summary>
	/// Summary description for SYSRepRpt.
	/// </summary>
	[Serializable]
	public class SYSRepRpt : INotifyPropertyChanged
	{
		#region +++  Local variables declaration for the class +++

		private int? _UID;
		private int? _RepKey;
		private string _RptNm;
		private string _RptDes;
		private string _RptDesLang1;
		private string _RptDesLang2;
		private string _RptDesLang3;
		private string _RptDesLang4;
		private string _RptDesLang5;
		private string _RptDesLang6;
		private string _RptDesLang7;
		private string _RptDesLang8;
		private string _RptDesLang9;
		private string _RptDesLang10;
		private string _RptTitle;
		private string _RptTitleLang1;
		private string _RptTitleLang2;
		private string _RptTitleLang3;
		private string _RptTitleLang4;
		private string _RptTitleLang5;
		private string _RptTitleLang6;
		private string _RptTitleLang7;
		private string _RptTitleLang8;
		private string _RptTitleLang9;
		private string _RptTitleLang10;
		private int? _RptLayOut;
		private string _RptAltRecordSource;
		private string _RptPermission;
		private string _RptPrintCondition;
		private bool _ShwItmCount;
		private bool _ShwLetterHead;
		private short? _PrtCopies;
        private string _RptOrderBy;
		private DateTime? _CreateDate;
		private int? _CreateUserKey;
		private DateTime? _LastModifiedDate;
		private int? _LastModifiedUserKey;
		private string _Custom1;
		private string _Custom2;
		private string _Custom3;
		private bool _isDirty;
        private int? _BuildIn;

		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region +++  Constructor and destructor codes  +++

		/// <summary>
		/// Default constructor that will initialize all properties with default values.
		/// </summary>

		public SYSRepRpt()
		{
			this._UID = null;
			this._RepKey = null;
			this._RptNm = string.Empty;
			this._RptDes = string.Empty;
			this._RptDesLang1 = string.Empty;
			this._RptDesLang2 = string.Empty;
			this._RptDesLang3 = string.Empty;
			this._RptDesLang4 = string.Empty;
			this._RptDesLang5 = string.Empty;
			this._RptDesLang6 = string.Empty;
			this._RptDesLang7 = string.Empty;
			this._RptDesLang8 = string.Empty;
			this._RptDesLang9 = string.Empty;
			this._RptDesLang10 = string.Empty;
			this._RptTitle = string.Empty;
			this._RptTitleLang1 = string.Empty;
			this._RptTitleLang2 = string.Empty;
			this._RptTitleLang3 = string.Empty;
			this._RptTitleLang4 = string.Empty;
			this._RptTitleLang5 = string.Empty;
			this._RptTitleLang6 = string.Empty;
			this._RptTitleLang7 = string.Empty;
			this._RptTitleLang8 = string.Empty;
			this._RptTitleLang9 = string.Empty;
			this._RptTitleLang10 = string.Empty;
			this._RptLayOut = null;
			this._RptAltRecordSource = string.Empty;
			this._RptPermission = string.Empty;
			this._RptPrintCondition = string.Empty;
			this._ShwItmCount = false;
			this._ShwLetterHead = false;
			this._PrtCopies = null;
            this._RptOrderBy = "";
			this._CreateDate = null;
			this._CreateUserKey = null;
			this._LastModifiedDate = null;
			this._LastModifiedUserKey = null;
			this._Custom1 = string.Empty;
			this._Custom2 = string.Empty;
			this._Custom3 = string.Empty;
			this._isDirty = false;
		}


		public SYSRepRpt Clone()
		{

			SYSRepRpt objCopy= (SYSRepRpt) this.MemberwiseClone();
			objCopy._isDirty=false;
			return objCopy;
		}

        public static SYSRepRpt Get(int? UID,int Option)
        {
            SYSRepRpt child = new SYSRepRpt();
            child.Fetch(new Criteria(UID, Option));
            return child;
        }
        public static SYSRepRpt Get(int? RepKey,string RptNm, int Option)
        {
            SYSRepRpt child = new SYSRepRpt();
            child.Fetch(new Criteria(RepKey,RptNm, Option));
            return child;
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

		public int? UID
		{
			get
			{
				return this._UID;
			}
			set
			{
				this._UID = value;
				NotifyPropertyChanged("UID");
			}
		}

		public int? RepKey
		{
			get
			{
				return this._RepKey;
			}
			set
			{
				this._RepKey = value;
				NotifyPropertyChanged("RepKey");
			}
		}

		public string RptNm
		{
			get
			{
				return this._RptNm;
			}
			set
			{
				this._RptNm = value;
				NotifyPropertyChanged("RptNm");
			}
		}

		public string RptDes
		{
			get
			{
				return this._RptDes;
			}
			set
			{
				this._RptDes = value;
				NotifyPropertyChanged("RptDes");
			}
		}

		public string RptDesLang1
		{
			get
			{
				return this._RptDesLang1;
			}
			set
			{
				this._RptDesLang1 = value;
				NotifyPropertyChanged("RptDesLang1");
			}
		}

		public string RptDesLang2
		{
			get
			{
				return this._RptDesLang2;
			}
			set
			{
				this._RptDesLang2 = value;
				NotifyPropertyChanged("RptDesLang2");
			}
		}

		public string RptDesLang3
		{
			get
			{
				return this._RptDesLang3;
			}
			set
			{
				this._RptDesLang3 = value;
				NotifyPropertyChanged("RptDesLang3");
			}
		}

		public string RptDesLang4
		{
			get
			{
				return this._RptDesLang4;
			}
			set
			{
				this._RptDesLang4 = value;
				NotifyPropertyChanged("RptDesLang4");
			}
		}

		public string RptDesLang5
		{
			get
			{
				return this._RptDesLang5;
			}
			set
			{
				this._RptDesLang5 = value;
				NotifyPropertyChanged("RptDesLang5");
			}
		}

		public string RptDesLang6
		{
			get
			{
				return this._RptDesLang6;
			}
			set
			{
				this._RptDesLang6 = value;
				NotifyPropertyChanged("RptDesLang6");
			}
		}

		public string RptDesLang7
		{
			get
			{
				return this._RptDesLang7;
			}
			set
			{
				this._RptDesLang7 = value;
				NotifyPropertyChanged("RptDesLang7");
			}
		}

		public string RptDesLang8
		{
			get
			{
				return this._RptDesLang8;
			}
			set
			{
				this._RptDesLang8 = value;
				NotifyPropertyChanged("RptDesLang8");
			}
		}

		public string RptDesLang9
		{
			get
			{
				return this._RptDesLang9;
			}
			set
			{
				this._RptDesLang9 = value;
				NotifyPropertyChanged("RptDesLang9");
			}
		}

		public string RptDesLang10
		{
			get
			{
				return this._RptDesLang10;
			}
			set
			{
				this._RptDesLang10 = value;
				NotifyPropertyChanged("RptDesLang10");
			}
		}

		public string RptTitle
		{
			get
			{
				return this._RptTitle;
			}
			set
			{
				this._RptTitle = value;
				NotifyPropertyChanged("RptTitle");
			}
		}

		public string RptTitleLang1
		{
			get
			{
				return this._RptTitleLang1;
			}
			set
			{
				this._RptTitleLang1 = value;
				NotifyPropertyChanged("RptTitleLang1");
			}
		}

		public string RptTitleLang2
		{
			get
			{
				return this._RptTitleLang2;
			}
			set
			{
				this._RptTitleLang2 = value;
				NotifyPropertyChanged("RptTitleLang2");
			}
		}

		public string RptTitleLang3
		{
			get
			{
				return this._RptTitleLang3;
			}
			set
			{
				this._RptTitleLang3 = value;
				NotifyPropertyChanged("RptTitleLang3");
			}
		}

		public string RptTitleLang4
		{
			get
			{
				return this._RptTitleLang4;
			}
			set
			{
				this._RptTitleLang4 = value;
				NotifyPropertyChanged("RptTitleLang4");
			}
		}

		public string RptTitleLang5
		{
			get
			{
				return this._RptTitleLang5;
			}
			set
			{
				this._RptTitleLang5 = value;
				NotifyPropertyChanged("RptTitleLang5");
			}
		}

		public string RptTitleLang6
		{
			get
			{
				return this._RptTitleLang6;
			}
			set
			{
				this._RptTitleLang6 = value;
				NotifyPropertyChanged("RptTitleLang6");
			}
		}

		public string RptTitleLang7
		{
			get
			{
				return this._RptTitleLang7;
			}
			set
			{
				this._RptTitleLang7 = value;
				NotifyPropertyChanged("RptTitleLang7");
			}
		}

		public string RptTitleLang8
		{
			get
			{
				return this._RptTitleLang8;
			}
			set
			{
				this._RptTitleLang8 = value;
				NotifyPropertyChanged("RptTitleLang8");
			}
		}

		public string RptTitleLang9
		{
			get
			{
				return this._RptTitleLang9;
			}
			set
			{
				this._RptTitleLang9 = value;
				NotifyPropertyChanged("RptTitleLang9");
			}
		}

		public string RptTitleLang10
		{
			get
			{
				return this._RptTitleLang10;
			}
			set
			{
				this._RptTitleLang10 = value;
				NotifyPropertyChanged("RptTitleLang10");
			}
		}

		public int? RptLayOut
		{
			get
			{
				return this._RptLayOut;
			}
			set
			{
				this._RptLayOut = value;
				NotifyPropertyChanged("RptLayOut");
			}
		}

		public string RptAltRecordSource
		{
			get
			{
				return this._RptAltRecordSource;
			}
			set
			{
				this._RptAltRecordSource = value;
				NotifyPropertyChanged("RptAltRecordSource");
			}
		}

		public string RptPermission
		{
			get
			{
				return this._RptPermission;
			}
			set
			{
				this._RptPermission = value;
				NotifyPropertyChanged("RptPermission");
			}
		}

		public string RptPrintCondition
		{
			get
			{
				return this._RptPrintCondition;
			}
			set
			{
				this._RptPrintCondition = value;
				NotifyPropertyChanged("RptPrintCondition");
			}
		}

		public bool ShwItmCount
		{
			get
			{
				return this._ShwItmCount;
			}
			set
			{
				this._ShwItmCount = value;
				NotifyPropertyChanged("ShwItmCount");
			}
		}

		public bool ShwLetterHead
		{
			get
			{
				return this._ShwLetterHead;
			}
			set
			{
				this._ShwLetterHead = value;
				NotifyPropertyChanged("ShwLetterHead");
			}
		}

		public short? PrtCopies
		{
			get
			{
				return this._PrtCopies;
			}
			set
			{
				this._PrtCopies = value;
				NotifyPropertyChanged("PrtCopies");
			}
		}

        public string RptOrderBy
        {
            get
            {
                return this._RptOrderBy;
            }
            set
            {
                this._RptOrderBy = value;
                NotifyPropertyChanged("RptOrderBy");
            }
        }

		public DateTime? CreateDate
		{
			get
			{
				return this._CreateDate;
			}
			set
			{
				this._CreateDate = value;
				NotifyPropertyChanged("CreateDate");
			}
		}

		public int? CreateUserKey
		{
			get
			{
				return this._CreateUserKey;
			}
			set
			{
				this._CreateUserKey = value;
				NotifyPropertyChanged("CreateUserKey");
			}
		}

		public DateTime? LastModifiedDate
		{
			get
			{
				return this._LastModifiedDate;
			}
			set
			{
				this._LastModifiedDate = value;
				NotifyPropertyChanged("LastModifiedDate");
			}
		}

		public int? LastModifiedUserKey
		{
			get
			{
				return this._LastModifiedUserKey;
			}
			set
			{
				this._LastModifiedUserKey = value;
				NotifyPropertyChanged("LastModifiedUserKey");
			}
		}

		public string Custom1
		{
			get
			{
				return this._Custom1;
			}
			set
			{
				this._Custom1 = value;
				NotifyPropertyChanged("Custom1");
			}
		}

		public string Custom2
		{
			get
			{
				return this._Custom2;
			}
			set
			{
				this._Custom2 = value;
				NotifyPropertyChanged("Custom2");
			}
		}

		public string Custom3
		{
			get
			{
				return this._Custom3;
			}
			set
			{
				this._Custom3 = value;
				NotifyPropertyChanged("Custom3");
			}
		}

        public int? BuildIn
        {
            get
            {
                return this._BuildIn;
            }
            set
            {
                this._BuildIn = value;
                NotifyPropertyChanged("BuildIn");
            }
        }

		public bool IsDirty
		{
			get
			{
				return this._isDirty;
			}
		}


		#endregion

		#region Criteria
		[Serializable()]
		public class Criteria
		{
			public int? _UID = null;
			public int? _option = null;
            public string _rptNm = "";
            public int? _RepKey = null;

			internal Criteria()
			{
			}
			internal Criteria(int? UID)
			{
				_UID = UID;
			}
			public Criteria(int? UID, int? Option)
			{
				_UID = UID;
				_option = Option;
			}
            internal Criteria(int? RepKey, string RptNm, int? Option)
            {
                _RepKey = RepKey;
                _rptNm = RptNm;
                _option = Option;
            }
		}
		#endregion //Criteria

		#region Data Access - Fetch
		public bool Fetch(Criteria criteria)
		{
			bool retValue = false;
	
			// Create new sql connection for this method. 
			using (SqlConnection cn = new SqlConnection(BOLib.Database.BossDemoConnection))
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
				cm.CommandText = "SYSRepRpt_Get";

				cm.Parameters.AddWithValue("@Option", criteria._option);				
				cm.Parameters.AddWithValue("@RepKey", criteria._RepKey);
                cm.Parameters.AddWithValue("@UID", criteria._UID);
                cm.Parameters.AddWithValue("@RptNm", criteria._rptNm);					
				cm.Parameters.AddWithValue("@RetValue", 0);
				cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
				// Using data reader as record set.
				using (IDataReader dr = cm.ExecuteReader())
				{
					//If data reader can read, continue...
					while (dr.Read())
					{
						retValue = this.Fetch(dr);
					}
				}// Already close and dispose data reader.
				
			
                if ((int)cm.Parameters["@RetValue"].Value == 1)
				    retValue = true;
			}// Already close and dispose sql connection.
			return retValue;
		}
		internal static SYSRepRpt Get(IDataReader dr)
		{
			
			SYSRepRpt child = new SYSRepRpt();
			child.Fetch(dr);
			return child;
		}

		internal bool Fetch(IDataReader dataReader)
		{
			
			// Fill data to entity object
			_UID =dataReader["UID"] == DBNull.Value ? null : (int?)dataReader["UID"];
            _RepKey =dataReader["RepKey"] == DBNull.Value ? null : (int?)dataReader["RepKey"];
            _RptNm =dataReader["RptNm"] == DBNull.Value ? string.Empty:dataReader["RptNm"].ToString();
            _RptDes =dataReader["RptDes"] == DBNull.Value ? string.Empty:dataReader["RptDes"].ToString();
            _RptDesLang1 =dataReader["RptDesLang1"] == DBNull.Value ? string.Empty:dataReader["RptDesLang1"].ToString();
            _RptDesLang2 =dataReader["RptDesLang2"] == DBNull.Value ? string.Empty:dataReader["RptDesLang2"].ToString();
            _RptDesLang3 =dataReader["RptDesLang3"] == DBNull.Value ? string.Empty:dataReader["RptDesLang3"].ToString();
            _RptDesLang4 =dataReader["RptDesLang4"] == DBNull.Value ? string.Empty:dataReader["RptDesLang4"].ToString();
            _RptDesLang5 =dataReader["RptDesLang5"] == DBNull.Value ? string.Empty:dataReader["RptDesLang5"].ToString();
            _RptDesLang6 =dataReader["RptDesLang6"] == DBNull.Value ? string.Empty:dataReader["RptDesLang6"].ToString();
            _RptDesLang7 =dataReader["RptDesLang7"] == DBNull.Value ? string.Empty:dataReader["RptDesLang7"].ToString();
            _RptDesLang8 =dataReader["RptDesLang8"] == DBNull.Value ? string.Empty:dataReader["RptDesLang8"].ToString();
            _RptDesLang9 =dataReader["RptDesLang9"] == DBNull.Value ? string.Empty:dataReader["RptDesLang9"].ToString();
            _RptDesLang10 =dataReader["RptDesLang10"] == DBNull.Value ? string.Empty:dataReader["RptDesLang10"].ToString();
            _RptTitle =dataReader["RptTitle"] == DBNull.Value ? string.Empty:dataReader["RptTitle"].ToString();
            _RptTitleLang1 =dataReader["RptTitleLang1"] == DBNull.Value ? string.Empty:dataReader["RptTitleLang1"].ToString();
            _RptTitleLang2 =dataReader["RptTitleLang2"] == DBNull.Value ? string.Empty:dataReader["RptTitleLang2"].ToString();
            _RptTitleLang3 =dataReader["RptTitleLang3"] == DBNull.Value ? string.Empty:dataReader["RptTitleLang3"].ToString();
            _RptTitleLang4 =dataReader["RptTitleLang4"] == DBNull.Value ? string.Empty:dataReader["RptTitleLang4"].ToString();
            _RptTitleLang5 =dataReader["RptTitleLang5"] == DBNull.Value ? string.Empty:dataReader["RptTitleLang5"].ToString();
            _RptTitleLang6 =dataReader["RptTitleLang6"] == DBNull.Value ? string.Empty:dataReader["RptTitleLang6"].ToString();
            _RptTitleLang7 =dataReader["RptTitleLang7"] == DBNull.Value ? string.Empty:dataReader["RptTitleLang7"].ToString();
            _RptTitleLang8 =dataReader["RptTitleLang8"] == DBNull.Value ? string.Empty:dataReader["RptTitleLang8"].ToString();
            _RptTitleLang9 =dataReader["RptTitleLang9"] == DBNull.Value ? string.Empty:dataReader["RptTitleLang9"].ToString();
            _RptTitleLang10 =dataReader["RptTitleLang10"] == DBNull.Value ? string.Empty:dataReader["RptTitleLang10"].ToString();
            _RptLayOut =dataReader["RptLayOut"] == DBNull.Value ? 10 : (int?)dataReader["RptLayOut"];
            _RptAltRecordSource =dataReader["RptAltRecordSource"] == DBNull.Value ? string.Empty:dataReader["RptAltRecordSource"].ToString();
            _RptPermission =dataReader["RptPermission"] == DBNull.Value ? string.Empty:dataReader["RptPermission"].ToString();
            _RptPrintCondition =dataReader["RptPrintCondition"] == DBNull.Value ? string.Empty:dataReader["RptPrintCondition"].ToString();
            _ShwItmCount =dataReader["ShwItmCount"] == DBNull.Value ? false : (bool)dataReader["ShwItmCount"];
            _ShwLetterHead =dataReader["ShwLetterHead"] == DBNull.Value ? false : (bool)dataReader["ShwLetterHead"];
            _PrtCopies =dataReader["PrtCopies"] == DBNull.Value ? 0 : (short?)dataReader["PrtCopies"];
            _Custom1 =dataReader["Custom1"] == DBNull.Value ? string.Empty:dataReader["Custom1"].ToString();
            _Custom2 =dataReader["Custom2"] == DBNull.Value ? string.Empty:dataReader["Custom2"].ToString();
            _Custom3 =dataReader["Custom3"] == DBNull.Value ? string.Empty:dataReader["Custom3"].ToString();
            _BuildIn =dataReader["BuildIn"] == DBNull.Value ? 0 : (int?)dataReader["BuildIn"];
            _RptOrderBy =dataReader["RptOrderBy"] == DBNull.Value ? string.Empty:dataReader["RptOrderBy"].ToString();


            //_CreateDate = Convert.ToDateTime(dataReader["CreateDate"]);
            //_CreateUserKey = Convert.ToInt32(dataReader["CreateUserKey"]);
            //_LastModifiedDate = Convert.ToDateTime(dataReader["LastModifiedDate"]);
            //_LastModifiedUserKey = Convert.ToInt32(dataReader["LastModifiedUserKey"]);

            return true;

	    }
		#endregion //Data Access - Fetch

       
	}
}