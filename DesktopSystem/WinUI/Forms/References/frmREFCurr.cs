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
    public partial class frmREFCurr : Form
    {
        #region Local Variables

        private BOLib.REFCurrFactory objCurrFactory = null;
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
        int PreRowIndex = 0;
        #endregion

        //Initialize
        public frmREFCurr()
        {
            InitializeComponent();
        }//Completed
        public frmREFCurr(string id)
        {
            InitializeComponent();
            recordID = id;
            formOpenMode = GEnum.formInitMode.Add;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }//Completed
        public frmREFCurr(int Key)
        {
            //For call from shortcut menu (Edit)
            InitializeComponent();
            formOpenMode = GEnum.formInitMode.Edit;
            recordKey = Key;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }//Completed
        public frmREFCurr(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            //Open from AuditLog as Read only data viewer
            InitializeComponent();
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
        }//Completed

        //Form Events
        private void frmREFCurr_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.tagrdCurrList.DisplayLayout.Bands[0].SortedColumns.Clear();
                //Call Initialization
                this.objCurrFactory = new BOLib.REFCurrFactory(BOLib.GEnum.InstanceMode.Normal);
                if (objCurrFactory.GUID <= 0)
                {
                    formClose = true;
                    return;
                }

                //Attach Event on Factory
                this.objCurrFactory.ErrorNotifierHeader_Set += new GVar.UINotifierEvent(this.ErrorNotifier_Set);
                this.objCurrFactory.ErrorNotifierHeader_Clear += new GVar.UINotifierEvent(this.ErrorNotifier_Clear);

                //Check if user has permission to edit Record ID
                canEditRecordID = SECPermUtility.Perform("sysRecID", false);

                if (this.IsOpenFromAuditLog)
                {
                    if (objCurrFactory.SetReadOnlyData(_dtHeader, _dsDetail) == false)
                    {
                        formClose = true;
                        return;
                    }
                    Refresh_Form(false);
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
                            this.CurrID.SetValueTrigger(recordID, false);
                    }
                }

                //Setup Grid Layout, ShortcutMenu, fill Combo
                GlobalUI.FormGrids_Set(this, (int)objCurrFactory.ConstantCodeKey, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objCurrFactory.ConstantCodeKey);
                GlobalUI.Combos_Fill(this, (int)objCurrFactory.ConstantCodeKey);

                //Setup List properties
                this.tagrdCurrList.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                this.tagrdCurrList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
                this.tagrdCurrList.DisplayLayout.Bands[0].Override.ActiveCellAppearance.BackColor = Color.Gold;
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
        private void frmREFCurr_Shown(object sender, EventArgs e)
        {
            //Close Form when FormLoad has encounter errors
            if (formClose)
                this.Close();
        }
        private void frmREFCurr_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool runProcess = false;
            this.formClose = true;

            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }
            if (this.formClose && objCurrFactory == null)
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

                //Dispose Factory
                if ((bool)this.objCurrFactory.Dispose() == false)
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
                    this.objCurrFactory.Dispose();
            }
        }
        private void frmREFCurr_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)objCurrFactory.ConstantCodeKey);
                    GlobalUI.RefreshGridDependentText(string.Empty, string.Empty, "ConKey", "ConNm", tagrdCurrDetailConList);
                }

                //Set Focus Next Control
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
        }

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
        private void Refresh_Form(bool skipRefreshGridList)
        {
            try
            {
                Refresh_Header();
                Refresh_GridDet();
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
                this.bdsREFCurr.DataSource = objCurrFactory.ObjREFCurr;
                this.bdsREFCurr.AllowNew = true;
                this.bdsREFCurr.ResetBindings(false);
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
                    string listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "0GF", tagrdCurrList.Name);
                    GlobalUI.Grid_Format(tagrdCurrList, listID, true, false);
                }
                if (this.tagrdCurrList.DisplayLayout.Bands[0].SortedColumns.Count <= 0)
                    this.tagrdCurrList.DisplayLayout.Bands[0].SortedColumns.Add(tagrdCurrList.DisplayLayout.Bands[0].Columns["CurrID"], false);
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
        private void Refresh_GridDet()
        {
            try
            {
                tagrdCurrDetailList.DataSource = objCurrFactory.ObjREFCurrDetItms;
                tagrdCurrDetailList.Rows.Refresh(RefreshRow.ReloadData);

                tagrdCurrDetailConList.DataSource = objCurrFactory.ObjREFCurrDetCons;
                tagrdCurrDetailConList.Rows.Refresh(RefreshRow.ReloadData);
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
            bool EnableMode = !this.objCurrFactory.IsReadOnly;
            this.tslReadOnly.Text = !EnableMode ? "Read Only" : string.Empty;

            //Special Condition for RecordID
            this.CurrID.Enabled = EnableMode;
            this.CurrID.ReadOnly = !canEditRecordID;

            this.CurrNm.Enabled = EnableMode;
            this.TxHdom.Enabled = EnableMode;
            this.TxLdom.Enabled = EnableMode;
            this.SymHdom.Enabled = EnableMode;
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
                if (this.objCurrFactory.IsNew)
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

            #region Set Grids Columns
            foreach (UltraGridColumn col in tagrdCurrDetailConList.DisplayLayout.Bands[0].Columns)
            {
                switch (col.Key.ToLower())
                {
                    case "currkey":
                    case "createdate":
                    case "createuserkey":
                    case "lastmodifieddate":
                    case "lastmodifieduserkey":
                        col.CellActivation = Activation.ActivateOnly;
                        break;

                    default:
                        if (EnableMode)
                            col.CellActivation = Activation.AllowEdit;
                        else
                            col.CellActivation = Activation.ActivateOnly;
                        break;
                }
            }
            foreach (UltraGridColumn col in tagrdCurrDetailList.DisplayLayout.Bands[0].Columns)
            {
                switch (col.Key.ToLower())
                {
                    case "currkey":
                    case "createdate":
                    case "createuserkey":
                    case "lastmodifieddate":
                    case "lastmodifieduserkey":
                        col.CellActivation = Activation.ActivateOnly;
                        break;

                    default:
                        if (EnableMode)
                            col.CellActivation = Activation.AllowEdit;
                        else
                            col.CellActivation = Activation.ActivateOnly;
                        break;
                }
            }
            #endregion
        }
        private void ListSelectionSync()
        {
            ListSyncInprogress = true;

            try
            {
                if (objCurrFactory.ObjREFCurr.CurrKey > 0)
                {
                    //When The Header.RecordKey is the same as the List.RecordKey, we do not need to select the related row on the list
                    //this will prevent the list from opening the record again
                    if (tagrdCurrList.Selected != null)
                        if (tagrdCurrList.Selected.Cells.Count > 0)
                            if (GFunc.NEInt(tagrdCurrList.Selected.Cells[0].Row.Cells["CurrKey"].Value, 0) == objCurrFactory.ObjREFCurr.CurrKey)
                                return;//already selected.

                    //Select the row on the list to reflect the current value in Header.recordkey
                    UltraGridRow ToSelectRow = this.tagrdCurrList.Rows.OfType<UltraGridRow>().ToList().Find(
                    row => row.Cells["CurrKey"].Text.Equals(objCurrFactory.ObjREFCurr.CurrKey.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    if (ToSelectRow != null)
                    {
                        ToSelectRow.Cells["CurrID"].Selected = true;
                        ToSelectRow.Cells["CurrID"].Activate();
                    }
                }
                else
                {
                    tagrdCurrList.Selected.Cells.Clear();
                    tagrdCurrList.ActiveRow = null;
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
                        this.objCurrFactory.IsDirty = false;
                        IsGridsDirty(true);
                    }
                }

                this.errorProvider1.Clear();

                if (this.objCurrFactory.IsDirty)
                {
                    if (Save_Process() == false)
                        return false;
                }

                if (this.objCurrFactory.New() == false)
                {                   
                    return false;
                }
                else
                {                   
                    this.errorProvider1.Clear();
                    this.CurrID.Focus();
                    GlobalUI.ResetControlDirty(this);
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
                this.Refresh_Form(false);
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

                if (objCurrFactory.IsDirty)
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
                if (this.objCurrFactory.Save())
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
                this.Refresh_Form(false);
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
                if (SECPermUtility.Edit(objCurrFactory.PermID, false))
                {
                    if (objCurrFactory.GetEdit(key) == false)
                    {
                        if (SysOptionUtility.GetBool("WarnOpenRecordAsReadOnly"))
                        {
                            btnSelect = MsgBox.Show("Try to open record as read only",
                                                  GEnum.MsgBoxIcon.Question,
                                                  GEnum.MsgBoxButton.Yes,
                                                  GEnum.MsgBoxButton.No);

                            if (btnSelect == GEnum.MsgBoxButton.Yes)
                            {
                                objCurrFactory.GetReadOnly(key);
                            }
                        }
                    }
                }
                else
                    objCurrFactory.GetReadOnly(key);

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
                this.Refresh_Form(true);
                this.FormLayout();
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

                if (this.objCurrFactory.Delete())
                {
                    IsGridsDirty(true);
                    this.objCurrFactory.New();
                  
                    //Move the cursor position of active row index to upper row
                    if (tagrdCurrList.ActiveRow.Index > 0)
                        PreRowIndex = tagrdCurrList.ActiveRow.Index - 1;

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
                this.Refresh_Form(false);
                this.FormLayout();

                if (tagrdCurrList.Rows.Count > 0)
                {
                    tagrdCurrList.Rows[PreRowIndex].Selected = true;
                    tagrdCurrList.Rows[PreRowIndex].Activate();
                }
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private bool Clear_Process()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (GFunc.IsNEZ(this.objCurrFactory.ObjREFCurr.CurrKey))
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

                    if (this.objCurrFactory.New())
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
                this.Refresh_Form(false);
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
                this.tagrdCurrDetailList.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdCurrDetailList.UpdateData();
                this.tagrdCurrDetailConList.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdCurrDetailConList.UpdateData();

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
            //This function check if the grid has uncommited data in its active row
            //it also has an option to undo those uncommited changes.        
            bool result = false;
            try
            {
                if (tagrdCurrDetailList.ActiveRow != null)
                {
                    if (tagrdCurrDetailList.ActiveRow.DataChanged && !tagrdCurrDetailList.ActiveRow.IsUnmodifiedTemplateAddRow)
                    {
                        if (undoChangesInGrid)
                        {
                            //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                            this.tagrdCurrDetailList.PerformAction(UltraGridAction.UndoCell);
                            this.tagrdCurrDetailList.PerformAction(UltraGridAction.UndoRow);
                        }
                        result = true;
                    }
                }

                if (tagrdCurrDetailConList.ActiveRow != null)
                {
                    if (tagrdCurrDetailConList.ActiveRow.DataChanged && !tagrdCurrDetailConList.ActiveRow.IsUnmodifiedTemplateAddRow)
                    {
                        if (undoChangesInGrid)
                        {
                            //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                            this.tagrdCurrDetailConList.PerformAction(UltraGridAction.UndoCell);
                            this.tagrdCurrDetailConList.PerformAction(UltraGridAction.UndoRow);
                        }
                        result = true;
                    }
                }
                return result;
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

        //Control Common Events
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
                GlobalUI.ItemNotInList(sender as TAUtil.TAComboBox, e, false, 0);
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
                    case "conkey":
                    case "connm":
                        tagrdCurrDetailConList.ActiveRow.Cells["ConKey"].Value = key;
                        tagrdCurrDetailConList.ActiveRow.Cells["ConNm"].Value = des;
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
                    default:
                        break;
                }

                if (FromButtonClick)
                {
                    if (DocHDRUtil.EditorButton_Popup((int)objCurrFactory.ConstantCodeKey, controlText, listSettingID, PopupType, ref key, ref id, ref des))
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
                            if (DocHDRUtil.EditorButton_Popup((int)objCurrFactory.ConstantCodeKey, controlText, listSettingID, PopupType, ref key, ref id, ref des))
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
        private void tabDetailList_KeyDown(object sender, KeyEventArgs e)
        {
            switch (tabDetailList.ActiveTab.Key.ToLower())
            {
                case "curr":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                        case Keys.Down:
                            // tagrdCurrDetailList.Select();
                            tagrdCurrDetailList.Focus();
                            UltraGridColumn FirstVisCol = tagrdCurrDetailList.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                            if (FirstVisCol != null)
                            {
                                tagrdCurrDetailList.ActiveCell = tagrdCurrDetailList.Rows.TemplateAddRow.Cells[FirstVisCol.Key];
                                tagrdCurrDetailList.PerformAction(UltraGridAction.EnterEditMode, false, false);
                            }
                            break;
                        case Keys.Up:
                            Custom3.Focus();
                            break;
                    }
                    break;
                case "vendor":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                        case Keys.Down:
                            tagrdCurrDetailConList.Focus();
                            UltraGridColumn FirstVisCol = tagrdCurrDetailConList.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                            if (FirstVisCol != null)
                            {
                                tagrdCurrDetailConList.ActiveCell = tagrdCurrDetailConList.Rows.TemplateAddRow.Cells[FirstVisCol.Key];
                                tagrdCurrDetailConList.PerformAction(UltraGridAction.EnterEditMode, false, false);
                            }
                            break;
                        case Keys.Up:
                            Custom3.Focus();
                            break;
                    }
                    break;
            }
        }

        //Grid Events
        private void tagrdCurrList_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            int key = 0;
            try
            {
                if (ListSyncInprogress == false)//disable selection when list synchonisation is in progress
                {
                    if (this.tagrdCurrList.ActiveRow != null && tagrdCurrList.Selected.Cells.Count > 0)//this line is required to check if user have selected a row in the list
                        key = GFunc.NEInt(tagrdCurrList.Selected.Cells[0].Row.Cells["CurrKey"].Value, 0);

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
        private void tagrdCurrList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.CurrID.Focus();
            }
        }//Completed

        private void tagrdCurrDetailList_CustomDataError(object sender, TAUtil.TAErrorEventArgs e)
        {
            try
            {
                //Common Fuction in GlobalUI for CustomDataError Event in Detail Grid
                GlobalUI.CustomDataError(sender, e, this);

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
        private void tagrdCurrDetailList_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {

                UltraGridCell currentCell = tagrdCurrDetailList.ActiveCell;
                switch (currentCell.Column.Key.ToLower())
                {

                    case "currrate":
                    case "countryrate":
                    case "customrate1":
                    case "customrate2":
                    case "customrate3":
                        currentCell.Value = GFunc.NEDec(currentCell.Value, 1);
                        break;
                }
                if (objCurrFactory.Validation_Detail(tagrdCurrDetailList.Name, e.Cell.Row, e.Cell.Column.Key) == false)
                    e.Cancel = true;
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
        private void tagrdCurrDetailList_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            //Check Validation on the cell value and When there's an error set the grid row
            try
            {



                if (!GFunc.IsNE(objCurrFactory) && tagrdCurrDetailList.ActiveRow != null)
                {
                    if (objCurrFactory.Validation_Detail(tagrdCurrDetailList.Name, tagrdCurrDetailList.ActiveRow, string.Empty) == false)
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
        private void tagrdCurrDetailList_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                e.DisplayPromptMsg = false;
                if (tagrdCurrDetailList.ActiveRow.IsAddRow == false)
                {
                    if (SysOptionUtility.GetBool("WarnDeleteRecordDetail"))
                    {
                        if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) != GEnum.MsgBoxButton.Delete)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                    //Move the cursor position of active row index to upper row
                    if (tagrdCurrDetailList.ActiveRow.Index > 0)
                        PreRowIndex = tagrdCurrDetailList.ActiveRow.Index - 1;
                    return;
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void tagrdCurrDetailList_AfterRowUpdate(object sender, RowEventArgs e)
        {
            this.objCurrFactory.ObjREFCurrDetItms.AcceptChanges();
            this.objCurrFactory.IsDirty = true;
        }//Completed
        private void tagrdCurrDetailList_AfterRowsDeleted(object sender, EventArgs e)
        {
            this.objCurrFactory.ObjREFCurrDetItms.AcceptChanges();
            this.objCurrFactory.IsDirty = true;
            if (tagrdCurrDetailList.Rows.Count > 0)
            {
                tagrdCurrDetailList.Rows[PreRowIndex].Selected = true;
                tagrdCurrDetailList.Rows[PreRowIndex].Activate();
                PreRowIndex = 0;
            }
        }//Completed
        private void tagrdCurrDetailList_Error(object sender, ErrorEventArgs e)
        {

        }//Completed
        private void tagrdCurrDetailList_ClickCellButton(object sender, CellEventArgs e)
        {
            //No Column
        }//Completed

        private void tagrdCurrDetailConList_CustomDataError(object sender, TAUtil.TAErrorEventArgs e)
        {
            try
            {
                if (e.ErrorCode == TAUtil.TAErrorCode.NOT_IN_LIST)
                {
                    TAUtil.TAGridEditor tagrdDetItms = sender as TAUtil.TAGridEditor;
                    if (tagrdDetItms.ActiveCell.Column.EditorComponent != null)
                    {
                        tagrdDetItms.PerformAction(UltraGridAction.EnterEditMode);

                        if (tagrdDetItms.ActiveCell.Column.EditorComponent.GetType() == typeof(TAUtil.TAComboBox))
                        {
                            TAUtil.TAComboBox taCombo = (TAUtil.TAComboBox)tagrdDetItms.ActiveCell.Column.EditorComponent;
                            taCombo.Text = tagrdDetItms.ActiveCell.Text;

                            switch (tagrdDetItms.ActiveCell.Column.Key.ToLower())
                            {
                                case "conkey":
                                    GlobalUI.ItemNotInList(tagrdDetItms.ActiveCell, null, 1);// ItemNotInListAdd
                                    break;
                                default:
                                    GlobalUI.ItemNotInList(tagrdDetItms.ActiveCell, null, 0);
                                    break;
                            }
                        }
                        else
                        {
                            GlobalUI.ItemNotInList(tagrdDetItms.ActiveCell, null, 0);
                        }
                    }
                }
                else
                {
                    MsgBox.Show(e.ErrorMessage);
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
        private void tagrdCurrDetailConList_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {

                UltraGridCell currentCell = tagrdCurrDetailConList.ActiveCell;
                switch (currentCell.Column.Key.ToLower())
                {

                    case "concurrrate":
                    case "concustomrate1":
                    case "concustomrate2":
                    case "concustomrate3":
                        currentCell.Value = GFunc.NEDec(currentCell.Value, 1);
                        break;
                }
                if (objCurrFactory.Validation_Detail(tagrdCurrDetailConList.Name, e.Cell.Row, e.Cell.Column.Key) == false)
                    e.Cancel = true;
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
        private void tagrdCurrDetailConList_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            //Check Validation on the cell value and When there's an error set the grid row
            try
            {



                if (!GFunc.IsNE(objCurrFactory) && tagrdCurrDetailConList.ActiveRow != null)
                {
                    if (objCurrFactory.Validation_Detail(tagrdCurrDetailConList.Name, tagrdCurrDetailConList.ActiveRow, string.Empty) == false)
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
        private void tagrdCurrDetailConList_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                e.DisplayPromptMsg = false;
                if (tagrdCurrDetailConList.ActiveRow.IsAddRow == false)
                {
                    if (SysOptionUtility.GetBool("WarnDeleteRecordDetail"))
                    {
                        if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) != GEnum.MsgBoxButton.Delete)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                    //Move the cursor position of active row index to upper row
                    if (tagrdCurrDetailConList.ActiveRow.Index > 0)
                        PreRowIndex = tagrdCurrDetailConList.ActiveRow.Index - 1;
                    return;
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void tagrdCurrDetailConList_AfterRowUpdate(object sender, RowEventArgs e)
        {
            this.objCurrFactory.ObjREFCurrDetCons.AcceptChanges();
            this.objCurrFactory.IsDirty = true;
        }//Completed
        private void tagrdCurrDetailConList_AfterRowsDeleted(object sender, EventArgs e)
        {
            try
            {
                this.objCurrFactory.ObjREFCurrDetCons.AcceptChanges();
                this.objCurrFactory.IsDirty = true;

                if (tagrdCurrDetailConList.Rows.Count > 0)
                {
                    tagrdCurrDetailConList.Rows[PreRowIndex].Selected = true;
                    tagrdCurrDetailConList.Rows[PreRowIndex].Activate();
                    PreRowIndex = 0;
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
            
        }//Completed
        private void tagrdCurrDetailConList_ClickCellButton(object sender, CellEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;
            string listSettingID = string.Empty;
            try
            {
                switch (e.Cell.Column.Key.ToLower())
                {
                    case "conkey":
                        listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, e.Cell.Column.Key, tagrdCurrDetailConList.Name);
                        if (DocHDRUtil.EditorButton_Popup((int)objCurrFactory.ConstantCodeKey, e.Cell.Text, listSettingID, (int)GEnum.PopupType.VendID, ref key, ref id, ref des))
                        {
                            tagrdCurrDetailConList.ActiveRow.Cells["ConKey"].Value = key;

                        }
                        break;
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

                if (this.ActiveControl.GetType() != typeof(TAUtil.TAGridEditor))
                {
                    l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl });
                }
                else
                {
                    string ActiveColKey = "";
                    if (((TAUtil.TAGridEditor)this.ActiveControl).ActiveCell != null)
                    {
                        ActiveColKey = GFunc.GridColumnKey_Get(this.ActiveControl);
                    }

                    l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, ActiveColKey });
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
                    SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { });
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
