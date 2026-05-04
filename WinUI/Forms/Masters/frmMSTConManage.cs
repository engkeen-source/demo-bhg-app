using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOLib;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using System.Collections;
using System.IO;
using TAUtil;
using System.Data.SqlClient;

namespace WinUI
{
    public partial class frmMSTConManage : Form
    {
        #region Member Variables, Properties, Constructors and Destructors
        private SYSRep _sysRep;
        private BOLib.MSTConManageFactory objFactory = null;
        private WinUI.ReportFactory objRFactory = null;      
        private string msgID = string.Empty;
        private string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        private bool isSave = false;
        private int ConKeyRemark = 0;
        private bool saveClick = false;
        private bool Watch = false;       
        private string ecellRemark = "";
        private int ecellValueCust = 0;
        private List<Int32> listCustomer = new List<Int32>();
        private List<Int32> listFollowUpDate = new List<Int32>();
        private List<Int32> listWatch = new List<Int32>();
        public SYSRep ObjSYSRep
        {
            get { return _sysRep; }
            set { _sysRep = value; }
        }

        private int Opt = 1;
        private int DueCalu = 20;
        private int CCBT = 10;
        private string DateV = DateTime.Now.ToString("dd-MMM-yyyy");
        DataSet dsReportHeader = null;
        DataSet dsReportDetail = null;

        /* added by YST */
        DataTable dtFollowUpList = null;
        string listID = "";

        #endregion Member
        public frmMSTConManage()
        {
            InitializeComponent();
        }
        private void frmMSTConManage_Load(object sender, EventArgs e)
        {
            try
            {   // Waiting Cursor
                this.Cursor = Cursors.WaitCursor;
                bool bRO;

                // Call Initialization
                this.objFactory = new BOLib.MSTConManageFactory();
                if (objFactory.IsLock)
                {
                    tsbAutoSave.Enabled = false;
                    tsbRefresh.Enabled = false;
                    tsbSaveCustomerType.Enabled = false;
                    tsbSaveFUpDate.Enabled = false;
                    tsbSaveRemark.Enabled = false;
                    btnRefresh.Enabled = false;
                }
                else
                {
                    tsbAutoSave.Enabled = true;
                    tsbRefresh.Enabled = true;
                    tsbSaveCustomerType.Enabled = true;
                    tsbSaveFUpDate.Enabled = true;
                    tsbSaveRemark.Enabled = true;
                    btnRefresh.Enabled = true;

                }
                if (!SECPermUtility.Any(MSTConManageFactory.constPermID, out bRO, false))
                {
                    formClose = true;
                    return;
                }
                DueCal.Value = 20;
                DueCalDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");                
                FollowUpDate.Value = DateTime.Now.ToString("dd MMM yyyy");
                this.RefreshCustomers();

                /* added by YST on 2021/11/09 */
                FollowUpListInitialize();
                FollowUpListDataBinding();

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
        private void RefreshCustomers()
        {
            string ConName = "";
            string DueCalDate = DateTime.Now.ToString("dd-MMM-yyyy");
            frmMain.gfrmMain.SetNotifyStatus("Loading.... Please wait.");
            if (objFactory.GetEdit(1, 20, 10, DueCalDate, ConName))
            {
                tagrdCustomer.DataSource = objFactory.ObjConmanageDT;
                tagrdCustomer.DataBind();
            }
            Opt = 1;
            DueCalu = 20;
            CCBT = 10;
            DateV = DueCalDate;

            //Format all grids and filter

            //Set ContextMenu
            GlobalUI.FormGrids_Set(this, (int)objFactory.CodeKey, out ContextMenuSetting);
            ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objFactory.CodeKey);

            GlobalUI.Combos_Fill(this, (int)objFactory.CodeKey);
            FormatCustomerGrid();
            if (tagrdCustomer.Rows.VisibleRowCount > 0)
            {
                tagrdCustomer.Rows.FirstVisibleCardRow.Cells["DocConID"].Selected = true;
                tagrdCustomer.Rows.FirstVisibleCardRow.Cells["DocConID"].Activate();
            }
            Format_tagrdCustomer();
            //FormatWatchList();
            frmMain.gfrmMain.SetNormalStaus("Ready");
        }
        private void ClearLock()
        {
            SqlConnection cn = new SqlConnection(AppInfor.CurrentDBConnectionStr);
            cn.Open();
            GFunc.ExecuteNonQuery(cn, "Delete From Sys_Lock Where UserKey= "+AppInfor.CurrentUserKey +" and CodeKey = " + 22220 + "");
            cn.Close();
        }
        private bool CheckUser()
        {
            try
            {
                //int UserKey = Int32.Parse(GFunc.ExecuteScalar("Select OpUserKey from SYS_Option  where opid = 'EmailAccount' and OpUserKey 	in (Select userkey From MST_SalesRep Where Emkey in (Select opvalue from Sys_Option where opid = 'DefaultFinalAprover1'))"));
                //if (UserKey == AppInfor.CurrentUserKey) return true; else return false;

                return true;
            }
            catch
            {
                return false;
            }
        }
        private void FormatRemarkGridReadOnly()
        {
            try
            {
                this.tagrdRemark.DisplayLayout.Override.ActiveCellAppearance.BackColor = Color.SkyBlue;
                tagrdRemark.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True;
                tagrdRemark.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.CellSelect;
                tagrdRemark.DisplayLayout.Bands[0].Columns["CreateDate"].CellActivation = Activation.Disabled;
                tagrdRemark.DisplayLayout.Bands[0].Columns["LastModifiedDate"].CellActivation = Activation.Disabled;
                tagrdRemark.DisplayLayout.Bands[0].Columns["ConNm"].CellActivation = Activation.Disabled;
                tagrdRemark.DisplayLayout.Bands[0].Columns["CreateUserName"].CellActivation = Activation.Disabled;
                tagrdRemark.DisplayLayout.Bands[0].Columns["LastModifiedUserName"].CellActivation = Activation.Disabled;
                tagrdRemark.DisplayLayout.Bands[0].Columns["Remark"].CellClickAction = CellClickAction.EditAndSelectText;
                tagrdRemark.DisplayLayout.Bands[0].Columns["RemarkDesc"].CellClickAction = CellClickAction.EditAndSelectText;
                tagrdRemark.DisplayLayout.Bands[0].Columns["RemarkType"].CellClickAction = CellClickAction.EditAndSelectText;
                tagrdRemark.DisplayLayout.Bands[0].Columns["ActionClose"].CellClickAction = CellClickAction.EditAndSelectText;
                tagrdRemark.ActiveRowScrollRegion.Scrollbar = Scrollbar.Show;
                this.tagrdRemark.Refresh();
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
        private void FormatCustomerGrid()
        {
            try
            {
                string listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "0GF", tagrdCustomer.Name);
                GlobalUI.Grid_Format(tagrdCustomer, listID, false, false);                
                //Set Grid Controls and format
                GlobalUI.GridControl_Set(tagrdCustomer, listID, (int)this.objFactory.CodeKey);
                //this.tagrdCustomer.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False; 
                this.tagrdCustomer.DisplayLayout.Override.ActiveCellAppearance.BackColor = Color.SkyBlue;
                //tagrdCustomer.DisplayLayout.Override.RowSelectors = DefaultableBoolean.False;
                //tagrdCustomer.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.CellSelect; 
                tagrdCustomer.DisplayLayout.Bands[0].Columns["LastModifiedDate"].CellActivation = Activation.Disabled;
                tagrdCustomer.DisplayLayout.Override.RowSelectors = DefaultableBoolean.False;
                tagrdCustomer.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.CellSelect;
                tagrdCustomer.DisplayLayout.Bands[0].Columns["Watch"].CellClickAction = CellClickAction.EditAndSelectText;
                tagrdCustomer.DisplayLayout.Bands[0].Columns["FollowUpDate"].CellClickAction = CellClickAction.EditAndSelectText;

                tagrdCustomer.DisplayLayout.Bands[0].Columns["T"].CellAppearance.TextHAlign = HAlign.Right;
                tagrdCustomer.DisplayLayout.Bands[0].Columns["B"].CellAppearance.TextHAlign = HAlign.Right;
                tagrdCustomer.DisplayLayout.Bands[0].Columns["1"].CellAppearance.TextHAlign = HAlign.Right;
                tagrdCustomer.DisplayLayout.Bands[0].Columns["2"].CellAppearance.TextHAlign = HAlign.Right;
                tagrdCustomer.DisplayLayout.Bands[0].Columns["3"].CellAppearance.TextHAlign = HAlign.Right;
                tagrdCustomer.DisplayLayout.Bands[0].Columns["4"].CellAppearance.TextHAlign = HAlign.Right;
                tagrdCustomer.DisplayLayout.Bands[0].Columns["5"].CellAppearance.TextHAlign = HAlign.Right;
                tagrdCustomer.DisplayLayout.Bands[0].Columns["6"].CellAppearance.TextHAlign = HAlign.Right;
                tagrdCustomer.DisplayLayout.Bands[0].Columns["7"].CellAppearance.TextHAlign = HAlign.Right;
                tagrdCustomer.DisplayLayout.Bands[0].Columns["8"].CellAppearance.TextHAlign = HAlign.Right;
                tagrdCustomer.DisplayLayout.Bands[0].Columns["9"].CellAppearance.TextHAlign = HAlign.Right;
                tagrdCustomer.DisplayLayout.Bands[0].Columns["10"].CellAppearance.TextHAlign = HAlign.Right;
                tagrdCustomer.DisplayLayout.Bands[0].Columns["11"].CellAppearance.TextHAlign = HAlign.Right;
                tagrdCustomer.DisplayLayout.Bands[0].Columns["12"].CellAppearance.TextHAlign = HAlign.Right;
                tagrdCustomer.DisplayLayout.Bands[0].Columns["MthPayAmt"].CellAppearance.TextHAlign = HAlign.Right;
                tagrdCustomer.DisplayLayout.Bands[0].Columns["CYR"].CellAppearance.TextHAlign = HAlign.Right; 
                tagrdCustomer.DisplayLayout.Bands[0].Columns["0"].CellAppearance.TextHAlign = HAlign.Right;
                tagrdCustomer.DisplayLayout.Bands[0].Columns["DocCreditLimit"].CellAppearance.TextHAlign = HAlign.Right;

                var now = DateTime.Now;
                var firstDayCurrentMonth = new DateTime(now.Year, now.Month, 1);

                var lastDayLastMonth = firstDayCurrentMonth.AddDays(-1);

                //DateTime lastDay = new DateTime(DueCalDate.DateValue.Value.Year, DueCalDate.DateValue.Value.Month + 1, 1).AddDays(-1);
                DueCalDate.Value = lastDayLastMonth.ToString("dd MMM yyyy");

                tagrdCustomer.DisplayLayout.Bands[0].Columns["0"].Header.Caption = DueCalDate.DateValue.Value.ToString("dd MMM yyyy") + " (30 Days)";
                tagrdCustomer.DisplayLayout.Bands[0].Columns["1"].Header.Caption = DueCalDate.DateValue.Value.AddMonths(-1).ToString("dd MMM yyyy") + " (60 Days)";
                tagrdCustomer.DisplayLayout.Bands[0].Columns["2"].Header.Caption = DueCalDate.DateValue.Value.AddMonths(-2).ToString("dd MMM yyyy") + " (90 Days)";
                tagrdCustomer.DisplayLayout.Bands[0].Columns["3"].Header.Caption = DueCalDate.DateValue.Value.AddMonths(-3).ToString("dd MMM yyyy") + " (120 Days)";
                tagrdCustomer.DisplayLayout.Bands[0].Columns["4"].Header.Caption = DueCalDate.DateValue.Value.AddMonths(-4).ToString("dd MMM yyyy") + " (150 Days)";
                tagrdCustomer.DisplayLayout.Bands[0].Columns["5"].Header.Caption = DueCalDate.DateValue.Value.AddMonths(-5).ToString("dd MMM yyyy") + " (180 Days)";
                tagrdCustomer.DisplayLayout.Bands[0].Columns["6"].Header.Caption = DueCalDate.DateValue.Value.AddMonths(-6).ToString("dd MMM yyyy") + " (210 Days)";
                tagrdCustomer.DisplayLayout.Bands[0].Columns["7"].Header.Caption = DueCalDate.DateValue.Value.AddMonths(-7).ToString("dd MMM yyyy") + " (240 Days)";
                tagrdCustomer.DisplayLayout.Bands[0].Columns["8"].Header.Caption = DueCalDate.DateValue.Value.AddMonths(-8).ToString("dd MMM yyyy") + " (270 Days)";
                tagrdCustomer.DisplayLayout.Bands[0].Columns["9"].Header.Caption = DueCalDate.DateValue.Value.AddMonths(-9).ToString("dd MMM yyyy") + " (300 Days)";
                tagrdCustomer.DisplayLayout.Bands[0].Columns["10"].Header.Caption = DueCalDate.DateValue.Value.AddMonths(-10).ToString("dd MMM yyyy") + " (330 Days)";
                tagrdCustomer.DisplayLayout.Bands[0].Columns["11"].Header.Caption = DueCalDate.DateValue.Value.AddMonths(-11).ToString("dd MMM yyyy") + " (360 Days)";
                tagrdCustomer.DisplayLayout.Bands[0].Columns["12"].Header.Caption = "< " + DueCalDate.DateValue.Value.AddMonths(-12).ToString("dd MMM yyyy") + " (>360 Days)";

                if (CheckUser())
                {
                    tagrdCustomer.DisplayLayout.Bands[0].Columns["ActiveWithProblem"].CellClickAction = CellClickAction.EditAndSelectText;
                    tagrdCustomer.DisplayLayout.Bands[0].Columns["COOApprovalRequired"].CellClickAction = CellClickAction.EditAndSelectText;
                }

                if (tagrdCustomer.Rows.VisibleRowCount > 0)
                {
                    if (tagrdCustomer.Rows.FirstVisibleCardRow != null)
                    {
                        UltraGridColumn VisibleCol = tagrdCustomer.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                        tagrdCustomer.DisplayLayout.ActiveRow.Cells[VisibleCol.Key].Selected = true;
                    }
                }
                //tagrdCustomer.ActiveRowScrollRegion.Scrollbar = Scrollbar.Show;
                this.tagrdCustomer.Refresh();
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
                if (this.objFactory.IsDirty && ( listCustomer.Count != 0 || listWatch.Count !=0 || listFollowUpDate.Count != 0))
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
                        if (btnSelect == GEnum.MsgBoxButton.I_Dont_Know || btnSelect == GEnum.MsgBoxButton.Discard_Changes)
                        {
                            return false;
                        }
                        // Yes, I want to save
                        else if (btnSelect == GEnum.MsgBoxButton.Save_Changes)
                        {
                            tsbSaveCustomerType_Click(null, null);
                            if (msgID != string.Empty)
                                return false;
                        }
                    }
                    else
                    {
                        tsbSaveCustomerType_Click(null, null);
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
                listCustomer.Clear();
                listWatch.Clear();
                listFollowUpDate.Clear();                
                // Default Cursor                
                this.Cursor = Cursors.Default;

            }
            return false;
        }
        private bool SaveChangesRemark()
        {

            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;
            bool processOk = true;
            try
            {
                // Check Form Validation
                this.Validate();


                // Check Factory Object is Dirty ...
                if (ecellRemark != "")
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
                        if (btnSelect == GEnum.MsgBoxButton.I_Dont_Know || btnSelect == GEnum.MsgBoxButton.Discard_Changes)
                        {
                            if (formClose) return false;

                        }
                        // Yes, I want to save
                        else if (btnSelect == GEnum.MsgBoxButton.Save_Changes)
                        {
                            tsbSaveRemark_Click(null, null);
                            if (msgID != string.Empty)
                                return false;
                        }
                    }
                    else
                    {
                        tsbSaveRemark_Click(null, null);
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
                
                if (!formClose)
                    // Default Cursor
                    this.Cursor = Cursors.Default;
                

            }
            return false;
        }
        private bool SaveChangesWatch()
        {

            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;
            bool processOk = true;
            try
            {
                // Check Form Validation
                this.Validate();

                // Check Factory Object is Dirty ...
                if (objFactory.IsDirtyWatch )
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
                        if (btnSelect == GEnum.MsgBoxButton.I_Dont_Know || btnSelect == GEnum.MsgBoxButton.Discard_Changes)
                        {
                            if (formClose) return false;

                        }
                        // Yes, I want to save
                        else if (btnSelect == GEnum.MsgBoxButton.Save_Changes)
                        {
                            tsbSaveFUpDate_Click(null, null);
                            if (msgID != string.Empty)
                                return false;
                        }
                    }
                    else
                    {
                        tsbSaveFUpDate_Click(null, null);
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
                listWatch.Clear();
                if (!formClose)
                    // Default Cursor
                    this.Cursor = Cursors.Default;

            }
            return false;
        }
        internal void Format_tagrdCustomer()
        {
            if (tagrdCustomer.Rows.Count > 0)
            {
                for (int i = 0; i < tagrdCustomer.Rows.Count; i++)
                {
                    if (tagrdCustomer.Rows[i].Cells["ActiveWithProblem"].Value.ToString().ToUpper() == "TRUE")
                        tagrdCustomer.Rows[i].Appearance.BackColor = Color.Red;
                    if (tagrdCustomer.Rows[i].Cells["COOApprovalRequired"].Value.ToString().ToUpper() == "TRUE")
                        tagrdCustomer.Rows[i].Appearance.BackColor = Color.Orange;
                    if (tagrdCustomer.Rows[i].Cells["ActiveWithProblem"].Value.ToString().ToUpper() == "FALSE" 
                        && tagrdCustomer.Rows[i].Cells["COOApprovalRequired"].Value.ToString().ToUpper() == "FALSE")
                        tagrdCustomer.Rows[i].Appearance.BackColor = Color.White;
                }
            }
        }        
        private void CustList_CustomUpdate(object sender, CancelEventArgs e)
        {
            this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
            int ConKey = 0;
            if (formClose)
                return;
            if (!GFunc.IsNEZ(CustList.Value))
                Int32.TryParse(CustList.Value.ToString(), out ConKey);

            if (this.tagrdCustomer.DisplayLayout.Bands[0].Columns.Count > 0)
            {
                if (ConKey == 0)
                {
                    FilterCondition filterCondition = new FilterCondition(FilterComparisionOperator.Equals, FilterCondition.BlankCellValue);
                    this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters["DocConKey"].FilterConditions.Add(filterCondition);
                }
                else
                    this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters["DocConKey"].FilterConditions.Add(FilterComparisionOperator.Equals, ConKey);
            }

            Format_tagrdCustomer();

            if (ConKey == 0)
            {
                FilterCondition filterCondition = new FilterCondition(FilterComparisionOperator.Equals, FilterCondition.BlankCellValue);
                this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters["DocConKey"].FilterConditions.Add(filterCondition);
            }
            else
            {
                this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters["DocConKey"].FilterConditions.Add(FilterComparisionOperator.Equals, ConKey);
            }
        }
        private void LOList_CustomUpdate(object sender, CancelEventArgs e)
        {
            this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
            string LOName = string.Empty;
            if (formClose)
                return;

            //LOName = LOList.Value.ToString();
            LOName = LOList.Text.ToString();

            if (this.tagrdCustomer.DisplayLayout.Bands[0].Columns.Count > 0)
            {

                if (LOName == "" || LOName == "LOCAL/OVERSEA")
                {
                    this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                }

                else
                    this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters["CGrpID"].FilterConditions.Add(FilterComparisionOperator.Equals, LOName);
            }

            Format_tagrdCustomer();
        }
        private void tagrdCustomer_BeforeSelectChange(object sender, BeforeSelectChangeEventArgs e)
        {
            #region /* commented by YST on 2021/11/09 */
            //this.Cursor = Cursors.WaitCursor;
            //int NewConKey = 0;
            //int ConKey = 0;

            //if (e.NewSelections.Cells.Count > 0)
            //{
            //    NewConKey = GFunc.NEInt(e.NewSelections.Cells[0].Row.Cells["DocConKey"].Value, 0);
            //}
            //else
            //    return;

            //if (tagrdCustomer.Selected.Cells.Count > 0)
            //{
            //    ConKey = GFunc.NEInt(tagrdCustomer.Selected.Cells[0].Row.Cells["DocConKey"].Value, 0);
            //}
            //if (SaveChangesRemark())
            //{ }
            //try
            //{
            //    if (objFactory.GetEditRemark(NewConKey))
            //    {
            //        if (objFactory.ObjConRemarkDT.Rows.Count > 0)
            //        {
            //            tagrdRemark.DataSource = objFactory.ObjConRemarkDT;
            //            tagrdRemark.DataBind();
            //            FormatRemarkGrid();
            //            FormatRemarkGridReadOnly();

            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.Show(ex.Message); // System Msg
            //}
            //finally
            //{

            //    this.Cursor = Cursors.Default;
            //    if (!isSave)
            //        e.Cancel = true;
            //    isSave = false;

            //}
            #endregion

            /* modifed by YST on 2021/11/09 */
            this.Cursor = Cursors.WaitCursor;
            int NewConKey = 0;
            int ConKey = 0;

            if (e.NewSelections.Cells.Count > 0)
            {
                NewConKey = GFunc.NEInt(e.NewSelections.Cells[0].Row.Cells["DocConKey"].Value, 0);
            }
            else
            {
                this.Cursor = Cursors.Default;
                return;
            }

            if (tagrdCustomer.Selected.Cells.Count > 0)
            {
                ConKey = GFunc.NEInt(tagrdCustomer.Selected.Cells[0].Row.Cells["DocConKey"].Value, 0);
            }           
            try
            {
                SaveChangesRemark();
                if (objFactory.GetEditRemark(NewConKey))
                {
                    if (objFactory.ObjConRemarkDT.Rows.Count > 0)
                    {
                        tagrdRemark.DataSource = objFactory.ObjConRemarkDT;
                        tagrdRemark.DataBind();
                        FormatRemarkGrid();
                        FormatRemarkGridReadOnly();
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message); 
            }
            finally
            {
                this.Cursor = Cursors.Default;
                if (!isSave)
                    e.Cancel = true;
                isSave = false;
            }
        }
        private void FormatRemarkGrid()
        {
            try
            {
                string listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "0GF", tagrdRemark.Name);
                GlobalUI.Grid_Format(tagrdRemark, listID, false, false);
                //Set Grid Controls and format
                GlobalUI.GridControl_Set(tagrdRemark, listID, (int)this.objFactory.CodeKey);

                this.tagrdRemark.DisplayLayout.Override.CellMultiLine = DefaultableBoolean.True;
                tagrdRemark.DisplayLayout.Override.ActiveCellAppearance.Reset(); tagrdRemark.DisplayLayout.Override.ActiveRowAppearance.Reset();
                if (tagrdRemark.Rows.Count > 0)
                {
                    for (int i = 0; i < tagrdRemark.Rows.Count; i++)
                    {
                        tagrdRemark.Rows[i].Cells["Remark"].Activation = Activation.Disabled;
                        tagrdRemark.Rows[i].Cells["RemarkDesc"].Activation = Activation.Disabled;
                        tagrdRemark.Rows[i].Cells["RemarkType"].Activation = Activation.Disabled;
                        if (SECPermUtility.Perform(GVar.PermissionID.Add_Customer_Remark, false) == false)
                        {
                            tagrdRemark.Rows[i].Cells["Remark"].Activation = Activation.Disabled;
                            tagrdRemark.Rows[i].Cells["RemarkDesc"].Activation = Activation.Disabled;
                            tagrdRemark.Rows[i].Cells["RemarkType"].Activation = Activation.Disabled;
                        }

                        else
                        {
                            if (tagrdRemark.Rows.Count > 1)
                            {
                                if (i == tagrdRemark.Rows.Count - 1)
                                {
                                    tagrdRemark.Rows[i].Cells["Remark"].Activation = Activation.AllowEdit;
                                    tagrdRemark.Rows[i].Cells["RemarkDesc"].Activation = Activation.AllowEdit;
                                    tagrdRemark.Rows[i].Cells["RemarkType"].Activation = Activation.AllowEdit;
                                }

                            }
                            else if (tagrdRemark.Rows.Count == 1)
                            {
                                tagrdRemark.Rows[i].Cells["Remark"].Activation = Activation.AllowEdit;
                                tagrdRemark.Rows[i].Cells["RemarkDesc"].Activation = Activation.AllowEdit;
                                tagrdRemark.Rows[i].Cells["RemarkType"].Activation = Activation.AllowEdit;
                            }
                        }
                    }

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
        private void tsbRefresh_Click(object sender, EventArgs e)
        {

            string DueCalDate = DateTime.Now.ToString("dd-MMM-yyyy");
            int DueCalc = 20;
            int CCB = 10;
            int Option = 2;

            Opt = Option;
            DueCalu = DueCalc;
            CCBT = CCB;
            DateV = DueCalDate;
            string ConName = "";
            frmMain.gfrmMain.SetNotifyStatus("Loading.... Please wait.");
            if (objFactory.GetEdit(Option, DueCalc, CCB, DueCalDate, ConName))
            {
                tagrdCustomer.DataSource = objFactory.ObjConmanageDT;
                tagrdCustomer.DataBind();
            }
            FormatCustomerGrid();
            Format_tagrdCustomer();
            frmMain.gfrmMain.SetNormalStaus("Ready");
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {

            string DueCalcDate = string.Empty;
            int DueCalc = 0;
            if (DueCalDate.DateValue.Value != null)
            {
                DueCalcDate = DueCalDate.DateValue.Value.ToString("dd-MMM-yyyy");
                DueCalDate.Value = DueCalDate.DateValue.Value.ToString("dd-MMM-yyyy");
            }
            if (DueCalcDate == string.Empty)
            {
                MsgBox.Show("Date As At is required.");
                return;
            }

            DueCalc = GFunc.NEInt(DueCal.Value, 0);

            if (DueCalc == 0)
            {
                MsgBox.Show("Due Calculation is required.");
                return;
            }
            int CCB = 10;
            int Option = 3;

            Opt = Option;
            CCBT = CCB;
            DateV = DueCalcDate;
            DueCalu = DueCalc;
            string ConName = "";

            frmMain.gfrmMain.SetNotifyStatus("Loading.... Please wait.");
            if (objFactory.GetEdit(Option, DueCalc, CCB, DueCalcDate, ConName))
            {
                tagrdCustomer.DataSource = objFactory.ObjConmanageDT;
                tagrdCustomer.DataBind();
            }
            FormatCustomerGrid();
            Format_tagrdCustomer();
            frmMain.gfrmMain.SetNormalStaus("Ready");
        }
        private void frmMSTConManage_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool runProcess = false;
            this.formClose = true;            
            if (objFactory.IsDirty && (listCustomer.Count != 0 || listWatch.Count != 0 || listFollowUpDate.Count != 0)) if (SaveChanges()) { }            
            if(ecellRemark != "") if (SaveChangesRemark()) { }
            if (objFactory.IsDirtyWatch) if (SaveChangesWatch()) { }
            ClearLock();

        }
        private void frmMSTConManage_Shown(object sender, EventArgs e)
        {
            // Check Form Close State is True ...
            if (formClose)
            {
                this.Close();
            }
        }
        private void CustType_CustomUpdate(object sender, CancelEventArgs e)
        {
            this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();

            if (formClose)
                return;

            if (this.tagrdCustomer.DisplayLayout.Bands[0].Columns.Count > 0)
            {
                if (CustType.Value.ToString() == "1")
                {
                    this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                }
                else if (CustType.Value.ToString() == "2")
                {
                    this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters["ActiveWithProblem"].FilterConditions.Add(FilterComparisionOperator.Equals, true);
                }
                else this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters["COOApprovalRequired"].FilterConditions.Add(FilterComparisionOperator.Equals, true);
            }
            Format_tagrdCustomer();

        }
        private void tabDetailList_Click(object sender, EventArgs e)
        {
            //  string ConName = "";
            //    frmMain.gfrmMain.SetNotifyStatus("Loading.... Please wait.");
            //    if (objFactory.GetEdit(Opt, DueCalu, CCBT, DateV, ConName))
            //    {
            //        tagrdCustomer.DataSource = objFactory.ObjConmanageDT;
            //        tagrdCustomer.DataBind();
            //    }
            //    FormatCustomerGrid();
            //    Format_tagrdCustomer();
            //    frmMain.gfrmMain.SetNormalStaus("Ready");
            if (tagrdCustomer.Rows.Count > 0)
            {
                for (int i = 0; i < tagrdCustomer.Rows.Count; i++)
                {
                    tagrdCustomer.Rows[i].Cells["Watch"].Value = 0;
                }
            }
                    this.Cursor = Cursors.Default;
        }
        private void tsbSaveRemark_Click(object sender, EventArgs e)
        {
            if (tagrdRemark.ActiveRow != null)
                tagrdRemark.ActiveRow.Update();
            objFactory.ObjConRemarkDT.AcceptChanges();
            // Waiting cursor
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.Validate();

                bool processOk = true;
                if (objFactory != null)
                {
                    if (ecellRemark != "")
                    {
                        MstConRemark objConRemark = new MstConRemark();
                        objConRemark.ConKey = Convert.ToInt32(tagrdRemark.ActiveRow.Cells["ConKey"].Value.ToString());
                        objConRemark.Remark = tagrdRemark.ActiveRow.Cells["Remark"].Value.ToString();
                        objConRemark.RemarkDesc = tagrdRemark.ActiveRow.Cells["RemarkDesc"].Text.ToString();
                        objConRemark.RemarkType = tagrdRemark.ActiveRow.Cells["RemarkType"].Text.ToString();
                        objConRemark.ActionClose = Convert.ToBoolean(tagrdRemark.ActiveRow.Cells["ActionClose"].Value);
                        ConKeyRemark = Convert.ToInt32(tagrdRemark.ActiveRow.Cells["ConKey"].Value.ToString());
                        isSave = objFactory.SaveRemark(objConRemark, 0);

                        if (isSave)
                        {
                            if (objFactory.GetEditRemark(ConKeyRemark))
                            {
                                if (objFactory.ObjConRemarkDT.Rows.Count > 0)
                                {
                                    tagrdRemark.DataSource = objFactory.ObjConRemarkDT;
                                    tagrdRemark.DataBind();                                

                                }
                            }
                            FormatRemarkGrid();
                            FormatRemarkGridReadOnly();
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
                ecellRemark = "";
                isSave = true;
                this.Cursor = Cursors.Default;
            }
        }
        private void tsbSaveCustomerType_Click(object sender, EventArgs e)
        {
            isSave = true;
            if (tagrdCustomer.Rows.VisibleRowCount > 0)
            {
                if (tagrdCustomer.Rows.FirstVisibleCardRow != null)
                {
                    UltraGridColumn VisibleCol = tagrdCustomer.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                    tagrdCustomer.Rows.FirstVisibleCardRow.Cells[VisibleCol.Key].Selected = true;
                    //tagrdCustomers.DisplayLayout.ActiveRow.Cells[VisibleCol.Key].Selected = true;
                }
            }

            // Waiting cursor
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.Validate();

                bool processOk = true;
                if (objFactory != null)
                {
                    if (tagrdCustomer.Selected.Cells.Count > 0)
                        tagrdCustomer.Selected.Cells[0].Row.Update();

                    if (objFactory.IsDirty && (listCustomer.Count != 0 || listWatch.Count != 0 || listFollowUpDate.Count != 0))
                    {
                        MSTConManage objConManage = new MSTConManage();
                        bool chkUser = CheckUser();                        

                        int ConKey = Convert.ToInt32(tagrdCustomer.Selected.Cells[0].Row.Cells["DocConKey"].Value);

                        objConManage.ConKey = ConKey;
                        //objConManage.FollowUpDate = Convert.ToDateTime(tagrdCustomer.Selected.Cells[0].Row.Cells["FollowUpdate"].Value);
                        objConManage.UserKey = AppInfor.CurrentUserKey;
                        
                        processOk = objFactory.SaveCustomerType(listWatch,listFollowUpDate,listCustomer,chkUser);
                        if (processOk)
                        {
                            string ConName = txtConName.Text.ToString();
                            frmMain.gfrmMain.SetNotifyStatus("Loading.... Please wait.");
                            if (objFactory.GetEdit(Opt, DueCalu, CCBT, DateV, ConName))
                            {
                                tagrdCustomer.DataSource = objFactory.ObjConmanageDT;
                                tagrdCustomer.DataBind();
                            }
                            Format_tagrdCustomer();
                            FormatCustomerGrid();
                            frmMain.gfrmMain.SetNormalStaus("Ready");
                            this.Cursor = Cursors.Default;

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
                listCustomer.Clear();
                listWatch.Clear();
                listFollowUpDate.Clear();           
                
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }       
        private void Due_UnDue_CustomUpdate(object sender, CancelEventArgs e)
        {
            this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
            if (formClose)
                return;

            if (this.tagrdCustomer.DisplayLayout.Bands[0].Columns.Count > 0)
            {
                if (Due_UnDue.Value.ToString() == "1")
                {
                    this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                }
                else if (Due_UnDue.Value.ToString() == "2")
                {
                    this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters["T"].FilterConditions.Add(FilterComparisionOperator.NotEquals, 0.0000);
                }
                else this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters["T"].FilterConditions.Add(FilterComparisionOperator.Equals, 0.0000);
            }
            Format_tagrdCustomer();
        }
        private void tagrdCustomer_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
        {
            try
            {
                if (tagrdCustomer.DisplayLayout.ActiveRow.Cells["ActiveWithProblem"].Value.ToString().ToUpper() == "TRUE")
                {
                    tagrdCustomer.DisplayLayout.ActiveRow.Cells["ActiveWithProblem"].Value = true;
                }

                if (tagrdCustomer.DisplayLayout.ActiveRow.Cells["COOApprovalRequired"].Value.ToString().ToUpper() == "TRUE")
                {
                    tagrdCustomer.DisplayLayout.ActiveRow.Cells["COOApprovalRequired"].Value = true;
                }

                if (tagrdCustomer.DisplayLayout.ActiveRow.Cells["ActiveWithProblem"].Value.ToString().ToUpper() == "TRUE" && tagrdCustomer.DisplayLayout.ActiveRow.Appearance.BackColor.Equals(Color.Orange))
                {
                    tagrdCustomer.DisplayLayout.ActiveRow.Cells["COOApprovalRequired"].Value = false;
                    tagrdCustomer.DisplayLayout.ActiveRow.Cells["ActiveWithProblem"].Value = true;
                }

                if (tagrdCustomer.DisplayLayout.ActiveRow.Cells["COOApprovalRequired"].Value.ToString().ToUpper() == "TRUE" && tagrdCustomer.DisplayLayout.ActiveRow.Appearance.BackColor.Equals(Color.Red))
                {
                    tagrdCustomer.DisplayLayout.ActiveRow.Cells["COOApprovalRequired"].Value = true;
                    tagrdCustomer.DisplayLayout.ActiveRow.Cells["ActiveWithProblem"].Value = false;
                }

                

               
                if (tagrdCustomer.DisplayLayout.ActiveRow.Cells["ActiveWithProblem"].Value.ToString().ToUpper() == "TRUE")
                {
                    tagrdCustomer.DisplayLayout.ActiveRow.Appearance.BackColor = Color.Red;
                }
                if (tagrdCustomer.DisplayLayout.ActiveRow.Cells["COOApprovalRequired"].Value.ToString().ToUpper() == "TRUE")
                {
                    tagrdCustomer.DisplayLayout.ActiveRow.Appearance.BackColor = Color.Orange;
                }
                if (tagrdCustomer.DisplayLayout.ActiveRow.Cells["COOApprovalRequired"].Value.ToString().ToUpper() == "FALSE" && tagrdCustomer.DisplayLayout.ActiveRow.Cells["ActiveWithProblem"].Value.ToString().ToUpper() == "FALSE")
                {
                    tagrdCustomer.DisplayLayout.ActiveRow.Appearance.BackColor = Color.White;
                }
                if (tagrdCustomer.Rows.VisibleRowCount > 0)
                {
                    if (tagrdCustomer.Rows.FirstVisibleCardRow != null)
                    {
                        UltraGridColumn VisibleCol = tagrdCustomer.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                        tagrdCustomer.Rows.FirstVisibleCardRow.Cells[VisibleCol.Key].Selected = true;
                        //tagrdCustomers.DisplayLayout.ActiveRow.Cells[VisibleCol].Selected = true;
                    }
                }


                #region "find same id of selected customer and set same value to COOApprovalRequired, ActivewithProblems columns " added by Jane on 10-June-2024
                int ConKey = 0;
                ConKey = Convert.ToInt32(tagrdCustomer.ActiveRow.Cells["DocConKey"].Value);
                Boolean CooApproval = GFunc.NEBool(tagrdCustomer.ActiveRow.Cells["COOApprovalRequired"].Value, false);
                Boolean ActiveWithProblem = GFunc.NEBool(tagrdCustomer.ActiveRow.Cells["ActiveWithProblem"].Value, false);
                for (int i = 0; i < tagrdCustomer.Rows.Count; i++)
                {
                    if (GFunc.NEInt(tagrdCustomer.Rows[i].Cells["DocConKey"].Value, 0) == ConKey)
                    {
                        tagrdCustomer.Rows[i].Cells["COOApprovalRequired"].Value = CooApproval;
                        tagrdCustomer.Rows[i].Cells["ActiveWithProblem"].Value = ActiveWithProblem;

                        if (CooApproval == true)
                            tagrdCustomer.Rows[i].Appearance.BackColor = Color.Orange;
                        else if (ActiveWithProblem == true)
                            tagrdCustomer.Rows[i].Appearance.BackColor = Color.Red;
                        else
                            tagrdCustomer.Rows[i].Appearance.BackColor = Color.White;
                    }
                }
                #endregion                    
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message); // System Msg
            }
            finally
            {
                this.Cursor = Cursors.Default;

            }
        }
        private void tsbSaveFUpDate_Click(object sender, EventArgs e) 
        {
            isSave = true;
            // Waiting cursor
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.Validate();
                bool processOk = true;
               
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
        private void tabDetailList_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (e.Tab.Text.ToString() == "Follow Up Watch List")
            {
                frmMain.gfrmMain.SetNotifyStatus("Loading.... Please wait.");
                frmMain.gfrmMain.SetNormalStaus("Ready");
            }            
            this.Cursor = Cursors.Default;          
        }
        private void tagrdCustomer_Leave(object sender, EventArgs e)
        {
            if (objFactory.IsDirty && !this.formClose && (listCustomer.Count != 0 || listWatch.Count != 0 || listFollowUpDate.Count != 0)) if (SaveChanges()) { }            
        }
        private void tagrdRemark_Leave(object sender, EventArgs e)
        {
            if ((ecellRemark != "") && (!this.formClose)) if (SaveChangesRemark()) { }
        }
        private void tsbClose_Click(object sender, EventArgs e)
        {
            ClearLock();
            if (objFactory.IsDirty && (listCustomer.Count != 0 || listWatch.Count != 0 || listFollowUpDate.Count != 0)) if (SaveChanges()) { }
            if (ecellRemark != "") if (SaveChangesRemark()) { }
            if (objFactory.IsDirtyWatch ) if (SaveChangesWatch()) { }
            this.Close();
        }
        private void tagrdRemark_CellChange(object sender, CellEventArgs e)
        {
            UltraGridCell cell = e.Cell;
            UltraGridColumn column = cell.Column;
            
            if (column.Key == "Remark")
            {
                string cellText = cell.Text;
                if(cellText != "")
                    //RemarkCellChage = true;
                ecellRemark = cellText;               
            }
        }
        private void tagrdCustomer_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //e.Layout.Bands[0].Columns["T"].Format = "C";
            //Set the mask to be in the following format:
            e.Layout.Bands[0].Columns["T"].MaskInput = "nnnnnnnnnnnnnnnnnnnn.nn";
            e.Layout.Bands[0].Columns["B"].MaskInput = "nnnnnnnnnnnnnnnnnnnn.nn";
            e.Layout.Bands[0].Columns["1"].MaskInput = "nnnnnnnnnnnnnnnnnnnn.nn";
            e.Layout.Bands[0].Columns["2"].MaskInput = "nnnnnnnnnnnnnnnnnnnn.nn";
            e.Layout.Bands[0].Columns["3"].MaskInput = "nnnnnnnnnnnnnnnnnnnn.nn";
            e.Layout.Bands[0].Columns["4"].MaskInput = "nnnnnnnnnnnnnnnnnnnn.nn";
            e.Layout.Bands[0].Columns["5"].MaskInput = "nnnnnnnnnnnnnnnnnnnn.nn";
            e.Layout.Bands[0].Columns["6"].MaskInput = "nnnnnnnnnnnnnnnnnnnn.nn";
            e.Layout.Bands[0].Columns["7"].MaskInput = "nnnnnnnnnnnnnnnnnnnn.nn";
            e.Layout.Bands[0].Columns["8"].MaskInput = "nnnnnnnnnnnnnnnnnnnn.nn";
            e.Layout.Bands[0].Columns["9"].MaskInput = "nnnnnnnnnnnnnnnnnnnn.nn";
            e.Layout.Bands[0].Columns["10"].MaskInput = "nnnnnnnnnnnnnnnnnnnn.nn";
            e.Layout.Bands[0].Columns["11"].MaskInput = "nnnnnnnnnnnnnnnnnnnn.nn";
            e.Layout.Bands[0].Columns["12"].MaskInput = "nnnnnnnnnnnnnnnnnnnn.nn";
            e.Layout.Bands[0].Columns["MthPayAmt"].MaskInput = "nnnnnnnnnnnnnnnnnnnn.nn";
            e.Layout.Bands[0].Columns["CYR"].MaskInput = "nnnnnnnnnnnnnnnnnnnn.nn";
            e.Layout.Bands[0].Columns["0"].MaskInput = "nnnnnnnnnnnnnnnnnnnn.nn"; 
            e.Layout.Bands[0].Columns["DocCreditLimit"].MaskInput = "nnnnnnnnnnnnnnnnnnnn.nn";





        }       
        private void btnCSearch_Click(object sender, EventArgs e)
        {
            string ConName = txtConName.Text.ToString();
            frmMain.gfrmMain.SetNotifyStatus("Loading.... Please wait.");
            if (objFactory.GetEdit(Opt, DueCalu, CCBT, DateV, ConName))
            {
                tagrdCustomer.DataSource = objFactory.ObjConmanageDT;
                tagrdCustomer.DataBind();
            }
            Format_tagrdCustomer();
            FormatCustomerGrid();
            frmMain.gfrmMain.SetNormalStaus("Ready");
            this.Cursor = Cursors.Default;
        }
        private void txtConName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                string ConName = txtConName.Text.ToString();
                frmMain.gfrmMain.SetNotifyStatus("Loading.... Please wait.");
                if (objFactory.GetEdit(Opt, DueCalu, CCBT, DateV, ConName))
                {
                    tagrdCustomer.DataSource = objFactory.ObjConmanageDT;
                    tagrdCustomer.DataBind();
                }
                Format_tagrdCustomer();
                FormatCustomerGrid();
                frmMain.gfrmMain.SetNormalStaus("Ready");
                this.Cursor = Cursors.Default;

            }
        }
        private void txtConName_Enter(object sender, EventArgs e)
        {
            //txtConName.Text = "";
        }
        private void CustList_Enter(object sender, EventArgs e)
        {
            //CustList.Text = "";
        }              
        private void cboRemarkType1_CustomUpdate(object sender, CancelEventArgs e)
        {
            this.tagrdRemark.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
            string RemarkType = string.Empty;
            if (formClose)
                return;

            RemarkType = cboRemarkType1.Text.ToString();

            if (this.tagrdRemark.DisplayLayout.Bands[0].Columns.Count > 0)
            {
                if (RemarkType == "" || RemarkType == "All")
                {
                    this.tagrdRemark.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                }
                else
                    this.tagrdRemark.DisplayLayout.Bands[0].ColumnFilters["RemarkType"].FilterConditions.Add(FilterComparisionOperator.Equals, RemarkType);
            }
            FormatRemarkGrid();
            FormatRemarkGridReadOnly();
        }
        private void cboRemarkDesc1_CustomUpdate(object sender, CancelEventArgs e)
        {
            this.tagrdRemark.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
            string RemarkDesc = string.Empty;
            if (formClose)
                return;

            RemarkDesc = cboRemarkDesc1.Text.ToString();

            if (this.tagrdRemark.DisplayLayout.Bands[0].Columns.Count > 0)
            {
                if (RemarkDesc == "" || RemarkDesc == "All")
                {
                    this.tagrdRemark.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                }
                else
                    this.tagrdRemark.DisplayLayout.Bands[0].ColumnFilters["RemarkDesc"].FilterConditions.Add(FilterComparisionOperator.Equals, RemarkDesc);
            }
            FormatRemarkGrid();
            FormatRemarkGridReadOnly();

        }        
        private void tagrdCustomer_AfterCellUpdate(object sender, CellEventArgs e)
        {
            UltraGridCell cell = e.Cell;
            UltraGridColumn column = cell.Column;
            if (column.Key == "Watch")
            {
                int ConKey = 0;                
                ConKey = Convert.ToInt32(tagrdCustomer.ActiveRow.Cells["DocConKey"].Value);
                if (listWatch.Contains(ConKey))
                    return;
                listWatch.Add(ConKey);
            }

            if (column.Key == "FollowUpDate")
            {
                int ConKey = 0;
                ConKey = Convert.ToInt32(tagrdCustomer.ActiveRow.Cells["DocConKey"].Value);
                if (listFollowUpDate.Contains(ConKey))
                    return;
                listFollowUpDate.Add(ConKey);
            }

            if (column.Key == "ActiveWithProblem" || column.Key == "COOApprovalRequired")
            {
                int ConKey = 0;
                ConKey = Convert.ToInt32(tagrdCustomer.ActiveRow.Cells["DocConKey"].Value);               

                if (listCustomer.Contains(ConKey))
                    return;
                listCustomer.Add(ConKey);

            
            }
            //int selConKey = Convert.ToInt32(tagrdCustomer.ActiveRow.Cells["DocConKey"].Value);
           
        //    For i As Integer = 1 To Me.UltraGrid1.Selected.Rows.Count
        //    Debug.WriteLine(Me.UltraGrid1.Selected.Rows(i).Cells("Column Key").Text)
        //Next

        }
        private void FollowUpDate_CustomUpdate(object sender, CancelEventArgs e)
        {
            DateTime FUpDate;
            FUpDate = Convert.ToDateTime(FollowUpDate.Value);
            string strFUpDate = FUpDate.ToString("dd MMM yyyy");
            this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
            if (formClose)
                return;

            if (this.tagrdCustomer.DisplayLayout.Bands[0].Columns.Count > 0)
            {
                if (strFUpDate == "")
                    ((DataTable)tagrdCustomer.DataSource).DefaultView.RowFilter = "FollowUpDate=" + "";
                else
                    // ((DataTable)tagrdCustomer.DataSource).DefaultView.RowFilter = "FollowUpDate=" + strFUpDate;
                    this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters["FollowUpDate"].FilterConditions.Add(FilterComparisionOperator.Equals, FUpDate);
            }
            //FormatWatchList();

            if (strFUpDate == "")
            {
                ((DataTable)tagrdCustomer.DataSource).DefaultView.RowFilter = "FollowUpDate=" + "";
                ((DataTable)tagrdCustomer.DataSource).DefaultView.RowFilter = "FollowUpDate=" + "";
            }
            else
            {
                this.tagrdCustomer.DisplayLayout.Bands[0].ColumnFilters["FollowUpDate"].FilterConditions.Add(FilterComparisionOperator.Equals, FUpDate);

            }
        }    
        private void tagrdCustomer_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        { try
            {
                frmMain.gfrmMain.SetNotifyStatus("Loading.... Please wait.");
                string DueCalString = "";
                if (DueCalu == 10) DueCalString = "Use Due Date";
                if (DueCalu == 20) DueCalString = "Use Invoice Date";
                if (DueCalu == 30) DueCalString = "Use Due Date + Additional Day";
                string CCB = "";
                if (CCBT == 10) CCB = "Credit Sales";
                if (CCBT == 20) CCB = "Cash Sales";
                int ConKey = 0;
                ConKey = Convert.ToInt32(tagrdCustomer.ActiveRow.Cells["DocConKey"].Value);
                string ConID = "";
                ConID = tagrdCustomer.ActiveRow.Cells["DocConID"].Value.ToString();
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@ConKey", ConKey));
                parmList.Add(new SqlParameter("@DateV", DateV));
                parmList.Add(new SqlParameter("@DueCal", DueCalu));
                parmList.Add(new SqlParameter("@CCB", CCBT));
                parmList.Add(new SqlParameter("@RepKey", 1290));
                Application.DoEvents();
                DataSet ds = GFunc.ExecuteProcDataSet("Rep_ConAge_CustStatement", parmList);
                DataTable dt = new DataTable();

                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                else
                {
                    MsgBox.Show("Report Data Get Fail"); //
                    return;
                }
                CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rptDoc.Load(Application.StartupPath + @"\Reports\" + "Cust_AgeDetRemF.rpt");
                rptDoc.SetDataSource(dt);

                List<ReportParameter> repParas = new List<ReportParameter>();

                string opCmpValue = SysOptionUtility.GetStr("CompanyName");
                repParas.Add(new ReportParameter("pCmpName", opCmpValue));
                repParas.Add(new ReportParameter("pRepRange", "DATE AS AT = \"" + DateV + "\",DUE CALCULATION = \"" + DueCalString + "\", FOR ALL DOC GROUP, CUSTOMER Between \"" + ConID + "\" And \"" + ConID + "\", CREDIT / CASH = \"" + CCB + "\", FOR ALL CUSTOMER CLASS, FOR ALL TERRITORY, FOR ALL INDUSTRY, FOR ALL CUSTOMER CURRENCY"));
                repParas.Add(new ReportParameter("pRepTitle", "Customer Aging Detail"));

                foreach (ReportParameter p in repParas)
                {
                    rptDoc.SetParameterValue(p.ParameterName, p.ParameterValue);
                }
                _sysRep = SYSRep.Get(1290);
                frmReportViewer fRptViewer = new frmReportViewer();
                fRptViewer.RepKey = 1290;
                fRptViewer.RptName = "Cust_AgeDetRemF.rpt";
                ObjSYSRep.RPTname1 = "Cust_AgeDetRemF.rpt";//For To do Form
                fRptViewer.RptDocument = rptDoc;
                fRptViewer.MdiParent = frmMain.gfrmMain;
                fRptViewer.Show();
                GC.Collect();
                frmMain.gfrmMain.SetNormalStaus("Ready");
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message); // System Msg
            }
        }

        /* Add Watch List -- modified UI Design by YST on 2021/11/09 , the old version is frmMSTConManage_ByNNT*/

        /* Follow Up List -- start // added by YST on 2021/11/09 */
        private void btnFUpRefresh_Click(object sender, EventArgs e)
        {
            FollowUpListDataBinding();
        }
        private void FUpRemarkType_CustomUpdate(object sender, CancelEventArgs e)
        {
            FollowUpListGridBinding();
        }
        private void FUpRemarkDes_CustomUpdate(object sender, CancelEventArgs e)
        {
            FollowUpListGridBinding();
        }
        private void FUpCustStatus_CustomUpdate(object sender, CancelEventArgs e)
        {
            FollowUpListGridBinding();
        }
        private void FUpCustName_TextChanged(object sender, EventArgs e)
        {
            FollowUpListGridBinding();
        }
        private void frm_KeyDown(object sender, KeyEventArgs e)
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
        private void FollowUpListInitialize()
        {
            FUpDateFrom.DateValue = GlobalUI.ListDefaultFromDate;
            FUpDateTo.DateValue = GlobalUI.ListDefaultToDate;
            FUpCustIDFrom.SelectedText = "";
            FUpCustIDTo.SelectedText = "";
            FUpCustStatus.SelectedRow = FUpCustStatus.Rows[0];
            FUpRemarkType.SelectedRow = FUpRemarkType.Rows[0];
            FUpRemarkDes.SelectedRow = FUpRemarkDes.Rows[0];
            FUpCustName.Text = "";
        }
        private void FollowUpListDataBinding()
        {
            try
            {
                List<SqlParameter> paralist = new List<SqlParameter>();
                paralist.Add(new SqlParameter("@DateFrom", FUpDateFrom.Value.ToString()));
                paralist.Add(new SqlParameter("@DateTo", FUpDateTo.Value));
                paralist.Add(new SqlParameter("@ConFrom", FUpCustIDFrom.Text));
                paralist.Add(new SqlParameter("@ConTo", FUpCustIDTo.Text));
                dtFollowUpList = GFunc.ExecuteProc("MSTConRemark_List", paralist);

                tagrdFUpRemarkList.DataSource = dtFollowUpList;
                tagrdFUpRemarkList.DataBind();

                listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "0GF", tagrdFUpRemarkList.Name);
                GlobalUI.Grid_Format(tagrdFUpRemarkList, listID, false, true);

                FollowUpListGridBinding();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }                                  
        }
        private void FollowUpListGridBinding()
        {
            try
            {
                string strFilter = "RemarkType <> '' ";
                if (FUpCustStatus.Text != "All" && !string.IsNullOrEmpty(FUpCustStatus.Text))
                {
                    if (FUpCustStatus.Text == "Halt")
                        strFilter = strFilter + "and ActiveWithProblem = True";
                    else
                        strFilter = strFilter + "and COOApprovalRequired = True";
                }
                if (FUpRemarkType.Text != "All" && !string.IsNullOrEmpty(FUpRemarkType.Text))
                {
                    strFilter = strFilter + "and RemarkType='" + FUpRemarkType.Text + "'";
                }
                if (FUpRemarkDes.Text != "All" && !string.IsNullOrEmpty(FUpRemarkDes.Text))
                {
                    strFilter = strFilter + "and RemarkDesc='" + FUpRemarkDes.Text + "'";
                }
                if (!string.IsNullOrEmpty(FUpCustName.Text))
                {
                    strFilter = strFilter + "and ConNm Like '%" + FUpCustName.Text.Replace("'", "''") + "%'";
                }

                ((DataTable)tagrdFUpRemarkList.DataSource).DefaultView.RowFilter = strFilter.Replace("*","[*]"); /* spcial characters %,*,] => [%], [*], []] for RowFilter */

                if (tagrdFUpRemarkList.Rows.Count > 0)
                {
                    for (int i = 0; i < tagrdFUpRemarkList.Rows.Count; i++)
                    {
                        if (tagrdFUpRemarkList.Rows[i].Cells["ActiveWithProblem"].Value.ToString() == "True")
                            tagrdFUpRemarkList.Rows[i].Appearance.BackColor = Color.Red;
                        else if (tagrdFUpRemarkList.Rows[i].Cells["COOApprovalRequired"].Value.ToString() == "True")
                            tagrdFUpRemarkList.Rows[i].Appearance.BackColor = Color.Orange;
                        else
                            tagrdFUpRemarkList.Rows[i].Appearance.BackColor = Color.White;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }           
        }

        /* Follow Up List -- end */
    }
}
