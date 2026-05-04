using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using Infragistics.Win.UltraWinTree;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using BOLib;
using DataDynamics.ActiveReports;
using System.Data.SqlClient;
using Infragistics.Win.UltraWinListView;
using System.Linq;
using System.Security.Permissions;
using System.Reflection;
using System.Xml;
using TAUtil;

namespace WinUI
{
    public partial class frmReportDirectory : Form
    {
        #region Member Variables, Properties, Constructors and Destructors

        internal ReportFactory objReportFactory = null;

        private string msgID = string.Empty;
        private bool formClose = false;
        string pRepRange = string.Empty;
        bool CriteriaRangeInclude = false;
        bool CriteriaSearchFormatInclude = false;

        //for Criteria Grid of TODO Form
        internal DataTable dtDatePeriodCriteria;
        //for selected rpt for TODO Form
        internal string SelectedRptName = "";

        private EditorButton editorButton = null;
        private ContextMenu cmunPopup = null;
        private ContextMenu cmunCombo = null;

        List<SqlParameter> parmList;
        frmRpxViewer fViewr;

        string[] TextBtnList = new string[] { "Itm", "Con", "Job", "Vendor" };
        private const string ItmCriteriaNm = "Itm";
        string ContextMenuSetting = string.Empty;

        public TAUtil.TAGridEditor CriteriaGrid
        {
            get
            {
                return tagrdCriteriaList;
            }
        }

        public frmReportDirectory()
        {
            InitializeComponent();

            //for Criteria Grid of TODO Form
            dtDatePeriodCriteria = new DataTable();
            dtDatePeriodCriteria.Columns.Add("CriteriaNm");
            dtDatePeriodCriteria.Columns.Add("CriteriaLabel");
            dtDatePeriodCriteria.Columns.Add("CriteriaValueDate");
            dtDatePeriodCriteria.Columns.Add("CriteriaValueInt");
            dtDatePeriodCriteria.Columns.Add("DateType");
            dtDatePeriodCriteria.Columns.Add("DateDifference");
            dtDatePeriodCriteria.Columns.Add("WeekDay");
            dtDatePeriodCriteria.Columns.Add("MthDayNum");
            dtDatePeriodCriteria.Columns.Add("MthWeek");
            dtDatePeriodCriteria.Columns.Add("MthDay");
            dtDatePeriodCriteria.Columns.Add("YearMthNum");
            dtDatePeriodCriteria.Columns.Add("YearMthDayNum");
            dtDatePeriodCriteria.Columns.Add("YearMthWeek");
            dtDatePeriodCriteria.Columns.Add("YearMthDay");
            dtDatePeriodCriteria.Columns.Add("PeriodType");
            dtDatePeriodCriteria.Columns.Add("PeriodDifference");
            dtDatePeriodCriteria.Columns.Add("PeriodMth");
        }

        #endregion

        private void frmReportDirectory_Load(object sender, EventArgs e)
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (System.Diagnostics.Debugger.IsAttached)
                    tsbCopyToRptOutputDirectory.Visible = true;
                else
                    tsbCopyToRptOutputDirectory.Visible = false;

                    // Call Initialization
                objReportFactory = new ReportFactory(0);

                // GlobalUI.BindComboValue(SelectGroups, GVar.ListSettingID.SECGroupsByUserBrowseCombo + "%" + AppInfor.CurrentUserKey, "GrpID", "GrpKey", (int)GEnum.SystemCode.Security_Group);
                this.Refresh_ReportList();

                tagrdCriteriaList.DisplayLayout.Bands[0].Columns.Add("CriteriaLabel1");
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns.Add("CriteriaSource1");
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns.Add("CriteriaLabel2");
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns.Add("CriteriaSource2");
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns.Add("CriteriaLabel3");
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns.Add("CriteriaSource3");

                FormatGrid(150);              

                lvwRptFileList.SubItemColumns.Add("RptNm");
                lvwRptFileList.SubItemColumns.Add("RptDes");
                lvwRptFileList.SubItemColumns.Add("Custom1");
                lvwRptFileList.SubItemColumns.Add("RptAltRecordSource");
                lvwRptFileList.SubItemColumns.Add("RptLayOut");
                lvwRptFileList.SubItemColumns.Add("RptPermission");
                lvwRptFileList.SubItemColumns.Add("RptPrintCondition");
                lvwRptFileList.SubItemColumns.Add("RptOrderBy");
                lvwRptFileList.SubItemColumns.Add("ShwItmCount");
                lvwRptFileList.SubItemColumns.Add("ShwLetterHead");
                lvwRptFileList.SubItemColumns.Add("PrtCopies");

                SetFormLevelData();

                if (!this.IsMdiChild)// called from TODO 
                {
                    tsbViewReport.Text = "&OK";
                    tsbViewReport.Image = global::WinUI.Properties.Resources.doc_ok_32;
                    tsbClose.Text = "&Cancel";

                }
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0);
                GlobalUI.Combos_Fill(this, 0);
                this.EnableButton(false);//Default is dpn't show that buttons
                tslReadOnly.Visible = false;
            }
            catch (TAException tex)
            {
                // Check Process 
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
                MsgBox.Show(ex.Message); // System Msg   
            }
            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }
        private void frmReportDirectory_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                    GlobalUI.Combos_Fill(this, 0);
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
        private void frmReportDirectory_Shown(object sender, EventArgs e)
        {
            // Check Form Close State is True ...
            if (formClose)
            {
                this.Close();
            }
        }
        private void frmReportDirectory_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (sender == this && !this.IsMdiChild) //call as Dialog from ToDo form and ToDo form still need to use objReportFactory.ObjSysRep, etc...
                {
                    //this.DialogResult = DialogResult.OK;
                    return;
                }
                // Call Dispose
                bool isOk = objReportFactory.Dispose();

                // Check Process
                if ((!isOk) && this.msgID != string.Empty)
                    MsgBox.Show(msgID); // Custom Msg    
                else
                {
                    if (fViewr != null)
                    {
                        fViewr.Dispose();
                        if (fViewr.rptDoc != null)
                        {
                            fViewr.rptDoc.Document.Dispose();
                            fViewr.rptDoc.Dispose();
                        }
                    }
                }

                //When the form is closed by main form, to proceed closing 
                frmMain.gfrmMain.Tag = string.Empty;

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

        private void lvwRptFileList_ItemDoubleClick(object sender, ItemDoubleClickEventArgs e)
        {
            try
            {
                tsbViewReport_Click(sender, e);
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

        private void DateButtons_Click(object sender, EventArgs e)
        {
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();

            Infragistics.Win.Misc.UltraButton btn = sender as Infragistics.Win.Misc.UltraButton;

            switch (btn.Name.ToLower())
            {
                case "btntoday":
                    dt1 = DateTime.Today;
                    dt2 = DateTime.Today;
                    break;
                case "btnthismonth":
                    dt1 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    dt2 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
                    break;
                case "btnthisyear":
                    dt1 = new DateTime(DateTime.Today.Year, 1, 1);
                    dt2 = new DateTime(DateTime.Today.Year, 12, 31);
                    break;
                case "btnthisfyear":
                    if (!SYSList.GetFiscalPeriod(ref dt1, ref dt2))
                    {
                        MsgBox.Show("FiscalPeriodGetFail");
                        return;
                    }
                    break;
            }

            for (int i = 0; i < objReportFactory.SYSRepCriterias.Rows.Count; i++)
            {
                if (Convert.ToBoolean(objReportFactory.SYSRepCriterias.Rows[i]["CriteriaSetDateButton"]) || objReportFactory.SYSRepCriterias.Rows[i]["CriteriaDataType"].Equals("IntegerPeriod"))
                {
                    if (objReportFactory.SYSRepCriterias.Rows[i]["CriteriaType"].Equals("Range"))
                    {
                        if (objReportFactory.SYSRepCriterias.Rows[i]["CriteriaDataType"].Equals("IntegerPeriod"))
                        {
                            tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].Value = dt1.Date.ToString("yyyyMM");
                            tagrdCriteriaList.Rows[i].Cells["CriteriaSource2"].Value = dt2.Date.ToString("yyyyMM");
                        }
                        else
                        {
                            tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].Value = dt1.Date.ToString("dd MMM yyyy");
                            tagrdCriteriaList.Rows[i].Cells["CriteriaSource2"].Value = dt2.Date.ToString("dd MMM yyyy");
                        }
                    }
                    else
                    {
                        if (GFunc.IsNE(btn.Tag))
                            if (objReportFactory.SYSRepCriterias.Rows[i]["CriteriaDataType"].Equals("IntegerPeriod"))
                            {
                                tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].Value = dt2.Date.ToString("yyyyMM");
                            }
                            else
                            {
                                tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].Value = dt2.Date.ToString("dd MMM yyyy");
                            }
                        else
                            if (objReportFactory.SYSRepCriterias.Rows[i]["CriteriaDataType"].Equals("IntegerPeriod"))
                            {
                                tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].Value = dt1.Date.ToString("yyyyMM");
                            }
                            else
                            {
                                tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].Value = dt1.Date.ToString("dd MMM yyyy");
                            }
                    }
                }
            }

            if (GFunc.IsNE(btn.Tag))
                btn.Tag = "Clicked";
            else
                btn.Tag = null;

        }

        private void tagrdReports_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            try
            {
                GlobalUI.FreezeControl(this.Handle, true);

                int RepKey = 0;
                btnGenerateXML.Visible = false;

                if (tagrdReports.Selected.Cells.Count > 0)
                {
                    RepKey = GFunc.NEInt(tagrdReports.Selected.Cells[0].Row.Cells["RepKey"].Value, 0);
                    objReportFactory.RefreshDataByRepKey(RepKey);
                }

                if (RepKey > 0)
                {
                    if (RepKey == 1135)//Account -> Tax Ledger
                        btnGenerateXML.Visible = true;

                    string formName = objReportFactory.ObjSYSRep.FormNm;
                    string RepType = objReportFactory.ObjSYSRep.RepType;

                    if (GFunc.CompareString(RepType, "Form") == true && formName != "")
                    {
                        lvwRptFileList.Items.Clear();
                        tagrdCriteriaList.DataSource = null;
                        splitContainer2.Visible = false;
                    }
                    else
                    {
                        splitContainer2.Visible = true;
                        tagrdCriteriaList.DataSource = objReportFactory.SYSRepCriterias;
                        tagrdCriteriaList.Refresh();
                        BuildCriterias();

                        lvwRptFileList.ViewSettingsList.ColumnWidth = lvwRptFileList.Width;
                        lvwRptFileList.Items.Clear();

                        int i = 0;
                        foreach (DataRow row in objReportFactory.ReportFileList.Rows)
                        {
                            //Jack; to display rptName for develpment time only;
                            //lvwRptFileList.Items.Add(row["UID"].ToString(), row["RptDes"].ToString()+row["RptNm"].ToString());
                            lvwRptFileList.Items.Add(row["UID"].ToString(), row["RptDes"].ToString());
                            lvwRptFileList.Items[i].SubItems["RptNm"].Value = row["RptNm"];
                            lvwRptFileList.Items[i].SubItems["RptDes"].Value = row["RptDes"];
                            lvwRptFileList.Items[i].SubItems["RptAltRecordSource"].Value = row["RptAltRecordSource"];
                            lvwRptFileList.Items[i].SubItems["Custom1"].Value = row["Custom1"];
                            lvwRptFileList.Items[i].SubItems["RptLayOut"].Value = row["RptLayOut"];
                            lvwRptFileList.Items[i].SubItems["RptPermission"].Value = row["RptPermission"];
                            lvwRptFileList.Items[i].SubItems["RptPrintCondition"].Value = row["RptPrintCondition"];
                            lvwRptFileList.Items[i].SubItems["RptOrderBy"].Value = row["RptOrderBy"];
                            lvwRptFileList.Items[i].SubItems["ShwItmCount"].Value = row["ShwItmCount"];
                            lvwRptFileList.Items[i].SubItems["ShwLetterHead"].Value = row["ShwLetterHead"];
                            lvwRptFileList.Items[i].SubItems["PrtCopies"].Value = row["PrtCopies"];

                            lvwRptFileList.Items[i].SubItems["PrtCopies"].Column.Width = 0;
                            lvwRptFileList.Items[i].Appearance.Image = WinUI.Properties.Resources.black_folder_32;
                            i++;
                        }
                    }
                }
                if (RepKey == 1466)
                {
                    List<SqlParameter> list = new List<SqlParameter>();
                    list.Add(new SqlParameter("@Option", 1));
                    list.Add(new SqlParameter("@EmID", AppInfor.CurrentUserID));
                    DataTable dt = GFunc.ExecuteProc("MSTUNIT_GET", list);

                    if (dt.Rows.Count > 0)
                    {
                        string teams = GFunc.NEStr(dt.Rows[0]["AccessTeam"], "");
                        string addFilter = "";

                        if (teams.Equals(""))
                        {
                            addFilter = "SalesRep," + AppInfor.CurrentUserID ;   
                        }
                        else if (!teams.Equals("ALL"))// 'SBU', 'IBU'
                        {
                            addFilter = "BUID," + teams;
                        }

                        if (addFilter != "")
                        {
                            if (GFunc.NEStr(tagrdCriteriaList.Rows[9].Cells["CriteriaNm"].Value, "").ToUpper().Equals("ADDITIONALFILTER"))//for a better performance
                                tagrdCriteriaList.Rows[9].Cells["CriteriaSource1"].Value = addFilter;
                            else
                            {
                                DataTable dt1 = tagrdCriteriaList.DataSource as DataTable;
                                DataRow[] drs = dt1.Select("CriteriaNm='AdditionalFilter'");
                                if (drs.Length > 0)
                                    drs[0]["CriteriaSource1"] = addFilter;
                            }

                            //Hide rows
                            if (GFunc.NEStr(tagrdCriteriaList.Rows[1].Cells["CriteriaNm"].Value, "").ToUpper().Equals("EMID"))//for a better performance
                                tagrdCriteriaList.Rows[1].Hidden = true;
                            else
                            {
                                foreach (UltraGridRow row in tagrdCriteriaList.Rows)
                                {
                                    if (GFunc.NEStr(row.Cells["CriteriaNm"].Value, "").ToUpper().Equals("EMID"))
                                    {
                                        row.Hidden = true;
                                        break;
                                    }
                                }
                            }
                            if (GFunc.NEStr(tagrdCriteriaList.Rows[8].Cells["CriteriaNm"].Value, "").ToUpper().Equals("BUID"))//for a better performance
                                tagrdCriteriaList.Rows[8].Hidden = true;
                            else
                            {
                                foreach (UltraGridRow row in tagrdCriteriaList.Rows)
                                {
                                    if (GFunc.NEStr(row.Cells["CriteriaNm"].Value, "").ToUpper().Equals("BUID"))
                                    {
                                        row.Hidden = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
            finally
            {
                GlobalUI.FreezeControl(this.Handle, true);
                this.Refresh();
            }
        }

        private void tagrdReports_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            try
            {
                if (tagrdReports.Selected.Cells.Count > 0)
                {
                    string formName;
                    int RepKey = GFunc.NEInt(tagrdReports.Selected.Cells[0].Row.Cells["RepKey"].Value, 0);
                    objReportFactory.RefreshDataByRepKey(RepKey);
                    formName = objReportFactory.ObjSYSRep.FormNm;

                    string RepType = objReportFactory.ObjSYSRep.RepType;

                    if ((RepKey > 0 && GFunc.CompareString(RepType, "Form") == true && formName != ""))
                    {
                        OpenForm(formName, RepKey);
                    }
                }
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }

        private void tagrdCriteriaList_BeforeCellActivate(object sender, Infragistics.Win.UltraWinGrid.CancelableCellEventArgs e)
        {//objReportFactory.SYSRepCriterias

            if (this.IsMdiChild == false)
                if (GFunc.CompareString(e.Cell.Row.Cells["CriteriaDataType"].Value.ToString().ToUpper(), "Date".ToUpper()))
                {
                    if (GFunc.CompareString(e.Cell.Row.Cells["CriteriaType"].Value.ToString().ToUpper(), "Range".ToUpper()))
                    {
                        if (e.Cell.Column == tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource1"])
                        {
                            e.Cell.Activation = Activation.ActivateOnly;
                            frmDateCriteria criteria = new frmDateCriteria(dtDatePeriodCriteria, e.Cell.Row.Cells["CriteriaNm"].Value.ToString() + "From", e.Cell.Row.Cells["CriteriaLabel"].Value.ToString() + " From");
                            if (criteria.ShowDialog() == DialogResult.OK)
                                e.Cell.Value = criteria.CriteriaDate;
                        }
                        else if (e.Cell.Column == tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource2"])
                        {
                            e.Cell.Activation = Activation.ActivateOnly;
                            frmDateCriteria criteria = new frmDateCriteria(dtDatePeriodCriteria, e.Cell.Row.Cells["CriteriaNm"].Value.ToString() + "To", e.Cell.Row.Cells["CriteriaLabel"].Value.ToString() + " To");
                            if (criteria.ShowDialog() == DialogResult.OK)
                                e.Cell.Value = criteria.CriteriaDate;
                        }
                    }
                    else
                    {
                        if (e.Cell.Column == tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource1"])
                        {
                            frmDateCriteria criteria = new frmDateCriteria(dtDatePeriodCriteria, e.Cell.Row.Cells["CriteriaNm"].Value.ToString(), e.Cell.Row.Cells["CriteriaLabel"].Value.ToString());
                            if (criteria.ShowDialog() == DialogResult.OK)
                                e.Cell.Value = criteria.CriteriaDate;
                        }
                    }
                }
                else if (GFunc.CompareString(e.Cell.Row.Cells["CriteriaDataType"].Value.ToString().ToUpper(), "IntegerPeriod".ToUpper()))
                {
                    if (GFunc.CompareString(e.Cell.Row.Cells["CriteriaType"].Value.ToString().ToUpper(), "Range".ToUpper()))
                    {
                        if (e.Cell.Column == tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource1"])
                        {
                            e.Cell.Activation = Activation.ActivateOnly;
                            frmPeriodChooser criteria = new frmPeriodChooser(dtDatePeriodCriteria, e.Cell.Row.Cells["CriteriaNm"].Value.ToString() + "From", e.Cell.Row.Cells["CriteriaLabel"].Value.ToString() + " From");
                            if (criteria.ShowDialog() == DialogResult.OK)
                                e.Cell.Value = criteria.criteriaValueInt;
                        }
                        else if (e.Cell.Column == tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource2"])
                        {
                            e.Cell.Activation = Activation.ActivateOnly;
                            frmPeriodChooser criteria = new frmPeriodChooser(dtDatePeriodCriteria, e.Cell.Row.Cells["CriteriaNm"].Value.ToString() + "To", e.Cell.Row.Cells["CriteriaLabel"].Value.ToString() + " To");
                            if (criteria.ShowDialog() == DialogResult.OK)
                                e.Cell.Value = criteria.criteriaValueInt;
                        }
                    }
                    else
                    {
                        if (e.Cell.Column == tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource1"])
                        {
                            frmPeriodChooser criteria = new frmPeriodChooser(dtDatePeriodCriteria, e.Cell.Row.Cells["CriteriaNm"].Value.ToString(), e.Cell.Row.Cells["CriteriaLabel"].Value.ToString());
                            if (criteria.ShowDialog() == DialogResult.OK)
                                e.Cell.Value = criteria.criteriaValueInt;
                        }
                    }
                }
        }
        private void tagrdCriteriaList_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
                switch (objReportFactory.RepKey)
                {
                    case 1145:
                        if (objReportFactory.SYSRepCriterias.Rows[e.Cell.Row.Index]["CriteriaNm"].ToString().ToLower() == "docdc")//DocDC
                        {
                            //Fill DocType Name, this code assume that DocTypeNm Criteria is the next index of DocDC in SYS_RepCriteria
                            string listID = objReportFactory.SYSRepCriterias.Rows[e.Cell.Row.Index + 1]["CriteriaListID"].ToString();//"SYSDocTypeNmByDC%0"
                            listID = listID.Split('%')[0] + "%" + GFunc.NEInt(e.Cell.Value, 0);

                            GlobalUI.BindComboValue((TAUtil.TAComboBox)tagrdCriteriaList.Rows[e.Cell.Row.Index + 1].Cells["CriteriaSource1"].EditorComponent,
                               listID);
                        }
                        break;
                }
                if (e.Cell.Column.Key.Equals("CriteriaSource1") || (e.Cell.Column.Key.Equals("CriteriaSource2")))
                {
                    if (GFunc.IsNE(e.Cell.EditorComponent) == false)
                    {
                        if (e.Cell.EditorComponent.GetType() == typeof(TAUtil.TAComboBox))
                        {
                            TAUtil.TAComboBox cbo = (TAUtil.TAComboBox)e.Cell.EditorComponent;
                            if (cbo.LimitToList)
                            {
                                DataTable dt = (DataTable)((TAUtil.TAComboBox)e.Cell.EditorComponent).DataSource;
                                IEnumerable<DataRow> dtFilter = null;
                                if (GFunc.CompareString(objReportFactory.SYSRepCriterias.Rows[e.Cell.Row.Index]["CriteriaDataType"].ToString(), "Integer")//String
                                    || GFunc.CompareString(objReportFactory.SYSRepCriterias.Rows[e.Cell.Row.Index]["CriteriaDataType"].ToString(), "IntegerPeriod"))
                                    dtFilter = dt.AsEnumerable().Where(r => GFunc.NEInt(r.Field<int?>(cbo.ValueMember), 0).Equals(GFunc.NEInt(e.Cell.Value, 0)));
                                else
                                    dtFilter = dt.AsEnumerable().Where(r => GFunc.NEStr(r.Field<string>(cbo.ValueMember), "").Equals(e.Cell.Value.ToString()));
                                if (dtFilter.Count() == 0)
                                {
                                    cbo.Text = e.Cell.Text;
                                    this.Combo_NotInList(cbo, new ValidationErrorEventArgs());
                                    e.Cancel = true;
                                }
                            }

                        }
                    }

                }
                else if (e.Cell.Column.Key.Equals("CriteriaSource3"))
                {
                    string wildCard = string.Empty;
                    if (GFunc.IsNE(e.Cell.EditorComponent) == false)
                    {
                        if (e.Cell.EditorComponent.GetType() == typeof(TAUtil.TAComboBox))
                        {
                            TAUtil.TAComboBox cbo = (TAUtil.TAComboBox)e.Cell.EditorComponent;
                            if (cbo.DataSource != null)
                            {
                                DataTable dt = (DataTable)((TAUtil.TAComboBox)e.Cell.EditorComponent).DataSource;
                                //dt.DefaultView.RowFilter = cbo.ValueMember + "=" + GFunc.NEInt(e.Cell.Value, 0).ToString() + "";
                                // if (dt.DefaultView.Count > 0)
                                if (dt.AsEnumerable().Where(r => r.Field<int?>(cbo.ValueMember) == GFunc.NEInt(e.Cell.Value, 0)).Count() == 0)
                                {
                                    wildCard = dt.DefaultView[0]["IDFormatText"].ToString();
                                }
                                // dt.DefaultView.RowFilter = "";
                            }
                        }
                    }

                    if (wildCard.Length > 0)
                    {
                        e.Cell.Row.Cells["CriteriaSource2"].Value = String.Empty;
                        e.Cell.Row.Cells["CriteriaSource2"].Appearance.BackColor = Color.AliceBlue;
                        e.Cell.Row.Cells["CriteriaSource2"].Activation = Activation.ActivateOnly;
                        e.Cell.Row.Cells["CriteriaSource2"].TabStop = DefaultableBoolean.False;
                    }
                    else
                    {
                        e.Cell.Row.Cells["CriteriaSource2"].Appearance.BackColor = System.Drawing.Color.White;
                        e.Cell.Row.Cells["CriteriaSource2"].Activation = Activation.AllowEdit;
                        e.Cell.Row.Cells["CriteriaSource2"].TabStop = DefaultableBoolean.True;
                    }
                    e.Cell.Row.Cells["CriteriaSource1"].Value = wildCard;
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }

        }
        private void tagrdCriteriaList_CustomDataError(object sender, TAUtil.TAErrorEventArgs e)
        {
            if (e.ErrorCode == TAUtil.TAErrorCode.NOT_IN_LIST)
            {
                if (tagrdCriteriaList.ActiveCell.EditorComponent != null)
                {
                    tagrdCriteriaList.PerformAction(UltraGridAction.EnterEditMode);
                    GlobalUI.ItemNotInList(tagrdCriteriaList.ActiveCell, null, 0);
                }
            }
            else
            {
                MsgBox.Show(e.ErrorMessage);
            }
        }

        private void tagrdAdditionalCriteria_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            for (int i = 0; i < tagrdCriteriaList.Rows.Count; i++)
            {
                UltraGridRow obj = tagrdCriteriaList.Rows[i];

                string CriteriaType = obj.Cells["CriteriaType"].Value.ToString();
                string CriteriaSource1 = "";
                if (CriteriaType.ToUpper() == GVar.ReportCriteriaType.SubFormSelection.ToUpper())
                {
                    for (int row = 0; row < tagrdAdditionalCriteria.Rows.Count; row++)
                    {
                        bool bSelect = false;
                        bool.TryParse(tagrdAdditionalCriteria.Rows[row].Cells["Checked"].Value.ToString(), out bSelect);
                        if (bSelect)
                        {
                            CriteriaSource1 = CriteriaSource1 + tagrdAdditionalCriteria.Rows[row].Cells["MsgValue"].Value.ToString() + ",";
                        }
                    }
                    if (CriteriaSource1.LastIndexOf(",") > -1)
                    {
                        CriteriaSource1 = CriteriaSource1.Substring(0, CriteriaSource1.Length - 1);
                    }
                    obj.Cells["CriteriaSource1"].Value = CriteriaSource1;
                }
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void tsbViewReport_Click(object sender, EventArgs e)
        {
            try
            {
                frmMain.gfrmMain.SetNotifyStatus("Loading Report ...............");
                tslReadOnly.Text = "Loading Report ...............";
                int RepKey = 0;
                List<SqlParameter> parmList = new List<SqlParameter>();

                if (this.IsMdiChild == false)//Call as Dialog from ToDo
                {

                    if (tagrdCriteriaList.ActiveRow != null)
                        tagrdCriteriaList.ActiveRow.Update();

                    parmList = objReportFactory.GetSqlParameters(tagrdCriteriaList); //use to check for validation of required parameter
                    if (parmList == null) return; // validation fail, ask for the required parameter's data from user again.

                    if (lvwRptFileList.Items.Count > 0)
                    {
                        UltraListViewItem item = null;

                        if (lvwRptFileList.SelectedItems.Count == 0)
                            item = lvwRptFileList.Items[0];
                        else
                            item = lvwRptFileList.SelectedItems[0];

                        SelectedRptName = item.SubItems["RptNm"].Value.ToString();
                    }
                    else
                        SelectedRptName = objReportFactory.ObjSYSRep.RPTname1;

                    this.Hide();
                    this.DialogResult = DialogResult.OK;
                    parmList = null;
                    return;


                }

                if (tagrdReports.Selected.Cells.Count > 0)
                {
                    RepKey = GFunc.NEInt(tagrdReports.Selected.Cells[0].Row.Cells["RepKey"].Value, 0);
                    objReportFactory.RefreshDataByRepKey(RepKey);
                    string formName = objReportFactory.ObjSYSRep.FormNm;
                    string RepType = objReportFactory.ObjSYSRep.RepType;

                    if (RepKey > 0 && GFunc.CompareString(RepType, "Form") == true && formName != "")
                    {
                        OpenForm(formName, RepKey);
                        return;
                    }
                    else if (RepKey == 1140)
                    {
                        frmAccSelection accSelection=new frmAccSelection();
                        if (accSelection.ShowDialog()  != DialogResult.OK)
                            return;
                        objReportFactory.XMLAccSelected = (accSelection).XMLAccount;
                    }
                
                }

                if (lvwRptFileList.Items.Count > 0)
                {
                    UltraListViewItem item = null;

                    if (lvwRptFileList.SelectedItems.Count == 0)
                        item = lvwRptFileList.Items[0];
                    else
                        item = lvwRptFileList.SelectedItems[0];

                    if (item.SubItems["RptNm"].Value.ToString() != "NA" || item.SubItems["RptNm"].Value.ToString() != "")
                    {
                        objReportFactory.PreviewReport(lvwRptFileList, tagrdCriteriaList, this.IsMdiChild);
                    }
                }


            }
            catch (TAException ex)
            {
                MsgBox.Show(ex.MsgID);
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
            finally
            {
                tslReadOnly.Text = "";
                frmMain.gfrmMain.SetNormalStaus("Ready");
            }
        }

        private void OpenForm(string formNm, int RepKey)
        {
            lvwRptFileList.Items.Clear();
            tagrdCriteriaList.DataSource = null;
            splitContainer2.Visible = false;

            AssemblyName oAName = Assembly.GetExecutingAssembly().GetName();
            Form objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + formNm);

            System.Reflection.ConstructorInfo[] ctors = objectForm.GetType().GetConstructors();
            foreach (ConstructorInfo ctor in ctors)
            {
                ParameterInfo[] pi = ctor.GetParameters();

                if (pi.Count() == 1)
                {
                    objectForm = null;
                    objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + formNm, false, BindingFlags.CreateInstance, null, new object[] { RepKey }, System.Globalization.CultureInfo.CurrentCulture, null);
                    break;
                }
            }

            if (objectForm != null)
            {                
                
                objectForm.MdiParent = frmMain.gfrmMain;
                objectForm.Show();

            }
            else
                MessageBox.Show("Form Name -> " + formNm + " cannot be opened.", "Data Error in SYS_Rep Table, FormNm Field");
        }
        private void tsmnuAdd_Click(object sender, EventArgs e)
        {
            TAUtil.TAComboBox cbo = null;
            string curValue = string.Empty;

            ContextMenu s = ((MenuItem)sender).GetContextMenu();
            if (s.SourceControl != null)
            {
                if (s.SourceControl.GetType() == typeof(TAUtil.TAComboBox))
                    cbo = (TAUtil.TAComboBox)s.SourceControl;
                else
                {
                    if (s.SourceControl.Parent != null)
                        if (s.SourceControl.Parent.GetType() == typeof(TAUtil.TAComboBox))
                            cbo = (TAUtil.TAComboBox)s.SourceControl.Parent;
                        else if (s.SourceControl.Parent.GetType() == typeof(TAUtil.TAGridEditor))
                        {
                            if (((TAUtil.TAGridEditor)s.SourceControl.Parent).ActiveCell != null)
                            {
                                if ((((TAUtil.TAGridEditor)s.SourceControl.Parent).ActiveCell).EditorComponent != null)
                                {
                                    if ((((TAUtil.TAGridEditor)s.SourceControl.Parent).ActiveCell).EditorComponent.GetType() == typeof(TAUtil.TAComboBox))
                                    {
                                        cbo = (((TAUtil.TAGridEditor)s.SourceControl.Parent).ActiveCell).EditorComponent as TAUtil.TAComboBox;
                                    }
                                }
                            }
                        }
                }
            }


            if (GFunc.IsNE(cbo))
                return;
            else
                curValue = cbo.Text.Trim();

            if (cbo.IsItemInList(curValue))
            {
                MsgBox.Show("Item already exist.");
                return;
            }
            else
                GlobalUI.OpenFormAsDialog(cbo, GEnum.FormOpenMode.Add, cbo.Text, 0);
        }
        private void tsmnuEdit_Click(object sender, EventArgs e)
        {
            TAUtil.TAComboBox cbo = null;

            ContextMenu s = ((MenuItem)sender).GetContextMenu();
            if (s.SourceControl != null)
            {
                if (s.SourceControl.GetType() == typeof(TAUtil.TAComboBox))
                    cbo = (TAUtil.TAComboBox)s.SourceControl;
                else
                {
                    if (s.SourceControl.Parent != null)
                        if (s.SourceControl.Parent.GetType() == typeof(TAUtil.TAComboBox))
                        {
                            cbo = (TAUtil.TAComboBox)s.SourceControl.Parent;
                        }
                        else if (s.SourceControl.Parent.GetType() == typeof(TAUtil.TAGridEditor))
                        {
                            if (((TAUtil.TAGridEditor)s.SourceControl.Parent).ActiveCell != null)
                            {
                                if ((((TAUtil.TAGridEditor)s.SourceControl.Parent).ActiveCell).EditorComponent != null)
                                {
                                    if ((((TAUtil.TAGridEditor)s.SourceControl.Parent).ActiveCell).EditorComponent.GetType() == typeof(TAUtil.TAComboBox))
                                    {
                                        cbo = (((TAUtil.TAGridEditor)s.SourceControl.Parent).ActiveCell).EditorComponent as TAUtil.TAComboBox;
                                    }
                                }
                            }
                        }
                }
            }
            if (GFunc.IsNE(cbo))
                return;
            else
            {
                if (!cbo.IsItemInList(cbo.Text.Trim()))
                {
                    MsgBox.Show("Item does not exist.");
                    return;
                }
                else
                    GlobalUI.OpenFormAsDialog(cbo, GEnum.FormOpenMode.Edit, cbo.Value, 0);
            }
        }
        private void ShowUltraOptionSet_ValueChanged(object sender, EventArgs e)
        {
            if (ShowReportsOption.CheckedIndex == 0)
            {
                RptName.SetValueTrigger(string.Empty, false);
                SelectGroups.Enabled = false;
                Refresh_ReportList();
            }
            else
            {
                SelectGroups.Enabled = true;
                SecGroupTaComboBox_CustomUpdate(sender, null);
            }
        }
        private void SecGroupTaComboBox_CustomUpdate(object sender, CancelEventArgs e)
        {
            Refresh_ReportList();
        }

        private void txtRptName_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (formClose)
                return;

            if (((DataTable)tagrdReports.DataSource).DefaultView.Count == 0)
                return;
            else
            {                
                if (this.tagrdReports.DisplayLayout.Bands[0].Columns.Count > 0)
                {
                    if (RptName.Text.Trim() != string.Empty)
                    {
                        this.tagrdReports.DisplayLayout.Bands[0].SortedColumns.Clear();
                        //GridFilterToDefaultView   
                        if(((DataTable)tagrdReports.DataSource).DefaultView.RowFilter!="")
                            ((DataTable)tagrdReports.DataSource).DefaultView.RowFilter =((DataTable)tagrdReports.DataSource).DefaultView.RowFilter +
                                " AND RepDes Like '%" + RptName.Text.Trim() + "%'";
                        else
                            ((DataTable)tagrdReports.DataSource).DefaultView.RowFilter = ((DataTable)tagrdReports.DataSource).DefaultView.RowFilter +
                               "RepDes Like '%" + RptName.Text.Trim() + "%'";
                    }
                    else
                    {
                        //((DataTable)tagrdReports.DataSource).DefaultView.RowFilter = "";
                        FilterDataByPermission();
                        FormatAvailableGrid();
                    }
                }
            }
           
        }
        private void txtRptName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtRptName_CustomUpdate(e, null);
            }
        }

        private void btnGenerateXML_Click(object sender, EventArgs e)
        {
            try
            {

                List<SqlParameter> parmList = objReportFactory.GetSqlParameters(tagrdCriteriaList);
                if (parmList != null)
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.Filter = "XML Files (*.xml)|*.xml";
                    dlg.FileName = "IAF1.1.xml";
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        DataTable dt = GFunc.ExecuteProc("IAF_GenerateXML", parmList);
                        if (dt.Rows.Count > 0)
                        {
                            XmlDocument doc = new XmlDocument();
                            string xmlString = dt.Rows[0][0].ToString();
                            doc.Load(new StringReader(xmlString));

                            // Create an XML declaration. 
                            XmlDeclaration xmldecl;
                            xmldecl = doc.CreateXmlDeclaration("1.0", null, null);
                            xmldecl.Encoding = "UTF-8";
                            xmldecl.Standalone = "yes";

                            XmlElement root = doc.DocumentElement;
                            doc.InsertBefore(xmldecl, root);

                            //In Query Results,we want to include every elements in PurchaseLines,SupplyLines,GLDataLines even if their values are null,
                            //That's why we used 'Elements XSINIL' syntax, now we will remove those attributes.
                            foreach (XmlNode node in root.ChildNodes)
                            {
                                if (GFunc.CompareString(node.Name, "Purchase") || GFunc.CompareString(node.Name, "Supply")
                                    || GFunc.CompareString(node.Name, "GLData"))
                                {
                                    //ChildNodes of Purchase are PurchaseLines
                                    //ChildNodes of Supply are SupplyLines
                                    //ChildNodes of GLData are GLDataLines
                                    foreach (XmlNode pNode in node.ChildNodes)
                                    {
                                        pNode.Attributes.RemoveAll();
                                        foreach (XmlElement el in pNode.ChildNodes)
                                        {
                                            if (el.Attributes.Count > 0)
                                                el.Attributes.RemoveAll();
                                        }
                                    }
                                }
                            }

                            doc.Save(dlg.FileName);

                            //Open folder and select the file.                     
                            System.Diagnostics.Process.Start("explorer.exe", "/select, " + dlg.FileName);
                        }
                        else
                            MessageBox.Show("XML Generation failed. No Data exists.");

                    }
                    dlg.Dispose();

                    #region commented
                    //DataSet ds = GFunc.ExecuteProcDataSet("IAF_TablesForXMLNew", objReportFactory.parmList);

                    //ds.Tables[0].TableName = "Company";
                    //ds.Tables[1].TableName = "CompanyInfo";

                    //ds.Tables[2].TableName = "Purchase";
                    //ds.Tables[3].TableName = "PurchaseLines";
                    //ds.Tables[4].TableName = "Supply";
                    //ds.Tables[5].TableName = "SupplyLines";
                    //ds.Tables[6].TableName = "GLData";
                    //ds.Tables[7].TableName = "GLDataLines";

                    //ds.Relations.Add(new DataRelation("CC", ds.Tables["Company"].Columns["Company_ID"], ds.Tables["CompanyInfo"].Columns["Company_ID"]));
                    //ds.Relations.Add(new DataRelation("CP", ds.Tables["Company"].Columns["Company_ID"], ds.Tables["Purchase"].Columns["Company_ID"]));
                    //ds.Relations.Add(new DataRelation("CS", ds.Tables["Company"].Columns["Company_ID"], ds.Tables["Supply"].Columns["Company_ID"]));
                    //ds.Relations.Add(new DataRelation("CG", ds.Tables["Company"].Columns["Company_ID"], ds.Tables["GLData"].Columns["Company_ID"]));
                    //ds.Relations.Add(new DataRelation("PL", ds.Tables["Purchase"].Columns["Purchase_Id"], ds.Tables["PurchaseLines"].Columns["Purchase_Id"]));
                    //ds.Relations.Add(new DataRelation("SS", ds.Tables["Supply"].Columns["Supply_Id"], ds.Tables["SupplyLines"].Columns["Supply_Id"]));
                    //ds.Relations.Add(new DataRelation("GL", ds.Tables["GLData"].Columns["GLData_Id"], ds.Tables["GLDataLines"].Columns["GLData_Id"]));

                    //foreach (DataRelation d in ds.Relations)
                    //    d.Nested = true;

                    //ds.WriteXml("myXmlDoc1.xml");
                    //ds.WriteXml("myXmlDoc2.xml",XmlWriteMode.IgnoreSchema);  
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridCellEditorButtonClick(object sender, EventArgs e)
        {
            try
            {
                frmRecordSearch fpopup = null;
                string cashType = string.Empty;
                string conType = string.Empty;
                string listSettingID = string.Empty;
                cashType = DocComUtility.DocCreditCashType_Get(0);
                conType = DocComUtility.DocConType_Get(0);


                if (tagrdCriteriaList.ActiveCell == null)
                    return;
                listSettingID = tagrdCriteriaList.ActiveCell.Row.Cells["CriteriaListID"].Value.ToString();
                switch (tagrdCriteriaList.ActiveCell.Tag.ToString().ToLower())
                {
                    case "con":
                    case "vendor":
                        fpopup = new frmRecordSearch(listSettingID, tagrdCriteriaList.ActiveCell.Text, listSettingID.Contains("Sales") ? GEnum.PopupType.VendID : GEnum.PopupType.CusID,
                            listSettingID.Contains("Sales") ? (int)GEnum.SystemCode.Customer : (int)GEnum.SystemCode.Vendor, listSettingID.Contains("Sales") ? (int)GEnum.SystemCode.Customer : (int)GEnum.SystemCode.Vendor);
                        fpopup.RecordSelectedEvent += new GVar.PopupSelectedEvent(this.GridCellBrowserSelected);
                        break;

                    case "itm":
                        fpopup = new frmRecordSearch(listSettingID, tagrdCriteriaList.ActiveCell.Text, GEnum.PopupType.ItmID, (int)GEnum.SystemCode.Inventory, (int)GEnum.SystemCode.Inventory);
                        fpopup.RecordSelectedEvent += new GVar.PopupSelectedEvent(this.GridCellBrowserSelected);

                        break;
                    case "job":
                        fpopup = new frmRecordSearch(listSettingID, tagrdCriteriaList.ActiveCell.Text, GEnum.PopupType.JobID, (int)GEnum.SystemCode.Job);
                        fpopup.RecordSelectedEvent += new GVar.PopupSelectedEvent(this.GridCellBrowserSelected);
                        break;
                }
                fpopup.ShowDialog();
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
        private void GridCellBrowserSelected(int key, string id)
        {
            tagrdCriteriaList.ActiveCell.Value = id;
        }
        private void Refresh_ReportList()
        {
            if (ShowReportsOption.CheckedIndex == 0)
                objReportFactory.GetRepsByGroup(0);
            else
            {
                if (!GFunc.IsNEZ(SelectGroups.Value))
                {
                    objReportFactory.GetRepsByGroup(Convert.ToInt32(SelectGroups.Value));
                }
                else
                    return;
            }

            tagrdReports.DataSource = objReportFactory.ROSYSRep;
            FilterDataByPermission();
            tagrdReports.DataBind();
            FormatAvailableGrid();
        }
        private void SetFormLevelData()
        {
            Infragistics.Win.Appearance appearance = new Infragistics.Win.Appearance();
            appearance.Image = WinUI.Properties.Resources.Browse;
            editorButton = new EditorButton();
            editorButton.Appearance = appearance;
            editorButton.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007ScrollbarButton;
            editorButton.Visible = false;

            cmunPopup = new ContextMenu();
            MenuItem mnuSearch = new MenuItem("&Search", this.GridCellEditorButtonClick, Shortcut.F6);
            cmunPopup.MenuItems.Add(mnuSearch);

            cmunCombo = new ContextMenu();
            MenuItem mnuAdd = new MenuItem("&Add", this.tsmnuAdd_Click);
            cmunCombo.MenuItems.Add(mnuAdd);
            mnuAdd = new MenuItem("&Edit", this.tsmnuEdit_Click);
            cmunCombo.MenuItems.Add(mnuAdd);

            this.utabCriteria.Tabs[1].Visible = false;
        }

        private void GetParameters()
        {
            string opCmpValue = SysOptionUtility.GetStr("CompanyName");

            foreach (DataRow obj in objReportFactory.ReportParameters.Rows)
            {
                fViewr.rptDoc.Parameters[obj["ParName"].ToString()].PromptUser = false;

                switch (obj["ParName"].ToString().ToLower())
                {
                    case "pcmpname":
                        fViewr.rptDoc.Parameters[obj["ParName"].ToString()].DefaultValue = opCmpValue;
                        break;
                    case "preprange":
                        fViewr.rptDoc.Parameters[obj["ParName"].ToString()].DefaultValue = pRepRange;
                        break;
                    default:
                        fViewr.rptDoc.Parameters[obj["ParName"].ToString()].DefaultValue = obj["Custom1"].ToString();
                        break;
                }

            }
        }
        private TAUtil.TAComboBox GetGridCombo(string ListSetting, string ValueColName, string DisplayColName, bool LimitToList, ContextMenu contexMenu, EditorButton editorBtn, EditorButtonEventHandler e)
        {
            TAUtil.TAComboBox tacbo = new TAUtil.TAComboBox();
            try
            {

                tacbo.Visible = false;

                // Set Data Source
                GlobalUI.BindComboValue(tacbo, ListSetting, DisplayColName, ValueColName, objReportFactory.RepKey);

                if (!GFunc.IsNE(contexMenu))
                {
                    tacbo.ContextMenu = contexMenu;
                }
                if (!GFunc.IsNE(editorBtn))
                {
                    tacbo.ButtonsRight.Add(editorButton);

                    tacbo.EditorButtonClick += e;
                }
                tacbo.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                tacbo.LimitToList = LimitToList;

                return tacbo;
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
            finally
            {

            }
            return tacbo;

        }
        private void BuildCriterias()
        {
            try
            {
                utabCriteria.Tabs[1].Visible = false;
                this.EnableButton(false);//Default is dpn't show that buttons
                CriteriaRangeInclude = false;
                CriteriaSearchFormatInclude = false;

                int label1MaxWidth = 0;

                if (objReportFactory.SYSRepCriterias.Rows.Count > 0)
                {
                    this.tagrdCriteriaList.DisplayLayout.Bands[0].SortedColumns.Clear();
                    this.tagrdCriteriaList.DisplayLayout.Bands[0].SortedColumns.Add("CriteriaSeq", false, false);

                    for (int i = 0; i < tagrdCriteriaList.Rows.Count; i++)
                    {
                        UltraGridRow obj = tagrdCriteriaList.Rows[i];

                        #region Set row values
                        int CriteriaSeq = 0;
                        string CriteriaType = "";
                        string CriteriaDataType = "";
                        string CriteriaNm = "";
                        string CriteriaLabel = "";
                        string CriteriaListID = "";
                        bool CriteriaSetDateButton = false;
                        bool CriteriaHaveFormatSearch = false;

                        int CriteriaRangeColValue = 0;
                        string CriteriaValueMember = "";
                        string CriteriaDisplayMember = "";
                        string CriteriaSpecialTag = "";
                        bool CriteriaHidden = false;
                        bool CriteriaLimitToList = false;
                        string CriteriaDefaultValue = "";
                        bool CriteriaRequired = false;
                        int.TryParse(obj.Cells["CriteriaSeq"].Value.ToString(), out CriteriaSeq);
                        CriteriaType = obj.Cells["CriteriaType"].Value.ToString();
                        CriteriaDataType = obj.Cells["CriteriaDataType"].Value.ToString();
                        CriteriaNm = obj.Cells["CriteriaNm"].Value.ToString();
                        CriteriaLabel = obj.Cells["CriteriaLabel"].Value.ToString();

                        CriteriaListID = obj.Cells["CriteriaListID"].Value.ToString();
                        bool.TryParse(obj.Cells["CriteriaSetDateButton"].Value.ToString(), out CriteriaSetDateButton);
                        bool.TryParse(obj.Cells["CriteriaHaveFormatSearch"].Value.ToString(), out CriteriaHaveFormatSearch);

                        int.TryParse(obj.Cells["CriteriaRangeColValue"].Value.ToString(), out CriteriaRangeColValue);
                        CriteriaValueMember = obj.Cells["CriteriaValueMember"].Value.ToString();
                        CriteriaDisplayMember = obj.Cells["CriteriaDisplayMember"].Value.ToString();
                        CriteriaSpecialTag = obj.Cells["CriteriaSpecialTag"].Value.ToString();
                        bool.TryParse(obj.Cells["CriteriaHidden"].Value.ToString(), out CriteriaHidden);
                        bool.TryParse(obj.Cells["CriteriaLimitToList"].Value.ToString(), out CriteriaLimitToList);
                        CriteriaDefaultValue = obj.Cells["CriteriaDefaultValue"].Value.ToString();
                        bool.TryParse(obj.Cells["CriteriaRequired"].Value.ToString(), out CriteriaRequired);
                        #endregion

                        if (CriteriaType.Equals(GVar.ReportCriteriaType.Range))
                        {
                            CriteriaRangeInclude = true;
                        }
                        if (CriteriaHaveFormatSearch)
                        {
                            CriteriaSearchFormatInclude = true;

                            tagrdCriteriaList.Rows[i].Cells["CriteriaSource3"].EditorComponent = GetGridCombo(GVar.ListSettingID.ReportSearchFormatCombo,
                                "IDFormatKey", "IDFormatDes", CriteriaLimitToList, cmunCombo, null, null);
                            tagrdCriteriaList.Rows[i].Cells["CriteriaLabel3"].Value = "Search Format ->";
                        }

                        tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].CellDisplayStyle = CellDisplayStyle.FullEditorDisplay;

                        if (TextBtnList.Contains(CriteriaNm))
                        {
                            TAUtil.TATextBoxEditor txt = null;
                            TAUtil.TAComboBox tacbo = null;
                            if (CriteriaNm == ItmCriteriaNm)
                            {
                                txt = GlobalUI.GetBrowseButton();
                                txt.EditorButtonClick += new EditorButtonEventHandler(this.GridCellEditorButtonClick);
                                txt.ContextMenu = cmunPopup;
                                tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].EditorComponent = txt;
                                tagrdCriteriaList.Rows[i].Cells["CriteriaSource2"].EditorComponent = txt;
                            }
                            else
                            {
                                tacbo = GetGridCombo(CriteriaListID, CriteriaValueMember, CriteriaDisplayMember,
                                    CriteriaLimitToList, cmunPopup, editorButton
                                    , new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.GridCellEditorButtonClick));

                                tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].EditorComponent = tacbo;
                                tagrdCriteriaList.Rows[i].Cells["CriteriaSource2"].EditorComponent = tacbo;
                            }

                            tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].Tag = CriteriaNm;
                            tagrdCriteriaList.Rows[i].Cells["CriteriaSource2"].Tag = CriteriaNm;

                            tagrdCriteriaList.Rows[i].Cells["CriteriaLabel2"].Value = "To";
                            tagrdCriteriaList.Rows[i].Cells["CriteriaLabel2"].TabStop = DefaultableBoolean.False;
                        }
  
                        else if (CriteriaListID.Trim() != string.Empty && CriteriaListID.Length > 4)
                        {

                            if (GFunc.CompareString("msglist", CriteriaType))
                            {

                                // Create ComboBox for First Column                          
                                TAUtil.TAComboBox tacbo = new TAUtil.TAComboBox();
                                tacbo.Visible = false;

                                // Set Data Source
                                DataTable dtCombo = BOLib.SYSList.GetMsgList((BOLib.GEnum.SYSMsgList)Convert.ToInt32(CriteriaListID));
                                tacbo.DataSource = dtCombo;
                                tacbo.DisplayMember = "DataDes";
                                tacbo.ValueMember = "MsgValue";
                                tacbo.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                foreach (UltraGridColumn col in tacbo.DisplayLayout.Bands[0].Columns)
                                    if (!col.Key.Equals(tacbo.DisplayMember))
                                        col.Hidden = true;
                                    else
                                    {
                                        col.Width = 250;
                                        col.Header.Caption = " ";
                                    }
                                this.tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].EditorComponent = tacbo;
                            }
                            else if (GFunc.CompareString(GVar.ReportCriteriaType.Range, CriteriaType))
                            {
                                TAUtil.TAComboBox tacboRange = GetGridCombo(CriteriaListID,
                                                            CriteriaValueMember, CriteriaDisplayMember, CriteriaLimitToList, null, null, null);

                                tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].EditorComponent = tacboRange;
                                tagrdCriteriaList.Rows[i].Cells["CriteriaSource2"].EditorComponent = tacboRange;

                                tagrdCriteriaList.Rows[i].Cells["CriteriaLabel2"].Value = "To";
                                tagrdCriteriaList.Rows[i].Cells["CriteriaLabel2"].TabStop = DefaultableBoolean.False;

                            }
                            else if (GFunc.CompareString(GVar.ReportCriteriaType.Range10, CriteriaType))
                            {

                                TAUtil.TAComboBox tacboRange10 = GetGridCombo(CriteriaListID,
                                                            CriteriaValueMember, CriteriaDisplayMember, CriteriaLimitToList, null, null, null);

                                tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].EditorComponent = tacboRange10;
                                tagrdCriteriaList.Rows[i].Cells["CriteriaSource2"].EditorComponent = tacboRange10;

                                tagrdCriteriaList.Rows[i].Cells["CriteriaLabel2"].Value = "To";
                                tagrdCriteriaList.Rows[i].Cells["CriteriaLabel2"].TabStop = DefaultableBoolean.False;

                            }
                            else if (GFunc.CompareString(GVar.ReportCriteriaType.Single, CriteriaType))
                            {
                                TAUtil.TAComboBox tacboSingle = GetGridCombo(CriteriaListID,
                                                            CriteriaValueMember, CriteriaDisplayMember, CriteriaLimitToList, null, null, null);

                                tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                                tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].EditorComponent = tacboSingle;


                            }
                        }
                        else if (CriteriaType.ToUpper() == GVar.ReportCriteriaType.SubFormSelection.ToUpper())
                        {
                            utabCriteria.Tabs[1].Visible = true;
                            obj.Hidden = true;
                            GlobalUI.Grid_Format(tagrdAdditionalCriteria, GVar.ListSettingID.SYSMsgListINTypeReport, true);
                            tagrdAdditionalCriteria.DisplayLayout.Bands[0].Columns["Checked"].Header.Caption = "";
                            tagrdAdditionalCriteria_CustomCellUpdate(null, null);
                        }
                        else
                        {
                            if (!GFunc.IsNE(CriteriaDataType))
                            {
                                switch (CriteriaDataType.ToUpper())
                                {
                                    case GVar.ReportCriteriaDataType.Date:
                                        TAUtil.TADateEditor tadt1 = new TAUtil.TADateEditor(true);
                                        tadt1.Format = "dd MMM yyyy";
                                        this.tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource1"].Format = "dd MMM yyyy";
                                        tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].EditorComponent = tadt1;
                                        tadt1.calendarContainer = this.tagrdCriteriaList;
                                        tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource1"].CellAppearance.TextHAlign = HAlign.Right;
                                        if (CriteriaType.ToUpper().Equals(GVar.ReportCriteriaType.Range.ToUpper()))
                                        {

                                            tagrdCriteriaList.Rows[i].Cells["CriteriaLabel2"].Value = "To";
                                            tagrdCriteriaList.Rows[i].Cells["CriteriaLabel2"].TabStop = DefaultableBoolean.False;
                                            tagrdCriteriaList.Rows[i].Cells["CriteriaSource2"].EditorComponent = tadt1;
                                            tadt1.calendarContainer = this.tagrdCriteriaList;
                                            this.tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource2"].Format = "dd MMM yyyy";
                                            tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource2"].CellAppearance.TextHAlign = HAlign.Right;
                                        }
                                        break;
                                    case GVar.ReportCriteriaDataType.Integer:
                                    case GVar.ReportCriteriaDataType.BigInteger:
                                    case GVar.ReportCriteriaDataType.IntegerPeriod:
                                        TAUtil.TANumericEditor taNu1 = new TAUtil.TANumericEditor();
                                        taNu1.Format = "0";

                                        tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].EditorComponent = taNu1;
                                        tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource1"].CellAppearance.TextHAlign = HAlign.Right;
                                        if (CriteriaType.ToUpper().Equals(GVar.ReportCriteriaType.Range))
                                        {
                                            TAUtil.TANumericEditor taNu2 = new TAUtil.TANumericEditor();

                                            taNu2.Format = "0";

                                            tagrdCriteriaList.Rows[i].Cells["CriteriaSource2"].EditorComponent = taNu2;
                                            tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource2"].CellAppearance.TextHAlign = HAlign.Right;
                                        }
                                        break;
                                    case GVar.ReportCriteriaDataType.Money:
                                    case GVar.ReportCriteriaDataType.Decimal:


                                        TAUtil.TANumericEditor taNu3 = new TAUtil.TANumericEditor();
                                        taNu3.Format = "0.00";
                                        tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].EditorComponent = taNu3;
                                        tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource1"].CellAppearance.TextHAlign = HAlign.Right;
                                        if (CriteriaType.ToUpper().Equals(GVar.ReportCriteriaType.Range))
                                        {
                                            TAUtil.TANumericEditor taNu4 = new TAUtil.TANumericEditor();
                                            taNu4.Format = "0.00";
                                            tagrdCriteriaList.Rows[i].Cells["CriteriaSource2"].EditorComponent = taNu4;
                                            tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource2"].CellAppearance.TextHAlign = HAlign.Right;
                                        }
                                        break;
                                    case GVar.ReportCriteriaDataType.Boolean:
                                        TAUtil.TACheckBoxEditor tachkBoolean = new TAUtil.TACheckBoxEditor();

                                        tachkBoolean.CheckAlign = ContentAlignment.MiddleCenter;
                                        tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].EditorComponent = tachkBoolean;
                                        tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource1"].CellAppearance.TextHAlign = HAlign.Center;

                                        break;
                                    default:
                                        tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource1"].CellAppearance.TextHAlign = HAlign.Left;
                                        if (CriteriaType.ToUpper().Equals(GVar.ReportCriteriaType.Range))
                                        {
                                            tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource2"].CellAppearance.TextHAlign = HAlign.Left;
                                        }
                                        break;
                                }
                            }
                        }

                        if (CriteriaHidden)
                        {
                            tagrdCriteriaList.Rows[i].Hidden = true;
                            continue;
                        }

                        tagrdCriteriaList.Rows[i].Cells["CriteriaLabel1"].Value = CriteriaLabel;
                        if (CriteriaLabel.Trim().Length > label1MaxWidth)
                            label1MaxWidth = CriteriaLabel.Trim().Length;

                        if (!GFunc.IsNE(tagrdCriteriaList.Rows[i].Cells["CriteriaDefaultValue"].Value))
                            tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].Value = tagrdCriteriaList.Rows[i].Cells["CriteriaDefaultValue"].Value;

                        if (CriteriaType.ToUpper().Equals(GVar.ReportCriteriaType.Range.ToUpper()))
                        {
                            tagrdCriteriaList.Rows[i].Cells["CriteriaLabel1"].Value = CriteriaLabel;
                        }
                        if (GFunc.CompareString(CriteriaDataType, GVar.ReportCriteriaDataType.Date) || GFunc.CompareString(CriteriaListID, "SYSPeriodAll"))
                        {
                            if (!this.IsMdiChild)// called from TODO                         
                                this.EnableButton(false);
                            else
                                this.EnableButton(true);
                        }
                    }
                }

                label1MaxWidth = (label1MaxWidth * (9 - (label1MaxWidth / 7))) + 30;
                FormatGrid(label1MaxWidth);
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
        }
        private void EnableButton(bool bVisable)
        {
            btnToday.Visible = bVisable;
            btnThisMonth.Visible = bVisable;
            btnThisYear.Visible = bVisable;
            btnThisFYear.Visible = bVisable;
            btnUseCurrentData.Visible = false;//currently always visiale false, we will implement later
        }

        private void FormatGrid(int label1MaxWidth)
        {
            label1MaxWidth += 10;
            int criteriaWidth = 150;
            utabCriteria.Tabs[0].Appearance.BackColor = tagrdCriteriaList.DisplayLayout.Appearance.BackColor;

            if (CriteriaRangeInclude && CriteriaSearchFormatInclude)
                tagrdCriteriaList.Width = label1MaxWidth + (criteriaWidth * 3) + 170 + 25;
            else if (CriteriaRangeInclude)
                tagrdCriteriaList.Width = label1MaxWidth + (criteriaWidth * 2) + 50 + 25;

            lvwRptFileList.Width = tagrdCriteriaList.Width;

            tagrdCriteriaList.DisplayLayout.Bands[0].ColHeadersVisible = false;

            foreach (UltraGridColumn col in this.tagrdCriteriaList.DisplayLayout.Bands[0].Columns)
            {
                if (!col.Key.Equals("CriteriaLabel1") && !col.Key.Equals("CriteriaLabel2") && !col.Key.Equals("CriteriaLabel3")
                    && !col.Key.Equals("CriteriaSource1") && !col.Key.Equals("CriteriaSource2") && !col.Key.Equals("CriteriaSource3"))
                    col.Hidden = true;
                else
                    col.AutoSizeEdit = DefaultableBoolean.True;

            }
            tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel1"].Width = label1MaxWidth;
            tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource1"].Width = criteriaWidth;

            if (CriteriaRangeInclude && CriteriaSearchFormatInclude)
            {
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel2"].Width = 50;
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel3"].Width = 120;

                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource2"].Width = criteriaWidth;
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource3"].Width = criteriaWidth;
            }
            else if (CriteriaRangeInclude)
            {
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel2"].Width = 50;
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource2"].Width = 150;

            }
            else if (CriteriaSearchFormatInclude)
            {
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel3"].Width = 150;
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource3"].Width = 150;
            }

            tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel1"].CellAppearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226))))); ;
            tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel1"].CellAppearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64))))); ;
            tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel1"].CellAppearance.FontData.Italic = DefaultableBoolean.True;
            tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel1"].CellAppearance.FontData.Name = "Calibri";
            tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel1"].CellAppearance.FontData.SizeInPoints = 10;
            tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel1"].CellActivation = Activation.ActivateOnly;
            tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel1"].TabStop = false;

            if (!CriteriaRangeInclude)
            {
                this.tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource2"].Hidden = true;
                this.tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel2"].Hidden = true;
            }
            else
            {
                this.tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource2"].Hidden = false;
                this.tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel2"].Hidden = false;

                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel2"].CellAppearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226))))); ;
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel2"].CellAppearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64))))); ;
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel2"].CellAppearance.FontData.Italic = DefaultableBoolean.True;
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel2"].CellAppearance.FontData.Name = "Calibri";
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel2"].CellAppearance.FontData.SizeInPoints = 10;
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel2"].CellActivation = Activation.ActivateOnly;
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel2"].CellAppearance.TextHAlign = HAlign.Center;
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel2"].TabStop = false;
            }

            if (!CriteriaSearchFormatInclude)
            {
                this.tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource3"].Hidden = true;
                this.tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel3"].Hidden = true;
            }
            else
            {
                this.tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaSource3"].Hidden = false;
                this.tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel3"].Hidden = false;

                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel3"].CellAppearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226))))); ;
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel3"].CellAppearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64))))); ;
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel3"].CellAppearance.FontData.Italic = DefaultableBoolean.True;
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel3"].CellAppearance.FontData.Name = "Calibri";
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel3"].CellAppearance.FontData.SizeInPoints = 9;
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel3"].CellActivation = Activation.ActivateOnly;
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel3"].CellAppearance.TextHAlign = HAlign.Center;
                tagrdCriteriaList.DisplayLayout.Bands[0].Columns["CriteriaLabel3"].TabStop = false;
            }

            foreach (UltraGridRow row in tagrdCriteriaList.Rows)
            {
                if (Convert.ToBoolean(objReportFactory.SYSRepCriterias.Rows[row.Index]["CriteriaHaveFormatSearch"]) == false)
                {
                    row.Cells["CriteriaSource3"].Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226))))); ;
                    row.Cells["CriteriaSource3"].Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64))))); ;
                    row.Cells["CriteriaSource3"].Appearance.FontData.Italic = DefaultableBoolean.True;
                    row.Cells["CriteriaSource3"].Appearance.FontData.Name = "Calibri";
                    row.Cells["CriteriaSource3"].Activation = Activation.ActivateOnly;
                    row.Cells["CriteriaSource3"].TabStop = DefaultableBoolean.False;
                }
                else
                {
                    row.Cells["CriteriaSource3"].Activation = Activation.AllowEdit;
                }
                if (objReportFactory.SYSRepCriterias.Rows[row.Index]["CriteriaType"].ToString().Contains("Range") == false)
                {
                    row.Cells["CriteriaSource2"].Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226))))); ;
                    row.Cells["CriteriaSource2"].Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64))))); ;
                    row.Cells["CriteriaSource2"].Appearance.FontData.Name = "Calibri";
                    row.Cells["CriteriaSource2"].Appearance.FontData.Italic = DefaultableBoolean.True;
                    row.Cells["CriteriaSource2"].Activation = Activation.ActivateOnly;
                    row.Cells["CriteriaSource2"].TabStop = DefaultableBoolean.False;
                }
                else
                {
                    row.Cells["CriteriaSource2"].Activation = Activation.AllowEdit;
                }
            }
            //Set Appearence           

            //Row
            Infragistics.Win.Appearance appearence_Row = new Infragistics.Win.Appearance();
            appearence_Row.FontData.Name = "Calibri";
            appearence_Row.FontData.SizeInPoints = 11F;
            appearence_Row.ForeColor = System.Drawing.Color.Black;
            appearence_Row.TextHAlignAsString = "LEFT";
            appearence_Row.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearence_Row.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            tagrdCriteriaList.DisplayLayout.Override.RowAppearance = appearence_Row;

            //Cell
            Infragistics.Win.Appearance appearence_Cell = new Infragistics.Win.Appearance();
            appearence_Cell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226))))); ;
            appearence_Cell.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226))))); ;
            tagrdCriteriaList.DisplayLayout.Override.CellAppearance = appearence_Cell;

            //Row Selector
            Infragistics.Win.Appearance appearence_RowSelector = new Infragistics.Win.Appearance();
            appearence_RowSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearence_RowSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            tagrdCriteriaList.DisplayLayout.Override.RowSelectorAppearance = appearence_RowSelector;

            //Appearence
            Infragistics.Win.Appearance appearence = new Infragistics.Win.Appearance();
            appearence.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearence.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            tagrdCriteriaList.DisplayLayout.Appearance = appearence;

            //Row Header Selector
            Infragistics.Win.Appearance appearence_RowHeaderSelector = new Infragistics.Win.Appearance();
            appearence_RowHeaderSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearence_RowHeaderSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            tagrdCriteriaList.DisplayLayout.Override.RowSelectorHeaderAppearance = appearence_RowHeaderSelector;


            //tagrdCriteriaList.DisplayLayout.AutoFitStyle = AutoFitStyle.None;
            tagrdCriteriaList.TextRenderingMode = TextRenderingMode.GDI;
        }
        private void FormatAvailableGrid()
        {
            try
            {
                tagrdReports.DisplayLayout.Bands[0].Columns["RepKey"].Hidden = true;
                tagrdReports.DisplayLayout.Bands[0].Columns["RepGrp"].Hidden = true;
                tagrdReports.DisplayLayout.Bands[0].Columns["RepGrpDes"].Hidden = true;
                tagrdReports.DisplayLayout.Bands[0].Columns["RPTRecordSource1"].Hidden = true;
                tagrdReports.DisplayLayout.Bands[0].Columns["RPTname1"].Hidden = true;

                tagrdReports.DisplayLayout.Bands[0].Columns["RepDes"].Header.Caption = "Available Reports";
                tagrdReports.DisplayLayout.Bands[0].Columns["RepDes"].CellActivation = Activation.ActivateOnly;

                tagrdReports.DisplayLayout.Bands[0].Columns["RepDes"].Header.Appearance.ForeColor = System.Drawing.Color.Black;
                tagrdReports.DisplayLayout.Bands[0].Columns["RepDes"].Header.Appearance.FontData.Bold = DefaultableBoolean.True;
                tagrdReports.DisplayLayout.Bands[0].Columns["RepDes"].Width = 400;

                if (ShowReportsOption.CheckedIndex == 0)
                {
                    this.tagrdReports.DisplayLayout.Bands[0].SortedColumns.Add("RepGrpDes", false, true);
                    this.tagrdReports.DisplayLayout.Bands[0].Columns["RepGrpDes"].GroupByMode = GroupByMode.Value;

                    this.tagrdReports.DisplayLayout.Bands[0].Override.GroupByRowDescriptionMask = "[value] ([count] reports)";

                    this.tagrdReports.DisplayLayout.Bands[0].Override.GroupByRowPadding = 5;
                    this.tagrdReports.DisplayLayout.Bands[0].Override.GroupByColumnAppearance.FontData.SizeInPoints = 9;
                }
                else
                    this.tagrdReports.DisplayLayout.Bands[0].SortedColumns.Clear();

                tagrdReports.ActiveRowScrollRegion.Scrollbar = Infragistics.Win.UltraWinGrid.Scrollbar.Show;
                this.tagrdReports.Refresh();
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
                tagrdReports.DisplayLayout.Override.HeaderAppearance = appearence_Header;

                //Row
                Infragistics.Win.Appearance appearence_Row = new Infragistics.Win.Appearance();
                appearence_Row.FontData.Name = "Calibri";
                appearence_Row.FontData.SizeInPoints = 11F;
                appearence_Row.ForeColor = System.Drawing.Color.Black;
                appearence_Row.TextHAlignAsString = "LEFT";
                appearence_Row.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Row.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdReports.DisplayLayout.Override.RowAppearance = appearence_Row;

                //Cell
                Infragistics.Win.Appearance appearence_Cell = new Infragistics.Win.Appearance();
                appearence_Cell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Cell.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdReports.DisplayLayout.Override.CellAppearance = appearence_Cell;

                //Row Selector
                Infragistics.Win.Appearance appearence_RowSelector = new Infragistics.Win.Appearance();
                appearence_RowSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_RowSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdReports.DisplayLayout.Override.RowSelectorAppearance = appearence_RowSelector;

                //Appearence
                Infragistics.Win.Appearance appearence = new Infragistics.Win.Appearance();
                appearence.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdReports.DisplayLayout.Appearance = appearence;

                //Row Header Selector
                Infragistics.Win.Appearance appearence_RowHeaderSelector = new Infragistics.Win.Appearance();
                appearence_RowHeaderSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_RowHeaderSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrdReports.DisplayLayout.Override.RowSelectorHeaderAppearance = appearence_RowHeaderSelector;

                //tagrdReports.DisplayLayout.AutoFitStyle = AutoFitStyle.None;
                tagrdReports.TextRenderingMode = TextRenderingMode.GDI;
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

        private void FilterDataByPermission()
        {
            string filterStr = ""; //Obsolete Item list can be seen by anyone
            //RepGrp Numbers refer to Sys_MsgList DataGrp=84, RepGrp
            string RepAccGroup = "100,110,120,";
            string RepCustroup = "200,210,220,";
            string RepVendGroup = "300,310,320,";
            string RepItemGroup = "400,410,420,430,440,";
            string RepSalesGroup = "230,240,250,";
            string RepPurchaseGroup = "330,340,";
            string RepJobGroup = "500,";
            string RepAdminGroup = "600,700,800,";
            string RepDocIN = "450,";
            string RepDocConsignment = "460,";

            

            if (SECPermUtility.Perform(GVar.PermissionID.Accounts_Reports, false))
                filterStr += RepAccGroup;
            if (SECPermUtility.Perform(GVar.PermissionID.Customer_Reports, false))
                filterStr += RepCustroup;
            if (SECPermUtility.Perform(GVar.PermissionID.Vendor_Reports, false))
                filterStr += RepVendGroup;
            if (SECPermUtility.Perform(GVar.PermissionID.Inventory_Reports, false))
                filterStr += RepItemGroup;
            if (SECPermUtility.Perform(GVar.PermissionID.AR_Documents_Reports, false))
                filterStr += RepSalesGroup;
            if (SECPermUtility.Perform(GVar.PermissionID.AP_Documents_Reports, false))
                filterStr += RepPurchaseGroup;
            if (SECPermUtility.Perform(GVar.PermissionID.Job_Report, false))
                filterStr += RepJobGroup;
            if (SECPermUtility.Perform(GVar.PermissionID.Administration_Reports, false))
                filterStr += RepAdminGroup;
            if (SECPermUtility.Perform(GVar.PermissionID.Inventory_Documents_Reports, false))
                filterStr += RepDocIN;
            if (SECPermUtility.Perform(GVar.PermissionID.Consignment_Documents_Reports, false))
                filterStr += RepDocConsignment;
            if (SECPermUtility.Perform("RepSalesTrack", false))
                filterStr += "3000,";
            if (SECPermUtility.Perform("RepSalesComm", false))
                filterStr += "3100,";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Option", 1));
            list.Add(new SqlParameter("@EmID", AppInfor.CurrentUserID));
            DataTable dt = GFunc.ExecuteProc("MSTUNIT_GET", list);
            string accessTeams = "";// GFunc.NEStr(dt.Rows[0]["AccessTeam"], "");

            if (dt.Rows.Count > 0)
            {
                accessTeams = GFunc.NEStr(dt.Rows[0]["AccessTeam"], "").ToUpper();
            }           

            if (filterStr != "")
            {
                filterStr = filterStr.Substring(0, filterStr.Length - 1);

                if (filterStr.Contains("3000"))
                {
                     if (!accessTeams.Equals("") && !accessTeams.Equals("ALL"))
                        filterStr = "(RepGrp IN (" + filterStr + ") AND RepKey<>1464 OR (RepKey=1467 OR RepKey=1466))";                   
                    else
                        filterStr = "RepGrp IN (" + filterStr + ")"; 
                }
                else
                    filterStr = "RepGrp IN (" + filterStr + ")"; 

                objReportFactory.ROSYSRep.DefaultView.RowFilter = filterStr;
            }
            else
                objReportFactory.ROSYSRep.DefaultView.RowFilter = "RepGrp=-1";
                
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

        private void tsbCopyToRptOutputDirectory_Click(object sender, EventArgs e)
        {
            if (lvwRptFileList.Items.Count > 0)
            {
                UltraListViewItem item = null;

                if (lvwRptFileList.SelectedItems.Count == 0)
                    item = lvwRptFileList.Items[0];
                else
                    item = lvwRptFileList.SelectedItems[0];

                if (item.SubItems["RptNm"].Value.ToString() != "NA" || item.SubItems["RptNm"].Value.ToString() != "")
                {
                    System.IO.File.Copy(Application.ExecutablePath.Split(new string[] { "bin" }, StringSplitOptions.None)[0] + "Reports\\" + item.SubItems["RptNm"].Value, Application.StartupPath + @"\Reports\" + item.SubItems["RptNm"].Value, true);
                }
            }
            
        }
    }
}
