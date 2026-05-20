

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

namespace BOLib
{
    [Serializable()]
    public class MSTShipNameFactory : CommandBase
    {
        #region Member variables and constants

        private MSTShipName _MSTShipName = null;
        private MSTShipNameDetItms _MSTShipNameDetItms = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isReadOnly = false;
        private int _guID = 0;

        // System Code Key for this Factory.
        private GEnum.SystemCode constCodeKey = GEnum.SystemCode.Ship_Name;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        // Permission ID for this Factory.
        private string constPermID = GVar.PermissionID.Ship_Name;
        public string PermID { get { return constPermID; } }

        // Custom Event Declaration 
        public GVar.DirtyEvent dirtyEvent = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;
        public GVar.UINotifierEvent shipNameListNotifier = null;

        #endregion // Member variables and constant

        #region Factory Properties

        public MSTShipName ObjMSTShipName
        {
            get
            {
                return this._MSTShipName;
            }
            set
            {
                this._MSTShipName = value;
            }
        }
        public MSTShipNameDetItms ObjMSTShipNameDetItms
        {
            get
            {
                return this._MSTShipNameDetItms;
            }
            set
            {
                this._MSTShipNameDetItms = value;
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
        public MSTShipNameFactory(GEnum.InstanceMode instanceMode)
        {
            try
            {
                this._instanceMode = instanceMode;
                this.Initialisation();
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
                    if (!SECPermUtility.Any(constPermID, out this._isReadOnly, true))
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
                            this._MSTShipName = new MSTShipName();
                            this._MSTShipNameDetItms = new MSTShipNameDetItms(cn);

                            this._isNew = false;
                            this._isReadOnly = false;
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        }
                    }
                }
                else if (this.InstanceMode == GEnum.InstanceMode.InternalCall)
                {
                    if (!SECPermUtility.Any(constPermID, out this._isReadOnly, true))
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
                            this._MSTShipName = new MSTShipName();
                            this._MSTShipNameDetItms = new MSTShipNameDetItms(cn);

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

        //Methods
        public bool New()
        {
            #region Declaration
            bool restoreFlag = false;
            MSTShipName copyMSTShipName = null;
            MSTShipNameDetItms copyMSTShipNameDetItms = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._MSTShipName != null)
                    copyMSTShipName = this._MSTShipName.Clone();

                if (this._MSTShipNameDetItms != null)
                    copyMSTShipNameDetItms = GFunc.TACopyDataTable(_MSTShipNameDetItms);

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
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                            return false;

                        //prepare new instance 
                        this._MSTShipName = MSTShipName.New();
                        this._MSTShipNameDetItms = new MSTShipNameDetItms(cn);

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        #endregion

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;

                        //Attach Events
                        this._MSTShipName.PropertyChanged += new PropertyChangedEventHandler(Obj_PropertyChanged);
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
                    this._MSTShipName = copyMSTShipName;
                    this._MSTShipNameDetItms = copyMSTShipNameDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTShipName = null;
                copyMSTShipNameDetItms = null;
                #endregion
            }
        }//Completed
        public bool GetEdit(int ShipNameKey)
        {
            #region Declaration
            bool restoreFlag = false;
            MSTShipName copyMSTShipName = null;
            MSTShipNameDetItms copyMSTShipNameDetItms = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._MSTShipName != null)
                    copyMSTShipName = this._MSTShipName.Clone();

                if (this._MSTShipNameDetItms != null)
                    copyMSTShipNameDetItms = GFunc.TACopyDataTable(_MSTShipNameDetItms);


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

                        // Check Lock
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, ShipNameKey, 0, _guID))
                            return false;

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                            return false;

                        // Add Lock
                        if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, ShipNameKey))
                            return false;

                        #region Get Record
                        if (_MSTShipName.Fetch(cn, new MSTShipName.Criteria(ShipNameKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }


                        //Record Not Found
                        if (GFunc.NEInt(this._MSTShipName.ShipNameKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }

                        _MSTShipNameDetItms.Clear();
                        if (_MSTShipNameDetItms.Fetch(cn, new MSTShipNameDetItms.Criteria(ShipNameKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        #endregion

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        this._isReadOnly = false;

                        //Attach Events
                        this._MSTShipName.PropertyChanged += new PropertyChangedEventHandler(Obj_PropertyChanged);
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
                    this._MSTShipName = copyMSTShipName;
                    this._MSTShipNameDetItms = copyMSTShipNameDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTShipName = null;
                copyMSTShipNameDetItms = null;
                #endregion
            }
        }//Completed
        public bool GetReadOnly(int ShipNameKey)
        {
            #region Declaration
            bool restoreFlag = false;
            MSTShipName copyMSTShipName = null;
            MSTShipNameDetItms copyMSTShipNameDetItms = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._MSTShipName != null)
                    copyMSTShipName = this._MSTShipName.Clone();

                if (this._MSTShipNameDetItms != null)
                    copyMSTShipNameDetItms = GFunc.TACopyDataTable(_MSTShipNameDetItms);


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

                        // Remove all locks by GUID except inprogress Locking
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey) == false)
                            return false;

                        #region Get Data
                        if (_MSTShipName.Fetch(cn, new MSTShipName.Criteria(ShipNameKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _MSTShipNameDetItms.Clear();
                        if (_MSTShipNameDetItms.Fetch(cn, new MSTShipNameDetItms.Criteria(ShipNameKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }
                        #endregion

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
                    this._MSTShipName = copyMSTShipName;
                    this._MSTShipNameDetItms = copyMSTShipNameDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTShipName = null;
                copyMSTShipNameDetItms = null;
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

                        // Remove all locks by GUID except inprogress Locking
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                            return false;

                        if (GFunc.IsNE(_MSTShipName))
                            _MSTShipName = MSTShipName.New();
                        GFunc.ConvertDataTableToObject(dtHeader, _MSTShipName);

                        _MSTShipNameDetItms = new MSTShipNameDetItms(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[0], _MSTShipNameDetItms);

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
            //So for example : Item Location, the above will not work
            #region Declaration
            bool restoreFlag = false;
            bool isNewRecord = this.IsNew;//Because this flag will be changed during saving, we need to know the original value when error occurs
            int? newRecordKey = 0;
            string autoID = string.Empty;
            string msgID = string.Empty;
            MSTShipName copyMSTShipName = null;
            MSTShipNameDetItms copyMSTShipNameDetItms = null;

            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._MSTShipName != null)
                    copyMSTShipName = this._MSTShipName.Clone();

                if (this._MSTShipNameDetItms != null)
                    copyMSTShipNameDetItms = GFunc.TACopyDataTable(_MSTShipNameDetItms);


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

                        #region Set default value for fields that cannot be empty but can have a general default value

                        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);//Get Server Date and Time (sdt)
                        _MSTShipName._createDate = GFunc.NEDateTime(_MSTShipName.CreateDate, svrDateTime);
                        _MSTShipName._createUserKey = GFunc.NEInt(_MSTShipName.CreateUserKey, AppInfor.currentUserKey);
                        _MSTShipName._lastModifiedDate = svrDateTime;
                        _MSTShipName._lastModifiedUserKey = AppInfor.currentUserKey;

                        //_MSTShipNameDetItms
                        foreach (DataRow dr in _MSTShipNameDetItms.Rows)
                        {
                            dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
                            dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
                            dr["LastModifiedDate"] = svrDateTime;
                            dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
                        }

                        #endregion

                        #region Validation
                        if (Validation_Header(cn) == false)
                            return false;

                        if (Validation_Detail("tagrdShipNameList", (DataTable)this.ObjMSTShipNameDetItms, cn) == false)
                            return false;

                        #endregion

                        #region Save Record

                        if (IsNew)
                        {
                            if (!_MSTShipName.Insert(cn, out newRecordKey))
                                return false;

                            //Update new JobKey to details tables
                            foreach (DataRow row in _MSTShipNameDetItms.Rows)
                            {
                                row["ShipNameKey"] = newRecordKey;

                            }
                            _MSTShipNameDetItms.AcceptChanges();

                            if (!_MSTShipNameDetItms.Insert(cn, newRecordKey))
                                return false;
                        }
                        else
                        {
                            if (!_MSTShipName.Update(cn))
                                return false;

                            if (!_MSTShipNameDetItms.Delete(cn, new MSTShipNameDetItms.Criteria(_MSTShipName.ShipNameKey, 0)))
                                return false;
                            if (!_MSTShipNameDetItms.Insert(cn, _MSTShipName.ShipNameKey))
                                return false;
                        }
                        #endregion

                        #region For New Record perform: Locking, set new recordKey
                        if (IsNew)
                        {
                            if (SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newRecordKey))
                                _MSTShipName.ShipNameKey = (int)newRecordKey;
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
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _MSTShipName.ShipNameKey, _MSTShipName.ShipName, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTShipName, _MSTShipNameDetItms });
                else
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _MSTShipName.ShipNameKey, _MSTShipName.ShipName, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTShipName, _MSTShipNameDetItms });
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
                    this._MSTShipName = copyMSTShipName;
                    this._MSTShipNameDetItms = copyMSTShipNameDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTShipName = null;
                copyMSTShipNameDetItms = null;
                #endregion
            }
        }//Completed
        public bool Delete()
        {
            #region Declaration
            bool restoreFlag = false;
            MSTShipName copyMSTShipName = null;
            MSTShipNameDetItms copyMSTShipNameDetItms = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._MSTShipName != null)
                    copyMSTShipName = this._MSTShipName.Clone();

                if (this._MSTShipNameDetItms != null)
                    copyMSTShipNameDetItms = GFunc.TACopyDataTable(_MSTShipNameDetItms);

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
                        if (SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _MSTShipName.ShipNameKey, GUID) == false)
                            return false;

                        //Check the record is used in other dependency tables
                        if (GFunc.CheckKeyDependantsExists(cn, "ShipNameKey", _MSTShipName.ShipNameKey, _MSTShipName.ShipName))
                            return false;

                        //Delete Record
                        if (_MSTShipName.Delete(cn, new MSTShipName.Criteria(_MSTShipName.ShipNameKey)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.DeleteFail);
                            return false;
                        }

                        // Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey) == false)
                            return false;

                        // Create New
                        this._MSTShipName = MSTShipName.New();
                        this._MSTShipNameDetItms = new MSTShipNameDetItms(cn);

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        #region Set Flag
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;
                        #endregion

                        #endregion
                    }
                }

                // AuditLog
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyMSTShipName.ShipNameKey, copyMSTShipName.ShipName, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyMSTShipName, copyMSTShipNameDetItms });

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
                    this._MSTShipName = copyMSTShipName;
                    this._MSTShipNameDetItms = copyMSTShipNameDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTShipName = null;
                copyMSTShipNameDetItms = null;
                #endregion
            }
        }//Completed
        public bool Dispose()//Completed
        {
            try
            {
                string msgID = string.Empty;
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

        //Validation
        private bool Validation_Header(SqlConnection cn)
        {
            #region Declaration
            string processOK = GVar.gcPass;
            string errorMsgID = string.Empty;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
            #endregion

            try
            {
                //Clear Error in UI
                if (GFunc.IsNE(this.ErrorNotifierHeader_Clear) == false)
                    this.ErrorNotifierHeader_Clear.Invoke(this, e);

                #region Validate Item Key and ID for New Record or existing record
                if (this.IsNew)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTShipName.ShipNameKey, "ShipNameKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                }
                else
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTShipName.ShipNameKey, "ShipNameKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                }
                #endregion

                #region Validation Process
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTShipName.ShipName, "ShipName", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTShipName.ConKey, "ConKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTShipName.BillName, "BillName", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTShipName.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTShipName.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTShipName.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTShipName.Custom4, "Custom4", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTShipName.Custom5, "Custom5", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                #endregion

                #region Check for Duplicate ShipNm and ConKey
                if (e.PropertyMessage.Count == 0)
                {
                    bool DuplicateID = _MSTShipName.Validation(cn, new MSTShipName.Criteria(_MSTShipName.ShipNameKey, _MSTShipName.ShipName,_MSTShipName.ConKey), this.IsNew);

                    if (!DuplicateID && !GFunc.IsNE(this.ErrorNotifierHeader_Set))
                    {
                        errorMsgID = "Duplicate shipName with the same customer";
                        e.PropertyMessage.Add("ShipName", SysMessageUtility.Get(cn, errorMsgID));
                        e.PropertyMessage.Add("ConKey", SysMessageUtility.Get(cn, errorMsgID));
                    }
                }
                #endregion

                #region Invoke Notifier
                if (e.PropertyMessage.Count > 0)
                {
                    if (!GFunc.IsNE(this.ErrorNotifierHeader_Set))
                        this.ErrorNotifierHeader_Set.Invoke(this, e);

                    return false;
                }
                else
                    return true;
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
        }//Completed
        private bool Validation_Detail(string grdNm, DataTable dt, SqlConnection cn)
        {
            //Validation Check for calls from Factory (Save method)
            string msgID = string.Empty;
            bool processOK = true;
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    msgID = string.Empty;
                    processOK = true;

                    if (dr.RowState == DataRowState.Deleted)
                        continue;
                    else
                    {
                        //Check Column values
                        UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
                        foreach (DataColumn c in dr.Table.Columns)
                        {
                            Validation_DetailCheck(dr, grdNm, dr[c.ColumnName.ToString()], c.ColumnName.ToString(), false, ref processOK, e);
                        }

                        //Check for Duplicate records
                        if (processOK)
                        {
                            Validation_DetailRelation(grdNm, dr["ShipMark"], false, ref processOK, e);
                        }

                        //Set RowError Text
                        if (processOK == false)
                        {
                            dr.RowError = GFunc.PropertyMessage_Merge(e, cn);
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
        public bool Validation_Detail(string grdNm, UltraGridRow grdrow, string fieldToCheck)
        {
            //Validation Check for calls from FORM (CustomCellUpdate, BeforeRowUpdate)
            string msgID = string.Empty;
            bool processOK = true;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());

            try
            {
                DataRow drow = ((DataTable)grdrow.Band.Layout.Grid.DataSource).DefaultView[grdrow.Index].Row;
                //Check Column values
                if (fieldToCheck == string.Empty)
                {
                    foreach (UltraGridCell c in grdrow.Cells)
                    {
                        Validation_DetailCheck(drow, grdNm, c.Value, c.Column.Key, false, ref processOK, e);
                    }
                }
                else
                    Validation_DetailCheck(drow, grdNm, grdrow.Cells[fieldToCheck].Value, fieldToCheck, false, ref processOK, e);

                //Check for Duplicate records when fieldToCheck is Empty (meaning RowBeforeUpdate)
                if (processOK && fieldToCheck == string.Empty)
                {
                    Validation_DetailRelation(grdNm, grdrow.Cells["ShipMark"].Value, grdrow.IsAddRow, ref processOK, e);
                }

                //Set RowError Text
                if (processOK == false)
                {
                    ((DataRowView)(grdrow.ListObject)).Row.RowError = GFunc.PropertyMessage_Merge(e);
                    throw new TAException(BOLib.MsgID.Common.ValidationFail);
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
        public bool Validation_DetailCheck(DataRow drow, string grdNm, object propValue, string CheckNm, bool failonError, ref bool processOK, UINotifierEventArgs e)
        {
            try
            {
                switch (grdNm)
                {
                    #region tagrdShipNameList Validation
                    case "tagrdShipNameList":
                        BaseUtility.Validation(propValue, "ShipMark", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom1", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom2", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom3", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                        break;
                    #endregion
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
            
            return processOK;
        }//Completed
        public bool Validation_DetailRelation(string grdNm, object propValue, bool IsAddRow, ref bool processOK, UINotifierEventArgs e)
        {
            bool errorFound = false;
            try
            {
                switch (grdNm)
                {
                    #region tagrdShipNameList
                    case "tagrdShipNameList":
                        var dupAss = ObjMSTShipNameDetItms.AsEnumerable().ToList().FindAll(o => (o.Field<int>("ShipMark") == int.Parse(propValue.ToString())));

                        if (IsAddRow)
                        {
                            if (dupAss.Count > 0)
                                errorFound = true;
                        }
                        else
                        {
                            if (dupAss.Count > 1)
                                errorFound = true;
                        }
                        if (errorFound)
                        {
                            e.PropertyMessage.Add("rowError", "ShipMark" + MsgID.Validation.DuplicateRecord);
                            processOK = false;
                        }
                        break;
                    #endregion

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
            
            return processOK;
        }//Completed

        //Attached Events
        private void Obj_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (this._isReadOnly == false)
            {
                if (this.dirtyEvent != null)
                    this.dirtyEvent.Invoke(this, e);

                _isDirty = true;
            }
        }//Completed

        //Error
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _MSTShipName, _MSTShipNameDetItms }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _MSTShipName, _MSTShipNameDetItms }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
    }
}
