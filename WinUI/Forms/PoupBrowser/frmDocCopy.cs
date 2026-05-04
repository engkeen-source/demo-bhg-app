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
using Microsoft.VisualBasic;
using TAUtil;
using System.IO;
using System.Configuration;
using System.Threading;

namespace WinUI
{
    public partial class frmDocCopy : Form
    {
        #region Local Variables
        string ContextMenuSetting = string.Empty;

        private bool useImportPrice = true; 
        private int TgtDK;                          //Caller DocKey
        private int TgtDC;                          //Caller DocCodeKey
        private int SourceDC;                       //To Copy from DocCodeKey
        private UltraGrid gdTgtDetail = null;       //Caller Form Detail Grid
        private DataTable dtTgtDetail = null;
        Hashtable htDetailGrd = new Hashtable();
        TAUtil.TAExcelImport objExcelImport = null;
        public GVar.ListEvent_CopyRecord CopyRecordEvent = null;
        #endregion

        //Initialize
        public frmDocCopy(int TargetDocKey, int TargetDocCodeKey, UltraGrid tagrdDetItms)
        {
            //Note: for some Document, the tagrdDetItms is never used but is pass over, e.g packing list
            //in future we will create another overload not to pass this grid parameter over.
            //we only need this grid for the import feature, so those documents that import is not allow, actually do not use this parameter at all
            InitializeComponent();
            TgtDK = TargetDocKey;
            TgtDC = TargetDocCodeKey;
            if (GFunc.IsNEZ(gdTgtDetail) == false)
            {
                dtTgtDetail = gdTgtDetail.DataSource as DataTable;
            }
            gdTgtDetail = tagrdDetItms;

        }//Completed
        public frmDocCopy()
        {
            InitializeComponent();
        }//Completed

        //Form Events
        private void frmDocCopy_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, TgtDC);
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
        private void frmDocCopy_Load(object sender, EventArgs e)
        {
            try
            {
                //Fill all Combo and set default value to all criteria
                GlobalUI.Combos_Fill(this, TgtDC);
                if (DocCode.Rows.Count > 0)
                {
                    DocCode.SetValueTrigger(TgtDC, false);  //Set Default to Caller DocCodeKey
                    SourceDC = GFunc.NEInt(DocCode.SelectedRow.Cells["CustomInt1"].Value, 0); // Source Document DocCodeKey
                }

                StartDate.DateValue = DateTime.Today.AddMonths(-1);
                EndDate.DateValue = DateTime.Today;

                //Grid Format
                GlobalUI.FormGrids_Set(this, TgtDC, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(TgtDC);

                Form_Layout();
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

        private void tagrdDocList_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            if (tagrdDocList.ActiveRow != null)
                btnAppend_Click(sender, e);
        }
        //Button Event
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }//Completed
        private void btnRequery_Click(object sender, EventArgs e)
        {
            try
            {
                StartDate.DateValue = GFunc.NEDateTime(this.StartDate.DateValue, DateTime.Today.AddMonths(-1));
                EndDate.DateValue = GFunc.NEDateTime(this.EndDate.DateValue, DateTime.Today);

                if (GFunc.IsNE(DocCode.Value) == false)
                {
                    SourceDC = GFunc.NEInt(DocCode.SelectedRow.Cells["CustomInt1"].Value, 0);
                    GlobalUI.FormGrids_Set(this, TgtDC, out ContextMenuSetting);
                    if (tagrdDocList.Rows.Count > 0)
                        tagrdDocList.Enabled = true;
                    else
                        tagrdDocList.Enabled = false;
                }
                else
                    tagrdDocList.Enabled = false;
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
                tagrdDocList.PerformAction(UltraGridAction.ExitEditMode);
                tagrdDocList.UpdateData();

                switch (tabControl1.SelectedTab.Text.ToLower())
                {
                    case "copy myself":
                        this.CopyRecordEvent.Invoke(GEnum.CopyOption.CopyMySelf, SourceDC, TgtDK, null, NSLink.Checked);
                        this.DialogResult = DialogResult.OK;
                        break;

                    case "copy from":
                        if (tagrdDocList.ActiveRow == null || tagrdDocList.Enabled == false)
                            return;
                        else
                        {
                            if (TgtDC == (int)GEnum.SystemCode.Delivery_Order || TgtDC == (int)GEnum.SystemCode.Sales_Invoice
                                || TgtDC == (int)GEnum.SystemCode.Sales_Credit_Note || TgtDC == (int)GEnum.SystemCode.Sales_Debit_Note || TgtDC == (int)GEnum.SystemCode.Sales_Order)
                            {
                                MSTCon objCon = MSTCon.Get(GFunc.NEStr(tagrdDocList.ActiveRow.Cells["ConID"].Value, ""));
                                if (objCon.CCBType == (int)GEnum.CCBType.CH)
                                {
                                    MsgBox.Show("Cannot copy Cash customer. Please select another record.");
                                    return;
                                }
                            }
                            this.CopyRecordEvent.Invoke(GEnum.CopyOption.CopyFrom, SourceDC, GFunc.NEInt(tagrdDocList.ActiveRow.Cells["DocKey"].Value, 0), null, this.NSLink.Checked);
                            this.DialogResult = DialogResult.OK;
                        }
                        break;

                    case "import":                       
                        //Check whether Default Item is selected or not
                        switch (TgtDC)
                        {
                            case (int)GEnum.SystemCode.Inventory_Adjustment:
                            case (int)GEnum.SystemCode.Received_Consignment:
                            case (int)GEnum.SystemCode.Order_Consignment:
                            case (int)GEnum.SystemCode.Issue_Consignment:
                            case (int)GEnum.SystemCode.Inventory_Transfer:
                            case (int)GEnum.SystemCode.Payment_Received:
                            case (int)GEnum.SystemCode.Payment_Issue:
                            case (int)GEnum.SystemCode.Cash_Payment_Received:                                
                                break;
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Delivery_Order:
                                {
                                    useImportPrice = true;
                                    break;
                                }
                                break;
                            default:
                                if (GFunc.IsNE(defaultItem.Value))
                                {
                                    //MsgBox.Show("Default Item cannot be empty!");
                                    if (!string.IsNullOrEmpty(ExcelSheets.Text))
                                        defaultItem.Value = SysOptionUtility.GetInt("DefaultItmKey");
                                    defaultItem.Focus();
                                    return;
                                }
                                break;
                        }

                        DataTable dtItems;
                        if (!string.IsNullOrEmpty(ExcelSheets.Text))
                        {
                            dtItems = this.GetExcelData(ExcelSheets.Text.ToString(), GFunc.NEInt(defaultItem.Value, 0));
                            if (dtItems != null && dtItems.Rows.Count > 0)
                            {
                                this.CopyRecordEvent.Invoke(GEnum.CopyOption.Import, SourceDC, 0, dtItems, false);
                            }
                        }
                        else
                        {
                            MsgBox.Show("Please browse the excel workbook with 'ImportSheet'.");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                this.Error(ex, true);
            }
        }//Completed

        //Form Display - Controlling and format 
        private void Form_Layout()
        {
            bool runDisableAppend = true;

            //Disable the combo when there is only one row in drop down
            if (DocCode.Rows.Count == 1)
                DocCode.Enabled = false;

            switch (TgtDC)
            {
                case (int)GEnum.SystemCode.Inventory_Production:
                case (int)GEnum.SystemCode.Return_Consignment:
                case (int)GEnum.SystemCode.Deposit:
                case (int)GEnum.SystemCode.Bank_Revaluation:
                case (int)GEnum.SystemCode.Consignment_Settlement:
                    tabControl1.TabPages.Remove(tabControl1.TabPages["CopyFrom"]);
                    tabControl1.TabPages.Remove(tabControl1.TabPages["Import"]);
                    SourceDC = 0;
                    runDisableAppend = false;
                    break;
                //case (int)GEnum.SystemCode.Inventory_Adjustment:
                case (int)GEnum.SystemCode.Packing_List:
                case (int)GEnum.SystemCode.Journal:
                case (int)GEnum.SystemCode.Purchase_Adjustment:
                case (int)GEnum.SystemCode.Sales_Adjustment:
                case (int)GEnum.SystemCode.Cash_Adjustment:
                    tabControl1.TabPages.Remove(tabControl1.TabPages["Import"]);
                    break;              
                default:
                    break;
            }

            //Control Default Item
            switch (TgtDC)
            {
                case (int)GEnum.SystemCode.Inventory_Adjustment:
                case (int)GEnum.SystemCode.Received_Consignment:
                case (int)GEnum.SystemCode.Order_Consignment:
                case (int)GEnum.SystemCode.Issue_Consignment:               
                case (int)GEnum.SystemCode.Inventory_Transfer:
                case (int)GEnum.SystemCode.Payment_Received:
                case (int)GEnum.SystemCode.Payment_Issue:
                case (int)GEnum.SystemCode.Cash_Payment_Received:
                    label5.Enabled = false;
                    defaultItem.Enabled = false;
                    break;
            }

            if (runDisableAppend)
            {
                if (GFunc.IsNEZ(DocCode.Value))
                    tagrdDocList.Enabled = false;
            }

            tagrdDocList.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            tagrdDocList.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
            tagrdDocList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
            if (tagrdDocList != null)
            {
                tagrdDocList.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True;
                tagrdDocList.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ExtendFirstColumn;
                tagrdDocList.DisplayLayout.Override.CellClickAction = CellClickAction.Default;
            }

        }//Completed

        //Control Event
        private void Combo_ItemNotInList(object sender, Infragistics.Win.UltraWinEditors.ValidationErrorEventArgs e)
        {
            GlobalUI.ItemNotInList(sender as TAUtil.TAComboBox, e, false, 0);
        }//Completed
        private void Criteria_CustomUpdate(object sender, CancelEventArgs e)
        {
            tagrdDocList.Enabled = false;
        }//Completed
        private void ExcelPath_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            OpenFileDialog oDlg = new OpenFileDialog();
            oDlg.Filter = "Excel files (*.xlsx)|*.xlsx|(*.xls)|*.xls";
            DialogResult dlg = oDlg.ShowDialog();

            if (dlg == DialogResult.OK)
            {
                ExcelPath.SetValueTrigger(oDlg.FileName, false);
            }
            else
                return;
            try
            {
                objExcelImport = new TAUtil.TAExcelImport(ExcelPath.Text);
                DataTable dt = new DataTable();
                dt.Columns.Add("ValueCol");
                dt.Columns.Add("Excel Sheet");

                int i = 0;
                if (!GFunc.IsNE(objExcelImport.ExcelSheets))
                {
                    foreach (string sheet in objExcelImport.ExcelSheets)
                    {
                        dt.Rows.Add(i++, sheet);
                    }
                }
                ExcelSheets.DataSource = dt;
                ExcelSheets.ValueMember = "ValueCol";
                ExcelSheets.DisplayMember = "Excel Sheet";
                ExcelSheets.SetValueTrigger(0, false);
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
                oDlg = null;
                this.Cursor = Cursors.Default;
            }
        }//Completed

        /// <summary>
        /// Retrieve Data from Excel Sheet and validate structure & data. 
        /// Execute Doc_Import store proc to get the related key values of excel data, filter un-necessary Item types and inaccessible records
        /// Insert the finalize imported data to the grid which is directly updated in Document Form
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="defaultItmKey"></param>
        /// <returns></returns>
        private DataTable GetExcelData(string sheetName, int defaultItmKey)
        {
            try
            {
                string defaultItemUOMID = string.Empty;
                string rowFilter = "";
                
                DocComUtility.AppRunningState = "ImportInProgress";

                //Retrieve data from Excel Sheet as DataTable                
                DataTable dtExcelData = objExcelImport.GetExcelData(sheetName);               
                try
                {
                    //filter blank rows
                   // dtExcelData.DefaultView.RowFilter = rowFilter;
                    //At this stage, blank rows are excluded in dtExcelData
                   // dtExcelData = dtExcelData.DefaultView.ToTable();
                    dtExcelData = dtExcelData.AsEnumerable().Where(row => row.ItemArray.Any(field => !(field is DBNull))).CopyToDataTable();
                }
                catch (Exception ex)
                {
                    MsgBox.Show("Unable to import.\n\nThere are blank rows between data in your source excel file.");
                    return null;
                }
               
                //TgtDC=0 mean this form was called by non Document,to get only excel data without validate and perform other row events.
                if (TgtDC == 0)
                {
                    return dtExcelData;
                }

                //Validate structure and data according to pre-definition in SQL Table SyS_DocItem_ImportStructure
                if (GFunc.ValidateExcelData(dtExcelData, TgtDC) == false)
                    return null;
                
                //Add row no to display error
                dtExcelData.Columns.Add("RowNo", typeof(int));
                int iRowNo=0;
                foreach (DataRow row in dtExcelData.Rows)
                {
                    iRowNo++;
                    row["RowNo"] = iRowNo;
                }
                dtExcelData.AcceptChanges();
                string XMLformat = "";
                string ItmID = string.Empty;
                string itmTypes = string.Empty;
                string errorMsg = string.Empty;
                int ImportFailed = 0;
                GlobalUI.bRuningImport = true;
                DocUtility.bRuningImport = true;
                SqlParameter paraOut1 = null;
                SqlParameter paraOut2 = null;
                Document objDoc = GlobalUI.ActiveFormDocument_Get();
                List<SqlParameter> paraList = new List<SqlParameter>();
                SqlParameter paraOut = null;
                DataTable dtImportData = null;
                dtExcelData.TableName = "dtExcelData";
                                
                int docConKey = 0;
                switch ((GEnum.SystemCode)objDoc.DocCodeKey)
                {
                    case GEnum.SystemCode.Purchase_Request:
                    case GEnum.SystemCode.Inventory_Transfer:
                    case GEnum.SystemCode.Inventory_Adjustment:
                        break;
                    default:
                        docConKey = GFunc.NEInt(GFunc.GetPropertyValue("DocConKey", objDoc), 0);
                        break;
                }
                switch (TgtDC)
                {
                    #region Import Expense to Paymment document
                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                        errorMsg = string.Empty;
                        paraList.Add(new SqlParameter("@DocCodeKey", TgtDC));
                        paraList.Add(new SqlParameter("@DefaultItmKey", this.defaultItem.Value));
                        paraList.Add(new SqlParameter("@DocKey", TgtDK));
                        paraList.Add(new SqlParameter("@DocConKey", docConKey));

                        paraList.Add(new SqlParameter("@CurrentDocItmKey", DocComUtility.GridAutoID_Get(gdTgtDetail, "DocKey", "DocItmKey") - 1));
                        paraList.Add(new SqlParameter("@userKey", AppInfor.CurrentUserKey));

                        //get the list of itmtype allow for import base on the doccodekey
                        //itmTypes = GFunc.GetItmTypeGyDocCode(TgtDC);
                        //itmTypes = GFunc.GetItmTypeGyDocCode(TgtDC); To remove , now add Items type in SP
                        //paraList.Add(new SqlParameter("@ItmType", itmTypes));To remove , now add Items type in SP

                        paraOut1 = new SqlParameter("@ImportFailed", ImportFailed);
                        paraOut1.Direction = ParameterDirection.Output;
                        paraList.Add(paraOut1);

                        paraOut2 = new SqlParameter("@FailedMessage", errorMsg);
                        paraOut2.Direction = ParameterDirection.Output;
                        paraList.Add(paraOut2);
                        XMLformat = GFunc.ConvertDataTableToXML(dtExcelData);
                        paraList.Add(new SqlParameter("@xmlExcelData", XMLformat));
                        dtImportData = GFunc.ExecuteProc("Doc_Import", paraList);

                       
                        if (dtImportData != null)
                        {
                            IEnumerable<DataRow> dtImportFilter = dtImportData.AsEnumerable().Where(r => r.Field<int>("ExpAccKey") == 0);

                            if (dtImportFilter.Count() > 0)
                            {
                                MsgBoxGrid.Show("Import Failed,Please check Excel Data.", dtImportFilter.CopyToDataTable(), GEnum.MsgBoxIcon.Error, GEnum.MsgBoxButton.OK);
                                return null;
                            }

                            foreach (DataRow row in dtImportData.Rows)
                            {
                                try
                                {
                                    gdTgtDetail.DisplayLayout.Bands[0].AddNew();
                                    gdTgtDetail.ActiveRow = gdTgtDetail.Rows[gdTgtDetail.Rows.Count - 1];                                    
                                    
                                    gdTgtDetail.ActiveRow.Cells["ExpAccKey"].Value = row["ExpAccKey"];
                                    gdTgtDetail.ActiveRow.Cells["ExpDate"].Value = row["ExpDate"];
                                    gdTgtDetail.ActiveRow.Cells["ExpRef"].Value = row["ExpRef"];
                                    gdTgtDetail.ActiveRow.Cells["ExpDes"].Value = row["ExpDes"];
                                    gdTgtDetail.ActiveRow.Cells["ExpAmtGST"].Value = row["ExpAmtGST"];
                                    gdTgtDetail.ActiveRow.Cells["ExpTaxGrpKey"].Value = GFunc.GetIntPropertyValue("DocTaxGrpKey", objDoc);
                                    gdTgtDetail.ActiveRow.Cells["ExpTaxGrpRate"].Value = GFunc.GetDecimalPropertyValue("DocTaxGrpRate", objDoc);
                                    gdTgtDetail.ActiveRow.Cells["ExpJobKey"].Value = row["ExpJobKey"];
                                    gdTgtDetail.ActiveRow.Cells["ExpJobPhaseKey"].Value = row["ExpJobPhaseKey"];
                                    gdTgtDetail.ActiveRow.Cells["ExpJobTaskKey"].Value = row["ExpJobTaskKey"];
                                    gdTgtDetail.ActiveRow.Cells["ExpJobCostTypeKey"].Value = row["ExpJobCostTypeKey"];
                                    gdTgtDetail.ActiveRow.Cells["ExpAccID"].Value = row["ExpAccID"];
                                    gdTgtDetail.ActiveRow.Cells["ExpAccDes"].Value = row["ExpAccDes"];

                                }
                                catch (Exception ex)
                                {
                                    MsgBox.Show("Importing is fail. Error Description : " + ex.Message);
                                    throw ex;
                                }
                                DocDetUtil.ExpAmtGST_CustomeUpdate(objDoc, gdTgtDetail);
                                gdTgtDetail.ActiveRow.Update();
                            }
                        }

                        break;
                    #endregion


                    #region Standard Import to Document Detail
                    default:

                        #region Standard Import Check

                        paraList.Add(new SqlParameter("@DocCodeKey", TgtDC));
                        XMLformat = GFunc.ConvertDataTableToXML(dtExcelData);
                        paraList.Add(new SqlParameter("@xmlExcelData", XMLformat));
                        DataTable dtImportCheck = GFunc.ExecuteProc("Doc_ImportCheck", paraList);

                        //Mic Check; 21 Nov 2012; Jack Added for user to change ItmID only before passing to grid
                        switch (TgtDC)
                        {

                            case (int)GEnum.SystemCode.Quotation:
                            case (int)GEnum.SystemCode.Sales_Order:
                            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                            case (int)GEnum.SystemCode.Delivery_Order:
                            case (int)GEnum.SystemCode.Sales_Invoice:
                            case (int)GEnum.SystemCode.Sales_Debit_Note:
                            case (int)GEnum.SystemCode.Sales_Credit_Note:
                            case (int)GEnum.SystemCode.Cash_Sale:
                            case (int)GEnum.SystemCode.Cash_Debit_Note:
                            case (int)GEnum.SystemCode.Cash_Credit_Note:
                            case (int)GEnum.SystemCode.Purchase_Request:
                            case (int)GEnum.SystemCode.Purchase_Order:
                            case (int)GEnum.SystemCode.Purchase_Delivery:
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            case (int)GEnum.SystemCode.Purchase_Credit_Note:

                            case (int)GEnum.SystemCode.Inventory_Adjustment:
                            //case (int)GEnum.SystemCode.Inventory_Transfer:
                            //case (int)GEnum.SystemCode.Issue_Consignment:
                            //case (int)GEnum.SystemCode.Order_Consignment:
                            //case (int)GEnum.SystemCode.Received_Consignment:

                                dtImportCheck.DefaultView.RowFilter = "ItmKey=0";
                                if (dtImportCheck.DefaultView.Count > 0)
                                {
                                    frmItmUpdate itmUpdate = new frmItmUpdate(dtImportCheck);
                                    if (itmUpdate.ShowDialog() == DialogResult.OK)
                                    {
                                        //User updated the ItmIDs
                                        dtImportCheck.DefaultView.RowFilter = "";
                                        dtExcelData = dtImportCheck;
                                        //Add row no to display error
                                        dtExcelData.Columns.Add("RowNo", typeof(int));
                                        iRowNo = 0;
                                        foreach (DataRow row in dtExcelData.Rows)
                                        {
                                            iRowNo++;
                                            row["RowNo"] = iRowNo;
                                        }
                                        dtExcelData.AcceptChanges();
                                        dtExcelData.TableName = "dtExcelData";
                                    }
                                }
                                break;

                            default:
                                break;
                        }

                        #endregion                      
          
                        paraList = new List<SqlParameter>();
                        GlobalUI.bRuningImport = true;
                        DocUtility.bRuningImport = true;
                        XMLformat = "";
                        ItmID = string.Empty;
                        itmTypes = string.Empty;
                        ImportFailed = 0;
                        errorMsg = string.Empty;
                        paraList.Add(new SqlParameter("@DocCodeKey", TgtDC));
                        paraList.Add(new SqlParameter("@DefaultItmKey", this.defaultItem.Value));
                        paraList.Add(new SqlParameter("@DocKey", TgtDK));
                        paraList.Add(new SqlParameter("@DocConKey", docConKey));
                        
                        paraList.Add(new SqlParameter("@CurrentDocItmKey", DocComUtility.GridAutoID_Get(gdTgtDetail, "DocKey", "DocItmKey") - 1));
                        paraList.Add(new SqlParameter("@userKey", AppInfor.CurrentUserKey));

                        //get the list of itmtype allow for import base on the doccodekey
                        //itmTypes = GFunc.GetItmTypeGyDocCode(TgtDC); To remove , now add Items type in SP
                        //paraList.Add(new SqlParameter("@ItmType", itmTypes));To remove , now add Items type in SP

                        paraOut1 = new SqlParameter("@ImportFailed", ImportFailed);
                        paraOut1.Direction = ParameterDirection.Output;
                        paraList.Add(paraOut1);

                        paraOut2 = new SqlParameter("@FailedMessage", SqlDbType.NVarChar, 255);                        
                        paraOut2.Direction = ParameterDirection.Output;
                        paraList.Add(paraOut2);
                        XMLformat = GFunc.ConvertDataTableToXML(dtExcelData);
                        paraList.Add(new SqlParameter("@xmlExcelData", XMLformat));

                        //Execute store proc which returns a DataSet containing 3 DataTables
                        /*dtImportData holds imported data from Excel + related Key Values 
                          + (InItmType column to know if the record is in the required Item type ) 
                          + (InAccessGroup column to know if the record can be accessed by the user)  */
                        //Note: InItmType and InAccessGroup columns are not used anymore. can remove from store proc
                        //SQL table MST_ItmDetAss data for the imported Item records
                        //SQL table MST_Itm data for the imported Item records
                        dtImportData = GFunc.ExecuteProc("Doc_Import", paraList);
                        //DataSet dsImportData = GFunc.ExecuteProcDataSet("Doc_Import", paraList);
                        if (GFunc.NEInt(paraOut1.Value,0) > 0)
                        {
                            //dtImportData.DefaultView.RowFilter = "InItmType = 0 Or InAccessGroup =0";
                            MsgBoxGrid.Show(GFunc.NEStr(paraOut2.Value, string.Empty) + " Import Failed,Please check Excel Data.", dtImportData, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
                            return null;
                        }
                       
                        foreach (DataRow row in dtImportData.Rows)
                        {
                            if (GFunc.NEInt(row["LineType"], 0) == 1100)
                            {
                                int? vPriceKey = GFunc.NEInt(GFunc.GetPropertyValue("DocPriceType", objDoc), 0);
                                int? vCurrKey = GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 0);
                                row["ItmPriceAfter"] = DocComUtility.Price_Get(vPriceKey, GFunc.NEInt(row["ItmKey"], 0), GFunc.NEInt(row["ItmVendorKey"], 0), vCurrKey);
                            }
                        }
                       
                        DocHDRUtil.DocTransferData((int)objDoc.DocCodeKey, (int)objDoc.DocKey, docConKey, dtImportData, objDoc, gdTgtDetail, 0, "", !useImportPrice , false);
                        break;
                    #endregion
                }
                   
                DocComUtility.AppRunningState = "";
                htDetailGrd.Clear();
                htDetailGrd.Add(GEnum.Details.Doc_Itm, gdTgtDetail);
                DocComUtility.CalForm(objDoc, htDetailGrd, true, false);

                this.Close();
            }
            catch (Exception ex)
            {
                throw this.Error(ex, false);
            }
            finally
            {
                GlobalUI.bRuningImport = false;
                DocUtility.bRuningImport = false;
                DocComUtility.AppRunningState = "";
            }
            return gdTgtDetail.DataSource as DataTable;
        }

        public void frmDocCopy_OnClosing(object sender, CancelEventArgs e)
        {
        }
        public void frmDocCopy_FormClosed(object sender, EventArgs e)
        {
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

        /* added by YST on 2023/07/14 */
        private void btnDownload_Click(object sender, EventArgs e)
        {
            Thread th = null;
            frmLoading f = null;
            string Msg = "", CodeID = "";
            string DownloadPath = "";
            string fileSeverPath = "";
            string excelTemplateName = "";
            string excelTemplatePath = "";
            string templateDownloadPath = "";

            List<SqlParameter> listPara = new List<SqlParameter>();
            listPara.Add(new SqlParameter("@DocCodeKey", TgtDC));
            DataTable dtImportColumns = GFunc.ExecuteProc("Get_ImportColumns", listPara);

            if (dtImportColumns == null || dtImportColumns.Rows.Count == 0)
            {
                MessageBox.Show("There is no column setting for excel template!");
                return;
            }
            try
            {                
                CodeID = dtImportColumns.Rows[0]["CodeID"].ToString();                
                DownloadPath = Path.GetTempPath();
                DownloadPath = DownloadPath.Substring(0, DownloadPath.IndexOf("AppData")) + "Downloads\\";
                //fileSeverPath = "\\\\sgbhvm02\\bossupdater\\ImportExcelTemplate\\";
                fileSeverPath = ConfigurationManager.AppSettings["ExcelTemplateFileSeverPath"];
                excelTemplateName = "ExcelImportTemplate.xlsx";
                excelTemplatePath = Application.StartupPath + "\\ImportExcelTemplate\\";                
                templateDownloadPath = DownloadPath + CodeID + "_ImportTemplate.xlsx";

                if (!Directory.Exists(excelTemplatePath))
                {
                    Directory.CreateDirectory(excelTemplatePath);
                    if (Directory.Exists(fileSeverPath))
                    {
                        fileSeverPath = fileSeverPath + excelTemplateName;
                        if (Directory.Exists(excelTemplatePath))
                        {
                            excelTemplatePath = excelTemplatePath + excelTemplateName;
                            File.Copy(fileSeverPath, excelTemplatePath);
                        }
                    }
                }
                else
                {
                    excelTemplatePath = excelTemplatePath + excelTemplateName;
                }

                try
                {
                    f = new frmLoading();
                    th = new Thread(() => ShowLoading(f));
                    th.Start();
                } catch { }

                Microsoft.Office.Interop.Excel.Application excelApp;                               
                Microsoft.Office.Interop.Excel.Workbook excelWorkbook;
                Microsoft.Office.Interop.Excel.Worksheet excelSheet;

                excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelWorkbook = excelApp.Workbooks.Open(excelTemplatePath);
                excelSheet = excelWorkbook.Worksheets["ImportSheet"];

                for (int i = 0; i < dtImportColumns.Rows.Count; i++)
                {
                    excelSheet.Cells[1, i + 1].Value = dtImportColumns.Rows[i]["ColHeader"];
                    excelSheet.Cells[2, i + 1].Value = dtImportColumns.Rows[i]["ColDataType"].ToString() == "Text" ? "Text" : "0";
                    excelSheet.Cells[3, i + 1].Value = dtImportColumns.Rows[i]["ColDataType"].ToString() == "Text" ? "Text" : "0";
                }

                CloseLoading(th, f);

                excelWorkbook.SaveAs(templateDownloadPath);
                excelWorkbook.Close(false);              
                excelApp.Quit();

                excelSheet = null;
                excelWorkbook = null;
                excelApp = null;

                Msg = "<font color='red'>" + CodeID + "_ImportTemplate.xlsx" + "</font> has been downloaded successfully under " + DownloadPath;
                MsgBox.Show(Msg, GEnum.MsgBoxIcon.Information, GEnum.MsgBoxButton.OK);
            }
            catch(TAException ex)
            {
                CloseLoading(th, f);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().Contains("Cannot access"))
                {
                    Msg = "Please close the file named '" + templateDownloadPath + "' if it is currently opened.";
                }
                else if (ex.Message.ToString().Contains("Invalid index"))
                {
                    Msg = "Excel sheet named 'ImportSheet' is missing in the workbook, <br/>" + excelTemplatePath;
                }
                else
                {
                    Msg = ex.Message;
                }

                MsgBox.Show(Msg, GEnum.MsgBoxIcon.Error, GEnum.MsgBoxButton.OK);
            }
            finally
            {
                CloseLoading(th, f);
            }
        }
        private void ShowLoading(frmLoading f)
        {
            if (f != null)
                f.ShowDialog();
        }
        private void CloseLoading(Thread th, frmLoading f)
        {
            try
            {
                if (th != null && f != null)
                {
                    f.CloseMe();
                    f = null;
                    th.Abort();
                }
            }
            catch { }
        }
        /*end by YST */
    }
}
