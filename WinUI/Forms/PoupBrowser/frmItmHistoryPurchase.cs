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
    public partial class frmItmHistoryPurchase : Form
    {
        #region Local Variables
        private int itmKey = 0;
        private int searchType = 10;  //means type of invoice refer to sysmsglist int ItmHisSearchTypeForPurchase 
        private string ContextMenuSetting = string.Empty;
        #endregion

        #region Initialize
        public frmItmHistoryPurchase()
        {
            InitializeComponent();
        }//Completed
        public frmItmHistoryPurchase(int ItmKey)
        {
            try
            {
                InitializeComponent();
                itmKey = ItmKey;
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
        public frmItmHistoryPurchase(int ItmKey, int SearchType)
        {
            //This code is not use but we can keep this for future use
            try
            {
                InitializeComponent();
                itmKey = ItmKey;
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

        }//Completed
        #endregion

        /// <summary>
        /// Reloads the form according to given criteria.
        /// </summary>
        /// <param name="searchType">10:Invoice
        /// 20:Delivery Order 30:Sale Order 40: Quotation</param>
        /// <param name="customerKey">Pass null if no Key</param>
        /// <param name="itemKey">Pass null if no Key</param>        

        //Form Events
        private void frmItmHistory_Load(object sender, EventArgs e)
        {
            try
            {
                
                ControlDefaultValue_Set();
                Refresh_Form();

                GlobalUI.FormGrids_Set(this, 0, true, out ContextMenuSetting);
                GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0);
                GlobalUI.Combos_Fill(this, 0);

                //ItmPriceUser
                if (SECPermUtility.Perform("ItemViewCost", false) == false)
                {
                    string col = "ItmPriceUser";

                    if (tagrdItmHistoryDet.DisplayLayout.Bands[0].Columns.Exists(col))
                    {
                        if (tagrdItmHistoryDet.DisplayLayout.Bands[0].Columns[col].EditorComponent == null)
                            tagrdItmHistoryDet.DisplayLayout.Bands[0].Columns[col].EditorComponent = new TAUtil.TANumericEditor();

                        ((TAUtil.TANumericEditor)tagrdItmHistoryDet.DisplayLayout.Bands[0].Columns[col].EditorComponent).PasswordChar = '*';

                        tagrdItmHistoryDet.DisplayLayout.Bands[0].Columns[col].CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FullEditorDisplay;
                        tagrdItmHistoryDet.DisplayLayout.Bands[0].Columns[col].CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly;

                        tagrdItmHistoryDet.DisplayLayout.Bands[0].Columns[col].ResetCellAppearance();
                    }
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
        private void frmItmHistoryPurchase_KeyDown(object sender, KeyEventArgs e)
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

        //Button Event
        private void btnRequery_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
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
            Cursor = Cursors.WaitCursor;
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
            Cursor = Cursors.WaitCursor;
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
                    ItmDes.SetValueTrigger(objItm.ItmDes, false);
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
                    ItmDes.SetValueTrigger(des, false);

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
                itmKey = GFunc.GetExistingRecKey(ItmDes.Text, GEnum.SystemCode.Inventory, false, true);
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
                    ItmDes.SetValueTrigger(objItm.ItmDes, false);

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

                if (DocHDRUtil.EditorButton_Popup(0, ItmDes.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref itmKey, ref id, ref des))
                {
                    ItmID.SetValueTrigger(id, false);
                    ItmDes.SetValueTrigger(des, false);

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
        private void ConKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int key = GFunc.GetExistingRecKey(ConKey.Text, GEnum.SystemCode.Vendor, true, true);

                if (GFunc.IsNEZ(key))
                {
                    //ConKey_EditorButtonClick(sender, null);
                    if (GFunc.IsNEZ(ConKey.Value))
                    {
                        ConNm.SetValueTrigger("", false);
                        Refresh_Form();
                    }
                }
                else
                {
                    MSTCon objCon = MSTCon.Get(key);
                    ConKey.SetValueTrigger(key, false);
                    ConNm.SetValueTrigger(objCon.ConNm, false);
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
        private void ConKey_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            //Cursor = Cursors.WaitCursor;
            //try
            //{
            //    int key = 0;
            //    string id = string.Empty;
            //    string Name = string.Empty;
            //    string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ((Control)sender).Name);

            //    if (DocHDRUtil.EditorButton_Popup(0, ConKey.Text, listSettingID, (int)GEnum.PopupType.VendID, ref key, ref id, ref Name))
            //    {
            //        ConKey.SetValueTrigger(key, false);
            //        ConNm.SetValueTrigger(Name, false);

            //        Refresh_Form();
            //    }
            //}
            //catch (TAException tex)
            //{
            //    Error(tex, true);
            //}
            //catch (Exception ex)
            //{
            //    Error(ex, true);
            //}
        }//Completed
        private void ConNm_CustomUpdate(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int key = GFunc.GetExistingRecKey(ConNm.Text, GEnum.SystemCode.Vendor, false, true);

                if (GFunc.IsNEZ(key))
                {
                    ConNm_EditorButtonClick(sender, null);
                    if (GFunc.IsNEZ(ConKey.Value))
                        Refresh_Form();
                }
                else
                {
                    MSTCon objCon = MSTCon.Get(key);
                    ConKey.SetValueTrigger(key, false);
                    ConNm.SetValueTrigger(objCon.ConNm, false);
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
        private void ConNm_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int key = 0;
                string id = string.Empty;
                string Name = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ((Control)sender).Name);

                if (DocHDRUtil.EditorButton_Popup(0, ConNm.Text, listSettingID, (int)GEnum.PopupType.VendNm, ref key, ref id, ref Name))
                {
                    ConKey.SetValueTrigger(key, false);
                    ConNm.SetValueTrigger(Name, false);

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
        private void pSearchType_CustomUpdate(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
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

        //Fuction
        private void ControlDefaultValue_Set()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                FromDate.DateValue = GFunc.NEDateTime(FromDate.DateValue, DateTime.Today.AddMonths(-1));
                ToDate.DateValue = GFunc.NEDateTime(ToDate.DateValue, DateTime.Today);
                pSearchType.SetValueTrigger(GFunc.NEInt(searchType,50), false);

                if (GFunc.IsNEZ(itmKey))
                {
                    ItmID.SetValueTrigger(string.Empty, false);
                    ItmDes.SetValueTrigger(string.Empty, false);
                }
                else
                {
                    MSTItm objItm = MSTItm.Get(itmKey);
                    ItmID.SetValueTrigger(objItm.ItmID, false);
                    ItmDes.SetValueTrigger(objItm.ItmDes, false);
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
        public void Reload(int itemKey, bool forceRefresh)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                itmKey = GFunc.NEInt(itemKey, 0);
                //ControlDefaultValue_Set();
                if (GFunc.IsNEZ(itmKey))
                {
                    ItmID.SetValueTrigger(string.Empty, false);
                    ItmDes.SetValueTrigger(string.Empty, false);
                }
                else
                {
                    MSTItm objItm = MSTItm.Get(itmKey);
                    ItmID.SetValueTrigger(objItm.ItmID, false);
                    ItmDes.SetValueTrigger(objItm.ItmDes, false);
                }
                if(forceRefresh)
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
                if (GFunc.IsNEZ(pSearchType.Value))
                    pSearchType.SetValueTrigger(10, false); //value 10 means  type of invoice refer to sysmsglist int ItmHisSearchTypeForPurchase

                if (GFunc.IsNEZ(ConKey.Value))
                    ConKey.SetValueTrigger(0, false);

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
            Cursor = Cursors.WaitCursor;
            try
            {
                CriteriaDefault_Set();

                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@pSearchType", pSearchType.Value));
                parmList.Add(new SqlParameter("@pConKey", ConKey.Value));
                parmList.Add(new SqlParameter("@pDateF", FromDate.DateValue));
                parmList.Add(new SqlParameter("@pDateT", ToDate.DateValue));
                parmList.Add(new SqlParameter("@pItmKey", itmKey));
                parmList.Add(new SqlParameter("@pShipNm", GFunc.NEStr(ShipName.Value, "")));
                parmList.Add(new SqlParameter("@RetVal", 0));
                parmList[6].Direction = ParameterDirection.Output;
                DataTable dtItemHistory = GFunc.ExecuteProc("Rep_ItmHistoryPurchase", parmList);
                tagrdItmHistoryDet.DataSource = dtItemHistory;

                GlobalUI.Grid_Format(tagrdItmHistoryDet, "frmItmHistoryPurchaseGrid", false, true);
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
            switch(((Control)sender).Name.ToLower())
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
