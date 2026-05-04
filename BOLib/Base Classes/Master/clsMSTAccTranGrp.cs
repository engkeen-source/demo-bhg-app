

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Collections;
using System.Transactions;

namespace BOLib
{ 
	[Serializable()] 
	public class MSTAccTranGrp: Csla.BusinessBase<MSTAccTranGrp>
	{	
		#region Business Properties and Methods

		//declare members
		internal int? _tranGrpKey = null;
		internal int? _tranGrpKeyParent = null;
		internal string _tranGrpID = string.Empty;
		internal string _tranGrpDes = string.Empty;
		internal string _tranGrpTitle = string.Empty;
		internal bool? _inActive = false;
		internal string _custom1 = string.Empty;
		internal string _custom2 = string.Empty;
		internal string _custom3 = string.Empty;
		internal string _custom4 = string.Empty;
		internal string _custom5 = string.Empty;
		internal DateTime? _createDate = null;
		internal int? _createUserKey = null;
		internal DateTime? _lastModifiedDate = null;
		internal int? _lastModifiedUserKey = null;
		
		public int? TranGrpKey
		{
			get 
			{ 
				return _tranGrpKey; 
			}
			set 
			{ 
				_tranGrpKey = value;
		      	PropertyHasChanged("TranGrpKey");
			}
		}

		public int? TranGrpKeyParent
		{
			get 
			{ 
				return _tranGrpKeyParent; 
			}
			set 
			{ 
				_tranGrpKeyParent = value;
		      	//PropertyHasChanged("TranGrpKeyParent");
			}
		}

		public string TranGrpID
		{
			get 
			{ 
				return _tranGrpID; 
			}
			set 
			{ 
				_tranGrpID = value;
		      	PropertyHasChanged("TranGrpID");
			}
		}

		public string TranGrpDes
		{
			get 
			{ 
				return _tranGrpDes; 
			}
			set 
			{ 
				_tranGrpDes = value;
		      	PropertyHasChanged("TranGrpDes");
			}
		}

		public string TranGrpTitle
		{
			get 
			{ 
				return _tranGrpTitle; 
			}
			set 
			{ 
				_tranGrpTitle = value;
		      	PropertyHasChanged("TranGrpTitle");
			}
		}

		public bool? InActive
		{
			get 
			{ 
				return _inActive; 
			}
			set 
			{ 
				_inActive = value;
		      	PropertyHasChanged("InActive");
			}
		}

		public string Custom1
		{
			get 
			{ 
				return _custom1; 
			}
			set 
			{ 
				_custom1 = value;
		      	PropertyHasChanged("Custom1");
			}
		}

		public string Custom2
		{
			get 
			{ 
				return _custom2; 
			}
			set 
			{ 
				_custom2 = value;
		      	PropertyHasChanged("Custom2");
			}
		}

		public string Custom3
		{
			get 
			{ 
				return _custom3; 
			}
			set 
			{ 
				_custom3 = value;
		      	PropertyHasChanged("Custom3");
			}
		}

		public string Custom4
		{
			get 
			{ 
				return _custom4; 
			}
			set 
			{ 
				_custom4 = value;
		      	PropertyHasChanged("Custom4");
			}
		}

		public string Custom5
		{
			get 
			{ 
				return _custom5; 
			}
			set 
			{ 
				_custom5 = value;
		      	PropertyHasChanged("Custom5");
			}
		}

		public DateTime? CreateDate
		{
			get 
			{ 
				return _createDate; 
			}
			set 
			{ 
				_createDate = value;
		      	PropertyHasChanged("CreateDate");
			}
		}

		public int? CreateUserKey
		{
			get 
			{ 
				return _createUserKey; 
			}
			set 
			{ 
				_createUserKey = value;
		      	PropertyHasChanged("CreateUserKey");
			}
		}

		public DateTime? LastModifiedDate
		{
			get 
			{ 
				return _lastModifiedDate; 
			}
			set 
			{ 
				_lastModifiedDate = value;
		      	PropertyHasChanged("LastModifiedDate");
			}
		}

		public int? LastModifiedUserKey
		{
			get 
			{ 
				return _lastModifiedUserKey; 
			}
			set 
			{ 
				_lastModifiedUserKey = value;
		      	PropertyHasChanged("LastModifiedUserKey");
			}
		}

		protected override object GetIdValue()
		{
			return _tranGrpKey.ToString();
		}

		#endregion //Business Properties and Methods	

		#region Validation Rules
		private void AddCustomRules()
		{
			//add custom/non-generated rules here...
		}

		private void AddCommonRules()
		{
			 
            ////
            //// TranGrpID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "TranGrpID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TranGrpID", 50));
            ////
            //// TranGrpDes
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "TranGrpDes");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TranGrpDes", 255));
            ////
            //// TranGrpTitle
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "TranGrpTitle");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TranGrpTitle", 50));
            ////
            //// Custom1
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom1", 255));
            ////
            //// Custom2
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom2", 255));
            ////
            //// Custom3
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom3", 255));
            ////
            //// Custom4
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom4", 255));
            ////
            //// Custom5
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom5", 255));
			
		}

		protected override void AddBusinessRules()
		{
			AddCommonRules();
			AddCustomRules();
		}
		#endregion //Validation Rules

		#region Factory Methods

		internal MSTAccTranGrp()
		{ /* require use of factory method */ }

        internal static MSTAccTranGrp New(int parentKey)
		{			
			MSTAccTranGrp child = new MSTAccTranGrp();
            child.TranGrpKeyParent = parentKey;			
			return child;
		}

		internal static MSTAccTranGrp NewChild()
		{			
			MSTAccTranGrp child = new MSTAccTranGrp();
			child.ValidationRules.CheckRules();
			child.MarkAsChild();			
			return child;
		}

		internal static MSTAccTranGrp Get(SafeDataReader dr)
		{			
			MSTAccTranGrp child = new MSTAccTranGrp();
			child.MarkAsChild();
			child.Fetch(dr);
			return child;
		}

        public static MSTAccTranGrp Get(int? tranGrpKey)
		{		
			MSTAccTranGrp child = new MSTAccTranGrp();
			child.Fetch(new Criteria(tranGrpKey, 1));
			return child;
		}
        public static MSTAccTranGrp Get(string tranGrpID,int option)
        {
            MSTAccTranGrp child = new MSTAccTranGrp();
            child.Fetch(new Criteria(0,tranGrpID,3));
            return child;
        }

		#endregion //Factory Methods

		#region Criteria

		[Serializable()]
		internal class Criteria
		{ 
			public int? _tranGrpKey = 0;
			public int? _option = null;
            public string _tranGrpID = string.Empty;

			internal Criteria()
			{
			}

			internal Criteria(int? TranGrpKey)
			{
				_tranGrpKey = TranGrpKey;
			}

            internal Criteria(int? TranGrpKey, string TranGrpID)
            {
                _tranGrpKey = TranGrpKey;
                _tranGrpID = TranGrpID;
            }

			internal Criteria(int? TranGrpKey, int? Option)
			{
				_tranGrpKey = TranGrpKey;
				_option = Option;
			}
     
            internal Criteria(int? TranGrpKey, string TranGrpID, int? Option)
            {
                _tranGrpKey = TranGrpKey;
                _tranGrpID = TranGrpID;
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
			
			// Using existing sql connection.
			using (SqlCommand cm = cn.CreateCommand())
			{
				cm.CommandType = CommandType.StoredProcedure;
				cm.CommandText = "MSTAccTranGrp_Get";

				cm.Parameters.AddWithValue("@Option", criteria._option);				
				cm.Parameters.AddWithValue("@TranGrpKey" , criteria._tranGrpKey);
                cm.Parameters.AddWithValue("@TranGrpID", criteria._tranGrpID);
				cm.Parameters.AddWithValue("@RetValue", 0);
				cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

				// Using data reader as record set.
				using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
				{
					// If data reader can read, continue...
                    if (dr.Read())
                    {
                        retValue = this.Fetch(dr);
                    }
                    else
                        this.Clear();
				}	// Already close and dispose data reader.

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue=true;
                else
                    retValue=false;
				
			}// Already close and dispose sql connection.
			
			return retValue;
		}

		internal bool Fetch(SafeDataReader dr)
		{
			_tranGrpKey = dr.GetInt32("TranGrpKey");
			_tranGrpKeyParent = dr.GetInt32("TranGrpKeyParent");
			_tranGrpID = dr.GetString("TranGrpID");
			_tranGrpDes = dr.GetString("TranGrpDes");
			_tranGrpTitle = dr.GetString("TranGrpTitle");
			_inActive = dr.GetBoolean("InActive");
			_custom1 = dr.GetString("Custom1");
			_custom2 = dr.GetString("Custom2");
			_custom3 = dr.GetString("Custom3");
			_custom4 = dr.GetString("Custom4");
			_custom5 = dr.GetString("Custom5");
            if (dr.GetValue("CreateDate") == DBNull.Value)
                _createDate = null;
            else
			    _createDate = dr.GetDateTime("CreateDate");
			_createUserKey = dr.GetInt32("CreateUserKey");
            if (dr.GetValue("LastModifiedDate") == null)
                _lastModifiedDate = null;
            else
                _lastModifiedDate = dr.GetDateTime("LastModifiedDate");
			_lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
			ValidationRules.CheckRules();

            return true;
		}
		#endregion //Data Access - Fetch

		#region Data Access - Insert

		internal bool Insert(out int? tranGrpKey)
		{
			bool retValue = false;			
			tranGrpKey = null;
			
			// Create Transaction Scope
			using(TransactionScope scope = new TransactionScope())	
			{
				// Create SqlConnection
				using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
				{
					// Open Connection
					cn.Open();

					// Call insert method.
					retValue = this.Insert(cn,out tranGrpKey);
				}// End of SqlConnection

				// No errors - commit transaction
				  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
			}// End of TransactionScope
			
			return retValue;
		}

		internal bool Insert(SqlConnection cn, out int? tranGrpKey)
		{
			tranGrpKey = 0;
			
			// Using existing sql connection.
			using (SqlCommand cm = cn.CreateCommand())
			{
				cm.CommandType = CommandType.StoredProcedure;
				cm.CommandText = "MSTAccTranGrp_AddUpdate";

				cm.Parameters.AddWithValue("@Option", 0);
				
				cm.Parameters.AddWithValue("@RetValue", 0);
				cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

				cm.Parameters.AddWithValue("@NewTranGrpKey" , tranGrpKey); 
			
				if (_tranGrpKey == null)
					cm.Parameters.AddWithValue("@TranGrpKey" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@TranGrpKey" , _tranGrpKey);

				if (_tranGrpKeyParent == null)
					cm.Parameters.AddWithValue("@TranGrpKeyParent" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@TranGrpKeyParent" , _tranGrpKeyParent);

				if (_tranGrpID == null)
					cm.Parameters.AddWithValue("@TranGrpID" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@TranGrpID" , _tranGrpID);

				if (_tranGrpDes == null)
					cm.Parameters.AddWithValue("@TranGrpDes" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@TranGrpDes" , _tranGrpDes);

				if (_tranGrpTitle == null)
					cm.Parameters.AddWithValue("@TranGrpTitle" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@TranGrpTitle" , _tranGrpTitle);

				if (_inActive == null)
					cm.Parameters.AddWithValue("@InActive" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@InActive" , _inActive);

				if (_custom1 == null)
					cm.Parameters.AddWithValue("@Custom1" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@Custom1" , _custom1);

				if (_custom2 == null)
					cm.Parameters.AddWithValue("@Custom2" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@Custom2" , _custom2);

				if (_custom3 == null)
					cm.Parameters.AddWithValue("@Custom3" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@Custom3" , _custom3);

				if (_custom4 == null)
					cm.Parameters.AddWithValue("@Custom4" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@Custom4" , _custom4);

				if (_custom5 == null)
					cm.Parameters.AddWithValue("@Custom5" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@Custom5" , _custom5);

				if (_createDate == null)
					cm.Parameters.AddWithValue("@CreateDate" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@CreateDate" , _createDate.Value);

				if (_createUserKey == null)
					cm.Parameters.AddWithValue("@CreateUserKey" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@CreateUserKey" , _createUserKey);

				if (_lastModifiedDate == null)
					cm.Parameters.AddWithValue("@LastModifiedDate" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@LastModifiedDate" , _lastModifiedDate.Value);

				if (_lastModifiedUserKey == null)
					cm.Parameters.AddWithValue("@LastModifiedUserKey" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@LastModifiedUserKey" , _lastModifiedUserKey);

				cm.Parameters["@NewTranGrpKey"].Direction = ParameterDirection.Output; 

				cm.ExecuteNonQuery();
			

				tranGrpKey = (int)cm.Parameters["@NewTranGrpKey"].Value;
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
				
			}// Already close and dispose sql connection.
			
		}

		#endregion //Data Access - Insert

		#region Data Access - Update

		internal bool Update()
		{
			bool retValue = false;			
			
			// Create Transaction Scope
			using(TransactionScope scope = new TransactionScope())	
			{
				// Create SqlConnection
				using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
				{
					// Open Connection
					cn.Open();

						// Call update method.
					retValue = this.Update(cn);
				}// End of SqlConnection

				// No errors - commit transaction
				  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
			}// End of TransactionScope
			
			return retValue;
		}

		internal bool Update(SqlConnection cn)
		{
			// Using existing sql connection.
			using (SqlCommand cm = cn.CreateCommand())
			{
				cm.CommandType = CommandType.StoredProcedure;
				cm.CommandText = "MSTAccTranGrp_AddUpdate";

				cm.Parameters.AddWithValue("@Option", 1);				

				cm.Parameters.AddWithValue("@RetValue", 0);
				cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

				cm.Parameters.AddWithValue("@NewTranGrpKey" , 0); 
			
				if (_tranGrpKey == null)
					cm.Parameters.AddWithValue("@TranGrpKey" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@TranGrpKey" , _tranGrpKey);

				if (_tranGrpKeyParent == null)
					cm.Parameters.AddWithValue("@TranGrpKeyParent" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@TranGrpKeyParent" , _tranGrpKeyParent);

				if (_tranGrpID == null)
					cm.Parameters.AddWithValue("@TranGrpID" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@TranGrpID" , _tranGrpID);

				if (_tranGrpDes == null)
					cm.Parameters.AddWithValue("@TranGrpDes" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@TranGrpDes" , _tranGrpDes);

				if (_tranGrpTitle == null)
					cm.Parameters.AddWithValue("@TranGrpTitle" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@TranGrpTitle" , _tranGrpTitle);

				if (_inActive == null)
					cm.Parameters.AddWithValue("@InActive" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@InActive" , _inActive);

				if (_custom1 == null)
					cm.Parameters.AddWithValue("@Custom1" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@Custom1" , _custom1);

				if (_custom2 == null)
					cm.Parameters.AddWithValue("@Custom2" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@Custom2" , _custom2);

				if (_custom3 == null)
					cm.Parameters.AddWithValue("@Custom3" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@Custom3" , _custom3);

				if (_custom4 == null)
					cm.Parameters.AddWithValue("@Custom4" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@Custom4" , _custom4);

				if (_custom5 == null)
					cm.Parameters.AddWithValue("@Custom5" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@Custom5" , _custom5);

				if (_createDate == null)
					cm.Parameters.AddWithValue("@CreateDate" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@CreateDate" , _createDate.Value);

				if (_createUserKey == null)
					cm.Parameters.AddWithValue("@CreateUserKey" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@CreateUserKey" , _createUserKey);

				if (_lastModifiedDate == null)
					cm.Parameters.AddWithValue("@LastModifiedDate" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@LastModifiedDate" , _lastModifiedDate.Value);

				if (AppInfor.currentUserKey == null)
					cm.Parameters.AddWithValue("@LastModifiedUserKey" , DBNull.Value); 
				else 
					cm.Parameters.AddWithValue("@LastModifiedUserKey" , _lastModifiedUserKey);

				cm.Parameters["@NewTranGrpKey"].Direction = ParameterDirection.Output; 

				cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
			}// Already close and dispose sql connection.
			
		}
		#endregion //Data Access - Update

		#region Data Access - Delete

		internal bool Delete(Criteria criteria)
		{
			bool retValue = false;
			
			// Create Transaction Scope
			using(TransactionScope scope = new TransactionScope())	
			{
				// Create SqlConnection
				using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
				{
					// Open Connection
					cn.Open();

					// Call delete method.
					retValue = this.Delete(cn, criteria);
				}// End of SqlConnection

				// No errors - commit transaction
				  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
			}// End of TransactionScope
			
			return retValue;
		}

		internal bool Delete(SqlConnection cn, Criteria criteria)
		{
			// Using existing sql connection.
			using (SqlCommand cm = cn.CreateCommand())
			{
				cm.CommandType = CommandType.StoredProcedure;
				cm.CommandText = "MSTAccTranGrp_Delete";					
				

				cm.Parameters.AddWithValue("@RetValue", 0);
				cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

				cm.Parameters.AddWithValue("@TranGrpKey" , criteria._tranGrpKey);

				cm.ExecuteNonQuery();

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;

			}// Already close and dispose sql connection.
			
		}

		#endregion //Data Access - Delete

		#region Data Access - Validation

		internal bool Validation(Criteria criteria, bool? isNew)
		{
			bool retValue = false;            
			
			// Create Transaction Scope
			using(TransactionScope scope = new TransactionScope())	
			{
				// Create SqlConnection
				using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
				{
					// Open Connection
					cn.Open();

					// Call validation method.
					retValue = this.Validation(cn, criteria, isNew);
				}// End of SqlConnection

				// No errors - commit transaction
				  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
			}// End of TransactionScope
			
			return retValue;
		}

		internal bool Validation(SqlConnection cn, Criteria criteria, bool? isNew)
		{
			// Using existing sql connection.
			using (SqlCommand cm = cn.CreateCommand())
			{
				cm.CommandType = CommandType.StoredProcedure;
				cm.CommandText = "MSTAccTranGrp_Validation";

                cm.Parameters.AddWithValue("@IsNew", isNew);
			
				cm.Parameters.AddWithValue("@RetValue", 0);
				cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

				cm.Parameters.AddWithValue("@TranGrpKey" , criteria._tranGrpKey);
                cm.Parameters.AddWithValue("@TranGrpID", criteria._tranGrpID);

				cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
			}
			
		}
		#endregion //Data Access - Validation

        private void Clear()
        {
            _tranGrpKey = null;
            _tranGrpKeyParent = null;
            _tranGrpID = string.Empty;
            _tranGrpDes = string.Empty;
            _tranGrpTitle = string.Empty;
            _inActive = false;
            _custom1 = string.Empty;
            _custom2 = string.Empty;
            _custom3 = string.Empty;
            _custom4 = string.Empty;
            _custom5 = string.Empty;
             _createDate = null;
            _createUserKey = null;
            _lastModifiedDate = null;
            _lastModifiedUserKey = null;
            
        }    
    
    }
}

	
