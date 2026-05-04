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
    public partial class frmItmHisSummary : Form
    {
        //Local Variable(s)
        private int itmKey = 0;
        private string ContextMenuSetting = string.Empty;

        //Initialize
        public frmItmHisSummary()
        {
            InitializeComponent();
        }
        public frmItmHisSummary(int vitmKey)
        {            
            itmKey = vitmKey;
            InitializeComponent();
        }//Completed

        //Form Event
        private void frmItmHisSummary_Load(object sender, EventArgs e)
        {
            try
            {
                GlobalUI.FormGrids_Set(this, 0, true, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0);

                if (GFunc.IsNEZ(itmKey))
                {
                    ItmID.SetValueTrigger(string.Empty, false);
                }
                if (itmKey > 0)
                {
                    MSTItm objMstItm = MSTItm.Get(itmKey);
                    ItmID.SetValueTrigger(objMstItm.ItmID, false);
                    Form_Refresh();
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
        }//Completed
        private void frmDocListDet_KeyDown(object sender, KeyEventArgs e)
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

        //Functions
        public void Reload(int vItmKey)
        {
            try
            {
                itmKey = vItmKey;
                if (itmKey> 0)
                {
                    MSTItm objMstItm = MSTItm.Get(this.itmKey);
                    ItmID.SetValueTrigger(objMstItm.ItmID,false);
                    Form_Refresh();
                }
                else
                    ItmID.SetValueTrigger(string.Empty, false);
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
        private void Form_Refresh()
        {
            //Get Total Qty from APPO,APBL,APDN,APCN,ARSO,ARIV,ARDN,ARCN in MSTItmHis
            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@ItmKey", GFunc.NEInt(itmKey,0)));
            parmList.Add(new SqlParameter("@Option", Convert.ToInt32(2)));     
            
            try
            {
                DataTable dtItemInfo = GFunc.ExecuteProc("ROEnquiry_Get", parmList);
                tagrdItmHisSummary.DataSource = dtItemInfo;
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
