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
    public class MSTConFactory : CommandBase
    {
        #region Member variables and constants

        public MSTCon _MSTCon = null;
        private REFAddrs _REFAddrs = null;
        private REFContactInfors _REFContactInfors = null;
        private DataTable _newContacts = null;

        private GEnum.InstanceMode _instanceMode = 0;
        private bool _isDirty = false;
        private bool _isValid = false;  //For future use
        private bool _isNew = false;
        private bool _isReadOnly = false;
        private int _guID = 0;

        //System Code Key for this Factory.
        private GEnum.SystemCode constCodeKey;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        //Permission ID for this Factory.
        private string constPermID = GVar.PermissionID.Customer_Record;
        public string PermID { get { return constPermID; } }

        //Event Declaration 
        public GVar.DirtyEvent dirtyEvent = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;

        //For Default Customer
        public int _cAccKey = 0;
        public string _cClass = string.Empty;
        public int _cCBType = 0;
        public int _cGrpKey = 0;
        public int _cIndustryKey = 0;
        public int _cCreditLimit = 0;
        public int _cTaxGrpKey = 0;
        public int _cTermKey = 0;
        public int _cTerritoryKey = 0;
        public int _cPriceType = 0;
        public int _conType = 10;

        //For Default Vendor
        public int _vAccKey = 0;
        public string _vClass = string.Empty;
        public int _vGrpKey = 0;
        public int _vIndustryKey = 0;
        public int _vCreditLimit = 0;
        public int _vTaxGrpKey = 0;
        public int _vTermKey = 0;
        public int _vTerritoryKey = 0;
        public int _vPriceType = 0;

        #endregion // Member variables and constant

        #region Factory Properties

        public MSTCon ObjMSTCon
        {
            get
            {
                return this._MSTCon;
            }
        }
        public REFAddrs ObjREFAddrs
        {
            get
            {
                return this._REFAddrs;
            }
            set
            {
                this._REFAddrs = ObjREFAddrs;
            }
        }
        public REFContactInfors ObjREFContactInfors
        {
            get
            {
                return this._REFContactInfors;
            }
        }
        public DataTable NewContacts
        {
            get
            {
                return this._newContacts;
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

        #endregion // Constructors

        //Constructors, Initialisation
        public MSTConFactory(GEnum.InstanceMode instanceMode)
        {
            this._instanceMode = instanceMode;
            //default initialise to customer
            constCodeKey = GEnum.SystemCode.Customer;
            constPermID = GVar.PermissionID.Customer_Record;
            try
            {
                Initialisation(instanceMode);
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
        public MSTConFactory(GEnum.InstanceMode instanceMode, GEnum.SystemCode _constCodeKey)
        {
            this._instanceMode = instanceMode;
            constCodeKey = _constCodeKey;
            if (constCodeKey == GEnum.SystemCode.Customer)
                constPermID = GVar.PermissionID.Customer_Record;
            else
                constPermID = GVar.PermissionID.Vendor_Record;
            try
            {
                Initialisation(instanceMode);
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
        public bool Initialisation(GEnum.InstanceMode instanceMode)
        {
            bool isInitialisation = false;
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
                            this._MSTCon = new MSTCon();
                            this._REFAddrs = new REFAddrs(cn);
                            this._REFContactInfors = new REFContactInfors(cn);
                            this._newContacts = GFunc.ExecuteQuery(cn,"exec REFContactInfor_Get 4, 40, 0");
                            this._isNew = false;                        
                            this._isReadOnly = false;
                            isInitialisation = true;
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
                            this._MSTCon = new MSTCon();
                            this._REFAddrs = new REFAddrs(cn);
                            this._REFContactInfors = new REFContactInfors(cn);
                            this._newContacts = GFunc.ExecuteQuery(cn, "exec REFContactInfor_Get 4, 40, 0");
                            this._isNew = false;
                            this._isReadOnly = false;
                            isInitialisation = true;
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        }
                    }
                }
                else
                {
                    //Use for situation where no locking and GUID is required but the factory is needed for some internal call
                    //for future use only
                    this._guID = 0;
                    this._instanceMode = GEnum.InstanceMode.InternalCall;
                    this._isReadOnly = false;
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
            
        }//Completed

        //Methods
        public bool New()
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTCon copyMSTCon = null;
            BOLib.REFAddrs copyREFAddrs = null;
            BOLib.REFContactInfors copyREFContactInfors = null;
            #endregion

            try
            {
                
                #region Make backup of objects for restore purpose

                if (this._MSTCon != null)
                    copyMSTCon = this._MSTCon.Clone();

                if (this._REFAddrs != null)
                    copyREFAddrs =  GFunc.TACopyDataTable(_REFAddrs);
                
                if (this._REFContactInfors != null)
                    copyREFContactInfors =  GFunc.TACopyDataTable(_REFContactInfors);

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

                        // Remove all locks by GUID except inprogress Locking
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                            return false;

                        //prepare new instance 
                        this._MSTCon = MSTCon.New();
                        this._MSTCon.Attachments = new SYSAttachments();
                        this._REFAddrs = new REFAddrs(cn);
                        this._REFContactInfors = new REFContactInfors(cn);
                        this._newContacts = GFunc.ExecuteQuery(cn, "exec REFContactInfor_Get 4, 40, 0");

                        //Set Default Value
                        SetDefaultValue(cn);

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        #endregion

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;

                        //Attach Events
                        this._MSTCon.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Obj_PropertyChanged);
                        this._MSTCon.Attachments.ListChanged += new ListChangedEventHandler(Attachments_ListChanged);
                    }
                    restoreFlag = false;
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
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._MSTCon = copyMSTCon;
                    this._REFAddrs = copyREFAddrs;
                    this._REFContactInfors = copyREFContactInfors;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTCon = null;
                copyREFAddrs = null;
                copyREFContactInfors = null;
                #endregion
            }
        }//Completed
        public bool GetEdit(int? conKey, string conID)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTCon copyMSTCon = null;
            BOLib.REFAddrs copyREFAddrs = null;
            BOLib.REFContactInfors copyREFContactInfors = null;
            #endregion

            try
            {
                
                #region Make backup of objects for restore purpose

            if (this._MSTCon != null)
                copyMSTCon = this._MSTCon.Clone();

            if (this._REFAddrs != null)
                copyREFAddrs =  GFunc.TACopyDataTable(_REFAddrs);

            if (this._REFContactInfors != null)
                copyREFContactInfors =  GFunc.TACopyDataTable(_REFContactInfors);

            #endregion

                #region Check Security Permission 
                if (SECPermUtility.Edit(constPermID, true)==false)
                    return false;
                #endregion

                #region Get conKey to open record and check RecordAccess rights
                if (conID !=null && conID != string.Empty)
                    conKey = MSTCon.Get(conID).ConKey;

                if (conKey == 0)
                    return false;

                if (_MSTCon.CanAccessRecord(conKey)==false)
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
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, conKey, 0, _guID))
                            return false;

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                            return false;

                        // Add Lock
                        if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, conKey))
                            return false;

                        #region Get Record
                        if (this._MSTCon.Fetch(cn, new MSTCon.Criteria(conKey, 1))==false)
                        {
                            MsgBox.Show(cn,MsgID.Common.GetFail);
                            return false;
                        }

                        //Record Not Found
                        if (GFunc.NEInt(this._MSTCon._conKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }

                        _REFAddrs.Clear();
                        if (this._REFAddrs.Fetch(cn, new REFAddrs.Criteria(0, (int?)GEnum.AddrLinkType.CustomerOrVendor, conKey, string.Empty, 2))==false)
                        {
                            MsgBox.Show(cn,MsgID.Common.GetFail);
                            return false;
                        }



                        _REFContactInfors.Clear();
                        if (this._REFContactInfors.Fetch(cn, new REFContactInfors.Criteria(0, (int?)GEnum.ContactLinkType.CustomerOrVendor, conKey, 2))==false)
                        {
                            MsgBox.Show(cn,MsgID.Common.GetFail);
                            return false;
                        }

                        this._newContacts = GFunc.ExecuteQuery(cn, "exec REFContactInfor_Get 4, 40,"+_MSTCon.ConKey);

                        this._MSTCon.Attachments.Fetch(cn, new SYSAttachments.Criteria((int)this.constCodeKey, _MSTCon.ConKey, 1));
                        #endregion

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags  
                        this._isDirty = false;
                        this._isNew = false;
                        this._isReadOnly = false;
                        
                        //Attach Events
                        this._MSTCon.PropertyChanged += new PropertyChangedEventHandler(Obj_PropertyChanged);
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
                    this._MSTCon = copyMSTCon;
                    this._REFAddrs = copyREFAddrs;
                    this._REFContactInfors = copyREFContactInfors;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTCon = null;
                copyREFAddrs = null;
                copyREFContactInfors = null;
                #endregion
            }
        }//Completed
        public bool GetReadOnly(int? conKey, string conID)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTCon copyMSTCon = null;
            BOLib.REFAddrs copyREFAddrs = null;
            BOLib.REFContactInfors copyREFContactInfors = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._MSTCon != null)
                    copyMSTCon = this._MSTCon.Clone();

                if (this._REFAddrs != null)
                    copyREFAddrs =  GFunc.TACopyDataTable(_REFAddrs);

                if (this._REFContactInfors != null)
                    copyREFContactInfors =  GFunc.TACopyDataTable(_REFContactInfors);

                #endregion

            
                #region Check Security Permission 
                if (SECPermUtility.Read(constPermID, true)==false)
                    return false;
                #endregion

                #region Get conKey to open record and check RecordAccess rights
                if (conID !=null && conID != string.Empty)
                    conKey = MSTCon.Get(conID).ConKey;

                if (conKey == 0)
                    return false;

                if (_MSTCon.CanAccessRecord(conKey)==false)
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
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                            return false;

                        #region Get Data
                        if (_MSTCon.Fetch(cn, new MSTCon.Criteria(conKey, 1))==false)
                        {
                            MsgBox.Show(cn,MsgID.Common.GetFail); 
                            return false;
                        }

                        _REFAddrs.Clear();
                        if (_REFAddrs.Fetch(cn, new REFAddrs.Criteria(0, (int?)GEnum.AddrLinkType.CustomerOrVendor, conKey, string.Empty, 2))==false)
                        {
                            MsgBox.Show(cn,MsgID.Common.GetFail); 
                            return false;
                        }

                        _REFContactInfors.Clear();
                        if (_REFContactInfors.Fetch(cn, new REFContactInfors.Criteria(0, (int?)GEnum.ContactLinkType.CustomerOrVendor, conKey, 2))==false)
                        {
                            MsgBox.Show(cn,MsgID.Common.GetFail); 
                            return false;
                        }

                        this._newContacts = GFunc.ExecuteQuery(cn, "exec REFContactInfor_Get 4, 40," + _MSTCon.ConKey);

                        this._MSTCon.Attachments.Fetch(cn, new SYSAttachments.Criteria((int)this.constCodeKey, _MSTCon.ConKey, 1));
                        #endregion

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        _isDirty = false;
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
                    this._MSTCon = copyMSTCon;
                    this._REFAddrs = copyREFAddrs;
                    this._REFContactInfors = copyREFContactInfors;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTCon = null;
                copyREFAddrs = null;
                copyREFContactInfors = null;
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

                        if (GFunc.IsNE(_MSTCon))
                            _MSTCon = MSTCon.New();
                        GFunc.ConvertDataTableToObject(dtHeader, _MSTCon);

                        _REFAddrs = new REFAddrs(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[0], _REFAddrs);
                        _REFContactInfors = new REFContactInfors(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[1], _REFContactInfors);
                        List<SYSAttachment> AttachmentList = new List<SYSAttachment>();

                        //Attachments saving part is not finished in LogUtility =>AddAuditLog=>GFunc.ConvertObjectToXML.
                        //GFunc.ConvertDataTableToObjectList<SYSAttachment>(dsDetail.Tables[2], AttachmentList);
                        //foreach (SYSAttachment obj in AttachmentList)
                        //{
                        //    _MSTCon.Attachments.Add(obj);
                        //}
                        

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
            int? newConKey = 0;
            string autoID = string.Empty;
            string msgID = string.Empty;
            BOLib.MSTCon copyMSTCon = null;
            BOLib.REFAddrs copyREFAddrs = null;
            BOLib.REFContactInfors copyREFContactInfors = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._MSTCon != null)
                    copyMSTCon = this._MSTCon.Clone();

                if (this._REFAddrs != null)
                    copyREFAddrs =  GFunc.TACopyDataTable(_REFAddrs);

                if (this._REFContactInfors != null)
                    copyREFContactInfors =  GFunc.TACopyDataTable(_REFContactInfors);

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
                        if (this.IsNew && GFunc.IsNE(_MSTCon._conID))
                        {
                            if (SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _MSTCon._conNm) == false)
                                return false;

                            _MSTCon._conID = autoID;
                        }
                        #endregion

                        #region Set default value for fields that cannot be empty but can have a general default value

                        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);//Get Server Date and Time (sdt)

                        _MSTCon._accessLevel = GFunc.NEInt(_MSTCon.AccessLevel,0);
                        _MSTCon._accessGroup = GFunc.NEInt(_MSTCon.AccessGroup,0);
                        _MSTCon._cBranchKey = GFunc.NEInt(_MSTCon.CBranchKey,0);
                        _MSTCon._cDeptKey = GFunc.NEInt(_MSTCon.CDeptKey,0);
                        _MSTCon._cGrpKey = GFunc.NEInt(_MSTCon.CGrpKey,0);
                        _MSTCon._cCreditLimit = GFunc.NEInt(_MSTCon.CCreditLimit,0);
                        _MSTCon._cCurrkey = GFunc.NEInt(_MSTCon.CCurrkey,1);
                        _MSTCon._cDefaultStateType = GFunc.NEInt(_MSTCon.CDefaultStateType,10);
                        _MSTCon._cCreditBal = GFunc.NEInt(_MSTCon.CCreditBal,0);
                        _MSTCon._cCashBal = GFunc.NEInt(_MSTCon.CCashBal, 0);
                        _MSTCon._vBranchKey = GFunc.NEInt(_MSTCon.VBranchKey, 0);
                        _MSTCon._vDeptKey = GFunc.NEInt(_MSTCon.VDeptKey,0);
                        _MSTCon._vGrpKey = GFunc.NEInt(_MSTCon.VGrpKey,0);
                        _MSTCon._vCreditLimit = GFunc.NEInt(_MSTCon.VCreditLimit,0);
                        _MSTCon._vCurrkey = GFunc.NEInt(_MSTCon.VCurrkey,1);
                        _MSTCon._vBal = GFunc.NEInt(_MSTCon.VBal,0);
                        _MSTCon._conChildren = GFunc.NEShort(_MSTCon.ConChildren,0);
                        _MSTCon._createDate = GFunc.NEDateTime(_MSTCon.CreateDate, svrDateTime);
                        _MSTCon._createUserKey = GFunc.NEInt(_MSTCon.CreateUserKey, AppInfor.currentUserKey);
                        _MSTCon._lastModifiedDate = svrDateTime;
                        _MSTCon._lastModifiedUserKey = AppInfor.currentUserKey;

                        //_REFAddrs
                        foreach (DataRow dr in _REFAddrs.Rows)
                        {
                            dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
                            dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
                            dr["LastModifiedDate"] = svrDateTime;
                            dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
                        }
                        //_REFContactInfors
                        foreach (DataRow dr in _REFContactInfors.Rows)
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

                        if (Validation_Detail("tagrdDetAssembly", (DataTable)this.ObjREFAddrs, cn) == false)
                            return false;
                        if (Validation_Detail("tagrdDetAlternates", (DataTable)this.ObjREFContactInfors, cn) == false)
                            return false;  

                        #endregion

                        #region Save Record

                        if (IsNew)
                        {
                            if (!_MSTCon.Insert(cn, out newConKey))
                                return false;

                            if (!_REFAddrs.Insert(cn, new REFAddrs.Criteria(0, (int?)GEnum.AddrLinkType.CustomerOrVendor, newConKey, "", 0)))
                            { return false; }

                            if (!_REFContactInfors.Insert(cn, new REFContactInfors.Criteria(0, (int?)GEnum.AddrLinkType.CustomerOrVendor, newConKey, 0)))
                            { return false; }

                            using (SqlCommand cm = cn.CreateCommand())
                            {
                                cm.CommandType = CommandType.StoredProcedure;
                                cm.CommandText = "REFContactInfor_InsertXML";
                                _newContacts.TableName = "dtREFContactInfor";
                                cm.Parameters.AddWithValue("@xmlContact", GFunc.ConvertDataTableToXML(_newContacts));
                                cm.Parameters.AddWithValue("@ConKey", newConKey);
                                cm.Parameters.AddWithValue("@UserKey", AppInfor.currentUserKey);
                                cm.ExecuteNonQuery();                              
                            }

                            if (_MSTCon.Attachments != null)
                            {
                                foreach (SYSAttachment obj in _MSTCon.Attachments)
                                {
                                    obj._docDK = newConKey;
                                }
                                DocUtility.AttachmentSave(cn, _MSTCon.Attachments, this.constCodeKey, _MSTCon.ConKey);
                            }
                        }
                        else
                        {
                            if (!_MSTCon.Update(cn))
                                return false;

                            if (!_REFAddrs.Delete(cn, new REFAddrs.Criteria(0, (int?)GEnum.AddrLinkType.CustomerOrVendor, _MSTCon._conKey, "", 0)))
                            { return false; }

                            if (!_REFAddrs.Insert(cn, new REFAddrs.Criteria(0, (int?)GEnum.AddrLinkType.CustomerOrVendor, _MSTCon._conKey, "", 0)))
                            { return false; }

                            if (!_REFContactInfors.Delete(cn, new REFContactInfors.Criteria(0, (int?)GEnum.AddrLinkType.CustomerOrVendor, _MSTCon._conKey, 0)))
                            { return false; }

                            if (!_REFContactInfors.Insert(cn, new REFContactInfors.Criteria(0, (int?)GEnum.AddrLinkType.CustomerOrVendor, _MSTCon._conKey, 0)))
                            { return false; }

                            using (SqlCommand cm = cn.CreateCommand())
                            {
                                cm.CommandType = CommandType.StoredProcedure;
                                cm.CommandText = "REFContactInfor_InsertXML";
                                _newContacts.TableName = "dtREFContactInfor";
                                cm.Parameters.AddWithValue("@xmlContact", GFunc.ConvertDataTableToXML(_newContacts));
                                cm.Parameters.AddWithValue("@ConKey", _MSTCon.ConKey);
                                cm.Parameters.AddWithValue("@UserKey", AppInfor.currentUserKey);
                                cm.ExecuteNonQuery();
                            }

                            if (_MSTCon.Attachments != null)
                                DocUtility.AttachmentSave(cn, _MSTCon.Attachments, this.constCodeKey, _MSTCon.ConKey);
                        }
                        #endregion

                        #region For New Record perform: Locking, set new recordKey
                        if (IsNew)
                        {
                            if (SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newConKey))
                                _MSTCon.ConKey = newConKey;
                            else
                                return false;
                        }
                        #endregion
                        //added for checking inactive refresh and requesting approval period, the record turns to readonly mode by nnt on 2019 April
                        _MSTCon.Fetch(cn, new MSTCon.Criteria(_MSTCon.ConKey, 1));

                        if (_MSTCon.Approval == true) this._isReadOnly = true;

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
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _MSTCon.ConKey, _MSTCon.ConID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTCon, _REFAddrs, _REFContactInfors, _MSTCon.Attachments });
                else
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _MSTCon.ConKey, _MSTCon.ConID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTCon, _REFAddrs, _REFContactInfors, _MSTCon.Attachments });
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
                    this._MSTCon = copyMSTCon;
                    this._REFAddrs = copyREFAddrs;
                    this._REFContactInfors = copyREFContactInfors;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTCon = null;
                copyREFAddrs = null;
                copyREFContactInfors = null;
                #endregion
            }
        }//Completed
        public bool Delete()
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTCon copyMSTCon = null;
            BOLib.REFAddrs copyREFAddrs = null;
            BOLib.REFContactInfors copyREFContactInfors = null;
            #endregion

            try
            {
                
                #region Make backup of objects for restore purpose

                if (this._MSTCon != null)
                    copyMSTCon = this._MSTCon.Clone();

                if (this._REFAddrs != null)
                    copyREFAddrs =  GFunc.TACopyDataTable(_REFAddrs);

                if (this._REFContactInfors != null)
                    copyREFContactInfors =  GFunc.TACopyDataTable(_REFContactInfors);

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
                        if (SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _MSTCon._conKey, GUID) == false)
                            return false;

                        //Check the record is used in other dependency tables
                        if (GFunc.CheckKeyDependantsExists(cn, "ConKey", _MSTCon._conKey.Value, _MSTCon._conID))
                            return false;

                        //Delete Record
                        if (_MSTCon.Delete(cn, new MSTCon.Criteria(_MSTCon._conKey)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.DeleteFail);
                            return false;
                        }

                        if (_REFAddrs.Delete(cn, new REFAddrs.Criteria(0, (int?)GEnum.AddrLinkType.CustomerOrVendor, _MSTCon._conKey, "", 0))==false)
                            return false;

                        if (_REFContactInfors.Delete(cn, new REFContactInfors.Criteria(0, (int?)GEnum.ContactLinkType.CustomerOrVendor, _MSTCon._conKey, 0))==false)
                            return false;

                        // Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey) == false)
                            return false;

                        // Create New
                        this._MSTCon = MSTCon.New();
                        this._REFAddrs = new REFAddrs(cn);
                        this._REFContactInfors = new REFContactInfors(cn);

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
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyMSTCon.ConKey, copyMSTCon.ConID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyMSTCon, copyREFAddrs, copyREFContactInfors });

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
                    this._MSTCon = copyMSTCon;
                    this._REFAddrs = copyREFAddrs;
                    this._REFContactInfors = copyREFContactInfors;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTCon = null;
                copyREFAddrs = null;
                copyREFContactInfors = null;
                #endregion
            }
        }//Completed
        public bool Dispose()
        {           
                if (SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0) == false)
                    return false;            
                else
                    return true;
        }//Completed

        //Functions
        public void SetDefaultValue()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    SetDefaultValue(cn);
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
        public void SetDefaultValue(SqlConnection cn)
        {
            bool runCust = false;
            bool runVend = false;
            
            try
            {
                if (GFunc.IsNEZ(_MSTCon.ConType))
                {
                    if (constCodeKey == GEnum.SystemCode.Customer)
                        _MSTCon.ConType = 10;
                    else
                        _MSTCon.ConType = 20;
                }

                #region determine process to run
                switch (_MSTCon.ConType)
                {
                    case 10:    //Customer
                    case 40:    //Prospect
                        runCust = true;
                        break;

                    case 20:    //Vendor
                        runVend = true;
                        runCust = false;
                        break;

                    default:    //Both
                        runCust = true;
                        runVend = true;
                        break;
                }
                #endregion

                #region Customer Defaults
                if (IsDirty == false)
                {
                    if (runCust)
                    {
                        _MSTCon._cCBType = SysOptionUtility.GetInt("CustCreditCashBoth", cn);
                        _MSTCon._cAccKey = SysOptionUtility.GetInt("CustAcc", cn);
                        _MSTCon._cGrpKey = SysOptionUtility.GetInt("CustGrp", cn);
                        _MSTCon._cTermKey = SysOptionUtility.GetInt("CustTerm", cn);
                        _MSTCon._cTaxGrpKey = SysOptionUtility.GetInt("CustTaxGrp", cn);
                        _MSTCon._cCreditLimit = SysOptionUtility.GetDec("CustLimit", cn);
                        _MSTCon._cClass = SysOptionUtility.GetStr("CustClass", cn);
                        _MSTCon._cTerritoryKey = SysOptionUtility.GetInt("CustTerritory", cn);
                        _MSTCon._cIndustryKey = SysOptionUtility.GetInt("CustIndustry", cn);
                        _MSTCon._cPriceType = SysOptionUtility.GetInt("CustPriceType", cn);
                    }
                    else
                    {
                        _MSTCon._cCBType = 30;  //Both
                        _MSTCon._cAccKey = null;
                        _MSTCon._cGrpKey = 0;
                        _MSTCon._cTermKey = 0;
                        _MSTCon._cTaxGrpKey = null;
                        _MSTCon._cCreditLimit = 0;
                        _MSTCon._cClass = string.Empty;
                        _MSTCon._cTerritoryKey = 0;
                        _MSTCon._cIndustryKey = 0;
                        _MSTCon._cPriceType = null;
                    }
                }
                else if (constCodeKey == GEnum.SystemCode.Vendor)
                {
                    if (runCust)
                    {
                        _MSTCon._cCBType = SysOptionUtility.GetInt("CustCreditCashBoth", cn);
                        _MSTCon._cAccKey = SysOptionUtility.GetInt("CustAcc", cn);
                        _MSTCon._cGrpKey = SysOptionUtility.GetInt("CustGrp", cn);
                        _MSTCon._cTermKey = SysOptionUtility.GetInt("CustTerm", cn);
                        _MSTCon._cTaxGrpKey = SysOptionUtility.GetInt("CustTaxGrp", cn);
                        _MSTCon._cCreditLimit = SysOptionUtility.GetDec("CustLimit", cn);
                        _MSTCon._cClass = SysOptionUtility.GetStr("CustClass", cn);
                        _MSTCon._cTerritoryKey = SysOptionUtility.GetInt("CustTerritory", cn);
                        _MSTCon._cIndustryKey = SysOptionUtility.GetInt("CustIndustry", cn);
                        _MSTCon._cPriceType = SysOptionUtility.GetInt("CustPriceType", cn);
                    }
                    else
                    {
                        _MSTCon._cCBType = 30;  //Both
                        _MSTCon._cAccKey = null;
                        _MSTCon._cGrpKey = 0;
                        _MSTCon._cTermKey = 0;
                        _MSTCon._cTaxGrpKey = null;
                        _MSTCon._cCreditLimit = 0;
                        _MSTCon._cClass = string.Empty;
                        _MSTCon._cTerritoryKey = 0;
                        _MSTCon._cIndustryKey = 0;
                        _MSTCon._cPriceType = null;
                    }

                }
                #endregion

                #region Vendor Default
                if (IsDirty == false)
                {
                    if (runVend)
                    {
                        _MSTCon._vAccKey = SysOptionUtility.GetInt("VendAcc", cn);
                        _MSTCon._vGrpKey = SysOptionUtility.GetInt("VendGrp", cn);
                        _MSTCon._vTermKey = SysOptionUtility.GetInt("VendTerm", cn);
                        _MSTCon._vTaxGrpKey = SysOptionUtility.GetInt("VendTaxGrp", cn);
                        _MSTCon._vCreditLimit = SysOptionUtility.GetDec("VendLimit", cn);
                        _MSTCon._vClass = SysOptionUtility.GetStr("VendClass", cn);
                        _MSTCon._vTerritoryKey = SysOptionUtility.GetInt("VendTerritory", cn);
                        _MSTCon._vIndustryKey = SysOptionUtility.GetInt("VendIndustry", cn);
                        _MSTCon._vPriceType = SysOptionUtility.GetInt("VendPriceType", cn);
                    }
                    else
                    {
                        _MSTCon._vAccKey = null;
                        _MSTCon._vGrpKey = 0;
                        _MSTCon._vTermKey = 0;
                        _MSTCon._vTaxGrpKey = null;
                        _MSTCon._vCreditLimit = 0;
                        _MSTCon._vClass = string.Empty;
                        _MSTCon._vTerritoryKey = 0;
                        _MSTCon._vIndustryKey = 0;
                        _MSTCon._vPriceType = null;
                    }
                }
                else if (constCodeKey == GEnum.SystemCode.Customer)
                {
                    if (runVend)
                    {
                        _MSTCon._vAccKey = SysOptionUtility.GetInt("VendAcc", cn);
                        _MSTCon._vGrpKey = SysOptionUtility.GetInt("VendGrp", cn);
                        _MSTCon._vTermKey = SysOptionUtility.GetInt("VendTerm", cn);
                        _MSTCon._vTaxGrpKey = SysOptionUtility.GetInt("VendTaxGrp", cn);
                        _MSTCon._vCreditLimit = SysOptionUtility.GetDec("VendLimit", cn);
                        _MSTCon._vClass = SysOptionUtility.GetStr("VendClass", cn);
                        _MSTCon._vTerritoryKey = SysOptionUtility.GetInt("VendTerritory", cn);
                        _MSTCon._vIndustryKey = SysOptionUtility.GetInt("VendIndustry", cn);
                        _MSTCon._vPriceType = SysOptionUtility.GetInt("VendPriceType", cn);
                    }
                    else
                    {
                        _MSTCon._vAccKey = null;
                        _MSTCon._vGrpKey = 0;
                        _MSTCon._vTermKey = 0;
                        _MSTCon._vTaxGrpKey = null;
                        _MSTCon._vCreditLimit = 0;
                        _MSTCon._vClass = string.Empty;
                        _MSTCon._vTerritoryKey = 0;
                        _MSTCon._vIndustryKey = 0;
                        _MSTCon._vPriceType = null;
                    }
                }

                #endregion
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
        public bool AddressinUse(string addrID)
        {
            string msgID = MsgID.MSTCon.AddrIDInUse;

            try
            {
                if (_MSTCon._conType == (int)GEnum.ConTypeCust.B)
                {
                    if (_MSTCon._cDefaultBillAddr.Equals(addrID) || _MSTCon._cDefaultShipAddr.Equals(addrID) || _MSTCon._cDefaultStateAddr.Equals(addrID)
                    || _MSTCon._vDefaultBillAddr.Equals(addrID) || _MSTCon._vDefaultBillAddr.Equals(addrID) || _MSTCon._vDefaultBillAddr.Equals(addrID))
                    {
                        return true;
                    }
                }
                else if (_MSTCon._conType == (int)GEnum.ConTypeCust.C || _MSTCon._conType == (int)GEnum.ConTypeCust.P)
                {
                    if (_MSTCon._cDefaultBillAddr.Equals(addrID) || _MSTCon._cDefaultShipAddr.Equals(addrID) || _MSTCon._cDefaultStateAddr.Equals(addrID))
                    {
                        return true;
                    }
                }
                else if (_MSTCon._conType == (int)GEnum.ConTypeVend.V)
                {
                    if (_MSTCon._vDefaultBillAddr.Equals(addrID) || _MSTCon._vDefaultBillAddr.Equals(addrID) || _MSTCon._vDefaultBillAddr.Equals(addrID))
                    {
                        return true;
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
            
            msgID = string.Empty;
            return false;
        }

        //Validation
        private bool Validation_Header(SqlConnection cn)
        {
            //fieldNameToCheck = string.empty to check for all fields
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

                #region Validate ConType
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.ConType, "ConType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                #endregion

                #region Validate Item Key and ID for New Record or existing record
                if (this.IsNew)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.ConKey, "ConKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.ConID, "ConID", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                }
                else
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.ConKey, "ConKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.ConID, "ConID", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                }
                #endregion

                #region Validation Process
                if (_MSTCon.ConType == 10 || _MSTCon.ConType == 30 || _MSTCon.ConType == 40)    //Customer or Both or Non Trade
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CBranchKey, "CBranchKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CBranchID, "CBranchID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CDeptKey, "CDeptKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CDeptID, "CDeptID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CGrpKey, "CGrpKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CGrpID, "CGrpID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CClass, "CClass", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CCreditLimit, "CCreditLimit", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.COverallDefaultDis, "COverallDefaultDis", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CCurrkey, "CCurrkey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CCurrID, "CCurrID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);

                    if (_MSTCon.ConType != 40)
                        processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CAccKey, "CAccKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);

                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CDefaultBillAddr, "CDefaultBillAddr", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CDefaultShipAddr, "CDefaultShipAddr", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CDefaultStateAddr, "CDefaultStateAddr", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CDefaultStateType, "CDefaultStateType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CDefaultContact, "CDefaultContact", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CDefaultContactState, "CDefaultContactState", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CRemDelivery, "CRemDelivery", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CRemPrice, "CRemPrice", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CRemValidity, "CRemValidity", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CRemPayment, "CRemPayment", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CRem, "CRem", GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CAttachment, "CAttachment", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CCreditBal, "CCreditBal", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CCashBal, "CCashBal", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                }

                if (_MSTCon.ConType == 20 || _MSTCon.ConType == 30 || _MSTCon.ConType == 40)    //Vendor or Both or non Trade
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VBranchKey, "VBranchKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VBranchID, "VBranchID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VDeptKey, "VDeptKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VDeptID, "VDeptID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VGrpKey, "VGrpKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VGrpID, "VGrpID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VClass, "VClass", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VCreditLimit, "VCreditLimit", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VOverallDefaultDis, "VOverallDefaultDis", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VCurrkey, "VCurrkey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VCurrID, "VCurrID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    
                    if (_MSTCon.ConType != 40)
                        processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VAccKey, "VAccKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VDefaultBillAddr, "VDefaultBillAddr", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VDefaultShipAddr, "VDefaultShipAddr", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VDefaultContact, "VDefaultContact", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VRemDelivery, "VRemDelivery", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VRemPrice, "VRemPrice", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VRemValidity, "VRemValidity", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VRemPayment, "VRemPayment", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VRem, "VRem", GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VAttachment, "VAttachment", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VBal, "VBal", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                }

                if (_MSTCon.ConType == 40)
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.VDefaultAPPYDocType, "VDefaultAPPYDocType", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);

                //Common validation
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.ConNm, "ConNm", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.CCBType, "CCBType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.NoFinCharge, "NoFinCharge", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.AccessLevel, "AccessLevel", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.AccessGroup, "AccessGroup", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.ConUEN, "ConUEN", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.Inactive, "Inactive", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.ActiveWithProblem, "ActiveWithProblem", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.ConNamFirst, "ConNamFirst", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.ConNamLast, "ConNamLast", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.ConNamMiddle, "ConNamMiddle", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.ConNamInitials, "ConNamInitials", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.ConSocSecNo, "ConSocSecNo", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.ConNationality, "ConNationality", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.OccuTitle, "OccuTitle", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.OccuIndustry, "OccuIndustry", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.OccuSalary, "OccuSalary", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.OccuGroup, "OccuGroup", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.Custom4, "Custom4", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTCon.Custom5, "Custom5", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                #endregion

                #region Check for Duplicate Item ID
                if (e.PropertyMessage.Count == 0)
                {
                    bool DuplicateID = _MSTCon.Validation(cn, new MSTCon.Criteria(_MSTCon._conKey, _MSTCon._conID), this.IsNew);
                    if (!DuplicateID && !GFunc.IsNE(this.ErrorNotifierHeader_Set))
                    {
                        errorMsgID = "ConID" + MsgID.Validation.DuplicateRecord;
                        e.PropertyMessage.Add("ConID", SysMessageUtility.Get(cn, errorMsgID));
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
                            Validation_DetailCheck(grdNm, dr[c.ColumnName.ToString()], c.ColumnName.ToString(), false, ref processOK, e);
                        }

                        //Check for Duplicate records
                        if (processOK)
                        {
                            string cellNm = RelationFieldCheckNm_Get(grdNm);
                            if(cellNm!=string.Empty)
                                Validation_DetailRelation(grdNm, dr[cellNm], false, ref processOK, e);
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
                //Check Column values
                if (fieldToCheck == string.Empty)
                {
                    foreach (UltraGridCell c in grdrow.Cells)
                    {
                        Validation_DetailCheck(grdNm, c.Value, c.Column.Key, false, ref processOK, e);
                    }
                }
                else
                    Validation_DetailCheck(grdNm, grdrow.Cells[fieldToCheck].Value, fieldToCheck, false, ref processOK, e);

                //Check for Duplicate records when fieldToCheck is Empty (meaning RowBeforeUpdate)
                if (processOK && fieldToCheck == string.Empty)
                {
                    string cellNm = RelationFieldCheckNm_Get(grdNm);
                    if (cellNm != string.Empty)
                        Validation_DetailRelation(grdNm, grdrow.Cells[cellNm].Value, grdrow.IsAddRow, ref processOK, e);
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
        public bool Validation_DetailCheck(string grdNm, object propValue, string CheckNm, bool failonError, ref bool processOK, UINotifierEventArgs e)
        {
            try
            {
                switch (grdNm)
                {
                    #region REF_ContactInfor
                    case "tagrdREFContact":
                        BaseUtility.Validation(propValue, "ContactType", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "ContactNum", CheckNm, GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "ContactPerson", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e); BaseUtility.Validation(propValue, "CreateDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.No, null, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom1", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom2", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom3", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                        break;
                    #endregion

                    #region REF_Addr Validation
                    case "tagrdREFAddr":
                        BaseUtility.Validation(propValue,"AddrLinkType", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes,null,GEnum.CompareOperator.GreatherThan,0,null,null, ref processOK, failonError,e);
                        BaseUtility.Validation(propValue,"AddrLinkKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes,null,GEnum.CompareOperator.GreatherThan,0,null,null, ref processOK, failonError,e);
                        BaseUtility.Validation(propValue, "AddrID", CheckNm, GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "AddrType", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "AddrStreet", CheckNm, GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "AddrPOBox", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "AddrCity", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "AddrState", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "AddrZipCode", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "AddrCountry", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "AddrRegion", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "AddrAttn", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "AddrTel1", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "AddrTel2", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "AddrFax", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "AddrEmail", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
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
                    #region REFAddr
                    case "tagrdREFAddr":
                        var dupAss = ObjREFContactInfors.AsEnumerable().ToList().FindAll(o => (o.Field<int>("AddrID") == int.Parse(propValue.ToString())));

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
                            e.PropertyMessage.Add("rowError", "Item" + MsgID.Validation.DuplicateRecord);
                            processOK = false;
                        }
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
        private string RelationFieldCheckNm_Get(string grdNm)
        {
            switch (grdNm)
            {
                case "tagrdREFAddr":
                    return "AddrID";

                default:
                    return string.Empty;
            }

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
        private void Attachments_ListChanged(object sender, ListChangedEventArgs e)
        {
            _isDirty = true;
        }//Completed

        //Error
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _MSTCon, _REFAddrs, _REFContactInfors}, constCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _MSTCon, _REFAddrs, _REFContactInfors}, constCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

    }
}
