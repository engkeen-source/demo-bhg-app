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

namespace WinUI
{
    public partial class frmARRO : Form, DocInterface
    {
        #region Local Variables

        private BOLib.ARROFactory objFactory = null;
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
        //int paypalDItm = 0;
        UltraGridRow pRow = null;

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
        public frmARRO()
        {
            InitializeComponent();
        }//Completed
        public frmARRO(GEnum.SystemCode DocCodeKey)
        {
            InitializeComponent();
            OpenCode = DocCodeKey;           
        }//Completed
        public frmARRO(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            InitializeComponent();
            OpenCode = DocCodeKey;
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
        }//Completed
        public frmARRO(int? SOURCE_DC, int? SOURCE_DK)
        {
            //Create Document with data from other source
            InitializeComponent();
            source_DC = SOURCE_DC;
            source_DK = SOURCE_DK;
            OpenCode = GEnum.SystemCode.Reserve_Order;
        }//Completed

        //Form Events
        private void frm_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.objFactory = new BOLib.ARROFactory(BOLib.GEnum.InstanceMode.Normal, OpenCode);
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

                    //Create Document from QO,SO
                    if (source_DK > 0 && !GFunc.IsNEZ(source_DC))
                    {
                        if (objFactory.GetCopy_ByDC(source_DC, source_DK) != GVar.gcPass)
                        {
                            formClose = true;
                            return;
                        }
                        //objFactory.Doc.DocTranGrpKey = MSTCon.Get(objFactory.Doc.DocConKey).ConChildren;
                        htDetailGrd.Clear();
                        htDetailGrd.Add(GEnum.Details.Doc_Itm, objFactory.DocDetItms);
                        DocHDRUtil.DocDate_CustomUpdate(objFactory.Doc, htDetailGrd);
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
                    DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, true);
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
                //tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmQtyBalance"].CellAppearance.ForeColor = Color.Red;  // added by KKAung on 10-Oct-2022               

                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].Header.Caption = "Latest Cost " + DocCurrKey.Text;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmControlPrice"].Header.Caption = "EStore Price " + DocCurrKey.Text;


                AuditLogCopyNPost.Visible = false;
                DocDate.Focus();
                EStoreMenuSetting();

                if (source_DK > 0 && !GFunc.IsNEZ(source_DC))
                    JobItemsVisibleCheckSet();

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
                DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, formload);
                DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, formload);
                DocPrinted.Enabled = false;               
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
                if (objFactory.Doc.DocID.ToUpper().StartsWith("ERO") && objFactory.Doc.DocTypeNm.ToUpper() == "ESTORE RO")                
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

                    ConInActive = ((DataTable)DocConKey.DataSource).Select("[Inactive]=True").AsEnumerable().CopyToDataTable();

                }
            }

            if (OpenID.Text == "")
            {
                ((DataTable)DocConKey.DataSource).DefaultView.RowFilter = "[Inactive]=False";
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
                if (objFactory.New(tagrdDetItms) == GVar.gcPass)
                {
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
        }
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
                if(objFactory.Doc.DocState == (int?)GEnum.DocState.Posted)
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
                btnAttachmentEdit.Text = "(" + objFactory.Doc.Attachments.Count(o => o.DocDItm == -1) + ")";
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
                        objFactory.DocDetItms = (ARRODetItms)tagrdDetItms.DataSource;


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
            ARROFactory objFactoryTmp = new ARROFactory(GEnum.InstanceMode.Normal, OpenCode);
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
                                    objFactory.Doc.DefJobKey = GFunc.NEInt(drs[0]["ItmJobKey"], 0);
                                    DefJobKey.Value = GFunc.NEInt(drs[0]["ItmJobKey"], 0);
                                    DefJobKey.SelectedRow = row;
                                }
                            }
                        }
                        DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false);//check mic /Pauk change formload=true to false
                        DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, false);//check mic /Pauk change formload=true to false
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
                        DocHDRUtil.FormControlLock_Set(this, objFactory.Doc, objFactory.PermID, false);
                        DocHDRUtil.FormGridLock_Set(objFactory.Doc, tagrdDetItms, GEnum.Details.Doc_Itm, false);
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
                else if (tagrdDetItms.Rows.Count > 0)
                {
                    pRow = tagrdDetItms.Rows.OfType<UltraGridRow>().ToList().Find
                        (row => (int)row.Cells["ItmKey"].Value == SysOptionUtility.ProcessingItem /*102937*/);
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
        private void tagrdDetItms_InitializeRow(object sender, InitializeRowEventArgs e)
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
                if (pRow != null && objFactory.Doc.IsDirty) CalCulateProcessFee(); 
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
                        else if (pRow !=null && pRow.Index == ((UltraGrid)sender).ActiveRow.Index)
                            pRow = null;
                    } 
                    //added by thettm on 17 jan 2019(end)
                    GridCellLock_Set();                  
                    
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
                            e.Cell.Row.Cells["NSLink"].Value = "11151-" + objFactory.Doc.DocKey + "-" + e.Cell.Row.Cells["DocItmKey"].Value;
                        }
                        break;
                }

                AddItm_Hash();
                string listSetingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, e.Cell.Column.Key, ((Control)sender).Name);
                //if (DocDetUtil.ItmRow_CustomCellUpdate(objFactory.Doc, htDetailGrd, GEnum.Details.Doc_Itm, listSetingID) == false)
                if (DocDetUtil.ItmRow_CustomCellUpdate(objFactory.Doc, htDetailGrd, GEnum.Details.Doc_Itm, listSetingID, ExclusiveSaleJob ? 1 : 0) == false)
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
                                POKey = GetDocKey(e.Cell.Text, itmtype == 600);
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
                            break;
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
                            else if (pRow!=null && pRow.Index == ((UltraGrid)sender).ActiveRow.Index)
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

                if (e.Rows.Count() > 0)
                    if (GFunc.NEInt(e.Rows[0].Cells["ItmBatchKey"].Value, 0) > 0)
                    {
                        MsgBox.Show("Not allow to delete this row which is addded from Job.");
                        e.Cancel = true; // Cancels the delete action
                        return;          // Stop checking further
                    }

                bool clearprocessing = false;

                if (pRow != null)
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
        private bool SaveChanges(bool canDiscardChanges, bool saveOnlyWhenDirty, bool promptToSave, GEnum.DocAction ButtonAction)
        {
            bool result = false;
            GEnum.MsgBoxButton btnSelect;

            try
            {
                if (objFactory.Doc.IsReadOnly)
                    return true;               

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

                    DataTable dtNS = (from row in objFactory.DocDetItms.AsEnumerable()
                                      where row.Field<int>("LineType") == 1000 && row.Field<int>("ItmType") == 600 &&
                                      !(row.Field<string>("NSLink").Substring(0, 5) == "13250" || GFunc.NEStr(row.Field<string>("APPOID"), "") != "")
                                      && GFunc.IsNEZ(row.Field<int?>("ItmBatchKey"))//Not Job
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

                    //Saving
                    if (GlobalUI.UpdateAssemblyChildItem(this.objFactory.Doc, tagrdDetItms) == false)
                        return false;

                    //// 22_dec_2017 
                    //string date = objFactory.Doc.DocReqDate.ToString().Split(' ')[0];
                    //string time = RequiredTime.Text.ToString();
                    //DateTime docreqdate = Convert.ToDateTime(date + " " + time);
                    //objFactory.Doc.DocReqDate = docreqdate;

                    string Msg = "";
                    if (string.IsNullOrEmpty(objFactory.Doc.DocReqDate.ToString()))
                        Msg = "Please fill Required Date.";
                    
                    else if (DateTime.Compare((DateTime)objFactory.Doc.DocReqDate,(DateTime)objFactory.Doc.DocDate) == 0)                   
                        Msg = "Required Date should not be same as Document Date.";
                   
                    else if (DateTime.Compare((DateTime)objFactory.Doc.DocReqDate, (DateTime)objFactory.Doc.DocDate) < 0)                   
                        Msg = "Required Date should not be ealier than Document Date.";

                    if (Msg != "")
                    {
                        MsgBox.Show(Msg,GEnum.MsgBoxIcon.Error,GEnum.MsgBoxButton.OK);
                        tabDetailList.SelectedTab = tabDetailList.Tabs["tsbMain"];
                        DocReqDate.Focus();
                        return false;
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
                    }

                    if ((SysOptionUtility.DatabaseBranchCode == DBCode.BHM ||
                         SysOptionUtility.DatabaseBranchCode == DBCode.SOP) &&
                         ButtonAction == GEnum.DocAction.Post)
                    {
                        /* Prompt warning message for the price 0 of the items added by YST on 2023/04/03 */
                        if (CheckPriceZero() == false)
                            return false;
                    }

                    /* Check Payment Mode and Document Type for estore processing fees, added by YST on 2024/04/01 */
                    if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                    {
                        if (CheckPaymentMode() == false)
                            return false;
                    }

                    if (objFactory.Save((int)ButtonAction) == GVar.gcPass)
                        result = true;

                    if (result && updateDoc && SysOptionUtility.HasDMASLink)
                        DocHDRUtil.ExportToDMAS(objFactory.Doc);

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
                Form_RefreshAll(false, false);
                DocList_Refresh();
            }
        }//Completed
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
                btnAttachmentEdit.Text = "(" + objFactory.Doc.Attachments.Count + ")";
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmLatestCostF"].Header.Caption = "Latest Cost " + DocCurrKey.Text;
                tagrdDetItms.DisplayLayout.Bands[0].Columns["ItmControlPrice"].Header.Caption = "EStore Price " + DocCurrKey.Text;

                if (tagrdDetItms.Rows.Count > 0)
                {
                    pRow = tagrdDetItms.Rows.OfType<UltraGridRow>().ToList().Find
                        (row => (int)row.Cells["ItmKey"].Value == SysOptionUtility.ProcessingItem/*102937*/);

                } 

                ////22 dec 2017
                //RequiredTime.Text = objFactory.Doc.DocReqDate.ToString().Split(' ')[1].ToString();

                #endregion
                OpenID.Text = DocID.Value.ToString();
                FilterCustomer(); //added by thettm on 12-sept-2017
                EStoreMenuSetting();
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

            try
            {
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
                      
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (MsgBoxGrid.Show("Already Created, Continue?", ds.Tables[0], GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                                runCreateProcess = true;
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
                        MsgBox.Show("Cannot create Document when reserve order has not been posted");
                        return false;
                    }
                }
                #endregion

                #region Create document process
                if (runCreateProcess)
                {
                    switch (destination_DC)
                    {
                        case (int)GEnum.SystemCode.Sales_Order:
                            // added by KKAung on 13 Jan 2022 (start)
                            frmARSO frmARSO = new frmARSO();
                            if (DocTypeNm.Text == "Reserve Order")
                            {
                                //int CCBType = GFunc.NEInt(DocConKey.SelectedRow.Cells["CCBType"].Value, 0);     
                                //bool CashCustomer = CCBType == (int)GEnum.CCBType.CH || (CCBType == (int)GEnum.CCBType.B && (DocTermKey.Text == "CASH" || DocTermKey.Text == "TT PAYMENT"));
                                bool CashCustomer = GFunc.NEInt(DocConKey.SelectedRow.Cells["CCBType"].Value, 0) == (int)GEnum.CCBType.CH; //added by KKAung on 18 Jan 2022 
                                this.Close();
                                frmARSO = new frmARSO(objFactory.Doc.DocCodeKey, objFactory.Doc.DocKey, CashCustomer);  
                            }
                            else
                            // added by KKAung on 13 Jan 2022 (end)
                            {
                                this.Close();
                                frmARSO = new frmARSO(objFactory.Doc.DocCodeKey, objFactory.Doc.DocKey);
                            }
                            frmARSO.MdiParent = frmMain.gfrmMain;
                            frmARSO.Show();
                            break;                      
                    }
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
            return true;
        }//Completed
        private bool CheckPriceZero()
        {
            /* Prompt warning message for the price 0 of the items on 2023/04/03 */
            bool result = true; GEnum.MsgBoxButton btnSelect;
            DataTable dtPrice = (from row in objFactory.DocDetItms.AsEnumerable()
                                 where (row.Field<decimal?>("ItmQty") > 0 && row.Field<decimal?>("ItmPriceAfter") == 0
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
        private bool CheckPaymentMode()
        {
            try
            {
                /* added by YST to check payment mode for the processing fees. Otherwise, procesing fees cannot be displayed in eStore website */
                bool result = true; string msgWarning = ""; int defAcckey = 0; string defAccDes = "";
                DataRow[] dr;
                DataTable dtItem = (DataTable)tagrdDetItms.DataSource;

                if (dtItem == null || dtItem.Rows.Count == 0) result = true;

                dr = dtItem.Select("ItmKey = " + SysOptionUtility.ProcessingItem + " or ItmID like 'PayPal%'");
                if (dr.Length > 0)
                {
                    if (GFunc.NEStr(dr[0]["ItmID"], "").ToLower().StartsWith("paypal") && !DocRemPayment1.Text.ToLower().EndsWith("pal")) /* Pay Pal or PayPal*/
                    {
                        msgWarning = "To apply PayPal charges to eStores, the payment mode should be PayPal.";
                        MsgBox.Show(msgWarning, GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                        DocRemPayment1.Focus();
                    }
                    else if (!DocRemPayment1.Text.ToLower().EndsWith("pal") && DocRemPayment1.Text.ToLower() != "stripe")
                    {
                        msgWarning = "To apply processing fees to eStores, the payment mode should be PayPal or Stripe.";
                        MsgBox.Show(msgWarning, GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                        DocRemPayment1.Focus();
                    }
                    else if (!DocTypeNm.Text.ToLower().Contains("estore"))
                    {
                        msgWarning = "The current document type is " + DocTypeNm.Text + ".<br/>Would you like to amend it to eStore RO ? ";
                        if (MsgBox.Show(msgWarning, GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                            DocTypeNm.Focus();
                        else result = true;
                    }
                }
                else if (DocRemPayment1.Text.ToLower().EndsWith("pal") || DocRemPayment1.Text.ToLower() == "stripe")
                {
                    msgWarning = "Please include processing fees for the PayPal or Stripe payment.";
                    MsgBox.Show(msgWarning, GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                    tabDetailList.SelectedTab = tabDetailList.Tabs["tsbItems"];
                }

                dtItem = null;
                if (msgWarning != "")
                {
                    result = false;
                }
                else
                {                   
                    if (DocRemPayment1.SelectedRow != null && (DocRemPayment1.Text.ToLower().EndsWith("pal") || DocRemPayment1.Text.ToLower() == "stripe"))
                    {
                        /* SELECT * FROM SYS_FormSettingID WHERE ListID = 'eStorePayType';
                           ListSQL => SELECT MsgValue,LangText1,Custom1 As DefAccKey,Custom2 As DefAccID,Custom3 As DefAccDes FROM SYS_MsgListText WHERE DataGrp=504; */
                        
                        int.TryParse(GFunc.NEStr(DocRemPayment1.SelectedRow.Cells["DefAccKey"].Value, ""), out defAcckey);
                        defAccDes = GFunc.NEStr(DocRemPayment1.SelectedRow.Cells["DefAccDes"].Value, "");

                        if (defAcckey > 0)
                        {
                            int rowIndex = tagrdDetItms.Rows.FirstOrDefault(r => Convert.ToInt32(r.Cells["ItmKey"].Value.ToString()) == SysOptionUtility.ProcessingItem).Index;
                            if (rowIndex >= 0)
                            {
                                tagrdDetItms.Rows[rowIndex].Cells["ItmAccKey"].Value = defAcckey;
                                tagrdDetItms.Rows[rowIndex].Cells["ItmAccDes"].Value = defAccDes;
                                objFactory.DocDetItms.AcceptChanges();
                                this.tagrdDetItms.UpdateData();
                            }
                        }
                    }
                    
                }
                return result;
            }
            catch 
            {
                MsgBox.Show("An exception is found. Please check with the authorized person.", GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                return false;
            }
            
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
            

            /*if (MSTSalesRep.Get(GFunc.NEInt(DocEmKey.Value, 0)).Inactive.Value)
            {
                MsgBox.Show("The sale representative is inactive. Please select another one.");
                e.Cancel = true;
                return;
            }
            if(GFunc.IsNEZ(DocTranGrpKey.Value))
            {
                MSTAccTranGrp t = MSTAccTranGrp.Get(DocEmKey.Text, 3);
                if (t != null)
                    if (!GFunc.IsNEZ(t.TranGrpKey))
                    {
                        DocTranGrpKey.SetValueTrigger(t.TranGrpKey, false);
                    }
            }*/
        }

    
        private void updateEStoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges(true, true, true, GEnum.DocAction.Undetermine) == false)
                {
                    return; //if User clicked "I don't know" or ("Save Changes" which validation is failed, the form will still be opened with dirty state)                   
                }
              
                this.Cursor = Cursors.WaitCursor;

                if (objFactory.Doc.DocRef != "")
                {
                    bool proceed = true;
                    string subject = "";
                    string emailBody = "";
                    int quoteID = 0;

                    if (objFactory.Doc.Custom3 == "Replied")
                    {
                        if (MsgBox.Show("The reserve order has been replied before.\nAre you sure to update it?", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                        {
                            proceed = false;
                        }
                    }

                    if (proceed)
                    {
                        string skus = "";
                        string data = "";
                        string ids = "";
                        string rem = "";
                        decimal bankchg = 0M;
                        decimal delchg = 0M;
                        //ttm
                        decimal paypalchg = 0M;
                        string delchgDesc = "";

                        int id = 0;
                        string sku = "";
                        string itmDes = "";
                        int qty = 0;
                        string UOM = "";
                        decimal price = 0M;
                        decimal amount = 0M;
                        string itmremark = "";
                        string docremark="";


                        MSTSalesRep objSR = MSTSalesRep.Get(objFactory.Doc.DocEmKey);

                         int MagentoVersion = SysOptionUtility.GetInt("MagentoVersion");

                         if (MagentoVersion == 1)
                         {
                             #region +++ magento 1++
                             //**temp
                             MySqlConnection con = new MySqlConnection("userid=bhestore_may;password=Thinzar@12;server=101.100.209.196;database=bhestore_magento18jul;connection timeout=180");
                             //  MySqlConnection con = new MySqlConnection("userid=root;password=;server=localhost;database=bhestore_magento18jul;connection timeout=180");

                             foreach (UltraGridRow row in tagrdDetItms.Rows)
                             {
                                 if (row.Cells["ItmID"].Text.ToUpper().Equals("BANK CHARGES"))
                                 {
                                     bankchg = GFunc.NEDec(row.Cells["ItmAmtF"].Value, 0);
                                 }
                                 else if (row.Cells["ItmID"].Text.ToUpper().Equals("FREIGHT CHARGES") || row.Cells["ItmID"].Text.ToUpper().Equals("F FREIGHT"))
                                 {
                                     delchg = GFunc.NEDec(row.Cells["ItmAmtF"].Value, 0);
                                     delchgDesc = GFunc.NEStr(row.Cells["ItmDes"].Value, "");
                                 }
                                 //ttm
                                 else if (row.Cells["ItmID"].Text.ToUpper().Equals("PROCESSING FEE") || row.Cells["ItmID"].Text.ToUpper().Contains("PAYPAL"))
                                 {
                                     paypalchg = GFunc.NEDec(row.Cells["ItmAmtF"].Value, 0);
                                 }
                                 else if (row.Cells["ItmIGrpDItm"].Text.Replace(",", "") != "0" && row.Cells["ItmIGrpDItm"].Text.Replace(",", "") != "")
                                 {
                                     ids += row.Cells["ItmIGrpDItm"].Text.Replace(",", "") + ",";
                                     skus += row.Cells["ItmID"].Text.Replace(",", "") + ",";
                                     data += row.Cells["ItmQty"].Text.Replace(",", "") + "," + row.Cells["ItmUOMKey"].Text.Replace(",", "") + "," + row.Cells["ItmPriceUser"].Text.Replace(",", "") + "," + row.Cells["ItmAmtShw"].Text.Replace(",", "") +
                                       "," + row.Cells["Custom1"].Text.Replace(",", "") + "," + row.Cells["Custom2"].Text.Replace(",", "") + "," + row.Cells["ItmDes"].Text.Replace(",", "").Replace("\n\r", "</br>") + ","
                                       + row.Cells["ItmMark"].Text.Replace(",", "") + "," + row.Cells["ItmID"].Text.Replace(",", "") + "#%#";
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
                                       "," + row.Cells["Custom1"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["Custom2"].Text.Replace(",", "").Replace(",", "") + "," + row.Cells["ItmDes"].Text.Replace(",", "").Replace("\n\r", "</br>")
                                       + "," + row.Cells["ItmMark"].Text.Replace(",", "") + "," + row.Cells["ItmID"].Text.Replace(",", "") + "#%#";
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

                             rem = GFunc.NEStr(objFactory.Doc.DocRem, "").Replace("\n\r", "</br>");

                             //ttm
                             string sql = "update `netgo_boss_quoteconfirm` inner join `netgo_boss_quote` on netgo_boss_quoteconfirm.quote_entity_id=netgo_boss_quote.entity_id set " +
                                   "`sales_id`='" + objSR.EmID + "'," +
                                       "`salesrep_email`='" + objSR.Custom1 + "' where so_num='" + objFactory.Doc.DocID + "'";


                             MySqlCommand cmd1 = new MySqlCommand(sql, con);
                             cmd1.CommandType = CommandType.Text;
                             con.Open();
                             cmd1.ExecuteNonQuery();
                             con.Close();

                             //ttm
                             string sql1 = "update `netgo_boss_quoteconfirm` set " +
                                       "`comment`='" + objFactory.Doc.DocRem + "'," +
                                         "`sales_confirm_date`='" + objFactory.Doc.DocDate + "'," +
                                          "`status`='salesconfirmed'," +
                                             "`bank_charges`=" + bankchg + "," +
                                              "`delivery_charges`=" + delchg + "," +
                                            "`sub_total`='" + (objFactory.Doc.DocSubTotal - bankchg - delchg) + "'," +
                                             "`gst_percent`='" + objFactory.Doc.DocTaxGrpRate + "'," +
                                              "`gst_amount`='" + objFactory.Doc.DocTaxTotal + "'," +
                                               "`pay_pal_fee`=" + paypalchg + "," +
                                                  "`payment_mode`='" + objFactory.Doc.DocRemPayment + "'," +
                                               "`grand_total`='" + objFactory.Doc.DocGrand + "'," +
                                                "`items_ids`='" + ids + "'," +
                                                 "`items_ids`='" + ids + "'," +
                                                 "`items_details`='" + data + "'," +
                              "`curr_id`='" + DocCurrKey.Text + "',`delivery_chargesdesc`='" + delchgDesc + "' where so_num='" + objFactory.Doc.DocID + "'";


                             MySqlCommand cmd2 = new MySqlCommand(sql1, con);
                             cmd2.CommandType = CommandType.Text;
                             con.Open();
                             cmd2.ExecuteNonQuery();
                             con.Close();

                             #endregion +++ magento 2++
                         }
                         else if (MagentoVersion == 2)
                         {

                            #region +++ magento 2++

                            //string conStr = "userid=ywpvyvgedh;password=4MAcPftrEU;server=172.104.41.102;database=ywpvyvgedh;connection timeout=180";
                            string conStr = "userid=afjmsnfvpe;password=nJUXgzG6PQ;server=172.104.41.102;database=afjmsnfvpe;connection timeout=180";
                            MySqlConnection con = new MySqlConnection(conStr);

                            int po_id = 0;
                            MySqlCommand cmd2 = new MySqlCommand("select po_id from `estore_boss_po` where `estore_boss_po`.`ro_no`='" + objFactory.Doc.DocID + "'", con);
                            cmd2.CommandType = CommandType.Text;
                            con.Open();
                            po_id = GFunc.NEInt(cmd2.ExecuteScalar(), 0);
                            con.Close();

                            if (po_id == 0)
                            {
                                throw new Exception("RO not exists in estore to update.");
                            }

                            string sql2 = "delete from `estore_boss_po_items` where po_id = " + po_id;

                            MySqlCommand cmd5 = new MySqlCommand(sql2, con);
                            cmd5.CommandType = CommandType.Text;
                            con.Open();
                            cmd5.ExecuteNonQuery();
                            con.Close();

                            foreach (UltraGridRow row in tagrdDetItms.Rows)
                            {
                                if (row.Cells["ItmID"].Text.ToUpper().Equals("BANK CHARGES"))
                                {
                                    bankchg = GFunc.NEDec(row.Cells["ItmAmtF"].Value, 0);
                                }
                                else if (row.Cells["ItmID"].Text.ToUpper().Equals("FREIGHT CHARGES") || row.Cells["ItmID"].Text.ToUpper().Equals("F FREIGHT"))
                                {
                                    delchg = GFunc.NEDec(row.Cells["ItmAmtF"].Value, 0);
                                    delchgDesc = GFunc.NEStr(row.Cells["ItmDes"].Value, "");
                                }
                                //ttm
                                else if (row.Cells["ItmID"].Text.ToUpper().Equals("PROCESSING FEE") || row.Cells["ItmID"].Text.ToUpper().Contains("PAYPAL"))
                                {
                                    paypalchg = GFunc.NEDec(row.Cells["ItmAmtF"].Value, 0);
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

                                    string sql1 = "insert into `estore_boss_po_items` " +
                                 " (po_id,	product_id,	remark,	sku,	created_at,	updated_at, product_name,	qty,	uom,	price,	amount) " +
                                 "VALUES (" + po_id + "," +
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
                                    qty = GFunc.NEInt(row.Cells["ItmQty"].Text.Replace(",", ""), 0);
                                    UOM = row.Cells["ItmUOMKey"].Text.Replace(",", "");
                                    price = GFunc.NEDec(row.Cells["ItmPriceUser"].Text.Replace(",", ""), 0);
                                    amount = GFunc.NEDec(row.Cells["ItmAmtShw"].Text.Replace(",", ""), 0);
                                    itmDes = row.Cells["ItmDes"].Text.Replace(",", "").Replace("'", "''").Replace("\n\r", "</br>");
                                    itmremark = row.Cells["Custom2"].Text.Replace(",", "").Replace("'", "''");

                                    string sql1 = "insert into `estore_boss_po_items` " +
                                   " (po_id,	product_id,	remark,	sku,	created_at,	updated_at, product_name,	qty,	uom,	price,	amount) " +
                                   "VALUES (" + po_id + "," +
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
                                else
                                    docremark = docremark + row.Cells["ItmDes"].Text.Replace("'", "''").Replace("\n\r", "</br>") + "</br>";
                            }

                            rem = GFunc.NEStr(objFactory.Doc.DocRem, "").Replace("'", "''").Replace("\n\r", "</br>");

                            string sql5 = "Update `estore_boss_po` set " +
                                  "`customer_po_no`='" + objFactory.Doc.DocCustPONum + "'," +
                                    "`remark`='" + rem.Replace("'", "''") + "'," +
                                    "`doc_remark`='" + docremark.Replace("'", "''") + "'," +
                                      "`date`='" + objFactory.Doc.DocDate.Value.ToString("yyyy-MM-dd") + "'," +
                                       "`status`='salesconfirmed'," +
                                  "`bank_charges`=" + bankchg + "," +
                                            "`delivery_charges`=" + delchg + "," +
                                            "`pay_pal_fee`=" + paypalchg + "," +
                                         "`sub_total`='" + (objFactory.Doc.DocSubTotal - bankchg - delchg - paypalchg) + "'," +
                                          "`gst_percent`='" + objFactory.Doc.DocTaxGrpRate + "'," +
                                           "`gst`='" + objFactory.Doc.DocTaxTotal + "'," +
                                 "`payment_mode`='" + objFactory.Doc.DocRemPayment.Replace("'", "''") + "'," +
                                                "`currency`='" + DocCurrKey.Text + "'," +
                                                "`currency_rate`='" + objFactory.Doc.DocCurrRate + "'," +
                                            "`grand_total`='" + objFactory.Doc.DocGrand + "'," +
                                 "`delivery_charges_desc`='" + delchgDesc.Replace("'", "''") + "'," +
                                          "sales_person='" + objSR.EmID + "', sales_person_email='" + objSR.Custom1 + "' where po_id=" + po_id;

                            MySqlCommand cmd7 = new MySqlCommand(sql5, con);
                            cmd7.CommandType = CommandType.Text;
                            con.Open();
                            cmd7.ExecuteNonQuery();
                            con.Close();
                            objFactory.Save((int)GEnum.DocAction.Post);

                            ////ttm
                            //string sql = "update `estore_boss_po` inner join `estore_boss_quotation` on estore_boss_po.quote_id=estore_boss_quotation.quotation_id set " +
                            //      "`sales_person`='" + objSR.EmID + "'," +
                            //          "`sales_person_email`='" + objSR.Custom1 + "' where ro_no='" + objFactory.Doc.DocID + "'";

                            //MySqlCommand cmd4 = new MySqlCommand(sql, con);
                            //cmd4.CommandType = CommandType.Text;
                            //con.Open();
                            //cmd4.ExecuteNonQuery();
                            //con.Close();
                            #endregion +++ magento 2++

                        }

                        MsgBox.Show("Reserve Order updated successfullly in EStore.");
                    }
                }
                else
                    MsgBox.Show("This reserve order is not linked to any EStore Quotation.");
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

        private void checkSOInEStoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int MagentoVersion = SysOptionUtility.GetInt("MagentoVersion");

            if (MagentoVersion == 1)
            {
                if (DocRemPayment.Text.ToLower().Equals("paypal"))
                    System.Diagnostics.Process.Start("https://bh-estore.com/netgo_boss/quote/poview/id/" + objFactory.Doc.Custom2);
                else
                    System.Diagnostics.Process.Start("https://bh-estore.com/netgo_boss/quote/posubmit/id/" + objFactory.Doc.Custom2);
            }
            else if (MagentoVersion == 2)
            {
                string Magento2Link = SysOptionUtility.GetStr("Magento2Link");
                System.Diagnostics.Process.Start(Magento2Link + "checkout/purchaseorder/process/reference_id/" + objFactory.Doc.Custom2);
            }
                ////**temp
                //if (DocRemPayment.Text.Equals("Pay Pal"))
                //    System.Diagnostics.Process.Start("http://localhost:8088/bhstore/www/netgo_boss/quote/poview/id/" + objFactory.Doc.Custom2);
                //else
                //    System.Diagnostics.Process.Start("http://localhost:8088/bhstore/www/netgo_boss/quote/posubmit/id/" + objFactory.Doc.Custom2);
            }

        private void confirmLinkToCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool proceed = false;
            try
            {
                if (SaveChanges(true, true, true, GEnum.DocAction.Undetermine) == false)
                {
                    return; //if User clicked "I don't know" or ("Save Changes" which validation is failed, the form will still be opened with dirty state)                   
                }

                this.Cursor = Cursors.WaitCursor;
                if (objFactory.Doc.DocRef != "")
                {

                    string subject = "";
                    string emailBody = "";
                    int quoteID = 0;

                    if (objFactory.Doc.Custom3 == "Replied")
                    {
                        if (MsgBox.Show("The order confirmation has been replied before.\nAre you sure to send this again to " + objFactory.Doc.DocBAddrAttn + "?", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                        {
                            subject = "BH eStore - Order Confirmation " + objFactory.Doc.DocRef + " (Updated)";
                            proceed = true;
                        }
                    }
                    else if (MsgBox.Show("Are you sure to reply order confirmation to " + objFactory.Doc.DocBAddrAttn + "?", GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                    {
                        subject = "BH eStore - Order Confirmation " + objFactory.Doc.DocRef;
                        proceed = true;
                    }

                    if (proceed)
                    {

                        string salesemail = MSTSalesRep.Get(objFactory.Doc.DocEmKey).Custom1;
                        if (salesemail == "")
                            salesemail = "estore@benghui.com";
                        frmMain.gfrmMain.SetNotifyStatus("Sending Order Confirmation email ...............");

                        /*string imglink = "https://bh-estore.com/skin/frontend/default/bhglobal/images/OrderRoute/Email_SalesPOConfirm.JPG";

                        if (!objFactory.Doc.DocQONum.Trim().StartsWith("eQO"))
                               imglink = "https://bh-estore.com/skin/frontend/default/bhglobal/images/OrderRoute/Email_SalesPOConfirm1.JPG";
                        */
                        string imglink = "";
                        string msg1 = "";
                        if (objFactory.Doc.DocRemDelivery == "Self Collection")
                            msg1 = "";
                        else
                            msg1 = "Reserved Items are ready for delivery";

                        int MagentoVersion = SysOptionUtility.GetInt("MagentoVersion");

                        if (MagentoVersion == 1)
                        {
                            imglink = "https://bh-estore.com/skin/frontend/default/bhglobal/images/OrderRoute/Email_SalesPOConfirm.JPG";

                            if (!objFactory.Doc.DocQONum.Trim().StartsWith("eQO"))
                                imglink = "https://bh-estore.com/skin/frontend/default/bhglobal/images/OrderRoute/Email_SalesPOConfirm1.JPG";

                            if (DocRemPayment.Text.ToLower().Equals("paypal"))
                                emailBody =
                                "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>Dear " + objFactory.Doc.DocBAddrAttn + ",</p> " +
                                "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                                "We will process your order as soon as your payment clears. " + msg1 + ".</p>" +
                                    @"</br> <img src='" + imglink + "' border='0'>" +
                                    "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                                "To complete the processing of your order, please proceed for payment.</p>" +
                                    "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                                    @"Please click <a href='https://bh-estore.com/netgo_boss/quote/poview/id/" + objFactory.Doc.Custom2 + "'>here</a> to view your order.</p>" +
                                "<p style='padding-top:20px;padding-left:10px;padding-bottom:0px;width:'100%';font:Arial;font-size:10px;>" +
                                "From <b>BH eStore Team</b> of Beng Hui Marine Electrical Pte Ltd</p>" +
                                @"<p style='padding-top:0px;padding-bottom:0px;'>Contact us at <b>+65 6291 4444</b> (Monday to Friday)</p>" +
                                @" </br> <a href='https://bh-estore.com/' target='_blank'><img src='https://bh-estore.com/media/ackemail.jpg' border='0' height='181' width='550'></a> </p>";
                            else if (DocRemPayment.Text.Equals("Cash Payment"))
                            {
                                emailBody =
                                "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>Dear " + objFactory.Doc.DocBAddrAttn + ",</p> " +
                                "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                                "We will process your order as soon as your payment clears. " + msg1 + ".</p>" +
                                    @"</br> <img src='" + imglink + "' border='0'>" +
                                    "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                                    @"Please click <a href='https://bh-estore.com/netgo_boss/quote/poview/id/" + objFactory.Doc.Custom2 + "'>here</a> to view your order.</p>" +
                                "<p style='padding-top:20px;padding-left:10px;padding-bottom:0px;width:'100%';font:Arial;font-size:10px;>" +
                                "From <b>BH eStore Team</b> of Beng Hui Marine Electrical Pte Ltd</p>" +
                                @"<p style='padding-top:0px;padding-bottom:0px;'>Contact us at <b>+65 6291 4444</b> (Monday to Friday)</p>" +
                                @" </br> <a href='https://bh-estore.com/' target='_blank'><img src='https://bh-estore.com/media/ackemail.jpg' border='0' height='181' width='550'></a> </p>";
                            }
                            else //if (DocRemPayment.Text.Equals("TT"))
                                emailBody =
                                    "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;'>Dear " + objFactory.Doc.DocBAddrAttn + ",</p> " +
                                "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                                "We will process your order as soon as your payment clears. " + msg1 + ".</p>" +
                                    @"<img src='" + imglink + "' border='0'> " +
                                    "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                                "To complete the processing of your order, please proceed for payment <a href='https://bh-estore.com/media/BHM -MB SGD Bank Detail.pdf'>Bank Detail</a>.</p>" +
                                "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                                "Once payment is made, please email the payment slip to <a href='mailto:" + salesemail + "'>" + salesemail + "</a>.</p>" +
                                    "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                                    @"Please click <a href='https://bh-estore.com/netgo_boss/quote/posubmit/id/" + objFactory.Doc.Custom2 + "'>here</a> to view your order.</p>" +
                            "<p style='padding-top:20px;padding-left:10px;padding-bottom:0px;width:'100%';font:Arial;font-size:10px;>" +
                            "From <b>BH eStore Team</b> of Beng Hui Marine Electrical Pte Ltd</p>" +
                            @"<p style='padding-top:0px;padding-bottom:0px;'>Contact us at <b>+65 6291 4444</b> (Monday to Friday)</p>" +
                            @" </br> <a href='https://bh-estore.com/' target='_blank'><img src='https://bh-estore.com/media/ackemail.jpg' border='0' height='181' width='550'></a> </p>";
                        }
                        else if (MagentoVersion == 2)
                        {
                            string Magento2Link = SysOptionUtility.GetStr("Magento2Link");
                            string poviewlink = Magento2Link + "checkout/purchaseorder/process/reference_id/" + objFactory.Doc.Custom2;

                            imglink = Magento2Link + "media/estore/images/email_sales_po_confirm.png";

                            if (!objFactory.Doc.DocQONum.Trim().StartsWith("eQO"))
                                imglink = Magento2Link + "media/estore/images/email_sales_po_direct_confirm.png";

                            if (DocRemPayment.Text.ToLower().Equals("paypal")|| DocRemPayment.Text.ToLower().Equals("stripe"))
                                emailBody =
                                @" </br> <a href='" + Magento2Link + "' target='_blank'>" + "<img src='" + Magento2Link + "media/logo/stores/1/email_logo.png' width = '190' height = '45'></a> </br>" +
                                "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>Dear " + objFactory.Doc.DocBAddrAttn + ",</p> " +
                                "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                                "We will process your order as soon as your payment clears. " + msg1 + ".</p>" +
                                    @"</br> <img src='" + imglink + "' border='0'>" +
                                    "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                                "To complete the processing of your order, please proceed for payment.</p>" +
                                    "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                                    @"Please click <a href='" + poviewlink + "'>here</a> to view your order.</p>" +
                                    @"<p style='padding-top:0px;padding-left:10px;padding-bottom:0px;'>Contact us at <b>+65 6291 4444</b> (Monday to Friday)</p>" +
                                @" <a href='" + Magento2Link + "' target='_blank'><img src='" + Magento2Link + "media/estore/images/email_footer.jpg' border='0' height='181' width='550'></a>" +
                                 "<p style='padding-top:20px;padding-left:10px;padding-bottom:0px;width:'100%';font:Arial;font-size:10px;>" +
                                "From <b>BH eStore Team</b> of Beng Hui Marine Electrical Pte Ltd</p>";
                            else if (DocRemPayment.Text.Equals("Cash Payment"))
                            {
                                emailBody =
                                @" </br> <a href='" + Magento2Link + "' target='_blank'>" + "<img src='" + Magento2Link + "media/logo/stores/1/email_logo.png' width = '190' height = '45'></a> </br>" +
                                "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>Dear " + objFactory.Doc.DocBAddrAttn + ",</p> " +
                                "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                                "We will process your order as soon as your payment clears. " + msg1 + ".</p>" +
                                    @"</br> <img src='" + imglink + "' border='0' >" +
                                    "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                                    @"Please click <a href='" + poviewlink + "'>here</a> to view your order.</p>" +
                                @"<p style='padding-top:0px;padding-left:10px;padding-bottom:0px;'>Contact us at <b>+65 6291 4444</b> (Monday to Friday)</p>" +
                                @"<a href='" + Magento2Link + "' target='_blank'><img src='" + Magento2Link + "media/estore/images/email_footer.jpg' border='0' height='181' width='550'></a>" +
                                "<p style='padding-top:20px;padding-left:10px;padding-bottom:0px;width:'100%';font:Arial;font-size:10px;>" +
                                "From <b>BH eStore Team</b> of Beng Hui Marine Electrical Pte Ltd</p>";
                            }
                            else //if (DocRemPayment.Text.Equals("TT"))
                                emailBody =
                                @" </br> <a href='" + Magento2Link + "' target='_blank'>" + "<img src='" + Magento2Link + "media/logo/stores/1/email_logo.png' width = '190' height = '45'></a> </br>" +
                                    "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;'>Dear " + objFactory.Doc.DocBAddrAttn + ",</p> " +
                                "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                                "We will process your order as soon as your payment clears. " + msg1 + ".</p>" +
                                    @"<img src='" + imglink + "' border='0' > " +
                                    "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                                "To complete the processing of your order, please proceed for payment <a href='" + Magento2Link + "media/estore/pdf/BHM-MB_SGD_Bank_Detail.pdf'>Bank Detail</a>.</p>" +
                                "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                                "Once payment is made, please email the payment slip to <a href='mailto:" + salesemail + "'>" + salesemail + "</a>.</p>" +
                                    "<p style='padding-top:20px;padding-left:10px;width:'100%';font:Arial;font-size:10px;>" +
                                    @"Please click <a href='" + poviewlink + "'>here</a> to view your order.</p>" +
                            @"<p style='padding-top:0px;padding-left:10px;padding-bottom:0px;'>Contact us at <b>+65 6291 4444</b> (Monday to Friday)</p>" +
                            @"<a href='" + Magento2Link + "' target='_blank'><img src='" + Magento2Link + "media/estore/images/email_footer.jpg' border='0' height='181' width='550'></a>" +
                            "<p style='padding-top:20px;padding-left:10px;padding-bottom:0px;width:'100%';font:Arial;font-size:10px;>" +
                           "From <b>BH eStore Team</b> of Beng Hui Marine Electrical Pte Ltd</p>";

                        }

                        GEmail.SendeStoreEmail(objFactory.Doc.DocBAddrEmail, subject, emailBody, null);
                        frmMain.gfrmMain.SetNormalStaus("Ready");
                        MsgBox.Show("Email has been sent to " + objFactory.Doc.DocBAddrAttn);
                    }
                    else
                        MsgBox.Show("This reserve order is not linked to any EStore RFQ.");
                }
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
                        GFunc.ExecuteNonQueryProc("Doc_UpdateEROStatus", parmList);
                        objFactory.Doc.Custom3 = "Replied";
                        objFactory.Doc.IsDirty = false;
                    }
                }
                this.Cursor = Cursors.Default;
            }
        }

     

        private void AuditLogCopyNPost_Click(object sender, EventArgs e)
        {
            objFactory.CopyMyself();
            tsbSave.Enabled = true;
            DocID.Enabled = true;

            //if(objFactory.Doc.IsReadOnly)
            //{
             
               
            //}
            //else
            //{
            //    objFactory.Save((int)4);
                    
            //}
        }
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

        private void chkClearBalance_CustomUpdate(object sender, EventArgs e)
        {
            /*
            decimal? ItmQty = 0;
            decimal? ItmBQty = 0;
            decimal? UOMConRate = 1;
            decimal? ItmDisPercent = 0;
            decimal? ItmQtyLink = 0;
            decimal? ItmQtyAdj = 0;
            int ItmType;
            switch (ItmType)
            {
                case (int)GEnum.INTypeGrp.Stock:
                case (int)GEnum.INTypeGrp.Non_Stock:
                    ItmQty = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQty"].Value, 0), GVar.RndDecs.Qtypt);
                    ItmQtyLink = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQtyLink"].Value, 0), GVar.RndDecs.Qtypt);
                    ItmQtyAdj = GFunc.RndC(GFunc.NEDec(grd.ActiveRow.Cells["ItmQtyAdj"].Value, 0), GVar.RndDecs.Qtypt);
                    grd.ActiveRow.Cells["ItmQtyBalance"].Value = ItmQty - ItmQtyLink - ItmQtyAdj;
                    break;
            }
            */
        }

        private void btnZeroOffBalance_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (DocDetUtil.DetItm_ZeroOffBalance(objFactory.Doc, tagrdDetItms))
                    objFactory.Doc.DocRemAdditional1 = "Clear-BalanceQty";
                else
                {
                    if(objFactory.Doc.IsDirty)
                        objFactory.Doc.DocRemAdditional1 = "";
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

        /* added by YST on 2025/02/25 */
        private void sendToEStoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(objFactory.Doc.DocState >= 100 && objFactory.Doc.DocTypeNm.ToLower().Contains("estore") && objFactory.Doc.DocID != "" && objFactory.Doc.DocKey > 0)
            {
                int MagentoVersion = SysOptionUtility.GetInt("MagentoVersion");
                if (MagentoVersion == 2)
                {
                    try
                    {
                        List<SqlParameter> parlist = new List<SqlParameter>();
                        parlist.Add(new SqlParameter("@DocID", objFactory.Doc.DocID));
                        DataTable dt = GFunc.ExecuteProcReader("Send_eRO_eStore", parlist);
                        if (dt.Rows.Count > 0)
                        {
                            bool originalIsDirtry = objFactory.Doc.IsDirty;
                            objFactory.Doc.DocRef = dt.Rows[0]["estore_reference_no"].ToString();
                            objFactory.Doc.Custom2 = dt.Rows[0]["estore_reference_id"].ToString();
                            objFactory.Doc.IsDirty = originalIsDirtry;
                            EStoreMenuSetting();
                        }
                        if (objFactory.Doc.DocRef != "")
                        {
                            updateEStoreToolStripMenuItem_Click(sender, e);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.ToString().Contains("The data value violated the integrity constraints for the column"))
                        {
                            MsgBox.Show("Customer information is insufficient to send eStore.", GEnum.MsgBoxIcon.Error,GEnum.MsgBoxButton.OK);
                            MsgBox.Show(ex.ToString()
                                .Replace("The OLE DB provider \"MSDASQL\" for linked server \"ESTORE2\" could not INSERT INTO table \"[ESTORE2]...[estore_boss_po]\" ", "")
                                .Replace("The data value violated the integrity constraints for the column", "")
                                );
                        }
                        else
                        {
                            MsgBox.Show("Oops... It could not be sent to eStore.<br/>Please verify with the authorized person.");
                        }
                    }
                }
            }
            else
            {
                MsgBox.Show("Please post the current eRO before sending it to eStore!");
            }
        }
        private void EStoreMenuSetting()
        {
            if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
            {
                bool has_estore_referenceid = false;
                has_estore_referenceid = GFunc.NEStr(objFactory.Doc.Custom2, "").Length > 0; /* select reference_id from ESTORE2...estore_boss_po where ro_no = 'eRO/202502/1004' */
                sendToEStoreToolStripMenuItem.Enabled = (!has_estore_referenceid && objFactory.Doc.DocState == (int)GEnum.DocState.Posted) ;
                updateEStoreToolStripMenuItem.Enabled = has_estore_referenceid;
                checkROInEStoreToolStripMenuItem.Enabled = has_estore_referenceid;
                confirmLinkToCustomerToolStripMenuItem.Enabled = has_estore_referenceid;
                tsmUpdateDOStatus.Enabled = has_estore_referenceid;
            }            
        }
        /*end by YST's update */

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
