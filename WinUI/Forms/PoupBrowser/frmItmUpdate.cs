using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOLib;
using TAUtil;
using Infragistics.Win.UltraWinGrid;

namespace WinUI
{
    public partial class frmItmUpdate : Form
    {
        #region Local Variables        
        private string ContextMenuSetting = string.Empty;
        #endregion

        public frmItmUpdate(DataTable dtItem)
        {
            InitializeComponent();
            tagrdItem.DataSource = dtItem;
        }

        private void frmItmUpdate_Load(object sender, EventArgs e)
        {
            try
            {
                //Set ContextMenu & Grid Setting                  
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0,this.Name);
                LockGridColumns();//Except ItmID
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

        private void LockGridColumns()
        {
            foreach (UltraGridColumn col in tagrdItem.DisplayLayout.Bands[0].Columns)
            {
                if (col.Key != "ItmID")
                    col.CellActivation = Activation.ActivateOnly;
            }
        }

        private void tagrdItm_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                if (GFunc.CompareString(e.Cell.Column.Key, "ItmID"))
                {
                    string listSetingID = "MSTItmAll_id";
                    int key = 0;
                    string id = string.Empty;
                    string des = string.Empty;
                    //Only if Item is Stock (Not include Assembly) , Non-Stock and Service allow to change the ID
                    MSTItm itmTmp = MSTItm.Get(e.Cell.Text);
                    if ((GFunc.GetINTypeGroup(itmTmp.ItmType) == (int)GEnum.INTypeGrp.Stock || GFunc.GetINTypeGroup(itmTmp.ItmType) == (int)GEnum.INTypeGrp.Stock) && itmTmp.ItmType != (int)GEnum.ItemType.Assembly)
                    {
                        if (DocHDRUtil.EditorButton_Popup(0, e.Cell.Text, listSetingID, (int)GEnum.PopupType.ItmID, ref key, ref id, ref des))
                        {
                            e.Cell.Value = id;
                        }
                    }
                    else
                        MsgBox.Show("Only Stock(except assemby) and Non-Stock items is allowed to be changed");
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
                
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
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

    }
}
