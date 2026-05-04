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
    public partial class frmWOSearch : Form
    {
         //Local Variables
        string ContextMenuSetting = string.Empty;
        private int _WOTypeKey;
        private int _WOCodeKey;
        private DateTime _DateIn;
        DataTable dt = null;
        DateTime? CallerFromDate = null;
        DateTime? CallerToDate = null;
        int _WOKey;
        string _WONo;
        int DocConKey;
        int DocCurrKey;
        string DocCurrID;

        //Properties
        public int WorkOrderKey
        {
            get { return _WOKey; }
            set { _WOKey = value; }
        }
        public string WorkOrderNo
        {
            get { return _WONo; }
            set { _WONo = value; }
        }
        public int WorkOrderTypeKey 
        { 
            get { return _WOTypeKey; }
            set { _WOTypeKey = value; } 
        }
        public DateTime DateIn
        {
            get { return _DateIn; }
            set { _DateIn = value; }
        }
        public int WOCodeKey
        {
            get { return _WOCodeKey; }
            set { _WOCodeKey = value; }
        }
      
        public GVar.ListEvent_OpenRecord ListEvent_OpenRecord = null;
        public TAUtil.TAGridEditor popGrd = null;

        //Initialize
        public frmWOSearch()
        {
            InitializeComponent();
        }
        public frmWOSearch(int CodeKey)
        {
            InitializeComponent();
            _WOCodeKey = (int)CodeKey;
        }//Completed
        //public frmWOSearch(int CodeKey, DateTime pDateFrom, DateTime pDateTo, int ConKey, int CurrKey)
        //{
        //    //Call from AppyIV only 
        //    InitializeComponent();
        //    _WOCodeKey = (int)CodeKey;
        //    CallerFromDate = pDateFrom;   
        //    CallerToDate = pDateTo;       
        //    DocConKey = ConKey;
        //    DocCurrKey = CurrKey;
        //    DocCurrID = REFCurr.Get(CurrKey).CurrID;
        //}  //Completed

        //Form Events
        private void frmWOSearch_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                FromDate.DateValue = GFunc.NEDateTime(CallerFromDate, DateTime.Parse(DateTime.Today.AddMonths(-1).ToString()));
                ToDate.DateValue = GFunc.NEDateTime(CallerToDate, DateTime.Parse(DateTime.Today.ToString()));

                GlobalUI.FormGrids_Set(this, _WOCodeKey, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(_WOCodeKey);
                GlobalUI.Combos_Fill(this, _WOCodeKey);

                
                //if (DocConKey > 0 && DocCurrID.Length > 0)
                //{
                //    //For AR/AP Credit Note Apply search
                //    SearchType.Visible = false;
                //    SearchText.Visible = false;
                //    lblSearchType.Visible = false;
                //    lblSearchText.Visible = false;
                //    ((DataTable)tagrdPopups.DataSource).DefaultView.RowFilter = "DocConKey=" + DocConKey+" AND CurrID='"+DocCurrID+"'";
                //}
                //else
                //{
                    
                            SearchType.SetValueTrigger(GEnum.WOSearchType.Work_Order, false);
                //}
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
        private void frmWOSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, _WOCodeKey);
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
                    //_WOCodeKey = GFunc.NEInt(tagrdPopups.ActiveRow.Cells["WorkOrderTypeKey"].Value, 0);
                    _WOKey = GFunc.NEInt(tagrdPopups.ActiveRow.Cells["WorkOrderKey"].Value, 0);
                    _WONo = GFunc.NEStr(tagrdPopups.ActiveRow.Cells["WorkOrderNo"].Value, "");
                    _DateIn = GFunc.NEDateTime(tagrdPopups.ActiveRow.Cells["DateIn"].Value, null);
                    if (this.ListEvent_OpenRecord != null)
                        this.ListEvent_OpenRecord.Invoke(_WOKey);
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
                    case (int)GEnum.WOSearchType.Work_Order:
                        searchCol = "WorkOrderNo LIKE '%" + SearchText.Text + "%'";
                        break;
                    case (int)GEnum.WOSearchType.Work_Order_Type:
                        searchCol = "TypeID LIKE '%" + SearchText.Text + "%'";
                        break;
                    case (int)GEnum.WOSearchType.Status:
                        searchCol = "DataDes LIKE '%" + SearchText.Text + "%'";
                        break;
                    case (int)GEnum.WOSearchType.Vehicle:
                        searchCol = "Vehicle LIKE '%" + SearchText.Text + "%'";
                        break;
                    case (int)GEnum.WOSearchType.Customer_Name:
                        searchCol = "ConNm LIKE '%" + SearchText.Text + "%'";
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

                //if (DocConKey > 0 && DocCurrID.Length > 0)
                //{
                //    ((DataTable)tagrdPopups.DataSource).DefaultView.RowFilter = "DocConKey=" + DocConKey + " AND CurrID='" + DocCurrID + "'";
                //}
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
