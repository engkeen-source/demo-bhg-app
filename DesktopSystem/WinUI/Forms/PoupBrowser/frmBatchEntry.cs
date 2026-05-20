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
using System.Diagnostics;
using TAUtil;

namespace WinUI
{
    public partial class frmBatchEntry : Form
    {
        #region Local Variables

        string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        private bool ReadonlyMode = false;
        private bool ReturnConsignmentMode = false;
        private bool AddBatchMode = false;          //TRUE - Only New Batch Entry is Allow, FALSE - Only Batch Qty (Add or Deduct) is allowed
        private short TransactionSign = -1;         //For AddMode it is always 1, else can be (-1 or 1) depending on the Caller DocCode

        private Document CallerObjDoc = null;
        private GEnum.SystemCode CallerDC = 0;      //When CallerDC = 0 mean ReadOnly Mode (no add/edit of emtry) else other value will represent Caller DocCodeKey
        private short CallerDocSign = 0;
        private DataTable dtParentItm = null;       //Datatable to store the list of Parent Item of all the batch items (LineType =1000, LineLinkKey = 0)
        private DataTable dtCallerGridSource = null;
        private UltraGrid grdCallerGrid = null;

        private int ParentcurrentRowIndex = 0;
        private DataRow ParentCurrentRow;
        private int ParentCount = 0;
        private DataTable dtItmBatch = null;            //Datasource for Batch List Grid
        private DataTable dtItmBatchSelected = null;    //Datasource for Batch Selected Grid (include batch parent and child row)
        private MSTItm objItm = null;                   //MSTItm Object for Parent Item

        private bool IsDirty = false;
        //private int CallerActiveRowDocItmKey = 0;
        //private string CallerActiveCell = string.Empty;
        private int CurrentParentItmKey = 0;
        private int CurrentEntryBatchKey = 0;           //Current BatchKey for BatchEntry Infor
        private int CurrentEntryLinkPointer = 0;        //Current DocItmKey of BatchEntry Infor that is related to BatchSelected Grid
        #endregion

        //Initialize
        public frmBatchEntry()
        {
            InitializeComponent();
        }//Completed
        public frmBatchEntry(Document objDoc, UltraGrid grdDetail, bool bReadOnlyMode)
        {
            //For Add/Edit
            //For Readonly mode - user dblClick on the Grid ItmDes
            //For Add/Edit mode - user dblClick on the Grid ItmQty
            //Not for Return Consignment, if open BatchEntry for Edit, the user can only work on the Batch list from the Issue Consignment
            InitializeComponent();
            grdDetail.UpdateData();
            ReadonlyMode = bReadOnlyMode;
            IsDirty = false;

            int[] ItmTypes = new int[] { 110, 210, 310, 410 };

            CallerObjDoc = objDoc;
            CallerDC = (GEnum.SystemCode)objDoc.DocCodeKey;
            CallerDocSign = (short)objDoc.DocSign;
            grdCallerGrid = grdDetail;
            dtCallerGridSource = grdDetail.DataSource as DataTable;
            dtItmBatchSelected = dtCallerGridSource.Copy();

            dtParentItm = dtItmBatchSelected.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == 0
                && ItmTypes.Contains(r.Field<int>("ItmType"))).OrderBy(r => r.Field<decimal>("ItmSN")).CopyToDataTable();
            ParentCount = GFunc.NEInt(dtParentItm.Rows.Count, 0);


            //If the batch record is from Purchase Delivery, it is not then allowed to be edited.
            if ((CallerObjDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Invoice ||
                    CallerObjDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Debit_Note ||
                    CallerObjDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Debit_Note))
            {
                if (grdDetail.ActiveRow != null)
                {
                    if (!GFunc.IsNEZ(grdDetail.ActiveRow.Cells["APPDDK"].Value))
                    {
                        ReadonlyMode = true;
                    }
                }
            }
            if (CallerObjDoc.IsReadOnly)
                ReadonlyMode = true;

            if (CallerDC == GEnum.SystemCode.Return_Consignment)
                ReturnConsignmentMode = true;

        }//Completed

        //Form Events
        private void frm_Load(object sender, EventArgs e)
        {
            try
            {
                //Get Entry Mode
                TransactionMode_Set();

                //Attached drag & drop events 
                this.tagrdItmBatchSelected.AllowDrop = true;
                this.tagrdItmBatchSelected.DisplayLayout.Override.AllowColMoving = AllowColMoving.WithinBand;
                this.tagrdItmBatchSelected.DragDrop += new System.Windows.Forms.DragEventHandler(GlobalUI.Grid_DragDropDocItm);
                this.tagrdItmBatchSelected.DragOver += new System.Windows.Forms.DragEventHandler(GlobalUI.Grid_DragOver);
                this.tagrdItmBatchSelected.SelectionDrag += new System.ComponentModel.CancelEventHandler(GlobalUI.Grid_SelectionDrag);
                this.tagrdItmBatchSelected.DisplayLayout.Override.SelectTypeRow = SelectType.ExtendedAutoDrag;


                //Form and Grid Format
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0, this.Name);
                GlobalUI.cmnuGlobal_Set(this);
                FormLayout(true);

                if ((CallerObjDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Invoice ||
                   CallerObjDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Debit_Note ||
                   CallerObjDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Debit_Note))
                {
                    //We need to reverse the effect of the computed callergrid qty
                    foreach (DataRow dr in dtItmBatchSelected.Rows)
                    {
                        //When it is from PD , the ItmBatchQty must be readonly and we cannot change the Sign
                        if (GFunc.IsNEZ(dr["APPDDK"])) 
                        {
                            dr["ItmBatchQty"] = (decimal)dr["ItmBatchQty"] * CallerDocSign * TransactionSign;
                        }
                    }
                }
                else
                {
                    //For other docCode we will update the proper sign for the ItmBatchQty
                    foreach (DataRow dr in dtItmBatchSelected.Rows)
                    {
                        dr["ItmBatchQty"] = (decimal)dr["ItmBatchQty"] * CallerDocSign * TransactionSign;
                    }
                }

                //Validation - Close form when ther are no batch item in Caller grid
                if (ParentCount <= 0)
                {
                    MsgBox.Show("There are not Batch item");
                    this.Close();
                    return;
                }

                //we need to show the infor for the current active row the user is currently working on
                //to do tha, we get the batch entry infor from server base on the caller grid(active row)
                if (grdCallerGrid.ActiveRow != null)
                {
                    int CallerGridCurrentParentRow = GFunc.NEInt(grdCallerGrid.ActiveRow.Cells["DocItmKey"].Value, 0);
                    for (int i = 0; i < dtParentItm.Rows.Count; i++)
                    {
                        if (GFunc.NEInt(dtParentItm.Rows[i]["DocItmKey"], 0) == CallerGridCurrentParentRow)
                        {
                            ParentcurrentRowIndex = i;
                            break;
                        }
                    }
                }
                LoadData(ParentcurrentRowIndex);
                
                IsDirty = false;
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
                //this.Cursor = Cursors.Default;
            }
        }//Completed
        private void frm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
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
        private void frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (ReadonlyMode == false)
                    e.Cancel = !SaveChanges();
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

        //Form Layout, Refresh Data
        private void FormLayout(bool FormLoad)
        {
            try
            {
                //Initialisation
                if (FormLoad)
                {
                    tagrdItmBatch.DisplayLayout.Bands[0].Override.AllowAddNew = AllowAddNew.No;
                    tagrdItmBatch.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                    tagrdItmBatch.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.False;
                    tagrdItmBatchSelected.DisplayLayout.Bands[0].Override.AllowAddNew = AllowAddNew.No;
                    tagrdItmBatchSelected.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                    return;
                }
                if (ReadonlyMode)
                {
                    ItmQty.Enabled = false;
                    btnAssign.Enabled = false;
                    BatchID.Enabled = false;
                    BatchQty.Enabled = false;
                    MFGDate.Enabled = false;
                    ExpDate.Enabled = false;
                    btnUpdate.Visible = false;
                    btnSaveAll.Visible = false;
                    tagrdItmBatchSelected.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.False;
                }
                else
                {
                    ItmQty.Enabled = true;
                    btnAssign.Enabled = true;
                    BatchID.Enabled = true;
                    BatchQty.Enabled = true;
                    MFGDate.Enabled = true;
                    ExpDate.Enabled = true;
                    btnUpdate.Visible = true;
                    btnSaveAll.Visible = true;
                    tagrdItmBatchSelected.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.True;
                }

                //Setup layout when information has been reloaded
                pnlBatchItm.Visible = !AddBatchMode;
                btnAssign.Visible = !AddBatchMode;
                if (AddBatchMode)
                {
                    ItmQty.Enabled = false;
                    BatchID.Focus();
                    this.Height = this.Height - pnlBatchItm.Height;//resize the form size
                }
                else
                    ItmQty.Focus();

                if (ReadonlyMode == false)
                {
                    MFGDate.Enabled = AddBatchMode;
                    ExpDate.Enabled = AddBatchMode;

                    MFGDate.TabStop = AddBatchMode;
                    ExpDate.TabStop = AddBatchMode;
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
        }//Completed
        private void Grid_Refresh()
        {
            try
            {
                tagrdItmBatch.DataSource = dtItmBatch;
                tagrdItmBatch.Rows.Refresh(RefreshRow.ReloadData);

                //GridFilterToDefaultView   
                dtItmBatchSelected.DefaultView.RowFilter = "LineLinkKey=" + GFunc.NEStr(ParentCurrentRow["DocItmKey"].ToString(), string.Empty);
                dtItmBatchSelected.DefaultView.Sort = "ItmDetSN";
                tagrdItmBatchSelected.DataSource = dtItmBatchSelected;
                tagrdItmBatchSelected.Rows.Refresh(RefreshRow.ReloadData);

                //ColumnFiltersCollection columnFilterHDR = this.tagrdItmBatchSelected.DisplayLayout.Bands[0].ColumnFilters;
                //columnFilterHDR.ClearAllFilters();
                //string ParentDocItmKey = GFunc.NEStr(ParentCurrentRow["DocItmKey"].ToString(), string.Empty);
                //columnFilterHDR["LineLinkKey"].FilterConditions.Add(FilterComparisionOperator.Equals, ParentDocItmKey);
                //tagrdItmBatchSelected.Refresh();               

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

        //Button Event
        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (CalculateTotalSelectedQty(true))
                {
                    if (ParentcurrentRowIndex < dtParentItm.Rows.Count - 1)
                    {
                        ParentcurrentRowIndex++;
                        LoadData(ParentcurrentRowIndex);
                        return;
                    }

                    if (AddBatchMode)
                    {
                        BatchID.Focus();
                    }
                    else
                        ItmQty.Focus();

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
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                if (CalculateTotalSelectedQty(true))
                {
                    if (ParentcurrentRowIndex > 0)
                    {
                        ParentcurrentRowIndex--;
                        LoadData(ParentcurrentRowIndex);
                        return;
                    }

                    if (AddBatchMode)
                    {
                        BatchID.Focus();
                    }
                    else
                        ItmQty.Focus();

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
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
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
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MFGDate.Value == null)
                    MFGDate.SetValueTrigger(DateTime.Today, true);
                BatchEntryInfo_Update();
                BatchID.Focus();
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
        private void btnAssign_Click(object sender, EventArgs e)
        {
            try
            {
                decimal reqQty = ItmQty.DecimalValue;

                #region Validation
                if (reqQty >= 0)
                {
                    MsgBox.Show("You can only auto assign for Batch Qty deduction");
                    return;
                }
                #endregion

                #region Clear all child data in BatchSelected grid
                int rowCount = dtItmBatchSelected.Rows.Count;
                for (int i = rowCount - 1; i > -1; i--)
                {
                    if (dtItmBatchSelected.Rows[i]["LineLinkKey"].ToString() == ParentCurrentRow["DocItmKey"].ToString())
                    {
                        dtItmBatchSelected.Rows[i].Delete();
                    }
                }
                dtItmBatchSelected.AcceptChanges();
                #endregion

                decimal assignQty = 0;
                decimal BatchBalQty = 0;

                reqQty = Math.Abs(reqQty);
                foreach (DataRow dr in dtItmBatch.Rows)
                {
                    if (reqQty == 0)
                        break;

                    BatchBalQty = GFunc.NEDec(dr["BatchQtyBal"], 0);

                    if (BatchBalQty <= 0)
                        continue;

                    if (reqQty > BatchBalQty)
                        assignQty = BatchBalQty;
                    else
                        assignQty = reqQty;

                    reqQty = reqQty - assignQty;
                    //After calculation we need to reverse the sign
                    assignQty = assignQty * -1;

                    BatchSelected_Add((int)dr["BatchKey"], GFunc.NEStr(dr["BatchID"], string.Empty), assignQty, dr["BatchMfgDate"], dr["BatchExpDate"], false);
                }

                BatchEntryInfo_Clear();
                dtItmBatchSelected.AcceptChanges();
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
        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (Save())
                    this.Close();
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

        //Control Events       
        private void BatchID_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {

                string batchID = GFunc.NEStr(BatchID.Value, string.Empty);
                if (GFunc.IsNE(batchID))
                    return;
                if (BatchInfo_Search(batchID) == false)
                    e.Cancel = true;

                IsDirty = true;
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
        private void BatchQty_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                btnUpdate.Focus();
                IsDirty = true;
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
        private void ItmQty_CustomUpdate(object sender, CancelEventArgs e)
        {
            IsDirty = true;
        }//Completed
        private void MFGDate_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                DateTime mfgDate;
                string defExpDate = objItm.DefaultExpDate;

                if (GFunc.IsNE(MFGDate.DateValue) == false)
                {
                    mfgDate = (DateTime)MFGDate.DateValue;
                    if (defExpDate.Length > 0)
                    {
                        int interval = int.Parse(defExpDate.Substring(0, defExpDate.Length - 1));
                        string dateType = defExpDate.Substring(defExpDate.Length - 1, 1);

                        switch (dateType.ToLower())
                        {
                            case "w"://Week
                                ExpDate.SetValueTrigger(mfgDate.AddDays(interval * 7), false);
                                break;
                            case "m"://Month
                                ExpDate.SetValueTrigger(mfgDate.AddMonths(interval), false);
                                break;
                            case "y"://Year
                                ExpDate.SetValueTrigger(mfgDate.AddYears(interval), false);
                                break;
                        }
                    }
                    else //mic check default to add 1 year
                    {
                        ExpDate.SetValueTrigger(mfgDate.AddYears(1), false);
                    }
                }
                IsDirty = true;
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
        private void ExpDate_CustomUpdate(object sender, CancelEventArgs e)
        {
            IsDirty = true;
        }//Completed

        //Grid Common Events        
        private void tagrdItmBatch_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            try
            {
                if (this.ReadonlyMode)
                    return;

                if (tagrdItmBatch.ActiveRow != null)
                {
                    UltraGridRow grow = tagrdItmBatch.ActiveRow;

                    //We cannot allow a batch that has already been selected to be inserted into the Batch Selected List again
                    //as this would cause duplication error
                    //Therefore we will try to find if it already exist in BatchSelect and select it.
                    if (BatchSelected_Get(grow.Cells["BatchID"].Value.ToString()))
                        return;

                    decimal reqQty = ItmQty.DecimalValue;
                    decimal assignQty = 0;
                    decimal balQty = (decimal)grow.Cells["BatchQtyBal"].Value;

                    //Get BatchSelected TotalQty
                    decimal totalQty = (from row in dtItmBatchSelected.AsEnumerable()
                                        where row.Field<int>("LineLinkKey") == (int)ParentCurrentRow["DocItmKey"]
                                        select row.Field<decimal>("ItmBatchQty")).Sum();

                    //Calculate Qty to assign to BatchSelect list. We will always insert this batch to the BatchSelect if not already inserted
                    //For Deduct Qty (-ve) - we will calculate the BatchQty to assign.
                    //For Add Qty (+ve) - the BatchQty to assign is 0.
                    if (reqQty < 0)//deduct qty
                    {
                        //-ve values is converted to +ve value to make it easier to understand
                        reqQty = Math.Abs(reqQty);
                        totalQty = totalQty * -1;

                        if (reqQty > totalQty)
                            reqQty = reqQty - totalQty;
                        else
                            reqQty = 0;

                        //Calculate the qty to assign
                        if (reqQty > balQty)
                            assignQty = balQty;
                        else
                            assignQty = reqQty;

                        //After calculation we need to reverse the sign
                        assignQty = assignQty * -1;
                    }
                    else
                        assignQty = 0;

                    BatchSelected_Add((int)grow.Cells["BatchKey"].Value, grow.Cells["BatchID"].Value.ToString(), assignQty, grow.Cells["BatchMfgDate"].Value, grow.Cells["BatchExpDate"].Value, true);
                    IsDirty = true;
                    BatchQty.Focus();
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
        }//Completed
        private void tagrdItmBatchSelected_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                e.DisplayPromptMsg = false;

                if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) == GEnum.MsgBoxButton.Delete)
                {
                    if (tagrdItmBatchSelected.Rows.Count == 0)
                        e.Cancel = true;
                }
                else
                    e.Cancel = true;
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
        private void tagrdItmBatchSelected_AfterRowsDeleted(object sender, EventArgs e)
        {
            try
            {
                CalculateTotalSelectedQty(true);
                BatchEntryInfo_Clear();
                IsDirty = true;
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
        private void tagrdItmBatchSelected_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            try
            {
                Debug.Print("tagrdItmBatchSelected_DoubleClickRow");
                BatchEntryInfo_Clear();
                if (tagrdItmBatchSelected.ActiveRow != null)
                {
                    UltraGridRow row = tagrdItmBatchSelected.ActiveRow;
                    BatchEntryInfo_Set((int)row.Cells["ItmBatchKey"].Value, row.Cells["BatchID"].Value.ToString(), (decimal)row.Cells["ItmBatchQty"].Value, row.Cells["BatchMfgDate"].Value, row.Cells["BatchExpDate"].Value, (int)row.Cells["DocItmKey"].Value);
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
        }//Completed

        //Functions
        private void BatchEntryInfo_Clear()
        {
            Debug.Print("BatchEntryInfo_Clear");
            CurrentEntryBatchKey = 0;
            CurrentEntryLinkPointer = 0;
            BatchID.SetValueTrigger("", false);
            BatchQty.SetValueTrigger("", false);
            MFGDate.SetValueTrigger(null, false);
            ExpDate.SetValueTrigger(null, false);
            if (ReadonlyMode == false)
                BatchID.Enabled = true;
        }//Completed
        private void BatchEntryInfo_Set(int BatchKey, string batchID, decimal batQty, object mfgDate, object expDate, int BatchSelectedDocItmKey)
        {
            try
            {
                DateTime vMfgDate = GFunc.NEDateTime(mfgDate, null).Date;
                DateTime vExpDate = GFunc.NEDateTime(expDate, null).Date;
                Debug.Print("BatchEntryInfo_Set");
                BatchID.SetValueTrigger(batchID, false);
                BatchQty.SetValueTrigger(batQty, false);
                MFGDate.SetValueTrigger(vMfgDate, false);
                ExpDate.SetValueTrigger(vExpDate, false);
                CurrentEntryBatchKey = BatchKey;
                CurrentEntryLinkPointer = BatchSelectedDocItmKey;
                if (ReadonlyMode == false)
                    BatchID.Enabled = false;

                BatchQty.Focus();
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
        private void BatchEntryInfo_Update()
        {
            try
            {
                Debug.Print("BatchEntryInfo_Set");
                if (BatchEntryInfo_Validation() == false)
                    return;

                if (AddBatchMode)
                {
                    #region Add New Batch Mode
                    if (CurrentEntryLinkPointer == 0 && AddBatchMode)
                        BatchSelected_Add(CurrentEntryBatchKey, BatchID.Value.ToString(), BatchQty.DecimalValue, MFGDate.DateValue, ExpDate.DateValue, false);
                    else
                    {
                        UltraGridRow gRow = this.tagrdItmBatchSelected.Rows.OfType<UltraGridRow>().ToList().Find(
                        row => row.Cells["DocItmKey"].Text.Equals(CurrentEntryLinkPointer.ToString(), StringComparison.CurrentCultureIgnoreCase));

                        gRow.Cells["BatchID"].Value = BatchID.Value;
                        gRow.Cells["ItmBatchQty"].Value = BatchQty.DecimalValue;
                        gRow.Cells["BatchMfgDate"].Value = MFGDate.DateValue;
                        gRow.Cells["BatchExpDate"].Value = ExpDate.DateValue;
                        tagrdItmBatchSelected.UpdateData();
                    }
                    #endregion
                }
                else
                {
                    #region Add or Deduct Batch Qty Mode
                    UltraGridRow gRow = this.tagrdItmBatchSelected.Rows.OfType<UltraGridRow>().ToList().Find(
                        row => row.Cells["DocItmKey"].Text.Equals(CurrentEntryLinkPointer.ToString(), StringComparison.CurrentCultureIgnoreCase));

                    gRow.Cells["ItmBatchQty"].Value = BatchQty.DecimalValue;
                    tagrdItmBatchSelected.UpdateData();
                    #endregion
                }
                CalculateTotalSelectedQty(true);
                BatchEntryInfo_Clear();
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
        private bool BatchEntryInfo_Validation()
        {
            try
            {
                Debug.Print("BatchEntryInfo_Validation");
                decimal batchQty = BatchQty.DecimalValue;
                string batchID = GFunc.NEStr(this.BatchID.Value, string.Empty);

                if (AddBatchMode)
                {
                    #region Add New Batch Mode
                    if (GFunc.IsNE(batchID))
                    {
                        MsgBox.Show(MsgID.Validation.IsRequire + "%" + "Batch ID");
                        return false;
                    }

                    if (IsDuplicateBatchID(2, batchID))
                    {
                        MsgBox.Show(MsgID.BatchEntry.BatchIDAlreadyExists + "%" + batchID);
                        return false;
                    }
                    if (BatchQty.DecimalValue <= 0)
                    {
                        MsgBox.Show("Batch quantity cannot be negative or zero.");
                        return false;
                    }
                    #endregion
                }
                else
                {
                    #region Add or Deduct Batch Qty Mode
                    if (GFunc.IsNEZ(CurrentEntryBatchKey) || GFunc.IsNEZ(CurrentEntryLinkPointer))
                    {
                        MsgBox.Show("You have not selected a batch, please select a batch from the list");
                        return false;

                    }

                    decimal BatchQtyBal = (from row in dtItmBatch.AsEnumerable()
                                           where row.Field<int>("BatchKey") == CurrentEntryBatchKey
                                           select row.Field<decimal>("BatchQtyBal")).Sum();

                    if (BatchQtyBal + batchQty < 0) //note: batchQty is -ve for deduct
                    {
                        MsgBox.Show(MsgID.BatchEntry.NotEnoughQuantity);
                        return false;
                    }
                    #endregion
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
        private bool BatchSelected_Get(string batchID)
        {
            //Search for a match in BatchSelect grid and Set it to the BatchEntry Controls
            try
            {
                Debug.Print("BatchSelected_Get");

                UltraGridRow grdRow = this.tagrdItmBatchSelected.Rows.OfType<UltraGridRow>().ToList()
                            .Find(r => r.Cells["BatchID"].Value.ToString().Equals(batchID, StringComparison.CurrentCulture)
                                && (int)r.Cells["ItmKey"].Value == CurrentParentItmKey
                                && (int)r.Cells["LineLinkKey"].Value == (int)ParentCurrentRow["DocItmKey"]);

                if (grdRow != null)
                {
                    this.tagrdItmBatchSelected.ActiveRow = grdRow;
                    BatchEntryInfo_Set((int)grdRow.Cells["ItmBatchKey"].Value, batchID, (decimal)grdRow.Cells["ItmBatchQty"].Value, grdRow.Cells["BatchMfgDate"].Value, grdRow.Cells["BatchExpDate"].Value, (int)grdRow.Cells["DocItmKey"].Value);
                    tagrdItmBatchSelected.ActiveRowScrollRegion.ScrollRowIntoView(grdRow);
                    return true;
                }
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
        }//Completed
        private bool BatchSelected_Add(int BatchKey, string batchID, decimal assignQty, object mfgDate, object expDate, bool SetBatchEntry)
        {
            try
            {
                Debug.Print("BatchSelected_Add");
                DataRow drSelect = dtItmBatchSelected.NewRow();

                //copy parent row data to a new row in BatchSelected
                //note: the parent and batchselect has the same structure.
                foreach (DataColumn dc in dtItmBatchSelected.Columns)
                {
                    drSelect[dc.ColumnName] = ParentCurrentRow[dc.ColumnName];
                }

                //update require information to new child row.
                int NewDocItmKey = DocComUtility.GridAutoID_Get(tagrdItmBatchSelected, "DocKey", "DocItmKey");
                drSelect["BatchID"] = batchID;
                drSelect["DocItmKey"] = NewDocItmKey;
                drSelect["ItmBatchKey"] = BatchKey;
                drSelect["LineType"] = LineType_Get((int)objItm.ItmType);
                drSelect["LineLinkKey"] = ParentCurrentRow["DocItmKey"];
                drSelect["ItmBatchQty"] = assignQty;
                drSelect["BatchMfgDate"] = mfgDate;
                drSelect["BatchExpDate"] = expDate;
                drSelect["ItmQty"] = 0;

                IEnumerable<DataRow> dtItmBatchFilter = dtItmBatchSelected.AsEnumerable().Where(p => p.Field<int>("LineLinkKey") == GFunc.NEInt(ParentCurrentRow["DocItmKey"], 0));
                if (dtItmBatchFilter.Count() > 0)
                    drSelect["ItmDetSN"] = dtItmBatchFilter.Max(p => p.Field<decimal>("ItmDetSN")) + 1;
                else
                    drSelect["ItmDetSN"] = 1;

                dtItmBatchSelected.Rows.Add(drSelect);
                dtItmBatchSelected.AcceptChanges();

                CalculateTotalSelectedQty(true);

                if (SetBatchEntry)
                    BatchEntryInfo_Set(BatchKey, batchID, assignQty, mfgDate, expDate, NewDocItmKey);

                IsDirty = true;
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private bool BatchInfo_Search(string batchID)
        {
            try
            {
                Debug.Print("BatchSelected_Add");
                if (AddBatchMode)
                {
                    #region New Batch Mode
                    if (CurrentEntryLinkPointer == 0)
                    {
                        #region Add new Batch or Find a Batch from BatchSelected List
                        //User is trying search for the Batch ID to edit
                        if (BatchSelected_Get(batchID))
                            return true;

                        //User is trying to add a new batch                        
                        if (IsDuplicateBatchID(1, batchID))
                        {
                            MsgBox.Show("Duplicate Batch ID, please use another Batch ID");
                            return false;
                        }
                        else
                            return true;
                        #endregion
                    }
                    else
                    {
                        #region Rename a Batch ID
                        //User is trying to rename the Batch ID                        
                        if (IsDuplicateBatchID(2, batchID))
                        {
                            MsgBox.Show("Duplicate Batch ID");
                            return false;
                        }
                        else
                            return true;
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region add or deduct current batch qty mode

                    //Validation --- Changing of BatchID is not allowed when user have select a record from batchSelect grid
                    if (CurrentEntryLinkPointer != 0)
                    {
                        MsgBox.Show("You cannot change the Batch ID of an existing ");
                        return false;
                    }

                    //Retrive Batch infor and Assign to Batch entry Control
                    //We will search if this infor is avaliable in BatchSelected Grid
                    //if not found we will then search in MSTItmBatch in server

                    #region Find the BatchID in BatchSelected and in MSTItmBatch
                    //User is trying search for the Batch ID to edit          
                    if (BatchSelected_Get(batchID))
                        return true;

                    //Search in MSTItmBatch
                    List<SqlParameter> paraList = new List<SqlParameter>();
                    paraList.Add(new SqlParameter("@Option", 4));
                    paraList.Add(new SqlParameter("@BatchID", batchID));
                    paraList.Add(new SqlParameter("@ItmKey", CurrentParentItmKey));
                    SqlParameter paraOut = new SqlParameter();
                    paraOut.ParameterName = "@RetValue";
                    paraOut.Value = 0;
                    paraOut.Direction = ParameterDirection.Output;
                    paraList.Add(paraOut);
                    DataTable dtBatchInfo = GFunc.ExecuteProc("MSTItmBatch_Get", paraList);
                    if (dtBatchInfo.Rows.Count > 0)
                    {
                        if (GFunc.NEBool(dtBatchInfo.Rows[0]["BatchStatus"], false) == false)
                        {
                            if (!(GFunc.NEInt(dtBatchInfo.Rows[0]["LogDC"], 0) == CallerObjDoc.DocCodeKey && GFunc.NEInt(dtBatchInfo.Rows[0]["LogDK"], 0) == CallerObjDoc.DocKey))
                            {
                                MsgBox.Show("the Batch is available for use.");
                                return false;
                            }
                        }

                        BatchSelected_Add((int)dtBatchInfo.Rows[0]["BatchKey"], batchID, 0, dtBatchInfo.Rows[0]["BatchMfgDate"], dtBatchInfo.Rows[0]["BatchExpDate"], true);
                        return true;
                    }
                    #endregion

                    MsgBox.Show("Batch ID not found");
                    return false;

                    #endregion
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
        }//Completed        

        private void LoadData(int ParentrowIndex)
        {

            try
            {
                #region Update Control Values and get ParentRow
                ParentCurrentRow = dtParentItm.Rows[ParentrowIndex];
                CurrentParentItmKey = (int)ParentCurrentRow["ItmKey"];
                objItm = MSTItm.Get(CurrentParentItmKey);
                ItmID.Text = GFunc.NEStr(ParentCurrentRow["ItmID"], string.Empty);
                ItmDes.Text = GFunc.NEStr(ParentCurrentRow["ItmDes"], string.Empty);
                ItmSN.Text = GFunc.NEDec(ParentCurrentRow["ItmSN"], 0).ToString();
                
                if ((CallerObjDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Invoice ||
                   CallerObjDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Debit_Note ||
                   CallerObjDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Debit_Note))
                {
                    if (GFunc.IsNEZ(ParentCurrentRow["APPDDK"]))
                        ReadonlyMode = false;
                    else
                        ReadonlyMode = true;
                }
                else
                    ReadonlyMode = false;
                #endregion

                #region Compute ParentQty
                decimal ParentQty = GFunc.NEDec(ParentCurrentRow["ItmQty"], 0) * CallerDocSign * TransactionSign;
                ItmQty.SetValueTrigger(ParentQty, false);
                #endregion

                #region Get BatchList
                //for AddMode(true), the batchlist is not use (visible false), so we will have to clear the data in it
                //for AddMode(False) we will retrive the batchlist
                if (AddBatchMode == false)
                {
                    //DataTable dt = ReverseTransSign(dtItmBatchSelected);
                    //Get Batch actual balance from server
                    List<SqlParameter> paraList = new List<SqlParameter>();
                    paraList.Add(new SqlParameter("@DocCodeKey", CallerObjDoc.DocCodeKey));
                    paraList.Add(new SqlParameter("@DocKey", CallerObjDoc.DocKey));
                    paraList.Add(new SqlParameter("@DocSign", CallerObjDoc.DocSign));
                    paraList.Add(new SqlParameter("@ParentDocItmKey", dtParentItm.Rows[ParentrowIndex]["DocItmKey"]));
                    paraList.Add(new SqlParameter("@ParentLineType", dtParentItm.Rows[ParentrowIndex]["LineType"]));
                    paraList.Add(new SqlParameter("@ParentItmKey", objItm.ItmKey));

                    string xmlDocDetail = GFunc.ConvertDataTableToXML(dtItmBatchSelected.DefaultView.ToTable("dtDocDetail"));
                    paraList.Add(new SqlParameter("@xmlDocDetail", xmlDocDetail));
                    dtItmBatch = GFunc.ExecuteProc("Doc_BatchEntryBalance_Get", paraList);
                }

                Grid_Refresh();
                #endregion

                #region Set form layout and perform calculation
                FormLayout(false);
                ItmCount.Text = "Record " + (ParentcurrentRowIndex + 1).ToString() + " of " + ParentCount.ToString();
                lblQty.Text = TransactionSign * CallerDocSign * ParentQty < 0 ? "Qty to deduct" : "Qty to add";
                CalculateTotalSelectedQty(true);
                BatchEntryInfo_Clear();              
                #endregion

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
        private bool CalculateTotalSelectedQty(bool CalculateTotalOnly)
        {
            //Saving:
            //Calculate totalQty selected and set ParentQty to totalQty
            try
            {
                //Get TotalQty Selected
                decimal totalQty = 0;
                dtItmBatchSelected.AcceptChanges();

                totalQty = (from row in dtItmBatchSelected.AsEnumerable()
                            where row.Field<int>("LineLinkKey") == GFunc.NEInt(ParentCurrentRow["DocItmKey"], 0)
                            select row.Field<decimal>("ItmBatchQty")).Sum();

                Total.SetValueTrigger(totalQty.ToString(), false);

                if (Math.Abs(totalQty) > 0)
                {
                    //Set Parent ItmQty to totalQty
                    ItmQty.SetValueTrigger(totalQty.ToString(), false);
                }

                //Currently all caller is not running the below code as the saving of the ItmQty in the parent row is only
                //done in the "SaveAll" button click. This is to prevent undesire changes to the Parent qty when the user
                //is just scanning thru the parent and do not wish to change the parent row itmqty
                if (CalculateTotalOnly == false)
                {

                    if (ReadonlyMode)
                        return true;

                    //We need to reverse the effect of the computed parent qty when updating the values back to the parent row
                    DataRow dr = this.dtItmBatchSelected.Rows.OfType<DataRow>().ToList().Find(r => r.Field<int>("DocItmKey") == GFunc.NEInt(ParentCurrentRow["DocItmKey"], 0));
                    dr["ItmQty"] = totalQty * CallerDocSign * TransactionSign;
                    ParentCurrentRow["ItmQty"] = totalQty * CallerDocSign * TransactionSign;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option">1=check duplicate batchID in Server,2=check duplicate batchID in Server and local grid</param>
        /// <param name="batchID"></param>
        /// <returns></returns>
        private bool IsDuplicateBatchID(int option, string batchID)
        {
            try
            {
                List<SqlParameter> paraList = new List<SqlParameter>();
                paraList.Add(new SqlParameter("@Option", option));
                paraList.Add(new SqlParameter("@ItmKey", CurrentParentItmKey));
                paraList.Add(new SqlParameter("@BatchID", batchID));
                paraList.Add(new SqlParameter("@CurrentEntryLinkPointer", CurrentEntryLinkPointer));
                paraList.Add(new SqlParameter("@BatchKey", CurrentEntryBatchKey));

                string xmlDocDetail = GFunc.ConvertDataTableToXML(dtItmBatchSelected.DefaultView.ToTable("dtDocDetail", false, "BatchID", "ItmBatchQty", "BatchMfgDate", "BatchExpDate", "DocItmKey", "ItmKey"));
                paraList.Add(new SqlParameter("@xmlDocDetail", xmlDocDetail));

                SqlParameter paraOut = new SqlParameter();
                paraOut.ParameterName = "@IsDuplicate";
                paraOut.Value = 0;
                paraOut.Direction = ParameterDirection.Output;
                paraList.Add(paraOut);

                GFunc.ExecuteProc("Doc_BatchID_Validation", paraList);

                if ((int)paraOut.Value > 0)
                    return true;

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
        }//Completed
        private int LineType_Get(int ItmType)
        {
            //use when we need to get the Child LineType base on the child Item Type
            int LineType = 0;

            switch (ItmType)
            {
                case (int)GEnum.ItemType.Stock:
                case (int)GEnum.ItemType.Finished_GD:
                    LineType = 1000;
                    break;
                case (int)GEnum.ItemType.StockB:
                case (int)GEnum.ItemType.Finished_GDB:
                    LineType = 1200;
                    break;
                case (int)GEnum.ItemType.Serial_StockB:
                case (int)GEnum.ItemType.Serial_Finished_GDB:
                    LineType = 1210;
                    break;
            }

            return LineType;
        }//Completed

        public void Reload(Document objDoc, UltraGrid grdDetail, int? docItmKey)
        {
            try
            {
                ////Get Parent Batch Item List
                //dtCallerGridSource.DefaultView.RowFilter = "DocItmKey=" + docItmKey;//"LineLinkKey = 0 And LineType >= 1000 And LineType <= 1300"; 
                //dtParentItm = dtCallerGridSource.DefaultView.ToTable();
                //dtCallerGridSource.DefaultView.RowFilter = "";

                ////Get working copy grid for batch entry List
                //dtItmBatchSelected = dtCallerGridSource.DefaultView.ToTable();

                //Check for change in Caller Document if so, reassign the datatable and objects again
                if (objDoc.GUID != CallerObjDoc.GUID)
                {
                    //reassign datatable and objects
                    CallerObjDoc = objDoc;
                    CallerDC = (GEnum.SystemCode)objDoc.DocCodeKey;
                    CallerDocSign = (short)objDoc.DocSign;
                    grdCallerGrid = grdDetail;
                    dtCallerGridSource = grdDetail.DataSource as DataTable;

                    //Get a working copy of the callergrid datasource
                    dtItmBatchSelected = dtCallerGridSource.Copy();

                    //reassign Entry Mode
                    TransactionMode_Set();

                    FormLayout(false);

                }

                //Get total Parent Item
                if (dtParentItm.Rows.Count > 0)
                {
                    ParentCount = GFunc.NEInt(dtParentItm.Rows.Count, 0);
                    //Set the current parent grid active row to match the caller grid active row (synchronised activerow between the 2 grid)
                    if (grdCallerGrid.ActiveRow != null)
                    {
                        for (int i = 0; i < dtParentItm.Rows.Count; i++)
                        {
                            if ((int)dtParentItm.Rows[i]["DocItmKey"] == (int)grdCallerGrid.ActiveRow.Cells["DocItmKey"].Value)
                            {
                                ParentcurrentRowIndex = i;
                                break;
                            }
                        }
                    }
                    LoadData(ParentcurrentRowIndex);
                }
                else
                {
                    LoadData(0);
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
        private bool SaveChanges()
        {
            try
            {
                if (IsDirty)
                {
                    GEnum.MsgBoxButton btnSelect = MsgBox.Show(MsgID.Common.SaveChanges,
                                          GEnum.MsgBoxIcon.Question,
                                          GEnum.MsgBoxButton.Save_Changes,
                                          GEnum.MsgBoxButton.Discard_Changes,
                                          GEnum.MsgBoxButton.I_Dont_Know);

                    if (btnSelect == GEnum.MsgBoxButton.I_Dont_Know)
                        return false;
                    else if (btnSelect == GEnum.MsgBoxButton.Discard_Changes)

                        return true;

                    return Save();
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
        private bool Save()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                tagrdItmBatch.PerformAction(UltraGridAction.ExitEditMode);
                tagrdItmBatch.UpdateData();
                tagrdItmBatchSelected.PerformAction(UltraGridAction.ExitEditMode);
                tagrdItmBatchSelected.UpdateData();

                if (SavingValidation() == false)
                    return false;

                if (CalculateTotalSelectedQty(false) == false)
                    return false;

                //We need to reverse the effect of the computed batchSelected qty when updating the values back to the callergrid
                //Only if it is not from PD, we'll update value

                bool runPDDKCheck = false;

                if ((CallerObjDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Invoice || CallerObjDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Credit_Note || CallerObjDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Debit_Note))
                    runPDDKCheck = true;

                foreach (DataRow dr in dtItmBatchSelected.Rows)
                {
                    if (runPDDKCheck)
                    {
                        if (GFunc.IsNEZ(dr["APPDDK"]))
                        {
                            dr["ItmBatchQty"] = (decimal)dr["ItmBatchQty"] * CallerDocSign * TransactionSign;
                        }
                    }
                    else
                        dr["ItmBatchQty"] = (decimal)dr["ItmBatchQty"] * CallerDocSign * TransactionSign;

                }
                dtItmBatchSelected.AcceptChanges();

                //update parent qty
                foreach (DataRow dr in dtParentItm.Rows)
                {
                    decimal vChildQtyTotal = GFunc.NEDec(dtItmBatchSelected.Compute("Sum(ItmBatchQty)", "LineLinkKey =" + dr["DocItmKey"].ToString()), 0.00M);
                    DataRow drBatchSelected = this.dtItmBatchSelected.Rows.OfType<DataRow>().ToList().Find(r => r.Field<int>("DocItmKey") == GFunc.NEInt(dr["DocItmKey"], 0));

                    drBatchSelected["ItmQty"] = vChildQtyTotal;
                }
                dtItmBatchSelected.AcceptChanges();

                //Prepare a Datatable that contains all data modification(added,edited,deleted,Unchanged)
                DataTable dtBatchOriginal = dtCallerGridSource.Select("(LineLinkKey = 0 AND ItmType In(110,210,310,410)) OR (LineLinkKey > 0 AND LineType IN(1200,1210,1300))").CopyToDataTable();
                dtBatchOriginal.PrimaryKey = new DataColumn[] { dtBatchOriginal.Columns["DocItmKey"] };
                DataTable dtBatchModified = dtItmBatchSelected.Select("(LineLinkKey = 0 AND ItmType In(110,210,310,410)) OR (LineLinkKey > 0 AND LineType IN(1200,1210,1300))").CopyToDataTable();
                GFunc.TAMerge(ref dtBatchOriginal, dtBatchModified);

                //Merge the Modified Data into the Caller DataSource
                dtCallerGridSource.PrimaryKey = new DataColumn[] { dtCallerGridSource.Columns["DocItmKey"] };
                dtCallerGridSource.Merge(dtBatchOriginal);
                dtCallerGridSource.PrimaryKey = null;
                dtCallerGridSource.AcceptChanges();

                if (grdCallerGrid.ActiveCell != null)
                {
                    //for situation where we have an active cell
                    grdCallerGrid.ActiveCell.Value = grdCallerGrid.ActiveCell.Value;
                }
                else
                {
                    if (grdCallerGrid.ActiveRow != null)
                    {
                        //for situation where we have active row but w/o a ActiveCell
                        grdCallerGrid.ActiveRow.Cells["DocItmKey"].Value = grdCallerGrid.ActiveRow.Cells["DocItmKey"].Value;
                    }
                    else
                    {
                        //for situation where there are no activeRow, we will use the first row in the grid as
                        //the first row[0] will never be hidden (parent is always before a child)
                        grdCallerGrid.ActiveRow = grdCallerGrid.Rows[0];
                        grdCallerGrid.ActiveRow.Cells["DocItmKey"].Value = grdCallerGrid.ActiveRow.Cells["DocItmKey"].Value;
                    }
                }
                grdCallerGrid.ActiveRow.Update();
                switch (CallerObjDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                        DocDetUtil.ItmQty_CustomUpdate(CallerObjDoc, grdCallerGrid);
                        break;
                }
                this.IsDirty = false;
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private bool SavingValidation()
        {
            //Validation:
            //check for duplicate new batchID with the following search:
            //search for duplicate batcID in BatchSelected
            //Search for duplicate batchID in MSTItmBatch
            try
            {
                //check for duplicate BatchID in BatchSelected and MSTItmBatch
                IEnumerable<DataRow> dtItmBatchFilter = dtItmBatchSelected.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == GFunc.NEInt(ParentCurrentRow["DocItmKey"], 0));


                //----------------------------------keep for reference in future------------------------------------------------
                //DataTable dublicateBatch = (from rowSection in dtItmBatchSelected.DefaultView.ToTable().AsEnumerable()
                //                            group rowSection by rowSection.Field<string>("BatchID") into dublicateBatchID
                //                            join row in dtItmBatchSelected.DefaultView.ToTable().AsEnumerable()
                //                                  on dublicateBatchID.Key equals row.Field<string>("BatchID")
                //                            where dublicateBatchID.Count() > 0
                //                            select row).AsDataTable(); 
                //--------------------------------------------------------------------------------------------------------

                DataTable dublicateBatch = (from row in dtItmBatchFilter
                                            group row by row.Field<string>("BatchID") into dublicateBatchID
                                            where dublicateBatchID.Count() > 1
                                            select new
                                            {
                                                BatchID = dublicateBatchID.Key
                                            }
                                            ).AsDataTable();

                if (dublicateBatch.Rows.Count > 0)
                {
                    MsgBoxGrid.Show("There are one or more duplicate batch id", dublicateBatch, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
                    return false;
                }
                List<SqlParameter> paraList = new List<SqlParameter>();
                paraList.Add(new SqlParameter("@Option", 3));

                dublicateBatch = (from row in dtItmBatchFilter
                                  select new
                                  {
                                      BatchID = row.Field<string>("BatchID"),
                                      ItmBatchQty = row.Field<decimal>("ItmBatchQty"),
                                      BatchMfgDate = row.Field<DateTime?>("BatchMfgDate"),
                                      BatchExpDate = row.Field<DateTime?>("BatchExpDate")
                                  }).AsDataTable();
                dublicateBatch.TableName = "dtDocDetail";

                string xmlDocDetail = GFunc.ConvertDataTableToXML(dublicateBatch);
                paraList.Add(new SqlParameter("@xmlDocDetail", xmlDocDetail));

                SqlParameter paraOut = new SqlParameter();
                paraOut.ParameterName = "@IsDuplicate";
                paraOut.Value = 0;
                paraOut.Direction = ParameterDirection.Output;
                paraList.Add(paraOut);

                dublicateBatch = GFunc.ExecuteProc("Doc_BatchID_Validation", paraList);
                if (dublicateBatch.Rows.Count > 0)
                {
                    MsgBoxGrid.Show("There are one or more duplicate batch id", dublicateBatch, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
                    return false;
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
            return true;
        }//Completed
        private bool TransactionMode_Set()
        {
            //Get Entry Mode
            switch (CallerDC)
            {
                case GEnum.SystemCode.Purchase_Invoice:
                case GEnum.SystemCode.Purchase_Debit_Note:
                case GEnum.SystemCode.Purchase_Delivery:
                    AddBatchMode = true;
                    TransactionSign = 1;
                    break;

                case GEnum.SystemCode.Purchase_Credit_Note:
                    AddBatchMode = false;
                    TransactionSign = 1;
                    break;

                case GEnum.SystemCode.Inventory_Adjustment:
                    if (CallerObjDoc.DocType == 400)//Add New Batch 
                    {
                        AddBatchMode = true;
                        TransactionSign = 1;
                    }
                    else
                    {
                        AddBatchMode = false;
                        TransactionSign = 1;
                    }
                    break;

                case GEnum.SystemCode.Delivery_Order:
                case GEnum.SystemCode.Sales_Invoice:
                case GEnum.SystemCode.Sales_Debit_Note:
                case GEnum.SystemCode.Sales_Credit_Note:
                case GEnum.SystemCode.Cash_Sale:
                case GEnum.SystemCode.Cash_Debit_Note:
                case GEnum.SystemCode.Cash_Credit_Note:
                case GEnum.SystemCode.Issue_Consignment:
                case GEnum.SystemCode.Return_Consignment:
                case GEnum.SystemCode.Inventory_Transfer:
                    AddBatchMode = false;
                    TransactionSign = -1;
                    break;
            }
            return true;
        }//Completed

        //Set Error Methods
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
                throw Error(tex, true);
            }
            catch (Exception ex)
            {
                throw Error(ex, true);
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

        private void frmBatchEntry_Activated(object sender, EventArgs e)
        {
            if (AddBatchMode)
            {
                BatchID.Focus();
            }
            else
                ItmQty.Focus();
        }

    }
}
