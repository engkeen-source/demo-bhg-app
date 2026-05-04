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
using System.Data.SqlClient;
using Infragistics.Win.UltraWinGrid;
using TAUtil;

namespace WinUI
{
    public partial class frmDocSelection : Form
    {   
        #region Declaration
        internal int CallerDC;
        internal int DocCodeKey;
        internal DataTable dtSelectedDocNums;
        private string ContextMenuSetting = string.Empty;
        internal int ConKey;
        #endregion

        //Initialize
        public frmDocSelection()
        {
            InitializeComponent();
        }//Completed
        public frmDocSelection(int DocCodeKey,int DocConKey)
        {
            InitializeComponent();
            CallerDC = DocCodeKey;
            ConKey = DocConKey;
        }//Completed
        
        //Forms Events
        private void frmDocSelection_Load(object sender, EventArgs e)
        {
            try
            {
                //Default Values
                FromDate.DateValue = DateTime.Today.AddMonths(-3);
                ToDate.DateValue = DateTime.Today;

                //Format all grids and filter
                GlobalUI.FormGrids_Set(this, CallerDC, out ContextMenuSetting);

                //Set ContextMenu & Grid Setting                           
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(CallerDC, this.Name);
                GlobalUI.Combos_Fill(this, CallerDC);

                //Layout
                tagrdDocNums.DisplayLayout.Bands[0].Override.AllowAddNew = AllowAddNew.No;
                tagrdDocNums.DisplayLayout.Bands[0].Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                tagrdDocNums.DisplayLayout.Bands[0].Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
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
        private void frmDocSelection_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
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
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                tagrdDocNums.PerformAction(UltraGridAction.ExitEditMode);
                tagrdDocNums.UpdateData();

                #region Validation
                if (tagrdDocNums.Selected.Rows.Count < 1)
                {
                    MsgBox.Show("You must select at least one document from the list.");
                    return;
                }

                if (GFunc.IsNEZ(DocCode.Value))
                {
                    MsgBox.Show("You must select a Document Code.");
                    return;
                }
                #endregion

                //Assign Data and value to public variables so that the caller can access these information.
                DocCodeKey = (int)DocCode.Value;
                dtSelectedDocNums = (tagrdDocNums.DataSource as DataTable).Copy();
                dtSelectedDocNums.Clear();

                foreach (UltraGridRow row in tagrdDocNums.Selected.Rows)
                {                    
                    DataRow dr = ((DataRowView)row.ListObject).Row;
                    dtSelectedDocNums.Rows.Add(dr.ItemArray);
                }
                
                this.DialogResult = DialogResult.OK;
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
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }//Completed
        private void btnRequery_Click(object sender, EventArgs e)
        {
            try
            {
                FillGrid();
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

        //Control Events
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
        }//Completed
        
        //Functions
        private void FillGrid()
        {
            try
            {
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@docCodeKey", GFunc.NEInt(DocCode.Value,0)));
                parmList.Add(new SqlParameter("@fromDate", FromDate.DateValue));
                parmList.Add(new SqlParameter("@toDate", ToDate.DateValue));
                parmList.Add(new SqlParameter("@ConKey", ConKey));
                SqlConnection cn = new SqlConnection(Database.BossDemoConnection);
                cn.Open();
                tagrdDocNums.DataSource = GFunc.ExecuteProc(cn, "DocID_Get", parmList);
                tagrdDocNums.ActiveRow = null;
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
                this.Cursor = Cursors.Default;
            }
        }//Completed

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
