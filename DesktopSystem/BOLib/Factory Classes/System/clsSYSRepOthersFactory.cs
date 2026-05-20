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
    public class SYSRepOthersFactory : CommandBase
    {
        #region Member variables and constants

        private SYSRepOtherss _SYS_RepOthers = null;

        private DataTable _ROSYSRep = null;

        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isOpenReadOnly = false;
        private int _guID = 0;

        // System Code Key for this Factory.
        private GEnum.SystemCode _codeKey = GEnum.SystemCode.Other_Report_Setting;
        public GEnum.SystemCode ConstantCodeKey { get { return _codeKey; } }
        // Permission ID for this Factory.
        public const string constPermID = GVar.PermissionID.Others_Report_Setting;

        public GVar.ErrorEvent errorEvent = null;
        public GVar.ListErrorEvent listErrorEvent = null;

        #endregion // Member variables and constant

        #region Factory Properties

        public SYSRepOtherss ObjSYS_RepOtherss
        {
            get
            {
                return this._SYS_RepOthers;
            }
        }

        public DataTable ROSYSRep
        {
            get
            {
                return this._ROSYSRep;
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
        public SYSRepOthersFactory(out string msgID)
        {
            try
            {
                Initialisation(out msgID);
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

        public bool Initialisation(out string msgID)
        {
            msgID = MsgID.Common.InitialisationFail;

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
                        { return false; }

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
                        msgID = string.Empty;

                        // No errors - commit transaction
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                    }
                }
            }
            catch (TAException tex)
            {
                throw (tex);
            }
            catch (Exception ex)
            {
                // Add Error to System Error Log                    
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int)_codeKey, ex, true, true, new object[] { _SYS_RepOthers });
                throw (ex);
            }

            return true;
        }

        #endregion //Initialisation Method

        public bool GetAllReports()
        {


            try
            {
                // Check Permission
                if (!SECPermUtility.Perform(constPermID, true))
                { return false; }

                //Get DataTable from Read Only List                  
                _ROSYSRep = SYSList.GetReports(4, 0);


                return true;


            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                // Add Error to System Audit Log
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int?)_codeKey, ex, true, true, _ROSYSRep);
                throw Error(ex);
            }
        }

        #region GetEdit Method

        public bool GetEdit(int? RepGrpKey)
        {
            // Initialisation
            bool isGetEdit = false;
            string msgID = MsgID.Common.GetFail;

            // Copy original object
            SYSRepOtherss copySYS_RepOthers = null;

            if (!GFunc.IsNE(this._SYS_RepOthers))
                copySYS_RepOthers = GFunc.TACopyDataTable(_SYS_RepOthers);

            try
            {
                // Check Permission
                if (!SECPermUtility.Perform(constPermID, true))
                { return false; }

                _ROSYSRep = SYSList.GetReports(5, RepGrpKey.Value);

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        // Check Lock                        
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, _codeKey, RepGrpKey, 0, _guID))
                        { return false; }


                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, _codeKey))
                            return false;


                        // Add Lock
                        if (!SysLockUtility.AddLock(cn, true, this._guID, _codeKey, RepGrpKey))
                            return false;

                        _SYS_RepOthers = new SYSRepOtherss();

                        if (!_SYS_RepOthers.Fetch(cn, new SYSRepOtherss.Criteria(RepGrpKey, 2)))
                            throw new ApplicationException(msgID);

                        this._SYS_RepOthers.RowChanged += new DataRowChangeEventHandler(_SYS_RepOthers_RowChanged);
                        this._SYS_RepOthers.RowDeleted += new DataRowChangeEventHandler(_SYS_RepOthers_RowDeleted);
                        // Commit Process                           
                        this._isNew = false;
                        this._isOpenReadOnly = false;
                        msgID = string.Empty;
                        isGetEdit = true;

                        // No errors - commit transaction
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

                        // Set Null to Backup Objects
                        copySYS_RepOthers = null;
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
                // Add Error to System Audit Log
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int)_codeKey, ex, true, false, new object[] { _SYS_RepOthers });
                this._SYS_RepOthers = copySYS_RepOthers;
                throw Error(ex);
            }

            return isGetEdit;
        }

        void _SYS_RepOthers_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            _isDirty = true;
        }

        void _SYS_RepOthers_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            _isDirty = true;
        }

        #endregion GetEdit Method

        #region Copy Method

        public bool Copy(int? NewRepGrpKey, int? CopyGrpKey)
        {
            string msgID = MsgID.Common.GetFail;

            // Copy original object
            SYSRepOtherss copySYS_RepOthers = null;



            try
            {

                if (!GFunc.IsNE(this._SYS_RepOthers))
                    copySYS_RepOthers = GFunc.TACopyDataTable(_SYS_RepOthers);

                // Check Permission
                if (SECPermUtility.Perform(constPermID, true) == false)
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

                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, _codeKey, NewRepGrpKey, 0, _guID))
                            return false;

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, _codeKey))
                            return false;


                        // Add Lock
                        if (!SysLockUtility.AddLock(cn, true, this._guID, _codeKey, NewRepGrpKey))
                            return false;

                        if (_SYS_RepOthers == null)
                            _SYS_RepOthers = new SYSRepOtherss();
                        else
                            _SYS_RepOthers.Rows.Clear();

                        if (!_SYS_RepOthers.Fetch(cn, new SYSRepOtherss.Criteria(CopyGrpKey, 2)))
                            throw new ApplicationException(msgID);
                        else
                        {
                            foreach (DataRow dr in _SYS_RepOthers.Rows)
                            {
                                dr["RepGroup"] = NewRepGrpKey;
                            }

                        }

                        this._SYS_RepOthers.RowChanged += new DataRowChangeEventHandler(_SYS_RepOthers_RowChanged);
                        this._SYS_RepOthers.RowDeleted += new DataRowChangeEventHandler(_SYS_RepOthers_RowDeleted);
                        // Commit Process                           
                        this._isNew = false;
                        this._isOpenReadOnly = false;
                        msgID = string.Empty;

                        // No errors - commit transaction
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

                        // Set Null to Backup Objects
                        copySYS_RepOthers = null;
                        _isDirty = true;
                    }
                }
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                // Add Error to System Audit Log
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int)_codeKey, ex, true, false, new object[] { _SYS_RepOthers });
                this._SYS_RepOthers = copySYS_RepOthers;
                throw Error(ex);
            }

            return true;
        }
        #endregion Copy Method

        #region Save Method

        public bool Save(int RepGroup)
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
                { return false; }

                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();                       

                        if (!_SYS_RepOthers.Delete(cn, new SYSRepOtherss.Criteria(0, RepGroup, 2)))
                        { return false; }

                        if (!_SYS_RepOthers.Insert(cn))
                        { return false; }

                        // Alert Process

                        // Commit Process
                        msgID = string.Empty;
                        isSave = true;

                        // No errors - commit transaction
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                        _isDirty = false;

                    }// End of SqlConnection
                }// End of TransactionScope

                // Audit Log
                //SysAuditLogUtility.AddAuditLog( GEnum.AuditLogMode.Add, _codeKey, new object[] { });

            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                if (isCommitTransFail)
                    msgID = MsgID.Validation.CommitTransFail;

                // Add Error to System Audit Log
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int)_codeKey, ex, true, false, new object[] { _SYS_RepOthers });
                throw (ex);
            }

            return isSave;
        }

        #endregion //Save Method

        #region GetReadOnly Method

        //public bool GetReadOnly(out string msgID, int? currKey)
        //{
        //    bool isGetReadOnly = false;
        //    msgID = MsgID.Common.GetFail;
        //    bool processOk = true;

        //    // Copy original object
        //    SYSRepOtherss copySYS_RepOthers = null;


        //    if (!GFunc.IsNE(this._SYS_RepOthers))
        //        copySYS_RepOthers = this._SYS_RepOthers.Copy();



        //    try
        //    {
        //    }
        //    catch
        //    {
        //    }
        //}
        #endregion


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

        private bool Validation()
        {
            bool isValid = true;
            foreach (DataRow dr in _SYS_RepOthers.Rows)
            {
                if (GFunc.IsNEZ(dr["RepKey"]) || GFunc.IsNEZ(dr["RepGroup"]))
                {
                    dr.RowError = "Validation failed.";
                    isValid = false;
                }
            }

            return isValid;
        }

        //Error Exceptions
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _SYS_RepOthers, _ROSYSRep });
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _SYS_RepOthers, _ROSYSRep });
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
    }
}