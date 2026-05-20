using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class MSTAccDeptFactory : CommandBase
    {
        #region Member variables and constants

        private MSTAccDept _MSTAccDept = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;  //For future use
        private bool _isNew = false;
        private bool _isReadOnly = false;
        private int _guID = 0;

        //System Code Key for this Factory.
        private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Department;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        //Permission ID for this Factory.
        private const string constPermID = GVar.PermissionID.Department;
        public string PermID { get { return constPermID; } }

        //Event Declaration
        public GVar.DirtyEvent dirtyEvent = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;

        #endregion

        #region Factory Properties
        public MSTAccDept ObjMSTAccDept
        {
            get
            {
                return this._MSTAccDept;
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
        public bool IsReadOnly
        {
            get
            {
                return this._isReadOnly;
            }
        }
        public int GUID
        {
            get
            {
                return this._guID;
            }
        }

        #endregion // Constructors

        //Constructors, Initialisation
        public MSTAccDeptFactory(GEnum.InstanceMode instanceMode)
        {
            try
            {
                this._instanceMode = instanceMode;
                Initialisation();
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
        public bool Initialisation()
        {
            try
            {
                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                {
                    if (SECPermUtility.Any(constPermID, out this._isReadOnly, true) == false)
                        return false;

                    using (TransactionScope scope = new TransactionScope())
                    {
                        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                        {
                            cn.Open();

                            //Get GUID Instance
                            //FORM will check for GUID > 0 to indicate Factory is valid
                            if ((this._guID = SysOptionUtility.GetNewLockingGUID(cn)) == 0)
                            {
                                this._guID = -1;
                                return false;
                            }

                            //Locking
                            if (SysLockUtility.IsProcessLock(cn, true, GEnum.SysLockOption.ByCodKey, constCodeKey, this._guID))
                            {
                                this._guID = -1;
                                return false;
                            }

                            //Add Inprogress Lock
                            if (!SysLockUtility.AddInprogressLock(cn, true, this._guID, constCodeKey))
                            {
                                this._guID = -1;
                                return false;
                            }

                            //Commit Process   
                            this._MSTAccDept = new MSTAccDept();
                            this._isNew = false;
                            this._isReadOnly = false;
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        }
                    }
                }
                else
                {
                    //Use for situation where no locking and GUID is required but the factory is needed for some internal call
                    //for future use only
                    this._guID = 0;
                    this._isReadOnly = false;
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

        //Methods
        public bool New()
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTAccDept copyMSTAccDept = null;
            #endregion
             
            try
            {
            
                #region Make backup of objects for restore purpose
                if (this._MSTAccDept != null)
                    copyMSTAccDept = this._MSTAccDept.Clone();
                #endregion

                #region Check Security Permission
                if (SECPermUtility.Any(constPermID, out this._isReadOnly, true) == false)
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
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey) == false)
                            return false;

                        //prepare new instance           
                        this._MSTAccDept = MSTAccDept.New();

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;

                        //Attach Events
                        this._MSTAccDept.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Obj_PropertyChanged);
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
                    this._MSTAccDept = copyMSTAccDept;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTAccDept = null;
                #endregion
            }
        }
        public bool GetEdit(int deptKey)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTAccDept copyMSTAccDept = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTAccDept != null)
                    copyMSTAccDept = this._MSTAccDept.Clone();
                #endregion

                
                #region Check Security Permission
                if (SECPermUtility.Edit(constPermID, true) == false)
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

                        //Check Lock
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, deptKey, 0, _guID))
                            return false;

                        //Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey) == false)
                            return false;

                        //Add Lock
                        if (SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, deptKey) == false)
                            return false;

                        //Get Record                                 
                        if (this._MSTAccDept.Fetch(cn, new MSTAccDept.Criteria(deptKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }
                        //Record Not Found
                        if (GFunc.NEInt(this._MSTAccDept._deptKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        this._isReadOnly = false;

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
                    this._MSTAccDept = copyMSTAccDept;
                #endregion

                #region Dispose Backup Objects
                copyMSTAccDept = null;
                #endregion
            }
        }
        public bool GetReadOnly(int deptkey)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTAccDept copyMSTAccDept = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTAccDept != null)
                    copyMSTAccDept = this._MSTAccDept.Clone();
                #endregion

               
                #region Check Security Permission
                if (SECPermUtility.Read(constPermID, true) == false)
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

                        //Remove all locks by GUID except inprogress Locking
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey) == false)
                            return false;

                        //Get record
                        if (this._MSTAccDept.Fetch(cn, new MSTAccDept.Criteria(deptkey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        this._isReadOnly = true;

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
                    this._MSTAccDept = copyMSTAccDept;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTAccDept = null;
                #endregion
            }
        }
        public bool SetReadOnlyData(DataTable dtHeader, DataSet dsDetail)
        {
            try
            {
               
                #region Check Security Permission
                if (SECPermUtility.Read(constPermID, true) == false)
                    return false;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        #region Get Data
                        cn.Open();

                        //Remove all locks by GUID except inprogress Locking
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey) == false)
                            return false;

                        if (GFunc.IsNE(_MSTAccDept))
                            _MSTAccDept = MSTAccDept.New();
                        GFunc.ConvertDataTableToObject(dtHeader, _MSTAccDept);

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        this._isReadOnly = true;

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
        public bool Save()
        {
            //For Master and Reference the RecordKey is obtain after saving. 
            //But we do not need to update this Record to the Record Detail as
            //the saving process will delete all details in the server and append the detail from local to server again
            //Note: this will only work if the detail in the server is never updated by other user.
            //So for example : Item Location, this will not work
            #region Declaration
            bool restoreFlag = false;
            bool isNewRecord = this.IsNew;//Because this flag will be changed during saving, we need to know the original value when error occurs
            int? newRecordKey = 0;
            string autoID = string.Empty;
            BOLib.MSTAccDept copyMSTAccDept = null;

            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTAccDept != null)
                    copyMSTAccDept = this._MSTAccDept.Clone();
                #endregion

           
                #region Check Permission
                if (this.IsReadOnly)
                {
                    MsgBox.Show(MsgID.Common.RecordIsReadOnly);
                    return false;
                }
                else
                {
                    if (this.IsNew)
                    {
                        if (SECPermUtility.Add(constPermID, true) == false)
                            return false;
                    }
                    else
                    {
                        if (SECPermUtility.Edit(constPermID, true) == false)
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

                        #region Get Auto Record ID
                        if (this.IsNew && GFunc.IsNE(_MSTAccDept._deptID))
                        {
                            if (SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _MSTAccDept._deptNm) == false)
                                return false;

                            _MSTAccDept._deptID = autoID;
                        }
                        #endregion

                        #region Set default value for fields that cannot be empty but can have a general default value
                        //Get Server Date and Time (sdt)
                        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                        _MSTAccDept._createDate = GFunc.NEDateTime(_MSTAccDept.CreateDate, svrDateTime);
                        _MSTAccDept._createUserKey = GFunc.NEInt(_MSTAccDept.CreateUserKey, AppInfor.currentUserKey);
                        _MSTAccDept._lastModifiedDate = svrDateTime;
                        _MSTAccDept._lastModifiedUserKey = AppInfor.currentUserKey;
                        #endregion

                        #region Validation
                        if (Validation_Header(cn) == false)
                            return false;
                        #endregion

                        #region Save Record
                        //Note: there is no delete permission for Sales Rep Payroll (only Read,Edit)
                        if (IsNew)
                        {
                            if (_MSTAccDept.Insert(cn, out newRecordKey) == false)
                            {
                                MsgBox.Show(cn,MsgID.Common.SaveFail);
                                return false;
                            }
                        }
                        else
                        {
                            if (_MSTAccDept.Update(cn) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.SaveFail);
                                return false;
                            }
                        }
                        #endregion

                        #region For New Record perform: Locking, set new recordKey
                        if (IsNew)
                        {
                            if (SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newRecordKey))
                                _MSTAccDept._deptKey = newRecordKey;
                            else
                                return false;
                        }
                        #endregion

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        #region Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        #endregion

                        #endregion
                    }
                }

                #region Update Auditlog
                if (isNewRecord)
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _MSTAccDept._deptKey, _MSTAccDept._deptID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTAccDept });
                else
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _MSTAccDept._deptKey, _MSTAccDept._deptID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTAccDept });
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
                    this._MSTAccDept = copyMSTAccDept;
                #endregion

                #region Dispose Backup Objects
                copyMSTAccDept = null;
                #endregion
            }
        }
        public bool Delete()
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTAccDept copyMSTAccDept = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTAccDept != null)
                    copyMSTAccDept = this._MSTAccDept.Clone();
                #endregion

                
                #region Check IsReadOnly, IsNew and Security Permission
                if (this.IsReadOnly)
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
                        if (SECPermUtility.Delete(constPermID, true) == false)
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
                        if (SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _MSTAccDept._deptKey, GUID) == false)
                            return false;

                        //Check the record is used in other dependency tables
                        if (GFunc.CheckKeyDependantsExists(cn, "DeptKey", _MSTAccDept._deptKey.Value, _MSTAccDept._deptID))
                            return false;

                        //Delete Record
                        if (_MSTAccDept.Delete(cn, new MSTAccDept.Criteria(_MSTAccDept._deptKey)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.DeleteFail);
                            return false;
                        }

                        //Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey) == false)
                            return false;

                        //Create New
                        this._MSTAccDept = MSTAccDept.New();

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        #region Set Flag
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;
                        #endregion

                        #endregion
                    }
                }

                //Audit Log                    
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyMSTAccDept.DeptKey, copyMSTAccDept.DeptID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyMSTAccDept });

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
                    this._MSTAccDept = copyMSTAccDept;
                #endregion

                #region Dispose Backup Objects
                copyMSTAccDept = null;
                #endregion
            }
        }
        public bool Dispose()
        {
            try
            {
                if (SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0) == false)
                        return false;
                else
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

        //Validation
        public bool Validation_Header(SqlConnection cn)
        {
            //fieldNameToCheck = string.empty to check for all fields
            string processOK = GVar.gcPass;
            string errorMsgID = string.Empty;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());

            try
            {
                //Clear Error in UL
                if (GFunc.IsNE(this.ErrorNotifierHeader_Clear) == false)
                    this.ErrorNotifierHeader_Clear.Invoke(this, e);

                #region Validation for each Field
                if (this.IsNew)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccDept.DeptKey, "DeptKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccDept.DeptID, "DeptID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                }
                else
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccDept.DeptKey, "DeptKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccDept.DeptID, "DeptID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccDept.DeptNm, "DeptNm", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccDept.Inactive, "Inactive", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccDept.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccDept.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccDept.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccDept.Custom4, "Custom4", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccDept.Custom5, "Custom5", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                #endregion

                if (e.PropertyMessage.Count == 0)
                {
                    //StoreProcedure Validation
                    if (_MSTAccDept.Validation(cn, new MSTAccDept.Criteria(_MSTAccDept.DeptKey, _MSTAccDept.DeptID), this.IsNew))
                    {
                        return true;
                    }
                    else
                    {
                        e.PropertyMessage.Add("DeptID", SysMessageUtility.Get(cn, MsgID.Validation.DuplicateRecordID + "DeptID"));
                        if (GFunc.IsNE(this.ErrorNotifierHeader_Set) == false)
                            this.ErrorNotifierHeader_Set.Invoke(this, e);

                        return false;
                    }
                }
                else
                {
                    if (GFunc.IsNE(this.ErrorNotifierHeader_Set) == false)
                        this.ErrorNotifierHeader_Set.Invoke(this, e);

                    return false;
                }
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

        //Attached Events
        private void Obj_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (this._isReadOnly == false)
                {
                    if (this.dirtyEvent != null)
                        this.dirtyEvent.Invoke(this, e);

                    this._isDirty = true;
                }
            }
            catch (TAException tex)
            {
                MsgBox.Show(tex.Message);
                Error(tex);
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
                Error(ex);
            }
        }

        //Error
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _MSTAccDept }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _MSTAccDept }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
    }
}

