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
using TAUtil;


namespace WinUI
{
    public partial class frmREFAccGrp : Form
    {
        #region Local Variables
        private BOLib.REFAccGrpFactory objAccGrpFactory = null;
        private string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        private GEnum.formInitMode formOpenMode = GEnum.formInitMode.Neither;
        private int recordKey = 0;
        private string recordID = string.Empty;
        private string msgID = string.Empty;
        private bool canEditRecordID = false;
        private bool ListSyncInprogress = false;

        //only use to show Read only data viewer for AuditLog 
        private bool IsOpenFromAuditLog = false;
        private DataTable _dtHeader = null;
        private DataSet _dsDetail = null;

        #endregion

        //Initialize
        public frmREFAccGrp()
        {
            InitializeComponent();
        }//Completed
        public frmREFAccGrp(string id)
        {
            InitializeComponent();
            recordID = id;
            formOpenMode = GEnum.formInitMode.Add;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }//Completed
        public frmREFAccGrp(int Key)
        {
            //For call from shortcut menu (Edit)
            InitializeComponent();
            formOpenMode = GEnum.formInitMode.Edit;
            recordKey = Key;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }//Completed
        public frmREFAccGrp(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            //Open from AuditLog as Read only data viewer
            InitializeComponent();
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
        }//Completed

        //Form Events
        private void frmREFAccGrp_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.tagrdAccGrp.DisplayLayout.Bands[0].SortedColumns.Clear();
                //Call Initialization
                this.objAccGrpFactory = new BOLib.REFAccGrpFactory(BOLib.GEnum.InstanceMode.Normal);
                if (objAccGrpFactory.GUID <= 0)
                {
                    formClose = true;
                    return;
                }

                //Attach Event on Factory
                this.objAccGrpFactory.ErrorNotifierHeader_Set += new GVar.UINotifierEvent(this.ErrorNotifier_Set);
                this.objAccGrpFactory.ErrorNotifierHeader_Clear += new GVar.UINotifierEvent(this.ErrorNotifier_Clear);
                
                //Check if user has permission to edit Record ID
                canEditRecordID = SECPermUtility.Perform("sysRecID", false);

                if (this.IsOpenFromAuditLog)
                {
                    if (objAccGrpFactory.SetReadOnlyData(_dtHeader, _dsDetail) == false)
                    {
                        formClose = true;
                        return;
                    }
                    Refresh_Header();
                    GlobalUI.FormEnable_Set(this, false);
                }
                else
                {
                    this.New_Process();

                    //When open from shortcutmenu (edit)
                    if (formOpenMode == GEnum.formInitMode.Edit)
                        this.OpenRecord(recordKey);
                    else if (formOpenMode == GEnum.formInitMode.Add)
                    {
                        if (canEditRecordID && recordID != string.Empty)
                            this.AccGrpID.SetValueTrigger(recordID, false);
                    }
                }

                //Setup Grid Layout, ShortcutMenu, fill Combo
                GlobalUI.FormGrids_Set(this, (int)objAccGrpFactory.ConstantCodeKey, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objAccGrpFactory.ConstantCodeKey);
                //GlobalUI.cmnuGlobal_Set(this);
                GlobalUI.Combos_Fill(this, (int)objAccGrpFactory.ConstantCodeKey);

                //Setup List properties
                this.tagrdAccGrp.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                this.tagrdAccGrp.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
                this.tagrdAccGrp.DisplayLayout.Bands[0].Override.ActiveCellAppearance.BackColor = Color.Gold;

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
        private void frmREFAccGrp_Shown(object sender, EventArgs e)
        {
            //Close Form when FormLoad has encounter errors
            if (formClose)
                this.Close();
        }//Completed
        private void frmREFAccGrp_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool runProcess = false;
            this.formClose = true;

            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }
            if (this.formClose && objAccGrpFactory == null)
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

             
                //Dispose Factory
                if ((bool)this.objAccGrpFactory.Dispose() == false)
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
                    this.objAccGrpFactory.Dispose();
            }
        }//Completed
        private void frmREFAccGrp_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)objAccGrpFactory.ConstantCodeKey);
                    //CombosDependent_Fill(string.Empty);
                }
                GlobalUI.SelectNextControl(this, e);
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

        //Menu Strip Event
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }//Completed
        private void tsbSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Save_Process();
                ListSelectionSync();
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
                ListSelectionSync();
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
                ListSelectionSync();
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

        //Functions
        private void Refresh_All(bool skipRefreshGridList)
        {
            try
            {
                Refresh_Header();
                if (skipRefreshGridList == false)
                    Refresh_GridList();               
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
        private void Refresh_Header()
        {
            try
            {
                this.bdsAccGrpDet.DataSource = objAccGrpFactory.ObjREFAccGrp;
                this.bdsAccGrpDet.AllowNew = true;
                this.bdsAccGrpDet.ResetBindings(false);
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
        private void Refresh_GridList()
        {
            try
            {
                string msgID = string.Empty;
                ListSyncInprogress = true;
                if (ContextMenuSetting != "")
                {
                    string listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "0GF", tagrdAccGrp.Name);
                    GlobalUI.Grid_Format(tagrdAccGrp, listID, true, false);                    
                }
                if (this.tagrdAccGrp.DisplayLayout.Bands[0].SortedColumns.Count <= 0)
                    this.tagrdAccGrp.DisplayLayout.Bands[0].SortedColumns.Add(tagrdAccGrp.DisplayLayout.Bands[0].Columns["AccGrpID"], false);
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
                ListSyncInprogress = false;
            }
        }//Completed
        private void FormLayout()
        {
            bool EnableMode = !this.objAccGrpFactory.IsReadOnly;
            this.tslReadOnly.Text = !EnableMode ? "Read Only" : string.Empty;

            //Special Condition for RecordID
            this.AccGrpID.Enabled = EnableMode;
            this.AccGrpID.ReadOnly = !canEditRecordID;

            this.AccGrpDes.Enabled = EnableMode;
            this.Custom1.Enabled = EnableMode;
            this.Custom2.Enabled = EnableMode;
            this.Custom3.Enabled = EnableMode;

            if (EnableMode == false)
            {
                this.tsbSave.Enabled = false;
                this.tsbDelete.Enabled = false;
                this.tsbClear.Enabled = false;
            }
            else
            {
                this.tsbSave.Enabled = true;
                if (this.objAccGrpFactory.IsNew)
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
        }//Completed
        private void ListSelectionSync()
        {
            ListSyncInprogress = true;

            try
            {
                if (objAccGrpFactory.ObjREFAccGrp.AccGrpKey > 0)
                {
                    //When The Header.RecordKey is the same as the List.RecordKey, we do not need to select the related row on the list
                    //this will prevent the list from opening the record again
                    if (tagrdAccGrp.Selected != null)
                        if (tagrdAccGrp.Selected.Cells.Count > 0)
                            if (GFunc.NEInt(tagrdAccGrp.Selected.Cells[0].Row.Cells["AccGrpKey"].Value, 0) == objAccGrpFactory.ObjREFAccGrp.AccGrpKey)
                                return;//already selected.

                    //Select the row on the list to reflect the current value in Header.recordkey
                    UltraGridRow ToSelectRow = this.tagrdAccGrp.Rows.OfType<UltraGridRow>().ToList().Find(
                    row => row.Cells["AccGrpKey"].Text.Equals(objAccGrpFactory.ObjREFAccGrp.AccGrpKey.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    if (ToSelectRow != null)
                    {
                        ToSelectRow.Cells["AccGrpID"].Selected = true;
                        ToSelectRow.Cells["AccGrpID"].Activate();
                    }
                }
                else
                {
                    tagrdAccGrp.Selected.Cells.Clear();
                    tagrdAccGrp.ActiveRow = null;
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
                ListSyncInprogress = false;
            }
        }//Completed

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
                        this.objAccGrpFactory.IsDirty = false;
                    }
                }

                this.errorProvider1.Clear();

                if (this.objAccGrpFactory.IsDirty)
                {
                    if (Save_Process() == false)
                        return false;
                }

                if (this.objAccGrpFactory.New() == false)
                {                   
                    return false;
                }
                else
                {                  
                    this.errorProvider1.Clear();
                 
                    this.AccGrpID.Focus();
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
                this.Refresh_All(false);
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

                if (objAccGrpFactory.IsDirty)
                {
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

                //Perform Saving
                if (this.objAccGrpFactory.Save())
                {
                    GlobalUI.ResetControlDirty(this);
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
                this.Refresh_All(false);
                this.FormLayout();
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private bool OpenRecord(int key)
        {
            this.Cursor = Cursors.WaitCursor;
            GEnum.MsgBoxButton btnSelect = GEnum.MsgBoxButton.Yes;

            try
            {
                if (SECPermUtility.Edit(objAccGrpFactory.PermID, false))
                {
                    if (objAccGrpFactory.GetEdit(key) == false)
                    {
                        if (SysOptionUtility.GetBool("WarnOpenRecordAsReadOnly"))
                        {
                            btnSelect = MsgBox.Show("Try to open record as read only",
                                                  GEnum.MsgBoxIcon.Question,
                                                  GEnum.MsgBoxButton.Yes,
                                                  GEnum.MsgBoxButton.No);

                            if (btnSelect == GEnum.MsgBoxButton.Yes)
                            {
                                objAccGrpFactory.GetReadOnly(key);
                            }
                        }
                    }
                }
                else
                    objAccGrpFactory.GetReadOnly(key);

                GlobalUI.ResetControlDirty(this);
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
                Refresh_All(true);
                FormLayout();
                ListSelectionSync();
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private bool Delete_Process()
        {
            this.Cursor = Cursors.WaitCursor;
            int PreRowIndex = 0;
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

                if (this.objAccGrpFactory.Delete())
                {
                    this.objAccGrpFactory.New();
               
                    //Move the cursor position of active row index to upper row
                    if (tagrdAccGrp.ActiveRow.Index > 0)
                        PreRowIndex = tagrdAccGrp.ActiveRow.Index - 1;

                    GlobalUI.ResetControlDirty(this);
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
                this.Refresh_All(false);
                this.FormLayout();
                if (tagrdAccGrp.Rows.Count > 0)
                {
                    tagrdAccGrp.Rows[PreRowIndex].Selected = true;
                    tagrdAccGrp.Rows[PreRowIndex].Activate();
                }
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private bool Clear_Process()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (GFunc.IsNEZ(this.objAccGrpFactory.ObjREFAccGrp.AccGrpKey))
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

                    if (this.objAccGrpFactory.New())
                    {                      
                        errorProvider1.Clear();
                        GlobalUI.ResetControlDirty(this);
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
                this.Refresh_All(false);
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
                this.Validate();

                if (TAUtil.ControlGVar.FormValidateFail)
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

        //Grid Events
        private void tagrdAccGrp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AccGrpID.Focus();
            }
        }//Completed
        private void tagrdAccGrp_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            int key = 0;
            try
            {
                if (ListSyncInprogress == false)//disable selection when list synchonisation is in progress
                {
                    if (this.tagrdAccGrp.ActiveRow != null && tagrdAccGrp.Selected.Cells.Count > 0)//this line is required to check if user have selected a row in the list
                        key = GFunc.NEInt(tagrdAccGrp.Selected.Cells[0].Row.Cells["AccGrpKey"].Value, 0);

                    if (this.SaveChanges() == false)
                    {
                        ListSelectionSync();
                        return;
                    }

                    if (key > 0)
                        this.OpenRecord(key);
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
        private void ErrorNotifier_Clear(object sender, BOLib.UINotifierEventArgs e)
        {
            this.errorProvider1.Clear();
            DocComUtility.ClearErrNotifier(this, e, errorProvider1);
        }
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
        }
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