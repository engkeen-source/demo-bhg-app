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
using System.Collections;
using TAUtil;

namespace WinUI
{
    public partial class frmMSTPriceInfo : Form
    {
        #region Member Variables, Properties and Constructors

        private BOLib.MSTPriceInfoFactory objFactory = null;
        string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        private GEnum.formInitMode formOpenMode = GEnum.formInitMode.Neither;
        private int recordKey = 0;
        private string recordID = string.Empty;
        private bool canEditRecordID = false;

        //only use to show Read only data viewer for AuditLog 
        private bool IsOpenFromAuditLog = false;
        private DataTable _dtHeader = null;
        private DataSet _dsDetail = null;

        frmList fMSTPriceList = null;
        public GVar.ListEvent_CloseFORM list_CloseMSTForm = null;
        public GVar.ListEvent_RefreshRecord ListEvent_RefreshRecord = null;

        #endregion

        //Initialize
        public frmMSTPriceInfo()
        {
            InitializeComponent();
        }//Completed
        public frmMSTPriceInfo(string priceID)
        {
            //For call from shortcut menu (Edit/Add)
            InitializeComponent();
            recordID = priceID;
            formOpenMode = GEnum.formInitMode.Add;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            tsbList.Enabled = false;
        }//Completed
        public frmMSTPriceInfo(int priceKey)
        {
            //For call from shortcut menu (Edit)
            InitializeComponent();
            formOpenMode = GEnum.formInitMode.Edit;
            this.recordKey = priceKey;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            tsbList.Enabled = false;
        }//Completed
        public frmMSTPriceInfo(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            InitializeComponent();
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
        }//Completed

        //Form Events
        private void frmMSTPriceInfo_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //Initialize
                this.objFactory = new BOLib.MSTPriceInfoFactory(BOLib.GEnum.InstanceMode.Normal);
                if (objFactory.GUID <= 0)
                {
                    formClose = true;
                    return;
                }

                //Attach Event on Factory
                this.objFactory.ErrorNotifierHeader_Set += new GVar.UINotifierEvent(this.ErrorNotifier_Set);
                this.objFactory.ErrorNotifierHeader_Clear += new GVar.UINotifierEvent(this.ErrorNotifier_Clear);

                //Check if user has permission to edit Record ID
                canEditRecordID = SECPermUtility.Perform("sysRecID", false);

                if (this.IsOpenFromAuditLog)
                {
                    if (objFactory.SetReadOnlyData(_dtHeader, _dsDetail) == false)
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
                            this.PriceID.SetValueTrigger(recordID, false);
                    }
                }

                //Set ContextMenu & Grid Setting & Grid Formatting                
                GlobalUI.FormGrids_Set(this, (int)objFactory.ConstantCodeKey, out ContextMenuSetting);
                tagrdPriceValueList.DisplayLayout.Bands[0].Columns["ItmID"].SortIndicator = SortIndicator.Ascending;

                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objFactory.ConstantCodeKey);
                GlobalUI.Combos_Fill(this, (int)objFactory.ConstantCodeKey);
                Refresh_DependentCombo(string.Empty);
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
        private void frmMSTPriceInfo_Shown(object sender, EventArgs e)
        {
            if (formClose)
                this.Close();
            else
                this.PriceID.Focus();
        }//Completed
        private void frmMSTPriceInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool runProcess = false;
            this.formClose = true;

            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }
            if (this.formClose && objFactory == null)
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

                if ((bool)this.objFactory.Dispose() == false)
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
                    this.objFactory.Dispose();
            }
        }//Completed
        private void frmMSTPriceInfo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                    GlobalUI.Combos_Fill(this, (int)objFactory.ConstantCodeKey);

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
            this.formClose = true;
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
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.Delete_Process();
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
                this.Clear_Process();
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
                if (!GFunc.IsNE(fMSTPriceList))
                {
                    fMSTPriceList.RecordSelectedEvent -= fMSTPriceList.RecordSelectedEvent;
                    fMSTPriceList.RecordSelectedEvent = new GVar.RecordSelectedEvent(this.OnListRecordSelected);
                    fMSTPriceList.Focus();
                }
                else
                {
                    fMSTPriceList = new frmList(objFactory.ConstantCodeKey, objFactory.PermID);

                    //Attach events to this FORM to call events in frmList
                    this.list_CloseMSTForm += new GVar.ListEvent_CloseFORM(fMSTPriceList.OnCaller_Close);
                    this.ListEvent_RefreshRecord += new GVar.ListEvent_RefreshRecord(fMSTPriceList.OnCaller_Changed);

                    //Attach events to frmList to call events in this FORM
                    fMSTPriceList.ListEvent_OpenRecord = new GVar.ListEvent_OpenRecord(this.OnListRecordSelected);
                    fMSTPriceList.ListEvent_CloseFORM = new GVar.ListEvent_CloseFORM(this.OnList_FormClose);
                    fMSTPriceList.MdiParent = frmMain.gfrmMain;
                    fMSTPriceList.Show();
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
        private void tsbCopy_Click(object sender, EventArgs e)
        {
            try
            {
                this.Copy_Process();
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
                    fMSTPriceList.Focus();
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
                this.Focus();
            }
        }//Completed
        private void OnList_FormClose()
        {
            fMSTPriceList = null;
            this.list_CloseMSTForm = null;
            this.ListEvent_RefreshRecord = null;
        }//Completed

        //Formating, Locking, Refreshing 
        private void Refresh_All(bool IncludeDependentCombo)
        {
            try
            {
                Refresh_Header(IncludeDependentCombo);
                Refresh_GridValueList();
                Refresh_GridRatio();
                GridCellDefault_Set();
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
                bdsMSTPriceInfo.DataSource = objFactory.ObjMSTPriceList;
                bdsMSTPriceInfo.ResetBindings(false);
                if (IncludeDependentCombo)
                    Refresh_DependentCombo(string.Empty);
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
        private void Refresh_GridValueList()
        {
            tagrdPriceValueList.DataSource = objFactory.ObjMSTPriceListDetValues;
            tagrdPriceValueList.Rows.Refresh(RefreshRow.ReloadData);
        }//Completed
        private void Refresh_GridRatio()
        {
            tagrdPriceRatioList.DataSource = objFactory.ObjMSTPriceListDetRatios;
            tagrdPriceRatioList.Rows.Refresh(RefreshRow.ReloadData);
        }//Completed
        private void Refresh_DependentCombo(string ctrlNm)
        {
            //When CtrlNm is Empty it mean refresh all dependant combo
            bool FactoryIsDirty = objFactory.IsDirty;
            DataTable dt = this.BuildInCode.DataSource as DataTable;
            if (dt == null)
                return;

            if (Convert.ToInt32(this.PriceType.Value) == 10)
            {
                if (this.objFactory.IsNew)
                {
                    dt.DefaultView.RowFilter = "MsgValue = 10";
                    this.BuildInCode.SetValueTrigger(10, false);//Normal
                }
                else
                {
                    dt.DefaultView.RowFilter = "";
                }
                
            }
            else
            {
                dt.DefaultView.RowFilter = "MsgValue > 10";
                if (this.objFactory.ObjMSTPriceList.BuildInCode == 10)
                    this.BuildInCode.SetValueTrigger(100, false);//Use Standard Price
            }
            objFactory.IsDirty = FactoryIsDirty;
        }//Completed
        private void FormLayout()
        {
            bool EnableMode = !this.objFactory.IsReadOnly; ;
            this.tslReadOnly.Text = !EnableMode ? "Read Only" : string.Empty;

            #region Grid Visible
            if ((int)PriceType.Value == 10)
            {
                tabDetailList.Tabs["Value"].Visible = true;
                tabDetailList.Tabs["Ratio"].Visible = false;
            }
            else
            {
                tabDetailList.Tabs["Value"].Visible = false;
                tabDetailList.Tabs["Ratio"].Visible = true;
            }
            #endregion

            #region Set Header Controls
            this.PriceID.Enabled = EnableMode;
            this.PriceDes.Enabled = EnableMode;
            this.Custom1.Enabled = EnableMode;
            this.Custom2.Enabled = EnableMode;
            this.Custom3.Enabled = EnableMode;

            //additional conditions
            if (Enabled)
            {
                if (this.objFactory.IsNew)
                {
                    this.PriceType.Enabled = true;
                    this.BuildInCode.Enabled = true;
                    this.CurrKey.Enabled = true;
                }
                else
                {
                    this.PriceType.Enabled = false;
                    if (Convert.ToInt32(this.PriceType.Value) == 20)//By Ratio
                    {
                        this.BuildInCode.Enabled = true;
                        this.CurrKey.Enabled = true;
                    }
                    else
                    {
                        this.BuildInCode.Enabled = false;
                        if ((int)BuildInCode.Value > 10)//Normal
                        {
                            this.CurrKey.Enabled = false;
                        }
                        else
                        {
                            this.CurrKey.Enabled = true;
                        }
                    }
                }
            }
            else
            {
                this.PriceType.Enabled = false;
                this.BuildInCode.Enabled = false;
                this.CurrKey.Enabled = false;
            }
            #endregion

            #region Set Buttons and RecordID and Grid
            if (EnableMode == false)
            {
                this.tsbSave.Enabled = false;
                this.tsbDelete.Enabled = false;
                this.tsbClear.Enabled = false;
                this.tsbCopy.Enabled = false;
                tagrdPriceValueList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
                tagrdPriceValueList.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                tagrdPriceValueList.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
                tagrdPriceRatioList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
                tagrdPriceRatioList.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                tagrdPriceRatioList.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
            }
            else
            {
                this.tsbSave.Enabled = true;
                if (this.objFactory.IsNew)
                {
                    this.tsbClear.Enabled = true;
                    this.tsbDelete.Enabled = false;
                    if (Convert.ToInt32(this.PriceType.Value) == 20)//By Ratio
                        this.tsbCopy.Enabled = false;
                    else
                        this.tsbCopy.Enabled = true;
                }
                else
                {
                    this.tsbClear.Enabled = false;
                    if (Convert.ToInt32(this.PriceType.Value) == 20)//By Ratio
                    {
                        this.tsbDelete.Enabled = true;
                        this.tsbCopy.Enabled = false;
                    }
                    else
                    {
                        if ((int)BuildInCode.Value > 10)//Normal
                        {
                            this.tsbDelete.Enabled = false;
                            this.tsbCopy.Enabled = false;
                        }
                        else
                        {
                            this.tsbDelete.Enabled = true;
                            this.tsbCopy.Enabled = true;
                        }
                    }
                }

                //Check if user has permission to edit Record ID
                if (canEditRecordID && EnableMode)
                    PriceID.ReadOnly = false;
                else
                    PriceID.ReadOnly = true;

                tagrdPriceValueList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;
                tagrdPriceValueList.DisplayLayout.Override.AllowAddNew = AllowAddNew.TemplateOnBottom;
                tagrdPriceValueList.DisplayLayout.Override.AllowDelete = DefaultableBoolean.True;
                tagrdPriceRatioList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;
                tagrdPriceRatioList.DisplayLayout.Override.AllowAddNew = AllowAddNew.TemplateOnBottom;
                tagrdPriceRatioList.DisplayLayout.Override.AllowDelete = DefaultableBoolean.True;
            }
            #endregion

            #region Set Grids Columns
            if ((int)PriceType.Value == 10)
                foreach (UltraGridColumn col in tagrdPriceValueList.DisplayLayout.Bands[0].Columns)
                {
                    switch (col.Key.ToLower())
                    {
                        case "pricekey":
                        case "itmkey":
                        case "itmtype":
                        case "createdate":
                        case "createuserKey":
                        case "lastmodifieddate":
                        case "lastmodifieduserKey":
                            col.CellActivation = Activation.ActivateOnly;
                            break;

                        default:
                            if (EnableMode)
                                col.CellActivation = Activation.AllowEdit;
                            else
                                col.CellActivation = Activation.ActivateOnly;
                            break;
                    }
                }
            else
                foreach (UltraGridColumn col in tagrdPriceRatioList.DisplayLayout.Bands[0].Columns)
                {
                    switch (col.Key.ToLower())
                    {
                        case "pricekey":
                        case "ratio":
                        case "effratio":
                        case "createdate":
                        case "createuserKey":
                        case "lastmodifieddate":
                        case "lastmodifieduserKey":
                            col.CellActivation = Activation.ActivateOnly;
                            break;

                        default:
                            if (EnableMode)
                                col.CellActivation = Activation.AllowEdit;
                            else
                                col.CellActivation = Activation.ActivateOnly;
                            break;
                    }
                }
            #endregion

        }//Completed
        private void GridCellDefault_Set()
        {
            try
            {
                #region tagrdPriceValueList
                this.tagrdPriceValueList.DisplayLayout.Bands[0].Columns["ItmQty"].DefaultCellValue = 0;
                this.tagrdPriceValueList.DisplayLayout.Bands[0].Columns["ItmPrice"].DefaultCellValue = 0;
                this.tagrdPriceValueList.DisplayLayout.Bands[0].Columns["CustomPrice"].DefaultCellValue = 0;
                this.tagrdPriceValueList.DisplayLayout.Bands[0].Columns["IgnorePriceUpdate"].DefaultCellValue = 1;
                this.tagrdPriceValueList.DisplayLayout.Bands[0].Columns["EffItmQty"].DefaultCellValue = 1;
                this.tagrdPriceValueList.DisplayLayout.Bands[0].Columns["EffItmPrice"].DefaultCellValue = 1;
                #endregion

                #region tagrdPriceRatioList
                this.tagrdPriceRatioList.DisplayLayout.Bands[0].Columns["Cat1"].DefaultCellValue = 0;
                this.tagrdPriceRatioList.DisplayLayout.Bands[0].Columns["Cat2"].DefaultCellValue = 0;
                this.tagrdPriceRatioList.DisplayLayout.Bands[0].Columns["Cat3"].DefaultCellValue = 0;
                this.tagrdPriceRatioList.DisplayLayout.Bands[0].Columns["Cat4"].DefaultCellValue = 0;
                this.tagrdPriceRatioList.DisplayLayout.Bands[0].Columns["Cat5"].DefaultCellValue = 0;
                this.tagrdPriceRatioList.DisplayLayout.Bands[0].Columns["RatioType"].DefaultCellValue = 10;
                this.tagrdPriceRatioList.DisplayLayout.Bands[0].Columns["Percentage"].DefaultCellValue = 0;
                this.tagrdPriceRatioList.DisplayLayout.Bands[0].Columns["Ratio"].DefaultCellValue = 1;
                this.tagrdPriceRatioList.DisplayLayout.Bands[0].Columns["EffPercentage"].DefaultCellValue = 0;
                this.tagrdPriceRatioList.DisplayLayout.Bands[0].Columns["EffRatio"].DefaultCellValue = 1;
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
                        this.objFactory.IsDirty = false;
                        IsGridsDirty(true);
                    }
                }

                this.errorProvider1.Clear();

                if (this.objFactory.IsDirty)
                {
                    if (Save_Process() == false)
                        return false;
                }

                if (this.objFactory.New() == false)
                {                   
                    return false;
                }
                else
                {
                    this.errorProvider1.Clear();                 
                    this.PriceID.Focus();
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

                if (objFactory.IsDirty)
                {
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show(MsgID.Common.SaveChanges,
                                          GEnum.MsgBoxIcon.Question,
                                          GEnum.MsgBoxButton.Save_Changes,
                                          GEnum.MsgBoxButton.Discard_Changes,
                                          GEnum.MsgBoxButton.I_Dont_Know);

                    if (btnSelect == GEnum.MsgBoxButton.Save_Changes)
                    {
                        return this.Save_Process();
                    }
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
                if (this.objFactory.Save())
                {
                    if (GFunc.IsNE(this.ListEvent_RefreshRecord) == false)
                        ListEvent_RefreshRecord.Invoke();                   
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

                if (SECPermUtility.Edit(objFactory.PermID, false))
                {
                    if (objFactory.GetEdit(key) == false)
                    {
                        if (SysOptionUtility.GetBool("WarnOpenRecordAsReadOnly"))
                        {
                            btnSelect = MsgBox.Show("Try to open record as read only",
                                                  GEnum.MsgBoxIcon.Question,
                                                  GEnum.MsgBoxButton.Yes,
                                                  GEnum.MsgBoxButton.No);

                            if (btnSelect == GEnum.MsgBoxButton.Yes)
                            {
                                objFactory.GetReadOnly(key);
                            }
                        }
                    }
                }
                else
                    objFactory.GetReadOnly(key);

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

                if (this.objFactory.Delete())
                {
                    IsGridsDirty(true);
                    if (!GFunc.IsNE(this.ListEvent_RefreshRecord))
                        ListEvent_RefreshRecord.Invoke();

                    this.objFactory.New();
               
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
                if (GFunc.IsNEZ(this.objFactory.ObjMSTPriceList.PriceKey))
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

                    if (this.objFactory.New())
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
        private void Copy_Process()
        {
            try
            {
                if (!this.objFactory.ObjMSTPriceListDetValues.HasErrors)
                {
                    ArrayList selectedItemList = new ArrayList();
                    frmItemCopy fItemCopy = new frmItemCopy();
                    fItemCopy.ShowDialog();

                    if (fItemCopy.SelectedItemList.Rows.Count> 0)
                    {
                        foreach (DataRow dr in fItemCopy.SelectedItemList.Rows)
                        {


                            DataRow drNew = this.objFactory.ObjMSTPriceListDetValues.NewRow();
                            drNew["ItmKey"] = GFunc.NEInt(dr["Key"], 0);
                            drNew["ItmType"] = GFunc.NEInt(dr["ItmType"], 0);
                            drNew["ItmDes"] = GFunc.NEStr(dr["Des"], "");
                            if (Convert.ToInt32(dr["ItmType"]) < 700)
                            {
                                drNew["ItmQty"] = 1;
                                drNew["EffItmQty"] = 1;
                            }
                            else
                            {
                                drNew["ItmQty"] = null;
                                drNew["EffItmQty"] = null;
                            }
                            this.objFactory.ObjMSTPriceListDetValues.Rows.Add(drNew);

                            this.tagrdPriceValueList.ActiveRow = this.tagrdPriceValueList.Rows[this.objFactory.ObjMSTPriceListDetValues.Rows.Count - 1];


                            this.bdsMSTPriceValueList.DataSource = this.objFactory.ObjMSTPriceListDetValues;
                            this.bdsMSTPriceValueList.ResetBindings(false);
                        }
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
        private bool form_CanValidate()
        {
            try
            {
                //this function check if we can validates all the header.controls
                //all grid is able to commit its changes to its datasource
                TAUtil.ControlGVar.FormValidateFail = false;
                this.errorProvider1.Clear();
                this.Validate();
                this.tagrdPriceValueList.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdPriceValueList.UpdateData();
                this.tagrdPriceRatioList.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdPriceRatioList.UpdateData();

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

            #region tagrdPriceValueList
            if (tagrdPriceValueList.ActiveRow != null)
            {
                if (tagrdPriceValueList.ActiveRow.DataChanged && !tagrdPriceValueList.ActiveRow.IsUnmodifiedTemplateAddRow)
                {
                    if (undoChangesInGrid)
                    {
                        //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                        this.tagrdPriceValueList.PerformAction(UltraGridAction.UndoCell);
                        this.tagrdPriceValueList.PerformAction(UltraGridAction.UndoRow);
                    }
                    return true;
                }
            }
            #endregion

            #region tagrdPriceRatioList
            if (tagrdPriceRatioList.ActiveRow != null)
            {
                if (tagrdPriceRatioList.ActiveRow.DataChanged && !tagrdPriceRatioList.ActiveRow.IsUnmodifiedTemplateAddRow)
                {
                    if (undoChangesInGrid)
                    {
                        //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                        this.tagrdPriceRatioList.PerformAction(UltraGridAction.UndoCell);
                        this.tagrdPriceRatioList.PerformAction(UltraGridAction.UndoRow);
                    }
                    return true;
                }
            }
            #endregion

            return false;
        }//Completed

        //Tab Events
        private void tabDetailList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (tabDetailList.ActiveTab.Key)
                {
                    case "Value":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                tagrdPriceValueList.Focus();
                                UltraGridColumn FirstVisCol = tagrdPriceValueList.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                                if (FirstVisCol != null)
                                {
                                    tagrdPriceValueList.ActiveCell = tagrdPriceValueList.Rows.TemplateAddRow.Cells[FirstVisCol.Key];
                                    tagrdPriceValueList.PerformAction(UltraGridAction.EnterEditMode, false, false);
                                }
                                break;
                            case Keys.Up:
                                Custom3.Focus();
                                break;
                        }
                        break;
                    case "Ratio":
                        switch (e.KeyCode)
                        {
                            case Keys.Enter:
                            case Keys.Down:
                                // tagrdCurrDetailConList.Select();
                                tagrdPriceRatioList.Focus();
                                UltraGridColumn FirstVisCol = tagrdPriceRatioList.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                                if (FirstVisCol != null)
                                {
                                    tagrdPriceRatioList.ActiveCell = tagrdPriceRatioList.Rows.TemplateAddRow.Cells[FirstVisCol.Key];
                                    tagrdPriceRatioList.PerformAction(UltraGridAction.EnterEditMode, false, false);
                                }
                                break;
                            case Keys.Up:
                                Custom3.Focus();
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
        private void PriceType_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                this.objFactory.ObjMSTPriceList.PriceType = GFunc.NEInt(this.objFactory.ObjMSTPriceList.PriceType, 10);
                this.Refresh_DependentCombo(string.Empty);
                this.FormLayout();
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
        private void CurrKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            this.objFactory.ObjMSTPriceList.CurrKey = GFunc.NEInt(this.objFactory.ObjMSTPriceList.CurrKey, 1);
        }//Completed
        private void BuildInCode_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (GFunc.IsNEZ(this.BuildInCode.Value))
            {
                MsgBox.Show("BuildInCode cannot be empty");
                e.Cancel = true;
            }
        }//Completed

        //Editor Controls Events
        private void RecSearchSelected(object sender, string fieldNm, int key, string id, string des)
        {
            //This is a common function used by header and grid to handle Con,Itm,Acc,Job RecordSearch
            //Currently the sender is not use (for future requirements)
            //this is because we do not have a situation where there is a conflict of fieldNm between Header and Grids
            //when this happen we will need to add another switch to check the gridName in order to resolve the conflict

            try
            {
                switch (fieldNm.ToLower())
                {
                    case "itmid":
                    case "itmdes":
                        tagrdPriceValueList.ActiveRow.Cells["ItmKey"].Value = key;
                        tagrdPriceValueList.ActiveRow.Cells["ItmID"].Value = id;
                        tagrdPriceValueList.ActiveRow.Cells["ItmDes"].Value = des;
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
                    case "itmid":
                        PopupType = (int)GEnum.PopupType.ItmID;
                        AccessType = (int)GEnum.RecAccessType.ItemID;
                        keySearch = "Itm";
                        break;

                    case "itmdes":
                        PopupType = (int)GEnum.PopupType.ItmDes;
                        AccessType = (int)GEnum.RecAccessType.ItemDes;
                        keySearch = "Itm";
                        break;
                }

                if (FromButtonClick)
                {
                    if (DocHDRUtil.EditorButton_Popup((int)objFactory.ConstantCodeKey, controlText, listSettingID, PopupType, ref key, ref id, ref des))
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
                            case "itm":
                                key = GFunc.ItmRecord_GetKey((GEnum.RecAccessType)AccessType, listSettingID, controlText, vendorKey, ref id, ref des, false);
                                break;

                        }
                        if (GFunc.IsNEZ(key))
                        {
                            //since value input by user cannot be match let the user select from Popup form
                            if (DocHDRUtil.EditorButton_Popup((int)objFactory.ConstantCodeKey, controlText, listSettingID, PopupType, ref key, ref id, ref des))
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

        //Grid common Events
        private void Grid_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                e.DisplayPromptMsg = false;
                TAUtil.TAGridEditor taGrid = sender as TAUtil.TAGridEditor;
                if (taGrid.Rows.Count > 0)
                {
                    if (taGrid.ActiveRow.IsAddRow == false)
                    {
                        if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) != GEnum.MsgBoxButton.Delete)
                        {
                            e.Cancel = true;
                            return;
                        }
                        return;
                    }
                }
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
        private void Grid_AfterRowsDeleted(object sender, EventArgs e)
        {
            objFactory.IsDirty = true;
            switch (((TAUtil.TAGridEditor)sender).Name)
            {
                case "tagrdPriceValueList":
                    objFactory.ObjMSTPriceListDetValues.AcceptChanges();
                    break;

                case "tagrdPriceRatioList":
                    objFactory.ObjMSTPriceListDetRatios.AcceptChanges();
                    break;
            }
        }//Completed
        private void Grid_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            try
            {              
                
                TAUtil.TAGridEditor grd = (TAUtil.TAGridEditor)sender;
                if (grd.ActiveRow != null)
                {
                    if (objFactory.Validation_Detail(grd.Name, grd.ActiveRow, string.Empty) == false)
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
        private void Grid_AfterRowUpdate(object sender, RowEventArgs e)
        {
            objFactory.IsDirty = true;
            switch (((TAUtil.TAGridEditor)sender).Name)
            {
                case "tagrdPriceValueList":
                    objFactory.ObjMSTPriceListDetValues.AcceptChanges();
                    break;

                case "tagrdPriceRatioList":
                    objFactory.ObjMSTPriceListDetRatios.AcceptChanges();
                    break;
            }
        }//Completed

        //Grid Value Events
        private void tagrdPriceValueList_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                switch (e.Cell.Column.Key.ToLower())
                {
                    case "itmid":
                    case "itmdes":
                        if (RecSearchProcess(sender, e.Cell.Column.Key, true))
                        {
                            if (objFactory.Validation_Detail(tagrdPriceValueList.Name, tagrdPriceValueList.ActiveRow, tagrdPriceValueList.ActiveCell.Column.Key))
                            {
                                MSTItm objItm = MSTItm.GetParent((int)tagrdPriceValueList.ActiveRow.Cells["ItmKey"].Value);
                                tagrdPriceValueList.ActiveRow.Cells["ItmKey"].Value = objItm.ItmKey;
                                tagrdPriceValueList.ActiveRow.Cells["ItmType"].Value = objItm.ItmType;
                            }
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
        private void tagrdPriceValueList_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
             
                UltraGridCell currentCell = tagrdPriceValueList.ActiveCell;

                switch (currentCell.Column.Key.ToLower())
                {
                    case "lastupdateddate":
                    case "ignorepriceupdate":
                    case "effstartdate":
                    case "effenddate":
                    case "custom1":
                    case "custom2":
                    case "custom3":
                        e.Cancel = !objFactory.Validation_Detail(tagrdPriceValueList.Name, tagrdPriceValueList.ActiveRow, currentCell.Column.Key);
                        break;

                    case "itmqty":
                    case "itmprice":
                    case "customprice":
                    case "effItmqty":
                    case "effItmprice":
                        currentCell.Value = GFunc.NEDec(currentCell.Value, 0);
                        e.Cancel = !objFactory.Validation_Detail(tagrdPriceValueList.Name, tagrdPriceValueList.ActiveRow, currentCell.Column.Key);
                        break;

                    case "itmid":
                    case "itmdes":
                        if (GFunc.CompareString(currentCell.Column.Key, "ItmDes") && !GFunc.IsNE(tagrdPriceValueList.ActiveRow.Cells["ItmID"].Value))
                            return;

                        if (RecSearchProcess(sender, e.Cell.Column.Key, false))
                        {
                            if (objFactory.Validation_Detail(tagrdPriceValueList.Name, tagrdPriceValueList.ActiveRow, currentCell.Column.Key))
                            {
                                MSTItm objItm = MSTItm.GetParent((int)tagrdPriceValueList.ActiveRow.Cells["ItmKey"].Value);
                                if (GFunc.IsNEZ(objItm.ItmKey))
                                {
                                    tagrdPriceValueList.ActiveRow.Cells["ItmKey"].Value = DBNull.Value;
                                    tagrdPriceValueList.ActiveRow.Cells["ItmType"].Value = 0;
                                }
                                else
                                {
                                    tagrdPriceValueList.ActiveRow.Cells["ItmKey"].Value = objItm.ItmKey;
                                    tagrdPriceValueList.ActiveRow.Cells["ItmType"].Value = objItm.ItmType;
                                }
                                return;
                            }
                        }
                        e.Cancel = true;
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

        //Grid Ratio Events
        private void tagrdPriceRatioList_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
               
                UltraGridCell currentCell = tagrdPriceRatioList.ActiveCell;
                switch (currentCell.Column.Key.ToLower())
                {
                    case "cat1":
                    case "cat2":
                    case "cat3":
                    case "cat4":
                    case "cat5":
                        currentCell.Value = GFunc.NEInt(currentCell.Value, 0);
                        e.Cancel = !objFactory.Validation_Detail(tagrdPriceRatioList.Name, tagrdPriceRatioList.ActiveRow, currentCell.Column.Key);
                        break;

                    case "ratiotype":
                        currentCell.Value = GFunc.NEInt(currentCell.Value, 10);
                        e.Cancel = !objFactory.Validation_Detail(tagrdPriceRatioList.Name, tagrdPriceRatioList.ActiveRow, currentCell.Column.Key);
                        break;

                    case "percentage":
                    case "effpercentage":
                        currentCell.Value = GFunc.NEDec(currentCell.Value, 0);
                        e.Cancel = !objFactory.Validation_Detail(tagrdPriceRatioList.Name, tagrdPriceRatioList.ActiveRow, currentCell.Column.Key);
                        break;

                    case "effstartdate":
                    case "effenddate":
                    case "custom1":
                    case "custom2":
                    case "custom3":
                        e.Cancel = !objFactory.Validation_Detail(tagrdPriceRatioList.Name, tagrdPriceRatioList.ActiveRow, currentCell.Column.Key);
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

        //Attached Methods
        private void ErrorNotifier_Clear(object sender, BOLib.UINotifierEventArgs e)
        {
            this.errorProvider1.Clear();
            DocComUtility.ClearErrNotifier(this, e, errorProvider1);
        }//Completed
        private void ErrorNotifier_Set(object sender, BOLib.UINotifierEventArgs e)
        {
            string propertyNm = string.Empty;
            string conNm = string.Empty;
            try
            {
                //For ErrorProvider
                foreach (object key in e.PropertyMessage.Keys)
                {
                    conNm = key.ToString();
                    Control co = this.Controls.Find(conNm, true)[0];
                    this.errorProvider1.SetError(co, e.PropertyMessage[key].ToString());
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

        //Error
        private void OnDataError(object sender, TAUtil.TAErrorEventArgs e)
        {
            try
            {
                if (e.ErrorCode == TAUtil.TAErrorCode.NOT_IN_LIST)
                {
                    TAUtil.TAGridEditor tagrdDetItms = sender as TAUtil.TAGridEditor;
                    if (tagrdDetItms.ActiveCell.Column.EditorComponent != null)
                    {
                        tagrdDetItms.PerformAction(UltraGridAction.EnterEditMode);

                        if (tagrdDetItms.ActiveCell.Column.EditorComponent.GetType() == typeof(TAUtil.TAComboBox))
                        {
                            TAUtil.TAComboBox taCombo = (TAUtil.TAComboBox)tagrdDetItms.ActiveCell.Column.EditorComponent;
                            taCombo.Text = tagrdDetItms.ActiveCell.Text;

                            switch (tagrdDetItms.ActiveCell.Column.Key.ToLower())
                            {
                                case "cat1":
                                case "cat2":
                                case "cat3":
                                case "cat4":
                                case "cat5":
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
                else if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_NUMERIC)
                {
                    throw new TAException(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
                }
                if (e.ErrorCode == TAUtil.TAErrorCode.FORMULA_NOT_RECOGNIZE)
                {
                    throw new TAException("FORMULA NOT RECOGNIZE");
                }
                if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_DATE)
                {
                    // throw new TAException(MsgID.Common.InvalidCellDataTypeDate + "%Currency Date");
                    MsgBox.Show("Please Enter a valid date!");
                    e.Cancel = true;
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
    }
}