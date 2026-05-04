using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    public class ARDOTrack : Csla.BusinessBase<ARDOTrack>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _id = 0;
        internal string _orderNo = string.Empty;
        internal string _referenceNo = string.Empty;
        internal string _transactionID = string.Empty;
        internal DateTime? _shippingDate = null;
        internal string _location = string.Empty;
        internal string _activity = string.Empty;
        internal string _createdBy = string.Empty;
        internal DateTime? _createdDate = null;
        internal string _lastModifiedBy = string.Empty;
        internal DateTime? _lastModifiedDate = null;

        public int? ID
        {
            get
            {
                return _id;
            }
        }

        public string order_no
        {
            get
            {
                return _orderNo;
            }
            set
            {
                _orderNo = value;
                PropertyHasChanged("order_no");
            }
        }

        public string reference_no
        {
            get
            {
                return _referenceNo;
            }
            set
            {
                _referenceNo = value;
                PropertyHasChanged("reference_no");
            }
        }

        public string transaction_id
        {
            get
            {
                return _transactionID;
            }
            set
            {
                _transactionID = value;
                PropertyHasChanged("transaction_id");
            }
        }

        public DateTime? shipping_date
        {
            get
            {
                return _shippingDate;
            }
            set
            {
                _shippingDate = value;
                PropertyHasChanged("shipping_date");
            }
        }

        public string location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                PropertyHasChanged("location");
            }
        }

        public string activity
        {
            get
            {
                return _activity;
            }
            set
            {
                _activity = value;
                PropertyHasChanged("activity");
            }
        }

        public string created_by
        {
            get
            {
                return _createdBy;
            }
            set
            {
                _createdBy = value;
                PropertyHasChanged("created_by");
            }
        }

        public DateTime? created_date
        {
            get
            {
                return _createdDate;
            }
            set
            {
                _createdDate = value;
                PropertyHasChanged("created_date");
            }
        }

        public string last_modified_by
        {
            get
            {
                return _lastModifiedBy;
            }
            set
            {
                _lastModifiedBy = value;
                PropertyHasChanged("last_modified_by");
            }
        }

        public DateTime? last_modified_date
        {
            get
            {
                return _lastModifiedDate;
            }
            set
            {
                _lastModifiedDate = value;
                PropertyHasChanged("last_modified_date");
            }
        }
        #endregion //Business Properties and Methods

        internal ARDOTrack()
        { /* require use of factory method */ }

        internal static ARDOTrack New()
        {
            ARDOTrack child = new ARDOTrack();         
            return child;
        }

        internal static ARDOTrack NewChild()
        {
            ARDOTrack child = new ARDOTrack();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal bool Fetch(Criteria criteria)
        {
            bool retValue = false;
            //// Using existing sql connection.
            //using (SqlCommand cm = cn.CreateCommand())
            //{
            //    cm.CommandType = CommandType.StoredProcedure;
            //    cm.CommandText = "MSTVehicle_Get";

            //    cm.Parameters.AddWithValue("@Option", criteria._option);

            //    cm.Parameters.AddWithValue("@vehicleKey", criteria._vehicleKey);


            //    cm.Parameters.AddWithValue("@RetValue", 0);
            //    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

            //    // Using data reader as record set.
            //    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
            //    {
            //        // If data reader can read, continue...
            //        if (dr.Read())
            //        {
            //            retValue = this.Fetch(dr);
            //        }
            //        else
            //            this.Clear();

            //    }// Already close and dispose data reader.



            //    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
            //        retValue = true;
            //    else
            //        retValue = false;

            //}// Already close and dispose sql connection.                       

            return retValue;
        }

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _id = null;
            public string _orderNo = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? ID)
            {
                _id = ID;
            }

        }

        #endregion //Criteria

        internal bool Insert(out int? id)
        {
            bool retValue = false;
            id = 0;
            // Using existing sql connection.
            //using (SqlCommand cm = cn.CreateCommand())
            //{
            //    // Get current user key
            //    _createUserKey = AppInfor.currentUserKey;
            //    _lastModifiedUserKey = AppInfor.currentUserKey;

            //    cm.CommandType = CommandType.StoredProcedure;
            //    cm.CommandText = "MSTVehicle_AddUpdate";

            //    cm.Parameters.AddWithValue("@Option", 0);

            //    cm.Parameters.AddWithValue("@NewVehicleKey", vehicleKey);

            //    if (_vehicleKey == null)
            //        cm.Parameters.AddWithValue("@VehicleKey", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@VehicleKey", _vehicleKey);

            //    if (_conKey == null)
            //        cm.Parameters.AddWithValue("@ConKey", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@ConKey", _conKey);

            //    if (_vehicle == null)
            //        cm.Parameters.AddWithValue("@Vehicle", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@Vehicle", _vehicle);

            //    if (_plateNo == null)
            //        cm.Parameters.AddWithValue("@PlateNo", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@PlateNo", _plateNo);

            //    if (_roadTaxExpiry == null)
            //        cm.Parameters.AddWithValue("@RoadTaxExpiry", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@RoadTaxExpiry", _roadTaxExpiry.Value);

            //    if (_insuranceExpiry == null)
            //        cm.Parameters.AddWithValue("@InsuranceExpiry", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@InsuranceExpiry", _insuranceExpiry.Value);

            //    if (_chassis == null)
            //        cm.Parameters.AddWithValue("@Chassis", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@Chassis", _chassis);

            //    if (_engine == null)
            //        cm.Parameters.AddWithValue("@Engine", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@Engine", _engine);

            //    if (_vehicleDateIn == null)
            //        cm.Parameters.AddWithValue("@VehicleDateIn", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@VehicleDateIn", _vehicleDateIn.Value);

            //    if (_mileage == null)
            //        cm.Parameters.AddWithValue("@Mileage", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@Mileage", _mileage);

            //    if (_brand == null)
            //        cm.Parameters.AddWithValue("@Brand", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@Brand", _brand);

            //    if (_model == null)
            //        cm.Parameters.AddWithValue("@Model", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@Model", _model);

            //    if (_colour == null)
            //        cm.Parameters.AddWithValue("@Colour", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@Colour", _colour);

            //    if (_registryDate == null)
            //        cm.Parameters.AddWithValue("@RegistryDate", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@RegistryDate", _registryDate.Value);

            //    if (_engineCapacity == null)
            //        cm.Parameters.AddWithValue("@EngineCapacity", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@EngineCapacity", _engineCapacity);

            //    if (_manufactureYear == null)
            //        cm.Parameters.AddWithValue("@ManufactureYear", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@ManufactureYear", _manufactureYear);

            //    if (_createDate == null)
            //        cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

            //    if (AppInfor.currentUserKey == null)
            //        cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);

            //    if (_lastModifiedDate == null)
            //        cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

            //    if (_lastModifiedUserKey == null)
            //        cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@LastModifiedUserKey", _lastModifiedUserKey);

            //    //if (_accessLevel == null)
            //    //    cm.Parameters.AddWithValue("@AccessLevel", DBNull.Value);
            //    //else
            //    //    cm.Parameters.AddWithValue("@AccessLevel", _accessLevel);

            //    //if (_accessGroup == null)
            //    //    cm.Parameters.AddWithValue("@AccessGroup", DBNull.Value);
            //    //else
            //    //    cm.Parameters.AddWithValue("@AccessGroup", _accessGroup);

            //    cm.Parameters["@NewVehicleKey"].Direction = ParameterDirection.InputOutput;

            //    cm.Parameters.AddWithValue("@RetValue", 0);
            //    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

            //    cm.ExecuteNonQuery();

            //    vehicleKey = (int)cm.Parameters["@NewVehicleKey"].Value;

            //    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
            //        retValue = true;
            //    else
            //        retValue = false;

            //}// Already close and dispose sql connection.            

            return retValue;
        }

        internal bool Update()
        {


            //// Using existing sql connection.
            //using (SqlCommand cm = cn.CreateCommand())
            //{

            //    cm.CommandType = CommandType.StoredProcedure;
            //    cm.CommandText = "MSTVehicle_AddUpdate";

            //    cm.Parameters.AddWithValue("@Option", 1);
            //    cm.Parameters.AddWithValue("@NewVehicleKey", 0);
            //    if (_vehicleKey == null)
            //        cm.Parameters.AddWithValue("@VehicleKey", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@VehicleKey", _vehicleKey);

            //    if (_conKey == null)
            //        cm.Parameters.AddWithValue("@ConKey", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@ConKey", _conKey);

            //    if (_vehicle == null)
            //        cm.Parameters.AddWithValue("@Vehicle", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@Vehicle", _vehicle);

            //    if (_plateNo == null)
            //        cm.Parameters.AddWithValue("@PlateNo", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@PlateNo", _plateNo);

            //    if (_roadTaxExpiry == null)
            //        cm.Parameters.AddWithValue("@RoadTaxExpiry", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@RoadTaxExpiry", _roadTaxExpiry.Value);

            //    if (_insuranceExpiry == null)
            //        cm.Parameters.AddWithValue("@InsuranceExpiry", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@InsuranceExpiry", _insuranceExpiry.Value);

            //    if (_chassis == null)
            //        cm.Parameters.AddWithValue("@Chassis", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@Chassis", _chassis);

            //    if (_engine == null)
            //        cm.Parameters.AddWithValue("@Engine", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@Engine", _engine);

            //    if (_vehicleDateIn == null)
            //        cm.Parameters.AddWithValue("@VehicleDateIn", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@VehicleDateIn", _vehicleDateIn.Value);

            //    if (_mileage == null)
            //        cm.Parameters.AddWithValue("@Mileage", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@Mileage", _mileage);

            //    if (_brand == null)
            //        cm.Parameters.AddWithValue("@Brand", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@Brand", _brand);

            //    if (_model == null)
            //        cm.Parameters.AddWithValue("@Model", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@Model", _model);

            //    if (_colour == null)
            //        cm.Parameters.AddWithValue("@Colour", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@Colour", _colour);

            //    if (_registryDate == null)
            //        cm.Parameters.AddWithValue("@RegistryDate", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@RegistryDate", _registryDate.Value);

            //    if (_engineCapacity == null)
            //        cm.Parameters.AddWithValue("@EngineCapacity", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@EngineCapacity", _engineCapacity);

            //    if (_manufactureYear == null)
            //        cm.Parameters.AddWithValue("@ManufactureYear", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@ManufactureYear", _manufactureYear);

            //    if (_createDate == null)
            //    {
            //        cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
            //    }
            //    else
            //    {
            //        if (this.IsValidSQLDateTime(_createDate.Value))
            //        {
            //            cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);
            //        }
            //        else
            //        {
            //            DateTime minDateTime = DateTime.MinValue;
            //            minDateTime = new DateTime(1753, 1, 1);

            //            cm.Parameters.AddWithValue("@CreateDate", minDateTime);
            //        }
            //    }

            //    if (_createUserKey == null)
            //        cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@CreateUserKey", _createUserKey);

            //    if (_lastModifiedDate == null)
            //        cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

            //    if (AppInfor.currentUserKey == null)
            //        cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

            //    //if (_accessLevel == null)
            //    //    cm.Parameters.AddWithValue("@AccessLevel", DBNull.Value);
            //    //else
            //    //    cm.Parameters.AddWithValue("@AccessLevel", _accessLevel);

            //    //if (_accessGroup == null)
            //    //    cm.Parameters.AddWithValue("@AccessGroup", DBNull.Value);
            //    //else
            //    //    cm.Parameters.AddWithValue("@AccessGroup", _accessGroup);

            //    cm.Parameters.AddWithValue("@RetValue", 0);
            //    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
            //    cm.Parameters["@NewVehicleKey"].Direction = ParameterDirection.InputOutput;

            //    cm.ExecuteNonQuery();

            //    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
            //        return true;
            //    else
                    return false;
            //}// Already close and dispose sql connection.

        }

        internal bool Delete(Criteria criteria)
        {
            //// Using existing sql connection.
            //using (SqlCommand cm = cn.CreateCommand())
            //{
            //    cm.CommandType = CommandType.StoredProcedure;
            //    cm.CommandText = "MSTVehicle_Delete";

            //    cm.Parameters.AddWithValue("@VehicleKey", criteria._vehicleKey);

            //    cm.Parameters.AddWithValue("@RetValue", 0);
            //    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

            //    cm.ExecuteNonQuery();



            //    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
            //        return true;
            //    else
                    return false;
            //}// Already close and dispose sql connection.

        }
    }
}
