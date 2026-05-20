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
using System.Transactions;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using System.Collections;
using TAUtil;



namespace WinUI
{
    public partial class frmInsertCSI : Form
    {

        #region Local Variable

        private string ContextMenuSetting = string.Empty;
        private Document objDoc;
        private UltraGrid tagrdDetItms;
        private UltraGrid tagrdDetExp;
        private GEnum.InsertAction insertAction = GEnum.InsertAction.InsertCS;
        int vpickSeq = 0;
        int ConKey = 0;
        int CurrKey = 0;

        #endregion

        //Initialize
        public frmInsertCSI()
        {
            InitializeComponent();
        }//Completed
        public frmInsertCSI(Document doc, UltraGrid ptagrdDetItms)
        {
            //note: use by insertCS to Sales Document
            InitializeComponent();
            this.objDoc = doc;
            tagrdDetItms = ptagrdDetItms;
        }//Completed
        public frmInsertCSI(Document doc, UltraGrid ptagrdDetItms,UltraGrid ptagrdDetExp)
        {
            //note: use by insertCS to return Consignment
            InitializeComponent();
            this.objDoc = doc;
            if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Return_Consignment)
            {
                this.insertAction = GEnum.InsertAction.InsertCSR;
            }
            tagrdDetItms = ptagrdDetItms;
            tagrdDetExp = ptagrdDetExp;
        }//Completed

        //Form Event
        private void frmInsertCSI_Load(object sender, EventArgs e)
        {
            try
            {
                DocDate.DateValue = DateTime.Now;
                CurrKey = (int)GFunc.GetIntPropertyValue("DocCurrKey", this.objDoc);
                ConKey = (int)GFunc.GetIntPropertyValue("DocConKey", objDoc);
           
                //Set ContextMenu & Grid Setting                  
                GlobalUI.FormGrids_Set(this, (int)objDoc.DocCodeKey, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objDoc.DocCodeKey);
                //GlobalUI.cmnuGlobal_Set(this);
                GlobalUI.Combos_Fill(this, (int)objDoc.DocCodeKey);
                RemoveMatchedRow();
                ComboDocID_Fill();
                ComboItm_Fill();
                FormLayout();

                //Set Default Value                
                UseSystemPrice.Checked = SysOptionUtility.GetBool("InsertSettlementUseSysPrice");
                PickSeq.Checked = SysOptionUtility.GetBool("InsertSettlementAppendInvOnPickSeq");

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
        private void frmInsertCSI_KeyDown(object sender, KeyEventArgs e)
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
        private void frmInsertCSI_FormClosed(object sender, FormClosedEventArgs e)
        {

            Hashtable docDet = new Hashtable();
            docDet.Add(GEnum.Details.Doc_Itm, tagrdDetItms);
            DocComUtility.CalForm(objDoc, docDet, true, false);

        }
        //Grid Event       
        private void tagrdItemCSI_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            UltraGridCell curCell = tagrdItemCSI.ActiveCell;

            try
            {
                switch (curCell.Column.Key.ToLower())
                {
                    case "itmqtyissue":
                        if (GFunc.IsNEZ(curCell.Value))
                            tagrdItemCSI.ActiveRow.Cells["Selected"].Value = 0;
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
                            tagrdItemCSI.ActiveRow.Cells["Selected"].Value = 1;
                            if (PickSeq.Checked)
                            {
                                vpickSeq++;
                                tagrdItemCSI.ActiveRow.Cells["pickSeq"].Value = vpickSeq;
                            }
                        }
                        break;
                    case "selected":
                        if (tagrdItemCSI.ActiveRow != null)
                        {
                            Selected_Set(tagrdItemCSI.ActiveRow, (bool)curCell.Value);
                            tagrdItemCSI.UpdateData();
                        }
                        //if (PickSeq.Checked)
                        //{
                        //    vpickSeq += 1;
                        //    tagrdItemCSI.ActiveRow.Cells["PickSeq"].Value = vpickSeq;
                        //}
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

        //Button Events
        private void btnAppend_Click(object sender, EventArgs e)
        {
            try
            {
                this.tagrdItemCSI.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdItemCSI.UpdateData();

                DataTable dt = tagrdItemCSI.DataSource as DataTable;
                #region Check and add lock

                //dt.DefaultView.RowFilter = "Selected = 1";

                //if (PickSeq.Checked)
                //    dt.DefaultView.Sort = "PickSeq Asc";
                //else
                //    dt.DefaultView.Sort = "ItmSN Asc";

               
                //var vLock = (from row in dt.DefaultView.ToTable().AsEnumerable()
                //             select new
                //             {
                //                 DocKey = row.Field<int>("DocKey"),

                //             }).Distinct();

                //DataTable dt_Lock = vLock.AsDataTable();

                IEnumerable<DataRow> dtFiltersToLock = dt.AsEnumerable().Where(r => r.Field<bool>("Selected") == true);
                if (PickSeq.Checked)
                    dtFiltersToLock = dtFiltersToLock.OrderBy(r => r.Field<int?>("PickSeq"));
                else
                    dtFiltersToLock = dtFiltersToLock.OrderBy(r => r.Field<decimal>("ItmSN"));

                DataTable dt_Lock = dtFiltersToLock.Select(r => new { DocKey = r.Field<int>("DocKey"), DocItmKey = r.Field<int>("DocItmKey"), QtySelect = r.Field<decimal>("ItmQtyIssue"), PickSeq = r.Field<int>("PickSeq"), DocID = r.Field<string>("DocID"), ItmSN = r.Field<decimal>("ItmSN"), ItmID = r.Field<string>("ItmID") }).AsDataTable();

                List<SqlParameter> paraList = new List<SqlParameter>();
                paraList.Add(new SqlParameter("@GUID", objDoc.GUID));
                paraList.Add(new SqlParameter("@SourceDocCodeKey", (int)GEnum.SystemCode.Issue_Consignment));
                string xmlData = GFunc.ConvertDataTableToXML(dt_Lock);
                paraList.Add(new SqlParameter("@xmlDocDetail", xmlData));

                DataTable dtLockData = GFunc.ExecuteProc("Document_MultiLockCheck", paraList);
                if (dtLockData.Rows.Count > 0)
                {
                    //GEnum.MsgBoxButton result = MsgBoxGrid.Show("Those Item that has been lock will not be transferred,do you wish to continue?", dtLockData, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No);
                    //if (result == GEnum.MsgBoxButton.No)
                    //    return;
                    MsgBoxGrid.Show("Your selected Consignment has been locked. Appending Data Fails.", dtLockData, GEnum.MsgBoxIcon.Information, GEnum.MsgBoxButton.OK);
                    return;
                }
                #endregion

                //if (AppendData(dt.DefaultView.ToTable(false, "DocKey", "DocItmKey")) == true)
                if (this.AppendData(dtFiltersToLock.Select(r => new { DocKey = r.Field<int>("DocKey"), DocItmKey = r.Field<int>("DocItmKey"), QtySelect = r.Field<decimal>("ItmQtyIssue"), PickSeq = r.Field<int>("PickSeq") }).AsDataTable()))
                {
                    //We need to remove the rest of the rows in the outstanding list and refresh the grid again to avoid
                    //situation where some item of the same DocKey has already been inserted to the caller document
                    DocKey.Text = string.Empty;
                    ItmKey.Text = string.Empty;                    

                    DataTable dtCSI = tagrdItemCSI.DataSource as DataTable;
                    for (int i = dtCSI.Rows.Count - 1; i > -1; i--)
                    {
                        if ((bool)dtCSI.Rows[i]["Selected"])
                        {
                            dtCSI.Rows[i].Delete();
                        }
                    }

                    dtCSI.AcceptChanges();

                 
  
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
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {

                foreach (UltraGridRow row in tagrdItemCSI.Rows.GetFilteredInNonGroupByRows())
                {
                    Selected_Set(row, true);
                }
                //foreach (UltraGridRow row in tagrdItemCSI.Rows.GetFilteredInNonGroupByRows())
                //{

                //    row.Cells["Selected"].Value = true;
                //    if (PickSeq.Checked)
                //    {
                //        vpickSeq++;
                //        row.Cells["pickSeq"].Value = vpickSeq;
                //    }

                //    row.Update();
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
        private void btnUnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (UltraGridRow row in tagrdItemCSI.Rows)
                {
                    Selected_Set(row, false);
                }
                //foreach (UltraGridRow row in tagrdItemCSI.Rows)
                //{
                //    row.Cells["Selected"].Value = false;
                //    row.Update();
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

        //Control Events
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
        }//Completed
        private void DocDate_CustomUpdate(object sender, CancelEventArgs e)
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
        }//Completed
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

        //Methods
        private bool AppendData(DataTable dtSelected)
        {
            try
            {                
                int soureceDocCodeKey = (int)GEnum.SystemCode.Issue_Consignment;
                int sourceDocKey = GFunc.NEInt(dtSelected.Rows[0]["DocKey"], 0);

                int SourceDocConKey = 0;

                List<SqlParameter> paraList = new List<SqlParameter>();
                paraList.Add(new SqlParameter("@GUID", objDoc.GUID));
                paraList.Add(new SqlParameter("@UserKey", AppInfor.CurrentUserKey));
                paraList.Add(new SqlParameter("@SourceDocCodeKey", soureceDocCodeKey));
                paraList.Add(new SqlParameter("@SourceDocKey", sourceDocKey));
                SqlParameter para = new SqlParameter("@SourceConKey", SourceDocConKey);
                para.Direction = ParameterDirection.Output;
                paraList.Add(para);
                paraList.Add(new SqlParameter("@DetailType", GEnum.Details.Doc_Itm));
                paraList.Add(new SqlParameter("@InsertAction", this.insertAction));
                string xmlData = GFunc.ConvertDataTableToXML(dtSelected);
                paraList.Add(new SqlParameter("@xmlDocDetail", xmlData));

                DataTable dtInsertData = GFunc.ExecuteProc("Document_DataTransfer", paraList);

                if (PickSeq.Checked) 
                       DocHDRUtil.DocTransferData(soureceDocCodeKey, sourceDocKey, SourceDocConKey,
                    dtInsertData, objDoc, tagrdDetItms, tagrdDetExp, (int)GEnum.InsertAction.InsertCS, "PickSeq,ItmSN,ItmDetSN", UseSystemPrice.Checked, false);
                else 
                       DocHDRUtil.DocTransferData(soureceDocCodeKey, sourceDocKey, SourceDocConKey,
                    dtInsertData, objDoc, tagrdDetItms,tagrdDetExp, (int)GEnum.InsertAction.InsertCS, "ItmSN,ItmDetSN", UseSystemPrice.Checked, false);

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
        private void FormLayout()
        {
            GlobalUI.GridAllColumnsActivateOnlySet(tagrdItemCSI);

            tagrdItemCSI.DisplayLayout.Bands[0].Columns["Selected"].CellActivation = Activation.AllowEdit;
            tagrdItemCSI.DisplayLayout.Bands[0].Columns["Selected"].Header.VisiblePosition = 0;
            tagrdItemCSI.DisplayLayout.Bands[0].Columns["ItmQtyIssue"].CellActivation = Activation.AllowEdit;
        }//Completed
        private void ComboItm_Fill()
        {

            if (tagrdItemCSI.DataSource != null)
            {
                DataTable dt = null;
                dt = ((DataTable)tagrdItemCSI.DataSource).DefaultView.ToTable();

                DataTable dtItem = (from row in dt.AsEnumerable()
                                    where row.Field<int>("ItmKey") > 0
                                    select new
                                    {
                                        ItmKey = row.Field<int>("ItmKey"),
                                        ItmID = row.Field<string>("ItmID"),
                                        ItmDes = row.Field<string>("MstItmDes")
                                    }).Distinct().AsDataTable();

                ItmKey.DataSource = dtItem;
            }

        }       //Completed
        private void Grid_Refresh()
        {
            try
            {
                string listSetingID = "frmInsertCSGrid";
                GlobalUI.Grid_Format(tagrdItemCSI, listSetingID, true, false);
               
                vpickSeq = 0;

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
            

        }//Completed
        private void ComboDocID_Fill()
        {
            try
            {
                if ((!GFunc.IsNE(DocDate.DateValue)))
                {
                    if (tagrdItemCSI.DataSource != null)
                    {
                        DataTable dt = null;
                        dt = ((DataTable)tagrdItemCSI.DataSource).DefaultView.ToTable();

                        DataTable dtPurchaseOrderNo = (from row in dt.AsEnumerable()
                                                       where row.Field<int>("DocKey") > 0
                                                       select new
                                                       {
                                                           DocKey = row.Field<int>("DocKey"),
                                                           DocID = row.Field<string>("DocID"),
                                                           DocDate = row.Field<DateTime>("DocDate")
                                                       }).Distinct().AsDataTable();

                        DocKey.DataSource = dtPurchaseOrderNo;
                    }

                    this.DocKey.Enabled = true;
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
                    ((DataTable)tagrdItemCSI.DataSource).DefaultView.RowFilter = "DocKey=" + docKey;

                if (itmKey > 0)
                    ((DataTable)tagrdItemCSI.DataSource).DefaultView.RowFilter = "ItmKey=" + itmKey;

                ((DataTable)tagrdItemCSI.DataSource).DefaultView.Sort = "ItmSN";
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
        private bool Selected_Set(UltraGridRow grdrow, bool ValueToSet)
        {
            decimal qtyIssue = 0;
            try
            {
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
                        qtyIssue = GFunc.NEDec(grdrow.Cells["QtyBalance"].Value, 0);
                        grdrow.Cells["ItmQtyIssue"].Value = qtyIssue;
                    }

                    grdrow.Cells["Selected"].Value = true;

                    if (PickSeq.Checked)
                    {
                        vpickSeq++;
                        grdrow.Cells["pickSeq"].Value = vpickSeq;
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
        private void RemoveMatchedRow()
        {
            #region Get the rows which matched with the caller grid
            DataTable dtCSI = tagrdItemCSI.DataSource as DataTable;
            DataTable dtTgtDetail = tagrdDetItms.DataSource as DataTable;

            var local = from row in dtCSI.AsEnumerable()
                        select new
                        {
                            DocKey = row.Field<int>("DocKey"),
                            DocItmKey = row.Field<int>("DocItmKey")
                        };

            var caller = from row in dtTgtDetail.AsEnumerable()
                         select new
                         {
                             DocKey = row.Field<int>("CSCSIDK"),
                             DocItmKey = row.Field<int>("CSCSIDItm")
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
            dtCSI.DefaultView.Sort = "DocKey,DocItmKey";
            #endregion

            //remove those rows matched with caller grid
            for (int i = dtCSI.Rows.Count - 1; i > -1; i--)
            {
                var query = dtexisting.AsEnumerable().Where(p => p.Field<int>("DocKey") == GFunc.NEInt(dtCSI.Rows[i]["DocKey"], 0) && p.Field<int>("DocItmKey") == GFunc.NEInt(dtCSI.Rows[i]["DocItmKey"], 0));
                if (query.Count() > 0)
                {
                    dtCSI.Rows[i].Delete();
                }
            }
            dtCSI.AcceptChanges();
            tagrdItemCSI.DataSource = dtCSI;

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
