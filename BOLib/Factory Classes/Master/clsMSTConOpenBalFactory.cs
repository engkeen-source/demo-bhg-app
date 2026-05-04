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
    public class MSTConOpenBalFactory : CommandBase
    {
        #region Member variables and constants

        private MSTConOpenBals _MSTConOpenBals = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;           
        private bool _isReadOnly = false;
        private int _guID = 0;

        //System Code Key for this Factory.
        private GEnum.SystemCode constCodeKey = GEnum.SystemCode.Delivery_Order;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        //Permission ID for this Factory.
        private string constPermID = GVar.PermissionID.Delivery_Order;
        public string PermID { get { return constPermID; } }

        #endregion // Member variables and constant

        #region Factory Properties
        public MSTConOpenBals ObjMSTConOpenBals
        {
            get
            {
                return this._MSTConOpenBals;
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
                this._isDirty=value;
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
        public MSTConOpenBalFactory(GEnum.InstanceMode instanceMode,GEnum.SystemCode _CodeKey)
        {
            this._instanceMode = instanceMode;
            constCodeKey = _CodeKey;
            if (constCodeKey == GEnum.SystemCode.AP_Opening_Balance)
                constPermID = GVar.PermissionID.Vendor_Opening_Balance;
            else
                constPermID = GVar.PermissionID.Customer_Opening_Balance;   //for Credit and Cash the permission is the same
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
                            _MSTConOpenBals = MSTConOpenBals.New(cn, constCodeKey);
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
            BOLib.MSTConOpenBals copyMSTConOpenBals = null;
            #endregion

            try
            {
                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                {
                    #region Make backup of objects for restore purpose
                    if (this._MSTConOpenBals != null)
                        copyMSTConOpenBals =  GFunc.TACopyDataTable(_MSTConOpenBals);
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
                            _MSTConOpenBals.Clear();
                            if (this._MSTConOpenBals.Fetch(cn, new MSTConOpenBals.Criteria(0, (int)ConstantCodeKey,1)) == false)
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
                    this._MSTConOpenBals = copyMSTConOpenBals;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTConOpenBals = null;
                #endregion
            }
        }//Completed
        public bool GetEdit(int? key)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTConOpenBals copyMSTConOpenBals = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTConOpenBals != null)
                    copyMSTConOpenBals =  GFunc.TACopyDataTable(_MSTConOpenBals);
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
                            if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, key, 0, _guID))
                                return false;

                            //Remove Lock
                            if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey) == false)
                                return false;

                            //Add Lock
                            if (SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, key) == false)
                                return false;

                            //Get Record                                 
                            _MSTConOpenBals.Clear();
                            if (_MSTConOpenBals.Fetch(cn, new MSTConOpenBals.Criteria(key, (int)ConstantCodeKey, 1)) == false)
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
                    this._MSTConOpenBals = copyMSTConOpenBals;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTConOpenBals = null;
                #endregion
            }

        }//Completed
        public bool GetReadOnly(int? key)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTConOpenBals copyMSTConOpenBals = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTConOpenBals != null)
                    copyMSTConOpenBals =  GFunc.TACopyDataTable(_MSTConOpenBals);
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
                            if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                                return false;

                            #region Get Data
                            _MSTConOpenBals.Clear();
                            if (_MSTConOpenBals.Fetch(cn, new MSTConOpenBals.Criteria(key,(int)ConstantCodeKey, 1)) == false)
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
                    this._MSTConOpenBals = copyMSTConOpenBals;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTConOpenBals = null;
                #endregion
            }

        }//Completed
        public bool SetReadOnlyData(DataSet dsDetail)
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

                            if (GFunc.IsNE(_MSTConOpenBals))
                                _MSTConOpenBals = MSTConOpenBals.New();

                            //GFunc.ConvertDataTableToObject(dsDetail.Tables[0], _MSTConOpenBals);//to remove 

                            GFunc.CopyDataTableToDetailObject(dsDetail.Tables[0], _MSTConOpenBals);

                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                            //Set Flags
                            this._isDirty = false;
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
        }//Completed
        public bool Save(int key)
        {
            //For Master and Reference the RecordKey is obtain after saving. 
            //But we do not need to update this Record to the Record Detail as
            //the saving process will delete all details in the server and append the detail from local to server again
            //Note: this will only work if the detail in the server is never updated by other user.
            //So for example : Item Location, the above will not work
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTConOpenBals copyMSTConOpenBals = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTConOpenBals != null)
                    copyMSTConOpenBals =  GFunc.TACopyDataTable(_MSTConOpenBals);

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
                        if (SECPermUtility.Edit(constPermID, true) == false)
                            return false;
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

                            #region Set default value for fields that cannot be empty but can have a general default value

                            DateTime svrDateTime = GFunc.GetSvrDateTime(cn);//Get Server Date and Time (sdt)
                            foreach (DataRow dr in _MSTConOpenBals.Rows)
                            {
                                //dr["DocKey"] = 0;
                                dr["DocKey"] = GFunc.NEInt(dr["DocKey"], 0);
                                dr["DocCodeKey"] = ConstantCodeKey;
                                dr["DocBranchKey"] = GFunc.NEInt(dr["DocBranchKey"],0) ;
                                dr["DocDeptKey"] = GFunc.NEInt(dr["DocDeptKey"], 0);
                                dr["DocGrpKey"] = GFunc.NEInt(dr["DocGrpKey"], 0);
                                dr["DocGrand"] = GFunc.NEDec(dr["DocGrand"], 0);
                                dr["DocCurrKey"] = GFunc.NEInt(dr["DocCurrKey"], 1);
                                dr["DocCurrRate"] = GFunc.NEDec(dr["DocCurrRate"], 1);
                                dr["DocHome"] = GFunc.NEDec(dr["DocHome"], 0);
                                dr["DocApplyAmtF"] = GFunc.NEDec(dr["DocApplyAmtF"], 0);
                                dr["DocApplyAmtH"] = GFunc.NEDec(dr["DocApplyAmtH"], 0);
                                dr["DocRevalueAmtH"] = GFunc.NEDec(dr["DocRevalueAmtH"], 0);
                                dr["DocRevalueRate"] = GFunc.NEDec(dr["DocRevalueRate"], 0);
                                dr["DocState"] = GFunc.NEInt(dr["DocState"], 100);              //Posted
                                dr["PurgeKeep"] = GFunc.NEInt(dr["PurgeKeep"], 0);
                                dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
                                dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
                                dr["LastModifiedDate"] = svrDateTime;
                                dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
                            }
                            #endregion

                            #region Validation
                            if (Validation_Detail(cn) == false)
                                return false;
                            
                            #endregion

                            #region Save Record
                            _MSTConOpenBals.AcceptChanges();
                            _MSTConOpenBals.Save(cn,new MSTConOpenBals.Criteria(key, 0),(int)this.constCodeKey);                              
                            #endregion

                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                            #region Set Flags
                            this._isDirty = false;
                            #endregion

                            #endregion
                        }
                    }

                    #region Update Auditlog
                    //we need to add a header obj so that the auditlog is able to display details even if the record has been deleted in the MSTCon
                    MSTCon objConTemp = MSTCon.Get(key);

                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, objConTemp.ConKey, objConTemp.ConID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { objConTemp, _MSTConOpenBals });

                    objConTemp = null;
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
                    this._MSTConOpenBals = copyMSTConOpenBals;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTConOpenBals = null;
                #endregion
            }
        }//Completed
        public bool Dispose()
        {
            string msgID = string.Empty;

            if (this.InstanceMode == GEnum.InstanceMode.Normal)
            {
                if (!SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0))
                    return false;
            }

            return true;
        }//Completed        

        //Validation is already done in BeforeRowUpdate event of grid in form.
        public bool Validation_Detail(SqlConnection cn)
        {
            try
            {    
                //Validation Check for calls from Factory (Save method)
                string msgID = string.Empty;
                bool processOK = true;

                foreach (DataRow dr in this._MSTConOpenBals.Rows)
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
                            Validation_DetailRelation(dr["DocID"], false, ref processOK, e);

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

                //Check for Duplicate records
                if (processOK)
                    Validation_DetailRelation(grdrow.Cells["DocID"].Value, grdrow.IsAddRow, ref processOK, e);

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
                BaseUtility.Validation(propValue, "DocCodeKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocBranchKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocID", CheckNm, GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocConKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocDeptKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocAccKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocGrpKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocGrand", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocCurrKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocCurrRate", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocHome", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocApplyAmtF", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocApplyAmtH", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocApplyFull", CheckNm, GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocRevalueAmtH", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocRevalueRate", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocPOID", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocDOID", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocRef", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocDes", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocRem", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocStatus", CheckNm, GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "DocState", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "PurgeKeep", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(propValue, "PurgeData", CheckNm, GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, ref processOK, failonError, e);
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

            try
            {
                var dupDocID = ObjMSTConOpenBals.AsEnumerable().ToList().FindAll(o => (o.Field<string>("DocID") == (propValue.ToString())));
                if (IsAddRow)
                {
                    if (dupDocID.Count > 0)
                        errorFound = true;
                }
                else
                {
                    if (dupDocID.Count > 1)
                        errorFound = true;
                }
                if (errorFound)
                {
                    e.PropertyMessage.Add("rowError", "DocID" + MsgID.Validation.DuplicateRecord);
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
        
        //Error
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _MSTConOpenBals}, constCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _MSTConOpenBals }, constCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

    }
}

