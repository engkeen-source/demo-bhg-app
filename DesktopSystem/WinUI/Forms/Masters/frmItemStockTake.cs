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
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using TAUtil;
namespace WinUI
{
    public partial class frmItemStockTake : Form
    {
        #region Local Variables

        private MSTStockCountFactory objFactory = null;
        string ContextMenuSetting = string.Empty;
        bool formClose = false;

        #endregion

        //Initialize
        public frmItemStockTake()
        {
            InitializeComponent();
        }//Completed

        //Form Events
        private void frmItemStockTake_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //Call Initialization
                this.objFactory = new BOLib.MSTStockCountFactory(BOLib.GEnum.InstanceMode.Normal);
                if (objFactory.GUID <= 0 || objFactory.StockCount_Get() == false)
                {
                    formClose = true;
                    return;
                }
                tagrdDetail.DataSource = objFactory.ObjMSTStockCounts;

                //Set ContextMenu & Grid Setting & Grid Formatting
                GlobalUI.FormGrids_Set(this, (int)GEnum.SystemCode.Stock_Count, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)GEnum.SystemCode.Stock_Count);
                GlobalUI.Combos_Fill(this, (int)GEnum.SystemCode.Stock_Count);

                GetStockCountOption();
                objFactory.ObjMSTStockCounts = (DataTable)tagrdDetail.DataSource;
                RemainingItems.SetValueTrigger(objFactory.ObjMSTStockCounts.AsEnumerable().Where(p => p.Field<bool>("HasBeenCounted") == false).Count(), false);
                StockCountStatus.Text = GEnum.StockCountStatus.Completed.ToString();
                Form_Layout();

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
        private void frmItemStockTake_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }
            if (this.formClose && objFactory == null)
            {
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (!objFactory.Dispose())
                {
                    throw new TAException(MsgID.Common.DisposeFail);
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
        private void frmItemStockTake_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)GEnum.SystemCode.Stock_Count);
                }
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
        private void frmItemStockTake_Shown(object sender, EventArgs e)
        {
            if (formClose)
                this.Close();
            else
            {
                CountType.Focus();
            }
        }//Completed

        //Form Display - Controlling and format 
        private void Form_Layout()
        {
            foreach (UltraGridColumn col in tagrdDetail.DisplayLayout.Bands[0].Columns)
            {
                col.CellActivation = Activation.ActivateOnly;
            }

            #region "Set Layout"
            switch (StockCountStatus.Text.ToLower())
            {
                case "newcount":
                    StartStopStatus.Visible = true;
                    btnStartCounting.Enabled = true;
                    btnStopCounting.Enabled = false;
                    btnSaveEntries.Enabled = false;
                    btnCreateAdj.Enabled = false;
                    tagrdDetail.DisplayLayout.Bands[0].Columns["CountQty"].CellActivation = Activation.ActivateOnly;
                    break;
                case "startcount":
                    btnStartCounting.Enabled = false;
                    btnStopCounting.Enabled = true;
                    btnSaveEntries.Enabled = false;
                    btnCreateAdj.Enabled = false;
                    tagrdDetail.DisplayLayout.Bands[0].Columns["CountQty"].CellActivation = Activation.ActivateOnly;
                    break;
                case "stopcount":
                    btnStartCounting.Enabled = false;
                    btnStopCounting.Enabled = false;
                    btnSaveEntries.Enabled = true;
                    btnCreateAdj.Enabled = false;
                    tagrdDetail.DisplayLayout.Bands[0].Columns["CountQty"].CellActivation = Activation.AllowEdit;
                    break;
                case "saveentries":
                    btnStartCounting.Enabled = true;
                    btnStopCounting.Enabled = false;
                    btnSaveEntries.Enabled = true;
                    btnCreateAdj.Enabled = false;
                    tagrdDetail.DisplayLayout.Bands[0].Columns["CountQty"].CellActivation = Activation.AllowEdit;
                    break;
                case "pending":
                    btnStartCounting.Enabled = false;
                    btnStopCounting.Enabled = false;
                    btnSaveEntries.Enabled = true;
                    btnCreateAdj.Enabled = false;
                    tagrdDetail.DisplayLayout.Bands[0].Columns["CountQty"].CellActivation = Activation.AllowEdit;
                    break;
                case "completed":
                    btnStartCounting.Enabled = false;
                    btnStopCounting.Enabled = false;
                    btnSaveEntries.Enabled = false;
                    btnCreateAdj.Enabled = true;
                    tagrdDetail.DisplayLayout.Bands[0].Columns["CountQty"].CellActivation = Activation.ActivateOnly;
                    break;
                default:  //'assume NA
                    btnStartCounting.Enabled = false;
                    btnStopCounting.Enabled = false;
                    btnSaveEntries.Enabled = false;
                    btnCreateAdj.Enabled = false;
                    tagrdDetail.DisplayLayout.Bands[0].Columns["CountQty"].CellActivation = Activation.ActivateOnly;
                    break;
            }
            #endregion

            #region "Record Count"
            if (objFactory.ObjMSTStockCounts != null && objFactory.ObjMSTStockCounts.Rows.Count > 0)
            {
                TotalItems.SetValueTrigger(objFactory.ObjMSTStockCounts.Rows.Count, false);
                ItemsCounted.SetValueTrigger(objFactory.ObjMSTStockCounts.AsEnumerable().Where(p => p.Field<bool>("HasBeenCounted") == true).Count(), false);
                RemainingItems.SetValueTrigger(objFactory.ObjMSTStockCounts.AsEnumerable().Where(p => p.Field<bool>("HasBeenCounted") == false).Count(), false);
            }
            #endregion
        }//Completed

        //Menu Strip Event
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.formClose = true;
            this.Close();
        }//Completed
        private void tsbImport_Click(object sender, EventArgs e)
        {
            try
            {
                frmExcelImport import = new frmExcelImport((int)this.objFactory.ConstantCodeKey) ;
                import.CopyRecordEvent += new GVar.ListEvent_CopyRecord(this.OnStockCount_ImportRecord);
                import.ShowDialog();
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

        //Button Events
        private void btnNewStockCount_Click(object sender, EventArgs e)
        {
            try
            {
                if (MsgBox.Show(MsgID.Common.ConfirmtoResetStockCount, GEnum.MsgBoxIcon.Alert, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                {
                    CountType.SetValueTrigger((int)GEnum.StockCountSelection.All, false);
                    StartDate.DateValue = DateTime.Today;
                    DateofLastCount.DateValue = null;
                    CompletedDate.DateValue = null;
                    StockCountStatus.Text = GEnum.StockCountStatus.NewCount.ToString();

                    if (objFactory.NewStockCount_Get())
                    {
                        tagrdDetail.DataSource = objFactory.ObjMSTStockCounts;
                        TotalItems.SetValueTrigger(objFactory.ObjMSTStockCounts.Rows.Count, false);
                        RemainingItems.SetValueTrigger(objFactory.ObjMSTStockCounts.Rows.Count, false);
                        ItemsCounted.SetValueTrigger(objFactory.ObjMSTStockCounts.Rows.Count, false);
                    }
                }
                Form_Layout();
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
        private void btnPrintCheckSheet_Click(object sender, EventArgs e)
        {
            try
            {
                frmPrintSelection print = new frmPrintSelection(objFactory.ObjMSTStockCounts, (int)objFactory.ConstantCodeKey, 2295, 0);
                print.ShowDialog();
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
        private void btnStartCounting_Click(object sender, EventArgs e)
        {
            try
            {
                if (MsgBox.Show(MsgID.Common.ConfirmtoAlreadyStockCount, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;

                    List<SqlParameter> parmList = new List<SqlParameter>();
                    parmList.Add(new SqlParameter("@pDate", DateTime.Today));
                    GFunc.ExecuteNonQueryProc("MSTItmStockTake_StartCounting", parmList);
                    objFactory.StockCount_Get();
                    tagrdDetail.DataSource = objFactory.ObjMSTStockCounts;

                    SysOptionUtility.Update(GVar.StockCountOption.StockCountStartDate, GFunc.GetSvrDateTime().ToString("dd MMM yyyy hh:mm tt"));                   
                    StockCountStatus.Text = GEnum.StockCountStatus.StartCount.ToString();
                    Form_Layout();
                    MsgBox.Show("You should begin your stock take now and you should NOT Add/Modify/Delete any transactions that is related to the items you are counting");
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
        private void btnStopCounting_Click(object sender, EventArgs e)
        {
            try
            {
                StockCountStatus.Text = GEnum.StockCountStatus.StopCount.ToString();
                Form_Layout();
                MsgBox.Show("You can enter your physical count now");
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
        private void btnSaveEntries_Click(object sender, EventArgs e)
        {
            try
            {
                //int Option = 0;
                tagrdDetail.PerformAction(UltraGridAction.ExitEditMode);
                tagrdDetail.UpdateData();
               // objFactory.ObjMSTStockCounts.DefaultView.RowFilter = "";

                //Save Data to server
                if (objFactory.ObjMSTStockCounts.Rows.Count > 0)
                {
                    objFactory.ObjMSTStockCounts.TableName = "dtDetail";
                    string xmlDetail = GFunc.ConvertDataTableToXML(objFactory.ObjMSTStockCounts.Copy());

                    List<SqlParameter> parmList = new List<SqlParameter>();
                    parmList.Add(new SqlParameter("@Option", 0));
                    parmList.Add(new SqlParameter("@xmlDetItms", xmlDetail));
                    GFunc.ExecuteNonQueryProc("MSTItmStockTake_Save", parmList);
                    SysOptionUtility.Update(GVar.StockCountOption.StockCountLastDate, GFunc.GetSvrDateTime().ToString("dd MMM yyyy hh:mm tt"));
                }

                //Check for all item has been counted
                RemainingItems.SetValueTrigger(objFactory.ObjMSTStockCounts.AsEnumerable().Where(p => p.Field<bool>("HasBeenCounted") == false).Count(), false);
                if (GFunc.NEDec(RemainingItems.Value, 0) > 0)
                    StockCountStatus.Text = GEnum.StockCountStatus.SaveEntries.ToString();
                else
                    StockCountStatus.Text = GEnum.StockCountStatus.Completed.ToString();

                //User confirm no more amendments
                if (StockCountStatus.Text == GEnum.StockCountStatus.Completed.ToString())
                {
                    int counterGrp = 1;
                    int counter = 0;
                    int[] ItmTypes = new int[] {100, 110, 210, 310, 410 };
                    if (MsgBox.Show("All Items has been checked, do you wish to End your stock take process", GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                    {
                        #region Prepare Adjustment Group for stock Items
                        IEnumerable<DataRow> dtFilter = objFactory.ObjMSTStockCounts.AsEnumerable().Where(r => ItmTypes.Contains(r.Field<int>("ItmType"))).OrderBy(r => r.Field<string>("ItmID"));

                        if(dtFilter.Count()>0)
                        {
                            foreach (DataRow row in dtFilter)
                            {
                                if (counter == 50)
                                {
                                    counterGrp++;
                                    counter = 1;
                                }
                                else
                                    counter++;
                                if(GFunc.NEDec(row["QtyToAdj"],0) !=0)
                                    row["DocAdjGrp"] = counterGrp;
                                else
                                    row["DocAdjGrp"] = 0;

                                row["DocAdjDone"] = 0;
                                row.EndEdit();

                            }
                            objFactory.ObjMSTStockCounts.AcceptChanges();
                        }
                        #endregion

                        #region Prepare Adjustment Group for Batch Items
                        //objFactory.ObjMSTStockCounts.DefaultView.RowFilter = "ItmType in (110,210,310,410)";
                        //objFactory.ObjMSTStockCounts.DefaultView.Sort = "ItmID ASC";
                        counter = 0;
                        counterGrp++;

                       // if (objFactory.ObjMSTStockCounts.DefaultView.Count > 0)
                        if (dtFilter.Count() > 0)
                        {
                            foreach (DataRow row in dtFilter)
                            {
                                //if (row.IsNew)
                                //    continue;
                                if (counter == 50)
                                {
                                    counterGrp++;
                                    counter = 1;
                                }
                                else
                                    counter++;

                                if (GFunc.NEDec(row["QtyToAdj"], 0) != 0)
                                    row["DocAdjGrp"] = counterGrp;
                                else
                                    row["DocAdjGrp"] = 0;
                                
                                row["DocAdjDone"] = 0;
                                row.EndEdit();
                            }
                            objFactory.ObjMSTStockCounts.AcceptChanges();
                        }
                        #endregion

                        //Save changes to Server                        
                        if (objFactory.ObjMSTStockCounts.Rows.Count > 0)
                        {
                            objFactory.ObjMSTStockCounts.TableName = "dtDetail";
                            string xmlDetail = GFunc.ConvertDataTableToXML(objFactory.ObjMSTStockCounts.Copy());

                            List<SqlParameter> parmList = new List<SqlParameter>();
                            parmList.Add(new SqlParameter("@Option", 0));
                            parmList.Add(new SqlParameter("@xmlDetItms", xmlDetail));
                            GFunc.ExecuteNonQueryProc("MSTItmStockTake_Save", parmList);
                        }

                        SysOptionUtility.Update(GVar.StockCountOption.StockCountCompletedDate, GFunc.GetSvrDateTime().ToString("dd MMM yyyy hh:mm tt"));
                        StockCountStatus.Text = GEnum.StockCountStatus.Completed.ToString();
                        DateofLastCount.SetValueTrigger(DateTime.Today, false);
                        CompletedDate.SetValueTrigger(DateTime.Today, false);
                    }
                    else
                    {
                        StockCountStatus.SetValueTrigger(GEnum.StockCountStatus.Pending.ToString(), false);
                        DateofLastCount.SetValueTrigger(DateTime.Today, false);
                    }
                }
                else
                {
                    DateofLastCount.SetValueTrigger(DateTime.Today, false);
                }
                Form_Layout();
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
        

        //Controls Events
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

        private void CountType_CustomUpdate(object sender, CancelEventArgs e)
        {
            //objFactory.ObjMSTStockCounts.DefaultView.RowFilter = "";
            tagrdDetail.DataSource = objFactory.ObjMSTStockCounts;
            try
            {               
                if ((int)CountType.Value == (int)GEnum.StockCountSelection.Counted)
                    //GridFilterToDefaultView   
                    objFactory.ObjMSTStockCounts.DefaultView.RowFilter = "HasBeenCounted=true";
                else if ((int)CountType.Value == (int)GEnum.StockCountSelection.Remaining)                    
                    //GridFilterToDefaultView   
                    objFactory.ObjMSTStockCounts.DefaultView.RowFilter = "HasBeenCounted=false";
                else if ((int)CountType.Value == (int)GEnum.StockCountSelection.Discrepancies)
                {                   
                    //GridFilterToDefaultView   
                    objFactory.ObjMSTStockCounts.DefaultView.RowFilter = "HasBeenCounted=1 AND FreezeQty<>CountQty";
                }
                tagrdDetail.DataSource = objFactory.ObjMSTStockCounts;
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
        
        //Grid Events
        private void tagrdDetail_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
                switch (tagrdDetail.ActiveCell.Column.Key.ToLower())
                {
                    case "countqty":
                        if ((bool)tagrdDetail.ActiveRow.Cells["HasBeenCounted"].Value)
                        {
                            if (MsgBox.Show(MsgID.Common.ConfirmtoResetStockCount, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) != GEnum.MsgBoxButton.Yes)
                            {
                                e.Cancel = true;
                                return;
                            }
                        }
                        tagrdDetail.ActiveRow.Cells["HasBeenCounted"].Value = true;
                        tagrdDetail.ActiveCell.Value = GFunc.RndC(GFunc.NEDec(tagrdDetail.ActiveCell.Value, 0), GVar.RndDecs.Qtypt);
                        tagrdDetail.ActiveRow.Cells["QtyToAdj"].Value = (GFunc.RndC(GFunc.NEDec(tagrdDetail.ActiveRow.Cells["FreezeQty"].Value, 0), GVar.RndDecs.Qtypt) - GFunc.RndC(GFunc.NEDec(tagrdDetail.ActiveCell.Value, 0), GVar.RndDecs.Qtypt)) * -1;
                        tagrdDetail.ActiveRow.Cells["CountDate"].Value = DateTime.Today;
                        break;
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

        //Functions
        private void GetStockCountOption()
        {
            try
            {
                StartDate.SetValueTrigger(SysOptionUtility.GetDate("StockCountStartDate"), false);
                DateofLastCount.SetValueTrigger(SysOptionUtility.GetDate("StockCountLastDate"), false);
                CompletedDate.SetValueTrigger(SysOptionUtility.GetDate("StockCountCompletedDate"), false);
                TotalItems.SetValueTrigger(SysOptionUtility.StockCountItemTotal, false);
                RemainingItems.SetValueTrigger(SysOptionUtility.StockCountItemRemaining, false);
                ItemsCounted.SetValueTrigger(SysOptionUtility.StockCountItemCounted, false);
                StockCountStatus.SetValueTrigger(((GEnum.StockCountStatus)SysOptionUtility.StockCountStatus).ToString(), false);
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

        private void OnStockCount_ImportRecord(GEnum.CopyOption copyOption, int CopyDocCodeKey, int CopyDocKey, DataTable dtStockCountExcel, bool bOverWrite)
        {
            try
            {               
                switch (copyOption)
                {
                    case GEnum.CopyOption.Import:
                        DataTable dtStockTake = tagrdDetail.DataSource as DataTable ;
                        dtStockTake.TableName = "dtStockCount";
                        string xmlStockCount = GFunc.ConvertDataTableToXML(dtStockTake);

                        dtStockCountExcel.TableName = "dtStockCountExcel";
                        string xmlStockCountExcel = GFunc.ConvertDataTableToXML(dtStockCountExcel);

                        if (GFunc.ValidateExcelData(dtStockCountExcel, (int)objFactory.ConstantCodeKey)==false)
                            return;

                        List<SqlParameter> paraList = new List<SqlParameter>();
                        paraList.Add(new SqlParameter("@xmlStockCount", xmlStockCount));
                        paraList.Add(new SqlParameter("@xmlStockCountExcel", xmlStockCountExcel));
                        paraList.Add(new SqlParameter("@bOverWrite", bOverWrite));
                        SqlParameter RetValue = new SqlParameter();
                        RetValue.ParameterName = "@RetValue";
                        RetValue.Value = 0;
                        RetValue.Direction = ParameterDirection.InputOutput;
                        paraList.Add(RetValue);
                        DataTable dt =  GFunc.ExecuteProc("MSTItmStockCount_Import", paraList);
                        if (GFunc.NEInt(RetValue.Value, 0) == (int)GEnum.SpState.Fail)
                        {
                            MsgBox.Show("Import failed");

                        }
                        else
                        {
                            objFactory.ObjMSTStockCounts = dt;
                            tagrdDetail.DataSource = objFactory.ObjMSTStockCounts;
                        }
                       
                        break;
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
        }

        //Set Error Methods
        private void OnDataError(object sender, TAUtil.TAErrorEventArgs e)
        {
            try
            {
                if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_NUMERIC)
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

        }      

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
