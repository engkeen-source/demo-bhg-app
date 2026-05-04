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
using System.Transactions;
using Infragistics.Win.UltraWinEditors;
using TAUtil;

namespace WinUI
{
    public partial class frmAccSelection : Form
    {
        #region Local Variable

        private Document objDoc;
        DataTable dtAccounts;
        int pickSeq = 0;
        string xmlAccount = string.Empty;
        
        private string ContextMenuSetting = string.Empty;
        Boolean formClose = false;


        public string XMLAccount
        {
            get { return xmlAccount; }
            set { xmlAccount = value;}
        }
        #endregion

        #region Initialisze

        public frmAccSelection()
        {
            InitializeComponent();
        }
        public frmAccSelection(Document doc, DataTable dtCaller)
        {
            InitializeComponent();

            this.objDoc = doc;
          
        }
        public frmAccSelection(Document doc, TAUtil.TAGridEditor tagrdDetItms)
        {
            InitializeComponent();
            this.objDoc = doc;
        }

        #endregion

        //Form Events
        private void frmAccSelection_Load(object sender, EventArgs e)
        {
            try
            { 
                //Format all grids and filter
                GlobalUI.FormGrids_Set(this, 0, true, out ContextMenuSetting);


                dtAccounts = tagrdAccount.DataSource as DataTable;
                dtAccounts.Columns.Add("Select", typeof(bool));
                dtAccounts.Columns["Select"].DefaultValue = false;
                tagrdAccount.DataSource = dtAccounts;

                foreach (UltraGridColumn col in tagrdAccount.DisplayLayout.Bands[0].Columns)
                {
                    col.CellActivation = Activation.ActivateOnly;
                }
                tagrdAccount.DisplayLayout.Bands[0].Columns["Select"].CellActivation = Activation.AllowEdit;

                //Bind Combo
                GlobalUI.Combos_Fill(this, 0);

                //Set ContextMenu & Grid Setting                           
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0,this.Name);

                btnUnSelectAll_Click(null, null);
              
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

        //Control Events

        //Button Click Events
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {  

                foreach (UltraGridRow row in tagrdAccount.Rows)
                {
                    row.Selected = true;
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
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            formClose = true;
            this.DialogResult = DialogResult.Cancel;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                tagrdAccount.PerformAction(UltraGridAction.ExitEditMode);
                tagrdAccount.UpdateData();
               
                foreach (UltraGridRow row in tagrdAccount.Rows)
                {
                    if (row.Selected == true)
                        row.Cells["Select"].Value = true;
                }

                if (dtAccounts.AsEnumerable().Where(r => r.Field<bool>("Select") == true).Count() <= 0)
                    return;

                CreateAccountList();  

                this.DialogResult = DialogResult.OK;
                
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
        private void CreateAccountList()
        {
            //dtAccounts.TableName = "dtSelectedAcc";
            //dtAccounts.DefaultView.RowFilter = "[Select] = true";
            IEnumerable<DataRow> dtFiltersToLock = dtAccounts.AsEnumerable().Where(r => r.Field<bool>("Select") == true);

            DataTable dtAccSelected = dtFiltersToLock.Select(r => new
            {
                Key = GFunc.NEInt(r.Field<int>("Key"), 0),
                ID = GFunc.NEStr(r.Field<string>("ID"), ""),
                Des = GFunc.NEStr(r.Field<string>("Des"), ""),
                AccTypeKey = GFunc.NEInt(r.Field<int>("AccTypeKey"), 0),
                AccGrpID = GFunc.NEStr(r.Field<string>("AccGrpID"), ""),
                AccGrpKey = GFunc.NEInt(r.Field<int>("AccGrpKey"), 0),
                AccTypeID = "",
                AccCurrKey = GFunc.NEInt(r.Field<int>("AccCurrKey"), 0),
                AccCurrID = GFunc.NEStr(r.Field<string>("CurrID"), ""),
                AccCurrSym = "",
            }).AsDataTable();

            dtAccSelected.TableName = "dtSelectedAcc";

            if (dtAccSelected.Rows.Count > 0)
                xmlAccount = GFunc.ConvertDataTableToXML(dtAccSelected);
        }
        private void AccountType_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (!GFunc.IsNEZ(AccountType.Value))
                {
                    //GridFilterToDefaultView   
                    ((DataTable)tagrdAccount.DataSource).DefaultView.RowFilter = "AccTypeKey=" + (int)AccountType.Value;
                }
                else
                    ((DataTable)tagrdAccount.DataSource).DefaultView.RowFilter = "";

                    
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

        private void btnUnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (UltraGridRow rows in tagrdAccount.Rows)
            {
                rows.Cells["Select"].Value = false;
            }
            tagrdAccount.UpdateData();
            dtAccounts.AcceptChanges();
        }        

        private void OnDataError(object sender, TAUtil.TAErrorEventArgs e)
        {
            try
            {
                if (e.ErrorCode == TAUtil.TAErrorCode.NOT_IN_LIST)
                {
                    if (sender.GetType() == typeof(TAUtil.TAGridEditor))
                    {
                        TAUtil.TAGridEditor grd = sender as TAUtil.TAGridEditor;
                        if (grd.ActiveCell.Column.EditorComponent != null)
                        {
                            grd.PerformAction(UltraGridAction.EnterEditMode);
                            GlobalUI.ItemNotInList(grd.ActiveCell, null, 0);// ItemNotInList


                        }
                    }
                }
                else if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_NUMERIC)
                {
                    throw new TAException(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
                }

                else if (e.ErrorCode == TAUtil.TAErrorCode.INTEGER_EXCEED_LIMIT)
                {
                    throw new TAException(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
                }
                else if (e.ErrorCode == TAUtil.TAErrorCode.FORMULA_NOT_RECOGNIZE)
                {
                    throw new TAException("FORMULA NOT RECOGNIZE");
                }
                else if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_DATE)
                {
                    if (formClose)
                        throw new TAException("Please enter valid date.");
                    else
                        MsgBox.Show("Please enter valid date.");
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
        }

        private void frmAccSelection_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                    GlobalUI.Combos_Fill(this, 0);

                //Set Focus Next Control
                GlobalUI.SelectNextControl(this, e);
            }
            catch (TAException ex)
            {
                Error(ex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }

        }//CodeCompleted
        

       
    }
}
