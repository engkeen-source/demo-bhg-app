using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOLib;
using System.Data.SqlClient;
using TAUtil;

namespace WinUI
{
    public partial class frmMSTPriceInfoUpdate : Form
    {
        #region Local Variables
        private string msgID = string.Empty;
        private bool formClose = false;
        private bool formEdit = false;
        private GEnum.PriceUpdateOption updateOption = GEnum.PriceUpdateOption.UpdateCustomerPriceList;
        GEnum.SystemCode OpenCode = GEnum.SystemCode.Price_Update;
        string PermID = GVar.PermissionID.Price_List_Batch_Update;

        string ContextMenuSetting = string.Empty;
        #endregion

        //Construstor
        public frmMSTPriceInfoUpdate(GEnum.SystemCode DocCodeKey)
        {
            InitializeComponent();
            OpenCode = DocCodeKey;
        }//Completed
        public frmMSTPriceInfoUpdate()
        {
            InitializeComponent();
        }//Completed

        //Form Events
        private void frmMSTPriceInfoUpdate_Load(object sender, EventArgs e)
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;
            bool isReadOnly = false;
            try
            {
                if (SECPermUtility.Any(PermID, out isReadOnly, true) == false)
                    formClose = true;
                
                GlobalUI.Combos_Fill(this, (int)GEnum.SystemCode.Price_Update);
                
                //get currency positon from sys_option (CurrKey, CurrID)
                DataTable dtcurr = GFunc.ExecuteQuery("Select a.OpValue,b.CurrID from SYS_Option a join REF_Curr b on a.OpValue=b.CurrKey where a.OpGrp=220 order by a.OpSeq");

                for (int i = 1; i < 16; i++)
                {
                    //Set Checkbox (StandardPrice 1 to 15) to store the currkey as define in system option
                    TAUtil.TACheckBoxEditor chk = (TAUtil.TACheckBoxEditor)this.Controls.Find("StandardPrice" + i.ToString(), true).First();
                    chk.Text = dtcurr.Rows[i-1].ItemArray[0].ToString();

                    //Set label (1 to 15) to show the currency ID as define in system option
                    Infragistics.Win.Misc.UltraLabel lbl = (Infragistics.Win.Misc.UltraLabel)this.Controls.Find("lbl" + i.ToString(), true).First();
                    lbl.Text = i.ToString() + ".  " + dtcurr.Rows[i - 1].ItemArray[1].ToString();

                    //Set StandardPrice Currency Rate (1 to 15) if Home currency then is alway 1 else get the rate from the currency table
                    TAUtil.TANumericEditor txt = (TAUtil.TANumericEditor)this.Controls.Find("txt" + i.ToString(), true).First();
                    txt.Text = DocComUtility.CurrRate_Get(GFunc.NEInt(chk.Text, 1), (DateTime)DateTime.Now, false).ToString();

                    //Disable Currency rate when currkey is Home currency
                    if (GFunc.NEInt(chk.Text, 0) == 1)
                    {
                        txt.Enabled = false;
                    }
                }
                
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)GEnum.SystemCode.Price_Update);

                //Set default options
                OptSelection.CheckedIndex = 0;
                updateOption = GEnum.PriceUpdateOption.UpdateCustomerPriceList;
                PriceDecPlace.SetValueTrigger(0,false);
                BasePrice.SetValueTrigger(10,false);
                PriceRoundMode.SetValueTrigger(0,false);
                ReportFiles.SetValueTrigger(3418,false);
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
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void frmMSTPriceInfoUpdate_Shown(object sender, EventArgs e)
        {
            if (formClose)
                this.Close();
        }//Completed
        private void frmMSTPriceInfoUpdate_FormClosing(object sender, FormClosingEventArgs e)
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
                e.Cancel = false;
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
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void frmMSTPriceInfoUpdate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)OpenCode);
                }
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
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }//Completed
        private void btnUpdatePrice_Click(object sender, EventArgs e)
        {
            try
            {
                if(UpdateData(GEnum.PriceUpdateMode.Update))
                    MsgBox.Show("Successfully Updated");
                else
                    MsgBox.Show("Update Fail");
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
        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateData(GEnum.PriceUpdateMode.PreviewData);
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
        private void btnUpdatePriceRatio_Click(object sender, EventArgs e)
        {
            try
            {
                if (UpdateData(GEnum.PriceUpdateMode.Update))
                    MsgBox.Show("Successfully Updated");
                else
                    MsgBox.Show("Update Fail");

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

        //Control Events
        private void Combo_NotInListAdd(object sender, Infragistics.Win.UltraWinEditors.ValidationErrorEventArgs e)
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
        private void Combo_NotInList(object sender, Infragistics.Win.UltraWinEditors.ValidationErrorEventArgs e)
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
        private void ItmFrom_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItmFrom");
                if (DocHDRUtil.EditorButton_Popup((int)GEnum.SystemCode.Price_Update, ItmFrom.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref key, ref id, ref des))
                    ItmFrom.SetValueTrigger(id, false);
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
        private void ItmTo_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItmTo");
                if (DocHDRUtil.EditorButton_Popup((int)GEnum.SystemCode.Price_Update, ItmTo.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref key, ref id, ref des))
                    ItmTo.SetValueTrigger(id, false);
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
        private void OptSelection_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int SelectedOption = GFunc.NEInt(OptSelection.Value,0);

                switch (SelectedOption)
                {
                    case 10 :    //Customer Price Update
                        tbRange.Tabs[0].Visible = true;
                        tbRange.Tabs[1].Visible = false;
                        pnlPriceRate.Visible = false;
                        tbPrice.Tabs[0].Visible = true;
                        tbPrice.Tabs[1].Visible = false;
                        ReportFileLabel.Visible = false;
                        ReportFiles.Visible = false;
                        updateOption = GEnum.PriceUpdateOption.UpdateCustomerPriceList;
                        GlobalUI.BindComboValue(BasePrice, "SYSMsgPriceUpdateCustBasePrice");
                        GlobalUI.BindComboValue(ReportFiles, "SYSRepRpt%2016400");
                        ReportFiles.DisplayMember = "rptdes";
                        ReportFiles.ValueMember = "UID";
                        break;

                    case 20:     //Item Price Update
                        tbRange.Tabs[0].Visible = false;
                        tbRange.Tabs[1].Visible = true;
                        pnlPriceRate.Visible = true;
                        tbPrice.Tabs[0].Visible = true;
                        tbPrice.Tabs[1].Visible = false;
                        ReportFileLabel.Visible = true;
                        ReportFiles.Visible = true;
                        updateOption = GEnum.PriceUpdateOption.UpdateInventoryPriceList;
                        GlobalUI.BindComboValue(BasePrice, "SYSMsgPriceUpdateBasrPrice");
                        GlobalUI.BindComboValue(ReportFiles, "SYSRepRpt%2016300");
                        ReportFiles.DisplayMember = "rptdes";
                        ReportFiles.ValueMember = "UID";
                        break;

                    default:    //Item Ratio Update
                        tbRange.Tabs[0].Visible = true;
                        tbRange.Tabs[1].Visible = false;
                        pnlPriceRate.Visible = true;
                        tbPrice.Tabs[0].Visible = false;
                        tbPrice.Tabs[1].Visible = true;
                        updateOption = GEnum.PriceUpdateOption.UpdateInventoryPriceRatioList;
                        break;
                }   //

                ItmFrom.Focus();
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
        private void tbRange_KeyDown(object sender, KeyEventArgs e)
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
        }//Completed
        private void VendorIDFrom_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;

                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "VendorIDFrom");
                if (DocHDRUtil.EditorButton_Popup((int)OpenCode, VendorIDFrom.Text, listSettingID, (int)GEnum.PopupType.VendID, ref key, ref id, ref des))
                {
                    VendorIDFrom.SetValueTrigger(id, false);
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
        private void VendorIDTo_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;

                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "VendorIDTo");
                if (DocHDRUtil.EditorButton_Popup((int)OpenCode, VendorIDTo.Text, listSettingID, (int)GEnum.PopupType.VendID, ref key, ref id, ref des))
                {
                    VendorIDTo.SetValueTrigger(id, false);
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

        //Custom Functions
        private bool UpdateData(GEnum.PriceUpdateMode mode)
        {
            int retValue = 0;

            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;
            bool bNothingSelected = true;
            string repPara = string.Empty;
           
            try
            {
                List<SqlParameter> paraList = new List<SqlParameter>();
                paraList.Add(new SqlParameter("@PriceUpdateOption", (int)updateOption));
                paraList.Add(new SqlParameter("@PriceUpdateMode", (int)mode));

                #region add critera for Itm and Categroy and Class range
                if (GFunc.IsNE(ItmFrom.Text) == false)
                {
                    paraList.Add(new SqlParameter("@ItmFrom", ItmFrom.Text));
                    repPara = repPara + "ITEM RANGE >=" + ItmFrom.Text + ",";
                }

                if (GFunc.IsNE(ItmTo.Text) == false)
                {
                    paraList.Add(new SqlParameter("@ItmTo", ItmTo.Text));
                    repPara = repPara + "ITEM RANGE <=" + ItmTo.Text + ",";
                }
                if (GFunc.IsNE(ItmFrom.Text) && GFunc.IsNE(ItmTo.Text))
                {
                    repPara = repPara + "FOR ALL ITEM ,";
                }

                if (GFunc.IsNE(Cat1From.Text) == false)
                {
                    paraList.Add(new SqlParameter("@Cat1From", Cat1From.Text));
                    repPara = repPara + "CATEGORY 1 RANGE >=" + Cat1From.Text + ",";
                }
                if (GFunc.IsNE(Cat1To.Text) == false)
                {
                    paraList.Add(new SqlParameter("@Cat1To", Cat1To.Text));
                    repPara = repPara + "CATEGORY 1 RANGE <=" + Cat1To.Text + ",";
                }

                if (GFunc.IsNE(Cat1From.Text) && GFunc.IsNE(Cat1To.Text))
                {
                    repPara = repPara + "FOR ALL CATEGORY 1 ,";
                }

                if (GFunc.IsNE(Cat2From.Text) == false)
                {
                    paraList.Add(new SqlParameter("@Cat2From", Cat2From.Text));
                    repPara = repPara + "CATEGORY 2 RANGE >=" + Cat2From.Text + ",";
                }
                if (GFunc.IsNE(Cat2To.Text) == false)
                {
                    paraList.Add(new SqlParameter("@Cat2To", Cat2To.Text));
                    repPara = repPara + "CATEGORY 2 RANGE <=" + Cat2To.Text + ",";
                }
                if (GFunc.IsNE(Cat2From.Text) && GFunc.IsNE(Cat2To.Text))
                {
                    repPara = repPara + "FOR ALL CATEGORY 2 ,";
                }

                if (GFunc.IsNE(Cat3From.Text) == false)
                {
                    paraList.Add(new SqlParameter("@Cat3From", Cat3From.Text));
                    repPara = repPara + "CATEGORY 3 RANGE >=" + Cat3From.Text + ",";
                }
                if (GFunc.IsNE(Cat3To.Text) == false)
                {
                    paraList.Add(new SqlParameter("@Cat3To", Cat3To.Text));
                    repPara = repPara + "CATEGORY 3 RANGE <=" + Cat3To.Text + ",";
                }
                if (GFunc.IsNE(Cat3From.Text) && GFunc.IsNE(Cat3To.Text))
                {
                    repPara = repPara + "FOR ALL CATEGORY 3 ,";
                }

                if (GFunc.IsNE(Cat4From.Text) == false)
                {
                    paraList.Add(new SqlParameter("@Cat4From", Cat4From.Text));
                    repPara = repPara + "CATEGORY 4 RANGE >=" + Cat4From.Text + ",";
                }
                if (GFunc.IsNE(Cat4To.Text) == false)
                {
                    paraList.Add(new SqlParameter("@Cat4To", Cat4To.Text));
                    repPara = repPara + "CATEGORY 4 RANGE <=" + Cat4To.Text + ",";
                }

                if (GFunc.IsNE(Cat4From.Text) && GFunc.IsNE(Cat4To.Text))
                {
                    repPara = repPara + "FOR ALL CATEGORY 4 ,";
                }

                if (GFunc.IsNE(Cat5From.Text) == false)
                {
                    paraList.Add(new SqlParameter("@Cat5From", Cat5From.Text));
                    repPara = repPara + "CATEGORY 5 RANGE >=" + Cat5From.Text + ",";
                }
                if (GFunc.IsNE(Cat5To.Text) == false)
                {
                    paraList.Add(new SqlParameter("@Cat5To", Cat5To.Text));
                    repPara = repPara + "CATEGORY 5 RANGE <=" + Cat5To.Text + ",";
                }
                if (GFunc.IsNE(Cat5From.Text) && GFunc.IsNE(Cat5To.Text))
                {
                    repPara = repPara + "FOR ALL CATEGORY 5 ,";
                }

                if (GFunc.IsNE(txtClass.Text) == false)
                {
                    paraList.Add(new SqlParameter("@Class", txtClass.Text));
                }
                else
                {
                    repPara = repPara + "FOR ALL CLASS,";
                }
                #endregion

                #region Customer Price Update
                if (updateOption == GEnum.PriceUpdateOption.UpdateCustomerPriceList)
                {
                    if (GFunc.IsNE(PriceIDFrom.Text) == false)
                    {
                        paraList.Add(new SqlParameter("@PriceIDFrom", PriceIDFrom.Text));
                        repPara = repPara + "PRICEID RANGE >=" + PriceIDFrom.Text + ",";
                    }
                    if (GFunc.IsNE(PriceIDTo.Text) == false)
                    {
                        paraList.Add(new SqlParameter("@PriceIDTo", PriceIDTo.Text));
                        repPara = repPara + "PRICEID RANGE <=" + PriceIDTo.Text + ",";
                    }

                    if (GFunc.IsNE(CurrKey.Text) == false)
                    {
                        paraList.Add(new SqlParameter("@CurrencyKey", CurrKey.Value));
                    }
                    else
                    {
                        repPara = repPara + "FOR ALL CURRENCY,";
                    }

                }
                #endregion

                #region Item Price Update

                if (updateOption == GEnum.PriceUpdateOption.UpdateInventoryPriceList)
                {
                    if (GFunc.IsNE(VendorIDFrom.Text) == false)
                    {
                        paraList.Add(new SqlParameter("@VendorIDFrom", VendorIDFrom.Text));
                        repPara = repPara + "VENDOR RANGE >=" + VendorIDFrom.Text + ",";
                    }
                    if (GFunc.IsNE(VendorIDTo.Text) == false)
                    {
                        paraList.Add(new SqlParameter("@VendorIDTo", VendorIDTo.Text));
                        repPara = repPara + "VENDOR RANGE >=" + VendorIDFrom.Text + ",";
                    }
                    if (GFunc.IsNE(VendorIDFrom.Text) && GFunc.IsNE(VendorIDTo.Text))
                    {
                        repPara = repPara + "FOR ALL VENDOR,";
                    }

                    paraList.Add(new SqlParameter("@Selected01", StandardPrice1.Checked ));
                    paraList.Add(new SqlParameter("@Selected02", StandardPrice2.Checked));
                    paraList.Add(new SqlParameter("@Selected03", StandardPrice3.Checked));
                    paraList.Add(new SqlParameter("@Selected04", StandardPrice4.Checked));
                    paraList.Add(new SqlParameter("@Selected05", StandardPrice5.Checked));
                    paraList.Add(new SqlParameter("@Selected06", StandardPrice6.Checked));
                    paraList.Add(new SqlParameter("@Selected07", StandardPrice7.Checked));
                    paraList.Add(new SqlParameter("@Selected08", StandardPrice8.Checked));
                    paraList.Add(new SqlParameter("@Selected09", StandardPrice9.Checked));
                    paraList.Add(new SqlParameter("@Selected10", StandardPrice10.Checked));
                    paraList.Add(new SqlParameter("@Selected11", StandardPrice11.Checked));
                    paraList.Add(new SqlParameter("@Selected12", StandardPrice12.Checked));
                    paraList.Add(new SqlParameter("@Selected13", StandardPrice13.Checked));
                    paraList.Add(new SqlParameter("@Selected14", StandardPrice14.Checked));
                    paraList.Add(new SqlParameter("@Selected15", StandardPrice15.Checked));

                    paraList.Add(new SqlParameter("@CurrRate1", GFunc.NEDec(txt1.Value,0)));
                    paraList.Add(new SqlParameter("@CurrRate2", GFunc.NEDec(txt2.Value, 0)));
                    paraList.Add(new SqlParameter("@CurrRate3", GFunc.NEDec(txt3.Value, 0)));
                    paraList.Add(new SqlParameter("@CurrRate4", GFunc.NEDec(txt4.Value, 0)));
                    paraList.Add(new SqlParameter("@CurrRate5", GFunc.NEDec(txt5.Value, 0)));
                    paraList.Add(new SqlParameter("@CurrRate6", GFunc.NEDec(txt6.Value, 0)));
                    paraList.Add(new SqlParameter("@CurrRate7", GFunc.NEDec(txt7.Value, 0)));
                    paraList.Add(new SqlParameter("@CurrRate8", GFunc.NEDec(txt8.Value, 0)));
                    paraList.Add(new SqlParameter("@CurrRate9", GFunc.NEDec(txt9.Value, 0)));
                    paraList.Add(new SqlParameter("@CurrRate10", GFunc.NEDec(txt10.Value, 0)));
                    paraList.Add(new SqlParameter("@CurrRate11", GFunc.NEDec(txt11.Value, 0)));
                    paraList.Add(new SqlParameter("@CurrRate12", GFunc.NEDec(txt12.Value, 0)));
                    paraList.Add(new SqlParameter("@CurrRate13", GFunc.NEDec(txt13.Value, 0)));
                    paraList.Add(new SqlParameter("@CurrRate14", GFunc.NEDec(txt14.Value, 0)));
                    paraList.Add(new SqlParameter("@CurrRate15", GFunc.NEDec(txt15.Value, 0)));


                    bNothingSelected = true;
                    for (int i = 1; i < 16; i++)
                    {
                        TAUtil.TACheckBoxEditor chk = (TAUtil.TACheckBoxEditor)this.Controls.Find("StandardPrice" + i.ToString(), true).First();
                        repPara = repPara + ((Infragistics.Win.Misc.UltraLabel)this.Controls.Find("lbl" + i.ToString(), true).First()).Text + ",";

                        if (chk.Checked)
                            bNothingSelected = false;

                    }

                    if (bNothingSelected)
                    {
                        MsgBox.Show("Please select at least one currency to update.");
                        return false;
                    }
                    
                }

                #endregion

                #region Ratio Update
                if (updateOption == GEnum.PriceUpdateOption.UpdateInventoryPriceRatioList)
                {
                                     
                    paraList.Add(new SqlParameter("@NewPriceRatio1", decimal.Parse(NewPriceRatio1.Text)));
                    paraList.Add(new SqlParameter("@NewPriceRatio2", decimal.Parse(NewPriceRatio2.Text)));
                    paraList.Add(new SqlParameter("@NewPriceRatio3", decimal.Parse(NewPriceRatio3.Text)));
                    paraList.Add(new SqlParameter("@NewPriceRatio4", decimal.Parse(NewPriceRatio4.Text)));
                    paraList.Add(new SqlParameter("@NewPriceRatio5", decimal.Parse(NewPriceRatio5.Text)));
                    paraList.Add(new SqlParameter("@NewPriceRatio6", decimal.Parse(NewPriceRatio6.Text)));
                    paraList.Add(new SqlParameter("@NewPriceRatio7", decimal.Parse(NewPriceRatio7.Text)));
                    paraList.Add(new SqlParameter("@NewPriceRatio8", decimal.Parse(NewPriceRatio8.Text)));
                    paraList.Add(new SqlParameter("@NewPriceRatio9", decimal.Parse(NewPriceRatio9.Text)));
                    paraList.Add(new SqlParameter("@NewPriceRatio10", decimal.Parse(NewPriceRatio10.Text)));
                    paraList.Add(new SqlParameter("@NewPriceRatio11", decimal.Parse(NewPriceRatio11.Text)));
                    paraList.Add(new SqlParameter("@NewPriceRatio12", decimal.Parse(NewPriceRatio12.Text)));
                    paraList.Add(new SqlParameter("@NewPriceRatio13", decimal.Parse(NewPriceRatio13.Text)));
                    paraList.Add(new SqlParameter("@NewPriceRatio14", decimal.Parse(NewPriceRatio14.Text)));
                    paraList.Add(new SqlParameter("@NewPriceRatio15", decimal.Parse(NewPriceRatio15.Text)));

                    bNothingSelected = true;
                    for (int i = 1; i < 16; i++)
                    {
                        TAUtil.TACheckBoxEditor chk = (TAUtil.TACheckBoxEditor)this.Controls.Find("Ratio" + i.ToString(), true).First();

                        if (chk.Checked)
                        {
                            bNothingSelected = false;
                            break;
                        }

                    }
                    if (bNothingSelected)
                    {
                        MsgBox.Show("Please select at least one ratio to update.");
                        return false;
                    }
                }
                SqlParameter para = new SqlParameter("@RetValue", retValue);
                para.Direction = ParameterDirection.Output;
                paraList.Add(para);
                #endregion

                #region add criteria for price calculation
                paraList.Add(new SqlParameter("@BasePriceType", GFunc.NEDec(BasePrice.Value, 10)));
                repPara = repPara + "BASE PRICE TYPE = " + BasePrice.Text + ",";

                paraList.Add(new SqlParameter("@Ratio", GFunc.NEDec(Ratio.Value, 0)));
                repPara = repPara + "RATIO = " + GFunc.NEDec(Ratio.Text, 0) + ",";

                paraList.Add(new SqlParameter("@AddtionalAmt", GFunc.NEDec(AddtionalAmt.Value, 0)));
                repPara = repPara + "ADDTIONAL AMT = " + GFunc.NEDec(AddtionalAmt.Text, 0) + ",";

                paraList.Add(new SqlParameter("@PriceDecPlace", GFunc.NEInt(PriceDecPlace.Value, 0)));
                repPara = repPara + "PRICE DEC PLACE = " + GFunc.NEStr(PriceDecPlace.Text, string.Empty) + ",";

                paraList.Add(new SqlParameter("@PriceRoundMode", GFunc.NEInt(PriceRoundMode.Value, 0)));
                repPara = repPara + "PRICE ROUND MODE = " + GFunc.NEStr(PriceRoundMode.Text, string.Empty);
                #endregion

                if (updateOption == GEnum.PriceUpdateOption.UpdateInventoryPriceList || updateOption == GEnum.PriceUpdateOption.UpdateCustomerPriceList)
                {
                    //Perform (update or prepare Preview Data) of Customer/Item Price update
                    //Note the SP could update the respective table or just prepare the data for preview purpose only
                    DataTable dtResult = GFunc.ExecuteProc("MSTPrice_Update", paraList);

                    string queryTest = GFunc.ExecuteProcQueryStringGet("MSTPrice_Update", paraList);
                    
                    #region Preview report to show user on the changes
                    ReportLoader rptLoader = new ReportLoader();
                    rptLoader.RepKey = updateOption == GEnum.PriceUpdateOption.UpdateInventoryPriceList ? 2016300 : 2016400;
                    string rptFileName = string.Empty;
                    string selectedRows = string.Empty;
                    if (updateOption == GEnum.PriceUpdateOption.UpdateInventoryPriceList)
                    {
                        rptFileName = ReportFiles.SelectedRow.Cells["RptNm"].Value.ToString();
                    }
                    else
                    {
                        rptFileName = "CustomerPriceUpdate.rpt";
                    }

                    CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                    rptDoc.Load(Application.StartupPath + @"\Reports\" + rptFileName);
                    rptDoc.SetDataSource(dtResult);

                    List<ReportParameter> repParaList = new List<ReportParameter>();
                    string opCmpValue = SysOptionUtility.GetStr("CompanyName");
                    repParaList.Add(new ReportParameter("pCmpName", opCmpValue));
                    repParaList.Add(new ReportParameter("pRepRange", repPara));
                    if (updateOption == GEnum.PriceUpdateOption.UpdateInventoryPriceList)
                    {
                        for (int i = 1; i < 16; i++)
                        {
                            string currID = ((Infragistics.Win.Misc.UltraLabel)this.Controls.Find("lbl" + i.ToString(), true).First()).Text;
                            currID = currID.Substring(currID.IndexOf(".") + 1).Trim();
                            repParaList.Add(new ReportParameter("pCurrency" + i.ToString(), currID));
                            selectedRows = selectedRows + (SysOptionUtility.GetBool("ItemPriceRow" + i.ToString("00")) == true ? i.ToString() : "0") + ",";
                        }
                        repParaList.Add(new ReportParameter("pDisplayRows", selectedRows));
                    }
                   
                    foreach (ReportParameter p in repParaList)
                    {
                        rptDoc.SetParameterValue(p.ParameterName, p.ParameterValue);
                    }
                 
                    frmReportViewer fRptViewer = new frmReportViewer();
                    fRptViewer.RepKey = rptLoader.RepKey;
                    fRptViewer.RptName = rptFileName;
                    fRptViewer.RptDocument = rptDoc;
                    fRptViewer.MdiParent = frmMain.gfrmMain;
                    fRptViewer.Show();
                    #endregion
                }
                else //Ratio Update
                {
                    paraList.Add(new SqlParameter("@Selected01", Ratio1.Checked));
                    paraList.Add(new SqlParameter("@Selected02", Ratio2.Checked));
                    paraList.Add(new SqlParameter("@Selected03", Ratio3.Checked));
                    paraList.Add(new SqlParameter("@Selected04", Ratio4.Checked));
                    paraList.Add(new SqlParameter("@Selected05", Ratio5.Checked));
                    paraList.Add(new SqlParameter("@Selected06", Ratio6.Checked));
                    paraList.Add(new SqlParameter("@Selected07", Ratio7.Checked));
                    paraList.Add(new SqlParameter("@Selected08", Ratio8.Checked));
                    paraList.Add(new SqlParameter("@Selected09", Ratio9.Checked));
                    paraList.Add(new SqlParameter("@Selected10", Ratio10.Checked));
                    paraList.Add(new SqlParameter("@Selected11", Ratio11.Checked));
                    paraList.Add(new SqlParameter("@Selected12", Ratio12.Checked));
                    paraList.Add(new SqlParameter("@Selected13", Ratio13.Checked));
                    paraList.Add(new SqlParameter("@Selected14", Ratio14.Checked));
                    paraList.Add(new SqlParameter("@Selected15", Ratio15.Checked));

                    GFunc.ExecuteProcDataSet("MSTPrice_Update", paraList);

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
        }

        //Error Functions
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
    }
}
