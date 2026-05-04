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
using Infragistics.Win;
using System.Collections;
using Microsoft.VisualBasic;
using TAUtil;

namespace WinUI
{
    public partial class frmKeyCustomersImport : Form
    {
        private BOLib.KeyCustomerFactoryImport objKeyCustomerFactoryimport = null;
        private int TgtDC;
        TAUtil.TAExcelImport objExcelImport = null;
       
        private bool formClose = false;   

       

        public frmKeyCustomersImport()
        {
            InitializeComponent();
        }
       
        private void frmKeyCustomersImport_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {

               //Call Initialization
                this.objKeyCustomerFactoryimport = new BOLib.KeyCustomerFactoryImport(BOLib.GEnum.InstanceMode.Normal);                
                if (objKeyCustomerFactoryimport.GUID <= 0)
                {
                    formClose = true;
                    return;
                }
                TgtDC = (int)objKeyCustomerFactoryimport.ConstantCodeKey;
                GlobalUI.Combos_Fill(this, TgtDC);
                cmdBudgetYear.Value = DateTime.Now.Year.ToString();

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

        private void ExcelPath_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            DataTable dtforclear = new DataTable();
            tagrdDocList.DataSource = dtforclear;
            tagrdDocList.DataBind();

            OpenFileDialog oDlg = new OpenFileDialog();
            oDlg.Filter = "Excel files (*.xlsx)|*.xlsx|(*.xls)|*.xls";
            DialogResult dlg = oDlg.ShowDialog();

            if (dlg == DialogResult.OK)
            {
                ExcelPath.SetValueTrigger(oDlg.FileName, false);
            }
            else
                return;
            try
            {
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

        private void btnAppend_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtItems = this.GetExcelData(ExcelSheets.Text.ToString());

                if (dtItems.Rows.Count > 0)
                {
                    tagrdDocList.DataSource = dtItems;
                    tagrdDocList.DataBind();
                    lblmessage.Visible = true;
                }
         
            }
            catch (Exception ex)
            {
                this.Error(ex, true);
            }
        }
        private DataTable GetExcelData(string sheetName)
        {
            DataTable dtExcelData;
            try
            {
                string defaultItemUOMID = string.Empty;
                string rowFilter = "";

                DocComUtility.AppRunningState = "ImportInProgress";

                //Retrieve data from Excel Sheet as DataTable
                dtExcelData = objExcelImport.GetExcelData(sheetName);
                try
                {
                    //filter blank rows                   
                    //At this stage, blank rows are excluded in dtExcelData                 
                    dtExcelData = dtExcelData.AsEnumerable().Where(row => row.ItemArray.Any(field => !(field is DBNull))).CopyToDataTable();
                }
                catch (Exception ex)
                {
                    MsgBox.Show("Unable to import.\n\nThere are blank rows between datas in your source excel file.");
                    return null;
                }

              
                //Validate structure and data according to pre-definition in SQL Table SyS_DocItem_ImportStructure
                if (GFunc.ValidateExcelData(dtExcelData, TgtDC) == false)
                    return null;

                List<SqlParameter> paraList = new List<SqlParameter>();
                GlobalUI.bRuningImport = true;
                DocUtility.bRuningImport = true;

                string XMLformat = "";
                dtExcelData.TableName = "dtExcelData";
                XMLformat = GFunc.ConvertDataTableToXML(dtExcelData);

                paraList.Add(new SqlParameter("@budgetYear", cmdBudgetYear.Value));
                paraList.Add(new SqlParameter("@xmlExcelData", XMLformat));
                dtExcelData = GFunc.ExecuteProc("KeyCustomer_Import", paraList);


            }
            catch (Exception ex)
            {
                throw this.Error(ex, false);
            }
            finally
            {
                GlobalUI.bRuningImport = false;
                DocUtility.bRuningImport = false;
                DocComUtility.AppRunningState = "";
            }
            
            return dtExcelData;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmKeyCustomersImport_Shown(object sender, EventArgs e)
        {
            if (formClose)
                this.Close();
        }

        private void ExcelSheets_RowSelected(object sender, RowSelectedEventArgs e)
        {
            DataTable dtforclear = new DataTable();
            tagrdDocList.DataSource = dtforclear;
            tagrdDocList.DataBind();
        }
    }

}
