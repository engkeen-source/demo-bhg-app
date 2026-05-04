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
    public class MSTItmLocOpenBalFactory : CommandBase
    {
        #region Member variables and constants
        private MSTItmLocOpenBals _MSTItmLocOpenBal = null;
        private MSTItmLocOpenBals _MSTItmLocOpenBals = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;

        private bool _isDirty = false;
        private bool _isReadOnly = false;
        private int _guID = 0;

        //System Code Key for this Factory.
        private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Inventory_Opening_Balance;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        //Permission ID for this Factory.
        private const string constPermID = GVar.PermissionID.Inventory_Opening_Balance;
        public string PermID { get { return constPermID; } }
        #endregion

        #region Factory Properties
        public MSTItmLocOpenBals ObjMSTItmLocOpenBal
        {
            get
            {
                return this._MSTItmLocOpenBal;
            }
        }
        public MSTItmLocOpenBals ObjMSTItmLocOpenBals
        {
            get
            {
                return this._MSTItmLocOpenBals;
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
        public bool IsReadOnly
        {
            get
            {
                return this._isReadOnly;
            }
        }
        public int? GUID
        {
            get
            {
                return this._guID;
            }
        }
        #endregion

        //Constructors, Initialisation
        public MSTItmLocOpenBalFactory(GEnum.InstanceMode instanceMode)
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
                            if (SysLockUtility.IsProcessLock(cn, true, GEnum.SysLockOption.ByCodKey, ConstantCodeKey, this._guID))
                            {
                                this._guID = -1;
                                return false;
                            }

                            //Add Inprogress Lock
                            if (!SysLockUtility.AddInprogressLock(cn, true, this._guID, ConstantCodeKey))
                            {
                                this._guID = -1;
                                return false;
                            }

                            //Commit Process   
                            _MSTItmLocOpenBals = MSTItmLocOpenBals.New(cn);
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

        public bool New()
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTItmLocOpenBals copyMSTItmLocOpenBals = null;
            #endregion

            try
            {

                #region Make backup of objects for restore purpose
                if (this._MSTItmLocOpenBals != null)
                    copyMSTItmLocOpenBals =  GFunc.TACopyDataTable(_MSTItmLocOpenBals);
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
                        _MSTItmLocOpenBals.Clear();
                        if (this._MSTItmLocOpenBals.Fetch(cn, new MSTItmLocOpenBals.Criteria(0, 2)) == false)
                            throw new TAException(MsgID.Common.NewFail);

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
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
                    this._MSTItmLocOpenBals = copyMSTItmLocOpenBals;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTItmLocOpenBals = null;
                #endregion
            }
        }//Completed
        public bool GetEdit(int? key)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTItmLocOpenBals copyMSTItmLocOpenBals = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTItmLocOpenBals != null)
                    copyMSTItmLocOpenBals =  GFunc.TACopyDataTable(_MSTItmLocOpenBals);
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
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, key, 0, _guID))
                            return false;

                        //Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey) == false)
                            return false;

                        //Add Lock
                        if (SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, key) == false)
                            return false;

                        //Get Record                                 
                        _MSTItmLocOpenBals.Clear();
                        if (_MSTItmLocOpenBals.Fetch(cn, new MSTItmLocOpenBals.Criteria(key, 2)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
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
                    this._MSTItmLocOpenBals = copyMSTItmLocOpenBals;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTItmLocOpenBals = null;
                #endregion
            }
        }//Completed
        public bool GetReadOnly(int? key)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTItmLocOpenBals copyMSTItmLocOpenBals = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTItmLocOpenBals != null)
                    copyMSTItmLocOpenBals =  GFunc.TACopyDataTable(_MSTItmLocOpenBals);
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
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                            return false;

                        #region Get Data
                        _MSTItmLocOpenBals.Clear();
                        if (_MSTItmLocOpenBals.Fetch(cn, new MSTItmLocOpenBals.Criteria(key, 2)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        #endregion

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        _isDirty = false;
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
                    this._MSTItmLocOpenBals = copyMSTItmLocOpenBals;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTItmLocOpenBals = null;
                #endregion
            }
        }//Completed
        public bool SetReadOnlyData(DataSet dsDetail)
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

                        if (GFunc.IsNE(_MSTItmLocOpenBals))
                            _MSTItmLocOpenBals = MSTItmLocOpenBals.New();

                        GFunc.ConvertDataTableToObject(dsDetail.Tables[0], _MSTItmLocOpenBals);

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
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
        public bool Save(int key, int ItmType, decimal UnitCost)
        {
            //For Master and Reference the RecordKey is obtain after saving. 
            //But we do not need to update this Record to the Record Detail as
            //the saving process will delete all details in the server and append the detail from local to server again
            //Note: this will only work if the detail in the server is never updated by other user.
            //So for example : Item Location, the above will not work
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTItmLocOpenBals copyMSTItmLocOpenBals = null;
            DataSet ds = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTItmLocOpenBals != null)
                    copyMSTItmLocOpenBals =  GFunc.TACopyDataTable(_MSTItmLocOpenBals);

                #endregion


                #region Check Permission
                if (this.IsReadOnly)
                {
                    MsgBox.Show(MsgID.Common.RecordIsReadOnly);
                    return false;
                }
                else
                {
                    if (SECPermUtility.Edit(constPermID, true) == false)
                        return false;
                }
                #endregion

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,DocUtility.GetTransOption()))
                { 
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        #region Save Data
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;


                        if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, -1000))
                            return false;

                        #region Set default value for fields that cannot be empty but can have a general default value
                        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);//Get Server Date and Time (sdt)
                        foreach (DataRow dr in _MSTItmLocOpenBals.Rows)
                        {
                            dr["ItmKey"] = key;
                            dr["BatchKey"] = GFunc.NEInt(dr["BatchKey"], 0);
                            dr["Qty"] = GFunc.NEDec(dr["Qty"], 0);
                            dr["DatePurchase"] = GFunc.NEDateTime(dr["DatePurchase"], svrDateTime);
                            dr["BatchExpDate"] = GFunc.NEDateTime(dr["BatchExpDate"], svrDateTime);
                            dr["BatchMfgDate"] = GFunc.NEDateTime(dr["BatchMfgDate"], svrDateTime);
                            dr["BatchCost"] = GFunc.NEDec(dr["BatchCost"], 0);
                        }
                        _MSTItmLocOpenBals.AcceptChanges();
                        #endregion

                        DataTable dt_OpenBal = this.ObjMSTItmLocOpenBals;
                        dt_OpenBal.TableName = "IN_Opening";
                        string XmlData = GFunc.ConvertDataTableToXML(dt_OpenBal);

                        //Validation For Sufficient Stock
                        if (Validation_Detail(cn, key, ItmType) == false)
                            return false;

                        // save 
                        //to add right parameters
                        List<SqlParameter> paraList = new List<SqlParameter>();
                        paraList.Add(new SqlParameter("@ItmKey", key));
                        paraList.Add(new SqlParameter("@UnitCost", UnitCost));
                        paraList.Add(new SqlParameter("@ItmType", ItmType));
                        paraList.Add(new SqlParameter("@XmlData", XmlData));
                        SqlParameter RetValue = new SqlParameter();
                        RetValue.ParameterName = "@RetValue";
                        RetValue.Value = 0;
                        RetValue.Direction = ParameterDirection.InputOutput;
                        paraList.Add(RetValue);

                        SqlParameter ValidationFailed = new SqlParameter();
                        ValidationFailed.ParameterName = "@ValidationFailed";
                        ValidationFailed.Value = 0;
                        ValidationFailed.Direction = ParameterDirection.InputOutput;
                        paraList.Add(ValidationFailed);

                        ds = GFunc.ExecuteProcDataSet(cn, "MstItmLocOpenBal_Save", paraList);
                        if (GFunc.NEInt(RetValue.Value, 0) == (int)GEnum.SpState.Fail)
                        {
                            if (GFunc.NEInt(ValidationFailed.Value, 0) == 1)
                            {
                                MsgBox.Show(cn, "Can not Save. The Batch you deleted is used in other transaction");
                            }
                            else if (GFunc.NEInt(ValidationFailed.Value, 0) == 2)
                            {
                                MsgBox.Show(cn, "For the same Batch, invalid information is entered. \n Please check your Batch details.");
                            }
                            return false;
                        }

                        // Record Locking
                        if (SysLockUtility.RemoveLock(cn, false, (int)GEnum.SysLockOption.ByCodeKeyAndDataKeyAndInprogressKeyAndGUID, constCodeKey, GUID, -1000, 0) == false)
                            return false;

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete(); 

                        #region Set Flags
                        this._isDirty = false;
                        #endregion

                        #endregion
                    }
                }

                #region Update Auditlog
                //we need to add a header obj so that the auditlog is able to display details even if the record has been deleted in the MSTItm
                MSTItm objItmTemp = MSTItm.Get(key);

                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, objItmTemp.ItmKey, objItmTemp.ItmID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { objItmTemp, _MSTItmLocOpenBals });

                objItmTemp = null;
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
                    this._MSTItmLocOpenBals = copyMSTItmLocOpenBals;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTItmLocOpenBals = null;
                #endregion
            }
        }//Completed
        public bool Dispose()
        {
            try
            {
                if (GFunc.IsNE(GUID) == false)
                {
                    if (!SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0))
                        return false;
                }
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
        public bool Validation_Detail(SqlConnection cn, int ItmKey, int ItmType)
        {
            //Validation Check for calls from Factory (Save method)
            string msgID = string.Empty;
            bool processOK = true;
            string strXml = string.Empty;

            try
            {
                foreach (DataRow dr in this._MSTItmLocOpenBals.Rows)
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
                            Validation_DetailCheck(ItmType, dr[c.ColumnName.ToString()], c.ColumnName.ToString(), false, ref processOK, e);
                        }

                        //Check for Duplicate records
                        if (processOK)
                        {
                            strXml = GFunc.ConvertDataTableToXML((DataTable)_MSTItmLocOpenBals);
                            return Validation_DetailRelation(cn, strXml, ItmKey, ItmType);
                        }
                        else
                        {
                            //Set RowError Text
                            foreach (object key in e.PropertyMessage.Keys)
                            {
                                if (GFunc.IsNE(msgID) == false)
                                    msgID += " and ";

                                msgID += SysMessageUtility.Get(cn, e.PropertyMessage[key].ToString());
                            }

                            GFunc.SetRowError(dr, msgID);
                            throw new TAException(BOLib.MsgID.Common.ValidationFail);
                        }
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
        public bool Validation_Detail(UltraGridRow grdrow, string fieldToCheck, int ItmKey, int ItmType)
        {
            //Validation Check for calls from FORM (CustomCellUpdate, BeforeRowUpdate)
            string msgID = string.Empty;
            string strXml = string.Empty;
            bool processOK = true;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());

            try
            {
                //Check Column values
                if (fieldToCheck == string.Empty)
                {
                    foreach (UltraGridCell c in grdrow.Cells)
                    {
                        Validation_DetailCheck(ItmType, c.Value, c.Column.Key, false, ref processOK, e);
                    }
                }
                else
                    Validation_DetailCheck(ItmType, grdrow.Cells[fieldToCheck].Value, fieldToCheck, false, ref processOK, e);


                //Set RowError Text
                foreach (object key in e.PropertyMessage.Keys)
                {
                    if (GFunc.IsNE(msgID) == false)
                        msgID += " and ";

                    msgID += SysMessageUtility.Get(e.PropertyMessage[key].ToString());
                    ((DataRowView)(grdrow.ListObject)).Row.RowError = msgID;
                    throw new TAException(BOLib.MsgID.Common.ValidationFail);
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
        public bool Validation_DetailCheck(int ItmType, object propValue, string CheckNm, bool failonError, ref bool processOK, UINotifierEventArgs e)
        {
            try
            {
                BaseUtility.Validation(propValue, "ItmKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "BatchKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "LocKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DatePurchase", CheckNm, GEnum.DataType.DateTime, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "Qty", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "BatchCost", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);

                switch (ItmType)
                {
                    case (int)GEnum.ItemType.StockB:
                    case (int)GEnum.ItemType.Finished_GDB:
                    case (int)GEnum.ItemType.Serial_StockB:
                    case (int)GEnum.ItemType.Serial_Finished_GDB:
                        BaseUtility.Validation(propValue, "BatchID", CheckNm, GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, ref processOK, failonError, e);
                        break;

                    default:
                        BaseUtility.Validation(propValue, "BatchID", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                        break;
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
        public bool Validation_DetailRelation(SqlConnection cn, string xmlData, int ItmKey, int ItmType)
        {
            //Variable Declaration
            int InsufficientStockQty = 0;
            int InsufficientLocQty = 0;
            int DuplicateLocID = 0;
            int InsufficientBatchQty = 0;
            int DuplicateBatchID = 0;
            DataTable errResult = null;
            int WarnInsufficientStockQty;
            int WarnInsufficientLocQty;

            try
            {
                if (ObjMSTItmLocOpenBals.Validation(cn, new MSTItmLocOpenBals.Criteria((int?)constCodeKey, (int?)ItmKey, ItmType, 0, xmlData), ref errResult, ref InsufficientStockQty, ref InsufficientLocQty, ref DuplicateLocID, ref InsufficientBatchQty, ref DuplicateBatchID))
                    return true;

                if (InsufficientBatchQty > 0 || DuplicateBatchID > 0 || DuplicateLocID > 0)
                {
                    MsgBoxGrid.Show(cn, "Validation Failed", errResult, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
                    return false;
                }

                if (InsufficientStockQty > 0 || InsufficientLocQty > 0)
                {
                    WarnInsufficientStockQty = SysOptionUtility.GetInt(MsgID.SystemOption.Posting.AllowOutOfStock, cn);
                    WarnInsufficientLocQty = SysOptionUtility.GetInt(MsgID.SystemOption.Posting.AllowOutOfStockLocation, cn);

                    if (WarnInsufficientStockQty == 30 || WarnInsufficientLocQty == 30) //Out of Stock not allow
                    {
                        MsgBoxGrid.Show(cn, "Insufficient Qty", errResult, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
                        return false;
                    }

                    if (WarnInsufficientStockQty == 20 || WarnInsufficientLocQty == 20) //Warn Out of Stock can continue saving
                    {
                        if (MsgBoxGrid.Show(cn, "Insufficient Qty, continue Saving", errResult, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                            return false;
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
        public bool HasDependentBatch(int BatchKey, int ItmType)
        {
            //Check the record is used in other dependency tables
            SqlConnection cn = null;

            try
            {
                if (GFunc.IsNEZ(BatchKey))
                    return false;

                switch (ItmType)
                {
                    case (int)GEnum.ItemType.StockB:
                    case (int)GEnum.ItemType.Serial_StockB:
                    case (int)GEnum.ItemType.Finished_GDB:
                    case (int)GEnum.ItemType.Serial_Finished_GDB:
                        using (cn = new SqlConnection(Database.BossDemoConnection))
                        {
                            cn.Open();
                            if (GFunc.CheckBatchDependantsExists(cn, BatchKey, (int)ConstantCodeKey, 0))
                                return true;
                        }
                        break;
                }
                return false;
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
                if (cn.State == ConnectionState.Open) cn.Close();
            }
        }//Completed

        //Error
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _MSTItmLocOpenBals }, constCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _MSTItmLocOpenBals }, constCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
    }
}

