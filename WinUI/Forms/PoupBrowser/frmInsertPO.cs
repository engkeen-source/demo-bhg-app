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
using System.Collections;
using TAUtil;
namespace WinUI
{
    public partial class frmInsertPO : Form  
    {
        #region Local Variables
        private string ContextMenuSetting = string.Empty;
        private Document objDoc;       
        UltraGrid tagrdDetItms = null;
        int ConKey = 0;
        int CurrKey = 0;
        int vpickSeq = 0;      
        #endregion

        //Initialize
        public frmInsertPO()
        {
            InitializeComponent();
        }//Completed
        public frmInsertPO(Document doc, UltraGrid tagrdDetItms, int VendorKey)
        {
            InitializeComponent();

            this.objDoc = doc;           
            this.tagrdDetItms = tagrdDetItms;            
        }//Completed
                    
        //Form Events
        private void frmInsertPO_Load(object sender, EventArgs e)
        {
            try
            {
                PrmDate.DateValue = DateTime.Now;
                CurrKey = (int)GFunc.GetIntPropertyValue("DocCurrKey", this.objDoc);
                ConKey = (int)GFunc.GetIntPropertyValue("DocConKey", objDoc);

                GlobalUI.FormGrids_Set(this, (int)objDoc.DocCodeKey, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objDoc.DocCodeKey);
                //GlobalUI.cmnuGlobal_Set(this);
                GlobalUI.Combos_Fill(this, (int)objDoc.DocCodeKey);

                RemoveMatchedRow(); 

                ComboDocID_Fill();
                ComboItm_Fill();
                FormLayout();

                UseSystemPrice.Checked = SysOptionUtility.GetBool("InsertAPPOUseSysPrice");
                PickSeq.Checked = SysOptionUtility.GetBool("InsertAPPOAppendInvOnPickSeq");

                Grid_Filter();

                DocKey.Select();
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
        private void frmInsertPO_KeyDown(object sender, KeyEventArgs e)
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
        private void frmInsertPO_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hashtable docDet = new Hashtable();
            docDet.Add(GEnum.Details.Doc_Itm, tagrdDetItms);
            DocComUtility.CalForm(objDoc, docDet, true, false);
        }
        //Grid Events
        private void tagrdItemPO_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            UltraGridCell curCell = tagrdItemPO.ActiveCell;
            try
            {
                switch (curCell.Column.Key.ToLower())
                {
                    case "itmqtyreceive":
                        if (GFunc.IsNEZ(curCell.Value))
                            tagrdItemPO.ActiveRow.Cells["Selected"].Value = 0;
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
                            tagrdItemPO.ActiveRow.Cells["Selected"].Value = 1;
                            if (PickSeq.Checked)
                            {
                                vpickSeq++;
                                tagrdItemPO.ActiveRow.Cells["pickSeq"].Value = vpickSeq;
                            }
                        }
                        break;
                    case "selected":
                        if (tagrdItemPO.ActiveRow != null)
                        {
                            Selected_Set(tagrdItemPO.ActiveRow, (bool)curCell.Value);
                            tagrdItemPO.UpdateData();
                        }
                        //if (PickSeq.Checked)
                        //{
                        //    vpickSeq += 1;
                        //    tagrdItemPO.ActiveRow.Cells["PickSeq"].Value = vpickSeq;
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

        //Button Events
        private void btnAppend_Click(object sender, EventArgs e)
        {
            try
            {
                this.tagrdItemPO.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdItemPO.UpdateData();

                DataTable dt = tagrdItemPO.DataSource as DataTable;
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

                DataTable dt_Lock = dtFiltersToLock.Select(r => new { DocKey = r.Field<int>("DocKey"), DocItmKey = r.Field<int>("DocItmKey"), QtySelect = r.Field<decimal>("ItmQtyReceive"), PickSeq = r.Field<int>("PickSeq"), DocID = r.Field<string>("DocID"), ItmSN = r.Field<decimal>("ItmSN"), ItmID = r.Field<string>("ItmID") }).AsDataTable();

                List<SqlParameter> paraList = new List<SqlParameter>();
                paraList.Add(new SqlParameter("@GUID", objDoc.GUID));
                paraList.Add(new SqlParameter("@SourceDocCodeKey", (int)GEnum.SystemCode.Purchase_Order ));
                string xmlData = GFunc.ConvertDataTableToXML(dt_Lock);
                paraList.Add(new SqlParameter("@xmlDocDetail", xmlData));

                DataTable dtLockData = GFunc.ExecuteProc("Document_MultiLockCheck", paraList);
                if (dtLockData.Rows.Count > 0)
                {
                    //GEnum.MsgBoxButton result = MsgBoxGrid.Show("Those Item that has been lock will not be transferred,do you wish to continue?", dtLockData, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No);
                    //if (result == GEnum.MsgBoxButton.No)
                    //    return;
                    MsgBoxGrid.Show("Your selected Purchase Order(s) has been locked. Appending Data Fails.", dtLockData, GEnum.MsgBoxIcon.Information,GEnum.MsgBoxButton.OK);
                    return;
                }
                #endregion

                //if (AppendData(dt.DefaultView.ToTable(false, "DocKey", "DocItmKey")) == true)
                if (this.AppendData(dtFiltersToLock.Select(r => new { DocKey = r.Field<int>("DocKey"), DocItmKey = r.Field<int>("DocItmKey"),QtySelect = r.Field<decimal>("ItmQtyReceive"), PickSeq = r.Field<int>("PickSeq") }).AsDataTable()))
                {
                    //We need to remove the rest of the rows in the outstanding list and refresh the grid again to avoid
                    //situation where some item of the same DocKey has already been inserted to the caller document
                    DocKey.Text = string.Empty;
                    ItmKey.Text = string.Empty;                    

                    DataTable dtPO = tagrdItemPO.DataSource as DataTable;
                    for (int i = dtPO.Rows.Count - 1; i > -1; i--)
                    {
                        if ((bool)dtPO.Rows[i]["Selected"])
                        {
                            dtPO.Rows[i].Delete();
                        }
                    }
                    dtPO.AcceptChanges();
                    ComboDocID_Fill(dtPO); /* added by yst on 29 dec 2018 to eliminate used DocID in Popup */
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
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {

                //foreach (UltraGridRow row in tagrdItemPO.Rows.GetFilteredInNonGroupByRows())
                //{

                //    row.Cells["Selected"].Value = true;
                //    if (PickSeq.Checked)
                //    {
                //        vpickSeq++;
                //        row.Cells["pickSeq"].Value = vpickSeq;
                //    }

                //    row.Update();
                //}

                foreach (UltraGridRow row in tagrdItemPO.Rows.GetFilteredInNonGroupByRows())
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
                foreach (UltraGridRow row in tagrdItemPO.Rows)
                {
                    Selected_Set(row, false);
                }
                //foreach (UltraGridRow row in tagrdItemPO.Rows)
                //{                   
                //    row.Cells["Selected"].Value = false ;                
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
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }//Completed

        //Method   
        private bool AppendData(DataTable dtSelected)
        {
            try
            {
                int soureceDocCodeKey = (int)GEnum.SystemCode.Purchase_Order;
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
                paraList.Add(new SqlParameter("@InsertAction", GEnum.InsertAction.InsertPO));
                string xmlData = GFunc.ConvertDataTableToXML(dtSelected);
                paraList.Add(new SqlParameter("@xmlDocDetail", xmlData));

                DataTable dtInsertData = GFunc.ExecuteProc("Document_DataTransfer", paraList);

                if (PickSeq.Checked) 
                      DocHDRUtil.DocTransferData(soureceDocCodeKey, SourceDocKey, SourceDocConKey,
                    dtInsertData, objDoc, tagrdDetItms,(int)GEnum.InsertAction.InsertPO, "PickSeq,ItmSN,ItmDetSN", UseSystemPrice.Checked, false);

                else 
                  DocHDRUtil.DocTransferData(soureceDocCodeKey, SourceDocKey, SourceDocConKey,
                    dtInsertData, objDoc, tagrdDetItms,(int)GEnum.InsertAction.InsertPO, "ItmSN,ItmDetSN", UseSystemPrice.Checked, false);

              
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
        }//Completed
        private void ComboItm_Fill()
        {
            
            if (tagrdItemPO.DataSource != null)
            {
                DataTable dt = null;
                dt = ((DataTable)tagrdItemPO.DataSource).DefaultView.ToTable();

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
            
        }//Completed
        private void FormLayout()
        {
            //GlobalUI.FormReadOnly_Set(tagrdItemPO);

            GlobalUI.GridAllColumnsActivateOnlySet(tagrdItemPO);

            tagrdItemPO.DisplayLayout.Bands[0].Columns["Selected"].Header.VisiblePosition = 0;
            tagrdItemPO.DisplayLayout.Bands[0].Columns["Selected"].CellActivation = Activation.AllowEdit;            
            tagrdItemPO.DisplayLayout.Bands[0].Columns["ItmQtyReceive"].CellActivation = Activation.AllowEdit;
           
        }//Completed
        private void ComboDocID_Fill(DataTable dtPO = null)
        {
            try
            {
                if ((!GFunc.IsNE(PrmDate.DateValue)))
                {
                    if (tagrdItemPO.DataSource != null)
                    {
                        DataTable dt = null;
                        dt = dtPO == null ? ((DataTable)tagrdItemPO.DataSource).DefaultView.ToTable() : dtPO; /* added by yst on 28 dec 2018 to eliminate used DocID in Popup*/

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
        private void Grid_Refresh()
        {
            string listSetingID = "frmInsertPOGrid";
            try
            {
                GlobalUI.Grid_Format(tagrdItemPO, listSetingID, true, false);          
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
                    ((DataTable)tagrdItemPO.DataSource).DefaultView.RowFilter = "DocKey=" + docKey;

                if (itmKey > 0)
                    ((DataTable)tagrdItemPO.DataSource).DefaultView.RowFilter = "ItmKey=" + itmKey;

                ((DataTable)tagrdItemPO.DataSource).DefaultView.Sort = "ItmSN";

                foreach (UltraGridRow gRow in tagrdItemPO.Rows)
                {
                    if (GFunc.NEInt(gRow.Cells["ItmType"].Value, 0) == (int)GEnum.ItemType.Charges)
                    {
                        gRow.Cells["ItmQtyReceive"].Activation = Activation.ActivateOnly;
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
                    grdrow.Cells["ItmQtyReceive"].Value = 0;
                    grdrow.Cells["pickSeq"].Value = 0;
                }
                else
                {
                    if ((int)grdrow.Cells["ItmType"].Value != (int)GEnum.ItemType.Charges)
                    {
                        qtyIssue = GFunc.NEDec(grdrow.Cells["QtyBalance"].Value,0);
                        grdrow.Cells["ItmQtyReceive"].Value = qtyIssue;
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
            try
            {
                #region Get the rows which matched with the caller grid
                DataTable dtPO = tagrdItemPO.DataSource as DataTable;
                DataTable dtTgdtDetail = tagrdDetItms.DataSource as DataTable;

                var local = from row in dtPO.AsEnumerable().Where(p => p.Field<int>("ItmType") == (int)GEnum.ItemType.Stock || p.Field<int>("ItmType") == (int)GEnum.ItemType.Non_Stock)
                            select new
                            {
                                DocKey = row.Field<int>("DocKey"),
                                DocItmKey = row.Field<int>("DocItmKey"),
                                ItmQty = row.Field<decimal>("QtyBalance") /* added by yst on 30 dec 2018 show balance qty row*/
                            };

                var caller = from row in dtTgdtDetail.AsEnumerable().Where(p => p.Field<int>("ItmType") == (int)GEnum.ItemType.Stock || p.Field<int>("ItmType") == (int)GEnum.ItemType.Non_Stock)
                             select new
                             {
                                 DocKey = row.Field<int>("APPODK"),
                                 DocItmKey = row.Field<int>("APPODItm"),
                                 ItmQty = row.Field<decimal>("ItmQty") /* added by yst on 30 dec 2018 show balance qty row*/
                             };

                DataTable dtexisting = (from L in local
                                        join C in caller
                                        on
                                        new { L.DocKey, L.DocItmKey,L.ItmQty }
                                        equals
                                        new { C.DocKey, C.DocItmKey,C.ItmQty }
                                        select L).AsDataTable();

                //sorting for delete row
                dtexisting.DefaultView.Sort = "DocKey,DocItmKey";
                dtPO.DefaultView.Sort = "DocKey,DocItmKey";
                #endregion

                //remove those rows matched with caller grid
                for (int i = dtPO.Rows.Count - 1; i > -1; i--)
                {
                    var query = dtexisting.AsEnumerable().Where(p => p.Field<int>("DocKey") == GFunc.NEInt(dtPO.Rows[i]["DocKey"], 0) && p.Field<int>("DocItmKey") == GFunc.NEInt(dtPO.Rows[i]["DocItmKey"], 0));
                    if (query.Count() > 0)
                    {
                        dtPO.Rows[i].Delete();
                    }
                }
                dtPO.AcceptChanges();
                tagrdItemPO.DataSource = dtPO;
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
