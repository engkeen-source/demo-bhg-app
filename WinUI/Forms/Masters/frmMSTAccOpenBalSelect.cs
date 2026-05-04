using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOLib;
using TAUtil;
namespace WinUI
{
    public partial class frmMSTAccOpenBalSelect : Form
    {
        #region Local Variables
        string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        private GEnum.SystemCode systemCode = GEnum.SystemCode.Account_Opening_Balance;
        private string PermID = GVar.PermissionID.Account_Opening_Balance;
        #endregion

        //Initialize
        public frmMSTAccOpenBalSelect(GEnum.SystemCode _systemcode)
        {
            InitializeComponent();
            this.systemCode = _systemcode;
        }//Completed
        public frmMSTAccOpenBalSelect()
        {
            InitializeComponent();
        }//Completed

        //Form Events
        private void frmMSTAccOpenBalSelect_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            bool isReadOnly = false;
            try
            {
                if (SECPermUtility.Any(PermID, out isReadOnly, true) == false) { formClose = true; }

                //Set ContextMenu & Grid Setting & Grid Formatting
                GlobalUI.FormGrids_Set(this, (int)systemCode, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)systemCode);
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
        private void frmMSTAccOpenBalSelect_Shown(object sender, EventArgs e)
        {
            if (formClose)
                this.Close();
        }//Completed
        private void frmMSTAccOpenBalSelect_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)GEnum.SystemCode.Account_Opening_Balance);
                    //CombosDependent_Fill(string.Empty);
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
        private void frmMSTAccOpenBalSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                e.Cancel = false;
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

        //MenuStrip Button Events
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.formClose = true;
            this.Close();
        }//Completed
        private void tsbOpenDepartment_Click(object sender, EventArgs e)
        {
            try
            {
                if (tagrdDepartmentList.ActiveRow == null)
                    throw new Exception("A Department must be selected");

                this.Hide();
                frmMSTAccOpenBal frm = new frmMSTAccOpenBal(this.systemCode, GFunc.NEInt(tagrdDepartmentList.ActiveRow.Cells["DeptKey"].Value, 0), GFunc.NEStr(tagrdDepartmentList.ActiveRow.Cells["DeptNm"].Value, ""));
                frm.MdiParent = frmMain.gfrmMain;
                frm.Show();

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
        }//Completed
        private void tsbOpenCompany_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                frmMSTAccOpenBal frm = new frmMSTAccOpenBal(this.systemCode, 0, string.Empty);
                frm.MdiParent = frmMain.gfrmMain;
                frm.Show();
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
        }//Completed

        //Error
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
                        string ActiveColKey = "";
                        if (((TAUtil.TAGridEditor)this.ActiveControl).ActiveCell != null)
                        {
                            ActiveColKey = GFunc.GridColumnKey_Get(this.ActiveControl);
                        }
                        l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, ActiveColKey });
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
