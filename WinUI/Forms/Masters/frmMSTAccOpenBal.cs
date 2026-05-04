using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOLib;
using System.Transactions;
using System.Data.SqlClient;
using Infragistics.Win.UltraWinGrid;
using TAUtil;
namespace WinUI
{
    public partial class frmMSTAccOpenBal : Form
    {
        #region Local Variables
        private bool formClose = false;
        string ContextMenuSetting = string.Empty;

        private GEnum.SystemCode constCodeKey = GEnum.SystemCode.Account_Opening_Balance;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }
        private string constPermID = GVar.PermissionID.Account_Opening_Balance;
        public string PermID { get { return constPermID; } }

        DataTable dtOpenBal = null;
        private bool IsDirty = false;
        private bool IsReadOnly = false;
        private int _guID = 0;
        private int deptKey = 0;
        private string deptNm = string.Empty;

        //only use to show Read only data viewer for AuditLog 
        private bool IsOpenFromAuditLog = false;
        private DataTable _dtHeader = null;
        private DataSet _dsDetail = null;
        #endregion

        //Initialize
        public frmMSTAccOpenBal()
        {
            InitializeComponent();
        }//Completed
        public frmMSTAccOpenBal(GEnum.SystemCode _systemcode, int _deptKey, string _deptname)
        {
            InitializeComponent();
            this.constCodeKey = _systemcode;
            this.deptKey = _deptKey;
            this.deptNm = _deptname;
        }//Completed
        public frmMSTAccOpenBal(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            //Open from AuditLog as Read only data viewer
            InitializeComponent();
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
            constCodeKey = DocCodeKey;
        }//Completed

        //Form Events
        private void frmMSTAccOpenBal_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //Initialised
                if (Initialisation() == false)
                {
                    formClose = true;
                    return;
                }

                if (this.IsOpenFromAuditLog)
                {
                    if (SetReadOnlyData(_dsDetail) == false)
                    {
                        formClose = true;
                        return;
                    }

                    Refresh_GridDet();
                    GlobalUI.FormEnable_Set(this, false);
                    Calculate();
                }
                else
                {
                    if (LoadAccountOpeningBalance()==false)
                    {
                        formClose = true;
                        return;
                    }
                    Refresh_GridDet();
                    FormLayout();
                    Calculate();
                }

                //Set ContextMenu & Grid Setting & Grid Formatting
                GlobalUI.FormGrids_Set(this, (int)ConstantCodeKey, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)ConstantCodeKey);
                this.IsDirty = false;

                //Setting Form Text
                if (this.deptNm == string.Empty)
                    this.Text = "Chart of Account Opening Balance";
                else
                    this.Text = "Chart of Account Opening Balance for Department " + this.deptNm;

            }
            catch (TAException tex)
            {
                Error(tex,true);
            }
            catch (Exception ex)
            {
                Error(ex,true);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void frmMSTAccOpenBal_Shown(object sender, EventArgs e)
        {
            if (formClose)
                this.Close();
        }//Completed
        private void frmMSTAccOpenBal_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool runProcess = false;
            this.formClose = true;

            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }

           


            try
            {
                #region Closing with Invalid DataType error encountered
                //When the caller performs this.close, the system actually perform validation on all control automatically
                //if there are any control that fails validation (invalid datatype, the e.cancel is set to true, we have no control over this (not sure if this was done by csla)
                //thus we need to check for e.cancel = true so that we can skip the rest of the codes to prevent error message from appearing twice or more
                if (e.Cancel == true)
                    runProcess = true;
                else
                {
                    if (this.SaveChanges() == false)
                    {
                        if (formClose == false)
                        {
                            frmMain.gfrmMain.Tag = GVar.CancelMainFormClosing;                            
                            e.Cancel = true;
                            return;
                        }
                        else
                            runProcess = true;
                    }
                }

                if (runProcess)
                {
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show("Validation Failed, Close the FORM anyway?",
                                         GEnum.MsgBoxIcon.Question,
                                         GEnum.MsgBoxButton.Yes,
                                         GEnum.MsgBoxButton.No);

                    if (btnSelect == GEnum.MsgBoxButton.No)
                    {
                        //to prohibit closing when error occurs even when the form is closed by main form
                        frmMain.gfrmMain.Tag = GVar.CancelMainFormClosing;
                        e.Cancel = true;
                        formClose = false; //(cancel form closing) if there has data when click save changes after close form 
                        return;
                    }
                    else
                    {
                        IsGridsDirty(true);
                        e.Cancel = false;
                    }
                }
                #endregion

                //Remove lock
                SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, ConstantCodeKey, _guID, 0, 0);

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
        private void frmMSTAccOpenBal_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)GEnum.SystemCode.Account_Opening_Balance);
                    //CombosDependent_Fill(string.Empty);
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

        //Menu Stip Events
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }//Completed
        private void tsbSave_Click(object sender, EventArgs e)
        {
            try
            {
                Save_Process();
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

        //Formating, Locking, Refreshing
        private void Refresh_GridDet()
        {
            tagrdAccOpeningBalanceList.DataSource = dtOpenBal;
            tagrdAccOpeningBalanceList.Rows.Refresh(RefreshRow.ReloadData);

        }//Completed
        private void FormLayout()
        {
            try
            {
                bool EnableMode = !this.IsReadOnly;
                this.tslReadOnly.Text = !EnableMode ? "Read Only" : string.Empty;

                this.tsbSave.Enabled = EnableMode;
                this.tagrdAccOpeningBalanceList.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                this.tagrdAccOpeningBalanceList.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;

                if (EnableMode)
                {
                    this.tagrdAccOpeningBalanceList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
                    foreach (UltraGridColumn col in tagrdAccOpeningBalanceList.DisplayLayout.Bands[0].Columns)
                    {
                        switch (col.Key.ToLower())
                        {
                            case "logfc":
                            case "logfd":
                            case "doccurrrate":
                            case "balanceh":
                                col.CellActivation = Activation.AllowEdit;
                                break;

                            default:
                                col.CellActivation = Activation.ActivateOnly;
                                break;
                        }
                    }
                }
                else
                    this.tagrdAccOpeningBalanceList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
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

        //function
        private bool Initialisation()
        {
            try
            {
                if (SECPermUtility.Any(PermID, out this.IsReadOnly, true) == false)
                    return false;

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        //Get GUID Instance
                        //FORM will check for GUID > 0 to indicate Factory is valid
                        if ((this._guID = SysOptionUtility.GetNewLockingGUID(cn)) == 0)
                        {
                            this._guID = -1;
                            return false;
                        }

                        //Locking
                        if (SysLockUtility.IsProcessLock(cn, true, GEnum.SysLockOption.ByCodKey, ConstantCodeKey, this._guID))
                        {
                            this._guID = -1;
                            return false;
                        }

                        //Add Inprogress Lock
                        if (!SysLockUtility.AddInprogressLock(cn, true, this._guID, ConstantCodeKey))
                        {
                            this._guID = -1;
                            return false;
                        }

                        //Commit Process   
                            if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                    }
                }
                return true;
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
        private bool SetReadOnlyData(DataSet dsDetail)
        {
            try
            {
                if (SECPermUtility.Read(PermID, true) == false)
                    return false;

                GFunc.ConvertDataTableToObject(dsDetail.Tables[0], dtOpenBal);

                this.deptNm = _dtHeader.Rows[0]["DeptNm"].ToString();

                //Set Flags
                this.IsDirty = false;
                this.IsReadOnly = true;
                return true;
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
        private bool SaveChanges()
        {
            try
            {
                if (form_CanValidate() == false)
                    return false;

                if (this.IsDirty)
                {
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show(MsgID.Common.SaveChanges,
                                          GEnum.MsgBoxIcon.Question,
                                          GEnum.MsgBoxButton.Save_Changes,
                                          GEnum.MsgBoxButton.Discard_Changes,
                                          GEnum.MsgBoxButton.I_Dont_Know);

                    if (btnSelect == GEnum.MsgBoxButton.Save_Changes)
                        return this.Save_Process();
                    else if (btnSelect == GEnum.MsgBoxButton.I_Dont_Know)
                    {
                        if (formClose)
                            formClose = false;

                        return false;
                    }
                }
                return true;
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
        private bool Save_Process()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //Perform Validation
                if (form_CanValidate() == false)
                    return false;

                #region unbalance check
                decimal _balanceH = 0;
                if (tagrdAccOpeningBalanceList.Rows.Count > 0)
                {
                    foreach (UltraGridRow dr in tagrdAccOpeningBalanceList.Rows)
                    {
                        _balanceH = _balanceH + (GFunc.NEDec(dr.Cells["BalanceH"].Value, 0));
                    }
                }

                if (GFunc.NEDec(_balanceH, 0) != 0)
                {
                    MsgBox.Show("Unbalance opening balance, cannot save");
                    return false;
                }
                #endregion

                #region Saving Data
                if (tagrdAccOpeningBalanceList.Rows.Count > 0)
                {
                    DataTable dt = (tagrdAccOpeningBalanceList.DataSource as DataTable).Copy();
                    dt.TableName = "dtMST_AccOpenBal";
                    string XMLMST_AccOpenBal = GFunc.ConvertDataTableToXML(dt);

                    //Save the Detail Grid
                    List<SqlParameter> parmList = new List<SqlParameter>();
                    parmList.Add(new SqlParameter("@xmlDetail", XMLMST_AccOpenBal));
                    parmList.Add(new SqlParameter("@DocDeptKey", this.deptKey));
                    SqlParameter RetValue = new SqlParameter();
                    RetValue.ParameterName = "@RetValue";
                    RetValue.Value = 0;
                    RetValue.Direction = ParameterDirection.InputOutput;
                    parmList.Add(RetValue);

                    GFunc.ExecuteNonQueryProc("MSTAccOpenBal_Save", parmList);

                    if (GFunc.NEInt(RetValue.Value, 0) == (int)GEnum.SpState.Pass)
                    {
                        MsgBox.Show("Successfully Save");
                        this.IsDirty = false;
                        return true;
                    }
                }
                #endregion
                
                return false;
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
        private bool form_CanValidate()
        {
            try
            {
                //this function check if we can validates all the header.controls
                //all grid is able to commit its changes to its datasource
                TAUtil.ControlGVar.FormValidateFail = false;
                this.Validate();
                this.tagrdAccOpeningBalanceList.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdAccOpeningBalanceList.UpdateData();

                //we need to check if the active row data cannot be commited 
                //if it cannot be commited, the IsGridDirty would return a false
                //thus saving should not be perform and the user needs to be inform of the data error
                if (IsGridsDirty(false) || TAUtil.ControlGVar.FormValidateFail)
                    return false;
                else
                    return true;
            }
            catch (TAException tex)
            {
                Error(tex, false);
                return false;
            }
            catch (Exception ex)
            {
                Error(ex, false);
                return false;
            }

        }//Completed
        private bool IsGridsDirty(bool undoChangesInGrid)
        {
            //This function check if the grid has uncommited data in its active orw
            //it also has an option to undo those uncommited changes. 

            if (tagrdAccOpeningBalanceList.ActiveRow != null)
            {
                if (tagrdAccOpeningBalanceList.ActiveRow.DataChanged && !tagrdAccOpeningBalanceList.ActiveRow.IsUnmodifiedTemplateAddRow)
                {
                    if (undoChangesInGrid)
                    {
                        //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                        this.tagrdAccOpeningBalanceList.PerformAction(UltraGridAction.UndoCell);
                        this.tagrdAccOpeningBalanceList.PerformAction(UltraGridAction.UndoRow);
                    }
                    return true;
                }
            }
            return false;
        }//Completed

        private bool LoadAccountOpeningBalance()
        {
            //Waiting Cursor
            this.Cursor = Cursors.WaitCursor;
            try
            {
                List<SqlParameter> paraList = new List<SqlParameter>();
                paraList.Add(new SqlParameter("@DeptKey", this.deptKey));
                paraList.Add(new SqlParameter("@RetValue", 0));
                paraList[1].Direction = ParameterDirection.Output;
                DataTable dt  = GFunc.ExecuteProc("MSTAccOpenBal_Get", paraList);

                if (GFunc.NEInt(paraList[1].Value, 0) == (int)GEnum.SpState.Pass)
                {
                    dtOpenBal = dt;
                    return true;
                }
                else
                    return false;
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
        private void Calculate()
        {
            decimal TotalLogHD = 0;
            decimal TotalLogHC = 0;
            try
            {
                foreach (UltraGridRow row in tagrdAccOpeningBalanceList.Rows)
                {
                    TotalLogHD += GFunc.NEDec(row.Cells["LogHD"].Value, 0);
                    TotalLogHC += GFunc.NEDec(row.Cells["LogHC"].Value, 0);
                }

                DiffAmount.SetValueTrigger(GFunc.RndC(TotalLogHD - TotalLogHC, GVar.RndDecs.Amtpt), false);
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

        //Grid Events
        private void tagrdAccOpeningBalanceList_CustomDataError(object sender, TAUtil.TAErrorEventArgs e)
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
                            grd.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
                            GlobalUI.ItemNotInList(grd.ActiveCell, null, 0);
                        }
                    }
                }
                if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_NUMERIC)
                {
                    throw new TAException(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
                }
                if (e.ErrorCode == TAUtil.TAErrorCode.FORMULA_NOT_RECOGNIZE)
                {
                    throw new TAException("FORMULA NOT RECOGNIZE");
                }
                else
                {
                    MsgBox.Show(e.ErrorMessage);
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
        private void tagrdAccOpeningBalanceList_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["DocCurrkey"].Value.ToString() == "1")
            {
                e.Row.Cells["DocCurrRate"].Activation = Activation.ActivateOnly;
                e.Row.Cells["BalanceH"].Activation = Activation.ActivateOnly;
            }
            else
            {
                e.Row.Cells["DocCurrRate"].Activation = Activation.AllowEdit;
                e.Row.Cells["BalanceH"].Activation = Activation.AllowEdit;
            }
        }//Completed
        private void tagrdAccOpeningBalanceList_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
                #region Declaration
                UltraGridRow grdRow = tagrdAccOpeningBalanceList.ActiveRow;
                UltraGridCell grdCell = tagrdAccOpeningBalanceList.ActiveCell;
                Decimal LogFD = 0;
                Decimal LogFC = 0;
                Decimal Amt = 0;
                Decimal CurrRate = 1;
                bool runCal = false;
                #endregion

                if (tagrdAccOpeningBalanceList.ActiveCell == null)
                    return;

                switch (grdCell.Column.Key.ToLower())
                {
                    #region DocCurrRate
                    case "doccurrrate":
                        if (grdRow.Cells["DocCurrkey"].Value.ToString() == "1")
                            grdCell.Value = 1;
                        else
                        {
                            CurrRate = Math.Abs(GFunc.RndC(grdCell.Value, GVar.RndDecs.Curpt));
                            if (CurrRate <= 0)
                                grdCell.Value = 1;
                            else
                                grdCell.Value = CurrRate;
                        }
                        runCal = true;
                        break;
                    #endregion

                    #region DebitF, CreditF
                    case "logfd":
                        grdRow.Cells["LogFC"].Value = 0;
                        grdCell.Value = Math.Abs(GFunc.RndC(grdCell.Value, GVar.RndDecs.Amtpt));
                        runCal = true;

                        break;

                    case "logfc":
                        grdRow.Cells["LogFD"].Value = 0;
                        grdCell.Value = Math.Abs(GFunc.RndC(grdCell.Value, GVar.RndDecs.Amtpt));
                        runCal = true;
                        break;
                    #endregion

                    #region BalanceH
                    case "balanceh":
                        Amt = GFunc.RndC(grdCell.Value, GVar.RndDecs.Amtpt);
                        grdCell.Value = Amt;

                        if (Amt < 0)
                        {
                            //Credit
                            grdRow.Cells["LogFD"].Value = 0;
                            grdRow.Cells["logHD"].Value = 0;
                            grdRow.Cells["logHC"].Value = Math.Abs(Amt);

                            if (grdRow.Cells["DocCurrkey"].Value.ToString() == "1")
                            {
                                //Home
                                grdRow.Cells["DocCurrRate"].Value = 1;
                                grdRow.Cells["LogFC"].Value = Math.Abs(Amt);
                            }
                            else
                            {
                                //Foreign
                                if (GFunc.NEDec(grdRow.Cells["LogFC"].Value, 0) == 0)
                                {
                                    grdRow.Cells["DocCurrRate"].Value = 1;
                                    grdRow.Cells["LogFC"].Value = Math.Abs(Amt);
                                }
                                else
                                {
                                    CurrRate = GFunc.RndDC(Amt, GFunc.NEDec(grdRow.Cells["LogFC"].Value, 0), GVar.RndDecs.Curpt);
                                    grdRow.Cells["DocCurrRate"].Value = Math.Abs(CurrRate);
                                }
                            }
                        }
                        else
                        {
                            //Debit
                            grdRow.Cells["LogFC"].Value = 0;
                            grdRow.Cells["logHC"].Value = 0;
                            grdRow.Cells["logHD"].Value = Math.Abs(Amt);

                            if (grdRow.Cells["DocCurrkey"].Value.ToString() == "1")
                            {
                                //Home
                                grdRow.Cells["DocCurrRate"].Value = 1;
                                grdRow.Cells["LogFD"].Value = Math.Abs(Amt);
                            }
                            else
                            {
                                //Foreign
                                if (GFunc.NEDec(grdRow.Cells["LogFD"].Value, 0) == 0)
                                {
                                    grdRow.Cells["DocCurrRate"].Value = 1;
                                    grdRow.Cells["LogFD"].Value = Math.Abs(Amt);
                                }
                                else
                                {
                                    CurrRate = GFunc.RndDC(Amt, GFunc.NEDec(grdRow.Cells["LogFD"].Value, 0), GVar.RndDecs.Curpt);
                                    grdRow.Cells["DocCurrRate"].Value = Math.Abs(CurrRate);
                                }
                            }
                        }
                        break;
                    #endregion
                }

                #region Row Calculation
                if (runCal)
                {
                    CurrRate = GFunc.NEDec(grdRow.Cells["DocCurrRate"].Value, 1);
                    LogFD = GFunc.NEDec(grdRow.Cells["LogFD"].Value, 0);
                    LogFC = GFunc.NEDec(grdRow.Cells["LogFC"].Value, 0);
                    grdRow.Cells["logHD"].Value = GFunc.RndC(CurrRate * LogFD, GVar.RndDecs.Amtpt);
                    grdRow.Cells["logHC"].Value = GFunc.RndC(CurrRate * LogFC, GVar.RndDecs.Amtpt);
                    grdRow.Cells["BalanceH"].Value = GFunc.NEDec(grdRow.Cells["logHD"].Value, 0) - GFunc.NEDec(grdRow.Cells["logHC"].Value, 0);

                    Calculate();
                }
                #endregion

                this.IsDirty = true;
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

        //Error
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
                    string ActiveColKey = "";
                    if (((TAUtil.TAGridEditor)this.ActiveControl).ActiveCell != null)
                    {
                        ActiveColKey = GFunc.GridColumnKey_Get(this.ActiveControl);
                    }

                    l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, ActiveColKey });
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
                    string ActiveColKey = "";
                    if (((TAUtil.TAGridEditor)this.ActiveControl).ActiveCell != null)
                    {
                        ActiveColKey = GFunc.GridColumnKey_Get(this.ActiveControl);
                    }
                    l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, ActiveColKey });
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
