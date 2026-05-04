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
    public class ARSOFactory
    {
        #region Member variables and constants
        private ARSO _Doc = null;
        private ARSODetItms _DocDetItms = null;

        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private int _guID = 0;
        private bool _isError = false;       
        private GEnum.SystemCode _codeKey = GEnum.SystemCode.Sales_Order;
        private string _permID = GVar.PermissionID.Sales_Order;
        Hashtable htDetails = new Hashtable();
        private bool approvalRequired = false;
        #endregion

        #region Custom Event Declaration
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;
        #endregion

        #region Factory Properties
        public ARSO Doc
        {
            get
            {
                return this._Doc;
            }
        }
        public ARSODetItms DocDetItms
        {
            get
            {
                return this._DocDetItms;
            }
            set
            {
                this._DocDetItms = value;
            }
        }
        public bool ApprovalRequired
        {
            get
            {
                return this.approvalRequired;
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
        #endregion

        //Constructors, Initialisation
        public ARSOFactory(GEnum.InstanceMode instanceMode, GEnum.SystemCode DocCodeKey)
        {
            try
            {
                this._instanceMode = instanceMode;
                Initialisation(DocCodeKey);
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
        public bool Initialisation(GEnum.SystemCode DocCodeKey)
        {
            try
            {
                //Initialise Properties
                bool isReadonly = false;
                this._isError = true;
                _codeKey = DocCodeKey;
                _permID = GVar.PermissionID.Sales_Order;

                //We cannot use throw to indicate an initialisation failure because the calling form should only close the form and there
                //is no need to show message as all message is already display by the utility function
                //therefore we use isErorr = true to indicate failure to initialise factory
                //in this function the return of true or false actually is not use at this point of time
                //Function return - True/False - not used
                //IsError status - True/False - used to check if the initialised of factory has failed

                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                {
                    if (SECPermUtility.Any(_permID, out isReadonly, true) == false)
                        return false;

                    using (TransactionScope scope = new TransactionScope())
                    {
                        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                        {
                            cn.Open();

                            if ((_guID = SysOptionUtility.GetNewLockingGUID(cn)) == 0)
                                return false;
                            
                            if (SysLockUtility.IsProcessLock(cn, true, GEnum.SysLockOption.ByCodKey, _codeKey, _guID))
                                return false;

                            if (SysLockUtility.AddInprogressLock(cn, true, _guID, _codeKey) == false)
                                return false;

                            #region prepare New instances
                            this._Doc = new ARSO();
                            this._Doc._GUID = _guID;
                            this._Doc._DocCodeKey = (int)_codeKey;
                            this._DocDetItms = new ARSODetItms(cn);
                            #endregion

                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        }
                    }
                }
                else if (this.InstanceMode == GEnum.InstanceMode.InternalCall)
                {
                    if (SECPermUtility.Any(_permID, out isReadonly, true) == false)
                        return false;

                    using (TransactionScope scope = new TransactionScope())
                    {
                        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                        {
                            cn.Open();

                            if ((_guID = SysOptionUtility.GetNewLockingGUID(cn)) == 0)
                                return false;

                            if (SysLockUtility.IsProcessLock(cn, true, GEnum.SysLockOption.ByCodKey, _codeKey, _guID))
                                return false;

                            #region prepare New instances
                            this._Doc = new ARSO();
                            this._Doc._GUID = _guID;
                            this._Doc._DocCodeKey = (int)_codeKey;
                            this._DocDetItms = new ARSODetItms(cn);
                            #endregion

                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        }
                    }
                }

                approvalRequired = DocUtility.DocApprovalRequired_Get((int)_codeKey, ref approvalRequired); /* added by YST */
                _isError = false;
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
        
        //Doc Methods
        public string New(TAUtil.TAGridEditor grd,Boolean isCash=false)
        {
            #region Declaration
            ARSO copyARSO = null;
            ARSODetItms copyARSODetItms = null;
            bool restoreFlag = false;
            int newDocKey = 0;
            bool isReadOnly = false;
            #endregion

            try
            {
                #region Copy original object or Attach Eventhandler to new object
                if (!GFunc.IsNE(_Doc))
                    copyARSO = _Doc.Clone();

                if (_DocDetItms != null)
                    copyARSODetItms = GFunc.TACopyDataTable(_DocDetItms);
                #endregion

                #region Check Security Permission
                if (!SECPermUtility.Any(_permID, out isReadOnly, true))
                    return GVar.gcCancel;
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
                            return GVar.gcCancel;
                        #endregion

                        #region prepare New instances
                        this._Doc = new ARSO();
                        this._Doc._GUID = _guID;
                        this._Doc._DocCodeKey = CodeKey;
                        this._DocDetItms = new ARSODetItms(cn);
                        this._Doc.Attachments = new SYSAttachments();
                        #endregion

                        #region Set ObjDoc flags
                        _Doc.IsReadOnly = isReadOnly;
                        _Doc.IsDirty = false;
                        _Doc.IsNew = true;
                        #endregion

                        #region Assign DocKey
                        newDocKey = SysOptionUtility.NewDocKey_Get(cn, _codeKey);
                        if (newDocKey == 0)
                            return GVar.gcCancel;
                        else
                            _Doc._DocKey = newDocKey;
                        #endregion

                        #region set default values to DocObj and Details
                        Doc_SetDefaultValue(cn);
                        DocDetItem_SetDefaultValue();
                        #endregion

                        #region prepare htDetails
                        htDetails.Clear();
                        htDetails.Add(GEnum.Details.Doc_Itm, grd);
                        #endregion

                        //Prepare new document for data entry
                        if (!DocUtility.Document_New(cn, _Doc, htDetails,isCash))
                            return GVar.gcCancel;

                        #region Attached events to handle objects and dtTables
                        this._Doc.PropertyChanged += new PropertyChangedEventHandler(Obj_PropertyChanged);
                        this._Doc.Attachments.ListChanged += new ListChangedEventHandler(Attachments_ListChanged);
                        #endregion

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                    }
                    restoreFlag = false;
                    return GVar.gcPass;
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
                    this._Doc = copyARSO;
                    this._DocDetItms = copyARSODetItms;
                }
                #endregion

                #region Reset Backup Objects
                copyARSO = null;
                copyARSODetItms = null;
                #endregion
            }
        }//Completed
        public DataTable CheckDuplicateCustPO()
        {
            List<SqlParameter> parList = new List<SqlParameter>();
            parList.Add(new SqlParameter("@DocCodeKey", _Doc.DocCodeKey));
            parList.Add(new SqlParameter("@DocKey", _Doc.DocKey));
            parList.Add(new SqlParameter("@DocDate", _Doc.DocDate));
            parList.Add(new SqlParameter("@DocCustPONum", _Doc.DocCustPONum));

            return GFunc.ExecuteProc("Doc_GetDuplicateCustPO", parList);
        }

        public string GetEdit(int docKey, string docID)
        {
            #region Declaration
            ARSO copyARSO = null;
            ARSODetItms copyARSODetItms = null;
            bool restoreFlag = false;
            #endregion

            try
            {
                #region Backup original object or Attach Eventhandler to new object

                if (!GFunc.IsNE(_Doc))
                    copyARSO = _Doc.Clone();

                if (_DocDetItms != null)
                    copyARSODetItms = GFunc.TACopyDataTable(_DocDetItms);

                #endregion

                #region Check Permission, Record Access and valid docKey/docID
                if (!SECPermUtility.Edit(_permID, true))
                    return GVar.gcCancel;

                if (GFunc.CanAccessDocument((int)_codeKey, docKey, docID) == false)
                    return GVar.gcCancel;

                if (docID != string.Empty)
                    docKey = GFunc.DocKey_Get((int)_codeKey, docID);

                if (docKey == 0)
                    throw new TAException("Unable to locate document.");
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        #region Check, remove and add lock
                        // Check Lock
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, _codeKey, docKey, 0, _guID))
                            return GVar.gcCancel;
                        
                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, _guID, _codeKey))
                            return GVar.gcCancel;

                        // Add Lock
                        if (!SysLockUtility.AddLock(cn, true, _guID, _codeKey, docKey))
                            return GVar.gcCancel;

                        //added by thettm on 24 dec 2018(start)
                        // Add Lock for Link Doc
                        List<SqlParameter> paraList1 = new List<SqlParameter>();
                        paraList1.Add(new SqlParameter("@GUID", _guID));
                        paraList1.Add(new SqlParameter("@CodeKey", _codeKey));
                        paraList1.Add(new SqlParameter("@DocKey", docKey));
                        paraList1.Add(new SqlParameter("@UserKey", AppInfor.currentUserKey));
                        SqlParameter ErrMsg = new SqlParameter("@ErrMsg", SqlDbType.NVarChar, 500);
                        ErrMsg.Direction = ParameterDirection.Output;
                        paraList1.Add(ErrMsg);
                        SqlParameter RetValue = new SqlParameter("@RetValue", SqlDbType.Int);
                        RetValue.Direction = ParameterDirection.InputOutput;
                        paraList1.Add(RetValue);

                        GFunc.ExecuteProc(cn, "Doc_Lock", paraList1);
                        if (GFunc.NEInt(RetValue.Value, 0) <= 0)
                        {
                            if ((int)RetValue.Value == -2)
                                MsgBox.Show(cn, GFunc.NEStr(ErrMsg.Value, "One or more Link Document is locked."));
                            return GVar.gcCancel;
                        }
                        //added by thettm on 24 dec 2018(end)


                        #endregion

                        #region Fetch Data into Object
                        //By DocKey
                        if (_Doc.Fetch(cn, new ARSO.Criteria((int)_codeKey, docKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return GVar.gcCancel;
                        }

                        //Record Not Found
                        if (GFunc.NEInt(this._Doc.DocKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }
                        #endregion

                        #region Fetch Data in dtTables
                        _DocDetItms.Clear();
                        _Doc.Attachments.Clear();
                        _DocDetItms.Fetch(cn, new ARSODetItms.Criteria(docKey, 1));
                        this._Doc.Attachments.Fetch(cn, new SYSAttachments.Criteria((int)this._Doc.DocCodeKey, this._Doc.DocKey, 1));
                        #endregion

                       if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  
                           throw new Exception("Transaction has aborted."); 
                        scope.Complete();                     
                    }
                }

                #region Set ObjDoc Flag
                _Doc.GUID = _guID;
                _Doc.IsNew = false;
                _Doc.IsDirty = false;
                _Doc.IsReadOnly = DocUtility.Doc_OpenDisAllowEdit(_Doc);
                #endregion

                #region Set Read Only /* modified by YST 2023/08/10 */
                /*
                 * ADL SO/OrderBook never should be ReadOnly
                 * BHM should be ReadOnly after releasing to WMS 
                 * BHM & other subsidiaries should be ReadOnly while pending approval or after converting DO (i.e one SO to one DO)                
                 */
                if (SysOptionUtility.DatabaseBranchCode == DBCode.ADL)
                    _Doc.IsReadOnly = false;
                else
                {
                    if (!_Doc.IsReadOnly)
                    {
                        if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                            _Doc.IsReadOnly = (_Doc.DocState == (int)GEnum.DocState.Posted);

                        if (!_Doc.IsReadOnly && _Doc.DocState > (int)GEnum.DocState.Pending) /* not to execute sp if IsReadOnly already true -- modified by YST */
                        {
                            //added by thettm on 28 jun 2018 (start)
                            List<SqlParameter> paraList = new List<SqlParameter>();
                            paraList.Add(new SqlParameter("@DocCodeKey", _Doc.DocCodeKey));
                            paraList.Add(new SqlParameter("@DocKey", _Doc.DocKey));
                            DataTable dtDocRelationShip = GFunc.ExecuteProc("DocumentLink_Get", paraList);
                            //added by thettm on 28 jun 2018 (end)

                            if (dtDocRelationShip.Select("DocCodeKey=" + ((int)GEnum.SystemCode.Delivery_Order).ToString()).Count() > 0)
                                _Doc.IsReadOnly = true;
                        }
                    }
                }                
                #endregion

                #region /* commented by YST */
                /*
                //added by thettm on 28 jun 2018 (start)
                List<SqlParameter> paraList = new List<SqlParameter>();
                paraList.Add(new SqlParameter("@DocCodeKey", _Doc.DocCodeKey));
                paraList.Add(new SqlParameter("@DocKey", _Doc.DocKey));
                DataTable dtDocRelationShip = GFunc.ExecuteProc("DocumentLink_Get", paraList);
                //added by thettm on 28 jun 2018 (end)
                
                if (!_Doc.IsReadOnly)
                    if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                        _Doc.IsReadOnly = (_Doc.DocState == (int)GEnum.DocState.Posted || _Doc.DocState == (int)GEnum.DocState.Pending);
                    else if (SysOptionUtility.DatabaseBranchCode == DBCode.ADL)            // added by KKAung on 20 Jun 2023
                        _Doc.IsReadOnly = false;
                    else if (dtDocRelationShip.Select("DocCodeKey=" + ((int)GEnum.SystemCode.Delivery_Order).ToString()).Count() > 0)
                        _Doc.IsReadOnly = true;


                //if (_Doc.IsReadOnly && SysOptionUtility.DatabaseBranchCode == "ADL")      // added by KKAung on 24 Apr 2023
                //    _Doc.IsReadOnly = false;     
                */
                #endregion

                #region Set detail default values
                DocDetItem_SetDefaultValue();
                #endregion

                restoreFlag = false;
                //Check if user has permission to edit the already printed document
                if (Doc.DocPrinted)
                {
                    if (SECPermUtility.Perform(GVar.PermissionID.Save_Printed_Sales_Order, true) == false)
                    {
                        MarkAsReadOnly();
                    }
                }
                return GVar.gcPass;
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
                    this._Doc = copyARSO;
                    this._DocDetItms = copyARSODetItms;
                }
                #endregion

                #region Set Null to Backup Objects
                copyARSO = null;
                copyARSODetItms = null;
                #endregion
            }
        }//Completed
        public void MarkAsReadOnly()
        {
            //Remove all locks by GUID except inprogress Locking
            SysLockUtility.RemoveLockGUIDKeepIP(true, _guID, _codeKey);
            _Doc._isReadOnly = true;
        }
        public string GetReadOnly(int docKey, string docID)
        {
            #region Declaration
            ARSO copyARSO = null;
            ARSODetItms copyARSODetItms = null;
            bool restoreFlag = false;
            #endregion

            try
            {
                #region Copy original object
                if (!GFunc.IsNE(this._Doc))
                    copyARSO = _Doc.Clone();

                if (_DocDetItms!=null)
                    copyARSODetItms = GFunc.TACopyDataTable(_DocDetItms);
                #endregion

                #region Check Permission and record access
                if (SECPermUtility.Read(_permID, true)==false)
                    return GVar.gcCancel;

                if (GFunc.CanAccessDocument((int)_codeKey, docKey, docID) == false)
                    return GVar.gcCancel;

                if (docID != string.Empty)
                    docKey = GFunc.DocKey_Get((int)_codeKey, docID);

                if (docKey == 0)
                {
                    MsgBox.Show("Unable to locate Document");
                    return GVar.gcCancel;
                }
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
                            return GVar.gcCancel;
                        #endregion

                        #region Fetch Data into Object
                        if (_Doc.Fetch(cn, new ARSO.Criteria((int)_codeKey, docKey, 1)) == false)
                        {
                            MsgBox.Show(cn,MsgID.Common.GetFail);
                            return GVar.gcCancel;
                        }
                        #endregion

                        #region Fetch data into dtTables
                        _DocDetItms.Clear();
                        _Doc.Attachments.Clear();
                        _DocDetItms.Fetch(cn, new ARSODetItms.Criteria(docKey, 1));
                        _Doc.Attachments.Fetch(cn, new SYSAttachments.Criteria((int)this._Doc.DocCodeKey, this._Doc.DocKey, 1));
                        #endregion

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        #region Set ObjDoc Flag
                        _Doc.GUID = _guID;
                        _Doc.IsReadOnly = true;
                        _Doc.IsNew = false;
                        _Doc.IsDirty = false;
                        #endregion

                    }
                }
                restoreFlag = false;
                return GVar.gcPass;
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
                    this._Doc = copyARSO;
                    this._DocDetItms = copyARSODetItms;
                }
                #endregion

                #region Set Null to Backup Objects
                copyARSO = null;
                copyARSODetItms = null;
                #endregion
            }
        }//Completed
        public string SetReadOnlyData(DataTable dtHeader, DataSet dsDetail)
        {
            try
            {               
                #region Check Permission and record access
                if (SECPermUtility.Read(_permID, true) == false)
                    return GVar.gcCancel;

                if (dtHeader == null)
                {
                    MsgBox.Show("Unable to locate Document");
                    return GVar.gcCancel;
                }
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        #region Remove all locks by GUID except inprogress Locking
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, _guID, _codeKey))
                            return GVar.gcCancel;
                        #endregion

                        #region prepare New instances
                        this._Doc = new ARSO();
                        this._DocDetItms = new ARSODetItms(cn);
                        this._Doc.Attachments = new SYSAttachments();
                        #endregion

                        #region Fetch Data into Object
                        GFunc.ConvertDataTableToObject(dtHeader, this._Doc);
                        #endregion

                        #region Fetch data into dtTables
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[0], _DocDetItms);
                        #endregion

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        #region Set ObjDoc Flag
                        _Doc.GUID = _guID;
                        _Doc.IsReadOnly = true;
                        _Doc.IsNew = false;
                        _Doc.IsDirty = false;
                        #endregion
                    }
                }
                return GVar.gcPass;
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
        public string Save(int ButtonAction)
        {
            #region Declaration
            SqlConnection cn = null;
            ARSO copyARSO = null;
            ARSODetItms copyARSODetItms = null;
            bool restoreFlag = false;
            #endregion

            try
            {
                #region Copy original object or Attach Eventhandler to new object
                if (!GFunc.IsNE(_Doc))
                    copyARSO = _Doc.Clone();

                if (_DocDetItms!=null)
                    copyARSODetItms = GFunc.TACopyDataTable(_DocDetItms);
                #endregion

                #region If readonly cannot save
                if (_Doc.IsReadOnly)
                {
                    MsgBox.Show(MsgID.Common.RecordIsReadOnly);
                    return GVar.gcCancel;
                }
                #endregion

                cn = new SqlConnection(Database.BossDemoConnection);
                cn.Open();

                #region Validation
                if (!Doc_Validation(cn))
                    return GVar.gcCancel;

                if (!DocDetItm_Validation(cn))
                    return GVar.gcCancel;
                cn.Close();
                #endregion

                //Turn on restore flag to restore objects if any error occurs
                restoreFlag = true;

                #region SaveProcess
                htDetails.Clear();
                htDetails.Add(GEnum.Details.Doc_Itm, _DocDetItms);
                htDetails.Add(GEnum.Details.Doc_Attachment, _Doc.Attachments);

                if (DocUtility.Doc_SaveProcess(_Doc, htDetails, _permID, ButtonAction, false) == false)
                    return GVar.gcCancel;

                //UPDATE STATUS, COMPLETED FOR "CASH PAYMENT" AND "TT(BANK CHARGES APPLICABLE)" INTO ESTORE_BOSS_PO
                if (_Doc.DocTypeNm == "eStore SO")
                {
                    cn = new SqlConnection(AppInfor.CurrentDBConnectionStr);
                    cn.Open();
                    List<SqlParameter> parmList = new List<SqlParameter>();
                    parmList.Add(new SqlParameter("@SO_NO", Doc.DocID));
                    GFunc.ExecuteNonQueryProc(cn, "[Update_To_ESTORE_BOSS_PO]", parmList);
                    cn.Close();
                }

                #endregion

                #region Set ObjDoc flags
                _Doc.IsDirty = false;
                _Doc.IsNew = false;
                if(SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                    _Doc.IsReadOnly = (_Doc.DocState == (int)GEnum.DocState.Posted || _Doc.DocState == (int)GEnum.DocState.Pending);

                if (SysOptionUtility.DatabaseBranchCode == DBCode.ADL)       // added by KKAung on 24 Apr 2023
                    _Doc.IsReadOnly = false;

                #endregion

                restoreFlag = false;
                return GVar.gcPass;
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
                if (cn != null && cn.State != ConnectionState.Closed) cn.Close();

                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._Doc = copyARSO;
                    this._DocDetItms = copyARSODetItms;
                }
                #endregion

                #region Reset Backup Objects
                copyARSO = null;
                copyARSODetItms = null;
                #endregion
            }
        }//Completed
        public string Delete(TAUtil.TAGridEditor grd)
        {
            //Use only when user delete document from Document FORM
            try
            {
                if (this.Delete() != GVar.gcPass)
                    return GVar.gcCancel;

                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();

                    //Note: we need to create new instance of the current obj and dataTable cos 
                    //if the this.New method were to fail it will not restore to the deleted information
                    _Doc = new ARSO();
                    _Doc._GUID = _guID;
                    _DocDetItms = new ARSODetItms(cn);
                }
                this.New(grd);
                return GVar.gcPass;
            }
            catch (TAException ex)
            {
                throw Error(ex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public string Delete()
        {
            try
            {
                #region Checking if can delete
                if (GFunc.IsNE(this._Doc))
                    return GVar.gcCancel;

                if (_Doc.DocState == (int)GEnum.DocState.New)
                    return GVar.gcCancel;

                if (_Doc.IsReadOnly)
                {
                    MsgBox.Show(MsgID.Common.RecordIsReadOnly);
                    return GVar.gcCancel;
                }
                else
                    if (!SECPermUtility.Delete(_permID, true))
                        return GVar.gcCancel;
                #endregion
              
                #region Record Locking
                if (!SysLockUtility.CheckAddLock(true, 0, _codeKey, _Doc.DocKey, _guID))
                    return GVar.gcCancel;
                #endregion

                #region Delete Document
                Hashtable details = new Hashtable();
                details.Add(GEnum.Details.Doc_Itm, _DocDetItms);
                details.Add(GEnum.Details.Doc_Attachment, _Doc.Attachments);

                if (DocUtility.Doc_DeleteProcess( _Doc, details, PermID) == false)
                    return GVar.gcCancel;
                #endregion

                #region Remove Lock
                if (!SysLockUtility.RemoveLockGUIDKeepIP( true, _guID, _codeKey))
                    return GVar.gcCancel;
                #endregion

                return GVar.gcPass;
            }
            catch (TAException ex)
            {
                throw Error(ex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed

        public string UnlockSO()
        {
            try
            {
             


                if (!SECPermUtility.Perform("ARSOAllowToUnlock", true))
                     return GVar.gcCancel;
               
               

                return GVar.gcPass;
            }
            catch (TAException ex)
            {
                throw Error(ex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed

        public string Clear(TAUtil.TAGridEditor grd)
        {
            try
            {
                if (_Doc.IsNew)
                    return this.New(grd);
                else
                    return GVar.gcCancel;
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
        public string CopyMyself()
        {
            #region Declaration
            string opValue = string.Empty;
            ARSO copyARSO = null;
            ARSODetItms copyARSODetItms = null;
            bool restoreFlag = false;
            int newDocKey = 0;
            #endregion

            try
            {
                #region Copy original object or Attach Eventhandler to new object
                if (!GFunc.IsNE(_Doc))
                    copyARSO = _Doc.Clone();

                if (_DocDetItms!=null)
                    copyARSODetItms = GFunc.TACopyDataTable(_DocDetItms);

                #endregion

                #region Check Security Permission
                if (!SECPermUtility.Add(_permID, true))
                    return GVar.gcCancel;
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
                            return GVar.gcCancel;
                        #endregion

                        #region Set ObjDoc flags
                        _Doc.GUID = _guID;
                        _Doc.IsDirty = false;
                        _Doc.IsNew = true;
                        _Doc.IsReadOnly = false;
                        #endregion

                        #region Assign DocKey
                        newDocKey = SysOptionUtility.NewDocKey_Get(cn, _codeKey);
                        if (newDocKey == 0)
                            return GVar.gcCancel;
                        else
                            _Doc._DocKey = newDocKey;
                        #endregion

                        #region Set values in Doc
                        _Doc._DocState = 10;
                        _Doc._DocID = "";
                        _Doc._DocStatus = "";
                        _Doc._DocDate = DateTime.Today.Date;
                        _Doc._DocPrinted = false;
                        _Doc._DocCompleted = false;
                        _Doc._ApproveUserKey = 0;
                        _Doc._ApproveDate = null;
                        _Doc._DisapproveUserKey = 0;
                        _Doc._DisapproveDate = null;
                        _Doc._DisapproveCount = 0;
                        _Doc._DisapproveMsg = string.Empty;
                        _Doc._Attachment = false;
                        _Doc._CreateDate = null;
                        _Doc._CreateUserKey = 0;
                        _Doc._LastModifiedDate = null;
                        _Doc._LastModifiedUserKey = 0;
                        _Doc._PurgeKeep = 0;
                        _Doc._PurgeData = false;
                        if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM) _Doc._DocReqDate = null;

                        _Doc._DocQONum = string.Empty;
                        _Doc._DocSONum = string.Empty;
                        _Doc._DocDONum = string.Empty;
                        _Doc._DocIVNum = string.Empty;
                        _Doc._DocPONum = string.Empty;
                        _Doc._DocPDNum = string.Empty;
                        _Doc._DocBLNum = string.Empty;
                        _Doc._Custom4 = string.Empty;
                        _Doc._Custom5 = string.Empty;

                        #endregion

                        #region Set values in DetItm
                        MSTItm objItm = new MSTItm();
                        foreach (DataRow item in _DocDetItms.Rows)
                        {
                            item["DocKey"] = _Doc.DocKey;
                            item["NSLink"] = "11150-" + _Doc.DocKey + "-" + item["DocItmKey"]; ;                          
                            item["ItmQtyLink"] = 0;
                            item["ItmQtyAdj"] = 0;
                            item["ItmOrderStatus"] = 10;
                            item["CreateDate"] = DBNull.Value;
                            item["CreateUserKey"] = 0;
                            item["LastModifiedDate"] = DBNull.Value;
                            item["LastModifiedUserKey"] = 0;
                            item["ARQOID"] = string.Empty;
                            item["ARQODK"] = 0;
                            item["ARQODItm"] = 0;
                            item["APPOID"] = string.Empty;
                            item["ARROID"] = string.Empty;
                            item["ARRODK"] = 0;
                            item["ARRODItm"] = 0;
                            item["ItmPrmDate"] = DateTime.Now;
                            item["ItmRef"] = DBNull.Value;

                            objItm.Fetch(cn, new MSTItm.Criteria(GFunc.NEInt(item["ItmKey"], 0), 1));
                            item["ItmStock"] = objItm.QtyStock;

                        }
                        _DocDetItms.Columns["DocKey"].DefaultValue = _Doc.DocKey;
                        #endregion

                        #region Set values in DetAttachment
                        /*foreach (SYSAttachment Obj in this._Doc.Attachments)
                        {
                            Obj.DocDC = _Doc.DocCodeKey;
                            Obj.DocDK = _Doc.DocKey;
                        }*/
                        this._Doc.Attachments.Clear();
                        #endregion

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                    }
                }
                restoreFlag = false;
                return GVar.gcPass;
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
                    this._Doc = copyARSO;
                    this._DocDetItms = copyARSODetItms;
                }
                #endregion

                #region Reset Backup Objects
                copyARSO = null;
                copyARSODetItms = null;
                #endregion
            }
        }//Completed
        public string CopyFrom(GEnum.SystemCode sourceDocCodekey, int sourceDocKey, TAUtil.TAGridEditor grd, bool NSLink, out DataTable dtDetail,bool isCash=false)
        {
            DataSet dsCopy = null;

            try
            {
                this.New(grd);
                dsCopy = DocUtility.Doc_Copy((int)sourceDocCodekey, sourceDocKey, (int)_codeKey, this._Doc, NSLink);
                if (isCash == true)
                    dsCopy.Tables[0].Rows[0]["DocTypeNm"] = "Cash Sales Order";
                GFunc.CopyDocumentHeader(dsCopy.Tables[0], this._Doc);
                dtDetail =dsCopy.Tables[1];
                // this._Doc.Attachments = GFunc.Get_Attachments(sourceDocKey, (int)sourceDocCodekey, (Document)this._Doc);
                this._Doc.Attachments.Clear();

                #region Set values in Doc
                _Doc._DocState = 10;
                _Doc._DocID = "";
                _Doc._DocStatus = "";
                _Doc._DocDate = DateTime.Today.Date;
                _Doc._DocPrinted = false;
                _Doc._DocCompleted = false;
                _Doc._ApproveUserKey = 0;
                _Doc._ApproveDate = null;
                _Doc._DisapproveUserKey = 0;
                _Doc._DisapproveDate = null;
                _Doc._DisapproveCount = 0;
                _Doc._DisapproveMsg = string.Empty;
                _Doc._Attachment = false;
                _Doc._CreateDate = null;
                _Doc._CreateUserKey = 0;
                _Doc._LastModifiedDate = null;
                _Doc._LastModifiedUserKey = 0;
                _Doc._PurgeKeep = 0;
                _Doc._PurgeData = false;
                if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM) _Doc._DocReqDate = null;

                _Doc._DocQONum = string.Empty;
                _Doc._DocSONum = string.Empty;
                _Doc._DocDONum = string.Empty;
                _Doc._DocIVNum = string.Empty;
                _Doc._DocPONum = string.Empty;
                _Doc._DocPDNum = string.Empty;
                _Doc._DocBLNum = string.Empty;


                #endregion

                return GVar.gcPass;
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
                dsCopy = null;
            }
        }//Completed
        public string GetCopy_ByDC(int? source_DC, int? source_DK)
        {
            #region Declaration
            bool restoreFlag = true;
            ARSO copyARSO = null;
            ARSODetItms copyARSODetItms = null;
            string msgID = MsgID.Common.GetFail;
            #endregion

            try
            {
                #region Copy original object or Attach Eventhandler
                if (GFunc.IsNE(this._Doc))
                    copyARSO = this._Doc.Clone();

                if (this._DocDetItms != null)
                    copyARSODetItms = GFunc.TACopyDataTable(_DocDetItms);
                #endregion

                #region Check Permission
                if (SECPermUtility.Edit(PermID, true)==false)
                    return GVar.gcCancel;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        #region remove lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, GEnum.SystemCode.Sales_Order))
                            return GVar.gcCancel;
                        #endregion

                        #region Fetch Data into Object
                        switch (source_DC)
                        {
                            case (int)GEnum.SystemCode.Quotation:
                                if (!_Doc.Fetch_ARQO(cn, new ARSO.Criteria((int)CodeKey, source_DK, _Doc.DocKey)))
                                {
                                    MsgBox.Show(cn,msgID);
                                    return GVar.gcCancel;
                                }        
                        
                                _DocDetItms.Clear();
                                _DocDetItms.Fetch_ARQODetItm(cn, new ARSODetItms.Criteria(source_DK, (int)CodeKey, _Doc.DocKey));
                                ////_Doc.Attachments.CopyWithDMAS(cn, source_DC, source_DK, (int)CodeKey,GFunc.NEInt(_Doc.DocKey,0));    /* Commented by KKAung on 22 Feb 2023,  Do not copy QO's attachments to SO */
                                //_Doc.Attachments = new SYSAttachments();

                                _Doc.Attachments.CopyWithDMAS(cn, source_DC, source_DK, (int)CodeKey, GFunc.NEInt(_Doc.DocKey, 0)); /* to copy attachments from QO to SO */

                                break;
                            case (int)GEnum.SystemCode.Reserve_Order:
                                if (!_Doc.Fetch_ARRO(cn, new ARSO.Criteria((int)CodeKey, source_DK, _Doc.DocKey)))
                                {
                                    MsgBox.Show(cn, msgID);
                                    return GVar.gcCancel;
                                }

                                _DocDetItms.Clear();
                                _DocDetItms.Fetch_ARRODetItm(cn, new ARSODetItms.Criteria(source_DK, (int)CodeKey, _Doc.DocKey));
                                ////_Doc.Attachments.CopyWithDMAS(cn,source_DC, source_DK, (int)CodeKey, GFunc.NEInt(_Doc.DocKey, 0));
                                //_Doc.Attachments = new SYSAttachments();

                                _Doc.Attachments.CopyWithDMAS(cn, source_DC, source_DK, (int)CodeKey, GFunc.NEInt(_Doc.DocKey, 0)); /* to copy attachments from RO to SO */

                                break;
                        }
                        #endregion

                        #region Set values in Doc
                        _Doc._DocState = 10;
                        _Doc._DocID = "";
                        _Doc._DocStatus = "";
                        _Doc._DocRemAdditional3 = ApprovalRequired ? "" : _Doc._DocRemAdditional3;/* added by YST on 2023/08/09 if approval required, this field is used for Request Remark that should not be copied from others */
                        _Doc._DocPrinted = false;
                        _Doc._ApproveUserKey = 0;
                        _Doc._ApproveDate = null;
                        _Doc._DisapproveUserKey = 0;
                        _Doc._DisapproveDate = null;
                        _Doc._DisapproveCount = 0;
                        _Doc._DisapproveMsg = string.Empty;
                        _Doc._CreateDate = null;
                        _Doc._CreateUserKey = 0;
                        _Doc._LastModifiedDate = null;
                        _Doc._LastModifiedUserKey = 0;                        
                        _Doc._PurgeKeep = 0;
                        _Doc._PurgeData = false;

                        if (_Doc._DocTypeNm != "eStore SO" && _Doc._DocTypeNm != "Direct Shipment" && _Doc._DocTypeNm != "Sales Order TI" && _Doc._DocTypeNm != "Cash Sales Order")  //updated by KKAung on 30 Aug 2021
                            DocUtility.CreateDocumentDocType_Set(cn, _Doc, (int)source_DC, (int)source_DK);
                        #endregion

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        #region Set Factory Flag
                        _Doc.GUID = _guID;
                        _Doc.IsNew = true;
                        _Doc.IsReadOnly = false;
                        _Doc.IsDirty = false;
                        restoreFlag = false;
                        #endregion
                    }
                }
                return GVar.gcPass;
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
                #region restore data to object
                if (restoreFlag == true)
                {
                    this._Doc = copyARSO;
                    this._DocDetItms = copyARSODetItms;
                }
                #endregion

                #region Set Null to Backup Objects
                copyARSO = null;
                copyARSODetItms = null;
                #endregion
            }
        }//Completed
        public bool Dispose()
        {
            try
            {
                if (SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, _codeKey, _guID, 0, 0))
                    return true;
                else
                    return false;
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

        //Set Default Values
        private bool Doc_SetDefaultValue(SqlConnection cn)
        {
            try
            {
                //Set default values in object
                _Doc._DocCodeKey = (int)_codeKey;
                _Doc._DocDate = DateTime.Today;
                _Doc._DocSign = 1;
                _Doc._DocDeptKey = 0;
                _Doc._DocTranGrpKey = 0;
                _Doc._DocGrpKey = 0;
                _Doc._DocSubTotal = 0;
                _Doc._DocOverallDisAcc = 0;
                _Doc._DocOverallDisRate = 0;
                _Doc._DocOverallDisAmt = 0;
                _Doc._DocTotalAfterDis = 0;
                _Doc._DocTaxGrpRate = 0;
                _Doc._DocTaxTotal = 0;
                _Doc._DocGrand = 0;
                _Doc._DocCurrKey = 1;
                _Doc._DocCurrRate = 1;
                _Doc._DocHome = 0;
                _Doc._DocCountryRate = 1;
                _Doc._DocTaxTotalLocal = 0;
                _Doc._DocCompleted = false;
                _Doc._DocState = 10;
                _Doc._DocPrinted = false;
                _Doc._ApproveUserKey = 0;
                _Doc._DisapproveUserKey = 0;
                _Doc._DisapproveCount = 0;
                _Doc._Attachment = false;
                _Doc._BranchKey = 0;
                _Doc._PurgeKeep = 0;
                _Doc._PurgeData = false;
                if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM) _Doc._DocReqDate = null;
                _Doc._DocRemDelivery = SysOptionUtility.GetStr("DefaultDocRemDelivery", cn);
                _Doc._DocRemPayment = SysOptionUtility.GetStr("DefaultDocRemPayment", cn);
                _Doc._DocRemPrice = SysOptionUtility.GetStr("DefaultDocRemPrice", cn);
                _Doc._DocRemValidity = SysOptionUtility.GetStr("DefaultDocRemValidity", cn);

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
        private bool DocDetItem_SetDefaultValue()
        {
            try
            {
                //Whole Column Default Value
                _DocDetItms.Columns["DocKey"].DefaultValue = _Doc.DocKey;
                _DocDetItms.Columns["DocItmKey"].DefaultValue = 0;
                _DocDetItms.Columns["LineType"].DefaultValue = 1000;
                _DocDetItms.Columns["LineLinkKey"].DefaultValue = 0;
                _DocDetItms.Columns["ItmSN"].DefaultValue = 0;
                _DocDetItms.Columns["ItmDeptKey"].DefaultValue = GFunc.NEInt(this._Doc.DocDeptKey, 0);
                _DocDetItms.Columns["ItmTranGrpKey"].DefaultValue = GFunc.NEInt(this._Doc.DocTranGrpKey, 0);
                _DocDetItms.Columns["ItmOrderStatus"].DefaultValue = 0;
                _DocDetItms.Columns["ItmAmtF"].DefaultValue = 0;
                _DocDetItms.Columns["ItmAmtH"].DefaultValue = 0;
                _DocDetItms.Columns["ItmTaxable"].DefaultValue = true;
                _DocDetItms.Columns["ItmTaxGrpRate"].DefaultValue = 0;
                _DocDetItms.Columns["ItmTaxGrpAmtF"].DefaultValue = 0;
                _DocDetItms.Columns["ItmTaxGrpAmtL"].DefaultValue = 0;
                _DocDetItms.Columns["ItmHide"].DefaultValue = false;
                //added by thettm on 24-oct-2017(start)
                _DocDetItms.Columns["Released_trigger"].DefaultValue = false;
                //added by thettm on 24-oct-2017(end)
                _DocDetItms.Columns["ItmVendorCurrKey"].DefaultValue = 1;
                _DocDetItms.Columns["ItmVendorCurrRate"].DefaultValue = 1;
                _DocDetItms.Columns["ItmVendorPrice"].DefaultValue = 0;
                _DocDetItms.Columns["ItmVendorPriceRatio"].DefaultValue = 0;
                _DocDetItms.Columns["ItmVendorPriceLock"].DefaultValue = false;
                _DocDetItms.Columns["ItmJobKey"].DefaultValue = 0;
                _DocDetItms.Columns["ItmJobPhaseKey"].DefaultValue = 0;
                _DocDetItms.Columns["ItmJobTaskKey"].DefaultValue = 0;
                _DocDetItms.Columns["ItmJobCostTypeKey"].DefaultValue = 0;
                _DocDetItms.Columns["ItmIGrpDItm"].DefaultValue = 0;
                _DocDetItms.Columns["ItmIGrpQtyLock"].DefaultValue = false;
                _DocDetItms.Columns["ItmIGrpToPrint"].DefaultValue = true;
                _DocDetItms.Columns["ItmIGrpQtySet"].DefaultValue = 0;
                _DocDetItms.Columns["ItmIGrpAmtSet"].DefaultValue = 0;
                _DocDetItms.Columns["ItmAttachment"].DefaultValue = false;
                _DocDetItms.Columns["ItmBatchKey"].DefaultValue = 0;
                _DocDetItms.Columns["ItmBatchQty"].DefaultValue = 0;
                DocDetItms.Columns["ItmLatestCostF"].DefaultValue = 0;
                DocDetItms.Columns["ItmLatestCostH"].DefaultValue = 0;
                _DocDetItms.Columns["NSLink"].DefaultValue = 0;
                _DocDetItms.Columns["ARQODK"].DefaultValue = 0;
                _DocDetItms.Columns["ARQODItm"].DefaultValue = 0;
                _DocDetItms.Columns["CreateDate"].DefaultValue = DateTime.Today;
                _DocDetItms.Columns["CreateUserKey"].DefaultValue = AppInfor.currentUserKey;
                _DocDetItms.Columns["LastModifiedDate"].DefaultValue = DateTime.Today;
                _DocDetItms.Columns["LastModifiedUserKey"].DefaultValue = AppInfor.currentUserKey;
                _DocDetItms.Columns["ItmDetSN"].DefaultValue = 0;

                _DocDetItms.DefaultView.Sort = "ItmSN ASC";
                _DocDetItms.Columns["DSQty"].DefaultValue = 0.0;
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

        //Validations
        public bool Doc_Validation(SqlConnection cn)
        {            
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
            string errorMsgID = string.Empty;

            try
            {
                #region Clear Error in UI
                if (!GFunc.IsNE(this.ErrorNotifierHeader_Clear))
                    this.ErrorNotifierHeader_Clear.Invoke(this, e);
                #endregion

                #region Validation
                //MsgBox Error
                if (this._Doc.DocTranGrpKey == null)
                    this._Doc.DocTranGrpKey = 0;
                
                BaseUtility.Validate(true, this._Doc.DocKey, "DocKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, 1, int.MaxValue, e, cn);
                BaseUtility.Validate(true, this._Doc.DocCodeKey, "DocCodeKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                BaseUtility.Validate(true, this._Doc.DocType, "DocType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, 1, int.MaxValue, e, cn) ;
                BaseUtility.Validate(true, this._Doc.DocSign, "DocSign", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, -1, -1, 1, e, cn);                     

                if (Doc.IsNew == false)
                    BaseUtility.Validate(false, this._Doc.DocID, "DocID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);

                if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM || SysOptionUtility.DatabaseBranchCode == DBCode.SOP || SysOptionUtility.DatabaseBranchCode == DBCode.ADL)
                    BaseUtility.Validate(false, this._Doc.DocEmKey, "DocEmkey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);

                BaseUtility.Validate(false, this._Doc.DocDate, "DocDate", GEnum.DataType.DateTime, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocReqDate, "DocReqDate", GEnum.DataType.DateTime, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocTypeNm, "DocTypeNm", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocConKey, "DocConKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocConNm, "DocConNm", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocConUEN, "DocConUEN", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocDeptKey, "DocDeptKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocTranGrpKey, "DocTranGrpKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocAccKey, "DocAccKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocGrpKey, "DocGrpKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                //BaseUtility.Validate(false, this._Doc.DocPriceType, "DocPriceType", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                //BaseUtility.Validate(false, this._Doc.DocTermKey, "DocTermKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                //BaseUtility.Validate(false, this._Doc.DocEmKey, "DocEmKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocBAddrStreet, "DocBAddrStreet", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocBAddrPOBox, "DocBAddrPOBox", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocBAddrCity, "DocBAddrCity", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocBAddrState, "DocBAddrState", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocBAddrZipCode, "DocBAddrZipCode", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocBAddrCountry, "DocBAddrCountry", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocBAddrRegion, "DocBAddrRegion", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocBAddrAttn, "DocBAddrAttn", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocBAddrTel1, "DocBAddrTel1", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocBAddrTel2, "DocBAddrTel2", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocBAddrFax, "DocBAddrFax", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocBAddrEmail, "DocBAddrEmail", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocSAddrStreet, "DocSAddrStreet", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocSAddrPOBox, "DocSAddrPOBox", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocSAddrCity, "DocSAddrCity", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocSAddrState, "DocSAddrState", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocSAddrZipCode, "DocSAddrZipCode", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocSAddrCountry, "DocSAddrCountry", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocSAddrRegion, "DocSAddrRegion", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocSAddrAttn, "DocSAddrAttn", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocSAddrTel1, "DocSAddrTel1", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocSAddrTel2, "DocSAddrTel2", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocSAddrFax, "DocSAddrFax", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocSAddrEmail, "DocSAddrEmail", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocShipName, "DocShipName", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocShipMark, "DocShipMark", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                //BaseUtility.Validate(false, this._Doc.DocShipKey, "DocShipKey", GEnum.DataType.Integer, GEnum.Require.No, null, null,null, null, null, e, cn);
                //BaseUtility.Validate(false, this._Doc.DocShipDate, "DocShipDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocCustPONum, "DocCustPONum", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocQONum, "DocQONum", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocSONum, "DocSONum", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocDONum, "DocDONum", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocIVNum, "DocIVNum", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocPONum, "DocPONum", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocPDNum, "DocPDNum", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocBLNum, "DocBLNum", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocRef, "DocRef", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocDes, "DocDes", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocRem, "DocRem", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocRemDelivery, "DocRemDelivery", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocRemPrice, "DocRemPrice", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocRemValidity, "DocRemValidity", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocRemPayment, "DocRemPayment", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocPermitNum, "DocPermitNum", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocGoodsDestination, "DocGoodsDestination", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocCountryOrigin, "DocCountryOrigin", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocRemAdditional1, "DocRemAdditional1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocRemAdditional2, "DocRemAdditional2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocRemAdditional3, "DocRemAdditional3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocRemAdditional4, "DocRemAdditional4", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocSubTotal, "DocSubTotal", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                //BaseUtility.Validate(false, this._Doc.DocOverallDisAcc, "DocOverallDisAcc", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocOverallDisRate, "DocOverallDisRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocOverallDisAmt, "DocOverallDisAmt", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocTotalAfterDis, "DocTotalAfterDis", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                //BaseUtility.Validate(false, this._Doc.DocTaxGrpKey, "DocTaxGrpKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocTaxGrpRate, "DocTaxGrpRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocTaxTotal, "DocTaxTotal", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocGrand, "DocGrand", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocCurrKey, "DocCurrKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocCurrRate, "DocCurrRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocHome, "DocHome", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocCountryRate, "DocCountryRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocTaxTotalLocal, "DocTaxTotalLocal", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocCompleted, "DocCompleted", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocStatus, "DocStatus", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocState, "DocState", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocPrinted, "DocPrinted", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.ApproveUserKey, "ApproveUserKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                //BaseUtility.Validate(false, this._Doc.ApproveDate, "ApproveDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DisapproveUserKey, "DisapproveUserKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                //BaseUtility.Validate(false, this._Doc.DisapproveDate, "DisapproveDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DisapproveCount, "DisapproveCount", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DisapproveMsg, "DisapproveMsg", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.Attachment, "Attachment", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.BranchKey, "BranchKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.Custom4, "Custom4", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.Custom5, "Custom5", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                #endregion

                #region Invoke Notifier
                if (e.PropertyMessage.Count > 0)
                {
                    if (!GFunc.IsNE(this.ErrorNotifierHeader_Set))
                        this.ErrorNotifierHeader_Set.Invoke(this, e);

                    return false;
                }
                else
                    return true;
                #endregion
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
        public bool DocDetItm_Validation(DataRow CheckRow)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocDetItm_Validation(cn,CheckRow);
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
        public bool DocDetItm_Validation(SqlConnection cn)
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
                foreach (DataRow dr in this._DocDetItms.Rows)
                {
                    if (GFunc.NEInt(dr["LineType"], (int)GEnum.RecDetailType.DItems) == (int)GEnum.RecDetailType.DItmAssembly)//Assembly Child is no need to check Validation.
                        continue;

                    #region Common Validation
                    BaseUtility.Validate(false, dr["DocKey"], "DocKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["DocItmKey"], "DocItmKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["LineType"], "LineType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["LineLinkKey"], "LineLinkKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmSN"], "ItmSN", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);

                    if (GFunc.NEInt(dr["ItmType"], 0) != (int)GEnum.ItemType.Remark)//Important
                    {
                        BaseUtility.Validate(false, dr["ItmKey"], "ItmKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmKeySelect"], "ItmKeySelect", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    }
                    else
                    {
                        BaseUtility.Validate(false, dr["ItmKey"], "ItmKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmKeySelect"], "ItmKeySelect", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    } 
                    
                    BaseUtility.Validate(false, dr["ItmType"], "ItmType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmDes"], "ItmDes", GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmDeptKey"], "ItmDeptKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmTranGrpKey"], "ItmTranGrpKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmStock"], "ItmStock", GEnum.DataType.Decimel, GEnum.Require.No, null, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmQtyLink"], "ItmQtyLink", GEnum.DataType.Decimel, GEnum.Require.No, null, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmQtyAdj"], "ItmQtyAdj", GEnum.DataType.Decimel, GEnum.Require.No, null, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmOrderStatus"], "ItmOrderStatus", GEnum.DataType.Integer, GEnum.Require.No, null, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmHide"], "ItmHide", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);

                    //added by thettm on 30-oct-2017(start)
                    BaseUtility.Validate(false, dr["Released_trigger"], "Released_trigger", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    //added by thettm on 30-oct-2017(end)

                    //BaseUtility.Validate(false, dr["ItmColorKey"], "ItmColorKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmScaleSize"], "ItmScaleSize", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmPacking"], "ItmPacking", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmRem"], "ItmRem", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmRef"], "ItmRef", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmMark"], "ItmMark", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);            
                    BaseUtility.Validate(false, dr["ItmAttachment"], "ItmAttachment", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);               
                    BaseUtility.Validate(false, dr["NSLink"], "NSLink", GEnum.DataType.String, GEnum.Require.Yes, 50, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ARQOID"], "ARQOID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ARQODK"], "ARQODK", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ARQODItm"], "ARQODItm", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                   
                    BaseUtility.Validate(false, dr["Custom1"], "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["Custom2"], "Custom2", GEnum.DataType.String, GEnum.Require.No, 1000, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["Custom3"], "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmDetSN"], "ItmDetSN", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    #endregion

                    #region ItmType Validation
                    switch (GFunc.NEInt(dr["ItmType"], 0))
                    {
                        
                        case (int)GEnum.ItemType.Consignment:
                        case (int)GEnum.ItemType.Finished_GD:
                        case (int)GEnum.ItemType.Finished_GDB:
                        case (int)GEnum.ItemType.Serial_Finished_GDB:
                        case (int)GEnum.ItemType.Serial_StockB:
                        case (int)GEnum.ItemType.Stock:
                        case (int)GEnum.ItemType.StockB:
                            //case (int)GEnum.ItemType.Substitute: not use -- comment by pauk
                            BaseUtility.Validate(false, dr["ItmLocKey"], "ItmLocKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmReqDate"], "ItmReqDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmPrmDate"], "ItmPrmDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);

                            if (GFunc.NEInt(dr["LineType"],0) != 1100)
                            {
                                BaseUtility.Validate(false, dr["ItmAccKey"], "ItmAccKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                            }
                            BaseUtility.Validate(false, dr["ItmStock"], "ItmStock", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmQty"], "ItmQty", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmUOMKey"], "ItmUOMKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmConRate"], "ItmConRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmLatestCostF"], "ItmLatestCostF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmLatestCostH"], "ItmLatestCostH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmListPrice"], "ItmListPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmPriceBefore"], "ItmPriceBefore", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmPriceAfter"], "ItmPriceAfter", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmDisPercent"], "ItmDisPercent", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmDisValue"], "ItmDisValue", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmPrice"], "ItmPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmPriceUser"], "ItmPriceUser", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmControlPrice"], "ItmControlPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmControlPriceBase"], "ItmControlPriceBase", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmAmtShw"], "ItmAmtShw", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmAmtF"], "ItmAmtF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmAmtH"], "ItmAmtH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmTaxable"], "ItmTaxable", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            //BaseUtility.Validate(false, dr["ItmTaxGrpKey"], "ItmTaxGrpKey", GEnum.DataType.Integer, GEnum.Require.No, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmTaxGrpRate"], "ItmTaxGrpRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmTaxGrpAmtF"], "ItmTaxGrpAmtF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmTaxGrpAmtL"], "ItmTaxGrpAmtL", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmVendorKey"], "ItmVendorKey", GEnum.DataType.Integer, GEnum.Require.No, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmVendorCurrKey"], "ItmVendorCurrKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmVendorCurrRate"], "ItmVendorCurrRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmVendorPrice"], "ItmVendorPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmVendorPriceRatio"], "ItmVendorPriceRatio", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmVendorPriceLock"], "ItmVendorPriceLock", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmJobKey"], "ItmJobKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, -1, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmJobPhaseKey"], "ItmJobPhaseKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmJobTaskKey"], "ItmJobTaskKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmJobCostTypeKey"], "ItmJobCostTypeKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmIGrpDItm"], "ItmIGrpDItm", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmIGrpQtyLock"], "ItmIGrpQtyLock", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmIGrpToPrint"], "ItmIGrpToPrint", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmIGrpQtySet"], "ItmIGrpQtySet", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmIGrpAmtSet"], "ItmIGrpAmtSet", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmBatchKey"], "ItmBatchKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmBatchQty"], "ItmBatchQty", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);

                            break;

                        case (int)GEnum.ItemType.Assembly:
                        case (int)GEnum.ItemType.Non_Stock:
                        case (int)GEnum.ItemType.Service:
                        case (int)GEnum.ItemType.Charges:
                            BaseUtility.Validate(false, dr["ItmReqDate"], "ItmReqDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmPrmDate"], "ItmPrmDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmAccKey"], "ItmAccKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);

                            if (GFunc.NEInt(dr["ItmType"],0) != (int)GEnum.ItemType.Charges)
                            {
                                BaseUtility.Validate(false, dr["ItmQty"], "ItmQty", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                                BaseUtility.Validate(false, dr["ItmUOMKey"], "ItmUOMKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                                BaseUtility.Validate(false, dr["ItmConRate"], "ItmConRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                            }
                            BaseUtility.Validate(false, dr["ItmLatestCostF"], "ItmLatestCostF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmLatestCostH"], "ItmLatestCostH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmListPrice"], "ItmListPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmPriceBefore"], "ItmPriceBefore", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmPriceAfter"], "ItmPriceAfter", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmDisPercent"], "ItmDisPercent", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmDisValue"], "ItmDisValue", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmPrice"], "ItmPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmPriceUser"], "ItmPriceUser", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmControlPrice"], "ItmControlPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmControlPriceBase"], "ItmControlPriceBase", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmAmtShw"], "ItmAmtShw", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmAmtF"], "ItmAmtF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmAmtH"], "ItmAmtH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmTaxable"], "ItmTaxable", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            //BaseUtility.Validate(false, dr["ItmTaxGrpKey"], "ItmTaxGrpKey", GEnum.DataType.Integer, GEnum.Require.No, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmTaxGrpRate"], "ItmTaxGrpRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmTaxGrpAmtF"], "ItmTaxGrpAmtF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmTaxGrpAmtL"], "ItmTaxGrpAmtL", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmVendorKey"], "ItmVendorKey", GEnum.DataType.Integer, GEnum.Require.No, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmVendorCurrKey"], "ItmVendorCurrKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmVendorCurrRate"], "ItmVendorCurrRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmVendorPrice"], "ItmVendorPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmVendorPriceRatio"], "ItmVendorPriceRatio", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmVendorPriceLock"], "ItmVendorPriceLock", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmJobKey"], "ItmJobKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, -1, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmJobPhaseKey"], "ItmJobPhaseKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmJobTaskKey"], "ItmJobTaskKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmJobCostTypeKey"], "ItmJobCostTypeKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmIGrpDItm"], "ItmIGrpDItm", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmIGrpQtyLock"], "ItmIGrpQtyLock", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmIGrpToPrint"], "ItmIGrpToPrint", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmIGrpQtySet"], "ItmIGrpQtySet", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmIGrpAmtSet"], "ItmIGrpAmtSet", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);

                            break;

                        case (int)GEnum.ItemType.Discount:
                            BaseUtility.Validate(false, dr["ItmAccKey"], "ItmAccKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmQty"], "ItmQty", GEnum.DataType.Decimel, GEnum.Require.No, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmAmtShw"], "ItmAmtShw", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmAmtF"], "ItmAmtF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmAmtH"], "ItmAmtH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmTaxable"], "ItmTaxable", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            //BaseUtility.Validate(false, dr["ItmTaxGrpKey"], "ItmTaxGrpKey", GEnum.DataType.Integer, GEnum.Require.No, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmTaxGrpRate"], "ItmTaxGrpRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmTaxGrpAmtF"], "ItmTaxGrpAmtF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmTaxGrpAmtL"], "ItmTaxGrpAmtL", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmJobKey"], "ItmJobKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, -1, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmJobPhaseKey"], "ItmJobPhaseKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmJobTaskKey"], "ItmJobTaskKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmJobCostTypeKey"], "ItmJobCostTypeKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                            break;

                        default:
                            //case (int)GEnum.ItemType.Master:
                            //case (int)GEnum.ItemType.Header:
                            //case (int)GEnum.ItemType.Remark:
                            //case (int)GEnum.ItemType.Sub_Total:
                            //case (int)GEnum.ItemType.BF_Total:
                            BaseUtility.Validate(false, dr["ItmAmtShw"], "ItmAmtShw", GEnum.DataType.Decimel, GEnum.Require.No, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmAmtF"], "ItmAmtF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmAmtH"], "ItmAmtH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            break;
                    }
                    #endregion

                    #region Additional Validation check
                    if (e.PropertyMessage.Count == 0)              
                    {
                        // Check for duplicate DocKey + DocItmKey  
                        KeyCount = 0;
                        KeyCount = _DocDetItms.AsEnumerable().Count(p => p.Field<int>("DocItmKey") == GFunc.NEInt(dr["DocItmKey"],0));

                        if (KeyCount > 1)
                        {                        
                            e.PropertyMessage.Add("DocItmKey", SysMessageUtility.Get(cn, "DocItmKey" + MsgID.Validation.DuplicateRecord));
                        }

                        // Check for duplicate ItmSN  
                        KeyCount = 0;
                        KeyCount = _DocDetItms.AsEnumerable().Count(p => p.Field<decimal>("ItmSN") == GFunc.NEDec(dr["ItmSN"],0) && p.Field<int>("LineType") == 1000);

                        if (KeyCount > 1)
                        {                       
                            e.PropertyMessage.Add("ItmSN", SysMessageUtility.Get(cn, "ItmSN" + MsgID.Validation.DuplicateRecord));
                        }

                        // Check for duplicate ItmDetSN  
                        KeyCount = 0;
                        KeyCount = _DocDetItms.AsEnumerable().Count(p => (p.Field<decimal>("ItmDetSN") == GFunc.NEDec(dr["ItmDetSN"],0)) && (p.Field<int>("LineLinkKey") == GFunc.NEInt(dr["LineLinkKey"],0)) && ((p.Field<decimal>("ItmDetSN") == 0) && (p.Field<int>("LineLinkKey") == 0)) == false);
                        if (KeyCount > 1)
                        {                       
                            e.PropertyMessage.Add("ItmDetSN", SysMessageUtility.Get(cn, "ItmSN" + MsgID.Validation.DuplicateRecord));
                        }
                    }
                    #endregion

                    #region Assign error message to display in grid
                    if (e.PropertyMessage.Count > 0)
                    {
                        foreach (object key in e.PropertyMessage.Keys)
                        {
                            if (!GFunc.IsNE(msgValue))
                                msgValue += " and ";

                            msgValue += e.PropertyMessage[key];

                        }
                        GFunc.SetRowError(dr, msgValue);
                        isValidate = false;
                        msgValue = "";
                        e.PropertyMessage.Clear();
                    }
                    else
                    {
                        dr.RowError = string.Empty;                    
                    }
                    #endregion                
                }

                #region Price check // Added by May on 25-Jun-2024   
                if (e.PropertyMessage.Count == 0 && SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                {
                    string SNlist = "";

                    IEnumerable<DataRow> dtItemsFilter = _DocDetItms.AsEnumerable().Where(p => (p.Field<decimal?>("ItmPriceAfter") > p.Field<decimal?>("ItmControlPrice")
                    && p.Field<decimal?>("ItmControlPrice") != 0 && p.Field<decimal?>("ItmControlPrice") != -999));

                    foreach (DataRow r in dtItemsFilter)
                    {
                        SNlist += GFunc.NEDec(r.Field<decimal?>("ItmSN"), 0).ToString("###") + ", ";
                    }
                    if (SNlist.Length > 1)
                        SNlist = SNlist.Remove(SNlist.Length - 2);
                    if (dtItemsFilter.Count() > 1)
                        MsgBox.Show("Warning!!! The prices of Items SN (" + SNlist + ") are higher than EStore prices.", GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                    else if (dtItemsFilter.Count() > 0)
                        MsgBox.Show("Warning!!! The price of Item SN " + SNlist + " is higher than EStore price.", GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);

                }
                #endregion
                return isValidate;
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
        public bool DocDetItm_Validation(SqlConnection cn,DataRow dr)
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
                if (dr != null)
                {
                    if (DocUtility.HiddenValueCurrentRow_Set(cn, Doc, (int)GEnum.Details.Doc_Itm, dr) == false)
                        return false;
                }

                if (GFunc.NEInt(dr["LineType"], (int)GEnum.RecDetailType.DItems) == (int)GEnum.RecDetailType.DItmAssembly)//Assembly Child is no need to check Validation.
                    return true;

                #region Common Validation
                BaseUtility.Validate(false, dr["DocKey"], "DocKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                BaseUtility.Validate(false, dr["DocItmKey"], "DocItmKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                BaseUtility.Validate(false, dr["LineType"], "LineType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                BaseUtility.Validate(false, dr["LineLinkKey"], "LineLinkKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, dr["ItmSN"], "ItmSN", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                BaseUtility.Validate(false, dr["ItmType"], "ItmType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);

                if (GFunc.NEInt(dr["ItmType"], 0) != (int)GEnum.ItemType.Remark)
                {
                    BaseUtility.Validate(false, dr["ItmKey"], "ItmKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmKeySelect"], "ItmKeySelect", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                }
                else
                {
                    BaseUtility.Validate(false, dr["ItmKey"], "ItmKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmKeySelect"], "ItmKeySelect", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                }
                
                BaseUtility.Validate(false, dr["ItmDes"], "ItmDes", GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, e, cn);
                BaseUtility.Validate(false, dr["ItmDeptKey"], "ItmDeptKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, dr["ItmTranGrpKey"], "ItmTranGrpKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                //BaseUtility.Validate(false, dr["ItmStock"], "ItmStock", GEnum.DataType.Decimel, GEnum.Require.No, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, dr["ItmQtyLink"], "ItmQtyLink", GEnum.DataType.Decimel, GEnum.Require.No, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, dr["ItmQtyAdj"], "ItmQtyAdj", GEnum.DataType.Decimel, GEnum.Require.No, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, dr["ItmOrderStatus"], "ItmOrderStatus", GEnum.DataType.Integer, GEnum.Require.No, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, dr["ItmHide"], "ItmHide", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);

                //added by thettm on 30-oct-2016(start)
                BaseUtility.Validate(false, dr["Released_trigger"], "Released_trigger", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                //added by thettm on 30-oct-2016(end)

                //BaseUtility.Validate(false, dr["ItmColorKey"], "ItmColorKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, dr["ItmScaleSize"], "ItmScaleSize", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, dr["ItmPacking"], "ItmPacking", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, dr["ItmRem"], "ItmRem", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, dr["ItmRef"], "ItmRef", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, dr["ItmMark"], "ItmMark", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, dr["ItmAttachment"], "ItmAttachment", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, dr["NSLink"], "NSLink", GEnum.DataType.String, GEnum.Require.Yes, 50, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, dr["ARQOID"], "ARQOID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, dr["ARQODK"], "ARQODK", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, dr["ARQODItm"], "ARQODItm", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);            
                BaseUtility.Validate(false, dr["Custom1"], "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, dr["Custom2"], "Custom2", GEnum.DataType.String, GEnum.Require.No, 1000, null, null, null, null, e, cn);
                BaseUtility.Validate(false, dr["Custom3"], "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, dr["ItmDetSN"], "ItmDetSN", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                #endregion

                #region ItmType Validation
                switch (GFunc.NEInt(dr["ItmType"],0))
                {

                    case (int)GEnum.ItemType.Consignment:
                    case (int)GEnum.ItemType.Finished_GD:
                    case (int)GEnum.ItemType.Finished_GDB:
                    case (int)GEnum.ItemType.Serial_Finished_GDB:
                    case (int)GEnum.ItemType.Serial_StockB:
                    case (int)GEnum.ItemType.Stock:
                    case (int)GEnum.ItemType.StockB:
                        BaseUtility.Validate(false, dr["ItmLocKey"], "ItmLocKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmReqDate"], "ItmReqDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmPrmDate"], "ItmPrmDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);

                        if (GFunc.NEInt(dr["LineType"],0) != 1100)
                        {
                            BaseUtility.Validate(false, dr["ItmAccKey"], "ItmAccKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                        }

                        //BaseUtility.Validate(false, dr["ItmStock"], "ItmStock", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmQty"], "ItmQty", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmUOMKey"], "ItmUOMKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmConRate"], "ItmConRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmLatestCostF"], "ItmLatestCostF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmLatestCostH"], "ItmLatestCostH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmListPrice"], "ItmListPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmPriceBefore"], "ItmPriceBefore", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmPriceAfter"], "ItmPriceAfter", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmDisPercent"], "ItmDisPercent", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmDisValue"], "ItmDisValue", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmPrice"], "ItmPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmPriceUser"], "ItmPriceUser", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmControlPrice"], "ItmControlPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmControlPriceBase"], "ItmControlPriceBase", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmAmtShw"], "ItmAmtShw", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmAmtF"], "ItmAmtF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmAmtH"], "ItmAmtH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmTaxable"], "ItmTaxable", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        //BaseUtility.Validate(false, dr["ItmTaxGrpKey"], "ItmTaxGrpKey", GEnum.DataType.Integer, GEnum.Require.No, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmTaxGrpRate"], "ItmTaxGrpRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmTaxGrpAmtF"], "ItmTaxGrpAmtF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmTaxGrpAmtL"], "ItmTaxGrpAmtL", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmVendorKey"], "ItmVendorKey", GEnum.DataType.Integer, GEnum.Require.No, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmVendorCurrKey"], "ItmVendorCurrKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmVendorCurrRate"], "ItmVendorCurrRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmVendorPrice"], "ItmVendorPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmVendorPriceRatio"], "ItmVendorPriceRatio", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmVendorPriceLock"], "ItmVendorPriceLock", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmJobKey"], "ItmJobKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, -1, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmJobPhaseKey"], "ItmJobPhaseKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmJobTaskKey"], "ItmJobTaskKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmJobCostTypeKey"], "ItmJobCostTypeKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmIGrpDItm"], "ItmIGrpDItm", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmIGrpQtyLock"], "ItmIGrpQtyLock", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmIGrpToPrint"], "ItmIGrpToPrint", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmIGrpQtySet"], "ItmIGrpQtySet", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmIGrpAmtSet"], "ItmIGrpAmtSet", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmBatchKey"], "ItmBatchKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmBatchQty"], "ItmBatchQty", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        break;

                    case (int)GEnum.ItemType.Assembly:
                    case (int)GEnum.ItemType.Non_Stock:
                    case (int)GEnum.ItemType.Service:
                    case (int)GEnum.ItemType.Charges:
                        BaseUtility.Validate(false, dr["ItmReqDate"], "ItmReqDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmPrmDate"], "ItmPrmDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmAccKey"], "ItmAccKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                        
                        if (GFunc.NEInt(dr["ItmType"],0) != (int)GEnum.ItemType.Charges)
                        {
                            BaseUtility.Validate(false, dr["ItmQty"], "ItmQty", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmUOMKey"], "ItmUOMKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmConRate"], "ItmConRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                        }
                        BaseUtility.Validate(false, dr["ItmLatestCostF"], "ItmLatestCostF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmLatestCostH"], "ItmLatestCostH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmListPrice"], "ItmListPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmPriceBefore"], "ItmPriceBefore", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmPriceAfter"], "ItmPriceAfter", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmDisPercent"], "ItmDisPercent", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmDisValue"], "ItmDisValue", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmPrice"], "ItmPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmPriceUser"], "ItmPriceUser", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmControlPrice"], "ItmControlPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmControlPriceBase"], "ItmControlPriceBase", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmAmtShw"], "ItmAmtShw", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmAmtF"], "ItmAmtF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmAmtH"], "ItmAmtH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmTaxable"], "ItmTaxable", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        //BaseUtility.Validate(false, dr["ItmTaxGrpKey"], "ItmTaxGrpKey", GEnum.DataType.Integer, GEnum.Require.No, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmTaxGrpRate"], "ItmTaxGrpRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmTaxGrpAmtF"], "ItmTaxGrpAmtF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmTaxGrpAmtL"], "ItmTaxGrpAmtL", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmVendorKey"], "ItmVendorKey", GEnum.DataType.Integer, GEnum.Require.No, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmVendorCurrKey"], "ItmVendorCurrKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmVendorCurrRate"], "ItmVendorCurrRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmVendorPrice"], "ItmVendorPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmVendorPriceRatio"], "ItmVendorPriceRatio", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmVendorPriceLock"], "ItmVendorPriceLock", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmJobKey"], "ItmJobKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, -1, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmJobPhaseKey"], "ItmJobPhaseKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmJobTaskKey"], "ItmJobTaskKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmJobCostTypeKey"], "ItmJobCostTypeKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmIGrpDItm"], "ItmIGrpDItm", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmIGrpQtyLock"], "ItmIGrpQtyLock", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmIGrpToPrint"], "ItmIGrpToPrint", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmIGrpQtySet"], "ItmIGrpQtySet", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmIGrpAmtSet"], "ItmIGrpAmtSet", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);

                        break;

                    case (int)GEnum.ItemType.Discount:
                        BaseUtility.Validate(false, dr["ItmAccKey"], "ItmAccKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmQty"], "ItmQty", GEnum.DataType.Decimel, GEnum.Require.No, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmAmtShw"], "ItmAmtShw", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmAmtF"], "ItmAmtF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmAmtH"], "ItmAmtH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmTaxable"], "ItmTaxable", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        //BaseUtility.Validate(false, dr["ItmTaxGrpKey"], "ItmTaxGrpKey", GEnum.DataType.Integer, GEnum.Require.No, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmTaxGrpRate"], "ItmTaxGrpRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmTaxGrpAmtF"], "ItmTaxGrpAmtF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmTaxGrpAmtL"], "ItmTaxGrpAmtL", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmJobKey"], "ItmJobKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, -1, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmJobPhaseKey"], "ItmJobPhaseKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmJobTaskKey"], "ItmJobTaskKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmJobCostTypeKey"], "ItmJobCostTypeKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                        break;

                    default:
                        //case (int)GEnum.ItemType.Master:
                        //case (int)GEnum.ItemType.Header:
                        //case (int)GEnum.ItemType.Remark:
                        //case (int)GEnum.ItemType.Sub_Total:
                        //case (int)GEnum.ItemType.BF_Total:
                        BaseUtility.Validate(false, dr["ItmAmtShw"], "ItmAmtShw", GEnum.DataType.Decimel, GEnum.Require.No, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmAmtF"], "ItmAmtF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        BaseUtility.Validate(false, dr["ItmAmtH"], "ItmAmtH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                        break;
                }
                #endregion

                #region Additional Validation check
                if (e.PropertyMessage.Count== 0)          
                {
                    // Check for duplicate DocKey + DocItmKey  
                    KeyCount = 0;
                    KeyCount = _DocDetItms.AsEnumerable().Count(p => p.Field<int>("DocItmKey") == GFunc.NEInt(dr["DocItmKey"],0));

                    if (KeyCount > 1)
                    {                   
                        e.PropertyMessage.Add("DocItmKey", SysMessageUtility.Get(cn, "DocItmKey" + MsgID.Validation.DuplicateRecord));
                    }

                    // Check for duplicate ItmSN  
                    KeyCount = 0;
                    KeyCount = _DocDetItms.AsEnumerable().Count(p => p.Field<decimal>("ItmSN") == GFunc.NEDec(dr["ItmSN"],0) && p.Field<int>("LineType") == 1000);

                    if (KeyCount > 1)
                    {                  
                        e.PropertyMessage.Add("ItmSN", SysMessageUtility.Get(cn, "ItmSN" + MsgID.Validation.DuplicateRecord));
                    }

                    // Check for duplicate ItmDetSN  
                    KeyCount = 0;
                    KeyCount = _DocDetItms.AsEnumerable().Count(p => (p.Field<decimal>("ItmDetSN") == GFunc.NEDec(dr["ItmDetSN"], 0)) && (p.Field<int>("LineLinkKey") == GFunc.NEInt(dr["LineLinkKey"], 0)) && ((p.Field<decimal>("ItmDetSN") == 0) && (p.Field<int>("LineLinkKey") == 0)) == false);
                    if (KeyCount > 1)
                    {                  
                        e.PropertyMessage.Add("ItmDetSN", SysMessageUtility.Get(cn, "ItmSN" + MsgID.Validation.DuplicateRecord));
                    }

                    // Check for DSQty & Total Qty Relation -- added by yst on 30 dec 2018
                    if (GFunc.NEInt(dr["ItmType"], 0) == (int)GEnum.ItemType.Stock ||
                        GFunc.NEInt(dr["ItmType"], 0) == (int)GEnum.ItemType.Non_Stock ||
                        GFunc.NEInt(dr["ItmType"], 0) == (int)GEnum.ItemType.Assembly
                       )
                    {
                        KeyCount = 0;
                        KeyCount = Convert.ToDecimal(dr["DSQty"].ToString()) - Convert.ToDecimal(dr["ItmQty"].ToString()) > 0 ? 1 : 0;
                        if (KeyCount > 0)
                        {
                            e.PropertyMessage.Add("DSQty", SysMessageUtility.Get(cn, "DSQty must be less than or equal Total Qty."));
                        }
                    }


                    // Check for GST Reverse DO  
                    KeyCount = 0;
                    string ItmID = GFunc.NEStr(dr["ItmID"], "");
                    string DoDocID = GFunc.NEStr(dr["ItmDes"], "");

                    if (ItmID == "GST Reverse DO")
                    {

                        if (DoDocID != string.Empty)
                            KeyCount = GFunc.DocKey_Get((int)GEnum.SystemCode.Delivery_Order, DoDocID.Trim());

                        if (KeyCount == 0)
                        {
                            e.PropertyMessage.Add("", SysMessageUtility.Get(cn, "Invalid DO Num ( " + DoDocID + " ) in Description."));
                        }
                    }

                }
                #endregion

                #region Assign error message to display in grid
                if (e.PropertyMessage.Count > 0)
                {
                    foreach (object key in e.PropertyMessage.Keys)
                    {
                        if (!GFunc.IsNE(msgValue))
                            msgValue += " and ";

                        msgValue += e.PropertyMessage[key];

                    }
                    GFunc.SetRowError(dr, msgValue);
                    isValidate = false;
                    msgValue = "";
                    e.PropertyMessage.Clear();
                }
                else
                {
                    dr.RowError = string.Empty;               
                }
                #endregion

                return isValidate;
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

        //Dirty Handle Events
        void Obj_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _Doc.IsDirty = true;
        }//Completed
        void Attachments_ListChanged(object sender, ListChangedEventArgs e)
        {
            _Doc.IsDirty = true;
        }//Completed
       
        //Error Exceptions
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _Doc, _DocDetItms }, _codeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _Doc, _DocDetItms }, _codeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
    }
}
