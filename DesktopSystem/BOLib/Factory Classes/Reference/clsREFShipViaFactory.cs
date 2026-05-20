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
    public class REFShipViaFactory : CommandBase
    {
        #region Member variables and constants

        private REFShipVia _REFShipVia = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isReadOnly = false;
        private int _guID = 0;

        //System Code Key for this Factory.
        private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Shipping_Mode;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        //Permission ID for this Factory.
        private const string constPermID = GVar.PermissionID.Shipping_Mode;
        public string PermID { get { return constPermID; } }

        //Event Declaration
        public GVar.DirtyEvent dirtyEvent = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;

        #endregion

        #region Factory Properties
        public REFShipVia ObjREFShipVia
        {
            get
            {
                return this._REFShipVia;
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

        #endregion

        //Constructors, Initialisation
        public REFShipViaFactory(GEnum.InstanceMode instanceMode)
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
        private bool Initialisation()
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

                            // Get Instance GUID           
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

                            // Commit Process      
                            this._REFShipVia = new REFShipVia();
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
            BOLib.REFShipVia copyREFShipVia = null;
            #endregion

            try
            {
                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                {
                    #region Make backup of objects for restore purpose
                    if (this._REFShipVia != null)
                        copyREFShipVia = this._REFShipVia.Clone();
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
                            this._REFShipVia = REFShipVia.New();

                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                            //Set Flags
                            this._isDirty = false;
                            this._isNew = true;
                            this._isReadOnly = false;

                            //Attach Events
                            this._REFShipVia.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Obj_PropertyChanged);
                            #endregion
                        }
                    }
                    restoreFlag = false;
                    return true;
                }
                else
                {
                    MsgBox.Show(MsgID.Common.WrongInstanceMode);
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
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._REFShipVia = copyREFShipVia;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFShipVia = null;
                #endregion
            }
        }//Completed
        public bool GetEdit(int? ShipViaKey)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.REFShipVia copyREFShipVia = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFShipVia != null)
                    copyREFShipVia = this._REFShipVia.Clone();
                #endregion

                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                {
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
                            if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, ShipViaKey, 0, _guID))
                                return false;

                            //Remove Lock
                            if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey) == false)
                                return false;

                            //Add Lock
                            if (SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, ShipViaKey) == false)
                                return false;

                            //Get Record                                 
                            if (this._REFShipVia.Fetch(cn, new REFShipVia.Criteria(ShipViaKey, 1)) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.GetFail);
                                return false;
                            }

                            //Record Not Found
                            if (GFunc.NEInt(this._REFShipVia._shipViaKey, 0) == 0)
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
                else
                {
                    MsgBox.Show(MsgID.Common.WrongInstanceMode);
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
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._REFShipVia = copyREFShipVia;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFShipVia = null;
                #endregion
            }
        }//Completed
        public bool GetReadOnly(int? ShipViaKey)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.REFShipVia copyREFShipVia = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFShipVia != null)
                    copyREFShipVia = this._REFShipVia.Clone();
                #endregion

                if (this.InstanceMode == GEnum.InstanceMode.Normal)
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

                            //Turn on restore flag to restore objects if any error occurs
                            restoreFlag = true;

                            //Remove all locks by GUID except inprogress Locking
                            if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey) == false)
                                return false;

                            //Get record
                            if (this._REFShipVia.Fetch(cn, new REFShipVia.Criteria(ShipViaKey, 1)) == false)
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
                else
                {
                    MsgBox.Show(MsgID.Common.WrongInstanceMode);
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
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._REFShipVia = copyREFShipVia;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFShipVia = null;
                #endregion
            }
        }//Completed
        public bool SetReadOnlyData(DataTable dtHeader, DataSet dsDetail)
        {
            try
            {
                if (this.InstanceMode == GEnum.InstanceMode.Normal)
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

                            if (GFunc.IsNE(_REFShipVia))
                                _REFShipVia = REFShipVia.New();
                            GFunc.ConvertDataTableToObject(dtHeader, _REFShipVia);

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
                else
                {
                    MsgBox.Show(MsgID.Common.WrongInstanceMode);
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
            int? newShipViaKey = 0;
            string autoID = string.Empty;
            BOLib.REFShipVia copyREFShipVia = null;

            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFShipVia != null)
                    copyREFShipVia = this._REFShipVia.Clone();
                #endregion

                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                {
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
                            if (this.IsNew && GFunc.IsNE(_REFShipVia._shipViaID))
                            {
                                if (!SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _REFShipVia._shipViaDes))
                                    return false;

                                _REFShipVia._shipViaID = autoID;
                            }
                            #endregion

                            #region Set default value for fields that cannot be empty but can have a general default value
                            //Get Server Date and Time (sdt)
                            DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                            _REFShipVia._createDate = GFunc.NEDateTime(_REFShipVia.CreateDate, svrDateTime);
                            _REFShipVia._createUserKey = GFunc.NEInt(_REFShipVia.CreateUserKey, AppInfor.currentUserKey);
                            _REFShipVia._lastModifiedDate = svrDateTime;
                            _REFShipVia._lastModifiedUserKey = AppInfor.currentUserKey;
                            #endregion

                            #region Validation
                            if (Validation(cn) == false)
                                return false;
                            #endregion

                            #region Save Record
                            //Note: there is no delete permission for Sales Rep Payroll (only Read,Edit)
                            if (IsNew)
                            {
                                if (_REFShipVia.Insert(cn, out newShipViaKey) == false)
                                {
                                    MsgBox.Show(cn,MsgID.Common.SaveFail);
                                    return false;
                                }
                            }
                            else
                            {
                                if (_REFShipVia.Update(cn) == false)
                                {
                                    MsgBox.Show(cn, MsgID.Common.SaveFail);
                                    return false;
                                }
                            }
                            #endregion

                            #region For New Record perform: Locking, set new recordKey
                            if (IsNew)
                            {
                                if (SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newShipViaKey))
                                    _REFShipVia._shipViaKey = newShipViaKey;
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
                        SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _REFShipVia.ShipViaKey, _REFShipVia.ShipViaID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _REFShipVia });
                    else
                        SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _REFShipVia.ShipViaKey, _REFShipVia.ShipViaID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _REFShipVia });
                    #endregion
                    

                    restoreFlag = false;
                    return true;
                }
                else
                {
                    MsgBox.Show(MsgID.Common.WrongInstanceMode);
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
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._REFShipVia = copyREFShipVia;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFShipVia = null;
                #endregion
            }
        }//Completed
        public bool Delete()
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.REFShipVia copyREFShipVia = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFShipVia != null)
                    copyREFShipVia = this._REFShipVia.Clone();
                #endregion

                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                {
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
                            if (SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _REFShipVia._shipViaKey, GUID) == false)
                                return false;


                            //Check the record is used in other dependency tables
                            if (GFunc.CheckKeyDependantsExists(cn, "ShipViaKey", _REFShipVia._shipViaKey.Value, _REFShipVia._shipViaID))
                                return false;

                            //Delete Record
                            if (_REFShipVia.Delete(cn, new REFShipVia.Criteria(_REFShipVia._shipViaKey)) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.DeleteFail);
                                return false;
                            }

                            //Remove Lock
                            if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                                return false;

                            //Create New
                            this._REFShipVia = REFShipVia.New();

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
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyREFShipVia.ShipViaKey, copyREFShipVia.ShipViaID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyREFShipVia });

                    restoreFlag = false;
                    return true;
                }
                else
                {
                    MsgBox.Show(MsgID.Common.WrongInstanceMode);
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
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._REFShipVia = copyREFShipVia;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFShipVia = null;
                #endregion
            }
        }//Completed
        public bool Dispose()
        {
            try
            {
                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                {
                    if (SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0) == false)
                        return false;
                }
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
        public bool Validation(SqlConnection cn)
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
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFShipVia._shipViaKey, "ShipViaKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFShipVia._shipViaID, "ShipViaID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                }
                else
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, _REFShipVia._shipViaKey, "ShipViaKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFShipVia._shipViaID, "ShipViaID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFShipVia._shipViaDes, "ShipViaDes", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFShipVia.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFShipVia.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFShipVia.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                #endregion

                if (e.PropertyMessage.Count == 0)
                {
                    //StoreProcedure Validation
                    if (_REFShipVia.Validation(cn, new REFShipVia.Criteria(_REFShipVia._shipViaKey, _REFShipVia._shipViaID), this.IsNew))
                    {
                        return true;
                    }
                    else
                    {
                        e.PropertyMessage.Add("ShipViaID", SysMessageUtility.Get(cn, MsgID.Validation.DuplicateRecordID + "ShipViaID"));
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
        }//Completed

        //Error
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _REFShipVia }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _REFShipVia }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
    }
    /*[Serializable()]
    public class REFShipViaFactory : CommandBase
    {
        #region Member variables and constants

        private REFShipVia _REFShipVia = null;
        private REFShipVias _REFShipVias = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isOpenReadOnly = false;
        private int _guID = 0;

        // System Code Key for this Factory.
        private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Shipping_Mode;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        // Permission ID for this Factory.
        public const string constPermID = GVar.PermissionID.Shipping_Mode;

        // Error Event Declaration
        public GVar.ErrorEvent errorEvent = null;

        // ReadOnly Event Declaration
        public GVar.ReadOnlyEvent readonlyEvent = null;

        //  Custom Event Declaration
        public GVar.UINotifierEvent ShipViaNotifier = null;
        public GVar.UINotifierEvent clearErrorNotifier = null;

        #endregion // Member variables and constant

        #region Factory Properties

        public REFShipVia ObjREFShipVia
        {
            get
            {
                return this._REFShipVia;
            }
        }

        public REFShipVias ObjREFShipVias
        {
            get
            {
                return this._REFShipVias;
            }
        }

        internal GEnum.InstanceMode InstanceMode
        {
            get
            {
                return this._instanceMode;
            }
        }

        public string ErrorMessageID
        {
            get;
            set;
        }

        public bool IsDirty
        {
            get
            {
                return this._isDirty;
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

        #endregion // Constructors

        #region Constructors

        /// <summary>
        /// Default constructor for this Factory.
        /// </summary>
        public REFShipViaFactory(GEnum.InstanceMode instanceMode)
        {
            this.Initialisation(instanceMode);
        }

        #endregion // Constructors

        #region Initialisation Method

        public bool Initialisation(GEnum.InstanceMode instanceMode)
        {
            bool isInitialisation = false;
            string msgID = MsgID.Common.InitialisationFail;

            if (this.InstanceMode == GEnum.InstanceMode.Normal)
            {
                // Check Permission
                if (!SECPermUtility.Any(constPermID, out this._isOpenReadOnly, true))
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
                        this._instanceMode = GEnum.InstanceMode.Normal;
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
                this._instanceMode = GEnum.InstanceMode.InternalCall;
                this._isOpenReadOnly = false;
                msgID = string.Empty;
                isInitialisation = true;
            }
            return isInitialisation;
        }

        #endregion //Initialisation Method

        #region GetEdit Method

        public bool GetEdit(int ShipViaKey)
        {
            // Initialisation
            bool isGetEdit = false;
            string msgID = MsgID.Common.GetFail;

            // Copy original object
            BOLib.REFShipVia copyREFShipVia = this._REFShipVia.Clone();

            if (this.InstanceMode == GEnum.InstanceMode.Normal)
            {
                try
                {
                    // Check Permission
                    if (!SECPermUtility.Edit(constPermID, true))
                        return false;

                    // Create TransactionScope
                    using (TransactionScope scope = new TransactionScope())
                    {
                        // Create SqlConnection
                        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                        {
                            // Open Connection
                            cn.Open();

                            // Check Lock
                            if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, ShipViaKey, 0, _guID))
                                return false;

                            // Remove Lock
                            if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                                return false;

                            // Add Lock
                            if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, ShipViaKey))
                                return false;

                            if (_REFShipVia == null)
                            {
                                _REFShipVia = new REFShipVia();
                            }

                            // Get Record                                 
                            if (!this._REFShipVia.Fetch(cn, new REFShipVia.Criteria(ShipViaKey, 1)))
                            {
                                MsgBox.Show(cn, msgID);
                                return false;
                            }


                            //Record Not Found
                            if (GFunc.NEInt(this._REFShipVia._shipViaKey, 0) == 0)
                            {                                
                                throw new TAException(MsgID.Common.GetFail);
                            }


                            // Commit Process
                            this._REFShipVia.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ObjREFShipVia_PropertyChanged);

                            this._isDirty = false;
                            this._isNew = false;
                            this._isOpenReadOnly = false;
                            msgID = string.Empty;
                            isGetEdit = true;

                            // No errors - commit transaction
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                            // Set Null to Backup Objects
                            copyREFShipVia = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Restore data when error is occur                    
                    this._REFShipVia = copyREFShipVia;
                    throw (ex);
                }
            }
            else
            {
                MsgBox.Show(MsgID.Common.WrongInstanceMode);
                return false;
            }
            return isGetEdit;
        }

        #endregion //GetEdit Method

        #region GetReadOnly Method

        public bool GetReadOnly(int ShipViaKey)
        {
            bool isGetReadOnly = false;
            string msgID = MsgID.Common.GetFail;

            // Copy original object
            BOLib.REFShipVia copyREFShipVia = this._REFShipVia.Clone();

            if (this.InstanceMode == GEnum.InstanceMode.Normal)
            {
                try
                {
                    // Check Permission
                    if (!SECPermUtility.Read(constPermID, true))
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

                            // Call REFShipVia.Fetch
                            if (!this._REFShipVia.Fetch(cn, new REFShipVia.Criteria(ShipViaKey, 1)))
                            {
                                MsgBox.Show(msgID);
                                return false;
                            }

                            this._isDirty = false;
                            this._isNew = false;
                            this._isOpenReadOnly = true;
                            msgID = string.Empty;
                            isGetReadOnly = true;

                            // No errors - commit transaction
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                            // Set Null to Backup Objects
                            copyREFShipVia = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Restore data when error is occur                    
                    this._REFShipVia = copyREFShipVia;
                    throw (ex);
                }
            }
            else
            {
                MsgBox.Show(MsgID.Common.WrongInstanceMode);
                return false;
            }
            return isGetReadOnly;
        }

        #endregion //GetReadOnly Method

        #region New Method

        public bool New()
        {
            bool isNew = false;
            string msgID = MsgID.Common.NewFail;

            BOLib.REFShipVia copyREFShipVia = null;

            // Copy original object
            if (!GFunc.IsNE(this._REFShipVia))
                copyREFShipVia = this._REFShipVia.Clone();

            if (this.InstanceMode == GEnum.InstanceMode.Normal)
            {
                try
                {
                    // Check Security Permission 
                    if (!SECPermUtility.Any(constPermID, out this._isOpenReadOnly, true))
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
                            if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                                return false;

                            // Call New
                            this._REFShipVia = REFShipVia.New();

                            this._REFShipVia.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ObjREFShipVia_PropertyChanged);
                            this._isDirty = false;
                            this._isNew = true;
                            this._isOpenReadOnly = false;
                            msgID = string.Empty;
                            isNew = true;

                            // No errors - commit transaction
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                            // Set Null to Backup Objects
                            copyREFShipVia = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Restore data when error is occur                    
                    this._REFShipVia = copyREFShipVia;
                    throw (ex);
                }
            }
            else
            {
                MsgBox.Show(MsgID.Common.WrongInstanceMode);
                return false;
            }
            return isNew;
        }

        #endregion //New Method

        #region Save Method

        public bool Save()
        {
            bool isSave = false;
            string msgID = string.Empty;

            if (this.IsNew)
                msgID = MsgID.Common.AddFail;
            else
                msgID = MsgID.Common.UpdateFail;

            bool isNewRecord = this.IsNew;
            int? newShipViaKey = 0;
            string autoID = string.Empty;
            string recordID = string.Empty;
            bool isCommitTransFail = true;

            if (this.InstanceMode == GEnum.InstanceMode.Normal)
            {
                try
                {
                    if (this.IsOpenReadOnly)
                    {
                        msgID = MsgID.Common.RecordIsReadOnly;
                        MsgBox.Show(msgID);
                        return false;

                    }
                    else
                    {
                        if (isNewRecord)
                        {
                            if (!SECPermUtility.Add(constPermID, true))
                                return false;
                        }
                        else
                        {
                            if (!SECPermUtility.Edit(constPermID, true))
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
                            // Backup ID
                            recordID = this._REFShipVia._shipViaID;
                            // Get AutoID
                            if (isNewRecord && GFunc.IsNE(_REFShipVia._shipViaID))
                            {
                                if (!SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _REFShipVia._shipViaDes))
                                    return false;
                                _REFShipVia.ShipViaID = autoID;
                            }

                            #region Set Server DateTime If Create and Modified Date is null
                            //Get Server Date and Time (sdt)
                            DateTime svrDateTime = GFunc.GetSvrDateTime(cn);

                            _REFShipVia.CreateDate = GFunc.NEDateTime(_REFShipVia.CreateDate, svrDateTime);
                            _REFShipVia.CreateUserKey = GFunc.NEInt(_REFShipVia.CreateUserKey, AppInfor.currentUserKey);

                            _REFShipVia.LastModifiedDate = svrDateTime;
                            _REFShipVia.LastModifiedUserKey = AppInfor.currentUserKey;

                            #endregion

                            // Validation
                            if (!Validation(cn))
                                return false;

                            // Save Record

                            if (isNewRecord)
                            {
                                if (!_REFShipVia.Insert(cn, out newShipViaKey))
                                    return false;
                            }
                            else
                            {
                                if (!_REFShipVia.Update(cn))
                                    return false;
                            }

                            // Record Locking
                            if (isNewRecord)
                                if (!SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newShipViaKey))
                                    return false;

                            // Commit Process
                            if (isNewRecord)
                                _REFShipVia._shipViaKey = newShipViaKey;

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
                        SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, new object[] { _REFShipVia }));
                    else
                        SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, new object[] { _REFShipVia }));
                }
                catch (TAException tex)
                {
                    // Restore the auto generated ID
                    _REFShipVia._shipViaID = recordID;
                    throw tex;
                }
                catch (Exception ex)
                {
                    if (isNewRecord)
                    {
                        // Restore the auto generated ID
                        this._REFShipVia._shipViaID = recordID;
                    }
                    if (isCommitTransFail)
                        throw new TAException(MsgID.Validation.CommitTransFail);

                    throw (ex);
                }
            }
            else
            {
                MsgBox.Show(MsgID.Common.WrongInstanceMode);
                return false;
            }
            return isSave;
        }

        #endregion //Save Method

        #region Delete Method

        public bool Delete()
        {
            bool isDelete = false;
            string msgID = MsgID.Common.DeleteFail;

            if (this.InstanceMode == GEnum.InstanceMode.Normal)
            {
                // Checking
                if (this.IsOpenReadOnly)
                {
                    msgID = MsgID.Common.RecordIsReadOnly;
                    MsgBox.Show(msgID);
                    return false;
                }
                else
                {
                    if (!SECPermUtility.Delete(constPermID, true))
                        return false;
                }

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        // Check Security Permission 

                        // Check Use in Database
                        // NotReady

                        // Record Locking
                        if (!SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _REFShipVia._shipViaKey, GUID))
                            return false;

                        // Check the record is used in other dependency tables
                        if (GFunc.CheckKeyDependantsExists(cn, "ShipViaKey", _REFShipVia._shipViaKey.Value, _REFShipVia._shipViaID))
                            return false;

                        //Check for Option Table
                        if (GFunc.CheckKeyDependcyinOptionTable(cn, "Ship", _REFShipVia._shipViaKey.Value))
                            return false;

                        // Delete Record
                        if (!_REFShipVia.Delete(cn, new REFShipVia.Criteria(_REFShipVia._shipViaKey)))
                            return false;

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                            return false;

                        // Create New
                        _REFShipVia = REFShipVia.New();

                        this._isDirty = false;
                        this._isNew = true;
                        this._isOpenReadOnly = false;
                        msgID = string.Empty;
                        isDelete = true;

                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                    }// End of SqlConnection
                }// End of TransactionScope
                //Audit Log

                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, new object[] { _REFShipVia }));
            }
            else
            {
                MsgBox.Show(MsgID.Common.WrongInstanceMode);
                return false;
            }
            return isDelete;
        }

        #endregion //Delete Method

        #region Validation Method

        public bool Validation(SqlConnection cn)
        {
            bool isValidation = false;
            string msgID = MsgID.Common.ValidationFail;
            string msgValue = string.Empty;

            string processOK = GVar.gcPass;
            string errorMsgID = string.Empty;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());

            try
            {
                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                {
                    // Clear Error in UI
                    if (!GFunc.IsNE(this.clearErrorNotifier))
                        this.clearErrorNotifier.Invoke(this, e);

                    //MsgBox Error
                    if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFShipVia.ShipViaKey, "ShipViaKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn) != GVar.gcPass)
                    {
                        ErrorMessageID = errorMsgID;
                        return false;
                    }
                    if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFShipVia.CreateDate, "CreateDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn) != GVar.gcPass)
                    {
                        ErrorMessageID = errorMsgID;
                        return false;
                    }
                    if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFShipVia.CreateUserKey, "CreateUserKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, 0, int.MaxValue, e, cn) != GVar.gcPass)
                    {
                        ErrorMessageID = errorMsgID;
                        return false;
                    }
                    if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFShipVia.LastModifiedDate, "LastModifiedDate", GEnum.DataType.DateTime, GEnum.Require.No, null, GEnum.CompareOperator.NotEqual, null, null, null, e, cn) != GVar.gcPass)
                    {
                        ErrorMessageID = errorMsgID;
                        return false;
                    }
                    if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFShipVia.LastModifiedUserKey, "LastModifiedUserKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, 0, int.MaxValue, e, cn) != GVar.gcPass)
                    {
                        ErrorMessageID = errorMsgID;
                        return false;
                    }

                    //Error Provider                   
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFShipVia.ShipViaID, "ShipViaID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFShipVia.ShipViaDes, "ShipViaDes", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFShipVia.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFShipVia.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFShipVia.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);

                    if (e.PropertyMessage.Count > 0)
                    {
                        isValidation = false;

                        ErrorMessageID = MsgID.Common.ValidationFail;

                        if (!GFunc.IsNE(this.ShipViaNotifier))
                            this.ShipViaNotifier.Invoke(this, e);
                        return false;
                    }
                    else
                        isValidation = true;

                    // StoreProcedure Validation
                    if (e.PropertyMessage.Count == 0)
                    {
                        if (this._REFShipVia.Validation(cn, new REFShipVia.Criteria(this._REFShipVia._shipViaKey, this._REFShipVia._shipViaID), this.IsNew))
                        {
                            msgID = string.Empty;
                        }
                        else
                        {
                            ErrorMessageID = MsgID.Validation.DuplicateRecordID + "ShipViaID";
                            e.PropertyMessage.Add("ShipViaID", SysMessageUtility.Get(cn, ErrorMessageID));
                            if (!GFunc.IsNE(this.ShipViaNotifier))
                                this.ShipViaNotifier.Invoke(this, e);
                            return false;
                        }
                    }
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

            return isValidation;
        }

        #endregion //Validation Method

        #region Dispose Method

        public bool Dispose()
        {
            bool isDispose = false;
            string msgID = string.Empty;

            if (this.InstanceMode == GEnum.InstanceMode.Normal)
                if (!SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0))
                    return false;

            isDispose = true;

            return isDispose;
        }

        #endregion //Dispose Method

        #region PropertyChanged

        private void ObjREFShipVia_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            string msgID = string.Empty;
            bool validateOk = true;

            if (!this._isOpenReadOnly)
            {
                this._isDirty = true;

                //UI Validation
                switch (e.PropertyName)
                {
                    case "ShipViaID":
                        if (IsNew)
                            validateOk = BaseUtility.Validation(out msgID, _REFShipVia._shipViaID, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null);
                        else
                            validateOk = BaseUtility.Validation(out msgID, _REFShipVia._shipViaID, e.PropertyName, GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null);
                        break;
                    case "ShipViaDes":
                        validateOk = BaseUtility.Validation(out msgID, _REFShipVia._shipViaDes, e.PropertyName, GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null);
                        break;
                    case "Custom1":
                        validateOk = BaseUtility.Validation(out msgID, _REFShipVia._custom1, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);
                        break;
                    case "Custom2":
                        validateOk = BaseUtility.Validation(out msgID, _REFShipVia._custom2, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);
                        break;
                    case "Custom3":
                        validateOk = BaseUtility.Validation(out msgID, _REFShipVia._custom3, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);
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

        #region Error

        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _REFShipVia }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _REFShipVia }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        #endregion
    }*/
}
