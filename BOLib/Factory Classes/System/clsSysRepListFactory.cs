using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using System.ComponentModel;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class SYSRepListFactory : CommandBase
    {
        #region Member variables and constants

        private DataTable _ROSYSRepGrp = null;
        private DataTable _ROSYSRep = null;
        //private DataTable _ROSYSRepRpt = null;

        SYSRepCriterias _Criterias = null;
        SYSRepParas _Parameters = null;
        SYSRepRpt _SYSRepRpt = null;
        SYSRep _SYSRep = null;

        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private int _guID = 0;

        // System Code Key for this Factory.
        public const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Report_Set_Rpt_Files;

        // Permission ID for this Factory.
        public const string constPermID = GVar.PermissionID.Report_Rpt_File_Setting;

        #endregion // Member variables and constant

        #region Factory Properties

        public DataTable ROSYSRepGrp
        {
            get
            {
                return this._ROSYSRepGrp;
            }
        }
  
        public SYSRepRpt SYSRepRpt
        {
            get
            {
                return this._SYSRepRpt;
            }
        }


        public DataTable ROSYSRep
        {
            get
            {
                return this._ROSYSRep;
            }
        }
        public SYSRep ObjSYSRep
        {
            get
            {
                _SYSRep = (_SYSRepRpt.RepKey != null) ? SYSRep.Get(_SYSRepRpt.RepKey) : null;
                return this._SYSRep;
            }
        }

        public SYSRepCriterias ObjRepCriterias
        {
            get
            {
                return this._Criterias;
            }
        }
        public SYSRepParas ObjRepParameters
        {
            get
            {
                return this._Parameters;
            }
            set { _Parameters = value; }
        }
        internal GEnum.InstanceMode InstanceMode
        {
            get
            {
                return this._instanceMode;
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
       
        public SYSRepListFactory(GEnum.InstanceMode instanceMode)
        {
            try
            {
                this.Initialisation(instanceMode);
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
        public bool Initialisation(GEnum.InstanceMode instanceMode)
        {
            bool isInitialisation = false;
            string msgID = MsgID.Common.InitialisationFail;
            bool processOK = true;

            // Check Permission
            try
            {
                processOK = SECPermUtility.Perform(constPermID, true);

                if (processOK)
                {
                    if (this.InstanceMode == GEnum.InstanceMode.Normal)
                    {
                        try
                        {
                            // Create TransactionScope
                            using (TransactionScope scope = new TransactionScope())
                            {
                                // Create SqlConnection
                                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                                {
                                    // Open Connection
                                    cn.Open();

                                    // Get Instance GUID
                                    if (processOK)
                                        this._guID = SysOptionUtility.GetNewLockingGUID(cn);

                                    // Locking
                                    if (SysLockUtility.IsProcessLock(cn, false, GEnum.SysLockOption.ByReadOnlyList, constCodeKey, this._guID))
                                    {
                                        this._guID = -1;
                                        return false;
                                    }


                                    // Add Inprogress Lock
                                    if (!SysLockUtility.AddListInprogressLock(cn, true, this._guID, constCodeKey))
                                    {
                                        this._guID = -1;
                                        return false;
                                    }

                                    // Commit Process
                                    if (processOK)
                                    {
                                        isInitialisation = true;
                                        // No errors - commit transaction
                                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                                    }
                                    else
                                    {
                                        isInitialisation = false;
                                        MsgBox.Show(cn,msgID);
                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            // Add Error to System Audit Log
                            throw Error(ex);
                        }
                    }
                    else
                    {
                        this._guID = 0;
                        this._instanceMode = GEnum.InstanceMode.InternalCall;
                        isInitialisation = true;
                    }
                }
                return isInitialisation;
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

        public bool Get(int SecGrpKey)
        {
            string msgID = MsgID.Common.GetFail;
            bool processOK = true;

            if (this.InstanceMode == GEnum.InstanceMode.Normal)
            {
                try
                {
                    // Check Permission
                    processOK = SECPermUtility.Perform(constPermID, true);

                    if (processOK)
                    {
                        if (SecGrpKey == 0)
                        {
                            _ROSYSRep = SYSList.GetReports(4, 0);
                        }
                        else
                        {
                            _ROSYSRep = SYSList.GetReports(3, SecGrpKey);
                        }

                        if (msgID == string.Empty)
                        {
                            processOK = true;
                        }
                        else
                        {
                            processOK = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Add Error to System Audit Log
                    throw Error(ex);
                }
            }
            else
            {
                msgID = MsgID.Common.WrongInstanceMode;
            }
            return processOK;
        }      
        public bool GetRptFile(int UID)
        {
            string msgID = MsgID.Common.GetFail;
            bool processOK = true;

            if (this.InstanceMode == GEnum.InstanceMode.Normal)
            {
                try
                {
                    // Check Permission
                    processOK = SECPermUtility.Perform(constPermID, true);

                    if (processOK)
                    {
                        _SYSRepRpt = new SYSRepRpt();
                        _SYSRepRpt.Fetch(new SYSRepRpt.Criteria(UID, 2));

                        if (msgID == string.Empty)
                        {
                            processOK = true;
                        }
                        else
                        {
                            processOK = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Add Error to System Audit Log
                    throw Error(ex);
                }
            }
            else
            {
                msgID = MsgID.Common.WrongInstanceMode;
            }
            return processOK;
        }      
        public bool GetCriterias(int? RepKey)
        {
            bool processOK = true;

            if (this.InstanceMode == GEnum.InstanceMode.Normal)
            {
                try
                {
                    // Check Permission
                    processOK = SECPermUtility.Perform(constPermID, true);

                    if (processOK)
                    {
                        _Criterias = new SYSRepCriterias();
                        if (_Criterias.Fetch(new SYSRepCriterias.Criteria(RepKey, 1)))
                            processOK = true;
                        else
                        {
                            processOK = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Add Error to System Audit Log
                    SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int?)constCodeKey, ex, true, true, _ROSYSRep);
                    throw Error(ex);
                }
            }           
            return processOK;
        }
        public bool GetParams(int? RepKey)
        {          
            bool processOK = true;

            if (this.InstanceMode == GEnum.InstanceMode.Normal)
            {
                try
                {
                    // Check Permission
                    processOK = SECPermUtility.Perform(constPermID, true);

                    if (processOK)
                    {
                        _Parameters = new SYSRepParas();
                        if (_Parameters.Fetch(new SYSRepParas.Criteria(RepKey, 1)))
                        {
                            processOK = true;
                        }
                        else
                        {
                            processOK = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Add Error to System Audit Log
                    SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int?)constCodeKey, ex, true, true, _Parameters);
                    throw Error(ex);
                }
            }             
            return processOK;           
        }          

        public bool Dispose()
        {
            try
            {
                bool processOK = true;

                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                    processOK = SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0);

                return processOK;
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
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _SYSRep, _SYSRepRpt });
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _SYSRep, _SYSRepRpt });
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
    }
}
