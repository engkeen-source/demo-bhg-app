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
    public class SYSRepRptFactory : CommandBase
    {
        #region Member variables and constants

        private SYSRepRpts _SYSRepRpts = null;

        private DataTable _SYSRep = null;

        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isOpenReadOnly = false;
        private int _guID = 0;

        // System Code Key for this Factory.
        private  GEnum.SystemCode _codeKey = GEnum.SystemCode.Report_Set_Rpt_Files;

        // Permission ID for this Factory.
        public const string constPermID = GVar.PermissionID.Report_Rpt_File_Setting;

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

        public SYSRepRpts ObjSYSRepRptss
        {
            get
            {
                return this._SYSRepRpts;
            }
        }

        public DataTable ObjSYSRep
        {
            get
            {
                return this._SYSRep;
            }
        }

        public bool IsDirty
        {
            get
            {
                return this._isDirty;
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
        public SYSRepRptFactory()
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
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
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
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int)_codeKey, ex,true,false, new object[] { _SYSRepRpts });
                throw (ex);
            }

            return true; ;
        }

        #endregion //Initialisation Method

        public bool GetAllReports()
        {
            

            try
            {
                // Check Permission
                if (!SECPermUtility.Perform(constPermID, true))
                { return false; }

                _SYSRep = SYSList.GetReports(1, 0);
                _SYSRep.ColumnChanged += new DataColumnChangeEventHandler(_SYSRep_ColumnChanged);

                return true;



            }
            catch (Exception ex)
            {
                // Add Error to System Audit Log
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int?)_codeKey, ex, true, true, _SYSRep);
                throw (ex);
            }
        }

        void _SYSRep_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            _isDirty = true;
        }


        #region GetEdit Method

        public bool GetEdit(int? RepKey)
        {
            // Initialisation
            bool isGetEdit = false;
            string msgID = MsgID.Common.GetFail;

            // Copy original object
            SYSRepRpts copySYSRepRpts = null;

            if (!GFunc.IsNE(this._SYSRepRpts))
                copySYSRepRpts =  GFunc.TACopyDataTable(_SYSRepRpts);

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

                        // Check Lock                        
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, _codeKey, RepKey, 0, _guID))
                            return false;

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, _codeKey))
                            return false;

                        // Add Lock
                        if (!SysLockUtility.AddLock(cn, true, this._guID, _codeKey, RepKey))
                            return false;

                        if (_SYSRepRpts != null)
                            _SYSRepRpts.Dispose();
                        _SYSRepRpts = new SYSRepRpts();

                        if (!_SYSRepRpts.Fetch(cn, new SYSRepRpts.Criteria(RepKey, 1)))
                            throw new ApplicationException(msgID);

                        this._SYSRepRpts.ColumnChanged += new DataColumnChangeEventHandler(_SYSRepRpts_ColumnChanged);
                        this._SYSRepRpts.RowChanged += new DataRowChangeEventHandler(_SYSRepRpts_RowChanged);
                        this._SYSRepRpts.RowDeleted += new DataRowChangeEventHandler(_SYSRepRpts_RowDeleted);
                        // Commit Process                           
                        this._isNew = false;
                        this._isOpenReadOnly = false;
                        msgID = string.Empty;
                        isGetEdit = true;

                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        // Set Null to Backup Objects
                        copySYSRepRpts = null;
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
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int)_codeKey, ex,true,false, new object[] { _SYSRepRpts });
                this._SYSRepRpts = copySYSRepRpts;
                throw (ex);
            }

            return isGetEdit;
        }

        void _SYSRepRpts_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            _isDirty = true;
        }

        void _SYSRepRpts_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            _isDirty = true;
        }

        void _SYSRepRpts_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            _isDirty = true;
        }

        #endregion GetEdit Method

        #region Copy Method

        public bool Copy(int? NewRepGrpKey, int? CopyGrpKey)
        {
            string msgID = MsgID.Common.GetFail;

            // Copy original object
            SYSRepRpts copySYSRepRpts = null;

            try
            {
                if (!GFunc.IsNE(this._SYSRepRpts))
                copySYSRepRpts =  GFunc.TACopyDataTable(_SYSRepRpts);

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

                        // Check Lock                        
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, _codeKey, NewRepGrpKey, 0, _guID))
                            return false;

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, _codeKey))
                            return false;

                        // Add Lock
                        if (!SysLockUtility.AddLock(cn, true, this._guID, _codeKey, NewRepGrpKey))
                            return false;

                        if (_SYSRepRpts == null)
                            _SYSRepRpts = new SYSRepRpts();
                        else
                            _SYSRepRpts.Rows.Clear();

                        if (!_SYSRepRpts.Fetch(cn, new SYSRepRpts.Criteria(CopyGrpKey, 2)))
                            throw new ApplicationException(msgID);
                        else
                        {
                            foreach (DataRow dr in _SYSRepRpts.Rows)
                            {
                                dr["RepGroup"] = NewRepGrpKey;
                            }

                        }

                        this._SYSRepRpts.RowChanged += new DataRowChangeEventHandler(_SYSRepRpts_RowChanged);
                        this._SYSRepRpts.RowDeleted += new DataRowChangeEventHandler(_SYSRepRpts_RowDeleted);
                        // Commit Process                           
                        this._isNew = false;
                        this._isOpenReadOnly = false;
                        msgID = string.Empty;

                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        // Set Null to Backup Objects
                        copySYSRepRpts = null;
                        _isDirty = true;
                    }
                }
            }
            catch (TAException tex)
            { throw Error(tex);  }
            catch (Exception ex)
            {
                // Add Error to System Audit Log
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error,(int) _codeKey, ex,true,false, new object[] { _SYSRepRpts });
                this._SYSRepRpts = copySYSRepRpts;
                throw Error(ex);
            }

            return true;
        }
        #endregion Copy Method

        #region Save Method

        public bool Save(SYSRep objRep)
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

                        if (!objRep.CustomUpdate(cn))
                        { return false; }

                        if (!_SYSRepRpts.Delete(cn, new SYSRepRpts.Criteria(objRep.RepKey, 1)))
                        {
                            return false;
                        }

                        if (!_SYSRepRpts.Insert(cn))
                        { return false; }

                        // Alert Process

                        // Commit Process
                        msgID = string.Empty;
                        isSave = true;

                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        _isDirty = false;
                    }// End of SqlConnection
                }// End of TransactionScope

                // Audit Log
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, _codeKey, objRep.RepKey,objRep.RPTname1,new object[]{ objRep});

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
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int)_codeKey, ex,true,false, new object[] { _SYSRepRpts });
                throw Error(ex);
            }

            return isSave;
        }

        #endregion //Save Method

        #region GetReadOnly Method

        //public bool GetReadOnly(, int? currKey)
        //{
        //    bool isGetReadOnly = false;
        //    msgID = MsgID.Common.GetFail;
        //    bool processOk = true;

        //    // Copy original object
        //    SYSRepRpts copySYSRepRpts = null;


        //    if (!GFunc.IsNE(this._SYSRepRpts))
        //        copySYSRepRpts = this._SYSRepRpts.Copy();



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
            try
            {
                foreach (DataRow dr in _SYSRepRpts.Rows)
                {
                    if (GFunc.IsNEZ(dr["RepKey"]) || GFunc.IsNE(dr["RptNm"]))
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

        //Error Exceptions
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _SYSRepRpts, _SYSRep });
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _SYSRepRpts, _SYSRep});
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
    }
}