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
using System.Collections;

namespace WinUI
{
    public partial class frmKeyCustomer : Form
    {
        #region Local Variables
        private BOLib.KeyCustomerFactory objKeyCustomerFactory = null;
        
        private string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        
        private GEnum.formInitMode formOpenMode = GEnum.formInitMode.Neither;
        private int grpKey = 0;
        private int budgetYear = 0;
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
        public frmKeyCustomer()
        {
            InitializeComponent();
        }//Completed
        public frmKeyCustomer(string id)
        {
            //For Call from Shortcut menu (Edit/Add)
            InitializeComponent();
            recordID = id;
            formOpenMode = GEnum.formInitMode.Add;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }//Completed
        public frmKeyCustomer(int _grpKey,int _budgetYear)
        {
            //For call from shortcut menu (Edit)
            InitializeComponent();
            formOpenMode = GEnum.formInitMode.Edit;
            grpKey = _grpKey;
            budgetYear = _budgetYear;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }//Completed
        public frmKeyCustomer(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            //Open from AuditLog as Read only data viewer
            InitializeComponent();
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
        }//Completed

        //Form Events
        private void frmKeyCustomer_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {

                //Call Initialization
                this.objKeyCustomerFactory = new BOLib.KeyCustomerFactory(BOLib.GEnum.InstanceMode.Normal);
                if (objKeyCustomerFactory.GUID <= 0)
                {
                    formClose = true;
                    return;
                }

                //Attach Event on Factory
                this.objKeyCustomerFactory.ErrorNotifierHeader_Set += new GVar.UINotifierEvent(this.ErrorNotifier_Set);
                this.objKeyCustomerFactory.ErrorNotifierHeader_Clear += new GVar.UINotifierEvent(this.ErrorNotifier_Clear);

                //Check if user has permission to edit Record ID
                canEditRecordID = SECPermUtility.Perform("sysRecID", false);

                if (this.IsOpenFromAuditLog)
                {
                    if (objKeyCustomerFactory.SetReadOnlyData(_dtHeader, _dsDetail) == false)
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
                        this.OpenRecord(grpKey, budgetYear);
                    else if (formOpenMode == GEnum.formInitMode.Add)
                    {
                        if (canEditRecordID && recordID != string.Empty)
                            this.DocConKey.SetValueTrigger(recordID, false);

                    }

                    objKeyCustomerFactory.ObjKeyCustomer.CustOrder = objKeyCustomerFactory.GetMaxCustomerOrder();
                }

                //Setup Grid Layout,Fill Data, ShortcutMenu, fill Combo
                GlobalUI.FormGrids_Set(this, (int)objKeyCustomerFactory.ConstantCodeKey, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objKeyCustomerFactory.ConstantCodeKey);
                // //GlobalUI.cmnuGlobal_Set(this);
                GlobalUI.Combos_Fill(this, (int)objKeyCustomerFactory.ConstantCodeKey);
                //BudgetYearType.SelectedText = DateTime.Now.Year.ToString();
                BudgetYearType.Value = DateTime.Now.Year.ToString();

                if (this.objKeyCustomerFactory.IsNew == true)
                 DefaultSetting();                


                // to bind grid of Keycustomerlist based on selectedbudgetyear
                Refresh_KeyCustomerList();

                //Setup List properties
                this.tagrdKeyCustomerList.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                this.tagrdKeyCustomerList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
                this.tagrdKeyCustomerList.DisplayLayout.Bands[0].Override.ActiveCellAppearance.BackColor = Color.Gold;
              
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
        }

        private void DefaultSetting()
        {
            objKeyCustomerFactory.ObjKeyCustomer.BudgetYear = DateTime.Now.Year;
            cmdDetailBudgetYear.Value = DateTime.Now.Year.ToString();

            // since setting default values, need to set IsDirty to false again
            this.objKeyCustomerFactory.IsDirty = false;
        }
        private bool OpenRecord(int grpKey,int budgetYear)
        {
            this.Cursor = Cursors.WaitCursor;
            GEnum.MsgBoxButton btnSelect = GEnum.MsgBoxButton.Yes;
            
            try
            {
                if (SECPermUtility.Edit(objKeyCustomerFactory.PermID, false))
                {
                    if (objKeyCustomerFactory.GetEdit(grpKey,budgetYear) == false)
                    {
                        if (SysOptionUtility.GetBool("WarnOpenRecordAsReadOnly"))
                        {
                            btnSelect = MsgBox.Show("Try to open record as read only",
                                                  GEnum.MsgBoxIcon.Question,
                                                  GEnum.MsgBoxButton.Yes,
                                                  GEnum.MsgBoxButton.No);
                            if (btnSelect == GEnum.MsgBoxButton.Yes)
                            {
                                objKeyCustomerFactory.GetReadOnly(grpKey, budgetYear);
                            }
                        }
                    }
                }
                else
                    objKeyCustomerFactory.GetReadOnly(grpKey, budgetYear);

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
        private void frmKeyCustomer_Shown(object sender, EventArgs e)
        {
            //Close Form when FormLoad has encounter errors
            if (formClose)
                this.Close();
        }//Completed
        private void frmKeyCustomer_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool runProcess = false;
            this.formClose = true;

            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }
            if (this.formClose && objKeyCustomerFactory == null)
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
                        //IsGridsDirty(true);
                        e.Cancel = false;
                    }
                }
                #endregion

                //Dispose Factory
                if ((bool)this.objKeyCustomerFactory.Dispose() == false)
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
                    this.objKeyCustomerFactory.Dispose();
            }
        }//Completed
        private void frmKeyCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                    GlobalUI.Combos_Fill(this, (int)objKeyCustomerFactory.ConstantCodeKey);             
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
                bool existingrecord = false;
                existingrecord = objKeyCustomerFactory.AlreadyExistOrNot(int.Parse(objKeyCustomerFactory.ObjKeyCustomer.ConKey1.ToString()), int.Parse(objKeyCustomerFactory.ObjKeyCustomer.BudgetYear.ToString()),int.Parse(objKeyCustomerFactory.ObjKeyCustomer.GrpKey.ToString()));

                if(existingrecord==true)
                {
                    MessageBox.Show("This Current ConKey and Current Budget Year have already been existed in Boss System!");
                    return;
                }

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
                DefaultSetting();
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
                DefaultSetting();
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
                //Refresh_GridDet();
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
                this.bdsKeyCustomers.DataSource = objKeyCustomerFactory.ObjKeyCustomer;
                this.bdsKeyCustomers.AllowNew = true;
                this.bdsKeyCustomers.ResetBindings(false);
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
                    string listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "0GF", tagrdKeyCustomerList.Name);
                    GlobalUI.Grid_Format(tagrdKeyCustomerList, listID, true, false);

                }
                //Please make sure in design time no columns is sorted or clear sorted columns in formload, if anycolumn is already defined as sorted column , this code will skip
                if (this.tagrdKeyCustomerList.DisplayLayout.Bands[0].SortedColumns.Count <= 0)
                    this.tagrdKeyCustomerList.DisplayLayout.Bands[0].SortedColumns.Add(tagrdKeyCustomerList.DisplayLayout.Bands[0].Columns["GrpKey"], false);

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
            bool EnableMode = !this.objKeyCustomerFactory.IsReadOnly;
            this.tslReadOnly.Text = !EnableMode ? "Read Only" : string.Empty;

            //Special Condition for RecordID

            this.DocConKey.Enabled = EnableMode;
            this.DocConKey.ReadOnly = !canEditRecordID;
            this.DocConNm.Enabled = EnableMode;
            this.cmdDetailBudgetYear.Enabled = EnableMode;
            //this.txtSaleRep.Enabled = EnableMode;
            //this.Custom1.Enabled = EnableMode;
            //this.Custom2.Enabled = EnableMode;
            //this.Custom3.Enabled = EnableMode;

            if (EnableMode == false)
            {
                this.tsbSave.Enabled = false;
                this.tsbDelete.Enabled = false;
                this.tsbClear.Enabled = false;
            }
            else
            {
                this.tsbSave.Enabled = true;
                if (this.objKeyCustomerFactory.IsNew)
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
                if (objKeyCustomerFactory.ObjKeyCustomer.GrpKey > 0)
                {
                    //When The Header.RecordKey is the same as the List.RecordKey, we do not need to select the related row on the list
                    //this will prevent the list from opening the record again
                    if (tagrdKeyCustomerList.Selected != null)
                        if (tagrdKeyCustomerList.Selected.Cells.Count > 0)
                            if (
                                GFunc.NEInt(tagrdKeyCustomerList.Selected.Cells[0].Row.Cells["GrpKey"].Value, 0) == objKeyCustomerFactory.ObjKeyCustomer.GrpKey &&
                                GFunc.NEInt(tagrdKeyCustomerList.Selected.Cells[0].Row.Cells["BudgetYear"].Value, 0) == objKeyCustomerFactory.ObjKeyCustomer.BudgetYear
                                )
                                return;//already selected.

                    //Select the row on the list to reflect the current value in Header.recordkey
                    Refresh_KeyCustomerList();

                    UltraGridRow ToSelectRow = this.tagrdKeyCustomerList.Rows.OfType<UltraGridRow>().ToList().Find(
                    row => row.Cells["GrpKey"].Text.Equals(objKeyCustomerFactory.ObjKeyCustomer.GrpKey.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    row.Cells["BudgetYear"].Text.Equals(objKeyCustomerFactory.ObjKeyCustomer.BudgetYear.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    if (ToSelectRow != null)
                    {                        
                        ToSelectRow.Cells["GrpKey"].Selected = true;
                        ToSelectRow.Cells["GrpKey"].Activate();
                    }
                }
                else
                {
                    Refresh_KeyCustomerList();
                    tagrdKeyCustomerList.Selected.Cells.Clear();
                    tagrdKeyCustomerList.ActiveRow = null;
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

        private bool form_CanValidate()
        {
            try
            {
                //this function check if we can validates all the header.controls
                //all grid is able to commit its changes to its datasource
                TAUtil.ControlGVar.FormValidateFail = false;
                this.errorProvider1.Clear();
                this.Validate();
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
                        this.objKeyCustomerFactory.IsDirty = false;
                        
                    }
                }

                this.errorProvider1.Clear();

                if (this.objKeyCustomerFactory.IsDirty)
                {
                    if (Save_Process() == false)
                        return false;
                }

                if (this.objKeyCustomerFactory.New() == false)
                {
                    return false;
                }
                else
                {
                    this.errorProvider1.Clear();
                    this.DocConKey.Focus();                    
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

                if (objKeyCustomerFactory.IsDirty)
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
                if (this.objKeyCustomerFactory.Save())
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

                if (this.objKeyCustomerFactory.Delete())
                {
                    
                    this.objKeyCustomerFactory.New();

                    //Move the cursor position of active row index to upper row
                    if (tagrdKeyCustomerList.ActiveRow.Index > 0)
                        PreRowIndex = tagrdKeyCustomerList.ActiveRow.Index - 1;

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
                if (tagrdKeyCustomerList.Rows.Count > 0)
                {
                    tagrdKeyCustomerList.Rows[PreRowIndex].Selected = true;
                    tagrdKeyCustomerList.Rows[PreRowIndex].Activate();
                }
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private bool Clear_Process()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (GFunc.IsNEZ(this.objKeyCustomerFactory.ObjKeyCustomer.GrpKey))
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

                    //IsGridsDirty(true);

                    if (this.objKeyCustomerFactory.New())
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
        

        //Grid Events
        private void tagrdKeyCustomerList_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            int GrpKey = 0;
            int BudgetYear = 0;
            try
            {
                if (ListSyncInprogress == false)//disable selection when list synchonisation is in progress
                {
                    if (this.tagrdKeyCustomerList.ActiveRow != null && tagrdKeyCustomerList.Selected.Cells.Count > 0)//this line is required to check if user have selected a row in the list
                    {
                        GrpKey = GFunc.NEInt(tagrdKeyCustomerList.Selected.Cells[0].Row.Cells["GrpKey"].Value, 0);
                        BudgetYear = GFunc.NEInt(tagrdKeyCustomerList.Selected.Cells[0].Row.Cells["BudgetYear"].Value, 0);
                    }

                    if (this.SaveChanges() == false)
                    {
                        ListSelectionSync();
                        return;
                    }

                    if (GrpKey > 0 && BudgetYear>0)
                        this.OpenRecord(GrpKey,BudgetYear);
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

        private void BudgetYearType_CustomUpdate(object sender, CancelEventArgs e)
        {
            Refresh_KeyCustomerList();
        }
        private void Refresh_KeyCustomerList()
        {          
            
        if (!GFunc.IsNEZ(BudgetYearType.Value))
        {
                objKeyCustomerFactory.GetRepsByGroup(Convert.ToInt32(BudgetYearType.Value));
        }
        else
        return;

        
            tagrdKeyCustomerList.DataSource = objKeyCustomerFactory.KeyCustomerGrpByBudYear;
            //FilterDataByPermission();
            tagrdKeyCustomerList.DataBind();
            //FormatAvailableGrid();
        }
        
       private void DocConKey_CustomUpdate(object sender, CancelEventArgs e)
        {
           //DocConNm.Text = DocConKey.SelectedRow.Cells[2].Text;
           objKeyCustomerFactory.ObjKeyCustomer.CUSTOMER1 = DocConKey.SelectedRow.Cells[2].Text;
           if(this.objKeyCustomerFactory.IsNew==true)
            objKeyCustomerFactory.ObjKeyCustomer.GrpKey = objKeyCustomerFactory.GetGrpKeyByConKey(int.Parse(DocConKey.Value.ToString()));
        }//Completed

        private void cmbSalesRep_CustomUpdate(object sender, CancelEventArgs e)
        {
            
            Refresh_Team();
        }
        private void Refresh_Team()
        {
            string team = "";
            //int budgetyear = 0;

            //if (cmdDetailBudgetYear.SelectedText != "") budgetyear = int.Parse(cmdDetailBudgetYear.SelectedText);
            //else if (cmdDetailBudgetYear.Text != "") budgetyear = int.Parse(cmdDetailBudgetYear.Text);

            if (cmbSalesRep.Value != "" && cmdDetailBudgetYear.Value != "")
            {

                team = objKeyCustomerFactory.GetTeamByEmId_And_BudgetYear(cmbSalesRep.Value.ToString(), int.Parse(cmdDetailBudgetYear.Value.ToString()));
            }
            else
                return;
            
            objKeyCustomerFactory.ObjKeyCustomer.Team = team;

        }

        private void DocConKey1_CustomUpdate(object sender, CancelEventArgs e)
        {
            objKeyCustomerFactory.ObjKeyCustomer.CUSTOMER2 = DocConKey1.SelectedRow.Cells[2].Text;
        }

        private void DocConKey2_CustomUpdate(object sender, CancelEventArgs e)
        {
            objKeyCustomerFactory.ObjKeyCustomer.CUSTOMER3 = DocConKey2.SelectedRow.Cells[2].Text;
            
        }

        private void DocConKey3_CustomUpdate(object sender, CancelEventArgs e)
        {
            objKeyCustomerFactory.ObjKeyCustomer.CUSTOMER4 = DocConKey3.SelectedRow.Cells[2].Text;
        }

        private void DocConKey4_CustomUpdate(object sender, CancelEventArgs e)
        {
            objKeyCustomerFactory.ObjKeyCustomer.CUSTOMER5 = DocConKey4.SelectedRow.Cells[2].Text;
        }

        private void DocConKey5_CustomUpdate(object sender, CancelEventArgs e)
        {
            objKeyCustomerFactory.ObjKeyCustomer.CUSTOMER6 = DocConKey5.SelectedRow.Cells[2].Text;
        }

        private void DocConKey6_CustomUpdate(object sender, CancelEventArgs e)
        {
            objKeyCustomerFactory.ObjKeyCustomer.CUSTOMER7 = DocConKey6.SelectedRow.Cells[2].Text;
        }

        private void DocConKey7_CustomUpdate(object sender, CancelEventArgs e)
        {
            objKeyCustomerFactory.ObjKeyCustomer.CUSTOMER8 = DocConKey7.SelectedRow.Cells[2].Text;
        }

        private void DocConKey8_CustomUpdate(object sender, CancelEventArgs e)
        {
            objKeyCustomerFactory.ObjKeyCustomer.CUSTOMER9 = DocConKey8.SelectedRow.Cells[2].Text;
        }

        private void DocConKey9_CustomUpdate(object sender, CancelEventArgs e)
        {
            objKeyCustomerFactory.ObjKeyCustomer.CUSTOMER10 = DocConKey9.SelectedRow.Cells[2].Text;
        }

        private void cmbSalesRep_AfterDropDown(object sender, EventArgs e)
        {
            cmbSalesRep.DataSource = objKeyCustomerFactory.GetListOfSaleRapByBudgetYear(int.Parse(cmdDetailBudgetYear.Value.ToString()));
            cmbSalesRep.DataBind();
        }

        private void tagrdKeyCustomerList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                DocConKey.Focus();
            }
        }
    }
}
