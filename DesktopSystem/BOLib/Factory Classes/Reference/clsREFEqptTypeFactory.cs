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
    public class REFEqptTypeFactory : CommandBase
    {
        #region Member variables and constants

        private REFEqptType _REFEqptType = null;
        private REFEqptTypes _REFEqptTypes = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isOpenReadOnly = false;
        private int _guID = 0;

        // System Code Key for this Factory.
        private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Machine_Type_List;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }
        // Permission ID for this Factory.
        public const string constPermID = GVar.PermissionID.Machine_Type;

        // Error Event Declaration
        public GVar.ErrorEvent errorEvent = null;

        // Dirty Event Declaration
        public GVar.DirtyEvent dirtyEvent = null;

        // ReadOnly Event Declaration
        public GVar.ReadOnlyEvent readonlyEvent = null;

        //  Custom Event Declaration
        public GVar.UINotifierEvent EqptTypeNotifier = null;
        public GVar.UINotifierEvent clearErrorNotifier = null;

        #endregion // Member variables and constant

        #region Factory Properties

        public REFEqptType ObjREFEqptType
        {
            get
            {
                return this._REFEqptType;
            }
        }

        public REFEqptTypes ObjREFEqptTypes
        {
            get
            {
                return this._REFEqptTypes;
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
        public REFEqptTypeFactory( GEnum.InstanceMode instanceMode)
        {
            try
            {
                this.Initialisation( instanceMode);
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

        #endregion // Constructors

        #region Initialisation Method

        public bool Initialisation(GEnum.InstanceMode instanceMode)
        {
            bool isInitialisation = false;
            string msgID = MsgID.Common.InitialisationFail;
            try
            {
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
                            if (SysLockUtility.IsProcessLock(cn, true, GEnum.SysLockOption.ByCodKey, constCodeKey,this._guID))
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
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            
        }

        #endregion //Initialisation Method

        #region GetEdit Method

        public bool GetEdit( int? EqptTypeKey)
        {
            // Initialisation
            bool isGetEdit = false;
            string msgID = MsgID.Common.GetFail;           

            // Copy original object
            BOLib.REFEqptType copyREFEqptType = this._REFEqptType.Clone();

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
                            if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, EqptTypeKey, 0, _guID))
                                return false;

                            // Remove Lock
                            if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                                return false;   

                            // Add Lock
                            if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, EqptTypeKey))
                                return false;

                            // Get Record                                 
                            if (!this._REFEqptType.Fetch(cn, new REFEqptType.Criteria(EqptTypeKey, 1)))
                            {
                                MsgBox.Show(cn,msgID);
                                return false;
                            }

                            //Record Not Found
                            if (GFunc.NEInt(this._REFEqptType._eqptTypeKey, 0) == 0)
                            {                                
                                throw new TAException(MsgID.Common.GetFail);
                            }


                            // Commit Process                           
                            this._REFEqptType.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ObjREFEqptType_PropertyChanged);

                            this._isDirty = false;
                            this._isNew = false;
                            this._isOpenReadOnly = false;
                            msgID = string.Empty;
                            isGetEdit = true;

                            // No errors - commit transaction
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                            // Set Null to Backup Objects
                            copyREFEqptType = null;
                           
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
                    this._REFEqptType = copyREFEqptType;
                    throw Error(ex);
                }
            }
            else
            {
                throw new TAException( MsgID.Common.WrongInstanceMode);
            }
            return isGetEdit;
        }

        #endregion //GetEdit Method

        #region GetReadOnly Method

        public bool GetReadOnly(int? EqptTypeKey)
        {
            bool isGetReadOnly = false;
            string msgID = MsgID.Common.GetFail;          

            // Copy original object
            BOLib.REFEqptType copyREFEqptType = this._REFEqptType.Clone();

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

                                // Call REFEqptType.Fetch
                            if (!this._REFEqptType.Fetch(cn, new REFEqptType.Criteria(EqptTypeKey, 1)))
                                return false;   
                               
                            this._isDirty = false;
                            this._isNew = false;
                            this._isOpenReadOnly = true;
                            msgID = string.Empty;
                            isGetReadOnly = true;

                            // No errors - commit transaction
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                            // Set Null to Backup Objects
                            copyREFEqptType = null;
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
                    this._REFEqptType = copyREFEqptType;

                    throw Error(ex);
                }
            }
            else
            {
                throw new TAException( MsgID.Common.WrongInstanceMode);
            }
            return isGetReadOnly;
        }

        #endregion //GetReadOnly Method

        #region New Method

        public bool New()
        {
            bool isNew = false;
            string msgID = MsgID.Common.NewFail;          

            BOLib.REFEqptType copyREFEqptType = null;

            // Copy original object
            if (!GFunc.IsNE(this._REFEqptType))
                copyREFEqptType = this._REFEqptType.Clone();

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
                             this._REFEqptType = REFEqptType.New();                           

                             this._REFEqptType.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ObjREFEqptType_PropertyChanged);

                                this._isDirty = false;
                                this._isNew = true;
                                this._isOpenReadOnly = false;
                                msgID = string.Empty;
                                isNew = true;

                                // No errors - commit transaction
                                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                                // Set Null to Backup Objects
                                copyREFEqptType = null;                          
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
                    this._REFEqptType = copyREFEqptType;

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
            int? newEqptTypeKey = 0;
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
                            recordID = this._REFEqptType._eqptTypeID;

                                // Get AutoID
                               
                                    if (isNewRecord && GFunc.IsNE(_REFEqptType._eqptTypeID))
                                    {
                                        if (!SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _REFEqptType._eqptTypeDes))
                                            return false;
                                        
                                           _REFEqptType.EqptTypeID = autoID;
                                    }

                                    #region Set Server DateTime If Create and Modified Date is null
                                    //Get Server Date and Time (sdt)
                                    DateTime svrDateTime = GFunc.GetSvrDateTime(cn);

                                    _REFEqptType._createDate = GFunc.NEDateTime(_REFEqptType.CreateDate, svrDateTime);
                                    _REFEqptType._createUserKey = GFunc.NEInt(_REFEqptType.CreateUserKey, AppInfor.currentUserKey);
                                    _REFEqptType._lastModifiedDate = svrDateTime;
                                    _REFEqptType._lastModifiedUserKey = AppInfor.currentUserKey;

                                    #endregion

                                // Validation
                                    if (!Validation(cn))
                                        return false;

                                // Save Record
                                if (isNewRecord)
                                {
                                    if (!_REFEqptType.Insert(cn, out newEqptTypeKey))
                                    {
                                        MsgBox.Show(cn,msgID);
                                        return false;
                                    }
                                }
                                else
                                {
                                    if (!_REFEqptType.Update(cn))
                                    {
                                        MsgBox.Show(cn,msgID);
                                        return false;
                                    }
                                }                    

                                // Record Locking
                                if (isNewRecord)
                                    if (!SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newEqptTypeKey))
                                        return false;
                             

                                // Commit Process                              
                                    if (isNewRecord)
                                        _REFEqptType._eqptTypeKey = newEqptTypeKey;

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
                            SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _REFEqptType._eqptTypeKey, _REFEqptType._eqptTypeID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _REFEqptType });
                        else
                            SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _REFEqptType._eqptTypeKey, _REFEqptType._eqptTypeID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _REFEqptType });
                                          
                }
                catch (TAException tex)
                {
                    // Restore the auto generated ID
                    this._REFEqptType._eqptTypeID = recordID;
                    throw Error(tex);
                }
                catch (Exception ex)
                {
                    if (isNewRecord)
                    {
                        // Restore the auto generated ID
                        this._REFEqptType._eqptTypeID= recordID;
                    }
                    if (isCommitTransFail)
                        throw new TAException(MsgID.Validation.CommitTransFail);
                    throw Error(ex);
                }
            }
            else
            {
                throw new TAException( MsgID.Common.WrongInstanceMode);
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
                            if (!SysLockUtility.CheckAddLock(cn, true, 6, constCodeKey, _REFEqptType._eqptTypeKey, GUID))
                                return false;

                            // Check the record is used in other dependency tables
                            if (GFunc.CheckKeyDependantsExists(cn, "EqptTypeKey", _REFEqptType._eqptTypeKey.Value, _REFEqptType._eqptTypeID))
                                return false;

                            // Delete Record
                            if (!_REFEqptType.Delete(cn, new REFEqptType.Criteria(_REFEqptType._eqptTypeKey)))
                                return false;

                            // Remove Lock
                            if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                                return false;

                            // Create New                           
                            _REFEqptType = REFEqptType.New();


                            this._isDirty = false;
                            this._isNew = true;
                            this._isOpenReadOnly = false;
                            msgID = string.Empty;
                            isDelete = true;

                            // No errors - commit transaction
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        }// End of SqlConnection
                    }// End of TransactionScope

                    //Audit Log               //Not ready; Copy Object Missing      
                    // SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[]{_REFEqptType})); To add with copy object later
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
                throw new TAException( MsgID.Common.WrongInstanceMode);
            }

            return isDelete;
        }

        #endregion //Delete Method

        #region Validation Method

        public bool Validation( SqlConnection cn)
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
                    if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFEqptType.EqptTypeKey, "EqptTypeKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn) != GVar.gcPass)
                    {
                        MsgBox.Show(cn,errorMsgID);
                        return false;
                    }
                    if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFEqptType.CreateDate, "CreateDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn) != GVar.gcPass)
                    {
                        MsgBox.Show(cn,errorMsgID);
                        return false;
                    }
                    if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFEqptType.CreateUserKey, "CreateUserKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, 0, int.MaxValue, e, cn) != GVar.gcPass)
                    {
                        MsgBox.Show(cn,errorMsgID);
                        return false;
                    }
                    if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFEqptType.LastModifiedDate, "LastModifiedDate", GEnum.DataType.DateTime, GEnum.Require.No, null, GEnum.CompareOperator.NotEqual, null, null, null, e, cn) != GVar.gcPass)
                    {
                        MsgBox.Show(cn,errorMsgID);
                        return false;
                    }
                    if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFEqptType.LastModifiedUserKey, "LastModifiedUserKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, 0, int.MaxValue, e, cn) != GVar.gcPass)
                    {
                        MsgBox.Show(cn,errorMsgID);
                        return false;
                    }

                    //Error Provider                    
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFEqptType.EqptTypeID, "EqptTypeID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFEqptType.EqptTypeDes, "EqptTypeDes", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFEqptType.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFEqptType.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFEqptType.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);

                    if (e.PropertyMessage.Count > 0)
                    {
                        isValidation = false;

                        ErrorMessageID = MsgID.Common.ValidationFail;

                        if (!GFunc.IsNE(this.EqptTypeNotifier))
                            this.EqptTypeNotifier.Invoke(this, e);
                        return false;
                    }
                    else
                        isValidation = true;

                    // StoreProcedure Validation
                    if (e.PropertyMessage.Count == 0)
                    {
                        if (this._REFEqptType.Validation(cn, new REFEqptType.Criteria(this._REFEqptType._eqptTypeKey, this._REFEqptType._eqptTypeID), this.IsNew))
                        {
                            msgID = string.Empty;
                        }
                        else
                        {
                            ErrorMessageID = MsgID.Validation.DuplicateRecordID + "EqptTypeID";
                            e.PropertyMessage.Add("EqptTypeID", SysMessageUtility.Get(cn, ErrorMessageID));
                            if (!GFunc.IsNE(this.EqptTypeNotifier))
                                this.EqptTypeNotifier.Invoke(this, e);
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
            try
            {
                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                    if (!SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0))
                        return false;
               
                    isDispose = true;        
                return isDispose;
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

        #endregion //Dispose Method

        #region PropertyChanged

        private void ObjREFEqptType_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            string msgID = string.Empty;           
            bool validateOk = true;
            try
            {
                if (!this._isOpenReadOnly)
                {
                    //IsDirty
                    if (this.dirtyEvent != null)
                        this.dirtyEvent.Invoke(this, e);

                    this._isDirty = true;

                    //UI Validation
                    switch (e.PropertyName)
                    {
                        case "EqptTypeID":
                            if (IsNew)
                                validateOk = BaseUtility.Validation(out msgID, _REFEqptType._eqptTypeID, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null);
                            else
                                validateOk = BaseUtility.Validation(out msgID, _REFEqptType._eqptTypeID, e.PropertyName, GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null);
                            break;
                        case "EqptTypeDes":
                            validateOk = BaseUtility.Validation(out msgID, _REFEqptType._eqptTypeDes, e.PropertyName, GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null);
                            break;
                        case "Custom1":
                            validateOk = BaseUtility.Validation(out msgID, _REFEqptType._custom1, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);
                            break;
                        case "Custom2":
                            validateOk = BaseUtility.Validation(out msgID, _REFEqptType._custom2, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);
                            break;
                        case "Custom3":
                            validateOk = BaseUtility.Validation(out msgID, _REFEqptType._custom3, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);
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
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _REFEqptType }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _REFEqptType }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        #endregion
    }
}
