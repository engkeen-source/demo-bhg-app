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
    public partial class frmRepSearchItm : Form
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

        int ItmKey = 0;      
        string ContextMenuSetting = string.Empty;
        GEnum.SystemCode CodeKey;
        int RepKey = 0;
        DataTable dtSearchItm = null;
        DataSet dsSearchItm = null;
        private ReportLoader _ReportLoader = null;
        #endregion

        public frmRepSearchItm()
        {
            InitializeComponent();
            //RepKey = 1845;
            //SetInitialSetting();
        }

        //Initialize
        public frmRepSearchItm(GEnum.SystemCode CodeKey)
        {
            InitializeComponent();
            this.CodeKey = CodeKey;
            if (CodeKey == GEnum.SystemCode.Customer)
                RepKey = 1845;
            else
                RepKey = 1850;
            SetInitialSetting();
        }

        public frmRepSearchItm(int repKeyParam)
        {
            InitializeComponent();
            this.RepKey = repKeyParam;
            if (RepKey == 1845)
                CodeKey = GEnum.SystemCode.Customer;
            else if (RepKey == 1850)
                CodeKey = GEnum.SystemCode.Inventory;
            SetInitialSetting();
        }
       
        //Form
        private void frmRepSearchItm_Load(object sender, EventArgs e)
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
                DateAvailable.SetValueTrigger(DateTime.Today, false);
                tagrdItms.ActiveRow = null;             

                SearchNumChar = SysOptionUtility.SearchItemNumChar;
                if (SysOptionUtility.SearchItemMatch == GEnum.SearchMatchOption.StartofField)
                    SearchFromStart = true;
                else
                    SearchFromStart = false;

                if (!SECPermUtility.Perform("ItemViewCost", false))
                {
                    Cost.PasswordChar='*';
                    AvgCost.PasswordChar = '*';
                    ObCost.PasswordChar = '*';
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
        private void frmRepSearchItm_KeyDown(object sender, KeyEventArgs e)
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
                {
                    RefreshData();
                    //if(pnlEst2.Visible)
                    //    RefreshAvailableInfo();
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

        private bool Validation(bool showMessage)
        {            
            if (ItmKey == 0)
            {
                if (showMessage)
                {
                    MsgBox.Show("Invalid Item. Please select an Item.");
                    ItmID.Focus();
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
            string conWildcard = Searchformat.Text.Trim();
            string spName = "";

            try
            {
                frmMain.gfrmMain.SetNotifyStatus("Loading Report ......");
                if (CodeKey == GEnum.SystemCode.Customer)
                    spName = "Rep_SearchItmSale";
                else if (CodeKey == GEnum.SystemCode.Vendor)
                    spName = "Rep_SearchItmPurchase";
                
                if (ConFrom.Text.Trim()=="" && ConTo.Text.Trim() == "" && Searchformat.Text.Trim() == "")
                    conWildcard = "%";

                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@SearchType", GFunc.NEInt(SearchType.Value, 0)));
                parmList.Add(new SqlParameter("@ItmKey", ItmKey));
                parmList.Add(new SqlParameter("@ConFrom", ConFrom.Text));
                parmList.Add(new SqlParameter("@ConTo", ConTo.Text));
                parmList.Add(new SqlParameter("@ConWild", conWildcard));
                parmList.Add(new SqlParameter("@DateFrom", DateFrom.DateValue));
                parmList.Add(new SqlParameter("@DateTo", DateTo.DateValue));
                //parmList.Add(new SqlParameter("@DateAvailable", DateAvailable.DateValue));
                parmList.Add(new SqlParameter("@DocDetItmDes", ""));

                dsSearchItm = GFunc.ExecuteProcDataSet(spName, parmList);               

                if (dsSearchItm.Tables.Count >= 2)
                {
                    if (dsSearchItm.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = dsSearchItm.Tables[0].Rows[0];
                      
                        ControlPrice.SetValueTrigger(dr["ControlPriceH"], false);
                        Cost.Text = GFunc.NEDec(dr["CostLatest"], 0M).ToString("#,##0.######");
                        AvgCost.Text = GFunc.NEDec(dr["CostAvg"], 0M).ToString("#,##0.######");
                        ObCost.Text= GFunc.NEDec(dr["ObCost"], 0M).ToString("#,##0.######");
                        Stock.SetValueTrigger(dr["QtyStock"], false);
                        StockCopy.SetValueTrigger(dr["QtyStock"], false);
                        BUOMTx.SetValueTrigger(GFunc.NEStr(dr["BUOMID"], ""), false);
                        if(dsSearchItm.Tables[0].Columns.Contains("ROQty"))
                            ROQty.Text = GFunc.NEDec(dr["ROQty"], 0M).ToString("#,##0.00");
                        SOQty.Text = GFunc.NEDec(dr["SOQty"], 0M).ToString("#,##0.00");
                        POQty.Text = GFunc.NEDec(dr["POQty"], 0M).ToString("#,##0.00");
                        ShippedQty.SetValueTrigger(dr["APPNQty"], false);

                        if (dsSearchItm.Tables[0].Columns.Contains("ItmType"))
                        {
                            if (GFunc.NEInt(dr["ItmType"], 0) == 250)
                                AvailableQty.Value = 0;
                            else
                                AvailableQty.SetValueTrigger(Stock.DecimalValue + GFunc.NEDec(POQty.Text, 0) + GFunc.NEDec(ShippedQty.Text, 0) - GFunc.NEDec(SOQty.Text, 0) - GFunc.NEDec(ROQty.Text, 0), false);
                        }
                        else
                            AvailableQty.SetValueTrigger(Stock.DecimalValue + GFunc.NEDec(POQty.Text, 0) + GFunc.NEDec(ShippedQty.Text, 0) - GFunc.NEDec(SOQty.Text, 0) - GFunc.NEDec(ROQty.Text, 0), false);

                        ControlPrice.Appearance.BackColor = System.Drawing.Color.Transparent;

                        decimal eStorePrice=Math.Round(GFunc.NEDec(dr["EStorePrice"],0),4);
                        decimal controlPrice = Math.Round(GFunc.NEDec(dr["ControlPriceH"], 0),4);
                        int itmtype = GFunc.NEInt(dr["ItmType"], 0);

                        if (itmtype == 100 || itmtype == 250 || itmtype == 600)
                        {
                            if (eStorePrice == -999)
                                ControlPrice.Appearance.BackColor = Color.Orange;
                            else if (eStorePrice == 0 && controlPrice > 0)
                                ControlPrice.Appearance.BackColor = Color.Khaki;
                            else if (eStorePrice > 0 && controlPrice != eStorePrice)
                                ControlPrice.Appearance.BackColor = Color.Red;
                            //else if (eStorePrice > 0 && controlPrice == 0)
                            //    ControlPrice.Appearance.BackColor = Color.Red;
                        }

                        if (!GFunc.IsNE(dr["LatestPODate"]))
                            lblPOData.Text = "Latest PO ETA Date =" + GFunc.NEDateTime(dr["LatestPODate"], DateTime.Today).ToString("dd MMM yyyy");
                        else
                            lblPOData.Text = "";
                        if (!GFunc.IsNE(dr["LatestRODate"]))
                            lblROData.Text = "Latest RO ETD Date =" + GFunc.NEDateTime(dr["LatestRODate"], DateTime.Today).ToString("dd MMM yyyy");
                        else
                            lblROData.Text = "";
                    }

                    dtSearchItm = dsSearchItm.Tables[1];
                    tagrdItms.DataSource = dsSearchItm.Tables[1];                    
                }

                /*    if(dsSearchItm.Tables.Count>=3)
                    {
                        if (dsSearchItm.Tables[2].Rows.Count > 0)
                        {
                            DataRow dr = dsSearchItm.Tables[2].Rows[0];                        
                            if (dsSearchItm.Tables[2].Columns.Contains("ROQty"))
                                ADROQty.Text = GFunc.NEDec(dr["ROQty"], 0M).ToString("#,##0.00");
                            ADSOQty.Text = GFunc.NEDec(dr["SOQty"], 0M).ToString("#,##0.00");
                            ADPOQty.Text = GFunc.NEDec(dr["POQty"], 0M).ToString("#,##0.00");
                            ADShippedQty.SetValueTrigger(dr["APPNQty"], false);

                            if (dsSearchItm.Tables[2].Columns.Contains("ItmType"))
                            {
                                if (GFunc.NEInt(dr["ItmType"], 0) == 250)
                                    ADAvailableQty.Value = 0;
                                else
                                    ADAvailableQty.SetValueTrigger(Stock.DecimalValue + GFunc.NEDec(ADPOQty.Text, 0) + GFunc.NEDec(ADShippedQty.Text, 0) - GFunc.NEDec(ADSOQty.Text, 0) - GFunc.NEDec(ADROQty.Text, 0), false);
                            }
                            else
                                ADAvailableQty.SetValueTrigger(Stock.DecimalValue + GFunc.NEDec(ADPOQty.Text, 0) + GFunc.NEDec(ADShippedQty.Text, 0) - GFunc.NEDec(ADSOQty.Text, 0) - GFunc.NEDec(ADROQty.Text, 0), false);
                        }
                    }*/
                dsSearchItm = null;
                if (GFunc.NEDec(ObCost.Text, 0) > 0)
                    ItmID.Appearance.BackColor = Color.LightGreen;
                else
                    ItmID.Appearance.BackColor = Color.White;
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
                    lblHeader.Text = "Item Purchase";
                    this.Text = "Report [Searh for Purchase by Item]";
                    lblConRange.Text = "Vendor Range";
                    break;
            }
        }

        private void Searchformat_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (Searchformat.Text == string.Empty)
                ConTo.Enabled = true;
            else
            {
                ConTo.Enabled = false;
                ConTo.SetValueTrigger("",false);
            }
        }      
      
        private void tagrdItms_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Bands[0].Summaries.Clear();
           
            e.Layout.Bands[0].Summaries.Add(Infragistics.Win.UltraWinGrid.SummaryType.Sum, e.Layout.Bands[0].Columns["ItmQty"]);
            e.Layout.Bands[0].Summaries.Add(Infragistics.Win.UltraWinGrid.SummaryType.Sum, e.Layout.Bands[0].Columns["ItmAmtF"]);
            e.Layout.Bands[0].Summaries.Add(Infragistics.Win.UltraWinGrid.SummaryType.Sum, e.Layout.Bands[0].Columns["ItmAmtH"]);
            e.Layout.Bands[0].Summaries.Add(Infragistics.Win.UltraWinGrid.SummaryType.Sum, e.Layout.Bands[0].Columns["ItmTotalCostH"]);

            e.Layout.Bands[0].Summaries.Add(Infragistics.Win.UltraWinGrid.SummaryType.Count, e.Layout.Bands[0].GetFirstVisibleCol(e.Layout.ColScrollRegions[0],false));

            e.Layout.Bands[0].Summaries[0].DisplayFormat = "{0:#,##0.00##}";
            e.Layout.Bands[0].Summaries[1].DisplayFormat = "{0:#,##0.00##}";
            e.Layout.Bands[0].Summaries[2].DisplayFormat = "{0:#,##0.00##}";
            e.Layout.Bands[0].Summaries[3].DisplayFormat = "{0:#,##0.00##}";
            e.Layout.Bands[0].Summaries[4].DisplayFormat = "Total =>";          
            e.Layout.Bands[0].Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;

            e.Layout.Bands[0].Override.SummaryFooterAppearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            e.Layout.Bands[0].Summaries[0].Appearance.BackColor = SystemColors.Window;            
            e.Layout.Bands[0].Summaries[1].Appearance.BackColor = SystemColors.Window;
            e.Layout.Bands[0].Summaries[2].Appearance.BackColor = SystemColors.Window;
            e.Layout.Bands[0].Summaries[3].Appearance.BackColor = SystemColors.Window;
            e.Layout.Bands[0].Summaries[4].Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            e.Layout.Bands[0].Summaries[4].Appearance.BorderAlpha = Infragistics.Win.Alpha.Transparent;

            e.Layout.Bands[0].Summaries[0].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            e.Layout.Bands[0].Summaries[1].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            e.Layout.Bands[0].Summaries[2].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            e.Layout.Bands[0].Summaries[3].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            e.Layout.Bands[0].Summaries[4].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

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
        private void ItmID_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;

            string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItmID");

            if (DocHDRUtil.EditorButton_Popup(0, ItmID.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref key, ref id, ref des))
            {
                ItmKey = key;
                ItmID.SetValueTrigger(id,false);
                if (Validation(false))
                    RefreshData();
            }
        }
        private void ItmID_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (ItmID.Text == string.Empty)
            {
                ItmKey = 0;
                RefreshData();
                return;
            }

            int key = 0;
            string id = string.Empty;
            string des = string.Empty;

            string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItmID");

            key = GFunc.ItmRecord_GetKey(GEnum.RecAccessType.ItemID, listSettingID, ItmID.Text, 0, ref id, ref des, true); //Record_GetKey((int)objInterestFactory.ConstantCodeKey, GEnum.RecAccessTypeItem.ItemID, ItmID.Text, ref id, ref des, true);
            if (key == 0)
            {
                //since value input by user cannot be match let the user select from Popup form
                if (DocHDRUtil.EditorButton_Popup(0, ItmID.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref key, ref id, ref des))
                { 
                    ItmKey = key;
                    ItmID.SetValueTrigger(id,false); 

                    if (Validation(false))
                        RefreshData();
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
                ItmKey = key;
                ItmID.SetValueTrigger(id,false);
                if (Validation(false))
                    RefreshData();
            }
        }
        private void ROQty_DoubleClick(object sender, EventArgs e)
        {
            ShowDetailForm(DetailType.ItmRO);
        }

        private void SOQty_DoubleClick(object sender, EventArgs e)
        {
            ShowDetailForm(DetailType.ItmSO);
        }
        private void ShippedQty_DoubleClick(object sender, EventArgs e)
        {
            ShowDetailForm(DetailType.ItmPS);
        }

        private void POQty_DoubleClick(object sender, EventArgs e)
        {
            ShowDetailForm(DetailType.ItmPO);
        }
        private void ADROQty_DoubleClick(object sender, EventArgs e)
        {
            ShowDetailForm(DetailType.ItmRODate);
        }

        private void ADSOQty_DoubleClick(object sender, EventArgs e)
        {
            ShowDetailForm(DetailType.ItmSODate);
        }
        private void ADShippedQty_DoubleClick(object sender, EventArgs e)
        {
            ShowDetailForm(DetailType.ItmPSDate);
        }

        private void ADPOQty_DoubleClick(object sender, EventArgs e)
        {
            ShowDetailForm(DetailType.ItmPODate);
        }

        private void ShowDetailForm(DetailType detailType)
        {
            frmRepSearchDetail f;
            //If it is already loaded, take that one
            foreach (Form form in Application.OpenForms[0].OwnedForms)
            {
                if (form.Name == "frmRepSearchDetail")
                {
                    f = (frmRepSearchDetail)form;
                    form.TopLevel = true;
                    f.Reload(ItmKey, detailType, GFunc.NEDateTime(DateAvailable.DateValue.Value, DateTime.Today));
                    return;
                }
            }

            //If it's not loaded yet, create new
            f = new frmRepSearchDetail(ItmKey, detailType, GFunc.NEDateTime(DateAvailable.DateValue.Value, DateTime.Today));
            f.TopLevel = true;
            f.Show(frmMain.gfrmMain);
        }

        private void ControlPrice_DoubleClick(object sender, EventArgs e)
        {
            frmPopupEstoreInfo f;
            //If it is already loaded, take that one
            foreach (Form form in Application.OpenForms[0].OwnedForms)
            {
                if (form.Name == "frmPopupEstoreInfo")
                {
                    f = (frmPopupEstoreInfo)form;
                    form.TopLevel = true;
                    f.Reload(ItmKey,ItmID.Text.Trim());
                    return;
                }
            }

            //If it's not loaded yet, create new
            f = new frmPopupEstoreInfo(ItmKey, ItmID.Text.Trim());
            f.TopLevel = true;
            f.Show(frmMain.gfrmMain);
        }
        private void btnPrintCheckSheet_Click(object sender, EventArgs e)
        {
            //int RepKey = 0;
           
            //if (CodeKey == GEnum.SystemCode.Customer)
            //    RepKey = 1845;//CUSTOMER SALES BY ITEM
            //else
            //    RepKey = 1850;//VENDOR PURCHASE BY ITEM

            ////show Print Dialog 
            //frmPrintSelection print = new frmPrintSelection(dtSearchItm, 0, RepKey, 0);
            //print.ShowDialog();

            _ReportLoader = new ReportLoader();

            string RptName = "";
            if (RepKey == 1845) //Item Sales By Customer 
                switch (GFunc.NEInt(SearchType.Value, 0))
                {
                    case 10: //Invoice
                        RptName = "S_ITEM_Cust_Invoice.rpt";
                        break;
                    case 20://Delivery_Order
                        RptName = "S_ITEM_Cust_DeliveryOrder.rpt";
                        break;
                    case 30://Sales_Order
                        RptName = "S_ITEM_Cust_SalesOrder.rpt";
                        break;
                    case 40://Quotation
                        RptName = "S_ITEM_Cust_Quotation.rpt";
                        break;
                    case 50://Invoice_and_Pending_DO
                        RptName = "S_ITEM_Cust_IVDO.rpt";
                        break;
                    case 60://Invoice_and_Pending_DO
                        RptName = "S_ITEM_Cust_Consignment.rpt";
                        break;
                    default:
                        RptName = "S_ITEM_Cust_Sales.rpt";
                        break;
                }
            else //Item Purchase By Vendor 
                switch (GFunc.NEInt(SearchType.Value, 0))
                {
                    case 10: //Invoice
                        RptName = "S_ITEM_Vend_Invoice.rpt";
                        break;
                    case 20://Delivery
                        RptName = "S_ITEM_Vend_Delivery.rpt";
                        break;
                    case 30://Purchase_Order
                        RptName = "S_ITEM_Vend_PurchaseOrder.rpt";
                        break;
                    case 40://Consignment
                        RptName = "S_ITEM_Vend_BLNoInvPD.rpt";
                        break;
                    case 50://Consignment
                        RptName = "S_ITEM_Vend_Consignment.rpt";
                        break;
                    default:
                        RptName = "S_ITEM_Vend_Purchase.rpt";
                        break;

                }

            CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rptDoc.Load(Application.StartupPath + @"\Reports\" + RptName);

            DataTable dt = dtSearchItm.DefaultView.ToTable();
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
            _ReportLoader = null;
        }
        private List<ReportParameter> GetReportParameters()
        {
            try
            {
                string IDText="";
                if (CodeKey == GEnum.SystemCode.Customer)
                    IDText = "Customer";
                else
                    IDText = "Vendor";
                List<ReportParameter> l_Reval = new List<ReportParameter>();

                string opCmpValue = SysOptionUtility.GetStr("CompanyName");

                string pRepRange = "";
                pRepRange = "DATE BETWEEN \"" + DateFrom.Value + "\" AND \"" + DateTo.Value + "\"";
                if (ConFrom.Text != "" || ConTo.Text != "")
                {
                    if (ConTo.Text == "" && Searchformat.Text == "")
                        pRepRange += ", "+IDText+" ID Like \"" + ConFrom.Text + "%\"";
                    else if (ConTo.Text != "")
                        pRepRange += ", " + IDText + " ID Between \"" + ConFrom.Text + "\" AND \"" + ConTo.Text + "\"";
                    else if (Searchformat.Text != "")
                        pRepRange += ", " + IDText + " ID Like \"" + ConFrom.Text + Searchformat.Text + "\"";
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

        private void ConFrom_CustomUpdate(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int Key = GFunc.GetExistingRecKey(ConFrom.Text, CodeKey, true, true);
                if (GFunc.IsNEZ(Key))
                {
                    ConFrom_EditorButtonClick(sender, null);
                }
                else
                {
                    MSTCon objItm = MSTCon.Get(Key);
                    ConFrom.SetValueTrigger(objItm.ConID, false);
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

        private void ConFrom_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int Key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ((Control)sender).Name);

                if (DocHDRUtil.EditorButton_Popup(0, ConFrom.Text, listSettingID, (int)GEnum.PopupType.CusID, ref Key, ref id, ref des))
                {
                    ConFrom.SetValueTrigger(id, false);
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

        private void ConTo_CustomUpdate(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int Key = GFunc.GetExistingRecKey(ConTo.Text, CodeKey, true, true);
                if (GFunc.IsNEZ(Key))
                {
                    ConTo_EditorButtonClick(sender, null);
                }
                else
                {
                    MSTCon objItm = MSTCon.Get(Key);
                    ConTo.SetValueTrigger(objItm.ConID, false);
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

        private void ConTo_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int Key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ((Control)sender).Name);

                if (DocHDRUtil.EditorButton_Popup(0, ConFrom.Text, listSettingID, (int)GEnum.PopupType.CusID, ref Key, ref id, ref des))
                {
                    ConTo.SetValueTrigger(id, false);
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

        private void ROQty_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick += new EventHandler(ROQty_DoubleClick);
        }

        private void ROQty_ControlRemoved(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick -= new EventHandler(ROQty_DoubleClick);
        }

        private void SOQty_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick += new EventHandler(SOQty_DoubleClick);
        }

        private void SOQty_ControlRemoved(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick -= new EventHandler(SOQty_DoubleClick);
        }

        private void POQty_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick+=new EventHandler(POQty_DoubleClick);
        }

        private void POQty_ControlRemoved(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick -= new EventHandler(POQty_DoubleClick);
        }

        private void ShippedQty_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick += new EventHandler(ShippedQty_DoubleClick);
        }

        private void ShippedQty_ControlRemoved(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick -= new EventHandler(ShippedQty_DoubleClick);
        }

        private void ControlPrice_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick += new EventHandler(ControlPrice_DoubleClick);
        }

        private void ControlPrice_ControlRemoved(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick -= new EventHandler(ControlPrice_DoubleClick);
        }
       

        private void ADPOQty_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick += new EventHandler(ADPOQty_DoubleClick);
        }
        private void ADPOQty_ControlRemoved(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick -= new EventHandler(ADPOQty_DoubleClick);
        }

        private void ADShippedQty_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick += new EventHandler(ADShippedQty_DoubleClick);
        }

        private void ADShippedQty_ControlRemoved(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick -= new EventHandler(ADShippedQty_DoubleClick);
        }

        private void ADROQty_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick += new EventHandler(ADROQty_DoubleClick);
        }

        private void ADROQty_ControlRemoved(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick -= new EventHandler(ADROQty_DoubleClick);
        }

        private void ADSOQty_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick += new EventHandler(ADSOQty_DoubleClick);
        }

        private void ADSOQty_ControlRemoved(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick -= new EventHandler(ADSOQty_DoubleClick);
        }

        private void frmRepSearchItm_Shown(object sender, EventArgs e)
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
                    ((DataTable)tagrdItms.DataSource).DefaultView.RowFilter = "ItmDes like '" + SearchItmDes.Text.Replace("'", "") + "%'";
                else
                    //tagrdSearchResult.DisplayLayout.Bands[0].ColumnFilters[currSearchCol].FilterConditions.Add(FilterComparisionOperator.Contains, SearchText.Text.Trim());
                    ((DataTable)tagrdItms.DataSource).DefaultView.RowFilter = "ItmDes like '%" + SearchItmDes.Text.Trim().Replace("'","") + "%'";
            }

            if (CodeKey == GEnum.SystemCode.Vendor && SECPermUtility.Perform("ItemViewCost", false) == false) // if there is no permission for ItemViewCost
            {
                HideAmount("ItmPrice");
                HideAmount("ItmAmtH");
                HideAmount("ItmAmtF");
                HideAmount("ItmAddAmtH");
                HideAmount("ItmAddDisAmtH");
                HideAmount("ItmTotalCostH");

                #region /*commented by YST on 2023-03-10 */
                /*
                if (tagrdItms.DisplayLayout.Bands[0].Columns["ItmPrice"].EditorComponent != null)
                {
                    if (tagrdItms.DisplayLayout.Bands[0].Columns["ItmPrice"].EditorComponent.GetType() == typeof(TAUtil.TANumericEditor))
                    {
                        ((TAUtil.TANumericEditor)tagrdItms.DisplayLayout.Bands[0].Columns["ItmPrice"].EditorComponent).PasswordChar = '*';
                        tagrdItms.DisplayLayout.Bands[0].Columns["ItmPrice"].CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FullEditorDisplay;
                        tagrdItms.DisplayLayout.Bands[0].Columns["ItmPrice"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly;
                    }
                }
                if (tagrdItms.DisplayLayout.Bands[0].Columns["ItmAmtH"].EditorComponent != null)
                {
                    if (tagrdItms.DisplayLayout.Bands[0].Columns["ItmAmtH"].EditorComponent.GetType() == typeof(TAUtil.TANumericEditor))
                    {
                        ((TAUtil.TANumericEditor)tagrdItms.DisplayLayout.Bands[0].Columns["ItmAmtH"].EditorComponent).PasswordChar = '*';
                        tagrdItms.DisplayLayout.Bands[0].Columns["ItmAmtH"].CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FullEditorDisplay;
                        tagrdItms.DisplayLayout.Bands[0].Columns["ItmAmtH"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly;
                    }
                }
                if (tagrdItms.DisplayLayout.Bands[0].Columns["ItmAmtF"].EditorComponent != null)
                {
                    if (tagrdItms.DisplayLayout.Bands[0].Columns["ItmAmtF"].EditorComponent.GetType() == typeof(TAUtil.TANumericEditor))
                    {
                        ((TAUtil.TANumericEditor)tagrdItms.DisplayLayout.Bands[0].Columns["ItmAmtF"].EditorComponent).PasswordChar = '*';
                        tagrdItms.DisplayLayout.Bands[0].Columns["ItmAmtF"].CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FullEditorDisplay;
                        tagrdItms.DisplayLayout.Bands[0].Columns["ItmAmtF"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly;
                    }
                }
                */
                #endregion
            }
        }

        /* added by YST on 2023-03-10 */
        private void HideAmount(string colName)
        {
            if (tagrdItms.DisplayLayout.Bands[0].Columns[colName].EditorComponent != null)
            {
                if (tagrdItms.DisplayLayout.Bands[0].Columns[colName].EditorComponent.GetType() == typeof(TAUtil.TANumericEditor))
                {
                    ((TAUtil.TANumericEditor)tagrdItms.DisplayLayout.Bands[0].Columns[colName].EditorComponent).PasswordChar = '*';
                    tagrdItms.DisplayLayout.Bands[0].Columns[colName].CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FullEditorDisplay;
                    tagrdItms.DisplayLayout.Bands[0].Columns[colName].CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly;
                }
            }
        }

        private void SearchItmDes_CustomUpdate(object sender, CancelEventArgs e)
        {
            FilterGrid();
        }

        private void Costlbl_DoubleClick(object sender, EventArgs e)
        {
            if (AvgCost.Visible == true)
            {
                Cost.Visible = false;
                AvgCost.Visible = false;
                ObCost.Visible = false;
            }
            else
            {
                Cost.Visible = true;
                AvgCost.Visible = true;
                ObCost.Visible = true;
            }
        }

        private void AvgCostLbl_DoubleClick(object sender, EventArgs e)
        {
            if (AvgCost.Visible == true)
            {
                Cost.Visible = false;
                AvgCost.Visible = false;
                ObCost.Visible = false;
            }
            else
            {
                Cost.Visible = true;
                AvgCost.Visible = true;
                ObCost.Visible = true;
            }
        }

        private void frmRepSearchItm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (dtSearchItm != null) dtSearchItm.Dispose();
            if (dsSearchItm != null) dsSearchItm.Dispose();
            this.Dispose();
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

        private void DateAvailable_CustomUpdate(object sender, CancelEventArgs e)
        {
            //RefreshAvailableInfo();
        }

        private void DateAvailable_ValueChanged(object sender, EventArgs e)
        {
            if (DateAvailable.Text.Length >= 6)
            {
                DateTime dt;
                if (DateTime.TryParse(DateAvailable.Text, out dt))
                {
                    //RefreshAvailableInfo();
                }
            }
        }

        //private void RefreshAvailableInfo()
        //{            
        //    try
        //    {
        //        this.Cursor = Cursors.WaitCursor;
        //        if (ItmKey != 0)
        //        {
        //            List<SqlParameter> parmList = new List<SqlParameter>();
        //            parmList.Add(new SqlParameter("@ItmKey", ItmKey));
        //            parmList.Add(new SqlParameter("@Date", DateAvailable.DateValue));

        //            dtSearchItm = GFunc.ExecuteProc("Rep_SearchItmInfo", parmList);
        //            if (dtSearchItm.Rows.Count > 0)
        //            {
        //                DataRow dr = dtSearchItm.Rows[0];

        //                ADSOQty.Text = GFunc.NEDec(dr["SOQty"], 0M).ToString("#,##0.00");
        //                ADPOQty.Text = GFunc.NEDec(dr["POQty"], 0M).ToString("#,##0.00");
        //                ADShippedQty.SetValueTrigger(dr["APPNQty"], false);
        //                ADROQty.SetValueTrigger(dr["ROQty"], false);

        //                if (dtSearchItm.Columns.Contains("ItmType"))
        //                {
        //                    if (GFunc.NEInt(dr["ItmType"], 0) == 250)
        //                        ADAvailableQty.Value = 0;
        //                    else
        //                        ADAvailableQty.SetValueTrigger(Stock.DecimalValue + GFunc.NEDec(ADPOQty.Text, 0) + GFunc.NEDec(ADShippedQty.Text, 0) - GFunc.NEDec(ADSOQty.Text, 0) - GFunc.NEDec(ADROQty.Text, 0), false);
        //                }
        //                else
        //                    ADAvailableQty.SetValueTrigger(Stock.DecimalValue + GFunc.NEDec(ADPOQty.Text, 0) + GFunc.NEDec(ADShippedQty.Text, 0) - GFunc.NEDec(ADSOQty.Text, 0) - GFunc.NEDec(ADROQty.Text, 0), false);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        this.Cursor = Cursors.Default;
        //    }
        //}

        private void btnShowHideEst_Click(object sender, EventArgs e)
        {
            if(pnlEst1.Visible)
            {
                pnlEst1.Visible = false;
                pnlEst2.Visible = false;
                btnShowHideEst.Text = ">>";
            }
            else
            {
                //pnlEst1.Visible = true;
                //pnlEst2.Visible = true;
                btnShowHideEst.Text = "<<";
                //RefreshAvailableInfo();
            }
        }
    }
}
