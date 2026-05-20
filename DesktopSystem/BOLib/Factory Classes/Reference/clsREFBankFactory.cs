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
    public class REFBankFactory : CommandBase
    {
        #region Member variables and constants

        private REFBank _REFBank = null;    
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isReadOnly = false;
        private int _guID = 0;

        //System Code Key for this Factory.
        private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Bank;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        //Permission ID for this Factory.
        private const string constPermID = GVar.PermissionID.Bank;
        public string PermID { get { return constPermID; } }

        //Event Declaration
        public GVar.DirtyEvent dirtyEvent = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;

        #endregion

        #region Factory Properties
        public REFBank ObjREFBank
        {
            get
            {
                return this._REFBank;
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
        public REFBankFactory(GEnum.InstanceMode instanceMode)
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
                            this._REFBank = new REFBank();
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
            BOLib.REFBank copyREFBank = null;
            #endregion

            try
            {
                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                {
                    #region Make backup of objects for restore purpose
                    if (this._REFBank != null)
                        copyREFBank = this._REFBank.Clone();
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
                            this._REFBank = REFBank.New();

                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                            //Set Flags
                            this._isDirty = false;
                            this._isNew = true;
                            this._isReadOnly = false;

                            //Attach Events
                            this._REFBank.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Obj_PropertyChanged);
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
                    this._REFBank = copyREFBank;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFBank = null;
                #endregion
            }
        }//Completed
        public bool GetEdit(int? BankKey)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.REFBank copyREFBank = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFBank != null)
                    copyREFBank = this._REFBank.Clone();
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
                            if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, BankKey, 0, _guID))
                                return false;

                            //Remove Lock
                            if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey) == false)
                                return false;

                            //Add Lock
                            if (SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, BankKey) == false)
                                return false;

                            //Get Record                                 
                            if (this._REFBank.Fetch(cn, new REFBank.Criteria(BankKey, 1)) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.GetFail);
                                return false;
                            }

                            //Record Not Found
                            if (GFunc.NEInt(this._REFBank._bankKey, 0) == 0)
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
                    this._REFBank = copyREFBank;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFBank = null;
                #endregion
            }
        }//Completed
        public bool GetReadOnly(int? BankKey)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.REFBank copyREFBank = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFBank != null)
                    copyREFBank = this._REFBank.Clone();
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
                            if (this._REFBank.Fetch(cn, new REFBank.Criteria(BankKey, 1)) == false)
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
                    this._REFBank = copyREFBank;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFBank = null;
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

                            if (GFunc.IsNE(_REFBank))
                                _REFBank = REFBank.New();
                            GFunc.ConvertDataTableToObject(dtHeader, _REFBank);

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
            int? newBankKey = 0;
            string autoID = string.Empty;
            BOLib.REFBank copyREFBank = null;

            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFBank != null)
                    copyREFBank = this._REFBank.Clone();
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
                            if (this.IsNew && GFunc.IsNE(_REFBank._bankID))
                            {
                                if (!SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _REFBank._bankDes))
                                    return false;

                                _REFBank._bankID = autoID;
                            }
                            #endregion

                            #region Set default value for fields that cannot be empty but can have a general default value
                            //Get Server Date and Time (sdt)
                            DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                            _REFBank._createDate = GFunc.NEDateTime(_REFBank.CreateDate, svrDateTime);
                            _REFBank._createUserKey = GFunc.NEInt(_REFBank.CreateUserKey, AppInfor.currentUserKey);
                            _REFBank._lastModifiedDate = svrDateTime;
                            _REFBank._lastModifiedUserKey = AppInfor.currentUserKey;
                            #endregion

                            #region Validation
                            if (Validation(cn) == false)
                                return false;
                            #endregion

                            #region Save Record
                            //Note: there is no delete permission for Sales Rep Payroll (only Read,Edit)
                            if (IsNew)
                            {
                                if (_REFBank.Insert(cn, out newBankKey) == false)
                                {
                                    MsgBox.Show(cn,MsgID.Common.SaveFail);
                                    return false;
                                }
                            }
                            else
                            {
                                if (_REFBank.Update(cn) == false)
                                {
                                    MsgBox.Show(cn, MsgID.Common.SaveFail);
                                    return false;
                                }
                            }
                            #endregion

                            #region For New Record perform: Locking, set new recordKey
                            if (IsNew)
                            {
                                if (SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newBankKey))
                                    _REFBank._bankKey = newBankKey;
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
                        SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _REFBank._bankKey, _REFBank._bankID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _REFBank });
                    else
                        SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _REFBank._bankKey, _REFBank._bankID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _REFBank });
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
                    this._REFBank = copyREFBank;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFBank = null;
                #endregion
            }
        }//Completed
        public bool Delete()
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.REFBank copyREFBank = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFBank != null)
                    copyREFBank = this._REFBank.Clone();
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
                            if (SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _REFBank._bankKey, GUID) == false)
                                return false;


                            //Check the record is used in other dependency tables
                            if (GFunc.CheckKeyDependantsExists(cn, "BankKey", _REFBank._bankKey.Value, _REFBank._bankID))
                                return false;

                            //Delete Record
                            if (_REFBank.Delete(cn, new REFBank.Criteria(_REFBank._bankKey)) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.DeleteFail);
                                return false;
                            }

                            //Remove Lock
                            if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                                return false;

                            //Create New
                            this._REFBank = REFBank.New();

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
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyREFBank._bankKey, copyREFBank._bankID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyREFBank });

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
                    this._REFBank = copyREFBank;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFBank = null;
                #endregion
            }
        }//Completed
        public bool Dispose()
        {
            if (this.InstanceMode == GEnum.InstanceMode.Normal)
            {
                if (SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0) == false)
                    return false;
            }
            return true;
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
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBank._bankKey, "BankKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBank._bankID, "BankID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                }
                else
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, _REFBank._bankKey, "BankKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBank._bankID, "BankID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBank._bankDes, "BankDes", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBank.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBank.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBank.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                #endregion

                if (e.PropertyMessage.Count == 0)
                {
                    //StoreProcedure Validation
                    if (_REFBank.Validation(cn, new REFBank.Criteria(_REFBank._bankKey, _REFBank._bankID), this.IsNew))
                    {
                        return true;
                    }
                    else
                    {
                        e.PropertyMessage.Add("BankID", SysMessageUtility.Get(cn, MsgID.Validation.DuplicateRecordID + "BankID"));
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
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _REFBank }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _REFBank }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
    }
}
