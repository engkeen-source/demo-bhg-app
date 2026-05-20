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
    public partial class frmFavouriteReportSetting : Form
    {
        #region Member Variables, Properties, Constructors and Destructors

        private BOLib.SYSRepOthersFactory objRepOthersFactory = null;
        private string msgID = string.Empty;
        private bool formClose = false;
        private bool AvailGrdDragging = false;
        private bool AssignGrdDragging = false;

        private string ContextMenuSetting = string.Empty;

        public frmFavouriteReportSetting()
        {
            InitializeComponent();
        }
        #endregion

        private void frmFavouriteReportSetting_Load(object sender, EventArgs e)
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;
            try
            {
                bool bRO;
                // Call Initialization
                this.objRepOthersFactory = new BOLib.SYSRepOthersFactory(out msgID);
                if (!SECPermUtility.Any(SYSRepRptFactory.constPermID, out bRO, false))
                {
                    formClose = true;
                    return;
                }
                this.RefreshReports();                
                FormatAvailableGrid();                

                GlobalUI.FormGrids_Set(this, (int)objRepOthersFactory.ConstantCodeKey, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);             
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objRepOthersFactory.ConstantCodeKey);
                GlobalUI.Combos_Fill(this, (int)objRepOthersFactory.ConstantCodeKey);
                if (SecGrp.Rows.Count > 0)
                    SecGrp.SelectedRow = SecGrp.Rows[0];
                SecGrpTAComboBox_CustomUpdate(null, new CancelEventArgs());

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
                    MsgBox.Show(tex.MsgID); // Custom Msg
                    this.formClose = true;

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

        private void SecGrpTAComboBox_CustomUpdate(object sender, CancelEventArgs e)
        {
            int RepGrpKey = 0;
            this.errorProvider1.Clear();
            if (SaveChanges())
            {
                if (!GFunc.IsNEZ(SecGrp.Value))
                    Int32.TryParse(SecGrp.Value.ToString(), out RepGrpKey);
                else
                    return;                

                if (objRepOthersFactory.GetEdit(RepGrpKey))
                {
                    tagrdAvailableReports.DataSource = objRepOthersFactory.ROSYSRep;
                    tagrdAvailableReports.DataBind();
                    tagrdAssignedRpts.DataSource = objRepOthersFactory.ObjSYS_RepOtherss;
                    tagrdAssignedRpts.DataBind();

                    FormatAssignGrid();
                    objRepOthersFactory.ROSYSRep.PrimaryKey = new DataColumn[] { objRepOthersFactory.ROSYSRep.Columns["RepKey"] };
                    objRepOthersFactory.ObjSYS_RepOtherss.PrimaryKey = new DataColumn[] { objRepOthersFactory.ObjSYS_RepOtherss.Columns["RepKey"] };
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void RefreshReports()
        {
            objRepOthersFactory.GetAllReports();
            tagrdAvailableReports.DataSource = objRepOthersFactory.ROSYSRep;
            tagrdAvailableReports.DataBind();
        }

        private void FormatAvailableGrid()
        {           
            try
            {               
                tagrdAvailableReports.DisplayLayout.Bands[0].Columns["RepKey"].Hidden = true;
                tagrdAvailableReports.DisplayLayout.Bands[0].Columns["RepGrp"].Hidden = true;
                tagrdAvailableReports.DisplayLayout.Bands[0].Columns["RepGrpDes"].Hidden = true;
               
                tagrdAvailableReports.DisplayLayout.Bands[0].Columns["RepDes"].Header.Caption = "Available Reports";
             //   grdAvailableReports.DisplayLayout.Bands[0].Columns["RepDes"].Header.Appearance.TextHAlign = HAlign.Center;
                tagrdAvailableReports.DisplayLayout.Bands[0].Columns["RepDes"].CellActivation = Activation.ActivateOnly;
                //tagrdAvailableReports.DisplayLayout.Bands[0].Columns["RepDes"].Header.Appearance.BackColor = System.Drawing.Color.PowderBlue;
                //tagrdAvailableReports.DisplayLayout.Bands[0].Columns["RepDes"].Header.Appearance.BackColor2 = System.Drawing.Color.White;
                //tagrdAvailableReports.DisplayLayout.Bands[0].Columns["RepDes"].Header.Appearance.BackGradientStyle = GradientStyle.GlassTop20;
                tagrdAvailableReports.DisplayLayout.Bands[0].Columns["RepDes"].Header.Appearance.ForeColor = System.Drawing.Color.Black;
                tagrdAvailableReports.DisplayLayout.Bands[0].Columns["RepDes"].Header.Appearance.FontData.Bold = DefaultableBoolean.True;

                //grdAvailableReports.DisplayLayout.Bands[0].Columns["RepDes"].Width = 356;
                this.tagrdAvailableReports.DisplayLayout.Bands[0].SortedColumns.Add("RepGrpDes", false, true);                
                this.tagrdAvailableReports.DisplayLayout.Bands[0].Columns["RepGrpDes"].GroupByMode = GroupByMode.Value;
               
                this.tagrdAvailableReports.DisplayLayout.Bands[0].Override.GroupByRowDescriptionMask = "[value] ([count] reports)";
                //this.grdAvailableReports.DisplayLayout.Bands[0].Override.GroupByRowSpacingAfter = 5;
                this.tagrdAvailableReports.DisplayLayout.Bands[0].Override.GroupByRowPadding= 5;
                this.tagrdAvailableReports.DisplayLayout.Bands[0].Override.GroupByColumnAppearance.FontData.SizeInPoints = 9;
          
                this.tagrdAvailableReports.Rows.Refresh(RefreshRow.FireInitializeRow, true);
                tagrdAvailableReports.ActiveRowScrollRegion.Scrollbar = Scrollbar.Show;
                this.tagrdAvailableReports.Refresh();
                //Set Appearence
                //Header
                Infragistics.Win.Appearance appearence_Header = new Infragistics.Win.Appearance();
                appearence_Header.AlphaLevel = ((short)(255));
                appearence_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.FontData.ItalicAsString = "True";
                appearence_Header.FontData.Name = "Calibri";
                appearence_Header.FontData.SizeInPoints = 10F;
                appearence_Header.ForeColor = System.Drawing.Color.Black;
                //appearence_Header.TextHAlignAsString = "LEFT";
                tagrdAvailableReports.DisplayLayout.Override.HeaderAppearance = appearence_Header;

                //Row
                Infragistics.Win.Appearance appearence_Row = new Infragistics.Win.Appearance();
                appearence_Row.FontData.Name = "Calibri";
                appearence_Row.FontData.SizeInPoints = 11F;
                appearence_Row.ForeColor = System.Drawing.Color.Black;
                appearence_Row.TextHAlignAsString = "LEFT";
                appearence_Row.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Row.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdAvailableReports.DisplayLayout.Override.RowAppearance = appearence_Row;

                //Cell
                Infragistics.Win.Appearance appearence_Cell = new Infragistics.Win.Appearance();
                appearence_Cell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Cell.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdAvailableReports.DisplayLayout.Override.CellAppearance = appearence_Cell;

                //Row Selector
                Infragistics.Win.Appearance appearence_RowSelector = new Infragistics.Win.Appearance();
                appearence_RowSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_RowSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdAvailableReports.DisplayLayout.Override.RowSelectorAppearance = appearence_RowSelector;

                //Appearence
                Infragistics.Win.Appearance appearence = new Infragistics.Win.Appearance();
                appearence.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdAvailableReports.DisplayLayout.Appearance = appearence;

                //Row Header Selector
                Infragistics.Win.Appearance appearence_RowHeaderSelector = new Infragistics.Win.Appearance();
                appearence_RowHeaderSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_RowHeaderSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdAvailableReports.DisplayLayout.Override.RowSelectorHeaderAppearance = appearence_RowHeaderSelector;


                tagrdAvailableReports.DisplayLayout.AutoFitStyle = AutoFitStyle.None;
                tagrdAvailableReports.TextRenderingMode = TextRenderingMode.GDI;

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

        private void FormatAssignGrid()
        {
            try
            {

                string listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "0GF", tagrdAssignedRpts.Name);
                GlobalUI.Grid_Format(tagrdAssignedRpts, listID, false, false);

                if (tagrdAssignedRpts.Rows.Count > 0)
                {
                    this.tagrdAssignedRpts.DisplayLayout.Bands[0].SortedColumns.Add("RepGrpDes", false, true);
                    this.tagrdAssignedRpts.DisplayLayout.Bands[0].Columns["RepGrpDes"].GroupByMode = GroupByMode.Value;

                    this.tagrdAssignedRpts.DisplayLayout.Bands[0].Override.GroupByRowDescriptionMask = "[value] ([count] reports)";
                    //this.tagrdAssignedRpts.DisplayLayout.Bands[0].Override.GroupByRowSpacingAfter = 5;
                    this.tagrdAssignedRpts.DisplayLayout.Bands[0].Override.GroupByRowPadding = 5;
                    this.tagrdAssignedRpts.DisplayLayout.Bands[0].Override.GroupByColumnAppearance.FontData.SizeInPoints = 9;

                    objRepOthersFactory.ObjSYS_RepOtherss.PrimaryKey = new DataColumn[] { objRepOthersFactory.ObjSYS_RepOtherss.Columns["RepKey"] };

                    foreach (UltraGridColumn col in tagrdAssignedRpts.DisplayLayout.Bands[0].Columns)
                    {
                        if (GFunc.CompareString(col.Key.ToUpper(), "REPDES") == false)
                            col.Hidden = true;
                        //else
                        //    col.Width = tagrdAssignedRpts.Width -16;
                    }
                    tagrdAssignedRpts.DisplayLayout.Bands[0].Columns["RepDes"].Header.Caption = "Assigned Reports";
                    //  tagrdAssignedRpts.DisplayLayout.Bands[0].Columns["RepDes"].Header.Appearance.TextHAlign = HAlign.Center;
                    tagrdAssignedRpts.DisplayLayout.Bands[0].Columns["RepDes"].CellActivation = Activation.ActivateOnly;
                    //tagrdAssignedRpts.DisplayLayout.Bands[0].Columns["RepDes"].Header.Appearance.BackColor = System.Drawing.Color.PowderBlue;
                    //tagrdAssignedRpts.DisplayLayout.Bands[0].Columns["RepDes"].Header.Appearance.BackColor2 = System.Drawing.Color.White;
                    //tagrdAssignedRpts.DisplayLayout.Bands[0].Columns["RepDes"].Header.Appearance.BackGradientStyle = GradientStyle.GlassTop20;
                    tagrdAssignedRpts.DisplayLayout.Bands[0].Columns["RepDes"].Header.Appearance.ForeColor = System.Drawing.Color.Black;
                    tagrdAssignedRpts.DisplayLayout.Bands[0].Columns["RepDes"].Header.Appearance.FontData.Bold = DefaultableBoolean.True;

                    tagrdAssignedRpts.ActiveRowScrollRegion.Scrollbar = Scrollbar.Show;
                }

                //Set Appearence
                //Header
                Infragistics.Win.Appearance appearence_Header = new Infragistics.Win.Appearance();
                appearence_Header.AlphaLevel = ((short)(255));
                appearence_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.FontData.ItalicAsString = "True";
                appearence_Header.FontData.Name = "Calibri";
                appearence_Header.FontData.SizeInPoints = 10F;
                appearence_Header.ForeColor = System.Drawing.Color.Black;
                //appearence_Header.TextHAlignAsString = "LEFT";
                tagrdAssignedRpts.DisplayLayout.Override.HeaderAppearance = appearence_Header;

                //Row
                Infragistics.Win.Appearance appearence_Row = new Infragistics.Win.Appearance();
                appearence_Row.FontData.Name = "Calibri";
                appearence_Row.FontData.SizeInPoints = 11F;
                appearence_Row.ForeColor = System.Drawing.Color.Black;
                appearence_Row.TextHAlignAsString = "LEFT";
                appearence_Row.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Row.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdAssignedRpts.DisplayLayout.Override.RowAppearance = appearence_Row;

                //Cell
                Infragistics.Win.Appearance appearence_Cell = new Infragistics.Win.Appearance();
                appearence_Cell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Cell.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdAssignedRpts.DisplayLayout.Override.CellAppearance = appearence_Cell;

                //Row Selector
                Infragistics.Win.Appearance appearence_RowSelector = new Infragistics.Win.Appearance();
                appearence_RowSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_RowSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdAssignedRpts.DisplayLayout.Override.RowSelectorAppearance = appearence_RowSelector;

                //Appearence
                Infragistics.Win.Appearance appearence = new Infragistics.Win.Appearance();
                appearence.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdAssignedRpts.DisplayLayout.Appearance = appearence;

                //Row Header Selector
                Infragistics.Win.Appearance appearence_RowHeaderSelector = new Infragistics.Win.Appearance();
                appearence_RowHeaderSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_RowHeaderSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdAssignedRpts.DisplayLayout.Override.RowSelectorHeaderAppearance = appearence_RowHeaderSelector;


                tagrdAssignedRpts.DisplayLayout.AutoFitStyle = AutoFitStyle.None;
                tagrdAssignedRpts.TextRenderingMode = TextRenderingMode.GDI;


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
        

        private void frmFavouriteReportSetting_FormClosing(object sender, FormClosingEventArgs e)
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

                // Call Dispose
                if (GFunc.IsNE(this.objRepOthersFactory)==false)                
                    this.objRepOthersFactory.Dispose();

                //When the form is closed by main form, to proceed closing 
                frmMain.gfrmMain.Tag = string.Empty;
                e.Cancel = false;

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

        private void frmFavouriteReportSetting_Shown(object sender, EventArgs e)
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
                bool processOk = true;         

                if ( objRepOthersFactory != null)
                {
                    //if (objRepOthersFactory.IsDirty)
                    //{
                        objRepOthersFactory.ObjSYS_RepOtherss.AcceptChanges();
                        processOk = objRepOthersFactory.Save(GFunc.NEInt( SecGrp.Value,0));

                        if (processOk)
                        {
                                                
                        }
                        else
                        {
                            throw new TAException(msgID);
                        }

                    //}
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

        private void tsbCopy_Click(object sender, EventArgs e)
        {
            int GrpKey = 0;

            if (GFunc.IsNEZ(SecGrp.Value))
            {               
                MsgBox.Show("NoGroupIsSelected");                
               this.errorProvider1.SetError(SecGrp, SysMessageUtility.Get("NoGroupIsSelected"));
                SecGrp.Focus();
                return;
            }
            else
            {
                GrpKey = Convert.ToInt32(SecGrp.Value);
                this.errorProvider1.SetError(SecGrp, "");
            }


            if (SecGrp.Rows.Count > 1)
            {
                frmSelectSecurityGrp sGrp = new frmSelectSecurityGrp((int)SecGrp.Value);
                if (sGrp.ShowDialog() == DialogResult.OK)
                {
                    if (objRepOthersFactory.Copy(Convert.ToInt32(SecGrp.Value), sGrp.SelectedSecGrpKey))
                        tagrdAssignedRpts.DataBind();
                }
            }
            else
                MsgBox.Show("Sorry! You cannot copy since there's only one security group");


        }

        private void CopyGroupSelected(int grpKey, string grpID)
        {
            //if (objRepOthersFactory.Copy(Convert.ToInt32(SecGrp.Value), grpKey))
            //    tagrdAssignedRpts.DataBind();
        }

        private void tsbClear_Click(object sender, EventArgs e)
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;
           
            try
            {
                // Check Form Validation
                this.Validate();

                // Check Factory Object is Dirty ...
                if (!GFunc.IsNEZ(SecGrp.Value))
                {
                    // Check Option Value is True (or) False
                    if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnClearRecord))
                    {
                        // Ask Confirmation for Delete
                        GEnum.MsgBoxButton btnSelect;
                        btnSelect = MsgBox.Show(MsgID.Common.ConfirmClear,
                                              GEnum.MsgBoxIcon.Question,
                                              GEnum.MsgBoxButton.Clear,
                                              GEnum.MsgBoxButton.Dont_Clear,
                                              GEnum.MsgBoxButton.I_Dont_Know);

                        // Check Clear
                        if (btnSelect == GEnum.MsgBoxButton.Clear)
                        {
                            // Call Clear
                            SecGrp.SetValueTrigger("",false);
                            objRepOthersFactory.IsDirty = false;
                            objRepOthersFactory.ObjSYS_RepOtherss.Rows.Clear();
                            tagrdAssignedRpts.DataBind();
                        }
                    }
                    else
                    {
                        // Call Clear
                        SecGrp.SetValueTrigger("",false);
                        objRepOthersFactory.IsDirty = false;
                        objRepOthersFactory.ObjSYS_RepOtherss.Rows.Clear();
                        tagrdAssignedRpts.DataBind();
                    }
                }
                else
                {
                    // Call Clear
                    SecGrp.SetValueTrigger("",false);
                    objRepOthersFactory.IsDirty = false;
                    objRepOthersFactory.ObjSYS_RepOtherss.Rows.Clear();
                    tagrdAssignedRpts.DataBind();
                }
            }
            catch (TAException tex)
            {
                MsgBox.Show(tex.MsgID);
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
                if (this.objRepOthersFactory.IsDirty)
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
                if (processOk)
                    return true;
                throw new TAException(msgID);

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
            return false;
        }

        private int GetSelectedGroupKey()
        {
            int GrpKey = 0;
            if (GFunc.IsNEZ(SecGrp.Value))
            {                
                MsgBox.Show("NoGroupIsSelected");                
                this.errorProvider1.SetError(SecGrp, SysMessageUtility.Get("NoGroupIsSelected"));
                SecGrp.Focus();
                return 0;
            }
            else
            {
                GrpKey = Convert.ToInt32(SecGrp.Value);
                this.errorProvider1.SetError(SecGrp, "");
            }
            return GrpKey;
        }

        private void btnAssign_Click(object sender, EventArgs e)
        {
            try
            {
                int GrpKey = GetSelectedGroupKey();
                if (GrpKey == 0)
                    return;

                if (tagrdAvailableReports.Selected.Rows.Count > 0)
                {
                    foreach (UltraGridRow row in tagrdAvailableReports.Selected.Rows)
                    {
                        if (row.IsGroupByRow)
                        {
                            UltraGridGroupByRow gRow = (UltraGridGroupByRow)row;
                            foreach (UltraGridRow childRow in gRow.Rows)
                            {

                                if (objRepOthersFactory.ObjSYS_RepOtherss.Rows.Find(childRow.Cells["RepKey"].Value) == null)
                                {
                                    DataRow dr = objRepOthersFactory.ObjSYS_RepOtherss.NewRow();
                                    dr["RepKey"] = childRow.Cells["RepKey"].Value;
                                    dr["RepGroup"] = GrpKey;
                                    dr["RepGrp"] = childRow.Cells["RepGrp"].Value;
                                    dr["RepGrpDes"] = childRow.Cells["RepGrpDes"].Value;
                                    dr["RepDes"] = childRow.Cells["RepDes"].Value;
                                    objRepOthersFactory.ObjSYS_RepOtherss.Rows.Add(dr);
                                }
                            }
                        }
                        else
                        {                            
                            if (objRepOthersFactory.ObjSYS_RepOtherss.Rows.Find(row.Cells["RepKey"].Value) ==null )
                            {
                                DataRow dr = objRepOthersFactory.ObjSYS_RepOtherss.NewRow();
                                dr["RepKey"] = row.Cells["RepKey"].Value;
                                dr["RepGroup"] = GrpKey;
                                dr["RepGrpDes"] = row.Cells["RepGrpDes"].Value;
                                dr["RepGrp"] = row.Cells["RepGrp"].Value;
                                dr["RepDes"] = row.Cells["RepDes"].Value;
                                objRepOthersFactory.ObjSYS_RepOtherss.Rows.Add(dr);                                
                            }
                        }
                    }
                    
                }
                foreach (DataRow dr in objRepOthersFactory.ObjSYS_RepOtherss.Rows)
                {
                    DataRow ddr = objRepOthersFactory.ROSYSRep.Rows.Find(dr["RepKey"].ToString());
                    if (ddr != null)
                    {
                        objRepOthersFactory.ROSYSRep.Rows.Remove(ddr);
                    }
                }
                objRepOthersFactory.ROSYSRep.AcceptChanges();
                tagrdAvailableReports.DataSource = objRepOthersFactory.ROSYSRep;
                tagrdAvailableReports.DataBind();
                tagrdAssignedRpts.DataBind();
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }

        private void btnAssignAll_Click(object sender, EventArgs e)
        {
            int GrpKey = GetSelectedGroupKey();
            if (objRepOthersFactory.ObjSYS_RepOtherss==null )
            {
                return;
            }
            objRepOthersFactory.ObjSYS_RepOtherss.Rows.Clear();
            foreach (UltraGridRow row in tagrdAvailableReports.Rows)
            {
                if (row.IsGroupByRow)
                {
                    UltraGridGroupByRow gRow = (UltraGridGroupByRow)row;
                    foreach (UltraGridRow childRow in gRow.Rows)
                    {                        
                        DataRow drc = objRepOthersFactory.ObjSYS_RepOtherss.NewRow();
                        drc["RepKey"] = childRow.Cells["RepKey"].Value;
                        drc["RepGrpDes"] = childRow.Cells["RepGrpDes"].Value;
                        drc["RepGroup"] = GrpKey;
                        drc["RepDes"] = childRow.Cells["RepDes"].Value;
                        objRepOthersFactory.ObjSYS_RepOtherss.Rows.Add(drc);                       
                    }
                    continue;
                }
                 
                DataRow dr = objRepOthersFactory.ObjSYS_RepOtherss.NewRow();
                dr["RepKey"] = row.Cells["RepKey"].Value;
                dr["RepGroup"] = GrpKey;
                dr["RepGrpDes"] = row.Cells["RepGrpDes"].Value;
                dr["RepDes"] = row.Cells["RepDes"].Value;
                objRepOthersFactory.ObjSYS_RepOtherss.Rows.Add(dr);                
            }
            tagrdAssignedRpts.DataBind();

            objRepOthersFactory.ROSYSRep.Rows.Clear();
            tagrdAvailableReports.DataBind();
        }
       
        private void btnRemove_Click(object sender, EventArgs e)
        {
            int i = 0;
            while (tagrdAssignedRpts.Selected.Rows.Count > 0 && tagrdAssignedRpts.Selected.Rows.Count!=i)
            {
                UltraGridRow row= tagrdAssignedRpts.Selected.Rows[i++];

                if (row.IsGroupByRow)
                {
                    UltraGridGroupByRow gRow = (UltraGridGroupByRow)row;
                    foreach (UltraGridRow childRow in gRow.Rows)
                    {
                        DataRow drc = objRepOthersFactory.ROSYSRep.NewRow();
                        drc["RepKey"] = childRow.Cells["RepKey"].Value;
                        drc["RepGrp"] = childRow.Cells["RepGrp"].Value;
                        drc["RepGrpDes"] = childRow.Cells["RepGrpDes"].Value;                        
                        drc["RepDes"] = childRow.Cells["RepDes"].Value;
                        objRepOthersFactory.ROSYSRep.Rows.Add(drc);
                        
                    }                    
                }
                else
                {
                    DataRow drc = objRepOthersFactory.ROSYSRep.NewRow();
                    drc["RepKey"] = row.Cells["RepKey"].Value;
                    drc["RepDes"] = row.Cells["RepDes"].Value;
                    drc["RepGrp"] = row.Cells["RepGrp"].Value;
                    drc["RepGrpDes"] = row.Cells["RepGrpDes"].Value;
                    objRepOthersFactory.ROSYSRep.Rows.Add(drc);                    
                }
            }
            
            foreach (DataRow dr in objRepOthersFactory.ROSYSRep.Rows)
            {
                DataRow ddr = objRepOthersFactory.ObjSYS_RepOtherss.Rows.Find(dr["RepKey"].ToString());
                if (ddr != null)
                {
                    objRepOthersFactory.ObjSYS_RepOtherss.Rows.Find(dr["RepKey"].ToString()).Delete();
                }
            }
            objRepOthersFactory.ObjSYS_RepOtherss.AcceptChanges();
            objRepOthersFactory.ROSYSRep.AcceptChanges();

            tagrdAvailableReports.DataBind();
            tagrdAssignedRpts.DataBind();
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            foreach (UltraGridRow row in tagrdAssignedRpts.Rows)
            {
                if (row.IsGroupByRow)
                {
                    UltraGridGroupByRow gRow = (UltraGridGroupByRow)row;
                    foreach (UltraGridRow childRow in gRow.Rows)
                    {
                        DataRow drc = objRepOthersFactory.ROSYSRep.NewRow();
                        drc["RepKey"] = childRow.Cells["RepKey"].Value;
                        drc["RepGrp"] = childRow.Cells["RepGrp"].Value;
                        drc["RepGrpDes"] = childRow.Cells["RepGrpDes"].Value;
                        drc["RepDes"] = childRow.Cells["RepDes"].Value;
                        objRepOthersFactory.ROSYSRep.Rows.Add(drc);
                    }
                }
                else
                {
                    DataRow dr = objRepOthersFactory.ROSYSRep.NewRow();
                    dr["RepKey"] = row.Cells["RepKey"].Value;
                    dr["RepDes"] = row.Cells["RepDes"].Value;
                    dr["RepGrp"] = row.Cells["RepGrp"].Value;
                    dr["RepGrpDes"] = row.Cells["RepGrpDes"].Value;
                    objRepOthersFactory.ROSYSRep.Rows.Add(dr);
                }
            }

            foreach (DataRow dr in objRepOthersFactory.ROSYSRep.Rows)
            {
                if (objRepOthersFactory.ObjSYS_RepOtherss.PrimaryKey == null || objRepOthersFactory.ObjSYS_RepOtherss.PrimaryKey.Length == 0)
                    objRepOthersFactory.ObjSYS_RepOtherss.PrimaryKey = new DataColumn[] { objRepOthersFactory.ObjSYS_RepOtherss.Columns["RepKey"] };
                DataRow ddr = objRepOthersFactory.ObjSYS_RepOtherss.Rows.Find(dr["RepKey"].ToString());
                if (ddr != null)
                {
                    objRepOthersFactory.ObjSYS_RepOtherss.Rows.Find(dr["RepKey"].ToString()).Delete();
                }
            }
            objRepOthersFactory.ObjSYS_RepOtherss.AcceptChanges();
            

            objRepOthersFactory.ObjSYS_RepOtherss.Rows.Clear();
            tagrdAssignedRpts.DataBind();
            tagrdAvailableReports.DataBind();
        }
       
        private void SecGrpTAComboBox_ItemNotInList(object sender, Infragistics.Win.UltraWinEditors.ValidationErrorEventArgs e)
        {
            GlobalUI.ItemNotInList(SecGrp, e, true, 1);
        }

        private void tsmnuAdd_Click(object sender, EventArgs e)
        {
            if (SecGrp.IsItemInList(SecGrp.Text))
            {
                MsgBox.Show("Item already exist.");
                return;
            }
            else
                GlobalUI.OpenFormAsDialog(SecGrp, GEnum.FormOpenMode.Add, SecGrp.Text, 0);
        }

        private void tsmnuEdit_Click(object sender, EventArgs e)
        {
            if (!SecGrp.IsItemInList(SecGrp.Text.Trim()))
            {
                MsgBox.Show("Item does not exist.");
                return;
            }
            else
                GlobalUI.OpenFormAsDialog(SecGrp, GEnum.FormOpenMode.Edit, SecGrp.Value, 0);            
        }

        private void AssignGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (tagrdAvailableReports.Selected.Cells.Count > 0)
                tagrdAvailableReports.Selected.Cells.Clear();

            if (tagrdAvailableReports.Selected.Cells.Count > 0)
                tagrdAvailableReports.Selected.Rows.Clear();

            Point gridPoint = new Point(e.X, e.Y);
            UIElement element = tagrdAssignedRpts.DisplayLayout.UIElement.ElementFromPoint(gridPoint);

            if (element != null)
            {
                Type selectedType = element.GetContext().GetType();

                if (selectedType == typeof(UltraGridRow)
                  || selectedType == typeof(UltraGridCell)
                  || selectedType == typeof(UltraGridColumn))
                    AssignGrdDragging = true;
            }
        }

        private void AssignGrid_MouseUp(object sender, MouseEventArgs e)
        {
            AssignGrdDragging = false;            
        }

        private void tagrdAvailableReports_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && AvailGrdDragging
               && ((tagrdAvailableReports.Selected.Rows.Count > 0) || 
               tagrdAvailableReports.Selected.Rows.Count>0))
            {
                GridMoveData moveData = new GridMoveData(tagrdAvailableReports);
                tagrdAvailableReports.DoDragDrop(moveData, DragDropEffects.Move);
            }
        }

        private void tagrdAssignedRpts_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(GridMoveData)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void tagrdAssignedRpts_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move)
            {
                GridMoveData moveData =
                   (GridMoveData)e.Data.GetData(typeof(GridMoveData));
                if (tagrdAssignedRpts.Equals(moveData.Source))
                    return;   // don't allow drop on same grid

                int GrpKey = GetSelectedGroupKey();
                
                if(GrpKey==0)
                    return;

                foreach (MoveData Rep in moveData.SelectedReports)
                {
                    //if (objRepOthersFactory.ObjSYS_RepOtherss.Rows.Find(Rep.RepKey) == null)
                    //{
                        DataRow dr = objRepOthersFactory.ObjSYS_RepOtherss.NewRow();
                        dr["RepKey"] = Rep.RepKey;
                        dr["RepGroup"] = GrpKey;
                        dr["RepDes"] = Rep.RepDes;
                        dr["RepGrp"] = Rep.RepGrpKey;
                        dr["RepGrpDes"] = Rep.RepGrpDes;
                        objRepOthersFactory.ObjSYS_RepOtherss.Rows.Add(dr);

                        objRepOthersFactory.ROSYSRep.Rows.Find(dr["RepKey"]).Delete();
                   // }
                }
            }
        }
     
        private void tagrdAssignedRpts_MouseMove(object sender, MouseEventArgs e)
        {          
            if (e.Button == MouseButtons.Left && AssignGrdDragging
             && ((tagrdAssignedRpts.Selected.Cells.Count > 0) ||
             tagrdAssignedRpts.Selected.Cells.Count > 0))
            {
                GridMoveData moveData = new GridMoveData(tagrdAssignedRpts);
                tagrdAssignedRpts.DoDragDrop(moveData, DragDropEffects.Move);                
            }
        }

        private void tagrdAssignedRpts_DragLeave(object sender, EventArgs e)
        {
            if (AssignGrdDragging && (sender as TAUtil.TAGridEditor) == tagrdAssignedRpts)
            {
                int i = 0;
                while (tagrdAssignedRpts.Selected.Rows.Count > 0)
                {
                    UltraGridRow row = tagrdAssignedRpts.Selected.Rows[i++];

                    DataRow drc = objRepOthersFactory.ROSYSRep.NewRow();
                    drc["RepKey"] = row.Cells["RepKey"].Value;
                    drc["RepDes"] = row.Cells["RepDes"].Value;
                    drc["RepGrpDes"] = row.Cells["RepGrpDes"].Value;
                    objRepOthersFactory.ROSYSRep.Rows.Add(drc);     

                    objRepOthersFactory.ObjSYS_RepOtherss.Rows.Find(row.Cells["RepKey"].Value).Delete();
                }
                tagrdAssignedRpts.DataBind();
            }
        }

        private void tagrdAvailableReports_MouseUp(object sender, MouseEventArgs e)
        {
            AvailGrdDragging = false;
        }

        private void tagrdAvailableReports_MouseDown(object sender, MouseEventArgs e)
        {
           // AvailGrdDragging = true;
            if (e.Button != MouseButtons.Left)
                return;

            if (tagrdAssignedRpts.Selected.Rows.Count > 0)
                tagrdAssignedRpts.Selected.Rows.Clear();

            Point gridPoint = new Point(e.X, e.Y);
            UIElement element = tagrdAvailableReports.DisplayLayout.UIElement.ElementFromPoint(gridPoint);

            if (element != null)
            {
                Type selectedType = element.GetContext().GetType();

                if (selectedType == typeof(UltraGridRow)
                    ||selectedType == typeof(UltraGridGroupByRow)
                  || selectedType == typeof(UltraGridCell)
                  || selectedType == typeof(UltraGridColumn))
                    AvailGrdDragging = true;
            }
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

        private void frmFavouriteReportSetting_KeyDown(object sender, KeyEventArgs e)
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

    public struct MoveData
    {
        public int RepKey;       
        public string RepDes;
        public int RepGrpKey;   
        public string RepGrpDes;
    }

    public class GridMoveData
    {
       private UltraGrid m_source;
       private List<MoveData> _Reports;

       public GridMoveData(UltraGrid source)
       {
         if (source == null)
            throw new ArgumentNullException("source");

         m_source = source;
         _Reports = GetReportsFromSource();
       }

       public UltraGrid Source {
           get{
               return m_source;
           }
       }

       public List<MoveData> SelectedReports 
       {    get 
            { 
                return _Reports; 
            } 
       }

       private List<MoveData> GetReportsFromSource()
       {   
           List<MoveData> list = new List<MoveData>();
           
           if (Source.Selected.Rows.Count > 0)
           {
               foreach (UltraGridRow row in Source.Selected.Rows)
               {
                   if (row.IsGroupByRow)
                   {
                       UltraGridGroupByRow gRow = (UltraGridGroupByRow)row;
                       foreach (UltraGridRow childRow in gRow.Rows)
                       {      
                           MoveData c = new MoveData();
                           c.RepKey = (int)childRow.Cells["RepKey"].Value;
                           if (!GFunc.IsNE(childRow.Cells["RepDes"].Value))
                               c.RepDes = childRow.Cells["RepDes"].Value.ToString();
                           c.RepGrpKey = (int)childRow.Cells["RepGrp"].Value;
                           c.RepGrpDes = childRow.Cells["RepGrpDes"].Value.ToString();

                           list.Add(c);
                       }
                   }
                   else
                   {
                       MoveData c = new MoveData();
                       c.RepKey = (int)row.Cells["RepKey"].Value;
                       if (!GFunc.IsNE(row.Cells["RepDes"].Value))
                           c.RepDes = row.Cells["RepDes"].Value.ToString();
                       c.RepGrpKey = (int)row.Cells["RepGrp"].Value;
                       c.RepGrpDes = row.Cells["RepGrpDes"].Value.ToString();
                       list.Add(c);
                   }
               }
           } 
          return list;
       }
    }
}
