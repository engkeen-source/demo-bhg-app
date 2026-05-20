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
    public partial class frmPrint : Form
    {      
        #region Properties  
        public string PrinterNm { get; set; }
        public int FromPage { get; set; }
        public int ToPage { get; set; }
        public bool Collate { get; set; }
        public int Copies { get; set; }       
        #endregion

        //Constructure
        public frmPrint()
        {
            InitializeComponent();          
        }       

        private void frmPrint_Load(object sender, EventArgs e)
        {
            lblPrinterNm.Text = PrinterNm;
            tanuFromPage.Text = FromPage.ToString();
            tanuToPage.Text = ToPage.ToString();
            CollateCopies.Enabled = false;
        }
        private void frmPrint_KeyDown(object sender, KeyEventArgs e)
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

        private void optAllPages_CheckedChanged(object sender, EventArgs e)
        {
            tanuFromPage.Enabled = false;
            tanuToPage.Enabled = false;
        }

        private void optPageRanges_CheckedChanged(object sender, EventArgs e)
        {
            tanuToPage.Enabled = true;
            tanuFromPage.Enabled = true;
        }

        private void NoOfCopies_ValueChanged(object sender, EventArgs e)
        {
            if (NoOfCopies.Value > 1)
                CollateCopies.Enabled = true;
            else
                CollateCopies.Enabled = false;
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
       
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                int fromPage,toPage,copies;
                 if(Int32.TryParse(tanuFromPage.Text,out fromPage))
                     FromPage=fromPage;
                 if (Int32.TryParse(tanuToPage.Text, out toPage))
                     ToPage = toPage;   
                Collate=CollateCopies.Checked;
                if(Int32.TryParse(NoOfCopies.Value.ToString(),out copies))
                    Copies=copies;
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
     
        private void NoPrinter_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
               
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
    }
}
