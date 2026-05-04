using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using System.Data.OleDb;
using BOLib;
using System.Runtime.InteropServices;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;

namespace WinUI
{
    public partial class frmReportViewer : Form
    {
        #region Member Variables, Properties, Constructors and Destructors
      
        private int zoom;
        public GVar.DocPrintUpdateEvent DocPrinted = null;

        public GEnum.SystemCode DocCodeKey { get; set; }//use in Document Printout
        public int? DocKey { get; set; }
        public string docXML; //for multi data

        public ReportLoader reportLoader { get; set; }

        public ReportDocument RptDocument
        {
            get;
            set;
        }       
        public string RptName
        {
            get;
            set;
        }

        public string RptTitle
        {
            get;
            set;
        }
        public int RepKey
        {
            get;
            set;
        }
       
        public frmReportViewer()
        {
            InitializeComponent();
        }

        #endregion

        #region Window Form Events

        private void frmReportViewer_Load(object sender, EventArgs e)
        {
            try
            {
                this.AddZoomsToCombo();

                // this.crRptViewer.ReportSource = null;
                this.crRptViewer.ReportSource = this.RptDocument;
                this.crRptViewer.Refresh();

                tstGoToPage.Text = "1";
                if (string.IsNullOrEmpty(this.RptName))
                    this.Text = "Report [Untitled]";
                else
                    this.Text = "Report [" + this.RptName + "]";


            }
            catch (Exception ex)
            {
                MessageBox.Show("Report loading fail! \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        #endregion

        #region Window Controls Events

        private void portableDocumentPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // old source code directly convert pdf from crystalreport decision

                DiskFileDestinationOptions dfdOptions;
                ExportOptions exOptions;

                SaveFileDialog sfDialog = new SaveFileDialog();
                sfDialog.Filter = "Portable Document Format (*.pdf)|*.pdf";
                sfDialog.Title = "Export Report";
                sfDialog.FileName = Path.GetFileName(RptDocument.FileName).Split('.')[0];
                DialogResult drResult = sfDialog.ShowDialog();
                if (drResult == DialogResult.OK)
                {
                    dfdOptions = new DiskFileDestinationOptions();
                    exOptions = this.RptDocument.ExportOptions;
                    exOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    exOptions.FormatOptions = null;
                    exOptions.ExportFormatType = ExportFormatType.PortableDocFormat;

                    dfdOptions.DiskFileName = sfDialog.FileName;
                    exOptions.DestinationOptions = dfdOptions;
                    //RptFileName
                    this.RptDocument.Export();
                }
            }
            catch(Exception ex)
            {
                MsgBox.Show("Export failed. The file to replace might be opened. If not, contact the eservices.");
            }

            //end old source code

            ////changed by nnt on 10 Oct 2019
            ////Covert doc first. Then doc to pdf by Microsoft Interop because crystal decision method can not change pdf with unicode embedded.
            //DiskFileDestinationOptions dfdOptions;
            //ExportOptions exOptions;

            //dfdOptions = new DiskFileDestinationOptions();
            //exOptions = this.RptDocument.ExportOptions;
            //exOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            //exOptions.FormatOptions = null;
            //exOptions.ExportFormatType = ExportFormatType.WordForWindows;
            //string inLineDocPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"zGL_JNOMS1.doc");

            //dfdOptions.DiskFileName = inLineDocPath;
            //exOptions.DestinationOptions = dfdOptions;
            ////RptFileName
            //this.RptDocument.Export();

            ////end convert doc and save internal folder of the boss system////

            ////start convert doc to pdf by using Microsoft Interop word
            //try
            //{
            //    SaveFileDialog sfDialog = new SaveFileDialog();
            //    sfDialog.Filter = "Portable Document Format (*.pdf)|*.pdf";
            //    sfDialog.Title = "Export Report";
            //    sfDialog.FileName = Path.GetFileName(RptDocument.FileName).Split('.')[0];
            //    DialogResult drResult = sfDialog.ShowDialog();
            //    if (drResult == DialogResult.OK)
            //    {
            //        object source = inLineDocPath;
            //        object destination = sfDialog.FileName;
            //        object missing = Type.Missing;
            //        Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application();
            //        application.Visible = true;
            //        application.Activate();
            //        //Open the source document
            //        application.Documents.Open(ref source, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
            //                                    ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
            //        application.Application.Visible = false;
            //        application.WindowState = Microsoft.Office.Interop.Word.WdWindowState.wdWindowStateMinimize;
            //        object format = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
            //        //Export it in the specified format
            //        application.ActiveDocument.SaveAs(ref destination, ref format, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
            //                                            ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
            //        application.ActiveDocument.Close();

            //    }
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.Show(ex.Message);
            //}
            //end convert doc to pdf by using Microsoft Interop word
        }

        private void mSWordDOCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiskFileDestinationOptions dfdOptions;
            ExportOptions exOptions;

            SaveFileDialog sfDialog = new SaveFileDialog();
            sfDialog.Filter = "Microsoft Word Document (*.doc)|*.doc";
            sfDialog.Title = "Export Report";
            sfDialog.FileName = Path.GetFileName(RptDocument.FileName).Split('.')[0];
            DialogResult drResult = sfDialog.ShowDialog();
            if (drResult == DialogResult.OK)
            {
                dfdOptions = new DiskFileDestinationOptions();
                exOptions = this.RptDocument.ExportOptions;
                exOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                exOptions.FormatOptions = null;
                exOptions.ExportFormatType = ExportFormatType.WordForWindows;

                dfdOptions.DiskFileName = sfDialog.FileName;
                exOptions.DestinationOptions = dfdOptions;
                this.RptDocument.Export();
            }
        }

        private void mSExcelXLSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiskFileDestinationOptions dfdOptions;
            ExportOptions exOptions;

            SaveFileDialog sfDialog = new SaveFileDialog();
            sfDialog.Filter = "Microsoft Excel Document (*.xls)|*.xls";
            sfDialog.Title = "Export Report";
            sfDialog.FileName = Path.GetFileName(RptDocument.FileName).Split('.')[0];
            DialogResult drResult = sfDialog.ShowDialog();
            if (drResult == DialogResult.OK)
            {
                dfdOptions = new DiskFileDestinationOptions();
                exOptions = this.RptDocument.ExportOptions;
                exOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                exOptions.FormatOptions = null;
                exOptions.ExportFormatType = ExportFormatType.Excel;

                dfdOptions.DiskFileName = sfDialog.FileName;
                exOptions.DestinationOptions = dfdOptions;
                this.RptDocument.Export();
            }

            
        }

        private void mSExcelXLSDataOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DiskFileDestinationOptions dfdOptions;
                ExportOptions exOptions;

                SaveFileDialog sfDialog = new SaveFileDialog();
                sfDialog.Filter = "Microsoft Excel Document (*.xls)|*.xls";
                sfDialog.Title = "Export Report";
                sfDialog.FileName = Path.GetFileName(RptDocument.FileName).Split('.')[0];
                DialogResult drResult = sfDialog.ShowDialog();
                if (drResult == DialogResult.OK)
                {
                    dfdOptions = new DiskFileDestinationOptions();
                    exOptions = this.RptDocument.ExportOptions;
                    exOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    exOptions.FormatOptions = null;
                    exOptions.ExportFormatType = ExportFormatType.ExcelRecord;

                    dfdOptions.DiskFileName = sfDialog.FileName;
                    exOptions.DestinationOptions = dfdOptions;

                    this.RptDocument.Export();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void crystalReportRPTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiskFileDestinationOptions dfdOptions;
            ExportOptions exOptions;

            SaveFileDialog sfDialog = new SaveFileDialog();
            sfDialog.Filter = "Crystal Report File (*.rpt)|*.rpt";
            sfDialog.Title = "Export Report";
            sfDialog.FileName = Path.GetFileName(RptDocument.FileName).Split('.')[0];
            DialogResult drResult = sfDialog.ShowDialog();
            if (drResult == DialogResult.OK)
            {
                dfdOptions = new DiskFileDestinationOptions();
                exOptions = this.RptDocument.ExportOptions;
                exOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                exOptions.FormatOptions = null;
                exOptions.ExportFormatType = ExportFormatType.CrystalReport;

                dfdOptions.DiskFileName = sfDialog.FileName;
                exOptions.DestinationOptions = dfdOptions;

                this.RptDocument.Export();
            }
        }

        private void tscZoom_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(e.ClickedItem.Tag) != 999)
                {
                    zoom = Convert.ToInt32(e.ClickedItem.Tag);
                    crRptViewer.Zoom(zoom);
                }
                else
                {
                    string defaultValue;
                    if (zoom == 1 || zoom == 2)
                        defaultValue = "100";
                    else
                        defaultValue = zoom.ToString();

                    //frmInputDialog fInput = new frmInputDialog("Please specify the zooming factor: (25-400)", "Zooming", defaultValue, "OK", "Cancel", true);
                    //fInput.inputEvent += new frmInputDialog.CustomEvent(ZoomEventHandler);
                    //fInput.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsbPrintSetup_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                frmPageSetup f = new frmPageSetup(RptDocument);
                if (f.ShowDialog() == DialogResult.OK)
                {                   
                    this.RptDocument.SaveAs(Application.StartupPath + @"\Reports\" + this.RptName);                   
                    this.RptDocument.Load(Application.StartupPath + @"\Reports\" + this.RptName);
                    crRptViewer.ReportSource = this.RptDocument;
                    this.crRptViewer.Refresh();
                }
                f.Close();              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        public Int32 GetPaperSize(String sPrinterName, String sPaperSizeName)
        {
            PrintDocument docPrintDoc = new PrintDocument();
            docPrintDoc.PrinterSettings.PrinterName = sPrinterName;
            for (int i = 0; i < docPrintDoc.PrinterSettings.PaperSizes.Count; i++)
            {
                int raw = docPrintDoc.PrinterSettings.PaperSizes[i].RawKind;
                if (docPrintDoc.PrinterSettings.PaperSizes[i].PaperName == sPaperSizeName)
                {
                    return raw;
                }
            }
            return 0;
        }

        private void tsbPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (GFunc.IsNE(this.RptDocument.PrintOptions.PrinterName))
                {
                    MsgBox.Show("Please set up the print setting first.");
                    return;
                }
                SYSRepRpt rpt = SYSRepRpt.Get(this.RepKey, RptName, 3);
                crRptViewer.ShowLastPage();
                int lastPageNo = crRptViewer.GetCurrentPageNumber();
                crRptViewer.ShowFirstPage();

                frmPrint f = new frmPrint();
                f.PrinterNm = this.RptDocument.PrintOptions.PrinterName;
                f.FromPage = 1;
                f.ToPage = lastPageNo;

                DialogResult dlgResult = f.ShowDialog();
                if (dlgResult == DialogResult.OK)
                {
                    if (this.reportLoader != null)
                    {
                        if (this.reportLoader.RptPrintCondition == "Fax")
                            SendFax(f.Copies, f.Collate, f.FromPage, f.ToPage);
                        else
                        {
                            if (GFunc.NEStr(rpt.Custom2, string.Empty) == string.Empty)// normal printing
                                this.RptDocument.PrintToPrinter(f.Copies, f.Collate, f.FromPage, f.ToPage);
                            else // multi printing for BengHui   
                            {
                                reportLoader.PrintMulti(rpt.Custom2, this.RptDocument, (short)f.Copies, f.Collate, f.FromPage, f.ToPage,RptTitle,this.docXML);
                            }
                        }  
                    }
                    else
                        this.RptDocument.PrintToPrinter(f.Copies, f.Collate, f.FromPage, f.ToPage);

                   if (DocPrinted != null)
                   {
                       DocPrinted.Invoke();
                   }
                   else if (this.docXML!=null)
                       reportLoader.DocPrintStatus_Set(this.RepKey, this.docXML);

                }
                f.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        public void SendFax(int Copies, bool Collate, int FromPage, int ToPage)
        {
            try
            {
                if (this.RptDocument.PrintOptions.PrinterName == "")
                {
                    MessageBox.Show("Fax sending failed. Please set up the printer first.");
                    return;
                }

                DataTable dtHdrRows = (from row in this.reportLoader.ReportDataSource.AsEnumerable()
                                       group row by new
                                                {
                                                    DocKey = row.Field<int>("DocKey"),
                                                    DocID = row.Field<string>("DocID"),
                                                    DocBAddrFax = row.Field<string>("DocBAddrFax")
                                                }
                                           into grp
                                           select new
                                           {
                                               DocKey = grp.Key.DocKey,
                                               DocID = grp.Key.DocID,
                                               DocBAddrFax = grp.Key.DocBAddrFax
                                           }).AsDataTable();

                if (dtHdrRows.Rows.Count == 1)//Single Document. No need to reload
                {
                    if (!GFunc.IsNE(dtHdrRows.Rows[0]["DocBAddrFax"]))
                    {
                        string tmpfileName = System.IO.Path.GetTempPath() + dtHdrRows.Rows[0]["DocID"].ToString().Replace("/","")
                                              + "@F211 " + dtHdrRows.Rows[0]["DocBAddrFax"] + "@.rpt";
                        if (File.Exists(tmpfileName))
                            File.Delete(tmpfileName);
                        this.RptDocument.SaveAs(tmpfileName);
                        this.RptDocument.PrintToPrinter(Copies, Collate,FromPage,ToPage);                        
                    }
                }
                else
                {
                    foreach (DataRow dr in dtHdrRows.Rows) //Multiple Docs. Need to split
                    {
                        if (GFunc.IsNE(dr["DocBAddrFax"]))
                            continue;

                        CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        rptDoc.Load(Application.StartupPath + @"\Reports\" + this.RptName);
                        reportLoader.ReportDataSource.DefaultView.RowFilter = "DocKey=" + dr["DocKey"];
                        rptDoc.SetDataSource(reportLoader.ReportDataSource.DefaultView.ToTable());                        

                        foreach (ReportParameter p in this.reportLoader.ReportParameter)
                        {                           
                            rptDoc.SetParameterValue(p.ParameterName, p.ParameterValue);
                        }
                        if (reportLoader.ReportParameter.Count < this.RptDocument.ParameterFields.Count) //for prompt parameter like Adhoc Remark
                        {
                            for (int i = reportLoader.ReportParameter.Count; i < this.RptDocument.ParameterFields.Count; i++)
                            {
                                if (!rptDoc.ParameterFields[i].HasCurrentValue && this.RptDocument.ParameterFields[i].HasCurrentValue)
                                    rptDoc.SetParameterValue(rptDoc.ParameterFields[i].Name, this.RptDocument.ParameterFields[i].CurrentValues[0].ToString());
                            }
                        }                        

                        string fileName = System.IO.Path.GetTempPath() + dtHdrRows.Rows[0]["DocID"].ToString().Replace("/", "")
                            + "@F211 " + dr["DocBAddrFax"] + "@.rpt";
                        if (File.Exists(fileName))
                            File.Delete(fileName);
                        rptDoc.SaveAs( fileName);
                        rptDoc.PrintToPrinter(Copies, Collate, FromPage,ToPage);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fax sending failed."+ex.Message);
            }
        }

        private void tsbFirstPage_Click(object sender, EventArgs e)
        {
            crRptViewer.ShowFirstPage();
            tstGoToPage.Text = crRptViewer.GetCurrentPageNumber().ToString();
        }

        private void tsbPreviousPage_Click(object sender, EventArgs e)
        {
            crRptViewer.ShowPreviousPage();
            tstGoToPage.Text = crRptViewer.GetCurrentPageNumber().ToString();
        }

        private void tsbNextPage_Click(object sender, EventArgs e)
        {
            crRptViewer.ShowNextPage();
            tstGoToPage.Text = crRptViewer.GetCurrentPageNumber().ToString();
        }

        private void tsbLastPage_Click(object sender, EventArgs e)
        {
            crRptViewer.ShowLastPage();
            tstGoToPage.Text = crRptViewer.GetCurrentPageNumber().ToString();
        }

        private void tsbSearch_Click(object sender, EventArgs e)
        {
            frmInputDialog fInput = new frmInputDialog("Find What:", "Find Text", "", "&Find Next", "Cancel");
            fInput.inputEvent += new frmInputDialog.CustomEvent(FindEventHandler);
            fInput.ShowDialog();
        }

        private void frmReportViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            crRptViewer.ReportSource = null;
            if (this.RptDocument != null)
                this.RptDocument.Dispose();
            this.RptDocument = null;            
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Private and Public Functions

        private void AddZoomsToCombo()
        {
            this.tscZoom.DropDownItems.Add("Page Width");
            this.tscZoom.DropDownItems[0].Tag = 1;
            this.tscZoom.DropDownItems.Add("Whole Page");
            this.tscZoom.DropDownItems[1].Tag = 2;
            this.tscZoom.DropDownItems.Add("400%");
            this.tscZoom.DropDownItems[2].Tag = 400;
            this.tscZoom.DropDownItems.Add("300%");
            this.tscZoom.DropDownItems[3].Tag = 300;
            this.tscZoom.DropDownItems.Add("200%");
            this.tscZoom.DropDownItems[4].Tag = 200;
            this.tscZoom.DropDownItems.Add("150%");
            this.tscZoom.DropDownItems[5].Tag = 150;
            this.tscZoom.DropDownItems.Add("100%");
            this.tscZoom.DropDownItems[6].Tag = 100;
            this.tscZoom.DropDownItems.Add("75%");
            this.tscZoom.DropDownItems[7].Tag = 75;
            this.tscZoom.DropDownItems.Add("50%");
            this.tscZoom.DropDownItems[8].Tag = 50;
            this.tscZoom.DropDownItems.Add("25%");
            this.tscZoom.DropDownItems[9].Tag = 25;
            this.tscZoom.DropDownItems.Add("Customize...");
            this.tscZoom.DropDownItems[10].Tag = 999;
        }

        #endregion

        #region Custom Event Methods

        private void FindEventHandler(object sender, EventArgs e)
        {
            crRptViewer.SearchForText((sender as TextBox).Text);
            tstGoToPage.Text = crRptViewer.GetCurrentPageNumber().ToString();
        }

        private void ZoomEventHandler(object sender, EventArgs e)
        {
            string z = (sender as TextBox).Text;
            if (z.Length > 0)
            {
                zoom = Convert.ToInt32(z);
                if (zoom > 400 || zoom < 25)
                    throw new Exception("Out of Range");
            }
            crRptViewer.Zoom(zoom);
        }

        #endregion

        private void tstGoToPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                int PageNo = 1;
                Int32.TryParse(tstGoToPage.Text, out PageNo);

                //if (PageNo < 1)
                //    PageNo = 1;
                //else if PageNo>crRptViewer.page
                crRptViewer.ShowNthPage(PageNo);
            }
        }      

        private void crRptViewer_HandleException(object source, CrystalDecisions.Windows.Forms.ExceptionEventArgs e)
        {
           if (e.Exception is EngineException)
           {
              EngineException engEx = (EngineException)e.Exception;
              if (engEx.ErrorID == EngineExceptionErrorID.DataSourceError)
              {
                 e.Handled = true;
                 MessageBox.Show ("An error has occurred while connecting to the database.");
              }
              else if (engEx.ErrorID == EngineExceptionErrorID.LogOnFailed)
              {
                 e.Handled = true;
                 MessageBox.Show("Incorrect Logon Parameters. Check your user name and password.");
              }
           }
        }
        void crRptViewer_ClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
            string HyperLinkText = ""; 

            if (e.ObjectInfo.Hyperlink == null)
                return;                       

            //See example below, a HyperLink set in RPT File 
            //"HyperLinkClient.exe DocCodeKey:"+Replace(ToText({dao.DocCodeKey}),",","")+",DocKey:"+Replace(ToText({dao.DocKey}),",","")

            //Remove extra information, dummy exe file name   
            
            HyperLinkText = e.ObjectInfo.Hyperlink.Replace("HyperLinkClient.exe ", "");
            
            try
            {
                if (HyperLinkText != "" && HyperLinkText.Contains(":"))
                {
                    GEnum.SystemCode docCodeKey = 0;
                    string HyperLinkFieldName = "";
                    string HyperLinkValue = "";

                    string[] vHyperLinkTmp = HyperLinkText.Split(new char[] { ',' });

                    foreach (string item in vHyperLinkTmp)
                    {
                        string[] vFieldAndValue = item.Split(new char[] { ':' });
                        if (GFunc.CompareString(vFieldAndValue[0], "DocCodeKey"))
                        {
                            docCodeKey = (GEnum.SystemCode)GFunc.NEInt(vFieldAndValue[1], 0);
                        }
                        else
                        {
                            HyperLinkFieldName = vFieldAndValue[0].ToString();
                            HyperLinkValue = vFieldAndValue[1].ToString();
                        }
                    }

                    if (DocCodeKey == null || DocCodeKey == 0)
                    {
                        GEnum.SystemCode docCode = GEnum.SystemCode.Customer;//Default
                        switch (HyperLinkFieldName)
                        {
                            case "ConID":
                                docCodeKey = GEnum.SystemCode.Customer;
                                break;
                            default:
                                break;
                        }
                    }
                    OpenLinkForm(docCodeKey, HyperLinkFieldName, HyperLinkValue);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenLinkForm(GEnum.SystemCode docCodeKey, string HyperLinkFieldName, string HyperLinkValue)
        {
            switch (docCodeKey)
            {
                case GEnum.SystemCode.Utility:
                    break;
                case GEnum.SystemCode.Quotation:
                    frmARQO fARQO = new frmARQO(GEnum.SystemCode.Quotation);
                    fARQO.MdiParent = frmMain.gfrmMain;
                    fARQO.Show();
                    fARQO.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));//Open By DocKey.
                    break;
                case GEnum.SystemCode.Reserve_Order:
                    frmARRO ARRO = new frmARRO(GEnum.SystemCode.Reserve_Order);
                    ARRO.MdiParent = frmMain.gfrmMain;
                    ARRO.Show();
                    ARRO.MdiParent = frmMain.gfrmMain;
                    ARRO.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));                    
                    break;
                case GEnum.SystemCode.Sales_Order:
                    frmARSO vfrmARSO = new frmARSO(GEnum.SystemCode.Sales_Order);
                    vfrmARSO.MdiParent = frmMain.gfrmMain;
                    vfrmARSO.Show();
                    vfrmARSO.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                    break;
                //case GEnum.SystemCode.Sales_Adjustment:
                //    frmARADJ vfrmARAdj = new frmARADJ(GEnum.SystemCode.Sales_Adjustment);
                //    vfrmARAdj.MdiParent = frmMain.gfrmMain;
                //    vfrmARAdj.Show();
                //    vfrmARAdj.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));//Open By DocKey.
                //    break;
                //case GEnum.SystemCode.Untrack_SO:
                //    break;
                //case GEnum.SystemCode.Works_Order:
                //    break;
                //case GEnum.SystemCode.Delivery_Order:
                //    frmARDO vfrmARDO = new frmARDO(GEnum.SystemCode.Delivery_Order);
                //    vfrmARDO.MdiParent = frmMain.gfrmMain;
                //    vfrmARDO.Show();
                //    vfrmARDO.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.DO_to_IV_Transfer:
                //    break;
                //case GEnum.SystemCode.Packing_List:
                //    frmARPL vfrmARPL = new frmARPL(GEnum.SystemCode.Packing_List);
                //    vfrmARPL.MdiParent = frmMain.gfrmMain;
                //    vfrmARPL.Show();
                //    vfrmARPL.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Sales_Invoice:
                //    frmARIV vfrmARIV = new frmARIV(GEnum.SystemCode.Sales_Invoice);
                //    vfrmARIV.MdiParent = frmMain.gfrmMain;
                //    vfrmARIV.Show();
                //    vfrmARIV.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Sales_Debit_Note:
                //    frmARIV vfrmARDN = new frmARIV(GEnum.SystemCode.Sales_Debit_Note);
                //    vfrmARDN.MdiParent = frmMain.gfrmMain;
                //    vfrmARDN.Show();
                //    vfrmARDN.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Sales_Credit_Note:
                //    frmARIV vfrmARCN = new frmARIV(GEnum.SystemCode.Sales_Credit_Note);
                //    vfrmARCN.MdiParent = frmMain.gfrmMain;
                //    vfrmARCN.Show();
                //    vfrmARCN.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Payment_Received:
                //    frmARPY vfrmARPY = new frmARPY(GEnum.SystemCode.Payment_Received);
                //    vfrmARPY.MdiParent = frmMain.gfrmMain;
                //    vfrmARPY.Show();
                //    vfrmARPY.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.AR_Opening_Balance:
                //    frmMSTConOpenBal vfrmARopen = new frmMSTConOpenBal(GEnum.SystemCode.AR_Opening_Balance);
                //    vfrmARopen.MdiParent = frmMain.gfrmMain;
                //    vfrmARopen.Show();
                //    break;
                //case GEnum.SystemCode.AR_Revaluation:
                //    frmGLRV vfrmGLVR = new frmGLRV(GEnum.SystemCode.AR_Revaluation);
                //    vfrmGLVR.MdiParent = frmMain.gfrmMain;
                //    vfrmGLVR.Show();
                //    vfrmGLVR.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Contra:
                //    frmARCT vfrmARCT = new frmARCT(GEnum.SystemCode.Contra);
                //    vfrmARCT.MdiParent = frmMain.gfrmMain;
                //    vfrmARCT.Show();
                //    vfrmARCT.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Cash_Sale:
                //    frmARIV vfrmARIVC = new frmARIV(GEnum.SystemCode.Cash_Sale);
                //    vfrmARIVC.MdiParent = frmMain.gfrmMain;
                //    vfrmARIVC.Show();
                //    vfrmARIVC.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Cash_Debit_Note:
                //    frmARIV vfrmARDNC = new frmARIV(GEnum.SystemCode.Cash_Debit_Note);
                //    vfrmARDNC.MdiParent = frmMain.gfrmMain;
                //    vfrmARDNC.Show();
                //    vfrmARDNC.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Cash_Credit_Note:
                //    frmARIV vfrmARCNC = new frmARIV(GEnum.SystemCode.Cash_Credit_Note);
                //    vfrmARCNC.MdiParent = frmMain.gfrmMain;
                //    vfrmARCNC.Show();
                //    vfrmARCNC.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Cash_Adjustment:
                //    frmARADJ vfrmARADJC = new frmARADJ(GEnum.SystemCode.Cash_Adjustment);
                //    vfrmARADJC.MdiParent = frmMain.gfrmMain;
                //    vfrmARADJC.Show();
                //    vfrmARADJC.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Cash_Payment_Received:
                //    frmARPY vfrmARPYC = new frmARPY(GEnum.SystemCode.Cash_Payment_Received);
                //    vfrmARPYC.MdiParent = frmMain.gfrmMain;
                //    vfrmARPYC.Show();
                //    vfrmARPYC.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.AR_Cash_Opening_Balance:
                //    break;
                //case GEnum.SystemCode.AR_Cash_Revaluation:
                //    break;
                //case GEnum.SystemCode.Cash_Contra:
                //    frmARCT vfrmARCTC = new frmARCT(GEnum.SystemCode.Cash_Contra);
                //    vfrmARCTC.MdiParent = frmMain.gfrmMain;
                //    vfrmARCTC.Show();
                //    vfrmARCTC.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Purchase_Plan:
                //    frmAPPN vfrmAPPN = new frmAPPN(GEnum.SystemCode.Purchase_Plan);
                //    vfrmAPPN.MdiParent = frmMain.gfrmMain;
                //    vfrmAPPN.Show();
                //    vfrmAPPN.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Purchase_Requisition:
                //    break;
                //case GEnum.SystemCode.Purchase_Request:
                //    frmAPRQ vfrmAPRQ = new frmAPRQ(GEnum.SystemCode.Purchase_Request);
                //    vfrmAPRQ.MdiParent = frmMain.gfrmMain;
                //    vfrmAPRQ.Show();
                //    vfrmAPRQ.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Purchase_Order:
                //    frmAPPO vfrmAppo = new frmAPPO(GEnum.SystemCode.Purchase_Order);
                //    vfrmAppo.MdiParent = frmMain.gfrmMain;
                //    vfrmAppo.Show();
                //    vfrmAppo.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Purchase_Order_Adjustment:
                //    frmAPPJ vfrmApPOAdj = new frmAPPJ(GEnum.SystemCode.Purchase_Order_Adjustment);
                //    vfrmApPOAdj.MdiParent = frmMain.gfrmMain;
                //    vfrmApPOAdj.Show();
                //    vfrmApPOAdj.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Untrack_PO:
                //    break;
                //case GEnum.SystemCode.AP_PO_Confirm_Number:
                //    break;
                //case GEnum.SystemCode.Purchase_Delivery:
                //    frmAPPD vfrmAPPD = new frmAPPD(GEnum.SystemCode.Purchase_Delivery);
                //    vfrmAPPD.MdiParent = frmMain.gfrmMain;
                //    vfrmAPPD.Show();
                //    vfrmAPPD.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Purchase_Invoice:
                //    frmAPBL vfrmAPBL = new frmAPBL(GEnum.SystemCode.Purchase_Invoice);
                //    vfrmAPBL.MdiParent = frmMain.gfrmMain;
                //    vfrmAPBL.Show();
                //    vfrmAPBL.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Purchase_Debit_Note:
                //    frmAPBL vfrmAPDN = new frmAPBL(GEnum.SystemCode.Purchase_Debit_Note);
                //    vfrmAPDN.MdiParent = frmMain.gfrmMain;
                //    vfrmAPDN.Show();
                //    vfrmAPDN.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Purchase_Credit_Note:
                //    frmAPBL vfrmAPCN = new frmAPBL(GEnum.SystemCode.Purchase_Credit_Note);
                //    vfrmAPCN.MdiParent = frmMain.gfrmMain;
                //    vfrmAPCN.Show();
                //    vfrmAPCN.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Purchase_Adjustment:
                //    if (GFunc.CompareString(HyperLinkFieldName, "DocKey"))
                //    {
                //        frmAPADJ frm_APAdj = new frmAPADJ(GEnum.SystemCode.Purchase_Adjustment);
                //        frm_APAdj.MdiParent = frmMain.gfrmMain;
                //        frm_APAdj.Show();
                //        frm_APAdj.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    }

                //    break;
                //case GEnum.SystemCode.Payment_Issue:
                //    if (GFunc.CompareString(HyperLinkFieldName, "DocKey"))
                //    {
                //        frmAPPY frm_APPY = new frmAPPY(GEnum.SystemCode.Payment_Issue);
                //        frm_APPY.MdiParent = frmMain.gfrmMain;
                //        frm_APPY.Show();
                //        frm_APPY.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    }

                //    break;
                //case GEnum.SystemCode.AP_Opening_Balance:
                //    break;
                //case GEnum.SystemCode.AP_Revaluation:
                //    break;
                //case GEnum.SystemCode.Inventory_Adjustment:
                //    frmINADJ vfrmINADJ = new frmINADJ(GEnum.SystemCode.Inventory_Adjustment);
                //    vfrmINADJ.MdiParent = frmMain.gfrmMain;
                //    vfrmINADJ.Show();
                //    vfrmINADJ.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Inventory_Production:
                //    frmINMFN vfrmINMFN = new frmINMFN(GEnum.SystemCode.Inventory_Production);
                //    vfrmINMFN.MdiParent = frmMain.gfrmMain;
                //    vfrmINMFN.Show();
                //    vfrmINMFN.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Inventory_Transfer:
                //    frmINTRN vfrmINTRN = new frmINTRN(GEnum.SystemCode.Inventory_Transfer);
                //    vfrmINTRN.MdiParent = frmMain.gfrmMain;
                //    vfrmINTRN.Show();
                //    vfrmINTRN.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Issue_Consignment:
                //    frmCSCSI vfrmCSCSI = new frmCSCSI(GEnum.SystemCode.Issue_Consignment);
                //    vfrmCSCSI.MdiParent = frmMain.gfrmMain;
                //    vfrmCSCSI.Show();
                //    vfrmCSCSI.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Return_Consignment:
                //    frmCSCSI vfrmCSCSR = new frmCSCSI(GEnum.SystemCode.Return_Consignment);
                //    vfrmCSCSR.MdiParent = frmMain.gfrmMain;
                //    vfrmCSCSR.Show();
                //    vfrmCSCSR.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Untrack_Issue_Consignment:
                //    break;
                //case GEnum.SystemCode.Order_Consignment:
                //    frmCSCPO vfrmCSCPO = new frmCSCPO(GEnum.SystemCode.Order_Consignment);
                //    vfrmCSCPO.MdiParent = frmMain.gfrmMain;
                //    vfrmCSCPO.Show();
                //    vfrmCSCPO.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Consignment_Order_Adjustment:
                //    break;
                //case GEnum.SystemCode.Untrack_Consignment_Order:
                //    break;
                //case GEnum.SystemCode.Received_Consignment:
                //    frmCSCPD vfrmCSCPD = new frmCSCPD(GEnum.SystemCode.Received_Consignment);
                //    vfrmCSCPD.MdiParent = frmMain.gfrmMain;
                //    vfrmCSCPD.Show();
                //    vfrmCSCPD.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Consignment_Settlement:
                //    frmCSCPS vfrmCSCPS = new frmCSCPS(GEnum.SystemCode.Consignment_Settlement);
                //    vfrmCSCPS.MdiParent = frmMain.gfrmMain;
                //    vfrmCSCPS.Show();
                //    vfrmCSCPS.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Journal:
                //    frmGLJNL vfrmGLJNL = new frmGLJNL(GEnum.SystemCode.Journal);
                //    vfrmGLJNL.MdiParent = frmMain.gfrmMain;
                //    vfrmGLJNL.Show();
                //    vfrmGLJNL.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Deposit:
                //    frmGLDP vfrmGLDP = new frmGLDP(GEnum.SystemCode.Deposit);
                //    vfrmGLDP.MdiParent = frmMain.gfrmMain;
                //    vfrmGLDP.Show();
                //    vfrmGLDP.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Bank_Revaluation:
                //    frmGLRV vfrmGLRV = new frmGLRV(GEnum.SystemCode.Bank_Revaluation);
                //    vfrmGLRV.MdiParent = frmMain.gfrmMain;
                //    vfrmGLRV.Show();
                //    vfrmGLRV.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                //    break;
                //case GEnum.SystemCode.Main_Screen:
                //    break;
                //case GEnum.SystemCode.System_Code:
                //    break;
                //case GEnum.SystemCode.CounterGrp:
                //    break;
                //case GEnum.SystemCode.System_Option:
                //    break;
                //case GEnum.SystemCode.Screen_Customisation:
                //    break;
                //case GEnum.SystemCode.Company_Setup_Check_List:
                //    break;
                //case GEnum.SystemCode.Document_Group:
                //    frmREFDocGrp frm_refDocGrp = new frmREFDocGrp(GFunc.NEInt(HyperLinkValue, 0));
                //    frm_refDocGrp.MdiParent = frmMain.gfrmMain;
                //    frm_refDocGrp.Show();

                //    break;
                //case GEnum.SystemCode.General_List:
                //    break;
                //case GEnum.SystemCode.Audit_Log:
                //    break;
                //case GEnum.SystemCode.Account:
                //    if (GFunc.CompareString(HyperLinkFieldName, "AccKey"))
                //    {
                //        frmMstAcc frm_mstAcc = new frmMstAcc(GFunc.NEInt(HyperLinkValue, 0));
                //        frm_mstAcc.MdiParent = frmMain.gfrmMain;
                //        frm_mstAcc.Show();
                //    }

                //    break;
                //case GEnum.SystemCode.Account_Opening_Balance:
                //    break;
                //case GEnum.SystemCode.Account_Unreconciled_Trans:
                //    break;
                //case GEnum.SystemCode.Period:
                //    break;
                //case GEnum.SystemCode.Branch:
                //    frmMstAccBranch vfrmBranch = new frmMstAccBranch(GFunc.NEInt(HyperLinkValue, 0));
                //    vfrmBranch.MdiParent = frmMain.gfrmMain;
                //    vfrmBranch.Show();
                //    break;
                //case GEnum.SystemCode.Department:
                //    frmMstAccDept vfrmDept = new frmMstAccDept(GFunc.NEInt(HyperLinkValue, 0));
                //    vfrmDept.MdiParent = frmMain.gfrmMain;
                //    vfrmDept.Show();
                //    break;
                //case GEnum.SystemCode.Bank_Reconcilation:
                //    break;
                //case GEnum.SystemCode.COSBatchPost:
                //    break;
                //case GEnum.SystemCode.Currency:
                //    frmREFCurr vRefCurr = new frmREFCurr(GFunc.NEInt(HyperLinkValue, 0));   //CurrID
                //    vRefCurr.MdiParent = frmMain.gfrmMain;
                //    vRefCurr.Show();
                //    break;
                //case GEnum.SystemCode.Bank:
                //    frmREFBank vRefBank = new frmREFBank(GFunc.NEInt(HyperLinkValue, 0));   //BankID
                //    vRefBank.MdiParent = frmMain.gfrmMain;
                //    vRefBank.Show();
                //    break;
                //case GEnum.SystemCode.Payment_Mode:
                //    frmREFPayMode vRefPayMode = new frmREFPayMode(GFunc.NEInt(HyperLinkValue, 0));   //PayModeID
                //    vRefPayMode.MdiParent = frmMain.gfrmMain;
                //    vRefPayMode.Show();
                //    break;
                //case GEnum.SystemCode.Tax_Authority:
                //    frmREFTaxA vRefTaxA = new frmREFTaxA(GFunc.NEInt(HyperLinkValue, 0));   //TaxAID
                //    vRefTaxA.MdiParent = frmMain.gfrmMain;
                //    vRefTaxA.Show();
                //    break;
                //case GEnum.SystemCode.Tax_Group:
                //    frmREFTaxGrp vRefTaxGrp = new frmREFTaxGrp(GFunc.NEInt(HyperLinkValue, 0));   //TaxGrpID
                //    vRefTaxGrp.MdiParent = frmMain.gfrmMain;
                //    vRefTaxGrp.Show();
                //    break;
                //case GEnum.SystemCode.Overhead:
                //    frmREFOverHead vRefOverHead = new frmREFOverHead(GFunc.NEInt(HyperLinkValue, 0));   //OverHeadID
                //    vRefOverHead.MdiParent = frmMain.gfrmMain;
                //    vRefOverHead.Show();
                //    break;
                //case GEnum.SystemCode.Account_Group:
                //    frmREFAccGrp vRefAccGrp = new frmREFAccGrp(GFunc.NEInt(HyperLinkValue, 0));   //AccGrpID
                //    vRefAccGrp.MdiParent = frmMain.gfrmMain;
                //    vRefAccGrp.Show();
                //    break;
                //case GEnum.SystemCode.Sales_Representative:
                //    frmMstSalesRep vfrmSalesRep = new frmMstSalesRep(GFunc.NEInt(HyperLinkValue, 0));
                //    vfrmSalesRep.MdiParent = frmMain.gfrmMain;
                //    vfrmSalesRep.Show();
                //    break;
                //case GEnum.SystemCode.Budget:
                //    break;
                //case GEnum.SystemCode.Transaction_Group:
                //    frmMstAccTranGrp vfrmTranGrp = new frmMstAccTranGrp(GFunc.NEInt(HyperLinkValue, 0));
                //    vfrmTranGrp.MdiParent = frmMain.gfrmMain;
                //    vfrmTranGrp.Show();
                //    break;
                //case GEnum.SystemCode.ARAP_Revaluation:
                //    break;
                //case GEnum.SystemCode.Customer:
                //    frmMSTCon vfrmMstCon = new frmMSTCon(GFunc.NEInt(HyperLinkValue, 0));   //By Use ConID
                //    vfrmMstCon.MdiParent = frmMain.gfrmMain;
                //    vfrmMstCon.Show();
                //    break;
                //case GEnum.SystemCode.Vendor:
                //    frmMSTCon vfrmCon = new frmMSTCon(GFunc.NEInt(HyperLinkValue, 0));
                //    vfrmCon.MdiParent = frmMain.gfrmMain;
                //    vfrmCon.Show();
                //    break;
                //case GEnum.SystemCode.Price_List:
                //    break;
                //case GEnum.SystemCode.Payment_Term:
                //    frmREFTerm vRefTerm = new frmREFTerm(GFunc.NEInt(HyperLinkValue, 0));   //TermID
                //    vRefTerm.MdiParent = frmMain.gfrmMain;
                //    vRefTerm.Show();
                //    break;
                //case GEnum.SystemCode.Territory:

                //    frmREFTerritory frm_refTerritory = new frmREFTerritory(GFunc.NEInt(HyperLinkValue, 0));
                //    frm_refTerritory.MdiParent = frmMain.gfrmMain;
                //    frm_refTerritory.Show();



                //    break;
                //case GEnum.SystemCode.Industry:

                //    frmREFIndustry frm_refIndustry = new frmREFIndustry(GFunc.NEInt(HyperLinkValue, 0));
                //    frm_refIndustry.MdiParent = frmMain.gfrmMain;
                //    frm_refIndustry.Show();

                //    break;
                //case GEnum.SystemCode.Shipping_Mode:
                //    frmREFShipVia vRefShipVia = new frmREFShipVia(GFunc.NEInt(HyperLinkValue, 0));   //ShipViaKey
                //    vRefShipVia.MdiParent = frmMain.gfrmMain;
                //    vRefShipVia.Show();
                //    break;
                //case GEnum.SystemCode.Packing_Type:
                //    frmREFPackingType vRefPackingType = new frmREFPackingType(GFunc.NEInt(HyperLinkValue, 0));   //PackingTypeKey
                //    vRefPackingType.MdiParent = frmMain.gfrmMain;
                //    vRefPackingType.Show();
                //    break;
                //case GEnum.SystemCode.Ship_Name:
                //    break;
                //case GEnum.SystemCode.Inventory:
                //    frmMSTItm vfrmMstItm = new frmMSTItm(GFunc.NEInt(HyperLinkValue, 0));   //By Use ItemKey
                //    vfrmMstItm.MdiParent = frmMain.gfrmMain;
                //    vfrmMstItm.Show();

                //    break;
                //case GEnum.SystemCode.Inventory_Opening_Balance:
                //    break;
                //case GEnum.SystemCode.Category:
                //    frmREFCat vRefCat = new frmREFCat(GFunc.NEInt(HyperLinkValue, 0));   //CatKey
                //    vRefCat.MdiParent = frmMain.gfrmMain;
                //    vRefCat.Show();
                //    break;
                //case GEnum.SystemCode.Brand:
                //    frmREFBrand vRefBrand = new frmREFBrand(GFunc.NEInt(HyperLinkValue, 0));   //BrandKey
                //    vRefBrand.MdiParent = frmMain.gfrmMain;
                //    vRefBrand.Show();
                //    break;
                //case GEnum.SystemCode.UOM:
                //    frmREFUOM vRefUOM = new frmREFUOM(GFunc.NEInt(HyperLinkValue, 0));   //UOMKey
                //    vRefUOM.MdiParent = frmMain.gfrmMain;
                //    vRefUOM.Show();
                //    break;
                //case GEnum.SystemCode.Color:
                //    frmREFColor vRefColor = new frmREFColor(GFunc.NEInt(HyperLinkValue, 0));   //ColorKey
                //    vRefColor.MdiParent = frmMain.gfrmMain;
                //    vRefColor.Show();
                //    break;
                //case GEnum.SystemCode.Scale:
                //    frmREFScale vRefScale = new frmREFScale(GFunc.NEInt(HyperLinkValue, 0));   //ScaleKey
                //    vRefScale.MdiParent = frmMain.gfrmMain;
                //    vRefScale.Show();
                //    break;
                //case GEnum.SystemCode.Location:
                //    frmREFLoc vRefLocation = new frmREFLoc(GFunc.NEInt(HyperLinkValue, 0));   //LocKey
                //    vRefLocation.MdiParent = frmMain.gfrmMain;
                //    vRefLocation.Show();
                //    break;
                //case GEnum.SystemCode.Stock_Count:
                //    break;
                //case GEnum.SystemCode.Job:
                //    frmMSTJob vfrmJob = new frmMSTJob(GFunc.NEInt(HyperLinkValue, 0));   //LocKey
                //    vfrmJob.MdiParent = frmMain.gfrmMain;
                //    vfrmJob.Show();
                //    break;
                //case GEnum.SystemCode.Job_Opening_Balance:

                //    break;

                //case GEnum.SystemCode.Job_Cost_Type:
                //    frmREFJobCostType vRefJobCostType = new frmREFJobCostType(HyperLinkValue);   //JobCostTypeID
                //    vRefJobCostType.MdiParent = frmMain.gfrmMain;
                //    vRefJobCostType.Show();
                //    break;
                //case GEnum.SystemCode.Job_Phase:
                //    frmREFJobPhase vRefJobPhase = new frmREFJobPhase(GFunc.NEInt(HyperLinkValue, 0));   //JobPhaseID
                //    vRefJobPhase.MdiParent = frmMain.gfrmMain;
                //    vRefJobPhase.Show();
                //    break;
                //case GEnum.SystemCode.Job_Task:
                //    frmREFJobTask vRefJobTask = new frmREFJobTask(GFunc.NEInt(HyperLinkValue, 0));   //JobTaskID
                //    vRefJobTask.MdiParent = frmMain.gfrmMain;
                //    vRefJobTask.Show();
                //    break;
                //case GEnum.SystemCode.Job_Group:
                //    frmREFJobGrp vRefJobGrp = new frmREFJobGrp(GFunc.NEInt(HyperLinkValue, 0));   //JobGrpID
                //    vRefJobGrp.MdiParent = frmMain.gfrmMain;
                //    vRefJobGrp.Show();
                //    break;
                //case GEnum.SystemCode.Job_Timesheet:
                //    break;
                //case GEnum.SystemCode.Machine_List:
                //    break;
                //case GEnum.SystemCode.Machine_Type_List:
                //    break;
                //case GEnum.SystemCode.Alerts:
                //    break;
                //case GEnum.SystemCode.Alert_Log:
                //    break;
                //case GEnum.SystemCode.To_Do:
                //    break;
                //case GEnum.SystemCode.To_Do_Log:
                //    break;
                //case GEnum.SystemCode.Other_Report_Setting:
                //    break;
                //case GEnum.SystemCode.Report_ID_Format:
                //    break;
                //case GEnum.SystemCode.Report_Set_Rpt_Files:
                //    break;
                //case GEnum.SystemCode.Cash_Flow:
                //    break;
                //case GEnum.SystemCode.Financial_Charge:
                //    break;
                //case GEnum.SystemCode.Interest_Rate:
                //    frmREFInterest vRefInterest = new frmREFInterest(HyperLinkValue);   //IntID
                //    vRefInterest.MdiParent = frmMain.gfrmMain;
                //    vRefInterest.Show();
                //    break;
                //case GEnum.SystemCode.Security_User:
                //    frmSECUser vfrmUser = new frmSECUser(GFunc.NEInt(HyperLinkValue, 0));   //LocKey
                //    vfrmUser.MdiParent = frmMain.gfrmMain;
                //    vfrmUser.Show();
                //    break;
                //case GEnum.SystemCode.Security_Group:
                //    frmSECGroup vfrmUserGrp = new frmSECGroup(GFunc.NEInt(HyperLinkValue, 0));   //LocKey
                //    vfrmUserGrp.MdiParent = frmMain.gfrmMain;
                //    vfrmUserGrp.Show();
                //    break;
                //case GEnum.SystemCode.Security_ChangePassword:
                //    break;
                //case GEnum.SystemCode.Uploaded_Document:
                //    break;
                //case GEnum.SystemCode.Import_Data:
                //    break;
                //case GEnum.SystemCode.Message_List:
                //    break;
                //case GEnum.SystemCode.RecordAccess:
                //    break;

            }
        }

        private void cSVTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //For CSV
            DiskFileDestinationOptions dfdOptions;
            ExportOptions exOptions;

            SaveFileDialog sfDialog = new SaveFileDialog();
            sfDialog.Filter = "Comma Separated Value files (*.csv)|*.csv";
            sfDialog.Title = "Export Report";
            sfDialog.FileName = Path.GetFileName(RptDocument.FileName).Split('.')[0];
            DialogResult drResult = sfDialog.ShowDialog();

            if (drResult == DialogResult.OK)
            {
                dfdOptions = new DiskFileDestinationOptions();
                exOptions = this.RptDocument.ExportOptions;
                exOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                exOptions.FormatOptions = null;
                exOptions.ExportFormatType = ExportFormatType.TabSeperatedText;

                dfdOptions.DiskFileName = sfDialog.FileName;
                exOptions.DestinationOptions = dfdOptions;

                this.RptDocument.Export();

                if (File.Exists(sfDialog.FileName))
                {
                    StringBuilder sb=new StringBuilder();
                    sb.Append(File.ReadAllText(sfDialog.FileName));
                    sb=sb.Replace("\"", "");
                    sb = sb.Replace("\r\n", "\t\r\n");
                    File.WriteAllText(sfDialog.FileName, sb.ToString());
                }
            }
        }

        private void cSVDelimitedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //For CSV
            DiskFileDestinationOptions dfdOptions;
            ExportOptions exOptions;

            SaveFileDialog sfDialog = new SaveFileDialog();
            sfDialog.Filter = "CSV for Tagetik (*.csv)|*.csv";
            sfDialog.Title = "Export Report";
            sfDialog.FileName = Path.GetFileName(RptDocument.FileName).Split('.')[0];
            DialogResult drResult = sfDialog.ShowDialog();

            if (drResult == DialogResult.OK)
            {
                dfdOptions = new DiskFileDestinationOptions();
                exOptions = this.RptDocument.ExportOptions;
                exOptions.ExportDestinationType = ExportDestinationType.DiskFile;
               // exOptions.FormatOptions = null;
                exOptions.ExportFormatType = ExportFormatType.CharacterSeparatedValues;

                CharacterSeparatedValuesFormatOptions csvOptions = new CharacterSeparatedValuesFormatOptions();
                csvOptions.Delimiter = "|";
                csvOptions.SeparatorText = "";
                exOptions.ExportFormatOptions = csvOptions;

                dfdOptions.DiskFileName = sfDialog.FileName;
                exOptions.DestinationOptions = dfdOptions;

                this.RptDocument.Export();

                if (File.Exists(sfDialog.FileName))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(File.ReadAllText(sfDialog.FileName));
                    sb = sb.Replace("\"", "");
                    sb = sb.Replace("||", "#");
                    sb = sb.Replace("|", "");
                    sb = sb.Replace("#", "|");
                    File.WriteAllText(sfDialog.FileName, sb.ToString());
                }

                //dfdOptions = new DiskFileDestinationOptions();
                //exOptions = this.RptDocument.ExportOptions;
                //exOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //exOptions.FormatOptions = null;
                //exOptions.ExportFormatType = ExportFormatType.TabSeperatedText;

                //dfdOptions.DiskFileName = sfDialog.FileName;
                //exOptions.DestinationOptions = dfdOptions;

                //this.RptDocument.Export();

                //if (File.Exists(sfDialog.FileName))
                //{
                //    StringBuilder sb = new StringBuilder();
                //    sb.Append(File.ReadAllText(sfDialog.FileName));
                //    sb = sb.Replace("\"", "");
                //    sb = sb.Replace("\t", "|");
                //   // sb = sb.Replace("\r\n", "\t\r\n");
                //    File.WriteAllText(sfDialog.FileName, sb.ToString());
                //}
            }
        }

        private void tsbEmail_Click(object sender, EventArgs e)
        {
            frmEmail f = new frmEmail(reportLoader);
            f.ShowDialog();
            f.Close();
        }

       
    }
}