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
    public class REFBrandFactory : CommandBase
    {
        #region Member variables and constants

        private REFBrand _REFBrand = null;
        private REFBrandDetItms _REFBrandDetItms = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;  //For future use
        private bool _isNew = false;
        private bool _isReadOnly = false;
        private int _guID = 0;

        //System Code Key for this Factory.
        private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Brand;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        //Permission ID for this Factory.
        private const string constPermID = GVar.PermissionID.Brand;
        public string PermID { get { return constPermID; } }

        //Event Declaration
        public GVar.DirtyEvent dirtyEvent = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;

        #endregion

        #region Factory Properties
        public REFBrand ObjREFBrand
        {
            get
            {
                return this._REFBrand;
            }
        }
        public REFBrandDetItms ObjREFBrandDetItms
        {
            get
            {
                return this._REFBrandDetItms;
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
        public REFBrandFactory(GEnum.InstanceMode instanceMode)
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
                            this._REFBrand = new REFBrand();
                            this._REFBrandDetItms = new REFBrandDetItms(cn);
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
            BOLib.REFBrand copyREFBrand = null;
            BOLib.REFBrandDetItms copyREFBrandDetItms = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFBrand != null)
                    copyREFBrand = this._REFBrand.Clone();

                if (this._REFBrandDetItms != null)
                    copyREFBrandDetItms = GFunc.TACopyDataTable(_REFBrandDetItms); 
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
                        this._REFBrand = REFBrand.New();
                        this._REFBrandDetItms = new REFBrandDetItms(cn); 
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;

                        //Attach Events
                        this._REFBrand.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Obj_PropertyChanged);
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
                    this._REFBrand = copyREFBrand;
                    this._REFBrandDetItms = copyREFBrandDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFBrand = null;
                copyREFBrandDetItms = null;
                #endregion
            }
        }//Completed
        public bool GetEdit(int? BrandKey)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.REFBrand copyREFBrand = null;
            BOLib.REFBrandDetItms copyREFBrandDetItms = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFBrand != null)
                    copyREFBrand = this._REFBrand.Clone();

                if (this._REFBrandDetItms != null)
                    copyREFBrandDetItms = GFunc.TACopyDataTable(_REFBrandDetItms); 
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
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, BrandKey, 0, _guID))
                            return false;

                        //Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey) == false)
                            return false;

                        //Add Lock
                        if (SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, BrandKey) == false)
                            return false;

                        //Get Record                                 
                        if (this._REFBrand.Fetch(cn, new REFBrand.Criteria(BrandKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        //Record Not Found
                        if (GFunc.NEInt(this._REFBrand._brandKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }


                        _REFBrandDetItms.Clear();
                        if (_REFBrandDetItms.Fetch(cn, new REFBrandDetItms.Criteria(BrandKey, 1)) == false)
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
                    this._REFBrand = copyREFBrand;
                    this._REFBrandDetItms = copyREFBrandDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFBrand = null;
                copyREFBrandDetItms = null;
                #endregion
            }
        }//Completed
        public bool GetReadOnly(int? BrandKey)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.REFBrand copyREFBrand = null;
            BOLib.REFBrandDetItms copyREFBrandDetItms = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFBrand != null)
                    copyREFBrand = this._REFBrand.Clone();

                if (this._REFBrandDetItms != null)
                    copyREFBrandDetItms = GFunc.TACopyDataTable(_REFBrandDetItms); 
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
                        if (this._REFBrand.Fetch(cn, new REFBrand.Criteria(BrandKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }
                        _REFBrandDetItms.Clear();
                        if (this._REFBrandDetItms.Fetch(cn, new REFBrandDetItms.Criteria(BrandKey, 1)) == false)
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
                    this._REFBrand = copyREFBrand;
                    this._REFBrandDetItms = copyREFBrandDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFBrand = null;
                copyREFBrandDetItms = null;
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

                        if (GFunc.IsNE(_REFBrand))
                            _REFBrand = REFBrand.New();
                        GFunc.ConvertDataTableToObject(dtHeader, _REFBrand);

                        _REFBrandDetItms = new REFBrandDetItms(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[0], _REFBrandDetItms);

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
            int? newBrandKey = 0;
            string autoID = string.Empty;
            BOLib.REFBrand copyREFBrand = null;
            BOLib.REFBrandDetItms copyREFBrandDetItms = null;

            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFBrand != null)
                    copyREFBrand = this._REFBrand.Clone();

                if (this._REFBrandDetItms != null)
                    copyREFBrandDetItms = GFunc.TACopyDataTable(_REFBrandDetItms); 

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
                        if (this.IsNew && GFunc.IsNE(_REFBrand._brandID))
                        {
                            if (SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _REFBrand._brandDes) == false)
                                return false;

                            _REFBrand._brandID = autoID;
                        }
                        #endregion

                        #region Set default value for fields that cannot be empty but can have a general default value

                        //Get Server Date and Time (sdt)
                        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                        _REFBrand._createDate = GFunc.NEDateTime(_REFBrand.CreateDate, svrDateTime);
                        _REFBrand._createUserKey = GFunc.NEInt(_REFBrand.CreateUserKey, AppInfor.currentUserKey);
                        _REFBrand._lastModifiedDate = svrDateTime;
                        _REFBrand._lastModifiedUserKey = AppInfor.currentUserKey;
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
                            if (_REFBrand.Insert(cn, out newBrandKey) == false)
                            {
                                MsgBox.Show(cn,MsgID.Common.SaveFail);
                                return false;
                            }

                            if (_REFBrandDetItms.Insert(cn, newBrandKey) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.SaveFail);
                                return false;
                            }
                        }
                        else
                        {
                            if (_REFBrand.Update(cn) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.SaveFail);
                                return false;
                            }

                            if (_REFBrandDetItms.Delete(cn, new REFBrandDetItms.Criteria(_REFBrand._brandKey, 0)) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.SaveFail);
                                return false;
                            }
                            if (_REFBrandDetItms.Insert(cn, _REFBrand._brandKey) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.SaveFail);
                                return false;
                            }
                        }
                        #endregion

                        #region For New Record perform: Locking, set new recordKey
                        if (IsNew)
                        {
                            if (SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newBrandKey))
                                _REFBrand._brandKey = newBrandKey;
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
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _REFBrand.BrandKey, _REFBrand.BrandID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _REFBrand, _REFBrandDetItms });
                else
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _REFBrand.BrandKey, _REFBrand.BrandID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _REFBrand, _REFBrandDetItms });
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
                    this._REFBrand = copyREFBrand;
                    this._REFBrandDetItms = copyREFBrandDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFBrand = null;
                copyREFBrandDetItms = null;
                #endregion
            }
        }//Completed
        public bool Delete()
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.REFBrand copyREFBrand = null;
            BOLib.REFBrandDetItms copyREFBrandDetItms = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFBrand != null)
                    copyREFBrand = this._REFBrand.Clone();

                if (this._REFBrandDetItms != null)
                    copyREFBrandDetItms = GFunc.TACopyDataTable(_REFBrandDetItms); 
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
                        if (SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _REFBrand._brandKey, GUID) == false)
                            return false;

                        //Check the record is used in other dependency tables
                        if (GFunc.CheckKeyDependantsExists(cn, "BrandKey", _REFBrand._brandKey.Value, _REFBrand._brandID))
                            return false;

                        //Check for Option Table
                        if (GFunc.CheckKeyDependcyinOptionTable(cn, "Brand", _REFBrand._brandKey.Value))
                            return false;

                        //Delete Record
                        if (_REFBrand.Delete(cn, new REFBrand.Criteria(_REFBrand._brandKey)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.DeleteFail);
                            return false;
                        }

                        //Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey) == false)
                            return false;

                        //Create New
                        this._REFBrand = REFBrand.New();
                        this._REFBrandDetItms = new REFBrandDetItms(cn);

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
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyREFBrand.BrandKey, copyREFBrand.BrandID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyREFBrand, copyREFBrandDetItms });

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
                    this._REFBrand = copyREFBrand;
                    this._REFBrandDetItms = copyREFBrandDetItms;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFBrand = null;
                copyREFBrandDetItms = null;
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
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBrand._brandKey, "BrandKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBrand.BrandID, "BrandID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                }
                else
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBrand.BrandKey, "BrandKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBrand.BrandID, "BrandID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                }

                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBrand.BrandDes, "BrandDes", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBrand.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBrand.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBrand.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);

                #endregion

                if (e.PropertyMessage.Count == 0)
                {
                    //StoreProcedure Validation
                    if (_REFBrand.Validation(cn, new REFBrand.Criteria(_REFBrand._brandKey, _REFBrand._brandID, 1), this.IsNew))
                    {
                        return true;
                    }
                    else
                    {
                        e.PropertyMessage.Add("BrandID", SysMessageUtility.Get(cn, MsgID.Validation.DuplicateRecordID + "BrandID"));
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
            try
            {
                //Validation Check for calls from Factory (Save method)
                string msgID = string.Empty;
                bool processOK = true;

                foreach (DataRow dr in this._REFBrandDetItms.Rows)
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
                            Validation_DetailRelation(dr["Model"], false, ref processOK, e);

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
            try
            {
                //Validation Check for calls from FORM (CustomCellUpdate, BeforeRowUpdate)
                string msgID = string.Empty;
                bool processOK = true;
                UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());

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
                    Validation_DetailRelation(grdrow.Cells["Model"].Value, grdrow.IsAddRow, ref processOK, e);
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
                BaseUtility.Validation(propValue, "Model", CheckNm, GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, ref processOK, failonError, e);
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

            var dupList = ObjREFBrandDetItms.AsEnumerable().ToList().FindAll(o =>
                            (o.Field<string>("Model") == ((string)propValue)));

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
                e.PropertyMessage.Add("rowError", "Model" + MsgID.Validation.DuplicateRecord);
                processOK = false;
            }
            else
                processOK = true;

            return processOK;
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
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _REFBrand, _REFBrandDetItms }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _REFBrand, _REFBrandDetItms }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
        /*
        #region Member variables and constants

        private REFBrand _REFBrand = null;
        private REFBrands _REFBrands = null;
        private REFBrandDetItm _REFBrandDetItm = null;
        private REFBrandDetItms _REFBrandDetItms = null;
        private GEnum.InstanceMode _instanceMode = 0;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isOpenReadOnly = false;
        private int _guID = 0;
        public string messageid;


        // System Code Key for this Factory.
        private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Brand;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        // Permission ID for this Factory.
        public const string constPermID = GVar.PermissionID.Brand;

        //  public delegate void ReadOnlyEventHandler(Object sender, EventArgs e);
        public GVar.ReadOnlyEvent readonlyEvent = null;
        public GVar.ErrorEvent errorEvent = null;
        public GVar.ListErrorEvent listErrorEvent = null;

        //  Custom Event Declaration
        public GVar.UINotifierEvent BrandNotifier = null;
        public GVar.UINotifierEvent clearErrorNotifier = null;

        #endregion // Member variables and constant

        #region Factory Properties

        public REFBrand ObjREFBrand
        {
            get
            {
                return this._REFBrand;
            }
        }

        public REFBrands ObjREFBrands
        {
            get
            {
                return this._REFBrands;
            }
        }

        public REFBrandDetItms ObjREFBrandDetItms
        {
            get
            {
                return this._REFBrandDetItms;
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

        #endregion // Constructors

        #region Constructors

        /// <summary>
        /// Default constructor for this Factory.
        /// </summary>
        public REFBrandFactory(GEnum.InstanceMode instanceMode)
        {
            Initialisation(instanceMode);
        }

        #endregion // Constructors

        #region Initialization Method

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
                        if ((this._guID=SysOptionUtility.GetNewLockingGUID(cn))==0)
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

        public bool GetEdit(int? brandKey)
        {
            // Initialisation
            bool isGetEdit = false;
            string msgID = MsgID.Common.GetFail;

            // Copy original object
            BOLib.REFBrand copyREFBrand = null;
            BOLib.REFBrandDetItms copyREFBrandDetItms = null;

            if (!GFunc.IsNE(this._REFBrand))
                copyREFBrand = this._REFBrand.Clone();

            if (!GFunc.IsNE(this._REFBrandDetItms))
                copyREFBrandDetItms = this._REFBrandDetItms;

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
                            if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, brandKey, 0, _guID))
                                return false;

                            // Remove Lock
                            if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                                return false;

                            // Add Lock
                            if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, brandKey))
                                return false;

                            if (_REFBrand == null)
                            {
                                _REFBrand = new REFBrand();
                            }
                            if (this._REFBrand.Fetch(cn, new REFBrand.Criteria(brandKey, 1)) == false)
                            {
                                MsgBox.Show(cn,msgID);
                                return false;
                            }

                            //Record Not Found
                            if (GFunc.NEInt(this._REFBrand._brandKey, 0) == 0)
                            {                                
                                throw new TAException(MsgID.Common.GetFail);
                            }

                            if (_REFBrandDetItms == null)
                            {
                                _REFBrandDetItms = new REFBrandDetItms(cn);
                            }

                            _REFBrandDetItms.Clear();
                            if (!_REFBrandDetItms.Fetch(cn, new REFBrandDetItms.Criteria(brandKey, 1)))
                            {
                                MsgBox.Show(msgID);
                                return false;
                            }



                            // Commit Process                          
                            this._REFBrand.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ObjREFBank_PropertyChanged);

                            this._isNew = false;
                            this._isOpenReadOnly = false;
                            msgID = string.Empty;
                            isGetEdit = true;

                            // No errors - commit transaction
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                            // Set Null to Backup Objects
                            copyREFBrand = null;
                            copyREFBrandDetItms = null;
                            this._isDirty = false;
                        }
                    }
                }
                catch (TAException tex)
                {
                    throw Error(tex);
                }
                catch (Exception ex)
                {
                    // Restore data when error is occur                    
                    this._REFBrand = copyREFBrand;
                    this._REFBrandDetItms = copyREFBrandDetItms;
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

        #endregion //GetEdit Method

        #region GetReadOnly Method

        public bool GetReadOnly(int? brandKey)
        {
            bool isGetReadOnly = false;
            string msgID = MsgID.Common.GetFail;

            // Copy original object
            BOLib.REFBrand copyREFBrand = null;
            BOLib.REFBrandDetItms copyREFBrandDetItms = null;

            if (!GFunc.IsNE(this._REFBrand))
                copyREFBrand = this._REFBrand.Clone();

            if (!GFunc.IsNE(this._REFBrandDetItms))
                copyREFBrandDetItms = this._REFBrandDetItms;

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

                            // Get Header Record
                            if (this._REFBrand.Fetch(cn, new REFBrand.Criteria(brandKey, 1)) == false)
                            {
                                MsgBox.Show(msgID);
                                return false;
                            }
                            if (_REFBrandDetItms == null)
                            {
                                _REFBrandDetItms = new REFBrandDetItms(cn);
                            }
                            // Get Detail Records   
                            _REFBrandDetItms.Clear();
                            if (!_REFBrandDetItms.Fetch(cn, new REFBrandDetItms.Criteria(brandKey, 1)))
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
                            copyREFBrand = null;
                            copyREFBrandDetItms = null;

                            this._isDirty = false;
                        }
                    }
                }
                catch (TAException tex)
                {
                    throw Error(tex);
                }
                catch (Exception ex)
                {
                    // Restore data when error is occur                    
                    this._REFBrand = copyREFBrand;
                    this._REFBrandDetItms = copyREFBrandDetItms;
                    throw Error(ex);
                }
            }
            else
            {
                MsgBox.Show(MsgID.Common.WrongInstanceMode);
                return false;
                //return false;
            }
            return isGetReadOnly;
        }

        #endregion //GetReadOnly Method

        #region New Method

        public bool New()
        {
            bool isNew = false;
            string msgID = MsgID.Common.NewFail;

            BOLib.REFBrand copyREFBrand = null;
            BOLib.REFBrandDetItms copyREFBrandDetItms = null;

            // Copy original object
            if (!GFunc.IsNE(this._REFBrand))
                copyREFBrand = this._REFBrand.Clone();

            if (!GFunc.IsNE(this._REFBrandDetItms))
                copyREFBrandDetItms = this._REFBrandDetItms;

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
                            this._REFBrand = REFBrand.New();

                            // Call New for Detail
                            this._REFBrandDetItms = new REFBrandDetItms(cn);
                            this._REFBrandDetItms.Columns[0].DefaultValue = 0;

                            this._REFBrand.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ObjREFBank_PropertyChanged);
                            this._REFBrandDetItms.ColumnChanged += new DataColumnChangeEventHandler(Details_CollectionChanged);

                            this._isDirty = false;
                            this._isNew = true;
                            this._isOpenReadOnly = false;
                            msgID = string.Empty;
                            isNew = true;

                            // No errors - commit transaction
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                            // Set Null to Backup Objects
                            copyREFBrand = null;
                            copyREFBrandDetItms = null;
                        }
                    }                   
                }
                catch (TAException tex)
                {
                    throw Error(tex);
                }
                catch (Exception ex)
                {
                    // Restore data when error is occur                    
                    this._REFBrand = copyREFBrand;
                    this._REFBrandDetItms = copyREFBrandDetItms;
                    throw Error(ex);
                }
            }
            else
            {
                MsgBox.Show(MsgID.Common.WrongInstanceMode);
                return false;
                //return false;
            }
            return isNew;
        }

        #endregion //New Method

        #region Save Method

        public bool Save()
        {
            bool isSave = false;
            string msgID = MsgID.Common.SaveFail;

            if (this.IsNew)
                msgID = MsgID.Common.AddFail;
            else
                msgID = MsgID.Common.UpdateFail;

            bool isNewRecord = this.IsNew;
            int? newBrandKey = 0;
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

                            // Backup ID
                            recordID = this._REFBrand._brandID;

                            // Get AutoID                               
                            if (isNewRecord && GFunc.IsNE(this._REFBrand._brandID))
                            {
                                if (!SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, this._REFBrand._brandDes))
                                    return false;

                                this._REFBrand.BrandID = autoID;
                            }

                            #region Set Server DateTime If Create and Modified Date is null
                            //Get Server Date and Time (sdt)
                            DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                            //Set Header Obj
                            _REFBrand.CreateDate = GFunc.NEDateTime(_REFBrand.CreateDate, svrDateTime);
                            _REFBrand.CreateUserKey = GFunc.NEInt(_REFBrand.CreateUserKey, AppInfor.currentUserKey);

                            _REFBrand.LastModifiedDate = svrDateTime;
                            _REFBrand.LastModifiedUserKey = AppInfor.currentUserKey;

                            //Set Detail DataTable
                            //_REFBrandDetItms
                            foreach (DataRow dr in _REFBrandDetItms.Rows)
                            {
                                dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
                                dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
                                dr["LastModifiedDate"] = svrDateTime;
                                dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
                            }
                            #endregion

                            // Validation
                            if (!Validation(cn))
                                return false;

                            // Save Header Record                               
                            if (isNewRecord)
                            {
                                if (!this._REFBrand.Insert(cn, out newBrandKey))
                                {
                                    return false;
                                }

                                if (!_REFBrandDetItms.Insert(cn, newBrandKey))
                                {
                                    return false;
                                }
                            }

                            else
                            {
                                if (!this._REFBrand.Update(cn))
                                {
                                    return false;
                                }
                                if (!_REFBrandDetItms.Delete(cn, new REFBrandDetItms.Criteria(_REFBrand._brandKey, 1)))
                                {
                                    return false;
                                }
                                if (!_REFBrandDetItms.Insert(cn, _REFBrand._brandKey))
                                {
                                    return false;
                                }

                            }

                            // Record Locking
                            if (isNewRecord)
                                if (!SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newBrandKey))
                                    return false;

                            // Commit Process                              
                            if (isNewRecord)
                                this._REFBrand._brandKey = newBrandKey;

                            this._isNew = false;
                            msgID = string.Empty;
                            isSave = true;

                            // No errors - commit transaction
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                            isCommitTransFail = false;
                            this._isDirty = false;

                        }// End of SqlConnection
                    }// End of TransactionScope                        

                    // Audit Log                        
                    if (isNewRecord)
                        SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey,_REFBrand._brandKey,_REFBrand._brandID, new object[]{this._REFBrand});
                    else
                        SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _REFBrand._brandKey, _REFBrand._brandID, new object[] { this._REFBrand });
                }
                catch (TAException tex)
                {
                    throw Error(tex);
                }
                catch (Exception ex)
                {
                    if (isNewRecord)
                    {
                        // Restore the auto generated ID
                        this._REFBrand._brandID = recordID;
                    }

                    if (isCommitTransFail)
                        msgID = MsgID.Validation.CommitTransFail;

                    throw Error(ex);
                }
            }
            else
            {
                MsgBox.Show(MsgID.Common.WrongInstanceMode);
                return false;
                //return false;
            }
            return isSave;
        }

        #endregion //Save Method

        #region Delete Method

        public bool Delete()
        {
            bool isDelete = false;
            string msgID = MsgID.Common.DeleteFail;

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
                    if (!SECPermUtility.Delete(constPermID, true))
                        return false;

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        // Record Locking
                        if (!SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, this._REFBrand._brandKey, GUID))
                            return false;

                        // Check the record is used in other dependency tables
                        if (GFunc.CheckKeyDependantsExists(cn, "BrandKey", _REFBrand._brandKey.Value, _REFBrand._brandID))
                            return false;

                        //Check for Option Table
                        if (GFunc.CheckKeyDependcyinOptionTable(cn, "Brand", _REFBrand._brandKey.Value))
                            return false;

                        // Delete Record
                        if (!this._REFBrand.Delete(cn, new REFBrand.Criteria(this._REFBrand._brandKey)))
                        {
                            return false;
                        }

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                            return false;

                        // Create New                           
                        this._REFBrand = REFBrand.New();

                        // Call New for Detail                           
                        this._REFBrandDetItms = new REFBrandDetItms(cn);

                        this._isNew = true;
                        this._isOpenReadOnly = false;
                        msgID = string.Empty;
                        isDelete = true;

                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                    }// End of SqlConnection
                }// End of TransactionScope

                //Audit Log               
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
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
                    if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFBrand.BrandKey, "BrandKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn) != GVar.gcPass)
                    {
                        ErrorMessageID = errorMsgID;
                        return false;
                    }
                    if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFBrand.CreateDate, "CreateDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn) != GVar.gcPass)
                    {
                        ErrorMessageID = errorMsgID;
                        return false;
                    }
                    if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFBrand.CreateUserKey, "CreateUserKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, 0, int.MaxValue, e, cn) != GVar.gcPass)
                    {
                        ErrorMessageID = errorMsgID;
                        return false;
                    }
                    if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFBrand.LastModifiedDate, "LastModifiedDate", GEnum.DataType.DateTime, GEnum.Require.No, null, GEnum.CompareOperator.NotEqual, null, null, null, e, cn) != GVar.gcPass)
                    {
                        ErrorMessageID = errorMsgID;
                        return false;
                    }
                    if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFBrand.LastModifiedUserKey, "LastModifiedUserKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, 0, int.MaxValue, e, cn) != GVar.gcPass)
                    {
                        ErrorMessageID = errorMsgID;
                        return false;
                    }

                    //Error Provider
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBrand.BrandID, "BrandID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBrand.BrandDes, "BrandDes", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBrand.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBrand.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFBrand.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);

                    if (e.PropertyMessage.Count > 0)
                    {
                        isValidation = false;

                        ErrorMessageID = MsgID.Common.ValidationFail;

                        if (!GFunc.IsNE(this.BrandNotifier))
                            this.BrandNotifier.Invoke(this, e);
                        return false;
                    }
                    else
                        isValidation = true;

                    // StoreProcedure Validation
                    if (e.PropertyMessage.Count == 0)
                    {
                        if (this._REFBrand.Validation(cn, new REFBrand.Criteria(this._REFBrand._brandKey, this._REFBrand._brandID, 0), this.IsNew))
                        {
                            msgID = string.Empty;
                        }
                        else
                        {
                            ErrorMessageID = MsgID.Validation.DuplicateRecordID + "BrandID";
                            e.PropertyMessage.Add("BrandID", SysMessageUtility.Get(cn, ErrorMessageID));
                            if (!GFunc.IsNE(this.BrandNotifier))
                                this.BrandNotifier.Invoke(this, e);
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
                        List<DataRow> objs = this._REFBrandDetItms.AsEnumerable().ToList().FindAll(s => s.RowError != string.Empty);

                        if (objs.Count > 0)
                        {
                            ErrorMessageID = MsgID.Common.RecordDetailValidationFail;
                            throw new TAException(ErrorMessageID);
                        }
                    }
                }
                //return isValidation;
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
                case "BrandKey":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "BrandKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    break;
                case "Model":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "Model", GEnum.DataType.String, GEnum.Require.No, null, null, null, null, null, e, cn);
                    break;

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

            foreach (DataRow drs in this._REFBrandDetItms.Rows)
            {
                DataRow dr = drs;
                if (CheckRow != null)
                {
                    dr = CheckRow;
                }

                #region Common Validation

                foreach (DataColumn dc in _REFBrandDetItms.Columns)
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
                    KeyCount = _REFBrandDetItms.AsEnumerable().Count(p => p.Field<string>("Model") == (string)dr["Model"]);

                    if (KeyCount > 1)
                    {
                        processOK = GVar.gcCancel;
                        e.PropertyMessage.Add("Model", SysMessageUtility.Get(cn, "Model" + MsgID.Validation.DuplicateRecord));
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

        void Details_CollectionChanged(object sender, DataColumnChangeEventArgs e)
        {
            _isDirty = true;
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

        private void ObjREFBank_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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
                    case "BrandID":
                        if (IsNew)
                            validateOk = BaseUtility.Validation(out msgID, this._REFBrand._brandID, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null);
                        else
                            validateOk = BaseUtility.Validation(out msgID, this._REFBrand._brandID, e.PropertyName, GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null);
                        break;
                    case "BrandDes":
                        validateOk = BaseUtility.Validation(out msgID, this._REFBrand._brandDes, e.PropertyName, GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null);
                        break;
                    case "Custom1":
                        validateOk = BaseUtility.Validation(out msgID, this._REFBrand._custom1, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);
                        break;
                    case "Custom2":
                        validateOk = BaseUtility.Validation(out msgID, this._REFBrand._custom2, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);
                        break;
                    case "Custom3":
                        validateOk = BaseUtility.Validation(out msgID, this._REFBrand._custom3, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);
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

        private bool ValidationForDetail(SqlConnection cn)
        {
            //Variable Declaration
            string msgID = string.Empty;
            string msgValue = string.Empty;
            bool processOk = true;
            
            for (int i = 0; i < _REFBrandDetItms.Rows.Count; i++)
            {

                processOk = BaseUtility.Validation(out msgID, this._REFBrandDetItms.Rows[i]["Model"], "Model", GEnum.DataType.String, GEnum.Require.Yes, 50, null, 0, null, null);
                if (!processOk)
                {
                    msgID = msgID + "% Model " + this._REFBrandDetItms.Rows[i]["Model"].ToString();
                    {
                        MsgBox.Show(cn, msgID);
                        this._REFBrandDetItms.Rows[i].RowError = msgID;
                        return false;
                    }
                }
                if (processOk)
                {
                    var dupList = this._REFBrandDetItms.AsEnumerable().ToList().FindAll(o =>
                            (o.Field<string>("Model") == this._REFBrandDetItms.Rows[i]["Model"].ToString()));

                    if (dupList.Count > 1)
                    {
                        msgID = "Model" + MsgID.Reference.REFBrandDetItm;
                        this._REFBrandDetItms.Rows[i].RowError = msgID;
                        processOk = false;
                    }
                }

                if (!processOk)
                {                        
                    this._REFBrandDetItms.Rows[i].RowError = SysMessageUtility.Get(cn,msgID);
                    {
                        cn.Close();
                        MsgBox.Show(cn, msgID);
                        return false;
                    }                        
                }
                else
                    this._REFBrandDetItms.Rows[i].RowError = string.Empty;
            }

            return processOk;
        }

        public void setDirty()
        {
            _isDirty = false;
        }
        #endregion

        #region Get Brand Record Collection
        /// <summary>
        /// Get REFBrand Collection by BrandID Range.  Use at Sale Order Adjustment (Criteria)
        /// </summary>
        /// <param name="BrandIDFrom">BrandID From</param>
        /// <param name="BrandIDTo">BrandID To</param>
        /// <returns>Return REFBrand Object Collection</returns>
        public REFBrands DOGetBrandsByBrandIDRange(string BrandIDFrom, string BrandIDTo)
        {
            try
            {
                return REFBrands.Get(BrandIDFrom, BrandIDTo);
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
        #endregion

        #region Error

        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _REFBrand, _REFBrandDetItm }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _REFBrand, _REFBrandDetItm }, ConstantCodeKey);
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
