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
using Infragistics.Win.UltraWinEditors;
using System.Transactions;
using TAUtil;

namespace WinUI
{
    public partial class frmMSTConOpenBal : Form
    {
        #region Local Variables
        private BOLib.MSTConOpenBalFactory objFactory;
        private string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        private int RecordKey = 0;
        DataTable dtCon = null; //for next/previous button search
        public GEnum.SystemCode SysCode = GEnum.SystemCode.AR_Opening_Balance;

        //only use to show Read only data viewer for AuditLog 
        private bool IsOpenFromAuditLog = false;
        private DataTable _dtHeader = null;
        private DataSet _dsDetail = null;

        int PreRowIndex = 0;
        #endregion

        //Initialize
        public frmMSTConOpenBal()
        {
            InitializeComponent();
        }//Completed
        public frmMSTConOpenBal(GEnum.SystemCode codekey)
        {
            InitializeComponent();
            this.SysCode = codekey;
        }//Completed
        public frmMSTConOpenBal(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            //Open from AuditLog as Read only data viewer
            InitializeComponent();
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
            SysCode = DocCodeKey;
        }//Completed

        //Form Events
        private void frmMSTConOpenBal_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                // Initialize
                this.objFactory = new BOLib.MSTConOpenBalFactory(BOLib.GEnum.InstanceMode.Normal, SysCode);
                if (objFactory.GUID <= 0)
                {
                    formClose = true;
                    return;
                }

                #region Setting Form Text
                switch (SysCode)
                {
                    case GEnum.SystemCode.AR_Cash_Opening_Balance:
                        this.Text = "Customer Opening Balance (Cash)";
                        break;
                    case GEnum.SystemCode.AR_Opening_Balance:
                        this.Text = "Customer Opening Balance (Credit)";
                        break;
                    case GEnum.SystemCode.AP_Opening_Balance:
                        this.Text = "Vendor Opening Balance";
                        break;
                }
                #endregion

                if (this.IsOpenFromAuditLog)
                {
                    if (objFactory.SetReadOnlyData(_dsDetail) == false)
                    {
                        formClose = true;
                        return;
                    }

                    RecordKey = (int)_dtHeader.Rows[0]["ConKey"];
                    ConKey.SetValueTrigger(_dtHeader.Rows[0]["ConKey"].ToString(), false);
                    ConNm.SetValueTrigger(_dtHeader.Rows[0]["ConNm"].ToString(), false);
                    Refresh_GridDet();
                    GlobalUI.FormEnable_Set(this, false);
                }
                else
                {
                    this.objFactory.New();
                    this.RefreshDataAndLayout();
                }

                //Setup FORM control/grid format, menu, listID
                GlobalUI.FormGrids_Set(this, (int)objFactory.ConstantCodeKey, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objFactory.ConstantCodeKey);
                GlobalUI.Combos_Fill(this, (int)objFactory.ConstantCodeKey);
                
                //Make a copy of the Con List from the combo(ConKey) for use in Next/Previous record function
                if (ConKey.DataSource != null)
                    dtCon = (ConKey.DataSource as DataTable).Copy();

                //Get Total of All records
                Calculation();
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
        private void frmMSTConOpenBal_Shown(object sender, EventArgs e)
        {
            if (formClose)
                this.Close();
            else
                this.ConKey.Focus();
        }//Completed
        private void frmMSTConOpenBal_FormClosing(object sender, FormClosingEventArgs e)
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
                    runProcess = true;
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

                //Dispose Factory
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
        private void frmMSTConOpenBal_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                    GlobalUI.Combos_Fill(this, (int)objFactory.ConstantCodeKey);

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

        //Menu Stip Events
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }//Completed
        private void btnSave_Click(object sender, EventArgs e)
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
        private void tsbNext_Click(object sender, EventArgs e)
        {
            try
            {
                string id = ConKey.Text;

                GFunc.GetIndexfromDT("ID", ConKey.Text, true, dtCon, out id);
                if (ConKey.Text != id)
                {
                    ConKey.Text = id;
                    CancelEventArgs ea = new CancelEventArgs();
                    ConKey_CustomUpdate(null, ea);
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
        private void tsbPrevious_Click(object sender, EventArgs e)
        {
            string id = ConKey.Text;
            try
            {
                GFunc.GetIndexfromDT("ID", ConKey.Text, false, dtCon, out id);
                if (ConKey.Text != id)
                {
                    ConKey.Text = id;
                    CancelEventArgs ea = new CancelEventArgs();
                    ConKey_CustomUpdate(null, ea);
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

        //Formating, Locking, Refreshing
        private void RefreshDataAndLayout()
        {
            try
            {
                Refresh_Header();
                Refresh_GridDet();
                FormLayout();
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
        private void Refresh_Header()
        {
            try
            {
                MSTCon objCon = MSTCon.Get(RecordKey);
                if (GFunc.IsNEZ(objCon.ConKey))
                {
                    ConKey.SetValueTrigger(string.Empty, false);
                    ConNm.SetValueTrigger(string.Empty, false);
                    RecordBalance.SetValueTrigger ("0.00",false);
                    TotalBalance.SetValueTrigger ("0.00",false);
                }
                else
                {
                    ConKey.SetValueTrigger(objCon.ConKey.ToString(),false);
                    ConNm.SetValueTrigger(objCon.ConNm.ToString(),false);
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
        private void Refresh_GridDet()
        {
            try
            {
                tagrdDetail.DataSource = objFactory.ObjMSTConOpenBals;
                tagrdDetail.Rows.Refresh(RefreshRow.ReloadData);

                //Set default values
                tagrdDetail.DisplayLayout.Bands[0].Columns["DocCodeKey"].DefaultCellValue = SysCode;
                tagrdDetail.DisplayLayout.Bands[0].Columns["DocConKey"].DefaultCellValue = RecordKey;
                tagrdDetail.DisplayLayout.Bands[0].Columns["DocCurrRate"].DefaultCellValue = 1;
                tagrdDetail.DisplayLayout.Bands[0].Columns["DocGrand"].DefaultCellValue = 0;
                tagrdDetail.DisplayLayout.Bands[0].Columns["DocHome"].DefaultCellValue = 0;
                tagrdDetail.DisplayLayout.Bands[0].Columns["DocApplyAmtF"].DefaultCellValue = 0;
                tagrdDetail.DisplayLayout.Bands[0].Columns["DocApplyAmtH"].DefaultCellValue = 0;
                tagrdDetail.DisplayLayout.Bands[0].Columns["DocApplyFull"].DefaultCellValue = 0;
                tagrdDetail.DisplayLayout.Bands[0].Columns["DocRevalueAmtH"].DefaultCellValue = 0;
                tagrdDetail.DisplayLayout.Bands[0].Columns["DocRevalueRate"].DefaultCellValue = 0;
                tagrdDetail.DisplayLayout.Bands[0].Columns["DocState"].DefaultCellValue = 100;  //Posted
                tagrdDetail.DisplayLayout.Bands[0].Columns["PurgeKeep"].DefaultCellValue = 0;
                tagrdDetail.DisplayLayout.Bands[0].Columns["PurgeData"].DefaultCellValue = 0;

                MSTCon objCon = MSTCon.Get(RecordKey);
                if (objCon.ConKey != null)
                {
                    if (SysCode == GEnum.SystemCode.AP_Opening_Balance)
                    {
                        tagrdDetail.DisplayLayout.Bands[0].Columns["DocBranchKey"].DefaultCellValue = GFunc.NEInt(objCon.VBranchKey,0);
                        tagrdDetail.DisplayLayout.Bands[0].Columns["DocDeptKey"].DefaultCellValue = GFunc.NEInt(objCon.VDeptKey,0);
                        tagrdDetail.DisplayLayout.Bands[0].Columns["DocAccKey"].DefaultCellValue = GFunc.NEInt(objCon.VAccKey, 0);
                        tagrdDetail.DisplayLayout.Bands[0].Columns["DocGrpKey"].DefaultCellValue = GFunc.NEInt(objCon.VGrpKey, 0);
                        tagrdDetail.DisplayLayout.Bands[0].Columns["DocCurrKey"].DefaultCellValue = GFunc.NEInt(objCon.VCurrkey, 0);
                    }
                    else
                    {
                        tagrdDetail.DisplayLayout.Bands[0].Columns["DocBranchKey"].DefaultCellValue = GFunc.NEInt(objCon.CBranchKey, 0);
                        tagrdDetail.DisplayLayout.Bands[0].Columns["DocDeptKey"].DefaultCellValue = GFunc.NEInt(objCon.CDeptKey, 0);
                        tagrdDetail.DisplayLayout.Bands[0].Columns["DocAccKey"].DefaultCellValue = GFunc.NEInt(objCon.CAccKey, 0);
                        tagrdDetail.DisplayLayout.Bands[0].Columns["DocGrpKey"].DefaultCellValue = GFunc.NEInt(objCon.CGrpKey, 0);
                        tagrdDetail.DisplayLayout.Bands[0].Columns["DocCurrKey"].DefaultCellValue = GFunc.NEInt(objCon.CCurrkey, 0);
                    }
                }
                objCon = null;
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
                bool EnableMode = !this.objFactory.IsReadOnly;
                this.tslReadOnly.Text = !EnableMode ? "Read Only" : string.Empty;

                this.tsbSave.Enabled = EnableMode;
                this.AutoSave.Enabled = EnableMode;
                this.tagrdDetail.Enabled = true;
                
                if (EnableMode)
                {
                    //To indicate to user that no entry is available until a valid record is selected
                    if (GFunc.IsNEZ(RecordKey))
                    {
                        this.tsbSave.Enabled = false;
                        this.tagrdDetail.Enabled = false;
                    }
                    this.tagrdDetail.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
                    this.tagrdDetail.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
                    this.tagrdDetail.DisplayLayout.Override.AllowAddNew = AllowAddNew.TemplateOnBottom;

                    #region set grid activation
                    foreach (UltraGridColumn gcol in tagrdDetail.DisplayLayout.Bands[0].Columns)
                    {
                        switch (gcol.Key.ToLower())
                        {
                            case "dockey":
                            case "doccodekey":
                            case "docconkey":
                            case "docapplyamtf":
                            case "docapplyamth":
                            case "docapplyfull":
                            case "docrevalueamth":
                            case "docrevaluerate":
                            case "docstate":
                            case "createdate":
                            case "createuserkey":
                            case "lastmodifieddate":
                            case "lastmodifieduserkey":
                            case "purgekeep":
                            case "purgedata":
                                gcol.CellActivation = Activation.ActivateOnly;
                                break;

                            default:
                                if (EnableMode)
                                    gcol.CellActivation = Activation.AllowEdit;
                                else
                                    gcol.CellActivation = Activation.ActivateOnly;
                                break;
                        }
                    }
                    #endregion
                }
                else
                {
                    this.tagrdDetail.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
                    this.tagrdDetail.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                    this.tagrdDetail.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
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
        private bool SaveChanges()
        {
            try
            {
                if (form_CanValidate() == false)
                    return false;

                if (objFactory.IsDirty && (AutoSave.Checked))
                    return this.Save_Process();

                if (objFactory.IsDirty)
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
                
                if (GFunc.IsNEZ(RecordKey))
                    return false;

                //Perform Saving
                if (this.objFactory.Save(RecordKey))
                {
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
                this.RefreshDataAndLayout();
                this.Cursor = Cursors.Default;
            }
        }//Completed
        public bool OpenRecord(int key)
        {
            this.Cursor = Cursors.WaitCursor;
            GEnum.MsgBoxButton btnSelect = GEnum.MsgBoxButton.Yes;

            try
            {
                if (SaveChanges() == false)
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
                                if (objFactory.GetReadOnly(key) == false)
                                    return false;
                            }
                            else
                                return false;
                        }
                    }
                    
                }
                else
                {
                    if (objFactory.GetReadOnly(key) == false)
                        return false;
                }

                RecordKey = key;
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
                RefreshDataAndLayout();
                Calculation();
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
                this.Validate();
                this.tagrdDetail.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdDetail.UpdateData();

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
                if (tagrdDetail.ActiveRow != null)
                {
                    if (tagrdDetail.ActiveRow.DataChanged && !tagrdDetail.ActiveRow.IsUnmodifiedTemplateAddRow)
                    {
                        if (undoChangesInGrid)
                        {
                            //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                            this.tagrdDetail.PerformAction(UltraGridAction.UndoCell);
                            this.tagrdDetail.PerformAction(UltraGridAction.UndoRow);
                        }
                        return true;
                    }
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
            
        }//Completed
        private void Calculation()
        {
            try
            {
                int key = GFunc.NEInt(ConKey.Value, 0);
                DataTable dt = new DataTable();
                decimal BalTotalH = 0;
                decimal ConTotalH = 0;
                

                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@Option", Convert.ToInt16(0)));
                parmList.Add(new SqlParameter("@DocConKey", key));
                parmList.Add(new SqlParameter("@DocCodeKey", (int)SysCode));
                parmList.Add(new SqlParameter("@RetValue", 0));
                parmList[3].Direction = ParameterDirection.Output;

                dt = GFunc.ExecuteProc("MSTConOpenBal_GetTotal", parmList);
                BalTotalH = GFunc.NEDec(dt.Rows[0]["TotalBalH"], 0);
                ConTotalH +=(tagrdDetail.DataSource as DataTable).AsEnumerable().Sum(p => p.Field<decimal>("DocHome"));
                ConTotalH = GFunc.NEDec(ConTotalH, 0);

                this.RecordBalance.SetValueTrigger(ConTotalH.ToString(), false);
                this.TotalBalance.SetValueTrigger((BalTotalH + ConTotalH).ToString(), false);
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
        private void ConKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                int key = GFunc.NEInt(this.ConKey.Value,0);

                if (key == 0)
                    e.Cancel = true;
                else
                    OpenRecord(key);
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
        private void ConKey_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                int PopupType = 0;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ConKey");

                switch (SysCode)
                {
                    case GEnum.SystemCode.AR_Cash_Opening_Balance:
                    case GEnum.SystemCode.AR_Opening_Balance:
                        PopupType = (int)GEnum.PopupType.CusID;
                        break;
                    case GEnum.SystemCode.AP_Opening_Balance:
                        PopupType = (int)GEnum.PopupType.VendID;
                        break;
                }
                if (DocHDRUtil.EditorButton_Popup((int)objFactory.ConstantCodeKey, ConKey.Text, listSettingID, PopupType, ref key, ref id, ref des))
                    OpenRecord(key);

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
        private void ConNm_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                int PopupType = 0;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ConNm");

                switch (SysCode)
                {
                    case GEnum.SystemCode.AR_Cash_Opening_Balance:
                    case GEnum.SystemCode.AR_Opening_Balance:
                        PopupType = (int)GEnum.PopupType.CusNm;
                        break;

                    case GEnum.SystemCode.AP_Opening_Balance:
                        PopupType = (int)GEnum.PopupType.VendNm;
                        break;
                }
                if (DocHDRUtil.EditorButton_Popup((int)objFactory.ConstantCodeKey, ConNm.Text, listSettingID, PopupType, ref key, ref id, ref des))
                    OpenRecord(key);
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
        private void ConNm_CustomUpdate(object sender, CancelEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;
            GEnum.RecAccessType RecAccessType = GEnum.RecAccessType.CustNm;
            int PopupType = 0;

            try
            {
                if (GFunc.IsNE(ConNm.Text))
                {
                    ConNm.SetValueTrigger(null, false);
                    ConKey.SetValueTrigger(null, false);
                    return;
                }

                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ConNm.Name);
                switch (SysCode)
                {
                    case GEnum.SystemCode.AR_Cash_Opening_Balance:
                    case GEnum.SystemCode.AR_Opening_Balance:
                        RecAccessType = GEnum.RecAccessType.CustNm;
                        PopupType = (int)GEnum.PopupType.CusNm;
                        break;

                    case GEnum.SystemCode.AP_Opening_Balance:
                        RecAccessType = GEnum.RecAccessType.VendNm;
                        PopupType = (int)GEnum.PopupType.VendNm;
                        break;
                }

                key = GFunc.ConRecord_GetKey(RecAccessType, listSettingID, ConNm.Text, ref id, ref des, true);
                if (GFunc.IsNEZ(key))
                {
                    if (DocHDRUtil.EditorButton_Popup((int)objFactory.ConstantCodeKey, ConNm.Text, listSettingID, PopupType, ref key, ref id, ref des) == false)
                    {
                        ConNm.SetValueTrigger(null, false);
                        ConKey.SetValueTrigger(null, false);
                        return;
                    }
                }
                this.OpenRecord(key);
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
        private void tagrdDetail_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                bool LocKCurrency = false;
                bool LockRow = false;

                if (objFactory.IsReadOnly == false)
                {
                    if (tagrdDetail.ActiveRow != null && this.tagrdDetail.Rows.Count > 0)
                    {
                        if (GFunc.NEDec(tagrdDetail.ActiveRow.Cells["DocApplyAmtF"].Value, 0) != 0)
                            LockRow = true;

                        if (GFunc.NEDec(tagrdDetail.ActiveRow.Cells["DocRevalueRate"].Value, 0) != 0)
                            LockRow = true;

                        if (GFunc.NEBool(tagrdDetail.ActiveRow.Cells["PurgeData"].Value, false))
                            LockRow = true;

                        if (GFunc.NEDec(tagrdDetail.ActiveRow.Cells["DocCurrKey"].Value, 1) == 1)
                            LocKCurrency = true;

                        #region Cell Activation
                        foreach (UltraGridCell cell in tagrdDetail.ActiveRow.Cells)
                        {
                            switch (cell.Column.Key.ToLower())
                            {
                                case "docbranchkey":
                                case "docid":
                                case "docdate":
                                case "docdateorg":
                                case "docconkey":
                                case "docdeptkey":
                                case "docacckey":
                                case "docgrpkey":
                                case "docgrand":
                                case "doccurrkey":
                                case "docpoid":
                                case "docdoid":
                                case "docref":
                                case "docdes":
                                case "docrem":
                                case "docstatus":
                                case "custom1":
                                case "custom2":
                                case "custom3":
                                    if (LockRow)
                                        cell.Activation = Activation.ActivateOnly;
                                    else
                                        cell.Activation = Activation.AllowEdit;
                                    break;

                                case "doccurrrate":
                                case "dochome":
                                    if (LockRow)
                                        cell.Activation = Activation.ActivateOnly;
                                    else
                                    {
                                        if (LocKCurrency)
                                            cell.Activation = Activation.ActivateOnly;
                                        else
                                            cell.Activation = Activation.AllowEdit;
                                    }
                                    break;
                            }
                        }
                        #endregion

                        if (LockRow)
                            tagrdDetail.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                        else
                            tagrdDetail.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
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
        private void tagrdDetail_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {

                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = string.Empty;
                switch (e.Cell.Column.Key.ToLower())
                {
                    case "docacckey":

                        listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, e.Cell.Column.Key, tagrdDetail.Name);
                        if (DocHDRUtil.EditorButton_Popup((int)SysCode, e.Cell.Text, listSettingID, (int)GEnum.PopupType.AccID, ref key, ref id, ref des))
                        {
                            tagrdDetail.ActiveRow.Cells["DocAccKey"].Value = key;
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
        private void tagrdDetail_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            decimal? currRate = 1;
            try
            {
                
                UltraGridCell currentCell = tagrdDetail.ActiveCell;                

                //Calculation
                switch (currentCell.Column.Key.ToLower())
                {
                    #region DocCurrKey
                    case "doccurrkey":
                        if (GFunc.NEInt(currentCell.Row.Cells["DocCurrKey"].Value, 1) == 1)
                        {
                            currentCell.Row.Cells["DocCurrRate"].Value = currRate;
                            currentCell.Row.Cells["DocCurrRate"].Activation = Activation.ActivateOnly;
                            currentCell.Row.Cells["DocHome"].Activation = Activation.ActivateOnly;
                        }
                        else
                        {
                            currentCell.Row.Cells["DocCurrRate"].Activation = Activation.AllowEdit;
                            currentCell.Row.Cells["DocHome"].Activation = Activation.AllowEdit;

                            if (GFunc.IsNE(currentCell.Row.Cells["DocDate"].Value))
                                currRate = DocComUtility.CurrRate_Get((int)currentCell.Value, DateTime.Today, false);
                            else
                                currRate = DocComUtility.CurrRate_Get((int)currentCell.Value, (DateTime)currentCell.Row.Cells["DocDate"].Value, false);
                        }
                        currentCell.Row.Cells["DocCurrKey"].Value = GFunc.NEInt(currentCell.Row.Cells["DocCurrKey"].Value, 1);
                        currentCell.Row.Cells["DocCurrRate"].Value = currRate;
                        currentCell.Row.Cells["DocHome"].Value = GFunc.RndC(GFunc.NEDec(currentCell.Row.Cells["DocGrand"].Value, 0) * GFunc.NEDec(currentCell.Row.Cells["DocCurrRate"].Value, 1), GVar.RndDecs.Amtpt);
                        break;
                    #endregion

                    #region DocDate
                    case "docdate":
                        if (GFunc.NEDateTime(currentCell.Value, DateTime.MinValue) > SysOptionUtility.GetDate("TransStartDate"))
                        {
                            MsgBox.Show("Document Date must be < System Transaction Start Date.");
                            e.Cancel = true;
                        }
                        else
                        {
                            tagrdDetail.ActiveRow.Cells["DocDateOrg"].Value = currentCell.Value;
                        }

                        break;
                    #endregion

                    #region DocGrand
                    case "docgrand":
                        currentCell.Row.Cells["DocGrand"].Value = GFunc.NEDec(currentCell.Row.Cells["DocGrand"].Value, 0);
                        currentCell.Row.Cells["DocHome"].Value = GFunc.RndC(GFunc.NEDec(currentCell.Value, 0) * GFunc.NEDec(currentCell.Row.Cells["DocCurrRate"].Value, 1), GVar.RndDecs.Amtpt);
                        break;
                    #endregion

                    #region DocHome
                    case "dochome":
                        currentCell.Row.Cells["DocHome"].Value = GFunc.NEDec(currentCell.Row.Cells["DocHome"].Value, 0);
                        decimal _DocGrand = ((decimal)GFunc.NEDec(currentCell.Row.Cells["DocGrand"].Value, 0));
                        if (GFunc.NEDec(currentCell.Value, 0) == 0)
                        {
                            currentCell.Row.Cells["DocGrand"].Value = 0;
                            currentCell.Row.Cells["DocCurrRate"].Value = 1;
                        }
                        else if ((GFunc.NEDec(currentCell.Value, 0) < 0 && GFunc.NEDec(currentCell.Row.Cells["DocGrand"].Value, 0) > 0) || (GFunc.NEDec(currentCell.Value, 0) > 0 && GFunc.NEDec(currentCell.Row.Cells["DocGrand"].Value, 0) < 0))
                        {
                            MsgBox.Show("Cannot update, Doc Home must have the same sign with Doc Grand");
                            e.Cancel = true;
                        }
                        else
                            currentCell.Row.Cells["DocCurrRate"].Value = GFunc.RndDC(GFunc.NEDec(currentCell.Value, 1), _DocGrand, GVar.RndDecs.Curpt);
                        break;
                    #endregion

                    #region DocCurrRate
                    case "doccurrrate":
                        currentCell.Row.Cells["DocCurrRate"].Value = GFunc.NEDec(currentCell.Row.Cells["DocCurrRate"].Value, 1);
                        decimal _DocHome = GFunc.RndC(GFunc.NEDec(currentCell.Row.Cells["DocGrand"].Value, 0) * GFunc.NEDec(currentCell.Row.Cells["DocCurrRate"].Value, 1), GVar.RndDecs.Amtpt);
                        currentCell.Row.Cells["DocHome"].Value = GFunc.RndC(GFunc.NEDec(_DocHome, 0), GVar.RndDecs.Amtpt);
                        break;
                    #endregion

                    #region DocAccKey
                    case "docacckey":
                        tagrdDetail.DisplayLayout.Bands[0].Columns["DocAccKey"].DefaultCellValue = currentCell.Value;
                        break;
                    #endregion

                    #region DocDeptKey
                    case "docdeptkey":
                        currentCell.Row.Cells["DocDeptKey"].Value = GFunc.NEInt(currentCell.Row.Cells["DocDeptKey"].Value, 0);
                        tagrdDetail.DisplayLayout.Bands[0].Columns["DocDeptKey"].DefaultCellValue = currentCell.Value;
                        break;
                    #endregion

                    #region DocBranchKey
                    case "docbranchkey":
                        currentCell.Row.Cells["DocBranchKey"].Value = GFunc.NEInt(currentCell.Row.Cells["DocBranchKey"].Value, 0);
                        tagrdDetail.DisplayLayout.Bands[0].Columns["DocBranchKey"].DefaultCellValue = currentCell.Value;
                        break;
                    #endregion

                    #region DocGrpKey
                    case "docgrpkey":
                        currentCell.Row.Cells["DocGrpKey"].Value = GFunc.NEInt(currentCell.Row.Cells["DocGrpKey"].Value, 0);
                        tagrdDetail.DisplayLayout.Bands[0].Columns["DocGrpKey"].DefaultCellValue = currentCell.Value;
                        break;
                    #endregion
                }
                //Factory Validation
                if (objFactory.Validation_Detail(tagrdDetail.ActiveRow, currentCell.Column.Key) == false)
                {
                    e.Cancel = true;
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
        }//Completed
        private void tagrdDetail_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            try
            {              

                if (this.tagrdDetail.ActiveRow != null && formClose == false)
                    e.Cancel = !objFactory.Validation_Detail(tagrdDetail.ActiveRow, string.Empty);
               
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
        private void tagrdDetail_AfterRowUpdate(object sender, RowEventArgs e)//Completed
        {
            objFactory.IsDirty = true;
            Calculation();
        }    
        private void tagrdDetail_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                e.DisplayPromptMsg = false;
                tagrdDetail.UpdateData();
                if (tagrdDetail.Rows.Count > 0 && tagrdDetail.ActiveRow.IsAddRow == false)
                {
                    if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) != GEnum.MsgBoxButton.Delete)
                    {
                        e.Cancel = true;
                        return;
                    }
                    //Move the cursor position of active row index to upper row
                    if (tagrdDetail.ActiveRow.Index > 0)
                        PreRowIndex = tagrdDetail.ActiveRow.Index - 1;
                    return;
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
        private void tagrdDetail_AfterRowsDeleted(object sender, EventArgs e)//Completed
        {
            try
            {

            }
            catch (TAException tex)
            {
                throw Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
            objFactory.IsDirty = true;
            objFactory.ObjMSTConOpenBals.AcceptChanges();
            Calculation();
            if (tagrdDetail.Rows.Count > 0)
            {
                tagrdDetail.Rows[PreRowIndex].Selected = true;
                tagrdDetail.Rows[PreRowIndex].Activate();
                PreRowIndex = 0;
            }
        }
        private void tagrdDetail_CustomDataError(object sender, TAUtil.TAErrorEventArgs e)
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
                                case "docbranchkey":
                                case "docdeptkey":
                                case "docacckey":
                                case "docgrpkey":
                                case "doccurrkey":
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
                   
                    throw new TAException("Please enter valid date.");
                   
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
        private bool Check_IsDirty()
        {
            bool RetValue = true;
            try
            {
                if (tagrdDetail.ActiveRow != null)
                {
                    tagrdDetail.PerformAction(UltraGridAction.ExitEditMode);
                    tagrdDetail.UpdateData();
                }

                if (objFactory.IsDirty)
                {
                    if (AutoSave.Checked)
                    {
                        btnSave_Click(null, null);
                        RetValue = true;
                    }
                    else
                    {
                        switch (MsgBox.Show(MsgID.Common.SaveChanges, GEnum.MsgBoxIcon.Information, GEnum.MsgBoxButton.Save_Changes, GEnum.MsgBoxButton.Discard_Changes, GEnum.MsgBoxButton.I_Dont_Know))
                        {
                            case GEnum.MsgBoxButton.Save_Changes:
                                btnSave_Click(null, null);
                                RetValue = true;
                                break;
                            case GEnum.MsgBoxButton.Discard_Changes:
                                RetValue = true;
                                break;
                            case GEnum.MsgBoxButton.I_Dont_Know:
                                RetValue = false;
                                break;
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
            return RetValue;
        }
       
        //Error
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
