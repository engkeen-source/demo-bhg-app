using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using BOLib;
using Infragistics.Win.UltraWinEditors;
using TAUtil;

namespace WinUI
{
    public partial class frmSpecialCalculation : Form
    {
        //Variable 
        private string ContextMenuSetting = string.Empty;
        private GEnum.SpecialCalculationType calType = GEnum.SpecialCalculationType.Sale;  //Sale Or Purchase
        private GEnum.SpecialCalculationProcessType processType = GEnum.SpecialCalculationProcessType.UOM; //UOM Or PriceMarkup   
        private UltraGrid Callergrd = null;
        private Document Callerdoc = null;
        MSTItm objItm = null;

        private int itmQty = 0;                 //Used in UOM Cal function
        private string itmDes = string.Empty;   //Used in UOM Cal function
         
        //Properties
        public int ItmQty
        {
            get
            {
                return itmQty;
            }
            set
            {
                itmQty = value;
            }
        }
        public string ItmDes
        {
            get
            {
                return itmDes;
            }
            set
            {
                itmDes = value;
            }
        }

        bool popupDummyForCreatePO = false;

        //Initializeation
        public frmSpecialCalculation(bool pDummyForCreatePO)
        {
            InitializeComponent();
            popupDummyForCreatePO = pDummyForCreatePO;
        }//Completed
       /// <param name="objDoc"></param>
       /// <param name="calType">Sale or Purchase</param>
       /// <param name="processType">SpecialUOM or SpecialPriceMarkup</param>
       /// <param name="grdDetail"></param>
       /// <param name="setGrid">set value to Grid or not</param>
        public frmSpecialCalculation(Document objDoc, GEnum.SpecialCalculationType CalType, GEnum.SpecialCalculationProcessType ProcessType, UltraGrid GrdDetail)
        {
            //Call From ARDO,ARIV,ARSO,ARQO and DocUtil
            InitializeComponent();
            Callerdoc = objDoc;
            calType = CalType;
            processType = ProcessType;            
            Callergrd = GrdDetail;
        }//Completed
       
        //From Events
        private void frmSpecialCalculation_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (popupDummyForCreatePO)
                    return;
                //Set ContextMenu & Grid Setting                           
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0);
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);

                //Check if SpecialUOM show only UOM Calculation Tab
                if (processType == GEnum.SpecialCalculationProcessType.UOM)
                {
                    tabSpecialCalculation.Tabs[0].Visible = true;
                    tabSpecialCalculation.Tabs[0].Selected = true;
                    calUOM_Get();
                    PackUOMQty.Focus();
                }

                if (processType == GEnum.SpecialCalculationProcessType.PriceMarkup)
                {
                    MarkupType.SetValueTrigger(SysOptionUtility.GetInt("DefaultDocMarkupType"), false);

                    if (GFunc.NEInt(MarkupType.Value, 0) == 0)
                    {
                        MsgBox.Show("Default Markup Type has not been set up in System Option.");
                        this.DialogResult = DialogResult.Cancel;
                        return;
                    }

                    tabSpecialCalculation.Tabs[2].Visible = true;
                    tabSpecialCalculation.Tabs[2].Selected = true;

                    //Fill Data to Combo
                    GlobalUI.Combos_Fill(this, 0);

                    //Set the default value for combo using system options' default value
                    MarkupRate.SetValueTrigger(SysOptionUtility.GetDec("DocumentMinMarkup"), false);                  
                    MarkupDecimalPlace.SetValueTrigger(SysOptionUtility.GetInt("PriceDecPlace"), false);
                    MarkupRoundingMode.SetValueTrigger(SysOptionUtility.GetInt("PriceRoundMode"), false);
                    MarkupRate.Focus();
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
        private void frmSpecialCalculation_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, 0);
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
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {               
                if (processType == GEnum.SpecialCalculationProcessType.UOM)
                {
                    if (calUOM_Validation())
                    {
                        calUOM_Set();
                    }
                }
                else if (processType == GEnum.SpecialCalculationProcessType.PriceMarkup)
                {
                    if (calPriceMarkup_Validation())
                    {
                        calPriceMarkup_Set();
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }//Completed

        //Control Events
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
        private void Combo_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if ((sender as TAUtil.TAComboBox).Value == null)
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
        }//Completed

        //Functions
        private void calUOM_Get()
        {
            try
            {
                int itmKey = 0;
                //to get UOM of current Item
                if (Callergrd.ActiveRow != null)
                {
                    UltraGridRow row = Callergrd.ActiveRow;
                    itmKey = GFunc.NEInt(row.Cells["ItmKey"].Value, 0);
                    objItm = MSTItm.Get(itmKey);

                    if (calType == GEnum.SpecialCalculationType.Sale)
                    {
                        PackUOMID.Text = objItm.SaleUOM;
                        PackUOMRate.Text = GFunc.NEDec(objItm.SaleUOMRate, 0).ToString("#,##0.00##");
                        BaseUOMID.Text = objItm.BUOMID;
                    }
                    else if (calType == GEnum.SpecialCalculationType.Purchase)
                    {
                        PackUOMID.Text = objItm.PurchaseUOM;
                        PackUOMRate.Text = GFunc.NEDec(objItm.PurchaseUOMRate, 0).ToString("#,##0.00##");
                        BaseUOMID.Text = objItm.BUOMID;
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
        private void calUOM_Set()
        {
            try
            {
                decimal? UOMQty = 0;
                decimal? UOMRate = 0;
                decimal? itmQty = 0;
                string itmDes = string.Empty;

                //Calculate the Qty base on UOMQty and UOMRate
                UOMQty = GFunc.NEDec(PackUOMQty.Value, 0);
                UOMRate = GFunc.NEDec(PackUOMRate.Text, 0);
                itmQty = UOMQty * UOMRate;

                string UOMQtySt = UOMNumberPlace((decimal)UOMQty);
                string UOMRateSt = UOMNumberPlace((decimal)UOMRate);

                if (Callergrd.ActiveRow != null)
                {
                    UltraGridRow row = Callergrd.ActiveRow;
                    itmDes = row.Cells["ItmDes"].Value.ToString();

                    int optionUOM = SysOptionUtility.GetInt(GVar.SystemOption.OpID.UOMDesOption);
                    switch (optionUOM)//change Item Description depends on UOM option
                    {
                        case 10: itmDes = itmDes + " " + UOMQtySt + " " + PackUOMID.Text; break;
                        case 20: itmDes = itmDes + " " + UOMQtySt + " " + PackUOMID.Text + " of " + UOMRateSt + " " + BaseUOMID.Text; break;
                        case 30: itmDes = itmDes + " \n" + UOMQtySt + " " + PackUOMID.Text; break;
                        case 40: itmDes = itmDes + " \n" + UOMQtySt + " " + PackUOMID.Text + " of " + UOMRateSt + " " + BaseUOMID.Text; break;
                    }

                    //Update the Grid values 
                    ItmQty = (int)itmQty;
                    ItmDes = itmDes;
                    
                    this.DialogResult = DialogResult.OK;
                }
                this.DialogResult = DialogResult.Cancel;
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
        private string UOMNumberPlace(decimal Num)
        {
            //Use only for UOM Calculation
            //This function is use to make the Qty display more presentable
            //e.g for a Qty value(1.0) we should show as 1 w/o the ".0"
            try
            {
                string reval = "";
                reval = Num.ToString("#,###.####");
                decimal vTmpA = GFunc.NEDec(reval, 0);
                
                if (GFunc.RndC(vTmpA,0) == Num)
                {
                    reval = GFunc.NEInt(vTmpA, 0).ToString();
                }
                else
                {
                    reval = vTmpA.ToString();
                }

                return reval;
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
        private bool calUOM_Validation()
        {
            try
            {
                string msgID = string.Empty;
                if (!BaseUtility.Validation(out msgID, PackUOMQty.Value, "UOMQty", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null))
                {
                    MsgBox.Show(msgID);
                    return false;
                }
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
        private void calPriceMarkup_Set()
        {
            //Loop all detail rows
            //Update ItemMarkupRate
            //Recalculate row value for affected columns
            //Use ItmMarkupRate function in DocDetUtility to find the conditions and formula
            try
            {
                decimal vMarkupRate = GFunc.NEDec(MarkupRate.Value,0);
                int vMarkUpType = GFunc.NEInt(MarkupType.Value,0);
                int vMarkUpDec = GFunc.NEInt(MarkupDecimalPlace.Value,0);                
                int vMarkUpRoundMode = GFunc.NEInt(MarkupRoundingMode.Value,0);

                bool restore = false;
                DataTable dtBackup = (Callergrd.DataSource as DataTable).Copy();
                DataTable dtItm = Callergrd.DataSource as DataTable;

                //commented by Jane on 02-Dec-2013 - to avoid calling grdRow.Update() which fires gridBeforeRowUpdate and gridAfterRowUpdate EVENTS//
                //foreach (UltraGridRow grdRow in Callergrd.Rows)
                //{
                //    if (grdRow.IsUnmodifiedTemplateAddRow)
                //        continue;

                //    if (GFunc.NEBool(grdRow.Cells["ItmVendorPriceLock"].Value, false) == false)
                //    {
                //        grdRow.Cells["ItmVendorPriceRatio"].Value = vMarkupRate;
                //        if (DocDetUtil.ItmMarkupRate_Update(Callerdoc, grdRow, vMarkUpType, vMarkUpDec, vMarkUpRoundMode) == false)
                //        {
                //            restore = true;//Try to restore if something goes wrong
                //            break;
                //        }
                //    }
                //    grdRow.Update();
                //}
                foreach (DataRow dr in dtItm.Rows)
                {
                    //if (grdRow.IsUnmodifiedTemplateAddRow)
                    //    continue;

                    if (GFunc.NEBool(dr["ItmVendorPriceLock"], false) == false)
                    {
                        dr["ItmVendorPriceRatio"] = vMarkupRate;
                        if (DocDetUtil.ItmMarkupRate_Update(Callerdoc, dr, vMarkUpType, vMarkUpDec, vMarkUpRoundMode) == false)
                        {
                            restore = true;//Try to restore if something goes wrong
                            break;
                        }
                    }                    
                }
                if (restore)
                {
                    Callergrd.DataSource = dtBackup;
                    MsgBox.Show("Unable to perform updates");
                    this.DialogResult = DialogResult.Cancel;
                }
                else
                {
                    System.Collections.Hashtable htDetailGrd = new System.Collections.Hashtable();
                    htDetailGrd.Add(GEnum.Details.Doc_Itm, Callergrd);
                    DocComUtility.CalForm(Callerdoc, htDetailGrd, true, false);
                    this.DialogResult = DialogResult.OK;
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
        private bool calPriceMarkup_Validation()
        {
            try
            {
                string msgID = string.Empty;
                if (!BaseUtility.Validation(out msgID, MarkupRate.Value, "MarkupRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null))
                {
                    MsgBox.Show(msgID);
                    return false;
                }
                if (!BaseUtility.Validation(out msgID, MarkupType.Value, "MarkupType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null))
                {
                    MsgBox.Show(msgID);
                    return false;
                }
                if (!BaseUtility.Validation(out msgID, MarkupDecimalPlace.Value, "MarkupDecimalPlace", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null))
                {
                    MsgBox.Show(msgID);
                    return false;
                }
                if (!BaseUtility.Validation(out msgID, MarkupRoundingMode.Value, "MarkupRoundingMode", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null))
                {
                    MsgBox.Show(msgID);
                    return false;
                }
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
