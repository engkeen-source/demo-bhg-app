using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using BOLib;
using System.Data.SqlClient;
using System.Reflection;
using TAUtil;


namespace WinUI
{
    public partial class frmDocSearch : Form
    {
        //Local Variables
        string ContextMenuSetting = string.Empty;
        private int _DocCodeKey;
        private DateTime _DocDate;
        DataTable dt = null;
        DateTime? CallerFromDate = null;
        DateTime? CallerToDate = null;
        int _DocKey;
        string _DocID;
        int DocConKey;
        int DocCurrKey;
        string DocCurrID;

        //Properties
        public int DocKey
        {
            get { return _DocKey; }
            set { _DocKey = value; }
        }
        public string DocID
        {
            get { return _DocID; }
            set { _DocID = value; }
        }
        public int DocCodeKey 
        { 
            get { return _DocCodeKey; } 
            set { _DocCodeKey = value; } 
        }
        public DateTime DocDate
        {
            get { return _DocDate; }
            set { _DocDate = value; }
        }
      
        public GVar.ListEvent_OpenRecord ListEvent_OpenRecord = null;
        public TAUtil.TAGridEditor popGrd = null;

        //Initialize
        public frmDocSearch()
        {
            InitializeComponent();
        }
        public frmDocSearch(int CodeKey)
        {
            InitializeComponent();
            _DocCodeKey = (int)CodeKey;
        }//Completed
        public frmDocSearch(int CodeKey, DateTime pDateFrom, DateTime pDateTo,int ConKey, int CurrKey)
        {
            //Call from AppyIV only 
            InitializeComponent();
            _DocCodeKey = (int)CodeKey;
            CallerFromDate = pDateFrom;   
            CallerToDate = pDateTo;       
            DocConKey = ConKey;
            DocCurrKey = CurrKey;
            DocCurrID = REFCurr.Get(CurrKey).CurrID;
        }  //Completed

        //Form Events
        private void frmDocSearch_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                FromDate.DateValue = GFunc.NEDateTime(CallerFromDate, DateTime.Parse(DateTime.Today.AddMonths(-1).ToString()));
                ToDate.DateValue = GFunc.NEDateTime(CallerToDate, DateTime.Parse(DateTime.Today.ToString()));

                GlobalUI.FormGrids_Set(this, _DocCodeKey, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(_DocCodeKey);
                GlobalUI.Combos_Fill(this, _DocCodeKey);

                
                if (DocConKey > 0 && DocCurrID.Length > 0)
                {
                    //For AR/AP Credit Note Apply search
                    SearchType.Visible = false;
                    SearchText.Visible = false;
                    lblSearchType.Visible = false;
                    lblSearchText.Visible = false;
                    ((DataTable)tagrdPopups.DataSource).DefaultView.RowFilter = "DocConKey=" + DocConKey+" AND CurrID='"+DocCurrID+"'";
                }
                else
                {
                    //for normal Document Search
                    switch (_DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Inventory_Adjustment:
                        case (int)GEnum.SystemCode.Inventory_Production:
                        case (int)GEnum.SystemCode.Inventory_Transfer:
                        case (int)GEnum.SystemCode.Journal:
                        case (int)GEnum.SystemCode.Deposit:
                        case (int)GEnum.SystemCode.Bank_Revaluation:
                            SearchType.SetValueTrigger(GEnum.DocSearchType.Document_Ref, false);
                            SearchType.Enabled = false;
                            break;

                        case (int)GEnum.SystemCode.AR_Cash_Opening_Balance:
                        case (int)GEnum.SystemCode.AR_Opening_Balance:
                        case (int)GEnum.SystemCode.AP_Opening_Balance:
                            SearchType.SetValueTrigger(GEnum.DocSearchType.Cust_Vend_Name, false);
                            SearchType.Enabled = false;
                            break;

                        default:
                            SearchType.SetValueTrigger(GEnum.DocSearchType.Cust_Vend_Name, false);
                            break;
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
        private void frmDocSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, _DocCodeKey);
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
     
        //Button Click Events
        private void tsbClose_Click(object sender, EventArgs e)
        {            
            this.Close();
        }//Completed
        private void tsbSelect_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (GFunc.IsNE(tagrdPopups.ActiveRow) == false)
                {
                    _DocCodeKey = GFunc.NEInt(tagrdPopups.ActiveRow.Cells["DocCodeKey"].Value, 0);
                    _DocKey = GFunc.NEInt(tagrdPopups.ActiveRow.Cells["DocKey"].Value, 0);
                    _DocID = GFunc.NEStr(tagrdPopups.ActiveRow.Cells["DocID"].Value, "");
                    _DocDate = GFunc.NEDateTime(tagrdPopups.ActiveRow.Cells["DocDate"].Value, null);
                    if (this.ListEvent_OpenRecord != null)
                        this.ListEvent_OpenRecord.Invoke(_DocKey);
                    this.DialogResult = DialogResult.OK;
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

        //Control Event
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
        private void FromDate_CustomUpdate(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                Form_Refresh();
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
        private void ToDate_CustomUpdate(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                Form_Refresh();
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
        private void SearchType_CustomUpdate(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                SearchData();
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
        private void SearchText_TextChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                SearchData();
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
        
        //Grid Common Events
        private void grdPopups_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                tsbSelect.PerformClick();
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
   
        //Form Function
        private void SearchData()
        {
            try
            {
                string searchCol = string.Empty;
                if (GFunc.IsNE(SearchType.Value))
                {
                    ((DataTable)tagrdPopups.DataSource).DefaultView.RowFilter = "";
                    SearchText.SetValueTrigger(string.Empty, false);
                    return;
                }
                
                switch (GFunc.NEInt(SearchType.Value, 0))
                {
                    case (int)GEnum.DocSearchType.Cust_Vend_Name:
                        searchCol = "DocConNm LIKE '%" + SearchText.Text + "%'";
                        break;
                    case (int)GEnum.DocSearchType.Ship_Name:
                        searchCol = "DocShipName LIKE '%" + SearchText.Text + "%'";
                        break;
                    case (int)GEnum.DocSearchType.Ship_Mark:
                        searchCol = "DocShipMark LIKE '%" + SearchText.Text + "%'";
                        break;
                    case (int)GEnum.DocSearchType.Document_Ref:
                        searchCol = "DocRef LIKE '%" + SearchText.Text + "%'";
                        break;

                    default:
                        ((DataTable)tagrdPopups.DataSource).DefaultView.RowFilter = "";
                        SearchText.SetValueTrigger(string.Empty, false);
                        return;
                }
                ((DataTable)tagrdPopups.DataSource).DefaultView.RowFilter = searchCol;
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
        private void Form_Refresh()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                string listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "0GF", tagrdPopups.Name);
                GlobalUI.Grid_Format(tagrdPopups, listID, true, false);

                if (DocConKey > 0 && DocCurrID.Length > 0)
                {
                    ((DataTable)tagrdPopups.DataSource).DefaultView.RowFilter = "DocConKey=" + DocConKey + " AND CurrID='" + DocCurrID + "'";
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

