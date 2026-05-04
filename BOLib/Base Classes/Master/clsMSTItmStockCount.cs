

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
    public class MSTItmStockCount : Csla.BusinessBase<MSTItmStockCount>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _itmKey = null;
        internal int? _locKey = null;
        internal int? _batchKey = null;
        internal int? _serialKey = null;
        internal int? _itmType = null;
        internal string _itmID = string.Empty;
        internal string _itmDes = string.Empty;
        internal string _locID = string.Empty;
        internal string _batchID = string.Empty;
        internal string _serialID = string.Empty;
        internal int? _catKey1 = null;
        internal string _catID1 = string.Empty;
        internal int? _catKey2 = null;
        internal string _catID2 = string.Empty;
        internal int? _catKey3 = null;
        internal string _catID3 = string.Empty;
        internal int? _catKey4 = null;
        internal string _catID4 = string.Empty;
        internal int? _catKey5 = null;
        internal string _catID5 = string.Empty;
        internal int? _colorKey = null;
        internal string _colorID = string.Empty;
        internal string _scaleSize = string.Empty;
        internal int? _bUOMKey = null;
        internal string _buomid = string.Empty;
        internal int? _masterKey = null;
        internal string _masterID = string.Empty;
        internal string _masterDes = string.Empty;
        internal string _sku1 = string.Empty;
        internal string _sku2 = string.Empty;
        internal decimal? _freezeQty = null;
        internal DateTime? _freezeDate = null;
        internal decimal? _countQty = null;
        internal DateTime? _countDate = null;
        internal bool? _hasBeenCounted = null;
        internal decimal? _qtyToAdj = null;
        internal short? _docAdjGrp = null;
        internal bool? _docAdjDone = null;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;

        public int? ItmKey
        {
            get
            {
                return _itmKey;
            }
            set
            {
                _itmKey = value;
                PropertyHasChanged("ItmKey");
            }
        }

        public int? LocKey
        {
            get
            {
                return _locKey;
            }
            set
            {
                _locKey = value;
                PropertyHasChanged("LocKey");
            }
        }

        public int? BatchKey
        {
            get
            {
                return _batchKey;
            }
            set
            {
                _batchKey = value;
                PropertyHasChanged("BatchKey");
            }
        }

        public int? SerialKey
        {
            get
            {
                return _serialKey;
            }
            set
            {
                _serialKey = value;
                PropertyHasChanged("SerialKey");
            }
        }

        public int? ItmType
        {
            get
            {
                return _itmType;
            }
            set
            {
                _itmType = value;
                PropertyHasChanged("ItmType");
            }
        }

        public string ItmID
        {
            get
            {
                return _itmID;
            }
            set
            {
                _itmID = value;
                PropertyHasChanged("ItmID");
            }
        }

        public string ItmDes
        {
            get
            {
                return _itmDes;
            }
            set
            {
                _itmDes = value;
                PropertyHasChanged("ItmDes");
            }
        }

        public string LocID
        {
            get
            {
                return _locID;
            }
            set
            {
                _locID = value;
                PropertyHasChanged("LocID");
            }
        }

        public string BatchID
        {
            get
            {
                return _batchID;
            }
            set
            {
                _batchID = value;
                PropertyHasChanged("BatchID");
            }
        }

        public string SerialID
        {
            get
            {
                return _serialID;
            }
            set
            {
                _serialID = value;
                PropertyHasChanged("SerialID");
            }
        }

        public int? CatKey1
        {
            get
            {
                return _catKey1;
            }
            set
            {
                _catKey1 = value;
                PropertyHasChanged("CatKey1");
            }
        }

        public string CatID1
        {
            get
            {
                return _catID1;
            }
            set
            {
                _catID1 = value;
                PropertyHasChanged("CatID1");
            }
        }

        public int? CatKey2
        {
            get
            {
                return _catKey2;
            }
            set
            {
                _catKey2 = value;
                PropertyHasChanged("CatKey2");
            }
        }

        public string CatID2
        {
            get
            {
                return _catID2;
            }
            set
            {
                _catID2 = value;
                PropertyHasChanged("CatID2");
            }
        }

        public int? CatKey3
        {
            get
            {
                return _catKey3;
            }
            set
            {
                _catKey3 = value;
                PropertyHasChanged("CatKey3");
            }
        }

        public string CatID3
        {
            get
            {
                return _catID3;
            }
            set
            {
                _catID3 = value;
                PropertyHasChanged("CatID3");
            }
        }

        public int? CatKey4
        {
            get
            {
                return _catKey4;
            }
            set
            {
                _catKey4 = value;
                PropertyHasChanged("CatKey4");
            }
        }

        public string CatID4
        {
            get
            {
                return _catID4;
            }
            set
            {
                _catID4 = value;
                PropertyHasChanged("CatID4");
            }
        }

        public int? CatKey5
        {
            get
            {
                return _catKey5;
            }
            set
            {
                _catKey5 = value;
                PropertyHasChanged("CatKey5");
            }
        }

        public string CatID5
        {
            get
            {
                return _catID5;
            }
            set
            {
                _catID5 = value;
                PropertyHasChanged("CatID5");
            }
        }

        public int? ColorKey
        {
            get
            {
                return _colorKey;
            }
            set
            {
                _colorKey = value;
                PropertyHasChanged("ColorKey");
            }
        }

        public string ColorID
        {
            get
            {
                return _colorID;
            }
            set
            {
                _colorID = value;
                PropertyHasChanged("ColorID");
            }
        }

        public string ScaleSize
        {
            get
            {
                return _scaleSize;
            }
            set
            {
                _scaleSize = value;
                PropertyHasChanged("ScaleSize");
            }
        }

        public int? BUOMKey
        {
            get
            {
                return _bUOMKey;
            }
            set
            {
                _bUOMKey = value;
                PropertyHasChanged("BUOMKey");
            }
        }

        public string BUOMID
        {
            get
            {
                return _buomid;
            }
            set
            {
                _buomid = value;
                PropertyHasChanged("BUOMID");
            }
        }

        public int? MasterKey
        {
            get
            {
                return _masterKey;
            }
            set
            {
                _masterKey = value;
                PropertyHasChanged("MasterKey");
            }
        }

        public string MasterID
        {
            get
            {
                return _masterID;
            }
            set
            {
                _masterID = value;
                PropertyHasChanged("MasterID");
            }
        }

        public string MasterDes
        {
            get
            {
                return _masterDes;
            }
            set
            {
                _masterDes = value;
                PropertyHasChanged("MasterDes");
            }
        }

        public string SKU1
        {
            get
            {
                return _sku1;
            }
            set
            {
                _sku1 = value;
                PropertyHasChanged("SKU1");
            }
        }

        public string SKU2
        {
            get
            {
                return _sku2;
            }
            set
            {
                _sku2 = value;
                PropertyHasChanged("SKU2");
            }
        }

        public decimal? FreezeQty
        {
            get
            {
                return _freezeQty;
            }
            set
            {
                _freezeQty = value;
                PropertyHasChanged("FreezeQty");
            }
        }

        public DateTime? FreezeDate
        {
            get
            {
                return _freezeDate;
            }
            set
            {
                _freezeDate = value;
                PropertyHasChanged("FreezeDate");
            }
        }

        public decimal? CountQty
        {
            get
            {
                return _countQty;
            }
            set
            {
                _countQty = value;
                PropertyHasChanged("CountQty");
            }
        }

        public DateTime? CountDate
        {
            get
            {
                return _countDate;
            }
            set
            {
                _countDate = value;
                PropertyHasChanged("CountDate");
            }
        }

        public bool? HasBeenCounted
        {
            get
            {
                return _hasBeenCounted;
            }
            set
            {
                _hasBeenCounted = value;
                PropertyHasChanged("HasBeenCounted");
            }
        }

        public decimal? QtyToAdj
        {
            get
            {
                return _qtyToAdj;
            }
            set
            {
                _qtyToAdj = value;
                PropertyHasChanged("QtyToAdj");
            }
        }

        public short? DocAdjGrp
        {
            get
            {
                return _docAdjGrp;
            }
            set
            {
                _docAdjGrp = value;
                PropertyHasChanged("DocAdjGrp");
            }
        }

        public bool? DocAdjDone
        {
            get
            {
                return _docAdjDone;
            }
            set
            {
                _docAdjDone = value;
                PropertyHasChanged("DocAdjDone");
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
            return _itmKey.ToString() + _locKey.ToString() + _batchKey.ToString() + _serialKey.ToString();
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
           // ItmID
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "ItmID");
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ItmID", 50));
           //
           // ItmDes
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "ItmDes");
           //
           // LocID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LocID", 50));
           //
           // BatchID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BatchID", 50));
           //
           // SerialID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("SerialID", 50));
           //
           // CatID1
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CatID1", 50));
           //
           // CatID2
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CatID2", 50));
           //
           // CatID3
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CatID3", 50));
           //
           // CatID4
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CatID4", 50));
           //
           // CatID5
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CatID5", 50));
           //
           // ColorID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ColorID", 50));
           //
           // ScaleSize
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ScaleSize", 50));
           //
           // Buomid
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Buomid", 50));
           //
           // MasterID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MasterID", 50));
           //
           // MasterDes
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MasterDes", 255));
           //
           // Sku1
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Sku1", 50));
           //
           // Sku2
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Sku2", 50));
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

        internal MSTItmStockCount()
        { /* require use of factory method */ }

        internal static MSTItmStockCount New()
        {          
            MSTItmStockCount child = new MSTItmStockCount();          
            return child;
        }

        internal static MSTItmStockCount NewChild()
        {           
            MSTItmStockCount child = new MSTItmStockCount();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();          
            return child;
        }

        internal static MSTItmStockCount Get(SafeDataReader dr)
        {           
            MSTItmStockCount child = new MSTItmStockCount();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTItmStockCount Get(int? itmKey, int? locKey, int? batchKey, int? serialKey)
        {           
            MSTItmStockCount child = new MSTItmStockCount();
            child.Fetch(new Criteria(itmKey, locKey, batchKey, serialKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _itmKey = null;
            public int? _locKey = null;
            public int? _batchKey = null;
            public int? _serialKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? ItmKey, int? LocKey, int? BatchKey, int? SerialKey)
            {
                _itmKey = ItmKey;
                _locKey = LocKey;
                _batchKey = BatchKey;
                _serialKey = SerialKey;
            }

            internal Criteria(int? ItmKey, int? LocKey, int? BatchKey, int? SerialKey, int? Option)
            {
                _itmKey = ItmKey;
                _locKey = LocKey;
                _batchKey = BatchKey;
                _serialKey = SerialKey;
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
                cm.CommandText = "MSTItmStockCount_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                 
                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
                cm.Parameters.AddWithValue("@LocKey", criteria._locKey);
                cm.Parameters.AddWithValue("@BatchKey", criteria._batchKey);
                cm.Parameters.AddWithValue("@SerialKey", criteria._serialKey);                   

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

                // Check Return Value -- Changed By Richard
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
            _itmKey = dr.GetInt32("ItmKey");
            _locKey = dr.GetInt32("LocKey");
            _batchKey = dr.GetInt32("BatchKey");
            _serialKey = dr.GetInt32("SerialKey");
            _itmType = dr.GetInt32("ItmType");
            _itmID = dr.GetString("ItmID");
            _itmDes = dr.GetString("ItmDes");
            _locID = dr.GetString("LocID");
            _batchID = dr.GetString("BatchID");
            _serialID = dr.GetString("SerialID");
            _catKey1 = dr.GetInt32("CatKey1");
            _catID1 = dr.GetString("CatID1");
            _catKey2 = dr.GetInt32("CatKey2");
            _catID2 = dr.GetString("CatID2");
            _catKey3 = dr.GetInt32("CatKey3");
            _catID3 = dr.GetString("CatID3");
            _catKey4 = dr.GetInt32("CatKey4");
            _catID4 = dr.GetString("CatID4");
            _catKey5 = dr.GetInt32("CatKey5");
            _catID5 = dr.GetString("CatID5");
            _colorKey = dr.GetInt32("ColorKey");
            _colorID = dr.GetString("ColorID");
            _scaleSize = dr.GetString("ScaleSize");
            _bUOMKey = dr.GetInt32("BUOMKey");
            _buomid = dr.GetString("BUOMID");
            _masterKey = dr.GetInt32("MasterKey");
            _masterID = dr.GetString("MasterID");
            _masterDes = dr.GetString("MasterDes");
            _sku1 = dr.GetString("SKU1");
            _sku2 = dr.GetString("SKU2");
            _freezeQty = dr.GetDecimal("FreezeQty");
            _freezeDate = dr.GetDateTime("FreezeDate");
            _countQty = dr.GetDecimal("CountQty");
            _countDate = dr.GetDateTime("CountDate");
            _hasBeenCounted = dr.GetBoolean("HasBeenCounted");
            _qtyToAdj = dr.GetDecimal("QtyToAdj");
            _docAdjGrp = dr.GetInt16("DocAdjGrp");
            _docAdjDone = dr.GetBoolean("DocAdjDone");
            _createDate = dr.GetDateTime("CreateDate");
            _createUserKey = dr.GetInt32("CreateUserKey");
            _lastModifiedDate = dr.GetDateTime("LastModifiedDate");
            _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            ValidationRules.CheckRules();
            
            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? itmKey, out int? locKey, out int? batchKey, out int? serialKey)
        {
            bool retValue = false;          
            itmKey = null;
            locKey = null;
            batchKey = null;
            serialKey = null;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn,out itmKey, out locKey, out batchKey, out serialKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? itmKey, out int? locKey, out int? batchKey, out int? serialKey)
        {
            itmKey = 0;
            locKey = 0;
            batchKey = 0;
            serialKey = 0;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmStockCount_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewItmKey", itmKey);
                cm.Parameters.AddWithValue("@NewLocKey", locKey);
                cm.Parameters.AddWithValue("@NewBatchKey", batchKey);
                cm.Parameters.AddWithValue("@NewSerialKey", serialKey);

                if (_itmKey == null)
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_locKey == null)
                    cm.Parameters.AddWithValue("@LocKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LocKey", _locKey);

                if (_batchKey == null)
                    cm.Parameters.AddWithValue("@BatchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchKey", _batchKey);

                if (_serialKey == null)
                    cm.Parameters.AddWithValue("@SerialKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SerialKey", _serialKey);

                if (_itmType == null)
                    cm.Parameters.AddWithValue("@ItmType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmType", _itmType);

                if (_itmID == null)
                    cm.Parameters.AddWithValue("@ItmID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmID", _itmID);

                if (_itmDes == null)
                    cm.Parameters.AddWithValue("@ItmDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmDes", _itmDes);

                if (_locID == null)
                    cm.Parameters.AddWithValue("@LocID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LocID", _locID);

                if (_batchID == null)
                    cm.Parameters.AddWithValue("@BatchID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchID", _batchID);

                if (_serialID == null)
                    cm.Parameters.AddWithValue("@SerialID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SerialID", _serialID);

                if (_catKey1 == null)
                    cm.Parameters.AddWithValue("@CatKey1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatKey1", _catKey1);

                if (_catID1 == null)
                    cm.Parameters.AddWithValue("@CatID1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID1", _catID1);

                if (_catKey2 == null)
                    cm.Parameters.AddWithValue("@CatKey2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatKey2", _catKey2);

                if (_catID2 == null)
                    cm.Parameters.AddWithValue("@CatID2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID2", _catID2);

                if (_catKey3 == null)
                    cm.Parameters.AddWithValue("@CatKey3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatKey3", _catKey3);

                if (_catID3 == null)
                    cm.Parameters.AddWithValue("@CatID3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID3", _catID3);

                if (_catKey4 == null)
                    cm.Parameters.AddWithValue("@CatKey4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatKey4", _catKey4);

                if (_catID4 == null)
                    cm.Parameters.AddWithValue("@CatID4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID4", _catID4);

                if (_catKey5 == null)
                    cm.Parameters.AddWithValue("@CatKey5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatKey5", _catKey5);

                if (_catID5 == null)
                    cm.Parameters.AddWithValue("@CatID5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID5", _catID5);

                if (_colorKey == null)
                    cm.Parameters.AddWithValue("@ColorKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColorKey", _colorKey);

                if (_colorID == null)
                    cm.Parameters.AddWithValue("@ColorID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColorID", _colorID);

                if (_scaleSize == null)
                    cm.Parameters.AddWithValue("@ScaleSize", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ScaleSize", _scaleSize);

                if (_bUOMKey == null)
                    cm.Parameters.AddWithValue("@BUOMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BUOMKey", _bUOMKey);

                if (_buomid == null)
                    cm.Parameters.AddWithValue("@Buomid", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Buomid", _buomid);

                if (_masterKey == null)
                    cm.Parameters.AddWithValue("@MasterKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MasterKey", _masterKey);

                if (_masterID == null)
                    cm.Parameters.AddWithValue("@MasterID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MasterID", _masterID);

                if (_masterDes == null)
                    cm.Parameters.AddWithValue("@MasterDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MasterDes", _masterDes);

                if (_sku1 == null)
                    cm.Parameters.AddWithValue("@Sku1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Sku1", _sku1);

                if (_sku2 == null)
                    cm.Parameters.AddWithValue("@Sku2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Sku2", _sku2);

                if (_freezeQty == null)
                    cm.Parameters.AddWithValue("@FreezeQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@FreezeQty", _freezeQty);

                if (_freezeDate == null)
                    cm.Parameters.AddWithValue("@FreezeDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@FreezeDate", _freezeDate.Value);

                if (_countQty == null)
                    cm.Parameters.AddWithValue("@CountQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CountQty", _countQty);

                if (_countDate == null)
                    cm.Parameters.AddWithValue("@CountDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CountDate", _countDate.Value);

                if (_hasBeenCounted == null)
                    cm.Parameters.AddWithValue("@HasBeenCounted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@HasBeenCounted", _hasBeenCounted);

                if (_qtyToAdj == null)
                    cm.Parameters.AddWithValue("@QtyToAdj", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyToAdj", _qtyToAdj);

                if (_docAdjGrp == null)
                    cm.Parameters.AddWithValue("@DocAdjGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocAdjGrp", _docAdjGrp);

                if (_docAdjDone == null)
                    cm.Parameters.AddWithValue("@DocAdjDone", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocAdjDone", _docAdjDone);

                if (_createDate == null)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

                if (AppInfor.currentUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                     cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);

                if (_lastModifiedDate == null)
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

                if (_lastModifiedUserKey == null)
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", _lastModifiedUserKey);

                cm.Parameters["@NewItmKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewLocKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewBatchKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewSerialKey"].Direction = ParameterDirection.Output;
                cm.ExecuteNonQuery();
                itmKey = (int)cm.Parameters["@NewItmKey"].Value;
                locKey = (int)cm.Parameters["@NewLocKey"].Value;
                batchKey = (int)cm.Parameters["@NewBatchKey"].Value;
                serialKey = (int)cm.Parameters["@NewSerialKey"].Value;
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
                cm.CommandText = "MSTItmStockCount_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);                 

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewItmKey", 0);
                cm.Parameters.AddWithValue("@NewLocKey", 0);
                cm.Parameters.AddWithValue("@NewBatchKey", 0);
                cm.Parameters.AddWithValue("@NewSerialKey", 0);

                if (_itmKey == null)
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_locKey == null)
                    cm.Parameters.AddWithValue("@LocKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LocKey", _locKey);

                if (_batchKey == null)
                    cm.Parameters.AddWithValue("@BatchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchKey", _batchKey);

                if (_serialKey == null)
                    cm.Parameters.AddWithValue("@SerialKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SerialKey", _serialKey);

                if (_itmType == null)
                    cm.Parameters.AddWithValue("@ItmType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmType", _itmType);

                if (_itmID == null)
                    cm.Parameters.AddWithValue("@ItmID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmID", _itmID);

                if (_itmDes == null)
                    cm.Parameters.AddWithValue("@ItmDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmDes", _itmDes);

                if (_locID == null)
                    cm.Parameters.AddWithValue("@LocID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LocID", _locID);

                if (_batchID == null)
                    cm.Parameters.AddWithValue("@BatchID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BatchID", _batchID);

                if (_serialID == null)
                    cm.Parameters.AddWithValue("@SerialID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SerialID", _serialID);

                if (_catKey1 == null)
                    cm.Parameters.AddWithValue("@CatKey1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatKey1", _catKey1);

                if (_catID1 == null)
                    cm.Parameters.AddWithValue("@CatID1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID1", _catID1);

                if (_catKey2 == null)
                    cm.Parameters.AddWithValue("@CatKey2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatKey2", _catKey2);

                if (_catID2 == null)
                    cm.Parameters.AddWithValue("@CatID2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID2", _catID2);

                if (_catKey3 == null)
                    cm.Parameters.AddWithValue("@CatKey3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatKey3", _catKey3);

                if (_catID3 == null)
                    cm.Parameters.AddWithValue("@CatID3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID3", _catID3);

                if (_catKey4 == null)
                    cm.Parameters.AddWithValue("@CatKey4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatKey4", _catKey4);

                if (_catID4 == null)
                    cm.Parameters.AddWithValue("@CatID4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID4", _catID4);

                if (_catKey5 == null)
                    cm.Parameters.AddWithValue("@CatKey5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatKey5", _catKey5);

                if (_catID5 == null)
                    cm.Parameters.AddWithValue("@CatID5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID5", _catID5);

                if (_colorKey == null)
                    cm.Parameters.AddWithValue("@ColorKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColorKey", _colorKey);

                if (_colorID == null)
                    cm.Parameters.AddWithValue("@ColorID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColorID", _colorID);

                if (_scaleSize == null)
                    cm.Parameters.AddWithValue("@ScaleSize", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ScaleSize", _scaleSize);

                if (_bUOMKey == null)
                    cm.Parameters.AddWithValue("@BUOMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BUOMKey", _bUOMKey);

                if (_buomid == null)
                    cm.Parameters.AddWithValue("@Buomid", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Buomid", _buomid);

                if (_masterKey == null)
                    cm.Parameters.AddWithValue("@MasterKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MasterKey", _masterKey);

                if (_masterID == null)
                    cm.Parameters.AddWithValue("@MasterID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MasterID", _masterID);

                if (_masterDes == null)
                    cm.Parameters.AddWithValue("@MasterDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MasterDes", _masterDes);

                if (_sku1 == null)
                    cm.Parameters.AddWithValue("@Sku1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Sku1", _sku1);

                if (_sku2 == null)
                    cm.Parameters.AddWithValue("@Sku2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Sku2", _sku2);

                if (_freezeQty == null)
                    cm.Parameters.AddWithValue("@FreezeQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@FreezeQty", _freezeQty);

                if (_freezeDate == null)
                    cm.Parameters.AddWithValue("@FreezeDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@FreezeDate", _freezeDate.Value);

                if (_countQty == null)
                    cm.Parameters.AddWithValue("@CountQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CountQty", _countQty);

                if (_countDate == null)
                    cm.Parameters.AddWithValue("@CountDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CountDate", _countDate.Value);

                if (_hasBeenCounted == null)
                    cm.Parameters.AddWithValue("@HasBeenCounted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@HasBeenCounted", _hasBeenCounted);

                if (_qtyToAdj == null)
                    cm.Parameters.AddWithValue("@QtyToAdj", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyToAdj", _qtyToAdj);

                if (_docAdjGrp == null)
                    cm.Parameters.AddWithValue("@DocAdjGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocAdjGrp", _docAdjGrp);

                if (_docAdjDone == null)
                    cm.Parameters.AddWithValue("@DocAdjDone", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocAdjDone", _docAdjDone);

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

                cm.Parameters["@NewItmKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewLocKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewBatchKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewSerialKey"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "MSTItmStockCount_Delete";

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
                cm.Parameters.AddWithValue("@LocKey", criteria._locKey);
                cm.Parameters.AddWithValue("@BatchKey", criteria._batchKey);
                cm.Parameters.AddWithValue("@SerialKey", criteria._serialKey);

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

        internal bool Validation(Criteria criteria,bool? isNew)
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
                cm.CommandText = "MSTItmStockCount_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
                cm.Parameters.AddWithValue("@LocKey", criteria._locKey);
                cm.Parameters.AddWithValue("@BatchKey", criteria._batchKey);
                cm.Parameters.AddWithValue("@SerialKey", criteria._serialKey);

                cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }
            
        }
        #endregion //Data Access - Validation
    }
}


