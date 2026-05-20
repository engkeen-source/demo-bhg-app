using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOLib;
using Infragistics.Win.UltraWinEditors;
using TAUtil;

namespace WinUI
{
    public partial class frmSelectSecurityGrp : Form
    {
        //Variables
        private int secGrpKeyToExclude;
        private int secGrpKey;
        private string secGrpID;
        private bool removeItm;
        private DataTable dtVendor;
        private string ContextMenuSetting = string.Empty;

        //Properties
        public int SelectedSecGrpKey
        {
            get { return secGrpKey; }
            set { secGrpKey = value; }
        }
        public string SecGrpID
        {
            get { return secGrpID; }
            set { secGrpID = value; }
        }
        public bool RemoveItem
        {
            get { return removeItm; }
            set { removeItm = value; }
        }

        //Initialization
        public frmSelectSecurityGrp()
        {
            InitializeComponent();
        }
        public frmSelectSecurityGrp(int SecGrpKeyToExclude)
        {
            //Call From frmFavouriteReportSetting
            secGrpKeyToExclude = SecGrpKeyToExclude;
            InitializeComponent();
        }

        //Form Events
        private void frmSelectVendor_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //This Form process is copy the reports authority which are under a group to another group
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0, this.Name);
                GlobalUI.Combos_Fill(this, 0);

                DataTable dt = SecGrp.DataSource as DataTable;
                dt.PrimaryKey = new DataColumn[] { dt.Columns["GrpKey"] };
                dt.DefaultView.Sort = "GrpKey";
                dt.Rows.Remove(dt.Rows[dt.DefaultView.Find(secGrpKeyToExclude) - 1]);   //Remove Current Selected of the frmFavouriteReportSetting mean remove the source group 
                SecGrp.SelectedRow = SecGrp.Rows[0];
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

        //Button Event
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (!GFunc.IsNEZ(SecGrp.Value))
                {
                    secGrpKey = GFunc.NEInt(SecGrp.Value, 0);
                    secGrpID = SecGrp.Text;
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MsgBox.Show("You must select the security group");
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
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }//CodeCompleted

        //Control Event
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
