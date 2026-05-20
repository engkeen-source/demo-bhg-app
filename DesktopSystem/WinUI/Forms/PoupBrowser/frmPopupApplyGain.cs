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
using BOLib;
using System.Collections;
using Infragistics.Win.UltraWinEditors;
using TAUtil;

namespace WinUI
{
    public partial class frmPopupApplyGain : Form
    {
        //Declaration
        private UltraGridRow CallergrdRow;
        string ContextMenuSetting = string.Empty;
        private Document CallerDoc;
        private bool GainMode = false;
        private decimal ItmApplyPayCurrRate = 1M;
        private string HomeCurrID = string.Empty;

        //Initilize
        public frmPopupApplyGain()
        {
            InitializeComponent();
        }//Completed
        public frmPopupApplyGain(Document objDoc, ref UltraGrid grd)
        {
            //Call From DocDetUtil
            //ItmApplyDocAmtF custom update

            InitializeComponent();
            if(grd.ActiveRow == null)
                return;

            CallergrdRow = grd.ActiveRow;
            CallerDoc = objDoc;

            if (objDoc.DocCodeKey != (int)GEnum.SystemCode.Payment_Issue)
            {
                GainMode = true;            
            }

        }//Completed

        //Form Events
        private void frm_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            REFCurr objREFCurr = null;
            int defaultGainLossAccKey = 0;
            int paymentCurrKey = 0;

            try
            {
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)CallerDoc.DocCodeKey);
                GlobalUI.Combos_Fill(this, (int)CallerDoc.DocCodeKey);


                #region set values to control
                //Get Home Currency ID
                objREFCurr = REFCurr.Get(1);
                HomeCurrID = objREFCurr.CurrID;

                //Set document Section
                objREFCurr = REFCurr.Get((int)CallergrdRow.Cells["LinkDocCurrKey"].Value);
                LinkDocID.SetValueTrigger((string)CallergrdRow.Cells["LinkDocID"].Value, false);
                LinkDocCurrIDDis.SetValueTrigger(objREFCurr.CurrID,false);
                LinkDocCurrID.SetValueTrigger(objREFCurr.CurrID,false);
                LinkDocCurrRate.SetValueTrigger(GFunc.RndC((decimal)CallergrdRow.Cells["LinkDocCurrRate"].Value,GVar.RndDecs.Curpt),false);
                
                ItmApplyDueAmtF.SetValueTrigger(GFunc.RndC((decimal)CallergrdRow.Cells["ItmApplyDueAmtF"].Value,GVar.RndDecs.Amtpt),false);
                ItmApplyDueAmtH.SetValueTrigger(GFunc.RndC((decimal)CallergrdRow.Cells["ItmApplyDueAmtH"].Value,GVar.RndDecs.Amtpt), false);
                ItmApplyDisAmtF.SetValueTrigger(GFunc.RndC((decimal)CallergrdRow.Cells["ItmApplyDisAmtF"].Value,GVar.RndDecs.Amtpt),false);
                ItmApplyDisAmtH.SetValueTrigger(GFunc.RndC((decimal)CallergrdRow.Cells["ItmApplyDisAmtH"].Value,GVar.RndDecs.Amtpt), false);
             
                //Set Payment Section
                ItmApplyRate.SetValueTrigger(GFunc.RndC((decimal)CallergrdRow.Cells["ItmApplyRate"].Value,GVar.RndDecs.Curpt),false);
                ItmApplyPayAmtF.SetValueTrigger(GFunc.RndC((decimal)CallergrdRow.Cells["ItmApplyPayAmtF"].Value,GVar.RndDecs.Amtpt), false);
                ItmApplyPayAmtH.SetValueTrigger(GFunc.RndC((decimal)CallergrdRow.Cells["ItmApplyPayAmtH"].Value,GVar.RndDecs.Amtpt),false);
                ItmApplyGainAmt.SetValueTrigger(GFunc.RndC((decimal)CallergrdRow.Cells["ItmApplyGainAmt"].Value, GVar.RndDecs.Amtpt), false);

                paymentCurrKey = (int)GFunc.GetIntPropertyValue("DocCurrKey", CallerDoc);
                objREFCurr = REFCurr.Get(paymentCurrKey);
                ItmApplyGainCurrID.SetValueTrigger(HomeCurrID, false);
                ItmApplyPayCurrID.SetValueTrigger(objREFCurr.CurrID,false);
                ItmApplyPayCurrRate = GFunc.RndC(GFunc.GetDecimalPropertyValue("DocCurrRate", CallerDoc), GVar.RndDecs.Curpt);
                
                ItmApplyGainAccKey.SetValueTrigger(CallergrdRow.Cells["ItmApplyGainAccKey"].Value,false);
                ItmApplyGainAccDes.SetValueTrigger(CallergrdRow.Cells["ItmApplyGainAccDes"].Value, false);

               
                #endregion

                #region get default gain account
                MSTAcc objMSTAcc = null;
                if (GFunc.IsNEZ(ItmApplyGainAccKey.Value))
                { 
                    if (GainMode)
                        defaultGainLossAccKey = GFunc.NEInt(SysOptionUtility.GetInt("AccExchangeGain"), 0);
                    else
                        defaultGainLossAccKey = GFunc.NEInt(SysOptionUtility.GetInt("AccExchangeLoss"), 0);

                    objMSTAcc = MSTAcc.Get(defaultGainLossAccKey);

                    ItmApplyGainAccKey.SetValueTrigger(defaultGainLossAccKey, false);
                    ItmApplyGainAccKey.SetValueTrigger(objMSTAcc.AccKey, false);
                    ItmApplyGainAccDes.SetValueTrigger(objMSTAcc.AccDes, false);
                }
                #endregion

                #region Set ApplyRate
                if ((int)CallergrdRow.Cells["LinkDocCurrKey"].Value == paymentCurrKey ) 
                {
                    if (GFunc.NEDec(ItmApplyPayAmtF.Value,0) == 0M)
                        ItmApplyRate.SetValueTrigger(1,false); 

                    ItmApplyRate.Enabled = false;
                    ItmApplyPayAmtF.Enabled = false;
                }
                else
                {
                    if (GFunc.NEDec(ItmApplyPayAmtF.Value, 0) == 0M)
                        ItmApplyRate.SetValueTrigger(GFunc.RndDC(LinkDocCurrRate.DecimalValue, ItmApplyPayCurrRate, GVar.RndDecs.Curpt),false); 
                }
                #endregion

                ItmApplyDocAmtF.SetValueTrigger(GFunc.RndC((decimal)CallergrdRow.Cells["ItmApplyDocAmtF"].Value, GVar.RndDecs.Amtpt), false);
                ItmApplyDocAmtH.SetValueTrigger(GFunc.RndC((decimal)CallergrdRow.Cells["ItmApplyDocAmtH"].Value, GVar.RndDecs.Amtpt), false);

                utlGainLossAmt.Text = (GainMode) ? "Gain Amount" : "Loss Amount";
                utlGainLossAcc.Text = (GainMode) ? "Gain Account" : "Loss Account";

                ItmApplyDocAmtH.Enabled = false;
                DocAmtF_Update();
                
                //Below code has been disable as it will replace the previously save values in the payment detail row (Mic)
                //to be remove in future if no error is reported for this pop up form. 1-Nov-2013
                //ItmApplyDocAmtF.SetValueTrigger(GFunc.RndC((decimal)CallergrdRow.Cells["ItmApplyDocAmtF"].Value, GVar.RndDecs.Amtpt), false);
                //ItmApplyDocAmtH.SetValueTrigger(GFunc.RndC((decimal)CallergrdRow.Cells["ItmApplyDocAmtH"].Value, GVar.RndDecs.Amtpt), false);


                if (CallerDoc.IsDirty)
                    ItmApplyPayAmtH.Enabled = false;
                else
                    ItmApplyPayAmtH.Enabled = true;

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
        private void frmPopupApplyGain_KeyDown(object sender, KeyEventArgs e)
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
                if(GFunc.NEDec(ItmApplyDueAmtF.Value,0) >= 0) // when DueAmt is (+)ve Value
                    if (GFunc.IsBetweenDec(GFunc.NEDec(ItmApplyDisAmtF.Value,0) + GFunc.NEDec(ItmApplyDocAmtF.Value,0), 0, GFunc.NEDec(ItmApplyDueAmtF.Value,0)) == false)
                    {
                        MsgBox.Show("Exceeded Amount Due");
                        return;
                    }
               else // when DueAmt is (-)ve Value
                    if (GFunc.IsBetweenDec(GFunc.NEDec(ItmApplyDisAmtF.Value, 0) + GFunc.NEDec(ItmApplyDocAmtF.Value, 0), -0, GFunc.NEDec(ItmApplyDueAmtF.Value, 0)) == false)
                    {
                        MsgBox.Show("Exceeded Amount Due");
                        return;
                    }

                if (GFunc.NEDec(ItmApplyDueAmtH.Value, 0) >= 0)// when DueAmt is (+)ve Value
                    if (GFunc.IsBetweenDec(GFunc.NEDec(ItmApplyDisAmtH.Value,0) + GFunc.NEDec(ItmApplyDocAmtH.Value,0), 0, GFunc.NEDec(ItmApplyDueAmtH.Value,0)) == false)
                    {
                        MsgBox.Show("Exceeded Amount Due in Home Currency");
                        return;
                    }
                else // when DueAmt is (-)ve Value
                    if (GFunc.IsBetweenDec(GFunc.NEDec(ItmApplyDisAmtH.Value,0) + GFunc.NEDec(ItmApplyDocAmtH.Value,0), -0, GFunc.NEDec(ItmApplyDueAmtH.Value,0)) == false)
                    {
                        MsgBox.Show("Exceeded Amount Due in Home Currency");
                        return;
                    }

                if (GFunc.NEDec(ItmApplyGainAmt.Value,0) !=0)
                {
                    if (GFunc.NEStr(ItmApplyGainAccKey.Value,string.Empty)==string.Empty)
                    {
                        MsgBox.Show("Gain Account cannot be empty");
                        return;
                    }
                }

                if ((GFunc.NEDec(ItmApplyDocAmtF.Value,0) == 0) && (GFunc.NEDec(ItmApplyPayAmtF.Value,0) != 0))
                {
                    MsgBox.Show("Document amount to applied cannot be zero");
                    return;
                }
                if ((GFunc.NEDec(ItmApplyPayAmtF.Value,0) == 0) && (GFunc.NEDec(ItmApplyDocAmtF.Value,0) != 0))
                {
                    MsgBox.Show("Payment amount to applied cannot be zero");
                    return;
                }
                #endregion

                //Set Value to grid
                CallergrdRow.Cells["ItmApplyDocAmtF"].Value = ItmApplyDocAmtF.Value;
                CallergrdRow.Cells["ItmApplyDocAmtH"].Value = ItmApplyDocAmtH.Value;
                CallergrdRow.Cells["ItmApplyPayAmtF"].Value = ItmApplyPayAmtF.Value;
                CallergrdRow.Cells["ItmApplyPayAmtH"].Value = ItmApplyPayAmtH.Value;
                CallergrdRow.Cells["ItmApplyGainAmt"].Value = ItmApplyGainAmt.Value;
                CallergrdRow.Cells["ItmApplyRate"].Value = ItmApplyRate.Value;
                CallergrdRow.Cells["ItmApplyGainAccKey"].Value = ItmApplyGainAccKey.Value;
                CallergrdRow.Cells["ItmApplyGainAccID"].Value = ItmApplyGainAccKey.Text;
                CallergrdRow.Cells["ItmApplyGainAccDes"].Value = ItmApplyGainAccDes.Value;

                if (GFunc.NEDec(CallergrdRow.Cells["ItmApplyDueAmtF"].Value, 0) == GFunc.NEDec(CallergrdRow.Cells["ItmApplyDocAmtF"].Value, 0) + GFunc.NEDec(CallergrdRow.Cells["ItmApplyDisAmtF"].Value, 0))
                    CallergrdRow.Cells["ItmApplyFull"].Value = 1;
                else
                    CallergrdRow.Cells["ItmApplyFull"].Value = 0;

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
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }//Completed
        private void btnItmApplyFull_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                decimal DueAmtF = ItmApplyDueAmtF.DecimalValue;
                decimal DisAmtF = ItmApplyDisAmtF.DecimalValue;
                decimal DueAmtH = ItmApplyDueAmtH.DecimalValue;
                decimal DisAmtH = ItmApplyDisAmtH.DecimalValue;
                ItmApplyDocAmtF.SetValueTrigger(DueAmtF - DisAmtF, false);
                ItmApplyDocAmtH.SetValueTrigger(DueAmtH - DisAmtH, false);
                DocAmtF_Update();
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

        //Controls Events
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
        private void ItmApplyGainAccKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int Key = GFunc.NEInt(ItmApplyGainAccKey.Value,0);
                if (GFunc.IsNEZ(Key))
                {
                    ItmApplyGainAccKey.SetValueTrigger(null, false);
                    ItmApplyGainAccDes.SetValueTrigger(string.Empty, false);
                }
                else
                {
                    MSTAcc objAcc = MSTAcc.Get(Key);
                    ItmApplyGainAccKey.SetValueTrigger(objAcc.AccKey, false);
                    ItmApplyGainAccDes.SetValueTrigger(objAcc.AccDes, false);
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
        private void ItmApplyGainAccKey_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ((Control)sender).Name);

                if (DocHDRUtil.EditorButton_Popup(0, ItmApplyGainAccKey.Text, listSettingID, (int)GEnum.PopupType.AccID, ref key, ref id, ref des))
                {
                    ItmApplyGainAccKey.SetValueTrigger(key, false);
                    ItmApplyGainAccDes.SetValueTrigger(des, false);
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
        private void ItmApplyGainAccDes_CustomUpdate(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int Key = GFunc.GetExistingRecKey(ItmApplyGainAccDes.Text, GEnum.SystemCode.Account, false, true);
                if (GFunc.IsNEZ(Key))
                {
                    ItmApplyGainAccDes_EditorButtonClick(null, null);
                }
                else
                {
                    MSTAcc objAcc = MSTAcc.Get(Key);
                    ItmApplyGainAccKey.SetValueTrigger(objAcc.AccKey, false);
                    ItmApplyGainAccDes.SetValueTrigger(objAcc.AccDes, false);
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
        private void ItmApplyGainAccDes_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int Key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItmApplyGainAccKey");

                if (DocHDRUtil.EditorButton_Popup(0, ItmApplyGainAccDes.Text, listSettingID, (int)GEnum.PopupType.AccDes, ref Key, ref id, ref des))
                {
                    ItmApplyGainAccKey.SetValueTrigger(Key, false);
                    ItmApplyGainAccDes.SetValueTrigger(des, false);
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
        private void ItmApplyDocAmtF_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (DocAmtF_Update() == false)
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
        private void ItmApplyPayAmtF_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                DocAmtF_Update();

                //#region declaration
                //decimal dueF = 0;
                //decimal dueH = 0;
                //decimal disF = 0;
                //decimal disH = 0;
                //decimal docF = 0;
                //decimal docH = 0;
                //decimal payF = 0;
                //decimal payH = 0;
                //decimal exrate = 1;
                //decimal docRate = 1;
                //decimal payRate = 1;
                //#endregion

                //#region set variables
                //dueF = (decimal)GFunc.RndC(GFunc.NEDec(ItmApplyDueAmtF.Value, 0), GVar.RndDecs.Amtpt);
                //dueH = (decimal)GFunc.RndC(GFunc.NEDec(ItmApplyDueAmtH.Value, 0), GVar.RndDecs.Amtpt);
                //disF = (decimal)GFunc.RndC(GFunc.NEDec(ItmApplyDisAmtF.Value, 0), GVar.RndDecs.Amtpt);
                //disH = (decimal)GFunc.RndC(GFunc.NEDec(ItmApplyDisAmtH.Value, 0), GVar.RndDecs.Amtpt);
                //docF = (decimal)GFunc.RndC(GFunc.NEDec(ItmApplyDocAmtF.Value, 0), GVar.RndDecs.Amtpt);
                //docH = (decimal)GFunc.RndC(GFunc.NEDec(ItmApplyDocAmtH.Value, 0), GVar.RndDecs.Amtpt);
                //payF = (decimal)GFunc.RndC(GFunc.NEDec(ItmApplyPayAmtF.Value, 0), GVar.RndDecs.Amtpt);
                //payH = (decimal)GFunc.RndC(GFunc.NEDec(ItmApplyPayAmtH.Value, 0), GVar.RndDecs.Amtpt);
                //exrate = (decimal)GFunc.RndC(GFunc.NEDec(ItmApplyRate.Value, 1), GVar.RndDecs.Curpt);
                //docRate = (decimal)GFunc.RndC(GFunc.NEDec(LinkDocCurrRate.Value, 0), GVar.RndDecs.Curpt);
                //payRate = (decimal)GFunc.RndC(GFunc.NEDec(ItmApplyPayCurrRate, 0), GVar.RndDecs.Curpt);
                //#endregion
                
                //if (payF == 0 || docF == 0)
                //{
                //    ItmApplyDocAmtF.SetValueTrigger(0, false);
                //    ItmApplyDocAmtH.SetValueTrigger(0, false);
                //    ItmApplyRate.SetValueTrigger(1, false);
                //    ItmApplyPayAmtF.SetValueTrigger(0, false);
                //    ItmApplyPayAmtH.SetValueTrigger(0, false);
                //    ItmApplyGainAmt.SetValueTrigger(0, false);
                //}
                //else
                //{
                //    payH = (decimal)GFunc.RndC(payF * payRate, GVar.RndDecs.Amtpt);
                //    exrate = (decimal)GFunc.RndDC(payF, docF, GVar.RndDecs.Amtpt);

                //    ItmApplyDocAmtF.SetValueTrigger(docF, false);
                //    ItmApplyDocAmtH.SetValueTrigger(docH, false);
                //    ItmApplyPayAmtF.SetValueTrigger(payF, false);
                //    ItmApplyPayAmtH.SetValueTrigger(payH, false);
                //    ItmApplyRate.SetValueTrigger(exrate, false);                  

                //    if (CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Payment_Received || CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Payment_Received || CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Contra || CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Contra)
                //        ItmApplyGainAmt.SetValueTrigger(payH - docH, false);//ar
                //    else if (CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Payment_Issue)
                //        ItmApplyGainAmt.SetValueTrigger(docH - payH, false);//ap
                //}
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
        private void ItmApplyRate_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                DocAmtF_Update();

               // #region declaration
               // decimal dueF = 0;
               // decimal dueH = 0;
               // decimal disF = 0;
               // decimal disH = 0;
               // decimal docF = 0;
               // decimal docH = 0;
               // decimal payF = 0;
               // decimal payH = 0;
               // decimal exrate = 1;
               // decimal docRate = 1;
               // decimal payRate = 1;
               // #endregion

               // #region set variables
               // dueF = (decimal)GFunc.RndC(GFunc.NEDec(ItmApplyDueAmtF.Value, 0), GVar.RndDecs.Amtpt);
               // dueH = (decimal)GFunc.RndC(GFunc.NEDec(ItmApplyDueAmtH.Value, 0), GVar.RndDecs.Amtpt);
               // disF = (decimal)GFunc.RndC(GFunc.NEDec(ItmApplyDisAmtF.Value, 0), GVar.RndDecs.Amtpt);
               // disH = (decimal)GFunc.RndC(GFunc.NEDec(ItmApplyDisAmtH.Value, 0), GVar.RndDecs.Amtpt);
               // docF = (decimal)GFunc.RndC(GFunc.NEDec(ItmApplyDocAmtF.Value, 0), GVar.RndDecs.Amtpt);
               // docH = (decimal)GFunc.RndC(GFunc.NEDec(ItmApplyDocAmtH.Value, 0), GVar.RndDecs.Amtpt);
               // exrate = (decimal)GFunc.RndC(GFunc.NEDec(ItmApplyRate.Value, 1), GVar.RndDecs.Curpt);
               // docRate = (decimal)GFunc.RndC(GFunc.NEDec(LinkDocCurrRate.Value, 0), GVar.RndDecs.Curpt);
               // payRate = (decimal)GFunc.RndC(GFunc.NEDec(ItmApplyPayCurrRate, 0), GVar.RndDecs.Curpt);
               // #endregion

               // payF = (decimal)GFunc.RndC(docF * exrate, GVar.RndDecs.Amtpt);
               // payH = (decimal)GFunc.RndC(payF * payRate, GVar.RndDecs.Amtpt);

               // ItmApplyPayAmtF.SetValueTrigger(payF, false);
               // ItmApplyPayAmtH.SetValueTrigger(payH, false);
               //// ItmApplyGainAmt.SetValueTrigger(payH - docH, false);

               // //ItmApplyGainAmt.SetValueTrigger(PayAmtH - DocAmtH, false);
               // if (CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Payment_Received || CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Payment_Received || CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Contra || CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Contra)
               //     ItmApplyGainAmt.SetValueTrigger(payH - docH, false);//ar
               // else if (CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Payment_Issue)
               //     ItmApplyGainAmt.SetValueTrigger(docH - payH, false);//ap
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
        private void ItmApplyDocAmtH_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                DocAmtF_Update();
                //decimal docH = ItmApplyDocAmtH.DecimalValue;
                //decimal payH = ItmApplyPayAmtH.DecimalValue;
                ////ItmApplyGainAmt.SetValueTrigger(PayAmtH - DocAmtH, false);
                //if (CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Payment_Received || CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Payment_Received || CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Contra || CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Contra)
                //    ItmApplyGainAmt.SetValueTrigger(payH - docH, false);//ar
                //else if (CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Payment_Issue)
                //    ItmApplyGainAmt.SetValueTrigger(docH - payH, false);//ap
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

        //Calculate Function
        private bool DocAmtF_Update()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                #region declaration
                decimal dueF = 0;
                decimal dueH = 0;
                decimal disF = 0;
                decimal disH = 0;
                decimal docF = 0;
                decimal docH = 0;
                decimal payF = 0;
                decimal payH = 0;
                decimal exrate = 1;
                decimal docRate = 1;
                decimal payRate = 1;
                #endregion

                #region set variables
                dueF = GFunc.RndC(GFunc.NEDec(ItmApplyDueAmtF.Value, 0), GVar.RndDecs.Amtpt);
                dueH = GFunc.RndC(GFunc.NEDec(ItmApplyDueAmtH.Value, 0), GVar.RndDecs.Amtpt);
                disF = GFunc.RndC(GFunc.NEDec(ItmApplyDisAmtF.Value, 0), GVar.RndDecs.Amtpt);
                disH = GFunc.RndC(GFunc.NEDec(ItmApplyDisAmtH.Value, 0), GVar.RndDecs.Amtpt);
                docF = GFunc.RndC(GFunc.NEDec(ItmApplyDocAmtF.Value, 0), GVar.RndDecs.Amtpt);
                docH = GFunc.RndC(GFunc.NEDec(ItmApplyDocAmtH.Value, 0), GVar.RndDecs.Amtpt);
                payF = GFunc.RndC(GFunc.NEDec(ItmApplyPayAmtF.Value, 0), GVar.RndDecs.Amtpt);
                payH = GFunc.RndC(GFunc.NEDec(ItmApplyPayAmtH.Value, 0), GVar.RndDecs.Amtpt);
                exrate = GFunc.RndC(GFunc.NEDec(ItmApplyRate.Value, 1), GVar.RndDecs.Curpt);
                docRate = GFunc.RndC(GFunc.NEDec(LinkDocCurrRate.Value, 0), GVar.RndDecs.Curpt);
                payRate = GFunc.RndC(GFunc.NEDec(ItmApplyPayCurrRate, 0), GVar.RndDecs.Curpt);
                #endregion

                if (docF == 0)
                {
                    //When user reset or clear the applied amount
                    ItmApplyDocAmtF.SetValueTrigger(0,false);
                    ItmApplyDocAmtH.SetValueTrigger(0, false);
                    ItmApplyPayAmtF.SetValueTrigger(0, false);
                    ItmApplyPayAmtH.SetValueTrigger(0, false);
                    ItmApplyGainAmt.SetValueTrigger(0,false);
                }
                else
                {
                    if (dueF - disF - docF == 0)
                    {
                        //User Apply full 

                        docH = dueH - disH;
                        
                        payF = (decimal)GFunc.RndC(docF * exrate, GVar.RndDecs.Amtpt);
                        payH = (decimal)GFunc.RndC(payF * payRate, GVar.RndDecs.Amtpt);
                        

                        ItmApplyDocAmtF.SetValueTrigger(docF,false);
                        ItmApplyDocAmtH.SetValueTrigger(docH, false);
                        ItmApplyPayAmtF.SetValueTrigger(payF, false);
                        ItmApplyPayAmtH.SetValueTrigger(payH, false);

                        if (CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Payment_Received || CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Payment_Received || CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Contra || CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Contra)
                            ItmApplyGainAmt.SetValueTrigger(payH-docH, false);//ar
                        else if(CallerDoc.DocCodeKey==(int)GEnum.SystemCode.Payment_Issue)
                            ItmApplyGainAmt.SetValueTrigger(docH - payH, false);//ap
                    }
                    else
                    {
                        //User Apply partial
                        docH = (decimal)GFunc.RndC(docF * docRate, GVar.RndDecs.Amtpt);
                        payF = (decimal)GFunc.RndC(docF * exrate, GVar.RndDecs.Amtpt);
                        payH = (decimal)GFunc.RndC(payF * payRate, GVar.RndDecs.Amtpt);

                        ItmApplyDocAmtF.SetValueTrigger(docF,false);
                        ItmApplyDocAmtH.SetValueTrigger(docH, false);
                        ItmApplyPayAmtF.SetValueTrigger(payF, false);
                        ItmApplyPayAmtH.SetValueTrigger(payH, false);

                        if (CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Payment_Received || CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Payment_Received || CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Contra || CallerDoc.DocCodeKey == (int)GEnum.SystemCode.Cash_Contra)
                            ItmApplyGainAmt.SetValueTrigger(payH - docH, false);//ar
                        else if(CallerDoc.DocCodeKey==(int)GEnum.SystemCode.Payment_Issue)
                            ItmApplyGainAmt.SetValueTrigger(docH - payH, false);//ap
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
            return true;
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
