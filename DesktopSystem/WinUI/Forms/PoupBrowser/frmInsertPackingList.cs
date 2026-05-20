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
using TAUtil;

namespace WinUI
{
    public partial class frmInsertPackingList : Form
    {
        #region Declaration
        private DataTable dtDocNums;
        private int _DocCodeKey;
        private bool _Batch;
        private string ContextMenuSetting = string.Empty;
        internal delegate void AppendRecord(DataTable dt, string packingNumber, decimal packingWeight, decimal NoOfPack, decimal length, decimal width, decimal height, bool appendOnPickingSequence);
        internal event AppendRecord RecordAppend;
        private string _DocIDList = string.Empty;
        private int lastPackingNum = 0;
        //private int CurrentMaxNum = 0;
        
        #endregion

        #region properties
        public string DocIDList
        {

            get
            {
                return this._DocIDList;
            }
            set
            {
                this._DocIDList = value;
            }
        }
        public int DocCodeKey
        {

            get
            {
                return this._DocCodeKey;
            }
            set
            {
                this._DocCodeKey = value;
            }
        }
        public bool Batch
        {

            get
            {
                return this._Batch;
            }
            set
            {
                this._Batch = value;
            }
        }
        #endregion

        //Initialize
        public frmInsertPackingList()
        {
            InitializeComponent();
        }//Completed
        public frmInsertPackingList(DataTable dtSelectedDocNums, int DocCodeKey, bool IncludeBatch, int lastPackingNum)
        {
            InitializeComponent();
            dtDocNums = dtSelectedDocNums;
            this.DocCodeKey = DocCodeKey;
            this.Batch = IncludeBatch;
            this.lastPackingNum = lastPackingNum;
            
        }//Completed

        //Form
        private void frmInsertPackingList_Load(object sender, EventArgs e)
        {
            try
            {
                //Get Selected Document list
                for (int i = 0; i < dtDocNums.Rows.Count; i++)
                {
                    DocIDList += dtDocNums.Rows[i]["DocKey"].ToString() + ((i == dtDocNums.Rows.Count - 1) ? "" : ",");
                }

                //Format all grids and filter
                GlobalUI.FormGrids_Set(this, (int)GEnum.SystemCode.Packing_List, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)GEnum.SystemCode.Packing_List, this.Name);
                GlobalUI.Combos_Fill(this, (int)GEnum.SystemCode.Packing_List);

                //Assign defaults values
                PackingData_initialise();

                #region Form Layout
                IncludeBatch.Checked = Batch;

                tagrdDetail.DisplayLayout.Bands[0].Override.AllowAddNew = AllowAddNew.No;
                for (int i = 0; i < tagrdDetail.DisplayLayout.Bands[0].Columns.Count; i++)
                {
                    switch(tagrdDetail.DisplayLayout.Bands[0].Columns[i].Key.ToLower())
                    {
                        case "qtyinput":
                        case "selected":
                            tagrdDetail.DisplayLayout.Bands[0].Columns[i].CellActivation = Activation.AllowEdit;
                            break;

                        default:
                            tagrdDetail.DisplayLayout.Bands[0].Columns[i].CellActivation = Activation.ActivateOnly;
                            break;
                    }
                }
                #endregion
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
        private void frmInsertPackingList_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.RecordAppend = null;
        }
        private void frmInsertPackingList_KeyDown(object sender, KeyEventArgs e)
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed

        //Grid//Completed
        private void tagrdDetail_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
                RowSelected_Update(e.Cell.Row, e.Cell.Column.Key);
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
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }//Completed
        private void btnAppend_Click(object sender, EventArgs e)
        {
            try
            {
                tagrdDetail.PerformAction(UltraGridAction.ExitEditMode);
                tagrdDetail.UpdateData();

                DataTable dtGrid = tagrdDetail.DataSource as DataTable;
                if (ValidateInput(false) == false)
                    return;

                //prepare selected row to be transfer 
                DataTable dtSelected = dtGrid.Copy();
                dtSelected.Clear();
                foreach (DataRow row in dtGrid.Rows)
                {
                    if (GFunc.NEBool(row["Selected"], false) == true)
                    {
                        if (this.LoosePacking.Checked)
                            row["ItmQtyPerPack"] = 0;
                        else
                        {
                            row["ItmQtyPerPack"] = GFunc.RndDC(GFunc.NEDec(row["QtyInput"], 0), GFunc.NEDec(NumberOfPack.Value,0), GVar.RndDecs.Qtypt);
                        }
                        dtSelected.Rows.Add(row.ItemArray);
                    }
                }
                dtGrid.AcceptChanges();

                //transfer select row to packing document and remove row if qty has been consumed
                RecordAppend.Invoke(dtSelected, PackingNumber.Text, GFunc.NEDec(PackingWeight.Value, 0), GFunc.NEDec(NumberOfPack.Value, 0), GFunc.NEDec(Length.Value, 0), GFunc.NEDec(Width.Value, 0), GFunc.NEDec(Height.Value, 0), AppendOnPickSeq.Checked);
                foreach (DataRow row in dtGrid.Rows)
                {
                    if (GFunc.NEDec(row["QtyInput"], 0) >= GFunc.NEDec(row["ItmQty"], 0) || GFunc.NEDec(row["ItmQty"], 0) == 0)
                        row.Delete();
                    else
                    {
                        row["ItmQty"] = GFunc.NEDec(row["ItmQty"], 0) - GFunc.NEDec(row["QtyInput"], 0);
                        row["QtyInput"] = 0;
                    }
                }
                dtGrid.AcceptChanges();

                //Increment lastpacking number
                lastPackingNum = lastPackingNum + GFunc.NEInt(NumberOfPack.Value, 0);
                PackingData_initialise();
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
        private void btnAppendSt_Click(object sender, EventArgs e)
        {
            #region Declaration
            string PackNumberStr = string.Empty; 
            decimal PackNumber = 0;
            decimal NumOfPack = 0;
            decimal StdPackSize = 0;
            decimal StdPackWeight = 0;
            decimal StdPackLength = 0;
            decimal StdPackWidth = 0;
            decimal StdPackHeight = 0;
            decimal QtyIssue = 0;
            decimal QtyConRate = 0;
            decimal PackNumberCounter = 0;
            #endregion

            this.Cursor = Cursors.WaitCursor;

            try
            {
                tagrdDetail.PerformAction(UltraGridAction.ExitEditMode);
                tagrdDetail.UpdateData();

                //Validation
                if (ValidateInput(true) == false)
                    return;

                //Initialise variables
                DataTable dtGrid = tagrdDetail.DataSource as DataTable;
                PackNumber = lastPackingNum + 1;

                //prepare selected row to be transfer and Standard Item Grp (DocKey, ItmKey)
                DataTable dtItmGrp = StandardItmGrp_Get(); 
                DataTable dtSelected = dtGrid.Copy();

                foreach (DataRow drItmGrp in dtItmGrp.Rows)
                {
                    //dtGrid.DefaultView.RowFilter = "ItmKey=" + drItmGrp["ItmKey"].ToString() + " And DocKey =" + drItmGrp["DocKey"].ToString() + " And Selected = 1";
                    IEnumerable<DataRow> dtGridFilter=dtGrid.AsEnumerable().Where(r=>r.Field<int>("ItmKey")==GFunc.NEInt(drItmGrp["ItmKey"],0) &&
                               r.Field<int>("DocKey") == GFunc.NEInt(drItmGrp["DocKey"], 0) && r.Field<bool>("Selected") == true);
                    dtSelected.Clear();
                    PackNumberCounter = 0;

                    foreach (DataRow row in dtGridFilter)
                    {
                        //if (row.IsNew)
                        //    continue;
                        //Get Standard Packing infor
                        QtyIssue = GFunc.NEDec(row["QtyInput"], 0);
                        QtyConRate = GFunc.NEDec(row["ItmConRate"], 1);
                        StdPackWeight = GFunc.NEDec(row["StdPackWeight"], 0);
                        StdPackLength = GFunc.NEDec(row["StdPackLength"], 0);
                        StdPackWidth = GFunc.NEDec(row["StdPackWidth"], 0);
                        StdPackHeight = GFunc.NEDec(row["StdPackHeight"], 0);
                        StdPackSize = GFunc.RndDC(GFunc.NEDec(row["StdPackSize"], 0), QtyConRate, GVar.RndDecs.Qtypt);

                        //Calculate Packing
                        NumOfPack = GFunc.RndDC(QtyIssue, StdPackSize, 0);
                        if (NumOfPack == 0)
                            NumOfPack = 1;

                        row["ItmQtyPerPack"] = GFunc.RndDC(QtyIssue, NumOfPack, GVar.RndDecs.Qtypt);
                        PackNumberCounter = PackNumberCounter + NumOfPack;

                        //transfer select row to packing document 
                        dtSelected.Rows.Add(row.ItemArray);
                    }

                    dtGrid.AcceptChanges();
                    PackNumberStr = PackNumber.ToString() + " - " + (PackNumber + PackNumberCounter-1).ToString();

                    RecordAppend.Invoke(dtSelected, PackNumberStr, StdPackWeight, NumOfPack, StdPackLength, StdPackWidth, StdPackHeight, AppendOnPickSeq.Checked);
                    PackNumber = PackNumber + PackNumberCounter;

                    //remove row if qty has been consumed or update remaining qty
                    foreach (DataRow row in dtGridFilter)
                    {
                        if (GFunc.NEDec(row["QtyInput"], 0) >= GFunc.NEDec(row["ItmQty"], 0) || GFunc.NEDec(row["ItmQty"], 0) == 0)
                            row.Delete();
                        else
                        {
                            row["ItmQty"] = (decimal)row["ItmQty"] - QtyIssue;
                            row["QtyInput"] = 0;
                        }
                    }
                }

                //Prepare for next packing entry
                lastPackingNum = lastPackingNum + GFunc.NEInt(NumberOfPack.Value, 0);
                PackingData_initialise();
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
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                foreach (UltraGridRow row in tagrdDetail.Rows)
                {
                    row.Cells["Selected"].Value = true;
                    RowSelected_Update(row, "Selected");
                    row.Update();
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
        private void btnUnSelectAll_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                foreach (UltraGridRow row in tagrdDetail.Rows)
                {
                    row.Cells["Selected"].Value = false;
                    RowSelected_Update(row, "Selected");
                    row.Update();
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
        private void LoosePacking_CheckedChanged(object sender, EventArgs e)
        {
            btnAppendSt.Enabled = !LoosePacking.Checked;
        }

        //Function
        private bool ValidateInput(bool useStandardPacking)
        {
            DataTable dtGrid = tagrdDetail.DataSource as DataTable;

            //Set default value when empty
            PackingWeight.SetValueTrigger(GFunc.NEDec(PackingWeight.Value, 0), false);
            NumberOfPack.SetValueTrigger(GFunc.NEDec(NumberOfPack.Value, 0), false);
            Length.SetValueTrigger(GFunc.NEDec(Length.Value, 0), false);
            Width.SetValueTrigger(GFunc.NEDec(Width.Value, 0), false);
            Height.SetValueTrigger(GFunc.NEDec(Height.Value, 0), false);

            //Validation
            if (PackingNumber.Text == string.Empty)
            {
                MsgBox.Show("Packing Number cannot be empty");
                PackingNumber.Focus();
                return false;
            }

            if (useStandardPacking)
            {
                int stdPackingNumber= 0;
                if (int.TryParse(PackingNumber.Text, out stdPackingNumber) == false)
                {
                    MsgBox.Show("You must use a valid Packing Number(Integer) in order to perform standard size packing");
                    PackingNumber.Focus();
                    return false;
                }
            }

            if (dtGrid.Rows.Count < 1)
            {
                MsgBox.Show("All items has been transfered.");
                return false;
            }
            return true;
        }//Completed
        private bool RowSelected_Update(UltraGridRow grdrow, string ColNm)
        {
            try
            {
                switch (ColNm.ToLower())
                {
                    case "selected":
                        if ((bool)grdrow.Cells["Selected"].Value)
                        {
                            grdrow.Cells["QtyInput"].Value = grdrow.Cells["ItmQty"].Value;
                            grdrow.Cells["Sequence"].Value = SequenceCounter_Get();
                        }
                        else
                        {
                            grdrow.Cells["QtyInput"].Value = 0;
                            grdrow.Cells["Sequence"].Value = 0;
                        }
                        break;

                    case "qtyinput":
                        if (GFunc.NEDec(grdrow.Cells["QtyInput"].Value, 0) == 0)
                        {
                            grdrow.Cells["Selected"].Value = false;
                            grdrow.Cells["Sequence"].Value = 0;
                        }
                        else
                        {
                            grdrow.Cells["Selected"].Value = true;
                            grdrow.Cells["Sequence"].Value = SequenceCounter_Get();
                        }
                        break;
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
        private int SequenceCounter_Get()
        {
            try
            {
                DataTable dt = tagrdDetail.DataSource as DataTable;
                return dt.AsEnumerable().Max(x => x.Field<int>("Sequence")) + 1;
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
        private bool PackingData_initialise()
        {
            try
            {                
                NumberOfPack.SetValueTrigger(1, false);
                PackingNumber.SetValueTrigger(lastPackingNum + 1, false);
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
        private DataTable StandardItmGrp_Get()
        {
            try
            {
                DataTable dtGrid = tagrdDetail.DataSource as DataTable;

                //Get Itm List for grouping and packing
                return (from row in dtGrid.AsEnumerable()
                        where row.Field<bool?>("Selected") == true
                        group row by new
                        {
                            DocKey = row.Field<int>("DocKey"),
                            ItmKey = row.Field<int>("ItmKey")                        
                        } into grp
                        select new
                        {
                            DocKey = grp.Key.DocKey,
                            ItmKey = grp.Key.ItmKey,
                            MinSeq = grp.Min(r=>r.Field<int>("Sequence"))
                        }).AsDataTable();
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
