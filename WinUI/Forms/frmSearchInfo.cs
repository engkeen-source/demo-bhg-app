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
    public partial class frmSearchInfo : Form
    {
        private Infragistics.Win.UltraWinGrid.UltraGrid grid=null;
        private string ContextMenuSetting = string.Empty;      
        private string column;
        //Initialize
        public frmSearchInfo()
        {
            InitializeComponent();
        }
        
        private void frmSearchInfo_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.Text = "Search";                
                //Set ContextMenu & Grid Setting                           
                 //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0);
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                GlobalUI.Combos_Fill(this, 0);
                //this.PopulateLookInCombo("");
                cboMatch.ActiveRow = cboMatch.Rows[0];
                cboSearchDirection.ActiveRow = cboSearchDirection.Rows[0];
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
        public void ShowMe(Infragistics.Win.UltraWinGrid.UltraGrid gridToSeach, string columnName,string columnHeader, string findText)
        {
            try
            {
                this.grid = gridToSeach;
                this.column = columnName;

                this.CancelButton = this.btnCancel;
                this.KeyPreview = true;

                //	Repopulate this, in case the search column has changed
                this.PopulateLookInCombo(columnHeader);
                txtFindWhat.SetValueTrigger(findText,false);

                //	Show the form, bring it to the foreground
                this.TopMost = true;
                this.Show();
                this.BringToFront();
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
        
        private void cmdFindNext_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.ProcessSearch();
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
        private void cmdCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void PopulateLookInCombo(string colHeader)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("LookInValue");
                dt.Columns.Add("Look In");
                dt.Rows.Add(this.column,colHeader);
                dt.Rows.Add("All columns", "All columns");

                this.cboLookIn.ValueMember = "LookInValue";
                this.cboLookIn.DisplayMember = "Look In";
                this.cboLookIn.DataSource = dt;

                this.cboLookIn.DisplayLayout.Bands[0].Columns[0].Hidden = true;
                this.cboLookIn.SelectedRow = this.cboLookIn.Rows[0];
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
        private void ProcessSearch()
        {
            try
            {
                //   Set the demo form's SearchInfo properties
                GlobalUI.SearchInfo objSearchInfo = new GlobalUI.SearchInfo();
                objSearchInfo.searchString = this.txtFindWhat.Text;                                
                objSearchInfo.searchDirection = (GlobalUI.SearchDirectionEnum)this.cboSearchDirection.Value;                
                objSearchInfo.searchContent = (GlobalUI.SearchContentEnum)this.cboMatch.Value;
                objSearchInfo.matchCase = this.chkMatchCase.Checked;
                objSearchInfo.lookIn =GFunc.NEStr(this.cboLookIn.Value,"");
                objSearchInfo.gridToSeach = this.grid;
                             
                GlobalUI.Search(objSearchInfo);
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


        #region Error
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

        #endregion

        private void frmSearchInfo_KeyDown(object sender, KeyEventArgs e)
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

        private void frmSearchInfo_FormClosing(object sender, FormClosingEventArgs e)
        {           
           
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
    }
}
