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
    public class MSTStockCountFactory : CommandBase
    {
        #region Member variables and constants

        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private DataTable _MSTStockCount = new DataTable();
        private bool _isReadOnly = false;
        private int _guID = 0;

        //System Code Key for this Factory.
        private GEnum.SystemCode constCodeKey = GEnum.SystemCode.Stock_Count;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        //Permission ID for this Factory.
        private const string constPermID = GVar.PermissionID.Stock_Take;
        public string PermID { get { return constPermID; } }

        //Error Event Declaration
        public GVar.ErrorEvent errorEvent = null;

        #endregion

        #region Factory Properties
      
        internal GEnum.InstanceMode InstanceMode
        {
            get
            {
                return this._instanceMode;
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
        public DataTable ObjMSTStockCounts
        {
            get
            {
                return this._MSTStockCount;
            }
            set
            {
                this._MSTStockCount = value;
            }
        }
        public string ErrorMessageID
        {
            get;
            set;
        }

        #endregion // Constructors

        //Constructors
        public MSTStockCountFactory(GEnum.InstanceMode instanceMode)
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

                            // Commit Process
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
        public bool StockCount_Get()
        {
            // Initialisation
            bool restoreFlag = false;
            DataTable copyMSTItmStockCount = null;

            try
            {
                // Copy original object
                if (this._MSTStockCount != null)
                    copyMSTItmStockCount = this._MSTStockCount.Copy();

                // Check Permission
                if (!SECPermUtility.Perform(constPermID, true))
                    return false;

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        // Check Lock
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, (int?)constCodeKey, 0, _guID))
                            return false;

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                            return false;

                        // Add Lock
                        if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, (int?)constCodeKey))
                            return false;
                        int Option = 0;
                        // Get Detail Values Records
                        this._MSTStockCount.Clear();
                        List<SqlParameter> paraList = new List<SqlParameter>();
                        paraList.Add(new SqlParameter("@Option", Option));
                        paraList.Add(new SqlParameter("@UserKey", AppInfor.CurrentUserKey));
                        _MSTStockCount = GFunc.ExecuteProc(cn, "MSTItmStockTake_Get", paraList);

                        this._isReadOnly = false;
                        restoreFlag = false;
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                    }
                }
                return true;
            }
            catch (TAException tex)
            {
                // Restore data when error is occur                    
                this._MSTStockCount = copyMSTItmStockCount;
                throw Error(tex);
            }
            catch (Exception ex)
            {
                // Restore data when error is occur                    
                this._MSTStockCount = copyMSTItmStockCount;
                throw Error(ex);
            }
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                    this._MSTStockCount = copyMSTItmStockCount;
                #endregion

                #region Dispose Backup Objects
                copyMSTItmStockCount = null;
                #endregion
            }
        }//Completed
        public bool NewStockCount_Get()
        {
            // Initialisation
            bool restoreFlag = false;
            DataTable copyMSTItmStockCount = null;

            try
            {
                // Copy original object
                if (this._MSTStockCount != null)
                    copyMSTItmStockCount = this._MSTStockCount.Copy();

                // Check Permission
                if (!SECPermUtility.Perform(constPermID, true))
                    return false;

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        // Get Detail Values Records
                        this._MSTStockCount.Clear();
                        List<SqlParameter> paraList = new List<SqlParameter>();
                        paraList.Add(new SqlParameter("@Option", 1));
                        paraList.Add(new SqlParameter("@UserKey", AppInfor.CurrentUserKey));
                        _MSTStockCount = GFunc.ExecuteProc(cn, "MSTItmStockTake_Get", paraList);

                        this._isReadOnly = false;
                        restoreFlag = false;
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                    }
                }
                return true;
            }
            catch (TAException tex)
            {
                // Restore data when error is occur                    
                this._MSTStockCount = copyMSTItmStockCount;
                throw Error(tex);
            }
            catch (Exception ex)
            {
                // Restore data when error is occur                    
                this._MSTStockCount = copyMSTItmStockCount;
                throw Error(ex);
            }
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                    this._MSTStockCount = copyMSTItmStockCount;
                #endregion

                #region Dispose Backup Objects
                copyMSTItmStockCount = null;
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
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            
        }//Completed

        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _MSTStockCount}, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _MSTStockCount }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        //#region Save Method

        //public bool Save()
        //{
        //    bool isSave = false;
        //    string msgID = String.Empty;
        //    if (this.IsNew)
        //        ErrorMessageID = MsgID.Common.AddFail;
        //    else
        //        ErrorMessageID = MsgID.Common.UpdateFail;

        //    bool isNewRecord = this.IsNew;
        //    int? newShipNameKey = 0;
        //    string autoID = string.Empty;

        //    bool isCommitTransFail = true;
        //    string recordID = string.Empty;

        //    if (this.InstanceMode == GEnum.InstanceMode.Normal)
        //    {
        //        try
        //        {
        //            if (this.IsOpenReadOnly)
        //            {
        //                MsgBox.Show(MsgID.Common.RecordIsReadOnly);
        //                return false;
        //            }
        //            else
        //            {
        //                if (isNewRecord)
        //                {
        //                    if (!SECPermUtility.Add(constPermID, true))
        //                    { return false; }
        //                }
        //                else
        //                {
        //                    if (!SECPermUtility.Edit(constPermID, true))
        //                    { return false; }
        //                }
        //            }

        //            // Create TransactionScope
        //            using (TransactionScope scope = new TransactionScope())
        //            {
        //                // Create SqlConnection
        //                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
        //                {
        //                    // Open Connection
        //                    cn.Open();

        //                    #region Set Server DateTime If Create and Modified Date is null
        //                    //Get Server Date and Time (sdt)
        //                    DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
        //                    //Set Header Obj
        //                    _MSTShipName.CreateDate = GFunc.NEDateTime(_MSTShipName.CreateDate, svrDateTime);
        //                    _MSTShipName.CreateUserKey = GFunc.NEInt(_MSTShipName.CreateUserKey, AppInfor.currentUserKey);

        //                    _MSTShipName.LastModifiedDate = svrDateTime;
        //                    _MSTShipName.LastModifiedUserKey = AppInfor.currentUserKey;

        //                    //Set Detail DataTable
        //                    foreach (DataRow dr in _MSTShipNameDetItms.Rows)
        //                    {
        //                        dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
        //                        dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
        //                        dr["LastModifiedDate"] = svrDateTime;
        //                        dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
        //                    }
        //                    #endregion

        //                    if (!this.ShipName_Validation(cn))
        //                    { return false; }

        //                    // Validation
        //                    if (!this.ShipNameList_Validation(cn))
        //                    { return false; }

        //                    // Backup ID
        //                    recordID = this._MSTShipName.ShipName;

        //                    // Get AutoID
        //                    if (isNewRecord && GFunc.IsNE(this._MSTShipName.ShipName))
        //                    {
        //                        if (SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, this._MSTShipName.ShipName))
        //                        {
        //                            this._MSTShipName.ShipName = autoID;
        //                        }
        //                    }

        //                    // Save Header Record
        //                    if (isNewRecord)
        //                    {
        //                        if (this._MSTShipName.Insert(cn, out newShipNameKey))
        //                        {
        //                            this._MSTShipName.ShipNameKey = (int)newShipNameKey;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (!this._MSTShipName.Update(cn))
        //                        { return false; }
        //                    }

        //                    if (!isNewRecord)
        //                    {
        //                        this._MSTShipNameDetItms.Delete(cn, new MSTShipNameDetItms.Criteria(this.ObjMSTShipName.ShipNameKey, 0));
        //                    }
        //                    this._MSTShipNameDetItms.Insert(cn, this.ObjMSTShipName.ShipNameKey);

        //                    // Record Locking
        //                    if (isNewRecord)
        //                    {
        //                        if (!SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newShipNameKey))
        //                        { return false; }
        //                    }

        //                    // Alert Process

        //                    // Commit Process
        //                    if (isNewRecord)
        //                        this._MSTShipName.ShipNameKey = (int)newShipNameKey;

        //                    _MSTShipName.IsDirty = false;
        //                    this._isNew = false;
        //                    msgID = string.Empty;
        //                    isSave = true;

        //                    // No errors - commit transaction
        //                      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

        //                    isCommitTransFail = false;

        //                }// End of SqlConnection
        //            }// End of TransactionScope

        //            // Audit Log
        //            if (isNewRecord)
        //                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, new object[] { this._MSTShipName }));
        //            else
        //                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, new object[] { this._MSTShipName }));

        //        }
        //        catch (TAException tex)
        //        {
        //            throw Error(tex);
        //        }
        //        catch (Exception ex)
        //        {
        //            // Restore the auto generated ID
        //            if (isNewRecord)
        //            {
        //                this._MSTShipName.ShipName = recordID;
        //            }
        //            // Restore the original new key to object
        //            if (isNewRecord)
        //                this._MSTShipName.ShipNameKey = 0;

        //            throw Error(ex);
        //        }
        //    }
        //    else
        //    {
        //        MsgBox.Show(MsgID.Common.WrongInstanceMode);
        //        return false;
        //    }
        //    return isSave;
        //}

        //#endregion //Save Method
        //#region GetReadOnly Method

        //public bool GetReadOnly(int ShipNameKey)
        //{
        //    bool isGetReadOnly = false;
        //    string msgID = MsgID.Common.GetFail;

        //    if (this.InstanceMode == GEnum.InstanceMode.Normal)
        //    {
        //        // Copy original object
        //        BOLib.MSTShipName copyMSTShipName = null;
        //        BOLib.MSTShipNameDetItms copyMSTShipNameDetItms = null;

        //        if (!GFunc.IsNE(this._MSTShipName))
        //            copyMSTShipName = this._MSTShipName.Clone();

        //        //detail data Table
        //        if (!GFunc.IsNE(this._MSTShipNameDetItms))
        //            copyMSTShipNameDetItms = this._MSTShipNameDetItms;
        //        else
        //        {
        //            this._MSTShipNameDetItms = new MSTShipNameDetItms();
        //            this._MSTShipNameDetItms.ColumnChanged += new DataColumnChangeEventHandler(Details_CollectionChanged);
        //        }

        //        try
        //        {
        //            // Check Permission
        //            if (!SECPermUtility.Read(constPermID, true))
        //            { return false; }

        //            // Create TransactionScope
        //            using (TransactionScope scope = new TransactionScope())
        //            {
        //                // Create SqlConnection
        //                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
        //                {
        //                    // Open Connection
        //                    cn.Open();

        //                    // Remove all locks by GUID except inprogress Locking
        //                    if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
        //                        return false;

        //                    // If Header Object is Null, Call New
        //                    if (GFunc.IsNE(this._MSTShipName))
        //                        this._MSTShipName = MSTShipName.New();

        //                    if (!GFunc.IsNE(msgID))
        //                    { return false; }


        //                    // Get Header Record                                 
        //                    if (!this._MSTShipName.Fetch(cn, new MSTShipName.Criteria(ShipNameKey)))
        //                    {
        //                        MsgBox.Show(msgID); return false;
        //                    }

        //                    // If Price Ratio List Object is Null, Call New
        //                    if (GFunc.IsNE(this._MSTShipNameDetItms))
        //                        this._MSTShipNameDetItms = MSTShipNameDetItms.New();

        //                    this._MSTShipNameDetItms.Clear();
        //                    if (!this._MSTShipNameDetItms.Fetch(cn, new MSTShipNameDetItms.Criteria(ShipNameKey, 1)))
        //                    {
        //                        MsgBox.Show(msgID); return false;
        //                    }

        //                    // Commit Process
        //                    this._MSTShipName.PropertyChanged += new PropertyChangedEventHandler(_MSTShipName_PropertyChanged);

        //                    _MSTShipName.IsDirty = false;
        //                    this._isNew = false;
        //                    this._isOpenReadOnly = true;
        //                    msgID = string.Empty;
        //                    isGetReadOnly = true;

        //                    // No errors - commit transaction
        //                      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

        //                }// End of SqlConnection
        //            }// End of TransactionScope
        //        }
        //        catch (TAException ex)
        //        {
        //            // Restore data when error is occur                    
        //            this._MSTShipName = copyMSTShipName;
        //            this._MSTShipNameDetItms = copyMSTShipNameDetItms;

        //            throw Error(ex);
        //        }
        //        catch (Exception ex)
        //        {
        //            // Restore data when error is occur                    
        //            this._MSTShipName = copyMSTShipName;
        //            this._MSTShipNameDetItms = copyMSTShipNameDetItms;

        //            throw Error(ex);
        //        }
        //        finally
        //        {
        //            // Set Null to Backup Objects
        //            copyMSTShipName = null;
        //            copyMSTShipNameDetItms = null;
        //        }
        //    }
        //    else
        //    {
        //        MsgBox.Show(MsgID.Common.WrongInstanceMode);
        //        return false;
        //    }
        //    return isGetReadOnly;
        //}

        //#endregion //GetReadOnly Method
    }
}
