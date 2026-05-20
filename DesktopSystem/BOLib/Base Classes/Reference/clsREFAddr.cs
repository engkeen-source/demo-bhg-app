
using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Collections;
using System.Transactions;
using TAUtil;


namespace BOLib
{
    [Serializable()]
    public class REFAddr : Csla.BusinessBase<REFAddr>, System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _addrKey = 0;
        internal int? _addrLinkType = null;
        internal int? _addrLinkKey = null;
        internal string _addrID = string.Empty;
        internal string _addrType = string.Empty;
        internal string _addrStreet = string.Empty;
        internal string _addrPOBox = string.Empty;
        internal string _addrCity = string.Empty;
        internal string _addrState = string.Empty;
        internal string _addrZipCode = string.Empty;
        internal string _addrCountry = string.Empty;
        internal string _addrRegion = string.Empty;
        internal string _addrAttn = string.Empty;
        internal string _addrTel1 = string.Empty;
        internal string _addrTel2 = string.Empty;
        internal string _addrFax = string.Empty;
        internal string _addrEmail = string.Empty;
        internal int? _addrShipViaKey = null;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _error = string.Empty;

        public int? AddrKey
        {
            get
            {
                return _addrKey;
            }
        }

        public int? AddrLinkType
        {
            get
            {
                return _addrLinkType;
            }
        }

        public int? AddrLinkKey
        {
            get
            {
                return _addrLinkKey;
            }
        }

        public string Error
        {
            get
            {
                return _error;
            }
            set
            {

                _error = value;

            }
        }

        public string AddrID
        {
            get
            {
                return _addrID;
            }
            set
            {
                _addrID = value;
                PropertyHasChanged("AddrID");
            }
        }

        public string AddrType
        {
            get
            {
                return _addrType;
            }
            set
            {
                _addrType = value;
                PropertyHasChanged("AddrType");
            }
        }

        public string AddrStreet
        {
            get
            {
                return _addrStreet;
            }
            set
            {
                _addrStreet = value;
                PropertyHasChanged("AddrStreet");
            }
        }

        public string AddrPOBox
        {
            get
            {
                return _addrPOBox;
            }
            set
            {
                _addrPOBox = value;
                PropertyHasChanged("AddrPOBox");
            }
        }

        public string AddrCity
        {
            get
            {
                return _addrCity;
            }
            set
            {
                _addrCity = value;
                PropertyHasChanged("AddrCity");
            }
        }

        public string AddrState
        {
            get
            {
                return _addrState;
            }
            set
            {
                _addrState = value;
                PropertyHasChanged("AddrState");
            }
        }

        public string AddrZipCode
        {
            get
            {
                return _addrZipCode;
            }
            set
            {
                _addrZipCode = value;
                PropertyHasChanged("AddrZipCode");
            }
        }

        public string AddrCountry
        {
            get
            {
                return _addrCountry;
            }
            set
            {
                _addrCountry = value;
                PropertyHasChanged("AddrCountry");
            }
        }

        public string AddrRegion
        {
            get
            {
                return _addrRegion;
            }
            set
            {
                _addrRegion = value;
                PropertyHasChanged("AddrRegion");
            }
        }

        public string AddrAttn
        {
            get
            {
                return _addrAttn;
            }
            set
            {
                _addrAttn = value;
                PropertyHasChanged("AddrAttn");
            }
        }

        public string AddrTel1
        {
            get
            {
                return _addrTel1;
            }
            set
            {
                _addrTel1 = value;
                PropertyHasChanged("AddrTel1");
            }
        }

        public string AddrTel2
        {
            get
            {
                return _addrTel2;
            }
            set
            {
                _addrTel2 = value;
                PropertyHasChanged("AddrTel2");
            }
        }

        public string AddrFax
        {
            get
            {
                return _addrFax;
            }
            set
            {
                _addrFax = value;
                PropertyHasChanged("AddrFax");
            }
        }

        public string AddrEmail
        {
            get
            {
                return _addrEmail;
            }
            set
            {
                _addrEmail = value;
                PropertyHasChanged("AddrEmail");
            }
        }

        public int? AddrShipViaKey
        {
            get
            {
                return _addrShipViaKey;
            }
            set
            {
                _addrShipViaKey = value;
                PropertyHasChanged("AddrShipViaKey");
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

        protected override object GetIdValue()
        {
            return _addrKey.ToString();
        }

        #endregion //Business Properties and Methods

        #region Factory Methods

        public REFAddr()
        { /* require use of factory method */ }

        public static REFAddr New()
        {
            REFAddr child = new REFAddr();
            return child;
        }

        internal static REFAddr NewChild()
        {
            REFAddr child = new REFAddr();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static REFAddr Get(SafeDataReader dr)
        {
            REFAddr child = new REFAddr();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static REFAddr Get(int? addrKey)
        {
            REFAddr child = new REFAddr();
            child.Fetch(new Criteria(addrKey, 1));
            return child;
        }

        public static REFAddr Get(SqlConnection cn, int? addrKey)
        {
            REFAddr child = new REFAddr();
            child.Fetch(cn, new Criteria(addrKey, 1));
            return child;
        }

        public static REFAddr Get(int? addrLinkType, int? addrLinkKey, string addrID)
        {
            REFAddr child = new REFAddr();
            child.Fetch(new Criteria(addrLinkType, addrLinkKey, addrID, 3));
            return child;
        }

        public static REFAddr Get(SqlConnection cn, int? addrLinkType, int? addrLinkKey, string addrID)
        {
            REFAddr child = new REFAddr();
            child.Fetch(cn, new Criteria(addrLinkType, addrLinkKey, addrID, 3));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _addrKey = null;
            public int? _addrLinkType = null;
            public int? _addrLinkKey = null;
            public string _addrID = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? AddrKey)
            {
                _addrKey = AddrKey;
            }

            internal Criteria(int? AddrKey, int? Option)
            {
                _addrKey = AddrKey;
                _option = Option;
            }

            internal Criteria(int? AddrLinkKey, string AddrID)
            {
                _addrLinkKey = AddrLinkKey;
                _addrID = AddrID;
            }

            internal Criteria(int? AddrLinkType, int? AddrLinkKey, int Option)
            {
                _addrLinkType = AddrLinkType;
                _addrLinkKey = AddrLinkKey;
                _option = Option;
            }

            internal Criteria(int? AddrLinkType, int? AddrLinkKey, string AddrID, int? Option)
            {
                _addrLinkType = AddrLinkType;
                _addrLinkKey = AddrLinkKey;
                _addrID = AddrID;
                _option = Option;
            }

            internal Criteria(int? AddrLinkType, int? AddrLinkKey, int? AddrKey, string AddrID, int? Option)
            {
                _addrLinkType = AddrLinkType;
                _addrLinkKey = AddrLinkKey;
                _addrKey = AddrKey;
                _addrID = AddrID;
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
                cm.CommandText = "REFAddr_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                if (GFunc.IsNE(criteria._addrKey))
                    cm.Parameters.AddWithValue("@AddrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrKey", criteria._addrKey);

                cm.Parameters.AddWithValue("@AddrLinkType", criteria._addrLinkType);
                cm.Parameters.AddWithValue("@AddrLinkKey", criteria._addrLinkKey);
                cm.Parameters.AddWithValue("@AddrID", criteria._addrID);

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
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
                    retValue = true;
                else
                {
                    throw new TAException(MsgID.Common.GetFail);
                }

            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _addrKey = dr.GetInt32("AddrKey");
            _addrLinkType = dr.GetInt32("AddrLinkType");
            _addrLinkKey = dr.GetInt32("AddrLinkKey");
            _addrID = dr.GetString("AddrID");
            _addrType = dr.GetString("AddrType");
            _addrStreet = dr.GetString("AddrStreet");
            _addrPOBox = dr.GetString("AddrPOBox");
            _addrCity = dr.GetString("AddrCity");
            _addrState = dr.GetString("AddrState");
            _addrZipCode = dr.GetString("AddrZipCode");
            _addrCountry = dr.GetString("AddrCountry");
            _addrRegion = dr.GetString("AddrRegion");
            _addrAttn = dr.GetString("AddrAttn");
            _addrTel1 = dr.GetString("AddrTel1");
            _addrTel2 = dr.GetString("AddrTel2");
            _addrFax = dr.GetString("AddrFax");
            _addrEmail = dr.GetString("AddrEmail");
            _addrShipViaKey = dr.GetInt32("AddrShipViaKey");
            _createDate = dr.GetDateTime("CreateDate");
            _createUserKey = dr.GetInt32("CreateUserKey");
            _lastModifiedDate = dr.GetDateTime("LastModifiedDate");
            _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            ValidationRules.CheckRules();

            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        //internal bool Insert()
        internal bool Insert(out int? AddrKey)
        {
            bool retValue = false;
            //addrLinkType = 0;
            //addrLinkKey = 0;
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    //retValue = this.Insert(cn);
                    retValue = this.Insert(cn, out AddrKey);

                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }
        internal bool Insert(SqlConnection cn, out int? AddrKey)
        {
            bool retValue = false;
            AddrKey = 0;

            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFAddr_AddUpdate";
                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@NewAddrKey", 0);
                if (_addrKey == null)
                    cm.Parameters.AddWithValue("@AddrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrKey", _addrKey);
                if (_addrLinkType == null)
                    cm.Parameters.AddWithValue("@AddrLinkType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrLinkType", _addrLinkType);
                if (_addrLinkKey == null)
                    cm.Parameters.AddWithValue("@AddrLinkKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrLinkKey", _addrLinkKey);
                if (_addrID == null)
                    cm.Parameters.AddWithValue("@AddrID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrID", _addrID);
                if (_addrType == null)
                    cm.Parameters.AddWithValue("@AddrType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrType", _addrType);
                if (_addrStreet == null)
                    cm.Parameters.AddWithValue("@AddrStreet", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrStreet", _addrStreet);
                if (_addrPOBox == null)
                    cm.Parameters.AddWithValue("@AddrPOBox", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrPOBox", _addrPOBox);
                if (_addrCity == null)
                    cm.Parameters.AddWithValue("@AddrCity", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrCity", _addrCity);
                if (_addrState == null)
                    cm.Parameters.AddWithValue("@AddrState", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrState", _addrState);
                if (_addrZipCode == null)
                    cm.Parameters.AddWithValue("@AddrZipCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrZipCode", _addrZipCode);
                if (_addrCountry == null)
                    cm.Parameters.AddWithValue("@AddrCountry", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrCountry", _addrCountry);
                if (_addrRegion == null)
                    cm.Parameters.AddWithValue("@AddrRegion", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrRegion", _addrRegion);
                if (_addrAttn == null)
                    cm.Parameters.AddWithValue("@AddrAttn", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrAttn", _addrAttn);
                if (_addrTel1 == null)
                    cm.Parameters.AddWithValue("@AddrTel1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrTel1", _addrTel1);
                if (_addrTel2 == null)
                    cm.Parameters.AddWithValue("@AddrTel2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrTel2", _addrTel2);
                if (_addrFax == null)
                    cm.Parameters.AddWithValue("@AddrFax", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrFax", _addrFax);
                if (_addrEmail == null)
                    cm.Parameters.AddWithValue("@addrEmail", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@addrEmail", _addrEmail);
                if (_addrShipViaKey == null)
                    cm.Parameters.AddWithValue("@AddrShipViaKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrShipViaKey", _addrShipViaKey);
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
                cm.Parameters["@NewAddrKey"].Direction = ParameterDirection.Output;

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.ExecuteNonQuery();

                AddrKey = (int)cm.Parameters["@NewAddrKey"].Value;

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                {
                    throw new TAException(MsgID.Common.UpdateFail);
                }
            }// Already close and dispose sql connection.

            return retValue;
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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }
        internal bool Update(SqlConnection cn)
        {
            bool retValue = false;

            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFAddr_AddUpdate";
                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewAddrKey", 0);
                if (_addrKey == null)
                    cm.Parameters.AddWithValue("@AddrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrKey", _addrKey);
                if (_addrLinkType == null)
                    cm.Parameters.AddWithValue("@AddrLinkType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrLinkType", _addrLinkType);
                if (_addrLinkKey == null)
                    cm.Parameters.AddWithValue("@AddrLinkKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrLinkKey", _addrLinkKey);
                if (_addrID == null)
                    cm.Parameters.AddWithValue("@AddrID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrID", _addrID);
                if (_addrType == null)
                    cm.Parameters.AddWithValue("@AddrType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrType", _addrType);
                if (_addrStreet == null)
                    cm.Parameters.AddWithValue("@AddrStreet", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrStreet", _addrStreet);
                if (_addrPOBox == null)
                    cm.Parameters.AddWithValue("@AddrPOBox", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrPOBox", _addrPOBox);
                if (_addrCity == null)
                    cm.Parameters.AddWithValue("@AddrCity", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrCity", _addrCity);
                if (_addrState == null)
                    cm.Parameters.AddWithValue("@AddrState", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrState", _addrState);
                if (_addrZipCode == null)
                    cm.Parameters.AddWithValue("@AddrZipCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrZipCode", _addrZipCode);
                if (_addrCountry == null)
                    cm.Parameters.AddWithValue("@AddrCountry", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrCountry", _addrCountry);
                if (_addrRegion == null)
                    cm.Parameters.AddWithValue("@AddrRegion", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrRegion", _addrRegion);
                if (_addrAttn == null)
                    cm.Parameters.AddWithValue("@AddrAttn", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrAttn", _addrAttn);
                if (_addrTel1 == null)
                    cm.Parameters.AddWithValue("@AddrTel1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrTel1", _addrTel1);
                if (_addrTel2 == null)
                    cm.Parameters.AddWithValue("@AddrTel2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrTel2", _addrTel2);
                if (_addrFax == null)
                    cm.Parameters.AddWithValue("@AddrFax", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrFax", _addrFax);
                if (_addrEmail == null)
                    cm.Parameters.AddWithValue("@addrEmail", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@addrEmail", _addrEmail);
                if (_addrShipViaKey == null)
                    cm.Parameters.AddWithValue("@AddrShipViaKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrShipViaKey", _addrShipViaKey);
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
                cm.Parameters["@NewAddrKey"].Direction = ParameterDirection.Output;

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.ExecuteNonQuery();

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                {
                    throw new TAException(MsgID.Common.UpdateFail);
                }
            }// Already close and dispose sql connection.

            return retValue;
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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFAddr_Delete";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@AddrLinkType", criteria._addrLinkType);
                cm.Parameters.AddWithValue("@AddrLinkKey", criteria._addrLinkKey);
                if (criteria._addrKey == null)
                    cm.Parameters.AddWithValue("@AddrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrKey", criteria._addrKey);
                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.ExecuteNonQuery();
                //retValue = true;
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                {
                    throw new TAException(MsgID.Common.DeleteFail);
                }
            }// Already close and dispose sql connection.
        }

        #endregion //Data Access - Delete

        #region Data Access - Validation

        internal bool Validation(Criteria criteria, bool isNew)
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

                    // Call validation method.
                    retValue = this.Validation(cn, criteria, isNew);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope            
            return retValue;
        }
        internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
        {
            bool retValue = false;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFAddr_Validation";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@IsNew", isNew);
                if (criteria._addrLinkKey == null)
                    cm.Parameters.AddWithValue("@AddrLinkKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AddrLinkKey", criteria._addrLinkKey);
                cm.Parameters.AddWithValue("@AddrLinkType", criteria._addrLinkType);
                cm.Parameters.AddWithValue("@AddrKey", criteria._addrKey);
                cm.Parameters.AddWithValue("@AddrID", criteria._addrID);

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.ExecuteNonQuery();

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                {
                    throw new TAException(MsgID.Common.ValidationFail);
                }
            }
            return retValue;
        }

        #endregion //Data Access - Validation

        private void Clear()
        {
            _addrKey = 0;
            _addrLinkType = null;
            _addrLinkKey = null;
            _addrID = string.Empty;
            _addrType = string.Empty;
            _addrStreet = string.Empty;
            _addrPOBox = string.Empty;
            _addrCity = string.Empty;
            _addrState = string.Empty;
            _addrZipCode = string.Empty;
            _addrCountry = string.Empty;
            _addrRegion = string.Empty;
            _addrAttn = string.Empty;
            _addrTel1 = string.Empty;
            _addrTel2 = string.Empty;
            _addrFax = string.Empty;
            _addrEmail = string.Empty;
            _addrShipViaKey = null;
            _createDate = null;
            _createUserKey = null;
            _lastModifiedDate = null;
            _lastModifiedUserKey = null;
            _custom1 = string.Empty;
            _custom2 = string.Empty;
            _custom3 = string.Empty;

        }

    }
}
