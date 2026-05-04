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
using Infragistics.Win.UltraWinEditors;
using TAUtil;

namespace WinUI
{
    public partial class frmBatchSelection : Form
    {
        #region Local Variables

        string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        private bool ReadonlyMode = false;
        private bool AddBatchMode = false;          //TRUE - Only New Batch Entry is Allow (FGB), FALSE - Only Batch Qty (Add or Deduct) is allowed
        private bool FGMode = false;                //TRUE - LineType = 3000

        private Document CallerObjDoc = null;
        private GEnum.SystemCode CallerDC = 0;      //When CallerDC = 0 mean ReadOnly Mode (no add/edit of emtry) else other value will represent Caller DocCodeKey
        private short CallerDocSign = 0;
        private DataTable dtParentItm = null;       //Datatable to store the list of Parent Item of all items (LineType =3000,3100,3200,3300, LineLinkKey = 0)
        private DataTable dtCallerGridSource = null;
        private UltraGrid grdCallerGrid = null;

        private int ParentcurrentRowIndex = 0;
        private DataRow ParentCurrentRow;
        private int ParentCount = 0;
        private DataTable dtItmBatch = null;            //Datasource for Batch List Grid
        private DataTable dtItmBatchSelected = null;    //Datasource for Batch Selected Grid
        private MSTItm objItm = null;                   //MSTItm Object for Parent Item

        private bool IsDirty = false;
        private int CurrentParentItmKey = 0;
        private int CurrentEntryBatchKey = 0;           //Current BatchKey for BatchEntry Infor
        private int CurrentEntryLinkPointer = 0;        //Current DocItmKey of BatchEntry Infor that is related to BatchSelected Grid                           

        //DataTable dtItmBatch = null;
        private DataTable _dtMaterialBatch = null;

        #endregion

        //Initialize
        public frmBatchSelection()
        {
            InitializeComponent();
        }
        public frmBatchSelection(Document objDoc, UltraGrid grdDetail, bool bReadOnlyMode)
        {
            //For Add/Edit
            //For Readonly mode - user dblClick on the Grid ItmDes (Currently this is not use, for future use)
            //For Add/Edit mode - user dblClick on the Grid ItmQty
            InitializeComponent();
            grdDetail.UpdateData();
            ReadonlyMode = bReadOnlyMode;
            IsDirty = false;

            CallerObjDoc = objDoc;
            CallerDC = (GEnum.SystemCode)objDoc.DocCodeKey;
            CallerDocSign = (short)objDoc.DocSign;
            grdCallerGrid = grdDetail;
            dtCallerGridSource = grdDetail.DataSource as DataTable;

            if (CallerObjDoc.IsReadOnly)
                ReadonlyMode = true;

        }//Completed

        //Form Events
        private void frm_Activated(object sender, EventArgs e)
        {
            if (GFunc.GetIntPropertyValue("DocProDetails", CallerObjDoc) == 10) //Finish Goods
                FGProduceQty.Focus();
            else
                BOMIssue.Focus();
        }//Completed

        private void SetParentItm()
        {
            //Get parent row for finished goods only
            int[] lineTypes = new int[] { 3000, 3100, 3200, 3300 };
            //&& lineTypes.Contains(r.Field<int>("LineType"))
            dtParentItm = dtCallerGridSource.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == 0 && lineTypes.Contains(r.Field<int>("LineType"))).CopyToDataTable();
            dtParentItm.DefaultView.Sort = "LineType,ItmSN Asc";
            ParentCount = GFunc.NEInt(dtParentItm.Rows.Count, 0);
        }

        private void frm_Load(object sender, EventArgs e)
        {
            try
            {                
                //Form and Grid Format
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0, this.Name);
                GlobalUI.Combos_Fill(this, (int)GEnum.SystemCode.Inventory_Production);
                FormLayout(true);

                //Get a working copy of the callergrid datasource
                SetParentItm();
                dtItmBatchSelected = dtCallerGridSource.Copy();

                //Validation - Close form when there are no batch item in Caller grid
                if (ParentCount <= 0)
                {
                    MsgBox.Show("There are no item in this document");
                    this.Close();
                    return;
                }

                //Assign CallerGrid AutoNumber to BatchSelectedGrid's Autonumber
                tagrdBatchRaw.DetailObjectKey = DocComUtility.GridAutoID_Get(grdCallerGrid, "DocKey", "DocItmKey") - 1;

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
                this.Cursor = Cursors.Default;
            }

        }//Completed
        private void frm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)CallerDC);
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
                //Common setting for grids
                tagrdBatchList.DisplayLayout.Bands[0].Override.AllowAddNew = AllowAddNew.No;
                tagrdBatchList.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                tagrdBatchList.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.False;
                tagrdBatchRaw.DisplayLayout.Bands[0].Override.AllowAddNew = AllowAddNew.No;
                tagrdBatchRaw.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                tagrdBatchFG.DisplayLayout.Bands[0].Override.AllowAddNew = AllowAddNew.No;
                tagrdBatchFG.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;


                if (FormLoad)
                {
                    //Common setting for Entry controls
                    ClearEntryData();

                    if (ReadonlyMode)
                    {
                        #region Set Control as readonly
                        //Finshed Goods
                        FGReq.Enabled = false;
                        FGProduceQty.Enabled = false;
                        FGProduceWeight.Enabled = false;
                        FGOverHeadKey.Enabled = false;
                        FGOverHeadCost.Enabled = false;
                        FGOverHeadAmtH.Enabled = false;
                        BatchNoFormat.Enabled = false;
                        InitialNumber.Enabled = false;
                        NoOfBatch.Enabled = false;
                        QtyEachBatch.Enabled = false;
                        btnGenerate.Visible = false;


                        //Material
                        BOMReq.Enabled = false;
                        BOMIssue.Enabled = false;
                        BOMReturn.Enabled = false;
                        btnAssign.Enabled = false;

                        //Entry Control
                        BatchID.Enabled = false;
                        BatchQty.Enabled = false;
                        MFGDate.Enabled = false;
                        ExpDate.Enabled = false;

                        btnSaveAll.Visible = false;
                        tagrdBatchFG.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.False;
                        tagrdBatchRaw.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.False;

                        Total.Enabled = false;
                        #endregion
                    }
                    else
                    {
                        tagrdBatchFG.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.True;
                        tagrdBatchRaw.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.True;
                    }
                    tpFGInfor.Tab.Visible = true;
                    tpMaterial.Tab.Visible = false;
                    btnGenerate.Visible = false;
                    return;
                }

                #region Set Layout
                if (FGMode)
                {
                    tpFGInfor.Tab.Visible = true;
                    tpMaterial.Tab.Visible = false;
                    tpFGInfor.Focus();

                    lblBatchNoFormat.Visible = AddBatchMode;
                    lblInitialNumber.Visible = AddBatchMode;
                    lblNoOfBatch.Visible = AddBatchMode;
                    lblQtyEachBatch.Visible = AddBatchMode;
                    BatchNoFormat.Visible = AddBatchMode;
                    InitialNumber.Visible = AddBatchMode;
                    NoOfBatch.Visible = AddBatchMode;
                    QtyEachBatch.Visible = AddBatchMode;
                    btnGenerate.Visible = AddBatchMode;
                    Total.Visible = AddBatchMode;
                    pnlBatchEntryFG.Visible = AddBatchMode;
                    tagrdBatchFG.Visible = AddBatchMode;

                }
                else
                {
                    tpFGInfor.Tab.Visible = false;
                    tpMaterial.Tab.Visible = true;
                    tpMaterial.Focus();

                    if (GFunc.IsBatchItmType(ParentCurrentRow["ItmType"]))
                    {
                        btnAssign.Visible = true;
                        pnlBatchEntryRaw.Visible = true;
                        tagrdBatchRaw.Visible = true;
                        tagrdBatchList.Visible = true;
                        Total.Visible = true;
                    }
                    else
                    {
                        btnAssign.Visible = false;
                        pnlBatchEntryRaw.Visible = false;
                        tagrdBatchRaw.Visible = false;
                        tagrdBatchList.Visible = false;
                        Total.Visible = false;
                    }

                }
                #endregion

                if (GFunc.GetIntPropertyValue("DocProDetails", CallerObjDoc) == 10) //Finish Goods
                    FGProduceQty.Focus();
                else
                    BOMIssue.Focus();

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
                tagrdBatchList.DataSource = dtItmBatch;
                tagrdBatchList.Refresh();

                string ParentDocItmKey = GFunc.NEStr(ParentCurrentRow["DocItmKey"].ToString(), string.Empty);
                dtItmBatchSelected.DefaultView.RowFilter = "LineLinkKey="+ParentDocItmKey;
                
                if (FGMode)
                {
                    tagrdBatchFG.DataSource = dtItmBatchSelected;
                    tagrdBatchFG.Refresh();
                }
                else
                {
                    tagrdBatchRaw.DataSource = dtItmBatchSelected;
                    tagrdBatchRaw.Refresh();
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
                //First clear All Data Entry controls
                ClearEntryData();

                #region Update Control Values and get ParentRow and set Flag
                ParentCurrentRow = dtParentItm.Rows[ParentrowIndex];
                CurrentParentItmKey = (int)ParentCurrentRow["ItmKey"];
                objItm = MSTItm.Get(CurrentParentItmKey);

                //Set AddBatchMode and FGMode
                if ((GEnum.RecDetailType)ParentCurrentRow["LineType"] == GEnum.RecDetailType.DItmFinishedGoods)
                {
                    AddBatchMode = GFunc.IsBatchItmType(objItm.ItmType);
                    FGMode = true;
                }
                else
                {
                    AddBatchMode = false;
                    FGMode = false;
                }
                #endregion

                #region Update entryInfo
                ItmID.SetValueTrigger(objItm.ItmID, false);
                ItmDes.SetValueTrigger(objItm.ItmDes, false);
                FinishedGoodID.SetValueTrigger(ParentCurrentRow["ItmFGID"].ToString(), false);
                ItmSN.Text = ParentCurrentRow["ItmSN"].ToString();

                ParentInfo_Set();

                #endregion

                #region Get BatchList
                //for AddMode(true), the batchlist is not use (visible false), so we will have to clear the data in it
                //for AddMode(False) we will retrive the batchlist
                if (AddBatchMode == false && GFunc.IsBatchItmType(objItm.ItmType))
                {
                    DataTable dt = ReverseTransSign(dtItmBatchSelected);
                    //Get Batch actual balance from server
                    List<SqlParameter> paraList = new List<SqlParameter>();
                    paraList.Add(new SqlParameter("@DocCodeKey", CallerObjDoc.DocCodeKey));
                    paraList.Add(new SqlParameter("@DocKey", CallerObjDoc.DocKey));
                    paraList.Add(new SqlParameter("@DocSign", CallerObjDoc.DocSign));
                    paraList.Add(new SqlParameter("@ParentDocItmKey", dtParentItm.Rows[ParentrowIndex]["DocItmKey"]));
                    paraList.Add(new SqlParameter("@ParentLineType", dtParentItm.Rows[ParentrowIndex]["LineType"]));
                    paraList.Add(new SqlParameter("@ParentItmKey", objItm.ItmKey));

                    string xmlDocDetail = GFunc.ConvertDataTableToXML(dt.DefaultView.ToTable("dtDocDetail"));
                    paraList.Add(new SqlParameter("@xmlDocDetail", xmlDocDetail));
                    dtItmBatch = GFunc.ExecuteProc("Doc_BatchEntryBalance_Get", paraList);
                }
                else
                {
                    if (dtItmBatch != null)
                        dtItmBatch.Rows.Clear();
                }

                Grid_Refresh();
                #endregion

                #region Set form layout and perform calculation
                FormLayout(false);
                CalculateTotal();
                ItmCount.Text = "Record " + (ParentcurrentRowIndex + 1).ToString() + " of " + ParentCount.ToString();
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

        //Control events        
        private void FGProduceQty_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (ValidateQty(FGProduceQty.Value))
                {
                    FGProduceQty.SetValueTrigger(GFunc.RndC(FGProduceQty.DecimalValue, GVar.RndDecs.Qtypt), false);
                    FGOverHeadAmtH.SetValueTrigger(GFunc.RndC(FGOverHeadCost.DecimalValue * FGProduceQty.DecimalValue, GVar.RndDecs.Amtpt), false);
                    this.IsDirty = true;
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
        private void FGOverHeadCost_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (ValidateQty(FGOverHeadCost.Value))
                {
                    FGOverHeadCost.SetValueTrigger(GFunc.RndC(FGOverHeadCost.DecimalValue, GVar.RndDecs.COSpt), false);
                    FGOverHeadAmtH.SetValueTrigger(GFunc.RndC(FGOverHeadCost.DecimalValue * FGProduceQty.DecimalValue, GVar.RndDecs.Amtpt), false);
                    this.IsDirty = true;
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
        private void FGOverHeadKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                //
               // DataTable dtOverHead = FGOverHeadKey.DataSource as DataTable;
               // dtOverHead.DefaultView.RowFilter = "OverHeadKey=" + GFunc.NEInt(FGOverHeadKey.Value, 0);
                IEnumerable<DataRow> dtOverHeadRows = (FGOverHeadKey.DataSource as DataTable).AsEnumerable().Where(r => r.Field<int>("OverHeadKey") == GFunc.NEInt(FGOverHeadKey.Value, 0));

                FGOverHeadCost.SetValueTrigger(GFunc.NEDec(dtOverHeadRows.ElementAt(0)["OverHeadCost"], 0), false);
                FGOverHeadAmtH.SetValueTrigger(GFunc.RndC(FGOverHeadCost.DecimalValue * FGProduceQty.DecimalValue, GVar.RndDecs.Amtpt), false);
               // dtOverHead.DefaultView.RowFilter = "";
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
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }//Completed        
        private void FGBatchID_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (GFunc.IsNE(FGBatchID.Value))
                    return;

                if (FGBatchInfo_Search(GFunc.NEStr(FGBatchID.Value, string.Empty)) == false)
                {
                    FGBatchEntryInfo_Clear();
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
        private void FGBatchQty_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (ValidateQty(FGBatchQty.Value))
                {
                    FGBatchQty.SetValueTrigger(GFunc.RndC(FGBatchQty.DecimalValue, GVar.RndDecs.Qtypt), false);
                    btnFGBatchUpdate.Focus();
                    IsDirty = true;
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
        private void FGMFGDate_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                DateTime mfgDate;
                string defExpDate = objItm.DefaultExpDate;

                if (GFunc.IsNE(FGMFGDate.DateValue) == false)
                {
                    mfgDate = (DateTime)FGMFGDate.DateValue;
                    if (defExpDate.Length > 0)
                    {
                        int interval = int.Parse(defExpDate.Substring(0, defExpDate.Length - 1));
                        string dateType = defExpDate.Substring(defExpDate.Length - 1, 1);

                        switch (dateType.ToLower())
                        {
                            case "w"://Week
                                FGExpDate.SetValueTrigger(mfgDate.AddDays(interval * 7), false);
                                break;
                            case "m"://Month
                                FGExpDate.SetValueTrigger(mfgDate.AddMonths(interval), false);
                                break;
                            case "y"://Year
                                FGExpDate.SetValueTrigger(mfgDate.AddYears(interval), false);
                                break;
                        }
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
        private void FGExpDate_CustomUpdate(object sender, CancelEventArgs e)
        {
            IsDirty = true;
        }//Completed

        private void BatchID_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {

                if (GFunc.IsNE(BatchID.Value))
                    return;

                if (RawBatchInfo_Search(GFunc.NEStr(BatchID.Value, string.Empty)) == false)
                {
                    RawBatchEntryInfo_Clear();
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
        private void BatchQty_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (ValidateQty(BatchQty.Value))
                {
                    BatchQty.SetValueTrigger(GFunc.RndC(BatchQty.DecimalValue, GVar.RndDecs.Qtypt), false);
                    btnRawBatchUpdate.Focus();
                    IsDirty = true;
                }
                else
                    e.Cancel = false;
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
        private void tagrdBatchFG_AfterRowsDeleted(object sender, EventArgs e)
        {
            try
            {
                CalculateTotal();
                FGBatchEntryInfo_Clear();
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
        private void tagrdBatchFG_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                e.DisplayPromptMsg = false;

                if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) == GEnum.MsgBoxButton.Delete)
                {
                    if (tagrdBatchFG.Rows.Count == 0)
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
        private void tagrdBatchFG_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            try
            {
                FGBatchEntryInfo_Clear();
                if (tagrdBatchFG.ActiveRow != null)
                {
                    UltraGridRow row = tagrdBatchFG.ActiveRow;
                    FGBatchEntryInfo_Set((int)row.Cells["ItmBatchKey"].Value, row.Cells["BatchID"].Value.ToString(), (decimal)row.Cells["ItmBatchQty"].Value, row.Cells["BatchMfgDate"].Value, row.Cells["BatchExpDate"].Value, (int)row.Cells["DocItmKey"].Value);
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

        private void tagrdBatchList_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {

            try
            {
                if (this.ReadonlyMode)
                    return;

                if (tagrdBatchList.ActiveRow != null)
                {
                    UltraGridRow grow = tagrdBatchList.ActiveRow;

                    //We cannot allow a batch that has already been selected to be inserted into the Batch Selected List again
                    //as this would cause duplication error
                    //Therefore we will try to find if it already exist in BatchSelect and select it.
                    if (RawBatchSelected_Get(grow.Cells["BatchID"].Value.ToString()))
                        return;

                    decimal reqQty = BOMUsed.DecimalValue;
                    decimal assignQty = 0;
                    decimal balQty = (decimal)grow.Cells["BatchQtyBal"].Value;

                    //Get BatchSelected TotalQty
                    decimal totalQty = (from row in dtItmBatchSelected.AsEnumerable()
                                        where row.Field<int>("LineLinkKey") == (int)ParentCurrentRow["DocItmKey"]
                                        select row.Field<decimal>("ItmBatchQty")).Sum();

                    //Calculate Qty to assign to BatchSelect list. We will always insert this batch to the BatchSelect if not already inserted
                    //For Deduct Qty (-ve) - we will calculate the BatchQty to assign.
                    //For Add Qty (+ve) - the BatchQty to assign is 0.
                    if (reqQty > 0)//deduct qty
                    {
                        //In INMFN, all qty is always +ve, the formula here is different from BatchEntry Form formula
                        if (reqQty > totalQty)
                            reqQty = reqQty - totalQty;
                        else
                            reqQty = 0;

                        //Calculate the qty to assign
                        if (reqQty > balQty)
                            assignQty = balQty;
                        else
                            assignQty = reqQty;
                    }
                    else
                        assignQty = 0;

                    RawBatchSelected_Add((int)grow.Cells["BatchKey"].Value, grow.Cells["BatchID"].Value.ToString(), assignQty, grow.Cells["BatchMfgDate"].Value, grow.Cells["BatchExpDate"].Value, true);
                    IsDirty = true;
                    BatchQty.Focus();
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
        private void tagrdBatchRaw_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                e.DisplayPromptMsg = false;

                if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) == GEnum.MsgBoxButton.Delete)
                {
                    if (tagrdBatchRaw.Rows.Count == 0)
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
        private void tagrdBatchRaw_AfterRowsDeleted(object sender, EventArgs e)
        {
            try
            {
                CalculateTotal();
                RawBatchEntryInfo_Clear();
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
        private void tagrdBatchRaw_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            try
            {

                RawBatchEntryInfo_Clear();
                if (tagrdBatchRaw.ActiveRow != null)
                {
                    UltraGridRow row = tagrdBatchRaw.ActiveRow;
                    RawBatchEntryInfo_Set((int)row.Cells["ItmBatchKey"].Value, row.Cells["BatchID"].Value.ToString(), (decimal)row.Cells["ItmBatchQty"].Value, row.Cells["BatchMfgDate"].Value, row.Cells["BatchExpDate"].Value, (int)row.Cells["DocItmKey"].Value);
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

        //Button Event        
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string msgID = string.Empty;

                if (GFunc.IsNE(BatchNoFormat.Value) || GFunc.IsNEZ(NoOfBatch.Value))
                {
                    msgID = "Batch No Format cannot be empty.";
                }
                if (GFunc.IsNE(InitialNumber.Value))
                {
                    msgID += "\n Initial Number of Batch cannot be zero or Blank";
                }
                if (GFunc.IsNE(NoOfBatch.Value))
                {
                    msgID += "\n No of Batch cannot be zero or Blank";
                }
                if (GFunc.IsNE(QtyEachBatch.Value))
                {
                    msgID += "\n Quantity in Each Batch cannot be zero or Blank";
                }
                if (msgID != string.Empty)
                {
                    MsgBox.Show(msgID);
                    return;
                }
                else
                {
                    #region Clear all child data in BatchSelected grid

                    FGBatchEntryInfo_Clear();

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

                    int interval = GFunc.NEInt(NoOfBatch.Value, 0) - GFunc.NEInt(InitialNumber.Value, 0);
                    int NewDocItmKey = DocComUtility.GridAutoID_Get(tagrdBatchFG, "DocKey", "DocItmKey");
                    for (int i = 0; i < GFunc.NEInt(NoOfBatch.Value, 0); i++)
                    {
                        string number = (GFunc.NEInt(InitialNumber.Value, 0) + i).ToString();
                        string batchID = BatchNoFormat.Value.ToString() + number.PadLeft(interval.ToString().Length + 1, '0');

                        FGBatchSelected_Add(CurrentEntryBatchKey, batchID, QtyEachBatch.DecimalValue,
                            DateTime.Today.Date, DateTime.Today.Date.AddYears(1), false);

                    }
                }
                tagrdBatchFG.DataSource = dtItmBatchSelected;
                tagrdBatchFG.DataBind();
                CalculateTotal();

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
                if (AddBatchMode == false && GFunc.IsBatchItmType(objItm.ItmType))
                {
                    decimal reqQty = BOMUsed.DecimalValue;

                    #region Validation
                    if (reqQty <= 0)
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
                        RawBatchSelected_Add((int)dr["BatchKey"], GFunc.NEStr(dr["BatchID"], string.Empty), assignQty, dr["BatchMfgDate"], dr["BatchExpDate"], false);
                    }

                    RawBatchEntryInfo_Clear();
                    dtItmBatchSelected.AcceptChanges();
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
        }//Completed 
        private void btnNext_Click(object sender, EventArgs e)
        {

            try
            {
                if (CalculateTotal())
                {
                    UpdateCallerDetailInfo();
                    if (ParentcurrentRowIndex < dtParentItm.Rows.Count - 1)
                    {
                        ParentcurrentRowIndex++;
                        LoadData(ParentcurrentRowIndex);
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
        }//Completed 
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                if (CalculateTotal())
                {
                    UpdateCallerDetailInfo();
                    if (ParentcurrentRowIndex > 0)
                    {
                        ParentcurrentRowIndex--;
                        LoadData(ParentcurrentRowIndex);
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
        }//Completed 
        private void btnRawBatchUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                RawBatchEntryInfo_Update();
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
        private void btnFGBatchUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                FGBatchEntryInfo_Update();
                FGBatchID.Focus();
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

        //Functions (material)
        private void RawBatchEntryInfo_Clear()
        {

            CurrentEntryBatchKey = 0;
            CurrentEntryLinkPointer = 0;
            BatchID.SetValueTrigger("", false);
            BatchQty.SetValueTrigger("", false);
            MFGDate.SetValueTrigger(null, false);
            ExpDate.SetValueTrigger(null, false);
            if (ReadonlyMode == false)
                BatchID.Enabled = true;
        }//Completed
        private void RawBatchEntryInfo_Set(int BatchKey, string batchID, decimal batQty, object mfgDate, object expDate, int BatchSelectedDocItmKey)
        {
            try
            {
                BatchID.SetValueTrigger(batchID, false);
                BatchQty.SetValueTrigger(batQty, false);
                if (GFunc.IsNE(mfgDate))
                    MFGDate.SetValueTrigger(DBNull.Value, false);
                else
                    MFGDate.SetValueTrigger(GFunc.NEDateTime(mfgDate, DBNull.Value), false);

                if (GFunc.IsNE(expDate))
                    ExpDate.SetValueTrigger(DBNull.Value, false);
                else
                    ExpDate.SetValueTrigger(GFunc.NEDateTime(mfgDate, DBNull.Value), false);
                CurrentEntryBatchKey = BatchKey;
                CurrentEntryLinkPointer = BatchSelectedDocItmKey;
                
                    BatchID.Enabled = !ReadonlyMode;
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
        private void RawBatchEntryInfo_Update()
        {
            try
            {
                if (GFunc.IsBatchItmType(objItm.ItmType) == false)
                    return;

                if (RawBatchEntryInfo_Validation() == false)
                    return;

                if (AddBatchMode == false)
                {
                    #region Add or Deduct Batch Qty Mode
                    UltraGridRow gRow = this.tagrdBatchRaw.Rows.OfType<UltraGridRow>().ToList().Find(
                        row => row.Cells["DocItmKey"].Text.Equals(CurrentEntryLinkPointer.ToString(), StringComparison.CurrentCultureIgnoreCase));

                    gRow.Cells["ItmBatchQty"].Value = BatchQty.DecimalValue;
                    tagrdBatchRaw.UpdateData();
                    #endregion
                }

              //  dtItmBatchSelected.DefaultView.RowFilter = "LineLinkKey =" + ParentCurrentRow["DocItmKey"].ToString();
                CalculateTotal();
                RawBatchEntryInfo_Clear();
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
        private bool RawBatchEntryInfo_Validation()
        {
            try
            {

                if (GFunc.IsBatchItmType(objItm.ItmType) == false)
                    return true;

                decimal batchQty = BatchQty.DecimalValue;
                string batchID = GFunc.NEStr(this.BatchID.Value, string.Empty);

                if (AddBatchMode == false)
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

                    if (BatchQtyBal - batchQty < 0)
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
        private bool RawBatchSelected_Get(string batchID)
        {
            //Search for a match in BatchSelect grid and Set it to the BatchEntry Controls
            try
            {

                UltraGridRow grdRow = this.tagrdBatchRaw.Rows.OfType<UltraGridRow>().ToList()
                            .Find(r => r.Cells["BatchID"].Value.ToString().Equals(batchID, StringComparison.CurrentCulture)
                                && (int)r.Cells["ItmKey"].Value == CurrentParentItmKey
                                && (int)r.Cells["LineLinkKey"].Value == (int)ParentCurrentRow["DocItmKey"]);

                if (grdRow != null)
                {
                    this.tagrdBatchRaw.ActiveRow = grdRow;
                    RawBatchEntryInfo_Set((int)grdRow.Cells["ItmBatchKey"].Value, batchID, (decimal)grdRow.Cells["ItmBatchQty"].Value, grdRow.Cells["BatchMfgDate"].Value, grdRow.Cells["BatchExpDate"].Value, (int)grdRow.Cells["DocItmKey"].Value);
                    tagrdBatchRaw.ActiveRowScrollRegion.ScrollRowIntoView(grdRow);
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
        private bool RawBatchSelected_Add(int BatchKey, string batchID, decimal assignQty, object mfgDate, object expDate, bool SetBatchEntry)
        {
            try
            {

                DataRow drSelect = dtItmBatchSelected.NewRow();

                //copy parent row data to a new row in BatchSelected
                //note: the parent and batchselect has the same structure.
                foreach (DataColumn dc in dtItmBatchSelected.Columns)
                {
                    drSelect[dc.ColumnName] = ParentCurrentRow[dc.ColumnName];
                }

                //update require information to new child row.
                int NewDocItmKey = DocComUtility.GridAutoID_Get(tagrdBatchRaw, "DocKey", "DocItmKey");
                drSelect["BatchID"] = batchID;
                drSelect["DocItmKey"] = NewDocItmKey;
                drSelect["ItmBatchKey"] = BatchKey;
                drSelect["LineType"] = LineType_Get();
                drSelect["LineLinkKey"] = ParentCurrentRow["DocItmKey"];
                drSelect["ItmBatchQty"] = assignQty;
                drSelect["BatchMfgDate"] = mfgDate == null ? DBNull.Value : mfgDate;
                drSelect["BatchExpDate"] = expDate == null ? DBNull.Value : expDate;

                //Set ItmDetSN By Incrementing
                //dtItmBatchSelected.DefaultView.RowFilter = "LineLinkKey=" + GFunc.NEInt(ParentCurrentRow["DocItmKey"], 0);
                IEnumerable<DataRow> dtItmBatchFilter = dtItmBatchSelected.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == GFunc.NEInt(ParentCurrentRow["DocItmKey"], 0));
                if (dtItmBatchFilter.Count() > 0)
                    //drSelect["ItmDetSN"] = dtItmBatchSelected.Rows.Count == 0 ? 1 : dtItmBatchSelected.AsEnumerable().Where(p => p.Field<int>("LineLinkKey") == GFunc.NEInt(ParentCurrentRow["DocItmKey"], 0)).Max(p => p.Field<decimal>("ItmDetSN")) + 1;
                    drSelect["ItmDetSN"] = dtItmBatchFilter.Max(p => p.Field<decimal>("ItmDetSN")) + 1;
                else
                    drSelect["ItmDetSN"] = 1;

               // dtItmBatchSelected.DefaultView.RowFilter = "";
                dtItmBatchSelected.Rows.Add(drSelect);
                dtItmBatchSelected.AcceptChanges();

                CalculateTotal();

                if (SetBatchEntry)
                    RawBatchEntryInfo_Set(BatchKey, batchID, assignQty, mfgDate, expDate, NewDocItmKey);

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
        }//Completed
        private bool RawBatchInfo_Search(string batchID)
        {
            try
            {
                if (GFunc.IsBatchItmType(objItm.ItmType) == false)
                    return true;

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
                if (RawBatchSelected_Get(batchID))
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
                            MsgBox.Show("the Batch is not available for use.");
                            return false;
                        }
                    }
                    RawBatchSelected_Add((int)dtBatchInfo.Rows[0]["BatchKey"], batchID, 0, dtBatchInfo.Rows[0]["BatchMfgDate"], dtBatchInfo.Rows[0]["BatchExpDate"], true);
                    return true;
                }
                #endregion

                MsgBox.Show("Batch ID not found");
                return false;

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
        }//Completed

        //Functions (finished Goods)
        private void FGBatchEntryInfo_Clear()
        {
            CurrentEntryBatchKey = 0;
            CurrentEntryLinkPointer = 0;
            FGBatchID.SetValueTrigger("", false);
            FGBatchQty.SetValueTrigger("", false);
            FGMFGDate.SetValueTrigger(null, false);
            FGExpDate.SetValueTrigger(null, false);
        }//Completed
        private void FGBatchEntryInfo_Set(int BatchKey, string batchID, decimal batQty, object mfgDate, object expDate, int BatchSelectedDocItmKey)
        {
            try
            {
                FGBatchID.SetValueTrigger(batchID, false);
                FGBatchQty.SetValueTrigger(batQty, false);
               
                if (GFunc.IsNE(mfgDate))
                    FGMFGDate.SetValueTrigger(DBNull.Value, false);
                else
                    FGMFGDate.SetValueTrigger(GFunc.NEDateTime(mfgDate, DBNull.Value), false);

                if (GFunc.IsNE(expDate))
                    FGExpDate.SetValueTrigger(DBNull.Value, false);
                else
                    FGExpDate.SetValueTrigger(GFunc.NEDateTime(expDate, DBNull.Value), false);

                CurrentEntryBatchKey = BatchKey;
                CurrentEntryLinkPointer = BatchSelectedDocItmKey;
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
        private void FGBatchEntryInfo_Update()
        {
            try
            {
                if (GFunc.IsBatchItmType(objItm.ItmType) == false)
                    return;

                if (FGBatchEntryInfo_Validation() == false)
                    return;

                #region Add New Batch Mode
                if (CurrentEntryLinkPointer == 0 && AddBatchMode)
                    FGBatchSelected_Add(CurrentEntryBatchKey, FGBatchID.Value.ToString(), FGBatchQty.DecimalValue, FGMFGDate.DateValue, FGExpDate.DateValue, false);
                else
                {
                    UltraGridRow gRow = this.tagrdBatchFG.Rows.OfType<UltraGridRow>().ToList().Find(
                    row => row.Cells["DocItmKey"].Text.Equals(CurrentEntryLinkPointer.ToString(), StringComparison.CurrentCultureIgnoreCase));

                    gRow.Cells["BatchID"].Value = FGBatchID.Value;
                    gRow.Cells["ItmBatchQty"].Value = FGBatchQty.DecimalValue;
                    gRow.Cells["BatchMfgDate"].Value = FGMFGDate.DateValue;
                    gRow.Cells["BatchExpDate"].Value = FGExpDate.DateValue;
                    tagrdBatchFG.UpdateData();
                }
                #endregion

               // dtItmBatchSelected.DefaultView.RowFilter = "LineLinkKey =" + ParentCurrentRow["DocItmKey"].ToString();
                CalculateTotal();
                FGBatchEntryInfo_Clear();
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
        private bool FGBatchEntryInfo_Validation()
        {
            try
            {

                if (GFunc.IsBatchItmType(objItm.ItmType) == false)
                    return true;

                decimal batchQty = FGBatchQty.DecimalValue;
                string batchID = GFunc.NEStr(this.FGBatchID.Value, string.Empty);

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
                if (FGBatchQty.DecimalValue <= 0)
                {
                    MsgBox.Show("Batch quantity cannot be negative or zero.");
                    return false;
                }
                #endregion

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
        private bool FGBatchSelected_Get(string batchID)
        {
            //Search for a match in BatchGen grid and Set it to the BatchEntry Controls
            try
            {

                UltraGridRow grdRow = this.tagrdBatchFG.Rows.OfType<UltraGridRow>().ToList()
                            .Find(r => r.Cells["BatchID"].Value.ToString().Equals(batchID, StringComparison.CurrentCulture)
                                && (int)r.Cells["ItmKey"].Value == CurrentParentItmKey
                                && (int)r.Cells["LineLinkKey"].Value == (int)ParentCurrentRow["DocItmKey"]);

                if (grdRow != null)
                {
                    this.tagrdBatchFG.ActiveRow = grdRow;
                    FGBatchEntryInfo_Set((int)grdRow.Cells["ItmBatchKey"].Value, batchID, (decimal)grdRow.Cells["ItmBatchQty"].Value, grdRow.Cells["BatchMfgDate"].Value, grdRow.Cells["BatchExpDate"].Value, (int)grdRow.Cells["DocItmKey"].Value);
                    tagrdBatchFG.ActiveRowScrollRegion.ScrollRowIntoView(grdRow);
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
        private bool FGBatchSelected_Add(int BatchKey, string batchID, decimal assignQty, object mfgDate, object expDate, bool SetBatchEntry)
        {
            try
            {

                DataRow drSelect = dtItmBatchSelected.NewRow();

                //copy parent row data to a new row in BatchSelected
                //note: the parent and batchselect has the same structure.
                foreach (DataColumn dc in dtItmBatchSelected.Columns)
                {
                    drSelect[dc.ColumnName] = ParentCurrentRow[dc.ColumnName];
                }

                //update require information to new child row.
                int NewDocItmKey = DocComUtility.GridAutoID_Get(tagrdBatchFG, "DocKey", "DocItmKey");
                drSelect["BatchID"] = batchID;
                drSelect["DocItmKey"] = NewDocItmKey;
                drSelect["ItmBatchKey"] = BatchKey;
                drSelect["LineType"] = LineType_Get();
                drSelect["LineLinkKey"] = ParentCurrentRow["DocItmKey"];
                drSelect["ItmBatchQty"] = assignQty;
                drSelect["BatchMfgDate"] = mfgDate == null ? DBNull.Value : mfgDate;
                drSelect["BatchExpDate"] = expDate == null ? DBNull.Value : expDate;

                //Set ItmDetSN By Incrementing
                //dtItmBatchSelected.DefaultView.RowFilter = "LineLinkKey=" + GFunc.NEInt(ParentCurrentRow["DocItmKey"], 0);
                //if (dtItmBatchSelected.DefaultView.Count > 0)
                //    drSelect["ItmDetSN"] = dtItmBatchSelected.Rows.Count == 0 ? 1 : dtItmBatchSelected.AsEnumerable().Where(p => p.Field<int>("LineLinkKey") == GFunc.NEInt(ParentCurrentRow["DocItmKey"], 0)).Max(p => p.Field<decimal>("ItmDetSN")) + 1;
                //else
                //    drSelect["ItmDetSN"] = 1;

                IEnumerable<DataRow> dtItmBatchFilter = dtItmBatchSelected.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == GFunc.NEInt(ParentCurrentRow["DocItmKey"], 0));
                if (dtItmBatchFilter.Count() > 0)                    
                    drSelect["ItmDetSN"] = dtItmBatchFilter.Max(p => p.Field<decimal>("ItmDetSN")) + 1;
                else
                    drSelect["ItmDetSN"] = 1;

              //  dtItmBatchSelected.DefaultView.RowFilter = "";
                dtItmBatchSelected.Rows.Add(drSelect);
                dtItmBatchSelected.AcceptChanges();

                CalculateTotal();

                if (SetBatchEntry)
                    FGBatchEntryInfo_Set(BatchKey, batchID, assignQty, mfgDate, expDate, NewDocItmKey);

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
        }//Completed
        private bool FGBatchInfo_Search(string batchID)
        {
            try
            {
                if (GFunc.IsBatchItmType(objItm.ItmType) == false)
                    return true;

                #region New Batch Mode
                if (CurrentEntryLinkPointer == 0)
                {
                    #region Add new Batch or Find a Batch from BatchSelected List
                    //User is trying search for the Batch ID to edit
                    if (FGBatchSelected_Get(batchID))
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
        private bool UpdateCallerDetailInfo()
        {
            try
            {

                DataRow[] childRows = this.dtItmBatchSelected.Select("LineLinkKey=" + (int)ParentCurrentRow["DocItmKey"]);

                DataRow ParentRow = this.dtItmBatchSelected.Rows.OfType<DataRow>().ToList()
                            .Find(r => (int)r["DocItmKey"] == (int)ParentCurrentRow["DocItmKey"]);

                if (ParentRow == null)
                    return false;

                if (FGMode)
                {
                    ParentRow["FGReq"] = FGReq.DecimalValue;
                    ParentRow["FGOverHeadKey"] =  FGOverHeadKey.Value == null? DBNull.Value : FGOverHeadKey.Value ;
                    ParentRow["FGOverHeadCost"] = FGOverHeadCost.DecimalValue;
                    if (GFunc.IsBatchItmType(objItm.ItmType))
                        ParentRow["FGProduceQty"] = Total.DecimalValue;
                    else
                        ParentRow["FGProduceQty"] = FGProduceQty.DecimalValue;
                    ParentRow["FGProduceWeight"] = GFunc.RndC(GFunc.NEDec(ParentRow["FGProduceQty"], 0) * GFunc.NEDec(ParentRow["FGWeight"], 0), GVar.RndDecs.Qtypt);
                    ParentRow["FGProduceGram"] = GFunc.RndC(GFunc.NEDec(ParentRow["FGProduceWeight"], 0) * DocComUtility.UOMGramRate_Get(GFunc.NEInt(ParentRow["FGWeightUOMKey"], 0)), GVar.RndDecs.Conpt);
                    ParentRow["FGOverHeadAmtH"] = GFunc.RndC(GFunc.NEDec(ParentRow["FGProduceQty"], 0) * GFunc.NEDec(ParentRow["FGOverHeadCost"], 0), GVar.RndDecs.Amtpt);

                    foreach (DataRow dr in childRows)
                    {
                        dr["FGProduceQty"] = ParentRow["FGProduceQty"];
                        dr["FGProduceWeight"] = ParentRow["FGProduceWeight"];
                        dr["FGProduceGram"] = ParentRow["FGProduceGram"];
                        dr["FGOverHeadAmtH"] = ParentRow["FGOverHeadAmtH"];
                    }
                }
                else
                {
                    if (GFunc.IsBatchItmType(objItm.ItmType))
                        ParentRow["BOMUsed"] = Total.DecimalValue;
                    else
                        ParentRow["BOMUsed"] = BOMUsed.DecimalValue;

                    ParentRow["BOMIssue"] = BOMIssue.DecimalValue;
                    ParentRow["BOMReturn"] = GFunc.NEDec(ParentRow["BOMIssue"], 0) - GFunc.NEDec(ParentRow["BOMUsed"], 0);
                    ParentRow["BOMUsedWeight"] = GFunc.RndC(GFunc.NEDec(ParentRow["BOMUsed"], 0) * GFunc.NEDec(ParentRow["BOMWeight"], 0), GVar.RndDecs.Qtypt);
                    ParentRow["BOMUsedGram"] = GFunc.RndC(GFunc.NEDec(ParentRow["BOMUsedWeight"], 0) * DocComUtility.UOMGramRate_Get(GFunc.NEInt(ParentRow["BOMWeightUOMKey"], 0)), GVar.RndDecs.Conpt);
                    ParentRow["BOMLabourCost"] = 0;
                    ParentRow["BOMLabourAmt"] = 0;

                    foreach (DataRow dr in childRows)
                    {
                        dr["BOMUsed"] = ParentRow["BOMUsed"];
                        dr["BOMReturn"] = ParentRow["BOMReturn"];
                        dr["BOMUsedWeight"] = ParentRow["BOMUsedWeight"];
                        dr["BOMUsedGram"] = ParentRow["BOMUsedGram"];
                        dr["BOMLabourCost"] = 0;
                        dr["BOMLabourAmt"] = 0;
                    }
                }

                //Update Parent Item row to move next and Prev
                foreach (DataColumn dc in ParentCurrentRow.Table.Columns)
                {
                    if (ParentRow.Table.Columns.Contains(dc.ColumnName))
                        ParentCurrentRow[dc.ColumnName] = ParentRow[dc.ColumnName];
                }

                dtItmBatchSelected.AcceptChanges();
                ParentInfo_Set();
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
        private void ParentInfo_Set()
        {
            try
            {
                if (FGMode)
                {
                    FGBUOMKey.SetValueTrigger(GFunc.NEInt(ParentCurrentRow["FGBUOMKey"], null), false);
                    FGReq.SetValueTrigger(GFunc.NEDec(ParentCurrentRow["FGReq"], 0), false);
                    FGProduceQty.SetValueTrigger(GFunc.NEDec(ParentCurrentRow["FGProduceQty"], 0), false);
                    FGProduceWeight.SetValueTrigger(GFunc.NEDec(ParentCurrentRow["FGProduceWeight"], 0), false);
                    FGOverHeadKey.SetValueTrigger(GFunc.NEInt(ParentCurrentRow["FGOverHeadKey"], 0), false);
                    FGOverHeadCost.SetValueTrigger(GFunc.NEDec(ParentCurrentRow["FGOverHeadCost"], 0), false);
                    FGOverHeadAmtH.SetValueTrigger(GFunc.NEDec(ParentCurrentRow["FGOverHeadAmtH"], 0), false);
                }
                else
                {
                    BOMBUOMKey.SetValueTrigger(GFunc.NEInt(ParentCurrentRow["BOMBUOMKey"], null), false);
                    BOMReq.SetValueTrigger(GFunc.NEDec(ParentCurrentRow["BOMReq"], 0), false);
                    BOMIssue.SetValueTrigger(GFunc.NEDec(ParentCurrentRow["BOMIssue"], 0), false);
                    BOMReturn.SetValueTrigger(GFunc.NEDec(ParentCurrentRow["BOMReturn"], 0), false);
                    BOMUsed.SetValueTrigger(GFunc.NEDec(ParentCurrentRow["BOMUsed"], 0), false);
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

        private bool IsDuplicateBatchID(int option, string batchID)
        {
            try
            {
                if (option == 0)
                {
                    int duplicateCount = (from row in dtItmBatchSelected.AsEnumerable()
                                          where row.Field<int>("ItmKey") == objItm.ItmKey && row.Field<string>("batchID").Equals(batchID, StringComparison.CurrentCulture)
                                          select row.Field<string>("BatchID")).Count();
                    if (duplicateCount > 0)
                    {
                        MsgBox.Show(batchID + " is already exist");
                        return true;
                    }
                    else
                        return false;
                }

                List<SqlParameter> paraList = new List<SqlParameter>();
                paraList.Add(new SqlParameter("@Option", option));
                paraList.Add(new SqlParameter("@ItmKey", CurrentParentItmKey));
                paraList.Add(new SqlParameter("@BatchID", batchID));
                paraList.Add(new SqlParameter("@CurrentEntryLinkPointer", CurrentEntryLinkPointer));
                paraList.Add(new SqlParameter("@BatchKey", CurrentEntryBatchKey));

                string xmlDocDetail = GFunc.ConvertDataTableToXML(dtItmBatchSelected.DefaultView.ToTable("dtDocDetail", false, "BatchID", "ItmBatchQty", "BatchMfgDate", "BatchExpDate", "DocItmKey"));
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
        private void QtyRounding(object sender, CancelEventArgs e)
        {
            try
            {
                TAUtil.TANumericEditor txtQty = sender as TAUtil.TANumericEditor;
                if (ValidateQty(txtQty.Value))
                {
                    txtQty.SetValueTrigger(GFunc.RndC(txtQty.DecimalValue, GVar.RndDecs.Qtypt), false);
                }
                else
                    e.Cancel = false;
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
        private void QtyRoundInt(object sender, CancelEventArgs e)
        {
            try
            {
                TAUtil.TANumericEditor txtQty = sender as TAUtil.TANumericEditor;
                if (ValidateQty(txtQty.Value))
                {
                    txtQty.SetValueTrigger(GFunc.RndC(txtQty.DecimalValue, 0), false);
                }
                else
                    e.Cancel = false;
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
        private void QtyChange(object sender, CancelEventArgs e)
        {
            try
            {
                TAUtil.TANumericEditor txtQty = sender as TAUtil.TANumericEditor;
                if (ValidateQty(txtQty.Value))
                {
                    txtQty.SetValueTrigger(GFunc.RndC(txtQty.DecimalValue, GVar.RndDecs.Qtypt), false);

                    BOMUsed.SetValueTrigger(GFunc.NEDec(BOMIssue.Value, 0) - GFunc.NEDec(BOMReturn.Value, 0), false);
                }
                else
                    e.Cancel = true;
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
        private DateTime? getExpDate(string defExpDate, DateTime mfgDate)
        {
            DateTime? expDate = null;
            try
            {
                if (defExpDate.Length > 0)
                {

                    int interval = int.Parse(defExpDate.Substring(0, defExpDate.Length - 1));
                    string dateType = defExpDate.Substring(defExpDate.Length - 1, 1);
                    expDate = mfgDate.AddDays(interval);
                    switch (dateType.ToLower())
                    {
                        case "w":
                            expDate = mfgDate.AddDays(interval * 7);
                            break;
                        case "m":
                            expDate = mfgDate.AddMonths(interval);
                            break;
                        case "y":
                            expDate = mfgDate.AddYears(interval);
                            break;
                    }
                }
                return expDate;
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
        private bool CalculateTotal()
        {
            decimal totalQty = 0;
            try
            {
                if (GFunc.IsBatchItmType(objItm.ItmType))
                {
                    dtItmBatchSelected.AcceptChanges();
                    totalQty = (from row in dtItmBatchSelected.AsEnumerable()
                                where row.Field<int>("LineLinkKey") == (int)ParentCurrentRow["DocItmKey"]
                                select row.Field<decimal>("ItmBatchQty")).Sum();
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
                Total.SetValueTrigger(totalQty.ToString(), false);
            }
            return true;
        }//Completed
        private void ClearEntryData()
        {
            //Header
            ItmID.SetValueTrigger(null, false);
            ItmDes.SetValueTrigger(null, false);
            FinishedGoodID.SetValueTrigger(null, false);

            //Finshed Goods.SetValueTrigger(null, false);
            FGBUOMKey.SetValueTrigger(null, false);
            FGReq.SetValueTrigger(null, false);
            FGProduceQty.SetValueTrigger(null, false);
            FGProduceWeight.SetValueTrigger(null, false);
            FGOverHeadKey.SetValueTrigger(null, false);
            FGOverHeadCost.SetValueTrigger(null, false);
            FGOverHeadAmtH.SetValueTrigger(null, false);

            BatchNoFormat.SetValueTrigger(null, false);
            InitialNumber.SetValueTrigger(null, false);
            NoOfBatch.SetValueTrigger(null, false);
            QtyEachBatch.SetValueTrigger(null, false);

            //Material
            BOMBUOMKey.SetValueTrigger(null, false);
            BOMReq.SetValueTrigger(null, false);
            BOMIssue.SetValueTrigger(null, false);
            BOMReturn.SetValueTrigger(null, false);
            BOMUsed.SetValueTrigger(null, false);


            //Entry Control.SetValueTrigger(null, false);
            BatchID.SetValueTrigger(null, false);
            BatchQty.SetValueTrigger(null, false);
            MFGDate.SetValueTrigger(null, false);
            ExpDate.SetValueTrigger(null, false);
        }//Completed
        private Int32 LineType_Get()
        {
            int LineType = 0;
            if ((GFunc.NEInt(ParentCurrentRow["ItmType"], 0) == (int)GEnum.ItemType.Finished_GDB && GFunc.NEInt(ParentCurrentRow["LineType"], 0) == (int)GEnum.RecDetailType.DItmFinishedGoods)
                || GFunc.NEInt(ParentCurrentRow["ItmType"], 0) == (int)GEnum.ItemType.Finished_GD && GFunc.NEInt(ParentCurrentRow["LineType"], 0) == (int)GEnum.RecDetailType.DItmFinishedGoods) //FG Batch
                LineType = 3010;
            else if ((GFunc.NEInt(ParentCurrentRow["ItmType"], 0) == (int)GEnum.ItemType.Finished_GDB && GFunc.NEInt(ParentCurrentRow["LineType"], 0) == (int)GEnum.RecDetailType.DItmRaw_Material)
                || GFunc.NEInt(ParentCurrentRow["ItmType"], 0) == (int)GEnum.ItemType.Finished_GD && GFunc.NEInt(ParentCurrentRow["LineType"], 0) == (int)GEnum.RecDetailType.DItmRaw_Material) //FG Batch
                LineType = 3110;
            else if (GFunc.NEInt(ParentCurrentRow["ItmType"], 0) == 410) //FG Serial Batch
                LineType = 3020;
            else if (GFunc.NEInt(ParentCurrentRow["LineType"], 0) == 3100 && (GFunc.NEInt(ParentCurrentRow["ItmType"], 0) == 100 || GFunc.NEInt(ParentCurrentRow["ItmType"], 0) == 110 || GFunc.NEInt(ParentCurrentRow["ItmType"], 0) == 250 || GFunc.NEInt(ParentCurrentRow["ItmType"], 0) == 500)) //Raw Material , Stock
                LineType = 3110;
            else if (GFunc.NEInt(ParentCurrentRow["LineType"], 0) == 3200 && (GFunc.NEInt(ParentCurrentRow["ItmType"], 0) == 100 || GFunc.NEInt(ParentCurrentRow["ItmType"], 0) == 110 || GFunc.NEInt(ParentCurrentRow["ItmType"], 0) == 250 || GFunc.NEInt(ParentCurrentRow["ItmType"], 0) == 500)) //Packing Material , Stock
                LineType = 3210;
            else if (GFunc.NEInt(ParentCurrentRow["LineType"], 0) == 3100 && GFunc.NEInt(ParentCurrentRow["ItmType"], 0) == 310) //Raw Material , Serial
                LineType = 3120;
            else if (GFunc.NEInt(ParentCurrentRow["LineType"], 0) == 3200 && GFunc.NEInt(ParentCurrentRow["ItmType"], 0) == 310) //Packing Material , Serial
                LineType = 3220;

            return LineType;
        }//Modified 
        private DataTable ReverseTransSign(DataTable dt)
        {
            try
            {
                DataTable dtReturn = dt.Copy();
                //We need to reverse the effect of the computed batchSelected qty when updating the values back to the callergrid
                foreach (DataRow dr in dtReturn.Rows)
                {
                    switch ((int)dr["LineType"])
                    {
                        case 3000:
                        case 3010:
                        case 3020:
                        case 3030:
                            dr["ItmBatchQty"] = (decimal)dr["ItmBatchQty"] * CallerDocSign;
                            break;
                        case 3100:
                        case 3110:
                        case 3120:
                        case 3130:
                        case 3200:
                        case 3210:
                        case 3220:
                        case 3230:
                            dr["ItmBatchQty"] = (decimal)dr["ItmBatchQty"] * CallerDocSign * -1;
                            break;
                    }
                }
                dtReturn.AcceptChanges();
                return dtReturn;
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }

        }

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

                    //reassign Entry Mode
                    switch (CallerDC)
                    {
                        case GEnum.SystemCode.Purchase_Invoice:
                        case GEnum.SystemCode.Purchase_Debit_Note:
                        case GEnum.SystemCode.Purchase_Delivery:
                            AddBatchMode = true;
                            break;

                        case GEnum.SystemCode.Inventory_Adjustment:
                            if (CallerObjDoc.DocType == 400)//Add New Batch

                                AddBatchMode = true;

                            break;

                        default:
                            AddBatchMode = false;
                            break;
                    }

                    FormLayout(false);

                    //Get all Parent row (where ItmType is Batch) from caller grid
                    //dtCallerGridSource.DefaultView.RowFilter = "LineLinkKey = 0 And ItmType In(110,210,310,410)";
                    //dtParentItm = dtCallerGridSource.DefaultView.ToTable();
                    //ParentCount = GFunc.NEInt(dtParentItm.Rows.Count, 0);

                    //Get a working copy of the callergrid datasource
                   // dtCallerGridSource.DefaultView.RowFilter = "";
                    SetParentItm();
                    dtItmBatchSelected = dtCallerGridSource.Copy();
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
            string BatchColName = string.Empty;
            try
            {
                UpdateCallerDetailInfo();
                if (FGMode)
                {
                    tagrdBatchFG.PerformAction(UltraGridAction.ExitEditMode);
                    tagrdBatchFG.UpdateData();
                }
                else
                {
                    tagrdBatchList.PerformAction(UltraGridAction.ExitEditMode);
                    tagrdBatchList.UpdateData();
                    tagrdBatchRaw.PerformAction(UltraGridAction.ExitEditMode);
                    tagrdBatchRaw.UpdateData();
                }

                if (SavingValidation() == false)
                    return false;

                if (CalculateTotal() == false)
                    return false;

                if (FGMode)
                {
                    BatchColName = "FGProduceQty";
                }
                else
                {
                    BatchColName = "BOMUsed";
                }

                foreach (DataRow dr in dtParentItm.Rows)
                {
                    decimal vChildQtyTotal = GFunc.NEDec(dtItmBatchSelected.Compute("Sum(ItmBatchQty)", "LineLinkKey =" + dr["DocItmKey"].ToString()), 0.00M);
                    DataRow drBatchSelected = this.dtItmBatchSelected.Rows.OfType<DataRow>().ToList().Find(r => r.Field<int>("DocItmKey") == GFunc.NEInt(dr["DocItmKey"], 0));

                    if (GFunc.NEInt(dr["ItmType"], 0) == (int)GEnum.ItemType.StockB ||
                        GFunc.NEInt(dr["ItmType"], 0) == (int)GEnum.ItemType.Finished_GDB ||
                        GFunc.NEInt(dr["ItmType"], 0) == (int)GEnum.ItemType.Serial_StockB ||
                        GFunc.NEInt(dr["ItmType"], 0) == (int)GEnum.ItemType.Serial_Finished_GDB)
                    {
                        //update parent qty
                        drBatchSelected[BatchColName] = vChildQtyTotal * CallerDocSign;
                    }
                    
                }

                dtCallerGridSource.Rows.Clear();
                GFunc.CopyDataTableToDetailObject(dtItmBatchSelected, dtCallerGridSource);
                dtCallerGridSource.AcceptChanges();
                grdCallerGrid.Refresh();
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
                //dtItmBatchSelected.DefaultView.RowFilter = "LineLinkKey =" + ParentCurrentRow["DocItmKey"].ToString();
                IEnumerable<DataRow> dtItmBatchFilter = dtItmBatchSelected.AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == GFunc.NEInt(ParentCurrentRow["DocItmKey"], 0));

                //----------------------------------keep for reference in future------------------------------------------------
                //DataTable dublicateBatch = (from rowSection in dtItmBatchSelected.DefaultView.ToTable().AsEnumerable()
                //                            group rowSection by rowSection.Field<string>("BatchID") into dublicateBatchID
                //                            join row in dtItmBatchSelected.DefaultView.ToTable().AsEnumerable()
                //                                  on dublicateBatchID.Key equals row.Field<string>("BatchID")
                //                            where dublicateBatchID.Count() > 0
                //                            select row).AsDataTable(); 
                //--------------------------------------------------------------------------------------------------------

                DataTable dublicateBatch = (from row in dtItmBatchFilter.AsEnumerable()
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

                //string xmlDocDetail = GFunc.ConvertDataTableToXML(dtItmBatchSelected.DefaultView.ToTable("dtDocDetail", false, "BatchID", "ItmBatchQty", "BatchMfgDate", "BatchExpDate"));
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
            finally
            {
               // dtItmBatchSelected.DefaultView.RowFilter = "";
            }
            return true;
        }//Completed
        private bool ValidateQty(object qty)
        {
            if (GFunc.NEDec(qty, 0) < 0)
            {
                MsgBox.Show("Quantity cann't be negative value");
                return false;
            }
            return true;
        }

        //Set Error Methods
        private void OnDataError(object sender, TAUtil.TAErrorEventArgs e)
        {
            if (e.ErrorCode == TAUtil.TAErrorCode.NOT_IN_LIST)
            {
                if (sender.GetType() == typeof(TAUtil.TAGridEditor))
                {
                    TAUtil.TAGridEditor grd = sender as TAUtil.TAGridEditor;
                    if (grd.ActiveCell.Column.EditorComponent != null)
                    {
                        grd.PerformAction(UltraGridAction.EnterEditMode);
                        GlobalUI.ItemNotInList(grd.ActiveCell, null, 0);
                    }
                }
            }
            else if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_NUMERIC)
            {

                MsgBox.Show(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
            }
            else if (e.ErrorCode == TAUtil.TAErrorCode.FORMULA_NOT_RECOGNIZE)
            {

                MsgBox.Show("FORMULA NOT RECOGNIZE");
            }
            else
            {
                MsgBox.Show(e.ErrorMessage);
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
