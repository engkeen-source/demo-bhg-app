using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinTree;
using System.Data.SqlClient;
using BOLib;
using TAUtil;

namespace WinUI
{
    public partial class frmPopupTreeView : Form
    {
        #region Local Variables & Properties
        private BOLib.MSTAccTranGrpFactory objTranGrpFactory = null;
        private string msgID = string.Empty;
        private DataTable dtTreeData = null;
        private int _tranGrpKey = 0;
        private string _tranGrpID = string.Empty;
        private bool formClose = false;
        string ContextMenuSetting = string.Empty;

        public string TranGrpID
        {
            get
            {
                return _tranGrpID;
            }
            set
            {
                _tranGrpID = value;
            }
        }
        public int TranGrpKey
        {
            get
            {
                return _tranGrpKey;
            }
            set
            {
                _tranGrpKey = value;
            }
        }
        #endregion

        // Initialize
        public frmPopupTreeView()
        {
            InitializeComponent();
        }

        //Form Events
        private void frmPopupTreeView_Load(object sender, EventArgs e)
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // Call Initialization
                InitializeUI();
                this.objTranGrpFactory = new BOLib.MSTAccTranGrpFactory(BOLib.GEnum.InstanceMode.Normal);

                GetTranGrpList();
                if (_tranGrpID != string.Empty)
                {
                    this.TranGroupID.Text = _tranGrpID;
                    this.tatreeTranGroup.ActiveNode = tatreeTranGroup.GetNodeByKey(_tranGrpID);
                    this.tatreeTranGroup.ActiveNode.Selected = true;
                }
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0);
                GlobalUI.Combos_Fill(this, 0);
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
                    MsgBox.Show(tex.MsgID); // Custom Msg
                    this.formClose = true;
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message); // System Msg   
            }
            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }
        private void frmPopupTreeView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }

            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // Call Dispose
                bool isOk = this.objTranGrpFactory.Dispose();

                //When the form is closed by main form, to proceed closing 
                frmMain.gfrmMain.Tag = string.Empty;
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
                    MsgBox.Show(tex.MsgID); // Custom Msg
                    this.formClose = true;
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message); // System Msg 
            }
            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }

        //Tree Display - Controlling and format        
        private void InitializeUI()
        {
            this.tatreeTranGroup.DisplayStyle = UltraTreeDisplayStyle.WindowsVista;
            GetTranGrpList();
        }
        private bool GetTranGrpList()
        {
            this.tatreeTranGroup.Nodes.Clear();

            try
            {
                dtTreeData = MASList.GetMSTAccTranGrps(0, null);

                if (msgID == string.Empty)
                {                   
                    UltraTreeNode parent = null;
                    BuildTreeView("0", parent);
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
            return false;
        }
        private void Select_Process()
        {
            this.tatreeTranGroup.ActiveNode = tatreeTranGroup.GetNodeByKey(TranGroupID.Text);
            _tranGrpKey = Convert.ToInt32(this.tatreeTranGroup.ActiveNode.Tag);
            this.tatreeTranGroup.ActiveNode.Selected = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        //Menu Strip Event
        private void tsbSelect_Click(object sender, EventArgs e)
        {
            if (TranGroupID.Text == string.Empty)
                TranGroupID.Text = this.tatreeTranGroup.SelectedNodes[0].Text;
            this.Select_Process();
        }
        private void tsbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Control Events
        private void TranGroupID_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)Keys.Tab || e.KeyChar == (char)Keys.Enter)
            //{
            //    this.tatreeTranGroup.ActiveNode = tatreeTranGroup.GetNodeByKey(TranGroupID.Text);
            //    _tranGrpKey = Convert.ToInt32(this.tatreeTranGroup.ActiveNode.Tag);
            //    this.tatreeTranGroup.ActiveNode.Selected = true;
            //}
        }

        //Tree Events
        private void tatreeTranGroup_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                TranGroupID.Text = this.tatreeTranGroup.SelectedNodes[0].Text;
                this.Select_Process();
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
        private void tatreeTranGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (tatreeTranGroup.SelectedNodes.Count > 0)
                    TranGroupID.Text = this.tatreeTranGroup.SelectedNodes[0].Text;
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
        private void BuildTreeView(string strParent, UltraTreeNode oParentNode)
        {
            try
            {
                IEnumerable<DataRow> dtRows = dtTreeData.AsEnumerable().Where(r => r.Field<int>("TranGrpKeyParent")== GFunc.NEInt(strParent,0));
                if (dtRows != null)
                {
                    foreach (DataRow Row in dtRows)
                    {
                        if (oParentNode == null)
                        {
                            UltraTreeNode oNode = new UltraTreeNode(Row["TranGrpID"].ToString());
                            oNode.Key = Row["TranGrpID"].ToString();
                            oNode.Tag = Row["TranGrpKey"].ToString();
                            this.tatreeTranGroup.Nodes.Add(oNode);
                            this.BuildTreeView(Row["TranGrpKey"].ToString(), oNode);
                        }
                        else
                        {
                            UltraTreeNode oNode = new UltraTreeNode(Row["TranGrpID"].ToString());
                            oNode.Key = Row["TranGrpID"].ToString();
                            oNode.Tag = Row["TranGrpKey"].ToString();
                            oParentNode.Nodes.Add(oNode);
                            this.BuildTreeView(Row["TranGrpKey"].ToString(), oNode);
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


        #region Error

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
                    l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, GFunc.GridColumnKey_Get(this.ActiveControl) });
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
                if (this.ActiveControl.GetType() != typeof(TAUtil.TAGridEditor))
                {
                    l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl });
                }
                else
                {
                    l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, GFunc.GridColumnKey_Get(this.ActiveControl) });
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

        private void TranGroupID_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyValue == (char)Keys.Tab || e.KeyValue == (char)Keys.Enter)
                {
                    this.tatreeTranGroup.ActiveNode = tatreeTranGroup.GetNodeByKey(TranGroupID.Text);
                    _tranGrpKey = Convert.ToInt32(this.tatreeTranGroup.ActiveNode.Tag);
                    this.tatreeTranGroup.ActiveNode.Selected = true;
                    tsbSelect_Click(sender, e);
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

        //private void frmPopupTreeView_KeyDown(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        if (this.ActiveControl.Name == "TranGroupID")
        //        {
        //            if (e.KeyValue == (char)Keys.Tab || e.KeyValue == (char)Keys.Enter)
        //            {
        //                this.tatreeTranGroup.ActiveNode = tatreeTranGroup.GetNodeByKey(TranGroupID.Text);
        //                _tranGrpKey = Convert.ToInt32(this.tatreeTranGroup.ActiveNode.Tag);
        //                this.tatreeTranGroup.ActiveNode.Selected = true;
        //            }

        //        }
        //        else
        //        {
        //            GlobalUI.SelectNextControl(this, e);
        //        }

        //    }
        //    catch (TAException tex)
        //    {
        //        Error(tex, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        Error(ex, true);
        //    }
        //}
    }
}
