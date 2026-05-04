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

namespace WinUI
{
    public partial class frmMSTItmOpeningBal : Form
    {
        #region Local Variables
        private MSTItmLocOpenBalFactory objMSTItmLocOpenBalFactory = null;
        string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        private int RecordKey = 0;
        DataTable dtItem = null;

        //only use to show Read only data viewer for AuditLog 
        private bool IsOpenFromAuditLog = false;
        private DataTable _dtHeader = null;
        private DataSet _dsDetail = null;
        #endregion

        //Initialize
        public frmMSTItmOpeningBal()
        {
            InitializeComponent();
        }//Completed
        public frmMSTItmOpeningBal(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            //Open from AuditLog as Read only data viewer
            InitializeComponent();
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
        }//Completed

        //Form Events
        private void frmMSTItmOpeningBal_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                // Initialize
                this.objMSTItmLocOpenBalFactory = new BOLib.MSTItmLocOpenBalFactory(BOLib.GEnum.InstanceMode.Normal);
                if (objMSTItmLocOpenBalFactory.GUID <= 0)
                {
                    formClose = true;
                    return;
                }

                if (this.IsOpenFromAuditLog)
                {
                    if (objMSTItmLocOpenBalFactory.SetReadOnlyData(_dsDetail) == false)
                    {
                        formClose = true;
                        return;
                    }

                    ItemID.SetValueTrigger(_dtHeader.Rows[0]["ItmID"].ToString(), false);
                    ItemDes.SetValueTrigger(_dtHeader.Rows[0]["ItmDes"].ToString(), false);
                    ItmType.SetValueTrigger((int)_dtHeader.Rows[0]["ItmType"], false);
                    BUOMID.SetValueTrigger(_dtHeader.Rows[0]["BUOMID"].ToString(), false);
                    CatID1.SetValueTrigger(_dtHeader.Rows[0]["CatID1"].ToString(), false);
                    CatID2.SetValueTrigger(_dtHeader.Rows[0]["CatID2"].ToString(), false);
                    CatID3.SetValueTrigger(_dtHeader.Rows[0]["CatID3"].ToString(), false);
                    CatID4.SetValueTrigger(_dtHeader.Rows[0]["CatID4"].ToString(), false);
                    CatID5.SetValueTrigger(_dtHeader.Rows[0]["CatID5"].ToString(), false);
                    TotalQty.SetValueTrigger(_dtHeader.Rows[0]["OpenBalQty"].ToString(), false);
                    UnitCost.SetValueTrigger(_dtHeader.Rows[0]["OpenBalCost"].ToString(), false);
                    TotalAmount.SetValueTrigger(_dtHeader.Rows[0]["OpenBalAmtH"].ToString(), false);

                    RecordKey = (int)_dtHeader.Rows[0]["ItmKey"];

                    Refresh_GridDet(true);
                    GlobalUI.FormEnable_Set(this, false);
                }
                else
                {
                    this.objMSTItmLocOpenBalFactory.New();
                    RefreshDataAndLayout(true);
                }

                //Setup FORM control/grid format, menu, listID
                GlobalUI.FormGrids_Set(this, (int)objMSTItmLocOpenBalFactory.ConstantCodeKey, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objMSTItmLocOpenBalFactory.ConstantCodeKey);
                GlobalUI.Combos_Fill(this, (int)objMSTItmLocOpenBalFactory.ConstantCodeKey);

                //Make a copy of the Item List for use in Next/Previous record function              
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItemID");
                GFunc.GetPopupListDataWithParams(listSettingID, ref dtItem, new string[] { "" }, false);

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
        private void frmMSTItmOpeningBal_Shown(object sender, EventArgs e)
        {
            if (formClose)
                this.Close();
            else
                this.ItemID.Focus();
        }//Completed
        private void frmMSTItmOpeningBal_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool runProcess = false;
            this.formClose = true;

            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }
            if (this.formClose && objMSTItmLocOpenBalFactory == null)
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
                if ((bool)this.objMSTItmLocOpenBalFactory.Dispose() == false)
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
                    this.objMSTItmLocOpenBalFactory.Dispose();
            }
        }//Completed
        private void frmMSTItmOpeningBal_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                    GlobalUI.Combos_Fill(this, (int)objMSTItmLocOpenBalFactory.ConstantCodeKey);

                //Set Focus Next Control
                if (this.ActiveControl.Parent != this.UnitCost)
                {
                    GlobalUI.SelectNextControl(this, e);
                }
                else
                {
                    this.UnitCost_KeyDown(sender, e);
                }
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
        private void tsbSave_Click(object sender, EventArgs e)
        {
            try
            {
                Save_Process();
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
            formClose = true;
            this.Close();
        }//Completed
        private void tsbNext_Click(object sender, EventArgs e)
        {
            try
            {
                string id = ItemID.Text;

                GFunc.GetIndexfromDT("ID", ItemID.Text, true, dtItem, out id);
                if (ItemID.Text != id)
                {
                    ItemID.Text = id;
                    CancelEventArgs ea = new CancelEventArgs();
                    ItemID_CustomUpdate(null, ea);
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
            string id = ItemID.Text;
            try
            {
                GFunc.GetIndexfromDT("ID", ItemID.Text, false, dtItem, out id);
                if (ItemID.Text != id)
                {
                    ItemID.Text = id;
                    CancelEventArgs ea = new CancelEventArgs();
                    ItemID_CustomUpdate(null, ea);
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
        private void RefreshDataAndLayout(bool formload)
        {
            try
            {
                Refresh_Header();
                Refresh_GridDet(formload);
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
        private void Refresh_All(bool formload)
        {
            try
            {
                Refresh_Header();
                Refresh_GridDet(formload);
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
                MSTItm objItm = MSTItm.Get(RecordKey);
                if (GFunc.IsNEZ(objItm.ItmKey))
                {
                    ItemID.SetValueTrigger(string.Empty, false);
                    ItemDes.SetValueTrigger(string.Empty, false);
                    ItmType.SetValueTrigger(0, false);
                    BUOMID.SetValueTrigger(string.Empty, false);
                    CatID1.SetValueTrigger(string.Empty, false);
                    CatID2.SetValueTrigger(string.Empty, false);
                    CatID3.SetValueTrigger(string.Empty, false);
                    CatID4.SetValueTrigger(string.Empty, false);
                    CatID5.SetValueTrigger(string.Empty, false);
                    UnitCost.SetValueTrigger("0", false);
                    TotalQty.SetValueTrigger("0", false);
                    TotalAmount.SetValueTrigger("0.00", false);
                }
                else
                {
                    ItemID.SetValueTrigger(objItm.ItmID, false);
                    ItemDes.SetValueTrigger(objItm.ItmDes, false);
                    ItmType.SetValueTrigger(objItm.ItmType, false);
                    BUOMID.SetValueTrigger(objItm.BUOMID, false);
                    CatID1.SetValueTrigger(objItm.CatID1, false);
                    CatID2.SetValueTrigger(objItm.CatID2, false);
                    CatID3.SetValueTrigger(objItm.CatID3, false);
                    CatID4.SetValueTrigger(objItm.CatID4, false);
                    CatID5.SetValueTrigger(objItm.CatID5, false);
                    UnitCost.SetValueTrigger(GFunc.NEDec(objItm.OpenBalCost, 0).ToString(), false);
                    TotalQty.SetValueTrigger(GFunc.NEDec(objItm.OpenBalQty, 0).ToString(), false);
                    TotalAmount.SetValueTrigger(GFunc.NEDec(objItm.OpenBalAmtH, 0).ToString(), false);
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
        private void Refresh_GridDet(bool formload)
        {
            try
            {
                if (formload)
                {
                    tagrdOpeningBalBatch.DataSource = objMSTItmLocOpenBalFactory.ObjMSTItmLocOpenBals;
                    tagrdOpeningBalLoc.DataSource = objMSTItmLocOpenBalFactory.ObjMSTItmLocOpenBals;
                }

                UltraGrid grd = Get_ActiveGrid();
                grd.DataSource = objMSTItmLocOpenBalFactory.ObjMSTItmLocOpenBals;
                grd.DataBind();
                //grd.Rows.Refresh(RefreshRow.ReloadData);

                //Set default values
                grd.DisplayLayout.Bands[0].Columns["ItmKey"].DefaultCellValue = RecordKey;
                grd.DisplayLayout.Bands[0].Columns["BatchKey"].DefaultCellValue = 0;
                grd.DisplayLayout.Bands[0].Columns["Qty"].DefaultCellValue = 0;
               // grd.DisplayLayout.Bands[0].Columns["BatchCost"].DefaultCellValue = GFunc.NEDec(this.UnitCost.Value, 0);
                grd.DisplayLayout.Bands[0].Columns["DatePurchase"].DefaultCellValue = DateTime.Today.Date;
                grd.DisplayLayout.Bands[0].Columns["BatchMfgDate"].DefaultCellValue = DateTime.Today.Date;
                grd.DisplayLayout.Bands[0].Columns["BatchExpDate"].DefaultCellValue = DateTime.Today.Date;
                grd.DisplayLayout.Bands[0].Columns["Amount"].DefaultCellValue = 0;
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
                bool EnableMode = !this.objMSTItmLocOpenBalFactory.IsReadOnly;
                this.tslReadOnly.Text = !EnableMode ? "Read Only" : string.Empty;

                this.tsbSave.Enabled = EnableMode;
                this.chkAutoSave.Enabled = EnableMode;

                if (EnableMode)
                {
                    //To indicate to user that no entry is available until a valid record is selected
                    if (GFunc.IsNEZ(RecordKey))
                    {
                        this.tsbSave.Enabled = false;
                        this.tagrdOpeningBalBatch.Enabled = false;
                        this.tagrdOpeningBalLoc.Enabled = false;
                    }
                    else
                    {
                        this.tagrdOpeningBalBatch.Enabled = true;
                        this.tagrdOpeningBalLoc.Enabled = true;
                    }

                    #region Grid Activation
                    this.tagrdOpeningBalLoc.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
                    this.tagrdOpeningBalLoc.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
                    this.tagrdOpeningBalLoc.DisplayLayout.Override.AllowAddNew = AllowAddNew.TemplateOnBottom;
                    this.tagrdOpeningBalBatch.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
                    this.tagrdOpeningBalBatch.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
                    this.tagrdOpeningBalBatch.DisplayLayout.Override.AllowAddNew = AllowAddNew.TemplateOnBottom;

                    foreach (UltraGridColumn gcol in tagrdOpeningBalLoc.DisplayLayout.Bands[0].Columns)
                    {
                        switch (gcol.Key.ToLower())
                        {
                            case "lockey":
                            case "datepurchase":
                            case "qty":
                            case "batchcost":
                            case "batchid":
                                gcol.CellActivation = Activation.AllowEdit;
                                break;

                            default:
                                gcol.CellActivation = Activation.ActivateOnly;
                                break;
                        }
                    }
                    foreach (UltraGridColumn gcol in tagrdOpeningBalBatch.DisplayLayout.Bands[0].Columns)
                    {
                        switch (gcol.Key)
                        {
                            case "LocKey":
                            case "DatePurchase":
                            case "Qty":
                            case "BatchID":
                            case "BatchExpDate":
                            case "BatchMfgDate":
                                gcol.CellActivation = Activation.AllowEdit;
                                break;

                            case "BatchCost":                            
                                gcol.CellActivation = Activation.AllowEdit;
                                //switch ((int)ItmType.Value)
                                //{
                                //    case (int)GEnum.ItemType.Finished_GDB:
                                //    case (int)GEnum.ItemType.StockB:
                                //        gcol.CellActivation = Activation.AllowEdit;
                                //        break;

                                //    default:
                                //        gcol.CellActivation = Activation.ActivateOnly;
                                //        break;
                                //}
                                break;

                            default:
                                gcol.CellActivation = Activation.ActivateOnly;
                                break;
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Grid Disabled
                    this.tagrdOpeningBalLoc.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
                    this.tagrdOpeningBalLoc.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                    this.tagrdOpeningBalLoc.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                    this.tagrdOpeningBalBatch.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
                    this.tagrdOpeningBalBatch.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                    this.tagrdOpeningBalBatch.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                    #endregion
                }

                #region Grid and UnitCost Visibility
                switch (GFunc.NEInt(ItmType.Value, 0))
                {
                    case (int)GEnum.ItemType.Finished_GDB:
                    case (int)GEnum.ItemType.StockB:
                        tagrdOpeningBalBatch.Visible = true;
                        tagrdOpeningBalBatch.TabStop = true;
                        tagrdOpeningBalLoc.Visible = false;
                        tagrdOpeningBalLoc.TabStop = false;
                        this.UnitCost.Enabled = false;
                        this.UnitCost.Visible = false;
                        break;

                    case (int)GEnum.ItemType.Serial_Finished_GDB:
                    case (int)GEnum.ItemType.Serial_StockB:
                        tagrdOpeningBalBatch.Visible = true;
                        tagrdOpeningBalBatch.TabStop = true;
                        tagrdOpeningBalLoc.Visible = false;
                        tagrdOpeningBalLoc.TabStop = false;
                        this.UnitCost.Enabled = EnableMode;
                        this.UnitCost.Visible = true;
                        break;

                    default:
                        tagrdOpeningBalBatch.Visible = false;
                        tagrdOpeningBalBatch.TabStop = false;
                        tagrdOpeningBalLoc.Visible = true;
                        tagrdOpeningBalBatch.TabStop = true;
                        this.UnitCost.Enabled = EnableMode;
                        this.UnitCost.Visible = true;
                        break;
                }
                if (this.UnitCost.Enabled)
                {
                    this.UnitCost.Focus();
                    //this.UnitCost.SelectionStart = 0;
                    //this.UnitCost.SelectionLength = this.UnitCost.TextLength;
                    this.UnitCost.SelectAll();
                }
                else
                {
                    SelectGridCell();
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

        //Functions
        private UltraGrid Get_ActiveGrid()
        {
            switch (GFunc.NEInt(ItmType.Value, 0))
            {
                case (int)GEnum.ItemType.Finished_GDB:
                case (int)GEnum.ItemType.Serial_Finished_GDB:
                case (int)GEnum.ItemType.Serial_StockB:
                case (int)GEnum.ItemType.StockB:
                    return tagrdOpeningBalBatch;

                default:
                    return tagrdOpeningBalLoc;
            }
        }//Completed
        private bool SaveChanges()
        {
            try
            {
                if (form_CanValidate() == false)
                    return false;

                if (objMSTItmLocOpenBalFactory.IsDirty && (chkAutoSave.Checked))
                    return this.Save_Process();

                if (objMSTItmLocOpenBalFactory.IsDirty)
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

                if (Calculation() == false)
                    return false;

                if (GFunc.IsNEZ(RecordKey) || GFunc.IsNEZ(ItmType.Value))
                    return false;

                //Perform Saving
                if (this.objMSTItmLocOpenBalFactory.Save(RecordKey, GFunc.NEInt(ItmType.Value, 0), GFunc.NEDec(UnitCost.Value, 0)))
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
                this.RefreshDataAndLayout(false);
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

                if (SECPermUtility.Edit(objMSTItmLocOpenBalFactory.PermID, false))
                {
                    if (objMSTItmLocOpenBalFactory.GetEdit(key) == false)
                    {
                        if (SysOptionUtility.GetBool("WarnOpenRecordAsReadOnly"))
                        {
                            btnSelect = MsgBox.Show("Try to open record as read only",
                                                  GEnum.MsgBoxIcon.Question,
                                                  GEnum.MsgBoxButton.Yes,
                                                  GEnum.MsgBoxButton.No);

                            if (btnSelect == GEnum.MsgBoxButton.Yes)
                            {
                                if (objMSTItmLocOpenBalFactory.GetReadOnly(key) == false)
                                    return false;
                            }
                            else
                                return false;
                        }
                    }
                }
                else
                {
                    if (objMSTItmLocOpenBalFactory.GetReadOnly(key) == false)
                        return false;
                }

                //set recordKey to ItmKey that is open
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
                RefreshDataAndLayout(false);
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
                Get_ActiveGrid().PerformAction(UltraGridAction.ExitEditMode);
                Get_ActiveGrid().UpdateData();

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

            UltraGrid grd = Get_ActiveGrid();

            if (grd.ActiveRow != null)
            {
                if (grd.ActiveRow.DataChanged && !grd.ActiveRow.IsUnmodifiedTemplateAddRow)
                {
                    if (undoChangesInGrid)
                    {
                        //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                        grd.PerformAction(UltraGridAction.UndoCell);
                        grd.PerformAction(UltraGridAction.UndoRow);
                    }
                    return true;
                }
            }
            return false;
        }//Completed
        private bool Calculation()
        {
            if (frmMain.gfrmMain.ActiveMdiChild != this)
                return false;

            try
            {
                UltraGrid grd = Get_ActiveGrid();
                decimal Totalqty = 0M;
                decimal cost = 0M; //GFunc.NEDec(this.UnitCost.Value, 0);
                decimal Amt = 0M;
                decimal TotalCost = 0M;

                if (grd.Rows.Count > 0)
                {
                    for (int i = 0; i < grd.Rows.Count; i++)
                    {
                        Amt = GFunc.RndC(GFunc.NEDec(grd.Rows[i].Cells["BatchCost"].Value, 0) * GFunc.NEDec(grd.Rows[i].Cells["Qty"].Value, 0), GVar.RndDecs.Amtpt);
                        //switch (GFunc.NEInt(ItmType.Value, 0))
                        //{
                        //    case (int)GEnum.ItemType.StockB:
                        //    case (int)GEnum.ItemType.Finished_GDB:
                        //        Amt = GFunc.RndC(GFunc.NEDec(grd.Rows[i].Cells["BatchCost"].Value, 0) * GFunc.NEDec(grd.Rows[i].Cells["Qty"].Value, 0), GVar.RndDecs.Amtpt);
                        //        break;

                        //    default:
                        //        // grd.Rows[i].Cells["BatchCost"].Value = cost;
                        //        //Amt = GFunc.RndC(cost * GFunc.NEDec(grd.Rows[i].Cells["Qty"].Value, 0), GVar.RndDecs.Amtpt);
                        //        Amt = GFunc.RndC(GFunc.NEDec(grd.Rows[i].Cells["BatchCost"].Value, 0) * GFunc.NEDec(grd.Rows[i].Cells["Qty"].Value, 0), GVar.RndDecs.Amtpt);
                        //        break;
                        //}
                        Totalqty = Totalqty + (decimal)grd.Rows[i].Cells["Qty"].Value;
                        grd.Rows[i].Cells["Amount"].Value = Amt;
                        TotalCost = TotalCost + Amt;
                    }
                }
                grd.UpdateData();
                objMSTItmLocOpenBalFactory.ObjMSTItmLocOpenBals.AcceptChanges();
                TotalQty.SetValueTrigger(Totalqty.ToString(), false);
                this.TotalAmount.SetValueTrigger(TotalCost.ToString(), false);
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

        //Control Events
        private void ItmIDSelected(int key)
        {
            try
            {
                if (this.OpenRecord(key) == false)
                    return;
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
        private void ItemID_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = string.Empty;

                listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItemID");

                if (key == 0)
                {
                    if (DocHDRUtil.EditorButton_Popup((int)objMSTItmLocOpenBalFactory.ConstantCodeKey, ItemID.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref key, ref id, ref des))
                        ItmIDSelected(key);
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
        private void ItemID_CustomUpdate(object sender, CancelEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;

            try
            {
                //Clear control value when user input null values
                if (GFunc.IsNE(ItemID.Text))
                    ItmIDSelected(0);
                else
                {
                    //Try to match record in server
                    string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItemID");
                    key = GFunc.ItmRecord_GetKey(GEnum.RecAccessType.ItemID, listSettingID, ItemID.Text, 0, ref id, ref des, true);
                    if (key == 0)
                    {
                        if (DocHDRUtil.EditorButton_Popup((int)objMSTItmLocOpenBalFactory.ConstantCodeKey, ItemID.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref key, ref id, ref des))
                            ItmIDSelected(key);
                        else
                            e.Cancel = true;
                    }
                    else
                        ItmIDSelected(key);

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
        private void ItemDes_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = string.Empty;

                listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItemDes");
                if (DocHDRUtil.EditorButton_Popup((int)objMSTItmLocOpenBalFactory.ConstantCodeKey, ItemDes.Text, listSettingID, (int)GEnum.PopupType.ItmDes, ref key, ref id, ref des))
                    ItmIDSelected(key);
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
        private void ItemDes_CustomUpdate(object sender, CancelEventArgs e)
        {
            int key = 0;
            string id = string.Empty;
            string des = string.Empty;

            try
            {
                //Clear control value when user input null values
                if (GFunc.IsNE(ItemDes.Text))
                    ItmIDSelected(0);
                else
                {
                    //Try to match record in server
                    string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItemDes");
                    key = GFunc.ItmRecord_GetKey(GEnum.RecAccessType.ItemDes, listSettingID, ItemDes.Text, 0, ref id, ref des, true);
                    if (key == 0)
                    {
                        if (DocHDRUtil.EditorButton_Popup((int)objMSTItmLocOpenBalFactory.ConstantCodeKey, ItemDes.Text, listSettingID, (int)GEnum.PopupType.ItmDes, ref key, ref id, ref des))
                            ItmIDSelected(key);
                        else
                            e.Cancel = true;
                    }
                    else
                        ItmIDSelected(key);

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
        private void UnitCost_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                decimal cost = GFunc.RndC(GFunc.NEDec(this.UnitCost.Value, 0), GVar.RndDecs.Prcpt);
                this.UnitCost.SetValueTrigger(cost, false);
               // Get_ActiveGrid().DisplayLayout.Bands[0].Columns["BatchCost"].DefaultCellValue = cost;
                Calculation();
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
        private void tabDetList_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Down:
                    Get_ActiveGrid().Focus();
                    UltraGridColumn FirstVisCol = Get_ActiveGrid().DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                    if (FirstVisCol != null)
                    {
                        Get_ActiveGrid().ActiveCell = Get_ActiveGrid().Rows.TemplateAddRow.Cells[FirstVisCol.Key];
                        Get_ActiveGrid().PerformAction(UltraGridAction.EnterEditMode, false, false);
                    }
                    break;
                case Keys.Up:
                    TotalAmount.Focus();
                    break;
            }
        }

        //Grid Events
        private void tagrdOpeningBal_CustomDataError(object sender, TAUtil.TAErrorEventArgs e)
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
                                case "lockey":
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
                    if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_NUMERIC)
                    {
                        throw new TAException(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
                    }
                    else if (e.ErrorCode == TAUtil.TAErrorCode.INTEGER_EXCEED_LIMIT)
                    {

                        throw new TAException(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
                    }
                    else if (e.ErrorCode == TAUtil.TAErrorCode.DECIMAL_EXCEED_LIMIT)
                    {
                        MsgBox.Show(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");

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
        private void tagrdOpeningBal_CustomCellUpdate(object sender, Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs e)
        {

            try
            {
                UltraGrid grd = Get_ActiveGrid();
                UltraGridRow currentRow = this.Get_ActiveGrid().ActiveRow;
                int BatchKey = 0;
                string BatchID;
                decimal BatchCost = 0;
                decimal Qty = 0;

                BatchKey = GFunc.NEInt(currentRow.Cells["BatchKey"].Value, 0);
                BatchID = currentRow.Cells["BatchID"].Value.ToString();

                switch (grd.ActiveCell.Column.Key)
                {
                    #region BatchID
                    case "BatchID":
                        //If BatchKey = 0 -- if this BatchID already exist in another row with a BatchKey > 0 we need to update this current row infor 
                        //                   with the matched row infor
                        if (grd.Rows.Count > 0 && BatchKey == 0 && GFunc.IsNE(BatchID) == false)
                        {
                            for (int i = 0; i < grd.Rows.Count; i++)
                            {
                                if (GFunc.NEStr(grd.Rows[i].Cells["BatchID"].Value, string.Empty) == BatchID)
                                {
                                    currentRow.Cells["BatchKey"].Value = grd.Rows[i].Cells["BatchKey"].Value;
                                    currentRow.Cells["DatePurchase"].Value = grd.Rows[i].Cells["DatePurchase"].Value;
                                    currentRow.Cells["BatchExpDate"].Value = grd.Rows[i].Cells["BatchExpDate"].Value;
                                    currentRow.Cells["BatchMfgDate"].Value = grd.Rows[i].Cells["BatchMfgDate"].Value;
                                    currentRow.Cells["BatchCost"].Value = grd.Rows[i].Cells["BatchCost"].Value;
                                    currentRow.Cells["Amount"].Value = GFunc.RndC(GFunc.NEDec(currentRow.Cells["BatchCost"].Value, 0) * GFunc.NEDec(grd.Rows[i].Cells["Qty"].Value, 0), GVar.RndDecs.Amtpt);
                                    break;
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Qty
                    case "Qty":
                        Qty = GFunc.RndC(currentRow.Cells["Qty"].Value, GVar.RndDecs.Qtypt);
                        currentRow.Cells["Qty"].Value = Qty;

                        switch (GFunc.NEInt(ItmType.Value, 0))
                        {
                            case (int)GEnum.ItemType.StockB:
                            case (int)GEnum.ItemType.Finished_GDB:
                            case (int)GEnum.ItemType.Serial_StockB:
                            case (int)GEnum.ItemType.Serial_Finished_GDB:
                                BatchCost = GFunc.NEDec(currentRow.Cells["BatchCost"].Value, 0);
                                currentRow.Cells["Amount"].Value = GFunc.RndC(BatchCost * Qty, GVar.RndDecs.Amtpt);
                                break;
                            default:
                                //BatchCost = GFunc.NEDec(UnitCost.Text, 0);
                                BatchCost = GFunc.NEDec(currentRow.Cells["BatchCost"].Value, 0);
                                currentRow.Cells["Amount"].Value = GFunc.RndC(BatchCost * Qty, GVar.RndDecs.Amtpt);
                                break;
                        }
                        break;
                    #endregion

                    #region BatchCost
                    case "BatchCost":
                        BatchCost = GFunc.RndC(currentRow.Cells["BatchCost"].Value, GVar.RndDecs.Prcpt);
                        currentRow.Cells["BatchCost"].Value = BatchCost;
                        Qty = GFunc.NEDec(currentRow.Cells["Qty"].Value, 0);

                        switch (GFunc.NEInt(ItmType.Value, 0))
                        {
                            case (int)GEnum.ItemType.StockB:
                            case (int)GEnum.ItemType.Finished_GDB:
                                currentRow.Cells["Amount"].Value = GFunc.RndC(BatchCost * Qty, GVar.RndDecs.Amtpt);
                                break;
                        }
                        break;
                    #endregion
                }
                objMSTItmLocOpenBalFactory.IsDirty = true;
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
        private void tagrdOpeningBal_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            try
            {

                if (Get_ActiveGrid().ActiveRow != null)
                    e.Cancel = !objMSTItmLocOpenBalFactory.Validation_Detail(Get_ActiveGrid().ActiveRow, string.Empty, RecordKey, (int)ItmType.Value);


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
        private void tagrdOpeningBal_AfterRowUpdate(object sender, RowEventArgs e)
        {

            try
            {
                UltraGrid grd = Get_ActiveGrid();
                UltraGridRow currentRow = this.Get_ActiveGrid().ActiveRow;
                int BatchKey = 0;
                string BatchID;

                BatchKey = GFunc.NEInt(currentRow.Cells["BatchKey"].Value, 0);
                BatchID = currentRow.Cells["BatchID"].Value.ToString();


                if (grd.Rows.Count > 0 && BatchKey > 0)
                {
                    //to assign the value to similar batch (because all row with the same batchkey must have the same value in cost,datepurchase, MfgDate, ExpDate
                    for (int i = 0; i < grd.Rows.Count; i++)
                    {
                        if (BatchKey > 0)
                        {
                            if ((int)grd.Rows[i].Cells["BatchKey"].Value == BatchKey && grd.Rows[i].Index != currentRow.Index)
                            {
                                grd.Rows[i].Cells["BatchID"].Value = currentRow.Cells["BatchID"].Value;
                                grd.Rows[i].Cells["DatePurchase"].Value = currentRow.Cells["DatePurchase"].Value;
                                grd.Rows[i].Cells["BatchExpDate"].Value = currentRow.Cells["BatchExpDate"].Value;
                                grd.Rows[i].Cells["BatchMfgDate"].Value = currentRow.Cells["BatchMfgDate"].Value;
                                grd.Rows[i].Cells["BatchCost"].Value = currentRow.Cells["BatchCost"].Value;
                                grd.Rows[i].Cells["Amount"].Value = GFunc.RndC(GFunc.NEDec(grd.Rows[i].Cells["BatchCost"].Value, 0) * GFunc.NEDec(grd.Rows[i].Cells["Qty"].Value, 0), GVar.RndDecs.Amtpt);
                            }
                        }
                        else
                        {
                            switch (GFunc.NEInt(ItmType.Value, 0))
                            {
                                case (int)GEnum.ItemType.StockB:
                                case (int)GEnum.ItemType.Finished_GDB:
                                case (int)GEnum.ItemType.Serial_StockB:
                                case (int)GEnum.ItemType.Serial_Finished_GDB:

                                    if (GFunc.CompareString((string)grd.Rows[i].Cells["BatchID"].Value, BatchID) && (int)grd.Rows[i].Cells["BatchKey"].Value == 0 && grd.Rows[i].Index != currentRow.Index)
                                    {
                                        grd.Rows[i].Cells["BatchID"].Value = currentRow.Cells["BatchID"].Value;
                                        grd.Rows[i].Cells["DatePurchase"].Value = currentRow.Cells["DatePurchase"].Value;
                                        grd.Rows[i].Cells["BatchExpDate"].Value = currentRow.Cells["BatchExpDate"].Value;
                                        grd.Rows[i].Cells["BatchMfgDate"].Value = currentRow.Cells["BatchMfgDate"].Value;
                                        grd.Rows[i].Cells["BatchCost"].Value = currentRow.Cells["BatchCost"].Value;
                                        grd.Rows[i].Cells["Amount"].Value = GFunc.RndC(GFunc.NEDec(grd.Rows[i].Cells["BatchCost"].Value, 0) * GFunc.NEDec(grd.Rows[i].Cells["Qty"].Value, 0), GVar.RndDecs.Amtpt);
                                    }
                                    break;
                            }
                        }
                    }
                }
                Calculation();
                objMSTItmLocOpenBalFactory.IsDirty = true;
                ((DataRowView)(e.Row.ListObject)).Row.RowError = "";                
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
        private void tagrdOpeningBal_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            e.DisplayPromptMsg = false;

            try
            {

                if (Get_ActiveGrid().Rows.Count > 0)
                {
                    UltraGridRow currentRow = this.Get_ActiveGrid().ActiveRow;

                    if (currentRow.IsAddRow == true)
                    {
                        if (MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know) != GEnum.MsgBoxButton.Delete)
                        {
                            e.Cancel = false;
                            return;
                        }
                        return;
                    }
                    else
                    {
                        switch (GFunc.NEInt(ItmType.Value, 0))
                        {
                            case (int)GEnum.ItemType.StockB:
                            case (int)GEnum.ItemType.Serial_StockB:
                            case (int)GEnum.ItemType.Finished_GDB:
                            case (int)GEnum.ItemType.Serial_Finished_GDB:
                                int BatchKey = GFunc.NEInt(currentRow.Cells["BatchKey"].Value, 0);
                                string BatchID = GFunc.NEStr(currentRow.Cells["BatchID"].Value, string.Empty);
                                if (GFunc.IsNEZ(BatchKey) == false)
                                {
                                    if (objMSTItmLocOpenBalFactory.HasDependentBatch(BatchKey, GFunc.NEInt(ItmType.Value, 0)))
                                        e.Cancel = true;
                                    return;
                                }
                                break;
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
        private void tagrdOpeningBal_AfterRowsDeleted(object sender, EventArgs e)
        {
            try
            {
                objMSTItmLocOpenBalFactory.ObjMSTItmLocOpenBals.AcceptChanges();
                Calculation();
                objMSTItmLocOpenBalFactory.IsDirty = true;
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

        private void SelectGridCell()
        {

            UltraGrid grd = Get_ActiveGrid();
            string firstColKey = this.tagrdOpeningBalBatch.DisplayLayout.Bands[0].Columns.OfType<UltraGridColumn>().ToList()
                            .Find(c => c.Header.VisiblePosition == 0).Key;
            if (grd.Rows.Count > 0)
            {
                grd.Rows[0].Cells[firstColKey].Selected = true;
                grd.Rows[0].Cells[firstColKey].Activate();
                grd.PerformAction(UltraGridAction.FirstCellInGrid);
                grd.PerformAction(UltraGridAction.EnterEditMode);

               // grd.Rows[0].Cells[firstColKey].SelectAll();
            }
        }
        private void UnitCost_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                
               if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab || e.KeyCode == Keys.Down)
                {
                    SelectGridCell();

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

    }
}
