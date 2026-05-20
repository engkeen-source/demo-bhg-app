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
    public partial class frmREFUOM : Form
    {
        #region Local Variables
        private BOLib.REFUOMFactory objUOMFactory = null;
        private string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        private GEnum.formInitMode formOpenMode = GEnum.formInitMode.Neither;
        private int recordKey = 0;
        private string recordID = string.Empty;
        private bool canEditRecordID = false;
        private string msgID = string.Empty;
        private bool ListSyncInprogress = false;

        //only use to show Read only data viewer for AuditLog 
        private bool IsOpenFromAuditLog = false;
        private DataTable _dtHeader = null;
        private DataSet _dsDetail = null;

        #endregion

        //Initialize
        public frmREFUOM()
        {
            InitializeComponent();
        }//Completed
        public frmREFUOM(string id)
        {
            //For Call from Shortcut menu (Edit/Add)
            InitializeComponent();
            recordID = id;
            formOpenMode = GEnum.formInitMode.Add;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }//Completed
        public frmREFUOM(int key)
        {
            //For call from shortcut menu (Edit)
            InitializeComponent();
            formOpenMode = GEnum.formInitMode.Edit;
            recordKey = key;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }//Completed
        public frmREFUOM(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            //Open from AuditLog as Read only data viewer
            InitializeComponent();
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
        }//Completed

        //Form Events
        private void frmREFUOM_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //Clear any sorting columns if any done in design time
                this.tagrdUOMList.DisplayLayout.Bands[0].SortedColumns.Clear();

                //Call Initialization
                this.objUOMFactory = new BOLib.REFUOMFactory(BOLib.GEnum.InstanceMode.Normal);
                if (objUOMFactory.GUID <= 0)
                {
                    formClose = true;
                    return;
                }

                //Attach Event on Factory
                this.objUOMFactory.ErrorNotifierHeader_Set += new GVar.UINotifierEvent(this.ErrorNotifier_Set);
                this.objUOMFactory.ErrorNotifierHeader_Clear += new GVar.UINotifierEvent(this.ErrorNotifier_Clear);

                //Check if user has permission to edit Record ID
                canEditRecordID = SECPermUtility.Perform("sysRecID", false);

                if (this.IsOpenFromAuditLog)
                {
                    if (objUOMFactory.SetReadOnlyData(_dtHeader, _dsDetail) == false)
                    {
                        formClose = true;
                        return;
                    }
                    this.Refresh_Form(false);
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
                            this.UOMID.SetValueTrigger(recordID, false);
                    }                    
                }

                //Setup Grid Layout,Fill Data, ShortcutMenu, fill Combo
                GlobalUI.FormGrids_Set(this, (int)objUOMFactory.ConstantCodeKey, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objUOMFactory.ConstantCodeKey);
               // //GlobalUI.cmnuGlobal_Set(this);
                GlobalUI.Combos_Fill(this, (int)objUOMFactory.ConstantCodeKey);
                GlobalUI.GridAllColumnsActivateOnlySet(tagrdUOMDetailList);

                //Setup List properties
                this.tagrdUOMList.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                this.tagrdUOMList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
                this.tagrdUOMList.DisplayLayout.Bands[0].Override.ActiveCellAppearance.BackColor = Color.Gold;

                //Set Default Values
                tagrdUOMDetailList.DisplayLayout.Bands[0].Columns["UOMConRate"].DefaultCellValue = 1;
                                
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
        private void frmREFUOM_Shown(object sender, EventArgs e)
        {
            //Close Form when FormLoad has encounter errors
            if (formClose)
                this.Close();
        }//Completed
        private void frmREFUOM_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool runProcess = false;
            this.formClose = true;

            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }
            if (this.formClose && objUOMFactory == null)
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
                if ((bool)this.objUOMFactory.Dispose() == false)
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
                    this.objUOMFactory.Dispose();
            }
        }//Completed
        private void frmREFUOM_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                    GlobalUI.Combos_Fill(this, (int)objUOMFactory.ConstantCodeKey);

                if (Custom3.Focused == true)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Tab:
                        case Keys.Enter:
                        case Keys.Down:
                            if (e.Shift == false) //For Backward case
                                GlobalUI.TabKeyDownForGrid(tagrdUOMDetailList);
                            break;
                    }
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
                if (skipRefreshGridList==false)
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
                this.bdsUOMDet.DataSource = objUOMFactory.ObjREFUOM;
                this.bdsUOMDet.AllowNew = true;
                this.bdsUOMDet.ResetBindings(false);
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
                    string listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "0GF", tagrdUOMList.Name);
                    GlobalUI.Grid_Format(tagrdUOMList, listID, true, false);
                    
                }
                //Please make sure in design time no columns is sorted or clear sorted columns in formload, if anycolumn is already defined as sorted column , this code will skip
                if (this.tagrdUOMList.DisplayLayout.Bands[0].SortedColumns.Count <= 0)
                    this.tagrdUOMList.DisplayLayout.Bands[0].SortedColumns.Add(tagrdUOMList.DisplayLayout.Bands[0].Columns["UOMID"], false);
                
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
                tagrdUOMDetailList.DataSource = objUOMFactory.ObjREFUOMDetItms;
                tagrdUOMDetailList.Rows.Refresh(RefreshRow.ReloadData);

                Filter_ConversionCombo();
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
            bool EnableMode = !this.objUOMFactory.IsReadOnly;
            this.tslReadOnly.Text = !EnableMode ? "Read Only" : string.Empty;

            //Special Condition for RecordID
            this.UOMID.Enabled = EnableMode;
            this.UOMID.ReadOnly = !canEditRecordID;

            this.UOMShw.Enabled = EnableMode;
            this.UOMType.Enabled = EnableMode;
            this.GramRate.Enabled = EnableMode;
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
                if (this.objUOMFactory.IsNew)
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
            foreach (UltraGridColumn col in tagrdUOMDetailList.DisplayLayout.Bands[0].Columns)
            {
                switch (col.Key.ToLower())
                {
                    case "uomkey":
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
        }//Completed
        private void ListSelectionSync()
        {
            ListSyncInprogress = true;

            try
            {
                if (objUOMFactory.ObjREFUOM.UOMKey > 0)
                {
                    //When The Header.RecordKey is the same as the List.RecordKey, we do not need to select the related row on the list
                    //this will prevent the list from opening the record again
                    if (tagrdUOMList.Selected != null)
                        if (tagrdUOMList.Selected.Cells.Count > 0)
                            if (GFunc.NEInt(tagrdUOMList.Selected.Cells[0].Row.Cells["UOMKey"].Value, 0) == objUOMFactory.ObjREFUOM.UOMKey)
                                return;//already selected.

                    //Select the row on the list to reflect the current value in Header.recordkey
                    UltraGridRow ToSelectRow = this.tagrdUOMList.Rows.OfType<UltraGridRow>().ToList().Find(
                    row => row.Cells["UOMKey"].Text.Equals(objUOMFactory.ObjREFUOM.UOMKey.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    if (ToSelectRow != null)
                    {
                        ToSelectRow.Cells["UOMID"].Selected = true;
                        ToSelectRow.Cells["UOMID"].Activate();
                    }
                }
                else
                {
                    tagrdUOMList.Selected.Cells.Clear();
                    tagrdUOMList.ActiveRow = null;
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
        private void Refresh_ConversionCombo()
        {
            try
            {
                GlobalUI.BindComboValue((TAUtil.TAComboBox)tagrdUOMDetailList.DisplayLayout.Bands[0].Columns["UOMConKey"].EditorComponent,
                    GVar.ListSettingID.REFUOM);
                Filter_ConversionCombo();
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
        private void Filter_ConversionCombo()
        {
            if (tagrdUOMDetailList.DisplayLayout.Bands[0].Columns["UOMConKey"].EditorComponent != null)
            {
                TAUtil.TAComboBox cbo = (TAUtil.TAComboBox)tagrdUOMDetailList.DisplayLayout.Bands[0].Columns["UOMConKey"].EditorComponent;
                ColumnFiltersCollection colFilters = cbo.DisplayLayout.Bands[0].ColumnFilters;
                colFilters.ClearAllFilters();
                colFilters["UOMKey"].FilterConditions.Add(FilterComparisionOperator.NotEquals, objUOMFactory.ObjREFUOM.UOMKey);
            }
        }

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
                        this.objUOMFactory.IsDirty = false;
                        IsGridsDirty(true);
                    }
                }

                this.errorProvider1.Clear();

                if (this.objUOMFactory.IsDirty)
                {
                    if (Save_Process() == false)
                        return false;
                }

                if (this.objUOMFactory.New() == false)
                {                  
                    return false;
                }
                else
                {
                    this.errorProvider1.Clear();
                 
                    this.UOMID.Focus();
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

                if (objUOMFactory.IsDirty)
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
                if (this.objUOMFactory.Save())
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
                if (SECPermUtility.Edit(objUOMFactory.PermID, false))
                {
                    if (objUOMFactory.GetEdit(key) == false)
                    {
                        if (SysOptionUtility.GetBool("WarnOpenRecordAsReadOnly"))
                        {
                            btnSelect = MsgBox.Show("Try to open record as read only",
                                                  GEnum.MsgBoxIcon.Question,
                                                  GEnum.MsgBoxButton.Yes,
                                                  GEnum.MsgBoxButton.No);
                            if (btnSelect == GEnum.MsgBoxButton.Yes)
                            {
                                objUOMFactory.GetReadOnly(key);
                            }
                        }
                    }
                }
                else
                    objUOMFactory.GetReadOnly(key);

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

                if (this.objUOMFactory.Delete())
                {
                    IsGridsDirty(true);
                    this.objUOMFactory.New();
                    
                    //Move the cursor position of active row index to upper row
                    if (tagrdUOMList.ActiveRow.Index > 0)
                        PreRowIndex = tagrdUOMList.ActiveRow.Index - 1;

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
                if (tagrdUOMList.Rows.Count > 0)
                {
                    tagrdUOMList.Rows[PreRowIndex].Selected = true;
                    tagrdUOMList.Rows[PreRowIndex].Activate();
                }
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private bool Clear_Process()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (GFunc.IsNEZ(this.objUOMFactory.ObjREFUOM.UOMKey))
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

                    if (this.objUOMFactory.New())
                    {                       
                        errorProvider1.Clear();//-----------------------albert
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
                this.tagrdUOMDetailList.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdUOMDetailList.UpdateData();

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
            if (tagrdUOMDetailList.ActiveRow != null)
            {
                if (tagrdUOMDetailList.ActiveRow.DataChanged && !tagrdUOMDetailList.ActiveRow.IsUnmodifiedTemplateAddRow)
                {
                    if (undoChangesInGrid)
                    {
                        //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                        this.tagrdUOMDetailList.PerformAction(UltraGridAction.UndoCell);
                        this.tagrdUOMDetailList.PerformAction(UltraGridAction.UndoRow);
                    }
                    return true;
                }
            }
            return false;
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
        private void tabDetailList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                    case Keys.Down:
                        tagrdUOMDetailList.Focus();

                        UltraGridColumn FirstVisCol = tagrdUOMDetailList.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                        if (FirstVisCol != null)
                        {
                            tagrdUOMDetailList.ActiveCell = tagrdUOMDetailList.Rows.TemplateAddRow.Cells[FirstVisCol.Key];
                            tagrdUOMDetailList.PerformAction(UltraGridAction.EnterEditMode, false, false);
                        }

                        break;
                    case Keys.Up:
                        Custom3.Focus();
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

        //Grid Events
        private void tagrdUOMList_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            int key = 0;
            try
            {
                if (ListSyncInprogress == false)//disable selection when list synchonisation is in progress
                {
                    if (this.tagrdUOMList.ActiveRow != null && tagrdUOMList.Selected.Cells.Count > 0)//this line is required to check if user have selected a row in the list
                        key = GFunc.NEInt(tagrdUOMList.Selected.Cells[0].Row.Cells["UOMKey"].Value, 0);

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
        private void tagrdUOMList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UOMID.Focus();
            }
        }//Completed

        private void tagrdUOMDetailList_CustomDataError(object sender, TAUtil.TAErrorEventArgs e)
        {
            try
            {
                //To handle grid combo NotInList event and whether to allow add new record when NotInList
                UltraGridCell curCell = tagrdUOMDetailList.ActiveCell;

                if (e.ErrorCode == TAUtil.TAErrorCode.NOT_IN_LIST)
                {
                    if (curCell.Column.EditorComponent != null)
                    {
                        switch (curCell.Column.Key.ToLower())
                        {
                            case "uomconkey":
                                if (curCell.Text.Trim() != string.Empty)
                                    GlobalUI.ItemNotInList(tagrdUOMDetailList.ActiveCell, null, 0);
                                break;
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
        private void tagrdUOMDetailList_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {

                UltraGridCell currentCell = tagrdUOMDetailList.ActiveCell;
                switch (currentCell.Column.Key.ToLower())
                {

                    case "uomconrate":
                        currentCell.Value = GFunc.NEDec(currentCell.Value, 1);
                        break;
                }
                if (objUOMFactory.Validation_Detail(e.Cell.Row, e.Cell.Column.Key) == false)
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
        private void tagrdUOMDetailList_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            //Check Validation on the cell value and When there's an error set the grid row
            try
            {
               
                

                if (!GFunc.IsNE(objUOMFactory) && tagrdUOMDetailList.ActiveRow != null)
                {
                    if (objUOMFactory.Validation_Detail(tagrdUOMDetailList.ActiveRow, string.Empty) == false)
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
        private void tagrdUOMDetailList_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                e.DisplayPromptMsg = false;
                if (tagrdUOMDetailList.ActiveRow.IsAddRow == false)
                {
                    if (SysOptionUtility.GetBool("WarnDeleteRecordDetail"))
                    {
                        if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) != GEnum.MsgBoxButton.Delete)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
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
        private void tagrdUOMDetailList_AfterRowUpdate(object sender, RowEventArgs e)
        {
            this.objUOMFactory.ObjREFUOMDetItms.AcceptChanges();
            this.objUOMFactory.IsDirty = true;
        }//Completed
        private void tagrdUOMDetailList_AfterRowsDeleted(object sender, EventArgs e)
        {
            this.objUOMFactory.ObjREFUOMDetItms.AcceptChanges();
            this.objUOMFactory.IsDirty = true;
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
