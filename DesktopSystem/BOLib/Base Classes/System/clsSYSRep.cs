using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
	/// <summary>
	/// Summary description for SYSRep.
	/// </summary>
	[Serializable]
	public class SYSRep : INotifyPropertyChanged
	{
		#region +++  Local variables declaration for the class +++

		private int? _RepKey;
		private int? _RepGrp;
		private string _RepType;
		private string _RepDesLang1;
		private string _RepDesLang2;
		private string _RepDesLang3;
		private string _RepDesLang4;
		private string _RepDesLang5;
		private string _RepDesLang6;
		private string _RepDesLang7;
		private string _RepDesLang8;
		private string _RepDesLang9;
		private string _RepDesLang10;
		private string _RepRemarksLang1;
		private string _RepRemarksLang2;
		private string _RepRemarksLang3;
		private string _RepRemarksLang4;
		private string _RepRemarksLang5;
		private string _RepRemarksLang6;
		private string _RepRemarksLang7;
		private string _RepRemarksLang8;
		private string _RepRemarksLang9;
		private string _RepRemarksLang10;
		private string _FormNm;
		private string _RPTname1;
		private string _RPTname2;
		private string _RPTCaption;
		private string _RPTCaptionLang1;
		private string _RPTCaptionLang2;
		private string _RPTCaptionLang3;
		private string _RPTCaptionLang4;
		private string _RPTCaptionLang5;
		private string _RPTCaptionLang6;
		private string _RPTCaptionLang7;
		private string _RPTCaptionLang8;
		private string _RPTCaptionLang9;
		private string _RPTCaptionLang10;
		private string _RPTRecordSource1;
		private string _RPTRecordSource2;
		private bool _Hidden;
		private DateTime? _CreateDate;
		private int? _CreateUserKey;
		private DateTime? _LastModifiedDate;
		private int? _LastModifiedUserKey;
		private string _Custom1;
		private string _Custom2;
		private string _Custom3;
		private bool _isDirty;

		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region +++  Constructor and destructor codes  +++

		/// <summary>
		/// Default constructor that will initialize all properties with default values.
		/// </summary>

		public SYSRep()
		{
			this._RepKey = null;
			this._RepGrp = null;
			this._RepType = string.Empty;
			this._RepDesLang1 = string.Empty;
			this._RepDesLang2 = string.Empty;
			this._RepDesLang3 = string.Empty;
			this._RepDesLang4 = string.Empty;
			this._RepDesLang5 = string.Empty;
			this._RepDesLang6 = string.Empty;
			this._RepDesLang7 = string.Empty;
			this._RepDesLang8 = string.Empty;
			this._RepDesLang9 = string.Empty;
			this._RepDesLang10 = string.Empty;
			this._RepRemarksLang1 = string.Empty;
			this._RepRemarksLang2 = string.Empty;
			this._RepRemarksLang3 = string.Empty;
			this._RepRemarksLang4 = string.Empty;
			this._RepRemarksLang5 = string.Empty;
			this._RepRemarksLang6 = string.Empty;
			this._RepRemarksLang7 = string.Empty;
			this._RepRemarksLang8 = string.Empty;
			this._RepRemarksLang9 = string.Empty;
			this._RepRemarksLang10 = string.Empty;
			this._FormNm = string.Empty;
			this._RPTname1 = string.Empty;
			this._RPTname2 = string.Empty;
			this._RPTCaption = string.Empty;
			this._RPTCaptionLang1 = string.Empty;
			this._RPTCaptionLang2 = string.Empty;
			this._RPTCaptionLang3 = string.Empty;
			this._RPTCaptionLang4 = string.Empty;
			this._RPTCaptionLang5 = string.Empty;
			this._RPTCaptionLang6 = string.Empty;
			this._RPTCaptionLang7 = string.Empty;
			this._RPTCaptionLang8 = string.Empty;
			this._RPTCaptionLang9 = string.Empty;
			this._RPTCaptionLang10 = string.Empty;
			this._RPTRecordSource1 = string.Empty;
			this._RPTRecordSource2 = string.Empty;
			this._Hidden = false;
			this._CreateDate = null;
			this._CreateUserKey = null;
			this._LastModifiedDate = null;
			this._LastModifiedUserKey = null;
			this._Custom1 = string.Empty;
			this._Custom2 = string.Empty;
			this._Custom3 = string.Empty;
			this._isDirty = false;
		}


		public SYSRep Clone()
		{

			SYSRep objCopy= (SYSRep) this.MemberwiseClone();
			objCopy._isDirty=false;
			return objCopy;
		}

        public  static SYSRep Get(int? repKey)
        {
            SYSRep child = new SYSRep();
            child.Fetch(new Criteria(repKey, 1));
            return child;
        }
        public static SYSRep Get(SqlConnection cn, int? repKey)
        {
            SYSRep child = new SYSRep();
            child.Fetch(cn,new Criteria(repKey, 1));
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

		public int? RepGrp
		{
			get
			{
				return this._RepGrp;
			}
			set
			{
				this._RepGrp = value;
				NotifyPropertyChanged("RepGrp");
			}
		}

		public string RepType
		{
			get
			{
				return this._RepType;
			}
			set
			{
				this._RepType = value;
				NotifyPropertyChanged("RepType");
			}
		}

		public string RepDesLang1
		{
			get
			{
				return this._RepDesLang1;
			}
			set
			{
				this._RepDesLang1 = value;
				NotifyPropertyChanged("RepDesLang1");
			}
		}

		public string RepDesLang2
		{
			get
			{
				return this._RepDesLang2;
			}
			set
			{
				this._RepDesLang2 = value;
				NotifyPropertyChanged("RepDesLang2");
			}
		}

		public string RepDesLang3
		{
			get
			{
				return this._RepDesLang3;
			}
			set
			{
				this._RepDesLang3 = value;
				NotifyPropertyChanged("RepDesLang3");
			}
		}

		public string RepDesLang4
		{
			get
			{
				return this._RepDesLang4;
			}
			set
			{
				this._RepDesLang4 = value;
				NotifyPropertyChanged("RepDesLang4");
			}
		}

		public string RepDesLang5
		{
			get
			{
				return this._RepDesLang5;
			}
			set
			{
				this._RepDesLang5 = value;
				NotifyPropertyChanged("RepDesLang5");
			}
		}

		public string RepDesLang6
		{
			get
			{
				return this._RepDesLang6;
			}
			set
			{
				this._RepDesLang6 = value;
				NotifyPropertyChanged("RepDesLang6");
			}
		}

		public string RepDesLang7
		{
			get
			{
				return this._RepDesLang7;
			}
			set
			{
				this._RepDesLang7 = value;
				NotifyPropertyChanged("RepDesLang7");
			}
		}

		public string RepDesLang8
		{
			get
			{
				return this._RepDesLang8;
			}
			set
			{
				this._RepDesLang8 = value;
				NotifyPropertyChanged("RepDesLang8");
			}
		}

		public string RepDesLang9
		{
			get
			{
				return this._RepDesLang9;
			}
			set
			{
				this._RepDesLang9 = value;
				NotifyPropertyChanged("RepDesLang9");
			}
		}

		public string RepDesLang10
		{
			get
			{
				return this._RepDesLang10;
			}
			set
			{
				this._RepDesLang10 = value;
				NotifyPropertyChanged("RepDesLang10");
			}
		}

		public string RepRemarksLang1
		{
			get
			{
				return this._RepRemarksLang1;
			}
			set
			{
				this._RepRemarksLang1 = value;
				NotifyPropertyChanged("RepRemarksLang1");
			}
		}

		public string RepRemarksLang2
		{
			get
			{
				return this._RepRemarksLang2;
			}
			set
			{
				this._RepRemarksLang2 = value;
				NotifyPropertyChanged("RepRemarksLang2");
			}
		}

		public string RepRemarksLang3
		{
			get
			{
				return this._RepRemarksLang3;
			}
			set
			{
				this._RepRemarksLang3 = value;
				NotifyPropertyChanged("RepRemarksLang3");
			}
		}

		public string RepRemarksLang4
		{
			get
			{
				return this._RepRemarksLang4;
			}
			set
			{
				this._RepRemarksLang4 = value;
				NotifyPropertyChanged("RepRemarksLang4");
			}
		}

		public string RepRemarksLang5
		{
			get
			{
				return this._RepRemarksLang5;
			}
			set
			{
				this._RepRemarksLang5 = value;
				NotifyPropertyChanged("RepRemarksLang5");
			}
		}

		public string RepRemarksLang6
		{
			get
			{
				return this._RepRemarksLang6;
			}
			set
			{
				this._RepRemarksLang6 = value;
				NotifyPropertyChanged("RepRemarksLang6");
			}
		}

		public string RepRemarksLang7
		{
			get
			{
				return this._RepRemarksLang7;
			}
			set
			{
				this._RepRemarksLang7 = value;
				NotifyPropertyChanged("RepRemarksLang7");
			}
		}

		public string RepRemarksLang8
		{
			get
			{
				return this._RepRemarksLang8;
			}
			set
			{
				this._RepRemarksLang8 = value;
				NotifyPropertyChanged("RepRemarksLang8");
			}
		}

		public string RepRemarksLang9
		{
			get
			{
				return this._RepRemarksLang9;
			}
			set
			{
				this._RepRemarksLang9 = value;
				NotifyPropertyChanged("RepRemarksLang9");
			}
		}

		public string RepRemarksLang10
		{
			get
			{
				return this._RepRemarksLang10;
			}
			set
			{
				this._RepRemarksLang10 = value;
				NotifyPropertyChanged("RepRemarksLang10");
			}
		}

		public string FormNm
		{
			get
			{
				return this._FormNm;
			}
			set
			{
				this._FormNm = value;
				NotifyPropertyChanged("FormNm");
			}
		}

		public string RPTname1
		{
			get
			{
				return this._RPTname1;
			}
			set
			{
				this._RPTname1 = value;
				NotifyPropertyChanged("RPTname1");
			}
		}

		public string RPTname2
		{
			get
			{
				return this._RPTname2;
			}
			set
			{
				this._RPTname2 = value;
				NotifyPropertyChanged("RPTname2");
			}
		}

		public string RPTCaption
		{
			get
			{
				return this._RPTCaption;
			}
			set
			{
				this._RPTCaption = value;
				NotifyPropertyChanged("RPTCaption");
			}
		}

		public string RPTCaptionLang1
		{
			get
			{
				return this._RPTCaptionLang1;
			}
			set
			{
				this._RPTCaptionLang1 = value;
				NotifyPropertyChanged("RPTCaptionLang1");
			}
		}

		public string RPTCaptionLang2
		{
			get
			{
				return this._RPTCaptionLang2;
			}
			set
			{
				this._RPTCaptionLang2 = value;
				NotifyPropertyChanged("RPTCaptionLang2");
			}
		}

		public string RPTCaptionLang3
		{
			get
			{
				return this._RPTCaptionLang3;
			}
			set
			{
				this._RPTCaptionLang3 = value;
				NotifyPropertyChanged("RPTCaptionLang3");
			}
		}

		public string RPTCaptionLang4
		{
			get
			{
				return this._RPTCaptionLang4;
			}
			set
			{
				this._RPTCaptionLang4 = value;
				NotifyPropertyChanged("RPTCaptionLang4");
			}
		}

		public string RPTCaptionLang5
		{
			get
			{
				return this._RPTCaptionLang5;
			}
			set
			{
				this._RPTCaptionLang5 = value;
				NotifyPropertyChanged("RPTCaptionLang5");
			}
		}

		public string RPTCaptionLang6
		{
			get
			{
				return this._RPTCaptionLang6;
			}
			set
			{
				this._RPTCaptionLang6 = value;
				NotifyPropertyChanged("RPTCaptionLang6");
			}
		}

		public string RPTCaptionLang7
		{
			get
			{
				return this._RPTCaptionLang7;
			}
			set
			{
				this._RPTCaptionLang7 = value;
				NotifyPropertyChanged("RPTCaptionLang7");
			}
		}

		public string RPTCaptionLang8
		{
			get
			{
				return this._RPTCaptionLang8;
			}
			set
			{
				this._RPTCaptionLang8 = value;
				NotifyPropertyChanged("RPTCaptionLang8");
			}
		}

		public string RPTCaptionLang9
		{
			get
			{
				return this._RPTCaptionLang9;
			}
			set
			{
				this._RPTCaptionLang9 = value;
				NotifyPropertyChanged("RPTCaptionLang9");
			}
		}

		public string RPTCaptionLang10
		{
			get
			{
				return this._RPTCaptionLang10;
			}
			set
			{
				this._RPTCaptionLang10 = value;
				NotifyPropertyChanged("RPTCaptionLang10");
			}
		}

		public string RPTRecordSource1
		{
			get
			{
				return this._RPTRecordSource1;
			}
			set
			{
				this._RPTRecordSource1 = value;
				NotifyPropertyChanged("RPTRecordSource1");
			}
		}

		public string RPTRecordSource2
		{
			get
			{
				return this._RPTRecordSource2;
			}
			set
			{
				this._RPTRecordSource2 = value;
				NotifyPropertyChanged("RPTRecordSource2");
			}
		}

		public bool Hidden
		{
			get
			{
				return this._Hidden;
			}
			set
			{
				this._Hidden = value;
				NotifyPropertyChanged("Hidden");
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
		internal class Criteria
		{
			public int? _RepKey = null;
			public int? _option = null;
			
			internal Criteria()
			{
			}
			internal Criteria(int? RepKey)
			{
                _option = 1;
				_RepKey = RepKey;
			}
			internal Criteria(int? RepKey, int? Option)
			{
				_RepKey = RepKey;
				_option = Option;
			}
		}
		#endregion //Criteria

		#region Data Access - Fetch

		internal bool Fetch(Criteria criteria)
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
				cm.CommandText = "SYSRep_Get";

				cm.Parameters.AddWithValue("@Option", criteria._option);
				
				cm.Parameters.AddWithValue("@RepKey", criteria._RepKey);
				
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
		internal static SYSRep Get(IDataReader dr)
		{
			
			SYSRep child = new SYSRep();
			child.Fetch(dr);
			return child;
		}

		internal bool Fetch(IDataReader dataReader)
		{
			
			
			// Fill data to entity object
            _RepKey = dataReader["RepKey"] == DBNull.Value ? null : (int?)dataReader["RepKey"];
            _RepGrp = dataReader["RepGrp"] == DBNull.Value ? null : (int?)dataReader["RepGrp"];
            _RepType = dataReader["RepType"] == DBNull.Value ? string.Empty : dataReader["RepType"].ToString();
            _RepDesLang1 = dataReader["RepDesLang1"] == DBNull.Value ? string.Empty : dataReader["RepDesLang1"].ToString();
            _RepDesLang2 = dataReader["RepDesLang2"] == DBNull.Value ? string.Empty : dataReader["RepDesLang2"].ToString();
            _RepDesLang3 = dataReader["RepDesLang3"] == DBNull.Value ? string.Empty : dataReader["RepDesLang3"].ToString();
            _RepDesLang4 = dataReader["RepDesLang4"] == DBNull.Value ? string.Empty : dataReader["RepDesLang4"].ToString();
            _RepDesLang5 = dataReader["RepDesLang5"] == DBNull.Value ? string.Empty : dataReader["RepDesLang5"].ToString();
            _RepDesLang6 = dataReader["RepDesLang6"] == DBNull.Value ? string.Empty : dataReader["RepDesLang6"].ToString();
            _RepDesLang7 = dataReader["RepDesLang7"] == DBNull.Value ? string.Empty : dataReader["RepDesLang7"].ToString();
            _RepDesLang8 = dataReader["RepDesLang8"] == DBNull.Value ? string.Empty : dataReader["RepDesLang8"].ToString();
            _RepDesLang9 = dataReader["RepDesLang9"] == DBNull.Value ? string.Empty : dataReader["RepDesLang9"].ToString();
            _RepDesLang10 = dataReader["RepDesLang10"] == DBNull.Value ? string.Empty : dataReader["RepDesLang10"].ToString();
            _RepRemarksLang1 = dataReader["RepRemarksLang1"] == DBNull.Value ? string.Empty : dataReader["RepRemarksLang1"].ToString();
            _RepRemarksLang2 = dataReader["RepRemarksLang2"] == DBNull.Value ? string.Empty : dataReader["RepRemarksLang2"].ToString();
            _RepRemarksLang3 = dataReader["RepRemarksLang3"] == DBNull.Value ? string.Empty : dataReader["RepRemarksLang3"].ToString();
            _RepRemarksLang4 = dataReader["RepRemarksLang4"] == DBNull.Value ? string.Empty : dataReader["RepRemarksLang4"].ToString();
            _RepRemarksLang5 = dataReader["RepRemarksLang5"] == DBNull.Value ? string.Empty : dataReader["RepRemarksLang5"].ToString();
            _RepRemarksLang6 = dataReader["RepRemarksLang6"] == DBNull.Value ? string.Empty : dataReader["RepRemarksLang6"].ToString();
            _RepRemarksLang7 = dataReader["RepRemarksLang7"] == DBNull.Value ? string.Empty : dataReader["RepRemarksLang7"].ToString();
            _RepRemarksLang8 = dataReader["RepRemarksLang8"] == DBNull.Value ? string.Empty : dataReader["RepRemarksLang8"].ToString();
            _RepRemarksLang9 = dataReader["RepRemarksLang9"] == DBNull.Value ? string.Empty : dataReader["RepRemarksLang9"].ToString();
            _RepRemarksLang10 = dataReader["RepRemarksLang10"] == DBNull.Value ? string.Empty : dataReader["RepRemarksLang10"].ToString();
            _FormNm = dataReader["FormNm"] == DBNull.Value ? string.Empty : dataReader["FormNm"].ToString();
            _RPTname1 = dataReader["RPTname1"] == DBNull.Value ? string.Empty : dataReader["RPTname1"].ToString();
            _RPTname2 = dataReader["RPTname2"] == DBNull.Value ? string.Empty : dataReader["RPTname2"].ToString();
            _RPTCaption = dataReader["RPTCaption"] == DBNull.Value ? string.Empty : dataReader["RPTCaption"].ToString();
            _RPTCaptionLang1 = dataReader["RPTCaptionLang1"] == DBNull.Value ? string.Empty : dataReader["RPTCaptionLang1"].ToString();
            _RPTCaptionLang2 = dataReader["RPTCaptionLang2"] == DBNull.Value ? string.Empty : dataReader["RPTCaptionLang2"].ToString();
            _RPTCaptionLang3 = dataReader["RPTCaptionLang3"] == DBNull.Value ? string.Empty : dataReader["RPTCaptionLang3"].ToString();
            _RPTCaptionLang4 = dataReader["RPTCaptionLang4"] == DBNull.Value ? string.Empty : dataReader["RPTCaptionLang4"].ToString();
            _RPTCaptionLang5 = dataReader["RPTCaptionLang5"] == DBNull.Value ? string.Empty : dataReader["RPTCaptionLang5"].ToString();
            _RPTCaptionLang6 = dataReader["RPTCaptionLang6"] == DBNull.Value ? string.Empty : dataReader["RPTCaptionLang6"].ToString();
            _RPTCaptionLang7 = dataReader["RPTCaptionLang7"] == DBNull.Value ? string.Empty : dataReader["RPTCaptionLang7"].ToString();
            _RPTCaptionLang8 = dataReader["RPTCaptionLang8"] == DBNull.Value ? string.Empty : dataReader["RPTCaptionLang8"].ToString();
            _RPTCaptionLang9 = dataReader["RPTCaptionLang9"] == DBNull.Value ? string.Empty : dataReader["RPTCaptionLang9"].ToString();
            _RPTCaptionLang10 = dataReader["RPTCaptionLang10"] == DBNull.Value ? string.Empty : dataReader["RPTCaptionLang10"].ToString();
            _RPTRecordSource1 = dataReader["RPTRecordSource1"] == DBNull.Value ? string.Empty : dataReader["RPTRecordSource1"].ToString();
            _RPTRecordSource2 = dataReader["RPTRecordSource2"] == DBNull.Value ? string.Empty : dataReader["RPTRecordSource2"].ToString();
            _Hidden = dataReader["Hidden"] == DBNull.Value ? false : (bool)dataReader["Hidden"];
            _Custom1 = dataReader["Custom1"] == DBNull.Value ? string.Empty : dataReader["Custom1"].ToString();
            _Custom2 = dataReader["Custom2"] == DBNull.Value ? string.Empty : dataReader["Custom2"].ToString();
            _Custom3 = dataReader["Custom3"] == DBNull.Value ? string.Empty : dataReader["Custom3"].ToString();


            //if (dataReader["CreateDate"] != DBNull.Value)
            //{
            //    _CreateDate = Convert.ToDateTime(dataReader["CreateDate"]);
            //}
            //_CreateUserKey = Convert.ToInt32(dataReader["CreateUserKey"]);
            //if (dataReader["CreateDate"] != DBNull.Value)
            //{
            //    _LastModifiedDate = Convert.ToDateTime(dataReader["LastModifiedDate"]);
            //}
            //_LastModifiedUserKey = Convert.ToInt32(dataReader["LastModifiedUserKey"]);

            return true;
    	}
			#endregion //Data Access - Fetch

        #region Data Access - Custom Update      

        internal bool CustomUpdate(SqlConnection cn)
        {            
            // Fill data to entity object               
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSRep_CustomUpdate";                   
                cm.Parameters.AddWithValue("@RepKey", this._RepKey);
                cm.Parameters.AddWithValue("@RPTname1", this._RPTname1);
                cm.Parameters.AddWithValue("@RPTname2", this._RPTname2);
               
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == 1)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.                   
        }
        #endregion //Data Access - Custom Update
    }
}