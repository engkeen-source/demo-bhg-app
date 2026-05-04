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
using Infragistics.Win.UltraWinGrid;

namespace BOLib
{
    [Serializable()]
    public class REFCurrFactory : CommandBase
    {
        #region Member variables and constants

        private REFCurr _REFCurr = null;
        private REFCurrs _REFCurrs = null;
        private REFCurrDetItms _REFCurrDetItms = null;
        private REFCurrDetCons _REFCurrDetCons = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isReadOnly = false;
        private int _guID = 0;

        // System Code Key for this Factory.
        private GEnum.SystemCode constCodeKey = GEnum.SystemCode.Currency;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        // Permission ID for this Factory.
        private string constPermID = GVar.PermissionID.Currency;
        public string PermID { get { return constPermID; } }

        // Custom Event Declaration 
        public GVar.DirtyEvent dirtyEvent = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;
        public GVar.UINotifierEvent CurrKeyListNotifier = null;

        #endregion // Member variables and constant

        #region Factory Properties 
        public REFCurr ObjREFCurr
        {
            get
            {
                return this._REFCurr;
            }
            set
            {
                this._REFCurr = value;
            }
        }
        public REFCurrs ObjREFCurrs
        {
            get
            {
                return this._REFCurrs;
            }
            set
            {
                this._REFCurrs = value;
            }
        }
        public REFCurrDetItms ObjREFCurrDetItms
        {
            get
            {
                return this._REFCurrDetItms;
            }
            set
            {
                this._REFCurrDetItms = value;
            }
        }
        public REFCurrDetCons ObjREFCurrDetCons
        {
            get
            {
                return this._REFCurrDetCons;
            }
            set
            {
                this._REFCurrDetCons = value;
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
        public REFCurrFactory(GEnum.InstanceMode instanceMode)
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
                            this._REFCurr = new REFCurr();
                            this._REFCurrDetItms = new REFCurrDetItms(cn);
                            this._REFCurrDetCons = new REFCurrDetCons(cn);

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
                            this._REFCurr = new REFCurr();
                            this._REFCurrDetItms = new REFCurrDetItms(cn);
                            this._REFCurrDetCons = new REFCurrDetCons(cn);

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
                SetDefaultValue();
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
            REFCurr copyREFCurr = null;
            REFCurrDetItms copyREFCurrDetItms = null;
            REFCurrDetCons copyREFCurrDetCons = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._REFCurr != null)
                    copyREFCurr = this._REFCurr.Clone();

                if (this._REFCurrDetItms != null)
                    copyREFCurrDetItms = GFunc.TACopyDataTable(_REFCurrDetItms); 
                if (this._REFCurrDetCons != null)
                    copyREFCurrDetCons = GFunc.TACopyDataTable(_REFCurrDetCons); 
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
                        this._REFCurr = REFCurr.New();
                        this._REFCurrDetItms = new REFCurrDetItms(cn);
                        this._REFCurrDetCons = new REFCurrDetCons(cn);
                        SetDefaultValue();
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        #endregion

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;

                        //Attach Events
                        this._REFCurr.PropertyChanged += new PropertyChangedEventHandler(Obj_PropertyChanged);
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
                    this._REFCurr = copyREFCurr;
                    this._REFCurrDetItms = copyREFCurrDetItms;
                    this._REFCurrDetCons = copyREFCurrDetCons;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFCurr = null;
                copyREFCurrDetItms = null;
                copyREFCurrDetCons = null;
                #endregion
            }
        }//Completed
        public bool GetEdit(int CurrKey)
        {
            #region Declaration
            bool restoreFlag = false;
            REFCurr copyREFCurr = null;
            REFCurrDetItms copyREFCurrDetItms = null;
            REFCurrDetCons copyREFCurrDetCons = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._REFCurr != null)
                    copyREFCurr = this._REFCurr.Clone();

                if (this._REFCurrDetItms != null)
                    copyREFCurrDetItms = GFunc.TACopyDataTable(_REFCurrDetItms); 
                if (this._REFCurrDetCons != null)
                    copyREFCurrDetCons = GFunc.TACopyDataTable(_REFCurrDetCons); 
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

                        // Check Lock
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, CurrKey, 0, _guID))
                            return false;

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                            return false;

                        // Add Lock
                        if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, CurrKey))
                            return false;

                        #region Get Record
                        if (_REFCurr.Fetch(cn, new REFCurr.Criteria(CurrKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        //Record Not Found
                        if (GFunc.NEInt(this._REFCurr.CurrKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }

                        _REFCurrDetItms.Clear();
                        if (_REFCurrDetItms.Fetch(cn, new REFCurrDetItms.Criteria(CurrKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _REFCurrDetCons.Clear();
                        if (_REFCurrDetCons.Fetch(cn, new REFCurrDetCons.Criteria(CurrKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }
                        #endregion

                        SetDefaultValue();
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        this._isReadOnly = false;

                        //Attach Events
                        this._REFCurr.PropertyChanged += new PropertyChangedEventHandler(Obj_PropertyChanged);
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
                    this._REFCurr = copyREFCurr;
                    this._REFCurrDetItms = copyREFCurrDetItms;
                    this._REFCurrDetCons = copyREFCurrDetCons;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFCurr = null;
                copyREFCurrDetItms = null;
                copyREFCurrDetCons = null;
                #endregion
            }
        }//Completed
        public bool GetReadOnly(int CurrKey)
        {
            #region Declaration
            bool restoreFlag = false;
            REFCurr copyREFCurr = null;
            REFCurrDetItms copyREFCurrDetItms = null;
            REFCurrDetCons copyREFCurrDetCons = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._REFCurr != null)
                    copyREFCurr = this._REFCurr.Clone();

                if (this._REFCurrDetItms != null)
                    copyREFCurrDetItms = GFunc.TACopyDataTable(_REFCurrDetItms);
                if (this._REFCurrDetCons != null)
                    copyREFCurrDetCons =GFunc.TACopyDataTable(_REFCurrDetCons);


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

                        // Remove all locks by GUID except inprogress Locking
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey) == false)
                            return false;

                        #region Get Data
                        if (_REFCurr.Fetch(cn, new REFCurr.Criteria(CurrKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _REFCurrDetItms.Clear();
                        if (_REFCurrDetItms.Fetch(cn, new REFCurrDetItms.Criteria(CurrKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }
                        _REFCurrDetCons.Clear();
                        if (_REFCurrDetCons.Fetch(cn, new REFCurrDetCons.Criteria(CurrKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }
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
                    this._REFCurr = copyREFCurr;
                    this._REFCurrDetItms = copyREFCurrDetItms;
                    this._REFCurrDetCons = copyREFCurrDetCons;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFCurr = null;
                copyREFCurrDetItms = null;
                copyREFCurrDetCons = null;
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

                        if (GFunc.IsNE(_REFCurr))
                            _REFCurr = REFCurr.New();
                        GFunc.ConvertDataTableToObject(dtHeader, _REFCurr);

                        _REFCurrDetItms = new REFCurrDetItms(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[0], _REFCurrDetItms);
                        _REFCurrDetCons = new REFCurrDetCons(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[1], _REFCurrDetCons);
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
            int? newRecordKey = 0;
            string autoID = string.Empty;
            string msgID = string.Empty;
            REFCurr copyREFCurr = null;
            REFCurrDetItms copyREFCurrDetItms = null;
            REFCurrDetCons copyREFCurrDetCons = null;

            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._REFCurr != null)
                    copyREFCurr = this._REFCurr.Clone();

                if (this._REFCurrDetItms != null)
                    copyREFCurrDetItms = GFunc.TACopyDataTable(_REFCurrDetItms);
                if (this._REFCurrDetCons != null)
                    copyREFCurrDetCons = GFunc.TACopyDataTable(_REFCurrDetCons);

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
                        if (this.IsNew && GFunc.IsNE(_REFCurr._currID))
                        {
                            if (SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _REFCurr.CurrNm) == false)
                                return false;

                            _REFCurr._currID = autoID;
                        }
                        #endregion

                        #region Set default value for fields that cannot be empty but can have a general default value

                        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);//Get Server Date and Time (sdt)
                        _REFCurr._createDate = GFunc.NEDateTime(_REFCurr.CreateDate, svrDateTime);
                        _REFCurr._createUserKey = GFunc.NEInt(_REFCurr.CreateUserKey, AppInfor.currentUserKey);
                        _REFCurr._lastModifiedDate = svrDateTime;
                        _REFCurr._lastModifiedUserKey = AppInfor.currentUserKey;

                        //_REFCurrDetItms
                        foreach (DataRow dr in _REFCurrDetItms.Rows)
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

                        if (Validation_Detail("tagrdCurrDetailList", (DataTable)this.ObjREFCurrDetItms, cn) == false)
                            return false;
                        if (Validation_Detail("tagrdCurrDetailConList", (DataTable)this.ObjREFCurrDetCons, cn) == false)
                            return false;

                        #endregion

                        #region Save Record

                        if (IsNew)
                        {
                            if (!_REFCurr.Insert(cn, out newRecordKey))
                                return false;
      
                            if (!_REFCurrDetItms.Insert(cn, newRecordKey))
                                return false;                          

                            if (!_REFCurrDetCons.Insert(cn, newRecordKey))
                                return false;
                        }
                        else
                        {
                            if (!_REFCurr.Update(cn))
                                return false;

                            if (!_REFCurrDetItms.Delete(cn, new REFCurrDetItms.Criteria(_REFCurr.CurrKey, 0)))
                                return false;
                            if (!_REFCurrDetItms.Insert(cn, _REFCurr.CurrKey))
                                return false;

                            if (!_REFCurrDetCons.Delete(cn, new REFCurrDetCons.Criteria(_REFCurr.CurrKey, 0)))
                                return false;
                            if (!_REFCurrDetCons.Insert(cn, _REFCurr.CurrKey))
                                return false;
                        }
                        #endregion

                        #region For New Record perform: Locking, set new recordKey
                        if (IsNew)
                        {
                            if (SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newRecordKey))
                                _REFCurr.CurrKey = (int)newRecordKey;
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

                //  No need to auto currency sync from boss to estore.
                #region Update currency_rate (USD and EUR) for estore magento2

                //if (SysOptionUtility.DatabaseBranchCode == "BHM")
                //{
                    //int MagentoVersion = SysOptionUtility.GetInt("MagentoVersion");
                    //if (MagentoVersion == 2)
                    //{

                    //    if (_REFCurr.CurrKey == 11 || _REFCurr.CurrKey == 14)
                    //    {
                    //        bool retValue = false;
                    //        SqlConnection cn = new SqlConnection(AppInfor.CurrentDBConnectionStr);
                    //        cn.Open();
                    //        List<SqlParameter> parmList = new List<SqlParameter>();
                    //        parmList.Add(new SqlParameter("@CurrKey", _REFCurr.CurrKey));
                    //        parmList.Add(new SqlParameter("@RetValue", SqlDbType.Int));
                    //        parmList[1].Direction = ParameterDirection.Output;
                    //        GFunc.ExecuteNonQueryProc(cn, "[Update_Currencyrate_eStore_Magento2]", parmList);
                    //        if (GFunc.NEInt(parmList[1].Value, 0) == (int)GEnum.SpState.Pass)
                    //        {
                    //            retValue = true;
                    //        }
                    //        else
                    //        {
                    //            retValue = false;
                    //        }
                    //        cn.Close();
                    //        return retValue;
                    //    }
                    //}
                //}
                #endregion

                #region Update Auditlog
                if (isNewRecord)
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _REFCurr.CurrKey, _REFCurr.CurrID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _REFCurr, _REFCurrDetItms, _REFCurrDetCons });
                else
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _REFCurr.CurrKey, _REFCurr.CurrID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _REFCurr, _REFCurrDetItms, _REFCurrDetCons });
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
                    this._REFCurr = copyREFCurr;
                    this._REFCurrDetItms = copyREFCurrDetItms;
                    this._REFCurrDetCons = copyREFCurrDetCons;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFCurr = null;
                copyREFCurrDetItms = null;
                copyREFCurrDetCons = null;
                #endregion
            }
        }//Completed
        public bool Delete()
        {
            #region Declaration
            bool restoreFlag = false;

            BOLib.REFCurr copyREFCurr = null;
            BOLib.REFCurrDetCons copyREFCurrDetCons = null;
            BOLib.REFCurrDetItms copyREFCurrDetItms = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._REFCurr != null)
                    copyREFCurr = this._REFCurr.Clone();

                if (this._REFCurrDetItms != null)
                    copyREFCurrDetItms = GFunc.TACopyDataTable(_REFCurrDetItms);
                if (this._REFCurrDetCons != null)
                    copyREFCurrDetCons = GFunc.TACopyDataTable(_REFCurrDetCons);
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
                        if (SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _REFCurr.CurrKey, GUID) == false)
                            return false;

                        //Check the record is used in other dependency tables
                        if (GFunc.CheckKeyDependantsExists(cn, "CurrKey", (int)_REFCurr.CurrKey, _REFCurr.CurrID))
                            return false;

                        //Check for Option Table
                        if (GFunc.CheckKeyDependcyinOptionTable(cn, "Currency", _REFCurr._currKey.Value))
                            return false;

                        //Delete Record
                        if (_REFCurr.Delete(cn, new REFCurr.Criteria(_REFCurr.CurrKey)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.DeleteFail);
                            return false;
                        }

                        // Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey) == false)
                            return false;

                        // Create New
                        this._REFCurr = REFCurr.New();
                        this._REFCurrDetItms = new REFCurrDetItms(cn);

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

                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, _REFCurr.CurrKey, _REFCurr.CurrID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyREFCurr, copyREFCurrDetItms, copyREFCurrDetCons });
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
                    this._REFCurr = copyREFCurr;
                    this._REFCurrDetItms = copyREFCurrDetItms;
                    this._REFCurrDetCons = copyREFCurrDetCons;
                }
                #endregion

                #region Dispose Backup Objects
                copyREFCurr = null;
                copyREFCurrDetItms = null;
                copyREFCurrDetCons = null;
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
        public void SetDefaultValue()
        {
            //RefCurr
            


            //RefCurrDetItm
            _REFCurrDetItms.Columns["CurrKey"].DefaultValue = 0;
            //_REFCurrDetItms.Columns["CurrDate"].DefaultValue=Empty
            _REFCurrDetItms.Columns["CurrRate"].DefaultValue = 1;
            _REFCurrDetItms.Columns["CountryRate"].DefaultValue = 1;
            _REFCurrDetItms.Columns["CustomRate1"].DefaultValue = 1;
            _REFCurrDetItms.Columns["CustomRate2"].DefaultValue = 1;
            _REFCurrDetItms.Columns["CustomRate3"].DefaultValue = 1;
            //_REFCurrDetItms.Columns["CreateDate"].DefaultValue=Empty
            //_REFCurrDetItms.Columns["CreateUserKey"].DefaultValue=Empty
            //_REFCurrDetItms.Columns["LastModifiedDate"].DefaultValue=Empty
            //_REFCurrDetItms.Columns["LastModifiedUserKey"].DefaultValue=Empty
            //_REFCurrDetItms.Columns["Custom1"].DefaultValue=Empty
            //_REFCurrDetItms.Columns["Custom2"].DefaultValue=Empty
            //_REFCurrDetItms.Columns["Custom3"].DefaultValue=Empty


            //_REFCurrDetCons
            _REFCurrDetCons.Columns["CurrKey"].DefaultValue = 0;
            //_REFCurrDetCons.Columns["ConKey"].DefaultValue=Empty
            //_REFCurrDetCons.Columns["ConCurrDate"].DefaultValue=Current Date;
            _REFCurrDetCons.Columns["ConCurrRate"].DefaultValue = 1;
            _REFCurrDetCons.Columns["ConCustomRate1"].DefaultValue = 1;
            _REFCurrDetCons.Columns["ConCustomRate2"].DefaultValue = 1;
            _REFCurrDetCons.Columns["ConCustomRate3"].DefaultValue = 1;
            //_REFCurrDetCons.Columns["CreateDate"].DefaultValue=Empty
            //_REFCurrDetCons.Columns["CreateUserKey"].DefaultValue=Empty
            //_REFCurrDetCons.Columns["LastModifiedDate"].DefaultValue=Empty
            //_REFCurrDetCons.Columns["LastModifiedUserKey"].DefaultValue=Empty
            //_REFCurrDetCons.Columns["Custom1"].DefaultValue=;
            //_REFCurrDetCons.Columns["Custom2"].DefaultValue=;
            //_REFCurrDetCons.Columns["Custom3"].DefaultValue=;



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

                #region Validation for each Field
                if (this.IsNew)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.CurrKey, "CurrKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.CurrID, "CurrID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                }
                else
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.CurrKey, "CurrKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.CurrID, "CurrID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                }

                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.CurrNm, "CurrNm", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.TxHdom, "TxHdom", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.TxLdom, "TxLdom", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.SymHdom, "SymHdom", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);

                #endregion

                if (e.PropertyMessage.Count == 0)
                {
                    //StoreProcedure Validation
                    if (_REFCurr.Validation(cn, new REFCurr.Criteria(_REFCurr.CurrKey, _REFCurr.CurrKey), this.IsNew))
                    {
                        return true;
                    }
                    else
                    {
                        e.PropertyMessage.Add("CurrID", SysMessageUtility.Get(cn, MsgID.Validation.DuplicateRecordID + "CurrID"));
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
        private bool Validation_Detail(string grdNm, DataTable dt, SqlConnection cn)
        {
            //Validation Check for calls from Factory (Save method)
            string msgID = string.Empty;
            bool processOK = true;

            try
            {        
                foreach (DataRow drow in dt.Rows)
                {
                    msgID = string.Empty;
                    processOK = true;

                    if (drow.RowState == DataRowState.Deleted)
                        continue;
                    else
                    {
                        //Check Column values
                        UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
                        foreach (DataColumn c in drow.Table.Columns)
                        {
                            Validation_DetailCheck(drow, grdNm, drow[c.ColumnName.ToString()], c.ColumnName.ToString(), false, ref processOK, e);
                        }

                        //Check for Duplicate records
                        if (processOK)
                        {

                            Validation_DetailRelation(drow, grdNm, 0, false, ref processOK, e);
                            if (grdNm == "tagrdCurrDetailList")
                            {
                                Validation_DetailRelation(drow, grdNm, 0, false, ref processOK, e);
                            }
                            else if (grdNm == "tagrdCurrDetailConList")
                            {
                                Validation_DetailRelation(drow, grdNm, 0, false, ref processOK, e);
                            }
                        }

                        //Set RowError Text
                        if (processOK == false)
                        {
                            foreach (object key in e.PropertyMessage.Keys)
                            {
                                if (GFunc.IsNE(msgID) == false)
                                    msgID += " and ";

                                msgID += SysMessageUtility.Get(cn, e.PropertyMessage[key].ToString());
                            }

                            drow.RowError = msgID;
                            throw new TAException(BOLib.MsgID.Common.ValidationFail);
                        }
                        else
                            drow.RowError = string.Empty;
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

            try
            {
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

                //Check for Duplicate records when fieldToCheck is Empty (meaning RowBeforeUpdate)
                if (processOK && fieldToCheck == string.Empty)
                {
                    if (grdNm == "tagrdCurrDetailList")
                    {
                        Validation_DetailRelation(drow, grdNm, 0, grdrow.IsAddRow, ref processOK, e);
                    }
                    else if (grdNm == "tagrdCurrDetailConList")
                    {
                        Validation_DetailRelation(drow, grdNm, 0, grdrow.IsAddRow, ref processOK, e);
                    }
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
        public bool Validation_DetailCheck(DataRow drow, string grdNm, object propValue, string CheckNm, bool failonError, ref bool processOK, UINotifierEventArgs e)
        {
            switch (grdNm)
            {
                #region tagrdCurrKeyList Validation
                case "tagrdCurrDetailList":
                    BaseUtility.Validation(propValue, "CurrDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.Yes, null, null, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "CurrRate", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "CountryRate", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "CustomRate1", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "CustomRate2", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "CustomRate3", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "CreateDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.No, null, null, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "CreateUserKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "LastModifiedDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.No, null, null, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "LastModifiedUserKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "Custom1", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "Custom2", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "Custom3", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);

                    break;
                #endregion

                #region tagrdCurrDetailConList Validation
                case "tagrdCurrDetailConList":
                    BaseUtility.Validation(propValue, "ConKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "ConCurrDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.Yes, null, null, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "ConCurrRate", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "ConCustomRate1", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "ConCustomRate2", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "ConCustomRate3", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "CreateDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.No, null, null, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "CreateUserKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "LastModifiedDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.No, null, null, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "LastModifiedUserKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "Custom1", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "Custom2", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                    BaseUtility.Validation(propValue, "Custom3", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);

                    break;
                #endregion

            }
            return processOK;
        }//Completed
        public bool Validation_DetailRelation(DataRow dr, string grdNm, object propValue, bool IsAddRow, ref bool processOK, UINotifierEventArgs e)
        {
            bool errorFound = false;

            switch (grdNm)
            {
                #region tagrdCurrKeyList
                case "tagrdCurrDetailList":
                    var dupAss = ObjREFCurrDetItms.AsEnumerable().Count(p => p.Field<DateTime>("CurrDate") == (DateTime)dr["CurrDate"]);

                    if (IsAddRow)
                    {
                        if (dupAss > 0)
                            errorFound = true;
                    }
                    else
                    {
                        if (dupAss > 1)
                            errorFound = true;
                    }
                    if (errorFound)
                    {
                        e.PropertyMessage.Add("rowError", "CurrDate" + MsgID.Validation.DuplicateRecord);
                        processOK = false;
                    }
                    break;
                #endregion

                #region tagrdCurrKeyList
                case "tagrdCurrDetailConList":
                    dupAss = ObjREFCurrDetCons.AsEnumerable().Count(p => (p.Field<int>("ConKey") == (int)dr["ConKey"]) && (p.Field<DateTime>("ConCurrDate") == (DateTime)dr["ConCurrDate"]));

                    if (IsAddRow)
                    {
                        if (dupAss > 0)
                            errorFound = true;
                    }
                    else
                    {
                        if (dupAss > 1)
                            errorFound = true;
                    }
                    if (errorFound)
                    {
                        e.PropertyMessage.Add("rowError", "ConCurrDate" + MsgID.Validation.DuplicateRecord);
                        processOK = false;
                    }
                    break;
                #endregion

            }
            return processOK;
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

        //Error
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _REFCurr, _REFCurrDetItms }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _REFCurr, _REFCurrDetItms }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
    }

    /*[Serializable()]
    public class REFCurrFactory1 : CommandBase
    {
        #region Member variables and constants

        private REFCurr _REFCurr = null;
        private REFCurrs _REFCurrs = null;

        private REFCurrDetItms _REFCurrDetItms = null;
        private REFCurrDetItm _REFCurrDetItm = null;

        private REFCurrDetCons _REFCurrDetCons = null;
        private REFCurrDetCon _REFCurrDetCon = null;

        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isOpenReadOnly = false;
        private int _guID = 0;
        public string messageid;

        // System Code Key for this Factory.
        private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Currency;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }
        // Permission ID for this Factory.
        public const string constPermID = GVar.PermissionID.Currency;
        public GVar.ErrorEvent errorEvent = null;
        public GVar.ListErrorEvent listErrorEvent = null;

        //  Custom Event Declaration
        public GVar.UINotifierEvent CurrencyNotifier = null;
        public GVar.UINotifierEvent clearErrorNotifier = null;

        #endregion // Member variables and constant

        #region Factory Properties

        public REFCurr ObjREFCurr
        {
            get
            {
                return this._REFCurr;
            }
        }

        public REFCurrs ObjREFCurrs
        {
            get
            {
                return this._REFCurrs;
            }
        }


        public REFCurrDetItms ObjREFCurrDetItms
        {
            get
            {
                return this._REFCurrDetItms;
            }
        }



        public REFCurrDetCons ObjREFCurrDetCons
        {
            get
            {
                return this._REFCurrDetCons;
            }
        }

        public REFCurrDetCon ObjREFCurrDetCon
        {
            get
            {
                return this._REFCurrDetCon;
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

        public int? GUID
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
        public REFCurrFactory()
        {
            Initialisation();
        }

        #endregion // Constructors

        #region Initialization Method

        public bool Initialisation()
        {
            string msgID = MsgID.Common.InitialisationFail;

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
                    this._isOpenReadOnly = false;
                    msgID = string.Empty;

                    // No errors - commit transaction
                      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                }
            }
            return true;
        }

        #endregion //Initialisation Method

        #region GetEdit Method

        public bool GetEdit(int? currKey)
        {
            // Initialisation
            bool isGetEdit = false;
            string msgID = MsgID.Common.GetFail;
            // Copy original object
            BOLib.REFCurr copyREFCurr = null;
            BOLib.REFCurrDetItms copyREFCurrDetItms = null;
            BOLib.REFCurrDetCons copyREFCurrDetCons = null;
            if (!GFunc.IsNE(this._REFCurr))
                copyREFCurr = this._REFCurr.Clone();
            if (!GFunc.IsNE(this._REFCurrDetItms))
                copyREFCurrDetItms = this._REFCurrDetItms;
            else
            {
                this._REFCurrDetItms = new REFCurrDetItms();
                this._REFCurrDetItms.ColumnChanged += new DataColumnChangeEventHandler(Details_CollectionChanged);
            }
            if (!GFunc.IsNE(this._REFCurrDetCons))
                copyREFCurrDetCons = this._REFCurrDetCons;
            else
            {
                this._REFCurrDetCons = new REFCurrDetCons();
                this._REFCurrDetCons.ColumnChanged += new DataColumnChangeEventHandler(Details_CollectionChanged);
            }
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
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, currKey, 0, _guID))
                            return false;

                        // Remove Lock                       
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                            return false;

                        // Add Lock                      
                        if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, currKey))
                            return false;
                        if (_REFCurr == null)
                        {
                            _REFCurr = new REFCurr();
                        }

                        //Get Record
                        if (!_REFCurr.Fetch(cn, new REFCurr.Criteria(currKey, 1)))
                        {
                            MsgBox.Show(cn,msgID);
                            return false;
                        }

                        //Record Not Found
                        if (GFunc.NEInt(this._REFCurr._currKey, 0) == 0)
                        {                            
                            throw new TAException(MsgID.Common.GetFail);
                        }

                        if (_REFCurrDetItms == null)
                        {
                            _REFCurrDetItms = new REFCurrDetItms(cn);
                        }
                        _REFCurrDetItms.Rows.Clear();
                        if (!_REFCurrDetItms.Fetch(cn, new REFCurrDetItms.Criteria(currKey, DateTime.Today, 1)))
                        {
                            MsgBox.Show(cn,msgID);
                            return false;
                        }
                        if (_REFCurrDetCons == null)
                        {
                            _REFCurrDetCons = new REFCurrDetCons(cn);
                        }
                        _REFCurrDetCons.Rows.Clear();
                        if (!_REFCurrDetCons.Fetch(cn, new REFCurrDetCons.Criteria(currKey, DateTime.Today, 1)))
                        {
                            MsgBox.Show(cn,msgID);
                            return false;
                        }
                        this._REFCurr.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(REFCurr_PropertyChanged);
                        // Commit Process                          
                        this._isNew = false;
                        this._isOpenReadOnly = false;
                        msgID = string.Empty;
                        isGetEdit = true;
                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        // Set Null to Backup Objects
                        copyREFCurr = null;
                        copyREFCurrDetItms = null;
                        copyREFCurrDetCons = null;
                        _isDirty = false;

                        _REFCurrDetItms.Columns["CurrKey"].DefaultValue = _REFCurr._currKey;                        
                        _REFCurrDetItms.Columns["CurrRate"].DefaultValue = 1;
                        _REFCurrDetItms.Columns["CountryRate"].DefaultValue = 1;
                        _REFCurrDetItms.Columns["CustomRate1"].DefaultValue = 1;
                        _REFCurrDetItms.Columns["CustomRate2"].DefaultValue = 1;
                        _REFCurrDetItms.Columns["CustomRate3"].DefaultValue = 1;

                        _REFCurrDetCons.Columns["CurrKey"].DefaultValue = _REFCurr._currKey;
                        _REFCurrDetCons.Columns["ConCurrRate"].DefaultValue = 1;
                        _REFCurrDetCons.Columns["ConCustomRate1"].DefaultValue = 1;
                        _REFCurrDetCons.Columns["ConCustomRate2"].DefaultValue =1;
                        _REFCurrDetCons.Columns["ConCustomRate3"].DefaultValue = 1;
                       
                        //
                    }
                }
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                this._REFCurr = copyREFCurr;
                this._REFCurrDetItms = copyREFCurrDetItms;
                this._REFCurrDetCons = copyREFCurrDetCons;
                throw Error(ex);
            }
            return isGetEdit;
        }

        #endregion //GetEdit Method

        #region GetReadOnly Method

        public bool GetReadOnly(int? currKey)
        {
            bool isGetReadOnly = false;
            string msgID = MsgID.Common.GetFail;

            // Copy original object
            BOLib.REFCurr copyREFCurr = null;
            BOLib.REFCurrDetItms copyREFCurrDetItms = null;
            BOLib.REFCurrDetCons copyREFCurrDetCons = null;

            if (!GFunc.IsNE(this._REFCurr))
                copyREFCurr = this._REFCurr.Clone();

            if (!GFunc.IsNE(this._REFCurrDetItms))
                copyREFCurrDetItms = this._REFCurrDetItms;

            if (!GFunc.IsNE(this._REFCurrDetCons))
                copyREFCurrDetCons = this._REFCurrDetCons;
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
                        if (!_REFCurr.Fetch(cn, new REFCurr.Criteria(currKey, 1)))
                        {
                            MsgBox.Show(msgID);
                            return false;
                        }

                        if (_REFCurrDetItms == null)
                            _REFCurrDetItms = new REFCurrDetItms(cn);

                        _REFCurrDetItms.Rows.Clear();
                        if (!_REFCurrDetItms.Fetch(cn, new REFCurrDetItms.Criteria(currKey, DateTime.Today, 1)))
                        {
                            MsgBox.Show(msgID);
                            return false;
                        }

                        if (_REFCurrDetCons == null)
                            _REFCurrDetCons = new REFCurrDetCons(cn);

                        _REFCurrDetCons.Rows.Clear();
                        if (!_REFCurrDetCons.Fetch(cn, new REFCurrDetCons.Criteria(currKey, DateTime.Today, 1)))
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
                        copyREFCurr = null;
                        copyREFCurrDetItms = null;
                        copyREFCurrDetCons = null;
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
                // Set Null to Backup Objects
                this._REFCurr = copyREFCurr;
                this._REFCurrDetItms = copyREFCurrDetItms;
                this._REFCurrDetCons = copyREFCurrDetCons;
                throw Error(ex);
            }

            return isGetReadOnly;
        }

        #endregion //GetReadOnly Method

        #region New Method

        public bool New()
        {
            bool isNew = false;
            string msgID = MsgID.Common.NewFail;

            BOLib.REFCurr copyREFCurr = null;
            BOLib.REFCurrDetItms copyREFCurrDetItms = null;
            BOLib.REFCurrDetCons copyREFCurrDetCons = null;
            // Copy original object
            if (!GFunc.IsNE(this._REFCurr))
                copyREFCurr = this._REFCurr.Clone();

            if (!GFunc.IsNE(this._REFCurrDetItms))
                copyREFCurrDetItms = this._REFCurrDetItms;
            else
            {
                this._REFCurrDetItms = new REFCurrDetItms();
                this._REFCurrDetItms.ColumnChanged += new DataColumnChangeEventHandler(Details_CollectionChanged);
            }
            if (!GFunc.IsNE(this._REFCurrDetCons))
                copyREFCurrDetCons = this._REFCurrDetCons;
            else
            {
                this._REFCurrDetCons = new REFCurrDetCons();
                this._REFCurrDetCons.ColumnChanged += new DataColumnChangeEventHandler(Details_CollectionChanged);
            }
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
                        this._REFCurr = REFCurr.New();
                        this._REFCurrs = REFCurrs.New();

                        this._REFCurrDetItm = new REFCurrDetItm();
                        this._REFCurrDetCon = REFCurrDetCon.New();


                        // Call New for Detail                       
                        if (!this._REFCurrDetItms.Fetch(cn, new REFCurrDetItms.Criteria(0, DateTime.Today.Date, 1)))
                        {
                            MsgBox.Show(msgID);
                            return false;
                        }

                        // Call New for Detail    
                        //this._REFCurrDetCons = new REFCurrDetCons(cn);
                        if (!this._REFCurrDetCons.Fetch(cn, new REFCurrDetCons.Criteria(0, DateTime.Today.Date, 1)))
                        {
                            MsgBox.Show(msgID);
                            return false;
                        }

                        this._REFCurr.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(REFCurr_PropertyChanged);


                        this._isDirty = false;
                        this._isNew = true;
                        this._isOpenReadOnly = false;
                        msgID = string.Empty;
                        isNew = true;

                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        // Set Null to Backup Objects
                        copyREFCurr = null;
                        copyREFCurrDetItms = null;
                        copyREFCurrDetCons = null;

                    }
                }
                _REFCurrDetItms.Columns["CurrRate"].DefaultValue = 1;
                _REFCurrDetItms.Columns["CountryRate"].DefaultValue = 1;
                _REFCurrDetItms.Columns["CustomRate1"].DefaultValue = 1;
                _REFCurrDetItms.Columns["CustomRate2"].DefaultValue = 1;
                _REFCurrDetItms.Columns["CustomRate3"].DefaultValue = 1;
           
                _REFCurrDetCons.Columns["ConCurrRate"].DefaultValue = 1;
                _REFCurrDetCons.Columns["ConCustomRate1"].DefaultValue = 1;
                _REFCurrDetCons.Columns["ConCustomRate2"].DefaultValue = 1;
                _REFCurrDetCons.Columns["ConCustomRate3"].DefaultValue = 1;               
                
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                // Restore data when error is occur                    
                this._REFCurr = copyREFCurr;
                this._REFCurrDetItms = copyREFCurrDetItms;
                throw Error(ex);
            }
            return isNew;
        }

        #endregion //New Method

        #region Save Method

        public bool Save()
        {
            bool isSave = false;

            bool isNewRecord = this.IsNew;
            int? newCurrKey = 0;
            string autoID = string.Empty;

            bool isCommitTransFail = true;
            string recordID = string.Empty;

            try
            {
                if (this.IsOpenReadOnly)
                {
                    string msgID = MsgID.Common.RecordIsReadOnly;
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
                        recordID = this._REFCurr._currID;

                        // Get AutoID

                        if (isNewRecord && GFunc.IsNE(_REFCurr._currID))
                        {
                            if (!SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _REFCurr._currNm))
                                return false;

                            _REFCurr._currID = autoID;
                        }

                        #region Set Server DateTime If Create and Modified Date is null
                        //Get Server Date and Time (sdt)
                        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                        //Set Header Obj
                        _REFCurr.CreateDate = GFunc.NEDateTime(_REFCurr.CreateDate, svrDateTime);
                        _REFCurr.CreateUserKey = GFunc.NEInt(_REFCurr.CreateUserKey, AppInfor.currentUserKey);

                        _REFCurr.LastModifiedDate = svrDateTime;
                        _REFCurr.LastModifiedUserKey = AppInfor.currentUserKey;

                        //Set Detail DataTable

                        //_REFCurrDetItms
                        foreach (DataRow dr in _REFCurrDetItms.Rows)
                        {
                            dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
                            dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
                            dr["LastModifiedDate"] = svrDateTime;
                            dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
                        }

                        //_REFCurrDetCons
                        foreach (DataRow dr in _REFCurrDetCons.Rows)
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
                            if (!_REFCurr.Insert(cn, out newCurrKey))
                                throw new TAException(MsgID.Common.AddFail);

                            if (!_REFCurrDetItms.Insert(cn, newCurrKey))
                                throw new TAException(MsgID.Common.AddFail);

                            if (!_REFCurrDetCons.Insert(cn, newCurrKey))
                                throw new TAException(MsgID.Common.AddFail);
                        }
                        else
                        {
                            if (!_REFCurr.Update(cn))
                                throw new TAException(MsgID.Common.UpdateFail);

                            if (!_REFCurrDetItms.Delete(cn, new REFCurrDetItms.Criteria(_REFCurr._currKey, null, 0)))
                                throw new TAException(MsgID.Common.UpdateFail);
                            if (!_REFCurrDetItms.Insert(cn, _REFCurr._currKey))
                                throw new TAException(MsgID.Common.UpdateFail);

                            if (!_REFCurrDetCons.Delete(cn, new REFCurrDetCons.Criteria(_REFCurr._currKey, null, 0)))
                                throw new TAException(MsgID.Common.UpdateFail);
                            if (!_REFCurrDetCons.Insert(cn, _REFCurr._currKey))
                                throw new TAException(MsgID.Common.UpdateFail);
                        }


                        // Record Locking

                        if (isNewRecord)
                            if (!SysLockUtility.AddLock(cn, true, _guID, constCodeKey, newCurrKey))
                                return false;



                        // Commit Process                           
                        if (isNewRecord)
                            _REFCurr._currKey = newCurrKey;
                        this._isNew = false;

                        isSave = true;

                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        isCommitTransFail = false;
                        _isDirty = false;

                    }// End of SqlConnection
                }// End of TransactionScope

                // Audit Log                
                if (isNewRecord)
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey,_REFCurr._currKey,_REFCurr._currID, new object[] { _REFCurrs, _REFCurrDetItms,_REFCurrDetCons  });
                else
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _REFCurr._currKey, _REFCurr._currID, new object[] { _REFCurrs, _REFCurrDetItms, _REFCurrDetCons });

            }
            catch (TAException tex)
            {
                // Restore the auto generated ID
                this._REFCurr._currID = recordID;
                throw Error(tex);
            }
            catch (Exception ex)
            {
                if (isNewRecord)
                {
                    // Restore the auto generated ID
                    this._REFCurr._currID = recordID;
                }
                if (isCommitTransFail)
                    throw new TAException(MsgID.Validation.CommitTransFail);
                throw Error(ex);
            }

            return isSave;
        }

        #endregion //Save Method

        #region Delete Method

        public bool Delete()
        {
            bool isDelete = false;
            string msgID = MsgID.Common.DeleteFail;
            BOLib.REFCurr copyREFCurr = null;
            BOLib.REFCurrDetCons copyREFCurrDetCons = null;
            BOLib.REFCurrDetItms copyREFCurrDetItms = null;
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


                #region Make backup of objects for restore purpose
                if (this._REFCurr != null)
                    copyREFCurr = this._REFCurr.Clone();
                 if (this._REFCurrDetCons != null)
                    copyREFCurrDetCons =(REFCurrDetCons)this._REFCurrDetCons.Copy();
                 if (this._REFCurrDetItms != null)
                     copyREFCurrDetItms = (REFCurrDetItms)this._REFCurrDetItms.Copy();
                #endregion

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        // Record Locking
                        if (!SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _REFCurr._currKey, GUID))
                            return false;

                        // Check the record is used in other dependency tables
                        if (GFunc.CheckKeyDependantsExists(cn, "CurrKey", _REFCurr._currKey.Value, _REFCurr._currID))
                            return false;

                        //Check for Option Table
                        //if (GFunc.CheckKeyDependcyinOptionTable(cn, "Curr", _REFCurr._currKey.Value))
                        //    return false;

                        // Delete Record
                        if (!_REFCurr.Delete(cn, new REFCurr.Criteria(_REFCurr._currKey)))
                            return false;


                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                            return false;

                        // Create New                            
                        this._REFCurr = REFCurr.New();

                        this._REFCurrDetItms = new REFCurrDetItms(cn);
                        this._REFCurrDetCons = new REFCurrDetCons(cn);

                        this._isNew = true;
                        this._isDirty = false;
                        this._isOpenReadOnly = false;
                        msgID = string.Empty;
                        isDelete = true;

                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                    }// End of SqlConnection
                }// End of TransactionScope
                // AuditLog                
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey,copyREFCurr._currKey,copyREFCurr._currID, new object[] { _REFCurrs, copyREFCurrDetItms,copyREFCurrDetCons  }); 
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

        internal bool Validation(SqlConnection cn)
        {
            // Initialisation
            bool isValidation = true;
            string msgID = MsgID.Common.ValidationFail;

            string msgValue = string.Empty;
            //string propName = string.Empty;
            string processOK = GVar.gcPass;
            string errorMsgID = string.Empty;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());

            try
            {
                // Clear Error in UI
                if (!GFunc.IsNE(this.clearErrorNotifier))
                    this.clearErrorNotifier.Invoke(this, e);

                //MsgBox Error
                if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFCurr.CurrKey, "CurrKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn) != GVar.gcPass)
                {
                    MsgBox.Show(errorMsgID);
                    return false;
                }
                if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFCurr.CreateDate, "CreateDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn) != GVar.gcPass)
                {
                    MsgBox.Show(errorMsgID);
                    return false;
                }
                if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFCurr.CreateUserKey, "CreateUserKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, 0, int.MaxValue, e, cn) != GVar.gcPass)
                {
                    MsgBox.Show(errorMsgID);
                    return false;
                }
                if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFCurr.LastModifiedDate, "LastModifiedDate", GEnum.DataType.DateTime, GEnum.Require.No, null, GEnum.CompareOperator.NotEqual, null, null, null, e, cn) != GVar.gcPass)
                {
                    MsgBox.Show(errorMsgID);
                    return false;
                }
                if (BaseUtility.Validation(processOK, true, out errorMsgID, this._REFCurr.LastModifiedUserKey, "LastModifiedUserKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, 0, int.MaxValue, e, cn) != GVar.gcPass)
                {
                    MsgBox.Show(errorMsgID);
                    return false;
                }

                //Error Provider                
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.CurrID, "CurrID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.CurrNm, "CurrNm", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.TxHdom, "TxHdom", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.TxLdom, "TxLdom", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.SymHdom, "SymHdom", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._REFCurr.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);

                if (e.PropertyMessage.Count > 0)
                {
                    isValidation = false;

                    ErrorMessageID = MsgID.Common.ValidationFail;

                    if (!GFunc.IsNE(this.CurrencyNotifier))
                        this.CurrencyNotifier.Invoke(this, e);
                    return false;
                }
                else
                    isValidation = true;


                // StoreProcedure Validation
                if (e.PropertyMessage.Count == 0)
                {
                    if (this._REFCurr.Validation(cn, new REFCurr.Criteria(this._REFCurr._currKey, this._REFCurr._currID, 0), this.IsNew))
                    {
                        msgID = string.Empty;
                    }
                    else
                    {
                        ErrorMessageID = MsgID.Validation.DuplicateRecordID + "CurrencyID";
                        e.PropertyMessage.Add("CurrID", SysMessageUtility.Get(cn, ErrorMessageID));
                        if (!GFunc.IsNE(this.CurrencyNotifier))
                            this.CurrencyNotifier.Invoke(this, e);
                        return false;
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

        private bool ValidationForDetail(SqlConnection cn)
        {
            //Variable Declaration
            string msgID = string.Empty;
            string msgValue = string.Empty;
            bool processOk = true;
            List<REFCurrDetItm> objREFCurrDetItmTmp = null;
            List<REFTaxADetItm> objREFCurrDetConItmTmp = null;

            for (int i = 0; i < _REFCurrDetItms.Rows.Count; i++)
            {
                if (i == _REFCurrDetItms.Rows.Count - 1 && GFunc.IsNE(_REFCurrDetItms.Rows[i]["CurrDate"]))
                    break;

                processOk = BaseUtility.Validation(out msgID, this._REFCurrDetItms.Rows[i]["CurrDate"], "CurrDate", GEnum.DataType.DateTime, GEnum.Require.Yes, null, null, null, null, null);
                if (processOk)
                {
                    objREFCurrDetItmTmp = null;

                    //if found 
                    if (objREFCurrDetItmTmp != null)
                    {
                        //if more than one object found 
                        if (objREFCurrDetItmTmp.Count > 1)
                        {
                            processOk = false;
                            messageid = "CurrDate" + MsgID.Validation.DuplicateRecord;
                            this._REFCurrDetItms.Rows[i].RowError = SysMessageUtility.Get(cn, messageid);
                            throw new TAException(messageid);
                        }
                    }
                    //var dupList = this._REFCurrDetItms.AsEnumerable().ToList().FindAll(o =>
                    //        (o.Field<DateTime?>("CurrDate") == DateTime.Parse(this._REFCurrDetItms.Rows[i]["CurrDate"].ToString())));

                    //if (dupList.Count > 1)
                    //{
                    //    msgID = "CurrDate" + MsgID.Validation.DuplicateRecord;
                    //    this._REFCurrDetItms.Rows[i].RowError = SysMessageUtility.Get(cn, msgID);
                    //    MsgBox.Show(msgID);
                    //    return false;

                    //}
                }
                if (processOk)
                    processOk = BaseUtility.Validation(out msgID, this._REFCurrDetItms.Rows[i]["CurrRate"], "CurrRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null);

                if (processOk)
                    processOk = BaseUtility.Validation(out msgID, this._REFCurrDetItms.Rows[i]["CountryRate"], "CountryRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null);

                if (processOk)
                    processOk = BaseUtility.Validation(out msgID, this._REFCurrDetItms.Rows[i]["CustomRate1"], "CustomRate1", GEnum.DataType.Decimel, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null);

                if (processOk)
                    processOk = BaseUtility.Validation(out msgID, this._REFCurrDetItms.Rows[i]["CustomRate2"], "CustomRate2", GEnum.DataType.Decimel, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null);

                if (processOk)
                    processOk = BaseUtility.Validation(out msgID, this._REFCurrDetItms.Rows[i]["CustomRate3"], "CustomRate3", GEnum.DataType.Decimel, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null);

                if (processOk)
                    processOk = BaseUtility.Validation(out msgID, this._REFCurrDetItms.Rows[i]["Custom1"], "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);

                if (processOk)
                    processOk = BaseUtility.Validation(out msgID, this._REFCurrDetItms.Rows[i]["Custom2"], "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);

                if (processOk)
                    processOk = BaseUtility.Validation(out msgID, this._REFCurrDetItms.Rows[i]["Custom3"], "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);

                if (!processOk)
                {
                    this._REFCurrDetItms.Rows[i].RowError = SysMessageUtility.Get(cn, msgID);
                    break;
                }
                else
                    this._REFCurrDetItms.Rows[i].RowError = string.Empty;

                if (!processOk)
                {
                    break;
                }
                else
                    this._REFCurrDetItms.Rows[i].RowError = string.Empty;
            }
            if (!processOk)
            {
                return processOk;
            }
            for (int i = 0; i < _REFCurrDetCons.Rows.Count; i++)
            {
                if (i == _REFCurrDetCons.Rows.Count - 1 && GFunc.IsNE(_REFCurrDetCons.Rows[i]["ConKey"]) && GFunc.IsNE(_REFCurrDetCons.Rows[i]["ConCurrDate"]))
                    break;

                processOk = BaseUtility.Validation(out msgID, this._REFCurrDetCons.Rows[i]["ConKey"], "ConKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null);

                if (processOk)
                    processOk = BaseUtility.Validation(out msgID, this._REFCurrDetCons.Rows[i]["ConCurrDate"], "ConCurrDate", GEnum.DataType.DateTime, GEnum.Require.Yes, null, null, null, null, null);

                if (processOk)
                    {
                        objREFCurrDetConItmTmp = null;

                        //if found 
                        if (objREFCurrDetConItmTmp != null)
                        {
                            //if more than one object found 
                            if (objREFCurrDetConItmTmp.Count > 1)
                            {                                
                                processOk = false;
                                messageid = "ConCurrDate" + MsgID.Validation.DuplicateRecord;
                                this._REFCurrDetItms.Rows[i].RowError= SysMessageUtility.Get(cn, messageid);
                                throw new TAException(messageid);                                
                            }
                        }
                    }
                
                //if (processOk)
                //{

                //    var dupList = this._REFCurrDetCons.AsEnumerable().ToList().FindAll(o =>
                //            (o.Field<DateTime?>("ConCurrDate") == DateTime.Parse(this._REFCurrDetCons.Rows[i]["ConCurrDate"].ToString())) &&
                //            (o.Field<int?>("ConKey") == int.Parse(this._REFCurrDetCons.Rows[i]["ConKey"].ToString())));

                //    if (dupList.Count > 1)
                //    {
                //        msgID = "CurrDate" + MsgID.Validation.DuplicateRecord;
                //        processOk = false;
                //    }

                //}
                if (processOk)
                    processOk = BaseUtility.Validation(out msgID, this._REFCurrDetCons.Rows[i]["ConCurrRate"], "CurrRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null);

                if (processOk)
                    processOk = BaseUtility.Validation(out msgID, this._REFCurrDetCons.Rows[i]["ConCustomRate1"], "CustomRate1", GEnum.DataType.Decimel, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null);

                if (processOk)
                    processOk = BaseUtility.Validation(out msgID, this._REFCurrDetCons.Rows[i]["ConCustomRate2"], "CustomRate2", GEnum.DataType.Decimel, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null);

                if (processOk)
                    processOk = BaseUtility.Validation(out msgID, this._REFCurrDetCons.Rows[i]["ConCustomRate3"], "CustomRate3", GEnum.DataType.Decimel, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null);

                if (processOk)
                    processOk = BaseUtility.Validation(out msgID, this._REFCurrDetCons.Rows[i]["Custom1"], "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);

                if (processOk)
                    processOk = BaseUtility.Validation(out msgID, this._REFCurrDetCons.Rows[i]["Custom2"], "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);

                if (processOk)
                    processOk = BaseUtility.Validation(out msgID, this._REFCurrDetCons.Rows[i]["Custom3"], "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);


                if (!processOk)
                {

                    this._REFCurrDetCons.Rows[i].RowError = SysMessageUtility.Get(cn, msgID);
                }
                else
                    this._REFCurrDetCons.Rows[i].RowError = string.Empty;

                if (!processOk)
                {
                    break;
                }
                else
                    this._REFCurrDetCons.Rows[i].RowError = string.Empty;
            }

            return processOk;
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
                case "CurrKey":
                    if(!this.IsNew)
                        processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "CurrKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    break;

                case "CurrDate":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "CurrDate", GEnum.DataType.DateTime, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    break;

                case "CurrRate":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "CurrRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    break;

                case "CountryRate":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "CountryRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    break;

                case "CustomRate1":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "CustomRate1", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    break;

                case "CustomRate2":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "CustomRate2", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    break;

                case "CustomRate3":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "CustomRate3", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
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

            foreach (DataRow drs in this.ObjREFCurrDetItms.Rows)
            {
                DataRow dr = drs;
                if (CheckRow != null)
                {
                    dr = CheckRow;
                }

                #region Common Validation

                foreach (DataColumn dc in ObjREFCurrDetItms.Columns)
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
                    KeyCount = ObjREFCurrDetItms.AsEnumerable().Count(p => p.Field<DateTime>("CurrDate") == (DateTime)dr["CurrDate"]);

                    if (KeyCount > 1)
                    {
                        processOK = GVar.gcCancel;
                        e.PropertyMessage.Add("CurrDate", SysMessageUtility.Get(cn, "CurrDate" + MsgID.Validation.DuplicateRecord));
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

        public bool RefDetItmCon_Validation(DataRow CheckRow)
        {
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                cn.Open();
                return RefDetItmCon_Validation(cn, CheckRow);
            }
        }

        public string RefDetItmCon_Validation(SqlConnection cn, string CellKey, object Value, out string errorMsgID, UINotifierEventArgs e)
        {
            string processOK = GVar.gcPass;
            errorMsgID = string.Empty;

            switch (CellKey)
            {
                               
                case "CurrKey":
                    if (!this.IsNew)
                        processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "CurrKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    break;

                case "ConKey":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "ConKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    break;

                case "ConCurrDate":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "CurrDate", GEnum.DataType.DateTime, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    break;

                case "ConCurrRate":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "CurrRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    break;

                case "ConCustomRate1":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "CustomRate1", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    break;

                case "ConCustomRate2":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "CustomRate2", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    break;

                case "ConCustomRate3":
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, Value, "CustomRate3", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
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

        public bool RefDetItmCon_Validation(SqlConnection cn, DataRow CheckRow)
        {
            #region Declaration
            string processOK = GVar.gcPass;
            string msgValue = string.Empty;
            string errorMsgID = string.Empty;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
            int KeyCount = 0;
            #endregion

            foreach (DataRow drs in this.ObjREFCurrDetCons.Rows)
            {
                DataRow dr = drs;
                if (CheckRow != null)
                {
                    dr = CheckRow;
                }

                #region Common Validation

                foreach (DataColumn dc in ObjREFCurrDetCons.Columns)
                {
                    processOK = RefDetItmCon_Validation(cn, dc.ColumnName, dr[dc.ColumnName], out errorMsgID, e);
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
                    KeyCount = ObjREFCurrDetCons.AsEnumerable().Count(p => (p.Field<int>("ConKey") == (int)dr["ConKey"]) && (p.Field<DateTime>("ConCurrDate") == (DateTime)dr["ConCurrDate"]));

                    if (KeyCount > 1)
                    {
                        processOK = GVar.gcCancel;
                        e.PropertyMessage.Add("ConCurrDate", SysMessageUtility.Get(cn, "ConCurrDate" + MsgID.Validation.DuplicateRecord));
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

            if (!SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0))
                return false;

            isDispose = true;

            return isDispose;
        }

        #endregion //Dispose Method

        #region PropertyChanged

        private void REFCurr_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            string msgID = string.Empty;
            string msgValue = string.Empty;
            bool validateOk = true;

            if (!this._isOpenReadOnly)
            {
                _isDirty = true;

                //UI Validation
                switch (e.PropertyName)
                {
                    case "CurrID":
                        if (IsNew)
                            validateOk = BaseUtility.Validation(out msgID, _REFCurr._currID, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null);
                        else
                            validateOk = BaseUtility.Validation(out msgID, _REFCurr._currID, e.PropertyName, GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null);
                        break;
                    case "CurrNm":
                        validateOk = BaseUtility.Validation(out msgID, _REFCurr._currNm, e.PropertyName, GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null);
                        break;
                    case "TxHdom":
                        validateOk = BaseUtility.Validation(out msgID, _REFCurr._txHdom, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);
                        break;
                    case "TxLdom":
                        validateOk = BaseUtility.Validation(out msgID, _REFCurr._txLdom, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);
                        break;
                    case "SymHdom":
                        validateOk = BaseUtility.Validation(out msgID, _REFCurr._symHdom, e.PropertyName, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null);
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
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _REFCurr }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _REFCurr }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        #endregion
    } */
}
