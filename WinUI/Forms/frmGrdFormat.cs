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
    public partial class frmGrdFormat : Form
    {

        #region Local Variables
        private DataTable dtformatgrd = null;
        private string msgID = string.Empty;
        private bool formClose = false;
        private GEnum.SystemCode _codeKey;
        //public GEnum.SystemCode CodeKey { get { return _codeKey; } set { _codeKey = value; ;} }
        private string FormSettingID = string.Empty;

        private bool[] colTabStop;
        private bool[] colShowStatus;
        private string[] colHeaderList;
        private string[] colWidthList;
        private string[] colFormatList;
        private double rowHeightCM;
        private UltraGrid grid;

        bool showAllCol = false;
        #endregion

        //Initialize
        public frmGrdFormat()
        {
            InitializeComponent();
        }

        //public frmGrdFormat(bool[] parashowstatus, string[] paraheaderlist, string[] paracolwidth, string[] paracolformat)
        //{
        //    try
        //    {
        //        InitializeComponent();
        //        colShowStatus = parashowstatus;
        //        colHeaderList = paraheaderlist;
        //        colWidthList = paracolwidth;
        //        colFormatList = paracolformat;
        //        this.MaximizeBox = false;
        //        this.StartPosition = FormStartPosition.CenterScreen;
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
        //public frmGrdFormat(UltraGrid CallerGrid)
        //{
        //    try
        //    {
        //        InitializeComponent();
        //        grid = CallerGrid;
        //        this.MaximizeBox = false;
        //        this.StartPosition = FormStartPosition.CenterScreen;
        //        GetCurrentGridSettings();
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

        public frmGrdFormat(UltraGrid CallerGrid, string formSettingID)
        {
            try
            {
                InitializeComponent();
                grid = CallerGrid;
                this.FormSettingID = formSettingID;
                this.MaximizeBox = false;
                this.StartPosition = FormStartPosition.CenterScreen;
                GetCurrentGridSettings();
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

        // Form Events        
        private void frmGrdFormat_Load(object sender, EventArgs e)
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;
            try
            {
                tagrdFormatGrid.SelectionDrag += new CancelEventHandler(GlobalUI.Grid_SelectionDrag);
                tagrdFormatGrid.DragOver += new DragEventHandler(GlobalUI.Grid_DragOver);
                tagrdFormatGrid.DragDrop += new DragEventHandler(GlobalUI.Grid_DragDrop);
                                

                dtformatgrd = new DataTable();
                //para to dt
                dtformatgrd.Columns.Add("EverHide");
                dtformatgrd.Columns.Add("Show", typeof(bool));
                dtformatgrd.Columns.Add("Caption");
                dtformatgrd.Columns.Add("key");
                dtformatgrd.Columns.Add("Format", typeof(int));
                dtformatgrd.Columns.Add("TabStop", typeof(bool));
                dtformatgrd.Columns.Add("Width", typeof(double));
                dtformatgrd.Columns.Add("GrdColumnKey", typeof(string));
                dtformatgrd.Columns["Width"].Caption = "Width (cm)";
                dtformatgrd.Columns["EverHide"].DefaultValue = false;
                dtformatgrd.PrimaryKey = new DataColumn[] { dtformatgrd.Columns["key"] };


                for (int i = 0; i < colHeaderList.Length; i++)
                {
                    DataRow dr = dtformatgrd.NewRow();
                    dr["Show"] = Convert.ToBoolean(colShowStatus[i].ToString());
                    dr["Key"] = i.ToString();
                    dr["Format"] = Convert.ToInt32(colFormatList[i]);
                    dr["Caption"] = colHeaderList[i];
                    dr["TabStop"] = colTabStop[i];
                    //Convert pixel width to centimeter and assign ; Assuming that 2.54 cm per inch and 96 pixels per inch //some think 72 pixels per inch
                    double centimeter = (Convert.ToDouble(colWidthList[i]) * 2.54) / 96;
                    dr["Width"] = Decimal.Round(Convert.ToDecimal(centimeter.ToString()), 2).ToString();
                    dtformatgrd.Rows.Add(dr);
                }

                for (int i = 0; i < colHeaderList.Length; i++)
                {
                    for (int j = 0; j < grid.DisplayLayout.Bands[0].Columns.Count; j++)
                    {
                        if (grid.DisplayLayout.Bands[0].Columns[i].Header.VisiblePosition == Convert.ToInt32(colFormatList[i]))
                            dtformatgrd.Rows[i]["GrdColumnKey"] = grid.DisplayLayout.Bands[0].Columns[i].Key;
                    }
                }

                SYSFormSettingIDDet formSettingDet = SYSFormSettingIDDet.Get(FormSettingID);
                                
                IEnumerable<DataRow> colHideFilter = from formatRow in dtformatgrd.AsEnumerable()
                                                     join row in formSettingDet.AsEnumerable() on formatRow.Field<string>("GrdColumnKey") equals row.Field<string>("ColFldNm")
                                                     where row.Field<bool>("ColHide") == true
                                                     select formatRow;

                foreach (DataRow dr in colHideFilter)
                {
                    dr["EverHide"] = true;
                }
                dtformatgrd.AcceptChanges();

                //bind to grd
                tagrdFormatGrid.DataSource = dtformatgrd;
                
                //Set current row height                
                GridRowHeightCM.Text = rowHeightCM.ToString();

                tagrdFormatGrid.DisplayLayout.Bands[0].Columns["Caption"].Width = 200;
                tagrdFormatGrid.DisplayLayout.Bands[0].SortedColumns.Add("Format", false);
                tagrdFormatGrid.Refresh();

                ReBindGrid();

                tagrdFormatGrid.DisplayLayout.Bands[0].Columns["Key"].Hidden = true;
                tagrdFormatGrid.DisplayLayout.Bands[0].Columns["Format"].Hidden = true;
                tagrdFormatGrid.DisplayLayout.Bands[0].Columns["EverHide"].Hidden = true;

                GridKeyActionMapping KeyActEnter = new GridKeyActionMapping(Keys.Enter, UltraGridAction.NextCellByTab, UltraGridState.Cell, UltraGridState.Cell, SpecialKeys.All, SpecialKeys.Alt);
                tagrdFormatGrid.KeyActionMappings.Add(KeyActEnter);

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
                tagrdFormatGrid.DisplayLayout.Override.HeaderAppearance = appearence_Header;

                //Row
                Infragistics.Win.Appearance appearence_Row = new Infragistics.Win.Appearance();
                appearence_Row.FontData.Name = "Calibri";
                appearence_Row.FontData.SizeInPoints = 11F;
                appearence_Row.ForeColor = System.Drawing.Color.Black;
                appearence_Row.TextHAlignAsString = "LEFT";
                appearence_Row.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Row.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdFormatGrid.DisplayLayout.Override.RowAppearance = appearence_Row;

                //Cell
                Infragistics.Win.Appearance appearence_Cell = new Infragistics.Win.Appearance();
                appearence_Cell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Cell.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdFormatGrid.DisplayLayout.Override.CellAppearance = appearence_Cell;

                //Row Selector
                Infragistics.Win.Appearance appearence_RowSelector = new Infragistics.Win.Appearance();
                appearence_RowSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_RowSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdFormatGrid.DisplayLayout.Override.RowSelectorAppearance = appearence_RowSelector;

                //Appearence
                Infragistics.Win.Appearance appearence = new Infragistics.Win.Appearance();
                appearence.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdFormatGrid.DisplayLayout.Appearance = appearence;

                //Row Header Selector
                Infragistics.Win.Appearance appearence_RowHeaderSelector = new Infragistics.Win.Appearance();
                appearence_RowHeaderSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_RowHeaderSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdFormatGrid.DisplayLayout.Override.RowSelectorHeaderAppearance = appearence_RowHeaderSelector;


                tagrdFormatGrid.DisplayLayout.AutoFitStyle = AutoFitStyle.None;
                tagrdFormatGrid.TextRenderingMode = TextRenderingMode.GDI;
            }
            catch (TAException tex)
            {
                Error(tex, true); //Custom Message
            }
            catch (Exception ex)
            {
                Error(ex, true); //System Message
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void frmGrdFormat_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (!GFunc.IsNE(dtformatgrd))
                    this.dtformatgrd.Dispose();
            }
            catch (TAException tex)
            {
                Error(tex, true); //System Message
            }
            catch (Exception ex)
            {
                Error(ex, true); //Custom Message
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void frmGrdFormat_KeyDown(object sender, KeyEventArgs e)
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
        private void frmGrdFormat_Shown(object sender, EventArgs e)
        {
            if (formClose)
            {
                this.Close();
            }
            else
            {
                tagrdFormatGrid.Focus();
            }
        }        

        // Click Events
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.formClose = true;
            this.Close();
        }
        private void tsbSave_Click(object sender, EventArgs e)
        {
            try
            {
                int blankCount = 0, showCount = 0;
                if (tagrdFormatGrid.ActiveRow != null)
                    tagrdFormatGrid.ActiveRow.Update();
                int i = 0;

                //Grid's Data Setting Save to each string array 
                //via looping the grid, headerlist,width,format(position)
                //Set three clears

                for (int j = 0; j < colHeaderList.Length; j++)
                {
                    colHeaderList[j] = colWidthList[j] = colFormatList[j] = "";
                    //tagrdFormatGrid.Rows[i].Cells["Format"].Value = i.ToString();
                }

                foreach (UltraGridRow ugr in tagrdFormatGrid.Rows)
                {
                    if (tagrdFormatGrid.Rows[i].Cells["caption"].Value.ToString() == "Task Completed")
                    {
                    }

                    int index = Convert.ToInt32(tagrdFormatGrid.Rows[i].Cells["key"].Value);
                    colHeaderList[index] = tagrdFormatGrid.Rows[i].Cells["caption"].Value.ToString();

                    if (Convert.ToBoolean(tagrdFormatGrid.Rows[i].Cells["Show"].Value) == false) //Check Show Column Value
                    {
                        colWidthList[index] = "0";
                        blankCount += 1;
                    }
                    else
                    {

                        showCount += 1;

                        //if less than zero , set 1 in default set 2.54 cm
                        if (Convert.ToDouble(tagrdFormatGrid.Rows[i].Cells["Width"].Value) < 0)
                        {
                            tagrdFormatGrid.Rows[i].Cells["Width"].Value = 2.54;
                        }
                        //colWidthList[index] = tagrdFormatGrid.Rows[i].Cells["Width"].Value.ToString();
                        //Convert Back to Pixel from Centimeter  ;  Assuming that  2.54 cm per inch and 96 pixel per inch                    
                        double pixel = (96 * Convert.ToDouble(tagrdFormatGrid.Rows[i].Cells["Width"].Value)) / 2.54;
                        colWidthList[index] = Convert.ToInt32(pixel).ToString();
                    }
                    colFormatList[index] = i.ToString();
                    colTabStop[index] = Convert.ToBoolean(tagrdFormatGrid.Rows[i].Cells["TabStop"].Value);
                    i++;

                }
                rowHeightCM = Convert.ToDouble( GridRowHeightCM.Text);
                GlobalUI.Grid_Format((TAUtil.TAGridEditor)grid, colHeaderList, colWidthList, colFormatList, colTabStop, "", false, rowHeightCM);
                rowHeightCM = Convert.ToDouble(GridRowHeightCM.Text);
                GlobalUI.SaveGrid(grid, FormSettingID,rowHeightCM);                
                this.Close();
            }
            catch (TAException tex)
            {
                Error(tex, true); //Custom Message
            }
            catch (Exception ex)
            {
                Error(ex, true); //System Message
            }
        }
        private void tsbSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (UltraGridRow row in tagrdFormatGrid.Rows)
                {
                    if (row.Hidden == false && row.Cells["Caption"].Value.ToString().Contains("-99") == false)
                    {
                        row.Cells["Show"].Value = true;
                        row.Update();
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
        private void tsbUnselectAll_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (UltraGridRow row in tagrdFormatGrid.Rows)
                {
                    if (row.Hidden == false)
                    {
                        row.Cells["Show"].Value = false;
                        row.Update();
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
        private void tsbSelectHighlighted_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (UltraGridRow row in tagrdFormatGrid.Selected.Rows)
                {
                    row.Cells["Show"].Value = true;
                    row.Update();
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
        private void tsbUnselectHighlighted_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (UltraGridRow row in tagrdFormatGrid.Selected.Rows)
                {
                    row.Cells["Show"].Value = false;
                    row.Update();
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
        private void tsbDisplayShownColumn_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (UltraGridRow row in tagrdFormatGrid.Rows)
                {
                    for (int i = 0; i < colShowStatus.Length; i++)
                    {
                        if (row.Cells["Format"].Value.ToString() == colFormatList[i] && (Convert.ToBoolean(row.Cells["Show"].Value) == false)) // Previously the style is based on DataGrid like (colShowStatus[i] == false) , Now base on FormatGrid like ((Convert.ToBoolean(row.Cells["Show"].Value) == false)
                            row.Hidden = true;
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
        private void tsbDisplayAll_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (UltraGridRow row in tagrdFormatGrid.Rows)
                {
                    
                    if (GFunc.NEBool(row.Cells["EverHide"].Value, false) == false || showAllCol == true )
                        row.Hidden = false;                
                    
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
        private void tsbTest_Click(object sender, EventArgs e)
        {
            string pswd = string.Empty;
            try
            {
                if (showAllCol == false && GlobalUI.PasswordInputBox("Developer Mode", "Enter your password to show hidden columns", ref pswd) == DialogResult.OK)
                {
                    if (pswd == "techace")
                    {
                        showAllCol = true; 
                        ReBindGrid();
                    }
                }
                else if (showAllCol && GlobalUI.PasswordInputBox("Developer Mode", "Enter your password to hide hidden columns", ref pswd) == DialogResult.OK)
                {
                    if (pswd == "techace")
                    {
                        showAllCol = false;
                        ReBindGrid();
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
            

        }//CodeCompleted

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

        //Grid Events
        private void tagrdFormatGrid_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column == tagrdFormatGrid.DisplayLayout.Bands[0].Columns["Show"])
                {
                    if (GFunc.NEBool(e.Cell.Value, false) == true)
                        e.Cell.Row.Cells["Width"].Value = 2;
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
                            GlobalUI.ItemNotInList(grd.ActiveCell, null, 0);// ItemNotInList


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
                    if (formClose)
                        throw new TAException("Please enter valid date.");
                    else
                        MsgBox.Show("Please enter valid date.");
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
        private void tagrdFormatGrid_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column == tagrdFormatGrid.DisplayLayout.Bands[0].Columns["Show"] || e.Cell.Column == tagrdFormatGrid.DisplayLayout.Bands[0].Columns["TabStop"])
                    tagrdFormatGrid.ActiveRow.Update();
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

        //Functions
        private void GetCurrentGridSettings()
        {
            try
            {
                int gridColumnCount = grid.DisplayLayout.Bands[0].Columns.Count;

                colTabStop = new bool[gridColumnCount];
                colShowStatus = new bool[gridColumnCount];
                colWidthList = new string[gridColumnCount];
                colHeaderList = new string[gridColumnCount];
                colFormatList = new string[gridColumnCount];//Currently do not use this field;Just for later Column sorting

                //Loop the grid and retrieve the settings of currently user edited
                for (int i = 0; i < gridColumnCount; i++)
                {
                    if (GFunc.CompareString(grid.DisplayLayout.Bands[0].Columns[i].Key, "ItmTaxGrpAmtL"))
                    {
                    }
                    if (grid.DisplayLayout.Bands[0].Columns[i].Hidden == true)
                    {
                        colShowStatus[i] = false;
                    }
                    else
                    {
                        colShowStatus[i] = true;
                    }

                    colHeaderList[i] = grid.DisplayLayout.Bands[0].Columns[i].Header.Caption.ToString();
                    colWidthList[i] = grid.DisplayLayout.Bands[0].Columns[i].Width.ToString();
                    colFormatList[i] = grid.DisplayLayout.Bands[0].Columns[i].Header.VisiblePosition.ToString();
                    colTabStop[i] = grid.DisplayLayout.Bands[0].Columns[i].TabStop;
                }
                
                double centimeter = Math.Round((Convert.ToDouble(grid.Rows[0].Height) * 2.54) / 96,2,MidpointRounding.AwayFromZero);
                rowHeightCM = centimeter;

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
        private void ReBindGrid()
        {
            //GridFilterToDefaultView

            if (!showAllCol)
                foreach (UltraGridRow row in tagrdFormatGrid.Rows)
                {
                    if (GFunc.NEBool(row.Cells["EverHide"].Value, false) == true)
                        row.Hidden = true;
                }
            else
                foreach (UltraGridRow row in tagrdFormatGrid.Rows)
                {
                     row.Hidden = false;
                }
        }

        //Grid Events
        //private void tagrdFormatGrid_SelectionDrag(object sender, CancelEventArgs e)
        //{
        //    try
        //    {
        //        tagrdFormatGrid.DoDragDrop(tagrdFormatGrid.Selected.Rows, DragDropEffects.Move);
        //    }
        //    catch (TAException tex)
        //    {
        //        Error(tex, true); //Custom Message
        //    }
        //    catch (Exception ex)
        //    {
        //        Error(ex, true); //System Message
        //    }
        //}
        //private void tagrdFormatGrid_DragOver(object sender, DragEventArgs e)
        //{
        //    try
        //    {
        //        e.Effect = DragDropEffects.Move;
        //        UltraGrid grid = sender as UltraGrid;
        //        Point pointInGridCoords = grid.PointToClient(new Point(e.X, e.Y));
        //        if (pointInGridCoords.Y < 20)
        //        {
        //            // Scroll up.
        //            this.tagrdFormatGrid.ActiveRowScrollRegion.Scroll(RowScrollAction.LineUp);                  
        //        }
        //        else if (pointInGridCoords.Y > grid.Height - 20)
        //        {
        //            // Scroll down.
        //            this.tagrdFormatGrid.ActiveRowScrollRegion.Scroll(RowScrollAction.LineDown);                   
        //        }
        //    }
        //    catch (TAException tex)
        //    {
        //        Error(tex, true); //Custom Message
        //    }
        //    catch (Exception ex)
        //    {
        //        Error(ex, true); //System Message
        //    }
        //}
        //private void tagrdFormatGrid_DragDrop(object sender, DragEventArgs e)
        //{
        //    int dropIndex;

        //    try
        //    {
        //        // Get the position on the grid where the dragged row(s) are to be dropped.
        //        //get the grid coordinates of the row (the drop zone)
        //        UIElement uieOver = tagrdFormatGrid.DisplayLayout.UIElement.ElementFromPoint(tagrdFormatGrid.PointToClient(new Point(e.X, e.Y)));

        //        //get the row that is the drop zone/or where the dragged row is to be dropped
        //        UltraGridRow ugrOver = uieOver.GetContext(typeof(UltraGridRow), true) as UltraGridRow;
        //        if (ugrOver != null)
        //        {
        //            dropIndex = ugrOver.Index;    //index/position of drop zone in grid

        //            //get the dragged row(s)which are to be dragged to another position in the grid
        //            SelectedRowsCollection SelRows = (SelectedRowsCollection)e.Data.GetData(typeof(SelectedRowsCollection)) as SelectedRowsCollection;

        //            if (dropIndex<SelRows[0].Index)//scroll up
        //            {
        //                ////get the count of selected rows and drop each starting at the dropIndex                  
        //                for (int i = SelRows.Count - 1; i >= 0; i--)
        //                {
        //                    tagrdFormatGrid.Rows.Move(SelRows[i], dropIndex);
        //                }
        //            }
        //            else 
        //            {
        //                ////get the count of selected rows and drop each starting at the dropIndex                  
        //                for (int i = 0; i <= SelRows.Count - 1; i++)
        //                {
        //                    tagrdFormatGrid.Rows.Move(SelRows[i], dropIndex);
        //                }
        //            }
        //        }
        //    }
        //    catch (TAException tex)
        //    {
        //        Error(tex, true); //Custom Message
        //    }
        //    catch (Exception ex)
        //    {
        //        Error(ex, true); //System Message
        //    }
        //}
    }
}