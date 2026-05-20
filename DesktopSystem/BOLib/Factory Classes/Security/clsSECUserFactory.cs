

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
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class SECUserFactory : CommandBase
    {
        #region Member variables and constants

        private SECUser _SECUser = null;
        private SECUsers _SECUsers = null;
        private SECUserDetGrps _SECUserDetGrps = null;
        private SECGrps _SECGrps = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isReadOnly = false;
        private int _guID = 0;
        private bool _isChangedAccLockOut = false;       

        // System Code Key for this Factory.
        private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Security_User;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        // Permission ID for this Factory.
        private string constPermID = GVar.PermissionID.Security_Users;
        public string PermID { get { return constPermID; } }

        // Custom Event Declaration 
        public GVar.DirtyEvent dirtyEvent = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;
        public GVar.UINotifierEvent SecUserListNotifier = null;

        //public GVar.ReadOnlyEvent readonlyEvent = null;Not ready
        //public GVar.UINotifierEvent errorEvent = null;Not ready
        //public GVar.ListErrorEvent listErrorEvent = null;Not ready

        //  Custom Event Declaration
        //public GVar.UINotifierEvent SECUserNotifier = null;Not ready
        //public GVar.UINotifierEvent clearErrorNotifier = null;Not ready

        #endregion // Member variables and constant

        #region Factory Properties
        public SECUser ObjSECUser
        {
            get
            {
                return this._SECUser;
            }
            set
            {
                this._SECUser = value;
            }
        }
        public SECUsers ObjSECUsers
        {
            get
            {
                return this._SECUsers;
            }
        }
        public SECUserDetGrps ObjSECUserDetGrps
        {
            get
            {
                return this._SECUserDetGrps;
            }
        }
        public SECGrps ObjSECGrps
        {
            get
            {
                return this._SECGrps;
            }
        }

        internal GEnum.InstanceMode? InstanceMode
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
       
        public bool IsChangedAccLockOut
        {
            get
            {
                return this._isChangedAccLockOut;
            }
        }
        #endregion

        //Constructors
        public SECUserFactory(GEnum.InstanceMode instanceMode)
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
                    if (!SECPermUtility.Any(PermID, out this._isReadOnly, true))
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
                            if (SysLockUtility.IsProcessLock(cn, false, GEnum.SysLockOption.ByCodKey, constCodeKey, this._guID))
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

        //Method
        public bool New()
        {
            #region Declaration
            bool restoreFlag = false;
            SECUser copySECUser = null;
            SECUserDetGrps copySECUserDetGrps = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (!GFunc.IsNE(this._SECUser))
                    copySECUser = this._SECUser.Clone();

                if (!GFunc.IsNE(this._SECUserDetGrps))
                    copySECUserDetGrps = this._SECUserDetGrps.Clone();
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

                        // Remove all locks by GUID except inprogress Locking
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                            return false;

                        //prepare new instance 
                        this._SECUser = SECUser.New();
                        this._SECUserDetGrps = SECUserDetGrps.New();

                        // New Group Collection                           
                        this._SECGrps = SECGrps.New();

                        // Fetch All Group List
                        if (!this._SECGrps.Fetch(cn, new SECGrps.Criteria(0, 0)))
                        {
                            MsgBox.Show(cn, "Unable to retrive Security Group List");
                            return false;
                        }

                        // Import Group List from Group List to UserGroup List                               
                        foreach (SECGrp obj in this._SECGrps)
                        {
                            SECUserDetGrp obj1 = SECUserDetGrp.NewChild();
                            obj1._userKey = 0;
                            obj1._grpKey = obj.GrpKey;
                            obj1._grpID = obj.GrpID;
                            obj1._grpDes = obj.GrpDes;
                            this._SECUserDetGrps.Add(obj1);
                        }

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                        #endregion

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;

                        //Attach Events
                        this._SECUser.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Obj_PropertyChanged);
                        this._SECUserDetGrps.ListChanged += new System.ComponentModel.ListChangedEventHandler(_SECUserDetGrps_ListChanged);
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
                    this._SECUser = copySECUser;
                    this._SECUserDetGrps = copySECUserDetGrps;
                }
                #endregion

                #region Dispose Backup Objects
                copySECUser = null;
                copySECUserDetGrps = null;
                #endregion
            }
        }//Completed
        public bool GetEdit(int userKey)
        {
            #region Declaration
            bool restoreFlag = false;
            SECUser copySECUser = null;
            SECUserDetGrps copySECUserDetGrps = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (!GFunc.IsNE(this._SECUser))
                    copySECUser = this._SECUser.Clone();

                if (!GFunc.IsNE(this._SECUserDetGrps))
                    copySECUserDetGrps = this._SECUserDetGrps.Clone();
                #endregion

                #region Check Security Permission
                if (SECPermUtility.Edit(constPermID, true) == false)
                    return false;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        // Check Lock
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, userKey, 0, _guID))
                            return false;

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                            return false;

                        // Add Lock
                        if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, userKey))
                            return false;

                        #region Get Record 
                        if (!this._SECUser.Fetch(cn, new SECUser.Criteria(userKey, 1)))
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        //Record Not Found
                        if (GFunc.NEInt(this._SECUser._userKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }

                        this._SECUser._password = TAUtil.Decoder.Decode(_SECUser._password).Replace(_SECUser._userKey.ToString(), "");
                         _SECUser._password = _SECUser._password.PadRight(10, ((char)255));      
                        //New Detail Objects
                        this._SECUserDetGrps = BOLib.SECUserDetGrps.New();
                        if (!this._SECUserDetGrps.Fetch(cn, new SECUserDetGrps.Criteria(userKey, 0, 1)))
                        {
                            MsgBox.Show(cn,MsgID.Common.GetFail);
                            return false;
                        }
                        this._SECUserDetGrps.AllowEdit = true;
                        this._SECUserDetGrps.AllowRemove = true;
                        this._SECUserDetGrps.AllowNew = true;
                             
                        // Fetch All Group List
                        this._SECGrps = SECGrps.New();
                        if (!this._SECGrps.Fetch(cn, new SECGrps.Criteria(0, 0)))
                        {
                            MsgBox.Show(cn,"Unable to retrive Security Group List");
                            return false;
                        }

                        // Import Security Permission List from Permission List to GroupPermission List
                        foreach (SECGrp obj in this._SECGrps)
                        {
                            SECUserDetGrp objSECUserDetGrp = this._SECUserDetGrps.ToList().Find(delegate(SECUserDetGrp obj1) { return obj1.GrpKey == obj._grpKey; });
                            if (GFunc.IsNE(objSECUserDetGrp))
                            {
                                SECUserDetGrp obj2 = SECUserDetGrp.NewChild();
                                obj2._userKey = this._SECUser._userKey;
                                obj2._grpKey = obj.GrpKey;
                                obj2._grpID = obj.GrpID;
                                obj2._grpDes = obj.GrpDes;
                                this._SECUserDetGrps.Add(obj2);
                            }
                            else
                            {                                   
                                this._SECUserDetGrps.RemoveAt(this._SECUserDetGrps.IndexOf(objSECUserDetGrp));
                                SECUserDetGrp obj2 = SECUserDetGrp.NewChild();
                                obj2._userKey = this._SECUser._userKey;
                                obj2._grpKey = obj.GrpKey;
                                obj2._grpID = obj.GrpID;
                                obj2._grpDes = obj.GrpDes;
                                obj2.WSelected=true;
                                this._SECUserDetGrps.Add(obj2);
                            }
                        }
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        #endregion

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        this._isReadOnly = false;                       
                        this._isChangedAccLockOut = false;

                        //Attach Events                     
                        this._SECUser.PropertyChanged += new PropertyChangedEventHandler(Obj_PropertyChanged);
                        this._SECUserDetGrps.ListChanged += new ListChangedEventHandler(_SECUserDetGrps_ListChanged);
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
                    this._SECUser = copySECUser;
                    this._SECUserDetGrps = copySECUserDetGrps;
                }
                #endregion

                #region Dispose Backup Objects
                copySECUser = null;
                copySECUserDetGrps = null;
                #endregion
            }
        }//Completed
        public bool GetReadOnly(int? userKey)
        {
            #region Declaration
            bool restoreFlag = false;
            SECUser copySECUser = null;
            SECUserDetGrps copySECUserDetGrps = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (!GFunc.IsNE(this._SECUser))
                    copySECUser = this._SECUser.Clone();

                if (!GFunc.IsNE(this._SECUserDetGrps))
                    copySECUserDetGrps = this._SECUserDetGrps.Clone();
                #endregion

                #region Check Security Permission
                if (SECPermUtility.Read(constPermID, true) == false)
                    return false;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        // Remove all locks by GUID except inprogress Locking
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                            return false;

                        #region Get Data
                        if (!this._SECUser.Fetch(cn, new SECUser.Criteria(userKey, 1)))
                        {
                            MsgBox.Show(cn,MsgID.Common.GetFail);
                            return false;
                        }
                        _SECUser._password = _SECUser._password.PadRight(10, ((char)255)); 

                        // Get Detail Records                          
                        this._SECUserDetGrps = BOLib.SECUserDetGrps.New();
                        if (!this._SECUserDetGrps.Fetch(cn, new SECUserDetGrps.Criteria(userKey, 0, 1)))
                        {
                            MsgBox.Show(cn,MsgID.Common.GetFail);
                            return false;
                        }
                        this._SECUserDetGrps.AllowEdit = false;
                        this._SECUserDetGrps.AllowRemove = false;
                        this._SECUserDetGrps.AllowNew = false;

                        // Fetch All Group List
                        this._SECGrps = SECGrps.New();
                        if (!this._SECGrps.Fetch(cn, new SECGrps.Criteria(0, 0)))
                        {
                            MsgBox.Show(cn,"Unable to retrive Security Group List");
                            return false;
                        }

                        // Import Security Permission List from Permission List to GroupPermission List                           
                        foreach (SECGrp obj in this._SECGrps)
                        {
                            SECUserDetGrp objSECUserDetGrp = this._SECUserDetGrps.ToList().Find(delegate(SECUserDetGrp obj1) { return obj1.GrpKey == obj._grpKey; });
                            if (GFunc.IsNE(objSECUserDetGrp))
                            {
                                SECUserDetGrp obj2 = SECUserDetGrp.NewChild();
                                obj2._userKey = this._SECUser._userKey;
                                obj2._grpKey = obj.GrpKey;
                                obj2._grpID = obj.GrpID;
                                obj2._grpDes = obj.GrpDes;
                                this._SECUserDetGrps.Add(obj2);
                            }
                            else
                            {
                                this._SECUserDetGrps[this._SECUserDetGrps.IndexOf(objSECUserDetGrp)]._userKey = this._SECUser._userKey;
                                this._SECUserDetGrps[this._SECUserDetGrps.IndexOf(objSECUserDetGrp)]._grpKey = obj._grpKey;
                                this._SECUserDetGrps[this._SECUserDetGrps.IndexOf(objSECUserDetGrp)]._grpID = obj._grpID;
                                this._SECUserDetGrps[this._SECUserDetGrps.IndexOf(objSECUserDetGrp)]._grpDes = obj._grpDes;
                            }
                        }
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        #endregion

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        this._isReadOnly = true;                       
                        this._isChangedAccLockOut = false;
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
                    this._SECUser = copySECUser;
                    this._SECUserDetGrps = copySECUserDetGrps;
                }
                #endregion

                #region Dispose Backup Objects
                copySECUser = null;
                copySECUserDetGrps = null;
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
                        cn.Open();

                        //Remove all locks by GUID except inprogress Locking
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, _guID, constCodeKey))
                            return false;

                        //prepare New instances
                        this._SECUser = new SECUser();
                        this._SECUserDetGrps = new SECUserDetGrps();

                        #region Fetch Data into Object
                        GFunc.ConvertDataTableToObject(dtHeader, this._SECUser);

                        // Get Detail Records                
                        if (dsDetail.Tables.Count > 0)
                        {
                            this._SECUserDetGrps.Fetch(dsDetail.Tables[0]);
                        }

                        this._SECUserDetGrps.AllowEdit = false;
                        this._SECUserDetGrps.AllowRemove = false;
                        this._SECUserDetGrps.AllowNew = false;

                        // Fetch All Group List
                        this._SECGrps = SECGrps.New();
                        if (!this._SECGrps.Fetch(cn, new SECGrps.Criteria(0, 0)))
                        {                                
                            return false;
                        }

                        // Import Security Permission List from Permission List to GroupPermission List                           
                        foreach (SECGrp obj in this._SECGrps)
                        {
                            SECUserDetGrp objSECUserDetGrp = this._SECUserDetGrps.ToList().Find(delegate(SECUserDetGrp obj1) { return obj1.GrpKey == obj._grpKey; });
                            if (GFunc.IsNE(objSECUserDetGrp))
                            {
                                SECUserDetGrp obj2 = SECUserDetGrp.NewChild();
                                obj2._userKey = this._SECUser._userKey;
                                obj2._grpKey = obj.GrpKey;
                                obj2._grpID = obj.GrpID;
                                obj2._grpDes = obj.GrpDes;
                                this._SECUserDetGrps.Add(obj2);
                            }
                            else
                            {
                                this._SECUserDetGrps[this._SECUserDetGrps.IndexOf(objSECUserDetGrp)]._userKey = this._SECUser._userKey;
                                this._SECUserDetGrps[this._SECUserDetGrps.IndexOf(objSECUserDetGrp)]._grpKey = obj._grpKey;
                                this._SECUserDetGrps[this._SECUserDetGrps.IndexOf(objSECUserDetGrp)]._grpID = obj._grpID;
                                this._SECUserDetGrps[this._SECUserDetGrps.IndexOf(objSECUserDetGrp)]._grpDes = obj._grpDes;
                            }
                        }
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        #endregion

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        this._isReadOnly = true;                       
                        this._isChangedAccLockOut = false;
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
            int newRecordKey = 0;
            string autoID = string.Empty;
            string msgID = string.Empty;
            SECUser copySECUser = null;
            SECUserDetGrps copySECUserDetGrps = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (!GFunc.IsNE(this._SECUser))
                    copySECUser = this._SECUser.Clone();

                if (!GFunc.IsNE(this._SECUserDetGrps))
                    copySECUserDetGrps = this._SECUserDetGrps.Clone();
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
                        if (this.IsNew && GFunc.IsNE(_SECUser._userID))
                        {
                            if (SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _SECUser._userName) == false)
                                return false;

                            _SECUser._userID = autoID;
                        }
                        #endregion

                        #region Set default value for fields that cannot be empty but can have a general default value
                        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);//Get Server Date and Time (sdt)
                        _SECUser._createDate = GFunc.NEDateTime(_SECUser.CreateDate, svrDateTime);
                        _SECUser._createUserKey = GFunc.NEInt(_SECUser.CreateUserKey, AppInfor.currentUserKey);
                        _SECUser._lastModifiedDate = svrDateTime;
                        _SECUser._lastModifiedUserKey = AppInfor.currentUserKey;
                        #endregion

                        // Validation
                        if (!Validation(cn))
                            return false;

                        #region Save Record
                        
                        // Update Password                                                                  
                        this._SECUser._password= this._SECUser._password.Replace(((char)255).ToString(),"");
                        if (!this.IsNew)                        {         
                            
                            this._SECUser._password = TAUtil.Encoder.Encode(this._SECUser._userKey.ToString() + this._SECUser._password);
                        }
                        if (IsNew)
                        {
                            if (!this._SECUser.Insert(cn, out newRecordKey))
                                return false;
                            
                            this._SECUser.UserKey = newRecordKey;
                            this._SECUser._password = TAUtil.Encoder.Encode(this._SECUser.UserKey + this._SECUser._password);                            
                            this._SECUser._custom1= "RESET";  // added by KKAung on 13 Sep 2022 
                
                            if (!this._SECUser.CustomUpdate(cn, (int)GEnum.SECUserCustomUpdateOption.ChangePassword))
                            {
                                MsgBox.Show(cn, "Unable to set password");
                                return false;
                            }
                            this._SECUser._custom1 = "";

                            foreach (SECUserDetGrp objDetail in this._SECUserDetGrps)
                            {
                                if ((bool)objDetail._wSelected)
                                {
                                    objDetail._userKey = newRecordKey;

                                    if (!objDetail.Insert(cn))
                                    {
                                        MsgBox.Show(cn,"Unable to save Security User Group");
                                        return false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!this._SECUser.Update(cn))
                                return false;

                            if (_SECUser._userKey == AppInfor.currentUserKey)
                            {
                                AppInfor.currentUserKey = (int)_SECUser.UserKey;
                                AppInfor.currentUserID = _SECUser.UserID;
                                AppInfor.currentUserName = _SECUser.UserName;
                                AppInfor.conAccessLevel = (int)_SECUser._conAccessLevel;
                                AppInfor.conAccessGroup = (int)_SECUser._conAccessGrp;
                                AppInfor.itemAccessLevel = (int)_SECUser._itmAccessLevel;
                                AppInfor.itemAccessGroup = (int)_SECUser._itmAccessGrp;
                                AppInfor.jobAccessLevel = (int)_SECUser._jobAccessLevel;
                                AppInfor.jobAccessGroup = (int)_SECUser._jobAccessGrp;                                    
                            }

                            // Delete Detail Records in Server
                            if (!this._SECUserDetGrps[0].Delete(cn, new SECUserDetGrp.Criteria(this._SECUser.UserKey, 0, 1)))
                            {
                                MsgBox.Show(cn,msgID);
                                return false;
                            }

                            // Save Detail Record                                
                            foreach (SECUserDetGrp objDetail in this._SECUserDetGrps)
                            {
                                if ((bool)objDetail._wSelected)
                                {
                                    objDetail._userKey = this._SECUser.UserKey;
                                    if (!objDetail.Insert(cn))
                                    {
                                        MsgBox.Show(cn,"Unable to save Security User Group");
                                        return false;
                                    }
                                }
                            }
                        }
                        #endregion
                        
                        #region For New Record perform: Locking, set new recordKey
                        if (IsNew)
                        {
                            if (SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newRecordKey))
                                _SECUser.UserKey = (int)newRecordKey;
                            else
                                return false;
                        }
                        #endregion

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();                            
                        #endregion

                        #region Set Flags
                        this._SECUser._password = TAUtil.Decoder.Decode(_SECUser._password).Replace(_SECUser._userKey.ToString(), "");
                        this._SECUser._password = this._SECUser._password.PadRight(10, (char)255);
                        this._isDirty = false;
                        this._isNew = false;                      
                        this._isChangedAccLockOut = false;
                        #endregion
                    }
                }

                #region Update Auditlog
                if (isNewRecord)
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, this._SECUser.UserKey, this._SECUser.UserID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { this._SECUser, this._SECUserDetGrps });
                else
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, this._SECUser.UserKey, this._SECUser.UserID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { this._SECUser, this._SECUserDetGrps });
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
                    this._SECUser = copySECUser;
                    this._SECUserDetGrps = copySECUserDetGrps;
                }
                #endregion

                #region Dispose Backup Objects
                copySECUser = null;
                copySECUserDetGrps = null;
                #endregion
            }
        }//Completed
        public bool Copy(int userKey)
        {
            #region Declaration
            bool restoreFlag = false;
            SECUser copySECUser = null;
            SECUserDetGrps copySECUserDetGrps = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (!GFunc.IsNE(this._SECUser))
                    copySECUser = this._SECUser.Clone();

                if (!GFunc.IsNE(this._SECUserDetGrps))
                    copySECUserDetGrps = this._SECUserDetGrps.Clone();
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        // Remove all locks by GUID except inprogress Locking                            
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                            return false;

                        // Get Header Record                               
                        if (!this._SECUser.Fetch(cn, new SECUser.Criteria(userKey, 1)))
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        this._SECUser._password = TAUtil.Decoder.Decode(_SECUser._password).Replace(_SECUser._userKey.ToString(), "");
                        this._SECUser._password = this._SECUser._password.PadRight(10, (char)255);
                        this._SECUser._userKey = 0;
                        this._SECUser._userID = string.Empty;
                        this._SECUser._loginIdentifierCurrent = string.Empty;
                        this._SECUser._loginIdentifierLast = string.Empty;
                        this._SECUser._loginTimeStampCurrent = null;
                        this._SECUser._loginTimeStampLast = null;
                        this._SECUser._securityKey = null;
                        this._SECUser._accDisabled = false;
                        this._SECUser._accLockOut = false;
                        this._SECUser._accLockOutTimeStamp = null;
                        this._SECUser._builtIn = false;

                        // Get Detail Records
                        this._SECUserDetGrps = BOLib.SECUserDetGrps.New();
                        if (!this._SECUserDetGrps.Fetch(cn, new SECUserDetGrps.Criteria(userKey, 0, 1)))
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }
                        this._SECUserDetGrps.AllowEdit = true;
                        this._SECUserDetGrps.AllowRemove = true;
                        this._SECUserDetGrps.AllowNew = true;

                        // Fetch All Group List
                        this._SECGrps = SECGrps.New();
                        if (!this._SECGrps.Fetch(cn, new SECGrps.Criteria(0, 0)))
                        {                   
                            MsgBox.Show(cn, "Unable to retrive Security Group List");
                            return false;
                        }

                        // Import Security Permission List from Permission List to GroupPermission List
                        foreach (SECGrp obj in this._SECGrps)
                        {
                            SECUserDetGrp objSECUserDetGrp = this._SECUserDetGrps.ToList().Find(delegate(SECUserDetGrp obj1) { return obj1.GrpKey == obj._grpKey; });
                            if (GFunc.IsNE(objSECUserDetGrp))
                            {
                                SECUserDetGrp obj2 = SECUserDetGrp.NewChild();
                                obj2._userKey = 0;
                                obj2._grpKey = obj.GrpKey;
                                obj2._grpID = obj.GrpID;
                                obj2._grpDes = obj.GrpDes;
                                this._SECUserDetGrps.Add(obj2);
                            }
                            else
                            {
                                this._SECUserDetGrps[this._SECUserDetGrps.IndexOf(objSECUserDetGrp)]._userKey = 0;
                                this._SECUserDetGrps[this._SECUserDetGrps.IndexOf(objSECUserDetGrp)]._grpKey = obj._grpKey;
                                this._SECUserDetGrps[this._SECUserDetGrps.IndexOf(objSECUserDetGrp)]._grpID = obj._grpID;
                                this._SECUserDetGrps[this._SECUserDetGrps.IndexOf(objSECUserDetGrp)]._grpDes = obj._grpDes;
                            }
                        }

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        this._isReadOnly = false;
                        this._isNew = true;
                        this._isDirty = true;                      
                        this._isChangedAccLockOut = false;
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
                    this._SECUser = copySECUser;
                    this._SECUserDetGrps = copySECUserDetGrps;
                }
                #endregion

                #region Dispose Backup Objects
                copySECUser = null;
                copySECUserDetGrps = null;
                #endregion
            }
        }//Completed
        public bool Delete()
        {
            #region Declaration
            bool restoreFlag = false;
            SECUser copySECUser = null;
            SECUserDetGrps copySECUserDetGrps = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (!GFunc.IsNE(this._SECUser))
                    copySECUser = this._SECUser.Clone();

                if (!GFunc.IsNE(this._SECUserDetGrps))
                    copySECUserDetGrps = this._SECUserDetGrps.Clone();
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

                        if (this.ObjSECUser.UserKey == 0)
                            return false;

                        // Check Built-In User
                        if ((bool)this._SECUser.BuiltIn)
                        {
                            throw new TAException(MsgID.SECUser.CannotDeleteBuiltUpUser);
                        }

                        // Check Built-In User
                        if (this._SECUser._userKey == AppInfor.currentUserKey)
                        {
                            throw new TAException(MsgID.SECUser.CannotDeleteCurrentUser);
                        }

                        // Check Current LogIn User
                        if (this._SECUser._loginIdentifierCurrent.Trim().Length > 0)
                        {
                            throw new TAException(MsgID.SECUser.CannotDeleteCurrentLogInUser);
                        }

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        // Record Locking
                        if (!SysLockUtility.CheckAddLock(cn, true, 6, GEnum.SystemCode.Security_Group, this._SECUser.UserKey, this._guID))
                            return false;

                        // Delete Record
                        if (!this._SECUser.Delete(cn, new SECUser.Criteria(this._SECUser.UserKey)))
                        {
                            MsgBox.Show(cn, MsgID.Common.DeleteFail);
                            return false;
                        }
                        foreach (SECUserDetGrp obj in this._SECUserDetGrps)
                        {
                            if (!obj.Delete(cn, new SECUserDetGrp.Criteria(obj.UserKey, obj.GrpKey, 0)))
                            {
                                MsgBox.Show(cn, MsgID.Common.DeleteFail);
                                return false;
                            }
                        }

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                            return false;

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        #endregion

                        #region Set Flag
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;                       
                        this._isChangedAccLockOut = false;
                        #endregion
                    }
                }

                // AuditLog 
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copySECUser.UserKey, copySECUser.UserID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copySECUser, copySECUserDetGrps });

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
                    this._SECUser = copySECUser;
                    this._SECUserDetGrps = copySECUserDetGrps;
                }
                #endregion

                #region Dispose Backup Objects
                copySECUser = null;
                copySECUserDetGrps = null;
                #endregion
            }
        }//Completed
        public bool Dispose()
        {
            try
            {
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
        }//Completed

        //Validation Method
        public bool Validation(SqlConnection cn)
        {
            //fieldNameToCheck = string.empty to check for all fields
            string processOK = GVar.gcPass;
            string errorMsgID = string.Empty;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());   
            
            try
            {
                //Clear Error in UI
                if (GFunc.IsNE(this.ErrorNotifierHeader_Clear) == false)
                    this.ErrorNotifierHeader_Clear.Invoke(this, e);

                #region Validation for each Field
                if (this.IsNew)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.UserKey, "UserKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.UserID, "UserID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                }
                else
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.UserKey, "UserKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.UserID, "UserID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                }

                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.UserName, "UserName", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.Password, "Password", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.ChangePWNL, "ChangePWNL", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.ChangePW, "ChangePW", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.AccDisabled, "AccDisabled", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.AccLockOut, "AccLockOut", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.AccLockOutTimeStamp, "AccLockOutTimeStamp", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.AccessLevel, "AccessLevel", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.BranchKey, "BranchKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.DeptKey, "DeptKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.TranGrpKey, "TranGrpKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.EMKey, "EMKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.ConAccessLevel, "ConAccessLevel", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.ConAccessGrp, "ConAccessGrp", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.ItmAccessLevel, "ItmAccessLevel", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.ItmAccessGrp, "ItmAccessGrp", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.JobAccessLevel, "JobAccessLevel", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.JobAccessGrp, "JobAccessGrp", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.BuiltIn, "BuiltIn", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.LoginRetries, "LoginRetries", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.LoginTimeStampCurrent, "LoginTimeStampCurrent", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.LoginTimeStampLast, "LoginTimeStampLast", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.LoginIdentifierCurrent, "LoginIdentifierCurrent", GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._SECUser.LoginIdentifierLast, "LoginIdentifierLast", GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, e, cn);
                #endregion
                                     
                //int minLen = SysOptionUtility.GetInt("PasswordMinLength",cn);
                int minLen = 0;         // reset password min length
                int maxLen = SysOptionUtility.GetInt("PasswordMaxLength",cn);
                if (this._SECUser._password == null)
                    this._SECUser._password = "";
                if (minLen > this._SECUser._password.Trim().Length || maxLen < this._SECUser._password.Trim().Length)
                {                           
                    e.PropertyMessage.Add("Password","Password length must be between " + minLen + " and "+ maxLen + " characters.");
                }                                    
               
                if (e.PropertyMessage.Count == 0)
                {
                    //StoreProcedure Validation
                    if (_SECUser.Validation(cn, new SECUser.Criteria(_SECUser._userKey, _SECUser._userID), this.IsNew))
                    {
                        return true;
                    }
                    else
                    {
                        e.PropertyMessage.Add("UserID", SysMessageUtility.Get(cn, MsgID.Validation.DuplicateRecordID + "UserID"));
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
        internal bool UserIDCheck(string userID)
        {
            try
            {
                SECUser user = new SECUser();
                if (user.CheckUserID(userID)==false)
                {
                    throw new Exception(MsgID.Login.UserIDOrPasswordInValid);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        internal bool GetUserInforForLogin(string userID)
        {
            try
            {
                this.ObjSECUser = SECUser.Get(userID);
                if ((GFunc.IsNE(this.ObjSECUser)) == false && (GFunc.IsNE(this.ObjSECUser.UserKey)) == false)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        internal bool GetUserInforForLogin(int? userKey)
        {
            try
            {
                this.ObjSECUser = SECUser.Get(userKey);
                if ((GFunc.IsNE(this.ObjSECUser)) == false && (GFunc.IsNE(this.ObjSECUser.UserKey)) == false)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed

        //Other Method
        internal bool ResetUserLoginRetry(string userID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        ObjSECUser._accLockOut = false;
                        ObjSECUser._loginRetries = 0;
                        if (!ObjSECUser.CustomUpdate(cn, (int)GEnum.SECUserCustomUpdateOption.ResetUserLoginRetry))
                            throw new TAException(MsgID.Login.ResetUserLoginRetryFail);

                        if (!ObjSECUser.Fetch(cn, new SECUser.Criteria(userID, 2)))
                            throw new TAException(MsgID.Login.ResetUserLoginRetryFail);

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
        internal bool IncrementUserLoginRetry(int userKey)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        if (!ObjSECUser.CustomUpdate(cn, (int)GEnum.SECUserCustomUpdateOption.IncrementUserLoginRetry))
                            throw new TAException(MsgID.Login.IncrementUserLoginRetryFail);

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
        internal bool SetUserLockout(int userKey)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        if (!ObjSECUser.CustomUpdate(cn, (int)GEnum.SECUserCustomUpdateOption.SetUserLockout))
                            throw new TAException(MsgID.Login.SetUserLockoutFail);

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
        internal bool SaveUserLogin(int userKey)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        this._SECUser._loginIdentifierCurrent = AppInfor.CurrentIdentifier;
                        this._SECUser._securityKey = AppInfor.securityKey;

                        if (!this._SECUser.CustomUpdate(cn, (int)GEnum.SECUserCustomUpdateOption.SaveUserLogin))
                            throw new TAException(MsgID.Login.SaveUserLoginFail);

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
        
        public bool ClearLoginAndLocks()
        {
            string msgID = string.Empty;
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();

                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.Connection = cn;
                    if (_SECUser._userKey == AppInfor.currentUserKey)
                    {
                        msgID = MsgID.SECUser.ClearLocksNotClearLogin;
                        sqlCmd.CommandText = "UpdateUser_ClearLocksOnly";
                    }
                    else
                        sqlCmd.CommandText = "UpdateUser_ClearLoggedIn";

                    sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@UserKey", _SECUser._userKey);
                    sqlCmd.ExecuteNonQuery();

                    _SECUser._loginIdentifierCurrent = string.Empty;
                    _SECUser._loginTimeStampCurrent = null;
                    _isDirty = false;
                    return true;
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

        //PropertyChanged
        private void Obj_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (this._isReadOnly == false)
            {
                if (this.dirtyEvent != null)
                    this.dirtyEvent.Invoke(this, e);

                _isDirty = true;
            }
        }//Completed
        private void _SECUserDetGrps_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemChanged)
            {
                this._isDirty = true;
            }
        }//Completed       
        
        public void OnChangedAccLockOut()
        {
            this._isChangedAccLockOut = true;
        }//Completed
        public void SetAccountLockOut(bool value)
        {
            if (this.IsChangedAccLockOut)
                this._SECUser._accLockOut = value;
        }//Completed
        public void CheckDirty()
        {
            this._isDirty = false;
        }//Completed

        //Error Exceptions
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _SECUser, _SECUserDetGrps }, constCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }//Completed
        private TAException Error(TAException ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _SECUser, _SECUserDetGrps }, constCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }//Completed


        //private void _SECUser_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{           
        //    try
        //    {
        //        if (!this._isReadOnly)
        //        {
        //            //IsDirty
        //            if (this.dirtyEvent != null)
        //                this.dirtyEvent.Invoke(this, e);

        //            this._isDirty = true;

        //        //    if (this.ObjSECUser._branchKey == null)
        //        //        this.ObjSECUser._branchKey = 0;

        //        //    if (this.ObjSECUser._deptKey == null)
        //        //        this.ObjSECUser._deptKey = 0;

        //        //    //UI Validation
        //        //    switch (e.PropertyName)
        //        //    {
        //        //        case "UserID":
        //        //            if (IsNew)
        //        //                validateOk = BaseUtility.Validation(out msgID, this._SECUser._userID, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null);
        //        //            else
        //        //                validateOk = BaseUtility.Validation(out msgID, this._SECUser._userID, e.PropertyName, GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null);
        //        //            break;
        //        //        case "UserName":
        //        //            validateOk = BaseUtility.Validation(out msgID, this._SECUser._userName, e.PropertyName, GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null);
        //        //            break;
        //        //        case "BranchKey":
        //        //            validateOk = BaseUtility.Validation(out msgID, this._SECUser._branchKey, e.PropertyName, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null);
        //        //            break;
        //        //        case "DeptKey":
        //        //            validateOk = BaseUtility.Validation(out msgID, this._SECUser._deptKey, e.PropertyName, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null);
        //        //            break;
        //        //        case "AccessLevel":
        //        //            validateOk = BaseUtility.Validation(out msgID, this._SECUser._accessLevel, e.PropertyName, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null);
        //        //            break;
        //        //        case "Custom1":
        //        //            validateOk = BaseUtility.Validation(out msgID, this._SECUser._custom1, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);
        //        //            break;
        //        //        case "Custom2":
        //        //            validateOk = BaseUtility.Validation(out msgID, this._SECUser._custom2, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);
        //        //            break;
        //        //        case "Custom3":
        //        //            validateOk = BaseUtility.Validation(out msgID, this._SECUser._custom3, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);
        //        //            break;
        //        //    }
        //        //}

        //        //if (!validateOk)
        //        //{

        //        //    if (errorEvent != null)
        //        //    {
        //        //        errorEvent.Invoke(SysMessageUtility.Get(msgID), e);
        //        //        throw new TAException(msgID);
        //        //    }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }

        //}
    }
}
