

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Collections;
using System.Transactions;
using System.ComponentModel;

namespace BOLib
{
    [Serializable()]
    public class MSTShipNameDetItm : Csla.BusinessBase<MSTShipNameDetItm>,INotifyPropertyChanged
    {
        #region Business Properties and Methods

        //declare members
        private int _ShipNameKey;
        private int? _ShipMark;
        private DateTime? _CreateDate;
        private int? _CreateUserKey;
        private DateTime? _LastModifiedDate;
        private int? _LastModifiedUserKey;
        private string _Custom1;
        private string _Custom2;
        private string _Custom3;
        private bool _isDirty;
        private string _error = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        //public MSTShipNameDetItm()
        //    : base()
        //{           
         		
        //this._ShipNameKey = 0;
         		
        //this._ShipMark = 0;
         		
        //this._CreateDate = DateTime.Today.Date;
         		
        //this._CreateUserKey = null;
         		
        //this._LastModifiedDate = DateTime.Today.Date;
         		
        //this._LastModifiedUserKey = null;
         		
        //this._Custom1 = null;
         		
        //this._Custom2 = null;
         		
        //this._Custom3 = null;
         
        //    this._isDirty = false;
        //}


        public MSTShipNameDetItm Clone()
        {

            MSTShipNameDetItm objCopy = (MSTShipNameDetItm)this.MemberwiseClone();
            objCopy._isDirty = false;
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
        
 		public int  ShipNameKey
		{
			
            get
            {
                return this._ShipNameKey;
            }
            set
            {
                this._ShipNameKey = value;
                NotifyPropertyChanged("ShipNameKey");
            }
        }
                
 		public int? ShipMark
		{
			
            get
            {
                return this._ShipMark;
            }
            set
            {
                this._ShipMark = value;
                NotifyPropertyChanged("ShipMark");
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
         

        protected override object GetIdValue()
        {
            return _ShipNameKey.ToString() + _ShipMark.ToString();
        }

        #endregion //Business Properties and Methods

        #region Validation Rules
        private void AddCustomRules()
        {
            //add custom/non-generated rules here...
        }

        private void AddCommonRules()
        {
            /*
           //
           // Custom1
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom1", 255));
           //
           // Custom2
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom2", 255));
           //
           // Custom3
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom3", 255));
            */
        }

        protected override void AddBusinessRules()
        {
            /*
           AddCommonRules();
           AddCustomRules();
            */
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal MSTShipNameDetItm()
        { /* require use of factory method */ }

        internal static MSTShipNameDetItm New()
        {
            
            MSTShipNameDetItm child = new MSTShipNameDetItm();
            
            return child;
        }

        public static MSTShipNameDetItm NewChild()
        {
           
            MSTShipNameDetItm child = new MSTShipNameDetItm();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static MSTShipNameDetItm Get(SafeDataReader dr)
        {
            
            MSTShipNameDetItm child = new MSTShipNameDetItm();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTShipNameDetItm Get(int? shipNameKey, int? shipMark)
        {
            
            MSTShipNameDetItm child = new MSTShipNameDetItm();
            child.Fetch(new Criteria(shipNameKey, shipMark, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _shipNameKey = null;
            public int? _shipMark = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? ShipNameKey, int? ShipMark)
            {
                _shipNameKey = ShipNameKey;
                _shipMark = ShipMark;
            }

            internal Criteria(int? ShipNameKey, int? ShipMark, int? Option)
            {
                _shipNameKey = ShipNameKey;
                _shipMark = ShipMark;
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
                cm.CommandText = "MSTShipNameDetItm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@ShipNameKey", criteria._shipNameKey);
                cm.Parameters.AddWithValue("@ShipMark", criteria._shipMark);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                // Using data reader as record set.
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    // If data reader can read, continue...
                    while (dr.Read())
                    {
                        retValue = this.Fetch(dr);
                    }
                }	// Already close and dispose data reader.

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    retValue = true;
                }
                else
                {
                    retValue = false;
                }

            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {            
            _ShipNameKey = dr.GetInt32("ShipNameKey");
            _ShipMark = dr.GetInt32("ShipMark");
            _CreateDate = dr.GetDateTime("CreateDate");
            _CreateUserKey = dr.GetInt32("CreateUserKey");
            _LastModifiedDate = dr.GetDateTime("LastModifiedDate");
            _LastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            _Custom1 = dr.GetString("Custom1");
            _Custom2 = dr.GetString("Custom2");
            _Custom3 = dr.GetString("Custom3");
            ValidationRules.CheckRules();

            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        public bool Insert()
        {
            bool retValue = false;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        public bool Insert(SqlConnection cn)
        {        
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTShipNameDetItm_AddUpdate";
                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;                 

                if (_ShipNameKey == null)
                    cm.Parameters.AddWithValue("@ShipNameKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ShipNameKey", _ShipNameKey);

                if (_ShipMark == null)
                    cm.Parameters.AddWithValue("@ShipMark", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ShipMark", _ShipMark);

         
                cm.Parameters.AddWithValue("@CreateDate", DateTime.Now);

                if (AppInfor.currentUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);

                if (_LastModifiedDate == null)
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedDate", _LastModifiedDate.Value);

                if (_LastModifiedUserKey == null)
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", _LastModifiedUserKey);

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

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }// Already close and dispose sql connection.
            
        }

        #endregion //Data Access - Insert

        #region Data Access - Delete

        internal bool Delete(Criteria criteria)
        {
            bool retValue = false;
           
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
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
                cm.CommandText = "MSTShipNameDetItm_Delete";
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@ShipNameKey", criteria._shipNameKey);
                cm.Parameters.AddWithValue("@ShipMark", criteria._shipMark);

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }// Already close and dispose sql connection.            
        }

        #endregion //Data Access - Delete

    }
}


