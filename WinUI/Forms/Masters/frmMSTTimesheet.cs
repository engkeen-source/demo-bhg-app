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
using BOLib;
using Infragistics.Win.UltraWinEditors;
using TAUtil;

namespace WinUI
{ 
    public partial class frmMSTTimesheet : Form
    {
        #region Local Variables

        private BOLib.MSTTimesheetFactory objTimesheetFactory = null;
        string ContextMenuSetting = string.Empty;
        private string msgID = string.Empty;
        private bool formClose = false;

        #endregion

        // Initialize
        public frmMSTTimesheet()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        
        //Form Events
        private void frmMSTTimesheet_Load(object sender, EventArgs e)
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.objTimesheetFactory = new BOLib.MSTTimesheetFactory(BOLib.GEnum.InstanceMode.Normal);
                if (objTimesheetFactory.GUID <= 0) formClose = true;

                // Attach Event on Factory
                this.objTimesheetFactory.MSTTSheetNotifier += new GVar.UINotifierEvent(this.MSTTSheetNotifier);
                this.objTimesheetFactory.dirtyEvent += new GVar.DirtyEvent(this.OnDirty);
                //this.New_Process();
                EnableRecordControls(false);

                //UI Control
                GlobalUI.FormGrids_Set(this, (int)objTimesheetFactory.ConstantCodeKey, out ContextMenuSetting,false);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objTimesheetFactory.ConstantCodeKey);
                GlobalUI.Combos_Fill(this, (int)objTimesheetFactory.ConstantCodeKey);
               
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
                    Error(tex, true); ; // Custom Msg
                    this.formClose = true;
                }
            }
            catch (Exception ex)
            {
                Error(ex,true); // System Msg   
            }
            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }
        private void frmMSTTimesheet_Shown(object sender, EventArgs e)
        {
            // Check Form Close State is True ...
            if (formClose)
            {
                this.Close();
            }
            else
            {
                this.OpenMonth.Focus();
            }
        }
        private void frmMSTTimesheet_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }
            if (this.formClose && objTimesheetFactory == null)
            {
                return;
            }
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (!this.IsSaveChanges())
                {
                    // Call Dispose
                    bool isOk = this.objTimesheetFactory.Dispose();
                }
                else
                {
                    //When the form is closed by main form, to prohibit closing
                    frmMain.gfrmMain.Tag = GVar.CancelMainFormClosing;
                    e.Cancel = true;
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);//System Message
            }
            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }
        private void frmMSTTimesheet_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)objTimesheetFactory.ConstantCodeKey);
                    //CombosDependent_Fill(string.Empty);
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
                
        // Item Not in List
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
        }
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
        }

        //Event Methods
        private void EnableRecordControls(bool bVal)
        {
            this.SupervisorKey.Enabled = bVal;
            this.Period.Enabled = false;
            this.EmKey.Enabled = false;
            this.ItmKey.Enabled = false;
            this.OverHeadKey.Enabled = false;
            this.tagrdTimesheet.Enabled = bVal;
        }
        private void OnReadOnly()
        {
            // Set Readonly True (or) False. Based on Factory ReadOnly State       
            this.SupervisorKey.ReadOnly = this.objTimesheetFactory.IsOpenReadOnly;
            this.Period.ReadOnly = this.objTimesheetFactory.IsOpenReadOnly;
            this.EmKey.ReadOnly = this.objTimesheetFactory.IsOpenReadOnly;
            this.ItmKey.ReadOnly = this.objTimesheetFactory.IsOpenReadOnly;
            this.OverHeadKey.ReadOnly = this.objTimesheetFactory.IsOpenReadOnly;

            // Check Factory Object is ReadOnly ...
            if (this.objTimesheetFactory.IsOpenReadOnly)
            {
                this.tslReadOnly.Text = "Read Only";
                this.tsbSave.Enabled = false;
            }
            else
            {
                this.tslReadOnly.Text = string.Empty;
                this.tsbSave.Enabled = true;
            }

            // Clear Error
            this.errorProvider1.Clear();
        }
        private bool IsSaveChanges()
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            // Variable Declaration
            bool isSave = false;

            try
            {
                // Check Form Validation
                this.Validate();

                // Check Factory Object is Dirty ...
                if (objTimesheetFactory.IsDirty)
                {
                    // Ask Confirmation To Save
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show(MsgID.Common.SaveChanges,
                                          GEnum.MsgBoxIcon.Question,
                                          GEnum.MsgBoxButton.Save_Changes,
                                          GEnum.MsgBoxButton.Discard_Changes,
                                          GEnum.MsgBoxButton.I_Dont_Know);

                    // Yes, I want to save
                    if (btnSelect == GEnum.MsgBoxButton.Save_Changes)
                        isSave = !this.Save_Process();
                    // No, I don't know 
                    else if (btnSelect == GEnum.MsgBoxButton.I_Dont_Know)
                        isSave = true;
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
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
            return isSave;
        }
        private bool New_Process()
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            //Variable Declaration
            bool isNew = true;

            try
            {
                // Clear Error
                this.errorProvider1.Clear();

                // Check Form Validation 
                this.Validate();

                // Check Factory Object is Dirty ...
                if (this.objTimesheetFactory.IsDirty)
                {
                    // Call Save
                    isNew = this.Save_Process();
                }

                // Check IsNew is True ... 
                if (isNew)
                {
                    // Call New
                    isNew = this.objTimesheetFactory.New();                    
                }

                // Check IsNew is True ... 
                if (isNew)
                {
                    this.Refresh_TimesheetInfo();
                    //this.FormatOtherGrid();
                    // Call ReadOnly
                    this.OnReadOnly();
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
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
            return isNew;
        }
        private bool Save_Process()
        {
            // Waiting cursor
            this.Cursor = Cursors.WaitCursor;

            // Variable Declaration
            bool isSave = false;

            try
            {
                // Validation
                bool isOK = this.Validate();
                if (!GFunc.IsNE(tagrdTimesheet.ActiveRow))
                {
                    if (!tagrdTimesheet.ActiveRow.IsUnmodifiedTemplateAddRow)
                    {
                        this.tagrdTimesheet.ActiveRow.Update();
                    }
                }
                // Check Factory Object is not null
                if (isOK && this.objTimesheetFactory != null)
                {
                    // Save Sales Rep
                    isSave = this.objTimesheetFactory.Save();

                    
                    // Check Process
                    if (isSave)
                    {                        
                        this.errorProvider1.Clear();

                        // Call ReadOnly
                        this.OnReadOnly();
                    }
                    else
                    {
                        throw new TAException(objTimesheetFactory.ErrorMessageID);
                    }
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
                this.Refresh_TimesheetInfo();
                // Default cursor
                this.Cursor = Cursors.Default;
            }
            return isSave;
        }
        private void Refresh_TimesheetInfo()
        {
            try
            {
                // Object binding for Timesheet Information 
                //this.bdsTimesheetDet.DataSource = objTimesheetFactory.ObjMSTTimesheet;
                //this.bdsTimesheetDet.ResetBindings(false);
                if (objTimesheetFactory.ObjMSTJobDetOthers == null)
                    return;
                tagrdTimesheet.DataSource = objTimesheetFactory.ObjMSTJobDetOthers;
                tagrdTimesheet.DataBind();
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
        private void FillUnboundData()
        {
            try
            {
                foreach (UltraGridRow dr in tagrdTimesheet.Rows)
                {
                    string combineText = string.Empty;

                    if (GFunc.IsNEZ(dr.Cells["JobKey"].Value))
                        break;
                    combineText = dr.Cells["JobKey"].Text + ","
                        + dr.Cells["JobPhaseKey"].Text + ","
                        + dr.Cells["JobTaskKey"].Text + ","
                        + dr.Cells["JobCostTypeKey"].Text;

                    dr.Cells["PhaseTask"].Value = combineText;
                    dr.Update();
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
        
        private void OnDirty(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName.ToLower())
            {
                case "supervisorkey":
                    this.errorProvider1.SetError(this.SupervisorKey, string.Empty);
                    break;
                case "month":
                    this.errorProvider1.SetError(this.Period, string.Empty);
                    break;
                case "emkey":
                    this.errorProvider1.SetError(this.EmKey, string.Empty);
                    break;
                case "itmkey":
                    this.errorProvider1.SetError(this.ItmKey, string.Empty);
                    break;
                case "overheadkey":
                    this.errorProvider1.SetError(this.OverHeadKey, string.Empty);
                    break;
            }
        }
        private void MSTTSheetNotifier(object sender, BOLib.UINotifierEventArgs e)
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

        //Menu Strip Events
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.formClose = true;
            this.Close();
        }
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
                Error(ex, true); // System Msg   
            }
        }

        //Button Events
        private void btnOpenRecord_Click(object sender, EventArgs e)
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            bool isOk = false;

            try
            {
                //Save Changes
                if (formClose)
                    return;

                isOk = this.IsSaveChanges();

                if ((isOk) && msgID == string.Empty)
                {
                    //I Don't Know
                    if (this.OpenMonth.Value != "" && this.OpenEmployee.Value != "")
                        this.SupervisorKey.Focus();
                    return;
                }
                else if ((isOk) && msgID != string.Empty)
                {
                    //Validation Fail
                    this.SupervisorKey.Focus();
                    return;
                }

                //Check if not empty
                if (GFunc.IsNE(this.OpenMonth.Value))
                {
                    msgID = "OpenMonth" + MsgID.Validation.IsRequire;
                    MsgBox.Show(msgID);
                    return;
                }

                if (GFunc.IsNE(this.OpenEmployee.Value))
                {
                    msgID = "OpenEmployee" + MsgID.Validation.IsRequire;
                    MsgBox.Show(msgID);
                    return;
                }

                //New Select
                this.Period.SetValueTrigger(this.OpenMonth.Value,false);

                int nEmKey = (int)this.OpenEmployee.Value;
                string sPeriod = this.OpenMonth.Value.ToString().Substring(4, 2) + "/01/" + this.OpenMonth.Value.ToString().Substring(0, 4);
                DateTime? dtPeriod = Convert.ToDateTime(sPeriod);

                // Call New
                isOk = objTimesheetFactory.New();

                //this.Refresh_TimesheetInfo();

                // Call GetEdit
                isOk = objTimesheetFactory.GetEdit(nEmKey, dtPeriod);


                if (!isOk)
                {
                    if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnRestoreRecord))
                    {
                        // Ask Confirmation For ReadOnly
                        GEnum.MsgBoxButton btnSelect;
                        btnSelect = MsgBox.Show(MsgID.Common.ResponseOpenAsReadOnly,
                                              GEnum.MsgBoxIcon.Warning,
                                              GEnum.MsgBoxButton.Yes,
                                              GEnum.MsgBoxButton.No,
                                              GEnum.MsgBoxButton.I_Dont_Know);

                        // Yes, i want to use as readonly
                        if (btnSelect == GEnum.MsgBoxButton.Yes)
                        {
                            // Call ReadOnly
                            isOk = this.objTimesheetFactory.GetReadOnly(nEmKey);

                        }
                        // No, i don't want
                        else if (btnSelect == GEnum.MsgBoxButton.No)
                        {
                            // Call Edit
                            isOk = objTimesheetFactory.GetEdit(this.objTimesheetFactory.ObjMSTTimesheet.EmKey, this.objTimesheetFactory.ObjMSTTimesheet.Period);

                        }
                        // Cancel Process
                        else
                            return;
                    }
                    else
                    {
                        // Call ReadOnly
                        isOk = this.objTimesheetFactory.GetReadOnly(nEmKey);
                    }
                }

                //Enable controls
                this.EnableRecordControls(true);

                // Call ReadOnly
                this.OnReadOnly();

                this.Refresh_TimesheetInfo();
                this.FillUnboundData();
                string sDate = this.OpenMonth.Value.ToString().Substring(0, 4) + "/" + this.OpenMonth.Value.ToString().Substring(4, 2) + "/01";
                tagrdTimesheet.DefaultValue = Convert.ToDateTime(sDate);

                //this.tagrdTimesheet.Rows[0].Cells["DocDate"].Value = Convert.ToDateTime(sDate);
                this.tagrdTimesheet.Focus();
                this.errorProvider1.Clear();
                
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true); // System Msg
            }
            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }

        //Grid Events
        private void tagrdTimesheet_AfterCellUpdate(object sender, CellEventArgs e)
        {

        }
        private void tagrdTimesheet_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            try
            {
                if (objTimesheetFactory.UserTimesheetPermission.Delete(ref msgID))
                {
                    this.Cursor = Cursors.WaitCursor;
                    e.DisplayPromptMsg = false;
                    tagrdTimesheet.UpdateData();
                    if (tagrdTimesheet.Rows.Count > 0)
                    {
                        if (tagrdTimesheet.ActiveRow.IsAddRow == false)
                        {
                            if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) != GEnum.MsgBoxButton.Delete)
                            {
                                e.Cancel = true;
                                return;
                            }
                            return;
                        }
                    }
                    e.Cancel = true;  
                }
                else
                {
                    e.Cancel = true;
                    throw new TAException(msgID);
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
        }
        private void tagrdTimesheet_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
                bool processOk = true;

               
                decimal cost = 0;
                decimal qty = 0;

                UltraGridCell currentCell = tagrdTimesheet.ActiveCell;
                processOk = true;
                switch (currentCell.Column.Key.ToLower())
                {
                    case "docdate":
                        if (currentCell.Text != "")
                        {
                            if (Convert.ToDateTime(currentCell.Text).Month != Convert.ToDateTime(objTimesheetFactory.ObjMSTTimesheet.Period).Month)
                            {
                                msgID = "DocDate" + MsgID.Validation.DataKeyInvalid;
                            }
                            else
                            {
                                msgID = "";
                            }
                        }
                        break;

                    case "phasetask":
                        if (currentCell.Value == null || currentCell.Value.ToString() == "0")
                        {
                            msgID = "PhaseTaskIsRequire";
                            processOk = false;
                        }
                        if (processOk)
                        {
                            if (currentCell.Row.Index <= this.tagrdTimesheet.Rows.Count - 1 && this.tagrdTimesheet.Rows[currentCell.Row.Index].Cells["PhaseTask"].Value.ToString() != string.Empty)
                            {
                                if (GFunc.IsNEZ(currentCell.Row.Cells["JobKey"].Value))
                                {
                                    //Get Keys from AllKeys
                                    int key = 0;
                                    if (currentCell.Value.ToString().Contains(",")) //JobKey,Phase,Task,CostType
                                    {
                                        string[] keyList = currentCell.Value.ToString().Split(',');

                                        if (keyList.Length > 2)
                                        {
                                            Int32.TryParse(keyList[0], out key);
                                            currentCell.Row.Cells["JobKey"].Value = key;

                                            Int32.TryParse(keyList[1], out key);
                                            currentCell.Row.Cells["JobPhaseKey"].Value = key;

                                            Int32.TryParse(keyList[2], out key);
                                            currentCell.Row.Cells["JobTaskKey"].Value = key;

                                            Int32.TryParse(keyList[3], out key);
                                            currentCell.Row.Cells["JobCostTypeKey"].Value = key;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
                if (!processOk)
                {
                    e.Cancel = true;


                    if (tagrdTimesheet.ActiveRow.Index < objTimesheetFactory.ObjMSTJobDetOthers.Rows.Count)
                    {
                        objTimesheetFactory.ObjMSTJobDetOthers.Rows[tagrdTimesheet.ActiveRow.Index].RowError = SysMessageUtility.Get(msgID);
                        throw new TAException(msgID);
                    }
                    else
                    {
                        throw new TAException(msgID);
                    }
                }
                else
                {
                    foreach (DataRow dr in objTimesheetFactory.ObjMSTJobDetOthers.Rows)
                    {
                        dr.RowError = string.Empty;
                    }
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true); // System Msg   
            }
        }
        private void tagrdTimesheet_ClickCellButton(object sender, CellEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;
            try
            {
                if (GFunc.CompareString(e.Cell.Column.Key , "OthItmDes"))
                {
                    string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "OthItmDes", tagrdTimesheet.Name);
                    if (DocHDRUtil.EditorButton_Popup((int)objTimesheetFactory.ConstantCodeKey, e.Cell.Text, listSettingID, (int)GEnum.PopupType.ItmDes, ref key, ref id, ref des))
                    {
                        tagrdTimesheet.ActiveRow.Cells["OthItmKey"].Value = key;
                        tagrdTimesheet.ActiveRow.Cells["OthItmKeySelect"].Value = key;
                        tagrdTimesheet.ActiveRow.Cells["OthItmID"].Value = id;
                        tagrdTimesheet.ActiveRow.Cells["OthItmDes"].Value = des;
                    }
                }
                if (GFunc.CompareString(e.Cell.Column.Key , "OthItmID"))
                {
                    string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "OthItmID", tagrdTimesheet.Name);
                    if (DocHDRUtil.EditorButton_Popup((int)objTimesheetFactory.ConstantCodeKey, e.Cell.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref key, ref id, ref des))
                    {
                        tagrdTimesheet.ActiveRow.Cells["OthItmKey"].Value = key;
                        tagrdTimesheet.ActiveRow.Cells["OthItmKeySelect"].Value = key;
                        tagrdTimesheet.ActiveRow.Cells["OthItmID"].Value = id;
                        tagrdTimesheet.ActiveRow.Cells["OthItmDes"].Value = des;
                    }
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
        private void tagrdTimesheet_AfterRowsDeleted(object sender, EventArgs e)
        {
            objTimesheetFactory.IsDirty = true;
            objTimesheetFactory.ObjMSTJobDetOthers.AcceptChanges();
        }
        private void tagrdTimesheet_AfterRowUpdate(object sender, RowEventArgs e)
        {
            objTimesheetFactory.IsDirty = true;
            objTimesheetFactory.ObjMSTJobDetOthers.AcceptChanges();
        }


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

                                switch (grd.ActiveCell.Column.Key)
                                {
                                    case "jobkey":
                                    case "jobotherkey":
                                    case "jobphasekey":
                                    case "jobtaskkey":
                                    case "jobcosttypekey":
                                    case "emkey":
                                    case "othitmkey":
                                    case "othitmkeyselect":
                                    case "othuomkey":
                                    case "doccurrkey":

                                        GlobalUI.ItemNotInList(grd.ActiveCell, null, 1);// ItemNotInListAdd
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

        }//CodeCompleted
        private void tagrdTimesheet_BeforeRowDeactivate(object sender, CancelEventArgs e)
        {
            try
            {
                bool processOk = true;
                if (!GFunc.IsNE(this.objTimesheetFactory.ObjMSTJobDetOthers) && this.tagrdTimesheet.ActiveRow != null)
                {
                    if (processOk)
                    {
                        processOk = BaseUtility.Validation(out msgID, this.tagrdTimesheet.ActiveRow.Cells["PhaseTask"].Value, "PhaseTask", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null);
                        if (!processOk)
                        {
                            msgID = "PhaseTaskIsRequire";
                        }
                    }
                    if (processOk)
                    {
                        if (Convert.ToDateTime(tagrdTimesheet.ActiveRow.Cells["DocDate"].Value.ToString()).Month != Convert.ToDateTime(objTimesheetFactory.ObjMSTTimesheet.Period).Month)
                        {
                            msgID = "DocDate" + MsgID.Validation.DataKeyInvalid;
                            processOk = false;
                        }
                    }

                    if (!processOk)
                    {
                        e.Cancel = true;
                        if (tagrdTimesheet.ActiveRow.Index < objTimesheetFactory.ObjMSTJobDetOthers.Rows.Count)
                        {
                            objTimesheetFactory.ObjMSTJobDetOthers.Rows[tagrdTimesheet.ActiveRow.Index].RowError = SysMessageUtility.Get(msgID);
                            throw new TAException(msgID);
                        }
                        throw new TAException(msgID);
                    }
                    else
                    {
                        foreach (DataRow dr in objTimesheetFactory.ObjMSTJobDetOthers.Rows)
                        {
                            dr.RowError = string.Empty;
                        }
                    }
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true); // System Msg   
            }
        }        

        #region Error

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

        #endregion               
      
    }
}
