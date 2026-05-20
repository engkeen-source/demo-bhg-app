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
using System.Data.SqlClient;
using Infragistics.Win.Misc;

namespace WinUI
{
    public partial class frmMSTItmInvBal : Form
    {
        #region Local Variables
        private int _CodeKey = 0;
        string ContextMenuSetting = string.Empty;
        private string msgID = string.Empty;
        private bool formClose = false;
        string ListSettingID = string.Empty;
        DataTable dtMstItmBal;
        DataView dv;
        #endregion

        // Initialize
        public frmMSTItmInvBal()
        {
            InitializeComponent();
        }        

        // Form Events        
        private void frmList_Load(object sender, EventArgs e)
        {
            try
            {             
                if (this.formClose == false)
                {
                    try
                    {
                        Grid_Rebind();
                        GlobalUI.FormGrids_Set(this, 23100, out ContextMenuSetting);
                        ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(_CodeKey, this.Name);
                        tagrdList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
                        tagrdList.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
                        tagrdList.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True;
                        tagrdList.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
                        tagrdList.DisplayLayout.Bands[0].Columns["ItemStatus"].Header.Appearance.TextHAlign = HAlign.Center;
                        tagrdList.DisplayLayout.Bands[0].Columns["ItemStatus"].CellAppearance.ImageHAlign = HAlign.Center;
                        tagrdList.DisplayLayout.Bands[0].Columns["ItemStatus"].CellActivation = Activation.NoEdit;
                        tagrdList.DisplayLayout.Bands[0].Columns["KittedQty_Standby"].CellAppearance.ImageHAlign = HAlign.Left;
                        tagrdList.DisplayLayout.Bands[0].Columns["PickedQty_ToExportDO"].CellAppearance.ImageHAlign = HAlign.Left;
                        tagrdList.DisplayLayout.Bands[0].Columns["PickedQty_ToExportPCN"].CellAppearance.ImageHAlign = HAlign.Left;                        
                        tagrdList.DisplayLayout.Bands[0].Columns["ReceivedQty_ToExportPD"].CellAppearance.ImageHAlign = HAlign.Left;
                        tagrdList.DisplayLayout.Bands[0].Columns["ReceivedQty_ToExportSCN"].CellAppearance.ImageHAlign = HAlign.Left;   
                                           
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
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
        private void frmList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
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

        // Grid Events        
        private void tagrdList_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells.Exists("ItemStatus"))
            {
                e.Row.Cells["ItemStatus"].ActiveAppearance.ForeColor = Color.Transparent;
                //e.Row.Cells["ItemStatus"].ActiveAppearance.BackColor = Color.White;
                e.Row.Cells["ItemStatus"].Appearance.ForeColor = Color.Transparent;
                e.Row.Cells["ItemStatus"].Appearance.TextHAlign = HAlign.Left;
                string itemStatus = GFunc.NEStr(e.Row.Cells["ItemStatus"].Value, "green");               

                if (itemStatus == "red")
                {
                    e.Row.Cells["ItemStatus"].Appearance.Image = global::WinUI.Properties.Resources.red_14;
                }                
                else
                {
                    e.Row.Cells["ItemStatus"].Appearance.Image = global::WinUI.Properties.Resources.green_14;
                }
                //else if (itemStatus == "orange")
                //{
                //    e.Row.Cells["ItemStatus"].Appearance.Image = global::WinUI.Properties.Resources.orange_14;
                //}
                //else if (itemStatus == "blue")
                //{
                //    e.Row.Cells["ItemStatus"].Appearance.Image = global::WinUI.Properties.Resources.light_blue_14;
                //}

            }

            //if (e.Row.Cells.Exists("ItemType") && GFunc.NEStr(e.Row.Cells["ItemType"].Value, "").ToLower().Contains("kit"))
            //{
            //    e.Row.Cells["KittedQty_Standby"].Appearance.Image = global::WinUI.Properties.Resources.light_blue_14;
            //}
            if (e.Row.Cells.Exists("KittedQty_Standby") && GFunc.NEDec(e.Row.Cells["KittedQty_Standby"].Value, 0) != 0)
            {
                e.Row.Cells["KittedQty_Standby"].Appearance.Image = global::WinUI.Properties.Resources.light_blue_14;
            }            
            if (e.Row.Cells.Exists("PickedQty_ToExportDO") && GFunc.NEDec(e.Row.Cells["PickedQty_ToExportDO"].Value,0) != 0)
            {
                e.Row.Cells["PickedQty_ToExportDO"].Appearance.Image = global::WinUI.Properties.Resources.orange_14;
            }
            if (e.Row.Cells.Exists("PickedQty_ToExportPCN") && GFunc.NEDec(e.Row.Cells["PickedQty_ToExportPCN"].Value, 0) != 0)
            {
                e.Row.Cells["PickedQty_ToExportPCN"].Appearance.Image = global::WinUI.Properties.Resources.orange_14;
            }            
            if (e.Row.Cells.Exists("ReceivedQty_ToExportSCN") && GFunc.NEDec(e.Row.Cells["ReceivedQty_ToExportSCN"].Value, 0) != 0)
            {
                e.Row.Cells["ReceivedQty_ToExportSCN"].Appearance.Image = global::WinUI.Properties.Resources.orange_14;
            }
            if (e.Row.Cells.Exists("ReceivedQty_ToExportPD") && GFunc.NEDec(e.Row.Cells["ReceivedQty_ToExportPD"].Value, 0) != 0)
            {
                e.Row.Cells["ReceivedQty_ToExportPD"].Appearance.Image = global::WinUI.Properties.Resources.orange_14;
            }
            
        }
        private void tagrdList_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            try
            {
                e.Row.Selected = true;
                string ItmID = "", CheckCol = "";
                ItmID = GFunc.NEStr(tagrdList.ActiveRow.Cells["ItemName"].Value, 0);
                CheckCol = tagrdList.ActiveCell.Column.Key;
                switch (CheckCol)
                {
                    case "GoodsInvnQty":
                    case "ExpiryItemQty":
                    case "KittedQty_Standby":
                    case "PickedQty_ToExportDO":
                    case "PickedQty_ToExportPCN":
                    case "ReceivedQty_ToExportSCN":
                    case "ReceivedQty_ToExportPD":
                    case "LastNTDocDate_TotalItmQty":
                        frmPopupWMSInfo f;
                        //If it is already loaded, take that one
                        foreach (Form form in Application.OpenForms[0].OwnedForms)
                        {
                            if (form.Name == "frmPopupWMSInfo")
                            {
                                f = (frmPopupWMSInfo)form;
                                f.Reload(ItmID, CheckCol);
                                return;
                            }
                        }

                        //If it's not loaded yet, create new
                        f = new frmPopupWMSInfo(ItmID, CheckCol);
                        f.Show(frmMain.gfrmMain);
                        break;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Click Events
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.formClose = true;
            this.Close();
        }               
        private void tsbExport_Click(object sender, EventArgs e)
        {
            try
            {
                int count = tsbExport.Tag == null ? 1 : (int)tsbExport.Tag;
                GlobalUI.Export(tagrdList, ref count);
                tsbExport.Tag = count;
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
        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Grid_Rebind();
                if (dv != null)
                {
                    ((DataTable)tagrdList.DataSource).DefaultView.RowFilter = dv.RowFilter;
                    DisplayRowCount();
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
        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtKeywordSearch.Text = string.Empty;
            ClearFilter();
        }
        private void tsbClearFilter_Click(object sender, EventArgs e)
        {
            ClearFilter();
            Grid_Rebind();
        }
        private void lblColorIndicator_Click(object sender, EventArgs e)
        {
            try
            {
                frmMain.gfrmMain.SetNotifyStatus("Loading.... Please wait.");
                UltraLabel lbl = (UltraLabel)sender;
                ClearFilter();

                #region // columnFilter 
                /*                            
                ColumnFiltersCollection columnFilter = tagrdList.DisplayLayout.Bands[0].ColumnFilters;               
                if (lbl.Tag.ToString() == "stock")
                {
                    columnFilter["ItmType"].FilterConditions.Add(FilterComparisionOperator.Equals, "100");
                }
                else if (lbl.Tag.ToString() == "orange")
                {
                    var layoutBand = tagrdList.DisplayLayout.Bands[0];
                    layoutBand.ColumnFilters.LogicalOperator = FilterLogicalOperator.Or;

                    layoutBand.ColumnFilters["PickedQty_ToExportDO"].FilterConditions.Add(FilterComparisionOperator.NotEquals, 0);
                    layoutBand.ColumnFilters["ReceivedQty_ToExportPD"].FilterConditions.Add(FilterComparisionOperator.NotEquals, 0);
                    layoutBand.ColumnFilters["PickedQty_ToExportPCN"].FilterConditions.Add(FilterComparisionOperator.NotEquals, 0);
                    layoutBand.ColumnFilters["ReceivedQty_ToExportSCN"].FilterConditions.Add(FilterComparisionOperator.NotEquals, 0);
                    layoutBand.ColumnFilters["KittedQty_Standby"].FilterConditions.Add(FilterComparisionOperator.NotEquals, 0);

                }
                else
                {
                    columnFilter["ItemStatus"].FilterConditions.Add(FilterComparisionOperator.Equals, lbl.Tag.ToString());
                }
                */
                #endregion

                #region GridFilterByDataView
                dv = ((DataTable)tagrdList.DataSource).DefaultView;
                if (lbl.Tag.ToString() == "stock")
                {                   
                    dv.RowFilter = $"ItmType = 100";
                }
                else if (lbl.Tag.ToString() == "blue")
                {
                    dv.RowFilter = $"ItemType LIKE '%{"kit"}%'";
                }
                else if (lbl.Tag.ToString() == "orange")
                {
                    dv.RowFilter = $"PickedQty_ToExportDO > 0 or ReceivedQty_ToExportPD > 0 or PickedQty_ToExportPCN > 0 or ReceivedQty_ToExportSCN > 0 ";
                }
                else
                {
                    dv.RowFilter = $"ItemStatus LIKE '%{lbl.Tag.ToString()}%'";
                }
                #endregion

                DisplayRowCount();

            }
            catch
            {

            }
            finally
            {
                frmMain.gfrmMain.SetNormalStaus("Ready");
            }
        }
        
        private void Grid_Rebind()
        {
            try
            {
                frmMain.gfrmMain.SetNotifyStatus("Loading.... Please wait.");
                List<SqlParameter> parmList = new List<SqlParameter>();
                dtMstItmBal = GFunc.ExecuteProc("MstItm_InvBalGet", parmList);

                tagrdList.DataSource = dtMstItmBal;
                tagrdList.DataBind();

                //var rows = dtMstItmBal.AsEnumerable().Where(r => r.Field<int>("ItmType") == 100);
                var rows = dtMstItmBal.AsEnumerable();

                int allCount = rows.Count(r => r.Field<int>("ItmType") == 100);
                int redCount = rows.Count(r => r.Field<string>("ItemStatus") == "red");
                int greenCount = rows.Count(r => r.Field<string>("ItemStatus") == "green");
                int blueCount = rows.Count(r => r.Field<string>("ItemType").ToLower().Contains("kit"));
                int orangeCount = rows.Count(r =>
                    GFunc.NEDec(r["PickedQty_ToExportDO"], 0) != 0 ||
                    GFunc.NEDec(r["PickedQty_ToExportPCN"], 0) != 0 ||                   
                    GFunc.NEDec(r["ReceivedQty_ToExportSCN"], 0) != 0 ||
                    GFunc.NEDec(r["ReceivedQty_ToExportPD"], 0) != 0
                );

                lblAll.Text = "All Active Stock (" + allCount.ToString("N0") + ")";               
                lblGreen.Text = "Matched (" + greenCount.ToString("N0") + ")";
                lblRed.Text = "Discrepancy (" + redCount.ToString("N0") + ")";
                lblOrange.Text = "In Progess (" + orangeCount.ToString("N0") + ")";
                lblBlue.Text = "Kitted Components (" + blueCount.ToString("N0") + ")";
                DisplayRowCount();
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
                this.Cursor = Cursors.Default;
                frmMain.gfrmMain.SetNormalStaus("Ready");
            }
        }
        private void ClearFilter()
        {
            txtKeywordSearch.Text = "";
            ((DataTable)tagrdList.DataSource).DefaultView.RowFilter = "";
            foreach (Infragistics.Win.UltraWinGrid.UltraGridBand band in tagrdList.DisplayLayout.Bands)
            {
                band.ColumnFilters.ClearAllFilters();
            }
            DisplayRowCount();
        }
        private void DisplayRowCount()
        {
            lblTotalRows.Text = "Total: " + tagrdList.Rows.VisibleRowCount.ToString("N0");
            //lblTotalRows.Text = "Total: " + ((DataTable)tagrdList.DataSource).DefaultView.Count.ToString("N0");
        }

        private void txtKeywordSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtKeywordSearch.Text.Trim().ToLower();
            FilterRowsHighlightKeyword(keyword);
        }
        private void FilterRowsHighlightKeyword(string keyword)
        {            
            bool hasKeyword = !string.IsNullOrEmpty(keyword);

            // Disable layout updates during the loop to prevent flickering and boost speed
            tagrdList.BeginUpdate();

            try
            {
                // Define the columns we care about
                string[] targetColumns = { "ItemNo", "ItemName", "Description" };

                foreach (UltraGridRow row in tagrdList.Rows)
                {
                    bool matchFound = false;

                    // Only check the specific columns you requested
                    foreach (string colKey in targetColumns)
                    {
                        UltraGridCell cell = row.Cells[colKey];
                        string cellValue = cell.Text;

                        // Use IndexOf with OrdinalIgnoreCase for better performance than .ToLower().Contains()
                        if (hasKeyword && cellValue.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            cell.Appearance.BackColor = Color.Yellow;
                            matchFound = true;
                        }
                        else
                        {
                            cell.Appearance.ResetBackColor();
                        }
                    }

                    // Hide/Show based on the 3-column match
                    row.Hidden = hasKeyword && !matchFound;
                }
            }
            finally
            {
                // Re-enable layout updates
                tagrdList.EndUpdate();
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

        #endregion

        //Context Menu
        //private void formatGridToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        frmGrdFormat fgrdformat = new frmGrdFormat(tagrdList, ListSettingID);
        //        fgrdformat.ShowDialog();
        //    }
        //    catch (TAException tex)
        //    {
        //        Error(tex, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        Error(ex, true);
        //    }

        //}

    }
}