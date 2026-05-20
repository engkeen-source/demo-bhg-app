using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using BOLib;
using System.IO;
using Infragistics.Win.UltraWinTabbedMdi;
using TAUtil;
using System.Data.SqlClient;

namespace WinUI
{
    public partial class frmMSTCon : Form
    {
        #region Local Variables

        private BOLib.MSTConFactory objMstConFactory = null;
        private string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        private GEnum.formInitMode formOpenMode = GEnum.formInitMode.Neither;
        private int recordKey = 0;
        private string recordID = string.Empty;
        private string msgID = string.Empty;
        private bool canEditRecordID = false;
        private bool openrecordNormal = true;
        private bool IsEdit = false;

        public GEnum.SystemCode SysCode =GEnum.SystemCode.Customer;
        string[,] parmList;

        //only use to show Read only data viewer for AuditLog 
        private bool IsOpenFromAuditLog = false;
        private DataTable _dtHeader = null;
        private DataSet _dsDetail = null;

        frmList fMSTConList = null;
        public GVar.ListEvent_CloseFORM list_CloseMSTForm = null;
        public GVar.ListEvent_RefreshRecord ListEvent_RefreshRecord = null;

        //For RefAddr Combo Fill
        private string _cdefaultBillAddr = string.Empty;
        private string _cdefaultShipAddr = string.Empty;
        private string _cdefaultStateAddr = string.Empty;
        private string _vdefaultBillAddr = string.Empty;
        private string _vdefaultShipAddr = string.Empty;

        //For Address Saving
        private bool isNewAddr = false;
        private bool isDirtyAddr = false;
        private int _addrKey = 0;
        private string _addrID = string.Empty;//This is used as a reference to store the current displayed address bcos _addrKey can be zero (new address) when customer record has not been saved
        int PreRowIndex = 0;
        #endregion

        //Initialize
        public frmMSTCon()
        {
            InitializeComponent();
        }//Completed
        public frmMSTCon(string conID)
        {
            InitializeComponent();
            recordID = conID;

            //Determine the System Code to use
            MSTCon rec = MSTCon.Get(recordID);
            if (GFunc.IsNEZ(rec.ConKey))
            {
                SysCode = GEnum.SystemCode.Customer;
            }
            else
            {
                if (rec.ConType == 20 || rec.ConType == 30) //vendor or both
                    SysCode = GEnum.SystemCode.Vendor;
                else
                    SysCode = GEnum.SystemCode.Customer;
            }
            formOpenMode = GEnum.formInitMode.Add;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            tsbList.Enabled = false;
            rec = null;
        }
        public frmMSTCon(string conID, GEnum.SystemCode sysCode)
        {
            InitializeComponent();
            SysCode = sysCode;
            recordID = conID;
            formOpenMode = GEnum.formInitMode.Add;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            tsbList.Enabled = false;
        }
        public frmMSTCon(int conkey)
        {
            InitializeComponent();
            this.recordKey = conkey;

            //Determine the System Code to use
            MSTCon rec = MSTCon.Get(recordKey);
            if (GFunc.IsNEZ(rec.ConKey))
            {
                formOpenMode = GEnum.formInitMode.Add;
                SysCode = GEnum.SystemCode.Customer;
            }
            else
            {
                if (rec.ConType == 20 || rec.ConType == 30) //vendor or both
                    SysCode = GEnum.SystemCode.Vendor;
                else
                    SysCode = GEnum.SystemCode.Customer;
            }
            formOpenMode = GEnum.formInitMode.Edit;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.tsbList.Enabled = false;
            rec = null;
        }//Completed
        public frmMSTCon(int conkey, GEnum.SystemCode sysCode)
        {
            InitializeComponent();
            SysCode = sysCode;
            recordKey = conkey;
            MSTCon rec = MSTCon.Get(recordKey);
            if (GFunc.IsNEZ(rec.ConKey))
                formOpenMode = GEnum.formInitMode.Add;
            else
                formOpenMode = GEnum.formInitMode.Edit;

            this.recordKey = conkey;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.tsbList.Enabled = false;
        }//Completed
        public frmMSTCon(GEnum.SystemCode sysCode)
        {
            InitializeComponent();
            SysCode = sysCode;
        }//Completed
        public frmMSTCon(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            InitializeComponent();
            SysCode = DocCodeKey;
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
        }//Completed

         
        //Form Events
        private void frmMSTCon_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            COOApprovalRequired.Enabled = false;
            try
            {
                // Initialize
                if (SysCode == BOLib.GEnum.SystemCode.Customer)
                    ultraLabel96.Text = "CUSTOMER RECORD";
                else if (SysCode == BOLib.GEnum.SystemCode.Vendor)
                    ultraLabel96.Text = "VENDOR RECORD";
                this.objMstConFactory = new BOLib.MSTConFactory(BOLib.GEnum.InstanceMode.Normal, SysCode);
                
                if (objMstConFactory.GUID <= 0)
                {
                    formClose = true;
                    return;
                }

                // Attach Event on Factory
                this.objMstConFactory.ErrorNotifierHeader_Set += new GVar.UINotifierEvent(this.ErrorNotifier_Set);
                this.objMstConFactory.ErrorNotifierHeader_Clear += new GVar.UINotifierEvent(this.ErrorNotifier_Clear);                

                //Check if user has permission to edit Record ID
                canEditRecordID = SECPermUtility.Perform("sysRecID", false);
                
                //Check if user has permission to enter credit limit amount
                if (SECPermUtility.Perform(GVar.PermissionID.Edit_Credit_Limit, false) == false)
                {
                    CCreditLimit.ReadOnly = true;
                    
                    if (objMstConFactory.ObjMSTCon.Rejected == true) Inactive.Enabled = false;
                }
                else
                {
                    CCreditLimit.ReadOnly = false;                    
                    if (objMstConFactory.ObjMSTCon.Rejected == true) Inactive.Enabled = false;
                }

                //Set FORM caption
                if (SysCode == GEnum.SystemCode.Customer)
                {
                    this.Text = "Customer Record";
                    tabDetail.Tabs[5].Visible = false;
                    tabDetail.SelectedTab = tabDetail.Tabs[0];
                }
                else
                {
                    this.Text = "Vendor Record";
                    tabDetail.SelectedTab = tabDetail.Tabs[1];
                }

                if (this.IsOpenFromAuditLog)
                {
                    if (objMstConFactory.SetReadOnlyData(_dtHeader, _dsDetail) == false)
                    {
                        formClose = true;
                        return;
                    }
                    Refresh_All(true);
                    GlobalUI.FormEnable_Set(this, false);
                    
                }
                else
                {
                    this.New_Process();

                    //When open from shortcutmenu (edit)
                    if (formOpenMode == GEnum.formInitMode.Edit)
                        this.OpenRecord(recordKey, recordID);
                    else if (formOpenMode == GEnum.formInitMode.Add)
                    {
                        if (canEditRecordID && recordID != string.Empty)
                            this.ConID.SetValueTrigger(recordID,false);
                    }
                }

                //Setup FORM control/grid format, menu, listID
                GlobalUI.FormGrids_Set(this, (int)objMstConFactory.ConstantCodeKey, out ContextMenuSetting);
                ////GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objMstConFactory.ConstantCodeKey);
                GlobalUI.Combos_Fill(this, (int)objMstConFactory.ConstantCodeKey);
                

                GlobalUI.Ctrl_Update(this, "CBranchKey", GEnum.CtlPropertyUpdate.Enabled, SysOptionUtility.UseBranch);
                GlobalUI.Ctrl_Update(this, "VBranchKey", GEnum.CtlPropertyUpdate.Enabled, SysOptionUtility.UseBranch);
                
                if (formOpenMode != GEnum.formInitMode.Neither)
                {
                    if (SysCode == GEnum.SystemCode.Customer)
                        CCurrkey_CustomUpdate(CCurrkey, null);
                    else if(SysCode == GEnum.SystemCode.Vendor)
                        VCurrkey_CustomUpdate(VCurrkey, null);
                }

                //Calibri, 9.75pt, style=Italic
                OpenConID.Font = new Font("Calibri", 9.75f, FontStyle.Italic);

            }
            catch (TAException tex)
            {
                if (tex.MsgID == MsgID.Common.NoMultiInstanceAllowed)
                {
                    this.formClose = true;
                    frmMain.gfrmMain.IsExistingForm(this);
                }
                else
                {
                    this.formClose = true;
                }
                Error(tex, true);
            }
            catch (Exception ex)
            {
                formClose = true;
                Error(ex, true);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed

        private void FormBindingSource_Set()
        {
            try
            {
                bdsConInfo.DataSource = objMstConFactory.ObjMSTCon;               
                bdsConInfo.AllowNew = true;
                bdsConInfo.ResetBindings(false);
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private void frmMSTCon_Shown(object sender, EventArgs e)
        {
            try
            {
                if (formClose)
                    this.Close();
                else
                    this.ConID.Focus();
                BankName.LimitToList = false;
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed
        private void frmMSTCon_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool runProcess = false;
            this.formClose = true;

            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }
            if (this.formClose && objMstConFactory == null)
                return;

            try
            {
                #region Closing with Invalid DataType error encountered
                //When the caller performs this.close, the system actually perform validation on all control automatically
                //if there are any control that fails validation (invalid datatype, the e.cancel is set to true, we have no control over this (not sure if this was done by csla)
                //thus we need to check for e.cancel = true so that we can skip the rest of the codes to prevent error message from appearing twice or more
                if (e.Cancel == true)
                    runProcess = true;
                else
                {
                    if (this.SaveChanges() == false)
                    {
                        if (formClose == false)
                        {
                            frmMain.gfrmMain.Tag = GVar.CancelMainFormClosing;  
                            e.Cancel = true;
                            return;
                        }
                        else
                            runProcess = true;
                    }
                }

                if (runProcess)
                {
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show("Validation Failed, Close the FORM anyway?",
                                          GEnum.MsgBoxIcon.Question,
                                          GEnum.MsgBoxButton.Yes,
                                          GEnum.MsgBoxButton.No);

                    if (btnSelect == GEnum.MsgBoxButton.No)
                    {
                        //to prohibit closing when error occurs even when the form is closed by main form
                        frmMain.gfrmMain.Tag = GVar.CancelMainFormClosing;
                        e.Cancel = true;
                        formClose = false; //(cancel form closing) if there has data when click save changes after close form 
                        return;
                    }
                    else
                    {
                        IsGridsDirty(true);
                        e.Cancel = false;
                    }
                }
                #endregion

                //Dispose Factory and List Form
                if (!GFunc.IsNE(this.list_CloseMSTForm))
                    list_CloseMSTForm.Invoke();

                if ((bool)this.objMstConFactory.Dispose() == false)
                    throw new TAException(MsgID.Common.DisposeFail);
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
            finally
            {
                if (e.Cancel == false)
                    this.objMstConFactory.Dispose();
            }
        }//Completed
        private void frmMSTCon_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                    GlobalUI.Combos_Fill(this, (int)objMstConFactory.ConstantCodeKey);

                //Set Focus Next Control
                GlobalUI.SelectNextControl(this, e);
            }
            catch (TAException ex)
            {
                Error(ex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed

        //Menu Strip Events
        private void tsbNew_Click(object sender, EventArgs e)
        {
            try
            {
                this.New_Process();
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed
        private void tsbSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Save_Process();
                
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Delete_Process();
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed
        private void tsbClear_Click(object sender, EventArgs e)
        {
            try
            {
                Clear_Process();
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }//Completed
        private void tsbList_Click(object sender, EventArgs e)
        {
            try
            {
                if (GFunc.IsNE(fMSTConList))
                {
                    fMSTConList = new frmList(objMstConFactory.ConstantCodeKey, objMstConFactory.PermID);

                    //Attach events to this FORM to call events in frmList
                    this.list_CloseMSTForm += new GVar.ListEvent_CloseFORM(fMSTConList.OnCaller_Close);
                    this.ListEvent_RefreshRecord += new GVar.ListEvent_RefreshRecord(fMSTConList.OnCaller_Changed);

                    //Attach events to frmList to call events in this FORM
                    fMSTConList.ListEvent_OpenRecord = new GVar.ListEvent_OpenRecord(this.OnListRecordSelected);
                    fMSTConList.ListEvent_CloseFORM = new GVar.ListEvent_CloseFORM(this.OnList_FormClose);
                    fMSTConList.MdiParent = frmMain.gfrmMain;
                    fMSTConList.Show();
                }
                else
                    fMSTConList.Activate();
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
            
        }//Completed
        private void btnAttachmentEdit_Click(object sender, EventArgs e)
        {
            try
            {
                frmAttachment f = new frmAttachment(objMstConFactory.ObjMSTCon.Attachments, (int)objMstConFactory.ConstantCodeKey, objMstConFactory.ObjMSTCon.ConKey, -1, 0);
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.Yes)
                {
                    if (objMstConFactory.ObjMSTCon.CAttachment == false)
                    {
                        CAttachment.Checked = true;
                        objMstConFactory.ObjMSTCon.CAttachment = true;
                    }
                }
                else
                {
                    if (objMstConFactory.ObjMSTCon.CAttachment == true)
                    {
                        CAttachment.Checked = false;
                        objMstConFactory.ObjMSTCon.CAttachment = false;
                    }
                }
                this.btnCAttachmentEdit.Text = "(" + objMstConFactory.ObjMSTCon.Attachments.Count + ")";
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed
        private void btnVAttachmentEdit_Click(object sender, EventArgs e)
        {
            try
            {
                frmAttachment f = new frmAttachment(objMstConFactory.ObjMSTCon.Attachments, (int)objMstConFactory.ConstantCodeKey, objMstConFactory.ObjMSTCon.ConKey, -1, 0);
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.Yes)
                {
                    if (objMstConFactory.ObjMSTCon.VAttachment == false)
                    {
                        VAttachment.Checked = true;
                        objMstConFactory.ObjMSTCon.VAttachment = true;
                    }
                }
                else
                {
                    if (objMstConFactory.ObjMSTCon.VAttachment == true)
                    {
                        VAttachment.Checked = false;
                        objMstConFactory.ObjMSTCon.VAttachment = false;
                    }
                }
                this.btnVAttachmentEdit.Text = "(" + objMstConFactory.ObjMSTCon.Attachments.Count + ")";
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed
     
        //List form invoke method
        private void OnListRecordSelected(int key)
        {
            //This method will be invoked by list form, when one record is selected from list to edit
            //Also use by OpenID
            try
            {
                if (this.OpenRecord(key, string.Empty))
                    this.Focus();
                else
                    fMSTConList.Focus();
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.Focus();
            }
        }//Completed
        private void OnList_FormClose()
        {
            fMSTConList = null;
            this.list_CloseMSTForm = null;
            this.ListEvent_RefreshRecord = null;
        }//Completed

        //Formating, Locking, Refreshing
        private void Refresh_All(bool IncludeDependent)
        {
            try
            {
                Addr_Clear();
                Refresh_Header(IncludeDependent);
                //FormBindingSource_Set();
                Refresh_GridContact();
                Refresh_GridAddress(IncludeDependent,true);
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            
            
        }//Completed
        private void Refresh_Header(bool IncludeDependent)
        {
            try
            {
                bdsConInfo.DataSource = objMstConFactory.ObjMSTCon;
                bdsConInfo.ResetBindings(false);
                if (IncludeDependent)
                    this.Refresh_DependentText(string.Empty);
               
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            
        }//Completed
        private void Refresh_GridAddress(bool IncludeDependent,bool fillAddr)
        {
            try
            {
                tagrdAddr.DataSource = objMstConFactory.ObjREFAddrs;
                tagrdAddr.Rows.Refresh(RefreshRow.ReloadData);
                if (IncludeDependent)
                    Refresh_DependentComboAddr();

                if(fillAddr)
                    Fill_Addr();
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            
           
        }//Completed
        private void Refresh_GridContact()
        {
            try
            {
                tagrdREFContact.DataSource = objMstConFactory.ObjREFContactInfors;
                tagrdREFContact.Rows.Refresh(RefreshRow.ReloadData);

                tagrdNewContact.DataSource = objMstConFactory.NewContacts;
                tagrdNewContact.Rows.Refresh(RefreshRow.ReloadData);
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
               throw Error(ex, false);
            }
           

        }//Completed
        private void Refresh_DependentText(string controlNm)
        {
            try
            {
                //If controlNm is Empty, it will refresh all control, else it will only refresh that control only
                //retain the factory isdirty state as we do not want to change due to propertychange event
                bool FactoryIsDirty = objMstConFactory.IsDirty;

                MSTAcc objAcc;

                #region AR Account
                if (GFunc.CompareString(controlNm , "CAccDes") || controlNm == string.Empty)
                {
                    if (GFunc.IsNE(objMstConFactory.ObjMSTCon.CAccKey) == false)
                    {
                        objAcc = MSTAcc.Get(objMstConFactory.ObjMSTCon.CAccKey);
                        CAccDes.SetValueTrigger(objAcc.AccDes, false);
                        objAcc = null;
                    }
                    else
                    {
                        CAccDes.SetValueTrigger(string.Empty, false);
                    }
                }
                #endregion

                #region AP Account
                if (GFunc.CompareString(controlNm , "VAccDes") || controlNm == string.Empty)
                {
                    if (GFunc.IsNE(objMstConFactory.ObjMSTCon.VAccKey) == false)
                    {
                        objAcc = MSTAcc.Get(objMstConFactory.ObjMSTCon.VAccKey);
                        VAccDes.SetValueTrigger(objAcc.AccDes, false);
                        objAcc = null;
                    }
                    else
                    {
                        VAccDes.SetValueTrigger(string.Empty,false);
                    }
                }
                #endregion

                objMstConFactory.IsDirty = FactoryIsDirty;
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            

        }//Completed
        private void Refresh_DependentComboAddr()
        {
            string listID;
            try
            {
                if (ContextMenuSetting == string.Empty)
                {
                    //Setup FORM control/grid format, menu, listID
                    GlobalUI.FormGrids_Set(this, (int)objMstConFactory.ConstantCodeKey, out ContextMenuSetting);
                    //GlobalUI.cmnuGlobal_Set(this);
                    ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objMstConFactory.ConstantCodeKey);
                    GlobalUI.Combos_Fill(this, (int)objMstConFactory.ConstantCodeKey);
                }

                this.CDefaultBillAddr.DataSource = objMstConFactory.ObjREFAddrs.Copy();
                listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, CDefaultBillAddr.Name);
                GlobalUI.BindComboValue(CDefaultBillAddr, listID);

                this.CDefaultShipAddr.DataSource = objMstConFactory.ObjREFAddrs.Copy();
                listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, CDefaultShipAddr.Name);
                GlobalUI.BindComboValue(CDefaultShipAddr, listID);

                this.CDefaultStateAddr.DataSource = objMstConFactory.ObjREFAddrs.Copy();
                listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, CDefaultStateAddr.Name);
                GlobalUI.BindComboValue(CDefaultStateAddr, listID);

                this.VDefaultBillAddr.DataSource = objMstConFactory.ObjREFAddrs.Copy();
                listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, VDefaultBillAddr.Name);
                GlobalUI.BindComboValue(VDefaultBillAddr, listID);

                this.VDefaultShipAddr.DataSource = objMstConFactory.ObjREFAddrs.Copy();
                listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, VDefaultShipAddr.Name);
                GlobalUI.BindComboValue(VDefaultShipAddr, listID);
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            
        }//Completed
        private void FormLayout()
        {
            bool EnableMode = !this.objMstConFactory.IsReadOnly;
            this.tslReadOnly.Text = !EnableMode ? "Read Only" : string.Empty;

            ControlLock();

            if (EnableMode == false)
            {
                this.tsbSave.Enabled = false;
                this.tsbDelete.Enabled = false;
                this.tsbClear.Enabled = false;
            }
            else
            {
                this.tsbSave.Enabled = true;
                if (this.objMstConFactory.IsNew)
                {
                    this.tsbClear.Enabled = true;
                    this.tsbDelete.Enabled = false;
                }
                else
                {
                    this.tsbClear.Enabled = false;
                    this.tsbDelete.Enabled = true;
                }
            }
        }
        private void ControlLock()
        {
            bool runCust = false;
            bool runVend = false;
            int conType = GFunc.NEInt(this.ConType.Value, 0);
            
            #region Get process to run
            switch (conType)
            {
                case 10:   //Customer
                    //case 40:    //Prospect //Non Trade displays both
                    runCust = true;
                    break;

                case 20:    //Vendor
                //case 40:    // Non Trade displays Vendor only suggested by May Thet Htar Aung in Finance Department /* added by YST on 2021/04/16 */
                    runVend = true;
                    break;

                default:    //Both
                    runCust = true;
                    runVend = true;
                    break;
            }
            #endregion

            #region Disable Tab base on Process to run
            if (runCust)
            {
                tabDetail.Tabs["Customer"].Enabled = true;
                
            }
            else
            {
                tabDetail.Tabs["Customer"].Enabled = false;
                
            }

            if (runVend)
                tabDetail.Tabs["Vendor"].Enabled = true;
            else
                tabDetail.Tabs["Vendor"].Enabled = false;
            #endregion

            bool EnableMode = !this.objMstConFactory.IsReadOnly;
            this.tslReadOnly.Text = !EnableMode ? "Read Only" : string.Empty;

            //Special Condition for RecordID
            this.ConID.Enabled = EnableMode;
            this.ConID.ReadOnly = !canEditRecordID;           

            #region Set Enable mode for all header controls
            this.CAccDes.Enabled = EnableMode;
            this.CAccKey.Enabled = EnableMode;
            this.CBranchKey.Enabled = EnableMode;
            this.CCBType.Enabled = EnableMode;
            this.CClass.Enabled = EnableMode;
            this.CCreditLimit.Enabled = EnableMode;
            this.CCurrkey.Enabled = EnableMode;
            this.CDefaultBillAddr.Enabled = EnableMode;
            this.CDefaultContact.Enabled = EnableMode;
            this.CDefaultContactState.Enabled = EnableMode;
            this.CDefaultShipAddr.Enabled = EnableMode;
            this.CDefaultStateAddr.Enabled = EnableMode;
            this.CDefaultStateType.Enabled = EnableMode;
            this.CDeptKey.Enabled = EnableMode;
            this.CEMKey.Enabled = EnableMode;
            this.CGrpKey.Enabled = EnableMode;
            this.CIndustryKey.Enabled = EnableMode;
            this.ConBirthday.Enabled = EnableMode;
            this.ConChildren.Enabled = EnableMode;
            this.ConGender.Enabled = EnableMode;
            this.ConMarital.Enabled = EnableMode;
            this.ConNamFirst.Enabled = EnableMode;
            this.ConNamInitials.Enabled = EnableMode;
            this.ConNamLast.Enabled = EnableMode;
            this.ConNamMiddle.Enabled = EnableMode;
            this.ConNationality.Enabled = EnableMode;
            this.ConNm.Enabled = EnableMode;
            this.ConSocSecNo.Enabled = EnableMode;
            this.ConType.Enabled = EnableMode;
            this.CPriceType.Enabled = EnableMode;
            this.CRem.Enabled = EnableMode;
            this.FormerKnownAs.Enabled = EnableMode;
            this.CRemDelivery.Enabled = EnableMode;
            this.CRemPayment.Enabled = EnableMode;
            this.CRemPrice.Enabled = EnableMode;
            this.CRemValidity.Enabled = EnableMode;
            this.CTaxGrpKey.Enabled = EnableMode;
            this.CTermKey.Enabled = EnableMode;
            this.COverallDefaultDis.Enabled = EnableMode;
            this.CTerritoryKey.Enabled = EnableMode;
            this.Custom1.Enabled = EnableMode;
            this.Custom2.Enabled = EnableMode;
            this.Custom3.Enabled = EnableMode;
            this.Custom4.Enabled = EnableMode;
            this.Custom5.Enabled = EnableMode;
            this.CustomerSinceDate.Enabled = EnableMode;            
            this.NoFinCharge.Enabled = EnableMode;
            this.OccuGroup.Enabled = EnableMode;
            this.OccuIndustry.Enabled = EnableMode;
            this.OccuSalary.Enabled = EnableMode;
            this.OccuTitle.Enabled = EnableMode;
            this.VAccDes.Enabled = EnableMode;
            this.VAccKey.Enabled = EnableMode;
            this.VBranchKey.Enabled = EnableMode;
            this.VClass.Enabled = EnableMode;
            this.VCreditLimit.Enabled = EnableMode;
            this.VCurrkey.Enabled = EnableMode;
            this.VDefaultBillAddr.Enabled = EnableMode;
            this.VDefaultContact.Enabled = EnableMode;
            this.VDefaultAPPYDocType.Enabled = EnableMode && (conType==40); //Mic Check; Jack Added 9 Nov 2012; Only editable for Non Trade
            this.VDefaultShipAddr.Enabled = EnableMode;
            this.VDeptKey.Enabled = EnableMode;
            this.VEMKey.Enabled = EnableMode;
            this.VendorSinceDate.Enabled = EnableMode;
            this.VGrpKey.Enabled = EnableMode;
            this.VIndustryKey.Enabled = EnableMode;
            this.VPriceType.Enabled = EnableMode;
            this.VRem.Enabled = EnableMode;
            this.VRemDelivery.Enabled = EnableMode;
            this.VRemPayment.Enabled = EnableMode;
            this.VRemPrice.Enabled = EnableMode;
            this.VRemValidity.Enabled = EnableMode;
            this.VTaxGrpKey.Enabled = EnableMode;
            this.VTermKey.Enabled = EnableMode;
            this.VOverallDefaultDis.Enabled = EnableMode;
            this.VTerritoryKey.Enabled = EnableMode;
            this.btnCAttachmentEdit.Enabled = EnableMode;
            this.btnVAttachmentEdit.Enabled = EnableMode;

            /* commented by YST on 2024/03/14
            //added by NNT
            if (objMstConFactory.ObjMSTCon.Rejected == true)
                this.Inactive.Enabled = false;            
            else
                this.Inactive.Enabled = EnableMode;
            this.ActiveWithProblem.Enabled = EnableMode;
            //this.COOApprovalRequired.Enabled = EnableMode;
            //end by NNT
            */

            /* Check Permission - //added by YST on 2024/03/14 , suggested by Auditor & Su San */
            bool AllowEdit = SECPermUtility.Perform("CVEditInactive", false);
            this.Inactive.Enabled = objMstConFactory.ObjMSTCon.Rejected == true ? false : EnableMode && AllowEdit;
            this.ActiveWithProblem.Enabled = EnableMode && AllowEdit;
            this.SalesRepIsHeadSales.Enabled = EnableMode && AllowEdit;
            //end by YST 

            #endregion

            if (EnableMode == false)
            {
                #region disable grid/button
                foreach (UltraGridColumn gcol in tagrdREFContact.DisplayLayout.Bands[0].Columns)
                {
                    switch (gcol.Key.ToLower())
                    {
                        case "contactlinktype":
                        case "contactlinkkey":
                        case "createdate":
                        case "createuserkey":
                        case "lastmodifieddate":
                        case "lastmodifieduserkey":
                            break;

                        default:
                            gcol.CellActivation = Activation.ActivateOnly;
                            break;
                    }
                }
                this.tagrdREFContact.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
                this.tagrdREFContact.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                this.tsbSave.Enabled = false;
                this.tsbDelete.Enabled = false;
                this.tsbClear.Enabled = false;
                #endregion
            }
            else
            {
                #region Enable grid/button
                foreach (UltraGridColumn gcol in tagrdREFContact.DisplayLayout.Bands[0].Columns)
                {
                    switch (gcol.Key.ToLower())
                    {
                        case "contactlinktype":
                        case "contactlinkkey":
                        case "createdate":
                        case "createuserkey":
                        case "lastmodifieddate":
                        case "lastmodifieduserkey":
                            break;

                        default:

                            gcol.CellActivation = Activation.AllowEdit;
                            break;
                    }
                }
                this.tagrdREFContact.DisplayLayout.Override.AllowDelete = DefaultableBoolean.True;
                this.tagrdREFContact.DisplayLayout.Override.AllowAddNew = AllowAddNew.TemplateOnBottom;

                this.tsbSave.Enabled = true;
                if (this.objMstConFactory.IsNew)
                {
                    this.tsbClear.Enabled = true;
                    this.tsbDelete.Enabled = false;
                }
                else
                {
                    this.tsbClear.Enabled = false;
                    this.tsbDelete.Enabled = true;
                }
                #endregion
            }
            this.Refresh();
        }

        //Functions
        private bool New_Process()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (form_CanValidate() == false)
                {
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show("Validation Failed, Discard changes?",
                                                    GEnum.MsgBoxIcon.Question,
                                                    GEnum.MsgBoxButton.Yes,
                                                    GEnum.MsgBoxButton.No);

                    if (btnSelect == GEnum.MsgBoxButton.No)
                    {
                        return false;
                    }
                    else
                    {
                        this.objMstConFactory.IsDirty = false;
                        IsGridsDirty(true);
                    }
                }

                this.errorProvider1.Clear();

                if (this.objMstConFactory.IsDirty)
                {
                    if (Save_Process() == false)
                        return false;
                }

                if (this.objMstConFactory.New() == false)
                {                  
                    return false;
                }
                else
                {                 
                    this.errorProvider1.Clear();                 
                    this.ConID.Focus();                   
                    return true;
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            finally
            {
                this.Refresh_All(true);
                this.FormLayout();
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private bool SaveChanges()
        {
            try
            {
                if (form_CanValidate() == false)
                    return false;

                if (objMstConFactory.IsDirty)
                {
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show(MsgID.Common.SaveChanges,
                                          GEnum.MsgBoxIcon.Question,
                                          GEnum.MsgBoxButton.Save_Changes,
                                          GEnum.MsgBoxButton.Discard_Changes,
                                          GEnum.MsgBoxButton.I_Dont_Know);

                    if (btnSelect == GEnum.MsgBoxButton.Save_Changes)
                    {                      
                        return this.Save_Process();
                    }
                    else if (btnSelect == GEnum.MsgBoxButton.I_Dont_Know)
                    {
                        if (formClose)
                            formClose = false;

                        return false;
                    }
                }

               
                this.errorProvider1.Clear();
                return true;
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private bool Save_Process()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //Perform Validation
                if (form_CanValidate() == false)
                    return false;

                
                if(isDirtyAddr)
                    Addr_Save();//Saving Address Info

                PriceTypeCurrCheck();

                //Perform Saving
                if (this.objMstConFactory.Save())
                {
                   
                    if (GFunc.IsNE(this.ListEvent_RefreshRecord) == false)
                        ListEvent_RefreshRecord.Invoke();
                                       
                    return true;
                }
                else
                {
                    throw new TAException(MsgID.Common.SaveFail);
                }

                //FormBindingSource_Set();

            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            finally
            {
                this.Refresh_All(true);
                this.FormLayout();
                this.Cursor = Cursors.Default;
            }
        }//Completed
        public bool OpenRecord(int key, string id)
        {
            IsEdit = true;
            openrecordNormal = true;
            this.Cursor = Cursors.WaitCursor;
            GEnum.MsgBoxButton btnSelect = GEnum.MsgBoxButton.Yes;
            COOApprovalRequired.Enabled = false;
            try
            {
                if (this.SaveChanges() == false)
                    return false;
                //added condition for check approval period, if the record in approval, the record will be in read only mode
                //by nnt on 2019 April

                if (SECPermUtility.Edit(objMstConFactory.PermID, false) || objMstConFactory.ObjMSTCon.Approval == true)
                {
                    if (objMstConFactory.GetEdit(key, id) == false || objMstConFactory.ObjMSTCon.Approval == true)
                    {
                        if (SysOptionUtility.GetBool("WarnOpenRecordAsReadOnly"))
                        {
                            btnSelect = MsgBox.Show("Try to open record as read only",
                                                  GEnum.MsgBoxIcon.Question,
                                                  GEnum.MsgBoxButton.Yes,
                                                  GEnum.MsgBoxButton.No);

                            if (btnSelect == GEnum.MsgBoxButton.Yes)
                            {
                                objMstConFactory.GetReadOnly(key, id);
                                openrecordNormal = true;
                            }
                            else
                            {
                                openrecordNormal = false;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //Check if user has permission to enter credit limit amount
                        if (SECPermUtility.Perform(GVar.PermissionID.Edit_Credit_Limit, false) == false)
                        {
                            CCreditLimit.ReadOnly = true;                           
                            if (objMstConFactory.ObjMSTCon.Rejected == true) Inactive.Enabled = false;
                        }
                        else
                        {
                            CCreditLimit.ReadOnly = false;                            
                            if (objMstConFactory.ObjMSTCon.Rejected == true) Inactive.Enabled = false;
                        }

                        if (objMstConFactory.ObjMSTCon.Rejected == true) Inactive.Enabled = false; 
                    }                 
                }
                else
                    objMstConFactory.GetReadOnly(key, id);                

                //not completed -- need to check contype
                this.btnCAttachmentEdit.Text = "(" + objMstConFactory.ObjMSTCon.Attachments.Count + ")";
                this.btnVAttachmentEdit.Text = "(" + objMstConFactory.ObjMSTCon.Attachments.Count + ")";

                return true;
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            finally
            {
                //we will always need to refresh header and detail regardless if it is GetEdit, GetReadOnly, Restore old data
                if (openrecordNormal)
                {
                    Refresh_All(true);
                    FormLayout();                    
                    this.Cursor = Cursors.Default;
                }
                else
                {

                    this.Focus();
                    this.Cursor = Cursors.Default;
                }
            }
        }//Completed
        private bool Delete_Process()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnDeleteRecord))
                {
                    //Ask Confirmation for Delete
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know);

                    if (btnSelect != GEnum.MsgBoxButton.Delete)
                        return false;
                }

                if (this.objMstConFactory.Delete())
                {
                    IsGridsDirty(true);
                    if (!GFunc.IsNE(this.ListEvent_RefreshRecord))
                        ListEvent_RefreshRecord.Invoke();

                    this.objMstConFactory.New();                 
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            finally
            {
                this.Refresh_All(true);
                this.FormLayout();
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private bool Clear_Process()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (GFunc.IsNEZ(this.objMstConFactory.ObjMSTCon.ConKey))
                {
                    if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnClearRecord))
                    {
                        //Ask Confirmation for Delete
                        GEnum.MsgBoxButton btnSelect;
                        btnSelect = MsgBox.Show(MsgID.Common.ConfirmClear,
                                              GEnum.MsgBoxIcon.Question,
                                              GEnum.MsgBoxButton.Clear,
                                              GEnum.MsgBoxButton.Dont_Clear,
                                              GEnum.MsgBoxButton.I_Dont_Know);

                        if (btnSelect != GEnum.MsgBoxButton.Clear)
                            return false;
                    }

                    IsGridsDirty(true);

                    if (this.objMstConFactory.New())
                    {                      
                        errorProvider1.Clear();
                        return true;
                    }
                    else
                        return false;
                }
                return false;
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            finally
            {
                this.Refresh_All(true);
                this.FormLayout();
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private bool form_CanValidate()
        {
            try
            {
                //this function check if we can validates all the header.controls
                //all grid is able to commit its changes to its datasource
                TAUtil.ControlGVar.FormValidateFail = false;
                this.errorProvider1.Clear();
                this.Validate();
                this.tagrdREFContact.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdREFContact.UpdateData();
                this.tagrdAddr.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdAddr.UpdateData();
                this.tagrdNewContact.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdNewContact.UpdateData();

                //added by thettm on 05 jun 2018 (start)
                if (BankCountry.Value !=null)
                if(
                 (
                 BankCountry.Value.ToString().Trim()== "Canada" ||
                 BankCountry.Value.ToString().Trim()== "India"
                 ) && BankAddress.Text=="")
                {
                    MessageBox.Show("Please kindly input the bank address for country "+BankCountry.Value.ToString()+"!");
                    return false;
                }

                if(DeliModeCode.Value!=null)
                {
                    if(DeliModeCodeValue.Text=="")
                    {
                        MessageBox.Show("Please kindly input for Other Delivery Mode Code Vale of "+DeliModeCode.Value.ToString()+"!");
                        return false;
                    }
                }
                //added by thettm on 05 jun 2018 (start)

                //we need to check if the active row data cannot be commited 
                //if it cannot be commited, the IsGridDirty would return a false
                //thus saving should not be perform and the user needs to be inform of the data error
                if (IsGridsDirty(false) || TAUtil.ControlGVar.FormValidateFail)
                    return false;
                else
                    return true;
            }
            catch (TAException tex)
            {
                throw Error(tex, false);                
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }

        }//Completed
        private bool IsGridsDirty(bool undoChangesInGrid)
        {
            //This function check if the grid has uncommited data in its active orw
            //it also has an option to undo those uncommited changes. 
            try
            {
                #region Contact
                if (tagrdREFContact.ActiveRow != null)
                {
                    if (tagrdREFContact.ActiveRow.DataChanged && !tagrdREFContact.ActiveRow.IsUnmodifiedTemplateAddRow)
                    {
                        if (undoChangesInGrid)
                        {
                            //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                            this.tagrdREFContact.PerformAction(UltraGridAction.UndoCell);
                            this.tagrdREFContact.PerformAction(UltraGridAction.UndoRow);
                        }
                        return true;
                    }
                }
                #endregion

                return false;
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            
        }//Completed

        //Tab Events
        private void tabDetList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (tabDetail.ActiveTab.Key.ToLower())
                {
                    case "general":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                ConNamFirst.Focus();
                                break;
                            case Keys.Up:
                                CCBType.Focus();
                                break;
                        }
                        break;
                    case "customer":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                CBranchKey.Focus();
                                break;
                            case Keys.Up:
                                CCBType.Focus();
                                break;
                        }
                        break;                    
                    case "vendor":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                VBranchKey.Focus();
                                break;
                            case Keys.Up:
                                CCBType.Focus();
                                break;
                        }
                        break;
                    case "address":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                AddrID.Focus();
                                break;
                            case Keys.Up:
                                CCBType.Focus();
                                break;
                        }
                        break;
                    case "phone":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                GlobalUI.TabKeyDownForGrid(tagrdREFContact);
                                break;
                            case Keys.Up:
                                CCBType.Focus();
                                break;
                        }
                        break;
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
            
        }
       
        //Control Events
        private void Combo_NotInListAdd(object sender, ValidationErrorEventArgs e)
        {
            try
            {
                GlobalUI.ItemNotInList(sender as TAUtil.TAComboBox, e, true, 0);
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed
        private void Combo_NotInList(object sender, ValidationErrorEventArgs e)
        {
            try
            {
                GlobalUI.ItemNotInList(sender as TAUtil.TAComboBox, e, false, null);
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed
        private void OpenID_CustomUpdate(object sender, CancelEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;
            GEnum.RecAccessType RecAccessType;
            int PopupType;

            try
            {
                if (GFunc.IsNE(OpenID.Text) == false)
                {
                    string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, OpenID.Name);
                    if (SysCode == GEnum.SystemCode.Customer)
                    {
                        RecAccessType = GEnum.RecAccessType.CustNm;
                        PopupType = (int)GEnum.PopupType.CusNm;
                    }
                    else
                    {
                        RecAccessType = GEnum.RecAccessType.VendNm;
                        PopupType = (int)GEnum.PopupType.VendNm;
                    }

                    key = GFunc.ConRecord_GetKey(RecAccessType, listSettingID, OpenID.Text, ref id, ref des, true);
                    if (GFunc.IsNEZ(key))
                    {
                        if (DocHDRUtil.EditorButton_Popup((int)objMstConFactory.ConstantCodeKey, OpenID.Text, listSettingID, PopupType, ref key, ref id, ref des) == false)
                            return;
                    }
                    OpenID.SetValueTrigger(des, false);
                    this.OnListRecordSelected(key);
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed
        private void OpenID_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;
            try
            {
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, OpenID.Name);
                if (objMstConFactory.ConstantCodeKey == GEnum.SystemCode.Customer)
                {
                    if (DocHDRUtil.EditorButton_Popup((int)objMstConFactory.ConstantCodeKey, OpenID.Text, listSettingID, (int)GEnum.PopupType.CusNm, ref key, ref id, ref des))
                    {
                        OpenID.SetValueTrigger(des, false);
                        OnListRecordSelected(key);
                    }
                }
                else if (objMstConFactory.ConstantCodeKey == GEnum.SystemCode.Vendor)
                {
                    if (DocHDRUtil.EditorButton_Popup((int)objMstConFactory.ConstantCodeKey, OpenID.Text, listSettingID, (int)GEnum.PopupType.VendNm, ref key, ref id, ref des))
                    {
                        OpenID.SetValueTrigger(des, false);
                        OnListRecordSelected(key);
                    }
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed
        private void ConType_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (ConType.Value != null)
            {   
                //if(!IsEdit)                
                objMstConFactory.SetDefaultValue();
                ControlLock();
            }           

            else
            {
                MsgBox.Show("Type cannot be empty");
                e.Cancel = true;
            }
            
        }//Completed
        private void CCBType_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (GFunc.IsNEZ(this.CCBType.Value))
            {
                MsgBox.Show("Credit/Cash/Both cannot be empty");
                e.Cancel = true;
            }

        }//Completed
        private void CCurrkey_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (GFunc.IsNEZ(this.CCurrkey.Value))
            {
                MsgBox.Show("Currency cannot be empty");
                e.Cancel = true;
            }
            else
                objMstConFactory._MSTCon.CCurrID = CCurrkey.Text;         
        }//Completed
        private void VCurrkey_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (GFunc.IsNEZ(this.VCurrkey.Value))
            {
                MsgBox.Show("Currency cannot be empty");
                e.Cancel = true;
            }
            else
                objMstConFactory._MSTCon.VCurrID = VCurrkey.Text;           
        }//Completed
        private void CCreditLimit_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (CCreditLimit.Value == null)
            {
                CCreditLimit.SetValueTrigger(0.00,false);
            }
        }//Completed
        private void VCreditLimit_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (CCreditLimit.Value == null)
            {
                CCreditLimit.SetValueTrigger(0.00, false);
            }
        }//Completed
        private void CDefaultStateType_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (GFunc.IsNEZ(this.CDefaultStateType.Value))
            {
                MsgBox.Show("Statement Type cannot be empty");
                e.Cancel = true;
            }

        }//Completed

        //Editor Controls Events
        private void RecSearchSelected(object sender, string fieldNm, int key, string id, string des)
        {
            //This is a common function used by header and grid to handle Con,Itm,Acc,Job RecordSearch
            //Currently the sender is not use (for future requirements)
            //this is because we do not have a situation where there is a conflict of fieldNm between Header and Grids
            //when this happen we will need to add another switch to check the gridName in order to resolve the conflict

            try
            {
                switch (fieldNm.ToLower())
                {
                    case "cacckey":
                    case "caccdes":
                        objMstConFactory.ObjMSTCon.CAccKey = key;
                        CAccDes.SetValueTrigger(des, false);
                        break;

                    case "vacckey":
                    case "vaccdes":
                        objMstConFactory.ObjMSTCon.VAccKey = key;
                        VAccDes.SetValueTrigger(des, false);
                        break;
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed
        private bool RecSearchProcess(object sender, string columnKey, bool FromButtonClick)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                int vendorKey = 0;
                string FieldName = string.Empty;
                string controlText = string.Empty;
                bool senderIsGrid = false;
                string listSettingID = string.Empty;
                int PopupType = 0;
                int AccessType = 0;
                string keySearch = string.Empty;

                //Get (ControlName or ColumnKey) and value
                if (sender.GetType() == typeof(TAUtil.TAGridEditor) || sender.GetType() == typeof(UltraGrid))
                {
                    FieldName = columnKey;
                    controlText = ((TAUtil.TAGridEditor)sender).ActiveCell.Text;
                    senderIsGrid = true;
                }
                else
                {
                    FieldName = ((Control)sender).Name;
                    controlText = ((Control)sender).Text;
                }

                //Get ListID
                if (senderIsGrid)
                    listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, FieldName, ((Control)sender).Name);
                else
                    listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, FieldName);

                //Get PopupType and KeySearch
                switch (FieldName.ToLower())
                {
                    case "cacckey":
                    case "vacckey":
                        PopupType = (int)GEnum.PopupType.AccID;
                        AccessType = (int)GEnum.RecAccessType.AccID;
                        keySearch = "Acc";
                        break;

                    case "caccdes":
                    case "vaccdes":
                        PopupType = (int)GEnum.PopupType.AccDes;
                        AccessType = (int)GEnum.RecAccessType.AccDes;
                        keySearch = "Acc";
                        break;
                }

                if (FromButtonClick)
                {
                    if (DocHDRUtil.EditorButton_Popup((int)objMstConFactory.ConstantCodeKey, controlText, listSettingID, PopupType, ref key, ref id, ref des))
                        RecSearchSelected(sender, FieldName, key, id, des);
                }
                else
                {
                    if (GFunc.IsNE(controlText))
                        //Clear all dependent controls
                        RecSearchSelected(sender, FieldName, key, id, des);
                    else
                    {
                        //Try to match record in server
                        //GFunc.ConRecord_GetKey and GFunc.AccRecord_GetKey is exactly the same, but i still split them up just in case
                        //the code or logic is change in the future version
                        switch (keySearch.ToLower())
                        {
                            case "con":
                                key = GFunc.ConRecord_GetKey((GEnum.RecAccessType)AccessType, listSettingID, controlText, ref id, ref des, false);
                                break;
                            case "acc":
                                key = GFunc.AccRecord_GetKey((GEnum.RecAccessType)AccessType, listSettingID, controlText, ref id, ref des, true);
                                break;
                            default:    //Itm
                                key = GFunc.ItmRecord_GetKey((GEnum.RecAccessType)AccessType, listSettingID, controlText, vendorKey, ref id, ref des, true);
                                break;

                        }
                        if (GFunc.IsNEZ(key))
                        {
                            //since value input by user cannot be match let the user select from Popup form
                            if (DocHDRUtil.EditorButton_Popup((int)objMstConFactory.ConstantCodeKey, controlText, listSettingID, PopupType, ref key, ref id, ref des))
                                RecSearchSelected(sender, FieldName, key, id, des);
                            else
                            {
                                //when user is still unable to select a matching record, undo the changes
                                MsgBox.Show("Please use a valid value");
                                return false;
                            }
                        }
                        else
                            RecSearchSelected(sender, FieldName, key, id, des);
                    }
                }
                return true;
            }
            catch (TAException tex)
            {
                Error(tex, true);
                return false;
            }
            catch (Exception ex)
            {
                Error(ex, true);
                return false;
            }
        }//Completed
        private void CAccKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            e.Cancel = !RecSearchProcess(sender, string.Empty, false);
        }//Completed
        private void VAccKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            e.Cancel = !RecSearchProcess(sender, string.Empty, false);
        }//Completed
        private void CAccDes_CustomUpdate(object sender, CancelEventArgs e)
            {
                e.Cancel = !RecSearchProcess(sender, string.Empty, false);
            }//Completed
        private void VAccDes_CustomUpdate(object sender, CancelEventArgs e)
        {
            e.Cancel = !RecSearchProcess(sender, string.Empty, false);
        }//Completed
        private void CAccKey_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            RecSearchProcess(sender, string.Empty, true);
        }//Completed
        private void VAccKey_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            RecSearchProcess(sender, string.Empty, true);
        }//Completed
        private void CAccDes_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            RecSearchProcess(sender, string.Empty, true);
        }//Completed
        private void VAccDes_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            RecSearchProcess(sender, string.Empty, true);
        }//Completed

        private void PriceTypeCurrCheck()
        {
            int? conType = objMstConFactory._MSTCon.ConType;
            if ((conType == (int)GEnum.ConType.Customer || conType == (int)GEnum.ConType.Both) && CPriceType.SelectedRow!=null)
            {
                if (GFunc.NEInt(CPriceType.SelectedRow.Cells["BuildInCode"].Value, 0) == (int)GEnum.PriceListCode.Normal)
                {
                    if(objMstConFactory._MSTCon.CCurrID != GFunc.NEStr(CPriceType.SelectedRow.Cells["CurrID"].Value, ""))
                    {
                        MsgBox.Show("Warning: Customer Currency and Price Type Currency are different.");
                    }
                }
            }

            if ((conType == (int)GEnum.ConType.Vendor || conType == (int)GEnum.ConType.Both) && VPriceType.SelectedRow != null)
            {
                if (GFunc.NEInt(VPriceType.SelectedRow.Cells["BuildInCode"].Value, 0) == (int)GEnum.PriceListCode.Normal)
                {
                    if (objMstConFactory._MSTCon.VCurrID != GFunc.NEStr(VPriceType.SelectedRow.Cells["CurrID"].Value, ""))
                    {
                        MsgBox.Show("Warning: Vendor Currency and Price Type Currency are different.");
                    }
                }
            }
        }

        //Address Info
        private void tagrdAddr_BeforeSelectChange(object sender, BeforeSelectChangeEventArgs e)
        {

            try            {

                if (isDirtyAddr)
                {
                    if (!Addr_Save())
                    {
                        return;

                    }
                }

                if (tagrdAddr.ActiveRow != null && e.NewSelections.Cells.Count != 0)
                {
                    _addrKey = GFunc.NEInt(e.NewSelections.Cells[0].Row.Cells["AddrKey"].Value, 0);
                    _addrID = e.NewSelections.Cells[0].Row.Cells["AddrID"].Value.ToString();
                    AddrID.SetValueTrigger(e.NewSelections.Cells[0].Row.Cells["AddrID"].Value.ToString(), false);
                    AddrType.SetValueTrigger(e.NewSelections.Cells[0].Row.Cells["AddrType"].Value, false);
                    AddrStreet.SetValueTrigger(e.NewSelections.Cells[0].Row.Cells["AddrStreet"].Value.ToString(), false);
                    AddrPOBox.SetValueTrigger(e.NewSelections.Cells[0].Row.Cells["AddrPOBox"].Value.ToString(), false);
                    AddrCity.SetValueTrigger(e.NewSelections.Cells[0].Row.Cells["AddrCity"].Value.ToString(), false);
                    AddrState.SetValueTrigger(e.NewSelections.Cells[0].Row.Cells["AddrState"].Value.ToString(), false);
                    AddrZipCode.SetValueTrigger(e.NewSelections.Cells[0].Row.Cells["AddrZipCode"].Value.ToString(), false);
                    AddrCountry.SetValueTrigger(e.NewSelections.Cells[0].Row.Cells["AddrCountry"].Value.ToString(), false);
                    AddrRegion.SetValueTrigger(e.NewSelections.Cells[0].Row.Cells["AddrRegion"].Value.ToString(), false);
                    AddrAttn.SetValueTrigger(e.NewSelections.Cells[0].Row.Cells["AddrAttn"].Value.ToString(), false);
                    AddrTel1.SetValueTrigger(e.NewSelections.Cells[0].Row.Cells["AddrTel1"].Value.ToString(), false);
                    AddrTel2.SetValueTrigger(e.NewSelections.Cells[0].Row.Cells["AddrTel2"].Value.ToString(), false);
                    AddrFax.SetValueTrigger(e.NewSelections.Cells[0].Row.Cells["AddrFax"].Value.ToString(), false);
                    AddrEmail.SetValueTrigger(e.NewSelections.Cells[0].Row.Cells["AddrEmail"].Value.ToString(), false);
                    AddrShipViaKey.SetValueTrigger(e.NewSelections.Cells[0].Row.Cells["AddrShipViaKey"].Value.ToString(), false);
                    AddrCustom1.SetValueTrigger(e.NewSelections.Cells[0].Row.Cells["Custom1"].Value.ToString(), false);
                    AddrCustom2.SetValueTrigger(e.NewSelections.Cells[0].Row.Cells["Custom2"].Value.ToString(), false);
                    AddrCustom3.SetValueTrigger(e.NewSelections.Cells[0].Row.Cells["Custom3"].Value.ToString(), false);

                    isNewAddr = false;
                }
            }
            catch (TAException tex)
            {
                e.Cancel = true;
                Error(tex, true);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                Error(ex, true);
            }


        }
        private void Addr_CustomUpdate(object sender, CancelEventArgs e)
        {
            isDirtyAddr = true;
            objMstConFactory.IsDirty = true;
            try
            {
                if (sender.GetType() == typeof(TAUtil.TATextBoxEditor))
                    ((TAUtil.TATextBoxEditor)sender).SetValueTrigger(((TAUtil.TATextBoxEditor)sender).Text.ToUpper(), false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                bool vClearAddr = true;
                if (isDirtyAddr)
                {
                    if (Addr_Save())
                        vClearAddr = true;
                    else
                    {
                        if (GEnum.MsgBoxButton.OK != MsgBox.Show("Validation failed, discard changes?", GEnum.MsgBoxButton.OK, GEnum.MsgBoxButton.Cancel))
                        {
                           vClearAddr = false;
                        }                     
                    }
                }

                if (vClearAddr)
                {
                    errorProvider1.Clear();
                    Addr_Clear();
                    AddrID.Focus();
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Addr_Save();
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable addr = tagrdAddr.DataSource as DataTable;
                DataRow[] addrRow = addr.Select("AddrID='" + _addrID + "'");
                                
                if (addrRow != null && addrRow.Length != 0)
                {
                    //Use _addrKey to get the AddrID from the Data
                    string addrID = addrRow[0]["AddrID"].ToString();
                    //Check if the address is used in somewhere else
                    if (GFunc.NEStr(VDefaultBillAddr.Value,"") == addrID || GFunc.NEStr(CDefaultBillAddr.Value,"") == addrID || GFunc.NEStr(VDefaultShipAddr.Value,"") == addrID || GFunc.NEStr(CDefaultShipAddr.Value,"") == addrID || GFunc.NEStr(CDefaultStateAddr.Value,"") == addrID)
                    {
                        MsgBox.Show("Current Address is in used and cannot be deleted!");
                        return;
                    }
                    else
                    {
                        objMstConFactory.ObjREFAddrs.Rows.Remove(addrRow[0]);
                        objMstConFactory.IsDirty = true;
                    }
                }

                //We need to clear the addr when it is deleted or when the user use the delete button to clear unsave addr
                Refresh_GridAddress(true, false);
                Addr_Clear();
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }

        }

        private void Addr_Clear()
        {
            _addrKey = 0;
            _addrID = string.Empty;
            AddrID.SetValueTrigger(string.Empty, false);
            AddrType.SetValueTrigger(string.Empty, false);
            AddrStreet.SetValueTrigger(string.Empty, false);
            AddrPOBox.SetValueTrigger(string.Empty, false);
            AddrCity.SetValueTrigger(string.Empty, false);
            AddrState.SetValueTrigger(string.Empty, false);
            AddrZipCode.SetValueTrigger(string.Empty, false);
            AddrCountry.SetValueTrigger(string.Empty, false);
            AddrRegion.SetValueTrigger(string.Empty, false);
            AddrAttn.SetValueTrigger(string.Empty, false);
            AddrTel1.SetValueTrigger(string.Empty, false);
            AddrTel2.SetValueTrigger(string.Empty, false);
            AddrFax.SetValueTrigger(string.Empty, false);
            AddrEmail.SetValueTrigger(string.Empty, false);
            AddrShipViaKey.SetValueTrigger(string.Empty, false);
            AddrCustom1.SetValueTrigger(string.Empty, false);
            AddrCustom2.SetValueTrigger(string.Empty, false);
            AddrCustom3.SetValueTrigger(string.Empty, false);
            isNewAddr = true;
            isDirtyAddr = false;
            tagrdAddr.Selected.Cells.Clear();
            tagrdAddr.ActiveRow = null;
            //form_CanValidate();//this will clear the error provider if all other control in the header do not contains error
            
            TAGlobal.ClearErrorIcon(AddrID);
            TAGlobal.ClearErrorIcon(AddrType);
            TAGlobal.ClearErrorIcon(AddrStreet);
            TAGlobal.ClearErrorIcon(AddrPOBox);
            TAGlobal.ClearErrorIcon(AddrCity);
            TAGlobal.ClearErrorIcon(AddrState);
            TAGlobal.ClearErrorIcon(AddrZipCode);
            TAGlobal.ClearErrorIcon(AddrCountry);
            TAGlobal.ClearErrorIcon(AddrRegion);
            TAGlobal.ClearErrorIcon(AddrAttn);
            TAGlobal.ClearErrorIcon(AddrTel1);
            TAGlobal.ClearErrorIcon(AddrTel2);
            TAGlobal.ClearErrorIcon(AddrFax);
            TAGlobal.ClearErrorIcon(AddrEmail);
            TAGlobal.ClearErrorIcon(AddrShipViaKey);
            TAGlobal.ClearErrorIcon(AddrCustom1);
            TAGlobal.ClearErrorIcon(AddrCustom2);
            TAGlobal.ClearErrorIcon(AddrCustom3);



        }
        private bool Addr_Save()
        {
            DataTable addr = tagrdAddr.DataSource as DataTable;
            bool processOK = true;
            bool failonError = false;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());

            try
            {
                ErrorNotifier_Clear(this, e);

                //Validation
                BaseUtility.Validation(AddrID.Text, "AddrID", "AddrID", GEnum.DataType.String, GEnum.Require.Yes, 50,GEnum.CompareOperator.NotEqual,string.Empty, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(AddrType.Value, "AddrType", "AddrType", GEnum.DataType.String, GEnum.Require.No, 50, null, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(AddrStreet.Text, "AddrStreet", "AddrStreet", GEnum.DataType.String, GEnum.Require.Yes, 255, null, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(AddrPOBox.Text, "AddrPOBox", "AddrPOBox", GEnum.DataType.String, GEnum.Require.No, 50, null, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(AddrCity.Text, "AddrCity", "AddrCity", GEnum.DataType.String, GEnum.Require.No, 50, null, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(AddrState.Text, "AddrState", "AddrState", GEnum.DataType.String, GEnum.Require.No, 50, null, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(AddrZipCode.Text, "AddrZipCode", "AddrZipCode", GEnum.DataType.String, GEnum.Require.No, 50, null, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(AddrCountry.Text, "AddrCountry", "AddrCountry", GEnum.DataType.String, GEnum.Require.No, 50, null, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(AddrRegion.Text, "AddrRegion", "AddrRegion", GEnum.DataType.String, GEnum.Require.No, 50, null, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(AddrAttn.Text, "AddrAttn", "AddrAttn", GEnum.DataType.String, GEnum.Require.No, 50, null, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(AddrTel1.Text, "AddrTel1", "AddrTel1", GEnum.DataType.String, GEnum.Require.No, 50, null, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(AddrTel2.Text, "AddrTel2", "AddrTel2", GEnum.DataType.String, GEnum.Require.No, 50, null, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(AddrFax.Text, "AddrFax", "AddrFax", GEnum.DataType.String, GEnum.Require.No, 50, null, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(AddrEmail.Text, "AddrEmail", "AddrEmail", GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(AddrShipViaKey.Value, "AddrShipViaKey", "AddrShipViaKey", GEnum.DataType.Integer, GEnum.Require.No, null, null, null, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(Custom1.Text, "Custom1", "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(Custom2.Text, "Custom2", "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                BaseUtility.Validation(Custom3.Text, "Custom3", "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);

                ErrorNotifier_Set(this, e);

                if (!processOK)
                    return false;



                if (isNewAddr)
                {
                    DataRow addrRow = addr.NewRow();

                    addrRow["AddrID"] = AddrID.Text;
                    addrRow["AddrType"] = AddrType.Value;
                    addrRow["AddrStreet"] = AddrStreet.Text;
                    addrRow["AddrPOBox"] = AddrPOBox.Text;
                    addrRow["AddrCity"] = AddrCity.Value;
                    addrRow["AddrState"] = AddrState.Value;
                    addrRow["AddrCountry"] = AddrCountry.Value;
                    addrRow["AddrZipCode"] = AddrZipCode.Text;
                    addrRow["AddrRegion"] = AddrRegion.Value;
                    addrRow["AddrAttn"] = AddrAttn.Text;
                    addrRow["AddrTel1"] = AddrTel1.Text;
                    addrRow["AddrTel2"] = AddrTel2.Text;
                    addrRow["AddrFax"] = AddrFax.Text;
                    addrRow["AddrEmail"] = AddrEmail.Text;
                    addrRow["AddrShipViaKey"] = GFunc.NEInt(AddrShipViaKey.Value, 0);
                    addrRow["Custom1"] = AddrCustom1.Text;
                    addrRow["Custom2"] = AddrCustom2.Text;
                    addrRow["Custom3"] = AddrCustom3.Text;


                    objMstConFactory.ObjREFAddrs.Rows.Add(addrRow);
                    isNewAddr = false;
                }
                else
                {
                    DataRow[] addrRow = addr.Select("AddrKey=" + _addrKey);

                    if (addrRow != null && addrRow.Length == 0)//updated by ID if record havn't save in server 
                    {
                        addrRow = addr.Select("AddrID='" + AddrID.Text + "'");
                    }

                    if (addrRow != null && addrRow.Length != 0)
                    {
                        addrRow[0]["AddrID"] = AddrID.Text;
                        addrRow[0]["AddrType"] = AddrType.Value;
                        addrRow[0]["AddrStreet"] = AddrStreet.Text;
                        addrRow[0]["AddrPOBox"] = AddrPOBox.Text;
                        addrRow[0]["AddrCity"] = AddrCity.Value;
                        addrRow[0]["AddrState"] = AddrState.Value;
                        addrRow[0]["AddrCountry"] = AddrCountry.Value;
                        addrRow[0]["AddrZipCode"] = AddrZipCode.Text;
                        addrRow[0]["AddrRegion"] = AddrRegion.Value;
                        addrRow[0]["AddrAttn"] = AddrAttn.Text;
                        addrRow[0]["AddrTel1"] = AddrTel1.Text;
                        addrRow[0]["AddrTel2"] = AddrTel2.Text;
                        addrRow[0]["AddrFax"] = AddrFax.Text;
                        addrRow[0]["AddrEmail"] = AddrEmail.Text;
                        addrRow[0]["AddrShipViaKey"] = GFunc.NEInt(AddrShipViaKey.Value, 0);
                        addrRow[0]["Custom1"] = AddrCustom1.Text;
                        addrRow[0]["Custom2"] = AddrCustom2.Text;
                        addrRow[0]["Custom3"] = AddrCustom3.Text;
                    }

                }

                objMstConFactory.IsDirty = true;
                isDirtyAddr = false;
                Refresh_GridAddress(true, false);
                tagrdAddr.ActiveRow = tagrdAddr.Rows[tagrdAddr.Rows.Count - 1];
                return true;
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }

            return false;
        }
        private void Fill_Addr()
        {
            try
            {
                if (tagrdAddr.Rows.Count > 0)
                {
                    tagrdAddr.Selected.Cells.Clear();
                    tagrdAddr.ActiveRow = null;
                    tagrdAddr.ActiveRow = tagrdAddr.Rows[0];
                }
                if (tagrdAddr.ActiveRow != null && tagrdAddr.ActiveRow.Cells.Count != 0)
                {
                    _addrKey = GFunc.NEInt(tagrdAddr.ActiveRow.Cells[0].Row.Cells["AddrKey"].Value, 0);
                    _addrID = tagrdAddr.ActiveRow.Cells[0].Row.Cells["AddrID"].Value.ToString();
                    AddrID.SetValueTrigger(tagrdAddr.ActiveRow.Cells[0].Row.Cells["AddrID"].Value.ToString(), false);
                    AddrType.SetValueTrigger(tagrdAddr.ActiveRow.Cells[0].Row.Cells["AddrType"].Value, false);
                    AddrStreet.SetValueTrigger(tagrdAddr.ActiveRow.Cells[0].Row.Cells["AddrStreet"].Value.ToString(), false);
                    AddrPOBox.SetValueTrigger(tagrdAddr.ActiveRow.Cells[0].Row.Cells["AddrPOBox"].Value.ToString(), false);
                    AddrCity.SetValueTrigger(tagrdAddr.ActiveRow.Cells[0].Row.Cells["AddrCity"].Value.ToString(), false);
                    AddrState.SetValueTrigger(tagrdAddr.ActiveRow.Cells[0].Row.Cells["AddrState"].Value.ToString(), false);
                    AddrZipCode.SetValueTrigger(tagrdAddr.ActiveRow.Cells[0].Row.Cells["AddrZipCode"].Value.ToString(), false);
                    AddrCountry.SetValueTrigger(tagrdAddr.ActiveRow.Cells[0].Row.Cells["AddrCountry"].Value.ToString(), false);
                    AddrRegion.SetValueTrigger(tagrdAddr.ActiveRow.Cells[0].Row.Cells["AddrRegion"].Value.ToString(), false);
                    AddrAttn.SetValueTrigger(tagrdAddr.ActiveRow.Cells[0].Row.Cells["AddrAttn"].Value.ToString(), false);
                    AddrTel1.SetValueTrigger(tagrdAddr.ActiveRow.Cells[0].Row.Cells["AddrTel1"].Value.ToString(), false);
                    AddrTel2.SetValueTrigger(tagrdAddr.ActiveRow.Cells[0].Row.Cells["AddrTel2"].Value.ToString(), false);
                    AddrFax.SetValueTrigger(tagrdAddr.ActiveRow.Cells[0].Row.Cells["AddrFax"].Value.ToString(), false);
                    AddrEmail.SetValueTrigger(tagrdAddr.ActiveRow.Cells[0].Row.Cells["AddrEmail"].Value.ToString(), false);
                    AddrShipViaKey.SetValueTrigger(tagrdAddr.ActiveRow.Cells[0].Row.Cells["AddrShipViaKey"].Value.ToString(), false);
                    AddrCustom1.SetValueTrigger(tagrdAddr.ActiveRow.Cells[0].Row.Cells["Custom1"].Value.ToString(), false);
                    AddrCustom2.SetValueTrigger(tagrdAddr.ActiveRow.Cells[0].Row.Cells["Custom2"].Value.ToString(), false);
                    AddrCustom3.SetValueTrigger(tagrdAddr.ActiveRow.Cells[0].Row.Cells["Custom3"].Value.ToString(), false);

                    isNewAddr = false;
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            
        }

        private void Text_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (sender.GetType() == typeof(TAUtil.TATextBoxEditor))
                    ((TAUtil.TATextBoxEditor)sender).SetValueTrigger(((TAUtil.TATextBoxEditor)sender).Text.ToUpper(), false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Grid Events       
        private void tagrdREFContact_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
            
                UltraGridCell currentCell = tagrdREFContact.ActiveCell;
                switch (currentCell.Column.Key.ToLower())
                {

                    case "contacttype":                    
                        currentCell.Value = GFunc.NEInt(currentCell.Value, 10);
                        break;
                }

                switch (currentCell.Column.Key.ToLower())
                {
                    case "contactperson":
                    case "contacttype":
                    case "contactnum":
                    case "custom1":
                    case "custom2":
                    case "custom3":
                        if (objMstConFactory.Validation_Detail(tagrdREFContact.Name, tagrdREFContact.ActiveRow, currentCell.Column.Key)==false)
                            e.Cancel = true;
                        break;
                }
            }
            catch (TAException tex)
            {
                e.Cancel = true;
                Error(tex, true);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                Error(ex, true);
            }
        }//Completed
        private void tagrdREFContact_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            try
            {               
                

                if (this.tagrdREFContact.ActiveRow != null)
                {
                    if (objMstConFactory.Validation_Detail(tagrdREFContact.Name, tagrdREFContact.ActiveRow, string.Empty) == false)
                        e.Cancel = true;
                }                
            }
            catch (TAException tex)
            {
                e.Cancel = true;
                Error(tex, true);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                Error(ex, true);
            }
        }//Completed
        private void tagrdREFContact_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                e.DisplayPromptMsg = false;

                if (tagrdREFContact.Rows.Count <= 0)
                {
                    e.Cancel = true;
                    return;
                }

                if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnDeleteRecordDetail))
                {
                    if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) != GEnum.MsgBoxButton.Delete)
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                //Move the cursor position of active row index to upper row
                if (tagrdREFContact.ActiveRow.Index > 0)
                    PreRowIndex = tagrdREFContact.ActiveRow.Index - 1;
                return;
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void tagrdREFContact_AfterRowsDeleted(object sender, EventArgs e)
        {
            objMstConFactory.IsDirty = true;
            objMstConFactory.ObjREFContactInfors.AcceptChanges();
            if (tagrdREFContact.Rows.Count > 0)
            {
                tagrdREFContact.Rows[PreRowIndex].Selected = true;
                tagrdREFContact.Rows[PreRowIndex].Activate();
                PreRowIndex = 0;
            }
        }//Completed
        private void tagrdREFContact_AfterRowUpdate(object sender, RowEventArgs e)
        {
            objMstConFactory.IsDirty = true;
            objMstConFactory.ObjREFContactInfors.AcceptChanges();
        }

        //Error
        private void OnDataError(object sender, TAUtil.TAErrorEventArgs e)
        {
            try
            {
                if (e.ErrorCode == TAUtil.TAErrorCode.NOT_IN_LIST)
                {
                    if (sender.GetType() == typeof(TAUtil.TAGridEditor))
                    {
                        TAUtil.TAGridEditor grd = sender as TAUtil.TAGridEditor;
                        if (grd.ActiveCell.Column.EditorComponent != null)
                        {
                            grd.PerformAction(UltraGridAction.EnterEditMode);
                            if (grd.ActiveCell.Column.EditorComponent.GetType() == typeof(TAUtil.TAComboBox))
                            {
                                TAUtil.TAComboBox taCombo = (TAUtil.TAComboBox)grd.ActiveCell.Column.EditorComponent;
                                taCombo.Text = grd.ActiveCell.Text;

                                switch (grd.ActiveCell.Column.Key.ToLower())
                                {

                                    case "addrshipviakey":

                                        GlobalUI.ItemNotInList(grd.ActiveCell, null, 1);// ItemNotInListAdd
                                        break;
                                    case "contacttype":
                                        GlobalUI.ItemNotInList(grd.ActiveCell, null, 0); // ItemNotInList
                                        break;
                                    default:
                                        GlobalUI.ItemNotInList(grd.ActiveCell, null, 0); // ItemNotInList
                                        break;
                                }
                            }
                            else
                            {
                                GlobalUI.ItemNotInList(grd.ActiveCell, null, 0);// ItemNotInList
                            }

                        }
                    }
                }
                else if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_NUMERIC)
                {
                    throw new TAException(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
                }

                else if (e.ErrorCode == TAUtil.TAErrorCode.INTEGER_EXCEED_LIMIT)
                {
                    throw new TAException(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
                }
                else if (e.ErrorCode == TAUtil.TAErrorCode.FORMULA_NOT_RECOGNIZE)
                {
                    throw new TAException("FORMULA NOT RECOGNIZE");
                }
                else if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_DATE)
                {
                    throw new TAException("Please enter valid date.");

                }

            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }

        }//Completed
        private void ErrorNotifier_Clear(object sender, BOLib.UINotifierEventArgs e)
        {
            this.errorProvider1.Clear();
            DocComUtility.ClearErrNotifier(this, e, errorProvider1);
        }//Completed
        private void ErrorNotifier_Set(object sender, BOLib.UINotifierEventArgs e)
        {
            string propertyNm = string.Empty;
            string conNm = string.Empty;
            try
            {
                //For ErrorProvider
                foreach (object key in e.PropertyMessage.Keys)
                {
                    conNm = key.ToString();
                    Control co = this.Controls.Find(conNm, true)[0];
                    this.errorProvider1.SetError(co, e.PropertyMessage[key].ToString());
                }

                foreach (object key in e.PropertyMessage.Keys)
                {
                    switch (conNm.ToLower())
                    {
                        case "cpricetype":
                        case "ctermkey":
                        case "cindustrykey":
                        case "cbranchkey":
                        case "cgrpkey":
                        case "cdeptkey":
                        case "cterritorykey":
                        case "ctaxgrpkey":
                            tabDetail.Tabs[1].Selected = true;
                            break;
                        case "vbranchkey":
                        case "vdeptkey":
                        case "vacckey":
                        case "vpricetype":
                        case "vtermkey":
                        case "vgrpkey":
                        case "vterritorykey":
                        case "vindustrykey":
                        case "vtaxgrpkey":
                            tabDetail.Tabs[2].Selected = true;
                            break;
                        default:
                            break;
                    }
                    Control co = this.Controls.Find(conNm, true)[0];
                    co.Focus();
                    break;
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed
        private Exception Error(Exception ex, bool ShowMessage)
        {
            Exception l_tmpex = ex;
            try
            {
                if (this.ActiveControl != null)
                {
                    if (this.ActiveControl.GetType() != typeof(TAUtil.TAGridEditor))
                    {
                        l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl });
                    }
                    else
                    {
                        l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, GFunc.GridColumnKey_Get(this.ActiveControl) });
                    }
                }
                else
                {
                    l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { });
                }
                if (ShowMessage)
                {
                    SysAuditLogUtility.AddErrorLog(l_tmpex);
                }

                return l_tmpex;
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return l_tmpex;
        }
        private TAException Error(TAException ex, bool ShowMessage)
        {
            try
            {
                TAException l_tmpex = ex;
                if (this.ActiveControl != null)
                {
                    if (this.ActiveControl.GetType() != typeof(TAUtil.TAGridEditor))
                    {
                        l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl });
                    }
                    else
                    {
                        string ActiveColKey = "";
                        if (((TAUtil.TAGridEditor)this.ActiveControl).ActiveCell != null)
                        {
                            ActiveColKey = GFunc.GridColumnKey_Get(this.ActiveControl);
                        }
                        l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, ActiveColKey });
                    }
                }
                else
                {
                    l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { });
                }
                if (ShowMessage)
                {
                    SysAuditLogUtility.AddErrorLog(l_tmpex);
                }

                return l_tmpex;
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
                
        private void OpenConID_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (GFunc.IsNE(OpenConID.Text) == false)
                {                    
                    this.OnListRecordSelected(GFunc.NEInt( OpenConID.Value,0));
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }

        private void OpenConID_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;
            try
            {
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, OpenConID.Name);
                if (objMstConFactory.ConstantCodeKey == GEnum.SystemCode.Customer)
                {
                    if (DocHDRUtil.EditorButton_Popup((int)objMstConFactory.ConstantCodeKey, OpenConID.Text, listSettingID, (int)GEnum.PopupType.CusID, ref key, ref id, ref des))
                    {
                        OpenConID.SetValueTrigger(id, false);
                        OnListRecordSelected(key);
                    }
                }
                else if (objMstConFactory.ConstantCodeKey == GEnum.SystemCode.Vendor)
                {
                    if (DocHDRUtil.EditorButton_Popup((int)objMstConFactory.ConstantCodeKey, OpenConID.Text, listSettingID, (int)GEnum.PopupType.VendID, ref key, ref id, ref des))
                    {
                        OpenConID.SetValueTrigger(id, false);
                        OnListRecordSelected(key);
                    }
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }

        private void CEMKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            MSTAccTranGrp t = MSTAccTranGrp.Get(CEMKey.Text,3);
            if(t!=null)
                if (!GFunc.IsNEZ(t.TranGrpKey))
                {
                    ConChildren.SetValueTrigger(t.TranGrpKey, false);
                }
        }

        //private void toolStripButton1_Click(object sender, EventArgs e)
        //{
        //    SqlConnection conn = new SqlConnection(Database.BOSSSystemMasterConnection);
        //    SqlCommand command = new SqlCommand();
        //    command.CommandType= System.Data.CommandType.Text;
        //    command.Connection = conn;
        //    command.CommandText = "exec TESTING;";
        //    conn.Open();
        //    string message = command.ExecuteScalar().ToString();
        //    MessageBox.Show(message);
        //    conn.Close();
        //}
        private void DeliModeCode_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (DeliModeCode.Value != null)
            {
                if (DeliModeCode.Value.ToString() == "AU")
                    DeliModeCodeValue.MaxLength = 6;
                else if (DeliModeCode.Value.ToString() == "CA")
                    DeliModeCodeValue.MaxLength =9;
                else if (DeliModeCode.Value.ToString() == "CC")
                    DeliModeCodeValue.MaxLength = 9;
                else if (DeliModeCode.Value.ToString() == "CN")
                    DeliModeCodeValue.MaxLength = 30;
                else if (DeliModeCode.Value.ToString() == "FW")
                    DeliModeCodeValue.MaxLength = 9;
                else if (DeliModeCode.Value.ToString() == "IFSC")
                    DeliModeCodeValue.MaxLength = 6;
                else if (DeliModeCode.Value.ToString() == "SC")
                    DeliModeCodeValue.MaxLength = 6;
            }
        }

        private void tabDetList_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (IsEdit)
            {
                this.Refresh_All(true);
                this.FormLayout();
                //this.Cursor = Cursors.Default;
            }
            

        }

        private void tagrdNewContact_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {

        }

        private void tagrdNewContact_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {

                UltraGridCell currentCell = tagrdNewContact.ActiveCell;

                switch (currentCell.Column.Key.ToLower())
                {
                    case "contactperson":
                        currentCell.Value = currentCell.Text.ToUpper();
                        break;
                }
            }
            catch (TAException tex)
            {
                e.Cancel = true;
                Error(tex, true);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                Error(ex, true);
            }
        }

        private void BankName_ItemNotInList(object sender, ValidationErrorEventArgs e)
        {
            try
            {
                GlobalUI.ItemNotInList(sender as TAUtil.TAComboBox, e, false, null);
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }
    }
}
