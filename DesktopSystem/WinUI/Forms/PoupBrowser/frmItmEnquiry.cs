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
using System.Data.SqlClient;
using BOLib;
using TAUtil;

namespace WinUI
{
    public partial class frmItmEnquiry : Form
    {
        #region Local Variables
        private int itemKey = 0;
        private string ContextMenuSetting = string.Empty;
        private int monthCount = 6;
        private int yearCount = 3;
     
        //Event to be assigned by the caller detail form
        public GVar.PopupSelectedEvent RecordSelectedEvent = null;
        #endregion

        #region Initialize
        public frmItmEnquiry()
        {
            InitializeComponent();           
        }
        public frmItmEnquiry(int? ItmKey)
        {            
            itemKey = GFunc.NEInt(ItmKey,0);
            InitializeComponent();
        }
        #endregion

        //Form Events
        private void frmItmEnquiry_Load(object sender, EventArgs e)
        {
            try
            {
                this.MaximizeBox = false;
                this.StartPosition = FormStartPosition.CenterScreen;

                GlobalUI.Combos_Fill(this, 0);
                GlobalUI.FormGrids_Set(this,0, true, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0);

                // case (int)GEnum.SystemCode.Payment_Issue:
               
                Form_refresh(true);
                if (SECPermUtility.Perform("ItemViewCost", false) == false)
                {
                    UnitCost.PasswordChar = '*';
                    CostLatest.PasswordChar = '*';
                    CostAvg.PasswordChar = '*';
                    CostLanded.PasswordChar = '*';

                    string[] cols = { "Amt1", "Amt2", "Amt3" };

                    foreach (string col in cols)
                    {
                        if (tagrdActualHistory.DisplayLayout.Bands[0].Columns.Exists(col))
                        {
                            if (tagrdActualHistory.DisplayLayout.Bands[0].Columns[col].EditorComponent == null)
                                tagrdActualHistory.DisplayLayout.Bands[0].Columns[col].EditorComponent = new TAUtil.TANumericEditor();

                            ((TAUtil.TANumericEditor)tagrdActualHistory.DisplayLayout.Bands[0].Columns[col].EditorComponent).PasswordChar = '*';

                            tagrdActualHistory.DisplayLayout.Bands[0].Columns[col].CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FullEditorDisplay;
                            tagrdActualHistory.DisplayLayout.Bands[0].Columns[col].CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly;

                            tagrdActualHistory.DisplayLayout.Bands[0].Columns[col].ResetCellAppearance();
                        }
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
        }
        private void frmItmEnquiry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, 0);
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
        
        //Button Event
        private void btnPurchaseReport_Click(object sender, EventArgs e)
        {
            try
            {
                GlobalUI.PopupDisplay(GlobalUI.Form_Name.FRM_ITMHISTORYPURCHASE, null,true);
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
        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            try
            {
                GlobalUI.PopupDisplay(GlobalUI.Form_Name.FRM_ITMHISTORYSALE, null, true);
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
        private void btnSummaryReport_Click(object sender, EventArgs e)
        {
            try
            {
                GlobalUI.PopupDisplay(GlobalUI.Form_Name.FRM_ITMHISSUMMARY, null,true);
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
        private void btnAttachment_Click(object sender, EventArgs e)
        {
            try
            {
                if (GFunc.IsNEZ(itemKey))
                    return;

                MSTItmFactory objMstItmFactory = new MSTItmFactory(GEnum.InstanceMode.Normal);
                objMstItmFactory.GetReadOnly(itemKey, string.Empty);

                frmAttachment f = new frmAttachment(objMstItmFactory.ObjMSTItm.Attachments, (int)GEnum.SystemCode.Inventory, itemKey, -1, 0);
                f.ShowDialog(this);
                btnAttachment.Text = "(" + objMstItmFactory.ObjMSTItm.Attachments.Count + ")";
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

        //Controls data refresh - Dependant combo, TextEditorPopup, Grid - Combo List, Set/Clear TextEditorPop value and Grid binding source and filter
        private void Form_refresh(bool forceRefresh)
        {
            #region Declare Variables
            decimal vAvailable = 0;
            string vBrandID = string.Empty;
            string vBUOMID = string.Empty;
            string vCatID1 = string.Empty;
            string vCatID2 = string.Empty;
            string vCatID3 = string.Empty;
            string vCatID4 = string.Empty;
            string vCatID5 = string.Empty;
            string vColorID = string.Empty;
            decimal vCostAvg = 0;
            decimal vCostLanded = 0;
            DateTime? vCostLandedDate = null;
            decimal vCostLatest = 0;
            DateTime? vCostLatestDate = null;
            string vDefaultExpDate = null;
            string vINClass = string.Empty;
            string vIndustryPN = string.Empty;
            decimal vINHeight = 0;
            decimal vINLength = 0;
            string vINPacking = string.Empty;
            decimal vINVolume = 0;
            decimal vINWidth = 0;
            string vItmDes = string.Empty;
            string vItmID = string.Empty;
            string vItmRem = string.Empty;
            decimal vLeadTimeInDays = 0;
            string vLocation = string.Empty;
            string vModel = string.Empty;
            decimal vQtyMax = 0;
            decimal vQtyMin = 0;
            decimal vQtyReOrder = 0;
            decimal vQtyStock = 0;
            string vScaleID = string.Empty;
            string vScaleSize = string.Empty;
            decimal vUnitCost = 0;
            decimal vWeightGross = 0;
            decimal vWeightNet = 0;
            string vWeightUOMID = string.Empty;
            #endregion

            try
            {
                Reset_Values();
 
                MSTItm objMstItm = MSTItm.Get(itemKey);

                #region Assign To Variables
                if (objMstItm != null)
                {
                    vBrandID = objMstItm.BrandID;
                    vBUOMID = objMstItm.BUOMID;
                    vCatID1 = objMstItm.CatID1;
                    vCatID2 = objMstItm.CatID2;
                    vCatID3 = objMstItm.CatID3;
                    vCatID4 = objMstItm.CatID4;
                    vCatID5 = objMstItm.CatID5;
                    vColorID = objMstItm.ColorID;
                    vCostAvg = GFunc.NEDec(objMstItm.CostAvg, 0);
                    vCostLanded = GFunc.NEDec(objMstItm.CostLanded, 0);
                    vCostLandedDate = objMstItm.CostLandedDate;
                    vCostLatest = GFunc.NEDec(objMstItm.CostLatest, 0);
                    vCostLatestDate = objMstItm.CostLatestDate;
                    vDefaultExpDate = objMstItm.DefaultExpDate;
                    vINClass = objMstItm.INClass;
                    vIndustryPN = objMstItm.IndustryPN;
                    vINHeight = GFunc.NEDec(objMstItm.INHeight, 0);
                    vINLength = GFunc.NEDec(objMstItm.INLength, 0);
                    vINPacking = objMstItm.INPacking;
                    vINVolume = GFunc.NEDec(objMstItm.INVolume, 0);
                    vINWidth = GFunc.NEDec(objMstItm.INWidth, 0);
                    vItmDes = objMstItm.ItmDes;
                    vItmID = objMstItm.ItmID;
                    vItmRem = objMstItm.ItmRem;
                    vLeadTimeInDays = GFunc.NEDec(objMstItm.LeadTimeInDays, 0);
                    vModel = objMstItm.Model;
                    vQtyMax = GFunc.NEDec(objMstItm.QtyMax, 0);
                    vQtyMin = GFunc.NEDec(objMstItm.QtyMin, 0);
                    vQtyReOrder = GFunc.NEDec(objMstItm.QtyReOrder, 0);
                    vQtyStock = GFunc.NEDec(objMstItm.QtyStock, 0);
                    vScaleID = objMstItm.ScaleID;
                    vScaleSize = objMstItm.ScaleSize;
                    vWeightGross = GFunc.NEDec(objMstItm.WeightGross, 0);
                    vWeightNet = GFunc.NEDec(objMstItm.WeightNet, 0);
                    vWeightUOMID = objMstItm.WeightUOMID;
                    
                }
                #endregion
                
                #region Assign To Controls Value
                ItmDes.SetValueTrigger(vItmDes, false);
                ItmRem.SetValueTrigger(vItmRem, false);
                               

                UnitCost.SetValueTrigger(vUnitCost, false);                
                CostAvg.SetValueTrigger(vCostAvg, false);
                CostLatest.SetValueTrigger(vCostLatest, false);
                ObCost.SetValueTrigger(objMstItm.ObCost, false);//added by MTS on 11 Jan 2022

                QtyMin.SetValueTrigger(vQtyMin, false);
                QtyReOrder.SetValueTrigger(vQtyReOrder, false);
                WeightUOMID.SetValueTrigger(vWeightUOMID, false);
                WeightNet.SetValueTrigger(vWeightNet, false);
                WeightGross.SetValueTrigger(vWeightGross, false);
                QtyMax.SetValueTrigger(vQtyMax, false);

                INClass.SetValueTrigger(vINClass, false);
                Model.SetValueTrigger(vModel, false);
                BrandID.SetValueTrigger(vBrandID, false);
                CatID5.SetValueTrigger(vCatID5, false);
                CatID4.SetValueTrigger(vCatID4, false);
                CatID3.SetValueTrigger(vCatID3, false);
                CatID2.SetValueTrigger(vCatID2, false);
                CatID1.SetValueTrigger(vCatID1, false);
                INPacking.SetValueTrigger(vINPacking, false);

                ItmID.SetValueTrigger(vItmID, false);
                ItmRem.SetValueTrigger(vItmRem, false);
                ItmDes.SetValueTrigger(vItmDes, false);
                IndustryPN.SetValueTrigger(vIndustryPN, false);

                ColorID.SetValueTrigger(vColorID, false);
                ScaleID.SetValueTrigger(vScaleID, false);
                ScaleSize.SetValueTrigger(vScaleSize, false);
                QtyStock.SetValueTrigger(vQtyStock, false);
                LeadTimeInDays.SetValueTrigger(vLeadTimeInDays, false);
                BUOMID.SetValueTrigger(vBUOMID, false);
                CostLatestDate.SetValueTrigger(vCostLatestDate, false);
                CostLanded.SetValueTrigger(vCostLanded, false);
                CostLandedDate.SetValueTrigger(vCostLandedDate, false);
                DefaultExpDate.SetValueTrigger(vDefaultExpDate, false);
                INLength.SetValueTrigger(vINLength, false);
                INWidth.SetValueTrigger(vINWidth, false);
                INHeight.SetValueTrigger(vINHeight, false);
                INVolume.SetValueTrigger(vINVolume, false);
                #endregion

                //if (!forceRefresh)
                //    return;
                //Set Location Grid Data
                MSTItmDetLocs objItmDetLocs = MSTItmDetLocs.Get(itemKey);
                tagrdDetLocation.DataSource = objItmDetLocs;

                //Set Price Grid Data
                MSTItmDetPrice objItmDetPriceList = MSTItmDetPrice.Get(itemKey);
                LoadPriceList(objItmDetPriceList);
                PriceListGrid_format();

                //Set Batchs Grid Data
                MSTItmBatchs objItmBatchs = MSTItmBatchs.GetWtihBalance(itemKey);
                tagrdDetBatchs.DataSource = objItmBatchs;
                tagrdDetBatchs.DisplayLayout.Bands[0].Columns["BatchExpDate"].CellActivation = Activation.ActivateOnly;
                tagrdDetBatchs.DisplayLayout.Bands[0].Columns["BatchMfgDate"].CellActivation = Activation.ActivateOnly;
                
                LoadPending();
                LoadActual();

                //Calculate SO AND PO
                DataTable dt = tagrdPending.DataSource as DataTable;
                decimal SOSum = 0; decimal POSum = 0;
                foreach (DataColumn col in dt.Columns)
                {
                    if (dt.Rows[0][col].GetType() != typeof(decimal))
                        continue;
                    SOSum += GFunc.NEDec(dt.Rows[0][col], 0);
                    POSum += GFunc.NEDec(dt.Rows[1][col], 0);
                }

                SO.SetValueTrigger(SOSum, false);
                MFN.SetValueTrigger(0, false);
                PO.SetValueTrigger(POSum, false);
                SubstituteItmKey.SetValueTrigger(0, false);

                vAvailable = vQtyStock - (SOSum);
                Available.SetValueTrigger(vAvailable, false);
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
        private void ActualGrid_format()
        {
            try
            {
                tagrdActualHistory.DisplayLayout.Bands[0].Columns["OutstandingType"].Header.Caption = "";

                int j=1;
                for (int i = 1; i <= yearCount; i++)
                {
                    tagrdActualHistory.DisplayLayout.Bands[0].Columns[j].Header.Caption = "Qty " +(DateTime.Now.Year - (i - 1)).ToString();
                    tagrdActualHistory.DisplayLayout.Bands[0].Columns[j + 1].Header.Caption = "Amt " + (DateTime.Now.Year - (i - 1)).ToString();
                    j += 2;
                }

                for (int i = 1; i < tagrdActualHistory.DisplayLayout.Bands[0].Columns.Count; i++)
                {
                    if (tagrdActualHistory.DisplayLayout.Bands[0].Columns[i].Key.Contains("Amt"))
                        tagrdActualHistory.DisplayLayout.Bands[0].Columns[i].Format = "#,##0.00";   //Amount Format
                    else
                        tagrdActualHistory.DisplayLayout.Bands[0].Columns[i].Format = "#,##0.00##"; //Quantity Format
                }

                CommonGrids_format(tagrdActualHistory);
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
        private void PendingGrid_format()
        {
            try
            {
                tagrdPending.DisplayLayout.Bands[0].Columns["OutstandingType"].Header.Caption = "";
                tagrdPending.DisplayLayout.Bands[0].Columns["Month0"].Header.Caption = "<" + (DateTime.Now.ToString("MMMM") + DateTime.Now.Year.ToString()); //First Column
                tagrdPending.DisplayLayout.Bands[0].Columns["Month" + (monthCount + 1)].Header.Caption = ">" + DateTime.Now.AddMonths(monthCount - 1).ToString("MMMM") + " " + DateTime.Now.AddMonths(monthCount - 1).Year.ToString(); //Last Column

                for (int i = 1; i <= monthCount; i++)
                {
                    //Middle columns
                    tagrdPending.DisplayLayout.Bands[0].Columns["Month" + (i)].Header.Caption = DateTime.Now.AddMonths(i - 1).ToString("MMMM") + " " + DateTime.Now.AddMonths(i - 1).Year.ToString();
                }
                CommonGrids_format(tagrdPending);
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
        private void PriceListGrid_format()
        {
            try
            {
                for (int j = 0; j < tagrdPriceList.DisplayLayout.Bands[0].Columns.Count; j++)
                {
                    if (tagrdPriceList.DisplayLayout.Bands[0].Columns[j].Key.Equals("Ratio"))
                        tagrdPriceList.DisplayLayout.Bands[0].Columns[j].Format = "0.00%";
                    else if (tagrdPriceList.DisplayLayout.Bands[0].Columns[j].Key.Contains("Price"))
                        tagrdPriceList.DisplayLayout.Bands[0].Columns[j].Format = "#,##0.00####";
                    else
                        tagrdPriceList.DisplayLayout.Bands[0].Columns[j].Format = "##.##";
                }
                CommonGrids_format(tagrdPriceList);
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
        private void CommonGrids_format(UltraGrid grd)
        {
            try
            {
                //Set Appearence
                //Header Appearence
                Infragistics.Win.Appearance appearence_Header = new Infragistics.Win.Appearance();
                appearence_Header.AlphaLevel = ((short)(255));
                appearence_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.FontData.ItalicAsString = "True";
                appearence_Header.FontData.Name = "Calibri";
                appearence_Header.FontData.SizeInPoints = 10F;
                appearence_Header.ForeColor = System.Drawing.Color.Black;
                grd.DisplayLayout.Override.HeaderAppearance = appearence_Header;

                //Row Appearence
                Infragistics.Win.Appearance appearence_Row = new Infragistics.Win.Appearance();
                appearence_Row.FontData.Name = "Calibri";
                appearence_Row.FontData.SizeInPoints = 11F;
                appearence_Row.ForeColor = System.Drawing.Color.Black;
                appearence_Row.TextHAlignAsString = "LEFT";
                appearence_Row.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Row.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Override.RowAppearance = appearence_Row;

                //Cell Appearence
                Infragistics.Win.Appearance appearence_Cell = new Infragistics.Win.Appearance();
                appearence_Cell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Cell.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Override.CellAppearance = appearence_Cell;

                //Row Selector Appearence
                Infragistics.Win.Appearance appearence_RowSelector = new Infragistics.Win.Appearance();
                appearence_RowSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_RowSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Override.RowSelectorAppearance = appearence_RowSelector;

                //Create Dispaly Layout Appearence For Grid
                Infragistics.Win.Appearance appearence = new Infragistics.Win.Appearance();
                appearence.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Appearance = appearence;

                //Row Header Selector
                Infragistics.Win.Appearance appearence_RowHeaderSelector = new Infragistics.Win.Appearance();
                appearence_RowHeaderSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_RowHeaderSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Override.RowSelectorHeaderAppearance = appearence_RowHeaderSelector;

                grd.DisplayLayout.AutoFitStyle = AutoFitStyle.None;
                grd.TextRenderingMode = TextRenderingMode.GDI;
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
        private bool LoadPriceList(MSTItmDetPrice objItmDetPriceList)
        {
            try
            {
                //Cell
                Infragistics.Win.Appearance appearence_Cell = new Infragistics.Win.Appearance();
                appearence_Cell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Cell.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Cell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Cell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                
                tagrdPriceList.Rows[0].Cells["Ratio"].Value = 0;

                for (int i = 0; i < tagrdPriceList.Rows.Count; i++)
                {
                    string propertyName = string.Empty;
                    System.Reflection.PropertyInfo propertyInfo = null;
                    object obj = new object();
                    tagrdPriceList.Rows[i].Cells[0].Appearance = appearence_Cell;
                    if (i != 0)
                    {
                        propertyName = "Ratio" + (i).ToString();

                        if (!GFunc.IsNE(objItmDetPriceList))
                            obj = GFunc.GetPropertyValue(propertyName, objItmDetPriceList);
                        else obj = 0;

                        if (obj != null)
                            tagrdPriceList.Rows[i].Cells["Ratio"].Value = GFunc.NEDec(obj, 0);

                        propertyName = "Price" + (i).ToString("00") + "01";
                        propertyInfo = objItmDetPriceList.GetType().GetProperty(propertyName);
                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objItmDetPriceList, null);

                        if (!GFunc.IsNE(obj))
                        {
                            tagrdPriceList.Rows[i].Cells["Price01"].Value = GFunc.NEDec(obj, 0);                   
                        }

                        propertyName = "Price" + (i).ToString("00") + "02";
                        propertyInfo = objItmDetPriceList.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objItmDetPriceList, null);

                        if (!GFunc.IsNE(obj))
                        {
                            tagrdPriceList.Rows[i].Cells["Price02"].Value = GFunc.NEDec(obj, 0);
                          
                        }

                        propertyName = "Price" + (i).ToString("00") + "03";
                        propertyInfo = objItmDetPriceList.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objItmDetPriceList, null);

                        if (!GFunc.IsNE(obj))
                        {
                            tagrdPriceList.Rows[i].Cells["Price03"].Value = GFunc.NEDec(obj, 0);
                           
                        }

                        propertyName = "Price" + (i).ToString("00") + "04";
                        propertyInfo = objItmDetPriceList.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objItmDetPriceList, null);

                        if (!GFunc.IsNE(obj))
                        {
                            tagrdPriceList.Rows[i].Cells["Price04"].Value = GFunc.NEDec(obj, 0);
                            
                        }

                        propertyName = "Price" + (i).ToString("00") + "05";
                        propertyInfo = objItmDetPriceList.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objItmDetPriceList, null);

                        if (!GFunc.IsNE(obj))
                        {
                            tagrdPriceList.Rows[i].Cells["Price05"].Value = GFunc.NEDec(obj, 0);
                           
                        }

                        propertyName = "Price" + (i).ToString("00") + "06";
                        propertyInfo = objItmDetPriceList.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objItmDetPriceList, null);

                        if (!GFunc.IsNE(obj))
                        {
                            tagrdPriceList.Rows[i].Cells["Price06"].Value = GFunc.NEDec(obj, 0);
                            
                        }

                        propertyName = "Price" + (i).ToString("00") + "07";
                        propertyInfo = objItmDetPriceList.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objItmDetPriceList, null);

                        if (!GFunc.IsNE(obj))
                        {
                            tagrdPriceList.Rows[i].Cells["Price07"].Value = GFunc.NEDec(obj, 0);
                            
                        }
                        propertyName = "Price" + (i).ToString("00") + "08";
                        propertyInfo = objItmDetPriceList.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objItmDetPriceList, null);

                        if (!GFunc.IsNE(obj))
                        {
                            tagrdPriceList.Rows[i].Cells["Price08"].Value = GFunc.NEDec(obj, 0);
                        }
                        propertyName = "Price" + (i).ToString("00") + "09";
                        propertyInfo = objItmDetPriceList.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objItmDetPriceList, null);

                        if (!GFunc.IsNE(obj))
                        {
                            tagrdPriceList.Rows[i].Cells["Price09"].Value = GFunc.NEDec(obj, 0);
                        }

                        propertyName = "Price" + (i).ToString("00") + "10";
                        propertyInfo = objItmDetPriceList.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objItmDetPriceList, null);

                        if (!GFunc.IsNE(obj))
                        {
                            tagrdPriceList.Rows[i].Cells["Price10"].Value = GFunc.NEDec(obj, 0);
                        }
                        propertyName = "Price" + (i).ToString("00") + "11";
                        propertyInfo = objItmDetPriceList.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objItmDetPriceList, null);

                        if (!GFunc.IsNE(obj))
                        {
                            tagrdPriceList.Rows[i].Cells["Price11"].Value = GFunc.NEDec(obj, 0);
                        }
                        propertyName = "Price" + (i).ToString("00") + "12";
                        propertyInfo = objItmDetPriceList.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objItmDetPriceList, null);

                        if (!GFunc.IsNE(obj))
                        {
                            tagrdPriceList.Rows[i].Cells["Price12"].Value = GFunc.NEDec(obj, 0);
                        }

                        propertyName = "Price" + (i).ToString("00") + "13";
                        propertyInfo = objItmDetPriceList.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objItmDetPriceList, null);

                        if (!GFunc.IsNE(obj))
                        {
                            tagrdPriceList.Rows[i].Cells["Price13"].Value = GFunc.NEDec(obj, 0);
                        }
                        propertyName = "Price" + (i).ToString("00") + "14";
                        propertyInfo = objItmDetPriceList.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objItmDetPriceList, null);

                        if (!GFunc.IsNE(obj))
                        {
                            tagrdPriceList.Rows[i].Cells["Price14"].Value = GFunc.NEDec(obj, 0);
                        }

                        propertyName = "Price" + (i).ToString("00") + "15";
                        propertyInfo = objItmDetPriceList.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objItmDetPriceList, null);

                        if (!GFunc.IsNE(obj))
                        {
                            tagrdPriceList.Rows[i].Cells["Price15"].Value = GFunc.NEDec(obj, 0);
                        }

                        propertyName = "StandardCost" + (i).ToString();
                        propertyInfo = objItmDetPriceList.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objItmDetPriceList, null);

                        if (!GFunc.IsNE(obj))
                        {
                            tagrdPriceList.Rows[i].Cells["StdCost"].Value = GFunc.NEDec(obj, 0);
                        }
                    }

                    if (i < 15)
                    {
                        propertyName = "StandardPrice" + (i + 1).ToString();
                        propertyInfo = objItmDetPriceList.GetType().GetProperty(propertyName);

                        if (!GFunc.IsNE(propertyInfo))
                            obj = propertyInfo.GetValue(objItmDetPriceList, null);

                        if (!GFunc.IsNE(obj))
                            tagrdPriceList.Rows[0].Cells["Price" + (i + 1).ToString("00")].Value = GFunc.NEDec(obj, 0);
                    }

                    if (i > 4) continue;
                    propertyName = "QtyDisQty" + (i + 1).ToString();
                    propertyInfo = objItmDetPriceList.GetType().GetProperty(propertyName);

                    if (!GFunc.IsNE(propertyInfo))
                        obj = propertyInfo.GetValue(objItmDetPriceList, null);
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
            finally
            {
                this.Cursor = Cursors.Default;
               
            }
            return false;
        }//Completed
        private bool LoadPending()
        {
            int curPeriod=DateTime.Now.Year*100 + DateTime.Now.Month;

            List<SqlParameter> parmlist = new List<SqlParameter>();
            parmlist.Add(new SqlParameter("@ItmKey", Convert.ToInt32(itemKey)));
            parmlist.Add(new SqlParameter("@MonthCount", monthCount));
            parmlist.Add(new SqlParameter("@Option", Convert.ToInt32(0)));

            try
            {
                //Get Total Qty And QtyLink From ARSO,APPO
                DataTable dtPending = GFunc.ExecuteProc("ROEnquiry_Get", parmlist);

                //Get Summary Infor for Form Header
                decimal? previousSales = (from p in dtPending.AsEnumerable() where p.Field<int>("MonthRange")==0 select p.Field<decimal?>("Sale") ).Sum();
                decimal? previousPurchase = (from p in dtPending.AsEnumerable() where p.Field<int>("MonthRange") == 0 select p.Field<decimal?>("Purchase")).Sum();
                decimal? forwardSales = (from p in dtPending.AsEnumerable() where p.Field<int>("MonthRange") == 7 select p.Field<decimal?>("Sale")).Sum();
                decimal? forwardPurchase = (from p in dtPending.AsEnumerable() where p.Field<int>("MonthRange") == 7 select p.Field<decimal?>("Purchase")).Sum();

                //Bind to Grid for ARSO Qty
                //List<DataRow> dtTmp = dtPending.AsEnumerable().ToList().FindAll(p => (p.Field<int>("Period") >= curPeriod) && (p.Field<int>("Period") < curPeriod + monthCount));
                //for (int i = 1; i <= dtTmp.Count; i++)
                //{
                //    tagrdPending.Rows[0].Cells[i+1].Value = dtTmp[i - 1]["CurrentSales"];
                //    tagrdPending.Rows[1].Cells[i+1].Value = dtTmp[i - 1]["CurrentPurchase"];
                //}

                List<DataRow> dtTmp = dtPending.AsEnumerable().ToList().FindAll(p => (p.Field<int>("MonthRange") >= 1) && (p.Field<int>("MonthRange") <= 6));
                for (int i = 1; i <= dtTmp.Count; i++)
                {
                    tagrdPending.Rows[0].Cells[GFunc.NEInt(dtTmp[i - 1]["MonthRange"], null)+1].Value = dtTmp[i - 1]["Sale"];
                    tagrdPending.Rows[1].Cells[GFunc.NEInt(dtTmp[i - 1]["MonthRange"], null)+1].Value = dtTmp[i - 1]["Purchase"];                                       
                }

                //Set Previous Amount, Forcast Amount 
                tagrdPending.Rows[0].Cells["Month0"].Value = previousSales;
                tagrdPending.Rows[0].Cells["Month" + (monthCount + 1) ].Value = forwardSales; //MonthCount+2----- MonthCount + Previous Column(1)+ Forward Column(1)-1
                tagrdPending.Rows[1].Cells["Month0"].Value = previousPurchase;
                tagrdPending.Rows[1].Cells["Month" + (monthCount + 1)].Value = forwardPurchase;

                PendingGrid_format();
                parmlist.Clear();
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
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private string LoadActual()
        {
            string processOK = MsgID.Common.GetFail;
            int curYear = DateTime.Now.Year;

            List<SqlParameter> parmlist = new List<SqlParameter>();
            parmlist.Add(new SqlParameter("@ItmKey", Convert.ToInt32(itemKey)));
            parmlist.Add(new SqlParameter("@MonthCount", monthCount));
            parmlist.Add(new SqlParameter("@Option", Convert.ToInt32(1)));

            try
            {
                //Get Total Qty and Amount from ARIV,ARDN,ARCN/ APBL,APDN,APCN
                DataTable dtActual = GFunc.ExecuteProc("ROEnquiry_Get", parmlist);  
                List<DataRow> dtTmp = dtActual.AsEnumerable().ToList().FindAll(p => (Convert.ToInt16(p.Field<string>("pYear")) >= curYear - yearCount + 1) && (Convert.ToInt16(p.Field<string>("pYear")) <= curYear));

                int k = 1, diffYear = 0;
                
                for (int i = 0; i < dtTmp.Count; i++)
                {
                    diffYear = curYear - (GFunc.NEInt(dtTmp[i]["pYear"],0));

                    if (diffYear > 0)
                    {
                        k = diffYear;
                        k += 2;
                    }

                    tagrdActualHistory.Rows[0].Cells[k].Value = dtTmp[i][0];
                    tagrdActualHistory.Rows[0].Cells[k + 1].Value = dtTmp[i][1];

                    tagrdActualHistory.Rows[1].Cells[k].Value = dtTmp[i][2];
                    tagrdActualHistory.Rows[1].Cells[k + 1].Value = dtTmp[i][3];
                    k += 2;
                }

                parmlist.Clear();
                ActualGrid_format();
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

            //formatActualGrid();
            
            return processOK;
        }//Completed
        
        //Controls Events
        private void ItmID_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, "ItmID");

                if (DocHDRUtil.EditorButton_Popup(0, ItmID.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref itemKey, ref id, ref des))
                {
                    Form_refresh(true);
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
        private void ItmID_CustomUpdate(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                itemKey = GFunc.GetExistingRecKey(ItmID.Text, GEnum.SystemCode.Inventory, true, true);
                if (GFunc.IsNEZ(itemKey))
                {
                    ItmID_EditorButtonClick(sender, null);
                    if (GFunc.IsNEZ(itemKey))
                        Form_refresh(true);
                }
                else
                {
                    Form_refresh(true);
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
        private void IndustryPN_CustomUpdate(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (GFunc.IsNEZ(IndustryPN.Value))
                {
                    itemKey = 0;
                    Form_refresh(true);
                }
                else
                {
                    itemKey = (int)IndustryPN.Value;
                    Form_refresh(true);
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

        //Grid Events
        private void tagrdPriceList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                for (int i = 0; i < 16; i++)
                {
                    if (i == 0)
                    {
                        tagrdPriceList.Rows[i].Cells["PriceLabel"].Value = "Standard Price";
                        tagrdPriceList.Rows[i].Cells["Ratio"].Value = 0;
                    }
                    else
                    {
                        tagrdPriceList.Rows[i].Cells["PriceLabel"].Value = "Price " + i;
                    }
                }

                tagrdPriceList.DisplayLayout.Bands[0].Columns["PriceLabel"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                tagrdPriceList.DisplayLayout.Bands[0].Columns["PriceLabel"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
                tagrdPriceList.DisplayLayout.Bands[0].Columns["PriceLabel"].CellAppearance.ForeColorDisabled = System.Drawing.Color.Black;
                tagrdPriceList.DisplayLayout.Bands[0].Columns["PriceLabel"].CellAppearance.BackColor = System.Drawing.Color.AliceBlue;

                PriceListGrid_format();

                tagrdPriceList.DisplayLayout.Bands[0].Columns["PriceLabel"].Header.Caption = "";
                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price01"].Header.Caption =
                        GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr1));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price02"].Header.Caption =
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr2));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price03"].Header.Caption =
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr3));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price04"].Header.Caption =
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr4));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price05"].Header.Caption =
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr5));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price06"].Header.Caption =
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr6));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price07"].Header.Caption =
                     GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr7));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price08"].Header.Caption =
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr8));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price09"].Header.Caption =
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr9));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price10"].Header.Caption =
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr10));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price11"].Header.Caption =
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr11));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price12"].Header.Caption =
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr12));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price13"].Header.Caption =
                        GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr13));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price14"].Header.Caption =
                    GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr14));

                tagrdPriceList.DisplayLayout.Bands[0].Columns["Price15"].Header.Caption =
                       GFunc.GetCurrencyID(SysOptionUtility.GetInt(GVar.SystemOption.ItemPriceList_CurrencySetup.ItemPriceCurr15));

                tagrdPriceList.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.False;
                tagrdPriceList.DisplayLayout.Bands[0].Override.AllowAddNew = AllowAddNew.No;

                for (int i = 0; i < tagrdPriceList.DisplayLayout.Bands[0].Columns.Count; i++)
                {
                    tagrdPriceList.DisplayLayout.Bands[0].Columns[i].Header.Appearance.FontData.Bold = DefaultableBoolean.True;
                    tagrdPriceList.DisplayLayout.Bands[0].Columns[i].Header.Appearance.TextHAlign = HAlign.Center;
                    tagrdPriceList.DisplayLayout.Bands[0].Columns[i].Header.Appearance.FontData.Underline = DefaultableBoolean.True;
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
        
        //functions
        public void Reload(int? ItemKey, bool forceRefresh)
        {
            try
            {
                itemKey = GFunc.NEInt(ItemKey, 0);
                if (itemKey > 0)
                {
                    Form_refresh(forceRefresh);
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
        private void Reset_Values()
        {
            try
            {
                #region PriceList DataSource
                DataTable dt = new DataTable();
                dt.Columns.Add("PriceLabel", typeof(string));
                dt.Columns.Add("Ratio", typeof(decimal));
                dt.Columns.Add("Price01", typeof(decimal));
                dt.Columns.Add("Price02", typeof(decimal));
                dt.Columns.Add("Price03", typeof(decimal));
                dt.Columns.Add("Price04", typeof(decimal));
                dt.Columns.Add("Price05", typeof(decimal));
                dt.Columns.Add("Price06", typeof(decimal));
                dt.Columns.Add("Price07", typeof(decimal));
                dt.Columns.Add("Price08", typeof(decimal));
                dt.Columns.Add("Price09", typeof(decimal));
                dt.Columns.Add("Price10", typeof(decimal));
                dt.Columns.Add("Price11", typeof(decimal));
                dt.Columns.Add("Price12", typeof(decimal));
                dt.Columns.Add("Price13", typeof(decimal));
                dt.Columns.Add("Price14", typeof(decimal));
                dt.Columns.Add("Price15", typeof(decimal));
                dt.Columns.Add("StdCost", typeof(decimal));

                for (int i = 0; i < 16; i++)
                {
                    if (i == dt.Rows.Count)
                    {
                        DataRow dr = dt.NewRow();
                        foreach (DataColumn col in dt.Columns)
                        {
                            if (GFunc.CompareString(col.ColumnName, "PriceLabel"))
                                dr[col.ColumnName] = DBNull.Value;
                            else
                                dr[col.ColumnName] = 0;
                        }
                        dt.Rows.Add(dr);
                    }
                }
                tagrdPriceList.DataSource = dt;
                #endregion

                #region Pending History DataSource
                try
                {
                    DataTable dtPending = new DataTable();

                    dtPending.Columns.Add("OutstandingType", typeof(string));

                    for (int i = 0; i < monthCount + 2; i++)
                        dtPending.Columns.Add("Month" + i, typeof(decimal));

                    for (int i = 0; i < 2; i++)
                    {
                        if (i == dtPending.Rows.Count)
                        {
                            DataRow dr = dtPending.NewRow();
                            foreach (DataColumn col in dtPending.Columns)
                            {
                                if (GFunc.CompareString(col.ColumnName, "OutstandingType"))
                                    continue;
                                dr[col.ColumnName] = 0;
                            }
                            dtPending.Rows.Add(dr);
                        }

                    }
                    tagrdPending.DataSource = dtPending;
                    tagrdPending.DataBind();

                    tagrdPending.Rows[0].Cells[0].Value = "Sales";
                    tagrdPending.Rows[1].Cells[0].Value = "Purchase";
                }
                catch (Exception ex)
                {
                    MsgBox.Show(ex.Message);
                }

                #endregion

                #region Actual History DataSource
                DataTable dtActual = new DataTable();
                dtActual.Columns.Add("OutstandingType", typeof(string));

                for (int i = 1; i <= yearCount; i++)
                {
                    dtActual.Columns.Add("Qty" + i, typeof(decimal));
                    dtActual.Columns.Add("Amt" + i, typeof(decimal));
                }

                for (int i = 0; i < 2; i++)
                {
                    if (i == dtActual.Rows.Count)
                    {
                        DataRow dr = dtActual.NewRow();
                        foreach (DataColumn col in dtActual.Columns)
                        {
                            if (GFunc.CompareString(col.ColumnName, "OutstandingType"))
                                continue;
                            dr[col.ColumnName] = 0;
                        }
                        dtActual.Rows.Add(dr);
                    }

                }
                tagrdActualHistory.DataSource = dtActual;
                tagrdActualHistory.DataBind();

                try
                {
                    tagrdActualHistory.Rows[0].Cells[0].Value = "Sales";
                    tagrdActualHistory.Rows[1].Cells[0].Value = "Purchase";
                }
                catch (Exception ex)
                {
                    MsgBox.Show(ex.Message);
                }

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
