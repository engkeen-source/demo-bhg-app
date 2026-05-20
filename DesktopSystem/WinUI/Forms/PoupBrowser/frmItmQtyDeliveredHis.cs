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
using Infragistics.Win;
using System.Collections;
using TAUtil;


namespace WinUI
{
    public partial class frmItmQtyDeliveredHis : Form
    {
        #region Local Variables
        private int docCodeKey;
        private int dockey;
        private int docItmKey;
        private string ContextMenuSetting = string.Empty;
        #endregion

        /// </summary>
        /// 
        /// <param name="customerID">Pass null if no ID</param>
        /// <param name="itemKey">Pass null if no ID</param>

        //Initialize
        public frmItmQtyDeliveredHis()
        {
            InitializeComponent();
        }//Completed
        public frmItmQtyDeliveredHis(int? vdocCodeKey, int? vdocKey, int vdocItmKey)
        {
            InitializeComponent();
            docCodeKey = GFunc.NEInt(vdocCodeKey, 0);
            dockey = GFunc.NEInt(vdocKey,0);
            docItmKey = GFunc.NEInt(vdocItmKey, 0);
        }//Completed

        //Form Events
        private void frmItmQtyDeliveredHis_Load(object sender, EventArgs e)
        {
            try
            {
                //Note: in this form the ListID is not use to set the grid.Datasource in the function GlobalUI.FormGrids_Set()
                //so we need to get the data first before we call the function GlobalUI.FormGrids_Set()

                List<SqlParameter> paraList = new List<SqlParameter>();
                SqlParameter para = new SqlParameter("@DocCodeKey", docCodeKey);
                paraList.Add(para);
                para = new SqlParameter("@DocKey", dockey);
                paraList.Add(para);
                para = new SqlParameter("@DocItmKey", docItmKey);
                paraList.Add(para);
                this.tagrdDocList.DataSource = GFunc.ExecuteProc("Doc_Order_Get", paraList);
                
                //GlobalUI.cmnuGlobal_Set(this);
                GlobalUI.FormGrids_Set(this, docCodeKey, out ContextMenuSetting);
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
        private void frmItmQtyDeliveredHis_KeyDown(object sender, KeyEventArgs e)
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

        //Button Event
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
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
