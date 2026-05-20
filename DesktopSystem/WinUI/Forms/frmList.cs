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
using TAUtil;


namespace WinUI
{
    public partial class frmList : Form
    {
        #region Local Variables
        private int _CodeKey = 0;
        string ContextMenuSetting = string.Empty;
        private BOLib.ListFactory objListFtry = null;
        private string msgID = string.Empty;
        private bool formClose = false;
        string ListSettingID = string.Empty;

        //Event to be assigned by the caller detail form
        public GVar.RecordSelectedEvent RecordSelectedEvent = null;
        public GVar.ListUpdateEvent ListUpdateEvent = null;

        public GVar.ListEvent_OpenRecord ListEvent_OpenRecord = null;
        public GVar.ListEvent_CloseFORM ListEvent_CloseFORM = null;

        //Variables for using when the user find text
        private frmSearchInfo searchForm = null;
        private UltraGridColumn searchCol = null;
        private GlobalUI.SearchInfo searchInfo = null;

        string[] colHeaders = null;
        string[] colWidths = null;
        string[] colFormats = null;
        string[] newcolformat = null;

        //SYSListSetting objListSetting = new SYSListSetting();
        //SYSFormSettingID objFormSettingID = new SYSFormSettingID();//MTS commented this, pls search the words "MTS" in this file to know the reason       

        public int CodeKey
        {
            get
            {
                return Convert.ToInt32(objListFtry.CodeKey);
            }
        }
        #endregion

        // Initialize
        public frmList()
        {
            InitializeComponent();
        }
        public frmList(GEnum.SystemCode codeKey, string permID)
        {
            InitializeComponent();

            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;
            try
            {
                // Call Initialization
                this.objListFtry = new BOLib.ListFactory(out msgID, codeKey, permID);
                _CodeKey = (int)codeKey;
                switch (codeKey)
                {
                    case GEnum.SystemCode.Alerts:
                        this.Text = "Alert List";
                        break;

                    case GEnum.SystemCode.Customer:
                        this.Text = "Customer List";
                        break;

                    case GEnum.SystemCode.Vendor:
                        this.Text = "Vendor List";
                        break;

                    case GEnum.SystemCode.Inventory:
                        this.Text = "Inventory Item List";
                        break;

                    case GEnum.SystemCode.Job:
                        this.Text = "Job List";                        
                        break;

                    case GEnum.SystemCode.Account:
                        this.Text = "Chart of Account List";
                        break;

                    case GEnum.SystemCode.Price_List:
                        this.Text = "Price List";
                        break;

                    case GEnum.SystemCode.Ship_Name:
                        this.Text = "Ship Name List";
                        break;
                    case GEnum.SystemCode.To_Do:
                        this.Text = "To Do List";
                        break;
                }
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
                    Error(tex, true); // Customer Msg   
                }
            }
            catch (Exception ex)
            {
                Error(ex, true); // System Msg   
            }
            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }

        // Form Events        
        private void frmList_Load(object sender, EventArgs e)
        {
            try
            {             

                if (this.formClose == false)
                {
                    //Gloal Context Menu Set                             

                    SetFormSetting();
                   
                    GlobalUI.FormGrids_Set(this, (int)objListFtry.CodeKey, out ContextMenuSetting);
                    //GlobalUI.cmnuGlobal_Set(this);
                    ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(_CodeKey, this.Name);
                    tagrdList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
                    tagrdList.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
                    ListSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "0GF", tagrdList.Name);
                    tagrdList.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True;
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
        private void frmList_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // Call Dispose
                bool isOk = this.objListFtry.Dispose(out this.msgID);

                if (this.ListEvent_CloseFORM != null)
                    this.ListEvent_CloseFORM.Invoke();
            }
            catch (TAException tex)
            {
                Error(tex, true); // Custom Msg   
            }
            catch (Exception ex)
            {
                Error(ex, true); // System Msg   
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void frmList_Shown(object sender, EventArgs e)
        {
            // Check Form Close State is True ...
            if (formClose)
            {
                this.Close();
            }
        }
        private void frmList_Activated(object sender, EventArgs e)
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (!GFunc.IsNE(tagrdList.Tag))
                {
                    int key = GFunc.NEInt(tagrdList.Tag, 0);
                    if (!GFunc.IsNE(tagrdList.Rows))
                    {
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in tagrdList.Rows)
                        {
                            if (GFunc.NEInt(row.Cells["Key"].Value, 0) == key)
                            {
                                tagrdList.ActiveRow = row;
                                tagrdList.ActiveRow.Selected = true;
                                break;
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
        }

        // Grid Event
        private void tagrdList_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            if (tagrdList.ActiveRow != null)
                tsbEdit_Click(sender, e);
        }
        // Click Events
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.formClose = true;
            this.Close();
        }
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            MSTAccFactory delObjectAcc = null;
            MSTConFactory delObjectCus = null;
            MSTShipNameFactory delObjectShipNm = null;
            MSTJobFactory delObjectJob = null;
            MSTItmFactory delObjectItm = null;
            MSTPriceInfoFactory delObjectPrice = null;
            TASToDoFactory delObjectToDo = null;

            int PreRowIndex = 0;

            try
            {          
                #region Prompt
                if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnDeleteRecord))
                {
                    // Ask Confirmation for Delete
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know);

                    // Check Delete
                    if (btnSelect != GEnum.MsgBoxButton.Delete)
                        return;
                }
                #endregion

                if (tagrdList.ActiveRow == null)
                    return;

                #region delete process
                switch ((GEnum.SystemCode)CodeKey)
                {
                    case GEnum.SystemCode.Account:
                        delObjectAcc = new MSTAccFactory(GEnum.InstanceMode.InternalCall);
                        if (delObjectAcc.GetEdit((int)tagrdList.ActiveRow.Cells["Key"].Value, ""))
                        {
                            delObjectAcc.Delete();
                            delObjectAcc.Dispose();                                        
                        }
                        else
                            return;
                        break;

                    case GEnum.SystemCode.Customer:
                    case GEnum.SystemCode.Vendor:
                        delObjectCus = new MSTConFactory(GEnum.InstanceMode.InternalCall, (GEnum.SystemCode)CodeKey);                      
                        if (delObjectCus.GetEdit((int)tagrdList.ActiveRow.Cells["Key"].Value, ""))
                        {
                            delObjectCus.Delete();
                            delObjectCus.Dispose();
                        }
                        else
                            return;
                        break;

                    case GEnum.SystemCode.Inventory:
                        delObjectItm = new MSTItmFactory(GEnum.InstanceMode.InternalCall);
                        if (delObjectItm.GetEdit((int)tagrdList.ActiveRow.Cells["Key"].Value, string.Empty))
                        {
                            delObjectItm.Delete();
                            delObjectItm.Dispose();
                        }
                        else
                            return;
                        break;

                    case GEnum.SystemCode.Ship_Name:
                        delObjectShipNm = new MSTShipNameFactory(GEnum.InstanceMode.InternalCall);
                        if (delObjectShipNm.GetEdit((int)tagrdList.ActiveRow.Cells["Key"].Value))
                        {
                            delObjectShipNm.Delete();
                            delObjectShipNm.Dispose();
                        }
                        else
                            return;
                        break;
                       
                    case GEnum.SystemCode.Job:
                        delObjectJob = new MSTJobFactory(GEnum.InstanceMode.InternalCall);
                        if (delObjectJob.GetEdit((int)tagrdList.ActiveRow.Cells["Key"].Value,string.Empty))
                        {
                            delObjectJob.Delete();
                            delObjectJob.Dispose();
                        }
                        else
                            return;
                        break;

                    case GEnum.SystemCode.Price_List:
                        delObjectPrice = new MSTPriceInfoFactory(GEnum.InstanceMode.InternalCall);
                        if (delObjectPrice.GetEdit((int)tagrdList.ActiveRow.Cells["Key"].Value))
                        {
                            delObjectPrice.Dispose();
                            delObjectPrice.Delete();
                        }
                        else
                            return;
                        break;
                    case GEnum.SystemCode.To_Do:
                        delObjectToDo = new TASToDoFactory(GEnum.InstanceMode.InternalCall);
                        if (delObjectToDo.GetEdit((int)tagrdList.ActiveRow.Cells["Key"].Value))
                        {
                            delObjectToDo.Dispose();
                            delObjectToDo.Delete();
                        }
                        else
                            return;
                        break;
                    case GEnum.SystemCode.Alerts:
                        TASAlertFactory delObjectAlert = new TASAlertFactory(GEnum.InstanceMode.InternalCall);
                        if (delObjectAlert.GetEdit((int)tagrdList.ActiveRow.Cells["Key"].Value))
                        {
                            delObjectAlert.Dispose();
                            delObjectAlert.Delete();
                        }
                        else
                            return;
                        break;
                }
                #endregion

                //Move the cursor position of active row index to upper row
                if (tagrdList.ActiveRow.Index > 0)
                    PreRowIndex = tagrdList.ActiveRow.Index - 1;

                GlobalUI.Grid_Format(tagrdList, ListSettingID, true, false);
                if (tagrdList.Rows.Count > 0)
                {
                    tagrdList.Rows[PreRowIndex].Selected = true;
                    tagrdList.Rows[PreRowIndex].Activate();
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
                if (delObjectAcc != null)
                    delObjectAcc.Dispose();
                if (delObjectCus != null)
                    delObjectCus.Dispose();
                if (delObjectShipNm != null)
                    delObjectShipNm.Dispose();
                if (delObjectJob != null)
                    delObjectJob.Dispose();
                if (delObjectItm != null)
                    delObjectItm.Dispose();
                if (delObjectPrice != null)
                    delObjectPrice.Dispose();
            }
        }
        private void tsbEdit_Click(object sender, EventArgs e)
        {
            int key = 0;
            try
            {
                if (!GFunc.IsNE(tagrdList.ActiveRow))
                {
                    key = GFunc.NEInt(tagrdList.ActiveRow.Cells["Key"].Value, 0);
                    if (this.ListEvent_OpenRecord != null)
                    {
                        this.ListEvent_OpenRecord.Invoke(key);                    
                    }
                }
                //Mark active key
                tagrdList.Tag = key;
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
        private void tsbExport_Click(object sender, EventArgs e)
        {
            try
            {
                int count = tsbExport.Tag == null ? 1 : (int)tsbExport.Tag;
                GlobalUI.Export(tagrdList, ref count);
                tsbExport.Tag = count;
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

        // private methods
        public void OnCaller_Changed()
        {
            if(ListSettingID!="")
                GlobalUI.Grid_Format(tagrdList, ListSettingID, true, false);
        }
        
        private void SetFormSetting()
        {
            switch (objListFtry.CodeKey)
            {
                case GEnum.SystemCode.Customer:                  
                    GlobalUI.MSTConListEvent += new GVar.ListUpdateEvent(this.OnCaller_Changed);
                    break;

                case GEnum.SystemCode.Vendor:                   
                    GlobalUI.MSTConListEvent += new GVar.ListUpdateEvent(this.OnCaller_Changed);
                    break;

                case GEnum.SystemCode.To_Do:                 
                    GlobalUI.MSTConListEvent += new GVar.ListUpdateEvent(this.OnCaller_Changed);
                    break;
                case GEnum.SystemCode.Alerts:                   
                    break;
                case GEnum.SystemCode.Inventory:                 
                    break;

                case GEnum.SystemCode.Job:
                    tsbItemList.Visible = true;
                    break;

                case GEnum.SystemCode.Account:
                    break;

                case GEnum.SystemCode.Price_List:
                    break;
                case GEnum.SystemCode.Ship_Name:
                    break;
            }
        }       
        private void ClearGlobalEvent()
        {
            if (!GFunc.IsNE(this.ListEvent_CloseFORM))
                this.ListEvent_CloseFORM.Invoke();           
        }
        private void FormatGrid(bool flag)
        {
            int c = tagrdList.DisplayLayout.Bands[0].Columns.Count;
            if ((colHeaders.Length == c && colWidths.Length == c && colFormats.Length == c && flag == false) || (flag == true))
                for (int i = 0; i < colHeaders.Length; i++)
                {
                    if (Convert.ToInt16(colWidths[i]) == 0)
                    {
                        this.tagrdList.DisplayLayout.Bands[0].Columns[i].Hidden = true;
                    }
                    else
                    {
                        if (this.tagrdList.DisplayLayout.Bands[0].Columns[i].Hidden)
                            this.tagrdList.DisplayLayout.Bands[0].Columns[i].Hidden = false;

                        this.tagrdList.DisplayLayout.Bands[0].Columns[i].Header.Caption = colHeaders[i];
                        this.tagrdList.DisplayLayout.Bands[0].Columns[i].Width = Convert.ToInt16(colWidths[i]);
                        this.tagrdList.DisplayLayout.Bands[0].Columns[i].Header.Appearance.FontData.Bold = DefaultableBoolean.True;

                        if (tagrdList.DisplayLayout.Bands[0].Columns[i].Key.Equals("NormalTimeStart")
                            || tagrdList.DisplayLayout.Bands[0].Columns[i].Key.Equals("NormalTimeEnd"))
                        {
                            tagrdList.DisplayLayout.Bands[0].Columns[i].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTimeWithoutDropDown;
                            tagrdList.DisplayLayout.Bands[0].Columns[i].Format = "h:mm:ss tt";
                        }
                        else if (tagrdList.DisplayLayout.Bands[0].Columns[i].DataType == typeof(DateTime?)
                            || tagrdList.DisplayLayout.Bands[0].Columns[i].DataType == typeof(DateTime))
                        {
                            tagrdList.DisplayLayout.Bands[0].Columns[i].Format = "dd MMM yyyy";
                        }

                        if (this.tagrdList.DisplayLayout.Bands[0].Columns[i].DataType == typeof(bool?))
                            this.tagrdList.DisplayLayout.Bands[0].Columns[i].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                        this.tagrdList.DisplayLayout.Bands[0].Columns[i].CellActivation = Activation.ActivateOnly;

                        this.tagrdList.DisplayLayout.Bands[0].Columns[i].Header.VisiblePosition = Convert.ToInt32(colFormats[i]);

                    }
                }
        }
        public void OnCaller_Close()
        {
            formClose = true;
            this.Close();
            
        }

        //Context Menu
        private void formatGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmGrdFormat fgrdformat = new frmGrdFormat(tagrdList, ListSettingID);
                fgrdformat.ShowDialog();
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

        #region Error

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

        #endregion

        private void frmList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
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
        }

        //ttm
        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Grid_Rebind();
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
        private void Grid_Rebind()
        {
            int SelectedDocKey = 0;

            try
            {
                string prevFilter = "";

                if (tagrdList.DataSource != null)
                    prevFilter = (tagrdList.DataSource as DataTable).DefaultView.RowFilter;

                if (tagrdList.ActiveRow != null)
                    SelectedDocKey = GFunc.NEInt(tagrdList.ActiveRow.Cells["key"].Value, 0);

                string listID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "0GF", tagrdList.Name);
                GlobalUI.Grid_Format(tagrdList, listID, true, false);
                (tagrdList.DataSource as DataTable).DefaultView.Sort = "CreateDate Desc ,ID desc";
                (tagrdList.DataSource as DataTable).DefaultView.RowFilter = prevFilter;

                if (SelectedDocKey > 0)
                {
                    UltraGridRow gRow = this.tagrdList.Rows.OfType<UltraGridRow>().ToList().Find(
                    row => row.Cells["key"].Text.Equals(SelectedDocKey.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    if (gRow != null)
                    {
                        tagrdList.ActiveRow = gRow;
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void tsbItemList_Click(object sender, EventArgs e)
        {
            bool alreadyExist = false;
            try
            {
                foreach (Form form in this.ParentForm.OwnedForms)
                {
                    if (form.Name == GlobalUI.Form_Name.FRM_DOCLISTDET)
                    {
                        alreadyExist = true;
                    }
                }
                if (!alreadyExist)
                {
                    int docKey = tagrdList.ActiveRow == null ? 0 : GFunc.NEInt(tagrdList.ActiveRow.Cells["Key"].Value, 0);
                    GlobalUI.PopupDisplay(GlobalUI.Form_Name.FRM_DOCLISTDET, (GEnum.SystemCode)_CodeKey, docKey);
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

        private void tagrdList_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                int docKey = tagrdList.ActiveRow == null ? 0 : GFunc.NEInt(tagrdList.ActiveRow.Cells["Key"].Value, 0);
                GlobalUI.PopupRefresh((GEnum.SystemCode)_CodeKey, docKey);
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