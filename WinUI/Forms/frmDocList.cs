using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using BOLib;
using TAUtil;

namespace WinUI
{
    public partial class frmDocList : Form
    {
        #region Local Variables

        //private BOLib.DocListFactory objDocListFactory = null;
        string ContextMenuSetting = string.Empty;
        
        private int _DocCodeKey = 0;
        private bool formClose = false;

        //Event to be assigned by the caller detail form
        public GVar.ListEvent_OpenRecord ListEvent_OpenRecord = null;
        public GVar.ListEvent_DeleteRecord ListEvent_DeleteRecord = null;
        public GVar.ListEvent_CloseFORM ListEvent_CloseFORM = null;

        //Variables for using when the user find text
        private frmSearchInfo searchForm = null;
        private UltraGridColumn searchCol = null;
        private GlobalUI.SearchInfo searchInfo = null;
        
        string[] colHeaders = null;
        string[] colWidths = null;
        string[] colFormats = null;

        string ListSettingID = string.Empty;
        SYSListSetting objListSetting = new SYSListSetting();

        #endregion

        #region +++ Properties
        public int DocCodeKey
        {
            get
            {
                return this._DocCodeKey;
            }
        }
        #endregion

        // Initialize
        public frmDocList()
        {
            InitializeComponent();
        }//Completed
        public frmDocList(int docCodeKey, bool iscash = false)
        {
            try
            {
                InitializeComponent();
                this._DocCodeKey = docCodeKey;

                switch (docCodeKey)
                {
                    case (int)GEnum.SystemCode.Packing_List:                       
                        this.Text = "Packing List";
                        break;
                    case (int)GEnum.SystemCode.Quotation:                      
                        this.Text = "Quotation List";
                        break;
                    case (int)GEnum.SystemCode.Delivery_Order:
                        //added by thettm on 20 feb 2018(start)                      
                        if (iscash)
                            this.Text = "Cash Delivery Order List";
                        else
                            this.Text = "Delivery Order List";
                        break;
                    //added by thettm on 20 feb 2018(end)   
                    case (int)GEnum.SystemCode.Defect_Report:
                        this.Text = "Defect Report List";
                        break;
                    case (int)GEnum.SystemCode.Sales_Invoice:                     
                        this.Text = "Sales Invoice List";
                        break;
                    case (int)GEnum.SystemCode.Sales_Credit_Note:                       
                        this.Text = "Sales Credit Note List";
                        break;
                    case (int)GEnum.SystemCode.Sales_Debit_Note:                      
                        this.Text = "Sales Debit Note List";
                        break;
                    case (int)GEnum.SystemCode.Cash_Sale:                       
                        this.Text = "Cash Sale List";
                        break;
                    case (int)GEnum.SystemCode.Cash_Credit_Note:                        
                        this.Text = "Cash Credit Note List";
                        break; 
                    case (int)GEnum.SystemCode.Cash_Debit_Note:                      
                        this.Text = "Cash Debit Note List";
                        break;
                    case (int)GEnum.SystemCode.Sales_Order:
                        //added by thettm on 20 feb 2018(start)     
                        if (iscash)
                            this.Text = "Cash Sales Order List";
                        else
                            this.Text = "Sales Order List";
                        //added by thettm on 20 feb 2018(end)   
                        break;
                    case (int)GEnum.SystemCode.Reserve_Order:
                        this.Text = "Reserve Order List";
                        break;
                    case (int)GEnum.SystemCode.Purchase_Order:                       
                        this.Text = "Purchase Order List";
                        break;
                    case (int)GEnum.SystemCode.Purchase_Delivery:                       
                        this.Text = "Purchase Deliver List";
                        break;
                    case (int)GEnum.SystemCode.Purchase_Invoice:                      
                        this.Text = "Purchase Invoice List";
                        break;
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:                      
                        this.Text = "Purchase Credit Note List";
                        break;
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:                      
                        this.Text = "Purchase Debit Note List";
                        break;
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:                      
                        this.Text = "Sale Order Adjustment List";
                        break;
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:                      
                        this.Text = "Purchase Order Adjustment List";
                        break;
                    case (int)GEnum.SystemCode.Purchase_Adjustment:                       
                        this.Text = "Purchase Adjustment List";
                        break;
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:                        
                        this.Text = "Consignment Order Adjustment List";
                        break;

                    case (int)GEnum.SystemCode.Payment_Received:                        
                        this.Text = "Payment List";
                        break;
                    case (int)GEnum.SystemCode.Cash_Payment_Received:                        
                        this.Text = "Cash Payment List";
                        break;
                    case (int)GEnum.SystemCode.Payment_Issue:                        
                        this.Text = "Payment Issue List";
                        break;
                    case (int)GEnum.SystemCode.GL_Payment:
                        this.Text = "GL Payment List";
                        this._DocCodeKey = (int)GEnum.SystemCode.Payment_Issue;
                        break;
                    case (int)GEnum.SystemCode.Contra:                        
                        this.Text = "Contra List";
                        break;
                    case (int)GEnum.SystemCode.Cash_Contra:                        
                        this.Text = "Cash Contra List";
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:                        
                        this.Text = "Issue Consignment List";
                        break;
                    case (int)GEnum.SystemCode.Return_Consignment:                        
                        this.Text = "Return Consignment List";
                        break;
                    case (int)GEnum.SystemCode.Sales_Adjustment:                        
                        this.Text = "AR Adjustment List";
                        break;
                    case (int)GEnum.SystemCode.Cash_Adjustment:                        
                        this.Text = "Cash Adjustment List";
                        break;
                    case (int)GEnum.SystemCode.Inventory_Adjustment:                        
                        this.Text = "Inventory Adjustment List";
                        break;
                    case (int)GEnum.SystemCode.Inventory_Transfer:                        
                        this.Text = "Inventory Transfer List";
                        break;
                    case (int)GEnum.SystemCode.Order_Consignment:                        
                        this.Text = "Order Consignment List";
                        break;
                    case (int)GEnum.SystemCode.Consignment_Settlement:                        
                        this.Text = "Consignment Settlement List";
                        break;
                    case (int)GEnum.SystemCode.Received_Consignment:                        
                        this.Text = "Received Consignment List";
                        break;
                    case (int)GEnum.SystemCode.Deposit:                        
                        this.Text = "Deposit List";
                        break;
                    case (int)GEnum.SystemCode.Bank_Revaluation:                      
                        this.Text = "Bank Revaluation List";
                        break;
                    case (int)GEnum.SystemCode.Journal:                      
                        this.Text = "Journal List";
                        break;
                    case (int)GEnum.SystemCode.Inventory_Production:                       
                        this.Text = "Inventory Production List";
                        break;
                    case (int)GEnum.SystemCode.Purchase_Plan:                      
                        this.Text = "Purchase Plan List";
                        break;
                    case (int)GEnum.SystemCode.Purchase_Request:                       
                        this.Text = "Purchase Request List";
                        break;
                        break;
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                        this.Text = "Purchase Shipment List";
                        break;
                    default:
                        this.Text = "Document List";
                        break;
                }
            }
            catch (TAException tex)
            {
                formClose = true;
                Error(tex, true);
            }
            catch (Exception ex)
            {
                formClose = true;
                Error(ex, true);
            }
        }//Completed

        //Form Events
        private void frm_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {

                pDateFrom.DateValue = GlobalUI.ListDefaultFromDate;
                pDateTo.DateValue = GlobalUI.ListDefaultToDate;

                GlobalUI.FormGrids_Set(this, _DocCodeKey, out ContextMenuSetting);
                (tagrdDocList.DataSource as DataTable).DefaultView.Sort = "DocDate Desc ,DocID desc";
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(DocCodeKey, this.Name);

                GlobalUI.Combos_Fill(this, DocCodeKey);
                pDocType.SetValueTrigger(0,false);

                this.tagrdDocList.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                
                if (tagrdDocList.DisplayLayout.Bands[0].Columns.Exists("Attachment"))
                {
                    tagrdDocList.DisplayLayout.Bands[0].Columns["Attachment"].Header.Caption = "";
                    tagrdDocList.DisplayLayout.Bands[0].Columns["Attachment"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    tagrdDocList.DisplayLayout.Bands[0].Columns["Attachment"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    tagrdDocList.DisplayLayout.Bands[0].Columns["Attachment"].CellButtonAppearance.Image = global::WinUI.Properties.Resources.attachment;
                    tagrdDocList.DisplayLayout.Bands[0].Columns["Attachment"].CellButtonAppearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));

                    foreach (UltraGridColumn col in tagrdDocList.DisplayLayout.Bands[0].Columns)
                    {
                        col.CellActivation = (col.Key == "Attachment") ? Activation.AllowEdit : Activation.ActivateOnly;
                    }
                }
                else
                    this.tagrdDocList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;

                this.tagrdDocList.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                tagrdDocList.DisplayLayout.Bands[0].Override.ActiveCellAppearance.BackColor = Color.Gold;

                tagrdDocList.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True;

                switch (DocCodeKey)
                {
                    //added by thettm on 20 feb 2018(start)
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                        FilterData();
                        break;
                    //added by thettm on 20 feb 2018(end)

                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                       
                        if (SECPermUtility.Perform("ItemViewCost", false) == false)
                        {
                            string[] Cols = new string[] { "DocSubTotal", "DocGrand", "DocHome", "DocOverallDisAmt", "DocTotalAfterDis", "DocTaxTotal" };

                            foreach (string col in Cols)
                            {
                                if (!tagrdDocList.DisplayLayout.Bands[0].Columns.Exists(col))
                                    continue;
                               
                                if(tagrdDocList.DisplayLayout.Bands[0].Columns[col].EditorComponent==null)
                                    tagrdDocList.DisplayLayout.Bands[0].Columns[col].EditorComponent = new TAUtil.TANumericEditor();                                
                                ((TAUtil.TANumericEditor)tagrdDocList.DisplayLayout.Bands[0].Columns[col].EditorComponent).PasswordChar ='*';

                                tagrdDocList.DisplayLayout.Bands[0].Columns[col].CellDisplayStyle = CellDisplayStyle.FullEditorDisplay;
                                tagrdDocList.DisplayLayout.Bands[0].Columns[col].CellActivation = Activation.ActivateOnly;
                                
                                tagrdDocList.DisplayLayout.Bands[0].Columns[col].ResetCellAppearance();
                            }
                        }
                        break;
                     case (int)GEnum.SystemCode.Payment_Issue:
                        if (this.Text.Equals("GL Payment List"))
                        {                            
                            pDocType.Value = "GL Payment";
                            if (pDocType.DataSource != null)
                            {
                                DataTable dt = pDocType.DataSource as DataTable;
                                dt.DefaultView.RowFilter = "DocTypeNm='GL Payment'";
                                pDocType.DataSource = dt;
                                pDocType.ReadOnly = true;
                            }
                        }
                        

                        break;
                }                
               
            }
            catch (TAException tex)
            {
                formClose = true;
                Error(tex, true);
            }
            catch (Exception ex)
            {
                formClose = true;
                Error(ex, true);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
         //added by thettm on 20 feb 2018(start)
        private void FilterData()
        {
            if (this.Text.ToString() == "Sales Order List")
            {
                ((DataTable)pDocType.DataSource).DefaultView.RowFilter = "[DocTypeNm]<>'Cash Sales Order'";
                pDocType.DataSource = ((DataTable)pDocType.DataSource).DefaultView.ToTable();
                GlobalUI.AddComboEmptyValue(pDocType, true);

                ((DataTable)tagrdDocList.DataSource).DefaultView.RowFilter = "[DocTypeNm]<>'Cash Sales Order'";
                tagrdDocList.DataSource = ((DataTable)tagrdDocList.DataSource).DefaultView.ToTable();

            }
            else if (this.Text.ToString() == "Cash Sales Order List")
            {
                ((DataTable)pDocType.DataSource).DefaultView.RowFilter = "[DocTypeNm]='Cash Sales Order'";
                pDocType.DataSource = ((DataTable)pDocType.DataSource).DefaultView.ToTable();
                GlobalUI.AddComboEmptyValue(pDocType, true);

                ((DataTable)tagrdDocList.DataSource).DefaultView.RowFilter = "[DocTypeNm]='Cash Sales Order'";
                tagrdDocList.DataSource = ((DataTable)tagrdDocList.DataSource).DefaultView.ToTable();

            }
            else if (this.Text.ToString() == "Delivery Order List")
            {
                ((DataTable)pDocType.DataSource).DefaultView.RowFilter = "[DocTypeNm]<>'Cash Delivery Order'";
                pDocType.DataSource = ((DataTable)pDocType.DataSource).DefaultView.ToTable();
                GlobalUI.AddComboEmptyValue(pDocType, true);

                ((DataTable)tagrdDocList.DataSource).DefaultView.RowFilter = "[DocTypeNm]<>'Cash Delivery Order'";
                tagrdDocList.DataSource = ((DataTable)tagrdDocList.DataSource).DefaultView.ToTable();

            }
            else if (this.Text.Equals("GL Payment List"))
            {
                //((DataTable)pDocType.DataSource).DefaultView.RowFilter = "[DocTypeNm]='GL Payment'";
                //pDocType.DataSource = ((DataTable)pDocType.DataSource).DefaultView.ToTable();
                //GlobalUI.AddComboEmptyValue(pDocType, true);

                ((DataTable)tagrdDocList.DataSource).DefaultView.RowFilter = "[DocTypeNm]='GL Payment'";
                tagrdDocList.DataSource = ((DataTable)tagrdDocList.DataSource).DefaultView.ToTable();

            }
            else
            {
                ((DataTable)pDocType.DataSource).DefaultView.RowFilter = "[DocTypeNm]='Cash Delivery Order'";
                pDocType.DataSource = ((DataTable)pDocType.DataSource).DefaultView.ToTable();
                GlobalUI.AddComboEmptyValue(pDocType, true);

                ((DataTable)tagrdDocList.DataSource).DefaultView.RowFilter = "[DocTypeNm]='Cash Delivery Order'";
                tagrdDocList.DataSource = ((DataTable)tagrdDocList.DataSource).DefaultView.ToTable();

            }
        }
        //added by thettm on 20 feb 2018(end)
        private void frm_Shown(object sender, EventArgs e)
        {
            try
            {
                if (formClose)
                {
                    this.Close();
                    this.Dispose();
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
        private void frm_FormClosing(object sender, FormClosingEventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (this.ListEvent_CloseFORM != null)
                    this.ListEvent_CloseFORM.Invoke();

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
        private void frm_Activated(object sender, EventArgs e)
        {
            try
            {
                if (!GFunc.IsNE(tagrdDocList.Tag))
                {
                    int key = GFunc.NEInt(tagrdDocList.Tag, 0);
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in tagrdDocList.Rows)
                    {
                        if (GFunc.NEInt(row.Cells["DocKey"].Value, 0) == key)
                        {
                            tagrdDocList.ActiveRow = row;
                            tagrdDocList.ActiveRow.Selected = true;
                            break;
                        }
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
        }
        private void frmDocList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
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

        //Form Display
        private void GridFilter_Set()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (GFunc.IsNEZ(pDocType.Value))
                {
                    ((DataTable)tagrdDocList.DataSource).DefaultView.RowFilter = "";
                }
                else
                {
                    //((DataTable)tagrdDocList.DataSource).DefaultView.RowFilter = "";             
                    ((DataTable)tagrdDocList.DataSource).DefaultView.RowFilter ="DocTypeNm='"+pDocType.Text+"'";
                   
                }

                if (tagrdDocList.Rows.FirstVisibleCardRow != null)
                {
                    tsbPrint.Enabled = true;
                    tsbDelete.Enabled = true;
                }
                else
                {
                    tsbPrint.Enabled = false;
                    tsbDelete.Enabled = false;
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
        private void Grid_Rebind()
        {
            int SelectedDocKey = 0;

            try
            {
                string prevFilter = "";

                if (tagrdDocList.DataSource != null)
                    prevFilter= (tagrdDocList.DataSource as DataTable).DefaultView.RowFilter;

                if (tagrdDocList.ActiveRow != null)
                    SelectedDocKey = GFunc.NEInt(tagrdDocList.ActiveRow.Cells["DocKey"].Value, 0);

                string listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "0GF", tagrdDocList.Name);
                GlobalUI.Grid_Format(tagrdDocList, listID, true, false);
                (tagrdDocList.DataSource as DataTable).DefaultView.Sort = "DocDate Desc ,DocID desc";
                (tagrdDocList.DataSource as DataTable).DefaultView.RowFilter = prevFilter;

                if (SelectedDocKey > 0)
                {
                    UltraGridRow gRow = this.tagrdDocList.Rows.OfType<UltraGridRow>().ToList().Find(
                    row => row.Cells["DocKey"].Text.Equals(SelectedDocKey.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    if (gRow != null)
                    {
                        tagrdDocList.ActiveRow = gRow;
                    }
                }
                //added by thettm on 20 feb 2018
                if ((DocCodeKey == (int)GEnum.SystemCode.Sales_Order 
                    || DocCodeKey == (int)GEnum.SystemCode.Delivery_Order || this.Text.Equals("GL Payment List"))
                    && prevFilter == "" ) FilterData();
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
        
        //Menu Strip Event
        private void tsbExport_Click(object sender, EventArgs e)
        {
            try
            {
                int count = tsbExport.Tag == null ? 1 : (int)tsbExport.Tag;
                GlobalUI.Export(tagrdDocList, ref count);
                tsbExport.Tag = count;
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
        private void tsbClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.formClose = true;
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
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DocList_DeleteRecord();
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
        private void tsbEdit_Click(object sender, EventArgs e)
        {
            try
            {
                DocList_OpenRecord();
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
        private void tsbItemList_Click(object sender, EventArgs e)
        {
            bool alreadyExist = false;
            try
            {
                foreach (Form form in this.ParentForm.OwnedForms)
                {
                    if (form.Name == GlobalUI.Form_Name.FRM_DOCLISTDET)
                    {
                        alreadyExist = true;
                    }
                }
                if (!alreadyExist)
                {
                    int docKey = tagrdDocList.ActiveRow == null ? 0 : GFunc.NEInt(tagrdDocList.ActiveRow.Cells["DocKey"].Value, 0);
                    GlobalUI.PopupDisplay(GlobalUI.Form_Name.FRM_DOCLISTDET, (GEnum.SystemCode)DocCodeKey, docKey);
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
        private void tsbPrint_Click(object sender, EventArgs e)
        {
            try
            {
                frmPrintSelectionList l_PrintListForm = new frmPrintSelectionList((GEnum.SystemCode)DocCodeKey);
                l_PrintListForm.ListRefresh += new GVar.ListEvent_RefreshRecord(this.Grid_Rebind);
                l_PrintListForm.Show();
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
        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                this.errorProvider1.SetError(pDateFrom, string.Empty);
                this.errorProvider1.SetError(pDateTo, string.Empty);

                Grid_Rebind();
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

        //Event invoke by or invoke to the document list FORM
        public void OnDoc_Close()
        {
            try
            {
                this.formClose = true;
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
        public void OnDoc_Changed()
        {
            try
            {
                Grid_Rebind();
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
        private void DocList_OpenRecord()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int key = 0;
                if (GFunc.IsNE(tagrdDocList.ActiveRow)==false)
                {
                    
                    key = GFunc.NEInt(tagrdDocList.ActiveRow.Cells["DocKey"].Value, 0);
                    if (this.ListEvent_OpenRecord != null)
                    {
                        this.ListEvent_OpenRecord.Invoke(key);                      
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
        }//Completed
        private void DocList_DeleteRecord()
        {
            this.Cursor = Cursors.WaitCursor;
            int PreRowIndex = 0;

            try
            {
                int key = 0;
                if (!GFunc.IsNE(tagrdDocList.ActiveRow))
                {
                    if (tagrdDocList.ActiveRow.Index > 0)
                        PreRowIndex = tagrdDocList.ActiveRow.Index - 1;
                    key = GFunc.NEInt(tagrdDocList.ActiveRow.Cells["DocKey"].Value, 0);
                    if (this.ListEvent_DeleteRecord != null)
                        this.ListEvent_DeleteRecord.Invoke(key);
                }
                Grid_Rebind();
                if (tagrdDocList.Rows.Count > 0)
                {
                    tagrdDocList.Rows[PreRowIndex].Selected = true;
                    tagrdDocList.Rows[PreRowIndex].Activate();
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
        
        //Control Event
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
        private void pDocType_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                GridFilter_Set();
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
        private void pDocType_ItemNotInList(object sender, ValidationErrorEventArgs e)
        {
            try
            {
                GlobalUI.ItemNotInList(pDocType, e, false, null);
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
        private void pDate_OnDataError(object sender, TAUtil.TAErrorEventArgs e)
        {
            try
            {
                this.errorProvider1.SetError(sender as TAUtil.TADateEditor, "Invalid Data Type");
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
        private void pDate_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                GlobalUI.ListDefaultFromDate = pDateFrom.DateValue.Value;
                GlobalUI.ListDefaultToDate = pDateTo.DateValue.Value;
                this.errorProvider1.SetError(sender as TAUtil.TADateEditor, string.Empty);
                Grid_Rebind();
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

        //Grid Event
        private void tagrdDocList_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            try
            {
                if (tagrdDocList.ActiveRow != null)
                    DocList_OpenRecord();
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
        private void tagrdDocList_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                int docKey = tagrdDocList.ActiveRow == null ? 0 : GFunc.NEInt(tagrdDocList.ActiveRow.Cells["DocKey"].Value, 0);
                GlobalUI.PopupRefresh((GEnum.SystemCode)DocCodeKey, docKey);
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

        private void tagrdDocList_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key.Equals("Attachment"))              
                {
                    SYSAttachments objs = new SYSAttachments();
                    int DC, DK;
                    DC = GFunc.NEInt(e.Cell.Row.Cells["DocCodeKey"].Value, 0);
                    DK = GFunc.NEInt(e.Cell.Row.Cells["DocKey"].Value, 0);
                    objs.Fetch(new SYSAttachments.Criteria(DC, DK, 1));
                    int beforecount = objs.Count;

                    frmAttachment f = new frmAttachment(objs, DC, DK, GFunc.NEStr(e.Cell.Row.Cells["DocKey"].Value, ""));
                    f.ShowDialog(this);
                    if (objs.Count != beforecount)
                    {
                        e.Cell.Row.Cells["Attachment"].Value = "(" + objs.Count + ")";
                        tagrdDocList.Update();
                    }
                }
                else if(e.Cell.Column.Key.Equals("Links"))
                {
                    if (GFunc.NEInt(e.Cell.Row.Cells["DocState"].Value,"") == (int?)GEnum.DocState.Posted)
                    {
                        GlobalUI.PopupDisplay("frmDocRelationship", (GEnum.SystemCode)GFunc.NEInt(e.Cell.Row.Cells["DocCodeKey"].Value, "")
                            , GFunc.NEInt(e.Cell.Row.Cells["DocKey"].Value, ""), GFunc.NEStr(e.Cell.Row.Cells["DocID"].Value, ""));
                    }
                    else
                    {
                        MsgBox.Show("Relationship will be show only for the posted documents.");
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
        }
    }
}


