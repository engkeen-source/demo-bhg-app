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
    public class REFUOMFactory : CommandBase
    {
        #region Member variables and constants

        private REFUOM _REFUOM = null;
        private REFUOMDetItms _REFUOMDetItms = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;  //For future use
        private bool _isNew = false;
        private bool _isReadOnly = false;
        private int _guID = 0;

        //System Code Key for this Factory.
        private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.UOM;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        //Permission ID for this Factory.
        private const string constPermID = GVar.PermissionID.Unit_of_Measure;
        
        public string PermID { get { return constPermID; } }

        //Event Declaration
        public GVar.DirtyEvent dirtyEvent = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;

        #endregion

        #region Factory Properties
        public REFUOM ObjREFUOM
        {
            get
            {
                return this._REFUOM;
            }
        }
        public REFUOMDetItms ObjREFUOMDetItms
        {
            get
            {
                return this._REFUOMDetItms;
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
        public REFUOMFactory(GEnum.InstanceMode instanceMode)
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
                    if (SECPermUtility.Any(constPermID, out this._isReadOnly, true)==false)
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
                            this._REFUOM = new REFUOM();
                            this._REFUOMDetItms = new REFUOMDetItms(cn);
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
            BOLib.REFUOM copyREFUOM = null;
            BOLib.REFUOMDetItms copyREFUOMDetItms = null;
            #endregion

            try
            {
                 #region Make backup of objects for restore purpose
                    if (this._REFUOM != null)
                        copyREFUOM = this._REFUOM.Clone();

                    if (this._REFUOMDetItms != null)
                        copyREFUOMDetItms = GFunc.TACopyDataTable(_REFUOMDetItms); 
                    #endregion

                #region Check Security Permission 
                if (SECPermUtility.Any(constPermID, out this._isReadOnly, true)==false)
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
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey)==false)
                            return false;

                        //prepare new instance           
                        this._REFUOM = REFUOM.New();
                        this._REFUOMDetItms = new REFUOMDetItms(cn);

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;

                        //Attach Events
                        this._REFUOM.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Obj_PropertyChanged);
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
                    this._REFUOM = copyREFUOM;
                    this._REFUOMDetItms = copyREFUOMDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFUOM = null;
                copyREFUOMDetItms = null;
                #endregion
            }
        }//Completed
        public bool GetEdit(int? UOMKey)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.REFUOM copyREFUOM = null;
            BOLib.REFUOMDetItms copyREFUOMDetItms = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFUOM != null)
                    copyREFUOM = this._REFUOM.Clone();

                if (this._REFUOMDetItms != null)
                    copyREFUOMDetItms = GFunc.TACopyDataTable(_REFUOMDetItms);
                #endregion

            
                #region Check Security Permission 
                if (SECPermUtility.Edit(constPermID, true)==false)
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
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, UOMKey, 0, _guID))
                            return false;

                        //Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey)==false)
                            return false;

                        //Add Lock
                        if (SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, UOMKey)==false)
                            return false;

                        //Get Record                                 
                        if (this._REFUOM.Fetch(cn, new REFUOM.Criteria(UOMKey, 1))==false)
                        {
                            MsgBox.Show(cn,MsgID.Common.GetFail); 
                            return false;
                        }

                        //Record Not Found
                        if (GFunc.NEInt(this._REFUOM._uOMKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }


                        _REFUOMDetItms.Clear();
                        if (_REFUOMDetItms.Fetch(cn, new REFUOMDetItms.Criteria(UOMKey, 1))==false)
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
                    this._REFUOM = copyREFUOM;
                    this._REFUOMDetItms = copyREFUOMDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFUOM = null;
                copyREFUOMDetItms = null;
                #endregion
            }
        }//Completed
        public bool GetReadOnly(int? UOMKey)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.REFUOM copyREFUOM = null;
            BOLib.REFUOMDetItms copyREFUOMDetItms = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFUOM != null)
                    copyREFUOM = this._REFUOM.Clone();

                if (this._REFUOMDetItms != null)
                    copyREFUOMDetItms = GFunc.TACopyDataTable(_REFUOMDetItms);
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
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey)==false)
                            return false;

                        //Get record
                        if (this._REFUOM.Fetch(cn, new REFUOM.Criteria(UOMKey, 1))==false)
                        {
                            MsgBox.Show(cn,MsgID.Common.GetFail); 
                            return false;
                        }
                        _REFUOMDetItms.Clear();
                        if (this._REFUOMDetItms.Fetch(cn, new REFUOMDetItms.Criteria(UOMKey, 1))==false)
                        {
                            MsgBox.Show(cn,MsgID.Common.GetFail); 
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
                    this._REFUOM = copyREFUOM;
                    this._REFUOMDetItms = copyREFUOMDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFUOM = null;
                copyREFUOMDetItms = null;
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

                        if (GFunc.IsNE(_REFUOM))
                            _REFUOM = REFUOM.New();
                        GFunc.ConvertDataTableToObject(dtHeader, _REFUOM);

                        _REFUOMDetItms = new REFUOMDetItms(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[0], _REFUOMDetItms);

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
            int? newUOMKey = 0;
            string autoID = string.Empty;
            BOLib.REFUOM copyREFUOM = null;
            BOLib.REFUOMDetItms copyREFUOMDetItms = null;

            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFUOM != null)
                    copyREFUOM = this._REFUOM.Clone();

                if (this._REFUOMDetItms != null)
                    copyREFUOMDetItms = GFunc.TACopyDataTable(_REFUOMDetItms);

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
                        if (SECPermUtility.Add(constPermID, true)==false)
                            return false;
                    }
                    else
                    {   
                        if (SECPermUtility.Edit(constPermID, true)==false)
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
                        if (this.IsNew && GFunc.IsNE(_REFUOM._uOMID))
                        {
                            if (SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _REFUOM._uOMShw)==false)
                                return false;

                            _REFUOM._uOMID = autoID;
                        }
                        #endregion

                        #region Set default value for fields that cannot be empty but can have a general default value
                        //Get Server Date and Time (sdt)
                        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                        _REFUOM._createDate = GFunc.NEDateTime(_REFUOM.CreateDate, svrDateTime);//-----Albert
                        _REFUOM._createUserKey = GFunc.NEInt(_REFUOM.CreateUserKey, AppInfor.currentUserKey);//-----Albert
                        _REFUOM._lastModifiedDate = svrDateTime;//-----Albert
                        _REFUOM._lastModifiedUserKey = AppInfor.currentUserKey;//-----Albert
                        #endregion

                        #region Validation
                        if (Validation_Header(cn)==false)
                            return false;
                        if (Validation_Detail(cn)==false)
                            return false;
                        #endregion
                        
                        #region Save Record
                        //Note: there is no delete permission for Sales Rep Payroll (only Read,Edit)
                        if (IsNew)
                        {
                            if (_REFUOM.Insert(cn, out newUOMKey)==false)
                            {
                                MsgBox.Show(cn,MsgID.Common.SaveFail); 
                                return false;
                            }
                           
                            if (_REFUOMDetItms.Insert(cn, newUOMKey)==false)
                            {
                                MsgBox.Show(cn, MsgID.Common.SaveFail); 
                                return false;
                            }                           
                        }
                        else
                        {
                            if (_REFUOM.Update(cn)==false)
                            {
                                MsgBox.Show(cn, MsgID.Common.SaveFail); 
                                return false;
                            }
                            
                            if (_REFUOMDetItms.Delete(cn, new REFUOMDetItms.Criteria(_REFUOM._uOMKey, 0, 0))==false)  
                            {
                                MsgBox.Show(cn, MsgID.Common.SaveFail);
                                return false;
                            }
                            if (_REFUOMDetItms.Insert(cn, _REFUOM._uOMKey)==false)
                            {
                                MsgBox.Show(cn, MsgID.Common.SaveFail);
                                return false;
                            }                       
                        }
                        #endregion

                        #region For New Record perform: Locking, set new recordKey
                        if (IsNew)
                        {
                            if (SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newUOMKey))
                                 _REFUOM._uOMKey = newUOMKey;
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
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _REFUOM.UOMKey, _REFUOM.UOMID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _REFUOM, _REFUOMDetItms });
                else
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _REFUOM.UOMKey, _REFUOM.UOMID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _REFUOM, _REFUOMDetItms });
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
                    this._REFUOM = copyREFUOM;
                    this._REFUOMDetItms = copyREFUOMDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFUOM = null;
                copyREFUOMDetItms = null;
                #endregion
            }
        }//Completed
        public bool Delete()
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.REFUOM copyREFUOM = null;
            BOLib.REFUOMDetItms copyREFUOMDetItms = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFUOM != null)
                    copyREFUOM = this._REFUOM.Clone();

                if (this._REFUOMDetItms != null)
                    copyREFUOMDetItms = GFunc.TACopyDataTable(_REFUOMDetItms);
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
                        if (SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _REFUOM._uOMKey, GUID)==false)
                            return false;

                        //Check the record is used in other dependency tables
                        if (GFunc.CheckKeyDependantsExists(cn, "UOMKey", _REFUOM._uOMKey.Value, _REFUOM._uOMID))
                            return false;

                        //Check for Option Table
                        if (GFunc.CheckKeyDependcyinOptionTable(cn, "UOM", _REFUOM._uOMKey.Value))
                            return false;
                        //Delete Record
                        if (_REFUOM.Delete(cn, new REFUOM.Criteria(_REFUOM._uOMKey))==false)
                        {
                            MsgBox.Show(cn,MsgID.Common.DeleteFail); 
                            return false;
                        }

                        //Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey)==false)
                            return false;

                        //Create New
                        this._REFUOM = REFUOM.New();
                        if (this._REFUOMDetItms.Fetch(cn, new REFUOMDetItms.Criteria(0, 0, 1))==false)
                            throw new TAException(MsgID.Common.DeleteFail);

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
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyREFUOM.UOMKey, copyREFUOM.UOMID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyREFUOM, copyREFUOMDetItms });                

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
                    this._REFUOM = copyREFUOM;
                    this._REFUOMDetItms = copyREFUOMDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFUOM = null;
                copyREFUOMDetItms = null;
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
                if (GFunc.IsNE(this.ErrorNotifierHeader_Clear)==false)
                    this.ErrorNotifierHeader_Clear.Invoke(this, e);

                #region Validation for each Field
                if (this.IsNew)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFUOM._uOMKey, "UOMKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFUOM._uOMID, "UOMID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                }
                else
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFUOM.UOMKey, "UOMKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFUOM.UOMID, "UOMID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                }

                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFUOM.UOMShw, "UOMShw", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFUOM.UOMType, "UOMType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFUOM.GramRate, "GramRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFUOM.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFUOM.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFUOM.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);

                #endregion

                if (e.PropertyMessage.Count == 0)
                {
                    //StoreProcedure Validation
                    if (_REFUOM.Validation(cn, new REFUOM.Criteria(_REFUOM._uOMKey, _REFUOM._uOMID,1), this.IsNew))
                    {
                        return true;
                    }
                    else
                    {
                        e.PropertyMessage.Add("UOMID", SysMessageUtility.Get(cn, MsgID.Validation.DuplicateRecordID + "UOMID"));
                        if (GFunc.IsNE(this.ErrorNotifierHeader_Set) == false)
                            this.ErrorNotifierHeader_Set.Invoke(this, e);

                        return false;
                    }
                }
                else
                {
                    if (GFunc.IsNE(this.ErrorNotifierHeader_Set)== false)
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
                foreach (DataRow dr in this._REFUOMDetItms.Rows)
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
                            Validation_DetailRelation(dr["UOMConKey"], false, ref processOK, e);

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
                    Validation_DetailRelation(grdrow.Cells["UOMConKey"].Value, grdrow.IsAddRow, ref processOK, e);
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
                BaseUtility.Validation(propValue, "UOMConKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "UOMConRate", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "Custom1", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "Custom2", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "Custom3", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
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
                var dupList = ObjREFUOMDetItms.AsEnumerable().ToList().FindAll(o =>
                                (o.Field<int?>("UOMConKey").Value == ((int?)propValue)));

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
                    e.PropertyMessage.Add("rowError", "UOMConKey" + MsgID.Validation.DuplicateRecord);
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
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _REFUOM, _REFUOMDetItms }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _REFUOM, _REFUOMDetItms }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

       /*
        #region Member variables and constants

        private REFUOM _REFUOM = null;
        private REFUOMs _REFUOMs = null;
        private REFUOMDetItm _REFUOMDetItm = null;
        private REFUOMDetItms _REFUOMDetItms = null;
        private GEnum.InstanceMode _instanceMode = 0;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isOpenReadOnly = false;
        private int _guID = 0;

        // System Code Key for this Factory.
        private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.UOM;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        // Permission ID for this Factory.
        public const string constPermID = GVar.PermissionID.Unit_of_Measure;

        public GVar.ReadOnlyEvent readonlyEvent = null;
        public GVar.ErrorEvent errorEvent = null;
        public GVar.ListErrorEvent listErrorEvent = null;

        //  Custom Event Declaration
        public GVar.UINotifierEvent UOMNotifier = null;
        public GVar.UINotifierEvent clearErrorNotifier = null;

        #endregion // Member variables and constant

        #region Factory Properties

        public REFUOM ObjREFUOM
        {
            get
            {
                return this._REFUOM;
            }
        }

        public REFUOMs ObjREFUOMs
        {
            get
            {
                return this._REFUOMs;
            }
        }

        public REFUOMDetItm ObjREFUOMDetItm
        {
            get
            {
                return this._REFUOMDetItm;
            }
        }

        public REFUOMDetItms ObjREFUOMDetItms
        {
            get
            {
                return this._REFUOMDetItms;
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
        public REFUOMFactory(GEnum.InstanceMode instanceMode)
        {
            Initialisation(instanceMode);
        }

        #endregion // Constructors

        #region Initialization Method

        public bool Initialisation(GEnum.InstanceMode instanceMode)
        {
            bool isInitialisation = false;
            string msgID = MsgID.Common.InitialisationFail;
            // bool processOK = true;

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

        public bool GetEdit(int? uomKey)
        {
            // Initialisation
            bool isGetEdit = false;
            string msgID = MsgID.Common.GetFail;

            // Copy original object
            BOLib.REFUOM copyREFUOM = null;
            BOLib.REFUOMDetItms copyREFUOMDetItms = null;

            if (!GFunc.IsNE(this._REFUOM))
                copyREFUOM = this._REFUOM.Clone();

            if (!GFunc.IsNE(this._REFUOMDetItms))
                copyREFUOMDetItms = this._REFUOMDetItms;
            else
            {
                this._REFUOMDetItms = new REFUOMDetItms();
                this._REFUOMDetItms.ColumnChanged += new DataColumnChangeEventHandler(Details_CollectionChanged);
                this._REFUOMDetItms.RowDeleted += new DataRowChangeEventHandler(_REFUOMDetItms_RowDeleted);
            }
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
                            if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, uomKey, 0, _guID))
                                return false;

                            // Remove Lock
                            if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                                return false;

                            // Add Lock
                            if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, uomKey))
                                return false;

                            if (_REFUOM == null)
                            {
                                _REFUOM = new REFUOM();
                            }

                            if (!_REFUOM.Fetch(cn, new REFUOM.Criteria(uomKey, 1)))
                            {
                                MsgBox.Show(cn, msgID);
                                return false;
                            }

                            //Record Not Found
                            if (GFunc.NEInt(this._REFUOM._uOMKey, 0) == 0)
                            {                                
                                throw new TAException(MsgID.Common.GetFail);
                            }


                            if (_REFUOMDetItms == null)
                            {
                                _REFUOMDetItms = new REFUOMDetItms(cn);
                            }
                            _REFUOMDetItms.Clear();

                            if (!_REFUOMDetItms.Fetch(cn, new REFUOMDetItms.Criteria(uomKey, 0, 1)))
                            {
                                MsgBox.Show(cn, msgID);
                                return false;
                            }

                            this._REFUOM.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(REFUOM_PropertyChanged);

                            // Commit Process                           
                            this._isNew = false;
                            this._isOpenReadOnly = false;
                            msgID = string.Empty;
                            isGetEdit = true;

                            // No errors - commit transaction
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                            // Set Null to Backup Objects
                            copyREFUOM = null;
                            copyREFUOMDetItms = null;
                            _isDirty = false;
                        }
                    }
                }
                catch (TAException tex)
                {
                    throw Error(tex);
                }
                catch (Exception ex)
                {
                    // Restore data
                    this._REFUOM = copyREFUOM;
                    this._REFUOMDetItms = copyREFUOMDetItms;
                    throw Error(ex);
                }
            }
            else
            {
                MsgBox.Show(MsgID.Common.WrongInstanceMode);
                return false;
            }
            return isGetEdit;
        }

        void _REFUOMDetItms_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            _isDirty = true;
        }

        #endregion //GetEdit Method

        #region GetReadOnly Method

        public bool GetReadOnly(int? uomKey)
        {
            bool isGetReadOnly = false;
            string msgID = MsgID.Common.GetFail;

            // Copy original object
            BOLib.REFUOM copyREFUOM = null;
            BOLib.REFUOMDetItms copyREFUOMDetItms = null;

            if (!GFunc.IsNE(this._REFUOM))
                copyREFUOM = this._REFUOM.Clone();

            if (!GFunc.IsNE(this._REFUOMDetItms))
                copyREFUOMDetItms = this._REFUOMDetItms;

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

                            // Get Record                                 
                            if (!_REFUOM.Fetch(cn, new REFUOM.Criteria(uomKey, 1)))
                            {
                                MsgBox.Show(msgID);
                                return false;
                            }

                            if (_REFUOMDetItms == null)
                                _REFUOMDetItms = new REFUOMDetItms(cn);
                            _REFUOMDetItms.Clear();

                            if (!_REFUOMDetItms.Fetch(cn, new REFUOMDetItms.Criteria(uomKey, 0, 1)))
                            {
                                MsgBox.Show(msgID);
                                return false;
                            }

                            this._isNew = false;
                            this._isOpenReadOnly = true;
                            msgID = string.Empty;
                            isGetReadOnly = true;

                            // No errors - commit transaction
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                            // Set Null to Backup Objects
                            copyREFUOM = null;
                            copyREFUOMDetItms = null;

                            _isDirty = false;
                        }
                    }
                }
                catch (TAException tex)
                {
                    throw Error(tex);
                }
                catch (Exception ex)
                {
                    //Restore Data
                    this._REFUOM = copyREFUOM;
                    this._REFUOMDetItms = copyREFUOMDetItms;
                    throw Error(ex);
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

            BOLib.REFUOM copyREFUOM = null;
            BOLib.REFUOMDetItms copyREFUOMDetItms = null;

            // Copy original object
            if (!GFunc.IsNE(this._REFUOM))
                copyREFUOM = this._REFUOM.Clone();

            if (!GFunc.IsNE(this._REFUOMDetItms))
                copyREFUOMDetItms = this._REFUOMDetItms;

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

                            // Call New for Header
                            this._REFUOM = REFUOM.New();


                            this._REFUOMDetItms = new REFUOMDetItms(cn);
                            this._REFUOMDetItms.Columns[2].DefaultValue = 1; // Default Value For Tax Rate

                            // Call New for Detail                       
                            if (!this._REFUOMDetItms.Fetch(cn, new REFUOMDetItms.Criteria(0, 1)))
                            {
                                MsgBox.Show(msgID);
                                return false;
                            }
                            this._REFUOM.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(REFUOM_PropertyChanged);
                            this._REFUOMDetItms.ColumnChanged += new DataColumnChangeEventHandler(Details_CollectionChanged);

                            this._isDirty = false;
                            this._isNew = true;
                            this._isOpenReadOnly = false;
                            msgID = string.Empty;
                            isNew = true;

                            // No errors - commit transaction
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                            // Set Null to Backup Objects
                            copyREFUOM = null;
                            copyREFUOMDetItms = null;
                        }
                    }
                    // Audit Log
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.NA, constCodeKey, new List<object>(new object[] { _REFUOMs, _REFUOMDetItms }));
                }
                catch (TAException tex)
                {
                    throw Error(tex);
                }
                catch (Exception ex)
                {
                    // Restore data when error is occur                    
                    this._REFUOM = copyREFUOM;
                    this._REFUOMDetItms = copyREFUOMDetItms;
                    throw Error(ex);
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
            int? newUOMKey = 0;
            string autoID = string.Empty;

            bool isCommitTransFail = true;
            string recordID = string.Empty;

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

                            recordID = _REFUOM._uOMID;

                            // Get AutoID                           
                            if (isNewRecord && GFunc.IsNE(_REFUOM._uOMID))
                            {
                                if (!SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _REFUOM._uOMShw))
                                    return false;
                                _REFUOM.UOMID = autoID;
                            }

                            #region Set Server DateTime If Create and Modified Date is null
                            //Get Server Date and Time (sdt)
                            DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                            //Set Header Obj
                            _REFUOM.CreateDate = GFunc.NEDateTime(_REFUOM.CreateDate, svrDateTime);
                            _REFUOM.CreateUserKey = GFunc.NEInt(_REFUOM.CreateUserKey, AppInfor.currentUserKey);

                            _REFUOM.LastModifiedDate = svrDateTime;
                            _REFUOM.LastModifiedUserKey = AppInfor.currentUserKey;
                            //_REFUOMDetItm._uOMKey = _REFUOM._uOMKey;

                            //Set Detail DataTable
                            //_REFBrandDetItms
                            foreach (DataRow dr in _REFUOMDetItms.Rows)
                            {
                                dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
                                dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
                                dr["LastModifiedDate"] = svrDateTime;
                                dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
                            }
                            #endregion

                            // Validation
                            if (!this.UOM_Validation(cn))
                            { return false; }

                            //if (!this.UOMList_Validation(cn))
                            //{ return false; }

                            // Save Header Record
                            if (isNewRecord)
                            {
                                if (!_REFUOM.Insert(cn, out newUOMKey))
                                    return false;

                                if (!_REFUOMDetItms.Insert(cn, newUOMKey))
                                    return false;
                            }
                            else
                            {
                                if (!_REFUOM.Update(cn))
                                    return false;
                                if (!_REFUOMDetItms.Delete(cn, new REFUOMDetItms.Criteria(_REFUOM._uOMKey, 0, 1)))
                                    return false;
                                if (!_REFUOMDetItms.Insert(cn, _REFUOM._uOMKey))
                                    return false;
                            }

                            // Record Locking
                            if (isNewRecord)
                            {
                                if (!SysLockUtility.AddLock(cn, true, _guID, constCodeKey, newUOMKey))
                                    return false;
                            }

                            if (isNewRecord)
                                _REFUOM._uOMKey = newUOMKey;
                            this._isNew = false;
                            msgID = string.Empty;
                            isSave = true;

                            // No errors - commit transaction
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                            isCommitTransFail = false;
                            _isDirty = false;

                        }// End of SqlConnection
                    }// End of TransactionScope

                    // Audit Log
                    if (isNewRecord)
                        SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, new object[] { _REFUOMs, _REFUOMDetItms }));
                    else
                        SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, new object[] { _REFUOMs, _REFUOMDetItms }));
                }
                catch (TAException tex)
                {
                    throw Error(tex);
                }
                catch (Exception ex)
                {
                    if (isNewRecord)
                        // Restore the auto generated ID
                        this._REFUOM._uOMID = recordID;
                    if (isCommitTransFail)
                        throw new TAException(MsgID.Validation.CommitTransFail);
                    throw Error(ex);
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
            // Copy original object
            //BOLib.REFUOM copyREFUOM = this._REFUOM.Clone();
            //BOLib.REFUOMDetItms copyREFUOMDetItms = this._REFUOMDetItms.Copy();

            if (this.InstanceMode == GEnum.InstanceMode.Normal)
            {
                try
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

                            // Record Locking
                            if (!SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _REFUOM._uOMKey, GUID))
                                return false;

                            // Check the record is used in other dependency tables
                            if (GFunc.CheckKeyDependantsExists(cn, "UOMKey", _REFUOM._uOMKey.Value, _REFUOM._uOMID))
                                return false;

                            //Check for Option Table
                            //if (GFunc.CheckKeyDependcyinOptionTable(cn, "UOM", _REFUOM._uOMKey.Value))
                            //    return false;

                            // Delete Record
                            if (!_REFUOM.Delete(cn, new REFUOM.Criteria(_REFUOM._uOMKey)))
                                return false;

                            // Remove Lock
                            if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                                return false;

                            // Create New
                            this._REFUOM = REFUOM.New();
                            this._REFUOMDetItms = new REFUOMDetItms(cn);

                            this._isNew = true;
                            this._isOpenReadOnly = false;
                            msgID = string.Empty;
                            isDelete = true;

                            // No errors - commit transaction
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                            ////Set null to backup objects
                            //copyREFUOM = null;
                            //copyREFUOMDetItms = null;
                        }// End of SqlConnection
                    }// End of TransactionScope
                    // AuditLog
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, new object[] { _REFUOMs, _REFUOMDetItms }));
                }
                catch (TAException tex)
                {
                    throw Error(tex);
                }
                catch (Exception ex)
                {
                    // Restore data
                    //this._REFUOM = copyREFUOM;
                    //this._REFUOMDetItms = copyREFUOMDetItms;
                    throw Error(ex);
                }
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

        private bool UOM_Validation(SqlConnection cn)
        {
            bool isValidation = true;
            string msgID = BOLib.MsgID.Common.ValidationFail;

            string processOK = GVar.gcPass;
            string errorMsgID = string.Empty;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());

            if (this.InstanceMode == GEnum.InstanceMode.Normal)
            {
                // Clear Error in UI
                if (!GFunc.IsNE(this.clearErrorNotifier))
                    this.clearErrorNotifier.Invoke(this, e);

                //MsgBox Error
                if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFUOM.UOMKey, "UOMKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn) != GVar.gcPass)
                {
                    ErrorMessageID = errorMsgID;
                    return false;
                }
                if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFUOM.CreateDate, "CreateDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn) != GVar.gcPass)
                {
                    ErrorMessageID = errorMsgID;
                    return false;
                }
                if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFUOM.CreateUserKey, "CreateUserKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, 0, int.MaxValue, e, cn) != GVar.gcPass)
                {
                    ErrorMessageID = errorMsgID;
                    return false;
                }
                if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFUOM.LastModifiedDate, "LastModifiedDate", GEnum.DataType.DateTime, GEnum.Require.No, null, GEnum.CompareOperator.NotEqual, null, null, null, e, cn) != GVar.gcPass)
                {
                    ErrorMessageID = errorMsgID;
                    return false;
                }
                if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFUOM.LastModifiedUserKey, "LastModifiedUserKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, 0, int.MaxValue, e, cn) != GVar.gcPass)
                {
                    ErrorMessageID = errorMsgID;
                    return false;
                }

                //Error Provider   
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFUOM.UOMID, "UOMID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFUOM.UOMShw, "UOMShw", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFUOM.UOMType, "UOMType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFUOM.GramRate, "GramRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFUOM.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFUOM.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFUOM.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);

                if (e.PropertyMessage.Count > 0)
                {
                    isValidation = false;

                    ErrorMessageID = MsgID.Common.ValidationFail;

                    if (!GFunc.IsNE(this.UOMNotifier))
                        this.UOMNotifier.Invoke(this, e);
                    return false;
                }
                else
                    isValidation = true;

                // StoreProcedure Validation
                if (processOK == GVar.gcPass)
                {
                    if (this._REFUOM.Validation(cn, new REFUOM.Criteria(this._REFUOM._uOMKey, this._REFUOM._uOMID, 0), this.IsNew))
                    {
                        msgID = string.Empty;
                    }
                    else
                    {
                        ErrorMessageID = MsgID.Validation.DuplicateRecordID + "UOMID";
                        e.PropertyMessage.Add("UOMID", SysMessageUtility.Get(cn, ErrorMessageID));
                        if (!GFunc.IsNE(this.UOMNotifier))
                            this.UOMNotifier.Invoke(this, e);
                        return false;
                    }
                }

                //If validation pass in header, then check Detail Errors
                if (isValidation)
                {
                    if (!RefDetItm_Validation(cn, null))
                    {
                        ErrorMessageID = MsgID.Common.RecordDetailValidationFail;
                        throw new TAException(ErrorMessageID);
                    }

                }
                if (isValidation)
                {

                    List<DataRow> objs = this._REFUOMDetItms.AsEnumerable().ToList().FindAll(s => s.RowError.ToString() != string.Empty);

                    if (objs.Count > 0)
                    {
                        ErrorMessageID = MsgID.Common.RecordDetailValidationFail;
                        throw new TAException(ErrorMessageID);

                    }
                }
            }
            return isValidation;
        }

        private bool UOMList_Validation(SqlConnection cn)
        {
            bool isValidation = true;
            string processOK = GVar.gcPass;
            string errorMsgID = string.Empty;
            string msgValue = string.Empty;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());

            try
            {
                foreach (DataRow dr in ObjREFUOMDetItms.Rows)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, dr["UOMConKey"], "UOMConKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, dr["UOMConRate"], "UOMConRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, dr["Custom1"], "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, dr["Custom2"], "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, dr["Custom3"], "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);

                    if (e.PropertyMessage.Count > 0)
                    {
                        foreach (object key in e.PropertyMessage.Keys)
                        {
                            if (!GFunc.IsNE(msgValue))
                                msgValue += " and ";

                            msgValue += e.PropertyMessage[key];
                            isValidation = false;
                        }
                        GFunc.SetRowError(dr, msgValue);
                        processOK = GVar.gcCancel;
                        throw new TAException(BOLib.MsgID.Common.ValidationFail);
                    }
                    else
                    {
                        dr.RowError = string.Empty;
                        isValidation = true;
                    }

                    //// StoreProcedure Validation
                    //if (processOK == GVar.gcPass)
                    //{
                    //    int? a = Convert.ToInt16(dr["UOMConKey"]);
                    //    if (this._REFUOMDetItm.Validation(cn, new REFUOMDetItm.Criteria(this._REFUOMDetItm._uOMKey, a, 0), this.IsNew))
                    //    {
                    //        errorMsgID = string.Empty;
                    //    }
                    //    else
                    //    {
                    //        ErrorMessageID = MsgID.Validation.DuplicateDetailIDUOMConKey + "UOMConKey" ;
                    //        e.PropertyMessage.Add("tagrdUOMDetailList", SysMessageUtility.Get(cn, ErrorMessageID));
                    //        if (!GFunc.IsNE(this.UOMDetailNotifier))
                    //            this.UOMDetailNotifier.Invoke(this, e);
                    //        return false;
                    //    }
                    //}
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

        public bool RefDetItm_Validation(DataRow CheckRow)
        {
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                cn.Open();
                return RefDetItm_Validation(cn, CheckRow);
            }
        }

        public string RefDetItm_Validation(SqlConnection cn, string CellKey, object Value, out string errorMsgID, UINotifierEventArgs e)
        {
            string processOK = GVar.gcPass;
            errorMsgID = string.Empty;

            switch (CellKey)
            {
                case "UOMConKey":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "UOMConKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    break;
                case "UOMConRate":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "UOMConRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    break;

                //case "TaxRate":
                //    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "TaxRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                //    break;

                case "Custom1":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    break;

                case "Custom2":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    break;

                case "Custom3":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    break;
            }
            return processOK;
        }

        public bool RefDetItm_Validation(SqlConnection cn, DataRow CheckRow)
        {
            #region Declaration
            string processOK = GVar.gcPass;
            string msgValue = string.Empty;
            string errorMsgID = string.Empty;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
            int KeyCount = 0;
            #endregion

            foreach (DataRow drs in this.ObjREFUOMDetItms.Rows)
            {
                DataRow dr = drs;
                if (CheckRow != null)
                {
                    dr = CheckRow;
                }

                #region Common Validation

                foreach (DataColumn dc in _REFUOMDetItms.Columns)
                {
                    processOK = RefDetItm_Validation(cn, dc.ColumnName, dr[dc.ColumnName], out errorMsgID, e);
                }
                #endregion


                #region Additional Validation check
                if (e.PropertyMessage.Count > 0)
                {
                    processOK = GVar.gcCancel;
                }
                else
                {
                    // Check for duplicate EffDate
                    KeyCount = 0;
                    KeyCount = ObjREFUOMDetItms.AsEnumerable().Count(p => p.Field<int>("UOMConKey") == (int)dr["UOMConKey"]);

                    if (KeyCount > 1)
                    {
                        processOK = GVar.gcCancel;
                        e.PropertyMessage.Add("UOMConKey", SysMessageUtility.Get(cn, "UOMConKey" + MsgID.Validation.DuplicateRecord));
                    }
                }
                #endregion

                #region Assign error message to display in grid

                if (e.PropertyMessage.Count > 0)
                {
                    foreach (object key in e.PropertyMessage.Keys)
                    {
                        if (!GFunc.IsNE(msgValue))
                            msgValue += " and ";

                        msgValue += e.PropertyMessage[key];
                    }
                    GFunc.SetRowError(dr, msgValue);

                    processOK = GVar.gcCancel;
                }
                else
                {
                    dr.RowError = string.Empty;
                    processOK = GVar.gcPass;
                }
                #endregion

                if (CheckRow != null)
                {
                    if (processOK == GVar.gcPass)
                        return true;
                    else
                    {
                        return false;
                    }
                }

            }

            if (processOK == GVar.gcPass)
                return true;
            else
            {
                return false;
            }

        }


        #endregion //Validation Method

        #region Dispose Method

        public bool Dispose()
        {
            bool isDispose = false;
            string msgID = string.Empty;

            if (this.InstanceMode == GEnum.InstanceMode.Normal)
            {
                if (SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0))
                    isDispose = true;
            }
            return isDispose;
        }

        #endregion //Dispose Method

        #region PropertyChanged

        private void REFUOM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            string msgID = string.Empty;
            string msgValue = string.Empty;
            bool validateOk = true;

            if (!this._isOpenReadOnly)
            {
                this._isDirty = true;

                //UI Validation
                switch (e.PropertyName)
                {
                    case "UOMID":
                        if (IsNew)
                            validateOk = BaseUtility.Validation(out msgID, this._REFUOM._uOMID, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null);
                        else
                            validateOk = BaseUtility.Validation(out msgID, this._REFUOM._uOMID, e.PropertyName, GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null);
                        break;
                    case "UOMShw":
                        validateOk = BaseUtility.Validation(out msgID, this._REFUOM._uOMShw, e.PropertyName, GEnum.DataType.String, GEnum.Require.Yes, 5, null, null, null, null);
                        break;
                    case "UOMType":
                        validateOk = BaseUtility.Validation(out msgID, this._REFUOM._uOMType, e.PropertyName, GEnum.DataType.Integer, GEnum.Require.Yes, null, null, null, null, null);
                        break;
                    case "GramRate":
                        validateOk = BaseUtility.Validation(out msgID, this._REFUOM._gramRate, e.PropertyName, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null);
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
        //private bool ValidationForDetail(SqlConnection cn)
        //{
        //    //Variable Declaration
        //    string msgID = string.Empty;
        //    string msgValue = string.Empty;
        //    bool processOk = true;

        //    for (int i = 0; i < _REFUOMDetItms.Rows.Count; i++)
        //    {
        //        //if (i == _REFCurrDetItms.Rows.Count  - 1 && GFunc.IsNE(_REFCurrDetItms.Rows[i]["currDate"]))
        //        //    break;
        //        if (_REFUOMDetItms.Rows[i].RowState == DataRowState.Deleted)
        //        {
        //            continue;
        //        }

        //        processOk = BaseUtility.Validation(out msgID, this._REFUOMDetItms.Rows[i]["UOMConKey"], "UOMConKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null);
        //        if (processOk)
        //        {
        //            var dupList = this.ObjREFUOMDetItms.AsEnumerable().ToList().FindAll(o =>
        //                    (o.Field<int?>("UOMConKey") == int.Parse(this._REFUOMDetItms.Rows[i]["UOMConKey"].ToString())));

        //            if (dupList.Count > 1)
        //            {
        //                msgID = "UOMConKey" + MsgID.Validation.DuplicateRecord;
        //                processOk = false;                        
        //                MsgBox.Show(msgID);
        //                return false;

        //            }
        //        }
        //        if (processOk)
        //        {
        //            processOk = BaseUtility.Validation(out msgID, this._REFUOMDetItms.Rows[i]["UOMConRate"], "UOMConRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 1, null, null);
        //        }
        //        if (!processOk)
        //        {

        //            this._REFUOMDetItms.Rows[i].RowError = SysMessageUtility.Get(cn, msgID);
        //        }
        //        else
        //            this._REFUOMDetItms.Rows[i].RowError = string.Empty;

        //        if (!processOk)
        //        {
        //            break;
        //        }
        //        else
        //            this._REFUOMDetItms.Rows[i].RowError = string.Empty;
        //    }
        //    if (!processOk)
        //    {
        //        return processOk;
        //    }

        //    return processOk;
        //}
        void Details_CollectionChanged(object sender, DataColumnChangeEventArgs e)
        {
            _isDirty = true;
        }
        #endregion

        #region Error

        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _REFUOM, _REFUOMDetItm }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _REFUOM, _REFUOMDetItm }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        #endregion*/
    }
}
