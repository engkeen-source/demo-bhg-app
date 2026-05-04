
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
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class REFScaleFactory : CommandBase
    {
        #region Member variables and constants

        private REFScale _REFScale = null;        
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isReadOnly = false;
        private int _guID = 0;

        //System Code Key for this Factory.
        private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Scale;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        //Permission ID for this Factory.
        private const string constPermID = GVar.PermissionID.Scale_and_Size;
        public string PermID { get { return constPermID; } }

        //Event Declaration
        public GVar.DirtyEvent dirtyEvent = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;

        #endregion

        #region Factory Properties
        public REFScale ObjREFScale
        {
            get
            {
                return this._REFScale;
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
        public REFScaleFactory(GEnum.InstanceMode instanceMode)
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
                            this._REFScale = new REFScale();
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
            BOLib.REFScale copyREFScale = null;
            #endregion

            try
            {
                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                {
                    #region Make backup of objects for restore purpose
                    if (this._REFScale != null)
                        copyREFScale = this._REFScale.Clone();
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
                            this._REFScale = REFScale.New();
                            
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                            //Set Flags
                            this._isDirty = false;
                            this._isNew = true;
                            this._isReadOnly = false;

                            //Attach Events
                            this._REFScale.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Obj_PropertyChanged);
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
                    this._REFScale = copyREFScale;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFScale = null;
                #endregion
            }
        }//Completed
        public bool GetEdit(int? ScaleKey)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.REFScale copyREFScale = null;
            

            
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (!GFunc.IsNE(this._REFScale))
                    copyREFScale = this._REFScale.Clone();

               
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
                            if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, ScaleKey, 0, _guID))
                                return false;

                            //Remove Lock
                            if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey) == false)
                                return false;

                            //Add Lock
                            if (SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, ScaleKey) == false)
                                return false;

                            //Get Record                                 
                            if (this._REFScale.Fetch(cn, new REFScale.Criteria(ScaleKey, 1)) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.GetFail);
                                return false;
                            }

                            //Record Not Found
                            if (GFunc.NEInt(this._REFScale._scaleKey, 0) == 0)
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
                    this._REFScale = copyREFScale;
                 
                }
                #endregion

                #region Dispose Backup Objects
                copyREFScale = null;
                
                #endregion
            }
        }//Completed
        public bool GetReadOnly(int? ScaleKey)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.REFScale copyREFScale = null;
            
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFScale != null)
                    copyREFScale = this._REFScale.Clone();
                
                
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
                            if (this._REFScale.Fetch(cn, new REFScale.Criteria(ScaleKey, 1)) == false)
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
                    this._REFScale = copyREFScale;
                    
                }
                #endregion

                #region Dispose Backup Objects
                copyREFScale = null;
                
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

                            if (GFunc.IsNE(_REFScale))
                                _REFScale = REFScale.New();
                            GFunc.ConvertDataTableToObject(dtHeader, _REFScale);

                            

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
            int? newScaleKey = 0;
            string autoID = string.Empty;
            BOLib.REFScale copyREFScale = null;
            
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFScale != null)
                    copyREFScale = this._REFScale.Clone();
                 
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
                            if (this.IsNew && GFunc.IsNE(_REFScale._scaleID))
                            {
                                if (!SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _REFScale._scaleDes))
                                    return false;

                                _REFScale._scaleID = autoID;
                            }
                            #endregion

                            #region Set default value for fields that cannot be empty but can have a general default value
                            //Get Server Date and Time (sdt)
                            DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                            _REFScale._createDate = GFunc.NEDateTime(_REFScale.CreateDate, svrDateTime);
                            _REFScale._createUserKey = GFunc.NEInt(_REFScale.CreateUserKey, AppInfor.currentUserKey);
                            _REFScale._lastModifiedDate = svrDateTime;
                            _REFScale._lastModifiedUserKey = AppInfor.currentUserKey;
                            #endregion                        

                            #region Validation
                            if (Validation(cn) == false)
                                return false;
                            #endregion

                            #region Save Record
                            //Note: there is no delete permission for Sales Rep Payroll (only Read,Edit)
                            if (IsNew)
                            {
                                if (_REFScale.Insert(cn, out newScaleKey) == false)
                                {
                                    MsgBox.Show(cn,MsgID.Common.SaveFail);
                                    return false;
                                }
                            }
                            else
                            {
                                if (_REFScale.Update(cn) == false)
                                {
                                    MsgBox.Show(cn, MsgID.Common.SaveFail);
                                    return false;
                                }
                            }
                            #endregion

                            #region For New Record perform: Locking, set new recordKey
                            if (IsNew)
                            {
                                if (SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newScaleKey))
                                    _REFScale._scaleKey = newScaleKey;
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
                        SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _REFScale._scaleKey, _REFScale._scaleID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _REFScale });
                    else
                        SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _REFScale._scaleKey, _REFScale._scaleID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _REFScale });
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
                    this._REFScale = copyREFScale;
                   
                }
                #endregion

                #region Dispose Backup Objects
                copyREFScale = null;
                
                #endregion
            }
        }//Completed
        public bool Delete()
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.REFScale copyREFScale = null;
            
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFScale != null)
                    copyREFScale = this._REFScale.Clone();
                
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
                            if (SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _REFScale._scaleKey, GUID) == false)
                                return false;


                            //Check the record is used in other dependency tables
                            if (GFunc.CheckKeyDependantsExists(cn, "ScaleKey", _REFScale._scaleKey.Value, _REFScale._scaleID))
                                return false;

                            //Delete Record
                            if (_REFScale.Delete(cn, new REFScale.Criteria(_REFScale._scaleKey)) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.DeleteFail);
                                return false;
                            }

                            //Remove Lock
                            if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                                return false;

                            //Create New
                            this._REFScale = REFScale.New();

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
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyREFScale._scaleKey, copyREFScale._scaleID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyREFScale });

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
                    this._REFScale = copyREFScale;
                    
                }
                #endregion

                #region Dispose Backup Objects
                copyREFScale = null;
                
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
                return true;
            }
            catch (Exception ex)
            {
                Error(ex);
                return true;
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
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFScale._scaleKey, "ScaleKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFScale._scaleID, "ScaleID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                }
                else
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, _REFScale._scaleKey, "ScaleKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFScale._scaleID, "ScaleID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFScale._scaleDes, "ScaleDes", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFScale.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFScale.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFScale.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                #endregion

                // Validation
                if (processOK == GVar.gcPass)
                {
                    // Check Detail ScaleSize Empty
                    string msgID = MsgID.Reference.REFScaleDetItm;
                    for (int i = 2; i <= 30; i++)
                    {
                        string propertyName = "Size" + i.ToString();
                        object obj = new object();
                        System.Reflection.PropertyInfo propertyInfo = this._REFScale.GetType().GetProperty(propertyName);

                        if (propertyInfo != null)
                            obj = propertyInfo.GetValue(this._REFScale, null);

                        if (!GFunc.IsNE(obj))
                        {
                            propertyName = "Size" + (i - 1).ToString();
                            propertyInfo = this._REFScale.GetType().GetProperty(propertyName);
                            if (propertyInfo != null)
                                obj = propertyInfo.GetValue(this._REFScale, null);
                            if (GFunc.IsNE(obj))
                            {
                                msgID = "ScaleSizeIsEmpty%" + (i - 1);
                                e.PropertyMessage.Add(propertyName, SysMessageUtility.Get(cn, msgID));
                                
                            }
                        }
                    }

                    // Check Detail ScaleSize Validation
                    DataTable dt = new DataTable();
                    DataRow dr = null;
                    dt.Columns.Add("Scale", typeof(string));

                    for (int i = 1; i <= 30; i++)
                    {
                        string propertyName = "Size" + i.ToString();
                        object obj = new object();
                        System.Reflection.PropertyInfo propertyInfo = this._REFScale.GetType().GetProperty(propertyName);

                        if (propertyInfo != null)
                            obj = propertyInfo.GetValue(this._REFScale, null);

                        if (!GFunc.IsNE(obj))
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                if (dt.Rows[j]["Scale"].ToString() == obj.ToString())
                                {
                                    msgID = "Scale size " + i.ToString() + " is already used! Please Choose another Scale Size";
                                    e.PropertyMessage.Add(propertyName, SysMessageUtility.Get(cn, msgID));
                                    
                                }
                            }
                            dr = dt.NewRow();
                            dr["Scale"] = obj.ToString();
                            dt.Rows.Add(dr);
                        }
                    }
                }

                if (e.PropertyMessage.Count == 0)
                {
                    //StoreProcedure Validation
                    if (_REFScale.Validation(cn, new REFScale.Criteria(_REFScale._scaleKey, _REFScale._scaleID), this.IsNew))
                    {
                        return true;
                    }
                    else
                    {
                        e.PropertyMessage.Add("ScaleID", SysMessageUtility.Get(cn, MsgID.Validation.DuplicateRecordID + "ScaleID"));
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
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _REFScale }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _REFScale }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
    }
    /*[Serializable()]
   public class REFScaleFactory1 : CommandBase
   {
       #region Member variables and constants

       private REFScale _REFScale = null;
       private REFScales _REFScales = null;
       private REFScaleDetItm _REFScaleDetItm = null;
       private REFScaleDetItms _REFScaleDetItms = null;
       private GEnum.InstanceMode _instanceMode = 0;
       private bool _isDirty = false;
       private bool _isValid = false;
       private bool _isNew = false;
       private bool _isOpenReadOnly = false;
       private int _guID = 0;

       private DataTable dataTable = new DataTable();
       private DataRow dr;

       // System Code Key for this Factory.
       private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Scale;
       public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

       // Permission ID for this Factory.
       public const string constPermID = GVar.PermissionID.Scale_and_Size;

       // Error Event Declaration
       public GVar.ErrorEvent errorEvent = null;

       // ReadOnly Event Declaration
       public GVar.ReadOnlyEvent readonlyEvent = null;

       //  Custom Event Declaration
       public GVar.UINotifierEvent ScaleNotifier = null;
       public GVar.UINotifierEvent clearErrorNotifier = null;

       #endregion // Member variables and constant

       #region Factory Properties

       public REFScale ObjREFScale
       {
           get
           {
               return this._REFScale;
           }
       }

       public REFScales ObjREFScales
       {
           get
           {
               return this._REFScales;
           }
       }

       public REFScaleDetItm ObjREFScaleDetItm
       {
           get
           {
               return this._REFScaleDetItm;
           }
       }

       public REFScaleDetItms ObjREFScaleDetItms
       {
           get
           {
               return this._REFScaleDetItms;
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

       #region Constructors

       /// <summary>
       /// Default constructor for this Factory.
       /// </summary>
       public REFScaleFactory(GEnum.InstanceMode instanceMode)
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
               //Check Permission
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

                       //Get Instance GUID
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
               // Audit Log
               //SysAuditLogUtility.AddAuditLog( GEnum.AuditLogMode.NA, constCodeKey, new List<object>(new object[]{ _REFScales, _REFScaleDetItms });

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

       public bool GetEdit(int? scaleKey)
       {
           // Initialisation
           bool isGetEdit = false;
           string msgID = MsgID.Common.GetFail;

           // Copy original object
           BOLib.REFScale copyREFScale = null;
           BOLib.REFScaleDetItms copyREFScaleDetItms = null;

           if (!GFunc.IsNE(this._REFScale))
               copyREFScale = this._REFScale.Clone();

           if (!GFunc.IsNE(this._REFScaleDetItms))
               copyREFScaleDetItms = _REFScaleDetItms;

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
                           if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, scaleKey, 0, _guID))
                               return false;

                           // Record Locking                                
                           if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                               return false;

                           // Add Lock
                           if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, scaleKey))
                               return false;

                           if (_REFScale == null)
                           {
                               _REFScale = new REFScale();
                           }
                           // Get Record                                 
                           if (!_REFScale.Fetch(cn, new REFScale.Criteria(scaleKey, 1)))
                           {
                               MsgBox.Show(cn, msgID);
                               return false;
                           }

                           //Record Not Found
                           if (GFunc.NEInt(this._REFScale._scaleKey, 0) == 0)
                           {                                
                               throw new TAException(MsgID.Common.GetFail);
                           }


                           if (_REFScaleDetItms == null)
                           {
                               _REFScaleDetItms = new REFScaleDetItms();
                           }

                           _REFScaleDetItms.Clear();

                           if (!_REFScaleDetItms.Fetch(cn, new REFScaleDetItms.Criteria(scaleKey, 0, 1)))
                           {
                               MsgBox.Show(cn, msgID);
                               return false;
                           }

                           this._REFScale.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(REFScale_PropertyChanged);
                           // Commit Process
                           this._isNew = false;
                           this._isOpenReadOnly = false;
                           msgID = string.Empty;
                           isGetEdit = true;

                           // No errors - commit transaction
                             if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                           // Set Null to Backup Objects
                           copyREFScale = null;
                           copyREFScaleDetItms = null;
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
                   this._REFScale = copyREFScale;
                   this._REFScaleDetItms = copyREFScaleDetItms;
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

       public bool GetReadOnly(int? ScaleKey)
       {
           bool isGetReadOnly = false;
           string msgID = MsgID.Common.GetFail;

           // Copy original object
           BOLib.REFScale copyREFScale = null;
           BOLib.REFScaleDetItms copyREFScaleDetItms = null;

           if (!GFunc.IsNE(this._REFScale))
               copyREFScale = this._REFScale.Clone();

           if (!GFunc.IsNE(this._REFScaleDetItms))
               copyREFScaleDetItms = _REFScaleDetItms;

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
                           if (!_REFScale.Fetch(cn, new REFScale.Criteria(ScaleKey, 1)))
                           {
                               MsgBox.Show(msgID);
                               return false;
                           }

                           if (_REFScaleDetItms == null)
                               _REFScaleDetItms = new REFScaleDetItms();

                           _REFScaleDetItms.Clear();

                           if (!_REFScaleDetItms.Fetch(cn, new REFScaleDetItms.Criteria(ScaleKey, 0, 1)))
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
                           copyREFScale = null;
                           copyREFScaleDetItms = null;
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
                   this._REFScale = copyREFScale;
                   this._REFScaleDetItms = copyREFScaleDetItms;
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

           BOLib.REFScale copyREFScale = null;
           BOLib.REFScaleDetItms copyREFScaleDetItms = null;

           if (!GFunc.IsNE(this._REFScale))
               copyREFScale = this._REFScale.Clone();

           if (!GFunc.IsNE(this._REFScaleDetItms))
               copyREFScaleDetItms = _REFScaleDetItms;

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

                           this._REFScale = REFScale.New();

                           this._REFScaleDetItms = REFScaleDetItms.New();

                           _REFScale.PropertyChanged += new PropertyChangedEventHandler(REFScale_PropertyChanged);
                           this._isDirty = false;
                           this._isNew = true;
                           this._isOpenReadOnly = false;
                           msgID = string.Empty;
                           isNew = true;

                           // No errors - commit transaction
                             if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                           // Set Null to Backup Objects
                           copyREFScale = null;
                           copyREFScaleDetItms = null;
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
                   this._REFScale = copyREFScale;
                   this._REFScaleDetItms = copyREFScaleDetItms;
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
           int? newScaleKey = 0;
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
                           recordID = this._REFScale._scaleID;

                           // Get AutoID
                           if (isNewRecord && GFunc.IsNE(_REFScale._scaleID))
                           {
                               if (!SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _REFScale._scaleDes))
                                   return false;
                               _REFScale.ScaleID = autoID;
                           }

                           #region Set Server DateTime If Create and Modified Date is null
                           //Get Server Date and Time (sdt)
                           DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                           //Set Header Obj
                           _REFScale.CreateDate = GFunc.NEDateTime(_REFScale.CreateDate, svrDateTime);
                           _REFScale.CreateUserKey = GFunc.NEInt(_REFScale.CreateUserKey, AppInfor.currentUserKey);

                           _REFScale.LastModifiedDate = svrDateTime;
                           _REFScale.LastModifiedUserKey = AppInfor.currentUserKey;

                           //Set Detail DataTable

                           //_REFScaleDetItms
                           foreach (REFScaleDetItm objScale in _REFScaleDetItms)
                           {
                               objScale._createDate = GFunc.NEDateTime(objScale.CreateDate, svrDateTime);
                               objScale._createUserKey = GFunc.NEInt(objScale.CreateUserKey, AppInfor.currentUserKey);
                               objScale._lastModifiedDate = svrDateTime;
                               objScale._lastModifiedUserKey = AppInfor.currentUserKey;
                           }

                           #endregion

                           // Validation
                           if (!Validation(cn))
                               return false;

                           // Save Record
                           if (isNewRecord)
                           {
                               if (!_REFScale.Insert(cn, out newScaleKey))
                                   return false;
                           }
                           else
                           {
                               if (!_REFScale.Update(cn))
                                   return false;
                           }

                           // Record Locking
                           if (isNewRecord)
                           {
                               if (!SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newScaleKey))
                                   return false;
                           }

                           // Commit Process

                           if (isNewRecord)
                               _REFScale._scaleKey = newScaleKey;

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
                   #region Update Auditlog
                   if (isNewRecord)
                       SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _REFScale.ScaleKey, _REFScale.ScaleID, new object[] { _REFScale,_REFScaleDetItms });
                   else
                       SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _REFScale.ScaleKey, _REFScale.ScaleID, new object[] { _REFScale, _REFScaleDetItms });
                   #endregion
                    
               }
               catch (TAException tex)
               {
                   // Restore the auto generated ID
                   //_REFScale._scaleID = recordID;                    
                   throw Error(tex);
               }
               catch (Exception ex)
               {
                   if (isNewRecord)
                   {
                       // Restore the auto generated ID
                       this._REFScale._scaleID = recordID;
                   }
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
           string msgID = string.Empty;

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
                           if (!SysLockUtility.CheckAddLock(cn, true, 6, constCodeKey, _REFScale._scaleKey, GUID))
                               return false;

                           // Check the record is used in other dependency tables
                           if (GFunc.CheckKeyDependantsExists(cn, "ScaleKey", _REFScale._scaleKey.Value, _REFScale._scaleID))
                               return false;

                           // Delete Record
                           if (!_REFScale.Delete(cn, new REFScale.Criteria(_REFScale._scaleKey)))
                               return false;

                           // Remove Lock
                           if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                               return false;

                           // Create New
                           this._REFScale = REFScale.New();
                           this._REFScaleDetItms = REFScaleDetItms.New();

                           this._isNew = true;
                           this._isOpenReadOnly = false;
                           msgID = string.Empty;
                           isDelete = true;

                           // No errors - commit transaction
                             if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                       }// End of SqlConnection
                   }// End of TransactionScope
                   // AuditLog

                   //SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyREFScale.ScaleKey, copyREFScale.ScaleID, new object[] { copyREFScale, copyREFScaleDetItm });To add with copy object later
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
           else
           {
               MsgBox.Show(MsgID.Common.WrongInstanceMode);
               return false;
           }
           return isDelete;
       }

       #endregion //Delete Method

       #region Validation Method

       internal bool Validation(SqlConnection cn)
       {
           // Initialisation
           bool isValidation = false;
           string msgID = MsgID.Common.ValidationFail;
           string processOK = GVar.gcPass;
           string errorMsgID = string.Empty;
           UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());

           if (this.InstanceMode == GEnum.InstanceMode.Normal)
           {
               // Clear Error in UI
               if (!GFunc.IsNE(this.clearErrorNotifier))
                   this.clearErrorNotifier.Invoke(this, e);

               //Error Provider 
               processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFScale.ScaleID, "ScaleID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
               processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFScale.ScaleDes, "ScaleDes", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
               processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFScale.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
               processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFScale.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
               processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFScale.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);


               if (e.PropertyMessage.Count > 0)
               {
                   isValidation = false;

                   ErrorMessageID = MsgID.Common.ValidationFail;

                   if (!GFunc.IsNE(this.ScaleNotifier))
                       this.ScaleNotifier.Invoke(this, e);
                   return false;
               }
               else
                   isValidation = true;

               // StoreProcedure Validation
               if (e.PropertyMessage.Count == 0)
               {
                   if (this._REFScale.Validation(cn, new REFScale.Criteria(this._REFScale._scaleKey, this._REFScale._scaleID), this.IsNew))
                   {
                       msgID = string.Empty;
                   }
                   else
                   {
                       ErrorMessageID = MsgID.Validation.DuplicateRecordID + "ScaleID";
                       e.PropertyMessage.Add("ScaleID", SysMessageUtility.Get(cn, ErrorMessageID));
                       if (!GFunc.IsNE(this.ScaleNotifier))
                           this.ScaleNotifier.Invoke(this, e);
                       return false;
                   }
               }

               // Validation
               if (isValidation)
               {
                   // Check Detail ScaleSize Empty
                   msgID = MsgID.Reference.REFScaleDetItm;
                   for (int i = 2; i <= 30; i++)
                   {
                       string propertyName = "Size" + i.ToString();
                       object obj = new object();
                       System.Reflection.PropertyInfo propertyInfo = this._REFScale.GetType().GetProperty(propertyName);

                       if (propertyInfo != null)
                           obj = propertyInfo.GetValue(this._REFScale, null);

                       if (!GFunc.IsNE(obj))
                       {
                           propertyName = "Size" + (i - 1).ToString();
                           propertyInfo = this._REFScale.GetType().GetProperty(propertyName);
                           if (propertyInfo != null)
                               obj = propertyInfo.GetValue(this._REFScale, null);
                           if (GFunc.IsNE(obj))
                           {
                               msgID = "ScaleSizeIsEmpty%" + (i - 1);
                               throw new TAException(msgID, "Size" + (i - 1));
                           }
                       }
                   }

                   // Check Detail ScaleSize Validation
                   dataTable.Reset();
                   dataTable.Columns.Add("Scale", typeof(string));

                   for (int i = 1; i <= 30; i++)
                   {
                       string propertyName = "Size" + i.ToString();
                       object obj = new object();
                       System.Reflection.PropertyInfo propertyInfo = this._REFScale.GetType().GetProperty(propertyName);

                       if (propertyInfo != null)
                           obj = propertyInfo.GetValue(this._REFScale, null);

                       if (!GFunc.IsNE(obj))
                       {
                           for (int j = 0; j < dataTable.Rows.Count; j++)
                           {
                               if (dataTable.Rows[j]["Scale"].ToString() == obj.ToString())
                               {
                                   msgID = "Scale Size is already used! Please Choose another Scale Size";
                                   throw new TAException(msgID);
                               }
                           }
                           dr = dataTable.NewRow();
                           dr["Scale"] = obj.ToString();
                           dataTable.Rows.Add(dr);
                       }
                   }
               }
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

       #region Factory Methods

       public bool AddNewDetail()
       {
           bool isAddNew = false;
           this._REFScaleDetItm = REFScaleDetItm.NewChild();
           this._REFScaleDetItm._scaleKey = this._REFScale._scaleKey;
           this._REFScaleDetItms.Add(this._REFScaleDetItm);
           isAddNew = true;
           return isAddNew;
       }

       #endregion

       #region PropertyChanged
       private void REFScale_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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
                   case "ScaleID":
                       if (IsNew)
                           validateOk = BaseUtility.Validation(out msgID, this._REFScale._scaleID, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null);
                       else
                           validateOk = BaseUtility.Validation(out msgID, this._REFScale._scaleID, e.PropertyName, GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null);
                       break;
                   case "ScaleDes":
                       validateOk = BaseUtility.Validation(out msgID, this._REFScale._scaleDes, e.PropertyName, GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null);
                       break;
                   case "Size1":
                       validateOk = BaseUtility.Validation(out msgID, this._REFScale._size1, e.PropertyName, GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null);
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
               ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _REFScales, _REFScaleDetItm }, ConstantCodeKey);
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
               ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _REFScale, _REFScaleDetItm }, ConstantCodeKey);
           }
           catch (Exception nex)
           {
               MsgBox.Show(nex.Message);
           }
           return ex;
       }

       #endregion
   }  */
}
