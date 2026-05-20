using System;
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
using Infragistics.Win.UltraWinTabbedMdi;
using System.IO;
using TAUtil;
using System.Data.SqlClient;

namespace WinUI
{
    public partial class frmMstAcc : Form
    {
        #region Local Variables

        private BOLib.MSTAccFactory objAccFactory = null;
        string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        private GEnum.formInitMode formOpenMode = GEnum.formInitMode.Neither;
        private int recordKey = 0;
        private string recordID = string.Empty;
        private string msgID = string.Empty;
        private bool canEditRecordID = false;

        /* added by YST on 2021/09/20 Management Approval */
        string currentAccID = "";
        string approvalStatus = "";

        //only use to show Read only data viewer for AuditLog 
        private bool IsOpenFromAuditLog = false;
        private DataTable _dtHeader = null;
        private DataSet _dsDetail = null;

        frmList fMSTAccList = null;
        public GVar.ListEvent_CloseFORM list_CloseMSTForm = null;
        public GVar.ListEvent_RefreshRecord ListEvent_RefreshRecord = null;

        #endregion

        //Initialize
        public frmMstAcc()
        {
            InitializeComponent();
        }//Completed
        public frmMstAcc(string id)
        {
            //For Call from Shortcut menu (Edit/Add)
            InitializeComponent();
            recordID = id;
            MSTAcc rec = MSTAcc.Get(recordID);
            formOpenMode = GEnum.formInitMode.Add;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            tsbList.Enabled = false;
        }//Completed
        public frmMstAcc(int key)
        {
            //For call from shortcut menu (Edit)
            InitializeComponent();
            formOpenMode = GEnum.formInitMode.Edit;
            recordKey = key;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }//Completed
        public frmMstAcc(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            //Open from AuditLog as Read only data viewer
            InitializeComponent();
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
        }//Completed

        //Form Events 
        private void frmMSTAcc_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                // Initialize
                this.objAccFactory = new BOLib.MSTAccFactory(BOLib.GEnum.InstanceMode.Normal);
                if (objAccFactory.GUID <= 0)
                {
                    formClose = true;
                    return;
                }

                // Attach Event on Factory
                this.objAccFactory.ErrorNotifierHeader_Set += new GVar.UINotifierEvent(this.ErrorNotifier_Set);
                this.objAccFactory.ErrorNotifierHeader_Clear += new GVar.UINotifierEvent(this.ErrorNotifier_Clear);

                //Check if user has permission to edit Record ID
                canEditRecordID = SECPermUtility.Perform("sysRecID", false);

                if (this.IsOpenFromAuditLog)
                {
                    if (objAccFactory.SetReadOnlyData(_dtHeader, _dsDetail) == false)
                    {
                        formClose = true;
                        return;
                    }
                    Refresh_All();
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
                            this.AccID.SetValueTrigger(recordID, false);
                    }
                }

                //Setup FORM control/grid format, menu, listID
                GlobalUI.FormGrids_Set(this, (int)objAccFactory.ConstantCodeKey, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objAccFactory.ConstantCodeKey);
                GlobalUI.Combos_Fill(this, (int)objAccFactory.ConstantCodeKey);

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
        private void frmMSTAcc_Shown(object sender, EventArgs e)
        {
            try
            {
                if (formClose)
                    this.Close();
                else
                    this.AccID.Focus();
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
        private void frmMSTAcc_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool runProcess = false;
            this.formClose = true;

            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }
            if (this.formClose && objAccFactory == null)
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
                        e.Cancel = false;
                    }
                }
                #endregion

                //Dispose Factory and List Form
                if (!GFunc.IsNE(this.list_CloseMSTForm))
                    list_CloseMSTForm.Invoke();

                if ((bool)this.objAccFactory.Dispose() == false)
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
                    this.objAccFactory.Dispose();
            }
        }//Completed        
        private void frmMstAcc_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                    GlobalUI.Combos_Fill(this, (int)objAccFactory.ConstantCodeKey);

                //Set Focus Next Control
                GlobalUI.SelectNextControl(this, e);
            }
            catch (TAException ex)
            {
                Error(ex,true);               
            }
            catch (Exception ex)
            {
                Error(ex,true);
                
            }
        }//Completed

        //Menu Strip Events
        private void tsbList_Click(object sender, EventArgs e)
        {
            try
            {
                if (GFunc.IsNE(fMSTAccList))
                {
                    fMSTAccList = new frmList(objAccFactory.ConstantCodeKey, objAccFactory.PermID);

                    //Attach events to this FORM to call events in frmList
                    this.list_CloseMSTForm += new GVar.ListEvent_CloseFORM(fMSTAccList.OnCaller_Close);
                    this.ListEvent_RefreshRecord += new GVar.ListEvent_RefreshRecord(fMSTAccList.OnCaller_Changed);

                    //Attach events to frmList to call events in this FORM
                    fMSTAccList.ListEvent_OpenRecord = new GVar.ListEvent_OpenRecord(this.OnListRecordSelected);
                    fMSTAccList.ListEvent_CloseFORM = new GVar.ListEvent_CloseFORM(this.OnList_FormClose);
                    fMSTAccList.MdiParent = frmMain.gfrmMain;
                    fMSTAccList.Show();
                }
                else
                    fMSTAccList.Activate();
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
            this.formClose = true;
            this.Close();
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
                this.Delete_Process();
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
        private void tsbClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.Clear_Process();
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
                    fMSTAccList.Focus();
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
            fMSTAccList = null;
            this.list_CloseMSTForm = null;
            this.ListEvent_RefreshRecord = null;
        }//Completed

        //Formating, Locking, Refreshing
        private void Refresh_All()
        {
            try
            {
                Refresh_Header();
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
        private void Refresh_Header()
        {
            bdsAccDet.DataSource = objAccFactory.ObjMSTAcc;
            bdsAccDet.ResetBindings(false);

            currentAccID = objAccFactory.ObjMSTAcc.AccID;
            approvalStatus = objAccFactory.ObjMSTAcc.ApprovalStatus;
            if (approvalStatus.ToLower().Contains("request"))
            {
                objAccFactory.GetReadOnly(objAccFactory.ObjMSTAcc.AccKey, currentAccID);
                txtApprovalStatus.Visible = true;
                if ( approvalStatus.Split('?').Length > 1 )
                {
                    txtApprovalStatus.Value = "Waiting finance manager's approval to amend Account ID as <b>" + approvalStatus.Split('?')[1] + "</b>";
                }
                else
                {
                    txtApprovalStatus.Value = "Waiting finance manager's approval to use as new Account ID" ; 
                }
            }
            else
            {
                txtApprovalStatus.Visible = false;
            }

        }//Completed
        private void FormLayout()
        {
            try
            {
                bool EnableMode = !this.objAccFactory.IsReadOnly;
                this.tslReadOnly.Text = !EnableMode ? "Read Only" : string.Empty;

                //Special Condition for RecordID
                this.AccID.Enabled = EnableMode;
                this.AccID.ReadOnly = !canEditRecordID;

                this.AccDes.Enabled = EnableMode;
                this.AccTypeKey.Enabled = EnableMode;
                this.AccCurrKey.Enabled = EnableMode;
                this.Inactive.Enabled = EnableMode;
                this.Custom1.Enabled = EnableMode;
                this.Custom2.Enabled = EnableMode;
                this.Custom3.Enabled = EnableMode;
                this.Custom4.Enabled = EnableMode;
                this.Custom5.Enabled = EnableMode;
                this.AccGrpKey.Enabled = EnableMode;

                if (EnableMode == false)
                {
                    this.tsbSave.Enabled = false;
                    this.tsbDelete.Enabled = false;
                    this.tsbClear.Enabled = false;
                }
                else
                {
                    this.tsbSave.Enabled = true;
                    if (this.objAccFactory.IsNew)
                    {
                        this.tsbClear.Enabled = true;
                        this.tsbDelete.Enabled = false;
                        if (GFunc.NEInt(this.AccTypeKey.Value, 0) == 300 || GFunc.NEInt(this.AccTypeKey.Value, 0) == 310) //Bank or Cash
                            this.AccCurrKey.Enabled = true;
                        else
                            this.AccCurrKey.Enabled = false;
                    }
                    else
                    {
                        this.tsbClear.Enabled = false;
                        this.tsbDelete.Enabled = true;
                        this.AccTypeKey.Enabled = false;
                        this.AccCurrKey.Enabled = false;
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
                        this.objAccFactory.IsDirty = false;
                    }
                }

                this.errorProvider1.Clear();

                if (this.objAccFactory.IsDirty)
                {
                    if (Save_Process() == false)
                        return false;
                }

                if (this.objAccFactory.New() == false)
                {                  
                    return false;
                }
                else
                {                  
                    this.errorProvider1.Clear();                 
                    this.AccID.Focus();
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
                this.Refresh_All();
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

                if (objAccFactory.IsDirty)
                {
                    this.Focus();

                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show(MsgID.Common.SaveChanges,
                                          GEnum.MsgBoxIcon.Question,
                                          GEnum.MsgBoxButton.Save_Changes,
                                          GEnum.MsgBoxButton.Discard_Changes,
                                          GEnum.MsgBoxButton.I_Dont_Know);

                    if (btnSelect == GEnum.MsgBoxButton.Save_Changes)
                        return this.Save_Process();
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

                /* added by YST on 2021/09/20 for Approval */
                if (objAccFactory.IsNew)
                {
                    objAccFactory.ObjMSTAcc.ApprovalStatus = "Requested";
                    objAccFactory.ObjMSTAcc.Inactive = true;                    
                }
                else
                {
                    if (currentAccID != objAccFactory.ObjMSTAcc.AccID)
                    {                        
                        objAccFactory.ObjMSTAcc.ApprovalStatus = "Requested?" + objAccFactory.ObjMSTAcc.AccID;
                        objAccFactory.ObjMSTAcc.AccID = currentAccID;
                    }                    
                }

                //Perform Saving
                if (this.objAccFactory.Save())
                {
                    if (GFunc.IsNE(this.ListEvent_RefreshRecord) == false)
                        ListEvent_RefreshRecord.Invoke();

                    return true;
                }
                else
                {                   
                    throw new TAException(MsgID.Common.SaveFail);
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
                this.Refresh_All();
                this.FormLayout();
                this.Cursor = Cursors.Default;                
            }
        }//Completed
        public bool OpenRecord(int key, string id)
        {
            this.Cursor = Cursors.WaitCursor;
            GEnum.MsgBoxButton btnSelect = GEnum.MsgBoxButton.Yes;

            try
            {
                if (this.SaveChanges() == false)
                    return false;

                if (SECPermUtility.Edit(objAccFactory.PermID, false))
                {
                    if (objAccFactory.GetEdit(key, id) == false)
                    {
                        if (SysOptionUtility.GetBool("WarnOpenRecordAsReadOnly"))
                        {
                            btnSelect = MsgBox.Show("Try to open record as read only",
                                                  GEnum.MsgBoxIcon.Question,
                                                  GEnum.MsgBoxButton.Yes,
                                                  GEnum.MsgBoxButton.No);

                            if (btnSelect == GEnum.MsgBoxButton.Yes)
                            {
                                objAccFactory.GetReadOnly(key, id);
                            }
                        }
                    }
                }
                else
                    objAccFactory.GetReadOnly(key, id);

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
                Refresh_All();
                FormLayout();
                this.Cursor = Cursors.Default;
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

                if (this.objAccFactory.Delete())
                {
                    if (!GFunc.IsNE(this.ListEvent_RefreshRecord))
                        ListEvent_RefreshRecord.Invoke();

                    this.objAccFactory.New();                 
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
                this.Refresh_All();
                this.FormLayout();
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private bool Clear_Process()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (GFunc.IsNEZ(this.objAccFactory.ObjMSTAcc.AccKey))
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

                    if (this.objAccFactory.New())
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
                this.Refresh_All();
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
                if (TAUtil.ControlGVar.FormValidateFail)
                    return false;
                else
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

            try
            {
                if (GFunc.IsNE(OpenID.Text) == false)
                {
                    string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, OpenID.Name);
                    key = GFunc.AccRecord_GetKey(GEnum.RecAccessType.AccDes, listSettingID, OpenID.Text, ref id, ref des, true);
                    if (GFunc.IsNEZ(key))
                    {
                        if (DocHDRUtil.EditorButton_Popup((int)objAccFactory.ConstantCodeKey, OpenID.Text, listSettingID, (int)GEnum.PopupType.AccDes, ref key, ref id, ref des) == false)
                            return;
                    }
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
                if (DocHDRUtil.EditorButton_Popup((int)objAccFactory.ConstantCodeKey, OpenID.Text, listSettingID, (int)GEnum.PopupType.AccDes, ref key, ref id, ref des))
                {
                    OpenID.SetValueTrigger(des, false);
                    OnListRecordSelected(key);
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
        private void AccTypeKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if ((int)AccTypeKey.Value == (int)GEnum.AccTypeKey.Bank || (int)AccTypeKey.Value == (int)GEnum.AccTypeKey.Petty_Cash || (int)AccTypeKey.Value == (int)GEnum.AccTypeKey.Temp_Holding_Fund)
                {
                    AccCurrKey.SetValueTrigger(GFunc.NEInt(AccCurrKey.Value, 1), false);
                    AccCurrKey.Enabled = true;
                }
                else
                {
                    AccCurrKey.SetValueTrigger(1, false);
                    AccCurrKey.Enabled = false;
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

        //Event Method
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

                //For Focus
                foreach (object key in e.PropertyMessage.Keys)
                {
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

        //Error
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
                        l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, GFunc.GridColumnKey_Get(this.ActiveControl) });
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

    }
}