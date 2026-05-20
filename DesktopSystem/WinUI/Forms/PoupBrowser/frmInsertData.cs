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
using System.Collections;
using Infragistics.Win.UltraWinEditors;
using TAUtil;

namespace WinUI
{
    public partial class frmInsertData : Form
    {

        #region Local Variables

        private string ContextMenuSetting = string.Empty;       //To store Grid Setting. because grid format is overwritten whenever the DocCode is changed
        private string formContextMenuSetting = string.Empty;   //To store the rest of the control settings

        private Document objDoc = null;
        private int docCodekey;
        private UltraGrid tagrdDetItms = null;
        private bool displayUseRemarkAsDes = false;


        #endregion

        //Initialize
        public frmInsertData()
        {
            InitializeComponent();
        }//Completed
        public frmInsertData(Document pObjDoc, TAUtil.TAGridEditor pGrid)
        {
            InitializeComponent();
            tagrdDetItms = pGrid;
            objDoc = pObjDoc;
            docCodekey = (int)pObjDoc.DocCodeKey;
            displayUseRemarkAsDes = pObjDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Order ? true : false;            
                
        }//Completed

        //Form Events
        private void frmInsertData_Load(object sender, EventArgs e)
        {
            try
            {
                StartDate.DateValue = DateTime.Today.AddMonths(-1);
                EndDate.DateValue = DateTime.Today;

                //Set ContextMenu & Grid Setting  
                GlobalUI.FormGrids_Set(this, docCodekey, out ContextMenuSetting);
                formContextMenuSetting = GlobalUI.ContextMenuSetting_GetNew(docCodekey);
                ContextMenuSetting += formContextMenuSetting;

                //GlobalUI.cmnuGlobal_Set(this);
                GlobalUI.Combos_Fill(this, (int)objDoc.DocCodeKey);

                ComboDocCode_Fill();
                FormLayout(false);

                this.DocCode.SetValueTrigger(docCodekey, true);
                tagrdDocItmDetail.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                tagrdDocItmDetail.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
                tagrdDocItmDetail.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;

                //ensure that no row is active and selected when we refresh the grid bcos a Active row may not mean that a row is selected
                tagrdDocItmDetail.ActiveRow = null;

                //Set Default Value                
                UseSystemPrice.Checked = SysOptionUtility.GetBool("InsertDataUseSysPrice");
                includeNSLink.Checked = SysOptionUtility.GetBool("InsertDataWithNSLink");                
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
        private void frmInsertData_KeyDown(object sender, KeyEventArgs e)
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
        private void frmInsertData_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hashtable docDet = new Hashtable();
            docDet.Add(GEnum.Details.Doc_Itm, tagrdDetItms);
            DocComUtility.CalForm(objDoc, docDet, true, false);

        }
        //Button Events
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }//Completed
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (UltraGridRow row in tagrdDocItmDetail.Rows.GetFilteredInNonGroupByRows())
                {
                    row.Selected = true;
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
                foreach (UltraGridRow row in tagrdDocItmDetail.Rows)
                {
                    row.Selected = false;
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
        private void btnAppend_Click(object sender, EventArgs e)
        {
            try
            {
                //this.tagrdDocItmDetail.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdDocItmDetail.UpdateData();

                foreach (UltraGridRow row in tagrdDocItmDetail.Rows)
                {                    

                    if (row.Selected)          
                        row.Cells["Selected"].Value = true;            
                    else
                        row.Cells["Selected"].Value = false;
                 
                }
                tagrdDocItmDetail.UpdateData();

                DataTable dt = tagrdDocItmDetail.DataSource as DataTable;
                //dt.DefaultView.RowFilter = "Selected = 1";

                if (AppendData(dt.AsEnumerable().Where(r => r.Field<bool>("Selected") == true).CopyToDataTable()))
                    this.DialogResult = DialogResult.OK;
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
        private void btnAppendAll_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnSelectAll_Click(null, null);
                this.btnAppend_Click(null, null);
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

        //Control Events
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
        private void StartDate_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                ComboDocID_Fill();
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
        private void EndDate_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                ComboDocID_Fill();
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
        private void DocCode_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                switch (GFunc.NEInt(DocCode.Value,0) )
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                        if (displayUseRemarkAsDes)
                        {
                            UseAdditionalRemAsDes.Visible = true;
                            UseAdditionalRemAsDes.Checked = SysOptionUtility.GetBool("UseAdditionalRemAsDesInPO");
                        }
                        
                        break;
                    default:
                        UseAdditionalRemAsDes.Visible = false;
                        UseAdditionalRemAsDes.Checked = false;
                        break;
                }
                    
                ComboDocID_Fill();
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
        private void DocID_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                Grid_Refresh();
                FormLayout(true);
                
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

        //Methods
        private bool AppendData(DataTable dtSelected)
        {
            try
            {
                int sourceDocCodeKey = GFunc.NEInt(DocCode.Value, 0);
                int sourceDocKey = GFunc.NEInt(this.DocID.Value, 0);
                int SourceDocConKey = 0;
                GEnum.Details det;
                List<SqlParameter> paraList = new List<SqlParameter>();
                DataTable dtDetailInsert = null;

                switch (sourceDocCodeKey)
                {
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        det = GEnum.Details.Doc_Exp;
                        break;

                    default:
                        det = GEnum.Details.Doc_Itm;
                        dtSelected =dtSelected.DefaultView.ToTable(false, "DocKey", "DocItmKey");
                        break;
                }

                paraList.Add(new SqlParameter("@GUID", objDoc.GUID));
                paraList.Add(new SqlParameter("@UserKey", AppInfor.CurrentUserKey));
                paraList.Add(new SqlParameter("@SourceDocCodeKey", sourceDocCodeKey));
                paraList.Add(new SqlParameter("@SourceDocKey", sourceDocKey));

                paraList.Add(new SqlParameter("@useRemarkAsDes", Convert.ToInt16(UseAdditionalRemAsDes.Checked)));

                SqlParameter para = new SqlParameter("@SourceConKey", SourceDocConKey);
                para.Direction = ParameterDirection.Output;
                paraList.Add(para);
                paraList.Add(new SqlParameter("@DetailType", det));               
                string xmlData = GFunc.ConvertDataTableToXML(dtSelected);
                paraList.Add(new SqlParameter("@xmlDocDetail", xmlData));

                if (det == GEnum.Details.Doc_Exp)
                {
                    return DocHDRUtil.DocTransferData_Expense(sourceDocCodeKey, sourceDocKey, SourceDocConKey, dtSelected, objDoc, tagrdDetItms, true, "", UseSystemPrice.Checked, includeNSLink.Checked);
                }
                else
                {
                    int[] lineTypes = new int[] { 1200, 1210, 1300 };

                    DataTable dtInsertData = GFunc.ExecuteProc("Document_DataTransfer", paraList);

                    switch (objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Purchase_Invoice:
                        case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        case (int)GEnum.SystemCode.Purchase_Delivery:
                            // dtInsertData.DefaultView.RowFilter = "LineType not in(" + 1200 + "," + 1210 + "," + 1300 + ")";
                            //dtDetailInsert = dtInsertData.DefaultView.ToTable();
                            dtDetailInsert = dtInsertData.AsEnumerable().Where(r => lineTypes.Contains(r.Field<int>("LineType"))==false).CopyToDataTable();
                            break;
                        case (int)GEnum.SystemCode.Inventory_Adjustment:
                            if (objDoc.DocType == 400) //Add New Batch
                            {
                                //dtInsertData.DefaultView.RowFilter = "LineType not in(" + 1200 + "," + 1210 + "," + 1300 + ")";
                                //dtDetailInsert = dtInsertData.DefaultView.ToTable();
                                dtDetailInsert = dtInsertData.AsEnumerable().Where(r => lineTypes.Contains(r.Field<int>("LineType")) == false).CopyToDataTable();
                            }

                                break;
                        default:
                                dtDetailInsert = dtInsertData.Copy();
                                break;

                    }
                    return TestInsertData(dtDetailInsert);
                    //return DocHDRUtil.DocTransferData(sourceDocCodeKey, sourceDocKey, SourceDocConKey, dtDetailInsert, objDoc, tagrdDetItms, 0, "", UseSystemPrice.Checked, includeNSLink.Checked);
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

        private bool TestInsertData(DataTable dtDetailInsert)
        {
            int sourceDocCodeKey = GFunc.NEInt(DocCode.Value, 0);
            int sourceDocKey = GFunc.NEInt(this.DocID.Value, 0);
            string sourceDocID = this.DocID.Text;
            int grdRowStartIndex = tagrdDetItms.ActiveRow.Index;
            string HdrLnkKeyFldNm = "";
            string HdrLnkIDFldNm = "";
            string DetLnkFldNm = "";


            if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Order && includeNSLink.Checked)
            {
                if (DocHDRUtil.DocTransferData(sourceDocCodeKey, sourceDocKey, 0, dtDetailInsert, objDoc, tagrdDetItms, (int)GEnum.InsertAction.InsertData, "", UseSystemPrice.Checked, includeNSLink.Checked))
                {
                    switch ((GEnum.SystemCode)sourceDocCodeKey)
                    {
                        case GEnum.SystemCode.Quotation:
                        case GEnum.SystemCode.Sales_Order:
                        case GEnum.SystemCode.Delivery_Order:
                        case GEnum.SystemCode.Sales_Invoice:
                        case GEnum.SystemCode.Cash_Sale:
                        case GEnum.SystemCode.Reserve_Order:
                            switch ((GEnum.SystemCode)sourceDocCodeKey)
                            {
                                case GEnum.SystemCode.Quotation: 
                                    HdrLnkKeyFldNm = "ARQODK";
                                    HdrLnkIDFldNm = "ARQOID";
                                    DetLnkFldNm = "ARQODItm";
                                    break;
                                case GEnum.SystemCode.Sales_Order:
                                    HdrLnkKeyFldNm = "ARSODK";
                                    HdrLnkIDFldNm = "ARSOID";
                                    DetLnkFldNm = "ARSODItm";                                    
                                    break;
                                case GEnum.SystemCode.Delivery_Order:
                                    HdrLnkKeyFldNm = "ARDODK";
                                    HdrLnkIDFldNm = "ARDOID";
                                    DetLnkFldNm = "ARDODItm";
                                    break;
                                case GEnum.SystemCode.Sales_Invoice:
                                case GEnum.SystemCode.Cash_Sale:
                                    HdrLnkKeyFldNm = "ARIVDK";
                                    HdrLnkIDFldNm = "ARIVID";
                                    DetLnkFldNm = "ARIVDItm";
                                    break;
                                case GEnum.SystemCode.Reserve_Order :
                                    HdrLnkKeyFldNm = "ARRODK";
                                    HdrLnkIDFldNm = "ARROID";
                                    DetLnkFldNm = "ARRODItm";
                                    break;
                            }                          

                            for (int i = 0; i < dtDetailInsert.Rows.Count; i++) //Update Source DKey and DItm for new inserted rows
                            {
                                tagrdDetItms.Rows[i + grdRowStartIndex].Cells[HdrLnkKeyFldNm].Value = sourceDocKey;
                                tagrdDetItms.Rows[i + grdRowStartIndex].Cells[HdrLnkIDFldNm].Value = sourceDocID;
                                tagrdDetItms.Rows[i + grdRowStartIndex].Cells[DetLnkFldNm].Value = dtDetailInsert.Rows[i]["DocItmKey"];
                                if (sourceDocCodeKey == (int)GEnum.SystemCode.Sales_Order)
                                {
                                    tagrdDetItms.Rows[i + grdRowStartIndex].Cells["ARSOPOID"].Value = dtDetailInsert.Rows[i]["DocCustPONum"];
                                }
                                tagrdDetItms.Rows[i + grdRowStartIndex].Update();                               
                            }
                            break;
                    }
                    return true;
                }
                return false;
            }
            else
            {
                switch ((GEnum.SystemCode)sourceDocCodeKey)
                {
                    case GEnum.SystemCode.Order_Consignment:
                    case GEnum.SystemCode.Received_Consignment:
                        // CS_CPOdetitm and CS_CPDDetitm tables don't have ItmdetSN. added by Jane on 10-Jun-2014
                        return DocHDRUtil.DocTransferData(sourceDocCodeKey, sourceDocKey, 0, dtDetailInsert, objDoc, tagrdDetItms, (int)GEnum.InsertAction.InsertData, "ItmSN", UseSystemPrice.Checked, includeNSLink.Checked);
                        break;
                    default:
                        return DocHDRUtil.DocTransferData(sourceDocCodeKey, sourceDocKey, 0, dtDetailInsert, objDoc, tagrdDetItms, (int)GEnum.InsertAction.InsertData, "ItmSN,ItmDetSN", UseSystemPrice.Checked, includeNSLink.Checked);
                        break;
                }
            }   
        }
        private void ComboDocCode_Fill()
        {
            try
            {
                GlobalUI.BindComboValue(DocCode, "SYSMsgDCInsertData" + "%" + docCodekey, "CodeDesLang1", "CodeKey", 0);

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
        private void ComboDocID_Fill()
        {
            try
            {
                if ((!GFunc.IsNE(StartDate.DateValue)) && (!GFunc.IsNE(EndDate.DateValue)) && (!GFunc.IsNEZ(DocCode.Value)))
                {
                    GlobalUI.BindComboValue(DocID, "frmInsertDataDocID" + "%" + DocCode.SelectedRow.Cells["CodeKey"].Value + "%" + DateTime.Parse(StartDate.Value.ToString()).ToString("dd-MMM-yyyy") + "%" + DateTime.Parse(EndDate.Value.ToString()).ToString("dd-MMM-yyyy"), "DocID", "DocKey", 0);
                    this.DocID.Enabled = true;
                }
                else
                {
                    this.DocID.DataSource = null;
                }
                this.DocID.SetValueTrigger(null, false);
                FormLayout(false);
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, true);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void FormLayout(bool enableAppend)
        {
            bool enableControl = false;
            try
            {
                if (enableAppend && !GFunc.IsNEZ(this.DocID.Value))
                    enableControl = true;

                this.tagrdDocItmDetail.Enabled = enableControl;
                this.btnAppend.Enabled = enableControl;
                this.btnAppendAll.Enabled = enableControl;
                this.btnSelectAll.Enabled = enableControl;
                this.btnUnSelectAll.Enabled = enableControl;

                OverrideSelectedRowFormat();
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
            try
            {
                SYSFormSettingID objFormSettingID = new SYSFormSettingID();
                string ListSettingID = "frmInsertDataGet" + GFunc.NEStr(DocCode.SelectedRow.Cells["TblNm"].Value, string.Empty).ToString().Replace("_", "") + "Detail%" + GFunc.NEInt(DocID.Value, 0).ToString();

                ContextMenuSetting = formContextMenuSetting;
                ContextMenuSetting += GlobalUI.ContextMenuSettingGrid_GetNew(tagrdDocItmDetail.Name, ListSettingID);
                GlobalUI.Grid_Format(tagrdDocItmDetail, ListSettingID, true, true);
                GlobalUI.GridControl_Set(tagrdDocItmDetail, "frmInsertDataGet" + GFunc.NEStr(DocCode.SelectedRow.Cells["TblNm"].Value, string.Empty).ToString().Replace("_", "") + "Detail", (int)objDoc.DocCodeKey);
                OverrideSelectedRowFormat();

                /* added by YST on 2023-10-12 to show child items in Purchase sides, but not to show in Sales sides, requested by Joan from Procurement department */
                switch ((GEnum.SystemCode)docCodekey)
                {
                    case GEnum.SystemCode.Quotation:
                    case GEnum.SystemCode.Reserve_Order:
                    case GEnum.SystemCode.Sales_Order:
                    case GEnum.SystemCode.Delivery_Order:
                    case GEnum.SystemCode.Sales_Invoice:
                    case GEnum.SystemCode.Cash_Sale:
                    case GEnum.SystemCode.Sales_Credit_Note:
                    case GEnum.SystemCode.Sales_Debit_Note:
                    case GEnum.SystemCode.Cash_Credit_Note:
                    case GEnum.SystemCode.Cash_Debit_Note:
                        if (tagrdDocItmDetail.DataSource != null)
                        {
                            ((DataTable)tagrdDocItmDetail.DataSource).DefaultView.RowFilter = "LineLinkKey = 0";
                        }                        
                        break;
                }

                //ensure that no row is active and selected when we refresh the grid bcos a Active row may not mean that a row is selected
                tagrdDocItmDetail.ActiveRow = null;
                
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
        private void OverrideSelectedRowFormat()
        {
            //Set Selected Row Appearence
            Infragistics.Win.Appearance appearence_SelectedRow = new Infragistics.Win.Appearance();
            appearence_SelectedRow.BackColor = System.Drawing.Color.BlanchedAlmond;
            appearence_SelectedRow.BackColor2 = SystemColors.Window;
            appearence_SelectedRow.ForeColor = Color.Black;
            appearence_SelectedRow.BackGradientStyle = GradientStyle.GlassBottom50Bright;
            tagrdDocItmDetail.DisplayLayout.Override.SelectedRowAppearance = appearence_SelectedRow;
        }//Completed

        //Set Error Methods
        private void OnDataError(object sender, TAUtil.TAErrorEventArgs e)
        {
            if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_NUMERIC)
            {
                MsgBox.Show(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
            }
            if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_DATE)
            {
                MsgBox.Show(MsgID.Common.InvalidCellDataTypeDate + "% Data Type");
            }
            if (e.ErrorCode == TAUtil.TAErrorCode.INTEGER_EXCEED_LIMIT)
            {
                MsgBox.Show(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
            }
            if (e.ErrorCode == TAUtil.TAErrorCode.FORMULA_NOT_RECOGNIZE)
            {
                MsgBox.Show("FORMULA NOT RECOGNIZE");
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