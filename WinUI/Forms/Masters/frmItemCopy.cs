using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using BOLib;
using TAUtil;

namespace WinUI
{
    public partial class frmItemCopy : Form
    {
        #region Local Variables

        private DataTable selectedItemList;
        private string msgID = string.Empty;
        private DataTable dtItemList = null;
        string ContextMenuSetting = string.Empty;

        #endregion

        //Properties
        public DataTable SelectedItemList
        {
            get
            {
                return this.selectedItemList;
            }
            set
            {
                this.selectedItemList = value;
            }
        }

        //Initialise
        public frmItemCopy()
        {
            InitializeComponent();
        }        

        // Form Events
        private void frmItemCopy_Load(object sender, System.EventArgs e)
        {
            try
            {
                GlobalUI.FormGrids_Set(this, (int)GEnum.SystemCode.Item_Copy, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)GEnum.SystemCode.Item_Copy);

                this.selectedItemList = (tagrdItemList.DataSource as DataTable).Clone();

                this.FormLayout();
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
        private void frmItemCopy_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }
        private void frmItemCopy_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Set Focus Next Control
                GlobalUI.SelectNextControl(this, e);
            }
            catch (TAException ex)
            {
                //Error(ex,true);
                MsgBox.Show(ex.MsgID);
            }
            catch (Exception ex)
            {
                //Error(ex,true);
                MsgBox.Show(ex.Message);
            }
        }

        //Button Click Events
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void tsbCopy_Click(object sender, EventArgs e)
        {
            try
            {
                this.CopyData();
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
        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                this.Refresh_GridList();            
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
        private void tsbSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                this.SelectAll();
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
        private void tsbSelectNone_Click(object sender, EventArgs e)
        {
            try
            {
                this.SelectNone();
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
        
        //Functions
        private void CopyData()
        {
            try
            {
                this.SelectedItemList.Clear();
                this.tagrdItemList.UpdateData();
                foreach (DataRow row in (this.tagrdItemList.DataSource as DataTable).Rows)
                {
                    if ((bool)row["Selected"] == true)
                    {
                        this.SelectedItemList.Rows.Add(row.ItemArray);
                    }
                }
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
        private void FormLayout()
        {
            try
            {
                GlobalUI.ControlReadOnly_Set(tagrdItemList, true);
                tagrdItemList.DisplayLayout.Bands[0].Columns["Selected"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
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
        private void Refresh_GridList()
        {
            this.tagrdItemList.DataBind();
        }       
        private void SelectAll()
        {
            try
            {
                foreach (DataRow row in (this.tagrdItemList.DataSource as DataTable).Rows)
                {
                    row["Selected"] = true;

                }
                (this.tagrdItemList.DataSource as DataTable).AcceptChanges();
                this.Refresh_GridList();
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
        private void SelectNone()
        {
            try
            {
                foreach (DataRow row in (this.tagrdItemList.DataSource as DataTable).Rows)
                {
                    row["Selected"] = false ;

                }
                (this.tagrdItemList.DataSource as DataTable).AcceptChanges();
                this.Refresh_GridList();
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
        

        #region Error
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
        #endregion

       
        
    }
}
