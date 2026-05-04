using System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using BOLib;
using Infragistics.Win.UltraWinTabbedMdi;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Documents.Excel;
using System.Text.RegularExpressions;
using System.Transactions;
using TAUtil;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Net.Sockets;

namespace WinUI
{
    public partial class frmARSO : Form, DocInterface
    {
        #region Local Variables

        private BOLib.ARSOFactory objFactory = null;
        private string ContextMenuSetting = string.Empty;

        private GEnum.SystemCode OpenCode;
        private bool formClose = false;
        private bool ExclusiveSaleJob = false;

        Hashtable htDetailGrd = new Hashtable();
        frmDocList DocListForm = null;
        public GVar.ListEvent_RefreshRecord ListEvent_RefreshRecord = null;
        public GVar.ListEvent_CloseFORM ListEvent_CloseFORM = null;
        private const string TextBoxChecker = "([a-zA-Z0-9]| [-_.,/():;?\\'\"\b])";

        //only use to show Read only data viewer for AuditLog 
        private bool IsOpenFromAuditLog = false;
        private DataTable _dtHeader = null;
        private DataSet _dsDetail = null;

        private DataTable ConInActive = null;//added by thettm on 12-sept-2017

        //For for Create Document from other source
        private int? source_DK = 0;
        private int? source_DC = 0;
        private bool isCash = false;
        //int paypalDItm = 0;
        UltraGridRow pRow = null;
        //private string link = "http://localhost:8088/estore05feb/";
        private string link = "https://bh-estore.com/";
        //private bool IsAldyCheckApp = false;
        private int RequestingApproval = 2;
        private bool OrangeCus = false, RedCus = false, CheckRejected = false, CheckCrLimitRejected = false;
        private int CheckApproval = 2, CheckCrLimitApproval = 2, CheckState = 0;
        private int DState = 0;
        bool isCancel = false;
        bool isCancelValidation = true;        
        string approvalStatus = null;
        bool webviewEnsure = false;

        DataTable dtJobEst = null;
        #endregion

        /// <summary>
        /// Doc Interface method
        /// </summary>
        /// <param name="objDoc"></param>
        /// <param name="detail"></param>       
        public void GetDocInfor(out Document objDoc, out Hashtable detail)
        {
            objDoc = objFactory.Doc;
            AddItm_Hash();
            detail = htDetailGrd;
        }//Completed

        private void Text_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (sender.GetType() == typeof(TAUtil.TATextBoxEditor))
                    ((TAUtil.TATextBoxEditor)sender).SetValueTrigger(((TAUtil.TATextBoxEditor)sender).Text.ToUpper(), false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //Initialize
        public frmARSO()
        {
            InitializeComponent();
        }//Completed
        //public frmARSO(GEnum.SystemCode DocCodeKey) //commented by thettm on 29 jan 2018
        public frmARSO(GEnum.SystemCode DocCodeKey,Boolean IsCash=false) // added by thettm on 29 jan 2018
        {
            InitializeComponent();
            OpenCode = DocCodeKey;
            isCash = IsCash; // added by thettm on 29 jan 2018
        }//Completed
        public frmARSO(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            InitializeComponent();
            OpenCode = DocCodeKey;
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
        }//Completed
        //public frmARSO(int? SOURCE_DC, int? SOURCE_DK) //commented by KKAung on 30 Aug 2021
        public frmARSO(int? SOURCE_DC, int? SOURCE_DK,Boolean IsCash=false) //added by KKAung on 30 Aug 2021
        {
            //Create Document with data from other source
            InitializeComponent();
            source_DC = SOURCE_DC;
            source_DK = SOURCE_DK;
            OpenCode = GEnum.SystemCode.Sales_Order;
            isCash = IsCash;

        }//Completed

        //Form Events
        private void frm_Load(object sender, EventArgs e)
        {
            webviewOrderTracking.EnsureCoreWebView2Async();
            if (SysOptionUtility.DatabaseBranchCode != DBCode.BHM)
            {
                this.tabDetailList.Tabs["tsbARStatus"].Visible = false;
                this.tabDetailList.Tabs["tsbOrderTracking"].Visible = false;
                tsbSave.Text = "&Post";
                tsbSave.ToolTipText = "Post";
            }

            if (SysOptionUtility.DatabaseBranchCode == DBCode.ADL)
            {
                DefAccKeyLabel.Visible = true;
                DefAccKey.Visible = true;
                btnSetAllDetAcc.Visible = true;
            }
                
            this.Cursor = Cursors.WaitCursor;
            updateEStoreToolStripMenuItem.Visible = false;
            checkSOInEStoreToolStripMenuItem.Visible = false;
            confirmLinkToCustomerToolStripMenuItem.Visible = false;
            //add nnt on 06 Aug 2020
            lblSOStatus.Text = "";

            try
            {
                this.objFactory = new BOLib.ARSOFactory(BOLib.GEnum.InstanceMode.Normal, OpenCode);
                if (objFactory.IsError)
                {
                    formClose = true;
                    return;
                }

                //Attach Event Notifier to Factory
                this.objFactory.ErrorNotifierHeader_Set += new GVar.UINotifierEvent(this.DocNotifier_Set);
                this.objFactory.ErrorNotifierHeader_Clear += new GVar.UINotifierEvent(this.DocNotifier_ClearErr);

                if (this.IsOpenFromAuditLog)
                {
                    if (objFactory.SetReadOnlyData(_dtHeader, _dsDetail) == GVar.gcCancel)
                    {
                        formClose = true;
                        return;
                    }
                }
                else
                {
                    objFactory.New(tagrdDetItms,isCash);

                    if (objFactory.Doc.DocReqDate == null) RequiredTime.Text = "00:00";

                    //Create Document from QO,SO
                    if (source_DK > 0 && !GFunc.IsNEZ(source_DC))
                    {
                        if (objFactory.GetCopy_ByDC(source_DC, source_DK) != GVar.gcPass)
                        {                            
                            formClose = true;
                            return;
                        }
                        //commented by May on 08-Dec-2023.. Not to override sales rep as head sales
                       // objFactory.Doc.DocTranGrpKey = MSTCon.Get(objFactory.Doc.DocConKey).ConChildren;
                        htDetailGrd.Clear();
                        htDetailGrd.Add(GEnum.Details.Doc_Itm, objFactory.DocDetItms);
                        DocHDRUtil.DocDate_CustomUpdate(objFactory.Doc, htDetailGrd);
                        //DocDetUtil.UpdateItmAccFromMaster(objFactory.Doc, tagrdDetItms);//added by Jane 06-Feb-2025

                    }
                }

                //Set FORM and grid binding Source
                Form_Rebind(true, true);
                GlobalUI.FormGrids_Set(this, objFactory.CodeKey, out ContextMenuSetting);
                GridFilter_Set();

                //Set ContextMenu & Grid Setting                           
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(objFactory.CodeKey);

                //Fill the list of all combos in Form and Grid / Clear ErrorProvider
                GlobalUI.Combos_Fill(this, (int)objFactory.Doc.DocCodeKey);
                FilterCustomer(); //added by thettm on 12-sept-2017
                AllDependent_Fill(string.Empty);

                //commented by Jane 06-Mar-2025 - can not set master data to the copied or converted transactions
                //if (source_DK > 0 && !GFunc.IsNEZ(source_DC))
                //{                   
                //    DocDetUtil.UpdateItmAccFromMaster(objFactory.Doc, tagrdDetItms);//added by Jane 06-Feb-2025
                //    UpdateSalesRepFromMaster();
                //}
                this.errorProvider1.Clear();              

                //Form Layout
                if (this.IsOpenFromAuditLog)
                    GlobalUI.FormEnable_Set(this, false);
                else
                {
                    //Hide/Lock Grid columns
                    DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, true,isCash, (DataTable)objFactory.DocDetItms);
                    DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, true);

                    //Attached drag & drop events 
                    this.tagrdDetItms.DragDrop += new System.Windows.Forms.DragEventHandler(GlobalUI.Grid_DragDropDocItm);
                    this.tagrdDetItms.DragOver += new System.Windows.Forms.DragEventHandler(GlobalUI.Grid_DragOver);
                    this.tagrdDetItms.SelectionDrag += new System.ComponentModel.CancelEventHandler(GlobalUI.Grid_SelectionDrag);
                    this.tagrdDetItms.DisplayLayout.Override.SelectTypeRow = SelectType.ExtendedAutoDrag;
                }
                tagrdDetItms.DisplayLayout.Bands[0].Columns["Custom1"].CellActivation = Activation.ActivateOnly;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["Custom2"].Header.Appearance.BackColor = Color.Blue;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["Custom2"].Header.Appearance.BackColor2 = Color.Blue;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["Custom2"].Header.Appearance.ForeColor = Color.LightGreen;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["Custom2"].CellAppearance.FontData.Italic = DefaultableBoolean.False;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["Custom2"].CellAppearance.FontData.Bold = DefaultableBoolean.True;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].Header.Caption = "Latest Cost " + DocCurrKey.Text;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmControlPrice"].Header.Caption = "EStore Price " + DocCurrKey.Text;
                //tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmQtyBalance"].CellAppearance.ForeColor = Color.Red;

                if (isCash)
                {
                    this.Text = "Cash Sales Order";
                    lblHeader.Text = "Cash Sales Order";
                    DocTypeNm.Value = "Cash Sales Order"; //updated by KKAung on 6-May-2022

                    if (this.objFactory.Doc.DocTypeNm != "Cash Sales Order")
                        this.objFactory.Doc.DocTypeNm = "Cash Sales Order"; // added by KKAung on 31 Jul 2023
                    
                    if (GFunc.NEInt(DocConKey.SelectedRow.Cells["CCBType"].Value, 0) == (int)GEnum.CCBType.CH)    //added by KKAung on 13 Jan 2022
                        DocTypeNm.Enabled = false;
                    else
                        DocTypeNm.Enabled = true;       //added by KKAung on 13 Jan 2022   
                    tsbCreatePf.Visible = true;
                    tsbCreateDO.Text = "Create CDO";
                    //groupBox1.Enabled = false; /* commented by YST -- PrintDept should be enable for CSO */
                }
                else
                {
                    this.Text = "Sales Order";
                    
                }
                //Add nnt on 5 Aug 2020

                
                //***if (!isCash) Check(); commented by Jane on 24-Sep-2025

                //btnAttachmentEdit.Enabled = false; Commented by May on 18-05-2023

                if (source_DK > 0 && !GFunc.IsNEZ(source_DC))
                    JobItemsVisibleCheckSet();

                DocDate.Focus();     
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
        private void frm_Shown(object sender, EventArgs e)
        {
            try
            {
                if (formClose)
                {
                    this.Close();
                    this.Dispose();
                }
                else
                {
                    DocDate.Focus();
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
            bool runProcess = false;
            this.formClose = true;

            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }

            if (formClose && (objFactory == null || objFactory.IsError))
                return;

            try
            {
                //When the caller performs this.close, the system actually perform validation on all control automatically
                //if there are any control that fails validation (invalid datatype, the e.cancel is set to true, we have no control over this (not sure if this was done by csla)
                //thus we need to check for e.cancel = true so that we can skip the rest of the codes to prevent error message from appearing twice or more
                if (e.Cancel == true)
                {
                    runProcess = true;
                }
                else
                {
                    if (SaveChanges(true, true, true, GEnum.DocAction.Undetermine) == false)
                    {
                        if (formClose == false)
                        {
                            frmMain.gfrmMain.Tag = GVar.CancelMainFormClosing;
                            e.Cancel = true;
                            return;
                        }
                        else
                            runProcess = true;
                    }
                }

                if (runProcess)
                {
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show("Validation Failed, Close the FORM anyway?",
                                                GEnum.MsgBoxIcon.Question,
                                                GEnum.MsgBoxButton.Yes,
                                                GEnum.MsgBoxButton.No);

                    if (btnSelect == GEnum.MsgBoxButton.No)
                    {
                        //to prohibit closing when error occurs even when the form is closed by main form
                        frmMain.gfrmMain.Tag = GVar.CancelMainFormClosing;
                        e.Cancel = true;
                        formClose = false; //(cancel form closing) if there has data when click save changes after close form 
                        return;
                    }
                    else
                    {
                        IsGridsDirty(true);
                        e.Cancel = false;
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
                if (e.Cancel == false)
                {
                    DocList_Close();
                    objFactory.Dispose();
                }
            }
        }//Completed
        private void frm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)GEnum.SystemCode.Sales_Order);
                    FilterCustomer(); //added by thettm on 12-sept-2017
                    AllDependent_Fill(string.Empty);
                    GlobalUI.RefreshGridDependentText("DocItmKey", string.Empty, "ItmKeySelect", "ItmID", tagrdDetItms);
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

        //Form Display - Controlling and format 
        private void Form_RefreshAll(bool formload, bool clearError)
        {
            //Refresh Data and layout

            try
            {
                Form_Rebind(formload, clearError);
                DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, formload,isCash,(DataTable)objFactory.DocDetItms);
                DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, formload);
                DocPrinted.Enabled = false;

                //added by thettm on 09 jul 2018(start)
                if (!objFactory.Doc.IsNew)
                {
                    DataRow[] linkedrow = ((DataTable)tagrdDetItms.DataSource).Select("ItmQtyLink>0");
                    if (linkedrow.Length > 0)
                        DocDate.Enabled = false;
                    else
                        DocDate.Enabled = true;                    
                }               
                //added by thettm on 09 jul 2018(end)

                foreach (UltraGridRow r in tagrdDetItms.Rows)
                {
                    List<SqlParameter> par = new List<SqlParameter>();
                    par.Add(new SqlParameter("@ItmKey", GFunc.NEInt(r.Cells["ItmKey"].Value, 0)));
                    DataTable dt = GFunc.ExecuteProc("ItemCheckHazardous", par);
                    if (dt.Rows.Count > 0)
                    {
                        r.Cells["ItmID"].Appearance.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                        r.Cells["ItmID"].Appearance.ForeColor = System.Drawing.Color.Black;
                }
                if (objFactory.Doc.DocID.StartsWith("eSO") && objFactory.Doc.DocTypeNm == "eStore SO")
                {
                    tsbEstore.Visible = true;
                    DocQONum.ReadOnly = true;
                    DocRef.ReadOnly = true;
                    DocRemPayment.ReadOnly = true;                   
                }
                else
                {
                    tsbEstore.Visible = false;
                    DocQONum.ReadOnly = false;
                    DocRef.ReadOnly = false;
                    DocRemPayment.ReadOnly = false;                   
                }
                EnableAttachButton();
                if (SysOptionUtility.DatabaseBranchCode != DBCode.BHM ||
                    SysOptionUtility.GetBool("DosCreation") || 
                    DocTypeNm.Text.ToLower().Contains("vn"))
                {
                    //tsbSave.Enabled = true;
                    tsbCreateDO.Enabled = true;
                    tsbCreatePO.Enabled = true;
                    tsbPrint.Enabled = true;
                }

                JobItemsVisibleCheckSet();
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
        //added by thettm on 12-sept-2017 (start)
        private void FilterCustomer()
        {
            if (ConInActive == null)
            {
                if (((DataTable)DocConKey.DataSource).Select("[Inactive]=True").Count() > 0)
                {
                    if(isCash)
                        ConInActive = ((DataTable)DocConKey.DataSource).Select("[Inactive]=True and [CCBType]<>10").AsEnumerable().CopyToDataTable();
                    else
                        ConInActive = ((DataTable)DocConKey.DataSource).Select("[Inactive]=True and [CCBType]<>20").AsEnumerable().CopyToDataTable();

                }
            }

            if (OpenID.Text == "")
            {
                if (isCash)
                    ((DataTable)DocConKey.DataSource).DefaultView.RowFilter = "[Inactive]=False and [CCBType]<>10";
                else
                    ((DataTable)DocConKey.DataSource).DefaultView.RowFilter = "[Inactive]=False and [CCBType]<>20";
                DocConKey.DataSource = ((DataTable)DocConKey.DataSource).DefaultView.ToTable();
                GlobalUI.AddComboEmptyValue(DocConKey, true);
            }
            else if (ConInActive != null)
            {
                if (ConInActive.Select("key=" + DocConKey.Value).Count() > 0 && ((DataTable)DocConKey.DataSource).Select("key=" + DocConKey.Value).Count() == 0)
                {
                    ((DataTable)DocConKey.DataSource).DefaultView.Table.ImportRow(ConInActive.Select("key=" + DocConKey.Value)[0]);
                }                        

            }
        }
        //added by thettm on 12-sept-2017 (end)
        private void Form_Rebind(bool formload, bool clearError)
        {
            try
            {
                FormBindingSource_Set();
                GridItmBindingSource_Set();

                if (formload == false)
                    CombosDependent_Fill(string.Empty);

                if (clearError)
                    this.errorProvider1.Clear();

                EnableAttachButton();
                DocDate.Enabled = objFactory.Doc.IsNew; /* added by YST on 2022/06/07 to allow to change date after Copy  */


                //to get the count of header's attachment file to show on btnAttachmentEdit.
                btnAttachmentEdit.Text = "Customer PO (" + objFactory.Doc.Attachments.Count(o => o.DocDItm == -1 && o.DocDetailType == 1) + ")";
                
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].Header.Caption = "Latest Cost " + DocCurrKey.Text;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmControlPrice"].Header.Caption = "EStore Price " + DocCurrKey.Text;
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
        private void FormBindingSource_Set()
        {
            try
            {
                bdsDocumentBindingSource.DataSource = objFactory.Doc;
                bdsDocumentBindingSource.AllowNew = true;
                bdsDocumentBindingSource.ResetBindings(false);
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

        private void EnableAttachButton()
        {
            //if (objFactory.Doc.DocState != (int)GEnum.DocState.Posted && !objFactory.Doc.IsNew)
            //    btnAttachmentEdit.Enabled = true;
            //else
            //    btnAttachmentEdit.Enabled = false;
            btnAttachmentEdit.Enabled = objFactory.Doc.DocKey != 0;
        }
        private void GridItmBindingSource_Set()
        {
            try
            {
                tagrdDetItms.DataSource = objFactory.DocDetItms;
                tagrdDetItms.Rows.Refresh(RefreshRow.ReloadData);
                GlobalUI.GridSequenceSort(objFactory.Doc.DocCodeKey, tagrdDetItms);
                GridFilter_Set();//Check mic
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
        private void GridFilter_Set()
        {
            try
            {
                //Filter DocDetItm
                //GridFilterToDefaultView   
                ((DataTable)tagrdDetItms.DataSource).DefaultView.RowFilter = "LineType=1000";
                ((DataTable)tagrdDetItms.DataSource).DefaultView.Sort = "ItmSN";
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
        private void GridCellLock_Set()
        {
            try
            {
                if (GFunc.IsNE(tagrdDetItms.ActiveRow) == false)
                {
                    UltraGridRow row = tagrdDetItms.ActiveRow;
                    if (!GFunc.IsNEZ(row.Cells["ItmBatchKey"].Value))
                    {
                        foreach (UltraGridCell cell in row.Cells)
                        {
                            if (!cell.Column.Key.ToLower().Equals("itmrem") && !cell.Column.Key.ToLower().Equals("custom2") && !cell.Column.Key.ToLower().Equals("itmqty")
                                && !cell.Column.Key.ToLower().Equals("itmmark") && !cell.Column.Key.ToLower().Equals("appoid"))
                                cell.Column.CellActivation = Activation.ActivateOnly;
                            else
                                cell.Column.CellActivation = Activation.AllowEdit;
                        }

                        if (GFunc.NEInt(row.Cells["ItmBatchKey"].Value, 0) != 9999)
                        {
                            row.Cells["ItmDes"].Column.CellActivation = Activation.ActivateOnly;
                            row.Cells["ItmPriceAfter"].Column.CellActivation = Activation.ActivateOnly;
                        }
                        else
                        {
                            row.Cells["ItmDes"].Column.CellActivation = Activation.AllowEdit;
                            row.Cells["ItmPriceAfter"].Column.CellActivation = Activation.AllowEdit;
                        }
                    }
                    else
                        DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, false);
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
        private void AllDependent_Fill(string controlNm)
        {
            try
            {
                DocTermKey.Enabled = objFactory.Doc.DocConID.ToUpper().Contains("ONE-TIME CUSTOMER"); /* added by YST becaue some customers pay TTPayment & want to show in printed invoice */
                CombosDependent_Fill(controlNm);
                TextDependent_Fill(controlNm);
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
        private void TextDependent_Fill(string controlNm)
        {
            //If controlNm is Empty, it will refresh all control, else it will only refresh that control only
            //retain the factory isdirty state as we do not want to change due to propertychange event

            try
            {
                bool FactoryIsDirty = objFactory.Doc.IsDirty;

                #region DocAccID
                if (GFunc.CompareString(controlNm, "DocAccKey") || controlNm == string.Empty)
                {
                    if (GFunc.IsNE(objFactory.Doc.DocAccKey) == false)
                    {
                        MSTAcc acc = MSTAcc.Get(objFactory.Doc.DocAccKey);
                        objFactory.Doc.DocAccID = acc.AccID;
                        objFactory.Doc.DocAccDes = acc.AccDes;
                        acc = null;
                    }
                    else
                    {
                        objFactory.Doc.DocAccID = string.Empty;
                        objFactory.Doc.DocAccDes = string.Empty;
                    }
                }
                #endregion

                objFactory.Doc.IsDirty = FactoryIsDirty;
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
        private void CombosDependent_Fill(string controlNm)
        {
            try
            {
                if (controlNm == "DocShipName" || controlNm == string.Empty)
                    GlobalUI.BindComboValue((TAUtil.TAComboBox)DocShipName, GVar.ListSettingID.MSTShipNameByConKey + "%" + GFunc.NEInt(objFactory.Doc.DocConKey, 0));

                if (controlNm == "DefBAddrKey" || controlNm == string.Empty)
                    GlobalUI.BindComboValue((TAUtil.TAComboBox)DefBAddrKey, GVar.ListSettingID.REFAddrByCon + "%" + GFunc.NEInt(objFactory.Doc.DocConKey, 0));

                if (controlNm == "DefSAddrKey" || controlNm == string.Empty)
                    GlobalUI.BindComboValue((TAUtil.TAComboBox)DefSAddrKey, GVar.ListSettingID.REFAddrByCon + "%" + GFunc.NEInt(objFactory.Doc.DocConKey, 0));

                //if (controlNm == "DefJobKey" || controlNm == string.Empty)
                //    GlobalUI.BindComboValue((TAUtil.TAComboBox)DefJobKey, GVar.ListSettingID.MSTJobSalesByConKey + "%" + GFunc.NEInt(objFactory.Doc.DocConKey, 0) + "%" + AppInfor.JobAccessLevel.ToString() + "%" + AppInfor.CurrentUserKey.ToString());

                //if (controlNm == "ItmJobKey" || controlNm == string.Empty)
                //    GlobalUI.BindComboValue((TAUtil.TAComboBox)tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmJobKey"].EditorComponent, GVar.ListSettingID.MSTJobSalesByConKey + "%" + GFunc.NEInt(objFactory.Doc.DocConKey, 0) + "%" + AppInfor.JobAccessLevel.ToString() + "%" + AppInfor.CurrentUserKey.ToString());

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

        //Menu Strip Event
        private void tsbNew_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (SaveChanges(true, true, false, GEnum.DocAction.Undetermine) == false)
                    return;

                //Prepare new instance
                if (objFactory.New(tagrdDetItms, isCash) == GVar.gcPass)
                {
                    if (objFactory.Doc.DocReqDate == null) RequiredTime.Text = "00:00";
                    pRow = null;
                    DocDate.Focus();
                }

                Form_RefreshAll(false, true);
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
        private void tsbClear_Click(object sender, EventArgs e)
        {
            GEnum.MsgBoxButton btnSelect = GEnum.MsgBoxButton.Clear;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (form_CanValidate() == false)
                    return;

                if (this.objFactory.Doc.IsDirty && this.objFactory.Doc.IsNew)
                {
                    if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnClearRecord))
                    {
                        btnSelect = MsgBox.Show(MsgID.Common.ConfirmClear,
                                              GEnum.MsgBoxIcon.Question,
                                              GEnum.MsgBoxButton.Clear,
                                              GEnum.MsgBoxButton.Dont_Clear,
                                              GEnum.MsgBoxButton.I_Dont_Know);
                    }
                }
                else
                    return;

                if (btnSelect == GEnum.MsgBoxButton.Clear)
                {
                    IsGridsDirty(true);

                    if (objFactory.New(tagrdDetItms) == GVar.gcPass)
                        DocDate.Focus();

                    Form_RefreshAll(false, true);
                    pRow = null;
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
        private void tsbDraft_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                SaveChanges(false, false, false, GEnum.DocAction.Save);
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
                //added by nnt on 5 Aug 2020
                if(!isCash) Check();
                this.Cursor = Cursors.Default;
            }
        }//Completed

        private bool CheckCusID()
        {
            if (isCash) return false;

            //if (SysOptionUtility.DatabaseBranchCode != DBCode.BHM) return false;
            if (SysOptionUtility.DatabaseBranchCode != DBCode.BHM && SysOptionUtility.DatabaseBranchCode != DBCode.ADL
      && SysOptionUtility.DatabaseBranchCode != DBCode.SOP && SysOptionUtility.DatabaseBranchCode != DBCode.BOS)
                return false;

            bool OrangeCus = false;
                try
                {
                    string proc = "Get_CusStatusSO";
                    string strDocConID = this.DocConKey.Text;
                    if (strDocConID != null || strDocConID != "")
                    {
                        List<SqlParameter> parList = new List<SqlParameter>();
                        parList.Add(new SqlParameter("@ConID", strDocConID));
                        DataTable dt = GFunc.ExecuteProcReader(proc, parList);
                        if (dt.Rows.Count > 0)
                        {
                            OrangeCus = GFunc.NEBool(dt.Rows[0][0], false);

                        }

                    }
                    return OrangeCus;
                }
                catch (Exception ex)
                {
                    return false;
                }

            

        }
        private void tsbSave_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                SaveChanges(false, false, false, GEnum.DocAction.Post);
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
                if (!isCash) Check();
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (objFactory.Delete(tagrdDetItms) == GVar.gcPass)
                    DocDate.Focus();
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
                Form_RefreshAll(false, true);
                DocList_Refresh();
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void tsbList_Click(object sender, EventArgs e)
        {
            try
            {
                if (GFunc.IsNE(DocListForm))
                {
                    DocListForm = new frmDocList((int)objFactory.Doc.DocCodeKey,isCash);

                    //Attach events to this FORM to call events in DocList
                    this.ListEvent_CloseFORM += new GVar.ListEvent_CloseFORM(DocListForm.OnDoc_Close);
                    this.ListEvent_RefreshRecord += new GVar.ListEvent_RefreshRecord(DocListForm.OnDoc_Changed);

                    //Attach events to DocList to call events in this FORM
                    DocListForm.ListEvent_DeleteRecord = new GVar.ListEvent_DeleteRecord(this.OnDocList_DeleteRecord);
                    DocListForm.ListEvent_OpenRecord = new GVar.ListEvent_OpenRecord(this.OnDocList_OpenRecord);
                    DocListForm.ListEvent_CloseFORM = new GVar.ListEvent_CloseFORM(this.OnDocList_FormClose);
                    DocListForm.MdiParent = frmMain.gfrmMain;
                    DocListForm.Show();
                }
                else
                    DocListForm.Focus();
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
            this.Cursor = Cursors.WaitCursor;
            

                try
            {
                if (SaveChanges(false, true, true, GEnum.DocAction.Print) == false)
                    return;

                if (objFactory.Doc.DocState == (int)GEnum.DocState.New)
                {
                    MsgBox.Show("Cannot print an empty document");
                    return;
                }
                else
                {
                    Document NewDoc = objFactory.Doc;
                    frmPrintSelection f = new frmPrintSelection(ref NewDoc, (int)objFactory.Doc.DocCodeKey);
                    if (objFactory.Doc.DocPrinted == false)
                        f.DocPrinted += new GVar.DocPrintUpdateEvent(this.OnDocPrinted);
                    if (f.ShowDialog() == DialogResult.OK)
                        frmMain.gfrmMain.ExistingPrintOutForm( (int)objFactory.Doc.DocCodeKey, objFactory.Doc.DocKey.Value);//to activate the Report Form
                    else
                        this.Focus();
                    f.Close();
                   
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
        private void tsbClose_Click(object sender, EventArgs e)
        {
            try
            {
                formClose = true;
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
        private void tsbCopy_Click(object sender, EventArgs e)
        {
            try
            {
                frmDocCopy copy = new frmDocCopy((int)objFactory.Doc.DocKey, (int)objFactory.Doc.DocCodeKey, this.tagrdDetItms);
                copy.CopyRecordEvent += new GVar.ListEvent_CopyRecord(this.OnDocCopy_CopyRecord);
                copy.ShowDialog();
                AddItm_Hash();
                DocComUtility.CalForm(objFactory.Doc, htDetailGrd, true, false);
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
                //if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM || SysOptionUtility.DatabaseBranchCode == DBCode.ADL
               || SysOptionUtility.DatabaseBranchCode == DBCode.SOP || SysOptionUtility.DatabaseBranchCode == DBCode.BOS)
                {
                   
                    CheckOrange();
                }

            }
        }//Completed
        private void tsbCreateDO_Click(object sender, EventArgs e)
        {
            try
            {               

                if (SECPermUtility.Add(objFactory.PermID, true))
                    CreateDocs((int)GEnum.SystemCode.Delivery_Order);

                this.Cursor = Cursors.Default;


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
        private void tsbCreateIV_Click(object sender, EventArgs e)
        {
            try
            {
                CreateDocs((int)GEnum.SystemCode.Sales_Invoice);
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
        private void tsbCreatePO_Click(object sender, EventArgs e)
        {
            try
            {
                CreateDocs((int)GEnum.SystemCode.Purchase_Order);
               
               
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
        private void tsbDocRelationShip_Click(object sender, EventArgs e)
        {
            try
            {
                if (objFactory.Doc.DocState == (int?)GEnum.DocState.Posted || objFactory.Doc.DocState == (int?)GEnum.DocState.Draft)
                {
                    GlobalUI.PopupDisplay("frmDocRelationship", (GEnum.SystemCode)objFactory.Doc.DocCodeKey, objFactory.Doc.DocKey, objFactory.Doc.DocID); 
                    
                }
                else
                {
                    MsgBox.Show("Relationship will be show only for the posted/draft documents.");
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
        private void tsbMarkUp_Click(object sender, EventArgs e)
        {
            try
            {
                frmSpecialCalculation specCal = new frmSpecialCalculation(objFactory.Doc, GEnum.SpecialCalculationType.Sale, GEnum.SpecialCalculationProcessType.PriceMarkup, tagrdDetItms);
                specCal.ShowDialog();
                AddItm_Hash();
                DocComUtility.CalForm(objFactory.Doc, htDetailGrd, true, false);

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
        private void btnAttachmentEdit_Click(object sender, EventArgs e)
        {
            try
            {
                bool dirty = objFactory.Doc.IsDirty;

                frmAttachment f = new frmAttachment(objFactory.Doc.Attachments, objFactory.Doc, 1);
                f.ShowDialog(this);

                if (f.DialogResult == DialogResult.Yes)
                {
                    if (objFactory.Doc.Attachment != true)//To prevent dirty  
                    {
                        objFactory.Doc.Attachment = true;
                        Attachment.Checked = true;
                    }
                }
                else if (objFactory.Doc.Attachment != false)//To prevent dirty
                {
                    objFactory.Doc.Attachment = false;
                    Attachment.Checked = false;
                }

                //filtering to get the count of header's attachment file to show on btnAttachmentEdit.          
                btnAttachmentEdit.Text = "Customer PO (" + objFactory.Doc.Attachments.Count(o => o.DocDItm == -1 && o.DocDetailType == 1) + ")";

                if (objFactory.Doc.IsDirty && SysOptionUtility.HasDMASLink) //If linked to DMAS, the attachments are already saved. If not, dirty state should not be restored back
                    objFactory.Doc.IsDirty = dirty;

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
        private void btnItmMarkReSequence_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DocDetUtil.ItmMark_ReSequence(objFactory.Doc, tagrdDetItms);
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
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                SaveChanges(false, false, false, GEnum.DocAction.Submit);
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
        private void btnApprove_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                SaveChanges(false, false, false, GEnum.DocAction.Post);
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
        private void btnReject_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                SaveChanges(false, false, false, GEnum.DocAction.Reject);
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
        private void btnSetAllDetJob_Click(object sender, EventArgs e)
        {
            //Set All Row Job Default
            try
            {
                int defJobKey = GFunc.NEInt(this.DefJobKey.Value, 0);
                if (SysOptionUtility.DatabaseBranchCode != DBCode.BHM)
                {
                    AddItm_Hash();
                    if (DocHDRUtil.DefJob_CustomUpdate(objFactory.Doc, htDetailGrd, defJobKey, true))
                        objFactory.Doc.IsDirty = true;
                }
                else
                {
                    UltraGridRow srow = DefJobKey.SelectedRow;
                    if (srow != null)
                    {
                        if (GFunc.NEStr(srow.Cells["JobClass"].Value, "").ToLower().Contains("exclusive"))
                            ExclusiveSaleJob = true;
                    }

                    DataTable dt = tagrdDetItms.DataSource as DataTable;
                    if (dt.AsEnumerable().Any(r => r.Field<int>("ItmJobKey") == defJobKey))
                    {
                        GEnum.MsgBoxButton act = MsgBox.Show("Item lines already exist for this job. Would you like to append the new lines or replace the existing ones?", GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Append_Job
                            , GEnum.MsgBoxButton.Replace_Job);
                        if (act == GEnum.MsgBoxButton.Replace_Job)
                        {

                            DataRow[] rows = dt.Select("ItmJobKey=" + defJobKey);

                            foreach (DataRow row in rows)
                            {
                                dt.Rows.Remove(row);
                            }
                            dt.AcceptChanges();
                            dt.DefaultView.RowFilter = "";
                            tagrdDetItms.DataSource = dt;
                        }
                        else if (act != GEnum.MsgBoxButton.Append_Job)
                            return;
                    }
                    else
                        dt.DefaultView.RowFilter = "";

                    dtJobEst = MSTJobDetEsts.Get(defJobKey);
                    if (dtJobEst.Rows.Count > 0)
                    {
                        int i = 0;
                        decimal NSum = dtJobEst.AsEnumerable().Sum(r => r.Field<decimal>("PrjCost"));
                        UltraGridRow row = tagrdDetItms.DisplayLayout.Bands[0].AddNew();
                        int ItmKey = SysOptionUtility.GetSysOpInt("JobEstimateCombineCostItem");
                        row.Cells["ItmKey"].Value = ItmKey;

                        AddItm_Hash();
                        //Could not use DataTable InsertAt function because want to call this function. If not, need to rewrite this function
                        DocDetUtil.ItmID_Update(objFactory.Doc, htDetailGrd, ItmKey);

                        if (DefJobKey.SelectedRow != null)
                            row.Cells["ItmDes"].Value = DefJobKey.SelectedRow.Cells["Des"].Value;
                        row.Cells["ItmKeySelect"].Value = ItmKey;


                        row.Cells["ItmQty"].Value = 1;
                        row.Cells["ItmVendorPrice"].Value = NSum;
                        row.Cells["ItmJobKey"].Value = defJobKey;

                        row.Cells["ItmBatchKey"].Value = 9999;//to not allow deleting the line

                        row.Update();

                        foreach (DataRow dr in dtJobEst.Rows)
                        {
                            bool selectedRow = GFunc.NEBool(dr["Selected"], false);
                            int ParentDocItmKey = 0;
                            ItmKey = GFunc.NEInt(dr["EstItmKey"], 0);

                            if (ItmKey == 0 || GFunc.NEStr(dr["EstItmID"], "") == "")
                            {
                                if (GFunc.NEInt(dr["EstQty"], 0) == 0)
                                    ItmKey = 3652;
                                else
                                    continue;
                            }
                            row = tagrdDetItms.DisplayLayout.Bands[0].AddNew();

                            row.Cells["ItmKey"].Value = ItmKey;

                            row.Cells["ItmID"].Value = dr["EstItmID"];
                            AddItm_Hash();

                            row.Cells["ItmJobKey"].Value = defJobKey;
                            DocDetUtil.ItmID_Update(objFactory.Doc, htDetailGrd, ItmKey, 2);


                            row.Cells["ItmKeySelect"].Value = dr["EstItmKeySelect"];
                            row.Cells["ItmDes"].Value = dr["EstItmDes"];
                            row.Cells["ItmType"].Value = dr["EstItmType"];
                            row.Cells["ItmQty"].Value = dr["EstQty"];
                            row.Cells["ItmUOMKey"].Value = dr["EstUOMKey"];

                            row.Cells["ItmBatchKey"].Value = GFunc.NEInt(dr["JobEstKey"], 0);//Job Line Key
                            row.Cells["ItmRem"].Value = dr["EstItmRem"];
                            row.Cells["ItmHide"].Value = !selectedRow;

                            if (GFunc.NEInt(dr["EstItmType"], 0) == (int)GEnum.ItemType.Assembly)
                            {
                                ParentDocItmKey = GFunc.NEInt(row.Cells["DocItmKey"].Value, 0);
                            }

                            row.Update();
                        }

                        tagrdDetItms.Rows.Refresh(RefreshRow.ReloadData);
                        objFactory.DocDetItms = (ARSODetItms)tagrdDetItms.DataSource;


                        AddItm_Hash();
                        DocHDRUtil.DocDate_CustomUpdate(objFactory.Doc, htDetailGrd);
                    }
                    tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmJobKey"].DefaultCellValue = defJobKey;
                    tagrdDetItms.Update();
                    GridCellLock_Set();
                    GridFilter_Set();
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
        private void btnSetAllDetAcc_Click(object sender, EventArgs e)
        {
            //Set All Row AccKey Default
            try
            {
                int defAccKey = GFunc.NEInt(this.DefAccKey.Value, 0);
                string defAccDes = this.DefAccKey.Text;
                AddItm_Hash();
                if (DocHDRUtil.DefAcc_CustomUpdate(objFactory.Doc, htDetailGrd, defAccKey, defAccDes, true))
                    objFactory.Doc.IsDirty = true;
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
        private void btnPnL_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges(true, true, true, GEnum.DocAction.Undetermine) == false)
                    throw new Exception(); 

                GlobalUI.PrintDocumentProfitAndLost((int)GEnum.SystemCode.Sales_Order , (int)objFactory.Doc.DocKey);
            }
            catch (Exception ex)
            {
            }
        }

        //Event invoke by or invoke to the document list FORM
        public void OnDocList_OpenRecord(int key)
        {
            try
            {
                if (this.OpenRecord(key, string.Empty))
                    this.Focus();
                else if (DocListForm != null)
                    DocListForm.Focus();
                //added by nnt on 05 Aug 2020
             if(!isCash)   Check();

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
                //if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                //{
                //    if (Convert.ToInt32(DocHome.Text.ToString()) > 0)
                //    {
                //        if (CheckCusID())
                //        {
                //            RequestingApproval = CheckApproval();
                //            if (DState != 100)
                //            {
                //                if (RequestingApproval == 2) { tsbSave.Enabled = false; tsbCreatePO.Enabled = false; MsgBox.Show("Release button is disable now because this customer is orange flag and need approval from COO for this SO."); }
                //                if (RequestingApproval == 1) { tsbSave.Enabled = false; tsbCreatePO.Enabled = false; tsbDraft.Enabled = false; MsgBox.Show("Release button is disable now because still requesting approval from COO for this SO."); }
                //                if (RequestingApproval == 0) { MsgBox.Show("This SO has been approved and can be released now."); }
                //            }

                //        }
                //    }

                //}
            }
        }//Completed              
        private void OnDocList_DeleteRecord(int key)
        {
            ARSOFactory objFactoryTmp = new ARSOFactory(GEnum.InstanceMode.Normal, OpenCode);
            try
            {
                if (objFactoryTmp.GetReadOnly(key, string.Empty) == GVar.gcPass)
                {
                    if (objFactoryTmp.Doc.DocPrinted && SECPermUtility.Perform(GVar.PermissionID.Save_Printed_Sales_Order, true) == false)
                        return;
                    objFactoryTmp.Doc.IsReadOnly = false;
                    objFactoryTmp.Delete();
                }
                return;
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
                objFactoryTmp.Dispose();
            }
        }//Completed
        private void OnDocList_FormClose()
        {
            this.ListEvent_CloseFORM = null;
            this.ListEvent_RefreshRecord = null;
            DocListForm = null;
        }//Completed
        private void DocList_Refresh()
        {
            try
            {
                if (!GFunc.IsNE(this.ListEvent_RefreshRecord))
                    ListEvent_RefreshRecord.Invoke();
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
        private void DocList_Close()
        {
            try
            {
                if (!GFunc.IsNE(this.ListEvent_CloseFORM))
                    ListEvent_CloseFORM.Invoke();
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
        private void OnDocCopy_CopyRecord(GEnum.CopyOption copyOption, int CopyDocCodeKey, int CopyDocKey, DataTable dt, bool NSLink)
        {
            try
            {
                htDetailGrd.Clear();
                if (!GVar.DocUpdateOption.ContainsKey(GVar.DeptUpdateOption))
                    GVar.DocUpdateOption.Add(GVar.DeptUpdateOption, true);

                switch (copyOption)
                {
                    case GEnum.CopyOption.CopyFrom:
                        if (SaveChanges(true, true, true, GEnum.DocAction.Undetermine) == false)
                            return;

                        DataTable dtDetail = new DataTable();
                        
                        
                        objFactory.CopyFrom((GEnum.SystemCode)CopyDocCodeKey, CopyDocKey, this.tagrdDetItms, NSLink, out dtDetail,isCash);

                        this.Form_Rebind(false, true);

                        DocHDRUtil.DocTransferData(CopyDocCodeKey, CopyDocKey, (int)objFactory.Doc.DocConKey, dtDetail, objFactory.Doc, tagrdDetItms, 0, "", false, NSLink);

                        AddItm_Hash();
                      
                        if (DocHDRUtil.DocDate_CustomUpdate(objFactory.Doc, htDetailGrd) == false)
                            MsgBox.Show("Unable to calculate document");
                       
                        if (MSTSalesRep.Get(GFunc.NEInt(objFactory.Doc.DocEmKey, 0)).Inactive.Value)
                        {
                            MsgBox.Show("The sale representative is inactive. Please select another one.");
                            
                        }

                        if (objFactory.Doc.DocReqDate == null) RequiredTime.Text = "00:00";

                        objFactory.Doc.DocTranGrpKey = MSTCon.Get(objFactory.Doc.DocConKey).ConChildren;

                        //commented by Jane 06-Mar-2025 - can not set master data to the copied or converted transactions
                        //DocDetUtil.UpdateItmAccFromMaster(objFactory.Doc, tagrdDetItms);//added by Jane 06-Feb-2025
                        //UpdateSalesRepFromMaster();

                        /* added by MayTS */
                        if (DefJobKey.DataSource != null)
                        {
                            DataRow[] drs = objFactory.DocDetItms.Select("ItmBatchKey<>0");

                            if (DefJobKey.Rows.Count > 0 && drs.Length > 0)
                            {
                                UltraGridRow row = DefJobKey.Rows
                                              .FirstOrDefault(r => GFunc.NEInt(r.Cells["Key"].Value, 0) == GFunc.NEInt(drs[0]["ItmJobKey"], 0));

                                if (row != null)
                                {
                                    objFactory.Doc.DefJobKey = GFunc.NEInt(drs[0]["ItmJobKey"], 0);
                                    DefJobKey.Value = GFunc.NEInt(drs[0]["ItmJobKey"], 0);
                                    DefJobKey.SelectedRow = row;
                                }
                            }
                        }

                        DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false,isCash,(DataTable)objFactory.DocDetItms);//check mic /Pauk change formload=true to false
                        DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, false);//check mic /Pauk change formload=true to false
                        break;

                    case GEnum.CopyOption.Import:
                        Form_Rebind(false, true);
                        DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false,isCash, (DataTable)objFactory.DocDetItms);//check mic /Pauk change formload=true to false
                        DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, false);//check mic /Pauk change formload=true to false
                        break;

                    case GEnum.CopyOption.CopyMySelf:
                        objFactory.CopyMyself();
                        Form_Rebind(false, true);
                        AddItm_Hash();                        

                        if (DocHDRUtil.DocDate_CustomUpdate(objFactory.Doc, htDetailGrd) == false)
                            MsgBox.Show("Unable to calculate document");

                        if (MSTSalesRep.Get(GFunc.NEInt(objFactory.Doc.DocEmKey, 0)).Inactive.Value)
                        {
                            MsgBox.Show("The sale representative is inactive. Please select another one.");
                            
                        }

                        if (objFactory.Doc.DocReqDate == null) RequiredTime.Text = "00:00";

                        if (DefJobKey.DataSource != null)
                        {
                            DataRow[] drs = objFactory.DocDetItms.Select("ItmBatchKey<>0");

                            if (DefJobKey.Rows.Count > 0 && drs.Length > 0)
                            {
                                UltraGridRow row = DefJobKey.Rows
                                              .FirstOrDefault(r => GFunc.NEInt(r.Cells["Key"].Value, 0) == GFunc.NEInt(drs[0]["ItmJobKey"], 0));

                                if (row != null)
                                {
                                    objFactory.Doc.DefJobKey = GFunc.NEInt(drs[0]["ItmJobKey"], 0);
                                    DefJobKey.Value = GFunc.NEInt(drs[0]["ItmJobKey"], 0);
                                    DefJobKey.SelectedRow = row;
                                }
                            }
                        }

                        objFactory.Doc.DocTranGrpKey = MSTCon.Get(objFactory.Doc.DocConKey).ConChildren;

                        //commented by Jane 06-Mar-2025 - can not set master data to the copied or converted transactions
                        //DocDetUtil.UpdateItmAccFromMaster(objFactory.Doc, tagrdDetItms);//added by Jane 06-Feb-2025
                        //UpdateSalesRepFromMaster();

                        DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false,isCash, (DataTable)objFactory.DocDetItms);
                        DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, false);
                        break;
                }

                //*** added by jane on 19-Nov-2025
                if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM || SysOptionUtility.DatabaseBranchCode == DBCode.ADL 
                || SysOptionUtility.DatabaseBranchCode == DBCode.SOP || SysOptionUtility.DatabaseBranchCode == DBCode.BOS)
                {
                    if (!isCash) CheckOrange();
                }
                //

                EnableAttachButton();
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
                GVar.DocUpdateOption.Remove(GVar.DeptUpdateOption);
            }
        }//Completed
        private void OnDocPrinted()
        {
            //Check if user has permission to edit the already printed document
            this.Focus();
            if (SECPermUtility.Perform(GVar.PermissionID.Save_Printed_Sales_Order, false) == false)
            {
                objFactory.MarkAsReadOnly();
                Form_RefreshAll(false, true);//To set form in ReadOnly state                    
                MsgBox.Show(MsgID.Permission.PermPerformIsFalse);
            }
            else
            {
                FormBindingSource_Set();//to refresh the print state
            }
            DocList_Refresh();
        }

        //Controls Events
        private void Combo_NotInListAdd(object sender, ValidationErrorEventArgs e)
        {
            try
            {
                GlobalUI.dependenFillEvent = null;
                string ctrlNm = (sender as Control).Name;
                switch (ctrlNm)
                {
                    case "DocShipName":
                    case "DefBAddrKey":
                    case "DefSAddrKey":
                    case "DefJobKey":
                    case "ItmJobKey":
                        GlobalUI.dependenFillEvent += new GlobalUI.DependentFillEvent(CombosDependent_Fill);
                        break;
                }
                GlobalUI.ItemNotInList(sender as TAUtil.TAComboBox, e, true, 0); /* Qick Add => true */
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
                GlobalUI.dependenFillEvent = null;
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
        private void OpenID_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                //commented by thettm on 11-jun-2018
                //if (GFunc.IsNE(OpenID.Text) == false) 
                //  OpenRecord(0, OpenID.Text);

                //added by thettm on 11-jun-2018(start)
                if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                {
                    if (GFunc.IsNE(OpenID.Text) == false && isCash == true && OpenID.Text.ToString().ToUpper().StartsWith("CSO"))
                        OpenRecord(0, OpenID.Text);
                    else if (GFunc.IsNE(OpenID.Text) == false && isCash == false && !OpenID.Text.ToString().ToUpper().StartsWith("CSO"))
                        OpenRecord(0, OpenID.Text);
                    else if (GFunc.IsNE(OpenID.Text) == false && OpenID.Text.ToString().ToUpper().StartsWith("CSO"))
                        MessageBox.Show("Your Sale Order is Cash Sale Order!");
                    else if(GFunc.IsNE(OpenID.Text) == false)
                        MessageBox.Show("Your Sale Order is Not Cash Sale Order!");
                }
                else if (GFunc.IsNE(OpenID.Text) == false)
                    OpenRecord(0, OpenID.Text);
                //added by thettm on 11-jun-2018(end)
                //added by nnt on 05 Aug 2020
               if(!isCash) Check();
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
        private void OpenID_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            try
            {
                frmDocSearch fpopup = new frmDocSearch((int)objFactory.Doc.DocCodeKey);
                fpopup.ListEvent_OpenRecord = new GVar.ListEvent_OpenRecord(this.OnDocList_OpenRecord);
                fpopup.ShowDialog();
                if (fpopup.DialogResult == DialogResult.OK)
                    OpenID.Text = fpopup.DocID;
                
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
        private void BranchKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                GFunc.NE(BranchKey, 0);
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
        private void DocAccKey_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            try
            {
                AddItm_Hash();
                DocHDRUtil.DocAccID_btnClick(this, objFactory.Doc, htDetailGrd, DocAccKey, GEnum.PopupType.AccID, ContextMenuSetting, objFactory.PermID);
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
        private void DocAccKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                AddItm_Hash();
                if (DocHDRUtil.DocAccID_CustomUpdate(this, objFactory.Doc, htDetailGrd, DocAccKey, GEnum.RecAccessType.AccID, ContextMenuSetting, objFactory.PermID) == false)
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
        }//Completed
        private void DocAccDes_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            try
            {
                AddItm_Hash();
                DocHDRUtil.DocAccID_btnClick(this, objFactory.Doc, htDetailGrd, DocAccDes, GEnum.PopupType.AccDes, ContextMenuSetting, objFactory.PermID);
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
        private void DocAccDes_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                AddItm_Hash();
                if (DocHDRUtil.DocAccID_CustomUpdate(this, objFactory.Doc, htDetailGrd, DocAccDes, GEnum.RecAccessType.AccDes, ContextMenuSetting, objFactory.PermID) == false)
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
        }//Completed
        private void DocConKey_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            try
            {
                AddItm_Hash();
                DocHDRUtil.DocConID_btnClick(this, objFactory.Doc, htDetailGrd, DocConKey, GEnum.PopupType.CusID, ContextMenuSetting, objFactory.PermID);
                DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false,isCash,(DataTable)objFactory.DocDetItms);
                DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, false);
                AllDependent_Fill(string.Empty);
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
        private void DocConKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                //added by nnt on 05 Aug 2020
                txtCrSO.Text = "";
                lblSOStatus.Text = "";
                DocConKey.Appearance.BackColor = System.Drawing.Color.White;
                DocConNm.Appearance.BackColor = System.Drawing.Color.White;

                //end added by nnt


                AddItm_Hash();
                if (DocHDRUtil.DocConID_CustomUpdate(this, objFactory.Doc, htDetailGrd, DocConKey, GEnum.RecAccessType.CustID, ContextMenuSetting, objFactory.PermID) == false)
                    e.Cancel = true;
                
                //commented and added by thettm on 08 jun 2018
                //DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false);
                DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false,isCash,(DataTable)objFactory.DocDetItms);

                DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, false);
                AllDependent_Fill(string.Empty);
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].Header.Caption = "Latest Cost " + DocCurrKey.Text;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmControlPrice"].Header.Caption = "EStore Price " + DocCurrKey.Text;
                if (pRow != null) CalCulateProcessFee();
                //if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM || SysOptionUtility.DatabaseBranchCode == DBCode.ADL
               || SysOptionUtility.DatabaseBranchCode == DBCode.SOP || SysOptionUtility.DatabaseBranchCode == DBCode.BOS)
                {

                   if(!isCash) CheckOrange();
                }
                EnableAttachButton();
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
        private void DocConNm_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            try
            {
                AddItm_Hash();
                DocHDRUtil.DocConID_btnClick(this, objFactory.Doc, htDetailGrd, DocConNm, GEnum.PopupType.CusNm, ContextMenuSetting, objFactory.PermID);
                DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false,isCash, (DataTable)objFactory.DocDetItms);
                DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, false);
                AllDependent_Fill(string.Empty);

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
        private void DocConNm_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (!GFunc.IsNEZ(objFactory.Doc.DocConKey))
                    return;

                AddItm_Hash();
                if (DocHDRUtil.DocConID_CustomUpdate(this, objFactory.Doc, htDetailGrd, DocConNm, GEnum.RecAccessType.CustNm, ContextMenuSetting, objFactory.PermID) == false)
                    e.Cancel = true;

                DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false,isCash, (DataTable)objFactory.DocDetItms);
                DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, false);
                AllDependent_Fill(string.Empty);
                EnableAttachButton();
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
        private void DocCurrKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                AddItm_Hash();
                if (DocHDRUtil.DocCurrKey_CustomUpdate(objFactory.Doc, htDetailGrd) == false)
                {
                    e.Cancel = true;
                    return;
                }
                DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false,isCash, (DataTable)objFactory.DocDetItms);
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].Header.Caption = "Latest Cost " + DocCurrKey.Text;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmControlPrice"].Header.Caption = "EStore Price " + DocCurrKey.Text;
                EnableAttachButton();
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
        private void DocCurrRate_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                AddItm_Hash();
                if (DocHDRUtil.DocCurrRate_CustomUpdate(objFactory.Doc, htDetailGrd) == false)
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
        }//Completed
        private void DocCountryRate_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                AddItm_Hash();
                if (DocHDRUtil.DocCountryRate_CustomUpdate(objFactory.Doc, htDetailGrd) == false)
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
        }//Completed
        private void DocDate_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                AddItm_Hash();
                if (DocHDRUtil.DocDate_CustomUpdate(objFactory.Doc, htDetailGrd) == false)
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
        }//Completed
        private void DocID_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (DocHDRUtil.DocID_CustomUpdate(objFactory.Doc, DocID.Text) == false)
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
        }//Completed
        private void DocDeptKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                AddItm_Hash();
                if (DocHDRUtil.DocDeptKey_CustomUpdate(objFactory.Doc, htDetailGrd) == false)
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
        }//Completed
        private void DocGrpKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                DocGrpKey.SetValueTrigger(GFunc.NEInt(DocGrpKey.Value, 0), false);
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
        private void DocOverallDisAcc_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            try
            {
                AddItm_Hash();
                DocHDRUtil.DocAccID_btnClick(this, objFactory.Doc, htDetailGrd, DocOverallDisAcc, GEnum.PopupType.AccDisID, ContextMenuSetting, objFactory.PermID);
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
        private void DocOverallDisRate_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                AddItm_Hash();
                if (DocHDRUtil.DocDiscountRate_CustomUpdate(objFactory.Doc, htDetailGrd) == false)
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
        }//Completed
        private void DocOverallDisAmt_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                AddItm_Hash();
                if (DocHDRUtil.DocDiscountAmt_CustomUpdate(objFactory.Doc, htDetailGrd) == false)
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
        }//Completed
        private void DocShipName_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (DocHDRUtil.DocShipName_CustomUpdate(objFactory.Doc) == false)
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
        }//Completed
        private void DocShipMark_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            try
            {
                DocHDRUtil.DocShipMark_btnClick(objFactory.Doc);
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
        private void DocTaxGrpKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                AddItm_Hash();
                if (DocHDRUtil.DocTaxGrpKey_CustomUpdate(objFactory.Doc, htDetailGrd) == false)
                    e.Cancel = true;
                else if (pRow != null)
                    CalCulateProcessFee();
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
        private void DocTranGrpKey_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            try
            {
                AddItm_Hash();
                DocHDRUtil.DocTranGrpID_btnClick(objFactory.Doc, htDetailGrd);
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
        private void DocTranGrpKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                AddItm_Hash();
                if (DocHDRUtil.DocTranGrpKey_CustomUpdate(objFactory.Doc, htDetailGrd) == false)
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
        }//Completed
        private void DocTypeNm_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                AddItm_Hash();

                CheckDirectShipment();
                if (CheckStockLoc() == false ) return;

                /* added by YST to auto fill Tax when DirectShipment to others <-> others to DirectShipment */
                if (DocConKey.Text != string.Empty && DocTaxGrpKey.Text == string.Empty) 
                {
                    DocConKey_CustomUpdate((TAComboBox)DocConKey, e);
                }
                /* end by YST */

                if (DocHDRUtil.DocTypeNm_CustomUpdate(objFactory.Doc, htDetailGrd, GFunc.NEStr(DocTypeNm.Value, string.Empty)) == false)
                    e.Cancel = true;
                else if (tagrdDetItms.Rows.Count > 0)
                {
                    pRow = tagrdDetItms.Rows.OfType<UltraGridRow>().ToList().Find
                        (row => (int)row.Cells["ItmKey"].Value == SysOptionUtility.ProcessingItem/*102937*/);
                    CalCulateProcessFee();
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
        private void DefBAddrKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (GFunc.IsNEZ(objFactory.Doc.DocConKey) == false)
                {
                    if (DocComUtility.Address_Set(objFactory.Doc, (int)GEnum.AddrLinkType.CustomerOrVendor, GFunc.NEInt(objFactory.Doc.DocConKey, 0), DefBAddrKey.Text, true, false) == false)
                        e.Cancel = true;
                }
                else
                {
                    MsgBox.Show("Customer cannot be empty");
                    e.Cancel = true;
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
        private void DefSAddrKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (GFunc.IsNEZ(objFactory.Doc.DocConKey) == false)
                {
                    if (DocComUtility.Address_Set(objFactory.Doc, (int)GEnum.AddrLinkType.CustomerOrVendor, GFunc.NEInt(objFactory.Doc.DocConKey, 0), DefSAddrKey.Text, false, true) == false)
                        e.Cancel = true;
                }
                else
                {
                    MsgBox.Show("Customer cannot be empty");
                    e.Cancel = true;
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
        private void DefLocKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                AddItm_Hash();
                DocHDRUtil.DefLocKey_CustomUpdate(objFactory.Doc, htDetailGrd, (int?)DefLocKey.Value);
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
        private void DefJobKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                int defJobKey = GFunc.NEInt(this.DefJobKey.Value, 0);
                AddItm_Hash();
                if (DocHDRUtil.DefJob_CustomUpdate(objFactory.Doc, htDetailGrd, defJobKey, false) == false)
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
        }
        private void DefAccKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                int defAccKey = GFunc.NEInt(this.DefAccKey.Value, 0);
                string defAccDes = this.DefAccKey.Text;
                AddItm_Hash();
                if (DocHDRUtil.DefAcc_CustomUpdate(objFactory.Doc, htDetailGrd, defAccKey, defAccDes, false) == false)
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
        }//Completed

        private void tabDetailList_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Down:
                    switch (tabDetailList.ActiveTab.Key.ToLower())
                    {
                        case "tsbitems":
                            GlobalUI.TabKeyDownForGrid(tagrdDetItms);
                            break;
                    }
                    break;
            }
        }//Completed

        //Grid Events
        private void tagrdDetItms_InitializeRow(object sender, InitializeRowEventArgs e) /* added by YST */
        {
            if (GFunc.NEDec(e.Row.Cells["ItmQtyBalance"].Value, 0) != 0)
            {
                e.Row.Cells["ItmQtyBalance"].Appearance.ForeColor = Color.Red;
            }
        }
        private void tagrdDetItms_BeforeRowInsert(object sender, BeforeRowInsertEventArgs e)
        {
            try
            {
                DocDetUtil.AutoIncrement((int)objFactory.Doc.DocCodeKey, tagrdDetItms);
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
        private void tagrdDetItms_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                GridCellLock_Set();
                //if (pRow != null) CalCulateProcessFee();
                GlobalUI.PopupRefresh(tagrdDetItms);
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
        private void tagrdDetItms_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                //added by nnt on 05 Aug 2020
                //if (Convert.ToInt32(e.Cell.Row.Cells["ItmQty"].Value) > 0) Check();
                //added by thettm on 28 jun 2018(start)
                if ((GFunc.CompareString(e.Cell.Column.Key, "ItmID") ||
                    GFunc.CompareString(e.Cell.Column.Key, "ItmDes")
                    ) && objFactory.Doc.IsNew ==false)
                if (tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmQty"].CellActivation ==
                    Activation.ActivateOnly)
                {
                    tagrdDetItms.AutoAddNewRow = false;
                    return;
                }
                //added by thettm on 28 jun 2018(end)

                /* added by YST */
                if (e.Cell.Column.Key == "ItmID" || e.Cell.Column.Key == "ItmDes")
                {
                    DataRow CurrentRow = ((DataRowView)((UltraGrid)sender).ActiveRow.ListObject).Row;
                    if (IsItemExported(CurrentRow))
                    {
                        return;
                    }
                }
                /* end */
                				
                if (GFunc.CompareString(e.Cell.Column.Key, "ItmAttachment"))
                {
                    if (tagrdDetItms.ActiveRow.Update())
                        DocDetUtil.ItmAttachment_btnClick(this, objFactory.Doc.Attachments, objFactory.Doc, tagrdDetItms);
                }
                else if (GFunc.CompareString(e.Cell.Column.Key, "APPOID"))
                {
                    UltraGrid grid = sender as UltraGrid;
                    int left = 0, top = 0;
                    if (this.WindowState == FormWindowState.Maximized)
                    {
                        left = this.Left + grid.Left + grid.DisplayLayout.UIElement.CurrentMousePosition.X - e.Cell.Column.Width;
                        top = this.Top + grid.Top + grid.DisplayLayout.UIElement.CurrentMousePosition.Y;
                    }
                    else
                    {
                        left = this.Left + grid.Left + grid.DisplayLayout.UIElement.CurrentMousePosition.X - e.Cell.Column.Width;
                        top = this.Top + grid.Top + tabDetailList.Top + tspBar.Height + grid.DisplayLayout.UIElement.CurrentMousePosition.Y;
                    }
                    cmnuPOBL.Show(new Point(left, top));
                   /* int DocKey = 0;
                    int DocItmKey = 0;
                    int itmtype =0;
                    if (tagrdDetItms.ActiveRow != null)
                    {
                        itmtype = GFunc.NEInt(tagrdDetItms.ActiveRow.Cells["ItmType"].Value, 0);
                        string[] nslink = GFunc.NEStr(tagrdDetItms.ActiveRow.Cells["NSLink"].Value, "").Split('-');
                        if (nslink.Length >= 3)

                            if (nslink[0] == "13250" || nslink[0] == "13500")
                            {
                                DocKey = GFunc.NEInt(nslink[1], 0);
                                DocItmKey = GFunc.NEInt(nslink[2], 0);
                            }
                            else
                            {
                                DataTable dt = GetDocKeyByNSLink(GFunc.NEStr(tagrdDetItms.ActiveRow.Cells["NSLink"].Value, ""), itmtype == 600);
                               
                                if(dt.Rows.Count>0)
                                {
                                    DocKey = GFunc.NEInt(dt.Rows[0]["DocKey"], 0);
                                    DocItmKey = GFunc.NEInt(dt.Rows[0]["DocItmKey"], 0);
                                }
                            }
                    }
                    if (itmtype == 600)
                    {
                        frmInsertSalesPO f = new frmInsertSalesPO(objFactory.Doc, tagrdDetItms, DocKey, DocItmKey);
                        f.StartPosition = FormStartPosition.CenterScreen;
                        f.ShowDialog();
                    }
                    else
                    {
                        frmInsertSalesBL f = new frmInsertSalesBL(objFactory.Doc, tagrdDetItms, DocKey, DocItmKey);
                        f.StartPosition = FormStartPosition.CenterScreen;
                        f.ShowDialog();
                    }*/
                    if (tagrdDetItms.ActiveRow != null)
                        tagrdDetItms.ActiveRowScrollRegion.ScrollRowIntoView(tagrdDetItms.ActiveRow);
                }
                else
                {
                    AddItm_Hash();
                    string listSetingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, e.Cell.Column.Key, ((Control)sender).Name);
                    DocDetUtil.DetItmGrid_CellButtonClick(objFactory.Doc, htDetailGrd, e.Cell, listSetingID);
                    //added by thettm on 17 jan 2019(start)  
                    if (e.Cell.Column.Key == "ItmID")
                    {
                        if (GFunc.NEInt(e.Cell.Row.Cells["ItmKey"].Value, 0) == SysOptionUtility.ProcessingItem)
                        {
                            if (pRow == null)
                            {
                                pRow = e.Cell.Row;
                                CalCulateProcessFee();
                            }
                            else if (pRow.Index != ((UltraGrid)sender).ActiveRow.Index)
                            {
                                e.Cell.Row.CancelUpdate();
                                MessageBox.Show("Processing Fee can't be added more than one time!");
                                return;
                            }
                        }
                        else if (pRow != null && pRow.Index == ((UltraGrid)sender).ActiveRow.Index)
                            pRow = null;
                    }
                   
                    //added by thettm on 17 jan 2019(end)
                    GridCellLock_Set();
                }

                // added by KKAung on 22-Oct-2022 (start)
                if (e.Cell.Column.Key == "ItmID" || e.Cell.Column.Key == "ItmDes")
                {
                    if (e.Cell.Text == "QASample")
                    {
                        tagrdDetItms.ActiveRow.Cells["ItmPrmDate"].Value = DBNull.Value;
                    }
                }
                // (end)
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
        private void tagrdDetItms_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            try
            {
                DocDetUtil.ItmRow_CellDblClick(objFactory.Doc, tagrdDetItms, e.Cell.Column.Key);
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
        private void tagrdDetItms_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
                //commented by Jane on 02-Dec-2013. 
                //After type itmpriceafter, then go to another form. This event not fire.
                //Need to fire this event for active cell to get related value updated if you go to another form also.
                //if (formClose || frmMain.gfrmMain.ActiveMdiChild != this || tagrdDetItms.ActiveCell == null)
                //    return;
                if (formClose || tagrdDetItms.ActiveCell == null)
                    return;

                //added by nnt on 05 Aug 2020
                //if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM || SysOptionUtility.DatabaseBranchCode == DBCode.ADL
              || SysOptionUtility.DatabaseBranchCode == DBCode.SOP || SysOptionUtility.DatabaseBranchCode == DBCode.BOS)
                {

                    if (DetailTotal.Text.Trim() != "0.00\r\n") CheckOrange();
                }

                /* added by YST */
                if (e.Cell.Column.Key == "ItmID")
                {
                    DataRow CurrentRow = ((DataRowView)((UltraGrid)sender).ActiveRow.ListObject).Row;
                    if (IsItemExported(CurrentRow))
                    {
                        tagrdDetItms.ActiveCell.Value = tagrdDetItms.ActiveCell.OriginalValue;
                        return;
                    }
                }
                /* end */

                switch (e.Cell.Column.Key)
                {                    
                    case "APPOID":

                        if (frmMain.gfrmMain.ActiveMdiChild != this)
                        {
                            return;
                        }
                        if (e.Cell.Text != "")
                        {
                            int itmtype = 0;
                            int DocKey = 0;
                            int DocItmKey = 0;
                            bool ok = false;
                            itmtype = GFunc.NEInt(tagrdDetItms.ActiveRow.Cells["ItmType"].Value, 0);
                            DocKey = GetDocKey(e.Cell.Text,itmtype==600);
                            if (DocKey == 0)
                            {
                                if (tagrdDetItms.ActiveRow != null)
                                {
                                    string[] nslink = GFunc.NEStr(tagrdDetItms.ActiveRow.Cells["NSLink"].Value, "").Split('-');
                                    if (nslink.Length >= 3)
                                        if (nslink[0] == "13250" || nslink[0] == "13500")
                                        {
                                            DocKey = GFunc.NEInt(nslink[1], 0);
                                            DocItmKey = GFunc.NEInt(nslink[2], 0);
                                        }
                                }
                            }
                            Form f = null;
                            if (itmtype == 600)
                            {
                                f = new frmInsertSalesPO(objFactory.Doc, tagrdDetItms, DocKey, DocItmKey);
                                f.StartPosition = FormStartPosition.CenterScreen;
                                ok = f.ShowDialog() == DialogResult.OK;                                
                            }
                            else
                            {
                                f = new frmInsertSalesBL(objFactory.Doc, tagrdDetItms, DocKey, DocItmKey);
                                f.StartPosition = FormStartPosition.CenterScreen;
                                ok = f.ShowDialog() == DialogResult.OK;  
                            }
                            if (ok)
                            {
                                if (tagrdDetItms.ActiveRow != null)
                                    tagrdDetItms.ActiveRowScrollRegion.ScrollRowIntoView(tagrdDetItms.ActiveRow);
                                if(f!=null)
                                    f.Close();
                            }
                            else
                            {
                                MsgBox.Show("The PO link will be cancelled as you did not select a PO and SN correctly.");
                                e.Cancel = true;
                                return;
                            }
                        }
                        else if (e.Cell.OriginalValue!="")
                        {
                            e.Cell.Row.Cells["NSLink"].Value = "11150-" + objFactory.Doc.DocKey + "-" + e.Cell.Row.Cells["DocItmKey"].Value;
                        }
                        break;                    
                }
                AddItm_Hash();                
                string listSetingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, e.Cell.Column.Key, ((Control)sender).Name);
                if (DocDetUtil.ItmRow_CustomCellUpdate(objFactory.Doc, htDetailGrd, GEnum.Details.Doc_Itm, listSetingID) == false)
                    e.Cancel = true;

                if (objFactory.Doc.DocTypeNm.ToUpper().Contains("ESTORE"))
                {
                    switch (e.Cell.Column.Key)
                     {
                         case "APPOID":
                             if (frmMain.gfrmMain.ActiveMdiChild != this)
                             {
                                 return;
                             }
                             if (e.Cell.Text != "")
                             {
                                 int POKey = 0;
                                 int POItmKey = 0;
                                 bool ok = false;
                                 int itmtype = 0;
                                 POKey = GetDocKey(e.Cell.Text,itmtype==600);
                                 if (POKey == 0)
                                 {
                                     if (tagrdDetItms.ActiveRow != null)
                                     {
                                         string[] nslink = GFunc.NEStr(tagrdDetItms.ActiveRow.Cells["NSLink"].Value, "").Split('-');
                                         if (nslink.Length >= 3)
                                             if (nslink[0] == "13250" || nslink[0] == "13500")
                                             {
                                                 POKey = GFunc.NEInt(nslink[1], 0);
                                                 POItmKey = GFunc.NEInt(nslink[2], 0);
                                             }
                                     }
                                 }
                                 Form f = null;
                                 if (itmtype == 600)
                                 {
                                     f = new frmInsertSalesPO(objFactory.Doc, tagrdDetItms, POKey, POItmKey);
                                     f.StartPosition = FormStartPosition.CenterScreen;
                                     ok = f.ShowDialog() == DialogResult.OK;
                                 }
                                 else
                                 {
                                     f = new frmInsertSalesBL(objFactory.Doc, tagrdDetItms, POKey, POItmKey);
                                     f.StartPosition = FormStartPosition.CenterScreen;
                                     ok = f.ShowDialog() == DialogResult.OK;
                                 }

                                 if (ok)
                                 {
                                     if (tagrdDetItms.ActiveRow != null)
                                         tagrdDetItms.ActiveRowScrollRegion.ScrollRowIntoView(tagrdDetItms.ActiveRow);
                                     if(f!=null)
                                        f.Close();
                                 }
                                 else
                                 {
                                     MsgBox.Show("The PO link will be cancelled as you did not select a PO and SN correctly.");
                                     e.Cancel = true;
                                     return;
                                 }
                             }
                             break;
                         case "ItmID":
                         case "ItmDes":
                             if(GFunc.NEInt(e.Cell.Row.Cells["ItmKey"].Value,0)== SysOptionUtility.ProcessingItem/*102937*/)
                             {
                                if (pRow == null)
                                {
                                    pRow = e.Cell.Row;
                                    CalCulateProcessFee();
                                }
                                else if (pRow.Index != ((UltraGrid)sender).ActiveRow.Index)
                                {
                                    e.Cell.Row.CancelUpdate();
                                    MessageBox.Show("Processing Fee can't be added more than one time!");
                                    return;
                                }
                            }
                           else if (pRow != null && pRow.Index == ((UltraGrid)sender).ActiveRow.Index)
                            pRow = null;
                        break;
                         case "ItmQty":
                         case "ItmPriceBefore":
                         case "ItmPriceAfter":
                         case "ItmAmtShw":
                        if (pRow!=null) CalCulateProcessFee(); //added by thettm on 16 jan 2019
                            break;                        
                    }
                }

                // added by KKAung on 9-Oct-2022 (start)
                if (e.Cell.Column.Key == "ItmID" || e.Cell.Column.Key == "ItmDes")
                {
                    if (e.Cell.Text == "QASample")
                    {
                        tagrdDetItms.ActiveRow.Cells["ItmPrmDate"].Value = DBNull.Value;
                    }
                }
                if (e.Cell.Column.Key.Equals("ItmQty"))
                    if (e.Cell.Column.Key.Equals("ItmQty"))
                    {
                        if (GFunc.NEInt(tagrdDetItms.ActiveRow.Cells["ItmBatchKey"].Value, 0) == 9999 && dtJobEst != null)//If Job header row
                        {
                            int activeRowIndex = tagrdDetItms.ActiveRow.Index;
                            int JobKey = GFunc.NEInt(tagrdDetItms.ActiveRow.Cells["ItmJobKey"].Value, 0);

                            if (dtJobEst.Rows.Count > 0)
                            {
                                if (JobKey != GFunc.NEInt(dtJobEst.Rows[0]["JobKey"], 0))
                                    dtJobEst = MSTJobDetEsts.Get(JobKey);
                            }
                            else
                                dtJobEst = MSTJobDetEsts.Get(JobKey);

                            int JobHQty = GFunc.NEInt(tagrdDetItms.ActiveRow.Cells["ItmQty"].Value, 0);
                            decimal NSum = dtJobEst.AsEnumerable().Sum(r => r.Field<decimal>("PrjCost"));
                            tagrdDetItms.ActiveRow.Cells["ItmVendorPrice"].Value = Math.Round(NSum * JobHQty, 2);


                            for (int i = activeRowIndex + 1; i < tagrdDetItms.Rows.Count; i++)
                            {
                                UltraGridRow row = tagrdDetItms.Rows[i];
                                if (GFunc.NEInt(row.Cells["ItmBatchKey"].Value, 0) > 0 && GFunc.NEInt(row.Cells["LineType"].Value, 0) == 1000
                                    && GFunc.NEInt(row.Cells["ItmJobKey"].Value, 0) == JobKey)
                                {   //Job Items Row, no price only Qty
                                    DataRow[] drs = dtJobEst.Select("JobEstKey=" + GFunc.NEInt(row.Cells["ItmBatchKey"].Value, 0));
                                    if (drs.Length > 0)
                                    {
                                        row.Cells["ItmQty"].Value = GFunc.NEDec(drs[0]["EstQty"], 0) * JobHQty;
                                    }
                                    row.Update();
                                    if (GFunc.NEInt(row.Cells["ItmType"].Value, 0) == (int)GEnum.ItemType.Assembly)
                                    {
                                        int parentQty = GFunc.NEInt(row.Cells["ItmQty"].Value, 0);
                                        DataRow[] drC = objFactory.DocDetItms.Select("LineLinkKey=" + GFunc.NEInt(row.Cells["DocItmKey"].Value, 0));
                                        foreach (DataRow cRow in drC)
                                        {
                                            cRow["ItmQty"] = parentQty * GFunc.NEInt(cRow["ItmIGrpQtySet"], 0);
                                        }
                                    }
                                    objFactory.DocDetItms.AcceptChanges();
                                }
                            }
                        }
                    }
                // (end)
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

        //added by thettm on 16 jan 2019(start)
        private void CalCulateProcessFee()
        {
            if (pRow != null)
            if (pRow.Index > -1 && objFactory.Doc.DocTypeNm.ToUpper().Contains("ESTORE"))
            {
              if (GFunc.NEInt(tagrdDetItms.Rows[pRow.Index].Cells["ItmKey"].Value, 0) == SysOptionUtility.ProcessingItem/*102937*/)
                {
                    AddItm_Hash();
                    DocComUtility.CalForm(objFactory.Doc, htDetailGrd, true, false);
                    decimal subTotal = objFactory.Doc.DocSubTotal - GFunc.NEDec(tagrdDetItms.Rows[pRow.Index].Cells["ItmAmtF"].Value, 0);
                    decimal gst = GFunc.RndC(subTotal * objFactory.Doc.DocTaxGrpRate, 2);
                    decimal fee = GFunc.RndC((subTotal + gst) * SysOptionUtility.ProcessingFee /*0.047M*/, 2);
                        if (fee != GFunc.NEDec(tagrdDetItms.Rows[pRow.Index].Cells["ItmAmtF"].Value, 0))
                        {
                            tagrdDetItms.Rows[pRow.Index].Cells["ItmPriceBefore"].Value = fee;
                            tagrdDetItms.Rows[pRow.Index].Cells["ItmPriceAfter"].Value = fee;
                            tagrdDetItms.Rows[pRow.Index].Cells["ItmPrice"].Value = fee;
                            tagrdDetItms.Rows[pRow.Index].Cells["ItmAmtF"].Value = fee;
                            tagrdDetItms.Rows[pRow.Index].Cells["ItmAmtH"].Value = GFunc.RndC(fee * objFactory.Doc.DocCurrRate, 2);
                            tagrdDetItms.Rows[pRow.Index].Cells["ItmAmtShw"].Value = fee;
                        }
                }
            }

        }
        //added by thettm on 16 jan 2019(start)
        private int GetDocKey(string docID, bool IsPO)
        {
            string proc = "Doc_GetDOKey";
            if (!IsPO)
            {
                proc = "Doc_GetBLKey";
            }

            List<SqlParameter> parList = new List<SqlParameter>();
            parList.Add(new SqlParameter("@DocID", docID));

            DataTable dt = GFunc.ExecuteProcReader(proc, parList);

            int doKey = 0;
            if (dt.Rows.Count > 0)
                doKey = GFunc.NEInt(dt.Rows[0][0], 0);

            return doKey;
        }
        private DataTable GetDocKeyByNSLink(string NSLink,bool IsPO)
        {
            string proc = "Doc_GetPOKeyByNSLink";

            if(!IsPO)
            {
                proc = "Doc_GetPIVKeyByNSLink";
            }

            List<SqlParameter> parList = new List<SqlParameter>();
            parList.Add(new SqlParameter("@NSLink", NSLink));

            DataTable dt = GFunc.ExecuteProcReader(proc, parList);

            return dt;
        }

        private void tagrdDetItms_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            try
            {
                DataRow NewRow = ((DataRowView)e.Row.ListObject).Row;
                
                if (objFactory.DocDetItm_Validation(NewRow) == false)
                {
                    e.Cancel = true;
                    return;
                }
                if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                {
                    decimal ePrice = GFunc.NEDec(e.Row.Cells["ItmControlPrice"].Value, 0);
                    if (GFunc.NEDec(e.Row.Cells["ItmPriceAfter"].Value, 0) > ePrice && ePrice != -999 && ePrice != 0)
                    {
                        MsgBox.Show("Warning!!! Sales Price " + GFunc.NEDec(e.Row.Cells["ItmPriceAfter"].Value, 0).ToString("$#,###.####") + " should not be higher than EStore Price ," + ePrice.ToString("$#,###.####"), GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                    }
                }
            }
            catch (TAException tex)
            {
                e.Cancel = true;
                Error(tex, true);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                Error(ex, true);
            }
        }//Completed
        private void tagrdDetItms_AfterRowUpdate(object sender, RowEventArgs e)
        {
            try
            {
                AddItm_Hash();                 
                DocDetUtil.ItmRow_Update(objFactory.Doc, htDetailGrd);
                //added by nnt on 05 Aug 2020
                if (!isCash)
                {
                    if (DocHome.Text.ToString() != "0.00") Check();
                    else if (DocHome.Text.ToString() == "0.00") { tsbDraft.Enabled = true; tsbCreateDO.Enabled = true; tsbPrint.Enabled = true; tsbSave.Enabled = true; txtCrSO.Text = ""; lblSOStatus.Text = ""; }
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
        private void tagrdDetItms_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            try
            {
                /* added by YST */
                DataRow CurrentRow = ((DataRowView)((UltraGrid)sender).ActiveRow.ListObject).Row;
                if (IsItemExported(CurrentRow))
                {
                    return;
                }
                /* end */

                if (e.Rows.Count() > 0)
                    if (GFunc.NEInt(e.Rows[0].Cells["ItmBatchKey"].Value, 0) > 0)
                    {
                        MsgBox.Show("Not allow to delete this row which is addded from Job.");
                        e.Cancel = true; // Cancels the delete action
                        return;          // Stop checking further
                    }

                bool clearprocessing = false;
                if (pRow != null && pRow.Index == ((UltraGrid)sender).ActiveRow.Index) clearprocessing = true;
                this.Cursor = Cursors.WaitCursor;
                e.DisplayPromptMsg = false;
                AddItm_Hash();
                if (!DocDetUtil.ItmRow_CancelDelete(objFactory.Doc, htDetailGrd, GEnum.Details.Doc_Itm))
                {
                    if (clearprocessing == true) pRow = null;
                    else if (pRow != null) CalCulateProcessFee();
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
                e.Cancel = true;//Always cancel the grid's auto deletion, we will handle the deletion in code and will not use the grid feature
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void tagrdDetItms_AfterRowsDeleted(object sender, EventArgs e)
        {
            objFactory.DocDetItms.AcceptChanges();
        }//Completed

        //Functions
        private bool form_CanValidate()
        {
            try
            {
                //this function check if we can validates all the header.controls
                //all grid is able to commit its changes to its datasource
                TAUtil.ControlGVar.FormValidateFail = false;
                this.errorProvider1.Clear();
                this.Validate();
                this.tagrdDetItms.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdDetItms.UpdateData();

                //we need to check if the active row data cannot be commited 
                //if it cannot be commited, the IsGridDirty would return a false
                //thus saving should not be perform and the user needs to be inform of the data error
                if (IsGridsDirty(false) || TAUtil.ControlGVar.FormValidateFail)
                    return false;
                else
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
        private bool IsGridsDirty(bool undoChangesInGrid)
        {
            //This function check if the grid has uncommited data in its active orw
            //it also has an option to undo those uncommited changes. 
            try
            {
                #region tagrdDetItms
                if (tagrdDetItms.ActiveRow != null)
                {
                    if (tagrdDetItms.ActiveRow.DataChanged && !tagrdDetItms.ActiveRow.IsUnmodifiedTemplateAddRow)
                    {
                        if (undoChangesInGrid)
                        {
                            //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                            this.tagrdDetItms.PerformAction(UltraGridAction.UndoCell);
                            this.tagrdDetItms.PerformAction(UltraGridAction.UndoRow);
                        }
                        return true;
                    }
                }
                #endregion

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
        private bool SaveChanges(bool canDiscardChanges, bool saveOnlyWhenDirty, bool promptToSave, GEnum.DocAction ButtonAction )
        {
            bool result = false;
            bool isEmailNoti = false;
            string docLogStatus = "Notified-Issue";
            GEnum.MsgBoxButton btnSelect;
            System.Threading.Thread th = null;
            frmShowProgress f = null;
            approvalStatus = null;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (objFactory.Doc.IsReadOnly)
                    return true;
                //if (pRow != null && objFactory.Doc.IsDirty)  CalCulateProcessFee();
                if (form_CanValidate() == false)
                {
                    #region Cancel process or discard changes and return to caller to continue process(e.g Closing of Form)
                    if (canDiscardChanges)
                    {
                        btnSelect = MsgBox.Show("Validation Failed, Discard changes?",
                                                        GEnum.MsgBoxIcon.Question,
                                                        GEnum.MsgBoxButton.Yes,
                                                        GEnum.MsgBoxButton.No);

                        if (btnSelect == GEnum.MsgBoxButton.Yes)
                        {
                            this.objFactory.Doc.IsDirty = false;
                            IsGridsDirty(true);
                            return true;
                        }
                    }
                    return false;
                    #endregion
                }

                //Prompt to Save Changes
                if (objFactory.Doc.IsDirty && promptToSave)
                {
                    this.Focus(); //set focus when form is called from List form. If not, the user won't know which data to save or discard.

                    if (ButtonAction == GEnum.DocAction.Print)
                    {
                        btnSelect = MsgBox.Show(MsgID.Common.SaveChanges,
                                            GEnum.MsgBoxIcon.Question,
                                            GEnum.MsgBoxButton.Save_Changes,
                                            GEnum.MsgBoxButton.I_Dont_Know);
                        ButtonAction = GEnum.DocAction.Undetermine;
                    }
                    else
                        btnSelect = MsgBox.Show(MsgID.Common.SaveChanges,
                                            GEnum.MsgBoxIcon.Question,
                                            GEnum.MsgBoxButton.Save_Changes,
                                            GEnum.MsgBoxButton.Discard_Changes,
                                            GEnum.MsgBoxButton.I_Dont_Know);

                    if (btnSelect == GEnum.MsgBoxButton.I_Dont_Know)
                        return false;
                    else if (btnSelect == GEnum.MsgBoxButton.Discard_Changes)
                    {
                        if (canDiscardChanges)
                        {
                            if (objFactory.Doc.DocState == (int)GEnum.DocState.New)
                            {
                                objFactory.Doc.Attachments.DeleteWithDMAS();
                            }
                            this.objFactory.Doc.IsDirty = false;
                            IsGridsDirty(true);
                            return true;
                        }
                        else
                            return false;
                    }
                    else if (btnSelect == 0)//btnSelect = 0 is the red X button on the top right corner of the msgbox.
                    {
                        this.objFactory.Doc.IsDirty = false;
                        IsGridsDirty(true);
                        return true;
                    }
                }

                //Save any pending changes (note: if saveOnlyWhenDirty (false), it will always save regardless of Isdirty State)
                if (objFactory.Doc.IsDirty || saveOnlyWhenDirty == false)
                {
                    bool updateDoc = false;
                    if (ButtonAction == GEnum.DocAction.Post && objFactory.Doc.IsDirty)
                    {
                        updateDoc = true;
                    }

                    #region Saving
                    if (ButtonAction == GEnum.DocAction.Undetermine)
                    {
                        if (DocUtility.ButtonAction_Get(objFactory.Doc, ref ButtonAction) == false)
                            return false;
                    }

                    //Saving
                    if (GlobalUI.UpdateAssemblyChildItem(this.objFactory.Doc, tagrdDetItms) == false)
                        return false;

                    if (objFactory.Doc.DocReqDate != null && objFactory.Doc.DocReqDate.ToString() != string.Empty)
                    {
                        // 22_dec_2017 
                        //string datetxt = objFactory.Doc.DocHome == 0 ? DateTime.Now.ToString() : objFactory.Doc.DocReqDate.ToString().Split(' ')[0];
                        string datetxt = objFactory.Doc.DocReqDate.ToString().Split(' ')[0];
                        DateTime date;
                        DateTime time;
                        //DateTime.TryParse(datetxt, out date);   //  commented by KKAung on 27 Feb 2023
                        date = new DateTime(objFactory.Doc.DocReqDate.Value.Year, objFactory.Doc.DocReqDate.Value.Month, objFactory.Doc.DocReqDate.Value.Day);      // added by KKAung on 27 Feb 2023
                        RequiredTime.Update();
                        DateTime.TryParse(RequiredTime.Text, out time);
                        DateTime docreqdate = date.Date.Add(time.TimeOfDay);  //Convert.ToDateTime(date + " " + time);
                        if (objFactory.Doc.DocReqDate != docreqdate) /* modified by YST on 2023/05/08 to avoid always assigning to object & changing IsDirty true */
                            objFactory.Doc.DocReqDate = docreqdate;
                    } 
                    else
                    {
                        if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM && ButtonAction == GEnum.DocAction.Post)
                        {
                            MsgBox.Show("Please assign the required date.", GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                            tabDetailList.SelectedTab = tabDetailList.Tabs["tsbMain"];
                            DocReqDate.Focus();
                            return false;
                        }
                    }

                    if ((optPrintLog.Checked == null || optPrintLog.Checked == false) && (optPrintSales.Checked == null || optPrintSales.Checked == false))
                    {
                        MsgBox.Show("Please select Print By Sales or Logistics.");
                        optPrintSales.Focus();
                        return false;
                    }
                    else
                    {
                        string PrintDept = optPrintLog.Checked ? "L" : "S"; 
                        if (objFactory.Doc.PrintDept != PrintDept) /* modified by YST on 2023/05/08 to avoid always assigning to object & changing IsDirty true */
                            objFactory.Doc.PrintDept = PrintDept;
                    }           

                    #region  // commented by YST on 2021/08/10
                    /*
                    DataTable dtDS = (from row in objFactory.DocDetItms.AsEnumerable()
                                        where row.Field<int>("LineType") == 1000 && ((row.Field<int>("ItmType") == 100 && row.Field<decimal>("DSQty") > 0)) &&
                                        !(row.Field<string>("NSLink").Substring(0, 5) == "13250" || GFunc.NEStr(row.Field<string>("APPOID"), "") != "")
                                        select new
                                        {
                                            SN = row.Field<decimal?>("ItmSN"),
                                            ItemID = row.Field<string>("ItmID"),
                                            ItemDescription = row.Field<string>("ItmDes"),
                                            Qty = row.Field<decimal?>("ItmQty"),
                                            DirectShipQty = row.Field<decimal?>("DSQty"),
                                            Price = row.Field<decimal?>("ItmPrice"),
                                            Amount = row.Field<decimal?>("ItmAmtF"),
                                            WarningMessage = "Direct Shipment items should link to PO"
                                        }).AsDataTable();

                    if (dtDS.DefaultView.Count > 0)
                    {
                        if (MsgBoxGrid.Show("Direct Shipment items has not been linked to PO. Are you sure to continue saving?",
                            dtDS, GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                            return false;
                    }

                    DataTable dtNS = (from row in objFactory.DocDetItms.AsEnumerable()
                                        where row.Field<int>("LineType") == 1000 && (row.Field<int>("ItmType") == 600 ) &&
                                        !(row.Field<string>("NSLink").Substring(0, 5) == "13250" || GFunc.NEStr(row.Field<string>("APPOID"),"")!="")
                                        select new
                                        {
                                            SN = row.Field<decimal?>("ItmSN"),
                                            ItemID = row.Field<string>("ItmID"),
                                            ItemDescription = row.Field<string>("ItmDes"),
                                            Qty = row.Field<decimal?>("ItmQty"),
                                            Price = row.Field<decimal?>("ItmPrice"),
                                            Amount = row.Field<decimal?>("ItmAmtF"),
                                            WarningMessage = "PO link missing"
                                        }).AsDataTable();


                    if (dtNS.DefaultView.Count > 0)
                    {
                        if (MsgBoxGrid.Show("Some Non Stock items has not been linked to PO. Are you sure to continue saving?",
                            dtNS, GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                            return false;
                    }
                    */
                    #endregion

                    if(GFunc.NEInt(DocConKey.Value,0) > 0)
                    {
                        /* added by YST not allow to save with Inactive customer */
                        DataRow[] dr = ((DataTable)DocConKey.DataSource).Select("Key = " + GFunc.NEStr(DocConKey.Value, "0"));
                        if (dr != null)
                        {
                            bool isValidCustomer = true;
                            if (dr.Length == 0)
                                isValidCustomer = false;
                            else
                            {
                                dr = ((DataTable)DocConKey.DataSource).Select("( [Inactive] = True or ActiveWithProblem = True ) and Key = " + GFunc.NEStr(DocConKey.Value, "0"));
                                if (dr.Length > 0) isValidCustomer = false;
                            }

                            if (!isValidCustomer)
                            {
                                MsgBox.Show("The selected customer is <b>inactive</b> or <b>active with problem</b>." +
                                        "<br/>System does not allow you to proceed with the Sales Order." +
                                        "<br/>Please check with the Management or Finance team to enable this customer for sales.", GEnum.MsgBoxIcon.Error, GEnum.MsgBoxButton.OK);

                                DocConKey.Focus();
                                return false;
                            }
                        }

                        if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM &&                            
                            ButtonAction == GEnum.DocAction.Post &&
                            objFactory.Doc.DocTypeNm != "Direct Shipment" && 
                            objFactory.Doc.DocTypeNm != "Sales Order - VN"
                          )
                        {
                            if (MsgBox.Show("Are you sure to release this SO to Warehouse?", GEnum.MsgBoxIcon.Warning,
                                GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                                return false;
                        }

                        #region /* Added by YST on 2021/10/07 - Customer PO Num Validation */
                        if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                        {                            
                            if (DocCustPONum.Text.Trim().Equals(""))
                            {
                                if (MsgBox.Show("Customer PO# should not be empty. If it is not applicable, system will set <font color='red'>N.A</font> as default.<br/>Would you like to set Customer PO Number yourself ?"
                                    , GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                                {
                                    DocCustPONum.Focus();
                                    return false;
                                }
                                else
                                {
                                    objFactory.Doc.DocCustPONum = "N.A";
                                }
                            }
                            else
                            {
                                if (DocCustPONum.Text.ToUpper().Trim().Equals("NR"))
                                {
                                    MsgBox.Show("For standardization, Customer PO# should be <font color='red'>N.A</font> instead of NR if it is not applicable.");
                                    objFactory.Doc.DocCustPONum = "N.A";
                                }
                                if (!DocCustPONum.Text.ToUpper().Trim().Equals("N.A") &&
                                    !DocCustPONum.Text.ToUpper().Trim().Equals("NR") &&
                                    !DocCustPONum.Text.ToUpper().Trim().Equals("NA"))
                                {
                                    DataTable dt = objFactory.CheckDuplicateCustPO();
                                    if (dt.Rows.Count > 0)
                                    {
                                        if (MsgBoxGrid.Show("Customer PO# already exists. Are you sure to issue this DO?", dt, GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                                            return false;
                                    }
                                    if (ButtonAction == GEnum.DocAction.Post)
                                    {
                                        /* Check Customer PO Attachment by YST on 2021/11/30 */
                                        if (btnAttachmentEdit.Text.Contains("(0)"))
                                        {
                                            if (MsgBox.Show("Customer PO Attachment should not be empty.<br/>Would you like to attach Customer PO ?"
                                                , GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                                            {
                                                btnAttachmentEdit.Focus();
                                                return false;
                                            }
                                        }
                                    }
                                }
                            }                           
                        }
                        /* End by YST */
                        #endregion

                        #region /* Added by YST  - Check POLink, GSTReverse, Price0, QA Remark */
                        if ((SysOptionUtility.DatabaseBranchCode == DBCode.BHM ||
                             SysOptionUtility.DatabaseBranchCode == DBCode.SOP ||
                             SysOptionUtility.DatabaseBranchCode == DBCode.GLH ) &&
                             ButtonAction == GEnum.DocAction.Post)
                        {
                            if (CheckPOLink(ButtonAction) != "")
                                return false;
                            if (CheckReverseGST() == false)
                                return false;
                            if (CheckPriceZero() == false)
                                return false;
                            if (CheckStockLoc() == false)
                                return false;

                            #region CheckQAItemApproval
                            /* Check Sales Approval (All QA Remark) requested by Jane(MRO) & added by YST on 2020/10/01 */
                            CheckSpecialItemsApproval();
                            if (approvalStatus != null && approvalStatus.ToLower().Contains("fail"))
                            {
                                return false;
                            }
                            else if (approvalStatus != null && approvalStatus != ApprovalStatus.Approved)
                            {
                                if (isCancelValidation == false)
                                {
                                    MsgBox.Show("Cancelled or transferred SO should have a total amount of 0 or a item quantity of 0.");
                                    return false;
                                }
                                if (approvalStatus == ApprovalStatus.Requested || approvalStatus == ApprovalStatus.Rejected)
                                {
                                    return false;
                                }
                                if (approvalStatus == "")
                                {
                                    string Msg = "This SO will change to pending status and will not allow any changes." + // "<br/>In the meantime, the Sales Order (SO) will be saved as <font color='red'>Draft</font> ." +
                                                 "<br/><b>Are you sure you want to proceed with requesting Management's approval?</b>";
                                    if (MsgBox.Show(Msg, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                                    {
                                        if (isCancel == true)
                                        {
                                            objFactory.Doc.PrintDept = "S";
                                            optPrintSales.Checked = true;
                                        }
                                        if (objFactory.Save((int)ButtonAction) == GVar.gcPass)
                                        {
                                            result = true;
                                            objFactory.Doc.DocState = (int)GEnum.DocState.Pending;
                                            objFactory.Doc.IsReadOnly = true;
                                            RequestSecialItemsApproval();
                                        }
                                    }
                                    else
                                        return false;
                                }
                            }
                            #endregion
                        }

                        //*** checking polink for orange customer -- added by Jane on 30-Dec-2024 --commented by Jane on 24-Sep-2025
                        //if ((SysOptionUtility.DatabaseBranchCode == DBCode.BHM ||
                        //     SysOptionUtility.DatabaseBranchCode == DBCode.SOP) && 
                        //     ButtonAction.ToString() == "Save")
                        //{
                        //    if (!isCash)
                        //    {
                        //        if (CheckCusID())
                        //        {                                  
                                        
                        //            if (CheckPOLink(ButtonAction) != "")
                        //                return false;                                        
                                   
                        //        }
                        //    }
                        //}
                        #endregion

                        #region /* Check QASample - Promised Date added by KKAung on 10-Oct-2022, commented by YST & modified in CheckSpecialItemsApproval() */
                        //DataRow[] drsample = objFactory.DocDetItms.Select("ItmID = '" + SpecialRemark.Sample + "'");
                        //if (drsample != null && drsample.Length > 0)    //GEnum.DocAction.Post
                        //{
                        //    if (GFunc.IsNE(drsample[0]["ItmPrmDate"]))
                        //    {
                        //        MsgBox.Show("Please fill QASample - Due Date.");
                        //        int rowIndex = tagrdDetItms.Rows.FirstOrDefault(r => r.Cells["ItmID"].Value.ToString() == "QASample").Index;
                        //        tabDetailList.SelectedTab = tabDetailList.Tabs["tsbItems"];
                        //        tagrdDetItms.ActiveCell = tagrdDetItms.Rows[rowIndex == -1 ? 0 : rowIndex].Cells["ItmPrmDate"];
                        //        tagrdDetItms.PerformAction(UltraGridAction.EnterEditMode);
                        //        return false;
                        //    }                        
                        //    if (GFunc.NEDateTime(drsample[0]["ItmPrmDate"], DateTime.Today) < GFunc.NEDateTime(objFactory.Doc.DocReqDate.Value.ToShortDateString(), DateTime.Today))
                        //    {
                        //        MsgBox.Show("QASample - Due Date should not be less than Required Date: " + objFactory.Doc.DocReqDate.Value.ToString("dd MMM yyyy") + ".");
                        //        int rowIndex = tagrdDetItms.Rows.FirstOrDefault(r => r.Cells["ItmID"].Value.ToString() == "QASample").Index;
                        //        tabDetailList.SelectedTab = tabDetailList.Tabs["tsbItems"];
                        //        tagrdDetItms.ActiveCell = tagrdDetItms.Rows[rowIndex == -1 ? 0 : rowIndex].Cells["ItmPrmDate"];
                        //        tagrdDetItms.PerformAction(UltraGridAction.EnterEditMode);
                        //        return false;
                        //    }
                        //}
                        #endregion

                        #region /* Added by YST on 2023/09/04 to check One-time Customer & JobID for Athena */
                        if (SysOptionUtility.DatabaseBranchCode == DBCode.ADL && ButtonAction == GEnum.DocAction.Post)
                        {
                            if (DocConKey.Text.ToLower().Contains("one-time"))
                            {
                                MsgBox.Show("System does not allow posting with <font color='red'>" + DocConKey.Text.Trim() + "</font>.<br/> Please select the correct Customer ID from the list provided." +
                                             "<br/>If the desired customer is not available in the list, kindly inform the finance team to create a new customer record.", GEnum.MsgBoxIcon.Error, GEnum.MsgBoxButton.OK);
                                return false;
                            }

                            /* Alert warning message to add JobID added by YST on 2023/12/05, requested by Jia Ying, ADPL Accountant */
                            if (CheckJobID(ButtonAction) == false)
                            {
                                return false;
                            }

                            /* Alert warning message to add JobID added by YST on 2023/12/05, requested by Jia Ying, ADPL Accountant */
                            if (CheckAccCategory() == false)
                            {
                                return false;
                            }
                        }
                        #endregion

                        #region /* Added by YST on 2023/05/08 to alert before save but not to alert the message if SO has no changes or IsDirty is false */                       
                        if (SysOptionUtility.DatabaseBranchCode == DBCode.ADL && objFactory.ApprovalRequired && ButtonAction == GEnum.DocAction.Post &&
                            (DocState.Text == "Draft" || objFactory.Doc.IsDirty || objFactory.Doc.Attachments.IsDirty))
                        {
                            string Msg = "Would you like to notify to the persons in charge for SO issue ?";
                            if (DocState.Text.Contains("Post") || objFactory.Doc.DocStatus.Contains("Post"))
                            {
                                Msg = Msg.Replace("issue", "amendment");
                                docLogStatus = "Notified-Amend";
                            }
                            if (MsgBox.Show(Msg, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                            {
                                isEmailNoti = true;                                                           
                            }
                            else
                            {
                                isEmailNoti = false;
                            }
                        }
                        #endregion

                        try
                        {
                            f = new frmShowProgress("            Saving ........");
                            th = new System.Threading.Thread(() => ShowForm(f));
                            th.Start();
                        }
                        catch { }
                    }
                                      
                    if (approvalStatus == null || approvalStatus == ApprovalStatus.Approved)
                    {
                        if (objFactory.Save((int)ButtonAction) == GVar.gcPass)
                            result = true;
                    }

                    /* added by YST on 2023/05/08 to send Email Notification according to Dialog Result */
                    if (result && isEmailNoti && SysOptionUtility.DatabaseBranchCode == DBCode.ADL)
                    {
                        try
                        {
                            List<SqlParameter> parlist = new List<SqlParameter>();
                            parlist.Add(new SqlParameter("@DocKey", objFactory.Doc.DocKey));
                            parlist.Add(new SqlParameter("@ApprovalStatus", docLogStatus));
                            GFunc.ExecuteScalar("ARSO_Approval", parlist);                            
                        }
                        catch (Exception ex)
                        {
                            MsgBox.Show("Email Sending Error <br/>" + ex.ToString());
                        }                        
                    }
                    /* end by YST */

                    //*** commented by Jane on 29-Nov-2025
                    //if (ButtonAction.ToString() == "Save")
                    //{
                    //    if (!isCash)
                    //    {
                    //        if (CheckCusID())
                    //        {

                    //            if (GFunc.NEInt(DocHome.Text, 0) > 0)
                    //            {
                    //                string strCustomerName = this.DocConNm.Text;
                    //                string strDocID = this.DocID.Text;
                    //                tsbSave.Enabled = false;
                    //                tsbCreatePO.Enabled = false;
                    //                if (strDocID != "")
                    //                {
                    //                    if (MsgBox.Show("Are you sure to Request Approval to COO?", GEnum.MsgBoxIcon.Warning,
                    //                    GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                    //                    {
                    //                        SqlConnection cn = new SqlConnection(AppInfor.CurrentDBConnectionStr);
                    //                        cn.Open();
                    //                        List<SqlParameter> lstDocID = new List<SqlParameter>();
                    //                        lstDocID.Add(new SqlParameter("@DocID", strDocID));
                    //                        GFunc.ExecuteNonQueryProc(cn, "ARSO_SendNotiApproval", lstDocID);
                    //                        cn.Close();
                    //                        tsbSave.Enabled = false;
                    //                        tsbDraft.Enabled = false;
                    //                        tsbCreatePO.Enabled = false;
                    //                        tsbCreateDO.Enabled = false;
                    //                        tsbPrint.Enabled = false;
                    //                        MsgBox.Show("Requesting approval for this customer has been sent to COO. Release button still be disable unless it has been approved.");
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    if (ButtonAction.ToString() == "Post")
                    {
                        if (!isCash)
                        {
                            if (CheckCusID())
                            {

                                if (GFunc.NEInt(DocHome.Text, 0) > 0)
                                {
                                    string strCustomerName = this.DocConNm.Text;
                                    string strDocID = this.DocID.Text;
                                    //tsbSave.Enabled = false;
                                    tsbCreatePO.Enabled = false;
                                    if (strDocID != "")
                                    {
                                        //if (MsgBox.Show("Are you sure to Request Approval to COO?", GEnum.MsgBoxIcon.Warning,
                                        //GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                                        //{
                                            SqlConnection cn = new SqlConnection(AppInfor.CurrentDBConnectionStr);
                                            cn.Open();
                                            List<SqlParameter> lstDocID = new List<SqlParameter>();
                                            lstDocID.Add(new SqlParameter("@DocID", strDocID));
                                            GFunc.ExecuteNonQueryProc(cn, "ARSO_SendNotiApproval", lstDocID);
                                            cn.Close();
                                            tsbSave.Enabled = false; 
                                            //tsbDraft.Enabled = false;
                                            tsbCreatePO.Enabled = false;
                                            tsbCreateDO.Enabled = false;
                                            tsbPrint.Enabled = false;

                                            objFactory.Doc.DocState = (int)GEnum.DocState.Pending;
                                            //MsgBox.Show("Requesting approval for this customer has been sent to COO. Release button still be disable unless it has been approved.");
                                            MsgBox.Show("Requesting approval for this Orange-flagged customer has been sent to COO.");
                                        //}
                                    }
                                }
                            }
                        }
                    }
                    if (result && updateDoc && SysOptionUtility.HasDMASLink)
                        DocHDRUtil.ExportToDMAS(objFactory.Doc);

                    #region "commented by Jane 30-Jul-2025, need to refresh/rebind form regardless of "result" variable value.. moved following functions to Finally section below"
                    //if(result)
                    //{
                    //    Form_RefreshAll(false, false);
                    //    DocList_Refresh();
                    //}
                    #endregion

                    return result;
                    #endregion
                }
                else
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

                Form_RefreshAll(false, false);
                DocList_Refresh();

                try
                {                    
                    if (th != null)
                    {
                        if (f != null)
                        {
                            f.CloseMe();
                            f = null;
                        }
                        //th.Abort();
                    }
                }
                catch { }
            }
        }//Completed       

        private void ShowForm(frmShowProgress fprg)
        {
            if (fprg != null)
                fprg.ShowDialog();
        }
        /* added by YST (start) */
        private string CheckPOLink(GEnum.DocAction ButtonAction)
        {
            /* Check PO Link modified by YST on 2021/08/10 not to allow saving without PO to be able to calculate costing for Non-stock items */
            string MsgPOLink = ButtonAction == GEnum.DocAction.Post ? "<b><font color='red'> System will not allow to proceed.</font></b>" : "";           
            DataTable dtDS = (from row in objFactory.DocDetItms.AsEnumerable()
                              where row.Field<int>("LineType") == 1000 
                              && ((row.Field<int>("ItmType") == (int)GEnum.ItemType.Stock && row.Field<decimal>("DSQty") > 0) || 
                                  (row.Field<int>("ItmType") == (int)GEnum.ItemType.Non_Stock && row.Field<decimal>("DSQty") > 0)) 
                              &&!(row.Field<string>("NSLink").Substring(0, 5) == GEnum.SystemCode.Purchase_Order.ToString() || GFunc.NEStr(row.Field<string>("APPOID"), "") != "") 
                              && !row.Field<string>("ItmID").Contains("/OS") /* old invetontory items code with OS are used as non-stock items that can't link to PO */
                              && GFunc.IsNEZ(row.Field<int?>("ItmBatchKey"))//Not Job
                              select new
                              {
                                  SN = row.Field<decimal?>("ItmSN"),
                                  Marking = row.Field<string>("ItmMark"),
                                  ItemID = row.Field<string>("ItmID"),
                                  ItemDescription = row.Field<string>("ItmDes"),
                                  Qty = row.Field<decimal?>("ItmQty"),
                                  DirectShipQty = row.Field<decimal?>("DSQty"),
                                  Price = row.Field<decimal?>("ItmPrice"),
                                  Amount = row.Field<decimal?>("ItmAmtF"),
                                  WarningMessage = "PO link missing"
                              }).AsDataTable();

            if (dtDS.DefaultView.Count > 0)
            {
                MsgPOLink = "<b>PO or Bill is required to link for the Direct Shipment items.</b>" + MsgPOLink;
                MsgBoxGrid.Show(MsgPOLink, dtDS, GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
            }
            else
            {
                DataTable dtNS = (from row in objFactory.DocDetItms.AsEnumerable()
                                  where row.Field<int>("LineType") == 1000
                                  && (row.Field<int>("ItmType") == (int)GEnum.ItemType.Non_Stock && row.Field<decimal>("ItmQty") > 0)
                                  && !(row.Field<string>("NSLink").Substring(0, 5) == GEnum.SystemCode.Purchase_Order.ToString() || GFunc.NEStr(row.Field<string>("APPOID"), "") != "")
                                  && !row.Field<string>("ItmID").Contains("/OS")
                                  && GFunc.IsNEZ(row.Field<int?>("ItmBatchKey"))//Not Job
                                  select new
                                  {
                                      SN = row.Field<decimal?>("ItmSN"),
                                      Marking = row.Field<string>("ItmMark"),
                                      ItemID = row.Field<string>("ItmID"),
                                      ItemDescription = row.Field<string>("ItmDes"),
                                      Qty = row.Field<decimal?>("ItmQty"),
                                      Price = row.Field<decimal?>("ItmPrice"),
                                      Amount = row.Field<decimal?>("ItmAmtF"),
                                      WarningMessage = "PO link missing"
                                  }).AsDataTable();


                if (dtNS.DefaultView.Count > 0)
                {
                    MsgPOLink = "<b>PO or Bill is required to link for Non Stock items.</b>" + MsgPOLink;
                    MsgBoxGrid.Show(MsgPOLink, dtNS, GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                }
                else
                {
                    MsgPOLink = "";
                }
            }       

            return MsgPOLink;
        }
        private string CheckSpecialItemsApproval()
        {
            DataTable dtSpecialItem = null;
            approvalStatus = null;
            string ItemID = "";
            isCancelValidation = true;

            DataRow[] drSepcialItem = objFactory.DocDetItms.Select("ItmID = '" + SpecialRemark.Cancel + "' OR ItmID = '" + SpecialRemark.Transfer + "'");
            if (drSepcialItem != null && drSepcialItem.Length > 0)
            {
                ItemID = drSepcialItem[0]["ItmID"].ToString();
                if (ItemID == SpecialRemark.Cancel || ItemID == SpecialRemark.Transfer)
                    isCancel = true;
            }        
            else
            {
                string Msg = ""; int rowIndex = 0;
                isCancel = false;
                drSepcialItem = objFactory.DocDetItms.Select("ItmID = '" + SpecialRemark.FOC + "'"
                                                              + " OR ItmID = '" + SpecialRemark.Sample + "'" 
                                                              + " OR ItmID = '" + SpecialRemark.GoodsReplacement + "'"
                                                              + " OR ItmID = '" + SpecialRemark.GoodsModification + "'"
                                                              + " OR ItmID = '" + SpecialRemark.WarrantyClaim + "'"
                                                              + " OR ItmID = '" + SpecialRemark.WrongSupply + "'"
                                                              + " OR ItmID = '" + SpecialRemark.ShortageSupply + "'");

                if (drSepcialItem != null && drSepcialItem.Length > 0)
                {
                    ItemID = drSepcialItem[0]["ItmID"].ToString();

                    var dtResult = objFactory.DocDetItms.Select("ItmQty > 0");
                    if (dtResult != null && dtResult.FirstOrDefault() != null)
                    {
                        DataTable dtFilter = objFactory.DocDetItms.Select("ItmQty > 0").CopyToDataTable();
                        if (dtFilter.Rows.Count > 0)
                        {
                            if (ItemID == SpecialRemark.WarrantyClaim)
                            {
                                drSepcialItem = objFactory.DocDetItms.Select("ItmWrtyEndDate Is not null AND LineLinkKey = 0  AND ItmQty > 0");
                                if (drSepcialItem != null && drSepcialItem.Length > 0)
                                {
                                    dtFilter = drSepcialItem.CopyToDataTable();
                                    drSepcialItem = dtFilter.Select("(ItmRef NOT LIKE 'RT%' OR ISNULL(ItmRef,'') = '') AND LineLinkKey = 0 ");
                                    if (drSepcialItem != null && drSepcialItem.Length > 0)
                                    {
                                        Msg = "Warranty tracking item requires Defect Report as an internal reference.";
                                    }
                                    else
                                    {
                                        drSepcialItem = null;
                                    }
                                }
                                else
                                {
                                    drSepcialItem = dtFilter.Select("ItmRef = '" + ItemID + "' AND LineLinkKey = 0");
                                }
                            }
                            else
                            {
                                drSepcialItem = dtFilter.Select("ItmRef = '" + ItemID + "' AND LineLinkKey = 0");
                            }

                            if ((drSepcialItem != null && drSepcialItem.Length == 0) || !string.IsNullOrEmpty(Msg))
                            {
                                GEnum.MsgBoxButton btnSelect;
                                if (string.IsNullOrEmpty(Msg))
                                {
                                    Msg = "There is no item indicating " + ItemID + " as the internal reference.<br/> Would you like to amend internal reference ?";
                                    btnSelect = MsgBox.Show(Msg, GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No);
                                    var row = tagrdDetItms.Rows.FirstOrDefault(r => GFunc.NEInt(r.Cells["ItmType"].Value, 0) < 700
                                                                                 && GFunc.NEDec(r.Cells["LineLinkKey"].Value, 0) == 0
                                                                                 && GFunc.NEStr(r.Cells["ItmRef"].Value, "") != ItemID
                                                                                 && GFunc.NEDec(r.Cells["ItmQty"].Value, 0) > 0);
                                    rowIndex = row != null ? row.Index : -1;
                                }
                                else
                                {
                                    btnSelect = MsgBox.Show(Msg, GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                                    var row = tagrdDetItms.Rows.FirstOrDefault(r => GFunc.NEInt(r.Cells["ItmType"].Value, 0) < 700
                                                                                 && GFunc.NEDec(r.Cells["LineLinkKey"].Value, 0) == 0
                                                                                 && GFunc.NEDec(r.Cells["ItmQty"].Value, 0) > 0
                                                                                 && !GFunc.NEStr(r.Cells["ItmRef"].Value, "").Contains("RT")
                                                                                 && GFunc.NEDateTime(r.Cells["ItmWrtyEndDate"].Value, DateTime.Today) > DateTime.Today
                                                                                 );
                                    rowIndex = row != null ? row.Index : -1;
                                }

                                if (btnSelect == GEnum.MsgBoxButton.Yes || btnSelect == GEnum.MsgBoxButton.OK)
                                {
                                    if (rowIndex >= 0)
                                    {
                                        tabDetailList.SelectedTab = tabDetailList.Tabs["tsbItems"];
                                        tagrdDetItms.ActiveCell = tagrdDetItms.Rows[rowIndex == -1 ? 0 : rowIndex].Cells["ItmRef"];
                                        tagrdDetItms.PerformAction(UltraGridAction.EnterEditMode);
                                    }
                                    approvalStatus = ItemID + "-Fail";
                                    ItemID = null;
                                }
                            }
                        }
                    }
                }
                if (ItemID == SpecialRemark.Sample)
                {
                    /* Check QASample - Promised Date added by KKAung on 10-Oct-2022 , modified by YST on 03-Apr-2023 */
                    drSepcialItem = objFactory.DocDetItms.Select("ItmID = '" + SpecialRemark.Sample + "'");

                    if (GFunc.IsNE(drSepcialItem[0]["ItmPrmDate"]))
                    {
                        Msg = "Sample items need to be returned.<br/>The promised date is required for QASample to ensure the sample items are returned.";
                    }
                    else if (GFunc.NEDateTime(drSepcialItem[0]["ItmPrmDate"], DateTime.Today) < GFunc.NEDateTime(objFactory.Doc.DocReqDate.Value.ToShortDateString(), DateTime.Today))
                    {
                        Msg = "Sample items need to be returned.<br/>The promised date should be scheduled after the delivery required date (" + objFactory.Doc.DocReqDate.Value.ToString("dd MMM yyyy") + ").";                                             
                    }
                    if ( Msg != "")
                    {
                        rowIndex = tagrdDetItms.Rows.FirstOrDefault(r => r.Cells["ItmID"].Value.ToString() == ItemID).Index;
                        tabDetailList.SelectedTab = tabDetailList.Tabs["tsbItems"];
                        tagrdDetItms.ActiveCell = tagrdDetItms.Rows[rowIndex == -1 ? 0 : rowIndex].Cells["ItmPrmDate"];
                        tagrdDetItms.PerformAction(UltraGridAction.EnterEditMode);
                        MsgBox.Show(Msg);
                        approvalStatus = ItemID + "-DateFail";
                        ItemID = null;
                    }                    
                }               
            }

            if (isCancel == true)
            {
                drSepcialItem = objFactory.DocDetItms.Select("ItmType < 700 AND LineLinkKey = 0  AND ItmQty > 0");
                if (objFactory.Doc.DocHome > 0 || (drSepcialItem != null && drSepcialItem.Length > 0))
                {
                    isCancelValidation = false;                    
                }
                approvalStatus = "";
            }
            else if (!string.IsNullOrEmpty(ItemID))
            {
                approvalStatus = CheckApprovalStatus(isCancel);
                switch (approvalStatus)
                {
                    case ApprovalStatus.Requested:
                        MsgBox.Show("It has been requested the approval. System will not save any changes.");
                        break;
                    case ApprovalStatus.Rejected:
                        MsgBox.Show("This SO has been rejected.<br/> Please amend or cancel SO so that you can release it.");
                        break;
                    case "":
                        break;
                    default:
                        break;
                }
            }

            return approvalStatus;
        }
        private string CheckApprovalStatus(bool isCancel)
        {
            string approvalStatus = "";

            try
            {
                List<SqlParameter> lstParam = new List<SqlParameter>();
                lstParam.Add(new SqlParameter("@DocKey", objFactory.Doc.DocKey));
                lstParam.Add(new SqlParameter("@IsCancel", isCancel));
                DataTable dt = GFunc.ExecuteProcReader("ARSO_GetApprovalStatus", lstParam);
                if (dt.Rows.Count > 0) approvalStatus = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return approvalStatus;
        }        
        private void RequestSecialItemsApproval()
        {
            SqlConnection cn = new SqlConnection(AppInfor.CurrentDBConnectionStr);
            try
            {
                cn.Open();
                List<SqlParameter> lstParam = new List<SqlParameter>();
                lstParam.Add(new SqlParameter("@DocKey", objFactory.Doc.DocKey));
                lstParam.Add(new SqlParameter("@ApprovalStatus", isCancel == true ? "Cancel" : "Special"));
                GFunc.ExecuteNonQueryProc(cn, "ARSO_ApprovalNotiAll", lstParam);
                cn.Close();                                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                cn.Close();
            }
        }         
        private string CheckReversedCN(string DoDocID, int? DocConKey, decimal DocSubTotal)
        {
            string reversedSO = ""; 
            try
            {
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@DoDocID", DoDocID));
                parmList.Add(new SqlParameter("@DocConKey", DocConKey));
                parmList.Add(new SqlParameter("@DocSubTotal", DocSubTotal));
                reversedSO = GFunc.ExecuteScalar("ARSO_CheckReversedCN", parmList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return reversedSO;
        }
        private void CheckDirectShipment()
        {
            /* added by YST to auto fill/clear Direct Ship Qty of stock and non-stock items when document type is changed */
            DataRow[] dr;
            string itmtype = (int)GEnum.ItemType.Stock + "," + (int)GEnum.ItemType.Non_Stock;
            DataTable dtItem = (DataTable)tagrdDetItms.DataSource;

            if (dtItem == null || dtItem.Rows.Count == 0) return;

            if (DocTypeNm.Text == "Direct Shipment")
            {
                dr = dtItem.Select("Itmtype in (" + itmtype + ") and DSQty = 0 and ItmQty > 0 and LineLinkKey = 0");
                if (dr.Length > 0)
                {
                    MsgBox.Show("Apply direct ship qty to all stock/non-stock items that have direct ship qty 0 ! ", GEnum.MsgBoxButton.OK);
                    foreach (DataRow row in objFactory.DocDetItms.Rows)
                    {
                        dr = dtItem.Select("Itmtype in (" + itmtype + ") and DSQty = 0 and ItmQty > 0 and LineLinkKey = 0 and DocItmKey = " + row["DocItmKey"]);
                        if (dr.Length > 0)
                        {
                            row["DSQty"] = row["ItmQty"];
                        }
                    }
                    objFactory.DocDetItms.AcceptChanges();
                }
            }
            else
            {
                dr = dtItem.Select("Itmtype in (" + itmtype + ") and DSQty > 0 ");
                if (dr.Length > 0)
                {
                    foreach (DataRow row in objFactory.DocDetItms.Rows)
                    {
                        row["DSQty"] = 0;
                    }
                    objFactory.DocDetItms.AcceptChanges();                  
                }
            }
            dtItem = null;
        }
        private bool CheckStockLoc()
        {
            bool isProceed = true;
            /* added by YST to validate Item Location to match with its document type */
            DataRow[] drDocTypeNm = ((DataTable)DocTypeNm.DataSource).Select("DocTypeNm like '%VN%'");
            if (drDocTypeNm == null || drDocTypeNm.Length == 0) return isProceed;

            //string itmtype = (int)GEnum.ItemType.Stock + "," + (int)GEnum.ItemType.Non_Stock;
            DataRow[] drItmLoc; DataRow drLoc = null; int rowIndex = -1; string Msg = "";            
            DataTable dtItem = (DataTable)tagrdDetItms.DataSource;

            if (dtItem == null || dtItem.Rows.Count == 0) return isProceed;

            #region //
            /*
            if (DocTypeNm.Text.ToLower().Contains("direct ship"))
            {
                //var itemTypes = new List<int> { (int)GEnum.ItemType.Stock, (int)GEnum.ItemType.Non_Stock};
                int DSLocCount = dtItem.AsEnumerable()
                                            .Where(row =>
                                                row.Field<int>("ItmType") == (int)GEnum.ItemType.Stock &&
                                                row.Field<decimal>("ItmQty") > 0
                                            )
                                            .Select(row => row.Field<int?>("ItmLocKey"))
                                            .Distinct()
                                            .Count();
                if (DSLocCount > 1)
                {
                    MsgBox.Show(Msg + "The location of all items should be the same.", GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                    tabDetailList.SelectedTab = tabDetailList.Tabs["tsbItems"];
                    return;
                }
            }
            */
            #endregion

            if (DefLocKey.DataSource == null)
            {
                MsgBox.Show(Msg + "There is no data source to verify the item location.", GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                return isProceed;
            }
            else
            {
                if (DocTypeNm.Text.ToLower().Contains("vn"))
                {
                    if (DefLocKey.DataSource != null)
                    {
                        drLoc = ((DataTable)DefLocKey.DataSource).Select("LocID = 'Vietnam'").First();
                        drItmLoc = dtItem.Select("Itmtype in (" + ((int)GEnum.ItemType.Stock).ToString() + ") and ItmLocKey <> " + (drLoc != null ? drLoc[0].ToString() : "0") + " and ItmQty > 0");
                        if (drItmLoc != null && drItmLoc.Length > 0)
                        {
                            Msg = "For <b>" + DocTypeNm.Text.Trim() + "</b>, all items must be <b>Vietnam-located</b> stock.";
                            var row = tagrdDetItms.Rows.FirstOrDefault(r => GFunc.NEInt((r.Cells["DocItmKey"].Value), 0) == GFunc.NEInt(drItmLoc[0]["DocItmKey"], 0));
                            rowIndex = row != null ? row.Index : -1;
                        }
                    }
                }
                else
                {
                    if (DefLocKey.DataSource != null)
                    {
                        drLoc = ((DataTable)DefLocKey.DataSource).Select("LocID = 'Main'").First();
                        drItmLoc = dtItem.Select("Itmtype in (" + ((int)GEnum.ItemType.Stock).ToString() + ") and ItmLocKey <> " + (drLoc != null ? drLoc[0].ToString() : "0") + " and ItmQty > 0");
                        if (drItmLoc != null && drItmLoc.Length > 0)
                        {
                            Msg = "For <b>" + DocTypeNm.Text.Trim() + "</b>, all items must be <b>Main-located</b> stock.";
                            var row = tagrdDetItms.Rows.FirstOrDefault(r => GFunc.NEInt((r.Cells["DocItmKey"].Value), 0) == GFunc.NEInt(drItmLoc[0]["DocItmKey"], 0));
                            rowIndex = row != null ? row.Index : -1;
                        }
                    }
                }
                if (rowIndex > -1)
                {
                    MsgBox.Show(Msg + "<br/>Please check the items and its location.", GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                    tabDetailList.SelectedTab = tabDetailList.Tabs["tsbItems"];
                    tagrdDetItms.ActiveCell = tagrdDetItms.Rows[rowIndex == -1 ? 0 : rowIndex].Cells["ItmID"];
                    tagrdDetItms.PerformAction(UltraGridAction.EnterEditMode);
                    //return false;
                    isProceed = false;
                }
            }
            dtItem = null;
            return isProceed;
        }
        private bool CheckReverseGST()
        {
            /* Check GST Reverse requested by Johnny & added by YST on 2019/10/25 */
            string reversedSO = "", reversedDO = ""; bool result = true ;
            DataRow[] dr = objFactory.DocDetItms.Select("ItmID = '" + SpecialRemark.GSTReverse + "'");
            if (dr != null && dr.Length > 0)
            {
                reversedDO = dr[0]["ItmDes"].ToString();
                reversedSO = CheckReversedCN(reversedDO, objFactory.Doc.DocConKey, objFactory.Doc.DocSubTotal);
                if (reversedSO.Contains("DO"))
                {
                    MsgBox.Show(reversedSO.Replace("DO", "<b>" + reversedDO + "</b>"));
                    result = false;
                }
                else
                {
                    if (reversedSO != "")
                    {
                        MsgBox.Show("<b>" + reversedDO + "</b>" + " has already finished GST Reversing as <b>" + reversedSO + "</b>.");
                        result = false;
                    }
                }
            }
            return result;
        }        
        private bool CheckPriceZero()
        {
            /* Prompt warning message for the price 0 of the items added by YST on 2023/04/03 */
            bool result = true; GEnum.MsgBoxButton btnSelect;
            DataTable dtPrice = (from row in objFactory.DocDetItms.AsEnumerable()
                                 where (row.Field<decimal?>("ItmQty") > 0  && row.Field<decimal?>("ItmPriceAfter") == 0 && row.Field<int?>("LineType") == 1000
                                 && GFunc.IsNEZ(row.Field<int?>("ItmBatchKey")))
                                 select new
                                 {
                                     SN = row.Field<decimal?>("ItmSN"),
                                     Marking = row.Field<string>("ItmMark"),
                                     ItemID = row.Field<string>("ItmID"),
                                     Item_Description = row.Field<string>("ItmDes"),
                                     Qty = row.Field<decimal?>("ItmQty"),
                                     Price = row.Field<decimal?>("ItmPriceAfter"),
                                     Amount = row.Field<decimal?>("ItmAmtF"),
                                     Warning_Message = "Price 0 will be under profit margin."
                                 }).AsDataTable();

            if (dtPrice.Rows.Count > 0)
            {
                dtPrice.Columns["Warning_Message"].Caption = "                         " + dtPrice.Columns["Warning_Message"].Caption;                
                btnSelect = MsgBoxGrid.Show("<font color='red'>Are you sure to release the following item(s) with price 0 ?</font>", dtPrice, GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No);
                if (btnSelect == GEnum.MsgBoxButton.Yes)
                    result = true;
                else
                {
                    var row = tagrdDetItms.Rows.FirstOrDefault(r => GFunc.NEInt((r.Cells["ItmQty"].Value),0) > 0 && GFunc.NEInt((r.Cells["ItmPriceAfter"].Value),0) == 0);
                    int rowIndex = row != null ? row.Index : -1;
                    tabDetailList.SelectedTab = tabDetailList.Tabs["tsbItems"];
                    tagrdDetItms.ActiveCell = tagrdDetItms.Rows[rowIndex == -1 ? 0 : rowIndex].Cells["ItmPriceAfter"];
                    tagrdDetItms.PerformAction(UltraGridAction.EnterEditMode);
                    result = false;
                }
            }
            return result;          
        }
        private bool CheckJobID(GEnum.DocAction ButtonAction)
        {
            /* Check Job Link added by YST on 2023/11/30 */
            string MsgJobLink = ButtonAction == GEnum.DocAction.Post ? "<b><font color='red'> The system will not allow you to proceed otherwise.</font></b>" : "";
            DataTable dtJob = (from row in objFactory.DocDetItms.AsEnumerable()
                               where row.Field<int?>("ItmJobKey") == 0 &&                                     
                                     row.Field<int?>("ItmType") < (int)GEnum.ItemType.Header &&
                                     (row.Field<decimal?>("ItmAmtShw") > 0 || row.Field<decimal?>("ItmQty") > 0)
                               select new
                               {
                                   SN = row.Field<decimal?>("ItmSN"),
                                   Marking = row.Field<string>("ItmMark"),
                                   ItemID = row.Field<string>("ItmID"),
                                   ItemDescription = row.Field<string>("ItmDes"),
                                   ItemQty = row.Field<decimal?>("ItmQty"),
                                   ItemPrice = row.Field<decimal?>("ItmPriceAfter"),
                                   ItemAmt = row.Field<decimal?>("ItmAmtShw"),
                                   WarningMessage = "JobID is required."
                               }).AsDataTable();

            if (dtJob.DefaultView.Count > 0)
            {
                int rowIndex = 0;
                dtJob.Columns["ItemID"].Caption = "  Item ID   ";
                dtJob.Columns["ItemDescription"].Caption = "       Item Description        ";
                dtJob.Columns["ItemQty"].Caption = "  Qty   ";
                dtJob.Columns["ItemPrice"].Caption = "  Price   ";
                dtJob.Columns["ItemAmt"].Caption = "  Amount   ";
                dtJob.Columns["WarningMessage"].Caption = "   Warning Message      ";
                MsgJobLink = "<b>Job ID is required for the following item(s).</b>" + MsgJobLink;
                MsgBoxGrid.Show(MsgJobLink, dtJob, GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                rowIndex = tagrdDetItms.Rows.FirstOrDefault(r => r.Cells["ItmSN"].Value.ToString() == dtJob.Rows[0]["SN"].ToString()).Index;
                tabDetailList.SelectedTab = tabDetailList.Tabs["tsbItems"];
                tagrdDetItms.ActiveCell = tagrdDetItms.Rows[rowIndex == -1 ? 0 : rowIndex].Cells["ItmJobKey"];
                tagrdDetItms.PerformAction(UltraGridAction.EnterEditMode);
                return false;
            }

            return true;
        }
        private bool CheckAccCategory()
        {
            /* Check Account Category added by YST on 2023/11/30 */
            string MsgCategory = "<br/><b><font color='red'> The system will not allow you to proceed otherwise.</font></b>";
            string AccDes = "";
            DataTable dtAcc = (from row in objFactory.DocDetItms.AsEnumerable()
                               where row.Field<int?>("ItmAccKey") > 0 && row.Field<string>("ItmAccDes") == "Sales" && /* Athena default sales account => AccKey - 1383 , AccID - 6100, AccDes - Sales */
                                     row.Field<int?>("ItmType") < (int)GEnum.ItemType.Header &&
                                     (row.Field<decimal?>("ItmAmtShw") > 0 || row.Field<decimal?>("ItmQty") > 0)
                               select new
                               {
                                   SN = row.Field<decimal?>("ItmSN"),
                                   Marking = row.Field<string>("ItmMark"),
                                   ItemID = row.Field<string>("ItmID"),
                                   ItemDescription = row.Field<string>("ItmDes"),
                                   ItemAccDes = row.Field<string>("ItmAccDes"),
                                   //ItemQty = row.Field<decimal?>("ItmQty"),
                                   //ItemPrice = row.Field<decimal?>("ItmPriceAfter"),
                                   //ItemAmt = row.Field<decimal?>("ItmAmtShw"),
                                   WarningMessage = "Incorrect Category!"
                               }).AsDataTable();

            if (dtAcc.DefaultView.Count > 0)
            {
                int rowIndex = 0;
                dtAcc.Columns["ItemID"].Caption = "  Item ID   ";
                dtAcc.Columns["ItemDescription"].Caption = "       Item Description        ";
                dtAcc.Columns["ItemAccDes"].Caption = "  Category   ";
                //dtAcc.Columns["ItemQty"].Caption = "  Qty   ";
                //dtAcc.Columns["ItemPrice"].Caption = "  Price   ";
                //dtAcc.Columns["ItemAmt"].Caption = "  Amount   ";
                dtAcc.Columns["WarningMessage"].Caption = "   Warning Message      ";
                AccDes = dtAcc.Rows[0]["ItemAccDes"].ToString();
                MsgCategory = "<b>Please select the correct category for the following item(s) instead of " + AccDes  + ".</b>" + MsgCategory;
                MsgBoxGrid.Show(MsgCategory, dtAcc, GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                rowIndex = tagrdDetItms.Rows.FirstOrDefault(r => r.Cells["ItmSN"].Value.ToString() == dtAcc.Rows[0]["SN"].ToString()).Index;
                tabDetailList.SelectedTab = tabDetailList.Tabs["tsbItems"];
                tagrdDetItms.ActiveCell = tagrdDetItms.Rows[rowIndex == -1 ? 0 : rowIndex].Cells["ItmAccDes"];
                tagrdDetItms.PerformAction(UltraGridAction.EnterEditMode);
                return false;
            }

            return true;
        }
        /* end  by YST */

        private bool OpenRecord(int key, string id)
        {
            this.Cursor = Cursors.WaitCursor;
            GEnum.MsgBoxButton btnSelect = GEnum.MsgBoxButton.Yes;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (id == string.Empty)
                {
                    if (GFunc.IsNEZ(key))
                        return false;
                }

                if (SaveChanges(true, true, true, GEnum.DocAction.Undetermine) == false)
                {
                    return true; //if User clicked "I don't know" or ("Save Changes" which validation is failed, the form will still be opened with dirty state)       
                }

                #region open record
                if (!GFunc.IsNEZ(key) && DocListForm != null)//if called from DocListForm, key is not zero
                    DocListForm.Focus();

                if (SECPermUtility.Edit(objFactory.PermID, false))
                {
                    if (objFactory.GetEdit(key, id) != GVar.gcPass)
                    {
                        if (SysOptionUtility.GetBool("WarnOpenRecordAsReadOnly"))
                        {
                            btnSelect = MsgBox.Show("Try to open record as read only",
                                                  GEnum.MsgBoxIcon.Question,
                                                  GEnum.MsgBoxButton.Yes,
                                                  GEnum.MsgBoxButton.No);
                        }
                        if (btnSelect == GEnum.MsgBoxButton.Yes)
                            objFactory.GetReadOnly(key, id);
                        else
                            return false;
                    }
                }
                else
                    objFactory.GetReadOnly(key, id);

                Form_RefreshAll(false, true);
                btnAttachmentEdit.Text = "Customer PO (" + objFactory.Doc.Attachments.Count(o => o.DocDItm == -1 && o.DocDetailType == 1) + ")";
                
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].Header.Caption = "Latest Cost " + DocCurrKey.Text;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmControlPrice"].Header.Caption = "EStore Price " + DocCurrKey.Text;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmJobKey"].DefaultCellValue = 0;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmAccKey"].DefaultCellValue = 0;
                DefJobKey.SelectedRow = null;
                DefAccKey.SelectedRow = null;
                objFactory.Doc.DefAccKey = 0;

                if (tagrdDetItms.Rows.Count>0)
                {
                    pRow = tagrdDetItms.Rows.OfType<UltraGridRow>().ToList().Find
                        (row => (int)row.Cells["ItmKey"].Value== SysOptionUtility.ProcessingItem/*102937*/);
                   
                }

                //22 dec 2017
                if (objFactory.Doc.DocReqDate != null)
                    RequiredTime.Value = objFactory.Doc.DocReqDate.Value;
                if (objFactory.Doc.PrintDept.ToUpper().Equals("L"))
                    optPrintLog.Checked = true;
                else
                    optPrintSales.Checked = true;
                #endregion
                OpenID.Text = DocID.Value.ToString();
                FilterCustomer(); //added by thettm on 12-sept-2017
                BindPickingList(); //added by YST on 2022/05/05                 
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
                this.Focus();
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void AddItm_Hash()
        {
            try
            {
                htDetailGrd.Clear();
                htDetailGrd.Add(GEnum.Details.Doc_Itm, tagrdDetItms);
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
        private bool CreateDocs(int destination_DC)
        {
            bool runCheckProcess = false;
            bool runCreateProcess = false;

            int spResult = 0;
            frmShowProgress f = null;
            System.Threading.Thread th = null;

            try
            {
                if (SaveChanges(false, true, true, GEnum.DocAction.Undetermine))
                    runCheckProcess = true;
                else
                    return false;                

                #region Check for if Document has been already been created
                if (runCheckProcess)
                {
                    if (objFactory.Doc.DocState == (int)GEnum.DocState.Posted || (destination_DC== (int)GEnum.SystemCode.Quotation))
                    {
                        //frmShowProgress f = new frmShowProgress("                     Checking to copy...");
                        ////frmShowProgress f = new frmShowProgress("      Creating " + ((GEnum.SystemCode)destination_DC).ToString() + "...");
                        //System.Threading.Thread th = new System.Threading.Thread(() => ShowForm(f));
                        //th.Start();

                        DataSet ds = new DataSet();
                        List<SqlParameter> parmList = new List<SqlParameter>();
                        parmList.Add(new SqlParameter("@source_DC", objFactory.Doc.DocCodeKey));
                        parmList.Add(new SqlParameter("@source_DID", objFactory.Doc.DocID));
                        parmList.Add(new SqlParameter("@source_DK", objFactory.Doc.DocKey));
                        parmList.Add(new SqlParameter("@destination_DC", destination_DC));
                        parmList.Add(new SqlParameter("@RetValue", spResult));
                        parmList[4].Direction = ParameterDirection.Output;
                        ds = GFunc.ExecuteProcDataSet("Doc_CreateDocs_Check", parmList);

                        //if (th != null)
                        //{
                        //    f.CloseMe();
                        //    th.Abort();
                        //    th = null;
                        //}

                        #region "commented by KKAung on 09 Jun 2023"
                        /*
                        if (destination_DC == (int)GEnum.SystemCode.Purchase_Order)
                        {
                            //PO to be created.
                            //PO to be created.
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                frmConfirmPO fPO = new frmConfirmPO(ds.Tables[1], (int)objFactory.Doc.DocCodeKey, (int)objFactory.Doc.DocKey, objFactory.Doc.DocID);
                                if (fPO.ShowDialog() == DialogResult.OK)
                                {                                                          
                                    objFactory.GetEdit((int)objFactory.Doc.DocKey, objFactory.Doc.DocID);
                                    Form_RefreshAll(false, true);
                                    frmMain.gfrmMain.ShowExistingPopupForm("frmPrintSelectionList");
                                }
                                return true;
                            }
                            //show Existing PO
                            if (runCreateProcess == false && ds.Tables[0].Rows.Count > 0)
                            {
                                if (MsgBoxGrid.Show("Already Created. Would you like to print " + (ds.Tables[0].Rows.Count == 1 ? "it" : "them") + "?", ds.Tables[0], GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                                {
                                    frmPrintSelectionList fList = new frmPrintSelectionList(GEnum.SystemCode.Purchase_Order, 20, objFactory.Doc.DocID);
                                    fList.Show();
                                }
                                return false;
                            }                            
                        }
                        else
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {   
                                if (MsgBoxGrid.Show("Already Created, Continue?", ds.Tables[0], GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                                {
                                    runCreateProcess = true;                                    
                                }
                                else
                                    return false;
                            }
                            else
                            {
                                runCreateProcess = true;
                            }
                        }
                        */
                        #endregion

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (MsgBoxGrid.Show("Already Created, Continue?", ds.Tables[0], GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                            {
                                runCreateProcess = true;
                            }
                            else
                                return false;
                        }
                        else
                        {
                            runCreateProcess = true;
                        }
                    }
                    else
                    {                        
                        MsgBox.Show("Cannot create Document when sales order has not been posted");
                        return false;
                    }
                }
                #endregion

                #region Create document process
                if (runCreateProcess)
                {
                    // added by KKAung on 09 Jun 2023 (start)
                    if (destination_DC == (int)GEnum.SystemCode.Purchase_Order && SECPermUtility.Edit(GVar.PermissionID.Purchase_Order, true) == false)  
                    {
                        return false;
                    }
                    // (end)
                    try
                    {
                        f = new frmShowProgress("      Creating " + ((GEnum.SystemCode)destination_DC).ToString() + "...");
                        th = new System.Threading.Thread(() => ShowForm(f));
                        th.Start();
                    }
                    catch { }
                    switch (destination_DC)
                    {
                        //case (int)GEnum.SystemCode.Delivery_Order:
                        //    this.Close();
                        //    frmARDO frmARDO = new frmARDO(objFactory.Doc.DocCodeKey, objFactory.Doc.DocKey, isCash);
                        //    frmARDO.MdiParent = frmMain.gfrmMain;
                        //    frmARDO.Show();
                        //    break;

                        //case (int)GEnum.SystemCode.Sales_Invoice:
                        //    this.Close();
                        //    frmARIV frmIV = new frmARIV(objFactory.Doc.DocCodeKey, objFactory.Doc.DocKey);
                        //    frmIV.MdiParent = frmMain.gfrmMain;
                        //    frmIV.Show();
                        //    break;
                         
                        // added by thettm on 29 jan 2018 (start)
                        case (int)GEnum.SystemCode.Quotation:
                            this.Close();
                            frmARQO frmQO = new frmARQO(objFactory.Doc.DocCodeKey, objFactory.Doc.DocKey);
                            frmQO.MdiParent = frmMain.gfrmMain;
                            frmQO.Show();
                            break;                   
                        
                    }

                    //if (th != null)
                    //{
                    //    f.CloseMe();
                    //    th.Abort();
                    //    th = null;
                    //}
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
            finally
            {
                try
                {
                    if (th != null)
                    {
                        f.CloseMe();
                        //th.Abort();
                        th = null;
                    }
                }
                catch
                { }
            }

            return true;
        }//Completed

        //Notifier
        private void DocNotifier_Set(object sender, BOLib.UINotifierEventArgs e)
        {
            try
            {
                DocComUtility.Notifier_CtrlSearch(this, e, errorProvider1);
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
        private void DocNotifier_ClearErr(object sender, BOLib.UINotifierEventArgs e)
        {
            DocComUtility.ClearErrNotifier(this, e, errorProvider1);
        }//Completed

        //Set Error Methods
        private void OnDataError(object sender, TAUtil.TAErrorEventArgs e)
        {
            try
            {
                if (e.ErrorCode == TAUtil.TAErrorCode.NOT_IN_LIST)
                {
                    if (tagrdDetItms.ActiveCell.Column.EditorComponent != null)
                    {
                        tagrdDetItms.PerformAction(UltraGridAction.EnterEditMode);

                        if (tagrdDetItms.ActiveCell.Column.EditorComponent.GetType() == typeof(TAUtil.TAComboBox))
                        {
                            TAUtil.TAComboBox taCombo = (TAUtil.TAComboBox)tagrdDetItms.ActiveCell.Column.EditorComponent;
                            taCombo.Text = tagrdDetItms.ActiveCell.Text;

                            switch (tagrdDetItms.ActiveCell.Column.Key.ToLower())
                            {
                                case "itmdeptkey":
                                case "itmtrangrpkey":
                                case "itmacckey":
                                case "itmlockey":
                                case "itmuomkey":
                                case "itmtaxgrpkey":
                                case "itmcolorkey":
                                case "itmvendorkey":
                                case "itmvendorcurrkey":
                                case "itmjobkey":
                                case "itmjobphasekey":
                                case "itmjobtaskkey":
                                case "itmjobcosttypekey":
                                    GlobalUI.ItemNotInList(tagrdDetItms.ActiveCell, null, 1);// ItemNotInListAdd
                                    break;
                                default:
                                    GlobalUI.ItemNotInList(tagrdDetItms.ActiveCell, null, 0);
                                    break;
                            }
                        }
                        else
                        {
                            GlobalUI.ItemNotInList(tagrdDetItms.ActiveCell, null, 0);
                        }
                    }
                }
                else
                {
                    MsgBox.Show(e.ErrorMessage);
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
        //Mic Check ; Jack Added 3 Dec 2012
        private void btnOrderByMarking_Click(object sender, EventArgs e)
        {
            try
            {
                DocDetUtil.DetItm_OrderByMarking(objFactory.DocDetItms, tagrdDetItms);
                AddItm_Hash();
                bool result = DocComUtility.CalForm(objFactory.Doc, htDetailGrd, true, false);
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

        private void DocReqDate_CustomUpdate(object sender, CancelEventArgs e)
        {
            /* ItmPrmDate also should follow DocReqDate like ItmReqDate because all reports and lists show ItmPrmDate only. 
             * requested by Feliani , added by YST , discussed and comfirmed by May on 2023/05/30 */
            try
            {
                DocReqDate.DateValue = GFunc.NEDateTime(DocReqDate.DateValue, DateTime.Today);

                if (objFactory.DocDetItms != null && objFactory.DocDetItms.Rows.Count > 0)
                {
                    int diffDateCount = objFactory.DocDetItms.AsEnumerable()
                            .Where(x => x["ItmPrmDate"] != DBNull.Value && Convert.ToDateTime(x["ItmPrmDate"]) != DocReqDate.DateValue)
                            .Select(r => r.Field<DateTime>("ItmPrmDate"))
                            .Distinct()
                            .Count();

                    if (diffDateCount > 0 && MsgBox.Show("Apply required date to all details!", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                    {
                        foreach (DataRow row in objFactory.DocDetItms.Rows)
                        {
                            row["ItmPrmDate"] = DocReqDate.DateValue;
                            row["ItmReqDate"] = DocReqDate.DateValue;
                        }
                        objFactory.DocDetItms.AcceptChanges();
                    }

                    /* Apply Warranty End Date  */
                    MSTItm objItm; double salesWrtyDays = 0.0;
                    foreach (DataRow row in objFactory.DocDetItms.Rows)
                    {
                        objItm = MSTItm.Get(row["ItmID"].ToString());
                        if (objItm.SalesWrtyYr > 0)
                        {
                            salesWrtyDays = Convert.ToDouble(objItm.SalesWrtyYr * 365);
                            row["ItmWrtyEndDate"] = ((DateTime)DocReqDate.DateValue).AddDays(salesWrtyDays);
                        }
                    }
                }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }

        private void DocPrmDate_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                DocPrmDate.DateValue = GFunc.NEDateTime(DocPrmDate.DateValue, DateTime.Today);

                if(objFactory.DocDetItms!=null)
                    if(objFactory.DocDetItms.Rows.Count>0)
                        if (MsgBox.Show("Apply default promised date to all details", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                        {
                   
                            foreach (DataRow row in objFactory.DocDetItms.Rows)
                            {
                                row["ItmPrmDate"] = DocPrmDate.DateValue;
                            }

                            objFactory.DocDetItms.AcceptChanges();                    
                        }
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }

        private void DocEmKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@DocEmKey", GFunc.NEInt(DocEmKey.Value, 0)));
            parmList.Add(new SqlParameter("@DocConKey", GFunc.NEInt(DocConKey.Value, 0)));
            parmList.Add(new SqlParameter("@DocTranGrpKey", GFunc.NEInt(DocTranGrpKey.Value, 0)));
            parmList.Add(new SqlParameter("@EmInActvie", SqlDbType.Bit));
            parmList.Add(new SqlParameter("@TranGrpKey", SqlDbType.Int));
            parmList.Add(new SqlParameter("@SalesRepAsHeadSales", SqlDbType.Bit));
            parmList.Add(new SqlParameter("@SalesRepTeam", SqlDbType.NVarChar, 50));
            parmList.Add(new SqlParameter("@HeadSalesTeam", SqlDbType.NVarChar, 50));

            parmList[3].Direction = ParameterDirection.Output;
            parmList[4].Direction = ParameterDirection.Output;
            parmList[5].Direction = ParameterDirection.Output;
            parmList[6].Direction = ParameterDirection.Output;
            parmList[7].Direction = ParameterDirection.Output;


            GFunc.ExecuteNonQueryProc("Doc_SalesRepGetCheckData", parmList);

            if (GFunc.NEBool(parmList[3].Value, false))
            {
                MsgBox.Show("The sale representative is inactive. Please select another one.");
                e.Cancel = true;
            }
            if (GFunc.IsNEZ(DocTranGrpKey.Value) || GFunc.NEBool(parmList[5].Value, false))
            {
                DocTranGrpKey.SetValueTrigger(GFunc.NEInt(parmList[4].Value, 0), false);
            }
            Custom4.SetValueTrigger(GFunc.NEStr(parmList[6].Value, ""), false);
            Custom5.SetValueTrigger(GFunc.NEStr(parmList[7].Value, ""), false);
            //if (MSTSalesRep.Get(GFunc.NEInt(DocEmKey.Value, 0)).Inactive.Value)
            //{
            //    MsgBox.Show("The sale representative is inactive. Please select another one.");
            //    e.Cancel = true;
            //    return;
            //}

            //if(GFunc.IsNEZ(DocTranGrpKey.Value))
            //{
            //    MSTAccTranGrp t = MSTAccTranGrp.Get(DocEmKey.Text, 3);
            //    if (t != null)
            //        if (!GFunc.IsNEZ(t.TranGrpKey))
            //        {
            //            DocTranGrpKey.SetValueTrigger(t.TranGrpKey, false);
            //        }
            //} 
        }
        private void updateEStoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
        //    try
        //    {
        //        if (SaveChanges(true, true, true, GEnum.DocAction.Undetermine) == false)
        //        {
        //            return; //if User clicked "I don't know" or ("Save Changes" which validation is failed, the form will still be opened with dirty state)                   
        //        }
        //        if (objFactory.Doc.DocQuoteStatus == 20)
        //        {
        //            MsgBox.Show("Customer has already confirmed the quotation. It cannot be updated.");
        //            return;
        //        }
        //        this.Cursor = Cursors.WaitCursor;

        //        if (objFactory.Doc.DocRef != "")
        //        {
        //            bool proceed = true;
        //            string subject = "";
        //            string emailBody = "";
        //            int quoteID = 0;

        //            if (objFactory.Doc.Custom3 == "Replied")
        //            {
        //                if (MsgBox.Show("The quotation has been replied before.\nAre you sure to update it?", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
        //                {
        //                    proceed = false;
        //                }
        //            }


        //            if (proceed)
        //            {
        //                string skus = "";
        //                string data = "";
        //                string ids = "";
        //                string rem = "";
        //                decimal bankchg = 0M;
        //                decimal delchg = 0M;
        //                ttm
        //                decimal paypalchg = 0M;
        //                string delchgDesc = "";
        //                MSTSalesRep objSR = MSTSalesRep.Get(objFactory.Doc.DocEmKey);
        //                **temp
        //                MySqlConnection con = new MySqlConnection("userid=bhestore_may;password=Thinzar@12;server=101.100.209.196;database=bhestore_magento18jul;connection timeout=180");
        //                MySqlConnection con = new MySqlConnection("userid=root;password=;server=localhost;database=bhestore_magentojan18;connection timeout=180");

        //                foreach (UltraGridRow row in tagrdDetItms.Rows)
        //                {
        //                    if (row.Cells["ItmID"].Text.ToUpper().Equals("BANK CHARGES"))
        //                    {
        //                        bankchg = GFunc.NEDec(row.Cells["ItmAmtF"].Value, 0);
        //                    }
        //                    else if (row.Cells["ItmID"].Text.ToUpper().Equals("FREIGHT CHARGES") || row.Cells["ItmID"].Text.ToUpper().Equals("F FREIGHT"))
        //                    {
        //                        delchg = GFunc.NEDec(row.Cells["ItmAmtF"].Value, 0);
        //                        delchgDesc = GFunc.NEStr(row.Cells["ItmDes"].Value, "");
        //                    }
        //                    ttm
        //                    else if (row.Cells["ItmID"].Text.ToUpper().Equals("PROCESSING FEE") || row.Cells["ItmID"].Text.ToUpper().Contains("PAYPAL"))
        //                    {
        //                        paypalchg = GFunc.NEDec(row.Cells["ItmAmtF"].Value, 0);
        //                    }
        //                    else if (row.Cells["ItmIGrpDItm"].Text.Replace(",", "") != "0" && row.Cells["ItmIGrpDItm"].Text.Replace(",", "") != "")
        //                    {
        //                        ids += row.Cells["ItmIGrpDItm"].Text.Replace(",", "") + ",";
        //                        skus += row.Cells["ItmID"].Text.Replace(",", "") + ",";
        //                        data += row.Cells["ItmQty"].Text.Replace(",", "") + "," + row.Cells["ItmUOMKey"].Text.Replace(",", "") + "," + row.Cells["ItmPriceUser"].Text.Replace(",", "") + "," + row.Cells["ItmAmtShw"].Text.Replace(",", "") +
        //                          "," + row.Cells["Custom1"].Text.Replace(",", "") + "," + row.Cells["Custom2"].Text.Replace(",", "") + "," + row.Cells["ItmDes"].Text.Replace(",", "").Replace("\n\r", "</br>") + ","
        //                          + row.Cells["ItmMark"].Text.Replace(",", "") + "," + row.Cells["ItmID"].Text.Replace(",", "") + "#%#";
        //                    }
        //                    else if (GFunc.NEInt(row.Cells["ItmType"].Value, 0) < 700)
        //                    {
        //                        MySqlCommand cmd = new MySqlCommand("SELECT entity_id FROM bhestore_magento18jul.catalog_product_entity where sku='" + row.Cells["ItmID"].Text + "';", con);

        //                        con.Open();

        //                        MySqlDataReader reader = cmd.ExecuteReader();

        //                        if (reader.Read())
        //                        {
        //                            ids += reader.GetInt32("entity_id") + ",";
        //                        }
        //                        else if (row.Cells["ItmID"].Text.ToUpper().Equals("BANK CHARGES") == false)
        //                        {
        //                            ids += "99999,";
        //                        }

        //                        skus += row.Cells["ItmID"].Text.Replace(",", "").Replace(",", "") + ",";
        //                        data += row.Cells["ItmQty"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["ItmUOMKey"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["ItmPriceUser"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["ItmAmtShw"].Text.Replace(",", "").Replace(",", "") +
        //                          "," + row.Cells["Custom1"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["Custom2"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["ItmDes"].Text.Replace(",", "").Replace("\n\r", "</br>")
        //                          + "," + row.Cells["ItmMark"].Text.Replace(",", "") + "," + row.Cells["ItmID"].Text.Replace(",", "") + "#%#";
        //                        con.Close();

        //                    }
        //                    else
        //                        rem = rem + row.Cells["ItmDes"].Text.Replace("\n\r", "</br>") + "</br>";
        //                }
        //                if (ids.Length > 0)
        //                {
        //                    skus = skus.Remove(skus.Length - 1);
        //                    data = data.Remove(data.Length - 3);
        //                    ids = ids.Remove(ids.Length - 1);
        //                }
        //                else
        //                {
        //                    MsgBox.Show("No data to reply.");
        //                    return;
        //                }

        //                rem = GFunc.NEStr(objFactory.Doc.DocRem, "").Replace("\n\r", "</br>");

        //                ttm
        //                string sql = "update `netgo_boss_quoteconfirm` inner join `netgo_boss_quote` on netgo_boss_quoteconfirm.quote_entity_id=netgo_boss_quote.entity_id set " +
        //                      "`sales_id`='" + objSR.EmID + "'," +
        //                          "`salesrep_email`='" + objSR.Custom1 + "' where so_num='" + objFactory.Doc.DocQONum + "'";


        //                MySqlCommand cmd1 = new MySqlCommand(sql, con);
        //                cmd1.CommandType = CommandType.Text;
        //                con.Open();
        //                cmd1.ExecuteNonQuery();
        //                con.Close();

        //                ttm
        //                string sql1 = "update `netgo_boss_quoteconfirm` set " +
        //                          "`comment`='" + objFactory.Doc.DocRem + "'," +
        //                            "`sales_confirm_date`='" + objFactory.Doc.DocDate + "'," +
        //                             "`status`='salesconfirmed'," +
        //                                "`bank_charges`=" + bankchg + "," +
        //                                 "`delivery_charges`=" + delchg + "," +
        //                               "`sub_total`='" + (objFactory.Doc.DocSubTotal - bankchg - delchg) + "'," +
        //                                "`gst_percent`='" + objFactory.Doc.DocTaxGrpRate + "'," +
        //                                 "`gst_amount`='" + objFactory.Doc.DocTaxTotal + "'," +
        //                                  "`pay_pal_fee`=" + paypalchg + "," +
        //                                     "`payment_mode`='" + objFactory.Doc.DocRemPayment + "'," +
        //                                  "`grand_total`='" + objFactory.Doc.DocGrand + "'," +
        //                                   "`items_ids`='" + ids + "'," +
        //                                    "`items_ids`='" + ids + "'," +
        //                                    "`items_details`='" + data + "'," +
        //                 "`curr_id`='" + DocCurrKey.Text + "',`delivery_chargesdesc`='" + delchgDesc + "' where so_num='" + objFactory.Doc.DocQONum + "'";


        //                MySqlCommand cmd2 = new MySqlCommand(sql1, con);
        //                cmd2.CommandType = CommandType.Text;
        //                con.Open();
        //                cmd2.ExecuteNonQuery();
        //                con.Close();

        //                MsgBox.Show("Sale Order updated successfullly in EStore.");
        //            }
        //        }
        //        else
        //            MsgBox.Show("This sale order is not linked to any EStore Quotation.");
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        this.Cursor = Cursors.Default;
        //    }
        }
        //private void updateEStoreToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (SaveChanges(true, true, true, GEnum.DocAction.Undetermine) == false)
        //        {
        //            return; //if User clicked "I don't know" or ("Save Changes" which validation is failed, the form will still be opened with dirty state)                   
        //        }
        //        //if (objFactory.Doc.DocQuoteStatus == 20)
        //        //{
        //        //    MsgBox.Show("Customer has already confirmed the quotation. It cannot be updated.");
        //        //    return;
        //        //}
        //        this.Cursor = Cursors.WaitCursor;

        //        if (objFactory.Doc.DocRef != "")
        //        {
        //            bool proceed = true;
        //            string subject = "";
        //            string emailBody = "";
        //            int quoteID = 0;

        //            if (objFactory.Doc.Custom3 == "Replied")
        //            {
        //                if (MsgBox.Show("The quotation has been replied before.\nAre you sure to update it?", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
        //                {
        //                    proceed = false;
        //                }
        //            }


        //            if (proceed)
        //            {
        //                string skus = "";
        //                string data = "";
        //                string ids = "";
        //                string rem = "";
        //                decimal bankchg = 0M;
        //                decimal delchg = 0M;
        //                //ttm
        //                decimal paypalchg = 0M;
        //                string delchgDesc = "";
        //                MSTSalesRep objSR = MSTSalesRep.Get(objFactory.Doc.DocEmKey);
        //                //**temp
        //              MySqlConnection con = new MySqlConnection("userid=bhestore_may;password=Thinzar@12;server=101.100.209.196;database=bhestore_magento18jul;connection timeout=180");
        //           //  MySqlConnection con = new MySqlConnection("userid=root;password=;server=localhost;database=bhestore_magento18jul;connection timeout=180");

        //                foreach (UltraGridRow row in tagrdDetItms.Rows)
        //                {
        //                    if (row.Cells["ItmID"].Text.ToUpper().Equals("BANK CHARGES"))
        //                    {
        //                        bankchg = GFunc.NEDec(row.Cells["ItmAmtF"].Value, 0);
        //                    }
        //                    else if (row.Cells["ItmID"].Text.ToUpper().Equals("FREIGHT CHARGES") || row.Cells["ItmID"].Text.ToUpper().Equals("F FREIGHT"))
        //                    {
        //                        delchg = GFunc.NEDec(row.Cells["ItmAmtF"].Value, 0);
        //                        delchgDesc = GFunc.NEStr(row.Cells["ItmDes"].Value, "");
        //                    }
        //                    //ttm
        //                    else if (row.Cells["ItmID"].Text.ToUpper().Equals("PROCESSING FEE") || row.Cells["ItmID"].Text.ToUpper().Contains("PAYPAL"))
        //                    {
        //                        paypalchg = GFunc.NEDec(row.Cells["ItmAmtF"].Value, 0);
        //                    }
        //                    else if (row.Cells["ItmIGrpDItm"].Text.Replace(",", "") != "0" && row.Cells["ItmIGrpDItm"].Text.Replace(",", "") != "")
        //                    {
        //                        ids += row.Cells["ItmIGrpDItm"].Text.Replace(",", "") + ",";
        //                        skus += row.Cells["ItmID"].Text.Replace(",", "") + ",";
        //                        data += row.Cells["ItmQty"].Text.Replace(",", "") + "," + row.Cells["ItmUOMKey"].Text.Replace(",", "") + "," + row.Cells["ItmPriceUser"].Text.Replace(",", "") + "," + row.Cells["ItmAmtShw"].Text.Replace(",", "") +
        //                          "," + row.Cells["Custom1"].Text.Replace(",", "") + "," + row.Cells["Custom2"].Text.Replace(",", "") + "," + row.Cells["ItmDes"].Text.Replace(",", "").Replace("\n\r", "</br>") + ","
        //                          + row.Cells["ItmMark"].Text.Replace(",", "") + "," + row.Cells["ItmID"].Text.Replace(",", "") + "#%#";
        //                    }
        //                    else if (GFunc.NEInt(row.Cells["ItmType"].Value, 0) < 700)
        //                    {
        //                        MySqlCommand cmd = new MySqlCommand("SELECT entity_id FROM bhestore_magento18jul.catalog_product_entity where sku='" + row.Cells["ItmID"].Text + "';", con);

        //                        con.Open();

        //                        MySqlDataReader reader = cmd.ExecuteReader();

        //                        if (reader.Read())
        //                        {
        //                            ids += reader.GetInt32("entity_id") + ",";
        //                        }
        //                        else if (row.Cells["ItmID"].Text.ToUpper().Equals("BANK CHARGES") == false)
        //                        {
        //                            ids += "99999,";
        //                        }

        //                        skus += row.Cells["ItmID"].Text.Replace(",", "").Replace(",", "") + ",";
        //                        data += row.Cells["ItmQty"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["ItmUOMKey"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["ItmPriceUser"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["ItmAmtShw"].Text.Replace(",", "").Replace(",", "") +
        //                          "," + row.Cells["Custom1"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["Custom2"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["ItmDes"].Text.Replace(",", "").Replace("\n\r", "</br>")
        //                          + "," + row.Cells["ItmMark"].Text.Replace(",", "") + "," + row.Cells["ItmID"].Text.Replace(",", "") + "#%#";
        //                        con.Close();

        //                    }
        //                    else
        //                        rem = rem + row.Cells["ItmDes"].Text.Replace("\n\r", "</br>") + "</br>";
        //                }
        //                if (ids.Length > 0)
        //                {
        //                    skus = skus.Remove(skus.Length - 1);
        //                    data = data.Remove(data.Length - 3);
        //                    ids = ids.Remove(ids.Length - 1);
        //                }
        //                else
        //                {
        //                    MsgBox.Show("No data to reply.");
        //                    return;
        //                }

        //                rem = GFunc.NEStr(objFactory.Doc.DocRem, "").Replace("\n\r", "</br>");

        //                //ttm
        //                string sql = "update `netgo_boss_quoteconfirm` inner join `netgo_boss_quote` on netgo_boss_quoteconfirm.quote_entity_id=netgo_boss_quote.entity_id set " +
        //                      "`sales_id`='" + objSR.EmID + "'," +
        //                          "`salesrep_email`='" + objSR.Custom1 + "' where so_num='" + objFactory.Doc.DocID + "'";


        //                MySqlCommand cmd1 = new MySqlCommand(sql, con);
        //                cmd1.CommandType = CommandType.Text;
        //                con.Open();
        //                cmd1.ExecuteNonQuery();
        //                con.Close();

        //                //ttm
        //                string sql1 = "update `netgo_boss_quoteconfirm` set " +                             
        //                          "`comment`='" + objFactory.Doc.DocRem + "'," +
        //                            "`sales_confirm_date`='" + objFactory.Doc.DocDate + "'," +
        //                             "`status`='salesconfirmed'," +
        //                                "`bank_charges`=" + bankchg + "," +
        //                                 "`delivery_charges`=" + delchg + "," +
        //                               "`sub_total`='" + (objFactory.Doc.DocSubTotal - bankchg - delchg) + "'," +
        //                                "`gst_percent`='" + objFactory.Doc.DocTaxGrpRate + "'," +
        //                                 "`gst_amount`='" + objFactory.Doc.DocTaxTotal + "'," +
        //                                  "`pay_pal_fee`=" + paypalchg + "," +
        //                                     "`payment_mode`='" + objFactory.Doc.DocRemPayment + "'," +
        //                                  "`grand_total`='" + objFactory.Doc.DocGrand + "'," +
        //                                   "`items_ids`='" + ids + "'," +
        //                                    "`items_ids`='" + ids + "'," +
        //                                    "`items_details`='" + data + "',"+
        //                 "`curr_id`='" + DocCurrKey.Text + "',`delivery_chargesdesc`='" + delchgDesc + "' where so_num='" + objFactory.Doc.DocID + "'";


        //                MySqlCommand cmd2 = new MySqlCommand(sql1, con);
        //                cmd2.CommandType = CommandType.Text;
        //                con.Open();
        //                cmd2.ExecuteNonQuery();
        //                con.Close();

        //                MsgBox.Show("Sale Order updated successfullly in EStore.");
        //            }
        //        }
        //        else
        //            MsgBox.Show("This sale order is not linked to any EStore Quotation.");
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        this.Cursor = Cursors.Default;
        //    }
        //}

        private void checkSOInEStoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
        //    //if (DocRemPayment.Text.Equals("Pay Pal"))
        //    //    System.Diagnostics.Process.Start("https://bh-estore.com/netgo_boss/quote/poview/id/" + objFactory.Doc.Custom2);
        //    //else
        //    //    System.Diagnostics.Process.Start("https://bh-estore.com/netgo_boss/quote/posubmit/id/" + objFactory.Doc.Custom2);

        //    //**temp
        //    if (DocRemPayment.Text.Equals("Pay Pal"))
        //        System.Diagnostics.Process.Start(link+"netgo_boss/quote/poview/id/" + objFactory.Doc.Custom2);
        //    else
        //        System.Diagnostics.Process.Start(link + "netgo_boss/quote/posubmit/id/" + objFactory.Doc.Custom2);
        }

        private void confirmLinkToCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
        //    try
        //    {
        //        if (SaveChanges(true, true, true, GEnum.DocAction.Undetermine) == false)
        //        {
        //            return; //if User clicked "I don't know" or ("Save Changes" which validation is failed, the form will still be opened with dirty state)                   
        //        }               

        //        this.Cursor = Cursors.WaitCursor;
        //        if (objFactory.Doc.DocRef != "")
        //        {
        //            bool proceed = false;
        //            string subject = "";
        //            string emailBody = "";
        //            int quoteID = 0;

        //            if (objFactory.Doc.Custom3 == "Replied")
        //            {
        //                if (MsgBox.Show("The order confirmation has been replied before.\nAre you sure to send this again to " + objFactory.Doc.DocBAddrAttn + "?", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
        //                {
        //                    subject = "BH eStore - Order Confirmation " + objFactory.Doc.DocRef + " (Updated)";
        //                    proceed = true;
        //                }
        //            }
        //            else if (MsgBox.Show("Are you sure to reply order confirmation to " + objFactory.Doc.DocBAddrAttn + "?", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
        //            {
        //                subject = "BH eStore - Order Confirmation " + objFactory.Doc.DocRef;
        //                proceed = true;
        //            }

        //            if (proceed)
        //            {
        //                if (!objFactory.Doc.Custom3.Equals("Replied"))
        //                {
        //                    List<SqlParameter> parmList = new List<SqlParameter>();
        //                    parmList.Add(new SqlParameter("@DocKey", objFactory.Doc.DocKey));
        //                    GFunc.ExecuteNonQueryProc("Doc_UpdateEQuoteStatus", parmList);
        //                    objFactory.Doc.Custom3 = "Replied";
        //                    objFactory.Doc.IsDirty = false;
        //                }
        //                string salesemail = MSTSalesRep.Get(objFactory.Doc.DocEmKey).Custom1;
        //                if (salesemail == "")
        //                    salesemail = "estore@benghui.com";
        //                frmMain.gfrmMain.SetNotifyStatus("Sending Order Confirmation email ...............");

        //                string imglink = link+"skin/frontend/default/bhglobal/images/OrderRoute/Email_SalesPOConfirm.JPG";

        //                if (!objFactory.Doc.DocQONum.Trim().StartsWith("eQO"))
        //                    imglink = link+"skin/frontend/default/bhglobal/images/OrderRoute/Email_SalesPOConfirm1.JPG";

        //                string msg1="";
        //                if (objFactory.Doc.DocRemDelivery == "Self Collection")
        //                    msg1 = "";
        //                else
        //                    msg1 = "Reserved Items are ready for delivery";

        //                if (DocRemPayment.Text.Equals("Pay Pal"))
        //                    emailBody =
        //                    "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>Dear " + objFactory.Doc.DocBAddrAttn + ",</p> " +
        //                    "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
        //                    "We will process your order as soon as your payment clears. " + msg1 + ".</p>" +
        //                      @"</br> <img src='"+imglink+"' border='0'>" +
        //                       "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +    
        //                    "To complete the processing of your order, please proceed for payment.</p>" +                           
        //                     "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +  
        //                     @"Please click <a href='"+link+"netgo_boss/quote/poview/id/" + objFactory.Doc.Custom2 + "'>here</a> to view your order.</p>" +                         
        //                  "<p style='padding-top:20px;padding-left:10px;padding-bottom:0px;width:'100%';font:Arial;font-size:10px;>" +
        //                  "From <b>BH eStore Team</b> of Beng Hui Marine Electrical Pte Ltd</p>" +
        //                  @"<p style='padding-top:0px;padding-bottom:0px;'>Contact us at <b>+65 6291 4444</b> (Monday to Friday)</p>" +
        //                 @" </br> <a href='"+link+"' target='_blank'><img src='"+link+"media/ackemail.jpg' border='0' height='181' width='550'></a> </p>";
        //                else if (DocRemPayment.Text.Equals("Cash Payment"))
        //                {
        //                    emailBody =
        //                    "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>Dear " + objFactory.Doc.DocBAddrAttn + ",</p> " +
        //                    "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
        //                    "We will process your order as soon as your payment clears. " + msg1 + ".</p>" +
        //                      @"</br> <img src='" + imglink + "' border='0'>" +
        //                       "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +  
        //                     @"Please click <a href='"+link+"netgo_boss/quote/poview/id/" + objFactory.Doc.Custom2 + "'>here</a> to view your order.</p>" +
        //                  "<p style='padding-top:20px;padding-left:10px;padding-bottom:0px;width:'100%';font:Arial;font-size:10px;>" +
        //                  "From <b>BH eStore Team</b> of Beng Hui Marine Electrical Pte Ltd</p>" +
        //                  @"<p style='padding-top:0px;padding-bottom:0px;'>Contact us at <b>+65 6291 4444</b> (Monday to Friday)</p>" +
        //                 @" </br> <a href='"+link+"' target='_blank'><img src='"+link+"ackemail.jpg' border='0' height='181' width='550'></a> </p>";
        //                }
        //                else //if (DocRemPayment.Text.Equals("TT"))
        //                    emailBody =
        //                     "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;'>Dear " + objFactory.Doc.DocBAddrAttn + ",</p> " +
        //                    "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
        //                    "We will process your order as soon as your payment clears. " + msg1 + ".</p>" +
        //                     @"<img src='" + imglink + "' border='0'> " +
        //                       "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
        //                    "To complete the processing of your order, please proceed for payment <a href='"+link+"media/BHM -MB SGD Bank Detail.pdf'>Bank Detail</a>.</p>" +
        //                    "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
        //                    "Once payment is made, please email the payment slip to <a href='mailto:" + salesemail + "'>" + salesemail + "</a>.</p>" +
        //                     "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
        //                     @"Please click <a href='"+link+"netgo_boss/quote/posubmit/id/" + objFactory.Doc.Custom2 + "'>here</a> to view your order.</p>" +
        //                "<p style='padding-top:20px;padding-left:10px;padding-bottom:0px;width:'100%';font:Arial;font-size:10px;>" +
        //              "From <b>BH eStore Team</b> of Beng Hui Marine Electrical Pte Ltd</p>" +
        //              @"<p style='padding-top:0px;padding-bottom:0px;'>Contact us at <b>+65 6291 4444</b> (Monday to Friday)</p>" +
        //             @" </br> <a href='"+link+"' target='_blank'><img src='"+link+"media/ackemail.jpg' border='0' height='181' width='550'></a> </p>";                    


        //                GEmail.SendEmail(objFactory.Doc.DocBAddrEmail, subject, emailBody, null);
        //                frmMain.gfrmMain.SetNormalStaus("Ready");
        //                MsgBox.Show("Email has been sent to " + objFactory.Doc.DocBAddrAttn);
        //            }
        //        }
        //        else
        //            MsgBox.Show("This quotation is not linked to any EStore RFQ.");
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        this.Cursor = Cursors.Default;
        //    }
        }
       

        private void tsbUnlockSO_Click(object sender, EventArgs e)
        {
            //objFactory.
        }

        // added by thettm on 29 jan 2018 (start)
        private void tsbCreatePf_Click(object sender, EventArgs e)
        {
            try
            {
                if (SECPermUtility.Add(objFactory.PermID, true))
                    CreateDocs((int)GEnum.SystemCode.Quotation);
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

        

        // added by thettm on 29 jan 2018 (start)
        //Function has NOT been match with TBS  --------------------------PLEASE CREATE NEW FUNCTION BELOW THIS LINE-----------------------------------------------
        private void mnuLinkPO_Click(object sender, EventArgs e)
        {
            int DocKey = 0;
            int DocItmKey = 0;

            if (tagrdDetItms.ActiveRow != null)
            {
                string[] nslink = GFunc.NEStr(tagrdDetItms.ActiveRow.Cells["NSLink"].Value, "").Split('-');
                if (nslink.Length >= 3)

                    if (nslink[0] == "13250")
                    {
                        DocKey = GFunc.NEInt(nslink[1], 0);
                        DocItmKey = GFunc.NEInt(nslink[2], 0);
                    }
                    else
                    {
                        DataTable dt = GetDocKeyByNSLink(GFunc.NEStr(tagrdDetItms.ActiveRow.Cells["NSLink"].Value, ""), true);

                        if (dt.Rows.Count > 0)
                        {
                            DocKey = GFunc.NEInt(dt.Rows[0]["DocKey"], 0);
                            DocItmKey = GFunc.NEInt(dt.Rows[0]["DocItmKey"], 0);
                        }
                    }
            }
            frmInsertSalesPO f = new frmInsertSalesPO(objFactory.Doc, tagrdDetItms, DocKey, DocItmKey);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
        }

        private void mnuLinkBill_Click(object sender, EventArgs e)
        {
            int DocKey = 0;
            int DocItmKey = 0;

            if (tagrdDetItms.ActiveRow != null)
            {
                string[] nslink = GFunc.NEStr(tagrdDetItms.ActiveRow.Cells["NSLink"].Value, "").Split('-');
                if (nslink.Length >= 3)

                    if (nslink[0] == "13500")
                    {
                        DocKey = GFunc.NEInt(nslink[1], 0);
                        DocItmKey = GFunc.NEInt(nslink[2], 0);
                    }
                    else
                    {
                        DataTable dt = GetDocKeyByNSLink(GFunc.NEStr(tagrdDetItms.ActiveRow.Cells["NSLink"].Value, ""), false);

                        if (dt.Rows.Count > 0)
                        {
                            DocKey = GFunc.NEInt(dt.Rows[0]["DocKey"], 0);
                            DocItmKey = GFunc.NEInt(dt.Rows[0]["DocItmKey"], 0);
                        }
                    }
            }
            frmInsertSalesBL f = new frmInsertSalesBL(objFactory.Doc, tagrdDetItms, DocKey, DocItmKey);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
        }

        //added by nnt on 5 Aug 2020
        #region CheckStatus 
        //private void Check()
        //{
        //    if (isCash) return;

        //    //if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
        //    if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM || SysOptionUtility.DatabaseBranchCode == DBCode.ADL
        //   || SysOptionUtility.DatabaseBranchCode == DBCode.SOP || SysOptionUtility.DatabaseBranchCode == DBCode.BOS)
        //    {

        //            lblSOStatus.Text = "";
        //            txtCrSO.Text = "";
        //            DocConKey.Appearance.BackColor = System.Drawing.Color.White;
        //            DocConNm.Appearance.BackColor = System.Drawing.Color.White;
        //            txtCrSO.Appearance.ForeColor = Color.Blue;
        //            lblSOStatus.Appearance.ForeColor = Color.Blue;

        //            CheckSOAllStatus();
        //            CheckSOCustomerSatus();

        //            if (OrangeCus == true && CheckState != 100 && DocHome.Text.ToString() != "0.00")
        //            {

        //                DocConKey.Appearance.BackColor = System.Drawing.Color.DarkOrange;
        //                DocConNm.Appearance.BackColor = System.Drawing.Color.Orange;
        //                //***tsbSave.Enabled = false;
        //                tsbPrint.Enabled = false;
        //                tsbCreateDO.Enabled = false;

        //                if (CheckApproval == 1 && CheckRejected == true )
        //                {
        //                    //MsgBox.Show("This customer was rejected by COO. If you need to request approval from COO, click on release button again.");
        //                    //tsbSave.Enabled = false;
        //                    tsbDraft.Enabled = true;
        //                    tsbCreateDO.Enabled = false;
        //                    tsbCreatePO.Enabled = false;
        //                    tsbPrint.Enabled = false; 
        //                    //***lblSOStatus.Text = "This customer was rejected by COO for oranage flag. If you need to request approval from COO, click on Draft button again.";
        //                    lblSOStatus.Text = "This customer was rejected by COO for oranage flag. If you need to request approval from COO, click on Release button again.";

        //            }
        //                else if (CheckApproval == 0 && !CheckRejected)
        //                {
        //                    tsbSave.Enabled = true;
        //                    tsbDraft.Enabled = true;
        //                    tsbCreatePO.Enabled = true;
        //                    tsbPrint.Enabled = true;
        //                    DocConKey.ReadOnly = true;

        //                    //lblSOStatus.Text = "This So was approved for orange flag and can be released now.";
        //                    //***lblSOStatus.Text = "The sales order with the Orange-flagged customer has been approved and is now ready for release.";

        //                }
        //                else if (CheckApproval == 1 && !CheckRejected)
        //                {
        //                    // MsgBox.Show("Still requesting approval from COO for this SO.");
        //                    //tsbDraft.Enabled = false;
        //                    //***tsbSave.Enabled = false;
        //                    tsbCreateDO.Enabled = false;
        //                    tsbCreatePO.Enabled = false;
        //                    tsbPrint.Enabled = false;
        //                    DocConKey.ReadOnly = true;
        //                    //lblSOStatus.Text = "Still requesting approval from COO for this SO for orange flag.";
        //                    lblSOStatus.Text = "Pending approval for sales to the Orange-flagged customer"; 

        //            }
        //                else if (CheckApproval == 2)
        //                {
        //                //lblSOStatus.Text = "Before releasing this SO, you need approval from COO because this customer is Orange flag. You can request approval by clicking Draft button.";
        //                //***lblSOStatus.Text = "Management approval is required for sales to the Orange-flagged customer. You can request approval by clicking the Draft button.";
        //                 lblSOStatus.Text = "This customer is classified as an Orange-flagged customer, and management approval will be required when releasing this sales order.";
        //                }
        //                else
        //                {

        //                    lblSOStatus.Text = "";
        //                }
        //            }
        //            else
        //            {
        //                lblSOStatus.Text = "";
        //                if (OrangeCus == true)
        //                {
        //                    DocConKey.Appearance.BackColor = System.Drawing.Color.DarkOrange;
        //                    DocConNm.Appearance.BackColor = System.Drawing.Color.Orange;
        //                }

        //            }




        //    }
        //}
        private void Check()
        {
            if (isCash) return;

            //if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
            if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM || SysOptionUtility.DatabaseBranchCode == DBCode.ADL
           || SysOptionUtility.DatabaseBranchCode == DBCode.SOP || SysOptionUtility.DatabaseBranchCode == DBCode.BOS)
            {

                lblSOStatus.Text = "";
                txtCrSO.Text = "";
                DocConKey.Appearance.BackColor = System.Drawing.Color.White;
                DocConNm.Appearance.BackColor = System.Drawing.Color.White;
                txtCrSO.Appearance.ForeColor = Color.Blue;
                lblSOStatus.Appearance.ForeColor = Color.Blue;

                CheckSOAllStatus();
                CheckSOCustomerSatus();

                if (OrangeCus == true && CheckState != 100 && DocHome.Text.ToString() != "0.00" )
                {

                    DocConKey.Appearance.BackColor = System.Drawing.Color.DarkOrange;
                    DocConNm.Appearance.BackColor = System.Drawing.Color.Orange;
                    //***tsbSave.Enabled = false;
                    tsbPrint.Enabled = false;
                    tsbCreateDO.Enabled = false;

                    if (CheckApproval == 1 && CheckRejected == true && CheckState ==20)
                    {
                        //MsgBox.Show("This customer was rejected by COO. If you need to request approval from COO, click on release button again.");
                        //tsbSave.Enabled = false;
                        tsbDraft.Enabled = true;
                        tsbCreateDO.Enabled = false;
                        tsbCreatePO.Enabled = false;
                        tsbPrint.Enabled = false;
                        //***lblSOStatus.Text = "This customer was rejected by COO for oranage flag. If you need to request approval from COO, click on Draft button again.";
                        lblSOStatus.Text = "This customer was rejected by COO for oranage flag. If you need to request approval from COO, click on Release button again.";

                    }
                   
                    else if (CheckApproval == 1 &&  CheckState == 30)
                    {
                        // MsgBox.Show("Still requesting approval from COO for this SO.");
                        //tsbDraft.Enabled = false;
                        tsbSave.Enabled = false;
                        tsbCreateDO.Enabled = false;
                        tsbCreatePO.Enabled = false;
                        tsbPrint.Enabled = false;
                        DocConKey.ReadOnly = true;
                        //lblSOStatus.Text = "Still requesting approval from COO for this SO for orange flag.";
                        lblSOStatus.Text = "Pending approval for sales to the Orange-flagged customer";

                    }
                    else if (CheckApproval == 2)
                    {
                        //lblSOStatus.Text = "Before releasing this SO, you need approval from COO because this customer is Orange flag. You can request approval by clicking Draft button.";
                        //***lblSOStatus.Text = "Management approval is required for sales to the Orange-flagged customer. You can request approval by clicking the Draft button.";
                        lblSOStatus.Text = "This customer is classified as an Orange-flagged customer, and management approval will be required when releasing this sales order.";
                    }
                    else
                    {

                        lblSOStatus.Text = "";
                    }
                }
                else if (OrangeCus == true && CheckState == 100 && DocHome.Text.ToString() != "0.00" && CheckApproval == 2)
                {

                    lblSOStatus.Text = "This customer is classified as an Orange-flagged customer, and management approval will be required when releasing this sales order.";
                    

                }
                else
                {
                    lblSOStatus.Text = "";
                    if (OrangeCus == true)
                    {
                        DocConKey.Appearance.BackColor = System.Drawing.Color.DarkOrange;
                        DocConNm.Appearance.BackColor = System.Drawing.Color.Orange;
                    }
                }




            }
        }
        private void CheckSOAllStatus()
        {
            string proc = "Get_SOStatus";
            string strDocID = this.DocID.Text;
            //if (strDocID != null || strDocID != "")

            CheckApproval = 2; //0 = aldy approved/rejected, 1 = pending approval, 2= not requested yet

            CheckRejected = false;

            if (strDocID != null && strDocID != "")
            {
                List<SqlParameter> parList = new List<SqlParameter>();
                parList.Add(new SqlParameter("@DocID", strDocID));
                DataTable dt = GFunc.ExecuteProcReader(proc, parList);
                if (dt.Rows.Count > 0)
                {
                    var objApproval = dt.Rows[0][0];
                    if (objApproval == DBNull.Value)
                        CheckApproval = 2;
                    else if (GFunc.NEBool(dt.Rows[0][0], false) == false)
                        CheckApproval = 0;
                    else CheckApproval = 1;

                    CheckRejected = GFunc.NEBool(dt.Rows[0][1], false);

                    var objApprovalCr = dt.Rows[0][2];
                    if (objApprovalCr == DBNull.Value)
                        CheckCrLimitApproval = 2;
                    else if (GFunc.NEBool(dt.Rows[0][2], false) == false)
                        CheckCrLimitApproval = 0;
                    else CheckCrLimitApproval = 1;

                    CheckCrLimitRejected = GFunc.NEBool(dt.Rows[0][3], false);

                    CheckState = GFunc.NEInt(dt.Rows[0][4], 0);
                }
            }
        }

        private void CheckSOCustomerSatus()
        {
            string proc = "Get_CusStatusSO";
            string strDocConID = this.DocConKey.Text;
            if (strDocConID != null || strDocConID != "")
            {
                List<SqlParameter> parList = new List<SqlParameter>();
                parList.Add(new SqlParameter("@ConID", strDocConID));
                DataTable dt = GFunc.ExecuteProcReader(proc, parList);
                if (dt.Rows.Count > 0)
                {
                    OrangeCus = GFunc.NEBool(dt.Rows[0][0], false);
                    RedCus = GFunc.NEBool(dt.Rows[0][1], false);

                }
            }
        }

        private void CheckOrange()
        {
            if (isCash) return;
                CheckSOCustomerSatus();
                if (OrangeCus == true )
                {
                    DocConKey.Appearance.BackColor = System.Drawing.Color.DarkOrange;
                    DocConNm.Appearance.BackColor = System.Drawing.Color.Orange;
                    lblSOStatus.Appearance.ForeColor = Color.Blue;
                //***tsbSave.Enabled = false;
                tsbCreateDO.Enabled = false;
                    tsbCreatePO.Enabled = false;
                    tsbPrint.Enabled = false;
                    //MsgBox.Show("Release button is disable now because need approval from COO for this orange customer.");

                    //lblSOStatus.Text = "Release button is disable now because need approval from COO for this orange customer."; /* commented by YST */

                    //***lblSOStatus.Text = "Release button is currently disabled because management approval is required for the Orange-flagged customer.";

                    lblSOStatus.Text = "This customer is classified as an Orange-flagged customer, and management approval will be required when releasing this sales order.";
            }

            
        }

        private void CheckOrangeAndAmount()
        {

            CheckSOCustomerSatus();
            if (OrangeCus == true && DocHome.Text.ToString() != "0.00")
            {
                DocConKey.Appearance.BackColor = System.Drawing.Color.DarkOrange;
                DocConNm.Appearance.BackColor = System.Drawing.Color.Orange;
                //***tsbSave.Enabled = false;
                tsbCreateDO.Enabled = false;
                tsbCreatePO.Enabled = false;
                tsbPrint.Enabled = false;
                //MsgBox.Show("Release button is disable now because need approval from COO for this orange customer.");
                //***lblSOStatus.Text = "Release button is disable now because need approval from COO for this orange customer.";

                lblSOStatus.Text = "This customer is classified as an Orange-flagged customer, and management approval will be required when releasing this sales order.";
            }


        }

        #endregion
        
        //added by nnt on Jul 2020
        #region GetARAging
        private void tabDetailList_SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
            {
                if (e.Tab.Text == "AR Aging Status")
                {
                    string strDocConID = "";
                    this.TDate.Text = DateTime.Now.ToString("dd MMM yyyy");

                    TDate.ButtonsRight[0].Enabled = true;

                    if (DocConKey.Text != "")
                    {
                        strDocConID = DocConKey.Text;

                        //this.TDate.ReadOnly = false;
                        this.TDate.Enabled = true;
                        this.TDate.ReadOnly = false;
                        btnPreview.Enabled = true;
                        DocConKey.Enabled = true;
                        webBrowser1.DocumentText = GetHTMLAge();

                    }
                }
                BindPickingList(); /* added by YST on 2022/05/05 */
            }
        }       

        private DataTable GetDueInvoicesByConID()
        {
            DataTable dtDueInvoices = new DataTable();
            string proc = "ARSO_GetCustAgeByConID";
            string strDocConID = this.DocConKey.Text;
            if (strDocConID != null || strDocConID != "")
            {
                List<SqlParameter> parList = new List<SqlParameter>();
                parList.Add(new SqlParameter("@ConID", strDocConID));
                dtDueInvoices = GFunc.ExecuteProcReader(proc, parList);

            }
            return dtDueInvoices;
        }

        private DataTable GetARAging()
        {
            string proc = "ARSO_AgingStatus";
            string strDocConID = this.DocConKey.Text;
            string strDocID = this.DocID.Text;
            DataTable dtARAging = null;
            if (strDocConID != null || strDocConID != "")
            {
                if (strDocID != null || strDocID != "")
                {
                    List<SqlParameter> parList = new List<SqlParameter>();
                    parList.Add(new SqlParameter("@DocConID", strDocConID));
                    parList.Add(new SqlParameter("@DocID", strDocID));
                    dtARAging = GFunc.ExecuteProc(proc, parList);

                }
            }
            return dtARAging;
        }

        private string GetHTMLAge()
        {
            DataTable dtInvoices = GetDueInvoicesByConID();
            DataTable dt = GetARAging();

            Decimal TDueHome = 0;

            if (dtInvoices.Rows.Count > 0)
            {

                for (int p = 0; p < dtInvoices.Rows.Count; p++)

                {
                    TDueHome += Convert.ToDecimal(dtInvoices.Rows[p]["TDueHome"]);

                }
            }

            TDueHome += Convert.ToDecimal(dt.Rows[0]["DocHome"]);
            Decimal CrLimitExceed = 0;
            string CrLimitEx = "No Exceed";

            if (TDueHome > Convert.ToDecimal(dt.Rows[0]["CrLimit"]))
            {

                CrLimitExceed = TDueHome - Convert.ToDecimal(dt.Rows[0]["CrLimit"]);
                CrLimitEx = "SGD" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", CrLimitExceed).ToString();
            }


            string LPaymentRDate = "";
            if (Convert.ToDateTime(dt.Rows[0]["DocDate"]).ToString("dd MMM yyyy") != "01 Jan 1900") { LPaymentRDate = Convert.ToDateTime(dt.Rows[0]["DocDate"]).ToString("dd MMM yyyy"); }
            string OldestAR = "";
            if (Convert.ToDateTime(dt.Rows[0]["DocDate"]).ToString("dd MMM yyyy") != "01 Jan 1900") { OldestAR = Convert.ToDateTime(dt.Rows[0]["DocDate"]).ToString("dd MMM yyyy"); }

            Decimal checkTotal = 0;
            string strMessageBody = "";

            DateTime LastMonthLastDate = DateTime.Today.AddDays(0 - DateTime.Today.Day);
            DateTime LastMonthFirstDate = LastMonthLastDate.AddDays(1 - LastMonthLastDate.Day);
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            var CurMthEndDate = startDate.AddMonths(1).AddDays(-1);

            int month = now.Month;
            int year = now.Year;

            string lstDayLastMth = string.Empty;
            string lstDayLastMth1 = string.Empty;
            string lstDayLastMth2 = string.Empty;
            string lstDayLastMth3 = string.Empty;
            string lstDayLastMth4 = string.Empty;
            string lstDayLastMth5 = string.Empty;
            string lstDayLastMth6 = string.Empty;
            string lstDayLastMth7 = string.Empty;
            int numberOfDays = 0;

            DateTime lastDayLastMonth;

            for (int i = 0; i <= 7; i++)
            {
                if (month == 1) { month = 12; year = now.Year - 1; }
                else
                    month = month - 1;
                numberOfDays = DateTime.DaysInMonth(year, month);
                if (i == 0) { lastDayLastMonth = new DateTime(year, month, numberOfDays); lstDayLastMth = lastDayLastMonth.ToString("dd MMM yy"); }
                if (i == 1) { lastDayLastMonth = new DateTime(year, month, numberOfDays); lstDayLastMth1 = lastDayLastMonth.ToString("dd MMM yy"); }
                if (i == 2) { lastDayLastMonth = new DateTime(year, month, numberOfDays); lstDayLastMth2 = lastDayLastMonth.ToString("dd MMM yy"); }
                if (i == 3) { lastDayLastMonth = new DateTime(year, month, numberOfDays); lstDayLastMth3 = lastDayLastMonth.ToString("dd MMM yy"); }
                if (i == 4) { lastDayLastMonth = new DateTime(year, month, numberOfDays); lstDayLastMth4 = lastDayLastMonth.ToString("dd MMM yy"); }
                if (i == 5) { lastDayLastMonth = new DateTime(year, month, numberOfDays); lstDayLastMth5 = lastDayLastMonth.ToString("dd MMM yy"); }
                if (i == 6) { lastDayLastMonth = new DateTime(year, month, numberOfDays); lstDayLastMth6 = lastDayLastMonth.ToString("dd MMM yy"); }
                if (i == 7) { lastDayLastMonth = new DateTime(year, month, numberOfDays); lstDayLastMth7 = lastDayLastMonth.ToString("dd MMM yy"); }

            }

            strMessageBody = "<!DOCTYPE html><html><head><style>table, td {border: 0.3px solid gray;font-family:Calibri, Arial, Helvetica, sans-serif !important;font-size:11.0pt;} </style></head><body>"
               + "<p><table>"
               + "<tr>"
               + "<td>Credit Limit Exceed by </td><td>: </td><td>" + CrLimitEx + "</td>"
               + "</tr>"

                + "<tr>"
               + "<td>Credit Limit</td><td>:</td><td>SGD " + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dt.Rows[0]["CrLimit"]).ToString() + "</td>"
               + "</tr>"


               + "<tr>"
               + "<td>Last Payment Received Date</td><td>:</td><td>" + LPaymentRDate + "</td>"
               + "</tr>"

               + "<tr>"
               + "<td>Last Payment Received Amount</td><td>:</td><td>" + dt.Rows[0]["CurrID"].ToString() + " " + String.Format("{0,15:#,##0.00 ;(#,##0.00);   }", dt.Rows[0]["DocAppAmtF"]).ToString() + "</td>"
               //String.Format("{ 0,15:#,##0.00 ;(#,##0.00);-}", dt.Rows[0]["DocAppAmtH"]) + "</td>"
               //String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["0"])
               + "</tr>"

               + "<tr>"
               + "<td>Last Payment Received ID</td><td>:</td><td>" + dt.Rows[0]["PayID"].ToString() + "</td>"
               //String.Format("{ 0,15:#,##0.00 ;(#,##0.00);-}", dt.Rows[0]["DocAppAmtH"]) + "</td>"
               //String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["0"])
               + "</tr>"

                + "<tr>"
               + "<td>Credit Term</td><td>:</td><td>" + dt.Rows[0]["CRTerm"].ToString() + "</td>"
               //String.Format("{ 0,15:#,##0.00 ;(#,##0.00);-}", dt.Rows[0]["DocAppAmtH"]) + "</td>"
               //String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["0"])
               + "</tr>"

               + "<tr>"
               + "<td>Last Remark Update</td><td>:</td><td>" + dt.Rows[0]["LCusRemark"].ToString() + "</td>"
               + "</tr></table></p>";
            if (dtInvoices.Rows.Count > 0)
            {
                strMessageBody += "<table style =\"padding: 2px 2px 2px 2px;background-color:#f0f0f5;\"><tr><td><table style=\"border-collapse:collapse; border:1px solid black;\">"
                    + "<thead><tr><th colspan =\"13\"style=\"1px solid #CED8F6; background-color:#08298A;color:White;\">AR AGING AS OF (" + DateTime.Today.ToString("dd MMM yyyy") + ")</th></tr>"
                    + "<tr><th colspan =\"13\" style=\"1px solid #CED8F6; background-color:#08298A;color:White;\">FOR YOUR IMMEDIATE ACTION</th></tr><tr>"
                    + "<th  style=\"border: 1px solid #08298A; background-color:#CED8F6;\"> CUR </th>"
                    + "<th  style=\"border: 1px solid #08298A; background-color:#CED8F6;\"> TERM </th>"
                    + "<th  style=\"border: 1px solid #08298A; background-color:#CED8F6;\"> CR LIMIT </th>"
                    + "<th  style=\"border: 1px solid #08298A; background-color:#CED8F6;\"> TOTAL DUE </th>"
                    + "<th  style=\"border: 1px solid #08298A; background-color:#CED8F6;\"> CURRENT </th>"
                    + "<th  style=\"border: 1px solid #08298A; background-color:#CED8F6;\">" + lstDayLastMth + "<br/>30 Days Due</th>"
                    + "<th  style=\"border: 1px solid #08298A; background-color:#CED8F6;\">" + lstDayLastMth1 + "<br/>60 Days Due</th>"
                    + "<th  style=\"border: 1px solid #08298A; background-color:#CED8F6;\">" + lstDayLastMth2 + "<br/>90 Days Due</th>"
                    + "<th  style=\"border: 1px solid #08298A; background-color:#CED8F6;\">" + lstDayLastMth3 + "<br/>120 Days Due</th>"
                    + "<th  style=\"border: 1px solid #08298A; background-color:#CED8F6;\">" + lstDayLastMth4 + "<br/>150 Days Due</th>"
                    + "<th  style=\"border: 1px solid #08298A; background-color:#CED8F6;\">" + lstDayLastMth5 + "<br/>180 Days Due</th>"
                    + "<th  style=\"border: 1px solid #08298A; background-color:#CED8F6;\">" + lstDayLastMth6 + "<br/>210 Days Due</th>"
                    + "<th  style=\"border: 1px solid #08298A; background-color:#CED8F6;\"> <" + lstDayLastMth7 + "<br/><240 Days Due</th>"
                    + "</tr></thead><tbody>";



                for (int j = 0; j < dtInvoices.Rows.Count; j++)
                {
                    checkTotal = Convert.ToDecimal(dtInvoices.Rows[j]["T"]);
                    decimal decTotal5mths = Convert.ToDecimal(dtInvoices.Rows[j]["8"]) + Convert.ToDecimal(dtInvoices.Rows[j]["9"]) + Convert.ToDecimal(dtInvoices.Rows[j]["10"]) + Convert.ToDecimal(dtInvoices.Rows[j]["11"]) + Convert.ToDecimal(dtInvoices.Rows[j]["12"]);

                    if (Convert.ToInt32(dtInvoices.Rows[j]["StandNetDueDay"]) == 0)
                    {
                        strMessageBody += "<tr>"
                        + "<td  style=\"border: 1px solid #08298A;width:30px;align:center;background-color:#CED8F6;text-align:center;padding-right:10px;\">" + dtInvoices.Rows[j]["DocCurrID"].ToString() + "</td>"
                        + "<td  style=\"border: 1px solid #08298A;width:30px;align:center;background-color:#CED8F6;text-align:center;padding-right:10px;\">" + dtInvoices.Rows[j]["DocTermID"].ToString() + "</td>"
                        + "<td  style=\"border: 1px solid #08298A;width:30px;align:center;background-color:#CED8F6;text-align:right;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["DocCreditLimit"]) + "</td>";
                        if (Convert.ToDecimal(dtInvoices.Rows[j]["T"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["T"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["T"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["0"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["0"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["0"]) + "</td>";


                        if (Convert.ToDecimal(dtInvoices.Rows[j]["1"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["1"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["1"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["2"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["2"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["2"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["3"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["3"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["3"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["4"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["4"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["4"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["5"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["5"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["5"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["6"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["6"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["6"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["7"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["7"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["7"]) + "</td>";

                        if (decTotal5mths > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#FB031A;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", decTotal5mths) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", decTotal5mths) + "</td>";

                        //if (Convert.ToDecimal(dtInvoices.Rows[j]["MthPayAmt"]) > 0)
                        //    strMessageBody += "<td  style=\"border: 0.1px solid gray;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0:0,0.00}", dtInvoices.Rows[j]["MthPayAmt"]) + "</td>";
                        //else strMessageBody += "<td  style=\"border: 0.1px solid gray;width:100px;text-align:right;background-color:#ffe6e6;padding-right:10px;\">" + String.Format("{0:0,0.00}", dtInvoices.Rows[j]["MthPayAmt"]) + "</td>";

                        strMessageBody += "</tr>";
                    }

                    if (Convert.ToInt32(dtInvoices.Rows[j]["StandNetDueDay"]) > 0 && Convert.ToInt32(dtInvoices.Rows[j]["StandNetDueDay"]) <= 30)
                    {
                        strMessageBody += "<tr>"
                        + "<td  style=\"border: 1px solid #08298A;width:30px;align:center;background-color:#CED8F6;text-align:center;padding-right:10px;\">" + dtInvoices.Rows[j]["DocCurrID"].ToString() + "</td>"
                         + "<td  style=\"border: 1px solid #08298A;width:30px;align:center;background-color:#CED8F6;text-align:center;padding-right:10px;\">" + dtInvoices.Rows[j]["DocTermID"].ToString() + "</td>"
                        + "<td  style=\"border: 1px solid #08298A;width:30px;align:center;background-color:#CED8F6;text-align:center;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["DocCreditLimit"]) + "</td>";
                        if (Convert.ToDecimal(dtInvoices.Rows[j]["T"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["T"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["T"]) + "</td>";

                        strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["0"]) + "</td>";


                        if (Convert.ToDecimal(dtInvoices.Rows[j]["1"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["1"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["1"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["2"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["2"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["2"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["3"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["3"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["3"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["4"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["4"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["4"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["5"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["5"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["5"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["6"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["6"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["6"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["7"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["7"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["7"]) + "</td>";

                        if (decTotal5mths > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#FB031A;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", decTotal5mths) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", decTotal5mths) + "</td>";

                        //if (Convert.ToDecimal(dtInvoices.Rows[j]["MthPayAmt"]) > 0)
                        //    strMessageBody += "<td  style=\"border: 0.1px solid gray;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0:0,0.00}", dtInvoices.Rows[j]["MthPayAmt"]) + "</td>";
                        //else strMessageBody += "<td  style=\"border: 0.1px solid gray;width:100px;text-align:right;background-color:#ffe6e6;padding-right:10px;\">" + String.Format("{0:0,0.00}", dtInvoices.Rows[j]["MthPayAmt"]) + "</td>";

                        strMessageBody += "</tr>";
                    }

                    if (Convert.ToInt32(dtInvoices.Rows[j]["StandNetDueDay"]) == 45 || Convert.ToInt32(dtInvoices.Rows[j]["StandNetDueDay"]) == 60)
                    {
                        strMessageBody += "<tr>"
                        + "<td  style=\"border: 1px solid #08298A;width:30px;align:center;background-color:#CED8F6;text-align:center;padding-right:10px;\">" + dtInvoices.Rows[j]["DocCurrID"].ToString() + "</td>"
                        + "<td  style=\"border: 1px solid #08298A;width:30px;align:center;background-color:#CED8F6;text-align:center;padding-right:10px;\">" + dtInvoices.Rows[j]["DocTermID"].ToString() + "</td>"
                        + "<td  style=\"border: 1px solid #08298A;width:30px;align:center;background-color:#CED8F6;text-align:center;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["DocCreditLimit"]) + "</td>";
                        if (Convert.ToDecimal(dtInvoices.Rows[j]["T"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["T"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["T"]) + "</td>";

                        strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["0"]) + "</td>";
                        strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["1"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["2"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["2"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["2"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["3"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["3"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["3"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["4"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["4"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["4"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["5"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["5"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["5"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["6"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["6"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["6"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["7"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["7"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["7"]) + "</td>";

                        if (decTotal5mths > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#FB031A;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", decTotal5mths) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", decTotal5mths) + "</td>";

                        //if (Convert.ToDecimal(dtInvoices.Rows[j]["MthPayAmt"]) > 0)
                        //    strMessageBody += "<td  style=\"border: 0.1px solid gray;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0:0,0.00}", dtInvoices.Rows[j]["MthPayAmt"]) + "</td>";
                        //else strMessageBody += "<td  style=\"border: 0.1px solid gray;width:100px;text-align:right;background-color:#ffe6e6;padding-right:10px;\">" + String.Format("{0:0,0.00}", dtInvoices.Rows[j]["MthPayAmt"]) + "</td>";

                        strMessageBody += "</tr>";
                    }

                    if (Convert.ToInt32(dtInvoices.Rows[j]["StandNetDueDay"]) == 90)
                    {
                        strMessageBody += "<tr>"
                        + "<td  style=\"border: 1px solid #08298A;width:30px;align:center;background-color:#CED8F6;text-align:center;padding-right:10px;\">" + dtInvoices.Rows[j]["DocCurrID"].ToString() + "</td>"
                        + "<td  style=\"border: 1px solid #08298A;width:30px;align:center;background-color:#CED8F6;text-align:center;padding-right:10px;\">" + dtInvoices.Rows[j]["DocTermID"].ToString() + "</td>"
                        + "<td  style=\"border: 1px solid #08298A;width:30px;align:center;background-color:#CED8F6;text-align:center;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["DocCreditLimit"]) + "</td>";
                        if (Convert.ToDecimal(dtInvoices.Rows[j]["T"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["T"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["T"]) + "</td>";

                        strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["0"]) + "</td>";
                        strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["1"]) + "</td>";
                        strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["2"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["3"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["3"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["3"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["4"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["4"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["4"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["5"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["5"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["5"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["6"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["6"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["6"]) + "</td>";

                        if (Convert.ToDecimal(dtInvoices.Rows[j]["7"]) > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["7"]) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", dtInvoices.Rows[j]["7"]) + "</td>";

                        if (decTotal5mths > 0)
                            strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#FB031A;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", decTotal5mths) + "</td>";
                        else strMessageBody += "<td  style=\"border: 1px solid #08298A;width:100px;text-align:right;background-color:#ffffff;padding-right:10px;\">" + String.Format("{0,15:#,##0.00 ;(#,##0.00);-   }", decTotal5mths) + "</td>";

                        //if (Convert.ToDecimal(dtInvoices.Rows[j]["MthPayAmt"]) > 0)
                        //    strMessageBody += "<td  style=\"border: 0.1px solid gray;width:100px;text-align:right;background-color:#b3c4ff;padding-right:10px;\">" + String.Format("{0:0,0.00}", dtInvoices.Rows[j]["MthPayAmt"]) + "</td>";
                        //else strMessageBody += "<td  style=\"border: 0.1px solid gray;width:100px;text-align:right;background-color:#ffe6e6;padding-right:10px;\">" + String.Format("{0:0,0.00}", dtInvoices.Rows[j]["MthPayAmt"]) + "</td>";

                        strMessageBody += "</tr>";
                    }

                }
                strMessageBody += "</tbody></table></td></tr></table>";

            }
            else strMessageBody += "<div style=\"width: 920px; height: 230px; border: 5px solid #ccc;background-color:#ffffff;\">"

                                 + "<div style =\"margin:60px 100px 100px 350px;font-family:Calibri;font-size:50px;font-weight:bold;color:#ccc;\" > NO AGING </div>"
                                 + " </ div > ";

            return strMessageBody;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            frmMain.gfrmMain.SetNotifyStatus("Loading.... Please wait.");
            try
            {
                PrintReport();
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
            frmMain.gfrmMain.SetNormalStaus("Ready");
        }

        //added by May on 18-Nov-2020
        private void DocCompleted_CustomUpdate(object sender, EventArgs e)
        {
            DocCompleted.DataBindings[0].WriteValue();
            AddItm_Hash();
            DocComUtility.CalForm(objFactory.Doc, htDetailGrd, true, false);
        }

        private void btnAttachSignedDO_Click(object sender, EventArgs e)
        {
            try
            {
                bool dirty = objFactory.Doc.IsDirty;

                frmAttachment f = new frmAttachment(objFactory.Doc.Attachments, objFactory.Doc, 2);
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.Yes)
                {
                    if (objFactory.Doc.Attachment != true)//To prevent dirty  
                    {
                        objFactory.Doc.Attachment = true;
                        Attachment.Checked = true;
                    }
                }
                else if (objFactory.Doc.Attachment != false)//To prevent dirty
                {
                    objFactory.Doc.Attachment = false;
                    Attachment.Checked = false;
                }

                //filtering to get the count of header's attachment file to show on btnAttachmentEdit.          
                btnAttachmentEdit.Text = "Customer PO (" + objFactory.Doc.Attachments.Count(o => o.DocDItm == -1 && o.DocDetailType == 1) + ")";
                if (objFactory.Doc.IsDirty && SysOptionUtility.HasDMASLink) //If linked to DMAS, the attachments are already saved. If not, dirty state should not be restored back
                    objFactory.Doc.IsDirty = dirty;
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

        private DataTable GetCustomer()
        {

            DataTable dtCustomer = null;
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@CCBType", GFunc.NEInt(10, 0)));
            parmList.Add(new SqlParameter("@fromID", GFunc.NEStr(DocConKey.Text, string.Empty)));
            parmList.Add(new SqlParameter("@toID", GFunc.NEStr(DocConKey.Text, string.Empty)));
            parmList.Add(new SqlParameter("@sortingTypeLevel", -1));    //ignore Record Access Level
            parmList.Add(new SqlParameter("@sortingTypeGrp", -1));      //ignore record access group

            dtCustomer = GFunc.ExecuteProc("MSTCON_GetCustomerList", parmList);
            dtCustomer.TableName = "dtSelectCustomer";

            dtCustomer.Columns.Add("DueAmtH", typeof(decimal));

            return dtCustomer;

        }

        private DataSet LoadReportData(DataTable dtCustomers)
        {
            // Preapre parameter list (Require parameter at least @MsgID)
            try
            {
                DateTime DateFrom;
                if (DateTime.Today.Month == 1)
                    DateFrom = ((DateTime)TDate.DateValue).AddDays(1 - DateTime.DaysInMonth(DateTime.Today.Year - 1, 12));
                else
                    DateFrom = ((DateTime)TDate.DateValue).AddDays(1 - DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month - 1));


                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@RepKey", 2265));
                parmList.Add(new SqlParameter("@StatementType", 2265));
                parmList.Add(new SqlParameter("@DateTo", TDate.DateValue));
                string xmlSelectedCustomer = GFunc.ConvertDataTableToXML(dtCustomers);
                parmList.Add(new SqlParameter("@xmlSelectedCustomer", xmlSelectedCustomer));

                //No more use in store procedure
                ////if (GFunc.NEInt(DueCalculation.Value, 0) > 0)
                ////    parmList.Add(new SqlParameter("@DueCalculateType", DueCalculation.Value));

                if (GFunc.NEInt(10, 0) > 0)
                    parmList.Add(new SqlParameter("@CCBType", 10));

                parmList.Add(new SqlParameter("@DateFrom", DateFrom));

                return GFunc.ExecuteProcDataSet("Rep_CVState", parmList);

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

        private void webviewOrderTracking_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            webviewEnsure = true;
        }        

        private bool ExistData(DataSet dsReportData)
        {
            if (dsReportData == null)
            {
                MsgBox.Show("There is no records for this report");
                return false;
            }
            else
            {
                if (dsReportData.Tables.Count <= 0)
                {
                    MsgBox.Show("There is no records for this report");
                    return false;
                }
                else
                    if (dsReportData.Tables[0].Rows.Count <= 0)
                {
                    MsgBox.Show("There is no records for this report");
                    return false;
                }
            }
            return true;
        } 

        private void PrintReport()
        {
            string repTitle = "STATEMENT OF ACCOUNT";
            string rptName = string.Empty;
            string SubReport = "AgeList";
            DataTable dtCustomer = GetCustomer();
            DataSet dsReportData = LoadReportData(dtCustomer);
            DataSet ds = new DataSet();
            if (dsReportData.Tables.Count > 1)
            {
                dsReportData.Tables[0].TableName = "Header";
                dsReportData.Tables[1].TableName = "Detail"; //Sub Report Data                   

                ds.Tables.Add(dsReportData.Tables[0].DefaultView.ToTable());
                ds.Tables.Add(dsReportData.Tables[1].DefaultView.ToTable());
            }
            else
            {
                dsReportData.Tables[0].TableName = "Header";

                ds.Tables.Add(dsReportData.Tables[0].DefaultView.ToTable());
            }

            if (!ExistData(ds))
                return;
            int repKey = GFunc.NEInt(2265, 0);
            ReportLoader _ReportLoader = new ReportLoader();
            List<ReportParameter> repParas = new List<ReportParameter>();

            string opCmpValue = SysOptionUtility.GetStr("CompanyName");
            string opCmpRegValue = SysOptionUtility.GetStr("CompanyRegNumber");
            int opaddrKey = SysOptionUtility.GetInt("DefaultLetterHeadAddr");

            string opCmpAddrValue = GFunc.AddrGet(opaddrKey, true);

            repParas.Add(new ReportParameter("pCmpName", opCmpValue));
            repParas.Add(new ReportParameter("pCmpAddr", opCmpAddrValue));
            repParas.Add(new ReportParameter("pRepTitle", repTitle));

            //Assign DateTo parameter to Rpx for Reminder with additional days


            DateTime agDate = GFunc.NEDateTime(TDate.Value, DateTime.Today);
            agDate = new DateTime(agDate.Year, agDate.Month, DateTime.DaysInMonth(agDate.Year, agDate.Month));
            //Assign DateTo parameter to Rpx for BF Statment only

            if (repKey == 2270 || repKey == 1335 || repKey == 2265)
            {
                repParas.Add(new ReportParameter("pStatementDate", TDate.DateValue));
            }


            if (repKey != 2290 && repKey != 2275 && repKey != 2280 && repKey != 2285 && repKey != 1335) //if Not Reminder
            {
                string SubReportPath = Application.StartupPath.ToString() + "\\" + SubReport;
                repParas.Add(new ReportParameter("pSubRepPath", SubReportPath));
                for (int i = 0; i < 12; i++)
                {
                    repParas.Add(new ReportParameter("pMth" + (i + 1).ToString(), agDate.AddMonths(i * -1).ToString("dd-MMM-yyyy")));
                }
            }
            else
            {
                repParas.Add(new ReportParameter("pStatementDate", TDate.DateValue));
            }



            _ReportLoader.ReportParameter = repParas;
            _ReportLoader.RepKey = repKey;

            _ReportLoader.LoadCrystalReport(ds, "zState_ContMF1.rpt", SubReport);
            _ReportLoader.PrintPreview();

        }

        private void tagrdDetItms_AfterCellUpdate(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Key == "ItmID")
            {
                int ItmKey = GFunc.NEInt(tagrdDetItms.Rows[e.Cell.Row.Index].Cells["ItmKey"].Value, 0);
                if (tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmRef"].EditorComponent != null )
                {
                    GlobalUI.BindComboValue((TAUtil.TAComboBox)tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmRef"].EditorComponent, "ItemInternalRef", "ID", "ItmRef", ItmKey);
                }
            }
            else if (e.Cell.Column.Key == "ItmRef")
            {
                string ItmRef = GFunc.NEStr(tagrdDetItms.Rows[e.Cell.Row.Index].Cells["ItmRef"].Value, "");
                string ItmID = GFunc.NEStr(tagrdDetItms.Rows[e.Cell.Row.Index].Cells["ItmID"].Value, "");
                if (tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmRef"].EditorComponent != null)
                {
                    var cboItmRef = tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmRef"].EditorComponent as TAUtil.TAComboBox;
                    if (cboItmRef != null && cboItmRef.DataSource != null)
                    {
                        IEnumerable<DataRow> dr  = ((DataTable)cboItmRef.DataSource).Select("ItmRef = '" + ItmRef + "'");
                        if (dr.Count() == 0)
                        {
                            if (ItmRef.Contains("RT"))
                            {
                                MsgBox.Show("<font color='red'>" + ItmRef + "</font> is not the valid report for <font color='red'>" + ItmID + "</font>.<br/>Please select the correct Defect Report ID.", GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                            }
                            else
                            {
                                MsgBox.Show("<font color='red'>" + ItmRef + "</font> is invalid reference.Please select the correct reference.", GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                            }
                            tagrdDetItms.Rows[e.Cell.Row.Index].Cells["ItmRef"].Value = "";
                        }
                    }                    
                }
            }
        }

        private void tagrdDetItms_ClickCell(object sender, ClickCellEventArgs e)
        {
            if (e.Cell.Column.Key == "ItmRef")
            {
                int ItmKey = GFunc.NEInt(tagrdDetItms.Rows[e.Cell.Row.Index].Cells["ItmKey"].Value, 0);
                if (tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmRef"].EditorComponent != null )
                {
                    GlobalUI.BindComboValue((TAUtil.TAComboBox)tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmRef"].EditorComponent, "ItemInternalRef", "ID", "ItmRef", ItmKey);
                }                   
            }
        }

        private void DateTo_Leave(object sender, EventArgs e)
        {
            TDate.Enabled = true;
            TDate.ReadOnly = false;
            if (TDate.Text != "") { TDate.Text = Convert.ToDateTime(TDate.Text).ToString("dd MMM yyyy"); }
        }
        #endregion GetARAging

        #region WMS-OrderTracking /* added by YST on 2022/05/05 */
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindPickingList();
        }
        private void BindPickingList()
        {
            if (tabDetailList.ActiveTab.Key != "tsbOrderTracking" || DocID.Text == "") return;
            try
            {
                /* 
                * Used WebView2 control instead of WebBrowser because WebBrowser control works with IE , IE can't work some of css style 
                * WebView2 control works with Microsoft Edge  
                * Tools >> NuGet Package Manager >> Manage NuGet Package Solution >> WebView2 >> Include Prerelease >> Install 
                */
                List<SqlParameter> listPara = new List<SqlParameter>();
                listPara.Add(new SqlParameter("@SOID", DocID.Text));
                string strHtml = GFunc.ExecuteScalar("ARSO_OrderTracking", listPara);
                if (webviewEnsure)
                {
                    webviewOrderTracking.CoreWebView2.NavigateToString(strHtml);
                }
                btnRefresh.Enabled = true;

                /*
                 List<SqlParameter> listPara = new List<SqlParameter>();
                 listPara.Add(new SqlParameter("@SOID", DocID.Text));
                 DataTable dtResult = GFunc.ExecuteProc("ARSO_OrderTracking", listPara);
                 tagrdOrderTracking.DataSource = dtResult;
                 */

            }
            catch (Exception ex)
            {

            }

        }                
        private bool IsItemExported(DataRow CurrentRow)
        {
            bool itemExported = false;
            if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM && CurrentRow != null) /* added by YST on 20191125 to block deletion/replacement of item rows that already exported to simplr */
            {
                //DataRow CurrentRow = ((DataRowView)((UltraGrid)sender).ActiveRow.ListObject).Row;
                if (objFactory.Doc.DocState != (int?)GEnum.DocState.New && CurrentRow["ItmType"] != DBNull.Value &&
                    ((int)CurrentRow["ItmType"] == (int)GEnum.ItemType.Stock ||
                    (int)CurrentRow["ItmType"] == (int)GEnum.ItemType.Non_Stock ||
                    (int)CurrentRow["ItmType"] == (int)GEnum.ItemType.Assembly))
                {
                    string itemExportedCheck = GFunc.ExecuteScalar("select count(1) from SimplrBHGlobal.dbo.OrdItem where DocItmKey = " + CurrentRow["DocItmKey"] + " and OrdNo = '" + DocID.Text + "'");
                    if (itemExportedCheck != "0")
                    {
                        MsgBox.Show("<font color='red'>The item is ready for pickup in the warehouse." +
                                    "<br/>You are not allowed to delete it or replace it with another item in the same line.</font>" +
                                    "<br/><font color='blue'>If you need to cancel the item, please set the quantity to zero and inform the warehouse.</font>"
                                    , GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);

                        itemExported = true;
                    }
                }
            }

            return itemExported;
        }
        #endregion

        private void UpdateSalesRepFromMaster()
        {
            try
            {
                MSTCon objCon = MSTCon.Get(objFactory.Doc.DocConKey);

                if (GFunc.NEInt(objCon.CEMKey, 0) != 0)//Set the sales rep from the master if a default sales rep is present in the master.
                {
                    objFactory.Doc.DocEmKey = objCon.CEMKey;

                }
                if (GFunc.NEInt(objCon.ConChildren, 0) != 0)//Set the head sales from the master if a default head sales is present in the master.
                {
                    objFactory.Doc.DocTranGrpKey = objCon.ConChildren;
                }

                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@DocEmKey", objFactory.Doc.DocEmKey));
                parmList.Add(new SqlParameter("@DocConKey", objFactory.Doc.DocConKey));
                parmList.Add(new SqlParameter("@DocTranGrpKey", objFactory.Doc.DocTranGrpKey));
                parmList.Add(new SqlParameter("@EmInActvie", SqlDbType.Bit));
                parmList.Add(new SqlParameter("@TranGrpKey", SqlDbType.Int));
                parmList.Add(new SqlParameter("@SalesRepAsHeadSales", SqlDbType.Bit));
                parmList.Add(new SqlParameter("@SalesRepTeam", SqlDbType.NVarChar, 50));
                parmList.Add(new SqlParameter("@HeadSalesTeam", SqlDbType.NVarChar, 50));

                parmList[3].Direction = ParameterDirection.Output;
                parmList[4].Direction = ParameterDirection.Output;
                parmList[5].Direction = ParameterDirection.Output;
                parmList[6].Direction = ParameterDirection.Output;
                parmList[7].Direction = ParameterDirection.Output;

                GFunc.ExecuteNonQueryProc("Doc_SalesRepGetCheckData", parmList);

                // set Sales Teams 
                Custom4.SetValueTrigger(GFunc.NEStr(parmList[6].Value, ""), false);
                Custom5.SetValueTrigger(GFunc.NEStr(parmList[7].Value, ""), false);

                //Prompt Warning Message if the sales rep is no longer employed.
                MSTSalesRep objsalesrep = MSTSalesRep.Get(objFactory.Doc.DocEmKey);
                SECUser objUser = SECUser.Get(objsalesrep.UserKey);

                if (objUser.AccDisabled == true)
                {
                    MsgBox.Show("The Sales Rep member, <font color='red'>" + objsalesrep.EmID
                                              + "</font> is no longer with the company."
                                              , GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                }

                objCon = null;
                objsalesrep = null;
                objUser = null;

                //if (objFactory.Doc.DocEmKey != objFactory.Doc.DocTranGrpKey)
                //{
                //    //Prompt Warning Message if Head sales is no longer employed.
                //    MSTSalesRep objsalesrep_Head = MSTSalesRep.Get(objFactory.Doc.DocTranGrpKey);
                //    SECUser objUser_Head = SECUser.Get(objsalesrep_Head.UserKey);

                //    if (objUser_Head.AccDisabled == true)
                //    {
                //        MsgBox.Show("The Head Sales, <font color='red'>" + objsalesrep_Head.EmID
                //                                  + "</font> is no longer with the company."
                //                                  , GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                //    }
                //    objsalesrep_Head = null;
                //    objUser_Head = null;
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
        }//Completed

        //private void SetStarChar(UltraGridRow row, Char ch)
        //{
        //    if (row.Cells["ItmID"].EditorComponent == null)
        //        row.Cells["ItmID"].EditorComponent = new TAUtil.TATextBoxEditor();
        //    if ((row.Cells["ItmID"].EditorComponent).GetType() == typeof(TAUtil.TATextBoxEditor))
        //        ((TAUtil.TATextBoxEditor)(row.Cells["ItmID"].EditorComponent)).PasswordChar = ch;

        //    if (row.Cells["ItmDes"].EditorComponent == null)
        //        row.Cells["ItmDes"].EditorComponent = new TAUtil.TATextBoxEditor();
        //    if ((row.Cells["ItmDes"].EditorComponent).GetType() == typeof(TAUtil.TATextBoxEditor))
        //        ((TAUtil.TATextBoxEditor)(row.Cells["ItmDes"].EditorComponent)).PasswordChar = ch;

        //    if (row.Cells["ItmQty"].EditorComponent == null)
        //        row.Cells["ItmQty"].EditorComponent = new TAUtil.TATextBoxEditor();
        //    if ((row.Cells["ItmQty"].EditorComponent).GetType() == typeof(TAUtil.TATextBoxEditor))
        //        ((TAUtil.TATextBoxEditor)(row.Cells["ItmQty"].EditorComponent)).PasswordChar = ch;
        //}

        private void JobItemsVisibleCheckSet()
        {
            /* added by MayTS */
            if (DefJobKey.DataSource != null)
            {
                DataRow[] drs = objFactory.DocDetItms.Select("ItmBatchKey=9999");

                if (DefJobKey.Rows.Count > 0 && drs.Length > 0)
                {
                    UltraGridRow JobRow = DefJobKey.Rows
                                  .FirstOrDefault(r => GFunc.NEInt(r.Cells["Key"].Value, 0) == GFunc.NEInt(drs[0]["ItmJobKey"], 0));

                    if (JobRow != null)
                    {
                        //DefJobKey.SelectedRow = JobRow;
                        DefJobKey.SetValueTrigger(GFunc.NEInt(drs[0]["ItmJobKey"], 0), false);
                        if (GFunc.NEStr(JobRow.Cells["JobClass"].Value, "").ToLower().Contains("exclusive"))
                            ExclusiveSaleJob = true;
                        else
                            ExclusiveSaleJob = false;

                        if (dtJobEst == null)
                            dtJobEst = MSTJobDetEsts.Get(GFunc.NEInt(DefJobKey.Value, 0));
                    }
                }
            }
        }

    }
}
