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
using Infragistics.Win.UltraWinTabbedMdi;
using System.IO;
using TAUtil;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WinUI
{
    public partial class frmMSTItm : Form
    {
        #region Local Variables

        private BOLib.MSTItmFactory objMstItmFactory = null;
        string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        private GEnum.formInitMode formOpenMode = GEnum.formInitMode.Neither;
        private int recordKey = 0;
        private string recordID = string.Empty;
        private string msgID = string.Empty;
        private bool canEditRecordID = false;
        private DataTable _dtControlStateData = null;
        
        //only use to show Read only data viewer for AuditLog 
        private bool IsOpenFromAuditLog = false;
        private DataTable _dtHeader = null;
        private DataSet _dsDetail = null;

        frmList fMSTItmList = null;
        public GVar.ListEvent_CloseFORM list_CloseMSTForm = null;
        public GVar.ListEvent_RefreshRecord ListEvent_RefreshRecord = null;

        int PreRowIndex = 0;

        private DataTable _dtItmSerial = null;
        private string lastSerialNoSave = string.Empty;
        private string lastMACIDNoSave = string.Empty;
        private string lastBBID = string.Empty;

        DataTable dtDefaultAcc = null;

        #endregion

        //Initialize
        public frmMSTItm()
        {
            InitializeComponent();
        }//Completed
        public frmMSTItm(string itemID, bool EditMode)
        {
            //For call from shortcut menu (Edit/Add)
            //This code is different from other master and reference form as we need to include a EditMode option
            //this is because when shortcut menu popup call, the caller is from a texteditor control which is different from other reference and master (combobox)
            //so because of this difference, we need to specify the add or edit mode
            InitializeComponent();
            recordID = itemID;
            formOpenMode = GEnum.formInitMode.Add;

            if (EditMode)
            {
                MSTItm item = MSTItm.Get(itemID);
                if (GFunc.IsNE(item) == false)
                {
                    if (item.ItmKey > 0)
                    {
                        recordKey = (int)item.ItmKey;
                        formOpenMode = GEnum.formInitMode.Edit;
                    }
                }
            }

            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            tsbList.Enabled = false;
        }//Completed
        public frmMSTItm(int itemKey)
        {
            //For call from shortcut menu (Edit)
            InitializeComponent();
            formOpenMode = GEnum.formInitMode.Edit;
            recordKey = itemKey;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            tsbList.Enabled = false;
        }//Completed
        public frmMSTItm(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            InitializeComponent();
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
        }//Completed

        //Form Events
        private void frmMSTItm_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                // Initialize
                this.objMstItmFactory = new BOLib.MSTItmFactory(BOLib.GEnum.InstanceMode.Normal);
                if (objMstItmFactory.GUID <= 0)
                {
                    formClose = true;
                    return;
                }

                // Attach Event on Factory
                this.objMstItmFactory.ErrorNotifierHeader_Clear += new GVar.UINotifierEvent(this.ErrorNotifier_Clear);
                this.objMstItmFactory.ErrorNotifierHeader_Set += new GVar.UINotifierEvent(this.ErrorNotifier_Set);

                //Check if user has permission to edit Record ID
                canEditRecordID = SECPermUtility.Perform("sysRecID", false);

                //Prepare Control Locking Infor
                ControlLockInfor();

                if (this.IsOpenFromAuditLog)
                {
                    if (objMstItmFactory.SetReadOnlyData(_dtHeader, _dsDetail) == false)
                    {
                        formClose = true;
                        return;
                    }
                    Refresh_All(true);
                    GlobalUI.FormEnable_Set(this, false);
                }
                else
                {

                    this.New_Process();

                    //When open from shortcutmenu (edit)
                    if (formOpenMode == GEnum.formInitMode.Edit)
                        this.OpenRecord(recordKey, recordID);
                    else if (formOpenMode == GEnum.formInitMode.Add)
                    {
                        if (canEditRecordID && recordID != string.Empty)
                            this.ItmID.SetValueTrigger(recordID, false);
                    }
                }
                //Set ContextMenu & Grid Setting & Grid Formatting
                GlobalUI.FormGrids_Set(this, (int)objMstItmFactory.ConstantCodeKey, out ContextMenuSetting);
                // //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objMstItmFactory.ConstantCodeKey);
                GlobalUI.Combos_Fill(this, (int)objMstItmFactory.ConstantCodeKey);

                //we need to set the price list grid column header again here after the function FormGrid_Set()
                //because the grid column would only be ready for column header name change after FormGrid_Set() has been run
                SetPriceGrids();            


               GlobalUI.Ctrl_Update(this, "BranchKey", GEnum.CtlPropertyUpdate.Enabled, SysOptionUtility.UseBranch);

                if (!SECPermUtility.Perform("AbilityToSetPurchaseBlockItem",false))
                    taBlockPurchase.Enabled = false;                

            }
            catch (TAException tex)
            {
                if (tex.MsgID == MsgID.Common.NoMultiInstanceAllowed)
                {
                    this.formClose = true;
                    frmMain.gfrmMain.IsExistingForm(this);
                }
                else
                {
                    this.formClose = true;
                }
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
        private void frmMSTItm_Shown(object sender, EventArgs e)
        {
            if (formClose)
            {
                this.Close();
                return;
            }
            else
                this.ItmID.Focus();

            int colsWidth = 0;
            foreach (UltraGridColumn col in tagrdQtyRatio.DisplayLayout.Bands[0].Columns)
            {
                if (col.Hidden == false)
                {
                    colsWidth = colsWidth + col.Width;
                }
            }
            splitContainer1.SplitterDistance = splitContainer1.Width - colsWidth;
            splitContainer1.FixedPanel = FixedPanel.Panel2;
        }//Completed
        private void frmMSTItm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool runProcess = false;
            this.formClose = true;

            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }
            if (this.formClose && objMstItmFactory == null)
                return;

            try
            {
                #region Closing with Invalid DataType error encountered
                //When the caller performs this.close, the system actually perform validation on all control automatically
                //if there are any control that fails validation (invalid datatype, the e.cancel is set to true, we have no control over this (not sure if this was done by csla)
                //thus we need to check for e.cancel = true so that we can skip the rest of the codes to prevent error message from appearing twice or more
                if (e.Cancel == true)
                {
                    runProcess = true;
                }
                else
                {
                    if (this.SaveChanges() == false)
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
                #endregion

                //Dispose Factory and List Form
                if (!GFunc.IsNE(this.list_CloseMSTForm))
                    list_CloseMSTForm.Invoke();

                if ((bool)this.objMstItmFactory.Dispose() == false)
                    throw new TAException(MsgID.Common.DisposeFail);
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
                    this.objMstItmFactory.Dispose();
            }
        }//Completed
        private void frmMSTItm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                    GlobalUI.Combos_Fill(this, (int)objMstItmFactory.ConstantCodeKey);

                //Set Focus Next Control
                GlobalUI.SelectNextControl(this, e);
            }
            catch (TAException ex)
            {
                Error(ex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed

        //Menu Strip Events
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }//Completed
        private void tsbSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Save_Process();
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
                Delete_Process();
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true); // System Msg
            }
        }//Completed
        private void tsbNew_Click(object sender, EventArgs e)
        {
            try
            {
                this.New_Process();
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
        private void tsbClear_Click(object sender, EventArgs e)
        {
            try
            {
                Clear_Process();
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
        private void tsbList_Click(object sender, EventArgs e)
        {
            try
            {
                if (GFunc.IsNE(fMSTItmList))
                {
                    fMSTItmList = new frmList(objMstItmFactory.ConstantCodeKey, objMstItmFactory.PermID);

                    //Attach events to this FORM to call events in frmList
                    this.list_CloseMSTForm += new GVar.ListEvent_CloseFORM(fMSTItmList.OnCaller_Close);
                    this.ListEvent_RefreshRecord += new GVar.ListEvent_RefreshRecord(fMSTItmList.OnCaller_Changed);

                    //Attach events to frmList to call events in this FORM
                    fMSTItmList.ListEvent_OpenRecord = new GVar.ListEvent_OpenRecord(this.OnListRecordSelected);
                    fMSTItmList.ListEvent_CloseFORM = new GVar.ListEvent_CloseFORM(this.OnList_FormClose);
                    fMSTItmList.MdiParent = frmMain.gfrmMain;
                    fMSTItmList.Show();
                }
                else
                    fMSTItmList.Activate();
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
        private void tsbCreateNewItem_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            try
            {
                if (SaveChanges())
                    if (objMstItmFactory.BuildMasterItem())
                    {
                        frmMSTItmMaster fMstItmPopup = new frmMSTItmMaster(this.objMstItmFactory);
                        fMstItmPopup.ComboSource = MasterItmType.DataSource;
                        fMstItmPopup.DisplayMember = MasterItmType.DisplayMember;
                        fMstItmPopup.ValueMember = MasterItmType.ValueMember;
                        fMstItmPopup.ReturnValue = MasterItmType.Value;
                        fMstItmPopup.Text = MasterItmType.Text;
                        fMstItmPopup.ShowDialog();
                    }
                    else
                    {
                        MsgBox.Show(msgID, GEnum.MsgBoxIcon.Alert, GEnum.MsgBoxButton.OK);

                        this.errorProvider1.SetError(ScaleKey, SysMessageUtility.Get(msgID));
                        tabDetList.Tabs["Measurement"].Selected = true;
                        ScaleKey.Focus();
                        ScaleKey.ReadOnly = false;
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
        private void btnAttachmentEdit_Click(object sender, EventArgs e)
        {
            try
            {
                frmAttachment f = new frmAttachment(objMstItmFactory.ObjMSTItm.Attachments, (int)objMstItmFactory.ConstantCodeKey, objMstItmFactory.ObjMSTItm.ItmKey, -1, 0);
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.Yes)
                {
                    if (objMstItmFactory.ObjMSTItm.INAttachment == false)//To prevent dirty   
                    {
                        INAttachment.Checked = true;
                        objMstItmFactory.ObjMSTItm.INAttachment = true;
                    }
                }
                else
                {
                    if (objMstItmFactory.ObjMSTItm.INAttachment == true)//To prevent dirty   
                    {
                        INAttachment.Checked = false;
                        objMstItmFactory.ObjMSTItm.INAttachment = false;
                    }
                }
                btnAttachmentEdit.Text = "(" + objMstItmFactory.ObjMSTItm.Attachments.Count + ")";
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

        //List form invoke method
        private void OnListRecordSelected(int key)
        {
            //This method will be invoked by list form, when one record is selected from list to edit
            //Also use by OpenID
            try
            {
                if (this.OpenRecord(key, string.Empty))
                    this.Focus();
                else
                    fMSTItmList.Focus();
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
        private void OnList_FormClose()
        {
            fMSTItmList = null;
            this.list_CloseMSTForm = null;
            this.ListEvent_RefreshRecord = null;
        }//Completed

        //Formating, Locking, Refreshing
        private void Refresh_All(bool IncludeDependentCombo)
        {
            try
            {
                Refresh_Header(IncludeDependentCombo);
                Refresh_GridAlternate();
                Refresh_GridLoc();
                Refresh_GridPrice();
                Refresh_GridBatch();
                if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                {
                    Refresh_GridSerial();
                    btnSerialPreview.Enabled = true;
                }
                Refresh_GridAssembly();
                Refresh_GridBOM(string.Empty);
                GridCellDefault_Set();

                //added by thettm on 06 jul 2017(start)
                if (SysOptionUtility.GetBool("UseMappingStockID") == false)
                {
                    MapStockID.Visible = false;
                    MapItmIDLBL.Visible = false;
                    Taxable.Location =new System.Drawing.Point(3, 186); 
                    
                }
                //added by thettm on 06 jul 2017(end)
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
        private void Refresh_Header(bool IncludeDependentCombo)
        {
            try
            {
                bdsItm.DataSource = objMstItmFactory.ObjMSTItm;
                bdsItm.ResetBindings(false);
                if (IncludeDependentCombo)
                    Refresh_DependentText(string.Empty);
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
        private void Refresh_GridAlternate()
        {
            try
            {
                tagrdDetAlternates.DataSource = objMstItmFactory.ObjMSTItmDetAlts;
                tagrdDetAlternates.Rows.Refresh(RefreshRow.ReloadData);
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
        private void Refresh_GridLoc()
        {
            tagrdDetLocation.DataSource = objMstItmFactory.ObjMSTItmDetLocs;
            tagrdDetLocation.Rows.Refresh(RefreshRow.ReloadData);
        }//Completed
        private void Refresh_GridPrice()
        {
            try
            {
                SetPriceGrids();
                LoadPriceList();
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
        private void Refresh_GridBatch()
        {
            tagrdDetBatchs.DataSource = objMstItmFactory.ObjMSTItmBatchs;
            tagrdDetBatchs.Rows.Refresh(RefreshRow.ReloadData);
        }//Completed
        private void Refresh_GridSerial()
        {
            tagrdDetSerials.DataSource = objMstItmFactory.ObjMSTItmSerials;    
            tagrdDetSerials.Rows.Refresh(RefreshRow.ReloadData);
        }//Completed
        private void Refresh_GridAssembly()
        {
            objMstItmFactory.ObjMSTItmDetAsss.DefaultView.Sort = "AssSN";
            tagrdDetAssembly.DataSource = objMstItmFactory.ObjMSTItmDetAsss;
            tagrdDetAssembly.Rows.Refresh(RefreshRow.ReloadData);
        }//Completed
        private void Refresh_GridBOM(string gridName)
        {
            try
            {
                //if gridName = string.Empty (all grid will be refresh) else only that gridname will be refresh
                if (GFunc.CompareString(gridName, "tagrdItmDetBOMRMs") || gridName == string.Empty)
                {
                    tagrdItmDetBOMRMs.DataSource = objMstItmFactory.ObjMSTItmDetBOMRMs;
                    tagrdItmDetBOMRMs.Rows.Refresh(RefreshRow.ReloadData);
                }
                if (GFunc.CompareString(gridName, "tagrdItmDetBOMPMs") || gridName == string.Empty)
                {
                    tagrdItmDetBOMPMs.DataSource = objMstItmFactory.ObjMSTItmDetBOMPMs;
                    tagrdItmDetBOMPMs.Rows.Refresh(RefreshRow.ReloadData);
                }
                if (GFunc.CompareString(gridName, "tagrdItmDetBOMLabours") || gridName == string.Empty)
                {
                    tagrdItmDetBOMLabours.DataSource = objMstItmFactory.ObjMSTItmDetBOMLBs;
                    tagrdItmDetBOMLabours.Rows.Refresh(RefreshRow.ReloadData);
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
        private void Refresh_DependentText(string controlNm)
        {
            //If controlNm is Empty, it will refresh all control, else it will only refresh that control only
            //retain the factory isdirty state as we do not want to change due to propertychange event
            bool FactoryIsDirty = objMstItmFactory.IsDirty;
            try
            {
                MSTAcc objAcc;
                MSTItm objMstItm;

                #region Income Account
                if (GFunc.CompareString(controlNm, "AccICNm") || controlNm == string.Empty)
                {
                    if (GFunc.IsNE(objMstItmFactory.ObjMSTItm.AccICKey) == false)
                    {
                        objAcc = MSTAcc.Get(objMstItmFactory.ObjMSTItm.AccICKey);
                        AccICNm.SetValueTrigger(objAcc.AccDes, false);
                        objMstItmFactory.ObjMSTItm.AccICID = objAcc.AccID;
                        objAcc = null;
                    }
                    else
                    {
                        AccICNm.SetValueTrigger(string.Empty, false);
                    }
                }
                #endregion

                #region Inventory Account
                if (GFunc.CompareString(controlNm, "AccINNm") || controlNm == string.Empty)
                {
                    if (GFunc.IsNE(objMstItmFactory.ObjMSTItm.AccINKey) == false)
                    {
                        objAcc = MSTAcc.Get(objMstItmFactory.ObjMSTItm.AccINKey);
                        AccINNm.SetValueTrigger(objAcc.AccDes, false);
                        objMstItmFactory.ObjMSTItm.AccINID = objAcc.AccID;
                        objAcc = null;
                    }
                    else
                    {
                        AccINNm.SetValueTrigger(string.Empty, false);
                    }
                }
                #endregion

                #region Purchase Account
                if (GFunc.CompareString(controlNm, "AccPHNm") || controlNm == string.Empty)
                {
                    if (GFunc.IsNE(objMstItmFactory.ObjMSTItm.AccPHKey) == false)
                    {
                        objAcc = MSTAcc.Get(objMstItmFactory.ObjMSTItm.AccPHKey);
                        AccPHNm.SetValueTrigger(objAcc.AccDes, false);
                        objMstItmFactory.ObjMSTItm.AccPHID = objAcc.AccID;
                        objAcc = null;
                    }
                    else
                    {
                        AccPHNm.SetValueTrigger(string.Empty, false);
                    }
                }
                #endregion

                if (GFunc.CompareString(controlNm, "AccDSICDes") || controlNm == string.Empty)
                {
                    if (GFunc.IsNE(objMstItmFactory.ObjMSTItm.AccDSICKey) == false)
                    {
                        objAcc = MSTAcc.Get(objMstItmFactory.ObjMSTItm.AccDSICKey);
                        AccDSICDes.SetValueTrigger(objAcc.AccDes, false);
                        objMstItmFactory.ObjMSTItm.AccDSICID = objAcc.AccID;
                        objAcc = null;
                    }
                    else
                    {
                        AccDSICDes.SetValueTrigger(string.Empty, false);
                    }
                }
                if (GFunc.CompareString(controlNm, "AccDSPHDes") || controlNm == string.Empty)
                {
                    if (GFunc.IsNE(objMstItmFactory.ObjMSTItm.AccDSPHKey) == false)
                    {
                        objAcc = MSTAcc.Get(objMstItmFactory.ObjMSTItm.AccDSPHKey);
                        AccDSPHDes.SetValueTrigger(objAcc.AccDes, false);
                        objMstItmFactory.ObjMSTItm.AccDSPHID = objAcc.AccID;
                        objAcc = null;
                    }
                    else
                    {
                        AccDSPHDes.SetValueTrigger(string.Empty, false);
                    }
                }

                #region Vendor Key
                if (GFunc.CompareString(controlNm, "CSGVendorNm") || controlNm == string.Empty)
                {
                    if (GFunc.IsNE(objMstItmFactory.ObjMSTItm.CSGVendorKey) == false)
                    {
                        MSTCon objVendor = MSTCon.Get(objMstItmFactory.ObjMSTItm.CSGVendorKey);
                        CSGVendorNm.SetValueTrigger(objVendor.ConNm, false);
                        objVendor = null;
                    }
                    else
                    {
                        CSGVendorNm.SetValueTrigger(string.Empty, false);
                    }
                }
                #endregion

                #region Master Item
                if (GFunc.CompareString(controlNm, "MasterItmID") || GFunc.CompareString(controlNm, "MasterItmDes") || controlNm == string.Empty)
                {
                    if (GFunc.IsNE(objMstItmFactory.ObjMSTItm.MasterItmKey) == false)
                    {
                        objMstItm = MSTItm.Get(objMstItmFactory.ObjMSTItm.MasterItmKey);
                        MasterItmID.SetValueTrigger(objMstItm.ItmID, false);
                        MasterItmDesc.SetValueTrigger(objMstItm.ItmDes, false);
                        objMstItm = null;
                    }
                    else
                    {
                        MasterItmID.SetValueTrigger(string.Empty, false);
                        MasterItmDesc.SetValueTrigger(string.Empty, false);
                    }
                }
                #endregion

                #region Substitute Item
                if (GFunc.CompareString(controlNm, "SubstituteItmID") || GFunc.CompareString(controlNm, "SubstituteItmDesc") || controlNm == string.Empty)
                {
                    if (GFunc.IsNE(objMstItmFactory.ObjMSTItm.SubstituteItmKey) == false)
                    {
                        objMstItm = MSTItm.Get(objMstItmFactory.ObjMSTItm.SubstituteItmKey);
                        SubstituteItmID.SetValueTrigger(objMstItm.ItmID, false);
                        SubstituteItmDesc.SetValueTrigger(objMstItm.ItmDes, false);
                        objMstItm = null;
                    }
                    else
                    {
                        SubstituteItmID.SetValueTrigger(string.Empty, false);
                        SubstituteItmDesc.SetValueTrigger(string.Empty, false);
                    }
                }
                #endregion

                objMstItmFactory.IsDirty = FactoryIsDirty;
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
        private void FormLayout()
        {
            try
            {

                bool ReadOnlyMode = this.objMstItmFactory.IsReadOnly;
                bool EnableMode = !ReadOnlyMode;
                this.tslReadOnly.Text = ReadOnlyMode ? "Read Only" : string.Empty;
                
                GlobalUI.FreezeControl(this.Handle, false);

                ControlLock();

                //added by thettm on 30 Aug 2018 (start)
                if (objMstItmFactory.ObjMSTItm.ItmType == 250)
                    SettingForAssembly();
                else if (objMstItmFactory.ObjMSTItm.ItmType == (int)GEnum.ItemType.Charges) /* added by YST on 2021/08/11 */
                {
                    lblChargesType.Visible = true;
                    ItmChargesType.Visible = true;
                }
                else
                {
                    lblChargesType.Visible = false;
                    ItmChargesType.Visible = false;
                    groupBox1.Visible = false;
                    INClass.Enabled = true;
                }
                //added by thettm on 30 Aug 2018 (end)

                FormLayoutMaster();

                //To restrict user from editing BOM infor if the user do not have the access rights
                if (objMstItmFactory.ObjMSTItm.ItmType == (int)GEnum.ItemType.Finished_GD
                    || objMstItmFactory.ObjMSTItm.ItmType == (int)GEnum.ItemType.Finished_GDB
                    || objMstItmFactory.ObjMSTItm.ItmType == (int)GEnum.ItemType.Serial_Finished_GDB)
                {
                    //Set False to everything first
                    BOMType.ReadOnly = true;
                    BOMOverHeadKey.ReadOnly = true;
                    BOMMultiplier.ReadOnly = true;
                    tagrdDetAlternates.DisplayLayout.Bands[0].Override.AllowAddNew = AllowAddNew.No;
                    tagrdDetAlternates.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.False;
                    tagrdDetAlternates.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                    //Check if user has permission to edit BOM tab
                    if (SECPermUtility.Add(GVar.PermissionID.Edit_Item_BOM, true) )
                    {
                        BOMType.ReadOnly = false;
                        BOMOverHeadKey.ReadOnly = false;
                        BOMMultiplier.ReadOnly = false;
                        tagrdDetAlternates.DisplayLayout.Bands[0].Override.AllowAddNew = AllowAddNew.TemplateOnBottom;
                    }
                    if (SECPermUtility.Edit(GVar.PermissionID.Edit_Item_BOM, true))
                    {
                        BOMType.ReadOnly = false;
                        BOMOverHeadKey.ReadOnly = false;
                        BOMMultiplier.ReadOnly = false;
                        tagrdDetAlternates.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.True;
                    }
                    if (SECPermUtility.Delete(GVar.PermissionID.Edit_Item_BOM, true) )
                    {
                        tagrdDetAlternates.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.True;
                    }
                }

                if (ReadOnlyMode)
                {
                    this.tsbSave.Enabled = false;
                    this.tsbDelete.Enabled = false;
                    this.tsbClear.Enabled = false;
                    this.tsbCreateNewItem.Enabled = false;

                    this.btnSerialGenerate.Enabled = false;
                    this.btnSerialSave.Enabled = false;
                    this.btnSerialSaveOne.Enabled = false;
                }
                else
                {
                    this.tsbSave.Enabled = true;
                    
                    if (objMstItmFactory.ObjMSTItm.ItmType == (int?)GEnum.ItemType.Master)
                        this.tsbCreateNewItem.Enabled = true;
                    else
                        this.tsbCreateNewItem.Enabled = false;

                    if (this.objMstItmFactory.IsNew)
                    {
                        this.tsbClear.Enabled = true;
                        this.tsbDelete.Enabled = false;
                    }
                    else
                    {
                        this.tsbClear.Enabled = false;
                        this.tsbDelete.Enabled = true;
                    }

                    //Check if user has permission to edit Record ID
                    if (canEditRecordID && ReadOnlyMode == false)
                        ItmID.ReadOnly = false;
                    else
                        ItmID.ReadOnly = true;

                    this.btnSerialGenerate.Enabled = true;
                    this.btnSerialSave.Enabled = false;
                    this.btnSerialSaveOne.Enabled = true;

                }

                if (GFunc.IsNEZ(this.objMstItmFactory.ObjMSTItm.SubstituteItmKey.Value))
                    this.tabDetList.Visible = true;
                else
                    this.tabDetList.Visible = false;


                // Set Serial grid
                if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                {
              
                    tagrdDetSerials.DisplayLayout.Bands[0].Columns["SerialID"].SortIndicator = SortIndicator.Ascending;
                    tagrdDetSerials.DisplayLayout.Bands[0].Columns["SerialID"].CellClickAction = CellClickAction.CellSelect;
                    if (tagrdDetSerials.DisplayLayout.Bands[0].Columns.Count>15)
                    {
                        tagrdDetSerials.DisplayLayout.Bands[0].Columns["BatchID"].CellClickAction = CellClickAction.CellSelect;
                        tagrdDetSerials.DisplayLayout.Bands[0].Columns["MACAddress"].CellClickAction = CellClickAction.CellSelect;
                    }
                    tagrdDetSerials.DisplayLayout.Bands[0].Columns["MfgDate"].CellClickAction = CellClickAction.CellSelect;
                    tagrdDetSerials.DisplayLayout.Bands[0].Columns["ItmStatus"].CellClickAction = CellClickAction.EditAndSelectText;
                    tagrdDetSerials.DisplayLayout.Bands[0].Columns["Custom1"].CellClickAction = CellClickAction.EditAndSelectText;
                    tagrdDetSerials.DisplayLayout.Bands[0].Columns["Custom2"].CellClickAction = CellClickAction.EditAndSelectText;
                    tagrdDetSerials.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.True;

                    if (tagrdDetSerials.Rows.Count > 0)
                    {
                        for (int i = 0; i < tagrdDetSerials.Rows.Count; i++)
                        {
                            tagrdDetSerials.Rows[i].Cells["ItmStatus"].Activation = Activation.AllowEdit;
                            tagrdDetSerials.Rows[i].Cells["Custom1"].Activation = Activation.AllowEdit;
                            tagrdDetSerials.Rows[i].Cells["Custom2"].Activation = Activation.AllowEdit;
                            tagrdDetSerials.Rows[i].Cells["SerialID"].Activation = Activation.NoEdit;
                            tagrdDetSerials.Rows[i].Cells["BatchID"].Activation = Activation.NoEdit;
                            tagrdDetSerials.Rows[i].Cells["MACAddress"].Activation = Activation.NoEdit;                            
                        }
                    }

                    MFNNumber.SetValueTrigger(string.Empty, false);
                    QtyToGenerate.SetValueTrigger(string.Empty, false);
                    MFNNo.SetValueTrigger(string.Empty, false);
                    SerialNo.SetValueTrigger(string.Empty, false);
                    MACID.SetValueTrigger(string.Empty, false);
                    BBID.SetValueTrigger(string.Empty, false);
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
                GlobalUI.FreezeControl(this.Handle, true);
                this.Refresh();
            }

        }//Completed
        private void FormLayoutMaster()
        {
            try
            {
                if (GFunc.NEInt(this.ItmType.Value, 0) == (int)GEnum.ItemType.Master)
                {


                    this.ControlLock_Set(this.MasterItmType, "N"); this.ControlLock_Set(this.MasterItmTypeLabel, "N");
                    this.ControlLock_Set(this.SubstituteItmID, ""); this.ControlLock_Set(this.SubstituteItmIDLabel, "");
                    this.ControlLock_Set(this.SubstituteItmDesc, ""); this.ControlLock_Set(this.SubstituteItmDescLabel, "");
                    this.ControlLock_Set(this.CSGVendorKey, "A"); this.ControlLock_Set(this.CSGVendorKeyLabel, "A");
                    this.ControlLock_Set(this.CSGVendorNm, "A"); this.ControlLock_Set(this.CSGVendorNmLabel, "A");
                    this.ControlLock_Set(this.SKU1, ""); this.ControlLock_Set(this.lblChargesType, "");
                    this.ControlLock_Set(this.SKU2, ""); this.ControlLock_Set(this.SKU2Label, "");
                    this.ControlLock_Set(this.CostMethod, "N"); this.ControlLock_Set(this.CostMethodLabel, "N");
                    this.ControlLock_Set(this.AccINKey, "A"); this.ControlLock_Set(this.AccINKeyLabel, "A");
                    this.ControlLock_Set(this.AccINNm, "A"); this.ControlLock_Set(this.AccINNmLabel, "A");
                    this.ControlLock_Set(this.AccPHKey, "A"); this.ControlLock_Set(this.AccPHKeyLabel, "A");
                    this.ControlLock_Set(this.AccPHNm, "A"); this.ControlLock_Set(this.AccPHNmLabel, "A");
                    this.ControlLock_Set(this.QtyStock, ""); this.ControlLock_Set(this.QtyStockLabel, "");
                    this.ControlLock_Set(this.QtyMin, "A"); this.ControlLock_Set(this.QtyMinLabel, "A");
                    this.ControlLock_Set(this.QtyMax, "A"); this.ControlLock_Set(this.QtyMaxLabel, "A");
                    this.ControlLock_Set(this.DefLocSale, "A"); this.ControlLock_Set(this.DefLocSaleLabel, "A");
                    this.ControlLock_Set(this.DefLocPurchase, "A"); this.ControlLock_Set(this.DefLocPurchaseLabel, "A");
                    this.ControlLock_Set(this.CostAvg, ""); this.ControlLock_Set(this.CostAvgLabel, "");
                    this.ControlLock_Set(this.OpenBalCost, ""); this.ControlLock_Set(this.OpenBalCostLabel, "");
                    this.ControlLock_Set(this.OpenBalQty, ""); this.ControlLock_Set(this.OpenBalQtyLabel, "");
                    this.ControlLock_Set(this.OpenBalAmtH, ""); this.ControlLock_Set(this.OpenBalAmtHLabel, "");
                    this.ControlLock_Set(this.ColorKey, ""); this.ControlLock_Set(this.ColorKeyLabel, "");
                    this.ControlLock_Set(this.ScaleKey, "N"); this.ControlLock_Set(this.ScaleKeyLabel, "N");
                    this.ControlLock_Set(this.ScaleSize, ""); this.ControlLock_Set(this.ScaleSizeLabel, "");
                    this.ControlLock_Set(this.SaleUOM, ""); this.ControlLock_Set(this.SaleUOMLabel, "");
                    this.ControlLock_Set(this.SaleUOMRate, ""); this.ControlLock_Set(this.SaleUOMRateLabel, "");
                    this.ControlLock_Set(this.PurchaseUOM, ""); this.ControlLock_Set(this.PurchaseUOMLabel, "");
                    this.ControlLock_Set(this.PurchaseUOMRate, ""); this.ControlLock_Set(this.PurchaseUOMRateLabel, "");  
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
        }
        private void ControlLock()
        {
            string ItemType = string.Empty;
            try
            {
                if (GFunc.NEInt(this.ItmType.Value, 0) == (int)GEnum.ItemType.Master)
                {
                    if (GFunc.NEInt(this.MasterItmType.Value, 0) == 0)
                    {
                        ItemType = GFunc.NEInt(this.ItmType.Value, 0).ToString();
                    }
                    else
                    {
                        ItemType = GFunc.NEInt(this.MasterItmType.Value, 100).ToString();
                    }

                }
                else
                {
                    ItemType = GFunc.NEInt(this.ItmType.Value, 0).ToString();
                }
                string RelatedControlNm = string.Empty;

                if (GFunc.IsNE(ItemType) == false)
                {
                    if (_dtControlStateData.Columns.Contains(ItemType))
                    {
                        foreach (DataRow dr in _dtControlStateData.Rows)
                        {
                            //Update Input Control's Label
                            Control[] objs = this.Controls.Find(dr["FldName"].ToString() + "Label", true);
                            if (objs.Count() > 0)
                            {
                                this.ControlLock_Set(objs[0], dr[ItemType].ToString());
                            }
                            //Update Input Control
                            objs = this.Controls.Find(dr["FldName"].ToString(), true);
                            if (objs.Count() > 0)
                            {
                                this.ControlLock_Set(objs[0], dr[ItemType].ToString());
                            }
                        }
                    }
                }

                this.Refresh();
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
        private void ControlLockInfor()
        {
            //this data generate from \\TA-NAS01\Share\Boss C#\Documentation\Master\MSTItm.xls
            List<string[]> stateDataList = new List<string[]>();
            stateDataList.Add(new string[] { "ItmType", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" });
            stateDataList.Add(new string[] { "MasterItmID", "R", "", "", "", "", "", "", "", "R", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "MasterItmDesc", "R", "", "", "", "", "", "", "", "R", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "MasterItmType", "", "", "", "", "", "", "", "N", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "SubstituteItmID", "N", "N", "N", "N", "N", "N", "N", "", "N", "N", "N", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "SubstituteItmDesc", "N", "N", "N", "N", "N", "N", "N", "", "N", "N", "N", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "ItmID", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A" });
            stateDataList.Add(new string[] { "ItmDes", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A" });
            stateDataList.Add(new string[] { "ItmRem", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A" });
            stateDataList.Add(new string[] { "AccessLevel", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R" });
            stateDataList.Add(new string[] { "AccessSecGrpKey", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R" });
            stateDataList.Add(new string[] { "CSGVendorKey", "A", "A", "A", "A", "A", "A", "A", "A", "N", "A", "A", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "CSGVendorNm", "A", "A", "A", "A", "A", "A", "A", "A", "N", "A", "A", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "IndustryPN", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "SKU1", "A", "A", "A", "A", "A", "A", "A", "", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A" });
            stateDataList.Add(new string[] { "SKU2", "A", "A", "A", "A", "A", "A", "A", "", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A" });
            stateDataList.Add(new string[] { "CatKey1", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "" });
            stateDataList.Add(new string[] { "CatKey2", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "" });
            stateDataList.Add(new string[] { "CatKey3", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "" });
            stateDataList.Add(new string[] { "CatKey4", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "" });
            stateDataList.Add(new string[] { "CatKey5", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "" });
            stateDataList.Add(new string[] { "Brandkey", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "Model", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "INClass", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "" });
            stateDataList.Add(new string[] { "Inactive", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A" });
            stateDataList.Add(new string[] { "CostMethod", "N", "R", "N", "R", "", "N", "N", "N", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "BranchKey", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "" });
            stateDataList.Add(new string[] { "DeptKey", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "" });
            stateDataList.Add(new string[] { "AccICKey", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "" });
            stateDataList.Add(new string[] { "AccICNm", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "" });
            stateDataList.Add(new string[] { "AccINKey", "A", "A", "A", "A", "", "A", "A", "A", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "AccINNm", "A", "A", "A", "A", "", "A", "A", "A", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "AccPHKey", "A", "A", "A", "A", "A", "A", "A", "A", "", "A", "A", "A", "A", "", "", "", "", "" });
            stateDataList.Add(new string[] { "AccPHNm", "A", "A", "A", "A", "A", "A", "A", "A", "", "A", "A", "A", "A", "", "", "", "", "" });
            stateDataList.Add(new string[] { "BUOMKey", "N", "N", "N", "N", "A", "N", "N", "N", "N", "A", "A", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "QtyStock", "R", "R", "R", "R", "", "R", "R", "", "R", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "QtyMin", "A", "A", "A", "A", "", "A", "A", "A", "A", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "QtyMax", "A", "A", "A", "A", "", "A", "A", "A", "A", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "QtyReOrder", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "LeadTimeInDays", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "DefLocSale", "A", "A", "A", "A", "", "A", "A", "A", "A", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "DefLocPurchase", "A", "A", "A", "A", "", "A", "A", "A", "A", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "CostLatest", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "CostLatestDate", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "CostLanded", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "CostLandedDate", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "CostAvg", "R", "R", "R", "R", "", "R", "R", "", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "ControlPriceH", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "OpenBalCost", "R", "R", "R", "R", "", "R", "R", "", "R", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "OpenBalQty", "R", "R", "R", "R", "", "R", "R", "", "R", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "OpenBalAmtH", "R", "R", "R", "R", "", "R", "R", "", "R", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "Taxable", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "" });
            stateDataList.Add(new string[] { "BOMType", "", "", "N", "N", "", "", "N", "", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "BOMMultiplier", "", "", "A", "A", "", "", "A", "", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "BOMOverHeadKey", "", "", "A", "A", "", "", "A", "", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "DefaultExpDate", "", "A", "", "A", "", "A", "A", "", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "DefaultExpDateTx", "", "A", "", "A", "", "A", "A", "", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "ColorKey", "A", "A", "A", "A", "A", "A", "A", "", "A", "A", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "ScaleKey", "R", "R", "R", "R", "R", "R", "R", "N", "R", "R", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "ScaleSizeNum", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "ScaleSize", "R", "R", "R", "R", "R", "R", "R", "", "R", "R", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "WeightNet", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "WeightGross", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "WeightUOMKey", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "INLength", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "INWidth", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "INHeight", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "INVolume", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "INPacking", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "INAttachment", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "btnAttachmentEdit", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "StdPackSize", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "StdPackWeight", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "StdPackLength", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "StdPackWidth", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "StdPackHeight", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "SaleUOM", "A", "A", "A", "A", "A", "A", "A", "", "A", "A", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "SaleUOMRate", "A", "A", "A", "A", "A", "A", "A", "", "A", "A", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "PurchaseUOM", "A", "A", "A", "A", "A", "A", "A", "", "A", "A", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "PurchaseUOMRate", "A", "A", "A", "A", "A", "A", "A", "", "A", "A", "", "", "", "", "", "", "", "" });
            if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
            {
                stateDataList.Add(new string[] { "Custom1", "A", "A", "A", "A", "R", "A", "A", "R", "R", "R", "R", "R", "R", "", "", "", "", "" });
                stateDataList.Add(new string[] { "Custom2", "A", "A", "A", "A", "R", "A", "A", "R", "R", "R", "R", "R", "R", "", "", "", "", "" });
                stateDataList.Add(new string[] { "Custom3", "A", "A", "A", "A", "R", "A", "A", "R", "R", "R", "R", "R", "R", "", "", "", "", "" });
                stateDataList.Add(new string[] { "Custom4", "A", "A", "A", "A", "R", "A", "A", "R", "R", "R", "R", "R", "R", "", "", "", "", "" });
                stateDataList.Add(new string[] { "Custom5", "A", "A", "A", "A", "R", "A", "A", "R", "R", "R", "R", "R", "R", "", "", "", "", "" });
                stateDataList.Add(new string[] { "Custom6", "A", "A", "A", "A", "R", "A", "A", "R", "R", "R", "R", "R", "R", "", "", "", "", "" });
                stateDataList.Add(new string[] { "Custom7", "A", "A", "A", "A", "R", "A", "A", "R", "R", "R", "R", "R", "R", "", "", "", "", "" });
                stateDataList.Add(new string[] { "Custom8", "A", "A", "A", "A", "R", "A", "A", "R", "R", "R", "R", "R", "R", "", "", "", "", "" });
            }
            else
            {
                stateDataList.Add(new string[] { "Custom1", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "", "", "", "", "" });
                stateDataList.Add(new string[] { "Custom2", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "", "", "", "", "" });
                stateDataList.Add(new string[] { "Custom3", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "", "", "", "", "" });
                stateDataList.Add(new string[] { "Custom4", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "", "", "", "", "" });
                stateDataList.Add(new string[] { "Custom5", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "", "", "", "", "" });
                stateDataList.Add(new string[] { "Custom6", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "", "", "", "", "" });
                stateDataList.Add(new string[] { "Custom7", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "", "", "", "", "" });
                stateDataList.Add(new string[] { "Custom8", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "R", "", "", "", "", "" });
            }
                        
            stateDataList.Add(new string[] { "Custom9", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "" });
            stateDataList.Add(new string[] { "Custom10", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "" });

            stateDataList.Add(new string[] { "tabPageAlternate", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "tagrdDetAlternates", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "tabPageAssembly", "", "", "", "", "A", "", "", "", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "tagrdDetAssembly", "", "", "", "", "A", "", "", "", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "tabPageBOM", "", "", "A", "A", "", "", "A", "", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "tagrdItmDetBOMLabours", "", "", "A", "A", "", "", "A", "", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "tagrdItmDetBOMPMs", "", "", "A", "A", "", "", "A", "", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "tagrdItmDetBOMRMs", "", "", "A", "A", "", "", "A", "", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "tabPageLocation", "A", "A", "A", "A", "", "A", "A", "A", "A", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "tagrdDetLocation", "A", "A", "A", "A", "", "A", "A", "A", "A", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "tabPagePriceList", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "tagrdPriceList", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "tabPageBatch", "", "A", "", "A", "", "A", "A", "", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "tagrdDetBatchs", "", "A", "", "A", "", "A", "A", "", "", "", "", "", "", "", "", "", "", "" });
            stateDataList.Add(new string[] { "tabDetList", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "", "", "", "", "", "" });
            if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
            {
                stateDataList.Add(new string[] { "tabPageSerial", "A", "A", "A", "A", "", "A", "A", "", "", "", "", "", "", "", "", "", "", "" });
            }
            else
            {
                stateDataList.Add(new string[] { "tabPageSerial", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" });
            }

            _dtControlStateData = new DataTable();
            _dtControlStateData.Columns.Add("FldName", typeof(string));
            _dtControlStateData.Columns.Add(((int)GEnum.ItemType.Stock).ToString(), typeof(string));
            _dtControlStateData.Columns.Add(((int)GEnum.ItemType.StockB).ToString(), typeof(string));
            _dtControlStateData.Columns.Add(((int)GEnum.ItemType.Finished_GD).ToString(), typeof(string));
            _dtControlStateData.Columns.Add(((int)GEnum.ItemType.Finished_GDB).ToString(), typeof(string));
            _dtControlStateData.Columns.Add(((int)GEnum.ItemType.Assembly).ToString(), typeof(string));
            _dtControlStateData.Columns.Add(((int)GEnum.ItemType.Serial_StockB).ToString(), typeof(string));
            _dtControlStateData.Columns.Add(((int)GEnum.ItemType.Serial_Finished_GDB).ToString(), typeof(string));
            _dtControlStateData.Columns.Add(((int)GEnum.ItemType.Master).ToString(), typeof(string));
            _dtControlStateData.Columns.Add(((int)GEnum.ItemType.Consignment).ToString(), typeof(string));
            _dtControlStateData.Columns.Add(((int)GEnum.ItemType.Non_Stock).ToString(), typeof(string));
            _dtControlStateData.Columns.Add(((int)GEnum.ItemType.Service).ToString(), typeof(string));
            _dtControlStateData.Columns.Add(((int)GEnum.ItemType.Charges).ToString(), typeof(string));
            _dtControlStateData.Columns.Add(((int)GEnum.ItemType.Discount).ToString(), typeof(string));
            _dtControlStateData.Columns.Add(((int)GEnum.ItemType.Header).ToString(), typeof(string));
            _dtControlStateData.Columns.Add(((int)GEnum.ItemType.Remark).ToString(), typeof(string));
            _dtControlStateData.Columns.Add(((int)GEnum.ItemType.Sub_Total).ToString(), typeof(string));
            _dtControlStateData.Columns.Add(((int)GEnum.ItemType.BF_Total).ToString(), typeof(string));
            _dtControlStateData.Columns.Add(((int)GEnum.ItemType.Total).ToString(), typeof(string));

            foreach (string[] strData in stateDataList)
            {
                DataRow drNew = _dtControlStateData.NewRow();
                foreach (DataColumn dc in _dtControlStateData.Columns)
                {
                    drNew[dc] = strData[dc.Ordinal];
                }
                _dtControlStateData.Rows.Add(drNew);
            }
        }//Completed
        private void ControlLock_Set(Control objControl, string controlState)
        {
            try
            {

                //To be remove if this is really a incorrect statement
                //this line should be incorrect as we should disable masterItmType when open as existing master record
                //if (GFunc.NEInt(this.ItmType.Value, 0) == (int)GEnum.ItemType.Master && (objControl.Name.Contains(this.MasterItmType.Name) || objControl.Name.Contains("Scale")))
                //{

                //    objControl.Visible = true;
                //    objControl.Enabled = true;
                //    return;
                //}
                GlobalUI.ControlReadOnly_Set(objControl, false);
                #region Blank
                if (GFunc.CompareString(GVar.ControlState.BLANK, controlState))
                {
                    if (objControl.GetType() == typeof(Infragistics.Win.UltraWinTabControl.UltraTabPageControl))
                        ((Infragistics.Win.UltraWinTabControl.UltraTabControl)objControl.Parent).Tabs[objControl.Name].Enabled = false;
                    if (objControl.GetType() == typeof(Infragistics.Win.UltraWinTabControl.UltraTabControl))
                        objControl.Enabled = false;
                    else
                        objControl.Visible = false;
                }
                #endregion

                #region A //editable regardless of IsNew state

                else if (GFunc.CompareString(GVar.ControlState.A, controlState))     //editable regardless of IsNew state           
                {
                    if (objControl.GetType() == typeof(Infragistics.Win.UltraWinTabControl.UltraTabPageControl))
                        ((Infragistics.Win.UltraWinTabControl.UltraTabControl)objControl.Parent).Tabs[objControl.Name].Enabled = true;
                    if (objControl.GetType() == typeof(Infragistics.Win.UltraWinTabControl.UltraTabControl))
                        objControl.Enabled = true;
                    else
                    {
                        objControl.Visible = true;
                        if (objMstItmFactory.IsReadOnly || objMstItmFactory.ObjMSTItm.SubstituteItmKey > 0)
                        {
                            if (objControl.GetType() == typeof(TAUtil.TAGridEditor))
                                GridLock(((TAUtil.TAGridEditor)objControl), true);
                            else
                            {
                                if (objMstItmFactory.ObjMSTItm.SubstituteItmKey > 0)
                                {
                                    switch (objControl.Name.ToLower())
                                    {
                                        case "itmid":
                                        case "itmdes":
                                        case "itmrem":
                                        case "industrypn":
                                        case "sku1":
                                        case "sku2":
                                        case "itmidlabel":
                                        case "itmdeslabel":
                                        case "itmremlabel":
                                        case "industrypnlabel":
                                        case "sku1label":
                                        case "sku2label":
                                            objControl.Enabled = true;
                                            break;

                                        default:
                                            GlobalUI.ControlReadOnly_Set(objControl, true);
                                            break;
                                    }
                                }
                                else
                                {
                                    GlobalUI.ControlReadOnly_Set(objControl, true);
                                    //objControl.Enabled = false;
                                }
                            }
                        }
                        else
                        {
                            if (objControl.GetType() == typeof(TAUtil.TAGridEditor))
                                GridLock(((TAUtil.TAGridEditor)objControl), false);
                            else
                                objControl.Enabled = true;
                        }
                    }
                }
                #endregion

                #region N //Editable only when IsNew (true)
                else if (GFunc.CompareString(GVar.ControlState.N, controlState))//Editable only when IsNew (true)
                {
                    if (objControl.GetType() == typeof(Infragistics.Win.UltraWinTabControl.UltraTabPageControl))
                        ((Infragistics.Win.UltraWinTabControl.UltraTabControl)objControl.Parent).Tabs[objControl.Name].Enabled = true;
                    else
                    {
                        objControl.Visible = true;
                        if (objMstItmFactory.IsReadOnly || (objMstItmFactory.IsNew == false) || objMstItmFactory.ObjMSTItm.SubstituteItmKey > 0)
                        {
                            if (objControl.GetType() == typeof(TAUtil.TAGridEditor))
                                GridLock(((TAUtil.TAGridEditor)objControl), true);
                            else
                            {
                                if (objMstItmFactory.ObjMSTItm.SubstituteItmKey > 0)
                                {
                                    switch (objControl.Name.ToLower())
                                    {
                                        case "itmid":
                                        case "itmdes":
                                        case "itmrem":
                                        case "industrypn":
                                        case "sku1":
                                        case "sku2":
                                        case "itmidlabel":
                                        case "itmdeslabel":
                                        case "itmremlabel":
                                        case "industrypnlabel":
                                        case "sku1label":
                                        case "sku2label":
                                            objControl.Enabled = true;
                                            break;

                                        default:
                                            objControl.Enabled = false;
                                            break;
                                    }
                                }
                                else
                                    objControl.Enabled = false;
                            }
                        }
                        else
                        {
                            if (objControl.GetType() == typeof(TAUtil.TAGridEditor))
                                GridLock(((TAUtil.TAGridEditor)objControl), false);
                            else
                                objControl.Enabled = true;
                        }
                    }

                }
                #endregion

                #region R //Always Readonly
                else if (GFunc.CompareString(GVar.ControlState.R, controlState))   //Always Readonly
                {
                    objControl.Visible = true;
                    objControl.Enabled = false;
                }

                #endregion

                #region RightButton

                if (objControl.GetType() == typeof(TAUtil.TATextBoxEditor))
                {
                    if (((TAUtil.TATextBoxEditor)objControl).ButtonsRight != null && ((TAUtil.TATextBoxEditor)objControl).ButtonsRight.Count > 0)
                    {
                        if (objControl.Enabled == false)
                            ((TAUtil.TATextBoxEditor)objControl).ButtonsRight[0].Enabled = false;
                        else
                            ((TAUtil.TATextBoxEditor)objControl).ButtonsRight[0].Enabled = true;

                        //for readOnly
                        ((TAUtil.TATextBoxEditor)objControl).ButtonsRight[0].Enabled = !((TAUtil.TATextBoxEditor)objControl).ReadOnly;
                    }
                }

                if (objControl.GetType() == typeof(TAUtil.TAComboBox))
                {
                    if (((TAUtil.TAComboBox)objControl).ButtonsRight != null && ((TAUtil.TAComboBox)objControl).ButtonsRight.Count > 0)
                    {
                        if (objControl.Enabled == false)
                            ((TAUtil.TAComboBox)objControl).ButtonsRight[0].Enabled = false;
                        else
                            ((TAUtil.TAComboBox)objControl).ButtonsRight[0].Enabled = true;

                        //for readOnly
                        ((TAUtil.TAComboBox)objControl).ButtonsRight[0].Enabled = !((TAUtil.TAComboBox)objControl).ReadOnly;
                    }
                }

                #endregion

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
        private void MasterControlLock_Set(Control objControl, string controlState)
        {
            try
            {
                switch (controlState)
                {
                    case GVar.ControlState.BLANK:
                        objControl.Visible = false;
                        break;

                    case GVar.ControlState.A:   //editable regardless of IsNew state                        
                        objControl.Visible = true;
                        break;

                    case GVar.ControlState.N:   //Editable only when IsNew (true)
                        if (objControl.GetType() == typeof(Infragistics.Win.UltraWinTabControl.UltraTabPageControl))
                            ((Infragistics.Win.UltraWinTabControl.UltraTabControl)objControl.Parent).Tabs[objControl.Name].Enabled = true;
                        else
                        {
                            objControl.Visible = true;
                            if (objMstItmFactory.IsReadOnly || (objMstItmFactory.IsNew == false) || objMstItmFactory.ObjMSTItm.SubstituteItmKey > 0)
                            {
                                if (objControl.GetType() == typeof(TAUtil.TAGridEditor))
                                    GridLock(((TAUtil.TAGridEditor)objControl), true);
                                else
                                {
                                    if (objMstItmFactory.ObjMSTItm.SubstituteItmKey > 0)
                                    {
                                        switch (objControl.Name.ToLower())
                                        {
                                            case "itmid":
                                            case "itmdes":
                                            case "itmrem":
                                            case "industrypn":
                                            case "sku1":
                                            case "sku2":
                                            case "itmidlabel":
                                            case "itmdeslabel":
                                            case "itmremlabel":
                                            case "industrypnlabel":
                                            case "sku1label":
                                            case "sku2label":
                                                objControl.Enabled = true;
                                                break;

                                            default:
                                                objControl.Enabled = false;
                                                break;
                                        }
                                    }
                                    else
                                        objControl.Enabled = false;
                                }
                            }
                            else
                            {
                                if (objControl.GetType() == typeof(TAUtil.TAGridEditor))
                                    GridLock(((TAUtil.TAGridEditor)objControl), false);
                                else
                                    objControl.Enabled = true;
                            }
                        }
                        break;

                    case GVar.ControlState.R:   //Always Readonly
                        objControl.Visible = true;
                        objControl.Enabled = false;
                        break;
                }
                if (objControl.GetType() == typeof(TAUtil.TATextBoxEditor))
                {
                    if (((TAUtil.TATextBoxEditor)objControl).ButtonsRight != null && ((TAUtil.TATextBoxEditor)objControl).ButtonsRight.Count > 0)
                    {
                        if (objControl.Enabled == false)
                            ((TAUtil.TATextBoxEditor)objControl).ButtonsRight[0].Enabled = false;
                        else
                            ((TAUtil.TATextBoxEditor)objControl).ButtonsRight[0].Enabled = true;
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
        }
        private void GridLock(TAUtil.TAGridEditor grd, bool ReadOnlyFlag)
        {
            bool EnableGridLockProcess = false;
            try
            {
                switch (grd.Name.ToLower())
                {
                    #region Alternate
                    case "tagrddetalternates":
                        foreach (UltraGridColumn col in grd.DisplayLayout.Bands[0].Columns)
                        {
                            switch (col.Key.ToLower())
                            {
                                case "itmkey":
                                case "altitmkey":
                                case "createdate":
                                case "createuserkey":
                                case "lastmodifieddate":
                                case "lastmodifieduserkey":
                                    col.CellActivation = Activation.ActivateOnly;
                                    break;

                                default:
                                    if (ReadOnlyFlag)
                                        col.CellActivation = Activation.ActivateOnly;
                                    else
                                        col.CellActivation = Activation.AllowEdit;
                                    break;
                            }
                        }
                        EnableGridLockProcess = true;
                        break;
                    #endregion

                    #region Assembly
                    case "tagrddetassembly":
                        foreach (UltraGridColumn col in grd.DisplayLayout.Bands[0].Columns)
                        {
                            switch (col.Key.ToLower())
                            {
                                case "itmkey":
                                case "assitmkey":
                                case "assitmtype":
                                case "asssn":
                                case "assuomkey":
                                case "createdate":
                                case "createuserkey":
                                case "lastmodifieddate":
                                case "lastmodifieduserkey":
                                case "serialtracking":
                                    col.CellActivation = Activation.ActivateOnly;
                                    break;

                                default:
                                    if (ReadOnlyFlag)
                                        col.CellActivation = Activation.ActivateOnly;
                                    else
                                        col.CellActivation = Activation.AllowEdit;
                                    break;
                            }
                        }
                        EnableGridLockProcess = true;
                        break;
                    #endregion

                    #region BOM
                    case "tagrditmdetbompms":
                    case "tagrditmdetbomrms":
                    case "tagrditmdetbomlabours":
                        foreach (UltraGridColumn col in grd.DisplayLayout.Bands[0].Columns)
                        {
                            switch (col.Key.ToLower())
                            {
                                case "itmkey":
                                case "bomitmkey":
                                case "bomlinetype":
                                case "bomitmtype":
                                case "createdate":
                                case "createuserkey":
                                case "lastmodifieddate":
                                case "lastmodifieduserkey":
                                    col.CellActivation = Activation.ActivateOnly;
                                    break;

                                default:
                                    if (ReadOnlyFlag)
                                        col.CellActivation = Activation.ActivateOnly;
                                    else
                                        col.CellActivation = Activation.AllowEdit;
                                    break;
                            }
                        }
                        EnableGridLockProcess = true;
                        break;
                    #endregion

                    #region Location
                    case "tagrddetlocation":
                        foreach (UltraGridColumn col in grd.DisplayLayout.Bands[0].Columns)
                        {
                            switch (col.Key.ToLower())
                            {
                                case "itmkey":
                                case "lockey":
                                case "locqty":
                                case "locqtyopenbal":
                                case "createdate":
                                case "createuserkey":
                                case "lastmodifieddate":
                                case "lastmodifieduserkey":
                                    col.CellActivation = Activation.ActivateOnly;
                                    break;

                                default:
                                    if (ReadOnlyFlag)
                                        col.CellActivation = Activation.ActivateOnly;
                                    else
                                        col.CellActivation = Activation.AllowEdit;
                                    break;
                            }
                        }
                        grd.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                        grd.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
                        break;
                    #endregion

                    #region Price List
                    case "tagrdpricelist":
                        foreach (UltraGridColumn col in grd.DisplayLayout.Bands[0].Columns)
                        {
                            switch (col.Key.ToLower())
                            {
                                case "itmkey":
                                case "createdate":
                                case "createuserkey":
                                case "lastmodifieddate":
                                case "lastmodifieduserkey":
                                    col.CellActivation = Activation.ActivateOnly;
                                    break;

                                default:
                                    if (ReadOnlyFlag)
                                        col.CellActivation = Activation.ActivateOnly;
                                    else
                                        col.CellActivation = Activation.AllowEdit;
                                    break;
                            }
                        }
                        grd.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                        grd.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
                        break;
                    #endregion

                    #region Batch
                    case "tagrddetbatchs":
                        foreach (UltraGridColumn col in grd.DisplayLayout.Bands[0].Columns)
                        {
                            col.CellActivation = Activation.ActivateOnly;
                        }
                        grd.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                        grd.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
                        break;
                    #endregion

                    #region Serial
                        case "tagrddetserials":
                        if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                        {
                            //tagrdDetSerials.DisplayLayout.Bands[0].Columns["SerialID"].SortIndicator = SortIndicator.Ascending;
                            //tagrdDetSerials.DisplayLayout.Bands[0].Columns["ItmStatus"].CellClickAction = CellClickAction.EditAndSelectText;
                            //tagrdDetSerials.DisplayLayout.Bands[0].Columns["Custom1"].CellClickAction = CellClickAction.EditAndSelectText;
                            //tagrdDetSerials.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                            if (tagrdDetSerials.Rows.Count > 0)
                            {
                                for (int i = 0; i < tagrdDetSerials.Rows.Count; i++)
                                {
                                    tagrdDetSerials.Rows[i].Cells["ItmStatus"].Activation = Activation.NoEdit;
                                    tagrdDetSerials.Rows[i].Cells["Custom1"].Activation = Activation.NoEdit;
                                    tagrdDetSerials.Rows[i].Cells["Custom2"].Activation = Activation.NoEdit;
                                }
                            }

                            btnSerialPreview.Enabled = false;
                            btnSerialGenerate.Enabled = false;
                            btnSerialSave.Enabled = false;
                            btnSerialSaveOne.Enabled = false;
                        }
                     
                        break;
                    #endregion

                }
                if (EnableGridLockProcess)
                {
                    if (ReadOnlyFlag)
                    {
                        grd.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                        grd.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
                    }
                    else
                    {
                        grd.DisplayLayout.Override.AllowAddNew = AllowAddNew.TemplateOnBottom;
                        grd.DisplayLayout.Override.AllowDelete = DefaultableBoolean.True;
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
        private void GridCellDefault_Set()
        {
            #region Assembly
            this.tagrdDetAssembly.DisplayLayout.Bands[0].Columns["DefaultSelection"].DefaultCellValue = true;
            this.tagrdDetAssembly.DisplayLayout.Bands[0].Columns["LockQty"].DefaultCellValue = false;
            this.tagrdDetAssembly.DisplayLayout.Bands[0].Columns["ToPrint"].DefaultCellValue = true;
            #endregion

            #region BOM
            this.tagrdItmDetBOMRMs.DisplayLayout.Bands[0].Columns["BOMLineType"].DefaultCellValue = 10;
            this.tagrdItmDetBOMRMs.DisplayLayout.Bands[0].Columns["BOMQty"].DefaultCellValue = 0;
            this.tagrdItmDetBOMRMs.DisplayLayout.Bands[0].Columns["BOMLabourCost"].DefaultCellValue = 0;
            this.tagrdItmDetBOMPMs.DisplayLayout.Bands[0].Columns["BOMLineType"].DefaultCellValue = 20;
            this.tagrdItmDetBOMPMs.DisplayLayout.Bands[0].Columns["BOMQty"].DefaultCellValue = 0;
            this.tagrdItmDetBOMPMs.DisplayLayout.Bands[0].Columns["BOMLabourCost"].DefaultCellValue = 0;
            this.tagrdItmDetBOMLabours.DisplayLayout.Bands[0].Columns["BOMLineType"].DefaultCellValue = 30;
            this.tagrdItmDetBOMLabours.DisplayLayout.Bands[0].Columns["BOMQty"].DefaultCellValue = 0;
            this.tagrdItmDetBOMLabours.DisplayLayout.Bands[0].Columns["BOMLabourCost"].DefaultCellValue = 0;
            #endregion

            #region Location
            this.tagrdDetLocation.DisplayLayout.Bands[0].Columns["LocQtyMin"].DefaultCellValue = true;
            this.tagrdDetLocation.DisplayLayout.Bands[0].Columns["LocQtyMax"].DefaultCellValue = false;
            #endregion
        }//Completed
        private bool SavePriceList()
        {
            try
            {
                for (int i = 0; i < tagrdPriceList.Rows.Count - 1; i++)
                {
                    string propertyName = string.Empty;
                    System.Reflection.PropertyInfo propertyInfo = null;

                    if (i > 1)
                    {
                        propertyName = "Ratio" + (i - 1).ToString();
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[i].Cells["Ratio"].Value))
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Ratio"].Value, 1), null);
                        else
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Ratio"].Value, 1), null);

                        propertyName = "Price" + (i - 1).ToString("00") + "01";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[i].Cells["Price01"].Value))
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price01"].Value, 0), null);
                        else
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price01"].Value, 0), null);

                        propertyName = "Price" + (i - 1).ToString("00") + "02";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[i].Cells["Price02"].Value))
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price02"].Value, 0), null);
                        else
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price02"].Value, 0), null);

                        propertyName = "Price" + (i - 1).ToString("00") + "03";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[i].Cells["Price03"].Value))
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price03"].Value, 0), null);
                        else
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price03"].Value, 0), null);

                        propertyName = "Price" + (i - 1).ToString("00") + "04";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[i].Cells["Price04"].Value))
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price04"].Value, 0), null);
                        else
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price04"].Value, 0), null);

                        propertyName = "Price" + (i - 1).ToString("00") + "05";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[i].Cells["Price05"].Value))
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price05"].Value, 0), null);
                        else
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price05"].Value, 0), null);

                        propertyName = "Price" + (i - 1).ToString("00") + "06";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[i].Cells["Price06"].Value))
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price06"].Value, 0), null);
                        else
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price06"].Value, 0), null);

                        propertyName = "Price" + (i - 1).ToString("00") + "07";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[i].Cells["Price07"].Value))
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price07"].Value, 0), null);
                        else
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, (decimal?)0, null);

                        propertyName = "Price" + (i - 1).ToString("00") + "08";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[i].Cells["Price08"].Value))
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price08"].Value, 0), null);
                        else
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price08"].Value, 0), null);

                        propertyName = "Price" + (i - 1).ToString("00") + "09";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[i].Cells["Price09"].Value))
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price09"].Value, 0), null);
                        else
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price09"].Value, 0), null);

                        propertyName = "Price" + (i - 1).ToString("00") + "10";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[i].Cells["Price10"].Value))
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price10"].Value, 0), null);
                        else
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price10"].Value, 0), null);

                        propertyName = "Price" + (i - 1).ToString("00") + "11";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[i].Cells["Price11"].Value))
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price11"].Value, 0), null);
                        else
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price11"].Value, 0), null);

                        propertyName = "Price" + (i - 1).ToString("00") + "12";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[i].Cells["Price12"].Value))
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price12"].Value, 0), null);
                        else
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price12"].Value, 0), null);

                        propertyName = "Price" + (i - 1).ToString("00") + "13";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[i].Cells["Price13"].Value))
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price13"].Value, 0), null);
                        else
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price13"].Value, 0), null);

                        propertyName = "Price" + (i - 1).ToString("00") + "14";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[i].Cells["Price14"].Value))
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price14"].Value, 0), null);
                        else
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price14"].Value, 0), null);

                        propertyName = "Price" + (i - 1).ToString("00") + "15";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[i].Cells["Price15"].Value))
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price15"].Value, 0), null);
                        else
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Price15"].Value, 0), null);

                        //propertyName = "StandardCost" + (i).ToString();
                        //propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        //if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[i].Cells["StdCost"].Value))
                        //    propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["StdCost"].Value, 0), null);
                        //else
                        //    propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[i].Cells["StdCost"].Value, 0), null);
                    }

                    if (i < 15)
                    {
                        propertyName = "StandardCost" + (i + 1).ToString();
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[0].Cells["Price" + (i + 1).ToString("00")].Value))
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[0].Cells["Price" + (i + 1).ToString("00")].Value, 0), null);
                        else
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, (decimal?)0, null);

                        propertyName = "StandardPrice" + (i + 1).ToString();
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (propertyInfo != null && !GFunc.IsNE(tagrdPriceList.Rows[1].Cells["Price" + (i + 1).ToString("00")].Value))
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdPriceList.Rows[1].Cells["Price" + (i + 1).ToString("00")].Value, 0), null);
                        else
                            propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, (decimal?)0, null);


                    }
                    if (i > 4) continue;

                    propertyName = "QtyDisQty" + (i + 1).ToString();
                    propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                    if (propertyInfo != null && !GFunc.IsNE(tagrdQtyRatio.Rows[i].Cells["QtyDiscount"].Value))
                        propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdQtyRatio.Rows[i].Cells["QtyDiscount"].Value, 0), null);
                    else
                        propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, (decimal?)0, null);

                    propertyName = "QtyDisRatio" + (i + 1).ToString();
                    propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                    if (propertyInfo != null && !GFunc.IsNE(tagrdQtyRatio.Rows[i].Cells["DiscountRatio"].Value))
                        propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, GFunc.NEDec(tagrdQtyRatio.Rows[i].Cells["DiscountRatio"].Value, 1), null);
                    else
                        propertyInfo.SetValue(objMstItmFactory.ObjMSTItmDetPrice, (decimal?)0, null);
                }
                return true;
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false); // System Msg   
            }
        }//Completed
        private bool LoadPriceList()
        {
            try
            {
                if (tagrdPriceList.Rows.Count > 0)
                    tagrdPriceList.Rows[0].Cells["Ratio"].Value = DBNull.Value;
                //DataTable dt = (DataTable)tagrdPriceList.DataSource;
                for (int i = 0; i < tagrdPriceList.Rows.Count - 1; i++)
                {

                    string propertyName = string.Empty;
                    System.Reflection.PropertyInfo propertyInfo = null;
                    object obj = new object();

                    if (i > 1)
                    {
                        #region Get Ratio and Price 1 to 15
                        propertyName = "Ratio" + (i - 1).ToString();
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        tagrdPriceList.Rows[i].Cells["Ratio"].Value = GFunc.NEDec(obj, 0);
                        //standard price1
                        propertyName = "Price" + (i - 1).ToString("00") + "01";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);
                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        tagrdPriceList.Rows[i].Cells["Price01"].Value = GFunc.NEDec(obj, 0);

                        //standard price2
                        propertyName = "Price" + (i - 1).ToString("00") + "02";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        tagrdPriceList.Rows[i].Cells["Price02"].Value = GFunc.NEDec(obj, 0);

                        // standard price3
                        propertyName = "Price" + (i - 1).ToString("00") + "03";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        tagrdPriceList.Rows[i].Cells["Price03"].Value = GFunc.NEDec(obj, 0);

                        // standard price4
                        propertyName = "Price" + (i - 1).ToString("00") + "04";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        tagrdPriceList.Rows[i].Cells["Price04"].Value = GFunc.NEDec(obj, 0);

                        //standard price5
                        propertyName = "Price" + (i - 1).ToString("00") + "05";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        tagrdPriceList.Rows[i].Cells["Price05"].Value = GFunc.NEDec(obj, 0);

                        //standard price6
                        propertyName = "Price" + (i - 1).ToString("00") + "06";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        tagrdPriceList.Rows[i].Cells["Price06"].Value = GFunc.NEDec(obj, 0);

                        // standard price7
                        propertyName = "Price" + (i - 1).ToString("00") + "07";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        tagrdPriceList.Rows[i].Cells["Price07"].Value = GFunc.NEDec(obj, 0);

                        // standard price8
                        propertyName = "Price" + (i - 1).ToString("00") + "08";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        tagrdPriceList.Rows[i].Cells["Price08"].Value = GFunc.NEDec(obj, 0);

                        // standard price9
                        propertyName = "Price" + (i - 1).ToString("00") + "09";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        tagrdPriceList.Rows[i].Cells["Price09"].Value = GFunc.NEDec(obj, 0);

                        // standard price10
                        propertyName = "Price" + (i - 1).ToString("00") + "10";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        tagrdPriceList.Rows[i].Cells["Price10"].Value = GFunc.NEDec(obj, 0);

                        // standard price11
                        propertyName = "Price" + (i - 1).ToString("00") + "11";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        tagrdPriceList.Rows[i].Cells["Price11"].Value = GFunc.NEDec(obj, 0);

                        // standard price12
                        propertyName = "Price" + (i - 1).ToString("00") + "12";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        tagrdPriceList.Rows[i].Cells["Price12"].Value = GFunc.NEDec(obj, 0);

                        //standard price13
                        propertyName = "Price" + (i - 1).ToString("00") + "13";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        tagrdPriceList.Rows[i].Cells["Price13"].Value = GFunc.NEDec(obj, 0);

                        // standard price14
                        propertyName = "Price" + (i - 1).ToString("00") + "14";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        tagrdPriceList.Rows[i].Cells["Price14"].Value = GFunc.NEDec(obj, 0);

                        // standard price15
                        propertyName = "Price" + (i - 1).ToString("00") + "15";
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        tagrdPriceList.Rows[i].Cells["Price15"].Value = GFunc.NEDec(obj, 0);


                        //propertyName = "StandardCost" + (i).ToString();
                        //propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        //if (!GFunc.IsNE(propertyInfo))
                        //    obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        //tagrdPriceList.Rows[1].Cells[i].Value = GFunc.NEDec(obj, 0);
                        #endregion
                    }

                    if (i < 15)
                    {
                        #region Get Standard Cost 1 to 15
                        propertyName = "StandardCost" + (i + 1).ToString();
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        tagrdPriceList.Rows[0].Cells["Price" + (i + 1).ToString("00")].Value = GFunc.NEDec(obj, 0);

                        #endregion

                        #region Get Standard Price 1 to 15
                        propertyName = "StandardPrice" + (i + 1).ToString();
                        propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                        tagrdPriceList.Rows[1].Cells["Price" + (i + 1).ToString("00")].Value = GFunc.NEDec(obj, 0);
                        #endregion


                    }

                    if (i > 4) continue;
                    propertyName = "QtyDisQty" + (i + 1).ToString();
                    propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                    if (!GFunc.IsNE(propertyInfo))
                        obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                    tagrdQtyRatio.Rows[i].Cells["QtyDiscount"].Value = GFunc.NEDec(obj, 0);

                    propertyName = "QtyDisRatio" + (i + 1).ToString();
                    propertyInfo = objMstItmFactory.ObjMSTItmDetPrice.GetType().GetProperty(propertyName);

                    if (!GFunc.IsNE(propertyInfo))
                        obj = propertyInfo.GetValue(objMstItmFactory.ObjMSTItmDetPrice, null);

                    tagrdQtyRatio.Rows[i].Cells["DiscountRatio"].Value = GFunc.NEDec(obj, 0);
                }

                tagrdPriceList.UpdateData();
                tagrdPriceList.Rows.Refresh(RefreshRow.ReloadData);
                tagrdQtyRatio.UpdateData();
                tagrdQtyRatio.Rows.Refresh(RefreshRow.ReloadData);


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
        private void ClearPriceList()
        {
            try
            {
                for (int i = 0; i < tagrdPriceList.Rows.Count; i++)
                {
                    tagrdPriceList.Rows[i].Cells["Ratio"].Value = (decimal?)0;

                    tagrdPriceList.Rows[i].Cells["Price01"].Value = (decimal?)0;

                    tagrdPriceList.Rows[i].Cells["Price02"].Value = (decimal?)0;

                    tagrdPriceList.Rows[i].Cells["Price03"].Value = (decimal?)0;

                    tagrdPriceList.Rows[i].Cells["Price04"].Value = (decimal?)0;

                    tagrdPriceList.Rows[i].Cells["Price05"].Value = (decimal?)0;

                    tagrdPriceList.Rows[i].Cells["Price06"].Value = (decimal?)0;

                    tagrdPriceList.Rows[i].Cells["Price07"].Value = (decimal?)0;

                    tagrdPriceList.Rows[i].Cells["Price08"].Value = (decimal?)0;

                    tagrdPriceList.Rows[i].Cells["Price09"].Value = (decimal?)0;

                    tagrdPriceList.Rows[i].Cells["Price10"].Value = (decimal?)0;

                    tagrdPriceList.Rows[i].Cells["Price11"].Value = (decimal?)0;

                    tagrdPriceList.Rows[i].Cells["Price12"].Value = (decimal?)0;

                    tagrdPriceList.Rows[i].Cells["Price13"].Value = (decimal?)0;

                    tagrdPriceList.Rows[i].Cells["Price14"].Value = (decimal?)0;

                    tagrdPriceList.Rows[i].Cells["Price15"].Value = (decimal?)0;

                    tagrdPriceList.Rows[i].Cells["StdCost"].Value = (decimal?)0;

                    if (i > 4) continue;
                    tagrdQtyRatio.Rows[i].Cells["QtyDiscount"].Value = (decimal?)0;

                    tagrdQtyRatio.Rows[i].Cells["DiscountRatio"].Value = (decimal?)0;
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

        //Functions
        private bool New_Process()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (form_CanValidate() == false)
                {
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show("Validation Failed, Discard changes?",
                                              GEnum.MsgBoxIcon.Question,
                                              GEnum.MsgBoxButton.Yes,
                                              GEnum.MsgBoxButton.No);

                    if (btnSelect == GEnum.MsgBoxButton.No)
                    {
                        return false;
                    }
                    else
                    {
                        this.objMstItmFactory.IsDirty = false;
                        IsGridsDirty(true);
                    }
                }

                this.errorProvider1.Clear();

                if (this.objMstItmFactory.IsDirty)
                {
                    if (Save_Process() == false)
                        return false;
                }

                if (this.objMstItmFactory.New(0) == false)
                {
                    this.ClearPriceList();
                    ControlPriceH.Appearance.BackColor = System.Drawing.Color.Transparent;
                    return false;
                }
                else
                {
                    this.ClearPriceList();
                    ControlPriceH.Appearance.BackColor = System.Drawing.Color.Transparent;
                    this.ItmID.Focus();
                    return true;
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
                this.Refresh_All(true);
                this.FormLayout();
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private bool SaveChanges()
        {
            try
            {

                if (form_CanValidate() == false)
                    return false;

                if (objMstItmFactory.IsDirty)
                {
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show(MsgID.Common.SaveChanges,
                                          GEnum.MsgBoxIcon.Question,
                                          GEnum.MsgBoxButton.Save_Changes,
                                          GEnum.MsgBoxButton.Discard_Changes,
                                          GEnum.MsgBoxButton.I_Dont_Know);

                    if (btnSelect == GEnum.MsgBoxButton.Save_Changes)
                        return this.Save_Process();
                    else if (btnSelect == GEnum.MsgBoxButton.I_Dont_Know)
                    {
                        if (formClose)
                            formClose = false;

                        return false;
                    }
                }
                this.errorProvider1.Clear();
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
        private bool Save_Process()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //Perform Validation
                if (form_CanValidate() == false)
                    return false;

                //Perform Saving
                SavePriceList();
                //BindDefaultAcctByCatKey1();

                if (this.objMstItmFactory.Save())
                {
                    if (GFunc.IsNE(this.ListEvent_RefreshRecord) == false)
                        ListEvent_RefreshRecord.Invoke();

                    /* Synchronize Estore Price to estore synctable and mysql table */
                    if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM && chkSyncEstore.Checked)
                    {
                        SyncEstore();
                    }

                    Display_eStorePriceColor();

                    return true;
                }
                else
                {
                    throw new TAException(MsgID.Common.SaveFail);
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
                this.Refresh_All(true);
                this.FormLayout();
                this.Cursor = Cursors.Default;
            }
        }//Completed
        public bool OpenRecord(int key, string id)
        {
            this.Cursor = Cursors.WaitCursor;
            GEnum.MsgBoxButton btnSelect = GEnum.MsgBoxButton.Yes;

            try
            {
                if (this.SaveChanges() == false)
                    return false;

                if (SECPermUtility.Edit(objMstItmFactory.PermID, false))
                {
                    if (objMstItmFactory.GetEdit(key, id) == false)
                    {
                        if (SysOptionUtility.GetBool("WarnOpenRecordAsReadOnly"))
                        {
                            btnSelect = MsgBox.Show("Try to open record as read only",
                                                  GEnum.MsgBoxIcon.Question,
                                                  GEnum.MsgBoxButton.Yes,
                                                  GEnum.MsgBoxButton.No);

                            if (btnSelect == GEnum.MsgBoxButton.Yes)
                            {
                                objMstItmFactory.GetReadOnly(key, id);
                            }
                        }
                    }
                }
                else
                    objMstItmFactory.GetReadOnly(key, id);

                btnAttachmentEdit.Text = "(" + objMstItmFactory.ObjMSTItm.Attachments.Count + ")";

                Display_eStorePriceColor();

                //added by thettm on 05 dec 2017 (start) certificate item
                if (objMstItmFactory.ObjMSTItm.CertiLink.ToString() != "")
                {
                    tabDetList.Tabs[11].Visible = true;
                    DataTable tblcertificate = new DataTable();
                    DataColumn colContent = tblcertificate.Columns.Add("Content", typeof(string));
                    
                    string[] lines = objMstItmFactory.ObjMSTItm.CertiLink.ToString().Split('|');
                    foreach (var line in lines)
                    {

                        DataRow row = tblcertificate.NewRow();
                        row.SetField(colContent, line.TrimStart());
                        tblcertificate.Rows.Add(row);
                    }
                    tagrdCertificate.DataSource = tblcertificate;
                    tagrdCertificate.DataBind();
                    tagrdCertificate.DisplayLayout.Bands[0].Columns[0].Width = 1000;


                }
                else
                    tabDetList.Tabs[11].Visible = false;
                //added by thettm on 05 dec 2017 (end) certificate item

                if (!SECPermUtility.Perform("AbilityToSetPurchaseBlockItem", false))
                    taBlockPurchase.Enabled = false;
                else                    
                    taBlockPurchase.Enabled = true;

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
                //we will always need to refresh header and detail regardless if it is GetEdit, GetReadOnly, Restore old data
                Refresh_All(true);
                FormLayout();
                this.Cursor = Cursors.Default;
            }
        }//Completed

        //added by thettm on 30 Aug 2018(start)
        private void SettingForAssembly()
        {
            groupBox1.Visible = true;
            INClass.Enabled = false;
            if (objMstItmFactory.ObjMSTItm.INClass == "" || objMstItmFactory.ObjMSTItm.INClass == "DIRECT PICKING")
            {
                optDP.Checked = true;
                objMstItmFactory.ObjMSTItm.INClass = "DIRECT PICKING";
            }
            else if (objMstItmFactory.ObjMSTItm.INClass == "KITTING ASSEMBLY")
            {
                optKA.Checked = true;
                objMstItmFactory.ObjMSTItm.INClass = "KITTING ASSEMBLY";
            }
        }
        //added by thettm on 30 Aug 2018(end)
        private bool Delete_Process()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnDeleteRecord))
                {
                    //Ask Confirmation for Delete
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know);

                    if (btnSelect != GEnum.MsgBoxButton.Delete)
                        return false;
                }

                if (this.objMstItmFactory.Delete())
                {
                    IsGridsDirty(true);
                    if (!GFunc.IsNE(this.ListEvent_RefreshRecord))
                        ListEvent_RefreshRecord.Invoke();

                    this.objMstItmFactory.New(0);
                    return true;
                }
                else
                {
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
            finally
            {
                this.Refresh_All(true);
                this.FormLayout();
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private bool Clear_Process()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (GFunc.IsNEZ(this.objMstItmFactory.ObjMSTItm.ItmKey))
                {
                    if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnClearRecord))
                    {
                        //Ask Confirmation for Delete
                        GEnum.MsgBoxButton btnSelect;
                        btnSelect = MsgBox.Show(MsgID.Common.ConfirmClear,
                                              GEnum.MsgBoxIcon.Question,
                                              GEnum.MsgBoxButton.Clear,
                                              GEnum.MsgBoxButton.Dont_Clear,
                                              GEnum.MsgBoxButton.I_Dont_Know);

                        if (btnSelect != GEnum.MsgBoxButton.Clear)
                            return false;
                    }

                    IsGridsDirty(true);

                    if (this.objMstItmFactory.New(0))
                    {
                        errorProvider1.Clear();
                        return true;
                    }
                    else
                        return false;
                }
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
            finally
            {
                this.Refresh_All(true);
                this.FormLayout();
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private bool form_CanValidate()
        {
            try
            {
                //this function check if we can validates all the header.controls
                //all grid is able to commit its changes to its datasource
                TAUtil.ControlGVar.FormValidateFail = false;
                this.errorProvider1.Clear();
                this.Validate();
                this.tagrdDetAlternates.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdDetAlternates.UpdateData();
                this.tagrdDetAssembly.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdDetAssembly.UpdateData();
                this.tagrdDetLocation.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdDetLocation.UpdateData();
                this.tagrdItmDetBOMLabours.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdItmDetBOMLabours.UpdateData();
                this.tagrdItmDetBOMPMs.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdItmDetBOMPMs.UpdateData();
                this.tagrdItmDetBOMRMs.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdItmDetBOMRMs.UpdateData();
                this.tagrdPriceList.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdPriceList.UpdateData();

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

            #region Alternate
            if (tagrdDetAlternates.ActiveRow != null)
            {
                if (tagrdDetAlternates.ActiveRow.DataChanged && !tagrdDetAlternates.ActiveRow.IsUnmodifiedTemplateAddRow)
                {
                    if (undoChangesInGrid)
                    {
                        //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                        this.tagrdDetAlternates.PerformAction(UltraGridAction.UndoCell);
                        this.tagrdDetAlternates.PerformAction(UltraGridAction.UndoRow);
                    }
                    return true;
                }
            }
            #endregion

            #region Assembly
            if (tagrdDetAssembly.ActiveRow != null)
            {
                if (tagrdDetAssembly.ActiveRow.DataChanged && !tagrdDetAssembly.ActiveRow.IsUnmodifiedTemplateAddRow)
                {
                    if (undoChangesInGrid)
                    {
                        //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                        this.tagrdDetAssembly.PerformAction(UltraGridAction.UndoCell);
                        this.tagrdDetAssembly.PerformAction(UltraGridAction.UndoRow);
                    }
                    return true;
                }
            }
            #endregion

            #region Location
            if (tagrdDetLocation.ActiveRow != null)
            {
                if (tagrdDetLocation.ActiveRow.DataChanged)
                {
                    if (undoChangesInGrid)
                    {
                        //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                        this.tagrdDetLocation.PerformAction(UltraGridAction.UndoCell);
                        this.tagrdDetLocation.PerformAction(UltraGridAction.UndoRow);
                    }
                    return true;
                }
            }
            #endregion

            #region BOM Labour
            if (tagrdItmDetBOMLabours.ActiveRow != null && !tagrdItmDetBOMLabours.ActiveRow.IsUnmodifiedTemplateAddRow)
            {
                if (tagrdItmDetBOMLabours.ActiveRow.DataChanged)
                {
                    if (undoChangesInGrid)
                    {
                        //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                        this.tagrdItmDetBOMLabours.PerformAction(UltraGridAction.UndoCell);
                        this.tagrdItmDetBOMLabours.PerformAction(UltraGridAction.UndoRow);
                    }
                    return true;
                }
            }
            #endregion

            #region BOM  Packing
            if (tagrdItmDetBOMPMs.ActiveRow != null)
            {
                if (tagrdItmDetBOMPMs.ActiveRow.DataChanged && !tagrdItmDetBOMPMs.ActiveRow.IsUnmodifiedTemplateAddRow)
                {
                    if (undoChangesInGrid)
                    {
                        //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                        this.tagrdItmDetBOMPMs.PerformAction(UltraGridAction.UndoCell);
                        this.tagrdItmDetBOMPMs.PerformAction(UltraGridAction.UndoRow);
                    }
                    return true;
                }
            }
            #endregion

            #region Raw Material
            if (tagrdItmDetBOMRMs.ActiveRow != null)
            {
                if (tagrdItmDetBOMRMs.ActiveRow.DataChanged && !tagrdItmDetBOMRMs.ActiveRow.IsUnmodifiedTemplateAddRow)
                {
                    if (undoChangesInGrid)
                    {
                        //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                        this.tagrdItmDetBOMRMs.PerformAction(UltraGridAction.UndoCell);
                        this.tagrdItmDetBOMRMs.PerformAction(UltraGridAction.UndoRow);
                    }
                    return true;
                }
            }
            #endregion

            #region Price List
            if (tagrdPriceList.ActiveRow != null)
            {
                if (tagrdPriceList.ActiveRow.DataChanged && !tagrdPriceList.ActiveRow.IsUnmodifiedTemplateAddRow)
                {
                    if (undoChangesInGrid)
                    {
                        //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                        this.tagrdPriceList.PerformAction(UltraGridAction.UndoCell);
                        this.tagrdPriceList.PerformAction(UltraGridAction.UndoRow);
                    }
                    return true;
                }
            }
            #endregion

            return false;
        }//Completed

        //Tab Events
        private void tabDetList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (tabDetList.ActiveTab.Key.ToLower())
                {
                    case "tabpagedefault":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                if (Brandkey.Visible)
                                    BranchKey.Focus();
                                else
                                    AccICKey.Focus();
                                break;
                            case Keys.Up:
                                ItmDes.Focus();
                                break;
                        }
                        break;
                    case "tabpagegeneral":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                BUOMKey.Focus();
                                break;
                            case Keys.Up:
                                ItmDes.Focus();
                                break;
                        }
                        break;
                    case "tabpagegrouping":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                Brandkey.Focus();
                                break;
                            case Keys.Up:
                                ItmDes.Focus();
                                break;
                        }
                        break;
                    case "tabpagepricelist":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                tagrdPriceList.Focus();
                                if (tagrdPriceList.Rows.Count > 0)
                                {
                                    UltraGridColumn FirstVisCol = tagrdPriceList.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                                    if (FirstVisCol != null)
                                    {
                                        tagrdPriceList.ActiveCell = tagrdPriceList.Rows[0].Cells[FirstVisCol.Key];
                                        tagrdPriceList.PerformAction(UltraGridAction.EnterEditMode, false, false);
                                    }
                                }
                                break;
                            case Keys.Up:
                                ItmDes.Focus();
                                break;
                        }
                        break;
                    case "tabpagelocation":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                tagrdDetLocation.Focus();
                                if (tagrdDetLocation.ActiveRow != null)
                                {
                                    UltraGridColumn FirstVisCol = tagrdDetLocation.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                                    if (FirstVisCol != null)
                                    {
                                        tagrdDetLocation.ActiveCell = tagrdDetLocation.Rows.TemplateAddRow.Cells[FirstVisCol.Key];
                                        tagrdDetLocation.PerformAction(UltraGridAction.EnterEditMode, false, false);
                                    }
                                }
                                break;
                            case Keys.Up:
                                ItmDes.Focus();
                                break;
                        }
                        break;
                    case "tabpagealternate":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                tagrdDetAlternates.Focus();
                                if (tagrdDetAlternates.ActiveRow != null)
                                {
                                    UltraGridColumn FirstVisCol = tagrdDetAlternates.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                                    if (FirstVisCol != null)
                                    {
                                        tagrdDetAlternates.ActiveCell = tagrdDetAlternates.Rows.TemplateAddRow.Cells[FirstVisCol.Key];
                                        tagrdDetAlternates.PerformAction(UltraGridAction.EnterEditMode, false, false);
                                    }
                                }
                                break;
                            case Keys.Up:
                                ItmDes.Focus();
                                break;
                        }
                        break;
                    case "tabpageothers":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                SaleUOM.Focus();
                                break;
                            case Keys.Up:
                                ItmDes.Focus();
                                break;
                        }
                        break;
                    case "tabpagebom":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                BOMType.Focus();
                                break;
                            case Keys.Up:
                                ItmDes.Focus();
                                break;
                        }
                        break;
                    case "tabpagemeasurement":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                ScaleKey.Focus();
                                break;
                            case Keys.Up:
                                ItmDes.Focus();
                                break;
                        }
                        break;
                    case "tabpagebatch":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                tagrdDetBatchs.Focus();
                                break;
                            case Keys.Up:
                                ItmDes.Focus();
                                break;
                        }
                        break;
                    case "tabpageassembly":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                tagrdDetAssembly.Focus();
                                if (tagrdDetAssembly.ActiveRow != null)
                                {
                                    UltraGridColumn FirstVisCol = tagrdDetAssembly.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                                    if (FirstVisCol != null)
                                    {
                                        tagrdDetAssembly.ActiveCell = tagrdDetAssembly.Rows.TemplateAddRow.Cells[FirstVisCol.Key];
                                        tagrdDetAssembly.PerformAction(UltraGridAction.EnterEditMode, false, false);
                                    }
                                }
                                break;
                            case Keys.Up:
                                ItmDes.Focus();
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
        private void tabBOMs_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (tabBOMs.ActiveTab.Key.ToLower())
                {
                    case "rm":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                tagrdItmDetBOMRMs.Focus();
                                UltraGridColumn FirstVisCol = tagrdItmDetBOMRMs.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                                if (FirstVisCol != null)
                                {
                                    tagrdItmDetBOMRMs.ActiveCell = tagrdItmDetBOMRMs.Rows.TemplateAddRow.Cells[FirstVisCol.Key];
                                    tagrdItmDetBOMRMs.PerformAction(UltraGridAction.EnterEditMode, false, false);
                                }
                                break;
                            case Keys.Up:
                                BOMType.Focus();
                                break;
                        }
                        break;
                    case "pm":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                tagrdItmDetBOMRMs.Focus();
                                UltraGridColumn FirstVisCol = tagrdItmDetBOMPMs.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                                if (FirstVisCol != null)
                                {
                                    tagrdItmDetBOMPMs.ActiveCell = tagrdItmDetBOMPMs.Rows.TemplateAddRow.Cells[FirstVisCol.Key];
                                    tagrdItmDetBOMPMs.PerformAction(UltraGridAction.EnterEditMode, false, false);
                                }
                                break;
                            case Keys.Up:
                                BOMType.Focus();
                                break;
                        }
                        break;
                    case "lb":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                tagrdItmDetBOMLabours.Focus();
                                UltraGridColumn FirstVisCol = tagrdItmDetBOMLabours.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                                if (FirstVisCol != null)
                                {
                                    tagrdItmDetBOMLabours.ActiveCell = tagrdItmDetBOMLabours.Rows.TemplateAddRow.Cells[FirstVisCol.Key];
                                    tagrdItmDetBOMLabours.PerformAction(UltraGridAction.EnterEditMode, false, false);
                                }
                                break;
                            case Keys.Up:
                                BOMType.Focus();
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
        private void OpenID_CustomUpdate(object sender, CancelEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;

            try
            {
                if (GFunc.IsNE(OpenID.Text) == false)
                {
                    string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, OpenID.Name);
                    key = GFunc.ItmRecord_GetKey(GEnum.RecAccessType.ItemDes, listSettingID, OpenID.Text, 0, ref id, ref des, true);
                    if (GFunc.IsNEZ(key))
                    {
                        if (DocHDRUtil.EditorButton_Popup((int)objMstItmFactory.ConstantCodeKey, OpenID.Text, listSettingID, (int)GEnum.PopupType.ItmDes, ref key, ref id, ref des) == false)
                            return;
                    }
                    OpenID.SetValueTrigger(des, false);
                    this.OnListRecordSelected(key);
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
        private void OpenID_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;

            try
            {
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, OpenID.Name);
                if (DocHDRUtil.EditorButton_Popup((int)objMstItmFactory.ConstantCodeKey, OpenID.Text, listSettingID, (int)GEnum.PopupType.ItmDes, ref key, ref id, ref des))
                {
                    OpenID.SetValueTrigger(des, false);
                    OnListRecordSelected(key);
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
        private void ItmType_CustomUpdate(object sender, CancelEventArgs e)
        {
            int selectedItmType;
            try
            {
                if (GFunc.IsNEZ(this.ItmType.Value))
                {
                    e.Cancel = true;
                    MsgBox.Show("Type cannot be empty");
                    return;
                }

                selectedItmType = (int)this.ItmType.Value;

                if (selectedItmType == 0 || objMstItmFactory.IsNew == false)
                {
                    e.Cancel = true;
                    MsgBox.Show("Invalid Type");
                    return;
                }

                IsGridsDirty(true);

                //Set MasterItmType default to Stock when ItmTyep is Master
                if (selectedItmType == (int)GEnum.ItemType.Master)
                {
                    objMstItmFactory.ObjMSTItm.MasterItmType = (int)GEnum.ItemType.Stock;
                    //selectedItmType = (int)GEnum.ItemType.Stock;
                }
                else
                {
                    objMstItmFactory.ObjMSTItm.MasterItmType = 0;
                }
                if (this.objMstItmFactory.New(selectedItmType))
                {
                    switch (objMstItmFactory.ObjMSTItm.ItmType)
                    {
                        case (int)GEnum.ItemType.StockB:
                        case (int)GEnum.ItemType.Finished_GDB:
                            objMstItmFactory.ObjMSTItm.CostMethod = (int)GEnum.CostMethod.FIFO;
                            break;
                        case (int)GEnum.ItemType.Charges: /* added by YST on 2021/08/13 */
                            objMstItmFactory.ObjMSTItm.Custom1 = ((DataTable)ItmChargesType.DataSource).Rows[0][1].ToString();                            
                            lblChargesType.Visible = true;
                            ItmChargesType.Visible = true;
                            groupBox1.Visible = false;
                            INClass.Enabled = true;
                            break;
                        //added by thettm on 30 Aug 2018(start)
                        case (int)GEnum.ItemType.Assembly:
                            SettingForAssembly();
                            break;
                        default:
                            lblChargesType.Visible = false;
                            ItmChargesType.Visible = false;
                            groupBox1.Visible = false;
                            INClass.Enabled = true;
                            break;
                        //added by thettm on 30 Aug 2018(end)
                    }
                    //Set MasterItmType default to Stock when ItmTyep is Master
                    if (selectedItmType == (int)GEnum.ItemType.Master)
                    {
                        objMstItmFactory.ObjMSTItm.MasterItmType = (int)GEnum.ItemType.Stock;
                        //selectedItmType = (int)GEnum.ItemType.Stock;
                    }
                    else
                    {
                        objMstItmFactory.ObjMSTItm.MasterItmType = 0;
                    }
                    this.Refresh_All(true);
                    this.FormLayout();
                    this.errorProvider1.Clear();
                    //this.ItmID.Focus();
                }
                else
                {
                    this.Refresh_All(true);
                    this.FormLayout();
                }
                if (tabPageGeneral.Enabled)
                    tabDetList.SelectedTab = tabDetList.Tabs["tabPageGeneral"];
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
        private void MasterItmType_CustomUpdate(object sender, CancelEventArgs e)
        {
            this.MasterItmType.SetValueTrigger(GFunc.NEInt(this.MasterItmType.Value, (int)GEnum.ItemType.Stock), false);
            FormLayout();
        }

        //Editor Controls Events
        private void RecSearchSelected(object sender, string fieldNm, int key, string id, string des)
        {
            //This is a common function used by header and grid to handle Con,Itm,Acc,Job RecordSearch
            //Currently the sender is not use (for future requirements)
            //this is because we do not have a situation where there is a conflict of fieldNm between Header and Grids
            //when this happen we will need to add another switch to check the gridName in order to resolve the conflict

            try
            {
                MSTItm objItm;
                TAUtil.TAGridEditor grd;

                switch (fieldNm.ToLower())
                {
                    case "altitmid":
                    case "altitmdes": 
                        tagrdDetAlternates.ActiveRow.Cells["AltItmKey"].Value = key;
                        tagrdDetAlternates.ActiveRow.Cells["AltItmID"].Value = id;
                        tagrdDetAlternates.ActiveRow.Cells["AltItmDes"].Value = des;
                        break;

                    case "assitmid":
                    case "assitmdes":
                        objItm = MSTItm.Get(key);
                        tagrdDetAssembly.ActiveRow.Cells["AssItmKey"].Value = objItm.ItmKey;
                        tagrdDetAssembly.ActiveRow.Cells["AssItmID"].Value = objItm.ItmID;
                        tagrdDetAssembly.ActiveRow.Cells["AssUOMKey"].Value = objItm.BUOMKey;
                        tagrdDetAssembly.ActiveRow.Cells["AssItmDes"].Value = objItm.ItmDes;
                        tagrdDetAssembly.ActiveRow.Cells["AssItmType"].Value = objItm.ItmType;
                        if (objItm.ItmType == (int)GEnum.ItemType.Charges)
                            tagrdDetAssembly.ActiveRow.Cells["AssQty"].Value = DBNull.Value;
                        else
                            tagrdDetAssembly.ActiveRow.Cells["AssQty"].Value = 0M;
                        break;

                    case "bomitmid":
                    case "bomitmdes":
                        objItm = MSTItm.Get(key);
                        grd = (TAUtil.TAGridEditor)sender;
                        grd.ActiveRow.Cells["BOMItmKey"].Value = key;
                        grd.ActiveRow.Cells["BOMItmID"].Value = objItm.ItmID;
                        grd.ActiveRow.Cells["BOMUOMKey"].Value = objItm.BUOMKey.ToDBValue();
                        grd.ActiveRow.Cells["BOMItmDes"].Value = objItm.ItmDes;
                        grd.ActiveRow.Cells["BOMItmType"].Value = objItm.ItmType.ToDBValue();
                        break;

                    case "masteritmid":
                    case "masteritmdesc":
                        //Currently not use
                        this.objMstItmFactory.ObjMSTItm.MasterItmKey = key;
                        this.objMstItmFactory.ObjMSTItm.MasterItmID = id;
                        MasterItmDesc.SetValueTrigger(des, false);
                        break;

                    case "substituteitmid":
                    case "substituteitmdesc":
                        this.objMstItmFactory.ObjMSTItm.SubstituteItmKey = key;
                        this.objMstItmFactory.ObjMSTItm.SubstituteItmID = id;
                        SubstituteItmDesc.SetValueTrigger(des, false);
                        ItmType.Enabled = false;
                        tabDetList.Visible = false;
                        break;

                    case "csgvendorkey":
                    case "csgvendornm":
                        this.objMstItmFactory.ObjMSTItm.CSGVendorKey = key;
                        this.objMstItmFactory.ObjMSTItm.CSGVendorID = id;
                        CSGVendorNm.SetValueTrigger(des, false);
                        break;

                    case "accickey":
                    case "accicnm":
                        this.objMstItmFactory.ObjMSTItm.AccICKey = key;
                        this.objMstItmFactory.ObjMSTItm.AccICID = id;
                        AccICNm.SetValueTrigger(des, false);

                        break;

                    case "accinkey":
                    case "accinnm":
                        this.objMstItmFactory.ObjMSTItm.AccINKey = key;
                        this.objMstItmFactory.ObjMSTItm.AccINID = id;
                        AccINNm.SetValueTrigger(des, false);
                        break;


                    case "accphkey":
                    case "accphnm":
                        this.objMstItmFactory.ObjMSTItm.AccPHKey = key;
                        this.objMstItmFactory.ObjMSTItm.AccPHID = id;
                        AccPHNm.SetValueTrigger(des, false);
                        break;

                    case "accdsickey":
                    case "accdsicdes":
                         this.objMstItmFactory.ObjMSTItm.AccDSICKey = key;
                        this.objMstItmFactory.ObjMSTItm.AccDSICID = id;
                        AccDSICDes.SetValueTrigger(des, false);
                        break;
                    case "accdsphkey":
                    case "accdsphdes":
                        this.objMstItmFactory.ObjMSTItm.AccPHKey = key;
                        this.objMstItmFactory.ObjMSTItm.AccPHID = id;
                        AccDSPHDes.SetValueTrigger(des, false);
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
        private bool RecSearchProcess(object sender, string columnKey, bool FromButtonClick)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                int vendorKey = 0;
                string FieldName = string.Empty;
                string controlText = string.Empty;
                bool senderIsGrid = false;
                string listSettingID = string.Empty;
                int PopupType = 0;
                int AccessType = 0;
                string keySearch = string.Empty;

                //Get (ControlName or ColumnKey) and value
                if (sender.GetType() == typeof(TAUtil.TAGridEditor) || sender.GetType() == typeof(UltraGrid))
                {
                    FieldName = columnKey;
                    controlText = ((TAUtil.TAGridEditor)sender).ActiveCell.Text;
                    senderIsGrid = true;
                }
                else
                {
                    FieldName = ((Control)sender).Name;
                    controlText = ((Control)sender).Text;
                }

                //Get ListID
                if (senderIsGrid)
                    listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, FieldName, ((Control)sender).Name);
                else
                    listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, FieldName);

                //Get PopupType and KeySearch
                switch (FieldName.ToLower())
                {
                    case "altitmid":
                    case "assitmid":
                    case "bomitmid":
                        PopupType = (int)GEnum.PopupType.ItmID;
                        AccessType = (int)GEnum.RecAccessType.ItemID;
                        keySearch = "Itm";
                        break;

                    case "altitmdes": 
                    case "assitmdes": 
                    case "bomitmdes":
                        PopupType = (int)GEnum.PopupType.ItmDes;
                        AccessType = (int)GEnum.RecAccessType.ItemDes;
                        keySearch = "Itm";
                        break;

                    case "masteritmid":
                        //Currently not use
                        PopupType = (int)GEnum.PopupType.ItmMid;
                        AccessType = (int)GEnum.RecAccessType.ItemID;
                        keySearch = "Itm";
                        break;

                    case "masteritmdesc":
                        //Currently not use
                        PopupType = (int)GEnum.PopupType.ItmMdes;
                        AccessType = (int)GEnum.RecAccessType.ItemDes;
                        keySearch = "Itm";
                        break;

                    case "csgvendorkey":
                        PopupType = (int)GEnum.PopupType.VendID;
                        AccessType = (int)GEnum.RecAccessType.VendID;
                        keySearch = "Con";
                        break;

                    case "csgvendornm":
                        PopupType = (int)GEnum.PopupType.VendNm;
                        AccessType = (int)GEnum.RecAccessType.VendNm;
                        keySearch = "Con";
                        break;

                    case "accickey":
                    case "accinkey":
                    case "accphkey":
                    case "accdsickey":                    
                    case "accdsphkey":
                        PopupType = (int)GEnum.PopupType.AccID;
                        AccessType = (int)GEnum.RecAccessType.AccID;
                        keySearch = "Acc";
                        break;

                    case "accicnm":
                    case "accinnm":
                    case "accphnm":
                    case "accdsicdes":                
                    case "accdsphdes":
                        PopupType = (int)GEnum.PopupType.AccDes;
                        AccessType = (int)GEnum.RecAccessType.ItemID;
                        keySearch = "Acc";
                        break;
                    case "substituteitmid":
                        PopupType = (int)GEnum.PopupType.ItmID;
                        AccessType = (int)GEnum.RecAccessType.ItemID;
                        keySearch = "Itm";
                        break;
                    case "substituteitmdes":
                        PopupType = (int)GEnum.PopupType.ItmFSCANid;
                        AccessType = (int)GEnum.RecAccessType.ItemIDSub;
                        keySearch = "Itm";
                        break;
                }

                if (FromButtonClick)
                {
                    if (DocHDRUtil.EditorButton_Popup((int)objMstItmFactory.ConstantCodeKey, controlText, listSettingID, PopupType, ref key, ref id, ref des))
                        RecSearchSelected(sender, FieldName, key, id, des);
                }
                else
                {
                    if (GFunc.IsNE(controlText))
                        //Clear all dependent controls
                        RecSearchSelected(sender, FieldName, key, id, des);
                    else
                    {
                        //Try to match record in server
                        //GFunc.ConRecord_GetKey and GFunc.AccRecord_GetKey is exactly the same, but i still split them up just in case
                        //the code or logic is change in the future version
                        switch (keySearch.ToLower())
                        {
                            case "con":
                                key = GFunc.ConRecord_GetKey((GEnum.RecAccessType)AccessType, listSettingID, controlText, ref id, ref des, false);
                                break;
                            case "acc":
                                key = GFunc.AccRecord_GetKey((GEnum.RecAccessType)AccessType, listSettingID, controlText, ref id, ref des, true);
                                break;
                            default:    //Itm
                                key = GFunc.ItmRecord_GetKey((GEnum.RecAccessType)AccessType, listSettingID, controlText, vendorKey, ref id, ref des, true);
                                break;

                        }
                        if (GFunc.IsNEZ(key))
                        {
                            //since value input by user cannot be match let the user select from Popup form
                            if (DocHDRUtil.EditorButton_Popup((int)objMstItmFactory.ConstantCodeKey, controlText, listSettingID, PopupType, ref key, ref id, ref des))
                                RecSearchSelected(sender, FieldName, key, id, des);
                            else
                            {
                                //when user is still unable to select a matching record, undo the changes
                                MsgBox.Show("Please use a valid value");
                                return false;
                            }
                        }
                        else
                            RecSearchSelected(sender, FieldName, key, id, des);
                    }
                }
                return true;
            }
            catch (TAException tex)
            {
                Error(tex, true);
                return false;
            }
            catch (Exception ex)
            {
                Error(ex, true);
                return false;
            }
        }//Completed
        private void SubstituteItmID_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (objMstItmFactory.IsNew && SubstituteSelected_Prompt() == true)
                e.Cancel = !RecSearchProcess(sender, string.Empty, false);
            else
                e.Cancel = true;
        }//Completed
        private void SubstituteItmDesc_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (objMstItmFactory.IsNew && SubstituteSelected_Prompt() == true)
                e.Cancel = !RecSearchProcess(sender, string.Empty, false);
            else
                e.Cancel = true;
        }//Completed
        private void SubstituteItmID_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            if (objMstItmFactory.IsNew && SubstituteSelected_Prompt() == true)
                RecSearchProcess(sender, string.Empty, true);

        }//Completed
        private void SubstituteItmDesc_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            if (objMstItmFactory.IsNew && SubstituteSelected_Prompt() == true)
                RecSearchProcess(sender, string.Empty, true);

        }//Completed
        private bool SubstituteSelected_Prompt()
        {
            try
            {
                if (MsgBox.Show(MsgID.Common.ConfirmToSubstitute, GEnum.MsgBoxIcon.Alert, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                    return true;
                else
                    return false;
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception tex)
            {
                throw tex;
            }
        }//Completed
        private void CSGVendorNm_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            RecSearchProcess(sender, string.Empty, true);
        }//Completed
        private void CSGVendorNm_CustomUpdate(object sender, CancelEventArgs e)
        {
            e.Cancel = !RecSearchProcess(sender, string.Empty, false);
        }//Completed
        private void CSGVendorKey_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            RecSearchProcess(sender, string.Empty, true);
        }//Completed
        private void CSGVendorKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            e.Cancel = !RecSearchProcess(sender, string.Empty, false);
        }//Completed
        private void AccICKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            e.Cancel = !RecSearchProcess(sender, string.Empty, false);
        }//Completed
        private void AccINKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            e.Cancel = !RecSearchProcess(sender, string.Empty, false);
        }//Completed
        private void AccPHKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            e.Cancel = !RecSearchProcess(sender, string.Empty, false);
        }//Completed
        private void AccPHKey_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            RecSearchProcess(sender, string.Empty, true);
        }//Completed
        private void AccINKey_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            RecSearchProcess(sender, string.Empty, true);
        }//Completed
        private void AccICKey_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            RecSearchProcess(sender, string.Empty, true);
        }//Completed
        private void AccICNm_CustomUpdate(object sender, CancelEventArgs e)
        {
            e.Cancel = !RecSearchProcess(sender, string.Empty, false);
        }//Completed
        private void AccINNm_CustomUpdate(object sender, CancelEventArgs e)
        {
            e.Cancel = !RecSearchProcess(sender, string.Empty, false);
        }//Completed
        private void AccPHNm_CustomUpdate(object sender, CancelEventArgs e)
        {
            e.Cancel = !RecSearchProcess(sender, string.Empty, false);
        }//Completed
        private void AccICNm_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            RecSearchProcess(sender, string.Empty, true);
        }//Completed
        private void AccINNm_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            RecSearchProcess(sender, string.Empty, true);
        }//Completed
        private void AccPHNm_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            RecSearchProcess(sender, string.Empty, true);
        }//Completed

        //Grid Alternate Events
        private void tagrdDetAlternates_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                switch (e.Cell.Column.Key.ToLower())
                {
                    case "altitmid":
                    case "altitmdes": 
                        RecSearchProcess(sender, e.Cell.Column.Key, true);
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
        private void tagrdDetAlternates_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
                UltraGridCell currentCell = tagrdDetAlternates.ActiveCell;

                switch (currentCell.Column.Key.ToLower())
                {
                    #region AltItmID, AltItmDes
                    case "altitmid":
                    case "altitmdes": 
                        if (RecSearchProcess(sender, e.Cell.Column.Key, false))
                        {
                            if (objMstItmFactory.Validation_Detail(tagrdDetAlternates.Name, tagrdDetAlternates.ActiveRow, currentCell.Column.Key))
                                return;
                        }
                        e.Cancel = true;
                        break;
                    #endregion

                    case "altrem":
                    case "custom1":
                    case "custom2":
                    case "custom3":
                        if (objMstItmFactory.Validation_Detail(tagrdDetAlternates.Name, tagrdDetAlternates.ActiveRow, currentCell.Column.Key) == false)
                            e.Cancel = true;
                        break;
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
        private void tagrdDetAlternates_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            try
            {



                if (this.tagrdDetAlternates.ActiveRow != null)
                {
                    if (objMstItmFactory.Validation_Detail(tagrdDetAlternates.Name, tagrdDetAlternates.ActiveRow, string.Empty) == false)
                        e.Cancel = true;
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
        private void tagrdDetAlternates_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                e.DisplayPromptMsg = false;
                Infragistics.Win.UltraWinGrid.UltraGrid ugrd = sender as Infragistics.Win.UltraWinGrid.UltraGrid;
                if (ugrd.Rows.Count <= 0)
                    e.Cancel = true;

                if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) == GEnum.MsgBoxButton.Delete)
                    return;
                else
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void tagrdDetAlternates_AfterRowsDeleted(object sender, EventArgs e)
        {
            objMstItmFactory.ObjMSTItmDetAlts.AcceptChanges();
        }//Completed

        //Grid Assembly Events
        private void tagrdDetAssembly_SelectionDrag(object sender, CancelEventArgs e)
        {
            try
            {
                if (tagrdDetAssembly.ActiveRow != null)
                    tagrdDetAssembly.DoDragDrop(tagrdDetAssembly.Selected.Rows, DragDropEffects.Move);
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
        private void tagrdDetAssembly_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                e.Effect = DragDropEffects.Move;
                UltraGrid grid = sender as UltraGrid;
                Point pointInGridCoords = grid.PointToClient(new Point(e.X, e.Y));
                if (pointInGridCoords.Y < 20)
                    // Scroll up.
                    this.tagrdDetAssembly.ActiveRowScrollRegion.Scroll(RowScrollAction.LineUp);
                else if (pointInGridCoords.Y > grid.Height - 20)
                    // Scroll down.
                    this.tagrdDetAssembly.ActiveRowScrollRegion.Scroll(RowScrollAction.LineDown);
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
        private void tagrdDetAssembly_DragDrop(object sender, DragEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            int dropIndex;
            try
            {
                // Get the position on the grid where the dragged row(s) are to be dropped.
                //get the grid coordinates of the row (the drop zone)
                UIElement uieOver = tagrdDetAssembly.DisplayLayout.UIElement.ElementFromPoint(tagrdDetAssembly.PointToClient(new Point(e.X, e.Y)));

                //get the row that is the drop zone/or where the dragged row is to be dropped
                UltraGridRow ugrOver = uieOver.GetContext(typeof(UltraGridRow), true) as UltraGridRow;
                if (ugrOver != null)
                {
                    dropIndex = ugrOver.Index;    //index/position of drop zone in grid

                    if (dropIndex == -1)
                        dropIndex = tagrdDetAssembly.Rows.Count - 1;

                    //get the dragged row(s)which are to be dragged to another position in the grid
                    SelectedRowsCollection SelRows = (SelectedRowsCollection)e.Data.GetData(typeof(SelectedRowsCollection)) as SelectedRowsCollection;

                    if (dropIndex < SelRows[0].Index)//scroll up
                    {
                        //get the count of selected rows and drop each starting at the dropIndex                  
                        for (int i = SelRows.Count - 1; i >= 0; i--)
                        {
                            tagrdDetAssembly.Rows.Move(SelRows[i], dropIndex);
                        }
                    }
                    else
                    {
                        //get the count of selected rows and drop each starting at the dropIndex                  
                        for (int i = 0; i <= SelRows.Count - 1; i++)
                        {
                            tagrdDetAssembly.Rows.Move(SelRows[i], dropIndex);
                        }
                    }
                }

                //resequence Line Number
                for (int i = 0; i < tagrdDetAssembly.Rows.Count; i++)
                {
                    tagrdDetAssembly.Rows[i].Cells["AssSN"].Value = (decimal)i + 1;
                }
                tagrdDetAssembly.UpdateData();
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
        private void tagrdDetAssembly_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                switch (e.Cell.Column.Key.ToLower())
                {
                    case "assitmid":
                    case "assitmdes":
                        RecSearchProcess(sender, e.Cell.Column.Key, true);
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
        private void tagrdDetAssembly_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {

                UltraGridCell currentCell = tagrdDetAssembly.ActiveCell;

                switch (currentCell.Column.Key.ToLower())
                {
                    #region AssItmID, AssItmDes
                    case "assitmid":
                    //case "assitmdes": commeted by thettm on 14 may 2018
                        if (RecSearchProcess(sender, e.Cell.Column.Key, false))
                        {
                            if (objMstItmFactory.Validation_Detail(tagrdDetAssembly.Name, tagrdDetAssembly.ActiveRow, currentCell.Column.Key))
                                return;
                        }
                        e.Cancel = true;
                        break;
                    #endregion

                    #region AssQty
                    case "assqty":
                        if (objMstItmFactory.Validation_Detail(tagrdDetAssembly.Name, tagrdDetAssembly.ActiveRow, "AssItmType"))
                        {
                            int itmType = (int)tagrdDetAssembly.ActiveRow.Cells["AssItmType"].Value;
                            switch (itmType)
                            {
                                default:
                                    currentCell.Value = GFunc.NEDec(currentCell.Value, 0);
                                    if (objMstItmFactory.Validation_Detail(tagrdDetAssembly.Name, tagrdDetAssembly.ActiveRow, currentCell.Column.Key) == false)
                                        e.Cancel = true;
                                    break;
                            }
                        }
                        else
                            e.Cancel = true;
                        break;
                    #endregion

                    #region DefaultSelection, LockQty, ToPrint, Custom 1,2,3
                    case "defaultselection":
                    case "lockqty":
                    case "toprint":
                    case "custom1":
                    case "custom2":
                    case "custom3":
                        if (objMstItmFactory.Validation_Detail(tagrdDetAssembly.Name, tagrdDetAssembly.ActiveRow, currentCell.Column.Key) == false)
                            e.Cancel = true;
                        break;
                    #endregion
                }

                return;
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
        private void tagrdDetAssembly_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            try
            {



                for (int i = 0; i < tagrdDetAssembly.Rows.Count; i++)
                {
                    tagrdDetAssembly.Rows[i].Cells["AssSN"].Value = (decimal)i + 1;
                }

                if (objMstItmFactory.Validation_Detail(tagrdDetAssembly.Name, tagrdDetAssembly.ActiveRow, string.Empty) == false)
                    e.Cancel = true;

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
        private void tagrdDetAssembly_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                e.DisplayPromptMsg = false;

                if (tagrdDetAssembly.Rows.Count <= 0)
                {
                    e.Cancel = true;
                    return;
                }

                if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) == GEnum.MsgBoxButton.Delete)
                {
                    //Move the cursor position of active row index to upper row
                    if (tagrdDetAssembly.ActiveRow.Index > 0)
                        PreRowIndex = tagrdDetAssembly.ActiveRow.Index - 1;
                    return;
                }
                else
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void tagrdDetAssembly_AfterRowsDeleted(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < tagrdDetAssembly.Rows.Count; i++)
                {
                    tagrdDetAssembly.Rows[i].Cells["AssSN"].Value = (decimal)i + 1;
                }
                objMstItmFactory.ObjMSTItmDetAsss.AcceptChanges();
                if (tagrdDetAssembly.Rows.Count > 0)
                {
                    tagrdDetAssembly.Rows[PreRowIndex].Selected = true;
                    tagrdDetAssembly.Rows[PreRowIndex].Activate();
                    PreRowIndex = 0;
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
        void Grid_AfterRowUpdate(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
        {
            objMstItmFactory.IsDirty = true;
        }

        //Grid Raw Material Events
        private void tagrdItmDetBOMRMs_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                switch (e.Cell.Column.Key.ToLower())
                {
                    case "bomitmid":
                    case "bomitmdes":
                        RecSearchProcess(sender, e.Cell.Column.Key, true);
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
        private void tagrdItmDetBOMRMs_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {

                UltraGridCell curCell = tagrdItmDetBOMRMs.ActiveCell;

                switch (curCell.Column.Key.ToLower())
                {
                    #region BOMItmID, BOMItmDes
                    case "bomitmid":
                    case "bomitmdes":
                        if (RecSearchProcess(sender, e.Cell.Column.Key, false))
                        {
                            if (objMstItmFactory.Validation_Detail(tagrdItmDetBOMRMs.Name, tagrdItmDetBOMRMs.ActiveRow, curCell.Column.Key))
                                return;
                        }
                        e.Cancel = true;
                        break;
                    #endregion

                    #region BOM Qty,LabourCost
                    case "bomqty":
                    case "bomlabourcost":
                        curCell.Value = GFunc.NEDec(curCell.Value, 0);
                        if (objMstItmFactory.Validation_Detail(tagrdItmDetBOMRMs.Name, tagrdItmDetBOMRMs.ActiveRow, curCell.Column.Key) == false)
                            e.Cancel = true;
                        break;
                    #endregion

                    #region Custom1,2,3
                    case "custom1":
                    case "custom2":
                    case "custom3":
                        if (objMstItmFactory.Validation_Detail(tagrdItmDetBOMRMs.Name, tagrdItmDetBOMRMs.ActiveRow, curCell.Column.Key) == false)
                            e.Cancel = true;
                        break;
                    #endregion
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
        private void tagrdItmDetBOMRMs_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            try
            {
                if (objMstItmFactory.Validation_Detail(tagrdItmDetBOMRMs.Name, tagrdItmDetBOMRMs.ActiveRow, string.Empty) == false)
                    e.Cancel = true;

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
        private void tagrdItmDetBOMRMs_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                e.DisplayPromptMsg = false;
                Infragistics.Win.UltraWinGrid.UltraGrid ugrd = sender as Infragistics.Win.UltraWinGrid.UltraGrid;
                if (ugrd.Rows.Count <= 0)
                    e.Cancel = true;

                if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) == GEnum.MsgBoxButton.Delete)
                    return;
                else
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void tagrdItmDetBOMRMs_AfterRowsDeleted(object sender, EventArgs e)
        {
            objMstItmFactory.ObjMSTItmDetBOMRMs.AcceptChanges();
        }//Completed

        //Grid Packing Materials Grid Events
        private void tagrdItmDetBOMPMs_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                switch (e.Cell.Column.Key.ToLower())
                {
                    case "bomitmid":
                    case "bomitmdes":
                        RecSearchProcess(sender, e.Cell.Column.Key, true);
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
        private void tagrdItmDetBOMPMs_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {

                UltraGridCell curCell = tagrdItmDetBOMPMs.ActiveCell;
                switch (curCell.Column.Key.ToLower())
                {
                    #region BOMItmID
                    case "bomitmid":
                    case "bomitmdes":
                        if (RecSearchProcess(sender, e.Cell.Column.Key, false))
                        {
                            if (objMstItmFactory.Validation_Detail(tagrdItmDetBOMPMs.Name, tagrdItmDetBOMPMs.ActiveRow, curCell.Column.Key))
                                return;
                        }
                        e.Cancel = true;
                        break;
                    #endregion

                    #region BOM Qty,LabourCost
                    case "bomqty":
                    case "bomlabourcost":
                        curCell.Value = GFunc.NEDec(curCell.Value, 0);
                        if (objMstItmFactory.Validation_Detail(tagrdItmDetBOMPMs.Name, tagrdItmDetBOMPMs.ActiveRow, curCell.Column.Key) == false)
                            e.Cancel = true;
                        break;
                    #endregion

                    #region Custom1,2,3
                    case "custom1":
                    case "custom2":
                    case "custom3":
                        if (objMstItmFactory.Validation_Detail(tagrdItmDetBOMPMs.Name, tagrdItmDetBOMPMs.ActiveRow, curCell.Column.Key) == false)
                            e.Cancel = true;
                        break;
                    #endregion
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
        private void tagrdItmDetBOMPMs_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            try
            {

                if (objMstItmFactory.Validation_Detail(tagrdItmDetBOMPMs.Name, tagrdItmDetBOMPMs.ActiveRow, string.Empty) == false)
                    e.Cancel = true;

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
        private void tagrdItmDetBOMPMs_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                e.DisplayPromptMsg = false;
                Infragistics.Win.UltraWinGrid.UltraGrid ugrd = sender as Infragistics.Win.UltraWinGrid.UltraGrid;
                if (ugrd.Rows.Count <= 0)
                    e.Cancel = true;

                if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) == GEnum.MsgBoxButton.Delete)
                    return;
                else
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void tagrdItmDetBOMPMs_AfterRowsDeleted(object sender, EventArgs e)
        {
            objMstItmFactory.ObjMSTItmDetBOMPMs.AcceptChanges();
        }//Completed

        //Grid Labour Events
        private void tagrdItmDetBOMLabours_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                switch (e.Cell.Column.Key.ToLower())
                {
                    case "bomitmid":
                    case "bomitmdes":
                        RecSearchProcess(sender, e.Cell.Column.Key, true);
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
        private void tagrdItmDetBOMLabours_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {

                UltraGridCell curCell = tagrdItmDetBOMLabours.ActiveCell;
                switch (curCell.Column.Key.ToLower())
                {
                    #region BOMItmID
                    case "bomitmid":
                    case "bomitmdes":
                        if (RecSearchProcess(sender, e.Cell.Column.Key, false))
                        {
                            if (objMstItmFactory.Validation_Detail(tagrdItmDetBOMLabours.Name, tagrdItmDetBOMLabours.ActiveRow, curCell.Column.Key))
                                return;
                        }
                        e.Cancel = true;
                        break;
                    #endregion

                    #region BOM Qty,LabourCost
                    case "bomqty":
                    case "bomlabourcost":
                        curCell.Value = GFunc.NEDec(curCell.Value, 0);
                        if (objMstItmFactory.Validation_Detail(tagrdItmDetBOMLabours.Name, tagrdItmDetBOMLabours.ActiveRow, curCell.Column.Key) == false)
                            e.Cancel = true;
                        break;
                    #endregion

                    #region Custom1,2,3
                    case "custom1":
                    case "custom2":
                    case "custom3":
                        if (objMstItmFactory.Validation_Detail(tagrdItmDetBOMLabours.Name, tagrdItmDetBOMLabours.ActiveRow, curCell.Column.Key) == false)
                            e.Cancel = true;
                        break;
                    #endregion
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
        private void tagrdItmDetBOMLabours_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            try
            {


                if (objMstItmFactory.Validation_Detail(tagrdItmDetBOMLabours.Name, tagrdItmDetBOMLabours.ActiveRow, string.Empty) == false)
                    e.Cancel = true;
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
        private void tagrdItmDetBOMLabours_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                e.DisplayPromptMsg = false;
                Infragistics.Win.UltraWinGrid.UltraGrid ugrd = sender as Infragistics.Win.UltraWinGrid.UltraGrid;
                if (ugrd.Rows.Count <= 0)
                    e.Cancel = true;

                if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) == GEnum.MsgBoxButton.Delete)
                    return;
                else
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void tagrdItmDetBOMLabours_AfterRowsDeleted(object sender, EventArgs e)
        {
            objMstItmFactory.ObjMSTItmDetBOMLBs.AcceptChanges();
        }//Completed

        //Grid Location Events
        private void tagrdDetLocaton_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {


                UltraGridCell currentCell = tagrdDetLocation.ActiveCell;

                switch (currentCell.Column.Key.ToLower())
                {
                    case "locqtymin":
                    case "locqtymax":
                        currentCell.Value = GFunc.NEDec(currentCell.Value, 0);
                        if (objMstItmFactory.Validation_Detail(tagrdDetLocation.Name, tagrdDetLocation.ActiveRow, currentCell.Column.Key) == false)
                            e.Cancel = true;
                        break;

                    case "custom1":
                    case "custom2":
                    case "custom3":
                        if (objMstItmFactory.Validation_Detail(tagrdDetLocation.Name, tagrdDetLocation.ActiveRow, currentCell.Column.Key) == false)
                            e.Cancel = true;
                        break;
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
        private void tagrdDetLocation_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            try
            {

                if (objMstItmFactory.Validation_Detail(tagrdDetLocation.Name, tagrdDetLocation.ActiveRow, string.Empty) == false)
                    e.Cancel = true;

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
        private void SetPriceGrids()
        {
            try
            {
                #region Setting tagrdPriceList
                for (int i = 0; i < 17; i++)
                {
                    if (i == tagrdPriceList.Rows.Count)
                        tagrdPriceList.DisplayLayout.Bands[0].AddNew();

                    if (i == 0)
                    {
                        tagrdPriceList.Rows[i].Cells["PriceLabel"].Value = "Standard Cost";
                        tagrdPriceList.Rows[i].Cells["Ratio"].Value = DBNull.Value;
                    }
                    else if (i == 1)
                    {
                        tagrdPriceList.Rows[i].Cells["PriceLabel"].Value = "Standard Price";
                        tagrdPriceList.Rows[i].Cells["Ratio"].Value = DBNull.Value;
                    }
                    else
                    {
                        tagrdPriceList.Rows[i].Cells["PriceLabel"].Value = "Price " + (i - 1).ToString();
                    }
                }

                tagrdPriceList.DisplayLayout.Bands[0].Columns["PriceLabel"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                tagrdPriceList.DisplayLayout.Bands[0].Columns["PriceLabel"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
                //tagrdPriceList.DisplayLayout.Bands[0].Columns["PriceLabel"].CellAppearance.ForeColorDisabled = System.Drawing.Color.Black;
                //tagrdPriceList.DisplayLayout.Bands[0].Columns["PriceLabel"].CellAppearance.BackColor = System.Drawing.Color.AliceBlue;

                tagrdPriceList.DisplayLayout.Bands[0].Columns["PriceLabel"].Header.Caption = "";
                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price01"].Header.Caption = "P1 " +
                        GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr1));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price02"].Header.Caption = "P2 " +
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr2));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price03"].Header.Caption = "P3 " +
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr3));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price04"].Header.Caption = "P4 " +
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr4));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price05"].Header.Caption = "P5 " +
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr5));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price06"].Header.Caption = "P6 " +
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr6));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price07"].Header.Caption = "P7 " +
                     GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr7));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price08"].Header.Caption = "P8 " +
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr8));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price09"].Header.Caption = "P9 " +
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr9));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price10"].Header.Caption = "P10 " +
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr10));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price11"].Header.Caption = "P11 " +
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr11));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price12"].Header.Caption = "P12 " +
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr12));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price13"].Header.Caption = "P13 " +
                        GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr13));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price14"].Header.Caption = "P14 " +
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr14));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price15"].Header.Caption = "P15 " +
                       GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr15));

                ////DateTime dt;


                tagrdPriceList.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.False;
                tagrdPriceList.DisplayLayout.Bands[0].Override.AllowAddNew = AllowAddNew.No;
                tagrdPriceList.DisplayLayout.Bands[0].Columns[0].Header.Fixed = true;
                tagrdPriceList.DisplayLayout.Bands[0].Columns["StdCost"].Hidden = true;

                #endregion

                #region Setting tagrdQtyRatio
                for (int i = 0; i < 5; i++)
                {
                    if (i == tagrdQtyRatio.Rows.Count)
                        tagrdQtyRatio.DisplayLayout.Bands[0].AddNew();

                    tagrdQtyRatio.Rows[i].Cells["Label"].Value = (i + 1);
                }
                tagrdQtyRatio.DisplayLayout.Bands[0].Columns["Label"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                tagrdQtyRatio.DisplayLayout.Bands[0].Columns["Label"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
                tagrdQtyRatio.DisplayLayout.Bands[0].Columns["Label"].CellAppearance.ForeColorDisabled = System.Drawing.Color.Black;
                //tagrdQtyRatio.DisplayLayout.Bands[0].Columns["Label"].CellAppearance.BackColor = System.Drawing.Color.AliceBlue;

                tagrdQtyRatio.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.False;

                //for (int i = 0; i < tagrdQtyRatio.DisplayLayout.Bands[0].Columns.Count; i++)
                //{
                //    tagrdQtyRatio.DisplayLayout.Bands[0].Columns[i].Header.Appearance.FontData.Bold = DefaultableBoolean.True;
                //    tagrdQtyRatio.DisplayLayout.Bands[0].Columns[i].Header.Appearance.TextHAlign = HAlign.Center;
                //    tagrdQtyRatio.DisplayLayout.Bands[0].Columns[i].Header.Appearance.FontData.Underline = DefaultableBoolean.True;
                //}
                #endregion
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
        private void tagrdPriceList_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
                //Declaration
                decimal vRatio = 0M;
                decimal vStdPrice = 0M;
                int PriceDec = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceDecPlace);
                int PriceRoundMode = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceRoundMode);

                //All Cell changes made by the user will need to validate and round off
                decimal cellvalue = GFunc.RndC(e.Cell.Value, GVar.RndDecs.Prcpt);
                if (cellvalue < 0)
                {
                    MsgBox.Show("Value must be >=0");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    e.Cell.Value = cellvalue;
                    this.objMstItmFactory.IsDirty = true;
                }

                //Get the Cell that the user has change
                int RowIndex = e.Cell.Row.Index;
                int ColIndex = e.Cell.Column.Index;

                //User has made changes to Standard Price
                //User made changes at Row 1 (which is the standard price) on column (Price 1 to 15 or StdCost)
                //when this happen we will need to update the remaining row for price 1 to 15 base on the ratio
                if (RowIndex == 1 && ColIndex > 1)
                {
                    vStdPrice = cellvalue;

                    for (int i = 1; i < 16; i++)
                    {
                        vRatio = GFunc.RndDC(GFunc.NEDec(tagrdPriceList.Rows[i].Cells["Ratio"].Value, 0), 100, GVar.RndDecs.Prcpt);
                        tagrdPriceList.Rows[i].Cells[ColIndex].Value = GFunc.RndUD(vStdPrice * (1 + vRatio), PriceRoundMode, PriceDec);
                    }
                    tagrdPriceList.UpdateData();
                }

                //User has made changes to the Ratio
                //We need to update the column (price 1 ot 15 and StdCost) for the active row
                else if (RowIndex > 0)
                {
                    if (ColIndex == 1)
                    {
                        vRatio = GFunc.RndDC(cellvalue, 100, GVar.RndDecs.Prcpt);

                        for (int j = 2; j < tagrdPriceList.DisplayLayout.Bands[0].Columns.Count; j++)
                        {
                            //Get standard  price at Row 1
                            vStdPrice = GFunc.NEDec(tagrdPriceList.Rows[1].Cells[j].Value, 0);
                            tagrdPriceList.Rows[RowIndex].Cells[j].Value = GFunc.RndUD(vStdPrice * (1 + vRatio), PriceRoundMode, PriceDec);
                        }
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
        private void tagrdQtyRatio_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
                //Validation and Rounding
                decimal cellvalue = GFunc.RndC(e.Cell.Value, GVar.RndDecs.Prcpt);
                if (cellvalue < 0)
                {
                    MsgBox.Show("Value must be >=0");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    e.Cell.Value = cellvalue;
                    this.objMstItmFactory.IsDirty = true;
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

        //Attached Events
        private void ErrorNotifier_Clear(object sender, BOLib.UINotifierEventArgs e)
        {
            this.errorProvider1.Clear();
            DocComUtility.ClearErrNotifier(this, e, errorProvider1);
        }
        private void ErrorNotifier_Set(object sender, BOLib.UINotifierEventArgs e)
        {
            string propertyNm = string.Empty;
            string conNm = string.Empty;
            Control co;
            try
            {
                //For ErrorProvider
                foreach (object key in e.PropertyMessage.Keys)
                {
                    conNm = key.ToString();
                    co = this.Controls.Find(conNm, true)[0];
                    this.errorProvider1.SetError(co, e.PropertyMessage[key].ToString());
                }


                //For Focus
                foreach (object key in e.PropertyMessage.Keys)
                {
                    conNm = key.ToString();
                    switch (conNm.ToLower())
                    {
                        case "weightuomkey":
                            tabDetList.Tabs["tabPageMeasurement"].Selected = true;
                            break;
                        case "buomkey":
                            tabDetList.Tabs["tabPageGeneral"].Selected = true;
                            break;
                        case "accickey":
                        case "accinkey":
                        case "accphkey":
                            tabDetList.Tabs["tabPageDefault"].Selected = true;
                            break;
                        case "bomtype":
                        case "defaultexpdate":
                            tabDetList.Tabs["tabPageBOM"].Selected = true;
                            break;
                        default:
                            break;
                    }
                    co = this.Controls.Find(conNm, true)[0];
                    co.Focus();
                    break;
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
        }

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
                            GlobalUI.ItemNotInList(grd.ActiveCell, null, 0);
                        }
                    }
                }
                if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_NUMERIC)
                {
                    throw new TAException(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
                }
                if (e.ErrorCode == TAUtil.TAErrorCode.FORMULA_NOT_RECOGNIZE)
                {
                    throw new TAException("FORMULA NOT RECOGNIZE");
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
                        string ActiveColKey = "";
                        if (((TAUtil.TAGridEditor)this.ActiveControl).ActiveCell != null)
                        {
                            ActiveColKey = GFunc.GridColumnKey_Get(this.ActiveControl);
                        }
                        l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, ActiveColKey });
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

        private void tabDetList_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)//Not ready Pauk
        {
            //We need to set selected Tab into front after changing tab enabled and disable
            e.Tab.TabPage.BringToFront();
        }

        private void OpenItmID_CustomUpdate(object sender, CancelEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;

            try
            {
                if (GFunc.IsNE(OpenItmID.Text) == false)
                {
                    string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, OpenItmID.Name);
                    key = GFunc.ItmRecord_GetKey(GEnum.RecAccessType.ItemID, listSettingID, OpenItmID.Text, 0, ref id, ref des, true);
                    if (GFunc.IsNEZ(key))
                    {
                        if (DocHDRUtil.EditorButton_Popup((int)objMstItmFactory.ConstantCodeKey, OpenItmID.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref key, ref id, ref des) == false)
                            return;
                    }
                    OpenItmID.SetValueTrigger(id, false);
                    this.OnListRecordSelected(key);
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

        private void OpenItmID_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;

            try
            {
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, OpenItmID.Name);
                if (DocHDRUtil.EditorButton_Popup((int)objMstItmFactory.ConstantCodeKey, OpenItmID.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref key, ref id, ref des))
                {
                    OpenItmID.SetValueTrigger(id, false);
                    OnListRecordSelected(key);
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

        private void tagrdCertificate_ClickCell(object sender, ClickCellEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Cell.Text);
        }

        private void AccDSICKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            e.Cancel = !RecSearchProcess(sender, string.Empty, false);
        }

        private void AccDSICDes_CustomUpdate(object sender, CancelEventArgs e)
        {
            e.Cancel = !RecSearchProcess(sender, string.Empty, false);
        }

        private void AccDSPHKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            e.Cancel = !RecSearchProcess(sender, string.Empty, false);
        }

        private void AccDSPHDes_CustomUpdate(object sender, CancelEventArgs e)
        {
            e.Cancel = !RecSearchProcess(sender, string.Empty, false);
        }

        private void AccDSICDes_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            RecSearchProcess(sender, string.Empty, true);
        }

        private void AccDSPHDes_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            RecSearchProcess(sender, string.Empty, true);
        }
        //added by thettm on 30 Aug 2018(start)
        private void optDP_CheckedChanged(object sender, EventArgs e)
        {
            if (optDP.Checked == true)
                objMstItmFactory.ObjMSTItm.INClass = "DIRECT PICKING";
            else
                objMstItmFactory.ObjMSTItm.INClass = "KITTING ASSEMBLY";
        }

        #region // commented by YST on 2022/05/18
        //added by nnt on 04 Sept 2019 (start)
        /*
        private void CatKey1_CustomUpdate(object sender, CancelEventArgs e)
        {
            int CatKey = 0;
            CatKey =   Convert.ToInt32(CatKey1.Value);
            System.Data.DataTable dsResult = new System.Data.DataTable();            
            dsResult = GetCatAcc(CatKey);
            if ((CatKey == 0) || (dsResult.Rows.Count == 0)) MsgBox.Show("No Match for Account Group. Please choose related Account Group or Others Account Group.");
            else
            {
                AccICKey.Value = dsResult.Rows[0]["AccKey"];
                AccICNm.Text = dsResult.Rows[0]["AccDes"].ToString();
                AccINKey.Value = dsResult.Rows[1]["AccKey"];
                AccINNm.Value = dsResult.Rows[1]["AccDes"].ToString();
                AccPHKey.Value = dsResult.Rows[2]["AccKey"];
                AccPHNm.Value = dsResult.Rows[2]["AccDes"].ToString();
                AccDSICKey.Value = dsResult.Rows[3]["AccKey"];
                AccDSICDes.Value = dsResult.Rows[3]["AccDes"].ToString();
                AccDSPHKey.Value = dsResult.Rows[4]["AccKey"];
                AccDSPHDes.Value = dsResult.Rows[4]["AccDes"].ToString();
            }
        }
        */
        #endregion

        private DataTable GetCatAcc(int CatKey)
        {
            
            System.Data.DataSet dsResult = new System.Data.DataSet();
            using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(BOLib.Database.BossDemoConnection))
            {
                System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand();
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.CommandTimeout = 0;
                cm.CommandText = "CatAccGroup_Get";
                cm.Parameters.AddWithValue("@CatID", CatKey);
                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                try
                {
                    sqlCon.Open();
                    sqlAdp.Fill(dsResult);
                    return dsResult.Tables[0];
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }

        }
        //added by nnt on 04 sept 2019(end)

        /* added by YST on 2022/05/18 (start) */
        private void BindDefaultAcctByCatKey1()
        {
            try
            {
                int CatKey1 = 0;
                if (this.CatKey1.Value != null ) int.TryParse(this.CatKey1.Value.ToString(), out CatKey1);
                dtDefaultAcc = GetCatAcc(CatKey1);

                if (CatKey1 == 0)
                {
                    MsgBox.Show("Please choose the correct Category.");
                }
                else if (dtDefaultAcc != null && dtDefaultAcc.Rows.Count == 0)
                {
                    GEnum.MsgBoxButton btnResult = MsgBox.Show("There are no default accounts for the selected category. <br/> Are you sure to continue ?", GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No);
                    if (btnResult == GEnum.MsgBoxButton.No)
                    {
                        return;
                    }
                }
                else
                {
                    foreach (DataRow dr in dtDefaultAcc.Rows)
                    {
                        switch (dr["AccContrlName"].ToString())
                        {
                            case "AccICKey":
                                AccICKey.Value = dr["AccKey"];
                                AccICNm.Value = dr["AccDes"];
                                objMstItmFactory.ObjMSTItm.AccICKey = Convert.ToInt32(dr["AccKey"]);
                                objMstItmFactory.ObjMSTItm.AccICID = dr["AccID"].ToString();
                                break;
                            case "AccINKey":
                                AccINKey.Value = dr["AccKey"];
                                AccINNm.Value = dr["AccDes"];
                                objMstItmFactory.ObjMSTItm.AccINKey = Convert.ToInt32(dr["AccKey"]);
                                objMstItmFactory.ObjMSTItm.AccINID = dr["AccID"].ToString();
                                break;
                            case "AccPHKey":
                                AccPHKey.Value = dr["AccKey"];
                                AccPHNm.Value = dr["AccDes"];
                                objMstItmFactory.ObjMSTItm.AccPHKey = Convert.ToInt32(dr["AccKey"]);
                                objMstItmFactory.ObjMSTItm.AccPHID = dr["AccID"].ToString();
                                break;
                            case "AccDSICKey":
                                AccDSICKey.Value = dr["AccKey"];
                                AccDSICDes.Value = dr["AccDes"];
                                objMstItmFactory.ObjMSTItm.AccDSICKey = Convert.ToInt32(dr["AccKey"]);
                                objMstItmFactory.ObjMSTItm.AccDSICID = dr["AccID"].ToString();
                                break;
                            case "AccDSPHKey":
                                AccDSPHKey.Value = dr["AccKey"];
                                AccDSPHDes.Value = dr["AccDes"];
                                objMstItmFactory.ObjMSTItm.AccDSPHKey = Convert.ToInt32(dr["AccKey"]);
                                objMstItmFactory.ObjMSTItm.AccDSPHID = dr["AccID"].ToString();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.ToString());
            }
        }
        private void CatKey1_CustomUpdate(object sender, CancelEventArgs e)
        {
            BindDefaultAcctByCatKey1();
        }
        /* added by YST on 2022/05/18 (end) */

        private void btnSerialGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string msgID = string.Empty;
                string prefixSerial = string.Empty;
                string prefixMACID = string.Empty;
                string prefixSerialCompare = string.Empty;
                string prefixMACIDCompare = string.Empty;
                string SerialID = string.Empty;
                string MACID = string.Empty;
                int MACIDINT = 0;
                int Qty = 0;

                string lastSerialNo = string.Empty;
                string lastMACIDNo = string.Empty;
                string lastSerialNoCompare = string.Empty;
                string lastMACIDNoCompare = string.Empty;

                lastSerialNoSave = string.Empty;
                lastMACIDNoSave = string.Empty;
                lastBBID = string.Empty;

                #region Validation check                

                if (!GFunc.IsNE(_dtItmSerial))
                    if (_dtItmSerial.Rows.Count > 0)
                        _dtItmSerial.Rows.Clear();

                if (objMstItmFactory.ObjMSTItm.ItmKey == 0)
                {
                    msgID += "Select Item ID";
                }
                if (msgID != string.Empty)
                {
                    MsgBox.Show(msgID);
                    return;
                }
                msgID = string.Empty;
                if (GFunc.IsNE(MFNNumber.Value))
                {
                    msgID += "Manufacturer Number cannot be blank.<br />";
                                        
                }
                if (GFunc.IsNE(QtyToGenerate.Value))
                {
                    msgID += "Quantity to generate serial no. cannot be blank.";

                }
                if (msgID != string.Empty)
                {
                    MsgBox.Show(msgID);
                    MFNNumber.Focus();
                    return;
                }
                if (QtyToGenerate.Value.ToString() == "0")
                {
                    btnSerialSave.Enabled = false;
                    MsgBox.Show("Qty to generate serial no. must be greater than zero.");
                    QtyToGenerate.Focus();
                    return;
                }

                _dtItmSerial = objMstItmFactory.GetSerialList(objMstItmFactory.ObjMSTItm.ItmKey);

                if (_dtItmSerial.Rows.Count > 0)
                {
                    DataRow[] row = _dtItmSerial.Select() ;
                    if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                    {
                        row = _dtItmSerial.Select("BatchID='" + MFNNumber.Value + "' AND Custom3='Singapore'", "SerialID DESC");
                    }
                    else if (SysOptionUtility.DatabaseBranchCode == "OMSTW" )
                    {
                        row = _dtItmSerial.Select("BatchID='" + MFNNumber.Value + "' AND Custom3='Taiwan'", "SerialID DESC");
                    }

                    if (row.Count()> 0)
                    {
                        MsgBox.Show("Duplicate MFN Number '" + MFNNumber.Value + "' for Item ID '" + objMstItmFactory.ObjMSTItm.ItmID + "'.");
                        MFNNumber.Focus();
                        return;
                    }
                }


                #endregion

                #region Get prefix and last counter number

                //_dtItmSerial = objMstItmFactory.GetSerialList(objMstItmFactory.ObjMSTItm.ItmKey);
            
                if (GFunc.IsNE(_dtItmSerial))
                {
                    return;
                }
                    
                if (objMstItmFactory.IsNew || _dtItmSerial.Rows.Count==0)
                {
                    if (GFunc.IsNE(Custom1.Value))
                    {
                        MsgBox.Show("Prefix Serial No. cannot be blank.");
                        tabDetList.SelectedTab = tabDetList.Tabs["tabPageOthers"];
                        Custom1.Focus();
                        return;
                    }
                   
                    if (GFunc.IsNEZ(Custom1.Value))
                    {
                        MsgBox.Show("Prefix Serial No. should be numeric.");
                        tabDetList.SelectedTab = tabDetList.Tabs["tabPageOthers"];
                        Custom1.Focus();
                        return;
                    }

                    prefixSerial = Custom1.Value.ToString();

                    if (GFunc.IsNE(Custom5.Value))
                    {
                        MsgBox.Show("Last Suffix Serial No. cannot be blank.");
                        tabDetList.SelectedTab = tabDetList.Tabs["tabPageOthers"];
                        Custom5.Focus();
                        return;
                    }

                    if (Custom5.Value.ToString() == "0000") Custom5.Value = "0";

                    if (GFunc.IsNEZ(Custom5.Value) && Custom5.Value.ToString() !="0")
                    {
                        MsgBox.Show("Last Suffix Serial No. should be numeric.");
                        tabDetList.SelectedTab = tabDetList.Tabs["tabPageOthers"];
                        Custom5.Focus();
                        return;
                    }

                    lastSerialNo = Custom5.Value.ToString();

                    if (!GFunc.IsNE(Custom2.Value))
                    {
                        prefixMACID = Custom2.Value.ToString();
                    }

                    if (!GFunc.IsNE(Custom6.Value))
                    {
                        lastMACIDNo = Custom6.Value.ToString();
                    }

                }
                else
                {
                    DataRow[] row = _dtItmSerial.Select();
                    if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                    {
                        row = _dtItmSerial.Select("Custom3='Singapore'", "SerialID DESC");
                    }
                    else if (SysOptionUtility.DatabaseBranchCode == "OMSTW")
                    {
                        row = _dtItmSerial.Select("Custom3='Taiwan'", "SerialID DESC");
                    }

                    if (row.Count() > 0)
                    {                       
                        prefixSerialCompare = GFunc.NEStr(row[0]["SerialID"].ToString(), "");
                        prefixSerialCompare = prefixSerialCompare.Substring(0,4);

                        lastSerialNoCompare = GFunc.NEStr(row[0]["SerialID"].ToString(), "");
                        lastSerialNoCompare = lastSerialNoCompare.Substring(lastSerialNoCompare.Length - 4, 4);

                        prefixMACIDCompare = GFunc.NEStr(row[0]["MACAddress"].ToString(), "");
                        if (prefixMACIDCompare != "")
                        {
                            prefixMACIDCompare = prefixMACIDCompare.Substring(0,prefixMACIDCompare.Length - 6);
                        }

                        //lastMACIDNoCompare = GFunc.NEStr(row[0]["MACAddress"].ToString(), "");
                        //if (lastMACIDNoCompare != "")
                        //{
                        //    lastMACIDNoCompare = lastMACIDNoCompare.Substring(lastMACIDNoCompare.Length - 5, 2) + lastMACIDNoCompare.Substring(lastMACIDNoCompare.Length - 2, 2);
                        //}
                    }
                    prefixSerial = GFunc.NEStr(Custom1.Value, "");
                    lastSerialNo = GFunc.NEStr(Custom5.Value, "");
                    prefixMACID = GFunc.NEStr(Custom2.Value, "");
                    lastMACIDNo = GFunc.NEStr(Custom6.Value, "");
                    if (!GFunc.IsNEZ(Custom8.Value))
                    {
                        lastBBID = Custom8.Value.ToString(); ;
                    }
                    else
                    {
                        lastBBID = string.Empty;
                    }

                    if(!GFunc.IsNE(prefixSerialCompare))
                    { 
                        if (prefixSerial != prefixSerialCompare)
                        {
                            MsgBox.Show("Prefix serial No. is " + prefixSerialCompare + "<br /> Please check ''Prefix (Serial No.)'' ");
                            tabDetList.SelectedTab = tabDetList.Tabs["tabPageOthers"];
                            Custom1.Focus();
                            return;
                        }
                    }

                    if (!GFunc.IsNE(lastSerialNoCompare))
                    {
                        if (lastSerialNo != lastSerialNoCompare)
                        {
                            MsgBox.Show("The last number suffix Serial No. is " + lastSerialNoCompare + "<br /> Please check ''Last number suffix (Serial No.)'' ");
                            tabDetList.SelectedTab = tabDetList.Tabs["tabPageOthers"];
                            Custom5.Focus();
                            return;
                        }
                    }

                    if (!GFunc.IsNE(prefixMACIDCompare))
                    {
                        if (prefixMACID != prefixMACIDCompare)
                        {
                            MsgBox.Show("The prefix MAC I/D is " + prefixMACIDCompare + "<br /> Please check ''Prefix (MAC I/D)'' ");
                            tabDetList.SelectedTab = tabDetList.Tabs["tabPageOthers"];
                            Custom2.Focus();
                            return;
                        }

                    }

                    if (!GFunc.IsNE(lastMACIDNoCompare))
                    {
                        if (GFunc.IsNEZ(lastMACIDNo))
                        {
                            MsgBox.Show("The last number suffix MAC I/D should be hexadecimal ");
                            tabDetList.SelectedTab = tabDetList.Tabs["tabPageOthers"];
                            Custom6.Focus();
                            return;
                        }
                        //if (lastMACIDNo != lastMACIDNoCompare)
                        //{
                        //    MsgBox.Show("The last number suffix MAC I/D is " + lastMACIDNoCompare + "<br /> Please check ''Last number suffix (MAC I/D)'' ");
                        //    tabDetList.SelectedTab = tabDetList.Tabs["tabPageOthers"];
                        //    Custom6.Focus();
                        //    return;
                        //}
                    }                  

                }
                #endregion

                #region Generate Serial no. and MACID no.

                if (!GFunc.IsNE(_dtItmSerial))
                    if (_dtItmSerial.Rows.Count > 0)
                        _dtItmSerial.Rows.Clear();

                Qty=GFunc.NEInt(QtyToGenerate.Value, 0);


                for (int i = 1; i <= Qty; i++)
                {
                    DataRow drSelect = _dtItmSerial.NewRow();

                    drSelect["ItmKey"] = objMstItmFactory.ObjMSTItm.ItmKey;

                    lastSerialNoSave = (GFunc.NEInt(lastSerialNo, 0) + i).ToString();
                    lastSerialNoSave = lastSerialNoSave.PadLeft(4, '0');
                    SerialID = prefixSerial + lastSerialNoSave;
                    drSelect["SerialID"] = SerialID;

                    if (!GFunc.IsNE(prefixMACID) && !GFunc.IsNE(lastMACIDNo))
                    {
                        try
                        {
                            MACIDINT = int.Parse(lastMACIDNo, System.Globalization.NumberStyles.HexNumber);
                            MACIDINT = (GFunc.NEInt(MACIDINT, 0) + i);
                            lastMACIDNoSave = MACIDINT.ToString("X");
                            lastMACIDNoSave = lastMACIDNoSave.PadLeft(4, '0');
                            MACID = prefixMACID + ":" + lastMACIDNoSave.Substring(0, 2) + ":" + lastMACIDNoSave.Substring(2, 2);
                        }
                        catch (Exception ex)
                        {
                            MsgBox.Show("The last number suffix Serial No. is not a hexadecimal.");
                            return;
                        }
                    }

                    if (!GFunc.IsNEZ(Custom8.Value))
                    {
                        lastBBID=(GFunc.NEInt(Custom8.Value, 0) + i).ToString();
                    }

                    drSelect["MACAddress"] = MACID;
                    drSelect["MfgDate"] = DateTime.Today.Date;
                    drSelect["ExpiryDate"] = DateTime.Today.Date.AddYears(1);
                    drSelect["ItmStatus"] = 1;
                    drSelect["BatchID"] = MFNNumber.Value.ToString();
                    drSelect["Custom2"] = lastBBID;

                    //Add Branch name (Singapore/Taiwan) when generate serial no.
                    if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                    {
                        drSelect["Custom3"] = "Singapore";
                    }
                    else  if (SysOptionUtility.DatabaseBranchCode == "OMSTW")
                    {
                        drSelect["Custom3"] = "Taiwan";
                    }

                    _dtItmSerial.Rows.Add(drSelect);
                    _dtItmSerial.AcceptChanges();
                }


                tagrdDetSerials.DataSource = _dtItmSerial;
                tagrdDetSerials.DataBind();             
                btnSerialSave.Enabled = true;
                btnSerialPreview.Enabled = false;

                #endregion
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
                tabDetList.SelectedTab = tabDetList.Tabs["tabPageOthers"];
            }
        }

        private void btnSerialSave_Click(object sender, EventArgs e)
        {
            try
            {
                #region Save Serial No. and MACI/D No.



                if (!GFunc.IsNE(_dtItmSerial))                                  
                {
                    if (_dtItmSerial.Rows.Count==0 )
                    {
                        MsgBox.Show("Please generate serial numbers before save");
                        return;
                    }
                }

                if (MsgBox.Show("Are you sure to save (Y/N)?<br />Please confrim generated Serial no. before Saving.<br />", GEnum.MsgBoxIcon.Alert, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                return;

                int Qty = GFunc.NEInt(QtyToGenerate.Value, 0);

                if (objMstItmFactory.SaveSerialList(MFNNumber.Value.ToString(), Qty, _dtItmSerial, lastSerialNoSave, lastMACIDNoSave, lastBBID))
                {
                    MsgBox.Show("Serial No. generated save successfully.");

                    btnSerialSave.Enabled = false;
                    btnSerialPreview.Enabled = true;
                    MFNNumber.SetValueTrigger(string.Empty, false);
                    QtyToGenerate.SetValueTrigger(string.Empty, false);

                }               
                
                #endregion
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

        private void btnSerialPreview_Click(object sender, EventArgs e)
        {
            try
            { 
                string SerialFrom = string.Empty;
                string SerialTo = string.Empty;

                if (tagrdDetSerials.Rows.Count == 0)
                {
                    MsgBox.Show("Select Item ID to Preview Serial No."); 
                    return;
                }
                ReportLoader _ReportLoader = new ReportLoader();

                string RptName =GFunc.NEStr(Custom7.Value,"");
            
                if(RptName=="")
                {
                    MsgBox.Show("Please select a barcode format.");
                    tabDetList.SelectedTab = tabDetList.Tabs["tabPageOthers"];
                    Custom7.Focus();
                    return;
                }

                if (tagrdDetSerials.Selected.Rows.Count>0)
                {        
                    SerialFrom = tagrdDetSerials.Selected.Rows[0].Cells["SerialID"].Value.ToString();
                    SerialTo = tagrdDetSerials.Selected.Rows[tagrdDetSerials.Selected.Rows.Count - 1].Cells["SerialID"].Value.ToString();
                    if (GFunc.NEInt(SerialFrom, 0) > GFunc.NEInt(SerialTo, 0))
                    {
                        SerialFrom = tagrdDetSerials.Selected.Rows[tagrdDetSerials.Selected.Rows.Count - 1].Cells["SerialID"].Value.ToString();
                        SerialTo = tagrdDetSerials.Selected.Rows[0].Cells["SerialID"].Value.ToString();                   
                    }                
                }
                else
                {
                    MsgBox.Show("Please select line.");
                    return;
                }

                frmMain.gfrmMain.SetNotifyStatus("Loading Report ......");

                CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rptDoc.Load(Application.StartupPath + @"\Reports\" + RptName);

                DataTable dt= objMstItmFactory.GetSerialList(objMstItmFactory.ObjMSTItm.ItmKey,SerialFrom,SerialTo);
                rptDoc.SetDataSource(dt);

                frmReportViewer fRptViewer = new frmReportViewer();
                fRptViewer.RepKey = (int)objMstItmFactory.ConstantCodeKey;
                fRptViewer.RptName = RptName;
                fRptViewer.RptDocument = rptDoc;
                fRptViewer.MdiParent = frmMain.gfrmMain;
                fRptViewer.Show();
                frmMain.gfrmMain.SetNotifyStatus("");
                _ReportLoader = null;
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

        private void tagrdDetSerials_CellChange(object sender, CellEventArgs e)
        {
            bool itmStatus = true;
            string custom1 = string.Empty;
            string custom2 = string.Empty;
            if(e.Cell.Column.ToString()=="ItmStatus")
            { 
                if (GFunc.NEBool(e.Cell.Value, false) == true)
                {
                    itmStatus = false;
                }
                else
                {
                    itmStatus = true;
                }
                custom1 = GFunc.NEStr(e.Cell.Row.Cells["Custom1"].Value, "");
                custom2 = GFunc.NEStr(e.Cell.Row.Cells["Custom2"].Value, "");
                objMstItmFactory.UpdateItmStatus(GFunc.NEInt(e.Cell.Row.Cells["SerialKey"].Value, 0), itmStatus, custom1,custom2);
            }
            
        }

        private void tagrdDetSerials_AfterCellUpdate(object sender, CellEventArgs e)
        {
            bool itmStatus = true;
            string custom1 = string.Empty;
            string custom2 = string.Empty;
            if (e.Cell.Column.ToString() == "Custom1" || e.Cell.Column.ToString() == "Custom2")
            {
                custom1 = GFunc.NEStr(e.Cell.Row.Cells["Custom1"].Value, "");
                custom2 = GFunc.NEStr(e.Cell.Row.Cells["Custom2"].Value, "");
                itmStatus = GFunc.NEBool(e.Cell.Row.Cells["ItmStatus"].Value, true);
                objMstItmFactory.UpdateItmStatus(GFunc.NEInt(e.Cell.Row.Cells["SerialKey"].Value, 0), itmStatus, custom1,custom2);
            }
        }

        private void btnSerialSaveOne_Click(object sender, EventArgs e)
        {

            try
            {
                #region Validation Check

                string lastserialNo = string.Empty;
                string prefixSerialNo = string.Empty;
                string prefixMACID = string.Empty;

                DataTable dt = null;

                if (objMstItmFactory.ObjMSTItm.ItmKey == 0)
                {
                    MsgBox.Show("Select Item ID");
                    ItmID.Focus();
                    return;
                }

                dt = objMstItmFactory.GetSerialList(objMstItmFactory.ObjMSTItm.ItmKey);
                if (dt.Rows.Count == 0)
                {
                    MsgBox.Show("Please use ''Auto Generate Serial No.''");
                    return;
                }           

                if (GFunc.IsNE(MFNNo.Value))
                {
                    MsgBox.Show("Manufacturer Number cannot be blank.");
                    MFNNo.Focus();
                    return;

                }

                dt = objMstItmFactory.GetSerialList(objMstItmFactory.ObjMSTItm.ItmKey);
                if (dt.Rows.Count > 0)
                {
                    DataRow[] row = dt.Select("BatchID='" + MFNNo.Value + "'", "SerialID DESC");
                    if (row.Count() > 0)
                    {
                        MsgBox.Show("Duplicate MFN Number '" + MFNNo.Value + "' for Item ID '" + objMstItmFactory.ObjMSTItm.ItmID + "'.");
                        MFNNumber.Focus();
                        return;
                    }
                }

                if (GFunc.IsNE(SerialNo.Value))
                {
                    MsgBox.Show("Serial no. cannot be blank.");
                    SerialNo.Focus();
                    return;
                }

                if (SerialNo.Value.ToString().Length != 8)
                {
                    MsgBox.Show("Serial no. should be eight digit. Please check the Serial no. '" + SerialNo.Value + "'");
                    SerialNo.Focus();
                    return;
                }

                if (GFunc.IsNEZ(SerialNo.Value.ToString().Substring(4, 4)))
                {
                    MsgBox.Show("Invalid number. Plese check Serial no. '" + SerialNo.Value + "'.");
                    SerialNo.Focus();
                    return;
                }

                prefixSerialNo = GFunc.NEStr(objMstItmFactory.ObjMSTItm.Custom1, string.Empty);

                if (SerialNo.Value.ToString().Substring(0, 4) != prefixSerialNo)
                {
                    MsgBox.Show("Plese check Serial no. '" + SerialNo.Value + "'. Prefix should be '" + prefixSerialNo + "'");
                    SerialNo.Focus();
                    return;
                }                          
                               
                if (dt.Rows.Count > 0)
                {
                    DataRow[] row = dt.Select("SerialID=" + SerialNo.Value.ToString());
                    if (row.Count()!=0)
                    {
                        MsgBox.Show("Serial No. '" + SerialNo.Value.ToString() + "' already exist.");
                        return;
                    }
                    row = dt.Select("", "SerialID DESC");
                    lastserialNo = row[0]["SerialID"].ToString();

                    if ( GFunc.NEInt(SerialNo.Value, 0) > (GFunc.NEInt(lastserialNo, 0)))
                    {
                        MsgBox.Show("Please use 'Auto Generate Serial No.'. Last Serial No. is '" + lastserialNo + "'");
                        return;
                    }

                    prefixMACID = GFunc.NEStr(objMstItmFactory.ObjMSTItm.Custom2, string.Empty);
                    if (!GFunc.IsNE(prefixMACID))
                    {
                        if (GFunc.IsNE(MACID.Value))
                        {
                            MsgBox.Show("Please type MACID is required.");
                            MACID.Focus();
                            return;
                        }
                        if (MACID.Value.ToString().Length!=17)
                        {
                            MsgBox.Show("MACID format is not correct.");
                            MACID.Focus();
                            return;
                        }
                        if (MACID.Value.ToString().Substring(0, 11) != prefixMACID)
                        {
                            MsgBox.Show("Prefix MACID should be '" + prefixMACID + "'");
                            MACID.Focus();
                            return;
                        }
                    }
                    else
                    {
                        MACID.Value = string.Empty;
                    }

                    if (!GFunc.IsNE(BBID.Value))
                    {
                        if (GFunc.NEInt(BBID.Value, 0) == 0)
                        {
                            MsgBox.Show("BBID should be numeric.");
                            BBID.Focus();
                            return;
                        }
                    }   

                }

                #endregion

                #region Save Serial No., MACI/D No. and BBID

                if (!GFunc.IsNE(dt))
                    if (dt.Rows.Count > 0)
                        dt.Rows.Clear();

                DataRow drSelect = dt.NewRow();

                drSelect["ItmKey"] = objMstItmFactory.ObjMSTItm.ItmKey;
                drSelect["SerialID"] = SerialNo.Value;                
                drSelect["MACAddress"] = GFunc.IsNE(MACID.Value) ? string.Empty : MACID.Value.ToString();
                drSelect["MfgDate"] = DateTime.Today.Date;
                drSelect["ExpiryDate"] = DateTime.Today.Date.AddYears(1);
                drSelect["ItmStatus"] = 1;
                drSelect["BatchID"] = MFNNo.Value.ToString();
                drSelect["Custom2"] = GFunc.IsNE(BBID.Value) ? string.Empty : BBID.Value.ToString();

                //Add Branch name (OMS Singapore/Taiwan) when generate serial no.
                if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                {
                    drSelect["Custom3"] = "Singapore";
                }
                else if (SysOptionUtility.DatabaseBranchCode == "OMSTW")
                {
                    drSelect["Custom3"] = "Taiwan";
                }

                dt.Rows.Add(drSelect);
                dt.AcceptChanges();

                if (!GFunc.IsNE(dt))
                {
                    if (dt.Rows.Count == 1)
                    {
                        string msg = "Are you sure to save (Y/N)?<br />Please confrim Serial no. before Saving.<br /><br />Serial No.: " + SerialNo.Value;
                        if(GFunc.NEStr(BBID.Value,string.Empty) != string.Empty)
                        {
                            msg += "<br /> BBID: " + BBID.Value; 
                        }
                        if (GFunc.NEStr(MACID.Value, string.Empty) != string.Empty)
                        {
                            msg += "<br /> MAC ID: " + MACID.Value;
                        }
                        if (MsgBox.Show(msg, GEnum.MsgBoxIcon.Alert, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                            return;

                        if (objMstItmFactory.SaveSerialList(MFNNo.Value.ToString(), 1, dt, "0","0","0"))
                        {
                            MsgBox.Show("Serial No. created successfully.");
                            Refresh_GridSerial();                            
                            btnSerialPreview.Enabled = true;
                            MFNNo.SetValueTrigger(string.Empty, false);
                            SerialNo.SetValueTrigger(string.Empty, false);
                            MACID.SetValueTrigger(string.Empty, false);
                            BBID.SetValueTrigger(string.Empty, false);

                            btnSerialGenerate.Enabled = true;
                            btnSerialSave.Enabled = false;
                        }
                    }
                }
                #endregion
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

        private void btnEstoreSync_Click(object sender, EventArgs e)
        {            
            if (objMstItmFactory.ObjMSTItm.ItmType == (int)GEnum.ItemType.Stock ||
                objMstItmFactory.ObjMSTItm.ItmType == (int)GEnum.ItemType.Assembly)
            {
                if (objMstItmFactory.ObjMSTItm.EStorePrice == -999)
                {
                    MsgBox.Show("Estore price cannot be updated because this BH code is not available on the eStore.<br/>Please inform the eStore Admin to prepare a new item if it should be a sales item on the eStore.");
                }
                else
                {
                    MSTItm item = MSTItm.Get(objMstItmFactory.ObjMSTItm.ItmID);
                    if (objMstItmFactory.ObjMSTItm.ControlPriceH != item.ControlPriceH)
                    {
                        MsgBox.Show("Please save the estore control price of the current inventory record.");
                    }
                    else
                    {
                        //string strSQL = "select entity_id from BH_EStoreItems where ItemKey = 103761 or sku = 'C6110500'";
                        string eid = GFunc.ExecuteScalar("select top 1 entity_id from BH_EStoreItems where ItemKey = " + objMstItmFactory.ObjMSTItm.ItmKey + " or sku = " + objMstItmFactory.ObjMSTItm.ItmID);
                        int entity_id = GFunc.NEInt(eid, 0);
                        if (entity_id > 0)
                        {
                            Update_eStoreApi(entity_id);
                        }
                    }
                }                    
            }
        }
       
        private void ControlPrice_DoubleClick(object sender, EventArgs e)
        {
            if (objMstItmFactory.ObjMSTItm.ItmType == (int)GEnum.ItemType.Stock ||
                objMstItmFactory.ObjMSTItm.ItmType == (int)GEnum.ItemType.Assembly)
            {
                MSTItm item = MSTItm.Get(objMstItmFactory.ObjMSTItm.ItmID);

                if (objMstItmFactory.ObjMSTItm.ControlPriceH != item.ControlPriceH)
                {
                    MsgBox.Show("Please save the estore control price of the current inventory record.");
                }
                else
                {
                    frmPopupEstoreInfo f;
                    //If it is already loaded, take that one
                    foreach (Form form in Application.OpenForms[0].OwnedForms)
                    {
                        if (form.Name == "frmPopupEstoreInfo")
                        {
                            f = (frmPopupEstoreInfo)form;
                            f.Reload(0, objMstItmFactory.ObjMSTItm.ItmID.Trim());
                            return;
                        }
                    }

                    //If it's not loaded yet, create new
                    f = new frmPopupEstoreInfo(0, objMstItmFactory.ObjMSTItm.ItmID.Trim());
                    f.Show(frmMain.gfrmMain);
                }
            }            
        }

        private void ControlPriceH_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick += new EventHandler(ControlPrice_DoubleClick);
        }

        private void Display_eStorePriceColor()
        {
            chkSyncEstore.Checked = false;
            if (objMstItmFactory.ObjMSTItm.ItmType == 100 || objMstItmFactory.ObjMSTItm.ItmType == 250 || objMstItmFactory.ObjMSTItm.ItmType == 600)
            {
                if (objMstItmFactory.ObjMSTItm.EStorePrice == -999)
                    ControlPriceH.Appearance.BackColor = Color.Orange;
                else if (objMstItmFactory.ObjMSTItm.EStorePrice == 0 && objMstItmFactory.ObjMSTItm.ControlPriceH > 0)
                    ControlPriceH.Appearance.BackColor = Color.Khaki;
                else if (objMstItmFactory.ObjMSTItm.EStorePrice > 0 && objMstItmFactory.ObjMSTItm.ControlPriceH != objMstItmFactory.ObjMSTItm.EStorePrice)
                    ControlPriceH.Appearance.BackColor = Color.Red;
                else
                    ControlPriceH.Appearance.BackColor = System.Drawing.Color.Transparent;
            }
            else
                ControlPriceH.Appearance.BackColor = System.Drawing.Color.Transparent;
        }

        private void Update_eStoreBossTable()
        {
            try
            {
                DataTable dtEstore = new DataTable("dtEstore");

                // Define columns 
                dtEstore.Columns.Add("ITEMKEY", typeof(int));
                dtEstore.Columns.Add("BHCODE", typeof(string));
                dtEstore.Columns.Add("ESTOREPRICE", typeof(decimal));

                // Add the row
                dtEstore.Rows.Add(objMstItmFactory.ObjMSTItm.ItmKey, objMstItmFactory.ObjMSTItm.ItmID, objMstItmFactory.ObjMSTItm.ControlPriceH);
                string xml = GFunc.ConvertDataTableToXML(dtEstore);

                List<SqlParameter> parList = new List<SqlParameter>();
                parList.Add(new SqlParameter("@XmlDetail", xml));
                parList.Add(new SqlParameter("@UserKey", AppInfor.CurrentUserKey));
                parList.Add(new SqlParameter("@UserID", AppInfor.CurrentUserID));
                GFunc.ExecuteProc("UpdateEStoreInfo", parList);

                /* To refresh promptly for EStorePrice without showing the 'Discard Changes' prompt */
                bool originalIsDirty = objMstItmFactory.IsDirty;
                objMstItmFactory.ObjMSTItm.EStorePrice = GFunc.NEDec(objMstItmFactory.ObjMSTItm.ControlPriceH,0);
                objMstItmFactory.IsDirty = originalIsDirty;

            }
            catch
            {
                MsgBox.Show("Estore price cannot be updated. Please inform the authorized person!");
            }            
        }
        private async void Update_eStoreApi(int entity_id)
        {
            try
            {
                await PostData(objMstItmFactory.ObjMSTItm, entity_id);
            }
            catch (Exception ex)
            {
                MsgBox.Show($"Error: {ex.Message}");
            }
        }

        private async Task PostData(MSTItm objItm, int entity_id)
        {
            /* estore api will update price and also sku when item id is changed by the entity_id */
            try
            {
                string apiUrl = "https://monitor.bh-estore.com/api/estore-product";
                string apiKey = "x0L0C8&amp;tNhyS1C05HGtziAhJO7rd(fT"; 
                
                // Set up custom headers (e.g., API key)
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "WindowsFormsApp");
                    client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

                    // Define the POST data (as a JSON string in this case)

                    var apiParameter = new { sku = objItm.ItmID, price = objItm.ControlPriceH, id = entity_id };
                    //var apiParameter = new { sku = "#T001", price = 0.3333, id = 30535 }; /* estore testing - entity_id = 30535 */

                    // Serialize the object to a JSON string using Newtonsoft.Json
                    var jsonContent = JsonConvert.SerializeObject(apiParameter);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Send the POST request
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // Check if the response was successful
                    if (response.IsSuccessStatusCode)
                    {
                        MsgBox.Show("The eStore price has been successfully synchronized with the eStore.");
                        //string responseBody = await response.Content.ReadAsStringAsync();
                        //MessageBox.Show("Request successful: " + responseBody);
                    }
                    else
                    {
                        string errorBody = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Request failed: {response.StatusCode} - {errorBody}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SyncEstore()
        {
            if (objMstItmFactory.ObjMSTItm.ItmType == (int)GEnum.ItemType.Stock ||
                objMstItmFactory.ObjMSTItm.ItmType == (int)GEnum.ItemType.Assembly)
            {
                if (MsgBox.Show("Are you sure you want to synchronize the estore price on the eStore website?", GEnum.MsgBoxIcon.Serious, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                {
                    try
                    {
                        Update_eStoreBossTable();

                        /*  -- api-call temporarily commented for client pc issue 
                        // string entity_id = GFunc.ExecuteScalar("select top 1 entity_id from BH_EStoreItems where ItemKey = " + objMstItmFactory.ObjMSTItm.ItmKey + " or sku = " + objMstItmFactory.ObjMSTItm.ItmID);
                        List<SqlParameter> parmList = new List<SqlParameter>();
                        parmList.Add(new SqlParameter("@Option", 1));
                        parmList.Add(new SqlParameter("@ItmKey", objMstItmFactory.ObjMSTItm.ItmKey));
                        parmList.Add(new SqlParameter("@ItmID", objMstItmFactory.ObjMSTItm.ItmID));
                        string eid = GFunc.ExecuteScalar("Get_EstoreInfo", parmList);
                        int entity_id = GFunc.NEInt(eid, 0);
                        if (entity_id > 0)
                        {
                            Update_eStoreApi(entity_id);
                            Update_eStoreBossTable();
                        }
                        */
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void chkSyncEstore_CustomUpdate(object sender, EventArgs e)
        {
            if (chkSyncEstore.Checked && objMstItmFactory.ObjMSTItm.EStorePrice == -999)
            {
                MsgBox.Show("This bh code has not been uploaded for sales on the eStore yet.<br/>Please inform the eStore Admin to upload it as a new item if it should be a sales item on the eStore.");
                chkSyncEstore.Checked = false;
            }
        }
    }
}
