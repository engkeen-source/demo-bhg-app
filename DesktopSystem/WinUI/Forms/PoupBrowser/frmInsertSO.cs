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
using Infragistics.Win;
using System.Collections;
using TAUtil;
namespace WinUI
{
    public partial class frmInsertSO : Form
    {
        #region Local Variables
        private string ContextMenuSetting = string.Empty;
        private Document objDoc;
        private UltraGrid gdTgtDetail = null;
        private DataTable dtTgtDetail;

        private int TgtDC = 0;
        private int ConKey = 0;
        private int CurrKey = 0;
        private int pickSeqCounter = 0;


        #endregion

        //Initialisze
        public frmInsertSO()
        {
            InitializeComponent();
        }        //Completed
        public frmInsertSO(Document doc, UltraGrid tagrdDetItms)
        {
            InitializeComponent();
            this.objDoc = doc;
            this.dtTgtDetail = tagrdDetItms.DataSource as DataTable;
            this.gdTgtDetail = tagrdDetItms;

        }//Completed

        //Form Events        
        private void frmInsertSO_Load(object sender, EventArgs e)
        {
            try
            {
                TgtDC = (int)objDoc.DocCodeKey;
                CurrKey = (int)GFunc.GetIntPropertyValue("DocCurrKey", this.objDoc);
                ConKey = (int)GFunc.GetIntPropertyValue("DocConKey", this.objDoc);
                PrmDate.DateValue = DateTime.Today.Date;
                //Set ContextMenu & Grid Setting                  
                GlobalUI.FormGrids_Set(this, (int)objDoc.DocCodeKey, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objDoc.DocCodeKey);
                //GlobalUI.cmnuGlobal_Set(this); 
                GlobalUI.Combos_Fill(this, TgtDC);

                ComboDocID_Fill();
                ComboItm_Fill();
                FormLayout();
                
                RemoveMatchedRow();
                //filter to show only parent rows
                Grid_Filter();

                //Set Default Value
                PrmDate.SetValueTrigger(DateTime.Today.Date, false);
                UseSystemPrice.Checked = SysOptionUtility.GetBool("InsertARSOUseSysPrice");
                UseMaxStock.Checked = SysOptionUtility.GetBool("InsertARSOUseMaxStock");
                UpdateChanges.Checked = SysOptionUtility.GetBool("InsertARSOUpdatesRemarkMarking");
                PickSeq.Checked = SysOptionUtility.GetBool("InsertARSOAppendInvOnPickSeq");

                PrmDate.Select();
                
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
        private void frmInsertSO_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)objDoc.DocCodeKey);
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
        private void frmInsertSO_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hashtable docDet = new Hashtable();
            docDet.Add(GEnum.Details.Doc_Itm, gdTgtDetail);
            DocComUtility.CalForm(objDoc, docDet, true, false);


        }

        //Grid Events
        private void tagrdItemSO_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            UltraGridCell curCell = tagrdItemSO.ActiveCell;

            try
            {
                switch (curCell.Column.Key.ToLower())
                {
                    case "itmqtyissue":
                        if (GFunc.IsNEZ(curCell.Value))
                            tagrdItemSO.ActiveRow.Cells["Selected"].Value = 0;
                        else
                        {
                            if (GFunc.NEDec(curCell.Value, 0) > GFunc.NEDec(curCell.Row.Cells["QtyBalance"].Value, 0))
                            {
                                if (MsgBox.Show("Selected quantity is greater than what is outstanding. Continue?", GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            }
                            tagrdItemSO.ActiveRow.Cells["Selected"].Value = 1;
                            if (PickSeq.Checked)
                            {
                                pickSeqCounter++;
                                tagrdItemSO.ActiveRow.Cells["pickSeq"].Value = pickSeqCounter;
                            }

                        }
                        break;
                    case "selected":
                        if (tagrdItemSO.ActiveRow != null)
                        {
                            Selected_Set(tagrdItemSO.ActiveRow, (bool)curCell.Value);
                            tagrdItemSO.UpdateData();
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed

        //Controls Events           
        private void PrmDate_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                Grid_Refresh();
                Grid_Filter();
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
        }//---------------------changed
        private void DocKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                Grid_Filter();
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
        }//---------------------changed
        private void ItmKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                Grid_Filter();
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
        }//---------------------changed      
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

        //Button Events
        private void btnAppend_Click(object sender, EventArgs e)
        {
            try
            {

                this.tagrdItemSO.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdItemSO.UpdateData();

                if (this.tagrdItemSO.DataSource != null)
                {

                    DataTable dt = tagrdItemSO.DataSource as DataTable;

                    #region Check and add lock

                    //dt.DefaultView.RowFilter = "Selected = 1";

                    //if (PickSeq.Checked)
                    //    dt.DefaultView.Sort = "PickSeq Asc";
                    //else
                    //    dt.DefaultView.Sort = "ItmSN Asc";


                    //var vLock = (from row in dt.DefaultView.ToTable().AsEnumerable()
                    //               select new
                    //               {
                    //                   DocKey = row.Field<int>("DocKey"),

                    //               }).Distinct();

                    //DataTable dt_Lock = vLock.AsDataTable();
                    IEnumerable<DataRow> dtFiltersToLock = dt.AsEnumerable().Where(r => r.Field<bool>("Selected") == true);
                    if (PickSeq.Checked)
                        dtFiltersToLock = dtFiltersToLock.OrderBy(r => r.Field<int?>("PickSeq"));
                    else
                        dtFiltersToLock = dtFiltersToLock.OrderBy(r => r.Field<decimal>("ItmSN"));

                    //DataTable dt_Lock = dtFiltersToLock.Select(r => new { DocKey = r.Field<int>("DocKey") }).Distinct().AsDataTable();
                    DataTable dt_Lock = dtFiltersToLock.Select(r => new { DocKey = r.Field<int>("DocKey"), DocItmKey = r.Field<int>("DocItmKey"), QtySelect = r.Field<decimal>("ItmQtyIssue"), PickSeq = r.Field<int>("PickSeq"), DocID = r.Field<string>("DocID"), ItmSN = r.Field<decimal>("ItmSN"), ItmID = r.Field<string>("ItmID") }).AsDataTable();

                    List<SqlParameter> paraList = new List<SqlParameter>();
                    paraList.Add(new SqlParameter("@GUID", objDoc.GUID));
                    paraList.Add(new SqlParameter("@SourceDocCodeKey", (int)GEnum.SystemCode.Sales_Order));
                    string xmlData = GFunc.ConvertDataTableToXML(dt_Lock);
                    paraList.Add(new SqlParameter("@xmlDocDetail", xmlData));

                    DataTable dtLockData = GFunc.ExecuteProc("Document_MultiLockCheck", paraList);
                    if (dtLockData.Rows.Count > 0)
                    {
                        //GEnum.MsgBoxButton result = MsgBoxGrid.Show("Those Item that has been lock will not be transferred,do you wish to continue?", dtLockData, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No);
                        //if (result == GEnum.MsgBoxButton.No)
                        //    return;
                        MsgBoxGrid.Show("Your selected Sales Order(s) has been locked. Appending Data Fails.", dtLockData, GEnum.MsgBoxIcon.Information, GEnum.MsgBoxButton.OK);
                        return;
                    }


                    #endregion

                    // if (this.AppendData(dt.DefaultView.ToTable(false, "DocKey", "DocItmKey")))
                    if (this.AppendData(dtFiltersToLock.Select(r => new { DocKey = r.Field<int>("DocKey"), DocItmKey = r.Field<int>("DocItmKey"), QtySelect = r.Field<decimal>("ItmQtyIssue"), PickSeq = r.Field<int>("PickSeq") }).AsDataTable()))
                    {
                        UpdateSalesOrder(dtFiltersToLock.CopyToDataTable());

                        //We need to remove the rest of the rows in the outstanding list and refresh the grid again to avoid
                        //situation where some item of the same DocKey has already been inserted to the caller document
                        DocKey.Text = string.Empty;
                        ItmKey.Text = string.Empty;
                        DataTable dtSO = tagrdItemSO.DataSource as DataTable;
                        for (int i = dtSO.Rows.Count - 1; i > -1; i--)
                        {
                            if ((bool)dtSO.Rows[i]["Selected"])
                            {
                                dtSO.Rows[i].Delete();
                            }
                        }

                        dtSO.AcceptChanges();
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

        }//---------------------changed 
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (UltraGridRow row in tagrdItemSO.Rows.GetFilteredInNonGroupByRows())
                {
                    Selected_Set(row, true);
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
            try
            {
                foreach (UltraGridRow row in tagrdItemSO.Rows)
                {
                    Selected_Set(row, false);
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
            this.Close();
        }//Completed

        //Methods   
        private void ComboDocID_Fill()
        {
            try
            {
                if ((!GFunc.IsNE(PrmDate.DateValue)))
                {
                    if (tagrdItemSO.DataSource != null)
                    {
                        DataTable dt = null;
                        dt = ((DataTable)tagrdItemSO.DataSource).DefaultView.ToTable();

                        DataTable dtDocKey = (from row in dt.AsEnumerable()
                                              where row.Field<int>("DocKey") > 0
                                              select new
                                              {
                                                  DocKey = row.Field<int>("DocKey"),
                                                  DocID = row.Field<string>("DocID"),
                                                  DocDate = row.Field<DateTime>("DocDate")
                                              }).Distinct().AsDataTable();

                        DocKey.DataSource = dtDocKey;
                    }
                }
                else
                {
                    this.DocKey.DataSource = null;
                }
                this.DocKey.SetValueTrigger(null, false);

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
        private void ComboItm_Fill()
        {

            if (tagrdItemSO.DataSource != null)
            {
                DataTable dt = null;
                dt = ((DataTable)tagrdItemSO.DataSource).DefaultView.ToTable();

                DataTable dtItem = (from row in dt.AsEnumerable()
                                    where row.Field<int>("ItmKey") > 0 && row.Field<int>("LineType") == (int)GEnum.RecDetailType.DItems
                                    select new
                                    {
                                        ItmKey = row.Field<int>("ItmKey"),
                                        ItmID = row.Field<string>("ItmID"),
                                        ItmDes = row.Field<string>("MstItmDes")
                                    }).Distinct().AsDataTable();

                ItmKey.DataSource = dtItem;
            }
            else
            {
                this.ItmKey.DataSource = null;
            }
            this.ItmKey.SetValueTrigger(null, false);


        }//Completed
        private bool AppendData(DataTable dtSelected)
        {
            try
            {
                int soureceDocCodeKey = (int)GEnum.SystemCode.Sales_Order;

                int SourceDocKey = GFunc.NEInt(DocKey.Value, 0);
                int SourceDocConKey = 0;

                List<SqlParameter> paraList = new List<SqlParameter>();

                paraList.Add(new SqlParameter("@GUID", objDoc.GUID));
                paraList.Add(new SqlParameter("@UserKey", AppInfor.CurrentUserKey));
                paraList.Add(new SqlParameter("@SourceDocCodeKey", soureceDocCodeKey));
                paraList.Add(new SqlParameter("@SourceDocKey", SourceDocKey));
                SqlParameter para = new SqlParameter("@SourceConKey", SourceDocConKey);
                para.Direction = ParameterDirection.Output;
                paraList.Add(para);
                paraList.Add(new SqlParameter("@DetailType", GEnum.Details.Doc_Itm));
                paraList.Add(new SqlParameter("@InsertAction", GEnum.InsertAction.InsertSO));
                string xmlData = GFunc.ConvertDataTableToXML(dtSelected);
                paraList.Add(new SqlParameter("@xmlDocDetail", xmlData));

                DataSet dsInsertData = GFunc.ExecuteProcDataSet("Document_DataTransfer", paraList);

                if (PickSeq.Checked)
                    DocHDRUtil.DocTransferData(soureceDocCodeKey, SourceDocKey, SourceDocConKey,
                    dsInsertData.Tables[0], objDoc, gdTgtDetail, (int)GEnum.InsertAction.InsertSO, "PickSeq,ItmSN,ItmDetSN", UseSystemPrice.Checked, false);

                else
                    DocHDRUtil.DocTransferData(soureceDocCodeKey, SourceDocKey, SourceDocConKey,
                   dsInsertData.Tables[0], objDoc, gdTgtDetail, (int)GEnum.InsertAction.InsertSO, "ItmSN,ItmDetSN", UseSystemPrice.Checked, false);



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
                GlobalUI.bRuningImport = false;
                DocUtility.bRuningImport = false;
            }
        }
        private void UpdateSalesOrder(DataTable dtUpdate)
        {
            try
            {
                //To update remarks and marking to the source ARSO
                if (UpdateChanges.Checked == true)
                {

                    DataTable dt_ItmSO = dtUpdate.DefaultView.ToTable();
                    dt_ItmSO.TableName = "dtItmSO";
                    string xmlData = GFunc.ConvertDataTableToXML(dt_ItmSO);

                    List<SqlParameter> paraList = new List<SqlParameter>();
                    paraList.Add(new SqlParameter("@DocKey", GFunc.NEInt(DocKey.Value, 0)));
                    paraList.Add(new SqlParameter("@xmlData", xmlData));
                    SqlParameter RetValue = new SqlParameter();
                    RetValue.ParameterName = "@RetValue";
                    RetValue.Value = 0;
                    RetValue.Direction = ParameterDirection.InputOutput;
                    paraList.Add(RetValue);
                    GFunc.ExecuteNonQueryProc("Doc_InsertSO_Update", paraList);
                    if (GFunc.NEInt(RetValue.Value, 0) == (int)GEnum.SpState.Fail)
                    {
                        MsgBox.Show("unable to update remarks and marking infor to respective Sales Order");
                    }
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
        private void FormLayout()
        {
            //GlobalUI.FormReadOnly_Set(tagrdItemSO);
            GlobalUI.GridAllColumnsActivateOnlySet(tagrdItemSO);
            tagrdItemSO.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            tagrdItemSO.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;

            tagrdItemSO.DisplayLayout.Bands[0].Columns["Selected"].CellActivation = Activation.AllowEdit;
            tagrdItemSO.DisplayLayout.Bands[0].Columns["ItmRem"].CellActivation = Activation.AllowEdit;
            tagrdItemSO.DisplayLayout.Bands[0].Columns["ItmMark"].CellActivation = Activation.AllowEdit;
            tagrdItemSO.DisplayLayout.Bands[0].Columns["ItmQtyIssue"].CellActivation = Activation.AllowEdit;


        }//Completed
        private bool Selected_Set(UltraGridRow grdrow, bool ValueToSet)
        {
            try
            {
                decimal qtyIssue = 0;

                if ((int)grdrow.Cells["LineType"].Value != (int)GEnum.RecDetailType.DItems)
                    return true;

                if (ValueToSet == false)
                {
                    grdrow.Cells["Selected"].Value = false;
                    grdrow.Cells["ItmQtyIssue"].Value = 0;
                    grdrow.Cells["pickSeq"].Value = 0;
                }
                else
                {
                    if ((int)grdrow.Cells["ItmType"].Value != (int)GEnum.ItemType.Charges)
                    {
                        if (this.UseMaxStock.Checked)
                        {
                            if ((decimal)grdrow.Cells["ItmStock"].Value >= (decimal)grdrow.Cells["QtyBalance"].Value)
                                qtyIssue = GFunc.RndDC((decimal)grdrow.Cells["QtyBalance"].Value, (decimal)grdrow.Cells["ItmConRate"].Value, GVar.RndDecs.Qtypt);
                            else
                                qtyIssue = GFunc.RndDC((decimal)grdrow.Cells["ItmStock"].Value, (decimal)grdrow.Cells["ItmConRate"].Value, GVar.RndDecs.Qtypt);
                        }
                        else
                        {
                            qtyIssue = (decimal)grdrow.Cells["QtyBalance"].Value;
                        }
                        grdrow.Cells["ItmQtyIssue"].Value = qtyIssue;
                    }

                    grdrow.Cells["Selected"].Value = true;

                    if (PickSeq.Checked)
                    {
                        pickSeqCounter++;
                        grdrow.Cells["pickSeq"].Value = pickSeqCounter;
                    }
                }
                grdrow.Update();
                return true;
            }
            catch (TAException tex)
            {
                return false;
                throw Error(tex, false);

            }
            catch (Exception ex)
            {
                return false;
                throw Error(ex, false);

            }
        }//Completed
        private void Grid_Refresh()
        {
            string listSetingID = "frmInsertSOGrid";
            try
            {
                GlobalUI.Grid_Format(tagrdItemSO, listSetingID, true, false);
                pickSeqCounter = 0;

                RemoveMatchedRow();
                //reload combo data
                ComboDocID_Fill();
                ComboItm_Fill();
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            

        }//---------------------changed
        private void Grid_Filter()
        {
            try
            {
                //Prepare Filter parameters                
                int docKey = GFunc.NEInt(DocKey.Value, 0);
                int itmKey = GFunc.NEInt(ItmKey.Value, 0);

                //Filter Grid
                //GridFilterToDefaultView   
                if (docKey > 0)
                    ((DataTable)tagrdItemSO.DataSource).DefaultView.RowFilter = "LineType=" + (int)GEnum.RecDetailType.DItems + " AND DocKey=" + docKey;

                if (itmKey > 0)
                    ((DataTable)tagrdItemSO.DataSource).DefaultView.RowFilter = "LineType=" + (int)GEnum.RecDetailType.DItems + " AND ItmKey=" + itmKey;

                ((DataTable)tagrdItemSO.DataSource).DefaultView.Sort = "ItmSN";

                foreach (UltraGridRow gRow in tagrdItemSO.Rows)
                {
                    if (GFunc.NEInt(gRow.Cells["ItmType"].Value, 0) == (int)GEnum.ItemType.Charges)
                    {
                        gRow.Cells["ItmQtyIssue"].Activation = Activation.ActivateOnly;
                    }
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
        }//---------------------changed

        private void RemoveMatchedRow()
        {
            #region Get the rows which matched with the caller grid
            DataTable dtso = tagrdItemSO.DataSource as DataTable;

            var local = from row in dtso.AsEnumerable()
                        select new
                        {
                            DocKey = row.Field<int>("DocKey"),
                            DocItmKey = row.Field<int>("DocItmKey")
                        };

            var caller = from row in dtTgtDetail.AsEnumerable()
                         select new
                         {
                             DocKey = row.Field<int>("ARSODK"),
                             DocItmKey = row.Field<int>("ARSODItm")
                         };

            DataTable dtexisting = (from L in local
                                    join C in caller
                                    on
                                    new { L.DocKey, L.DocItmKey }
                                    equals
                                    new { C.DocKey, C.DocItmKey }
                                    select L).AsDataTable();

            //sorting for delete row
            dtexisting.DefaultView.Sort = "DocKey,DocItmKey";
            dtso.DefaultView.Sort = "DocKey,DocItmKey";
            #endregion

            //remove those rows matched with caller grid
            for (int i = dtso.Rows.Count - 1; i > -1; i--)
            {
                var query = dtexisting.AsEnumerable().Where(p => p.Field<int>("DocKey") == GFunc.NEInt(dtso.Rows[i]["DocKey"], 0) && p.Field<int>("DocItmKey") == GFunc.NEInt(dtso.Rows[i]["DocItmKey"], 0));
                if (query.Count() > 0)
                {
                    dtso.Rows[i].Delete();
                }
            }
            dtso.AcceptChanges();
            tagrdItemSO.DataSource = dtso;             

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
