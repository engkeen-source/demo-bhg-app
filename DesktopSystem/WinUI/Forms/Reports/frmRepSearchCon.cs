using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using BOLib;
using Infragistics.Win.UltraWinEditors;
using System.Transactions;
using TAUtil;

namespace WinUI
{
    public partial class frmRepSearchCon : Form
    {
        int SearchNumChar = 1;
        Boolean SearchFromStart = false;
        /* ItmHisSearchType from SYS_MsgList
       MsgValue	DataDes
      10	    Invoice
      20	    Delivery Order
      30	    Sales Order
      40	    Quotation
      50	    Invoice and Pending DO
      90	    All
      100	    Invoice
      110	    Delivery
      120	    Purchase Order
       */

        #region Local Variable        
        string ContextMenuSetting = string.Empty;
        GEnum.SystemCode CodeKey;
        int RepKey=0;
        int ITEM_PURCHASE_BY_VENDOR = 1850;
        int ITEM_SALES_BY_CUSTOMER = 1310;
        int CUSTOMER_SALES_BY_ITEM = 1845;
        DataTable dtSearchCon = null;
        private ReportLoader _ReportLoader = null;

        #endregion

        //Initialize
        public frmRepSearchCon()
        {
            InitializeComponent();
           
        }
        public frmRepSearchCon(GEnum.SystemCode CodeKey)
        {
            InitializeComponent();
            this.CodeKey = CodeKey;
            if (CodeKey == GEnum.SystemCode.Customer)
                RepKey = 1310;
            else
                RepKey = 1555;
            SetInitialSetting();
        }
        public frmRepSearchCon(int RepKey)
        {
            InitializeComponent();
            this.RepKey = RepKey;
            if (RepKey == ITEM_SALES_BY_CUSTOMER || RepKey == CUSTOMER_SALES_BY_ITEM)
                this.CodeKey = GEnum.SystemCode.Customer;
            else
                this.CodeKey = GEnum.SystemCode.Vendor;

            SetInitialSetting();
        }
        //Form
        private void frmRepSearchCon_Load(object sender, EventArgs e)
        {
            try
            { 
                //Setup FORM control/grid format, menu, listID
                GlobalUI.FormGrids_Set(this,(int)CodeKey, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)CodeKey);
                GlobalUI.Combos_Fill(this, (int)CodeKey);               
                SearchType.SetValueTrigger(10,false); //Invoice
                
                DateFrom.SetValueTrigger(DateTime.Today.AddMonths(-1),false);
                DateTo.SetValueTrigger(DateTime.Today, false);
                
                tagrdItms.ActiveRow = null;

                SearchNumChar = SysOptionUtility.SearchItemNumChar;
                if (SysOptionUtility.SearchItemMatch == GEnum.SearchMatchOption.StartofField)
                    SearchFromStart = true;
                else
                    SearchFromStart = false;
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

        private void frmRepSearchCon_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                    GlobalUI.Combos_Fill(this, 0);

                //Set Focus Next Control
                GlobalUI.SelectNextControl(this, e);
            }
            catch (TAException ex)
            {
                //Error(ex,true);
                MsgBox.Show(ex.MsgID);
            }
            catch (Exception ex)
            {
                //Error(ex,true);
                MsgBox.Show(ex.Message);
            }
        }          

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validation(true))
                    RefreshData();

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
            this.Close();
        }

        private bool Validation(bool showMessage)
        {  
            if (GFunc.IsNEZ(ConKey.Value))
            {
                if (showMessage)
                {
                    if(CodeKey == GEnum.SystemCode.Customer)
                        MsgBox.Show("Invalid Customer. Please select a Customer.");
                    else
                        MsgBox.Show("Invalid Vendor. Please select a Vendor.");
                    ConKey.Focus();
                }
                return false;
            }
            if (DateFrom.DateValue == null)
            {
                if (showMessage)
                {
                    MsgBox.Show("Invalid From Date. Please enter a Date Value.");
                    DateFrom.Focus();
                }
                return false;
            }

            if (DateTo.DateValue == null)
            {
                if (showMessage)
                {
                    MsgBox.Show("Invalid From Date. Please enter a Date Value.");
                    DateTo.Focus();
                }
                return false;
            }

            return true;
        }
        private void RefreshData()
        {
            string itmWildcard = Searchformat.Text.Trim();
            string spName = "";
            try
            {
                frmMain.gfrmMain.SetNotifyStatus("Loading Report ......");
                if (CodeKey == GEnum.SystemCode.Customer)
                    spName = "Rep_SearchConSale";
                else if (CodeKey == GEnum.SystemCode.Vendor)
                    spName = "Rep_SearchConPurchase";
                if (ItmIDFrom.Text.Trim() == ""&& ItmIDTo.Text.Trim() == "" && Searchformat.Text.Trim() == "")
                    itmWildcard = "%";

                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@SearchType", GFunc.NEInt(SearchType.Value, 0)));
                parmList.Add(new SqlParameter("@ConKey", GFunc.NEInt(ConKey.Value, 0)));
                parmList.Add(new SqlParameter("@ItmIDFrom", ItmIDFrom.Text));
                parmList.Add(new SqlParameter("@ItmIDTo", ItmIDTo.Text));

                parmList.Add(new SqlParameter("@ItmIDWild", itmWildcard));
                parmList.Add(new SqlParameter("@DateFrom", DateFrom.DateValue));
                parmList.Add(new SqlParameter("@DateTo", DateTo.DateValue));
                parmList.Add(new SqlParameter("@DocDetItmDes", SearchItmDes.Text));

                dtSearchCon = GFunc.ExecuteProc(spName, parmList);
                tagrdItms.DataSource = dtSearchCon;              

                FilterGrid();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                frmMain.gfrmMain.SetNormalStaus("Ready");
            }
        }
        private void SetInitialSetting()
        {
            switch (CodeKey)
            {
                case GEnum.SystemCode.Customer:
                    //Designer Setting already set for Customer
                    break;
                case GEnum.SystemCode.Vendor:
                    lblHeader.Text = "Vendor Purchase";
                    this.Text = "Report [Searh for Purchase by Vendor]";
                    lblCon.Text = "Vendor";
                    break;
            }
        }
       
        private void tagrdItms_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Bands[0].Summaries.Clear();
           
            e.Layout.Bands[0].Summaries.Add(Infragistics.Win.UltraWinGrid.SummaryType.Sum, e.Layout.Bands[0].Columns["ItmQty"]);
            e.Layout.Bands[0].Summaries.Add(Infragistics.Win.UltraWinGrid.SummaryType.Sum, e.Layout.Bands[0].Columns["ItmAmtF"]);
            e.Layout.Bands[0].Summaries.Add(Infragistics.Win.UltraWinGrid.SummaryType.Sum, e.Layout.Bands[0].Columns["ItmAmtH"]);
            e.Layout.Bands[0].Summaries.Add(Infragistics.Win.UltraWinGrid.SummaryType.Count, e.Layout.Bands[0].GetFirstVisibleCol(e.Layout.ColScrollRegions[0],false));

            e.Layout.Bands[0].Summaries[0].DisplayFormat = "{0:#,##0.00##}";
            e.Layout.Bands[0].Summaries[1].DisplayFormat = "{0:#,##0.00##}";
            e.Layout.Bands[0].Summaries[2].DisplayFormat = "{0:#,##0.00##}";
            e.Layout.Bands[0].Summaries[3].DisplayFormat = "Total =>";          
            e.Layout.Bands[0].Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;

            e.Layout.Bands[0].Override.SummaryFooterAppearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            e.Layout.Bands[0].Summaries[0].Appearance.BackColor = SystemColors.Window;
            e.Layout.Bands[0].Summaries[1].Appearance.BackColor = SystemColors.Window;
            e.Layout.Bands[0].Summaries[2].Appearance.BackColor = SystemColors.Window;
            e.Layout.Bands[0].Summaries[3].Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            e.Layout.Bands[0].Summaries[3].Appearance.BorderAlpha = Infragistics.Win.Alpha.Transparent;

            e.Layout.Bands[0].Summaries[0].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            e.Layout.Bands[0].Summaries[1].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            e.Layout.Bands[0].Summaries[2].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            e.Layout.Bands[0].Summaries[3].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;         

            e.Layout.Bands[0].Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.BottomFixed;            
            e.Layout.Override.SummaryFooterSpacingBefore = 5;
            e.Layout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
        }

        private void SearchType_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (Validation(false))
                RefreshData();
        }
        private void ConKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int Key = GFunc.GetExistingRecKey(ConKey.Text, CodeKey, true , true);
                if (GFunc.IsNEZ(Key))
                {
                    ConKey_EditorButtonClick(sender, null);
                    if (GFunc.IsNEZ(Key) && Validation(false))
                        RefreshData();
                }
                else
                {
                    MSTCon objItm = MSTCon.Get(Key);
                    ConKey.SetValueTrigger(objItm.ConKey, false);

                    if (Validation(false))
                        RefreshData();
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
        private void ConKey_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int Key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ((Control)sender).Name);

                if (DocHDRUtil.EditorButton_Popup((int)CodeKey, ConKey.Text, listSettingID, (int)GEnum.PopupType.CusID, ref Key, ref id, ref des))
                {
                    ConKey.SetValueTrigger(id, false);
                    if (Validation(false))
                        RefreshData();
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
        private void ItmIDFrom_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;

            string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItmID");

            if (DocHDRUtil.EditorButton_Popup(0, ItmIDFrom.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref key, ref id, ref des))
            {                
                ItmIDFrom.SetValueTrigger(id,false);                
            }
        }
        private void ItmIDFrom_CustomUpdate(object sender, CancelEventArgs e)
        {           
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;

            if (ItmIDFrom.Text == "")
                return;
            string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItmID");

            key = GFunc.ItmRecord_GetKey(GEnum.RecAccessType.ItemID, listSettingID, ItmIDFrom.Text, 0, ref id, ref des, true); //Record_GetKey((int)objInterestFactory.ConstantCodeKey, GEnum.RecAccessTypeItem.ItemID, ItmID.Text, ref id, ref des, true);
            if (key == 0)
            {
                //since value input by user cannot be match let the user select from Popup form
                if (DocHDRUtil.EditorButton_Popup(0, ItmIDFrom.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref key, ref id, ref des))
                {                    
                    ItmIDFrom.SetValueTrigger(id,false);
                }
                else
                {
                    //when user is still unable to select a matching record, undo the changes
                    MsgBox.Show("Please use a valid value");
                    e.Cancel = true;
                }
            }
            else
            {               
                ItmIDFrom.SetValueTrigger(id,false);
            }
        }
        private void ItmIDTo_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;
           
            string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItmID");

            if (DocHDRUtil.EditorButton_Popup(0, ItmIDTo.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref key, ref id, ref des))
            {              
                ItmIDTo.SetValueTrigger(id,false);               
            }
        }
        private void ItmIDTo_CustomUpdate(object sender, CancelEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;

            if (ItmIDTo.Text == "")
                return;

            string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItmID");

            key = GFunc.ItmRecord_GetKey(GEnum.RecAccessType.ItemID, listSettingID, ItmIDTo.Text, 0, ref id, ref des, true); //Record_GetKey((int)objInterestFactory.ConstantCodeKey, GEnum.RecAccessTypeItem.ItemID, ItmID.Text, ref id, ref des, true);
            if (key == 0)
            {
                //since value input by user cannot be match let the user select from Popup form
                if (DocHDRUtil.EditorButton_Popup(0, ItmIDFrom.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref key, ref id, ref des))
                {
                    ItmIDTo.SetValueTrigger(id,false);                   
                }
                else
                {
                    //when user is still unable to select a matching record, undo the changes
                    MsgBox.Show("Please use a valid value");
                    e.Cancel = true;
                }
            }
            else
            {
                ItmIDTo.SetValueTrigger(id,false);
            }
        }       

        private void Searchformat_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (Searchformat.Text == string.Empty)
                ItmIDTo.Enabled = true;
            else
            {
                ItmIDTo.Enabled = false;
                ItmIDTo.SetValueTrigger("",false);
            }
        }    

        private void Combo_NotInListAdd(object sender, ValidationErrorEventArgs e)
        {
            try
            {
                GlobalUI.ItemNotInList(sender as TAUtil.TAComboBox, e, true, 0);
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
        }//CodeCompleted

        private void btnPrintReport_Click(object sender, EventArgs e)
        {  
            //show Print Dialog 
            //frmPrintSelection print = new frmPrintSelection(dtSearchCon, 0, 1310, 0);
            //print.ShowDialog(); 
            _ReportLoader = new ReportLoader();

            string RptName = "";
            if(RepKey == 1310 ) //Item Sales By Customer 
                switch (GFunc.NEInt(SearchType.Value, 0))
                {
                    case 10: //Invoice
                        RptName = "S_Cust_Invoice.rpt";
                        break;
                    case 20://Delivery_Order
                        RptName = "S_Cust_DeliveryOrder.rpt";
                        break;
                    case 30://Sales_Order
                        RptName = "S_Cust_SalesOrder.rpt";
                        break;
                    case 40://Quotation
                        RptName = "S_Cust_Quotation.rpt";
                        break;
                    case 50://Invoice_and_Pending_DO
                        RptName = "S_Cust_IVDO.rpt";
                        break;
                    case 60://Invoice_and_Pending_DO
                        RptName = "S_Cust_Consignment.rpt";
                        break;
                    default:
                        RptName = "S_Cust_Sales.rpt";
                        break;
                }
            else //Item Purchase By Vendor 
                switch (GFunc.NEInt(SearchType.Value,0))
                {
                    case 10: //Invoice
                        RptName = "S_Vend_Invoice.rpt";
                        break;
                    case 20://Delivery
                        RptName = "S_Vend_Delivery.rpt";
                        break;
                    case 30://Purchase_Order
                        RptName = "S_Vend_PurchaseOrder.rpt";
                        break;
                    case 40://Consignment
                        RptName = "S_Vend_BLNoInvPD.rpt";
                        break;
                    case 50://Consignment
                        RptName = "S_Vend_Consignment.rpt";
                        break;                 
                    default:
                        RptName = "S_Vend_Purchase.rpt";
                        break;                  
                }
         
                CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rptDoc.Load(Application.StartupPath + @"\Reports\" + RptName);

                DataTable dt = dtSearchCon.DefaultView.ToTable();
                rptDoc.SetDataSource(dt);
             
                List<ReportParameter> rptParams = GetReportParameters();
                foreach (ReportParameter p in rptParams)
                {
                    rptDoc.SetParameterValue(p.ParameterName, p.ParameterValue);
                }

                frmReportViewer fRptViewer = new frmReportViewer();
                fRptViewer.RepKey = RepKey;
                fRptViewer.RptName = RptName;
                fRptViewer.RptDocument = rptDoc;
                fRptViewer.MdiParent = frmMain.gfrmMain;
                fRptViewer.Show();
          
        }
        private List<ReportParameter> GetReportParameters()
        {
            //Set Parameter For Report Which Are Already Define In This Form(Show Total,Show Price,ect.) If There Are No Parameter, Process Also OK.
            try
            {
                List<ReportParameter> l_Reval = new List<ReportParameter>();

                string opCmpValue = SysOptionUtility.GetStr("CompanyName");
              
                string pRepRange = "";
                pRepRange = "DATE BETWEEN \"" + DateFrom.Value + "\" AND \"" + DateTo.Value+ "\"";
                if (ItmIDFrom.Text != "" || ItmIDTo.Text != "")
                {
                    if (ItmIDTo.Text == "" && Searchformat.Text == "")
                        pRepRange += ", Item ID Like \"" + ItmIDFrom.Text + "%\"";
                    else if (ItmIDTo.Text != "")
                        pRepRange += ", Item ID Between \"" + ItmIDFrom.Text + "\" AND \"" + ItmIDTo.Text + "\"";
                    else if (Searchformat.Text != "")
                        pRepRange += ", Item ID Like \"" + ItmIDFrom.Text + Searchformat.Text + "\"";
                }

                if (SearchItmDes.Text != "")
                {
                    pRepRange += ", " + SearchType.Text + " Item Des Like \"" + SearchItmDes.Text + "%" + "\"";
                }
                // can't use objRepListFactory.ObjRepParameters. because there is no records in sys_repCriteria for printing.
                ReportParameter l_ParampCmpName = new ReportParameter("pCmpName", opCmpValue);
                ReportParameter l_ParampRepRange = new ReportParameter("pRepRange", pRepRange);
               

                l_Reval.Add(l_ParampCmpName);
                l_Reval.Add(l_ParampRepRange);

                return l_Reval;
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
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

        private void frmRepSearchCon_Shown(object sender, EventArgs e)
        {
            Searchformat.Focus();
        }

        private void SearchItmDes_TextChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (SearchItmDes.Text.Trim().Length >= SearchNumChar)
                {
                    FilterGrid();
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

        private void FilterGrid()
        {

            if (tagrdItms.DataSource == null)
                return;

            ((DataTable)tagrdItms.DataSource).DefaultView.RowFilter = "";            

            if (SearchItmDes.Text.Length > 0)
            {
                //GridFilterToDefaultView   
                if (SearchFromStart)
                    //tagrdSearchResult.DisplayLayout.Bands[0].ColumnFilters[currSearchCol].FilterConditions.Add(FilterComparisionOperator.StartsWith, SearchText.Text.Trim());
                    ((DataTable)tagrdItms.DataSource).DefaultView.RowFilter = "ItmDes like '" + SearchItmDes.Text + "%'";
                else
                    //tagrdSearchResult.DisplayLayout.Bands[0].ColumnFilters[currSearchCol].FilterConditions.Add(FilterComparisionOperator.Contains, SearchText.Text.Trim());
                    ((DataTable)tagrdItms.DataSource).DefaultView.RowFilter = "ItmDes like '%" + SearchItmDes.Text.Trim() + "%'";
            }
        }

        private void SearchItmDes_CustomUpdate(object sender, CancelEventArgs e)
        {
            FilterGrid();
        }

        private void btnFilterRefresh_Click(object sender, EventArgs e)
        {
            /* to clear the grid filter of the previous searched item -- added by YST on 2024-09-23 */
            try
            {
                if (Validation(true))
                {                    
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridBand band in tagrdItms.DisplayLayout.Bands)
                    {
                        band.ColumnFilters.ClearAllFilters();
                    }                 
                    RefreshData();
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
    }
}
