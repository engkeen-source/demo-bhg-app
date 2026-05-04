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
    //Not used; Code may not be consistent with other forms
    public partial class frmREFEqptType : Form
    {
        #region Local Variables

        private BOLib.REFEqptTypeFactory objEqptTypeFactory = null;
        string ContextMenuSetting = string.Empty;
        private string msgID = string.Empty;
        private bool formEdit = false;
        private bool formClose = false;
        private string localEqptTypeID = string.Empty;
        private int localEqptTypeKey = 0;


        //only use to show Read only data viewer for AuditLog 
        private bool IsOpenFromAuditLog = false;
        private DataTable _dtHeader = null;
        private DataSet _dsDetail = null;


        #endregion

        #region Initialize

        public frmREFEqptType()
        {
            InitializeComponent();
        }
        public frmREFEqptType(string ID)
        {
            InitializeComponent();
            localEqptTypeID = ID;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        public frmREFEqptType(int Key)
        {
            InitializeComponent();
            formEdit = true;
            localEqptTypeKey = Key;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        #endregion

        //Form Events
        private void frmREFEqptType_Load(object sender, EventArgs e)
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.tagrdEqptTypes.DisplayLayout.Bands[0].SortedColumns.Clear();
                // Call Initialization
                this.objEqptTypeFactory = new BOLib.REFEqptTypeFactory(BOLib.GEnum.InstanceMode.Normal);
                this.objEqptTypeFactory.EqptTypeNotifier += new GVar.UINotifierEvent(this.EqptTypeNotifier);
                // Attach Event on Factory
                this.objEqptTypeFactory.dirtyEvent += new GVar.DirtyEvent(this.OnDirty);

                if (objEqptTypeFactory.GUID <= 0)
                {
                    formClose = true;
                    return;
                }                                

                this.New_Process();

                this.Refresh_EqptTypeList();
                
                if (formEdit)
                {
                    if (this.objEqptTypeFactory.GetEdit(localEqptTypeKey))
                    {
                        //Binding Object
                        Refresh_EqptTypeInfo();
                    }                    
                }

                //GlobalUI.cmnuGlobal_Set(this);                
                GlobalUI.FormGrids_Set(this, (int)objEqptTypeFactory.ConstantCodeKey, out ContextMenuSetting);
                ContextMenuSetting = GlobalUI.ContextMenuSetting_GetNew((int)objEqptTypeFactory.ConstantCodeKey);

                this.tagrdEqptTypes.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                this.tagrdEqptTypes.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
                tagrdEqptTypes.DisplayLayout.Bands[0].Override.ActiveCellAppearance.BackColor = Color.Gold;
            }
            catch (TAException tex)
            {
                // Check Process 
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
                Error(ex, true); // System Msg   
            }
            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }
        private void frmREFEqptType_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }
            if (this.formClose && objEqptTypeFactory == null)
            {
                return;
            }
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (!this.IsSaveChanges())
                {
                    // Call Dispose
                    bool isOk = this.objEqptTypeFactory.Dispose();

                    // Check Process
                    if ((!isOk) && this.msgID != string.Empty)
                        MsgBox.Show(msgID); // Custom Msg
                }
                else
                {
                    //When the form is closed by main form, to prohibit closing
                    frmMain.gfrmMain.Tag = GVar.CancelMainFormClosing;
                    e.Cancel = true;
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true); ; // System Msg 
            }
            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }
        private void frmREFEqptType_Shown(object sender, EventArgs e)
        {
            // Check Form Close State is True ...
            if (formClose)
            {
                this.Close();
            }
            else
            {
                this.tagrdEqptTypes.Selected.Rows.Clear();
                this.splitContainer1.Panel2.Focus();
                this.EqptTypeID.Focus();
            }
        }
        private void frmREFEqptType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F6)
                {
                    this.tagrdEqptTypes.Focus();
                }

                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)GEnum.SystemCode.Purchase_Adjustment);
                    //CombosDependent_Fill(string.Empty);
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
        }

        //Menu Strip Events
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.formClose = true;
            this.Close();
        }
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
                Error(ex, true); ; // System Msg 
            }
        }
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
                Error(ex, true); ; // System Msg 
            }
        }
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
                Error(ex, true); ; // System Msg 
            }
        }
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
                Error(ex, true); ; // System Msg 
            }
        }

        //Grid Common Events
        private void tagrdEqptType_BeforeSelectChange(object sender, BeforeSelectChangeEventArgs e)
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            // Variable Declaration
            bool isOk = false;

            try
            {
              

                // Call Save Changes
                isOk = this.IsSaveChanges();

                // Check Process
                if ((isOk) && msgID == string.Empty)
                {
                    //I Don't Know
                    e.Cancel = true;
                    if (this.tagrdEqptTypes.Selected.Cells.Count > 0)
                        this.tagrdEqptTypes.Selected.Cells[0].Activate();
                    return;
                }
                else if ((isOk) && msgID != string.Empty)
                {
                    //Validation Fail
                    e.Cancel = true;
                    if (this.tagrdEqptTypes.Selected.Cells.Count > 0)
                        this.tagrdEqptTypes.Selected.Cells[0].Activate();

                    this.EqptTypeID.Focus();
                    return;
                }

                //New Select Row
                if (e.NewSelections.Cells.Count > 0)
                {
                    this.tsbDelete.Enabled = true;

                    int nEqptTypeKey = GFunc.NEInt(e.NewSelections.Cells[0].Row.Cells["EqptTypeKey"].Value, 0);
                    if (nEqptTypeKey != this.objEqptTypeFactory.ObjREFEqptType.EqptTypeKey)
                    {
                        // Call GetEdit
                        isOk = objEqptTypeFactory.GetEdit(nEqptTypeKey);

                        if (!isOk)
                        {
                            if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnRestoreRecord))
                            {
                                // Ask Confirmation For ReadOnly
                                GEnum.MsgBoxButton btnSelect;
                                btnSelect = MsgBox.Show(MsgID.Common.ResponseOpenAsReadOnly,
                                                      GEnum.MsgBoxIcon.Warning,
                                                      GEnum.MsgBoxButton.Yes,
                                                      GEnum.MsgBoxButton.No,
                                                      GEnum.MsgBoxButton.I_Dont_Know);

                                // Yes, i want to use as readonly
                                if (btnSelect == GEnum.MsgBoxButton.Yes)
                                {
                                    // Call ReadOnly
                                    isOk = this.objEqptTypeFactory.GetReadOnly(nEqptTypeKey);
                                  
                                }
                                // No, i don't want
                                else if (btnSelect == GEnum.MsgBoxButton.No)
                                {
                                    // Call Edit
                                    isOk = objEqptTypeFactory.GetEdit(this.objEqptTypeFactory.ObjREFEqptType.EqptTypeKey);

                                }
                                // Cancel Process
                                else
                                    return;
                            }
                            else
                            {
                                // Call ReadOnly
                                isOk = this.objEqptTypeFactory.GetReadOnly(nEqptTypeKey);
                            }
                        }
                    }

                    this.Refresh_EqptTypeInfo();

                    // Call ReadOnly
                    this.OnReadOnly();
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true); ; // System Msg 
            }
            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }
        private void tagrdEqptType_Click(object sender, EventArgs e)
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            bool isOk = false;

            try
            {
               

                isOk = this.IsSaveChanges();

                if ((isOk) && msgID == string.Empty)
                {
                    //I Don't Know
                    if (this.tagrdEqptTypes.Selected.Cells.Count > 0)
                        this.tagrdEqptTypes.Selected.Cells[0].Activate();
                    return;
                }
                else if ((isOk) && msgID != string.Empty)
                {
                    //Validation Fail
                    if (this.tagrdEqptTypes.Selected.Cells.Count > 0)
                        this.tagrdEqptTypes.Selected.Cells[0].Activate();

                    this.EqptTypeID.Focus();
                    return;
                }

                // Check Binding Source Record Count
                if (this.bdsEqptTypes.Count != 0)
                {
                    // Check Factory Object is Dirty ...
                    if (this.objEqptTypeFactory.IsDirty)
                    {
                        int nEqptTypeKey = ((REFEqptType)this.bdsEqptTypeDet.Current).EqptTypeKey.Value;
                        if (nEqptTypeKey == this.objEqptTypeFactory.ObjREFEqptType.EqptTypeKey)
                        {
                            // Call GetEdit
                            isOk = this.objEqptTypeFactory.GetEdit(nEqptTypeKey);

                            if (!isOk)
                            {                              
                                if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnRestoreRecord))
                                {
                                    // Ask Confirmation For ReadOnly
                                    GEnum.MsgBoxButton btnSelect;
                                    btnSelect = MsgBox.Show(MsgID.Common.ResponseOpenAsReadOnly,
                                                          GEnum.MsgBoxIcon.Warning,
                                                          GEnum.MsgBoxButton.Yes,
                                                          GEnum.MsgBoxButton.No,
                                                          GEnum.MsgBoxButton.I_Dont_Know);

                                    // Yes, I want to see
                                    if (btnSelect == GEnum.MsgBoxButton.Yes)
                                    {
                                        // Call GetReadOnly
                                        isOk = this.objEqptTypeFactory.GetReadOnly(nEqptTypeKey);
                                       
                                    }
                                    // No, I don't want to see
                                    else if (btnSelect == GEnum.MsgBoxButton.No)
                                    {
                                        // Call GetEdit
                                        isOk = objEqptTypeFactory.GetEdit(this.objEqptTypeFactory.ObjREFEqptType.EqptTypeKey);

                                    }
                                    else
                                        return;
                                }
                                else
                                {
                                    // Call GetReadOnly
                                    isOk = this.objEqptTypeFactory.GetReadOnly(nEqptTypeKey);
                                   
                                }
                            }
                        }
                    }
                    this.Refresh_EqptTypeInfo();
                    this.errorProvider1.Clear();
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true); ; // System Msg 
            }
            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }
        private void tagrdEqptType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EqptTypeID.Focus();
            }
        }

        //Control data refresh - Dependant combo, TextEditorPopup, Grid - Combo List, Set/Clear TextEditorPop value and Grid binding source and filter
        private void Refresh_EqptTypeList()
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // Call read-only list 
                this.bdsEqptTypes.DataSource = BOLib.REFList.GetEqptTypes(out msgID);

                // Databinding for EqptType ReadOnly List
                this.tagrdEqptTypes.DataSource = this.bdsEqptTypes;

                if (this.tagrdEqptTypes.DisplayLayout.Bands[0].SortedColumns.Count <= 0)
                    this.tagrdEqptTypes.DisplayLayout.Bands[0].SortedColumns.Add(tagrdEqptTypes.DisplayLayout.Bands[0].Columns["EqptTypeID"], false);

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
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }
        private void Refresh_EqptTypeInfo()
        {
            try
            {
                // Object binding for EqptType Information 
                this.bdsEqptTypeDet.DataSource = objEqptTypeFactory.ObjREFEqptType;
                this.bdsEqptTypeDet.ResetBindings(false);

                if (!GFunc.IsNE(localEqptTypeID))
                {
                    EqptTypeID.Text = localEqptTypeID;//localEqptTypeID.ToString(); 
                    objEqptTypeFactory.ObjREFEqptType.EqptTypeID = EqptTypeID.Text;                    
                }

                this.EqptTypeID.MaxLength = 50;
                this.EqptTypeDes.MaxLength = 255;
                this.Custom1.MaxLength = 255;
                this.Custom2.MaxLength = 255;
                this.Custom3.MaxLength = 255;
                if (objEqptTypeFactory.IsDirty)
                {
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
        private bool Save_Process()
        {
            // Waiting cursor
            this.Cursor = Cursors.WaitCursor;

            // Variable Declaration
            bool isSave = false;

            try
            {
                // Validation
                this.Validate();

                // Check Factory Object is not null
                if (this.objEqptTypeFactory != null)
                {
                    // Call Save
                    isSave = this.objEqptTypeFactory.Save();
                    // Check Process
                    if (isSave)
                    {
                        this.Refresh_EqptTypeList();
                        this.Refresh_EqptTypeInfo();
                        this.errorProvider1.Clear();

                        // Call ReadOnly
                        this.OnReadOnly();

                        GlobalUI.ResetControlDirty(this);
                    }
                    else
                    {
                        throw new TAException(objEqptTypeFactory.ErrorMessageID);
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
                // Default cursor
                this.Cursor = Cursors.Default;
            }
            return isSave;
        }
        private bool New_Process()
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            //Variable Declaration
            bool isNew = true;

            try
            {
                // Clear Error
                this.errorProvider1.Clear();

                // Check Form Validation 
                this.Validate();

                // Check Factory Object is Dirty ...
                if (this.objEqptTypeFactory.IsDirty)
                {
                    // Call Save
                    isNew = this.Save_Process();
                }

                // Check IsNew is True ... 
                if (isNew)
                {
                    // Call New
                    isNew = this.objEqptTypeFactory.New();
                }

                // Check IsNew is True ... 
                if (isNew)
                {
                    this.Refresh_EqptTypeInfo();

                    // Call ReadOnly
                    this.OnReadOnly();
                    GlobalUI.ResetControlDirty(this);

                    this.tsbDelete.Enabled = false;
                    this.tsbClear.Enabled = true;
                    this.splitContainer1.Panel2.Focus();
                    this.EqptTypeID.Focus();
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
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
            return isNew;
        }
        private bool Delete_Process()
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            // Variable Declaration
            bool isDelete = true;

            try
            {                
                // Check Option Value is True (or) False
                if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnDeleteRecord))
                {
                    // Ask Confirmation for Delete
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show(MsgID.Common.ConfirmDelete, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete, GEnum.MsgBoxButton.Dont_Delete, GEnum.MsgBoxButton.I_Dont_Know);

                    // Check Delete
                    if (btnSelect == GEnum.MsgBoxButton.Delete)
                    {
                        // Call Delete
                        isDelete = this.objEqptTypeFactory.Delete();
                      
                    }
                    else
                        isDelete = false;  // Cancel Process
                }
                else
                {
                    // Call Delete
                    isDelete = this.objEqptTypeFactory.Delete();
                }            

                // Check Process
                if (isDelete)
                {
                    this.New_Process();
                    this.Refresh_EqptTypeList();
                    this.Refresh_EqptTypeInfo();

                    GlobalUI.ResetControlDirty(this);
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
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
            return isDelete;
        }
        private bool Clear_Process()
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            // Variable Declaration
            bool isClear = true;

            try
            {
                // Check Form Validation
                this.Validate();

                // Check Factory Object is Dirty ...
                if (this.objEqptTypeFactory.IsDirty)
                {                    
                    // Check Option Value is True (or) False
                    if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnClearRecord))
                    {
                        // Ask Confirmation for Delete
                        GEnum.MsgBoxButton btnSelect;
                        btnSelect = MsgBox.Show(MsgID.Common.ConfirmClear,
                                              GEnum.MsgBoxIcon.Question,
                                              GEnum.MsgBoxButton.Clear,
                                              GEnum.MsgBoxButton.Dont_Clear,
                                              GEnum.MsgBoxButton.I_Dont_Know);

                        // Check Clear
                        if (btnSelect == GEnum.MsgBoxButton.Clear)
                        {
                            // Call Clear
                            isClear = this.objEqptTypeFactory.New();
                           
                        }
                        else
                            isClear = false;  // Cancel Process
                    }
                    else
                    {
                        // Call Clear
                        isClear = this.objEqptTypeFactory.New();
                    }

                    // Check Process
                    if (isClear)
                    {
                        this.Refresh_EqptTypeInfo();
                        this.errorProvider1.Clear();
                        this.splitContainer1.Panel2.Focus();
                        this.EqptTypeID.Focus();
                        GlobalUI.ResetControlDirty(this);
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
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
            return isClear;
        }
        private void OnReadOnly()
        {
            // Set Readonly True (or) False. Based on Factory ReadOnly State       
            this.EqptTypeID.ReadOnly = this.objEqptTypeFactory.IsOpenReadOnly;
            this.EqptTypeDes.ReadOnly = this.objEqptTypeFactory.IsOpenReadOnly;
            this.Custom1.ReadOnly = this.objEqptTypeFactory.IsOpenReadOnly;
            this.Custom2.ReadOnly = this.objEqptTypeFactory.IsOpenReadOnly;
            this.Custom3.ReadOnly = this.objEqptTypeFactory.IsOpenReadOnly;

            // Check Factory Object is ReadOnly ...
            if (this.objEqptTypeFactory.IsOpenReadOnly)
            {
                this.tslReadOnly.Text = "Read Only";
                this.tsbDelete.Enabled = false;
                this.tsbSave.Enabled = false;
            }
            else
            {
                this.tslReadOnly.Text = string.Empty;
                this.tsbDelete.Enabled = true;
                this.tsbSave.Enabled = true;
            }

            this.tsbClear.Enabled = false;

            // Clear Error
            this.errorProvider1.Clear();
        }
        private bool IsSaveChanges()
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            // Variable Declaration
            bool isSave = false;

            try
            {
                // Check Form Validation
                this.Validate();

                // Check Factory Object is Dirty ...
                if (objEqptTypeFactory.IsDirty)
                {
                    // Ask Confirmation To Save
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show(MsgID.Common.SaveChanges,
                                          GEnum.MsgBoxIcon.Question,
                                          GEnum.MsgBoxButton.Save_Changes,
                                          GEnum.MsgBoxButton.Discard_Changes,
                                          GEnum.MsgBoxButton.I_Dont_Know);

                    // Yes, I want to save
                    if (btnSelect == GEnum.MsgBoxButton.Save_Changes)
                        isSave = !this.Save_Process();
                    // No, I don't know 
                    else if (btnSelect == GEnum.MsgBoxButton.I_Dont_Know)
                        isSave = true;
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
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
            return isSave;
        }
        private void EqptTypeNotifier(object sender, BOLib.UINotifierEventArgs e)
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
                Error(ex, true); ; // System Msg 
            }
        }        

        //Event Method
        private void OnDirty(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName.ToLower())
            {
                case "eqpttypeid":
                    this.errorProvider1.SetError(this.EqptTypeID, string.Empty);
                    break;
                case "eqpttypedes":
                    this.errorProvider1.SetError(this.EqptTypeDes, string.Empty);
                    break;
            }
        }
        private void OnHeaderError(string errorMsg,string propertyName)
        {
            switch (propertyName.ToLower())
            {
                case "eqpttypeid":
                    this.errorProvider1.SetError(this.EqptTypeID, errorMsg);
                    EqptTypeID.Focus();
                    break;

                case "eqpttypedes":
                    this.errorProvider1.SetError(this.EqptTypeDes, errorMsg);
                    EqptTypeDes.Focus();
                    break;
            }
        }

        #region Error
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
        #endregion
        
    }
}