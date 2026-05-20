using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Infragistics.Win.UltraWinGrid;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class MSTJobFactory : CommandBase
    {
        #region Member variables and constants

        private MSTJob _MSTJob = null;
        private MSTJobDetEsts _MSTJobDetEsts = null;
        private MSTJobDetOthers _MSTJobDetOthers = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isReadOnly = false;
        private int _guID = 0;

        //System Code Key for this Factory.
        private GEnum.SystemCode constCodeKey = GEnum.SystemCode.Job;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        //Permission ID for this Factory.
        private string constPermID = GVar.PermissionID.Job;
        public string PermID { get { return constPermID; } }

        //Event Declaration
        public GVar.DirtyEvent dirtyEvent = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;
        #endregion // Member variables and constant

        #region Factory Properties

        public MSTJob ObjMSTJob
        {
            get
            {
                return this._MSTJob;
            }
        }
        public MSTJobDetEsts ObjMSTJobDetEsts
        {
            get
            {
                return this._MSTJobDetEsts;
            }
        }
        public MSTJobDetOthers ObjMSTJobDetOthers
        {
            get
            {
                return this._MSTJobDetOthers;
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
        public MSTJobFactory(GEnum.InstanceMode instanceMode)
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
                            this._MSTJob = new MSTJob();
                            this._MSTJobDetEsts = new MSTJobDetEsts(cn);
                            this._MSTJobDetOthers = new MSTJobDetOthers(cn);

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
                            this._MSTJob = new MSTJob();
                            this._MSTJobDetEsts = new MSTJobDetEsts(cn);
                            this._MSTJobDetOthers = new MSTJobDetOthers(cn);

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
            MSTJob copyMSTJob = null;
            MSTJobDetEsts copyMSTJobDetEsts = null;
            MSTJobDetOthers copyMSTJobDetOthers = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._MSTJob != null)
                    copyMSTJob = this._MSTJob.Clone();

                if (this._MSTJobDetEsts != null)
                    copyMSTJobDetEsts = GFunc.TACopyDataTable(_MSTJobDetEsts);

                if (this._MSTJobDetOthers != null)
                    copyMSTJobDetOthers = GFunc.TACopyDataTable(_MSTJobDetOthers);

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
                        this._MSTJob = MSTJob.New();
                        this._MSTJob.Attachments = new SYSAttachments();
                        this._MSTJobDetEsts = new MSTJobDetEsts(cn);
                        this._MSTJobDetOthers = new MSTJobDetOthers(cn);

                        //Set Default Value
                        SetDefaultValue();

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        #endregion

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;

                        //Attach Events
                        this._MSTJob.PropertyChanged += new PropertyChangedEventHandler(Obj_PropertyChanged);
                        this._MSTJob.Attachments.ListChanged += new ListChangedEventHandler(Attachments_ListChanged);
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
                    this._MSTJob = copyMSTJob;
                    this._MSTJobDetEsts = copyMSTJobDetEsts;
                    this._MSTJobDetOthers = copyMSTJobDetOthers;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTJob = null;
                copyMSTJobDetEsts = null;
                copyMSTJobDetOthers = null;
                #endregion
            }
        }//Completed
        public bool GetEdit(int? jobKey, string jobID)
        {
            #region Declaration
            bool restoreFlag = false;
            MSTJob copyMSTJob = null;
            MSTJobDetEsts copyMSTJobDetEsts = null;
            MSTJobDetOthers copyMSTJobDetOthers = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._MSTJob != null)
                    copyMSTJob = this._MSTJob.Clone();

                if (this._MSTJobDetEsts != null)
                    copyMSTJobDetEsts = GFunc.TACopyDataTable(_MSTJobDetEsts);

                if (this._MSTJobDetOthers != null)
                    copyMSTJobDetOthers = GFunc.TACopyDataTable(_MSTJobDetOthers);

                #endregion

                #region Check Security Permission
                if (SECPermUtility.Edit(constPermID, true) == false)
                    return false;
                #endregion

                #region Get JobKey to open record and check RecordAccess rights
                if (jobID != null && jobID != string.Empty)
                    jobKey = MSTJob.Get(jobID).JobKey;

                if (jobKey == 0)
                    return false;

                if (_MSTJob.CanAccessRecord(jobKey) == false)
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
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, jobKey, 0, _guID))
                            return false;

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                            return false;

                        // Add Lock
                        if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, jobKey))
                            return false;

                        #region Get Record
                        if (_MSTJob.Fetch(cn, new MSTJob.Criteria(jobKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }


                        //Record Not Found
                        if (GFunc.NEInt(this._MSTJob._jobKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }

                        _MSTJobDetEsts.Clear();
                        if (_MSTJobDetEsts.Fetch(cn, new MSTJobDetEsts.Criteria(jobKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _MSTJobDetOthers.Clear();
                        if (_MSTJobDetOthers.Fetch(cn, new MSTJobDetOthers.Criteria(jobKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        this._MSTJob.Attachments.Fetch(cn, new SYSAttachments.Criteria((int)this.constCodeKey, this._MSTJob.JobKey, 1));
                        #endregion

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        this._isReadOnly = false;

                        //Attach Events
                        this._MSTJob.PropertyChanged += new PropertyChangedEventHandler(Obj_PropertyChanged);
                        this._MSTJob.Attachments.ListChanged += new ListChangedEventHandler(Attachments_ListChanged);
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
                    this._MSTJob = copyMSTJob;
                    this._MSTJobDetEsts = copyMSTJobDetEsts;
                    this._MSTJobDetOthers = copyMSTJobDetOthers;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTJob = null;
                copyMSTJobDetEsts = null;
                copyMSTJobDetOthers = null;
                #endregion
            }
        }//Completed
        public bool GetReadOnly(int? jobKey, string jobID)
        {
            #region Declaration
            bool restoreFlag = false;
            MSTJob copyMSTJob = null;
            MSTJobDetEsts copyMSTJobDetEsts = null;
            MSTJobDetOthers copyMSTJobDetOthers = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._MSTJob != null)
                    copyMSTJob = this._MSTJob.Clone();

                if (this._MSTJobDetEsts != null)
                    copyMSTJobDetEsts = GFunc.TACopyDataTable(_MSTJobDetEsts);

                if (this._MSTJobDetOthers != null)
                    copyMSTJobDetOthers = GFunc.TACopyDataTable(_MSTJobDetOthers);
                #endregion

                #region Check Security Permission
                if (SECPermUtility.Read(constPermID, true) == false)
                    return false;
                #endregion

                #region Get JobKey to open record and check RecordAccess rights
                if (jobID != null && jobID != string.Empty)
                    jobKey = MSTJob.Get(jobID).JobKey;

                if (jobKey == 0)
                    return false;

                if (_MSTJob.CanAccessRecord(jobKey) == false)
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
                        if (_MSTJob.Fetch(cn, new MSTJob.Criteria(jobKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _MSTJobDetEsts.Clear();
                        if (_MSTJobDetEsts.Fetch(cn, new MSTJobDetEsts.Criteria(jobKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _MSTJobDetOthers.Clear();
                        if (_MSTJobDetOthers.Fetch(cn, new MSTJobDetOthers.Criteria(jobKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        this._MSTJob.Attachments.Fetch(cn, new SYSAttachments.Criteria((int)this.constCodeKey, this._MSTJob.JobKey, 1));
                        this._MSTJob.Attachments.ListChanged += new ListChangedEventHandler(Attachments_ListChanged);
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
                    this._MSTJob = copyMSTJob;
                    this._MSTJobDetEsts = copyMSTJobDetEsts;
                    this._MSTJobDetOthers = copyMSTJobDetOthers;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTJob = null;
                copyMSTJobDetEsts = null;
                copyMSTJobDetOthers = null;
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

                        if (GFunc.IsNE(_MSTJob))
                            _MSTJob = MSTJob.New();
                        GFunc.ConvertDataTableToObject(dtHeader, _MSTJob);

                        _MSTJobDetEsts = new MSTJobDetEsts(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[0], _MSTJobDetEsts);
                        _MSTJobDetOthers = new MSTJobDetOthers(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[1], _MSTJobDetOthers);

                        //GFunc.ConvertDataTableToObject(dsDetail.Tables[2], _MSTJob.Attachments);//Attachments saving part is not finished in LogUtility =>AddAuditLog=>GFunc.ConvertObjectToXML.
                       
                       
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
            int? newJobKey = 0;
            string autoID = string.Empty;
            string msgID = string.Empty;
            MSTJob copyMSTJob = null;
            MSTJobDetEsts copyMSTJobDetEsts = null;
            MSTJobDetOthers copyMSTJobDetOthers = null;

            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._MSTJob != null)
                    copyMSTJob = this._MSTJob.Clone();

                if (this._MSTJobDetEsts != null)
                    copyMSTJobDetEsts = GFunc.TACopyDataTable(_MSTJobDetEsts);

                if (this._MSTJobDetOthers != null)
                    copyMSTJobDetOthers = GFunc.TACopyDataTable(_MSTJobDetOthers);

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
                        if (this.IsNew && GFunc.IsNE(_MSTJob._jobID))
                        {
                            if (SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _MSTJob._jobDes) == false)
                                return false;

                            _MSTJob._jobID = autoID;
                        }
                        #endregion

                        #region Set default value for fields that cannot be empty but can have a general default value

                        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);//Get Server Date and Time (sdt)
                        _MSTJob._jobGrpKey = GFunc.NEInt(_MSTJob.JobGrpKey, 0);
                        _MSTJob._jobConKey = GFunc.NEInt(_MSTJob.JobConKey, 0);
                        _MSTJob._accessLevel = GFunc.NEInt(_MSTJob.AccessLevel, 0);
                        _MSTJob._accessGroup = GFunc.NEInt(_MSTJob.AccessGroup, 0);
                        _MSTJob._contractAmt = GFunc.NEDec(_MSTJob.ContractAmt, 0);
                        _MSTJob._retaintionAmt = GFunc.NEDec(_MSTJob.RetaintionAmt, 0);
                        _MSTJob._createDate = GFunc.NEDateTime(_MSTJob.CreateDate, svrDateTime);
                        _MSTJob._createUserKey = GFunc.NEInt(_MSTJob.CreateUserKey, AppInfor.currentUserKey);
                        _MSTJob._lastModifiedDate = svrDateTime;
                        _MSTJob._lastModifiedUserKey = AppInfor.currentUserKey;

                        //_MSTJobDetEsts
                        foreach (DataRow dr in _MSTJobDetEsts.Rows)
                        {
                            dr["JobKey"] = _MSTJob.JobKey;
                            dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
                            dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
                            dr["LastModifiedDate"] = svrDateTime;
                            dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
                        }
                        //_MSTJobDetOthers
                        foreach (DataRow dr in _MSTJobDetOthers.Rows)
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

                        if (Validation_Detail("tagrdMSTJobDetEst", (DataTable)this.ObjMSTJobDetEsts, cn) == false)
                            return false;
                        if (Validation_Detail("tagrdMSTJobDetOther", (DataTable)this.ObjMSTJobDetOthers, cn) == false)
                            return false;

                        #endregion

                        #region Save Record

                        if (IsNew)
                        {
                            if (!_MSTJob.Insert(cn, out newJobKey))
                                return false;

                            //Update new JobKey to details tables
                            foreach (DataRow row in _MSTJobDetEsts.Rows )
                            {
                                row["JobKey"] = newJobKey;

                            }
                            _MSTJobDetEsts.AcceptChanges();

                            foreach (DataRow row in _MSTJobDetOthers.Rows)
                            {
                                row["JobKey"] = newJobKey;

                            }
                            _MSTJobDetOthers.AcceptChanges();

                            //if (!_MSTJobDetEsts.Insert(cn, new MSTJobDetEsts.Criteria(newJobKey, 0)))
                            //    return false;
                            if (!_MSTJobDetEsts.Save(cn, newJobKey,_MSTJob.JobID,GFunc.ConvertDataTableToXML(_MSTJobDetEsts)))
                                return false;

                            if (!_MSTJobDetOthers.Insert(cn, new MSTJobDetOthers.Criteria(newJobKey, 0)))
                                return false;

                            if (_MSTJob.Attachments != null)
                            {
                                foreach (SYSAttachment obj in _MSTJob.Attachments)
                                {
                                    obj._docDK = newJobKey;
                                }
                                DocUtility.AttachmentSave(cn, _MSTJob.Attachments, this.constCodeKey, _MSTJob.JobKey);
                            }
                        }
                        else
                        {
                            if (!_MSTJob.Update(cn))
                                return false;

                            if (!_MSTJobDetEsts.Delete(cn, new MSTJobDetEsts.Criteria(_MSTJob._jobKey, 0)))
                                return false;
                            //if (!_MSTJobDetEsts.Insert(cn, new MSTJobDetEsts.Criteria(_MSTJob._jobKey, 0)))
                            //    return false;
                            if (!_MSTJobDetEsts.Save(cn, _MSTJob.JobKey, _MSTJob.JobID, GFunc.ConvertDataTableToXML(_MSTJobDetEsts)))
                                return false;

                            if (!_MSTJobDetOthers.Delete(cn, new MSTJobDetOthers.Criteria(_MSTJob._jobKey, 0)))
                                return false;
                            if (!_MSTJobDetOthers.Insert(cn, new MSTJobDetOthers.Criteria(_MSTJob._jobKey, 0)))
                                return false;

                            if (_MSTJob.Attachments != null)
                            {
                                DocUtility.AttachmentSave(cn, _MSTJob.Attachments, this.constCodeKey, _MSTJob._jobKey);
                            }
                        }
                        #endregion

                        #region For New Record perform: Locking, set new recordKey
                        if (IsNew)
                        {
                            if (SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newJobKey))
                                _MSTJob._jobKey = newJobKey;
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
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _MSTJob.JobKey, _MSTJob.JobID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTJob, _MSTJobDetEsts, _MSTJobDetOthers, _MSTJob.Attachments });
                else
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _MSTJob.JobKey, _MSTJob.JobID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTJob, _MSTJobDetEsts, _MSTJobDetOthers, _MSTJob.Attachments });
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
                    this._MSTJob = copyMSTJob;
                    this._MSTJobDetEsts = copyMSTJobDetEsts;
                    this._MSTJobDetOthers = copyMSTJobDetOthers;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTJob = null;
                copyMSTJobDetEsts = null;
                copyMSTJobDetOthers = null;
                #endregion
            }
        }//Completed
        public bool Delete()
        {
            #region Declaration
            bool restoreFlag = false;
            MSTJob copyMSTJob = null;
            MSTJobDetEsts copyMSTJobDetEsts = null;
            MSTJobDetOthers copyMSTJobDetOthers = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._MSTJob != null)
                    copyMSTJob = this._MSTJob.Clone();
                if (this._MSTJobDetEsts != null)
                    copyMSTJobDetEsts = GFunc.TACopyDataTable(_MSTJobDetEsts);

                if (this._MSTJobDetOthers != null)
                    copyMSTJobDetOthers = GFunc.TACopyDataTable(_MSTJobDetOthers);

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
                        if (SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _MSTJob._jobKey, GUID) == false)
                            return false;

                        //Check the record is used in other dependency tables
                        if (GFunc.CheckKeyDependantsExists(cn, "JobKey", _MSTJob._jobKey.Value, _MSTJob._jobID))
                            return false;

                        //Delete Record
                        if (_MSTJob.Delete(cn, new MSTJob.Criteria(_MSTJob._jobKey)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.DeleteFail);
                            return false;
                        }

                        // Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey) == false)
                            return false;

                        // Create New
                        this._MSTJob = MSTJob.New();
                        this._MSTJobDetEsts = new MSTJobDetEsts(cn);
                        this._MSTJobDetOthers = new MSTJobDetOthers(cn);

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
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyMSTJob.JobKey, copyMSTJob.JobID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyMSTJob, copyMSTJobDetEsts, copyMSTJobDetOthers });

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
                    this._MSTJob = copyMSTJob;
                    this._MSTJobDetEsts = copyMSTJobDetEsts;
                    this._MSTJobDetOthers = copyMSTJobDetOthers;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTJob = null;
                copyMSTJobDetEsts = null;
                copyMSTJobDetOthers = null;
                #endregion
            }
        }//Completed
        public bool CopyMyself()
        {
            try
            {
                if (!_isDirty)
                {
                    // Check Permission
                    if (!SECPermUtility.Add(constPermID, true))
                    { return false; }

                    using (TransactionScope scope = new TransactionScope())
                    {
                        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                        {
                            cn.Open();

                            //Remove Lock
                            if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                                return false;

                            //Clear only the header but keep the detail
                            //When user save this record, it will become a new job record
                            _MSTJob.JobKey = 0;
                            _MSTJob.JobStatus = 10;

                            _MSTJob.JobID = GFunc.ExecuteScalar(cn, "exec MSTJob_GetReviseID '" + _MSTJob.JobID + "'");
                           
                            // Commit Process                           
                            this._isNew = true;
                            this._isReadOnly = false;
                            
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        }
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
        public bool CanImportBOM()
        {
            try
            {
                // Check Permission
                if (!SECPermUtility.Edit(constPermID, true))
                    return false;

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        // Locking
                        if (SysLockUtility.IsProcessLock(cn, false, GEnum.SysLockOption.ByCodKey, GEnum.SystemCode.Inventory, this._guID))
                        {
                            MsgBox.Show(cn, "Import cannot continue. Please close Inventory window first.");
                            return false;
                        }
                        else if (SysLockUtility.IsProcessLock(cn, false, GEnum.SysLockOption.ByCodKey, GEnum.SystemCode.UOM, this._guID))
                        {
                            MsgBox.Show(cn, "Import cannot continue. Please close UOM window first.");
                            return false;
                        }

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
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
        public bool Dispose()
        {
            try
            {
                if (SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0) == false)
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
            
        }//Completed

        public DataTable GetItemInfo(int ItmKey)
        {            
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@ItmKey", ItmKey));

            DataTable dt = GFunc.ExecuteProc("Rep_SearchItmInfo", parmList);            
            return dt;
        }
        //Functions
        private void SetDefaultValue()
        {
            try
            {
                _MSTJob.JobKey = 0;
                _MSTJob.JobGrpKey = 0;
                _MSTJob.JobStatus = 10; //Pending
                _MSTJob.JobAttachment = false;
                _MSTJob.ContractAmt = 0;
                _MSTJob.RetaintionAmt = 0;
                _MSTJob.AccessLevel = 0;
                _MSTJob.AccessGroup = 0;
                _MSTJob.PurgeKeep = 0;
                _MSTJob.PurgeData = false;
                _MSTJob._minMarkupSalePercent = 35;
                _MSTJob._maxMarkupSalePercent = 80;
                _MSTJobDetEsts.Columns["JobEstKey"].DefaultValue = 1;
                _MSTJobDetEsts.Columns["EstSN"].DefaultValue = 1;

            }
            catch (TAException ex)
            {
                throw Error(ex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        
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
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobKey, "JobKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobID, "JobID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                }
                else
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobKey, "JobKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobID, "JobID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                }
                #endregion

                #region Validation Process
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobDes, "JobDes", GEnum.DataType.String, GEnum.Require.Yes, 3000, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobRem, "JobRem", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobGrpKey, "JobGrpKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobGrpID, "JobGrpID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobConKey, "JobConKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobConID, "JobConID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobClass, "JobClass", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobPOID, "JobPOID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobSupervisor, "JobSupervisor", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobContact, "JobContact", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobShipName, "JobShipName", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobShipMark, "JobShipMark", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobMemo, "JobMemo", GEnum.DataType.String, GEnum.Require.No, 8000, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobStatus, "JobStatus", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.JobAttachment, "JobAttachment", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.ContractAmt, "ContractAmt", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.RetaintionAmt, "RetaintionAmt", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.RetaintionDate, "RetaintionDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.AccessLevel, "AccessLevel", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.AccessGroup, "AccessGroup", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.PurgeKeep, "PurgeKeep", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.PurgeData, "PurgeData", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.Custom4, "Custom4", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTJob.Custom5, "Custom5", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                #endregion

                #region Check For Date
                if (e.PropertyMessage.Count == 0)
                {
                    if (!GFunc.IsNE(this._MSTJob._jobStartDate) && !GFunc.IsNE(this._MSTJob._retaintionDate))
                        if (((DateTime)this._MSTJob._retaintionDate.Value).Date < ((DateTime)this._MSTJob._jobStartDate.Value).Date)
                        {
                            e.PropertyMessage.Add("RetaintionDate", SysMessageUtility.Get(cn, MsgID.MSTJob.DateLessThanStartDate + "%Retaintion Date"));
                        }
                    if (!GFunc.IsNE(this._MSTJob._jobEndDate) && !GFunc.IsNE(this._MSTJob._jobStartDate))
                        if (((DateTime)this._MSTJob._jobEndDate.Value).Date < ((DateTime)this._MSTJob._jobStartDate.Value).Date)
                        {
                            e.PropertyMessage.Add("JobEndDate", SysMessageUtility.Get(cn, MsgID.MSTJob.DateLessThanStartDate + "%End Date"));
                        }
                    if (!GFunc.IsNE(this._MSTJob._jobTgtDate) && !GFunc.IsNE(this._MSTJob._jobStartDate))
                        if (((DateTime)this._MSTJob._jobTgtDate.Value).Date < ((DateTime)this._MSTJob._jobStartDate.Value).Date)
                        {
                            e.PropertyMessage.Add("JobTgtDate", SysMessageUtility.Get(cn, MsgID.MSTJob.DateLessThanStartDate + "%Target Date"));
                        }
                }
                #endregion

                #region Check for Duplicate Job ID
                if (e.PropertyMessage.Count == 0)
                {
                    bool DuplicateID = _MSTJob.Validation(cn, new MSTJob.Criteria(_MSTJob._jobKey, _MSTJob._jobID), this.IsNew);

                    if (!DuplicateID && !GFunc.IsNE(this.ErrorNotifierHeader_Set))
                    {
                        errorMsgID = "JobID" + MsgID.Validation.DuplicateRecord;
                        e.PropertyMessage.Add("JobID", SysMessageUtility.Get(cn, errorMsgID));
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
                        
                        Validation_DetailCheck(dr, grdNm,"","", false, ref processOK, e);


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
                    Validation_DetailCheck(drow, grdNm, "", "", false, ref processOK, e);
                }
                else
                    Validation_DetailCheck(drow, grdNm, grdrow.Cells[fieldToCheck].Value, fieldToCheck, false, ref processOK, e);

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
                #region get isStkType
                bool isStkType = false;
                int itmType = 0;

                if (GFunc.CompareString(grdNm, "tagrdMSTJobDetEst"))
                    itmType = GFunc.NEInt(drow["EstItmType"], 0);
                else
                    itmType = GFunc.NEInt(drow["OthItmType"], 0);

                switch (itmType)
                {
                    case (int)GEnum.ItemType.Finished_GDB:
                    case (int)GEnum.ItemType.Serial_Finished_GDB:
                    case (int)GEnum.ItemType.Serial_StockB:
                    case (int)GEnum.ItemType.StockB:
                    case (int)GEnum.ItemType.Finished_GD:
                    case (int)GEnum.ItemType.Stock:
                    case (int)GEnum.ItemType.Consignment:
                    case (int)GEnum.ItemType.Assembly:
                    case (int)GEnum.ItemType.Non_Stock:
                    case (int)GEnum.ItemType.Service:
                        isStkType = true;
                        break;
                }
                #endregion

                switch (grdNm)
                {
                    #region tagrdMSTJobDetEst Validation
                    case "tagrdMSTJobDetEst":
                        if (CheckNm == "")
                        {
                            BaseUtility.Validation(drow["JobEstKey"], "JobEstKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["JobPhaseKey"], "JobPhaseKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["JobTaskKey"], "JobTaskKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["JobCostTypeKey"], "JobCostTypeKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["EstSN"], "EstSN", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["EstItmDes"], "EstItmDes", CheckNm, GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["EstItmRem"], "EstItmRem", CheckNm, GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["EstCostF"], "EstCostF", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["EstCostH"], "EstCostH", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["EstAmtF"], "EstAmtF", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["EstAmtH"], "EstAmtH", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["DocCurrKey"], "DocCurrKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["DocCurrRate"], "DocCurrRate", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["TransmitMode"], "TransmitMode", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["Attention"], "Attention", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["emailAddr"], "emailAddr", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["FaxNumber"], "FaxNumber", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["TransmitStatus"], "TransmitStatus", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["Custom1"], "Custom1", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["Custom2"], "Custom2", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                            BaseUtility.Validation(drow["Custom3"], "Custom3", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                            //UOMKey
                            if (isStkType)
                                BaseUtility.Validation(drow["EstUOMKey"], "EstUOMKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                            else
                                BaseUtility.Validation(drow["EstUOMKey"], "EstUOMKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, null, null, null, null, ref processOK, failonError, e);
                        }
                        else
                        {
                            switch(CheckNm)
                            {
                                case "JobEstKey":
                                    BaseUtility.Validation(drow["JobEstKey"], "JobEstKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                                    break;
                                case "JobPhaseKey":
                                    BaseUtility.Validation(drow["JobPhaseKey"], "JobPhaseKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                                    break;
                                case "JobTaskKey":
                                    BaseUtility.Validation(drow["JobTaskKey"], "JobTaskKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                                    break;
                                case "JobCostTypeKey":
                                    BaseUtility.Validation(drow["JobCostTypeKey"], "JobCostTypeKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                                    break;
                                case "EstSN":
                                    BaseUtility.Validation(drow["EstSN"], "EstSN", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                                    break;
                                case "EstItmDes":
                                    BaseUtility.Validation(drow["EstItmDes"], "EstItmDes", CheckNm, GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, ref processOK, failonError, e);
                                    break;
                                case "EstItmRem":
                                    BaseUtility.Validation(drow["EstItmRem"], "EstItmRem", CheckNm, GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, ref processOK, failonError, e);
                                    break;
                                case "EstCostF":
                                    BaseUtility.Validation(drow["EstCostF"], "EstCostF", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                                    break;
                                case "EstCostH":
                                    BaseUtility.Validation(drow["EstCostH"], "EstCostH", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                                    break;
                                case "EstAmtF":
                                    BaseUtility.Validation(drow["EstAmtF"], "EstAmtF", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                                    break;
                                case "EstAmtH":
                                    BaseUtility.Validation(drow["EstAmtH"], "EstAmtH", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                                    break;                                    
                                case "DocCurrKey":
                                    BaseUtility.Validation(drow["DocCurrKey"], "DocCurrKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                                    break;
                                case "DocCurrRate":
                                    BaseUtility.Validation(drow["DocCurrRate"], "DocCurrRate", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                                    break;
                                case "TransmitMode":
                                    BaseUtility.Validation(drow["TransmitMode"], "TransmitMode", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                                    break;
                                case "Attention":
                                    BaseUtility.Validation(drow["Attention"], "Attention", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                                    break;
                                case "emailAddr":
                                    BaseUtility.Validation(drow["emailAddr"], "emailAddr", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                                    break;
                                case "FaxNumber":
                                    BaseUtility.Validation(drow["FaxNumber"], "FaxNumber", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                                    break;
                                case "TransmitStatus":
                                    BaseUtility.Validation(drow["TransmitStatus"], "TransmitStatus", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                                    break;
                                case "Custom1":
                                    BaseUtility.Validation(drow["Custom1"], "Custom1", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                                    break;
                                case "Custom2":
                                    BaseUtility.Validation(drow["Custom2"], "Custom2", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                                    break;
                                case "Custom3":
                                    BaseUtility.Validation(drow["Custom3"], "Custom3", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                                    break;
                                case "EstUOMKey":
                                    //UOMKey
                                    if (isStkType)
                                        BaseUtility.Validation(drow["EstUOMKey"], "EstUOMKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                                    else
                                        BaseUtility.Validation(drow["EstUOMKey"], "EstUOMKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, null, null, null, null, ref processOK, failonError, e);
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region tagrdMSTJobDetOther Validation
                    case "tagrdMSTJobDetOther":
                        BaseUtility.Validation(propValue, "JobOtherKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "JobPhaseKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "JobTaskKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "JobCostTypeKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "OthLineType", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "OthItmDes", CheckNm, GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "OthItmRem", CheckNm, GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "OthQty", CheckNm, GEnum.DataType.Decimel, GEnum.Require.No, null, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "OthUOMKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "OthConRate", CheckNm, GEnum.DataType.Decimel, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "OthPriceF", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "OthPriceH", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "OthExpAmtF", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "OthExpAmtH", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "OthRevAmtF", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "OthRevAmtH", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "OthPaidAmtF", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "OthPaidAmtH", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "DocID", CheckNm, GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "DocDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "DocDes", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "DocCurrKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "DocCurrRate", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom1", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom2", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom3", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                        //UOMKey
                        if (isStkType)
                            BaseUtility.Validation(propValue, "OthUOMKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        else
                            BaseUtility.Validation(propValue, "OthUOMKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, null, null, null, null, ref processOK, failonError, e);

                        break;
                    #endregion
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
        
        //Attached Events
        void Attachments_ListChanged(object sender, ListChangedEventArgs e)
        {
            _isDirty = true;
        }
        void Obj_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this._isReadOnly == false)
            {
                if (this.dirtyEvent != null)
                    this.dirtyEvent.Invoke(this, e);

                _isDirty = true;
            }
        }

        //Error
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _MSTJob, _MSTJobDetOthers, _MSTJobDetEsts }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _MSTJob, _MSTJobDetOthers, _MSTJobDetEsts }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
    }
}
