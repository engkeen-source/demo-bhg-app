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
using System.Diagnostics;
using Infragistics.Win.UltraWinGrid;
using TAUtil;
using System.Globalization;


namespace BOLib
{
    [Serializable()]
    public class TASToDoFactory : CommandBase
    {
        #region Member variables and constants

        private TASToDo _TASToDo = null;
        private TASToDoDetCriterias _TASToDoDetCriterias = null;
        private TASToDoDetSubs _TASToDoDetSubs = null;
        private TASToDoDetDocLists _TASToDoDetDocLists = null;
       

        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isOpenReadOnly = false;
        private int _guID = 0;

        // System Code Key for this Factory.
        private GEnum.SystemCode constCodeKey = GEnum.SystemCode.To_Do;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        // Permission ID for this Factory.
        public string PermID = GVar.PermissionID.To_Do;

        // Custom Event Declaration 
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;

        #endregion // Member variables and constant

        #region Factory Properties

        public TASToDo ObjTASToDo
        {
            get
            {
                return this._TASToDo;
            }
            set
            {
                this._TASToDo = value;
            }
        }
     
        public TASToDoDetCriterias ObjTASToDoDetCriterias
        {
            get
            {
                return this._TASToDoDetCriterias;
            }
            set
            {
                this._TASToDoDetCriterias = value;
            }
        }
        public TASToDoDetSubs ObjTASToDoDetSubs
        {
            get
            {
                return this._TASToDoDetSubs;
            }
            set
            {
                this._TASToDoDetSubs = value;
            }
        }

        public TASToDoDetDocLists ObjTASToDoDetDocLists
        {
            get
            {
                return this._TASToDoDetDocLists;
            }
            set
            {
                this._TASToDoDetDocLists = value;
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
                this._isDirty=value;
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
      
        public TASToDoFactory(GEnum.InstanceMode instanceMode)
        {
            this._instanceMode = instanceMode;
            this.Initialisation(instanceMode);
        }
        internal TASToDoFactory()
        {
        }
        public bool Initialisation(GEnum.InstanceMode instanceMode)
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
                        this._TASToDo = TASToDo.New();
                        this._TASToDoDetCriterias = new TASToDoDetCriterias(cn);
                        this._TASToDoDetSubs = new TASToDoDetSubs(cn);
                        this._TASToDoDetDocLists = new TASToDoDetDocLists(cn);

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
                        this._TASToDo = TASToDo.New();
                        this._TASToDoDetCriterias = new TASToDoDetCriterias(cn);
                        this._TASToDoDetSubs = new TASToDoDetSubs(cn);
                        this._TASToDoDetDocLists = new TASToDoDetDocLists(cn);

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
      
        public bool New()
        {
            bool restoreFlag = false;
            BOLib.TASToDo copyTASToDo = null;
            BOLib.TASToDoDetCriterias copyTASToDoDetCriterias = null;
            BOLib.TASToDoDetSubs copyTASToDoDetSubs = null;
            BOLib.TASToDoDetDocLists copyTASToDoDetDocLists = null;

            // Copy original object
            if (!GFunc.IsNE(this._TASToDo))
                copyTASToDo = this._TASToDo.Clone();

            //detail data Tables
            if (!GFunc.IsNE(this._TASToDoDetCriterias))
                copyTASToDoDetCriterias = this._TASToDoDetCriterias.Copy();

            if (!GFunc.IsNE(this._TASToDoDetSubs))
                copyTASToDoDetSubs = this._TASToDoDetSubs.Copy();

            if (!GFunc.IsNE(this._TASToDoDetDocLists))
                copyTASToDoDetDocLists = this._TASToDoDetDocLists.Copy();

            // Check Security Permission 
            if (SECPermUtility.Any(PermID, out this._isOpenReadOnly, true) == false)
                return false;
                                  
            try
            {
                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        // Remove all locks by GUID except inprogress Locking
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                            return false;
                     
                        this._TASToDo = TASToDo.New();
                        this._TASToDoDetCriterias = new TASToDoDetCriterias(cn);
                        this._TASToDoDetSubs = new TASToDoDetSubs(cn);
                        this._TASToDoDetDocLists = new TASToDoDetDocLists(cn);

                        HeaderDefaultValue_Set();
                        DetailSubDefaultValue_Set();
                        DetailCriteriaDefaultValue_Set();

                        this._TASToDo.PropertyChanged += new PropertyChangedEventHandler(_TASToDoList_PropertyChanged);

                        this._isDirty = false;
                        this._isNew = true;
                        this._isOpenReadOnly = false;
                      
                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                    }// End of SqlConnection
                }// End of TransactionScope
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
                if (restoreFlag == true)
                {
                    // Restore data when error is occur                    
                    this._TASToDo = copyTASToDo;
                    this._TASToDoDetCriterias = copyTASToDoDetCriterias;
                    this._TASToDoDetSubs = copyTASToDoDetSubs;
                    this._TASToDoDetDocLists = copyTASToDoDetDocLists;
                }
                // Null set to Backup Objects
                copyTASToDo = null;
                copyTASToDoDetCriterias = null;
                copyTASToDoDetSubs = null;
                copyTASToDoDetDocLists = null;
            }
           
           
        }
        public bool GetEdit(int toDoKey)
        {
            // Initialisation
            bool restoreFlag = false;
            // Copy original object
            BOLib.TASToDo copyTASToDo = null;
            BOLib.TASToDoDetCriterias copyTASToDoDetCriterias = null;
            BOLib.TASToDoDetSubs copyTASToDoDetSubs = null;
            BOLib.TASToDoDetDocLists copyTASToDoDetDocLists = null;

            try
            {

                #region Make backup of objects for restore purpose

                if (this._TASToDo != null)
                    copyTASToDo = this._TASToDo.Clone();             

                //detail data Table
                if (this._TASToDoDetCriterias != null)
                    copyTASToDoDetCriterias = this._TASToDoDetCriterias.Copy();              

                if (this._TASToDoDetSubs != null)
                    copyTASToDoDetSubs = this._TASToDoDetSubs.Copy();              

                if (this._TASToDoDetDocLists != null)
                    copyTASToDoDetDocLists = this._TASToDoDetDocLists.Copy();            

                #endregion

                // Check Permission
                if (!SECPermUtility.Edit(PermID, true))
                    return false;

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;
                       
                        // Check Lock
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, toDoKey, 0, _guID))
                            return false;

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                            return false;

                        // Add Lock
                        if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, toDoKey))
                            return false;                      

                        // Get Header Record                                 
                        if (!this._TASToDo.Fetch(cn, new TASToDo.Criteria(toDoKey, 1)))
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        //Record Not Found
                        if (GFunc.NEInt(this._TASToDo._toDoKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }

                        this._TASToDoDetCriterias.Clear();
                        if (!this._TASToDoDetCriterias.Fetch(cn, new TASToDoDetCriterias.Criteria(toDoKey, 1)))
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }
                        this._TASToDoDetSubs.Clear();
                        if (!this._TASToDoDetSubs.Fetch(cn, new TASToDoDetSubs.Criteria(toDoKey, 1)))
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        this._TASToDoDetDocLists.Clear();
                        if (!this._TASToDoDetDocLists.Fetch(cn, new TASToDoDetDocLists.Criteria(toDoKey, 1)))
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        this._isDirty = false;
                        this._isNew = false;
                        this._isOpenReadOnly = false;

                        DetailSubDefaultValue_Set();
                        DetailCriteriaDefaultValue_Set();

                        this._TASToDo.PropertyChanged += new PropertyChangedEventHandler(_TASToDoList_PropertyChanged);

                    }// End of SqlConnection
                } // End of TransactionScope
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
                    this._TASToDo = copyTASToDo;
                    this._TASToDoDetCriterias = copyTASToDoDetCriterias;
                    this._TASToDoDetDocLists = copyTASToDoDetDocLists;
                    this._TASToDoDetSubs = copyTASToDoDetSubs;
                }
                #endregion

                #region Dispose Backup Objects
                copyTASToDo = null;
                copyTASToDoDetCriterias = null;
                copyTASToDoDetDocLists = null;
                copyTASToDoDetSubs = null;
                #endregion
            }
        }
        public bool GetReadOnly(int toDoKey)
        {
            // Initialisation
            bool restoreFlag = false;
            // Copy original object
            BOLib.TASToDo copyTASToDo = null;
            BOLib.TASToDoDetCriterias copyTASToDoDetCriterias = null;
            BOLib.TASToDoDetSubs copyTASToDoDetSubs = null;
            BOLib.TASToDoDetDocLists copyTASToDoDetDocLists = null;

            try
            {

                #region Make backup of objects for restore purpose

                if (this._TASToDo != null)
                    copyTASToDo = this._TASToDo.Clone();

                //detail data Table
                if (this._TASToDoDetCriterias != null)
                    copyTASToDoDetCriterias = this._TASToDoDetCriterias;

                if (this._TASToDoDetSubs != null)
                    copyTASToDoDetSubs = this._TASToDoDetSubs;

                if (this._TASToDoDetDocLists != null)
                    copyTASToDoDetDocLists = this._TASToDoDetDocLists;

                #endregion

                // Check Permission
                if (!SECPermUtility.Edit(PermID, true))
                    return false;

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                            return false;

                        // Get Header Record                                 
                        if (!this._TASToDo.Fetch(cn, new TASToDo.Criteria(toDoKey, 1)))
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        //Record Not Found
                        if (GFunc.NEInt(this._TASToDo._toDoKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }

                        this._TASToDoDetCriterias.Clear();
                        if (!this._TASToDoDetCriterias.Fetch(cn, new TASToDoDetCriterias.Criteria(toDoKey, 1)))
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }
                        this._TASToDoDetSubs.Clear();
                        if (!this._TASToDoDetSubs.Fetch(cn, new TASToDoDetSubs.Criteria(toDoKey, 1)))
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        this._TASToDoDetDocLists.Clear();
                        if (!this._TASToDoDetDocLists.Fetch(cn, new TASToDoDetDocLists.Criteria(toDoKey, 1)))
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        this._isDirty = false;
                        this._isNew = false;
                        this._isOpenReadOnly = true;

                        DetailSubDefaultValue_Set();
                        DetailCriteriaDefaultValue_Set();

                        this._TASToDo.PropertyChanged += new PropertyChangedEventHandler(_TASToDoList_PropertyChanged);

                    }// End of SqlConnection
                } // End of TransactionScope
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
                    this._TASToDo = copyTASToDo;
                    this._TASToDoDetCriterias = copyTASToDoDetCriterias;
                    this._TASToDoDetDocLists = copyTASToDoDetDocLists;
                    this._TASToDoDetSubs = copyTASToDoDetSubs;
                }
                #endregion

                #region Dispose Backup Objects
                copyTASToDo = null;
                copyTASToDoDetCriterias = null;
                copyTASToDoDetDocLists = null;
                copyTASToDoDetSubs = null;
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

                        if (GFunc.IsNE(_TASToDo))
                            _TASToDo = TASToDo.New();
                        GFunc.ConvertDataTableToObject(dtHeader, _TASToDo);

                        _TASToDoDetCriterias = new TASToDoDetCriterias(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[0], _TASToDoDetCriterias);
                        _TASToDoDetDocLists = new TASToDoDetDocLists(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[1], _TASToDoDetDocLists);
                        _TASToDoDetSubs = new TASToDoDetSubs(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[2], _TASToDoDetSubs);

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

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
        }//Completed
      
        public bool Save()
        {
              // Initialisation
            bool restoreFlag = false;
            bool isNewRecord = this.IsNew;//Because this flag will be changed during saving, we need to know the original value when error occurs
            int newRecordKey = 0;

            BOLib.TASToDo copyTASToDo = null;
            BOLib.TASToDoDetCriterias copyTASToDoDetCriterias = null;
            BOLib.TASToDoDetSubs copyTASToDoDetSubs = null;
            BOLib.TASToDoDetDocLists copyTASToDoDetDocLists = null;

            try
            {

                #region Make backup of objects for restore purpose

                if (this._TASToDo != null)
                    copyTASToDo = this._TASToDo.Clone();

                //detail data Table
                if (this._TASToDoDetCriterias != null)
                    copyTASToDoDetCriterias = this._TASToDoDetCriterias.Copy();

                if (this._TASToDoDetSubs != null)
                    copyTASToDoDetSubs = this._TASToDoDetSubs.Copy();

                if (this._TASToDoDetDocLists != null)
                    copyTASToDoDetDocLists = this._TASToDoDetDocLists.Copy();

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


                // Create TransactionScope  
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        #region Set Server DateTime If Created and Modified Date is null
                        //Get Server Date and Time (sdt)
                        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                        _TASToDo._createDate = GFunc.NEDateTime(_TASToDo.CreateDate, svrDateTime);
                        _TASToDo._createUserKey = GFunc.NEInt(_TASToDo.CreateUserKey, AppInfor.currentUserKey);
                        _TASToDo._lastModifiedDate = svrDateTime;
                        _TASToDo._lastModifiedUserKey = AppInfor.currentUserKey;
                        #endregion

                        #region Validation
                        if (Validation_Header(cn) == false)
                            return false;

                        if (Validation_Detail("tagrdDetDocList", (DataTable)this._TASToDoDetDocLists, cn) == false)
                            return false;
                        if (Validation_Detail("tagrdDetSub", (DataTable)this._TASToDoDetSubs, cn) == false)
                            return false;

                        #endregion

                        #region Header & Detail Saving
                        // Save Header Record
                        if (IsNew)
                        {
                            if (!this._TASToDo.Insert(cn, out newRecordKey))
                                return false;

                            //Update new Key to details tables
                            foreach (DataRow row in _TASToDoDetCriterias.Rows)
                            {
                                row["ToDoKey"] = newRecordKey;
                            }
                            //Update new Key to details tables
                            foreach (DataRow row in _TASToDoDetSubs.Rows)
                            {
                                row["ToDoKey"] = newRecordKey;
                            }
                            //Update new Key to details tables
                            foreach (DataRow row in _TASToDoDetDocLists.Rows)
                            {
                                row["ToDoKey"] = newRecordKey;
                            }

                            this._TASToDoDetSubs.Save(cn, newRecordKey, this._TASToDoDetSubs);
                            this._TASToDoDetCriterias.Save(cn, newRecordKey, this._TASToDoDetCriterias);
                            this._TASToDoDetDocLists.Save(cn, newRecordKey, this._TASToDoDetDocLists);

                        }
                        else
                        {
                            if (this._TASToDo.Update(cn) == false)
                                return false;

                            this._TASToDoDetSubs.Delete(cn, new TASToDoDetSubs.Criteria(this.ObjTASToDo.ToDoKey, 0));
                            this._TASToDoDetCriterias.Delete(cn, new TASToDoDetCriterias.Criteria(this.ObjTASToDo.ToDoKey, 0));
                            this._TASToDoDetDocLists.Delete(cn, new TASToDoDetDocLists.Criteria(this.ObjTASToDo.ToDoKey, 0));

                            this._TASToDoDetSubs.Save(cn, this.ObjTASToDo.ToDoKey, this._TASToDoDetSubs);
                            this._TASToDoDetCriterias.Save(cn, this._TASToDo.ToDoKey, this._TASToDoDetCriterias);
                            this._TASToDoDetDocLists.Save(cn, this._TASToDo.ToDoKey, this._TASToDoDetDocLists);
                           
                        }                    


                        #endregion Header & Detail Saving

                        // Record Locking
                        if (isNewRecord && this._instanceMode != GEnum.InstanceMode.InternalCall)
                        {
                            if (SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newRecordKey))
                                _TASToDo.ToDoKey = (int)newRecordKey;
                            else
                                return false;
                        }


                        if (!SaveToAgent(cn))
                            throw new TAException("Save to Agent Task failed.");
                      
                        this._isDirty = false;
                        this._isNew = false;

                    }
                    // No errors - commit transaction
                    if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)
                        throw new Exception("Transaction has aborted.");
                    scope.Complete();

                }// End of TransactionScope

                // Audit Log
                if (isNewRecord)
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, this._TASToDo.ToDoKey, this._TASToDo.ToDoDes, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { this._TASToDo,_TASToDoDetCriterias,_TASToDoDetDocLists, _TASToDoDetSubs });
                else
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, this._TASToDo.ToDoKey, this._TASToDo.ToDoDes, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { this._TASToDo,_TASToDoDetCriterias, _TASToDoDetDocLists, _TASToDoDetSubs });

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
                    this._TASToDo = copyTASToDo;
                    this._TASToDoDetCriterias = copyTASToDoDetCriterias;
                    this._TASToDoDetDocLists = copyTASToDoDetDocLists;
                    this._TASToDoDetSubs = copyTASToDoDetSubs;
                }
                #endregion

                #region Dispose Backup Objects
                copyTASToDo = null;
                copyTASToDoDetCriterias = null;
                copyTASToDoDetDocLists = null;
                copyTASToDoDetSubs = null;
                #endregion
            }
        }
        private bool SaveToAgent(SqlConnection cn)
        {
            bool hasReminder = false;
            DateTime RemindDateTime = new DateTime();
            DateTime NextRunDateTime = new DateTime();
          
           // string strXml = GFunc.ConvertObjectToXML(ObjTASToDo);// May trying out new method, this is old code to be removed later
            //DataTable dt = GFunc.ConvertObjectToDataTable(ObjTASToDo,"TASToDo");

            if (ObjTASToDo.RecurType == 10)//Once
                NextRunDateTime = ObjTASToDo.DateStart.Value.Date + ObjTASToDo.TimeStart.Value.TimeOfDay;
            else if (ObjTASToDo.RecurType == 20)//Daily
            {
                if (ObjTASToDo.DateStart.Value.Date + ObjTASToDo.TimeStart.Value.TimeOfDay <= DateTime.Now)
                {
                    NextRunDateTime = ObjTASToDo.DateStart.Value.Date.AddDays(ObjTASToDo.RecurIntDayNum) + ObjTASToDo.TimeStart.Value.TimeOfDay;
                }
                else
                    NextRunDateTime =  ObjTASToDo.DateStart.Value.Date + ObjTASToDo.TimeStart.Value.TimeOfDay;
                if (NextRunDateTime > ObjTASToDo.DateEnd.Value.Date + ObjTASToDo.TimeStart.Value.TimeOfDay)
                    NextRunDateTime = new DateTime(1900, 1, 1);
            }
            else if (ObjTASToDo.RecurType == 30)//Weekly
            {
                
                if (ObjTASToDo.RecurIntWeekDay < (int)ObjTASToDo.DateStart.Value.Date.DayOfWeek)
                {
                    NextRunDateTime = ObjTASToDo.DateStart.Value.Date.AddDays(7 - (int)DateTime.Today.DayOfWeek + ObjTASToDo.RecurIntWeekDay) + ObjTASToDo.TimeStart.Value.TimeOfDay;
                }
                else
                    NextRunDateTime = ObjTASToDo.DateStart.Value.Date.AddDays(ObjTASToDo.RecurIntWeekDay - (int)ObjTASToDo.DateStart.Value.Date.DayOfWeek) + ObjTASToDo.TimeStart.Value.TimeOfDay;

                if (NextRunDateTime > ObjTASToDo.DateEnd.Value.Date + ObjTASToDo.TimeStart.Value.TimeOfDay)
                    NextRunDateTime = new DateTime(1900, 1, 1);
            }
            else if (ObjTASToDo.RecurType == 40)//Monthly
            {
                if (ObjTASToDo.RecurIntMthDayNum != 0)//By exact day
                {
                    if (ObjTASToDo.RecurIntMthDayNum < ObjTASToDo.DateStart.Value.Date.Day ||
                        (ObjTASToDo.RecurIntMthDayNum == ObjTASToDo.DateStart.Value.Date.Day && ObjTASToDo.TimeStart.Value.TimeOfDay < DateTime.Now.TimeOfDay))
                    {
                        NextRunDateTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month + ObjTASToDo.RecurIntMthNum, ObjTASToDo.RecurIntMthDayNum) + ObjTASToDo.TimeStart.Value.TimeOfDay;
                    }
                    else                        
                        NextRunDateTime = new DateTime(ObjTASToDo.DateStart.Value.Date.Year, ObjTASToDo.DateStart.Value.Date.Month , ObjTASToDo.RecurIntMthDayNum) + ObjTASToDo.TimeStart.Value.TimeOfDay;
                   
                }
                else
                {
                                
                    DateTime fallenDate = ObjTASToDo.DateStart.Value.Date;                   
                    int week;
                    int WeekNumOfStartDate;
                    int DayOfWeek;
                    if ((DateTime.IsLeapYear(fallenDate.Year) == false && ObjTASToDo.RecurIntMthWeek == 5 && fallenDate.Month == 2))
                        week = 4;
                    else
                        week = ObjTASToDo.RecurIntMthWeek;

                    DateTime firstDayOfMonth = new DateTime(fallenDate.Year, fallenDate.Month, 1);                    
                    DayOfWeek = (int)new DateTime(fallenDate.Year, fallenDate.Month, 1).DayOfWeek;            
                    int Day = ObjTASToDo.RecurIntMthDay;

                    //To get Week Number of the StateDate 
                    DateTime date = ObjTASToDo.DateStart.Value.Date;
                    DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);
                    while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                        date = date.AddDays(1);
                    WeekNumOfStartDate = ((int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1);

                   // WeekNumOfStartDate = (ObjTASToDo.DateStart.Value.Date.Day/7) +1;

                    //found in this month
                    if (ObjTASToDo.RecurIntMthWeek > WeekNumOfStartDate || 
                        (ObjTASToDo.RecurIntMthWeek == WeekNumOfStartDate && DayOfWeek < Day) ||
                        (ObjTASToDo.RecurIntMthWeek == WeekNumOfStartDate && DayOfWeek == Day && ObjTASToDo.TimeStart.Value.TimeOfDay > DateTime.Now.TimeOfDay)) //if the required week day is not exist in current week and RecurWeek is found in the month of StartDate , skip to next week
                    {
                        NextRunDateTime = firstDayOfMonth.AddDays((week * 7 - DayOfWeek) + Day).Date + ObjTASToDo.TimeStart.Value.TimeOfDay;
                    }
                    //eg.   startdate is 16-Mar-2012(Friday) 3rd week.
                    //      RecurDay 2nd week of every Month.                                                                                                    
                    //      ThereFore, Recur 2nd week is earlier then StartDate 3rd week. Recur datetime should not be earlier then start datetime,
                    //      Then NextRunTime should not be in March. it should be in April.

                    //not found in this month, skip to next month
                    else if (ObjTASToDo.RecurIntMthWeek < WeekNumOfStartDate || 
                        (ObjTASToDo.RecurIntMthWeek == WeekNumOfStartDate && DayOfWeek > Day) ||
                        (ObjTASToDo.RecurIntMthWeek == WeekNumOfStartDate && DayOfWeek == Day && ObjTASToDo.TimeStart.Value.TimeOfDay < DateTime.Now.TimeOfDay)) //RecurWeek is not found in the month of StartDate, skip to next month                                                                                                     
                    {       
                        firstDayOfMonth = firstDayOfMonth.AddMonths(ObjTASToDo.RecurIntMthNum);
                        DayOfWeek = (int)new DateTime(firstDayOfMonth.Year, firstDayOfMonth.Month, 1).DayOfWeek;    
                        NextRunDateTime = firstDayOfMonth.AddDays(((week - 1) * 7) + (Day - DayOfWeek)).Date + ObjTASToDo.TimeStart.Value.TimeOfDay;
                    }                  
                    else 
                    {
                        NextRunDateTime = firstDayOfMonth.AddDays(((week - 1) * 7)).Date + ObjTASToDo.TimeStart.Value.TimeOfDay;
                    }
                }
      
                if (NextRunDateTime > ObjTASToDo.DateEnd.Value.Date + ObjTASToDo.TimeStart.Value.TimeOfDay)
                    NextRunDateTime = new DateTime(1900, 1, 1);
           
            }
            else if (ObjTASToDo.RecurType == 50)//Yearly //To start 17-Feb
            {
                if (ObjTASToDo.RecurIntYearDayNum != 0)//By exact day
                {
                    if (ObjTASToDo.RecurIntYearMthNum > ObjTASToDo.DateStart.Value.Date.Month                      
                        || ((ObjTASToDo.RecurIntYearMthNum == ObjTASToDo.DateStart.Value.Date.Month && ObjTASToDo.RecurIntYearDayNum > ObjTASToDo.DateStart.Value.Date.Day) && ObjTASToDo.TimeStart.Value.TimeOfDay < DateTime.Now.TimeOfDay))
                    {
                        NextRunDateTime = new DateTime(ObjTASToDo.DateStart.Value.Date.Year, ObjTASToDo.RecurIntYearMthNum, ObjTASToDo.RecurIntYearDayNum) + ObjTASToDo.TimeStart.Value.TimeOfDay;
                    }
                    else
                        NextRunDateTime = new DateTime(ObjTASToDo.DateStart.Value.Date.Year + ObjTASToDo.RecurIntYearNum, ObjTASToDo.RecurIntYearMthNum, ObjTASToDo.RecurIntYearDayNum) + ObjTASToDo.TimeStart.Value.TimeOfDay;
                }
                else
                {

                    DateTime fallenDate = ObjTASToDo.DateStart.Value.Date;

                    int week;
                    int WeekNumOfStartDate;
                    int DayOfWeek;
                    if ((DateTime.IsLeapYear(fallenDate.Year) == false && ObjTASToDo.RecurIntMthWeek == 5 && fallenDate.Month == 2))
                        week = 4;
                    else
                        week = ObjTASToDo.RecurIntYearMthWeek;

                    DateTime firstDayOfMonth = new DateTime(fallenDate.Year, 1, 1);
                    DayOfWeek = (int)new DateTime(fallenDate.Year,1, 1).DayOfWeek;
                    int Day = ObjTASToDo.RecurIntYearMthDay;

                    //To get Week Number of the StateDate 
                    DateTime date = ObjTASToDo.DateStart.Value.Date;
                    DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);
                    while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                        date = date.AddDays(1);
                    WeekNumOfStartDate = ((int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1);

                   // WeekNumOfStartDate = (ObjTASToDo.DateStart.Value.Date.Day / 7) + 1;

                    //found in this year
                    if (ObjTASToDo.RecurIntYearMthNum > ObjTASToDo.DateStart.Value.Date.Month ||
                        (ObjTASToDo.RecurIntYearMthNum >= ObjTASToDo.DateStart.Value.Date.Month && ObjTASToDo.RecurIntYearMthWeek > WeekNumOfStartDate) ||
                        (ObjTASToDo.RecurIntYearMthNum == ObjTASToDo.DateStart.Value.Date.Month && ObjTASToDo.RecurIntYearMthWeek == WeekNumOfStartDate && DayOfWeek < Day) ||
                        (ObjTASToDo.RecurIntYearMthNum == ObjTASToDo.DateStart.Value.Date.Month && ObjTASToDo.RecurIntYearMthWeek == WeekNumOfStartDate && DayOfWeek == Day && ObjTASToDo.TimeStart.Value.TimeOfDay > DateTime.Now.TimeOfDay)) //if the required week day is not exist in current week and RecurWeek is found in the month of StartDate , skip to next week
                    {
                        firstDayOfMonth = firstDayOfMonth.AddMonths(ObjTASToDo.RecurIntYearMthNum-1);
                        DayOfWeek = (int)firstDayOfMonth.DayOfWeek;
                        if(DayOfWeek < Day)
                            NextRunDateTime = firstDayOfMonth.AddDays(((week-1) * 7 - DayOfWeek) + Day).Date + ObjTASToDo.TimeStart.Value.TimeOfDay;
                        else
                            NextRunDateTime = firstDayOfMonth.AddDays((week * 7 - DayOfWeek) + Day).Date + ObjTASToDo.TimeStart.Value.TimeOfDay;
                    }                 
                    //not found in this year, skip to next year
                    else if (ObjTASToDo.RecurIntYearMthNum < ObjTASToDo.DateStart.Value.Date.Month ||
                        (ObjTASToDo.RecurIntYearMthNum <= ObjTASToDo.DateStart.Value.Date.Month && ObjTASToDo.RecurIntYearMthWeek < WeekNumOfStartDate) ||                        
                        (ObjTASToDo.RecurIntYearMthNum == ObjTASToDo.DateStart.Value.Date.Month && ObjTASToDo.RecurIntYearMthWeek == WeekNumOfStartDate && DayOfWeek > Day) ||
                        (ObjTASToDo.RecurIntYearMthNum == ObjTASToDo.DateStart.Value.Date.Month && ObjTASToDo.RecurIntYearMthWeek == WeekNumOfStartDate && DayOfWeek == Day && ObjTASToDo.TimeStart.Value.TimeOfDay < DateTime.Now.TimeOfDay)) //RecurWeek is not found in the month of StartDate, skip to next month                                                                                                     
                    {
                        firstDayOfMonth = firstDayOfMonth.AddYears(ObjTASToDo.RecurIntYearNum);
                        firstDayOfMonth = new DateTime(firstDayOfMonth.Year,ObjTASToDo.RecurIntYearMthNum,1);                        
                        DayOfWeek = (int)new DateTime(firstDayOfMonth.Year, firstDayOfMonth.Month, 1).DayOfWeek;
                        NextRunDateTime = firstDayOfMonth.AddDays(((week - 1) * 7) + (Day - DayOfWeek)).Date + ObjTASToDo.TimeStart.Value.TimeOfDay;
                    }
                    else
                    {
                        NextRunDateTime = firstDayOfMonth.AddDays(((week - 1) * 7)).Date + ObjTASToDo.TimeStart.Value.TimeOfDay;
                    }
                } 

                if (NextRunDateTime > ObjTASToDo.DateEnd.Value.Date + ObjTASToDo.TimeStart.Value.TimeOfDay)
                    NextRunDateTime = new DateTime(1900, 1, 1);
            }           

            if (ObjTASToDo.RemindType > 0 && NextRunDateTime>new DateTime(1900,1,1))
            {
                hasReminder = true;
                switch (ObjTASToDo.RemindType)
                {
                    case 10://By Date and Time
                        RemindDateTime = ObjTASToDo.RemindDate.Value;//.Date + ObjTASToDo.TimeStart.Value.TimeOfDay;
                        break;
                    case 20://By Days before
                        RemindDateTime = NextRunDateTime.AddDays(-1 * ObjTASToDo.RemindDayBefore.Value);
                        break;
                    case 30://By Hours before
                        RemindDateTime = NextRunDateTime.AddHours(-1 * ObjTASToDo.RemindHourBefore.Value);
                        break;
                }
            }

            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "TASToDoSaveToAgent";

                SqlParameter parameter = new SqlParameter();
                parameter = new SqlParameter();

                //parameter.ParameterName = "@CriteriaXML";//May trying out new method, this is old code to be removed later
                //parameter.DbType = DbType.Xml;
                //parameter.Direction = ParameterDirection.Input;
                //parameter.Value = strXml;
                //cm.Parameters.Add(parameter);                
              
                parameter = new SqlParameter();
                parameter.ParameterName = "@ToDoKey";
                parameter.DbType = DbType.Int32;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = ObjTASToDo.ToDoKey;
                cm.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.ParameterName = "@IsReminder"; //If there is reminder, will insert new record
                parameter.DbType = DbType.Boolean;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value =false;
                cm.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.ParameterName = "@TaskName"; 
                parameter.DbType = DbType.String;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = ObjTASToDo.ToDoDes;
                cm.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.ParameterName = "@ApplicationName";
                parameter.DbType = DbType.String;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = SysOptionUtility.GetStr("ServerAgentServiceExePath", cn);
                cm.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.ParameterName = "@NextRunDateTime";
                parameter.DbType = DbType.DateTime;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = NextRunDateTime;
                cm.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.ParameterName = "@LastRunDateTime";
                parameter.DbType = DbType.DateTime;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = DBNull.Value;
                cm.Parameters.Add(parameter);

                //@Mode
                parameter = new SqlParameter();
                parameter.ParameterName = "@Mode";
                parameter.DbType = DbType.Int32;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = 1; 
                cm.Parameters.Add(parameter);
                
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
               
                cm.ExecuteNonQuery();               
            
                if(GFunc.NEInt(cm.Parameters["@RetValue"].Value,0) == (int)GEnum.SpState.Pass)
                {
                    if (hasReminder)//Add Reminder as another Task
                    {
                        cm.Parameters["@IsReminder"].Value = true;
                        cm.Parameters["@NextRunDateTime"].Value = RemindDateTime;
                        cm.ExecuteNonQuery();
                        if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                            return true;
                    }
                    return true;
                }
                else
                    return false;
            }
        }              
        public bool Delete()
        {
            bool restoreFlag = false;

            // Copy original object
            BOLib.TASToDo copyTASToDo = null;
            BOLib.TASToDoDetCriterias copyTASToDoDetCriterias = null;
            BOLib.TASToDoDetSubs copyTASToDoDetSubs = null;
            BOLib.TASToDoDetDocLists copyTASToDoDetDocLists = null;

            try
            {

                #region Make backup of objects for restore purpose

                if (this._TASToDo != null)
                    copyTASToDo = this._TASToDo.Clone();

                //detail data Table
                if (this._TASToDoDetCriterias != null)
                    copyTASToDoDetCriterias = this._TASToDoDetCriterias.Copy();

                if (this._TASToDoDetSubs != null)
                    copyTASToDoDetSubs = this._TASToDoDetSubs.Copy();

                if (this._TASToDoDetDocLists != null)
                    copyTASToDoDetDocLists = this._TASToDoDetDocLists.Copy();

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

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        if (this.ObjTASToDo.ToDoKey == 0)
                        { return false; }

                        // Record Locking
                        if (!SysLockUtility.CheckAddLock(cn, true, 0, GEnum.SystemCode.Security_Group, this._TASToDo.ToDoKey, this._guID))
                        { return false; }

                        // Check the record is used in other dependency tables
                        if (GFunc.CheckKeyDependantsExists(cn, "ToDoKey", _TASToDo.ToDoKey, _TASToDo.ToDoDes))
                        { return false; }

                        // Delete Record
                        if (!this._TASToDo.Delete(cn, new TASToDo.Criteria(this._TASToDo.ToDoKey, 0)))
                        { return false; }

                        //Information: In store prco, also delete from detail Tables and TAAgent Database- AgentTask Table

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                        { return false; }

                        // Call New for Price List
                        this._TASToDo = TASToDo.New();
                        this._TASToDoDetCriterias = new TASToDoDetCriterias(cn);
                        this._TASToDoDetSubs = new TASToDoDetSubs(cn);
                        this._TASToDoDetDocLists = new TASToDoDetDocLists(cn);

                        // Alert Process
                        this._isNew = true;
                        this._isOpenReadOnly = false;                     
                        _isDirty = false;
                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                    }// End of SqlConnection
                }// End of TransactionScope

                // AuditLog                                        
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyTASToDo.ToDoKey, copyTASToDo.ToDoDes, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyTASToDo, copyTASToDoDetCriterias, copyTASToDoDetSubs, copyTASToDoDetDocLists });

                restoreFlag = false;
                return true;
            }
            catch (Exception ex)
            {               
                throw (ex);
            }
            finally
            {
                if (restoreFlag)
                {
                    // Restore data when error is occur                    
                    this._TASToDo = copyTASToDo;
                    this._TASToDoDetCriterias = copyTASToDoDetCriterias;
                    this._TASToDoDetSubs = copyTASToDoDetSubs;
                    this._TASToDoDetDocLists = copyTASToDoDetDocLists;
                }
                // Set Null to Backup Objects
                copyTASToDo = null;
                copyTASToDoDetCriterias = null;
                copyTASToDoDetSubs = null;
                copyTASToDoDetDocLists = null;
            }
          
        }      
        public bool Dispose()
        {
            if (this.InstanceMode == GEnum.InstanceMode.Normal)
            {
                if (!SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0))
                    return false;
            }

            return true;
        }

        private void HeaderDefaultValue_Set()
        {
            ObjTASToDo.RecurType = 10;
        }
        private void DetailCriteriaDefaultValue_Set()
        {
            ObjTASToDoDetCriterias.Columns["ToDoKey"].DefaultValue = ObjTASToDo.ToDoKey;
            ObjTASToDoDetCriterias.Columns["CriteriaKey"].DefaultValue = 0;
            ObjTASToDoDetCriterias.Columns["CriteriaName"].DefaultValue = string.Empty;
            ObjTASToDoDetCriterias.Columns["CriteriaLabel"].DefaultValue = string.Empty;
            ObjTASToDoDetCriterias.Columns["CriteriaDataType"].DefaultValue = string.Empty;
            ObjTASToDoDetCriterias.Columns["CriteriaValueChar"].DefaultValue = string.Empty;
            ObjTASToDoDetCriterias.Columns["CriteriaValueInt"].DefaultValue = null;
            ObjTASToDoDetCriterias.Columns["CriteriaValueMoney"].DefaultValue = null;
            ObjTASToDoDetCriterias.Columns["CriteriaValueDate"].DefaultValue = DateTime.Today;
            ObjTASToDoDetCriterias.Columns["DateType"].DefaultValue = 10;
            ObjTASToDoDetCriterias.Columns["DateDifference"].DefaultValue = 0;
            ObjTASToDoDetCriterias.Columns["WeekDay"].DefaultValue = 0;
            ObjTASToDoDetCriterias.Columns["MthDayNum"].DefaultValue = 0;
            ObjTASToDoDetCriterias.Columns["MthWeek"].DefaultValue = 0;
            ObjTASToDoDetCriterias.Columns["MthDay"].DefaultValue = 0;
            ObjTASToDoDetCriterias.Columns["YearMthNum"].DefaultValue = 0;
            ObjTASToDoDetCriterias.Columns["YearMthDayNum"].DefaultValue = 0;
            ObjTASToDoDetCriterias.Columns["YearMthWeek"].DefaultValue = 0;
            ObjTASToDoDetCriterias.Columns["YearMthDay"].DefaultValue = 0;
            ObjTASToDoDetCriterias.Columns["PeriodType"].DefaultValue = 0;
            ObjTASToDoDetCriterias.Columns["PeriodDifference"].DefaultValue = 0;
            ObjTASToDoDetCriterias.Columns["PeriodMth"].DefaultValue = 0;

        }
        private void DetailSubDefaultValue_Set()
        {
            ObjTASToDoDetSubs.Columns["ToDoKey"].DefaultValue = ObjTASToDo.ToDoKey;
        }

        private void _TASToDoList_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!this._isOpenReadOnly)
                this._isDirty = true;
        }     

        //Validation
        private bool Validation_Header(SqlConnection cn)
        {          
            string processOK = GVar.gcPass;
            string errorMsgID = string.Empty;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());         

            try
            {
                //Clear Error in UI
                if (GFunc.IsNE(this.ErrorNotifierHeader_Clear) == false)
                    this.ErrorNotifierHeader_Clear.Invoke(this, e);

                #region Validate ToDoKeyfor New Record or existing record
                //if (this.IsNew)
                //    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.ToDoKey, "ToDoKey", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                //else
                //    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.ToDoKey, "ToDoKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                #endregion

                #region Validation Process
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.ToDoDes, "ToDoDes", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.ToDoPriority, "ToDoPriority", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.ToDoType, "ToDoType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.RecurType, "RecurType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.RecurIntDayNum, "RecurIntDayNum", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.RecurIntWeekNum, "RecurIntWeekNum", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.RecurIntWeekDay, "RecurIntWeekDay", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.RecurIntMthNum, "RecurIntMthNum", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.RecurIntMthDayNum, "RecurIntMthDayNum", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.RecurIntMthWeek, "RecurIntMthWeek", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.RecurIntMthDay, "RecurIntMthDay", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.RecurIntYearNum, "RecurIntYearNum", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.RecurIntYearDayNum, "RecurIntYearDayNum", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.RecurIntYearMthNum, "RecurIntYearMthNum", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.RecurIntYearMthDay, "RecurIntYearMthDay", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.RecurIntYearMthWeek, "RecurIntYearMthWeek", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);

                if(this._TASToDo.ToDoType==50)
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.RepFileNm, "RepFileNm", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.DocDC, "DocDC", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.DocDK, "DocDK", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.DocID, "DocID", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.InActive, "InActive", GEnum.DataType.Boolean, GEnum.Require.No, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._TASToDo.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
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
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
            string msgID = string.Empty;
            bool processOK = true;

            if (grdNm == "tagrdDetSub" || (_TASToDo._toDoType==40 && grdNm=="tagrdDetDocList"))
            {
                if (dt.Rows.Count == 0)
                {                   
                    DocComUtility.InvokeGridNotifier(grdNm, e, this.ErrorNotifierHeader_Set);
                    throw new TAException("Validation detail failed. At least one record must be entered.");
                }
            }

            foreach (DataRow dr in dt.Rows)
            {
                msgID = string.Empty;
                processOK = true;

                if (dr.RowState == DataRowState.Deleted)
                    continue;
                else
                {
                    //Check Column values                   
                    foreach (DataColumn c in dr.Table.Columns)
                    {
                        Validation_DetailCheck(dr, grdNm, dr[c.ColumnName.ToString()], c.ColumnName.ToString(), false, ref processOK, e);
                    }

                    if (processOK && grdNm == "tagrdDetSub")
                        Validation_DetailRelation(dr["UserKey"], false, ref processOK, e);
                    //Set RowError Text
                    if (processOK == false)
                    {
                        dr.RowError = GFunc.PropertyMessage_Merge(e, cn);
                        DocComUtility.InvokeGridNotifier(grdNm, e, this.ErrorNotifierHeader_Set);
                        throw new TAException(BOLib.MsgID.Common.ValidationFail);
                    }                    
                    else
                        dr.RowError = string.Empty;
                }
            }
            return processOK;
        }//Completed
        public bool Validation_Detail(string grdNm, UltraGridRow grdrow, string fieldToCheck)
        {
            //Validation Check for calls from FORM (CustomCellUpdate, BeforeRowUpdate)
            string msgID = string.Empty;
            bool processOK = true;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
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

            if (processOK && grdNm == "tagrdDetSub")
                Validation_DetailRelation(drow["UserKey"], grdrow.IsAddRow, ref processOK, e);

            //Set RowError Text
            if (processOK == false)
            {
                ((DataRowView)(grdrow.ListObject)).Row.RowError = GFunc.PropertyMessage_Merge(e);              
                throw new TAException(BOLib.MsgID.Common.ValidationFail);
            }
            else
                ((DataRowView)(grdrow.ListObject)).Row.RowError = string.Empty;

            return processOK;
        }//Completed
        public bool Validation_DetailCheck(DataRow drow, string grdNm, object propValue, string CheckNm, bool failonError, ref bool processOK, UINotifierEventArgs e)
        {
            switch (grdNm)
            {
                #region tagrdDetSub Validation
                case "tagrdDetSub":
                    BaseUtility.Validation(propValue, "ToDoKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "UserKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "Email", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e); 
                    break;
                #endregion

                #region tagrdDetDocList Validation
                case "tagrdDetDocList":
                    BaseUtility.Validation(propValue, "ToDoKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "DocDC", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "DocDK", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "Custom1", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "Custom2", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "Custom3", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                    break;
                #endregion
            }
            return processOK;
        }//Completed       

        public bool Validation_DetailRelation(object propValue, bool IsAddRow, ref bool processOK, UINotifierEventArgs e)
        {
            bool errorFound = false;

            var dupList = _TASToDoDetSubs.AsEnumerable().ToList().FindAll(o =>
                            (o.Field<int?>("UserKey").Value == ((int?)propValue).Value));

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
        }//Completed

        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _TASToDo, _TASToDoDetCriterias, _TASToDoDetSubs }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _TASToDo, _TASToDoDetCriterias, _TASToDoDetSubs }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }          
    }
}
