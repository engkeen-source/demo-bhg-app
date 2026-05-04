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
    public class ARQOFactory:DocFactory
    {
        #region Member variables and constants
        private ARQO _Doc = null;
        private ARQOVendors _DocDetVendors = null;
        private ARQODetItms _DocDetItms = null;
        private ARQODetItmVendors _DocDetItmVendors = null;

        Hashtable htDetails = new Hashtable();

        #endregion

        #region Custom Event Declaration
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;
        #endregion

        #region Factory Properties
        public ARQO Doc
        {
            get
            {
                return this._Doc;
            }
        }
        public ARQODetItms DocDetItms
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
        public ARQODetItmVendors DocDetItmVendors
        {
            get
            {
                return this._DocDetItmVendors;
            }
        }
        public ARQOVendors DocDetVendors
        {
            get
            {
                return this._DocDetVendors;
            }
        }
        
        #endregion

        //Constructors, Initialisation
        public ARQOFactory(GEnum.InstanceMode instanceMode, GEnum.SystemCode DocCodeKey)
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

        //added by thettm on 29 jan 2018(start)
        public string GetCopy_ByDC(int? source_DC, int? source_DK)
        {
            #region Declaration
            bool restoreFlag = true;
            ARQO copyARQO = null;
            ARQODetItms copyARQODetItms = null;            
            string msgID = MsgID.Common.GetFail;
            #endregion

            try
            {
                #region Copy original object or Attach Eventhandler
                if (!GFunc.IsNE(this._Doc))
                    copyARQO = this._Doc.Clone();

                if (this._DocDetItms != null)
                    copyARQODetItms = GFunc.TACopyDataTable(_DocDetItms);
                #endregion

                #region Check Permission
                if (SECPermUtility.Edit(PermID, true) == false)
                    return GVar.gcCancel;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        #region Check, remove and add lock
                        // Check Lock
                        if (source_DC == (int)GEnum.SystemCode.Sales_Order)
                        {
                            if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, (GEnum.SystemCode)source_DC, source_DK, 0, _guID))
                                return GVar.gcCancel;
                        }

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, _guID, (GEnum.SystemCode)CodeKey))
                            return GVar.gcCancel;

                        // Add Lock
                        if (source_DC == (int)GEnum.SystemCode.Sales_Order)
                        {
                            if (!SysLockUtility.AddLock(cn, true, _guID, (GEnum.SystemCode)source_DC, source_DK))
                                return GVar.gcCancel;
                        }
                        #endregion

                        #region Fetch Data into Object
                        switch (source_DC)
                        {
                             case (int)GEnum.SystemCode.Sales_Order:
                                if (!_Doc.Fetch_ARSO(cn, new ARQO.Criteria((int)CodeKey, source_DK, _Doc.DocKey, 1)))
                                {
                                    MsgBox.Show(cn, msgID);
                                    return GVar.gcCancel;
                                }
                                _DocDetItms.Clear();
                                _DocDetItms.Fetch_ARSODetItm(cn, new ARQODetItms.Criteria(source_DK, (int)CodeKey, _Doc.DocKey));
                                _Doc.Attachments.CopyWithDMAS(cn, source_DC, source_DK, (int)CodeKey, GFunc.NEInt(_Doc.DocKey, 0));
                                break;
                        }
                        #endregion

                        #region Set values in Doc
                        _Doc._DocState = 10;
                        //_Doc._DocID = "";
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

                        //DocUtility.CreateDocumentDocType_Set(cn, _Doc, (int)source_DC, (int)source_DK);
                        #endregion

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

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
                    this._Doc = copyARQO;
                    this._DocDetItms = copyARQODetItms;
                }
                #endregion

                #region Set Null to Backup Objects
                copyARQO = null;
                copyARQODetItms = null;
                #endregion
            }
        }// Completed added by thettm on 29 jan 2018(start)
        public bool Initialisation(GEnum.SystemCode DocCodeKey)
        {
            try
            {
                //Initialise Properties
                bool isReadonly = false;
                this._isError = false;
                _codeKey = DocCodeKey;
                _permID = GVar.PermissionID.Quotation;                
                _guID = 0;
                _isError = false;           
                approvalRequired = false;
                _reloadRefCombos = false;
                //We cannot use throw to indicate an initialisation failure because the calling form should only close the form and there
                //is no need to show message as all message is already display by the utility function
                //therefore we use isErorr = true to indicate failure to initialise factory
                //in this function the return of true or false actually is not use at this point of time
                //Function return - True/False - not used
                //IsError status - True/False - used to check if the initialised of factory has failed

                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                {
                    base.Initialisation(out isReadonly);
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

                            if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)
                                throw new Exception("Initialisation failed. Transaction has aborted.");
                            scope.Complete();
                        }
                    }
                }
                else//added by May. For QO import from EStore
                {
                    this._Doc = new ARQO();
                    this._Doc._GUID = _guID;
                    this._Doc._DocCodeKey = (int)_codeKey;
                    this._DocDetItms = new ARQODetItms();
                    this._DocDetItmVendors = new ARQODetItmVendors();
                    this._DocDetVendors = new ARQOVendors();

                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();
                        int newDocKey = SysOptionUtility.NewDocKey_Get(cn, _codeKey);
                        if (newDocKey == 0)
                            throw new Exception("New DocKey cannot be generated.");
                        else
                            _Doc._DocKey = newDocKey;
                       
                        Doc_SetDefaultValue(cn);
                        DocDetItem_SetDefaultValue();
                        DocDetItemVendor_SetDefaultValue();
                        DocDetVendor_SetDefaultValue();
                    }
                }
                //approvalRequired = SysOptionUtility.GetInt("DocApproveForARQO") > 10;/* commented by YST */
                approvalRequired = DocUtility.DocApprovalRequired_Get((int)_codeKey, ref approvalRequired); /* added by YST */

               
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
        public string New(UltraGrid grd)
        {
            #region Declaration
            ARQO copyARQO = null;
            ARQODetItms copyARQODetItms = null;
            ARQODetItmVendors copyARQODetItmVendorss = null;
            ARQOVendors copyARQOVendors = null;
            bool restoreFlag = false;
            int newDocKey = 0;
            bool isReadOnly = false;
            #endregion

            try
            {
                #region Copy original object or Attach Eventhandler to new object
                if (!GFunc.IsNE(_Doc))
                    copyARQO = _Doc.Clone();

                if (_DocDetItms != null)
                    copyARQODetItms = GFunc.TACopyDataTable(_DocDetItms);

                if (_DocDetItmVendors != null)
                    copyARQODetItmVendorss = GFunc.TACopyDataTable(_DocDetItmVendors);

                if (_DocDetVendors != null)
                    copyARQOVendors = GFunc.TACopyDataTable(_DocDetVendors); 
                #endregion

                //will only check when saving
                #region Check Security Permission
                //if (!SECPermUtility.Any(_permID, out isReadOnly, true))
                //    return GVar.gcCancel;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        #region Remove all locks by GUID except inprogress Locking
                        if(this._Doc!=null)//added by May to skip additional lock removing in the first opening state 
                            if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, _guID, _codeKey))
                                return GVar.gcCancel;
                        #endregion

                        #region prepare New instances
                        this._Doc = new ARQO();
                        this._Doc._GUID = _guID;
                        this._Doc._DocCodeKey = CodeKey;
                        this._DocDetItms = new ARQODetItms(cn);
                        this._DocDetItmVendors = new ARQODetItmVendors(cn);
                        this._DocDetVendors = new ARQOVendors(cn);
                        this._Doc.Attachments = new SYSAttachments();
                        #endregion

                        #region Set ObjDoc flags
                        _Doc._isReadOnly = isReadOnly;
                        _Doc._isDirty = false;
                        _Doc._isNew = true;
                        #endregion

                        #region Assign DocKey
                        newDocKey = SysOptionUtility.NewDocKey_Get(cn, _codeKey);
                        if (newDocKey == 0)
                            return GVar.gcCancel;
                        else
                            _Doc._DocKey = newDocKey;
                        #endregion

                        #region set default values to docObj and Details
                        Doc_SetDefaultValue(cn);
                        DocDetItem_SetDefaultValue();
                        DocDetItemVendor_SetDefaultValue();
                        DocDetVendor_SetDefaultValue();
                        #endregion

                        #region prepare htDetails
                        htDetails.Clear();
                        htDetails.Add(GEnum.Details.Doc_Itm, grd);
                        htDetails.Add(GEnum.Details.Doc_ItmVendor, _DocDetItmVendors);
                        htDetails.Add(GEnum.Details.Doc_Vendor, _DocDetVendors);
                        #endregion

                        //Prepare new document for data entry
                        if (!DocUtility.Document_New(cn, _Doc, htDetails))
                            return GVar.gcCancel;

                        #region Attached events to handle objects and dtTables
                        this._Doc.PropertyChanged += new PropertyChangedEventHandler(Obj_PropertyChanged);
                        this._Doc.Attachments.ListChanged += new ListChangedEventHandler(Attachments_ListChanged);
                        #endregion

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)
                            throw new Exception("New process failed. Transaction has aborted.");

                        scope.Complete();
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
                    this._Doc = copyARQO;
                    this._DocDetItms = copyARQODetItms;
                    this._DocDetItmVendors = copyARQODetItmVendorss;
                    this._DocDetVendors = copyARQOVendors;
                }
                #endregion

                #region Reset Backup Objects
                copyARQO = null;
                copyARQODetItms = null;
                copyARQODetItmVendorss = null;
                copyARQOVendors = null;
                #endregion
            }
        }//Completed
        public string GetEdit(int docKey, string docID)
        {
            #region Declaration
            ARQO copyARQO = null;
            ARQODetItms copyARQODetItms = null;
            ARQODetItmVendors copyARQODetItmVendorss = null;
            ARQOVendors copyARQOVendors = null;
            bool restoreFlag = false;
            #endregion

            try
            {
                #region Backup original object or Attach Eventhandler to new object
                if (this._Doc != null)
                    copyARQO = _Doc.Clone();

                if (!GFunc.IsNE(_DocDetItms))
                    copyARQODetItms = GFunc.TACopyDataTable(_DocDetItms);

                if (_DocDetVendors != null)
                    copyARQOVendors = GFunc.TACopyDataTable(_DocDetVendors);

                if (_DocDetItmVendors != null)
                    copyARQODetItmVendorss = GFunc.TACopyDataTable(_DocDetItmVendors); 
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

                //using (TransactionScope scope = new TransactionScope())
                //{
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
                        #endregion

                        #region Fetch Data into Object
                        if (_Doc.Fetch(cn, new ARQO.Criteria((int)_codeKey, docKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return GVar.gcCancel;
                        }

                        //Record Not Found
                        if (GFunc.NEInt(this._Doc.DocKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);//Need to throw because to prevent GetReadOnly from running when GetEdit Fail
                        }
                        #endregion

                        #region Fetch Data in dtTables
                        _DocDetItms.Clear();
                        _DocDetItms.Fetch(cn, new ARQODetItms.Criteria(docKey, 1));

                        _DocDetItmVendors.Clear();
                        _DocDetItmVendors.Fetch(cn, new ARQODetItmVendors.Criteria(docKey, 1));

                        _DocDetVendors.Clear();
                        _DocDetVendors.Fetch(cn, new ARQOVendors.Criteria(docKey, 1));

                        _Doc.Attachments.Clear();
                        _Doc.Attachments.Fetch(cn, new SYSAttachments.Criteria((int)this._Doc.DocCodeKey, this._Doc.DocKey, _Doc.DocStatus == "Approved" ? 1 : 5));
                        #endregion


                        //if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)
                        //    throw new Exception("GetEdit process failed. Transaction has aborted.");

                        //scope.Complete();
                    }
                //}               

                #region Set ObjDoc Flag
                _Doc._GUID = _guID;
                _Doc._isNew = false;
                _Doc._isDirty = false;
                _Doc._isReadOnly = DocUtility.Doc_OpenDisAllowEdit(_Doc);               
                #endregion

                #region Set detail default values
                DocDetItem_SetDefaultValue();
                DocDetItemVendor_SetDefaultValue();
                DocDetVendor_SetDefaultValue();
                #endregion
                restoreFlag = false;
                
                

                //Check if user has permission to edit the already printed document
                if (Doc.DocPrinted)
                {
                    if (SECPermUtility.Perform(GVar.PermissionID.Save_Printed_Quotation, true) == false)
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
                    this._Doc = copyARQO;
                    this._DocDetItms = copyARQODetItms;
                    this._DocDetItmVendors = copyARQODetItmVendorss;
                    this._DocDetVendors = copyARQOVendors;
                }
                #endregion

                #region Set Null to Backup Objects
                copyARQO = null;
                copyARQODetItms = null;
                copyARQODetItmVendorss = null;
                copyARQOVendors = null;
                #endregion
            }
        }//Completed       
        public string GetReadOnly(int docKey, string docID)
        {
            #region Declaration
            ARQO copyARQO = null;
            ARQODetItms copyARQODetItms = null;
            ARQODetItmVendors copyARQODetItmVendorss = null;
            ARQOVendors copyARQOVendors = null;
            bool restoreFlag = false;
            #endregion

            try
            {
                #region Copy original object
                if (!GFunc.IsNE(this._Doc))
                    copyARQO = _Doc.Clone();

                if (_DocDetItms != null)
                    copyARQODetItms = GFunc.TACopyDataTable(_DocDetItms);

                if (_DocDetVendors != null)
                    copyARQOVendors = GFunc.TACopyDataTable(_DocDetVendors);

                if (_DocDetItmVendors != null)
                    copyARQODetItmVendorss = GFunc.TACopyDataTable(_DocDetItmVendors);
                #endregion

                #region Check Permission and record access
                if (SECPermUtility.Read(_permID, true) == false)
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
                        if (_Doc.Fetch(cn, new ARQO.Criteria((int)_codeKey, docKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return GVar.gcCancel;
                        }
                        #endregion

                        #region Fetch data into dtTables
                        _DocDetItms.Clear();
                        _DocDetItms.Fetch(cn, new ARQODetItms.Criteria(docKey, 1));

                        _DocDetItmVendors.Clear();
                        _DocDetItmVendors.Fetch(cn, new ARQODetItmVendors.Criteria(docKey, 1));

                        _DocDetVendors.Clear();
                        _DocDetVendors.Fetch(cn, new ARQOVendors.Criteria(docKey, 1));

                        _Doc.Attachments.Clear();
                        _Doc.Attachments.Fetch(cn, new SYSAttachments.Criteria((int)this._Doc.DocCodeKey, this._Doc.DocKey, _Doc.DocStatus == "Approved" ? 1 : 5));
                        #endregion

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)
                            throw new Exception("GetReadOnly process failed. Transaction has aborted.");

                        scope.Complete();

                        #region Set ObjDoc Flag
                        _Doc._GUID = _guID;
                        _Doc._isReadOnly = true;
                        _Doc._isNew = false;
                        _Doc._isDirty = false;
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
                    this._Doc = copyARQO;
                    this._DocDetItms = copyARQODetItms;
                    this._DocDetItmVendors = copyARQODetItmVendorss;
                    this._DocDetVendors = copyARQOVendors;
                }
                #endregion

                #region Set Null to Backup Objects
                copyARQO = null;
                copyARQODetItms = null;
                copyARQODetItmVendorss = null;
                copyARQOVendors = null;
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
                        this._Doc = new ARQO();
                        this._DocDetItms = new ARQODetItms(cn);
                        this._DocDetItmVendors = new ARQODetItmVendors(cn);
                        this._DocDetVendors = new ARQOVendors(cn);
                        this._Doc.Attachments = new SYSAttachments();
                        #endregion

                        #region Fetch Data into Object
                        GFunc.ConvertDataTableToObject(dtHeader, this._Doc);
                        #endregion

                        #region Fetch data into dtTables/Object
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[0], _DocDetItms);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[1], _DocDetItmVendors);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[2], _DocDetVendors);
                        #endregion

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)
                            throw new Exception("Transaction has aborted.");
                        scope.Complete();

                        #region Set ObjDoc Flag
                        _Doc._GUID = _guID;
                        _Doc._isReadOnly = true;
                        _Doc._isNew = false;
                        _Doc._isDirty = false;
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

        public void MarkAsReadOnly()
        {
           //Remove all locks by GUID except inprogress Locking
            SysLockUtility.RemoveLockGUIDKeepIP(true, _guID, _codeKey);              
            _Doc._isReadOnly = true;
        }

        public string Save(int ButtonAction, System.Windows.Forms.Form frm = null)
        {
            #region Declaration
            SqlConnection cn = null;
            ARQO copyARQO = null;
            ARQODetItms copyARQODetItms = null;
            ARQODetItmVendors copyARQODetItmVendorss = null;
            ARQOVendors copyARQOVendors = null;
            bool restoreFlag = false;
            #endregion

            try
            {
                #region Copy original object or Attach Eventhandler to new object
                if (!GFunc.IsNE(_Doc))
                    copyARQO = _Doc.Clone();

                if (_DocDetItms != null)
                    copyARQODetItms = GFunc.TACopyDataTable(_DocDetItms);

                if (_DocDetItmVendors != null)
                    copyARQODetItmVendorss = GFunc.TACopyDataTable(_DocDetItmVendors);

                if (_DocDetVendors != null)
                    copyARQOVendors = GFunc.TACopyDataTable(_DocDetVendors);
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

                if (!DocDetItm_Validation(cn, null))
                    return GVar.gcCancel;

                if (!DocDetItmVendor_Validation(cn, null))
                    return GVar.gcCancel;

                if (!DocDetVendor_Validation(cn))
                    return GVar.gcCancel;
                cn.Close();
                #endregion

                //Turn on restore flag to restore objects if any error occurs
                restoreFlag = true;

                #region SaveProcess

                htDetails.Clear();
                htDetails.Add(GEnum.Details.Doc_Itm, _DocDetItms);
                htDetails.Add(GEnum.Details.Doc_ItmVendor, _DocDetItmVendors);
                htDetails.Add(GEnum.Details.Doc_Vendor, _DocDetVendors);
                htDetails.Add(GEnum.Details.Doc_Attachment, _Doc.Attachments);

                if (ButtonAction == (int)GEnum.DocAction.Save)
                    _Doc.DocStatus = "Draft";
                //else if (ButtonAction == (int)GEnum.DocAction.Post)
                //    if (SysOptionUtility.DatabaseBranchCode.Equals("ADL"))
                //        _Doc.DocStatus = ApprovalStatus.Requested;

                if (DocUtility.Doc_SaveProcess(_Doc, htDetails, _permID, ButtonAction, false, frm) == false)
                    return GVar.gcCancel;

                #endregion

                #region Set ObjDoc flags
                _Doc._isDirty = false;
                _Doc._isNew = false;
                _Doc._isReadOnly = _Doc.DocState == (int)GEnum.DocState.Pending; /* added by YST on 2022/06/30 */
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
                if (cn != null && cn.State != ConnectionState.Closed)
                    cn.Close();

                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._Doc = copyARQO;
                    this._DocDetItms = copyARQODetItms;
                    this._DocDetItmVendors = copyARQODetItmVendorss;
                    this._DocDetVendors = copyARQOVendors;
                }
                #endregion

                #region Reset Backup Objects
                copyARQO = null;
                copyARQODetItms = null;
                copyARQODetItmVendorss = null;
                copyARQOVendors = null;
                #endregion
            }
        }//Completed

        public string Delete(UltraGrid grd)
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
                    _Doc = new ARQO();
                    _Doc._GUID = _guID;
                    _DocDetItms = new ARQODetItms(cn);
                    _DocDetItmVendors = new ARQODetItmVendors(cn);
                    _DocDetVendors = new ARQOVendors(cn);
                }
                this.New(grd);
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
                if (!SysLockUtility.CheckAddLock( true, 0, _codeKey, _Doc.DocKey, _guID))
                    return GVar.gcCancel;
                #endregion

                #region Delete Document
                Hashtable details = new Hashtable();
                details.Add(GEnum.Details.Doc_Itm, _DocDetItms);
                details.Add(GEnum.Details.Doc_ItmVendor, _DocDetItmVendors);
                details.Add(GEnum.Details.Doc_Vendor, _DocDetVendors);
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
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public string Clear(UltraGrid grd)
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
            ARQO copyARQO = null;
            ARQODetItms copyARQODetItms = null;
            ARQODetItmVendors copyARQODetItmVendorss = null;
            ARQOVendors copyARQOVendors = null;
            bool restoreFlag = false;
            int newDocKey = 0;
            #endregion

            try
            {
                #region Copy original object or Attach Eventhandler to new object
                if (!GFunc.IsNE(_Doc))
                    copyARQO = _Doc.Clone();

                if (_DocDetItms != null)
                    copyARQODetItms = GFunc.TACopyDataTable(_DocDetItms);

                if (_DocDetItmVendors != null)
                    copyARQODetItmVendorss = GFunc.TACopyDataTable(_DocDetItmVendors);

                if (_DocDetVendors != null)
                    copyARQOVendors = GFunc.TACopyDataTable(_DocDetVendors);
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
                        _Doc._GUID = _guID;
                        _Doc._isDirty = false;
                        _Doc._isNew = true;
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
                        _Doc._DocID = string.Empty;
                        _Doc._DocDate = DateTime.Today.Date;
                        _Doc._DocQuoteStatus = 10;//pending
                        _Doc._DocQuoteReason = string.Empty;
                        _Doc._DocState = 10;//New
                        _Doc.DocStatus = "New";
                        _Doc.DocRemAdditional3 = "";
                        _Doc._DocPrinted = false;
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

                        _Doc._DocQONum = string.Empty;
                        _Doc._DocSONum = string.Empty;
                        _Doc._DocDONum = string.Empty;
                        _Doc._DocIVNum = string.Empty;
                        _Doc._DocPONum = string.Empty;
                        _Doc._DocPDNum = string.Empty;
                        _Doc._Custom4 = string.Empty;
                        _Doc._Custom5 = string.Empty;

                        #endregion

                        #region Set values in DetItm
                        foreach (DataRow item in _DocDetItms.Rows)
                        {
                            item["DocKey"] = _Doc.DocKey;
                            item["NSLink"] = "11100-" + _Doc._DocKey+"-"+GFunc.NEStr( item["DocItmKey"],"");
                            item["CreateDate"] = DBNull.Value;
                            item["CreateUserKey"] = 0;
                            item["LastModifiedDate"] = DBNull.Value;
                            item["LastModifiedUserKey"] = 0;
                            item["APPOID"] = "";
                        }
                        _DocDetItms.Columns["DocKey"].DefaultValue = _Doc.DocKey;
                        #endregion

                        #region Set values in DetItmVendor
                        foreach (DataRow item in _DocDetItmVendors.Rows)
                        {
                            item["DocKey"] = _Doc.DocKey;
                            item["Selected"] = false;
                            item["VendorStatus"] = 10;//Ready
                            item["CreateDate"] = DBNull.Value;
                            item["CreateUserKey"] = 0;
                            item["LastModifiedDate"] = DBNull.Value;
                            item["LastModifiedUserKey"] = 0;
                        }
                        _DocDetItmVendors.Columns["DocKey"].DefaultValue = _Doc.DocKey;
                        #endregion

                        #region Set values in DetVendor
                        foreach (DataRow item in _DocDetVendors.Rows)
                        {
                            item["DocKey"] = _Doc.DocKey;
                            item["TransmitStatus"] = 30;//Pending
                            item["CreateDate"] = DBNull.Value;
                            item["CreateUserKey"] = 0;
                            item["LastModifiedDate"] = DBNull.Value;
                            item["LastModifiedUserKey"] = 0;
                        }
                        _DocDetVendors.Columns["DocKey"].DefaultValue = _Doc.DocKey;
                        #endregion

                        //#region Set values in DetAttachment
                        //foreach (SYSAttachment Obj in this._Doc.Attachments)
                        //{
                        //    Obj.DocDC = _Doc.DocCodeKey;
                        //    Obj.DocDK = _Doc.DocKey;
                        //}
                        //#endregion
                        //_Doc.attachments.CopyWithDMAS(cn,copyARQO.DocCodeKey,copyARQO.DocKey,_Doc.DocCodeKey, _Doc._DocKey);
                        _Doc.attachments.Clear(); //May Modified on 14-Apr-2025
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)
                            throw new Exception("Copy process failed. Transaction has aborted. Please try one more time.");
                        scope.Complete();

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
                    this._Doc = copyARQO;
                    this._DocDetItms = copyARQODetItms;
                    this._DocDetItmVendors = copyARQODetItmVendorss;
                    this._DocDetVendors = copyARQOVendors;
                }
                #endregion

                #region Reset Backup Objects
                copyARQO = null;
                copyARQODetItms = null;
                copyARQODetItmVendorss = null;
                copyARQOVendors = null;
                #endregion
            }
        }//Completed

        public string RevisedCopy()
        {
            #region Declaration
            string opValue = string.Empty;
            ARQO copyARQO = null;
            ARQODetItms copyARQODetItms = null;
            ARQODetItmVendors copyARQODetItmVendorss = null;
            ARQOVendors copyARQOVendors = null;
            bool restoreFlag = false;
            int newDocKey = 0;
         
            #endregion

            try
            {
                #region Copy original object or Attach Eventhandler to new object
                if (!GFunc.IsNE(_Doc))
                    copyARQO = _Doc.Clone();

                if (_DocDetItms != null)
                    copyARQODetItms = GFunc.TACopyDataTable(_DocDetItms);

                if (_DocDetItmVendors != null)
                    copyARQODetItmVendorss = GFunc.TACopyDataTable(_DocDetItmVendors);

                if (_DocDetVendors != null)
                    copyARQOVendors = GFunc.TACopyDataTable(_DocDetVendors);
                #endregion

                #region Check Security Permission
                if (!SECPermUtility.Add(_permID, true))
                    return GVar.gcCancel;
                #endregion

                    //Turn on restore flag to restore objects if any error occurs
                    restoreFlag = true;

                    #region Remove all locks by GUID except inprogress Locking
                    if (!SysLockUtility.RemoveLockGUIDKeepIP(true, _guID, _codeKey))
                        return GVar.gcCancel;
                    #endregion

                    #region Set ObjDoc flags
                    _Doc._GUID = _guID;
                    _Doc._isDirty = false;
                    _Doc._isNew = true;
                    _Doc.IsReadOnly = false;
                    #endregion

                    #region Assign DocKey
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();
                        newDocKey = SysOptionUtility.NewDocKey_Get(cn,_codeKey);
                    }
                    if (newDocKey == 0)
                        return GVar.gcCancel;
                    else
                        _Doc._DocKey = newDocKey;
                    #endregion

                    #region Set values in Doc
                    //_Doc._DocID = ;
                    if(_Doc._DocID.Length>0)
                    {
                        int c = 0;
                        string id = _Doc._DocID;
                        string[] idarr = _Doc.DocID.Split('/');
                        if(idarr.Length>2)
                        {
                            string num = idarr[2];
                            if(num.Contains("-"))
                            {
                                string[] numArr = num.Split('-');
                                if(numArr.Length>1)
                                {
                                    if(Int32.TryParse(numArr[1], out c))
                                    {
                                        id = idarr[0] + "/" + idarr[1] + "/" + numArr[0];
                                    }                                   
                                }                                
                            }                             
                        }
                        c++;//new ARQO.Criteria((int)codeKey, 0, vAutoID, 0), true)
                        string docID = id + "-" + c;
                        int i = 1;
                        while(_Doc.Validation(new ARQO.Criteria((int)_codeKey, 0, docID, 0), true)==false && i<=50)
                        {
                            c++;
                            docID = id + "-" + c;
                        }
                        if (i > 50)
                        {                            
                            MsgBox.Show("Revise Copy failed. Please copy as ");                           
                        }
                        else
                        {
                            _Doc.DocID = docID;
                        }
                    }                    

                    _Doc._DocDate = DateTime.Today.Date;
                    _Doc._DocStatus = "New";
                    _Doc._DocQuoteStatus = 10;//pending
                    _Doc._DocQuoteReason = string.Empty;
                    _Doc._DocState = 10;//New
                    _Doc._DocPrinted = false;
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

                    _Doc._DocQONum = string.Empty;
                    _Doc._DocSONum = string.Empty;
                    _Doc._DocDONum = string.Empty;
                    _Doc._DocIVNum = string.Empty;
                    _Doc._DocPONum = string.Empty;
                    _Doc._DocPDNum = string.Empty;

                    #endregion

                    #region Set values in DetItm
                    foreach (DataRow item in _DocDetItms.Rows)
                    {
                        item["DocKey"] = _Doc.DocKey;
                        item["NSLink"] = "11100-" + _Doc._DocKey + "-" + GFunc.NEStr(item["DocItmKey"], "");
                    item["CreateDate"] = DBNull.Value;
                        item["CreateUserKey"] = 0;
                        item["LastModifiedDate"] = DBNull.Value;
                        item["LastModifiedUserKey"] = 0;
                    }
                    _DocDetItms.Columns["DocKey"].DefaultValue = _Doc.DocKey;
                    #endregion

                    #region Set values in DetItmVendor
                    foreach (DataRow item in _DocDetItmVendors.Rows)
                    {
                        item["DocKey"] = _Doc.DocKey;
                        item["Selected"] = false;
                        item["VendorStatus"] = 10;//Ready
                        item["CreateDate"] = DBNull.Value;
                        item["CreateUserKey"] = 0;
                        item["LastModifiedDate"] = DBNull.Value;
                        item["LastModifiedUserKey"] = 0;
                    }
                    _DocDetItmVendors.Columns["DocKey"].DefaultValue = _Doc.DocKey;
                    #endregion

                    #region Set values in DetVendor
                    foreach (DataRow item in _DocDetVendors.Rows)
                    {
                        item["DocKey"] = _Doc.DocKey;
                        item["TransmitStatus"] = 30;//Pending
                        item["CreateDate"] = DBNull.Value;
                        item["CreateUserKey"] = 0;
                        item["LastModifiedDate"] = DBNull.Value;
                        item["LastModifiedUserKey"] = 0;
                    }
                    _DocDetVendors.Columns["DocKey"].DefaultValue = _Doc.DocKey;
                #endregion

                #region Attachment CopyWithDMAS
                List<SqlParameter> list2 = new List<SqlParameter>();
                list2.Add(new SqlParameter("@DocDC", _Doc.DocCodeKey));
                list2.Add(new SqlParameter("@DocDK", _Doc.DocKey));
                list2.Add(new SqlParameter("@SourceDC", copyARQO.DocCodeKey));
                list2.Add(new SqlParameter("@SourceDK", copyARQO.DocKey));
                list2.Add(new SqlParameter("@SecGrp", SysOptionUtility.DatabaseBranchCode + "Sales"));
                GFunc.ExecuteNonQueryProc("SYSAttachment_CopyWithDMAS", list2);
                #endregion
                #region Set values in DetAttachment
                //foreach (SYSAttachment Obj in this._Doc.Attachments)
                //{
                //    //Obj.DocDC = _Doc.DocCodeKey;
                //    //Obj.DocDK = _Doc.DocKey;
                //}
                #endregion
                _Doc.attachments.Fetch(new SYSAttachments.Criteria(_Doc.DocCodeKey, _Doc.DocKey, 1));
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
                    this._Doc = copyARQO;
                    this._DocDetItms = copyARQODetItms;
                    this._DocDetItmVendors = copyARQODetItmVendorss;
                    this._DocDetVendors = copyARQOVendors;
                }
                #endregion

                #region Reset Backup Objects
                copyARQO = null;
                copyARQODetItms = null;
                copyARQODetItmVendorss = null;
                copyARQOVendors = null;
                #endregion
            }
        }//Completed

        public string CopyFrom(GEnum.SystemCode sourceDocCodekey, int sourceDocKey, UltraGrid grd, bool NsLink, out DataTable dtDetail)
        {
            DataSet dsCopy = null;

            try
            {
                this.New(grd);
                dsCopy = DocUtility.Doc_Copy((int)sourceDocCodekey, sourceDocKey, (int)_codeKey, this._Doc, NsLink);

                //Note: we do not copy the DocDetItmVendor and DocDetVendor as for a new QO, these should be empty
                GFunc.CopyDocumentHeader(dsCopy.Tables[0], this._Doc);
                dtDetail = dsCopy.Tables[1];
                //this._Doc.Attachments = GFunc.Get_Attachments(sourceDocKey, (int)sourceDocCodekey, (Document)this._Doc);
                //this._Doc.Attachments.CopyWithDMAS((int)sourceDocCodekey,sourceDocKey, _Doc.DocCodeKey, _Doc.DocKey);
                this._Doc.Attachments.Clear();//May 14-Apr-2025

                #region Set values in Doc
                _Doc._DocID = string.Empty;
                _Doc._DocDate = DateTime.Today.Date;
                _Doc._DocStatus = "New";                
                _Doc.DocRemAdditional3 = "";
                _Doc._DocQuoteStatus = 10;//pending
                _Doc._DocQuoteReason = string.Empty;
                _Doc._DocState = 10;//New
                _Doc._DocPrinted = false;
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

                _Doc._DocQONum = string.Empty;
                _Doc._DocSONum = string.Empty;
                _Doc._DocDONum = string.Empty;
                _Doc._DocIVNum = string.Empty;
                _Doc._DocPONum = string.Empty;
                _Doc._DocPDNum = string.Empty;

                #endregion

                return GVar.gcPass;

            }
            catch (TAException tex)
            {
                this.New(grd);
                throw Error(tex);
            }
            catch (Exception ex)
            {
                this.New(grd);
                throw Error(ex);
            }
            finally
            {
                dsCopy = null;
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

        public bool GenerateItmVendor()
        {
            try
            {
                string DocItmVendorXML = "";

                var varItms = (from row in _DocDetItms.AsEnumerable()
                               select new
                               {
                                   DocItmKey = row.Field<int>("DocItmKey"),
                                   ItmKey = row.Field<int?>("ItmKey"),
                                   ItmVendorKey = row.Field<int?>("ItmVendorKey")
                               });

                DataSet dsItmVendor = new DataSet();
                dsItmVendor.Tables.Add(varItms.AsDataTable());
                dsItmVendor.Tables[0].TableName = "dtDocItm";

                dsItmVendor.Tables.Add(_DocDetItmVendors.Copy());
                dsItmVendor.Tables[1].TableName = "dtDocItmVendor";

                DocItmVendorXML = GFunc.ConvertDataTableToXML(dsItmVendor);

                List<SqlParameter> parmList = new List<SqlParameter>();

                parmList.Add(new SqlParameter("@DocKey", _Doc.DocKey));
                parmList.Add(new SqlParameter("@xmlDocItmVendor", DocItmVendorXML));
                parmList.Add(new SqlParameter("@RetValue", 0));
                parmList[2].Direction = ParameterDirection.Output;

                DataTable dt = GFunc.ExecuteProc("ARQO_Vendor_Generate", parmList);

                _DocDetItmVendors.Clear();
                GFunc.CopyDataTableToDetailObject(dt, _DocDetItmVendors);

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

            #region commented Previous code which produce the result by using both linq & server
            /*var varItms = (from row in _DocDetItms.AsEnumerable()
                           select new
                           {
                               DocItmKey = row.Field<int>("DocItmKey"),
                               ItmKey = row.Field<int>("ItmKey")
                           });

            var varItmVendors = (from row in _DocDetItmVendors.AsEnumerable()
                                 select new
                                 {
                                     DocItmKey = row.Field<int>("DocItmKey"),
                                     VendorKey = row.Field<int>("VendorKey"),
                                     VendorItmPrice = row.Field<decimal>("VendorItmPrice")
                                 });

            DataTable dtDocItmVendor = (from itmRows in varItms
                                        join rows in varItmVendors on
                     itmRows.DocItmKey equals rows.DocItmKey
                                        select new
                                        {
                                            DocItmKey = rows.DocItmKey,
                                            ItmKey = itmRows.ItmKey,
                                            VendorKey = rows.VendorKey,
                                            VendorItmPrice = rows.VendorItmPrice,
                                        }).AsDataTable();

            string DocItmVendor = "";          

            try
            {
                List<SqlParameter> parmList = new List<SqlParameter>();

                DocItmVendor = GFunc.ConvertDataTableToXML(dtDocItmVendor);             

        
                parmList.Add(new SqlParameter("@xmlDocItmVendor", DocItmVendor));
                parmList.Add(new SqlParameter("@RetValue", 0));
                parmList[1].Direction = ParameterDirection.Output;

                DataTable dt = GFunc.ExecuteProc("ARQO_Vendor_Generate", parmList);


                var result = (from row in dt.AsEnumerable()
                              select new
                              {
                                  DocItmKey = row.Field<int>("DocItmKey"),
                                  ItmKey = row.Field<int>("ItmKey"),
                                  VendorKey = row.Field<int>("VendorKey"),
                                  VendorItmPrice = row.Field<decimal>("VendorItmPrice")
                              });


                var existing = from s in result
                               join h in varItmVendors
                               on
                               new { s.DocItmKey, s.VendorKey }
                               equals
                                new { h.DocItmKey, h.VendorKey }
                               select s;

                //Get Vendors but not existing ...
                var moreVendors = result.Except(existing);

                if (!GFunc.IsNE(moreVendors) && moreVendors.Count() > 0)
                {
                    foreach (var row in moreVendors)
                    {
                        DataRow drVendor = _DocDetItmVendors.NewRow();
                        drVendor["DocKey"] = _Doc.DocKey;
                        drVendor["DocItmKey"] = row.DocItmKey;
                        drVendor["VendorKey"] = row.VendorKey;
                        drVendor["VendorItmPrice"] = row.VendorItmPrice;
                        _DocDetItmVendors.Rows.Add(drVendor);
                    }
                }

                DataTable lessVendors = existing.Except(result).AsDataTable();
                _DocDetItmVendors.PrimaryKey = new DataColumn[] { _DocDetItmVendors.Columns["DocItmKey"], _DocDetItmVendors.Columns["VendorKey"] };

                //Really Remove from Vendor Grid
                foreach (DataRow row in lessVendors.Rows)
                {
                    _DocDetItmVendors.Rows.Remove(_DocDetItmVendors.Rows.Find(new object[2] { row["DocItmKey"], row["VendorKey"].ToString() }));
                }
                _DocDetItmVendors.PrimaryKey = null;
                return true;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }*/
            #endregion
        }//Completed

        //ttm
        public void UpdateApprovalInformation(int status, string disapprovemsg)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    SqlCommand com = new SqlCommand("Update_ApprovalInformation", cn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@SaleRepKey", AppInfor.CurrentUserKey);
                    com.Parameters.AddWithValue("@QuoDoID", _Doc.DocID.ToString());
                    com.Parameters.AddWithValue("@status", status);
                    com.Parameters.AddWithValue("@disapprovemsg", disapprovemsg);
                    com.Parameters.AddWithValue("@whichApp", 1);
                    com.ExecuteNonQuery();
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

        }
        //ttm
        public void InsertVendorGrid()
        {
            //In preparing for result and existing, can only use VendorKey, remove ConID and ConNm 
            //This information will be update at the end of the result
            //Insert unique Vendors to RFQ tab from ItmVendorDetGrid(=_ARQODetItmVendors)
            try
            {
                _DocDetItmVendors.AcceptChanges();
                var vendors = (from row in _DocDetItmVendors.AsEnumerable()
                               select new
                               {
                                   VendorKey = row.Field<int>("VendorKey"),
                               }).Distinct();

                DataTable dtDocItm = vendors.AsDataTable();
                dtDocItm.TableName = "dtDocItmVendor";

                DataSet dsVendor = new DataSet();

                dsVendor.Tables.Add(dtDocItm);
                dsVendor.Tables.Add(_DocDetVendors.DefaultView.ToTable("dtDocVendor"));

                string DocVendorXML = GFunc.ConvertDataTableToXML(dsVendor);

                List<SqlParameter> parmList = new List<SqlParameter>();

                parmList.Add(new SqlParameter("@DocKey", _Doc.DocKey));
                parmList.Add(new SqlParameter("@xmlDocVendor", DocVendorXML));
                parmList.Add(new SqlParameter("@UserKey", AppInfor.currentUserKey));

                parmList.Add(new SqlParameter("@RetValue", 0));
                parmList[3].Direction = ParameterDirection.Output;

                DataTable dt = GFunc.ExecuteProc("ARQO_Vendor_GenerateRFQ", parmList);

                _DocDetVendors.Clear();
                GFunc.CopyDataTableToDetailObject(dt, _DocDetVendors);
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

        //Set Default Values
        private bool Doc_SetDefaultValue(SqlConnection cn)
        {
            try
            {
                //Set default values in object            
                _Doc._DocCodeKey = (int)_codeKey;
                _Doc._DocDate = DateTime.Today;
                _Doc._DocDateValid = DateTime.Today.AddDays(GFunc.NEInt(SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultDateValid, cn), 0));
                _Doc._DocSign = 1;
                if(approvalRequired)
                    _Doc.DocStatus = "New";
                _Doc._DocQuoteStatus = 10;//pending
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
                _Doc._DocState = 10;
                _Doc._DocPrinted = false;
                _Doc._ApproveUserKey = 0;
                _Doc._DisapproveUserKey = 0;
                _Doc._DisapproveCount = 0;
                _Doc._Attachment = false;
                _Doc._BranchKey = 0;
                _Doc._PurgeKeep = 0;
                _Doc._PurgeData = false;

                //trying to reduce database routes
                DataTable dt = SYSOptions.GetOptions(cn,
                    new SYSOptions.Criteria("DefaultDocRemDelivery,DefaultDocRemPayment,DefaultDocRemPrice,DefaultDocRemValidity"
                    , AppInfor.currentUserKey, 8));
                if(dt.Rows.Count>0)
                {
                    foreach(DataRow dr in dt.Rows)
                    {
                        switch(GFunc.NEStr(dr["OpID"],""))
                        {
                            case "DefaultDocRemDelivery":
                                _Doc._DocRemDelivery = GFunc.NEStr(dr["OpValue"], "");
                                break;
                            case "DefaultDocRemPayment":
                                _Doc._DocRemPayment = GFunc.NEStr(dr["OpValue"], "");
                                break;
                            case "DefaultDocRemPrice":
                                _Doc._DocRemPrice = GFunc.NEStr(dr["OpValue"], "");
                                break;
                            case "DefaultDocRemValidity":
                                _Doc._DocRemValidity = GFunc.NEStr(dr["OpValue"], "");
                                break;
                        }
                    }
                }
                //_Doc._DocRemDelivery = SysOptionUtility.GetStr("DefaultDocRemDelivery", cn);
                //_Doc._DocRemPayment = SysOptionUtility.GetStr("DefaultDocRemPayment", cn);
                //_Doc._DocRemPrice = SysOptionUtility.GetStr("DefaultDocRemPrice", cn);
                //_Doc._DocRemValidity = SysOptionUtility.GetStr("DefaultDocRemValidity", cn);

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
                _DocDetItms.Columns["ItmAmtF"].DefaultValue = 0;
                _DocDetItms.Columns["ItmAmtH"].DefaultValue = 0;
                _DocDetItms.Columns["ItmTaxable"].DefaultValue = true;
                _DocDetItms.Columns["ItmTaxGrpRate"].DefaultValue = 0;
                _DocDetItms.Columns["ItmTaxGrpAmtF"].DefaultValue = 0;
                _DocDetItms.Columns["ItmTaxGrpAmtL"].DefaultValue = 0;
                _DocDetItms.Columns["ItmHide"].DefaultValue = false;
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
                _DocDetItms.Columns["NSLink"].DefaultValue = 0;
                _DocDetItms.Columns["CreateDate"].DefaultValue = DateTime.Today;
                _DocDetItms.Columns["CreateUserKey"].DefaultValue = AppInfor.currentUserKey;
                _DocDetItms.Columns["LastModifiedDate"].DefaultValue = DateTime.Today;
                _DocDetItms.Columns["LastModifiedUserKey"].DefaultValue = AppInfor.currentUserKey;
                _DocDetItms.Columns["ItmDetSN"].DefaultValue = 0;

                _DocDetItms.DefaultView.Sort = "ItmSN ASC";
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
        private bool DocDetItemVendor_SetDefaultValue()
        {
            try
            {
                //Whole Column Default Value
                _DocDetItmVendors.Columns["DocKey"].DefaultValue = _Doc.DocKey;
                _DocDetItmVendors.Columns["DocItmKey"].DefaultValue = 0;
                _DocDetItmVendors.Columns["VendorEqItmPrice"].DefaultValue = 0;
                _DocDetItmVendors.Columns["VendorItmPrice"].DefaultValue = 0;
                _DocDetItmVendors.Columns["Selected"].DefaultValue = false;
                _DocDetItmVendors.Columns["VendorStatus"].DefaultValue = 10;
                _DocDetItmVendors.Columns["CreateDate"].DefaultValue = DateTime.Today;
                _DocDetItmVendors.Columns["CreateUserKey"].DefaultValue = AppInfor.currentUserKey;
                _DocDetItmVendors.Columns["LastModifiedDate"].DefaultValue = DateTime.Today;
                _DocDetItmVendors.Columns["LastModifiedUserKey"].DefaultValue = AppInfor.currentUserKey;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            return true;
        }//Completed
        private bool DocDetVendor_SetDefaultValue()
        {
            try
            {
                //Whole Column Default Value
                _DocDetVendors.Columns["DocKey"].DefaultValue = _Doc.DocKey;
                _DocDetVendors.Columns["VendorCurrKey"].DefaultValue = 1;
                _DocDetVendors.Columns["TransmitStatus"].DefaultValue = 30;
                _DocDetVendors.Columns["CreateDate"].DefaultValue = DateTime.Today;
                _DocDetVendors.Columns["CreateUserKey"].DefaultValue = AppInfor.currentUserKey;
                _DocDetVendors.Columns["LastModifiedDate"].DefaultValue = DateTime.Today;
                _DocDetVendors.Columns["LastModifiedUserKey"].DefaultValue = AppInfor.currentUserKey;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }

            return true;
        }//Completed

        //Validations
        public bool Doc_Validation(SqlConnection cn)
        {            
            string errorMsgID = string.Empty;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());

            try
            {
                #region Clear Error in UI
                if (!GFunc.IsNE(this.ErrorNotifierHeader_Clear))
                    this.ErrorNotifierHeader_Clear.Invoke(this, e);
                #endregion

                #region Validation
                if (this._Doc.DocTranGrpKey == null)
                    this._Doc.DocTranGrpKey = 0;

                //When Fail will THROW exception
                BaseUtility.Validate(true, this._Doc.DocKey, "DocKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, 0, int.MaxValue, e, cn);
                BaseUtility.Validate(true, this._Doc.DocCodeKey, "DocCodeKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                BaseUtility.Validate(true, this._Doc.DocType, "DocType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                BaseUtility.Validate(true, this._Doc.DocSign, "DocSign", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, -1, -1, 1, e, cn);

                //When Fail will show in ErrorProvider and continue next statement
                if (Doc.IsNew == false)
                    BaseUtility.Validate(false, this._Doc.DocID, "DocID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);

                if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM || SysOptionUtility.DatabaseBranchCode == DBCode.SOP || SysOptionUtility.DatabaseBranchCode == DBCode.ADL)
                    BaseUtility.Validate(false, this._Doc.DocEmKey, "DocEmkey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);

                BaseUtility.Validate(false, this._Doc.DocDate, "DocDate", GEnum.DataType.DateTime, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocDateValid, "DocDateValid", GEnum.DataType.DateTime, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocTypeNm, "DocTypeNm", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocConKey, "DocConKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocConNm, "DocConNm", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocConUEN, "DocConUEN", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocDeptKey, "DocDeptKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocTranGrpKey, "DocTranGrpKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocAccKey, "DocAccKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocGrpKey, "DocGrpKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                //BaseUtility.Validate(false, this._Doc.DocPriceType, "DocPriceType", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocTermKey, "DocTermKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                //BaseUtility.Validate(false, this._Doc.DocEmKey, "DocEmKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocEnquiryDate, "DocEnquiryDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocQuoteStatus, "DocQuoteStatus", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocQuoteReason, "DocQuoteReason", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
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
                //BaseUtility.Validate(false, this._Doc.DocShipKey, "DocShipKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocShipDate, "DocShipDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
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
                if (SysOptionUtility.DatabaseBranchCode.Equals(DBCode.ADL))
                {
                    BaseUtility.Validate(false, this._Doc.DocRemAdditional1, "DocRemAdditional1", GEnum.DataType.String, GEnum.Require.No, 1000, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, this._Doc.DocRemAdditional2, "DocRemAdditional2", GEnum.DataType.String, GEnum.Require.No, 1000, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, this._Doc.DocRemAdditional3, "DocRemAdditional3", GEnum.DataType.String, GEnum.Require.No, 1000, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, this._Doc.DocRemAdditional4, "DocRemAdditional4", GEnum.DataType.String, GEnum.Require.No, 1000, null, null, null, null, e, cn);
                }
                else
                {
                    BaseUtility.Validate(false, this._Doc.DocRemAdditional1, "DocRemAdditional1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, this._Doc.DocRemAdditional2, "DocRemAdditional2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, this._Doc.DocRemAdditional3, "DocRemAdditional3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, this._Doc.DocRemAdditional4, "DocRemAdditional4", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                }
                BaseUtility.Validate(false, this._Doc.DocSubTotal, "DocSubTotal", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                //BaseUtility.Validate(false, this._Doc.DocOverallDisAcc, "DocOverallDisAcc", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocOverallDisRate, "DocOverallDisRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                BaseUtility.Validate(false, this._Doc.DocOverallDisAmt, "DocOverallDisAmt", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
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
                    return DocDetItm_Validation(cn, CheckRow);
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
        public bool DocDetItm_Validation(SqlConnection cn, DataRow CheckRow)
        {
            #region Declaration

            string msgValue = string.Empty;

            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
            int KeyCount = 0;
            #endregion

            try
            {
                if (CheckRow != null)
                {
                    if (DocUtility.HiddenValueCurrentRow_Set(cn, Doc, (int)GEnum.Details.Doc_Itm, CheckRow) == false)
                        return false;
                }

                foreach (DataRow drs in this._DocDetItms.Rows)
                {
                    DataRow dr = drs;
                    if (CheckRow != null)
                        dr = CheckRow;

                    if (GFunc.NEInt(dr["LineType"], (int)GEnum.RecDetailType.DItems) == (int)GEnum.RecDetailType.DItmAssembly)//Assembly Child Validation is not required
                        continue;

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
                    //BaseUtility.Validate(false, dr["ItmColorKey"], "ItmColorKey", GEnum.DataType.Integer, GEnum.Require.No, null, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmScaleSize"], "ItmScaleSize", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmHide"], "ItmHide", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmPacking"], "ItmPacking", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmRem"], "ItmRem", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmMark"], "ItmMark", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["ItmAttachment"], "ItmAttachment", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["NSLink"], "NSLink", GEnum.DataType.String, GEnum.Require.Yes, 50, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["Custom1"], "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["Custom2"], "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
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
                            BaseUtility.Validate(false, dr["ItmAccKey"], "ItmAccKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmLocKey"], "ItmLocKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);                                              
                            //BaseUtility.Validate(false, dr["ItmStock"], "ItmStock", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmQty"], "ItmQty", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmReqDate"], "ItmReqDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmPrmDate"], "ItmPrmDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
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
                            
                            BaseUtility.Validate(false, dr["ItmAccKey"], "ItmAccKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);

                            if (GFunc.NEInt(dr["ItmType"], 0) != (int)GEnum.ItemType.Charges)
                            {
                                BaseUtility.Validate(false, dr["ItmQty"], "ItmQty", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                                BaseUtility.Validate(false, dr["ItmUOMKey"], "ItmUOMKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                                BaseUtility.Validate(false, dr["ItmConRate"], "ItmConRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                            }
                            BaseUtility.Validate(false, dr["ItmReqDate"], "ItmReqDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
                            BaseUtility.Validate(false, dr["ItmPrmDate"], "ItmPrmDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, e, cn);
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
                        KeyCount = _DocDetItms.AsEnumerable().Count(p => p.Field<int>("DocItmKey") == GFunc.NEInt(dr["DocItmKey"], 0));

                        if (KeyCount > 1)
                        {
                            e.PropertyMessage.Add("DocItmKey", SysMessageUtility.Get(cn, "DocItmKey" + MsgID.Validation.DuplicateRecord));
                        }

                        // Check for duplicate ItmSN  
                        KeyCount = 0;
                        KeyCount = _DocDetItms.AsEnumerable().Count(p => p.Field<decimal>("ItmSN") == GFunc.NEDec(dr["ItmSN"], 0) && p.Field<int>("LineType") == 1000);

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
                        dr.RowError = msgValue;
                    }
                    else
                    {
                        dr.RowError = string.Empty;
                    }
                    #endregion

                    if (CheckRow != null)
                    {
                        if (e.PropertyMessage.Count == 0)
                            return true;
                        else
                            return false;
                    }
                }

                #region Price check // Added by May on 25-Jun-2024   
                if (e.PropertyMessage.Count == 0 && SysOptionUtility.DatabaseBranchCode == DBCode.BHM)                    
                {
                    string SNlist = "";
                                  
                    IEnumerable<DataRow> dtItemsFilter = _DocDetItms.AsEnumerable().Where(p => (p.Field<decimal?>("ItmPriceAfter") > p.Field<decimal?>("ItmControlPrice")
                    && p.Field<decimal?>("ItmControlPrice") != 0 && p.Field<decimal?>("ItmControlPrice") != -999));
                    
                    foreach (DataRow r in dtItemsFilter)
                    {
                        SNlist +=GFunc.NEDec(r.Field<decimal?>("ItmSN"),0).ToString("###") + ", ";
                    }
                    if (SNlist.Length > 1)
                        SNlist = SNlist.Remove(SNlist.Length - 2);
                    if (dtItemsFilter.Count() > 1)
                        MsgBox.Show("Warning!!! The prices of Items SN (" + SNlist + ") are higher than EStore prices.", GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                    else if (dtItemsFilter.Count() > 0)
                        MsgBox.Show("Warning!!! The price of Item SN " + SNlist + " is higher than EStore price.", GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                    
                }
                #endregion

                #region return result
                if (e.PropertyMessage.Count == 0)
                    return true;
                else
                    return false;
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
        public bool DocDetItmVendor_Validation(DataRow CheckRow)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocDetItmVendor_Validation(cn, CheckRow);
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
        public bool DocDetItmVendor_Validation(SqlConnection cn, DataRow CheckRow)
        {
            #region Declaration

            string msgValue = string.Empty;

            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
            int KeyCount = 0;
            #endregion

            try
            {
                foreach (DataRow drs in this._DocDetItmVendors.Rows)
                {
                    DataRow dr = drs;
                    if (CheckRow != null)
                    {
                        dr = CheckRow;
                    }

                    #region Common Validation
                    BaseUtility.Validate(false, dr["DocKey"], "DocKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["DocItmKey"], "DocItmKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["VendorKey"], "VendorKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["VendorEqItmQty"], "VendorEqItmQty", GEnum.DataType.Decimel, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["VendorEqItmPrice"], "VendorEqItmPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["VendorItmQty"], "VendorItmQty", GEnum.DataType.Decimel, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["VendorItmPrice"], "VendorItmPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["VendorLeadTime"], "VendorLeadTime", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["VendorWarranty"], "VendorWarranty", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["VendorRemarks"], "VendorRemarks", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["Selected"], "Selected", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["VendorStatus"], "VendorStatus", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["Custom1"], "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["Custom2"], "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["Custom3"], "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    #endregion

                    #region Additional Validation check
                    if (e.PropertyMessage.Count == 0)
                    {
                        // Check for duplicate DocKey + DocItmKey + VendorKey                    
                        KeyCount = _DocDetItmVendors.AsEnumerable().Count(p => (p.Field<int>("DocItmKey") == GFunc.NEInt(dr["DocItmKey"], 0) && p.Field<int>("VendorKey") == GFunc.NEInt(dr["VendorKey"], 0)));

                        if (KeyCount > 1)
                        {
                            e.PropertyMessage.Add("VendorKey", SysMessageUtility.Get(cn, "VendorKey" + MsgID.Validation.DuplicateRecord));
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
                        dr.RowError = msgValue;

                    }
                    else
                    {
                        dr.RowError = string.Empty;

                    }
                    #endregion

                    if (CheckRow != null)
                    {
                        if (e.PropertyMessage.Count == 0)
                            return true;
                        else
                            return false;
                    }
                }

                #region return result
                if (e.PropertyMessage.Count == 0)
                    return true;
                else
                    return false;
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
        public bool DocDetVendor_Validation()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocDetVendor_Validation(cn);
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
        public bool DocDetVendor_Validation(SqlConnection cn)
        {
            #region Declaration

            string msgValue = string.Empty;

            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
            int KeyCount = 0;
            #endregion

            try
            {
                foreach (DataRow dr in this._DocDetVendors.Rows)
                {
                    #region Common Validation
                    BaseUtility.Validate(false, dr["DocKey"], "DocKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["VendorKey"], "VendorKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["VendorCurrKey"], "VendorCurrKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["TransmitMode"], "TransmitMode", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["Attention"], "Attention", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["EmailAddr"], "EmailAddr", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["FaxNumber"], "FaxNumber", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["TransmitStatus"], "TransmitStatus", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    BaseUtility.Validate(false, dr["Custom1"], "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["Custom2"], "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    BaseUtility.Validate(false, dr["Custom3"], "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    #endregion

                    #region Additional Validation check
                    if (e.PropertyMessage.Count == 0)
                    {

                        // Check for duplicate VendorKey                    
                        KeyCount = _DocDetVendors.AsEnumerable().Count(p => (p.Field<int>("VendorKey") == GFunc.NEInt(dr["VendorKey"], 0)));

                        if (KeyCount > 1)
                        {

                            e.PropertyMessage.Add("VendorKey", SysMessageUtility.Get(cn, "VendorKey" + MsgID.Validation.DuplicateRecord));
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
                        dr.RowError = msgValue;

                    }
                    else
                    {
                        dr.RowError = string.Empty;

                    }
                    #endregion
                }

                #region return result
                if (e.PropertyMessage.Count == 0)
                    return true;
                else
                    return false;
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

        //Dirty Handle Events
        void Obj_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _Doc.IsDirty = true;
        }//Completed
        void Attachments_ListChanged(object sender, ListChangedEventArgs e)
        {
            _Doc.IsDirty = true;
        }//Completed

        //Error
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _Doc, _DocDetItms, _DocDetItmVendors, _DocDetVendors }, _codeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _Doc, _DocDetItms, _DocDetItmVendors, _DocDetVendors }, _codeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
    }
}
