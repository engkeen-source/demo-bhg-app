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
using Infragistics.Win.UltraWinEditors;
using TAUtil;

namespace WinUI
{
    public partial class frmItmPrevPrice : Form
    {
        #region Local Variables
        private int itmKey = 0;
        private int conKey = 0;
        private string searchType = "";  //AR or AP
        private string ContextMenuSetting = string.Empty;
        #endregion

        #region Initialize
        public frmItmPrevPrice()
        {
            InitializeComponent();
        }       
        public frmItmPrevPrice(int ItmKey,int ConKey,string SearchType)
        {
            try
            {
                InitializeComponent();
                itmKey = ItmKey;
                conKey = ConKey;
                searchType = SearchType;
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
        #endregion

        /// <summary>
        /// Reloads the form according to given criteria.
        /// </summary>
        /// <param name="searchType">10:Invoice
        /// 20:Delivery Order 30:Sale Order 40: Quotation</param>
        /// <param name="customerKey">Pass null if no Key</param>
        /// <param name="itemKey">Pass null if no Key</param>        
        
        //Form Events
        private void frmItemHistory_Load(object sender, EventArgs e)
        {
            try
            {
               // GlobalUI.FormGrids_Set(this, 0, true, out ContextMenuSetting);
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0);
                GlobalUI.Combos_Fill(this, 0);

                ControlDefaultValue_Set();
                Refresh_Form();
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
        private void frmItmPrevPrice_KeyDown(object sender, KeyEventArgs e)
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

        //Button Common Click
        private void btnRequery_Click(object sender, EventArgs e)
        {
            try
            {
                Refresh_Form();
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

        //From Controls Events
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
        }//Completed
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
        private void ItmID_CustomUpdate(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                itmKey = GFunc.GetExistingRecKey(ItmID.Text, GEnum.SystemCode.Inventory, true, true);
                if (GFunc.IsNEZ(itmKey))
                {
                    ItmID_EditorButtonClick(sender, null);
                    if (GFunc.IsNEZ(itmKey))
                        Refresh_Form();
                }
                else
                {
                    MSTItm objItm = MSTItm.Get(itmKey);
                    ItmID.SetValueTrigger(objItm.ItmID, false);
                   lblItmDes.Text = objItm.ItmDes;
                    Refresh_Form();
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
        }//Completed
        private void ItmID_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ((Control)sender).Name);

                if (DocHDRUtil.EditorButton_Popup(0, ItmID.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref itmKey, ref id, ref des))
                {
                    ItmID.SetValueTrigger(id, false);
                    lblItmDes.Text = des;

                    Refresh_Form();
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
        }//Completed
        private void ItmDes_CustomUpdate(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                itmKey = GFunc.GetExistingRecKey(lblItmDes.Text, GEnum.SystemCode.Inventory, false, true);
                if (GFunc.IsNEZ(itmKey))
                {
                    ItmDes_EditorButtonClick(sender, null);
                    if (GFunc.IsNEZ(itmKey))
                        Refresh_Form();
                }
                else
                {
                    MSTItm objItm = MSTItm.Get(itmKey);
                    ItmID.SetValueTrigger(objItm.ItmID, false);
                    lblItmDes.Text = objItm.ItmDes;

                    Refresh_Form();
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
        }//Completed
        private void ItmDes_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ((Control)sender).Name);

                if (DocHDRUtil.EditorButton_Popup(0, lblItmDes.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref itmKey, ref id, ref des))
                {
                    ItmID.SetValueTrigger(id, false);
                    lblItmDes.Text = des;

                    Refresh_Form();
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
        }//Completed      

        //Fuction
        private void ControlDefaultValue_Set()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                FromDate.DateValue = GFunc.NEDateTime(FromDate.DateValue, DateTime.Today.AddMonths(-3));
                ToDate.DateValue = GFunc.NEDateTime(ToDate.DateValue, DateTime.Today);               

                if (GFunc.IsNEZ(itmKey))
                {
                    ItmID.SetValueTrigger(string.Empty, false);
                    lblItmDes.Text = "";
                }
                else
                {
                    MSTItm objItm = MSTItm.Get(itmKey);
                    ItmID.SetValueTrigger(objItm.ItmID, false);
                    lblItmDes.Text = objItm.ItmDes;
                }
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
        public void Reload(int itemKey)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                itmKey = GFunc.NEInt(itemKey, 0);
                ControlDefaultValue_Set();
                Refresh_Form();
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
        private void CriteriaDefault_Set()
        {
            try
            {               
                if (GFunc.IsNE(itmKey))
                    itmKey = 0;

                if (GFunc.IsNE(FromDate.DateValue))
                    FromDate.DateValue = DateTime.Today.AddMonths(-1);

                if (GFunc.IsNE(ToDate.DateValue))
                    ToDate.DateValue = DateTime.Today;
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private void Refresh_Form()
        {
            try
            {
                CriteriaDefault_Set();

                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@ItmKey", itmKey));
                parmList.Add(new SqlParameter("@DocConKey", conKey));
                parmList.Add(new SqlParameter("@DocDateFrom", FromDate.DateValue));
                parmList.Add(new SqlParameter("@DocDateTo", ToDate.DateValue));
                parmList.Add(new SqlParameter("@RepType", searchType));

                DataTable dtItemHistory = GFunc.ExecuteProc("Rep_ItmPrevPrice", parmList);
                tagrdItm.DataSource = dtItemHistory;
                GlobalUI.Grid_Format(tagrdItm, "frmItmPrevPriceGrid", false, false);          
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
        private void CustomDataError(object sender, TAUtil.TAErrorEventArgs e)
        {
            switch (((Control)sender).Name.ToLower())
            {
                case "fromdate":
                case "todate":
                    MsgBox.Show("Invalid Date");
                    break;
            }
        }//Completed
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
