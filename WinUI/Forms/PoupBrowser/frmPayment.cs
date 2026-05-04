using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOLib;
using Infragistics.Win.UltraWinEditors;
using TAUtil;

namespace WinUI
{
    public partial class frmPayment : Form
    {
        //Local Variable
        private Document doc = null;
        string ContextMenuSetting = string.Empty;

        //Initialize
        public frmPayment()
        {
            InitializeComponent();
        }//Completed
        public frmPayment(Document objDoc)
        {
            InitializeComponent();
            doc = objDoc;
        }//Completed

        //Form Events
        private void frmPayment_Load(object sender, EventArgs e)
        {
            decimal? docApplyAmtF = 0;
            try
            {
                //Use values from system option when current value is null or empty or zero for each property
                if (GFunc.IsNE(GFunc.GetPropertyValue("DocPaidAccKey", doc)) == false)
                    GLAccount.SetValueTrigger(GFunc.NEInt(GFunc.GetPropertyValue("DocPaidAccKey", doc),0),false);
                else
                    GLAccount.SetValueTrigger(SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultARIVPaymentAcc), false);

                if (GFunc.IsNE(GFunc.GetPropertyValue("DocPaidModeKey", doc)) == false)
                    PaymentMode.SetValueTrigger(GFunc.NEInt(GFunc.GetPropertyValue("DocPaidModeKey", doc), 0), false);
                else
                    PaymentMode.SetValueTrigger(SysOptionUtility.GetInt(GVar.SystemOption.OpID.DefaultARIVPaymentMode), false);

                docApplyAmtF = GFunc.GetDecimalPropertyValue("DocApplyAmtF", doc);

                //When the Invoice has been applied, this form is readonly
                if (docApplyAmtF != 0 || doc.IsReadOnly)
                {                                         
                    GLAccount.Enabled=false;
                    PaymentMode.Enabled=false;
                    ChequeNo.Enabled=false;
                    ChequeDate.Enabled=false;
                    ChequeBank.Enabled=false;
                    Amount.Enabled=false;
                    DocPaidRef.Enabled=false;
                    DocPaidDes.Enabled=false;
                    btnOK.Enabled=false;
                    btnDelete.Enabled = false;
                }

                GlobalUI.FormGrids_Set(this, GFunc.NEInt(doc.DocCodeKey, 0), out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(GFunc.NEInt(doc.DocCodeKey, 0));
                GlobalUI.Combos_Fill(this, GFunc.NEInt(doc.DocCodeKey, 0));
                CombosDependent_Fill(string.Empty);
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
        private void frmPayment_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, GFunc.NEInt(doc.DocCodeKey, 0));
                    CombosDependent_Fill(string.Empty);
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

        //Button Click Events
        private void btnOK_Click(object sender, EventArgs e)
        {
            decimal? DocGrand= GFunc.GetDecimalPropertyValue("DocGrand", doc);

            try
            {
                if (Validation())
                {
                    GFunc.SetPropertyValue("DocPaidDate", doc, ChequeDate.DateValue);
                    GFunc.SetPropertyValue("DocPaidAccKey", doc, GLAccount.Value);
                    GFunc.SetPropertyValue("DocPaidModeKey", doc, PaymentMode.Value);
                    GFunc.SetPropertyValue("DocPaidChqNum", doc, ChequeNo.Text);
                    GFunc.SetPropertyValue("DocPaidAmtF", doc,GFunc.NEDec(Amount.Text,0));
                    GFunc.SetPropertyValue("DocPaidBankKey", doc, ChequeBank.Value);
                    GFunc.SetPropertyValue("DocPaidRef", doc, DocPaidRef.Text);
                    GFunc.SetPropertyValue("DocPaidDes", doc, DocPaidDes.Text);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
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
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }//Completed
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                GFunc.SetPropertyValue("DocPaidDate", doc, null);
                GFunc.SetPropertyValue("DocPaidAccKey", doc, null);
                GFunc.SetPropertyValue("DocPaidModeKey", doc, null);
                GFunc.SetPropertyValue("DocPaidChqNum", doc, null);
                GFunc.SetPropertyValue("DocPaidRef", doc, null);
                GFunc.SetPropertyValue("DocPaidDes", doc, null);
                GFunc.SetPropertyValue("DocPaidAmtF", doc, 0M);
                GFunc.SetPropertyValue("DocPaidBankKey", doc, null);
                this.DialogResult = DialogResult.OK;
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
        private void GLAccount_CustomUpdate(object sender, CancelEventArgs e)
        {
            //Cursor = Cursors.WaitCursor;
            //try
            //{
            //    int itmKey = GFunc.GetExistingRecKey(GLAccount.Text, GEnum.SystemCode.Account, true, true);
            //    if (GFunc.IsNEZ(itmKey))
            //    {
            //        GLAccount_EditorButtonClick(sender, null);
            //    }
            //    else
            //    {
            //        MSTAcc objItm = MSTAcc.Get(itmKey);
            //        GLAccount.SetValueTrigger(objItm.AccID, false);
            //    }

            //}
            //catch (TAException tex)
            //{
            //    Error(tex, true);
            //}
            //catch (Exception ex)
            //{
            //    Error(ex, true);
            //}
            //finally
            //{
            //    this.Cursor = Cursors.Default;
            //}

        }
        private void GLAccount_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int itmKey = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ((Control)sender).Name);

                if (DocHDRUtil.EditorButton_Popup((int)doc.DocCodeKey, GLAccount.Text, listSettingID, (int)GEnum.PopupType.AccID, ref itmKey, ref id, ref des))
                {
                    GLAccount.SetValueTrigger(id, false);
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

        //Functions
        private bool Validation()
        {
            string msgID = string.Empty;

            if (!BaseUtility.Validation(out msgID, Amount.Text, "Amount", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null))
            {
                MsgBox.Show(msgID);
                return false;
            }
            if (!BaseUtility.Validation(out msgID, ChequeDate.DateValue, "ChequeDate", GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null))
            {
                MsgBox.Show(msgID);
                return false;
            }
            if (!BaseUtility.Validation(out msgID, ChequeNo.Value, "ChequeNo", GEnum.DataType.String, GEnum.Require.No, null, null, null, null, null))
            {
                MsgBox.Show(msgID);
                return false;
            }
            if (!BaseUtility.Validation(out msgID, ChequeBank.Value, "ChequeBank", GEnum.DataType.String, GEnum.Require.No, null, null, null, null, null))
            {
                MsgBox.Show(msgID);
                return false;
            }
            if (!BaseUtility.Validation(out msgID, PaymentMode.Value, "PaymentMode", GEnum.DataType.String, GEnum.Require.No, null, null, null, null, null))
            {
                MsgBox.Show(msgID);
                return false;
            }
            if (!BaseUtility.Validation(out msgID, GLAccount.Value, "GLAccount", GEnum.DataType.String, GEnum.Require.Yes, null, null, null, null, null))
            {
                MsgBox.Show(msgID);
                return false;
            }

            return true;
        }//Completed
        private void CombosDependent_Fill(string controlNm)
        {
            try
            {
                if (controlNm == "GLAccount" || controlNm == string.Empty)
                    GlobalUI.BindComboValue((TAUtil.TAComboBox)GLAccount, GVar.ListSettingID.MSTAccByCurr_id + "%" + GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey",doc),0)+"%%"+ AppInfor.CurrentUserKey +"%"+ AppInfor.ItemAccessLevel );
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

        //Function has NOT been match with TBS  --------------------------PLEASE CREATE NEW FUNCTION BELOW THIS LINE-----------------------------------------------
        
    }
}
