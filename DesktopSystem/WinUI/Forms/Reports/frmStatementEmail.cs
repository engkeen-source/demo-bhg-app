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
using System.Transactions;
using TAUtil;

namespace WinUI
{
    public partial class frmStatementEmail : Form
    {
        
        DataTable _dtCustomers;
        private string ContextMenuSetting = string.Empty;
        public DataTable dtCustEmail;
        public bool DisplayInOutlook = false;
        public string subject = "";
        public string msg = "";
        public string ccs = "";
        //Initialize
        public frmStatementEmail()
        {
            InitializeComponent();
        }//Completed

        public frmStatementEmail(DataTable dtCust)
        {
            //Call From Mstjob when click btnSend_Click
            InitializeComponent();
            
            _dtCustomers = dtCust;
          
        }//Completed

        public frmStatementEmail(DataTable dtCust,string subject,string msg,string ccs)
        {
            //Call From Mstjob when click btnSend_Click
            InitializeComponent();

            _dtCustomers = dtCust;
            Subject.Text = subject;
            Message.Text = msg;
            tatxtCC.Text = ccs;

        }//Completed

        //Form Event
        private void frmStatementEmail_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0, this.Name);
                RefreshData();
              
                Message.Multiline = true;
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
        private void frmStatementEmail_KeyDown(object sender, KeyEventArgs e)
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
        }//Completed

        //Button Events
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }//Completed

        private void RefreshData()
        {
            // Preapre parameter list (Require parameter at least @MsgID)
            try
            {
                List<SqlParameter> parmList = new List<SqlParameter>();

                string xmlSelectedCustomer = GFunc.ConvertDataTableToXML(_dtCustomers);
                parmList.Add(new SqlParameter("@xmlSelectedCustomer", xmlSelectedCustomer));

                dtCustEmail = GFunc.ExecuteProc("MST_ConGetStaEmail", parmList);
                tagrdCustEmail.DataSource = dtCustEmail;
                tagrdCustEmail.Refresh();
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

        private void SetData()
        {
            if (tagrdCustEmail.ActiveCell != null)
                tagrdCustEmail.UpdateData();
            this.subject = Subject.Text;
            this.msg = Message.Text;
            this.ccs = tatxtCC.Text;
        }

        private void btnOpenInOutlook_Click(object sender, EventArgs e)
        {
            SetData();
            DisplayInOutlook = true;
            this.DialogResult = DialogResult.OK;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                SetData();
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
