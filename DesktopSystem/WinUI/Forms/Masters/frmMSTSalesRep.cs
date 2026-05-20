using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using BOLib;
using Infragistics.Win.UltraWinTabbedMdi;
using TAUtil;
namespace WinUI
{
    public partial class frmMstSalesRep : Form
    {
        #region Local Variables
        private BOLib.MSTSalesRepFactory objSalesRepFactory = null;
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
        int PreRowIndex = 0;
        #endregion

        //Initialize
        public frmMstSalesRep()
        {
            InitializeComponent();
        }//Completed
        public frmMstSalesRep(string id)
        {
            //For Call from Shortcut menu (Edit/Add)
            InitializeComponent();
            recordID = id;
            formOpenMode = GEnum.formInitMode.Add;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }//Completed
        public frmMstSalesRep(int key)
        {
            //For call from shortcut menu (Edit)
            InitializeComponent();
            formOpenMode = GEnum.formInitMode.Edit;
            recordKey = key;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }//Completed
        public frmMstSalesRep(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            //Open from AuditLog as Read only data viewer
            InitializeComponent();
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
        }//Completed

        //Form Events
        private void frmMSTSalesRep_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.tagrdSalesRep.DisplayLayout.Bands[0].SortedColumns.Clear();
                //Call Initialization
                this.objSalesRepFactory = new BOLib.MSTSalesRepFactory(BOLib.GEnum.InstanceMode.Normal);
                

                if (objSalesRepFactory.GUID <= 0)
                {
                    formClose = true;
                    return;
                }

                //Attach Event on Factory
                this.objSalesRepFactory.ErrorNotifierHeader_Set += new GVar.UINotifierEvent(this.ErrorNotifier_Set);
                this.objSalesRepFactory.ErrorNotifierHeader_Clear += new GVar.UINotifierEvent(this.ErrorNotifier_Clear);

                //Check if user has permission to edit Record ID
                canEditRecordID = SECPermUtility.Perform("sysRecID", false);

                if (this.IsOpenFromAuditLog)
                {
                    if (objSalesRepFactory.SetReadOnlyData(_dtHeader, _dsDetail) == false)
                    {
                        formClose = true;
                        return;
                    }
                    this.Refresh_All(false);
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
                            this.EmID.SetValueTrigger(recordID, false);
                    }
                }

                //Setup Grid Layout, ShortcutMenu, fill Combo
                GlobalUI.FormGrids_Set(this, (int)objSalesRepFactory.ConstantCodeKey, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objSalesRepFactory.ConstantCodeKey);
                //GlobalUI.cmnuGlobal_Set(this);
                GlobalUI.Combos_Fill(this, (int)objSalesRepFactory.ConstantCodeKey);

                //Setup List properties
                this.tagrdSalesRep.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                this.tagrdSalesRep.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
                this.tagrdSalesRep.DisplayLayout.Bands[0].Override.ActiveCellAppearance.BackColor = Color.Gold;
                this.tagrdSalesRep.DisplayLayout.Bands[0].Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;

                //Set Default Values
                tagrdExpenditureList.DisplayLayout.Bands[0].Columns["TransAmt"].DefaultCellValue = 0;
                SqlConnection cn = new SqlConnection(Database.BossDemoConnection);
                cn.Open();                
                tagrdApproverList.DisplayLayout.Bands[0].Columns["SaleLimit"].DefaultCellValue = SysOptionUtility.GetDec("SecondApproverLimitForARQO", cn);                
                tagrdApproverList.DisplayLayout.Bands[0].Columns["ProfitMarginLimit"].DefaultCellValue = SysOptionUtility.GetInt("SecondApproverProfitItemLimitForARQO", cn);
                cn.Close();
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
        private void frmMSTSalesRep_Shown(object sender, EventArgs e)
        {
            //Close Form when FormLoad has encounter errors
            try
            {
                if (formClose)
                    this.Close();
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
        private void frmMSTSalesRep_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool runProcess = false;
            this.formClose = true;

            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }

            if (this.formClose && objSalesRepFactory == null)
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
                if ((bool)this.objSalesRepFactory.Dispose() == false)
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
                    this.objSalesRepFactory.Dispose();
            }
        }//Completed
        private void frmMSTSalesRep_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                    GlobalUI.Combos_Fill(this, (int)objSalesRepFactory.ConstantCodeKey);

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
            this.formClose = true;
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
                Refresh_GridDet();
                if (skipRefreshGridList == false)
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

                this.bdsSalesRepDet.DataSource = objSalesRepFactory.ObjMSTSalesRep;
                this.bdsSalesRepDet.AllowNew = true;
                this.bdsSalesRepDet.ResetBindings(false);
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
                    string listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "0GF", tagrdSalesRep.Name);
                    GlobalUI.Grid_Format(tagrdSalesRep, listID, true, false);
                }
                if (this.tagrdSalesRep.DisplayLayout.Bands[0].SortedColumns.Count <= 0)
                    this.tagrdSalesRep.DisplayLayout.Bands[0].SortedColumns.Add(tagrdSalesRep.DisplayLayout.Bands[0].Columns["EmID"], false);
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

                tagrdExpenditureList.DataSource = objSalesRepFactory.ObjMSTSalesRepPayrolls;
                tagrdExpenditureList.Rows.Refresh(RefreshRow.ReloadData);

                tagrdApproverList.DataSource = objSalesRepFactory.ObjMSTSalesRepApprovers;
                tagrdApproverList.Rows.Refresh(RefreshRow.ReloadData);
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
            try
            {
                bool EnableMode = !this.objSalesRepFactory.IsReadOnly;
                bool PayrollIsReadOnly = false;
                bool ApproverIsReadOnly = false;
                this.tslReadOnly.Text = !EnableMode ? "Read Only" : string.Empty;

                //Special Condition for RecordID
                this.EmID.Enabled = EnableMode;
                this.EmID.ReadOnly = !canEditRecordID;

                this.EmNm.Enabled = EnableMode;
                this.EmClass.Enabled = EnableMode;
                this.EmDOB.Enabled = EnableMode;
                this.EmRef.Enabled = EnableMode;
                this.JobLabourItmID.Enabled = EnableMode;
                this.JobLabourItmDes.Enabled = EnableMode;
                this.UserKey.Enabled = EnableMode;
                this.JobCostGrpKey.Enabled = EnableMode;
                this.Inactive.Enabled = EnableMode;
                this.DateHired.Enabled = EnableMode;
                this.DateTerminated.Enabled = EnableMode;
                this.Custom1.Enabled = EnableMode;
                this.Custom2.Enabled = EnableMode;
                this.Custom3.Enabled = EnableMode;
                this.EmEmail.Enabled = EnableMode;
                this.Custom5.Enabled = EnableMode;

                if (SECPermUtility.Any(GVar.PermissionID.Sales_Representative_Payroll, out PayrollIsReadOnly, false) == false)
                    this.tagrdExpenditureList.Visible = false;
                else
                    this.tagrdExpenditureList.Visible = true;
                
                if (SECPermUtility.Any(GVar.PermissionID.Sales_Approver, out ApproverIsReadOnly, false) == false)
                    this.tagrdApproverList.Visible = false;
                else
                    this.tagrdApproverList.Visible = true;


                if (EnableMode == false)
                {
                    foreach (UltraGridColumn gcol in tagrdExpenditureList.DisplayLayout.Bands[0].Columns)
                    {
                        switch (gcol.Key.ToLower())
                        {
                            case "emkey":
                            case "transkey":
                                gcol.CellActivation = Activation.ActivateOnly;
                                break;

                            default:
                                gcol.CellActivation = Activation.AllowEdit;
                                break;
                        }
                    }
                    this.tagrdExpenditureList.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
                    this.tagrdExpenditureList.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;

                    foreach (UltraGridColumn gcol in tagrdApproverList.DisplayLayout.Bands[0].Columns)
                    {
                        switch (gcol.Key.ToLower())
                        {
                            //case "salelimit":
                            //    gcol.CellActivation = Activation.NoEdit;
                            //    break;
                            //case "profitmarginlimit":
                            //    gcol.CellActivation = Activation.NoEdit;
                            //    break;
                            default:
                                gcol.CellActivation = Activation.AllowEdit;
                                break;
                        }
                    }
                    this.tagrdApproverList.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
                    this.tagrdApproverList.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;

                    this.tsbSave.Enabled = false;
                    this.tsbDelete.Enabled = false;
                    this.tsbClear.Enabled = false;
                }
                else
                {
                    Activation activation = Activation.ActivateOnly;
                    if (SECPermUtility.Edit(GVar.PermissionID.Sales_Representative_Payroll, false))
                        activation = Activation.AllowEdit;

                    foreach (UltraGridColumn gcol in tagrdExpenditureList.DisplayLayout.Bands[0].Columns)
                    {
                        switch (gcol.Key.ToLower())
                        {
                            case "emkey":
                            case "transkey":
                                gcol.CellActivation = Activation.ActivateOnly;
                                break;

                            default:

                                gcol.CellActivation = activation;
                                break;
                        }
                    }

                    if (activation == Activation.ActivateOnly)
                    {
                        this.tagrdExpenditureList.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
                        this.tagrdExpenditureList.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                    }
                    else
                    {
                        this.tagrdExpenditureList.DisplayLayout.Override.AllowDelete = DefaultableBoolean.True;
                        this.tagrdExpenditureList.DisplayLayout.Override.AllowAddNew = AllowAddNew.TemplateOnBottom;
                    }


                    
                    foreach (UltraGridColumn gcol in tagrdApproverList.DisplayLayout.Bands[0].Columns)
                    {
                        switch (gcol.Key.ToLower())
                        {                          
                            //case "salelimit": 
                            //    gcol.CellActivation = Activation.NoEdit;
                            //    break;
                            //case "profitmarginlimit":
                            //    gcol.CellActivation = Activation.NoEdit;
                            //    break;
                            default:

                                gcol.CellActivation = activation;
                                break;
                        }
                    }

                    if (activation == Activation.ActivateOnly)
                    {
                        this.tagrdApproverList.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
                        this.tagrdApproverList.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                    }
                    else
                    {
                        this.tagrdApproverList.DisplayLayout.Override.AllowDelete = DefaultableBoolean.True;
                        this.tagrdApproverList.DisplayLayout.Override.AllowAddNew = AllowAddNew.TemplateOnBottom;
                    }


                    this.tsbSave.Enabled = true;
                    if (this.objSalesRepFactory.IsNew)
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
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            
        }//Completed
        private void ListSelectionSync()
        {
            ListSyncInprogress = true;

            try
            {
                if (objSalesRepFactory.ObjMSTSalesRep.EmKey > 0)
                {
                    //When The Header.RecordKey is the same as the List.RecordKey, we do not need to select the related row on the list
                    //this will prevent the list from opening the record again
                    if (tagrdSalesRep.Selected != null)
                        if (tagrdSalesRep.Selected.Cells.Count > 0)
                            if (GFunc.NEInt(tagrdSalesRep.Selected.Cells[0].Row.Cells["EmKey"].Value, 0) == objSalesRepFactory.ObjMSTSalesRep.EmKey)
                                return;//already selected.

                    //Select the row on the list to reflect the current value in Header.recordkey
                    UltraGridRow ToSelectRow = this.tagrdSalesRep.Rows.OfType<UltraGridRow>().ToList().Find(
                    row => row.Cells["EmKey"].Text.Equals(objSalesRepFactory.ObjMSTSalesRep.EmKey.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    if (ToSelectRow != null)
                    {
                        ToSelectRow.Cells["EmID"].Selected = true;
                        ToSelectRow.Cells["EmID"].Activate();
                    }
                }
                else
                {
                    tagrdSalesRep.Selected.Cells.Clear();
                    tagrdSalesRep.ActiveRow = null;
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
                        this.objSalesRepFactory.IsDirty = false;
                        IsGridsDirty(true);
                    }
                }

                this.errorProvider1.Clear();

                if (this.objSalesRepFactory.IsDirty)
                {
                    if (Save_Process() == false)
                        return false;
                }

                if (this.objSalesRepFactory.New() == false)
                {                   
                    return false;
                }
                else
                {                   
                    this.errorProvider1.Clear();
                    
                    this.EmID.Focus();
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

                if (objSalesRepFactory.IsDirty)
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
                if (this.objSalesRepFactory.Save())
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
                if (SECPermUtility.Edit(objSalesRepFactory.PermID, false))
                {
                    if (objSalesRepFactory.GetEdit(key) == false)
                    {
                        if (SysOptionUtility.GetBool("WarnOpenRecordAsReadOnly"))
                        {
                            btnSelect = MsgBox.Show("Try to open record as read only",
                                                  GEnum.MsgBoxIcon.Question,
                                                  GEnum.MsgBoxButton.Yes,
                                                  GEnum.MsgBoxButton.No);

                            if (btnSelect == GEnum.MsgBoxButton.Yes)
                            {
                                objSalesRepFactory.GetReadOnly(key);
                            }
                        }
                    }
                }
                else
                    objSalesRepFactory.GetReadOnly(key);

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
                Refresh_GridDet();
                FormLayout();
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

                if (this.objSalesRepFactory.Delete())
                {
                    IsGridsDirty(true);
                    this.objSalesRepFactory.New();                  
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
                if (GFunc.IsNEZ(this.objSalesRepFactory.ObjMSTSalesRep.EmKey))
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

                    if (this.objSalesRepFactory.New())
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
                this.tagrdExpenditureList.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdExpenditureList.UpdateData();
                this.tagrdApproverList.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdApproverList.UpdateData();

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
            if (tagrdExpenditureList.ActiveRow != null)
            {
                if (tagrdExpenditureList.ActiveRow.DataChanged && !tagrdExpenditureList.ActiveRow.IsUnmodifiedTemplateAddRow)
                {
                    if (undoChangesInGrid)
                    {
                        //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                        this.tagrdExpenditureList.PerformAction(UltraGridAction.UndoCell);
                        this.tagrdExpenditureList.PerformAction(UltraGridAction.UndoRow);
                    }
                    return true;
                }
            }
            if (tagrdApproverList.ActiveRow != null)
            {
                if (tagrdApproverList.ActiveRow.DataChanged && !tagrdApproverList.ActiveRow.IsUnmodifiedTemplateAddRow)
                {
                    if (undoChangesInGrid)
                    {
                        //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                        this.tagrdApproverList.PerformAction(UltraGridAction.UndoCell);
                        this.tagrdApproverList.PerformAction(UltraGridAction.UndoRow);
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

        //Control Events
        private void UserKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            string email = this.UserKey.SelectedRow.Cells["UserEmail"].Value.ToString();
            objSalesRepFactory.ObjMSTSalesRep.EmEmail = email;
            objSalesRepFactory.ObjMSTSalesRep.Custom1 = email;

            this.UserKey.SetValueTrigger(GFunc.NEInt(this.UserKey.Value, 0), false);
        }//Completed

        private bool JobLabourItmSelected(int key, string id, string des)
        {
            try
            {
                objSalesRepFactory.ObjMSTSalesRep.JobLabourItmKey = key;
                JobLabourItmID.SetValueTrigger(id, false);
                JobLabourItmDes.SetValueTrigger(des, false);
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
        private bool JobLabourItmProcess(Control ctrl, bool FromButtonClick)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                int vendorKey = 0;
                int PopupType = 0;
                string controlText = ctrl.Text;

                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ctrl.Name);

                switch (ctrl.Name.ToLower())
                {
                    case "joblabouritmid":
                        PopupType = (int)GEnum.PopupType.ItmID;
                        break;

                    case "joblabouritmdes":
                        PopupType = (int)GEnum.PopupType.ItmDes;
                        break;
                }

                if (FromButtonClick)
                {
                    if (DocHDRUtil.EditorButton_Popup((int)objSalesRepFactory.ConstantCodeKey, controlText, listSettingID, PopupType, ref key, ref id, ref des))
                        JobLabourItmSelected(key, id, des);
                }
                else
                {
                    if (GFunc.IsNE(controlText))
                        //Clear all dependent controls
                        JobLabourItmSelected(key, id, des);
                    else
                    {
                        //Try to match record in server
                        key = GFunc.ItmRecord_GetKey((GEnum.RecAccessType)PopupType, listSettingID, controlText, vendorKey, ref id, ref des, false);
                        if (GFunc.IsNEZ(key))
                        {
                            //since value input by user cannot be match let the user select from Popup form
                            if (DocHDRUtil.EditorButton_Popup((int)objSalesRepFactory.ConstantCodeKey, controlText, listSettingID, PopupType, ref key, ref id, ref des))
                                JobLabourItmSelected(key, id, des);
                            else
                            {
                                //when user is still unable to select a matching record, undo the changes
                                MsgBox.Show("Please use a valid value");
                                return false;
                            }
                        }
                        else
                            JobLabourItmSelected(key, id, des);
                    }
                }
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
        private void JobLabourItmID_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            try
            {
                JobLabourItmProcess(sender as Control, true);
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
        private void JobLabourItmID_CustomUpdate(object sender, CancelEventArgs e)
        {
            e.Cancel = !JobLabourItmProcess(sender as Control, false);
        }//Completed
        private void JobLabourItmDes_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            JobLabourItmProcess(sender as Control, true);
        }//Completed
        private void JobLabourItmDes_CustomUpdate(object sender, CancelEventArgs e)
        {
            e.Cancel = !JobLabourItmProcess(sender as Control, false);
        }//Completed

        //Grid Events
        private void tagrdSalesRep_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            int key = 0;
            try
            {
                if (ListSyncInprogress == false)//disable selection when list synchonisation is in progress
                {
                    if (this.tagrdSalesRep.ActiveCell != null && tagrdSalesRep.Selected.Cells.Count > 0)//this line is required to check if user have selected a row in the list
                        key = GFunc.NEInt(tagrdSalesRep.Selected.Cells[0].Row.Cells["EMKey"].Value, 0);

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
        private void tagrdSalesRep_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EmID.Focus();
            }
        }//Completed
        private void tabExpenditureList_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Down:
                    tagrdExpenditureList.Focus();

                    UltraGridColumn FirstVisCol = tagrdExpenditureList.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                    if (FirstVisCol != null)
                    {
                        tagrdExpenditureList.ActiveCell = tagrdExpenditureList.Rows.TemplateAddRow.Cells[FirstVisCol.Key];
                        tagrdExpenditureList.PerformAction(UltraGridAction.EnterEditMode, false, false);
                    }

                    break;
                case Keys.Up:
                    Custom5.Focus();
                    break;
            }
        }//Completed
        private void tagrdApproverList_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Down:
                    tagrdApproverList.Focus();

                    UltraGridColumn FirstVisCol = tagrdApproverList.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                    if (FirstVisCol != null)
                    {
                        tagrdApproverList.ActiveCell = tagrdApproverList.Rows.TemplateAddRow.Cells[FirstVisCol.Key];
                        tagrdApproverList.PerformAction(UltraGridAction.EnterEditMode, false, false);
                    }

                    break;
                case Keys.Up:
                    Custom5.Focus();
                    break;
            }
        }//Completed
        private void tagrdExpenditureList_CustomDataError(object sender, TAUtil.TAErrorEventArgs e)
        {
            try
            {
                //To handle grid combo NotInList event and whether to allow add new record when NotInList
                UltraGridCell curCell = tagrdExpenditureList.ActiveCell;

                if (e.ErrorCode == TAUtil.TAErrorCode.NOT_IN_LIST)
                {
                    if (curCell.Column.EditorComponent != null)
                    {
                        switch (curCell.Column.Key)
                        {
                            case "transtype":
                                if (curCell.Text.Trim() != string.Empty)
                                    GlobalUI.ItemNotInList(tagrdExpenditureList.ActiveCell, null, 0);
                                break;

                            case "transdeptkey":
                            case "transgrpkey":
                                if (curCell.Text.Trim() != string.Empty)
                                    GlobalUI.ItemNotInList(tagrdExpenditureList.ActiveCell, null, 1);
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
        private void tagrdApproverList_CustomDataError(object sender, TAUtil.TAErrorEventArgs e)
        {
            try
            {
                //To handle grid combo NotInList event and whether to allow add new record when NotInList
                UltraGridCell curCell = tagrdApproverList.ActiveCell;

                if (e.ErrorCode == TAUtil.TAErrorCode.NOT_IN_LIST)
                {
                    if (curCell.Column.EditorComponent != null)
                    {
                        switch (curCell.Column.Key)
                        {
                            case "Approver":
                                if (curCell.Text.Trim() != string.Empty)
                                    GlobalUI.ItemNotInList(tagrdApproverList.ActiveCell, null, 0);
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
        private void tagrdExpenditureList_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {

                UltraGridCell currentCell = tagrdExpenditureList.ActiveCell;
                switch (currentCell.Column.Key.ToLower())
                {
                    case "transamt":
                        currentCell.Value = GFunc.NEDec(currentCell.Value, 0);
                        break;
                    case "transdeptkey":
                    case "transgrpkey":
                        currentCell.Value = GFunc.NEInt(currentCell.Value, 0);
                        break;
                }
                if (objSalesRepFactory.Validation_Detail(e.Cell.Row, e.Cell.Column.Key, "tagrdExpenditureList") == false)
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
        private void tagrdApproverList_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {

                UltraGridCell currentCell = tagrdApproverList.ActiveCell;
                SqlConnection cn = new SqlConnection(Database.BossDemoConnection);
              
                if (objSalesRepFactory.Validation_Detail(e.Cell.Row, e.Cell.Column.Key, "tagrdApproverList") == false)
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
        private void tagrdExpenditureList_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            //Check Validation on the cell value and When there's an error set the grid row
            try
            {                              

                if (!GFunc.IsNE(objSalesRepFactory) && tagrdExpenditureList.ActiveRow != null)
                {
                    if (objSalesRepFactory.Validation_Detail(tagrdExpenditureList.ActiveRow, string.Empty, "tagrdExpenditureList") == false)
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
        private void tagrdApproverList_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            //Check Validation on the cell value and When there's an error set the grid row
            try
            {

                if (!GFunc.IsNE(objSalesRepFactory) && tagrdApproverList.ActiveRow != null)
                {
                    if (objSalesRepFactory.Validation_Detail(tagrdApproverList.ActiveRow, string.Empty, "tagrdApproverList") == false)
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
        private void tagrdExpenditureList_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                e.DisplayPromptMsg = false;
                if (tagrdExpenditureList.ActiveRow.IsAddRow == false)
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
                    if (tagrdExpenditureList.ActiveRow.Index > 0)
                        PreRowIndex = tagrdExpenditureList.ActiveRow.Index - 1;
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
        private void tagrdApproverList_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            
            try
            {
                e.DisplayPromptMsg = false;
                if (tagrdApproverList.ActiveRow.IsAddRow == false)
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
                    if (tagrdApproverList.ActiveRow.Index > 0)
                        PreRowIndex = tagrdApproverList.ActiveRow.Index - 1;
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
        private void tagrdExpenditureList_AfterRowUpdate(object sender, RowEventArgs e)
        {              
            this.objSalesRepFactory.ObjMSTSalesRepPayrolls.AcceptChanges();
            this.objSalesRepFactory.IsDirty = true;
        }//Completed
        private void tagrdApproverList_AfterRowUpdate(object sender, RowEventArgs e)
        {
            this.objSalesRepFactory.ObjMSTSalesRepApprovers.AcceptChanges();
            this.objSalesRepFactory.IsDirty = true;
        }//Completed
        private void tagrdExpenditureList_AfterRowsDeleted(object sender, EventArgs e)
        {
            try
            {
                this.objSalesRepFactory.ObjMSTSalesRepPayrolls.AcceptChanges();
                this.objSalesRepFactory.IsDirty = true;
                if (tagrdExpenditureList.Rows.Count > 0)
                {
                    tagrdExpenditureList.Rows[PreRowIndex].Selected = true;
                    tagrdExpenditureList.Rows[PreRowIndex].Activate();
                    PreRowIndex = 0;
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
        private void tagrdApproverList_AfterRowsDeleted(object sender, EventArgs e)
        {
            try
            {
                this.objSalesRepFactory.ObjMSTSalesRepApprovers.AcceptChanges();
                this.objSalesRepFactory.IsDirty = true;
                if (tagrdApproverList.Rows.Count > 0)
                {
                    tagrdApproverList.Rows[PreRowIndex].Selected = true;
                    tagrdApproverList.Rows[PreRowIndex].Activate();
                    PreRowIndex = 0;
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
        private void tagrdExpenditureList_Error(object sender, ErrorEventArgs e)
        {
            if (e.ErrorType == ErrorType.Data)
                formClose = false;
        }//Completed
        private void tagrdApproverList_Error(object sender, ErrorEventArgs e)
        {
            if (e.ErrorType == ErrorType.Data)
                formClose = false;
        }//Completed
        private void tagrdExpenditureList_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                if (GFunc.CompareString(e.Cell.Column.Key, "TransGrpKey"))
                {
                    frmPopupTreeView _frmPopupTreeView = new frmPopupTreeView();
                    _frmPopupTreeView.ShowDialog();
                    if (_frmPopupTreeView.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        tagrdExpenditureList.ActiveRow.Cells["TransGrpKey"].Value = _frmPopupTreeView.TranGrpKey;
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
                //foreach (object key in e.PropertyMessage.Keys)
                //{
                //    Control co = this.Controls.Find(conNm, true)[0];
                //    co.Focus();
                //    break;
                //}
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
        private void Text_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (sender.GetType() == typeof(TAUtil.TATextBoxEditor))
                    ((TAUtil.TATextBoxEditor)sender).SetValueTrigger(((TAUtil.TATextBoxEditor)sender).Text.ToUpper(),false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
 
        /* added by YST on 2023/02/20 to bind sales limit & purchase limit according to Sales Authority Matrix */
        private void EmClass_CustomUpdate(object sender, CancelEventArgs e)
        {            
            if (EmClass.SelectedRow != null)
            {
                decimal SalesLimit = 0, PurLimit = 0;

                SalesLimit = Convert.ToDecimal(EmClass.SelectedRow.Cells["SalesLimit"].Value.ToString());
                if (objSalesRepFactory.ObjMSTSalesRep.SaleLimit != SalesLimit)
                {
                    txtSaleLimit.Text = String.Format("{0:#,##0.00; (#,##0.00)} ", SalesLimit);
                    objSalesRepFactory.ObjMSTSalesRep.SaleLimit = SalesLimit;
                }

                PurLimit = Convert.ToDecimal(EmClass.SelectedRow.Cells["PurchaseLimit"].Value.ToString());
                if (objSalesRepFactory.ObjMSTSalesRep.PurchaseLimit != PurLimit)
                {
                    txtPurLimit.Text = String.Format("{0:#,##0.00; (#,##0.00)} ", PurLimit);
                    objSalesRepFactory.ObjMSTSalesRep.PurchaseLimit = PurLimit;
                }
            }
        }
        /* end adding by YST */

        #region 
        private void CDefaultStateType_CustomUpdate(object sender, CancelEventArgs e)
        {

        }

        private void CCreditLimit_CustomUpdate(object sender, CancelEventArgs e)
        {

        }

        private void CCurrkey_CustomUpdate(object sender, CancelEventArgs e)
        {

        }

        private void CEMKey_CustomUpdate(object sender, CancelEventArgs e)
        {

        }

        private void btnVAttachmentEdit_Click(object sender, EventArgs e)
        {

        }

        private void VAccKey_CustomUpdate(object sender, CancelEventArgs e)
        {

        }

        private void VAccKey_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {

        }

        private void VCurrkey_CustomUpdate(object sender, CancelEventArgs e)
        {

        }

        private void VAccDes_CustomUpdate(object sender, CancelEventArgs e)
        {

        }

        private void VAccDes_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {

        }

        private void VCreditLimit_CustomUpdate(object sender, CancelEventArgs e)
        {

        }

        private void OnDataError(object sender, TAErrorEventArgs e)
        {

        }

        private void tagrdAddr_BeforeSelectChange(object sender, BeforeSelectChangeEventArgs e)
        {

        }

        private void Addr_CustomUpdate(object sender, CancelEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {

        }
        #endregion

    }
}