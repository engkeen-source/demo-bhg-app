using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using System.ComponentModel;
using System.Collections.Generic;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class ListFactory : CommandBase
    {
        #region Member variables and constants
       
        private GEnum.SystemCode _codeKey = 0;
        private DataTable _ROList = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private int _guID = 0;

        // Permission ID for this Factory.
        public string _permID = string.Empty;

        #endregion // Member variables and constant

        #region Factory Properties

        public DataTable ROList
        {
            get
            {
                return this._ROList;
            }
        }      

        public int GUID
        {
            get
            {
                return this._guID;
            }
        }
        public GEnum.SystemCode CodeKey
        {
            get
            {
                return this._codeKey;
            }
        }    

        #endregion // Constructors

        #region Constructors

        /// <summary>
        /// Default constructor for this Factory.
        /// </summary>
        public ListFactory(out string msgID, GEnum.SystemCode codeKey,string permID)
        {
            _codeKey = codeKey;
            _permID = permID;
            try
            {
                this.Initialisation(out msgID);
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

        #region Initialisation Method

        public bool Initialisation(out string msgID)
        {
            bool isInitialisation = false;
            msgID = MsgID.Common.InitialisationFail;
            bool processOK = true;

            // Check Permission
            try
            {
                processOK = SECPermUtility.Read(_permID, true );
                if (processOK)
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

                                //// Get Instance GUID
                                //if (processOK)
                                //    this._guID = SysOptionUtility.GetNewLockingGUID(cn);

                                //// Locking
                                //if (processOK)
                                //    processOK = !SysLockUtility.IsProcessLock(cn, false, GEnum.SysLockOption.ByReadOnlyList, _codeKey,this._guID);

                                //// Add Inprogress Lock
                                //if (processOK)
                                //    processOK = SysLockUtility.AddListInprogressLock(cn, true, this._guID, _codeKey);

                                // Commit Process
                                if (processOK)
                                {
                                    msgID = string.Empty;
                                    isInitialisation = true;
                                    // No errors - commit transaction
                                      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                                }
                            }
                        }
                    }
                    catch (TAException tex)
                    {
                        throw (tex);
                    }
                    catch (Exception ex)
                    {
                        // Add Error to System Audit Log
                        SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error,(int?) _codeKey, ex,true,true, _ROList);
                        throw (ex);
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

        #endregion //Initialisation Method

        #region Get Method

        public bool Get(string ListSettingID)
        {            
            bool processOK = true;
            string msgID;
            try
            {
                // Check Permission
                processOK = SECPermUtility.Read(_permID, true );

               
            }
            catch (TAException tex)
            {
                throw (tex);
            }
            catch (Exception ex)
            {
                // Add Error to System Audit Log
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Error, (int?)this.CodeKey, ex, true, true, new object[] { _ROList });
                throw (ex);
            }
         
            return processOK;
        }

        #endregion //GetReadOnly Method

        #region Delete Method

        public bool Delete(out string msgID, int? Key)
        {
            msgID = MsgID.Common.DeleteFail;
            bool processOK = true;
            object[] objToLog=null;

            try
            {
                processOK = SECPermUtility.Delete(_permID, true);

                if (processOK)
                {
                    // Create TransactionScope
                    using (TransactionScope scope = new TransactionScope())
                    {
                        // Create SqlConnection
                        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                        {
                            // Open Connection
                            cn.Open();

                            // Check Use in Database
                            // NotReady

                            // Record Locking
                            if (processOK)
                                processOK = SysLockUtility.CheckAddLock(cn, true, 0, _codeKey, Key, _guID);

                            // Delete Record
                            switch (_codeKey)
                            {
                                case GEnum.SystemCode.Customer:
                                case GEnum.SystemCode.Vendor:
                                    MSTCon objMSTCon = MSTCon.New();                                   
                                    REFAddr objREFAddr = REFAddr.New();
                                    REFContactInfor objREFContactInfor = REFContactInfor.New();

                                    if (processOK)
                                    {                                      
                                        // Check the record is used in other dependency tables
                                        if (GFunc.CheckKeyDependantsExists(cn, "ConKey", (int)Key, string.Empty))
                                        { return false; }
                                        processOK = objMSTCon.Delete(cn, new MSTCon.Criteria(Key));
                                    }
                                    if (processOK)
                                    {                                       
                                        processOK = objREFAddr.Delete(cn, new REFAddr.Criteria((int?)GEnum.AddrLinkType.CustomerOrVendor, Key, 0));
                                    }
                                    if (processOK)
                                    {                                        
                                        objREFContactInfor.Delete(cn, new REFContactInfor.Criteria(0, (int?)GEnum.ContactLinkType.CustomerOrVendor, Key, 0));
                                    }
                                    //Delete all Equipment items by EqptConKey

                                    //if (processOK)
                                    //{
                                    //    MSTEqpt objMSTEqpt = MSTEqpt.New();
                                    //    objMSTEqpt.Delete(cn, new MSTEqpt.Criteria(Key, 0, 1));
                                    //    objToLog = new object[] { objMSTEqpt };
                                    //}
                                    objToLog = new object[] {objMSTCon,objREFAddr, objREFContactInfor };
                                    break;
                                case GEnum.SystemCode.Inventory:
                                    if (GFunc.CheckKeyDependantsExists(cn, "ItmKey", (int)Key, string.Empty))
                                    { return false; }

                                    MSTItmDetAlt _MSTItmDetAlts = MSTItmDetAlt.New();
                                    MSTItmDetAss _MSTItmDetAsss = MSTItmDetAss.New();
                                    MSTItmDetBOM _MSTItmDetBOMRMs = MSTItmDetBOM.New();
                                    MSTItmDetBOM _MSTItmDetBOMPMs = MSTItmDetBOM.New();
                                    MSTItmDetPrice _MSTItmDetPrice = MSTItmDetPrice.New();
                                    MSTItm objMSTItm = MSTItm.New();

                                    if (processOK)
                                    {                                       
                                        processOK = _MSTItmDetAlts.Delete(cn, new MSTItmDetAlt.Criteria(Key,1));
                                    }
                                    if (processOK)
                                    {                                        
                                        processOK = _MSTItmDetAsss.Delete(cn, new MSTItmDetAss.Criteria(Key, 1));
                                    }
                                    if (processOK)
                                    {
                                        processOK = _MSTItmDetBOMRMs.Delete(cn, new MSTItmDetBOM.Criteria(Key, GEnum.BOMLineType.Raw_Material, 1));
                                    }
                                    if (processOK)
                                    {
                                        processOK = _MSTItmDetBOMPMs.Delete(cn, new MSTItmDetBOM.Criteria(Key, GEnum.BOMLineType.Packing_Material, 1));
                                    }
                                    if (processOK)
                                    {                                        
                                        processOK = _MSTItmDetPrice.Delete(cn, new MSTItmDetPrice.Criteria(Key, 1));
                                    }
                                    if (processOK)
                                    {                                        
                                        processOK = objMSTItm.Delete(cn, new MSTItm.Criteria(Key));
                                    }
                                    
                                    objToLog=new object[]{objMSTItm,_MSTItmDetAlts,_MSTItmDetAsss,_MSTItmDetBOMRMs,_MSTItmDetBOMPMs,_MSTItmDetPrice};
                                    break;
                                case GEnum.SystemCode.Account:
                                    if (processOK)
                                    {
                                        MSTAcc objMSTAcc = MSTAcc.New();
                                        // Check the record is used in other dependency tables
                                        if (GFunc.CheckKeyDependantsExists(cn, "AccKey",(int)Key,string.Empty))
                                            return false;
                                        processOK = objMSTAcc.Delete(cn, new MSTAcc.Criteria(Key));
                                        objToLog = new object[] { objMSTAcc };
                                    }
                                    break;
                                case GEnum.SystemCode.Job:
                                    if (processOK)
                                    {
                                        MSTJob objMSTJob = MSTJob.New();
                                        // Check the record is used in other dependency tables
                                        if (GFunc.CheckKeyDependantsExists(cn, "JobKey", (int)Key,string.Empty))
                                        { return false; }
                                        processOK = objMSTJob.Delete(cn, new MSTJob.Criteria(Key));
                                        
                                        objToLog = new object[] { objMSTJob };
                                    }
                                    break;
                                case GEnum.SystemCode.Price_List:
                                    if (processOK)
                                    {
                                        MSTPriceList objMSTPriceList = MSTPriceList.New();
                                        MSTPriceListDetValue ObjMSTPriceListDetValue = MSTPriceListDetValue.New();
                                        MSTPriceListDetRatio ObjMSTPriceListDetRatio = MSTPriceListDetRatio.New();

                                        // Check the record is used in other dependency tables
                                        if (GFunc.CheckKeyDependantsExists(cn, "PriceKey", (int)Key,string.Empty))
                                        { return false; }

                                        //Check for Option Table
                                        if (GFunc.CheckKeyDependcyinOptionTable(cn, "MSTPrice", (int)Key))
                                            return false;

                                        //Delete Record
                                        processOK = objMSTPriceList.Delete(cn, new MSTPriceList.Criteria(Key));

                                        if (processOK)
                                        {                                            
                                            processOK = ObjMSTPriceListDetValue.Delete(cn, new MSTPriceListDetValue.Criteria(Key));
                                        }
                                        if (processOK)
                                        {                                            
                                            ObjMSTPriceListDetRatio.Delete(cn, new MSTPriceListDetRatio.Criteria(Key));
                                        }
                                         objToLog=new object[]{objMSTPriceList,ObjMSTPriceListDetValue, ObjMSTPriceListDetRatio};
                                    }
                                    
                                    break;
                                case GEnum.SystemCode.Ship_Name:
                                    if (processOK)
                                    {
                                        MSTShipName ObjMSTShipName = MSTShipName.New();
                                        MSTShipNameDetItms ObjMSTShipNameDetItms = new MSTShipNameDetItms(cn);

                                        // Check the record is used in other dependency tables
                                        if (GFunc.CheckKeyDependantsExists(cn, "ShipNameKey", (int)Key, string.Empty))
                                        { return false; }

                                        processOK = ObjMSTShipName.Delete(cn, new MSTShipName.Criteria(Key, 0));

                                        if (processOK)
                                        {                                           
                                            ObjMSTShipNameDetItms.Delete(cn, new MSTShipNameDetItms.Criteria(Key, 1));
                                        }
                                        objToLog = new object[] { ObjMSTShipName, ObjMSTShipNameDetItms };
                                    }
                                    break;
                                case GEnum.SystemCode.To_Do:
                                    if (processOK)
                                    {
                                        TASToDo ObjToDo = TASToDo.New();                                       
                                        // Check the record is used in other dependency tables
                                        if (GFunc.CheckKeyDependantsExists(cn, "ToDoKey", (int)Key, ObjToDo.ToDoDes))
                                        { return false; }

                                        // Delete Record
                                        processOK= ObjToDo.Delete(cn, new TASToDo.Criteria((int)Key, 0));
                                        //Information: In store prco, also delete from detail Tables and TAAgent Database- AgentTask Table

                                        objToLog = new object[] { ObjToDo };
                                    }
                                    break;
                            }

                            if (processOK)
                                processOK = SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, _codeKey);

                            // Alert Process
                            if (processOK)
                            {
                                //NotReady
                            }

                            if (processOK)
                            {
                                // No errors - commit transaction
                                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                            }
                        }// End of SqlConnection
                    }// End of TransactionScope
                    //Audit Log
                }

                if (processOK)
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, _codeKey,Key,"", objToLog);
            }
            catch (TAException tex)
            {
                throw (tex);
            }
            catch (Exception ex)
            {
                // Add Error to System Audit Log
                SysAuditLogUtility.AddErrorLog(GEnum.AuditLogMode.Delete, (int?)_codeKey, ex,true,true, _ROList);

                throw (ex);
            }
           
            return processOK;
        }

        #region Dispose Method

        public bool Dispose(out string msgID)
        {
            bool isDispose = false;
            bool processOK = true;
            msgID = string.Empty;
            try
            {
                processOK = SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, _codeKey, GUID, 0, 0);
                if (processOK)
                    isDispose = true;

                return isDispose;
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

        #endregion //Dispose Method

        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _ROList });
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _ROList });
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        #endregion

    }
}
