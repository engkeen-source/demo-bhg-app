using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using Infragistics.Win.UltraWinGrid;
using TAUtil;
using System.Globalization;

namespace BOLib
{
    [Serializable()]
    public class TASAlertFactory : CommandBase
    {
        #region Member variables and constants

        private TASAlert _TASAlert = null;
        private TASAlerts _TASAlerts = null;
        private TASAlertDetSub _TASAlertDetSub = null;
        private TASAlertDetSubs _TASAlertDetSubs = null;

        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isOpenReadOnly = false;
        private int _guID = 0;

        // System Code Key for this Factory.
        private GEnum.SystemCode constCodeKey = GEnum.SystemCode.Alerts;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        // Permission ID for this Factory.
        public string PermID = GVar.PermissionID.Alerts;

        // Custom Event Declaration 
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;        
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;

        #endregion // Member variables and constant

        #region Factory Properties

        public TASAlert ObjTASAlert
        {
            get
            {
                return this._TASAlert;
            }
            set
            {
                this._TASAlert = value;
            }
        }

        public TASAlerts ObjTASAlerts
        {
            get
            {
                return this._TASAlerts;
            }
        }

        public TASAlertDetSub ObjTASAlertDetSub
        {
            get
            {
                return this._TASAlertDetSub;
            }
            set
            {
                this._TASAlertDetSub = value;
            }
        }

        public TASAlertDetSubs ObjTASAlertDetSubs
        {
            get
            {
                return this._TASAlertDetSubs;
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

        #endregion // Constructors

      
        public TASAlertFactory(GEnum.InstanceMode instanceMode)
        {
            this._instanceMode = instanceMode;
            try
            {
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
        public TASAlertFactory()
        {
        }
        public bool Initialisation(GEnum.InstanceMode instanceMode)
        {
            try
            {
                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                {
                    if (!SECPermUtility.Any(PermID, out this._isOpenReadOnly, true))
                        return false;

                    using (TransactionScope scope = new TransactionScope())
                    {
                        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                        {
                            cn.Open();

                            //Get Instance GUID
                            //FORM will check for GUID > 0 to indicate Factory is valid
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

                            //Commit Process 
                            this._TASAlert = new TASAlert();
                            this._TASAlertDetSubs = new TASAlertDetSubs(cn);

                            this._isNew = false;
                            this._isOpenReadOnly = false;
                            if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                        }
                    }
                }
                else if (this.InstanceMode == GEnum.InstanceMode.InternalCall)
                {
                    if (!SECPermUtility.Any(PermID, out this._isOpenReadOnly, true))
                        return false;

                    using (TransactionScope scope = new TransactionScope())
                    {
                        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                        {
                            cn.Open();

                            //Get Instance GUID
                            //FORM will check for GUID > 0 to indicate Factory is valid
                            if ((this._guID = SysOptionUtility.GetNewLockingGUID(cn)) == 0)
                            {
                                this._guID = -1;
                                return false;
                            }

                            //Locking
                            if (SysLockUtility.CheckInProgressLock(cn, true, constCodeKey))
                            {
                                this._guID = -1;
                                return true;
                            }

                            //Commit Process 
                            this._TASAlert = new TASAlert();
                            this._TASAlertDetSubs = new TASAlertDetSubs(cn);

                            this._isNew = false;
                            this._isOpenReadOnly = false;
                            if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                        }
                    }
                }
                else
                {
                    //Use for situation where no locking and GUID is required but the factory is needed for some internal call
                    //for future use only
                    this._guID = 0;
                    this._isOpenReadOnly = false;
                }
                return true;
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

        public bool New()
        {
            #region Declaration
            bool restoreFlag = false;
            TASAlert copyTASAlert = null;
            TASAlertDetSubs copyTASAlertDetSubs = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._TASAlert != null)
                    copyTASAlert = this._TASAlert.Clone();

                if (this._TASAlertDetSubs != null)
                    copyTASAlertDetSubs = this._TASAlertDetSubs.Copy();

                #endregion

                #region Check Security Permission
                if (SECPermUtility.Any(PermID, out this._isOpenReadOnly, true) == false)
                    return false;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        #region Prepare New Instance
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        //Remove all locks by GUID except inprogress Locking
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                            return false;

                        //prepare new instance 
                        this._TASAlert = TASAlert.New();
                        this._TASAlertDetSubs = new TASAlertDetSubs(cn);
                        
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                        #endregion

                        HeaderDefaultValueSet();
                        //Set Flags
                        this._isDirty = false;
                        this._isNew = true;
                        this._isOpenReadOnly = false;

                        //Attach Events
                        this._TASAlert.PropertyChanged += new PropertyChangedEventHandler(Obj_PropertyChanged);
                    }
                }
                restoreFlag = false;
                return true;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._TASAlert = copyTASAlert;
                    this._TASAlertDetSubs = copyTASAlertDetSubs;
                }
                #endregion

                #region Dispose Backup Objects
                copyTASAlert = null;
                copyTASAlertDetSubs = null;
                #endregion
            }
        }
        public bool Save()
        {
            //For Master and Reference the RecordKey is obtain after saving. 
            //But we do not need to update this Record to the Record Detail as
            //the saving process will delete all details in the server and append the detail from local to server again
            //Note: this will only work if the detail in the server is never updated by other user.
            //So for example : Item Location, the above will not work
            #region Declaration
            bool restoreFlag = false;
            bool isNewRecord = this.IsNew;//Because this flag will be changed during saving, we need to know the original value when error occurs
            int newRecordKey = 0;

            TASAlert copyTASAlert = null;
            TASAlertDetSubs copyTASAlertDetSubs = null;

            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._TASAlert != null)
                    copyTASAlert = this._TASAlert.Clone();

                if (this._TASAlertDetSubs != null)
                    copyTASAlertDetSubs = this._TASAlertDetSubs.Copy();


                #endregion

                #region Check Permission
                if (this.IsOpenReadOnly)
                {
                    MsgBox.Show(MsgID.Common.RecordIsReadOnly);
                    return false;
                }
                else
                {
                    if (this.IsNew)
                    {
                        if (SECPermUtility.Add(PermID, true) == false)
                            return false;
                    }
                    else
                    {
                        if (SECPermUtility.Edit(PermID, true) == false)
                            return false;
                    }
                }
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        #region Save Data
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        #region Set default value for fields that cannot be empty but can have a general default value

                        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);//Get Server Date and Time (sdt)
                        _TASAlert._createDate = GFunc.NEDateTime(_TASAlert.CreateDate, svrDateTime);
                        _TASAlert._createUserKey = GFunc.NEInt(_TASAlert.CreateUserKey, AppInfor.currentUserKey);
                        _TASAlert._lastModifiedDate = svrDateTime;
                        _TASAlert._lastModifiedUserKey = AppInfor.currentUserKey;
                     
                        #endregion

                        #region Validation
                        if (ValidationForAlertInfo(cn) == false)
                            return false;

                        if (Validation_Detail(cn) == false)
                            return false;

                        #endregion

                        #region Save Record
                        //calculate NextRunDateTime
                        ObjTASAlert.NextRunDateTime = GetNextRunDateTime();
                        
                        if (IsNew)
                        {
                            if (!_TASAlert.Insert(cn, out newRecordKey))
                                return false;

                            //Update new JobKey to details tables
                            foreach (DataRow row in _TASAlertDetSubs.Rows)
                            {
                                row["AlertKey"] = newRecordKey;

                            }
                            _TASAlertDetSubs.AcceptChanges();

                            if (!_TASAlertDetSubs.Save(cn, newRecordKey,_TASAlertDetSubs))
                                return false;
                        }
                        else
                        {
                            if (ObjTASAlert._TaskState > 2) 
                                ObjTASAlert._TaskState = 2;
                            if (!_TASAlert.Update(cn))
                                return false;

                            if (!_TASAlertDetSubs.Delete(cn, new TASAlertDetSubs.Criteria(_TASAlert.AlertKey, 0)))
                                return false;
                            if (!_TASAlertDetSubs.Save(cn, _TASAlert.AlertKey, _TASAlertDetSubs))
                                return false;
                        }
                        #endregion

                        #region For New Record perform: Locking, set new recordKey
                        if (IsNew)
                        {
                            if (SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newRecordKey))
                                _TASAlert.AlertKey = (int)newRecordKey;
                            else
                                return false;
                        }
                        #endregion

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) 
                            throw new Exception("Transaction has aborted."); 
                        scope.Complete();

                        #region Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        #endregion

                        #endregion
                    }
                }

                #region Update Auditlog
                if (isNewRecord)
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _TASAlert.AlertKey, _TASAlert.AlertID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _TASAlert, _TASAlertDetSubs });
                else
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _TASAlert.AlertKey, _TASAlert.AlertID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _TASAlert, _TASAlertDetSubs });
                #endregion

                restoreFlag = false;
                return true;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._TASAlert = copyTASAlert;
                    this._TASAlertDetSubs = copyTASAlertDetSubs;
                }
                #endregion

                #region Dispose Backup Objects
                copyTASAlert = null;
                copyTASAlertDetSubs = null;
                #endregion
            }
        }
        public bool GetEdit(int alertKey)
        {
            #region Declaration
            bool restoreFlag = false;
            TASAlert copyTASAlert = null;
            TASAlertDetSubs copyTASAlertDetSubs = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._TASAlert != null)
                    copyTASAlert = this._TASAlert.Clone();

                if (this._TASAlertDetSubs != null)
                    copyTASAlertDetSubs = this._TASAlertDetSubs.Copy();

                #endregion

                #region Check Security Permission
                if (SECPermUtility.Edit(PermID, true) == false)
                    return false;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        #region Get Data
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        // Check Lock
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, alertKey, 0, _guID))
                            return false;

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                            return false;

                        // Add Lock
                        if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, alertKey))
                            return false;

                        #region Get Record
                        if (_TASAlert.Fetch(cn, new TASAlert.Criteria(alertKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }


                        //Record Not Found
                        if (GFunc.NEInt(this._TASAlert.AlertKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }

                        _TASAlertDetSubs.Clear();
                        if (_TASAlertDetSubs.Fetch(cn, new TASAlertDetSubs.Criteria(alertKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        #endregion

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) 
                            throw new Exception("Transaction has aborted."); 
                        scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        this._isOpenReadOnly = false;

                        //Attach Events
                        this._TASAlert.PropertyChanged += new PropertyChangedEventHandler(Obj_PropertyChanged);
                        #endregion
                    }
                }
                restoreFlag = false;
                return true;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._TASAlert = copyTASAlert;
                    this._TASAlertDetSubs = copyTASAlertDetSubs;
                }
                #endregion

                #region Dispose Backup Objects
                copyTASAlert = null;
                copyTASAlertDetSubs = null;
                #endregion
            }
        }
        public bool GetReadOnly(int alertKey)
        {
            #region Declaration
            bool restoreFlag = false;
            TASAlert copyTASAlert = null;
            TASAlertDetSubs copyTASAlertDetSubs = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._TASAlert != null)
                    copyTASAlert = this._TASAlert.Clone();

                if (this._TASAlertDetSubs != null)
                    copyTASAlertDetSubs = this._TASAlertDetSubs.Copy();


                #endregion

                #region Check Security Permission
                if (SECPermUtility.Read(PermID, true) == false)
                    return false;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        #region Get Data
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        // Remove all locks by GUID except inprogress Locking
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey) == false)
                            return false;

                        #region Get Data
                        if (_TASAlert.Fetch(cn, new TASAlert.Criteria(alertKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _TASAlertDetSubs.Clear();
                        if (_TASAlertDetSubs.Fetch(cn, new TASAlertDetSubs.Criteria(alertKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }
                        #endregion

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        this._isOpenReadOnly = true;
                        #endregion
                    }
                }
                restoreFlag = false;
                return true;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._TASAlert = copyTASAlert;
                    this._TASAlertDetSubs = copyTASAlertDetSubs;
                }
                #endregion

                #region Dispose Backup Objects
                copyTASAlert = null;
                copyTASAlertDetSubs = null;
                #endregion
            }
        }
        public bool SetReadOnlyData(DataTable dtHeader, DataSet dsDetail)
        {
            try
            {
                #region Check Security Permission
                if (SECPermUtility.Read(PermID, true) == false)
                    return false;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        #region Get Data
                        cn.Open();

                        // Remove all locks by GUID except inprogress Locking
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                            return false;

                        if (GFunc.IsNE(_TASAlert))
                            _TASAlert = TASAlert.New();
                        GFunc.ConvertDataTableToObject(dtHeader, _TASAlert);

                        _TASAlertDetSubs = new TASAlertDetSubs(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[0], _TASAlertDetSubs);

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) 
                            throw new Exception("Transaction has aborted."); 
                        scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        this._isOpenReadOnly = true;
                        #endregion
                    }
                }
                return true;
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
        public bool Delete()
        {
            #region Declaration
            bool restoreFlag = false;
            TASAlert copyTASAlert = null;
            TASAlertDetSubs copyTASAlertDetSubs = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._TASAlert != null)
                    copyTASAlert = this._TASAlert.Clone();

                if (this._TASAlertDetSubs != null)
                    copyTASAlertDetSubs = this._TASAlertDetSubs.Copy();

                #endregion

                #region Check IsReadOnly, IsNew and Security Permission
                if (this.IsOpenReadOnly)
                {
                    MsgBox.Show(MsgID.Common.RecordIsReadOnly);
                    return false;
                }
                else
                {
                    if (IsNew)
                    {
                        return false;
                    }
                    else
                    {
                        if (SECPermUtility.Delete(PermID, true) == false)
                            return false;
                    }
                }
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        #region Delete Data
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        //Record Locking
                        if (SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _TASAlert.AlertKey, GUID) == false)
                            return false;

                        //Check the record is used in other dependency tables
                        if (GFunc.CheckKeyDependantsExists(cn, "AlertKey", _TASAlert.AlertKey.Value, _TASAlert.AlertID))
                            return false;

                        //Delete Record
                        if (_TASAlert.Delete(cn, new TASAlert.Criteria(_TASAlert.AlertKey)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.DeleteFail);
                            return false;
                        }

                        // Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey) == false)
                            return false;

                        // Create New
                        this._TASAlert = TASAlert.New();
                        this._TASAlertDetSubs = new TASAlertDetSubs(cn);

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) 
                            throw new Exception("Transaction has aborted."); 
                        scope.Complete();

                        #region Set Flag
                        this._isDirty = false;
                        this._isNew = true;
                        this._isOpenReadOnly = false;
                        #endregion

                        #endregion
                    }
                }

                // AuditLog
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyTASAlert.AlertKey, copyTASAlert.AlertID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyTASAlert, copyTASAlertDetSubs });

                restoreFlag = false;
                return true;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._TASAlert = copyTASAlert;
                    this._TASAlertDetSubs = copyTASAlertDetSubs;
                }
                #endregion

                #region Dispose Backup Objects
                copyTASAlert = null;
                copyTASAlertDetSubs = null;
                #endregion
            }
        }
        public bool Dispose()
        {
            try
            {
                if (!SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0))
                    return false;
                return true;
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
     
        private void Obj_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!this._isOpenReadOnly)
                this._isDirty = true;
        }
             

        private bool ValidationForAlertInfo(SqlConnection cn)
        {
            bool isValidation = true;
            string processOK = GVar.gcPass;
            string errorMsgID = string.Empty;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());

            try
            {
                // Clear Error in UI
                if (!GFunc.IsNE(this.ErrorNotifierHeader_Clear))
                    this.ErrorNotifierHeader_Clear.Invoke(this, e);

                if (this.IsNew)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.AlertKey, "AlertKey", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);                    
                }
                else
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.AlertKey, "AlertKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);                    
                }

                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.AlertID, "AlertID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.AlertDes, "AlertDes", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.AlertApplyGrp, "AlertApplyGrp", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.AlertApplyTo, "AlertApplyTo", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.AlertIDFrom, "AlertIDFrom", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.AlertIDTo, "AlertIDTo", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.AlertCondition, "AlertCondition", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.AlertValueAmt, "AlertValueAmt", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.AlertValueDate, "AlertValueDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.AlertLastActivateDate, "AlertLastActivateDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.RecurType, "RecurType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.RecurIntHourMins, "RecurIntHourMins", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.RecurIntDayNum, "RecurIntDayNum", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.RecurIntWeekNum, "RecurIntWeekNum", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.RecurIntWeekDay, "RecurIntWeekDay", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.RecurIntMthNum, "RecurIntMthNum", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.RecurIntMthDayNum, "RecurIntMthDayNum", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.RecurIntMthWeek, "RecurIntMthWeek", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.RecurIntMthDay, "RecurIntMthDay", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.RecurIntYearNum, "RecurIntYearNum", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.RecurIntYearDayNum, "RecurIntYearDayNum", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.RecurIntYearMthNum, "RecurIntYearMthNum", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.RecurIntYearMthDay, "RecurIntYearMthDay", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.RecurIntYearMthWeek, "RecurIntYearMthWeek", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);

                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.Custom4, "Custom4", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASAlert.Custom5, "Custom5", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);

                if (e.PropertyMessage.Count > 0)
                {
                    ErrorMessageID = MsgID.Common.ValidationFail;
                    isValidation = false;                  
                }
                else
                {
                    if (_TASAlert.Validation(cn, new TASAlert.Criteria(_TASAlert.AlertKey, _TASAlert.AlertID,0), this.IsNew))
                    {
                        isValidation = true;
                    }
                    else
                    {
                        e.PropertyMessage.Add("AlertID", SysMessageUtility.Get(cn, MsgID.Validation.DuplicateRecordID + "AlertID"));
                        isValidation = false;
                    }                   
                }

                if (isValidation==false)
                    this.ErrorNotifierHeader_Set.Invoke(this, e);

                return isValidation;
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
        public bool Validation_Detail(SqlConnection cn)
        {
            //Validation Check for calls from Factory (Save method)
            string msgID = string.Empty;
            bool processOK = true;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
            try
            {
                if (_TASAlertDetSubs.Rows.Count == 0)
                {
                    DocComUtility.InvokeGridNotifier("tagrdSubscriberEmail", e, this.ErrorNotifierHeader_Set);
                    throw new TAException("Validation detail failed. At least one record of Alert Type(Email or SMS) must be entered.");
                }

                foreach (DataRow dr in this._TASAlertDetSubs.Rows)
                {
                    msgID = string.Empty;
                    processOK = true;

                    if (dr.RowState == DataRowState.Deleted)
                        continue;
                    else
                    {
                        //Check Column values                   
                        int alertType=GFunc.NEInt(dr["AlertType"],0);
                        foreach (DataColumn c in dr.Table.Columns)
                        {
                            Validation_DetailCheck(dr[c.ColumnName.ToString()], c.ColumnName.ToString(),alertType, false, ref processOK, e);
                        }

                        //Check for Duplicate records
                        if (processOK)
                            Validation_DetailRelation(dr["UserKey"],dr["AlertType"], false, ref processOK, e);

                        //Set RowError Text
                        if (processOK == false)
                        {
                            foreach (object key in e.PropertyMessage.Keys)
                            {
                                if (GFunc.IsNE(msgID) == false)
                                    msgID += " and ";

                                msgID += SysMessageUtility.Get(cn, e.PropertyMessage[key].ToString());
                            }

                            GFunc.SetRowError(dr, msgID);
                            if(GFunc.NEInt(dr["AlertType"],0)==30)
                                DocComUtility.InvokeGridNotifier("tagrdSubscriberEmail", e, this.ErrorNotifierHeader_Set);
                            else if (GFunc.NEInt(dr["AlertType"], 0) == 40)
                                DocComUtility.InvokeGridNotifier("tagrdSubscriberSMS", e, this.ErrorNotifierHeader_Set);
                            throw new TAException(BOLib.MsgID.Common.ValidationFail);
                        }
                        else
                            dr.RowError = string.Empty;
                    }
                }
                return processOK;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            
        }//Completed
        public bool Validation_Detail(UltraGridRow grdrow, string fieldToCheck)
        {
            //Validation Check for calls from FORM (CustomCellUpdate, BeforeRowUpdate)
            string msgID = string.Empty;
            bool processOK = true;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
            try
            {
                //Check Column values
                int alertType = GFunc.NEInt(grdrow.Cells["AlertType"].Value, 0);

                if (fieldToCheck == string.Empty) 
                {
                    foreach (UltraGridCell c in grdrow.Cells)
                    {
                        Validation_DetailCheck(c.Value, c.Column.Key,alertType, false, ref processOK, e);                   
                    }
                }
                else
                    Validation_DetailCheck(grdrow.Cells[fieldToCheck].Value, fieldToCheck, alertType, false, ref processOK, e);

                //Check for Duplicate records
                if (processOK)
                    Validation_DetailRelation(grdrow.Cells["UserKey"].Value, grdrow.Cells["AlertType"].Value, grdrow.IsAddRow, ref processOK, e);

                //Set RowError Text
                if (processOK == false)
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();
                        foreach (object key in e.PropertyMessage.Keys)
                        {
                            if (GFunc.IsNE(msgID) == false)
                                msgID += " and ";

                            msgID += SysMessageUtility.Get(cn, e.PropertyMessage[key].ToString());
                        }
                    }
                    ((DataRowView)(grdrow.ListObject)).Row.RowError = msgID;                
                }
                else
                    ((DataRowView)(grdrow.ListObject)).Row.RowError = string.Empty;

                return processOK;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            
        }//Completed
        public bool Validation_DetailCheck(object propValue, string CheckNm, int AlertType, bool failonError, ref bool processOK, UINotifierEventArgs e)
        {
            try
            {
                if (_isNew)
                {
                    BaseUtility.Validation(propValue, "AlertKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "UserKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                }
                else
                {
                    BaseUtility.Validation(propValue, "AlertKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "UserKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                }
                
                switch(AlertType)
                {
                    case (int)GEnum.AlertType.Email:

                        BaseUtility.Validation(propValue, "Email", CheckNm, GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "EmailSubject", CheckNm, GEnum.DataType.String, GEnum.Require.Yes, 250, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "EmailStandardMessage", CheckNm, GEnum.DataType.String, GEnum.Require.Yes, 250, null, null, null, null, ref processOK, failonError, e);                   
                        break;

                    case (int)GEnum.AlertType.SMS:
                        BaseUtility.Validation(propValue, "PhoneNumber", CheckNm, GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "SMSText", CheckNm, GEnum.DataType.String, GEnum.Require.Yes, 250, null, null, null, null, ref processOK, failonError, e);
                        break;
                    case (int)GEnum.AlertType.Popup_Messgae:
                        BaseUtility.Validation(propValue, "AlertMessage", CheckNm, GEnum.DataType.String, GEnum.Require.Yes, 250, null, null, null, null, ref processOK, failonError, e);
                        break;
                }          
                
                return processOK;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            
        }//Completed
        public bool Validation_DetailRelation(object propValue,object AlertType, bool IsAddRow, ref bool processOK, UINotifierEventArgs e)
        {
            bool errorFound = false;
            try
            {
                var dupList = _TASAlertDetSubs.AsEnumerable().ToList().FindAll(o =>
                                (o.Field<int?>("UserKey").Value == ((int?)propValue).Value) && o.Field<int?>("AlertType").Value == (int)AlertType );

                if (IsAddRow)
                {
                    if (dupList.Count > 0)
                        errorFound = true;
                }
                else
                {
                    if (dupList.Count > 1)
                        errorFound = true;
                }
                if (errorFound)
                {
                    e.PropertyMessage.Add("rowError", "User" + MsgID.Validation.DuplicateRecord);
                    processOK = false;
                }
                else
                    processOK = true;

                return processOK;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            
        }//Completed

        private DateTime GetNextRunDateTime()
        {
           
            DateTime NextRunDateTime = new DateTime();

            if (ObjTASAlert.RecurType == 10)//Minute
                NextRunDateTime = DateTime.Now.AddMinutes(ObjTASAlert.RecurIntHourMins);
            if (ObjTASAlert.RecurType == 20)//Hourly
                NextRunDateTime = DateTime.Now.AddHours(ObjTASAlert.RecurIntHourMins);
            else if (ObjTASAlert.RecurType == 30)//Daily
            {
                if (DateTime.Today.Date + DateTime.Today.TimeOfDay <= DateTime.Now)
                {
                    NextRunDateTime = DateTime.Today.Date.AddDays(ObjTASAlert.RecurIntDayNum) + DateTime.Today.TimeOfDay;
                }
                else
                    NextRunDateTime = DateTime.Today.Date + DateTime.Today.TimeOfDay;
               
            }
            else if (ObjTASAlert.RecurType == 40)//Weekly
            {               
                if (ObjTASAlert.RecurIntWeekDay < (int)DateTime.Today.Date.DayOfWeek)
                {
                    NextRunDateTime = DateTime.Today.Date.AddDays(7 - (int)DateTime.Today.DayOfWeek + ObjTASAlert.RecurIntWeekDay) + DateTime.Today.TimeOfDay;
                }
                else
                    NextRunDateTime = DateTime.Today.Date.AddDays(ObjTASAlert.RecurIntWeekDay - (int)DateTime.Today.Date.DayOfWeek) + DateTime.Today.TimeOfDay;
            
            }
            else if (ObjTASAlert.RecurType == 50)//Monthly
            {
                if (ObjTASAlert.RecurIntMthDayNum != 0)//By exact day
                {
                    if (ObjTASAlert.RecurIntMthDayNum < DateTime.Today.Date.Day ||
                        (ObjTASAlert.RecurIntMthDayNum == DateTime.Today.Date.Day && DateTime.Today.TimeOfDay < DateTime.Now.TimeOfDay))
                    {
                        NextRunDateTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month + ObjTASAlert.RecurIntMthNum, ObjTASAlert.RecurIntMthDayNum) + DateTime.Today.TimeOfDay;
                    }
                    else
                        NextRunDateTime = new DateTime(DateTime.Today.Date.Year, DateTime.Today.Date.Month, ObjTASAlert.RecurIntMthDayNum) + DateTime.Today.TimeOfDay;

                }
                else
                {
                               
                    DateTime fallenDate = DateTime.Today.Date;
                    int week;
                    int WeekNumOfStartDate;
                    int DayOfWeek;
                    if ((DateTime.IsLeapYear(fallenDate.Year) == false && ObjTASAlert.RecurIntMthWeek == 5 && fallenDate.Month == 2))
                        week = 4;
                    else
                        week = ObjTASAlert.RecurIntMthWeek;

                    DateTime firstDayOfMonth = new DateTime(fallenDate.Year, fallenDate.Month, 1);
                    DayOfWeek = (int)new DateTime(fallenDate.Year, fallenDate.Month, 1).DayOfWeek;
                    int Day = ObjTASAlert.RecurIntMthDay;

                    //To get Week Number of the StateDate 
                    DateTime date = DateTime.Today.Date;
                    DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);
                    while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                        date = date.AddDays(1);
                    WeekNumOfStartDate = ((int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1);

                    //found in this month
                    if (ObjTASAlert.RecurIntMthWeek > WeekNumOfStartDate ||
                        (ObjTASAlert.RecurIntMthWeek == WeekNumOfStartDate && DayOfWeek < Day) ||
                        (ObjTASAlert.RecurIntMthWeek == WeekNumOfStartDate && DayOfWeek == Day && DateTime.Today.TimeOfDay > DateTime.Now.TimeOfDay)) //if the required week day is not exist in current week and RecurWeek is found in the month of StartDate , skip to next week
                    {
                        NextRunDateTime = firstDayOfMonth.AddDays((week * 7 - DayOfWeek) + Day).Date + DateTime.Today.TimeOfDay;
                    }
                    //not found in this month, skip to next month
                    else if (ObjTASAlert.RecurIntMthWeek < WeekNumOfStartDate ||
                        (ObjTASAlert.RecurIntMthWeek == WeekNumOfStartDate && DayOfWeek > Day) ||
                        (ObjTASAlert.RecurIntMthWeek == WeekNumOfStartDate && DayOfWeek == Day && DateTime.Today.TimeOfDay < DateTime.Now.TimeOfDay)) //RecurWeek is not found in the month of StartDate, skip to next month                                                                                                     
                    {
                        firstDayOfMonth = firstDayOfMonth.AddMonths(ObjTASAlert.RecurIntMthNum);
                        DayOfWeek = (int)new DateTime(firstDayOfMonth.Year, firstDayOfMonth.Month, 1).DayOfWeek;
                        NextRunDateTime = firstDayOfMonth.AddDays(((week - 1) * 7) + (Day - DayOfWeek)).Date + DateTime.Today.TimeOfDay;
                    }
                    else
                    {
                        NextRunDateTime = firstDayOfMonth.AddDays(((week - 1) * 7)).Date + DateTime.Today.TimeOfDay;
                    }
                }

            
            }
            else if (ObjTASAlert.RecurType == 60)//Yearly //To start 17-Feb
            {
                if (ObjTASAlert.RecurIntYearDayNum != 0)//By exact day
                {
                    if (ObjTASAlert.RecurIntYearMthNum > DateTime.Today.Date.Month
                        || ((ObjTASAlert.RecurIntYearMthNum == DateTime.Today.Date.Month && ObjTASAlert.RecurIntYearDayNum > DateTime.Today.Date.Day) && DateTime.Today.TimeOfDay < DateTime.Now.TimeOfDay))
                    {
                        NextRunDateTime = new DateTime(DateTime.Today.Date.Year, ObjTASAlert.RecurIntYearMthNum, ObjTASAlert.RecurIntYearDayNum) + DateTime.Today.TimeOfDay;
                    }
                    else
                        NextRunDateTime = new DateTime(DateTime.Today.Date.Year + ObjTASAlert.RecurIntYearNum, ObjTASAlert.RecurIntYearMthNum, ObjTASAlert.RecurIntYearDayNum) + DateTime.Today.TimeOfDay;
                }
                else
                {

                    DateTime fallenDate = DateTime.Today.Date;

                    int week;
                    int WeekNumOfStartDate;
                    int DayOfWeek;
                    if ((DateTime.IsLeapYear(fallenDate.Year) == false && ObjTASAlert.RecurIntMthWeek == 5 && fallenDate.Month == 2))
                        week = 4;
                    else
                        week = ObjTASAlert.RecurIntYearMthWeek;

                    DateTime firstDayOfMonth = new DateTime(fallenDate.Year, 1, 1);
                    DayOfWeek = (int)new DateTime(fallenDate.Year, 1, 1).DayOfWeek;
                    int Day = ObjTASAlert.RecurIntYearMthDay;

                    //To get Week Number of the StateDate 
                    DateTime date = DateTime.Today.Date;
                    DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);
                    while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                        date = date.AddDays(1);
                    WeekNumOfStartDate = ((int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1);

                    //found in this year
                    if (ObjTASAlert.RecurIntYearMthNum > DateTime.Today.Date.Month ||
                        (ObjTASAlert.RecurIntYearMthNum >= DateTime.Today.Date.Month && ObjTASAlert.RecurIntYearMthWeek > WeekNumOfStartDate) ||
                        (ObjTASAlert.RecurIntYearMthNum == DateTime.Today.Date.Month && ObjTASAlert.RecurIntYearMthWeek == WeekNumOfStartDate && DayOfWeek < Day) ||
                        (ObjTASAlert.RecurIntYearMthNum == DateTime.Today.Date.Month && ObjTASAlert.RecurIntYearMthWeek == WeekNumOfStartDate && DayOfWeek == Day && DateTime.Today.TimeOfDay > DateTime.Now.TimeOfDay)) //if the required week day is not exist in current week and RecurWeek is found in the month of StartDate , skip to next week
                    {
                        firstDayOfMonth = firstDayOfMonth.AddMonths(ObjTASAlert.RecurIntYearMthNum - 1);
                        DayOfWeek = (int)firstDayOfMonth.DayOfWeek;
                        if (DayOfWeek < Day)
                            NextRunDateTime = firstDayOfMonth.AddDays(((week - 1) * 7 - DayOfWeek) + Day).Date + DateTime.Today.TimeOfDay;
                        else
                            NextRunDateTime = firstDayOfMonth.AddDays((week * 7 - DayOfWeek) + Day).Date + DateTime.Today.TimeOfDay;
                    }
                    //not found in this year, skip to next year
                    else if (ObjTASAlert.RecurIntYearMthNum < DateTime.Today.Date.Month ||
                        (ObjTASAlert.RecurIntYearMthNum <= DateTime.Today.Date.Month && ObjTASAlert.RecurIntYearMthWeek < WeekNumOfStartDate) ||
                        (ObjTASAlert.RecurIntYearMthNum == DateTime.Today.Date.Month && ObjTASAlert.RecurIntYearMthWeek == WeekNumOfStartDate && DayOfWeek > Day) ||
                        (ObjTASAlert.RecurIntYearMthNum == DateTime.Today.Date.Month && ObjTASAlert.RecurIntYearMthWeek == WeekNumOfStartDate && DayOfWeek == Day && DateTime.Today.TimeOfDay < DateTime.Now.TimeOfDay)) //RecurWeek is not found in the month of StartDate, skip to next month                                                                                                     
                    {
                        firstDayOfMonth = firstDayOfMonth.AddYears(ObjTASAlert.RecurIntYearNum);
                        firstDayOfMonth = new DateTime(firstDayOfMonth.Year, ObjTASAlert.RecurIntYearMthNum, 1);
                        DayOfWeek = (int)new DateTime(firstDayOfMonth.Year, firstDayOfMonth.Month, 1).DayOfWeek;
                        NextRunDateTime = firstDayOfMonth.AddDays(((week - 1) * 7) + (Day - DayOfWeek)).Date + DateTime.Today.TimeOfDay;
                    }
                    else
                    {
                        NextRunDateTime = firstDayOfMonth.AddDays(((week - 1) * 7)).Date + DateTime.Today.TimeOfDay;
                    }
                }

         
                
            }
            return NextRunDateTime;
        }

        private void DetailDefaultValueSet()
        {
            _TASAlertDetSubs.Columns["AlertKey"].DefaultValue = _TASAlert.AlertKey;
        }
        private void HeaderDefaultValueSet()
        {
            _TASAlert.TaskState = 1;
        }

        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _TASAlert, _TASAlertDetSub }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _TASAlert, _TASAlertDetSub }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

    }
}
