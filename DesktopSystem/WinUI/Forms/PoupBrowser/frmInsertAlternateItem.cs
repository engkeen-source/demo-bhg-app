using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOLib;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win;
using TAUtil;

namespace WinUI
{
    public partial class frmInsertAlternateItem : Form
    {
        #region Local Variable
        private string ContextMenuSetting = string.Empty;
        private UltraGrid tagrdDetItms = null;
        Document objDoc = null;
        int ItmKey = 0;
        Hashtable htDetailGrd = new Hashtable();
        #endregion

        //Initialize
        public frmInsertAlternateItem()
        {
            InitializeComponent();
        }//Completed
        public frmInsertAlternateItem(Document parmObjDoc, UltraGrid tagrdDetItms)
        {
            InitializeComponent();
            this.tagrdDetItms = tagrdDetItms;                
            objDoc = parmObjDoc;
            ItmKey = GFunc.NEInt(tagrdDetItms.ActiveRow.Cells["ItmKey"].Value,0); 
        }//Completed
        
        //From Events
        private void frmInsertAlternateItem_Load(object sender, EventArgs e)
        {
            try
            {
            
                GlobalUI.FormGrids_Set(this, (int)objDoc.DocCodeKey, out ContextMenuSetting);
                GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objDoc.DocCodeKey);
                //GlobalUI.Combos_Fill(this, (int)objDoc.DocCodeKey);
                GlobalUI.GridAllColumnsActivateOnlySet(tagrdInsertAltItem);

                tagrdInsertAltItem.DisplayLayout.Bands[0].Override.AllowColSizing = AllowColSizing.None;
                tagrdInsertAltItem.DisplayLayout.Bands[0].Columns.Add("Select");
                tagrdInsertAltItem.DisplayLayout.Bands[0].Columns["Select"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                tagrdInsertAltItem.DisplayLayout.Bands[0].Columns["Select"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                tagrdInsertAltItem.DisplayLayout.Bands[0].Columns["Select"].CellButtonAppearance.BackColor = Color.Transparent; //System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
                tagrdInsertAltItem.DisplayLayout.Bands[0].Columns["Select"].CellButtonAppearance.Image = global::WinUI.Properties.Resources.ok_16;
                
                tagrdInsertAltItem.DisplayLayout.Bands[0].Columns["Select"].Header.Caption = "";
                tagrdInsertAltItem.DisplayLayout.Bands[0].Columns["Select"].PerformAutoResize();
               // tagrdInsertAltItem.DisplayLayout.Bands[0].Columns["Select"]
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
        
        //Grid Events
        private void tagrdInsertAltItem_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {

                htDetailGrd.Clear();
                htDetailGrd.Add(GEnum.Details.Doc_Itm, tagrdDetItms);

                int ItmAltKey = GFunc.NEInt(this.tagrdInsertAltItem.ActiveRow.Cells["AltItmKey"].Value, 0);
                DocDetUtil.ItmID_Update(objDoc, htDetailGrd, ItmAltKey);

                this.Close();
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

        //Custom Error Functions
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
