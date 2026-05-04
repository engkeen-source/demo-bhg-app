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
using TAUtil;

namespace WinUI
{
    public partial class frmExcelImport : Form
    {
        #region Local Variables

        TAUtil.TAExcelImport objExcelImport = null;
        public GVar.ListEvent_CopyRecord CopyRecordEvent = null;
        private int DocCodeKey;                          //Caller DocCodeKey

        #endregion
        public frmExcelImport(int pDocCodeKey)
        {
            InitializeComponent();
            this.DocCodeKey = pDocCodeKey;
        }

        //Form Events
        private void frmExcelImport_Load(object sender, EventArgs e)
        {
            try
            {
                Layout_Process();
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
        private void frmExcelImport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)GEnum.SystemCode.Stock_Count);
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
            
        }//CodeCompleted

        //Menu Strip Event
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void tsbImport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtItems = this.GetExcelData(ExcelSheets.Text.ToString());

                if (dtItems != null && dtItems.Rows.Count > 0)
                {
                    this.CopyRecordEvent.Invoke(GEnum.CopyOption.Import, 0, 0, dtItems, chkOverwrite.Checked);
                    this.Close();
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

        //Control Event
        private void ExcelPath_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            OpenFileDialog oDlg = new OpenFileDialog();           

            
            try
            {
                oDlg.Filter = "Excel files (*.xlsx)|*.xlsx|(*.xls)|*.xls";
                DialogResult dlg = oDlg.ShowDialog();
                if (dlg == DialogResult.OK)
                {
                    ExcelPath.SetValueTrigger(oDlg.FileName, false);
                }

                objExcelImport = new TAUtil.TAExcelImport(ExcelPath.Text);
                DataTable dt = new DataTable();
                dt.Columns.Add("ValueCol");
                dt.Columns.Add("Excel Sheet");

                int i = 0;
                if (!GFunc.IsNE(objExcelImport.ExcelSheets))
                {
                    foreach (string sheet in objExcelImport.ExcelSheets)
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
        private DataTable GetExcelData(string sheetName)
        {
            try
            {
                string defaultItemUOMID = string.Empty;
              //  string rowFilter = "";

                //Retrieve data from Excel Sheet as DataTable
                DataTable dtExcelData = objExcelImport.GetExcelData(sheetName);

                //try to exclude blank rows which are aldy deleted in Excel
                dtExcelData = dtExcelData.AsEnumerable().Where(row => row.ItemArray.Any(field => !(field is DBNull))).CopyToDataTable();

                //building filter string: logic to achieve=> In a row, at least one column must have data
                //foreach (DataColumn dc in dtExcelData.Columns)
                //{
                //    rowFilter = rowFilter + " len([" + dc.ColumnName + "]) > 0 OR ";
                //}
                ////Remove the extra last word "OR"
                //rowFilter = rowFilter.Substring(0, rowFilter.LastIndexOf("OR"));

                //try
                //{
                //    //filter blank rows
                //    dtExcelData.DefaultView.RowFilter = rowFilter;
                //    //At this stage, blank rows are excluded in dtExcelData
                //    dtExcelData = dtExcelData.DefaultView.ToTable();
                //}
                //catch (Exception ex)
                //{
                //    MsgBox.Show("Unable to import.\n\nThere are blank rows between datas in your source excel file.");
                //    return null;
                //}                

                //Validate structure and data according to pre-definition in SQL Table SyS_DocItem_ImportStructure
                if (GFunc.ValidateExcelData(dtExcelData, DocCodeKey) == false)
                    return null;

                return dtExcelData;
            }
            catch (Exception ex)
            {
                throw this.Error(ex, false);
            }
            finally
            {
                GlobalUI.bRuningImport = false;
                DocUtility.bRuningImport = false;
            }

        }
        private void Layout_Process()
        {
            #region "Set Layout"
            tabImport.TabPages.Clear();
            switch (DocCodeKey)
            {
                case (int)GEnum.SystemCode.Stock_Count:
                    tabImport.TabPages.Add(tbStockCount);
                    break;
            }
            #endregion

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
