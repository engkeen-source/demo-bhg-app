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
using System.IO;
using TAUtil;
namespace WinUI
{
    public partial class frmMstAccDept : Form
    {
        #region Local Variables

        private BOLib.MSTAccDeptFactory objDeptFactory = null;
        string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        private GEnum.formInitMode formOpenMode = GEnum.formInitMode.Neither;
        private int recordKey = 0;
        private string recordID = "";
        private bool canEditRecordID = false;
        private string msgID = string.Empty;
        private bool ListSyncInprogress = false;

        //only use to show Read only data viewer for AuditLog 
        private bool IsOpenFromAuditLog = false;
        private DataTable _dtHeader = null;
        private DataSet _dsDetail = null;

        #endregion

        //Initialize
        public frmMstAccDept()
        {
            InitializeComponent();
        }//Completed
        public frmMstAccDept(string id)
        {
            //For Call from Shortcut menu (Edit/Add)
            InitializeComponent();
            recordID = id;
            formOpenMode = GEnum.formInitMode.Add;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }//Completed
        public frmMstAccDept(int key)
        {
            //For call from shortcut menu (Edit)
            InitializeComponent();
            formOpenMode = GEnum.formInitMode.Edit;
            recordKey = key;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }//Completed
        public frmMstAccDept(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            //Open from AuditLog as Read only data viewer
            InitializeComponent();
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
        }//Completed

        //Form Events
        private void frmMSTAccDept_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.tagrdDept.DisplayLayout.Bands[0].SortedColumns.Clear();
                //Call Initialization
                this.objDeptFactory = new BOLib.MSTAccDeptFactory(BOLib.GEnum.InstanceMode.Normal);
                if (objDeptFactory.GUID <= 0)
                {
                    formClose = true;
                    return;
                }

                //Attach Event on Factory
                this.objDeptFactory.ErrorNotifierHeader_Set += new GVar.UINotifierEvent(this.ErrorNotifier_Set);
                this.objDeptFactory.ErrorNotifierHeader_Clear += new GVar.UINotifierEvent(this.ErrorNotifier_Clear);

                //Check if user has permission to edit Record ID
                canEditRecordID = SECPermUtility.Perform("sysRecID", false);

                if (this.IsOpenFromAuditLog)
                {
                    if (objDeptFactory.SetReadOnlyData(_dtHeader, _dsDetail) == false)
                    {
                        formClose = true;
                        return;
                    }
                    Refresh_All(false);
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
                            this.DeptID.SetValueTrigger(recordID, false);
                    }
                }

                //Setup Grid Layout, ShortcutMenu, fill Combo
                GlobalUI.FormGrids_Set(this, (int)objDeptFactory.ConstantCodeKey, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objDeptFactory.ConstantCodeKey);
                //GlobalUI.cmnuGlobal_Set(this);
                GlobalUI.Combos_Fill(this, (int)objDeptFactory.ConstantCodeKey);

                //Setup List properties
                this.tagrdDept.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                this.tagrdDept.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
                this.tagrdDept.DisplayLayout.Bands[0].Override.ActiveCellAppearance.BackColor = Color.Gold;
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
        private void frmMSTAccDept_Shown(object sender, EventArgs e)
        {
            //Close Form when FormLoad has encounter errors
            if (formClose)
                this.Close();
        }//Completed
        private void frmMSTAccDept_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool runProcess = false;
            this.formClose = true;

            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }
            if (this.formClose && objDeptFactory == null)
                return;

            try
            {
                #region Closing with Invalid DataType error encountered
                //When the caller performs this.close, the system actually perform validation on all control automatically
                //if there are any control that fails validation (invalid datatype, the e.cancel is set to true, we have no control over this (not sure if this was done by csla)
                //thus we need to check for e.cancel = true so that we can skip the rest of the codes to prevent error message from appearing twice or more
                if (e.Cancel == true)
                {
                    runProcess = true;
                }
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

                

                if ((bool)this.objDeptFactory.Dispose() == false)
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
                    this.objDeptFactory.Dispose();
            }
        }//Completed        
        private void frmMSTAccDept_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)objDeptFactory.ConstantCodeKey);
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

        //Menu Strip Events
        private void tsbClose_Click(object sender, EventArgs e)
        {
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

        //Functions
        private void Refresh_All(bool skipRefreshGridList)
        {
            try
            {
                Refresh_Header();
                Refresh_GridList();
                GlobalUI.ResetControlDirty(this);
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
            try
            {
                this.bdsDeptDet.DataSource = objDeptFactory.ObjMSTAccDept;
                this.bdsDeptDet.AllowNew = true;
                this.bdsDeptDet.ResetBindings(false);
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
                    string listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "0GF", tagrdDept.Name);
                    GlobalUI.Grid_Format(tagrdDept, listID, true, false);
                }
                if (this.tagrdDept.DisplayLayout.Bands[0].SortedColumns.Count <= 0)
                    this.tagrdDept.DisplayLayout.Bands[0].SortedColumns.Add(tagrdDept.DisplayLayout.Bands[0].Columns["DeptID"], false);
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
            try
            {
                bool EnableMode = !this.objDeptFactory.IsReadOnly;
                this.tslReadOnly.Text = !EnableMode ? "Read Only" : string.Empty;

                //Special Condition for RecordID
                this.DeptID.Enabled = EnableMode;
                this.DeptID.ReadOnly = !canEditRecordID;

                this.DeptNm.Enabled = EnableMode;
                this.Inactive.Enabled = EnableMode;
                this.Custom1.Enabled = EnableMode;
                this.Custom2.Enabled = EnableMode;
                this.Custom3.Enabled = EnableMode;
                this.Custom4.Enabled = EnableMode;
                this.Custom5.Enabled = EnableMode;

                if (EnableMode == false)
                {
                    this.tsbSave.Enabled = false;
                    this.tsbDelete.Enabled = false;
                    this.tsbClear.Enabled = false;
                }
                else
                {
                    this.tsbSave.Enabled = true;
                    if (this.objDeptFactory.IsNew)
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
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
            
        }//Completed
        private void ListSelectionSync()
        {
            ListSyncInprogress = true;

            try
            {
                if (objDeptFactory.ObjMSTAccDept.DeptKey > 0)
                {
                    //When The Header.RecordKey is the same as the List.RecordKey, we do not need to select the related row on the list
                    //this will prevent the list from opening the record again
                    if (tagrdDept.Selected != null)
                        if (tagrdDept.Selected.Cells.Count > 0)
                            if (GFunc.NEInt(tagrdDept.Selected.Cells[0].Row.Cells["DeptKey"].Value, 0) == objDeptFactory.ObjMSTAccDept.DeptKey)
                                return;//already selected.

                    //Select the row on the list to reflect the current value in Header.recordkey
                    UltraGridRow ToSelectRow = this.tagrdDept.Rows.OfType<UltraGridRow>().ToList().Find(
                    row => row.Cells["DeptKey"].Text.Equals(objDeptFactory.ObjMSTAccDept.DeptKey.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    if (ToSelectRow != null)
                    {
                        ToSelectRow.Cells["DeptID"].Selected = true;
                        ToSelectRow.Cells["DeptID"].Activate();
                    }
                }
                else
                {
                    tagrdDept.Selected.Cells.Clear();
                    tagrdDept.ActiveRow = null;
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
                        this.objDeptFactory.IsDirty = false;
                    }
                }

                this.errorProvider1.Clear();

                if (this.objDeptFactory.IsDirty)
                {
                    if (Save_Process() == false)
                        return false;
                }

                if (this.objDeptFactory.New() == false)
                {                   
                    return false;
                }
                else
                {                   
                    this.errorProvider1.Clear();                 
                    this.DeptID.Focus();
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

                if (objDeptFactory.IsDirty)
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
                if (this.objDeptFactory.Save())
                {
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
                if (SECPermUtility.Edit(objDeptFactory.PermID, false))
                {
                    if (objDeptFactory.GetEdit(key) == false)
                    {
                        if (SysOptionUtility.GetBool("WarnOpenRecordAsReadOnly"))
                        {
                            btnSelect = MsgBox.Show("Try to open record as read only",
                                                  GEnum.MsgBoxIcon.Question,
                                                  GEnum.MsgBoxButton.Yes,
                                                  GEnum.MsgBoxButton.No);

                            if (btnSelect == GEnum.MsgBoxButton.Yes)
                            {
                                objDeptFactory.GetReadOnly(key);
                            }
                        }
                    }
                }
                else
                    objDeptFactory.GetReadOnly(key);

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
                this.Refresh_All(true);
                this.FormLayout();                
                ListSelectionSync();
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

                if (this.objDeptFactory.Delete())
                {
                    this.objDeptFactory.New();
                    int PreRowIndex = 0;
                    //Move the cursor position of active row index to upper row
                    if (tagrdDept.ActiveRow.Index > 0)
                        PreRowIndex = tagrdDept.ActiveRow.Index - 1;
                    this.Refresh_All(false);

                    if (tagrdDept.Rows.Count > 0)
                    {
                        tagrdDept.Rows[PreRowIndex].Selected = true;
                        tagrdDept.Rows[PreRowIndex].Activate();
                    }
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
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private bool Clear_Process()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (GFunc.IsNEZ(this.objDeptFactory.ObjMSTAccDept.DeptKey))
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

                    if (this.objDeptFactory.New())
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

        //Grid Common Events
        private void tagrdDept_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            int key = 0;
            try
            {
                if (ListSyncInprogress == false)//disable selection when list synchonisation is in progress
                {
                    if (this.tagrdDept.ActiveRow != null && tagrdDept.Selected.Cells.Count > 0)//this line is required to check if user have selected a row in the list
                        key = GFunc.NEInt(tagrdDept.Selected.Cells[0].Row.Cells["DeptKey"].Value, 0);

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
        private void tagrdDept_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DeptID.Focus();
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

    }
}