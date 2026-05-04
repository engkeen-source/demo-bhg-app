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
    public class MSTSalesRepFactory : CommandBase
    {
        //NOTE: PAYROLL special feature to take extra caution
        //The payroll data requires additional permission to perform edit
        //and the permission available is only READ,EDIT
        #region Member variables and constants

        private MSTSalesRep _MSTSalesRep = null;
        private MSTSalesRepPayRolls _MSTSalesRepPayrolls = null;
        private MSTSalesRepApprovers _MSTSalesRepApprovers = null;

        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;  //For future use
        private bool _isNew = false;
        private bool _isReadOnly = false;
        private int _guID = 0;

        //System Code Key for this Factory.
        private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Sales_Representative;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        //Permission ID for this Factory.
        private const string constPermID = GVar.PermissionID.Sales_Representative;
        private const string constPayrollPermID = GVar.PermissionID.Sales_Representative_Payroll;
        private const string constApprovalPermID = GVar.PermissionID.Sales_Approver;
        public string PermID { get { return constPermID; } }

        //Event Declaration
        public GVar.DirtyEvent dirtyEvent = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;

        #endregion

        #region Factory Properties
        public MSTSalesRep ObjMSTSalesRep
        {
            get
            {
                return this._MSTSalesRep;
            }
        }
        public MSTSalesRepPayRolls ObjMSTSalesRepPayrolls
        {
            get
            {
                return this._MSTSalesRepPayrolls;
            }
        }
        public MSTSalesRepApprovers ObjMSTSalesRepApprovers
        {
            get
            {
                return this._MSTSalesRepApprovers;
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
        public MSTSalesRepFactory(GEnum.InstanceMode instanceMode)
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
                            this._MSTSalesRep = new MSTSalesRep();
                            this._MSTSalesRepPayrolls = new MSTSalesRepPayRolls(cn);
                            this._MSTSalesRepApprovers = new MSTSalesRepApprovers(cn);


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
            BOLib.MSTSalesRep copyMSTSalesRep = null;
            BOLib.MSTSalesRepPayRolls copyMSTSalesRepPayrolls = null;
            BOLib.MSTSalesRepApprovers copyMSTSalesRepApprovers = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTSalesRep != null)
                    copyMSTSalesRep = this._MSTSalesRep.Clone();

                if (this._MSTSalesRepPayrolls != null)
                    copyMSTSalesRepPayrolls = GFunc.TACopyDataTable(_MSTSalesRepPayrolls);

                if(this._MSTSalesRepApprovers !=null)
                    copyMSTSalesRepApprovers = GFunc.TACopyDataTable(_MSTSalesRepApprovers);
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
                        this._MSTSalesRep = MSTSalesRep.New();
                        _MSTSalesRepPayrolls.Clear();
                        if (this._MSTSalesRepPayrolls.Fetch(cn, new MSTSalesRepPayRolls.Criteria(0, 1))==false)
                            throw new TAException(MsgID.Common.NewFail);
                        _MSTSalesRepApprovers.Clear();
                        if (this._MSTSalesRepApprovers.Fetch(cn, new MSTSalesRepApprovers.Criteria(0, 1)) == false)
                            throw new TAException(MsgID.Common.NewFail);


                        /* commented by YST on 2023/02/22 to fix the error that occurrs when query can't return EmNm for FinalSaleApprover
                        //Set Default Value ttm
                        
                        _MSTSalesRep.FinalSaleAppKey = SysOptionUtility.GetInt("DefaultFinalAprover1", cn);
                        _MSTSalesRep.FinalSaleApprover = GFunc.ExecuteScalar(cn, "SELECT EmNm FROM MST_SalesRep WHERE EmKey = " + _MSTSalesRep.FinalSaleAppKey.ToString()).ToString();
                        _MSTSalesRep.FinalSaleAppKey = int.Parse(GFunc.ExecuteScalar(cn, "select EmKey from MST_SalesRep where EmNm ='" + _MSTSalesRep.FinalSaleApprover + "' OR EmID='"+ _MSTSalesRep.FinalSaleApprover + "'").ToString());
                        _MSTSalesRep.MarginLimitForFinalApprover = SysOptionUtility.GetInt("FianlApproverProfitItemLimitForARQO", cn);
                        */

                        /* added by YST on 2023/02/22 */
                        DataTable dtApproverInfo = GFunc.ExecuteProc(cn, "MSTSalesRepFinalApproverInfo_Get", null);
                        if (dtApproverInfo != null && dtApproverInfo.Rows.Count > 0)
                        {
                            _MSTSalesRep.FinalSaleAppKey = GFunc.NEInt(dtApproverInfo.Rows[0]["FinalSaleApproverKey"], 0);
                            _MSTSalesRep.FinalSaleApprover = GFunc.NEStr(dtApproverInfo.Rows[0]["FinalSaleApproverNm"], "");
                            _MSTSalesRep.MarginLimitForFinalApprover = GFunc.NEInt(dtApproverInfo.Rows[0]["MarginLimitForFinalApprover"], 0);
                        }
                        /*end adding by YST */

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;

                        //Attach Events
                        this._MSTSalesRep.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Obj_PropertyChanged);
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
                    this._MSTSalesRep = copyMSTSalesRep;
                    this._MSTSalesRepPayrolls = copyMSTSalesRepPayrolls;
                    this._MSTSalesRepApprovers = copyMSTSalesRepApprovers;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTSalesRep = null;
                copyMSTSalesRepPayrolls = null;
                #endregion
            }
        }//Completed
        public bool GetEdit(int? EmKey)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTSalesRep copyMSTSalesRep = null;
            BOLib.MSTSalesRepPayRolls copyMSTSalesRepPayrolls = null;
            BOLib.MSTSalesRepApprovers copyMSTSalesApprovers = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTSalesRep != null)
                    copyMSTSalesRep = this._MSTSalesRep.Clone();

                if (this._MSTSalesRepPayrolls != null)
                    copyMSTSalesRepPayrolls = GFunc.TACopyDataTable(_MSTSalesRepPayrolls);

                if (this._MSTSalesRepApprovers != null)
                    copyMSTSalesApprovers = GFunc.TACopyDataTable(_MSTSalesRepApprovers);
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
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, EmKey, 0, _guID))
                            return false;

                        //Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey)==false)
                            return false;

                        //Add Lock
                        if (SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, EmKey)==false)
                            return false;

                        //Get Record                                 
                        if (this._MSTSalesRep.Fetch(cn, new MSTSalesRep.Criteria(EmKey, 1))==false)
                        {
                            MsgBox.Show(cn,MsgID.Common.GetFail); 
                            return false;
                        }

                        //Record Not Found
                        if (GFunc.NEInt(this._MSTSalesRep._emKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }


                        _MSTSalesRepPayrolls.Clear();
                        if (_MSTSalesRepPayrolls.Fetch(cn, new MSTSalesRepPayRolls.Criteria(EmKey, 1))==false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail); 
                            return false;
                        }

                        _MSTSalesRepApprovers.Clear();
                        if (_MSTSalesRepApprovers.Fetch(cn, new MSTSalesRepApprovers.Criteria(EmKey, 1)) == false)
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
                    this._MSTSalesRep = copyMSTSalesRep;
                    this._MSTSalesRepPayrolls = copyMSTSalesRepPayrolls;
                    this._MSTSalesRepApprovers = copyMSTSalesApprovers;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTSalesRep = null;
                copyMSTSalesRepPayrolls = null;
                #endregion
            }
        }//Completed
        public bool GetReadOnly(int? EmKey)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTSalesRep copyMSTSalesRep = null;
            BOLib.MSTSalesRepPayRolls copyMSTSalesRepPayrolls = null;
            BOLib.MSTSalesRepApprovers copyMSTSalesApprovers = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTSalesRep != null)
                    copyMSTSalesRep = this._MSTSalesRep.Clone();

                if (this._MSTSalesRepPayrolls != null)
                    copyMSTSalesRepPayrolls =  GFunc.TACopyDataTable(_MSTSalesRepPayrolls);

                if (this._MSTSalesRepApprovers != null)
                    copyMSTSalesApprovers = GFunc.TACopyDataTable(_MSTSalesRepApprovers);
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
                        if (this._MSTSalesRep.Fetch(cn, new MSTSalesRep.Criteria(EmKey, 1))==false)
                        {
                            MsgBox.Show(cn,MsgID.Common.GetFail); 
                            return false;
                        }
                        _MSTSalesRepPayrolls.Clear();
                        if (this._MSTSalesRepPayrolls.Fetch(cn, new MSTSalesRepPayRolls.Criteria(EmKey, 1))==false)
                        {
                            MsgBox.Show(cn,MsgID.Common.GetFail); 
                            return false;
                        }

                        _MSTSalesRepApprovers.Clear();
                        if (this._MSTSalesRepApprovers.Fetch(cn, new MSTSalesRepApprovers.Criteria(EmKey, 1)) == false)
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
                    this._MSTSalesRep = copyMSTSalesRep;
                    this._MSTSalesRepPayrolls = copyMSTSalesRepPayrolls;
                    this._MSTSalesRepApprovers = copyMSTSalesApprovers;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTSalesRep = null;
                copyMSTSalesRepPayrolls = null;
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

                        if (GFunc.IsNE(_MSTSalesRep))
                            _MSTSalesRep = MSTSalesRep.New();
                        GFunc.ConvertDataTableToObject(dtHeader, _MSTSalesRep);

                        _MSTSalesRepPayrolls = new MSTSalesRepPayRolls(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[0], _MSTSalesRepPayrolls);

                        _MSTSalesRepApprovers = new MSTSalesRepApprovers(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[0], _MSTSalesRepApprovers);

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
            int? newEmKey = 0;
            string autoID = string.Empty;
            BOLib.MSTSalesRep copyMSTSalesRep = null;
            BOLib.MSTSalesRepPayRolls copyMSTSalesRepPayrolls = null;
            BOLib.MSTSalesRepApprovers copyMSTSalesApprovers = null;

            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTSalesRep != null)
                    copyMSTSalesRep = this._MSTSalesRep.Clone();

                if (this._MSTSalesRepPayrolls != null)
                    copyMSTSalesRepPayrolls =  GFunc.TACopyDataTable(_MSTSalesRepPayrolls);

                if (this._MSTSalesRepApprovers != null)
                    copyMSTSalesApprovers = GFunc.TACopyDataTable(_MSTSalesRepApprovers);

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
                        if (this.IsNew && GFunc.IsNE(_MSTSalesRep._emID))
                        {
                            if (SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _MSTSalesRep._emNm)==false)
                                return false;

                            _MSTSalesRep._emID = autoID;
                        }
                        #endregion

                        #region Set default value for fields that cannot be empty but can have a general default value
                        _MSTSalesRep._userKey = GFunc.NEInt(_MSTSalesRep.UserKey, 0);
                        _MSTSalesRep._jobCostGrpKey = GFunc.NEInt(_MSTSalesRep.JobCostGrpKey, 0);
                        _MSTSalesRep._jobLabourItmKey = GFunc.NEInt(_MSTSalesRep.JobLabourItmKey, 0);

                        //Get Server Date and Time (sdt)
                        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                        _MSTSalesRep._createDate = GFunc.NEDateTime(_MSTSalesRep.CreateDate, svrDateTime);
                        _MSTSalesRep._createUserKey = GFunc.NEInt(_MSTSalesRep.CreateUserKey, AppInfor.currentUserKey);
                        _MSTSalesRep._lastModifiedDate = svrDateTime;
                        _MSTSalesRep._lastModifiedUserKey = AppInfor.currentUserKey;
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
                            if (_MSTSalesRep.Insert(cn, out newEmKey)==false)
                            {
                                MsgBox.Show(cn,MsgID.Common.SaveFail); 
                                return false;
                            }
                            if (SECPermUtility.Edit(cn, constPayrollPermID,false)==true)
                            {
                                if (_MSTSalesRepPayrolls.Insert(cn, newEmKey)==false)
                                {
                                    MsgBox.Show(cn, MsgID.Common.SaveFail); 
                                    return false;
                                }
                            }
                            if (SECPermUtility.Edit(cn, constApprovalPermID, false) == true)
                            {
                                if (_MSTSalesRepApprovers.Insert(cn, newEmKey) == false)
                                {
                                    MsgBox.Show(cn, MsgID.Common.SaveFail);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            if (_MSTSalesRep.Update(cn)==false)
                            {
                                MsgBox.Show(cn, MsgID.Common.SaveFail); 
                                return false;
                            }
                            if (SECPermUtility.Edit(cn, constPayrollPermID,false)==true)
                            {
                                if (_MSTSalesRepPayrolls.Delete(cn, new MSTSalesRepPayRolls.Criteria(_MSTSalesRep._emKey, 0, 0))==false)  
                                {
                                    MsgBox.Show(cn, MsgID.Common.SaveFail);
                                    return false;
                                }
                                if (_MSTSalesRepPayrolls.Insert(cn, _MSTSalesRep._emKey)==false)
                                {
                                    MsgBox.Show(cn, MsgID.Common.SaveFail);
                                    return false;
                                }
                            }
                            if (SECPermUtility.Edit(cn, constApprovalPermID, false) == true)
                            {
                                if (_MSTSalesRepApprovers.Delete(cn, new MSTSalesRepApprovers.Criteria(_MSTSalesRep._emKey, 0, 0)) == false)
                                {
                                    MsgBox.Show(cn, MsgID.Common.SaveFail);
                                    return false;
                                }
                                if (_MSTSalesRepApprovers.Insert(cn, _MSTSalesRep._emKey) == false)
                                {
                                    MsgBox.Show(cn, MsgID.Common.SaveFail);
                                    return false;
                                }
                            }
                        }
                        #endregion

                        #region For New Record perform: Locking, set new recordKey
                        if (IsNew)
                        {
                            if (SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newEmKey))
                                 _MSTSalesRep._emKey = newEmKey;
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
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _MSTSalesRep._emKey, _MSTSalesRep._emID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTSalesRep, _MSTSalesRepPayrolls, _MSTSalesRepApprovers });
                else
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _MSTSalesRep._emKey, _MSTSalesRep._emID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTSalesRep, _MSTSalesRepPayrolls, _MSTSalesRepApprovers });
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
                    this._MSTSalesRep = copyMSTSalesRep;
                    this._MSTSalesRepPayrolls = copyMSTSalesRepPayrolls;
                    this._MSTSalesRepApprovers = copyMSTSalesApprovers;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTSalesRep = null;
                copyMSTSalesRepPayrolls = null;
                #endregion
            }
        }//Completed
        public bool Delete()
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTSalesRep copyMSTSalesRep = null;
            BOLib.MSTSalesRepPayRolls copyMSTSalesRepPayrolls = null;
            BOLib.MSTSalesRepApprovers copyMSTSalesApprovers = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTSalesRep != null)
                    copyMSTSalesRep = this._MSTSalesRep.Clone();

                if (this._MSTSalesRepPayrolls != null)
                    copyMSTSalesRepPayrolls =  GFunc.TACopyDataTable(_MSTSalesRepPayrolls);

                if (this._MSTSalesRepApprovers != null)
                    copyMSTSalesApprovers = GFunc.TACopyDataTable(_MSTSalesRepApprovers);
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
                        else
                        {
                            //Check if Detail has any records, if have records, 
                            //must check if user have permission.Edit for detail
                            //Note: there is no delete permission for Sales Rep Payroll (only Read,Edit)
                            if (_MSTSalesRepPayrolls.Rows.Count > 0)
                            {
                                if (SECPermUtility.Edit(constPayrollPermID, true) == false)
                                    return false;
                            }
                            if (_MSTSalesRepApprovers.Rows.Count > 0)
                            {
                                if (SECPermUtility.Edit(constApprovalPermID, true) == false)
                                    return false;
                            }
                        }
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
                        if (SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _MSTSalesRep._emKey, GUID)==false)
                            return false;

                        //Check the record is used in other dependency tables
                        if (GFunc.CheckKeyDependantsExists(cn, "EMKey", _MSTSalesRep._emKey.Value, _MSTSalesRep._emID))
                            return false;

                        //Delete Record
                        if (_MSTSalesRep.Delete(cn, new MSTSalesRep.Criteria(_MSTSalesRep._emKey))==false)
                        {
                            MsgBox.Show(cn,MsgID.Common.DeleteFail); 
                            return false;
                        }

                        //ttm
                        if (_MSTSalesRepPayrolls.Delete(cn, new MSTSalesRepPayRolls.Criteria(_MSTSalesRep._emKey, 0, 0)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.SaveFail);
                            return false;
                        }

                        if (_MSTSalesRepApprovers.Delete(cn, new MSTSalesRepApprovers.Criteria(_MSTSalesRep._emKey, 0, 0)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.SaveFail);
                            return false;
                        }
                        //ttm

                        //Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey)==false)
                            return false;

                        //Create New
                        this._MSTSalesRep = MSTSalesRep.New();
                        if (this._MSTSalesRepPayrolls.Fetch(cn, new MSTSalesRepPayRolls.Criteria(0, 0, 1))==false)
                            throw new TAException(MsgID.Common.DeleteFail);
                        if (this._MSTSalesRepApprovers.Fetch(cn, new MSTSalesRepApprovers.Criteria(0, 0, 1)) == false)
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
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyMSTSalesRep._emKey, copyMSTSalesRep._emID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyMSTSalesRep, copyMSTSalesRepPayrolls });

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
                    this._MSTSalesRep = copyMSTSalesRep;
                    this._MSTSalesRepPayrolls = copyMSTSalesRepPayrolls;
                    this._MSTSalesRepApprovers = copyMSTSalesApprovers;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTSalesRep = null;
                copyMSTSalesRepPayrolls = null;
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
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTSalesRep._emKey, "EmKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTSalesRep._emID, "EmID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                }
                else
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, _MSTSalesRep._emKey, "EmKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTSalesRep._emID, "EmID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                }

                
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTSalesRep.EmNm, "EmNm", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTSalesRep.EmClass, "EmClass", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTSalesRep.EmRef, "EmRef", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTSalesRep.EmEmail, "EmEmail", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTSalesRep.UserKey, "UserKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTSalesRep.Inactive, "Inactive", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTSalesRep.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTSalesRep.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTSalesRep.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTSalesRep.Custom4, "Custom4", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTSalesRep.Custom5, "Custom5", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                #endregion

                if (e.PropertyMessage.Count == 0)
                {
                    //StoreProcedure Validation
                    if (_MSTSalesRep.Validation(cn, new MSTSalesRep.Criteria(_MSTSalesRep._emKey, _MSTSalesRep._emID), this.IsNew))
                    {
                        return true;
                    }
                    else
                    {
                        e.PropertyMessage.Add("EmID", SysMessageUtility.Get(cn, MsgID.Validation.DuplicateRecordID + "EmID"));
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
                foreach (DataRow dr in this._MSTSalesRepPayrolls.Rows)
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
                            Validation_DetailRelation(dr["TransDate"], false, ref processOK, e);

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

                foreach (DataRow dr in this._MSTSalesRepApprovers.Rows)
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
                            Validation_DetailRelation(dr["Approver"], false, ref processOK, e);

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
        public bool Validation_Detail(UltraGridRow grdrow, string fieldToCheck,string targetcontrol="")
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

                //Check for Duplicate records
                if (processOK)
                   // if(targetcontrol== "tagrdExpenditureList")
                   // Validation_DetailRelation(grdrow.Cells["TransDate"].Value, grdrow.IsAddRow, ref processOK, e);
                   //else 
                   if (targetcontrol== "tagrdApproverList")
                    Validation_DetailRelation(grdrow.Cells["Approver"].Value, grdrow.IsAddRow, ref processOK, e);

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
                BaseUtility.Validation(propValue, "TransType", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "TransDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "TransDes", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "TransAmt", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "TransDeptKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "TransGrpKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);

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
            //NOTE: There are no relationship validation require for SalesRep Payroll. So the following code is disabled. 
            //But we still keep these code in case there are other relationship validation in future
            //-----------------------------------------------------------------------------------------------------------------
            //bool errorFound = false;

            //var dupList = ObjMSTSalesRepPayrolls.AsEnumerable().ToList().FindAll(o =>
            //                (o.Field<DateTime?>("TransDate").Value == ((DateTime)propValue).Date)); //this will raise error when propValue is DBNull
            //var dupList = ObjMSTSalesRepPayrolls.AsEnumerable().ToList().FindAll(o =>
            //                (o.Field<DateTime?>("TransDate").Value == (GFunc.NEDateTime(propValue,DateTime.MaxValue)).Date));

            //if (IsAddRow)
            //{
            //    if (dupList.Count > 0)
            //        errorFound = true;
            //}
            //else
            //{
            //    if (dupList.Count > 1)
            //        errorFound = true;
            //}
            //if (errorFound)
            //{
            //    e.PropertyMessage.Add("rowError", "TransDate" + MsgID.Validation.DuplicateRecord);
            //    processOK = false;
            //}
            //else
            //    processOK = true;
            //-----------------------------------------------------------------------------------------------------------------



            bool errorFound = false;
            
            var dupList = ObjMSTSalesRepApprovers.AsEnumerable().ToList().FindAll(o =>
                            (o.Field<String>("Approver") == propValue.ToString()));

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
                e.PropertyMessage.Add("rowError", "ApproverKey" + MsgID.Validation.DuplicateRecord);
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
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _MSTSalesRep, _MSTSalesRepPayrolls,_MSTSalesRepApprovers }, ConstantCodeKey);
                
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _MSTSalesRep, _MSTSalesRepPayrolls, _MSTSalesRepApprovers }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
    }
}
