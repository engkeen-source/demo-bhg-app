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
using Infragistics.Win.Misc;
using TAUtil;
using System.IO;
namespace WinUI
{
    public partial class frmCustomerStatement : Form
    {
        #region Local Variable(s)

        private int? docKey = null;//currently not used
        GEnum.SystemCode docCodeKey;
        DataTable dtCustomer = null;
        private string ContextMenuSetting = string.Empty;
        DataTable dtReports = null;

        DataSet dsReportData = null;
        #endregion

        //Initialize
        public frmCustomerStatement()
        {
            InitializeComponent();
        }//Completed        

        //Form Event
        private void frmCustomerStatement_Load(object sender, EventArgs e)
        {
            try
            {
                //GlobalUI.cmnuGlobal_Set(this);
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0, this.Name);
                GlobalUI.Combos_Fill(this, 0);

                //Form Layout
                tagrdCustomerList.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True;
                tagrdCustomerList.DisplayLayout.Override.SelectTypeRow = SelectType.Extended;
                tagrdCustomerList.DisplayLayout.Bands[0].Columns["CCurrID"].Hidden = true;
                tagrdCustomerList.DisplayLayout.Bands[0].Columns["EmailStatement"].CellActivation = Activation.Disabled; /* added by YST -- checkbox should be readonly */

                //Get a copy of the rpt combo list so that we can apply filter to this source when statementtype is changed
                dtReports = ((DataTable)RptFile.DataSource).Copy();

                //Set DateFrom value to 1st Day of last month
                DateTo.DateValue = DateTime.Today.AddDays(-DateTime.Today.Day);
                
                if (DateTime.Today.Month == 1)
                    DateFrom.DateValue = ((DateTime)DateTo.DateValue).AddDays(1 - DateTime.DaysInMonth(DateTime.Today.Year - 1, 12));
                else
                    DateFrom.DateValue = ((DateTime)DateTo.DateValue).AddDays(1 - DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month - 1));
                CCBKey.Value = 10;
                StatementType.Value = 2265;
                StatementType_CustomUpdate(null, null);
                tacboDueOption.Value = 20;
                SendType.Value = 10;
                this.EnablePrint(false);
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
        private void frmCustomerStatement_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, 0);
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed

        //Button Event(s)
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
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
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintReport(false);
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
        private void btnPrintAll_Click(object sender, EventArgs e)
        {
            try
            {
                PrintReport(true);
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
        private void btnRequery_Click(object sender, EventArgs e)
        {
            try
            {
                frmMain.gfrmMain.SetNotifyStatus("Loading.... Please wait.");

                GetCustomerList();               
                
                EnablePrint(true);
                frmMain.gfrmMain.SetNormalStaus("Ready");
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

        //Control Events
        private void Combo_ItemNotInList(object sender, Infragistics.Win.UltraWinEditors.ValidationErrorEventArgs e)
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
        }//Completed
        private void ConIDFrom_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {

                int key = 0;
                string id = string.Empty;
                string des = string.Empty;

                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ConIDFrom");
                if (DocHDRUtil.EditorButton_Popup((int)docCodeKey, ConIDFrom.Text, listSettingID, (int)GEnum.PopupType.CusID, ref key, ref id, ref des))
                {
                    this.ConIDFrom.SetValueTrigger(id, false);
                    this.EnablePrint(false);
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
        }//Completed
        private void ConIDTo_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;

                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ConIDTo");
                if (DocHDRUtil.EditorButton_Popup((int)docCodeKey, ConIDTo.Text, listSettingID, (int)GEnum.PopupType.CusID, ref key, ref id, ref des))
                {
                    this.ConIDTo.SetValueTrigger(id, false);
                    this.EnablePrint(false);
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
        }//Completed
        private void Combo_CustomUpdate(object sender, CancelEventArgs e)
        {
            //if(((TAComboBox)sender).Name.ToUpper() !="RPTFILE" )
                this.EnablePrint(false);
        }//Completed
        private void StatementType_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                int CState = 0;
                //refresh RptFile Combo source        
                if (dtReports != null)
                {
                    //dtReports.DefaultView.RowFilter = "RepKey = " + GFunc.NEInt(StatementType.Value, 0);
                    //dtReports.DefaultView.Sort = "UID Desc";
                   // RptFile.DataSource = dtReports.DefaultView.ToTable();
                    //IEnumerable<DataRow> dtReportFilter=dtReports.AsEnumerable().Where(r =>  GFunc.NEInt(r.Field<int?>("RepKey"),0) == GFunc.NEInt(StatementType.Value, 0)).OrderByDescending(r => r.Field<int?>("UID"));
                  //  RptFile.DataSource = dtReportFilter.CopyToDataTable();

                    dtReports.DefaultView.RowFilter="RepKey="+GFunc.NEInt(StatementType.Value, 0);
                    dtReports.DefaultView.Sort = "UID";
                    DataTable dtReportFilter = dtReports.DefaultView.ToTable();
                    RptFile.DataSource = dtReportFilter;

                    if (dtReportFilter.Rows.Count > 0)
                    {
                        DataRow dr = dtReportFilter.AsEnumerable().FirstOrDefault(o => o.Field<bool>("IsDefault") == true);
                        if(dr!=null)
                            RptFile.Value = dr["UID"];
                    }
                }
                btnEmailLog.Enabled = false;
                //visible Disable DateFrom,AddDays by StatementType
                switch ((int)StatementType.Value)
                {
                    case 2270:  //BF Statement
                        DateFrom.Visible = true;
                        AddDays.Visible = false;
                        AddDays_Lbl.Visible = false;
                        btnEmailLog.Enabled = true;
                        break;

                    case 1335:  //reminder Letter with additional days
                        DateFrom.Visible = false;
                        AddDays.Visible = true;
                        AddDays.Enabled  = true;
                        AddDays_Lbl.Visible = true;
                        break;

                    default:
                        DateFrom.Visible = false;
                        AddDays.Visible = false;
                        AddDays_Lbl.Visible = false;
                        break;
                }

                if ((int)StatementType.Value == 2270)
                    CState = 20;
                else if ((int)StatementType.Value == 2265)
                {
                    CState = 10;
                    btnEmailLog.Enabled = true;
                }

                if (CState != 0)
                {
                    ((DataTable)ConIDFrom.DataSource).DefaultView.RowFilter = "CDefaultStateType=" + CState;
                    ((DataTable)ConIDTo.DataSource).DefaultView.RowFilter = "CDefaultStateType=" + CState;
                }
                else
                {
                    ((DataTable)ConIDFrom.DataSource).DefaultView.RowFilter = "";
                    ((DataTable)ConIDTo.DataSource).DefaultView.RowFilter = "";
                }

                EnablePrint(false);
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

        //Functions
        private void EnablePrint(bool Requery)
        {
            btnPrint.Enabled = Requery;
            btnPrintAll.Enabled = Requery;
            chkPreview.Enabled = Requery;
            btnSavePdf.Enabled = Requery;
            btnSend.Enabled = Requery;
            btnSendAll.Enabled = Requery;

        }//Completed
        private void GetCustomerList()
        {
            try
            {
                if (GFunc.IsNEZ(StatementType.Value))
                {
                    MsgBox.Show("Statement Type cannot be empty");
                    StatementType.Focus();
                    return;
                }

                bool statement = false;
                
                if (GFunc.NEInt(StatementType.Value, 0) == 2265 || GFunc.NEInt(StatementType.Value, 0) == 2270)
                    statement = true;


                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@CCBType", GFunc.NEInt(CCBKey.Value, 0)));
                parmList.Add(new SqlParameter("@fromID", GFunc.NEStr(ConIDFrom.Text, string.Empty)));
                parmList.Add(new SqlParameter("@toID", GFunc.NEStr(ConIDTo.Text, string.Empty)));
                parmList.Add(new SqlParameter("@sortingTypeLevel", -1));    //ignore Record Access Level
                parmList.Add(new SqlParameter("@sortingTypeGrp", -1));      //ignore record access group

                dtCustomer = GFunc.ExecuteProc("MSTCon_GetCustomerList", parmList);
                dtCustomer.TableName = "dtSelectCustomer";
          
                dtCustomer.Columns.Add("DueAmtH", typeof(decimal));
                //dtCustomer.Columns.Add("DocTermID", typeof(string));
                //dtCustomer.Columns.Add("MthPayAmt", typeof(decimal));
                //dtCustomer.Columns.Add("DueTotal", typeof(decimal));
                //dtCustomer.Columns.Add("Amt0", typeof(decimal));
                //dtCustomer.Columns.Add("Amt1", typeof(decimal));
                //dtCustomer.Columns.Add("Amt2", typeof(decimal));
                //dtCustomer.Columns.Add("Amt3", typeof(decimal));
                //dtCustomer.Columns.Add("Amt4", typeof(decimal));
                //dtCustomer.Columns.Add("AmtR", typeof(decimal));

                
               
                dsReportData = LoadReportData(dtCustomer);

                if (statement)
                {
                    foreach (DataRow dr in dtCustomer.Rows)
                    {
                        dsReportData.Tables[0].DefaultView.RowFilter = "ConKey=" + dr["ConKey"];
                        decimal a = 0;
                        foreach (DataRowView drv in dsReportData.Tables[0].DefaultView)
                        {
                            //dr["DocTermID"] = drv["DocTermID"];
                            //dr["MthPayAmt"] = drv["MthPayAmt"];
                            //dr["DueTotal"] = drv["DueTotal"];
                            //dr["Amt0"] = drv["Amt0"];
                            //dr["Amt1"] = drv["Amt1"];
                            //dr["Amt2"] = drv["Amt2"];
                            //dr["Amt3"] = drv["Amt3"];
                            //dr["Amt4"] = drv["Amt4"];
                            //dr["AmtR"] = drv["AmtR"];
                            a += GFunc.NEDec(drv["DueAmtH"], 0M);

                        }
                        dr["DueAmtH"] = a;
                        dr.AcceptChanges();
                    }
                    dsReportData.Tables[0].DefaultView.RowFilter = "";

                    DataTable dtAge = (from myRow in dsReportData.Tables[1].AsEnumerable()
                                       join custRow in dtCustomer.AsEnumerable()
                                        on myRow["DocConKey"] equals custRow["ConKey"]
                                       orderby myRow["DocConID"]
                                       select new
                                       {
                                            ConKey = myRow.Field<int>("DocConKey")                                             
                                           ,EmailStatement = custRow.Field<bool>("EmailStatement")                                           
                                           ,Territory=custRow.Field<string>("CTerritoryID")
                                           ,Industry = custRow.Field<string>("CIndustryID")
                                           ,ConID = myRow.Field<string>("DocConID")
                                           ,ConNm = myRow.Field<string>("DocConNm")                                           
                                           ,EmNm = myRow.Field<string>("DocEmNm")                   
                                           ,BUnit = myRow.Field<string>("Team")
                                           ,DocTermID = myRow.Field<string>("DocTermID")     
                                           ,CurrID = myRow.Field<string>("DocCurrID")
                                           ,MthPayAmt = myRow.Field<decimal>("MthPayAmt")
                                           ,DueAmount = myRow.Field<decimal>("T")
                                           ,Amt0 = myRow.Field<decimal>("0")
                                           ,Amt1 = myRow.Field<decimal>("1")
                                           ,Amt2 = myRow.Field<decimal>("2")
                                           ,Amt3 = myRow.Field<decimal>("3")
                                           ,Amt4 = myRow.Field<decimal>("4")
                                           ,AmtR = myRow.Field<decimal>("5") + myRow.Field<decimal>("6") + myRow.Field<decimal>("7") + myRow.Field<decimal>("8")
                                                 + myRow.Field<decimal>("9") + myRow.Field<decimal>("10") + myRow.Field<decimal>("11") + myRow.Field<decimal>("12")                                         
                                          ,DueAmtH = custRow.Field<decimal>("DueAmtH")
                                          
                                       } ).AsDataTable();

                 
                    if (tagrdBalance.DataSource == null)// first time
                    {
                        tagrdBalance.DataSource = dtAge;
                        GlobalUI.Grid_FormatAppearance(tagrdBalance);
                        tagrdBalance.ContextMenuStrip =GlobalUI.cmnuGlobal;
                    }
                    else
                        tagrdBalance.DataSource = dtAge;
                }
                else
                    tagrdBalance.DataSource = null;

                tagrdCustomerList.DataSource = dtCustomer;              

                if (statement)
                {
                    tagrdCustomerList.DisplayLayout.Bands[0].Columns["DueAmtH"].Header.Caption = "Total Due in SGD";

                    tagrdBalance.DisplayLayout.Bands[0].Columns["EmailStatement"].CellActivation = Activation.Disabled; /* added by YST -- checkbox should be readonly */
                    tagrdBalance.DisplayLayout.Bands[0].Columns["ConKey"].Hidden=true;
                    tagrdBalance.DisplayLayout.Bands[0].Columns["DueAmtH"].Hidden = true;
                    tagrdBalance.DisplayLayout.Bands[0].Columns["ConNm"].Header.Caption = "Customer Name";
                    tagrdBalance.DisplayLayout.Bands[0].Columns["EmNm"].Header.Caption = "Sales In Charge";
                    tagrdBalance.DisplayLayout.Bands[0].Columns["MthPayAmt"].Header.Caption = "Total Payment";
                    tagrdBalance.DisplayLayout.Bands[0].Columns["Amt0"].Header.Caption = DateTo.DateValue.Value.ToString("dd MMM yyyy");
                    tagrdBalance.DisplayLayout.Bands[0].Columns["Amt1"].Header.Caption = DateTo.DateValue.Value.AddMonths(-1).ToString("dd MMM yyyy");
                    tagrdBalance.DisplayLayout.Bands[0].Columns["Amt2"].Header.Caption = DateTo.DateValue.Value.AddMonths(-2).ToString("dd MMM yyyy");
                    tagrdBalance.DisplayLayout.Bands[0].Columns["Amt3"].Header.Caption = DateTo.DateValue.Value.AddMonths(-3).ToString("dd MMM yyyy");
                    tagrdBalance.DisplayLayout.Bands[0].Columns["Amt4"].Header.Caption = DateTo.DateValue.Value.AddMonths(-4).ToString("dd MMM yyyy");
                    tagrdBalance.DisplayLayout.Bands[0].Columns["AmtR"].Header.Caption = "<" + DateTo.DateValue.Value.AddMonths(-4).ToString("dd MMM yyyy");

                    tagrdBalance.DisplayLayout.Bands[0].Columns["DueAmount"].Format = "#,##0.00";
                    tagrdBalance.DisplayLayout.Bands[0].Columns["MthPayAmt"].Format = "#,##0.00";
                    tagrdBalance.DisplayLayout.Bands[0].Columns["Amt0"].Format = "#,##0.00";
                    tagrdBalance.DisplayLayout.Bands[0].Columns["Amt1"].Format = "#,##0.00";
                    tagrdBalance.DisplayLayout.Bands[0].Columns["Amt2"].Format = "#,##0.00";
                    tagrdBalance.DisplayLayout.Bands[0].Columns["Amt3"].Format = "#,##0.00";
                    tagrdBalance.DisplayLayout.Bands[0].Columns["Amt4"].Format = "#,##0.00";
                    tagrdBalance.DisplayLayout.Bands[0].Columns["AmtR"].Format = "#,##0.00";

                    tagrdBalance.DisplayLayout.Bands[0].Columns["DueAmount"].CellAppearance.TextHAlign = HAlign.Right;
                    tagrdBalance.DisplayLayout.Bands[0].Columns["MthPayAmt"].CellAppearance.TextHAlign = HAlign.Right;
                    tagrdBalance.DisplayLayout.Bands[0].Columns["Amt0"].CellAppearance.TextHAlign = HAlign.Right;
                    tagrdBalance.DisplayLayout.Bands[0].Columns["Amt1"].CellAppearance.TextHAlign = HAlign.Right;
                    tagrdBalance.DisplayLayout.Bands[0].Columns["Amt2"].CellAppearance.TextHAlign = HAlign.Right;
                    tagrdBalance.DisplayLayout.Bands[0].Columns["Amt3"].CellAppearance.TextHAlign = HAlign.Right;
                    tagrdBalance.DisplayLayout.Bands[0].Columns["Amt4"].CellAppearance.TextHAlign = HAlign.Right;
                    tagrdBalance.DisplayLayout.Bands[0].Columns["AmtR"].CellAppearance.TextHAlign = HAlign.Right;
                    
                }
                
                GridFilter(statement);
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
        }//Completed

        private void GridFilter(bool statement)
        {
            if (dtCustomer == null)
                btnRequery_Click(null, null);

            if (GFunc.NEInt(StatementType.Value, 0) == 2265)
                dtCustomer.DefaultView.RowFilter = "CDefaultStateType=10";
            else if (GFunc.NEInt(StatementType.Value, 0) == 2270)
                dtCustomer.DefaultView.RowFilter = "CDefaultStateType=20";

            if (dtCustomer.DefaultView.RowFilter != "")
            {
                dtCustomer.DefaultView.RowFilter = dtCustomer.DefaultView.RowFilter + " And EmailStatement=" + (GFunc.NEInt(SendType.Value, 0) == 20);
            }
            else
                dtCustomer.DefaultView.RowFilter = "EmailStatement=" + (GFunc.NEInt(SendType.Value, 0) == 20);

            if (statement)
            {
                DataTable dt = (tagrdBalance.DataSource as DataTable);
                if (dt.DefaultView.RowFilter != "")
                {
                    dt.DefaultView.RowFilter = dt.DefaultView.RowFilter + " And EmailStatement=" + (GFunc.NEInt(SendType.Value, 0) == 20);
                }
                else
                    dt.DefaultView.RowFilter = "EmailStatement=" + (GFunc.NEInt(SendType.Value, 0) == 20);

                if (GFunc.NEInt(tacboDueOption.Value, 0) == 20)
                {
                    dtCustomer.DefaultView.RowFilter = dtCustomer.DefaultView.RowFilter + " And DueAmtH>0";
                    dt.DefaultView.RowFilter = dt.DefaultView.RowFilter + " And DueAmtH>0";
                }
                else if (GFunc.NEInt(tacboDueOption.Value, 0) == 30)
                {
                    dtCustomer.DefaultView.RowFilter = dtCustomer.DefaultView.RowFilter + " And DueAmtH<=0";
                    dt.DefaultView.RowFilter = dt.DefaultView.RowFilter + " And DueAmtH<=0";
                }

                
            }
        }

        private DataTable GetSelectedCustomer(bool bAllCustomer)
        {
            try
            {               
                DataTable dt = (tagrdCustomerList.DataSource as DataTable).DefaultView.ToTable();

                if (bAllCustomer)
                {
                    //get all customers
                   // dt = (tagrdCustomerList.DataSource as DataTable).Copy();
                }
                else if (tagrdCustomerList.Selected.Rows.Count > 0)
                {
                    dt.Rows.Clear();
                    foreach (UltraGridRow row in tagrdCustomerList.Selected.Rows)
                    {
                        DataRow NewRow = ((DataRowView)row.ListObject).Row;
                        dt.Rows.Add(NewRow.ItemArray);
                    }
                }
                else if (tagrdCustomerList.ActiveRow!=null)
                {
                    dt.Rows.Clear();
                    DataRow NewRow = ((DataRowView)tagrdCustomerList.ActiveRow.ListObject).Row;
                    dt.Rows.Add(NewRow.ItemArray);
                }

                dt.TableName = "dtSelectCustomer";
             
                return dt;
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
        }//Completed

        private string GetSelectedCustomerKeys(bool bAllCustomer)
        {
            try
            {
                string keys = "";
                DataTable dt = (tagrdCustomerList.DataSource as DataTable).DefaultView.ToTable();
                if (bAllCustomer)
                {
                    foreach (DataRowView drv in dt.DefaultView)
                    {
                        keys += GFunc.NEStr(drv["ConKey"], "") + ",";
                    }
                    if (keys.Length >= 1)
                        keys = keys.Remove(keys.Length - 1);
                }
                else if (tagrdCustomerList.Selected.Rows.Count > 0)
                {                    
                    foreach (UltraGridRow row in tagrdCustomerList.Selected.Rows)
                    {
                        keys +=GFunc.NEStr( row.Cells["ConKey"].Value,"")+",";                        
                    }
                    if(keys.Length>=1)
                        keys= keys.Remove(keys.Length - 1);
                }
                else if (tagrdCustomerList.ActiveRow != null)
                {
                    keys = GFunc.NEStr(tagrdCustomerList.ActiveRow.Cells["ConKey"].Value, "");                   
                }

                return keys;
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
        }//Completed


        private DataSet LoadReportData(DataTable dtCustomers)
        {
            // Preapre parameter list (Require parameter at least @MsgID)
            try
            {
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@RepKey", StatementType.Value));
                parmList.Add(new SqlParameter("@StatementType", StatementType.Value));
                parmList.Add(new SqlParameter("@DateTo", DateTo.DateValue));
                string xmlSelectedCustomer = GFunc.ConvertDataTableToXML(dtCustomers);
                parmList.Add(new SqlParameter("@xmlSelectedCustomer", xmlSelectedCustomer));

              
                if (GFunc.NEInt(DueCalculation.Value, 0) > 0)
                    parmList.Add(new SqlParameter("@DueCal", DueCalculation.Value));

                if (GFunc.NEInt(CCBKey.Value, 0) > 0)
                    parmList.Add(new SqlParameter("@CCBType", CCBKey.Value));
                
                parmList.Add(new SqlParameter("@DateFrom", DateFrom.DateValue));
              
                return GFunc.ExecuteProcDataSet("Rep_CVState", parmList);

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
        }//Completed

        private bool Validation(bool btnPrintAll)
        {
            //Validation and Set Default values
            if (GFunc.IsNEZ(DueCalculation.Value))
                DueCalculation.SetValueTrigger(20, false);  //Use Invoice Date

            if (GFunc.IsNEZ(CCBKey.Value))
                CCBKey.SetValueTrigger(10, false);  //use Credit          

            if (AddDays.Visible && (GFunc.NEInt(AddDays.Value, 0) < 0))
            {
                MsgBox.Show("Additional Days must be >=0");
                return false;
            }
            if (GFunc.IsNEZ(StatementType.Value))
            {
                MsgBox.Show("Statement Type cannot be empty");
                return false;
            }
            if ((int)StatementType.Value != 2270) //not BF Statement
            {
                DateFrom.DateValue = DateTo.DateValue;
            }
            if (GFunc.IsNE(DateFrom.DateValue) || GFunc.IsNE(DateTo.DateValue))
            {
                MsgBox.Show("Date range cannot be empty");
                return false;
            }
            if (tagrdCustomerList.Selected.Rows.Count == 0 && btnPrintAll == false && tagrdCustomerList.ActiveRow == null)
            {
                MsgBox.Show("Please select a customer");
                return false;
            }
            if (GFunc.IsNE(this.RptFile.SelectedRow))
            {
                MsgBox.Show("Please select a report name");
                return false;
            }
            return true;
        }

        private void PrintReport(bool btnPrintAll)
        {
            string repTitle = string.Empty;
            string rptName = string.Empty;
            string SubReport = "";

            try
            {
                if (Validation(btnPrintAll) == false)
                    return;

                if ((int)CCBKey.Value == 10)
                    repTitle = "STATEMENT OF ACCOUNT";
                else
                    repTitle = "CASH TERM STATEMENT OF ACCOUNT";

                DataSet ds = new DataSet();

                string keys = GetSelectedCustomerKeys(btnPrintAll);
                if (dsReportData.Tables.Count > 1)
                {
                    dsReportData.Tables[0].TableName = "Header";
                    dsReportData.Tables[1].TableName = "Detail"; //Sub Report Data   

                    if (GFunc.NEInt(StatementType.Value, 0) == 2265 || GFunc.NEInt(StatementType.Value, 0) == 2270)
                    {
                        dsReportData.Tables[0].DefaultView.RowFilter = "DocConKey IN(" + keys + ")";
                        dsReportData.Tables[1].DefaultView.RowFilter = "DocConKey IN(" + keys + ")";
                    }

                    ds.Tables.Add(dsReportData.Tables[0].DefaultView.ToTable());
                    ds.Tables.Add(dsReportData.Tables[1].DefaultView.ToTable());
                }
                else
                {
                    dsReportData.Tables[0].TableName = "Header";
                    dsReportData.Tables[0].DefaultView.RowFilter = "DocConKey IN(" + keys + ")";

                    ds.Tables.Add(dsReportData.Tables[0].DefaultView.ToTable());
                }

                if (!ExistData(ds))
                    return;             

                int repKey = GFunc.NEInt(StatementType.Value, 0);
                ReportLoader _ReportLoader = new ReportLoader();
                List<ReportParameter> repParas = new List<ReportParameter>();

                string opCmpValue = SysOptionUtility.GetStr("CompanyName");
                string opCmpRegValue = SysOptionUtility.GetStr("CompanyRegNumber");
                int opaddrKey = SysOptionUtility.GetInt("DefaultLetterHeadAddr");
                

                if (repKey != 2290 && repKey != 2275 && repKey != 2280 && repKey != 2285 && repKey != 1335) //if Not Reminder
                {
                    DataTable dtSub = GFunc.ExecuteQuery("select * from SYS_RepPara where ParName='pSubRepPath' and RepKey = " + repKey);                    
                  
                    if (dtSub != null)
                        SubReport = GFunc.NEStr(dtSub.Rows[0]["ParDefaultValue"], "");
                }

                string opCmpAddrValue = GFunc.AddrGet(opaddrKey, true);               

                repParas.Add(new ReportParameter("pCmpName", opCmpValue));
                repParas.Add(new ReportParameter("pCmpAddr", opCmpAddrValue));
                repParas.Add(new ReportParameter("pRepTitle", repTitle));

                //Assign DateTo parameter to Rpx for Reminder with additional days
                if (AddDays.Visible)
                    repParas.Add(new ReportParameter("pAddDays", GFunc.NEInt(AddDays.Value, 0)));

                DateTime agDate = GFunc.NEDateTime(DateTo.Value, DateTime.Today);
                agDate = new DateTime(agDate.Year, agDate.Month, DateTime.DaysInMonth(agDate.Year,agDate.Month));
                //Assign DateTo parameter to Rpx for BF Statment only
                if (repKey == 2270 || repKey == 1335 || repKey == 2265)
                {
                    repParas.Add(new ReportParameter("pStatementDate", DateTo.DateValue));  
                }

                if (repKey != 2290 && repKey != 2275 && repKey != 2280 && repKey != 2285 && repKey != 1335) //if Not Reminder
                {
                    string SubReportPath = Application.StartupPath.ToString()+"\\" + SubReport;
                    repParas.Add(new ReportParameter("pSubRepPath", SubReportPath));
                    for (int i = 0; i < 12; i++)
                    {
                        repParas.Add(new ReportParameter("pMth" + (i + 1).ToString(), agDate.AddMonths(i * -1).ToString("dd-MMM-yyyy")));
                    }
                }
                else
                {
                    repParas.Add(new ReportParameter("pStatementDate", DateTo.DateValue));
                }    
              
                rptName = this.RptFile.SelectedRow.Cells["RptNm"].Value.ToString();
                
                _ReportLoader.ReportParameter = repParas;                
                _ReportLoader.RepKey = repKey;

                _ReportLoader.LoadCrystalReport(ds, this.RptFile.SelectedRow.Cells["RptNm"].Value.ToString(), SubReport);
              
                if (chkPreview.Checked)
                    _ReportLoader.PrintPreview();
                else
                    _ReportLoader.Print();

            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed

        private bool ExistData(DataSet dsReportData)
        {
            if (dsReportData == null)
            {
                MsgBox.Show("There is no records for this report");
                return false;
            }
            else
            {
                if (dsReportData.Tables.Count <= 0)
                {
                    MsgBox.Show("There is no records for this report");
                    return false;
                }
                else
                    if (dsReportData.Tables[0].Rows.Count <= 0)
                    {
                        MsgBox.Show("There is no records for this report");
                        return false;
                    }
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

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (Validation(false) == false)
                return;

            frmMain.gfrmMain.SetNotifyStatus("Working ...............");      
      
            Send(false);
            frmMain.gfrmMain.SetNormalStaus("Ready");
        }

        private void btnSendAll_Click(object sender, EventArgs e)
        {
            if (Validation(false) == false)
                return;
            frmMain.gfrmMain.SetNotifyStatus("Working ...............");
            //DataTable dtCust = GetSelectedCustomer(true);
            Send(true);
            frmMain.gfrmMain.SetNormalStaus("Ready");
        }
        private void Send(bool All)
        {
            int _RepKey = (int)StatementType.Value;
            string _RptName = this.RptFile.SelectedRow.Cells["RptNm"].Value.ToString();
            string imsg = "", sub = "", ccs = "";

            // ccs = "ivy.yap@bhholding.sg";
            if (_RepKey != 2290 && _RepKey != 2275 && _RepKey != 2280 && _RepKey != 2285 && _RepKey != 1335)
            {
                /* commented by YST on 2022/01/11 */
                /*
                imsg = "<p style=\"font-size:11.0pt\">Dear Valued Customer of BHM,</p> <p style=\"font-size:11.0pt\"> "
                    + "Good Day and thank you for your continuous support.</p> <p style=\"font-size:11.0pt\"> "
                    + "Please find attached SOA  - Statement of Account in PDF as of [Month-Year] for your kind attention.</p><p style=\"font-size:11.0pt\"> "
                    + "For enquiries and clarifications for any payment, please email to payment@bhglobal.com.sg or call +65 6210 4746</p><p style=\"font-size:11.0pt;margin-bottom:0.0pt\"> "
                    + "For enquiries and clarifications for any invoice, please email to ar@bhglobal.com.sg or call +65 6210 4746</p><p style=\"font-size:11.0pt;margin-bottom:0.0pt\"> "
                    + "Thank you.</p>";
                sub = "Statement of Account for <Month Year> from Beng Hui Marine Electrical Pte Ltd to <CompanyName>";
                */

                /* modified by YST on 2022/01/11 as per requested by May Thet Htar Aung from finance department */
                imsg = "<p style=\"font-size:11.0pt\"> Hi Accounts,</p>"
                     + "<p style=\"font-size:11.0pt\"> Please find the attached statement of account for your checking and advise us the next payment schedule."
                     + "<br> If there is any missing invoice(s) / any question,  please do not hesitate to contact payment@bhglobal.com.sg.</p>"
                     + "<p style=\"font-size:11.0pt;margin-bottom:0.0pt\">Thank you for supporting us.</p>";
                sub = "SOA as at " + DateTo.Text + " - CompanyName";
            }
            else
            {
                if(SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                {
                    //modified on 11-Jan-2024, removed the word 1st
                    if (_RptName.Equals("zState_ReminderAdd1.rpt") || _RptName.Equals("zState_Reminder1BM.rpt"))
                    {
                        imsg = "<p style=\"font-size:11.0pt\"> Dear Valued Customer of Beng Hui Marine,</p> <p style=\"font-size:11.0pt\"> Attached are the Reminder to alert your good company to make payment for outstanding accounts within 1 weeks’ time from Letter’s date and BHM Bank Account Detail.</p>"
                        + " <p style=\"font-size:11.0pt\">  OR </p><p style=\"font-size:11.0pt\"> "
                            + "Please reply us with payment advice attached, to allow us to monitor incoming payment together.</p> <p style=\"font-size:11.0pt\"> Your reply is important to us.</p><p style=\"font-size:11.0pt\"> For enquiries and clarifications, please email to payment@bhglobal.com.sg or call to +65 6210 4746</p><p style=\"font-size:11.0pt\"> Thank you.</p>"
                            + "</p><p style=\"font-size:11.0pt;font-weight:bold;text-decoration: underline\"> Remarks: [ In order to maintain our goodwill relationship, please expedite payment process and avoid further delay as we have more future project to move together ] </p>";
                        sub = "ALERT: Reminder To <CompanyName> from Beng Hui Marine Electrical Pte Ltd dated <" + DateTime.Today.ToString("dd MMM yyyy") + ">";
                    }
                    else if (_RptName.Equals("zState_ReminderAdd2.rpt") || _RptName.Equals("zState_Reminder2BM.rpt"))
                    {
                        imsg = "<p style=\"font-size:11.0pt\"> Dear Valued Customer of Beng Hui Marine,</p> <p style=\"font-size:11.0pt\"> Attached Letters – 2nd Reminder to inform everyone that we still have not received any payment yet from date given and BHM Bank Account Detail.</p>"

                            + "<p style=\"font-size:11.0pt\">  OR </p><p style=\"font-size:11.0pt\"> "
                           + "Please reply us with payment advice <span style=\"font-weight:bold;text-decoration: underline\">[ attachment ]</span>, to allow us to monitor incoming payment together.</p>"
                           + " <p style=\"font-size:11.0pt\"> Your reply is important to us.</p>"
                            + "<p style=\"font-size:11.0pt\"> Thank you.</p>"
                           + "</p><p style=\"font-size:11.0pt;font-weight:bold;text-decoration: underline\"> Remarks: [ In order to maintain our goodwill relationship, please expedite payment process and avoid further delay as we have more future project to move together ] </p><p style=\"font-size:11.0pt\"> For enquiries and clarifications, please email to payment@bhglobal.com.sg or call to +65 6210 4746</p>";
                        sub = "ALERT: 2nd  Reminder To <CompanyName> from Beng Hui Marine Electrical Pte Ltd dated <" + DateTime.Today.ToString("dd MMM yyyy") + ">";
                    }
                    else if (_RptName.Equals("zState_ReminderAdd3.rpt") || _RptName.Equals("zState_Reminder3BM.rpt"))
                    {
                        imsg = "<p style=\"font-size:11.0pt\"> Dear Valued Customer of Beng Hui Marine,</p>"
                        + " <p style=\"font-size:11.0pt\"> This serves as a final reminder for your immediate settlement of these invoices.</p> "
                               + "<p style=\"font-size:11.0pt\">We will proceed with legal action if we do not receive your settlement within the next 10 working days.</p>"
                               + "<p style=\"font-size:11.0pt\">Please reply us with payment advice attached, to allow us to monitor incoming payment together.</p>"
                               + "<p style=\"font-size:11.0pt\">Your prompt attention to this matter is greatly appreciated.</p>"                            
                        + "<p style=\"font-size:11.0pt\">For enquiries and clarifications, please email to payment@bhglobal.com.sg or call to +65 6210 4746</p>"
                        + "<p style=\"font-size:11.0pt\"> Thank you.</p>";

                        sub = "ALERT: FINAL REMINDER To <CompanyName> from Beng Hui Marine Electrical Pte Ltd dated <" + DateTime.Today.ToString("dd MMM yyyy") + ">";
                    }
                    else if (_RptName.Equals("zState_Warning.rpt"))
                    {
                        imsg = "<p style=\"font-size:11.0pt\"> Dear Valued Customer of Beng Hui Marine,</p>"
                        + " <p style=\"font-size:11.0pt\"> Attached Letters – WARNING LETTER to inform you that we have not received any payment or reply from you despite sending the final reminder and BHM Bank Account Detail.</p> "
                               + "<p style=\"font-size:11.0pt\">We have no choice but to commence legal action against <CompanyName>.</p>"
                        + "<p style=\"font-size:11.0pt\"> However if we receive payment from you to settle the accounts, we will cancel the legal action accordingly.</p><p style=\"font-size:11.0pt\"> For enquiries and clarifications, please email to payment@bhglobal.com.sg or call to +65 6210 4746</p>"
                        + "<p style=\"font-size:11.0pt\"> Thank you.</p>";

                        sub = "ALERT: WARNING LETTER To <CompanyName> from Beng Hui Marine Electrical Pte Ltd dated <" + DateTime.Today.ToString("dd MMM yyyy") + ">";
                    }
                }
                else
                {
                    //modified on 11-Jan-2024, removed the word 1st
                    if (_RptName.Equals("zState_ReminderAdd1.rpt"))
                    {
                        imsg = "<p style=\"font-size:11.0pt\"> Dear Valued Customer of Beng Hui Marine,</p> <p style=\"font-size:11.0pt\"> Attached are the Reminder to alert your good company to make payment for outstanding accounts within 1 weeks’ time from Letter’s date and BHM Bank Account Detail.</p>"
                        + " <p style=\"font-size:11.0pt\">  OR </p><p style=\"font-size:11.0pt\"> "
                            + "Please reply us with payment advice attached, to allow us to monitor incoming payment together.</p> <p style=\"font-size:11.0pt\"> Your reply is important to us.</p><p style=\"font-size:11.0pt\"> For enquiries and clarifications, please email to payment@bhglobal.com.sg or call to +65 6210 4746</p><p style=\"font-size:11.0pt\"> Thank you.</p>"
                            + "</p><p style=\"font-size:11.0pt;font-weight:bold;text-decoration: underline\"> Remarks: [ In order to maintain our goodwill relationship, please expedite payment process and avoid further delay as we have more future project to move together ] </p>";
                        sub = "ALERT: Reminder To <CompanyName> from Beng Hui Marine Electrical Pte Ltd dated <" + DateTime.Today.ToString("dd MMM yyyy") + ">";
                    }
                    else if (_RptName.Equals("zState_ReminderAdd2.rpt"))
                    {
                        imsg = "<p style=\"font-size:11.0pt\"> Dear Valued Customer of Beng Hui Marine,</p> <p style=\"font-size:11.0pt\"> Attached Letters – 2nd Reminder to inform everyone that we still have not received any payment yet from date given and BHM Bank Account Detail.</p>"

                            + "<p style=\"font-size:11.0pt\">  OR </p><p style=\"font-size:11.0pt\"> "
                           + "Please reply us with payment advice <span style=\"font-weight:bold;text-decoration: underline\">[ attachment ]</span>, to allow us to monitor incoming payment together.</p>"
                           + " <p style=\"font-size:11.0pt\"> Your reply is important to us.</p>"
                            + "<p style=\"font-size:11.0pt\"> Thank you.</p>"
                           + "</p><p style=\"font-size:11.0pt;font-weight:bold;text-decoration: underline\"> Remarks: [ In order to maintain our goodwill relationship, please expedite payment process and avoid further delay as we have more future project to move together ] </p><p style=\"font-size:11.0pt\"> For enquiries and clarifications, please email to payment@bhglobal.com.sg or call to +65 6210 4746</p>";
                        sub = "ALERT: 2nd  Reminder To <CompanyName> from Beng Hui Marine Electrical Pte Ltd dated <" + DateTime.Today.ToString("dd MMM yyyy") + ">";
                    }
                    else if (_RptName.Equals("zState_ReminderAdd3.rpt"))
                    {
                        imsg = "<p style=\"font-size:11.0pt\"> Dear Valued Customer of Beng Hui Marine,</p>"
                        + " <p style=\"font-size:11.0pt\"> We would like to inform that your account is now seriously overdue. Despite several reminders, we have not received any payment or heard from you.</p> "
                               + "<p style=\"font-size:11.0pt\">This is the final reminder and we seek your co-operation to settle the outstanding accounts. We will proceed with legal action if we do not receive your settlement within the next 5 working days.</p>"
                            + "<p style=\"font-size:11.0pt\">If you have any queries, please contact Ling Yenn / Ivy at 6291 4444 or <a href=\"mailto:lingyenn@bhglobal.com.sg\">lingyenn@bhglobal.com.sg</a> / <a href=\"mailto:ivy.yap@bhholding.sg\">ivy.yap@bhholding.sg</a>.</p>"
                        + "<p style=\"font-size:11.0pt\">Please disregard this letter if you have already sent us your payment.</p><p style=\"font-size:11.0pt\"> For enquiries and clarifications, please email to payment@bhglobal.com.sg or call to +65 6210 4746</p>"
                        + "<p style=\"font-size:11.0pt\"> Thank you.</p>";

                        sub = "ALERT: FINAL REMINDER To <CompanyName> from Beng Hui Marine Electrical Pte Ltd dated <" + DateTime.Today.ToString("dd MMM yyyy") + ">";
                    }
                    else if (_RptName.Equals("zState_Warning.rpt"))
                    {
                        imsg = "<p style=\"font-size:11.0pt\"> Dear Valued Customer of Beng Hui Marine,</p>"
                        + " <p style=\"font-size:11.0pt\"> Attached Letters – WARNING LETTER to inform you that we have not received any payment or reply from you despite sending the final reminder and BHM Bank Account Detail.</p> "
                               + "<p style=\"font-size:11.0pt\">We have no choice but to commence legal action against <CompanyName>.</p>"
                        + "<p style=\"font-size:11.0pt\"> However if we receive payment from you to settle the accounts, we will cancel the legal action accordingly.</p><p style=\"font-size:11.0pt\"> For enquiries and clarifications, please email to payment@bhglobal.com.sg or call to +65 6210 4746</p>"
                        + "<p style=\"font-size:11.0pt\"> Thank you.</p>";

                        sub = "ALERT: WARNING LETTER To <CompanyName> from Beng Hui Marine Electrical Pte Ltd dated <" + DateTime.Today.ToString("dd MMM yyyy") + ">";
                    }
                }
            }
                
            DataTable dtCust = GetSelectedCustomer(All);
            frmStatementEmail f = new frmStatementEmail(dtCust,sub,imsg,ccs);



            if (f.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //DataSet dsReportData = LoadReportData(dtCust);
                    DataSet dsSrc = new DataSet();

                    string keys = GetSelectedCustomerKeys(All);
                    if (dsReportData.Tables.Count > 1)
                    {
                        dsReportData.Tables[0].TableName = "Header";
                        dsReportData.Tables[1].TableName = "Detail"; //Sub Report Data   

                        if (GFunc.NEInt(StatementType.Value, 0) == 2265 || GFunc.NEInt(StatementType.Value, 0) == 2270)
                        {
                            dsReportData.Tables[0].DefaultView.RowFilter = "DocConKey IN(" + keys + ")";
                            dsReportData.Tables[1].DefaultView.RowFilter = "DocConKey IN(" + keys + ")";
                        }

                        dsSrc.Tables.Add(dsReportData.Tables[0].DefaultView.ToTable());
                        dsSrc.Tables.Add(dsReportData.Tables[1].DefaultView.ToTable());
                    }
                    else
                    {
                        dsReportData.Tables[0].TableName = "Header";
                        dsReportData.Tables[0].DefaultView.RowFilter = "DocConKey IN(" + keys + ")";

                        dsSrc.Tables.Add(dsReportData.Tables[0].DefaultView.ToTable());
                    }

                    if (!ExistData(dsSrc))
                        return;                   

                
                    DateTime _AgeDate = DateTo.DateValue.Value;
                    string _SubReport = "";
                    string repTitle = "";
                    DataTable dt = f.dtCustEmail;  

                    if ((int)CCBKey.Value == 10)
                        repTitle = "STATEMENT OF ACCOUNT";
                    else
                        repTitle = "CASH TERM STATEMENT OF ACCOUNT";

                    List<SqlParameter> parmList = new List<SqlParameter>();

                    //When Data cannot be retrive from LoadReportData we will stop the process

                    List<ReportParameter> repParas = new List<ReportParameter>();

                    string opCmpValue = SysOptionUtility.GetStr("CompanyName");
                    string opCmpRegValue = SysOptionUtility.GetStr("CompanyRegNumber");
                    int opaddrKey = SysOptionUtility.GetInt("DefaultLetterHeadAddr");


                    if (_RepKey != 2290 && _RepKey != 2275 && _RepKey != 2280 && _RepKey != 2285 && _RepKey != 1335) //if Not Reminder
                    {
                        DataTable dtSub = GFunc.ExecuteQuery("select * from SYS_RepPara where ParName='pSubRepPath' and RepKey = " + _RepKey);

                        //dtSub.DefaultView.Count.ToString()
                        if (dtSub != null)
                            _SubReport = GFunc.NEStr(dtSub.Rows[0]["ParDefaultValue"], "");
                    }
                    string opCmpAddrValue = GFunc.AddrGet(opaddrKey, true);

                    repParas.Add(new ReportParameter("pCmpName", opCmpValue));
                    repParas.Add(new ReportParameter("pCmpAddr", opCmpAddrValue));
                    repParas.Add(new ReportParameter("pRepTitle", repTitle));

                    //Assign DateTo parameter to Rpx for Reminder with additional days
                    if (AddDays.Visible)
                        repParas.Add(new ReportParameter("pAddDays", GFunc.NEInt(AddDays.Value, 0)));

                    _AgeDate = new DateTime(_AgeDate.Year, _AgeDate.Month, DateTime.DaysInMonth(_AgeDate.Year, _AgeDate.Month));
                    //Assign DateTo parameter to Rpx for BF Statment only
                    if (_RepKey == 2270 || _RepKey == 1335 || _RepKey == 2265)
                    {
                        repParas.Add(new ReportParameter("pStatementDate", _AgeDate));
                    }

                    if (_RepKey != 2290 && _RepKey != 2275 && _RepKey != 2280 && _RepKey != 2285 && _RepKey != 1335) //if Not Reminder
                    {
                        string SubReportPath = Application.StartupPath.ToString() + "\\" + _SubReport;
                        repParas.Add(new ReportParameter("pSubRepPath", SubReportPath));
                        for (int i = 0; i < 12; i++)
                        {
                            repParas.Add(new ReportParameter("pMth" + (i + 1).ToString(), _AgeDate.AddMonths(i * -1).ToString("dd-MMM-yyyy")));
                        }
                    }
                    else
                    {
                        repParas.Add(new ReportParameter("pStatementDate", _AgeDate));
                    }


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dsSrc.Tables[0].DefaultView.RowFilter = "DocConKey=" + GFunc.NEInt(dt.Rows[i]["ConKey"], 0);
                        DataSet ds = new DataSet();
                        DataTable dtH = dsSrc.Tables[0].DefaultView.ToTable();

                        if (dtH.Rows.Count == 0)
                            continue;
                        DataTable dtD = new DataTable();
                        dtH.TableName = "Header";

                        if (dsReportData.Tables.Count > 1)
                        {
                            dsSrc.Tables[1].DefaultView.RowFilter = "DocConKey=" + GFunc.NEInt(dt.Rows[i]["ConKey"], 0);

                            dtD = dsSrc.Tables[1].DefaultView.ToTable();
                            dtD.TableName = "Detail"; //Sub Report Data              
                        }

                        ds.Tables.Add(dtH);
                        ds.Tables.Add(dtD);

                        CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc = LoadReport(_RptName, ds, _SubReport, repParas);

                        string custNm = GFunc.NEStr(dt.Rows[i]["ConID"], "");
                        custNm = custNm.Replace("/", "-");
                        custNm = custNm.Replace("\"", "-");
                        string fileNm = System.IO.Path.GetTempPath() + "\\" + _AgeDate.Year.ToString() + _AgeDate.Month.ToString("00") + "_" + custNm + ".pdf";

                        if (CreateReportFile(fileNm, rptDoc) == false)
                            break;

                        string subject = f.subject.Replace("<Month Year>", "<" + _AgeDate.ToString("MMMM") + " " + _AgeDate.Year.ToString() + ">");
                        subject = subject.Replace("CompanyName", GFunc.NEStr(dt.Rows[i]["ConID"], ""));

                        string msg = f.msg.Replace("<Month Year>", "[" + _AgeDate.ToString("MMMM") + " " + _AgeDate.Year.ToString() + "]");
                        msg = msg.Replace("[Month-Year]", "[" + _AgeDate.ToString("MMMM") + "-" + _AgeDate.Year.ToString() + "]");

                       // msg = f.msg.Replace("<Month Year>", "<" + _AgeDate.ToString("MMMM") + " " + _AgeDate.Year.ToString() + ">");
                        if (GFunc.NEStr(dt.Rows[i]["ContactPerson"], "").Contains(","))
                            msg = msg.Replace("<Contact>", "All");
                        else
                            msg = msg.Replace("<Contact>", GFunc.NEStr(dt.Rows[i]["ContactPerson"], ""));

                        msg = msg.Replace("<CompanyName>", "&lt;" + GFunc.NEStr(dt.Rows[i]["ConID"], "") + "&gt;");
                        
                        //changed by nnt 22nd April 2019 to send SOA and reminder email by payment@bhglobal.com.sg
                        string toEmail = GFunc.NEStr(dt.Rows[i]["ContactNum"], "");
                        toEmail = toEmail.Replace(",", ";");
                        if (f.DisplayInOutlook)
                        {
                            string[] to = GFunc.NEStr(dt.Rows[i]["ContactNum"], "").Split(',');
                            OutlookEmail.SendEmail(to, subject, msg, new string[] { fileNm }, f.ccs, f.DisplayInOutlook, true);
                        }
                        else
                        {
                            System.Net.Mail.Attachment attach = null;
                            if (fileNm.Length > 0)
                            {
                                attach = new System.Net.Mail.Attachment(fileNm);
                            }
                            GEmail.SendEmailCC(toEmail, subject, msg, attach, f.ccs);
                        }
                        //end changed by nnt 22nd April 2019

                        System.IO.File.Delete(fileNm);
                        SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.EmailSent, GEnum.SystemCode.Customer,
                           GFunc.NEInt(StatementType.Value, 0), GFunc.NEStr(dt.Rows[i]["ConID"], ""), DateTime.Today,
                           GFunc.NEStr(dt.Rows[i]["ContactNum"], "") + (f.ccs == "" ? " " : ", CC:") + f.ccs,
                            new string[] { subject, GFunc.NEStr(dtH.Rows[0]["CurrID"], "") + " " + GFunc.NEStr(dtH.Rows[0]["DueAmtF"], "") }, DateTo.DateValue.Value);
                    }
                }
                catch (Exception ex)
                {
                    SysAuditLogUtility.AddErrorLog(ex);
                }
            }
            f.Close();
        }

        private void btnSavePdf_Click(object sender, EventArgs e)
        {
            if (Validation(false) == false)
                return;

            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.RootFolder = Environment.SpecialFolder.Desktop;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                frmMain.gfrmMain.SetNotifyStatus("Working ...............");
               // ReportLoader _ReportLoader = null;
                DataTable dtCustomer = GetSelectedCustomer(false);
                if (dtCustomer.Rows.Count == 0)
                {
                    MsgBox.Show("No customers found. Please refresh the customers list");
                    frmMain.gfrmMain.SetNotifyStatus("Ready");
                    return;
                }
                //DataSet dsReportData = LoadReportData(dtCustomer); 
                int _RepKey = (int)StatementType.Value;
                string _RptName = this.RptFile.SelectedRow.Cells["RptNm"].Value.ToString();                
                DateTime _AgeDate = DateTo.DateValue.Value;
                string _SubReport = "";
                string repTitle;

                if ((int)CCBKey.Value == 10)
                    repTitle = "STATEMENT OF ACCOUNT";
                else
                    repTitle = "CASH TERM STATEMENT OF ACCOUNT";

                List<SqlParameter> parmList = new List<SqlParameter>();

                //When Data cannot be retrive from LoadReportData we will stop the process
           
                List<ReportParameter> repParas = new List<ReportParameter>();


                string opCmpValue = SysOptionUtility.GetStr("CompanyName");
                string opCmpRegValue = SysOptionUtility.GetStr("CompanyRegNumber");
                int opaddrKey = SysOptionUtility.GetInt("DefaultLetterHeadAddr");


                if (_RepKey != 2290 && _RepKey != 2275 && _RepKey != 2280 && _RepKey != 2285 && _RepKey != 1335) //if Not Reminder
                {
                    DataTable dtSub = GFunc.ExecuteQuery("select * from SYS_RepPara where ParName='pSubRepPath' and RepKey = " + _RepKey);

                    //dtSub.DefaultView.Count.ToString()
                    if (dtSub != null)
                        _SubReport = GFunc.NEStr(dtSub.Rows[0]["ParDefaultValue"], "");
                }
                string opCmpAddrValue = GFunc.AddrGet(opaddrKey, true);

                repParas.Add(new ReportParameter("pCmpName", opCmpValue));
                repParas.Add(new ReportParameter("pCmpAddr", opCmpAddrValue));
                repParas.Add(new ReportParameter("pRepTitle", repTitle));

                //Assign DateTo parameter to Rpx for Reminder with additional days
                if (AddDays.Visible)
                    repParas.Add(new ReportParameter("pAddDays", GFunc.NEInt(AddDays.Value, 0)));

                _AgeDate = new DateTime(_AgeDate.Year, _AgeDate.Month, DateTime.DaysInMonth(_AgeDate.Year, _AgeDate.Month));
                //Assign DateTo parameter to Rpx for BF Statment only
                if (_RepKey == 2270 || _RepKey == 1335 || _RepKey == 2265)
                {
                    repParas.Add(new ReportParameter("pStatementDate", _AgeDate));
                }

                if (_RepKey != 2290 && _RepKey != 2275 && _RepKey != 2280 && _RepKey != 2285 && _RepKey != 1335) //if Not Reminder
                {
                    string SubReportPath = Application.StartupPath.ToString() + "\\" + _SubReport;
                    repParas.Add(new ReportParameter("pSubRepPath", SubReportPath));
                    for (int i = 0; i < 12; i++)
                    {
                        repParas.Add(new ReportParameter("pMth" + (i + 1).ToString(), _AgeDate.AddMonths(i * -1).ToString("dd-MMM-yyyy")));
                    }
                }
                else
                {
                    repParas.Add(new ReportParameter("pStatementDate", _AgeDate));
                }

                //_ReportLoader.ReportParameter = repParas;
                //_ReportLoader.RepKey = _RepKey;

                for (int i = 0; i < dtCustomer.Rows.Count; i++)
                {
                    dsReportData.Tables[0].DefaultView.RowFilter = "DocConKey=" + GFunc.NEInt(dtCustomer.Rows[i]["ConKey"], 0);
                    DataSet ds = new DataSet();
                    DataTable dtH = dsReportData.Tables[0].DefaultView.ToTable();
                    DataTable dtD = new DataTable();
                    dtH.TableName = "Header";

                    if (dsReportData.Tables.Count > 1)
                    {
                        dsReportData.Tables[1].DefaultView.RowFilter = "DocConKey=" + GFunc.NEInt(dtCustomer.Rows[i]["ConKey"], 0);

                        dtD = dsReportData.Tables[1].DefaultView.ToTable();
                        dtD.TableName = "Detail"; //Sub Report Data              
                    }

                    ds.Tables.Add(dtH);
                    ds.Tables.Add(dtD);

                    CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc= LoadReport(_RptName, ds, _SubReport,repParas);

                    string custNm = GFunc.NEStr(dtCustomer.Rows[i]["ConID"], "");
                    string fileNm=fd.SelectedPath + "\\" + _AgeDate.Year.ToString() + _AgeDate.Month.ToString("00") + "_" + custNm + ".pdf";
                    
                    if (CreateReportFile(fileNm,rptDoc) == false)
                        break;
                }
                frmMain.gfrmMain.SetNormalStaus("Ready");
            }
        }

        private bool CreateReportFile(string fileNm,CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc)
        {
            bool fail = true;
            int i = 0;

            CrystalDecisions.Shared.DiskFileDestinationOptions dfdOptions = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            CrystalDecisions.Shared.ExportOptions exOptions = rptDoc.ExportOptions;

            exOptions = rptDoc.ExportOptions;
            exOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            exOptions.FormatOptions = null;
            exOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;

            exOptions.DestinationOptions = dfdOptions;

            while (fail && i < 10)
            {
                try
                {
                    dfdOptions.DiskFileName = fileNm;
                    rptDoc.Export();
                    rptDoc.Close();
                    fail = false;
                }
                catch
                {
                    i++;
                    fileNm = fileNm.Remove(fileNm.LastIndexOf(".")) + i.ToString() + ".pdf";
                }
            }

            return true;
        }

        private CrystalDecisions.CrystalReports.Engine.ReportDocument LoadReport(string rptNm, DataSet dsSource, string subRptNm, List<ReportParameter> para)
        {
            try
            {               
                CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rptDoc.Load(Application.StartupPath + @"\Reports\" + rptNm);
                rptDoc.SetDataSource(dsSource.Tables[0]);

                //Set Sup report datasource
                if (dsSource.Tables.Count > 1 && subRptNm!="")
                {
                    rptDoc.OpenSubreport(subRptNm).SetDataSource(dsSource.Tables[1]);
                }

                List<ReportParameter> rptParams = para;

                foreach (ReportParameter p in rptParams)
                {
                    if (p.ParameterName != "pSubRepPath")
                    {
                        rptDoc.SetParameterValue(p.ParameterName, p.ParameterValue);
                    }
                }
                return rptDoc;
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

        private void tachkEmailSOA_CheckedChanged(object sender, EventArgs e)
        {
            GridFilter(GFunc.NEInt(StatementType.Value, 0) == 2265 || GFunc.NEInt(StatementType.Value, 0) == 2270);
        }

        private void tacboDueOption_CustomUpdate(object sender, CancelEventArgs e)
        {
            GridFilter(GFunc.NEInt(StatementType.Value, 0) == 2265 || GFunc.NEInt(StatementType.Value, 0) == 2270);
        }

        private void SendType_CustomUpdate(object sender, CancelEventArgs e)
        {
            GridFilter(GFunc.NEInt(StatementType.Value, 0) == 2265 || GFunc.NEInt(StatementType.Value, 0) == 2270);
        }

        private void btnEmailLog_Click(object sender, EventArgs e)
        {
            try
            {
                frmMain.gfrmMain.SetNotifyStatus("Loading ...............");

                int StaType = GFunc.NEInt(StatementType.Value, 0);
                if (StaType != 2270 && StaType != 2265)
                    return;

                List<SqlParameter> pList = new List<SqlParameter>();
                pList.Add(new SqlParameter("@StatementType", StaType));
                pList.Add(new SqlParameter("@Date", DateTo.DateValue));

                DataTable dt = new DataTable();
                using (SqlConnection cn = new SqlConnection(Database.BOSSSystemMasterConnection))
                {
                    dt = GFunc.ExecuteProc(cn, "Rep_CustStaEmailLog", pList);
                }

                if (dt.Rows.Count == 0)
                {
                    MsgBox.Show("No log record to show.");
                    return;
                }
               
                ReportLoader _ReportLoader = new ReportLoader();

                string RptName = "CustStaEmailLog.rpt";

                List<ReportParameter> repParas = new List<ReportParameter>();

                string opCmpValue = SysOptionUtility.GetStr("CompanyName");

                repParas.Add(new ReportParameter("pCmpName", opCmpValue));
                repParas.Add(new ReportParameter("pRepRange", StatementType.Text + " As At " + DateTo.DateValue.Value.ToString("dd MMM yyyy")));
                _ReportLoader.ReportParameter = repParas;
                _ReportLoader.RepKey = StaType;

               
                _ReportLoader.LoadCrystalReport(dt, RptName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                frmMain.gfrmMain.SetNormalStaus("Ready");    
            }
        }
    }
}
