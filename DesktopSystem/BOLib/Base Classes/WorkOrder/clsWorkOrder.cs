using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using TAUtil;

namespace BOLib
{
    [Serializable]
    public class WorkOrder :Document, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++

        internal int? _workOrderKey;
        internal int? _invoiceKey;
        internal string _workOrderNo;
        internal int? _workOrderTypeKey;
        internal int? _incharge1;
        internal int? _incharge2;
        internal int? _vehicleKey;
        internal string _mileage;
        internal string _colour;
        internal int? _conKey;
        internal string _conNm;
        internal string _contactPerson;
        internal string _mobilePhone;
        internal string _email;
        internal DateTime? _dateIn;
        internal DateTime? _dateOutReq;
        internal DateTime? _dateOutAct;
        internal int? _statusKey;
        internal string _remark;
        internal int? _deptKey;
        internal string _recommendedBy;
        internal string _additionalRemark;
        internal string _brand;
        internal string _model;

        internal bool? _ShowEnquiryForm;
        internal SYSAttachments attachments = new SYSAttachments();

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public WorkOrder()
            :base()
        {
            this.Clear();
            base.PropertyChanged += new PropertyChangedEventHandler(WorkOrder_PropertyChanged);
        }
        void WorkOrder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }

        private void Clear()
        {
            this._workOrderKey = 0;
            this._invoiceKey = null;
            this._workOrderNo = string.Empty;
            this._workOrderTypeKey = 0;
            this._incharge1 = null;
            this._incharge2 = null;
            this._vehicleKey = null;
            this._mileage = string.Empty;
            this._colour = string.Empty;
            this._conKey = null;
            this._conNm = string.Empty;
            this._contactPerson = string.Empty;
            this._mobilePhone = string.Empty;
            this._email = string.Empty;
            this._dateIn = null;
            this._dateOutReq = null;
            this._dateOutAct = null;
            this._statusKey = 0;
            this._remark = string.Empty;
            this._brand = string.Empty;
            this._model = string.Empty;
            this._deptKey = 0;
            this._recommendedBy = string.Empty;
            this._additionalRemark = string.Empty;
            this._ShowEnquiryForm = false;
            this._isDirty = false;
        }
        public static WorkOrder Get(int? workOrderKey)
        {
            WorkOrder child = new WorkOrder();
            child.Fetch(new Criteria(workOrderKey, 1));
            return child;
        }
        public WorkOrder Clone()
        {

            WorkOrder objCopy = (WorkOrder)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
        }


        /// <summary>
        /// Disposing objects
        /// </summary>
        public void Dispose()
        {
        }

        #endregion

        private void NotifyPropertyChanged(String info)
        {
            base._isDirty = true;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #region +++  Properties  +++

        public int? WorkOrderKey
        {
            get
            {
                return this._workOrderKey;
            }
            set
            {
                this._workOrderKey = value;
                NotifyPropertyChanged("WorkOrderKey");
            }
        }

        public int? InvoiceKey
        {
            get
            {
                return this._invoiceKey;
            }
            set
            {
                this._invoiceKey = value;
                NotifyPropertyChanged("InvoiceKey");
            }
        }

        public string WorkOrderNo
        {
            get
            {
                return this._workOrderNo;
            }
            set
            {
                this._workOrderNo = value;
                NotifyPropertyChanged("WorkOrderNo");
            }
        }

        public int? WorkOrderTypeKey
        {
            get
            {
                return this._workOrderTypeKey;
            }
            set
            {
                this._workOrderTypeKey = value;
                NotifyPropertyChanged("WorkOrderTypeKey");
            }
        }

        public int? Incharge1
        {
            get
            {
                return this._incharge1;
            }
            set
            {
                this._incharge1 = value;
                NotifyPropertyChanged("Incharge1");
            }
        }

        public int? Incharge2
        {
            get
            {
                return this._incharge2;
            }
            set
            {
                this._incharge2 = value;
                NotifyPropertyChanged("Incharge2");
            }
        }

        public int? VehicleKey
        {
            get
            {
                return this._vehicleKey;
            }
            set
            {
                this._vehicleKey = value;
                NotifyPropertyChanged("VehicleKey");
            }
        }

        public string Mileage
        {
            get
            {
                return this._mileage;
            }
            set
            {
                this._mileage = value;
                NotifyPropertyChanged("Mileage");
            }
        }

        public string Colour
        {
            get
            {
                return this._colour;
            }
            set
            {
                this._colour = value;
                NotifyPropertyChanged("Colour");
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

        public string ContactPerson
        {
            get
            {
                return this._contactPerson;
            }
            set
            {
                this._contactPerson = value;
                NotifyPropertyChanged("ContactPerson");
            }
        }

        public string MobilePhone
        {
            get
            {
                return this._mobilePhone;
            }
            set
            {
                this._mobilePhone = value;
                NotifyPropertyChanged("MobilePhone");
            }
        }

        public string Email
        {
            get
            {
                return this._email;
            }
            set
            {
                this._email = value;
                NotifyPropertyChanged("Email");
            }
        }

        public DateTime? DateIn
        {
            get
            {
                return this._dateIn;
            }
            set
            {
                this._dateIn = value;
                NotifyPropertyChanged("DateIn");
            }
        }

        public DateTime? DateOutReq
        {
            get
            {
                return this._dateOutReq;
            }
            set
            {
                this._dateOutReq = value;
                NotifyPropertyChanged("DateOutReq");
            }
        }

        public DateTime? DateOutAct
        {
            get
            {
                return this._dateOutAct;
            }
            set
            {
                this._dateOutAct = value;
                NotifyPropertyChanged("DateOutAct");
            }
        }

        public int? StatusKey
        {
            get
            {
                return this._statusKey;
            }
            set
            {
                this._statusKey = value;
                NotifyPropertyChanged("StatusKey");
            }
        }

        public string Remark
        {
            get
            {
                return this._remark;
            }
            set
            {
                this._remark = value;
                NotifyPropertyChanged("Remark");
            }
        }

        public string Brand
        {
            get
            {
                return this._brand;
            }
            set
            {
                this._brand = value;
                NotifyPropertyChanged("Brand");
            }
        }

        public string Model
        {
            get
            {
                return this._model;
            }
            set
            {
                this._model = value;
                NotifyPropertyChanged("Model");
            }
        }

        public int? DeptKey
        {
            get
            {
                return this._deptKey;
            }
            set
            {
                this._deptKey = value;
                NotifyPropertyChanged("DeptKey");
            }
        }
        public string RecommendedBy
        {
            get
            {
                return this._recommendedBy;
            }
            set
            {
                this._recommendedBy = value;
                NotifyPropertyChanged("RecommendedBy");
            }
        }
        public string AdditionalRemark
        {
            get
            {
                return this._additionalRemark;
            }
            set
            {
                this._additionalRemark = value;
                NotifyPropertyChanged("AdditionalRemark");
            }
        }       

        public bool? ShowEnquiryForm
        {
            get
            {
                return this._ShowEnquiryForm;
            }
            set
            {
                this._ShowEnquiryForm = value;
                NotifyPropertyChanged("ShowEnquiryForm");
            }
        }

        public SYSAttachments Attachments
        {
            get { return attachments; }
            set { attachments = value; }
        }

        #endregion

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _workOrderKey = null;
            public int? _vehicleKey = null;
            public string _workOrderNo = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? WorkOrderKey)
            {
                _workOrderKey = WorkOrderKey;
            }

            internal Criteria(int? WorkOrderKey, int? Option)
            {
                _workOrderKey = WorkOrderKey;
                _option = Option;
            }

            internal Criteria(int? WorkOrderKey, string KeyID)
            {
                _workOrderKey = WorkOrderKey;
                _workOrderNo = KeyID;
            }

            internal Criteria(string WorkOrderNo, int? Option)
            {
                _workOrderNo = WorkOrderNo;
                _option = Option;
            }

            internal Criteria(int? WorkOrderKey,int? VehicleKey, int? Option)
            {
                _workOrderKey = WorkOrderKey;
                _vehicleKey = VehicleKey;
                _option = Option;
            }
        }

        #endregion //Criteria

            internal static WorkOrder New()
            {
                WorkOrder child = new WorkOrder();   
                return child;
            }
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
			internal bool Fetch(SqlConnection cn,Criteria criteria)
			{
				bool retValue = false;
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "WO_Get";

                    cm.Parameters.AddWithValue("@Option", criteria._option);
                    cm.Parameters.AddWithValue("@WorkOrderKey", criteria._workOrderKey);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Using data reader as record set.
                    using ( IDataReader dr = cm.ExecuteReader())
                    {                       
                        //If data reader can read, continue...
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
                        retValue=false;
                }// Already close and dispose sql connection.
                
				return retValue;
			}
            internal static WorkOrder Get(IDataReader dr)
			{
                WorkOrder child = new WorkOrder();
				child.Fetch(dr);
				return child;
			}
            internal static WorkOrder Get(SqlConnection cn, Criteria criteria)
            {
                WorkOrder child = new WorkOrder();
                child.Fetch(cn,criteria);
                return child;
            }

			internal bool Fetch(IDataReader dataReader)
			{
                _workOrderKey = dataReader["WorkOrderKey"] == DBNull.Value ? null : (int?)dataReader["WorkOrderKey"];
                _invoiceKey = dataReader["InvoiceKey"] == DBNull.Value ? null : (int?)dataReader["InvoiceKey"];
                _workOrderNo = dataReader["WorkOrderNo"] == DBNull.Value ? string.Empty : dataReader["WorkOrderNo"].ToString();
                _workOrderTypeKey = dataReader["WorkOrderTypeKey"] == DBNull.Value ? null : (int?)dataReader["WorkOrderTypeKey"];
                _incharge1 = dataReader["Incharge1"] == DBNull.Value ? null : (int?)dataReader["Incharge1"];
                _incharge2 = dataReader["Incharge2"] == DBNull.Value ? null : (int?)dataReader["Incharge2"];
                _vehicleKey = dataReader["VehicleKey"] == DBNull.Value ? null : (int?)dataReader["VehicleKey"];
                _mileage = dataReader["Mileage"] == DBNull.Value ? string.Empty : dataReader["Mileage"].ToString();
                _colour = dataReader["Colour"] == DBNull.Value ? string.Empty : dataReader["Colour"].ToString();
                _conKey = dataReader["ConKey"] == DBNull.Value ? null : (int?)dataReader["ConKey"];
                _conNm = dataReader["ConNm"] == DBNull.Value ? string.Empty : dataReader["ConNm"].ToString();
                _contactPerson = dataReader["ContactPerson"] == DBNull.Value ? string.Empty : dataReader["ContactPerson"].ToString();
                _mobilePhone = dataReader["MobilePhone"] == DBNull.Value ? string.Empty : dataReader["MobilePhone"].ToString();
                _email = dataReader["Email"] == DBNull.Value ? string.Empty : dataReader["Email"].ToString();
                _dateIn = dataReader["DateIn"] == DBNull.Value ? null : (DateTime?)dataReader["DateIn"];
                _dateOutReq = dataReader["DateOutReq"] == DBNull.Value ? null : (DateTime?)dataReader["DateOutReq"];
                _dateOutAct = dataReader["DateOutAct"] == DBNull.Value ? null : (DateTime?)dataReader["DateOutAct"];
                _statusKey = dataReader["DataDes"] == DBNull.Value ? null : (int?)dataReader["MsgValue"];
                _remark = dataReader["Remark"] == DBNull.Value ? string.Empty : dataReader["Remark"].ToString();
                _brand = dataReader["Brand"] == DBNull.Value ? string.Empty : dataReader["Brand"].ToString();
                _model = dataReader["Model"] == DBNull.Value ? string.Empty : dataReader["Model"].ToString();
                _deptKey = dataReader["DeptKey"] == DBNull.Value ? null : (int?)dataReader["DeptKey"];
                _recommendedBy = dataReader["RecommendedBy"] == DBNull.Value ? string.Empty : dataReader["RecommendedBy"].ToString();
                _additionalRemark = dataReader["AdditionalRemark"] == DBNull.Value ? string.Empty : dataReader["AdditionalRemark"].ToString();
                _CreateDate = dataReader["CreateDate"] == DBNull.Value ? null : (DateTime?)dataReader["CreateDate"];
                _CreateUserKey = dataReader["CreateUserKey"] == DBNull.Value ? null : (int?)dataReader["CreateUserKey"];
                _LastModifiedDate = dataReader["LastModifiedDate"] == DBNull.Value ? null : (DateTime?)dataReader["LastModifiedDate"];
                _LastModifiedUserKey = dataReader["LastModifiedUserKey"] == DBNull.Value ? null : (int?)dataReader["LastModifiedUserKey"];
                return true;
			}
			#endregion //Data Access - Fetch

			#region Data Access - Insert

			internal bool Insert()
			{
				bool retValue = false;
				DocKey = null;
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create new sql connection for this method. 
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open sql connection. 
                        cn.Open();
                        retValue = this.Insert(cn);
                    }
                    // No errors - commit transaction
                      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                }// Already close and dispose sql connection.
                
                return retValue;
			}
			internal bool Insert(SqlConnection cn)
			{
				string msgID = "RecordAddFail";
				DocKey=0;
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "WO_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);

                    if (_workOrderKey == null)
                        cm.Parameters.AddWithValue("@WorkOrderKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@WorkOrderKey", _DocKey);

                    if (_invoiceKey == null)
                        cm.Parameters.AddWithValue("@InvoiceKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@InvoiceKey", _invoiceKey);

                    if (_workOrderNo == null)
                        cm.Parameters.AddWithValue("@WorkOrderNo", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@WorkOrderNo", _workOrderNo);

                    if (_workOrderTypeKey == null)
                        cm.Parameters.AddWithValue("@WorkOrderTypeKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@WorkOrderTypeKey", _workOrderTypeKey);

                    if (_incharge1 == null)
                        cm.Parameters.AddWithValue("@Incharge1", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Incharge1", _incharge1);

                    if (_incharge2 == null)
                        cm.Parameters.AddWithValue("@Incharge2", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Incharge2", _incharge2);

                    if (_vehicleKey == null)
                        cm.Parameters.AddWithValue("@VehicleKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@VehicleKey", _vehicleKey);

                    if (_mileage == null)
                        cm.Parameters.AddWithValue("@Mileage", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Mileage", _mileage);

                    if (_colour == null)
                        cm.Parameters.AddWithValue("@Colour", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Colour", _colour);

                    if (_conKey == null)
                        cm.Parameters.AddWithValue("@ConKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@ConKey", _conKey);

                    if (_contactPerson == null)
                        cm.Parameters.AddWithValue("@ContactPerson", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@ContactPerson", _contactPerson);

                    if (_mobilePhone == null)
                        cm.Parameters.AddWithValue("@MobilePhone", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@MobilePhone", _mobilePhone);

                    if (_email == null)
                        cm.Parameters.AddWithValue("@Email", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Email", _email);

                    if (_dateIn == null)
                        cm.Parameters.AddWithValue("@DateIn", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@DateIn", _dateIn);

                    if (_dateOutReq == null)
                        cm.Parameters.AddWithValue("@DateOutReq", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@DateOutReq", _dateOutReq);

                    if (_dateOutAct == null)
                        cm.Parameters.AddWithValue("@DateOutAct", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@DateOutAct", _dateOutAct);

                    if (_statusKey == null)
                        cm.Parameters.AddWithValue("@StatusKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@StatusKey", _statusKey);

                    if (_remark == null)
                        cm.Parameters.AddWithValue("@Remark", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Remark", _remark);

                    if (_deptKey == null)
                        cm.Parameters.AddWithValue("@DeptKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@DeptKey", _deptKey);

                    if (_recommendedBy == null)
                        cm.Parameters.AddWithValue("@RecommendedBy", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@RecommendedBy", _recommendedBy);

                    if (_additionalRemark == null)
                        cm.Parameters.AddWithValue("@AdditionalRemark", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@AdditionalRemark", _additionalRemark);

                    //if (_CreateDate == null)
                    //    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                    //else
                    //    cm.Parameters.AddWithValue("@CreateDate", _CreateDate);
                    if (AppInfor.currentUserKey == null)
                        cm.Parameters.AddWithValue("@UserKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@UserKey", AppInfor.currentUserKey);
                    //if (_LastModifiedDate == null)
                    //    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    //else
                    //    cm.Parameters.AddWithValue("@LastModifiedDate", _LastModifiedDate);
                    //cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                    
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    // Execute command.
                    cm.ExecuteNonQuery();

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
                    cm.CommandText = "WO_AddUpdate";

					cm.Parameters.AddWithValue("@Option", 1);
					cm.Parameters.AddWithValue("@MsgID", msgID);
					
					if (_workOrderKey == null)
                        cm.Parameters.AddWithValue("@WorkOrderKey", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@WorkOrderKey", _workOrderKey);

					if (_invoiceKey == null)
                        cm.Parameters.AddWithValue("@InvoiceKey", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@InvoiceKey", _invoiceKey);
					if (_workOrderNo == null)
                        cm.Parameters.AddWithValue("@WorkOrderNo", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@WorkOrderNo", _workOrderNo);
					if (_workOrderTypeKey == null)
                        cm.Parameters.AddWithValue("@WorkOrderTypeKey", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@WorkOrderTypeKey", _workOrderTypeKey);
					if (_incharge1 == null)
                        cm.Parameters.AddWithValue("@Incharge1", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@Incharge1", _incharge1);
					if (_incharge2 == null)
                        cm.Parameters.AddWithValue("@Incharge2", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@Incharge2", _incharge2);
					if (_vehicleKey == null)
                        cm.Parameters.AddWithValue("@VehicleKey", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@VehicleKey", _vehicleKey);
					if (_mileage == null)
                        cm.Parameters.AddWithValue("@Mileage", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@Mileage", _mileage);
					if (_colour == null)
                        cm.Parameters.AddWithValue("@Colour", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@Colour", _colour);
					if (_conKey == null)
                        cm.Parameters.AddWithValue("@ConKey", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@ConKey", _conKey);
					if (_contactPerson == null)
                        cm.Parameters.AddWithValue("@ContactPerson", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@ContactPerson", _contactPerson);
					if (_mobilePhone == null)
                        cm.Parameters.AddWithValue("@MobilePhone", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@MobilePhone", _mobilePhone);
					if (_email == null)
                        cm.Parameters.AddWithValue("@Email", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@Email", _email);
					if (_dateIn == null)
                        cm.Parameters.AddWithValue("@DateIn", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@DateIn", _dateIn);
					if (_dateOutReq == null)
                        cm.Parameters.AddWithValue("@DateOutReq", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@DateOutReq", _dateOutReq);
					if (_dateOutAct == null)
                        cm.Parameters.AddWithValue("@DateOutAct", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@DateOutAct", _dateOutAct);
					if (_statusKey == null)
                        cm.Parameters.AddWithValue("@StatusKey", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@StatusKey", _statusKey);
					if (_remark == null)
                        cm.Parameters.AddWithValue("@Remark", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@Remark", _remark);
					if (_deptKey == null)
                        cm.Parameters.AddWithValue("@DeptKey", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@DeptKey", _deptKey);
					if (_recommendedBy == null)
                        cm.Parameters.AddWithValue("@RecommendedBy", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@RecommendedBy", _recommendedBy);
					if (_additionalRemark == null)
                        cm.Parameters.AddWithValue("@AdditionalRemark", DBNull.Value);
					else
                        cm.Parameters.AddWithValue("@AdditionalRemark", _additionalRemark);
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
					
					cm.Parameters.AddWithValue("@RetValue", 0);
					cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
					//cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;
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

            internal bool UpdateStatus(int intWorkOrderKey, string strStatus)
            {
                bool retValue = false;
                using (TransactionScope scope = new TransactionScope())
                {
                    //Create new sql connection for this method. 
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open sql connection. 
                        cn.Open();
                        retValue = this.UpdateStatus(cn, intWorkOrderKey, strStatus);
                    }
                    // No errors - commit transaction
                    if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                }// Already close and dispose sql connection.

                return retValue;
            }
            internal bool UpdateStatus(SqlConnection cn, int intWorkOrderKey, string strStatus)
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "WO_UpdateStatus";

                    cm.Parameters.AddWithValue("@Option", 0);

                    if (intWorkOrderKey == null)
                        cm.Parameters.AddWithValue("@WorkOrderKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@WorkOrderKey", intWorkOrderKey);

                    if (strStatus == null)
                        cm.Parameters.AddWithValue("@Status", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Status", strStatus);

                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    //cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();

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
					cm.CommandText = "WO_Delete";

					cm.Parameters.AddWithValue("@MsgID", msgID);
                    cm.Parameters.AddWithValue("@WorkOrderKey", criteria._workOrderKey);
					
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
			#region Data Access - Validation

			internal bool Validation(Criteria criteria,bool isNew)
			{
				bool retValue = false;
                try
                {
                    //using (TransactionScope scope = new TransactionScope())
                    //{
                        //Create new sql connection for this method. 
                        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                        {
                            // Open sql connection. 
                            cn.Open();
                            retValue = Validation(cn, criteria, isNew);
                        }
                    //    // No errors - commit transaction
                    //      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                    //}// Already close and dispose sql connection.
                }
                catch (TAException taex)
                {
                    throw taex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
				
				return retValue;
			}
			internal bool Validation(SqlConnection cn, Criteria criteria,bool isNew)
			{
				
				string msgID = "DocID"+ MsgID.Validation.DuplicateRecord;
                try
                {
                    using (SqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "WO_Validation";

                        cm.Parameters.AddWithValue("@isNew", isNew);
                        cm.Parameters.AddWithValue("@WorkOrderKey", criteria._workOrderKey);
                        cm.Parameters.AddWithValue("@WorkOrderNo", criteria._workOrderNo);
                        cm.Parameters.AddWithValue("@RetValue", 0);

                        cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                        // Execute command.
                        cm.ExecuteNonQuery();

                        if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                            return true;
                        else
                            return false;
                    }// Already close and dispose sql command.
                }
                catch (TAException tex)
                {
                    throw tex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
			}
			#endregion Validation

            #region Data Access - Search
            internal DataTable Search(int option)
            {
                DataTable dt = new DataTable();

                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    dt = this.Search(cn, option);
                }


                return dt;
            }

            internal DataTable Search(SqlConnection cn, int option)
            {
                DataTable dt = new DataTable();
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "WO_Search";

                    cm.Parameters.AddWithValue("@Option", option);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    SqlDataReader dr = cm.ExecuteReader();
                    dt.Load(dr);
                    
                }// Already close and dispose sql connection.

                return dt;
            }
        #endregion Search

            #region Data Access - GetCurrentUserDept
            //internal int? GetCurrentUserDept()
            //{
            //    int? deptKey = 0;

            //    // Create new sql connection for this method. 
            //    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            //    {
            //        // Open sql connection. 
            //        cn.Open();
            //        deptKey = this.GetCurrentUserDept(cn);
            //    }


            //    return deptKey;
            //}
            internal int? GetCurrentUserDept(SqlConnection cn)
            {
                int? deptKey = 0;
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "WO_GetCurrentUserDept";

                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@UserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Using data reader as record set.
                    using (IDataReader dr = cm.ExecuteReader())
                    {
                        //If data reader can read, continue...
                        if (dr.Read())
                        {
                            deptKey = dr["DeptKey"] == DBNull.Value ? null : (int?)dr["DeptKey"];
                        }
                        else
                            this.Clear();


                    }// Already close and dispose data reader.                   

                }// Already close and dispose sql connection.

                return deptKey;
            }
            #endregion

            #region Data Access - Get History Count and Movement Count
            //internal int? GetHistoryCount(int intWorkOrderKey, int intVehicle)
            //{
            //    int? count = 0;

            //    // Create new sql connection for this method. 
            //    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            //    {
            //        // Open sql connection. 
            //        cn.Open();
            //        count = this.GetHistoryCount(cn, intWorkOrderKey,intVehicle);
            //    }


            //    return count;
            //}

            internal int? GetCount(SqlConnection cn,Criteria criteria)
            {
                int? count = 0;
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "WO_GetCount";

                    cm.Parameters.AddWithValue("@Option", criteria._option);
                    cm.Parameters.AddWithValue("@WorkOrderkey", criteria._workOrderKey);
                    cm.Parameters.AddWithValue("@VehicleKey", criteria._vehicleKey);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Using data reader as record set.
                    using (IDataReader dr = cm.ExecuteReader())
                    {
                        //If data reader can read, continue...
                        if (dr.Read())
                        {
                            count = dr["Count"] == DBNull.Value ? null : (int?)dr["Count"];
                        }
                        else
                            this.Clear();


                    }// Already close and dispose data reader.                   

                }// Already close and dispose sql connection.

                return count;
            }
            
            #endregion

            #region Data Access - Get Work Order Key
            internal int? GetWorkOrderKey(string strWorkOrderNo)
            {
                int? WOKey ;

                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    WOKey = this.GetWorkOrderKey(cn, strWorkOrderNo);
                }


                return WOKey;
            }

            internal int? GetWorkOrderKey(SqlConnection cn, string strWorkOrderNo)
            {
                int? WOKey = 0;
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "WO_GetWorkOrderKey";

                    cm.Parameters.AddWithValue("@WorkOrderNo", strWorkOrderNo);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Using data reader as record set.
                    using (IDataReader dr = cm.ExecuteReader())
                    {
                        //If data reader can read, continue...
                        if (dr.Read())
                        {
                            WOKey = dr["WorkOrderKey"] == DBNull.Value ? null : (int?)dr["WorkOrderKey"];
                        }
                        else
                            this.Clear();


                    }// Already close and dispose data reader.                   

                }// Already close and dispose sql connection.

                return WOKey;

                
            }
            #endregion

            #region Fill Combo
            internal DataTable GetVehicleAndCustomerInfo(int option, int vehicleKey, string vehicle)
            {
                DataTable dt = new DataTable();

                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    dt = this.GetVehicleAndCustomerInfo(cn, option, vehicleKey, vehicle);
                }


                return dt;
            }

            internal DataTable GetVehicleAndCustomerInfo(SqlConnection cn, int option, int vehicleKey, string vehicle)
            {
                DataTable dt = new DataTable();
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "WO_GetVehicleAndCustomerInfo";

                    cm.Parameters.AddWithValue("@Option", option);
                    cm.Parameters.AddWithValue("@VehicleKey", vehicleKey);
                    cm.Parameters.AddWithValue("@Vehicle", vehicle);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    SqlDataReader dr = cm.ExecuteReader();
                    dt.Load(dr);

                }// Already close and dispose sql connection.

                return dt;
            }
        #endregion
    }
}
