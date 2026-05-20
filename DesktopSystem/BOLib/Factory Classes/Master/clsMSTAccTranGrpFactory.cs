using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class MSTAccTranGrpFactory : CommandBase
    {
        #region Member variables and constants

        private MSTAccTranGrp _MSTAccTranGrp = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isReadOnly = false;
        private int _guID = 0;

        //System Code Key for this Factory.
        private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Transaction_Group;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        //Permission ID for this Factory.
        public const string constPermID = GVar.PermissionID.Transaction_Group;
        public string PermID { get { return constPermID; } }

        //Default Parent Group Key
        private const int DEF_ParentKey = 0;

        //Event Declaration
        public GVar.DirtyEvent dirtyEvent = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;
        #endregion // Member variables and constant

        #region Factory Properties
        public MSTAccTranGrp ObjMSTAccTranGrp
        {
            get
            {
                return this._MSTAccTranGrp;
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
        public MSTAccTranGrpFactory(GEnum.InstanceMode instanceMode)
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
            
        }//Completed
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
                            this._MSTAccTranGrp = new MSTAccTranGrp();
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
            
        }//Completed

        //Method
        public bool New(int parentKey)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTAccTranGrp copyMSTAccTranGrp = null;
            #endregion

            try
            {
                
                #region Make backup of objects for restore purpose
                if (this._MSTAccTranGrp != null)
                    copyMSTAccTranGrp = this._MSTAccTranGrp.Clone();
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
                        this._MSTAccTranGrp = MSTAccTranGrp.New(parentKey);

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;

                        //Attach Events
                        this._MSTAccTranGrp.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Obj_PropertyChanged);
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
                    this._MSTAccTranGrp = copyMSTAccTranGrp;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTAccTranGrp = null;
                #endregion
            }
        }//Completed
        public bool GetEdit(int TranGrpKey, string TranGrpID)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTAccTranGrp copyMSTAccTranGrp = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTAccTranGrp != null)
                    copyMSTAccTranGrp = this._MSTAccTranGrp.Clone();
                #endregion

               
                #region Check Security Permission
                if (SECPermUtility.Edit(constPermID, true) == false)
                    return false;
                #endregion

                #region Get Key to open record
                if (TranGrpID != null && TranGrpID != string.Empty)
                    TranGrpKey = MSTAccTranGrp.Get(TranGrpID,2).TranGrpKey.Value;

                if (TranGrpKey == 0)
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
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, TranGrpKey, 0, _guID))
                            return false;

                        //Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey) == false)
                            return false;

                        //Add Lock
                        if (SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, TranGrpKey) == false)
                            return false;

                        //Get Record                                 
                        if (this._MSTAccTranGrp.Fetch(cn, new MSTAccTranGrp.Criteria(TranGrpKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        //Record Not Found
                        if (GFunc.NEInt(this._MSTAccTranGrp._tranGrpKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }


                        this._MSTAccTranGrp.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Obj_PropertyChanged);

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
                    this._MSTAccTranGrp = copyMSTAccTranGrp;
                #endregion

                #region Dispose Backup Objects
                copyMSTAccTranGrp = null;
                #endregion
            }
        }//Completed
        public bool GetReadOnly(int TranGrpKey, string TranGrpID)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTAccTranGrp copyMSTAccTranGrp = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTAccTranGrp != null)
                    copyMSTAccTranGrp = this._MSTAccTranGrp.Clone();
                #endregion

            
                #region Check Security Permission
                if (SECPermUtility.Read(constPermID, true) == false)
                    return false;
                #endregion

                #region Get Key to open record
                if (TranGrpID != null && TranGrpID != string.Empty)
                    TranGrpKey = MSTAccTranGrp.Get(TranGrpID,2).TranGrpKey.Value;

                if (TranGrpKey == 0)
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
                        if (this._MSTAccTranGrp.Fetch(cn, new MSTAccTranGrp.Criteria(TranGrpKey, 1)) == false)
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
                    this._MSTAccTranGrp = copyMSTAccTranGrp;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTAccTranGrp = null;
                #endregion
            }
        }//Completed
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

                        if (GFunc.IsNE(_MSTAccTranGrp))
                            _MSTAccTranGrp = MSTAccTranGrp.New(DEF_ParentKey);
                        GFunc.ConvertDataTableToObject(dtHeader, _MSTAccTranGrp);

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
        }//Completed
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
            BOLib.MSTAccTranGrp copyMSTAccTranGrp = null;

            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTAccTranGrp != null)
                    copyMSTAccTranGrp = this._MSTAccTranGrp.Clone();
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
                        if (this.IsNew && GFunc.IsNE(_MSTAccTranGrp._tranGrpID))
                        {
                            if (SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _MSTAccTranGrp._tranGrpDes) == false)
                                return false;

                            _MSTAccTranGrp._tranGrpID = autoID;
                        }
                        #endregion

                        #region Set default value for fields that cannot be empty but can have a general default value
                        //Get Server Date and Time (sdt)
                        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                        _MSTAccTranGrp._createDate = GFunc.NEDateTime(_MSTAccTranGrp.CreateDate, svrDateTime);
                        _MSTAccTranGrp._createUserKey = GFunc.NEInt(_MSTAccTranGrp.CreateUserKey, AppInfor.currentUserKey);
                        _MSTAccTranGrp._lastModifiedDate = svrDateTime;
                        _MSTAccTranGrp._lastModifiedUserKey = AppInfor.currentUserKey;
                        #endregion

                        #region Validation
                        if (Validation_Header(cn) == false)
                            return false;
                        #endregion

                        #region Save Record
                        //Note: there is no delete permission for Sales Rep Payroll (only Read,Edit)
                        if (IsNew)
                        {
                            if (_MSTAccTranGrp.Insert(cn, out newRecordKey) == false)
                            {
                                MsgBox.Show(cn,MsgID.Common.SaveFail);
                                return false;
                            }
                        }
                        else
                        {
                            if (_MSTAccTranGrp.Update(cn) == false)
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
                                _MSTAccTranGrp._tranGrpKey = newRecordKey;
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
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _MSTAccTranGrp._tranGrpKey, _MSTAccTranGrp._tranGrpID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTAccTranGrp });
                else
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _MSTAccTranGrp._tranGrpKey, _MSTAccTranGrp._tranGrpID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTAccTranGrp });
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
                    this._MSTAccTranGrp = copyMSTAccTranGrp;
                #endregion

                #region Dispose Backup Objects
                copyMSTAccTranGrp = null;
                #endregion
            }
        }//Completed
        public bool Delete()
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTAccTranGrp copyMSTAccTranGrp = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTAccTranGrp != null)
                    copyMSTAccTranGrp = this._MSTAccTranGrp.Clone();
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
                            if (SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _MSTAccTranGrp._tranGrpKey, GUID) == false)
                                return false;

                            //Check the record is used in other dependency tables
                            if (GFunc.CheckKeyDependantsExists(cn, "TranGrpKey", _MSTAccTranGrp._tranGrpKey.Value, _MSTAccTranGrp._tranGrpID))
                                return false;

                            //Delete Record
                            if (_MSTAccTranGrp.Delete(cn, new MSTAccTranGrp.Criteria(_MSTAccTranGrp._tranGrpKey)) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.DeleteFail);
                                return false;
                            }

                            //Remove Lock
                            if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey) == false)
                                return false;

                            //Create New
                            this._MSTAccTranGrp = MSTAccTranGrp.New(DEF_ParentKey);

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
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyMSTAccTranGrp.TranGrpKey, copyMSTAccTranGrp.TranGrpID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyMSTAccTranGrp });

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
                    this._MSTAccTranGrp = copyMSTAccTranGrp;
                #endregion

                #region Dispose Backup Objects
                copyMSTAccTranGrp = null;
                #endregion
            }
        }//Completed
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
                
        }//Completed
        
        //Validation
        public bool Validation_Header(SqlConnection cn)
        {
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
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccTranGrp.TranGrpKey, "TranGrpKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccTranGrp.TranGrpID, "TranGrpID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                }
                else
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccTranGrp.TranGrpKey, "TranGrpKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccTranGrp.TranGrpID, "TranGrpID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccTranGrp.TranGrpKeyParent, "TranGrpKeyParent", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccTranGrp.TranGrpDes, "TranGrpDes", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccTranGrp.TranGrpTitle, "TranGrpTitle", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccTranGrp.InActive, "InActive", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccTranGrp.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccTranGrp.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccTranGrp.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccTranGrp.Custom4, "Custom4", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTAccTranGrp.Custom5, "Custom5", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                #endregion

                if (e.PropertyMessage.Count == 0)
                {
                    //StoreProcedure Validation
                    if (_MSTAccTranGrp.Validation(cn, new MSTAccTranGrp.Criteria(GFunc.NEInt(_MSTAccTranGrp.TranGrpKey,0), _MSTAccTranGrp.TranGrpID), this.IsNew))
                    {
                        return true;
                    }
                    else
                    {
                        e.PropertyMessage.Add("TranGrpID", SysMessageUtility.Get(cn, MsgID.Validation.DuplicateRecordID + "TranGrpID"));
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
            if (this._isReadOnly == false)
            {
                if (this.dirtyEvent != null)
                    this.dirtyEvent.Invoke(this, e);

                this._isDirty = true;
            }
        }//Completed

        //Error
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _MSTAccTranGrp }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _MSTAccTranGrp }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
    }
}
