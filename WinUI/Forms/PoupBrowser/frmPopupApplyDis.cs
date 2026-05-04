using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using BOLib;
using TAUtil;

namespace WinUI
{
    public partial class frmPopupApplyDis : Form
    {
        //Declaration Variables
        private Document CallerDoc;
        private UltraGrid Callergrd;
        private string ContextMenuSetting = string.Empty;

        //Initialize
        public frmPopupApplyDis()
        {
            InitializeComponent();
        }//Completed
        public frmPopupApplyDis(Document objDoc, ref UltraGrid grd)
        {
            //Call From DocDetUtil
            InitializeComponent();

            this.Callergrd = grd;
            CallerDoc = objDoc;
        }//Completed
    
        //Form Events
        private void frmPopupApplyDis_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)CallerDoc.DocCodeKey);
                GlobalUI.Combos_Fill(this, (int)CallerDoc.DocCodeKey);

                if (LoadData() == false)
                    this.DialogResult = DialogResult.Cancel;
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
        private void frmPopupApplyDis_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)CallerDoc.DocCodeKey);
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
        }//Completed

        //Button Events
        private void btnOK_Click(object sender, EventArgs e)
        {
            //Wating Cursor
            this.Cursor = Cursors.WaitCursor;
            try
            {
                #region Checking
                if (GFunc.IsNEZ(DisAcc.Value))
                {
                    MsgBox.Show("Dicount Account can not be empty.");
                    return;
                }

                if (Callergrd.ActiveRow == null)
                    return;

                decimal vDocLinkCurrRate = GFunc.RndC(GFunc.NEDec(Callergrd.ActiveRow.Cells["LinkDocCurrRate"].Value, 0),GVar.RndDecs.Curpt);
                decimal vDisAmtF = GFunc.RndC(DisAmt.DecimalValue,GVar.RndDecs.Amtpt);

                Callergrd.ActiveRow.Cells["ItmApplyDisAmtF"].Value = vDisAmtF;
                Callergrd.ActiveRow.Cells["ItmApplyDisAccKey"].Value = GFunc.NEInt(DisAcc.Value,0);

                if (GFunc.NEDec(Callergrd.ActiveRow.Cells["ItmApplyDueAmtF"].Value, 0) == vDisAmtF + GFunc.NEDec(Callergrd.ActiveRow.Cells["ItmApplyDocAmtF"].Value, 0))           
                    Callergrd.ActiveRow.Cells["ItmApplyDisAmtH"].Value = GFunc.NEDec(Callergrd.ActiveRow.Cells["ItmApplyDueAmtH"].Value,0) - GFunc.NEDec(Callergrd.ActiveRow.Cells["ItmApplyDocAmtH"].Value,0);
                else
                    Callergrd.ActiveRow.Cells["ItmApplyDisAmtH"].Value = vDisAmtF * vDocLinkCurrRate;

                Callergrd.ActiveRow.Cells["ItmApplyDisAccKey"].Value = GFunc.NEInt(DisAcc.Value,0);

                this.DialogResult = DialogResult.OK;
                #endregion
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
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }//Completed

        //Controls Events
        private void DisAcc_CustomUpdate(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int Key = GFunc.GetExistingRecKey(DisAcc.Text, GEnum.SystemCode.Account, true, true);
                if (GFunc.IsNEZ(Key))
                {
                    DisAcc_EditorButtonClick(sender, null);
                }
                else
                {
                    MSTAcc objAcc = MSTAcc.Get(Key);
                    DisAcc.SetValueTrigger(objAcc.AccKey, false);
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
        }//Completed
        private void DisAcc_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ((Control)sender).Name);

                if (DocHDRUtil.EditorButton_Popup((int)CallerDoc.DocCodeKey, DisAcc.Text, listSettingID, (int)GEnum.PopupType.AccID, ref key, ref id, ref des))
                {
                    DisAcc.SetValueTrigger(key, false);
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
        }//Completed

        //Methods
        private bool LoadData()
        {
            try
            {
                decimal vDiscountAmt, Percent = 0;

                if (Callergrd.ActiveRow == null)
                    return false;

                int vLinDocDK = GFunc.NEInt(Callergrd.ActiveRow.Cells["LinkDocTermKey"].Value, 0);
                if (vLinDocDK == 0)
                    return false;

                REFTerm vRefTerm = REFTerm.Get(vLinDocDK);
                
                //Get Discount Account
                int defDisAcc = GFunc.NEInt(Callergrd.ActiveRow.Cells["ItmApplyDisAccKey"].Value,0);
                if (GFunc.IsNEZ(defDisAcc))
                {
                    if (CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Payment_Issue)
                        defDisAcc = SysOptionUtility.GetInt("AccDiscountReceived");
                    else
                        defDisAcc = SysOptionUtility.GetInt("AccDiscountAllowed");
                }

                if (GFunc.IsNE(vRefTerm) == false)
                {
                    Term.SetValueTrigger(vRefTerm.TermID, false);
                    Rem.SetValueTrigger(vRefTerm.TermDes, false);
                    StandNetDueDay.SetValueTrigger(vRefTerm.StandNetDueDay, false);
                    StandDisDay.SetValueTrigger(vRefTerm.StandDisDay,false);

                    Percent = GFunc.RndC(GFunc.NEDec(vRefTerm.StandDisPercent, 0), GVar.RndDecs.Amtpt);
                    StandDisPercent.SetValueTrigger(Percent, false);

                    DateNetDueDay.SetValueTrigger(vRefTerm.DateNetDueDay, false);
                    DateDueDayNextMth.SetValueTrigger(vRefTerm.DateDueDayNextMth, false);
                    DateDisDay.SetValueTrigger(vRefTerm.DateDisDay, false);
                    DateDisPercent.SetValueTrigger(vRefTerm.DateDisPercent, false);

                    decimal vAmt = GFunc.NEDec(Callergrd.ActiveRow.Cells["ItmApplyDueAmtF"].Value, 0);
                    vDiscountAmt = GFunc.RndC(vAmt * Percent, GVar.RndDecs.Amtpt);
                    DisAmt.SetValueTrigger(vDiscountAmt, false);
                    DisAcc.SetValueTrigger(defDisAcc, false);

                    return true;
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
                this.Cursor = Cursors.Default;
            }
        }//Completed

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

    }
}
