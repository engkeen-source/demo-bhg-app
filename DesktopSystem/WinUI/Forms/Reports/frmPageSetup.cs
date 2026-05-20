using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Management;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Globalization;
using BOLib;
using Infragistics.Win.UltraWinEditors;
using TAUtil;
using CrystalDecisions.CrystalReports.Engine;

namespace WinUI
{
    public partial class frmPageSetup : Form
    {
        #region Variables              

       // DataDynamics.ActiveReports.ActiveReport Report_Source;//keep Report file to get parameter and pagesetting
        ReportDocument Report_Source;       

        PrintDialog pDlg = new PrintDialog();
        //DataTable dtPaperSize;
        //DataTable dtPaperSouce;

        //For Print Document Dialog Box
        [DllImport("winspool.Drv", EntryPoint = "DocumentPropertiesW", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

        static extern int DocumentProperties(IntPtr hwnd, IntPtr hPrinter,
        [MarshalAs(UnmanagedType.LPWStr)] string pDeviceName,
        IntPtr pDevModeOutput, IntPtr pDevModeInput, int fMode);

        [DllImport("kernel32.dll")]
        static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        static extern bool GlobalFree(IntPtr hMem);
        //End For Print Document Dialog Box
        #endregion      

        //Constructure
        public frmPageSetup()
        {
            InitializeComponent();
        }
     
        public frmPageSetup(ReportDocument Parm_ReportSource)
        {
            InitializeComponent();
            Report_Source = Parm_ReportSource;           
        }

        //Form Events
        private void frmPageSetup_Load(object sender, EventArgs e)
        {
            try
            {
                //get all installed printer from current computer
                GetAllPrinter();
                SetMargins();
               
                if (Report_Source.PrintOptions.PrinterName != "")
                {                    
                    Report_Source.PrintOptions.CopyTo(pDlg.PrinterSettings, pDlg.PrinterSettings.DefaultPageSettings);

                    PrinterNm.SetValueTrigger(pDlg.PrinterSettings.PrinterName, false);
                    SetPrinterPaperSizesAndPaperSource(true);

                    //Set Default Printer Setting And Page Setting
                    SetDefaultData();    
                }
                else
                    NoPrinter.Checked = true;                        
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

        private void SetMargins()
        {
            //Setting the UOM
            DataTable dt = new DataTable();
            dt.Columns.Add("ValueCol", typeof(int));
            dt.Columns.Add("Measured Unit", typeof(string));
            dt.Rows.Add(0, "Centimeters");
            //  dt.Rows.Add(1, "Inches");
            MarginIn.DataSource = dt;
            MarginIn.ValueMember = "ValueCol";
            MarginIn.ValueMember = "Measured Unit";

            if (SysOptionUtility.GetBool("Print Margin measurement in cm"))
                MarginIn.SetValueTrigger("Centimeters", false);
            //else
            //    MarginIn.SetValueTrigger("Inches", false);
            
            //Setting the values
            TopMarginValue.Value = System.Math.Round(this.Report_Source.PrintOptions.PageMargins.topMargin / 567M, 2);
            LeftMarginValue.Value = System.Math.Round(this.Report_Source.PrintOptions.PageMargins.leftMargin / 567M, 2);
            RightMarginValue.Value = System.Math.Round(this.Report_Source.PrintOptions.PageMargins.rightMargin / 567M, 2);
            BottomMarginValue.Value = System.Math.Round(this.Report_Source.PrintOptions.PageMargins.bottomMargin / 567M, 2);

        }
        private void frmPageSetup_KeyDown(object sender, KeyEventArgs e)
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

        // Control Events
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
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
        private void btnProperties_Click(object sender, EventArgs e)
        {
            try
            {
                OpenPrinterDocumentDialog();
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
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.Report_Source.PrintOptions.PrinterName = pDlg.PrinterSettings.PrinterName;
                this.Report_Source.ReportClientDocument.PrintOutputController.ModifyPrinterName(pDlg.PrinterSettings.PrinterName);
                try
                {
                    this.Report_Source.PrintOptions.CopyFrom(pDlg.PrinterSettings, pDlg.PrinterSettings.DefaultPageSettings);
                }
                catch (Exception pex)
                {
                    Error(pex, false);
                    this.Report_Source.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)pDlg.PrinterSettings.DefaultPageSettings.PaperSize.Kind;
                    this.Report_Source.PrintOptions.PaperSource = (CrystalDecisions.Shared.PaperSource)pDlg.PrinterSettings.DefaultPageSettings.PaperSource.Kind;
                }
                if(this.Report_Source.PrintOptions.PrinterName!=pDlg.PrinterSettings.PrinterName)
                    this.Report_Source.PrintOptions.PrinterName = pDlg.PrinterSettings.PrinterName;
               
                if (pDlg.PrinterSettings.DefaultPageSettings.PaperSize.Kind == PaperKind.Custom)
                    this.Report_Source.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)pDlg.PrinterSettings.DefaultPageSettings.PaperSize.RawKind;
                if (pDlg.PrinterSettings.DefaultPageSettings.PaperSource.Kind == PaperSourceKind.Custom)
                    this.Report_Source.PrintOptions.PaperSource = (CrystalDecisions.Shared.PaperSource)pDlg.PrinterSettings.DefaultPageSettings.PaperSource.RawKind;


                CrystalDecisions.Shared.PageMargins pg = new CrystalDecisions.Shared.PageMargins(this.Report_Source.PrintOptions.PageMargins.leftMargin, this.Report_Source.PrintOptions.PageMargins.topMargin, this.Report_Source.PrintOptions.PageMargins.rightMargin, this.Report_Source.PrintOptions.PageMargins.bottomMargin);
                this.Report_Source.PrintOptions.ApplyPageMargins(pg);
                pg.leftMargin = Convert.ToInt32(Convert.ToDecimal(LeftMarginValue.Text) * 567M);
                pg.topMargin = Convert.ToInt32(Convert.ToDecimal(TopMarginValue.Text) * 567M);
                pg.rightMargin = Convert.ToInt32(Convert.ToDecimal(RightMarginValue.Text) * 567M);
                pg.bottomMargin = Convert.ToInt32(Convert.ToDecimal(BottomMarginValue.Text) * 567M);

                try
                {                   
                    this.Report_Source.PrintOptions.ApplyPageMargins(pg);                    
                }
                catch (Exception ex)
                {
                  //  this.Report_Source.ReportClientDocument.PrintOutputController.ModifyPageMargins(pg.leftMargin, pg.rightMargin, pg.topMargin, pg.bottomMargin);
                }
               

                this.DialogResult = DialogResult.OK;
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
        private void btnSetDefult_Click(object sender, EventArgs e)
        {
            try
            {
                SetDefaultData();
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
        private void NoPrinter_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                PrinterNm.Enabled = !NoPrinter.Checked;
                btnProperties.Enabled = !NoPrinter.Checked;
                PaperSource.Enabled = !NoPrinter.Checked;
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
        private void PrinterName_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                pDlg.PrinterSettings.PrinterName = PrinterNm.Text;
                ChangesPrinter();
                SetPrinterPaperSizesAndPaperSource(false);
                SetDefaultData();
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
        private void PaperSize_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {               
                pDlg.PrinterSettings.DefaultPageSettings.PaperSize =(PaperSize)PaperSize.Value;
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
        private void PaperSource_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                pDlg.PrinterSettings.DefaultPageSettings.PaperSource= (PaperSource)PaperSource.Value;
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

        //Methods
        private void GetAllPrinter()
        {
            //this function used to get all installed printer from current computer
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PrinterName");
                foreach (string name in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    dt.Rows.Add(name);
                }
                PrinterNm.DataSource = dt;

                PrinterNm.DisplayLayout.Bands[0].ColHeadersVisible = false;

                if (PrinterNm.DisplayLayout.Bands[0].Columns["PrinterName"].Width < PrinterNm.Width)//Added by May. To make the width of column same as combo box if there is only one column and the column width is less than combo width
                    PrinterNm.DisplayLayout.Bands[0].Columns["PrinterName"].Width = PrinterNm.Width;

                PrinterNm.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
                PrinterNm.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            }
            catch (TAException tex)
            {
                throw Error(tex, false );
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }
      
        private void SetPrinterPaperSizesAndPaperSource(bool GetPapersizeFromRpt)
        {
            try
            {
                System.Drawing.Printing.PaperSize pz=null;
                System.Drawing.Printing.PaperSource ps = null;
                //Add Paper Size
                DataTable dtPaperSize = new DataTable();
                dtPaperSize.Columns.Add("ValueCol",typeof(PaperSize));
                dtPaperSize.Columns.Add("PaperSize",typeof(string));
                int i = 0;
                bool found = false;
                foreach (System.Drawing.Printing.PaperSize item in pDlg.PrinterSettings.PaperSizes)
                {
                    dtPaperSize.Rows.Add(item, item.PaperName);

                    if (GetPapersizeFromRpt && !found)
                    {                        
                        if ((int)item.RawKind == (int)Report_Source.PrintOptions.PaperSize)
                        {
                            pz = item;
                            found = true;                            
                        }
                    }
                }

                if (found)
                {
                    pDlg.PrinterSettings.DefaultPageSettings.PaperSize = pz;
                }
                else
                    pDlg.PrinterSettings.DefaultPageSettings.PaperSize = (PaperSize)dtPaperSize.Rows[0]["ValueCol"];


                PaperSize.DataSource = dtPaperSize;
                PaperSize.ValueMember = "ValueCol";
                PaperSize.DisplayMember = "PaperSize";               
                               
                //Add Paper Source
                DataTable dtPaperSouce = new DataTable();
                dtPaperSouce.Columns.Add("ValueCol",typeof(PaperSource));
                dtPaperSouce.Columns.Add("PaperSource",typeof(string));
                i = 0;
                found = false;
                foreach (System.Drawing.Printing.PaperSource item in pDlg.PrinterSettings.PaperSources)
                {
                    dtPaperSouce.Rows.Add(item, item.SourceName);
                    if (GetPapersizeFromRpt && !found)
                    {
                        if ((int)item.RawKind == (int)Report_Source.PrintOptions.PaperSource)
                        {
                            ps = item;
                            found = true;
                        }
                    }
                }
                PaperSource.DataSource = dtPaperSouce;
                PaperSource.ValueMember = "ValueCol";
                PaperSource.DisplayMember = "PaperSource";
                if (found)
                {                   
                    pDlg.PrinterSettings.DefaultPageSettings.PaperSource = ps;
                }
                else if (dtPaperSouce.Rows.Count > 0)
                {
                    pDlg.PrinterSettings.DefaultPageSettings.PaperSource = (PaperSource)dtPaperSouce.Rows[0]["ValueCol"];
                }

                PaperSize.DisplayLayout.Bands[0].ColHeadersVisible = false;
                PaperSize.DisplayLayout.Bands[0].Columns[0].Hidden = true;
                PaperSize.DisplayLayout.Bands[0].Columns[1].Width = PaperSize.Width;
                PaperSize.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
                PaperSize.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;

                PaperSource.DisplayLayout.Bands[0].ColHeadersVisible = false;
                PaperSource.DisplayLayout.Bands[0].Columns[0].Hidden = true;
                PaperSource.DisplayLayout.Bands[0].Columns[1].Width = PaperSource.Width;
                PaperSource.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
                PaperSource.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;

                Landscape.Checked = pDlg.PrinterSettings.DefaultPageSettings.Landscape;

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
        private void SetDefaultData()
        {
            try
            {   
                if (!NoPrinter.Checked)
                {
                    PaperSize.SetValueTrigger(pDlg.PrinterSettings.DefaultPageSettings.PaperSize, false);
                    PaperSource.SetValueTrigger(pDlg.PrinterSettings.DefaultPageSettings.PaperSource, false);
                }

                Landscape.Checked = pDlg.PrinterSettings.DefaultPageSettings.Landscape;   
               
                ChangesPrinter();
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
        private void ChangesPrinter()
        {
            try
            {
                ManagementClass Management = new ManagementClass("Win32_Printer");
                ManagementObjectCollection ManagementObject = Management.GetInstances();

                foreach (ManagementObject mo in ManagementObject)
                {
                    if (mo != null)
                    {
                        string DeviceName = "";
                        PropertyDataCollection ac = mo.Properties;

                        DeviceName = (ac["Caption"].Value != null) ? ac["Caption"].Value.ToString() : "";

                        if (DeviceName == PrinterNm.Text)
                        {
                            //Set Printer Status, Type, Loacation and Comment
                            StatusValue.Text = (ac["Status"].Value != null) ? ac["Status"].Value.ToString() : "";        //Status
                            TypeValue.Text = (ac["DriverName"].Value != null) ? ac["DriverName"].Value.ToString() : "";  //Type
                            WhereValue.Text = (ac["PortName"].Value != null) ? ac["PortName"].Value.ToString() : "";     //Where
                            CommentValue.Text = (ac["Comment"].Value != null) ? ac["Comment"].Value.ToString() : "";     //Comment

                            break;
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
        private void OpenPrinterDocumentDialog()
        {
            IntPtr hDevMode = new IntPtr();
            IntPtr pDevMode = new IntPtr();
            IntPtr devModeData = new IntPtr();
            try
            {  
                hDevMode = pDlg.PrinterSettings.GetHdevmode();
                pDevMode = GlobalLock(hDevMode);

                // If this parameter is zero, the DocumentProperties function returns the number of bytes required by the printer driver's DEVMODE data structure.
                Int32 fMode = 0;

                //int sizeNeeded = DocumentProperties(this.Handle, IntPtr.Zero, pDlg.PrinterSettings.PrinterName, pDevMode, pDevMode, fMode);
                int sizeNeeded = 7824;

                //Get pointer for ptinter like C++
                devModeData = Marshal.AllocHGlobal(sizeNeeded);

                // Output value. The function writes the printer driver's current print settings, 
                // including private data, to the DEVMODE data structure specified by the pDevModeOutput parameter.
                fMode = 14;//DM_OUT_BUFFER;

                int OKCancel = DocumentProperties(this.Handle, IntPtr.Zero, PrinterNm.Text, devModeData, pDevMode, fMode);

                if (OKCancel == 1)
                {
                    pDlg.PrinterSettings.SetHdevmode(devModeData);
                    pDlg.PrinterSettings.DefaultPageSettings.SetHdevmode(devModeData);

                    SetPrinterProperties();
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
                GlobalUnlock(hDevMode);
                GlobalFree(hDevMode);
                Marshal.FreeHGlobal(devModeData);
            }
        }
        private void SetPrinterProperties()
        {
            try
            {
                System.Drawing.Printing.PaperSize pz = (PaperSize)pDlg.PrinterSettings.DefaultPageSettings.PaperSize;
                System.Drawing.Printing.PaperSource ps = (PaperSource)pDlg.PrinterSettings.DefaultPageSettings.PaperSource;

                PaperSize.SetValueTrigger(pz, false);
                PaperSource.SetValueTrigger(ps, false);

                Landscape.Checked = pDlg.PrinterSettings.DefaultPageSettings.Landscape;                       
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
       
        // Error Methods
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

        private void Portrait_CheckedChanged(object sender, EventArgs e)
        {
            pDlg.PrinterSettings.DefaultPageSettings.Landscape = Landscape.Checked;
        }

        private void Landscape_CheckedChanged(object sender, EventArgs e)
        {
            pDlg.PrinterSettings.DefaultPageSettings.Landscape = Landscape.Checked;
        }
    }
}
