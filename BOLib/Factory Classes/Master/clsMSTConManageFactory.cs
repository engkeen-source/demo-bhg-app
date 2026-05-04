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
    public class MSTConManageFactory : CommandBase
    {
        #region Member variables and constants      
        private MSTConManageDTable _ConManageDT = null;
        private MSTConManageWatchDTable _ConWatchDT = null;
        private MSTConRemarkDTable _ConRemarkDT = null;        
        private DataTable _ConRemark = null;
        private DataTable _ConManage = null;
        private string checkRemark = string.Empty;

        private bool _isSaveRemark = false;
        private bool _isSaveWatch = false;
        private bool _isSave = false;


        private bool _isDirty = false;
        private bool _isDirtyWatch = false;
        private bool _isDirtyRemark = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isOpenReadOnly = false;
        private int _guID = 0;
        private bool _isLock = false;

        // System Code Key for this Factory.
        private GEnum.SystemCode _codeKey = GEnum.SystemCode.CustomerManage;

        // Permission ID for this Factory.
        public const string constPermID = GVar.PermissionID.Customer_Manage;

        public GVar.ErrorEvent errorEvent = null;
        public GVar.ListErrorEvent listErrorEvent = null;

        #endregion // Member variables and constant

        #region Factory Properties

        public GEnum.SystemCode CodeKey
        {
            get
            {
                return this._codeKey;
            }
        }
        public DataTable ObjConManage
        {
            get
            {
                return this._ConManage;
            }
        }

        public MSTConManageDTable ObjConmanageDT
        {
            get
            {
                return this._ConManageDT;
            }
        }

        public MSTConManageWatchDTable ObjConWatchDT
        {
            get
            {
                return this._ConWatchDT;
            }
        }

        public DataTable ObjConRemark
        {
            get
            {
                return this._ConRemark;
            }
        }

        public MSTConRemarkDTable ObjConRemarkDT
        {
            get
            {
                return this._ConRemarkDT;
            }
        }

        public bool IsDirty
        {
            get
            {
                return this._isDirty;
            }
        }
        public bool IsLock
        {
            get
            {
                return this._isLock;
            }
        }

        public bool IsDirtyWatch
        {
            get
            {
                return this._isDirtyWatch;
            }
        }

        public bool IsDirtyRemark
        {
            get
            {
                return this._isDirtyRemark;
            }
        }

        public bool isSave
        {
            get
            {
                return this._isSave;
            }
        }
        public bool isSaveRemark
        {
            get
            {
                return this._isSaveRemark;
            }
        }

        public bool isSaveWatch
        {
            get
            {
                return this._isSaveWatch;
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

        #endregion // Factory Properties

        #region Constructors

        /// <summary>
        /// Default constructor for this Factory.
        /// </summary>
        public MSTConManageFactory()
        {
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

        }

        #endregion // Constructors

        #region Initialization Method

        public bool Initialisation()
        {

            try
            {
                // Check Permission
                if (!SECPermUtility.Any(constPermID, out this._isOpenReadOnly, true))
                { return false; }

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        // Check Lock
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, _codeKey, 0, 0, _guID))
                        {
                            _isLock = true;
                            return false;
                        }

                        // Get Instance GUID
                        if ((this._guID = SysOptionUtility.GetNewLockingGUID(cn)) == 0)
                        {
                            this._guID = -1;
                            return false;
                        }

                        // Locking
                        if (SysLockUtility.IsProcessLock(cn, false, GEnum.SysLockOption.ByCodKey, _codeKey, this._guID))
                        {
                            this._guID = -1;
                            return false;
                        }

                        // Add Inprogress Lock
                        if (!SysLockUtility.AddInprogressLock(cn, true, this._guID, _codeKey))
                        {
                            this._guID = -1;
                            return false;
                        }

                        this._isNew = false;
                        this._isOpenReadOnly = false;

                        // No errors - commit transaction
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                    }
                }
            }
            catch (TAException Tex)
            {
                throw Tex;
            }
            catch (Exception ex)
            {
                // Add Error to System Error Log                    
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int)_codeKey, ex, true, false, new object[] { _ConManage });
                throw (ex);
            }
            return true; ;
        }

        #endregion //Initialisation Method     

       



        //Error Exceptions
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _ConManage, _ConManageDT });
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _ConManage, _ConManageDT });
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        #region Save Method

        public bool SaveFollowUpDate(MSTConManage objMSTConManage)
        {
            bool isSave = false;
            string msgID = string.Empty;

            if (this.IsNew)
                msgID = MsgID.Common.AddFail;
            else
                msgID = MsgID.Common.UpdateFail;

            bool isCommitTransFail = true;
            string recordID = string.Empty;

            try
            {
                if (this.IsOpenReadOnly)
                {
                    MsgBox.Show(MsgID.Common.RecordIsReadOnly);
                    return false;
                }
                else
                {
                    if (!SECPermUtility.Perform(constPermID, true))
                    { return false; }
                }


                if (!this.Validation())
                    return false;

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        if (!_ConWatchDT.InsertFollowUpDate(cn))
                        { return false; }

                        // Alert Process

                        // Commit Process
                        msgID = string.Empty;
                        isSave = true;
                        _isSaveWatch = true;

                        // No errors - commit transaction
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                        _isDirtyWatch = false;
                    }// End of SqlConnection
                }// End of TransactionScope

                // Audit Log
                //SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, _codeKey, objMSTConManage.ConKey,  new object[] { objMSTConManage });

            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                if (isCommitTransFail)
                    msgID = MsgID.Validation.CommitTransFail;

                // Add Error to System Audit Log
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int)_codeKey, ex, true, false, new object[] { _ConWatchDT });
                throw Error(ex);
            }

            return isSave;
        }

        public bool RemoveFromWatchList(MSTConManage objMSTConManage)
        {
            bool isSave = false;
            string msgID = string.Empty;

            if (this.IsNew)
                msgID = MsgID.Common.AddFail;
            else
                msgID = MsgID.Common.UpdateFail;

            bool isCommitTransFail = true;
            string recordID = string.Empty;

            try
            {
                if (this.IsOpenReadOnly)
                {
                    MsgBox.Show(MsgID.Common.RecordIsReadOnly);
                    return false;
                }
                else
                {
                    if (!SECPermUtility.Perform(constPermID, true))
                    { return false; }
                }


                if (!this.Validation())
                    return false;

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        if (!_ConWatchDT.RemoveFromWatchList(cn))
                        { return false; }

                        // Alert Process

                        // Commit Process
                        msgID = string.Empty;
                        isSave = true;
                        _isSaveWatch = true;

                        // No errors - commit transaction
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                        _isDirtyWatch = false;
                    }// End of SqlConnection
                }// End of TransactionScope

                // Audit Log
                //SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, _codeKey, objMSTConManage.ConKey,  new object[] { objMSTConManage });

            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                if (isCommitTransFail)
                    msgID = MsgID.Validation.CommitTransFail;

                // Add Error to System Audit Log
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int)_codeKey, ex, true, false, new object[] { _ConWatchDT });
                throw Error(ex);
            }

            return isSave;
        }

        public bool SaveCustomerType(List<Int32> lstWatch,List<Int32>lstFollowUpDate,List<Int32>lstCustomer,bool CheckUser)
        {
            bool isSave = false;
            string msgID = string.Empty;

            if (this.IsNew)
                msgID = MsgID.Common.AddFail;
            else
                msgID = MsgID.Common.UpdateFail;

            bool isCommitTransFail = true;
            string recordID = string.Empty;

            try
            {
                if (this.IsOpenReadOnly)
                {
                    MsgBox.Show(MsgID.Common.RecordIsReadOnly);
                    return false;
                }
                else
                {
                    if (!SECPermUtility.Perform(constPermID, true))
                    { return false; }
                }


                if (!this.Validation())
                    return false;

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        if (!_ConManageDT.InsertCustomerType(cn,lstWatch,lstFollowUpDate,lstCustomer,CheckUser))
                        { return false; }

                        // Alert Process

                        // Commit Process
                        msgID = string.Empty;
                        isSave = true;
                        _isSave = true;

                        // No errors - commit transaction
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                        _isDirty = false;
                    }// End of SqlConnection
                }// End of TransactionScope

                // Audit Log
                //SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, _codeKey, objMSTConManage.ConKey,  new object[] { objMSTConManage });

            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                if (isCommitTransFail)
                    msgID = MsgID.Validation.CommitTransFail;

                // Add Error to System Audit Log
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int)_codeKey, ex, true, false, new object[] { _ConManageDT });
                throw Error(ex);
            }

            return isSave;
        }

        

        #endregion //Save Method
        #region Save Method

        public bool SaveRemark(MstConRemark objMSTConRemark, int option)
        {
            bool isSave = false;
            string msgID = string.Empty;

            if (this.IsNew)
                msgID = MsgID.Common.AddFail;
            else
                msgID = MsgID.Common.UpdateFail;

            bool isCommitTransFail = true;
            string recordID = string.Empty;

            try
            {
                if (this.IsOpenReadOnly)
                {
                    MsgBox.Show(MsgID.Common.RecordIsReadOnly);
                    return false;
                }
                else
                {
                    if (!SECPermUtility.Perform(constPermID, true))
                    { return false; }
                }


                if (!this.Validation())
                    return false;

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        if (!objMSTConRemark.CustomAddUpdate(cn, new MstConRemark.Criteria(objMSTConRemark.ConKey, option, objMSTConRemark.Remark, objMSTConRemark.ActionClose, objMSTConRemark.ConRemarkID)))
                        { return false; }
                        else
                        {
                            if (!objMSTConRemark.UpdateCustomerRecord(cn, new MstConRemark.Criteria(objMSTConRemark.ConKey)))
                            { return false; }

                            if (!objMSTConRemark.SendEmailsRemark(cn, new MstConRemark.Criteria(objMSTConRemark.ConKey, option, objMSTConRemark.Remark)))
                            { return false; }
                        }
                        

                        // Alert Process

                        // Commit Process
                        msgID = string.Empty;
                        isSave = true;
                        _isSaveRemark = true;

                        // No errors - commit transaction
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                        _isDirtyRemark = false;

                    }// End of SqlConnection
                }// End of TransactionScope

                // Audit Log
                //SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, _codeKey, objMSTConManage.ConKey,  new object[] { objMSTConManage });

            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                if (isCommitTransFail)
                    msgID = MsgID.Validation.CommitTransFail;

                // Add Error to System Audit Log
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int)_codeKey, ex, true, false, new object[] { _ConManageDT });
                throw Error(ex);
            }

            return isSave;
        }

        #endregion //Save Method

        #region GetEdit Method

        public bool GetEdit(int Option, int DueCal, int CCB, string DateV,string ConName)
        {
            // Initialisation
            bool isGetEdit = false;
            string msgID = MsgID.Common.GetFail;

            // Copy original object
            MSTConManageDTable copyConManageDT = null;

            if (!GFunc.IsNE(this._ConManageDT))
                copyConManageDT = GFunc.TACopyDataTable(_ConManageDT);

            try
            {
                // Check Permission
                if (!SECPermUtility.Perform(constPermID, true))
                    return false;

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        //// Check Lock                        
                        //if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, _codeKey, ConKey, 0, _guID))
                        //    return false;

                        //// Remove Lock
                        //if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, _codeKey))
                        //    return false;

                        // Add Lock
                        //if (!SysLockUtility.AddLock(cn, true, this._guID, _codeKey, ConKey))
                        //    return false;

                        if (_ConManageDT != null)
                            _ConManageDT.Dispose();
                        _ConManageDT = new MSTConManageDTable();

                        if (!_ConManageDT.Fetch(cn, new MSTConManageDTable.Criteria(Option, DueCal, CCB, DateV,ConName)))
                            throw new ApplicationException(msgID);
                        this._ConManageDT.ColumnChanged += new DataColumnChangeEventHandler(_ConManageDT_ColumnChanged);
                        this._ConManageDT.RowChanged += new DataRowChangeEventHandler(_ConManageDT_RowChanged);
                        this._ConManageDT.RowDeleted += new DataRowChangeEventHandler(_ConManageDT_RowDeleted);
                        // Commit Process                           
                        this._isNew = false;
                        this._isOpenReadOnly = false;
                        msgID = string.Empty;
                        isGetEdit = true;

                        // No errors - commit transaction
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

                        // Set Null to Backup Objects
                        copyConManageDT = null;
                        _isDirty = false;
                    }
                }
            }
            catch (TAException Tex)
            {
                throw (Tex);
            }
            catch (Exception ex)
            {
                // Add Error to System Audit Log
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int)_codeKey, ex, true, false, new object[] { _ConManageDT });
                this._ConManageDT = copyConManageDT;
                throw (ex);
            }

            return isGetEdit;
        }


        public bool GetEditWatch(int Option, int DueCal, int CCB, string DateV, string ConName)
        {
            // Initialisation
            bool isGetEdit = false;
            string msgID = MsgID.Common.GetFail;

            // Copy original object
            MSTConManageWatchDTable copyConManageWatchDT = null;

            if (!GFunc.IsNE(this._ConWatchDT))
                copyConManageWatchDT = GFunc.TACopyDataTable(_ConWatchDT);

            try
            {
                // Check Permission
                if (!SECPermUtility.Perform(constPermID, true))
                    return false;

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        //// Check Lock                        
                        //if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, _codeKey, ConKey, 0, _guID))
                        //    return false;

                        //// Remove Lock
                        //if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, _codeKey))
                        //    return false;

                        // Add Lock
                        //if (!SysLockUtility.AddLock(cn, true, this._guID, _codeKey, ConKey))
                        //    return false;

                        if (_ConWatchDT != null)
                            _ConWatchDT.Dispose();
                        _ConWatchDT = new MSTConManageWatchDTable();

                        if (!_ConWatchDT.Fetch(cn, new MSTConManageWatchDTable.Criteria(Option, DueCal, CCB, DateV,ConName)))
                            throw new ApplicationException(msgID);
                        this._ConWatchDT.ColumnChanged += new DataColumnChangeEventHandler(_ConWatchDT_ColumnChanged);
                        this._ConWatchDT.RowChanged += new DataRowChangeEventHandler(_ConWatchDT_RowChanged);
                        this._ConWatchDT.RowDeleted += new DataRowChangeEventHandler(_ConWatchDT_RowDeleted);
                        // Commit Process                           
                        this._isNew = false;
                        this._isOpenReadOnly = false;
                        msgID = string.Empty;
                        isGetEdit = true;

                        // No errors - commit transaction
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

                        // Set Null to Backup Objects
                        copyConManageWatchDT = null;
                        _isDirtyWatch = false;
                    }
                }
            }
            catch (TAException Tex)
            {
                throw (Tex);
            }
            catch (Exception ex)
            {
                // Add Error to System Audit Log
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int)_codeKey, ex, true, false, new object[] { _ConWatchDT });
                this._ConWatchDT = copyConManageWatchDT;
                throw (ex);
            }

            return isGetEdit;
        }

        public bool GetEditRemark(int? ConKey)
        {
            // Initialisation
            bool isGetEdit = false;
            string msgID = MsgID.Common.GetFail;

            // Copy original object
            MSTConRemarkDTable copyConRemarkDT = null;

            if (!GFunc.IsNE(this._ConRemarkDT))
                copyConRemarkDT = GFunc.TACopyDataTable(_ConRemarkDT);

            try
            {
                // Check Permission
                if (!SECPermUtility.Perform(constPermID, true))
                    return false;

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        //// Check Lock                        
                        //if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, _codeKey, ConKey, 0, _guID))
                        //    return false;

                        //// Remove Lock
                        //if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, _codeKey))
                        //    return false;

                        // Add Lock
                        //if (!SysLockUtility.AddLock(cn, true, this._guID, _codeKey, ConKey))
                        //    return false;

                        if (_ConRemarkDT != null)
                            _ConRemarkDT.Dispose();
                        _ConRemarkDT = new MSTConRemarkDTable();

                        if (!_ConRemarkDT.Fetch(cn, new MSTConRemarkDTable.Criteria(ConKey, 1)))
                        {

                        }
                        //return false;

                        //if (!_ConRemarkDT.Fetch(cn, new MSTConRemarkDTable.Criteria(ConKey, 1)))
                        //    throw new ApplicationException(msgID);                        
                        this._ConRemarkDT.RowChanged += new DataRowChangeEventHandler(_ConRemarkDT_RowChanged);
                        this._ConRemarkDT.RowDeleted += new DataRowChangeEventHandler(_ConRemarkDT_RowDeleted);
                        this._ConRemarkDT.ColumnChanged += new DataColumnChangeEventHandler(_ConRemarkDT_ColumnChanged);
                        // Commit Process                           
                        this._isNew = false;
                        this._isOpenReadOnly = false;
                        msgID = string.Empty;
                        isGetEdit = true;
                        

                        // No errors - commit transaction
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

                        // Set Null to Backup Objects
                        copyConRemarkDT = null;
                        _isDirtyRemark = false;
                    }
                }
            }
            catch (TAException Tex)
            {
                throw (Tex);
            }
            catch (Exception ex)
            {
                // Add Error to System Audit Log
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int)_codeKey, ex, true, false, new object[] { _ConRemarkDT });
                this._ConRemarkDT = copyConRemarkDT;
                throw (ex);
            }
            return isGetEdit;
        }

        void _ConManageDT_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            _isDirty = true;
        }

        void _ConManageDT_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            _isDirty = true;
        }

        void _ConManageDT_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            _isDirty = true;
        }

        void _ConWatchDT_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            _isDirtyWatch = true;
        }

        void _ConWatchDT_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            _isDirtyWatch = true;
        }

        void _ConWatchDT_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            _isDirtyWatch = true;
        }
        void _ConRemarkDT_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {

            _isDirtyRemark = true;
        }

        void _ConRemarkDT_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            _isDirtyRemark = true;
        }

        void _ConRemarkDT_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            _isDirtyRemark = true;
        }

        #endregion GetEdit Method

        private bool Validation()
        {
            bool isValid = true;
            try
            {
                foreach (DataRow dr in _ConManageDT.Rows)
                {
                    if (GFunc.IsNEZ(dr["DocConKey"]) || GFunc.IsNE(dr["DocConNm"]))
                    {
                        dr.RowError = "Validation failed.";
                        isValid = false;
                    }
                }

                return isValid;
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

        #region Dispose Method
        public bool Dispose()
        {
            string msgID = string.Empty;
            try
            {
                if (!SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, _codeKey, GUID, 0, 0))
                { return false; }
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

        #endregion

        private bool ValidationRemark()
        {
            bool isValid = true;
            try
            {
                foreach (DataRow dr in _ConRemarkDT.Rows)
                {
                    if (GFunc.IsNE(dr["Remark"]))
                    {
                        dr.RowError = "Validation failed.";
                        isValid = false;
                    }
                }

                return isValid;
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

    }
}
