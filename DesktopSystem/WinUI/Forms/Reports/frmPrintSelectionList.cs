using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOLib;
using Infragistics.Win.UltraWinEditors;
using TAUtil;

namespace WinUI
{
    public partial class frmPrintSelectionList : Form
    {
        #region Local Variables

        private GEnum.SystemCode _DocCodeKey = GEnum.SystemCode.Sales_Order;
        public GVar.ListEvent_RefreshRecord ListRefresh = null;
        private ReportLoader _ReportLoader = null;             
        private string ContextMenuSetting = string.Empty;
        private string _SelectedRptTitle = string.Empty;

        #endregion

        public frmPrintSelectionList()
        {
            InitializeComponent();
        }        
        public frmPrintSelectionList(GEnum.SystemCode DocCodeKey)
        {
            InitializeComponent();
            _DocCodeKey = DocCodeKey;
        }

        //Search PO Types, 10=DO, 20=SO, 30=IV, 40=QO        
        public frmPrintSelectionList(GEnum.SystemCode DocCodeKey,int searchPOType, string searchPONum)
        {
            InitializeComponent();
            _DocCodeKey = DocCodeKey;
            POSearchType.SetValueTrigger(searchPOType,false);
            POSearchDocID.SetValueTrigger(searchPONum,false);
        }

        private void frmPrintSelectionList_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                FromDate.DateValue = GlobalUI.ListDefaultFromDate;
                ToDate.DateValue = GlobalUI.ListDefaultToDate;

                GlobalUI.FormGrids_Set(this, (int)this._DocCodeKey, out ContextMenuSetting);

                _ReportLoader = new ReportLoader();
                
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)this._DocCodeKey);
                GlobalUI.Combos_Fill(this, (int)this._DocCodeKey);

                Print.Value=1;//Default set to Selected Document

                //Set Default RptFileName from SYS_Rep
                DataTable dt = GFunc.ExecuteQuery("Exec SYSRepRptDefault_Set " + (int)_DocCodeKey);
                if (dt != null && dt.Rows.Count > 0)
                {
                    RptFile1.SetValueTrigger(dt.Rows[0]["UID"], true);
                }              

                switch (_DocCodeKey)
                {
                    case GEnum.SystemCode.Purchase_Order:
                        POSearchType.Visible = true;
                        POSearchDocID.Visible = true;
                        tsbRequery.Margin = new Padding(680,tsbRequery.Margin.Top,tsbRequery.Margin.Right,tsbRequery.Margin.Bottom);
                        if (GFunc.IsNEZ(POSearchType.Value))
                            POSearchType.SetValueTrigger(10, false);//Default to search by DO Num
                        else if (POSearchDocID.Text.Length > 0)
                        {                            
                            Print.Value = 0;//To print all
                        }
                        break;
                    case GEnum.SystemCode.Sales_Invoice: /* added by YST on 2021/11/16 */
                        if (SysOptionUtility.DatabaseBranchCode == "BHM")
                        {
                            DateRangeultraLabel.Text = "Date Range < Last Modified Date >";
                            //ModifyUser.SelectedRow = ModifyUser.Rows[0];
                            ModifyUser.Text = AppInfor.CurrentUserID;
                            ModifyUser.Visible = true;
                            DocPrinted.Visible = true;
                            DocPrinted.Checked = true;                            
                        }
                        else
                        {
                            tsbRequery.Margin = new Padding(520, tsbRequery.Margin.Top, tsbRequery.Margin.Right, tsbRequery.Margin.Bottom);
                        }
                        break;
                    default:                       
                        POSearchType.Visible = false;
                        POSearchDocID.Visible = false;
                        ModifyUser.Visible = false;
                        DocPrinted.Visible = false;
                        tsbRequery.Margin = new Padding(520, tsbRequery.Margin.Top, tsbRequery.Margin.Right, tsbRequery.Margin.Bottom);                        
                        break;
                }

                AdditionalFilter();
                RptFile1.DropDownWidth = 550;
                RptFile2.DropDownWidth = 550;
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }
        private void frmPrintSelectionList_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
               
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }
        private void frmPrintSelectionList_KeyDown(object sender, KeyEventArgs e)
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

        }

        private void RequerytoolStripButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                GlobalUI.FormGrids_Set(this, (int)this._DocCodeKey, out ContextMenuSetting);
                AdditionalFilter();               
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }
        private void POSearchDocID_CustomUpdate(object sender, CancelEventArgs e)
        {
            AdditionalFilter();
        }
        
        private void AdditionalFilter()
        {
            switch (_DocCodeKey)
            {
                case GEnum.SystemCode.Purchase_Order:
                    ((DataTable)tagrdPrintSelectionList.DataSource).DefaultView.RowFilter = "";
                    if (POSearchDocID.Text.Length > 0)
                    {
                        string searchField = "";                       

                        switch (GFunc.NEInt(POSearchType.Value, 0))
                        {
                            case 10://DO Num
                                searchField = "DocDONum";
                                break;
                            case 20://SO Num
                                searchField = "DocSONum";
                                break;
                            case 30://IV Num
                                searchField = "DocIVNum";
                                break;
                            case 40:
                                searchField = "DocQONum";
                                break;
                            case 50:
                                searchField = "DocPDNum";
                                break;
                            case 60:
                                searchField = "DocBLNum";
                                break;
                        }
                     
                        ((DataTable)tagrdPrintSelectionList.DataSource).DefaultView.RowFilter = searchField + "='" + POSearchDocID.Text + "'";
                    }
                    break;
                case GEnum.SystemCode.Sales_Invoice: /* added by YST on 2021/11/16 */
                    if (SysOptionUtility.DatabaseBranchCode == "BHM")
                    {
                        string strFilter = "LastModifiedUserID <> '' ";
                        if(!ModifyUser.Text.Contains("Select") && !string.IsNullOrEmpty(ModifyUser.Text))
                        {
                            strFilter = strFilter + "And LastModifiedUserID='" + ModifyUser.Text + "'";
                        }
                        if (DocPrinted.Checked)
                        {
                            strFilter = strFilter + "And DocPrinted=" + !DocPrinted.Checked;
                        }

                        ((DataTable)tagrdPrintSelectionList.DataSource).DefaultView.RowFilter = strFilter;
                    }
                    break;
            }
            ((DataTable)tagrdPrintSelectionList.DataSource).DefaultView.Sort = "DocDate desc,DocID desc";
        }

        private void tsbPreview_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (PrintPreview(GEnum.PrintType.Preview))
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(ex.Data.Count, ex.StackTrace);
                MsgBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;

            }
        }
        private void tsbPrint_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                PrintPreview(GEnum.PrintType.Print);
                if (this.ListRefresh != null)
                    this.ListRefresh.Invoke();
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }
        private void tsbEmail_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                PrintPreview(GEnum.PrintType.Email);
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Control Events
        private void RptFile1_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (!GFunc.IsNEZ((RptFile1.Value)))
                {
                    SYSRepRpt objRpt = SYSRepRpt.Get(Convert.ToInt32(RptFile1.Value), 2);//option 2 for UID

                    //Show Rpt Information
                    _SelectedRptTitle=objRpt.RptTitle;
                    Layout.SetValueTrigger(objRpt.RptLayOut, false);
                    Copies.SetValueTrigger(objRpt.PrtCopies.ToString(), false);

                    LayoutSetting();

                    LetterHead.Checked = objRpt.ShwLetterHead;
                    ItemCount.Checked = objRpt.ShwItmCount;
                }
            }
            catch (Exception ex)
            {
                Error(ex, false);
            }
        }

        private void Layout_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                LayoutSetting();
            }
            catch (Exception ex)
            {
                Error(ex, false);
            }
        }

        private void LayoutSetting()
        {
            try
            {
                int CheckSwitch = GFunc.NEInt(Layout.Value, -1);
                switch (CheckSwitch)
                {
                    case 10: //Option 'All'
                        ColumnText.Checked = true;
                        Marking.Checked = true;
                        ID.Checked = true;
                        Description.Checked = true;
                        Qty.Checked = true;
                        Price.Checked = true;
                        Amount.Checked = true;
                        Total.Checked = true;
                        //ItemCount.Checked = true;
                        //LetterHead.Checked = true;
                        HideDisTotalChg.Checked = false;
                        break;

                    case 20: //Option 'Description'
                        ColumnText.Checked = true;
                        Marking.Checked = true;
                        ID.Checked = true;
                        Description.Checked = true;
                        Qty.Checked = true;
                        ItemCount.Checked = true;
                        HideDisTotalChg.Checked = true;
                        LetterHead.Checked = true;
                        Price.Checked = false;
                        Amount.Checked = false;
                        Total.Checked = false;
                        break;

                    case 30: //Option 'Price'
                        ColumnText.Checked = true;
                        Marking.Checked = true;
                        ID.Checked = true;
                        Description.Checked = true;
                        Qty.Checked = true;
                        ItemCount.Checked = true;
                        HideDisTotalChg.Checked = true;
                        LetterHead.Checked = true;
                        Price.Checked = true;
                        Amount.Checked = false;
                        Total.Checked = false;
                        break;

                    case 40: //Option 'Amount'
                        ColumnText.Checked = true;
                        Marking.Checked = true;
                        ID.Checked = true;
                        Description.Checked = true;
                        Qty.Checked = true;
                        ItemCount.Checked = true;
                        HideDisTotalChg.Checked = false;
                        LetterHead.Checked = true;
                        Price.Checked = true;
                        Amount.Checked = true;
                        Total.Checked = false;
                        break;

                    default:
                        CheckSwitch = -1;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool PrintPreview(GEnum.PrintType pType)
        {
            try
            {
                bool printed = false;

                if (tagrdPrintSelectionList.Rows.Count == 0)
                    return false;
                Dictionary<string, bool> _CheckControlVale = new Dictionary<string,bool>();
                _CheckControlVale.Add(ColumnText.Name, ColumnText.Checked);
                _CheckControlVale.Add(Marking.Name, Marking.Checked);
                _CheckControlVale.Add(ID.Name, ID.Checked);
                _CheckControlVale.Add(Description.Name, Description.Checked);
                _CheckControlVale.Add(Qty.Name, Qty.Checked);
                _CheckControlVale.Add(Price.Name, Price.Checked);
                _CheckControlVale.Add(Amount.Name, Amount.Checked);
                _CheckControlVale.Add(Total.Name, Total.Checked);
                _CheckControlVale.Add(ItemCount.Name, ItemCount.Checked);
                _CheckControlVale.Add(HideDisTotalChg.Name, HideDisTotalChg.Checked);
                _CheckControlVale.Add(LetterHead.Name, LetterHead.Checked);                
                frmPrintSelection frm_PrintSelection = new frmPrintSelection();                
                
                frm_PrintSelection.ReportLoader = _ReportLoader;
                frm_PrintSelection.RepKey = (int)_DocCodeKey;                
                frm_PrintSelection.CheckValue = _CheckControlVale;

                short NoCopies = GFunc.NEShort(Copies.Text, 1);                             

                DataTable dtPrintedDoc = new DataTable("DocPrint");              
                dtPrintedDoc.Columns.Add("DocKey", typeof(int));

                
                if (pType == GEnum.PrintType.Email)
                {
                    List<string> docArray = new List<string>();
                    List<string> emails = new List<string>();

                    SetFilteredDocAndEmail(docArray, emails);
                  
                    for (int i = 0; i < docArray.Count; i++)
                    {
                        frm_PrintSelection.DocKey = docArray[i];
                        frm_PrintSelection.DocEmail = emails[i];

                        if (RptFile1.SelectedRow !=null)
                        {
                            printed = frm_PrintSelection.Print(NoCopies, RptFile1.SelectedRow.Cells["RptNm"].Value.ToString(), pType,
                                RptFile1.SelectedRow.Cells["RptPrintCondition"].Value.ToString(), RptFile1.SelectedRow.Cells["Custom1"].Value.ToString());
                        }
                        if (RptFile2.SelectedRow!=null)
                        {
                            printed = frm_PrintSelection.Print(NoCopies, RptFile2.SelectedRow.Cells["RptNm"].Value.ToString(), pType, 
                                RptFile2.SelectedRow.Cells["RptPrintCondition"].Value.ToString()
                                , RptFile2.SelectedRow.Cells["Custom1"].Value.ToString());
                        }
                        if (printed)
                        {
                            DataRow row = dtPrintedDoc.NewRow();                           
                            row["DocKey"] = Convert.ToInt32(docArray[i]);
                            dtPrintedDoc.Rows.Add(row);
                        }
                    }
                }
                else //preview or print
                {
                    SetFilteredDocs(ref dtPrintedDoc);

                    if (RptFile1.SelectedRow != null)
                    {
                        printed = frm_PrintSelection.PrintMulti(NoCopies, RptFile1.SelectedRow.Cells["RptNm"].Value.ToString(), pType,
                            RptFile1.SelectedRow.Cells["RptPrintCondition"].Value.ToString(), GFunc.ConvertDataTableToXML(dtPrintedDoc), RptFile1.SelectedRow.Cells["RptPrintCondition"].Value.ToString());
                    }
                    if (RptFile2.SelectedRow != null)
                    {
                        printed = frm_PrintSelection.PrintMulti(NoCopies, RptFile2.SelectedRow.Cells["RptNm"].Value.ToString(), pType,
                            RptFile2.SelectedRow.Cells["RptPrintCondition"].Value.ToString(), GFunc.ConvertDataTableToXML(dtPrintedDoc), RptFile2.SelectedRow.Cells["RptPrintCondition"].Value.ToString());
                    }                    
                }

                if (pType == GEnum.PrintType.Print)
                {
                    //Change Svr Print Status; 
                    printed = _ReportLoader.DocPrintStatus_Set((int)_DocCodeKey, GFunc.ConvertDataTableToXML(dtPrintedDoc));
                }
                //---------------------------------
                return printed;
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }
        private void SetFilteredDocAndEmail(List<string> docKeys, List<string> emails)
        {          
            try
            {
                if ((int)Print.Value == 0)
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow item in tagrdPrintSelectionList.Rows)
                    {
                        docKeys.Add(item.Cells["DocKey"].Value.ToString());
                        emails.Add(item.Cells["DocBAddrEmail"].Value.ToString());
                    }                
                }
                else
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow item in tagrdPrintSelectionList.Selected.Rows)
                    {
                        docKeys.Add(item.Cells["DocKey"].Value.ToString());
                        emails.Add(item.Cells["DocBAddrEmail"].Value.ToString());
                    }                
                }              
               
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }

        private void SetFilteredDocs(ref DataTable dt)
        {
            try
            {
                if ((int)Print.Value == 0)
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow item in tagrdPrintSelectionList.Rows)
                    {
                        DataRow dr = dt.NewRow();
                        dr["DocKey"] = item.Cells["DocKey"].Value;
                        dt.Rows.Add(dr);
                    }         
                }
                else
                {
                    if (tagrdPrintSelectionList.Selected.Rows.Count == 0 && tagrdPrintSelectionList.Rows.Count > 0)
                        tagrdPrintSelectionList.Rows[0].Selected = true;
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow item in tagrdPrintSelectionList.Selected.Rows)
                    {
                        DataRow dr = dt.NewRow();
                        dr["DocKey"] = item.Cells["DocKey"].Value;
                        dt.Rows.Add(dr);
                    }       
                }              
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
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
                Error(tex, false);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }
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
        }

        #region Set Error Methods
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
                    l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, GFunc.GridColumnKey_Get(this.ActiveControl) });
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
                    l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, GFunc.GridColumnKey_Get(this.ActiveControl) });
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
        #endregion       
    }

    public class PrintSelectionListTable : DataTable
    {
        internal const string _DocKey = "DocKey";
        internal const string _DocConKey = "DocConKey";
        internal const string _DocNum = "DocNum";
        internal const string _DocDate = "Date";
        internal const string _Type = "Type";
        internal const string _CustomerVendor = "CustomerVendor";
        internal const string _Grand = "Grand";
        internal const string _Printed = "Printed";

        public PrintSelectionListTable()
        {
            this.Columns.Add(_DocKey, typeof(int));
            this.Columns.Add(_DocConKey, typeof(int));
            this.Columns.Add(_DocNum, typeof(string));
            this.Columns.Add(_DocDate, typeof(DateTime));
            this.Columns.Add(_Type, typeof(string));
            this.Columns.Add(_CustomerVendor, typeof(string));
            this.Columns.Add(_Grand, typeof(decimal));
            this.Columns.Add(_Printed, typeof(bool));
        }

        public static DataTable CreateNew()
        {
            PrintSelectionListTable l_reval = new PrintSelectionListTable();
            return l_reval;
        }

        public static DataTable ImageTable(DataTable SourceTable)
        {
            PrintSelectionListTable l_reval = new PrintSelectionListTable();
            
            foreach (DataRow item in SourceTable.Rows)
            {
                DataRow l_Row = l_reval.NewRow();
                foreach (DataColumn Column in SourceTable.Columns)
	            {
                    switch (Column.ColumnName.ToLower())
                    {
                        case "dockey":
                            l_Row[_DocKey] = item[Column.ColumnName];
                            break;
                        case "docid":
                           l_Row[_DocNum] = item[Column.ColumnName];
                            break;
                        case "DocDate":
                            l_Row[_DocDate] = item[Column.ColumnName];
                            break;
                        case "doctypenm":
                            l_Row[_Type] = item[Column.ColumnName];
                            break;
                        case "docconnm":
                            l_Row[_Type] = item[Column.ColumnName];
                            break;
                        case "docconkey":
                            l_Row[_DocConKey] = item[Column.ColumnName];
                            break;
                        case "docgrand":
                            l_Row[_Grand] = item[Column.ColumnName];
                            break;
                        case "docprinted":
                            l_Row[_Printed] = item[Column.ColumnName];
                            break;
                    }
	            }

                l_reval.Rows.Add(l_Row);
            }

            return l_reval;
        }
    }
}
