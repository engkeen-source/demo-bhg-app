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
using Infragistics.Win.UltraWinTree;
using System.Data.SqlClient;
using BOLib;
using TAUtil;

namespace WinUI
{
    public partial class frmMstAccTranGrp : Form
    {
        #region Local Variables

        private BOLib.MSTAccTranGrpFactory objTranGrpFactory = null;
        string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        private GEnum.formInitMode formOpenMode = GEnum.formInitMode.Neither;
        private int recordKey = 0;
        private string recordID = "";
        private bool canEditRecordID = false;
        private string msgID = string.Empty;
        private bool ListSyncInprogress = false;

        //only use to show Read only data viewer for AuditLog 
        private bool IsOpenFromAuditLog = false;
        private DataTable _dtHeader = null;
        private DataSet _dsDetail = null;

        private UltraTreeNode oFormEditSeletedNode = null;
        private DataTable dtTreeData = null;
        private const int DEF_ParentKey = 0;
        private UltraTreeNode oCurrNode = null;

        #endregion

        // Initialize
        public frmMstAccTranGrp()
        {
            InitializeComponent();
        }//Completed
        public frmMstAccTranGrp(string id)
        {
            //For Call from Shortcut menu (Edit/Add)
            InitializeComponent();
            recordID = id;
            formOpenMode = GEnum.formInitMode.Add;
            TranGrpID.Tag = id;
            this.taTreeGroup.DisplayStyle = UltraTreeDisplayStyle.WindowsVista;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }//Completed
        public frmMstAccTranGrp(int key)
        {
            //For call from shortcut menu (Edit)
            InitializeComponent();
            formOpenMode = GEnum.formInitMode.Edit;
            recordKey = key;
            TranGrpID.Tag = key;
            this.taTreeGroup.DisplayStyle = UltraTreeDisplayStyle.WindowsVista;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }//Completed
        public frmMstAccTranGrp(GEnum.SystemCode DocCodeKey, DataTable dtHeader, DataSet dsDetail)
        {
            //Open from AuditLog as Read only data viewer
            InitializeComponent();
            IsOpenFromAuditLog = true;
            _dtHeader = dtHeader;
            _dsDetail = dsDetail;
        }//Completed

        //Form Events
        private void frmMstAccTranGrp_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {                
                //Call Initialization
                this.objTranGrpFactory = new BOLib.MSTAccTranGrpFactory(BOLib.GEnum.InstanceMode.Normal);
                if (objTranGrpFactory.GUID <= 0)
                {
                    formClose = true;
                    return;
                }

                //Attach Event on Factory
                this.objTranGrpFactory.ErrorNotifierHeader_Set += new GVar.UINotifierEvent(this.ErrorNotifier_Set);
                this.objTranGrpFactory.ErrorNotifierHeader_Clear += new GVar.UINotifierEvent(this.ErrorNotifier_Clear);

                

                //Check if user has permission to edit Record ID
                canEditRecordID = SECPermUtility.Perform("sysRecID", false);

                if (this.IsOpenFromAuditLog)
                {
                    if (objTranGrpFactory.SetReadOnlyData(_dtHeader, _dsDetail) == false)
                    {
                        formClose = true;
                        return;
                    }
                    GetTranGrpList();
                    Refresh_Header();
                    GlobalUI.FormEnable_Set(this, false);
                }
                else
                {
                    this.New_Process(DEF_ParentKey,false);
                    GetTranGrpList();
                    //When open from shortcutmenu (edit)
                    if (formOpenMode == GEnum.formInitMode.Edit)
                        this.OpenRecord(recordKey);
                    else if (formOpenMode == GEnum.formInitMode.Add)
                    {
                        if (canEditRecordID && recordID != string.Empty)
                            this.TranGrpID.SetValueTrigger(recordID, false);
                    }
                }

                this.taTreeGroup.DisplayStyle = UltraTreeDisplayStyle.WindowsVista;

                //Setup Grid Layout, ShortcutMenu, fill Combo
                GlobalUI.FormGrids_Set(this, (int)objTranGrpFactory.ConstantCodeKey, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objTranGrpFactory.ConstantCodeKey);
                //GlobalUI.cmnuGlobal_Set(this);
                GlobalUI.Combos_Fill(this, (int)objTranGrpFactory.ConstantCodeKey);
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
        private void frmMSTAccTranGrp_Shown(object sender, EventArgs e)
        {
            //Close Form when FormLoad has encounter errors
            if (formClose)
                this.Close();
            else
                TranGrpID.Focus();

        }//Completed
        private void frmMstAccTranGrp_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool runProcess = false;
            this.formClose = true;

            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }
            if (this.formClose && objTranGrpFactory == null)
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
                        e.Cancel = false;
                    }
                }
                #endregion

                //Dispose Factory
                if ((bool)this.objTranGrpFactory.Dispose() == false)
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
                    this.objTranGrpFactory.Dispose();
            }
        }//Completed        
        private void frmMstAccTranGrp_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)objTranGrpFactory.ConstantCodeKey);
                    //CombosDependent_Fill(string.Empty);
                }
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
        private void tsbNewGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (taTreeGroup.SelectedNodes.Count > 0)
                    this.New_Process(GFunc.NEInt(taTreeGroup.SelectedNodes[0].Tag, 0),false);
                else
                    this.New_Process(DEF_ParentKey, false);
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
        private void tsbNewSubGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (taTreeGroup.SelectedNodes.Count > 0)
                    this.New_Process(Convert.ToInt32(taTreeGroup.SelectedNodes[0].Key),true);
                else
                    MsgBox.Show(MsgID.Common.CannotBeEmpty + "%Transaction Group to add Sub Group ");
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
            this.Delete_Process();
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
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }//Completed                

        //Functions
        private void Refresh_All()
        {
            try
            {
                Refresh_Header();              
                GlobalUI.ResetControlDirty(this);
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
        private void Refresh_Header()
        {
            try
            {
                this.bdsTranGrpDet.DataSource = objTranGrpFactory.ObjMSTAccTranGrp;
                this.bdsTranGrpDet.AllowNew = true;
                this.bdsTranGrpDet.ResetBindings(false);
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
        private void Refresh_GridList()
        {
            try
            {
                string msgID = string.Empty;
                ListSyncInprogress = true;
                GetTranGrpList();
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
                ListSyncInprogress = false;
            }
        }//Completed
        private bool GetTranGrpList()
        {
            this.taTreeGroup.Nodes.Clear();

            try
            {
            
                dtTreeData = MASList.GetMSTAccTranGrps(0, null);
           
                UltraTreeNode parent = null;
                BuildTreeView(0, parent);
                if (oCurrNode != null)
                {
                    taTreeGroup.ActiveNode = oCurrNode;
                    taTreeGroup.ActiveNode.Selected = true;
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
            return false;
        }
        private void BuildTreeView(int iParent, UltraTreeNode oParentNode)
        {
            try
            {
                IEnumerable<DataRow> dtRows = dtTreeData.AsEnumerable().Where(r => r.Field<int>("TranGrpKeyParent") == iParent).OrderBy(o => o.Field<string>("TranGrpID"));

                foreach (DataRow row in dtRows)
                {
                    if (oParentNode == null)
                    {
                        UltraTreeNode oNode = new UltraTreeNode(row["TranGrpID"].ToString());
                        oNode.Key = GFunc.NEStr(row["TranGrpKey"], ""); 
                        oNode.Tag = GFunc.NEStr(row["TranGrpKeyParent"], "");
                        this.taTreeGroup.Nodes.Add(oNode);

                        if (formOpenMode == GEnum.formInitMode.Edit)
                            if (oNode.Key.Trim() == TranGrpID.Tag.ToString())
                                oFormEditSeletedNode = oNode;

                        if (objTranGrpFactory != null)
                            if (!objTranGrpFactory.IsNew)
                                if (oNode.Key == objTranGrpFactory.ObjMSTAccTranGrp.TranGrpKey.ToString())
                                    oCurrNode = oNode;
                        this.BuildTreeView(Int32.Parse(row["TranGrpKey"].ToString()), oNode);
                    }
                    else
                    {
                        UltraTreeNode oNode = new UltraTreeNode(row["TranGrpID"].ToString());
                        oNode.Key = GFunc.NEStr(row["TranGrpKey"], "");
                        oNode.Tag = GFunc.NEStr(row["TranGrpKeyParent"], "");
                        oParentNode.Nodes.Add(oNode);
                        if (formOpenMode == GEnum.formInitMode.Edit)
                            if (oNode.Key.Trim() == TranGrpID.Tag.ToString())
                                oFormEditSeletedNode = oNode;

                        if (objTranGrpFactory != null)
                            if (!objTranGrpFactory.IsNew)
                                if (oNode.Key == objTranGrpFactory.ObjMSTAccTranGrp.TranGrpKey.ToString())
                                    oCurrNode = oNode;
                        this.BuildTreeView(Int32.Parse(row["TranGrpKey"].ToString()), oNode);
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
        }
        private void FormLayout()
        {
            try
            {
                bool EnableMode = !this.objTranGrpFactory.IsReadOnly;
                this.tslReadOnly.Text = !EnableMode ? "Read Only" : string.Empty;

                //Special Condition for RecordID
                this.TranGrpID.Enabled = EnableMode;
                this.TranGrpID.ReadOnly = !canEditRecordID;

                this.TranGrpDes.Enabled = EnableMode;
                this.Custom1.Enabled = EnableMode;
                this.Custom2.Enabled = EnableMode;
                this.Custom3.Enabled = EnableMode;
                this.Custom4.Enabled = EnableMode;
                this.Custom5.Enabled = EnableMode;

                if (EnableMode == false)
                {
                    this.tsbSave.Enabled = false;
                    this.tsbDelete.Enabled = false;
                    this.tsbClear.Enabled = false;
                }
                else
                {
                    this.tsbSave.Enabled = true;
                    if (this.objTranGrpFactory.IsNew)
                    {
                        this.tsbClear.Enabled = true;
                        this.tsbDelete.Enabled = false;
                    }
                    else
                    {
                        this.tsbClear.Enabled = false;
                        this.tsbDelete.Enabled = true;
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
        private void ListSelectionSync()
        {
            ListSyncInprogress = true;

            try
            {
                if (objTranGrpFactory.ObjMSTAccTranGrp.TranGrpKey > 0)
                {
                    //When The Header.RecordKey is the same as the List.RecordKey, we do not need to select the related row on the list
                    //this will prevent the list from opening the record again
                    if (taTreeGroup.SelectedNodes != null)
                        if (taTreeGroup.SelectedNodes.Count > 0)
                            if (GFunc.NEInt(taTreeGroup.SelectedNodes[0].Key, 0) == objTranGrpFactory.ObjMSTAccTranGrp.TranGrpKey)
                                return;//already selected.

                    //Select the row on the list to reflect the current value in Header.recordkey                   
                    taTreeGroup.ActiveNode = taTreeGroup.GetNodeByKey(objTranGrpFactory.ObjMSTAccTranGrp.TranGrpKey.ToString());
                    if(taTreeGroup.ActiveNode != null)
                        taTreeGroup.ActiveNode.Selected = true;
                }
                else
                {
                    taTreeGroup.Nodes.Clear();
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
                ListSyncInprogress = false;
            }
        }//Completed

        private bool New_Process(int parentKey,bool SubGroupButtonClicked)
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
                        this.objTranGrpFactory.IsDirty = false;
                    }
                }

                this.errorProvider1.Clear();

                if (this.objTranGrpFactory.IsDirty)
                {
                    if (Save_Process() == false)
                        return false;
                }

                if (this.objTranGrpFactory.New(parentKey) == false)
                {
                    oCurrNode = null;
                    return false;
                }
                else
                {
                    if (SubGroupButtonClicked == false && parentKey == 0)
                    {
                        objTranGrpFactory.ObjMSTAccTranGrp.TranGrpTitle = "Team";
                    }
                    else
                        objTranGrpFactory.ObjMSTAccTranGrp.TranGrpTitle = "Member";
                    this.objTranGrpFactory.IsDirty = false;
                    this.errorProvider1.Clear();                 
                    this.TranGrpID.Focus();
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
                this.Refresh_All();
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

                if (objTranGrpFactory.IsDirty)
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
                if (this.objTranGrpFactory.Save())
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
                this.Refresh_All();
                this.FormLayout();
                Refresh_GridList();
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private bool OpenRecord(int key)
        {
            this.Cursor = Cursors.WaitCursor;
            GEnum.MsgBoxButton btnSelect = GEnum.MsgBoxButton.Yes;

            try
            {
                if (SECPermUtility.Edit(objTranGrpFactory.PermID, false))
                {
                    if (objTranGrpFactory.GetEdit(key,"") == false)
                    {
                        if (SysOptionUtility.GetBool("WarnOpenRecordAsReadOnly"))
                        {
                            btnSelect = MsgBox.Show("Try to open record as read only",
                                                  GEnum.MsgBoxIcon.Question,
                                                  GEnum.MsgBoxButton.Yes,
                                                  GEnum.MsgBoxButton.No);

                            if (btnSelect == GEnum.MsgBoxButton.Yes)
                            {
                                objTranGrpFactory.GetReadOnly(key,"");
                            }
                        }
                    }
                }
                else
                    objTranGrpFactory.GetReadOnly(key,"");

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
                this.Refresh_All();
                this.FormLayout();
                ListSelectionSync();
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

                //Check if it has child nodes
                if (taTreeGroup.SelectedNodes[0].HasNodes == true)
                {
                    MsgBox.Show("You must delete the child records before the parent can be deleted");
                    return false;
                }

                if (this.objTranGrpFactory.Delete())
                {
                    oCurrNode = null;   
                    Refresh_GridList();
                    this.objTranGrpFactory.New(DEF_ParentKey);

                                 
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
                this.Refresh_All();                
                this.FormLayout();
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private bool Clear_Process()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (GFunc.IsNEZ(this.objTranGrpFactory.ObjMSTAccTranGrp.TranGrpKey))
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

                    if (this.objTranGrpFactory.New(DEF_ParentKey))
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
                this.Refresh_All();
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

                if (TAUtil.ControlGVar.FormValidateFail)
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
        //TreeView Events       
        private void taTreeGroup_AfterSelect(object sender, SelectEventArgs e)
        {
            int key = 0;

            try
            {
                //Save Changes
                if (formClose)
                    return;

                if (ListSyncInprogress == false)//disable selection when list synchonisation is in progress
                {
                    if (this.taTreeGroup.ActiveNode != null)
                        key = GFunc.NEInt(taTreeGroup.ActiveNode.Key,0);

                    if (this.SaveChanges() == false)
                    {
                        ListSelectionSync();
                        return;
                    }

                    if (key > 0)
                        this.OpenRecord(key);
                }

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
        private void taTreeGroup_Click(object sender, EventArgs e)
        {
            //// Waiting Cursor
            //this.Cursor = Cursors.WaitCursor;

            //bool isOk = false;

            //try
            //{
            //    //Save Changes
            //    if (formClose)
            //        return;

            //    if (this.taTreeGroup.SelectedNodes.Count > 0)
            //    {
            //        int nTranGrpKey = Convert.ToInt32(this.taTreeGroup.SelectedNodes[0].Key);
            //        if (nTranGrpKey != this.objTranGrpFactory.ObjMSTAccTranGrp.TranGrpKey)
            //        {
            //            isOk = this.SaveChanges();

            //            if ((isOk) && msgID == string.Empty)
            //            {
            //                //I Don't Know
            //                if (this.taTreeGroup.SelectedNodes != null)
            //                    this.taTreeGroup.Nodes[0].Selected = true;
            //                return;
            //            }
            //            else if ((isOk) && msgID != string.Empty)
            //            {
            //                //Validation Fail
            //                if (this.taTreeGroup.SelectedNodes != null)
            //                    this.taTreeGroup.Nodes[0].Selected = true;

            //                this.TranGrpID.Focus();
            //                return;
            //            }
            //            // Call GetEdit
            //            isOk = objTranGrpFactory.GetEdit(nTranGrpKey);

            //            if (!isOk)
            //            {
            //                if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnRestoreRecord))
            //                {
            //                    // Ask Confirmation For ReadOnly
            //                    GEnum.MsgBoxButton btnSelect;
            //                    btnSelect = MsgBox.Show(MsgID.Common.ResponseOpenAsReadOnly,
            //                                          GEnum.MsgBoxButton.Yes,
            //                                          GEnum.MsgBoxButton.No,
            //                                          GEnum.MsgBoxButton.I_Dont_Know,
            //                                          GEnum.MsgBoxIcon.Warning);

            //                    // Yes, i want to use as readonly
            //                    if (btnSelect == GEnum.MsgBoxButton.Yes)
            //                    {
            //                        // Call ReadOnly
            //                        isOk = this.objTranGrpFactory.GetReadOnly(nTranGrpKey);
            //                    }
            //                    // No, i don't want
            //                    else if (btnSelect == GEnum.MsgBoxButton.No)
            //                    {
            //                        // Call Edit
            //                        isOk = objTranGrpFactory.GetEdit((int)this.objTranGrpFactory.ObjMSTAccTranGrp.TranGrpKey);
            //                    }
            //                    // Cancel Process
            //                    else
            //                        return;
            //                }
            //                else
            //                {
            //                    // Call ReadOnly
            //                    isOk = this.objTranGrpFactory.GetReadOnly(nTranGrpKey);
            //                }
            //            }
            //        }

            //        this.Refresh_Header();

            //        // Call ReadOnly
            //        //this.OnReadOnly();
            //    }
            //}
            //catch (TAException tex)
            //{
            //    Error(tex, true);
            //}
            //catch (Exception ex)
            //{
            //    Error(ex, true); // System Msg
            //}
            //finally
            //{
            //    // Default Cursor
            //    this.Cursor = Cursors.Default;
            //}
        }        

        //Error
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim() != "")
            {
                try
                {
                    DataRow[] dr = dtTreeData.Select("TranGrpID LIKE '%" + txtSearch.Text + "%'", "TranGrpID ASC");
                    if (dr != null && dr.Length > 0)
                    {
                        taTreeGroup.ActiveNode = taTreeGroup.GetNodeByKey(dr[0]["TranGrpKey"].ToString());
                        if (taTreeGroup.ActiveNode != null)
                            taTreeGroup.ActiveNode.Selected = true;
                    }
                    else
                    {
                        MsgBox.Show("Input text cannot find in the list!");
                    }
                }
                catch { }               
            }
            else
            {
                MsgBox.Show("There is no input text for searching!");
            }
           
        }
    }
}