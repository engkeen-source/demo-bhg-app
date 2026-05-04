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
using System.Data.SqlClient;
using BOLib;
using TAUtil;

namespace WinUI
{
    public partial class frmMSTBudgetCopy : Form
    {
        #region Variable Declaration

        private MSTBudgetFactory _objMSTBudgetFactory;
        private GEnum.BudgetType _budgetType;
        private int _periodfrom=0;
        private int _periodto = 0;
        private string msgID = string.Empty;
        internal bool? processOk = true;
        string ContextMenuSetting = string.Empty;

        #endregion

        //Initialize
        public frmMSTBudgetCopy()
        {
            InitializeComponent();
        }
        public frmMSTBudgetCopy(MSTBudgetFactory obj, GEnum.BudgetType _budgetType)
        {
            this._objMSTBudgetFactory = obj;
            this._budgetType = _budgetType;
            InitializeComponent();
        }

        //Form Event
        private void frmMSTBudgetCopy_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //Get Records
                this.BudgetType.SetValueTrigger(_budgetType.ToString().Replace('_', ' '), false);
                //Form Setting Code are written in frmMSTBudgetCopy_Shown event in order to retrieve budget type

                switch ((int)_budgetType)
                {
                    //For Budget Type 110,120,420,440
                    case (int)GEnum.BudgetType.Item_Sales:
                    case (int)GEnum.BudgetType.Item_Purchases:
                    case (int)GEnum.BudgetType.Document_Group_and_Item_Sales:
                    case (int)GEnum.BudgetType.Document_Group_Item_Purchases:
                        panel2.Enabled = true;
                        break;
                    default: //For Budget Others
                        panel2.Enabled = false;
                        break;
                }

                GlobalUI.Combos_Fill(this, 0);              
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

        private void frmMSTBudgetCopy_Shown(object sender, EventArgs e)
        {
            try
            {
                //Set ContextMenu & Grid Setting & Grid Formatting
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0, this.Name);

                switch (_budgetType)
                {
                    case GEnum.BudgetType.Account:
                        LoadAccount();
                        break;

                    case GEnum.BudgetType.Item_Sales:
                    case GEnum.BudgetType.Item_Purchases:
                        LoadItem();
                        break;

                    case GEnum.BudgetType.Customer_Sales:
                        LoadCustomer();
                        break;

                    case GEnum.BudgetType.Industry_Sales:
                    case GEnum.BudgetType.Industry_Purchase:
                        LoadIndustry();
                        break;

                    case GEnum.BudgetType.Territory_Sales:
                    case GEnum.BudgetType.Territory_Purchase:
                        LoadTerritory();
                        break;

                    case GEnum.BudgetType.Vendor_Purchase:
                        LoadVendor();
                        break;

                    case GEnum.BudgetType.Document_Group_Sales:
                    case GEnum.BudgetType.Document_Group_Purchase:
                        LoadDocument_Group();                    
                        break;            

                    case GEnum.BudgetType.Document_Group_and_Item_Sales:
                    case GEnum.BudgetType.Document_Group_Item_Purchases:
                        LoadDocument_Group();
                        LoadItem();
                        break;

                    default:
                        break;
                }

                this.FromBranchID.SetValueTrigger(_objMSTBudgetFactory.FromBranchID, false);
                this.ToBranchID.SetValueTrigger(_objMSTBudgetFactory.ToBranchID, false);
                this.FromDeptID.SetValueTrigger(_objMSTBudgetFactory.FromDeptID, false);
                this.ToDeptID.SetValueTrigger( _objMSTBudgetFactory.ToDeptID, false);
                this.FromBudgetPeriod.SetValueTrigger(_objMSTBudgetFactory.FromBudgetPeriod, false);
                this.ToBudgetPeriod.SetValueTrigger( _objMSTBudgetFactory.ToBudgetPeriod, false);
                this.Ratio.SetValueTrigger( _objMSTBudgetFactory.AmountRatio.ToString(), false);
                this.AddValue.SetValueTrigger(_objMSTBudgetFactory.AddAmount.ToString(), false);
                this.UnitRatio.SetValueTrigger(_objMSTBudgetFactory.UnitRatio.ToString(), false);
                this.AddUnit.SetValueTrigger(_objMSTBudgetFactory.AddUnit.ToString(), false);

                tagrdGroupList.DisplayLayout.Bands[0].Columns["RecordID"].CellActivation = Activation.ActivateOnly;
                tagrdGroupList.DisplayLayout.Bands[0].Columns["RecordDes"].CellActivation = Activation.ActivateOnly;
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

        private void frmMSTBudgetCopy_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)_objMSTBudgetFactory.ConstantCodeKey);
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
        }//Completed
        
        //Button Click Events
        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Copy_Process();
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
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }//Completed
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (UltraGridRow rows in tagrdGroupList.Rows)
                {
                    rows.Cells["Selected"].Value = true;
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
        private void btnUnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (UltraGridRow rows in tagrdGroupList.Rows)
                {
                    rows.Cells["Selected"].Value = false;
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

        //Control Event
        private void SelectBudgetRecKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                this.Validate();              
                ColumnFiltersCollection colFilter = this.tagrdGroupList.DisplayLayout.Bands[0].ColumnFilters;
                if (SelectBudgetRecKey.Text != string.Empty)
                {
                    switch (_budgetType)
                    {
                        case GEnum.BudgetType.Account:
                        case GEnum.BudgetType.Item_Sales:
                        case GEnum.BudgetType.Item_Purchases:
                        case GEnum.BudgetType.Document_Group_and_Item_Sales:
                        case GEnum.BudgetType.Document_Group_Item_Purchases:
                        case GEnum.BudgetType.Customer_Sales:
                        case GEnum.BudgetType.Vendor_Purchase:                      
                        case GEnum.BudgetType.Industry_Sales:
                        case GEnum.BudgetType.Industry_Purchase:
                        case GEnum.BudgetType.Territory_Sales:
                        case GEnum.BudgetType.Territory_Purchase:
                            colFilter.ClearAllFilters();
                            colFilter["RecordKey"].FilterConditions.Add(FilterComparisionOperator.NotEquals, GFunc.NEInt(SelectBudgetRecKey.Value,0));
                            break;
                        case GEnum.BudgetType.Document_Group_Sales:
                        case GEnum.BudgetType.Document_Group_Purchase:
                            colFilter.ClearAllFilters();
                            colFilter["RecordKey"].FilterConditions.Add(FilterComparisionOperator.NotEquals,GFunc.NEInt(DocGrp.Value,0));
                            break;
                        default:
                            break;
                    }                         
                }
                else
                {
                    colFilter.ClearAllFilters();
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

        private void DocGrp_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                this.Validate();               
                if (DocGrp.Text != string.Empty)
                {
                    switch (_budgetType)
                    {                       
                        case GEnum.BudgetType.Document_Group_Sales:
                        case GEnum.BudgetType.Document_Group_Purchase:
                            ColumnFiltersCollection colFilter = this.tagrdGroupList.DisplayLayout.Bands[0].ColumnFilters;
                            colFilter.ClearAllFilters();
                            colFilter["RecordKey"].FilterConditions.Add(FilterComparisionOperator.NotEquals, GFunc.NEInt(DocGrp.Value, 0));
                            break;
                        default:
                            break;
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

        //Form Display - Controlling,format and Control data refresh - Dependant combo, TextEditorPopup, Grid - Combo List, Set/Clear TextEditorPop value and Grid binding source and filter
        private void LoadCustomer()
        {
            try
            {
                this.ulabSelect.Text = "Customer";
                GlobalUI.BindComboValue(SelectBudgetRecKey, GVar.ListSettingID.MSTConSales_id, "ID", "Key",0);
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
        private void LoadVendor()
        {
            try
            {
                this.ulabSelect.Text = "Vendor";
                GlobalUI.BindComboValue(SelectBudgetRecKey, GVar.ListSettingID.MSTConPurchase_id, "ID", "Key",0);
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
        private void LoadIndustry()
        {
            try
            {
                this.ulabSelect.Text = "Industry";
                GlobalUI.BindComboValue(SelectBudgetRecKey, GVar.ListSettingID.REFIndustry, "IndustryID", "IndustryKey", 0);
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
        private void LoadTerritory()
        {
            try
            {
                this.ulabSelect.Text = "Territory";
                GlobalUI.BindComboValue(SelectBudgetRecKey, GVar.ListSettingID.REFTerritory, "TerritoryID", "TerritoryKey",0);
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
        private void LoadDocument_Group()
        {
            this.DocGrp.Enabled = true;
            try
            {
                GlobalUI.BindComboValue(DocGrp, GVar.ListSettingID.REFDocGrp, "DocGrpID", "DocGrpKey", 0);

                if (_budgetType != GEnum.BudgetType.Document_Group_and_Item_Sales && _budgetType != GEnum.BudgetType.Document_Group_Item_Purchases)
                {
                    this.SelectBudgetRecKey.Enabled = false;
                    ulabSelect.Text = "Items";
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
        private void LoadAccount()
        {
            try
            {               
                this.ulabSelect.Text = "Account";
                GlobalUI.BindComboValue(SelectBudgetRecKey, GVar.ListSettingID.MSTAccAll_id, "ID", "Key", 0);
            }
            catch (TAException tex)
            {
                throw Error(tex,false);
            }
            catch (Exception ex)
            {              
                throw Error(ex,false);
            }
        }
        private void LoadItem()
        {
            try
            {
                this.ulabSelect.Text = "Item";
                GlobalUI.BindComboValue(SelectBudgetRecKey, GVar.ListSettingID.MSTItmFSCANVG_id, "ID", "Key", 0);             
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
        private void GetAccBranchs()
        {
            try
            {
                FromBranchID.SetValueTrigger(0, false);
                GlobalUI.BindComboValue(FromBranchID, "MSTAccBranchSortID");

                ToBranchID.SetValueTrigger(0, false);
                GlobalUI.BindComboValue(ToBranchID, "MSTAccBranchSortID");
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
        private void GetAccDepts()
        {
            try
            {
                FromDeptID.SetValueTrigger(0, false);
                GlobalUI.BindComboValue(FromDeptID, "MSTAccDeptSortID");

                ToDeptID.SetValueTrigger(0, false);
                GlobalUI.BindComboValue(ToDeptID, "MSTAccDeptSortID");
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
        private void Copy_Process()
        {
            try
            {
                #region Check value
                if (DocGrp.Enabled)
                    if (GFunc.IsNE(DocGrp.Value) && GFunc.NEInt(DocGrp.Value, 0) == 0)
                    {
                        DocGrp.Focus();
                        throw new TAException("Document Group cannot be empty");
                    }
                if (GFunc.IsNE(SelectBudgetRecKey.Value) || GFunc.NEInt(SelectBudgetRecKey.Value, 0) == 0)
                {
                    if (_budgetType == GEnum.BudgetType.Document_Group_and_Item_Sales || _budgetType == GEnum.BudgetType.Document_Group_Item_Purchases)
                    {
                        SelectBudgetRecKey.Focus();
                        throw new TAException(ulabSelect.Text + " cannot be empty");
                    }
                }
                if (GFunc.IsNEZ(FromBudgetPeriod.Value))
                {
                    FromBudgetPeriod.Focus();
                    throw new TAException("PeriodFromIsRequire");
                }
                else if (GFunc.IsNEZ(ToBudgetPeriod.Value))
                {
                    ToBudgetPeriod.Focus();
                    throw new TAException("PeriodToIsRequire");
                }
                else if ((int)FromBudgetPeriod.Value > (int)ToBudgetPeriod.Value)
                {
                    FromBudgetPeriod.Focus();
                    throw new TAException("PeriodFromGreaterThanTo");
                }
                #endregion

                #region Set value
                if (DocGrp.Enabled)
                {
                    _objMSTBudgetFactory.SelectBudgetRecKey = GFunc.NEInt(this.DocGrp.Value, 0);
                    _objMSTBudgetFactory.SelectBudgetRecSubKey = GFunc.NEInt(this.SelectBudgetRecKey.Value, 0);
                }
                else
                {
                    _objMSTBudgetFactory.SelectBudgetRecKey = GFunc.NEInt(this.SelectBudgetRecKey.Value, 0);
                    _objMSTBudgetFactory.SelectBudgetRecSubKey = 0;
                }
                _objMSTBudgetFactory.FromBranchID = this.FromBranchID.Text;
                _objMSTBudgetFactory.ToBranchID = this.ToBranchID.Text;
                _objMSTBudgetFactory.FromDeptID = this.FromDeptID.Text;
                _objMSTBudgetFactory.ToDeptID = this.ToDeptID.Text;
                _objMSTBudgetFactory.FromBudgetPeriod = GFunc.NEInt(this.FromBudgetPeriod.Value, 0);
                _objMSTBudgetFactory.ToBudgetPeriod = GFunc.NEInt(this.ToBudgetPeriod.Value, 0);
                _objMSTBudgetFactory.AmountRatio = GFunc.NEDec(this.Ratio.Value, 0);
                _objMSTBudgetFactory.AddAmount = GFunc.NEDec(this.AddValue.Value, 0);
                _objMSTBudgetFactory.UnitRatio = GFunc.NEDec(this.UnitRatio.Value, 0);
                _objMSTBudgetFactory.AddUnit = GFunc.NEDec(this.AddUnit.Value, 0);
                #endregion

                #region Copy data
                if (_objMSTBudgetFactory.Copy(tagrdGroupList))
                    MsgBox.Show(MsgID.CommonSuccess.CopySuccess);
                #endregion
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
        //Not ready; Not used by anywhere
        private void GetPeriodMonth()
        {
            DataTable dtMonth = BOLib.SYSList.GetSystemPeriodMonths((int)GEnum.SystemPeriodStatus.All, 0);

            FromBudgetPeriod.DataSource = dtMonth;
            FromBudgetPeriod.ValueMember = "Period";
            FromBudgetPeriod.DisplayMember = "MonthYear";

            ToBudgetPeriod.DataSource = dtMonth;
            ToBudgetPeriod.ValueMember = "Period";
            ToBudgetPeriod.DisplayMember = "MonthYear";
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
        }//CodeCompleted
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
        }//CodeCompleted
       
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
                    SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] {});
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
