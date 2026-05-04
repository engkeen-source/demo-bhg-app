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
using Microsoft.VisualBasic;
using TAUtil;

namespace WinUI.Forms.PoupBrowser
{
    public partial class frmDocListDet : Form
    {
        //Local Variable(s)
        private int docKey = 0;
        GEnum.SystemCode docCodeKey ;
        string ContextMenuSetting = string.Empty;
        private int _DocCodeKey = 0;

        public int DocCodeKey
        {
            get
            {
                return this._DocCodeKey;
            }
        }
       

        //Initialize
        public frmDocListDet()
        {
            InitializeComponent();
        }//Completed
        public frmDocListDet(GEnum.SystemCode DocCodeKey,int DocKey)
        { 
            InitializeComponent();
            docKey = DocKey;
            docCodeKey = DocCodeKey;
            _DocCodeKey = Convert.ToInt32(DocCodeKey);
        }//Completed

        //Form Event
        private void frmDocListDet_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                //Switch for DC that do not have Detail
                switch (docCodeKey)
                {
                    case GEnum.SystemCode.Sales_Adjustment:
                    case GEnum.SystemCode.Cash_Adjustment:
                    case GEnum.SystemCode.Purchase_Adjustment:
                    case GEnum.SystemCode.Cash_Contra:
                    case GEnum.SystemCode.Contra:
                        this.Close();
                        return;
                }

                
                //GlobalUI.cmnuGlobal_Set(this);
                //ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0);
                
                GlobalUI.Combos_Fill(this, 0);
                Form_Refresh();
                GlobalUI.FormGrids_Set(this, DocCodeKey, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(DocCodeKey, this.Name);

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
                Cursor = Cursors.Default;
            }
        }//Completed
        private void frmDocListDet_KeyDown(object sender, KeyEventArgs e)
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

        //Functions
        public void Reload(GEnum.SystemCode DocCodeKey, int DocKey)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                docKey = DocKey;
                this.docCodeKey = DocCodeKey;
                Form_Refresh();
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
                Cursor = Cursors.Default;
            }
        }//Completed
        private void Form_Refresh()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                // Preapre parameter list (Require parameter at least @MsgID)
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@DocKey", docKey));
                parmList.Add(new SqlParameter("@DocCodeKey", docCodeKey));            
            
                DataTable dtItemInfo = GFunc.ExecuteProc("Rep_ItmInfo", parmList);
                tagrdItmInfo.DataSource = dtItemInfo;
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
