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
using Infragistics.Win.UltraWinMaskedEdit;
using System.Transactions;
using TAUtil;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography;
using System.IO;

namespace WinUI
{
    public partial class frmARQO : Form, DocInterface
    {
        #region Local Variables

        private BOLib.ARQOFactory objFactory = null;
        private string ContextMenuSetting = string.Empty;
        private GEnum.SystemCode OpenCode;
        private bool formClose = false;
        private bool ExclusiveSaleJob = false;
        //added by thettm on 29 jan 2018(start)
        private int? source_DK = 0;
        private int? source_DC = 0;
        UltraGridRow pRow = null;
        //added by thettm on 29 jan 2018(end)

        Hashtable htDetailGrd = new Hashtable();
        frmDocList DocListForm = null;
        public GVar.ListEvent_RefreshRecord ListEvent_RefreshRecord = null;
        public GVar.ListEvent_CloseFORM ListEvent_CloseFORM = null;

        //only use to show Read only data viewer for AuditLog 
        private bool IsOpenFromAuditLog = false;
        private DataTable _dtHeader = null;
        private DataSet _dsDetail = null;
        
        private DataTable ConInActive = null;//added by thettm on 12-sept-2017
        // private string link = "http://localhost:8088/estore05feb/";//Thet local
        // private string link = "http://localhost:8088/bhstore/www/"; //May Local
        private string link = "https://bh-estore.com/";

        DataTable dtJobEst = null;
        #endregion

        //Doc Interface method
        public void GetDocInfor(out Document objDoc, out Hashtable detail)
        {
            objDoc = objFactory.Doc;
            AddItm_Hash();
            detail = htDetailGrd;
        }//Completed

        //Initialize
        public frmARQO()
        {
            InitializeComponent();
        }//Completed
        public frmARQO(GEnum.SystemCode DocCodeKey)
        {
            InitializeComponent();
            OpenCode = DocCodeKey;

        }//Completed
        // addby by thettm on 29 jan 2018 (start)
        public frmARQO(int? SOURCE_DC, int? SOURCE_DK)
        {
            //Create Document with data from other source
            InitializeComponent();
            source_DC = SOURCE_DC;
            source_DK = SOURCE_DK;
            OpenCode = GEnum.SystemCode.Quotation;
        }//Completed
        // addby by thettm on 29 jan 2018 (end)
        public frmARQO(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            //Open from AuditLog as Read only data viewer
            InitializeComponent();
            OpenCode = DocCodeKey;
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
        }//Completed

        //Form Events
        private void frm_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            
            if (SysOptionUtility.DatabaseBranchCode != "BHM")
            {
                this.tabDetailList.Tabs[6].Visible = false;
                this.lblEstoreDelType.Visible = false;
                this.taCboEDeliveryTypeAddRem4.Visible = false;
            }                
            try
            {
                //Initialise
                this.objFactory = new BOLib.ARQOFactory(BOLib.GEnum.InstanceMode.Normal, OpenCode);
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
                    objFactory.New(tagrdDetItms);
                    if (source_DK > 0 && !GFunc.IsNEZ(source_DC))
                    {
                        //Create Document from QO,SO 
                        if (objFactory.GetCopy_ByDC(source_DC, source_DK) != GVar.gcPass)
                        {
                            formClose = true;
                            return;
                        }
                        // commented by May on 14 - Feb - 2024
                        // objFactory.Doc.DocTranGrpKey = MSTCon.Get(objFactory.Doc.DocConKey).ConChildren;

                        htDetailGrd.Clear();
                        htDetailGrd.Add(GEnum.Details.Doc_Itm, objFactory.DocDetItms);
                        DocHDRUtil.DocDate_CustomUpdate(objFactory.Doc, htDetailGrd);
                    }
                }

                //Set FORM binding Source and format all grids and filter
                Form_Rebind(true, true);
                if (SysOptionUtility.DatabaseBranchCode.Equals("BHM")|| SysOptionUtility.DatabaseBranchCode.Equals("ADL"))
                    GlobalUI.FormGrids_SetNew(this, objFactory.CodeKey,objFactory.ReloadRefCombo, out ContextMenuSetting);
                else
                {
                    GlobalUI.FormGrids_Set(this, objFactory.CodeKey, objFactory.Doc.IsReadOnly, out ContextMenuSetting);
                    ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(objFactory.CodeKey);
                    GlobalUI.cmnuGlobal_Set(this);                    
                    GlobalUI.Combos_Fill(this, (int)objFactory.Doc.DocCodeKey);
                }
                GridFilter_Set();

                //Set ContextMenu & Grid Setting                           
                //ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(objFactory.CodeKey);
                // //GlobalUI.cmnuGlobal_Set(this);

                // commented by May on 14 - Feb - 2024, setting combo data in  GlobalUI.FormGrids_SetNew function above
                //Fill the list of all combos in Form and Grid / Clear ErrorProvider
                //GlobalUI.Combos_Fill(this, (int)objFactory.Doc.DocCodeKey);
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

                    DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, true);                
                   
                    DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, true);
                    DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItmVendors, GEnum.Details.Doc_ItmVendor, true);
                    DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetVendor, GEnum.Details.Doc_Vendor, true);

                    //Attached drag & drop events 
                    this.tagrdDetItms.DragDrop += new System.Windows.Forms.DragEventHandler(GlobalUI.Grid_DragDropDocItm);
                    this.tagrdDetItms.DragOver += new System.Windows.Forms.DragEventHandler(GlobalUI.Grid_DragOver);
                    this.tagrdDetItms.SelectionDrag += new System.ComponentModel.CancelEventHandler(GlobalUI.Grid_SelectionDrag);
                    this.tagrdDetItms.DisplayLayout.Override.SelectTypeRow = SelectType.ExtendedAutoDrag;
                }

                if (SysOptionUtility.DatabaseBranchCode.Equals(DBCode.BHM))
                {
                    tagrdDetItms.DisplayLayout.Bands[0].Columns["Custom1"].CellActivation = Activation.ActivateOnly;
                    tagrdDetItms.DisplayLayout.Bands[0].Columns["Custom2"].Header.Appearance.BackColor = Color.Blue;
                    tagrdDetItms.DisplayLayout.Bands[0].Columns["Custom2"].Header.Appearance.BackColor2 = Color.Blue;
                    tagrdDetItms.DisplayLayout.Bands[0].Columns["Custom2"].Header.Appearance.ForeColor = Color.LightGreen;
                    tagrdDetItms.DisplayLayout.Bands[0].Columns["Custom2"].CellAppearance.FontData.Italic = DefaultableBoolean.False;
                    tagrdDetItms.DisplayLayout.Bands[0].Columns["Custom2"].CellAppearance.FontData.Bold = DefaultableBoolean.True;
                }
                
                tsbEstore.Visible = objFactory.Doc.DocState != (int)GEnum.DocState.New;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].Header.Caption = "Latest Cost " + DocCurrKey.Text;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmControlPrice"].Header.Caption = "EStore Price " + DocCurrKey.Text;                
               
                tsbCreateRO.Visible = SysOptionUtility.GetBool("UseReserveOrder");

                ControlSetting();

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
                    GlobalUI.Combos_Fill(this, (int)GEnum.SystemCode.Quotation);
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
                DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, formload);              
                DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, formload);
                DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItmVendors, GEnum.Details.Doc_ItmVendor, formload);
                DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetVendor, GEnum.Details.Doc_Vendor, formload);

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

                DisableCreateButtons();
                ControlSetting();

                if (objFactory.Doc.DocState == (int)GEnum.DocState.InProgress)
                    DocState.Appearance.ForeColorDisabled = Color.Blue;
                else
                    DocState.Appearance.ForeColorDisabled = Color.Empty;

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
                if (objFactory.ApprovalRequired)
                {
                    DocState.Visible = SysOptionUtility.DatabaseBranchCode == DBCode.BHM ? true : string.IsNullOrEmpty(DocStatus.Text); /* modified by YST for ADL to show DocState when DocStatus of old transactions are blank */
                    DocStatus.Visible = !DocState.Visible; /* modified by YST */
                    ultraLabel4.Text = "Document Status";

                    if (GFunc.NEStr(objFactory.Doc.DocStatus, "").ToLower().Equals("approved"))
                        tslReadOnly.Text = "Approved";
                    else if (GFunc.NEStr(objFactory.Doc.DocStatus, "").ToLower().Equals("requested") ||
                             GFunc.NEStr(objFactory.Doc.DocStatus, "").ToLower().Equals("recommended"))
                        tslReadOnly.Text = "Pending for Approval";
                }
                else
                    DocStatus.Visible = false;                
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
                 
                    ConInActive = ((DataTable)DocConKey.DataSource).Select("[Inactive]=True").AsEnumerable().CopyToDataTable();
                   
                }
            }         

            if (OpenID.Text=="")
            {               
                ((DataTable)DocConKey.DataSource).DefaultView.RowFilter = "[Inactive]=False";
                DocConKey.DataSource = ((DataTable)DocConKey.DataSource).DefaultView.ToTable();
                GlobalUI.AddComboEmptyValue(DocConKey, true);                
            }
            else if (ConInActive != null)
            {
                if(ConInActive.Select("key="+DocConKey.Value).Count()>0 && ((DataTable)DocConKey.DataSource).Select("key=" + DocConKey.Value).Count()==0)
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
                GridItmVendorBindingSource_Set();
                GridVendorBindingSource_Set();

                if (formload == false)
                    CombosDependent_Fill(string.Empty);

                if (clearError)
                    this.errorProvider1.Clear();

                //to get the count of header's attachment file to show on btnAttachmentEdit.
                btnAttachmentEdit.Text = "(" + objFactory.Doc.Attachments.Count(o => o.DocDItm == -1) + ")";
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
        private void GridItmVendorBindingSource_Set()
        {
            try
            {
                tagrdDetItmVendors.DataSource = objFactory.DocDetItmVendors;
                tagrdDetItmVendors.Rows.Refresh(RefreshRow.ReloadData);
                GlobalUI.GridSequenceSort(objFactory.Doc.DocCodeKey, tagrdDetItmVendors);
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
        private void GridVendorBindingSource_Set()
        {
            try
            {
                tagrdDetVendor.DataSource = objFactory.DocDetVendors;
                tagrdDetVendor.Rows.Refresh(RefreshRow.ReloadData);
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

                //Filter DocDetItmVendor
                int DocItmKey = 0;
                int VendorKey = 0;
                if (tagrdDetItms.ActiveRow != null)
                {
                    DocItmKey = GFunc.NEInt(tagrdDetItms.ActiveRow.Cells["DocItmKey"].Value, 0);

                    if (tagrdDetItmVendors.ActiveRow != null)
                    {
                        VendorKey = GFunc.NEInt(tagrdDetItmVendors.ActiveRow.Cells["VendorKey"].Value, 0);
                    }

                    ((DataTable)tagrdDetItmVendors.DataSource).DefaultView.RowFilter = "DocItmKey=" + DocItmKey;
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
                            if(!cell.Column.Key.ToLower().Equals("itmrem")&& !cell.Column.Key.ToLower().Equals("custom2")&& !cell.Column.Key.ToLower().Equals("itmqty")
                                && !cell.Column.Key.ToLower().Equals("itmmark") && !cell.Column.Key.ToLower().Equals("appoid"))
                                cell.Column.CellActivation = Activation.ActivateOnly;         
                            else
                                cell.Column.CellActivation = Activation.AllowEdit;
                        }

                        if (GFunc.NEDec(row.Cells["ItmBatchKey"].Value, 0) != 9999)
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

                    int itmType = GFunc.GetINTypeGroup(tagrdDetItms.ActiveRow.Cells["ItmType"].Value);

                    switch (itmType)
                    {
                        case (int)GEnum.INTypeGrp.Stock:
                        case (int)GEnum.INTypeGrp.Non_Stock:
                        case (int)GEnum.INTypeGrp.Charges:
                            tagrdDetItmVendors.Enabled = true;
                            break;

                        default:
                            tagrdDetItmVendors.Enabled = false;
                            break;
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
        private void AllDependent_Fill(string controlNm)
        {
            try
            {
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
                /*if (GFunc.CompareString(controlNm, "DocAccKey") || controlNm == string.Empty)
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
                */
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
                if(!GFunc.IsNEZ(objFactory.Doc.DocConKey))
                {
                    if(((TAUtil.TAComboBox)DocShipName).DataSource!=null)
                    {
                        ((DataTable)((TAUtil.TAComboBox)DocShipName).DataSource).DefaultView.RowFilter = "ConKey=" + GFunc.NEInt(objFactory.Doc.DocConKey, 0);
                    }
                    if (((TAUtil.TAComboBox)DefBAddrKey).DataSource != null)
                    {
                        ((DataTable)((TAUtil.TAComboBox)DefBAddrKey).DataSource).DefaultView.RowFilter = "ConKey=" + GFunc.NEInt(objFactory.Doc.DocConKey, 0);
                    }
                    if (((TAUtil.TAComboBox)DefSAddrKey).DataSource != null)
                    {
                        ((DataTable)((TAUtil.TAComboBox)DefSAddrKey).DataSource).DefaultView.RowFilter = "ConKey=" + GFunc.NEInt(objFactory.Doc.DocConKey, 0);
                    }
                    //if (((TAUtil.TAComboBox)DefJobKey).DataSource != null)
                    //{
                    //    ((DataTable)((TAUtil.TAComboBox)DefJobKey).DataSource).DefaultView.RowFilter = "JobConKey=" + GFunc.NEInt(objFactory.Doc.DocConKey, 0) +" or Key=0";
                    //    DefJobKey.Value = DBNull.Value;
                    //}

                    //if(tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmJobKey"].EditorComponent!=null)
                    //    if (((TAUtil.TAComboBox)tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmJobKey"].EditorComponent).DataSource != null)
                    //    {
                    //        ((DataTable)((TAUtil.TAComboBox)tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmJobKey"].EditorComponent).DataSource).DefaultView.RowFilter = "JobConKey=" + GFunc.NEInt(objFactory.Doc.DocConKey, 0);
                    //    }

                }
                /*if (controlNm == "DocShipName" || controlNm == string.Empty)
                    GlobalUI.BindComboValue((TAUtil.TAComboBox)DocShipName, GVar.ListSettingID.MSTShipNameByConKey + "%" + GFunc.NEInt(objFactory.Doc.DocConKey, 0));

                if (controlNm == "DefBAddrKey" || controlNm == string.Empty)
                    GlobalUI.BindComboValue((TAUtil.TAComboBox)DefBAddrKey, GVar.ListSettingID.REFAddrByCon + "%" + GFunc.NEInt(objFactory.Doc.DocConKey, 0));

                if (controlNm == "DefSAddrKey" || controlNm == string.Empty)
                    GlobalUI.BindComboValue((TAUtil.TAComboBox)DefSAddrKey, GVar.ListSettingID.REFAddrByCon + "%" + GFunc.NEInt(objFactory.Doc.DocConKey, 0));

                if (controlNm == "DefJobKey" || controlNm == string.Empty)
                    GlobalUI.BindComboValue((TAUtil.TAComboBox)DefJobKey, "MSTJobSalesFunc_id%" + GFunc.NEInt(objFactory.Doc.DocConKey, 0) + "%" + AppInfor.JobAccessLevel.ToString() + "%" + AppInfor.CurrentUserKey.ToString());

                if (controlNm == "ItmJobKey" || controlNm == string.Empty)
                    GlobalUI.BindComboValue((TAUtil.TAComboBox)tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmJobKey"].EditorComponent, "MSTJobSalesFunc_id%" + GFunc.NEInt(objFactory.Doc.DocConKey, 0) + "%" + AppInfor.JobAccessLevel.ToString() + "%" + AppInfor.CurrentUserKey.ToString());
                */
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

        private void ControlSetting()
        {
            DocPrinted.Enabled = false;

            if (objFactory.ApprovalRequired || SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
            {
                if (GFunc.NEStr(objFactory.Doc.DocStatus, "").ToLower().Equals("approved"))
                    tslReadOnly.Text = "Approved";
                else if (GFunc.NEStr(objFactory.Doc.DocStatus, "").ToLower().Equals("requested") ||
                         GFunc.NEStr(objFactory.Doc.DocStatus, "").ToLower().Equals("recommended"))
                    tslReadOnly.Text = "Pending for Approval";
            }
            if (objFactory.ApprovalRequired && SysOptionUtility.DatabaseBranchCode != DBCode.BHM)
            {
                DocState.Visible = string.IsNullOrEmpty(DocStatus.Text); /* modified by YST */
                DocStatus.Visible = !DocState.Visible; /* modified by YST */
                ultraLabel4.Text = "Document Status";
            }
            else
                DocStatus.Visible = false;

            DocStatus.Enabled = false;
            btnAttachmentEdit.Enabled = objFactory.Doc.DocKey!=0;
            btnAttachmentEdit.Text = "(" + objFactory.Doc.Attachments.Count(o => o.DocDItm == -1) + ")";
            if (SysOptionUtility.DatabaseBranchCode.Equals("BHM"))
                tsbEstore.Visible = objFactory.Doc.DocState != (int)GEnum.DocState.New;
            else
                tsbEstore.Visible = false;           
        }
        //Menu Strip Event
        private void tsbNew_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (!objFactory.Doc.IsReadOnly)//May added on 23-May-2023
                {
                    if (SaveChanges(true, true, false, 
                        (objFactory.Doc.DocStatus == ApprovalStatus.Approved|| !objFactory.ApprovalRequired) ? GEnum.DocAction.Post : GEnum.DocAction.Save) == false)
                        return;
                }               

                //Prepare new instance
                if (objFactory.New(tagrdDetItms) == GVar.gcPass)
                {
                    pRow = null;
                    DocDate.Focus();
                }
                ExclusiveSaleJob = false;
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
                    
                    ExclusiveSaleJob = false;
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
                this.Cursor = Cursors.Default;
            }
        }//Completed
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
                this.Cursor = Cursors.Default;
            }
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
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (objFactory.Delete(tagrdDetItms) == GVar.gcPass)
                    DocDate.Focus();

                Form_RefreshAll(false, true);
                DocList_Refresh();
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
        private void tsbList_Click(object sender, EventArgs e)
        {
            try
            {              

                if (GFunc.IsNE(DocListForm))
                {
                    DocListForm = new frmDocList((int)objFactory.Doc.DocCodeKey);

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
                    {
                        frmMain.gfrmMain.ExistingPrintOutForm((int)objFactory.Doc.DocCodeKey, objFactory.Doc.DocKey.Value);//to activate the Report Form                        
                    }
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
        }//Completed
        private void tsbCreateSO_Click(object sender, EventArgs e)
        {
           
            try
            {               
                CreateDocs((int)GEnum.SystemCode.Sales_Order);
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
        private void tsbCreateRO_Click(object sender, EventArgs e)
        {
            try
            {
                CreateDocs((int)GEnum.SystemCode.Reserve_Order);
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
                if (objFactory.Doc.DocState == (int?)GEnum.DocState.Posted)
                {
                    GlobalUI.PopupDisplay("frmDocRelationship", (GEnum.SystemCode)objFactory.Doc.DocCodeKey, objFactory.Doc.DocKey, objFactory.Doc.DocID);
                }
                else
                {
                    MsgBox.Show("Relationship will be show only for the posted documents.");
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

                //frmAttachment f = new frmAttachment(objFactory.Doc.Attachments, (int?)objFactory.Doc.DocCodeKey, objFactory.Doc.DocKey, -1, 0);
                frmAttachment f = new frmAttachment(objFactory.Doc.Attachments, objFactory.Doc, 1);        //updated by KKAung on 15 Feb 2023     
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
                btnAttachmentEdit.Text = "(" + objFactory.Doc.Attachments.Count(o => o.DocDItm == -1) + ")";

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
                //SaveChanges(false, false, false, GEnum.DocAction.Post);
                int finalapprover = int.Parse(GFunc.ExecuteQuery("select UserKey from MST_SalesRep where EmKey =" + SysOptionUtility.GetInt("DefaultFinalAproverForQuotation").ToString()).Rows[0][0].ToString());
                decimal homeamt = Convert.ToDecimal(GFunc.GetPropertyValue("DocHome", objFactory.Doc).ToString());
                if (AppInfor.CurrentUserKey != finalapprover)
                {
                    decimal totalOfCostOfSale = DocUtility.GetCostOfSaleTotal(DocUtility.CostOfSaleList((DataTable)tagrdDetItms.DataSource, objFactory.Doc));
                    if (homeamt < (totalOfCostOfSale / Convert.ToDecimal(0.85)))
                    {
                        if (MsgBox.Show("Total Amount is under profit margin(15%). Approval is required. Would you like to submit for approval now?", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                        {
                            SaveChanges(false, false, false, GEnum.DocAction.Submit);
                        }
                        return;
                    }
                }

                objFactory.UpdateApprovalInformation(1, "");
                this.OpenRecord(int.Parse(objFactory.Doc.DocKey.ToString()), string.Empty);
                Sendmail(1);
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

        //ttm
        private void Sendmail(int state)
        {
            string subject = "";
            if (state == 1) subject = "Acknowledgement Mail About Approval";
            else
                subject = "Acknowledgement Mail About Rejecting";

            string body = "";
            body = "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>Dear " + DocEmKey.Text.ToString() + ",</p> ";

            if (state == 1) body += "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                    "<b>Quotation (" + objFactory.Doc.DocID.ToString() + ") has been approved.</b></p>";
            else
            {
                body += "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                      "<b>Quotation (" + objFactory.Doc.DocID.ToString() + ") has been rejected";
                if (DisapproveMsg.Text != "")
                    body += " because of " + DisapproveMsg.Text;
                body += ".</b></p>";
            }

            body += "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>Thanks & Best Regards,<br/>" + AppInfor.CurrentUserName + "</p>";

            string toemail = GFunc.ExecuteQuery("SELECT OpValue FROM SYS_Option WHERE OpUserKey =" + objFactory.Doc.DocEmKey + " AND OpID = 'EmailAccount'").Rows[0][0].ToString();
            frmMain.gfrmMain.SetNotifyStatus("Sending Quotation email ...............");
            GEmail.SendSystemEmail(toemail, subject, body, null);
            frmMain.gfrmMain.SetNormalStaus("Ready");
        }
        //ttm
        private void btnReject_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //SaveChanges(false, false, false, GEnum.DocAction.Reject);
                objFactory.UpdateApprovalInformation(2, DisapproveMsg.Text);
                this.OpenRecord(int.Parse(objFactory.Doc.DocKey.ToString()), string.Empty);
                Sendmail(0);
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
                this.Cursor = Cursors.WaitCursor;

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
                        objFactory.DocDetItms = (ARQODetItms)tagrdDetItms.DataSource;


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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed 

        //private void SetStarChar(UltraGridRow row,Char ch)
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
        private void btnGenerateItmVendor_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (MsgBox.Show(MsgID.Document.ConfirmGenerateItemVendor, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                {
                    objFactory.GenerateItmVendor();
                    tagrdDetItmVendors.DataSource = objFactory.DocDetItmVendors;
                    objFactory.Doc.IsDirty = true;
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
        private void btnSend_Click(object sender, EventArgs e)
        {
            bool runProcess = true;
            try
            {
                this.Cursor = Cursors.WaitCursor;

               
                #region validation
                if (objFactory.DocDetVendors.Rows.Count > 0)
                {
                    if (GFunc.NEInt(rfqVendorSendMode.Value, 0) == (int)GEnum.TransmitMode.Email)
                    {
                        if (GFunc.IsNE(rfqVendorMessage.Value) || GFunc.IsNEZ(rfqVendorSendMode.Value) || GFunc.IsNE(rfqVendorRpt.Value) || GFunc.IsNE(rfqVendorSubject.Value))
                            runProcess = false;
                    }
                    else
                    {
                        if (GFunc.IsNEZ(rfqVendorSendMode.Value) || GFunc.IsNE(rfqVendorRpt.Value))
                            runProcess = false;
                    }
                }
                else
                    return;
                #endregion

                if (runProcess)
                {
                    if (SaveChanges(false, false, true, GEnum.DocAction.Undetermine) != true)
                        return;

                    Rfq_Send();
                }
                else
                    MsgBox.Show(MsgID.Validation.IncorrectRFQInformation);
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
        private void btnEditSendList_Click(object sender, EventArgs e)
        {
            try
            {
                frmDocVendSendListSelect f = new frmDocVendSendListSelect(objFactory.Doc, tagrdDetItms, tagrdDetItmVendors);
                f.TopLevel = true;
                f.WindowState = FormWindowState.Normal;
                f.ShowDialog();

                if (f.DialogResult == DialogResult.Yes)
                {
                    objFactory.Doc.IsDirty = true;
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
        private void btnPnL_Click(object sender, EventArgs e)
        {

            try
            {
                if (SaveChanges(true, true, true, GEnum.DocAction.Undetermine) == false)
                    throw new Exception(); 

                GlobalUI.PrintDocumentProfitAndLost((int)GEnum.SystemCode.Quotation,(int)objFactory.Doc.DocKey);
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
                {
                    this.Focus();
                    tagrdDetVendor.DisplayLayout.Bands[0].Columns["EmailAddr"].CellDisplayStyle = CellDisplayStyle.FullEditorDisplay;
                }
                else if (DocListForm != null)
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
        private void OnDocList_DeleteRecord(int key)
        {
            ARQOFactory objFactoryTmp = new ARQOFactory(GEnum.InstanceMode.Normal, OpenCode);
            try
            {
                if (objFactoryTmp.GetReadOnly(key, string.Empty) == GVar.gcPass)
                {
                    if (objFactoryTmp.Doc.DocPrinted && SECPermUtility.Perform(GVar.PermissionID.Save_Printed_Quotation, true) == false)
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
                        objFactory.CopyFrom((GEnum.SystemCode)CopyDocCodeKey, CopyDocKey, this.tagrdDetItms, NSLink, out dtDetail);

                        this.Form_Rebind(false, true);

                        DocHDRUtil.DocTransferData(CopyDocCodeKey, CopyDocKey, (int)objFactory.Doc.DocConKey, dtDetail, objFactory.Doc, tagrdDetItms, 0, "", false, NSLink);

                        AddItm_Hash();
                        if (DocHDRUtil.DocDate_CustomUpdate(objFactory.Doc, htDetailGrd) == false)
                            MsgBox.Show("Unable to calculate document");

                        if (MSTSalesRep.Get(GFunc.NEInt(objFactory.Doc.DocEmKey, 0)).Inactive.Value)
                        {
                            MsgBox.Show("The sale representative is inactive. Please select another one.");
                            
                        }
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
                                    objFactory.Doc.DefJobKey= GFunc.NEInt(drs[0]["ItmJobKey"], 0);
                                    DefJobKey.Value = GFunc.NEInt(drs[0]["ItmJobKey"], 0);
                                    DefJobKey.SelectedRow = row;
                                }
                            }
                        }
                        DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false);//check mic /Pauk change formload=true to false
                        DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, false);//check mic /Pauk change formload=true to false                      
                        ControlSetting();                        
                        break;

                    case GEnum.CopyOption.Import:
                        Form_Rebind(false, true);
                        DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false);//check mic /Pauk change formload=true to false
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
                        objFactory.Doc.DocTranGrpKey = MSTCon.Get(objFactory.Doc.DocConKey).ConChildren;

                        //commented by Jane 06-Mar-2025 - can not set master data to the copied or converted transactions
                        //DocDetUtil.UpdateItmAccFromMaster(objFactory.Doc, tagrdDetItms);//added by Jane 06-Feb-2025
                        //UpdateSalesRepFromMaster();

                        DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false);
                        DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, false);
                        DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItmVendors, GEnum.Details.Doc_ItmVendor, false);
                        DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetVendor, GEnum.Details.Doc_Vendor, false);                       
                        ControlSetting();
                        
                        break;
                }
               
                DisableCreateButtons();

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
                GVar.DocUpdateOption.Remove(GVar.DeptUpdateOption);
            }
        }//Completed
        private void OnDocPrinted()
        {
            //Check if user has permission to edit the already printed document
            this.Focus();
            if (SECPermUtility.Perform(GVar.PermissionID.Save_Printed_Quotation, false) == false)
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
                if (GFunc.IsNE(OpenID.Text) == false)
                    OpenRecord(0, OpenID.Text);

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
                DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false);
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
                AddItm_Hash();
                if (DocHDRUtil.DocConID_CustomUpdate(this, objFactory.Doc, htDetailGrd, DocConKey, GEnum.RecAccessType.CustID, ContextMenuSetting, objFactory.PermID) == false)
                    e.Cancel = true;               

                DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false);
                DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, false);
                AllDependent_Fill(string.Empty);
                DisableCreateButtons();

                //added by thettm on 27 feb 2017(start)
                if (DocConNm.Text == "CASHESTORE")
                    objFactory.Doc.DocTypeNm = "eStore Quotation";
                //added by thettm on 27 feb 2017(end)
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].Header.Caption = "Latest Cost " + DocCurrKey.Text;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmControlPrice"].Header.Caption = "EStore Price " + DocCurrKey.Text;
                if (pRow != null) CalCulateProcessFee();
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
                DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false);
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

                DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false);                
                DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, false);
                AllDependent_Fill(string.Empty);
                DisableCreateButtons();

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
                DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false);
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].Header.Caption = "Latest Cost " + DocCurrKey.Text;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmControlPrice"].Header.Caption = "EStore Price " + DocCurrKey.Text;
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
        private void DocDateValid_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (DocHDRUtil.DocDateValid_CustomUpdate(objFactory.Doc, DocDateValid.DateValue) == false)
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
                if (DocHDRUtil.DocTypeNm_CustomUpdate(objFactory.Doc, htDetailGrd, GFunc.NEStr(DocTypeNm.Value, string.Empty)) == false)
                    e.Cancel = true;
                //added by thettm on 27 feb 2017 (start)
                if (objFactory.Doc.DocTypeNm == "eStore Quotation")
                {                 
                    DataTable dt =(DataTable) DocConKey.DataSource;
                    DataRow[] dr = dt.Select("ID='CASHESTORE'");
                    objFactory.Doc.DocConKey = Convert.ToInt16(dr[0].ItemArray[0].ToString());
                    
                     if (DocHDRUtil.DocConID_CustomUpdate(this, objFactory.Doc, htDetailGrd, DocConKey, GEnum.RecAccessType.CustID, ContextMenuSetting, objFactory.PermID) == false)
                        e.Cancel = true;

                    DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false);                   
                    DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, false);
                    AllDependent_Fill(string.Empty);
                    DisableCreateButtons();

                    if (tagrdDetItms.Rows.Count > 0)
                    {
                        pRow = tagrdDetItms.Rows.OfType<UltraGridRow>().ToList().Find
                           (row => (int)row.Cells["ItmKey"].Value == SysOptionUtility.ProcessingItem/*102937*/);
                        CalCulateProcessFee();
                    }

                }
                //added by thettm on 27 feb 2017 (start)
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
                    decimal subTotal = GFunc.NEDec(objFactory.Doc.DocSubTotal,0) - GFunc.NEDec(tagrdDetItms.Rows[pRow.Index].Cells["ItmAmtF"].Value, 0);
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
                //int defJobKey = GFunc.NEInt(this.DefJobKey.Value, 0);
                //AddItm_Hash();
                //if (DocHDRUtil.DefJob_CustomUpdate(objFactory.Doc, htDetailGrd, defJobKey, false) == false)
                //    e.Cancel = true;
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
        private void rfqVendorSendMode_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if ((int)rfqVendorSendMode.Value == (int)GEnum.TransmitMode.Email)
                {
                    rfqVendorMessage.Visible = true;
                    rfqVendorSubject.Visible = true;
                    rfqVendorMessageLabel.Visible = true;
                    rfqVendorSubjectLabel.Visible = true;
                }
                else
                {
                    rfqVendorMessage.Visible = false;
                    rfqVendorSubject.Visible = false;
                    rfqVendorMessageLabel.Visible = false;
                    rfqVendorSubjectLabel.Visible = false;
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
        private void DetailSN_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                FilterItmVendorGrid(false);
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
        private void tabDetailList_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                if (tabDetailList.SelectedTab.Text == "RFQ Vendor")
                {
                    objFactory.InsertVendorGrid();
                    tagrdDetVendor.DataSource = objFactory.DocDetVendors;
                }
                if (SysOptionUtility.DatabaseBranchCode == "BHM")
                {
                    if (tabDetailList.SelectedTab.Text == "AR Aging Status")
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
        private void tabDetailList_KeyDown(object sender, KeyEventArgs e)
        {
            try
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

                            case "tsbrfq":
                                GlobalUI.TabKeyDownForGrid(tagrdDetVendor);
                                break;
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
        }//Completed

        //Grid Events
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
                FilterItmVendorGrid(true);
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
                if (GFunc.CompareString(e.Cell.Column.Key, "ItmAttachment"))
                {
                    if (tagrdDetItms.ActiveRow.Update())
                        DocDetUtil.ItmAttachment_btnClick(this, objFactory.Doc.Attachments, objFactory.Doc, tagrdDetItms);
                }
                else if (GFunc.CompareString(e.Cell.Column.Key, "APPOID"))
                {

                   // TAUtil.TATextBoxEditor txt = e.Cell.Column.Editor;
                   UltraGrid grid=sender as UltraGrid;
                   int left = 0, top = 0;
                   if(this.WindowState==FormWindowState.Maximized)
                   {
                       left = this.Left+ grid.Left + grid.DisplayLayout.UIElement.CurrentMousePosition.X-e.Cell.Column.Width;
                       top = this.Top + grid.Top + grid.DisplayLayout.UIElement.CurrentMousePosition.Y;
                   }
                   else
                   {
                       left = this.Left + grid.Left + grid.DisplayLayout.UIElement.CurrentMousePosition.X - e.Cell.Column.Width;
                       top = this.Top + grid.Top+ tabDetailList.Top + tspBar.Height +  grid.DisplayLayout.UIElement.CurrentMousePosition.Y;
                   }
                   cmnuPOBL.Show(new Point(left, top));
                   /*
                    int DocKey = 0;
                    int DocItmKey = 0;
                    int itmtype = 0;
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

                                if (dt.Rows.Count > 0)
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
                    htDetailGrd.Clear();
                    htDetailGrd.Add(GEnum.Details.Doc_Itm, tagrdDetItms);
                    htDetailGrd.Add(GEnum.Details.Doc_ItmVendor, tagrdDetItmVendors);

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
                    FilterItmVendorGrid(true);
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
        private DataTable GetDocKeyByNSLink(string NSLink, bool IsPO)
        {
            string proc = "Doc_GetPOKeyByNSLink";

            if (!IsPO)
            {
                proc = "Doc_GetPIVKeyByNSLink";
            }

            List<SqlParameter> parList = new List<SqlParameter>();
            parList.Add(new SqlParameter("@NSLink", NSLink));

            DataTable dt = GFunc.ExecuteProcReader(proc, parList);

            return dt;
        }

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
                this.Cursor = Cursors.WaitCursor;
                //commented by Jane on 02-Dec-2013. 
                //After type itmpriceafter, then go to another form. This event not fire.
                //Need to fire this event for active cell to get related value updated if you go to another form also.
                //if (formClose || frmMain.gfrmMain.ActiveMdiChild != this || tagrdDetItms.ActiveCell == null)
                //    return;

                if (formClose || tagrdDetItms.ActiveCell == null)
                    return;
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
                            DocKey = GetDocKey(e.Cell.Text, itmtype == 600);
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
                                if (f != null)
                                    f.Close();
                            }
                            else
                            {
                                MsgBox.Show("The PO link will be cancelled as you did not select a PO and SN correctly.");
                                e.Cancel = true;
                                return;
                            }
                        }
                        else if (e.Cell.OriginalValue != "")
                        {
                            e.Cell.Row.Cells["NSLink"].Value = "11150-" + objFactory.Doc.DocKey + "-" + e.Cell.Row.Cells["DocItmKey"].Value;
                        }
                        break;
                }
                AddItm_Hash();
                string listSetingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, e.Cell.Column.Key, ((Control)sender).Name);
                if (DocDetUtil.ItmRow_CustomCellUpdate(objFactory.Doc, htDetailGrd, GEnum.Details.Doc_Itm, listSetingID, ExclusiveSaleJob?1:0) == false)
                    e.Cancel = true;

                if (objFactory.Doc.DocTypeNm.ToUpper().Contains("ESTORE"))
                {
                    switch (e.Cell.Column.Key)
                    {
                        case "ItmID":
                        case "ItmDes":
                            if (GFunc.NEInt(e.Cell.Row.Cells["ItmKey"].Value, 0) == SysOptionUtility.ProcessingItem/*102937*/)
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
                            if (pRow != null) CalCulateProcessFee(); //added by thettm on 16 jan 2019
                            break;
                    }
                }

                int itmType = GFunc.GetINTypeGroup(tagrdDetItms.ActiveRow.Cells["ItmType"].Value);
                switch (itmType)
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                    case (int)GEnum.INTypeGrp.Charges:
                        tagrdDetItmVendors.Enabled = true;
                        break;

                    default:
                        tagrdDetItmVendors.Enabled = false;
                        break;
                }

                if (e.Cell.Column.Key.Equals("ItmQty"))
                {
                    if (GFunc.NEInt(tagrdDetItms.ActiveRow.Cells["ItmBatchKey"].Value, 0)==9999 && dtJobEst!=null)//If Job header row
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
                            if (GFunc.NEInt(row.Cells["ItmBatchKey"].Value, 0)>0 && GFunc.NEInt(row.Cells["LineType"].Value, 0) ==1000 
                                && GFunc.NEInt(row.Cells["ItmJobKey"].Value, 0)==JobKey)
                            {   //Job Items Row, no price only Qty
                                DataRow[] drs = dtJobEst.Select("JobEstKey=" + GFunc.NEInt(row.Cells["ItmBatchKey"].Value, 0));
                                if(drs.Length>0)
                                {
                                    row.Cells["ItmQty"].Value = GFunc.NEDec(drs[0]["EstQty"],0) * JobHQty;                                    
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
                FilterItmVendorGrid(true);

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
                FilterItmVendorGrid(true);
                ShowGPSummary();
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
                if(e.Rows.Count()>0)
                    if ( GFunc.NEInt(e.Rows[0].Cells["ItmBatchKey"].Value,0)>0)
                    {
                        MsgBox.Show("Not allow to delete this row which is addded from Job.");
                        e.Cancel = true; // Cancels the delete action
                        return;          // Stop checking further
                    }
                bool clearprocessing = false;
                if (pRow != null && pRow.Index == ((UltraGrid)sender).ActiveRow.Index) clearprocessing = true;
                this.Cursor = Cursors.WaitCursor;
                e.DisplayPromptMsg = false;

                htDetailGrd.Clear();
                htDetailGrd.Add(GEnum.Details.Doc_Itm, tagrdDetItms);
                htDetailGrd.Add(GEnum.Details.Doc_ItmVendor, tagrdDetItmVendors);
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
            try
            {
                objFactory.DocDetItms.AcceptChanges();
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

        private void tagrdDetItmVendors_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                string listSetingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, e.Cell.Column.Key, ((Control)sender).Name);
                int detkey = GFunc.NEInt(tagrdDetItms.ActiveRow.Cells["DocItmKey"].Value, 0);
                int itmkey = GFunc.NEInt(tagrdDetItms.ActiveRow.Cells["ItmKey"].Value, 0);
                decimal qty = GFunc.NEDec(tagrdDetItms.ActiveRow.Cells["ItmQty"].Value, 0);

                switch (tagrdDetItmVendors.ActiveCell.Column.Key.ToLower())
                {
                    case "vendorkey":
                        DocDetUtil.ItmVendorVendID_btnClick(objFactory.Doc, (UltraGrid)tagrdDetItmVendors, e.Cell, GEnum.PopupType.VendID, listSetingID, detkey, itmkey, qty);
                        break;

                    case "vendornm":
                        DocDetUtil.ItmVendorVendID_btnClick(objFactory.Doc, (UltraGrid)tagrdDetItmVendors, e.Cell, GEnum.PopupType.VendNm, listSetingID, detkey, itmkey, qty);
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
        }//Completed
        private void tagrdDetItmVendors_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
                switch (tagrdDetItmVendors.ActiveCell.Column.Key.ToLower())
                {
                    case "vendoreqitmqty":
                        tagrdDetItmVendors.ActiveRow.Cells["VendorEqItmQty"].Value = GFunc.NEDec(tagrdDetItmVendors.ActiveRow.Cells["VendorEqItmQty"].Value, 0);
                        break;
                    case "vendoreqitmprice":
                        tagrdDetItmVendors.ActiveRow.Cells["VendorEqItmPrice"].Value = GFunc.NEDec(tagrdDetItmVendors.ActiveRow.Cells["VendorEqItmPrice"].Value, 0);
                        break;
                    case "vendoritmqty":
                        tagrdDetItmVendors.ActiveRow.Cells["VendorItmQty"].Value = GFunc.NEDec(tagrdDetItmVendors.ActiveRow.Cells["VendorItmQty"].Value, 0);
                        break;
                    case "vendoritmprice":
                        tagrdDetItmVendors.ActiveRow.Cells["VendorItmPrice"].Value = GFunc.NEDec(tagrdDetItmVendors.ActiveRow.Cells["VendorItmPrice"].Value, 0);
                        break;
                    case "vendorstatus":
                        tagrdDetItmVendors.ActiveRow.Cells["VendorStatus"].Value = GFunc.NEDec(tagrdDetItmVendors.ActiveRow.Cells["VendorStatus"].Value, 10);//value of 10 in status = Ready
                        break;
                    case "vendorkey":
                    case "vendornm":
                        int detkey = 0;
                        int itmkey = 0;
                        decimal qty = 0;

                        string listSetingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, e.Cell.Column.Key, ((Control)sender).Name);
                        if (tagrdDetItms.ActiveRow != null)
                        {
                            detkey = GFunc.NEInt(tagrdDetItms.ActiveRow.Cells["DocItmKey"].Value, 0);
                            itmkey = GFunc.NEInt(tagrdDetItms.ActiveRow.Cells["ItmKey"].Value, 0);
                            qty = GFunc.NEDec(tagrdDetItms.ActiveRow.Cells["ItmQty"].Value, 0);
                        }
                        if (detkey == 0 || itmkey == 0)
                        {
                            MsgBox.Show("You must select an Item in the item detail list");
                            e.Cancel = true;
                            return;
                        }

                        if (tagrdDetItmVendors.ActiveCell.Column.Key.ToLower() == "vendorkey")
                        {
                            if (DocDetUtil.ItmVendorVendID_CustomUpdate(objFactory.Doc, tagrdDetItmVendors, listSetingID, GEnum.RecAccessType.VendID, detkey, itmkey, qty) == false)
                                e.Cancel = true;
                        }
                        else
                        {
                            if (DocDetUtil.ItmVendorVendID_CustomUpdate(objFactory.Doc, tagrdDetItmVendors, listSetingID, GEnum.RecAccessType.VendNm, detkey, itmkey, qty) == false)
                                e.Cancel = true;
                        }
                        break;

                    case "selected":
                        if (GFunc.IsNEZ(tagrdDetItmVendors.ActiveRow.Cells["VendorKey"].Value))
                        {
                            MsgBox.Show(MsgID.Common.CannotBeEmpty + "%Vendor");
                            e.Cancel = true;
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
        }//Completed
        private void tagrdDetItmVendors_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            try
            {


                DataRow NewRow = ((DataRowView)e.Row.ListObject).Row;
                if (objFactory.DocDetItmVendor_Validation(NewRow) == false)
                {
                    e.Cancel = true;
                    this.tagrdDetItms.PerformAction(UltraGridAction.PrevCell);
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
        }
        private void tagrdDetItmVendors_AfterRowUpdate(object sender, RowEventArgs e)
        {
            objFactory.Doc.IsDirty = true;
        }//Completed
        private void tagrdDetItmVendors_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                e.DisplayPromptMsg = false;

                if (SysOptionUtility.GetBool("WarnDeleteDocumentDetail"))
                {
                    if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) != GEnum.MsgBoxButton.Delete)
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void tagrdDetItmVendors_AfterRowsDeleted(object sender, EventArgs e)
        {
            try
            {
                objFactory.DocDetItmVendors.AcceptChanges();
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

        private void tagrdDetVendor_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                TAUtil.TAComboBox tcb = null;
                if (e.Row.Cells["EmailAddr"].EditorComponent == null)
                {
                    tcb = new TAUtil.TAComboBox();
                    tcb.LimitToList = false;
                    tcb.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
                    tcb.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    tcb.TextRenderingMode = TextRenderingMode.GDI;
                    tcb.Visible = false;
                    e.Row.Cells["EmailAddr"].EditorComponent = tcb;
                }
                else
                    tcb = e.Row.Cells["EmailAddr"].EditorComponent as TAUtil.TAComboBox;

                GlobalUI.BindComboValue(tcb, "REFAddrEmail_Con%" + GFunc.NEInt(e.Row.Cells["VendorKey"].Value, 0));
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
        private void tagrdDetVendor_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                htDetailGrd.Clear();
                htDetailGrd.Add(GEnum.Details.Doc_Itm, tagrdDetItms);

                string listSetingID = GlobalUI.FormSettingID_Get(tagrdDetVendor);
                DocDetUtil.DetItmGrid_CellButtonClick(objFactory.Doc, htDetailGrd, e.Cell, listSetingID);
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
        private void tagrdDetVendor_AfterRowUpdate(object sender, RowEventArgs e)
        {
            objFactory.Doc.IsDirty = true;
        }//Completed

        private void tagrdDetVendor_BeforeRowUpdate(object sender, Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e)
        {
            if (GFunc.IsNE(e.Row.Cells["TransmitMode"].Value))
                e.Row.Cells["TransmitMode"].Value = 0;
        }
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
                this.tagrdDetItmVendors.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdDetItmVendors.UpdateData();
                this.tagrdDetVendor.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdDetVendor.UpdateData();

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

                #region tagrdDetItmVendors
                if (tagrdDetItmVendors.ActiveRow != null)
                {
                    if (tagrdDetItmVendors.ActiveRow.DataChanged && !tagrdDetItms.ActiveRow.IsUnmodifiedTemplateAddRow)
                    {
                        if (undoChangesInGrid)
                        {
                            //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                            this.tagrdDetItmVendors.PerformAction(UltraGridAction.UndoCell);
                            this.tagrdDetItmVendors.PerformAction(UltraGridAction.UndoRow);
                        }
                        return true;
                    }
                }
                #endregion

                #region tagrdDetVendor
                if (tagrdDetVendor.ActiveRow != null)
                {
                    if (tagrdDetVendor.ActiveRow.DataChanged && !tagrdDetItms.ActiveRow.IsUnmodifiedTemplateAddRow)
                    {
                        if (undoChangesInGrid)
                        {
                            //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                            this.tagrdDetVendor.PerformAction(UltraGridAction.UndoCell);
                            this.tagrdDetVendor.PerformAction(UltraGridAction.UndoRow);
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
        private bool SaveChanges(bool canDiscardChanges, bool saveOnlyWhenDirty, bool promptToSave, GEnum.DocAction ButtonAction)
        {
            bool result = false;
            System.Threading.Thread th = null;
            frmShowProgress f = null;
            GEnum.MsgBoxButton btnSelect;
            try
            {                
                if (objFactory.Doc.IsReadOnly)
                    return true;
                //if (pRow != null && objFactory.Doc.IsDirty) CalCulateProcessFee();
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
                            if(objFactory.Doc.DocState==(int)GEnum.DocState.New && objFactory.Doc.Attachments.Count>0)
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
                    else if (btnSelect == 0)//btnSelect = 0 is the red X button on the top right corner of the msg.
                    {
                        this.objFactory.Doc.IsDirty = false;
                        IsGridsDirty(true);
                        return true;
                    }
                }

                if (GFunc.NEInt(DocConKey.Value, 0) > 0)
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
                                    "<br/>System does not allow you to proceed with the Reserve Order." +
                                    "<br/>Please check with the Management or Finance team to enable this customer for sales.", GEnum.MsgBoxIcon.Error, GEnum.MsgBoxButton.OK);

                            DocConKey.Focus();
                            return false;
                        }
                    }

                    //if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM && !GFunc.IsNEZ(DefJobKey.Value))
                    //{
                    //    if (objFactory.Doc.DocDate >= Convert.ToDateTime("2025-08-20") && objFactory.Doc.IsDirty)
                    //    {
                    //        decimal?  QOAmtH = objFactory.Doc.DocSubTotal * objFactory.Doc.DocCurrRate;                           

                    //        decimal NSum = (tagrdDetItms.DataSource as DataTable).AsEnumerable().Sum(r => GFunc.NEDec(r.Field<decimal?>("ItmVendorPrice"),0));
                    //        UltraGridRow row = DefJobKey.SelectedRow;
                            
                    //        if(row!=null)
                    //        {                                
                    //            decimal min = GFunc.NEDec(row.Cells["MinMarkupSalePercent"].Value,0);
                    //            decimal max = GFunc.NEDec(row.Cells["MaxMarkupSalePercent"].Value, 0);

                    //            if(QOAmtH<(NSum*(1+min/100M)))
                    //            {
                    //                MsgBox.Show("Quotation amount cannot be less than "+ (NSum * (1 + min / 100M)).ToString("#,###.##")+" = " + min + " % of Project Cost.",GEnum.MsgBoxIcon.Warning,GEnum.MsgBoxButton.OK);
                    //                return false;
                    //            }
                    //            else if(QOAmtH > (NSum * (1+max / 100M)))
                    //            {
                    //                if(MsgBox.Show("Quotation amount is more than " + (NSum * (1 + max / 100M)).ToString("#,###.##") + " = " + max + " % of Project Cost."
                    //                   +"<br/>Would you like to Proceed?" , GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Yes,GEnum.MsgBoxButton.No)==GEnum.MsgBoxButton.No)
                    //                return false;
                    //            }
                                   
                    //        }
                    //    }

                    //}
                }

                //Save any pending changes (note: if saveOnlyWhenDirty (false), it will always save regardless of Isdirty State)
                if (objFactory.Doc.IsDirty || saveOnlyWhenDirty == false)
                {
                    bool updateDoc = false;

                    if (ButtonAction == GEnum.DocAction.Post)
                    {
                        /* Prompt warning message for Request Remark for Athena, added by YST on 2023/11/29, requested by Jasper, confirmed by Ken */
                        if (SysOptionUtility.DatabaseBranchCode == DBCode.ADL)
                        {
                            if (objFactory.Doc.DocRemAdditional3 == null || objFactory.Doc.DocRemAdditional3.Trim() == "")
                            {
                                MsgBox.Show("Request remark is required to request the approval.", GEnum.MsgBoxIcon.Serious, GEnum.MsgBoxButton.OK);
                                tabDetailList.SelectedTab = tabDetailList.Tabs["tsbMain"];
                                DocRemAdditional3.Focus();
                                return false;
                            }
                        }

                        if (objFactory.ApprovalRequired && SysOptionUtility.DatabaseBranchCode != DBCode.BHM)
                            if (MsgBox.Show("The quotation will not be able to change after posting.<br/> Are you sure to proceed to request approval?",
                               GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                                return false;
                        updateDoc = true;
                    }
                    else if (ButtonAction == GEnum.DocAction.Undetermine)
                    {
                        if (DocUtility.ButtonAction_Get(objFactory.Doc, ref ButtonAction) == false)
                            return false;
                    }                   

                    //Update data in DetVendor
                    objFactory.InsertVendorGrid();

                    /* Prompt warning message for the price 0 of the items added by YST on 2023/04/03 */
                    if ((SysOptionUtility.DatabaseBranchCode == DBCode.BHM || 
                         SysOptionUtility.DatabaseBranchCode == DBCode.SOP) && 
                            ButtonAction == GEnum.DocAction.Post)
                    {
                        if (CheckPriceZero() == false)
                            return false;
                    }

                    #region Saving
                        
                        string msg = "";
                        try
                        {
                            if (ButtonAction == GEnum.DocAction.Post && objFactory.ApprovalRequired && SysOptionUtility.DatabaseBranchCode != "BHM")
                                msg = "Saving && sending for approval...";
                            else if (ButtonAction == GEnum.DocAction.Save || !objFactory.ApprovalRequired)
                                msg = "                Saving Changes.....";

                            try
                            {
                                f = new frmShowProgress(msg);
                                th = new System.Threading.Thread(() => ShowForm(f));
                                th.Start();
                            }
                            catch { }

                            //Saving
                            if (objFactory.Save((int)ButtonAction, frmMain.gfrmMain) == GVar.gcPass)
                                result = true;
                            msg = "";

                            if (result && SysOptionUtility.HasDMASLink && SysOptionUtility.DatabaseBranchCode != "BHM"
                            && objFactory.ApprovalRequired && ButtonAction == GEnum.DocAction.Post)
                            {
                                if (objFactory.DocDetItms.Rows.Count > 0)
                                    DocHDRUtil.ExportToDMAS(objFactory.Doc, objFactory.ApprovalRequired);

                                List<SqlParameter> parlist = new List<SqlParameter>();
                                parlist.Add(new SqlParameter("@DocKey", objFactory.Doc.DocKey));
                                parlist.Add(new SqlParameter("@ApprovalStatus", "Requested"));

                                string status = GFunc.ExecuteScalar("ARQO_Approval",parlist);

                                if (status.Contains("Requested"))
                                    objFactory.Doc.DocStatus = "Requested";
                                else if (status.Contains("Approved"))
                                    objFactory.Doc.DocStatus = "Approved";
                                else if (status.Contains("Recommended"))
                                    objFactory.Doc.DocStatus = "Recommended";
                                else if (status.Contains("Rejected"))
                                    objFactory.Doc.DocStatus = "Rejected";
                                else
                                    objFactory.Doc.DocStatus = GFunc.ExecuteScalar("exec DocState_Get 11100,"+objFactory.Doc.DocKey+",'"+SysOptionUtility.DatabaseBranchCode+"'"); 

                                objFactory.Doc.Attachments.Fetch(new SYSAttachments.Criteria((int)objFactory.Doc.DocCodeKey, objFactory.Doc.DocKey, objFactory.Doc.DocStatus=="Approved"?1:5));
                                btnAttachmentEdit.Text = "(" + objFactory.Doc.Attachments.Count + ")";
                                if (objFactory.Doc.DocStatus == ApprovalStatus.Approved ||
                                    objFactory.Doc.DocStatus == ApprovalStatus.Recommended ||
                                    objFactory.Doc.DocStatus == ApprovalStatus.Requested )
                                {
                                    objFactory.Doc.IsReadOnly = true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            msg = ex.Message;
                        }
                        finally
                        {   
                            if (msg != "")
                                MsgBox.Show(msg);

                            try
                            {
                                if (result)
                                {
                                    Form_RefreshAll(false, false);
                                    DocList_Refresh();
                                }
                                if (th != null)//May added on 18-May-2023..Refreshing everytime take a while with VPN. No need to refresh if no changes
                                {
                                    if (f != null)
                                    {
                                        f.CloseMe();
                                        f = null;
                                    }
                                    //th.Abort();
                                }
                            }
                            catch
                                { }
                        }        
                    
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
        }//Completed

        private void ShowForm(frmShowProgress fprg)
        {            
            if(fprg!=null)
                fprg.ShowDialog();
        }


        public bool OpenRecord(int key, string id)
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
                btnAttachmentEdit.Text = "(" + objFactory.Doc.Attachments.Count + ")";


                if (tagrdDetItms.Rows.Count>0)
                {

                    pRow = tagrdDetItms.Rows.OfType<UltraGridRow>().ToList().Find
                        (row => (int)row.Cells["ItmKey"].Value == SysOptionUtility.ProcessingItem/*102937*/);
                }

                #endregion
                OpenID.Text = DocID.Value.ToString(); 
                FilterCustomer(); //added by thettm on 12-sept-2017
                
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
            bool runQuoteStatus = false;
            int spResult = 0;
            System.Threading.Thread th = null;
            frmShowProgress f = null;

            if (SaveChanges(false, true, true, GEnum.DocAction.Undetermine))
                    runCheckProcess = true;
               

                #region Check for if Document has been already been created
                if (runCheckProcess)
                {
                    if (objFactory.Doc.DocState == (int)GEnum.DocState.Posted)
                    {
                        DataSet ds = new DataSet();
                        List<SqlParameter> parmList = new List<SqlParameter>();
                        parmList.Add(new SqlParameter("@source_DC", objFactory.Doc.DocCodeKey));
                        parmList.Add(new SqlParameter("@source_DID", objFactory.Doc.DocID));
                        parmList.Add(new SqlParameter("@source_DK", objFactory.Doc.DocKey));
                        parmList.Add(new SqlParameter("@destination_DC", destination_DC));
                        parmList.Add(new SqlParameter("@RetValue", spResult));
                        parmList[4].Direction = ParameterDirection.Output;
                        ds = GFunc.ExecuteProcDataSet("Doc_CreateDocs_Check", parmList);

                        #region "commented by KKAung on 04 Jun 2023"
                        /*
                        if (destination_DC == (int)GEnum.SystemCode.Purchase_Order)
                        {
                            //PO to be created.
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                frmConfirmPO fPO = new frmConfirmPO(ds.Tables[1], (int)objFactory.Doc.DocCodeKey, (int)objFactory.Doc.DocKey, objFactory.Doc.DocID);
                                if (fPO.ShowDialog() == DialogResult.OK)
                                {
                                    runCreateProcess = true;
                                    runQuoteStatus = false;
                                    objFactory.GetEdit((int)objFactory.Doc.DocKey, objFactory.Doc.DocID);
                                    Form_RefreshAll(false, true);
                                    frmMain.gfrmMain.ShowExistingPopupForm("frmPrintSelectionList");
                                }
                                fPO.Dispose();
                            }
                            //show Existing PO
                            if (runCreateProcess == false && ds.Tables[0].Rows.Count > 0)
                            {
                                if (MsgBoxGrid.Show("Already Created. Would you like to print " + (ds.Tables[0].Rows.Count == 1 ? "it" : "them") + "?", ds.Tables[0], GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                                {
                                    frmPrintSelectionList fLst = new frmPrintSelectionList(GEnum.SystemCode.Purchase_Order, 40, objFactory.Doc.DocID);
                                    fLst.Show();
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
                        MsgBox.Show("Cannot create Document when quotation has not been posted");
                        return false;
                    }
                }
            #endregion

            #region Create document process
            try
            {
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
                        f = new frmShowProgress("     Creating " + ((GEnum.SystemCode)destination_DC).ToString() + "...");
                        th = new System.Threading.Thread(() => ShowForm(f));
                        th.Start();
                    }
                    catch { }

                    switch (destination_DC)
                    {
                        case (int)GEnum.SystemCode.Reserve_Order:

                            frmARRO frmRO = new frmARRO(objFactory.Doc.DocCodeKey, objFactory.Doc.DocKey);
                            frmRO.MdiParent = frmMain.gfrmMain;
                            frmRO.Show();
                            runQuoteStatus = true;
                            break;
                        case (int)GEnum.SystemCode.Sales_Order:
                            ////bool CashCustomer = GFunc.NEInt(DocConKey.SelectedRow.Cells["CCBType"].Value, 0) == (int)GEnum.CCBType.CH; //added by KKAung on 30 Aug 2021 
                            //int CCBType = GFunc.NEInt(DocConKey.SelectedRow.Cells["CCBType"].Value, 0);     //added by KKAung on 13 Jan 2022
                            //bool CashCustomer = CCBType == (int)GEnum.CCBType.CH || (CCBType == (int)GEnum.CCBType.B && (DocTermKey.Text == "CASH" || DocTermKey.Text == "TT PAYMENT"));    //added by KKAung on 13 Jan 2022
                            //frmARSO frmSO = new frmARSO(objFactory.Doc.DocCodeKey, objFactory.Doc.DocKey,CashCustomer);  // updated by KKAung on 30 Aug2021

                            bool CashCustomer = GFunc.NEInt(DocConKey.SelectedRow.Cells["CCBType"].Value, 0) == (int)GEnum.CCBType.CH; //added by KKAung on 18 Jan 2022 
                            frmARSO frmSO = new frmARSO(objFactory.Doc.DocCodeKey, objFactory.Doc.DocKey, CashCustomer);  // updated by KKAung on 18 Jan 2022
                            frmSO.MdiParent = frmMain.gfrmMain;
                            frmSO.Show();
                            runQuoteStatus = true;
                            break;
                       
                    }
                }
                #endregion

                #region Set Quote Status
                if (runQuoteStatus)
                {
                    bool isdirty = objFactory.Doc.IsDirty;
                    DocQuoteStatus.SetValueTrigger(20, false);//won

                    List<SqlParameter> parmList = new List<SqlParameter>();
                    parmList.Add(new SqlParameter("@DocKey", objFactory.Doc.DocKey));
                    parmList.Add(new SqlParameter("@Status", objFactory.Doc.DocQuoteStatus));
                    parmList.Add(new SqlParameter("@RetValue", 0));
                    parmList[2].Direction = ParameterDirection.Output;

                    GFunc.ExecuteProc("ARQO_Status_Set", parmList);

                    //restore the dirty state
                    objFactory.Doc.IsDirty = isdirty;
                    this.Close();
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
            return true;
        }//Completed
        private void Rfq_Send()
        {

            string _recordSource = string.Empty;
            bool RecordForPreview = true;
            bool sendSuccessful = false;
            ReportLoader ReportLoader = new ReportLoader();
            int RepKey = 11102;
            int transmitMode = (int)this.rfqVendorSendMode.Value;
            string Subject = rfqVendorSubject.Text;
            string Message = rfqVendorMessage.Text;
            List<SqlParameter> parmList = new List<SqlParameter>();
            DataTable objDetVendors = objFactory.DocDetVendors;

            //Loop thru all DocDetVendor and send RFQ when the transmit mode matched
            for (int i = 0; i < objDetVendors.Rows.Count; i++)
            {
                DataRow dr = objDetVendors.Rows[i];

                sendSuccessful = false;
                if ((int)dr["TransmitMode"] == transmitMode)
                {
                    try
                    {
                        ReportLoader._reportFileName = this.rfqVendorRpt.Value.ToString();

                        //Common Process before Print/Fax/Email/Preview
                        parmList.Clear();
                        parmList.Add(new SqlParameter("@DocKey", objFactory.Doc.DocKey));
                        parmList.Add(new SqlParameter("@DocCodeKey", RepKey));//Request for Quotation RepKey
                        parmList.Add(new SqlParameter("@VendorKey", dr["VendorKey"].ToString()));
                        ReportLoader.ReportSqlParameter = parmList;

                        //Get Report Data With repKey
                        SYSRep rep = SYSRep.Get(RepKey);
                        _recordSource = rep.RPTRecordSource1;

                        //Get report Datasource with store procedure name and parameter from Sys_repCriter               
                        DataTable dtSource = GFunc.ExecuteProc(_recordSource, ReportLoader.ReportSqlParameter);

                        if (SendOnlyNewItems.Checked)
                        {
                            dtSource.DefaultView.RowFilter = "DetRFQSend=false";
                            if (dtSource.DefaultView.Count == 0)
                                continue;
                            else
                                dtSource = dtSource.DefaultView.ToTable();
                        }
                        //report parameters
                        ReportLoader.ReportParameter = GlobalUI.GetReportParameters();
                        ReportParameter pAdditionalNote = new ReportParameter("pAdditionalNote", AdditionalNote.Text);
                        ReportLoader.ReportParameter.Add(pAdditionalNote);

                        ReportParameter pSendOnlyNewItem = new ReportParameter("pSendOnlyNewItem", SendOnlyNewItems.Checked);
                        ReportLoader.ReportParameter.Add(pSendOnlyNewItem);


                        ReportParameter pFaxNumber = new ReportParameter("pFaxNumber", GFunc.NEStr(dr["FaxNumber"], ""));
                        ReportLoader.ReportParameter.Add(pFaxNumber);

                        if (ReportLoader._reportFileName.ToUpper().EndsWith(".RPX"))
                        {
                            ReportLoader.rpxDoc = new DataDynamics.ActiveReports.ActiveReport();
                            //Set report layout with report file name
                            ReportLoader.rpxDoc.LoadLayout(Application.StartupPath + "\\Reports\\" + ReportLoader._reportFileName);//Orignal

                            //MovingControlsInRpx();
                            if (dtSource != null && dtSource.Rows.Count > 0)
                            {
                                ReportLoader.rpxDoc.DataSource = dtSource;
                            }
                            else
                            {
                                throw new TAException(MsgID.Report.NoDataExistForThisReport);
                            }

                            ReportLoader.rpxDoc.AddScriptReference(Application.StartupPath + "\\TAReport.dll");

                            if (GFunc.IsNE(ReportLoader.ReportParameter) == false)
                            {
                                foreach (ReportParameter item in ReportLoader.ReportParameter)
                                {
                                    try
                                    {
                                        ReportLoader.rpxDoc.Parameters[item.ParameterName].PromptUser = false;
                                        ReportLoader.rpxDoc.Parameters[item.ParameterName].Value = item.ParameterValue.ToString();
                                        ReportLoader.rpxDoc.Parameters[item.ParameterName].DefaultValue = item.ParameterValue.ToString();
                                    }
                                    catch (Exception ex)
                                    {
                                        break;
                                    }
                                }
                            }
                            ReportLoader.rpxDoc.Run();
                        }
                        else if (ReportLoader._reportFileName.ToUpper().EndsWith(".RPT"))
                        {
                            ReportLoader.rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                            ReportLoader.rptDoc.Load(Application.StartupPath + @"\Reports\" + ReportLoader._reportFileName);
                            ReportLoader.rptDoc.SetDataSource(dtSource);

                            foreach (ReportParameter p in ReportLoader.ReportParameter)
                            {
                                ReportLoader.rptDoc.SetParameterValue(p.ParameterName, p.ParameterValue);
                            }
                        }

                        //Perform Preview
                        //Preview can only to perform on the first report so only when RecordForPreview is true we can run preview
                        //show preview only for vendor in the first record
                        if (rfqVendorPreview.Checked && RecordForPreview)
                        {
                            ReportLoader.PrintPreview();
                            return; //skip all other process when preview is required
                        }
                        else
                            RecordForPreview = false;

                        //Perform email/Fax/Print
                        switch (transmitMode)
                        {
                            case (int)GEnum.TransmitMode.Email:
                                if (GFunc.isEmail(GFunc.NEStr(dr["EmailAddr"].ToString(), string.Empty)))
                                {
                                    if (ReportLoader.SendEmail(dr["EmailAddr"].ToString(), Subject, Message, ReportFileType.AcrobatPDFFile))
                                        sendSuccessful = true;
                                }
                                else
                                {
                                    throw new TAException("Invalid Email address.");
                                }
                                break;
                            case (int)GEnum.TransmitMode.Fax:
                                if (GFunc.NEStr(dr["FaxNumber"].ToString(), string.Empty) != string.Empty)
                                {
                                    if (ReportLoader.SendFax(dr["FaxNumber"].ToString(), GFunc.NEStr(dr["ItmVendorConID"].ToString(), dr["VendorKey"].ToString())))
                                        sendSuccessful = true;
                                }
                                else
                                {
                                    throw new TAException("Invalid Fax number.");
                                }
                                break;
                            case (int)GEnum.TransmitMode.Print:
                                if (ReportLoader.Print())
                                    sendSuccessful = true;
                                break;
                        }
                        if (sendSuccessful)
                        {
                            dr["TransmitStatus"] = (int)GEnum.TransmitStatus.Sucessful;
                        }

                    }
                    catch (TAException tex)
                    {
                        if (MsgBox.Show(tex.MsgID + "\n Do you want to continue?", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        dr["TransmitStatus"] = (int)GEnum.TransmitStatus.Failed;
                    }
                }
            }
            try
            {
                objDetVendors.AcceptChanges();
                IEnumerable<DataRow> dtVendFilter = objFactory.DocDetVendors.AsEnumerable().Where(r => r.Field<int>("TransmitStatus") == (int)GEnum.TransmitStatus.Sucessful
                    && r.Field<int>("TransmitMode") == transmitMode);

                foreach (DataRow dv_Vend in dtVendFilter)
                {
                    IEnumerable<DataRow> dtItmVendFilter = objFactory.DocDetItmVendors.AsEnumerable().Where(r => r.Field<int?>("VendorKey") == GFunc.NEInt(dv_Vend["VendorKey"], null));
                    foreach (DataRow dr_ItmVend in dtItmVendFilter)
                    {
                        dr_ItmVend["Selected"] = 1;
                        dr_ItmVend.EndEdit();
                    }
                }
                objFactory.DocDetItmVendors.AcceptChanges();
                GridItmVendorBindingSource_Set();

                dtVendFilter = objDetVendors.AsEnumerable().Where(r => r.Field<int>("TransmitMode") == transmitMode);
                if (dtVendFilter.Count() > 0)
                {
                    string xmlString = GFunc.ConvertDataTableToXML(dtVendFilter.CopyToDataTable());
                    parmList.Clear();
                    parmList.Add(new SqlParameter("@xmlDetVendors", xmlString));
                    GFunc.ExecuteNonQueryProc("ARQO_Vendor_TransmitStatus", parmList);
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
            finally
            {
                FilterItmVendorGrid(true);
                objFactory.Doc.IsDirty = false;
            }
        }//Completed
        private void FilterItmVendorGrid(bool rebindcombo)
        {
            try
            {
                if (tagrdDetItms.ActiveRow == null)
                    return;

                int docItmKey = (int)tagrdDetItms.ActiveRow.Cells["DocItmKey"].Value;

                if (HasChildItems(docItmKey))
                {
                    if (rebindcombo)
                        FillChildItemCombo(docItmKey);

                    DataTable dt = (DataTable)DetailSN.DataSource;
                    docItmKey = GFunc.NEInt(DetailSN.Value, GFunc.NEInt(dt.Rows[0]["DocItmKey"], 0));
                    DetailSN.SetValueTrigger(docItmKey, false);
                }

                objFactory.DocDetItmVendors.DefaultView.RowFilter = "DocItmKey=" + docItmKey;
                objFactory.DocDetItmVendors.Columns["DocItmKey"].DefaultValue = docItmKey;
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
        private void FillChildItemCombo(int docItmKey)
        {
            try
            {
                if (HasChildItems(docItmKey))
                {
                    DataTable dt = (from row in objFactory.DocDetItms.AsEnumerable()
                                    where row.Field<int>("LineLinkKey") == docItmKey && row.Field<int>("LineType") > 1000
                                    select new
                                    {
                                        DocItmKey = row.Field<int>("DocItmKey"),
                                        ItmDetSN = row.Field<decimal>("ItmDetSN"),
                                        ItmID = row.Field<string>("ItmID"),
                                        ItmDes = row.Field<string>("ItmDes")
                                    }).AsDataTable();

                    DetailSN.DataSource = dt;
                    if (DetailSN.Rows.Count > 0)
                        DetailSN.SelectedRow = DetailSN.Rows[0];
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
        private Boolean HasChildItems(int docItmKey)
        {
            try
            {
                int count = (from obj in objFactory.DocDetItms.AsEnumerable()
                             where obj.Field<int>("LineLinkKey") == docItmKey && obj.Field<int>("LineType") > 1000
                             select obj).Count();
                if (count > 0)
                {
                    DetailSN.Enabled = true;
                    return true;
                }
                else
                {
                    DetailSN.Enabled = false;
                    return false;
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
        private bool CheckPriceZero()
        {
            /* Prompt warning message for the price 0 of the items on 2023/04/03 */
            bool result = true; GEnum.MsgBoxButton btnSelect;
            DataTable dtPrice = (from row in objFactory.DocDetItms.AsEnumerable()
                                 where (row.Field<decimal?>("ItmQty") > 0 && row.Field<decimal?>("ItmPriceAfter") == 0 
                                 && (GFunc.IsNEZ(row.Field<int?>("ItmBatchKey"))))
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
                btnSelect = MsgBoxGrid.Show("<font color='red'>Are you sure to release the following item(s) with price 0 ?</font>", dtPrice, GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No);
                if (btnSelect == GEnum.MsgBoxButton.Yes)
                    result = true;
                else
                {
                    int rowIndex = tagrdDetItms.Rows.FirstOrDefault(r => Convert.ToDecimal(r.Cells["ItmQty"].Value) > 0 && Convert.ToDecimal(r.Cells["ItmPriceAfter"].Value) == 0).Index;
                    tabDetailList.SelectedTab = tabDetailList.Tabs["tsbItems"];
                    tagrdDetItms.ActiveCell = tagrdDetItms.Rows[rowIndex == -1 ? 0 : rowIndex].Cells["ItmPriceAfter"];
                    tagrdDetItms.PerformAction(UltraGridAction.EnterEditMode);
                    result = false;
                }
            }
            return result;
        }

        private void DisableCreateButtons()
        {
            if (DocConKey.SelectedRow != null)
            {
                bool CashCustomer = GFunc.NEInt(DocConKey.SelectedRow.Cells["CCBType"].Value, 0) == (int)GEnum.CCBType.CH;

                //updated by KKAung on 30 Aug 2021, allow to convert RO/SO for Cash customers

                //tsbCreateRO.Enabled = !CashCustomer;
                //tsbCreateSO.Enabled = !CashCustomer;
                tsbCreateIV.Enabled = !CashCustomer;

                ////12-Oct-2016 [Khin] If the inactive customer, don't allow to convert DO/SO/IV
                //if (GFunc.NEInt(DocConKey.SelectedRow.Cells["Inactive"].Value, 0) == 1)
                //{
                //    tsbCreateDO.Enabled = false;
                //    tsbCreateSO.Enabled = false;
                //    tsbCreateIV.Enabled = false;
                //}
                ////12-Oct-2016 [Khin] End

                //12-Oct-2016 [Khin] If the inactive customer, don't allow to convert DO/SO/IV
                if (GFunc.NEInt(DocConKey.SelectedRow.Cells["ActiveWithProblem"].Value, 0) == 1)
                {
                    tsbCreateRO.Enabled = false;
                    tsbCreateSO.Enabled = false;
                    tsbCreateIV.Enabled = false;
                }
                else if(objFactory.Doc.DocStatus=="Approved" || objFactory.Doc.DocStatus == "")
                {
                    tsbCreateRO.Enabled = true;
                    tsbCreateSO.Enabled = true;
                    tsbCreateIV.Enabled = true;
                }
                //12-Oct-2016 [Khin] End

            }
            if (objFactory.Doc.DocQONum != "" && objFactory.Doc.DocTypeNm == "eStore Quotation")
            {
                EStoreQOSetting(true);
                tsbCreateSO.Enabled = true;
            }
            else
                EStoreQOSetting(false);
        }

        private void EStoreQOSetting(bool normalQO)
        {
           
            sendToEStoreToolStripMenuItem.Enabled = !normalQO;
            updateEStoreToolStripMenuItem.Enabled = normalQO;
            checkQuotationInEStoreToolStripMenuItem.Enabled = normalQO;
            replyLinkToCustomerToolStripMenuItem.Enabled = normalQO;
        }
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
                    if (sender.GetType() == typeof(TAUtil.TAGridEditor))
                    {
                        TAUtil.TAGridEditor grd = sender as TAUtil.TAGridEditor;
                        if (grd.ActiveCell.Column.EditorComponent != null)
                        {
                            grd.PerformAction(UltraGridAction.EnterEditMode);
                            if (grd.ActiveCell.Column.EditorComponent.GetType() == typeof(TAUtil.TAComboBox))
                            {
                                TAUtil.TAComboBox taCombo = (TAUtil.TAComboBox)grd.ActiveCell.Column.EditorComponent;
                                taCombo.Text = grd.ActiveCell.Text;

                                switch (grd.ActiveCell.Column.Key.ToLower())
                                {
                                    case "itmkey":
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
                                    case "vendorkey":
                                        GlobalUI.ItemNotInList(grd.ActiveCell, null, 1);// ItemNotInListAdd
                                        break;

                                    default:
                                        if (((TAUtil.TAComboBox)grd.ActiveCell.Column.EditorComponent).LimitToList)
                                            GlobalUI.ItemNotInList(grd.ActiveCell, null, 0); // ItemNotInList
                                        break;
                                }
                            }
                            // GlobalUI.ItemNotInList(grd.ActiveCell, null, 0);// ItemNotInList
                        }
                    }
                }
                else if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_NUMERIC)
                {
                    throw new TAException(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
                }

                else if (e.ErrorCode == TAUtil.TAErrorCode.INTEGER_EXCEED_LIMIT)
                {
                    throw new TAException(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
                }
                else if (e.ErrorCode == TAUtil.TAErrorCode.FORMULA_NOT_RECOGNIZE)
                {
                    throw new TAException("FORMULA NOT RECOGNIZE");
                }
                else if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_DATE)
                {
                    if (formClose)
                        throw new TAException("Please enter valid date.");
                    else
                        MsgBox.Show("Please enter valid date.");
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

        private void btnHideVendorSect_Click(object sender, EventArgs e)
        {
            if (btnHideVendorSect.Text == "Hide")
            {
                btnHideVendorSect.Text = "Show";
                splitContainer1.Panel2Collapsed = true;
            }
            else                 
            {
                btnHideVendorSect.Text = "Hide";
                splitContainer1.Panel2Collapsed = false;
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
        }        
        
        private bool HeadSalesUpdate()
        {
            //List<SqlParameter> parmList = new List<SqlParameter>();
            //parmList.Add(new SqlParameter("@DocEmKey", GFunc.NEInt(DocEmKey.Value, 0)));            
            //parmList.Add(new SqlParameter("@DocConKey", GFunc.NEInt(DocConKey.Value, 0)));
            //parmList.Add(new SqlParameter("@DocTranGrpKey", GFunc.NEInt(DocTranGrpKey.Value, 0)));
            //parmList.Add(new SqlParameter("@EmInActvie", SqlDbType.Bit));
            //parmList.Add(new SqlParameter("@TranGrpKey", SqlDbType.Int));
            //parmList.Add(new SqlParameter("@SalesRepAsHeadSales", SqlDbType.Bit));
            //parmList.Add(new SqlParameter("@SalesRepTeam", SqlDbType.NVarChar,50));
            //parmList.Add(new SqlParameter("@HeadSalesTeam", SqlDbType.NVarChar, 50));

            //parmList[3].Direction = ParameterDirection.Output;
            //parmList[4].Direction = ParameterDirection.Output;
            //parmList[5].Direction = ParameterDirection.Output;
            //parmList[6].Direction = ParameterDirection.Output;
            //parmList[7].Direction = ParameterDirection.Output;
            

            //GFunc.ExecuteNonQueryProc("Doc_SalesRepGetCheckData", parmList);

            //if (GFunc.NEBool(parmList[3].Value, false))
            //{                             
            //    return false;
            //}
            //if (GFunc.IsNEZ(DocTranGrpKey.Value) || GFunc.NEBool(parmList[5].Value, false))
            //{
            //    DocTranGrpKey.SetValueTrigger(GFunc.NEInt(parmList[4].Value, 0), false);
            //}
            //Custom4.SetValueTrigger(GFunc.NEStr(parmList[6].Value, ""),false);
            //Custom5.SetValueTrigger(GFunc.NEStr(parmList[7].Value, ""),false);
            return true;
        }

        private void cboDelivery_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
               DocRemDelivery.SetValueTrigger((!GFunc.IsNE(DocRemDelivery.Text) ? DocRemDelivery.Text + "\n\r" : "") + cboDelivery.Text, true); 
            else
                DocRemDelivery.SetValueTrigger(cboDelivery.Text, true);
        }

        private void cboPrice_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                DocRemPrice.SetValueTrigger((!GFunc.IsNE(DocRemPrice.Text) ? DocRemPrice.Text + "\n\r" : "") + cboPrice.Text, true);
            else
                DocRemPrice.SetValueTrigger(cboPrice.Text, true);
        }

        private void cboValidity_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                DocRemValidity.SetValueTrigger((!GFunc.IsNE(DocRemValidity.Text) ? DocRemValidity.Text + "\n\r" : "") + cboValidity.Text, true);
            else
                DocRemValidity.SetValueTrigger(cboValidity.Text, true);
        }

        private void cboPayment_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                DocRemPayment.SetValueTrigger((!GFunc.IsNE(DocRemPayment.Text)? DocRemPayment.Text + "\n\r":"") + cboPayment.Text, true);
            else
                DocRemPayment.SetValueTrigger(cboPayment.Text, true);
        }

        private void sendToEStoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
             try
            {
                if (SaveChanges(true, true, true, GEnum.DocAction.Undetermine) == false)
                {
                    return; //if User clicked "I don't know" or ("Save Changes" which validation is failed, the form will still be opened with dirty state)                   
                }

                if (objFactory.Doc.DocState != 100)
                {
                    MsgBox.Show("Quotation must be posted before sending to estore.");
                    return;
                }

                if (!GFunc.IsNEZ(objFactory.Doc.DocQONum))
                {
                    MsgBox.Show("Quotation already exists in estore. Please click Update EStore button instead.");
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                
                int id = 0;
                string sku = "";
                string itmDes = "";
                int qty = 0;
                string UOM = "";
                decimal price = 0M;
                decimal amount = 0M;
                string itmremark = "";        
                string skus = "";
                string data = "";
                string ids = "";
                string rem = "";
                decimal bankchg = 0M;
                decimal deliverychg = 0M;

                string salesemail = GFunc.NEStr(MSTSalesRep.Get(objFactory.Doc.DocEmKey).Custom1,"");
                objFactory.Doc.DocTypeNm = "eStore Quotation";

                int MagentoVersion = SysOptionUtility.GetInt("MagentoVersion");
         
                if (MagentoVersion == 1)
                {
                    #region +++ magento 1++

                    string conStr = "userid=bhestore_may;password=Thinzar@12;server=101.100.209.196;database=bhestore_magento18jul;connection timeout=180";
                    MySqlConnection con = new MySqlConnection(conStr);
                    //  MySqlConnection con = new MySqlConnection("userid=ywpvyvgedh;password=4MAcPftrEU;server=172.104.41.102;database=ywpvyvgedh;connection timeout=180");
                    //MySqlConnection con = new MySqlConnection("userid=root;password=;server=localhost;database=bhestore_magento18jul;connection timeout=180");

                    foreach (UltraGridRow row in tagrdDetItms.Rows)
                    {
                        if (row.Cells["ItmID"].Text.ToUpper().Equals("BANK CHARGES"))
                        {
                            bankchg = GFunc.NEDec(row.Cells["ItmAmtF"].Value, 0);
                        }                    
                        else if (GFunc.NEInt(row.Cells["ItmType"].Value, 0) < 700)
                        {
                            MySqlCommand cmd = new MySqlCommand("SELECT entity_id FROM catalog_product_entity where sku='" + row.Cells["ItmID"].Text + "';", con);

                            con.Open();

                            MySqlDataReader reader = cmd.ExecuteReader();

                            if (reader.Read())
                            {
                                ids += reader.GetInt32("entity_id") + ",";
                            }
                            else if (row.Cells["ItmID"].Text.ToUpper().Equals("BANK CHARGES") == false)
                            {
                                ids += "99999,";
                            }

                            skus += row.Cells["ItmID"].Text.Replace(",", "").Replace(",", "") + ",";
                            data += row.Cells["ItmQty"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["ItmUOMKey"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["ItmPriceUser"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["ItmAmtShw"].Text.Replace(",", "").Replace(",", "") +
                              "," + row.Cells["Custom1"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["Custom2"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["ItmDes"].Text.Replace(",", "").Replace("\n\r", "</br>") + "," + row.Cells["ItmMark"].Text.Replace(",", "") + "," + row.Cells["ItmID"].Text.Replace(",", "") + "#%#";
                            con.Close();

                        }
                        else
                            rem = rem + row.Cells["ItmDes"].Text.Replace("\n\r", "</br>") + "</br>";
                    }
                    if (ids.Length > 0)
                    {
                        skus = skus.Remove(skus.Length - 1);
                        data = data.Remove(data.Length - 3);
                        ids = ids.Remove(ids.Length - 1);
                    }
                    else
                    {
                        MsgBox.Show("No data to reply.");
                        return;
                    }

                    rem += GFunc.NEStr(objFactory.Doc.DocRem, "").Replace("\n\r", "</br>")
                              + (objFactory.Doc.DocRem != "" && objFactory.Doc.DocRemDelivery != "" ? "</br>" : "")
                              + GFunc.NEStr(objFactory.Doc.DocRemDelivery, "").Replace("\n\r", "</br>")
                             + (objFactory.Doc.DocRemDelivery != "" && objFactory.Doc.DocRemPrice != "" ? "</br>" : "") + GFunc.NEStr(objFactory.Doc.DocRemPrice, "").Replace("\n\r", "</br>")
                             + (objFactory.Doc.DocRemPrice != "" && objFactory.Doc.DocRemValidity != "" ? "</br>" : "") + GFunc.NEStr(objFactory.Doc.DocRemValidity, "").Replace("\n\r", "</br>")
                             + (objFactory.Doc.DocRemValidity != "" && objFactory.Doc.DocRemPayment != "" ? "</br>" : "") + GFunc.NEStr(objFactory.Doc.DocRemPayment, "").Replace("\n\r", "</br>");

                    string sql = "insert into `netgo_boss_quote` " +
                       " (`name`, `company`, `email`, `contact_no`, `designation`, `comment`, " +
            "`customer_id`, `rfq_id`, `quote_id`, `quote_date`, `sales_id`, `vessel_marking`, `payment_term`, " +
            "`doc_remark`, `curr_id`, `sub_total`, `gst_percent`, `gst_amount`, `grand_total`, `product_id`, `skus`, `additional_data`," +
             "`updated_at`, `status`,  `created_at`, `country`, `curr_rate`, `salesrep_email`) " +
            "VALUES ('" + objFactory.Doc.DocBAddrAttn + "'," +
                        "'" + objFactory.Doc.DocConNm + "'," +
                         "'" + objFactory.Doc.DocBAddrEmail + "'," +
                          "'" + objFactory.Doc.DocBAddrTel1 + "'," +
                           "'" + objFactory.Doc.DocRemAdditional2 + "'," +//designation
                              "'" + objFactory.Doc.DocRemAdditional1 + "',0,0," + //comment,customer_id,rfq_id
                               "'" + objFactory.Doc.DocID + "'," +//quote_id
                                "'" + objFactory.Doc.DocDate + "'," +//quote_date
                                 "'" + DocEmKey.Text.Replace(",", "") + "'," +//sales_id
                                    "" + bankchg + "," +//vessel_marking
                                 "'" + DocTermKey.Text.Replace(",", "") + "'," +//payment_term
                                  "'" + rem.Replace(",", " ").Replace("'", "''") + "'," +//doc_remark                             
                                  "'" + DocCurrKey.Text.Replace(",", "") + "'," +//curr_id
                                   "'" + (objFactory.Doc.DocSubTotal - bankchg) + "'," +//sub_total
                                    "'" + objFactory.Doc.DocTaxGrpRate + "'," +//gst_percent
                                     "'" + objFactory.Doc.DocTaxTotal + "'," +//gst_amount
                        //    "'" + objFactory.Doc.Custom2 + "'," +//reference_no
                                      "'" + objFactory.Doc.DocGrand + "'," +//grand_total
                                       "'" + ids + "'," +//product_id
                                        "'" + skus + "'," +//skus
                                          "'" + data + "'," +//additional_data
                                           "'" + DateTime.Now + "',0,'" + DateTime.Now + "'," +//updated_at,status,created_at
                                             "'" + DocGrpKey.Text + "'," +//country
                                       "'" + objFactory.Doc.DocCurrRate + "'," +//curr_rate
                                        "'" + salesemail + "')";//salesrep_email                                 


                    MySqlCommand cmd1 = new MySqlCommand(sql, con);
                    cmd1.CommandType = CommandType.Text;
                    con.Open();
                    cmd1.ExecuteNonQuery();
                    con.Close();

                    int newInsertedID = 0;
                    MySqlCommand cmd2 = new MySqlCommand("SELECT entity_id FROM netgo_boss_quote where quote_id='" + objFactory.Doc.DocID + "';", con);

                    con.Open();

                    MySqlDataReader reader1 = cmd2.ExecuteReader();

                    if (reader1.Read())
                    {
                        newInsertedID += reader1.GetInt32("entity_id");
                    }

                    con.Close();
                    sql = "INSERT INTO `netgo_boss_quote_store` values (" + newInsertedID + ",1" + ")";

                    MySqlCommand cmd4 = new MySqlCommand(sql, con);
                    cmd4.CommandType = CommandType.Text;
                    con.Open();
                    cmd4.ExecuteNonQuery();
                    con.Close();


                    objFactory.Doc.DocQONum = newInsertedID.ToString();
                    string refno = Encrypt(newInsertedID.ToString());
                    objFactory.Doc.Custom2 = refno;

                    MySqlCommand cmd3 = new MySqlCommand("Update netgo_boss_quote set reference_no='" + refno + "' where quotation_id=" + newInsertedID, con);
                    cmd3.CommandType = CommandType.Text;
                    con.Open();
                    cmd3.ExecuteNonQuery();
                    con.Close();
                    #endregion +++ magento 1+++
                }

                else if (MagentoVersion == 2)
                {
                    #region +++ magento  2+++

                    if (((DataTable)tagrdDetItms.DataSource).Select("[ItmType]<700").Count() == 0)
                    {
                        MsgBox.Show("No data send to Estore!");
                        return;
                    }

                    //string conStr = "userid=ywpvyvgedh;password=4MAcPftrEU;server=172.104.41.102;database=ywpvyvgedh;connection timeout=180";
                    string conStr = "userid=afjmsnfvpe;password=nJUXgzG6PQ;server=172.104.41.102;database=afjmsnfvpe;connection timeout=180";
                    MySqlConnection con = new MySqlConnection(conStr);

                    string sql = "insert into `estore_boss_quotation` " +
                         " (`name`, `company`, `email`, `contact_no`, `designation`, `comment`, " +
              "`customer_id`, `quote_id`, `quotation_no`, `date`, `sales_person`,  " +
              " `currency`, `sub_total`, `gst_percent`, `gst`, `grand_total`," +
               "`updated_at`, `status`, `sent`, `created_at`, `country`, `currency_rate`, `sales_person_email`) " +
              "VALUES ('" + objFactory.Doc.DocBAddrAttn.Replace("'", "''") + "'," +
                          "'" + objFactory.Doc.DocConNm + "'," +
                           "'" + objFactory.Doc.DocBAddrEmail + "'," +
                            "'" + objFactory.Doc.DocBAddrTel1.Replace("'", "''") + "'," +
                             "'" + objFactory.Doc.DocRemAdditional2.Replace("'", "''") + "'," +//designation
                                "'" + objFactory.Doc.DocRemAdditional1.Replace("'", "''") + "',0,0," + //comment,customer_id,quote_id
                                 "'" + objFactory.Doc.DocID + "'," +//quotation_no
                                  "'" + objFactory.Doc.DocDate.Value.ToString("yyyy-MM-dd") + "'," +//quote_date
                                   "'" + DocEmKey.Text.Replace(",", "") + "'," +//sales_id 
                                    "'" + DocCurrKey.Text.Replace(",", "") + "'," +//curr_id
                                     "'" + (objFactory.Doc.DocSubTotal - bankchg - deliverychg) + "'," +//sub_total
                                      "'" + objFactory.Doc.DocTaxGrpRate + "'," +//gst_percent
                                       "'" + objFactory.Doc.DocTaxTotal + "'," +//gst
                                        //"'" + objFactory.Doc.Custom2 + "'," +//reference_no
                                        "'" + objFactory.Doc.DocGrand + "'," +//grand_total                                          
                                             "'" + DateTime.Now.ToString("yyyy-MM-dd h:mm:ss") + "','updated','1','" + DateTime.Now.ToString("yyyy-MM-dd h:mm:ss") + "'," +//updated_at,status,sent,created_at
                                               "'" + DocGrpKey.Text + "'," +//country
                                         "'" + objFactory.Doc.DocCurrRate + "'," +//curr_rate
                                          "'" + salesemail + "')";//salesrep_email                                 


                    MySqlCommand cmd1 = new MySqlCommand(sql, con);
                    cmd1.CommandType = CommandType.Text;
                    con.Open();
                    cmd1.ExecuteNonQuery();
                    con.Close();

                    int newInsertedID = 0;
                    MySqlCommand cmd2 = new MySqlCommand("SELECT quotation_id FROM estore_boss_quotation where quotation_no='" + objFactory.Doc.DocID + "';", con);

                    con.Open();

                    MySqlDataReader reader1 = cmd2.ExecuteReader();

                    if (reader1.Read())
                    {
                        newInsertedID = reader1.GetInt32("quotation_id");
                    }

                    con.Close();


                    objFactory.Doc.DocQONum = newInsertedID.ToString();
                    string refno = Encrypt(newInsertedID.ToString());
                    objFactory.Doc.Custom2 = refno;

                    //To add code base on table structures for magento 2 
                    foreach (UltraGridRow row in tagrdDetItms.Rows)
                    {
                        if (row.Cells["ItmID"].Text.ToUpper().Equals("BANK CHARGES"))
                        {
                            bankchg = GFunc.NEDec(row.Cells["ItmAmtF"].Value, 0);
                        }
                        // add code for delivery charges 
                        else if (row.Cells["ItmID"].Text.ToUpper().Equals("FREIGHT CHARGES") || row.Cells["ItmID"].Text.ToUpper().Equals("F FREIGHT"))
                        {
                            deliverychg += GFunc.NEDec(row.Cells["ItmAmtF"].Value, 0);
                        }
                        else if (GFunc.NEInt(row.Cells["ItmType"].Value, 0) < 700)
                        {
                            MySqlCommand cmd = new MySqlCommand("SELECT entity_id FROM catalog_product_entity where sku='" + row.Cells["ItmID"].Text + "';", con);

                            con.Open();

                            MySqlDataReader reader = cmd.ExecuteReader();

                            if (reader.Read())
                            {
                                id = reader.GetInt32("entity_id");
                            }
                            else if (row.Cells["ItmID"].Text.ToUpper().Equals("BANK CHARGES") == false)
                            {
                                id = 99999;
                            }
                            con.Close();

                            row.Cells["ItmIgrpdItm"].Value = id;
                            row.Update();

                            sku = row.Cells["ItmID"].Text.Replace(",", "");
                            //data += row.Cells["Custom1"].Text.Replace(",", "");
                            qty = GFunc.NEInt(row.Cells["ItmQty"].Text.Replace(",", ""), 0);
                            UOM = row.Cells["ItmUOMKey"].Text.Replace(",", "");
                            price = GFunc.NEDec(row.Cells["ItmPriceUser"].Text.Replace(",", ""), 0);
                            amount = GFunc.NEDec(row.Cells["ItmAmtShw"].Text.Replace(",", ""), 0);
                            itmDes = row.Cells["ItmDes"].Text.Replace(",", "").Replace("'", "''").Replace("\n\r", "</br>");
                            itmremark = row.Cells["Custom2"].Text.Replace(",", "").Replace("'", "''");

                            string sql1 = "insert into `estore_boss_quotation_items` " +
                         " (quotation_id,	product_id,	remark,	sku,	created_at,	updated_at,	product_name,	qty,	uom,	price,	amount) " +
                        "VALUES (" + newInsertedID + "," +
                          "" + id + "," +
                           "'" + itmremark + "'," +
                            "'" + sku + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd h:mm:ss") + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd h:mm:ss") + "'," +
                                 "'" + itmDes + "'," +
                                  "'" + qty + "'," +
                                   "'" + UOM + "'," +
                                    "'" + price + "'," +
                                     "'" + amount + "')";

                            MySqlCommand cmd5 = new MySqlCommand(sql1, con);
                            cmd5.CommandType = CommandType.Text;
                            con.Open();
                            cmd5.ExecuteNonQuery();
                            con.Close();

                        }
                        else
                            rem = rem + row.Cells["ItmDes"].Text.Replace("'", "''").Replace("\n\r", "</br>") + "</br>";
                    }

                    rem += GFunc.NEStr(objFactory.Doc.DocRem, "").Replace("'", "''").Replace("\n\r", "</br>")
                            + (objFactory.Doc.DocRem != "" && objFactory.Doc.DocRemDelivery != "" ? "</br>" : "")
                            + GFunc.NEStr(objFactory.Doc.DocRemDelivery, "").Replace("'", "''").Replace("\n\r", "</br>")
                           + (objFactory.Doc.DocRemDelivery != "" && objFactory.Doc.DocRemPrice != "" ? "</br>" : "") + GFunc.NEStr(objFactory.Doc.DocRemPrice, "").Replace("'", "''").Replace("\n\r", "</br>")
                           + (objFactory.Doc.DocRemPrice != "" && objFactory.Doc.DocRemValidity != "" ? "</br>" : "") + GFunc.NEStr(objFactory.Doc.DocRemValidity, "").Replace("'", "''").Replace("\n\r", "</br>")
                           + (objFactory.Doc.DocRemValidity != "" && objFactory.Doc.DocRemPayment != "" ? "</br>" : "") + GFunc.NEStr(objFactory.Doc.DocRemPayment, "").Replace("'", "''").Replace("\n\r", "</br>");

                    MySqlCommand cmd6 = new MySqlCommand("Update estore_boss_quotation set reference_id='" + refno + "', bank_charges='" + bankchg + "', delivery_charges='" + deliverychg +
                                        "' , sub_total ='" + (objFactory.Doc.DocSubTotal - bankchg - deliverychg) + "', doc_remark='" + rem + "' where quotation_id=" + newInsertedID, con);
                    cmd6.CommandType = CommandType.Text;
                    con.Open();
                    cmd6.ExecuteNonQuery();
                    con.Close();
                    #endregion +++ magento  2+++
                }

                objFactory.Save((int)GEnum.DocAction.Post);
                EStoreQOSetting(true);

                MsgBox.Show("Quotation sent successfullly to EStore.");
            }
             catch (Exception ex)
             {
                 MsgBox.Show(ex.Message);
             }
             finally
             {
                 this.Cursor = Cursors.Default;
             }
        }


        private void updateEStoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
             try
            {
                if (SaveChanges(true, true, true, GEnum.DocAction.Undetermine) == false)
                {
                    return; //if User clicked "I don't know" or ("Save Changes" which validation is failed, the form will still be opened with dirty state)                   
                }
                if (objFactory.Doc.DocQuoteStatus == 20)
                {
                    MsgBox.Show("Customer has already confirmed the quotation. It cannot be updated.");
                    return;
                }
                this.Cursor = Cursors.WaitCursor;

                if (objFactory.Doc.DocQONum != "")
                {
                    bool proceed = true;
                    int quoteID = 0;

                    if (objFactory.Doc.Custom3 == "Replied")
                    {
                        if (MsgBox.Show("The quotation has been replied before.\nAre you sure to update it?", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                        {
                            proceed = false;
                        }
                    }
                    if (proceed)
                    {
                        int id = 0;
                        string sku = "";
                        string itmDes = "";
                        int qty = 0;
                        string UOM = "";
                        decimal price = 0M;
                        decimal amount = 0M;
                        string itmremark = "";

                        string skus = "";
                        string data = "";
                        string ids = "";
                        string rem = "";
                        decimal bankchg = 0M;
                        decimal deliverychg = 0M;

                        string salesemail = GFunc.NEStr(MSTSalesRep.Get(objFactory.Doc.DocEmKey).Custom1,"");
                        int MagentoVersion = SysOptionUtility.GetInt("MagentoVersion");

                        if (MagentoVersion == 1)
                        {
                            #region +++ magento 1++
                            MySqlConnection con = new MySqlConnection("userid=bhestore_may;password=Thinzar@12;server=101.100.209.196;database=bhestore_magento18jul;connection timeout=180");
                            //MySqlConnection con = new MySqlConnection("userid=ywpvyvgedh;password=4MAcPftrEU;server=172.104.41.102;database=ywpvyvgedh;connection timeout=180");
                            //MySqlConnection con = new MySqlConnection("userid=root;password=;server=localhost;database=bhestore_magento18jul;connection timeout=180");

                            foreach (UltraGridRow row in tagrdDetItms.Rows)
                            {
                                if (row.Cells["ItmID"].Text.ToUpper().Equals("BANK CHARGES"))
                                {
                                    bankchg = GFunc.NEDec(row.Cells["ItmAmtF"].Value, 0);
                                }
                                else if (row.Cells["ItmIGrpDItm"].Text.Replace(",", "") != "0" && row.Cells["ItmIGrpDItm"].Text.Replace(",", "") != "")
                                {
                                    ids += row.Cells["ItmIGrpDItm"].Text.Replace(",", "") + ",";
                                    skus += row.Cells["ItmIGrpDItm"].Text.Replace(",", "") + ",";
                                    data += row.Cells["ItmQty"].Text.Replace(",", "") + "," + row.Cells["ItmUOMKey"].Text.Replace(",", "") + "," + row.Cells["ItmPriceUser"].Text.Replace(",", "") + "," + row.Cells["ItmAmtShw"].Text.Replace(",", "") +
                                      "," + row.Cells["Custom1"].Text.Replace(",", "") + "," + row.Cells["Custom2"].Text.Replace(",", "") + "," + row.Cells["ItmDes"].Text.Replace(",", "").Replace("\n\r", "</br>") + "," + row.Cells["ItmMark"].Text.Replace(",", "") + "," + row.Cells["ItmID"].Text.Replace(",", "") + "#%#";
                                }
                                else if (GFunc.NEInt(row.Cells["ItmType"].Value, 0) < 700)
                                {
                                    MySqlCommand cmd = new MySqlCommand("SELECT entity_id FROM bhestore_magento18jul.catalog_product_entity where sku='" + row.Cells["ItmID"].Text + "';", con);

                                    con.Open();

                                    MySqlDataReader reader = cmd.ExecuteReader();

                                    if (reader.Read())
                                    {
                                        ids += reader.GetInt32("entity_id") + ",";
                                    }
                                    else if (row.Cells["ItmID"].Text.ToUpper().Equals("BANK CHARGES") == false)
                                    {
                                        ids += "99999,";
                                    }

                                    skus += row.Cells["ItmID"].Text.Replace(",", "").Replace(",", "") + ",";
                                    data += row.Cells["ItmQty"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["ItmUOMKey"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["ItmPriceUser"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["ItmAmtShw"].Text.Replace(",", "").Replace(",", "") +
                                      "," + row.Cells["Custom1"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["Custom2"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["ItmDes"].Text.Replace(",", "").Replace("\n\r", "</br>") + "," + row.Cells["ItmMark"].Text.Replace(",", "") + "," + row.Cells["ItmID"].Text.Replace(",", "") + "#%#";
                                    con.Close();

                                }
                                else
                                    rem = rem + row.Cells["ItmDes"].Text.Replace("\n\r", "</br>") + "</br>";
                            }
                            if (ids.Length > 0)
                            {
                                skus = skus.Remove(skus.Length - 1);
                                data = data.Remove(data.Length - 3);
                                ids = ids.Remove(ids.Length - 1);
                            }
                            else
                            {
                                MsgBox.Show("No data to reply.");
                                return;
                            }

                            rem += GFunc.NEStr(objFactory.Doc.DocRem, "").Replace("\n\r", "</br>")
                                      + (objFactory.Doc.DocRem != "" && objFactory.Doc.DocRemDelivery != "" ? "</br>" : "")
                                      + GFunc.NEStr(objFactory.Doc.DocRemDelivery, "").Replace("\n\r", "</br>")
                                     + (objFactory.Doc.DocRemDelivery != "" && objFactory.Doc.DocRemPrice != "" ? "</br>" : "") + GFunc.NEStr(objFactory.Doc.DocRemPrice, "").Replace("\n\r", "</br>")
                                     + (objFactory.Doc.DocRemPrice != "" && objFactory.Doc.DocRemValidity != "" ? "</br>" : "") + GFunc.NEStr(objFactory.Doc.DocRemValidity, "").Replace("\n\r", "</br>")
                                     + (objFactory.Doc.DocRemValidity != "" && objFactory.Doc.DocRemPayment != "" ? "</br>" : "") + GFunc.NEStr(objFactory.Doc.DocRemPayment, "").Replace("\n\r", "</br>");

                            string sql = "update `bhestore_magento18jul`.`netgo_boss_quote` set `name`='" + objFactory.Doc.DocBAddrAttn + "'," +
                                  "`quote_id`='" + objFactory.Doc.DocID + "'," +
                                "`name`='" + objFactory.Doc.DocBAddrAttn + "'," +
                                "`company`='" + objFactory.Doc.DocConNm + "'," +
                                 "`email`='" + objFactory.Doc.DocBAddrEmail + "'," +
                                  "`contact_no`='" + objFactory.Doc.DocBAddrTel1 + "'," +
                                   "`designation`='" + objFactory.Doc.DocRemAdditional2 + "'," +
                                      "`comment`='" + objFactory.Doc.DocRemAdditional1 + "'," +
                                       "`quote_id`='" + objFactory.Doc.DocID + "'," +
                                        "`quote_date`='" + objFactory.Doc.DocDate + "'," +
                                         "`sales_id`='" + DocEmKey.Text.Replace(",", "") + "'," +
                                         "`payment_term`='" + DocTermKey.Text.Replace(",", "") + "'," +
                                          "`doc_remark`='" + rem.Replace(",", " ").Replace("'", "''") + "'," +
                                            "`vessel_marking`=" + bankchg + "," +
                                          "`curr_id`='" + DocCurrKey.Text.Replace(",", "") + "'," +
                                           "`sub_total`='" + (objFactory.Doc.DocSubTotal - bankchg) + "'," +
                                            "`gst_percent`='" + objFactory.Doc.DocTaxGrpRate + "'," +
                                             "`gst_amount`='" + objFactory.Doc.DocTaxTotal + "'," +
                                              "`reference_no`='" + objFactory.Doc.Custom2 + "'," +
                                              "`grand_total`='" + objFactory.Doc.DocGrand + "'," +
                                               "`product_id`='" + ids + "'," +
                                               "`curr_rate`='" + objFactory.Doc.DocCurrRate + "'," +
                                               "`skus`='" + skus + "'," +
                                                "`salesrep_email`='" + salesemail + "'," +
                                                "`additional_data`='" + data + "' where entity_id=" + objFactory.Doc.DocQONum;


                            MySqlCommand cmd1 = new MySqlCommand(sql, con);
                            cmd1.CommandType = CommandType.Text;
                            con.Open();
                            cmd1.ExecuteNonQuery();
                            con.Close();
                            #endregion +++ magento 1+++
                        }

                        else if (MagentoVersion == 2)
                        {
                            #region +++ magento 2++

                            //string conStr = "userid=ywpvyvgedh;password=4MAcPftrEU;server=172.104.41.102;database=ywpvyvgedh;connection timeout=180";
                            string conStr = "userid=afjmsnfvpe;password=nJUXgzG6PQ;server=172.104.41.102;database=afjmsnfvpe;connection timeout=180";
                            MySqlConnection con = new MySqlConnection(conStr);

                            string sql = "update `estore_boss_quotation` set `name`='" + objFactory.Doc.DocBAddrAttn.Replace("'", "''") + "'," +
                            //  "`reference_id`='" + objFactory.Doc.Custom2 + "'," +
                            "`company`='" + objFactory.Doc.DocConNm + "'," +
                             "`email`='" + objFactory.Doc.DocBAddrEmail + "'," +
                              "`contact_no`='" + objFactory.Doc.DocBAddrTel1.Replace("'", "''") + "'," +
                               "`designation`='" + objFactory.Doc.DocRemAdditional2.Replace("'", "''") + "'," +
                                  "`comment`='" + objFactory.Doc.DocRemAdditional1.Replace("'", "''") + "'," +
                                   "`quotation_no`='" + objFactory.Doc.DocID + "'," +
                                    "`date`='" + objFactory.Doc.DocDate.Value.ToString("yyyy-MM-dd") + "'," +
                                     "`sales_person`='" + DocEmKey.Text.Replace(",", "").Replace("'", "''") + "'," +
                                      "`currency`='" + DocCurrKey.Text.Replace(",", "") + "'," +
                                       "`sub_total`='" + (objFactory.Doc.DocSubTotal - bankchg - deliverychg) + "'," +
                                       "`gst_percent`='" + objFactory.Doc.DocTaxGrpRate + "'," +
                                         "`gst`='" + objFactory.Doc.DocTaxTotal + "'," +
                                          "`grand_total`='" + objFactory.Doc.DocGrand + "'," +
                                          "`status`='updated'," +
                                          "`updated_at`='" + DateTime.Now.ToString("yyyy-MM-dd h:mm:ss") + "'," +
                                            "`country`='" + DocGrpKey.Text + "'," +
                                           "`currency_rate`='" + objFactory.Doc.DocCurrRate + "'," +
                                            "`sales_person_email`='" + salesemail + "' where quotation_id=" + objFactory.Doc.DocQONum;

                            MySqlCommand cmd1 = new MySqlCommand(sql, con);
                            cmd1.CommandType = CommandType.Text;
                            con.Open();
                            cmd1.ExecuteNonQuery();
                            con.Close();

                            int quotation_id = GFunc.NEInt(objFactory.Doc.DocQONum, 0);
                            
                            string sql1 = "delete from `estore_boss_quotation_items` where quotation_id =" + quotation_id;

                            MySqlCommand cmd5 = new MySqlCommand(sql1, con);
                            cmd5.CommandType = CommandType.Text;
                            con.Open();
                            cmd5.ExecuteNonQuery();
                            con.Close();

                            //To add code base on table structures for magento 2 
                            foreach (UltraGridRow row in tagrdDetItms.Rows)
                            {
                                if (row.Cells["ItmID"].Text.ToUpper().Equals("BANK CHARGES"))
                                {
                                    bankchg = GFunc.NEDec(row.Cells["ItmAmtF"].Value, 0);

                                }
                                // add code for delivery charges 
                                else if (row.Cells["ItmID"].Text.ToUpper().Equals("FREIGHT CHARGES") || row.Cells["ItmID"].Text.ToUpper().Equals("F FREIGHT"))
                                {
                                    deliverychg += GFunc.NEDec(row.Cells["ItmAmtF"].Value, 0);
                                }
                                else if (row.Cells["ItmIGrpDItm"].Text.Replace(",", "") != "0" && row.Cells["ItmIGrpDItm"].Text.Replace(",", "") != "")
                                {
                                    id = GFunc.NEInt(row.Cells["ItmIGrpDItm"].Value, 0);
                                    sku = row.Cells["ItmID"].Text.Replace(",", "");
                                    qty = GFunc.NEInt(row.Cells["ItmQty"].Text.Replace(",", ""), 0);
                                    UOM = row.Cells["ItmUOMKey"].Text.Replace(",", "");
                                    price = GFunc.NEDec(row.Cells["ItmPriceUser"].Text.Replace(",", ""), 0);
                                    amount = GFunc.NEDec(row.Cells["ItmAmtShw"].Text.Replace(",", ""), 0);
                                    itmDes = row.Cells["ItmDes"].Text.Replace(",", "").Replace("'", "''").Replace("\n\r", "</br>");
                                    itmremark = row.Cells["Custom2"].Text.Replace(",", "").Replace("'", "''");

                                    sql1 = "insert into `estore_boss_quotation_items` " +
                                    " (quotation_id,	product_id,	remark,	sku,	created_at,	updated_at, product_name,	qty,	uom,	price,	amount) " +
                                    "VALUES (" + quotation_id + "," +
                                    "" + id + "," +
                                    "'" + itmremark + "'," +
                                        "'" + sku + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd h:mm:ss") + "'," +
                                            "'" + DateTime.Now.ToString("yyyy-MM-dd h:mm:ss") + "'," +
                                            "'" + itmDes + "'," +
                                            "'" + qty + "'," +
                                            "'" + UOM + "'," +
                                                "'" + price + "'," +
                                                "'" + amount + "')";

                                    MySqlCommand cmd3 = new MySqlCommand(sql1, con);
                                    cmd3.CommandType = CommandType.Text;
                                    con.Open();
                                    cmd3.ExecuteNonQuery();
                                    con.Close();
                                }
                                else if (GFunc.NEInt(row.Cells["ItmType"].Value, 0) < 700)
                                {
                                    MySqlCommand cmd = new MySqlCommand("SELECT entity_id FROM catalog_product_entity where sku='" + row.Cells["ItmID"].Text + "';", con);

                                    con.Open();

                                    MySqlDataReader reader = cmd.ExecuteReader();

                                    if (reader.Read())
                                    {
                                        id = reader.GetInt32("entity_id");
                                    }                                   
                                    else if (row.Cells["ItmID"].Text.ToUpper().Equals("BANK CHARGES") == false)
                                    {
                                        id = 99999;
                                    }
                                    con.Close();

                                    row.Cells["ItmIgrpdItm"].Value = id;
                                    row.Update();

                                    sku = row.Cells["ItmID"].Text.Replace(",", "");
                                    //data += row.Cells["Custom1"].Text.Replace(",", "");
                                    qty = GFunc.NEInt(row.Cells["ItmQty"].Text.Replace(",", ""), 0);
                                    UOM = row.Cells["ItmUOMKey"].Text.Replace(",", "");
                                    price = GFunc.NEDec(row.Cells["ItmPriceUser"].Text.Replace(",", ""), 0);
                                    amount = GFunc.NEDec(row.Cells["ItmAmtShw"].Text.Replace(",", ""), 0);
                                    itmDes = row.Cells["ItmDes"].Text.Replace(",", "").Replace("'", "''").Replace("\n\r", "</br>");
                                    itmremark = row.Cells["Custom2"].Text.Replace(",", "").Replace("'", "''");

                                    sql1 = "insert into `estore_boss_quotation_items` " +
                                    " (quotation_id,	product_id,	remark,	sku,	created_at,	updated_at, product_name,	qty,	uom,	price,	amount) " +
                                    "VALUES (" + quotation_id + "," +
                                    "" + id + "," +
                                    "'" + itmremark + "'," +
                                     "'" + sku + "'," +
                                      "'" + DateTime.Now.ToString("yyyy-MM-dd h:mm:ss") + "'," +
                                         "'" + DateTime.Now.ToString("yyyy-MM-dd h:mm:ss") + "'," +
                                          "'" + itmDes + "'," +
                                           "'" + qty + "'," +
                                            "'" + UOM + "'," +
                                             "'" + price + "'," +
                                              "'" + amount + "')";

                                    MySqlCommand cmd6 = new MySqlCommand(sql1, con);
                                    cmd6.CommandType = CommandType.Text;
                                    con.Open();
                                    cmd6.ExecuteNonQuery();
                                    con.Close();

                                }
                                else
                                    rem = rem + row.Cells["ItmDes"].Text.Replace("'", "''").Replace("\n\r", "</br>") + "</br>";
                            }

                            rem += GFunc.NEStr(objFactory.Doc.DocRem, "").Replace("'", "''").Replace("\n\r", "</br>")
                                    + (objFactory.Doc.DocRem != "" && objFactory.Doc.DocRemDelivery != "" ? "</br>" : "")
                                    + GFunc.NEStr(objFactory.Doc.DocRemDelivery, "").Replace("'", "''").Replace("\n\r", "</br>")
                                   + (objFactory.Doc.DocRemDelivery != "" && objFactory.Doc.DocRemPrice != "" ? "</br>" : "") + GFunc.NEStr(objFactory.Doc.DocRemPrice, "").Replace("'", "''").Replace("\n\r", "</br>")
                                   + (objFactory.Doc.DocRemPrice != "" && objFactory.Doc.DocRemValidity != "" ? "</br>" : "") + GFunc.NEStr(objFactory.Doc.DocRemValidity, "").Replace("'", "''").Replace("\n\r", "</br>")
                                   + (objFactory.Doc.DocRemValidity != "" && objFactory.Doc.DocRemPayment != "" ? "</br>" : "") + GFunc.NEStr(objFactory.Doc.DocRemPayment, "").Replace("'", "''").Replace("\n\r", "</br>");

                            MySqlCommand cmd7 = new MySqlCommand("Update `estore_boss_quotation` set sub_total='" + (objFactory.Doc.DocSubTotal - bankchg - deliverychg) + "', bank_charges='" + bankchg +
                                                "', delivery_charges='" + deliverychg + "', doc_remark='" + rem + "' where quotation_id=" + quotation_id, con);
                           
                            cmd7.CommandType = CommandType.Text;
                            con.Open();
                            cmd7.ExecuteNonQuery();
                            con.Close();
                            objFactory.Save((int)GEnum.DocAction.Post);

                            #endregion +++ magento 2++
                        }

                        MsgBox.Show("Quotation updated successfullly in EStore.");
                    }
                }
                else
                    MsgBox.Show("This quotation is not linked to any EStore RFQ.");
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        private void checkQuotationInEStoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int MagentoVersion = SysOptionUtility.GetInt("MagentoVersion");
            

            if (MagentoVersion == 1)
            {
                System.Diagnostics.Process.Start(link + "netgo_boss/quote/quotecartview/id/" + objFactory.Doc.Custom2);
            }
            else if (MagentoVersion == 2)
            {
                string Magento2Link = SysOptionUtility.GetStr("Magento2Link");
                System.Diagnostics.Process.Start(Magento2Link + "checkout/quotation/view/reference_id/" + objFactory.Doc.Custom2);
            }
        }


        private void replyLinkToCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool proceed = false;
            try
            {
                if (SaveChanges(true, true, true, GEnum.DocAction.Undetermine) == false)
                {
                    return; //if User clicked "I don't know" or ("Save Changes" which validation is failed, the form will still be opened with dirty state)                   
                }
                if (objFactory.Doc.DocQuoteStatus == 20)
                {
                    MsgBox.Show("Customer has already confirmed the quotation. It cannot be updated.");
                    return;
                }

                string quoteviewlink = "";
                int MagentoVersion = SysOptionUtility.GetInt("MagentoVersion");
                string Magento2Link = SysOptionUtility.GetStr("Magento2Link");

                if (MagentoVersion == 1)
                {
                    quoteviewlink = link + "netgo_boss/quote/quotecartview/id/" + objFactory.Doc.Custom2;
                }
                else if (MagentoVersion == 2)
                {
                    quoteviewlink = Magento2Link + "checkout/quotation/view/reference_id/" + objFactory.Doc.Custom2;
                }

                this.Cursor = Cursors.WaitCursor;
                if (objFactory.Doc.DocQONum != "")
                {

                    string subject = "";
                    string emailBody = "";
                    int quoteID = 0;

                    if (objFactory.Doc.Custom3 == "Replied")
                    {
                        if (MsgBox.Show("The quotation has been replied before.\nAre you sure to send this again to " + objFactory.Doc.DocBAddrAttn + "?", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                        {
                            subject = "BH eStore - Quotation " + objFactory.Doc.DocID + " (Updated)";
                            proceed = true;
                        }
                    }
                    else if (MsgBox.Show("Are you sure to reply this quotation to " + objFactory.Doc.DocBAddrAttn + "?", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                    {
                        subject = "BH eStore - Quotation " + objFactory.Doc.DocID;
                        proceed = true;
                    }

                    if (proceed)
                    {

                        frmMain.gfrmMain.SetNotifyStatus("Sending Quotation email ...............");
                        if (MagentoVersion == 1)
                        {
                            emailBody =
                         "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;'>Dear " + objFactory.Doc.DocBAddrAttn + ",</p> " +
                         "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;'>" +
                         " Thank you for submitting your enquiry to Beng Hui Marine Electrical Pte Ltd.</p>" +
                            "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;'>" +
                          @"Please click <a href='" + quoteviewlink + objFactory.Doc.Custom2 + "'>here</a> to access the quotation.</p>" +
                            "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;'>" +
                         "Prefer to contact us by phone? CALL +65 6291 4444.</p>" +
                         @" </br> <a href='" + link + "' target='_blank'><img src='" + link + "media/ackemail.jpg' border='0' height='181' width='550'></a> </br>"
                        + "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;'>" +
                         " From BH eStore Team (Beng Hui Marine Electrical Pte Ltd) .</p>";

                        }
                        else if (MagentoVersion == 2)
                        {
                            emailBody =
                        @" </br> <a href='" + Magento2Link + "' target='_blank'>" + "<img src='" + Magento2Link + "media/logo/stores/1/email_logo.png' width = '190' height = '45'></a> </br>" +
                        "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;'>Dear " + objFactory.Doc.DocBAddrAttn + ",</p> " +
                        "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;'>" +
                        " Thank you for submitting your enquiry to Beng Hui Marine Electrical Pte Ltd.</p>" +
                        "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;'>" +
                        @"Please click <a href='" + quoteviewlink + "'>here</a> to access the quotation.</p>" +
                        "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;'>" +
                        "Prefer to contact us by phone? CALL +65 6291 4444.</p>" +
                        @"<a href='" + Magento2Link + "' target='_blank'><img src='" + Magento2Link + "media/estore/images/email_footer.jpg' border='0' height='181' width='550'></a>" +
                         "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;'>" +
                        " From BH eStore Team (Beng Hui Marine Electrical Pte Ltd).</p>";
                        }

                        GEmail.SendeStoreEmail(objFactory.Doc.DocBAddrEmail, subject, emailBody, null);
                        frmMain.gfrmMain.SetNormalStaus("Ready");
                        MsgBox.Show("Quotation link has been sent to " + objFactory.Doc.DocBAddrAttn);
                    }
                }
                else
                    MsgBox.Show("This quotation is not linked to any EStore RFQ.");
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
            finally
            {
                if (proceed)
                {
                    if (!objFactory.Doc.Custom3.Equals("Replied"))
                    {
                        List<SqlParameter> parmList = new List<SqlParameter>();
                        parmList.Add(new SqlParameter("@DocKey", objFactory.Doc.DocKey));
                        GFunc.ExecuteNonQueryProc("Doc_UpdateEQuoteStatus", parmList);
                        objFactory.Doc.Custom3 = "Replied";
                        objFactory.Doc.IsDirty = false;
                    }
                }
                this.Cursor = Cursors.Default;
            }
        }

        private void tspNewRevisedQuote_Click(object sender, EventArgs e)
        {
            try
            {
                if (objFactory.Doc.IsNew)
                {
                    MsgBox.Show("No data to revise. Please open a quotation.");
                    return;
                }
                else if (!SaveChanges(false, true, true, GEnum.DocAction.Undetermine))
                    return;

                htDetailGrd.Clear();
                if (!GVar.DocUpdateOption.ContainsKey(GVar.DeptUpdateOption))
                    GVar.DocUpdateOption.Add(GVar.DeptUpdateOption, true);

                objFactory.RevisedCopy();
               // SaveAttachmentsWithDMAS();

                Form_Rebind(false, true);
                AddItm_Hash();

                if (DocHDRUtil.DocDate_CustomUpdate(objFactory.Doc, htDetailGrd) == false)
                    MsgBox.Show("Unable to calculate document");

                if (MSTSalesRep.Get(GFunc.NEInt(objFactory.Doc.DocEmKey, 0)).Inactive.Value)
                {
                    MsgBox.Show("The sale representative is inactive. Please select another one.");
                }
                objFactory.Doc.DocTranGrpKey = MSTCon.Get(objFactory.Doc.DocConKey).ConChildren;
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
                DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false);
                DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, false);
                DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItmVendors, GEnum.Details.Doc_ItmVendor, false);
                DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetVendor, GEnum.Details.Doc_Vendor, false);
                DisableCreateButtons();
                ControlSetting();
                MsgBox.Show("This revised copy has not been saved yet. Please click either Draft or Post button to save it.");

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
                GVar.DocUpdateOption.Remove(GVar.DeptUpdateOption);
            }
        }

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

        static string Encrypt(string plainText)
        {
            string key = "yourSecretKey\0\0\0";
          string cipherText;
          byte[] Key;
          int Mode,BlockSize,Padding;
          var rijndael = new System.Security.Cryptography.RijndaelManaged()
          {
             Key=Encoding.UTF8.GetBytes(key),
             Mode=CipherMode.ECB,
             BlockSize=256,
             Padding=PaddingMode.Zeros,
          };
          ICryptoTransform encryptor = rijndael.CreateEncryptor(rijndael.Key, null);

          using (var memoryStream = new MemoryStream())
          {
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
              using (var streamWriter = new StreamWriter(cryptoStream))
              {
                streamWriter.Write(plainText);
                streamWriter.Flush();
              }
              //cipherText = Convert.ToBase64String(Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(memoryStream.ToArray())));
              cipherText = Convert.ToBase64String(memoryStream.ToArray());
              //cryptoStream.FlushFinalBlock();
            }
          }
            
          cipherText = cipherText.Replace('+', '-').Replace('/', '_').Replace("=", "");
          return cipherText;
        }
        //added by nnt on Dec 2020
        #region GetARAging
        

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

        #endregion GetARAging

        private void taNumericEditor7_ValueChanged(object sender, EventArgs e)
        {
            ShowGPSummary();
        }
        
        private void ShowGPSummary()
        {
            /* added by YST to show summary GP for Athena */
            if (SysOptionUtility.DatabaseBranchCode == DBCode.ADL)
            {
                pnlGP.BringToFront();
                pnlSubTotal.SendToBack();
                decimal? TotalPrice = 0, GP = 0, GPPercent = 0;
                decimal? TotalCost = objFactory.DocDetItms.AsEnumerable()
                            .Where(x => x["ItmQty"] != DBNull.Value && x["ItmVendorPrice"] != DBNull.Value)
                            .Select(r => r.Field<decimal?>("ItmQty") * r.Field<decimal?>("ItmVendorPrice"))
                            .Sum();
                TotalPrice = objFactory.Doc.DocSubTotal;
                GP = TotalPrice - TotalCost;
                if (TotalPrice > 0) GPPercent = (GP / TotalPrice) * 100;
                lblGPHeder.Text = "Total Price\nTotal Cost\nMargin";
                lblColon.Text = ":\n:\n:";
                lblGP.Text = String.Format("{0: #, ##0.00; -#,##0.00} ", TotalPrice).Replace(" ", "") + "\n"
                           + String.Format("{0: #, ##0.00; -#,##0.00} ", TotalCost).Replace(" ", "") + "\n"
                           //+ String.Format("{0: #, ##0.00; -#,##0.00} ", GP).Replace(" ", "")                           
                           + (GPPercent > 0 ? GFunc.RndC(GPPercent, GVar.RndDecs.Amtpt).ToString() + " %": "");
            }
        }

        private void tsbReloadLocal_Click(object sender, EventArgs e)
        {
            try
            {
                if (MsgBox.Show("Are you sure to update local form settings?"
                                , GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                {
                    string filePath = Application.StartupPath + @"\FormSettings\" + SysOptionUtility.DatabaseBranchCode + @"\Ref\";
                    List<SqlParameter> list = new List<SqlParameter>();
                    list.Add(new SqlParameter("@UserKey", AppInfor.CurrentUserKey));
                    string refdata = GFunc.ExecuteScalar("SYSFormSetting_GetMstRefJson", list);
                    //File.WriteAllText(Path.GetDirectoryName(filePath) + @"\MSTNREF.txt", refdata);
                    if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    string[] arr = refdata.Split(new string[] { "}]," }, StringSplitOptions.None);
                    foreach (string data in arr)
                    {
                        if (data == "")
                            continue;
                        try
                        {
                            string[] dataArr = data.Split(new string[] { ":[" }, StringSplitOptions.None);
                            if (dataArr.Length >= 1)
                            {
                                string fileName = filePath + (dataArr[0].Replace("\"", "").Replace("{", "")) + ".txt";
                                File.WriteAllText(fileName, "[" + dataArr[1] + "]");
                                //DataTable dt = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>("[" + dataArr[1] + "]");
                                //string aaa = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                            }
                        }
                        catch (Exception ex)
                        {
                            MsgBox.Show(ex.Message + "\nError in the setting " + data);
                        }
                    }
                    //DataSet dsAllSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<DataSet>(refdata);

                }
            }
            catch(Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
        }


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
    }
}
