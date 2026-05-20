using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOLib;
using System.Collections;
using Infragistics.Win.UltraWinEditors;
using TAUtil;

namespace WinUI
{
    public partial class frmApplyIV : Form
    {
        #region Declaration
        private int DocDC = 0;
        private int DocDK = 0;
        private int DocConKey = 0;
        private int DocCurrKey = 1;
        private decimal DocCurrRate = 1;
        private decimal DocGrand = 0M;
        private decimal DocHome = 0M;
      
        private int DocApplyDC = 0;
        private int DocApplyDK = 0;
        private decimal DocApplyGain = 0;

        private int NewApplyDC = 0;
        private int NewApplyDK = 0;
        private decimal NewApplyGain = 0;

        private int SvrApplyDC = 0;
        private int SvrApplyDK = 0;
        private decimal SvrApplyAmtH = 0;
        private decimal SvrApplyAmtF = 0;
        
        private Document doc = null;
        private string ContextMenuSetting = string.Empty;
        #endregion

        //Initialize
        public frmApplyIV()
        {
            InitializeComponent();
        }//Completed
        public frmApplyIV(Document objDoc)
        {
            InitializeComponent();
            doc = objDoc;
        }//Completed

        //From Events
        private void frmApplyIV_Load(object sender, EventArgs e)
        {
            try
            {
                GlobalUI.FormGrids_Set(this, (int)doc.DocCodeKey, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)doc.DocCodeKey);
                GlobalUI.Combos_Fill(this, (int)doc.DocCodeKey);

                DocumentSource.SetValueTrigger(20, false);//set Invoice as Default
                //Initialise Variable
                DocDC = (int)doc.DocCodeKey;
                DocDK = (int)doc.DocKey;
                DocConKey = (int)GFunc.GetIntPropertyValue("DocConKey", doc);
                DocCurrKey = (int)GFunc.GetIntPropertyValue("DocCurrKey", doc);
                DocCurrRate = (decimal)GFunc.GetDecimalPropertyValue("DocCurrRate",doc);
                DocGrand = (decimal)GFunc.GetDecimalPropertyValue("DocGrand", doc);
                DocHome = (decimal)GFunc.GetDecimalPropertyValue("DocHome", doc);
                DocApplyDC = GFunc.NEInt(GFunc.GetPropertyValue("DocApplyIVDC", doc), 0);
                DocApplyDK = GFunc.NEInt(GFunc.GetPropertyValue("DocApplyIVDK", doc), 0);
                DocApplyGain = GFunc.NEDec(GFunc.GetDecimalPropertyValue("DocApplyGainAmt", doc), 0);
                NewApplyDC = DocApplyDC;
                NewApplyDK = DocApplyDK;
                NewApplyGain = DocApplyGain;

                //Get Applied Doc Infor for CN at Server
                DataTable dtSvrDataCN = DocComUtility.SvrData_Get(DocDC, DocDK);
                if (dtSvrDataCN != null)
                {
                    if (dtSvrDataCN.Rows.Count > 0)
                    {
                        SvrApplyDC = GFunc.NEInt(dtSvrDataCN.Rows[0]["DocApplyIVDC"], -1);  //must use -1 so that we can compare correctly
                        SvrApplyDK = GFunc.NEInt(dtSvrDataCN.Rows[0]["DocApplyIVDK"], -1);  //must use -1 so that we can compare correctly                        
                        SvrApplyAmtH = (decimal)dtSvrDataCN.Rows[0]["DocHome"] - (decimal)dtSvrDataCN.Rows[0]["DocApplyGainAmt"];                    
                        SvrApplyAmtF = (decimal)dtSvrDataCN.Rows[0]["DocApplyAmtF"];

                        if (doc.DocCodeKey == (int)GEnum.SystemCode.Purchase_Credit_Note)
                            SvrApplyAmtH = (decimal)dtSvrDataCN.Rows[0]["DocHome"] - ((decimal)dtSvrDataCN.Rows[0]["DocApplyGainAmt"] * -1);
                    }
                }

                //Set Control Values for Display
                DocApplyGainAccKey.SetValueTrigger(GFunc.NEInt(GFunc.GetIntPropertyValue("DocApplyGainAccKey", doc),0), false);
                DocApplyGainAccDes.SetValueTrigger(MSTAcc.Get((int)DocApplyGainAccKey.Value).AccDes, false);
                DocApplyGainAmt.SetValueTrigger(DocApplyGain, false);
                ApplyInfor_Get(DocApplyDC, DocApplyDK);
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
        private void frmApplyIV_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)doc.DocCodeKey);
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

        //Button Common Events
        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                // ensure that Gain/loss account is not empty when there are gain/loss computed
                if (NewApplyGain != 0 && GFunc.IsNEZ(DocApplyGainAccKey.Value))
                {
                    MsgBox.Show("You must select an Exchange Gain/Loss Account");
                    return;
                }

                GFunc.SetPropertyValue("DocApplyIVDC", doc, NewApplyDC);
                GFunc.SetPropertyValue("DocApplyIVDK", doc, NewApplyDK);
                GFunc.SetPropertyValue("DocApplyIVID", doc, DocApplyIVID.Text);
                GFunc.SetPropertyValue("DocApplyGainAmt", doc, NewApplyGain);
                GFunc.SetPropertyValue("DocApplyGainAccKey", doc, DocApplyGainAccKey.Value);

                if (NewApplyDK == 0)
                {
                    GFunc.SetPropertyValue("DocApplyAmtF", doc, 0M);
                    GFunc.SetPropertyValue("DocApplyAmtH", doc, 0M);
                }

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
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }//Completed
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                GFunc.SetPropertyValue("DocApplyIVDC", doc, 0);
                GFunc.SetPropertyValue("DocApplyIVDK", doc, 0);
                GFunc.SetPropertyValue("DocApplyIVID", doc, "");
                GFunc.SetPropertyValue("DocApplyGainAmt", doc, 0M);
                GFunc.SetPropertyValue("DocApplyGainAccKey", doc, 0);           
                GFunc.SetPropertyValue("DocApplyAmtF", doc, 0M);
                GFunc.SetPropertyValue("DocApplyAmtH", doc, 0M);
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
            
        }
        
        //Control Event 
        private void Combo_NotInListAdd(object sender, ValidationErrorEventArgs e)
        {

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
        private void DocumentSource_CustomUpdate(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                RemoveLock_ApplyDoc();
                 
                //Reset values in variables
                NewApplyDK = 0;
                NewApplyDC = 0;
                NewApplyGain = 0M;

                //Reset values in control
                DocApplyIVID.Text = string.Empty;
                DocApplyGainAmt.SetValueTrigger(0M,false);
                DocApplyIVDate.SetValueTrigger(null,false);
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
        private void DocApplyGainAccKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                string vAccDes = string.Empty;

                if (DocApplyGainAccKey.SelectedRow != null)
                    vAccDes = ((DataRowView)DocApplyGainAccKey.SelectedRow.ListObject).Row["Des"].ToString();
                    
                DocApplyGainAccDes.SetValueTrigger(vAccDes, false);
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
        private void DocApplyGainAccKey_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, DocApplyGainAccKey.Name);

                if (DocHDRUtil.EditorButton_Popup(0, ((Control)sender).Text, listSettingID, (int)GEnum.PopupType.AccID, ref key, ref id, ref des))
                {
                    DocApplyGainAccKey.SetValueTrigger(key, false);
                    DocApplyGainAccDes.SetValueTrigger(des,false);
                }
            }
            catch (TAUtil.TAException taex)
            {
                Error(taex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed
        private void DocApplyGainAccDes_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;

                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, DocApplyGainAccDes.Name);
                if (DocHDRUtil.EditorButton_Popup((int)doc.DocCodeKey, DocApplyGainAccDes.Text, listSettingID, (int)GEnum.PopupType.AccDes, ref key, ref id, ref des))
                {
                    DocApplyGainAccKey.SetValueTrigger(key, false);
                    DocApplyGainAccDes.SetValueTrigger(des, false);
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
        private void DocApplyGainAccDes_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, DocApplyGainAccDes.Name);

                if (DocHDRUtil.EditorButton_Popup(0, ((Control)sender).Text, listSettingID, (int)GEnum.PopupType.AccDes, ref key, ref id, ref des))
                {
                    DocApplyGainAccKey.SetValueTrigger(key, false);
                    DocApplyGainAccDes.SetValueTrigger(des, false);
                }
            }
            catch (TAUtil.TAException taex)
            {
                Error(taex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed
        private void DocApplyIVID_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            try
            {
                int SearchCode  = 0;
                decimal NewApplyCurrRate = 1;
                decimal NewApplyGrand = 0M;
                decimal NewApplyHome = 0M;
                decimal NewApplyAmtF = 0M;
                decimal NewApplyAmtH = 0M;
                decimal NewApplyRevalueRate = 0M;
                decimal NewApplyRevalueAmtH = 0M;
                decimal NewApplyRate = 0M;
                decimal GainLossAmt = 0M;
                decimal PreviousApplyAmtF = SvrApplyAmtF;
                decimal PreviousApplyAmtH = SvrApplyAmtH;
                DataTable dtSvrData = null;     //record for Document to Apply 

                #region Determine SearchCode for Document Search Form
                if (GFunc.NEInt(DocumentSource.Value, 0) == 10)
                {
                    switch (DocDC)
                    {
                        case (int)GEnum.SystemCode.Sales_Credit_Note:
                            SearchCode = (int)GEnum.SystemCode.AR_Opening_Balance;
                            break;

                        case (int)GEnum.SystemCode.Cash_Credit_Note:
                            SearchCode = (int)GEnum.SystemCode.AR_Cash_Opening_Balance;
                            break;

                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            SearchCode = (int)GEnum.SystemCode.AP_Opening_Balance;
                            break;
                    }
                }
                else
                {
                    switch (DocDC)
                    {
                        case (int)GEnum.SystemCode.Sales_Credit_Note:
                            SearchCode = (int)GEnum.SystemCode.Sales_Invoice;
                            break;

                        case (int)GEnum.SystemCode.Cash_Credit_Note:
                            SearchCode = (int)GEnum.SystemCode.Cash_Sale;
                            break;

                        case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            SearchCode = (int)GEnum.SystemCode.Purchase_Invoice;
                            break;
                    }
                }
                #endregion

                frmDocSearch vfrmDocSearch = new frmDocSearch(SearchCode, DateTime.Now.Date.AddMonths(-1), DateTime.Now.Date,DocConKey,DocCurrKey );
                if (vfrmDocSearch.ShowDialog() == DialogResult.OK)
                {
                    if (!GFunc.IsNEZ(vfrmDocSearch.DocKey))
                    {
                        //Get Applied Doc infor for Document to be applied
                        dtSvrData = DocComUtility.SvrData_Get(vfrmDocSearch.DocCodeKey, vfrmDocSearch.DocKey);
                        if (dtSvrData != null)
                        {
                            //Check for valid apply document selected
                            if (GFunc.NEInt(dtSvrData.Rows[0]["DocConKey"],0) != DocConKey || GFunc.NEInt(dtSvrData.Rows[0]["DocCurrKey"],0) != DocCurrKey)
                            {
                                MsgBox.Show("You have selected an Invalid document to apply, please select another");
                                return;
                            }

                            //Get Apply Document infor
                            NewApplyCurrRate = GFunc.NEDec(dtSvrData.Rows[0]["DocCurrRate"],0);
                            NewApplyGrand = GFunc.NEDec(dtSvrData.Rows[0]["DocGrand"],0);
                            NewApplyHome = GFunc.NEDec(dtSvrData.Rows[0]["DocHome"],0);
                            NewApplyAmtF = GFunc.NEDec(dtSvrData.Rows[0]["DocApplyAmtF"],0);
                            NewApplyAmtH = GFunc.NEDec(dtSvrData.Rows[0]["DocApplyAmtH"],0);
                            NewApplyRevalueRate = GFunc.NEDec(dtSvrData.Rows[0]["DocRevalueRate"],0);
                            NewApplyRevalueAmtH = GFunc.NEDec(dtSvrData.Rows[0]["DocRevalueAmtH"],0);

                            //Get ApplyRate
                            if (GFunc.IsNEZ(NewApplyRevalueRate))
                                NewApplyRate = NewApplyCurrRate;
                            else
                                NewApplyRate = NewApplyRevalueRate;

                            //Validation
                            if (SvrApplyDC != GFunc.NEInt(dtSvrData.Rows[0]["DocCodeKey"], 0) || SvrApplyDK != GFunc.NEInt(dtSvrData.Rows[0]["DocKey"], 0))
                            {
                                //As we are applying to a new Invoice document, there is no PreviousApplyAmt, so we set it to zero
                                PreviousApplyAmtF = 0M;
                                PreviousApplyAmtH = 0M;                               
                            }

                            if (DocGrand > NewApplyGrand - NewApplyAmtF -(PreviousApplyAmtF))
                            {
                                MsgBox.Show("The Credit Note Amount exceeded the apply document outstanding amount, can't continue");
                                return;
                            }
                          

                            //Lock Document to Apply
                            RemoveLock_ApplyDoc();
                            SqlConnection cn = new SqlConnection(Database.BossDemoConnection);
                            cn.Open();
                            SysLockUtility.CheckAddLock(cn, true, 1, (GEnum.SystemCode)dtSvrData.Rows[0]["DocCodeKey"], GFunc.NEInt(dtSvrData.Rows[0]["DocKey"],0), doc.GUID);
                            cn.Close();

                            //Calculate Gain/Loss Amount
                            if (DocCurrKey != 1)
                            {
                                //Check if the Apply Document will be ApplyFull after this CN is applied to it
                                //When = 0 means apply to new Document and it will become applyfull
                                //When > 0 means re-apply to the same document and it will become applyfull
                                //When < 0 means apply to document but will NOT become applyfull

                                if (NewApplyGrand - NewApplyAmtF - PreviousApplyAmtF - DocGrand > 0)
                                    GainLossAmt = GFunc.RndC((NewApplyRate * DocGrand) - DocHome, GVar.RndDecs.Amtpt);
                                else
                                    GainLossAmt = GFunc.RndC(NewApplyHome + NewApplyRevalueAmtH - (NewApplyAmtH - PreviousApplyAmtH) - DocHome, GVar.RndDecs.Amtpt);
                                 
                                if (doc.DocCodeKey == (int)GEnum.SystemCode.Sales_Credit_Note || doc.DocCodeKey == (int)GEnum.SystemCode.Cash_Credit_Note)
                                {
                                    //for AR, we need to reverse the sign to update the correct value in DocApplyGainAmt field in AR_IV_tbl
                                    GainLossAmt *= -1;
                                }
                            }

                            NewApplyDC = GFunc.NEInt(dtSvrData.Rows[0]["DocCodeKey"],0);
                            NewApplyDK = GFunc.NEInt(dtSvrData.Rows[0]["DocKey"],0);
                            NewApplyGain = GainLossAmt;
                            DocApplyIVID.Text = GFunc.NEStr(dtSvrData.Rows[0]["DocID"],0);
                            DocApplyIVDate.SetValueTrigger(GFunc.NEDateTime(dtSvrData.Rows[0]["DocDate"],0),false);
                            DocApplyGainAmt.SetValueTrigger(GainLossAmt,false);
                            return;
                        }
                    }
                }              
                return;
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

        //Functions
        private bool RemoveLock_ApplyDoc()
        {
            try
            {
                //remove Lock -- we do not remove the apply document lock that is still exist in server record
                if (NewApplyDC != SvrApplyDC || NewApplyDK != SvrApplyDK)               
                    return SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByCodeKeyandDataKey, (GEnum.SystemCode)NewApplyDC, doc.GUID, NewApplyDK, 0);

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
        private bool ApplyInfor_Get(int docCodeKey, int docKey)
        {
            DataTable tmpTbl = new DataTable();
            int DocumentSourceValue = 0;
            try
            {
                if (GFunc.IsNEZ(docCodeKey) || GFunc.IsNEZ(docKey))
                    return false;

                switch (docCodeKey)
                {
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                        List<SqlParameter> vParam = new List<SqlParameter>();
                        vParam.Add(new SqlParameter("@DocKey", docKey));
                        vParam.Add(new SqlParameter("@DocCodeKey", docCodeKey));
                        vParam.Add(new SqlParameter("@DocID",""));
                        vParam.Add(new SqlParameter("@Option",1));
                        vParam.Add(new SqlParameter("@RetValue",Convert.ToInt32(0)));
                        tmpTbl = GFunc.ExecuteProc("ARIV_Get", vParam);
                        DocumentSourceValue = 20;
                        break;

                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                        vParam = new List<SqlParameter>();
                        vParam.Add(new SqlParameter("@DocKey", docKey));
                        vParam.Add(new SqlParameter("@DocCodeKey", docCodeKey));
                        vParam.Add(new SqlParameter("@DocID", ""));
                        vParam.Add(new SqlParameter("@Option", 1));
                        vParam.Add(new SqlParameter("@RetValue", Convert.ToInt32(0)));
                        tmpTbl = GFunc.ExecuteProc("APBL_Get", vParam);
                        DocumentSourceValue = 20;
                        break;

                    case (int)GEnum.SystemCode.AR_Opening_Balance:
                    case (int)GEnum.SystemCode.AR_Cash_Opening_Balance:
                    case (int)GEnum.SystemCode.AP_Opening_Balance:
                        vParam = new List<SqlParameter>();
                        vParam.Add(new SqlParameter("@DocKey", docKey));
                        vParam.Add(new SqlParameter("@DocCodeKey", docCodeKey));
                        vParam.Add(new SqlParameter("@DocID", ""));
                        vParam.Add(new SqlParameter("@Option", 1));
                        vParam.Add(new SqlParameter("@RetValue", Convert.ToInt32(0)));
                        tmpTbl = GFunc.ExecuteProc("MSTConOpenBal_Get", vParam);
                        DocumentSourceValue = 10;
                    break;
                }

                if (tmpTbl != null)
                {
                    if (tmpTbl.Rows.Count > 0)
                    {
                        DocumentSource.SetValueTrigger(DocumentSourceValue, false);
                        DocApplyIVID.SetValueTrigger(GFunc.NEStr(tmpTbl.Rows[0]["DocID"], ""),false);
                        DocApplyIVDate.SetValueTrigger(GFunc.NEDateTime(tmpTbl.Rows[0]["DocDate"], null),false);
                        return true;
                    }
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
