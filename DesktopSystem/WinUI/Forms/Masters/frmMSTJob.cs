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
using System.Data.SqlClient;
using System.Collections;
using TAUtil;
namespace WinUI
{
    public partial class frmMSTJob : Form
    {
        #region Local Variables

        private BOLib.MSTJobFactory objMSTJobFactory = null;
        string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        private GEnum.formInitMode formOpenMode = GEnum.formInitMode.Neither;
        private int recordKey = 0;
        private string recordID = string.Empty;
        private bool canEditRecordID = false;
        Hashtable htDetailGrd = new Hashtable();

        //GridSeq variables        

        //only use to show Read only data viewer for AuditLog 
        private bool IsOpenFromAuditLog = false;
        private DataTable _dtHeader = null;
        private DataSet _dsDetail = null;

        frmList fMSTJobList = null;
        public GVar.ListEvent_CloseFORM list_CloseMSTForm = null;
        public GVar.ListEvent_RefreshRecord ListEvent_RefreshRecord = null;
        int PreRowIndex = 0;
        #endregion

        //Initialize
        public frmMSTJob()
        {
            InitializeComponent();
        }//Completed
        public frmMSTJob(string jobID)
        {
            //For call from shortcut menu (Edit/Add)
            InitializeComponent();
            recordID = jobID;
            formOpenMode = GEnum.formInitMode.Add;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            tsbList.Enabled = false;
        }//Completed
        public frmMSTJob(int jobKey)
        {
            //For call from shortcut menu (Edit)
            InitializeComponent();
            formOpenMode = GEnum.formInitMode.Edit;
            this.recordKey = jobKey;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            tsbList.Enabled = false;
        }//Completed
        public frmMSTJob(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            InitializeComponent();
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
        }//Completed

        //Form Events
        private void frmMSTJob_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //Initialize
                this.objMSTJobFactory = new BOLib.MSTJobFactory(BOLib.GEnum.InstanceMode.Normal);
                if (objMSTJobFactory.GUID <= 0)
                {
                    formClose = true;
                    return;
                }

                //Attach Event on Factory
                this.objMSTJobFactory.ErrorNotifierHeader_Set += new GVar.UINotifierEvent(this.ErrorNotifier_Set);
                this.objMSTJobFactory.ErrorNotifierHeader_Clear += new GVar.UINotifierEvent(this.ErrorNotifier_Clear);

                //Check if user has permission to edit Record ID
                canEditRecordID = SECPermUtility.Perform("sysRecID", false);

                if (this.IsOpenFromAuditLog)
                {
                    if (objMSTJobFactory.SetReadOnlyData(_dtHeader, _dsDetail) == false)
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
                            this.JobID.SetValueTrigger(recordID, false);
                    }
                }

                //Set ContextMenu & Grid Setting & Grid Formatting
                GlobalUI.FormGrids_Set(this, (int)objMSTJobFactory.ConstantCodeKey, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objMSTJobFactory.ConstantCodeKey);
                GlobalUI.Combos_Fill(this, (int)objMSTJobFactory.ConstantCodeKey);

                //Disable Column Sorting
                tagrdMSTJobDetEst.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.Select;
                tagrdMSTJobDetEst.DisplayLayout.Override.SelectTypeCol = SelectType.None;
                tagrdMSTJobDetOther.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.Select;
                tagrdMSTJobDetOther.DisplayLayout.Override.SelectTypeCol = SelectType.None;                
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
        private void frmMSTJob_Shown(object sender, EventArgs e)
        {
            try
            {
                if (formClose)
                    this.Close();
                else
                    this.JobID.Focus();
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
        private void frmMSTJob_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool runProcess = false;
            this.formClose = true;

            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }
            if (this.formClose && objMSTJobFactory == null)
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

                if ((bool)this.objMSTJobFactory.Dispose() == false)
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
                    this.objMSTJobFactory.Dispose();
            }
        }//Completed
        private void frmMSTJob_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                    GlobalUI.Combos_Fill(this, (int)objMSTJobFactory.ConstantCodeKey);

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

        //Menu Strip Events
        private void tsbList_Click(object sender, EventArgs e)
        {
            try
            {
                if (GFunc.IsNE(fMSTJobList))
                {
                    fMSTJobList = new frmList(objMSTJobFactory.ConstantCodeKey, objMSTJobFactory.PermID);

                    //Attach events to this FORM to call events in frmList
                    this.list_CloseMSTForm += new GVar.ListEvent_CloseFORM(fMSTJobList.OnCaller_Close);
                    this.ListEvent_RefreshRecord += new GVar.ListEvent_RefreshRecord(fMSTJobList.OnCaller_Changed);

                    //Attach events to frmList to call events in this FORM
                    fMSTJobList.ListEvent_OpenRecord = new GVar.ListEvent_OpenRecord(this.OnListRecordSelected);
                    fMSTJobList.ListEvent_CloseFORM = new GVar.ListEvent_CloseFORM(this.OnList_FormClose);
                    fMSTJobList.MdiParent = frmMain.gfrmMain;
                    fMSTJobList.Show();
                }
                else
                {
                    fMSTJobList.Activate();
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
                Error(ex, true);
            }
        }//Completed
        private void tsbNew_Click(object sender, EventArgs e)
        {
            try
            {
                New_Process();
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
        private void tsbCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Copy_Process();
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
        private void btnAttachmentEdit_Click(object sender, EventArgs e)
        {
            try
            {
                frmAttachment f = new frmAttachment(objMSTJobFactory.ObjMSTJob.Attachments, (int)objMSTJobFactory.ConstantCodeKey, objMSTJobFactory.ObjMSTJob.JobKey, -1, 0);
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.Yes)
                {
                    if (objMSTJobFactory.ObjMSTJob.JobAttachment != true)//To prevent dirty    
                    {
                        JobAttachment.Checked = true;
                        objMSTJobFactory.ObjMSTJob.JobAttachment = true;
                    }
                }
                else
                {
                    if (objMSTJobFactory.ObjMSTJob.JobAttachment != false)//To prevent dirty    
                    {
                        JobAttachment.Checked = false;
                        objMSTJobFactory.ObjMSTJob.JobAttachment = false;
                    }
                }
                btnAttachmentEdit.Text = "(" + objMSTJobFactory.ObjMSTJob.Attachments.Count + ")";
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
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataRow dr in objMSTJobFactory.ObjMSTJobDetEsts.Rows)
                {
                    dr["Selected"] = true;
                }
                objMSTJobFactory.ObjMSTJobDetEsts.AcceptChanges();
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
        private void btnUnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataRow dr in objMSTJobFactory.ObjMSTJobDetEsts.Rows)
                {
                    dr["Selected"] = false;
                }
                objMSTJobFactory.ObjMSTJobDetEsts.AcceptChanges();
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
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges() == false)
                    return;

                if(objMSTJobFactory.ObjMSTJobDetEsts.AsEnumerable().Where(r=>r.Field<bool?>("Selected")==true 
                    && r.Field<int?>("DocVendorKey")>0).Count()<=0)
                {
                    MsgBox.Show("No row has been selected");
                    return;
                }


                //load FrmSelection
                frmSendSelection objfrmSendSelection = new frmSendSelection(tagrdMSTJobDetEst, (int)objMSTJobFactory.ObjMSTJob.JobKey);
                objfrmSendSelection.ShowDialog();

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
                //remove the filter
               // objMSTJobFactory.ObjMSTJobDetEsts.DefaultView.RowFilter = "";
            }

        }//Completed
        
        private void btnImportBOM_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges() == false)
                    return;

                if (objMSTJobFactory.CanImportBOM() == false)
                    return;

                tagrdMSTJobDetEst.DisplayLayout.Bands[0].SortedColumns.Clear();
                tagrdMSTJobDetEst.DisplayLayout.Bands[0].SortedColumns.Add("EstSN", false);

                DataTable dtImport = tagrdMSTJobDetEst.DataSource as DataTable;
                decimal CurrentSN = 0;
                int CurrentEstKey = 0;

                bool ReOrderSN = false;

                if (tagrdMSTJobDetEst.Rows.Count > 0)
                {
                    if (tagrdMSTJobDetEst.ActiveRow != null)
                    {
                        CurrentSN = GFunc.NEDec(tagrdMSTJobDetEst.ActiveRow.Cells["EstSN"].Value, 0M);
                        CurrentEstKey = GFunc.NEInt(tagrdMSTJobDetEst.ActiveRow.Cells["JobEstKey"].Value, 0);
                        if (tagrdMSTJobDetEst.ActiveRow.Index != objMSTJobFactory.ObjMSTJobDetEsts.Rows.Count - 1)
                            ReOrderSN = true;
                    }
                }

                frmImportSelection frm = new frmImportSelection(ref dtImport, (int)this.objMSTJobFactory.ConstantCodeKey, objMSTJobFactory.ObjMSTJob.JobKey.Value, CurrentSN, CurrentEstKey, ReOrderSN);

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    tagrdMSTJobDetEst.DataBind();
                    tagrdMSTJobDetEst.DisplayLayout.Bands[0].SortedColumns.Clear();
                    tagrdMSTJobDetEst.DisplayLayout.Bands[0].SortedColumns.Add("EstSN", false);
                    PrjCostUpdate(0);
                }
                frm.Close();
                frm = null;
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
        private void btnUpatePrice_Click(object sender, EventArgs e)
        {
            try
            {                
                if (objMSTJobFactory.ObjMSTJobDetEsts.AsEnumerable().Where(r => r.Field<bool?>("Selected") == true
                  && r.Field<int?>("DocVendorKey") > 0).Count() <= 0)
                {
                    MsgBox.Show("There is no selected record with vendor.");
                    return;
                }

                DataTable dtEstimate = (from r in objMSTJobFactory.ObjMSTJobDetEsts.AsEnumerable()
                                 where (r.Field<bool?>("Selected") == true && r.Field<int?>("DocVendorKey") > 0)
                                 select new
                                 {
                                     ConKey = r.Field<int>("DocVendorKey"),
                                     ItmKey = r.Field<int>("EstItmKey"),
                                     CurrKey = r.Field<int>("DocCurrKey"),
                                     Cost = r.Field<decimal>("EstCostF")
                                 }).AsDataTable();

                                
                SqlConnection cn = new SqlConnection(Database.BossDemoConnection);
                cn.Open();
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "MSTJobDetEst_PriceUpdate";
                    dtEstimate.TableName = "dtEstimate";
                    string xml = GFunc.ConvertDataTableToXML(dtEstimate);
                    cm.Parameters.AddWithValue("@xml",xml);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    cm.ExecuteNonQuery();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        MsgBox.Show("Updated Successfully");
                    else
                        MsgBox.Show("Update Fail");
                }
                cn.Close();

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
                    fMSTJobList.Focus();
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
            fMSTJobList = null;
            this.list_CloseMSTForm = null;
            this.ListEvent_RefreshRecord = null;
        }//Completed

        //Formating, Locking, Refreshing 
        private void Refresh_All(bool IncludeDependentCombo)
        {
            try
            {
                Refresh_Header(IncludeDependentCombo);
                Refresh_GridEstimate();
                Refresh_GridOthers();
                GridCellDefault_Set();
                CalculateTotalAmount();
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
                bdsJobDet.DataSource = objMSTJobFactory.ObjMSTJob;
                bdsJobDet.ResetBindings(false);
                if (IncludeDependentCombo)
                {
                    Refresh_DependentText(string.Empty);
                    Refresh_DependentCombo(String.Empty);
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
        private void FormLayout()
        {
            bool EnableMode = !this.objMSTJobFactory.IsReadOnly; ;
            this.tslReadOnly.Text = !EnableMode ? "Read Only" : string.Empty;

            #region Set Buttons and JobID and Grid
            if (EnableMode == false)
            {
                this.tsbSave.Enabled = false;
                this.tsbDelete.Enabled = false;
                this.tsbClear.Enabled = false;
                this.tsbCopy.Enabled = false;
                tagrdMSTJobDetEst.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
                tagrdMSTJobDetEst.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                tagrdMSTJobDetEst.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
                tagrdMSTJobDetOther.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
                tagrdMSTJobDetOther.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                tagrdMSTJobDetOther.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
            }
            else
            {
                this.tsbSave.Enabled = true;
                if (this.objMSTJobFactory.IsNew)
                {
                    this.tsbClear.Enabled = true;
                    this.tsbDelete.Enabled = false;
                    this.tsbCopy.Enabled = false;
                }
                else
                {
                    this.tsbClear.Enabled = false;
                    this.tsbDelete.Enabled = true;
                    this.tsbCopy.Enabled = true;
                }

                //Check if user has permission to edit Record ID
                if (canEditRecordID && EnableMode)
                    JobID.ReadOnly = false;
                else
                    JobID.ReadOnly = true;

                tagrdMSTJobDetEst.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;
                tagrdMSTJobDetEst.DisplayLayout.Override.AllowAddNew = AllowAddNew.TemplateOnBottom;
                tagrdMSTJobDetEst.DisplayLayout.Override.AllowDelete = DefaultableBoolean.True;
                tagrdMSTJobDetOther.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;
                tagrdMSTJobDetOther.DisplayLayout.Override.AllowAddNew = AllowAddNew.TemplateOnBottom;
                tagrdMSTJobDetOther.DisplayLayout.Override.AllowDelete = DefaultableBoolean.True;
            }
            #endregion

            #region Set Header Controls
            this.ContractAmt.Enabled = EnableMode;
            this.Custom1.Enabled = EnableMode;
            this.Custom2.Enabled = EnableMode;
            this.Custom3.Enabled = EnableMode;
            this.Custom4.Enabled = EnableMode;
            this.Custom5.Enabled = EnableMode;
            this.JobAttachment.Enabled = EnableMode;
            this.JobClass.Enabled = EnableMode;
            this.JobConKey.Enabled = EnableMode;
            this.JobConNm.Enabled = EnableMode;
            this.JobContact.Enabled = EnableMode;
            this.JobDes.Enabled = EnableMode;
            this.JobEMKey.Enabled = EnableMode;
            this.JobEndDate.Enabled = EnableMode;
            this.JobGrpKey.Enabled = EnableMode;
            this.JobID.Enabled = EnableMode;
            this.JobMemo.Enabled = EnableMode;
            this.JobPODate.Enabled = EnableMode;
            this.JobPOID.Enabled = EnableMode;
            this.JobRem.Enabled = EnableMode;
            this.JobShipMark.Enabled = EnableMode;
            this.JobShipName.Enabled = EnableMode;
            this.JobStartDate.Enabled = EnableMode;
            this.JobStatus.Enabled = EnableMode;
            this.JobSupervisor.Enabled = EnableMode;
            this.JobTgtDate.Enabled = EnableMode;
            this.RetaintionAmt.Enabled = EnableMode;
            this.RetaintionDate.Enabled = EnableMode;
            #endregion

            #region Set Grids Columns
            foreach (UltraGridColumn col in tagrdMSTJobDetEst.DisplayLayout.Bands[0].Columns)
            {
                switch (col.Key.ToLower())
                {
                    case "jobkey":
                    case "jobestkey":
                    case "estconrate":
                    case "estcosth":
                    case "estamtf":
                    case "estamth":
                    case "docdk":
                    case "docditm":
                    case "estsn":
                    case "estitmkey":
                    case "createdate":
                    case "createuserkey":
                    case "lastmodifieddate":
                    case "lastmodifieduserkey":
                    case "estitmtype":
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
            foreach (UltraGridColumn col in tagrdMSTJobDetOther.DisplayLayout.Bands[0].Columns)
            {
                switch (col.Key.ToLower())
                {
                    case "jobkey":
                    case "jobotherkey":
                    case "othitmkey":
                    case "othitmkeyselect":
                    case "othconrate":
                    case "othpriceh":
                    case "othexpamtf":
                    case "othexpamth":
                    case "othrevamtf":
                    case "othrevamth":
                    case "othpaidamth":
                    case "createdate":
                    case "createuserkey":
                    case "lastmodifieddate":
                    case "lastmodifieduserkey":
                    case "othitmtype":
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

            //Set False to everything first
            tagrdMSTJobDetOther.DisplayLayout.Bands[0].Override.AllowAddNew = AllowAddNew.No;
            tagrdMSTJobDetOther.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.False;
            tagrdMSTJobDetOther.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
            
            //Check if user has permission to edit/add/delete Detail tab
            if (SECPermUtility.Add(GVar.PermissionID.Job_Detail, false))
            {
                tagrdMSTJobDetOther.DisplayLayout.Bands[0].Override.AllowAddNew = AllowAddNew.TemplateOnBottom;
            }
            if (SECPermUtility.Edit(GVar.PermissionID.Job_Detail, false))
            {
                tagrdMSTJobDetOther.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.True;
            }
            if (SECPermUtility.Delete(GVar.PermissionID.Job_Detail, false))
            {
                tagrdMSTJobDetOther.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.True;
            }

        }//Completed
        private void Refresh_GridEstimate()
        {
            tagrdMSTJobDetEst.DataSource = objMSTJobFactory.ObjMSTJobDetEsts;
            tagrdMSTJobDetEst.Rows.Refresh(RefreshRow.ReloadData);

             foreach(UltraGridRow row in tagrdMSTJobDetEst.Rows)
                if (GFunc.NEBool(row.Cells["HighLight"].Value, false) == true)
                    row.Appearance.BackColor = Color.LightYellow;

        }//Completed
        private void Refresh_GridOthers()
        {
            tagrdMSTJobDetOther.DataSource = objMSTJobFactory.ObjMSTJobDetOthers;
            tagrdMSTJobDetOther.Rows.Refresh(RefreshRow.ReloadData);
            
        }//Completed
        private void Refresh_DependentText(string controlNm)
        {
            //If controlNm is Empty, it will refresh all control, else it will only refresh that control only
            //retain the factory isdirty state as we do not want to change due to propertychange event
            bool FactoryIsDirty = objMSTJobFactory.IsDirty;

            MSTCon objCon;
            try
            {
                #region Customer
                if (GFunc.CompareString(controlNm , "JobConID") || controlNm == string.Empty)
                {
                    if (GFunc.IsNE(objMSTJobFactory.ObjMSTJob.JobConKey) == false)
                    {
                        objCon = MSTCon.Get(objMSTJobFactory.ObjMSTJob.JobConKey);
                        JobConNm.SetValueTrigger(objCon.ConNm, false);
                        objMSTJobFactory.ObjMSTJob.JobConID = objCon.ConID;
                        objMSTJobFactory.ObjMSTJob.JobConNm = objCon.ConNm;
                        objCon = null;
                    }
                    else
                    {
                        JobConNm.SetValueTrigger(string.Empty, false);
                    }
                }
                #endregion

                objMSTJobFactory.IsDirty = FactoryIsDirty;
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
        private void Refresh_DependentCombo(string ctrlNm)
        {
            //When CtrlNm is Empty it mean refresh all dependant combo
            bool FactoryIsDirty = objMSTJobFactory.IsDirty;
            try
            {
                if (GFunc.CompareString(ctrlNm, "JobShipName") || ctrlNm == string.Empty)
                {
                    JobShipName.SetValueTrigger(null, false);
                    JobShipMark.SetValueTrigger(null, false);

                    GlobalUI.BindComboValue(JobShipName, GVar.ListSettingID.MSTShipNameByConKey + "%" + GFunc.NEInt(JobConKey.Value, 0), "ShipName", "ShipName", 0);
                }
                objMSTJobFactory.IsDirty = FactoryIsDirty;
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
            #region tagrdMSTJobDetEst
            this.tagrdMSTJobDetEst.DisplayLayout.Bands[0].Columns["JobKey"].DefaultCellValue = objMSTJobFactory.ObjMSTJob.JobKey;
            this.tagrdMSTJobDetEst.DisplayLayout.Bands[0].Columns["JobPhaseKey"].DefaultCellValue = 0;
            this.tagrdMSTJobDetEst.DisplayLayout.Bands[0].Columns["JobTaskKey"].DefaultCellValue = 0;
            this.tagrdMSTJobDetEst.DisplayLayout.Bands[0].Columns["JobCostTypeKey"].DefaultCellValue = 0;
            this.tagrdMSTJobDetEst.DisplayLayout.Bands[0].Columns["DocCurrKey"].DefaultCellValue = 1;
            this.tagrdMSTJobDetEst.DisplayLayout.Bands[0].Columns["DocCurrRate"].DefaultCellValue = 1;
            this.tagrdMSTJobDetEst.DisplayLayout.Bands[0].Columns["EstConRate"].DefaultCellValue = 1;
            this.tagrdMSTJobDetEst.DisplayLayout.Bands[0].Columns["EstCostF"].DefaultCellValue = 0;
            this.tagrdMSTJobDetEst.DisplayLayout.Bands[0].Columns["EstCostH"].DefaultCellValue = 0;
            this.tagrdMSTJobDetEst.DisplayLayout.Bands[0].Columns["EstAmtF"].DefaultCellValue = 0;
            this.tagrdMSTJobDetEst.DisplayLayout.Bands[0].Columns["EstAmtH"].DefaultCellValue = 0;
            this.tagrdMSTJobDetEst.DisplayLayout.Bands[0].Columns["TransmitMode"].DefaultCellValue = 10;        //email
            this.tagrdMSTJobDetEst.DisplayLayout.Bands[0].Columns["TransmitStatus"].DefaultCellValue = 30;      //Pending
            this.tagrdMSTJobDetEst.DisplayLayout.Bands[0].Columns["EstSN"].DefaultCellValue = 0;
            #endregion

            #region tagrdMSTJobDetOther
            this.tagrdMSTJobDetOther.DisplayLayout.Bands[0].Columns["JobKey"].DefaultCellValue = objMSTJobFactory.ObjMSTJob.JobKey;
            this.tagrdMSTJobDetOther.DisplayLayout.Bands[0].Columns["JobPhaseKey"].DefaultCellValue = 0;
            this.tagrdMSTJobDetOther.DisplayLayout.Bands[0].Columns["JobTaskKey"].DefaultCellValue = 0;
            this.tagrdMSTJobDetOther.DisplayLayout.Bands[0].Columns["JobCostTypeKey"].DefaultCellValue = 0;
            this.tagrdMSTJobDetOther.DisplayLayout.Bands[0].Columns["OthLineType"].DefaultCellValue = 310;  //Other Expenses
            this.tagrdMSTJobDetOther.DisplayLayout.Bands[0].Columns["DocCurrKey"].DefaultCellValue = 1;
            this.tagrdMSTJobDetOther.DisplayLayout.Bands[0].Columns["DocCurrRate"].DefaultCellValue = 1;
            this.tagrdMSTJobDetOther.DisplayLayout.Bands[0].Columns["OthConRate"].DefaultCellValue = 1;
            this.tagrdMSTJobDetOther.DisplayLayout.Bands[0].Columns["OthPriceF"].DefaultCellValue = 0;
            this.tagrdMSTJobDetOther.DisplayLayout.Bands[0].Columns["OthPriceH"].DefaultCellValue = 0;
            this.tagrdMSTJobDetOther.DisplayLayout.Bands[0].Columns["OthExpAmtF"].DefaultCellValue = 0;
            this.tagrdMSTJobDetOther.DisplayLayout.Bands[0].Columns["OthExpAmtH"].DefaultCellValue = 0;
            this.tagrdMSTJobDetOther.DisplayLayout.Bands[0].Columns["OthRevAmtF"].DefaultCellValue = 0;
            this.tagrdMSTJobDetOther.DisplayLayout.Bands[0].Columns["OthRevAmtH"].DefaultCellValue = 0;
            this.tagrdMSTJobDetOther.DisplayLayout.Bands[0].Columns["OthPaidAmtF"].DefaultCellValue = 0;
            this.tagrdMSTJobDetOther.DisplayLayout.Bands[0].Columns["OthPaidAmtH"].DefaultCellValue = 0;
            #endregion

        }//Completed
        private void GridCurrentRowLocking(string grdName)
        {
            UltraGrid grd;

            if (objMSTJobFactory.IsReadOnly)
                return;

            switch (grdName.ToLower())
            {
                #region tagrdMSTJobDetEst
                case "tagrdmstjobdetest":
                    grd = tagrdMSTJobDetEst;
                    if (grd.ActiveRow == null)
                        return;
                    
                    grd.ActiveRow.Cells["ItmStock"].Activation = Activation.ActivateOnly;
                    int ItmType = GFunc.NEInt(grd.ActiveRow.Cells["EstItmType"].Value, 0);

                    switch (ItmType)
                    {
                        case (int)GEnum.ItemType.Stock:                        
                        case (int)GEnum.ItemType.Finished_GDB:
                        case (int)GEnum.ItemType.Serial_Finished_GDB:
                        case (int)GEnum.ItemType.Serial_StockB:
                        case (int)GEnum.ItemType.StockB:
                        case (int)GEnum.ItemType.Finished_GD:                        
                        case (int)GEnum.ItemType.Consignment:
                        case (int)GEnum.ItemType.Assembly:
                        case (int)GEnum.ItemType.Non_Stock:
                        case (int)GEnum.ItemType.Service:                            
                            grd.ActiveRow.Cells["EstUOMKey"].Activation = Activation.AllowEdit;
                            grd.ActiveRow.Cells["EstQty"].Activation = Activation.AllowEdit;
                            grd.ActiveRow.Cells["EstCostF"].Activation = (ItmType!=(int)GEnum.ItemType.Stock)? Activation.AllowEdit: Activation.ActivateOnly;
                            break;

                        default:                                                  
                            grd.ActiveRow.Cells["EstUOMKey"].Activation = Activation.ActivateOnly;
                            grd.ActiveRow.Cells["EstQty"].Activation = Activation.ActivateOnly;
                            break;

                    }

                    if (GFunc.NEInt(grd.ActiveRow.Cells["DocCurrKey"].Value, 0) == 1)
                        grd.ActiveRow.Cells["DocCurrRate"].Activation = Activation.ActivateOnly;
                    else
                        grd.ActiveRow.Cells["DocCurrRate"].Activation = Activation.AllowEdit;

                    break;
                #endregion

                #region tagrdMSTJobDetOther
                case "tagrdmstjobdetother":
                    grd = tagrdMSTJobDetOther;
                    if (grd.ActiveRow == null)
                        return;

                    switch (GFunc.NEInt(grd.ActiveRow.Cells["OthItmType"].Value,0))
                    {
                        case (int)GEnum.ItemType.Finished_GDB:
                        case (int)GEnum.ItemType.Serial_Finished_GDB:
                        case (int)GEnum.ItemType.Serial_StockB:
                        case (int)GEnum.ItemType.StockB:
                        case (int)GEnum.ItemType.Finished_GD:
                        case (int)GEnum.ItemType.Stock:
                        case (int)GEnum.ItemType.Consignment:
                        case (int)GEnum.ItemType.Assembly:
                        case (int)GEnum.ItemType.Non_Stock:
                        case (int)GEnum.ItemType.Service:
                            grd.ActiveRow.Cells["OthUOMKey"].Activation = Activation.AllowEdit;
                            grd.ActiveRow.Cells["OthQty"].Activation = Activation.AllowEdit;
                            break;

                        default:
                            grd.ActiveRow.Cells["OthUOMKey"].Activation = Activation.ActivateOnly;
                            grd.ActiveRow.Cells["OthQty"].Activation = Activation.ActivateOnly;
                            break;

                    }

                    if (GFunc.NEInt(grd.ActiveRow.Cells["DocCurrKey"].Value, 0) == 1)
                        grd.ActiveRow.Cells["DocCurrRate"].Activation = Activation.ActivateOnly;
                    else
                        grd.ActiveRow.Cells["DocCurrRate"].Activation = Activation.AllowEdit;

                    break;
                #endregion

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
                        this.objMSTJobFactory.IsDirty = false;
                        IsGridsDirty(true);
                    }
                }

                this.errorProvider1.Clear();

                if (this.objMSTJobFactory.IsDirty)
                {
                    if (Save_Process() == false)
                        return false;
                }

                if (this.objMSTJobFactory.New() == false)
                {
                    tagrdMSTJobDetEst.HeaderObjectKey = string.Empty;
                    tagrdMSTJobDetEst.DetailObjectKey = 0;
                    tagrdMSTJobDetOther.HeaderObjectKey = string.Empty;
                    tagrdMSTJobDetOther.DetailObjectKey = 0;
                  
                    return false;
                }
                else
                {                   
                    this.errorProvider1.Clear();                    
                    this.JobID.Focus();
                    this.objMSTJobFactory.IsDirty = false;
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

                if (objMSTJobFactory.IsDirty)
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

                if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                {
                    //if (GFunc.IsNEZ(objMSTJobFactory.ObjMSTJob.MaxMarkupSalePercent))
                    //{
                    //    MsgBox.Show("Please enter Maximum markup % for Sale");
                    //    tabDetailList.Tabs[1].Selected = true;
                    //    tanuMaxMarkup.Focus();
                    //    return false;
                    //}
                    //else if (GFunc.IsNEZ(objMSTJobFactory.ObjMSTJob.MinMarkupSalePercent))
                    //{
                    //    MsgBox.Show("Please enter Minimum markup % for Sale");
                    //    tabDetailList.Tabs[1].Selected = true;
                    //    tanuMaxMarkup.Focus();
                    //    return false;
                    //}
                    //Perform Saving

                    if (objMSTJobFactory.ObjMSTJob.JobStatus == 30 && !GFunc.IsNEZ(objMSTJobFactory.ObjMSTJob.JobEMKey))
                    {
                        string msg = "The system will send an email to the sales team if the Job Status is set to \"Complete.\" Are you sure you want to proceed?";

                        if (objMSTJobFactory.ObjMSTJob.Custom5.ToLower().Contains("sen"))
                        {
                            msg = "Would you like to send email to the sales team about the job update?";
                            if (MsgBox.Show(msg, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton1, GEnum.MsgBoxButton.Save_Only, GEnum.MsgBoxButton.Save_Send_Email) == GEnum.MsgBoxButton.Save_Send_Email)
                                objMSTJobFactory.ObjMSTJob.Custom5 = "Resend";
                        }
                        else if (MsgBox.Show(msg, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                            return true;
                    }
                }
                if (this.objMSTJobFactory.Save())
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

                if (SECPermUtility.Edit(objMSTJobFactory.PermID, false))
                {
                    if (objMSTJobFactory.GetEdit(key, id) == false)
                    {
                        if (SysOptionUtility.GetBool("WarnOpenRecordAsReadOnly"))
                        {
                            btnSelect = MsgBox.Show("Try to open record as read only",
                                                  GEnum.MsgBoxIcon.Question,
                                                  GEnum.MsgBoxButton.Yes,
                                                  GEnum.MsgBoxButton.No);

                            if (btnSelect == GEnum.MsgBoxButton.Yes)
                            {
                                objMSTJobFactory.GetReadOnly(key, id);
                            }
                        }
                    }
                    else
                    {
                        tagrdMSTJobDetEst.HeaderObjectKey = string.Empty;
                        tagrdMSTJobDetEst.DetailObjectKey = 0;
                        tagrdMSTJobDetOther.HeaderObjectKey = string.Empty;
                        tagrdMSTJobDetOther.DetailObjectKey = 0;
                    }
                }
                else
                    objMSTJobFactory.GetReadOnly(key, id);

                btnAttachmentEdit.Text = "(" + objMSTJobFactory.ObjMSTJob.Attachments.Count + ")";
               
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

                if (this.objMSTJobFactory.Delete())
                {
                    IsGridsDirty(true);
                    if (!GFunc.IsNE(this.ListEvent_RefreshRecord))
                        ListEvent_RefreshRecord.Invoke();

                    this.objMSTJobFactory.New();
                  
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
                if (GFunc.IsNEZ(this.objMSTJobFactory.ObjMSTJob.JobKey))
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

                    if (this.objMSTJobFactory.New())
                    {
                        tagrdMSTJobDetEst.HeaderObjectKey = string.Empty;
                        tagrdMSTJobDetEst.DetailObjectKey = 0;
                        tagrdMSTJobDetOther.HeaderObjectKey = string.Empty;
                        tagrdMSTJobDetOther.DetailObjectKey = 0;
                      
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
                this.tagrdMSTJobDetEst.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdMSTJobDetEst.UpdateData();
                this.tagrdMSTJobDetOther.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdMSTJobDetOther.UpdateData();

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
        private bool Copy_Process()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (form_CanValidate() == false)
                    return false;

                if (objMSTJobFactory.IsNew && !objMSTJobFactory.IsDirty)
                    return false;

                if (objMSTJobFactory.IsDirty)
                {
                    if (Save_Process() == false)
                        return false;
                }

                this.errorProvider1.Clear();
                this.objMSTJobFactory.CopyMyself();
                this.Refresh_All(true);
                this.FormLayout();
                this.JobID.Focus();
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
            }
        }//Completed        
        private bool IsGridsDirty(bool undoChangesInGrid)
        {
            //This function check if the grid has uncommited data in its active orw
            //it also has an option to undo those uncommited changes. 

            #region tagrdMSTJobDetEst
            if (tagrdMSTJobDetEst.ActiveRow != null)
            {
                if (tagrdMSTJobDetEst.ActiveRow.DataChanged && !tagrdMSTJobDetEst.ActiveRow.IsUnmodifiedTemplateAddRow)
                {
                    if (undoChangesInGrid)
                    {
                        //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                        this.tagrdMSTJobDetEst.PerformAction(UltraGridAction.UndoCell);
                        this.tagrdMSTJobDetEst.PerformAction(UltraGridAction.UndoRow);
                    }
                    return true;
                }
            }
            #endregion

            #region tagrdMSTJobDetOther
            if (tagrdMSTJobDetOther.ActiveRow != null)
            {
                if (tagrdMSTJobDetOther.ActiveRow.DataChanged && !tagrdMSTJobDetOther.ActiveRow.IsUnmodifiedTemplateAddRow)
                {
                    if (undoChangesInGrid)
                    {
                        //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                        this.tagrdMSTJobDetOther.PerformAction(UltraGridAction.UndoCell);
                        this.tagrdMSTJobDetOther.PerformAction(UltraGridAction.UndoRow);
                    }
                    return true;
                }
            }
            #endregion

            return false;
        }//Completed
        private void Calculation(string grdNm)
        {
            try
            {
                if (grdNm == tagrdMSTJobDetOther.Name)
                    JobDetOtherCalculation();
                else
                    JobDetEstCalculation();

                CalculateTotalAmount();
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
        private void JobDetOtherCalculation()
        {
            try
            {
                UltraGridCell currentCell = tagrdMSTJobDetOther.ActiveCell;
                decimal currRate = GFunc.RndC(currentCell.Row.Cells["DocCurrRate"].Value, GVar.RndDecs.Curpt);
                decimal qty = 0;
                decimal price = 0;
                decimal amt = 0;

                switch (GFunc.GetINTypeGroup(currentCell.Row.Cells["OthItmType"].Value))
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                        qty = GFunc.RndC(currentCell.Row.Cells["OthQty"].Value, GVar.RndDecs.Qtypt);
                        price = GFunc.RndC(currentCell.Row.Cells["OthPriceF"].Value, GVar.RndDecs.Prcpt);
                        amt = GFunc.RndC(qty * price, GVar.RndDecs.Amtpt);
                        currentCell.Row.Cells["OthQty"].Value = qty;
                        currentCell.Row.Cells["OthPriceF"].Value = price;
                        currentCell.Row.Cells["OthPriceH"].Value = GFunc.RndC(price * currRate, GVar.RndDecs.Prcpt);
                        break;

                    default:
                        amt = GFunc.RndC(currentCell.Row.Cells["OthPriceF"].Value, GVar.RndDecs.Amtpt);
                        currentCell.Row.Cells["OthQty"].Value = DBNull.Value;
                        currentCell.Row.Cells["OthPriceF"].Value = amt;
                        currentCell.Row.Cells["OthPriceH"].Value = GFunc.RndC(amt * currRate, GVar.RndDecs.Amtpt);
                        break;
                }
                if ((int)currentCell.Row.Cells["OthLineType"].Value == 320 || (int)currentCell.Row.Cells["OthLineType"].Value == 360)//Revenue
                {
                    currentCell.Row.Cells["OthExpAmtF"].Value = 0;
                    currentCell.Row.Cells["OthExpAmtH"].Value = 0;
                    currentCell.Row.Cells["OthRevAmtF"].Value = amt;
                    currentCell.Row.Cells["OthRevAmtH"].Value = GFunc.RndC(amt * currRate, GVar.RndDecs.Amtpt);
                }
                else
                {
                    currentCell.Row.Cells["OthExpAmtF"].Value = amt;
                    currentCell.Row.Cells["OthExpAmtH"].Value = GFunc.RndC(amt * currRate, GVar.RndDecs.Amtpt);
                    currentCell.Row.Cells["OthRevAmtF"].Value = 0;
                    currentCell.Row.Cells["OthRevAmtH"].Value = 0;
                }
                currentCell.Row.Cells["DocCurrRate"].Value = currRate;
                currentCell.Row.Cells["OthPaidAmtH"].Value = GFunc.RndC(GFunc.NEDec(currentCell.Row.Cells["OthPaidAmtF"].Value, 0) * currRate, GVar.RndDecs.Amtpt);
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
        private void JobDetEstCalculation()
        {
            try
            {
                UltraGridCell currentCell = tagrdMSTJobDetEst.ActiveCell;
                decimal currRate = GFunc.RndC(currentCell.Row.Cells["DocCurrRate"].Value, GVar.RndDecs.Curpt);
                decimal qty = 0;
                decimal cost = 0;
                decimal amt = 0;

                switch (GFunc.GetINTypeGroup(currentCell.Row.Cells["EstItmType"].Value))
                {
                    case (int)GEnum.INTypeGrp.Stock:
                    case (int)GEnum.INTypeGrp.Non_Stock:
                        qty = GFunc.RndC(currentCell.Row.Cells["EstQty"].Value, GVar.RndDecs.Qtypt);
                        cost = GFunc.RndC(currentCell.Row.Cells["EstCostF"].Value, GVar.RndDecs.Prcpt);
                        amt = GFunc.RndC(qty * cost, GVar.RndDecs.Amtpt);
                        currentCell.Row.Cells["EstQty"].Value = qty;
                        currentCell.Row.Cells["EstCostF"].Value = cost;
                        currentCell.Row.Cells["EstCostH"].Value = GFunc.RndC(cost * currRate, GVar.RndDecs.Prcpt);
                        break;

                    default:
                        amt = GFunc.RndC(currentCell.Row.Cells["EstCostF"].Value, GVar.RndDecs.Amtpt);
                        currentCell.Row.Cells["EstQty"].Value = DBNull.Value;
                        currentCell.Row.Cells["EstCostF"].Value = amt;
                        currentCell.Row.Cells["EstCostH"].Value = GFunc.RndC(amt * currRate, GVar.RndDecs.Amtpt);
                        break;
                }
                currentCell.Row.Cells["DocCurrRate"].Value = currRate;
                currentCell.Row.Cells["EstAmtF"].Value = amt;
                currentCell.Row.Cells["EstAmtH"].Value = GFunc.RndC(amt * currRate, GVar.RndDecs.Amtpt);

                if (GFunc.IsNEZ(currentCell.Row.Cells["PrjCostRate"].Value))
                    currentCell.Row.Cells["PrjCostRate"].Value = 0;
                currentCell.Row.Cells["PrjCost"].Value = amt * currRate * (1+GFunc.NEDec(currentCell.Row.Cells["PrjCostRate"].Value,0)/100M);
                
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
        private void CalculateTotalAmount()
        {
            try
            {
                decimal sn = 0;
                decimal TotalEstAmt = 0;
                decimal TotalPrjAmt = 0;
                decimal TotalOthExpAmt = 0;
                decimal TotalOthRevAmt = 0;
                decimal TotalOthPaidAmt = 0;
                foreach (DataRow drEst in objMSTJobFactory.ObjMSTJobDetEsts.Rows)
                {
                    sn = sn + 1;
                    drEst["EstSN"] = sn;
                    TotalEstAmt = TotalEstAmt + GFunc.NEDec(drEst["EstAmtH"], 0);
                    TotalPrjAmt += GFunc.NEDec(drEst["PrjCost"], 0);
                }

                TotalEstAmountH.SetValueTrigger(TotalEstAmt, false);
                tanuPrjCost.SetValueTrigger(TotalPrjAmt, false);

                foreach (DataRow drOth in objMSTJobFactory.ObjMSTJobDetOthers.Rows)
                {
                    TotalOthExpAmt = TotalOthExpAmt + GFunc.NEDec(drOth["OthExpAmtH"], 0);
                    TotalOthRevAmt = TotalOthRevAmt + GFunc.NEDec(drOth["OthRevAmtH"], 0);
                    TotalOthPaidAmt = TotalOthPaidAmt + GFunc.NEDec(drOth["OthPaidAmtH"], 0);
                }
                TotalOthExpAmountH.SetValueTrigger(TotalOthExpAmt,false);
                TotalOthRevAmountH.SetValueTrigger(TotalOthRevAmt,false);
                TotalOthPaidAmountH.SetValueTrigger(TotalOthPaidAmt, false);
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

        //Tab Events
        private void tabDetailList_KeyDown(object sender, KeyEventArgs e)
        {
            switch (tabDetailList.ActiveTab.Key.ToLower())
            {
                case "general":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                        case Keys.Down:
                            JobClass.Focus();
                            break;
                        case Keys.Up:
                            JobContact.Focus();
                            break;
                    }
                    break;
                case "memo":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                        case Keys.Down:
                            JobMemo.Focus();
                            break;
                        case Keys.Up:
                            JobContact.Focus();
                            break;
                    }
                    break;
                case "estimate":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                        case Keys.Down:
                            tagrdMSTJobDetEst.Focus();
                            UltraGridColumn FirstVisCol = tagrdMSTJobDetEst.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                            if (FirstVisCol != null)
                            {
                                tagrdMSTJobDetEst.ActiveCell = tagrdMSTJobDetEst.Rows.TemplateAddRow.Cells[FirstVisCol.Key];
                                tagrdMSTJobDetEst.PerformAction(UltraGridAction.EnterEditMode, false, false);
                            }
                            break;
                        case Keys.Up:
                            JobContact.Focus();
                            break;
                    }
                    break;
                case "detail":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                        case Keys.Down:
                            tagrdMSTJobDetOther.Focus();
                            UltraGridColumn FirstVisCol = tagrdMSTJobDetOther.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                            if (FirstVisCol != null)
                            {
                                tagrdMSTJobDetOther.ActiveCell = tagrdMSTJobDetOther.Rows.TemplateAddRow.Cells[FirstVisCol.Key];
                                tagrdMSTJobDetOther.PerformAction(UltraGridAction.EnterEditMode, false, false);
                            }
                            break;
                        case Keys.Up:
                            JobContact.Focus();
                            break;
                    }
                    break;
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
                    key = GFunc.JobRecord_GetKey(GEnum.RecAccessType.JobDes, listSettingID, OpenID.Text, 0, ref id, ref des, true);
                    OpenID.Text = des;
                    if (GFunc.IsNEZ(key))
                    {
                        if (DocHDRUtil.EditorButton_Popup((int)objMSTJobFactory.ConstantCodeKey, OpenID.Text, listSettingID, (int)GEnum.PopupType.JobDes, ref key, ref id, ref des) == false)
                            return;
                    }

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
                if (DocHDRUtil.EditorButton_Popup((int)objMSTJobFactory.ConstantCodeKey, OpenID.Text, listSettingID, (int)GEnum.PopupType.JobDes, ref key, ref id, ref des))
                {
                    OpenID.Text = des;
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
        private void JobShipName_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                JobShipMark.SetValueTrigger(string.Empty, false);
                if (GFunc.NEInt(JobConKey.Value, 0) == 0 || JobShipName.Value.ToString() == string.Empty)
                    return;
                else
                {
                    this.JobShipMark.SetValueTrigger(DocHDRUtil.ShipMark_GetLast((int)JobConKey.Value, JobShipName.Text.ToString()).ToString(), false);
                    objMSTJobFactory.ObjMSTJob.JobShipMark = this.JobShipMark.Text;
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
        private void JobShipMark_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            try
            {
                int conKey = GFunc.NEInt(this.JobConKey.Value,0);
                string shipNm = GFunc.NEStr(this.JobShipName.Value,string.Empty);

                if (conKey == 0 || shipNm == string.Empty)
                    return;

                int newmark = DocHDRUtil.ShipMark_GetNew(conKey, shipNm);
                if (newmark > 0)
                    this.JobShipMark.SetValueTrigger(newmark, false);
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
                    case "jobconkey":
                    case "jobconnm":
                        objMSTJobFactory.ObjMSTJob.JobConKey = key;
                        objMSTJobFactory.ObjMSTJob.JobConNm = des;
                        break;

                    case "docvendorkey":
                    case "docvendornm":
                        tagrdMSTJobDetEst.ActiveRow.Cells["DocVendorKey"].Value = key;
                        tagrdMSTJobDetEst.ActiveRow.Cells["DocVendorID"].Value = id;
                        tagrdMSTJobDetEst.ActiveRow.Cells["DocVendorNm"].Value = des;
                        break;

                    case "estitmid":
                    case "estitmdes":
                        tagrdMSTJobDetEst.ActiveRow.Cells["EstItmKey"].Value = key;
                        tagrdMSTJobDetEst.ActiveRow.Cells["EstItmID"].Value = id;
                        tagrdMSTJobDetEst.ActiveRow.Cells["EstItmDes"].Value = des;
                        tagrdMSTJobDetEst.ActiveRow.Cells["EstItmKeySelect"].Value = key;
                        break;

                    case "othitmid":
                    case "othitmdes":
                        tagrdMSTJobDetOther.ActiveRow.Cells["OthItmKey"].Value = key;
                        tagrdMSTJobDetOther.ActiveRow.Cells["OthItmID"].Value = id;
                        tagrdMSTJobDetOther.ActiveRow.Cells["OthItmDes"].Value = des;
                        tagrdMSTJobDetOther.ActiveRow.Cells["OthItmKeySelect"].Value = key;
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
                    case "jobconkey":
                        PopupType = (int)GEnum.PopupType.CusID;
                        AccessType = (int)GEnum.RecAccessType.CustID;
                        keySearch = "Con";
                        break;

                    case "docvendorkey":
                        PopupType = (int)GEnum.PopupType.VendID;
                        AccessType = (int)GEnum.RecAccessType.VendID;
                        keySearch = "Con";
                        break;

                    case "jobconnm":
                        PopupType = (int)GEnum.PopupType.CusNm;
                        AccessType = (int)GEnum.RecAccessType.CustNm;
                        keySearch = "Con";
                        break;

                    case "docvendornm":
                        PopupType = (int)GEnum.PopupType.VendNm;
                        AccessType = (int)GEnum.RecAccessType.VendNm;
                        keySearch = "Con";
                        break;

                    case "estitmid":
                    case "othitmid":
                        PopupType = (int)GEnum.PopupType.ItmID;
                        AccessType = (int)GEnum.RecAccessType.ItemID;
                        keySearch = "Itm";
                        break;

                    case "estitmdes":
                    case "othitmdes":
                        PopupType = (int)GEnum.PopupType.ItmDes;
                        AccessType = (int)GEnum.RecAccessType.ItemDes;
                        keySearch = "Itm";
                        break;
                }

                if (FromButtonClick)
                {
                    if (DocHDRUtil.EditorButton_Popup((int)objMSTJobFactory.ConstantCodeKey, controlText, listSettingID, PopupType, ref key, ref id, ref des))
                        RecSearchSelected(sender, FieldName, key, id, des);
                    else
                        return false;
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
                                key = GFunc.AccRecord_GetKey((GEnum.RecAccessType)AccessType, listSettingID, controlText, ref id, ref des, false);
                                break;
                            default:    //Itm
                                key = GFunc.ItmRecord_GetKey((GEnum.RecAccessType)AccessType, listSettingID, controlText, vendorKey, ref id, ref des, false);
                                break;

                        }
                        if (GFunc.IsNEZ(key))
                        {
                            //since value input by user cannot be match let the user select from Popup form
                            if (DocHDRUtil.EditorButton_Popup((int)objMSTJobFactory.ConstantCodeKey, controlText, listSettingID, PopupType, ref key, ref id, ref des))
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
        private void JobConKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (RecSearchProcess(sender, string.Empty, false))
                {
                    Refresh_DependentCombo(JobShipName.Name);
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
            
        }//Completed
        private void JobConKey_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            if (RecSearchProcess(sender, string.Empty, true))
                Refresh_DependentCombo(JobShipName.Name);
        }//Completed
        private void JobConNm_CustomUpdate(object sender, CancelEventArgs e)
        {
            if (RecSearchProcess(sender, string.Empty, false))
                Refresh_DependentCombo(JobShipName.Name);
            else
                e.Cancel = true;
        }//Completed
        private void JobConNm_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            if (RecSearchProcess(sender, string.Empty, true))
                Refresh_DependentCombo(JobShipName.Name);
        }//Completed

        //Grid JobEstimate Events
        private void tagrdMSTJobDetEst_AfterRowActivate(object sender, EventArgs e)
        {
            GridCurrentRowLocking(tagrdMSTJobDetEst.Name);            
        }//Completed
        private void tagrdMSTJobDetEst_AfterRowInsert(object sender, RowEventArgs e)
        {
            //maxEstKey = maxEstKey + 1;
            //e.Row.Cells["JobEstKey"].Value = maxEstKey;

            //e.Row.Cells["EstSN"].Value = objMSTJobFactory.ObjMSTJobDetEsts.Rows.Count > 0 ?
            //    this.objMSTJobFactory.ObjMSTJobDetEsts.AsEnumerable().ToList().Max(o =>
            //                (o.Field<decimal>("EstSN"))) + 1.0M : 1.0M;
        }//Completed
        private void tagrdMSTJobDetEst_BeforeRowInsert(object sender, BeforeRowInsertEventArgs e)
        {
            try
            {
                DocDetUtil.AutoIncrement((int)this.objMSTJobFactory.ConstantCodeKey, tagrdMSTJobDetEst);
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
        private void tagrdMSTJobDetEst_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                switch (e.Cell.Column.Key.ToLower())
                {
                    case "estitmid":
                    case "estitmdes":
                        if (RecSearchProcess(sender, e.Cell.Column.Key, true))
                        {
                            if (objMSTJobFactory.Validation_Detail(tagrdMSTJobDetEst.Name, tagrdMSTJobDetEst.ActiveRow, tagrdMSTJobDetEst.ActiveCell.Column.Key))
                            {
                                int ItmKey = GFunc.NEInt(tagrdMSTJobDetEst.ActiveRow.Cells["EstItmKeySelect"].Value, 0);
                                DataTable dt = objMSTJobFactory.GetItemInfo(ItmKey);
                                if (dt == null)
                                {
                                    tagrdMSTJobDetEst.ActiveRow.Cells["EstItmKey"].Value = DBNull.Value;
                                    tagrdMSTJobDetEst.ActiveRow.Cells["EstItmType"].Value = 0;
                                    tagrdMSTJobDetEst.ActiveRow.Cells["EstUOMKey"].Value = DBNull.Value;
                                }
                                else if (dt.Rows.Count > 0)
                                {
                                    tagrdMSTJobDetEst.ActiveRow.Cells["EstItmKey"].Value = ItmKey;
                                    tagrdMSTJobDetEst.ActiveRow.Cells["EstItmType"].Value = GFunc.NEInt(dt.Rows[0]["ItmType"], 0);
                                    tagrdMSTJobDetEst.ActiveRow.Cells["EstUOMKey"].Value = GFunc.NEInt(dt.Rows[0]["BUOMKey"], 0);
                                    tagrdMSTJobDetEst.ActiveRow.Cells["ItmStock"].Value = GFunc.NEDec(dt.Rows[0]["AvailableQty"], 0);
                                    tagrdMSTJobDetEst.ActiveRow.Cells["EstCostH"].Value = GFunc.NEDec(dt.Rows[0]["CostLanded"], 0);
                                    tagrdMSTJobDetEst.ActiveRow.Cells["EstCostF"].Value = GFunc.NEDec(dt.Rows[0]["CostLanded"], 0);
                                }
                                JobDetEstCalculation();
                                GridCurrentRowLocking(tagrdMSTJobDetEst.Name);                               
                            }
                        }
                        break;

                    case "docvendornm":
                    case "docvendorkey":
                        if (RecSearchProcess(sender, e.Cell.Column.Key, true))
                            objMSTJobFactory.Validation_Detail(tagrdMSTJobDetEst.Name, tagrdMSTJobDetEst.ActiveRow, tagrdMSTJobDetEst.ActiveCell.Column.Key);
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
        private void tagrdMSTJobDetEst_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
              

                UltraGridCell currentCell = tagrdMSTJobDetEst.ActiveCell;

                if (objMSTJobFactory.ObjMSTJob.JobStatus == 30 && objMSTJobFactory.ObjMSTJob.Custom5.ToLower().Contains("sen"))
                {
                    if (!currentCell.Column.Key.ToLower().Equals("highlight"))
                        currentCell.Row.Cells["Highlight"].Value = true;
                }
                switch (currentCell.Column.Key.ToLower())
                {
                    case "prjcostrate":                        
                        decimal costPercent = GFunc.NEDec(currentCell.Row.Cells["PrjCostRate"].Value, 0);
                        currentCell.Row.Cells["PrjCost"].Value = GFunc.NEDec(currentCell.Row.Cells["EstAmtH"].Value, 0) * (1 + costPercent / 100M);
                        currentCell.Row.Update();
                        break;
                    case "jobphasekey":
                    case "jobtaskkey":
                    case "jobcosttypekey":
                    case "transmitmode":
                    case "transmitstatus":
                        currentCell.Value = GFunc.NEInt(currentCell.Value, 0);
                        e.Cancel = !objMSTJobFactory.Validation_Detail(tagrdMSTJobDetEst.Name, tagrdMSTJobDetEst.ActiveRow, currentCell.Column.Key);
                        break;

                    case "estsn":
                    case "estitmrem":
                    case "docid":
                    case "docdes":
                    case "docetd":                    
                    case "attention":
                    case "emailaddr":
                    case "faxnumber":                    
                    case "custom1":
                    case "custom2":
                    case "custom3":
                        e.Cancel = !objMSTJobFactory.Validation_Detail(tagrdMSTJobDetEst.Name, tagrdMSTJobDetEst.ActiveRow, currentCell.Column.Key);
                        break;

                    case "estitmdes":
                        //we cannot popup the search form because for Job, the description must be free text w/o any ItmID
                        break;

                    case "estitmid":
                        if (GFunc.CompareString(currentCell.Column.Key,"EstItmDes") && !GFunc.IsNE(tagrdMSTJobDetEst.ActiveRow.Cells["EstItmID"].Value))
                            return;

                        if (RecSearchProcess(sender, e.Cell.Column.Key, false))
                        {
                            if (objMSTJobFactory.Validation_Detail(tagrdMSTJobDetEst.Name, tagrdMSTJobDetEst.ActiveRow, currentCell.Column.Key))
                            {
                                int ItmKey = GFunc.NEInt(tagrdMSTJobDetEst.ActiveRow.Cells["EstItmKeySelect"].Value, 0);
                                DataTable dt=objMSTJobFactory.GetItemInfo(ItmKey);
                                if (dt==null)
                                {
                                    tagrdMSTJobDetEst.ActiveRow.Cells["EstItmKey"].Value = DBNull.Value;
                                    tagrdMSTJobDetEst.ActiveRow.Cells["EstItmType"].Value = 0;
                                    tagrdMSTJobDetEst.ActiveRow.Cells["EstUOMKey"].Value = DBNull.Value;
                                }
                                else if(dt.Rows.Count>0)
                                {                                    
                                    tagrdMSTJobDetEst.ActiveRow.Cells["EstItmKey"].Value = ItmKey;
                                    tagrdMSTJobDetEst.ActiveRow.Cells["EstItmType"].Value = GFunc.NEInt(dt.Rows[0]["ItmType"], 0);
                                    tagrdMSTJobDetEst.ActiveRow.Cells["EstUOMKey"].Value = GFunc.NEInt(dt.Rows[0]["BUOMKey"], 0);

                                    if (GFunc.NEInt(dt.Rows[0]["ItmType"], 0) == (int)GEnum.ItemType.Stock)
                                    {
                                        tagrdMSTJobDetEst.ActiveRow.Cells["ItmStock"].Value = GFunc.NEDec(dt.Rows[0]["AvailableQty"], 0);
                                        tagrdMSTJobDetEst.ActiveRow.Cells["EstCostH"].Value = GFunc.NEDec(dt.Rows[0]["CostLanded"], 0);
                                        tagrdMSTJobDetEst.ActiveRow.Cells["EstCostF"].Value = GFunc.NEDec(dt.Rows[0]["CostLanded"], 0);
                                    }
                                }
                                JobDetEstCalculation();
                                GridCurrentRowLocking(tagrdMSTJobDetEst.Name);
                                return;
                            }
                        }   
                        e.Cancel = true;
                        break;

                    case "estqty":
                        if (objMSTJobFactory.Validation_Detail(tagrdMSTJobDetEst.Name, tagrdMSTJobDetEst.ActiveRow, currentCell.Column.Key))
                        {
                            JobDetEstCalculation();
                            return;
                        }
                        e.Cancel = true;
                        break;

                    case "estuomkey":
                        if (objMSTJobFactory.Validation_Detail(tagrdMSTJobDetEst.Name, tagrdMSTJobDetEst.ActiveRow, currentCell.Column.Key))
                        {
                            tagrdMSTJobDetEst.ActiveRow.Cells["EstConRate"].Value = DocComUtility.UOMConRate_Get(GFunc.NEInt(tagrdMSTJobDetEst.ActiveRow.Cells["EstItmKey"].Value, 0), GFunc.NEInt(tagrdMSTJobDetEst.ActiveRow.Cells["EstUOMKey"].Value, 0));
                            return;
                        }
                        e.Cancel = true;
                        break;

                    case "estcostf":
                        if (objMSTJobFactory.Validation_Detail(tagrdMSTJobDetEst.Name, tagrdMSTJobDetEst.ActiveRow, currentCell.Column.Key))
                        {
                            currentCell.Row.Cells["EstCostH"].Value = GFunc.NEDec(GFunc.NEDec(currentCell.Row.Cells["DocCurrRate"].Value, 0) * GFunc.NEDec(currentCell.Value, 0), 0);
                            JobDetEstCalculation();
                            return;
                        }
                        e.Cancel = true;
                        break;

                    case "docvendorkey":
                    case "docvendornm":
                        if (GFunc.CompareString(currentCell.Column.Key, "DocVendorNm") && !GFunc.IsNEZ(tagrdMSTJobDetEst.ActiveRow.Cells["DocVendorKey"].Value))
                            return;

                        if (RecSearchProcess(sender, e.Cell.Column.Key, false))
                        {
                            if (objMSTJobFactory.Validation_Detail(tagrdMSTJobDetEst.Name, tagrdMSTJobDetEst.ActiveRow, currentCell.Column.Key))
                                return;
                        }
                        e.Cancel = true;
                        break;

                    case "doccurrkey":
                        if (objMSTJobFactory.Validation_Detail(tagrdMSTJobDetEst.Name, tagrdMSTJobDetEst.ActiveRow, currentCell.Column.Key))
                        {
                            currentCell.Row.Cells["DocCurrRate"].Value = DocComUtility.CurrRate_Get((int)currentCell.Value, DateTime.Today, false);
                            JobDetEstCalculation();
                            GridCurrentRowLocking(tagrdMSTJobDetEst.Name);
                            return;
                        }
                        e.Cancel = true;
                        break;

                    case "doccurrrate":
                        if (objMSTJobFactory.Validation_Detail(tagrdMSTJobDetEst.Name, tagrdMSTJobDetEst.ActiveRow, currentCell.Column.Key))
                        {
                            JobDetEstCalculation();
                            return;
                        }
                        e.Cancel = true;
                        break;
                   case "highlight":
                        if (GFunc.NEBool(e.Cell.Value, false) == true)
                            e.Cell.Row.Appearance.BackColor = Color.LightYellow;
                        else
                            e.Cell.Row.Appearance.BackColor = Color.White;
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
        private void tagrdMSTJobDetEst_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            try
            {                             

                if (this.tagrdMSTJobDetEst.ActiveRow != null)
                {
                    
                    if (objMSTJobFactory.Validation_Detail(tagrdMSTJobDetEst.Name, tagrdMSTJobDetEst.ActiveRow, string.Empty) == false)
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
        private void tagrdMSTJobDetEst_AfterRowUpdate(object sender, RowEventArgs e)
        {
            try
            {
                objMSTJobFactory.IsDirty = true;
                objMSTJobFactory.ObjMSTJobDetEsts.AcceptChanges();
                CalculateTotalAmount();
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
        private void tagrdMSTJobDetEst_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                e.DisplayPromptMsg = false;
                if (tagrdMSTJobDetEst.Rows.Count > 0)
                {
                    if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) != GEnum.MsgBoxButton.Delete)
                    {
                        e.Cancel = true;
                        return;
                    }
                    
                    //Move the cursor position of active row index to upper row
                    if (tagrdMSTJobDetEst.ActiveRow.Index > 0)
                        PreRowIndex = tagrdMSTJobDetEst.ActiveRow.Index - 1;
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
        private void tagrdMSTJobDetEst_AfterRowsDeleted(object sender, EventArgs e)
        {
            try
            {
                objMSTJobFactory.IsDirty = true;
                objMSTJobFactory.ObjMSTJobDetEsts.AcceptChanges();
                CalculateTotalAmount();

                if (tagrdMSTJobDetEst.Rows.Count > 0)
                {
                    tagrdMSTJobDetEst.Rows[PreRowIndex].Selected = true;
                    tagrdMSTJobDetEst.Rows[PreRowIndex].Activate();
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
        
        //Grid JobOthers Events
        private void tagrdMSTJobDetOther_AfterRowActivate(object sender, EventArgs e)
        {
            GridCurrentRowLocking(tagrdMSTJobDetOther.Name);
        }//Completed
        private void tagrdMSTJobDetOther_AfterRowInsert(object sender, RowEventArgs e)
        {
            //maxOthKey = maxOthKey + 1;
            //e.Row.Cells["JobOtherKey"].Value = maxOthKey;
        }//Completed
        private void tagrdMSTJobDetOther_BeforeRowInsert(object sender, BeforeRowInsertEventArgs e)
        {
            try
            {
                DocDetUtil.AutoIncrement((int)this.objMSTJobFactory.ConstantCodeKey, tagrdMSTJobDetOther);
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
        private void tagrdMSTJobDetOther_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                switch (e.Cell.Column.Key.ToLower())
                {
                    case "othitmid":
                    case "othitmdes":
                        if (RecSearchProcess(sender, e.Cell.Column.Key, true))
                        {
                            if (objMSTJobFactory.Validation_Detail(tagrdMSTJobDetOther.Name, tagrdMSTJobDetOther.ActiveRow, tagrdMSTJobDetOther.ActiveCell.Column.Key))
                            {
                                MSTItm objItm = MSTItm.GetParent((int)tagrdMSTJobDetOther.ActiveRow.Cells["OthItmKeySelect"].Value);
                                tagrdMSTJobDetOther.ActiveRow.Cells["OthItmKey"].Value = objItm.ItmKey;
                                tagrdMSTJobDetOther.ActiveRow.Cells["OthItmType"].Value = objItm.ItmType;
                                tagrdMSTJobDetOther.ActiveRow.Cells["OthUOMKey"].Value = objItm.BUOMKey;
                                JobDetOtherCalculation();
                                GridCurrentRowLocking(tagrdMSTJobDetOther.Name);
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
        private void tagrdMSTJobDetOther_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            string msgID = string.Empty;
            try
            {
               

                UltraGridCell currentCell = tagrdMSTJobDetOther.ActiveCell;
                switch (currentCell.Column.Key.ToLower())
                {
                    case "jobphasekey":
                    case "jobtaskkey":
                    case "jobcosttypekey":
                    case "supervisor":
                    case "emkey":
                    case "costgrp":
                        currentCell.Value = GFunc.NEInt(currentCell.Value, 0);
                        e.Cancel = !objMSTJobFactory.Validation_Detail(tagrdMSTJobDetOther.Name, tagrdMSTJobDetOther.ActiveRow, currentCell.Column.Key);
                        break;

                    
                    case "othitmrem":
                    case "docid":
                    case "docdate":
                    case "docdes":
                    case "custom1":
                    case "custom2":
                    case "custom3":
                        e.Cancel = !objMSTJobFactory.Validation_Detail(tagrdMSTJobDetOther.Name, tagrdMSTJobDetOther.ActiveRow, currentCell.Column.Key);
                        break;

                    case "othlinetype":
                        if (objMSTJobFactory.Validation_Detail(tagrdMSTJobDetOther.Name, tagrdMSTJobDetOther.ActiveRow, currentCell.Column.Key))
                        {
                            JobDetOtherCalculation();
                            GridCurrentRowLocking(tagrdMSTJobDetOther.Name);
                            return;
                        }
                        e.Cancel = true;
                        break;

                    case "othitmdes":
                        //we cannot popup the search form because for Job, the description must be free text w/o any ItmID
                        break;

                    case "othitmid":
                        if (GFunc.CompareString(currentCell.Column.Key, "OthItmDes") && !GFunc.IsNE(tagrdMSTJobDetOther.ActiveRow.Cells["OthItmID"].Value))
                            return;

                        if (RecSearchProcess(sender, e.Cell.Column.Key, false))
                        {
                            if (objMSTJobFactory.Validation_Detail(tagrdMSTJobDetOther.Name, tagrdMSTJobDetOther.ActiveRow, currentCell.Column.Key))
                            {
                                MSTItm objItm = MSTItm.GetParent((int)tagrdMSTJobDetOther.ActiveRow.Cells["OthItmKeySelect"].Value);
                                if (GFunc.IsNEZ(objItm.ItmKey))
                                {
                                    tagrdMSTJobDetOther.ActiveRow.Cells["OthItmKey"].Value = DBNull.Value;
                                    tagrdMSTJobDetOther.ActiveRow.Cells["OthItmType"].Value = 0;
                                    tagrdMSTJobDetOther.ActiveRow.Cells["OthUOMKey"].Value = DBNull.Value;
                                }
                                else
                                {
                                    tagrdMSTJobDetOther.ActiveRow.Cells["OthItmKey"].Value = objItm.ItmKey;
                                    tagrdMSTJobDetOther.ActiveRow.Cells["OthItmType"].Value = objItm.ItmType;
                                    tagrdMSTJobDetOther.ActiveRow.Cells["OthUOMKey"].Value = objItm.BUOMKey;
                                }
                                JobDetOtherCalculation();
                                GridCurrentRowLocking(tagrdMSTJobDetOther.Name);
                                return;
                            }
                        }
                        e.Cancel = true;
                        break;

                    case "othqty":
                        if (objMSTJobFactory.Validation_Detail(tagrdMSTJobDetOther.Name, tagrdMSTJobDetOther.ActiveRow, currentCell.Column.Key))
                        {
                            JobDetOtherCalculation();
                            return;
                        }
                        e.Cancel = true;
                        break;

                    case "othuomkey":
                        if (objMSTJobFactory.Validation_Detail(tagrdMSTJobDetOther.Name, tagrdMSTJobDetOther.ActiveRow, currentCell.Column.Key))
                        {
                            tagrdMSTJobDetOther.ActiveRow.Cells["OthConRate"].Value = DocComUtility.UOMConRate_Get(GFunc.NEInt(tagrdMSTJobDetOther.ActiveRow.Cells["OthItmKey"].Value, 0), GFunc.NEInt(tagrdMSTJobDetOther.ActiveRow.Cells["OthUOMKey"].Value, 0));
                            return;
                        }
                        e.Cancel = true;
                        break;

                    case "othpricef":
                    case "othpaidamtf":
                    case "doccurrrate":
                        if (objMSTJobFactory.Validation_Detail(tagrdMSTJobDetOther.Name, tagrdMSTJobDetOther.ActiveRow, currentCell.Column.Key))
                        {
                            JobDetOtherCalculation();
                            return;
                        }
                        break;

                    case "doccurrkey":
                        if (objMSTJobFactory.Validation_Detail(tagrdMSTJobDetOther.Name, tagrdMSTJobDetOther.ActiveRow, currentCell.Column.Key))
                        {
                            currentCell.Row.Cells["DocCurrRate"].Value = DocComUtility.CurrRate_Get((int)currentCell.Value, DateTime.Today, false);
                            JobDetOtherCalculation();
                            GridCurrentRowLocking(tagrdMSTJobDetOther.Name);
                            return;
                        }
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
        private void tagrdMSTJobDetOther_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            string msgID = string.Empty;
            try
            {              
                

                if (this.tagrdMSTJobDetOther.ActiveRow != null)
                {
                    
                    if (objMSTJobFactory.Validation_Detail(tagrdMSTJobDetOther.Name, tagrdMSTJobDetOther.ActiveRow, string.Empty) == false)
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
        private void tagrdMSTJobDetOther_AfterRowUpdate(object sender, RowEventArgs e)
        {
            try
            {
                objMSTJobFactory.IsDirty = true;
                objMSTJobFactory.ObjMSTJobDetOthers.AcceptChanges();
                CalculateTotalAmount();
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
        private void tagrdMSTJobDetOther_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            try
            {
                e.DisplayPromptMsg = false;
                if (tagrdMSTJobDetOther.Rows.Count > 0)
                {
                    if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) != GEnum.MsgBoxButton.Delete)
                    {
                        e.Cancel = true;
                        return;
                    }
                    //Move the cursor position of active row index to upper row
                    if (tagrdMSTJobDetEst.ActiveRow.Index > 0)
                        PreRowIndex = tagrdMSTJobDetEst.ActiveRow.Index - 1;
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
        }//Completed
        private void tagrdMSTJobDetOther_AfterRowsDeleted(object sender, EventArgs e)
        {
            try
            {
                objMSTJobFactory.IsDirty = true;
                objMSTJobFactory.ObjMSTJobDetOthers.AcceptChanges();
                CalculateTotalAmount();

                if (tagrdMSTJobDetOther.Rows.Count > 0)
                {
                    tagrdMSTJobDetOther.Rows[PreRowIndex].Selected = true;
                    tagrdMSTJobDetOther.Rows[PreRowIndex].Activate();
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
        
        
        //Attached Methods
        private void ErrorNotifier_Clear(object sender, BOLib.UINotifierEventArgs e)
        {
            this.errorProvider1.Clear();
            DocComUtility.ClearErrNotifier(this, e, errorProvider1);
        }
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

                //For Focus
                foreach (object key in e.PropertyMessage.Keys)
                {
                    Control co = this.Controls.Find(conNm, true)[0];
                    switch (conNm.ToLower())
                    {
                        case "jobclass":
                            tabDetailList.Tabs[0].Selected = true;
                            break;
                        case "jobsupervisor":
                            tabDetailList.Tabs[0].Selected = true;
                            break;
                        case "weightuomkey":
                            //tabDetList.Tabs["Measureme nt"].Selected = true;
                            break;
                        case "buomkey":
                            //tabDetList.Tabs["General"].Selected = true;
                            break;
                        default:
                            break;
                    }
                    co.Focus();
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
        }

        //Error
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
                            switch (grd.ActiveCell.Column.Key.ToString())
                            {
                                case "TransmitMode":
                                case "TransmitStatus":
                                case "EstItmType":
                                case "OthLineType":
                                    GlobalUI.ItemNotInList(grd.ActiveCell, null, 0);
                                    break;

                                default:
                                    GlobalUI.ItemNotInList(grd.ActiveCell, null, 1);
                                    break;
                            }
                        }
                    }
                }
                if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_NUMERIC)
                {
                    throw new TAException(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
                }

                if (e.ErrorCode == TAUtil.TAErrorCode.INTEGER_EXCEED_LIMIT)
                {
                    throw new TAException(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
                }
                if (e.ErrorCode == TAUtil.TAErrorCode.FORMULA_NOT_RECOGNIZE)
                {
                    throw new TAException("FORMULA NOT RECOGNIZE");
                }
                throw new TAException("Invalid value");
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

                if (this.ActiveControl.GetType() != typeof(TAUtil.TAGridEditor))
                {
                    l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl });
                }
                else
                {
                    string ActiveColKey = "";
                    if (((TAUtil.TAGridEditor)this.ActiveControl).ActiveCell != null)
                    {
                        ActiveColKey = GFunc.GridColumnKey_Get(this.ActiveControl);
                    }

                    l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, ActiveColKey });
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
                    SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { });
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

        private void tanuDefCostPercent_CustomUpdate(object sender, CancelEventArgs e)
        {

            decimal costPercent = 0M;
            if (Decimal.TryParse(tanuDefCostPercent.Text, out costPercent))
            {
                if (costPercent < 0)
                    return;

                if (tagrdMSTJobDetEst.Rows.Count > 0)
                {
                    if (MsgBox.Show("Do you want to update the prject cost for existing Rows as well?", GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.Yes)
                    {
                        PrjCostUpdate(costPercent);
                    }
                }
            }            
        }

        private void PrjCostUpdate(decimal costPercent)
        {
            foreach (UltraGridRow r in tagrdMSTJobDetEst.Rows)
            {
                int ItmKey = GFunc.NEInt(r.Cells["EstItmKey"].Value, 0);
                DataTable dt = objMSTJobFactory.GetItemInfo(ItmKey);
                if (dt!= null)
                if (dt.Rows.Count > 0)
                {
                    //tagrdMSTJobDetEst.ActiveRow.Cells["EstItmKey"].Value = ItmKey;
                    //tagrdMSTJobDetEst.ActiveRow.Cells["EstItmType"].Value = GFunc.NEInt(dt.Rows[0]["ItmType"], 0);
                    

                    if (GFunc.NEInt(dt.Rows[0]["ItmType"], 0) == (int)GEnum.ItemType.Stock)
                    {
                        r.Cells["ItmStock"].Value = GFunc.NEDec(dt.Rows[0]["AvailableQty"], 0);
                        r.Cells["EstCostH"].Value = GFunc.NEDec(dt.Rows[0]["CostLanded"], 0);
                        r.Cells["EstCostF"].Value = GFunc.NEDec(dt.Rows[0]["CostLanded"], 0);
                            r.Cells["EstAmtF"].Value = GFunc.NEDec(dt.Rows[0]["CostLanded"], 0) * GFunc.NEDec(r.Cells["EstQty"].Value, 0);
                            r.Cells["EstAmtH"].Value = r.Cells["EstAmtF"].Value;
                    }
                }

                decimal AmtF = GFunc.NEDec(r.Cells["EstAmtH"].Value, 0);
                r.Cells["PrjCostRate"].Value = costPercent;
                r.Cells["PrjCost"].Value = AmtF * (1 + costPercent / 100M);
                r.Update();
            }
            tagrdMSTJobDetEst.DisplayLayout.Bands[0].Columns["PrjCostRate"].DefaultCellValue = costPercent;
            tagrdMSTJobDetEst.Refresh();
        }
    }
}
