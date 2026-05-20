using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Transactions;
using System.Collections.Specialized;
using System.Collections;
using Infragistics.Win.UltraWinGrid;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class SYSFinRepFactory
    {
        #region Member variables and constants
        private MSTFinMain _MstFinMain = null;
        private MSTFinMains _MstFinMains = null;
        private MSTFinDetails _MstFinDetails = null;
        private MSTFinRows _MstFinRows = null;
        private MSTFinColumns _MstFinColumns = null;
        private MSTFinDesigners _MstFinDesigners = null;

        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isReadOnly = false;
        private int _guID = 0;
        private bool _isError = false;
        private bool _isDirty = false;
        private GEnum.SystemCode _codeKey = GEnum.SystemCode.Financial_Statement;
        private string _permID = GVar.PermissionID.Financial_Reports;
        Hashtable htFinRepHeads = new Hashtable();
        #endregion

        #region Custom Event Declaration
        public GVar.UINotifierEvent SYSFinRepNotifier = null;
        public GVar.UINotifierEvent SYSFinRepHeadsNotifier = null;
        public GVar.UINotifierEvent clearErrorNotifier = null;
        #endregion

        #region Factory Properties

        public MSTFinMain MSTFinMain
        {
            get
            {
                return this._MstFinMain;
            }
            set
            {
                _MstFinMain = value;
            }
        }
        public MSTFinMains MSTFinMains
        {
            get
            {
                return this._MstFinMains;
            }
            set
            {
                _MstFinMains = value;
            }
        }
        public MSTFinDetails MstFinDetails
        {
            get
            {
                return this._MstFinDetails;
            }
            set
            {
                _MstFinDetails = value;
            }
        }
        public MSTFinRows MstFinRows
        {
            get
            {
                return this._MstFinRows;
            }
            set
            {
                _MstFinRows = value;
            }
        }
        public MSTFinColumns MstFinColumns
        {
            get
            {
                return this._MstFinColumns;
            }
            set
            {
                _MstFinColumns = value;
            }
        }
        public MSTFinDesigners MstFinDesigners
        {
            get
            {
                return this._MstFinDesigners;
            }
            set
            {
                _MstFinDesigners = value;
            }
        }

        public bool IsError
        {
            get
            {
                return _isError;
            }
        }
        public int CodeKey
        {
            get
            {
                return (int)_codeKey;
            }
        }
        public string PermID
        {
            get
            {
                return _permID;
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
            set
            {
                _isDirty = value;
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

        //Contructors, Initialisation
        public SYSFinRepFactory(GEnum.InstanceMode instanceMode)
        {
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
                    //We cannot use throw to indicate an initialisation failure because the calling form should only close the form and there
                    //is no need to show message as all message is already display by the utility function
                    //therefore we use isErorr = true to indicate failure to initialise factory
                    //in this function the return of true or false actually is not use at this point of time
                    //Function return - True/False - not used
                    //IsError status - True/False - used to check if the initialised of factory has failed
                    if (SECPermUtility.Any(_permID, out _isReadOnly, true) == false)
                        return false;

                    using (TransactionScope scope = new TransactionScope())
                    {
                        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                        {
                            cn.Open();

                            if ((_guID = SysOptionUtility.GetNewLockingGUID(cn)) == 0)
                            {
                                this._guID = -1;
                                return false;
                            }

                            //if (SysLockUtility.IsProcessLock(cn, false, GEnum.SysLockOption.ByCodKey, _codeKey, _guID))
                            //{
                            //    this._guID = -1;
                            //    return false;
                            //}

                            //if (SysLockUtility.AddInprogressLock(cn, true, _guID, _codeKey) == false)
                            //{
                            //    this._guID = -1;
                            //    return false;
                            //}

                            #region prepare New instances
                            _MstFinMain = new MSTFinMain();
                            _MstFinMains = new MSTFinMains(cn);
                            _MstFinDetails = new MSTFinDetails(cn);
                            _MstFinRows = new MSTFinRows(cn);
                            _MstFinColumns = new MSTFinColumns(cn);
                            _MstFinDesigners = new MSTFinDesigners(cn);
                            this._isReadOnly = false;

                            #endregion

                            if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                        }
                    }

                    return true;
                }
                else
                {
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
        public DataTable FetchFinRepsMain()
        {
            //Get List of Financial Statement from MST_FinMain
            try
            {
                _MstFinMains.Clear();
                _MstFinMains.Fetch(new MSTFinMains.Criteria(0, 0));
                return _MstFinMains;
            }
            catch (TAException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }//Completed
        public bool GetEdit(int FinRepKey)
        {
            #region Declaration
            MSTFinMain copyMstFinMain = null;
            MSTFinDesigners copyMstFinDesigners = null;
            MSTFinDetails copyMstFinDetails = null;
            MSTFinRows copyMstFinRows = null;
            MSTFinColumns copyMstFinColumns = null;

            bool restoreFlag = false;
            #endregion

            try
            {

                #region Backup original object or Attach Eventhandler to new object
                if (_MstFinMain != null)
                {
                    copyMstFinMain = _MstFinMain;
                }
                else
                {
                    _MstFinMain = new MSTFinMain();
                    _MstFinMain.PropertyChanged += new PropertyChangedEventHandler(_SYSFinRep_PropertyChanged);
                }

                if (_MstFinDetails != null)
                    copyMstFinDetails = GFunc.TACopyDataTable(_MstFinDetails);
                else
                {
                    _MstFinDetails = new MSTFinDetails();
                    _MstFinDetails.ColumnChanged += new DataColumnChangeEventHandler(Details_CollectionChanged);
                }

                if (_MstFinRows != null)
                    copyMstFinRows = GFunc.TACopyDataTable(_MstFinRows);
                else
                {
                    _MstFinRows = new MSTFinRows();
                    _MstFinRows.ColumnChanged += new DataColumnChangeEventHandler(Details_CollectionChanged);
                }

                if (_MstFinColumns != null)
                    copyMstFinColumns = GFunc.TACopyDataTable(_MstFinColumns);
                else
                {
                    _MstFinColumns = new MSTFinColumns();
                    _MstFinColumns.ColumnChanged += new DataColumnChangeEventHandler(Details_CollectionChanged);
                }

                if (_MstFinDesigners != null)
                    copyMstFinDesigners = GFunc.TACopyDataTable(_MstFinDesigners);
                else
                {
                    _MstFinDesigners = new MSTFinDesigners();
                    _MstFinDesigners.ColumnChanged += new DataColumnChangeEventHandler(Details_CollectionChanged);
                }

                #endregion

                #region Check Permission
                if (!SECPermUtility.Perform(_permID, true))
                    return false;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        #region Check, remove and add lock
                        //May uncommented on 25 Nov 2014
                        //// Check Lock
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, _codeKey, FinRepKey, 0, _guID))
                            return false;

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, _guID, _codeKey))
                            return false;

                        // Add Lock
                        if (!SysLockUtility.AddLock(cn, true, _guID, _codeKey, FinRepKey))
                            return false;

                        #endregion

                        #region Fetch Data into Object
                        if (_MstFinMain.Fetch(cn, new MSTFinMain.Criteria(FinRepKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        //Record Not Found
                        if (GFunc.NEInt(this._MstFinMain.RepKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }
                        #endregion

                        #region Fetch Data in dtTables
                        _MstFinDetails.Clear();
                        _MstFinDetails.Fetch(cn, new MSTFinDetails.Criteria(FinRepKey, 1));

                        _MstFinRows.Clear();
                        _MstFinRows.Fetch(cn, new MSTFinRows.Criteria(FinRepKey, 1));

                        _MstFinColumns.Clear();
                        _MstFinColumns.Fetch(cn, new MSTFinColumns.Criteria(FinRepKey, 1));

                        _MstFinDesigners.Clear();
                        _MstFinDesigners.Fetch(cn, new MSTFinDesigners.Criteria(FinRepKey, 1));

                        #endregion

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)
                            throw new Exception("Transaction has aborted.");
                        scope.Complete();
                    }
                }

                #region Set ObjDoc Flag
                _isDirty = false;
                _MstFinMain.GUID = _guID;
                _MstFinMain.IsNew = false;
                _MstFinMain.IsDirty = false;
                _MstFinMain.IsReadOnly = false;
                #endregion

                #region Set detail default values
                RepDetail_SetDefaultValue();
                Row_SetDefaultValue();
                Column_SetDefaultValue();
                Designer_SetDefaultValue();
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
                    this._MstFinMain = copyMstFinMain;
                    this._MstFinDetails = copyMstFinDetails;
                    this._MstFinRows = copyMstFinRows;
                    this._MstFinColumns = copyMstFinColumns;
                    this._MstFinDesigners = copyMstFinDesigners;
                }
                #endregion

                #region Set Null to Backup Objects
                copyMstFinMain = null;
                copyMstFinDetails = null;
                copyMstFinColumns = null;
                copyMstFinRows = null;
                copyMstFinDesigners = null;
                #endregion
            }
        }//Completed
        public bool GetReadOnly(int FinRepKey)
        {
            #region Declaration
            MSTFinMain copyMstFinMain = null;
            MSTFinDetails copyMstFinDetails = null;
            MSTFinRows copyMstFinRows = null;
            MSTFinColumns copyMstColumns = null;
            MSTFinDesigners copyMstDesigners = null;
            bool restoreFlag = false;
            #endregion

            try
            {
                #region Copy original object
                if (!GFunc.IsNE(this._MstFinMain))
                    copyMstFinMain = _MstFinMain.Clone();
                else
                    _MstFinMain = new MSTFinMain();

                if (!GFunc.IsNE(this._MstFinDetails))
                    copyMstFinDetails = GFunc.TACopyDataTable(_MstFinDetails);
                else
                    _MstFinDetails = new MSTFinDetails();

                if (!GFunc.IsNE(this._MstFinRows))
                    copyMstFinRows = GFunc.TACopyDataTable(_MstFinRows);
                else
                    _MstFinRows = new MSTFinRows();

                if (!GFunc.IsNE(this._MstFinColumns))
                    copyMstColumns = GFunc.TACopyDataTable(_MstFinColumns);
                else
                    _MstFinColumns = new MSTFinColumns();

                if (!GFunc.IsNE(this._MstFinDesigners))
                    copyMstDesigners = GFunc.TACopyDataTable(_MstFinDesigners);
                else
                    _MstFinDesigners = new MSTFinDesigners();
                #endregion

                #region Check Permission
                if (SECPermUtility.Perform(_permID, true) == false)
                    return false;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        #region Remove all locks by GUID except inprogress Locking
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, _guID, _codeKey))
                            return false;
                        #endregion

                        #region Fetch Data into Object
                        if (_MstFinMain.Fetch(cn, new MSTFinMain.Criteria(FinRepKey, 1)))
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }
                        #endregion

                        #region Fetch data into dtTables
                        _MstFinDetails.Clear();
                        _MstFinDetails.Fetch(cn, new MSTFinDetails.Criteria(FinRepKey, 1));

                        _MstFinRows.Clear();
                        _MstFinRows.Fetch(cn, new MSTFinRows.Criteria(FinRepKey, 1));

                        _MstFinColumns.Clear();
                        _MstFinColumns.Fetch(cn, new MSTFinColumns.Criteria(FinRepKey, 1));

                        _MstFinDesigners.Clear();
                        _MstFinDesigners.Fetch(cn, new MSTFinDesigners.Criteria(FinRepKey, 1));
                        #endregion

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)
                            throw new Exception("Transaction has aborted.");
                        scope.Complete();

                        #region Set ObjDoc Flag
                        _isDirty = false;
                        _MstFinMain.GUID = _guID;
                        _MstFinMain.IsReadOnly = true;
                        _MstFinMain.IsNew = false;
                        _MstFinMain.IsDirty = false;
                        #endregion

                    }
                }
                restoreFlag = false;
                return true;
            }
            catch (TAException tex)
            {
                throw (tex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._MstFinMain = copyMstFinMain;
                    this._MstFinDetails = copyMstFinDetails;
                    this._MstFinRows = copyMstFinRows;
                    this._MstFinColumns = copyMstColumns;
                    this._MstFinDesigners = copyMstDesigners;
                }
                #endregion

                #region Set Null to Backup Objects
                copyMstFinMain = null;
                copyMstFinDetails = null;
                copyMstFinRows = null;
                copyMstColumns = null;
                copyMstDesigners = null;
                #endregion
            }
        }//Completed
        public bool New()
        {
            #region Declaration
            MSTFinMain copyMstFinMain = null;
            MSTFinDetails copyMstFinDetails = null;
            MSTFinRows copyMstFinRows = null;
            MSTFinColumns copyMstColumns = null;
            MSTFinDesigners copyMstDesigners = null;

            bool restoreFlag = false;
            int newRepKey = 0;
            #endregion

            try
            {
                #region Copy original object or Attach Eventhandler to new object
                if (!GFunc.IsNE(_MstFinMain))
                    copyMstFinMain = _MstFinMain.Clone();

                if (!GFunc.IsNE(this._MstFinDetails))
                    copyMstFinDetails = GFunc.TACopyDataTable(_MstFinDetails);
                else
                    _MstFinDetails = new MSTFinDetails();

                if (!GFunc.IsNE(this._MstFinRows))
                    copyMstFinRows = GFunc.TACopyDataTable(_MstFinRows);
                else
                    _MstFinRows = new MSTFinRows();

                if (!GFunc.IsNE(this._MstFinColumns))
                    copyMstColumns = GFunc.TACopyDataTable(_MstFinColumns);
                else
                    _MstFinColumns = new MSTFinColumns();

                if (!GFunc.IsNE(this._MstFinDesigners))
                    copyMstDesigners = GFunc.TACopyDataTable(_MstFinDesigners);
                else
                    _MstFinDesigners = new MSTFinDesigners();
                #endregion

                #region Check Security Permission
                if (SECPermUtility.Any(_permID, out this._isReadOnly, true) == false)
                    return false;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        #region Remove all locks by GUID except inprogress Locking
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, _guID, _codeKey))
                            return false;
                        #endregion

                        #region prepare New instances
                        _MstFinMain = new MSTFinMain();
                        _MstFinDetails = new MSTFinDetails();
                        _MstFinRows = new MSTFinRows();
                        _MstFinColumns = new MSTFinColumns();
                        _MstFinDesigners = new MSTFinDesigners();

                        //Fetch Default Template (Copy the default Template for new financial report, repKey = 0 is the default template)
                        this._MstFinMain.Fetch(cn, new MSTFinMain.Criteria(0, 1));
                        this._MstFinDetails.Fetch(cn, new MSTFinDetails.Criteria(0, 1));
                        this._MstFinRows.Fetch(cn, new MSTFinRows.Criteria(0, 1));
                        this._MstFinColumns.Fetch(cn, new MSTFinColumns.Criteria(0, 1));
                        this._MstFinDesigners.Fetch(cn, new MSTFinDesigners.Criteria(0, 1));

                        #endregion

                        #region Set ObjDoc flags
                        _isDirty = false;
                        _MstFinMain.GUID = _guID;
                        _MstFinMain.RepName = "";
                        _MstFinMain.Remarks = "";
                        _MstFinMain.IsReadOnly = _isReadOnly;
                        _MstFinMain.IsDirty = false;
                        _MstFinMain.IsNew = true;
                        #endregion

                        #region Assign DocKey
                        newRepKey = SysOptionUtility.NewDocKey_Get(cn, _codeKey);
                        if (newRepKey == 0)
                            return false;
                        else
                            _MstFinMain.RepKey = newRepKey;

                        foreach (DataRow dr in _MstFinDetails.Rows)
                        {
                            dr["RepKey"] = _MstFinMain.RepKey;
                        }
                        _MstFinDetails.AcceptChanges();

                        foreach (DataRow dr in _MstFinRows.Rows)
                        {
                            dr["RepKey"] = _MstFinMain.RepKey;
                        }
                        _MstFinRows.AcceptChanges();

                        foreach (DataRow dr in _MstFinColumns.Rows)
                        {
                            dr["RepKey"] = _MstFinMain.RepKey;
                        }
                        _MstFinColumns.AcceptChanges();

                        foreach (DataRow dr in _MstFinDesigners.Rows)
                        {
                            dr["RepKey"] = _MstFinMain.RepKey;
                        }
                        _MstFinDesigners.AcceptChanges();

                        #endregion

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)
                            throw new Exception("Transaction has aborted.");
                        scope.Complete();
                    }
                }
                #region set default values to DocObj and Details
                RepDetail_SetDefaultValue();
                Row_SetDefaultValue();
                Column_SetDefaultValue();
                Designer_SetDefaultValue();
                #endregion

                #region Attached events to handle objects and dtTables
                this._MstFinMain.PropertyChanged += new PropertyChangedEventHandler(_SYSFinRep_PropertyChanged);
                this._MstFinDetails.ColumnChanged += new DataColumnChangeEventHandler(Details_CollectionChanged);
                this._MstFinRows.ColumnChanged += new DataColumnChangeEventHandler(Details_CollectionChanged);
                this._MstFinColumns.ColumnChanged += new DataColumnChangeEventHandler(Details_CollectionChanged);
                this._MstFinDesigners.ColumnChanged += new DataColumnChangeEventHandler(Details_CollectionChanged);
                #endregion

                restoreFlag = false;
                return true;
            }
            catch (TAException ex)
            {
                throw Error(ex); ;
            }
            catch (Exception ex)
            {
                throw Error(ex); ;
            }
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._MstFinMain = copyMstFinMain;
                    this._MstFinDetails = copyMstFinDetails;
                    this._MstFinRows = copyMstFinRows;
                    this._MstFinColumns = copyMstColumns;
                    this._MstFinDesigners = copyMstDesigners;
                }
                #endregion

                #region Reset Backup Objects
                copyMstFinMain = null;
                copyMstFinRows = null;
                copyMstColumns = null;
                copyMstFinDetails = null;
                copyMstDesigners = null;
                #endregion
            }
        }//Completed
        public bool Save()
        {
            #region Declaration
            MSTFinMain copyMstFinMain = null;
            MSTFinDetails copyMstFinDetails = null;
            MSTFinRows copyMstFinRows = null;
            MSTFinColumns copyMstColumns = null;
            MSTFinDesigners copyMstDesigners = null;
            bool restoreFlag = false;
            #endregion

            try
            {
                #region Copy original object or Attach Eventhandler to new object
                if (!GFunc.IsNE(_MstFinMain))
                    copyMstFinMain = _MstFinMain.Clone();

                if (!GFunc.IsNE(this._MstFinDetails))
                    copyMstFinDetails = GFunc.TACopyDataTable(_MstFinDetails);

                if (!GFunc.IsNE(this._MstFinRows))
                    copyMstFinRows = GFunc.TACopyDataTable(_MstFinRows);

                if (!GFunc.IsNE(this._MstFinColumns))
                    copyMstColumns = GFunc.TACopyDataTable(_MstFinColumns);

                if (!GFunc.IsNE(this._MstFinDesigners))
                    copyMstDesigners = GFunc.TACopyDataTable(_MstFinDesigners);
                #endregion

                #region If readonly cannot save
                if (_MstFinMain.IsReadOnly)
                {
                    MsgBox.Show(MsgID.Common.RecordIsReadOnly);
                    return false;
                }
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    // Create new sql connection for this method. 
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        #region Validation
                        if (!MstFinMain_Validation(cn))
                            return false;

                        if (!MstFinChild_Validation(cn))
                            return false;
                        #endregion

                        #region SaveProcess
                        if (!_MstFinDetails.Delete(cn, new MSTFinDetails.Criteria(MSTFinMain.RepKey, 0)))
                            return false;

                        if (!_MstFinRows.Delete(cn, new MSTFinRows.Criteria(MSTFinMain.RepKey, 0)))
                            return false;

                        if (!MstFinColumns.Delete(cn, new MSTFinColumns.Criteria(MSTFinMain.RepKey, 0)))
                            return false;

                        if (!MstFinDesigners.Delete(cn, new MSTFinDesigners.Criteria(MSTFinMain.RepKey, 0)))
                            return false;

                        if (!_MstFinMain.Delete(cn, new MSTFinMain.Criteria(_MstFinMain.RepKey)))
                            return false;

                        if (!_MstFinMain.Insert(cn, _MstFinMain.RepKey))
                            return false;

                        if (!_MstFinDetails.Insert(cn, new MSTFinDetails.Criteria(_MstFinMain.RepKey, 0)))
                            return false;

                        if (!_MstFinRows.Insert(cn, new MSTFinRows.Criteria(_MstFinMain.RepKey, 0)))
                            return false;

                        if (!_MstFinColumns.Insert(cn, new MSTFinColumns.Criteria(MSTFinMain.RepKey, 0)))
                            return false;

                        if (!_MstFinDesigners.Insert(cn, new MSTFinDesigners.Criteria(MSTFinMain.RepKey, 0)))
                            return false;

                        #endregion

                        #region Add to Auditlog
                        if (_MstFinMain.IsNew)
                            SysAuditLogUtility.AddAuditLog(cn, GEnum.AuditLogMode.Add, _codeKey, _MstFinMain.RepKey, _MstFinMain.RepName, new object[] { _MstFinMain, _MstFinDetails, _MstFinRows, _MstFinColumns, _MstFinDesigners });
                        else
                            SysAuditLogUtility.AddAuditLog(cn, GEnum.AuditLogMode.Edit, _codeKey, _MstFinMain.RepKey, _MstFinMain.RepName, new object[] { _MstFinMain, _MstFinDetails, _MstFinRows, _MstFinColumns, _MstFinDesigners });
                        #endregion
                    }
                    if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)
                        throw new Exception("Transaction has aborted.");
                    scope.Complete();
                }

                #region Set ObjDoc flags
                _isDirty = false;
                _MstFinMain.IsDirty = false;
                _MstFinMain.IsNew = false;
                #endregion

                restoreFlag = false;
                return true;
            }

            catch (TAException ex)
            {
                throw Error(ex);
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
                    this._MstFinMain = copyMstFinMain;
                    this._MstFinDetails = copyMstFinDetails;
                    this._MstFinRows = copyMstFinRows;
                    this._MstFinColumns = copyMstColumns;
                    this._MstFinDesigners = copyMstDesigners;
                }
                #endregion

                #region Reset Backup Objects
                copyMstFinMain = null;
                copyMstFinRows = null;
                copyMstColumns = null;
                copyMstFinDetails = null;
                copyMstDesigners = null;
                #endregion
            }
        }//Completed
        public bool Delete()
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        if (!Delete(cn))
                            return false;

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)
                            throw new Exception("Transaction has aborted.");
                        scope.Complete();
                    }
                }
                return true;
            }
            catch (TAException ex)
            {
                throw SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _MstFinMain, _MstFinDetails, _MstFinRows, _MstFinColumns, _MstFinDesigners }, _codeKey);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public bool Delete(SqlConnection cn)
        {
            #region Declaration
            MSTFinMain copyMstFinMain = null;
            MSTFinDetails copyMstFinDetails = null;
            MSTFinRows copyMstFinRows = null;
            MSTFinColumns copyMstColumns = null;
            MSTFinDesigners copyMstDesigners = null;
            bool restoreFlag = false;
            #endregion

            try
            {
                #region Copy original object or Attach Eventhandler to new object
                if (!GFunc.IsNE(_MstFinMain))
                    copyMstFinMain = _MstFinMain.Clone();

                if (!GFunc.IsNE(this._MstFinDetails))
                    //copyMstFinDetails = GFunc.TACopyDataTable(_MstFinDetails); commented by Jane on 20-Sep-2013. that function create another sqlconnection within transactionscope.
                    copyMstFinDetails = _MstFinDetails.Clone() ;

                if (!GFunc.IsNE(this._MstFinRows))
                    //copyMstFinRows = GFunc.TACopyDataTable(_MstFinRows);
                    copyMstFinRows = _MstFinRows.Clone();

                if (!GFunc.IsNE(this._MstFinColumns))
                    //copyMstColumns = GFunc.TACopyDataTable(_MstFinColumns);
                    copyMstColumns = _MstFinColumns.Clone();

                if (!GFunc.IsNE(this._MstFinDesigners))
                    //copyMstDesigners = GFunc.TACopyDataTable(_MstFinDesigners);
                    copyMstDesigners = _MstFinDesigners.Clone();

                #endregion

                #region Checking if can delete
                if (GFunc.IsNE(this._MstFinMain))
                    return false;

                if (_MstFinMain.IsReadOnly)
                {
                    MsgBox.Show(cn,MsgID.Common.RecordIsReadOnly);
                    return false;
                }
                #endregion

                #region Record Locking
                if (!SysLockUtility.CheckAddLock(cn, true, 0, _codeKey, _MstFinMain.RepKey, _guID))
                    return false;
                #endregion

                //Turn on restore flag to restore objects if any error occurs
                restoreFlag = true;

                #region Delete process

                //Delete Detail
                if (!_MstFinDetails.Delete(cn, new MSTFinDetails.Criteria(MSTFinMain.RepKey, 0)))
                    return false;

                if (!_MstFinRows.Delete(cn, new MSTFinRows.Criteria(MSTFinMain.RepKey, 0)))
                    return false;

                if (!_MstFinColumns.Delete(cn, new MSTFinColumns.Criteria(MSTFinMain.RepKey, 0)))
                    return false;

                if (!_MstFinDesigners.Delete(cn, new MSTFinDesigners.Criteria(MSTFinMain.RepKey, 0)))
                    return false;

                if (!_MstFinMain.Delete(cn, new MSTFinMain.Criteria(_MstFinMain.RepKey)))
                    return false;

                #endregion

                #region Remove Lock
                if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, _guID, _codeKey))
                    return false;
                #endregion

                SysAuditLogUtility.AddAuditLog(cn, GEnum.AuditLogMode.Delete, _codeKey, _MstFinMain.RepKey, _MstFinMain.RepName, new object[] { _MstFinMain, _MstFinDetails, _MstFinRows, _MstFinColumns, _MstFinDesigners /*, _DocDetItmVendors, _DocDetVendors */});
                restoreFlag = false;
                return true;
            }
            catch (TAException ex)
            {
                throw SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _MstFinMain, _MstFinDetails, _MstFinRows, _MstFinColumns, _MstFinDesigners /*, _DocDetItmVendors, _DocDetVendors*/ }, _codeKey);
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
                    this._MstFinMain = copyMstFinMain;
                    this._MstFinDetails = copyMstFinDetails;
                    this._MstFinRows = copyMstFinRows;
                    this._MstFinColumns = copyMstColumns;
                    this._MstFinDesigners = copyMstDesigners;
                }
                #endregion

                #region Reset Backup Objects
                copyMstFinMain = null;
                copyMstFinRows = null;
                copyMstColumns = null;
                copyMstFinDetails = null;
                copyMstDesigners = null;
                #endregion
            }
        }//Completed

        public bool NewReport(int CopyFromRepKey, string NewReportName, string NewReportRemark)
        {
            try
            {

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();
                        this._MstFinMain.RepKey = CopyFromRepKey;
                        this._MstFinMain.RepName = NewReportName;
                        this._MstFinMain.Remarks = NewReportRemark;
                        this._MstFinMain.RepType = (int)GEnum.FinRepType.Active_Report;
                        this._MstFinMain.RepKey= this._MstFinMain.New(cn);

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)
                            throw new Exception("Transaction has aborted.");
                        scope.Complete();

                    }
                }
                return true;
            }
            catch (TAException ex)
            {

                throw Error(ex);
            }
            catch (Exception ex)
            {

                throw Error(ex);
            }
            finally
            {

            }
        }//Completed
        

        //Set Default Values
        private bool RepDetail_SetDefaultValue()
        {
            _MstFinDetails.Columns["DetType"].DefaultValue = 0;
            _MstFinDetails.Columns["DetSeq"].DefaultValue = 0;
            _MstFinDetails.Columns["DetHeight"].DefaultValue = 0M;
            _MstFinDetails.Columns["FirstColumn"].DefaultValue = false;
            _MstFinDetails.Columns["ColFormat"].DefaultValue = "";
            _MstFinDetails.Columns["BodyTextValue"].DefaultValue = "";
            _MstFinDetails.Columns["BodyTextFormat"].DefaultValue = "";
            _MstFinDetails.Columns["RowNo"].DefaultValue = 0;
            _MstFinDetails.Columns["RowSummaryText"].DefaultValue = "";
            _MstFinDetails.Columns["RowRevValueForBal"].DefaultValue = false;
            _MstFinDetails.Columns["RowRevValueForFormula"].DefaultValue = false;
            _MstFinDetails.Columns["RowHide"].DefaultValue = "";
            _MstFinDetails.Columns["PageBreak"].DefaultValue = false;

            return true;
        }
        private bool Row_SetDefaultValue()
        {
            _MstFinRows.Columns["RowAccTypeKey"].DefaultValue = 0;
            _MstFinRows.Columns["RowAccGrpKey"].DefaultValue = 0;
            _MstFinRows.Columns["RowAccF"].DefaultValue = "";
            _MstFinRows.Columns["RowAccT"].DefaultValue = "";
            _MstFinRows.Columns["RowDeptF"].DefaultValue = "";
            _MstFinRows.Columns["RowDeptT"].DefaultValue = "";
            _MstFinRows.Columns["RowBranchF"].DefaultValue = "";
            _MstFinRows.Columns["RowBranchT"].DefaultValue = "";
            _MstFinRows.Columns["RowRangeFilter"].DefaultValue = "";
            _MstFinRows.Columns["RowDisplayType"].DefaultValue = 0;
            _MstFinRows.Columns["LineSummaryText"].DefaultValue = "";

            return true;
        }
        private bool Column_SetDefaultValue()
        {
            _MstFinColumns.Columns["ColType"].DefaultValue = 0;
            _MstFinColumns.Columns["ColText"].DefaultValue = "";
            _MstFinColumns.Columns["ColDisplay"].DefaultValue = true;
            _MstFinColumns.Columns["ColWidth"].DefaultValue = 0M;
            _MstFinColumns.Columns["ColDetailFormat"].DefaultValue = "";
            _MstFinColumns.Columns["ColBalanceExp"].DefaultValue = "";
            _MstFinColumns.Columns["ColFormulaExp"].DefaultValue = "";
            _MstFinColumns.Columns["ColIgnoreRowReverse"].DefaultValue = false;
            _MstFinColumns.Columns["TotalExp"].DefaultValue = "";

            return true;
        }
        private bool Designer_SetDefaultValue()
        {   //Whole Column Default Value
            _MstFinDesigners.Columns["DesignerText"].DefaultValue = "";

            return true;
        }

        //Validations
        public bool MstFinMain_Validation(SqlConnection cn)
        {
            string processOK = GVar.gcCancel;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
            string errorMsgID = string.Empty;

            try
            {

                #region Clear Error in UI
                if (!GFunc.IsNE(this.clearErrorNotifier))
                    this.clearErrorNotifier.Invoke(this, e);
                #endregion

                #region Validation
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MstFinMain.RepKey, "RepKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                if (processOK == GVar.gcCancel)
                {
                    throw new TAException(errorMsgID);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MstFinMain.RepName, "RepName", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                if (processOK == GVar.gcCancel)
                {
                    throw new TAException(errorMsgID);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MstFinMain.RepType, "RepType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                if (processOK == GVar.gcCancel)
                {
                    throw new TAException(errorMsgID);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MstFinMain.Remarks, "Remarks", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                if (processOK == GVar.gcCancel)
                {
                    throw new TAException(errorMsgID);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MstFinMain.PaperSize, "PaperSize", GEnum.DataType.String, GEnum.Require.No, 0, null, null, null, null, e, cn);
                if (processOK == GVar.gcCancel)
                {
                    throw new TAException(errorMsgID);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MstFinMain.MarginTop, "MarginTop", GEnum.DataType.Decimel, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                if (processOK == GVar.gcCancel)
                {
                    throw new TAException(errorMsgID);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MstFinMain.MarginBottom, "MarginBottom", GEnum.DataType.Decimel, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                if (processOK == GVar.gcCancel)
                {
                    throw new TAException(errorMsgID);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MstFinMain.MarginLeft, "MarginLeft", GEnum.DataType.Decimel, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                if (processOK == GVar.gcCancel)
                {
                    throw new TAException(errorMsgID);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MstFinMain.MarginRight, "MarginRight", GEnum.DataType.Decimel, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                if (processOK == GVar.gcCancel)
                {
                    throw new TAException(errorMsgID);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MstFinMain.Hidden, "Hidden", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                if (processOK == GVar.gcCancel)
                {
                    throw new TAException(errorMsgID);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MstFinMain.BuildIn, "BuildIn", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                if (processOK == GVar.gcCancel)
                {
                    throw new TAException(errorMsgID);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MstFinMain.CreateDate, "CreateDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                if (processOK == GVar.gcCancel)
                {
                    throw new TAException(errorMsgID);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MstFinMain.CreateUserKey, "CreateUserKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                if (processOK == GVar.gcCancel)
                {
                    throw new TAException(errorMsgID);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MstFinMain.LastModifiedDate, "LastModifiedDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                if (processOK == GVar.gcCancel)
                {
                    throw new TAException(errorMsgID);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MstFinMain.LastModifiedUserKey, "LastModifiedUserKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                if (processOK == GVar.gcCancel)
                {
                    throw new TAException(errorMsgID);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MstFinMain.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                if (processOK == GVar.gcCancel)
                {
                    throw new TAException(errorMsgID);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MstFinMain.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                if (processOK == GVar.gcCancel)
                {
                    throw new TAException(errorMsgID);
                }
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MstFinMain.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                if (processOK == GVar.gcCancel)
                {
                    throw new TAException(errorMsgID);
                }
                #endregion

                #region Invoke Notifier
                if (e.PropertyMessage.Count > 0)
                {
                    if (!GFunc.IsNE(this.SYSFinRepNotifier))
                        this.SYSFinRepNotifier.Invoke(this, e);

                    return false;
                }
                else
                    return true;
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
        }
        public bool MstFinChild_Validation(SqlConnection cn)
        {
            try
            {
                if (MstFinDetails_Validation(cn) == false)
                { return false; }

                if (MstFinRows_Validation(cn) == false)
                { return false; }

                if (MstFinColumn_Validation(cn) == false)
                { return false; }

                if (MstFinDesigner_Validation(cn) == false)
                { return false; }

                return true;
            }
            catch (TAException ex)
            {
                throw Error(ex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public bool MstFinDetails_Validation(SqlConnection cn)
        {
            #region Declaration
            bool isValidate = true;
            string msgValue = string.Empty;
            string errorMsgID = string.Empty;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
            #endregion

            try
            {
                foreach (DataRow dr in this._MstFinDetails.Rows)
                {
                    #region Common Validation
                    BaseUtility.Validate(true, dr["RepKey"], "RepKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RepDetKey"], "RepDetKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["DetType"], "DetType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["DetSeq"], "DetSeq", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["DetHeight"], "DetHeight", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["FirstColumn"], "FirstColumn", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["ColFormat"], "ColFormat", GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["BodyTextValue"], "BodyTextValue", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["BodyTextFormat"], "BodyTextFormat", GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RowNo"], "RowNo", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RowSummaryText"], "RowSummaryText", GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RowRevValueForBal"], "RowRevValueForBal", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RowRevValueForFormula"], "RowRevValueForFormula", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RowHide"], "RowHide", GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["TotalExp"], "TotalExp", GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["TotalFormat"], "TotalFormat", GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["TotalHide"], "TotalHide", GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["PageBreak"], "PageBreak", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["Custom1"], "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["Custom2"], "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["Custom3"], "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    #endregion
                }

                return isValidate;
            }
            catch (TAException ex)
            {
                throw Error(ex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public bool MstFinRows_Validation(SqlConnection cn)
        {
            #region Declaration
            bool isValidate = true;
            string msgValue = string.Empty;
            string errorMsgID = string.Empty;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
            #endregion

            try
            {
                foreach (DataRow dr in this._MstFinRows.Rows)
                {
                    #region Common Validation
                    BaseUtility.Validate(true, dr["RepKey"], "RepKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RepDetKey"], "RepDetKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RowNo"], "RowNo", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RowSeq"], "RowSeq", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RowAccTypeKey"], "RowAccTypeKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RowAccGrpKey"], "RowAccGrpKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RowAccF"], "RowAccF", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RowAccT"], "RowAccT", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RowDeptF"], "RowDeptF", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RowDeptT"], "RowDeptT", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RowBranchF"], "RowBranchF", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RowBranchT"], "RowBranchT", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RowRangeFilter"], "RowRangeFilter", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RowDisplayType"], "RowDisplayType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["LineSummaryText"], "LineSummaryText", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["Custom1"], "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["Custom2"], "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["Custom3"], "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    #endregion
                }

                return isValidate;
            }
            catch (TAException ex)
            {
                throw Error(ex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public bool MstFinColumn_Validation(SqlConnection cn)
        {
            #region Declaration
            bool isValidate = true;
            string msgValue = string.Empty;
            string errorMsgID = string.Empty;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
            int KeyCount = 0;
            #endregion

            try
            {
                foreach (DataRow dr in this._MstFinColumns.Rows)
                {
                    #region Common Validation
                    BaseUtility.Validate(true, dr["RepKey"], "RepKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RepDetKey"], "RepDetKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["ColNo"], "ColNo", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["ColType"], "ColType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["ColText"], "ColText", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["ColDisplay"], "ColDisplay", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["ColWidth"], "ColWidth", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["ColDetailFormat"], "ColDetailFormat", GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["ColBalanceExp"], "ColBalanceExp", GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["ColFormulaExp"], "ColFormulaExp", GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["ColIgnoreRowReverse"], "ColIgnoreRowReverse", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["TotalExp"], "TotalExp", GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["Custom1"], "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["Custom2"], "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["Custom3"], "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    #endregion
                }
                return isValidate;
            }
            catch (TAException ex)
            {
                throw Error(ex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public bool MstFinDesigner_Validation(SqlConnection cn)
        {
            #region Declaration
            bool isValidate = true;
            string msgValue = string.Empty;
            string errorMsgID = string.Empty;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
            #endregion

            try
            {
                foreach (DataRow dr in this._MstFinDesigners.Rows)
                {
                    #region Common Validation
                    BaseUtility.Validate(true, dr["RepKey"], "RepKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["RepDetKey"], "RepDetKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["ColNo"], "ColNo", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["DesignerText"], "DesignerText", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["CreateDate"], "CreateDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["CreateUserKey"], "CreateUserKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["LastModifiedDate"], "LastModifiedDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["LastModifiedUserKey"], "LastModifiedUserKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(true, dr["Custom1"], "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["Custom2"], "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(true, dr["Custom3"], "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    #endregion

                }

                return isValidate;
            }
            catch (TAException ex)
            {
                throw Error(ex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        //Dirty Handle Events
        void _SYSFinRep_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _MstFinMain.IsDirty = true;
        }
        void Details_CollectionChanged(object sender, DataColumnChangeEventArgs e)
        {
            _MstFinMain.IsDirty = true;
        }

        //Dispose
        public bool Dispose()
        {
            try
            {
                return SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, _codeKey, _guID, 0, 0);
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

        //Error Exception
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _MstFinMain, _MstFinDetails, _MstFinRows, _MstFinColumns, _MstFinDesigners }, _codeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _MstFinMain, _MstFinDetails, _MstFinRows, _MstFinColumns, _MstFinDesigners }, _codeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
    }
}






