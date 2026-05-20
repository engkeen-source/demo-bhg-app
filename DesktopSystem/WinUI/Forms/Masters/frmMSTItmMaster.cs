using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOLib;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using TAUtil;
namespace WinUI
{
    public partial class frmMSTItmMaster : Form
    {
        //Local Variable

        MSTItmFactory objMSTItmFactory;
        private string msgID = string.Empty;
        string ContextMenuSetting = string.Empty;
        #region Initialize

        public frmMSTItmMaster(MSTItmFactory objMSTItmFact)
        {
            this.objMSTItmFactory = objMSTItmFact;
            InitializeComponent();
        }
        public frmMSTItmMaster()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties
        public object ComboSource 
        { get; set; }

        public string DisplayMember { get; set; }

        public string ValueMember { get; set; }

        public object ReturnValue { get; set; }

        public string ReturnText { get; set; }
        #endregion

        //Form Events
        private void frmMSTItmMaster_Load(object sender, EventArgs e)
        {
            try
            {
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting, false);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0, this.Name);
                //GlobalUI.cmnuGlobal_Set(this);
                GlobalUI.Combos_Fill(this, 0);
                BuildDataGrid();
                BuildingCombo();
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
        private void frmMSTItmMaster_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, 0);
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

        //Combo Binding
        private void BuildingCombo()
        {
            try
            {
                MasterItmType.DataSource = ComboSource;
                MasterItmType.DisplayMember = DisplayMember;
                MasterItmType.ValueMember = ValueMember;
                MasterItmType.SetValueTrigger(ReturnValue, false);
            }
            catch (TAException ex)
            {
                throw Error(ex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex,false);
            }
        }

        //Grid Function
        private void BuildDataGrid()
        {
            try
            {
                bdsItem.DataSource = objMSTItmFactory.ItemTemplate;
                tagrdItems.DataSource = bdsItem.DataSource;
                tagrdItems.DisplayLayout.Bands[0].Columns["ColorKey"].Hidden = true;
                tagrdItems.DisplayLayout.Bands[0].Columns["ColorID"].Hidden = true;
                int iCol = 4;
                for (int i = 4; i < objMSTItmFactory.ItemTemplate.Columns.Count; i++)
                {
                    if (i > 2)
                    {
                        tagrdItems.DisplayLayout.Bands[0].Columns[i].Hidden = true;
                        iCol = i + 1;
                        i = iCol;
                    }               
                }
                //Empty Header in Color Column
                tagrdItems.DisplayLayout.Bands[0].Columns[2].Header.Caption = "";
                tagrdItems.DisplayLayout.Bands[0].Columns[2].CellAppearance.FontData.Bold = DefaultableBoolean.True;
                tagrdItems.DisplayLayout.Bands[0].Columns[2].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                tagrdItems.DisplayLayout.Bands[0].Columns[2].CellActivation= Infragistics.Win.UltraWinGrid.Activation.Disabled;
                tagrdItems.DisplayLayout.Bands[0].Columns[2].CellAppearance.ForeColorDisabled= System.Drawing.Color.Black;
                tagrdItems.DisplayLayout.Bands[0].Columns[2].CellAppearance.BackColor = System.Drawing.Color.AliceBlue;
                iCol = 3;
                for (int i = 3; i < objMSTItmFactory.ItemTemplate.Columns.Count; i++)
                {
                    if (i > 2)
                    {
                        tagrdItems.DisplayLayout.Bands[0].Columns[i].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;                  
                        tagrdItems.DisplayLayout.Bands[0].Columns[i].Width = 100;
                        iCol = i + 1;
                        i = iCol;
                    }
                }
                for (int i = 0; i < objMSTItmFactory.ItemTemplate.Columns.Count; i++)
                {
                    this.tagrdItems.DisplayLayout.Bands[0].Columns[i].Header.Appearance.FontData.Bold = DefaultableBoolean.True;
                    this.tagrdItems.DisplayLayout.Bands[0].Columns[i].Header.Appearance.TextHAlign = HAlign.Center;
                    this.tagrdItems.DisplayLayout.Bands[0].Columns[i].Header.Appearance.FontData.Underline = DefaultableBoolean.True;
                }

                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow item in tagrdItems.Rows)
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridCell cell in item.Cells)
                    {
                        if (cell.Column.Style == Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox)
                        {
                            if (Convert.ToBoolean(cell.Value) == true)
                            {
                                cell.Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
                            }
                        }
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

        //Menu Strip Events
        private void tsbSave_Click(object sender, EventArgs e)
        {
            try
            {
                string msgID = string.Empty;
                if (!GFunc.IsNE(tagrdItems.ActiveRow))
                    tagrdItems.ActiveRow.Update();
                objMSTItmFactory.SaveTemplateItems();
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
        private void tsbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void tagrdItems_CustomDataError(object sender, TAUtil.TAErrorEventArgs e)
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
                            grd.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
                            GlobalUI.ItemNotInList(grd.ActiveCell, null, 0);
                        }
                    }
                }
                if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_NUMERIC)
                {
                    throw new TAException(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
                }
                if (e.ErrorCode == TAUtil.TAErrorCode.FORMULA_NOT_RECOGNIZE)
                {
                    throw new TAException("FORMULA NOT RECOGNIZE");
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
    }
}
