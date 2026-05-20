using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using System.ComponentModel;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class MSTTimesheetFactory : CommandBase
    {
        #region Member variables and constants

        private MSTTimesheet _MSTTimesheet = null;
        private MSTJobDetOther _MSTJobDetOther = null;
        private MSTJobDetOthers _MSTJobDetOthers = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isOpenReadOnly = false;
        private int _guID = 0;

        private SECUserPermissionVw _userTimesheetPermission;

        // System Code Key for this Factory.
        private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Job_Timesheet;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        // Permission ID for this Factory.
        public const string constPermID = GVar.PermissionID.Job_TimeSheet;

        // Error Event Declaration
        public GVar.ErrorEvent errorEvent = null;

        // Dirty Event Declaration
        public GVar.DirtyEvent dirtyEvent = null;

        // ReadOnly Event Declaration
        public GVar.ReadOnlyEvent readonlyEvent = null;

        //Custom Event Declaration
        public GVar.UINotifierEvent MSTTSheetNotifier = null;
        public GVar.UINotifierEvent clearErrorNotifier = null;

        #endregion

        #region Factory Properties

        public MSTTimesheet ObjMSTTimesheet
        {
            get
            {
                return this._MSTTimesheet;
            }
        }
        public MSTJobDetOther ObjMSTJobDetOther
        {
            get
            {
                return this._MSTJobDetOther;
            }
        }

        public MSTJobDetOthers ObjMSTJobDetOthers
        {
            get
            {
                return this._MSTJobDetOthers;
            }
        }

        public SECUserPermissionVw UserTimesheetPermission
        {
            get
            {
                return this._userTimesheetPermission;
            }
        }

        internal GEnum.InstanceMode InstanceMode
        {
            get
            {
                return this._instanceMode;
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

        public bool IsValid
        {
            get
            {
                return this._isValid;
            }
        }

        public bool IsNew
        {
            get
            {
                return this._isNew;
            }
        }

        public bool IsOpenReadOnly
        {
            get
            {
                return this._isOpenReadOnly;
            }
        }

        public int GUID
        {
            get
            {
                return this._guID;
            }
        }


        public string ErrorMessageID
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for this Factory.
        /// </summary>
        public MSTTimesheetFactory(GEnum.InstanceMode instanceMode)
        {
            try
            {
                this._instanceMode = instanceMode;
                this.Initialisation(instanceMode);
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            
        }

        #endregion // Constructors

        #region Initialisation Method

        public bool Initialisation(GEnum.InstanceMode instanceMode)
        {
            bool isInitialisation = false;
            string msgID = MsgID.Common.InitialisationFail;
            try
            {
                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                {
                    // Check Permission
                    if (!SECPermUtility.Any(constPermID, out this._isOpenReadOnly, true))
                        return false;

                    _userTimesheetPermission = SECUserPermissionVw.Get(AppInfor.securityKey, constPermID);
                    if (!_userTimesheetPermission.Read(ref msgID))
                        return false;

                    // Create TransactionScope
                    using (TransactionScope scope = new TransactionScope())
                    {
                        // Create SqlConnection
                        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                        {
                            // Open Connection
                            cn.Open();

                            // Get Instance GUID
                            if ((this._guID = SysOptionUtility.GetNewLockingGUID(cn)) == 0)
                            {
                                this._guID = -1;
                                return false;
                            }

                            // Locking
                            if (SysLockUtility.IsProcessLock(cn, true, GEnum.SysLockOption.ByCodKey, constCodeKey, this._guID))
                            {
                                this._guID = -1;
                                return false;
                            }

                            // Add Inprogress Lock
                            if (!SysLockUtility.AddInprogressLock(cn, true, this._guID, constCodeKey))
                            {
                                this._guID = -1;
                                return false;
                            }

                            // Commit Process                          
                            this._isNew = false;                        
                            this._isOpenReadOnly = false;
                            msgID = string.Empty;
                            isInitialisation = true;

                            // No errors - commit transaction
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        }
                    }
                }
                else
                {
                    this._guID = 0;                
                    this._isOpenReadOnly = false;
                    msgID = string.Empty;
                    isInitialisation = true;
                }
                return isInitialisation;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            
        }

        #endregion //Initialisation Method

        #region GetEdit Method

        public bool GetEdit(int? EmKey, DateTime? Period)
        {
            // Initialisation
            bool isGetEdit = false;
            string msgID = MsgID.Common.GetFail;

            // Copy original object
            BOLib.MSTTimesheet copyMSTTimesheet = this._MSTTimesheet.Clone();
            BOLib.MSTJobDetOthers copyMSTJobDetOthers = GFunc.TACopyDataTable(_MSTJobDetOthers);

            
            try
            {
                // Check Permission
                if (!_userTimesheetPermission.Edit(ref msgID))
                    return false;

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        // Get Record                                 

                        if (!this._MSTTimesheet.Fetch(cn, new MSTTimesheet.Criteria(EmKey, 1)))
                            throw new TAException(msgID);

                        //Record Not Found
                        if (GFunc.NEInt(this._MSTTimesheet._emKey, 0) == 0)
                        {                                
                            throw new TAException(MsgID.Common.GetFail);
                        }

                        this._MSTTimesheet._period = Period;


                        if (GFunc.IsNE(this._MSTTimesheet._itmKey) || this._MSTTimesheet._itmKey == 0)
                        {
                            msgID = "ItmKey" + MsgID.Validation.DataKeyInvalid;
                            {
                                //MsgBox.Show(msgID);   
                                
                                return false;                                    
                            }

                        }

                        if (GFunc.IsNE(this._MSTTimesheet._overHeadKey) || this._MSTTimesheet._overHeadKey == 0)
                        {
                            msgID = "CostGrpKey" + MsgID.Validation.DataKeyInvalid;
                            {
                                //MsgBox.Show(msgID); 
                                return false;
                            }
                        }


                        _MSTJobDetOthers.Clear();
                        if (!_MSTJobDetOthers.Fetch(cn, new MSTJobDetOthers.Criteria(EmKey, Period, 3)))
                            throw new TAException(msgID);


                        if (_MSTJobDetOthers.Rows.Count > 0)
                        {
                            // Check Lock
                            //if (processOK)
                            //    processOK = !SysLockUtility.RemoveLock(cn, out msgID,1,constCodeKey,this._guID,EmKey,null);

                            // Remove Lock
                            if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                                return false;

                            // Add Lock
                            if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, EmKey))
                                return false;

                        }

                        // Commit Process                               
                        this._MSTTimesheet.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ObjMSTTimesheet_PropertyChanged);
                        this._MSTJobDetOthers.ColumnChanged += new DataColumnChangeEventHandler(Details_CollectionChanged);

                        this._isDirty = false;
                        this._isNew = false;
                        this._isOpenReadOnly = false;
                        msgID = string.Empty;
                        isGetEdit = true;

                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                    }
                }

            }
            catch (TAException tex)
            {
                // Restore data when error occurs                    
                this._MSTTimesheet = copyMSTTimesheet;
                this._MSTJobDetOthers = copyMSTJobDetOthers;
                throw Error(tex);
            }
            catch (Exception ex)
            {
                // Restore data when error occurs                    
                this._MSTTimesheet = copyMSTTimesheet;
                this._MSTJobDetOthers = copyMSTJobDetOthers;
                throw Error(ex);
            }
            finally
            {
                // Set Null to Backup Objects
                copyMSTTimesheet = null;
                copyMSTJobDetOthers = null;
            }
            
            return isGetEdit;
        }

        #endregion //GetEdit Method

        void Details_CollectionChanged(object sender, DataColumnChangeEventArgs e)
        {
            _isDirty = true;
        }

        #region GetReadOnly Method

        public bool GetReadOnly(int? EmKey)
        {
            bool isGetReadOnly = false;
            string msgID = MsgID.Common.GetFail;

            // Copy original object
            BOLib.MSTTimesheet copyMSTTimesheet = this._MSTTimesheet.Clone();

            try
            {
                // Check Permission
                if (!_userTimesheetPermission.Read(ref msgID))
                    return false;

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        // Remove all locks by GUID except inprogress Locking
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                            return false;

                        // Call MSTSalesRep.Fetch
                        if (!this._MSTTimesheet.Fetch(cn, new MSTTimesheet.Criteria(EmKey, 0)))
                        {
                            MsgBox.Show(cn,msgID); return false;
                        }

                        //// Call MSTSalesRepPayrolls.Fetch
                        //if (_userPayrollPermission.CanRead)
                        //    processOK = this._MSTSalesRepPayrolls.Fetch(cn, new MSTSalesRepPayRolls.Criteria(EmKey, 1), out msgID);


                        this._isDirty = false;
                        this._isNew = false;
                        this._isOpenReadOnly = true;
                        msgID = string.Empty;
                        isGetReadOnly = true;

                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                    }
                }
            }
            catch (TAException tex)
            {
                // Restore data when error occurs
                this._MSTTimesheet = copyMSTTimesheet;
                throw Error(tex);
            }
            catch (Exception ex)
            {
                // Restore data when error occurs
                this._MSTTimesheet = copyMSTTimesheet;
                throw Error(ex);
            }
            finally
            {
                // Set Null to Backup Objects
                copyMSTTimesheet = null;
            }
            
            return isGetReadOnly;
        }

        #endregion //GetReadOnly Method

        #region New Method

        public bool New()
        {
            bool isNew = false;
            string msgID = MsgID.Common.NewFail;

            BOLib.MSTTimesheet copyMSTTimesheet = null;
            BOLib.MSTJobDetOthers copyMSTJobDetOthers = null;

            // Copy original object
            if (!GFunc.IsNE(this._MSTTimesheet))
                copyMSTTimesheet = this._MSTTimesheet.Clone();

            // Copy original object
            if (!GFunc.IsNE(this._MSTJobDetOthers))
                copyMSTJobDetOthers = GFunc.TACopyDataTable(_MSTJobDetOthers);
                        
            try
            {
                //// Check Security Permission 
                ////processOK = SECPermUtility.Any(constPermID, out this._isOpenReadOnly, true);
                //processOK = _userTimesheetPermission.Edit(ref msgID);

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        // Remove all locks by GUID except inprogress Locking
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                            return false;

                        // Call New for Header                           
                        this._MSTTimesheet = MSTTimesheet.New();

                        // Call New for Detail                           
                        this._MSTJobDetOthers = MSTJobDetOthers.New(cn);


                        this._MSTTimesheet.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ObjMSTTimesheet_PropertyChanged);
                        this._MSTJobDetOthers.ColumnChanged += new DataColumnChangeEventHandler(Details_CollectionChanged);

                        this._isDirty = false;
                        this._isNew = true;
                        this._isOpenReadOnly = false;
                        msgID = string.Empty;
                        isNew = true;

                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                    }
                }
            }
            catch (TAException tex)
            {
                // Restore data when error is occur                    
                this._MSTTimesheet = copyMSTTimesheet;
                this._MSTJobDetOthers = copyMSTJobDetOthers;

                throw Error(tex);
            }
            catch (Exception ex)
            {
                // Restore data when error is occur                    
                this._MSTTimesheet = copyMSTTimesheet;
                this._MSTJobDetOthers = copyMSTJobDetOthers;

                throw Error(ex);
            }
            finally
            {
                // Set Null to Backup Objects
                copyMSTTimesheet = null;
                copyMSTJobDetOthers = null;
            }
            
            return isNew;
        }

        #endregion //New Method

        #region Save Method

        public bool Save()
        {
            bool isSave = false;
            string msgID = String.Empty;
            if (this.IsNew)
                ErrorMessageID = MsgID.Common.AddFail;
            else
                ErrorMessageID = MsgID.Common.UpdateFail;

            bool isNewRecord = this.IsNew;
            int? newEmKey = 0;
            string autoID = string.Empty;

            bool isCommitTransFail = true;
            int? recordKey = null;

            
            try
            {
                if (this.IsOpenReadOnly)
                {
                    MsgBox.Show(MsgID.Common.RecordIsReadOnly);
                    return false;
                }
                else
                {
                    if (isNewRecord)
                    {
                        if (!_userTimesheetPermission.Add(ref msgID))
                            return false;
                    }
                    else
                    {
                        if (!_userTimesheetPermission.Edit(ref msgID))
                            return false;
                    }
                }

                // Create TransactionScope                        
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        recordKey = _MSTTimesheet._emKey;

                        #region Set Server DateTime If Create and Modified Date is null
                        //Get Server Date and Time (sdt)
                        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                        //Set Header Obj
                        _MSTTimesheet._createDate = GFunc.NEDateTime(_MSTTimesheet.CreateDate, svrDateTime);
                        _MSTTimesheet._createUserKey = GFunc.NEInt(_MSTTimesheet.CreateUserKey, AppInfor.currentUserKey);
                        _MSTTimesheet._lastModifiedDate = svrDateTime;
                        _MSTTimesheet._lastModifiedUserKey = AppInfor.currentUserKey;
                        #endregion

                        // Validation
                        if (!Validation(cn))
                            return false;

                        // Save Record

                        if (_userTimesheetPermission.CanEdit)
                        {
                            if (!_MSTJobDetOthers.DeleteFromTimeSheet(cn, new MSTJobDetOthers.Criteria(_MSTTimesheet._emKey, _MSTTimesheet._period, 1)))
                            {
                                MsgBox.Show(cn,msgID); return false;
                            }

                            if (!_MSTJobDetOthers.InsertFromTimeSheet(cn, new MSTJobDetOthers.Criteria(_MSTTimesheet._emKey, _MSTTimesheet._supervisorKey, _MSTTimesheet._overHeadKey, 1)))
                            {
                                MsgBox.Show(cn,msgID); return false;
                            }
                        }

                        // Record Locking
                        if (isNewRecord)
                            if (!SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newEmKey))
                                return false;

                        // Commit Process                               
                        if (isNewRecord)
                            _MSTTimesheet._emKey = newEmKey;

                        this._isDirty = false;
                        this._isNew = false;
                        msgID = string.Empty;
                        isSave = true;

                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        isCommitTransFail = false;


                    }// End of SqlConnection
                }// End of TransactionScope                        

                // Audit Log                        
                if (isNewRecord)
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _MSTTimesheet.ItmKey, _MSTTimesheet.Period.ToString(), GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTTimesheet });
                else
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _MSTTimesheet.ItmKey, _MSTTimesheet.Period.ToString(), GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTTimesheet });
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                if (isNewRecord)
                {
                    //Restore the auto generated ID
                    _MSTTimesheet._emKey = recordKey;
                }
                if (isCommitTransFail)
                   msgID=MsgID.Validation.CommitTransFail;
                throw Error(ex);
            }
            
            return isSave;
        }

        #endregion //Save Method

        #region Validation Method

        public bool Validation(SqlConnection cn)
        {
            bool isValidation = true;
            string processOK = GVar.gcPass;
            string errorMsgID = string.Empty;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
            try
            {
                //Clear Error is UL
                if (!GFunc.IsNE(this.clearErrorNotifier))
                    this.clearErrorNotifier.Invoke(this, e);

                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTTimesheet._emKey, "EmKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTTimesheet._supervisorKey, "SupervisorKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);

                if (e.PropertyMessage.Count > 0)
                {
                    isValidation = false;
                    ErrorMessageID = MsgID.Common.ValidationFail;
                    if (!GFunc.IsNE(this.MSTTSheetNotifier))
                        this.MSTTSheetNotifier.Invoke(this, e);
                    return false;
                }
                else _isValid = true;

                //Atleast one record 
                if (_MSTJobDetOthers.Rows.Count < 1 || GFunc.IsNE(_MSTJobDetOthers.Rows[0]["jobCostTypeKey"]))
                {
                    ErrorMessageID = "PhaseTaskCostTypeEmpty";
                    e.PropertyMessage.Add("", SysMessageUtility.Get(cn, ErrorMessageID));

                    return false;
                }
                else
                    this._isValid = true;

                if (!ValidationForDetail(cn))
                {
                    _isValid = false;
                    ErrorMessageID = MsgID.Common.RecordDetailValidationFail;
                    if (!GFunc.IsNE(this.MSTTSheetNotifier))
                        this.MSTTSheetNotifier.Invoke(this, e);
                    return false;
                }
                if (this._MSTJobDetOthers.HasErrors)
                {
                    _isValid = false;
                    ErrorMessageID = MsgID.Common.RecordDetailValidationFail;
                    if (!GFunc.IsNE(this.MSTTSheetNotifier))
                        this.MSTTSheetNotifier.Invoke(this, e);
                    return false;
                }
            }
            catch (TAException tex)
            {
                Error(tex);
            }
            catch (Exception ex)
            {
                Error(ex);
            }
            return isValidation;
        }

        #endregion //Validation Method

        #region Dispose Method

        public bool Dispose()
        {
            bool isDispose = false;
            string msgID = string.Empty;
            try
            {
                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                    if (!SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0))
                        return false;

                isDispose = true;
                return isDispose;
            }
            catch (TAException tex)
            {
                Error(tex);
                return false;
            }
            catch (Exception ex)
            {
                Error(ex);
                return false;   
            }
            
        }

        #endregion //Dispose Method

        #region PropertyChanged

        private void ObjMSTTimesheet_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            string msgID = string.Empty;
            string msgValue = string.Empty;
            bool validateOk = true;

            if (!this._isOpenReadOnly)
            {
                //IsDirty
                if (this.dirtyEvent != null)
                    this.dirtyEvent.Invoke(this, e);

                this._isDirty = true;

                //UI Validation
                switch (e.PropertyName)
                {
                    case "SuperVisorKey":
                        validateOk = BaseUtility.Validation(out msgID, _MSTTimesheet._supervisorKey, e.PropertyName, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null);
                        break;
                    case "Month":
                        validateOk = BaseUtility.Validation(out msgID, _MSTTimesheet._period, e.PropertyName, GEnum.DataType.DateTime, GEnum.Require.Yes, null, null, null, null, null);
                        break;
                    case "EmKey":
                        validateOk = BaseUtility.Validation(out msgID, _MSTTimesheet._emKey, e.PropertyName, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null);
                        break;
                    case "ItmKey":
                        validateOk = BaseUtility.Validation(out msgID, _MSTTimesheet._itmKey, e.PropertyName, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null);
                        break;
                    case "OverheadKey":
                        validateOk = BaseUtility.Validation(out msgID, _MSTTimesheet._overHeadKey, e.PropertyName, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null);
                        break;
                }
            }

            if (!validateOk)
            {
                if (errorEvent != null)
                {
                    errorEvent.Invoke(SysMessageUtility.Get(msgID), e);
                    return;
                }
            }
        }

        #endregion

        #region ValidationForDetail Method
        private bool ValidationForDetail(SqlConnection cn)
        {
            //Variable Declaration
            string msgID = string.Empty;
            string msgValue = string.Empty;
            bool processOk = true;
            try
            {
                #region "MSTJobDetOther"

                for (int i = 0; i < _MSTJobDetOthers.Rows.Count; i++)
                {
                    if (processOk)
                        processOk = BaseUtility.Validation(out msgID, this._MSTJobDetOthers.Rows[i]["JobKey"], "JobKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null);


                    if (processOk)
                        processOk = BaseUtility.Validation(out msgID, this._MSTJobDetOthers.Rows[i]["JobPhaseKey"], "JobPhaseKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null);

                    if (processOk)
                        processOk = BaseUtility.Validation(out msgID, this._MSTJobDetOthers.Rows[i]["JobTaskKey"], "JobTaskKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null);

                    if (processOk)
                        processOk = BaseUtility.Validation(out msgID, this._MSTJobDetOthers.Rows[i]["JobCostTypeKey"], "JobCostTypeKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null);

                    if (processOk)
                        processOk = BaseUtility.Validation(out msgID, this._MSTJobDetOthers.Rows[i]["OthQty"], "OthQty", GEnum.DataType.Decimel, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null);


                    if (processOk)
                        processOk = BaseUtility.Validation(out msgID, this._MSTJobDetOthers.Rows[i]["DocDate"], "DocDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null);

                    if (processOk)
                        processOk = BaseUtility.Validation(out msgID, this._MSTJobDetOthers.Rows[i]["DocDes"], "DocDes", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);

                    if (!processOk)
                    {
                        this._MSTJobDetOthers.Rows[i].RowError = SysMessageUtility.Get(cn, msgID);
                    }
                    else
                        this._MSTJobDetOthers.Rows[i].RowError = string.Empty;

                    if (!processOk)
                    {
                        break;
                    }
                    else
                        this._MSTJobDetOthers.Rows[i].RowError = string.Empty;
                }
                if (!processOk)
                {
                    return processOk;
                }
                #endregion
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }

            return processOk;
        }
        #endregion

        #region Error

        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _MSTTimesheet, _MSTJobDetOther }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
        private TAException Error(TAException ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _MSTTimesheet, _MSTJobDetOther }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        #endregion
    }
}
