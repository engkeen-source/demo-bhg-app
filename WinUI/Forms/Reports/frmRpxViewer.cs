using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using DataDynamics.ActiveReports.Export;
using BOLib;
using System.Diagnostics;
using System.IO;
using DataDynamics.ActiveReports.Design;
using DataDynamics.ActiveReports.Interop;
using TAUtil;
using System.Linq;

namespace WinUI
{
    public partial class frmRpxViewer : Form
    {
        #region Member Variables
        //Variables
        public DataDynamics.ActiveReports.ActiveReport rptDoc; // keep current  Active report file to preview, save page setting and save printer setting
        private DataDynamics.ActiveReports.Design.Designer _Designer = null; //to show active report setting  dialog and keep design unit

        public BOLib.GVar.ListUpdateEvent ListUpdateEvent = null; // raise deligate event to target form for hyperlink click event 
        public BOLib.GVar.UpdateEvent_RefreshReport UpdateRefreshEvent = null;// raise deligate event to frmFinPrint form and reload report viewer  
        public GVar.DocPrintUpdateEvent DocPrinted = null;

        private bool _useFinStatement = false;//true mean this form is used for Finicial Report , false mean used from other report
        private bool dataSourceChanged = false;

        private string _HyperLinkString = "";//to keep hyperlink value from Active report hyperlink control
        private string TemporyExportLocation = Path.GetTempPath(); // keetp the current computer tempory file location
        
        public ReportLoader reportLoder = null;      

        private enum ToolBarButtonID { Single_Page_View = 8, Mutiaple_Page_View = 9, Close = 330, Print_Setup = 331, Doc_Save = 332, Margin_Setting = 333, Export = 334, Allo_Mutiaple_Page = 335, Print=2 };

        private const string _DetailPrifix = "TAhylinkDet_";
        private const string _PageHeaderPrifix = "TAhylinkPageH_";
        private const string _PageFooterPrifix = "TAhylinkPageF_";
        private const string _ReportHeaderPrifix = "TAhylinkReportH_";
        private const string _ReportFooterPrifix = "TAhylinkReportF_";
        private const string _GroupHeaderPrifix = "TAhylinkGroupH_";
        private const string _GroupFooterPrifix = "TAhylinkGroupF_";

        #endregion

        #region Properties
        /// <summary>
        /// Get Or Set The Physical Report File Path.
        /// </summary>
        public string ReportSourceFilePath { get; set; }
        public GEnum.SystemCode DocCodeKey { get; set; }//use in hyperlink and Document Printout
        public int? DocKey { get; set; }
        public int ReportPeriod { get; set; } 
        public bool UseFinStatement { get; set; }
        public string ReportExportFileNameAndPath { get; set; }
        //set report factory object to save Finical report to database
        public BOLib.SYSFinRepFactory FinReportFactory { get; set; }

        #endregion

        //Constructor
        public frmRpxViewer()
        {
            InitializeComponent();
            _Designer = new Designer();
        }
        public frmRpxViewer(string ReportFilePath)
        {
            InitializeComponent();
            ReportSourceFilePath = ReportFilePath;
            _Designer = new Designer();
        }

        //Form Events
        private void frmRpxViewer_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.ReportSourceFilePath))
                    this.Text = "Report [Untitled]";
                else
                    this.Text = "Report [" + Path.GetFileName(this.ReportSourceFilePath) + "]";

                //Add Close Button
                DataDynamics.ActiveReports.Toolbar.Button btnClose = new DataDynamics.ActiveReports.Toolbar.Button();
                btnClose.Caption = "Close";
                btnClose.ToolTip = "Close";
                btnClose.ButtonStyle = DataDynamics.ActiveReports.Toolbar.ButtonStyle.TextAndIcon;
                btnClose.Id = (int)ToolBarButtonID.Close;
                this.arvMain.Toolbar.Tools.Insert(1, btnClose);   

                //Add Setup Button
                DataDynamics.ActiveReports.Toolbar.Button btnSetup = new DataDynamics.ActiveReports.Toolbar.Button();
                btnSetup.Caption = "Print Setting";
                btnSetup.ToolTip = "PrintSetup";
                btnSetup.ButtonStyle = DataDynamics.ActiveReports.Toolbar.ButtonStyle.TextAndIcon;
                btnSetup.Id = (int)ToolBarButtonID.Print_Setup;
                this.arvMain.Toolbar.Tools.Insert(23, btnSetup);

                //Re-allocate Print Button
                arvMain.Toolbar.Tools.ToolById(2).Visible = false;
                DataDynamics.ActiveReports.Toolbar.Tool Pri = arvMain.Toolbar.Tools.ToolById(2);
                DataDynamics.ActiveReports.Toolbar.Button Print = new
                DataDynamics.ActiveReports.Toolbar.Button();
                Print.ButtonStyle = DataDynamics.ActiveReports.Toolbar.ButtonStyle.TextAndIcon;
                Print.Caption = Pri.Caption;
                Print.Id = 45;
                Print.ImageIndex = Pri.ImageIndex;
                Print.Shortcut = Pri.Shortcut;
                Print.Size = Pri.Size;
                Print.ToolTip = Pri.ToolTip;
                arvMain.Toolbar.Tools.Remove(Pri);
                arvMain.Toolbar.Tools.Insert(24, Print);             


                //Add Export Button
                DataDynamics.ActiveReports.Toolbar.Button btnExport = new DataDynamics.ActiveReports.Toolbar.Button();
                btnExport.Caption = "Export";
                btnExport.ToolTip = "Export";
                btnExport.ButtonStyle = DataDynamics.ActiveReports.Toolbar.ButtonStyle.TextAndIcon;
                btnExport.Id = (int)ToolBarButtonID.Export;
                this.arvMain.Toolbar.Tools.Insert(25, btnExport);

                //Add Allow Mutiple View  Button
                DataDynamics.ActiveReports.Toolbar.CheckButton chkMutiple = new DataDynamics.ActiveReports.Toolbar.CheckButton();
                chkMutiple.ButtonStyle = DataDynamics.ActiveReports.Toolbar.ButtonStyle.TextAndIcon;
                chkMutiple.Caption = "Allow Multiple Page View";
                chkMutiple.ToolTip = "Allow Multiple Page View";
                chkMutiple.CheckState = CheckState.Checked;
                chkMutiple.Id = (int)ToolBarButtonID.Allo_Mutiaple_Page;
                this.arvMain.Toolbar.Tools.Insert(26, chkMutiple);               

                    
                //We still keep these code in case we need to use hyperlink to open document/form
                //May commented on 9-Dec-2011
             
                //DOInitializeObject();
                //DOInsertScript();

                //Load Printer And Page Setting
               // If Data Source is null, that is Finicial Report.
                if (UseFinStatement == false)
                {
                    //DataDynamics.ActiveReports.ParameterCollection parmCol = new DataDynamics.ActiveReports.ParameterCollection(this);
                    //parmCol.AddRange(rptDoc.Parameters);

                    //rptDoc = ReportLoader.LoadRpxPageAndPrinterSetting(this.ReportSourceFilePath, rptDoc);
                    //rptDoc.Parameters.Clear();
                    //rptDoc.Parameters.AddRange(parmCol);   
                    GetPrinterInfo();
                    rptDoc.Run();
                    arvMain.ReportViewer.Zoom = 1.05f;
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
        }
        private void frmRpxViewer_Activated(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {

                if (dataSourceChanged)
                {
                    rptDoc.Run(false);
                    dataSourceChanged = false;
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
                Cursor = Cursors.Arrow;
            }
        }
        private void frmRpxViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                rptDoc.Dispose();
                rptDoc = null;
                arvMain.Dispose();
                arvMain = null;
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
        private void tsbClose_Click(object sender, EventArgs e)
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
        }
        private void arvMain_HyperLink(object sender, DataDynamics.ActiveReports.Viewer.HyperLinkEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    _HyperLinkString = e.HyperLink;
                }

                #region Old Codes
                int Key = 0;
                int RepKey = 0;

                if (!BOLib.GFunc.IsNE(reportLoder.ReportName))
                    Int32.TryParse(arvMain.Tag.ToString(), out RepKey);

                switch (RepKey)
                {
                    case 3000:
                        Int32.TryParse(e.HyperLink.ToString(), out Key);
                        if (Key != 0)
                        {
                            frmMSTCon fCon = new frmMSTCon(Key);
                            fCon.MdiParent = frmMain.gfrmMain;
                            // fCon.ListUpdateEvent += new BOLib.GVar.ListUpdateEvent(this.AfterRecordUpdated);
                            fCon.Show();
                        }
                        break;
                }
                #endregion
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
                Cursor = Cursors.Arrow;
            }
        }
        private void arvMain_ToolClick(object sender, DataDynamics.ActiveReports.Toolbar.ToolClickEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                switch (e.Tool.Id)
                {
                    case (int)ToolBarButtonID.Close:   //Close Btn
                        this.Close();
                        break;
                    case (int)ToolBarButtonID.Print_Setup:   //Print Setup
                        ShowPrintSetup();
                        break;                   
                    case (int)ToolBarButtonID.Export:   //Export
                        ShowExportForm();
                        break;
                    case (int)ToolBarButtonID.Allo_Mutiaple_Page:   //Allow Mitiaple Page View
                        SetAllowMtiaplePageViewValue();
                        break;
                    case (int)ToolBarButtonID.Mutiaple_Page_View:   //Mitiaple Page View Button
                        ((DataDynamics.ActiveReports.Toolbar.CheckButton)arvMain.Toolbar.Tools.ToolById((int)ToolBarButtonID.Allo_Mutiaple_Page)).CheckState = CheckState.Checked;
                        ((DataDynamics.ActiveReports.Toolbar.CheckButton)arvMain.Toolbar.Tools.ToolById((int)ToolBarButtonID.Allo_Mutiaple_Page)).Caption = "Allow Multiple Page View";
                        ((DataDynamics.ActiveReports.Toolbar.CheckButton)arvMain.Toolbar.Tools.ToolById((int)ToolBarButtonID.Allo_Mutiaple_Page)).ToolTip = "Allow Multiple Page View";
                        break;
                    case 45: //Print

                        if (GFunc.IsNE(this.rptDoc.Document.Printer.PrinterName))
                        {
                            MsgBox.Show("Please set up the print setting first.");
                            return;
                        }

                      
                        frmPrint f = new frmPrint();
                        f.PrinterNm = this.rptDoc.Document.Printer.PrinterName;
                        f.FromPage = 1;
                        f.ToPage = arvMain.Document.Pages.Count;

                        DialogResult dlgResult = f.ShowDialog();
                        if (dlgResult == DialogResult.OK)
                        {  
                            arvMain.Document.Printer.PrinterSettings.Copies=Convert.ToInt16(f.Copies);
                            arvMain.Document.Printer.PrinterSettings.Collate = f.Collate;
                            arvMain.Document.Printer.PrinterSettings.FromPage = f.FromPage;
                            arvMain.Document.Printer.PrinterSettings.ToPage = f.ToPage;
                            if (arvMain.Document.Printer.PrinterName == "ActiveFax")
                            {
                                arvMain.Document.Name = arvMain.Document.Name + "@F211 " + "65070790" + "@"; //to set fax number for ActiveFax server
                            }

                            arvMain.Document.Print(false, true);

                           if (DocPrinted != null)
                           {
                               DocPrinted.Invoke();
                           }
                        }
                        f.Close();  
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
                Cursor = Cursors.Arrow;
            }
        }
        private void arvMain_DoubleClick(object sender, EventArgs e)
        {
            //Currently this is not used but we may need it in future for hyperlink to open document/form
            Cursor = Cursors.WaitCursor;
            try
            {
                ProcessForHyperLinkEvent(_HyperLinkString);
                _HyperLinkString = "";
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
                Cursor = Cursors.Arrow;
            }
        }

        //Methods
        /// <summary>
        /// Do Initialize Object and some variables. When Form Loading
        /// </summary>
        private void InitializeObject()
        {
            try
            {
                reportLoder = new ReportLoader();
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
        private void RecordUpdated()
        {
            try
            {
                //Currently this is not used but we may need it in future for hyperlink to open document/form
                if (this.ListUpdateEvent != null)
                {
                    this.ListUpdateEvent.Invoke();
                    dataSourceChanged = true;
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
        }
        private void PageSettingUpdated()
        {
            try
            {
                if (UseFinStatement)
                {
                    //Note PageSetting are save in current Fin datatable(Factory) only it is only save to server table when the
                    //user click save on the Fin designer
                    //For FinStatement, the control in the RPX are created on the fly, when the user change the PageSetting, we will
                    //need to get the new pagesetting and close the current viewer instance and recreate a new viewer
                    FinStatement_SavePageSetting(true);

                    if (this.UpdateRefreshEvent != null)
                    {
                        this.UpdateRefreshEvent.Invoke(FinReportFactory, rptDoc.DataSource as DataTable);//create new viewer instance
                        this.Close();//Close current viewer
                        
                        frmMain.gfrmMain.ExistingFinStatementForm(FinReportFactory.CodeKey, FinReportFactory.MSTFinMain.RepKey,this.ReportPeriod);
                    }
                }
                else
                {
                    //Note PageSetting are save in Rpt/Rpx file only when the user choose SAVE menu                   
                    this.rptDoc.Run();
                    this.arvMain.Document = rptDoc.Document;
                    this.arvMain.ReportViewer.RepositionPage = true;
                    this.arvMain.ReportViewer.Refresh();

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
        }
        public void ShowExportForm()
        {
            try
            {
                frmReportExport frmExport = new frmReportExport(UseFinStatement);
                DialogResult res = frmExport.ShowDialog();

                if (res == DialogResult.OK)
                {
                    Export(frmExport.FileType, frmExport.DestinationType);
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
        }
        private void Export(string Parm_FileType, string Parm_DestinationType)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                string FileFilter = "";
                InitializeObject();

                reportLoder.rpxDoc = this.rptDoc;
                reportLoder.ReportName = this.ReportSourceFilePath;

                if (GFunc.CompareString(ReportFileType.AcrobatPDFFile, Parm_FileType))
                {
                    FileFilter = "PDF files (*.pdf)|*.pdf";
                }
                else if (GFunc.CompareString(ReportFileType.HTMLFile, Parm_FileType))
                {
                    FileFilter = "Html files (*.html)|*.html";
                }
                else if (GFunc.CompareString(ReportFileType.ExcelFile, Parm_FileType))
                {
                    FileFilter = "Excel files (*.xls)|*.xls";
                }
                else if (GFunc.CompareString(ReportFileType.ExcelFileDataOnly, Parm_FileType))
                {
                    FileFilter = "Excel files (*.xls)|*.xls";
                }
                else if (GFunc.CompareString(ReportFileType.RichTextFile, Parm_FileType))
                {
                    FileFilter = "Rich Text files (*.rts)|*.rts";
                }
                else if (GFunc.CompareString(ReportFileType.CSVFile, Parm_FileType))
                {
                    FileFilter = "Separated Values (*.csv)|*.csv";
                }
                

                if (UseFinStatement && Parm_FileType == ReportFileType.ExcelFileDataOnly)
                {
                    ExportDataOnlyToExcelForBalanceSheet(Parm_DestinationType);
                }
                else
                {
                    if (Parm_DestinationType.ToLower() == ReportFileDestinationType.Application.ToLower())
                    {
                        reportLoder.PrintFile(Parm_FileType);
                    }
                    else if (Parm_DestinationType.ToLower() == ReportFileDestinationType.DiskFile.ToLower())
                    {
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = FileFilter;
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            reportLoder.CreateReportFile(Parm_FileType, sfd.FileName);
                        }
                    }
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
                Cursor = Cursors.Default;
            }
        }
        /// <summary>
        /// Show Printer Setup Dialog Box For Current Report Doc.
        /// </summary>
        private void ShowPrintSetup()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                frmPageSetupRpx f = new frmPageSetupRpx(rptDoc);
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ReportLoader.SaveRpxPageAndPrinterSetting(this.ReportSourceFilePath, rptDoc);
                  
                    PageSettingUpdated();
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
        /// <summary>
        /// DO Check Allow Mutiaple Page View.
        /// </summary>
        private void SetAllowMtiaplePageViewValue()
        {
            try
            {
                DataDynamics.ActiveReports.Toolbar.CheckButton mitiapleViewCheck = (DataDynamics.ActiveReports.Toolbar.CheckButton)arvMain.Toolbar.Tools.ToolById((int)ToolBarButtonID.Allo_Mutiaple_Page);
                mitiapleViewCheck.CheckState = (mitiapleViewCheck.CheckState != CheckState.Checked) ? CheckState.Checked : CheckState.Unchecked;

                switch (mitiapleViewCheck.CheckState)
                {
                    case CheckState.Checked:    //Allow Mutiaple
                        mitiapleViewCheck.Caption = "Allow Multiple Page View";
                        mitiapleViewCheck.ToolTip = "Allow Multiple Page View";
                        arvMain.ReportViewer.MultiplePageCols = 3;
                        arvMain.ReportViewer.MultiplePageRows = 2;
                        arvMain.ReportViewer.MultiplePageMode = false;
                        break;
                    case CheckState.Unchecked:  //Not Allow Mutiaple
                        mitiapleViewCheck.Caption = "Disallow Multiple Page View";
                        mitiapleViewCheck.ToolTip = "Disallow Multiple Page View";
                        arvMain.ReportViewer.MultiplePageCols = 1;
                        arvMain.ReportViewer.MultiplePageRows = 1;
                        arvMain.ReportViewer.MultiplePageMode = true;
                        break;
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
        }
        /// <summary>
        /// To Get Hidden Field Value At Report, When HyperLink is Click
        /// </summary>
        /// <param name="HyperLinkText">(HyperLinkText : Hidden Field Name Separate By ',')DocKey,DocID,DocItmID,ect</param>
        private void ProcessForHyperLinkEvent(string HyperLinkText)
        {
            //Currently this is not used but we may need it in future for hyperlink to open document/form
            try
            {
                if (HyperLinkText != "" && HyperLinkText.Contains(":"))
                {
                    string vHyperLinkFilter = "";
                    string vHyperLinkValue = "";
                    string[] vHyperLinkTmp = HyperLinkText.Split(new char[] { ',' });

                    foreach (string item in vHyperLinkTmp)
                    {
                        string[] vFieldAndValue = item.Split(new char[] { ':' });
                        if (GFunc.CompareString(vFieldAndValue[0], "DocCodeKey"))
                        {
                            DocCodeKey = (GEnum.SystemCode)GFunc.NEInt(vFieldAndValue[1], 0);
                        }
                        else
                        {
                            vHyperLinkFilter = vFieldAndValue[0].ToString();
                            vHyperLinkValue = vFieldAndValue[1].ToString();
                        }
                    }

                    OpenDocumentFromHyperLinkEvent(vHyperLinkFilter, vHyperLinkValue);
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
        }
        private void OpenDocumentFromHyperLinkEvent(string HyperLink, string HyperLinkValue)
        {
            //currently not use ,We keep this code in case we need to use hyperlink to open document/form
            try
            {
                switch (DocCodeKey)
                {
                    case GEnum.SystemCode.Utility:
                        break;
                    case GEnum.SystemCode.Quotation:
                        frmARQO vfrmARQO = new frmARQO(GEnum.SystemCode.Quotation);
                        vfrmARQO.MdiParent = frmMain.gfrmMain;
                        vfrmARQO.Show();
                        vfrmARQO.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));//Open By DocKey.
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
                    ////case GEnum.SystemCode.Sales_Adjustment:
                    ////    frmARADJ vfrmARAdj = new frmARADJ(GEnum.SystemCode.Sales_Order_Adjustment);
                    ////    vfrmARAdj.MdiParent = frmMain.gfrmMain;
                    ////    vfrmARAdj.Show();
                    ////    vfrmARAdj.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));//Open By DocKey.
                    ////    break;
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
                    //    if (GFunc.CompareString(HyperLink, "DocKey"))
                    //    {
                    //        frmAPADJ frm_APAdj = new frmAPADJ(GEnum.SystemCode.Purchase_Adjustment);
                    //        frm_APAdj.MdiParent = frmMain.gfrmMain;
                    //        frm_APAdj.Show();
                    //        frm_APAdj.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
                    //    }

                    //    break;
                    //case GEnum.SystemCode.Payment_Issue:
                    //    if (GFunc.CompareString(HyperLink, "DocKey"))
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
                    //    if (GFunc.CompareString(HyperLink, "AccKey"))
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
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }
        private void ExportDataOnlyToExcelForBalanceSheet(string Parm_DestinationType)
        {
            //since infratgistic export feature do NOT have export to excel with data only
            //we use a grid to have a work around for this type of export
            //first we assign the activeReport datasource to the grid datasource
            //then we use this grid to export to excel.
            //Note this tagrdFinExport grid is hidden in the viewer form
            try
            {

                DataTable dtFinRepData = rptDoc.DataSource as DataTable;
                DataTable dtCopy = new DataTable();
                dtCopy.Columns.Add("BodyText", typeof(string));

                #region Copy column to DatatTable
                foreach (DataDynamics.ActiveReports.TextBox control in rptDoc.Sections["Detail"].Controls)
                {
                    if (control.Name != "BodyText")
                    {
                        DataColumn dc = null;
                        if (control.OutputFormat != null && control.OutputFormat != string.Empty && control.OutputFormat != "")
                        {
                            dc = new DataColumn(control.DataField, typeof(decimal));
                        }
                        else
                        {
                            dc = new DataColumn(control.DataField, typeof(string));
                        }
                        dc.AllowDBNull = true;
                        dtCopy.Columns.Add(dc);
                    }
                }
                #endregion

                #region Copy Data to DatatTable
                foreach (DataRow drData in dtFinRepData.Rows)
                {
                    DataRow dr = dtCopy.NewRow();
                    foreach (DataColumn dcCopy in dtCopy.Columns)
                    {
                        if (dcCopy.DataType == typeof(decimal))
                        {
                            //to ensure null value is retain
                            if (dr["BodyText"].ToString().Length == 0)
                                dr[dcCopy.ColumnName] = GFunc.NEDec(drData[dcCopy.ColumnName].ToString(), null);
                        }
                        else
                        {
                            dr[dcCopy.ColumnName] = drData[dcCopy.ColumnName];
                        }
                    }
                    dtCopy.Rows.Add(dr);
                }
                dtCopy.AcceptChanges();

                #endregion

                #region rename column name with Active Report Column Header
                foreach (DataDynamics.ActiveReports.ARControl control in rptDoc.Sections[0].Controls)
                {
                    if (control.Name.StartsWith("lblC"))
                    {
                        if (dtCopy.Columns.Contains(control.Name.Replace("lbl", "")))
                        {
                            if (control.GetType() == typeof(DataDynamics.ActiveReports.Label))
                            {
                                dtCopy.Columns[control.Name.Replace("lbl", "")].ColumnName = GFunc.NEStr(((DataDynamics.ActiveReports.Label)control).Text, "BLANK");
                            }
                            else if(control.GetType() == typeof(DataDynamics.ActiveReports.TextBox))
                                dtCopy.Columns[control.Name.Replace("lbl", "")].ColumnName = GFunc.NEStr(((DataDynamics.ActiveReports.TextBox)control).Text, "BLANK");
                        }
                    }
                }

                tagrdFinExport.DataSource = dtCopy;
                if (tagrdFinExport.DisplayLayout.Bands[0].Columns.Exists("BodyText"))
                    tagrdFinExport.DisplayLayout.Bands[0].Columns["BodyText"].Header.Caption = "";
                #endregion

                #region Change Cell Appearance for deimal column
                foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn grdcol in tagrdFinExport.DisplayLayout.Bands[0].Columns)
                {
                    if (grdcol.DataType == typeof(decimal))
                    {
                        grdcol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                    }
                }

                #endregion

                #region Export to excel
                tagrdFinExport.DataBind();

                int exportCount = 0;

                Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter excelExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter();

                //Get tempory file path
                string filePath = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".xls";

                if (Parm_DestinationType.ToLower() == ReportFileDestinationType.DiskFile.ToLower())
                {
                    SaveFileDialog ofDlg = new SaveFileDialog();
                    ofDlg.Filter = "Microsoft Excel (*.xls)|*.xls;";
                    ofDlg.Title = "Export Data";
                    ofDlg.FileName = "ExportFile" + exportCount++.ToString() + ".xls";
                    DialogResult dlg = ofDlg.ShowDialog();
                    if (dlg == DialogResult.OK)
                    {
                        filePath = ofDlg.FileName;
                    }
                    else
                    {
                        filePath = "";
                    }
                }

                if (filePath != "")
                    excelExporter.Export(tagrdFinExport, filePath);

                if (Parm_DestinationType.ToLower() == ReportFileDestinationType.Application.ToLower() && filePath != "")
                    System.Diagnostics.Process.Start(filePath);

                #endregion

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
        /// <summary>
        /// Inser Script For HyperLink Event
        /// </summary>
        /// 
        private void InsertScript()
        {
            //Currently this is not used but we may need it in future for hyperlink feature
            try
            {
                IEnumerator ie = rptDoc.Sections.GetEnumerator();

                while (ie.MoveNext())
                {
                    FindControlAndSetvalue(((DataDynamics.ActiveReports.Section)ie.Current).Type, ((DataDynamics.ActiveReports.Section)ie.Current).Name, ((DataDynamics.ActiveReports.Section)ie.Current).Controls);
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
        }
        /// <summary>
        /// Insert Value To Each Hyperlink Which We set In Report DOC
        /// </summary>
        /// <param name="Parm_SectionType">Header, Detail,Group or Fooder</param>
        /// <param name="Parm_SectionName">Name Of The Section</param>
        /// <param name="Parm_ControlCollection">Section Control Collection</param>
        private void FindControlAndSetvalue(DataDynamics.ActiveReports.SectionType Parm_SectionType, string Parm_SectionName, DataDynamics.ActiveReports.ControlCollection Parm_ControlCollection)
        {
            //currently not use ,We keep this code to in case
            try
            {
                string l_PrefixText = "";
                string l_ControlName = "";
                string l_valueControlName = "";
                string l_Script = "";
                string l_BrforePrint = "_BeforePrint";
                bool l_ValueIsNeedInsert = false;

                IEnumerator ie = Parm_ControlCollection.GetEnumerator();

                switch (Parm_SectionType)
                {
                    case DataDynamics.ActiveReports.SectionType.Detail:
                        l_PrefixText = _DetailPrifix;
                        goto SetValue;
                        break;
                    case DataDynamics.ActiveReports.SectionType.GroupFooter:
                        l_PrefixText = _GroupFooterPrifix;
                        goto SetValue;
                        break;
                    case DataDynamics.ActiveReports.SectionType.GroupHeader:
                        l_PrefixText = _GroupHeaderPrifix;
                        goto SetValue;
                        break;
                    case DataDynamics.ActiveReports.SectionType.PageFooter:
                        l_PrefixText = _PageFooterPrifix;
                        goto SetValue;
                        break;
                    case DataDynamics.ActiveReports.SectionType.PageHeader:
                        l_PrefixText = _PageHeaderPrifix;
                        break;
                    case DataDynamics.ActiveReports.SectionType.ReportFooter:
                        l_PrefixText = _ReportFooterPrifix;
                        goto SetValue;
                        break;
                    case DataDynamics.ActiveReports.SectionType.ReportHeader:
                        l_PrefixText = _ReportHeaderPrifix;
                        goto SetValue;
                        break;
                }

            SetValue:
                l_Script = "public void " + Parm_SectionName + l_BrforePrint + "()\n{\n";
                while (ie.MoveNext())
                {
                    if (((DataDynamics.ActiveReports.ARControl)ie.Current).GetType() == typeof(DataDynamics.ActiveReports.TextBox))
                    {
                        string ControlName = ((DataDynamics.ActiveReports.TextBox)ie.Current).Name;
                        if (ControlName.Contains(l_PrefixText))
                        {
                            if (ControlName[0] == l_PrefixText[0] && ControlName[1] == l_PrefixText[1] && ControlName[2] == l_PrefixText[2])
                            {
                                l_ValueIsNeedInsert = true;
                                int TimEnd = ControlName.IndexOf('_');
                                TimEnd++;
                                l_valueControlName = ControlName.Remove(0, TimEnd);
                                l_Script += ControlName + ".HyperLink = \"" + l_valueControlName + ":\"+" + l_valueControlName + ".Text;";
                            }
                        }
                    }
                }

                if (l_ValueIsNeedInsert)
                {
                    l_Script += "\n}\n";
                    rptDoc.Script += l_Script;
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
        }
      
        private void FinStatement_SavePageSetting(bool SavetoServer)
        {
            //when user change Page margin or printer name SavetoServer=false and it will be save into local data table 
            //because for the Financial report local dataTable need to return back to frmPrint form
            //otherwise SavetoServer=true and save to Database server
            try
            {
                string PageSettingInfo = "";//this.rptDoc.PageSettings.Collate.ToString();
                //PageSettingInfo += (this.rptDoc.PageSettings.DefaultPaperSize != null) ? "/" + this.rptDoc.PageSettings.DefaultPaperSize.ToString() : "/";
                //PageSettingInfo += (this.rptDoc.PageSettings.DefaultPaperSource != null) ? "/" + this.rptDoc.PageSettings.DefaultPaperSource.ToString() : "/";
                //PageSettingInfo += (this.rptDoc.PageSettings.Duplex != null) ? "/" + this.rptDoc.PageSettings.Duplex.ToString() : "/";
                //PageSettingInfo += (this.rptDoc.PageSettings.Gutter != null) ? "/" + this.rptDoc.PageSettings.Gutter.ToString() : "/";
                PageSettingInfo += (this.rptDoc.PageSettings.Margins != null) ? this.rptDoc.PageSettings.Margins.Top.ToString() + "," + this.rptDoc.PageSettings.Margins.Bottom.ToString() + "," + this.rptDoc.PageSettings.Margins.Left.ToString() + "," + this.rptDoc.PageSettings.Margins.Right.ToString() : "/";
                //PageSettingInfo += (this.rptDoc.PageSettings.MirrorMargins != null) ? "/" + this.rptDoc.PageSettings.MirrorMargins.ToString() : "/";
                PageSettingInfo += (this.rptDoc.PageSettings.Orientation != null) ? "/" + this.rptDoc.PageSettings.Orientation.ToString() : "/";
                PageSettingInfo += (this.rptDoc.PageSettings.PaperHeight != null) ? "/" + (this.rptDoc.Document.Printer.DefaultPageSettings.PaperSize.Height/100M).ToString() : "/";
                PageSettingInfo += (this.rptDoc.PageSettings.PaperKind != null) ? "/" + (int)this.rptDoc.PageSettings.PaperKind : "/";
                PageSettingInfo += (this.rptDoc.PageSettings.PaperName != null) ? "/" + this.rptDoc.PageSettings.PaperName : "/";
                PageSettingInfo += (this.rptDoc.PageSettings.PaperSource != null) ? "/" + (int)this.rptDoc.PageSettings.PaperSource : "/";
                PageSettingInfo += (this.rptDoc.PageSettings.PaperSourceName != null) ? "/" + this.rptDoc.PageSettings.PaperSourceName.ToString() : "/";
                PageSettingInfo += (this.rptDoc.PageSettings.PaperWidth != null) ? "/" + (this.rptDoc.Document.Printer.DefaultPageSettings.PaperSize.Width   /100M).ToString() : "/";
                PageSettingInfo += (_Designer.DesignUnits != null) ? "/" + _Designer.DesignUnits.ToString() : "/";
                PageSettingInfo += "/" +this.rptDoc.Document.Printer.PrinterName;

                FinReportFactory.MSTFinMain.MarginBottom = (this.rptDoc.PageSettings.Margins != null) ? GFunc.NEDec(this.rptDoc.PageSettings.Margins.Bottom, .5) : .5M;
                FinReportFactory.MSTFinMain.MarginLeft = (this.rptDoc.PageSettings.Margins != null) ? GFunc.NEDec(this.rptDoc.PageSettings.Margins.Left, .5) : .5M;
                FinReportFactory.MSTFinMain.MarginRight = (this.rptDoc.PageSettings.Margins != null) ? GFunc.NEDec(this.rptDoc.PageSettings.Margins.Right, .5) : .5M;
                FinReportFactory.MSTFinMain.MarginTop = (this.rptDoc.PageSettings.Margins != null) ? GFunc.NEDec(this.rptDoc.PageSettings.Margins.Top, .5) : .5M;

                FinReportFactory.MSTFinMain.PaperSize = PageSettingInfo;


                if (SavetoServer)
                    FinReportFactory.Save();
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
        private void GetPrinterInfo()
        {
            //this function use to get Printer setting except finStatement report,
            //finStatement report will use current computer default printer setting because we use only one physical file, it will be difficult to save each use and each finReport
            try
            {
                if (!UseFinStatement)
                {
                    IEnumerator ie = rptDoc.Parameters.GetEnumerator();
                    while (ie.MoveNext())
                    {
                        if (GFunc.CompareString(ie.Current.ToString(), "PrinterName"))
                        {
                            arvMain.Document.Printer.PrinterName = (rptDoc.Parameters["PrinterName"].DefaultValue != null) ? rptDoc.Parameters["PrinterName"].DefaultValue : "";
                        }
                        else if (GFunc.CompareString(ie.Current.ToString(), "PaperSource"))
                        {
                            string[] strs = rptDoc.Parameters["PaperSource"].DefaultValue.ToString().Split(',');

                            if (strs.Length == 3)
                            {
                                foreach (System.Drawing.Printing.PaperSource item in arvMain.Document.Printer.PrinterSettings.PaperSources)
                                {
                                    if ((int)item.Kind == Convert.ToInt32(strs[0]))
                                    {
                                        if (item.SourceName == strs[2])
                                        {
                                            arvMain.Document.Printer.DefaultPageSettings.PaperSource = item;
                                            rptDoc.PageSettings.PaperSource = item.Kind;
                                            rptDoc.PageSettings.PaperSourceName = item.SourceName;
                                            break;
                                        }
                                    }
                                }
                            }                           
                        }
                        else if (GFunc.CompareString(ie.Current.ToString(), "PaperSize"))
                        {
                            if (rptDoc.Parameters["PaperSize"].DefaultValue != null)
                            {
                                string[] strs = rptDoc.Parameters["PaperSize"].DefaultValue.ToString().Split(',');

                                if (strs.Length == 3)
                                {
                                    foreach (System.Drawing.Printing.PaperSize item in arvMain.Document.Printer.PrinterSettings.PaperSizes)
                                    {
                                        if ((int)item.Kind == Convert.ToInt32(strs[0]))
                                        {
                                            if (item.PaperName == strs[2])
                                            {
                                                arvMain.Document.Printer.DefaultPageSettings.PaperSize = item;
                                                rptDoc.PageSettings.PaperKind = item.Kind;
                                                rptDoc.PageSettings.PaperName = item.PaperName;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (GFunc.CompareString(ie.Current.ToString(), "Landscape"))
                        {
                            arvMain.Document.Printer.DefaultPageSettings.Landscape = (rptDoc.Parameters["Landscape"].DefaultValue != null) ? Convert.ToBoolean(rptDoc.Parameters["Landscape"].DefaultValue) : false;
                        }
                        else if (GFunc.CompareString(ie.Current.ToString(), "Margins"))
                        {
                            if (rptDoc.Parameters["Margins"].DefaultValue != null)
                            {
                                string[] margins = rptDoc.Parameters["Margins"].DefaultValue.ToString().Split(',');
                                if (margins.Length == 4)
                                {
                                    decimal margin = 0;
                                    if (Decimal.TryParse(margins[0], out margin))
                                    {  
                                        rptDoc.Document.Printer.DefaultPageSettings.Margins.Left = Convert.ToInt32(margin*200);
                                        rptDoc.PageSettings.Margins.Left = (float)margin;
                                    }
                                    if (Decimal.TryParse(margins[1], out margin))
                                    {
                                        rptDoc.Document.Printer.DefaultPageSettings.Margins.Top = Convert.ToInt32(margin * 200);
                                        rptDoc.PageSettings.Margins.Top = (float)margin;
                                    }
                                    if (Decimal.TryParse(margins[2], out margin))
                                    {
                                        rptDoc.Document.Printer.DefaultPageSettings.Margins.Right = Convert.ToInt32(margin * 200);
                                        rptDoc.PageSettings.Margins.Right = (float)margin;
                                    }
                                    if (Decimal.TryParse(margins[3], out margin))
                                    {
                                        rptDoc.Document.Printer.DefaultPageSettings.Margins.Bottom = Convert.ToInt32(margin * 200);
                                        rptDoc.PageSettings.Margins.Bottom = (float)margin;
                                    }                                    
                                }
                            }
                        }
                    }
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
        }

        private void SendFax(frmPrint f)
        {
            try
            {
                DataTable dtHdrRows = (from row in reportLoder.ReportDataSource.AsEnumerable()
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
                        string docName = dtHdrRows.Rows[0]["DocID"].ToString().Replace("/", "")
                                              + "@F211 " + dtHdrRows.Rows[0]["DocBAddrFax"] + "@";                       
                        rptDoc.Document.Name = docName;
                        rptDoc.Document.Print(false, false);
                    }
                }
                else
                {
                    foreach (DataRow dr in dtHdrRows.Rows) //Multiple Docs. Need to split
                    {
                        if (GFunc.IsNE(dr["DocBAddrFax"]))
                            continue;

                        DataDynamics.ActiveReports.ActiveReport rpxDoc = ReportLoader.LoadRpxPageAndPrinterSetting(Application.StartupPath + @"\Reports\" + reportLoder._reportFileName, rptDoc);
                        rpxDoc.Document.Printer.PrinterSettings.Copies = 1;

                        reportLoder.ReportDataSource.DefaultView.RowFilter = "DocKey=" + dr["DocKey"];
                        rptDoc.DataSource=reportLoder.ReportDataSource.DefaultView.ToTable();


                        foreach (ReportParameter p in this.reportLoder.ReportParameter)
                        {
                            //if (p.ParameterName == "pFaxNumber")
                            //    rptDoc.SetParameterValue("pFaxNumber", dr["DocBAddrFax"]);
                            //else if (p.ParameterName == "pDocID")
                            //    rptDoc.SetParameterValue("pDocID", dr["DocID"]);
                            //else
                            rptDoc.Parameters[p.ParameterName].Value= p.ParameterValue.ToString();
                        }

                        string docName = dtHdrRows.Rows[0]["DocID"].ToString().Replace("/", "")
                                                + "@F211 " + dtHdrRows.Rows[0]["DocBAddrFax"] + "@";
                        rptDoc.Document.Name = docName;
                        rptDoc.Document.Print(false, false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fax sending failed." + ex.Message);
            }
        }

        //Error Methods
        private Exception Error(Exception ex, bool ShowMessage)
        {
            Exception l_tmpex = ex;
            try
            {

                if (this.ActiveControl.GetType() != typeof(TAUtil.TAGridEditor))
                {
                    l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl });
                }
                else
                {
                    l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, GFunc.GridColumnKey_Get(this.ActiveControl) });
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
                if (this.ActiveControl.GetType() != typeof(TAUtil.TAGridEditor))
                {
                    l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl });
                }
                else
                {
                    l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, GFunc.GridColumnKey_Get(this.ActiveControl) });
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
