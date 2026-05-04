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
    public partial class frmInsertSalesPO : Form  
    {
        #region Local Variables
        private string ContextMenuSetting = string.Empty;
        private Document objDoc;
        bool CallfromNSLink = false; 
        UltraGrid tagrdDetItms = null;
        int ConKey = 0;       
        int PODocKey = 0;
        int PODocItmKey = 0;
        int FilterItmKey = 0;
        #endregion

        //Initialize
        public frmInsertSalesPO()
        {
            InitializeComponent();
        }//Completed
        public frmInsertSalesPO(Document doc, UltraGrid tagrdDetItms,int PODocKey,int PODocItmKey)
        {
            InitializeComponent();

            this.objDoc = doc;           
            this.tagrdDetItms = tagrdDetItms;
            this.PODocKey = PODocKey;
            this.PODocItmKey = PODocItmKey;
            if (tagrdDetItms.ActiveRow != null)
                this.FilterItmKey = GFunc.NEInt(tagrdDetItms.ActiveRow.Cells["ItmKey"].Value, 0);
        }//Completed

        public frmInsertSalesPO(UltraGrid tagrdDetItms, int PODocKey, int PODocItmKey)
        {
            InitializeComponent();
          
            this.tagrdDetItms = tagrdDetItms;
            this.PODocKey = PODocKey;
            this.PODocItmKey = PODocItmKey;
        
            this.CallfromNSLink = true;
            if (tagrdDetItms.ActiveRow != null)
                this.FilterItmKey = GFunc.NEInt(tagrdDetItms.ActiveRow.Cells["ItmKey"].Value, 0);
        }//Completed

        //Form Events
        private void frmInsertSalesPO_Load(object sender, EventArgs e)
        {
            try
            {
                PrmDate.DateValue = DateTime.Now;
               
                ConKey = 0;
                int codekey = 11350;
                if (!CallfromNSLink)
                {
                    codekey = objDoc.DocCodeKey.Value;                   
                }

                GlobalUI.FormGrids_Set(this, codekey, out ContextMenuSetting);//11350
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(codekey);
                //GlobalUI.cmnuGlobal_Set(this);
                GlobalUI.Combos_Fill(this, codekey);

             
               // RemoveMatchedRow();
                ComboDocID_Fill();
            
                FormLayout();

                if (PODocKey > 0)
                {
                    DocKey.Value = PODocKey;
                }
                Grid_Filter();


                if (PODocItmKey > 0)
                {
                    DocKey.Select();
                    var row = tagrdDetItms.Rows.FirstOrDefault(r => (GFunc.NEInt(r.Cells["DocKey"].Value, 0) == PODocKey && GFunc.NEInt(r.Cells["DocItmKey"].Value, 0) == PODocItmKey));
                    if (row != null)
                        row.Selected = true;
                }
                else
                    ConKey1.Focus();
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
                    if(!CallfromNSLink)
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
            if (!CallfromNSLink)
            {
                Hashtable docDet = new Hashtable();
                docDet.Add(GEnum.Details.Doc_Itm, tagrdDetItms);
                DocComUtility.CalForm(objDoc, docDet, true, false);
            }
        }            

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
                errorProvider1.Clear();
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
        //private void ItmKey_CustomUpdate(object sender, CancelEventArgs e)
        //{
        //    try
        //    {
        //        Grid_Filter();
        //    }
        //    catch (TAException tex)
        //    {
        //        Error(tex, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        Error(ex, true);
        //    }
        //    finally
        //    {
        //        this.Cursor = Cursors.Default;
        //    }
        //}//Completed
        private void ConKey1_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                PODocKey = 0;
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
        }
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
                errorProvider1.SetError(DocKey, "Check if PO Num is correct.\nCheck if the document state of PO Num is released or posted.\nCheck if it has matching Item ID or Vendor ID.");
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
     
        private void btnLink_Click(object sender, EventArgs e)
        {
            try
            {
                this.tagrdItemPO.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdItemPO.UpdateData();

                if(this.tagrdItemPO.Selected.Rows.Count>0)
                {
                    if (GFunc.IsPostingItmType(tagrdItemPO.Selected.Rows[0].Cells["ItmType"].Value))
                    {
                        tagrdDetItms.ActiveRow.Cells["NSLink"].Value = "13250-" + PODocKey + "-" +PODocItmKey;
                        if (!CallfromNSLink)
                        {
                            tagrdDetItms.ActiveRow.Cells["APPOID"].Value = tagrdItemPO.Selected.Rows[0].Cells["DocID"].Value;
                            tagrdDetItms.ActiveRow.Cells["APPOID"].Tag = tagrdItemPO.Selected.Rows[0].Cells["DocID"].Value;
                        }
                        else
                        {
                            tagrdDetItms.ActiveRow.Cells["ItmPOID"].Value = tagrdItemPO.Selected.Rows[0].Cells["DocID"].Value;
                            tagrdDetItms.ActiveRow.Cells["ItmPOSN"].Value = tagrdItemPO.Selected.Rows[0].Cells["ItmSN"].Value;
                            tagrdDetItms.ActiveRow.Cells["ItmPODK"].Value = tagrdItemPO.Selected.Rows[0].Cells["DocKey"].Value;
                            tagrdDetItms.ActiveRow.Cells["ItmPODItm"].Value = tagrdItemPO.Selected.Rows[0].Cells["DocItmKey"].Value;

                            if (GFunc.IsNEZ(tagrdItemPO.Selected.Rows[0].Cells["ItmQty"].Value))
                                tagrdDetItms.ActiveRow.Cells["ItmVendorPriceH"].Value = tagrdItemPO.Selected.Rows[0].Cells["ItmAmtH"].Value;
                            else
                                tagrdDetItms.ActiveRow.Cells["ItmVendorPriceH"].Value =GFunc.RndDC(tagrdItemPO.Selected.Rows[0].Cells["ItmAmtH"].Value,
                                    tagrdItemPO.Selected.Rows[0].Cells["ItmQty"].Value, 6);

                        }
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MsgBox.Show("The selected item cannot be linked as a purchase cost. Please select an item which has cost.");                        
                    }
                }
                else
                {
                    MsgBox.Show("Please select item from the list below.");
                    return;
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
            this.DialogResult = DialogResult.Cancel;
        }//Completed
       

        private void FormLayout()
        {
            //GlobalUI.FormReadOnly_Set(tagrdItemPO);

            GlobalUI.GridAllColumnsActivateOnlySet(tagrdItemPO);
                    
            tagrdItemPO.DisplayLayout.Bands[0].Columns["ItmQtyReceive"].CellActivation = Activation.AllowEdit;
            tagrdItemPO.DisplayLayout.Bands[0].Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;
           
        }//Completed
        private void ComboDocID_Fill()
        {
            try
            {
                if ((!GFunc.IsNE(PrmDate.DateValue)))
                {
                    if (tagrdItemPO.DataSource != null)
                    {
                        DataTable dt = null;
                        dt = ((DataTable)tagrdItemPO.DataSource).DefaultView.ToTable();

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

        private void btnRequery_Click(object sender, EventArgs e)
        {
            Grid_Refresh();
        }

        private void Grid_Refresh()
        {
            ConKey = GFunc.NEInt(ConKey1.Value, 0);

            string listSetingID = "frmInsertSalesPOGrid";
            try
            {
                GlobalUI.Grid_Format(tagrdItemPO, listSetingID, true, false);                                       
                ComboDocID_Fill();               
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

                //Filter Grid               
                //GridFilterToDefaultView   
                if (docKey > 0)
                {
                    ((DataTable)tagrdItemPO.DataSource).DefaultView.RowFilter = "DocKey=" + docKey;
                    if (ConKey == 0)
                    {
                        if (((DataTable)tagrdItemPO.DataSource).DefaultView.Count > 0)
                        {
                            ConKey1.Value = ((DataTable)tagrdItemPO.DataSource).DefaultView[0]["DocConKey"];
                            ConKey = GFunc.NEInt(ConKey1.Value, 0);
                            foreach(DataRowView dr in ((DataTable)tagrdItemPO.DataSource).DefaultView)
                                if (GFunc.NEInt(dr["DocItmKey"], 0) == PODocItmKey)
                                {
                                    DocSN.Text = GFunc.NEStr(dr["ItmSN"], "");
                                    break;
                                }

                            ((DataTable)tagrdItemPO.DataSource).DefaultView.Sort = "ItmSN";
                        }
                    }
                }                
               
                //foreach (UltraGridRow gRow in tagrdItemPO.Rows)
                //{
                //    if (GFunc.NEInt(gRow.Cells["ItmType"].Value, 0) == (int)GEnum.ItemType.Charges)
                //    {
                //        gRow.Cells["ItmQtyReceive"].Activation = Activation.ActivateOnly;
                //    }
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
        }

        private void tagrdItemPO_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {          
            if (tagrdItemPO.Selected != null)
            {
                Infragistics.Win.UltraWinGrid.UltraGridRow r = null;
                if (tagrdItemPO.Selected.Rows.Count > 0)
                    r = tagrdItemPO.Selected.Rows[0];
                else if(tagrdItemPO.Selected.Cells.Count>0)
                    r = tagrdItemPO.Selected.Cells[0].Row;
                if (r != null)
                {
                    PODocKey = GFunc.NEInt(r.Cells["DocKey"].Value, 0);
                    ConKey = GFunc.NEInt(r.Cells["DocConKey"].Value, 0);
                    PODocItmKey = GFunc.NEInt(r.Cells["DocItmKey"].Value, 0);
                    DocKey.Value = PODocKey;
                    ConKey1.Value = ConKey;
                    DocSN.Text = GFunc.NEStr(r.Cells["ItmSN"].Value, "");
                }
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

        private void tagrdItemPO_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            btnLink_Click(sender, e);
        }
    }    

}
