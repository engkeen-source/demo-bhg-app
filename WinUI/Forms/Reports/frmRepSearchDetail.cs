using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using BOLib;
using Infragistics.Win.UltraWinEditors;
using System.Transactions;
using TAUtil;

namespace WinUI
{    
    public enum DetailType
    {
        ItmPO=1,        
        ItmSO=2,
        ConPO = 3,
        ConSO = 4,       
        ItmPS=5,
        ItmRO = 6,
        ItmPODate = 7,
        ItmSODate = 8,
        ItmPSDate = 9,        
        ItmRODate=10
    }

    public partial class frmRepSearchDetail : Form
    {
        private int DocCodeKey = 0;
        int ItmConKey = 0;        
        DetailType Type=DetailType.ItmPO;
       
        string ContextMenuSetting = string.Empty;
        
        //Initialize
        public frmRepSearchDetail(int Key,DetailType type,DateTime date)
        {
            InitializeComponent();
            this.ItmConKey = Key;
            this.Type = type;
            DateAvailable.SetValueTrigger(date, false);
        }

        public void Reload(int Key, DetailType type, DateTime date)
        {
            this.ItmConKey = Key;
            this.Type = type;
            DateAvailable.SetValueTrigger(date, false);
            RefreshData();
        }

        //Form
        private void frmRepSearchDetail_Load(object sender, EventArgs e)
        {
            try
            {                
                this.DocCodeKey=(this.Type==DetailType.ItmPO|| this.Type == DetailType.ItmPODate) ?(int)GEnum.SystemCode.Vendor:(int)GEnum.SystemCode.Customer;

                RefreshData();
                if ((int)this.Type < 7)
                {
                    pnlDateFilter.Visible = false;
                    tagrdTrans.Top = 6;
                }
                else
                {
                    pnlDateFilter.Visible = true;
                    tagrdTrans.Top = 46;
                }
                if (this.Type == DetailType.ItmPS || this.Type == DetailType.ItmPSDate)
                    label16.Text = "Shipped Date <=";
                else 
                    label16.Text = "Promised Date <=";
                //Setup FORM control/grid format, menu, listID
                GlobalUI.FormGrids_Set(this,DocCodeKey, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(DocCodeKey);
                GlobalUI.Combos_Fill(this, DocCodeKey);
                tagrdTrans.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
                tagrdTrans.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
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


        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    
        private void RefreshData()
        {           
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@SearchType", (int)this.Type));

            if(this.Type==DetailType.ConPO || this.Type==DetailType.ConSO)
                parmList.Add(new SqlParameter("@ConKey", this.ItmConKey));
            else
                parmList.Add(new SqlParameter("@ItmKey", this.ItmConKey));

            DataTable dt = GFunc.ExecuteProc("Rep_SearchDetailItmCon", parmList);

            if ((int)this.Type > 6)
                dt.DefaultView.RowFilter = "ItmPrmDate<='" + DateAvailable.DateValue + "'";
            else
                dt.DefaultView.RowFilter = "";

            tagrdTrans.DataSource = dt;
            tagrdTrans.Refresh();
        }

       
        private void tagrdItms_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Bands[0].Summaries.Clear();
            e.Layout.Bands[0].Summaries.Add(Infragistics.Win.UltraWinGrid.SummaryType.Sum, e.Layout.Bands[0].Columns["ItmQty"]);

            e.Layout.Bands[0].Summaries[0].DisplayFormat = "Total = {0:#,##0.00}";
         
            e.Layout.Bands[0].Summaries[0].Appearance.BackColor = Color.White;         
            e.Layout.Bands[0].Summaries[0].Appearance.BorderColor = Color.Ivory;
        
            e.Layout.Bands[0].Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Override.SummaryFooterAppearance.BackColor = Color.White;

            e.Layout.Bands[0].Summaries[0].Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            e.Layout.Bands[0].Summaries[0].Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            e.Layout.Bands[0].Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Override.SummaryFooterAppearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
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

        private void tagrdTrans_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            int docCodeKey = (int)e.Row.Cells["DocCodeKey"].Value;
            int docKey = (int)e.Row.Cells["DocKey"].Value;
            GlobalUI.OpenDocument(docCodeKey, docKey);
        }
    }
}
