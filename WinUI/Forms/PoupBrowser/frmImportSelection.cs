using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOLib;
using System.Data.SqlClient;
using Infragistics.Win.UltraWinGrid;
using TAUtil;

namespace WinUI
{
    public partial class frmImportSelection : Form
    {
        TAUtil.TAExcelImport _objExcelImport = null;
        DataTable dtExcelData = null;
        DataTable dtRefJobEst = null;

        int CodeKey = 0;
        int JobKey = 0;
        decimal JobEstSN = 0;
        int JobEstKey = 0;
        bool ReOrderSN = false;
        UltraGridRow LastDefaultItem = null;

        private string ContextMenuSetting = "";
        public frmImportSelection()
        {
            InitializeComponent();
           
        }
        public frmImportSelection(ref DataTable JobEstDetailTable, int SystemCodeKey, int JobKey, decimal LastSN, int LastEstKey, bool ReOrderSN)
        {
            InitializeComponent();
            dtRefJobEst = JobEstDetailTable;
            CodeKey = SystemCodeKey;
            JobEstSN = LastSN;
            JobEstKey = LastEstKey;
            this.JobKey = JobKey;
            this.ReOrderSN = ReOrderSN;
        }

        //From Events
        private void frmImportSelection_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {               
                GlobalUI.FormGrids_Set(this, (int)GEnum.SystemCode.Job, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)GEnum.SystemCode.Job, this.Name);

                GlobalUI.Combos_Fill(this, (int)GEnum.SystemCode.Job);

                ultraExpandableGroupBox2.Enabled = false;
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
        private void frmImportSelection_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                _objExcelImport = null;
                if (dtExcelData != null)
                {
                    dtExcelData.Dispose();
                    dtExcelData = null;
                }
                this.Hide();
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
        private void frmImportSelection_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)GEnum.SystemCode.Job);
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

        //Button Events
        private void tsbSelectAllItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                DataTable dt = tagrdExcelData.DataSource as DataTable;
               // dt.DefaultView.RowFilter = "EstItmKey=0 OR EstItmKey IS NULL";
                IEnumerable<DataRow> dtFilter = dt.AsEnumerable().Where(r => GFunc.IsNEZ(r.Field<int?>("EstItmKey")));

                foreach (DataRow dr in dtFilter)
                {
                    //if (dr.IsNew)
                    //    continue;
                    if (!GFunc.NEBool(dr["AddItem"], false))
                        dr["AddItem"] = true;
                }
               // dt.DefaultView.RowFilter = "";
                tagrdExcelData.DataBind();
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
        private void tsbUnselectAllItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                DataTable dt = tagrdExcelData.DataSource as DataTable;
                //dt.DefaultView.RowFilter = "EstItmKey=0 OR EstItmKey IS NULL";
                IEnumerable<DataRow> dtFilter = dt.AsEnumerable().Where(r => GFunc.IsNEZ(r.Field<int?>("EstItmKey")));

                foreach (DataRow dr in dtFilter)
                {
                    //if (dr.IsNew)
                    //    continue;
                    if (GFunc.NEBool(dr["AddItem"], false))
                        dr["AddItem"] = 0;
                }
               // dt.DefaultView.RowFilter = "";
                tagrdExcelData.DataBind();
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
        private void tsbSelectAllUOM_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                DataTable dt = tagrdExcelData.DataSource as DataTable;
               // dt.DefaultView.RowFilter = "EstUOMKey=0 OR EstUOMKey IS NULL";               
                IEnumerable<DataRow> dtFilter = dt.AsEnumerable().Where(r => GFunc.IsNEZ(r.Field<int?>("EstUOMKey")));

                foreach (DataRow dr in dtFilter)
                {
                    //if (dr.IsNew)
                    //    continue;
                    if (!GFunc.NEBool(dr["AddUOM"], false))
                        dr["AddUOM"] = true;
                }
                //dt.DefaultView.RowFilter = "";
                tagrdExcelData.DataBind();
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
        private void tsbUnSelectAllUOM_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                DataTable dt = tagrdExcelData.DataSource as DataTable;
                // dt.DefaultView.RowFilter = "EstUOMKey=0 OR EstUOMKey IS NULL";               
                IEnumerable<DataRow> dtFilter = dt.AsEnumerable().Where(r => GFunc.IsNEZ(r.Field<int?>("EstUOMKey")));

                foreach (DataRow dr in dtFilter)
                {
                    //if (dr.IsNew)
                    //    continue;              
                    if (GFunc.NEBool(dr["AddUOM"], false))
                        dr["AddUOM"] = false;
                }
               //dt.DefaultView.RowFilter = "";
                tagrdExcelData.DataBind();
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
        private void tsbExport_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                int count = tsbExport.Tag == null ? 1 : (int)tsbExport.Tag;
                GlobalUI.Export(tagrdExcelData, ref count);
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void tsbImport_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {

                tagrdExcelData.PerformAction(UltraGridAction.ExitEditMode);
                tagrdExcelData.UpdateData();

                if (GFunc.ValidateUserAccess(dtExcelData, this.CodeKey))
                {
                    #region Add Items and UOMs

                    DataTable dt = tagrdExcelData.DataSource as DataTable;
                    //dt.DefaultView.RowFilter = "AddUOM=1";
                    IEnumerable<DataRow> dtFilter = dt.AsEnumerable().Where(r => r.Field<bool?>("AddUOM") == true);
                    if (dtFilter != null)
                    {
                        if (dtFilter.Count() > 0)
                        {
                            REFUOMFactory objUOMFactory = new REFUOMFactory(GEnum.InstanceMode.Normal);
                            if (objUOMFactory.GUID > 0)
                            {
                                // dt.DefaultView.RowFilter = "AddUOM=1";

                                foreach (DataRow dr in dtFilter)
                                {
                                    //if (dr.IsNew)
                                    //    continue;
                                    objUOMFactory.New();
                                    objUOMFactory.ObjREFUOM.UOMID = dr["UOM"].ToString();
                                    objUOMFactory.ObjREFUOM.UOMShw = dr["UOM"].ToString();
                                    if (objUOMFactory.Save())
                                        dr["EstUOMKey"] = objUOMFactory.ObjREFUOM.UOMKey;
                                }
                            }
                            objUOMFactory.Dispose();
                        }
                    }
                   

                   // dt.DefaultView.RowFilter = "AddItem=1";
                    dtFilter = dt.AsEnumerable().Where(r => r.Field<bool?>("AddItem") == true);
                    if (dtFilter != null)
                    {
                        if (dtFilter.Count() > 0)
                        {
                            MSTItmFactory objItmFactory = new MSTItmFactory(GEnum.InstanceMode.Normal);
                            if (objItmFactory.GUID > 0)
                            {
                                foreach (DataRow dr in dtFilter)
                                {
                                    objItmFactory.New((int)GEnum.ItemType.Non_Stock);
                                    objItmFactory.ObjMSTItm.ItmID = GFunc.NEStr(dr["ItmID"], "");
                                    objItmFactory.ObjMSTItm.ItmDes = GFunc.NEStr(dr["EstItmDes"], "");
                                    objItmFactory.ObjMSTItm.BUOMKey = GFunc.NEInt(dr["EstUOMKey"], GFunc.NEInt(defaultItem.SelectedRow.Cells["BUOMKey"].Value, 0));

                                    if (objItmFactory.Save())
                                    {
                                        dr["EstItmKey"] = objItmFactory.ObjMSTItm.ItmKey;
                                        dr["EstItmKeySelect"] = objItmFactory.ObjMSTItm.ItmKey;
                                    }
                                }
                            }
                            objItmFactory.Dispose();
                        }
                    }
                   

                    #endregion Add Items and UOMs

                    #region Importing to Job Estimate Detail Tables

                   // dtExcelData.DefaultView.RowFilter = "";

                    if (ClearData.Checked)
                    {
                        dtRefJobEst.Rows.Clear();
                        this.JobEstKey = 0;
                        this.JobEstSN = 0;
                    }

                    foreach (DataRow drv in dtExcelData.Rows)
                    {
                        JobEstKey++;
                        JobEstSN++;

                        DataRow dr = dtRefJobEst.NewRow();
                        dr["JobKey"] = JobKey;
                        dr["JobEstKey"] = JobEstKey;
                        dr["EstSN"] = JobEstSN;

                        dr["EstUOMKey"] = GFunc.IsNEZ(drv["EstUOMKey"]) ? (GFunc.IsNEZ(drv["ItmBUOMKey"]) ?
                            (defaultItem.SelectedRow != null ? defaultItem.SelectedRow.Cells["BUOMKey"].Value : 0) : drv["ItmBUOMKey"]) : drv["EstUOMKey"];

                        if (GFunc.IsNEZ(drv["EstItmKey"]))
                        {
                            if (defaultItem.SelectedRow != null)
                            {
                                dr["EstItmKey"] = defaultItem.SelectedRow.Cells["Key"].Value;
                                dr["EstItmID"] = defaultItem.SelectedRow.Cells["ID"].Value;
                                dr["EstItmType"] = (int)GEnum.ItemType.Non_Stock;
                                dr["EstItmKeySelect"] = dr["EstItmKey"];
                                dr["EstItmDes"] = GFunc.IsNE(drv["EstItmDes"]) ? defaultItem.SelectedRow.Cells["Des"].Value : drv["EstItmDes"];
                            }
                        }
                        else
                        {
                            dr["EstItmKey"] = drv["EstItmKey"];
                            dr["EstItmID"] = drv["ItmID"];
                            dr["EstItmType"] = drv["EstItmType"];
                            dr["EstItmKeySelect"] = dr["EstItmKey"];
                            dr["EstItmDes"] = drv["EstItmDes"];
                        }

                        dr["JobPhaseKey"] = drv["JobPhaseKey"];
                        dr["JobTaskKey"] = drv["JobTaskKey"];
                        dr["JobCostTypeKey"] = drv["JobCostTypeKey"];
                        dr["DocVendorKey"] = drv["DocVendorKey"];

                        dr["EstItmRem"] = drv["EstItmRem"];
                        dr["EstQty"] = drv["EstQty"];

                        dr["DocCurrKey"] = drv["DocCurrKey"];
                        dr["DocCurrRate"] = drv["DocCurrRate"];
                        dr["EstConRate"] = drv["EstConRate"];
                        dr["EstCostF"] = drv["EstCostF"];
                        dr["EstCostH"] = drv["EstCostH"];
                        dr["EstAmtF"] = drv["EstAmtF"];
                        dr["EstAmtH"] = drv["EstAmtH"];
                        dr["DocDK"] = drv["DocDK"];
                        dr["DocDItm"] = drv["DocDItm"];
                        dr["DocID"] = drv["DocID"];
                        dr["DocDes"] = drv["DocDes"];
                        dr["DocETD"] = drv["DocETD"];
                        dr["TransmitMode"] = drv["TransmitMode"];
                        dr["Attention"] = drv["Attention"];
                        dr["emailAddr"] = drv["emailAddr"];
                        dr["FaxNumber"] = drv["FaxNumber"];
                        dr["TransmitStatus"] = drv["TransmitStatus"];
                        dr["Selected"] = false;
                        dtRefJobEst.Rows.InsertAt(dr, Convert.ToInt32(JobEstSN) - 1);
                    }

                    if (ReOrderSN)
                    {
                        int i = Convert.ToInt32(JobEstSN) + 1;
                        int j = JobEstKey + 1;

                        for (; i <= dtRefJobEst.Rows.Count; i++)
                        {
                            dtRefJobEst.Rows[i - 1]["EstSN"] = i;
                            dtRefJobEst.Rows[i - 1]["JobEstKey"] = j;
                            j++;
                        }
                    }

                    if (dtExcelData.Rows.Count > 1)
                        MsgBox.Show(dtExcelData.Rows.Count + " Records are imported successfully.", GEnum.MsgBoxIcon.Information, GEnum.MsgBoxButton.OK);
                    else
                        MsgBox.Show(dtExcelData.Rows.Count + " is imported successfully.", GEnum.MsgBoxIcon.Information, GEnum.MsgBoxButton.OK);

                    #endregion Importing to Job Estimate Detail Tables

                    this.DialogResult = DialogResult.OK;
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
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void btnRetrieveData_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (Validation())
                {                    
                    this.GetExcelData(ExcelSheets.Text, GFunc.NEInt(defaultItem.Value, 0));
                    tagrdExcelData.DataSource = dtExcelData;
                    this.FormSetting();
                    ultraExpandableGroupBox2.Enabled = true;

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

        //Control Events
        private void Combo_ItemNotInList(object sender, Infragistics.Win.UltraWinEditors.ValidationErrorEventArgs e)
        {
            GlobalUI.ItemNotInList(sender as TAUtil.TAComboBox, e, false, 0);
        }
        private void defaultItem_CustomUpdate(object sender, CancelEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (!GFunc.IsNEZ(defaultItem.Value) && dtExcelData != null && LastDefaultItem != null)//if default item is changed after retrieving excel data, update the default in existing excel data
                {
                    if (defaultItem.Value != LastDefaultItem.Cells["Key"].Value)
                        if (dtExcelData.Rows.Count > 0)
                        {
                            DataTable dt = defaultItem.DataSource as DataTable;
                            DataRow drUOM = dt.Select("Key=" + defaultItem.Value.ToString())[0];

                            //dtExcelData.DefaultView.RowFilter = "IsUOMDefault=1";
                            IEnumerable<DataRow> dtFilter = dtExcelData.AsEnumerable().Where(r => r.Field<int?>("IsUOMDefault") == 1);

                            foreach (DataRow drv in dtFilter)
                            {
                                drv["EstUOMKey"] = drUOM["BUOMKey"];
                            }

                            //dtExcelData.DefaultView.RowFilter = "IsItemDefault=1";
                            dtFilter = dtExcelData.AsEnumerable().Where(r => r.Field<int?>("IsItemDefault") == 1);
                            foreach (DataRow drv in dtFilter)
                            {
                                //if (drv.IsNew)
                                //    continue;
                                drv["EstItmKey"] = defaultItem.Value;
                                drv["EstItmKeySelect"] = defaultItem.Value;
                                drv["ItmID"] = defaultItem.Text;
                            }
                           // dtExcelData.DefaultView.RowFilter = "";
                            tagrdExcelData.DataBind();
                        }
                }
                LastDefaultItem = defaultItem.SelectedRow;
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
        private void Selection_CustomUpdate(object sender, CancelEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                tagrdExcelData.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();

                switch (GFunc.NEInt(Selection.Value, 0))
                {
                    case 1:
                        tagrdExcelData.DisplayLayout.Bands[0].ColumnFilters["AddItem"].FilterConditions.Add(Infragistics.Win.UltraWinGrid.FilterComparisionOperator.Equals, DBNull.Value);
                        tagrdExcelData.DisplayLayout.Bands[0].ColumnFilters["AddUOM"].FilterConditions.Add(Infragistics.Win.UltraWinGrid.FilterComparisionOperator.Equals, DBNull.Value);
                        tagrdExcelData.DisplayLayout.Bands[0].ColumnFilters.LogicalOperator = Infragistics.Win.UltraWinGrid.FilterLogicalOperator.And;
                        break;
                    case 2:
                        tagrdExcelData.DisplayLayout.Bands[0].ColumnFilters["AddItem"].FilterConditions.Add(Infragistics.Win.UltraWinGrid.FilterComparisionOperator.NotEquals, DBNull.Value);
                        tagrdExcelData.DisplayLayout.Bands[0].ColumnFilters["AddUOM"].FilterConditions.Add(Infragistics.Win.UltraWinGrid.FilterComparisionOperator.NotEquals, DBNull.Value);
                        tagrdExcelData.DisplayLayout.Bands[0].ColumnFilters.LogicalOperator = Infragistics.Win.UltraWinGrid.FilterLogicalOperator.Or;
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void ExcelPath_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            OpenFileDialog oDlg = new OpenFileDialog();
            oDlg.Filter = "Excel files (*.xls)|*.xls|(*.xlsx)|*.xlsx";
            DialogResult dlg = oDlg.ShowDialog();

            if (dlg == DialogResult.OK)
            {
                ExcelPath.SetValueTrigger(oDlg.FileName, false);
            }
            try
            {
                _objExcelImport = new TAUtil.TAExcelImport(ExcelPath.Text);
                DataTable dt = new DataTable();
                dt.Columns.Add("ValueCol");
                dt.Columns.Add("Excel Sheet");

                int i = 0;
                if (!GFunc.IsNE(_objExcelImport.ExcelSheets))
                {
                    foreach (string sheet in _objExcelImport.ExcelSheets)
                    {
                        dt.Rows.Add(i++, sheet);
                    }
                }
                ExcelSheets.DataSource = dt;
                ExcelSheets.ValueMember = "ValueCol";
                ExcelSheets.DisplayMember = "Excel Sheet";
                ExcelSheets.SetValueTrigger(0, false);
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
                oDlg = null;
                this.Cursor = Cursors.Default;
            }
        }

        //Functions
        private void FormSetting()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {   
                Selection.SetValueTrigger(0,false);

                if (!tagrdExcelData.DisplayLayout.Bands[0].Columns["AddItem"].Hidden || !tagrdExcelData.DisplayLayout.Bands[0].Columns["AddUOM"].Hidden)
                {
                    foreach (UltraGridColumn col in tagrdExcelData.DisplayLayout.Bands[0].Columns)
                        if (GFunc.CompareString(col.Key, "AddItem") == false && GFunc.CompareString(col.Key, "AddUOM") == false)
                            col.CellActivation = Activation.ActivateOnly;
                }
                else
                    tagrdExcelData.DisplayLayout.Bands[0].Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;

                tagrdExcelData.DisplayLayout.Bands[0].Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
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
            }
        }
      
        private void GetExcelData(string sheetName, int defaultItmKey)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                dtExcelData = _objExcelImport.GetExcelData(sheetName);
                //string rowFilter = "";

                ////filter blank rows
                //foreach (DataColumn dc in dtExcelData.Columns)
                //{
                //    if (dc.ColumnName.Trim() != string.Empty && dc.DataType == typeof(string))
                //        rowFilter = rowFilter + " len([" + dc.ColumnName.Trim() + "]) > 0 OR ";
                //}

                //dtExcelData.DefaultView.RowFilter = rowFilter.Substring(0, rowFilter.LastIndexOf("OR"));
                //dtExcelData = dtExcelData.DefaultView.ToTable();

                //try to exclude blank rows which are aldy deleted in Excel
                dtExcelData = dtExcelData.AsEnumerable().Where(row => row.ItemArray.Any(field => !(field is DBNull))).CopyToDataTable();

                if (GFunc.ValidateExcelData(dtExcelData, this.CodeKey))
                {
                    string XMLformat = "";

                    string itmTypes = GFunc.GetItmTypeGyDocCode(this.CodeKey);

                    List<SqlParameter> paraList = new List<SqlParameter>();

                    paraList.Add(new SqlParameter("@DocCodeKey", this.CodeKey));
                    paraList.Add(new SqlParameter("@DefaultItmKey", this.defaultItem.Value));
                    paraList.Add(new SqlParameter("@DefaultItmID", this.defaultItem.Text));
                    paraList.Add(new SqlParameter("@userKey", AppInfor.CurrentUserKey));
                    paraList.Add(new SqlParameter("@ItmType", itmTypes));

                    XMLformat = GFunc.ConvertDataTableToXML(dtExcelData);

                    paraList.Add(new SqlParameter("@xmlExcelData", XMLformat));

                    dtExcelData = GFunc.ExecuteProc("JobBom_Import", paraList);

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
                this.Cursor = Cursors.Default;
            }
        }
        private bool Validation()
        {
            if (!System.IO.File.Exists(ExcelPath.Text))
            {
                MsgBox.Show("Invalid File Path. Please select a correct excel file");
                ExcelPath.Focus();
                return false;
            }

            if (ExcelSheets.Text.Trim() == string.Empty)
            {
                MsgBox.Show("Invalid Excel Sheet. Please select a work sheet.");
                ExcelSheets.Focus();
                return false;
            }

            if (GFunc.IsNEZ(defaultItem.Value))
            {
                MsgBox.Show("Please select a default item.");
                defaultItem.Focus();
                return false;
            }
            return true;
        }

        //Set Error Methods
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
    }
}
