using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using BOLib;
using System.Data.SqlClient;
using TAUtil;

namespace WinUI
{
    public partial class frmPrintCond : Form
    {
        //Variables
        private DataTable dtDoc = null;
        private int DocCodeKey = 0;
        string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        private SqlConnection cn = null; 

        //Initialize
        public frmPrintCond()
        {
            InitializeComponent();
        }//Completed
        public frmPrintCond(int docCodeKey, DataTable dtDocList, string title,bool IsWarning)
        {
            InitializeComponent();
            dtDoc = dtDocList;
            DocCodeKey = docCodeKey;
            this.Text = title;
            tslUnprintedMsg.Visible = !IsWarning;
        }//Completed
        public frmPrintCond(SqlConnection cn, int docCodeKey, DataTable dtDocList, string title, bool IsWarning)
        {
            InitializeComponent();
            dtDoc = dtDocList;
            DocCodeKey = docCodeKey;
            this.cn = cn;
            this.Text = title;
            tslUnprintedMsg.Visible = !IsWarning;
        }//Completed

        //Form Events
        private void frmPrintCond_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //Set Grid DataSource
                tagrdList.DataSource = dtDoc;
                tagrdList.Refresh();
               
                //Format grid
                if (this.cn == null)
                {
                    GlobalUI.FormGrids_Set(this, DocCodeKey, out ContextMenuSetting);
                    ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(DocCodeKey);
                }
                else
                {
                    GlobalUI.FormGrids_Set(this.cn, this, DocCodeKey, out ContextMenuSetting);
                    ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(this.cn, DocCodeKey);

                }
                //GlobalUI.cmnuGlobal_Set(this);
                GlobalUI.ControlReadOnly_Set(tagrdList, true);
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
        private void frmPrintCond_Shown(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                // Check Form Close State is True ...
                if (formClose)
                {
                    this.Close();
                }
                else
                {
                    if (this.cn == null)
                    {
                        MsgBox.Show(MsgID.Document.ItemsBelowSaleLimit);
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
            
        }//Completed
        private void frmPrintCond_KeyDown(object sender, KeyEventArgs e)
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
        }//Completed

        //Menu Strip Events
        private void tsbCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        private void tsbOverride_Click(object sender, EventArgs e)
        {
            //Continue the process
            this.DialogResult = DialogResult.OK;
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