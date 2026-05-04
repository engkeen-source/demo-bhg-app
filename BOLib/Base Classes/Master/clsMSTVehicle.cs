using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTVehicle : Csla.BusinessBase<MSTVehicle>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _vehicleKey = 0;
        internal int? _conKey = null;
        internal string _vehicle = string.Empty;
        internal string _plateNo = string.Empty;
        internal DateTime? _roadTaxExpiry = null;
        internal DateTime? _insuranceExpiry = null;
        internal string _chassis = null;
        internal string _engine = null;
        internal DateTime? _vehicleDateIn = null;
        internal string _mileage = null;
        internal string _brand = null;
        internal string _model = null;
        internal string _colour = null;
        internal DateTime? _registryDate = null;
        internal string _engineCapacity = null;
        internal int? _manufactureYear = null;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        //internal int? _accessLevel = 0;
        //internal int? _accessGroup = 0;

        public int? VehicleKey
        {
            get
            {
                return _vehicleKey;
            }
        }

        public int? ConKey
        {
            get
            {
                return _conKey;
            }
            set
            {
                _conKey = value;
                PropertyHasChanged("ConKey");
            }
        }

        public string Vehicle
        {
            get
            {
                return _vehicle;
            }
            set
            {
                _vehicle = value;
                PropertyHasChanged("Vehicle");
            }
        }

        public string PlateNo
        {
            get
            {
                return _plateNo;
            }
            set
            {
                _plateNo = value;
                PropertyHasChanged("PlateNo");
            }
        }

        public string Chassis
        {
            get
            {
                return _chassis;
            }
            set
            {
                _chassis = value;
                PropertyHasChanged("Chassis");
            }
        }


        public string Engine
        {
            get
            {
                return _engine;
            }
            set
            {
                _engine = value;
                PropertyHasChanged("Engine");
            }
        }

        public DateTime? RoadTaxExpiry
        {
            get
            {
                return _roadTaxExpiry;
            }
            set
            {
                _roadTaxExpiry = value;
                PropertyHasChanged("RoadTaxExpiry");
            }
        }

        public DateTime? InsuranceExpiry
        {
            get
            {
                return _insuranceExpiry;
            }
            set
            {
                _insuranceExpiry = value;
                PropertyHasChanged("InsuranceExpiry");
            }
        }

        public DateTime? VehicleDateIn
        {
            get
            {
                return _vehicleDateIn;
            }
            set
            {
                _vehicleDateIn = value;
                PropertyHasChanged("VehicleDateIn");
            }
        }

        public string Mileage
        {
            get
            {
                return _mileage;
            }
            set
            {
                _mileage = value;
                PropertyHasChanged("Mileage");
            }
        }

        public string Brand
        {
            get
            {
                return _brand;
            }
            set
            {
                _brand = value;
                PropertyHasChanged("Brand");
            }
        }

        public string Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                PropertyHasChanged("Model");
            }
        }

        public string Colour
        {
            get
            {
                return _colour;
            }
            set
            {
                _colour = value;
                PropertyHasChanged("Colour");
            }
        }

        public DateTime? RegistryDate
        {
            get
            {
                return _registryDate;
            }
            set
            {
                _registryDate = value;
                PropertyHasChanged("RegistryDate");
            }
        }

        public string EngineCapacity
        {
            get
            {
                return _engineCapacity;
            }
            set
            {
                _engineCapacity = value;
                PropertyHasChanged("EngineCapacity");
            }
        }

        public int? ManufactureYear
        {
            get
            {
                return _manufactureYear;
            }
            set
            {
                _manufactureYear = value;
                PropertyHasChanged("ManufactureYear");
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

        //public int? AccessLevel
        //{
        //    get
        //    {
        //        return _accessLevel;
        //    }
        //    set
        //    {
        //        _accessLevel = value;
        //        PropertyHasChanged("AccessLevel");
        //    }
        //}

        //public int? AccessGroup
        //{
        //    get
        //    {
        //        return _accessGroup;
        //    }
        //    set
        //    {
        //        if (_accessGroup != value)
        //        {
        //            _accessGroup = value;
        //            PropertyHasChanged("AccessGroup");
        //        }
        //    }
        //}

        protected override object GetIdValue()
        {
            return _vehicleKey.ToString();
        }

        #endregion //Business Properties and Methods

        #region Factory Methods

        internal MSTVehicle()
        { /* require use of factory method */ }

        internal static MSTVehicle New()
        {
            MSTVehicle child = new MSTVehicle();         
            return child;
        }

        internal static MSTVehicle NewChild()
        {
            MSTVehicle child = new MSTVehicle();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static MSTVehicle Get(SafeDataReader dr)
        {
            MSTVehicle child = new MSTVehicle();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTVehicle Get(int? vehicleKey)
        {
            MSTVehicle child = new MSTVehicle();
            child.Fetch(new Criteria(vehicleKey, 1));
            return child;
        }

        public static MSTVehicle Get(string vehicle)
        {
            MSTVehicle child = new MSTVehicle();
            child.Fetch(new Criteria(vehicle, 2));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _vehicleKey = null;
            public string _vehicle = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? VehicleKey)
            {
                _vehicleKey = VehicleKey;
            }

            internal Criteria(int? VehicleKey, int? Option)
            {
                _vehicleKey = VehicleKey;
                _option = Option;
            }

            internal Criteria(int? VehicleKey, string KeyID)
            {
                _vehicleKey = VehicleKey;
                _vehicle = KeyID;
            }

            internal Criteria(string Vehicle, int? Option)
            {
                _vehicle = Vehicle;
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
                cm.CommandText = "MSTVehicle_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@vehicleKey", criteria._vehicleKey);


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

                }// Already close and dispose data reader.



                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.                       

            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _vehicleKey = dr.GetInt32("VehicleKey");
            _conKey = dr.GetInt32("ConKey");
            _vehicle = dr.GetString("Vehicle");
            _plateNo = dr.GetString("PlateNo");
            _roadTaxExpiry = dr.GetDateTime("RoadTaxExpiry");
            _insuranceExpiry = dr.GetDateTime("InsuranceExpiry");
            _chassis = dr.GetString("Chassis");
            _engine = dr.GetString("Engine");
            _vehicleDateIn = dr.GetDateTime("VehicleDateIn");
            _mileage = dr.GetString("Mileage");
            _brand = dr.GetString("Brand");
            _model = dr.GetString("Model");
            _colour = dr.GetString("Colour");
            _registryDate = dr.GetDateTime("RegistryDate");
            _engineCapacity = dr.GetString("EngineCapacity");
            _manufactureYear = dr.GetInt32("ManufactureYear");
            _createDate = dr.GetDateTime("CreateDate");
            _createUserKey = dr.GetInt32("CreateUserKey");
            _lastModifiedDate = dr.GetDateTime("LastModifiedDate");
            _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            //_accessLevel = dr.GetInt32("AccessLevel");
            //_accessGroup = dr.GetInt32("AccessGroup");
            ValidationRules.CheckRules();
            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? vehicleKey)
        {
            bool retValue = false;
            vehicleKey = null;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out vehicleKey);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope               

            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? vehicleKey)
        {
            bool retValue = false;
            vehicleKey = 0;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTVehicle_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@NewVehicleKey", vehicleKey);

                if (_vehicleKey == null)
                    cm.Parameters.AddWithValue("@VehicleKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VehicleKey", _vehicleKey);

                if (_conKey == null)
                    cm.Parameters.AddWithValue("@ConKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey", _conKey);

                if (_vehicle == null)
                    cm.Parameters.AddWithValue("@Vehicle", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Vehicle", _vehicle);

                if (_plateNo == null)
                    cm.Parameters.AddWithValue("@PlateNo", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PlateNo", _plateNo);

                if (_roadTaxExpiry == null)
                    cm.Parameters.AddWithValue("@RoadTaxExpiry", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RoadTaxExpiry", _roadTaxExpiry.Value);

                if (_insuranceExpiry == null)
                    cm.Parameters.AddWithValue("@InsuranceExpiry", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@InsuranceExpiry", _insuranceExpiry.Value);

                if (_chassis == null)
                    cm.Parameters.AddWithValue("@Chassis", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Chassis", _chassis);

                if (_engine == null)
                    cm.Parameters.AddWithValue("@Engine", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Engine", _engine);

                if (_vehicleDateIn == null)
                    cm.Parameters.AddWithValue("@VehicleDateIn", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VehicleDateIn", _vehicleDateIn.Value);

                if (_mileage == null)
                    cm.Parameters.AddWithValue("@Mileage", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Mileage", _mileage);

                if (_brand == null)
                    cm.Parameters.AddWithValue("@Brand", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Brand", _brand);

                if (_model == null)
                    cm.Parameters.AddWithValue("@Model", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Model", _model);

                if (_colour == null)
                    cm.Parameters.AddWithValue("@Colour", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Colour", _colour);

                if (_registryDate == null)
                    cm.Parameters.AddWithValue("@RegistryDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RegistryDate", _registryDate.Value);

                if (_engineCapacity == null)
                    cm.Parameters.AddWithValue("@EngineCapacity", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EngineCapacity", _engineCapacity);

                if (_manufactureYear == null)
                    cm.Parameters.AddWithValue("@ManufactureYear", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ManufactureYear", _manufactureYear);

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

                //if (_accessLevel == null)
                //    cm.Parameters.AddWithValue("@AccessLevel", DBNull.Value);
                //else
                //    cm.Parameters.AddWithValue("@AccessLevel", _accessLevel);

                //if (_accessGroup == null)
                //    cm.Parameters.AddWithValue("@AccessGroup", DBNull.Value);
                //else
                //    cm.Parameters.AddWithValue("@AccessGroup", _accessGroup);

                cm.Parameters["@NewVehicleKey"].Direction = ParameterDirection.InputOutput;

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                vehicleKey = (int)cm.Parameters["@NewVehicleKey"].Value;

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

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


            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTVehicle_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewVehicleKey", 0);
                if (_vehicleKey == null)
                    cm.Parameters.AddWithValue("@VehicleKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VehicleKey", _vehicleKey);

                if (_conKey == null)
                    cm.Parameters.AddWithValue("@ConKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey", _conKey);

                if (_vehicle == null)
                    cm.Parameters.AddWithValue("@Vehicle", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Vehicle", _vehicle);

                if (_plateNo == null)
                    cm.Parameters.AddWithValue("@PlateNo", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PlateNo", _plateNo);

                if (_roadTaxExpiry == null)
                    cm.Parameters.AddWithValue("@RoadTaxExpiry", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RoadTaxExpiry", _roadTaxExpiry.Value);

                if (_insuranceExpiry == null)
                    cm.Parameters.AddWithValue("@InsuranceExpiry", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@InsuranceExpiry", _insuranceExpiry.Value);

                if (_chassis == null)
                    cm.Parameters.AddWithValue("@Chassis", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Chassis", _chassis);

                if (_engine == null)
                    cm.Parameters.AddWithValue("@Engine", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Engine", _engine);

                if (_vehicleDateIn == null)
                    cm.Parameters.AddWithValue("@VehicleDateIn", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VehicleDateIn", _vehicleDateIn.Value);

                if (_mileage == null)
                    cm.Parameters.AddWithValue("@Mileage", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Mileage", _mileage);

                if (_brand == null)
                    cm.Parameters.AddWithValue("@Brand", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Brand", _brand);

                if (_model == null)
                    cm.Parameters.AddWithValue("@Model", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Model", _model);

                if (_colour == null)
                    cm.Parameters.AddWithValue("@Colour", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Colour", _colour);

                if (_registryDate == null)
                    cm.Parameters.AddWithValue("@RegistryDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RegistryDate", _registryDate.Value);

                if (_engineCapacity == null)
                    cm.Parameters.AddWithValue("@EngineCapacity", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EngineCapacity", _engineCapacity);

                if (_manufactureYear == null)
                    cm.Parameters.AddWithValue("@ManufactureYear", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ManufactureYear", _manufactureYear);

                if (_createDate == null)
                {
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                }
                else
                {
                    if (this.IsValidSQLDateTime(_createDate.Value))
                    {
                        cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);
                    }
                    else
                    {
                        DateTime minDateTime = DateTime.MinValue;
                        minDateTime = new DateTime(1753, 1, 1);

                        cm.Parameters.AddWithValue("@CreateDate", minDateTime);
                    }
                }

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

                //if (_accessLevel == null)
                //    cm.Parameters.AddWithValue("@AccessLevel", DBNull.Value);
                //else
                //    cm.Parameters.AddWithValue("@AccessLevel", _accessLevel);

                //if (_accessGroup == null)
                //    cm.Parameters.AddWithValue("@AccessGroup", DBNull.Value);
                //else
                //    cm.Parameters.AddWithValue("@AccessGroup", _accessGroup);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewVehicleKey"].Direction = ParameterDirection.InputOutput;

                cm.ExecuteNonQuery();

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
                cm.CommandText = "MSTVehicle_Delete";

                cm.Parameters.AddWithValue("@VehicleKey", criteria._vehicleKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();



                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
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
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTVehicle_Validation";


                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@VehicleKey", criteria._vehicleKey);
                cm.Parameters.AddWithValue("@Vehicle", criteria._vehicle);
                cm.Parameters.AddWithValue("@RetValue", 0);

                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();


                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }

        }
        #endregion //Data Access - Validation


        internal bool CanAccessRecord(int? vehicleKey)
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
                    retValue = this.CanAccessRecord(cn, vehicleKey);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool CanAccessRecord(SqlConnection cn, int? vehicleKey)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECRecAccess_Check";


                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@Option", 3);
                cm.Parameters.AddWithValue("@Key", vehicleKey);

                cm.Parameters.AddWithValue("@UserAccessLevel", AppInfor.conAccessLevel);
                cm.Parameters.AddWithValue("@UserKey", AppInfor.CurrentUserKey);

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }
        }
    

        private void Clear()
        {
            _vehicleKey = 0;
            _conKey = null;
            _vehicle = string.Empty;
            _plateNo = string.Empty;
            _roadTaxExpiry = null;
            _insuranceExpiry = null;
            _chassis = string.Empty;
            _engine = string.Empty;
            _vehicleDateIn = null;
            _mileage = string.Empty;
            _brand = string.Empty;
            _model = string.Empty;
            _colour = string.Empty;
            _registryDate = null;
            _engineCapacity = string.Empty;
            _manufactureYear = null;
            _createDate = null;
            _createUserKey = null;
            _lastModifiedDate = null;
            _lastModifiedUserKey = null;

        }

        internal bool IsValidSQLDateTime(DateTime dDate)
        {
            bool valid = false;

            DateTime minDateTime = DateTime.MinValue;
            DateTime maxDateTime = DateTime.MaxValue;

            minDateTime = new DateTime(1753, 1, 1);
            maxDateTime = new DateTime(9999, 12, 31, 23, 59, 59, 997);

            if (dDate >= minDateTime && dDate <= maxDateTime)
            {
                valid = true;
            }

            return valid;
        }
    }
}
