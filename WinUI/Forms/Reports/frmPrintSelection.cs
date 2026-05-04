using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BOLib;
using System.Data.SqlClient;
using DataDynamics.ActiveReports;
using System.IO;
using Infragistics.Win.UltraWinEditors;
using TAUtil;
using System.Xml;
using CrystalDecisions.CrystalReports.Engine;
namespace WinUI
{
    public partial class frmPrintSelection : Form
    {
        #region Variables

        private int _ConKey = -1;
        private string msgID = string.Empty;
        private bool formClose = false;
        private int _RepKey = 0;
        private string _DocKey = "0";
        
        private ReportLoader _ReportLoader = null;
        private Dictionary<string, bool> _CheckControlValue = null;
        private Document DOC = null;
        public GVar.DocPrintUpdateEvent DocPrinted = null;

        private string _DocEmail = string.Empty;
        private string DocBAddrEmail = string.Empty;
        private string ContextMenuSetting = string.Empty;
        private int DocCodeKey = 0;
        private int AdditionalKey = 0;

        private DataTable dtStockCount = null;
        #endregion

        #region Properties
        
        public Dictionary<string, bool> CheckValue
        {
            set
            {
                _CheckControlValue = value;

                IEnumerator ie = _CheckControlValue.Keys.GetEnumerator();
                while (ie.MoveNext())
                {
                    Control[] l_tmp = this.Controls.Find(ie.Current.ToString(), true);
                    if (l_tmp.Length > 0)
                    {
                        ((TAUtil.TACheckBoxEditor)l_tmp[0]).Checked = _CheckControlValue[ie.Current.ToString()];
                    }
                }
            }
        }
        
        public ReportLoader ReportLoader
        {
            get { return _ReportLoader; }
            set { _ReportLoader = value; }
        }
        
        public int ConKey
        {
            get { return _ConKey; }
            set { _ConKey = value; }
        }
        public int RepKey
        {
            get { return _RepKey; }
            set { _RepKey = value; }
        }
        public string DocKey
        {
            get { return _DocKey; }
            set { _DocKey = value; }
        }
        public string DocEmail
        {
            get { return _DocEmail; }
            set { _DocEmail = value; }
        }

        #endregion

        //Constructor 
        public frmPrintSelection()
        {
            InitializeComponent();
        }      

        public frmPrintSelection(ref Document Doc, int vDocCodeKey)
        {
            InitializeComponent();
            DOC = Doc;
            _DocKey = DOC.DocKey.ToString();
            this.DocCodeKey = vDocCodeKey;          

            switch ((int)DocCodeKey)
            {
                case (int)GEnum.SystemCode.Packing_List:
                case (int)GEnum.SystemCode.Sales_Adjustment:
                case (int)GEnum.SystemCode.Contra:
                case (int)GEnum.SystemCode.Payment_Received:
                case (int)GEnum.SystemCode.Cash_Adjustment:
                case (int)GEnum.SystemCode.Cash_Payment_Received:
                case (int)GEnum.SystemCode.Cash_Contra:
                case (int)GEnum.SystemCode.Purchase_Plan:
                // case (int)GEnum.SystemCode.Purchase_Request: //Remove this line. Purchase_Request does not have ConKey
                case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                case (int)GEnum.SystemCode.Purchase_Adjustment:
                case (int)GEnum.SystemCode.Payment_Issue:
                case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                case (int)GEnum.SystemCode.Consignment_Settlement:
                case (int)GEnum.SystemCode.Quotation:
                case (int)GEnum.SystemCode.Sales_Order:
                case (int)GEnum.SystemCode.Reserve_Order:
                case (int)GEnum.SystemCode.Cash_Debit_Note:
                case (int)GEnum.SystemCode.Purchase_Credit_Note:
                case (int)GEnum.SystemCode.Purchase_Debit_Note:
                case (int)GEnum.SystemCode.Purchase_Invoice:
                case (int)GEnum.SystemCode.Purchase_Delivery:
                case (int)GEnum.SystemCode.Cash_Credit_Note:
                case (int)GEnum.SystemCode.Purchase_Order:
                case (int)GEnum.SystemCode.Consolidate_Invoice:

                case (int)GEnum.SystemCode.Sales_Credit_Note:
                case (int)GEnum.SystemCode.Sales_Debit_Note:
                case (int)GEnum.SystemCode.Sales_Invoice:
                case (int)GEnum.SystemCode.Delivery_Order:
                case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                case (int)GEnum.SystemCode.Received_Consignment:
                case (int)GEnum.SystemCode.Order_Consignment:
                case (int)GEnum.SystemCode.Return_Consignment:
                case (int)GEnum.SystemCode.Issue_Consignment:
                    _ConKey = GFunc.NEInt(GFunc.GetIntPropertyValue("DocConKey", Doc), 0);
                    break;
                case (int)GEnum.SystemCode.Cash_Sale:
                    _ConKey = GFunc.NEInt(GFunc.GetIntPropertyValue("DocConKey", Doc), 0);
                    CollectPayment.Checked = true;
                    break;
                default:
                    break;
            }

            switch ((int)DocCodeKey)
            {
                case (int)GEnum.SystemCode.Quotation:
                case (int)GEnum.SystemCode.Sales_Order:
                case (int)GEnum.SystemCode.Reserve_Order:
                case (int)GEnum.SystemCode.Delivery_Order:
                case (int)GEnum.SystemCode.Packing_List:
                case (int)GEnum.SystemCode.Sales_Invoice:
                case (int)GEnum.SystemCode.Sales_Debit_Note:
                case (int)GEnum.SystemCode.Sales_Credit_Note:
                case (int)GEnum.SystemCode.Payment_Received:
                case (int)GEnum.SystemCode.Cash_Sale:
                case (int)GEnum.SystemCode.Cash_Debit_Note:
                case (int)GEnum.SystemCode.Cash_Credit_Note:
                case (int)GEnum.SystemCode.Cash_Payment_Received:
                case (int)GEnum.SystemCode.Purchase_Plan:
                case (int)GEnum.SystemCode.Purchase_Order:
                case (int)GEnum.SystemCode.Purchase_Invoice:
                case (int)GEnum.SystemCode.Purchase_Debit_Note:
                case (int)GEnum.SystemCode.Purchase_Credit_Note:
                case (int)GEnum.SystemCode.Payment_Issue:
                case (int)GEnum.SystemCode.Issue_Consignment:
                case (int)GEnum.SystemCode.Return_Consignment:
                case (int)GEnum.SystemCode.Order_Consignment:
                    DocBAddrEmail = GFunc.NEStr(GFunc.GetStringPropertyValue("DocBAddrEmail", Doc), "");
                    break;
                default:
                    break;
            }
            RepKey = (int)DocCodeKey;
            LayoutVisible_Set();
        }
        public frmPrintSelection(DataTable pdtStockCount, int DocCodeKey, int RepKey, int AdditionalKey)
        {
            InitializeComponent();

            switch (DocCodeKey)
            {
                case (int)GEnum.SystemCode.Stock_Count:

                    RptFile2.Enabled = false;
                    Layout.Enabled = false;
                    SettinggroupBox.Visible = false;
                    break;
            }
            dtStockCount = pdtStockCount;
            this.DocCodeKey = DocCodeKey;
            this.RepKey = RepKey;
            this.AdditionalKey = AdditionalKey;
        }

        //Form Events
        private void frmPrintSelection_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (RepKey == (int)GEnum.SystemCode.Cash_Sale) PaymentMethod.Visible = true; else PaymentMethod.Visible = false;
            try
            {
                if (this.DocCodeKey == (int)GEnum.SystemCode.Stock_Count || this.DocCodeKey == 0)
                {
                    //Non Document PrintOut
                    _ReportLoader = new ReportLoader();
                    
                    GlobalUI.FormGrids_Set(this, RepKey, out ContextMenuSetting);
                    ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(DocCodeKey);
                    GlobalUI.Combos_Fill(this, this.DocCodeKey);
                    GlobalUI.BindComboValue(RptFile1, "SYSRepRptNonDoc %" + RepKey + "%" + AdditionalKey);
                    GlobalUI.BindComboValue(RptFile1, "SYSRepRpt%" + RepKey);

                }
                else
                {
                    //For Document PrintOut
                    _ReportLoader = new ReportLoader();
                    
                    GlobalUI.FormGrids_Set(this, RepKey, out ContextMenuSetting);
                    ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)DOC.DocCodeKey);
                    GlobalUI.Combos_Fill(this, (int)DOC.DocCodeKey);

                    //Set Default Values and Dependent combo fill
                    ConEmailAddress_Get();
                    Email.SetValueTrigger(DocBAddrEmail, false);//Set default email address

                    //Set Default RptFileName from SYS_Rep
                    DataTable dt = GFunc.ExecuteQuery("Exec SYSRepRptDefault_Set " + RepKey);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        RptFile1.SetValueTrigger(dt.Rows[0]["UID"], true);

                    }
                    if (dt != null && dt.Rows.Count > 1)
                    {
                        RptFile2.SetValueTrigger(dt.Rows[1]["UID"], false);
                    }
                   
                    RptFile1.DropDownWidth = 550;
                    RptFile2.DropDownWidth = 550;

                    if (DOC != null)
                    {
                        if (DOC.DocCodeKey == (int)GEnum.SystemCode.Quotation)
                        {
                            Subject.Text = "Quotation " + DOC.DocID;
                            Message.Text =
                              "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>Dear "
                              + GFunc.GetStringPropertyValueOrEmpty("DocBAddrAttn", DOC) + ",</p> " +
                              "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                              " Thank you for your enquiry.</p>" +
                                 "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                              @"We are pleased to offer you as per attached for your kind consideration.</p>" +
                                 "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                              "Should you require any further clarification, please do not hesitate to contact us.</p>" +
                                "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                              " Thank you.</p>";

                            tsbPDF.Visible = true;
                        }
                        else
                            tsbPDF.Visible = false;
                    }
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
                this.formClose = true;
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        private void frmPrintSelection_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }
            if (this.formClose)
            {
                return;
            }          
        }
        private void frmPrintSelection_KeyDown(object sender, KeyEventArgs e)
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

        //Menu Strip Event
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        private void tsbPreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (PrintCriteria(GEnum.PrintType.Preview))
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }
        private void tsbEmail_Click(object sender, EventArgs e)
        {
            try
            {
                if (PrintCriteria(GEnum.PrintType.Email))
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }

        private void tsbPDF_Click(object sender, EventArgs e)
        {
            // BackgroundWorker w = new BackgroundWorker();
            List<SqlParameter> parmList = new List<SqlParameter>();

            switch (RepKey)
            {               
                case (int)GEnum.SystemCode.Quotation:               
                    if (DOC != null)
                    {
                        parmList.Add(new SqlParameter("@DocCodeKey", DOC.DocCodeKey));
                        parmList.Add(new SqlParameter("@DocKey", DOC.DocKey));
                    }
                    else
                    {
                        parmList.Add(new SqlParameter("@DocCodeKey", this.RepKey));
                        parmList.Add(new SqlParameter("@DocKey", this.DocKey));
                    }
                    break;
                default:
                    break;
            }
            SYSRepRpt objRptTmp1 = SYSRepRpt.Get(GFunc.NEInt(RptFile1.Value, 0), 2);
            _ReportLoader.RepKey = RepKey;
            _ReportLoader.ReportSqlParameter = parmList;
            _ReportLoader.ReportName =objRptTmp1.RptNm;
            _ReportLoader.RptPrintCondition = objRptTmp1.RptPrintCondition;
            _ReportLoader._reportFileName = objRptTmp1.RptNm;

            string filepath = "";
            SaveFileDialog sfd=new SaveFileDialog();
            sfd.FileName = GFunc.GetStringPropertyValueOrEmpty("DocConNm", DOC) + "-" + DOC.DocID.Replace("/", "")+".pdf";

            if (sfd.ShowDialog() == DialogResult.OK)
                filepath = sfd.FileName;
            else
                return;

            if (objRptTmp1.RptNm.ToUpper().EndsWith(".RPT"))
            {
                SYSRep rep = SYSRep.Get(RepKey);
                SYSRepRpt rpt = SYSRepRpt.Get(RepKey, objRptTmp1.RptNm, 3);
                DataSet ds = GFunc.ExecuteProcDataSet(GFunc.IsNE(rpt.RptAltRecordSource) ? rep.RPTRecordSource1 : rpt.RptAltRecordSource, parmList);
                DataTable dt = ds.Tables[0];

                if (_ReportLoader.CheckPrintCondition(null, RepKey, ref dt) == false)
                    return;

                //Add QRCode column and data into the datasource
                if (objRptTmp1.RptNm.ToLower().Contains("qrcode"))
                    dt = _ReportLoader.GetSourceWithQRCode(dt, Path.GetExtension(objRptTmp1.RptNm));

                CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rptDoc.Load(Application.StartupPath + @"\Reports\" + objRptTmp1.RptNm);
                rptDoc.SetDataSource(dt);

                _ReportLoader.ReportDataSource = dt;
                //Needed for multi print
                //Title.Text = rpt.RptTitle;

                _ReportLoader.ReportParameter = GetReportParameters();
                foreach (ReportParameter p in _ReportLoader.ReportParameter)
                {
                    rptDoc.SetParameterValue(p.ParameterName, p.ParameterValue);
                }
                _ReportLoader.rptDoc = rptDoc;
                _ReportLoader.CreatePdfFile(Path.GetDirectoryName(filepath)+@"\",Path.GetFileNameWithoutExtension(filepath));
            }
            else // .RPX
            {
                _ReportLoader.ReportParameter = GetReportParameters();
                if (dtStockCount != null)
                {
                    _ReportLoader.LoadReport(dtStockCount, objRptTmp1.RptNm, false);
                }
                else
                {
                    _ReportLoader.LoadReportWCondition((int)RepKey, false);
                }
                _ReportLoader.CreatePdfFile(Path.GetDirectoryName(filepath) + @"\", Path.GetFileNameWithoutExtension(filepath));
            }
           
        }
        private void tsbPrint_Click(object sender, EventArgs e)
        {
            try
            {
                bool isPrinted = false;

                isPrinted = PrintCriteria(GEnum.PrintType.Print);

                //Update DocPrinted Status
                if (isPrinted)
                {
                    this.DialogResult = DialogResult.Cancel; //Not to show ReportViewer form;
                    this.Hide();
                    DocumentPrinted();                   
                }
                //else
                //{
                //    PrintFailed("PrintCriteria(" + GEnum.PrintType.Print + ") >> return - false "); // temp code to investigate the error
                //}
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

        //Control Events
        private void RptFile1_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (!GFunc.IsNEZ((RptFile1.Value)))
                {
                    SYSRepRpt objRpt = SYSRepRpt.Get(Convert.ToInt32(RptFile1.Value), 2);//option 2 for UID

                    //Show Rpt Information
                    Title.SetValueTrigger(objRpt.RptTitle, false);
                    Layout.SetValueTrigger(objRpt.RptLayOut, false);
                    Copies.SetValueTrigger(objRpt.PrtCopies.ToString(), false);

                    LayoutSetting();

                    LetterHead.Checked = objRpt.ShwLetterHead;
                    ItemCount.Checked = objRpt.ShwItmCount;

                    RptFile2.Value = "";

                    DataRow[] drs = ((DataTable)RptFile2.DataSource).Select("RptNm='" +GFunc.NEStr(objRpt.Custom3,"") + "'");
                    if (drs.Length > 0)
                    {
                        RptFile2.Value = drs[0]["UID"];
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Error(ex, false);
            }
        }
        private void Layout_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                LayoutSetting();
            }
            catch (Exception ex)
            {
                Error(ex, false);
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
                Error(tex, false);
            }
            catch (Exception ex)
            {
                Error(ex, true);
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

        //Methods       
        private bool PrintCriteria(GEnum.PrintType pType)
        {
            short NoCopies = GFunc.NEShort(Copies.Text, 1);
            int RPXUID1 = GFunc.NEInt(RptFile1.Value, 0);
            int RPXUID2 = GFunc.NEInt(RptFile2.Value, 0);

            if (RPXUID1 > 0)
            {
                SYSRepRpt objRptTmp1 = SYSRepRpt.Get(RPXUID1, 2);
                if (Print(NoCopies, objRptTmp1.RptNm, pType,objRptTmp1.RptPrintCondition,objRptTmp1.Custom1) == false)
                    return false;
            }
            if (RPXUID2 > 0)
            {
                SYSRepRpt objRptTmp2 = SYSRepRpt.Get(RPXUID2, 2);
                if (Print(NoCopies, objRptTmp2.RptNm, pType, objRptTmp2.RptPrintCondition, objRptTmp2.Custom1) == false)
                    return false;
            }
            return true;
        }
        public bool Print(short NoCopies, string RptName, GEnum.PrintType pType,string PrintCondition,string custom1)
        {
            try
            {                
                // BackgroundWorker w = new BackgroundWorker();
                List<SqlParameter> parmList = new List<SqlParameter>();

                switch (RepKey)
                {
                    case (int)GEnum.SystemCode.Packing_List:
                    case (int)GEnum.SystemCode.Sales_Adjustment:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                    case (int)GEnum.SystemCode.Bank_Revaluation:
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Works_Order:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Consolidate_Invoice:
                        if (DOC != null)
                        {
                            parmList.Add(new SqlParameter("@DocCodeKey", DOC.DocCodeKey));
                            parmList.Add(new SqlParameter("@DocKey", DOC.DocKey));
                        }
                        else
                        {
                            parmList.Add(new SqlParameter("@DocCodeKey", this.RepKey));
                            parmList.Add(new SqlParameter("@DocKey", this.DocKey));
                        }
                        break;
                    default:
                        break;
                }
                _ReportLoader.RepKey = RepKey;
                _ReportLoader.ReportSqlParameter = parmList;
                _ReportLoader.ReportName = RptName;
                _ReportLoader.RptPrintCondition = PrintCondition;

                if (RptName.ToUpper().EndsWith(".RPT"))
                {
                    SYSRep rep = SYSRep.Get(RepKey);
                    SYSRepRpt rpt = SYSRepRpt.Get(RepKey, RptName, 3);
                    DataSet ds = GFunc.ExecuteProcDataSet(GFunc.IsNE(rpt.RptAltRecordSource) ? rep.RPTRecordSource1 : rpt.RptAltRecordSource, parmList);
                    DataTable dt = ds.Tables[0];

                    if (_ReportLoader.CheckPrintCondition(null, RepKey, ref dt) == false)
                        return false;

                    //Add QRCode column and data into the datasource
                    if (RptName.ToLower().Contains("qrcode"))
                        dt = _ReportLoader.GetSourceWithQRCode(dt, Path.GetExtension(RptName));                   

                    CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();                    
                    rptDoc.Load(Application.StartupPath + @"\Reports\" + RptName);
                    rptDoc.SetDataSource(dt);

                    _ReportLoader.ReportDataSource = dt;
                    //Needed for multi print
                    //Title.Text = rpt.RptTitle;

                    _ReportLoader.ReportParameter = GetReportParameters();
                    foreach (ReportParameter p in _ReportLoader.ReportParameter)
                    {
                        if (rptDoc.ParameterFields[p.ParameterName] != null) /* modified by YST on 2021/09/07 */
                            rptDoc.SetParameterValue(p.ParameterName, p.ParameterValue);
                    }

                    if (RptName == "zAR_DO30.rpt" || RptName == "zAR_IV60.rpt" || RptName == "zAR_IV50.rpt" || RptName == "zAR_IV60(MBU).rpt" || RptName == "zAR_IV60(Finance).rpt")
                        rptDoc.SetParameterValue("pHideCollectPayment", CollectPayment.Checked);

                    if (RptName == "zAR_IVC50.rpt")
                        rptDoc.SetParameterValue("pHidePaymentMethod", PaymentMethod.Checked);

                    var missingParams = rptDoc.ParameterFields
                   .Cast<CrystalDecisions.Shared.ParameterField>()
                   .Where(p => (p.CurrentValues == null || p.CurrentValues.Count == 0))
                   .ToList();

                    foreach (CrystalDecisions.Shared.ParameterField pf in missingParams)
                    {
                        if (!pf.HasCurrentValue)
                            if (pf.DefaultValues.Count > 0)
                                rptDoc.SetParameterValue(pf.Name, pf.DefaultValues[0]);
                    }

                    if (pType == GEnum.PrintType.Preview)
                    {
                        try
                        {
                            frmReportViewer fRptViewer = new frmReportViewer();
                            fRptViewer.RepKey = RepKey;
                            fRptViewer.RptName = RptName;
                            fRptViewer.RptTitle = Title.Text;
                            fRptViewer.RptDocument = rptDoc;
                            fRptViewer.reportLoader = _ReportLoader;
                            fRptViewer.MdiParent = frmMain.gfrmMain;
                            fRptViewer.Show();
                            //Mic check , Assign the event to raise if the user clicked Print button later
                            if (DOC != null)
                            {
                                fRptViewer.DocCodeKey = (GEnum.SystemCode)DOC.DocCodeKey;
                                fRptViewer.DocKey = DOC.DocKey;
                                if (DOC.DocPrinted == false) //Only assign if it has not been printed yet
                                    fRptViewer.DocPrinted += new GVar.DocPrintUpdateEvent(DocumentPrinted);
                            }
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        
                    }
                    else if (pType == GEnum.PrintType.Print)
                    {
                        if (GFunc.IsNE(rptDoc.PrintOptions.PrinterName))
                        {
                            MsgBox.Show("The report has not set up printer yet. Please preview the report and set up the printer first.");
                            return false;
                        }
                        _ReportLoader._reportFileName = RptName;
                        _ReportLoader.rptDoc = rptDoc;

                        if (_ReportLoader.ReportParameter.Count < rptDoc.ParameterFields.Count) //for prompt parameter like Adhoc Remark
                        {
                            for (int i = _ReportLoader.ReportParameter.Count; i < rptDoc.ParameterFields.Count; i++)
                            {
                                if (!rptDoc.ParameterFields[i].HasCurrentValue)
                                {
                                    frmInputDialog f = new frmInputDialog(rptDoc.ParameterFields[i].PromptText, "Report", "", "OK", "Cancel");
                                    if (f.ShowDialog() == DialogResult.OK)
                                    {
                                        rptDoc.SetParameterValue(rptDoc.ParameterFields[i].Name, f.txtInput.Text);
                                    }
                                    else
                                        return false;
                                    f.Close();
                                }
                            }                           
                        }
                        if (PrintCondition == "Fax")
                            return _ReportLoader.SendFax(GFunc.NEStr(dt.Rows[0]["DocBAddrFax"], ""), GFunc.NEStr(dt.Rows[0]["DocID"], ""));
                        else
                        {                            
                            if (GFunc.NEStr(rpt.Custom2, string.Empty) == string.Empty)// normal printing
                                return _ReportLoader.Print(NoCopies, false, true);
                            else // multi printing for BengHui 
                                return _ReportLoader.PrintMulti(rpt.Custom2, rptDoc, NoCopies, Title.Text);
                        }
                                
                    }
                    else
                    {
                        _ReportLoader._reportFileName = RptName;
                        _ReportLoader.rptDoc = rptDoc;
                        string msg =  Message.Text.Replace("\n", "</p><p>");

                        if (DocEmail == string.Empty)
                        {
                            if (Email.Text == string.Empty)
                            {
                                MsgBox.Show("Email cannot be empty.");
                                Email.Focus();
                                return false;
                            }
                           
                            _ReportLoader.EmailDisplayinOutlook(Email.Text, Subject.Text, msg, GFunc.GetStringPropertyValueOrEmpty("DocConNm",DOC)+"-"+DOC.DocID.Replace("/",""));
                        }
                        else
                            _ReportLoader.EmailDisplayinOutlook(DocEmail, Subject.Text, msg, GFunc.GetStringPropertyValueOrEmpty("DocConNm", DOC) + "-" + DOC.DocID.Replace("/", ""));
                    }
                }
                else // .RPX
                {
                    _ReportLoader.ReportParameter = GetReportParameters();
                    if (dtStockCount != null)
                    {
                        _ReportLoader.LoadReport(dtStockCount, RptName,false);
                    }
                    else
                    {
                        _ReportLoader.LoadReportWCondition((int)RepKey,false);
                    }
                    if (pType == GEnum.PrintType.Preview)
                    {
                        if (_ReportLoader.PrintPreview())
                        {
                            //Mic check , Assign the event to raise if the user clicked Print button later
                            if (DOC != null)
                            {
                                _ReportLoader.RpxViewerForm.DocCodeKey = (GEnum.SystemCode)DOC.DocCodeKey;
                                _ReportLoader.RpxViewerForm.DocKey = DOC.DocKey;
                                if(DOC.DocPrinted==false) //Only assign if it has not been printed yet
                                    _ReportLoader.RpxViewerForm.DocPrinted += new GVar.DocPrintUpdateEvent(DocumentPrinted);
                            }
                            return true;
                        }
                    }
                    else if (pType == GEnum.PrintType.Print)
                    {
                        if (GFunc.IsNE(_ReportLoader.rpxDoc.Document.Printer.PrinterName))
                        {
                            MsgBox.Show("The report has not set up printer yet. Please preview the report and set up the printer first.");
                            return false;
                        }
                        _ReportLoader._reportFileName = RptName;   
                   
                        return _ReportLoader.Print(NoCopies, false, true);
                    }
                    else
                    {
                        _ReportLoader._reportFileName = RptName;
                        if (DocEmail == string.Empty)
                            _ReportLoader.EmailDisplayinOutlook(Email.Text, Subject.Text, Message.Text, ReportFileType.AcrobatPDFFile);
                        else
                            _ReportLoader.EmailDisplayinOutlook(DocEmail, Subject.Text, Message.Text, ReportFileType.AcrobatPDFFile);
                    }
                }
                return true;
               
            }
            catch (TAException tex)
            {                
                if(tex.PropertyName=="PrintCondition")
                    Error(tex, false);
                else
                    Error(tex, true);
                return false;
            }
            catch (Exception ex)
            {
                Error(ex, true);
                return false;
            }
        }

     
        //Mic check 
        private void DocumentPrinted()
        {
            //Just called the existing event to update DocPrinted flag that Albert has done
            if (Svr_PrintedStatus_Set(GFunc.NEInt(DocKey, 0)))
            {
                Obj_PrintedStatus_Set();
                if (DocPrinted != null)//To refresh the form and List of the Document
                    DocPrinted.Invoke();                
            }
            //else
            //{
            //    PrintFailed("Svr_PrintedStatus_Set(" + GFunc.NEInt(DocKey, 0) + ") >> return - false"); // temp code to investigate the error                
            //}
        }

        public bool PrintMulti(short NoCopies, string RptName, GEnum.PrintType pType,string printCondition, string docKeyFilterXML,string custom1)
        {
            try
            {
                //BackgroundWorker w = new BackgroundWorker();
                List<SqlParameter> parmList = new List<SqlParameter>();

                SYSRepRpt objRpt = SYSRepRpt.Get(RepKey,RptName,3);
                Title.SetValueTrigger(objRpt.RptTitle, false);

                switch (RepKey)
                {
                    case (int)GEnum.SystemCode.Packing_List:
                    case (int)GEnum.SystemCode.Sales_Adjustment:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                    case (int)GEnum.SystemCode.Bank_Revaluation:
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Works_Order:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                        if (DOC != null)
                        {
                            parmList.Add(new SqlParameter("@DocCodeKey", DOC.DocCodeKey));
                            parmList.Add(new SqlParameter("@DocKey", Convert.ToInt32(0)));
                        }
                        else
                        {
                            parmList.Add(new SqlParameter("@DocCodeKey", this.RepKey));
                            parmList.Add(new SqlParameter("@DocKey", Convert.ToInt32(0)));
                        }
                        parmList.Add(new SqlParameter("@DocKeysFilteredXML", docKeyFilterXML));
                        break;
                    default:
                        break;
                }
             
                _ReportLoader.RepKey = RepKey;
                _ReportLoader.ReportSqlParameter = parmList;
                _ReportLoader.ReportName = RptName;
                _ReportLoader.RptPrintCondition = printCondition;

                if (RptName.ToUpper().EndsWith(".RPT"))
                {
                    DataSet ds = GFunc.ExecuteProcDataSet(SYSRep.Get(RepKey).RPTRecordSource1, parmList);
                    DataTable dt = ds.Tables[0];
                    if (_ReportLoader.CheckPrintCondition(null, RepKey, ref dt) == false)
                        return false;

                    DataTable dtSource;
                     //Add QRCode column and data into the datasource
                    if (RptName.ToLower().Contains("qrcode"))
                        dtSource = _ReportLoader.GetSourceWithQRCode(dt, Path.GetExtension(RptName));
                    else
                        dtSource = dt;
                    
                    _ReportLoader.ReportDataSource = dt;

                    CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();                   
                    rptDoc.Load(Application.StartupPath + @"\Reports\" + RptName);
                    rptDoc.SetDataSource(dtSource);

                    _ReportLoader.ReportParameter = GetReportParameters();                   

                    foreach (ReportParameter p in _ReportLoader.ReportParameter)
                    {
                        if (rptDoc.ParameterFields[p.ParameterName] != null) /* modified by YST on 2021/09/07 */
                            rptDoc.SetParameterValue(p.ParameterName, p.ParameterValue);
                    }

                    if (RptName == "zAR_DO30.rpt" || RptName == "zAR_IV60.rpt" || RptName == "zAR_IV50.rpt" || RptName == "zAR_IV60(MBU).rpt" || RptName == "zAR_IV60(Finance).rpt")
                        rptDoc.SetParameterValue("pHideCollectPayment", CollectPayment.Checked);

                    if (RptName == "zAR_IVC50.rpt")
                        rptDoc.SetParameterValue("pHidePaymentMethod", PaymentMethod.Checked);


                    if (pType == GEnum.PrintType.Preview)
                    {
                        frmReportViewer fRptViewer = new frmReportViewer();
                        fRptViewer.RepKey = RepKey;    
                        fRptViewer.RptName = RptName;                        
                        fRptViewer.RptDocument = rptDoc;
                        fRptViewer.MdiParent = frmMain.gfrmMain;
                        fRptViewer.reportLoader = _ReportLoader;
                        
                        fRptViewer.Show();
                        //Mic check , Assign the event to raise if the user clicked Print button later
                        if (DOC != null)
                        {
                            fRptViewer.DocCodeKey = (GEnum.SystemCode)DOC.DocCodeKey;
                            if (DOC.DocPrinted == false) //Only assign if it has not been printed yet
                                fRptViewer.DocPrinted += new GVar.DocPrintUpdateEvent(DocumentPrinted);
                        }
                        //else
                        //    fRptViewer.DocCodeKey = (GEnum.SystemCode)_RepKey;

                        fRptViewer.docXML = docKeyFilterXML;
                    }
                    else if (pType == GEnum.PrintType.Print)
                    {
                        if (GFunc.IsNE(rptDoc.PrintOptions.PrinterName))
                        {
                            MsgBox.Show("The report has not set up printer yet. Please preview the report and set up the printer first.");
                            return false;
                        }

                        _ReportLoader._reportFileName = RptName;
                        _ReportLoader.rptDoc = rptDoc;

                        if (_ReportLoader.ReportParameter.Count < rptDoc.ParameterFields.Count) //for prompt parameter like Adhoc Remark
                        {
                            for (int i = _ReportLoader.ReportParameter.Count; i < rptDoc.ParameterFields.Count; i++)
                            {
                                if (!rptDoc.ParameterFields[i].HasCurrentValue)
                                {
                                    frmInputDialog f = new frmInputDialog(rptDoc.ParameterFields[i].PromptText, "Report", "", "OK", "Cancel");
                                    if (f.ShowDialog() == DialogResult.OK)
                                    {
                                        ReportParameter p = new ReportParameter(rptDoc.ParameterFields[i].Name, f.txtInput.Text);
                                        _ReportLoader.ReportParameter.Add(p);
                                        rptDoc.SetParameterValue(rptDoc.ParameterFields[i].Name, f.txtInput.Text);
                                    }
                                    else
                                        return false;
                                }
                            }
                        }
                        if (printCondition == "Fax")
                        {
                            frmReportViewer fRptViewer = new frmReportViewer();
                            fRptViewer.RepKey = RepKey;
                            fRptViewer.RptName = RptName;
                            fRptViewer.RptDocument = rptDoc;
                            fRptViewer.MdiParent = frmMain.gfrmMain;
                            fRptViewer.reportLoader = _ReportLoader;
                            fRptViewer.SendFax(1, false, 0, 0);
                            fRptViewer.Close();
                            return true;
                        }
                        else
                        {
                            if (GFunc.NEStr(objRpt.Custom2, string.Empty) == string.Empty)// normal printing
                                return _ReportLoader.Print(NoCopies, false, true);
                            else // multi printing for BengHui                             
                                return _ReportLoader.PrintMulti(objRpt.Custom2, rptDoc, NoCopies,Title.Text);                            
                        }
                        
                    }
                    else
                    {
                        if (DocEmail == string.Empty)
                            _ReportLoader.SendEmail(Email.Text, Subject.Text, Message.Text, ReportFileType.AcrobatPDFFile);
                        else
                            _ReportLoader.SendEmail(DocEmail, Subject.Text, Message.Text, ReportFileType.AcrobatPDFFile);
                    }
                }
                else
                {
                    _ReportLoader.ReportParameter = GetReportParameters();

                    if (dtStockCount != null)
                    {
                        _ReportLoader.LoadReport(dtStockCount, RptName);
                    }
                    else
                    {
                        _ReportLoader.LoadReportWCondition((int)RepKey, true);
                    }

                    if (pType == GEnum.PrintType.Preview)
                    {
                        if (_ReportLoader.PrintPreview())
                        {
                            if (DOC != null)
                            {
                                _ReportLoader.RpxViewerForm.DocCodeKey = (GEnum.SystemCode)DOC.DocCodeKey;                             
                            }
                            return true;
                        }
                    }
                    else if (pType == GEnum.PrintType.Print)
                    {
                        return _ReportLoader.Print(NoCopies, false, true);
                    }
                    else
                    {
                        if (DocEmail == string.Empty)
                            return _ReportLoader.SendEmail(Email.Text, Subject.Text, Message.Text, ReportFileType.AcrobatPDFFile);
                        else
                            return _ReportLoader.SendEmail(DocEmail, Subject.Text, Message.Text, ReportFileType.AcrobatPDFFile);
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

            return false;
        }

        

        private void LayoutVisible_Set()
        {
            try
            {
                switch ((int)DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Packing_List:
                    case (int)GEnum.SystemCode.Sales_Adjustment:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                    case (int)GEnum.SystemCode.Bank_Revaluation:
                        Layout.Visible = false;
                        SettinggroupBox.Visible = false;
                        break;
                    case (int)GEnum.SystemCode.Purchase_Plan:
                        Layout.Visible = false;
                        SettinggroupBox.Visible = true;

                        ColumnText.Visible  = false;
                        Marking.Visible = false;
                        ID.Visible = false;
                        Description.Visible = false;
                        Qty.Visible = false;
                        Price.Visible = false;
                        Amount.Visible = false;
                        Total.Visible = false;
                        ItemCount.Visible = false;
                        LetterHead.Visible = true;
                        HideDisTotalChg.Visible = false;
                        break;
                    //case (int)GEnum.SystemCode.Quotation:
                    //    MSTCon objCon= MSTCon.Get(_ConKey);
                    //    if(GFunc.NEBool(objCon.ActiveWithProblem,false))
                    //    {
                    //        RptFile1.ReadOnly = true;
                    //        RptFile2.ReadOnly = true;
                    //    }
                    //    else if (RptFile1.ReadOnly)
                    //    {
                    //        RptFile1.ReadOnly = false;
                    //        RptFile2.ReadOnly = false;
                    //    }
                    //    break;
                    default :
                        Layout.Visible = true;
                        SettinggroupBox.Visible = true;
                        break;

                }
                
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }

     
        private void ConEmailAddress_Get()
        {
            try
            {
                GlobalUI.BindComboValue(Email, GVar.ListSettingID.REFContactInforConEmail + "%" + _ConKey);
            }
            catch (TAException ex)
            {
                throw Error(ex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }

        //private List<ReportParameter> GetReportParameters()
        //{
        //    //Set Parameter For Report Which Are Already Define In This Form(Show Total,Show Price,ect.) If There Are No Parameter, Process Also OK.
        //    try
        //    {
        //        List<ReportParameter> l_Reval = new List<ReportParameter>();

        //        string opCmpValue = SysOptionUtility.GetStr("CompanyName");
        //        string opCmpRegValue = SysOptionUtility.GetStr("CompanyRegNumber");
        //        int opCmpAddrValue = SysOptionUtility.GetInt("DefaultLetterHeadAddr");


        //        string CmpAddr = GFunc.AddrGet(opCmpAddrValue);


        //        // can't use objRepListFactory.ObjRepParameters. because there is no records in sys_repCriteria for printing.
        //        ReportParameter l_ParampCmpName = new ReportParameter("pCmpName", opCmpValue);
        //        ReportParameter l_ParampCmpAddr = new ReportParameter("pCmpAddr", CmpAddr);
        //        ReportParameter l_ParampGSTRegNum = new ReportParameter("pGSTRegNum", opCmpRegValue);
        //        ReportParameter l_ParampRepTitle = new ReportParameter("pRepTitle", objRepListFactory.SYSRepRpt.RptTitle);
        //        ReportParameter l_ParampHideColHead = new ReportParameter("pHideColHead", ColumnText.Checked);
        //        ReportParameter l_ParampHideFinalTotal = new ReportParameter("pHideFinalTotal", Total.Checked);
        //        ReportParameter l_ParampHideItmAmt = new ReportParameter("pHideItmAmt", Amount.Checked);
        //        ReportParameter l_ParampHideItmCount = new ReportParameter("pHideItmCount", ItemCount.Checked);
        //        ReportParameter l_ParampHideItmDes = new ReportParameter("pHideItmDes", Description.Checked);
        //        ReportParameter l_ParampHideItmID = new ReportParameter("pHideItmID", ID.Checked);
        //        ReportParameter l_ParampHideItmMarking = new ReportParameter("pHideItmMarking", Marking.Checked);
        //        ReportParameter l_ParampHideItmPrice = new ReportParameter("pHideItmPrice", Price.Checked);
        //        ReportParameter l_ParampHideItmQty = new ReportParameter("pHideItmQty", Qty.Checked);
        //        ReportParameter l_ParampHideLetterHead = new ReportParameter("pHideLetterHead", LetterHead.Checked);
        //        ReportParameter l_ParampShwItmOnly = new ReportParameter("pShwItmOnly", HideDisTotalChg.Checked);

        //        l_Reval.Add(l_ParampCmpName);
        //        l_Reval.Add(l_ParampCmpAddr);
        //        l_Reval.Add(l_ParampGSTRegNum);
        //        l_Reval.Add(l_ParampRepTitle);
        //        l_Reval.Add(l_ParampHideColHead);
        //        l_Reval.Add(l_ParampHideFinalTotal);
        //        l_Reval.Add(l_ParampHideItmAmt);
        //        l_Reval.Add(l_ParampHideItmCount);
        //        l_Reval.Add(l_ParampHideItmDes);
        //        l_Reval.Add(l_ParampHideItmID);
        //        l_Reval.Add(l_ParampHideItmMarking);
        //        l_Reval.Add(l_ParampHideItmPrice);
        //        l_Reval.Add(l_ParampHideItmQty);
        //        l_Reval.Add(l_ParampHideLetterHead);
        //        l_Reval.Add(l_ParampShwItmOnly);

        //        return l_Reval;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex, false);
        //    }
        //}

        private List<ReportParameter> GetReportParameters()
        {
            try
            {
                REFCurr objCurr = null;
                List<ReportParameter> repportParmList = new List<ReportParameter>();
                string opCmpValue = SysOptionUtility.GetStr("CompanyName");
                string opCountryCurrSym = string.Empty;
                string opHomeCurrSym = string.Empty;

          
                string opCmpRegValue = SysOptionUtility.GetStr("GSTRegNumber");
                int opCmpAddrValue = SysOptionUtility.GetInt("DefaultLetterHeadAddr");
                string CmpAddr = GFunc.AddrGet(opCmpAddrValue);

                objCurr = REFCurr.Get(SysOptionUtility.GetInt("CountryCurrency"));
                opCountryCurrSym = objCurr.SymHdom;

                objCurr = REFCurr.Get(1);
                opHomeCurrSym = objCurr.SymHdom;

                SYSRepParas objRepParas = SYSRepParas.Get(RepKey);
                
                foreach (DataRow obj in objRepParas.Rows)
                {
                    ReportParameter l_Paramp = new ReportParameter(obj["ParName"].ToString(), false);
                  
                        switch (obj["ParName"].ToString().ToLower())
                        {
                            case "pcmpname":
                                l_Paramp.ParameterValue = opCmpValue;
                                break;                           
                            case "pcmpaddr":
                                l_Paramp.ParameterValue = CmpAddr;
                                break;
                            case "pgstregnum":
                                l_Paramp.ParameterValue = opCmpRegValue;
                                break;
                            case "preptitle":
                                l_Paramp.ParameterValue = Title.Text;
                                break;
                            case "phidecolhead":
                                l_Paramp.ParameterValue = !ColumnText.Checked;
                                break;
                            case "phidefinaltotal":
                                l_Paramp.ParameterValue = !Total.Checked;
                                break;
                            case "phideitmamt":
                                l_Paramp.ParameterValue = !Amount.Checked;
                                break;
                            case "phideitmcount":
                                l_Paramp.ParameterValue = !ItemCount.Checked;
                                break;
                            case "phideitmdes":
                                l_Paramp.ParameterValue = !Description.Checked;
                                break;
                            case "phideitmid":
                                l_Paramp.ParameterValue = !ID.Checked;
                                break;
                            case "phideitmmarking":
                                l_Paramp.ParameterValue = !Marking.Checked;
                                break;
                            case "phideitmprice":
                                l_Paramp.ParameterValue = !Price.Checked;
                                break;
                            case "phideitmqty":
                                l_Paramp.ParameterValue = !Qty.Checked;
                                break;
                            case "phideletterhead":
                                l_Paramp.ParameterValue = !LetterHead.Checked;
                                break;
                            case "pshwitmonly":
                                l_Paramp.ParameterValue = HideDisTotalChg.Checked;
                                break;
                            //ttm
                            case "phidecollectpayment":
                                l_Paramp.ParameterValue = CollectPayment.Checked;
                                break;
                            //ttm
                            case "phidepaymentmethod":
                                l_Paramp.ParameterValue = PaymentMethod.Checked;
                                break;
                            case "phidehscode":
                                l_Paramp.ParameterValue = HSCode.Checked;
                                break;
                            case "phidecoo":
                                l_Paramp.ParameterValue = COO.Checked;
                                break;

                        //case "psubreppath":

                        //    l_Paramp.ParameterValue = _ReportLoader.ReportName == "AP_PY1.rpx" ? Application.StartupPath.ToString() + "\\AP_PY_Exp.rpx" : Application.StartupPath.ToString() + "\\AR_PY_Exp.rpx";
                        //    break;  
                        default:
                                //fViewr.rptDoc.Parameters[obj.ParName].PromptUser = true;
                                l_Paramp.ParameterValue = obj["Custom1"].ToString();
                                break;
                        }                   

                    repportParmList.Add(l_Paramp);
                }

                return repportParmList;
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }      
        
        private void LayoutSetting()
        {
            try
            {
                int CheckSwitch = GFunc.NEInt(Layout.Value, -1);
                switch (CheckSwitch)
                {
                    case 10: //Option 'All'
                        ColumnText.Checked = true;
                        Marking.Checked = true;
                        ID.Checked = true;
                        Description.Checked = true;
                        Qty.Checked = true;
                        Price.Checked = true;
                        Amount.Checked = true;
                        Total.Checked = true;
                        //ItemCount.Checked = true;
                        //LetterHead.Checked = true;
                        HideDisTotalChg.Checked = false;
                        break;

                    case 20: //Option 'Description'
                        ColumnText.Checked = true;
                        Marking.Checked = true;
                        ID.Checked = true;
                        Description.Checked = true;
                        Qty.Checked = true;
                        ItemCount.Checked = true;
                        HideDisTotalChg.Checked = false;
                        LetterHead.Checked = true;
                        Price.Checked = false;
                        Amount.Checked = false;
                        Total.Checked = false;
                        break;

                    case 30: //Option 'Price'
                        ColumnText.Checked = true;
                        Marking.Checked = true;
                        ID.Checked = true;
                        Description.Checked = true;
                        Qty.Checked = true;
                        ItemCount.Checked = true;
                        HideDisTotalChg.Checked = false;
                        LetterHead.Checked = true;
                        Price.Checked = true;
                        Amount.Checked = false;
                        Total.Checked = false;
                        break;

                    case 40: //Option 'Amount'
                        ColumnText.Checked = true;
                        Marking.Checked = true;
                        ID.Checked = true;
                        Description.Checked = true;
                        Qty.Checked = true;
                        ItemCount.Checked = true;
                        HideDisTotalChg.Checked = false;
                        LetterHead.Checked = true;
                        Price.Checked = true;
                        Amount.Checked = true;
                        Total.Checked = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }
        public bool Svr_PrintedStatus_Set(int DocKey)
        {
            try
            {
                //Change Print Status 
                _ReportLoader.DocPrintStatus_Set((int)DocCodeKey, DocKey);

                return true;
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
        private void Obj_PrintedStatus_Set()
        {
            try
            {
                switch (_RepKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Bank_Revaluation:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Purchase_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Sales_Adjustment:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Packing_List:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Works_Order:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Adjustment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Deposit:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                        DOC.DocPrinted = true;
                        DOC.IsDirty = false;
                        break;
                    default:
                        break;
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }

        /* added by YST - temp code to investigate DocPrinted error */
        private void PrintFailed(string callFunc)
        {
            if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM && DOC.DocCodeKey == (int)GEnum.SystemCode.Delivery_Order)
            {
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@receivedemail", "eservices@benghui.com"));
                parmList.Add(new SqlParameter("@emailsubject", DBCode.BHM + " : " + DOC.DocID + " - Print Failed"));
                parmList.Add(new SqlParameter("@emailbody", "<p>" + DateTime.Now + " >> " + this.Name + " >> " + callFunc + "</p>"));
                GFunc.ExecuteNonQueryProc("Send_DBMail", parmList);
            }
        }

        //Error Methods
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
                    l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name });

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
