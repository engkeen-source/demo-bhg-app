

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using System.ComponentModel;

namespace BOLib
{
    [Serializable()]
    public class MSTShipName : INotifyPropertyChanged
    {
        #region Business Properties and Methods

        internal int _shipNameKey;
        internal string _shipName;
        internal int? _conKey;
        internal string _conID;
        internal string _conNm;
        internal string _BillName;
        internal DateTime? _createDate;
        internal int? _createUserKey;
        internal DateTime? _lastModifiedDate;
        internal int? _lastModifiedUserKey;
        internal string _custom1;
        internal string _custom2;
        internal string _custom3;
        internal string _custom4;
        internal string _custom5;
        internal bool _isDirty;
        internal string _error = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;
        #region Assign New Value
        public MSTShipName()
        {

            this._shipNameKey = 0;

            this._shipName = string.Empty;

            this._conKey = 0;

            this._createDate = DateTime.Today.Date;

            this._createUserKey = null;

            this._lastModifiedDate = DateTime.Today.Date;

            this._lastModifiedUserKey = null;

            this._custom1 = null;

            this._custom2 = null;

            this._custom3 = null;

            this._custom4 = null;

            this._custom5 = null;

            this._isDirty = false;
        }
        public MSTShipName Clone()
        {

            MSTShipName objCopy = (MSTShipName)this.MemberwiseClone();
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
        #endregion

        #region Properties
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

        public int ShipNameKey
        {

            get
            {
                return this._shipNameKey;
            }
            set
            {
                this._shipNameKey = value;
                NotifyPropertyChanged("ShipNameKey");
            }
        }

        public string ShipName
        {
            get
            {
                return this._shipName;
            }
            set
            {
                this._shipName = value;
                NotifyPropertyChanged("ShipName");
            }
        }
        
        public string BillName
        {
            get
            {
                return this._BillName;
            }
            set
            {
                this._BillName = value;
                NotifyPropertyChanged("BillName");
            }
        }

        public int? ConKey
        {
            get
            {
                return this._conKey;
            }
            set
            {
                this._conKey = value;
                NotifyPropertyChanged("ConKey");
            }
        }
        public string ConID
        {

            get
            {
                return this._conID;
            }
            set
            {
                this._conID = value;
                NotifyPropertyChanged("ConID");
            }
        }
        public string ConNm
        {

            get
            {
                return this._conNm;
            }
            set
            {
                this._conNm = value;
                NotifyPropertyChanged("ConNm");
            }
        }
        public DateTime? CreateDate
        {

            get
            {
                return this._createDate;
            }
            set
            {
                this._createDate = value;
                NotifyPropertyChanged("CreateDate");
            }
        }
        public int? CreateUserKey
        {

            get
            {
                return this._createUserKey;
            }
            set
            {
                this._createUserKey = value;
                NotifyPropertyChanged("CreateUserKey");
            }
        }
        public DateTime? LastModifiedDate
        {

            get
            {
                return this._lastModifiedDate;
            }
            set
            {
                this._lastModifiedDate = value;
                NotifyPropertyChanged("LastModifiedDate");
            }
        }
        public int? LastModifiedUserKey
        {

            get
            {
                return this._lastModifiedUserKey;
            }
            set
            {
                this._lastModifiedUserKey = value;
                NotifyPropertyChanged("LastModifiedUserKey");
            }
        }
        public string Custom1
        {

            get
            {
                return this._custom1;
            }
            set
            {
                this._custom1 = value;
                NotifyPropertyChanged("Custom1");
            }
        }
        public string Custom2
        {

            get
            {
                return this._custom2;
            }
            set
            {
                this._custom2 = value;
                NotifyPropertyChanged("Custom2");
            }
        }
        public string Custom3
        {

            get
            {
                return this._custom3;
            }
            set
            {
                this._custom3 = value;
                NotifyPropertyChanged("Custom3");
            }
        }
        public string Custom4
        {

            get
            {
                return this._custom4;
            }
            set
            {
                this._custom4 = value;
                NotifyPropertyChanged("Custom4");
            }
        }
        public string Custom5
        {

            get
            {
                return this._custom5;
            }
            set
            {
                this._custom5 = value;
                NotifyPropertyChanged("Custom5");
            }
        }
        public bool IsDirty
        {

            get
            {
                return this._isDirty;
            }
            set
            {
                this._isDirty = value;
            }
        }

        //protected override object GetIdValue()
        //{
        //    return _shipNameKey.ToString();
        //}
        #endregion
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
           // ShipName
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "ShipName");
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ShipName", 255));
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
           //
           // Custom4
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom4", 255));
           //
           // Custom5
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom5", 255));
            */
        }

        //protected override void AddBusinessRules()
        //{
        //    /*
        //   AddCommonRules();
        //   AddCustomRules();
        //    */
        //}
        #endregion //Validation Rules

        #region Factory Methods
       
        internal static MSTShipName New()
        {
            
            MSTShipName child = new MSTShipName();
            
            return child;
        }

        internal static MSTShipName NewChild()
        {
            
            MSTShipName child = new MSTShipName();
            //child.ValidationRules.CheckRules();
            //child.MarkAsChild();
            
            return child;
        }

        internal static MSTShipName Get(SafeDataReader dr)
        {
            
            MSTShipName child = new MSTShipName();
            //child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTShipName Get(int? shipNameKey)
        {
            
            MSTShipName child = new MSTShipName();
            child.Fetch(new Criteria(shipNameKey, 1));
            return child;
        }

        public static MSTShipName Get(string shipName,int conkey)
        {
            
            MSTShipName child = new MSTShipName();
            child.Fetch(new Criteria(shipName,conkey, 3));
            return child;
        }
        public static MSTShipName Get(SqlConnection cn, string shipName, int conkey)
        {

            MSTShipName child = new MSTShipName();
            child.Fetch(cn, new Criteria(shipName,conkey, 2));
            return child;
        }
        public static MSTShipName Get(SqlConnection cn, int shipKey)
        {
            MSTShipName child = new MSTShipName();
            child.Fetch(cn, new Criteria(shipKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _shipNameKey = 0;
            public int? _option = null;
            public string _shipname = string.Empty;
            public int? _conKey = 0;

            internal Criteria()
            {
            }

            internal Criteria(int? ShipNameKey)
            {
                _shipNameKey = ShipNameKey;
                _option = 1;
            }

            internal Criteria(int? ShipNameKey, int? Option)
            {
                _shipNameKey = ShipNameKey;
                _option = Option;
            }
            internal Criteria(string ShipName,int? conKey, int? Option)
            {
                _shipname= ShipName;
                _conKey = conKey;
                _option = Option;
            }
            internal Criteria(int? ShipNameKey, string ShipName, int? conKey)
            {
                _shipNameKey = ShipNameKey;
                _shipname = ShipName;
                _conKey = conKey;
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
                cm.CommandText = "MSTShipName_Get";
                cm.Parameters.AddWithValue("@Option", criteria._option);                    
                cm.Parameters.AddWithValue("@ShipNameKey", criteria._shipNameKey);
                cm.Parameters.AddWithValue("@ShipName", criteria._shipname);
                cm.Parameters.AddWithValue("@ConKey", criteria._conKey);
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
            _shipNameKey = dr.GetInt32("ShipNameKey");
            _shipName = dr.GetString("ShipName");
            _conKey= dr.GetInt32("ConKey");
            _conID = dr.GetString("ConID");
            _conNm = dr.GetString("ConNm");
            _BillName = dr.GetString("BillName");
            _createDate = dr.GetDateTime("CreateDate");
            _createUserKey= dr.GetInt32("CreateUserKey");
            _lastModifiedDate= dr.GetDateTime("LastModifiedDate");
            _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            _custom1= dr.GetString("Custom1");
            _custom2= dr.GetString("Custom2");
            _custom3= dr.GetString("Custom3");
            _custom4= dr.GetString("Custom4");
            _custom5= dr.GetString("Custom5");
            //ValidationRules.CheckRules();
            
            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert( out int? shipNameKey)
        {
            bool retValue = false;
            
            shipNameKey = null;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn,  out shipNameKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn,  out int? shipNameKey)
        {           
            shipNameKey = 0;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTShipName_AddUpdate";
                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters.AddWithValue("@NewShipNameKey", shipNameKey);
                cm.Parameters["@NewShipNameKey"].Direction = ParameterDirection.Output;
                if (_shipNameKey == null)
                    cm.Parameters.AddWithValue("@ShipNameKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ShipNameKey", _shipNameKey);

                if (_shipName == null)
                    cm.Parameters.AddWithValue("@ShipName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ShipName", _shipName);

                if (_BillName == null)
                    cm.Parameters.AddWithValue("@BillName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BillName", _BillName);

                if (_conKey == null)
                    cm.Parameters.AddWithValue("@ConKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey", _conKey);

                if (_createDate == null)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

                if (_createUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                     cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);

                if (_lastModifiedDate == null)
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedDate", DateTime.Today.Date);

                if (_lastModifiedUserKey == null)
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", _lastModifiedUserKey);

                if (_custom1 == null)
                    cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom1", _custom1);

                if (_custom2 == null)
                    cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom2", _custom2);

                if (_custom3 == null)
                    cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom3", _custom3);

                if (_custom4 == null)
                    cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom4", _custom4);

                if (_custom5 == null)
                    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom5", _custom5);

                cm.Parameters["@NewShipNameKey"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                
                shipNameKey = (int)cm.Parameters["@NewShipNameKey"].Value;

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

        #region Data Access - Update

        internal bool Update()
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
                cm.CommandText = "MSTShipName_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewShipNameKey", 0);

                if (_shipNameKey == null)
                    cm.Parameters.AddWithValue("@ShipNameKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ShipNameKey", _shipNameKey);

                if (_shipName == null)
                    cm.Parameters.AddWithValue("@ShipName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ShipName", _shipName);

                if (_BillName == null)
                    cm.Parameters.AddWithValue("@BillName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BillName", _BillName);

                if (_conKey == null)
                    cm.Parameters.AddWithValue("@ConKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey", _conKey);

                if (_createDate == null)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

                if (_createUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateUserKey", _createUserKey);

                if (_lastModifiedDate == null)
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

                if (AppInfor.currentUserKey == null)
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

                if (_custom1 == null)
                    cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom1", _custom1);

                if (_custom2 == null)
                    cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom2", _custom2);

                if (_custom3 == null)
                    cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom3", _custom3);

                if (_custom4 == null)
                    cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom4", _custom4);

                if (_custom5 == null)
                    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom5", _custom5);

                cm.Parameters["@NewShipNameKey"].Direction = ParameterDirection.Output;

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
        #endregion //Data Access - Update

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
                cm.CommandText = "MSTShipName_Delete";
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@ShipNameKey", criteria._shipNameKey);

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

        #region Data Access - Validation

        internal bool Validation(Criteria criteria,  bool? isNew)
        {
            bool retValue = false;

            using (TransactionScope scope = new TransactionScope())
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();

                    retValue = this.Validation(cn, criteria,  isNew);
                }
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }
            return retValue;
        }

        internal bool Validation(SqlConnection cn, Criteria criteria,  bool? isNew)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTShipNameConKey_Validation";
                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@ShipNameKey", criteria._shipNameKey);
                cm.Parameters.AddWithValue("@ShipName", criteria._shipname);
                cm.Parameters.AddWithValue("@ConKey", criteria._conKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;                
                cm.ExecuteNonQuery();
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }            
        }
        #endregion 


        private void Clear()
        {
            _shipNameKey=0;
            _shipName=string.Empty;
            _conKey=0;
            _conID=string.Empty;
            _conNm=string.Empty;
            _createDate=null;
            _createUserKey = null;
            _lastModifiedDate=null;
            _lastModifiedUserKey = null;
            _custom1 = string.Empty;
            _custom2 = string.Empty; ;
            _custom3 = string.Empty; ;
            _custom4 = string.Empty; ;
            _custom5 = string.Empty; ;

        }
    
    }
}


