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
    public partial class frmREFInterest : Form
    {
        #region Local Variables
        private BOLib.REFInterestFactory objInterestFactory = null;
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
        public frmREFInterest()
        {
            InitializeComponent();
        }//Completed
        public frmREFInterest(string id)
        {
            InitializeComponent();
            recordID = id;
            formOpenMode = GEnum.formInitMode.Add;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }//Completed
        public frmREFInterest(int Key)
        {
            //For call from shortcut menu (Edit)
            InitializeComponent();
            formOpenMode = GEnum.formInitMode.Edit;
            recordKey = Key;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }//Completed
        public frmREFInterest(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            //Open from AuditLog as Read only data viewer
            InitializeComponent();
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
        }//Completed

        //Form Events
        private void frmREFInterest_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.tagrdInterest.DisplayLayout.Bands[0].SortedColumns.Clear();
                //Call Initialization
                this.objInterestFactory = new BOLib.REFInterestFactory(BOLib.GEnum.InstanceMode.Normal);
                if (objInterestFactory.GUID <= 0)
                {
                    formClose = true;
                    return;
                }

                //Attach Event on Factory
                this.objInterestFactory.ErrorNotifierHeader_Set += new GVar.UINotifierEvent(this.ErrorNotifier_Set);
                this.objInterestFactory.ErrorNotifierHeader_Clear += new GVar.UINotifierEvent(this.ErrorNotifier_Clear);

                //Check if user has permission to edit Record ID
                canEditRecordID = SECPermUtility.Perform("sysRecID", false);

                if (this.IsOpenFromAuditLog)
                {
                    if (objInterestFactory.SetReadOnlyData(_dtHeader, _dsDetail) == false)
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
                            this.IntID.SetValueTrigger(recordID, false);
                    }
                }

                //Setup Grid Layout, ShortcutMenu, fill Combo
                GlobalUI.FormGrids_Set(this, (int)objInterestFactory.ConstantCodeKey, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objInterestFactory.ConstantCodeKey);
                //GlobalUI.cmnuGlobal_Set(this);
                GlobalUI.Combos_Fill(this, (int)objInterestFactory.ConstantCodeKey);

                //Setup List properties
                this.tagrdInterest.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                this.tagrdInterest.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
                this.tagrdInterest.DisplayLayout.Bands[0].Override.ActiveCellAppearance.BackColor = Color.Gold;

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
        private void frmREFInterest_Shown(object sender, EventArgs e)
        {
            //Close Form when FormLoad has encounter errors
            if (formClose)
                this.Close();
        }//Completed
        private void frmREFInterest_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool runProcess = false;
            this.formClose = true;

            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }
            if (this.formClose && objInterestFactory == null)
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
                if ((bool)this.objInterestFactory.Dispose() == false)
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
                    this.objInterestFactory.Dispose();
            }
        }//Completed
        private void frmREFInterest_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)objInterestFactory.ConstantCodeKey);
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
        private void Refresh_Form(bool skipRefreshGridList)
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
                this.bdsInterestDet.DataSource = objInterestFactory.ObjREFInterest;
                this.bdsInterestDet.AllowNew = true;
                this.bdsInterestDet.ResetBindings(false);

                ItmKey.SetValueTrigger(string.Empty, false);
                if (GFunc.NEInt(objInterestFactory.ObjREFInterest.ItmKey,0) != 0)
                {
                    if (tagrdInterest.Selected.Cells.Count > 0)
                    {
                        ItmKey.SetValueTrigger(GFunc.NEStr(tagrdInterest.Selected.Cells[0].Row.Cells["ItmID"].Value, string.Empty),false);
                    }
                }
                ItmDesDoc.SetValueTrigger(objInterestFactory.ObjREFInterest.ItmDesDoc,false);
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
                    string listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "0GF", tagrdInterest.Name);
                    GlobalUI.Grid_Format(tagrdInterest, listID, true, false);
                }
                if (this.tagrdInterest.DisplayLayout.Bands[0].SortedColumns.Count <= 0)
                    this.tagrdInterest.DisplayLayout.Bands[0].SortedColumns.Add(tagrdInterest.DisplayLayout.Bands[0].Columns["IntID"], false);
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
            bool EnableMode = !this.objInterestFactory.IsReadOnly;
            this.tslReadOnly.Text = !EnableMode ? "Read Only" : string.Empty;

            //Special Condition for RecordID
            this.IntID.Enabled = EnableMode;
            this.IntID.ReadOnly = !canEditRecordID;

           
            this.IntDes.Enabled = EnableMode;
            this.Custom1.Enabled = EnableMode;
            this.Custom2.Enabled = EnableMode;
            this.Custom3.Enabled = EnableMode;
            this.AnnualIntRate.Enabled = EnableMode;
            this.MinCharge.Enabled = EnableMode;
            this.ItmKey.Enabled = EnableMode;
            this.ItmDesDoc.Enabled = EnableMode;
            this.IntOnInt.Enabled = EnableMode;

            

            if (EnableMode == false)
            {
                this.tsbSave.Enabled = false;
                this.tsbDelete.Enabled = false;
                this.tsbClear.Enabled = false;
            }
            else
            {
                this.tsbSave.Enabled = true;
                if (this.objInterestFactory.IsNew)
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
                if (objInterestFactory.ObjREFInterest.IntKey > 0)
                {
                    //When The Header.RecordKey is the same as the List.RecordKey, we do not need to select the related row on the list
                    //this will prevent the list from opening the record again
                    if (tagrdInterest.Selected != null)
                        if (tagrdInterest.Selected.Cells.Count > 0)
                            if (GFunc.NEInt(tagrdInterest.Selected.Cells[0].Row.Cells["IntKey"].Value, 0) == objInterestFactory.ObjREFInterest.IntKey)
                                return;//already selected.

                    //Select the row on the list to reflect the current value in Header.recordkey
                    UltraGridRow ToSelectRow = this.tagrdInterest.Rows.OfType<UltraGridRow>().ToList().Find(
                    row => row.Cells["IntKey"].Text.Equals(objInterestFactory.ObjREFInterest.IntKey.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    if (ToSelectRow != null)
                    {
                        ToSelectRow.Cells["IntID"].Selected = true;
                        ToSelectRow.Cells["IntID"].Activate();
                    }
                }
                else
                {
                    tagrdInterest.Selected.Cells.Clear();
                    tagrdInterest.ActiveRow = null;
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
                        this.objInterestFactory.IsDirty = false;
                    }
                }

                this.errorProvider1.Clear();

                if (this.objInterestFactory.IsDirty)
                {
                    if (Save_Process() == false)
                        return false;
                }

                if (this.objInterestFactory.New() == false)
                {                  
                    return false;
                }
                else
                {                   
                    this.errorProvider1.Clear();
                 
                    this.IntID.Focus();
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

                if (objInterestFactory.IsDirty)
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
                if (this.objInterestFactory.Save())
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
                if (SECPermUtility.Edit(objInterestFactory.PermID, false))
                {
                    if (objInterestFactory.GetEdit(key) == false)
                    {
                        if (SysOptionUtility.GetBool("WarnOpenRecordAsReadOnly"))
                        {
                            btnSelect = MsgBox.Show("Try to open record as read only",
                                                  GEnum.MsgBoxIcon.Question,
                                                  GEnum.MsgBoxButton.Yes,
                                                  GEnum.MsgBoxButton.No);

                            if (btnSelect == GEnum.MsgBoxButton.Yes)
                            {
                                objInterestFactory.GetReadOnly(key);
                            }
                        }
                    }
                }
                else
                    objInterestFactory.GetReadOnly(key);

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

                if (this.objInterestFactory.Delete())
                {
                    this.objInterestFactory.New();
                   
                    //Move the cursor position of active row index to upper row
                    if (tagrdInterest.ActiveRow.Index > 0)
                        PreRowIndex = tagrdInterest.ActiveRow.Index - 1;

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
                if (tagrdInterest.Rows.Count > 0)
                {
                    tagrdInterest.Rows[PreRowIndex].Selected = true;
                    tagrdInterest.Rows[PreRowIndex].Activate();
                }
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private bool Clear_Process()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (GFunc.IsNEZ(this.objInterestFactory.ObjREFInterest.IntKey))
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

                    if (this.objInterestFactory.New())
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
        private void tagrdInterest_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                IntID.Focus();
            }
        }//Completed
        private void tagrdInterest_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            int key = 0;
            try
            {
                if (ListSyncInprogress == false)//disable selection when list synchonisation is in progress
                {
                    if (this.tagrdInterest.ActiveRow != null && tagrdInterest.Selected.Cells.Count > 0)//this line is required to check if user have selected a row in the list
                        key = GFunc.NEInt(tagrdInterest.Selected.Cells[0].Row.Cells["IntKey"].Value, 0);

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

        //Control Events
        private void ItmID_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;
            try
            {
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItmID");                
                if (DocHDRUtil.EditorButton_Popup((int)objInterestFactory.ConstantCodeKey, ItmKey.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref key, ref id, ref des))
                {                
                    ItmKey.SetValueTrigger(id,false);
                    ItmDesDoc.SetValueTrigger(des,false);
                    objInterestFactory.ObjREFInterest.ItmKey = key;
                    objInterestFactory.ObjREFInterest.ItmDesDoc = des;
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
        private void ItmID_CustomUpdate(object sender, CancelEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;

            try
            {
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItmID");

                key = GFunc.ItmRecord_GetKey(GEnum.RecAccessType.ItemID, listSettingID, ItmKey.Text, 0, ref id, ref des, true);
                if (key == 0)
                {
                    //since value input by user cannot be match let the user select from Popup form
                    if (DocHDRUtil.EditorButton_Popup((int)objInterestFactory.ConstantCodeKey, ItmKey.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref key, ref id, ref des))
                    {
                        //set control to the match record found
                        objInterestFactory.ObjREFInterest.ItmKey = key;
                        ItmKey.SetValueTrigger(id,false);
                        ItmDesDoc.SetValueTrigger(des,false);
                    }
                    else
                    {
                        //when user is still unable to select a matching record, undo the changes
                        MsgBox.Show("Please use a valid value");
                        e.Cancel = true;
                    }
                }
                else
                {
                    ItmKey.SetValueTrigger(id, false);
                    ItmDesDoc.SetValueTrigger(des, false);
                    objInterestFactory.ObjREFInterest.ItmKey = key;
                    objInterestFactory.ObjREFInterest.ItmDesDoc = des;
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
        private void ItmDesDoc_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;

            try
            {
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItmDesDoc");
                if (DocHDRUtil.EditorButton_Popup((int)objInterestFactory.ConstantCodeKey, ItmDesDoc.Text, listSettingID, (int)GEnum.PopupType.ItmDes, ref key, ref id, ref des))
                {

                    ItmKey.SetValueTrigger(id, false);
                    ItmDesDoc.SetValueTrigger(des, false);
                    objInterestFactory.ObjREFInterest.ItmKey = key;
                    objInterestFactory.ObjREFInterest.ItmDesDoc = des;

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
        private void ItmDesDoc_CustomUpdate(object sender, CancelEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;
            
            try
            {
                //if ItemID is not empty, we don't show the popup even if item description is not valid. 
                if (ItmKey.Text != string.Empty && ItmDesDoc.Text != string.Empty)
                {
                    objInterestFactory.ObjREFInterest.ItmDesDoc = ItmDesDoc.Text;
                    return;
                }

                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItmDesDoc");
                key = GFunc.ItmRecord_GetKey(GEnum.RecAccessType.ItemDes, listSettingID, ItmDesDoc.Text, 0, ref id, ref des, true);
                if (key == 0)
                {
                    //since value input by user cannot be match let the user select from Popup form
                    if (DocHDRUtil.EditorButton_Popup(0, ItmDesDoc.Text, listSettingID, (int)GEnum.PopupType.ItmDes, ref key, ref id, ref des))
                    {
                        //set control to the match record found
                        objInterestFactory.ObjREFInterest.ItmKey = key;
                        ItmKey.SetValueTrigger(id, false);
                        ItmDesDoc.SetValueTrigger(des, false);
                    }
                    else
                    {
                        //when user is still unable to select a matching record, undo the changes
                        MsgBox.Show("Please use a valid value");
                        e.Cancel = true;
                    }
                }
                else
                {

                    ItmKey.SetValueTrigger(id, false);
                    ItmDesDoc.SetValueTrigger(des, false);
                    objInterestFactory.ObjREFInterest.ItmKey = key;
                    objInterestFactory.ObjREFInterest.ItmDesDoc = des;
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

        private void AnnualIntRate_CustomUpdate(object sender, CancelEventArgs e)
        {
            string msgValue = string.Empty;
            try
            {
                string vAnnualInRate = AnnualIntRate.Text.Replace("%","");
                if (!BaseUtility.Validation(out msgID, vAnnualInRate, "AnnualIntRate", GEnum.DataType.Decimel, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null))
                {
                    errorProvider1.SetError(AnnualIntRate, SysMessageUtility.Get(msgID));
                }
                else
                {
                    errorProvider1.SetError(AnnualIntRate, string.Empty);
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
        private void MinCharge_CustomUpdate(object sender, CancelEventArgs e)
        {
            string msgValue = string.Empty;
            try
            {
                if (!BaseUtility.Validation(out msgID, MinCharge.Text, "MinCharge", GEnum.DataType.Decimel, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null))
                {
                    errorProvider1.SetError(MinCharge, SysMessageUtility.Get(msgID));
                }
                else
                {
                    errorProvider1.SetError(MinCharge, string.Empty);
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
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
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