using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using BOLib;
using Infragistics.Win.UltraWinEditors;
using TAUtil;

namespace WinUI
{
    public partial class frmFilter : Form
    {

        private int opr=0;//1- >;2- >=;3- <;4- <=;5 -Between
        private string value1 = string.Empty;
        private string value2 = string.Empty;
        string ContextMenuSetting = string.Empty;

        public string Val1
        {
            get { return value1; }
            set { value1=value  ; }
        }

        public string Val2
        {
            get { return value2; }
            set {  value2=value; }
        }

        public int SelectedOperator
        {
            get { return opr; }
            set { value = opr; }
        }


        public frmFilter()
        {
            InitializeComponent();
        }

        private void frmFilter_Load(object sender, EventArgs e)
        {
            try
            {
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0);
                GlobalUI.Combos_Fill(this, 0);            
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
                if (GFunc.IsNE(Operator.Value) == false)
                {
                    if (GFunc.NEInt(Operator.Value, 0) == 5)
                    {
                        if (Value1.Text == string.Empty && Value2.Text == string.Empty)
                            return;
                    }
                    else if (Value1.Text == string.Empty)
                        return;

                }
                else
                {
                    return;
                }

                if (!GFunc.IsNE(Operator.Value))
                {
                    opr = (int)Operator.Value;
                    value1 = Value1.Text;
                    value2 = Value2.Text;
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void Operator_CustomUpdate(object sender, CancelEventArgs e)
        {        
            if (GFunc.NEInt(Operator.Value,0) != 5)//Between
            {
                Value2.Enabled = false;
            }
            else
                Value2.Enabled = true;
            
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

    }
}
