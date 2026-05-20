using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using BOLib;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using System.Collections;
using System.IO;
using TAUtil;

namespace WinUI
{
    public partial class frmRptFileSetting : Form
    {
        #region Member Variables, Properties, Constructors and Destructors

        private BOLib.SYSRepRptFactory objFactory = null;
        private string msgID = string.Empty;
        private string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        
        public frmRptFileSetting()
        {
            InitializeComponent();
        }
        #endregion

        private void frmRptFileSetting_Load(object sender, EventArgs e)
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;
            try
            {
                bool bRO;
                // Call Initialization
                this.objFactory = new BOLib.SYSRepRptFactory();
                if (!SECPermUtility.Any(SYSRepRptFactory.constPermID, out bRO, false))
                {
                    formClose = true;
                    return;
                }

                this.RefreshReports();              

                //Format all grids and filter
                GlobalUI.FormGrids_Set(this, (int)objFactory.CodeKey,out ContextMenuSetting);

                //Set ContextMenu
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objFactory.CodeKey);
                GlobalUI.Combos_Fill(this, (int)objFactory.CodeKey);
                
                FormatReportGrid();
                RepGrp.SetValueTrigger(900,false);
                RepGrpTAComboBox_CustomUpdate(this, null);
                if (tagrdReports.Rows.VisibleRowCount > 0)
                {
                    tagrdReports.Rows.FirstVisibleCardRow.Cells["RepDes"].Selected = true;
                    tagrdReports.Rows.FirstVisibleCardRow.Cells["RepDes"].Activate();
                }
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
                    MsgBox.Show(tex.MsgID);
                    this.formClose = true;
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void RepGrpTAComboBox_CustomUpdate(object sender, CancelEventArgs e)
        {
            int RepGrpKey = 0;
            if (formClose)
                return;
            if (SaveChanges())
            {
                if (!GFunc.IsNEZ(RepGrp.Value))
                    Int32.TryParse(RepGrp.Value.ToString(), out RepGrpKey);                

                if (this.tagrdReports.DisplayLayout.Bands[0].Columns.Count > 0)
                {
                    //ColumnFiltersCollection columnFilterHDR = this.tagrdReports.DisplayLayout.Bands[0].ColumnFilters;
                    //columnFilterHDR.ClearAllFilters();
                    //columnFilterHDR["RepGrp"].FilterConditions.Add(FilterComparisionOperator.Equals, RepGrpKey);
                    //GridFilterToDefaultView   

                    if (RepGrpKey == 0)
                        ((DataTable)tagrdReports.DataSource).DefaultView.RowFilter = "";
                    else
                        ((DataTable)tagrdReports.DataSource).DefaultView.RowFilter = "RepGrp="+RepGrpKey;
                }               
            }
            else
            {
                e.Cancel = true;
                return;
            }
            if (tagrdReports.Selected.Cells.Count > 0)
                tagrdReports.Selected.Cells.Clear();
            if (tagrdReports.Rows.VisibleRowCount > 0)
            {
                if (tagrdReports.Rows.FirstVisibleCardRow != null)
                {
                    UltraGridColumn VisibleCol = tagrdReports.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                    tagrdReports.Rows.FirstVisibleCardRow.Cells[VisibleCol.Key].Selected = true;
                }
            }
        }

        private void RefreshReports()
        {
            objFactory.GetAllReports();
            tagrdReports.DataSource = objFactory.ObjSYSRep;
            tagrdReports.DataBind();
        }

        private void FormatReportGrid()
        {
            try
            {
                //tagrdReports.DisplayLayout.Bands[0].Columns["RepKey"].Hidden = true;
                //tagrdReports.DisplayLayout.Bands[0].Columns["RepGrp"].Hidden = true;
                //tagrdReports.DisplayLayout.Bands[0].Columns["RepType"].Hidden = true;
                //tagrdReports.DisplayLayout.Bands[0].Columns["RepRemarks"].Hidden = true;
                //tagrdReports.DisplayLayout.Bands[0].Columns["RPTCaption"].Hidden = true;
                //tagrdReports.DisplayLayout.Bands[0].Columns["RPTRecordSource1"].Hidden = true;
                //tagrdReports.DisplayLayout.Bands[0].Columns["RPTRecordSource2"].Hidden = true;

                //tagrdReports.DisplayLayout.Bands[0].Columns["RepDes"].Header.Caption = "Report Name";
                //tagrdReports.DisplayLayout.Bands[0].Columns["RPTname1"].Header.Caption = "Default RPT File";
                //tagrdReports.DisplayLayout.Bands[0].Columns["RPTname2"].Header.Caption = "Secondary RPT File";
                tagrdReports.DisplayLayout.Bands[0].Columns["RepGrp"].CellClickAction = CellClickAction.CellSelect;
                tagrdReports.DisplayLayout.Bands[0].Columns["RepDes"].CellClickAction = CellClickAction.CellSelect;
                tagrdReports.DisplayLayout.Bands[0].Columns["RPTname1"].CellClickAction = CellClickAction.CellSelect;
                tagrdReports.DisplayLayout.Bands[0].Columns["RPTname2"].CellClickAction = CellClickAction.CellSelect;

                //tagrdReports.DisplayLayout.Bands[0].Columns["RepDes"].Width = 420;
                //tagrdReports.DisplayLayout.Bands[0].Columns["RPTname1"].Width = 120;
                //tagrdReports.DisplayLayout.Bands[0].Columns["RPTname2"].Width = 120;

                tagrdReports.ActiveRowScrollRegion.Scrollbar = Scrollbar.Show;
                this.tagrdReports.Refresh();


                // Check error
                if (msgID != string.Empty)
                {
                    MsgBox.Show(msgID);  // Custom Msg                    
                }

            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message); // System Msg
            }
            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }

        private void FormatRPTFileGrid()
        {
            try
            {
                string listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "0GF", tagrdRpts.Name);
                GlobalUI.Grid_Format(tagrdRpts, listID, false, false);

                //Set Grid Controls and format
                GlobalUI.GridControl_Set(tagrdRpts, listID, (int)this.objFactory.CodeKey);                

                if (!GFunc.IsNEZ(RepGrp.Value))

                    if ((int)RepGrp.Value == 900)//Document_PrintOut
                    {
                        tagrdRpts.DisplayLayout.Bands[0].Columns["RptLayOut"].Hidden = false;
                        tagrdRpts.DisplayLayout.Bands[0].Columns["ShwItmCount"].Hidden = false;
                        tagrdRpts.DisplayLayout.Bands[0].Columns["ShwLetterHead"].Hidden = false;
                        tagrdRpts.DisplayLayout.Bands[0].Columns["PrtCopies"].Hidden = false;
                     
                    }
                    else
                    {
                        tagrdRpts.DisplayLayout.Bands[0].Columns["RptLayOut"].Hidden = true;
                        tagrdRpts.DisplayLayout.Bands[0].Columns["ShwItmCount"].Hidden = true;
                        tagrdRpts.DisplayLayout.Bands[0].Columns["ShwLetterHead"].Hidden = true;
                        tagrdRpts.DisplayLayout.Bands[0].Columns["PrtCopies"].Hidden = true;

                    }              
              
                tagrdRpts.DisplayLayout.Bands[0].Columns["RptDes"].CellMultiLine = DefaultableBoolean.True;
                tagrdRpts.DisplayLayout.Bands[0].Columns["RptDes"].VertScrollBar = true;

                //Set default values
                tagrdRpts.DisplayLayout.Bands[0].Columns["RptLayOut"].DefaultCellValue = 10;
                tagrdRpts.DisplayLayout.Bands[0].Columns["ShwLetterHead"].DefaultCellValue = false;
                tagrdRpts.DisplayLayout.Bands[0].Columns["ShwItmCount"].DefaultCellValue = false;
                tagrdRpts.DisplayLayout.Bands[0].Columns["PrtCopies"].DefaultCellValue = 1;


            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message); // System Msg
            }
            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }

        private void frmRptFileSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
                {
                    e.Cancel = true;
                    return;
                }

                if (this.SaveChanges())
                {
                    formClose = true;
                    // Call Dispose
                    bool isOk = this.objFactory.Dispose();

                    //When the form is closed by main form, to proceed closing 
                    frmMain.gfrmMain.Tag = string.Empty;
                }
                else
                {
                    //When the form is closed by main form, to prohibit closing
                    frmMain.gfrmMain.Tag = GVar.CancelMainFormClosing;
                    e.Cancel = true;
                }

            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message); // System Msg 
            }
            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }

        private void frmRptFileSetting_Shown(object sender, EventArgs e)
        {
            // Check Form Close State is True ...
            if (formClose)
            {
                this.Close();
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            // Waiting cursor
            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.Validate();
                this.tagrdRpts.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdRpts.UpdateData();

                bool processOk = true;

                if (objFactory != null)
                {
                    if (tagrdReports.Selected.Cells.Count > 0)
                        tagrdReports.Selected.Cells[0].Row.Update();

                    if (objFactory.IsDirty)
                    {
                        if (tagrdRpts.ActiveRow != null)
                            tagrdRpts.ActiveRow.Update();
                        objFactory.ObjSYSRepRptss.AcceptChanges();

                        SYSRep objRep = new SYSRep();

                        int RepKey = Convert.ToInt32(tagrdReports.Selected.Cells[0].Row.Cells["RepKey"].Value);

                        objRep.RepKey = RepKey;
                        objRep.RPTname1 = tagrdReports.Selected.Cells[0].Row.Cells["RPTname1"].Value.ToString();
                        objRep.RPTname2 = tagrdReports.Selected.Cells[0].Row.Cells["RPTname2"].Value.ToString();

                        processOk = objFactory.Save(objRep);

                        if (processOk)
                        {

                        }
                        else
                        {
                            throw new TAException(msgID);
                        }

                    }
                }
            }
            catch (TAException tex)
            {
                MsgBox.Show(tex.MsgID);     //Custom Msg
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);  //System Msg
            }

            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }

        private bool SaveChanges()
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;
            bool processOk = true;

            try
            {
                // Check Form Validation
                this.Validate();

                // Check Factory Object is Dirty ...
                if (this.objFactory.IsDirty && !formClose)
                {
                    if (!tsbAutoSave.Checked)
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
                        {
                            return false;
                        }
                        // Yes, I want to save
                        else if (btnSelect == GEnum.MsgBoxButton.Save_Changes)
                        {
                            tsbSave_Click(null, null);
                            if (msgID != string.Empty)
                                return false;
                        }
                    }
                    else
                    {
                        tsbSave_Click(null, null);
                        if (msgID != string.Empty)
                            return false;
                    }

                }
                if (processOk)
                    return true;
                throw new TAException(msgID);

            }
            catch (TAException tex)
            {
                MsgBox.Show(tex.MsgID);     //Custom Msg
                return false;
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);  //System Msg
                return false;
            }

            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
            return false;
        }

        private void RepGrpTAComboBox_ItemNotInList(object sender, Infragistics.Win.UltraWinEditors.ValidationErrorEventArgs e)
        {
            GlobalUI.ItemNotInList(RepGrp, e,false,null);
        }

        private void tagrdReports_BeforeSelectChange(object sender, BeforeSelectChangeEventArgs e)
        {
            int NewRepKey = 0;
            int RepKey = 0;

            if (e.NewSelections.Cells.Count > 0)
            {
                NewRepKey = GFunc.NEInt(e.NewSelections.Cells[0].Row.Cells["RepKey"].Value, 0);
            }
            else
                return;
            if (tagrdReports.Selected.Cells.Count > 0)
            {
                RepKey = GFunc.NEInt(tagrdReports.Selected.Cells[0].Row.Cells["RepKey"].Value, 0);
            }
            if (SaveChanges())
            {
                if (objFactory.GetEdit(NewRepKey))
                {
                    tagrdRpts.DataSource = objFactory.ObjSYSRepRptss;
                    tagrdRpts.DataBind();
                    FormatRPTFileGrid();
                }
            }
            else
            {
                e.Cancel = true;
                return;
            }
        }

        private void tagrdRpts_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string msgID = string.Empty;

                e.DisplayPromptMsg = false;
                if (tagrdReports.ActiveRow.Cells["RptName1"].Value == tagrdRpts.ActiveRow.Cells["RptNm"].Value)
                {
                    MsgBox.Show(MsgID.ReportSetting.AlreadyUsedAsDefaultReport);
                    e.Cancel = true;
                    return;
                }
                if (tagrdReports.ActiveRow.Cells["RptName2"].Value == tagrdRpts.ActiveRow.Cells["RptNm"].Value)
                {
                    MsgBox.Show(MsgID.ReportSetting.AlreadyUsedAsSecondaryReport);
                    e.Cancel = true;
                    return;
                }

                if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) != GEnum.MsgBoxButton.Delete)
                {
                    e.Cancel = true;
                }

            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnSecondary_Click(object sender, EventArgs e)
        {
            if (tagrdReports.ActiveRow == null)
            {
                UltraGridRow[] rows = tagrdReports.Rows.GetFilteredInNonGroupByRows();
                if (rows.Length > 0)
                    tagrdReports.ActiveRow = rows[0];
                else
                    return;
            }
            if (tagrdRpts.ActiveRow == null)
            {
                if (tagrdRpts.Rows.Count > 0)
                    tagrdRpts.ActiveRow = tagrdRpts.Rows[0];
                else
                    return;
            }
            tagrdReports.ActiveRow.Cells["RptName2"].Value = tagrdRpts.ActiveRow.Cells["RptNm"].Value;
        }

        private void btnSetDefault_Click(object sender, EventArgs e)
        {
            if (tagrdReports.ActiveRow == null)
            {
                UltraGridRow[] rows = tagrdReports.Rows.GetFilteredInNonGroupByRows();
                if (rows.Length > 0)
                    tagrdReports.ActiveRow = rows[0];
                else
                    return;
            }
            if (tagrdRpts.ActiveRow == null)
            {
                if (tagrdRpts.Rows.Count > 0)
                    tagrdRpts.ActiveRow = tagrdRpts.Rows[0];
                else
                    return;
            }
            tagrdReports.ActiveRow.Cells["RptName1"].Value = tagrdRpts.ActiveRow.Cells["RptNm"].Value;
        }

        private void tagrdRpts_AfterRowInsert(object sender, RowEventArgs e)
        {
            if (tagrdReports.ActiveRow == null)
            {
                UltraGridRow[] rows = tagrdReports.Rows.GetFilteredInNonGroupByRows();
                if (rows.Length > 0)
                    tagrdReports.ActiveRow = rows[0];
                else
                    return;
            }
            e.Row.Cells["RepKey"].Value = tagrdReports.ActiveRow.Cells["RepKey"].Value;
        }

        private void btnClearSecondary_Click(object sender, EventArgs e)
        {
            if (tagrdReports.ActiveRow == null)
            {
                UltraGridRow[] rows = tagrdReports.Rows.GetFilteredInNonGroupByRows();
                if (rows.Length > 0)
                    tagrdReports.ActiveRow = rows[0];
                else
                    return;
            }
            tagrdReports.ActiveRow.Cells["RptName2"].Value = "";
        }

        private void tagrdRpts_CustomDataError(object sender, TAUtil.TAErrorEventArgs e)
        {
            if (e.ErrorCode == TAUtil.TAErrorCode.NOT_IN_LIST)
            {
                if (tagrdRpts.ActiveCell.Column.EditorComponent != null)
                {
                    tagrdRpts.PerformAction(UltraGridAction.EnterEditMode);
                    GlobalUI.ItemNotInList(tagrdRpts.ActiveCell, null, 0);
                }
            }
            else
            {
                MsgBox.Show(e.ErrorMessage);
            }
        }

        private void tsbAutoSave_Click(object sender, EventArgs e)
        {
            if (tsbAutoSave.CheckState == CheckState.Checked)
            {
                tsbSave.Enabled = false;
                tsbAutoSave.Checked = true;
                tsbAutoSave.Image = WinUI.Properties.Resources.white_list_save_321;
                tsbAutoSave.ToolTipText = "Currently Auto Saving is on. Click this to turn it off";
            }
            else
            {
                tsbSave.Enabled = true;
                tsbAutoSave.Checked = false;
                tsbAutoSave.Image = WinUI.Properties.Resources.white_list_save_32;
                tsbAutoSave.ToolTipText = "Currently Auto Saving is off. Click this to turn it on";
            }
        }


        #region Set Error Methods
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

        private void frmRptFileSetting_KeyDown(object sender, KeyEventArgs e)
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
    }
}
