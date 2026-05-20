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
    public class REFTaxGrpFactory : CommandBase
    {
        #region Member variables and constants

        private REFTaxGrp _REFTaxGrp = null;
        private REFTaxGrpDetItms _REFTaxGrpDetItms = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;  //For future use
        private bool _isNew = false;
        private bool _isReadOnly = false;
        private int _guID = 0;

        //System Code Key for this Factory.
        private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Tax_Group;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        //Permission ID for this Factory.
        private const string constPermID = GVar.PermissionID.Tax_Group;

        public string PermID { get { return constPermID; } }

        //Event Declaration
        public GVar.DirtyEvent dirtyEvent = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;

        #endregion

        #region Factory Properties
        public REFTaxGrp ObjREFTaxGrp
        {
            get
            {
                return this._REFTaxGrp;
            }
        }
        public REFTaxGrpDetItms ObjREFTaxGrpDetItms
        {
            get
            {
                return this._REFTaxGrpDetItms;
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
        public REFTaxGrpFactory(GEnum.InstanceMode instanceMode)
        {
            this._instanceMode = instanceMode;
            try
            {
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
                            this._REFTaxGrp = new REFTaxGrp();
                            this._REFTaxGrpDetItms = new REFTaxGrpDetItms(cn);
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
            BOLib.REFTaxGrp copyREFTaxGrp = null;
            BOLib.REFTaxGrpDetItms copyREFTaxGrpDetItms = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFTaxGrp != null)
                    copyREFTaxGrp = this._REFTaxGrp.Clone();

                if (this._REFTaxGrpDetItms != null)
                    copyREFTaxGrpDetItms = GFunc.TACopyDataTable(_REFTaxGrpDetItms);
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
                        this._REFTaxGrp = REFTaxGrp.New();
                        this._REFTaxGrpDetItms = new REFTaxGrpDetItms(cn);
                        
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;

                        //Attach Events
                        this._REFTaxGrp.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Obj_PropertyChanged);
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
                    this._REFTaxGrp = copyREFTaxGrp;
                    this._REFTaxGrpDetItms = copyREFTaxGrpDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFTaxGrp = null;
                copyREFTaxGrpDetItms = null;
                #endregion
            }
        }//Completed
        public bool GetEdit(int? TaxGrpKey)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.REFTaxGrp copyREFTaxGrp = null;
            BOLib.REFTaxGrpDetItms copyREFTaxGrpDetItms = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFTaxGrp != null)
                    copyREFTaxGrp = this._REFTaxGrp.Clone();

                if (this._REFTaxGrpDetItms != null)
                    copyREFTaxGrpDetItms = GFunc.TACopyDataTable(_REFTaxGrpDetItms); 
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
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, TaxGrpKey, 0, _guID))
                            return false;

                        //Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey) == false)
                            return false;

                        //Add Lock
                        if (SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, TaxGrpKey) == false)
                            return false;

                        //Get Record                                 
                        if (this._REFTaxGrp.Fetch(cn, new REFTaxGrp.Criteria(TaxGrpKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        //Record Not Found
                        if (GFunc.NEInt(this._REFTaxGrp._taxGrpKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }


                        _REFTaxGrpDetItms.Clear();
                        if (_REFTaxGrpDetItms.Fetch(cn, new REFTaxGrpDetItms.Criteria(TaxGrpKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
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
                {
                    this._REFTaxGrp = copyREFTaxGrp;
                    this._REFTaxGrpDetItms = copyREFTaxGrpDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFTaxGrp = null;
                copyREFTaxGrpDetItms = null;
                #endregion
            }
        }//Completed
        public bool GetReadOnly(int? TaxGrpKey)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.REFTaxGrp copyREFTaxGrp = null;
            BOLib.REFTaxGrpDetItms copyREFTaxGrpDetItms = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFTaxGrp != null)
                    copyREFTaxGrp = this._REFTaxGrp.Clone();

                if (this._REFTaxGrpDetItms != null)
                    copyREFTaxGrpDetItms = GFunc.TACopyDataTable(_REFTaxGrpDetItms); 
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
                        if (this._REFTaxGrp.Fetch(cn, new REFTaxGrp.Criteria(TaxGrpKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }
                        _REFTaxGrpDetItms.Clear();
                        if (this._REFTaxGrpDetItms.Fetch(cn, new REFTaxGrpDetItms.Criteria(TaxGrpKey, 1)) == false)
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
                    this._REFTaxGrp = copyREFTaxGrp;
                    this._REFTaxGrpDetItms = copyREFTaxGrpDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFTaxGrp = null;
                copyREFTaxGrpDetItms = null;
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

                        if (GFunc.IsNE(_REFTaxGrp))
                            _REFTaxGrp = REFTaxGrp.New();
                        GFunc.ConvertDataTableToObject(dtHeader, _REFTaxGrp);

                        _REFTaxGrpDetItms = new REFTaxGrpDetItms(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[0], _REFTaxGrpDetItms);

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
            int? newTaxGrpKey = 0;
            string autoID = string.Empty;
            BOLib.REFTaxGrp copyREFTaxGrp = null;
            BOLib.REFTaxGrpDetItms copyREFTaxGrpDetItms = null;

            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFTaxGrp != null)
                    copyREFTaxGrp = this._REFTaxGrp.Clone();

                if (this._REFTaxGrpDetItms != null)
                    copyREFTaxGrpDetItms = GFunc.TACopyDataTable(_REFTaxGrpDetItms); 

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
                        if (this.IsNew && GFunc.IsNE(_REFTaxGrp._taxGrpID))
                        {
                            if (SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _REFTaxGrp._taxGrpDes) == false)
                                return false;

                            _REFTaxGrp._taxGrpID = autoID;
                        }
                        #endregion

                        #region Set default value for fields that cannot be empty but can have a general default value

                        //Get Server Date and Time (sdt)
                        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                        _REFTaxGrp._createDate = GFunc.NEDateTime(_REFTaxGrp.CreateDate, svrDateTime);
                        _REFTaxGrp._createUserKey = GFunc.NEInt(_REFTaxGrp.CreateUserKey, AppInfor.currentUserKey);
                        _REFTaxGrp._lastModifiedDate = svrDateTime;
                        _REFTaxGrp._lastModifiedUserKey = AppInfor.currentUserKey;
                        #endregion

                        #region Validation
                        if (Validation_Header(cn) == false)
                            return false;
                        if (Validation_Detail(cn) == false)
                            return false;
                        #endregion

                        #region Save Record
                        //Note: there is no delete permission for Sales Rep Payroll (only Read,Edit)
                        if (IsNew)
                        {
                            if (_REFTaxGrp.Insert(cn, out newTaxGrpKey) == false)
                            {
                                MsgBox.Show(cn,MsgID.Common.SaveFail);
                                return false;
                            }

                            if (_REFTaxGrpDetItms.Insert(cn, newTaxGrpKey) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.SaveFail);
                                return false;
                            }
                        }
                        else
                        {
                            if (_REFTaxGrp.Update(cn) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.SaveFail);
                                return false;
                            }

                            if (_REFTaxGrpDetItms.Delete(cn, new REFTaxGrpDetItms.Criteria(_REFTaxGrp._taxGrpKey, 0, 0)) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.SaveFail);
                                return false;
                            }
                            if (_REFTaxGrpDetItms.Insert(cn, _REFTaxGrp._taxGrpKey) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.SaveFail);
                                return false;
                            }
                        }
                        #endregion

                        #region For New Record perform: Locking, set new recordKey
                        if (IsNew)
                        {
                            if (SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newTaxGrpKey))
                                _REFTaxGrp._taxGrpKey = newTaxGrpKey;
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
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _REFTaxGrp.TaxGrpKey, _REFTaxGrp.TaxGrpID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _REFTaxGrp, _REFTaxGrpDetItms });
                else
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _REFTaxGrp.TaxGrpKey, _REFTaxGrp.TaxGrpID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _REFTaxGrp, _REFTaxGrpDetItms });
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
                    this._REFTaxGrp = copyREFTaxGrp;
                    this._REFTaxGrpDetItms = copyREFTaxGrpDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFTaxGrp = null;
                copyREFTaxGrpDetItms = null;
                #endregion
            }
        }//Completed
        public bool Delete()
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.REFTaxGrp copyREFTaxGrp = null;
            BOLib.REFTaxGrpDetItms copyREFTaxGrpDetItms = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFTaxGrp != null)
                    copyREFTaxGrp = this._REFTaxGrp.Clone();

                if (this._REFTaxGrpDetItms != null)
                    copyREFTaxGrpDetItms = GFunc.TACopyDataTable(_REFTaxGrpDetItms); 
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
                        if (SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _REFTaxGrp._taxGrpKey, GUID) == false)
                            return false;

                        //Check the record is used in other dependency tables
                        if (GFunc.CheckKeyDependantsExists(cn, "TaxGrpKey", _REFTaxGrp._taxGrpKey.Value, _REFTaxGrp._taxGrpID))
                            return false;

                        //Check for Option Table
                        if (GFunc.CheckKeyDependcyinOptionTable(cn, "TaxGrp", _REFTaxGrp._taxGrpKey.Value))
                            return false;

                        //Delete Record
                        if (_REFTaxGrp.Delete(cn, new REFTaxGrp.Criteria(_REFTaxGrp._taxGrpKey)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.DeleteFail);
                            return false;
                        }

                        //Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey) == false)
                            return false;

                        //Create New
                        this._REFTaxGrp = REFTaxGrp.New();
                        this._REFTaxGrpDetItms = new REFTaxGrpDetItms(cn);

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
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyREFTaxGrp.TaxGrpKey, copyREFTaxGrp.TaxGrpID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyREFTaxGrp, copyREFTaxGrpDetItms });

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
                    this._REFTaxGrp = copyREFTaxGrp;
                    this._REFTaxGrpDetItms = copyREFTaxGrpDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFTaxGrp = null;
                copyREFTaxGrpDetItms = null;
                #endregion
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
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFTaxGrp._taxGrpKey, "TaxGrpKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFTaxGrp.TaxGrpID, "TaxGrpID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                }
                else
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFTaxGrp.TaxGrpKey, "TaxGrpKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFTaxGrp.TaxGrpID, "TaxGrpID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                }

                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFTaxGrp.TaxGrpDes, "TaxGrpDes", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFTaxGrp.GST, "GST", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFTaxGrp.GSTCustom, "GSTCustom", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFTaxGrp.CurrKey, "CurrKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFTaxGrp.OpenBal, "OpenBal", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFTaxGrp.ReportCode, "ReportCode", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFTaxGrp.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFTaxGrp.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFTaxGrp.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);


                #endregion

                if (e.PropertyMessage.Count == 0)
                {
                    //StoreProcedure Validation
                    if (_REFTaxGrp.Validation(cn, new REFTaxGrp.Criteria(_REFTaxGrp._taxGrpKey, _REFTaxGrp._taxGrpID, 1), this.IsNew))
                    {
                        return true;
                    }
                    else
                    {
                        e.PropertyMessage.Add("TaxGrpID", SysMessageUtility.Get(cn, MsgID.Validation.DuplicateRecordID + "TaxGrpID"));
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
        public bool Validation_Detail(SqlConnection cn)
        {
            //Validation Check for calls from Factory (Save method)
            string msgID = string.Empty;
            bool processOK = true;
            try
            {
                foreach (DataRow dr in this._REFTaxGrpDetItms.Rows)
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
                            Validation_DetailCheck(dr[c.ColumnName.ToString()], c.ColumnName.ToString(), false, ref processOK, e);
                        }

                        //Check for Duplicate records
                        if (processOK)
                            Validation_DetailRelation(dr["TaxKey"], false, ref processOK, e);

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
                if (fieldToCheck == string.Empty)
                {
                    foreach (UltraGridCell c in grdrow.Cells)
                    {
                        Validation_DetailCheck(c.Value, c.Column.Key, false, ref processOK, e);
                    }
                }
                else
                    Validation_DetailCheck(grdrow.Cells[fieldToCheck].Value, fieldToCheck, false, ref processOK, e);

                //Check for Duplicate records when fieldToCheck is Empty (meaning RowBeforeUpdate)
                if (processOK && fieldToCheck == string.Empty)
                {
                    Validation_DetailRelation(grdrow.Cells["TaxKey"].Value, grdrow.IsAddRow, ref processOK, e);
                }

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
        public bool Validation_DetailCheck(object propValue, string CheckNm, bool failonError, ref bool processOK, UINotifierEventArgs e)
        {
            try
            {
                BaseUtility.Validation(propValue, "TaxKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "Custom1", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "Custom2", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "Custom3", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
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
        public bool Validation_DetailRelation(object propValue, bool IsAddRow, ref bool processOK, UINotifierEventArgs e)
        {
            bool errorFound = false;

            try
            {
                var dupList = ObjREFTaxGrpDetItms.AsEnumerable().ToList().FindAll(o =>
                                (o.Field<int?>("TaxKey").Value == (GFunc.NEInt(propValue, 0))));

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
                    e.PropertyMessage.Add("rowError", "TaxKey" + MsgID.Validation.DuplicateRecord);
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
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _REFTaxGrp, _REFTaxGrpDetItms }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _REFTaxGrp, _REFTaxGrpDetItms }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

    }
}

