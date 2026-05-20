using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using BOLib;
using TAUtil;

namespace WinUI
{
    public partial class frmMSTBudget : Form
    {
        #region Local Variables

        private BOLib.MSTBudgetFactory objMstBudgetFactory = null;
        string ContextMenuSetting = string.Empty;
        private string msgID = string.Empty;
        private bool formClose = false;  

        #endregion

        public frmMSTBudget()
        {
            InitializeComponent();
        }

        //Form Events
        private void frmMSTBudget_Load(object sender, EventArgs e)
        {
            //Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            // check 
            try
            {
                // Initialize
                this.objMstBudgetFactory = new BOLib.MSTBudgetFactory(BOLib.GEnum.InstanceMode.Normal);
                if (objMstBudgetFactory.GUID <= 0) formClose = true;                
                
                bdsMSTBudget.DataSource = objMstBudgetFactory.ObjMSTBudget;
                Refresh_CurrentYear();
                Refresh_PreviousYear();

                this.GetPeriodMonth();

                //ui apperance
                SetDocItemSubVisibility();

                this.PeriodFrom.SetValueTrigger(DateTime.Today.Year * 100 + DateTime.Today.Month, false);
                this.PeriodTo.SetValueTrigger((DateTime.Today.Year + 1) * 100 + DateTime.Today.Month - 1, false);
               
                this.tsbSave.Enabled = false;               
                this.tsbCopy.Enabled = false;

                GlobalUI.FormGrids_Set(this, (int)objMstBudgetFactory.ConstantCodeKey, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objMstBudgetFactory.ConstantCodeKey);

                GlobalUI.Combos_Fill(this, (int)objMstBudgetFactory.ConstantCodeKey);

                this.LockGrids();
                this.SetGridColumns();
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
                    Error(tex, true); // Custom Msg
                    this.formClose = true;
                }
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
        private void frmMSTBudget_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool runProcess = false;
            this.formClose = true;

            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }
            if (this.formClose && objMstBudgetFactory == null)
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
                    if (this.SaveChanges("") == false)
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

                if ((bool)this.objMstBudgetFactory.Dispose() == false)
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
                    this.objMstBudgetFactory.Dispose();
            }
        }
        private void frmMSTBudget_Shown(object sender, EventArgs e)
        {
            // Check Form Close State is True ...
            if (formClose)
            {
                this.Close();
            }
        }
        private void frmMSTBudget_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)objMstBudgetFactory.ConstantCodeKey);
                    //CombosDependent_Fill(string.Empty);
                }
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
        }

        //Menu Strip Events
        private void tsbSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (tagrdCurrentYear.Rows.Count == 0)
                    MsgBox.Show(MsgID.Record.NotEffected);
                else
                {
                    this.Save_Process("");                        
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

        }
        private void tsbCopy_Click(object sender, EventArgs e)
        {
            try
            {
                #region Set Value to factory (for Copy) 

                if (tagrdCurrentYear.Rows.Count > 0)
                {
                    SaveChanges("");
                    objMstBudgetFactory.IsDirty = false;
                }

                if (this.BudgetRecSubKey.Visible)
                {
                    objMstBudgetFactory.SelectBudgetRecKey = GFunc.NEInt(this.RecKey.Value, 0);
                    objMstBudgetFactory.SelectBudgetRecSubKey = GFunc.NEInt(this.BudgetRecSubKey.Value, 0);
                }
                else
                {
                    objMstBudgetFactory.SelectBudgetRecKey = GFunc.NEInt(this.RecKey.Value, 0);
                    objMstBudgetFactory.SelectBudgetRecSubKey = 0;
                }
                objMstBudgetFactory.BudgetType= GFunc.NEInt(this.BudgetType.Value, 0);
                objMstBudgetFactory.FromBranchID =this.BudgetBranchKey.Text;
                objMstBudgetFactory.ToBranchID = this.BudgetBranchKey.Text;
                objMstBudgetFactory.FromDeptID =this.BudgetDeptKey.Text;
                objMstBudgetFactory.ToDeptID =this.BudgetDeptKey.Text;
                objMstBudgetFactory.AmountRatio = GFunc.NEDec(this.AmountRatio.Value, 0);
                objMstBudgetFactory.AddAmount= GFunc.NEDec(this.AddAmount.Value, 0);
                objMstBudgetFactory.UnitRatio = GFunc.NEDec(this.UnitRatio.Value, 0);
                objMstBudgetFactory.AddUnit = GFunc.NEDec(this.AddUnit.Value, 0);
                objMstBudgetFactory.FromBudgetPeriod = GFunc.NEInt(this.PeriodFrom.Value,0);
                objMstBudgetFactory.ToBudgetPeriod = GFunc.NEInt(this.PeriodTo.Value, 0);
                #endregion

                frmMSTBudgetCopy fMSTBudgetCopy = new frmMSTBudgetCopy(objMstBudgetFactory, (GEnum.BudgetType)BudgetType.Value);
                fMSTBudgetCopy.ShowDialog();
                btnRequery_Click(sender, e);
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

        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }       

        //Button Common Events
        private void btnRequery_Click(object sender, EventArgs e)
        {
            try
            {
                if (GFunc.NEInt(BudgetType.Value,0) == 0)
                    BudgetType.SetValueTrigger(null, false);
                if (Validation())
                {
                    FillCurrentData(false);
                }
                else
                {
                    ClearBudgetDetails();
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
       
        private void btnAssign_Click(object sender, EventArgs e)
        {
            // Waiting cursor
            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.Validate();
                foreach(UltraGridRow row in tagrdCurrentYear.Selected.Rows)
                {
                    row.Cells["BudgetAmountH"].Value =
                        GFunc.NEDec(tagrdPreviousYear.Rows[row.Index].Cells["BudgetAmountH"].Value, 0) *
                        GFunc.NEDec(this.AmountRatio.Value, 1) + GFunc.NEDec(this.AddAmount.Value, 0);

                    if (!tagrdPreviousYear.DisplayLayout.Bands[0].Columns["BudgetWeight"].Hidden)
                    {
                        row.Cells["BudgetWeight"].Value =
                           GFunc.NEDec(tagrdPreviousYear.Rows[row.Index].Cells["BudgetWeight"].Value, 0) *
                        GFunc.NEDec(this.UnitRatio.Value, 1) + GFunc.NEDec(this.AddUnit.Value, 0);
                    }
                    if (!tagrdPreviousYear.DisplayLayout.Bands[0].Columns["BudgetQty"].Hidden)
                    {
                        row.Cells["BudgetQty"].Value =
                            GFunc.NEDec(tagrdPreviousYear.Rows[row.Index].Cells["BudgetQty"].Value, 0) *
                        GFunc.NEDec(this.UnitRatio.Value, 1) + GFunc.NEDec(this.AddUnit.Value, 0);
                    }
                    row.Update();
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
            finally
            {
                // Default cursor
                this.Cursor = Cursors.Default;
            }
        }
        private void btnAssignAll_Click(object sender, EventArgs e)
        {
            // Waiting cursor
            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.Validate();
                this.objMstBudgetFactory.AssignAllPeriod(GFunc.NEDec(this.AmountRatio.Value, 0), GFunc.NEDec(this.AddAmount.Value, 0), GFunc.NEDec(this.UnitRatio.Value, 0), GFunc.NEDec(this.AddUnit.Value, 0), GFunc.NEInt(this.BudgetItmMode.Value, 0));
                this.Refresh_CurrentYear();
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
                // Default cursor
                this.Cursor = Cursors.Default;
            }
        }
      

        //Control data refresh - Dependant combo, TextEditorPopup, Grid - Combo List, Set/Clear TextEditorPop value and Grid binding source and filter
        private void FillCurrentData(bool CheckLock)
        {
            //int recKey = 0;
            //int subRecKey = 0;
            bool isOk = false;

            try
            {
                this.Validate();

                isOk = this.objMstBudgetFactory.GetEdit(GFunc.NEInt(BudgetType.Value,0), GFunc.NEInt(BudgetBranchKey.Value,0), GFunc.NEInt(BudgetDeptKey.Value,0),
                    GFunc.NEInt(this.RecKey.Value,0),GFunc.NEInt(this.BudgetRecSubKey.Value,0), GFunc.NEInt(PeriodFrom.Value,0), GFunc.NEInt(PeriodTo.Value,0), CheckLock);
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
                            isOk = this.objMstBudgetFactory.GetReadOnly(objMstBudgetFactory.ObjMSTBudget.BudgetType.Value, objMstBudgetFactory.ObjMSTBudget.BudgetBranchKey.Value, objMstBudgetFactory.ObjMSTBudget.BudgetDeptKey.Value,(int) this.RecKey.Value,(int) this.BudgetRecSubKey.Value,
                              (int?)PeriodFrom.Value, (int?)PeriodTo.Value);

                            // Check Process
                            if ((!isOk) && this.msgID != string.Empty)
                                MsgBox.Show(this.msgID); // Custom Msg
                        }
                        // No, i don't want
                        else if (btnSelect == GEnum.MsgBoxButton.No)
                        {
                            // Call Edit
                            isOk = this.objMstBudgetFactory.GetEdit((int)BudgetType.Value, (int)BudgetBranchKey.Value, (int)BudgetDeptKey.Value, (int)this.RecKey.Value, (int)this.BudgetRecSubKey.Value, (int)PeriodFrom.Value, (int)PeriodTo.Value, CheckLock);
                        }
                        // Cancel Process
                        else
                            return;
                    }
                    else
                    {
                        // Call ReadOnly
                        isOk = this.objMstBudgetFactory.GetReadOnly((int)BudgetType.Value, (int)BudgetBranchKey.Value, (int)BudgetDeptKey.Value, (int)this.RecKey.Value, (int)this.BudgetRecSubKey.Value, (int)PeriodFrom.Value, (int)PeriodTo.Value);
                    }
                }
                if (isOk)
                {
                    this.Refresh_PreviousYear();
                    this.Refresh_CurrentYear();
                    bdsMSTBudget.DataSource = objMstBudgetFactory.ObjMSTBudget;
                    bdsMSTBudget.ResetBindings(false);
                    SetBudgetItemMode();
                    SetGridColumns();
                    tsbSave.Enabled = true;                   
                }
                // Call ReadOnly
                this.OnReadOnly();
            }
            catch (TAException tex)
            {
                throw Error(tex, false); // Custom Msg                  
            }
            catch (Exception ex)
            {
                throw Error(ex, false); // System Msg   
            }
            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }
        private void GetPeriodMonth()
        {
            DataTable dtMonth = BOLib.SYSList.GetSystemPeriodMonths((int)GEnum.SystemPeriodStatus.All, 0);

            PeriodFrom.DataSource = dtMonth;
            PeriodFrom.ValueMember = "Period";
            PeriodFrom.DisplayMember = "MonthYear";

            PeriodTo.DataSource = dtMonth;
            PeriodTo.ValueMember = "Period";
            PeriodTo.DisplayMember = "MonthYear";
        }

        private void SetDocItemSubVisibility()
        {
            try
            {
                switch ((int)BudgetType.Value)
                {
                    //For Budget Type 420,440                  
                    case (int)GEnum.BudgetType.Document_Group_and_Item_Sales:
                    case (int)GEnum.BudgetType.Document_Group_Item_Purchases:    
                            BudgetRecSub_lbl.Visible = true;
                            BudgetRecSubKey.Visible = true;
                        break;
                    default: //For Budget Others
                        BudgetRecSub_lbl.Visible = false;
                        BudgetRecSubKey.Visible = false;                       
                        break;
                }
                SetBudgetItemMode();
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

        private void SetBudgetItemMode()
        {
            try
            {
                switch ((int)BudgetType.Value)
                {
                    //For Budget Type 110,120,420,440
                    case (int)GEnum.BudgetType.Item_Sales:
                    case (int)GEnum.BudgetType.Item_Purchases:
                    case (int)GEnum.BudgetType.Document_Group_and_Item_Sales:
                    case (int)GEnum.BudgetType.Document_Group_Item_Purchases:
                        if (GFunc.IsNEZ(BudgetItmMode.Value) || tagrdCurrentYear.Rows.Count==0)
                            BudgetItmMode.SetValueTrigger(10, false);                        
                        BudgetItmMode.Visible = true;
                        BudgetItmMode_lbl.Visible = true;
                        pnlUnitFormula.Visible = true;
                        if (FormulaExpandableGroupBox.Height < pnlUnitFormula.Height * 2)
                            FormulaExpandableGroupBox.Height += pnlUnitFormula.Height;
                        break;
                    default: //For Budget Others
                        BudgetItmMode.Visible = false;
                        BudgetItmMode_lbl.Visible = false;
                        BudgetItmMode.SetValueTrigger(0, false);
                        pnlUnitFormula.Visible = false;
                        if( FormulaExpandableGroupBox.Height>pnlUnitFormula.Height*2)
                            FormulaExpandableGroupBox.Height -= pnlUnitFormula.Height;
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
        private void SetGridColumns()
        {
            try
            {
                string CurrGridPrevListID = GlobalUI.ListSettingID_Get(ContextMenuSetting, tagrdCurrentYear.Name);
                string PrevGridPrevListID = GlobalUI.ListSettingID_Get(ContextMenuSetting, tagrdPreviousYear.Name);
                if ((int)BudgetItmMode.Value == (int)GEnum.BudgetItemMode.Unit)
                {
                    GlobalUI.Grid_Format(tagrdCurrentYear, "frmMSTBudgetGridCurrentItemUnit", false);
                    GlobalUI.Grid_Format(tagrdPreviousYear, "frmMSTBudgetGridPreviousItemUnit", false);

                    if (GFunc.CompareString(CurrGridPrevListID, "frmMSTBudgetGridCurrentItemUnit") == false)
                        ContextMenuSetting=ContextMenuSetting.Replace(CurrGridPrevListID, "frmMSTBudgetGridCurrentItemUnit");
                    if (GFunc.CompareString(PrevGridPrevListID, "frmMSTBudgetGridPreviousItemUnit") == false)
                        ContextMenuSetting = ContextMenuSetting.Replace(PrevGridPrevListID, "frmMSTBudgetGridPreviousItemUnit");                  
                }
                else if ((int)BudgetItmMode.Value == (int)GEnum.BudgetItemMode.Weight)
                {
                    GlobalUI.Grid_Format(tagrdCurrentYear, "frmMSTBudgetGridCurrentItemWeight", false);
                    GlobalUI.Grid_Format(tagrdPreviousYear, "frmMSTBudgetGridPreviousItemWeight", false);

                    if (GFunc.CompareString(CurrGridPrevListID, "frmMSTBudgetGridCurrentItemWeight") == false)
                        ContextMenuSetting = ContextMenuSetting.Replace(CurrGridPrevListID, "frmMSTBudgetGridCurrentItemWeight");
                    if (GFunc.CompareString(PrevGridPrevListID, "frmMSTBudgetGridPreviousItemWeight") == false)
                        ContextMenuSetting = ContextMenuSetting.Replace(PrevGridPrevListID, "frmMSTBudgetGridPreviousItemWeight");                 
                }
                else
                {
                    GlobalUI.Grid_Format(tagrdCurrentYear, "frmMSTBudgetGridCurrent", false);
                    GlobalUI.Grid_Format(tagrdPreviousYear, "frmMSTBudgetGridPrevious", false);

                    if (GFunc.CompareString(CurrGridPrevListID, "frmMSTBudgetGridCurrent") == false)
                        ContextMenuSetting = ContextMenuSetting.Replace(CurrGridPrevListID, "frmMSTBudgetGridCurrent");
                    if (GFunc.CompareString(PrevGridPrevListID, "frmMSTBudgetGridPrevious") == false)
                        ContextMenuSetting = ContextMenuSetting.Replace(PrevGridPrevListID, "frmMSTBudgetGridPrevious");                 
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
        private void Setcombovalue()
        {
            this.RecKey.SetValueTrigger(0, false);
            this.BudgetRecSubKey.SetValueTrigger(0, false);
            this.BudgetBranchKey.SetValueTrigger(0, false);
            this.BudgetDeptKey.SetValueTrigger(0, false);           
            this.PeriodFrom.SetValueTrigger(DateTime.Today.Year * 100 + DateTime.Today.Month,false);
            this.PeriodTo.SetValueTrigger((DateTime.Today.Year + 1) * 100 + DateTime.Today.Month - 1,false);
        }

        //Combo Events
        private void Combo_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (tagrdCurrentYear.Rows.Count > 0)
                {
                    if (((TAUtil.TAComboBox)sender).Value != ((TAUtil.TAComboBox)sender).OldValue)
                        if (SaveChanges("combo") == false)
                            e.Cancel = true;
                        else
                        {
                            ClearBudgetDetails();
                            SetBudgetItemMode();
                            objMstBudgetFactory.IsDirty = false;
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
        private void BudgetType_CustomUpdate(object sender, CancelEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (BudgetType.Value == null)
                {
                    ClearBudgetDetails();
                    return;
                }

                if (BudgetType.Value == BudgetType.OldValue)
                    return;
                if (tagrdCurrentYear.Rows.Count > 0)
                {
                    if (SaveChanges("combo") == false)
                    {
                        e.Cancel = true;
                        return;
                    }
                    objMstBudgetFactory.IsDirty = false;
                }

                //Set UI
                this.Setcombovalue();
                SetDocItemSubVisibility();              
                this.SetGridColumns(); //Set Grid Column
                ClearBudgetDetails();

                if ((BudgetType.Text.Trim() != string.Empty) && (BudgetType.Text.Trim() != "0"))
                {
                    //To Show the Copy Button
                    this.tsbCopy.Enabled = true;
                    //To Show label name according BudgetType
                    if ((int)GEnum.BudgetType.Item_Sales == (int)BudgetType.Value || (int)GEnum.BudgetType.Item_Purchases == (int)BudgetType.Value)
                    { this.RecKey_lbl.Text = "Items"; }                   
                    else if ((int)GEnum.BudgetType.Document_Group_Item_Purchases == (int)BudgetType.Value || (int)GEnum.BudgetType.Document_Group_and_Item_Sales == (int)BudgetType.Value
                        || (int)GEnum.BudgetType.Document_Group_Sales == (int)BudgetType.Value || (int)GEnum.BudgetType.Document_Group_Purchase == (int)BudgetType.Value)
                    { this.RecKey_lbl.Text = "Document Group"; }
                    else
                    { this.RecKey_lbl.Text = BudgetType.Text; }

                    if (!BudgetType.IsItemInList(BudgetType.Text))
                        return;

                    switch ((int)BudgetType.Value)
                    {
                        case (int)GEnum.BudgetType.Account:
                            this.GetAccs();
                            break;

                        case (int)GEnum.BudgetType.Item_Sales:
                            //SetDocItemVisibility(true);
                            this.SetGridColumns(); //Set Grid Column
                            this.GetItems();
                            break;

                        case (int)GEnum.BudgetType.Item_Purchases:
                           // SetDocItemVisibility(true);
                            this.SetGridColumns(); //Set Grid Column
                            this.GetItems();
                            break;

                        case (int)GEnum.BudgetType.Customer_Sales:
                            this.GetCustomers();
                            break;

                        case (int)GEnum.BudgetType.Industry_Sales:
                        case (int)GEnum.BudgetType.Industry_Purchase:
                            this.GetIndustrys();
                            break;

                        case (int)GEnum.BudgetType.Territory_Sales:
                        case (int)GEnum.BudgetType.Territory_Purchase:
                            this.GetTerritorys();
                            break;

                        case (int)GEnum.BudgetType.Vendor_Purchase:
                            this.GetVendors();
                            break;                       
                      
                        case (int)GEnum.BudgetType.Document_Group_Sales:
                        case (int)GEnum.BudgetType.Document_Group_Purchase:
                           // this.tabBudgetType.Tabs[0].Text = "Doc Group";
                            this.GetDocGrps();
                            break;

                        case (int)GEnum.BudgetType.Document_Group_and_Item_Sales:
                        case (int)GEnum.BudgetType.Document_Group_Item_Purchases:
                            //SetDocItemVisibility(true);
                            this.SetGridColumns(); //Set Grid Column
                          //  this.tabBudgetType.Tabs[0].Text = "Items";
                            this.GetDocGrps();
                            this.GetItemsRecSub();
                            break;                                       

                        default:
                            break;
                    }
                }
                else
                {
                    //To Enable the Copy Button
                    this.tsbCopy.Enabled = false;
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
        private void BudgetItmMode_CustomUpdate(object sender, CancelEventArgs e)
        {            
            this.SetGridColumns();
        }
        private void PeriodFrom_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if(PeriodFrom.Rows.Count>0 && PeriodTo.Rows.Count>0 && !GFunc.IsNEZ(PeriodFrom.Value))
                {
                    int periodFrom =(int)PeriodFrom.Value;
                    int periodTo=(periodFrom/100 + 1) * 100;
                    if ((periodFrom % 100 - 1) == 0)
                        periodTo = (periodTo / 100 - 1) * 100 + 12;
                    else
                        periodTo += (periodFrom % 100 - 1);

                    if (PeriodTo.IsItemInList(periodTo.ToString()))
                        PeriodTo.SetValueTrigger(periodTo, false);
                    else
                    {
                        int lastPeriod = GFunc.NEInt(PeriodTo.Rows[PeriodTo.Rows.Count - 1].Cells["Period"].Value, 0);
                        if(lastPeriod==0)//Bcos of AddComboEmptyValue funtin
                            lastPeriod = GFunc.NEInt(PeriodTo.Rows[PeriodTo.Rows.Count - 2].Cells["Period"].Value, 0);
                        PeriodTo.SetValueTrigger(lastPeriod, false);
                    }
                    Combo_CustomUpdate(sender, e);
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
                                    
                                    case "budgetbranchkey":
                                    case "budgetdeptkey":
                                    case "budgetreckey":
                                    case "budgetrecsubkey":
                                    case "budgetperiod":
                                    case "budgetmode":                                    
                                    case "budgetitmmode":                                  

                                        GlobalUI.ItemNotInList(grd.ActiveCell, null, 1);// ItemNotInListAdd
                                        break;
                                    case "budgettype":
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

        }//CodeCompleted
        private void tagrdCurrentYear_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            try
            {
                bool processOk = true;
                if (this.tagrdCurrentYear.ActiveRow != null)
                {
                    processOk = BaseUtility.Validation(out msgID, this.tagrdCurrentYear.ActiveRow.Cells["BudgetAmountH"].Value, "BudgetAmountH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null);
                    if (processOk)
                        processOk = BaseUtility.Validation(out msgID, this.tagrdCurrentYear.ActiveRow.Cells["BudgetQty"].Value, "BudgetQty", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null);
                    if (processOk)
                        processOk = BaseUtility.Validation(out msgID, this.tagrdCurrentYear.ActiveRow.Cells["BudgetWeight"].Value, "BudgetWeight", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null);

                    #region Check processOK
                    if (!processOk)
                    {
                        e.Cancel = true;
                        this.objMstBudgetFactory.ObjMSTBudgets.Rows[tagrdCurrentYear.ActiveRow.Index].RowError = SysMessageUtility.Get(msgID);
                    }
                    else
                    {
                        this.objMstBudgetFactory.ObjMSTBudgets.Rows[tagrdCurrentYear.ActiveRow.Index].RowError = string.Empty;
                        objMstBudgetFactory.IsDirty = true;
                    }
                    #endregion
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

        private void GetVendors()
        {
            RecKey.SetValueTrigger(0,false);
            GlobalUI.BindComboValue(RecKey, GVar.ListSettingID.MSTConPurchase_id,"ID","Key",0);
        }
        private void GetCustomers()
        {
            RecKey.SetValueTrigger(0,false);
           // GlobalUI.BindComboValue(RecKey, "MSTConCustAllByPermmission %" + AppInfor.ConAccessLevel + "%" + AppInfor.ConAccessGroup);
            GlobalUI.BindComboValue(RecKey, GVar.ListSettingID.MSTConSalesAll_id, "ID", "Key", 0);
        }
        private void GetAccs()
        {
            RecKey.SetValueTrigger(0,false);
            GlobalUI.BindComboValue(RecKey, GVar.ListSettingID.MSTAccAll_id, "ID", "Key", 0);           
        }       
        private void GetDocGrps()
        {
            RecKey.SetValueTrigger(0,false);
            //GlobalUI.BindComboValue(RecKey, "REFDocGrpSortID");
            GlobalUI.BindComboValue(RecKey, GVar.ListSettingID.REFDocGrp, "DocGrpID", "DocGrpKey", 0);
        }
        private void GetItems()
        {
            RecKey.SetValueTrigger(0,false);
            GlobalUI.BindComboValue(RecKey, GVar.ListSettingID.MSTItmFSCANVG_id, "ID", "Key", 0);
          
        }       
        private void GetIndustrys()
        {
            RecKey.SetValueTrigger(0,false);
            //GlobalUI.BindComboValue(RecKey, "REFIndustrySortID");
            GlobalUI.BindComboValue(RecKey, GVar.ListSettingID.REFIndustry, "IndustryID", "IndustryKey", 0);
        }
        private void GetTerritorys()
        {
            RecKey.SetValueTrigger(0,false);
           // GlobalUI.BindComboValue(RecKey, "REFTerritorySortID");
            GlobalUI.BindComboValue(RecKey, GVar.ListSettingID.REFTerritory, "TerritoryID", "TerritoryKey", 0);
        }
        private void GetItemsRecSub()
        {
            BudgetRecSubKey.SetValueTrigger(0,false);
           // GlobalUI.BindComboValue(BudgetRecSubKey, "MSTItemBudgetList");
            GlobalUI.BindComboValue(BudgetRecSubKey, GVar.ListSettingID.MSTItmAll_id, "ID", "Key", 0);
        }

        private void ClearBudgetDetails()
        {
            objMstBudgetFactory.ObjPrevMSTBudgets.Rows.Clear();            
            objMstBudgetFactory.ObjMSTBudgets.Rows.Clear();
            Refresh_PreviousYear();
            Refresh_CurrentYear();
        }
        private void Refresh_CurrentYear()
        {
            try
            {
                // Object binding for User Information                 
                this.tagrdCurrentYear.DataSource = this.objMstBudgetFactory.ObjMSTBudgets;
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
        private void Refresh_PreviousYear()
        {
            try
            {
                // Object binding for User Information
                this.tagrdPreviousYear.DataSource = this.objMstBudgetFactory.ObjPrevMSTBudgets;         
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
        private void LockGrids()
        {
            try
            {
                GlobalUI.GridAllColumnsActivateOnlySet(tagrdPreviousYear);
                foreach (UltraGridColumn c in tagrdCurrentYear.DisplayLayout.Bands[0].Columns)
                    if (c.Key != "BudgetAmountH" && c.Key != "BudgetQty" && c.Key != "BudgetWeight")
                        c.CellActivation = Activation.ActivateOnly;
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false); // System Msg
            }
            
        }
        
        private bool Save_Process(string _case)
        {
            // Waiting cursor
            this.Cursor = Cursors.WaitCursor;

            // Variable Declaration
            bool isSave = false;

            try
            {
                // Validation
                //if (GFunc.CompareString(_case , "combo")==false)
                //    this.Validate();

                if (tagrdCurrentYear.ActiveRow != null)
                    tagrdCurrentYear.ActiveRow.Update();

                // Check Factory Object is not null
                if (this.objMstBudgetFactory != null)
                {
                    // Check Validation
                    if (GFunc.CompareString(_case , "combo")==false)
                        isSave = Validation();
                    else
                        isSave = ValidationToOldValue();

                    // Call Save
                    if (isSave)
                    {
                        MSTBudget CurrMSTBudgetBackup = objMstBudgetFactory.ObjMSTBudget.Clone();

                        //Set value to factory for Heading save                         
                        if (GFunc.CompareString(_case, "combo") == false)
                            isSave = this.objMstBudgetFactory.Save(GFunc.NEInt(RecKey.Value, 0), (GEnum.BudgetItemMode)BudgetItmMode.Value);
                        else
                        {                           
                            objMstBudgetFactory.ObjMSTBudget.BudgetBranchKey =GFunc.NEInt(BudgetBranchKey.OldValue,0);
                            objMstBudgetFactory.ObjMSTBudget.BudgetDeptKey = GFunc.NEInt(BudgetDeptKey.OldValue,0);
                            objMstBudgetFactory.ObjMSTBudget.BudgetItmMode = GFunc.NEInt(BudgetItmMode.OldValue,0);
                            objMstBudgetFactory.ObjMSTBudget.BudgetRecKey =GFunc.NEInt(RecKey.OldValue,0);
                            objMstBudgetFactory.ObjMSTBudget.BudgetRecSubKey = GFunc.NEInt(BudgetRecSubKey.OldValue,0);
                            objMstBudgetFactory.ObjMSTBudget.BudgetType = GFunc.NEInt(BudgetType.OldValue, 0);
                            isSave = this.objMstBudgetFactory.Save(GFunc.NEInt(RecKey.OldValue, 0), (GEnum.BudgetItemMode)BudgetItmMode.Value);
                        }

                        // Check Process
                        if (isSave)
                        {
                            if (GFunc.IsNE(this.MdiParent))
                            {
                                this.Close();
                                return true;
                            }
                            objMstBudgetFactory.ObjMSTBudget = CurrMSTBudgetBackup;
                            bdsMSTBudget.DataSource = objMstBudgetFactory.ObjMSTBudget;
                            this.Refresh_PreviousYear();
                            this.Refresh_CurrentYear();

                            // Call ReadOnly
                            this.OnReadOnly();
                        }
                    }
                    
                }
            }
            catch (TAException tex)
            {
                Error(tex, false);
            }
            catch (Exception ex)
            {
                Error(ex, false);
            }
            finally
            {
                // Default cursor
                this.Cursor = Cursors.Default;
            }
            return isSave;
        }        
        private bool Validation()
        {
            bool processOK = false;
            string propname = string.Empty; //For Focus().

            try
            {
                propname = "BudgetType";
                processOK = BaseUtility.Validation(out msgID, BudgetType.Value, "BudgetType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null);               

                if (processOK)
                {
                    propname = "BudgetRecKey";
                    processOK = BaseUtility.Validation(out msgID, RecKey.Value, "BudgetRecKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null);                    
                }

                if (processOK)
                {
                    if ((int)GEnum.BudgetType.Document_Group_and_Item_Sales == (int)BudgetType.Value || (int)GEnum.BudgetType.Document_Group_Item_Purchases == (int)BudgetType.Value)
                    {
                        propname = "BudgetRecSubKey";
                        processOK = BaseUtility.Validation(out msgID, BudgetRecSubKey.Value, "BudgetRecSubKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null);
                    }
                }
               
                if (processOK)
                {
                    if (GFunc.IsNEZ(PeriodFrom.Value))
                    {
                        processOK = false;
                        PeriodFrom.Focus();
                        throw new TAException("PeriodFromIsRequire");
                    }
                    else if (GFunc.IsNEZ(PeriodTo.Value))
                    {
                        processOK = false;
                        PeriodTo.Focus();
                        throw new TAException("PeriodToIsRequire");
                    }
                    else if ((int)PeriodFrom.Value > (int)PeriodTo.Value)
                    {
                        processOK = false;
                        PeriodFrom.Focus();
                        throw new TAException("PeriodFromGreaterThanTo");
                    }
                }

                if (!processOK)
                {
                    if (GFunc.CompareString(propname , "BudgetRecKey"))
                    {
                        RecKey.Focus();
                        propname = RecKey_lbl.Text + " cannot be empty";
                        throw new TAException(propname);
                    }
                    else if (GFunc.CompareString(propname , "BudgetRecSubKey"))
                    {
                        this.BudgetRecSubKey.Focus();
                        throw new TAException("Items cannot be empty");
                    }
                    else
                    {
                        if (GFunc.CompareString(propname , "BudgetType"))
                        { this.BudgetType.Focus(); }
                        if (GFunc.CompareString(propname , "BudgetItmMode"))
                        { this.BudgetItmMode.Focus(); }

                        throw new TAException(msgID);
                    }
                }

                //Check combo null value
                if (GFunc.IsNE(BudgetBranchKey.Value))
                    BudgetBranchKey.SetValueTrigger(0,false);
                if (GFunc.IsNE(BudgetDeptKey.Value))
                    BudgetDeptKey.SetValueTrigger(0,false);
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            return processOK;
        }
        private bool ValidationToOldValue()
        {
            bool processOK = false;
            string propname = string.Empty; //For Focus().
            Control c = this.ActiveControl;

            try
            {
                propname = "BudgetType";                
                processOK = BaseUtility.Validation(out msgID, BudgetType.OldValue, "BudgetType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null);

                if (processOK)
                {
                    propname = "BudgetRecKey";
                    processOK = BaseUtility.Validation(out msgID, RecKey.OldValue, "BudgetRecKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null);
                }

                if (processOK)
                {
                    if ((int)GEnum.BudgetType.Document_Group_and_Item_Sales == (int)BudgetType.OldValue || (int)GEnum.BudgetType.Document_Group_Item_Purchases == (int)BudgetType.OldValue)
                    {
                        propname = "BudgetRecSubKey";
                        processOK = BaseUtility.Validation(out msgID, BudgetRecSubKey.OldValue, "BudgetRecSubKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null);                        
                    }
                }              

                if (processOK)
                {
                    //PeriodFrom is Unbound field, old value has to retrieve from Factory object
                    if (GFunc.IsNEZ(objMstBudgetFactory.FromBudgetPeriod))
                    {
                        processOK = false;
                        PeriodFrom.Focus();
                        throw new TAException("PeriodFromIsRequire");
                    }
                    //PeriodTo is Unbound field, old value has to retrieve from Factory object
                    else if (GFunc.IsNEZ(objMstBudgetFactory.ToBudgetPeriod))
                    {
                        processOK = false;
                        PeriodTo.Focus();
                        throw new TAException("PeriodToIsRequire");
                    }
                    else if (objMstBudgetFactory.FromBudgetPeriod > objMstBudgetFactory.ToBudgetPeriod)                   
                    {
                        processOK = false;
                        PeriodFrom.Focus();
                        throw new TAException("PeriodFromGreaterThanTo");
                    }
                }

                if (!processOK)
                {
                    if (GFunc.CompareString(propname , "BudgetRecKey"))
                    {
                        RecKey.Focus();
                        propname = RecKey_lbl.Text + " cannot be empty";
                        throw new TAException(propname);
                    }
                    else if (GFunc.CompareString(propname , "BudgetRecSubKey"))
                    {
                        this.BudgetRecSubKey.Focus();
                        throw new TAException("Items cannot be empty");
                    }
                    else
                    {
                        if (GFunc.CompareString(propname , "BudgetType"))
                        { this.BudgetType.Focus(); }
                        //if (GFunc.CompareString(propname , "BudgetItmMode"))
                        //{ this.BudgetItmMode.Focus(); }

                        throw new TAException(msgID);
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
            return processOK;
        }
        private void OnReadOnly()
        {
            // Set Readonly True (or) False. Based on Factory ReadOnly State       
            this.btnAssign.Enabled = !this.objMstBudgetFactory.IsOpenReadOnly;
            this.btnAssignAll.Enabled = !this.objMstBudgetFactory.IsOpenReadOnly;
            this.FormulaExpandableGroupBox.Enabled = !this.objMstBudgetFactory.IsOpenReadOnly;
            this.tagrdCurrentYear.Enabled = !this.objMstBudgetFactory.IsOpenReadOnly;
            this.tagrdPreviousYear.Enabled = !this.objMstBudgetFactory.IsOpenReadOnly;

            // Check Factory Object is ReadOnly ...
            if (this.objMstBudgetFactory.IsOpenReadOnly)
            {
                this.tslReadOnly.Text = "Read Only";
                this.tsbSave.Enabled = false;
            }
            else
            {
                this.tslReadOnly.Text = string.Empty;
                this.tsbSave.Enabled = true;
            }
        }
        private void Clear_Process()
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;
            bool processOk = true;
            try
            {
                // Check Form Validation
                this.Validate();

                // Check Factory Object is Dirty ...
                if (this.objMstBudgetFactory.IsDirty)
                {
                    // Check Option Value is True (or) False
                    if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnClearRecord))
                    {
                        // Ask Confirmation for Clear
                        GEnum.MsgBoxButton btnSelect;
                        btnSelect = MsgBox.Show(MsgID.Common.ConfirmClear,
                                              GEnum.MsgBoxIcon.Question,
                                              GEnum.MsgBoxButton.Clear,
                                              GEnum.MsgBoxButton.Dont_Clear,
                                              GEnum.MsgBoxButton.I_Dont_Know);

                        // Check Delete
                        if (btnSelect == GEnum.MsgBoxButton.Clear)
                        {
                            processOk = true;
                        }
                        else
                            processOk = false;
                    }
                }
                if (processOk)
                    processOk = this.objMstBudgetFactory.Clear();
                //Check Process
                if (processOk)
                {
                    //Set UI

                    bdsMSTBudget.DataSource = objMstBudgetFactory.ObjMSTBudget;
                    SetDocItemSubVisibility();
                    this.Setcombovalue();
                    this.SetGridColumns(); //Set Grid Column
                    ClearBudgetDetails();
                    this.LockGrids();
                    //this.BudgetType.SetValueTrigger(0,false);
                    this.BudgetType.Focus();
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
                //Waiting Cursor
                this.Cursor = Cursors.Default;
            }
        }
        private bool SaveChanges(string _case)
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;
            bool isOK = true;
            try
            {
                // Check Form Validation
                if (GFunc.CompareString(_case , "combo")==false)
                    this.Validate();

                //try to update active row
                if (GFunc.IsNEZ(BudgetType.Value))
                    return true;
                try
                {
                    if (tagrdCurrentYear.ActiveRow != null)
                        this.tagrdCurrentYear.ActiveRow.Update();
                }
                catch
                {
                    isOK = false;
                }

                // Check Factory Object is Dirty ...
                if (this.objMstBudgetFactory.IsDirty)
                {
                    // Ask Confirmation To Save
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show(MsgID.Common.SaveChanges,
                                          GEnum.MsgBoxIcon.Question,
                                          GEnum.MsgBoxButton.Save_Changes,
                                          GEnum.MsgBoxButton.Discard_Changes,
                                          GEnum.MsgBoxButton.I_Dont_Know);

                    //No, I don't want to save
                    if (btnSelect == GEnum.MsgBoxButton.I_Dont_Know)
                        return false;
                    // Yes, I want to save
                    else if (btnSelect == GEnum.MsgBoxButton.Save_Changes)
                    {
                        isOK = this.Save_Process(_case);
                    }
                }
                if (isOK)
                    return true;              
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
            return false;
        }               

        #region Error

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
                    l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, GFunc.GridColumnKey_Get(this.ActiveControl) });
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
                if (this.ActiveControl.GetType() != typeof(TAUtil.TAGridEditor))
                {
                    l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl });
                }
                else
                {
                    l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, GFunc.GridColumnKey_Get(this.ActiveControl) });
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
