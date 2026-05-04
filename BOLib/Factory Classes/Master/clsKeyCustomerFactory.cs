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
    public class KeyCustomerFactory : CommandBase
    {
        #region Member variables and constants

        private KeyCustomer _KeyCustomer = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isReadOnly = false;
        private int _guID = 0;
        private DataTable _KeyCustomerGrpByBudYear = null;

        

        //System Code Key for this Factory.
        private GEnum.SystemCode constCodeKey = GEnum.SystemCode.KeyCustomer;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        //Permission ID for this Factory.
        private string constPermID = GVar.PermissionID.KeyCustomer;
        public string PermID { get { return constPermID; } }

        //Event Declaration
        public GVar.DirtyEvent dirtyEvent = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;

        #endregion

        #region Factory Properties
        public KeyCustomer ObjKeyCustomer
        {
            get
            {
                return this._KeyCustomer;
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
        public KeyCustomerFactory(GEnum.InstanceMode instanceMode)
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
                            this._KeyCustomer = new KeyCustomer();
                            this._isNew = false;
                            this._isReadOnly = false;
                            if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                        }
                    }
                }
                else if (this.InstanceMode == GEnum.InstanceMode.InternalCall)
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
                            if (SysLockUtility.CheckInProgressLock(cn, true, constCodeKey))
                            {
                                this._guID = -1;
                                return true;
                            }

                            //Commit Process   
                            this._KeyCustomer = new KeyCustomer();
                            this._isNew = false;
                            this._isReadOnly = false;
                            if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
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
            BOLib.KeyCustomer copyKeyCustomer = null;
            #endregion

            try
            {

                #region Make backup of objects for restore purpose
                if (this._KeyCustomer != null)
                    copyKeyCustomer = this._KeyCustomer.Clone();
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
                        this._KeyCustomer = KeyCustomer.New();

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;

                        //Attach Events
                        this._KeyCustomer.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Obj_PropertyChanged);
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
                    this._KeyCustomer = copyKeyCustomer;
                }
                #endregion

                #region Dispose Backup Objects
                copyKeyCustomer = null;
                #endregion
            }
        }
        public bool GetEdit(int? grpKey, int? budgetYear)
        //(int? accKey, int? accID)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.KeyCustomer copyKeyCustomer = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._KeyCustomer != null)
                    copyKeyCustomer = this._KeyCustomer.Clone();
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
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, grpKey, 0, _guID))
                            return false;

                        //Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey) == false)
                            return false;

                        //Add Lock
                        if (SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, grpKey) == false)
                            return false;

                        //Get Record                                 
                        if (this._KeyCustomer.Fetch(cn, new KeyCustomer.Criteria(grpKey, budgetYear,2)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }
                        //Record Not Found
                        if (GFunc.NEInt(this._KeyCustomer._grpKey, 0) == 0 && GFunc.NEInt(this._KeyCustomer._budgetYear, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

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
                    this._KeyCustomer = copyKeyCustomer;
                #endregion

                #region Dispose Backup Objects
                copyKeyCustomer = null;
                #endregion
            }
        }
        public bool GetReadOnly(int? grpKey, int? budgetYear)
        //(int? accKey, string accID)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.KeyCustomer copyKeyCustomer = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._KeyCustomer != null)
                    copyKeyCustomer = this._KeyCustomer.Clone();
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
                        if (this._KeyCustomer.Fetch(cn, new KeyCustomer.Criteria(grpKey, budgetYear, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

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
                    this._KeyCustomer = copyKeyCustomer;
                }
                #endregion

                #region Dispose Backup Objects
                copyKeyCustomer = null;
                #endregion
            }
        }
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

                        if (GFunc.IsNE(_KeyCustomer))
                            _KeyCustomer = KeyCustomer.New();
                        GFunc.ConvertDataTableToObject(dtHeader, _KeyCustomer);

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

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
                                          // int? newRecordKey = 0;
            string autoID = string.Empty;
            BOLib.KeyCustomer copyKeyCustomer = null;

            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._KeyCustomer != null)
                    copyKeyCustomer = this._KeyCustomer.Clone();
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

                        #region Set default value for fields that cannot be empty but can have a general default value
                        _KeyCustomer._grpKey = GFunc.NEInt(_KeyCustomer._grpKey, 0);
                        _KeyCustomer._budgetYear = GFunc.NEInt(_KeyCustomer._budgetYear, 0);

                        //Get Server Date and Time(sdt)
                        //DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                        //_KeyCustomer._createDate = GFunc.NEDateTime(_KeyCustomer.CreateDate, svrDateTime);
                        //_KeyCustomer._createUserKey = GFunc.NEInt(_KeyCustomer.CreateUserKey, AppInfor.currentUserKey);
                        //_KeyCustomer._lastModifiedDate = svrDateTime;
                        //_KeyCustomer._lastModifiedUserKey = AppInfor.currentUserKey;
                        //#endregion

                        //#region Validation
                        //if (Validation_Header(cn) == false)
                        //    return false;
                        //#endregion

                        #region Save Record
                        //Note: there is no delete permission for Sales Rep Payroll (only Read,Edit)
                        if (IsNew)
                        {
                            if (_KeyCustomer.Insert(cn) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.SaveFail);
                                return false;
                            }
                        }
                        else
                        {
                            if (_KeyCustomer.Update(cn) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.SaveFail);
                                return false;
                            }
                        }
                        #endregion

                        #region For New Record perform: Locking, set new recordKey
                        //if (IsNew)
                        //{
                        //    if (SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newRecordKey))
                        //        _KeyCustomer._accKey = newRecordKey;
                        //    else
                        //        return false;
                        //}
                        #endregion

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

                        #region Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        #endregion

                        #endregion
                    }
                }

                #region Update Auditlog
                if (isNewRecord)
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _KeyCustomer._grpKey, _KeyCustomer._budgetYear.ToString(), GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _KeyCustomer });
                else
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _KeyCustomer._grpKey, _KeyCustomer._budgetYear.ToString(), GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _KeyCustomer });
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
                    this._KeyCustomer = copyKeyCustomer;
                #endregion

                #region Dispose Backup Objects
                copyKeyCustomer = null;
                #endregion
            }
        }
        public bool Delete()
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.KeyCustomer copyKeyCustomer = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._KeyCustomer != null)
                    copyKeyCustomer = this._KeyCustomer.Clone();
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
                        if (SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _KeyCustomer._grpKey, GUID) == false)
                            return false;

                       
                        ////Check the record is used in other dependency tables
                        //if (GFunc.CheckKeyDependantsExists(cn, "AccKey", _KeyCustomer._grpKey.Value, _KeyCustomer._budgetYear.ToString()))
                        //    return false;

                        //Delete Record
                        if (_KeyCustomer.Delete(cn, new KeyCustomer.Criteria(_KeyCustomer._grpKey, _KeyCustomer._budgetYear,0)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.DeleteFail);
                            return false;
                        }

                        //Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey) == false)
                            return false;

                        //Create New
                        this._KeyCustomer = KeyCustomer.New();

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

                        #region Set Flag
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;
                        #endregion

                        #endregion
                    }
                }

                //Audit Log                    
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyKeyCustomer.GrpKey, copyKeyCustomer.BudgetYear.ToString(), GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyKeyCustomer });

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
                    this._KeyCustomer = copyKeyCustomer;
                #endregion

                #region Dispose Backup Objects
                copyKeyCustomer = null;
                #endregion
            }
        }
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
                return false;
            }
            catch (Exception ex)
            {
                Error(ex);
                return false;
            }

        }

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
        }

        //Error
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _KeyCustomer }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _KeyCustomer }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
        public DataTable KeyCustomerGrpByBudYear
        {
            get
            {
                return this._KeyCustomerGrpByBudYear;
            }
        }
        public void GetRepsByGroup(int BudgetYear)
        {
            try
            {

                System.Data.DataSet dsResult = new System.Data.DataSet();
                using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(BOLib.Database.BossDemoConnection))
                {
                    System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand();
                    cm.CommandType = System.Data.CommandType.Text;
                    cm.CommandText = "SELECT * FROM BH_KeyCustomer WHERE BudgetYear = '" + BudgetYear + "' ORDER BY CustOrder ";


                    System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                    try
                    {
                        sqlCon.Open();
                        sqlAdp.Fill(dsResult);
                        _KeyCustomerGrpByBudYear = dsResult.Tables[0];
                       
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                }
                
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetTeamByEmId_And_BudgetYear(string EmID,int BudgetYear) //GetTeamByEmKey
        {
            try
            {
                string Team = "";
                //System.Data.DataSet dsResult = new System.Data.DataSet();
                using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(BOLib.Database.BossDemoConnection))
                {
                    System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand();
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    //cm.CommandText = "select Department from MST_SalesTeam where EmKey='" + EmKey + "' ";

                    cm.CommandText = "GetTeamByEmId_And_BudgetYear";
                    cm.Parameters.AddWithValue("@EmId", EmID);
                    cm.Parameters.AddWithValue("@BudgetYear", BudgetYear);                    


                    System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                    try
                    {
                        sqlCon.Open();
                        Team = cm.ExecuteScalar().ToString();
                        sqlCon.Close();
                        return Team;
                      

                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                }

            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetMaxCustomerOrder()
        {
            try
            {
                int MaxCusOrder = 0;
                //System.Data.DataSet dsResult = new System.Data.DataSet();
                using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(BOLib.Database.BossDemoConnection))
                {
                    System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand();
                    cm.CommandType = System.Data.CommandType.Text;
                    //cm.CommandText = "select Department from MST_SalesTeam where EmKey='" + EmKey + "' ";

                    cm.CommandText = "select MAX(CustOrder)+1 from BH_KeyCustomer";


                    System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                    try
                    {
                        sqlCon.Open();
                        MaxCusOrder = int.Parse(cm.ExecuteScalar().ToString());
                        sqlCon.Close();
                        return MaxCusOrder;
                      

                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                }

            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetListOfSaleRapByBudgetYear(int BudgetYear)
        {
            try
            {

                System.Data.DataSet dsResult = new System.Data.DataSet();
                using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(BOLib.Database.BossDemoConnection))
                {
                    System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand();
                    cm.CommandType = System.Data.CommandType.Text;
                    cm.CommandText = "SELECT EmID, EmNm "+
                                     " FROM MST_SalesRep "+
                                     "  WHERE EmID IN "+
                                     " ( "+
                                       " SELECT EmID "+
                                       " FROM MST_SalesTeam "+
                                       " WHERE "+BudgetYear+" BETWEEN YEAR(DateFrom) AND YEAR(DateTo) "+
                                       " ) "+
                                       " ORDER BY MST_SalesRep.EmID";


                    System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                    try
                    {
                        sqlCon.Open();
                        sqlAdp.Fill(dsResult);
                        return dsResult.Tables[0];

                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                }

            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetGrpKeyByConKey(int ConKey)
        {
            try
            {
                int GrpKey = 0;
                //System.Data.DataSet dsResult = new System.Data.DataSet();
                using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(BOLib.Database.BossDemoConnection))
                {
                    System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand();
                    cm.CommandType = System.Data.CommandType.Text;
                    //cm.CommandText = "select Department from MST_SalesTeam where EmKey='" + EmKey + "' ";

                    cm.CommandText = "if exists(select top 1 GrpKey from BH_KeyCustomer where ConKey1 = " + ConKey + ")" +
                                                " select top 1 GrpKey from BH_KeyCustomer where ConKey1 = " + ConKey +
                                                " else select max(GrpKey) + 1 from BH_KeyCustomer";


                    System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                    try
                    {
                        sqlCon.Open();
                        GrpKey = int.Parse(cm.ExecuteScalar().ToString());
                        sqlCon.Close();
                        return GrpKey;


                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                }

            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool AlreadyExistOrNot(int ConKey, int BudgetYear,int GrpKey)
        {
            bool result = false;
            try
            {
                
                //System.Data.DataSet dsResult = new System.Data.DataSet();
                using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(BOLib.Database.BossDemoConnection))
                {
                    System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand();
                    cm.CommandType = System.Data.CommandType.Text;
                    //cm.CommandText = "select Department from MST_SalesTeam where EmKey='" + EmKey + "' ";

                    cm.CommandText = "if exists(select * from BH_KeyCustomer where ConKey1= " + ConKey+ " and BudgetYear= "+BudgetYear+ " and GrpKey<>"+GrpKey+") select 1 else select 0";


                    System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                    try
                    {
                        
                        sqlCon.Open();
                        if (cm.ExecuteScalar().ToString() == "1") result = true;
                        else result = false;
                        sqlCon.Close();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                }

            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }
    }
} 
#endregion